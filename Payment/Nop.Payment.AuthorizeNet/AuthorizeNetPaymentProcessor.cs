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
using System.Globalization;
using System.Net;
using System.Text;
using System.Web;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.Common;

namespace NopSolutions.NopCommerce.Payment.Methods.AuthorizeNET
{
    /// <summary>
    /// Authorize.Net payment processor
    /// </summary>
    public class AuthorizeNetPaymentProcessor : IPaymentMethod
    {
        #region Fields
        private bool useSandBox = true;
        private string loginID;
        private string transactionKey;
        #endregion

        #region Ctor
        /// <summary>
        /// Creates a new instance of the AuthorizeNetPaymentProcessor class
        /// </summary>
        public AuthorizeNetPaymentProcessor()
        {

        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets current transaction mode
        /// </summary>
        /// <returns>Current transaction mode</returns>
        public static TransactMode GetCurrentTransactionMode()
        {
            TransactMode transactionModeEnum = TransactMode.Authorize;
            string transactionMode = SettingManager.GetSettingValue("PaymentMethod.AuthorizeNET.TransactionMode");
            if (!String.IsNullOrEmpty(transactionMode))
                transactionModeEnum = (TransactMode)Enum.Parse(typeof(TransactMode), transactionMode);
            return transactionModeEnum;
        }

        /// <summary>
        /// Initializes the Authorize.NET payment processor
        /// </summary>
        private void InitSettings()
        {
            useSandBox = SettingManager.GetSettingValueBoolean("PaymentMethod.AuthorizeNET.UseSandbox");
            transactionKey = SettingManager.GetSettingValue("PaymentMethod.AuthorizeNET.TransactionKey");
            loginID = SettingManager.GetSettingValue("PaymentMethod.AuthorizeNET.LoginID");

            if (string.IsNullOrEmpty(transactionKey))
                throw new NopException("Authorize.NET API transaction key is not set");

            if (string.IsNullOrEmpty(loginID))
                throw new NopException("Authorize.NET API login ID is not set");
        }

        /// <summary>
        /// Gets Authorize.NET URL
        /// </summary>
        /// <returns></returns>
        private string GetAuthorizeNETUrl()
        {
            return useSandBox ? "https://test.authorize.net/gateway/transact.dll" :
                "https://secure.authorize.net/gateway/transact.dll";
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
            InitSettings();
            TransactMode transactionMode = GetCurrentTransactionMode();

            WebClient webClient = new WebClient();
            NameValueCollection form = new NameValueCollection();
            form.Add("x_login", loginID);
            form.Add("x_tran_key", transactionKey);
            if (useSandBox)
                form.Add("x_test_request", "TRUE");
            else
                form.Add("x_test_request", "FALSE");

            form.Add("x_delim_data", "TRUE");
            form.Add("x_delim_char", "|");
            form.Add("x_encap_char", "");
            form.Add("x_version", APIVersion);
            form.Add("x_relay_response", "FALSE");
            form.Add("x_method", "CC");
            //TODO populate currency code
            form.Add("x_currency_code", "USD");
            if (transactionMode == TransactMode.Authorize)
                form.Add("x_type", "AUTH_ONLY");
            else if (transactionMode == TransactMode.AuthorizeAndCapture)
                form.Add("x_type", "AUTH_CAPTURE");
            else
                throw new NopException("Not supported transaction mode");

            form.Add("x_amount", paymentInfo.OrderTotal.ToString("####.00", new CultureInfo("en-US", false).NumberFormat));
            form.Add("x_card_num", paymentInfo.CreditCardNumber);
            form.Add("x_exp_date", paymentInfo.CreditCardExpireMonth.ToString("D2") + paymentInfo.CreditCardExpireYear.ToString());
            form.Add("x_card_code", paymentInfo.CreditCardCVV2);
            form.Add("x_first_name", paymentInfo.BillingAddress.FirstName);
            form.Add("x_last_name", paymentInfo.BillingAddress.LastName);
            if (string.IsNullOrEmpty(paymentInfo.BillingAddress.Company))
                form.Add("x_company", paymentInfo.BillingAddress.Company);
            form.Add("x_address", paymentInfo.BillingAddress.Address1);
            form.Add("x_city", paymentInfo.BillingAddress.City);
            if (paymentInfo.BillingAddress.StateProvince != null)
                form.Add("x_state", paymentInfo.BillingAddress.StateProvince.Abbreviation);
            form.Add("x_zip", paymentInfo.BillingAddress.ZipPostalCode);
            if (paymentInfo.BillingAddress.Country != null)
                form.Add("x_country", paymentInfo.BillingAddress.Country.TwoLetterISOCode);
            form.Add("x_invoice_num", OrderGuid.ToString());
            form.Add("x_customer_ip", HttpContext.Current.Request.UserHostAddress);

            string reply = null;
            Byte[] responseData = webClient.UploadValues(GetAuthorizeNETUrl(), form);
            reply = Encoding.ASCII.GetString(responseData);

            if (!String.IsNullOrEmpty(reply))
            {
                string[] responseFields = reply.Split('|');
                switch (responseFields[0])
                {
                    case "1":
                        processPaymentResult.AuthorizationTransactionCode = string.Format("{0},{1}", responseFields[6], responseFields[4]);
                        processPaymentResult.AuthorizationTransactionResult = string.Format("Approved ({0}: {1})", responseFields[2], responseFields[3]);
                        processPaymentResult.AVSResult = responseFields[5];
                        //responseFields[38];
                        if (transactionMode == TransactMode.Authorize)
                        {
                            processPaymentResult.PaymentStatus = PaymentStatusEnum.Authorized;
                        }
                        else
                        {
                            processPaymentResult.PaymentStatus = PaymentStatusEnum.Paid;
                        }
                        break;
                    case "2":
                        processPaymentResult.Error = string.Format("Declined ({0}: {1})", responseFields[2], responseFields[3]);
                        processPaymentResult.FullError = string.Format("Declined ({0}: {1})", responseFields[2], responseFields[3]);
                        break;
                    case "3":
                        processPaymentResult.Error = string.Format("Error: {0}", reply);
                        processPaymentResult.FullError = string.Format("Error: {0}", reply);
                        break;

                }
            }
            else
            {
                processPaymentResult.Error = "Authorize.NET unknown error";
                processPaymentResult.FullError = "Authorize.NET unknown error";
            }
        }

        /// <summary>
        /// Post process payment (payment gateways that require redirecting)
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>The error status, or String.Empty if no errors</returns>
        public string PostProcessPayment(Order order)
        {
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
            InitSettings();

            WebClient webClient = new WebClient();
            NameValueCollection form = new NameValueCollection();
            form.Add("x_login", loginID);
            form.Add("x_tran_key", transactionKey);
            if (useSandBox)
                form.Add("x_test_request", "TRUE");
            else
                form.Add("x_test_request", "FALSE");

            form.Add("x_delim_data", "TRUE");
            form.Add("x_delim_char", "|");
            form.Add("x_encap_char", "");
            form.Add("x_version", APIVersion);
            form.Add("x_relay_response", "FALSE");
            form.Add("x_method", "CC");
            //TODO populate currency code
            form.Add("x_currency_code", "USD");
            form.Add("x_type", "PRIOR_AUTH_CAPTURE");

            form.Add("x_amount", order.OrderTotal.ToString("####.00", new CultureInfo("en-US", false).NumberFormat));
            string[] codes = processPaymentResult.AuthorizationTransactionCode.Split(',');
            form.Add("x_trans_id", codes[0]);

            string reply = null;
            Byte[] responseData = webClient.UploadValues(GetAuthorizeNETUrl(), form);
            reply = Encoding.ASCII.GetString(responseData);

            DateTime nowDT = DateTime.Now;

            if (!String.IsNullOrEmpty(reply))
            {
                string[] responseFields = reply.Split('|');
                switch (responseFields[0])
                {
                    case "1":
                        processPaymentResult.AuthorizationTransactionCode = string.Format("{0},{1}", responseFields[6], responseFields[4]);
                        processPaymentResult.AuthorizationTransactionResult = string.Format("Approved ({0}: {1})", responseFields[2], responseFields[3]);
                        processPaymentResult.AVSResult = responseFields[5];
                        //responseFields[38];
                        processPaymentResult.PaymentStatus = PaymentStatusEnum.Paid;
                        break;
                    case "2":
                        processPaymentResult.Error = string.Format("Declined ({0}: {1})", responseFields[2], responseFields[3]);
                        processPaymentResult.FullError = string.Format("Declined ({0}: {1})", responseFields[2], responseFields[3]);
                        break;
                    case "3":
                        processPaymentResult.Error = string.Format("Error: {0}", reply);
                        processPaymentResult.FullError = string.Format("Error: {0}", reply);
                        break;
                }
            }
            else
            {
                processPaymentResult.Error = "Authorize.NET unknown error";
                processPaymentResult.FullError = "Authorize.NET unknown error";
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets Authorize.NET API version
        /// </summary>
        public string APIVersion
        {
            get
            {
                return "3.1";
            }
        }

        /// <summary>
        /// Gets a value indicating whether capture is allowed from admin panel
        /// </summary>
        public bool CanCapture
        {
            get 
            { 
                return true;
            }
        }
        #endregion
    }
}
