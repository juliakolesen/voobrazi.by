//------------------------------------------------------------------------------
// The contents of this file are title to the nopCommerce Public License Version 1.0 ("License"); you may not use this file except in compliance with the License.
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

namespace NopSolutions.NopCommerce.DataAccess.Content.Topics
{
    /// <summary>
    /// Topic provider for SQL Server
    /// </summary>
    public partial class SQLTopicProvider : DBTopicProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBTopic GetTopicFromReader(IDataReader dataReader)
        {
            DBTopic topic = new DBTopic();
            topic.TopicID = NopSqlDataHelper.GetInt(dataReader, "TopicID");
            topic.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            return topic;
        }

        private DBLocalizedTopic GetLocalizedTopicFromReader(IDataReader dataReader)
        {
            DBLocalizedTopic localizedTopic = new DBLocalizedTopic();
            localizedTopic.TopicLocalizedID = NopSqlDataHelper.GetInt(dataReader, "TopicLocalizedID");
            localizedTopic.TopicID = NopSqlDataHelper.GetInt(dataReader, "TopicID");
            localizedTopic.LanguageID = NopSqlDataHelper.GetInt(dataReader, "LanguageID");
            localizedTopic.Title = NopSqlDataHelper.GetString(dataReader, "Title");
            localizedTopic.Body = NopSqlDataHelper.GetString(dataReader, "Body");
            localizedTopic.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            localizedTopic.UpdatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "UpdatedOn");
            return localizedTopic;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a topic
        /// </summary>
        /// <param name="TopicID">Topic identifier</param>
        public override void DeleteTopic(int TopicID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TopicDelete");
            db.AddInParameter(dbCommand, "TopicID", DbType.Int32, TopicID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Inserts a topic
        /// </summary>
        /// <param name="Name">The name</param>
        /// <returns>Topic</returns>
        public override DBTopic InsertTopic(string Name)
        {
            DBTopic topic = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TopicInsert");
            db.AddOutParameter(dbCommand, "TopicID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int TopicID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@TopicID"));
                topic = GetTopicByID(TopicID);
            }
            return topic;
        }

        /// <summary>
        /// Updates the topic
        /// </summary>
        /// <param name="TopicID">The topic identifier</param>
        /// <param name="Name">The name</param>
        /// <returns>Topic</returns>
        public override DBTopic UpdateTopic(int TopicID, string Name)
        {
            DBTopic topic = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TopicUpdate");
            db.AddInParameter(dbCommand, "TopicID", DbType.Int32, TopicID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                topic = GetTopicByID(TopicID);

            return topic;
        }

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
        /// Gets a topic by template identifier
        /// </summary>
        /// <param name="TopicID">Topic identifier</param>
        /// <returns>Topic</returns>
        public override DBTopic GetTopicByID(int TopicID)
        {
            DBTopic topic = null;
            if (TopicID == 0)
                return topic;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TopicLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "TopicID", DbType.Int32, TopicID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    topic = GetTopicFromReader(dataReader);
                }
            }
            return topic;
        }

        /// <summary>
        /// Gets all topics
        /// </summary>
        /// <returns>Topic collection</returns>
        public override DBTopicCollection GetAllTopics()
        {
            DBTopicCollection topicCollection = new DBTopicCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TopicLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBTopic topic = GetTopicFromReader(dataReader);
                    topicCollection.Add(topic);
                }
            }
            return topicCollection;
        }

        /// <summary>
        /// Gets a localized topic by identifier
        /// </summary>
        /// <param name="LocalizedTopicID">Localized topic identifier</param>
        /// <returns>Localized topic</returns>
        public override DBLocalizedTopic GetLocalizedTopicByID(int LocalizedTopicID)
        {
            DBLocalizedTopic localizedTopic = null;
            if (LocalizedTopicID == 0)
                return localizedTopic;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TopicLocalizedLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "TopicLocalizedID", DbType.Int32, LocalizedTopicID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    localizedTopic = GetLocalizedTopicFromReader(dataReader);
                }
            }
            return localizedTopic;
        }

        /// <summary>
        /// Gets a localized topic by parent TopicID and language identifier
        /// </summary>
        /// <param name="TopicID">The topic identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Localized topic</returns>
        public override DBLocalizedTopic GetLocalizedTopic(int TopicID, int LanguageID)
        {
            DBLocalizedTopic localizedTopic = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TopicLocalizedLoadByTopicIDAndLanguageID");
            db.AddInParameter(dbCommand, "TopicID", DbType.Int32, TopicID);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    localizedTopic = GetLocalizedTopicFromReader(dataReader);
                }
            }
            return localizedTopic;
        }

        /// <summary>
        /// Gets a localized topic by name and language identifier
        /// </summary>
        /// <param name="Name">Topic name</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Localized topic</returns>
        public override DBLocalizedTopic GetLocalizedTopic(string Name, int LanguageID)
        {
            DBLocalizedTopic localizedTopic = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TopicLocalizedLoadByNameAndLanguageID");
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    localizedTopic = GetLocalizedTopicFromReader(dataReader);
                }
            }
            return localizedTopic;
        }

        /// <summary>
        /// Deletes a localized topic
        /// </summary>
        /// <param name="LocalizedTopicID">Topic identifier</param>
        public override void DeleteLocalizedTopic(int LocalizedTopicID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TopicLocalizedDelete");
            db.AddInParameter(dbCommand, "TopicLocalizedID", DbType.Int32, LocalizedTopicID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets all localized topics
        /// </summary>
        /// <param name="TopicName">Topic name</param>
        /// <returns>Localized topic collection</returns>
        public override DBLocalizedTopicCollection GetAllLocalizedTopics(string TopicName)
        {
            DBLocalizedTopicCollection localizedTopics = new DBLocalizedTopicCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TopicLocalizedLoadAllByName");
            db.AddInParameter(dbCommand, "Name", DbType.String, TopicName);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBLocalizedTopic localizedTopic = GetLocalizedTopicFromReader(dataReader);
                    localizedTopics.Add(localizedTopic);
                }
            }
            return localizedTopics;
        }

        /// <summary>
        /// Inserts a localized topic
        /// </summary>
        /// <param name="TopicID">The topic identifier</param>
        /// <param name="LanguageID">The language identifier</param>
        /// <param name="Title">The title</param>
        /// <param name="Body">The body</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Localized topic</returns>
        public override DBLocalizedTopic InsertLocalizedTopic(int TopicID,
            int LanguageID, string Title, string Body,
            DateTime CreatedOn, DateTime UpdatedOn)
        {
            DBLocalizedTopic localizedTopic = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TopicLocalizedInsert");
            db.AddOutParameter(dbCommand, "TopicLocalizedID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "TopicID", DbType.Int32, TopicID);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
            db.AddInParameter(dbCommand, "Title", DbType.String, Title);
            db.AddInParameter(dbCommand, "Body", DbType.String, Body);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, UpdatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int TopicLocalizedID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@TopicLocalizedID"));
                localizedTopic = GetLocalizedTopicByID(TopicLocalizedID);
            }
            return localizedTopic;
        }

        /// <summary>
        /// Updates the localized topic
        /// </summary>
        /// <param name="TopicLocalizedID">The localized topic identifier</param>
        /// <param name="TopicID">The topic identifier</param>
        /// <param name="LanguageID">The language identifier</param>
        /// <param name="Title">The title</param>
        /// <param name="Body">The body</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Localized topic</returns>
        public override DBLocalizedTopic UpdateLocalizedTopic(int TopicLocalizedID,
            int TopicID, int LanguageID,
            string Title, string Body,
            DateTime CreatedOn, DateTime UpdatedOn)
        {
            DBLocalizedTopic localizedTopic = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TopicLocalizedUpdate");
            db.AddInParameter(dbCommand, "TopicLocalizedID", DbType.Int32, TopicLocalizedID);
            db.AddInParameter(dbCommand, "TopicID", DbType.Int32, TopicID);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
            db.AddInParameter(dbCommand, "Title", DbType.String, Title);
            db.AddInParameter(dbCommand, "Body", DbType.String, Body);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, UpdatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                localizedTopic = GetLocalizedTopicByID(TopicLocalizedID);

            return localizedTopic;
        }
        #endregion
    }
}

