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
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Xml;
using System.Xml.Serialization;
using GCheckout.AutoGen;
using GCheckout.Checkout;
using GCheckout.Util;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Audit;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Localization;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Products.Attributes;
using NopSolutions.NopCommerce.BusinessLogic.Shipping;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.BusinessLogic.Utils;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.Payment.Methods.GoogleCheckout
{
    /// <summary>
    /// Google Checkout payment processor
    /// </summary>
    public class GoogleCheckoutPaymentProcessor : IPaymentMethod
    {
        #region Utilities

        private void logMessage(string message)
        {
            try
            {
                if (!SettingManager.GetSettingValueBoolean("PaymentMethod.GoogleCheckout.DebugModeEnabled"))
                    return;
                message = string.Format("{0}*******{1}{2}", DateTime.Now, Environment.NewLine, message);
                string logPath = HttpContext.Current.Server.MapPath("google/google_log.txt");
                using (FileStream fs = new FileStream(logPath, FileMode.Append, FileAccess.Write, FileShare.Read))
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(message);
                }
            }
            catch (Exception exc)
            {
                LogManager.InsertLog(LogTypeEnum.CommonError, exc.Message, exc);
            }
        }

        private void processNewOrderNotification(string xmlData)
        {
            try
            {
                NewOrderNotification newOrderNotification = (NewOrderNotification)EncodeHelper.Deserialize(xmlData, typeof(NewOrderNotification));
                string googleOrderNumber = newOrderNotification.googleordernumber;

                XmlNode CustomerInfo = newOrderNotification.shoppingcart.merchantprivatedata.Any[0];
                int CustomerID = Convert.ToInt32(CustomerInfo.Attributes["CustomerID"].Value);
                int CustomerLanguageID = Convert.ToInt32(CustomerInfo.Attributes["CustomerLanguageID"].Value);
                int CustomerCurrencyID = Convert.ToInt32(CustomerInfo.Attributes["CustomerCurrencyID"].Value);
                Customer customer = CustomerManager.GetCustomerByID(CustomerID);

                NopSolutions.NopCommerce.BusinessLogic.Orders.ShoppingCart Cart = ShoppingCartManager.GetCustomerShoppingCart(customer.CustomerID, ShoppingCartTypeEnum.ShoppingCart);

                if (customer == null)
                {
                    logMessage("Could not load a customer");
                    return;
                }

                NopContext.Current.User = customer;

                if (Cart.Count == 0)
                {
                    logMessage("Cart is empty");
                    return;
                }

                //validate cart
                foreach (NopSolutions.NopCommerce.BusinessLogic.Orders.ShoppingCartItem sci in Cart)
                {
                    bool ok = false;
                    foreach (Item item in newOrderNotification.shoppingcart.items)
                    {
                        if (!String.IsNullOrEmpty(item.merchantitemid))
                        {
                            if ((Convert.ToInt32(item.merchantitemid) == sci.ShoppingCartItemID) && (item.quantity == sci.Quantity))
                            {
                                ok = true;
                                break;
                            }
                        }
                    }

                    if (!ok)
                    {
                        logMessage(string.Format("Shopping Cart item has been changed. {0}. {1}", sci.ShoppingCartItemID, sci.Quantity));
                        return;
                    }
                }


                string[] billingFullname = newOrderNotification.buyerbillingaddress.contactname.Trim().Split(new char[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
                string billingFirstName = billingFullname[0];
                string billingLastName = string.Empty;
                if (billingFullname.Length > 1)
                    billingLastName = billingFullname[1];
                string billingEmail = newOrderNotification.buyerbillingaddress.email.Trim();
                string billingAddress1 = newOrderNotification.buyerbillingaddress.address1.Trim();
                string billingAddress2 = newOrderNotification.buyerbillingaddress.address2.Trim();
                string billingPhoneNumber = newOrderNotification.buyerbillingaddress.phone.Trim();
                string billingCity = newOrderNotification.buyerbillingaddress.city.Trim();
                int billingStateProvinceID = 0;
                StateProvince billingStateProvince = StateProvinceManager.GetStateProvinceByAbbreviation(newOrderNotification.buyerbillingaddress.region.Trim());
                if (billingStateProvince != null)
                    billingStateProvinceID = billingStateProvince.StateProvinceID;
                string billingZipPostalCode = newOrderNotification.buyerbillingaddress.postalcode.Trim();
                int billingCountryID = 0;
                Country billingCountry = CountryManager.GetCountryByTwoLetterISOCode(newOrderNotification.buyerbillingaddress.countrycode.Trim());
                if (billingCountry != null)
                    billingCountryID = billingCountry.CountryID;

                NopSolutions.NopCommerce.BusinessLogic.CustomerManagement.Address BillingAddress = customer.BillingAddresses.FindAddress(
                    billingFirstName, billingLastName, billingPhoneNumber,
                    billingEmail, string.Empty, string.Empty, billingAddress1, billingAddress2, billingCity,
                    billingStateProvinceID, billingZipPostalCode, billingCountryID);

                if (BillingAddress == null)
                {
                    BillingAddress = CustomerManager.InsertAddress(CustomerID, true,
                        billingFirstName, billingLastName, billingPhoneNumber, billingEmail,
                        string.Empty, string.Empty, billingAddress1,
                        billingAddress2, billingCity,
                        billingStateProvinceID, billingZipPostalCode,
                        billingCountryID, DateTime.Now, DateTime.Now);
                }
                customer = CustomerManager.SetDefaultBillingAddress(customer.CustomerID, BillingAddress.AddressID);

                NopSolutions.NopCommerce.BusinessLogic.CustomerManagement.Address ShippingAddress = null;
                customer.LastShippingOption = null;
                bool shoppingCartRequiresShipping = ShippingManager.ShoppingCartRequiresShipping(Cart);
                if (shoppingCartRequiresShipping)
                {
                    string[] shippingFullname = newOrderNotification.buyershippingaddress.contactname.Trim().Split(new char[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
                    string shippingFirstName = shippingFullname[0];
                    string shippingLastName = string.Empty;
                    if (shippingFullname.Length > 1)
                        shippingLastName = shippingFullname[1];
                    string shippingEmail = newOrderNotification.buyershippingaddress.email.Trim();
                    string shippingAddress1 = newOrderNotification.buyershippingaddress.address1.Trim();
                    string shippingAddress2 = newOrderNotification.buyershippingaddress.address2.Trim();
                    string shippingPhoneNumber = newOrderNotification.buyershippingaddress.phone.Trim();
                    string shippingCity = newOrderNotification.buyershippingaddress.city.Trim();
                    int shippingStateProvinceID = 0;
                    StateProvince shippingStateProvince = StateProvinceManager.GetStateProvinceByAbbreviation(newOrderNotification.buyershippingaddress.region.Trim());
                    if (shippingStateProvince != null)
                        shippingStateProvinceID = shippingStateProvince.StateProvinceID;
                    int shippingCountryID = 0;
                    string shippingZipPostalCode = newOrderNotification.buyershippingaddress.postalcode.Trim();
                    Country shippingCountry = CountryManager.GetCountryByTwoLetterISOCode(newOrderNotification.buyershippingaddress.countrycode.Trim());
                    if (shippingCountry != null)
                        shippingCountryID = shippingCountry.CountryID;

                    ShippingAddress = customer.ShippingAddresses.FindAddress(
                        shippingFirstName, shippingLastName, shippingPhoneNumber,
                        shippingEmail, string.Empty, string.Empty, 
                        shippingAddress1, shippingAddress2, shippingCity,
                        shippingStateProvinceID, shippingZipPostalCode, shippingCountryID);
                    if (ShippingAddress == null)
                    {
                        ShippingAddress = CustomerManager.InsertAddress(CustomerID, false,
                             shippingFirstName, shippingLastName, shippingPhoneNumber, shippingEmail,
                             string.Empty, string.Empty, shippingAddress1,
                             shippingAddress2, shippingCity, shippingStateProvinceID,
                             shippingZipPostalCode, shippingCountryID,
                             DateTime.Now, DateTime.Now);
                    }

                    customer = CustomerManager.SetDefaultShippingAddress(customer.CustomerID, ShippingAddress.AddressID);

                    string shippingMethod = string.Empty;
                    decimal shippingCost = decimal.Zero;
                    if (newOrderNotification.orderadjustment != null &&
                        newOrderNotification.orderadjustment.shipping != null &&
                        newOrderNotification.orderadjustment.shipping.Item != null)
                    {
                        FlatRateShippingAdjustment ShippingMethod = (FlatRateShippingAdjustment)newOrderNotification.orderadjustment.shipping.Item;
                        shippingMethod = ShippingMethod.shippingname;
                        shippingCost = ShippingMethod.shippingcost.Value;


                        ShippingOption shippingOption = new ShippingOption();
                        shippingOption.Name = shippingMethod;
                        shippingOption.Rate = shippingCost;
                        customer.LastShippingOption = shippingOption;
                    }
                }

                //customer.LastCalculatedTax = decimal.Zero;

                PaymentMethod googleCheckoutPaymentMethod = PaymentMethodManager.GetPaymentMethodBySystemKeyword("GoogleCheckout");

                PaymentInfo paymentInfo = new PaymentInfo();
                paymentInfo.PaymentMethodID = googleCheckoutPaymentMethod.PaymentMethodID;
                paymentInfo.BillingAddress = BillingAddress;
                paymentInfo.ShippingAddress = ShippingAddress;
                paymentInfo.CustomerLanguage = LanguageManager.GetLanguageByID(CustomerLanguageID);
                paymentInfo.CustomerCurrency = CurrencyManager.GetCurrencyByID(CustomerCurrencyID);
                paymentInfo.GoogleOrderNumber = googleOrderNumber;
                int orderID = 0;
                string result = OrderManager.PlaceOrder(paymentInfo, customer, out orderID);
                if (!String.IsNullOrEmpty(result))
                {
                    logMessage("new-order-notification received. CreateOrder() error: Order Number " + orderID + ". " + result);
                    return;
                }

                Order order = OrderManager.GetOrderByID(orderID);
                logMessage("new-order-notification received and saved: Order Number " + orderID);

            }
            catch (Exception exc)
            {
                logMessage("processNewOrderNotification Exception: " + exc.Message + ": " + exc.StackTrace);
            }
        }
        private void processOrderStateChangeNotification(string xmlData)
        {
            try
            {
                OrderStateChangeNotification changeOrder = (OrderStateChangeNotification)EncodeHelper.Deserialize(xmlData, typeof(OrderStateChangeNotification));

                FinancialOrderState orderState = changeOrder.newfinancialorderstate;
                Order order = getMerchantOrderByGoogleOrderID(changeOrder.googleordernumber);
                if (order != null)
                {
                    string message = string.Format("Order status {0} from Google: Order Number {1}", orderState, changeOrder.googleordernumber);
                    logMessage(message);
                    OrderManager.InsertOrderNote(order.OrderID, message, DateTime.Now);

                    if (orderState == FinancialOrderState.CHARGING ||
                        orderState == FinancialOrderState.REVIEWING)
                    {
                    }

                    if (orderState == FinancialOrderState.CHARGEABLE)
                    {
                        order = OrderManager.MarkAsAuthorized(order.OrderID);
                    }
                    if (orderState == FinancialOrderState.CHARGED)
                    {
                        order = OrderManager.MarkOrderAsPaid(order.OrderID);
                    }
                    if (orderState == FinancialOrderState.CANCELLED || orderState == FinancialOrderState.CANCELLED_BY_GOOGLE)
                    {
                        order = OrderManager.CancelOrder(order.OrderID, true);
                    }
                }
            }
            catch (Exception exc)
            {
                logMessage("processOrderStateChangeNotification Exception: " + exc.Message + ": " + exc.StackTrace);
            }
        }
        private void processErrorNotification(string xmlData)
        {
            try
            {
                ErrorResponse errorResponse = (ErrorResponse)EncodeHelper.Deserialize(xmlData, typeof(ErrorResponse));

                StringBuilder errorSB = new StringBuilder();
                errorSB.Append(string.Format("Error response message received: {0}", errorResponse.errormessage));
                foreach (string Warning in errorResponse.warningmessages)
                    errorSB.Append("Warning: " + Warning);
                string message = errorSB.ToString();
                logMessage(message);
            }
            catch (Exception exc)
            {
                logMessage("processErrorNotification Exception: " + exc.Message + ": " + exc.StackTrace);
            }
        }
        private void processRiskInformationNotification(string xmlData)
        {
            RiskInformationNotification riskInformationNotification = (RiskInformationNotification)EncodeHelper.Deserialize(xmlData, typeof(RiskInformationNotification));
             
            StringBuilder riskSB = new StringBuilder();
            riskSB.Append("Risk Information: ");
            riskSB.Append("googleordernumber: ");
            riskSB.Append(riskInformationNotification.googleordernumber);
            riskSB.Append(", avsresponse: ");
            riskSB.Append(riskInformationNotification.riskinformation.avsresponse);
            riskSB.Append(", ipaddress: ");
            riskSB.Append(riskInformationNotification.riskinformation.ipaddress);
            riskSB.Append(", partialccnumber: ");
            riskSB.Append(riskInformationNotification.riskinformation.partialccnumber);
            string message = riskSB.ToString();
            logMessage(message);

            Order order = getMerchantOrderByGoogleOrderID(riskInformationNotification.googleordernumber);
            if (order != null)
            {
                OrderManager.InsertOrderNote(order.OrderID, message, DateTime.Now);
            }
        }
        private Order getMerchantOrderByGoogleOrderID(string GoogleOrderID)
        {
            if (String.IsNullOrEmpty(GoogleOrderID))
                return null;

            PaymentMethod googleCheckoutPaymentMethod = PaymentMethodManager.GetPaymentMethodBySystemKeyword("GoogleCheckout");
            if (googleCheckoutPaymentMethod == null)
                return null;

            return OrderManager.GetOrderByAuthorizationTransactionIDAndPaymentMethodID(GoogleOrderID, googleCheckoutPaymentMethod.PaymentMethodID);
        }

        #endregion

        #region Methods
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
            processPaymentResult.AuthorizationTransactionID = paymentInfo.GoogleOrderNumber;

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
        /// Process google callback request
        /// </summary>
        /// <param name="xmlData">xml data</param>
        public void ProcessCallBackRequest(string xmlData)
        {
            if (String.IsNullOrEmpty(xmlData))
                return;

            try
            {
                string commandName = EncodeHelper.GetTopElement(xmlData);
                logMessage(string.Format("Google callback command: {0}", commandName));
                logMessage(string.Format("Raw xml request: {0}", xmlData));

                switch (commandName)
                {
                    case "new-order-notification":
                        processNewOrderNotification(xmlData);
                        break;
                    case "order-state-change-notification":
                        processOrderStateChangeNotification(xmlData);
                        break;
                    case "risk-information-notification":
                        processRiskInformationNotification(xmlData);
                        break;
                    case "error":
                        processErrorNotification(xmlData);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception exc)
            {
                logMessage(string.Format("An error occurred: {0}", exc));
            }
        }

        /// <summary>
        /// Captures payment (from admin panel)
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="processPaymentResult">Process payment result</param>
        public void Capture(Order order, ref ProcessPaymentResult processPaymentResult)
        {
            string googleOrderNumber = processPaymentResult.AuthorizationTransactionID;
            GCheckout.OrderProcessing.ChargeOrderRequest chargeOrderRequest = new GCheckout.OrderProcessing.ChargeOrderRequest(googleOrderNumber);
            GCheckoutResponse chargeOrderResponse = chargeOrderRequest.Send();
            if (chargeOrderResponse.IsGood)
            {
                processPaymentResult.PaymentStatus = PaymentStatusEnum.Paid;
                processPaymentResult.CaptureTransactionResult = chargeOrderResponse.ResponseXml;
            }
            else
            {
                processPaymentResult.Error = chargeOrderResponse.ErrorMessage;
            }
        }

        /// <summary>
        /// Post cart to google
        /// </summary>
        /// <param name="Req">Pre-generated request</param>
        /// <param name="Cart">Shopping cart</param>
        /// <returns>Response</returns>
        public GCheckoutResponse PostCartToGoogle(CheckoutShoppingCartRequest Req, NopSolutions.NopCommerce.BusinessLogic.Orders.ShoppingCart Cart)
        {
            foreach (NopSolutions.NopCommerce.BusinessLogic.Orders.ShoppingCartItem sci in Cart)
            {
                ProductVariant productVariant = sci.ProductVariant;
                if (productVariant != null)
                {
                    string pvAttributeDescription = ProductAttributeHelper.FormatAttributes(productVariant, sci.AttributesXML, NopContext.Current.User, ", ", false);
                    string fullName = productVariant.FullProductName;
                    string description = pvAttributeDescription;
                    decimal unitPrice = TaxManager.GetPrice(sci.ProductVariant, PriceHelper.GetUnitPrice(sci, NopContext.Current.User, true));
                    Req.AddItem(fullName, description, sci.ShoppingCartItemID.ToString(), unitPrice, sci.Quantity);
                }
            }

            decimal shoppingCartSubTotalDiscount;
            decimal shoppingCartSubTotal = ShoppingCartManager.GetShoppingCartSubTotal(Cart, NopContext.Current.User, out shoppingCartSubTotalDiscount);
            if (shoppingCartSubTotalDiscount > decimal.Zero)
                Req.AddItem("Discount", string.Empty, string.Empty, (decimal)(-1.0) * shoppingCartSubTotalDiscount, 1);

            bool shoppingCartRequiresShipping = ShippingManager.ShoppingCartRequiresShipping(Cart);
            if (shoppingCartRequiresShipping)
            {
                string shippingError = string.Empty;
                //TODO AddMerchantCalculatedShippingMethod
                //TODO AddCarrierCalculatedShippingOption
                ShippingOptionCollection shippingOptions = ShippingManager.GetShippingOptions(Cart, NopContext.Current.User, null, ref shippingError);
                foreach (ShippingOption shippingOption in shippingOptions)
                    Req.AddFlatRateShippingMethod(shippingOption.Name, TaxManager.GetShippingPrice(shippingOption.Rate, NopContext.Current.User));
            }

            //add only US, GB states
            //CountryCollection countries = CountryManager.GetAllCountries();                
            //foreach (Country country in countries)
            //{
            //    foreach (StateProvince state in country.StateProvinces)
            //    {
            //        TaxByStateProvinceCollection taxByStateProvinceCollection = TaxByStateProvinceManager.GetAllByStateProvinceID(state.StateProvinceID);
            //        foreach (TaxByStateProvince taxByStateProvince in taxByStateProvinceCollection)
            //        {
            //            if (!String.IsNullOrEmpty(state.Abbreviation))
            //            {
            //                Req.AddStateTaxRule(state.Abbreviation, (double)taxByStateProvince.Percentage, false);
            //            }
            //        }
            //    }
            //}

            XmlDocument customerInfoDoc = new XmlDocument();
            XmlElement customerInfo = customerInfoDoc.CreateElement("CustomerInfo");
            customerInfo.SetAttribute("CustomerID", NopContext.Current.User.CustomerID.ToString());
            customerInfo.SetAttribute("CustomerLanguageID", NopContext.Current.WorkingLanguage.LanguageID.ToString());
            customerInfo.SetAttribute("CustomerCurrencyID", NopContext.Current.WorkingCurrency.CurrencyID.ToString());
            Req.AddMerchantPrivateDataNode(customerInfo);

            Req.ContinueShoppingUrl = CommonHelper.GetStoreLocation(false);
            Req.EditCartUrl = CommonHelper.GetStoreLocation(false) + "ShoppingCart.aspx";

            GCheckoutResponse Resp = Req.Send();

            return Resp;
        }

        /// <summary>
        /// Get notification acknowledgment text
        /// </summary>
        /// <returns>Notification ack</returns>
        public string GetNotificationAcknowledgmentText()
        {
            NotificationAcknowledgment gNotificationAcknowledgment = new NotificationAcknowledgment();

            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                XmlSerializer serResponse = new XmlSerializer(gNotificationAcknowledgment.GetType(), "http://checkout.google.com/schema/2");
                serResponse.Serialize(sw, gNotificationAcknowledgment);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Verifies message authentication
        /// </summary>
        /// <param name="authStr">Authenticatio string</param>
        /// <returns>Result</returns>
        public bool VerifyMessageAuthentication(string authStr)
        {
            bool result = false;
            System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration("~");

            if (!Convert.ToBoolean(config.AppSettings.Settings["GoogleAuthenticateCallback"].Value))
            {
                result = true;
            }
            else if (String.IsNullOrEmpty(authStr) || authStr.IndexOf("Basic", 0) != 0)
            {
                result = false;
            }
            else
            {
                byte[] decodedBytes = Convert.FromBase64String(authStr.Trim().Substring(6));
                string decodedAuthString = Encoding.ASCII.GetString(decodedBytes);

                string username = decodedAuthString.Split(':')[0];
                string password = decodedAuthString.Split(':')[1];

                string merchantID = config.AppSettings.Settings["GoogleMerchantID"].Value;
                string merchantKey = config.AppSettings.Settings["GoogleMerchantKey"].Value;

                result = (username == merchantID && password == merchantKey);
            }

            logMessage(string.Format("callback authorization result = {0}", result));

            return result;
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
