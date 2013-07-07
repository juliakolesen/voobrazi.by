
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Shipping;
using NopSolutions.NopCommerce.BusinessLogic.Utils;


namespace Nop.Payment.PostPaymentCommon
{
    public abstract class PostPaymentProcessor : IPaymentMethod
    {
        #region properties

        protected bool ConvertToUsd
        {
            get { return HttpContext.Current.Request.Cookies["Currency"] != null && HttpContext.Current.Request.Cookies["Currency"].Value == "USD"; }
        }

        protected decimal ShippingRate
        {
            get { return SettingManager.GetSettingValueDecimalNative("ShippingRateComputationMethod.FixedRate.Rate"); }
        }

        protected decimal FreeShippingBorder
        {
            get
            {
                decimal freeShBorder = SettingManager.GetSettingValueDecimalNative("Shipping.FreeShippingOverX.Value");
                if (ConvertToUsd)
                {
                    freeShBorder = Math.Round(PriceConverter.ToUsd(freeShBorder));
                }

                return freeShBorder;
            }
        }

        protected decimal ShippingPrice { get; set; }

        protected bool ShoppingCartRequiresShipping
        {
            get
            {
                ShoppingCart cart = ShoppingCartManager.GetCurrentShoppingCart(ShoppingCartTypeEnum.ShoppingCart);
                return ShippingManager.ShoppingCartRequiresShipping(cart) && HttpContext.Current.Session["SelfOrder"] == null;
            }
        }

        public static string CurrencyId
        {
            get
            {
                return "BYR";
            }
        }

        #endregion properties

        public void ProcessPayment(PaymentInfo paymentInfo, Customer customer, Guid orderGuid, ref ProcessPaymentResult processPaymentResult)
        {
            processPaymentResult.PaymentStatus = PaymentStatusEnum.Pending;
        }

        public string PostProcessPayment(Order order)
        {
            return PostProcessPayment(order, new IndividualOrderCollection(), new Customer());
        }

        public string PostProcessPayment(Order order, IndividualOrderCollection indOrders, Customer customer)
        {
            object shippingPrice;
            decimal totalWithFee = CalculatePricaWithFee(order, indOrders, out shippingPrice);
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

            BuildPostForm(order, indOrders, totalWithFee, (decimal)shippingPrice, customer);
            return string.Empty;
        }

        protected abstract void BuildPostForm(Order order, IndividualOrderCollection indOrders, decimal price, decimal shipping, Customer customer);

        private decimal CalculatePricaWithFee(Order order, IndividualOrderCollection indOrders, out object shippingPrice)
        {
            var prodVars = order.OrderProductVariants.Select(opv => ProductManager.GetProductVariantByID(opv.ProductVariantID)).ToList();
            decimal totalWithFee = CalculateTotalOrderServiceFee(order.OrderProductVariants, prodVars, order, out shippingPrice);
            ShippingPrice = (decimal) shippingPrice;
            totalWithFee += AddServiceFee(IndividualOrderManager.GetTotalPriceIndOrders(indOrders), ConvertToUsd);

            return totalWithFee;
        }

        protected abstract decimal AddServiceFee(decimal totalPrice, bool convertToUsd);


        protected abstract decimal CalculateTotalOrderServiceFee(OrderProductVariantCollection orderProductVariants,
                                                        List<ProductVariant> prodVars, Order order,
                                                        out object shippingPrice);
        
        public abstract string GetPaymentServiceUrl();

        public decimal GetAdditionalHandlingFee()
        {
            return decimal.Zero;
        }

        public bool CanCapture
        {
            get
            {
                return false;
            }
        }

        public void Capture(Order order, ref ProcessPaymentResult processPaymentResult)
        {
            throw new NotImplementedException();
        }
    }
}
