//------------------------------------------------------------------------------
// The contents of this file are subject to the nopCommerce Public License Version 1.0 ("License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at  http://www.nopCommerce.com/License.aspx. 
// 
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. 
// See the License for the specific language governing rights and limitations under the License.
// 
// The Original Code is nopCommerce.
// The Initial Developer of the Original Code is NopSolutions.
// All Rights Reserved.
// 
// Contributor(s): _______. 
//------------------------------------------------------------------------------


using System;
using System.Text;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Audit;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Messages;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Shipping;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Utils;


namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class CheckoutConfirmControl : BaseNopUserControl
    {
        ShoppingCart Cart;

        protected void btnNextStep_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    PaymentInfo paymentInfo = this.PaymentInfo;
                    //if (paymentInfo == null)
                    //    Response.Redirect("~/CheckoutPaymentInfo.aspx");
                    paymentInfo.BillingAddress = NopContext.Current.User.BillingAddress;
                    paymentInfo.ShippingAddress = NopContext.Current.User.ShippingAddress;
                    paymentInfo.CustomerLanguage = NopContext.Current.WorkingLanguage;
                    paymentInfo.CustomerCurrency = NopContext.Current.WorkingCurrency;

                    int orderId = 0;
                    string result = "";
                    if (Request["OrderId"] == null)
                        result = OrderManager.PlaceOrder(paymentInfo, NopContext.Current.User, out orderId);
                    else
                        orderId = int.Parse(Request["OrderId"]);

                    this.PaymentInfo = null;
                    Order order = OrderManager.GetOrderByID(orderId);
                    if (!String.IsNullOrEmpty(result))
                    {
                        lError.Text = Server.HtmlEncode(result);
                        return;
                    }

                    if (Request["OrderId"] != null)
                    {
                        order.PaymentMethodID = paymentInfo.PaymentMethodID;
                        order.PaymentMethodName = PaymentMethodManager.GetPaymentMethodByID(paymentInfo.PaymentMethodID).Name;
                    }

                    Guid CustomerSessionGUID = NopContext.Current.Session.CustomerSessionGUID;
                    IndividualOrderCollection indOrders = IndividualOrderManager.GetIndividualOrderByCurrentUserSessionGuid(CustomerSessionGUID);
                    StartPaymentMethod(indOrders, order);

                    string subj = "Заказ в магазине Voobrazi.by";
                    StringBuilder body = new StringBuilder();
                    body.AppendFormat("Доставка: {0}<br /><br />", ((bool)Session["Delivery"]) ? "Курьером" : "Самовывоз").AppendLine();
                    body.AppendFormat("<b>Заказчик</b><br />").AppendLine();
                    body.AppendFormat("ФИО: {0} {1}<br />", paymentInfo.BillingAddress.FirstName, paymentInfo.BillingAddress.LastName).AppendLine();
                    body.AppendFormat("Телефоны: {0} {1}<br />", paymentInfo.BillingAddress.PhoneNumber, !string.IsNullOrEmpty(paymentInfo.BillingAddress.FaxNumber) ? ", " + paymentInfo.BillingAddress.FaxNumber : "").AppendLine();
                    body.AppendFormat("Адрес: {0} {1}<br />", paymentInfo.BillingAddress.City, paymentInfo.BillingAddress.Address1).AppendLine();
                    body.AppendFormat("Email: {0}<br /><br />", !string.IsNullOrEmpty(NopContext.Current.User.BillingAddress.Email) ? NopContext.Current.User.BillingAddress.Email : NopContext.Current.User.Email).AppendLine();
                    body.AppendFormat("Комментарии: {0}<br /><br />", tbComments.Text).AppendLine();

                    decimal total = 0;                
                    decimal indOrderTotal = IndividualOrderManager.GetTotalPriceIndOrders(indOrders);
                    if (Request.Cookies["Currency"] != null && Request.Cookies["Currency"].Value == "USD")
                    {
                        indOrderTotal = Math.Round(PriceConverter.ToUsd(indOrderTotal));
                    }

                    total += indOrderTotal;
                    body.AppendFormat("Индивидуальные заказы:<br />");
                    foreach (var indOrder in indOrders)
                    {
                        body.Append(indOrder.OrderText + "<br />");
                    }

                    if (Session["fn"] != null)
                    {
                        body.AppendLine();
                        body.Append("<b>Получатель не заказчик</b><br />").AppendLine();
                        body.AppendFormat("ФИО: {0} {1}<br />", Session["fn"], Session["ln"]).AppendLine();
                        body.AppendFormat("Телефон: {0}<br />", Session["pn"]).AppendLine();
                        body.AppendFormat("Адрес: {0}<br />", Session["address"]).AppendLine();
                        body.AppendFormat("Дополнительная информация: {0}<br />", Session["ai"]).AppendLine();
                    }
                    body.AppendFormat("Уведомление о доставке: {0} | {1}<br />", chbByMail.Checked ? "Письмом на Email" : "", chbSMS.Checked ? "СМС сообщение" : "").AppendLine();

                    body.AppendFormat("<br /><br /> Заказано:<br />");

                    foreach (OrderProductVariant variant in order.OrderProductVariants)
                    {
                        body.AppendFormat(" - {0} ({1}) x {2}шт. -- {3}; <br />", variant.ProductVariant.Product.Name, PriceHelper.FormatShippingPrice(variant.ProductVariant.Price, true), variant.Quantity, PriceHelper.FormatShippingPrice(variant.ProductVariant.Price * variant.Quantity, true));
                        total += variant.ProductVariant.Price * variant.Quantity;
                    }

                    string shipping = GetLocaleResourceString("ShoppingCart.ShippingNotRequired");

                    bool shoppingCartRequiresShipping = ShippingManager.ShoppingCartRequiresShipping(Cart) && Session["SelfOrder"] == null;
                    if (shoppingCartRequiresShipping)
                    {
                        decimal? shoppingCartShippingBase = ShippingManager.GetShoppingCartShippingTotal(Cart, NopContext.Current.User);
                        if (shoppingCartShippingBase.HasValue)
                        {
                            decimal shoppingCartShipping = CurrencyManager.ConvertCurrency(shoppingCartShippingBase.Value, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingCurrency);
                            shipping = PriceHelper.FormatShippingPrice(shoppingCartShipping, true);
                            total += shoppingCartShipping;
                        }
                    }

                    body.AppendFormat("Доставка: {0}<br />", shipping).AppendLine();
                    body.AppendFormat("<b>Итого:</b> {0}<br />", total).AppendLine();

                    body.AppendFormat("<br />Дополнительная информация: {0}<br />", Session["ai"]).AppendLine();
                    MessageManager.SendEmail(subj, body.ToString(), MessageManager.AdminEmailAddress, MessageManager.AdminEmailAddress);
                    Session.Remove("SelfOrder");
                    Response.Redirect("~/CheckoutCompleted.aspx");
                }
                catch (Exception exc)
                {
                    Session.Remove("SelfOrder");
                    LogManager.InsertLog(LogTypeEnum.OrderError, exc.Message, exc);
                    lError.Text = Server.HtmlEncode("Во время обработки заказа произошла ошибка. Для выполнения заказа, пожалуйста, свяжитесь с администратором. Контактную информацию можно найти на странице Контакты.");
                }
            }
        }

        private static void StartPaymentMethod(IndividualOrderCollection indOrders, Order order)
        {
            PaymentMethod pm = PaymentMethodManager.GetPaymentMethodByID(order.PaymentMethodID);
            if (pm.ClassName.Equals("Nop.Payment.WebPay.WebPayPaymentProcessor, Nop.Payment.WebPay"))
            {
                var webPayMethod = new Nop.Payment.WebPay.WebPayPaymentProcessor();
                webPayMethod.PostProcessPayment(order, indOrders);
            }
            else if (pm.ClassName.Equals("Nop.Payment.Assist.AssistPaymentProcessor, Nop.Payment.Assist"))
            {
                var assistPaymentProcessor = new Nop.Payment.Assist.AssistPaymentProcessor();
                assistPaymentProcessor.PostProcessPayment(order, indOrders, NopContext.Current.User);
            }
            else
            {
                PaymentManager.PostProcessPayment(order);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((NopContext.Current.User == null) || (NopContext.Current.User.IsGuest && !CustomerManager.AnonymousCheckoutAllowed))
            {
                string loginURL = CommonHelper.GetLoginPageURL(true);
                Response.Redirect(loginURL);
            }

            Cart = ShoppingCartManager.GetCurrentShoppingCart(ShoppingCartTypeEnum.ShoppingCart);
            IndividualOrderCollection indOrders = IndividualOrderManager.GetCurrentUserIndividualOrders();
            if (Cart.Count == 0 && indOrders.Count == 0)
                Response.Redirect("~/ShoppingCart.aspx");

            btnNextStep.Attributes.Add("onclick", "this.disabled = true;" + Page.ClientScript.GetPostBackEventReference(this.btnNextStep, ""));
        }

        protected PaymentInfo PaymentInfo
        {
            get
            {
                if (this.Session["OrderPaymentInfo"] != null)
                    return (PaymentInfo)(this.Session["OrderPaymentInfo"]);
                return null;
            }
            set
            {
                this.Session["OrderPaymentInfo"] = value;
            }
        }
    }
}