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
using System.ComponentModel;
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
using NopSolutions.NopCommerce.BusinessLogic.Content.NewsManagement;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Utils.Html;
using NopSolutions.NopCommerce.Common;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class NewsItemControl : BaseNopUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                this.BindData();
        }

        private void BindData()
        {
            pnlError.Visible = false;

            News news = NewsManager.GetNewsByID(this.NewsID);
            if (news != null && news.Published)
            {
                this.lTitle.Text = Server.HtmlEncode(news.Title);
                this.lCreatedOn.Text = DateTimeHelper.ConvertToUserTime(news.CreatedOn).ToString();
                this.lFull.Text = news.Full;

                if (news.AllowComments)
                {
                    if (!NewsManager.AllowNotRegisteredUsersToLeaveComments
                        && (NopContext.Current.User == null || NopContext.Current.User.IsGuest))
                    {
                        lblLeaveYourComment.Text = GetLocaleResourceString("News.OnlyRegisteredUsersCanLeaveComments");
                        txtTitle.Enabled = false;
                        txtComment.Enabled = false;
                        btnComment.Enabled = false;
                    }
                    else
                    {
                        lblLeaveYourComment.Text = GetLocaleResourceString("News.LeaveYourComment");
                        txtTitle.Enabled = true;
                        txtComment.Enabled = true;
                        btnComment.Enabled = true;
                    }

                    NewsCommentCollection newsComments = news.NewsComments;
                    if (newsComments.Count > 0)
                    {
                        rptrComments.DataSource = newsComments;
                        rptrComments.DataBind();
                    }
                }
                else
                {
                    pnlComments.Visible = false;
                }
            }
            else
                Response.Redirect("~/Default.aspx");
        }

        protected void btnComment_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    News news = NewsManager.GetNewsByID(this.NewsID);
                    if (news != null && news.AllowComments)
                    {
                        if (!NewsManager.AllowNotRegisteredUsersToLeaveComments
                               && (NopContext.Current.User == null || NopContext.Current.User.IsGuest))
                        {
                            lblLeaveYourComment.Text = GetLocaleResourceString("News.OnlyRegisteredUsersCanLeaveComments");
                            return;
                        }

                        string title = txtTitle.Text.Trim();
                        string comment = txtComment.Text.Trim();
                        if (String.IsNullOrEmpty(comment))
                        {
                            throw new NopException(GetLocaleResourceString("News.PleaseEnterCommentText"));
                        }

                        int customerID = 0;
                        if (NopContext.Current.User != null && !NopContext.Current.User.IsGuest)
                            customerID = NopContext.Current.User.CustomerID;

                        NewsManager.InsertNewsComment(news.NewsID, customerID, title, comment, DateTime.Now);
                        txtTitle.Text = string.Empty;
                        txtComment.Text = string.Empty;
                        BindData();
                    }
                    else
                        Response.Redirect("~/Default.aspx");
                }
            }
            catch (Exception exc)
            {
                pnlError.Visible = true;
                lErrorMessage.Text = Server.HtmlEncode(exc.Message);
            }
        }

        public int NewsID
        {
            get
            {
                return CommonHelper.QueryStringInt("NewsID");
            }
        }
    }
}