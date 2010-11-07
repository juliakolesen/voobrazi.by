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

namespace NopSolutions.NopCommerce.DataAccess.Shipping
{
    /// <summary>
    /// ShippingByWeightAndCountry provider for SQL Server
    /// </summary>
    public partial class SQLShippingByWeightAndCountryProvider : DBShippingByWeightAndCountryProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBShippingByWeightAndCountry GetShippingByWeightAndCountryFromReader(IDataReader dataReader)
        {
            DBShippingByWeightAndCountry shippingByWeightAndCountry = new DBShippingByWeightAndCountry();
            shippingByWeightAndCountry.ShippingByWeightAndCountryID = NopSqlDataHelper.GetInt(dataReader, "ShippingByWeightAndCountryID");
            shippingByWeightAndCountry.ShippingMethodID = NopSqlDataHelper.GetInt(dataReader, "ShippingMethodID");
            shippingByWeightAndCountry.CountryID = NopSqlDataHelper.GetInt(dataReader, "CountryID");
            shippingByWeightAndCountry.From = NopSqlDataHelper.GetDecimal(dataReader, "From");
            shippingByWeightAndCountry.To = NopSqlDataHelper.GetDecimal(dataReader, "To");
            shippingByWeightAndCountry.UsePercentage = NopSqlDataHelper.GetBoolean(dataReader, "UsePercentage");
            shippingByWeightAndCountry.ShippingChargePercentage = NopSqlDataHelper.GetDecimal(dataReader, "ShippingChargePercentage");
            shippingByWeightAndCountry.ShippingChargeAmount = NopSqlDataHelper.GetDecimal(dataReader, "ShippingChargeAmount");
            return shippingByWeightAndCountry;
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
        /// Gets a ShippingByWeightAndCountry
        /// </summary>
        /// <param name="ShippingByWeightAndCountryID">ShippingByWeightAndCountry identifier</param>
        /// <returns>ShippingByWeightAndCountry</returns>
        public override DBShippingByWeightAndCountry GetByID(int ShippingByWeightAndCountryID)
        {
            DBShippingByWeightAndCountry shippingByWeightAndCountry = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingByWeightAndCountryLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ShippingByWeightAndCountryID", DbType.Int32, ShippingByWeightAndCountryID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    shippingByWeightAndCountry = GetShippingByWeightAndCountryFromReader(dataReader);
                }
            }
            return shippingByWeightAndCountry;
        }

        /// <summary>
        /// Deletes a ShippingByWeightAndCountry
        /// </summary>
        /// <param name="ShippingByWeightAndCountryID">ShippingByWeightAndCountry identifier</param>
        public override void DeleteShippingByWeightAndCountry(int ShippingByWeightAndCountryID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingByWeightAndCountryDelete");
            db.AddInParameter(dbCommand, "ShippingByWeightAndCountryID", DbType.Int32, ShippingByWeightAndCountryID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets all ShippingByWeightAndCountrys
        /// </summary>
        /// <returns>ShippingByWeightAndCountry collection</returns>
        public override DBShippingByWeightAndCountryCollection GetAll()
        {
            DBShippingByWeightAndCountryCollection shippingByWeightAndCountryCollection = new DBShippingByWeightAndCountryCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingByWeightAndCountryLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBShippingByWeightAndCountry shippingByWeightAndCountry = GetShippingByWeightAndCountryFromReader(dataReader);
                    shippingByWeightAndCountryCollection.Add(shippingByWeightAndCountry);
                }
            }

            return shippingByWeightAndCountryCollection;
        }

        /// <summary>
        /// Inserts a ShippingByWeightAndCountry
        /// </summary>
        /// <param name="ShippingMethodID">The shipping method identifier</param>
        /// <param name="CountryID">The country identifier</param>
        /// <param name="From">The "from" value</param>
        /// <param name="To">The "to" value</param>
        /// <param name="UsePercentage">A value indicating whether to use percentage</param>
        /// <param name="ShippingChargePercentage">The shipping charge percentage</param>
        /// <param name="ShippingChargeAmount">The shipping charge amount</param>
        /// <returns>ShippingByWeightAndCountry</returns>
        public override DBShippingByWeightAndCountry InsertShippingByWeightAndCountry(int ShippingMethodID,
            int CountryID, decimal From, decimal To,
            bool UsePercentage, decimal ShippingChargePercentage, decimal ShippingChargeAmount)
        {
            DBShippingByWeightAndCountry shippingByWeightAndCountry = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingByWeightAndCountryInsert");
            db.AddOutParameter(dbCommand, "ShippingByWeightAndCountryID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "ShippingMethodID", DbType.Int32, ShippingMethodID);
            db.AddInParameter(dbCommand, "CountryID", DbType.Int32, CountryID);
            db.AddInParameter(dbCommand, "From", DbType.Decimal, From);
            db.AddInParameter(dbCommand, "To", DbType.Decimal, To);
            db.AddInParameter(dbCommand, "UsePercentage", DbType.Boolean, UsePercentage);
            db.AddInParameter(dbCommand, "ShippingChargePercentage", DbType.Decimal, ShippingChargePercentage);
            db.AddInParameter(dbCommand, "ShippingChargeAmount", DbType.Decimal, ShippingChargeAmount);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int ShippingByWeightAndCountryID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ShippingByWeightAndCountryID"));
                shippingByWeightAndCountry = GetByID(ShippingByWeightAndCountryID);
            }
            return shippingByWeightAndCountry;
        }

        /// <summary>
        /// Updates the ShippingByWeightAndCountry
        /// </summary>
        /// <param name="ShippingByWeightAndCountryID">The ShippingByWeightAndCountry identifier</param>
        /// <param name="ShippingMethodID">The shipping method identifier</param>
        /// <param name="CountryID">The country identifier</param>
        /// <param name="From">The "from" value</param>
        /// <param name="To">The "to" value</param>
        /// <param name="UsePercentage">A value indicating whether to use percentage</param>
        /// <param name="ShippingChargePercentage">The shipping charge percentage</param>
        /// <param name="ShippingChargeAmount">The shipping charge amount</param>
        /// <returns>ShippingByWeightAndCountry</returns>
        public override DBShippingByWeightAndCountry UpdateShippingByWeightAndCountry(int ShippingByWeightAndCountryID,
            int ShippingMethodID, int CountryID, decimal From, decimal To, bool UsePercentage, decimal ShippingChargePercentage, decimal ShippingChargeAmount)
        {
            DBShippingByWeightAndCountry shippingByWeightAndCountry = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingByWeightAndCountryUpdate");
            db.AddInParameter(dbCommand, "ShippingByWeightAndCountryID", DbType.Int32, ShippingByWeightAndCountryID);
            db.AddInParameter(dbCommand, "ShippingMethodID", DbType.Int32, ShippingMethodID);
            db.AddInParameter(dbCommand, "CountryID", DbType.Int32, CountryID);
            db.AddInParameter(dbCommand, "From", DbType.Decimal, From);
            db.AddInParameter(dbCommand, "To", DbType.Decimal, To);
            db.AddInParameter(dbCommand, "UsePercentage", DbType.Boolean, UsePercentage);
            db.AddInParameter(dbCommand, "ShippingChargePercentage", DbType.Decimal, ShippingChargePercentage);
            db.AddInParameter(dbCommand, "ShippingChargeAmount", DbType.Decimal, ShippingChargeAmount);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                shippingByWeightAndCountry = GetByID(ShippingByWeightAndCountryID);

            return shippingByWeightAndCountry;
        }

        /// <summary>
        /// Gets all ShippingByWeightAndCountrys by shipping method identifier
        /// </summary>
        /// <param name="ShippingMethodID">The shipping method identifier</param>
        /// <param name="CountryID">The country identifier</param>
        /// <returns>ShippingByWeightAndCountry collection</returns>
        public override DBShippingByWeightAndCountryCollection GetAllByShippingMethodIDAndCountryID(int ShippingMethodID, int CountryID)
        {
            DBShippingByWeightAndCountryCollection shippingByWeightAndCountryCollection = new DBShippingByWeightAndCountryCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingByWeightAndCountryLoadByShippingMethodIDAndCountryID");
            db.AddInParameter(dbCommand, "ShippingMethodID", DbType.Int32, ShippingMethodID);
            db.AddInParameter(dbCommand, "CountryID", DbType.Int32, CountryID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBShippingByWeightAndCountry shippingByWeightAndCountry = GetShippingByWeightAndCountryFromReader(dataReader);
                    shippingByWeightAndCountryCollection.Add(shippingByWeightAndCountry);
                }
            }

            return shippingByWeightAndCountryCollection;
        }
        #endregion
    }
}
