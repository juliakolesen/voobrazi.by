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
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.BusinessLogic.Utils;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Common;


namespace NopSolutions.NopCommerce.Payment.Methods.Moneybookers
{
    /// <summary>
    /// Moneybookers payment processor
    /// </summary>
    public class MoneybookersPaymentProcessor : IPaymentMethod
    {
        #region Fields
        private string payToEmail = string.Empty;
        #endregion

        #region Ctor
        /// <summary>
        /// Creates a new instance of the MoneybookersPaymentProcessor class
        /// </summary>
        public MoneybookersPaymentProcessor()
        {
            payToEmail = SettingManager.GetSettingValue("PaymentMethod.Moneybookers.PayToEmail");
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets Moneybookers URL
        /// </summary>
        /// <returns></returns>
        private string GetMoneybookersUrl()
        {
            //return useSandBox ? "http://www.moneybookers.com/app/test_payment.pl" : "https://www.moneybookers.com/app/payment.pl";
            return "https://www.moneybookers.com/app/payment.pl";
        }

        /// <summary>
        /// Process payment
        /// </summary>
        /// <param name="paymentInfo">Payment info required for an order processing</param>
        /// <param name="customer">Customer</param>
        /// <param name="OrderGuid">Unique order identifier</param>
        /// <param name="processPaymentResult">Process payment result</param>
        public void ProcessPayment(PaymentInfo paymentInfo, Customer customer, Guid OrderGuid, ref ProcessPaymentResult processPaymentResult)
        {
            processPaymentResult.PaymentStatus = PaymentStatusEnum.Pending;
        }

        /// <summary>
        /// Post process payment (payment gateways that require redirecting)
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>The error status, or String.Empty if no errors</returns>
        public string PostProcessPayment(Order order)
        {
            RemotePost remotePostHelper = new RemotePost();
            remotePostHelper.FormName = "MoneybookersForm";
            remotePostHelper.Url = GetMoneybookersUrl();

            remotePostHelper.Add("pay_to_email", payToEmail);
            remotePostHelper.Add("recipient_description", SettingManager.StoreName);
            remotePostHelper.Add("transaction_id", order.OrderID.ToString());
            remotePostHelper.Add("cancel_url", CommonHelper.GetStoreLocation(false) + "Default.aspx");
            remotePostHelper.Add("status_url", CommonHelper.GetStoreLocation(false) + "MoneybookersReturn.aspx");
            //supported moneybookers languages (EN, DE, ES, FR, IT, PL, GR, RO, RU, TR, CN, CZ or NL)
            //TODO Check if customer working language is supported by Moneybookers.
            remotePostHelper.Add("language", "EN");
            remotePostHelper.Add("amount", order.OrderTotal.ToString(new CultureInfo("en-US", false).NumberFormat));
            //TODO Primary store currency should be set to USD now (3-letter codes)
            remotePostHelper.Add("currency", CurrencyManager.PrimaryStoreCurrency.CurrencyCode);
            remotePostHelper.Add("detail1_description", "Order ID:");
            remotePostHelper.Add("detail1_text", order.OrderID.ToString());

            remotePostHelper.Add("firstname", order.BillingFirstName);
            remotePostHelper.Add("lastname", order.BillingLastName);
            remotePostHelper.Add("address", order.BillingAddress1);
            remotePostHelper.Add("phone_number", order.BillingPhoneNumber);
            remotePostHelper.Add("postal_code", order.BillingZipPostalCode);
            remotePostHelper.Add("city", order.BillingCity);
            StateProvince billingStateProvince = StateProvinceManager.GetStateProvinceByID(order.BillingStateProvinceID);
            if (billingStateProvince != null)
                remotePostHelper.Add("state", billingStateProvince.Abbreviation);
            else
                remotePostHelper.Add("state", order.BillingStateProvince);
            Country billingCountry = CountryManager.GetCountryByID(order.BillingCountryID);
            if (billingCountry != null)
                remotePostHelper.Add("country", billingCountry.ThreeLetterISOCode);
            else
                remotePostHelper.Add("country", order.BillingCountry);
            remotePostHelper.Post();
            return string.Empty;
        }

        /// <summary>
        /// Gets additional handling fee
        /// </summary>
        /// <returns>Additional handling fee</returns>
        public decimal GetAdditionalHandlingFee()
        {
            //override if payment method requires additional handling fee
            return decimal.Zero;
        }

        /// <summary>
        /// Captures payment (from admin panel)
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="processPaymentResult">Process payment result</param>
        public void Capture(Order order, ref ProcessPaymentResult processPaymentResult)
        {
            throw new NopException("Capture method not supported");
        }

        #endregion

        #region Properies

        /// <summary>
        /// Gets a value indicating whether capture is allowed from admin panel
        /// </summary>
        public bool CanCapture
        {
            get
            {
                return false;
            }
        }
        #endregion
    }
}