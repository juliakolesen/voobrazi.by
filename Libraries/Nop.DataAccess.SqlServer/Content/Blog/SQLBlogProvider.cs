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
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Configuration.Provider;
using System.Collections.Specialized;

namespace NopSolutions.NopCommerce.DataAccess.Content.Blog
{
    /// <summary>
    /// Blog provider for SQL Server
    /// </summary>
    public partial class SQLBlogProvider : DBBlogProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBBlogPost GetBlogPostFromReader(IDataReader dataReader)
        {
            DBBlogPost blogPost = new DBBlogPost();
            blogPost.BlogPostID = NopSqlDataHelper.GetInt(dataReader, "BlogPostID");
            blogPost.LanguageID = NopSqlDataHelper.GetInt(dataReader, "LanguageID");
            blogPost.BlogPostTitle = NopSqlDataHelper.GetString(dataReader, "BlogPostTitle");
            blogPost.BlogPostBody = NopSqlDataHelper.GetString(dataReader, "BlogPostBody");
            blogPost.BlogPostAllowComments = NopSqlDataHelper.GetBoolean(dataReader, "BlogPostAllowComments");
            blogPost.CreatedByID = NopSqlDataHelper.GetInt(dataReader, "CreatedByID");
            blogPost.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            return blogPost;
        }

        private DBBlogComment GetBlogCommentFromReader(IDataReader dataReader)
        {
            DBBlogComment blogComment = new DBBlogComment();
            blogComment.BlogCommentID = NopSqlDataHelper.GetInt(dataReader, "BlogCommentID");
            blogComment.BlogPostID = NopSqlDataHelper.GetInt(dataReader, "BlogPostID");
            blogComment.CustomerID = NopSqlDataHelper.GetInt(dataReader, "CustomerID");
            blogComment.CommentText = NopSqlDataHelper.GetString(dataReader, "CommentText");
            blogComment.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            return blogComment;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Initializes the provider with the property values specified in the application's configuration file. This method is not intended to be used directly from your code
        /// </summary>
        /// <param name="name">The name of the provider instance to initialize</param>
        /// <param name="config">A NameValueCollection that contains the names and values of configuration options for the provider.</param>
        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            base.Initialize(name, config);

            string connectionStringName = config["connectionStringName"];
            if (String.IsNullOrEmpty(connectionStringName))
                throw new ProviderException("Connection name not specified");
            this._sqlConnectionString = NopSqlDataHelper.GetConnectionString(connectionStringName);
            if ((this._sqlConnectionString == null) || (this._sqlConnectionString.Length < 1))
            {
                throw new ProviderException(string.Format("Connection string not found. {0}", connectionStringName));
            }
            config.Remove("connectionStringName");

            if (config.Count > 0)
            {
                string key = config.GetKey(0);
                if (!string.IsNullOrEmpty(key))
                {
                    throw new ProviderException(string.Format("Provider unrecognized attribute. {0}", new object[] { key }));
                }
            }
        }

        /// <summary>
        /// Deletes an blog post
        /// </summary>
        /// <param name="BlogPostID">Blog post identifier</param>
        public override void DeleteBlogPost(int BlogPostID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BlogPostDelete");
            db.AddInParameter(dbCommand, "BlogPostID", DbType.Int32, BlogPostID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets an blog post
        /// </summary>
        /// <param name="BlogPostID">Blog post identifier</param>
        /// <returns>Blog post</returns>
        public override DBBlogPost GetBlogPostByID(int BlogPostID)
        {
            DBBlogPost blogPost = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BlogPostLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "BlogPostID", DbType.Int32, BlogPostID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    blogPost = GetBlogPostFromReader(dataReader);
                }
            }
            return blogPost;
        }

        /// <summary>
        /// Gets all blog posts
        /// </summary>
        /// <param name="LanguageID">Language identifier. 0 if you want to get all news</param>
        /// <returns>Blog posts</returns>
        public override DBBlogPostCollection GetAllBlogPosts(int LanguageID)
        {
            DBBlogPostCollection blogPostCollection = new DBBlogPostCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BlogPostLoadAll");
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBBlogPost blogPost = GetBlogPostFromReader(dataReader);
                    blogPostCollection.Add(blogPost);
                }
            }

            return blogPostCollection;
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
        public override DBBlogPost InsertBlogPost(int LanguageID, string BlogPostTitle, string BlogPostBody,
            bool BlogPostAllowComments, int CreatedByID, DateTime CreatedOn)
        {
            DBBlogPost blogPost = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BlogPostInsert");
            db.AddOutParameter(dbCommand, "BlogPostID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
            db.AddInParameter(dbCommand, "BlogPostTitle", DbType.String, BlogPostTitle);
            db.AddInParameter(dbCommand, "BlogPostBody", DbType.String, BlogPostBody);
            db.AddInParameter(dbCommand, "BlogPostAllowComments", DbType.Boolean, BlogPostAllowComments);
            db.AddInParameter(dbCommand, "CreatedByID", DbType.Int32, CreatedByID);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int BlogPostID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@BlogPostID"));
                blogPost = GetBlogPostByID(BlogPostID);
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
        public override DBBlogPost UpdateBlogPost(int BlogPostID, int LanguageID, string BlogPostTitle, string BlogPostBody,
            bool BlogPostAllowComments, int CreatedByID, DateTime CreatedOn)
        {
            DBBlogPost blogPost = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BlogPostUpdate");
            db.AddInParameter(dbCommand, "BlogPostID", DbType.Int32, BlogPostID);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
            db.AddInParameter(dbCommand, "BlogPostTitle", DbType.String, BlogPostTitle);
            db.AddInParameter(dbCommand, "BlogPostBody", DbType.String, BlogPostBody);
            db.AddInParameter(dbCommand, "BlogPostAllowComments", DbType.Boolean, BlogPostAllowComments);
            db.AddInParameter(dbCommand, "CreatedByID", DbType.Int32, CreatedByID);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                blogPost = GetBlogPostByID(BlogPostID);

            return blogPost;
        }

        /// <summary>
        /// Deletes an blog comment
        /// </summary>
        /// <param name="BlogCommentID">Blog comment identifier</param>
        public override void DeleteBlogComment(int BlogCommentID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BlogCommentDelete");
            db.AddInParameter(dbCommand, "BlogCommentID", DbType.Int32, BlogCommentID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets an blog comment
        /// </summary>
        /// <param name="BlogCommentID">Blog comment identifier</param>
        /// <returns>An blog comment</returns>
        public override DBBlogComment GetBlogCommentByID(int BlogCommentID)
        {
            DBBlogComment blogComment = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BlogCommentLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "BlogCommentID", DbType.Int32, BlogCommentID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    blogComment = GetBlogCommentFromReader(dataReader);
                }
            }
            return blogComment;
        }

        /// <summary>
        /// Gets a collection of blog comments by blog post identifier
        /// </summary>
        /// <param name="BlogPostID">Blog post identifier</param>
        /// <returns>A collection of blog comments</returns>
        public override DBBlogCommentCollection GetBlogCommentsByBlogPostID(int BlogPostID)
        {
            DBBlogCommentCollection blogCommentCollection = new DBBlogCommentCollection();
            if (BlogPostID == 0)
                return blogCommentCollection;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BlogCommentLoadByBlogPostID");
            db.AddInParameter(dbCommand, "BlogPostID", DbType.Int32, BlogPostID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBBlogComment blogComment = GetBlogCommentFromReader(dataReader);
                    blogCommentCollection.Add(blogComment);
                }
            }
            return blogCommentCollection;
        }

        /// <summary>
        /// Gets all blog comments
        /// </summary>
        /// <returns>Blog comments</returns>
        public override DBBlogCommentCollection GetAllBlogComments()
        {
            DBBlogCommentCollection blogCommentCollection = new DBBlogCommentCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BlogCommentLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBBlogComment blogComment = GetBlogCommentFromReader(dataReader);
                    blogCommentCollection.Add(blogComment);
                }
            }

            return blogCommentCollection;
        }

        /// <summary>
        /// Inserts an blog comment
        /// </summary>
        /// <param name="BlogPostID">The blog post identifier</param>
        /// <param name="CustomerID">The customer identifier who commented the blog post</param>
        /// <param name="CommentText">The comment text</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>Blog comment</returns>
        public override DBBlogComment InsertBlogComment(int BlogPostID,
            int CustomerID, string CommentText, DateTime CreatedOn)
        {
            DBBlogComment blogComment = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BlogCommentInsert");
            db.AddOutParameter(dbCommand, "BlogCommentID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "BlogPostID", DbType.Int32, BlogPostID);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, CustomerID);
            db.AddInParameter(dbCommand, "CommentText", DbType.String, CommentText);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int BlogCommentID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@BlogCommentID"));
                blogComment = GetBlogCommentByID(BlogCommentID);
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
        public override DBBlogComment UpdateBlogComment(int BlogCommentID, int BlogPostID,
            int CustomerID, string CommentText, DateTime CreatedOn)
        {
            DBBlogComment blogComment = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BlogCommentUpdate");
            db.AddInParameter(dbCommand, "BlogCommentID", DbType.Int32, BlogCommentID);
            db.AddInParameter(dbCommand, "BlogPostID", DbType.Int32, BlogPostID);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, CustomerID);
            db.AddInParameter(dbCommand, "CommentText", DbType.String, CommentText);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                blogComment = GetBlogCommentByID(BlogCommentID);

            return blogComment;
        }
        #endregion
    }
}
