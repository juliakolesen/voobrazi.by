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
    /// ShippingByWeight provider for SQL Server
    /// </summary>
    public partial class SQLShippingByWeightProvider : DBShippingByWeightProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBShippingByWeight GetShippingByWeightFromReader(IDataReader dataReader)
        {
            DBShippingByWeight shippingByWeight = new DBShippingByWeight();
            shippingByWeight.ShippingByWeightID = NopSqlDataHelper.GetInt(dataReader, "ShippingByWeightID");
            shippingByWeight.ShippingMethodID = NopSqlDataHelper.GetInt(dataReader, "ShippingMethodID");
            shippingByWeight.From = NopSqlDataHelper.GetDecimal(dataReader, "From");
            shippingByWeight.To = NopSqlDataHelper.GetDecimal(dataReader, "To");
            shippingByWeight.UsePercentage = NopSqlDataHelper.GetBoolean(dataReader, "UsePercentage");
            shippingByWeight.ShippingChargePercentage = NopSqlDataHelper.GetDecimal(dataReader, "ShippingChargePercentage");
            shippingByWeight.ShippingChargeAmount = NopSqlDataHelper.GetDecimal(dataReader, "ShippingChargeAmount");
            return shippingByWeight;
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
        /// Gets a ShippingByWeight
        /// </summary>
        /// <param name="ShippingByWeightID">ShippingByWeight identifier</param>
        /// <returns>ShippingByWeight</returns>
        public override DBShippingByWeight GetByID(int ShippingByWeightID)
        {
            DBShippingByWeight shippingByWeight = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingByWeightLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ShippingByWeightID", DbType.Int32, ShippingByWeightID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    shippingByWeight = GetShippingByWeightFromReader(dataReader);
                }
            }
            return shippingByWeight;
        }

        /// <summary>
        /// Deletes a ShippingByWeight
        /// </summary>
        /// <param name="ShippingByWeightID">ShippingByWeight identifier</param>
        public override void DeleteShippingByWeight(int ShippingByWeightID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingByWeightDelete");
            db.AddInParameter(dbCommand, "ShippingByWeightID", DbType.Int32, ShippingByWeightID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets all ShippingByWeights
        /// </summary>
        /// <returns>ShippingByWeight collection</returns>
        public override DBShippingByWeightCollection GetAll()
        {
            DBShippingByWeightCollection shippingByWeightCollection = new DBShippingByWeightCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingByWeightLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBShippingByWeight shippingByWeight = GetShippingByWeightFromReader(dataReader);
                    shippingByWeightCollection.Add(shippingByWeight);
                }
            }

            return shippingByWeightCollection;
        }

        /// <summary>
        /// Inserts a ShippingByWeight
        /// </summary>
        /// <param name="ShippingMethodID">The shipping method identifier</param>
        /// <param name="From">The "from" value</param>
        /// <param name="To">The "to" value</param>
        /// <param name="UsePercentage">A value indicating whether to use percentage</param>
        /// <param name="ShippingChargePercentage">The shipping charge percentage</param>
        /// <param name="ShippingChargeAmount">The shipping charge amount</param>
        /// <returns>ShippingByWeight</returns>
        public override DBShippingByWeight InsertShippingByWeight(int ShippingMethodID, decimal From, decimal To,
            bool UsePercentage, decimal ShippingChargePercentage, decimal ShippingChargeAmount)
        {
            DBShippingByWeight shippingByWeight = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingByWeightInsert");
            db.AddOutParameter(dbCommand, "ShippingByWeightID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "ShippingMethodID", DbType.Int32, ShippingMethodID);
            db.AddInParameter(dbCommand, "From", DbType.Decimal, From);
            db.AddInParameter(dbCommand, "To", DbType.Decimal, To);
            db.AddInParameter(dbCommand, "UsePercentage", DbType.Boolean, UsePercentage);
            db.AddInParameter(dbCommand, "ShippingChargePercentage", DbType.Decimal, ShippingChargePercentage);
            db.AddInParameter(dbCommand, "ShippingChargeAmount", DbType.Decimal, ShippingChargeAmount);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int ShippingByWeightID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ShippingByWeightID"));
                shippingByWeight = GetByID(ShippingByWeightID);
            }
            return shippingByWeight;
        }

        /// <summary>
        /// Updates the ShippingByWeight
        /// </summary>
        /// <param name="ShippingByWeightID">The ShippingByWeight identifier</param>
        /// <param name="ShippingMethodID">The shipping method identifier</param>
        /// <param name="From">The "from" value</param>
        /// <param name="To">The "to" value</param>
        /// <param name="UsePercentage">A value indicating whether to use percentage</param>
        /// <param name="ShippingChargePercentage">The shipping charge percentage</param>
        /// <param name="ShippingChargeAmount">The shipping charge amount</param>
        /// <returns>ShippingByWeight</returns>
        public override DBShippingByWeight UpdateShippingByWeight(int ShippingByWeightID, int ShippingMethodID, decimal From, decimal To,
            bool UsePercentage, decimal ShippingChargePercentage, decimal ShippingChargeAmount)
        {
            DBShippingByWeight shippingByWeight = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingByWeightUpdate");
            db.AddInParameter(dbCommand, "ShippingByWeightID", DbType.Int32, ShippingByWeightID);
            db.AddInParameter(dbCommand, "ShippingMethodID", DbType.Int32, ShippingMethodID);
            db.AddInParameter(dbCommand, "From", DbType.Decimal, From);
            db.AddInParameter(dbCommand, "To", DbType.Decimal, To);
            db.AddInParameter(dbCommand, "UsePercentage", DbType.Boolean, UsePercentage);
            db.AddInParameter(dbCommand, "ShippingChargePercentage", DbType.Decimal, ShippingChargePercentage);
            db.AddInParameter(dbCommand, "ShippingChargeAmount", DbType.Decimal, ShippingChargeAmount);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                shippingByWeight = GetByID(ShippingByWeightID);

            return shippingByWeight;
        }

        /// <summary>
        /// Gets all ShippingByWeights by shipping method identifier
        /// </summary>
        /// <param name="ShippingMethodID">The shipping method identifier</param>
        /// <returns>ShippingByWeight collection</returns>
        public override DBShippingByWeightCollection GetAllByShippingMethodID(int ShippingMethodID)
        {
            DBShippingByWeightCollection shippingByWeightCollection = new DBShippingByWeightCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingByWeightLoadByShippingMethodID");
            db.AddInParameter(dbCommand, "ShippingMethodID", DbType.Int32, ShippingMethodID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBShippingByWeight shippingByWeight = GetShippingByWeightFromReader(dataReader);
                    shippingByWeightCollection.Add(shippingByWeight);
                }
            }

            return shippingByWeightCollection;
        }
        #endregion
    }
}
