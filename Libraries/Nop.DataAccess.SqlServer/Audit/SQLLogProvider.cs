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

namespace NopSolutions.NopCommerce.DataAccess.Audit
{
    /// <summary>
    /// Log provider for SQL Server
    /// </summary>
    public partial class SQLLogProvider : DBLogProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBLog GetLogFromReader(IDataReader dataReader)
        {
            DBLog log = new DBLog();
            log.LogID = NopSqlDataHelper.GetInt(dataReader, "LogID");
            log.LogTypeID = NopSqlDataHelper.GetInt(dataReader, "LogTypeID");
            log.Severity = NopSqlDataHelper.GetInt(dataReader, "Severity");
            log.Message = NopSqlDataHelper.GetString(dataReader, "Message");
            log.Exception = NopSqlDataHelper.GetString(dataReader, "Exception");
            log.IPAddress = NopSqlDataHelper.GetString(dataReader, "IPAddress");
            log.CustomerID = NopSqlDataHelper.GetInt(dataReader, "CustomerID");
            log.PageURL = NopSqlDataHelper.GetString(dataReader, "PageURL");
            log.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            return log;
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
        /// Deletes a log item
        /// </summary>
        /// <param name="LogID">Log item identifier</param>
        public override void DeleteLog(int LogID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_LogDelete");
            db.AddInParameter(dbCommand, "LogID", DbType.Int32, LogID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Clears a log
        /// </summary>
        public override void ClearLog()
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_LogClear");
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets all log items
        /// </summary>
        /// <returns>Log item collection</returns>
        public override DBLogCollection GetAllLogs()
        {
            DBLogCollection logCollection = new DBLogCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_LogLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBLog log = GetLogFromReader(dataReader);
                    logCollection.Add(log);
                }
            }

            return logCollection;
        }

        /// <summary>
        /// Gets a log item
        /// </summary>
        /// <param name="LogID">Log item identifier</param>
        /// <returns>Log item</returns>
        public override DBLog GetLogByID(int LogID)
        {
            DBLog log = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_LogLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "LogID", DbType.Int32, LogID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    log = GetLogFromReader(dataReader);
                }
            }
            return log;
        }

        /// <summary>
        /// Inserts a log item
        /// </summary>
        /// <param name="LogTypeID">Log item type identifier</param>
        /// <param name="Severity">The severity</param>
        /// <param name="Message">The short message</param>
        /// <param name="Exception">The full exception</param>
        /// <param name="IPAddress">The IP address</param>
        /// <param name="CustomerID">The customer identifier</param>
        /// <param name="PageURL">The page URL</param>
        /// <param name="CreatedOn">The date and time of instance creationL</param>
        /// <returns>Log item</returns>
        public override DBLog InsertLog(int LogTypeID, int Severity, string Message,
            string Exception, string IPAddress, int CustomerID, string PageURL, DateTime CreatedOn)
        {
            DBLog log = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_LogInsert");
            db.AddOutParameter(dbCommand, "LogID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "LogTypeID", DbType.Int32, LogTypeID);
            db.AddInParameter(dbCommand, "Severity", DbType.Int32, Severity);
            db.AddInParameter(dbCommand, "Message", DbType.String, Message);
            db.AddInParameter(dbCommand, "Exception", DbType.String, Exception);
            db.AddInParameter(dbCommand, "IPAddress", DbType.String, IPAddress);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, CustomerID);
            db.AddInParameter(dbCommand, "PageURL", DbType.String, PageURL);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int LogID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@LogID"));
                log = GetLogByID(LogID);
            }
            return log;
        }
        #endregion
    }
}
