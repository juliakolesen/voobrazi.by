﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.Common.Utils;

namespace Nop.Payment.WebPay
{
    using System.Web;
    using NopSolutions.NopCommerce.BusinessLogic.Products;
    using NopSolutions.NopCommerce.BusinessLogic.Utils;

    public class WebPayPaymentProcessor : IPaymentMethod
    {
        public static string Sk
        {
            get
            {
                return UseSandBox ? "92E6467729814FC4BF365C3C820042AD" : "25A591BD53304045B0FB1E5293B4733E";
            }
        }

        public static string StoreId
        {
            get
            {
                return UseSandBox ? "994668300" : "813689890";
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

        private decimal shippingRate;
        private decimal freeShippingBorder;
        private decimal shippingPrice;

        public string PostProcessPayment(Order order)
        {
            return PostProcessPayment(order, new IndividualOrderCollection());
        }

        public string PostProcessPayment(Order order, IndividualOrderCollection indOrders)
        {
            bool convertToUsd = HttpContext.Current.Request.Cookies["Currency"] != null && HttpContext.Current.Request.Cookies["Currency"].Value == "USD";
            shippingRate = SettingManager.GetSettingValueDecimalNative("ShippingRateComputationMethod.FixedRate.Rate");
            freeShippingBorder = SettingManager.GetSettingValueDecimalNative("Shipping.FreeShippingOverX.Value");
            if (convertToUsd)
            {
                freeShippingBorder = Math.Round(PriceConverter.ToUsd(freeShippingBorder));
            }
            var prodVars = new List<ProductVariant>();
            foreach (var opv in order.OrderProductVariants)
            {
                prodVars.Add(ProductManager.GetProductVariantByID(opv.ProductVariantID));
            }

            decimal totalWithFee = (int)CalculateTotalServiceFee(order.OrderProductVariants, prodVars, order, convertToUsd, out shippingPrice);
            totalWithFee += AddServiceFee(IndividualOrderManager.GetTotalPriceIndOrders(indOrders), convertToUsd);

            var remotePostHelper = new RemotePost { FormName = "WebPeyForm", Url = GetWebPayUrl() };

            OrderManager.UpdateOrder(order.OrderID, order.OrderGUID, order.CustomerID, order.CustomerLanguageID, order.CustomerTaxDisplayType, order.OrderSubtotalInclTax, order.OrderSubtotalExclTax, order.OrderShippingInclTax,
                order.OrderShippingExclTax, order.PaymentMethodAdditionalFeeInclTax, order.PaymentMethodAdditionalFeeExclTax, order.OrderTax, order.OrderTotal, order.OrderDiscount, order.OrderSubtotalInclTaxInCustomerCurrency,
                order.OrderShippingExclTaxInCustomerCurrency, order.OrderShippingInclTaxInCustomerCurrency, order.OrderShippingExclTaxInCustomerCurrency, order.PaymentMethodAdditionalFeeInclTaxInCustomerCurrency,
                order.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency, order.OrderTaxInCustomerCurrency, order.OrderTotalInCustomerCurrency, order.CustomerCurrencyCode, order.OrderWeight, order.AffiliateID,
                order.OrderStatus, order.AllowStoringCreditCardNumber, order.CardType, order.CardName, order.CardNumber, order.MaskedCreditCardNumber, order.CardCVV2, order.CardExpirationMonth, order.CardExpirationYear,
                order.PaymentMethodID, order.PaymentMethodName, order.AuthorizationTransactionID, order.AuthorizationTransactionCode, order.AuthorizationTransactionResult, order.CaptureTransactionID, order.CaptureTransactionResult,
                order.PurchaseOrderNumber, order.PaymentStatus, order.BillingFirstName, order.BillingLastName, order.BillingPhoneNumber, order.BillingEmail, order.BillingFaxNumber, order.BillingCompany, order.BillingAddress1,
                order.BillingAddress2, order.BillingCity, order.BillingStateProvince, order.BillingStateProvinceID, order.BillingZipPostalCode, order.BillingCountry, order.BillingCountryID, order.ShippingStatus,
                order.ShippingFirstName, order.ShippingLastName, order.ShippingPhoneNumber, order.ShippingEmail, order.ShippingFaxNumber, order.ShippingCompany, order.ShippingAddress1, order.ShippingAddress2,
                order.ShippingCity, order.ShippingStateProvince, order.ShippingStateProvinceID, order.ShippingZipPostalCode, order.ShippingCountry, order.ShippingCountryID, order.ShippingMethod,
                order.ShippingRateComputationMethodID, order.ShippedDate, order.Deleted, order.CreatedOn);

            remotePostHelper.Add("*scart", "");
            remotePostHelper.Add("wsb_storeid", StoreId);
            remotePostHelper.Add("wsb_store", "voobrazi.by");
            remotePostHelper.Add("wsb_order_num", order.OrderID.ToString());
            remotePostHelper.Add("wsb_currency_id", convertToUsd ? "USD" : CurrencyId);
            remotePostHelper.Add("wsb_version", "2");
            remotePostHelper.Add("wsb_language_id", "russian");
            string seed = ((int)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds).ToString();
            remotePostHelper.Add("wsb_seed", seed);

            var signatureBulder = new StringBuilder(seed);
            signatureBulder.Append(StoreId);
            signatureBulder.Append(order.OrderID);
            signatureBulder.Append(UseSandBox ? "1" : "0");
            signatureBulder.Append(convertToUsd ? "USD" : CurrencyId);

            signatureBulder.Append(totalWithFee.ToString());
            signatureBulder.Append(Sk);

            byte[] buffer = Encoding.Default.GetBytes(signatureBulder.ToString());
            var sha1 = new SHA1CryptoServiceProvider();
            string signature = BitConverter.ToString(sha1.ComputeHash(buffer)).Replace("-", "").ToLower();
            remotePostHelper.Add("wsb_signature", signature);
            remotePostHelper.Add("wsb_return_url", string.Format(SettingManager.GetSettingValue("PaymentMethod.WebPay.ReturnUrl"), order.OrderID));
            remotePostHelper.Add("wsb_cancel_return_url", string.Format(SettingManager.GetSettingValue("PaymentMethod.WebPay.CancelUrl"), order.OrderID));
            remotePostHelper.Add("wsb_notify_url", string.Format(SettingManager.GetSettingValue("PaymentMethod.WebPay.NotifyUrl"), order.OrderID));
            remotePostHelper.Add("wsb_test", UseSandBox ? "1" : "0");

            for (int i = 0; i < order.OrderProductVariants.Count; i++)
            {
                var opv = order.OrderProductVariants[i];
                remotePostHelper.Add(string.Format("wsb_invoice_item_name[{0}]", i), opv.ProductVariant.Product.Name);
                remotePostHelper.Add(string.Format("wsb_invoice_item_quantity[{0}]", i), opv.Quantity.ToString());

                var prodVar = ProductManager.GetProductVariantByID(opv.ProductVariantID);
                decimal prodPrice = !convertToUsd ? prodVar.Price : Math.Round(PriceConverter.ToUsd(prodVar.Price));

                remotePostHelper.Add(string.Format("wsb_invoice_item_price[{0}]", i), AddServiceFee(prodPrice, convertToUsd).ToString());
            }

            int cartCount = order.OrderProductVariants.Count;
            for (int i = order.OrderProductVariants.Count; i < indOrders.Count + cartCount; i++)
            {
                var opv = indOrders[i - cartCount];
                remotePostHelper.Add(string.Format("wsb_invoice_item_name[{0}]", i), "Индивидуальный заказ");
                remotePostHelper.Add(string.Format("wsb_invoice_item_quantity[{0}]", i), "1");

                decimal prodPrice = !convertToUsd ? opv.Price : Math.Round(PriceConverter.ToUsd(opv.Price));

                remotePostHelper.Add(string.Format("wsb_invoice_item_price[{0}]", i), AddServiceFee(prodPrice, convertToUsd).ToString());
            }

            remotePostHelper.Add("wsb_tax", "0");
            remotePostHelper.Add("wsb_shipping_name", "Доставка курьером");

            remotePostHelper.Add("wsb_shipping_price", AddServiceFee(shippingPrice, convertToUsd).ToString());
            remotePostHelper.Add("wsb_total", totalWithFee.ToString());

            if (!string.IsNullOrEmpty(order.ShippingEmail))
                remotePostHelper.Add("wsb_email", order.ShippingEmail);
            remotePostHelper.Add("wsb_phone", order.ShippingPhoneNumber);

            remotePostHelper.Post();
            return string.Empty;
        }

        private decimal CalculateTotalServiceFee(IEnumerable<OrderProductVariant> orderProducts, IEnumerable<ProductVariant> prodVars, Order order, bool convvertToUsd, out decimal shippingPrice)
        {
            decimal retVal = convvertToUsd ?
                prodVars.Sum(prodVar => (Math.Round(PriceConverter.ToUsd(AddServiceFee(prodVar.Price, convvertToUsd))) * orderProducts.First(op => op.ProductVariantID == prodVar.ProductVariantID).Quantity))
                : prodVars.Sum(prodVar => (AddServiceFee(prodVar.Price, convvertToUsd) * orderProducts.First(op => op.ProductVariantID == prodVar.ProductVariantID).Quantity));

            shippingPrice = retVal > freeShippingBorder ? 0 : shippingRate;
            if (convvertToUsd)
            {
                shippingPrice = Math.Round(PriceConverter.ToUsd(shippingPrice));
            }
            retVal += AddServiceFee(shippingPrice, convvertToUsd);
            return retVal;
        }

        private static decimal AddServiceFee(decimal price, bool convvertToUsd)
        {
            decimal priceWithFee = price + ((price / 100) * SettingManager.GetSettingValueInteger("PaymentMethod.WebPay.ServiceFee"));
            if (!convvertToUsd)
                return Math.Ceiling((priceWithFee / 100) * 100);

            return Math.Round((priceWithFee / 100) * 100);
        }

        private static bool UseSandBox
        {
            get
            {
                try
                {
                    return SettingManager.GetSettingValueBoolean("PaymentMethod.WebPay.UseSandbox");
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

        public void ProcessRecurringPayment(PaymentInfo paymentInfo, Customer customer, Guid orderGuid, ref ProcessPaymentResult processPaymentResult)
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
    }
}
