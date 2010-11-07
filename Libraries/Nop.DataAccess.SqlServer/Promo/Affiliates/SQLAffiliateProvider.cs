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
using System.Configuration.Provider;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace NopSolutions.NopCommerce.DataAccess.Promo.Affiliates
{
    /// <summary>
    /// Affiliate provider for SQL Server
    /// </summary>
    public partial class SQLAffiliateProvider : DBAffiliateProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBAffiliate GetAffiliateFromReader(IDataReader dataReader)
        {
            DBAffiliate affiliate = new DBAffiliate();
            affiliate.AffiliateID = NopSqlDataHelper.GetInt(dataReader, "AffiliateID");
            affiliate.FirstName = NopSqlDataHelper.GetString(dataReader, "FirstName");
            affiliate.LastName = NopSqlDataHelper.GetString(dataReader, "LastName");
            affiliate.MiddleName = NopSqlDataHelper.GetString(dataReader, "MiddleName");
            affiliate.PhoneNumber = NopSqlDataHelper.GetString(dataReader, "PhoneNumber");
            affiliate.Email = NopSqlDataHelper.GetString(dataReader, "Email");
            affiliate.FaxNumber = NopSqlDataHelper.GetString(dataReader, "FaxNumber");
            affiliate.Company = NopSqlDataHelper.GetString(dataReader, "Company");
            affiliate.Address1 = NopSqlDataHelper.GetString(dataReader, "Address1");
            affiliate.Address2 = NopSqlDataHelper.GetString(dataReader, "Address2");
            affiliate.City = NopSqlDataHelper.GetString(dataReader, "City");
            affiliate.StateProvince = NopSqlDataHelper.GetString(dataReader, "StateProvince");
            affiliate.ZipPostalCode = NopSqlDataHelper.GetString(dataReader, "ZipPostalCode");
            affiliate.CountryID = NopSqlDataHelper.GetInt(dataReader, "CountryID");
            affiliate.Deleted = NopSqlDataHelper.GetBoolean(dataReader, "Deleted");
            affiliate.Active = NopSqlDataHelper.GetBoolean(dataReader, "Active");
            return affiliate;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Initializes the provider with the property values specified in the application's configuration file. This method is not intended to be used directly from your code
        /// </summary>
        /// <param name="name">The name of the provider instance to initialize</param>
        /// <param name="config">A NameValueCollection that contains the names and values of configuration options for the provider.</param>
        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            base.Initialize(name, config);

            string connectionStringName = config["connectionStringName"];
            if (String.IsNullOrEmpty(connectionStringName))
                throw new ProviderException("Connection name not specified");
            this._sqlConnectionString = NopSqlDataHelper.GetConnectionString(connectionStringName);
            if ((this._sqlConnectionString == null) || (this._sqlConnectionString.Length < 1))
            {
                throw new ProviderException(string.Format("Connection string not found. {0}", connectionStringName));
            }
            config.Remove("connectionStringName");

            if (config.Count > 0)
            {
                string key = config.GetKey(0);
                if (!string.IsNullOrEmpty(key))
                {
                    throw new ProviderException(string.Format("Provider unrecognized attribute. {0}", new object[] { key }));
                }
            }
        }

        /// <summary>
        /// Gets an affiliate by affiliate identifier
        /// </summary>
        /// <param name="AffiliateID">Affiliate identifier</param>
        /// <returns>Affiliate</returns>
        public override DBAffiliate GetAffiliateByID(int AffiliateID)
        {
            DBAffiliate affiliate = null;
            if (AffiliateID == 0)
                return affiliate;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_AffiliateLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "AffiliateID", DbType.Int32, AffiliateID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    affiliate = GetAffiliateFromReader(dataReader);
                }
            }
            return affiliate;
        }

        /// <summary>
        /// Gets all affiliates
        /// </summary>
        /// <returns>Affiliate collection</returns>
        public override DBAffiliateCollection GetAllAffiliates()
        {

            DBAffiliateCollection affiliateCollection = new DBAffiliateCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_AffiliateLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBAffiliate affiliate = GetAffiliateFromReader(dataReader);
                    affiliateCollection.Add(affiliate);
                }
            }

            return affiliateCollection;
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
        public override DBAffiliate InsertAffiliate(string FirstName, string LastName, string MiddleName,
            string PhoneNumber, string Email, string FaxNumber, string Company, string Address1,
            string Address2, string City, string StateProvince, string ZipPostalCode,
            int CountryID, bool Deleted, bool Active)
        {
            DBAffiliate affiliate = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_AffiliateInsert");
            db.AddOutParameter(dbCommand, "AffiliateID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "FirstName", DbType.String, FirstName);
            db.AddInParameter(dbCommand, "LastName", DbType.String, LastName);
            db.AddInParameter(dbCommand, "MiddleName", DbType.String, MiddleName);
            db.AddInParameter(dbCommand, "PhoneNumber", DbType.String, PhoneNumber);
            db.AddInParameter(dbCommand, "Email", DbType.String, Email);
            db.AddInParameter(dbCommand, "FaxNumber", DbType.String, FaxNumber);
            db.AddInParameter(dbCommand, "Company", DbType.String, Company);
            db.AddInParameter(dbCommand, "Address1", DbType.String, Address1);
            db.AddInParameter(dbCommand, "Address2", DbType.String, Address2);
            db.AddInParameter(dbCommand, "City", DbType.String, City);
            db.AddInParameter(dbCommand, "StateProvince", DbType.String, StateProvince);
            db.AddInParameter(dbCommand, "ZipPostalCode", DbType.String, ZipPostalCode);
            db.AddInParameter(dbCommand, "CountryID", DbType.Int32, CountryID);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, Deleted);
            db.AddInParameter(dbCommand, "Active", DbType.Boolean, Active);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int AffiliateID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@AffiliateID"));
                affiliate = GetAffiliateByID(AffiliateID);
            }
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
        public override DBAffiliate UpdateAffiliate(int AffiliateID, string FirstName, string LastName,
            string MiddleName, string PhoneNumber, string Email, string FaxNumber, string Company,
            string Address1, string Address2, string City, string StateProvince,
            string ZipPostalCode, int CountryID, bool Deleted, bool Active)
        {
            DBAffiliate affiliate = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_AffiliateUpdate");
            db.AddInParameter(dbCommand, "AffiliateID", DbType.Int32, AffiliateID);
            db.AddInParameter(dbCommand, "FirstName", DbType.String, FirstName);
            db.AddInParameter(dbCommand, "LastName", DbType.String, LastName);
            db.AddInParameter(dbCommand, "MiddleName", DbType.String, MiddleName);
            db.AddInParameter(dbCommand, "PhoneNumber", DbType.String, PhoneNumber);
            db.AddInParameter(dbCommand, "Email", DbType.String, Email);
            db.AddInParameter(dbCommand, "FaxNumber", DbType.String, FaxNumber);
            db.AddInParameter(dbCommand, "Company", DbType.String, Company);
            db.AddInParameter(dbCommand, "Address1", DbType.String, Address1);
            db.AddInParameter(dbCommand, "Address2", DbType.String, Address2);
            db.AddInParameter(dbCommand, "City", DbType.String, City);
            db.AddInParameter(dbCommand, "StateProvince", DbType.String, StateProvince);
            db.AddInParameter(dbCommand, "ZipPostalCode", DbType.String, ZipPostalCode);
            db.AddInParameter(dbCommand, "CountryID", DbType.Int32, CountryID);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, Deleted);
            db.AddInParameter(dbCommand, "Active", DbType.Boolean, Active);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                affiliate = GetAffiliateByID(AffiliateID);

            return affiliate;
        }

        #endregion
    }
}
