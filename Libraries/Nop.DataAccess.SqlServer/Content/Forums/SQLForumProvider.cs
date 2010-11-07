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

namespace NopSolutions.NopCommerce.DataAccess.Content.Forums
{
    /// <summary>
    /// Forum provider for SQL Server
    /// </summary>
    public partial class SQLForumProvider : DBForumProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBForumGroup GetForumGroupFromReader(IDataReader dataReader)
        {
            DBForumGroup forumGroup = new DBForumGroup();
            forumGroup.ForumGroupID = NopSqlDataHelper.GetInt(dataReader, "ForumGroupID");
            forumGroup.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            forumGroup.Description = NopSqlDataHelper.GetString(dataReader, "Description");
            forumGroup.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            forumGroup.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            forumGroup.UpdatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "UpdatedOn");
            return forumGroup;
        }

        private DBForum GetForumFromReader(IDataReader dataReader)
        {
            DBForum forum = new DBForum();
            forum.ForumID = NopSqlDataHelper.GetInt(dataReader, "ForumID");
            forum.ForumGroupID = NopSqlDataHelper.GetInt(dataReader, "ForumGroupID");
            forum.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            forum.Description = NopSqlDataHelper.GetString(dataReader, "Description");
            forum.NumTopics = NopSqlDataHelper.GetInt(dataReader, "NumTopics");
            forum.NumPosts = NopSqlDataHelper.GetInt(dataReader, "NumPosts");
            forum.LastTopicID = NopSqlDataHelper.GetInt(dataReader, "LastTopicID");
            forum.LastPostID = NopSqlDataHelper.GetInt(dataReader, "LastPostID");
            forum.LastPostUserID = NopSqlDataHelper.GetInt(dataReader, "LastPostUserID");
            forum.LastPostTime = NopSqlDataHelper.GetNullableUtcDateTime(dataReader, "LastPostTime");
            forum.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            forum.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            forum.UpdatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "UpdatedOn");
            return forum;
        }

        private DBForumTopic GetForumTopicFromReader(IDataReader dataReader)
        {
            DBForumTopic forumTopic = new DBForumTopic();
            forumTopic.ForumTopicID = NopSqlDataHelper.GetInt(dataReader, "TopicID");
            forumTopic.ForumID = NopSqlDataHelper.GetInt(dataReader, "ForumID");
            forumTopic.UserID = NopSqlDataHelper.GetInt(dataReader, "UserID");
            forumTopic.TopicTypeID = NopSqlDataHelper.GetInt(dataReader, "TopicTypeID");
            forumTopic.Subject = NopSqlDataHelper.GetString(dataReader, "Subject");
            forumTopic.NumPosts = NopSqlDataHelper.GetInt(dataReader, "NumPosts");
            forumTopic.Views = NopSqlDataHelper.GetInt(dataReader, "Views");
            forumTopic.LastPostID = NopSqlDataHelper.GetInt(dataReader, "LastPostID");
            forumTopic.LastPostUserID = NopSqlDataHelper.GetInt(dataReader, "LastPostUserID");
            forumTopic.LastPostTime = NopSqlDataHelper.GetNullableUtcDateTime(dataReader, "LastPostTime");
            forumTopic.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            forumTopic.UpdatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "UpdatedOn");
            return forumTopic;
        }

        private DBForumPost GetForumPostFromReader(IDataReader dataReader)
        {
            DBForumPost forumPost = new DBForumPost();
            forumPost.ForumPostID = NopSqlDataHelper.GetInt(dataReader, "PostID");
            forumPost.TopicID = NopSqlDataHelper.GetInt(dataReader, "TopicID");
            forumPost.UserID = NopSqlDataHelper.GetInt(dataReader, "UserID");
            forumPost.Text = NopSqlDataHelper.GetString(dataReader, "Text");
            forumPost.IPAddress = NopSqlDataHelper.GetString(dataReader, "IPAddress");
            forumPost.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            forumPost.UpdatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "UpdatedOn");
            return forumPost;
        }

        private DBPrivateMessage GetPrivateMessageFromReader(IDataReader dataReader)
        {
            DBPrivateMessage privateMessage = new DBPrivateMessage();
            privateMessage.PrivateMessageID = NopSqlDataHelper.GetInt(dataReader, "PrivateMessageID");
            privateMessage.FromUserID = NopSqlDataHelper.GetInt(dataReader, "FromUserID");
            privateMessage.ToUserID = NopSqlDataHelper.GetInt(dataReader, "ToUserID");
            privateMessage.Subject = NopSqlDataHelper.GetString(dataReader, "Subject");
            privateMessage.Text = NopSqlDataHelper.GetString(dataReader, "Text");
            privateMessage.IsRead = NopSqlDataHelper.GetBoolean(dataReader, "IsRead");
            privateMessage.IsDeletedByAuthor = NopSqlDataHelper.GetBoolean(dataReader, "IsDeletedByAuthor");
            privateMessage.IsDeletedByRecipient = NopSqlDataHelper.GetBoolean(dataReader, "IsDeletedByRecipient");
            privateMessage.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            return privateMessage;
        }

        private DBForumSubscription GetForumSubscriptionFromReader(IDataReader dataReader)
        {
            DBForumSubscription forumSubscription = new DBForumSubscription();
            forumSubscription.ForumSubscriptionID = NopSqlDataHelper.GetInt(dataReader, "SubscriptionID");
            forumSubscription.SubscriptionGUID = NopSqlDataHelper.GetGuid(dataReader, "SubscriptionGUID");
            forumSubscription.UserID = NopSqlDataHelper.GetInt(dataReader, "UserID");
            forumSubscription.ForumID = NopSqlDataHelper.GetInt(dataReader, "ForumID");
            forumSubscription.TopicID = NopSqlDataHelper.GetInt(dataReader, "TopicID");
            forumSubscription.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            return forumSubscription;
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
        /// Deletes a forum group
        /// </summary>
        /// <param name="ForumGroupID">The forum group identifier</param>
        public override void DeleteForumGroup(int ForumGroupID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_GroupDelete");
            db.AddInParameter(dbCommand, "ForumGroupID", DbType.Int32, ForumGroupID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a forum group
        /// </summary>
        /// <param name="ForumGroupID">The forum group identifier</param>
        /// <returns>Forum group</returns>
        public override DBForumGroup GetForumGroupByID(int ForumGroupID)
        {
            DBForumGroup forumGroup = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_GroupLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ForumGroupID", DbType.Int32, ForumGroupID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    forumGroup = GetForumGroupFromReader(dataReader);
                }
            }
            return forumGroup;
        }

        /// <summary>
        /// Gets all forum groups
        /// </summary>
        /// <returns>Forum groups</returns>
        public override DBForumGroupCollection GetAllForumGroups()
        {
            DBForumGroupCollection forumGroupCollection = new DBForumGroupCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_GroupLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBForumGroup forumGroup = GetForumGroupFromReader(dataReader);
                    forumGroupCollection.Add(forumGroup);
                }
            }

            return forumGroupCollection;
        }

        /// <summary>
        /// Inserts a forum group
        /// </summary>
        /// <param name="Name">The language name</param>
        /// <param name="Description">The description</param>
        /// <param name="DisplayOrder">The display order</param>        
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Forum group</returns>
        public override DBForumGroup InsertForumGroup(string Name, string Description,
            int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn)
        {
            DBForumGroup forumGroup = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_GroupInsert");
            db.AddOutParameter(dbCommand, "ForumGroupID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "Description", DbType.String, Description);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, UpdatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int ForumGroupID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ForumGroupID"));
                forumGroup = GetForumGroupByID(ForumGroupID);
            }
            return forumGroup;
        }

        /// <summary>
        /// Updates the forum group
        /// </summary>
        /// <param name="ForumGroupID">The forum group identifier</param>
        /// <param name="Name">The language name</param>
        /// <param name="Description">The description</param>
        /// <param name="DisplayOrder">The display order</param>        
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Forum group</returns>
        public override DBForumGroup UpdateForumGroup(int ForumGroupID, string Name, string Description,
            int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn)
        {
            DBForumGroup forumGroup = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_GroupUpdate");
            db.AddInParameter(dbCommand, "ForumGroupID", DbType.Int32, ForumGroupID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "Description", DbType.String, Description);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, UpdatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                forumGroup = GetForumGroupByID(ForumGroupID);

            return forumGroup;
        }

        /// <summary>
        /// Deletes a forum
        /// </summary>
        /// <param name="ForumID">The forum identifier</param>
        public override void DeleteForum(int ForumID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_ForumDelete");
            db.AddInParameter(dbCommand, "ForumID", DbType.Int32, ForumID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a forum
        /// </summary>
        /// <param name="ForumID">The forum identifier</param>
        /// <returns>Forum</returns>
        public override DBForum GetForumByID(int ForumID)
        {
            DBForum forum = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_ForumLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ForumID", DbType.Int32, ForumID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    forum = GetForumFromReader(dataReader);
                }
            }
            return forum;
        }

        /// <summary>
        /// Gets forums by group identifier
        /// </summary>
        /// <param name="ForumGroupID">The forum group identifier</param>
        /// <returns>Forums</returns>
        public override DBForumCollection GetAllForumsByGroupID(int ForumGroupID)
        {
            DBForumCollection forumCollection = new DBForumCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_ForumLoadAllByForumGroupID");
            db.AddInParameter(dbCommand, "ForumGroupID", DbType.Int32, ForumGroupID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBForum forum = GetForumFromReader(dataReader);
                    forumCollection.Add(forum);
                }
            }
            return forumCollection;
        }

        /// <summary>
        /// Inserts a forum
        /// </summary>
        /// <param name="ForumGroupID">The forum group identifier</param>
        /// <param name="Name">The language name</param>
        /// <param name="Description">The description</param>
        /// <param name="NumTopics">The number of topics</param>
        /// <param name="NumPosts">The number of posts</param>
        /// <param name="LastTopicID">The last topic identifier</param>
        /// <param name="LastPostID">The last post identifier</param>
        /// <param name="LastPostUserID">The last post user identifier</param>
        /// <param name="LastPostTime">The last post date and time</param>
        /// <param name="DisplayOrder">The display order</param>        
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Forum</returns>
        public override DBForum InsertForum(int ForumGroupID,
            string Name, string Description,
            int NumTopics, int NumPosts, int LastTopicID, int LastPostID,
            int LastPostUserID, DateTime? LastPostTime, int DisplayOrder,
            DateTime CreatedOn, DateTime UpdatedOn)
        {
            DBForum forum = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_ForumInsert");
            db.AddOutParameter(dbCommand, "ForumID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "ForumGroupID", DbType.Int32, ForumGroupID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "Description", DbType.String, Description);
            db.AddInParameter(dbCommand, "NumTopics", DbType.Int32, NumTopics);
            db.AddInParameter(dbCommand, "NumPosts", DbType.Int32, NumPosts);
            db.AddInParameter(dbCommand, "LastTopicID", DbType.Int32, LastTopicID);
            db.AddInParameter(dbCommand, "LastPostID", DbType.Int32, LastPostID);
            db.AddInParameter(dbCommand, "LastPostUserID", DbType.Int32, LastPostUserID);
            if (LastPostTime.HasValue)
                db.AddInParameter(dbCommand, "LastPostTime", DbType.DateTime, LastPostTime.Value);
            else
                db.AddInParameter(dbCommand, "LastPostTime", DbType.DateTime, null);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, UpdatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int ForumID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ForumID"));
                forum = GetForumByID(ForumID);
            }
            return forum;
        }

        /// <summary>
        /// Updates the forum
        /// </summary>
        /// <param name="ForumID">The forum identifier</param>
        /// <param name="ForumGroupID">The forum group identifier</param>
        /// <param name="Name">The language name</param>
        /// <param name="Description">The description</param>
        /// <param name="NumTopics">The number of topics</param>
        /// <param name="NumPosts">The number of posts</param>
        /// <param name="LastTopicID">The last topic identifier</param>
        /// <param name="LastPostID">The last post identifier</param>
        /// <param name="LastPostUserID">The last post user identifier</param>
        /// <param name="LastPostTime">The last post date and time</param>
        /// <param name="DisplayOrder">The display order</param>        
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Forum</returns>
        public override DBForum UpdateForum(int ForumID, int ForumGroupID,
            string Name, string Description,
            int NumTopics, int NumPosts, int LastTopicID, int LastPostID,
            int LastPostUserID, DateTime? LastPostTime, int DisplayOrder,
            DateTime CreatedOn, DateTime UpdatedOn)
        {
            DBForum forum = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_ForumUpdate");
            db.AddInParameter(dbCommand, "ForumID", DbType.Int32, ForumID);
            db.AddInParameter(dbCommand, "ForumGroupID", DbType.Int32, ForumGroupID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "Description", DbType.String, Description);
            db.AddInParameter(dbCommand, "NumTopics", DbType.Int32, NumTopics);
            db.AddInParameter(dbCommand, "NumPosts", DbType.Int32, NumPosts);
            db.AddInParameter(dbCommand, "LastTopicID", DbType.Int32, LastTopicID);
            db.AddInParameter(dbCommand, "LastPostID", DbType.Int32, LastPostID);
            db.AddInParameter(dbCommand, "LastPostUserID", DbType.Int32, LastPostUserID);
            if (LastPostTime.HasValue)
                db.AddInParameter(dbCommand, "LastPostTime", DbType.DateTime, LastPostTime.Value);
            else
                db.AddInParameter(dbCommand, "LastPostTime", DbType.DateTime, null);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, UpdatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                forum = GetForumByID(ForumID);

            return forum;
        }

        /// <summary>
        /// Gets a forum
        /// </summary>
        /// <param name="ForumID">The forum identifier</param>
        public override void UpdateForumStats(int ForumID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_ForumUpdateCounts");
            db.AddInParameter(dbCommand, "ForumID", DbType.Int32, ForumID);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Deletes a topic
        /// </summary>
        /// <param name="ForumTopicID">The topic identifier</param>
        public override void DeleteTopic(int ForumTopicID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_TopicDelete");
            db.AddInParameter(dbCommand, "TopicID", DbType.Int32, ForumTopicID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a topic
        /// </summary>
        /// <param name="ForumTopicID">The topic identifier</param>
        /// <param name="IncreaseViews">The value indicating whether to increase topic views</param>
        /// <returns>Topic</returns>
        public override DBForumTopic GetTopicByID(int ForumTopicID, bool IncreaseViews)
        {
            DBForumTopic forumTopic = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_TopicLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "TopicID", DbType.Int32, ForumTopicID);
            db.AddInParameter(dbCommand, "IncreaseViews", DbType.Boolean, IncreaseViews);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    forumTopic = GetForumTopicFromReader(dataReader);
                }
            }
            return forumTopic;
        }

        /// <summary>
        /// Gets all topics
        /// </summary>
        /// <param name="ForumID">The forum group identifier</param>
        /// <param name="UserID">The user identifier</param>
        /// <param name="Keywords">Keywords</param>
        /// <param name="SearchPosts">A value indicating whether to search in posts</param>
        /// <param name="PageSize">Page size</param>
        /// <param name="PageIndex">Page index</param>
        /// <param name="TotalRecords">Total records</param>
        /// <returns>Topics</returns>
        public override DBForumTopicCollection GetAllTopics(int ForumID, int UserID, string Keywords,
            bool SearchPosts, int PageSize, int PageIndex, out int TotalRecords)
        {
            TotalRecords = 0;
            DBForumTopicCollection forumTopicCollection = new DBForumTopicCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_TopicLoadAll");
            db.AddInParameter(dbCommand, "ForumID", DbType.Int32, ForumID);
            db.AddInParameter(dbCommand, "UserID", DbType.Int32, UserID);
            db.AddInParameter(dbCommand, "Keywords", DbType.String, Keywords);
            db.AddInParameter(dbCommand, "SearchPosts", DbType.Boolean, SearchPosts);
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, PageSize);
            db.AddInParameter(dbCommand, "PageIndex", DbType.Int32, PageIndex);
            db.AddOutParameter(dbCommand, "TotalRecords", DbType.Int32, 0);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBForumTopic forumTopic = GetForumTopicFromReader(dataReader);
                    forumTopicCollection.Add(forumTopic);
                }
            }
            TotalRecords = Convert.ToInt32(db.GetParameterValue(dbCommand, "@TotalRecords"));

            return forumTopicCollection;
        }

        /// <summary>
        /// Gets active topics
        /// </summary>
        /// <param name="ForumID">The forum group identifier</param>
        /// <param name="TopicCount">Topic count. 0 if you want to get all topics</param>
        /// <returns>Topics</returns>
        public override DBForumTopicCollection GetActiveTopics(int ForumID, int TopicCount)
        {
            DBForumTopicCollection forumTopicCollection = new DBForumTopicCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_TopicLoadActive");
            db.AddInParameter(dbCommand, "ForumID", DbType.Int32, ForumID);
            db.AddInParameter(dbCommand, "TopicCount", DbType.Int32, TopicCount);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBForumTopic forumTopic = GetForumTopicFromReader(dataReader);
                    forumTopicCollection.Add(forumTopic);
                }
            }
            return forumTopicCollection;
        }

        /// <summary>
        /// Inserts a topic
        /// </summary>
        /// <param name="ForumID">The forum identifier</param>
        /// <param name="UserID">The user identifier</param>
        /// <param name="TopicTypeID">The topic type identifier</param>
        /// <param name="Subject">The subject</param>
        /// <param name="NumPosts">The number of posts</param>
        /// <param name="Views">The number of views</param>
        /// <param name="LastPostID">The last post identifier</param>
        /// <param name="LastPostUserID">The last post user identifier</param>
        /// <param name="LastPostTime">The last post date and time</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Topic</returns>
        public override DBForumTopic InsertTopic(int ForumID, int UserID,
            int TopicTypeID, string Subject,
            int NumPosts, int Views, int LastPostID,
            int LastPostUserID, DateTime? LastPostTime,
            DateTime CreatedOn, DateTime UpdatedOn)
        {
            DBForumTopic forumTopic = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_TopicInsert");
            db.AddOutParameter(dbCommand, "TopicID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "ForumID", DbType.Int32, ForumID);
            db.AddInParameter(dbCommand, "UserID", DbType.Int32, UserID);
            db.AddInParameter(dbCommand, "TopicTypeID", DbType.Int32, TopicTypeID);
            db.AddInParameter(dbCommand, "Subject", DbType.String, Subject);
            db.AddInParameter(dbCommand, "NumPosts", DbType.Int32, NumPosts);
            db.AddInParameter(dbCommand, "Views", DbType.Int32, Views);
            db.AddInParameter(dbCommand, "LastPostID", DbType.Int32, LastPostID);
            db.AddInParameter(dbCommand, "LastPostUserID", DbType.Int32, LastPostUserID);
            if (LastPostTime.HasValue)
                db.AddInParameter(dbCommand, "LastPostTime", DbType.DateTime, LastPostTime.Value);
            else
                db.AddInParameter(dbCommand, "LastPostTime", DbType.DateTime, null);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, UpdatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int TopicID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@TopicID"));
                forumTopic = GetTopicByID(TopicID, false);
            }
            return forumTopic;
        }

        /// <summary>
        /// Updates the topic
        /// </summary>
        /// <param name="ForumTopicID">The forum topic identifier</param>
        /// <param name="ForumID">The forum identifier</param>
        /// <param name="UserID">The user identifier</param>
        /// <param name="TopicTypeID">The topic type identifier</param>
        /// <param name="Subject">The subject</param>
        /// <param name="NumPosts">The number of posts</param>
        /// <param name="Views">The number of views</param>
        /// <param name="LastPostID">The last post identifier</param>
        /// <param name="LastPostUserID">The last post user identifier</param>
        /// <param name="LastPostTime">The last post date and time</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Topic</returns>
        public override DBForumTopic UpdateTopic(int ForumTopicID, int ForumID, int UserID,
            int TopicTypeID, string Subject, 
            int NumPosts, int Views, int LastPostID,
            int LastPostUserID, DateTime? LastPostTime,
            DateTime CreatedOn, DateTime UpdatedOn)
        {
            DBForumTopic forumTopic = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_TopicUpdate");
            db.AddInParameter(dbCommand, "TopicID", DbType.Int32, ForumTopicID);
            db.AddInParameter(dbCommand, "ForumID", DbType.Int32, ForumID);
            db.AddInParameter(dbCommand, "UserID", DbType.Int32, UserID);
            db.AddInParameter(dbCommand, "TopicTypeID", DbType.Int32, TopicTypeID);
            db.AddInParameter(dbCommand, "Subject", DbType.String, Subject);
            db.AddInParameter(dbCommand, "NumPosts", DbType.Int32, NumPosts);
            db.AddInParameter(dbCommand, "Views", DbType.Int32, Views);
            db.AddInParameter(dbCommand, "LastPostID", DbType.Int32, LastPostID);
            db.AddInParameter(dbCommand, "LastPostUserID", DbType.Int32, LastPostUserID);
            if (LastPostTime.HasValue)
                db.AddInParameter(dbCommand, "LastPostTime", DbType.DateTime, LastPostTime.Value);
            else
                db.AddInParameter(dbCommand, "LastPostTime", DbType.DateTime, null);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, UpdatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                forumTopic = GetTopicByID(ForumTopicID, false);

            return forumTopic;
        }

        /// <summary>
        /// Deletes a post
        /// </summary>
        /// <param name="ForumPostID">The post identifier</param>
        public override void DeletePost(int ForumPostID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_PostDelete");
            db.AddInParameter(dbCommand, "PostID", DbType.Int32, ForumPostID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a post
        /// </summary>
        /// <param name="ForumPostID">The post identifier</param>
        /// <returns>Post</returns>
        public override DBForumPost GetPostByID(int ForumPostID)
        {
            DBForumPost forumPost = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_PostLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "PostID", DbType.Int32, ForumPostID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    forumPost = GetForumPostFromReader(dataReader);
                }
            }
            return forumPost;
        }

        /// <summary>
        /// Gets all posts
        /// </summary>
        /// <param name="ForumTopicID">The forum topic identifier</param>
        /// <param name="UserID">The user identifier</param>
        /// <param name="Keywords">Keywords</param>
        /// <param name="AscSort">Sort order</param>
        /// <param name="PageSize">Page size</param>
        /// <param name="PageIndex">Page index</param>
        /// <param name="TotalRecords">Total records</param>
        /// <returns>Posts</returns>
        public override DBForumPostCollection GetAllPosts(int ForumTopicID, int UserID, string Keywords,
            bool AscSort, int PageSize, int PageIndex, out int TotalRecords)
        {
            TotalRecords = 0;
            DBForumPostCollection forumPostCollection = new DBForumPostCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_PostLoadAll");
            db.AddInParameter(dbCommand, "TopicID", DbType.Int32, ForumTopicID);
            db.AddInParameter(dbCommand, "UserID", DbType.Int32, UserID);
            db.AddInParameter(dbCommand, "Keywords", DbType.String, Keywords);
            db.AddInParameter(dbCommand, "AscSort", DbType.Boolean, AscSort);
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, PageSize);
            db.AddInParameter(dbCommand, "PageIndex", DbType.Int32, PageIndex);
            db.AddOutParameter(dbCommand, "TotalRecords", DbType.Int32, 0);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBForumPost forumPost = GetForumPostFromReader(dataReader);
                    forumPostCollection.Add(forumPost);
                }
            }
            TotalRecords = Convert.ToInt32(db.GetParameterValue(dbCommand, "@TotalRecords"));

            return forumPostCollection;
        }

        /// <summary>
        /// Inserts a post
        /// </summary>
        /// <param name="ForumTopicID">The forum topic identifier</param>
        /// <param name="UserID">The user identifier</param>
        /// <param name="Text">The text</param>
        /// <param name="IPAddress">The IP address</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Post</returns>
        public override DBForumPost InsertPost(int ForumTopicID, int UserID,
            string Text, string IPAddress, DateTime CreatedOn, DateTime UpdatedOn)
        {
            DBForumPost forumPost = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_PostInsert");
            db.AddOutParameter(dbCommand, "PostID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "TopicID", DbType.Int32, ForumTopicID);
            db.AddInParameter(dbCommand, "UserID", DbType.Int32, UserID);
            db.AddInParameter(dbCommand, "Text", DbType.String, Text);
            db.AddInParameter(dbCommand, "IPAddress", DbType.String, IPAddress);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, UpdatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int PostID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@PostID"));
                forumPost = GetPostByID(PostID);
            }
            return forumPost;
        }

        /// <summary>
        /// Updates the post
        /// </summary>
        /// <param name="ForumPostID">The forum post identifier</param>
        /// <param name="ForumTopicID">The forum topic identifier</param>
        /// <param name="UserID">The user identifier</param>
        /// <param name="Text">The text</param>
        /// <param name="IPAddress">The IP address</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Post</returns>
        public override DBForumPost UpdatePost(int ForumPostID, int ForumTopicID, int UserID,
            string Text, string IPAddress, DateTime CreatedOn, DateTime UpdatedOn)
        {
            DBForumPost forumPost = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_PostUpdate");
            db.AddInParameter(dbCommand, "PostID", DbType.Int32, ForumPostID);
            db.AddInParameter(dbCommand, "TopicID", DbType.Int32, ForumTopicID);
            db.AddInParameter(dbCommand, "UserID", DbType.Int32, UserID);
            db.AddInParameter(dbCommand, "Text", DbType.String, Text);
            db.AddInParameter(dbCommand, "IPAddress", DbType.String, IPAddress);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, UpdatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                forumPost = GetPostByID(ForumPostID);

            return forumPost;
        }

        /// <summary>
        /// Deletes a private message
        /// </summary>
        /// <param name="ForumPrivateMessageID">The private message identifier</param>
        public override void DeletePrivateMessage(int ForumPrivateMessageID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_PrivateMessageDelete");
            db.AddInParameter(dbCommand, "PrivateMessageID", DbType.Int32, ForumPrivateMessageID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a private message
        /// </summary>
        /// <param name="ForumPrivateMessageID">The private message identifier</param>
        /// <returns>Private message</returns>
        public override DBPrivateMessage GetPrivateMessageByID(int ForumPrivateMessageID)
        {
            DBPrivateMessage privateMessage = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_PrivateMessageLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "PrivateMessageID", DbType.Int32, ForumPrivateMessageID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    privateMessage = GetPrivateMessageFromReader(dataReader);
                }
            }
            return privateMessage;
        }

        /// <summary>
        /// Gets private messages
        /// </summary>
        /// <param name="FromUserID">The user identifier who sent the message</param>
        /// <param name="ToUserID">The user identifier who should receive the message</param>
        /// <param name="IsRead">A value indicating whether loaded messages are read. false - to load not read messages only, 1 to load read messages only, null to load all messages</param>
        /// <param name="IsDeletedByAuthor">A value indicating whether loaded messages are deleted by author. false - messages are not deleted by author, null to load all messages</param>
        /// <param name="IsDeletedByRecipient">A value indicating whether loaded messages are deleted by recipient. false - messages are not deleted by recipient, null to load all messages</param>
        /// <param name="Keywords">Keywords</param>
        /// <param name="PageSize">Page size</param>
        /// <param name="PageIndex">Page index</param>
        /// <param name="TotalRecords">Total records</param>
        /// <returns>Private messages</returns>
        public override DBPrivateMessageCollection GetAllPrivateMessages(int FromUserID, int ToUserID,
            bool? IsRead, bool? IsDeletedByAuthor, bool? IsDeletedByRecipient, 
            string Keywords, int PageSize, int PageIndex, out int TotalRecords)
        {
            TotalRecords = 0;
            DBPrivateMessageCollection privateMessageCollection = new DBPrivateMessageCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_PrivateMessageLoadAll");
            db.AddInParameter(dbCommand, "FromUserID", DbType.Int32, FromUserID);
            db.AddInParameter(dbCommand, "ToUserID", DbType.Int32, ToUserID);
            if (IsRead.HasValue)
                db.AddInParameter(dbCommand, "IsRead", DbType.Boolean, IsRead.Value);
            else
                db.AddInParameter(dbCommand, "IsRead", DbType.Boolean, null);
            if (IsDeletedByAuthor.HasValue)
                db.AddInParameter(dbCommand, "IsDeletedByAuthor", DbType.Boolean, IsDeletedByAuthor.Value);
            else
                db.AddInParameter(dbCommand, "IsDeletedByAuthor", DbType.Boolean, null);
            if (IsDeletedByRecipient.HasValue)
                db.AddInParameter(dbCommand, "IsDeletedByRecipient", DbType.Boolean, IsDeletedByRecipient.Value);
            else
                db.AddInParameter(dbCommand, "IsDeletedByRecipient", DbType.Boolean, null);
            db.AddInParameter(dbCommand, "Keywords", DbType.String, Keywords);
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, PageSize);
            db.AddInParameter(dbCommand, "PageIndex", DbType.Int32, PageIndex);
            db.AddOutParameter(dbCommand, "TotalRecords", DbType.Int32, 0);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBPrivateMessage privateMessage = GetPrivateMessageFromReader(dataReader);
                    privateMessageCollection.Add(privateMessage);
                }
            }
            TotalRecords = Convert.ToInt32(db.GetParameterValue(dbCommand, "@TotalRecords"));

            return privateMessageCollection;
        }

        /// <summary>
        /// Inserts a private message
        /// </summary>
        /// <param name="FromUserID">The user identifier who sent the message</param>
        /// <param name="ToUserID">The user identifier who should receive the message</param>
        /// <param name="Subject">The subject</param>
        /// <param name="Text">The text</param>
        /// <param name="IsRead">The value indivating whether message is read</param>
        /// <param name="IsDeletedByAuthor">The value indivating whether message is deleted by author</param>
        /// <param name="IsDeletedByRecipient">The value indivating whether message is deleted by recipient</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>Private message</returns>
        public override DBPrivateMessage InsertPrivateMessage(int FromUserID, int ToUserID,
            string Subject, string Text, bool IsRead,
            bool IsDeletedByAuthor, bool IsDeletedByRecipient, DateTime CreatedOn)
        {
            DBPrivateMessage privateMessage = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_PrivateMessageInsert");
            db.AddOutParameter(dbCommand, "PrivateMessageID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "FromUserID", DbType.Int32, FromUserID);
            db.AddInParameter(dbCommand, "ToUserID", DbType.Int32, ToUserID);
            db.AddInParameter(dbCommand, "Subject", DbType.String, Subject);
            db.AddInParameter(dbCommand, "Text", DbType.String, Text);
            db.AddInParameter(dbCommand, "IsRead", DbType.Boolean, IsRead);
            db.AddInParameter(dbCommand, "IsDeletedByAuthor", DbType.Boolean, IsDeletedByAuthor);
            db.AddInParameter(dbCommand, "IsDeletedByRecipient", DbType.Boolean, IsDeletedByRecipient);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int PrivateMessageID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@PrivateMessageID"));
                privateMessage = GetPrivateMessageByID(PrivateMessageID);
            }
            return privateMessage;
        }

        /// <summary>
        /// Updates the private message
        /// </summary>
        /// <param name="PrivateMessageID">The private message identifier</param>
        /// <param name="FromUserID">The user identifier who sent the message</param>
        /// <param name="ToUserID">The user identifier who should receive the message</param>
        /// <param name="Subject">The subject</param>
        /// <param name="Text">The text</param>
        /// <param name="IsRead">The value indivating whether message is read</param>
        /// <param name="IsDeletedByAuthor">The value indivating whether message is deleted by author</param>
        /// <param name="IsDeletedByRecipient">The value indivating whether message is deleted by recipient</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>Private message</returns>
        public override DBPrivateMessage UpdatePrivateMessage(int PrivateMessageID, int FromUserID,
            int ToUserID, string Subject, string Text, bool IsRead,
            bool IsDeletedByAuthor, bool IsDeletedByRecipient, DateTime CreatedOn)
        {
            DBPrivateMessage privateMessage = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_PrivateMessageUpdate");
            db.AddInParameter(dbCommand, "PrivateMessageID", DbType.Int32, PrivateMessageID);
            db.AddInParameter(dbCommand, "FromUserID", DbType.Int32, FromUserID);
            db.AddInParameter(dbCommand, "ToUserID", DbType.Int32, ToUserID);
            db.AddInParameter(dbCommand, "Subject", DbType.String, Subject);
            db.AddInParameter(dbCommand, "Text", DbType.String, Text);
            db.AddInParameter(dbCommand, "IsRead", DbType.Boolean, IsRead);
            db.AddInParameter(dbCommand, "IsDeletedByAuthor", DbType.Boolean, IsDeletedByAuthor);
            db.AddInParameter(dbCommand, "IsDeletedByRecipient", DbType.Boolean, IsDeletedByRecipient);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                privateMessage = GetPrivateMessageByID(PrivateMessageID);

            return privateMessage;
        }

        /// <summary>
        /// Deletes a forum subscription
        /// </summary>
        /// <param name="ForumSubscriptionID">The forum subscription identifier</param>
        public override void DeleteSubscription(int ForumSubscriptionID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_SubscriptionDelete");
            db.AddInParameter(dbCommand, "SubscriptionID", DbType.Int32, ForumSubscriptionID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a forum subscription
        /// </summary>
        /// <param name="ForumSubscriptionID">The forum subscription identifier</param>
        /// <returns>Forum subscription</returns>
        public override DBForumSubscription GetSubscriptionByID(int ForumSubscriptionID)
        {
            DBForumSubscription forumSubscription = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_SubscriptionLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "SubscriptionID", DbType.Int32, ForumSubscriptionID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    forumSubscription = GetForumSubscriptionFromReader(dataReader);
                }
            }
            return forumSubscription;
        }

        /// <summary>
        /// Gets a forum subscription
        /// </summary>
        /// <param name="SubscriptionGUID">The forum subscription identifier</param>
        /// <returns>Forum subscription</returns>
        public override DBForumSubscription GetSubscriptionByGUID(int SubscriptionGUID)
        {
            DBForumSubscription forumSubscription = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_SubscriptionLoadByGUID");
            db.AddInParameter(dbCommand, "SubscriptionGUID", DbType.Guid, SubscriptionGUID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    forumSubscription = GetForumSubscriptionFromReader(dataReader);
                }
            }
            return forumSubscription;
        }

        /// <summary>
        /// Gets forum subscriptions
        /// </summary>
        /// <param name="UserID">The user identifier</param>
        /// <param name="ForumID">The forum identifier</param>
        /// <param name="TopicID">The topic identifier</param>
        /// <param name="PageSize">Page size</param>
        /// <param name="PageIndex">Page index</param>
        /// <param name="TotalRecords">Total records</param>
        /// <returns>Forum subscriptions</returns>
        public override DBForumSubscriptionCollection GetAllSubscriptions(int UserID, int ForumID,
            int TopicID, int PageSize, int PageIndex, out int TotalRecords)
        {
            TotalRecords = 0;
            DBForumSubscriptionCollection forumSubscriptionCollection = new DBForumSubscriptionCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_SubscriptionLoadAll");
            db.AddInParameter(dbCommand, "UserID", DbType.Int32, UserID);
            db.AddInParameter(dbCommand, "ForumID", DbType.Int32, ForumID);
            db.AddInParameter(dbCommand, "TopicID", DbType.Int32, TopicID);
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, PageSize);
            db.AddInParameter(dbCommand, "PageIndex", DbType.Int32, PageIndex);
            db.AddOutParameter(dbCommand, "TotalRecords", DbType.Int32, 0);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBForumSubscription forumSubscription = GetForumSubscriptionFromReader(dataReader);
                    forumSubscriptionCollection.Add(forumSubscription);
                }
            }
            TotalRecords = Convert.ToInt32(db.GetParameterValue(dbCommand, "@TotalRecords"));

            return forumSubscriptionCollection;
        }
        
        /// <summary>
        /// Inserts a forum subscription
        /// </summary>
        /// <param name="SubscriptionGUID">The forum subscription identifier</param>
        /// <param name="UserID">The user identifier</param>
        /// <param name="ForumID">The forum identifier</param>
        /// <param name="TopicID">The topic identifier</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>Forum subscription</returns>
        public override DBForumSubscription InsertSubscription(Guid SubscriptionGUID, int UserID,
            int ForumID, int TopicID, DateTime CreatedOn)
        {
            DBForumSubscription forumSubscription = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_SubscriptionInsert");
            db.AddOutParameter(dbCommand, "SubscriptionID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "SubscriptionGUID", DbType.Guid, SubscriptionGUID);
            db.AddInParameter(dbCommand, "UserID", DbType.Int32, UserID);
            db.AddInParameter(dbCommand, "ForumID", DbType.Int32, ForumID);
            db.AddInParameter(dbCommand, "TopicID", DbType.Int32, TopicID);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int SubscriptionID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@SubscriptionID"));
                forumSubscription = GetSubscriptionByID(SubscriptionID);
            }
            return forumSubscription;
        }

        /// <summary>
        /// Updates the forum subscription
        /// </summary>
        /// <param name="SubscriptionID">The forum subscription identifier</param>
        /// <param name="SubscriptionGUID">The forum subscription identifier</param>
        /// <param name="UserID">The user identifier</param>
        /// <param name="ForumID">The forum identifier</param>
        /// <param name="TopicID">The topic identifier</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>Forum subscription</returns>
        public override DBForumSubscription UpdateSubscription(int SubscriptionID, 
            Guid SubscriptionGUID, int UserID, int ForumID, int TopicID, DateTime CreatedOn)
        {
            DBForumSubscription forumSubscription = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_SubscriptionUpdate");
            db.AddInParameter(dbCommand, "SubscriptionID", DbType.Int32, SubscriptionID);
            db.AddInParameter(dbCommand, "SubscriptionGUID", DbType.Guid, SubscriptionGUID);
            db.AddInParameter(dbCommand, "UserID", DbType.Int32, UserID);
            db.AddInParameter(dbCommand, "ForumID", DbType.Int32, ForumID);
            db.AddInParameter(dbCommand, "TopicID", DbType.Int32, TopicID);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                forumSubscription = GetSubscriptionByID(SubscriptionID);

            return forumSubscription;
        }
        #endregion
    }
}
