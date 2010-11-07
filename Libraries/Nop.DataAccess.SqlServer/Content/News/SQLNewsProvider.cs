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
using System.Configuration.Provider;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace NopSolutions.NopCommerce.DataAccess.Content.NewsManagement
{
    /// <summary>
    /// News provider for SQL Server
    /// </summary>
    public partial class SQLNewsProvider : DBNewsProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBNews GetNewsFromReader(IDataReader dataReader)
        {
            DBNews news = new DBNews();
            news.NewsID = NopSqlDataHelper.GetInt(dataReader, "NewsID");
            news.LanguageID = NopSqlDataHelper.GetInt(dataReader, "LanguageID");
            news.Title = NopSqlDataHelper.GetString(dataReader, "Title");
            news.Short = NopSqlDataHelper.GetString(dataReader, "Short");
            news.Full = NopSqlDataHelper.GetString(dataReader, "Full");
            news.Published = NopSqlDataHelper.GetBoolean(dataReader, "Published");
            news.AllowComments = NopSqlDataHelper.GetBoolean(dataReader, "AllowComments");
            news.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            return news;
        }

        private DBNewsComment GetNewsCommentFromReader(IDataReader dataReader)
        {
            DBNewsComment newsComment = new DBNewsComment();
            newsComment.NewsCommentID = NopSqlDataHelper.GetInt(dataReader, "NewsCommentID");
            newsComment.NewsID = NopSqlDataHelper.GetInt(dataReader, "NewsID");
            newsComment.CustomerID = NopSqlDataHelper.GetInt(dataReader, "CustomerID");
            newsComment.Title = NopSqlDataHelper.GetString(dataReader, "Title");
            newsComment.Comment = NopSqlDataHelper.GetString(dataReader, "Comment");
            newsComment.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            return newsComment;
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
        /// Gets a news
        /// </summary>
        /// <param name="NewsID">The news identifier</param>
        /// <returns>News</returns>
        public override DBNews GetNewsByID(int NewsID)
        {
            DBNews news = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_NewsLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "NewsID", DbType.Int32, NewsID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    news = GetNewsFromReader(dataReader);
                }
            }
            return news;
        }

        /// <summary>
        /// Deletes a news
        /// </summary>
        /// <param name="NewsID">The news identifier</param>
        public override void DeleteNews(int NewsID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_NewsDelete");
            db.AddInParameter(dbCommand, "NewsID", DbType.Int32, NewsID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets news item collection
        /// </summary>
        /// <param name="LanguageID">Language identifier. 0 if you want to get all news</param>
        /// <param name="NewsCount">News item count. 0 if you want to get all news</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>News item collection</returns>
        public override DBNewsCollection GetNews(int LanguageID, int NewsCount, bool showHidden)
        {
            DBNewsCollection newsCollection = new DBNewsCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_NewsLoadAll");
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
            db.AddInParameter(dbCommand, "NewsCount", DbType.Int32, NewsCount);
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBNews news = GetNewsFromReader(dataReader);
                    newsCollection.Add(news);
                }
            }

            return newsCollection;
        }

        /// <summary>
        /// Inserts a news item
        /// </summary>
        /// <param name="LanguageID">The language identifier</param>
        /// <param name="Title">The news title</param>
        /// <param name="Short">The short text</param>
        /// <param name="Full">The full text</param>
        /// <param name="Published">A value indicating whether the entity is published</param>
        /// <param name="AllowComments">A value indicating whether the entity allows comments</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>News item</returns>
        public override DBNews InsertNews(int LanguageID, string Title, string Short,
            string Full, bool Published, bool AllowComments, DateTime CreatedOn)
        {
            DBNews news = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_NewsInsert");
            db.AddOutParameter(dbCommand, "NewsID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
            db.AddInParameter(dbCommand, "Title", DbType.String, Title);
            db.AddInParameter(dbCommand, "Short", DbType.String, Short);
            db.AddInParameter(dbCommand, "Full", DbType.String, Full);
            db.AddInParameter(dbCommand, "Published", DbType.Boolean, Published);
            db.AddInParameter(dbCommand, "AllowComments", DbType.Boolean, AllowComments);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int NewsID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@NewsID"));
                news = GetNewsByID(NewsID);
            }
            return news;
        }

        /// <summary>
        /// Updates the news item
        /// </summary>
        /// <param name="NewsID">The news identifier</param>
        /// <param name="LanguageID">The language identifier</param>
        /// <param name="Title">The news title</param>
        /// <param name="Short">The short text</param>
        /// <param name="Full">The full text</param>
        /// <param name="Published">A value indicating whether the entity is published</param>
        /// <param name="AllowComments">A value indicating whether the entity allows comments</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>News item</returns>
        public override DBNews UpdateNews(int NewsID, int LanguageID, string Title, string Short,
            string Full, bool Published, bool AllowComments, DateTime CreatedOn)
        {
            DBNews news = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_NewsUpdate");
            db.AddInParameter(dbCommand, "NewsID", DbType.Int32, NewsID);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
            db.AddInParameter(dbCommand, "Title", DbType.String, Title);
            db.AddInParameter(dbCommand, "Short", DbType.String, Short);
            db.AddInParameter(dbCommand, "Full", DbType.String, Full);
            db.AddInParameter(dbCommand, "Published", DbType.Boolean, Published);
            db.AddInParameter(dbCommand, "AllowComments", DbType.Boolean, AllowComments);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                news = GetNewsByID(NewsID);

            return news;
        }

        /// <summary>
        /// Gets a news comment
        /// </summary>
        /// <param name="NewsCommentID">News comment identifer</param>
        /// <returns>News comment</returns>
        public override DBNewsComment GetNewsCommentByID(int NewsCommentID)
        {
            DBNewsComment newsComment = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_NewsCommentLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "NewsCommentID", DbType.Int32, NewsCommentID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    newsComment = GetNewsCommentFromReader(dataReader);
                }
            }
            return newsComment;
        }

        /// <summary>
        /// Gets a news comment collection by news identifier
        /// </summary>
        /// <param name="NewsID">The news identifier</param>
        /// <returns>News comment collection</returns>
        public override DBNewsCommentCollection GetNewsCommentsByNewsID(int NewsID)
        {
            DBNewsCommentCollection newsCommentCollection = new DBNewsCommentCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_NewsCommentLoadByNewsID");
            db.AddInParameter(dbCommand, "NewsID", DbType.Int32, NewsID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBNewsComment newsComment = GetNewsCommentFromReader(dataReader);
                    newsCommentCollection.Add(newsComment);
                }
            }
            return newsCommentCollection;
        }

        /// <summary>
        /// Deletes a news comment
        /// </summary>
        /// <param name="NewsCommentID">The news comment identifier</param>
        public override void DeleteNewsComment(int NewsCommentID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_NewsCommentDelete");
            db.AddInParameter(dbCommand, "NewsCommentID", DbType.Int32, NewsCommentID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets all news comments
        /// </summary>
        /// <returns>News comment collection</returns>
        public override DBNewsCommentCollection GetAllNewsComments()
        {
            DBNewsCommentCollection newsCommentCollection = new DBNewsCommentCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_NewsCommentLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBNewsComment newsComment = GetNewsCommentFromReader(dataReader);
                    newsCommentCollection.Add(newsComment);
                }
            }

            return newsCommentCollection;
        }

        /// <summary>
        /// Inserts a news comment
        /// </summary>
        /// <param name="NewsID">The news identifier</param>
        /// <param name="CustomerID">The customer identifier</param>
        /// <param name="Title">The title</param>
        /// <param name="Comment">The comment</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>News comment</returns>
        public override DBNewsComment InsertNewsComment(int NewsID, int CustomerID, string Title,
            string Comment, DateTime CreatedOn)
        {
            DBNewsComment newsComment = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_NewsCommentInsert");
            db.AddOutParameter(dbCommand, "NewsCommentID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "NewsID", DbType.Int32, NewsID);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, CustomerID);
            db.AddInParameter(dbCommand, "Title", DbType.String, Title);
            db.AddInParameter(dbCommand, "Comment", DbType.String, Comment);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int NewsCommentID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@NewsCommentID"));
                newsComment = GetNewsCommentByID(NewsCommentID);
            }
            return newsComment;
        }

        /// <summary>
        /// Updates the news comment
        /// </summary>
        /// <param name="NewsCommentID">The news comment identifier</param>
        /// <param name="NewsID">The news identifier</param>
        /// <param name="CustomerID">The customer identifier</param>
        /// <param name="Title">The title</param>
        /// <param name="Comment">The comment</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>News comment</returns>
        public override DBNewsComment UpdateNewsComment(int NewsCommentID, int NewsID, int CustomerID, string Title,
            string Comment, DateTime CreatedOn)
        {
            DBNewsComment newsComment = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_NewsCommentUpdate");
            db.AddInParameter(dbCommand, "NewsCommentID", DbType.Int32, NewsCommentID);
            db.AddInParameter(dbCommand, "NewsID", DbType.Int32, NewsID);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, CustomerID);
            db.AddInParameter(dbCommand, "Title", DbType.String, Title);
            db.AddInParameter(dbCommand, "Comment", DbType.String, Comment);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);

            if (db.ExecuteNonQuery(dbCommand) > 0)
                newsComment = GetNewsCommentByID(NewsCommentID);

            return newsComment;
        }
        #endregion
    }
}
