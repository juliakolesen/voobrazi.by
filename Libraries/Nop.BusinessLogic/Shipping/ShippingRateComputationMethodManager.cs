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
    /// Shipping rate computation method manager
    /// </summary>
    public partial class ShippingRateComputationMethodManager
    {
        #region Constants
        private const string SHIPPINGRATECOMPUTATIONMETHODS_ALL_KEY = "Nop.shippingratecomputationmethod.all";
        private const string SHIPPINGRATECOMPUTATIONMETHODS_BY_ID_KEY = "Nop.shippingratecomputationmethod.id-{0}";
        private const string SHIPPINGRATECOMPUTATIONMETHODS_PATTERN_KEY = "Nop.shippingratecomputationmethod.";
        #endregion

        #region Utilities
        private static ShippingRateComputationMethodCollection DBMapping(DBShippingRateComputationMethodCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            ShippingRateComputationMethodCollection collection = new ShippingRateComputationMethodCollection();
            foreach (DBShippingRateComputationMethod dbItem in dbCollection)
            {
                ShippingRateComputationMethod item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ShippingRateComputationMethod DBMapping(DBShippingRateComputationMethod dbItem)
        {
            if (dbItem == null)
                return null;

            ShippingRateComputationMethod item = new ShippingRateComputationMethod();
            item.ShippingRateComputationMethodID = dbItem.ShippingRateComputationMethodID;
            item.Name = dbItem.Name;
            item.Description = dbItem.Description;
            item.ConfigureTemplatePath = dbItem.ConfigureTemplatePath;
            item.ClassName = dbItem.ClassName;
            item.DisplayOrder = dbItem.DisplayOrder;

            return item;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Deletes a shipping rate computation method
        /// </summary>
        /// <param name="ShippingRateComputationMethodID">Shipping rate computation method identifier</param>
        public static void DeleteShippingRateComputationMethod(int ShippingRateComputationMethodID)
        {
            DBProviderManager<DBShippingRateComputationMethodProvider>.Provider.DeleteShippingRateComputationMethod(ShippingRateComputationMethodID);
            if (ShippingRateComputationMethodManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(SHIPPINGRATECOMPUTATIONMETHODS_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets a shipping rate computation method
        /// </summary>
        /// <param name="ShippingRateComputationMethodID">Shipping rate computation method identifier</param>
        /// <returns>Shipping rate computation method</returns>
        public static ShippingRateComputationMethod GetShippingRateComputationMethodByID(int ShippingRateComputationMethodID)
        {
            if (ShippingRateComputationMethodID == 0)
                return null;

            string key = string.Format(SHIPPINGRATECOMPUTATIONMETHODS_BY_ID_KEY, ShippingRateComputationMethodID);
            object obj2 = NopCache.Get(key);
            if (ShippingRateComputationMethodManager.CacheEnabled && (obj2 != null))
            {
                return (ShippingRateComputationMethod)obj2;
            }

            DBShippingRateComputationMethod dbItem = DBProviderManager<DBShippingRateComputationMethodProvider>.Provider.GetShippingRateComputationMethodByID(ShippingRateComputationMethodID);
            ShippingRateComputationMethod shippingRateComputationMethod = DBMapping(dbItem);

            if (ShippingRateComputationMethodManager.CacheEnabled)
            {
                NopCache.Max(key, shippingRateComputationMethod);
            }
            return shippingRateComputationMethod;
        }

        /// <summary>
        /// Gets all shipping rate computation methods
        /// </summary>
        /// <returns>Shipping rate computation method collection</returns>
        public static ShippingRateComputationMethodCollection GetAllShippingRateComputationMethods()
        {
            string key = string.Format(SHIPPINGRATECOMPUTATIONMETHODS_ALL_KEY);
            object obj2 = NopCache.Get(key);
            if (ShippingRateComputationMethodManager.CacheEnabled && (obj2 != null))
            {
                return (ShippingRateComputationMethodCollection)obj2;
            }

            DBShippingRateComputationMethodCollection dbCollection = DBProviderManager<DBShippingRateComputationMethodProvider>.Provider.GetAllShippingRateComputationMethods();
            ShippingRateComputationMethodCollection shippingRateComputationMethods = DBMapping(dbCollection);

            if (ShippingRateComputationMethodManager.CacheEnabled)
            {
                NopCache.Max(key, shippingRateComputationMethods);
            }
            return shippingRateComputationMethods;
        }

        /// <summary>
        /// Inserts a shipping rate computation method
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="Description">The description</param>
        /// <param name="ConfigureTemplatePath">The configure template path</param>
        /// <param name="ClassName">The class name</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Shipping rate computation method</returns>
        public static ShippingRateComputationMethod InsertShippingRateComputationMethod(string Name, string Description,
           string ConfigureTemplatePath, string ClassName, int DisplayOrder)
        {
            DBShippingRateComputationMethod dbItem = DBProviderManager<DBShippingRateComputationMethodProvider>.Provider.InsertShippingRateComputationMethod(Name, Description,
           ConfigureTemplatePath, ClassName, DisplayOrder);
            ShippingRateComputationMethod shippingRateComputationMethod = DBMapping(dbItem);

            if (ShippingRateComputationMethodManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(SHIPPINGRATECOMPUTATIONMETHODS_PATTERN_KEY);
            }
            return shippingRateComputationMethod;
        }

        /// <summary>
        /// Updates the shipping rate computation method
        /// </summary>
        /// <param name="ShippingRateComputationMethodID">The shipping rate computation method identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="Description">The description</param>
        /// <param name="ConfigureTemplatePath">The configure template path</param>
        /// <param name="ClassName">The class name</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Shipping rate computation method</returns>
        public static ShippingRateComputationMethod UpdateShippingRateComputationMethod(int ShippingRateComputationMethodID, string Name, string Description,
           string ConfigureTemplatePath, string ClassName, int DisplayOrder)
        {
            DBShippingRateComputationMethod dbItem = DBProviderManager<DBShippingRateComputationMethodProvider>.Provider.UpdateShippingRateComputationMethod(ShippingRateComputationMethodID, Name, Description,
            ConfigureTemplatePath, ClassName, DisplayOrder);
            ShippingRateComputationMethod shippingRateComputationMethod = DBMapping(dbItem);

            if (ShippingRateComputationMethodManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(SHIPPINGRATECOMPUTATIONMETHODS_PATTERN_KEY);
            }
            return shippingRateComputationMethod;
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
                return SettingManager.GetSettingValueBoolean("Cache.ShippingRateComputationMethodManager.CacheEnabled");
            }
        }
        #endregion
    }
}
