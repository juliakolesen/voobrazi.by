using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;

using Nop.Payment.WebPay;

using NopSolutions.NopCommerce.BusinessLogic.Infrastructure;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Payment;

namespace NopSolutions.NopCommerce.Web
{
    public class WebPay : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var request = context.Request;

            var orderService = IoC.Resolve<OrderService>();

            if (request.RequestType == "POST")
                ProcessNotification(context, request, orderService);
            else
            {
                switch (request["status"])
                {
                    case "succeeded":
                        context.Response.Redirect("~/checkoutcompleted.aspx");
                        break;
                    case "failed":
                        context.Response.Redirect(string.Format("~/checkoutconfirm.aspx?OrderId={0}", request["orderId"]));
                        break;
                    default:
                        break;
                }
            }

            context.Response.Write("HTTP/1.0 200 OK");
        }

        private static void ProcessNotification(HttpContext context, HttpRequest request, OrderService orderService)
        {
            int orderId = int.Parse(context.Request["site_order_id"]);
            Order order = orderService.GetOrderById(orderId);

            var orderTmpTotalWithSpread = order.OrderTotalWithSpread.HasValue ? order.OrderTotalWithSpread.Value : 0;

            var signatureBulder = new StringBuilder(request["batch_timestamp"]);
            signatureBulder.Append(WebPayPaymentProcessor.CurrencyId);
            signatureBulder.Append(((int)orderTmpTotalWithSpread).ToString());
            signatureBulder.Append(request["payment_method"]);
            signatureBulder.Append(request["order_id"]);
            signatureBulder.Append(order.OrderId);
            signatureBulder.Append(request["transaction_id"]);
            signatureBulder.Append(request["payment_type"]);
            signatureBulder.Append(request["rrn"]);
            signatureBulder.Append(WebPayPaymentProcessor.SK);

            byte[] buffer = Encoding.Default.GetBytes(signatureBulder.ToString());
            var md5 = new MD5CryptoServiceProvider();
            string signature = BitConverter.ToString(md5.ComputeHash(buffer)).Replace("-", "").ToLower();

            bool orderPayed = false;

            if (request["wsb_signature"] == signature)
            {
                order.AuthorizationTransactionCode = context.Request["payment_type"];
                switch (context.Request["payment_type"])
                {
                    case "1":
                        order.AuthorizationTransactionResult = "Completed";
                        order.PaymentStatus = PaymentStatusEnum.Paid;
                        orderPayed = true;
                        break;
                    case "2":
                        order.AuthorizationTransactionResult = "Declined";
                        order.PaymentStatusId = (int)PaymentStatusEnum.Pending;
                        break;
                    case "3":
                        order.AuthorizationTransactionResult = "Pending";
                        order.PaymentStatusId = (int)PaymentStatusEnum.Pending;
                        break;
                    case "4":
                        order.AuthorizationTransactionResult = "Authorized";
                        order.PaymentStatus = PaymentStatusEnum.Paid;
                        orderPayed = true;

                        var shoppingCartService = IoC.Resolve<ShoppingCartService>();
                        var cart = shoppingCartService.GetCustomerShoppingCart(order.CustomerId, ShoppingCartTypeEnum.ShoppingCart);
                        if (cart.Count > 0)
                            foreach (var sc in cart)
                                shoppingCartService.DeleteShoppingCartItem(sc.ShoppingCartItemId, false);
                        break;
                    case "5":
                        order.AuthorizationTransactionResult = "Refunded";
                        order.PaymentStatusId = (int)PaymentStatusEnum.Refunded;
                        break;
                    case "6":
                        order.AuthorizationTransactionResult = "System";
                        break;
                    case "7":
                        order.AuthorizationTransactionResult = "Voided";
                        order.PaymentStatusId = (int)PaymentStatusEnum.Voided;
                        break;
                    default:
                        break;
                }
                order.AuthorizationTransactionId = context.Request["transaction_id"];
            }
            else
            {
                order.AuthorizationTransactionCode = "-1";
                order.AuthorizationTransactionResult = "Wrong_Signature";
            }

            if (orderPayed)
            {
                order.PaymentStatusId = (int)PaymentStatusEnum.Paid;
                order.PaidDate = DateTime.Now;
            }
            orderService.UpdateOrder(order);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}