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

namespace NopSolutions.NopCommerce.DataAccess.Media
{
    /// <summary>
    /// Download provider for SQL Server
    /// </summary>
    public partial class SQLDownloadProvider : DBDownloadProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBDownload GetDownloadFromReader(IDataReader dataReader)
        {
            DBDownload download = new DBDownload();
            download.DownloadID = NopSqlDataHelper.GetInt(dataReader, "DownloadID");
            download.UseDownloadURL = NopSqlDataHelper.GetBoolean(dataReader, "UseDownloadURL");
            download.DownloadURL = NopSqlDataHelper.GetString(dataReader, "DownloadURL");
            download.DownloadBinary = NopSqlDataHelper.GetBytes(dataReader, "DownloadBinary");
            download.ContentType = NopSqlDataHelper.GetString(dataReader, "ContentType");
            download.Extension = NopSqlDataHelper.GetString(dataReader, "Extension");
            download.IsNew = NopSqlDataHelper.GetBoolean(dataReader, "IsNew");
            return download;
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
        /// Gets a download
        /// </summary>
        /// <param name="DownloadID">Download identifier</param>
        /// <returns>Download</returns>
        public override DBDownload GetDownloadByID(int DownloadID)
        {
            DBDownload download = null;
            if (DownloadID == 0)
                return download;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_DownloadLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "DownloadID", DbType.Int32, DownloadID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    download = GetDownloadFromReader(dataReader);
                }
            }
            return download;
        }

        /// <summary>
        /// Deletes a download
        /// </summary>
        /// <param name="DownloadID">Download identifier</param>
        public override void DeleteDownload(int DownloadID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_DownloadDelete");
            db.AddInParameter(dbCommand, "DownloadID", DbType.Int32, DownloadID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Inserts a download
        /// </summary>
        /// <param name="UseDownloadURL">The value indicating whether DownloadURL property should be used</param>
        /// <param name="DownloadURL">The download URL</param>
        /// <param name="DownloadBinary">The download binary</param>
        /// <param name="ContentType">The mimi-type of the download</param>
        /// <param name="Extension">The extension</param>
        /// <param name="IsNew">A value indicating whether the download is new</param>
        /// <returns>Download</returns>
        public override DBDownload InsertDownload(bool UseDownloadURL, string DownloadURL, 
            byte[] DownloadBinary, string ContentType, string Extension, bool IsNew)
        {
            DBDownload download = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_DownloadInsert");
            db.AddOutParameter(dbCommand, "DownloadID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "UseDownloadURL", DbType.Boolean, UseDownloadURL);
            db.AddInParameter(dbCommand, "DownloadURL", DbType.String, DownloadURL);
            if (DownloadBinary != null)
                db.AddInParameter(dbCommand, "DownloadBinary", DbType.Binary, DownloadBinary);
            else
                db.AddInParameter(dbCommand, "DownloadBinary", DbType.Binary, null);
            db.AddInParameter(dbCommand, "ContentType", DbType.String, ContentType);
            db.AddInParameter(dbCommand, "Extension", DbType.String, Extension);
            db.AddInParameter(dbCommand, "IsNew", DbType.Boolean, IsNew);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int DownloadID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@DownloadID"));
                download = GetDownloadByID(DownloadID);
            }
            return download;
        }

        /// <summary>
        /// Updates the download
        /// </summary>
        /// <param name="DownloadID">The download identifier</param>
        /// <param name="UseDownloadURL">The value indicating whether DownloadURL property should be used</param>
        /// <param name="DownloadURL">The download URL</param>
        /// <param name="DownloadBinary">The download binary</param>
        /// <param name="ContentType">The mime-type of the download</param>
        /// <param name="Extension">The extension</param>
        /// <param name="IsNew">A value indicating whether the download is new</param>
        /// <returns>Download</returns>
        public override DBDownload UpdateDownload(int DownloadID, bool UseDownloadURL, string DownloadURL, 
            byte[] DownloadBinary, string ContentType, string Extension, bool IsNew)
        {
            DBDownload download = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_DownloadUpdate");
            db.AddInParameter(dbCommand, "DownloadID", DbType.Int32, DownloadID);
            db.AddInParameter(dbCommand, "UseDownloadURL", DbType.Boolean, UseDownloadURL);
            db.AddInParameter(dbCommand, "DownloadURL", DbType.String, DownloadURL);
            if (DownloadBinary != null)
                db.AddInParameter(dbCommand, "DownloadBinary", DbType.Binary, DownloadBinary);
            else
                db.AddInParameter(dbCommand, "DownloadBinary", DbType.Binary, null);
            db.AddInParameter(dbCommand, "ContentType", DbType.String, ContentType);
            db.AddInParameter(dbCommand, "Extension", DbType.String, Extension);
            db.AddInParameter(dbCommand, "IsNew", DbType.Boolean, IsNew);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                download = GetDownloadByID(DownloadID);

            return download;
        }
        #endregion
    }
}
