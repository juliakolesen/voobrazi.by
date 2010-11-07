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

namespace NopSolutions.NopCommerce.DataAccess.Tax
{
    /// <summary>
    /// Tax provider provider for SQL Server
    /// </summary>
    public partial class SQLTaxProviderProvider : DBTaxProviderProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBTaxProvider GetTaxProviderFromReader(IDataReader dataReader)
        {
            DBTaxProvider taxProvider = new DBTaxProvider();
            taxProvider.TaxProviderID = NopSqlDataHelper.GetInt(dataReader, "TaxProviderID");
            taxProvider.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            taxProvider.Description = NopSqlDataHelper.GetString(dataReader, "Description");
            taxProvider.ConfigureTemplatePath = NopSqlDataHelper.GetString(dataReader, "ConfigureTemplatePath");
            taxProvider.ClassName = NopSqlDataHelper.GetString(dataReader, "ClassName");
            taxProvider.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            return taxProvider;
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
        /// Deletes a tax provider
        /// </summary>
        /// <param name="TaxProviderID">Tax provider identifier</param>
        public override void DeleteTaxProvider(int TaxProviderID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TaxProviderDelete");
            db.AddInParameter(dbCommand, "TaxProviderID", DbType.Int32, TaxProviderID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a tax provider
        /// </summary>
        /// <param name="TaxProviderID">Tax provider identifier</param>
        /// <returns>Tax provider</returns>
        public override DBTaxProvider GetTaxProviderByID(int TaxProviderID)
        {

            DBTaxProvider taxProvider = null;
            if (TaxProviderID == 0)
                return taxProvider;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TaxProviderLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "TaxProviderID", DbType.Int32, TaxProviderID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    taxProvider = GetTaxProviderFromReader(dataReader);
                }
            }
            return taxProvider;
        }

        /// <summary>
        /// Gets all tax providers
        /// </summary>
        /// <returns>Shipping rate computation method collection</returns>
        public override DBTaxProviderCollection GetAllTaxProviders()
        {
            DBTaxProviderCollection taxProviderCollection = new DBTaxProviderCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TaxProviderLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBTaxProvider taxProvider = GetTaxProviderFromReader(dataReader);
                    taxProviderCollection.Add(taxProvider);
                }
            }
            return taxProviderCollection;
        }

        /// <summary>
        /// Inserts a tax provider
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="Description">The description</param>
        /// <param name="ConfigureTemplatePath">The configure template path</param>
        /// <param name="ClassName">The class name</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Tax provider</returns>
        public override DBTaxProvider InsertTaxProvider(string Name, string Description,
           string ConfigureTemplatePath, string ClassName, int DisplayOrder)
        {
            DBTaxProvider taxProvider = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TaxProviderInsert");
            db.AddOutParameter(dbCommand, "TaxProviderID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "Description", DbType.String, Description);
            db.AddInParameter(dbCommand, "ConfigureTemplatePath", DbType.String, ConfigureTemplatePath);
            db.AddInParameter(dbCommand, "ClassName", DbType.String, ClassName);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int TaxProviderID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@TaxProviderID"));
                taxProvider = GetTaxProviderByID(TaxProviderID);
            }
            return taxProvider;
        }

        /// <summary>
        /// Updates the tax provider
        /// </summary>
        /// <param name="TaxProviderID">The tax provider identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="Description">The description</param>
        /// <param name="ConfigureTemplatePath">The configure template path</param>
        /// <param name="ClassName">The class name</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Tax provider</returns>
        public override DBTaxProvider UpdateTaxProvider(int TaxProviderID, string Name, string Description,
           string ConfigureTemplatePath, string ClassName, int DisplayOrder)
        {
            DBTaxProvider taxProvider = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TaxProviderUpdate");
            db.AddInParameter(dbCommand, "TaxProviderID", DbType.Int32, TaxProviderID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "Description", DbType.String, Description);
            db.AddInParameter(dbCommand, "ConfigureTemplatePath", DbType.String, ConfigureTemplatePath);
            db.AddInParameter(dbCommand, "ClassName", DbType.String, ClassName);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                taxProvider = GetTaxProviderByID(TaxProviderID);

            return taxProvider;
        }
        #endregion
    }
}
