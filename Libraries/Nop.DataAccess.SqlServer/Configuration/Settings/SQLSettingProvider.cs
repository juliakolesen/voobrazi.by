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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace NopSolutions.NopCommerce.DataAccess.Configuration.Settings
{
    /// <summary>
    /// Category provider for SQL Server
    /// </summary>
    public partial class SQLSettingProvider : DBSettingProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBSetting GetSettingFromReader(IDataReader dataReader)
        {
            DBSetting setting = new DBSetting();
            setting.SettingID = NopSqlDataHelper.GetInt(dataReader, "SettingID");
            setting.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            setting.Value = NopSqlDataHelper.GetString(dataReader, "Value");
            setting.Description = NopSqlDataHelper.GetString(dataReader, "Description");
            return setting;
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
        /// Gets a setting
        /// </summary>
        /// <param name="SettingID">Setting identifer</param>
        /// <returns>Setting</returns>
        public override DBSetting GetSettingByID(int SettingID)
        {
            DBSetting setting = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SettingLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "SettingID", DbType.Int32, SettingID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    setting = GetSettingFromReader(dataReader);
                }
            }

            return setting;
        }

        /// <summary>
        /// Deletes a setting
        /// </summary>
        /// <param name="SettingID">Setting identifer</param>
        public override void DeleteSetting(int SettingID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);

            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SettingDelete");
            db.AddInParameter(dbCommand, "SettingID", DbType.Int32, SettingID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets all settings
        /// </summary>
        /// <returns>Setting collection</returns>
        public override DBSettingCollection GetAllSettings()
        {
            DBSettingCollection settingCollection = new DBSettingCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SettingLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBSetting setting = GetSettingFromReader(dataReader);
                    settingCollection.Add(setting);
                }
            }
            return settingCollection;
        }

        /// <summary>
        /// Adds a setting
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="Value">The value</param>
        /// <param name="Description">The description</param>
        /// <returns>Setting</returns>
        public override DBSetting AddSetting(string Name, string Value, string Description)
        {
            DBSetting setting = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SettingInsert");
            db.AddOutParameter(dbCommand, "SettingID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "Value", DbType.String, Value);
            db.AddInParameter(dbCommand, "Description", DbType.String, Description);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int SettingID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@SettingID"));
                setting = GetSettingByID(SettingID);
            }
            return setting;
        }

        /// <summary>
        /// Updates a setting
        /// </summary>
        /// <param name="SettingID">Setting identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="Value">The value</param>
        /// <param name="Description">The description</param>
        /// <returns>Setting</returns>
        public override DBSetting UpdateSetting(int SettingID, string Name, string Value, string Description)
        {
            DBSetting setting = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SettingUpdate");
            db.AddInParameter(dbCommand, "SettingID", DbType.Int32, SettingID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "Value", DbType.String, Value);
            db.AddInParameter(dbCommand, "Description", DbType.String, Description);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                setting = GetSettingByID(SettingID);

            return setting;
        }
        #endregion
    }
}
