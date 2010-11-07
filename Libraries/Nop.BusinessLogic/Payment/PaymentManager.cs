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
using System.Text;
using System.Web.Compilation;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.BusinessLogic.Shipping;
using NopSolutions.NopCommerce.Common;

namespace NopSolutions.NopCommerce.BusinessLogic.Payment
{
    /// <summary>
    /// Payment manager
    /// </summary>
    public partial class PaymentManager
    {
        #region Methods
        /// <summary>
        /// Process payment
        /// </summary>
        /// <param name="paymentInfo">Payment info required for an order processing</param>
        /// <param name="customer">Customer</param>
        /// <param name="OrderGuid">Unique order identifier</param>
        /// <param name="processPaymentResult">Process payment result</param>
        public static void ProcessPayment(PaymentInfo paymentInfo, Customer customer, Guid OrderGuid, ref ProcessPaymentResult processPaymentResult)
        {
            if (paymentInfo.OrderTotal == decimal.Zero)
            {
                processPaymentResult.Error = string.Empty;
                processPaymentResult.FullError = string.Empty;
                processPaymentResult.PaymentStatus = PaymentStatusEnum.Paid;
            }
            else
            {
                PaymentMethod paymentMethod = PaymentMethodManager.GetPaymentMethodByID(paymentInfo.PaymentMethodID);
                if (paymentMethod == null)
                    throw new NopException("Payment method couldn't be loaded");
                IPaymentMethod iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
                iPaymentMethod.ProcessPayment(paymentInfo, customer, OrderGuid, ref processPaymentResult);
            }
        }

        /// <summary>
        /// Post process payment (payment gateways that require redirecting)
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>The error status, or String.Empty if no errors</returns>
        public static string PostProcessPayment(Order order)
        {
            //already paid or order.OrderTotal == decimal.Zero
            if (order.PaymentStatus == PaymentStatusEnum.Paid)
                return string.Empty;

            PaymentMethod paymentMethod = PaymentMethodManager.GetPaymentMethodByID(order.PaymentMethodID);
            if (paymentMethod == null)
                throw new NopException("Payment method couldn't be loaded");
            IPaymentMethod iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
            return iPaymentMethod.PostProcessPayment(order);
        }

        /// <summary>
        /// Gets additional handling fee
        /// </summary>
        /// <param name="PaymentMethodID">Payment method identifier</param>
        /// <returns>Additional handling fee</returns>
        public static decimal GetAdditionalHandlingFee(int PaymentMethodID)
        {
            PaymentMethod paymentMethod = PaymentMethodManager.GetPaymentMethodByID(PaymentMethodID);
            if (paymentMethod == null)
                return decimal.Zero;
            IPaymentMethod iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
            return iPaymentMethod.GetAdditionalHandlingFee();
        }

        /// <summary>
        /// Gets a value indicating whether capture is allowed from admin panel
        /// </summary>
        /// <param name="PaymentMethodID">Payment method identifier</param>
        /// <returns>A value indicating whether capture is allowed from admin panel</returns>
        public static bool CanCapture(int PaymentMethodID)
        {
            PaymentMethod paymentMethod = PaymentMethodManager.GetPaymentMethodByID(PaymentMethodID);
            if (paymentMethod == null)
                return false;
            IPaymentMethod iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
            return iPaymentMethod.CanCapture;
        }
        
        /// <summary>
        /// Captures payment (from admin panel)
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="processPaymentResult">Process payment result</param>
        public static void Capture(Order order, ref ProcessPaymentResult processPaymentResult)
        {
            PaymentMethod paymentMethod = PaymentMethodManager.GetPaymentMethodByID(order.PaymentMethodID);
            if (paymentMethod == null)
                throw new NopException("Payment method couldn't be loaded");
            IPaymentMethod iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
            iPaymentMethod.Capture(order, ref processPaymentResult);
        }

        /// <summary>
        /// Gets masked credit card number
        /// </summary>
        /// <param name="CreditCardNumber">Credit card number</param>
        /// <returns>Masked credit card number</returns>
        public static string GetMaskedCreditCardNumber(string CreditCardNumber)
        {
            if (String.IsNullOrEmpty(CreditCardNumber))
                return string.Empty;

            if (CreditCardNumber.Length <= 4)
                return CreditCardNumber;

            string last4 = CreditCardNumber.Substring(CreditCardNumber.Length - 4, 4);
            string maskedChars = string.Empty;
            for (int i = 0; i < CreditCardNumber.Length - 4; i++)
            {
                maskedChars += "*";
            }
            return maskedChars + last4;
        }
        #endregion
    }
}
