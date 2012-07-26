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
using System.Web.UI;
using System.Web.UI.WebControls;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.Common.Utils;


namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class CheckoutPaymentMethodControl : BaseNopUserControl
    {
        ShoppingCart Cart = null;

        protected string FormatPaymentMethodInfo(PaymentMethod paymentMethod)
        {
            decimal paymentMethodAdditionalFee = PaymentManager.GetAdditionalHandlingFee(paymentMethod.PaymentMethodID);
            decimal rateBase = TaxManager.GetPaymentMethodAdditionalFee(paymentMethodAdditionalFee, NopContext.Current.User);
            decimal rate = CurrencyManager.ConvertCurrency(rateBase, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingCurrency);
            if (rate > decimal.Zero)
            {
                string rateStr = PriceHelper.FormatPaymentMethodAdditionalFee(rate, true);
                return string.Format("({0})", rateStr);
            }
            else
            {
                return string.Empty;
            }
        }

        protected void btnNextStep_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                int paymentMethodID = this.SelectedPaymentMethodID;
                PaymentMethod paymentMethod = PaymentMethodManager.GetPaymentMethodByID(paymentMethodID);
                PaymentInfo pi = new PaymentInfo();
                pi.PaymentMethodID = paymentMethodID;
                PaymentInfo = pi;
                if (paymentMethod != null && paymentMethod.IsActive)
                {
                    NopContext.Current.User = CustomerManager.SetLastPaymentMethodID(NopContext.Current.User.CustomerID, paymentMethodID);
                    Response.Redirect(string.Format("~/CheckoutConfirm.aspx{0}", Request["OrderId"] != null ? "?OrderId=" + Request["OrderId"] : ""));
                }
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

            if (!Page.IsPostBack)
            {
                PaymentMethodCollection paymentMethods = PaymentMethodManager.GetAllPaymentMethods();
                PaymentMethod paypalExpressPaymentMethod = null;
                foreach (PaymentMethod pm in paymentMethods)
                    if (pm.SystemKeyword == "PayPalExpress")
                        paypalExpressPaymentMethod = pm;
                PaymentMethod googleCheckoutPaymentMethod = null;
                foreach (PaymentMethod pm in paymentMethods)
                    if (pm.SystemKeyword == "GoogleCheckout")
                        googleCheckoutPaymentMethod = pm;

                bool hasButtonMethods = false;
                if (paypalExpressPaymentMethod != null && paypalExpressPaymentMethod.IsActive)
                    hasButtonMethods = true;

                PaymentMethodCollection boundPaymentMethods = new PaymentMethodCollection();
                foreach (PaymentMethod pm in paymentMethods)
                    if (pm != paypalExpressPaymentMethod && pm != googleCheckoutPaymentMethod)
                        boundPaymentMethods.Add(pm);

                if (boundPaymentMethods.Count == 0)
                {
                    if (hasButtonMethods)
                    {
                        phSelectPaymentMethod.Visible = false;
                        phNoPaymentMethods.Visible = false;
                    }
                    else
                    {
                        phSelectPaymentMethod.Visible = false;
                        phNoPaymentMethods.Visible = true;
                    }
                }
                else if (boundPaymentMethods.Count == 1)
                {
                    if (hasButtonMethods)
                    {
                        phSelectPaymentMethod.Visible = true;
                        phNoPaymentMethods.Visible = false;
                        dlPaymentMethod.DataSource = boundPaymentMethods;
                        dlPaymentMethod.DataBind();
                    }
                    else
                    {
                        phSelectPaymentMethod.Visible = false;
                        phNoPaymentMethods.Visible = false;
                        NopContext.Current.User = CustomerManager.SetLastPaymentMethodID(NopContext.Current.User.CustomerID, paymentMethods[0].PaymentMethodID);
                        Response.Redirect("~/CheckoutConfirm.aspx");
                    }
                }
                else
                {
                    phSelectPaymentMethod.Visible = true;
                    phNoPaymentMethods.Visible = false;
                    dlPaymentMethod.DataSource = boundPaymentMethods;
                    dlPaymentMethod.DataBind();
                }
            }

            divAdditionalPaymentInfo.Visible = Request["OrderId"] != null;
        }

        public int SelectedPaymentMethodID
        {
            get
            {
                int selectedPaymentMethodID = 0;
                foreach (DataListItem item in this.dlPaymentMethod.Items)
                {
                    RadioButton rdPaymentMethod = (RadioButton)item.FindControl("rdPaymentMethod");
                    if (rdPaymentMethod.Checked)
                    {
                        selectedPaymentMethodID = Convert.ToInt32(this.dlPaymentMethod.DataKeys[item.ItemIndex].ToString());
                        break;
                    }
                }
                return selectedPaymentMethodID;
            }
        }

        protected PaymentInfo PaymentInfo
        {
            set
            {
                this.Session["OrderPaymentInfo"] = value;
            }
        }
    }
}