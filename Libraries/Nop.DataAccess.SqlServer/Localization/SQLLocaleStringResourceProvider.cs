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

namespace NopSolutions.NopCommerce.DataAccess.Localization
{
    /// <summary>
    /// Locale string resource provider for SQL Server
    /// </summary>
    public partial class SQLLocaleStringResourceProvider : DBLocaleStringResourceProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBLocaleStringResource GetLocaleStringResourceFromReader(IDataReader dataReader)
        {
            DBLocaleStringResource localeStringResource = new DBLocaleStringResource();
            localeStringResource.LocaleStringResourceID = NopSqlDataHelper.GetInt(dataReader, "LocaleStringResourceID");
            localeStringResource.LanguageID = NopSqlDataHelper.GetInt(dataReader, "LanguageID");
            localeStringResource.ResourceName = NopSqlDataHelper.GetString(dataReader, "ResourceName");
            localeStringResource.ResourceValue = NopSqlDataHelper.GetString(dataReader, "ResourceValue");
            return localeStringResource;
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
        /// Deletes a locale string resource
        /// </summary>
        /// <param name="LocaleStringResourceID">Locale string resource identifier</param>
        public override void DeleteLocaleStringResource(int LocaleStringResourceID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_LocaleStringResourceDelete");
            db.AddInParameter(dbCommand, "LocaleStringResourceID", DbType.Int32, LocaleStringResourceID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a locale string resource
        /// </summary>
        /// <param name="LocaleStringResourceID">Locale string resource identifier</param>
        /// <returns>Locale string resource</returns>
        public override DBLocaleStringResource GetLocaleStringResourceByID(int LocaleStringResourceID)
        {
            DBLocaleStringResource localeStringResource = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_LocaleStringResourceLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "LocaleStringResourceID", DbType.Int32, LocaleStringResourceID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    localeStringResource = GetLocaleStringResourceFromReader(dataReader);
                }
            }
            return localeStringResource;
        }

        /// <summary>
        /// Gets all locale string resources by language identifier
        /// </summary>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Locale string resource collection</returns>
        public override DBLocaleStringResourceCollection GetAllResourcesByLanguageID(int LanguageID)
        {
            DBLocaleStringResourceCollection localeStringResourceCollection = new DBLocaleStringResourceCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_LocaleStringResourceLoadAllByLanguageID");
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBLocaleStringResource localeStringResource = GetLocaleStringResourceFromReader(dataReader);
                    localeStringResourceCollection.Add(localeStringResource);
                }
            }

            return localeStringResourceCollection;
        }

        /// <summary>
        /// Inserts a locale string resource
        /// </summary>
        /// <param name="LanguageID">The language identifier</param>
        /// <param name="ResourceName">The resource name</param>
        /// <param name="ResourceValue">The resource value</param>
        /// <returns>Locale string resource</returns>
        public override DBLocaleStringResource InsertLocaleStringResource(int LanguageID, string ResourceName, string ResourceValue)
        {
            DBLocaleStringResource localeStringResource = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_LocaleStringResourceInsert");
            db.AddOutParameter(dbCommand, "LocaleStringResourceID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
            db.AddInParameter(dbCommand, "ResourceName", DbType.String, ResourceName);
            db.AddInParameter(dbCommand, "ResourceValue", DbType.String, ResourceValue);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int LocaleStringResourceID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@LocaleStringResourceID"));
                localeStringResource = GetLocaleStringResourceByID(LocaleStringResourceID);
            }
            return localeStringResource;
        }

        /// <summary>
        /// Updates the locale string resource
        /// </summary>
        /// <param name="LocaleStringResourceID">The locale string resource identifier</param>
        /// <param name="LanguageID">The language identifier</param>
        /// <param name="ResourceName">The resource name</param>
        /// <param name="ResourceValue">The resource value</param>
        /// <returns>Locale string resource</returns>
        public override DBLocaleStringResource UpdateLocaleStringResource(int LocaleStringResourceID, int LanguageID, string ResourceName, string ResourceValue)
        {
            DBLocaleStringResource localeStringResource = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_LocaleStringResourceUpdate");
            db.AddInParameter(dbCommand, "LocaleStringResourceID", DbType.Int32, LocaleStringResourceID);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
            db.AddInParameter(dbCommand, "ResourceName", DbType.String, ResourceName);
            db.AddInParameter(dbCommand, "ResourceValue", DbType.String, ResourceValue);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                localeStringResource = GetLocaleStringResourceByID(LocaleStringResourceID);

            return localeStringResource;
        }
        #endregion
    }
}
