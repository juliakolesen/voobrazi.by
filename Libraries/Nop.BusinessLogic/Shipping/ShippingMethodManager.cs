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
    /// Shipping method manager
    /// </summary>
    public partial class ShippingMethodManager
    {
        #region Constants
        private const string SHIPPINGMETHODS_ALL_KEY = "Nop.shippingMethod.all";
        private const string SHIPPINGMETHODS_BY_ID_KEY = "Nop.shippingMethod.id-{0}";
        private const string SHIPPINGMETHODS_PATTERN_KEY = "Nop.shippingMethod.";
        #endregion

        #region Utilities
        private static ShippingMethodCollection DBMapping(DBShippingMethodCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            ShippingMethodCollection collection = new ShippingMethodCollection();
            foreach (DBShippingMethod dbItem in dbCollection)
            {
                ShippingMethod item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ShippingMethod DBMapping(DBShippingMethod dbItem)
        {
            if (dbItem == null)
                return null;

            ShippingMethod item = new ShippingMethod();
            item.ShippingMethodID = dbItem.ShippingMethodID;
            item.Name = dbItem.Name;
            item.Description = dbItem.Description;
            item.DisplayOrder = dbItem.DisplayOrder;

            return item;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Deletes a shipping method
        /// </summary>
        /// <param name="ShippingMethodID">The shipping method identifier</param>
        public static void DeleteShippingMethod(int ShippingMethodID)
        {
            DBProviderManager<DBShippingMethodProvider>.Provider.DeleteShippingMethod(ShippingMethodID);
            if (ShippingMethodManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(SHIPPINGMETHODS_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets a shipping method
        /// </summary>
        /// <param name="ShippingMethodID">The shipping method identifier</param>
        /// <returns>Shipping method</returns>
        public static ShippingMethod GetShippingMethodByID(int ShippingMethodID)
        {
            if (ShippingMethodID == 0)
                return null;

            string key = string.Format(SHIPPINGMETHODS_BY_ID_KEY, ShippingMethodID);
            object obj2 = NopCache.Get(key);
            if (ShippingMethodManager.CacheEnabled && (obj2 != null))
            {
                return (ShippingMethod)obj2;
            }

            DBShippingMethod dbItem = DBProviderManager<DBShippingMethodProvider>.Provider.GetShippingMethodByID(ShippingMethodID);
            ShippingMethod shippingMethod = DBMapping(dbItem);

            if (ShippingMethodManager.CacheEnabled)
            {
                NopCache.Max(key, shippingMethod);
            }
            return shippingMethod;
        }

        /// <summary>
        /// Gets all shipping methods
        /// </summary>
        /// <returns>Shipping method collection</returns>
        public static ShippingMethodCollection GetAllShippingMethods()
        {
            string key = string.Format(SHIPPINGMETHODS_ALL_KEY);
            object obj2 = NopCache.Get(key);
            if (ShippingMethodManager.CacheEnabled && (obj2 != null))
            {
                return (ShippingMethodCollection)obj2;
            }

            DBShippingMethodCollection dbCollection = DBProviderManager<DBShippingMethodProvider>.Provider.GetAllShippingMethods();
            ShippingMethodCollection shippingMethods = DBMapping(dbCollection);

            if (ShippingMethodManager.CacheEnabled)
            {
                NopCache.Max(key, shippingMethods);
            }
            return shippingMethods;
        }

        /// <summary>
        /// Inserts a shipping method
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="Description">The description</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Shipping method</returns>
        public static ShippingMethod InsertShippingMethod(string Name, string Description, int DisplayOrder)
        {
            DBShippingMethod dbItem = DBProviderManager<DBShippingMethodProvider>.Provider.InsertShippingMethod(Name, 
                Description, DisplayOrder);
            ShippingMethod shippingMethod = DBMapping(dbItem);

            if (ShippingMethodManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(SHIPPINGMETHODS_PATTERN_KEY);
            }
            return shippingMethod;
        }

        /// <summary>
        /// Updates the shipping method
        /// </summary>
        /// <param name="ShippingMethodID">The shipping method identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="Description">The description</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Shipping method</returns>
        public static ShippingMethod UpdateShippingMethod(int ShippingMethodID, string Name,
            string Description, int DisplayOrder)
        {
            DBShippingMethod dbItem = DBProviderManager<DBShippingMethodProvider>.Provider.UpdateShippingMethod(ShippingMethodID, 
                Name, Description, DisplayOrder);
            ShippingMethod shippingMethod = DBMapping(dbItem);

            if (ShippingMethodManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(SHIPPINGMETHODS_PATTERN_KEY);
            }

            return shippingMethod;
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
                return SettingManager.GetSettingValueBoolean("Cache.ShippingMethodManager.CacheEnabled");
            }
        }
        #endregion
    }
}
