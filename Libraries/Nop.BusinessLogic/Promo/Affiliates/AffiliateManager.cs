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
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Promo.Affiliates;

namespace NopSolutions.NopCommerce.BusinessLogic.Promo.Affiliates
{
    /// <summary>
    /// Affiliate manager
    /// </summary>
    public partial class AffiliateManager
    {
        #region Utilities
        private static AffiliateCollection DBMapping(DBAffiliateCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            AffiliateCollection collection = new AffiliateCollection();
            foreach (DBAffiliate dbItem in dbCollection)
            {
                Affiliate item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static Affiliate DBMapping(DBAffiliate dbItem)
        {
            if (dbItem == null)
                return null;

            Affiliate item = new Affiliate();
            item.AffiliateID = dbItem.AffiliateID;
            item.FirstName = dbItem.FirstName;
            item.LastName = dbItem.LastName;
            item.MiddleName = dbItem.MiddleName;
            item.PhoneNumber = dbItem.PhoneNumber;
            item.Email = dbItem.Email;
            item.FaxNumber = dbItem.FaxNumber;
            item.Company = dbItem.Company;
            item.Address1 = dbItem.Address1;
            item.Address2 = dbItem.Address2;
            item.City = dbItem.City;
            item.StateProvince = dbItem.StateProvince;
            item.ZipPostalCode = dbItem.ZipPostalCode;
            item.CountryID = dbItem.CountryID;
            item.Deleted = dbItem.Deleted;
            item.Active = dbItem.Active;
            return item;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets an affiliate by affiliate identifier
        /// </summary>
        /// <param name="AffiliateID">Affiliate identifier</param>
        /// <returns>Affiliate</returns>
        public static Affiliate GetAffiliateByID(int AffiliateID)
        {
            if (AffiliateID == 0)
                return null;

            DBAffiliate dbItem = DBProviderManager<DBAffiliateProvider>.Provider.GetAffiliateByID(AffiliateID);
            Affiliate affiliate = DBMapping(dbItem);
            return affiliate;
        }

        /// <summary>
        /// Marks affiliate as deleted 
        /// </summary>
        /// <param name="AffiliateID">Affiliate identifier</param>
        public static void MarkAffiliateAsDeleted(int AffiliateID)
        {
            Affiliate affiliate = GetAffiliateByID(AffiliateID);
            if (affiliate != null)
            {
                affiliate = UpdateAffiliate(affiliate.AffiliateID, affiliate.FirstName, affiliate.LastName, affiliate.MiddleName, affiliate.PhoneNumber,
                      affiliate.Email, affiliate.FaxNumber, affiliate.Company, affiliate.Address1, affiliate.Address2, affiliate.City,
                      affiliate.StateProvince, affiliate.ZipPostalCode, affiliate.CountryID, true, affiliate.Active);
            }
        }

        /// <summary>
        /// Gets all affiliates
        /// </summary>
        /// <returns>Affiliate collection</returns>
        public static AffiliateCollection GetAllAffiliates()
        {
            DBAffiliateCollection dbCollection = DBProviderManager<DBAffiliateProvider>.Provider.GetAllAffiliates();
            AffiliateCollection affiliates = DBMapping(dbCollection);
            return affiliates;
        }

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
        public static Affiliate InsertAffiliate(string FirstName, string LastName, string MiddleName, 
            string PhoneNumber, string Email, string FaxNumber, string Company, string Address1,
            string Address2, string City, string StateProvince, string ZipPostalCode,
            int CountryID, bool Deleted, bool Active)
        {
            DBAffiliate dbItem = DBProviderManager<DBAffiliateProvider>.Provider.InsertAffiliate(FirstName, LastName, MiddleName,
                PhoneNumber, Email, FaxNumber, Company, Address1,
                Address2, City, StateProvince, ZipPostalCode,
                CountryID, Deleted, Active);
            Affiliate affiliate = DBMapping(dbItem);
            return affiliate;
        }
        
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
        public static Affiliate UpdateAffiliate(int AffiliateID, string FirstName, string LastName,
            string MiddleName, string PhoneNumber, string Email, string FaxNumber, string Company,
            string Address1, string Address2, string City, string StateProvince,
            string ZipPostalCode, int CountryID, bool Deleted, bool Active)
        {
            DBAffiliate dbItem = DBProviderManager<DBAffiliateProvider>.Provider.UpdateAffiliate(AffiliateID, FirstName, LastName,
                MiddleName, PhoneNumber, Email, FaxNumber, Company,
                Address1, Address2, City, StateProvince,
                ZipPostalCode, CountryID, Deleted, Active);
            Affiliate affiliate = DBMapping(dbItem);
            return affiliate;
        }
        #endregion
    }
}
