
using System;
using System.Configuration;
using System.Linq;
using System.Text;

using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Infrastructure;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.BusinessLogic.Security;

namespace Nop.Payment.EasyPay
{
    public class EasyPayException : Exception
    {
        public EasyPayException(string message)
            : base(message)
        {
        }
    }

    public class EasyPayPaymentProcessor : IPaymentMethod
    {
        public void ProcessPayment(PaymentInfo paymentInfo, Customer customer, Guid orderGuid, ref ProcessPaymentResult processPaymentResult)
        {
            processPaymentResult.PaymentStatus = PaymentStatusEnum.Pending;
        }

        private static EasyPayService easyPayService;

        public string PostProcessPayment(Order order)
        {
            if (easyPayService == null)
                easyPayService = new EasyPayService(UseSandbox) { RequestEncoding = Encoding.GetEncoding(1251) };

            int percentsToAddToWeighItemsPrice = int.Parse(ConfigurationManager.AppSettings["PercentsToAddToWeighItemsPrice"]);
            decimal orderTotalNoSpread = order.OrderProductVariants.Sum(orderProductVariant => orderProductVariant.Quantity * orderProductVariant.UnitPriceExclTax);
            decimal orderTmpTotalWithSpread = order.OrderProductVariants.Sum(orderProductVariant => PaymentService.GetTmpPriceForItem(orderProductVariant, percentsToAddToWeighItemsPrice, false));
            orderTmpTotalWithSpread = PaymentService.GetPriceWithShipping(orderTotalNoSpread, orderTmpTotalWithSpread);

            order.OrderTotalWithSpread = orderTmpTotalWithSpread;
            IoC.Resolve<OrderService>().UpdateOrder(order);

            var orderDescription = new StringBuilder();
            foreach (var pv in order.OrderProductVariants)
                orderDescription.AppendFormat("- {0} x {1} = {2}{3}", pv.ProductVariant.Product.Name, pv.Quantity, PaymentService.GetTmpPriceForItem(pv, percentsToAddToWeighItemsPrice, true), Environment.NewLine);
            var orderDescriptionString = orderDescription.Length > 1999 ? string.Format("{0}...", orderDescription.ToString().Substring(0, 1996)) : orderDescription.ToString();
            int eazyPayInvoiceLivingTimeSec = int.Parse(ConfigurationManager.AppSettings["EazyPayInvoiceLivingTimeSec"]);
            Status status = easyPayService.EP_CreateInvoice(MerchantNumber, P, order.OrderId.ToString(), orderTmpTotalWithSpread, eazyPayInvoiceLivingTimeSec, SecurityHelper.Decrypt(order.CardNumber), "Заказ в интернет-гипермаркете До Дивана", orderDescriptionString, string.Empty);

            //var status = new Status { code = 200 };
            if (status.code != 200 && status.code != 211)
                throw new EasyPayException(string.Format("Не удалось создать счет в платежной системе EasyPay. Код ошибки: {0}. Ошибка: {1}", status.code, status.message));

            return "";
        }

        public static string MethodName
        {
            get
            {
                return "EasyPay";
            }
        }

        public static bool UseSandbox
        {
            get
            {
                try
                {
                    return IoC.Resolve<SettingManager>().GetSettingValueBoolean("PaymentMethod.EasyPay.UseSandbox");
                }
                catch { }
                return true;
            }
        }

        public static string MerchantNumber
        {
            get
            {
                return UseSandbox ? "ok6666" : "ok0743";
            }
        }

        public static string P
        {
            get
            {
                return UseSandbox ? "rubin" : "gjhHRGUhtuh6t7y8et";
            }
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
