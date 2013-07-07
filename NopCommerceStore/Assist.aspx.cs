using System.Web;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Payment;

namespace NopCommerceStore
{
    public partial class Assist : System.Web.UI.Page
    {
        public override void ProcessRequest(HttpContext context)
        {
            context.Response.Clear();

            var request = context.Request;

            if (request["status"] != null)
            {
                switch (request["status"])
                {
                    case "succeeded":
                        ProcessNotification(context);
                        context.Response.Redirect("~/checkoutcompleted.aspx");
                        break;
                    case "failed":
                        context.Response.Redirect(string.Format("~/CheckoutPaymentMethod.aspx?OrderId={0}",
                                                                request["ordernumber"]));
                        break;
                }
            }

            context.Response.Write("HTTP/1.0 200 OK");
        }

        private static void ProcessNotification(HttpContext context)
        {
            int orderId = int.Parse(context.Request["ordernumber"]);
            Order order = OrderManager.GetOrderByID(orderId);

            order.PaymentStatusID = (int)PaymentStatusEnum.Paid;
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
        }
    }
}