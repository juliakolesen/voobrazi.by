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
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.Common;

namespace NopSolutions.NopCommerce.Payment.Methods.eWay
{
    /// <summary>
    /// eWay payment processor
    /// </summary>
    public class eWayPaymentProcessor : IPaymentMethod
    {
        #region Fields
        private bool useSandBox = true;
        private string ewayTestCustomerID;
        private string ewayLiveCustomerID;
        private string APPROVED_RESPONSE = "00";
        private string HONOUR_RESPONSE = "08";
        #endregion

        #region Ctor
        /// <summary>
        /// Creates a new instance of the eWayPaymentProcessor class
        /// </summary>
        public eWayPaymentProcessor()
        {

        }
        #endregion

        #region Methods

        /// <summary>
        /// Initializes the eWay payment processor
        /// </summary>
        private void InitSettings()
        {
            useSandBox = SettingManager.GetSettingValueBoolean("PaymentMethod.eWay.UseSandbox");
            ewayTestCustomerID = SettingManager.GetSettingValue("PaymentMethod.eWay.TestCustomerID");
            ewayLiveCustomerID = SettingManager.GetSettingValue("PaymentMethod.eWay.LiveCustomerID");
        }

        /// <summary>
        /// Gets eWay URL
        /// </summary>
        /// <returns></returns>
        private string GeteWayUrl()
        {
            //return useSandBox ? "https://www.eway.com.au/gateway/xmltest/TestPage.asp" :
            //    "https://www.eway.com.au/gateway/xmlpayment.asp";

            return useSandBox ? "https://www.eway.com.au/gateway_cvn/xmltest/TestPage.asp" :
                "https://www.eway.com.au/gateway_cvn/xmlpayment.asp";
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

            GatewayConnector eWAYgateway = new GatewayConnector();
            GatewayRequest eWAYRequest = new GatewayRequest();
            if (useSandBox)
                eWAYRequest.EwayCustomerID = ewayTestCustomerID;
            else
                eWAYRequest.EwayCustomerID = ewayLiveCustomerID;

            eWAYRequest.CardNumber = paymentInfo.CreditCardNumber;
            eWAYRequest.CardExpiryMonth = paymentInfo.CreditCardExpireMonth.ToString("D2");
            eWAYRequest.CardExpiryYear = paymentInfo.CreditCardExpireYear.ToString();
            eWAYRequest.CardHolderName = paymentInfo.CreditCardName;
            //Integer
            eWAYRequest.InvoiceAmount = Convert.ToInt32(paymentInfo.OrderTotal * 100);
            eWAYRequest.PurchaserFirstName = paymentInfo.BillingAddress.FirstName;
            eWAYRequest.PurchaserLastName = paymentInfo.BillingAddress.LastName;
            eWAYRequest.PurchaserEmailAddress = paymentInfo.BillingAddress.Email;
            eWAYRequest.PurchaserAddress = paymentInfo.BillingAddress.Address1;
            eWAYRequest.PurchaserPostalCode = paymentInfo.BillingAddress.ZipPostalCode;
            eWAYRequest.InvoiceReference = OrderGuid.ToString();
            eWAYRequest.InvoiceDescription = SettingManager.GetSettingValue("Common.StoreName") + ". Order #" + OrderGuid.ToString();
            eWAYRequest.TransactionNumber = OrderGuid.ToString();
            eWAYRequest.CVN = paymentInfo.CreditCardCVV2;
            eWAYRequest.EwayOption1 = string.Empty;
            eWAYRequest.EwayOption2 = string.Empty;
            eWAYRequest.EwayOption3 = string.Empty;

            // Do the payment, send XML doc containing information gathered
            eWAYgateway.Uri = GeteWayUrl();
            GatewayResponse eWAYResponse = eWAYgateway.ProcessRequest(eWAYRequest);
            if (eWAYResponse != null)
            {
                // Payment succeeded get values returned
                if (eWAYResponse.Status && (eWAYResponse.Error.StartsWith(APPROVED_RESPONSE) || eWAYResponse.Error.StartsWith(HONOUR_RESPONSE)))
                {
                    processPaymentResult.AuthorizationTransactionCode = eWAYResponse.AuthorisationCode;
                    processPaymentResult.AuthorizationTransactionResult = eWAYResponse.InvoiceReference;
                    processPaymentResult.AuthorizationTransactionID = eWAYResponse.TransactionNumber;
                    processPaymentResult.PaymentStatus = PaymentStatusEnum.Paid;
                    //processPaymentResult.AuthorizationDate = DateTime.Now;
                }
                else
                {
                    processPaymentResult.Error = "An invalid response was recieved from the payment gateway." + eWAYResponse.Error;
                    processPaymentResult.FullError = "An invalid response was recieved from the payment gateway." + eWAYRequest.ToXml().ToString() + ". " + eWAYResponse.Error;
                }
            }
            else
            {
                // invalid response recieved from server.
                processPaymentResult.Error = "An invalid response was recieved from the payment gateway.";
                processPaymentResult.FullError = "An invalid response was recieved from the payment gateway." + eWAYRequest.ToXml().ToString();
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
            throw new NopException("Capture method not supported");
        }

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
