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

namespace NopSolutions.NopCommerce.DataAccess.CustomerManagement
{
    /// <summary>
    /// Customer provider for SQL Server
    /// </summary>
    public partial class SQLCustomerProvider : DBCustomerProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBAddress GetAddressFromReader(IDataReader dataReader)
        {
            DBAddress address = new DBAddress();
            address.AddressID = NopSqlDataHelper.GetInt(dataReader, "AddressID");
            address.CustomerID = NopSqlDataHelper.GetInt(dataReader, "CustomerID");
            address.IsBillingAddress = NopSqlDataHelper.GetBoolean(dataReader, "IsBillingAddress");
            address.FirstName = NopSqlDataHelper.GetString(dataReader, "FirstName");
            address.LastName = NopSqlDataHelper.GetString(dataReader, "LastName");
            address.PhoneNumber = NopSqlDataHelper.GetString(dataReader, "PhoneNumber");
            address.Email = NopSqlDataHelper.GetString(dataReader, "Email");
            address.FaxNumber = NopSqlDataHelper.GetString(dataReader, "FaxNumber");
            address.Company = NopSqlDataHelper.GetString(dataReader, "Company");
            address.Address1 = NopSqlDataHelper.GetString(dataReader, "Address1");
            address.Address2 = NopSqlDataHelper.GetString(dataReader, "Address2");
            address.City = NopSqlDataHelper.GetString(dataReader, "City");
            address.StateProvinceID = NopSqlDataHelper.GetInt(dataReader, "StateProvinceID");
            address.ZipPostalCode = NopSqlDataHelper.GetString(dataReader, "ZipPostalCode");
            address.CountryID = NopSqlDataHelper.GetInt(dataReader, "CountryID");
            address.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            address.UpdatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "UpdatedOn");
            return address;
        }

        private DBCustomer GetCustomerFromReader(IDataReader dataReader)
        {
            DBCustomer customer = new DBCustomer();
            customer.CustomerID = NopSqlDataHelper.GetInt(dataReader, "CustomerID");
            customer.CustomerGUID = NopSqlDataHelper.GetGuid(dataReader, "CustomerGUID");
            customer.Email = NopSqlDataHelper.GetString(dataReader, "Email");
            customer.Username = NopSqlDataHelper.GetString(dataReader, "Username");
            customer.PasswordHash = NopSqlDataHelper.GetString(dataReader, "PasswordHash");
            customer.SaltKey = NopSqlDataHelper.GetString(dataReader, "SaltKey");
            customer.AffiliateID = NopSqlDataHelper.GetInt(dataReader, "AffiliateID");
            customer.BillingAddressID = NopSqlDataHelper.GetInt(dataReader, "BillingAddressID");
            customer.ShippingAddressID = NopSqlDataHelper.GetInt(dataReader, "ShippingAddressID");
            customer.LastPaymentMethodID = NopSqlDataHelper.GetInt(dataReader, "LastPaymentMethodID");
            customer.LastAppliedCouponCode = NopSqlDataHelper.GetString(dataReader, "LastAppliedCouponCode");
            customer.LanguageID = NopSqlDataHelper.GetInt(dataReader, "LanguageID");
            customer.CurrencyID = NopSqlDataHelper.GetInt(dataReader, "CurrencyID");
            customer.TaxDisplayTypeID = NopSqlDataHelper.GetInt(dataReader, "TaxDisplayTypeID");
            customer.IsAdmin = NopSqlDataHelper.GetBoolean(dataReader, "IsAdmin");
            customer.IsTaxExempt = NopSqlDataHelper.GetBoolean(dataReader, "IsTaxExempt");
            customer.IsGuest = NopSqlDataHelper.GetBoolean(dataReader, "IsGuest");
            customer.IsForumModerator = NopSqlDataHelper.GetBoolean(dataReader, "IsForumModerator");
            customer.TotalForumPosts = NopSqlDataHelper.GetInt(dataReader, "TotalForumPosts");
            customer.Signature = NopSqlDataHelper.GetString(dataReader, "Signature");
            customer.AdminComment = NopSqlDataHelper.GetString(dataReader, "AdminComment");
            customer.Active = NopSqlDataHelper.GetBoolean(dataReader, "Active");
            customer.Deleted = NopSqlDataHelper.GetBoolean(dataReader, "Deleted");
            customer.RegistrationDate = NopSqlDataHelper.GetUtcDateTime(dataReader, "RegistrationDate");
            customer.TimeZoneID = NopSqlDataHelper.GetString(dataReader, "TimeZoneID");
            customer.AvatarID = NopSqlDataHelper.GetInt(dataReader, "AvatarID");
            return customer;
        }

        private DBCustomerAttribute GetCustomerAttributeFromReader(IDataReader dataReader)
        {
            DBCustomerAttribute customerAttribute = new DBCustomerAttribute();
            customerAttribute.CustomerAttributeID = NopSqlDataHelper.GetInt(dataReader, "CustomerAttributeID");
            customerAttribute.CustomerID = NopSqlDataHelper.GetInt(dataReader, "CustomerID");
            customerAttribute.Key = NopSqlDataHelper.GetString(dataReader, "Key");
            customerAttribute.Value = NopSqlDataHelper.GetString(dataReader, "Value");
            return customerAttribute;
        }

        private DBCustomerRole GetCustomerRoleFromReader(IDataReader dataReader)
        {
            DBCustomerRole customerRole = new DBCustomerRole();
            customerRole.CustomerRoleID = NopSqlDataHelper.GetInt(dataReader, "CustomerRoleID");
            customerRole.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            customerRole.FreeShipping = NopSqlDataHelper.GetBoolean(dataReader, "FreeShipping");
            customerRole.TaxExempt = NopSqlDataHelper.GetBoolean(dataReader, "TaxExempt");
            customerRole.Active = NopSqlDataHelper.GetBoolean(dataReader, "Active");
            customerRole.Deleted = NopSqlDataHelper.GetBoolean(dataReader, "Deleted");
            return customerRole;
        }

        private DBCustomerSession GetCustomerSessionFromReader(IDataReader dataReader)
        {
            DBCustomerSession customerSession = new DBCustomerSession();
            customerSession.CustomerSessionGUID = NopSqlDataHelper.GetGuid(dataReader, "CustomerSessionGUID");
            customerSession.CustomerID = NopSqlDataHelper.GetInt(dataReader, "CustomerID");
            customerSession.LastAccessed = NopSqlDataHelper.GetUtcDateTime(dataReader, "LastAccessed");
            customerSession.IsExpired = NopSqlDataHelper.GetBoolean(dataReader, "IsExpired");
            return customerSession;
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
        /// Deletes an address by address identifier 
        /// </summary>
        /// <param name="AddressID">Address identifier</param>
        public override void DeleteAddress(int AddressID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_AddressDelete");
            db.AddInParameter(dbCommand, "AddressID", DbType.Int32, AddressID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets an address by address identifier
        /// </summary>
        /// <param name="AddressID">Address identifier</param>
        /// <returns>Address</returns>
        public override DBAddress GetAddressByID(int AddressID)
        {
            DBAddress address = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_AddressLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "AddressID", DbType.Int32, AddressID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    address = GetAddressFromReader(dataReader);
                }
            }
            return address;
        }

        /// <summary>
        /// Gets a collection of addresses by customer identifier
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="GetBillingAddresses">Gets or sets a value indicating whether the addresses are billing or shipping</param>
        /// <returns>A collection of addresses</returns>
        public override DBAddressCollection GetAddressesByCustomerID(int CustomerID, bool GetBillingAddresses)
        {
            DBAddressCollection addressCollection = new DBAddressCollection();
            if (CustomerID == 0)
                return addressCollection;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_AddressLoadByCustomerID");
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, CustomerID);
            db.AddInParameter(dbCommand, "GetBillingAddresses", DbType.Boolean, GetBillingAddresses);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBAddress address = GetAddressFromReader(dataReader);
                    addressCollection.Add(address);
                }
            }
            return addressCollection;
        }

        /// <summary>
        /// Inserts an address
        /// </summary>
        /// <param name="CustomerID">The customer identifier</param>
        /// <param name="IsBillingAddress">A value indicating whether the address is billing or shipping</param>
        /// <param name="FirstName">The first name</param>
        /// <param name="LastName">The last name</param>
        /// <param name="PhoneNumber">The phone number</param>
        /// <param name="Email">The email</param>
        /// <param name="FaxNumber">The fax number</param>
        /// <param name="Company">The company</param>
        /// <param name="Address1">The address 1</param>
        /// <param name="Address2">The address 2</param>
        /// <param name="City">The city</param>
        /// <param name="StateProvinceID">The state/province identifier</param>
        /// <param name="ZipPostalCode">The zip/postal code</param>
        /// <param name="CountryID">The country identifier</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>An address</returns>
        public override DBAddress InsertAddress(int CustomerID, bool IsBillingAddress, string FirstName, string LastName,
            string PhoneNumber, string Email, string FaxNumber, string Company, string Address1,
            string Address2, string City, int StateProvinceID, string ZipPostalCode,
            int CountryID, DateTime CreatedOn, DateTime UpdatedOn)
        {
            DBAddress address = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_AddressInsert");
            db.AddOutParameter(dbCommand, "AddressID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, CustomerID);
            db.AddInParameter(dbCommand, "IsBillingAddress", DbType.Boolean, IsBillingAddress);
            db.AddInParameter(dbCommand, "FirstName", DbType.String, FirstName);
            db.AddInParameter(dbCommand, "LastName", DbType.String, LastName);
            db.AddInParameter(dbCommand, "PhoneNumber", DbType.String, PhoneNumber);
            db.AddInParameter(dbCommand, "Email", DbType.String, Email);
            db.AddInParameter(dbCommand, "FaxNumber", DbType.String, FaxNumber);
            db.AddInParameter(dbCommand, "Company", DbType.String, Company);
            db.AddInParameter(dbCommand, "Address1", DbType.String, Address1);
            db.AddInParameter(dbCommand, "Address2", DbType.String, Address2);
            db.AddInParameter(dbCommand, "City", DbType.String, City);
            db.AddInParameter(dbCommand, "StateProvinceID", DbType.Int32, StateProvinceID);
            db.AddInParameter(dbCommand, "ZipPostalCode", DbType.String, ZipPostalCode);
            db.AddInParameter(dbCommand, "CountryID", DbType.Int32, CountryID);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, UpdatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int AddressID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@AddressID"));
                address = GetAddressByID(AddressID);
            }
            return address;
        }

        /// <summary>
        /// Updates the address
        /// </summary>
        /// <param name="AddressID">The address identifier</param>
        /// <param name="CustomerID">The customer identifier</param>
        /// <param name="IsBillingAddress">A value indicating whether the address is billing or shipping</param>
        /// <param name="FirstName">The first name</param>
        /// <param name="LastName">The last name</param>
        /// <param name="PhoneNumber">The phone number</param>
        /// <param name="Email">The email</param>
        /// <param name="FaxNumber">The fax number</param>
        /// <param name="Company">The company</param>
        /// <param name="Address1">The address 1</param>
        /// <param name="Address2">The address 2</param>
        /// <param name="City">The city</param>
        /// <param name="StateProvinceID">The state/province identifier</param>
        /// <param name="ZipPostalCode">The zip/postal code</param>
        /// <param name="CountryID">The country identifier</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>An address</returns>
        public override DBAddress UpdateAddress(int AddressID, int CustomerID, bool IsBillingAddress, string FirstName, string LastName,
            string PhoneNumber, string Email, string FaxNumber, string Company,
            string Address1, string Address2, string City, int StateProvinceID,
            string ZipPostalCode, int CountryID, DateTime CreatedOn, DateTime UpdatedOn)
        {
            DBAddress address = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_AddressUpdate");
            db.AddInParameter(dbCommand, "AddressID", DbType.Int32, AddressID);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, CustomerID);
            db.AddInParameter(dbCommand, "IsBillingAddress", DbType.Boolean, IsBillingAddress);
            db.AddInParameter(dbCommand, "FirstName", DbType.String, FirstName);
            db.AddInParameter(dbCommand, "LastName", DbType.String, LastName);
            db.AddInParameter(dbCommand, "PhoneNumber", DbType.String, PhoneNumber);
            db.AddInParameter(dbCommand, "Email", DbType.String, Email);
            db.AddInParameter(dbCommand, "FaxNumber", DbType.String, FaxNumber);
            db.AddInParameter(dbCommand, "Company", DbType.String, Company);
            db.AddInParameter(dbCommand, "Address1", DbType.String, Address1);
            db.AddInParameter(dbCommand, "Address2", DbType.String, Address2);
            db.AddInParameter(dbCommand, "City", DbType.String, City);
            db.AddInParameter(dbCommand, "StateProvinceID", DbType.Int32, StateProvinceID);
            db.AddInParameter(dbCommand, "ZipPostalCode", DbType.String, ZipPostalCode);
            db.AddInParameter(dbCommand, "CountryID", DbType.Int32, CountryID);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, UpdatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                address = GetAddressByID(AddressID);

            return address;
        }

        /// <summary>
        /// Gets all customers
        /// </summary>
        /// <param name="RegistrationFrom">Customer registration from; null to load all customers</param>
        /// <param name="RegistrationTo">Customer registration to; null to load all customers</param>
        /// <param name="Email">Customer Email</param>
        /// <param name="Username">Customer username</param>
        /// <param name="DontLoadGuestCustomers">A value indicating whether to don't load guest customers</param>
        /// <param name="PageSize">Page size</param>
        /// <param name="PageIndex">Page index</param>
        /// <param name="TotalRecords">Total records</param>
        /// <returns>Customer collection</returns>
        public override DBCustomerCollection GetAllCustomers(DateTime? RegistrationFrom, DateTime? RegistrationTo,
            string Email, string Username, bool DontLoadGuestCustomers,
            int PageSize, int PageIndex, out int TotalRecords)
        {
            TotalRecords = 0;
            DBCustomerCollection customerCollection = new DBCustomerCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerLoadAll");
            if (RegistrationFrom.HasValue)
                db.AddInParameter(dbCommand, "StartTime", DbType.DateTime, RegistrationFrom.Value);
            else
                db.AddInParameter(dbCommand, "StartTime", DbType.DateTime, null);
            if (RegistrationTo.HasValue)
                db.AddInParameter(dbCommand, "EndTime", DbType.DateTime, RegistrationTo.Value);
            else
                db.AddInParameter(dbCommand, "EndTime", DbType.DateTime, null);
            db.AddInParameter(dbCommand, "Email", DbType.String, Email);
            db.AddInParameter(dbCommand, "Username", DbType.String, Username);
            db.AddInParameter(dbCommand, "DontLoadGuestCustomers", DbType.Boolean, DontLoadGuestCustomers);
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, PageSize);
            db.AddInParameter(dbCommand, "PageIndex", DbType.Int32, PageIndex);
            db.AddOutParameter(dbCommand, "TotalRecords", DbType.Int32, 0);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBCustomer customer = GetCustomerFromReader(dataReader);
                    customerCollection.Add(customer);
                }
            }
            TotalRecords = Convert.ToInt32(db.GetParameterValue(dbCommand, "@TotalRecords"));

            return customerCollection;
        }

        /// <summary>
        /// Gets all customers for newsletters
        /// </summary>
        /// <returns>Customer collection</returns>
        public override DBCustomerCollection GetAllCustomersForNewsLetters()
        {
            DBCustomerCollection customerCollection = new DBCustomerCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerLoadAllForNewsLetters");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBCustomer customer = GetCustomerFromReader(dataReader);
                    customerCollection.Add(customer);
                }
            }

            return customerCollection;
        }

        /// <summary>
        /// Gets all customers by affiliate identifier
        /// </summary>
        /// <param name="AffiliateID">Affiliate identifier</param>
        /// <returns>Customer collection</returns>
        public override DBCustomerCollection GetAffiliatedCustomers(int AffiliateID)
        {
            DBCustomerCollection customerCollection = new DBCustomerCollection();
            if (AffiliateID == 0)
                return customerCollection;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerLoadByAffiliateID");
            db.AddInParameter(dbCommand, "AffiliateID", DbType.Int32, AffiliateID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBCustomer customer = GetCustomerFromReader(dataReader);
                    customerCollection.Add(customer);
                }
            }

            return customerCollection;
        }

        /// <summary>
        /// Gets all customers by customer role id
        /// </summary>
        /// <param name="CustomerRoleID">Customer role identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Customer collection</returns>
        public override DBCustomerCollection GetCustomersByCustomerRoleID(int CustomerRoleID, bool showHidden)
        {
            DBCustomerCollection customerCollection = new DBCustomerCollection();
            if (CustomerRoleID == 0)
                return customerCollection;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerLoadByCustomerRoleID");
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            db.AddInParameter(dbCommand, "CustomerRoleID", DbType.Int32, CustomerRoleID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBCustomer customer = GetCustomerFromReader(dataReader);
                    customerCollection.Add(customer);
                }
            }

            return customerCollection;
        }

        /// <summary>
        /// Gets a customer by email
        /// </summary>
        /// <param name="Email">Customer Email</param>
        /// <returns>A customer</returns>
        public override DBCustomer GetCustomerByEmail(string Email)
        {
            DBCustomer customer = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerLoadByEmail");
            db.AddInParameter(dbCommand, "Email", DbType.String, Email);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    customer = GetCustomerFromReader(dataReader);
                }
            }

            return customer;
        }

        /// <summary>
        /// Gets a customer by email
        /// </summary>
        /// <param name="Username">Customer username</param>
        /// <returns>A customer</returns>
        public override DBCustomer GetCustomerByUsername(string Username)
        {
            DBCustomer customer = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerLoadByUsername");
            db.AddInParameter(dbCommand, "Username", DbType.String, Username);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    customer = GetCustomerFromReader(dataReader);
                }
            }

            return customer;
        }

        /// <summary>
        /// Gets a customer
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <returns>A customer</returns>
        public override DBCustomer GetCustomerByID(int CustomerID)
        {
            DBCustomer customer = null;
            if (CustomerID == 0)
                return customer;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, CustomerID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    customer = GetCustomerFromReader(dataReader);
                }
            }

            return customer;
        }

        /// <summary>
        /// Gets a customer by GUID
        /// </summary>
        /// <param name="CustomerGUID">Customer GUID</param>
        /// <returns>A customer</returns>
        public override DBCustomer GetCustomerByGUID(Guid CustomerGUID)
        {
            DBCustomer customer = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerLoadByGUID");
            db.AddInParameter(dbCommand, "CustomerGUID", DbType.Guid, CustomerGUID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    customer = GetCustomerFromReader(dataReader);
                }
            }

            return customer;
        }

        /// <summary>
        /// Adds a customer
        /// </summary>
        /// <param name="CustomerGUID">The customer identifier</param>
        /// <param name="Email">The email</param>
        /// <param name="Username">The username</param>
        /// <param name="passwordHash">The password hash</param>
        /// <param name="saltKey">The salt key</param>
        /// <param name="AffiliateID">The affiliate identifier</param>
        /// <param name="BillingAddressID">The billing address identifier</param>
        /// <param name="ShippingAddressID">The shipping address identifier</param>
        /// <param name="LastPaymentMethodID">The last payment method identifier</param>
        /// <param name="LastAppliedCouponCode">The last applied coupon code</param>
        /// <param name="LanguageID">The language identifier</param>
        /// <param name="CurrencyID">The currency identifier</param>
        /// <param name="TaxDisplayTypeID">The tax display type identifier</param>
        /// <param name="IsTaxExempt">A value indicating whether the customer is tax exempt</param>
        /// <param name="IsAdmin">A value indicating whether the customer is administrator</param>
        /// <param name="IsGuest">A value indicating whether the customer is guest</param>
        /// <param name="IsForumModerator">A value indicating whether the customer is forum moderator</param>
        /// <param name="TotalForumPosts">A forum post count</param>
        /// <param name="Signature">Signature</param>
        /// <param name="AdminComment">Admin comment</param>
        /// <param name="Active">A value indicating whether the customer is active</param>
        /// <param name="Deleted">A value indicating whether the customer has been deleted</param>
        /// <param name="RegistrationDate">The date and time of customer registration</param>
        /// <param name="TimeZoneID">The time zone identifier</param>
        /// <param name="AvatarID">The avatar identifier</param>
        /// <returns>A customer</returns>
        public override DBCustomer AddCustomer(Guid CustomerGUID, string Email,
            string Username, string passwordHash, string saltKey,
            int AffiliateID, int BillingAddressID,
            int ShippingAddressID, int LastPaymentMethodID,
            string LastAppliedCouponCode, int LanguageID, int CurrencyID,
            int TaxDisplayTypeID, bool IsTaxExempt, bool IsAdmin,
            bool IsGuest, bool IsForumModerator, int TotalForumPosts, string Signature, string AdminComment,
            bool Active, bool Deleted, DateTime RegistrationDate, string TimeZoneID, int AvatarID)
        {
            DBCustomer customer = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerInsert");
            db.AddOutParameter(dbCommand, "CustomerID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "CustomerGUID", DbType.Guid, CustomerGUID);
            db.AddInParameter(dbCommand, "Email", DbType.String, Email);
            db.AddInParameter(dbCommand, "Username", DbType.String, Username);
            db.AddInParameter(dbCommand, "PasswordHash", DbType.String, passwordHash);
            db.AddInParameter(dbCommand, "SaltKey", DbType.String, saltKey);
            db.AddInParameter(dbCommand, "AffiliateID", DbType.Int32, AffiliateID);
            db.AddInParameter(dbCommand, "BillingAddressID", DbType.Int32, BillingAddressID);
            db.AddInParameter(dbCommand, "ShippingAddressID", DbType.Int32, ShippingAddressID);
            db.AddInParameter(dbCommand, "LastPaymentMethodID", DbType.Int32, LastPaymentMethodID);
            db.AddInParameter(dbCommand, "LastAppliedCouponCode", DbType.String, LastAppliedCouponCode);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
            db.AddInParameter(dbCommand, "CurrencyID", DbType.Int32, CurrencyID);
            db.AddInParameter(dbCommand, "TaxDisplayTypeID", DbType.Int32, TaxDisplayTypeID);
            db.AddInParameter(dbCommand, "IsTaxExempt", DbType.Boolean, IsTaxExempt);
            db.AddInParameter(dbCommand, "IsAdmin", DbType.Boolean, IsAdmin);
            db.AddInParameter(dbCommand, "IsGuest", DbType.Boolean, IsGuest);
            db.AddInParameter(dbCommand, "IsForumModerator", DbType.Boolean, IsForumModerator);
            db.AddInParameter(dbCommand, "TotalForumPosts", DbType.Int32, TotalForumPosts);
            db.AddInParameter(dbCommand, "Signature", DbType.String, Signature);
            db.AddInParameter(dbCommand, "AdminComment", DbType.String, AdminComment);
            db.AddInParameter(dbCommand, "Active", DbType.Boolean, Active);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, Deleted);
            db.AddInParameter(dbCommand, "RegistrationDate", DbType.DateTime, RegistrationDate);
            db.AddInParameter(dbCommand, "TimeZoneID", DbType.String, TimeZoneID);
            db.AddInParameter(dbCommand, "AvatarID", DbType.Int32, AvatarID);

            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int CustomerID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@CustomerID"));
                customer = GetCustomerByID(CustomerID);
            }

            return customer;
        }

        /// <summary>
        /// Updates the customer
        /// </summary>
        /// <param name="CustomerID">The customer identifier</param>
        /// <param name="CustomerGUID">The customer identifier</param>
        /// <param name="Email">The email</param>
        /// <param name="Username">The username</param>
        /// <param name="PasswordHash">The password hash</param>
        /// <param name="SaltKey">The salk key</param>
        /// <param name="AffiliateID">The affiliate identifier</param>
        /// <param name="BillingAddressID">The billing address identifier</param>
        /// <param name="ShippingAddressID">The shipping address identifier</param>
        /// <param name="LastPaymentMethodID">The last payment method identifier</param>
        /// <param name="LastAppliedCouponCode">The last applied coupon code</param>
        /// <param name="LanguageID">The language identifier</param>
        /// <param name="CurrencyID">The currency identifier</param>
        /// <param name="TaxDisplayTypeID">The tax display type identifier</param>
        /// <param name="IsTaxExempt">A value indicating whether the customer is tax exempt</param>
        /// <param name="IsAdmin">A value indicating whether the customer is administrator</param>
        /// <param name="IsGuest">A value indicating whether the customer is guest</param>
        /// <param name="IsForumModerator">A value indicating whether the customer is forum moderator</param>
        /// <param name="TotalForumPosts">A forum post count</param>
        /// <param name="Signature">Signature</param>
        /// <param name="AdminComment">Admin comment</param>
        /// <param name="Active">A value indicating whether the customer is active</param>
        /// <param name="Deleted">A value indicating whether the customer has been deleted</param>
        /// <param name="RegistrationDate">The date and time of customer registration</param>
        /// <param name="TimeZoneID">The time zone identifier</param>
        /// <param name="AvatarID">The avatar identifier</param>
        /// <returns>A customer</returns>
        public override DBCustomer UpdateCustomer(int CustomerID, Guid CustomerGUID, string Email,
            string Username, string PasswordHash, string SaltKey,
            int AffiliateID, int BillingAddressID,
            int ShippingAddressID, int LastPaymentMethodID,
            string LastAppliedCouponCode, int LanguageID,
            int CurrencyID, int TaxDisplayTypeID, bool IsTaxExempt, 
            bool IsAdmin, bool IsGuest, bool IsForumModerator,
            int TotalForumPosts, string Signature, string AdminComment, bool Active,
            bool Deleted, DateTime RegistrationDate, string TimeZoneID, int AvatarID)
        {
            DBCustomer customer = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerUpdate");
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, CustomerID);
            db.AddInParameter(dbCommand, "CustomerGUID", DbType.Guid, CustomerGUID);
            db.AddInParameter(dbCommand, "Email", DbType.String, Email);
            db.AddInParameter(dbCommand, "Username", DbType.String, Username);
            db.AddInParameter(dbCommand, "PasswordHash", DbType.String, PasswordHash);
            db.AddInParameter(dbCommand, "SaltKey", DbType.String, SaltKey);
            db.AddInParameter(dbCommand, "AffiliateID", DbType.Int32, AffiliateID);
            db.AddInParameter(dbCommand, "BillingAddressID", DbType.Int32, BillingAddressID);
            db.AddInParameter(dbCommand, "ShippingAddressID", DbType.Int32, ShippingAddressID);
            db.AddInParameter(dbCommand, "LastPaymentMethodID", DbType.Int32, LastPaymentMethodID);
            db.AddInParameter(dbCommand, "LastAppliedCouponCode", DbType.String, LastAppliedCouponCode);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
            db.AddInParameter(dbCommand, "CurrencyID", DbType.Int32, CurrencyID);
            db.AddInParameter(dbCommand, "TaxDisplayTypeID", DbType.Int32, TaxDisplayTypeID);
            db.AddInParameter(dbCommand, "IsTaxExempt", DbType.Boolean, IsTaxExempt);
            db.AddInParameter(dbCommand, "IsAdmin", DbType.Boolean, IsAdmin);
            db.AddInParameter(dbCommand, "IsGuest", DbType.Boolean, IsGuest);
            db.AddInParameter(dbCommand, "IsForumModerator", DbType.Boolean, IsForumModerator);
            db.AddInParameter(dbCommand, "TotalForumPosts", DbType.Int32, TotalForumPosts);
            db.AddInParameter(dbCommand, "Signature", DbType.String, Signature);
            db.AddInParameter(dbCommand, "AdminComment", DbType.String, AdminComment);
            db.AddInParameter(dbCommand, "Active", DbType.Boolean, Active);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, Deleted);
            db.AddInParameter(dbCommand, "RegistrationDate", DbType.DateTime, RegistrationDate);
            db.AddInParameter(dbCommand, "TimeZoneID", DbType.String, TimeZoneID);
            db.AddInParameter(dbCommand, "AvatarID", DbType.Int32, AvatarID);

            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                customer = GetCustomerByID(CustomerID);
            }

            return customer;
        }

        /// <summary>
        /// Deletes a customer attribute
        /// </summary>
        /// <param name="CustomerAttributeID">Customer attribute identifier</param>
        public override void DeleteCustomerAttribute(int CustomerAttributeID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerAttributeDelete");
            db.AddInParameter(dbCommand, "CustomerAttributeID", DbType.Int32, CustomerAttributeID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a customer attribute
        /// </summary>
        /// <param name="CustomerAttributeID">Customer attribute identifier</param>
        /// <returns>A customer attribute</returns>
        public override DBCustomerAttribute GetCustomerAttributeByID(int CustomerAttributeID)
        {

            DBCustomerAttribute customerAttribute = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerAttributeLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "CustomerAttributeID", DbType.Int32, CustomerAttributeID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    customerAttribute = GetCustomerAttributeFromReader(dataReader);
                }
            }
            return customerAttribute;
        }

        /// <summary>
        /// Gets a collection of customer attributes by customer identifier
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <returns>Customer attributes</returns>
        public override DBCustomerAttributeCollection GetCustomerAttributesByCustomerID(int CustomerID)
        {
            DBCustomerAttributeCollection customerAttributeCollection = new DBCustomerAttributeCollection();
            if (CustomerID == 0)
                return customerAttributeCollection;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerAttributeLoadAllByCustomerID");
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, CustomerID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBCustomerAttribute customerAttribute = GetCustomerAttributeFromReader(dataReader);
                    customerAttributeCollection.Add(customerAttribute);
                }
            }
            return customerAttributeCollection;
        }

        /// <summary>
        /// Inserts a customer attribute
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="Key">An attribute key</param>
        /// <param name="Value">An attribute value</param>
        /// <returns>A customer attribute</returns>
        public override DBCustomerAttribute InsertCustomerAttribute(int CustomerID, string Key, string Value)
        {
            DBCustomerAttribute customerAttribute = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerAttributeInsert");
            db.AddOutParameter(dbCommand, "CustomerAttributeID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, CustomerID);
            db.AddInParameter(dbCommand, "Key", DbType.String, Key);
            db.AddInParameter(dbCommand, "Value", DbType.String, Value);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int CustomerAttributeID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@CustomerAttributeID"));
                customerAttribute = GetCustomerAttributeByID(CustomerAttributeID);
            }
            return customerAttribute;
        }

        /// <summary>
        /// Updates the customer attribute
        /// </summary>
        /// <param name="CustomerAttributeID">Customer attribute identifier</param>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="Key">An attribute key</param>
        /// <param name="Value">An attribute value</param>
        /// <returns>A customer attribute</returns>
        public override DBCustomerAttribute UpdateCustomerAttribute(int CustomerAttributeID, int CustomerID, string Key, string Value)
        {
            DBCustomerAttribute customerAttribute = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerAttributeUpdate");
            db.AddInParameter(dbCommand, "CustomerAttributeID", DbType.Int32, CustomerAttributeID);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, CustomerID);
            db.AddInParameter(dbCommand, "Key", DbType.String, Key);
            db.AddInParameter(dbCommand, "Value", DbType.String, Value);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                customerAttribute = GetCustomerAttributeByID(CustomerAttributeID);

            return customerAttribute;
        }

        /// <summary>
        /// Gets customer roles by customer identifier
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Customer role collection</returns>
        public override DBCustomerRoleCollection GetCustomerRolesByCustomerID(int CustomerID, bool showHidden)
        {
            DBCustomerRoleCollection customerRoleCollection = new DBCustomerRoleCollection();
            if (CustomerID == 0)
                return customerRoleCollection;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerRoleLoadByCustomerID");
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, CustomerID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBCustomerRole customerRole = GetCustomerRoleFromReader(dataReader);
                    customerRoleCollection.Add(customerRole);
                }
            }

            return customerRoleCollection;
        }

        /// <summary>
        /// Gets all customer roles
        /// </summary>
        /// <returns>Customer role collection</returns>
        public override DBCustomerRoleCollection GetAllCustomerRoles(bool showHidden)
        {
            DBCustomerRoleCollection customerRoleCollection = new DBCustomerRoleCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerRoleLoadAll");
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBCustomerRole customerRole = GetCustomerRoleFromReader(dataReader);
                    customerRoleCollection.Add(customerRole);
                }
            }

            return customerRoleCollection;
        }

        /// <summary>
        /// Gets a customer role
        /// </summary>
        /// <param name="CustomerRoleID">Customer role identifier</param>
        /// <returns>Customer role</returns>
        public override DBCustomerRole GetCustomerRoleByID(int CustomerRoleID)
        {
            DBCustomerRole customerRole = null;
            if (CustomerRoleID == 0)
                return customerRole;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerRoleLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "CustomerRoleID", DbType.Int32, CustomerRoleID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    customerRole = GetCustomerRoleFromReader(dataReader);
                }
            }
            return customerRole;
        }

        /// <summary>
        /// Inserts a customer role
        /// </summary>
        /// <param name="Name">The customer role name</param>
        /// <param name="FreeShipping">A value indicating whether the customer role is marked as free shiping</param>
        /// <param name="TaxExempt">A value indicating whether the customer role is marked as tax exempt</param>
        /// <param name="Active">A value indicating whether the customer role is active</param>
        /// <param name="Deleted">A value indicating whether the customer role has been deleted</param>
        /// <returns>Customer role</returns>
        public override DBCustomerRole InsertCustomerRole(string Name, bool FreeShipping, 
            bool TaxExempt, bool Active, bool Deleted)
        {
            DBCustomerRole customerRole = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerRoleInsert");
            db.AddOutParameter(dbCommand, "CustomerRoleID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "FreeShipping", DbType.Boolean, FreeShipping);
            db.AddInParameter(dbCommand, "TaxExempt", DbType.Boolean, TaxExempt);
            db.AddInParameter(dbCommand, "Active", DbType.Boolean, Active);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, Deleted);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int CustomerRoleID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@CustomerRoleID"));
                customerRole = GetCustomerRoleByID(CustomerRoleID);
            }
            return customerRole;
        }

        /// <summary>
        /// Updates the customer role
        /// </summary>
        /// <param name="CustomerRoleID">The customer role identifier</param>
        /// <param name="Name">The customer role name</param>
        /// <param name="FreeShipping">A value indicating whether the customer role is marked as free shiping</param>
        /// <param name="TaxExempt">A value indicating whether the customer role is marked as tax exempt</param>
        /// <param name="Active">A value indicating whether the customer role is active</param>
        /// <param name="Deleted">A value indicating whether the customer role has been deleted</param>
        /// <returns>Customer role</returns>
        public override DBCustomerRole UpdateCustomerRole(int CustomerRoleID, string Name,
           bool FreeShipping, bool TaxExempt, bool Active, bool Deleted)
        {
            DBCustomerRole customerRole = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerRoleUpdate");
            db.AddInParameter(dbCommand, "CustomerRoleID", DbType.Int32, CustomerRoleID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "FreeShipping", DbType.Boolean, FreeShipping);
            db.AddInParameter(dbCommand, "TaxExempt", DbType.Boolean, TaxExempt);
            db.AddInParameter(dbCommand, "Active", DbType.Boolean, Active);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, Deleted);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                customerRole = GetCustomerRoleByID(CustomerRoleID);

            return customerRole;
        }

        /// <summary>
        /// Adds a customer to role
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="CustomerRoleID">Customer role identifier</param>
        public override void AddCustomerToRole(int CustomerID, int CustomerRoleID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Customer_CustomerRole_MappingInsert");
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, CustomerID);
            db.AddInParameter(dbCommand, "CustomerRoleID", DbType.Int32, CustomerRoleID);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Removes a customer from role
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="CustomerRoleID">Customer role identifier</param>
        public override void RemoveCustomerFromRole(int CustomerID, int CustomerRoleID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Customer_CustomerRole_MappingDelete");
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, CustomerID);
            db.AddInParameter(dbCommand, "CustomerRoleID", DbType.Int32, CustomerRoleID);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Adds a discount to a customer role
        /// </summary>
        /// <param name="CustomerRoleID">Customer role identifier</param>
        /// <param name="DiscountID">Discount identifier</param>
        public override void AddDiscountToCustomerRole(int CustomerRoleID, int DiscountID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerRole_Discount_MappingInsert");
            db.AddInParameter(dbCommand, "CustomerRoleID", DbType.Int32, CustomerRoleID);
            db.AddInParameter(dbCommand, "DiscountID", DbType.Int32, DiscountID);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Removes a discount from a customer role
        /// </summary>
        /// <param name="CustomerRoleID">Customer role identifier</param>
        /// <param name="DiscountID">Discount identifier</param>
        public override void RemoveDiscountFromCustomerRole(int CustomerRoleID, int DiscountID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerRole_Discount_MappingDelete");
            db.AddInParameter(dbCommand, "CustomerRoleID", DbType.Int32, CustomerRoleID);
            db.AddInParameter(dbCommand, "DiscountID", DbType.Int32, DiscountID);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a customer roles assigned to discount
        /// </summary>
        /// <param name="DiscountID">Discount identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Customer roles</returns>
        public override DBCustomerRoleCollection GetCustomerRolesByDiscountID(int DiscountID, bool showHidden)
        {
            DBCustomerRoleCollection customerRoleCollection = new DBCustomerRoleCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerRoleLoadByDiscountID");
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            db.AddInParameter(dbCommand, "DiscountID", DbType.Int32, DiscountID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBCustomerRole customerRole = GetCustomerRoleFromReader(dataReader);
                    customerRoleCollection.Add(customerRole);
                }
            }

            return customerRoleCollection;
        }

        /// <summary>
        /// Gets a customer session
        /// </summary>
        /// <param name="CustomerSessionGUID">Customer session GUID</param>
        /// <returns>Customer session</returns>
        public override DBCustomerSession GetCustomerSessionByGUID(Guid CustomerSessionGUID)
        {
            DBCustomerSession customerSession = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerSessionLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "CustomerSessionGUID", DbType.Guid, CustomerSessionGUID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    customerSession = GetCustomerSessionFromReader(dataReader);
                }
            }

            return customerSession;
        }

        /// <summary>
        /// Gets a customer session by customer identifier
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <returns>Customer session</returns>
        public override DBCustomerSession GetCustomerSessionByCustomerID(int CustomerID)
        {
            DBCustomerSession customerSession = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerSessionLoadByCustomerID");
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, CustomerID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    customerSession = GetCustomerSessionFromReader(dataReader);
                }
            }

            return customerSession;
        }

        /// <summary>
        /// Deletes a customer session
        /// </summary>
        /// <param name="CustomerSessionGUID">Customer session GUID</param>
        public override void DeleteCustomerSession(Guid CustomerSessionGUID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerSessionDelete");
            db.AddInParameter(dbCommand, "CustomerSessionGUID", DbType.Guid, CustomerSessionGUID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets all customer sessions
        /// </summary>
        /// <returns>Customer session collection</returns>
        public override DBCustomerSessionCollection GetAllCustomerSessions()
        {
            DBCustomerSessionCollection customerSessionCollection = new DBCustomerSessionCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerSessionLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBCustomerSession customerSession = GetCustomerSessionFromReader(dataReader);
                    customerSessionCollection.Add(customerSession);
                }
            }

            return customerSessionCollection;
        }

        /// <summary>
        /// Inserts a customer session
        /// </summary>
        /// <param name="CustomerSessionGUID">Customer session GUID</param>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="LastAccessed">The last accessed date and time</param>
        /// <param name="IsExpired">A value indicating whether the customer session is expired</param>
        /// <returns>Customer session</returns>
        public override DBCustomerSession InsertCustomerSession(Guid CustomerSessionGUID, int CustomerID, DateTime LastAccessed, bool IsExpired)
        {
            DBCustomerSession customerSession = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerSessionInsert");
            db.AddInParameter(dbCommand, "CustomerSessionGUID", DbType.Guid, CustomerSessionGUID);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, CustomerID);
            db.AddInParameter(dbCommand, "LastAccessed", DbType.DateTime, LastAccessed);
            db.AddInParameter(dbCommand, "IsExpired", DbType.Boolean, IsExpired);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                customerSession = GetCustomerSessionByGUID(CustomerSessionGUID);
            }
            return customerSession;
        }

        /// <summary>
        /// Updates the customer session
        /// </summary>
        /// <param name="CustomerSessionGUID">Customer session GUID</param>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="LastAccessed">The last accessed date and time</param>
        /// <param name="IsExpired">A value indicating whether the customer session is expired</param>
        /// <returns>Customer session</returns>
        public override DBCustomerSession UpdateCustomerSession(Guid CustomerSessionGUID, int CustomerID, DateTime LastAccessed, bool IsExpired)
        {
            DBCustomerSession customerSession = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerSessionUpdate");
            db.AddInParameter(dbCommand, "CustomerSessionGUID", DbType.Guid, CustomerSessionGUID);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, CustomerID);
            db.AddInParameter(dbCommand, "LastAccessed", DbType.DateTime, LastAccessed);
            db.AddInParameter(dbCommand, "IsExpired", DbType.Boolean, IsExpired);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                customerSession = GetCustomerSessionByGUID(CustomerSessionGUID);

            return customerSession;
        }

        /// <summary>
        /// Gets a report of customers registered from "dateTime" until today
        /// </summary>
        /// <param name="dateFrom">Customer registration date</param>
        /// <returns>Int</returns>
        public override int GetRegisteredCustomersReport(DateTime dateFrom)
        {
            int count;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerRegisteredReport");
            db.AddInParameter(dbCommand, "Date", DbType.DateTime, dateFrom);
            return count = (int)db.ExecuteScalar(dbCommand);
        }
        #endregion
    }
}
