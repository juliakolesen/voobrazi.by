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
    /// ShippingByTotal provider for SQL Server
    /// </summary>
    public partial class SQLShippingByTotalProvider : DBShippingByTotalProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBShippingByTotal GetShippingByTotalFromReader(IDataReader dataReader)
        {
            DBShippingByTotal shippingByTotal = new DBShippingByTotal();
            shippingByTotal.ShippingByTotalID = NopSqlDataHelper.GetInt(dataReader, "ShippingByTotalID");
            shippingByTotal.ShippingMethodID = NopSqlDataHelper.GetInt(dataReader, "ShippingMethodID");
            shippingByTotal.From = NopSqlDataHelper.GetDecimal(dataReader, "From");
            shippingByTotal.To = NopSqlDataHelper.GetDecimal(dataReader, "To");
            shippingByTotal.UsePercentage = NopSqlDataHelper.GetBoolean(dataReader, "UsePercentage");
            shippingByTotal.ShippingChargePercentage = NopSqlDataHelper.GetDecimal(dataReader, "ShippingChargePercentage");
            shippingByTotal.ShippingChargeAmount = NopSqlDataHelper.GetDecimal(dataReader, "ShippingChargeAmount");
            return shippingByTotal;
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
        /// Get a ShippingByTotal
        /// </summary>
        /// <param name="ShippingByTotalID">ShippingByTotal identifier</param>
        /// <returns>ShippingByTotal</returns>
        public override DBShippingByTotal GetByID(int ShippingByTotalID)
        {
            DBShippingByTotal shippingByTotal = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingByTotalLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ShippingByTotalID", DbType.Int32, ShippingByTotalID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    shippingByTotal = GetShippingByTotalFromReader(dataReader);
                }
            }
            return shippingByTotal;
        }

        /// <summary>
        /// Deletes a ShippingByTotal
        /// </summary>
        /// <param name="ShippingByTotalID">ShippingByTotal identifier</param>
        public override void DeleteShippingByTotal(int ShippingByTotalID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingByTotalDelete");
            db.AddInParameter(dbCommand, "ShippingByTotalID", DbType.Int32, ShippingByTotalID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets all ShippingByTotals
        /// </summary>
        /// <returns>ShippingByTotal collection</returns>
        public override DBShippingByTotalCollection GetAll()
        {
            DBShippingByTotalCollection shippingByTotalCollection = new DBShippingByTotalCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingByTotalLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBShippingByTotal shippingByTotal = GetShippingByTotalFromReader(dataReader);
                    shippingByTotalCollection.Add(shippingByTotal);
                }
            }

            return shippingByTotalCollection;
        }

        /// <summary>
        /// Inserts a ShippingByTotal
        /// </summary>
        /// <param name="ShippingMethodID">The shipping method identifier</param>
        /// <param name="From">The "from" value</param>
        /// <param name="To">The "to" value</param>
        /// <param name="UsePercentage">A value indicating whether to use percentage</param>
        /// <param name="ShippingChargePercentage">The shipping charge percentage</param>
        /// <param name="ShippingChargeAmount">The shipping charge amount</param>
        /// <returns>ShippingByTotal</returns>
        public override DBShippingByTotal InsertShippingByTotal(int ShippingMethodID, decimal From, decimal To,
            bool UsePercentage, decimal ShippingChargePercentage, decimal ShippingChargeAmount)
        {
            DBShippingByTotal shippingByTotal = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingByTotalInsert");
            db.AddOutParameter(dbCommand, "ShippingByTotalID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "ShippingMethodID", DbType.Int32, ShippingMethodID);
            db.AddInParameter(dbCommand, "From", DbType.Decimal, From);
            db.AddInParameter(dbCommand, "To", DbType.Decimal, To);
            db.AddInParameter(dbCommand, "UsePercentage", DbType.Boolean, UsePercentage);
            db.AddInParameter(dbCommand, "ShippingChargePercentage", DbType.Decimal, ShippingChargePercentage);
            db.AddInParameter(dbCommand, "ShippingChargeAmount", DbType.Decimal, ShippingChargeAmount);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int ShippingByTotalID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ShippingByTotalID"));
                shippingByTotal = GetByID(ShippingByTotalID);
            }
            return shippingByTotal;
        }

        /// <summary>
        /// Updates the ShippingByTotal
        /// </summary>
        /// <param name="ShippingByTotalID">The ShippingByTotal identifier</param>
        /// <param name="ShippingMethodID">The shipping method identifier</param>
        /// <param name="From">The "from" value</param>
        /// <param name="To">The "to" value</param>
        /// <param name="UsePercentage">A value indicating whether to use percentage</param>
        /// <param name="ShippingChargePercentage">The shipping charge percentage</param>
        /// <param name="ShippingChargeAmount">The shipping charge amount</param>
        /// <returns>ShippingByTotal</returns>
        public override DBShippingByTotal UpdateShippingByTotal(int ShippingByTotalID, int ShippingMethodID, decimal From, decimal To,
            bool UsePercentage, decimal ShippingChargePercentage, decimal ShippingChargeAmount)
        {
            DBShippingByTotal shippingByTotal = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingByTotalUpdate");
            db.AddInParameter(dbCommand, "ShippingByTotalID", DbType.Int32, ShippingByTotalID);
            db.AddInParameter(dbCommand, "ShippingMethodID", DbType.Int32, ShippingMethodID);
            db.AddInParameter(dbCommand, "From", DbType.Decimal, From);
            db.AddInParameter(dbCommand, "To", DbType.Decimal, To);
            db.AddInParameter(dbCommand, "UsePercentage", DbType.Boolean, UsePercentage);
            db.AddInParameter(dbCommand, "ShippingChargePercentage", DbType.Decimal, ShippingChargePercentage);
            db.AddInParameter(dbCommand, "ShippingChargeAmount", DbType.Decimal, ShippingChargeAmount);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                shippingByTotal = GetByID(ShippingByTotalID);

            return shippingByTotal;
        }

        /// <summary>
        /// Gets all ShippingByTotals by shipping method identifier
        /// </summary>
        /// <param name="ShippingMethodID">The shipping method identifier</param>
        /// <returns>ShippingByTotal collection</returns>
        public override DBShippingByTotalCollection GetAllByShippingMethodID(int ShippingMethodID)
        {
            DBShippingByTotalCollection shippingByTotalCollection = new DBShippingByTotalCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingByTotalLoadByShippingMethodID");
            db.AddInParameter(dbCommand, "ShippingMethodID", DbType.Int32, ShippingMethodID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBShippingByTotal shippingByTotal = GetShippingByTotalFromReader(dataReader);
                    shippingByTotalCollection.Add(shippingByTotal);
                }
            }

            return shippingByTotalCollection;
        }
        #endregion
    }
}
