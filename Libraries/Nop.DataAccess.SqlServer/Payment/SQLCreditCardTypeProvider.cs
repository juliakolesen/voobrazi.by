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

namespace NopSolutions.NopCommerce.DataAccess.Payment
{
    /// <summary>
    /// Credit card type provider for SQL Server
    /// </summary>
    public partial class SQLCreditCardTypeProvider : DBCreditCardTypeProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBCreditCardType GetCreditCardTypeFromReader(IDataReader dataReader)
        {
            DBCreditCardType creditCardType = new DBCreditCardType();
            creditCardType.CreditCardTypeID = NopSqlDataHelper.GetInt(dataReader, "CreditCardTypeID");
            creditCardType.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            creditCardType.SystemKeyword = NopSqlDataHelper.GetString(dataReader, "SystemKeyword");
            creditCardType.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            creditCardType.Deleted = NopSqlDataHelper.GetBoolean(dataReader, "Deleted");
            return creditCardType;
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
        /// Gets a credit card type
        /// </summary>
        /// <param name="CreditCardTypeID">Credit card type identifier</param>
        /// <returns>Credit card type</returns>
        public override DBCreditCardType GetCreditCardTypeByID(int CreditCardTypeID)
        {
            DBCreditCardType creditCardType = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CreditCardTypeLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "CreditCardTypeID", DbType.Int32, CreditCardTypeID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    creditCardType = GetCreditCardTypeFromReader(dataReader);
                }
            }
            return creditCardType;
        }

        /// <summary>
        /// Gets all credit card types
        /// </summary>
        /// <returns>Credit card type collection</returns>
        public override DBCreditCardTypeCollection GetAllCreditCardTypes()
        {

            DBCreditCardTypeCollection creditCardTypeCollection = new DBCreditCardTypeCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CreditCardTypeLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBCreditCardType creditCardType = GetCreditCardTypeFromReader(dataReader);
                    creditCardTypeCollection.Add(creditCardType);
                }
            }

            return creditCardTypeCollection;
        }

        /// <summary>
        /// Inserts a credit card type
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="SystemKeyword">The system keyword</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="Deleted">A value indicating whether the entity has been deleted</param>
        /// <returns>A credit card type</returns>
        public override DBCreditCardType InsertCreditCardType(string Name, string SystemKeyword, int DisplayOrder, bool Deleted)
        {
            DBCreditCardType creditCardType = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CreditCardTypeInsert");
            db.AddOutParameter(dbCommand, "CreditCardTypeID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "SystemKeyword", DbType.String, SystemKeyword);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, Deleted);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int CreditCardTypeID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@CreditCardTypeID"));
                creditCardType = GetCreditCardTypeByID(CreditCardTypeID);
            }
            return creditCardType;
        }

        /// <summary>
        /// Updates the credit card type
        /// </summary>
        /// <param name="CreditCardTypeID">Credit card type identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="SystemKeyword">The system keyword</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="Deleted">A value indicating whether the entity has been deleted</param>
        /// <returns>A credit card type</returns>
        public override DBCreditCardType UpdateCreditCardType(int CreditCardTypeID, string Name, string SystemKeyword,
            int DisplayOrder, bool Deleted)
        {
            DBCreditCardType creditCardType = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CreditCardTypeUpdate");
            db.AddInParameter(dbCommand, "CreditCardTypeID", DbType.Int32, CreditCardTypeID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "SystemKeyword", DbType.String, SystemKeyword);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, Deleted);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                creditCardType = GetCreditCardTypeByID(CreditCardTypeID);

            return creditCardType;
        }
        #endregion
    }
}
