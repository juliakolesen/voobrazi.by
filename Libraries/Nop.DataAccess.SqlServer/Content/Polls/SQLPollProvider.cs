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

namespace NopSolutions.NopCommerce.DataAccess.Content.Polls
{
    /// <summary>
    /// Poll provider for SQL Server
    /// </summary>
    public partial class SQLPollProvider : DBPollProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBPoll GetPollFromReader(IDataReader dataReader)
        {
            DBPoll poll = new DBPoll();
            poll.PollID = NopSqlDataHelper.GetInt(dataReader, "PollID");
            poll.LanguageID = NopSqlDataHelper.GetInt(dataReader, "LanguageID");
            poll.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            poll.SystemKeyword = NopSqlDataHelper.GetString(dataReader, "SystemKeyword");
            poll.Published = NopSqlDataHelper.GetBoolean(dataReader, "Published");
            poll.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            return poll;
        }

        private DBPollAnswer GetPollAnswerFromReader(IDataReader dataReader)
        {
            DBPollAnswer pollAnswer = new DBPollAnswer();
            pollAnswer.PollAnswerID = NopSqlDataHelper.GetInt(dataReader, "PollAnswerID");
            pollAnswer.PollID = NopSqlDataHelper.GetInt(dataReader, "PollID");
            pollAnswer.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            pollAnswer.Count = NopSqlDataHelper.GetInt(dataReader, "Count");
            pollAnswer.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            return pollAnswer;
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
        /// Gets a poll
        /// </summary>
        /// <param name="PollID">The poll identifier</param>
        /// <returns>Poll</returns>
        public override DBPoll GetPollByID(int PollID)
        {
            DBPoll poll = null;
            if (PollID == 0)
                return poll;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PollLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "PollID", DbType.Int32, PollID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    poll = GetPollFromReader(dataReader);
                }
            }
            return poll;
        }

        /// <summary>
        /// Gets a poll
        /// </summary>
        /// <param name="SystemKeyword">Poll system keyword</param>
        /// <returns>Poll</returns>
        public override DBPoll GetPollBySystemKeyword(string SystemKeyword)
        {
            DBPoll poll = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PollLoadBySystemKeyword");
            db.AddInParameter(dbCommand, "SystemKeyword", DbType.String, SystemKeyword);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    poll = GetPollFromReader(dataReader);
                }
            }
            return poll;
        }

        /// <summary>
        /// Gets poll collection
        /// </summary>
        /// <param name="LanguageID">Language identifier. 0 if you want to get all news</param>
        /// <param name="PollCount">Poll count to load. 0 if you want to get all polls</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Poll collection</returns>
        public override DBPollCollection GetPolls(int LanguageID, int PollCount, bool showHidden)
        {
            DBPollCollection pollCollection = new DBPollCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PollLoadAll");
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
            db.AddInParameter(dbCommand, "PollCount", DbType.Int32, PollCount);
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBPoll poll = GetPollFromReader(dataReader);
                    pollCollection.Add(poll);
                }
            }

            return pollCollection;
        }

        /// <summary>
        /// Deletes a poll
        /// </summary>
        /// <param name="PollID">The poll identifier</param>
        public override void DeletePoll(int PollID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PollDelete");
            db.AddInParameter(dbCommand, "PollID", DbType.Int32, PollID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Inserts a poll
        /// </summary>
        /// <param name="LanguageID">The language identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="SystemKeyword">The system keyword</param>
        /// <param name="Published">A value indicating whether the entity is published</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Poll</returns>
        public override DBPoll InsertPoll(int LanguageID, string Name,
            string SystemKeyword, bool Published, int DisplayOrder)
        {
            DBPoll poll = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PollInsert");
            db.AddOutParameter(dbCommand, "PollID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "SystemKeyword", DbType.String, SystemKeyword);
            db.AddInParameter(dbCommand, "Published", DbType.Boolean, Published);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int PollID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@PollID"));
                poll = GetPollByID(PollID);
            }
            return poll;
        }

        /// <summary>
        /// Updates the poll
        /// </summary>
        /// <param name="PollID">The poll identifier</param>
        /// <param name="LanguageID">The language identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="SystemKeyword">The system keyword</param>
        /// <param name="Published">A value indicating whether the entity is published</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Poll</returns>
        public override DBPoll UpdatePoll(int PollID, int LanguageID, string Name, 
            string SystemKeyword, bool Published, int DisplayOrder)
        {
            DBPoll poll = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PollUpdate");
            db.AddInParameter(dbCommand, "PollID", DbType.Int32, PollID);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "SystemKeyword", DbType.String, SystemKeyword);
            db.AddInParameter(dbCommand, "Published", DbType.Boolean, Published);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                poll = GetPollByID(PollID);

            return poll;
        }

        /// <summary>
        /// Is voting record already exists
        /// </summary>
        /// <param name="PollID">Poll identifier</param>
        /// <param name="CustomerID">Customer identifier</param>
        /// <returns>Poll</returns>
        public override bool PollVotingRecordExists(int PollID, int CustomerID)
        {
            bool result = false;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PollVotingRecordExists");
            db.AddInParameter(dbCommand, "PollID", DbType.Int32, PollID);
            db.AddInParameter(dbCommand, "CustomerID", DbType.String, CustomerID);
            if ((int)db.ExecuteScalar(dbCommand) > 0)
                result = true;

            return result;
        }

        /// <summary>
        /// Gets a poll answer
        /// </summary>
        /// <param name="PollAnswerID">Poll answer identifier</param>
        /// <returns>Poll answer</returns>
        public override DBPollAnswer GetPollAnswerByID(int PollAnswerID)
        {
            DBPollAnswer pollAnswer = null;
            if (PollAnswerID == 0)
                return pollAnswer;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PollAnswerLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "PollAnswerID", DbType.Int32, PollAnswerID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    pollAnswer = GetPollAnswerFromReader(dataReader);
                }
            }
            return pollAnswer;
        }

        /// <summary>
        /// Gets a poll answers by poll identifier
        /// </summary>
        /// <param name="PollID">Poll identifier</param>
        /// <returns>Poll answer collection</returns>
        public override DBPollAnswerCollection GetPollAnswersByPollID(int PollID)
        {
            DBPollAnswerCollection pollAnswerCollection = new DBPollAnswerCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PollAnswerLoadByPollID");
            db.AddInParameter(dbCommand, "PollID", DbType.Int32, PollID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBPollAnswer pollAnswer = GetPollAnswerFromReader(dataReader);
                    pollAnswerCollection.Add(pollAnswer);
                }
            }

            return pollAnswerCollection;
        }

        /// <summary>
        /// Deletes a poll answer
        /// </summary>
        /// <param name="PollAnswerID">Poll answer identifier</param>
        public override void DeletePollAnswer(int PollAnswerID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PollAnswerDelete");
            db.AddInParameter(dbCommand, "PollAnswerID", DbType.Int32, PollAnswerID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Inserts a poll answer
        /// </summary>
        /// <param name="PollID">The poll identifier</param>
        /// <param name="Name">The poll answer name</param>
        /// <param name="Count">The current count</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Poll answer</returns>
        public override DBPollAnswer InsertPollAnswer(int PollID, string Name, int Count, int DisplayOrder)
        {
            DBPollAnswer pollAnswer = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PollAnswerInsert");
            db.AddOutParameter(dbCommand, "PollAnswerID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "PollID", DbType.Int32, PollID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "Count", DbType.Int32, Count);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int PollAnswerID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@PollAnswerID"));
                pollAnswer = GetPollAnswerByID(PollAnswerID);
            }
            return pollAnswer;
        }

        /// <summary>
        /// Updates the poll answer
        /// </summary>
        /// <param name="PollAnswerID">The poll answer identifier</param>
        /// <param name="PollID">The poll identifier</param>
        /// <param name="Name">The poll answer name</param>
        /// <param name="Count">The current count</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Poll answer</returns>
        public override DBPollAnswer UpdatePollAnswer(int PollAnswerID, int PollID, string Name, int Count, int DisplayOrder)
        {
            DBPollAnswer pollAnswer = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PollAnswerUpdate");
            db.AddInParameter(dbCommand, "PollAnswerID", DbType.Int32, PollAnswerID);
            db.AddInParameter(dbCommand, "PollID", DbType.Int32, PollID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "Count", DbType.Int32, Count);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                pollAnswer = GetPollAnswerByID(PollAnswerID);

            return pollAnswer;
        }

        /// <summary>
        /// Creates a poll voting record
        /// </summary>
        /// <param name="PollAnswerID">The poll answer identifier</param>
        /// <param name="CustomerID">Customer identifer</param>
        public override void CreatePollVotingRecord(int PollAnswerID, int CustomerID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PollVotingRecordCreate");
            db.AddInParameter(dbCommand, "PollAnswerID", DbType.Int32, PollAnswerID);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, CustomerID);
            db.ExecuteNonQuery(dbCommand);
        }

        #endregion
    }
}
