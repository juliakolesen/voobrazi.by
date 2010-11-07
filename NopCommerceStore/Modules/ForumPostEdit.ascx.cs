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
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Utils.Html;
using NopSolutions.NopCommerce.Common;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class ForumPostEditControl : BaseNopUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                FillDropDowns();
                BindData();
            }

            if (ForumManager.ForumEditor == EditorTypeEnum.BBCodeEditor)
            {
                LoadBBCodeEditorJS();
            }
        }

        private void LoadBBCodeEditorJS()
        {
            string bbCodeJS = "<script src='" + Page.ResolveUrl("~/editors/BBEditor/ed.js") + "' type='text/javascript'></script>";
            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "BBCodeEditor", bbCodeJS, false);
        }

        private void FillDropDowns()
        {
            ddlPriority.Items.Clear();

            ListItem ddlPriorityNormalItem = new ListItem(GetLocaleResourceString("Forum.Normal"), ((int)ForumTopicTypeEnum.Normal).ToString());
            ddlPriority.Items.Add(ddlPriorityNormalItem);

            ListItem ddlPriorityAnnouncementItem = new ListItem(GetLocaleResourceString("Forum.Announcement"), ((int)ForumTopicTypeEnum.Announcement).ToString());
            ddlPriority.Items.Add(ddlPriorityAnnouncementItem);
        }

        private void BindData()
        {
            pnlError.Visible = false;

            txtTopicBodySimple.Visible = false;
            txtTopicBodyBBCode.Visible = false;
            txtTopicBodyHtml.Visible = false;
            switch (ForumManager.ForumEditor)
            {
                case EditorTypeEnum.SimpleTextBox:
                    {
                        txtTopicBodySimple.Visible = true;
                        rfvTopicBody.ControlToValidate = "txtTopicBodySimple";
                    }
                    break;
                case EditorTypeEnum.BBCodeEditor:
                    {
                        txtTopicBodyBBCode.Visible = true;
                        rfvTopicBody.ControlToValidate = "txtTopicBodyBBCode";
                    }
                    break;
                case EditorTypeEnum.HtmlEditor:
                    {
                        txtTopicBodyHtml.Visible = true;
                        rfvTopicBody.Enabled = false;
                    }
                    break;
                default:
                    break;
            }

            if (this.AddTopic)
            {
                #region Adding topic

                Forum forum = ForumManager.GetForumByID(this.ForumID);
                if (forum == null)
                {
                    Response.Redirect(SEOHelper.GetForumMainURL());
                }

                if (!ForumManager.IsUserAllowedToCreateTopic(NopContext.Current.User, forum))
                {
                    string loginURL = CommonHelper.GetLoginPageURL(true);
                    Response.Redirect(loginURL);
                }

                lblTitle.Text = GetLocaleResourceString("Forum.NewTopic");
                phForumName.Visible = true;
                lblForumName.Text = Server.HtmlEncode(forum.Name);
                txtTopicTitle.Visible = true;
                txtTopicTitle.Text = string.Empty;
                lblTopicTitle.Visible = false;
                lblTopicTitle.Text = string.Empty;
                
                ctrlForumBreadcrumb.ForumID = forum.ForumID;
                ctrlForumBreadcrumb.BindData();

                phPriority.Visible = ForumManager.IsUserAllowedToSetTopicPriority(NopContext.Current.User);
                phSubscribe.Visible = ForumManager.IsUserAllowedToSubscribe(NopContext.Current.User);

                #endregion
            }
            else if (this.EditTopic)
            {
                #region Editing topic
                ForumTopic forumTopic = ForumManager.GetTopicByID(this.ForumTopicID);

                if (forumTopic == null)
                {
                    Response.Redirect(SEOHelper.GetForumMainURL());
                }

                if (!ForumManager.IsUserAllowedToEditTopic(NopContext.Current.User, forumTopic))
                {
                    string loginURL = CommonHelper.GetLoginPageURL(true);
                    Response.Redirect(loginURL);
                }

                Forum forum = forumTopic.Forum;
                if (forum == null)
                {
                    Response.Redirect(SEOHelper.GetForumMainURL());
                }

                lblTitle.Text = GetLocaleResourceString("Forum.EditTopic");
                phForumName.Visible = true;
                lblForumName.Text = Server.HtmlEncode(forum.Name);
                txtTopicTitle.Visible = true;
                txtTopicTitle.Text = forumTopic.Subject;
                lblTopicTitle.Visible = false;
                lblTopicTitle.Text = string.Empty;

                ctrlForumBreadcrumb.ForumTopicID = forumTopic.ForumTopicID;
                ctrlForumBreadcrumb.BindData();
                
                CommonHelper.SelectListItem(this.ddlPriority, forumTopic.TopicTypeID);

                ForumPost firstPost = forumTopic.FirstPost;
                if (firstPost != null)
                {
                    switch (ForumManager.ForumEditor)
                    {
                        case EditorTypeEnum.SimpleTextBox:
                            {
                                txtTopicBodySimple.Text = firstPost.Text;
                            }
                            break;
                        case EditorTypeEnum.BBCodeEditor:
                            {
                                txtTopicBodyBBCode.Text = firstPost.Text;
                            }
                            break;
                        case EditorTypeEnum.HtmlEditor:
                            {
                                txtTopicBodyHtml.Value = firstPost.Text;
                            }
                            break;
                        default:
                            break;
                    }
                }


                phPriority.Visible = ForumManager.IsUserAllowedToSetTopicPriority(NopContext.Current.User);
                //subscription
                if (ForumManager.IsUserAllowedToSubscribe(NopContext.Current.User.CustomerID))
                {
                    phSubscribe.Visible = true;
                    ForumSubscription forumSubscription = ForumManager.GetAllSubscriptions(NopContext.Current.User.CustomerID,
                        0, forumTopic.ForumTopicID, 1, 0).FirstOrDefault();
                    cbSubscribe.Checked = forumSubscription != null;
                }
                else
                {
                    phSubscribe.Visible = false;
                }
                #endregion
            }
            else if (this.AddPost)
            {
                #region Adding post

                ForumTopic forumTopic = ForumManager.GetTopicByID(this.ForumTopicID);
                if (forumTopic == null)
                {
                    Response.Redirect(SEOHelper.GetForumMainURL());
                }

                if (!ForumManager.IsUserAllowedToCreatePost(NopContext.Current.User, forumTopic))
                {
                    string loginURL = CommonHelper.GetLoginPageURL(true);
                    Response.Redirect(loginURL);
                }

                ctrlForumBreadcrumb.ForumTopicID = forumTopic.ForumTopicID;
                ctrlForumBreadcrumb.BindData();

                lblTitle.Text = GetLocaleResourceString("Forum.NewPost");
                phForumName.Visible = false;
                lblForumName.Text = string.Empty;
                txtTopicTitle.Visible = false;
                txtTopicTitle.Text = string.Empty;
                lblTopicTitle.Visible = true;
                lblTopicTitle.Text = Server.HtmlEncode(forumTopic.Subject);

                phPriority.Visible = false;
                //subscription
                if (ForumManager.IsUserAllowedToSubscribe(NopContext.Current.User))
                {
                    phSubscribe.Visible = true;
                    ForumSubscription forumSubscription = ForumManager.GetAllSubscriptions(NopContext.Current.User.CustomerID,
                        0, forumTopic.ForumTopicID, 1, 0).FirstOrDefault();
                    cbSubscribe.Checked = forumSubscription != null;
                }
                else
                {
                    phSubscribe.Visible = false;
                }
                #endregion
            }
            else if (this.EditPost)
            {
                #region Editing post

                ForumPost forumPost = ForumManager.GetPostByID(this.ForumPostID);

                if (forumPost == null)
                {
                    Response.Redirect(SEOHelper.GetForumMainURL());
                }

                if (!ForumManager.IsUserAllowedToEditPost(NopContext.Current.User, forumPost))
                {
                    string loginURL = CommonHelper.GetLoginPageURL(true);
                    Response.Redirect(loginURL);
                }

                ForumTopic forumTopic = forumPost.Topic;
                if (forumTopic == null)
                {
                    Response.Redirect(SEOHelper.GetForumMainURL());
                }

                lblTitle.Text = GetLocaleResourceString("Forum.EditPost");
                phForumName.Visible = false;
                lblForumName.Text = string.Empty;
                txtTopicTitle.Visible = false;
                txtTopicTitle.Text = string.Empty;
                lblTopicTitle.Visible = true;
                lblTopicTitle.Text = Server.HtmlEncode(forumTopic.Subject);

                ctrlForumBreadcrumb.ForumTopicID = forumTopic.ForumTopicID;
                ctrlForumBreadcrumb.BindData();


                switch (ForumManager.ForumEditor)
                {
                    case EditorTypeEnum.SimpleTextBox:
                        {
                            txtTopicBodySimple.Text = forumPost.Text;
                        }
                        break;
                    case EditorTypeEnum.BBCodeEditor:
                        {
                            txtTopicBodyBBCode.Text = forumPost.Text;
                        }
                        break;
                    case EditorTypeEnum.HtmlEditor:
                        {
                            txtTopicBodyHtml.Value = forumPost.Text;
                        }
                        break;
                    default:
                        break;
                }

                phPriority.Visible = false;
                //subscription
                if (ForumManager.IsUserAllowedToSubscribe(NopContext.Current.User.CustomerID))
                {
                    phSubscribe.Visible = true;
                    ForumSubscription forumSubscription = ForumManager.GetAllSubscriptions(NopContext.Current.User.CustomerID,
                        0, forumTopic.ForumTopicID, 1, 0).FirstOrDefault();
                    cbSubscribe.Checked = forumSubscription != null;
                }
                else
                {
                    phSubscribe.Visible = false;
                }
                #endregion
            }
            else
            {
                Response.Redirect(SEOHelper.GetForumMainURL());
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string text = string.Empty;

                switch (ForumManager.ForumEditor)
                {
                    case EditorTypeEnum.SimpleTextBox:
                        {
                            text = txtTopicBodySimple.Text.Trim();
                        }
                        break;
                    case EditorTypeEnum.BBCodeEditor:
                        {
                            text = txtTopicBodyBBCode.Text.Trim();
                        }
                        break;
                    case EditorTypeEnum.HtmlEditor:
                        {
                            text = txtTopicBodyHtml.Value;
                        }
                        break;
                    default:
                        break;
                }
                
                string subject = txtTopicTitle.Text;
                ForumTopicTypeEnum topicType = ForumTopicTypeEnum.Normal;
                bool subscribe = cbSubscribe.Checked;

                string IPAddress = string.Empty;
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                    IPAddress = HttpContext.Current.Request.UserHostAddress;

                DateTime nowDT = DateTime.Now;

                if (ForumManager.IsUserAllowedToSetTopicPriority(NopContext.Current.User))
                {
                    topicType = (ForumTopicTypeEnum)Enum.ToObject(typeof(ForumTopicTypeEnum), int.Parse(ddlPriority.SelectedItem.Value));
                }

                text = text.Trim();
                if (String.IsNullOrEmpty(text))
                    throw new NopException(GetLocaleResourceString("Forum.TextCannotBeEmpty"));

                if (this.AddTopic)
                {
                    #region Adding topic
                    Forum forum = ForumManager.GetForumByID(this.ForumID);
                    if (forum == null)
                    {
                        Response.Redirect(SEOHelper.GetForumMainURL());
                    }

                    if (!ForumManager.IsUserAllowedToCreateTopic(NopContext.Current.User, forum))
                    {
                        string loginURL = CommonHelper.GetLoginPageURL(true);
                        Response.Redirect(loginURL);
                    }
                    
                    subject = subject.Trim();
                    if (String.IsNullOrEmpty(subject))
                        throw new NopException(GetLocaleResourceString("Forum.TopicSubjectCannotBeEmpty"));

                    ForumTopic forumTopic = ForumManager.InsertTopic(forum.ForumID, NopContext.Current.User.CustomerID,
                        topicType, subject, 0, 0, 0, 0, null, nowDT, nowDT, true);

                    ForumPost forumPost = ForumManager.InsertPost(forumTopic.ForumTopicID, NopContext.Current.User.CustomerID,
                        text, IPAddress, nowDT, nowDT, false);

                    forumTopic = ForumManager.UpdateTopic(forumTopic.ForumTopicID, forumTopic.ForumID,
                        forumTopic.UserID, forumTopic.TopicType, forumTopic.Subject, 1,
                        0, forumPost.ForumPostID, forumTopic.UserID,
                        forumPost.CreatedOn, forumTopic.CreatedOn, nowDT);

                    //subscription
                    if (ForumManager.IsUserAllowedToSubscribe(NopContext.Current.User))
                    {
                        if (subscribe)
                        {
                            ForumSubscription forumSubscription = ForumManager.InsertSubscription(Guid.NewGuid(),
                                NopContext.Current.User.CustomerID, 0, forumTopic.ForumTopicID, nowDT);
                        }
                    }

                    string topicURL = SEOHelper.GetForumTopicURL(forumTopic.ForumTopicID);
                    Response.Redirect(topicURL);
                    #endregion
                }
                else if (this.EditTopic)
                {
                    #region Editing topic
                    ForumTopic forumTopic = ForumManager.GetTopicByID(this.ForumTopicID);
                    if (forumTopic == null)
                    {
                        Response.Redirect(SEOHelper.GetForumMainURL());
                    }

                    if (!ForumManager.IsUserAllowedToEditTopic(NopContext.Current.User, forumTopic))
                    {
                        string loginURL = CommonHelper.GetLoginPageURL(true);
                        Response.Redirect(loginURL);
                    }

                    subject = subject.Trim();
                    if (String.IsNullOrEmpty(subject))
                        throw new NopException(GetLocaleResourceString("Forum.TopicSubjectCannotBeEmpty"));

                    forumTopic = ForumManager.UpdateTopic(forumTopic.ForumTopicID, forumTopic.ForumID,
                        forumTopic.UserID, topicType, subject, forumTopic.NumPosts,
                        forumTopic.Views, forumTopic.LastPostID, forumTopic.LastPostUserID,
                        forumTopic.LastPostTime, forumTopic.CreatedOn, nowDT);

                    ForumPost firstPost = forumTopic.FirstPost;
                    if (firstPost != null)
                    {
                        firstPost = ForumManager.UpdatePost(firstPost.ForumPostID, firstPost.TopicID,
                            firstPost.UserID, text, firstPost.IPAddress, firstPost.CreatedOn, nowDT);
                    }
                    else
                    {
                        //error
                        firstPost = ForumManager.InsertPost(forumTopic.ForumTopicID,
                            forumTopic.UserID, text, IPAddress, forumTopic.CreatedOn, nowDT, false);
                    }

                    //subscription
                    if (ForumManager.IsUserAllowedToSubscribe(NopContext.Current.User.CustomerID))
                    {
                        ForumSubscription forumSubscription = ForumManager.GetAllSubscriptions(NopContext.Current.User.CustomerID,
                            0, forumTopic.ForumTopicID, 1, 0).FirstOrDefault();
                        if (subscribe)
                        {
                            if (forumSubscription == null)
                            {
                                forumSubscription = ForumManager.InsertSubscription(Guid.NewGuid(),
                                    NopContext.Current.User.CustomerID, 0, forumTopic.ForumTopicID, nowDT);
                            }
                        }
                        else
                        {
                            if (forumSubscription != null)
                            {
                                ForumManager.DeleteSubscription(forumSubscription.ForumSubscriptionID);
                            }
                        }
                    }

                    string topicURL = SEOHelper.GetForumTopicURL(forumTopic.ForumTopicID);
                    Response.Redirect(topicURL);
                    #endregion
                }
                else if (this.AddPost)
                {
                    #region Adding post
                    ForumTopic forumTopic = ForumManager.GetTopicByID(this.ForumTopicID);
                    if (forumTopic == null)
                    {
                        Response.Redirect(SEOHelper.GetForumMainURL());
                    }

                    if (!ForumManager.IsUserAllowedToCreatePost(NopContext.Current.User, forumTopic))
                    {
                        string loginURL = CommonHelper.GetLoginPageURL(true);
                        Response.Redirect(loginURL);
                    }

                    ForumPost forumPost = ForumManager.InsertPost(this.ForumTopicID, NopContext.Current.User.CustomerID,
                        text, IPAddress, nowDT, nowDT, true);

                    //subscription
                    if (ForumManager.IsUserAllowedToSubscribe(NopContext.Current.User.CustomerID))
                    {
                        ForumSubscription forumSubscription = ForumManager.GetAllSubscriptions(NopContext.Current.User.CustomerID,
                            0, forumPost.TopicID, 1, 0).FirstOrDefault();
                        if (subscribe)
                        {
                            if (forumSubscription == null)
                            {
                                forumSubscription = ForumManager.InsertSubscription(Guid.NewGuid(),
                                    NopContext.Current.User.CustomerID, 0, forumPost.TopicID, nowDT);
                            }
                        }
                        else
                        {
                            if (forumSubscription != null)
                            {
                                ForumManager.DeleteSubscription(forumSubscription.ForumSubscriptionID);
                            }
                        }
                    }
                    

                    int pageSize = 10;
                    if (ForumManager.PostsPageSize > 0)
                    {
                        pageSize = ForumManager.PostsPageSize;
                    }
                    int pageIndex = ForumManager.CalculateTopicPageIndex(forumPost.TopicID, pageSize, forumPost.ForumPostID);
                    string topicURL = SEOHelper.GetForumTopicURL(forumPost.TopicID, "p", pageIndex + 1, forumPost.ForumPostID);
                    Response.Redirect(topicURL);
                    #endregion
                }
                else if (this.EditPost)
                {
                    #region Editing post
                    ForumPost forumPost = ForumManager.GetPostByID(this.ForumPostID);
                    if (forumPost == null)
                    {
                        Response.Redirect(SEOHelper.GetForumMainURL());
                    }

                    if (!ForumManager.IsUserAllowedToEditPost(NopContext.Current.User, forumPost))
                    {
                        string loginURL = CommonHelper.GetLoginPageURL(true);
                        Response.Redirect(loginURL);
                    }

                    forumPost = ForumManager.UpdatePost(forumPost.ForumPostID, forumPost.TopicID,
                        forumPost.UserID, text, forumPost.IPAddress, forumPost.CreatedOn, nowDT);

                    //subscription
                    if (ForumManager.IsUserAllowedToSubscribe(NopContext.Current.User.CustomerID))
                    {
                        ForumSubscription forumSubscription = ForumManager.GetAllSubscriptions(NopContext.Current.User.CustomerID,
                            0, forumPost.TopicID, 1, 0).FirstOrDefault();
                        if (subscribe)
                        {
                            if (forumSubscription == null)
                            {
                                forumSubscription = ForumManager.InsertSubscription(Guid.NewGuid(),
                                    NopContext.Current.User.CustomerID, 0, forumPost.TopicID, nowDT);
                            }
                        }
                        else
                        {
                            if (forumSubscription != null)
                            {
                                ForumManager.DeleteSubscription(forumSubscription.ForumSubscriptionID);
                            }
                        }
                    }

                    int pageSize = 10;
                    if (ForumManager.PostsPageSize > 0)
                    {
                        pageSize = ForumManager.PostsPageSize;
                    }
                    int pageIndex = ForumManager.CalculateTopicPageIndex(forumPost.TopicID, pageSize, forumPost.ForumPostID);
                    string topicURL = SEOHelper.GetForumTopicURL(forumPost.TopicID, "p", pageIndex + 1, forumPost.ForumPostID);
                    Response.Redirect(topicURL);
                    #endregion
                }
            }
            catch (Exception exc)
            {
                pnlError.Visible = true;
                lErrorMessage.Text = Server.HtmlEncode(exc.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.AddTopic)
                {
                    Forum forum = ForumManager.GetForumByID(this.ForumID);
                    if (forum != null)
                    {
                        string forumUrl = SEOHelper.GetForumURL(forum);
                        Response.Redirect(forumUrl);
                    }
                    else
                    {
                        Response.Redirect(SEOHelper.GetForumMainURL());
                    }
                }
                else if (this.EditTopic)
                {
                    ForumTopic forumTopic = ForumManager.GetTopicByID(this.ForumTopicID);
                    if (forumTopic != null)
                    {
                        string topicUrl = SEOHelper.GetForumTopicURL(forumTopic.ForumTopicID);
                        Response.Redirect(topicUrl);
                    }
                    else
                    {
                        Response.Redirect(SEOHelper.GetForumMainURL());
                    }
                }
                else if (this.AddPost)
                {
                    ForumTopic forumTopic = ForumManager.GetTopicByID(this.ForumTopicID);
                    if (forumTopic != null)
                    {
                        string topicUrl = SEOHelper.GetForumTopicURL(forumTopic.ForumTopicID);
                        Response.Redirect(topicUrl);
                    }
                    else
                    {
                        Response.Redirect(SEOHelper.GetForumMainURL());
                    }
                }
                else if (this.EditPost)
                {
                    ForumPost forumPost = ForumManager.GetPostByID(this.ForumPostID);
                    if (forumPost != null)
                    {
                        string topicUrl = SEOHelper.GetForumTopicURL(forumPost.TopicID);
                        Response.Redirect(topicUrl);
                    }
                    else
                    {
                        Response.Redirect(SEOHelper.GetForumMainURL());
                    }
                }
            }
            catch (Exception exc)
            {
                pnlError.Visible = true;
                lErrorMessage.Text = Server.HtmlEncode(exc.Message);
            }
        }

        [DefaultValue(true)]
        public bool AddTopic
        {
            get
            {
                object obj2 = this.ViewState["AddTopic"];
                return ((obj2 != null) && ((bool)obj2));
            }
            set
            {
                this.ViewState["AddTopic"] = value;
            }
        }

        public bool EditTopic
        {
            get
            {
                object obj2 = this.ViewState["EditTopic"];
                return ((obj2 != null) && ((bool)obj2));
            }
            set
            {
                this.ViewState["EditTopic"] = value;
            }
        }

        public bool AddPost
        {
            get
            {
                object obj2 = this.ViewState["AddPost"];
                return ((obj2 != null) && ((bool)obj2));
            }
            set
            {
                this.ViewState["AddPost"] = value;
            }
        }

        public bool EditPost
        {
            get
            {
                object obj2 = this.ViewState["EditPost"];
                return ((obj2 != null) && ((bool)obj2));
            }
            set
            {
                this.ViewState["EditPost"] = value;
            }
        }

        public int ForumPostID
        {
            get
            {
                return CommonHelper.QueryStringInt("PostID");
            }
        }

        public int ForumTopicID
        {
            get
            {
                return CommonHelper.QueryStringInt("TopicID");
            }
        }

        public int ForumID
        {
            get
            {
                return CommonHelper.QueryStringInt("ForumID");
            }
        }
    }
}
