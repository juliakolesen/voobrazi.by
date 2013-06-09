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
using System.Web;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class HeaderMenuControl : BaseNopUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string sPagePath = HttpContext.Current.Request.Url.AbsolutePath;
            Action.Style.Add("background",
                             sPagePath.Contains("Actions.aspx")
                                 ? "url(../../images/ff_images/menu/action2.gif) center no-repeat"
                                 : "url(../../images/ff_images/menu/action.gif) center no-repeat");

            Clients.Style.Add("background",
                             sPagePath.Contains("ToCorporateClients.aspx")
                                 ? "url(../../images/ff_images/menu/client2.gif) center no-repeat"
                                 : "url(../../images/ff_images/menu/client.gif) center no-repeat");

            Green.Style.Add("background",
                            sPagePath.Contains("Greening.aspx")
                                ? "url(../../images/ff_images/menu/landscaping2.gif) center no-repeat"
                                : "url(../../images/ff_images/menu/landscaping.gif) center no-repeat");

            News.Style.Add("background",
                            sPagePath.Contains("News")
                                ? "url(../../images/ff_images/menu/news2.gif) center no-repeat"
                                : "url(../../images/ff_images/menu/news.gif) center no-repeat");
        }
    }
}