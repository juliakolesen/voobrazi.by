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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using NopSolutions.NopCommerce.BusinessLogic.Caching;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Configuration.Settings;

namespace NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings
{
    /// <summary>
    /// Setting manager
    /// </summary>
    public partial class SettingManager
    {
        #region Constants
        private const string SETTINGS_ALL_KEY = "Nop.setting.all";
        #endregion

        #region Utilities
        private static SettingCollection DBMapping(DBSettingCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            SettingCollection collection = new SettingCollection();
            foreach (DBSetting dbItem in dbCollection)
            {
                Setting item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static Setting DBMapping(DBSetting dbItem)
        {
            if (dbItem == null)
                return null;

            Setting item = new Setting();
            item.SettingID = dbItem.SettingID;
            item.Name = dbItem.Name;
            item.Value = dbItem.Value;
            item.Description = dbItem.Description;

            return item;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets a setting
        /// </summary>
        /// <param name="SettingID">Setting identifer</param>
        /// <returns>Setting</returns>
        public static Setting GetSettingByID(int SettingID)
        {
            if (SettingID == 0)
                return null;

            DBSetting dbItem = DBProviderManager<DBSettingProvider>.Provider.GetSettingByID(SettingID);
            Setting setting = DBMapping(dbItem);
            return setting;
        }

        /// <summary>
        /// Deletes a setting
        /// </summary>
        /// <param name="SettingID">Setting identifer</param>
        public static void DeleteSetting(int SettingID)
        {
            DBProviderManager<DBSettingProvider>.Provider.DeleteSetting(SettingID);

            if (SettingManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(SETTINGS_ALL_KEY);
            }
        }

        /// <summary>
        /// Gets all settings
        /// </summary>
        /// <returns>Setting collection</returns>
        public static SettingCollection GetAllSettings()
        {
            string key = SETTINGS_ALL_KEY;
            object obj2 = NopCache.Get(key);
            if (SettingManager.CacheEnabled && (obj2 != null))
            {
                return (SettingCollection)obj2;
            }

            DBSettingCollection dbCollection = DBProviderManager<DBSettingProvider>.Provider.GetAllSettings();
            SettingCollection settingCollection = DBMapping(dbCollection);

            if (SettingManager.CacheEnabled)
            {
                NopCache.Max(key, settingCollection);
            }
            return settingCollection;
        }

         /// <summary>
        /// Inserts/updates a param
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="Value">The value</param>
        /// <returns>Setting</returns>
        public static Setting SetParam(string Name, string Value)
        {
            Setting setting = GetSettingByName(Name);
            if (setting != null)
                return SetParam(Name, Value, setting.Description);
            else
                return SetParam(Name, Value, string.Empty);
        }

        /// <summary>
        /// Inserts/updates a param
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="Value">The value</param>
        /// <param name="Description">The description</param>
        /// <returns>Setting</returns>
        public static Setting SetParam(string Name, string Value, string Description)
        {
            Setting setting = GetSettingByName(Name);
            if (setting != null)
            {
                if (setting.Name != Name || setting.Value != Value || setting.Description != Description)
                    setting = UpdateSetting(setting.SettingID, Name, Value, Description);
            }
            else
                setting = AddSetting(Name, Value, Description);

            return setting;
        }

        /// <summary>
        /// Inserts/updates a param in US locale
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="Value">The value</param>
        /// <returns>Setting</returns>
        public static Setting SetParamNative(string Name, decimal Value)
        {
            return SetParamNative(Name, Value, string.Empty);
        }

        /// <summary>
        /// Inserts/updates a param in US locale
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="Value">The value</param>
        /// <param name="Description">The description</param>
        /// <returns>Setting</returns>
        public static Setting SetParamNative(string Name, decimal Value, string Description)
        {
            string valueStr = Value.ToString(new CultureInfo("en-US"));
            return SetParam(Name, valueStr, Description);
        }

        /// <summary>
        /// Adds a setting
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="Value">The value</param>
        /// <param name="Description">The description</param>
        /// <returns>Setting</returns>
        public static Setting AddSetting(string Name, string Value, string Description)
        {
            DBSetting dbItem = DBProviderManager<DBSettingProvider>.Provider.AddSetting(Name, Value, Description);
            Setting setting = DBMapping(dbItem);

            if (SettingManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(SETTINGS_ALL_KEY);
            }

            return setting;
        }

        /// <summary>
        /// Updates a setting
        /// </summary>
        /// <param name="SettingID">Setting identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="Value">The value</param>
        /// <param name="Description">The description</param>
        /// <returns>Setting</returns>
        public static Setting UpdateSetting(int SettingID, string Name, string Value, string Description)
        {
            DBSetting dbItem = DBProviderManager<DBSettingProvider>.Provider.UpdateSetting(SettingID, Name, Value, Description);
            Setting setting = DBMapping(dbItem);
            if (SettingManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(SETTINGS_ALL_KEY);
            }

            return setting;
        }

        /// <summary>
        /// Gets a boolean value of a setting
        /// </summary>
        /// <param name="Name">The setting name</param>
        /// <returns>The setting value</returns>
        public static bool GetSettingValueBoolean(string Name)
        {
            return GetSettingValueBoolean(Name, false);
        }

        /// <summary>
        /// Gets a boolean value of a setting
        /// </summary>
        /// <param name="Name">The setting name</param>
        /// <param name="DefaultValue">The default value</param>
        /// <returns>The setting value</returns>
        public static bool GetSettingValueBoolean(string Name, bool DefaultValue)
        {
            string value = GetSettingValue(Name);
            if (!String.IsNullOrEmpty(value))
            {
                return bool.Parse(value);
            }
            return DefaultValue;
        }

        /// <summary>
        /// Gets an integer value of a setting
        /// </summary>
        /// <param name="Name">The setting name</param>
        /// <returns>The setting value</returns>
        public static int GetSettingValueInteger(string Name)
        {
            return GetSettingValueInteger(Name, 0);
        }

        /// <summary>
        /// Gets an integer value of a setting
        /// </summary>
        /// <param name="Name">The setting name</param>
        /// <param name="DefaultValue">The default value</param>
        /// <returns>The setting value</returns>
        public static int GetSettingValueInteger(string Name, int DefaultValue)
        {
            string value = GetSettingValue(Name);
            if (!String.IsNullOrEmpty(value))
            {
                return int.Parse(value);
            }
            return DefaultValue;
        }

        /// <summary>
        /// Gets a decimal value of a setting in US locale
        /// </summary>
        /// <param name="Name">The setting name</param>
        /// <returns>The setting value</returns>
        public static decimal GetSettingValueDecimalNative(string Name)
        {
            return GetSettingValueDecimalNative(Name, decimal.Zero);
        }

        /// <summary>
        /// Gets a decimal value of a setting in US locale
        /// </summary>
        /// <param name="Name">The setting name</param>
        /// <param name="DefaultValue">The default value</param>
        /// <returns>The setting value</returns>
        public static decimal GetSettingValueDecimalNative(string Name, decimal DefaultValue)
        {
            string value = GetSettingValue(Name);
            if (!String.IsNullOrEmpty(value))
            {
                return decimal.Parse(value, new CultureInfo("en-US"));
            }
            return DefaultValue;
        }

        /// <summary>
        /// Gets a setting value
        /// </summary>
        /// <param name="Name">The setting name</param>
        /// <returns>The setting value</returns>
        public static string GetSettingValue(string Name)
        {
            Setting setting = GetSettingByName(Name);
            if (setting != null)
                return setting.Value;
            return string.Empty;
        }

        /// <summary>
        /// Gets a setting value
        /// </summary>
        /// <param name="Name">The setting name</param>
        /// <param name="DefaultValue">The default value</param>
        /// <returns>The setting value</returns>
        public static string GetSettingValue(string Name, string DefaultValue)
        {
            string value = GetSettingValue(Name);
            if (!String.IsNullOrEmpty(value))
            {
                return value;
            }
            return DefaultValue;
        }

        /// <summary>
        /// Get a setting by name
        /// </summary>
        /// <param name="Name">The setting name</param>
        /// <returns>Setting instance</returns>
        public static Setting GetSettingByName(string Name)
        {
            if (String.IsNullOrEmpty(Name))
                return null;

            SettingCollection settingCollection = GetAllSettings();
            foreach (Setting setting in settingCollection)
                if (setting.Name.ToLower() == Name.ToLower())
                    return setting;
            return null;
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
                return true;
            }
        }

        /// <summary>
        /// Gets or sets a store name
        /// </summary>
        public static string StoreName
        {
            get
            {
                string storeName = SettingManager.GetSettingValue("Common.StoreName");
                return storeName;
            }
            set
            {
                SettingManager.SetParam("Common.StoreName", value);
            }
        }

        /// <summary>
        /// Gets or sets a store URL (ending with "/")
        /// </summary>
        public static string StoreURL
        {
            get
            {
                string storeURL = SettingManager.GetSettingValue("Common.StoreURL");
                if (!storeURL.EndsWith("/"))
                    storeURL += "/";
                return storeURL;
            }
            set
            {
                SettingManager.SetParam("Common.StoreURL", value);
            }
        }

        #endregion
    }
}
