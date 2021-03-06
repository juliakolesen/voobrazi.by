//------------------------------------------------------------------------------
// The contents of this file are subject to the nopCommerce Public License Version 1.0 ("License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at  http://www.nopCommerce.com/License.aspx. 
// 
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. 
// See the License for the specific language governing rights and limitations under the License.
// 
// The Original Code is nopCommerce.
// The Initial Developer of the Original Code is NopSolutions.
// All Rights Reserved.
// 
// Contributor(s): _______. 
//------------------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;


namespace NopSolutions.NopCommerce.BusinessLogic.Profile
{
    /// <summary>
    /// Represents a datetime helper
    /// </summary>
    public partial class DateTimeHelper
    {
        #region Methods
        /// <summary>
        /// Retrieves a System.TimeZoneInfo object from the registry based on its identifier.
        /// </summary>
        /// <param name="id">The time zone identifier, which corresponds to the System.TimeZoneInfo.Id property.</param>
        /// <returns>A System.TimeZoneInfo object whose identifier is the value of the id parameter.</returns>
        public static TimeZoneInfo FindTimeZoneById(string id)
        {
            return TimeZoneInfo.FindSystemTimeZoneById(id);
        }

        /// <summary>
        /// Returns a sorted collection of all the time zones
        /// </summary>
        /// <returns>A read-only collection of System.TimeZoneInfo objects.</returns>
        public static ReadOnlyCollection<TimeZoneInfo> GetSystemTimeZones()
        {
            return TimeZoneInfo.GetSystemTimeZones();
        }

        /// <summary>
        /// Converts the date and time to current user date and time
        /// </summary>
        /// <param name="dt">The date and time (respesents local system time or UTC time) to convert.</param>
        /// <returns>A DateTime value that represents time that corresponds to the dateTime parameter in customer time zone.</returns>
        public static DateTime ConvertToUserTime(DateTime dt)
        {
            TimeZoneInfo currentUserTimeZoneInfo = DateTimeHelper.CurrentTimeZone;
            return TimeZoneInfo.ConvertTime(dt, currentUserTimeZoneInfo);
        }

        /// <summary>
        /// Converts the date and time to current user date and time
        /// </summary>
        /// <param name="dt">The date and time to convert.</param>
        /// <param name="sourceTimeZone">The time zone of dateTime.</param>
        /// <returns>A DateTime value that represents time that corresponds to the dateTime parameter in customer time zone.</returns>
        public static DateTime ConvertToUserTime(DateTime dt, TimeZoneInfo sourceTimeZone)
        {
            TimeZoneInfo currentUserTimeZoneInfo = DateTimeHelper.CurrentTimeZone;
            return TimeZoneInfo.ConvertTime(dt, sourceTimeZone, currentUserTimeZoneInfo);
        }

        /// <summary>
        /// Converts the date and time to Coordinated Universal Time (UTC)
        /// </summary>
        /// <param name="dt">The date and time (respesents local system time or UTC time) to convert.</param>
        /// <returns>A DateTime value that represents the Coordinated Universal Time (UTC) that corresponds to the dateTime parameter. The DateTime value's Kind property is always set to DateTimeKind.Utc.</returns>
        public static DateTime ConvertToUtcTime(DateTime dt)
        {
            return TimeZoneInfo.ConvertTimeToUtc(dt);
        }

        /// <summary>
        /// Converts the date and time to Coordinated Universal Time (UTC)
        /// </summary>
        /// <param name="dt">The date and time to convert.</param>
        /// <param name="sourceTimeZone">The time zone of dateTime.</param>
        /// <returns>A DateTime value that represents the Coordinated Universal Time (UTC) that corresponds to the dateTime parameter. The DateTime value's Kind property is always set to DateTimeKind.Utc.</returns>
        public static DateTime ConvertToUtcTime(DateTime dt, TimeZoneInfo sourceTimeZone)
        {
            return TimeZoneInfo.ConvertTimeToUtc(dt, sourceTimeZone);
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a default store time zone
        /// </summary>
        public static TimeZoneInfo DefaultStoreTimeZone
        {
            get
            {
                string defaultTimeZoneID = SettingManager.GetSettingValue("Common.DefaultStoreTimeZoneID");
                TimeZoneInfo timeZoneInfo = null;
                try
                {
                    timeZoneInfo = DateTimeHelper.FindTimeZoneById(defaultTimeZoneID);
                }
                catch (Exception exc)
                {
                    Debug.Write(exc.ToString());
                }

                if (timeZoneInfo == null)
                    timeZoneInfo = TimeZoneInfo.Local;

                return timeZoneInfo;
            }
            set
            {
                string defaultTimeZoneID = string.Empty;
                if (value != null)
                {
                    defaultTimeZoneID = value.Id;
                }

                SettingManager.SetParam("Common.DefaultStoreTimeZoneID", defaultTimeZoneID);
            }
        }

        /// <summary>
        /// Gets or sets the current user time zone
        /// </summary>
        public static TimeZoneInfo CurrentTimeZone
        {
            get
            {
                //registered user only
                TimeZoneInfo timeZoneInfo = null;
                if (DateTimeHelper.AllowCustomersToSetTimeZone)
                {
                    string timeZoneID = string.Empty;
                    Customer customer = NopContext.Current.User;
                    if (customer != null)
                        timeZoneID = customer.TimeZoneID;

                    try
                    {
                        timeZoneInfo = DateTimeHelper.FindTimeZoneById(timeZoneID);
                    }
                    catch (Exception exc)
                    {
                        Debug.Write(exc.ToString());
                    }
                }

                if (timeZoneInfo == null)
                    timeZoneInfo = DateTimeHelper.DefaultStoreTimeZone;

                return timeZoneInfo;
            }
            set
            {
                //registered user only

                if (!DateTimeHelper.AllowCustomersToSetTimeZone)
                    return;

                string timeZoneID = string.Empty;
                if (value != null)
                {
                    timeZoneID = value.Id;
                }

                Customer customer = NopContext.Current.User;
                if (customer != null)
                {
                    customer = CustomerManager.SetTimeZoneID(customer.CustomerID, timeZoneID);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether customers are allowed to select theirs time zone
        /// </summary>
        public static bool AllowCustomersToSetTimeZone
        {
            get
            {
                bool allowCustomersToSetTimeZone = SettingManager.GetSettingValueBoolean("Common.AllowCustomersToSetTimeZone");
                return allowCustomersToSetTimeZone;
            }
            set
            {
                SettingManager.SetParam("Common.AllowCustomersToSetTimeZone", value.ToString());
            }
        }

        #endregion
    }
}