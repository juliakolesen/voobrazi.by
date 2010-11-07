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
// Contributor(s): Asif Raza Ashraf. 
// Email: 5177637[AT]gmail[DOT]com
//------------------------------------------------------------------------------


using System;
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

namespace NopSolutions.NopCommerce.Payment.Methods.CDGcommerce
{
	/// <summary>
	/// CDGcommerce payment processor
	/// </summary>
	public class CDGcommercePaymentProcessor : IPaymentMethod
	{
		#region Fields

		private string loginID;
		private string RestrictKey;

		#endregion

		#region Methods


		/// <summary>
		/// Process payment
		/// </summary>
		/// <param name="paymentInfo">Payment info required for an order processing</param>
		/// <param name="customer">Customer</param>
		/// <param name="OrderGuid">Unique order identifier</param>
		/// <param name="processPaymentResult">Process payment result</param>
        public void ProcessPayment(PaymentInfo paymentInfo, Customer customer, Guid OrderGuid, ref
		                           ProcessPaymentResult processPaymentResult)
		{
			InitSettings();
			
			WebClient webClient = new WebClient();
			NameValueCollection form = new NameValueCollection();
			form.Add("gwlogin", loginID);
			if (!string.IsNullOrEmpty(RestrictKey))
				form.Add("RestrictKey", RestrictKey);
			form.Add("trans_method", "CC");
			form.Add("CVVtype", "1");
            form.Add("Dsep", "|");
			form.Add("MAXMIND", "1");

			form.Add("amount", paymentInfo.OrderTotal.ToString("####.00", new CultureInfo("en-US", false).NumberFormat));
			form.Add("ccnum", paymentInfo.CreditCardNumber);
			form.Add("ccmo", paymentInfo.CreditCardExpireMonth.ToString("D2"));
			form.Add("ccyr", paymentInfo.CreditCardExpireYear.ToString());
			form.Add("CVV2", paymentInfo.CreditCardCVV2);

			form.Add("FNAME", paymentInfo.BillingAddress.FirstName);
			form.Add("LNAME", paymentInfo.BillingAddress.LastName);

			if (string.IsNullOrEmpty(paymentInfo.BillingAddress.Company))
				form.Add("company", paymentInfo.BillingAddress.Company);

			form.Add("BADDR1", paymentInfo.BillingAddress.Address1);
			form.Add("BCITY", paymentInfo.BillingAddress.City);
			if (paymentInfo.BillingAddress.StateProvince != null)
				form.Add("BSTATE", paymentInfo.BillingAddress.StateProvince.Name);
			form.Add("BZIP1", paymentInfo.BillingAddress.ZipPostalCode);
			if (paymentInfo.BillingAddress.Country != null)
				form.Add("BCOUNTRY", paymentInfo.BillingAddress.Country.TwoLetterISOCode);
			form.Add("invoice_num", OrderGuid.ToString());
			form.Add("customer_ip", HttpContext.Current.Request.UserHostAddress);
			form.Add("BCUST_EMAIL", paymentInfo.BillingAddress.Email);


			string reply = null;
			Byte[] responseData = webClient.UploadValues(GetCDGcommerceUrl(), form);
			reply = Encoding.ASCII.GetString(responseData);

			if (null != reply)
			{
				processPaymentResult.AuthorizationTransactionResult = reply;

				string[] responseFields = reply.Split('|');
				switch (responseFields[0])
				{
					case "\"APPROVED\"":
						processPaymentResult.AuthorizationTransactionCode = responseFields[1];
						processPaymentResult.AVSResult = "AVRResponse: " + responseFields[3] + " Max Score:" + responseFields[5];
						processPaymentResult.AuthorizationTransactionID = responseFields[2]; //responseFields[38];
						processPaymentResult.PaymentStatus = PaymentStatusEnum.Paid;
						break;
					case "\"DECLINED\"":
						processPaymentResult.Error = responseFields[6];
						processPaymentResult.FullError = responseFields[7] + " " + responseFields[6];
						break;
				}
			}
			else
			{
				processPaymentResult.Error = "CDGcommerce unknown error";
				processPaymentResult.FullError = "CDGcommerce unknown error";
			}
		}

		/// <summary>
		/// Post process payment (payment gateways that require posting data)
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
		/// Initializes the CDGcommerce payment processor
		/// </summary>
		private void InitSettings()
		{
			RestrictKey = SettingManager.GetSettingValue("PaymentMethod.CDGcommerce.RestrictKey");
			loginID = SettingManager.GetSettingValue("PaymentMethod.CDGcommerce.LoginID");

			if (string.IsNullOrEmpty(loginID))
                throw new NopException("CDGcommerce API login ID is not set");
		}

		/// <summary>
		/// Gets CDGcommerce URL
		/// </summary>
		/// <returns></returns>
		private string GetCDGcommerceUrl()
		{
			return "https://secure.quantumgateway.com/cgi/tqgwdbe.php";
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
        
        #region Properties

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