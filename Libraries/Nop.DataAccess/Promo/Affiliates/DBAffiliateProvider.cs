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

namespace NopSolutions.NopCommerce.DataAccess.Promo.Affiliates
{
    /// <summary>
    /// Acts as a base class for deriving custom affiliate provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/AffiliateProvider")]
    public abstract partial class DBAffiliateProvider : BaseDBProvider
    {
        #region Methods
        /// <summary>
        /// Gets an affiliate by affiliate identifier
        /// </summary>
        /// <param name="AffiliateID">Affiliate identifier</param>
        /// <returns>Affiliate</returns>
        public abstract DBAffiliate GetAffiliateByID(int AffiliateID);

        /// <summary>
        /// Gets all affiliates
        /// </summary>
        /// <returns>Affiliate collection</returns>
        public abstract DBAffiliateCollection GetAllAffiliates();

        /// <summary>
        /// Inserts an affiliate
        /// </summary>
        /// <param name="FirstName">The first name</param>
        /// <param name="LastName">The last name</param>
        /// <param name="MiddleName">The middle name</param>
        /// <param name="PhoneNumber">The phone number</param>
        /// <param name="Email">The email</param>
        /// <param name="FaxNumber">The fax number</param>
        /// <param name="Company">The company</param>
        /// <param name="Address1">The address 1</param>
        /// <param name="Address2">The address 2</param>
        /// <param name="City">The city</param>
        /// <param name="StateProvince">The state/province</param>
        /// <param name="ZipPostalCode">The zip/postal code</param>
        /// <param name="CountryID">The country identifier</param>
        /// <param name="Deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="Active">A value indicating whether the entity is active</param>
        /// <returns>An affiliate</returns>
        public abstract DBAffiliate InsertAffiliate(string FirstName, string LastName, string MiddleName,
            string PhoneNumber, string Email, string FaxNumber, string Company, string Address1,
            string Address2, string City, string StateProvince, string ZipPostalCode,
            int CountryID, bool Deleted, bool Active);

        /// <summary>
        /// Updates the affiliate
        /// </summary>
        /// <param name="AffiliateID">The affiliate identifier</param>
        /// <param name="FirstName">The first name</param>
        /// <param name="LastName">The last name</param>
        /// <param name="MiddleName">The middle name</param>
        /// <param name="PhoneNumber">The phone number</param>
        /// <param name="Email">The email</param>
        /// <param name="FaxNumber">The fax number</param>
        /// <param name="Company">The company</param>
        /// <param name="Address1">The address 1</param>
        /// <param name="Address2">The address 2</param>
        /// <param name="City">The city</param>
        /// <param name="StateProvince">The state/province</param>
        /// <param name="ZipPostalCode">The zip/postal code</param>
        /// <param name="CountryID">The country identifier</param>
        /// <param name="Deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="Active">A value indicating whether the entity is active</param>
        /// <returns>An affiliate</returns>
        public abstract DBAffiliate UpdateAffiliate(int AffiliateID, string FirstName, string LastName,
            string MiddleName, string PhoneNumber, string Email, string FaxNumber, string Company,
            string Address1, string Address2, string City, string StateProvince,
            string ZipPostalCode, int CountryID, bool Deleted, bool Active);

        #endregion
    }
}
