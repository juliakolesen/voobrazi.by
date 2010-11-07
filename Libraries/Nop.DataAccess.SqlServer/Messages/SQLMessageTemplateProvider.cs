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

namespace NopSolutions.NopCommerce.DataAccess.Messages
{
    /// <summary>
    /// Message template provider for SQL Server
    /// </summary>
    public partial class SQLMessageTemplateProvider : DBMessageTemplateProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBMessageTemplate GetMessageTemplateFromReader(IDataReader dataReader)
        {
            DBMessageTemplate messageTemplate = new DBMessageTemplate();
            messageTemplate.MessageTemplateID = NopSqlDataHelper.GetInt(dataReader, "MessageTemplateID");
            messageTemplate.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            return messageTemplate;
        }

        private DBLocalizedMessageTemplate GetLocalizedMessageTemplateFromReader(IDataReader dataReader)
        {
            DBLocalizedMessageTemplate localizedMessageTemplate = new DBLocalizedMessageTemplate();
            localizedMessageTemplate.MessageTemplateLocalizedID = NopSqlDataHelper.GetInt(dataReader, "MessageTemplateLocalizedID");
            localizedMessageTemplate.MessageTemplateID = NopSqlDataHelper.GetInt(dataReader, "MessageTemplateID");
            localizedMessageTemplate.LanguageID = NopSqlDataHelper.GetInt(dataReader, "LanguageID");
            localizedMessageTemplate.Subject = NopSqlDataHelper.GetString(dataReader, "Subject");
            localizedMessageTemplate.Body = NopSqlDataHelper.GetString(dataReader, "Body");
            return localizedMessageTemplate;
        }

        private DBQueuedEmail GetQueuedEmailFromReader(IDataReader dataReader)
        {
            DBQueuedEmail queuedEmail = new DBQueuedEmail();
            queuedEmail.QueuedEmailID = NopSqlDataHelper.GetInt(dataReader, "QueuedEmailID");
            queuedEmail.Priority = NopSqlDataHelper.GetInt(dataReader, "Priority");
            queuedEmail.From = NopSqlDataHelper.GetString(dataReader, "From");
            queuedEmail.FromName = NopSqlDataHelper.GetString(dataReader, "FromName");
            queuedEmail.To = NopSqlDataHelper.GetString(dataReader, "To");
            queuedEmail.ToName = NopSqlDataHelper.GetString(dataReader, "ToName");
            queuedEmail.Cc = NopSqlDataHelper.GetString(dataReader, "Cc");
            queuedEmail.Bcc = NopSqlDataHelper.GetString(dataReader, "Bcc");
            queuedEmail.Subject = NopSqlDataHelper.GetString(dataReader, "Subject");
            queuedEmail.Body = NopSqlDataHelper.GetString(dataReader, "Body");
            queuedEmail.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            queuedEmail.SendTries = NopSqlDataHelper.GetInt(dataReader, "SendTries");
            queuedEmail.SentOn = NopSqlDataHelper.GetNullableUtcDateTime(dataReader, "SentOn");
            return queuedEmail;
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
        /// Gets a message template by template identifier
        /// </summary>
        /// <param name="MessageTemplateID">Message template identifier</param>
        /// <returns>Message template</returns>
        public override DBMessageTemplate GetMessageTemplateByID(int MessageTemplateID)
        {
            DBMessageTemplate messageTemplate = null;
            if (MessageTemplateID == 0)
                return messageTemplate;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_MessageTemplateLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "MessageTemplateID", DbType.Int32, MessageTemplateID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    messageTemplate = GetMessageTemplateFromReader(dataReader);
                }
            }
            return messageTemplate;
        }

        /// <summary>
        /// Gets all message templates
        /// </summary>
        /// <returns>Message template collection</returns>
        public override DBMessageTemplateCollection GetAllMessageTemplates()
        {
            DBMessageTemplateCollection messageTemplateCollection = new DBMessageTemplateCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_MessageTemplateLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBMessageTemplate messageTemplate = GetMessageTemplateFromReader(dataReader);
                    messageTemplateCollection.Add(messageTemplate);
                }
            }
            return messageTemplateCollection;
        }

        /// <summary>
        /// Gets a localized message template by identifier
        /// </summary>
        /// <param name="LocalizedMessageTemplateID">Localized message template identifier</param>
        /// <returns>Localized message template</returns>
        public override DBLocalizedMessageTemplate GetLocalizedMessageTemplateByID(int LocalizedMessageTemplateID)
        {
            DBLocalizedMessageTemplate localizedMessageTemplate = null;
            if (LocalizedMessageTemplateID == 0)
                return localizedMessageTemplate;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_MessageTemplateLocalizedLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "MessageTemplateLocalizedID", DbType.Int32, LocalizedMessageTemplateID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    localizedMessageTemplate = GetLocalizedMessageTemplateFromReader(dataReader);
                }
            }
            return localizedMessageTemplate;
        }

        /// <summary>
        /// Gets a localized message template by name and language identifier
        /// </summary>
        /// <param name="Name">Message template name</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Localized message template</returns>
        public override DBLocalizedMessageTemplate GetLocalizedMessageTemplate(string Name, int LanguageID)
        {
            DBLocalizedMessageTemplate localizedMessageTemplate = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_MessageTemplateLocalizedLoadByNameAndLanguageID");
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    localizedMessageTemplate = GetLocalizedMessageTemplateFromReader(dataReader);
                }
            }
            return localizedMessageTemplate;
        }

        /// <summary>
        /// Deletes a localized message template
        /// </summary>
        /// <param name="LocalizedMessageTemplateID">Message template identifier</param>
        public override void DeleteLocalizedMessageTemplate(int LocalizedMessageTemplateID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_MessageTemplateLocalizedDelete");
            db.AddInParameter(dbCommand, "MessageTemplateLocalizedID", DbType.Int32, LocalizedMessageTemplateID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets all localized message templates
        /// </summary>
        /// <param name="MessageTemplatesName">Message template name</param>
        /// <returns>Localized message template collection</returns>
        public override DBLocalizedMessageTemplateCollection GetAllLocalizedMessageTemplates(string MessageTemplatesName)
        {
            DBLocalizedMessageTemplateCollection localizedMessageTemplates = new DBLocalizedMessageTemplateCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_MessageTemplateLocalizedLoadAllByName");
            db.AddInParameter(dbCommand, "Name", DbType.String, MessageTemplatesName);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBLocalizedMessageTemplate localizedMessageTemplate = GetLocalizedMessageTemplateFromReader(dataReader);
                    localizedMessageTemplates.Add(localizedMessageTemplate);
                }
            }
            return localizedMessageTemplates;
        }

        /// <summary>
        /// Inserts a localized message template
        /// </summary>
        /// <param name="MessageTemplateID">The message template identifier</param>
        /// <param name="LanguageID">The language identifier</param>
        /// <param name="Subject">The subject</param>
        /// <param name="Body">The body</param>
        /// <returns>Localized message template</returns>
        public override DBLocalizedMessageTemplate InsertLocalizedMessageTemplate(int MessageTemplateID,
            int LanguageID, string Subject, string Body)
        {
            DBLocalizedMessageTemplate localizedMessageTemplate = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_MessageTemplateLocalizedInsert");
            db.AddOutParameter(dbCommand, "MessageTemplateLocalizedID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "MessageTemplateID", DbType.Int32, MessageTemplateID);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
            db.AddInParameter(dbCommand, "Subject", DbType.String, Subject);
            db.AddInParameter(dbCommand, "Body", DbType.String, Body);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int MessageTemplateLocalizedID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@MessageTemplateLocalizedID"));
                localizedMessageTemplate = GetLocalizedMessageTemplateByID(MessageTemplateLocalizedID);
            }
            return localizedMessageTemplate;
        }

        /// <summary>
        /// Updates the localized message template
        /// </summary>
        /// <param name="MessageTemplateLocalizedID">The localized message template identifier</param>
        /// <param name="MessageTemplateID">The message template identifier</param>
        /// <param name="LanguageID">The language identifier</param>
        /// <param name="Subject">The subject</param>
        /// <param name="Body">The body</param>
        /// <returns>Localized message template</returns>
        public override DBLocalizedMessageTemplate UpdateLocalizedMessageTemplate(int MessageTemplateLocalizedID,
            int MessageTemplateID, int LanguageID,
            string Subject, string Body)
        {
            DBLocalizedMessageTemplate localizedMessageTemplate = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_MessageTemplateLocalizedUpdate");
            db.AddInParameter(dbCommand, "MessageTemplateLocalizedID", DbType.Int32, MessageTemplateLocalizedID);
            db.AddInParameter(dbCommand, "MessageTemplateID", DbType.Int32, MessageTemplateID);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
            db.AddInParameter(dbCommand, "Subject", DbType.String, Subject);
            db.AddInParameter(dbCommand, "Body", DbType.String, Body);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                localizedMessageTemplate = GetLocalizedMessageTemplateByID(MessageTemplateLocalizedID);

            return localizedMessageTemplate;
        }

        /// <summary>
        /// Gets a queued email by identifier
        /// </summary>
        /// <param name="QueuedEmailID">Email item identifier</param>
        /// <returns>Email item</returns>
        public override DBQueuedEmail GetQueuedEmailByID(int QueuedEmailID)
        {
            DBQueuedEmail queuedEmail = null;
            if (QueuedEmailID == 0)
                return queuedEmail;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_QueuedEmailLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "QueuedEmailID", DbType.Int32, QueuedEmailID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    queuedEmail = GetQueuedEmailFromReader(dataReader);
                }
            }
            return queuedEmail;
        }

        /// <summary>
        /// Deletes a queued email
        /// </summary>
        /// <param name="QueuedEmailID">Email item identifier</param>
        public override void DeleteQueuedEmail(int QueuedEmailID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_QueuedEmailDelete");
            db.AddInParameter(dbCommand, "QueuedEmailID", DbType.Int32, QueuedEmailID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets all queued emails
        /// </summary>
        /// <param name="FromEmail">From Email</param>
        /// <param name="ToEmail">To Email</param>
        /// <param name="StartTime">The start time</param>
        /// <param name="EndTime">The end time</param>
        /// <param name="QueuedEmailCount">Email item count. 0 if you want to get all items</param>
        /// <param name="LoadNotSentItemsOnly">A value indicating whether to load only not sent emails</param>
        /// <param name="MaxSendTries">Maximum send tries</param>
        /// <returns>Email item collection</returns>
        public override DBQueuedEmailCollection GetAllQueuedEmails(string FromEmail,
            string ToEmail, DateTime? StartTime, DateTime? EndTime,
            int QueuedEmailCount, bool LoadNotSentItemsOnly, int MaxSendTries)
        {
            DBQueuedEmailCollection queuedEmailCollection = new DBQueuedEmailCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_QueuedEmailLoadAll");
            db.AddInParameter(dbCommand, "FromEmail", DbType.String, FromEmail);
            db.AddInParameter(dbCommand, "ToEmail", DbType.String, ToEmail);
            if (StartTime.HasValue)
                db.AddInParameter(dbCommand, "StartTime", DbType.DateTime, StartTime.Value);
            else
                db.AddInParameter(dbCommand, "StartTime", DbType.DateTime, DBNull.Value);
            if (EndTime.HasValue)
                db.AddInParameter(dbCommand, "EndTime", DbType.DateTime, EndTime.Value);
            else
                db.AddInParameter(dbCommand, "EndTime", DbType.DateTime, DBNull.Value);
            db.AddInParameter(dbCommand, "QueuedEmailCount", DbType.Int32, QueuedEmailCount);
            db.AddInParameter(dbCommand, "LoadNotSentItemsOnly", DbType.Boolean, LoadNotSentItemsOnly);
            db.AddInParameter(dbCommand, "MaxSendTries", DbType.Int32, MaxSendTries);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBQueuedEmail queuedEmail = GetQueuedEmailFromReader(dataReader);
                    queuedEmailCollection.Add(queuedEmail);
                }
            }

            return queuedEmailCollection;
        }

        /// <summary>
        /// Inserts a queued email
        /// </summary>
        /// <param name="Priority">The priority</param>
        /// <param name="From">From</param>
        /// <param name="FromName">From name</param>
        /// <param name="To">To</param>
        /// <param name="ToName">To name</param>
        /// <param name="Cc">Cc</param>
        /// <param name="Bcc">Bcc</param>
        /// <param name="Subject">Subject</param>
        /// <param name="Body">Body</param>
        /// <param name="CreatedOn">The date and time of item creation</param>
        /// <param name="SendTries">The send tries</param>
        /// <param name="SentOn">The sent date and time. Null if email is not sent yet</param>
        /// <returns>Queued email</returns>
        public override DBQueuedEmail InsertQueuedEmail(int Priority, string From, string FromName,
            string To, string ToName, string Cc, string Bcc, string Subject,
            string Body, DateTime CreatedOn, int SendTries, DateTime? SentOn)
        {
            DBQueuedEmail queuedEmail = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_QueuedEmailInsert");
            db.AddOutParameter(dbCommand, "QueuedEmailID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Priority", DbType.Int32, Priority);
            db.AddInParameter(dbCommand, "From", DbType.String, From);
            db.AddInParameter(dbCommand, "FromName", DbType.String, FromName);
            db.AddInParameter(dbCommand, "To", DbType.String, To);
            db.AddInParameter(dbCommand, "ToName", DbType.String, ToName);
            db.AddInParameter(dbCommand, "Cc", DbType.String, Cc);
            db.AddInParameter(dbCommand, "Bcc", DbType.String, Bcc);
            db.AddInParameter(dbCommand, "Subject", DbType.String, Subject);
            db.AddInParameter(dbCommand, "Body", DbType.String, Body);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            db.AddInParameter(dbCommand, "SendTries", DbType.Int32, SendTries);
            if (SentOn.HasValue)
                db.AddInParameter(dbCommand, "SentOn", DbType.DateTime, SentOn.Value);
            else
                db.AddInParameter(dbCommand, "SentOn", DbType.DateTime, DBNull.Value);

            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int QueuedEmailID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@QueuedEmailID"));
                queuedEmail = GetQueuedEmailByID(QueuedEmailID);
            }
            return queuedEmail;
        }

        /// <summary>
        /// Updates a queued email
        /// </summary>
        /// <param name="QueuedEmailID">Email item identifier</param>
        /// <param name="Priority">The priority</param>
        /// <param name="From">From</param>
        /// <param name="FromName">From name</param>
        /// <param name="To">To</param>
        /// <param name="ToName">To name</param>
        /// <param name="Cc">Cc</param>
        /// <param name="Bcc">Bcc</param>
        /// <param name="Subject">Subject</param>
        /// <param name="Body">Body</param>
        /// <param name="CreatedOn">The date and time of item creation</param>
        /// <param name="SendTries">The send tries</param>
        /// <param name="SentOn">The sent date and time. Null if email is not sent yet</param>
        /// <returns>Queued email</returns>
        public override DBQueuedEmail UpdateQueuedEmail(int QueuedEmailID, int Priority,
            string From, string FromName, string To, string ToName, string Cc, string Bcc,
            string Subject, string Body, DateTime CreatedOn, int SendTries, DateTime? SentOn)
        {
            DBQueuedEmail queuedEmail = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_QueuedEmailUpdate");
            db.AddInParameter(dbCommand, "QueuedEmailID", DbType.Int32, QueuedEmailID);
            db.AddInParameter(dbCommand, "Priority", DbType.Int32, Priority);
            db.AddInParameter(dbCommand, "From", DbType.String, From);
            db.AddInParameter(dbCommand, "FromName", DbType.String, FromName);
            db.AddInParameter(dbCommand, "To", DbType.String, To);
            db.AddInParameter(dbCommand, "ToName", DbType.String, ToName);
            db.AddInParameter(dbCommand, "Cc", DbType.String, Cc);
            db.AddInParameter(dbCommand, "Bcc", DbType.String, Bcc);
            db.AddInParameter(dbCommand, "Subject", DbType.String, Subject);
            db.AddInParameter(dbCommand, "Body", DbType.String, Body);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            db.AddInParameter(dbCommand, "SendTries", DbType.Int32, SendTries);
            if (SentOn.HasValue)
                db.AddInParameter(dbCommand, "SentOn", DbType.DateTime, SentOn.Value);
            else
                db.AddInParameter(dbCommand, "SentOn", DbType.DateTime, DBNull.Value);

            if (db.ExecuteNonQuery(dbCommand) > 0)
                queuedEmail = GetQueuedEmailByID(QueuedEmailID);

            return queuedEmail;
        }
        #endregion
    }
}

