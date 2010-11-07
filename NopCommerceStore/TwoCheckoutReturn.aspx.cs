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
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
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
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.Web
{
    public partial class TwoCheckoutReturnPage : BaseNopPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.CacheControl = "private";
            Response.Expires = 0;
            Response.AddHeader("pragma", "no-cache");
            
            //TODO implement, validate MD5
            Response.Redirect("~/Default.aspx");

            if ((NopContext.Current.User == null) || (NopContext.Current.User.IsGuest && !CustomerManager.AnonymousCheckoutAllowed))
            {
                string loginURL = CommonHelper.GetLoginPageURL(true);
                Response.Redirect(loginURL);
            }

            if (!Page.IsPostBack)
            {
                bool ok = false;
                if (HttpContext.Current.Request.Form["x_2checked"] != null)
                    ok = HttpContext.Current.Request.Form["x_2checked"] == "Y" || HttpContext.Current.Request.Form["x_2checked"] == "K";

                if (ok)
                {
                    OrderCollection orderCollection = NopContext.Current.User.Orders;
                    if (orderCollection.Count == 0)
                        Response.Redirect("~/Default.aspx");
                    Order lastOrder = orderCollection[0];
                    OrderManager.MarkOrderAsPaid(lastOrder.OrderID);
                    Response.Redirect("~/CheckoutCompleted.aspx");
                }
                else
                    Response.Redirect("~/Default.aspx");
            }
        }
    }
}