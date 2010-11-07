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
using NopSolutions.NopCommerce.Payment.Methods.PayPal.PayPalSvc;
using NopSolutions.NopCommerce.Common;


namespace NopSolutions.NopCommerce.Payment.Methods.PayPal
{
    /// <summary>
    /// Paypal Direct payment processor
    /// </summary>
    public class PayPalDirectPaymentProcessor : IPaymentMethod
    {
        #region Fields
        private bool useSandBox = true;
        private string APIAccountName;
        private string APIAccountPassword;
        private string Signature;
        private PayPalAPISoapBinding service1;
        private PayPalAPIAASoapBinding service2;
        #endregion

        #region Ctor
        /// <summary>
        /// Creates a new instance of the PayPalDirectPaymentProcessor class
        /// </summary>
        public PayPalDirectPaymentProcessor()
        {

        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets transaction mode configured by store owner
        /// </summary>
        /// <returns></returns>
        private TransactMode GetCurrentTransactionMode()
        {
            TransactMode transactionModeEnum = TransactMode.Authorize;
            string transactionMode = SettingManager.GetSettingValue("PaymentMethod.PaypalDirect.TransactionMode");
            if (!String.IsNullOrEmpty(transactionMode))
                transactionModeEnum = (TransactMode)Enum.Parse(typeof(TransactMode), transactionMode);
            return transactionModeEnum;
        }

        /// <summary>
        /// Initializes the PayPalDirectPaymentProcessor
        /// </summary>
        private void InitSettings()
        {
            useSandBox = SettingManager.GetSettingValueBoolean("PaymentMethod.PaypalDirect.UseSandbox");
            APIAccountName = SettingManager.GetSettingValue("PaymentMethod.PaypalDirect.APIAccountName");
            APIAccountPassword = SettingManager.GetSettingValue("PaymentMethod.PaypalDirect.APIAccountPassword");
            Signature = SettingManager.GetSettingValue("PaymentMethod.PaypalDirect.Signature");

            if (string.IsNullOrEmpty(APIAccountName))
                throw new NopException("Paypal Direct API Account Name is empty");

            if (string.IsNullOrEmpty(Signature))
                throw new NopException("Paypal Direct API Account Password is empty");

            if (string.IsNullOrEmpty(APIAccountPassword))
                throw new NopException("Paypal Direct Signature is empty");

            service1 = new PayPalAPISoapBinding();
            service2 = new PayPalAPIAASoapBinding();

            if (!useSandBox)
            {
                service2.Url = service1.Url = "https://api-3t.paypal.com/2.0/";
            }
            else
            {
                service2.Url = service1.Url = "https://api-3t.sandbox.paypal.com/2.0/";
            }

            service1.RequesterCredentials = new CustomSecurityHeaderType();
            service1.RequesterCredentials.Credentials = new UserIdPasswordType();
            service1.RequesterCredentials.Credentials.Username = APIAccountName;
            service1.RequesterCredentials.Credentials.Password = APIAccountPassword;
            service1.RequesterCredentials.Credentials.Signature = Signature;
            service1.RequesterCredentials.Credentials.Subject = "";

            service2.RequesterCredentials = new CustomSecurityHeaderType();
            service2.RequesterCredentials.Credentials = new UserIdPasswordType();
            service2.RequesterCredentials.Credentials.Username = APIAccountName;
            service2.RequesterCredentials.Credentials.Password = APIAccountPassword;
            service2.RequesterCredentials.Credentials.Signature = Signature;
            service2.RequesterCredentials.Credentials.Subject = "";
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
            TransactMode transactionMode = GetCurrentTransactionMode();

            if (paymentInfo.BillingAddress.StateProvince == null)
                throw new NopException("Please enter a valid state in the billing address");

            if (transactionMode == TransactMode.Authorize)
            {
                AuthorizeOrSale(paymentInfo, customer, OrderGuid, processPaymentResult, true);
                if (!String.IsNullOrEmpty(processPaymentResult.Error))
                    return;
            }
            else
            {
                AuthorizeOrSale(paymentInfo, customer, OrderGuid, processPaymentResult, false);
                if (!String.IsNullOrEmpty(processPaymentResult.Error))
                    return;
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
        /// Gets Paypal API version
        /// </summary>
        public string APIVersion
        {
            get
            {
                return "2.0";
            }
        }

        /// <summary>
        /// Captures payment (from admin panel)
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="processPaymentResult">Process payment result</param>
        public void Capture(Order order, ref ProcessPaymentResult processPaymentResult)
        {
            InitSettings();

            string authorizationID = processPaymentResult.AuthorizationTransactionID;
            DoCaptureReq req = new DoCaptureReq();
            req.DoCaptureRequest = new DoCaptureRequestType();
            req.DoCaptureRequest.Version = this.APIVersion;
            req.DoCaptureRequest.AuthorizationID = authorizationID;
            req.DoCaptureRequest.Amount = new BasicAmountType();
            req.DoCaptureRequest.Amount.Value = order.OrderTotal.ToString("N", new CultureInfo("en-us"));
            req.DoCaptureRequest.Amount.currencyID = PaypalHelper.GetPaypalCurrency(CurrencyManager.PrimaryStoreCurrency);
            req.DoCaptureRequest.CompleteType = CompleteCodeType.Complete;
            DoCaptureResponseType response = service2.DoCapture(req);

            string error = string.Empty;
            bool Success = PaypalHelper.CheckSuccess(response, out error);
            if (Success)
            {
                processPaymentResult.PaymentStatus = PaymentStatusEnum.Paid;
                processPaymentResult.CaptureTransactionID = response.DoCaptureResponseDetails.PaymentInfo.TransactionID;
                processPaymentResult.CaptureTransactionResult = response.Ack.ToString();
                //processPaymentResult.CaptureDate = response.Timestamp;
                //processPaymentResult.CaptureDate = DateTime.Now;
            }
            else
            {
                processPaymentResult.Error = error;
            }
        }

        /// <summary>
        /// Authorizes the payment
        /// </summary>
        /// <param name="paymentInfo">Payment info required for an order processing</param>
        /// <param name="customer">Customer</param>
        /// <param name="OrderGuid">Unique order identifier</param>
        /// <param name="processPaymentResult">Process payment result</param>
        /// <param name="authorizeOnly">A value indicating whether to authorize only; true - authorize; false - sale</param>
        protected void AuthorizeOrSale(PaymentInfo paymentInfo, Customer customer, Guid OrderGuid, ProcessPaymentResult processPaymentResult, bool authorizeOnly)
        {
            InitSettings();

            DoDirectPaymentReq req = new DoDirectPaymentReq();
            req.DoDirectPaymentRequest = new DoDirectPaymentRequestType();
            req.DoDirectPaymentRequest.Version = this.APIVersion;
            DoDirectPaymentRequestDetailsType details = new DoDirectPaymentRequestDetailsType();
            req.DoDirectPaymentRequest.DoDirectPaymentRequestDetails = details;
            details.IPAddress = HttpContext.Current.Request.UserHostAddress;
            if (authorizeOnly)
                details.PaymentAction = PaymentActionCodeType.Authorization;
            else
                details.PaymentAction = PaymentActionCodeType.Sale;
            details.CreditCard = new CreditCardDetailsType();
            details.CreditCard.CreditCardNumber = paymentInfo.CreditCardNumber;
            details.CreditCard.CreditCardType = GetPaypalCreditCardType(paymentInfo.CreditCardType);
            details.CreditCard.ExpMonthSpecified = true;
            details.CreditCard.ExpMonth = paymentInfo.CreditCardExpireMonth;
            details.CreditCard.ExpYearSpecified = true;
            details.CreditCard.ExpYear = paymentInfo.CreditCardExpireYear;
            details.CreditCard.CVV2 = paymentInfo.CreditCardCVV2;

            details.CreditCard.CardOwner = new PayerInfoType();
            details.CreditCard.CardOwner.PayerCountry = GetPaypalCountryCodeType(paymentInfo.BillingAddress.Country);

            details.CreditCard.CardOwner.Address = new AddressType();
            details.CreditCard.CardOwner.Address.CountrySpecified = true;
            details.CreditCard.CardOwner.Address.Street1 = paymentInfo.BillingAddress.Address1;
            details.CreditCard.CardOwner.Address.Street2 = paymentInfo.BillingAddress.Address2;
            details.CreditCard.CardOwner.Address.CityName = paymentInfo.BillingAddress.City;
            if (paymentInfo.BillingAddress.StateProvince != null)
                details.CreditCard.CardOwner.Address.StateOrProvince = paymentInfo.BillingAddress.StateProvince.Abbreviation;
            details.CreditCard.CardOwner.Address.Country = GetPaypalCountryCodeType(paymentInfo.BillingAddress.Country);
            details.CreditCard.CardOwner.Address.PostalCode = paymentInfo.BillingAddress.ZipPostalCode;
            details.CreditCard.CardOwner.PayerName = new PersonNameType();
            details.CreditCard.CardOwner.PayerName.FirstName = paymentInfo.BillingAddress.FirstName;
            details.CreditCard.CardOwner.PayerName.LastName = paymentInfo.BillingAddress.LastName;
            details.PaymentDetails = new PaymentDetailsType();
            details.PaymentDetails.OrderTotal = new BasicAmountType();
            details.PaymentDetails.OrderTotal.Value = paymentInfo.OrderTotal.ToString("N", new CultureInfo("en-us"));
            details.PaymentDetails.OrderTotal.currencyID = PaypalHelper.GetPaypalCurrency(CurrencyManager.PrimaryStoreCurrency);
            details.PaymentDetails.Custom = OrderGuid.ToString();
            details.PaymentDetails.ButtonSource = "nopCommerceCart";

            DoDirectPaymentResponseType response = service2.DoDirectPayment(req);

            string error = string.Empty;
            bool Success = PaypalHelper.CheckSuccess(response, out error);
            if (Success)
            {
                processPaymentResult.AuthorizationTransactionID = response.TransactionID;
                processPaymentResult.AuthorizationTransactionResult = response.Ack.ToString();

                //TODO save AVSCode and CVVCode in datatabase
                processPaymentResult.AVSResult = response.AVSCode;
                processPaymentResult.AuthorizationTransactionCode = response.CVV2Code;
                if (authorizeOnly)
                    processPaymentResult.PaymentStatus = PaymentStatusEnum.Authorized;
                else
                    processPaymentResult.PaymentStatus = PaymentStatusEnum.Paid;
                //processPaymentResult.AuthorizationDate = response.Timestamp;
                //processPaymentResult.AuthorizationDate = DateTime.Now;
            }
            else
            {
                processPaymentResult.Error = error;
                processPaymentResult.FullError = error;
            }
        }

        /// <summary>
        /// Get Paypal country code
        /// </summary>
        /// <param name="country">Country</param>
        /// <returns>Paypal country code</returns>
        protected CountryCodeType GetPaypalCountryCodeType(Country country)
        {
            CountryCodeType payerCountry = CountryCodeType.US;
            try
            {
                payerCountry = (CountryCodeType)Enum.Parse(typeof(CountryCodeType), country.TwoLetterISOCode);
            }
            catch
            {
            }
            return payerCountry;
        }

        /// <summary>
        /// Get Paypal credit card type
        /// </summary>
        /// <param name="CreditCardType">Credit card type</param>
        /// <returns>Paypal credit card type</returns>
        protected CreditCardTypeType GetPaypalCreditCardType(string CreditCardType)
        {
            CreditCardTypeType creditCardTypeType = (CreditCardTypeType)Enum.Parse(typeof(CreditCardTypeType), CreditCardType);
            return creditCardTypeType;
            //if (CreditCardType.ToLower() == "visa")
            //    return CreditCardTypeType.Visa;

            //if (CreditCardType.ToLower() == "mastercard")
            //    return CreditCardTypeType.MasterCard;

            //if (CreditCardType.ToLower() == "americanexpress")
            //    return CreditCardTypeType.Amex;

            //if (CreditCardType.ToLower() == "discover")
            //    return CreditCardTypeType.Discover;

            //throw new NopException("Unknown credit card type");
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
                return true;
            }
        }
        #endregion
    }
}
