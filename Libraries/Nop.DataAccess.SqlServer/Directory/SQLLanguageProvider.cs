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

namespace NopSolutions.NopCommerce.DataAccess.Directory
{
    /// <summary>
    /// Language provider for SQL Server
    /// </summary>
    public partial class SQLLanguageProvider : DBLanguageProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBLanguage GetLanguageFromReader(IDataReader dataReader)
        {
            DBLanguage language = new DBLanguage();
            language.LanguageID = NopSqlDataHelper.GetInt(dataReader, "LanguageID");
            language.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            language.LanguageCulture = NopSqlDataHelper.GetString(dataReader, "LanguageCulture");
            language.Published = NopSqlDataHelper.GetBoolean(dataReader, "Published");
            language.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            return language;
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
        /// Deletes a language
        /// </summary>
        /// <param name="LanguageID">Language identifier</param>
        public override void DeleteLanguage(int LanguageID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_LanguageDelete");
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets all languages
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Language collection</returns>
        public override DBLanguageCollection GetAllLanguages(bool showHidden)
        {
            DBLanguageCollection languageCollection = new DBLanguageCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_LanguageLoadAll");
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBLanguage language = GetLanguageFromReader(dataReader);
                    languageCollection.Add(language);
                }
            }
            return languageCollection;
        }

        /// <summary>
        /// Gets a language
        /// </summary>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Language</returns>
        public override DBLanguage GetLanguageByID(int LanguageID)
        {
            DBLanguage language = null;
            if (LanguageID == 0)
                return language;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_LanguageLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    language = GetLanguageFromReader(dataReader);
                }
            }

            return language;
        }

        /// <summary>
        /// Inserts a language
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="LanguageCulture">The language culture</param>
        /// <param name="Published">A value indicating whether the language is published</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Language</returns>
        public override DBLanguage InsertLanguage(string Name, string LanguageCulture, bool Published, int DisplayOrder)
        {
            DBLanguage language = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_LanguageInsert");
            db.AddOutParameter(dbCommand, "LanguageID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "LanguageCulture", DbType.String, LanguageCulture);
            db.AddInParameter(dbCommand, "Published", DbType.Boolean, Published);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int LanguageID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@LanguageID"));
                language = GetLanguageByID(LanguageID);
            }

            return language;
        }

        /// <summary>
        /// Updates a language
        /// </summary>
        /// <param name="LanguageID">Language identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="LanguageCulture">The language culture</param>
        /// <param name="Published">A value indicating whether the language is published</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Language</returns>
        public override DBLanguage UpdateLanguage(int LanguageID, string Name, string LanguageCulture, bool Published, int DisplayOrder)
        {
            DBLanguage language = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_LanguageUpdate");
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "LanguageCulture", DbType.String, LanguageCulture);
            db.AddInParameter(dbCommand, "Published", DbType.Boolean, Published);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                language = GetLanguageByID(LanguageID);

            return language;
        }
        #endregion
    }
}
