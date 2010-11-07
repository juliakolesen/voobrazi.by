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
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Web;
using NopSolutions.NopCommerce.BusinessLogic.Caching;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Localization;
using NopSolutions.NopCommerce.BusinessLogic.Messages;
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.BusinessLogic.Utils.Html;
using NopSolutions.NopCommerce.Common.Utils.Html;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Content.Blog;

namespace NopSolutions.NopCommerce.BusinessLogic.Content.Blog
{
    /// <summary>
    /// Blog post manager
    /// </summary>
    public partial class BlogManager
    {
        #region Constants
        private const string BLOGPOST_BY_ID_KEY = "Nop.blogpost.id-{0}";
        private const string BLOGPOST_PATTERN_KEY = "Nop.blogpost.";
        #endregion

        #region Utilities
        private static BlogPostCollection DBMapping(DBBlogPostCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            BlogPostCollection collection = new BlogPostCollection();
            foreach (DBBlogPost dbItem in dbCollection)
            {
                BlogPost item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static BlogPost DBMapping(DBBlogPost dbItem)
        {
            if (dbItem == null)
                return null;

            BlogPost item = new BlogPost();
            item.BlogPostID = dbItem.BlogPostID;
            item.LanguageID = dbItem.LanguageID;
            item.BlogPostTitle = dbItem.BlogPostTitle;
            item.BlogPostBody = dbItem.BlogPostBody;
            item.BlogPostAllowComments = dbItem.BlogPostAllowComments;
            item.CreatedByID = dbItem.CreatedByID;
            item.CreatedOn = dbItem.CreatedOn;

            return item;
        }

        private static BlogCommentCollection DBMapping(DBBlogCommentCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            BlogCommentCollection collection = new BlogCommentCollection();
            foreach (DBBlogComment dbItem in dbCollection)
            {
                BlogComment item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static BlogComment DBMapping(DBBlogComment dbItem)
        {
            if (dbItem == null)
                return null;

            BlogComment item = new BlogComment();
            item.BlogCommentID = dbItem.BlogCommentID;
            item.BlogPostID = dbItem.BlogPostID;
            item.CustomerID = dbItem.CustomerID;
            item.CommentText = dbItem.CommentText;
            item.CreatedOn = dbItem.CreatedOn;

            return item;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Deletes an blog post
        /// </summary>
        /// <param name="BlogPostID">Blog post identifier</param>
        public static void DeleteBlogPost(int BlogPostID)
        {
            DBProviderManager<DBBlogProvider>.Provider.DeleteBlogPost(BlogPostID);
            if (BlogManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(BLOGPOST_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets an blog post
        /// </summary>
        /// <param name="BlogPostID">Blog post identifier</param>
        /// <returns>Blog post</returns>
        public static BlogPost GetBlogPostByID(int BlogPostID)
        {
            if (BlogPostID == 0)
                return null;

            string key = string.Format(BLOGPOST_BY_ID_KEY, BlogPostID);
            object obj2 = NopCache.Get(key);
            if (BlogManager.CacheEnabled && (obj2 != null))
            {
                return (BlogPost)obj2;
            }

            DBBlogPost dbItem = DBProviderManager<DBBlogProvider>.Provider.GetBlogPostByID(BlogPostID);
            BlogPost blogPost = DBMapping(dbItem);

            if (BlogManager.CacheEnabled)
            {
                NopCache.Max(key, blogPost);
            }
            return blogPost;
        }

        /// <summary>
        /// Gets all blog posts
        /// </summary>
        /// <param name="LanguageID">Language identifier. 0 if you want to get all news</param>
        /// <returns>Blog posts</returns>
        public static BlogPostCollection GetAllBlogPosts(int LanguageID)
        {
            DBBlogPostCollection dbCollection = DBProviderManager<DBBlogProvider>.Provider.GetAllBlogPosts(LanguageID);
            BlogPostCollection collection = DBMapping(dbCollection);
            return collection;
        }

        /// <summary>
        /// Inserts an blog post
        /// </summary>
        /// <param name="LanguageID">The language identifier</param>
        /// <param name="BlogPostTitle">The blog post title</param>
        /// <param name="BlogPostBody">The blog post title</param>
        /// <param name="BlogPostAllowComments">A value indicating whether the blog post comments are allowed</param>
        /// <param name="CreatedByID">The user identifier who created the blog post</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>Blog post</returns>
        public static BlogPost InsertBlogPost(int LanguageID, string BlogPostTitle, string BlogPostBody,
            bool BlogPostAllowComments, int CreatedByID, DateTime CreatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);

            DBBlogPost dbItem = DBProviderManager<DBBlogProvider>.Provider.InsertBlogPost(LanguageID, BlogPostTitle, BlogPostBody,
                BlogPostAllowComments, CreatedByID, CreatedOn);
            BlogPost blogPost = DBMapping(dbItem);

            if (BlogManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(BLOGPOST_PATTERN_KEY);
            }

            return blogPost;
        }

        /// <summary>
        /// Updates the blog post
        /// </summary>
        /// <param name="LanguageID">The language identifier</param>
        /// <param name="BlogPostID">Blog post identifier</param>
        /// <param name="BlogPostTitle">The blog post title</param>
        /// <param name="BlogPostBody">The blog post title</param>
        /// <param name="BlogPostAllowComments">A value indicating whether the blog post comments are allowed</param>
        /// <param name="CreatedByID">The user identifier who created the blog post</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>Blog post</returns>
        public static BlogPost UpdateBlogPost(int BlogPostID,int LanguageID, string BlogPostTitle, string BlogPostBody,
            bool BlogPostAllowComments, int CreatedByID, DateTime CreatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);

            DBBlogPost dbItem = DBProviderManager<DBBlogProvider>.Provider.UpdateBlogPost(BlogPostID, LanguageID, BlogPostTitle, BlogPostBody,
                BlogPostAllowComments, CreatedByID, CreatedOn);
            BlogPost blogPost = DBMapping(dbItem);

            if (BlogManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(BLOGPOST_PATTERN_KEY);
            }

            return blogPost;
        }

        /// <summary>
        /// Deletes an blog comment
        /// </summary>
        /// <param name="BlogCommentID">Blog comment identifier</param>
        public static void DeleteBlogComment(int BlogCommentID)
        {
            DBProviderManager<DBBlogProvider>.Provider.DeleteBlogComment(BlogCommentID);
        }

        /// <summary>
        /// Gets an blog comment
        /// </summary>
        /// <param name="BlogCommentID">Blog comment identifier</param>
        /// <returns>An blog comment</returns>
        public static BlogComment GetBlogCommentByID(int BlogCommentID)
        {
            if (BlogCommentID == 0)
                return null;

            DBBlogComment dbItem = DBProviderManager<DBBlogProvider>.Provider.GetBlogCommentByID(BlogCommentID);
            BlogComment blogComment = DBMapping(dbItem);
            return blogComment;
        }

        /// <summary>
        /// Gets a collection of blog comments by blog post identifier
        /// </summary>
        /// <param name="BlogPostID">Blog post identifier</param>
        /// <returns>A collection of blog comments</returns>
        public static BlogCommentCollection GetBlogCommentsByBlogPostID(int BlogPostID)
        {
            DBBlogCommentCollection dbCollection = DBProviderManager<DBBlogProvider>.Provider.GetBlogCommentsByBlogPostID(BlogPostID);
            BlogCommentCollection collection = DBMapping(dbCollection);
            return collection;
        }

        /// <summary>
        /// Gets all blog comments
        /// </summary>
        /// <returns>Blog comments</returns>
        public static BlogCommentCollection GetAllBlogComments()
        {
            DBBlogCommentCollection dbCollection = DBProviderManager<DBBlogProvider>.Provider.GetAllBlogComments();
            BlogCommentCollection collection = DBMapping(dbCollection);
            return collection;
        }

        /// <summary>
        /// Inserts an blog comment
        /// </summary>
        /// <param name="BlogPostID">The blog post identifier</param>
        /// <param name="CustomerID">The customer identifier who commented the blog post</param>
        /// <param name="CommentText">The comment text</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>Blog comment</returns>
        public static BlogComment InsertBlogComment(int BlogPostID,
            int CustomerID, string CommentText, DateTime CreatedOn)
        {
            return InsertBlogComment(BlogPostID, CustomerID, CommentText, CreatedOn, BlogManager.NotifyAboutNewBlogComments);
        }

        /// <summary>
        /// Inserts an blog comment
        /// </summary>
        /// <param name="BlogPostID">The blog post identifier</param>
        /// <param name="CustomerID">The customer identifier who commented the blog post</param>
        /// <param name="CommentText">The comment text</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="notify">A value indicating whether to notify the store owner</param>
        /// <returns>Blog comment</returns>
        public static BlogComment InsertBlogComment(int BlogPostID,
            int CustomerID, string CommentText, DateTime CreatedOn, bool notify)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);

            DBBlogComment dbItem = DBProviderManager<DBBlogProvider>.Provider.InsertBlogComment(BlogPostID,
                CustomerID, CommentText, CreatedOn);
            BlogComment blogComment = DBMapping(dbItem);

            if (notify)
            {
                MessageManager.SendBlogCommentNotificationMessage(blogComment, LocalizationManager.DefaultAdminLanguage.LanguageID);
            }

            return blogComment;
        }

        /// <summary>
        /// Updates the blog comment
        /// </summary>
        /// <param name="BlogCommentID">The blog comment identifier</param>
        /// <param name="BlogPostID">The blog post identifier</param>
        /// <param name="CustomerID">The customer identifier who commented the blog post</param>
        /// <param name="CommentText">The comment text</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>Blog comment</returns>
        public static BlogComment UpdateBlogComment(int BlogCommentID, int BlogPostID,
            int CustomerID, string CommentText, DateTime CreatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);

            DBBlogComment dbItem = DBProviderManager<DBBlogProvider>.Provider.UpdateBlogComment(BlogCommentID, BlogPostID,
                CustomerID, CommentText, CreatedOn);
            BlogComment blogComment = DBMapping(dbItem);
            return blogComment;
        }
        
        /// <summary>
        /// Formats the text
        /// </summary>
        /// <param name="Text">Text</param>
        /// <returns>Formatted text</returns>
        public static string FormatCommentText(string Text)
        {
            if (String.IsNullOrEmpty(Text))
                return string.Empty;

            Text = HtmlHelper.FormatText(Text, false, true, false, false, false, false);
            return Text;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets a value indicating whether cache is enabled
        /// </summary>
        public static bool CacheEnabled
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("Cache.BlogManager.CacheEnabled");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether blog is enabled
        /// </summary>
        public static bool BlogEnabled
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("Common.EnableBlog");
            }
            set
            {
                SettingManager.SetParam("Common.EnableBlog", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether not registered user can leave comments
        /// </summary>
        public static bool AllowNotRegisteredUsersToLeaveComments
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("Blog.AllowNotRegisteredUsersToLeaveComments");
            }
            set
            {
                SettingManager.SetParam("Blog.AllowNotRegisteredUsersToLeaveComments", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to notify about new blog comments
        /// </summary>
        public static bool NotifyAboutNewBlogComments
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("Blog.NotifyAboutNewBlogComments");
            }
            set
            {
                SettingManager.SetParam("Blog.NotifyAboutNewBlogComments", value.ToString());
            }
        }
        #endregion
    }
}
