﻿//------------------------------------------------------------------------------
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
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.Content.Forums;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Localization;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Common;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class ForumSearchControl : BaseNopUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(this.SearchTerms))
                    txtSearchTerm.Text = this.SearchTerms;
                BindData();
            }
        }
        
        protected void BindData()
        {
            try
            {
                if (!String.IsNullOrEmpty(txtSearchTerm.Text))
                {
                    //can be removed
                    if (String.IsNullOrEmpty(txtSearchTerm.Text))
                        throw new NopException(LocalizationManager.GetLocaleResourceString("Forum.SearchTermCouldNotBeEmpty"));
                    if (txtSearchTerm.Text.Length < 3)
                        throw new NopException(LocalizationManager.GetLocaleResourceString("Forum.SearchTermMinimumLengthIs3Characters"));

                    string keywords = txtSearchTerm.Text;

                    int totalRecords = 0;
                    int pageSize = 10;
                    if (ForumManager.SearchResultsPageSize > 0)
                    {
                        pageSize = ForumManager.SearchResultsPageSize;
                    }

                    ForumTopicCollection forumTopics = ForumManager.GetAllTopics(0, 0, keywords, true,
                        pageSize, this.CurrentPageIndex, out totalRecords);
                    if (forumTopics.Count > 0)
                    {
                        this.searchPager1.PageSize = pageSize;
                        this.searchPager1.TotalRecords = totalRecords;
                        this.searchPager1.PageIndex = this.CurrentPageIndex;

                        this.searchPager2.PageSize = pageSize;
                        this.searchPager2.TotalRecords = totalRecords;
                        this.searchPager2.PageIndex = this.CurrentPageIndex;

                        rptrSearchResults.DataSource = forumTopics;
                        rptrSearchResults.DataBind();
                    }

                    rptrSearchResults.Visible = (forumTopics.Count > 0);
                    lblNoResults.Visible = !(rptrSearchResults.Visible);
                }
                else
                {
                    rptrSearchResults.Visible = false;
                }
            }
            catch (Exception exc)
            {
                rptrSearchResults.Visible = false;
                lblError.Text = Server.HtmlEncode(exc.Message);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string url = SEOHelper.GetForumSearchURL(txtSearchTerm.Text);
            Response.Redirect(url);
        }

        protected void rptrSearchResults_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ForumTopic forumTopic = e.Item.DataItem as ForumTopic;
                Customer customer = forumTopic.User;

                Panel pnlTopicImage = e.Item.FindControl("pnlTopicImage") as Panel;
                if (pnlTopicImage != null)
                {
                    switch (forumTopic.TopicType)
                    {
                        case ForumTopicTypeEnum.Normal:
                            pnlTopicImage.CssClass = "post";
                            break;
                        case ForumTopicTypeEnum.Announcement:
                            pnlTopicImage.CssClass = "postannoucement";
                            break;
                        default:
                            pnlTopicImage.CssClass = "post";
                            break;
                    }
                }

                Label lblTopicType = e.Item.FindControl("lblTopicType") as Label;
                if (lblTopicType != null)
                {
                    switch (forumTopic.TopicType)
                    {
                        case ForumTopicTypeEnum.Announcement:
                            lblTopicType.Text = string.Format("[{0}]", GetLocaleResourceString("Forum.Announcement"));
                            break;
                        default:
                            lblTopicType.Visible = false;
                            break;
                    }
                }

                HyperLink hlTopic = e.Item.FindControl("hlTopic") as HyperLink;
                if (hlTopic != null)
                {
                    hlTopic.NavigateUrl = SEOHelper.GetForumTopicURL(forumTopic.ForumTopicID);
                    hlTopic.Text = Server.HtmlEncode(forumTopic.Subject);
                }

                HyperLink hlTopicStarter = e.Item.FindControl("hlTopicStarter") as HyperLink;
                if (hlTopicStarter != null)
                {
                    if (CustomerManager.AllowViewingProfiles)
                    {
                        if (customer != null)
                        {
                            hlTopicStarter.Text = Server.HtmlEncode(CustomerManager.FormatUserName(customer));
                            hlTopicStarter.NavigateUrl = SEOHelper.GetUserProfileURL(customer.CustomerID);
                        }
                    }
                    else
                    {
                        hlTopicStarter.Visible = false;
                    }
                }

                Label lblTopicStarter = e.Item.FindControl("lblTopicStarter") as Label;
                if (lblTopicStarter != null)
                {
                    if (!CustomerManager.AllowViewingProfiles)
                    {
                        if (customer != null)
                        {
                            lblTopicStarter.Text = Server.HtmlEncode(CustomerManager.FormatUserName(customer));
                        }
                    }
                    else
                    {
                        lblTopicStarter.Visible = false;
                    }
                }
            }
        }

        public string SearchTerms
        {
            get
            {
                return CommonHelper.QueryString("SearchTerms");
            }
        }

        public int CurrentPageIndex
        {
            get
            {
                int _pageIndex = CommonHelper.QueryStringInt(searchPager1.QueryStringProperty);
                _pageIndex--;
                if (_pageIndex < 0)
                    _pageIndex = 0;
                return _pageIndex;
            }
        }

    }
}
