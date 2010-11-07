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
using System.Collections.ObjectModel;
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
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.Content.Forums;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Utils.Html;
using NopSolutions.NopCommerce.Common.Xml;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class ProfileInfoControl : BaseNopUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            Customer customer = CustomerManager.GetCustomerByID(this.CustomerID);
            if (customer == null)
            {
                this.Visible = false;
                return;
            }

            if (CustomerManager.AllowCustomersToUploadAvatars)
            {
                phAvatar.Visible = true;
                Picture customerAvatar = customer.Avatar;
                int avatarSize = SettingManager.GetSettingValueInteger("Media.Customer.AvatarSize", 85);
                if (customerAvatar != null)
                {
                    string pictureUrl = PictureManager.GetPictureUrl(customerAvatar, avatarSize, false);
                    this.imgAvatar.ImageUrl = pictureUrl;
                }
                else
                {
                    if (CustomerManager.DefaultAvatarEnabled)
                    {
                        string pictureUrl = PictureManager.GetDefaultPictureUrl(PictureTypeEnum.Avatar, avatarSize);
                        this.imgAvatar.ImageUrl = pictureUrl;
                    }
                    else
                    {
                        phAvatar.Visible = false;
                    }
                }
            }
            else
            {
                phAvatar.Visible = false;
            }

            //TODO make configurable
            //if (false)
            //{
            //    lblFullName.Text = Server.HtmlEncode(customer.FullName);
            //}
            //else
            //{
            phFullName.Visible = false;
            //}


            if (CustomerManager.ShowCustomersLocation)
            {
                phLocation.Visible = true;
                Country country = CountryManager.GetCountryByID(customer.CountryID);
                if (country != null)
                {
                    lblCountry.Text = Server.HtmlEncode(country.Name);
                }
                else
                {
                    phLocation.Visible = false;
                }
            }
            else
            {
                phLocation.Visible = false;
            }

            if (ForumManager.AllowPrivateMessages)
            {
                if (customer != null && !customer.IsGuest)
                {
                    btnSendPM.CustomerID = customer.CustomerID;
                    phPM.Visible = true;
                }
                else
                {
                    phPM.Visible = false;
                }
            }
            else
            {
                phPM.Visible = false;
            }

            if (ForumManager.ForumsEnabled && ForumManager.ShowCustomersPostCount)
            {
                phTotalPosts.Visible = true;
                lblTotalPosts.Text = customer.TotalForumPosts.ToString();
            }
            else
            {
                phTotalPosts.Visible = false;
            }

            if (CustomerManager.ShowCustomersJoinDate)
            {
                phJoinDate.Visible = true;
                lblJoinDate.Text = DateTimeHelper.ConvertToUserTime(customer.RegistrationDate).ToString("f");
            }
            else
            {
                phJoinDate.Visible = false;
            }


            if (customer.DateOfBirth.HasValue)
            {
                lblDateOfBirth.Text = customer.DateOfBirth.Value.ToString("D");
            }
            else
            {
                phDateOfBirth.Visible = false;
            }

            if (ForumManager.ForumsEnabled)
            {
                int totaRecords = 0;
                int pageSize = 5;
                if (ForumManager.LatestUserPostsPageSize > 0)
                {
                    pageSize = ForumManager.LatestUserPostsPageSize;
                }
                ForumPostCollection forumPosts = ForumManager.GetAllPosts(0, customer.CustomerID, string.Empty,
                    false, pageSize, 0, out totaRecords);
                if (forumPosts.Count > 0)
                {
                    rptrLatestPosts.DataSource = forumPosts;
                    rptrLatestPosts.DataBind();
                }
                else
                {
                    phLatestPosts.Visible = false;
                }
            }
            else
            {
                phLatestPosts.Visible = false;
            }
        }

        protected void rptrLatestPosts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ForumPost forumPost = e.Item.DataItem as ForumPost;

                HyperLink hlTopic = e.Item.FindControl("hlTopic") as HyperLink;
                if (hlTopic != null)
                {
                    ForumTopic forumTopic = forumPost.Topic;
                    if (forumTopic != null)
                    {
                        hlTopic.Text = Server.HtmlEncode(forumTopic.Subject);
                        hlTopic.NavigateUrl = SEOHelper.GetForumTopicURL(forumPost.TopicID);
                    }
                }

                Label lblPosted = e.Item.FindControl("lblPosted") as Label;
                if (lblPosted != null)
                {
                    lblPosted.Text = DateTimeHelper.ConvertToUserTime(forumPost.CreatedOn).ToString("MMMM dd, yyyy");
                }

                Label lblPost = e.Item.FindControl("lblPost") as Label;
                if (lblPost != null)
                {
                    lblPost.Text = ForumManager.FormatPostText(forumPost.Text);
                }
            }
        }

        public int CustomerID
        {
            get
            {
                return CommonHelper.QueryStringInt("UserID");
            }
        }
    }
}