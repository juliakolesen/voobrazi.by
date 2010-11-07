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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.BusinessLogic.Promo.Affiliates;
using NopSolutions.NopCommerce.BusinessLogic.Shipping;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.BusinessLogic.Utils;
using NopSolutions.NopCommerce.Common.Xml;

namespace NopSolutions.NopCommerce.BusinessLogic.CustomerManagement
{
    /// <summary>
    /// Represents a customer
    /// </summary>
    public partial class Customer : BaseEntity
    {
        #region Fields
        private CustomerAttributeCollection customerAttributesCache=null;
        #endregion

        #region Ctor
        /// <summary>
        /// Creates a new instance of the Customer class
        /// </summary>
        public Customer()
        {
        }
        #endregion

        #region Utilities
        private void resetCachedValues()
        {
            customerAttributesCache = null;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the customer identifier
        /// </summary>
        public int CustomerID { get; set; }

        /// <summary>
        /// Gets or sets the customer GUID
        /// </summary>
        public Guid CustomerGUID { get; set; }

        /// <summary>
        /// Gets or sets the email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password hash
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Gets or sets the salt key
        /// </summary>
        public string SaltKey { get; set; }

        /// <summary>
        /// Gets or sets the affiliate identifier
        /// </summary>
        public int AffiliateID { get; set; }

        /// <summary>
        /// Gets or sets the billing address identifier
        /// </summary>
        public int BillingAddressID { get; set; }

        /// <summary>
        /// Gets or sets the shipping address identifier
        /// </summary>
        public int ShippingAddressID { get; set; }

        /// <summary>
        /// Gets or sets the last payment method identifier
        /// </summary>
        public int LastPaymentMethodID { get; set; }

        /// <summary>
        /// Gets or sets the last applied coupon code
        /// </summary>
        public string LastAppliedCouponCode { get; set; }

        /// <summary>
        /// Gets or sets the language identifier
        /// </summary>
        public int LanguageID { get; set; }

        /// <summary>
        /// Gets or sets the currency identifier
        /// </summary>
        public int CurrencyID { get; set; }

        /// <summary>
        /// Gets or sets the tax display type identifier
        /// </summary>
        public int TaxDisplayTypeID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer is tax exempt
        /// </summary>
        public bool IsTaxExempt { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer is administrator
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer is guest
        /// </summary>
        public bool IsGuest { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer is forum moderator
        /// </summary>
        public bool IsForumModerator { get; set; }

        /// <summary>
        /// Gets or sets the forum post count
        /// </summary>
        public int TotalForumPosts { get; set; }

        /// <summary>
        /// Gets or sets the signature
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// Gets or sets the admin comment
        /// </summary>
        public string AdminComment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer is active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer has been deleted
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets the date and time of customer registration
        /// </summary>
        public DateTime RegistrationDate { get; set; }

        /// <summary>
        /// Gets or sets the time zone identifier
        /// </summary>
        public string TimeZoneID { get; set; }

        /// <summary>
        /// Gets or sets the avatar identifier
        /// </summary>
        public int AvatarID { get; set; }

        #endregion

        #region Custom Properties

        /// <summary>
        /// Gets the last billing address
        /// </summary>
        public Affiliate Affiliate
        {
            get
            {
                return AffiliateManager.GetAffiliateByID(AffiliateID);
            }
        }

        /// <summary>
        /// Gets the customer attributes
        /// </summary>
        public CustomerAttributeCollection CustomerAttributes
        {
            get
            {
                if (customerAttributesCache == null)
                    customerAttributesCache = CustomerManager.GetCustomerAttributesByCustomerID(CustomerID);

                return customerAttributesCache;
            }
        }

        /// <summary>
        /// Gets the customer roles
        /// </summary>
        public CustomerRoleCollection CustomerRoles
        {
            get
            {
                return CustomerManager.GetCustomerRolesByCustomerID(CustomerID);
            }
        }

        /// <summary>
        /// Gets the last billing address
        /// </summary>
        public Address BillingAddress
        {
            get
            {
                return CustomerManager.GetAddressByID(BillingAddressID);
            }
        }

        /// <summary>
        /// Gets the last shipping address
        /// </summary>
        public Address ShippingAddress
        {
            get
            {
                return CustomerManager.GetAddressByID(ShippingAddressID);
            }
        }

        /// <summary>
        /// Gets the last payment method
        /// </summary>
        public PaymentMethod LastPaymentMethod
        {
            get
            {
                return PaymentMethodManager.GetPaymentMethodByID(LastPaymentMethodID);
            }
        }

        /// <summary>
        /// Gets the language
        /// </summary>
        public Language Language
        {
            get
            {
                return LanguageManager.GetLanguageByID(LanguageID);
            }
        }

        /// <summary>
        /// Gets the currency
        /// </summary>
        public Currency Currency
        {
            get
            {
                return CurrencyManager.GetCurrencyByID(CurrencyID);
            }
        }

        /// <summary>
        /// Gets the billing addresses
        /// </summary>
        public AddressCollection BillingAddresses
        {
            get
            {
                return CustomerManager.GetAddressesByCustomerID(CustomerID, true);
            }
        }

        /// <summary>
        /// Gets the shipping addresses
        /// </summary>
        public AddressCollection ShippingAddresses
        {
            get
            {
                return CustomerManager.GetAddressesByCustomerID(CustomerID, false);
            }
        }

        /// <summary>
        /// Gets the orders
        /// </summary>
        public OrderCollection Orders
        {
            get
            {
                return OrderManager.GetOrdersByCustomerID(CustomerID);
            }
        }

        /// <summary>
        /// Gets or sets the last shipping option
        /// </summary>
        public ShippingOption LastShippingOption
        {
            get
            {
                ShippingOption shippingOption = null;
                CustomerAttribute lastShippingOptionAttr = this.CustomerAttributes.FindAttribute("LastShippingOption", this.CustomerID);
                if (lastShippingOptionAttr != null && !String.IsNullOrEmpty(lastShippingOptionAttr.Value))
                {
                    using (TextReader tr = new StringReader(lastShippingOptionAttr.Value))
                    {
                        XmlSerializer xmlS = new XmlSerializer(typeof(ShippingOption));
                        shippingOption = (ShippingOption)xmlS.Deserialize(tr);
                    }
                }
                return shippingOption;
            }
            set
            {
                CustomerAttribute lastShippingOptionAttr = this.CustomerAttributes.FindAttribute("LastShippingOption", this.CustomerID);
                if (value != null)
                {
                    StringBuilder sb = new StringBuilder();
                    using (TextWriter tw = new StringWriter(sb))
                    {
                        XmlSerializer xmlS = new XmlSerializer(typeof(ShippingOption));
                        xmlS.Serialize(tw, value);
                        string serialized = sb.ToString();
                        if (lastShippingOptionAttr != null)
                            lastShippingOptionAttr = CustomerManager.UpdateCustomerAttribute(lastShippingOptionAttr.CustomerAttributeID, this.CustomerID, "LastShippingOption", serialized);
                        else
                            lastShippingOptionAttr = CustomerManager.InsertCustomerAttribute(this.CustomerID, "LastShippingOption", serialized);
                    }
                }
                else
                {
                    if (lastShippingOptionAttr != null)
                        CustomerManager.DeleteCustomerAttribute(lastShippingOptionAttr.CustomerAttributeID);
                }

                resetCachedValues();
            }
        }

        /// <summary>
        /// Gets the tax display type
        /// </summary>
        public TaxDisplayTypeEnum TaxDisplayType
        {
            get
            {
                return (TaxDisplayTypeEnum)TaxDisplayTypeID;
            }
            set
            {
                TaxDisplayTypeID = (int)value;
            }
        }
        
        /// <summary>
        /// Gets the avatar
        /// </summary>
        public Picture Avatar
        {
            get
            {
                return PictureManager.GetPictureByID(AvatarID);
            }
        }

        /// <summary>
        /// Gets the customer full name
        /// </summary>
        public string FullName
        {
            get
            {
                if (String.IsNullOrEmpty(this.FirstName))
                    return this.LastName;
                else
                    return string.Format("{0} {1}", this.FirstName, this.LastName);
            }
        }

        /// <summary>
        /// Gets or sets the gender
        /// </summary>
        public string Gender
        {
            get
            {
                CustomerAttributeCollection customerAttributes = this.CustomerAttributes;
                CustomerAttribute genderAttr = customerAttributes.FindAttribute("Gender", this.CustomerID);
                if (genderAttr != null)
                    return genderAttr.Value;
                else
                    return string.Empty;
            }
            set
            {
                CustomerAttributeCollection customerAttributes = this.CustomerAttributes;
                CustomerAttribute genderAttr = customerAttributes.FindAttribute("Gender", this.CustomerID);
                if (genderAttr != null)
                    genderAttr = CustomerManager.UpdateCustomerAttribute(genderAttr.CustomerAttributeID, genderAttr.CustomerID, "Gender", value);
                else
                    genderAttr = CustomerManager.InsertCustomerAttribute(this.CustomerID, "Gender", value);

                resetCachedValues();
            }
        }

        /// <summary>
        /// Gets or sets the first name
        /// </summary>
        public string FirstName
        {
            get
            {
                CustomerAttributeCollection customerAttributes = this.CustomerAttributes;
                CustomerAttribute firstNameAttr = customerAttributes.FindAttribute("FirstName", this.CustomerID);
                if (firstNameAttr != null)
                    return firstNameAttr.Value;
                else
                    return string.Empty;
            }
            set
            {
                if (value == null)
                    value = string.Empty;
                value = value.Trim();

                CustomerAttributeCollection customerAttributes = this.CustomerAttributes;
                CustomerAttribute firstNameAttr = customerAttributes.FindAttribute("FirstName", this.CustomerID);
                if (firstNameAttr != null)
                    firstNameAttr = CustomerManager.UpdateCustomerAttribute(firstNameAttr.CustomerAttributeID, firstNameAttr.CustomerID, "FirstName", value);
                else
                    firstNameAttr = CustomerManager.InsertCustomerAttribute(this.CustomerID, "FirstName", value);

                resetCachedValues();
            }
        }

        /// <summary>
        /// Gets or sets the last name
        /// </summary>
        public string LastName
        {
            get
            {
                CustomerAttributeCollection customerAttributes = this.CustomerAttributes;
                CustomerAttribute lastNameAttr = customerAttributes.FindAttribute("LastName", this.CustomerID);
                if (lastNameAttr != null)
                    return lastNameAttr.Value;
                else
                    return string.Empty;
            }
            set
            {
                if (value == null)
                    value = string.Empty;
                value = value.Trim();

                CustomerAttributeCollection customerAttributes = this.CustomerAttributes;
                CustomerAttribute lastNameAttr = customerAttributes.FindAttribute("LastName", this.CustomerID);
                if (lastNameAttr != null)
                    lastNameAttr = CustomerManager.UpdateCustomerAttribute(lastNameAttr.CustomerAttributeID, lastNameAttr.CustomerID, "LastName", value);
                else
                    lastNameAttr = CustomerManager.InsertCustomerAttribute(this.CustomerID, "LastName", value);

                resetCachedValues();
            }
        }

        /// <summary>
        /// Gets or sets the date of birth
        /// </summary>
        public DateTime? DateOfBirth
        {
            get
            {
                CustomerAttributeCollection customerAttributes = this.CustomerAttributes;
                CustomerAttribute dateOfBirthAttr = customerAttributes.FindAttribute("DateOfBirth", this.CustomerID);
                if (dateOfBirthAttr != null)
                {
                    try
                    {
                        return XmlHelper.DeserializeDateTime(dateOfBirthAttr.Value);
                    }
                    catch
                    {
                        return null;
                    }
                }
                else
                    return null;
            }
            set
            {
                CustomerAttributeCollection customerAttributes = this.CustomerAttributes;
                CustomerAttribute dateOfBirthAttr = customerAttributes.FindAttribute("DateOfBirth", this.CustomerID);

                if (dateOfBirthAttr != null)
                {
                    if (value.HasValue)
                    {
                        dateOfBirthAttr = CustomerManager.UpdateCustomerAttribute(dateOfBirthAttr.CustomerAttributeID, dateOfBirthAttr.CustomerID, "DateOfBirth", XmlHelper.SerializeDateTime(value.Value));
                    }
                    else
                    {
                        CustomerManager.DeleteCustomerAttribute(dateOfBirthAttr.CustomerAttributeID);
                    }
                }
                else
                {
                    if (value.HasValue)
                    {
                        dateOfBirthAttr = CustomerManager.InsertCustomerAttribute(this.CustomerID, "DateOfBirth", XmlHelper.SerializeDateTime(value.Value));
                    }
                }

                resetCachedValues();
            }
        }

        /// <summary>
        /// Gets or sets the company
        /// </summary>
        public string Company
        {
            get
            {
                CustomerAttributeCollection customerAttributes = this.CustomerAttributes;
                CustomerAttribute companyAttr = customerAttributes.FindAttribute("Company", this.CustomerID);
                if (companyAttr != null)
                    return companyAttr.Value;
                else
                    return string.Empty;
            }
            set
            {
                if (value == null)
                    value = string.Empty;
                value = value.Trim();

                CustomerAttributeCollection customerAttributes = this.CustomerAttributes;
                CustomerAttribute companyAttr = customerAttributes.FindAttribute("Company", this.CustomerID);
                if (companyAttr != null)
                    companyAttr = CustomerManager.UpdateCustomerAttribute(companyAttr.CustomerAttributeID, companyAttr.CustomerID, "Company", value);
                else
                    companyAttr = CustomerManager.InsertCustomerAttribute(this.CustomerID, "Company", value);
                
                resetCachedValues();
            }
        }

        /// <summary>
        /// Gets or sets the street address
        /// </summary>
        public string StreetAddress
        {
            get
            {
                CustomerAttributeCollection customerAttributes = this.CustomerAttributes;
                CustomerAttribute streetAddressAttr = customerAttributes.FindAttribute("StreetAddress", this.CustomerID);
                if (streetAddressAttr != null)
                    return streetAddressAttr.Value;
                else
                    return string.Empty;
            }
            set
            {
                if (value == null)
                    value = string.Empty;
                value = value.Trim();

                CustomerAttributeCollection customerAttributes = this.CustomerAttributes;
                CustomerAttribute streetAddressAttr = customerAttributes.FindAttribute("StreetAddress", this.CustomerID);
                if (streetAddressAttr != null)
                    streetAddressAttr = CustomerManager.UpdateCustomerAttribute(streetAddressAttr.CustomerAttributeID, streetAddressAttr.CustomerID, "StreetAddress", value);
                else
                    streetAddressAttr = CustomerManager.InsertCustomerAttribute(this.CustomerID, "StreetAddress", value);

                resetCachedValues();
            }
        }

        /// <summary>
        /// Gets or sets the street address 2
        /// </summary>
        public string StreetAddress2
        {
            get
            {
                CustomerAttributeCollection customerAttributes = this.CustomerAttributes;
                CustomerAttribute streetAddress2Attr = customerAttributes.FindAttribute("StreetAddress2", this.CustomerID);
                if (streetAddress2Attr != null)
                    return streetAddress2Attr.Value;
                else
                    return string.Empty;
            }
            set
            {
                if (value == null)
                    value = string.Empty;
                value = value.Trim();

                CustomerAttributeCollection customerAttributes = this.CustomerAttributes;
                CustomerAttribute streetAddress2Attr = customerAttributes.FindAttribute("StreetAddress2", this.CustomerID);
                if (streetAddress2Attr != null)
                    streetAddress2Attr = CustomerManager.UpdateCustomerAttribute(streetAddress2Attr.CustomerAttributeID, streetAddress2Attr.CustomerID, "StreetAddress2", value);
                else
                    streetAddress2Attr = CustomerManager.InsertCustomerAttribute(this.CustomerID, "StreetAddress2", value);

                resetCachedValues();
            }
        }

        /// <summary>
        /// Gets or sets the zip/postal code
        /// </summary>
        public string ZipPostalCode
        {
            get
            {
                CustomerAttributeCollection customerAttributes = this.CustomerAttributes;
                CustomerAttribute zipPostalCodeAttr = customerAttributes.FindAttribute("ZipPostalCode", this.CustomerID);
                if (zipPostalCodeAttr != null)
                    return zipPostalCodeAttr.Value;
                else
                    return string.Empty;
            }
            set
            {
                if (value == null)
                    value = string.Empty;
                value = value.Trim();

                CustomerAttributeCollection customerAttributes = this.CustomerAttributes;
                CustomerAttribute zipPostalCodeAttr = customerAttributes.FindAttribute("ZipPostalCode", this.CustomerID);
                if (zipPostalCodeAttr != null)
                    zipPostalCodeAttr = CustomerManager.UpdateCustomerAttribute(zipPostalCodeAttr.CustomerAttributeID, zipPostalCodeAttr.CustomerID, "ZipPostalCode", value);
                else
                    zipPostalCodeAttr = CustomerManager.InsertCustomerAttribute(this.CustomerID, "ZipPostalCode", value);

                resetCachedValues();
            }
        }

        /// <summary>
        /// Gets or sets the city
        /// </summary>
        public string City
        {
            get
            {
                CustomerAttributeCollection customerAttributes = this.CustomerAttributes;
                CustomerAttribute cityAttr = customerAttributes.FindAttribute("City", this.CustomerID);
                if (cityAttr != null)
                    return cityAttr.Value;
                else
                    return string.Empty;
            }
            set
            {
                if (value == null)
                    value = string.Empty;
                value = value.Trim();

                CustomerAttributeCollection customerAttributes = this.CustomerAttributes;
                CustomerAttribute cityAttr = customerAttributes.FindAttribute("City", this.CustomerID);
                if (cityAttr != null)
                    cityAttr = CustomerManager.UpdateCustomerAttribute(cityAttr.CustomerAttributeID, cityAttr.CustomerID, "City", value);
                else
                    cityAttr = CustomerManager.InsertCustomerAttribute(this.CustomerID, "City", value);

                resetCachedValues();
            }
        }

        /// <summary>
        /// Gets or sets the phone number
        /// </summary>
        public string PhoneNumber
        {
            get
            {
                CustomerAttributeCollection customerAttributes = this.CustomerAttributes;
                CustomerAttribute phoneNumberAttr = customerAttributes.FindAttribute("PhoneNumber", this.CustomerID);
                if (phoneNumberAttr != null)
                    return phoneNumberAttr.Value;
                else
                    return string.Empty;
            }
            set
            {
                if (value == null)
                    value = string.Empty;
                value = value.Trim();

                CustomerAttributeCollection customerAttributes = this.CustomerAttributes;
                CustomerAttribute phoneNumberAttr = customerAttributes.FindAttribute("PhoneNumber", this.CustomerID);
                if (phoneNumberAttr != null)
                    phoneNumberAttr = CustomerManager.UpdateCustomerAttribute(phoneNumberAttr.CustomerAttributeID, phoneNumberAttr.CustomerID, "PhoneNumber", value);
                else
                    phoneNumberAttr = CustomerManager.InsertCustomerAttribute(this.CustomerID, "PhoneNumber", value);

                resetCachedValues();
            }
        }

        /// <summary>
        /// Gets or sets the fax number
        /// </summary>
        public string FaxNumber
        {
            get
            {
                CustomerAttributeCollection customerAttributes = this.CustomerAttributes;
                CustomerAttribute faxNumberAttr = customerAttributes.FindAttribute("FaxNumber", this.CustomerID);
                if (faxNumberAttr != null)
                    return faxNumberAttr.Value;
                else
                    return string.Empty;
            }
            set
            {
                if (value == null)
                    value = string.Empty;
                value = value.Trim();

                CustomerAttributeCollection customerAttributes = this.CustomerAttributes;
                CustomerAttribute faxNumberAttr = customerAttributes.FindAttribute("FaxNumber", this.CustomerID);
                if (faxNumberAttr != null)
                    faxNumberAttr = CustomerManager.UpdateCustomerAttribute(faxNumberAttr.CustomerAttributeID, faxNumberAttr.CustomerID, "FaxNumber", value);
                else
                    faxNumberAttr = CustomerManager.InsertCustomerAttribute(this.CustomerID, "FaxNumber", value);

                resetCachedValues();
            }
        }

        /// <summary>
        /// Gets or sets the country identifier
        /// </summary>
        public int CountryID
        {
            get
            {
                CustomerAttributeCollection customerAttributes = this.CustomerAttributes;
                CustomerAttribute countryIDAttr = customerAttributes.FindAttribute("CountryID", this.CustomerID);
                if (countryIDAttr != null)
                {
                    int _countryID = 0;
                    int.TryParse(countryIDAttr.Value, out _countryID);
                    return _countryID;
                }
                else
                    return 0;
            }
            set
            {
                CustomerAttributeCollection customerAttributes = this.CustomerAttributes;
                CustomerAttribute countryIDAttr = customerAttributes.FindAttribute("CountryID", this.CustomerID);
                if (countryIDAttr != null)
                    countryIDAttr = CustomerManager.UpdateCustomerAttribute(countryIDAttr.CustomerAttributeID, countryIDAttr.CustomerID, "CountryID", value.ToString());
                else
                    countryIDAttr = CustomerManager.InsertCustomerAttribute(this.CustomerID, "CountryID", value.ToString());

                resetCachedValues();
            }
        }

        /// <summary>
        /// Gets or sets the state/province identifier
        /// </summary>
        public int StateProvinceID
        {
            get
            {
                CustomerAttributeCollection customerAttributes = this.CustomerAttributes;
                CustomerAttribute stateProvinceIDAttr = customerAttributes.FindAttribute("StateProvinceID", this.CustomerID);
                if (stateProvinceIDAttr != null)
                {
                    int _stateProvinceID = 0;
                    int.TryParse(stateProvinceIDAttr.Value, out _stateProvinceID);
                    return _stateProvinceID;
                }
                else
                    return 0;
            }
            set
            {
                CustomerAttributeCollection customerAttributes = this.CustomerAttributes;
                CustomerAttribute stateProvinceIDAttr = customerAttributes.FindAttribute("StateProvinceID", this.CustomerID);
                if (stateProvinceIDAttr != null)
                    stateProvinceIDAttr = CustomerManager.UpdateCustomerAttribute(stateProvinceIDAttr.CustomerAttributeID, stateProvinceIDAttr.CustomerID, "StateProvinceID", value.ToString());
                else
                    stateProvinceIDAttr = CustomerManager.InsertCustomerAttribute(this.CustomerID, "StateProvinceID", value.ToString());

                resetCachedValues();
            }
        }

        /// <summary>
        /// Gets or sets the value indivating whether customer is agree to receive newsletters
        /// </summary>
        public bool ReceiveNewsletter
        {
            get
            {
                CustomerAttributeCollection customerAttributes = this.CustomerAttributes;
                CustomerAttribute newsletterAttr = customerAttributes.FindAttribute("Newsletter", this.CustomerID);
                if (newsletterAttr != null)
                {
                    bool _newsLetters = false;
                    bool.TryParse(newsletterAttr.Value, out _newsLetters);
                    return _newsLetters;
                }
                else
                    return false;
            }
            set
            {
                CustomerAttributeCollection customerAttributes = this.CustomerAttributes;
                CustomerAttribute newsletterAttr = customerAttributes.FindAttribute("Newsletter", this.CustomerID);
                if (newsletterAttr != null)
                    newsletterAttr = CustomerManager.UpdateCustomerAttribute(newsletterAttr.CustomerAttributeID, newsletterAttr.CustomerID, "Newsletter", value.ToString());
                else
                    newsletterAttr = CustomerManager.InsertCustomerAttribute(this.CustomerID, "Newsletter", value.ToString());

                resetCachedValues();
            }
        }

        /// <summary>
        /// Gets or sets the password recovery token
        /// </summary>
        public string PasswordRecoveryToken
        {
            get
            {
                CustomerAttributeCollection customerAttributes = this.CustomerAttributes;
                CustomerAttribute passwordRecoveryAttr = customerAttributes.FindAttribute("PasswordRecoveryToken", this.CustomerID);
                if (passwordRecoveryAttr != null)
                    return passwordRecoveryAttr.Value;
                else
                    return string.Empty;
            }
            set
            {
                CustomerAttributeCollection customerAttributes = this.CustomerAttributes;
                CustomerAttribute passwordRecoveryAttr = customerAttributes.FindAttribute("PasswordRecoveryToken", this.CustomerID);

                if (passwordRecoveryAttr != null)
                {
                    if (!String.IsNullOrEmpty(value))
                    {
                        passwordRecoveryAttr = CustomerManager.UpdateCustomerAttribute(passwordRecoveryAttr.CustomerAttributeID, passwordRecoveryAttr.CustomerID, "PasswordRecoveryToken", value);
                    }
                    else
                    {
                        CustomerManager.DeleteCustomerAttribute(passwordRecoveryAttr.CustomerAttributeID);
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(value))
                    {
                        passwordRecoveryAttr = CustomerManager.InsertCustomerAttribute(this.CustomerID, "PasswordRecoveryToken", value);
                    }
                }
                resetCachedValues();
            }
        }

        /// <summary>
        /// Gets or sets the account activation token
        /// </summary>
        public string AccountActivationToken
        {
            get
            {
                CustomerAttributeCollection customerAttributes = this.CustomerAttributes;
                CustomerAttribute accountActivationAttr = customerAttributes.FindAttribute("AccountActivationToken", this.CustomerID);
                if (accountActivationAttr != null)
                    return accountActivationAttr.Value;
                else
                    return string.Empty;
            }
            set
            {
                CustomerAttributeCollection customerAttributes = this.CustomerAttributes;
                CustomerAttribute accountActivationAttr = customerAttributes.FindAttribute("AccountActivationToken", this.CustomerID);

                if (accountActivationAttr != null)
                {
                    if (!String.IsNullOrEmpty(value))
                    {
                        accountActivationAttr = CustomerManager.UpdateCustomerAttribute(accountActivationAttr.CustomerAttributeID, accountActivationAttr.CustomerID, "AccountActivationToken", value);
                    }
                    else
                    {
                        CustomerManager.DeleteCustomerAttribute(accountActivationAttr.CustomerAttributeID);
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(value))
                    {
                        accountActivationAttr = CustomerManager.InsertCustomerAttribute(this.CustomerID, "AccountActivationToken", value);
                    }
                }
                resetCachedValues();
            }
        }

        #endregion
    }
}