
using System;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;

using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Infrastructure;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.Common.Utils;

namespace Nop.Payment.WebMoney
{
    public class WebMoneyPaymentProcessor : IPaymentMethod
    {
        public void ProcessPayment(PaymentInfo paymentInfo, Customer customer, Guid orderGuid, ref ProcessPaymentResult processPaymentResult)
        {
            processPaymentResult.PaymentStatus = PaymentStatusEnum.Pending;
        }

        public string PostProcessPayment(Order order)
        {
            var remotePostHelper = new RemotePost { FormName = "WebMoneyForm", Url = GetWebMoneyUrl() };

            int percentsToAddToWeighItemsPrice = int.Parse(ConfigurationManager.AppSettings["PercentsToAddToWeighItemsPrice"]);
            decimal orderTotalNoSpread = order.OrderProductVariants.Sum(orderProductVariant => orderProductVariant.Quantity * orderProductVariant.UnitPriceExclTax);
            decimal orderTmpTotalWithSpread = order.OrderProductVariants.Sum(orderProductVariant => PaymentService.GetTmpPriceForItem(orderProductVariant, percentsToAddToWeighItemsPrice, false));
            orderTmpTotalWithSpread = PaymentService.GetPriceWithShipping(orderTotalNoSpread, orderTmpTotalWithSpread);

            order.OrderTotalWithSpread = orderTmpTotalWithSpread;
            IoC.Resolve<OrderService>().UpdateOrder(order);

#if(!RELEASE)
            orderTmpTotalWithSpread = 1;
#endif

            remotePostHelper.Add("LMI_PAYEE_PURSE", IoC.Resolve<ISettingManager>().GetSettingValue("PaymentMethod.WebMoney.WMID"));
            remotePostHelper.Add("LMI_PAYMENT_AMOUNT", ((int)orderTmpTotalWithSpread).ToString());
            remotePostHelper.Add("LMI_PAYMENT_NO", order.OrderId.ToString());

            var orderDescription = new StringBuilder();
            foreach (var pv in order.OrderProductVariants)
                orderDescription.AppendFormat("- {0} x {1} = {2}{3}", pv.ProductVariant.Product.Name, pv.Quantity, ((int)PaymentService.GetTmpPriceForItem(pv, percentsToAddToWeighItemsPrice, true)), Environment.NewLine);
            var orderDescriptionString = orderDescription.Length > 252 ? string.Format("{0}...", orderDescription.ToString().Substring(0, 252)) : orderDescription.ToString();

            remotePostHelper.Add("LMI_PAYMENT_DESC", HttpContext.Current.Server.HtmlEncode(orderDescriptionString));

            remotePostHelper.Add("LMI_MODE", UseSandBox ? "1" : "0");

#if(!RELEASE)
            remotePostHelper.Add("LMI_RESULT_URL", "http://www.stg.dodivana.by/WebMoney.ashx");
            remotePostHelper.Add("LMI_SUCCESS_URL", "http://www.stg.dodivana.by/WebMoneySucceeded.ashx");
            remotePostHelper.Add("LMI_SUCCESS_METHOD", "GET");
            remotePostHelper.Add("LMI_FAIL_URL", "http://www.stg.dodivana.by/WebMoneyFail.ashx");
            remotePostHelper.Add("LMI_FAIL_METHOD", "GET");
#endif

            if (UseSandBox)
                remotePostHelper.Add("LMI_SIM_MODE", IoC.Resolve<ISettingManager>().GetSettingValue("PaymentMethod.WebMoney.SimulationMode"));

            remotePostHelper.Add("OrderId", order.OrderId.ToString());

            remotePostHelper.Post(Encoding.GetEncoding(1251));
            return string.Empty;
        }

        public static string Purse
        {
            get
            {
                return "B269238757313";
            }
        }

        public static bool UseSandBox
        {
            get
            {
                try
                {
                    return IoC.Resolve<ISettingManager>().GetSettingValueBoolean("PaymentMethod.WebMoney.UseSandbox");
                }
                catch (Exception) { }
                return false;
            }
        }

        private static string GetWebMoneyUrl()
        {
            return "https://merchant.webmoney.ru/lmi/payment.asp";
        }

        public decimal GetAdditionalHandlingFee()
        {
            return decimal.Zero;
        }

        public void Capture(Order order, ref ProcessPaymentResult processPaymentResult)
        {
            throw new NotImplementedException();
        }

        public void Refund(Order order, ref CancelPaymentResult cancelPaymentResult)
        {
            throw new NotImplementedException();
        }

        public void Void(Order order, ref CancelPaymentResult cancelPaymentResult)
        {
            throw new NotImplementedException();
        }

        public void ProcessRecurringPayment(PaymentInfo paymentInfo, Customer customer, Guid orderGuid, ref ProcessPaymentResult processPaymentResult)
        {
            throw new NotImplementedException();
        }

        public void CancelRecurringPayment(Order order, ref CancelPaymentResult cancelPaymentResult)
        {
            throw new NotImplementedException();
        }

        public bool CanCapture
        {
            get
            {
                return false;
            }
        }

        public bool CanPartiallyRefund
        {
            get
            {
                return false;
            }
        }

        public bool CanRefund
        {
            get
            {
                return false;
            }
        }

        public bool CanVoid
        {
            get
            {
                return false;
            }
        }

        public RecurringPaymentTypeEnum SupportRecurringPayments
        {
            get
            {
                return RecurringPaymentTypeEnum.NotSupported;
            }
        }

        public PaymentMethodTypeEnum PaymentMethodType
        {
            get
            {
                return PaymentMethodTypeEnum.Standard;
            }
        }
    }
}
