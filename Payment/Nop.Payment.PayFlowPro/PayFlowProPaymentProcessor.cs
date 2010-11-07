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
using PayPal.Payments.DataObjects;
using PayPal.Payments.Common.Utility;
using PayPal.Payments.Transactions;
using System.Threading;
using NopSolutions.NopCommerce.Common;


namespace NopSolutions.NopCommerce.Payment.Methods.PayFlowPro
{
    /// <summary>
    /// PayFlow payment processor
    /// </summary>
    public class PayFlowProPaymentProcessor : IPaymentMethod
    {
        #region Fields
        private bool useSandBox = true;
        private string  user;
        private string vendor;
        private string partner;
        private string password;
        #endregion

        #region Ctor
        /// <summary>
        /// Creates a new instance of the PayFlowPaymentProcessor class
        /// </summary>
        public PayFlowProPaymentProcessor()
        {

        }
        #endregion

        #region Utilities
        /// <summary>
        /// Gets Paypal URL
        /// </summary>
        /// <returns></returns>
        private string GetPaypalUrl()
        {
            return useSandBox ? "pilot-payflowpro.verisign.com/transaction" :
                "payflowpro.verisign.com/transaction";
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
            string transactionMode = SettingManager.GetSettingValue("PaymentMethod.PayFlowPro.TransactionMode");
            if (!String.IsNullOrEmpty(transactionMode))
                transactionModeEnum = (TransactMode)Enum.Parse(typeof(TransactMode), transactionMode);
            return transactionModeEnum;
        }

        /// <summary>
        /// Initializes the PayPalDirectPaymentProcessor
        /// </summary>
        private void InitSettings()
        {
            useSandBox = SettingManager.GetSettingValueBoolean("PaymentMethod.PayFlowPro.UseSandbox");
            user = SettingManager.GetSettingValue("PaymentMethod.PayFlowPro.User");
            vendor = SettingManager.GetSettingValue("PaymentMethod.PayFlowPro.Vendor");
            partner = SettingManager.GetSettingValue("PaymentMethod.PayFlowPro.Partner");
            password = SettingManager.GetSettingValue("PaymentMethod.PayFlowPro.Password");
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

            //little hack here
            CultureInfo userCulture = Thread.CurrentThread.CurrentCulture;
            NopContext.Current.SetCulture(new CultureInfo("en-US"));
            try
            {
                BillTo to = new BillTo();
                to.FirstName = paymentInfo.BillingAddress.FirstName;
                to.LastName = paymentInfo.BillingAddress.LastName;
                to.Street = paymentInfo.BillingAddress.Address1;
                to.City = paymentInfo.BillingAddress.City;
                to.Zip = paymentInfo.BillingAddress.ZipPostalCode;
                if (paymentInfo.BillingAddress.StateProvince != null)
                    to.State = paymentInfo.BillingAddress.StateProvince.Abbreviation;
                ShipTo to2 = new ShipTo();
                to2.ShipToFirstName = paymentInfo.ShippingAddress.FirstName;
                to2.ShipToLastName = paymentInfo.ShippingAddress.LastName;
                to2.ShipToStreet = paymentInfo.ShippingAddress.Address1;
                to2.ShipToCity = paymentInfo.ShippingAddress.City;
                to2.ShipToZip = paymentInfo.ShippingAddress.ZipPostalCode;
                if (paymentInfo.ShippingAddress.StateProvince != null)
                    to2.ShipToState = paymentInfo.ShippingAddress.StateProvince.Abbreviation;

                Invoice invoice = new Invoice();
                invoice.BillTo = to;
                invoice.ShipTo = to2;
                invoice.InvNum = OrderGuid.ToString();
                //For values which have more than two decimal places 
                //Currency Amt = new Currency(new decimal(25.1214));
                //Amt.NoOfDecimalDigits = 2;
                //If the NoOfDecimalDigits property is used then it is mandatory to set one of the following properties to true.
                //Amt.Round = true;
                //Amt.Truncate = true;
                //Inv.Amt = Amt;
                decimal orderTotal = Math.Round(paymentInfo.OrderTotal, 2);
                //UNDONE USD only
                invoice.Amt = new PayPal.Payments.DataObjects.Currency(orderTotal, CurrencyManager.PrimaryStoreCurrency.CurrencyCode);

                string creditCardExp = string.Empty;
                if (paymentInfo.CreditCardExpireMonth < 10)
                {
                    creditCardExp = "0" + paymentInfo.CreditCardExpireMonth.ToString();
                }
                else
                {
                    creditCardExp = paymentInfo.CreditCardExpireMonth.ToString();
                }
                creditCardExp = creditCardExp + paymentInfo.CreditCardExpireYear.ToString().Substring(2, 2);
                CreditCard credCard = new CreditCard(paymentInfo.CreditCardNumber, creditCardExp);
                credCard.Cvv2 = paymentInfo.CreditCardCVV2;
                CardTender tender = new CardTender(credCard);
                // <vendor> = your merchant (login id)  
                // <user> = <vendor> unless you created a separate <user> for Payflow Pro
                // partner = paypal
                UserInfo userInfo = new UserInfo(user, vendor, partner, password);
                string url = GetPaypalUrl();
                PayflowConnectionData payflowConnectionData = new PayflowConnectionData(url, 443, null, 0, null, null);

                Response response = null;
                if (transactionMode == TransactMode.Authorize)
                {
                    response = new AuthorizationTransaction(userInfo, payflowConnectionData, invoice, tender, PayflowUtility.RequestId).SubmitTransaction();
                }
                else
                {
                    response = new SaleTransaction(userInfo, payflowConnectionData, invoice, tender, PayflowUtility.RequestId).SubmitTransaction();
                }

                if (response.TransactionResponse != null)
                {
                    if (response.TransactionResponse.Result == 0)
                    {
                        processPaymentResult.AuthorizationTransactionID = response.TransactionResponse.Pnref;
                        processPaymentResult.AuthorizationTransactionResult = response.TransactionResponse.RespMsg;

                        if (transactionMode == TransactMode.Authorize)
                        {
                            processPaymentResult.PaymentStatus = PaymentStatusEnum.Authorized;
                        }
                        else
                        {
                            processPaymentResult.PaymentStatus = PaymentStatusEnum.Paid;
                        }
                    }
                    else
                    {
                        processPaymentResult.Error = string.Format("{0} - {1}", response.TransactionResponse.Result, response.TransactionResponse.RespMsg);
                        processPaymentResult.FullError = string.Format("Response Code : {0}. Response Description : {1}", response.TransactionResponse.Result, response.TransactionResponse.RespMsg);
                    }
                }
                else
                {
                    processPaymentResult.Error = "Error during checkout";
                    processPaymentResult.FullError = "Error during checkout";
                }
            }
            catch (Exception exc)
            {
                throw;
            }
            finally
            {
                NopContext.Current.SetCulture(userCulture);
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
            //TODO implement capture from admin panel using CaptureTransaction object
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
