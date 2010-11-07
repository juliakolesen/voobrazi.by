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
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Utils.Html;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class ForumPostControl : BaseNopUserControl
    {
        ForumPost forumPost = null;

        public override void DataBind()
        {
            base.DataBind();
            this.BindData();
        }

        public void BindData()
        {
            if (forumPost != null)
            {
                lAnchor.Text = string.Format("<a name=\"{0}\"></a>", forumPost.ForumPostID);

                btnEdit.Visible = ForumManager.IsUserAllowedToEditPost(NopContext.Current.User, forumPost);
                btnDelete.Visible = ForumManager.IsUserAllowedToDeletePost(NopContext.Current.User, forumPost);

                lblDate.Text = DateTimeHelper.ConvertToUserTime(forumPost.CreatedOn).ToString("f");
                lblText.Text = ForumManager.FormatPostText(forumPost.Text);
                lblForumPostID.Text = forumPost.ForumPostID.ToString();

                Customer customer = forumPost.User;
                if (customer != null)
                {
                    if (CustomerManager.AllowViewingProfiles)
                    {
                        hlUser.Text = Server.HtmlEncode(CustomerManager.FormatUserName(customer));
                        hlUser.NavigateUrl = SEOHelper.GetUserProfileURL(customer.CustomerID);
                        lblUser.Visible = false;
                    }
                    else
                    {
                        lblUser.Text = Server.HtmlEncode(CustomerManager.FormatUserName(customer));
                        hlUser.Visible = false;
                    }

                    if (CustomerManager.AllowCustomersToUploadAvatars)
                    {
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
                                imgAvatar.Visible = false;
                            }
                        }
                    }
                    else
                    {
                        imgAvatar.Visible = false;
                    }

                    if (customer.IsForumModerator)
                    {
                        lblStatus.Text = GetLocaleResourceString("Forum.Moderator");
                    }
                    else
                    {
                        phStatus.Visible = false;
                    }

                    if (ForumManager.ShowCustomersPostCount)
                    {
                        lblTotalPosts.Text = customer.TotalForumPosts.ToString();
                    }
                    else
                    {
                        phTotalPosts.Visible = false;
                    }

                    if (CustomerManager.ShowCustomersJoinDate)
                    {
                        lblJoined.Text = DateTimeHelper.ConvertToUserTime(customer.RegistrationDate).ToString("d");
                    }
                    else
                    {
                        phJoined.Visible = false;
                    }

                    if (CustomerManager.ShowCustomersLocation)
                    {
                        Country country = CountryManager.GetCountryByID(customer.CountryID);
                        if (country != null)
                        {
                            lblLocation.Text = Server.HtmlEncode(country.Name);
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

                    if (ForumManager.SignaturesEnabled && !String.IsNullOrEmpty(customer.Signature))
                    {
                        lblSignature.Text = ForumManager.FormatSignatureText(customer.Signature);
                    }
                    else
                    {
                        pnlSignature.Visible = false;
                    }
                }
                else
                {
                    //error, cannot be
                }
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            int forumPostID = 0;
            int.TryParse(lblForumPostID.Text, out forumPostID);
            ForumPost forumPost = ForumManager.GetPostByID(forumPostID);
            if (forumPost != null)
            {
                if (!ForumManager.IsUserAllowedToEditPost(NopContext.Current.User, forumPost))
                {
                    string loginURL = CommonHelper.GetLoginPageURL(true);
                    Response.Redirect(loginURL);
                }

                string url = SEOHelper.GetEditForumPostURL(forumPost.ForumPostID);
                Response.Redirect(url);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int forumPostID = 0;
            int.TryParse(lblForumPostID.Text, out forumPostID);
            ForumPost forumPost = ForumManager.GetPostByID(forumPostID);
            if (forumPost != null)
            {
                ForumTopic forumTopic = forumPost.Topic;
                if (!ForumManager.IsUserAllowedToDeletePost(NopContext.Current.User, forumPost))
                {
                    string loginURL = CommonHelper.GetLoginPageURL(true);
                    Response.Redirect(loginURL);
                }

                ForumManager.DeletePost(forumPost.ForumPostID);

                string url = string.Empty;
                if (forumTopic != null)
                {
                    url = SEOHelper.GetForumTopicURL(forumTopic.ForumTopicID);
                }
                else
                {
                    url = SEOHelper.GetForumMainURL();
                }
                Response.Redirect(url);
            }
        }

        public ForumPost ForumPost
        {
            get
            {
                return forumPost;
            }
            set
            {
                forumPost = value;
            }
        }

    }
}
