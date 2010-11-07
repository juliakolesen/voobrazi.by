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
using NopSolutions.NopCommerce.DataAccess.Localization;

namespace NopSolutions.NopCommerce.BusinessLogic.Localization
{
    /// <summary>
    /// Locale string resource manager
    /// </summary>
    public partial class LocaleStringResourceManager
    {
        #region Constants
        private const string LOCALSTRINGRESOURCES_ALL_KEY = "Nop.localestringresource.all-{0}";
        private const string LOCALSTRINGRESOURCES_PATTERN_KEY = "Nop.localestringresource.";
        #endregion

        #region Utilities
        private static LocaleStringResourceDictionary DBMapping(DBLocaleStringResourceCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            LocaleStringResourceDictionary dictionary = new LocaleStringResourceDictionary();
            foreach (DBLocaleStringResource dbItem in dbCollection)
            {
                LocaleStringResource item = DBMapping(dbItem);
                dictionary.Add(item.ResourceName.ToLowerInvariant(), item);
            }

            return dictionary;
        }

        private static LocaleStringResource DBMapping(DBLocaleStringResource dbItem)
        {
            if (dbItem == null)
                return null;

            LocaleStringResource item = new LocaleStringResource();
            item.LocaleStringResourceID = dbItem.LocaleStringResourceID;
            item.LanguageID = dbItem.LanguageID;
            item.ResourceName = dbItem.ResourceName;
            item.ResourceValue = dbItem.ResourceValue;

            return item;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Deletes a locale string resource
        /// </summary>
        /// <param name="LocaleStringResourceID">Locale string resource identifier</param>
        public static void DeleteLocaleStringResource(int LocaleStringResourceID)
        {
            DBProviderManager<DBLocaleStringResourceProvider>.Provider.DeleteLocaleStringResource(LocaleStringResourceID);
            if (LocaleStringResourceManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(LOCALSTRINGRESOURCES_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets a locale string resource
        /// </summary>
        /// <param name="LocaleStringResourceID">Locale string resource identifier</param>
        /// <returns>Locale string resource</returns>
        public static LocaleStringResource GetLocaleStringResourceByID(int LocaleStringResourceID)
        {
            if (LocaleStringResourceID == 0)
                return null;

            DBLocaleStringResource dbItem = DBProviderManager<DBLocaleStringResourceProvider>.Provider.GetLocaleStringResourceByID(LocaleStringResourceID);
            LocaleStringResource localeStringResource = DBMapping(dbItem);
            return localeStringResource;
        }

        /// <summary>
        /// Gets all locale string resources by language identifier
        /// </summary>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Locale string resource collection</returns>
        public static LocaleStringResourceDictionary GetAllResourcesByLanguageID(int LanguageID)
        {
            string key = string.Format(LOCALSTRINGRESOURCES_ALL_KEY, LanguageID);
            object obj2 = NopCache.Get(key);
            if (LocaleStringResourceManager.CacheEnabled && (obj2 != null))
            {
                return (LocaleStringResourceDictionary)obj2;
            }

            DBLocaleStringResourceCollection dbCollection = DBProviderManager<DBLocaleStringResourceProvider>.Provider.GetAllResourcesByLanguageID(LanguageID);
            LocaleStringResourceDictionary localeStringResourceDictionary = DBMapping(dbCollection);

            if (LocaleStringResourceManager.CacheEnabled)
            {
                NopCache.Max(key, localeStringResourceDictionary);
            }
            return localeStringResourceDictionary;
        }

        /// <summary>
        /// Inserts a locale string resource
        /// </summary>
        /// <param name="LanguageID">The language identifier</param>
        /// <param name="ResourceName">The resource name</param>
        /// <param name="ResourceValue">The resource value</param>
        /// <returns>Locale string resource</returns>
        public static LocaleStringResource InsertLocaleStringResource(int LanguageID, string ResourceName, string ResourceValue)
        {
            DBLocaleStringResource dbItem = DBProviderManager<DBLocaleStringResourceProvider>.Provider.InsertLocaleStringResource(LanguageID, ResourceName, ResourceValue);
            LocaleStringResource localeStringResource = DBMapping(dbItem);
            if (LocaleStringResourceManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(LOCALSTRINGRESOURCES_PATTERN_KEY);
            }
            return localeStringResource;
        }

        /// <summary>
        /// Updates the locale string resource
        /// </summary>
        /// <param name="LocaleStringResourceID">The locale string resource identifier</param>
        /// <param name="LanguageID">The language identifier</param>
        /// <param name="ResourceName">The resource name</param>
        /// <param name="ResourceValue">The resource value</param>
        /// <returns>Locale string resource</returns>
        public static LocaleStringResource UpdateLocaleStringResource(int LocaleStringResourceID, int LanguageID, string ResourceName, string ResourceValue)
        {
            DBLocaleStringResource dbItem = DBProviderManager<DBLocaleStringResourceProvider>.Provider.UpdateLocaleStringResource(LocaleStringResourceID, LanguageID, ResourceName, ResourceValue);
            LocaleStringResource localeStringResource = DBMapping(dbItem);
            if (LocaleStringResourceManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(LOCALSTRINGRESOURCES_PATTERN_KEY);
            }
            return localeStringResource;
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
                return SettingManager.GetSettingValueBoolean("Cache.LocaleStringResourceManager.CacheEnabled");
            }
        }
        #endregion
    }
}
