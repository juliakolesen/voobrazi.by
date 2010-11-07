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

namespace NopSolutions.NopCommerce.DataAccess.Tax
{
    /// <summary>
    /// Tax rate provider for SQL Server
    /// </summary>
    public partial class SQLTaxRateProvider : DBTaxRateProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBTaxRate GetTaxRateFromReader(IDataReader dataReader)
        {
            DBTaxRate taxRate = new DBTaxRate();
            taxRate.TaxRateID = NopSqlDataHelper.GetInt(dataReader, "TaxRateID");
            taxRate.TaxCategoryID = NopSqlDataHelper.GetInt(dataReader, "TaxCategoryID");
            taxRate.CountryID = NopSqlDataHelper.GetInt(dataReader, "CountryID");
            taxRate.StateProvinceID = NopSqlDataHelper.GetInt(dataReader, "StateProvinceID");
            taxRate.Zip = NopSqlDataHelper.GetString(dataReader, "Zip");
            taxRate.Percentage = NopSqlDataHelper.GetDecimal(dataReader, "Percentage");
            return taxRate;
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
        /// Deletes a tax rate
        /// </summary>
        /// <param name="TaxRateID">Tax rate identifier</param>
        public override void DeleteTaxRate(int TaxRateID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TaxRateDelete");
            db.AddInParameter(dbCommand, "TaxRateID", DbType.Int32, TaxRateID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a tax rate
        /// </summary>
        /// <param name="TaxRateID">Tax rate identifier</param>
        /// <returns>Tax rate</returns>
        public override DBTaxRate GetTaxRateByID(int TaxRateID)
        {
            DBTaxRate taxRate = null;
            if (TaxRateID == 0)
                return taxRate;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TaxRateLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "TaxRateID", DbType.Int32, TaxRateID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    taxRate = GetTaxRateFromReader(dataReader);
                }
            }
            return taxRate;
        }

        /// <summary>
        /// Gets all tax rates
        /// </summary>
        /// <returns>Tax rate collection</returns>
        public override DBTaxRateCollection GetAllTaxRates()
        {
            DBTaxRateCollection taxRateCollection = new DBTaxRateCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TaxRateLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBTaxRate taxRate = GetTaxRateFromReader(dataReader);
                    taxRateCollection.Add(taxRate);
                }
            }
            return taxRateCollection;
        }

        /// <summary>
        /// Inserts a tax rate
        /// </summary>
        /// <param name="TaxCategoryID">The tax category identifier</param>
        /// <param name="CountryID">The country identifier</param>
        /// <param name="StateProvinceID">The state/province identifier</param>
        /// <param name="Zip">The zip</param>
        /// <param name="Percentage">The percentage</param>
        /// <returns>Tax rate</returns>
        public override DBTaxRate InsertTaxRate(int TaxCategoryID, int CountryID,
            int StateProvinceID, string Zip, decimal Percentage)
        {
            DBTaxRate taxRate = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TaxRateInsert");
            db.AddOutParameter(dbCommand, "TaxRateID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "TaxCategoryID", DbType.Int32, TaxCategoryID);
            db.AddInParameter(dbCommand, "CountryID", DbType.Int32, CountryID);
            db.AddInParameter(dbCommand, "StateProvinceID", DbType.Int32, StateProvinceID);
            db.AddInParameter(dbCommand, "Zip", DbType.String, Zip);
            db.AddInParameter(dbCommand, "Percentage", DbType.Decimal, Percentage);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int TaxRateID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@TaxRateID"));
                taxRate = GetTaxRateByID(TaxRateID);
            }
            return taxRate;
        }

        /// <summary>
        /// Updates the tax rate
        /// </summary>
        /// <param name="TaxRateID">The tax rate identifier</param>
        /// <param name="TaxCategoryID">The tax category identifier</param>
        /// <param name="CountryID">The country identifier</param>
        /// <param name="StateProvinceID">The state/province identifier</param>
        /// <param name="Zip">The zip</param>
        /// <param name="Percentage">The percentage</param>
        /// <returns>Tax rate</returns>
        public override DBTaxRate UpdateTaxRate(int TaxRateID, int TaxCategoryID, int CountryID,
            int StateProvinceID, string Zip, decimal Percentage)
        {
            DBTaxRate taxRate = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TaxRateUpdate");
            db.AddInParameter(dbCommand, "TaxRateID", DbType.Int32, TaxRateID);
            db.AddInParameter(dbCommand, "TaxCategoryID", DbType.Int32, TaxCategoryID);
            db.AddInParameter(dbCommand, "CountryID", DbType.Int32, CountryID);
            db.AddInParameter(dbCommand, "StateProvinceID", DbType.Int32, StateProvinceID);
            db.AddInParameter(dbCommand, "Zip", DbType.String, Zip);
            db.AddInParameter(dbCommand, "Percentage", DbType.Decimal, Percentage);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                taxRate = GetTaxRateByID(TaxRateID);

            return taxRate;
        }
        #endregion
    }
}
