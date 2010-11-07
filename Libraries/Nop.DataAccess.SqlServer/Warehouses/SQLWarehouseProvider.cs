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

namespace NopSolutions.NopCommerce.DataAccess.Warehouses
{
    /// <summary>
    /// Warehouse provider for SQL Server
    /// </summary>
    public partial class SQLWarehouseProvider : DBWarehouseProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBWarehouse GetWarehouseFromReader(IDataReader dataReader)
        {
            DBWarehouse warehouse = new DBWarehouse();
            warehouse.WarehouseID = NopSqlDataHelper.GetInt(dataReader, "WarehouseID");
            warehouse.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            warehouse.PhoneNumber = NopSqlDataHelper.GetString(dataReader, "PhoneNumber");
            warehouse.Email = NopSqlDataHelper.GetString(dataReader, "Email");
            warehouse.FaxNumber = NopSqlDataHelper.GetString(dataReader, "FaxNumber");
            warehouse.Address1 = NopSqlDataHelper.GetString(dataReader, "Address1");
            warehouse.Address2 = NopSqlDataHelper.GetString(dataReader, "Address2");
            warehouse.City = NopSqlDataHelper.GetString(dataReader, "City");
            warehouse.StateProvince = NopSqlDataHelper.GetString(dataReader, "StateProvince");
            warehouse.ZipPostalCode = NopSqlDataHelper.GetString(dataReader, "ZipPostalCode");
            warehouse.CountryID = NopSqlDataHelper.GetInt(dataReader, "CountryID");
            warehouse.Deleted = NopSqlDataHelper.GetBoolean(dataReader, "Deleted");
            warehouse.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            warehouse.UpdatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "UpdatedOn");
            return warehouse;
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
        /// Gets all warehouses
        /// </summary>
        /// <returns>Warehouse collection</returns>
        public override DBWarehouseCollection GetAllWarehouses()
        {
            DBWarehouseCollection warehouseCollection = new DBWarehouseCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_WarehouseLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBWarehouse warehouse = GetWarehouseFromReader(dataReader);
                    warehouseCollection.Add(warehouse);
                }
            }

            return warehouseCollection;
        }

        /// <summary>
        /// Gets a warehouse
        /// </summary>
        /// <param name="WarehouseID">The warehouse identifier</param>
        /// <returns>Warehouse</returns>
        public override DBWarehouse GetWarehouseByID(int WarehouseID)
        {
            DBWarehouse warehouse = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_WarehouseLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "WarehouseID", DbType.Int32, WarehouseID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    warehouse = GetWarehouseFromReader(dataReader);
                }
            }
            return warehouse;
        }

        /// <summary>
        /// Inserts a warehouse
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="PhoneNumber">The phone number</param>
        /// <param name="Email">The email</param>
        /// <param name="FaxNumber">The fax number</param>
        /// <param name="Address1">The address 1</param>
        /// <param name="Address2">The address 2</param>
        /// <param name="City">The city</param>
        /// <param name="StateProvince">The state/province</param>
        /// <param name="ZipPostalCode">The zip/postal code</param>
        /// <param name="CountryID">The country identifier</param>
        /// <param name="Deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Warehouse</returns>
        public override DBWarehouse InsertWarehouse(string Name, string PhoneNumber, string Email, string FaxNumber,
            string Address1, string Address2, string City, string StateProvince,
            string ZipPostalCode, int CountryID, bool Deleted, DateTime CreatedOn, DateTime UpdatedOn)
        {
            DBWarehouse warehouse = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_WarehouseInsert");
            db.AddOutParameter(dbCommand, "WarehouseID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "PhoneNumber", DbType.String, PhoneNumber);
            db.AddInParameter(dbCommand, "Email", DbType.String, Email);
            db.AddInParameter(dbCommand, "FaxNumber", DbType.String, FaxNumber);
            db.AddInParameter(dbCommand, "Address1", DbType.String, Address1);
            db.AddInParameter(dbCommand, "Address2", DbType.String, Address2);
            db.AddInParameter(dbCommand, "City", DbType.String, City);
            db.AddInParameter(dbCommand, "StateProvince", DbType.String, StateProvince);
            db.AddInParameter(dbCommand, "ZipPostalCode", DbType.String, ZipPostalCode);
            db.AddInParameter(dbCommand, "CountryID", DbType.Int32, CountryID);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, Deleted);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, UpdatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int WarehouseID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@WarehouseID"));
                warehouse = GetWarehouseByID(WarehouseID);
            }

            return warehouse;
        }

        /// <summary>
        /// Updates the warehouse
        /// </summary>
        /// <param name="WarehouseID">The warehouse identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="PhoneNumber">The phone number</param>
        /// <param name="Email">The email</param>
        /// <param name="FaxNumber">The fax number</param>
        /// <param name="Address1">The address 1</param>
        /// <param name="Address2">The address 2</param>
        /// <param name="City">The city</param>
        /// <param name="StateProvince">The state/province</param>
        /// <param name="ZipPostalCode">The zip/postal code</param>
        /// <param name="CountryID">The country identifier</param>
        /// <param name="Deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Warehouse</returns>
        public override DBWarehouse UpdateWarehouse(int WarehouseID, string Name, string PhoneNumber, string Email, string FaxNumber,
            string Address1, string Address2, string City, string StateProvince,
            string ZipPostalCode, int CountryID, bool Deleted, DateTime CreatedOn, DateTime UpdatedOn)
        {
            DBWarehouse warehouse = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_WarehouseUpdate");
            db.AddInParameter(dbCommand, "WarehouseID", DbType.Int32, WarehouseID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "PhoneNumber", DbType.String, PhoneNumber);
            db.AddInParameter(dbCommand, "Email", DbType.String, Email);
            db.AddInParameter(dbCommand, "FaxNumber", DbType.String, FaxNumber);
            db.AddInParameter(dbCommand, "Address1", DbType.String, Address1);
            db.AddInParameter(dbCommand, "Address2", DbType.String, Address2);
            db.AddInParameter(dbCommand, "City", DbType.String, City);
            db.AddInParameter(dbCommand, "StateProvince", DbType.String, StateProvince);
            db.AddInParameter(dbCommand, "ZipPostalCode", DbType.String, ZipPostalCode);
            db.AddInParameter(dbCommand, "CountryID", DbType.Int32, CountryID);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, Deleted);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, UpdatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                warehouse = GetWarehouseByID(WarehouseID);

            return warehouse;
        }
        #endregion
    }
}
