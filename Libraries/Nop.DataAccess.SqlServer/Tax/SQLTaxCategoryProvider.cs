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
    /// Tax category provider for SQL Server
    /// </summary>
    public partial class SQLTaxCategoryProvider : DBTaxCategoryProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBTaxCategory GetTaxCategoryFromReader(IDataReader dataReader)
        {
            DBTaxCategory taxCategory = new DBTaxCategory();
            taxCategory.TaxCategoryID = NopSqlDataHelper.GetInt(dataReader, "TaxCategoryID");
            taxCategory.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            taxCategory.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            taxCategory.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            taxCategory.UpdatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "UpdatedOn");
            return taxCategory;
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
        /// Deletes a tax category
        /// </summary>
        /// <param name="TaxCategoryID">The tax category identifier</param>
        public override void DeleteTaxCategory(int TaxCategoryID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DBTaxCategory taxCategory = GetTaxCategoryByID(TaxCategoryID);
            if (taxCategory != null)
            {
                DbCommand dbCommand = db.GetStoredProcCommand("Nop_TaxCategoryDelete");
                db.AddInParameter(dbCommand, "TaxCategoryID", DbType.Int32, TaxCategoryID);
                int retValue = db.ExecuteNonQuery(dbCommand);
            }
        }

        /// <summary>
        /// Gets all tax categories
        /// </summary>
        /// <returns>Tax category collection</returns>
        public override DBTaxCategoryCollection GetAllTaxCategories()
        {
            DBTaxCategoryCollection taxCategoryCollection = new DBTaxCategoryCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TaxCategoryLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBTaxCategory taxCategory = GetTaxCategoryFromReader(dataReader);
                    taxCategoryCollection.Add(taxCategory);
                }
            }

            return taxCategoryCollection;
        }

        /// <summary>
        /// Gets a tax category
        /// </summary>
        /// <param name="TaxCategoryID">Tax category identifier</param>
        /// <returns>Tax category</returns>
        public override DBTaxCategory GetTaxCategoryByID(int TaxCategoryID)
        {
            DBTaxCategory taxCategory = null;
            if (TaxCategoryID == 0)
                return taxCategory;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TaxCategoryLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "TaxCategoryID", DbType.Int32, TaxCategoryID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    taxCategory = GetTaxCategoryFromReader(dataReader);
                }
            }
            return taxCategory;
        }

        /// <summary>
        /// Inserts a tax category
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Tax category</returns>
        public override DBTaxCategory InsertTaxCategory(string Name,
            int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn)
        {
            DBTaxCategory taxCategory = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TaxCategoryInsert");
            db.AddOutParameter(dbCommand, "TaxCategoryID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, UpdatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int TaxCategoryID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@TaxCategoryID"));
                taxCategory = GetTaxCategoryByID(TaxCategoryID);
            }
            return taxCategory;
        }

        /// <summary>
        /// Updates the tax category
        /// </summary>
        /// <param name="TaxCategoryID">The tax category identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Tax category</returns>
        public override DBTaxCategory UpdateTaxCategory(int TaxCategoryID, string Name,
            int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn)
        {
            DBTaxCategory taxCategory = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TaxCategoryUpdate");
            db.AddInParameter(dbCommand, "TaxCategoryID", DbType.Int32, TaxCategoryID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, UpdatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                taxCategory = GetTaxCategoryByID(TaxCategoryID);

            return taxCategory;
        }
        #endregion
    }
}
