using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Nop.Payment.WebPay;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Payment;

namespace NopCommerceStore
{
    public class WebPay : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var request = context.Request;

            if (request.RequestType == "POST")
                ProcessNotification(context, request);
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
                }
            }

            context.Response.Write("HTTP/1.0 200 OK");
        }

        private static void ProcessNotification(HttpContext context, HttpRequest request)
        {
            int orderId = int.Parse(context.Request["site_order_id"]);
            Order order = OrderManager.GetOrderByID(orderId);

            var orderTmpTotalWithSpread = order.OrderTotal;

            var signatureBulder = new StringBuilder(request["batch_timestamp"]);
            signatureBulder.Append(WebPayPaymentProcessor.CurrencyId);
            signatureBulder.Append(((int)orderTmpTotalWithSpread).ToString());
            signatureBulder.Append(request["payment_method"]);
            signatureBulder.Append(request["order_id"]);
            signatureBulder.Append(order.OrderID);
            signatureBulder.Append(request["transaction_id"]);
            signatureBulder.Append(request["payment_type"]);
            signatureBulder.Append(request["rrn"]);
            signatureBulder.Append(WebPayPaymentProcessor.Sk);

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
                        order.PaymentStatusID = (int)PaymentStatusEnum.Pending;
                        break;
                    case "3":
                        order.AuthorizationTransactionResult = "Pending";
                        order.PaymentStatusID = (int)PaymentStatusEnum.Pending;
                        break;
                    case "4":
                        order.AuthorizationTransactionResult = "Authorized";
                        order.PaymentStatus = PaymentStatusEnum.Paid;
                        orderPayed = true;

                        var cart = ShoppingCartManager.GetCustomerShoppingCart(order.CustomerID, ShoppingCartTypeEnum.ShoppingCart);
                        if (cart.Count > 0)
                            foreach (var sc in cart)
                                ShoppingCartManager.DeleteShoppingCartItem(sc.ShoppingCartItemID, false);
                        break;
                    case "5":
                        order.AuthorizationTransactionResult = "Refunded";
                        order.PaymentStatusID = (int)PaymentStatusEnum.Refunded;
                        break;
                    case "6":
                        order.AuthorizationTransactionResult = "System";
                        break;
                    case "7":
                        order.AuthorizationTransactionResult = "Voided";
                        order.PaymentStatusID = (int)PaymentStatusEnum.Voided;
                        break;
                }
                order.AuthorizationTransactionID = context.Request["transaction_id"];
            }
            else
            {
                order.AuthorizationTransactionCode = "-1";
                order.AuthorizationTransactionResult = "Wrong_Signature";
            }

            if (orderPayed)
            {
                order.PaymentStatusID = (int)PaymentStatusEnum.Paid;
            }
            OrderManager.UpdateOrder(order);
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