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
using System.Collections;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Web.Templates.Payment;
 

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class CheckoutPaymentInfoControl : BaseNopUserControl
    {
        ShoppingCart Cart = null;

        private void CreateChildControlsTree()
        {
            PaymentMethod paymentMethod = null;
            if (NopContext.Current.User != null)
            {
                PaymentMethodCollection paymentMethods = PaymentMethodManager.GetAllPaymentMethods();
                if (paymentMethods.Count == 1)
                    paymentMethod = paymentMethods[0];
                else
                    paymentMethod = NopContext.Current.User.LastPaymentMethod;
            }
            if (paymentMethod != null)
            {
                Control child = null;
                child = base.LoadControl(paymentMethod.UserTemplatePath);
                this.PaymentInfoPlaceHolder.Controls.Add(child);
            }
            else
            {
                Response.Redirect("~/CheckoutPaymentMethod.aspx");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.CreateChildControlsTree();
        }

        public bool ValidateForm()
        {
            IPaymentMethodModule ctrl = GetPaymentModule();
            if (ctrl != null)
                return ctrl.ValidateForm() && Page.IsValid;
            return Page.IsValid;
        }

        public PaymentInfo GetPaymentInfo()
        {
            PaymentInfo paymentInfo = null;
            IPaymentMethodModule ctrl = GetPaymentModule();
            if (ctrl != null)
                paymentInfo = ctrl.GetPaymentInfo();
            paymentInfo.PaymentMethodID = NopContext.Current.User.LastPaymentMethodID;
            return paymentInfo;
        }

        private IPaymentMethodModule GetPaymentModule()
        {
            foreach (Control ctrl in this.PaymentInfoPlaceHolder.Controls)
                if (ctrl is IPaymentMethodModule)
                    return (IPaymentMethodModule)ctrl;
            return null;
        }

        protected void btnNextStep_Click(object sender, EventArgs e)
        {
            if (this.ValidateForm())
            {
                this.PaymentInfo = this.GetPaymentInfo();
                Response.Redirect("~/CheckoutConfirm.aspx");
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
            if (Cart.Count == 0)
                Response.Redirect("~/ShoppingCart.aspx");
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