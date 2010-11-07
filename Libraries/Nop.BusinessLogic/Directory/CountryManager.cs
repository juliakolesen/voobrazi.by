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
using NopSolutions.NopCommerce.DataAccess.Directory;


namespace NopSolutions.NopCommerce.BusinessLogic.Directory
{
    /// <summary>
    /// Country manager
    /// </summary>
    public partial class CountryManager
    {
        #region Constants
        private const string COUNTRIES_ALL_KEY = "Nop.country.all-{0}";
        private const string COUNTRIES_REGISTRATION_KEY = "Nop.country.registration-{0}";
        private const string COUNTRIES_BILLING_KEY = "Nop.country.billing-{0}";
        private const string COUNTRIES_SHIPPING_KEY = "Nop.country.shipping-{0}";
        private const string COUNTRIES_BY_ID_KEY = "Nop.country.id-{0}";
        private const string COUNTRIES_PATTERN_KEY = "Nop.country.";
        #endregion

        #region Utilities
        private static CountryCollection DBMapping(DBCountryCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            CountryCollection collection = new CountryCollection();
            foreach (DBCountry dbItem in dbCollection)
            {
                Country item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static Country DBMapping(DBCountry dbItem)
        {
            if (dbItem == null)
                return null;

            Country item = new Country();
            item.CountryID = dbItem.CountryID;
            item.Name = dbItem.Name;
            item.AllowsRegistration = dbItem.AllowsRegistration;
            item.AllowsBilling = dbItem.AllowsBilling;
            item.AllowsShipping = dbItem.AllowsShipping;
            item.TwoLetterISOCode = dbItem.TwoLetterISOCode;
            item.ThreeLetterISOCode = dbItem.ThreeLetterISOCode;
            item.NumericISOCode = dbItem.NumericISOCode;
            item.Published = dbItem.Published;
            item.DisplayOrder = dbItem.DisplayOrder;

            return item;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Deletes a country
        /// </summary>
        /// <param name="CountryID">Country identifier</param>
        public static void DeleteCountry(int CountryID)
        {
            DBProviderManager<DBCountryProvider>.Provider.DeleteCountry(CountryID);
            if (CountryManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(COUNTRIES_PATTERN_KEY);
            }
        }
        
        /// <summary>
        /// Gets all countries
        /// </summary>
        /// <returns>Country collection</returns>
        public static CountryCollection GetAllCountries()
        {
            bool showHidden = NopContext.Current.IsAdmin;
            string key = string.Format(COUNTRIES_ALL_KEY, showHidden);
            object obj2 = NopCache.Get(key);
            if (CountryManager.CacheEnabled && (obj2 != null))
            {
                return (CountryCollection)obj2;
            }

            DBCountryCollection dbCollection = DBProviderManager<DBCountryProvider>.Provider.GetAllCountries(showHidden);
            CountryCollection countryCollection = DBMapping(dbCollection);

            if (CountryManager.CacheEnabled)
            {
                NopCache.Max(key, countryCollection);
            }
            return countryCollection;
        }

        /// <summary>
        /// Gets all countries that allow registration
        /// </summary>
        /// <returns>Country collection</returns>
        public static CountryCollection GetAllCountriesForRegistration()
        {
            bool showHidden = NopContext.Current.IsAdmin;
            string key = string.Format(COUNTRIES_REGISTRATION_KEY, showHidden);
            object obj2 = NopCache.Get(key);
            if (CountryManager.CacheEnabled && (obj2 != null))
            {
                return (CountryCollection)obj2;
            }

            DBCountryCollection dbCollection = DBProviderManager<DBCountryProvider>.Provider.GetAllCountriesForRegistration(showHidden);
            CountryCollection countryCollection = DBMapping(dbCollection);

            if (CountryManager.CacheEnabled)
            {
                NopCache.Max(key, countryCollection);
            }
            return countryCollection;
        }

        /// <summary>
        /// Gets all countries that allow billing
        /// </summary>
        /// <returns>Country collection</returns>
        public static CountryCollection GetAllCountriesForBilling()
        {
            bool showHidden = NopContext.Current.IsAdmin;
            string key = string.Format(COUNTRIES_BILLING_KEY, showHidden);
            object obj2 = NopCache.Get(key);
            if (CountryManager.CacheEnabled && (obj2 != null))
            {
                return (CountryCollection)obj2;
            }

            DBCountryCollection dbCollection = DBProviderManager<DBCountryProvider>.Provider.GetAllCountriesForBilling(showHidden);
            CountryCollection countryCollection = DBMapping(dbCollection);

            if (CountryManager.CacheEnabled)
            {
                NopCache.Max(key, countryCollection);
            }
            return countryCollection;
        }

        /// <summary>
        /// Gets all countries that allow shipping
        /// </summary>
        /// <returns>Country collection</returns>
        public static CountryCollection GetAllCountriesForShipping()
        {
            bool showHidden = NopContext.Current.IsAdmin;
            string key = string.Format(COUNTRIES_SHIPPING_KEY, showHidden);
            object obj2 = NopCache.Get(key);
            if (CountryManager.CacheEnabled && (obj2 != null))
            {
                return (CountryCollection)obj2;
            }

            DBCountryCollection dbCollection = DBProviderManager<DBCountryProvider>.Provider.GetAllCountriesForShipping(showHidden);
            CountryCollection countryCollection = DBMapping(dbCollection);

            if (CountryManager.CacheEnabled)
            {
                NopCache.Max(key, countryCollection);
            }
            return countryCollection;
        }

        /// <summary>
        /// Gets a country 
        /// </summary>
        /// <param name="CountryID">Country identifier</param>
        /// <returns>Country</returns>
        public static Country GetCountryByID(int CountryID)
        {
            if (CountryID == 0)
                return null;

            string key = string.Format(COUNTRIES_BY_ID_KEY, CountryID);
            object obj2 = NopCache.Get(key);
            if (CountryManager.CacheEnabled && (obj2 != null))
            {
                return (Country)obj2;
            }

            DBCountry dbItem = DBProviderManager<DBCountryProvider>.Provider.GetCountryByID(CountryID);
            Country country = DBMapping(dbItem);

            if (CountryManager.CacheEnabled)
            {
                NopCache.Max(key, country);
            }
            return country;
        }

        /// <summary>
        /// Gets a country by two letter ISO code
        /// </summary>
        /// <param name="TwoLetterISOCode">Country two letter ISO code</param>
        /// <returns>Country</returns>
        public static Country GetCountryByTwoLetterISOCode(string TwoLetterISOCode)
        {
            DBCountry dbItem = DBProviderManager<DBCountryProvider>.Provider.GetCountryByTwoLetterISOCode(TwoLetterISOCode);
            Country country = DBMapping(dbItem);
            return country;
        }

        /// <summary>
        /// Gets a country by three letter ISO code
        /// </summary>
        /// <param name="ThreeLetterISOCode">Country three letter ISO code</param>
        /// <returns>Country</returns>
        public static Country GetCountryByThreeLetterISOCode(string ThreeLetterISOCode)
        {
            DBCountry dbItem = DBProviderManager<DBCountryProvider>.Provider.GetCountryByThreeLetterISOCode(ThreeLetterISOCode);
            Country country = DBMapping(dbItem);
            return country;
        }

        /// <summary>
        /// Inserts a country
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="AllowsRegistration">A value indicating whether registration is allowed to this country</param>
        /// <param name="AllowsBilling">A value indicating whether billing is allowed to this country</param>
        /// <param name="AllowsShipping">A value indicating whether shipping is allowed to this country</param>
        /// <param name="TwoLetterISOCode">The two letter ISO code</param>
        /// <param name="ThreeLetterISOCode">The three letter ISO code</param>
        /// <param name="NumericISOCode">The numeric ISO code</param>
        /// <param name="Published">A value indicating whether the entity is published</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Country</returns>
        public static Country InsertCountry(string Name,
            bool AllowsRegistration, bool AllowsBilling, bool AllowsShipping,
            string TwoLetterISOCode, string ThreeLetterISOCode, int NumericISOCode,
            bool Published, int DisplayOrder)
        {
            DBCountry dbItem = DBProviderManager<DBCountryProvider>.Provider.InsertCountry(Name,
                AllowsRegistration, AllowsBilling, AllowsShipping,
                TwoLetterISOCode, ThreeLetterISOCode, NumericISOCode, Published,
                DisplayOrder);
            Country country = DBMapping(dbItem);

            if (CountryManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(COUNTRIES_PATTERN_KEY);
            }
            return country;
        }

        /// <summary>
        /// Updates the country
        /// </summary>
        /// <param name="CountryID">The country identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="AllowsRegistration">A value indicating whether registration is allowed to this country</param>
        /// <param name="AllowsBilling">A value indicating whether billing is allowed to this country</param>
        /// <param name="AllowsShipping">A value indicating whether shipping is allowed to this country</param>
        /// <param name="TwoLetterISOCode">The two letter ISO code</param>
        /// <param name="ThreeLetterISOCode">The three letter ISO code</param>
        /// <param name="NumericISOCode">The numeric ISO code</param>
        /// <param name="Published">A value indicating whether the entity is published</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Country</returns>
        public static Country UpdateCountry(int CountryID, string Name,
            bool AllowsRegistration, bool AllowsBilling, bool AllowsShipping,
            string TwoLetterISOCode, string ThreeLetterISOCode, int NumericISOCode,
            bool Published, int DisplayOrder)
        {
            DBCountry dbItem = DBProviderManager<DBCountryProvider>.Provider.UpdateCountry(CountryID,
                Name, AllowsRegistration, AllowsBilling, AllowsShipping,
                TwoLetterISOCode, ThreeLetterISOCode, NumericISOCode,
                Published, DisplayOrder);
            Country country = DBMapping(dbItem);

            if (CountryManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(COUNTRIES_PATTERN_KEY);
            }

            return country;
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
                return SettingManager.GetSettingValueBoolean("Cache.CountryManager.CacheEnabled");
            }
        }
        #endregion
    }
}
