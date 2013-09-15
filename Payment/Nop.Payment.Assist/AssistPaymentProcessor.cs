
using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Payment.PostPaymentCommon;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Utils;
using NopSolutions.NopCommerce.Common.Utils;

namespace Nop.Payment.Assist
{
    public class AssistPaymentProcessor: PostPaymentProcessor
    {
        #region IPaymentMethod Members

        protected override void BuildPostForm(Order order, IndividualOrderCollection indOrders, decimal price, decimal shipping, Customer customer)
        {
            var remotePostHelper = new RemotePost { FormName = "AssistForm", Url = GetPaymentServiceUrl() };
            remotePostHelper.Add("Merchant_ID", "706403");
            remotePostHelper.Add("OrderNumber", order.OrderID.ToString());
            remotePostHelper.Add("OrderAmount", Math.Round(price + shipping).ToString());
            remotePostHelper.Add("OrderCurrency", ConvertToUsd ? "USD" : CurrencyId);
            remotePostHelper.Add("TestMode", IsTestMode? "1": "0");
            if (!String.IsNullOrEmpty(customer.BillingAddress.LastName))
            {
                remotePostHelper.Add("Lastname", customer.BillingAddress.LastName);
            }
            if (!String.IsNullOrEmpty(customer.BillingAddress.FirstName))
            {
                remotePostHelper.Add("Firstname", customer.BillingAddress.FirstName);
            }
            if (!String.IsNullOrEmpty(customer.BillingAddress.Email))
            {
                remotePostHelper.Add("Email", customer.BillingAddress.Email);
            }

            remotePostHelper.Add("URL_RETURN_OK", SettingManager.GetSettingValue("PaymentMethod.Assist.OkUrl"));
            remotePostHelper.Add("URL_RETURN_NO", SettingManager.GetSettingValue("PaymentMethod.Assist.NoUrl"));

            remotePostHelper.Post();
        }

        protected override decimal AddServiceFee(decimal totalPrice, bool convertToUsd)
        {
            decimal priceWithFee = totalPrice + ((totalPrice / 100) * SettingManager.GetSettingValueInteger("PaymentMethod.Assist.ServiceFee"));
            if (!convertToUsd)
                return Math.Ceiling((priceWithFee / 100) * 100);

            return Math.Round((priceWithFee / 100) * 100);
        }

        protected override decimal CalculateTotalOrderServiceFee(OrderProductVariantCollection orderProducts, List<ProductVariant> prodVars, Order order, out object shippingPrice)
        {
            decimal retVal = ConvertToUsd
                                 ? prodVars.Sum(prodVar => (Math.Round(PriceConverter.ToUsd(AddServiceFee(prodVar.Price, ConvertToUsd)))*
                                      orderProducts.First(op => op.ProductVariantID == prodVar.ProductVariantID).Quantity))
                                 : prodVars.Sum(prodVar =>(AddServiceFee(prodVar.Price, ConvertToUsd)*
                                      orderProducts.First(op => op.ProductVariantID == prodVar.ProductVariantID).Quantity));
            if (ShoppingCartRequiresShipping)
            {
                decimal shippingBlr = AddServiceFee(((FreeShippingEnabled && retVal > FreeShippingBorder) ? 0 : ShippingRate), 
                                                ConvertToUsd);
                shippingPrice = ConvertToUsd ? Math.Round(PriceConverter.ToUsd(shippingBlr)) : shippingBlr;
            }
            else
            {
                shippingPrice = (decimal) 0;
            }

            return retVal;
        }

        #endregion

        public override string GetPaymentServiceUrl()
        {
            const string testUrl = "https://test.paysec.by/pay/order.cfm";

            try
            {
                string paymentUrl = SettingManager.GetSettingValue("PaymentMethod.Assist.PaymentUrl");
                if (!String.IsNullOrEmpty(paymentUrl))
                    return paymentUrl;
            }
            catch (Exception){}

            return testUrl;
        }

        private static bool IsTestMode
        {
            get
            {
                try
                {
                    return SettingManager.GetSettingValueBoolean("PaymentMethod.Assist.TestMode");
                }
                catch (Exception) { }

                return true;
            }
        }
    }
}
