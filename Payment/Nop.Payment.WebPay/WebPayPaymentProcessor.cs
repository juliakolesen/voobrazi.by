using System;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Infrastructure;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.Common.Utils;

namespace Nop.Payment.WebPay
{
    public class WebPayPaymentProcessor : IPaymentMethod
    {
        public static string SK
        {
            get
            {
                return UseSandBox ? "06FE0178B956470E8D8A80C487DAB748" : "E80CF105F526463A83DBD61EA22160C0";
            }
        }

        public static string StoreId
        {
            get
            {
                return UseSandBox ? "581574924" : "238278039";
            }
        }

        public static string CurrencyId
        {
            get
            {
                return "BYR";
            }
        }

        public void ProcessPayment(PaymentInfo paymentInfo, Customer customer, Guid orderGuid, ref ProcessPaymentResult processPaymentResult)
        {
            processPaymentResult.PaymentStatus = PaymentStatusEnum.Pending;
        }

        public string PostProcessPayment(Order order)
        {
            var remotePostHelper = new RemotePost { FormName = "WebPeyForm", Url = GetWebPayUrl() };

            int percentsToAddToWeighItemsPrice = int.Parse(ConfigurationManager.AppSettings["PercentsToAddToWeighItemsPrice"]);
            decimal orderTotalNoSpread = order.OrderProductVariants.Sum(orderProductVariant => orderProductVariant.Quantity * orderProductVariant.UnitPriceExclTax);
            decimal orderTmpTotalWithSpread = order.OrderProductVariants.Sum(orderProductVariant => PaymentService.GetTmpPriceForItem(orderProductVariant, percentsToAddToWeighItemsPrice, false));
            orderTmpTotalWithSpread = PaymentService.GetPriceWithShipping(orderTotalNoSpread, orderTmpTotalWithSpread);

            var shippingPrice = orderTotalNoSpread < PaymentService.FreeShippingBorder ? PaymentService.ShippingCost : 0;

            order.OrderTotalWithSpread = orderTmpTotalWithSpread;
            IoC.Resolve<OrderService>().UpdateOrder(order);

            remotePostHelper.Add("*scart", "");
            remotePostHelper.Add("wsb_storeid", StoreId);
            remotePostHelper.Add("wsb_store", "«До дивана»");
            remotePostHelper.Add("wsb_order_num", order.OrderId.ToString());
            remotePostHelper.Add("wsb_currency_id", CurrencyId);
            remotePostHelper.Add("wsb_version", "2");
            remotePostHelper.Add("wsb_language_id", "russian");
            string seed = ((int)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds).ToString();
            remotePostHelper.Add("wsb_seed", seed);

            var signatureBulder = new StringBuilder(seed);
            signatureBulder.Append(StoreId);
            signatureBulder.Append(order.OrderId);
            signatureBulder.Append(UseSandBox ? "1" : "0");
            signatureBulder.Append(CurrencyId);
            signatureBulder.Append(((int)orderTmpTotalWithSpread).ToString());
            signatureBulder.Append(SK);

            byte[] buffer = Encoding.Default.GetBytes(signatureBulder.ToString());
            var sha1 = new SHA1CryptoServiceProvider();
            string signature = BitConverter.ToString(sha1.ComputeHash(buffer)).Replace("-", "").ToLower();
            remotePostHelper.Add("wsb_signature", signature);
            remotePostHelper.Add("wsb_return_url", string.Format(IoC.Resolve<ISettingManager>().GetSettingValue("PaymentMethod.WebPay.ReturnUrl"), order.OrderId));
            remotePostHelper.Add("wsb_cancel_return_url", string.Format(IoC.Resolve<ISettingManager>().GetSettingValue("PaymentMethod.WebPay.CancelUrl"), order.OrderId));
            remotePostHelper.Add("wsb_notify_url", string.Format(IoC.Resolve<ISettingManager>().GetSettingValue("PaymentMethod.WebPay.NotifyUrl"), order.OrderId));
            remotePostHelper.Add("wsb_test", UseSandBox ? "1" : "0");

            for (int i = 0; i < order.OrderProductVariants.Count; i++)
            {
                var pv = order.OrderProductVariants[i];
                remotePostHelper.Add(string.Format("wsb_invoice_item_name[{0}]", i), pv.ProductVariant.Product.Name);
                remotePostHelper.Add(string.Format("wsb_invoice_item_quantity[{0}]", i), pv.Quantity.ToString());
                remotePostHelper.Add(string.Format("wsb_invoice_item_price[{0}]", i), PaymentService.GetTmpPriceForItem(pv, percentsToAddToWeighItemsPrice, true).ToString());
            }

            remotePostHelper.Add("wsb_tax", "0");
            remotePostHelper.Add("wsb_shipping_name", "Доставка курьером");
            remotePostHelper.Add("wsb_shipping_price", shippingPrice.ToString());
            remotePostHelper.Add("wsb_total", ((int)orderTmpTotalWithSpread).ToString());
            if (!string.IsNullOrEmpty(order.ShippingEmail))
                remotePostHelper.Add("wsb_email", order.ShippingEmail);
            remotePostHelper.Add("wsb_phone", order.ShippingPhoneNumber);

            remotePostHelper.Post();
            return string.Empty;
        }

        private static bool UseSandBox
        {
            get
            {
                try
                {
                    return IoC.Resolve<ISettingManager>().GetSettingValueBoolean("PaymentMethod.WebPay.UseSandbox");
                }
                catch (Exception) { }
                return true;
            }
        }

        private static string GetWebPayUrl()
        {
            return UseSandBox ? "https://secure.sandbox.webpay.by:8843" : "https://secure.webpay.by";
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
