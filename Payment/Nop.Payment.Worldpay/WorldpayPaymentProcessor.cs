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


namespace NopSolutions.NopCommerce.Payment.Methods.Worldpay
{
    /// <summary>
    /// Worldpay payment processor
    /// </summary>
    public class WorldpayPaymentProcessor : IPaymentMethod
    {
        #region Fields
        private bool useSandBox = true;
        private string instanceID;
        #endregion

        #region Ctor
        /// <summary>
        /// Creates a new instance of the WorldpayPaymentProcessor class
        /// </summary>
        public WorldpayPaymentProcessor()
        {
            useSandBox = SettingManager.GetSettingValueBoolean("PaymentMethod.Worldpay.UseSandbox");
            instanceID = SettingManager.GetSettingValue("PaymentMethod.Worldpay.InstanceID");

            if (string.IsNullOrEmpty(instanceID))
                throw new NopException("Worldpay Instance ID is not set");
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
        }
        
        /// <summary>
        /// Post process payment (payment gateways that require redirecting)
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>The error status, or String.Empty if no errors</returns>
        public string PostProcessPayment(Order order)
        {
            string returnURL = CommonHelper.GetStoreLocation(false) + "WorldpayReturn.aspx";

            RemotePost remotePostHelper = new RemotePost();
            remotePostHelper.FormName = "WorldpayForm";
            remotePostHelper.Url =  GetWorldpayUrl();

            remotePostHelper.Add("instId", instanceID);
            remotePostHelper.Add("cartId", order.OrderID.ToString());
            remotePostHelper.Add("currency", "USD");
            remotePostHelper.Add("amount", order.OrderTotal.ToString(new CultureInfo("en-US", false).NumberFormat));
            remotePostHelper.Add("desc", SettingManager.StoreName);
            remotePostHelper.Add("M_UserID", order.CustomerID.ToString());
            remotePostHelper.Add("M_FirstName", order.BillingFirstName);
            remotePostHelper.Add("M_LastName", order.BillingLastName);
            remotePostHelper.Add("M_Addr1", order.BillingAddress1);
            remotePostHelper.Add("tel", order.BillingPhoneNumber);
            remotePostHelper.Add("M_Addr2", order.BillingAddress2);
            remotePostHelper.Add("M_Business", order.BillingCompany);

            StateProvince billingStateProvince = StateProvinceManager.GetStateProvinceByID(order.BillingStateProvinceID);
            if (billingStateProvince != null)
                remotePostHelper.Add("M_StateCounty", billingStateProvince.Abbreviation);
            else
                remotePostHelper.Add("M_StateCounty", order.BillingStateProvince);
            if (!useSandBox)
                remotePostHelper.Add("testMode", "0");
            else
                remotePostHelper.Add("testMode", "100");
            //TODO remotePostHelper.Add("testMode", "101");
            remotePostHelper.Add("postcode", order.BillingZipPostalCode);
            Country billingCountry = CountryManager.GetCountryByID(order.BillingCountryID);
            if (billingCountry != null)
                remotePostHelper.Add("country", billingCountry.TwoLetterISOCode);
            else
                remotePostHelper.Add("country", order.BillingCountry);
            remotePostHelper.Add("email", order.BillingEmail);
            remotePostHelper.Add("address", order.BillingAddress1 + "," + order.BillingCountry);
            remotePostHelper.Add("MC_callback", returnURL);
            remotePostHelper.Add("name", order.BillingFirstName + " " + order.BillingLastName);
            
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
        /// Gets Worldpay URL
        /// </summary>
        /// <returns></returns>
        private string GetWorldpayUrl()
        {
            return useSandBox ? "https://select-test.worldpay.com/wcc/purchase" :
                "https://select.worldpay.com/wcc/purchase";
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