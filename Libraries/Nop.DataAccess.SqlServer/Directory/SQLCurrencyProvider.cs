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
using System.Collections.Specialized;
using System.Configuration.Provider;

namespace NopSolutions.NopCommerce.DataAccess.Directory
{
    /// <summary>
    /// Currency provider for SQL Server
    /// </summary>
    public partial class SQLCurrencyProvider : DBCurrencyProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBCurrency GetCurrencyFromReader(IDataReader dataReader)
        {
            DBCurrency currency = new DBCurrency();
            currency.CurrencyID = NopSqlDataHelper.GetInt(dataReader, "CurrencyID");
            currency.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            currency.CurrencyCode = NopSqlDataHelper.GetString(dataReader, "CurrencyCode");
            currency.Rate = NopSqlDataHelper.GetDecimal(dataReader, "Rate");
            currency.DisplayLocale = NopSqlDataHelper.GetString(dataReader, "DisplayLocale");
            currency.CustomFormatting = NopSqlDataHelper.GetString(dataReader, "CustomFormatting");
            currency.Published = NopSqlDataHelper.GetBoolean(dataReader, "Published");
            currency.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            currency.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            currency.UpdatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "UpdatedOn");
            return currency;
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
        /// Deletes currency
        /// </summary>
        /// <param name="CurrencyID">Currency identifier</param>
        public override void DeleteCurrency(int CurrencyID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CurrencyDelete");
            db.AddInParameter(dbCommand, "CurrencyID", DbType.Int32, CurrencyID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a currency
        /// </summary>
        /// <param name="CurrencyID">Currency identifier</param>
        /// <returns>Currency</returns>
        public override DBCurrency GetCurrencyByID(int CurrencyID)
        {
            DBCurrency currency = null;
            if (CurrencyID == 0)
                return currency;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CurrencyLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "CurrencyID", DbType.Int32, CurrencyID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    currency = GetCurrencyFromReader(dataReader);
                }
            }
            return currency;
        }

        /// <summary>
        /// Gets all currencies
        /// </summary>
        /// <returns>Currency collection</returns>
        public override DBCurrencyCollection GetAllCurrencies(bool showHidden)
        {
            DBCurrencyCollection currencyCollection = new DBCurrencyCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CurrencyLoadAll");
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBCurrency currency = GetCurrencyFromReader(dataReader);
                    currencyCollection.Add(currency);
                }
            }

            return currencyCollection;
        }

        /// <summary>
        /// Inserts a currency
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="CurrencyCode">The currency code</param>
        /// <param name="Rate">The rate</param>
        /// <param name="DisplayLocale">The display locale</param>
        /// <param name="CustomFormatting">The custom formatting</param>
        /// <param name="Published">A value indicating whether the entity is published</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>A currency</returns>
        public override DBCurrency InsertCurrency(string Name, string CurrencyCode, decimal Rate,
           string DisplayLocale, string CustomFormatting, bool Published, int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn)
        {
            DBCurrency currency = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CurrencyInsert");
            db.AddOutParameter(dbCommand, "CurrencyID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "CurrencyCode", DbType.String, CurrencyCode);
            db.AddInParameter(dbCommand, "Rate", DbType.Decimal, Rate);
            db.AddInParameter(dbCommand, "DisplayLocale", DbType.String, DisplayLocale);
            db.AddInParameter(dbCommand, "CustomFormatting", DbType.String, CustomFormatting);
            db.AddInParameter(dbCommand, "Published", DbType.Boolean, Published);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, UpdatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int CurrencyID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@CurrencyID"));
                currency = GetCurrencyByID(CurrencyID);
            }
            return currency;
        }

        /// <summary>
        /// Updates the currency
        /// </summary>
        /// <param name="CurrencyID">Currency identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="CurrencyCode">The currency code</param>
        /// <param name="Rate">The rate</param>
        /// <param name="DisplayLocale">The display locale</param>
        /// <param name="CustomFormatting">The custom formatting</param>
        /// <param name="Published">A value indicating whether the entity is published</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>A currency</returns>
        public override DBCurrency UpdateCurrency(int CurrencyID, string Name, string CurrencyCode, decimal Rate,
           string DisplayLocale, string CustomFormatting, bool Published, int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn)
        {
            DBCurrency currency = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CurrencyUpdate");
            db.AddInParameter(dbCommand, "CurrencyID", DbType.Int32, CurrencyID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "CurrencyCode", DbType.String, CurrencyCode);
            db.AddInParameter(dbCommand, "Rate", DbType.Decimal, Rate);
            db.AddInParameter(dbCommand, "DisplayLocale", DbType.String, DisplayLocale);
            db.AddInParameter(dbCommand, "CustomFormatting", DbType.String, CustomFormatting);
            db.AddInParameter(dbCommand, "Published", DbType.Boolean, Published);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, UpdatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                currency = GetCurrencyByID(CurrencyID);

            return currency;
        }
        #endregion
    }
}
