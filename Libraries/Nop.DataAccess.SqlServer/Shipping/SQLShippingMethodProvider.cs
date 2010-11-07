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
    /// Shipping method provider for SQL Server
    /// </summary>
    public partial class SQLShippingMethodProvider : DBShippingMethodProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBShippingMethod GetShippingMethodFromReader(IDataReader dataReader)
        {
            DBShippingMethod shippingMethod = new DBShippingMethod();
            shippingMethod.ShippingMethodID = NopSqlDataHelper.GetInt(dataReader, "ShippingMethodID");
            shippingMethod.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            shippingMethod.Description = NopSqlDataHelper.GetString(dataReader, "Description");
            shippingMethod.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            return shippingMethod;
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
        /// Deletes a shipping method
        /// </summary>
        /// <param name="ShippingMethodID">The shipping method identifier</param>
        public override void DeleteShippingMethod(int ShippingMethodID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingMethodDelete");
            db.AddInParameter(dbCommand, "ShippingMethodID", DbType.Int32, ShippingMethodID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a shipping method
        /// </summary>
        /// <param name="ShippingMethodID">The shipping method identifier</param>
        /// <returns>Shipping method</returns>
        public override DBShippingMethod GetShippingMethodByID(int ShippingMethodID)
        {

            DBShippingMethod shippingMethod = null;
            if (ShippingMethodID == 0)
                return shippingMethod;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingMethodLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ShippingMethodID", DbType.Int32, ShippingMethodID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    shippingMethod = GetShippingMethodFromReader(dataReader);
                }
            }
            return shippingMethod;
        }

        /// <summary>
        /// Gets all shipping methods
        /// </summary>
        /// <returns>Shipping method collection</returns>
        public override DBShippingMethodCollection GetAllShippingMethods()
        {
            DBShippingMethodCollection shippingMethodCollection = new DBShippingMethodCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingMethodLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBShippingMethod shippingMethod = GetShippingMethodFromReader(dataReader);
                    shippingMethodCollection.Add(shippingMethod);
                }
            }
            return shippingMethodCollection;
        }

        /// <summary>
        /// Inserts a shipping method
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="Description">The description</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Shipping method</returns>
        public override DBShippingMethod InsertShippingMethod(string Name, string Description, int DisplayOrder)
        {
            DBShippingMethod shippingMethod = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingMethodInsert");
            db.AddOutParameter(dbCommand, "ShippingMethodID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "Description", DbType.String, Description);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int ShippingMethodID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ShippingMethodID"));
                shippingMethod = GetShippingMethodByID(ShippingMethodID);
            }
            return shippingMethod;
        }

        /// <summary>
        /// Updates the shipping method
        /// </summary>
        /// <param name="ShippingMethodID">The shipping method identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="Description">The description</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Shipping method</returns>
        public override DBShippingMethod UpdateShippingMethod(int ShippingMethodID, string Name, string Description,
            int DisplayOrder)
        {
            DBShippingMethod shippingMethod = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingMethodUpdate");
            db.AddInParameter(dbCommand, "ShippingMethodID", DbType.Int32, ShippingMethodID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "Description", DbType.String, Description);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                shippingMethod = GetShippingMethodByID(ShippingMethodID);

            return shippingMethod;
        }
        #endregion
    }
}
