using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;

using Nop.Payment.WebMoney;

using NopSolutions.NopCommerce.BusinessLogic.Infrastructure;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Payment;

namespace NopSolutions.NopCommerce.Web
{
    public class WebMoney : IHttpHandler
    {
        private const string SK = "9842A04F-AC8F-4D4F-BAC9-4675917DFA20";

        public void ProcessRequest(HttpContext context)
        {
            var request = context.Request;

            if (!string.IsNullOrEmpty(context.Request["LMI_PREREQUEST"]) && context.Request["LMI_PREREQUEST"] == "1") // for pre-processing-request
            {
                context.Response.Clear();
                context.Response.Write("YES");
                context.Response.End();
                return;
            }

            //for post-processing-request
            //string redirectUrl = string.Empty;

            var orderService = IoC.Resolve<OrderService>();
            Order order = orderService.GetOrderById(int.Parse(request["OrderId"]));

            var orderTmpTotalWithSpread = order.OrderTotalWithSpread.HasValue ? order.OrderTotalWithSpread.Value : 0;

#if(!RELEASE)
            orderTmpTotalWithSpread = 1;
#endif
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var signatureBulder = new StringBuilder(WebMoneyPaymentProcessor.Purse);
            signatureBulder.Append(string.Format("{0:#.00}", orderTmpTotalWithSpread));
            signatureBulder.Append(order.OrderId);
            signatureBulder.Append(WebMoneyPaymentProcessor.UseSandBox ? "1" : "0");
            signatureBulder.Append(request["LMI_SYS_INVS_NO"]);
            signatureBulder.Append(request["LMI_SYS_TRANS_NO"]);
            signatureBulder.Append(request["LMI_SYS_TRANS_DATE"]);
            signatureBulder.Append(SK);
            signatureBulder.Append(request["LMI_PAYER_PURSE"]);
            signatureBulder.Append(request["LMI_PAYER_WM"]);
            byte[] buffer = Encoding.GetEncoding(1251).GetBytes(signatureBulder.ToString());
            var md5 = new MD5CryptoServiceProvider();
            string signature = BitConverter.ToString(md5.ComputeHash(buffer)).Replace("-", "").ToUpper();

            if (request["LMI_HASH"] != signature)
            {
                //redirectUrl = string.Format("~/checkoutconfirm.aspx?OrderId={0}", request["OrderId"]);
                order.AuthorizationTransactionCode = string.Format("-1");
                order.AuthorizationTransactionId = string.Format("LMI_SYS_TRANS_NO={0}", request["LMI_SYS_TRANS_NO"]);
                order.AuthorizationTransactionResult = string.Format("Wrong Signature;Input string:{0};{1}::{2}", signatureBulder.ToString(), request["LMI_HASH"], signature);
                order.PaymentStatus = PaymentStatusEnum.Pending;
            }
            else
            {
                order.AuthorizationTransactionCode = "Completed";
                order.AuthorizationTransactionId = string.Format("LMI_SYS_TRANS_NO={0}", request["LMI_SYS_TRANS_NO"]);
                order.AuthorizationTransactionResult = string.Format("Completed;LMI_PAYER_PURSE={0};LMI_SYS_INVS_NO={1};LMI_PAYER_WM ={2}", request["LMI_PAYER_PURSE "], request["LMI_SYS_INVS_NO"], request["LMI_PAYER_WM"]);
                order.PaymentStatus = PaymentStatusEnum.Paid;
                order.PaidDate = DateTime.Now;
            }
            orderService.UpdateOrder(order);

            //if (!string.IsNullOrEmpty(redirectUrl))
            //    context.Response.Redirect(redirectUrl);

            context.Response.Write("HTTP/1.0 200 OK");
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