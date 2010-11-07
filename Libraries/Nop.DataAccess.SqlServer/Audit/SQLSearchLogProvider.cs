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
using System.Web;
using System.Collections.Specialized;
using System.Configuration.Provider;

namespace NopSolutions.NopCommerce.DataAccess.Audit
{
    /// <summary>
    /// Search log provider for SQL Server
    /// </summary>
    public partial class SQLSearchLogProvider : DBSearchLogProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBSearchLog GetSearchLogFromReader(IDataReader dataReader)
        {
            DBSearchLog searchLog = new DBSearchLog();
            searchLog.SearchLogID = NopSqlDataHelper.GetInt(dataReader, "SearchLogID");
            searchLog.SearchTerm = NopSqlDataHelper.GetString(dataReader, "SearchTerm");
            searchLog.CustomerID = NopSqlDataHelper.GetInt(dataReader, "CustomerID");
            searchLog.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            return searchLog;
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
        /// Get order product variant sales report
        /// </summary>
        /// <param name="StartTime">Order start time; null to load all</param>
        /// <param name="EndTime">Order end time; null to load all</param>
        /// <param name="Count">Item count. 0 if you want to get all items</param>
        /// <returns>Result</returns>
        public override IDataReader SearchTermReport(DateTime? StartTime, DateTime? EndTime, int Count)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SearchTermReport");
            if (StartTime.HasValue)
                db.AddInParameter(dbCommand, "StartTime", DbType.DateTime, StartTime.Value);
            else
                db.AddInParameter(dbCommand, "StartTime", DbType.DateTime, null);
            if (EndTime.HasValue)
                db.AddInParameter(dbCommand, "EndTime", DbType.DateTime, EndTime.Value);
            else
                db.AddInParameter(dbCommand, "EndTime", DbType.DateTime, null);

            db.AddInParameter(dbCommand, "Count", DbType.Int32, Count);
            return db.ExecuteReader(dbCommand);
        }

        /// <summary>
        /// Gets all search log items
        /// </summary>
        /// <returns>Search log collection</returns>
        public override DBSearchLogCollection GetAllSearchLogs()
        {
            DBSearchLogCollection searchLogCollection = new DBSearchLogCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SearchLogLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBSearchLog searchLog = GetSearchLogFromReader(dataReader);
                    searchLogCollection.Add(searchLog);
                }
            }

            return searchLogCollection;
        }

        /// <summary>
        /// Gets a search log item
        /// </summary>
        /// <param name="SearchLogID">The search log item identifier</param>
        /// <returns>Search log item</returns>
        public override DBSearchLog GetSearchLogByID(int SearchLogID)
        {
            DBSearchLog searchLog = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SearchLogLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "SearchLogID", DbType.Int32, SearchLogID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    searchLog = GetSearchLogFromReader(dataReader);
                }
            }
            return searchLog;
        }

        /// <summary>
        /// Inserts a search log item
        /// </summary>
        /// <param name="SearchTerm">The search term</param>
        /// <param name="CustomerID">The customer identifier</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>Search log item</returns>
        public override DBSearchLog InsertSearchLog(string SearchTerm, int CustomerID, DateTime CreatedOn)
        {
            DBSearchLog searchLog = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SearchLogInsert");
            db.AddOutParameter(dbCommand, "SearchLogID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "SearchTerm", DbType.String, SearchTerm);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, CustomerID);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int SearchLogID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@SearchLogID"));
                searchLog = GetSearchLogByID(SearchLogID);
            }

            return searchLog;
        }

        #endregion
    }
}
