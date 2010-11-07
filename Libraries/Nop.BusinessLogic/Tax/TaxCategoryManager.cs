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
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Tax;

namespace NopSolutions.NopCommerce.BusinessLogic.Tax
{
    /// <summary>
    /// Tax category manager
    /// </summary>
    public partial class TaxCategoryManager
    {
        #region Constants
        private const string TAXCATEGORIES_ALL_KEY = "Nop.taxcategory.all";
        private const string TAXCATEGORIES_BY_ID_KEY = "Nop.taxcategory.id-{0}";
        private const string TAXCATEGORIES_PATTERN_KEY = "Nop.taxcategory.";
        #endregion

        #region Utilities
        private static TaxCategoryCollection DBMapping(DBTaxCategoryCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            TaxCategoryCollection collection = new TaxCategoryCollection();
            foreach (DBTaxCategory dbItem in dbCollection)
            {
                TaxCategory item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static TaxCategory DBMapping(DBTaxCategory dbItem)
        {
            if (dbItem == null)
                return null;

            TaxCategory item = new TaxCategory();
            item.TaxCategoryID = dbItem.TaxCategoryID;
            item.Name = dbItem.Name;
            item.DisplayOrder = dbItem.DisplayOrder;
            item.CreatedOn = dbItem.CreatedOn;
            item.UpdatedOn = dbItem.UpdatedOn;

            return item;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Deletes a tax category
        /// </summary>
        /// <param name="TaxCategoryID">The tax category identifier</param>
        public static void DeleteTaxCategory(int TaxCategoryID)
        {
            DBProviderManager<DBTaxCategoryProvider>.Provider.DeleteTaxCategory(TaxCategoryID);
            if (TaxCategoryManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(TAXCATEGORIES_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets all tax categories
        /// </summary>
        /// <returns>Tax category collection</returns>
        public static TaxCategoryCollection GetAllTaxCategories()
        {
            string key = string.Format(TAXCATEGORIES_ALL_KEY);
            object obj2 = NopCache.Get(key);
            if (TaxCategoryManager.CacheEnabled && (obj2 != null))
            {
                return (TaxCategoryCollection)obj2;
            }

            DBTaxCategoryCollection dbCollection = DBProviderManager<DBTaxCategoryProvider>.Provider.GetAllTaxCategories();
            TaxCategoryCollection taxCategoryCollection = DBMapping(dbCollection);

            if (TaxCategoryManager.CacheEnabled)
            {
                NopCache.Max(key, taxCategoryCollection);
            }
            return taxCategoryCollection;
        }

        /// <summary>
        /// Gets a tax category
        /// </summary>
        /// <param name="TaxCategoryID">Tax category identifier</param>
        /// <returns>Tax category</returns>
        public static TaxCategory GetTaxCategoryByID(int TaxCategoryID)
        {
            if (TaxCategoryID == 0)
                return null;

            string key = string.Format(TAXCATEGORIES_BY_ID_KEY, TaxCategoryID);
            object obj2 = NopCache.Get(key);
            if (TaxCategoryManager.CacheEnabled && (obj2 != null))
            {
                return (TaxCategory)obj2;
            }

            DBTaxCategory dbItem = DBProviderManager<DBTaxCategoryProvider>.Provider.GetTaxCategoryByID(TaxCategoryID);
            TaxCategory taxCategory = DBMapping(dbItem);

            if (TaxCategoryManager.CacheEnabled)
            {
                NopCache.Max(key, taxCategory);
            }
            return taxCategory;
        }

        /// <summary>
        /// Inserts a tax category
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Tax category</returns>
        public static TaxCategory InsertTaxCategory(string Name,
            int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            UpdatedOn = DateTimeHelper.ConvertToUtcTime(UpdatedOn);

             DBTaxCategory dbItem =DBProviderManager<DBTaxCategoryProvider>.Provider.InsertTaxCategory(Name, 
                 DisplayOrder, CreatedOn, UpdatedOn);
             TaxCategory taxCategory = DBMapping(dbItem);

            if (TaxCategoryManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(TAXCATEGORIES_PATTERN_KEY);
            }
            return taxCategory;
        }

        /// <summary>
        /// Updates the tax category
        /// </summary>
        /// <param name="TaxCategoryID">The tax category identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Tax category</returns>
        public static TaxCategory UpdateTaxCategory(int TaxCategoryID, string Name,
            int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            UpdatedOn = DateTimeHelper.ConvertToUtcTime(UpdatedOn);

            DBTaxCategory dbItem = DBProviderManager<DBTaxCategoryProvider>.Provider.UpdateTaxCategory(TaxCategoryID, Name,
                DisplayOrder, CreatedOn, UpdatedOn);
            TaxCategory taxCategory = DBMapping(dbItem);

            if (TaxCategoryManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(TAXCATEGORIES_PATTERN_KEY);
            }
            return taxCategory;
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
                return SettingManager.GetSettingValueBoolean("Cache.TaxCategoryManager.CacheEnabled");
            }
        }
        #endregion
    }
}
