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
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Web.Configuration;
using System.Web.Hosting;


namespace NopSolutions.NopCommerce.DataAccess.Content.Blog
{
    /// <summary>
    /// Acts as a base class for deriving custom log provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/BlogProvider")]
    public abstract partial class DBBlogProvider : BaseDBProvider
    {
        #region Methods
        /// <summary>
        /// Deletes an blog post
        /// </summary>
        /// <param name="BlogPostID">Blog post identifier</param>
        public abstract void DeleteBlogPost(int BlogPostID);
        
        /// <summary>
        /// Gets an blog post
        /// </summary>
        /// <param name="BlogPostID">Blog post identifier</param>
        /// <returns>Blog post</returns>
        public abstract DBBlogPost GetBlogPostByID(int BlogPostID);

        /// <summary>
        /// Gets all blog posts
        /// </summary>
        /// <param name="LanguageID">Language identifier. 0 if you want to get all news</param>
        /// <returns>Blog posts</returns>
        public abstract DBBlogPostCollection GetAllBlogPosts(int LanguageID);

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
        public abstract DBBlogPost InsertBlogPost(int LanguageID, string BlogPostTitle, string BlogPostBody,
            bool BlogPostAllowComments, int CreatedByID, DateTime CreatedOn);

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
        public abstract DBBlogPost UpdateBlogPost(int BlogPostID, int LanguageID, string BlogPostTitle, string BlogPostBody,
            bool BlogPostAllowComments, int CreatedByID, DateTime CreatedOn);

        /// <summary>
        /// Deletes an blog comment
        /// </summary>
        /// <param name="BlogCommentID">Blog comment identifier</param>
        public abstract void DeleteBlogComment(int BlogCommentID);

        /// <summary>
        /// Gets an blog comment
        /// </summary>
        /// <param name="BlogCommentID">Blog comment identifier</param>
        /// <returns>An blog comment</returns>
        public abstract DBBlogComment GetBlogCommentByID(int BlogCommentID);

        /// <summary>
        /// Gets a collection of blog comments by blog post identifier
        /// </summary>
        /// <param name="BlogPostID">Blog post identifier</param>
        /// <returns>A collection of blog comments</returns>
        public abstract DBBlogCommentCollection GetBlogCommentsByBlogPostID(int BlogPostID);

        /// <summary>
        /// Gets all blog comments
        /// </summary>
        /// <returns>Blog comments</returns>
        public abstract DBBlogCommentCollection GetAllBlogComments();

        /// <summary>
        /// Inserts an blog comment
        /// </summary>
        /// <param name="BlogPostID">The blog post identifier</param>
        /// <param name="CustomerID">The customer identifier who commented the blog post</param>
        /// <param name="CommentText">The comment text</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>Blog comment</returns>
        public abstract DBBlogComment InsertBlogComment(int BlogPostID,
            int CustomerID, string CommentText, DateTime CreatedOn);

        /// <summary>
        /// Updates the blog comment
        /// </summary>
        /// <param name="BlogCommentID">The blog comment identifier</param>
        /// <param name="BlogPostID">The blog post identifier</param>
        /// <param name="CustomerID">The customer identifier who commented the blog post</param>
        /// <param name="CommentText">The comment text</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>Blog comment</returns>
        public abstract DBBlogComment UpdateBlogComment(int BlogCommentID, int BlogPostID,
            int CustomerID, string CommentText, DateTime CreatedOn);
        #endregion
    }
}
