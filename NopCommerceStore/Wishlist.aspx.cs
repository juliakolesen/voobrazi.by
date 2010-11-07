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
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.Web
{
    public partial class WishlistPage : BaseNopPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Response.CacheControl = "private";
            Response.Expires = 0;
            Response.AddHeader("pragma", "no-cache");

            if (!SettingManager.GetSettingValueBoolean("Common.EnableWishlist"))
                Response.Redirect("~/Default.aspx");

            string title = GetLocaleResourceString("PageTitle.Wishlist");
            SEOHelper.RenderTitle(this, title, true);

            if (!Page.IsPostBack)
            {
                CommonHelper.EnsureSSL();

                Customer customer = CustomerManager.GetCustomerByGUID(this.CustomerGUID.HasValue ? this.CustomerGUID.Value : Guid.Empty);
                if (customer != null)
                {
                    lblTitle.Text = string.Format(GetLocaleResourceString("Wishlist.WishlistOf"), Server.HtmlEncode(customer.FullName), Server.HtmlEncode(customer.Email));
                    CustomerSession customerSession = CustomerManager.GetCustomerSessionByCustomerID(customer.CustomerID);
                    if (customerSession != null)
                        ctrlWishlist.CustomerSessionGuid = customerSession.CustomerSessionGUID;
                    ctrlWishlist.IsEditable = false;
                    ctrlWishlist.BindData();
                }
                else
                {
                    lblTitle.Text = GetLocaleResourceString("Wishlist.YourWishlist");
                    if (NopContext.Current.Session != null)
                        ctrlWishlist.CustomerSessionGuid = NopContext.Current.Session.CustomerSessionGUID;
                    ctrlWishlist.IsEditable = true;
                    ctrlWishlist.BindData();

                    if (NopContext.Current.User != null && !NopContext.Current.User.IsGuest)
                    {
                        lblYourWishlistURL.Visible = true;
                        lnkWishListUrl.Visible = true;
                        lblYourWishlistURL.Text = GetLocaleResourceString("Wishlist.YourWishlistURL");
                        string wishListUrl = CommonHelper.GetStoreLocation() + "Wishlist.aspx?CustomerGUID=" + NopContext.Current.User.CustomerGUID.ToString();
                        lnkWishListUrl.NavigateUrl = wishListUrl;
                        lnkWishListUrl.Text = wishListUrl;                      
                    }
                }
            }
        }

        public Guid? CustomerGUID
        {
            get
            {
                return CommonHelper.QueryStringGUID("CustomerGUID");
            }
        }
    }
}