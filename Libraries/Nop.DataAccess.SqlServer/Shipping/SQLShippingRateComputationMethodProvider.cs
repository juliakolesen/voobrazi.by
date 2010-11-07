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
    /// Shipping rate computation method provider for SQL Server
    /// </summary>
    public partial class SQLShippingRateComputationMethodProvider : DBShippingRateComputationMethodProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBShippingRateComputationMethod GetShippingRateComputationMethodFromReader(IDataReader dataReader)
        {
            DBShippingRateComputationMethod shippingRateComputationMethod = new DBShippingRateComputationMethod();
            shippingRateComputationMethod.ShippingRateComputationMethodID = NopSqlDataHelper.GetInt(dataReader, "ShippingRateComputationMethodID");
            shippingRateComputationMethod.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            shippingRateComputationMethod.Description = NopSqlDataHelper.GetString(dataReader, "Description");
            shippingRateComputationMethod.ConfigureTemplatePath = NopSqlDataHelper.GetString(dataReader, "ConfigureTemplatePath");
            shippingRateComputationMethod.ClassName = NopSqlDataHelper.GetString(dataReader, "ClassName");
            shippingRateComputationMethod.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            return shippingRateComputationMethod;
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
        /// Deletes a shipping rate computation method
        /// </summary>
        /// <param name="ShippingRateComputationMethodID">Shipping rate computation method identifier</param>
        public override void DeleteShippingRateComputationMethod(int ShippingRateComputationMethodID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingRateComputationMethodDelete");
            db.AddInParameter(dbCommand, "ShippingRateComputationMethodID", DbType.Int32, ShippingRateComputationMethodID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a shipping rate computation method
        /// </summary>
        /// <param name="ShippingRateComputationMethodID">Shipping rate computation method identifier</param>
        /// <returns>Shipping rate computation method</returns>
        public override DBShippingRateComputationMethod GetShippingRateComputationMethodByID(int ShippingRateComputationMethodID)
        {

            DBShippingRateComputationMethod shippingRateComputationMethod = null;
            if (ShippingRateComputationMethodID == 0)
                return shippingRateComputationMethod;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingRateComputationMethodLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ShippingRateComputationMethodID", DbType.Int32, ShippingRateComputationMethodID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    shippingRateComputationMethod = GetShippingRateComputationMethodFromReader(dataReader);
                }
            }
            return shippingRateComputationMethod;
        }

        /// <summary>
        /// Gets all shipping rate computation methods
        /// </summary>
        /// <returns>Shipping rate computation method collection</returns>
        public override DBShippingRateComputationMethodCollection GetAllShippingRateComputationMethods()
        {
            DBShippingRateComputationMethodCollection shippingRateComputationMethodCollection = new DBShippingRateComputationMethodCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingRateComputationMethodLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBShippingRateComputationMethod shippingRateComputationMethod = GetShippingRateComputationMethodFromReader(dataReader);
                    shippingRateComputationMethodCollection.Add(shippingRateComputationMethod);
                }
            }
            return shippingRateComputationMethodCollection;
        }

        /// <summary>
        /// Inserts a shipping rate computation method
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="Description">The description</param>
        /// <param name="ConfigureTemplatePath">The configure template path</param>
        /// <param name="ClassName">The class name</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Shipping rate computation method</returns>
        public override DBShippingRateComputationMethod InsertShippingRateComputationMethod(string Name, string Description,
           string ConfigureTemplatePath, string ClassName, int DisplayOrder)
        {
            DBShippingRateComputationMethod shippingRateComputationMethod = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingRateComputationMethodInsert");
            db.AddOutParameter(dbCommand, "ShippingRateComputationMethodID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "Description", DbType.String, Description);
            db.AddInParameter(dbCommand, "ConfigureTemplatePath", DbType.String, ConfigureTemplatePath);
            db.AddInParameter(dbCommand, "ClassName", DbType.String, ClassName);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int ShippingRateComputationMethodID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ShippingRateComputationMethodID"));
                shippingRateComputationMethod = GetShippingRateComputationMethodByID(ShippingRateComputationMethodID);
            }
            return shippingRateComputationMethod;
        }

        /// <summary>
        /// Updates the shipping rate computation method
        /// </summary>
        /// <param name="ShippingRateComputationMethodID">The shipping rate computation method identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="Description">The description</param>
        /// <param name="ConfigureTemplatePath">The configure template path</param>
        /// <param name="ClassName">The class name</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Shipping rate computation method</returns>
        public override DBShippingRateComputationMethod UpdateShippingRateComputationMethod(int ShippingRateComputationMethodID, string Name, string Description,
           string ConfigureTemplatePath, string ClassName, int DisplayOrder)
        {
            DBShippingRateComputationMethod shippingRateComputationMethod = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingRateComputationMethodUpdate");
            db.AddInParameter(dbCommand, "ShippingRateComputationMethodID", DbType.Int32, ShippingRateComputationMethodID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "Description", DbType.String, Description);
            db.AddInParameter(dbCommand, "ConfigureTemplatePath", DbType.String, ConfigureTemplatePath);
            db.AddInParameter(dbCommand, "ClassName", DbType.String, ClassName);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                shippingRateComputationMethod = GetShippingRateComputationMethodByID(ShippingRateComputationMethodID);

            return shippingRateComputationMethod;
        }
        #endregion
    }
}
