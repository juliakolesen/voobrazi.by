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
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using NopSolutions.NopCommerce.BusinessLogic.Caching;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Shipping;

namespace NopSolutions.NopCommerce.BusinessLogic.Shipping
{
    /// <summary>
    /// Shipping status manager
    /// </summary>
    public partial class ShippingStatusManager
    {
        #region Constants
        private const string SHIPPINGTATUSES_ALL_KEY = "Nop.shippingstatus.all";
        private const string SHIPPINGTATUSES_BY_ID_KEY = "Nop.shippingstatus.id-{0}";
        private const string SHIPPINGTATUSES_PATTERN_KEY = "Nop.shippingstatus.";
        #endregion

        #region Utilities
        private static ShippingStatusCollection DBMapping(DBShippingStatusCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            ShippingStatusCollection collection = new ShippingStatusCollection();
            foreach (DBShippingStatus dbItem in dbCollection)
            {
                ShippingStatus item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ShippingStatus DBMapping(DBShippingStatus dbItem)
        {
            if (dbItem == null)
                return null;

            ShippingStatus item = new ShippingStatus();
            item.ShippingStatusID = dbItem.ShippingStatusID;
            item.Name = dbItem.Name;

            return item;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Gets a shipping status full name
        /// </summary>
        /// <param name="ShippingStatusID">Shipping status identifier</param>
        /// <returns>Shipping status name</returns>
        public static string GetShippingStatusName(int ShippingStatusID)
        {
            ShippingStatus shippingStatus = GetShippingStatusByID(ShippingStatusID);
            if (shippingStatus != null)
                return shippingStatus.Name;
            else
                return ((ShippingStatusEnum)ShippingStatusID).ToString();
        }

        /// <summary>
        /// Gets a shipping status by ID
        /// </summary>
        /// <param name="ShippingStatusID">Shipping status identifier</param>
        /// <returns>Shipping status</returns>
        public static ShippingStatus GetShippingStatusByID(int ShippingStatusID)
        {
            if (ShippingStatusID == 0)
                return null;

            string key = string.Format(SHIPPINGTATUSES_BY_ID_KEY, ShippingStatusID);
            object obj2 = NopCache.Get(key);
            if (ShippingStatusManager.CacheEnabled && (obj2 != null))
            {
                return (ShippingStatus)obj2;
            }

            DBShippingStatus dbItem = DBProviderManager<DBShippingStatusProvider>.Provider.GetShippingStatusByID(ShippingStatusID);
            ShippingStatus shippingStatus = DBMapping(dbItem);

            if (ShippingStatusManager.CacheEnabled)
            {
                NopCache.Max(key, shippingStatus);
            }
            return shippingStatus;
        }

        /// <summary>
        /// Gets all shipping statuses
        /// </summary>
        /// <returns>Shipping status collection</returns>
        public static ShippingStatusCollection GetAllShippingStatuses()
        {
            string key = string.Format(SHIPPINGTATUSES_ALL_KEY);
            object obj2 = NopCache.Get(key);
            if (ShippingStatusManager.CacheEnabled && (obj2 != null))
            {
                return (ShippingStatusCollection)obj2;
            }

            DBShippingStatusCollection dbCollection = DBProviderManager<DBShippingStatusProvider>.Provider.GetAllShippingStatuses();
            ShippingStatusCollection shippingStatusCollection = DBMapping(dbCollection);
            
            if (ShippingStatusManager.CacheEnabled)
            {
                NopCache.Max(key, shippingStatusCollection);
            }
            return shippingStatusCollection;
        }

        #endregion

        #region Property
        /// <summary>
        /// Gets a value indicating whether cache is enabled
        /// </summary>
        public static bool CacheEnabled
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("Cache.ShippingStatusManager.CacheEnabled");
            }
        }
        #endregion
    }
}
