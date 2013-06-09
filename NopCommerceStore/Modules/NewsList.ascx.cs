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
using System.ComponentModel;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Content.NewsManagement;
using NopSolutions.NopCommerce.BusinessLogic.SEO;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class NewsListControl : BaseNopUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }

        protected string GetNewsRSSUrl()
        {
            return SEOHelper.GetNewsRssURL();
        }

        protected void BindData()
        {
            NewsCollection newsCollection = NewsManager.GetAllNews(0);
            if (newsCollection.Count > 0)
            {
                rptrNews.DataSource = newsCollection;
                rptrNews.DataBind();

            }
            else
                this.Visible = false;
        }

        [DefaultValue(0)]
        public int NewsCount
        {
            get
            {
                if (ViewState["NewsCount"] == null)
                    return 0;
                else
                    return (int)ViewState["NewsCount"];
            }
            set { ViewState["NewsCount"] = value; }
        }
    }
}