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
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Web.Configuration;
using System.Web.Hosting;

namespace NopSolutions.NopCommerce.DataAccess.Directory
{
    /// <summary>
    /// Acts as a base class for deriving custom country provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/CountryProvider")]
    public abstract partial class DBCountryProvider : BaseDBProvider
    {
        #region Methods
        /// <summary>
        /// Deletes a country
        /// </summary>
        /// <param name="CountryID">Country identifier</param>
        public abstract void DeleteCountry(int CountryID);

        /// <summary>
        /// Gets all countries
        /// </summary>
        /// <returns>Country collection</returns>
        public abstract DBCountryCollection GetAllCountries(bool showHidden);

        /// <summary>
        /// Gets all countries that allow registration
        /// </summary>
        /// <returns>Country collection</returns>
        public abstract DBCountryCollection GetAllCountriesForRegistration(bool showHidden);

        /// <summary>
        /// Gets all countries that allow billing
        /// </summary>
        /// <returns>Country collection</returns>
        public abstract DBCountryCollection GetAllCountriesForBilling(bool showHidden);

        /// <summary>
        /// Gets all countries that allow shipping
        /// </summary>
        /// <returns>Country collection</returns>
        public abstract DBCountryCollection GetAllCountriesForShipping(bool showHidden);

        /// <summary>
        /// Gets a country 
        /// </summary>
        /// <param name="CountryID">Country identifier</param>
        /// <returns>Country</returns>
        public abstract DBCountry GetCountryByID(int CountryID);

        /// <summary>
        /// Gets a country by two letter ISO code
        /// </summary>
        /// <param name="TwoLetterISOCode">Country two letter ISO code</param>
        /// <returns>Country</returns>
        public abstract DBCountry GetCountryByTwoLetterISOCode(string TwoLetterISOCode);

        /// <summary>
        /// Gets a country by three letter ISO code
        /// </summary>
        /// <param name="ThreeLetterISOCode">Country three letter ISO code</param>
        /// <returns>Country</returns>
        public abstract DBCountry GetCountryByThreeLetterISOCode(string ThreeLetterISOCode);

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
        public abstract DBCountry InsertCountry(string Name,
            bool AllowsRegistration, bool AllowsBilling, bool AllowsShipping,
            string TwoLetterISOCode, string ThreeLetterISOCode, int NumericISOCode,
            bool Published, int DisplayOrder);

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
        public abstract DBCountry UpdateCountry(int CountryID, string Name,
            bool AllowsRegistration, bool AllowsBilling, bool AllowsShipping,
            string TwoLetterISOCode, string ThreeLetterISOCode, int NumericISOCode,
            bool Published, int DisplayOrder);
        #endregion
    }
}
