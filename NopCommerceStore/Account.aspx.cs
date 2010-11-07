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
using NopSolutions.NopCommerce.BusinessLogic.Content.Forums;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.Common.Utils;
namespace NopSolutions.NopCommerce.Web
{
    /// <summary>
    /// AccountPage page handler.
    /// </summary>
    public partial class AccountPage : BaseNopPage
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //TODO use Response.Cache.SetCacheability(HttpCacheability.NoCache); or  Response.Cache.SetNoStore();
            Response.CacheControl = "private";
            Response.Expires = 0;
            Response.AddHeader("pragma", "no-cache");

            if (!Page.IsPostBack)
            {
                CommonHelper.EnsureSSL();
            }

            if (NopContext.Current.User == null || NopContext.Current.User.IsGuest)
            {
                string loginURL = CommonHelper.GetLoginPageURL(true);
                Response.Redirect(loginURL);
            }

            string title = GetLocaleResourceString("PageTitle.Account");
            SEOHelper.RenderTitle(this, title, true);

            if (!Page.IsPostBack)
            {
                pnlAvatar.Visible = CustomerManager.AllowCustomersToUploadAvatars;
            }
        }
    }
}