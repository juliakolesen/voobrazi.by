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
using System.Globalization;
using System.Threading;
using System.Web;
using NopSolutions.NopCommerce.BusinessLogic.Configuration;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Localization;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.BusinessLogic.Utils;
using NopSolutions.NopCommerce.Common;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.BusinessLogic
{
    /// <summary>
    /// Represents a NopContext
    /// </summary>
    public partial class NopContext
    {
        #region Constants
        private const string CONST_CUSTOMERSESSION = "Nop.CustomerSession";
        private const string CONST_CUSTOMERSESSIONCOOKIE = "Nop.CustomerSessionGUIDCookie";
        #endregion

        #region Fields
        private Customer currentCustomer;
        private bool isAdmin;
        private HttpContext context = HttpContext.Current;
        #endregion

        #region Ctor
        /// <summary>
        /// Creates a new instance of the NopContext class
        /// </summary>
        private NopContext()
        {

        }
        #endregion

        #region Methods
        /// <summary>
        /// Save customer session to data source
        /// </summary>
        /// <returns>Saved customer ssion</returns>
        private CustomerSession SaveSessionToDatabase()
        {
            Guid sessionId = Guid.NewGuid();
            while (CustomerManager.GetCustomerSessionByGUID(sessionId) != null)
                sessionId = Guid.NewGuid();
            CustomerSession session = new CustomerSession();
            int CustomerID = 0;
            if (this.User != null)
            {
                CustomerID = this.User.CustomerID;
            }
            session.CustomerSessionGUID = sessionId;
            session.CustomerID = CustomerID;
            session.LastAccessed = DateTime.UtcNow;
            session.IsExpired = false;
            session = CustomerManager.SaveCustomerSession(session.CustomerSessionGUID, session.CustomerID, session.LastAccessed, session.IsExpired);
            return session;
        }

        /// <summary>
        /// Gets customer session
        /// </summary>
        /// <param name="createInDatabase">Create session in database if no one exists</param>
        /// <returns>Customer session</returns>
        public CustomerSession GetSession(bool createInDatabase)
        {
            return this.GetSession(createInDatabase, null);
        }

        /// <summary>
        /// Gets customer session
        /// </summary>
        /// <param name="createInDatabase">Create session in database if no one exists</param>
        /// <param name="sessionId">Session identifier</param>
        /// <returns>Customer session</returns>
        public CustomerSession GetSession(bool createInDatabase, Guid? sessionId)
        {
            CustomerSession byId = null;
            object obj2 = Current[CONST_CUSTOMERSESSION];
            if (obj2 != null)
                byId = (CustomerSession)obj2;
            if ((byId == null) && (sessionId.HasValue))
            {
                byId = CustomerManager.GetCustomerSessionByGUID(sessionId.Value);
                return byId;
            }
            if (byId == null && createInDatabase)
            {
                byId = SaveSessionToDatabase();
            }
            string customerSessionCookieValue = string.Empty;
            if ((HttpContext.Current.Request.Cookies[CONST_CUSTOMERSESSIONCOOKIE] != null) && (HttpContext.Current.Request.Cookies[CONST_CUSTOMERSESSIONCOOKIE].Value != null))
                customerSessionCookieValue = HttpContext.Current.Request.Cookies[CONST_CUSTOMERSESSIONCOOKIE].Value;
            if ((byId) == null && (!string.IsNullOrEmpty(customerSessionCookieValue)))
            {
                CustomerSession dbCustomerSession = CustomerManager.GetCustomerSessionByGUID(new Guid(customerSessionCookieValue));
                byId = dbCustomerSession;
            }
            Current[CONST_CUSTOMERSESSION] = byId;
            return byId;
        }

        /// <summary>
        /// Saves current session to client
        /// </summary>
        public void SessionSaveToClient()
        {
            if (HttpContext.Current != null && this.Session != null)
                SetCookie(HttpContext.Current.ApplicationInstance, CONST_CUSTOMERSESSIONCOOKIE, this.Session.CustomerSessionGUID.ToString());
        }

        /// <summary>
        /// Reset customer session
        /// </summary>
        public void ResetSession()
        {
            if (HttpContext.Current != null)
                SetCookie(HttpContext.Current.ApplicationInstance, CONST_CUSTOMERSESSIONCOOKIE, string.Empty);
            this.Session = null;
            this.User = null;
            this["Nop.SessionReseted"] = true;
        }

        /// <summary>
        /// Sets cookie
        /// </summary>
        /// <param name="application">Application</param>
        /// <param name="key">Key</param>
        /// <param name="val">Value</param>
        private static void SetCookie(HttpApplication application, string key, string val)
        {
            HttpCookie cookie = new HttpCookie(key);
            cookie.Value = val;
            if (string.IsNullOrEmpty(val))
            {
                cookie.Expires = DateTime.Now.AddMonths(-1);
            }
            else
            {
                cookie.Expires = DateTime.Now.AddHours((double)NopConfig.CookieExpires);
            }
            application.Response.Cookies.Remove(key);
            application.Response.Cookies.Add(cookie);
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets an instance of the NopContext, which can be used to retrieve information about current context.
        /// </summary>
        public static NopContext Current
        {
            get
            {
                if (HttpContext.Current == null)
                    return null;

                if (HttpContext.Current.Items["NopContext"] == null)
                {
                    NopContext context2 = new NopContext();
                    HttpContext.Current.Items.Add("NopContext", context2);
                    return context2;
                }
                return (NopContext)HttpContext.Current.Items["NopContext"];
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the context is running in admin-mode
        /// </summary>
        public bool IsAdmin
        {
            get
            {
                return isAdmin;
            }
            set
            {
                isAdmin = value;
            }
        }

        /// <summary>
        /// Gets or sets an object item in the context by the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key.</returns>
        public object this[string key]
        {
            get
            {
                if (this.context == null)
                {
                    return null;
                }

                if (this.context.Items[key] != null)
                {
                    return this.context.Items[key];
                }
                return null;
            }
            set
            {
                if (this.context != null)
                {
                    this.context.Items.Remove(key);
                    this.context.Items.Add(key, value);

                }
            }
        }

        /// <summary>
        /// Gets or sets the current session
        /// </summary>
        public CustomerSession Session
        {
            get
            {
                return this.GetSession(false);
            }
            set
            {
                Current[CONST_CUSTOMERSESSION] = value;
            }
        }

        /// <summary>
        /// Gets or sets the current user
        /// </summary>
        public Customer User
        {
            get
            {
                return this.currentCustomer;
            }
            set
            {
                this.currentCustomer = value;
            }
        }

        /// <summary>
        /// Get or set current user working currency
        /// </summary>
        public Currency WorkingCurrency
        {
            get
            {
                if (NopContext.Current.IsAdmin)
                {
                    return CurrencyManager.PrimaryStoreCurrency;
                }
                Customer customer = NopContext.Current.User;
                CurrencyCollection publishedCurrencies = CurrencyManager.GetAllCurrencies();
                if (customer != null)
                {
                    Currency customerCurrency = customer.Currency;
                    if (customerCurrency != null)
                        foreach (Currency _currency in publishedCurrencies)
                            if (_currency.CurrencyID == customerCurrency.CurrencyID)
                                return customerCurrency;
                }
                else if (CommonHelper.GetCookieInt("Nop.CustomerCurrency") > 0)
                {
                    Currency customerCurrency = CurrencyManager.GetCurrencyByID(CommonHelper.GetCookieInt("Nop.CustomerCurrency"));
                    if (customerCurrency != null)
                        foreach (Currency _currency in publishedCurrencies)
                            if (_currency.CurrencyID == customerCurrency.CurrencyID)
                                return customerCurrency;
                }
                foreach (Currency _currency in publishedCurrencies)
                    return _currency;
                return CurrencyManager.PrimaryStoreCurrency;
            }
            set
            {
                if (value == null)
                    return;
                Customer customer = NopContext.Current.User;
                if (customer != null)
                {
                    customer = CustomerManager.UpdateCustomer(customer.CustomerID, customer.CustomerGUID,
                        customer.Email, customer.Username, customer.PasswordHash, customer.SaltKey, customer.AffiliateID,
                        customer.BillingAddressID, customer.ShippingAddressID,
                        customer.LastPaymentMethodID, customer.LastAppliedCouponCode,
                        customer.LanguageID, value.CurrencyID, customer.TaxDisplayType,
                        customer.IsTaxExempt, customer.IsAdmin, customer.IsGuest,
                        customer.IsForumModerator, customer.TotalForumPosts,
                        customer.Signature, customer.AdminComment, customer.Active,
                        customer.Deleted, customer.RegistrationDate, customer.TimeZoneID, customer.AvatarID);

                    NopContext.Current.User = customer;
                }
                if (!NopContext.Current.IsAdmin)
                {
                    CommonHelper.SetCookie("Nop.CustomerCurrency", value.CurrencyID.ToString(), new TimeSpan(365, 0, 0, 0, 0));
                }
            }
        }

        /// <summary>
        /// Get or set current user working language
        /// </summary>
        public Language WorkingLanguage
        {
            get
            {
                //if (NopContext.Current.IsAdmin)
                //{
                //    return LocalizationManager.DefaultAdminLanguage;
                //}
                Customer customer = NopContext.Current.User;
                LanguageCollection publishedLanguages = LanguageManager.GetAllLanguages(false);
                if (customer != null)
                {
                    Language customerLanguage = customer.Language;
                    if (customerLanguage != null && customerLanguage.Published)
                        return customerLanguage;
                }
                else if (CommonHelper.GetCookieInt("Nop.CustomerLanguage") > 0)
                {
                    Language customerLanguage = LanguageManager.GetLanguageByID(CommonHelper.GetCookieInt("Nop.CustomerLanguage"));
                    if (customerLanguage != null)
                        if (customerLanguage != null && customerLanguage.Published)
                            return customerLanguage;
                }
                foreach (Language _language in publishedLanguages)
                    return _language;

                throw new NopException("Languages could not be loaded");
            }
            set
            {
                if (value == null)
                    return;
                Customer customer = NopContext.Current.User;
                if (customer != null)
                {
                    customer = CustomerManager.UpdateCustomer(customer.CustomerID, customer.CustomerGUID,
                        customer.Email, customer.Username, customer.PasswordHash, customer.SaltKey, customer.AffiliateID,
                        customer.BillingAddressID, customer.ShippingAddressID,
                        customer.LastPaymentMethodID, customer.LastAppliedCouponCode,
                        value.LanguageID, customer.CurrencyID, customer.TaxDisplayType,
                        customer.IsTaxExempt, customer.IsAdmin, customer.IsGuest,
                        customer.IsForumModerator, customer.TotalForumPosts,
                        customer.Signature, customer.AdminComment, customer.Active, customer.Deleted, customer.RegistrationDate,
                        customer.TimeZoneID, customer.AvatarID);

                    NopContext.Current.User = customer;
                }
                //if (!NopContext.Current.IsAdmin)
                //{
                    CommonHelper.SetCookie("Nop.CustomerLanguage", value.LanguageID.ToString(), new TimeSpan(365, 0, 0, 0, 0));
                //}
            }
        }

        /// <summary>
        /// Get or set current tax display type
        /// </summary>
        public TaxDisplayTypeEnum TaxDisplayType
        {
            get
            {
                //if (NopContext.Current.IsAdmin)
                //{
                //    return TaxManager.TaxDisplayType;
                //}

                if (TaxManager.AllowCustomersToSelectTaxDisplayType)
                {
                    Customer customer = NopContext.Current.User;
                    if (customer != null)
                    {
                        return customer.TaxDisplayType;
                    }
                    else if (CommonHelper.GetCookieInt("Nop.TaxDisplayTypeID") > 0)
                    {
                        return (TaxDisplayTypeEnum)Enum.ToObject(typeof(TaxDisplayTypeEnum), CommonHelper.GetCookieInt("Nop.TaxDisplayTypeID"));
                    }
                }
                return TaxManager.TaxDisplayType;
            }
            set
            {
                if (!TaxManager.AllowCustomersToSelectTaxDisplayType)
                    return;

                Customer customer = NopContext.Current.User;
                if (customer != null)
                {
                    customer = CustomerManager.UpdateCustomer(customer.CustomerID, customer.CustomerGUID,
                        customer.Email, customer.Username, customer.PasswordHash, customer.SaltKey, customer.AffiliateID,
                        customer.BillingAddressID, customer.ShippingAddressID,
                        customer.LastPaymentMethodID, customer.LastAppliedCouponCode,
                        customer.LanguageID, customer.CurrencyID, value,
                        customer.IsTaxExempt, customer.IsAdmin, customer.IsGuest,
                        customer.IsForumModerator, customer.TotalForumPosts,
                        customer.Signature, customer.AdminComment, customer.Active, customer.Deleted, 
                        customer.RegistrationDate, customer.TimeZoneID, customer.AvatarID);
                }
                if (!NopContext.Current.IsAdmin)
                {
                    CommonHelper.SetCookie("Nop.TaxDisplayTypeID", ((int)value).ToString(), new TimeSpan(365, 0, 0, 0, 0));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string LastProductPageVisited
        {
            get
            {
                if ((HttpContext.Current.Session != null) && (HttpContext.Current.Session["Nop.LastProductPageVisited"] != null))
                {
                    return HttpContext.Current.Session["Nop.LastProductPageVisited"].ToString();
                }
                return string.Empty;
            }
            set
            {
                if ((HttpContext.Current != null) && (HttpContext.Current.Session != null))
                {
                    HttpContext.Current.Session["Nop.LastProductPageVisited"] = value;
                }
            }
        }

        /// <summary>
        /// Sets the CultureInfo 
        /// </summary>
        /// <param name="culture">Culture</param>
        public void SetCulture(CultureInfo culture)
        {
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }
        
        #endregion
    }
}
