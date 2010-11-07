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
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Utils;
using NopSolutions.NopCommerce.Common;
using NopSolutions.NopCommerce.Common.Utils;


namespace NopSolutions.NopCommerce.Payment.Methods.TwoCheckout
{
    /// <summary>
    /// 2Checkout payment processor
    /// </summary>
    public class TwoCheckoutPaymentProcessor : IPaymentMethod
    {
        #region Fields
        private bool useSandBox = true;
        private string vendorID;
        private string serverURL = "https://www.2checkout.com/2co/buyer/purchase";
        #endregion

        #region Ctor
        /// <summary>
        /// Creates a new instance of the TwoCheckoutPaymentProcessor class
        /// </summary>
        public TwoCheckoutPaymentProcessor()
        {
            useSandBox = SettingManager.GetSettingValueBoolean("PaymentMethod.TwoCheckout.UseSandbox");
            vendorID = SettingManager.GetSettingValue("PaymentMethod.TwoCheckout.VendorID");

            if (string.IsNullOrEmpty(vendorID))
                throw new NopException("2Checkout Vendor Id is empty");
        }
        #endregion

        #region Utilities
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
        }

        /// <summary>
        /// Post process payment (payment gateways that require redirecting)
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>The error status, or String.Empty if no errors</returns>
        public string PostProcessPayment(Order order)
        {
            string returnURL = CommonHelper.GetStoreLocation(false) + "TwoCheckoutReturn.aspx";

            RemotePost remotePostHelper = new RemotePost();
            remotePostHelper.FormName = "TwoCheckoutForm";
            remotePostHelper.Url = serverURL;

            remotePostHelper.Add("id_type", "1");

            OrderProductVariantCollection orderProductVariants = order.OrderProductVariants;
            for (int i = 0; i < orderProductVariants.Count; i++)
            {
                int pNum = i + 1;
                OrderProductVariant opv = orderProductVariants[i];
                ProductVariant pv = orderProductVariants[i].ProductVariant;
                Product product = pv.Product;

                string c_prod = string.Format("c_prod_{0}", pNum);
                string c_prod_value = string.Format("{0},{1}", pv.SKU, opv.Quantity);
                remotePostHelper.Add(c_prod, c_prod_value);
                string c_name = string.Format("c_name_{0}", pNum);
                string c_name_value = pv.FullProductName;
                string c_attributes = opv.AttributeDescription;
                if (!String.IsNullOrEmpty(c_attributes))
                {
                    c_name_value += c_attributes;
                    c_name_value = c_name_value.Replace("<br />", ". ");
                }
                remotePostHelper.Add(c_name, c_name_value);

                string c_description = string.Format("c_description_{0}", pNum);
                string c_description_value = pv.FullProductName;
                c_description_value = c_description_value.Replace("<br />", ". ");
                remotePostHelper.Add(c_description, c_description_value);

                string c_price = string.Format("c_price_{0}", pNum);
                string c_price_value = opv.UnitPriceInclTax.ToString("####.00", new CultureInfo("en-US", false).NumberFormat);
                remotePostHelper.Add(c_price, c_price_value);

                string c_tangible = string.Format("c_tangible_{0}", pNum);
                string c_tangible_value = "Y";
                if (pv.IsDownload)
                {
                    c_tangible_value = "N";
                }
                remotePostHelper.Add(c_tangible, c_tangible_value);
            }

            remotePostHelper.Add("x_login", vendorID);
            remotePostHelper.Add("x_amount", order.OrderTotal.ToString("####.00", new CultureInfo("en-US", false).NumberFormat));
            //TODO add primary store currency to parameters
            remotePostHelper.Add("x_invoice_num", order.OrderID.ToString());
            //remotePostHelper.Add("x_receipt_link_url", returnURL);
            //remotePostHelper.Add("x_return_url", returnURL);
            //remotePostHelper.Add("x_return", returnURL);
            if (useSandBox)
                remotePostHelper.Add("demo", "Y");
            remotePostHelper.Add("x_First_Name", order.BillingFirstName);
            remotePostHelper.Add("x_Last_Name", order.BillingLastName);
            remotePostHelper.Add("x_Address", order.BillingAddress1);
            remotePostHelper.Add("x_City", order.BillingCity);
            StateProvince billingStateProvince = StateProvinceManager.GetStateProvinceByID(order.BillingStateProvinceID);
            if (billingStateProvince != null)
                remotePostHelper.Add("x_State", billingStateProvince.Abbreviation);
            else
                remotePostHelper.Add("x_State", order.BillingStateProvince);
            remotePostHelper.Add("x_Zip", order.BillingZipPostalCode);
            Country billingCountry = CountryManager.GetCountryByID(order.BillingCountryID);
            if (billingCountry != null)
                remotePostHelper.Add("x_Country", billingCountry.ThreeLetterISOCode);
            else
                remotePostHelper.Add("x_Country", order.BillingCountry);

            remotePostHelper.Add("x_EMail", order.BillingEmail);
            remotePostHelper.Add("x_Phone", order.BillingPhoneNumber);
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