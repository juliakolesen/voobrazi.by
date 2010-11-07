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
using System.Web.Caching;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.BusinessLogic.Shipping;
using NopSolutions.NopCommerce.Common.Utils;
 

namespace NopSolutions.NopCommerce.Web
{
    public partial class CheckoutPage : BaseNopPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CommonHelper.EnsureSSL();
            }

            Response.CacheControl = "private";
            Response.Expires = 0;
            Response.AddHeader("pragma", "no-cache");

            ShoppingCart cart = ShoppingCartManager.GetCurrentShoppingCart(ShoppingCartTypeEnum.ShoppingCart);
            if (cart.Count == 0)
                Response.Redirect("~/ShoppingCart.aspx");

            if (NopContext.Current.User == null && CustomerManager.AnonymousCheckoutAllowed)
            {
                //create anonymous record
                string email = "anonymous@anonymous.com";
                string password = string.Empty;
                MembershipCreateStatus status = MembershipCreateStatus.UserRejected;
                Customer guestCustomer = CustomerManager.AddCustomer(email, email, password, false, true, true, out status);
                if (guestCustomer != null && status == MembershipCreateStatus.Success)
                {
                    NopContext.Current.User = guestCustomer;

                    if (NopContext.Current.Session == null)
                    {
                        NopContext.Current.Session = NopContext.Current.GetSession(true);
                    }

                    NopContext.Current.Session = CustomerManager.SaveCustomerSession(NopContext.Current.Session.CustomerSessionGUID,
                        guestCustomer.CustomerID, DateTime.Now, NopContext.Current.Session.IsExpired);
                }
            }

            if ((NopContext.Current.User == null) || (NopContext.Current.User.IsGuest && !CustomerManager.AnonymousCheckoutAllowed))
            {
                string loginURL = CommonHelper.GetLoginPageURL(true);
                Response.Redirect(loginURL);
            }

            string title = GetLocaleResourceString("PageTitle.Checkout");
            SEOHelper.RenderTitle(this, title, true);

			Response.Redirect("~/CheckoutBillingAddress.aspx");
        }

    }
} 