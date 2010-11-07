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
using NopSolutions.NopCommerce.DataAccess.Tax;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;

namespace NopSolutions.NopCommerce.BusinessLogic.Tax
{
    /// <summary>
    /// Tax rate manager
    /// </summary>
    public partial class TaxRateManager
    {
        #region Constants
        private const string TAXRATE_ALL_KEY = "Nop.taxrate.all";
        private const string TAXRATE_BY_ID_KEY = "Nop.taxrate.id-{0}";
        private const string TAXRATE_PATTERN_KEY = "Nop.taxrate.";
        #endregion

        #region Utilities
        private static TaxRateCollection DBMapping(DBTaxRateCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            TaxRateCollection collection = new TaxRateCollection();
            foreach (DBTaxRate dbItem in dbCollection)
            {
                TaxRate item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static TaxRate DBMapping(DBTaxRate dbItem)
        {
            if (dbItem == null)
                return null;

            TaxRate item = new TaxRate();
            item.TaxRateID = dbItem.TaxRateID;
            item.TaxCategoryID = dbItem.TaxCategoryID;
            item.CountryID = dbItem.CountryID;
            item.StateProvinceID = dbItem.StateProvinceID;
            item.Zip = dbItem.Zip;
            item.Percentage = dbItem.Percentage;

            return item;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Deletes a tax rate
        /// </summary>
        /// <param name="TaxRateID">Tax rate identifier</param>
        public static void DeleteTaxRate(int TaxRateID)
        {
            DBProviderManager<DBTaxRateProvider>.Provider.DeleteTaxRate(TaxRateID);
            if (TaxRateManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(TAXRATE_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets a tax rate
        /// </summary>
        /// <param name="TaxRateID">Tax rate identifier</param>
        /// <returns>Tax rate</returns>
        public static TaxRate GetTaxRateByID(int TaxRateID)
        {
            if (TaxRateID == 0)
                return null;

            string key = string.Format(TAXRATE_BY_ID_KEY, TaxRateID);
            object obj2 = NopCache.Get(key);
            if (TaxRateManager.CacheEnabled && (obj2 != null))
            {
                return (TaxRate)obj2;
            }

            DBTaxRate dbItem = DBProviderManager<DBTaxRateProvider>.Provider.GetTaxRateByID(TaxRateID);
            TaxRate taxRate = DBMapping(dbItem);

            if (TaxRateManager.CacheEnabled)
            {
                NopCache.Max(key, taxRate);
            }
            return taxRate;
        }

        /// <summary>
        /// Gets all tax rates
        /// </summary>
        /// <returns>Tax rate collection</returns>
        public static TaxRateCollection GetAllTaxRates()
        {
            string key = TAXRATE_ALL_KEY;
            object obj2 = NopCache.Get(key);
            if (TaxRateManager.CacheEnabled && (obj2 != null))
            {
                return (TaxRateCollection)obj2;
            }

            DBTaxRateCollection dbCollection = DBProviderManager<DBTaxRateProvider>.Provider.GetAllTaxRates();
            TaxRateCollection collection = DBMapping(dbCollection);

            if (TaxRateManager.CacheEnabled)
            {
                NopCache.Max(key, collection);
            } 
            
            return collection;
        }

        /// <summary>
        /// Gets all tax rates by params
        /// </summary>
        /// <param name="TaxCategoryID">The tax category identifier</param>
        /// <param name="CountryID">The country identifier</param>
        /// <param name="StateProvinceID">The state/province identifier</param>
        /// <param name="Zip">The zip</param>
        /// <returns>Tax rate collection</returns>
        public static TaxRateCollection GetAllTaxRates(int TaxCategoryID, int CountryID,
            int StateProvinceID, string Zip)
        {
            if (Zip == null)
                Zip = string.Empty;
            if (!String.IsNullOrEmpty(Zip))
                Zip = Zip.Trim();

            TaxRateCollection existingRates = GetAllTaxRates().FindTaxRates(CountryID, TaxCategoryID);

            //filter by state/province
            TaxRateCollection matchedByStateProvince = new TaxRateCollection();
            foreach (TaxRate taxRate in existingRates)
            {
                if (StateProvinceID == taxRate.StateProvinceID)
                    matchedByStateProvince.Add(taxRate);
            }
            if (matchedByStateProvince.Count == 0)
            {
                foreach (TaxRate taxRate in existingRates)
                {
                    if (taxRate.StateProvinceID == 0)
                        matchedByStateProvince.Add(taxRate);
                }
            }

            //filter by zip
            TaxRateCollection matchedByZip = new TaxRateCollection();
            foreach (TaxRate taxRate in matchedByStateProvince)
            {
                if (Zip.ToLower() == taxRate.Zip.ToLower())
                    matchedByZip.Add(taxRate);
            }
            if (matchedByZip.Count == 0)
            {
                foreach (TaxRate taxRate in matchedByStateProvince)
                {
                    if (taxRate.Zip.Trim() == string.Empty)
                        matchedByZip.Add(taxRate);
                }
            }

            return matchedByZip;
        }

        /// <summary>
        /// Inserts a tax rate
        /// </summary>
        /// <param name="TaxCategoryID">The tax category identifier</param>
        /// <param name="CountryID">The country identifier</param>
        /// <param name="StateProvinceID">The state/province identifier</param>
        /// <param name="Zip">The zip</param>
        /// <param name="Percentage">The percentage</param>
        /// <returns>Tax rate</returns>
        public static TaxRate InsertTaxRate(int TaxCategoryID, int CountryID,
            int StateProvinceID, string Zip, decimal Percentage)
        {
            if (Zip == null)
                Zip = string.Empty;
            if (!String.IsNullOrEmpty(Zip))
                Zip = Zip.Trim();

            DBTaxRate dbItem = DBProviderManager<DBTaxRateProvider>.Provider.InsertTaxRate(TaxCategoryID, CountryID, StateProvinceID, Zip, Percentage);
            TaxRate taxRate = DBMapping(dbItem);

            if (TaxRateManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(TAXRATE_PATTERN_KEY);
            }

            return taxRate;
        }

        /// <summary>
        /// Updates the tax rate
        /// </summary>
        /// <param name="TaxRateID">The tax rate identifier</param>
        /// <param name="TaxCategoryID">The tax category identifier</param>
        /// <param name="CountryID">The country identifier</param>
        /// <param name="StateProvinceID">The state/province identifier</param>
        /// <param name="Zip">The zip</param>
        /// <param name="Percentage">The percentage</param>
        /// <returns>Tax rate</returns>
        public static TaxRate UpdateTaxRate(int TaxRateID, int TaxCategoryID, int CountryID,
            int StateProvinceID, string Zip, decimal Percentage)
        {
            if (Zip == null)
                Zip = string.Empty;
            if (!String.IsNullOrEmpty(Zip))
                Zip = Zip.Trim();

            DBTaxRate dbItem = DBProviderManager<DBTaxRateProvider>.Provider.UpdateTaxRate(TaxRateID, TaxCategoryID, CountryID, StateProvinceID, Zip, Percentage);
            TaxRate taxRate = DBMapping(dbItem);

            if (TaxRateManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(TAXRATE_PATTERN_KEY);
            }

            return taxRate;
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
                return SettingManager.GetSettingValueBoolean("Cache.TaxRateManager.CacheEnabled");
            }
        }
        #endregion
    }
}
