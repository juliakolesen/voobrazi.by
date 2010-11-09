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


namespace NopSolutions.NopCommerce.DataAccess.CustomerManagement
{
    /// <summary>
    /// Acts as a base class for deriving custom customer provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/CustomerProvider")]
    public abstract partial class DBCustomerProvider : BaseDBProvider
    {
        #region Methods

        /// <summary>
        /// Deletes an address by address identifier 
        /// </summary>
        /// <param name="AddressID">Address identifier</param>
        public abstract void DeleteAddress(int AddressID);

        /// <summary>
        /// Gets an address by address identifier
        /// </summary>
        /// <param name="AddressID">Address identifier</param>
        /// <returns>Address</returns>
        public abstract DBAddress GetAddressByID(int AddressID);

        /// <summary>
        /// Gets a collection of addresses by customer identifier
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="GetBillingAddresses">Gets or sets a value indicating whether the addresses are billing or shipping</param>
        /// <returns>A collection of addresses</returns>
        public abstract DBAddressCollection GetAddressesByCustomerID(int CustomerID, bool GetBillingAddresses);

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
        public abstract DBAddress InsertAddress(int CustomerID, bool IsBillingAddress, string FirstName, string LastName,
            string PhoneNumber, string Email, string FaxNumber, string Company, string Address1,
            string Address2, string City, int StateProvinceID, string ZipPostalCode,
            int CountryID, DateTime CreatedOn, DateTime UpdatedOn);

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
        public abstract DBAddress UpdateAddress(int AddressID, int CustomerID, bool IsBillingAddress, string FirstName, string LastName,
            string PhoneNumber, string Email, string FaxNumber, string Company,
            string Address1, string Address2, string City, int StateProvinceID,
            string ZipPostalCode, int CountryID, DateTime CreatedOn, DateTime UpdatedOn);

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
        public abstract DBCustomerCollection GetAllCustomers(DateTime? RegistrationFrom, DateTime? RegistrationTo,
            string Email, string Username, bool DontLoadGuestCustomers, 
            int PageSize, int PageIndex, out int TotalRecords);

        /// <summary>
        /// Gets all customers for newsletters
        /// </summary>
        /// <returns>Customer collection</returns>
        public abstract DBCustomerCollection GetAllCustomersForNewsLetters();

        /// <summary>
        /// Gets all customers by affiliate identifier
        /// </summary>
        /// <param name="AffiliateID">Affiliate identifier</param>
        /// <returns>Customer collection</returns>
        public abstract DBCustomerCollection GetAffiliatedCustomers(int AffiliateID);

        /// <summary>
        /// Gets all customers by customer role id
        /// </summary>
        /// <param name="CustomerRoleID">Customer role identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Customer collection</returns>
        public abstract DBCustomerCollection GetCustomersByCustomerRoleID(int CustomerRoleID, bool showHidden);

        /// <summary>
        /// Gets a customer by email
        /// </summary>
        /// <param name="Email">Customer Email</param>
        /// <returns>A customer</returns>
        public abstract DBCustomer GetCustomerByEmail(string Email);

        /// <summary>
        /// Gets a customer by username
        /// </summary>
        /// <param name="Username">Customer username</param>
        /// <returns>A customer</returns>
        public abstract DBCustomer GetCustomerByUsername(string Username);

        /// <summary>
        /// Gets a customer
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <returns>A customer</returns>
        public abstract DBCustomer GetCustomerByID(int CustomerID);

        /// <summary>
        /// Gets a customer by GUID
        /// </summary>
        /// <param name="CustomerGUID">Customer GUID</param>
        /// <returns>A customer</returns>
        public abstract DBCustomer GetCustomerByGUID(Guid CustomerGUID);

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
        public abstract DBCustomer AddCustomer(Guid CustomerGUID, string Email,
            string Username, string passwordHash, string saltKey,
            int AffiliateID, int BillingAddressID,
            int ShippingAddressID, int LastPaymentMethodID,
            string LastAppliedCouponCode, int LanguageID, int CurrencyID, int TaxDisplayTypeID,
            bool IsTaxExempt, bool IsAdmin, bool IsGuest, bool IsForumModerator,
            int TotalForumPosts, string Signature, string AdminComment, bool Active, bool Deleted, 
            DateTime RegistrationDate, string TimeZoneID, int AvatarID);

        /// <summary>
        /// Updates the customer
        /// </summary>
        /// <param name="CustomerID">The customer identifier</param>
        /// <param name="CustomerGUID">The customer identifier</param>
        /// <param name="Email">The email</param>
        /// <param name="Username">The username</param>
        /// <param name="PasswordHash">The password hash</param>
        /// <param name="SaltKey">The salt key</param>
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
        public abstract DBCustomer UpdateCustomer(int CustomerID, Guid CustomerGUID, string Email,
            string Username, string PasswordHash, string SaltKey,
            int AffiliateID, int BillingAddressID,
            int ShippingAddressID, int LastPaymentMethodID,
            string LastAppliedCouponCode, int LanguageID,
            int CurrencyID, int TaxDisplayTypeID, bool IsTaxExempt, 
            bool IsAdmin, bool IsGuest, bool IsForumModerator,
            int TotalForumPosts, string Signature, string AdminComment, bool Active,
            bool Deleted, DateTime RegistrationDate, string TimeZoneID, int AvatarID);

        /// <summary>
        /// Deletes a customer attribute
        /// </summary>
        /// <param name="CustomerAttributeID">Customer attribute identifier</param>
        public abstract void DeleteCustomerAttribute(int CustomerAttributeID);

        /// <summary>
        /// Gets a customer attribute
        /// </summary>
        /// <param name="CustomerAttributeID">Customer attribute identifier</param>
        /// <returns>A customer attribute</returns>
        public abstract DBCustomerAttribute GetCustomerAttributeByID(int CustomerAttributeID);

        /// <summary>
        /// Gets a collection of customer attributes by customer identifier
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <returns>Customer attributes</returns>
        public abstract DBCustomerAttributeCollection GetCustomerAttributesByCustomerID(int CustomerID);

        /// <summary>
        /// Inserts a customer attribute
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="Key">An attribute key</param>
        /// <param name="Value">An attribute value</param>
        /// <returns>A customer attribute</returns>
        public abstract DBCustomerAttribute InsertCustomerAttribute(int CustomerID, string Key, string Value);

        /// <summary>
        /// Updates the customer attribute
        /// </summary>
        /// <param name="CustomerAttributeID">Customer attribute identifier</param>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="Key">An attribute key</param>
        /// <param name="Value">An attribute value</param>
        /// <returns>A customer attribute</returns>
        public abstract DBCustomerAttribute UpdateCustomerAttribute(int CustomerAttributeID, int CustomerID, string Key, string Value);

        /// <summary>
        /// Gets customer roles by customer identifier
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Customer role collection</returns>
        public abstract DBCustomerRoleCollection GetCustomerRolesByCustomerID(int CustomerID, bool showHidden);

        /// <summary>
        /// Gets all customer roles
        /// </summary>
        /// <returns>Customer role collection</returns>
        public abstract DBCustomerRoleCollection GetAllCustomerRoles(bool showHidden);

        /// <summary>
        /// Gets a customer role
        /// </summary>
        /// <param name="CustomerRoleID">Customer role identifier</param>
        /// <returns>Customer role</returns>
        public abstract DBCustomerRole GetCustomerRoleByID(int CustomerRoleID);

        /// <summary>
        /// Inserts a customer role
        /// </summary>
        /// <param name="Name">The customer role name</param>
        /// <param name="FreeShipping">A value indicating whether the customer role is marked as free shiping</param>
        /// <param name="TaxExempt">A value indicating whether the customer role is marked as tax exempt</param>
        /// <param name="Active">A value indicating whether the customer role is active</param>
        /// <param name="Deleted">A value indicating whether the customer role has been deleted</param>
        /// <returns>Customer role</returns>
        public abstract DBCustomerRole InsertCustomerRole(string Name, bool FreeShipping, bool TaxExempt, bool Active, bool Deleted);

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
        public abstract DBCustomerRole UpdateCustomerRole(int CustomerRoleID, string Name,
           bool FreeShipping, bool TaxExempt, bool Active, bool Deleted);

        /// <summary>
        /// Adds a customer to role
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="CustomerRoleID">Customer role identifier</param>
        public abstract void AddCustomerToRole(int CustomerID, int CustomerRoleID);

        /// <summary>
        /// Removes a customer from role
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="CustomerRoleID">Customer role identifier</param>
        public abstract void RemoveCustomerFromRole(int CustomerID, int CustomerRoleID);

        /// <summary>
        /// Adds a discount to a customer role
        /// </summary>
        /// <param name="CustomerRoleID">Customer role identifier</param>
        /// <param name="DiscountID">Discount identifier</param>
        public abstract void AddDiscountToCustomerRole(int CustomerRoleID, int DiscountID);

        /// <summary>
        /// Removes a discount from a customer role
        /// </summary>
        /// <param name="CustomerRoleID">Customer role identifier</param>
        /// <param name="DiscountID">Discount identifier</param>
        public abstract void RemoveDiscountFromCustomerRole(int CustomerRoleID, int DiscountID);

        /// <summary>
        /// Gets a customer roles assigned to discount
        /// </summary>
        /// <param name="DiscountID">Discount identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Customer roles</returns>
        public abstract DBCustomerRoleCollection GetCustomerRolesByDiscountID(int DiscountID, bool showHidden);

        /// <summary>
        /// Gets a customer session
        /// </summary>
        /// <param name="CustomerSessionGUID">Customer session GUID</param>
        /// <returns>Customer session</returns>
        public abstract DBCustomerSession GetCustomerSessionByGUID(Guid CustomerSessionGUID);

        /// <summary>
        /// Gets a customer session by customer identifier
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <returns>Customer session</returns>
        public abstract DBCustomerSession GetCustomerSessionByCustomerID(int CustomerID);

        /// <summary>
        /// Deletes a customer session
        /// </summary>
        /// <param name="CustomerSessionGUID">Customer session GUID</param>
        public abstract void DeleteCustomerSession(Guid CustomerSessionGUID);

        /// <summary>
        /// Gets all customer sessions
        /// </summary>
        /// <returns>Customer session collection</returns>
        public abstract DBCustomerSessionCollection GetAllCustomerSessions();

        /// <summary>
        /// Inserts a customer session
        /// </summary>
        /// <param name="CustomerSessionGUID">Customer session GUID</param>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="LastAccessed">The last accessed date and time</param>
        /// <param name="IsExpired">A value indicating whether the customer session is expired</param>
        /// <returns>Customer session</returns>
        public abstract DBCustomerSession InsertCustomerSession(Guid CustomerSessionGUID, int CustomerID, DateTime LastAccessed, bool IsExpired);

        /// <summary>
        /// Updates the customer session
        /// </summary>
        /// <param name="CustomerSessionGUID">Customer session GUID</param>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="LastAccessed">The last accessed date and time</param>
        /// <param name="IsExpired">A value indicating whether the customer session is expired</param>
        /// <returns>Customer session</returns>
        public abstract DBCustomerSession UpdateCustomerSession(Guid CustomerSessionGUID, int CustomerID, DateTime LastAccessed, bool IsExpired);

        /// <summary>
        /// Gets a report of customers registered from "dateTime" until today
        /// </summary>
        /// <param name="dateFrom">Customer registration date from</param>
        /// <returns>Int</returns>
        public abstract int GetRegisteredCustomersReport(DateTime dateFrom);
        #endregion
    }
}
