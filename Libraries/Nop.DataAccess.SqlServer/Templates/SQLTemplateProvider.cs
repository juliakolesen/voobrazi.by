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

namespace NopSolutions.NopCommerce.DataAccess.Templates
{
    /// <summary>
    /// Template provider for SQL Server
    /// </summary>
    public partial class SQLTemplateProvider : DBTemplateProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities

        private DBCategoryTemplate GetCategoryTemplateFromReader(IDataReader dataReader)
        {
            DBCategoryTemplate categoryTemplate = new DBCategoryTemplate();
            categoryTemplate.CategoryTemplateID = NopSqlDataHelper.GetInt(dataReader, "CategoryTemplateID");
            categoryTemplate.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            categoryTemplate.TemplatePath = NopSqlDataHelper.GetString(dataReader, "TemplatePath");
            categoryTemplate.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            categoryTemplate.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            categoryTemplate.UpdatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "UpdatedOn");
            return categoryTemplate;
        }

        private DBProductTemplate GetProductTemplateFromReader(IDataReader dataReader)
        {
            DBProductTemplate productTemplate = new DBProductTemplate();
            productTemplate.ProductTemplateID = NopSqlDataHelper.GetInt(dataReader, "ProductTemplateID");
            productTemplate.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            productTemplate.TemplatePath = NopSqlDataHelper.GetString(dataReader, "TemplatePath");
            productTemplate.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            productTemplate.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            productTemplate.UpdatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "UpdatedOn");
            return productTemplate;
        }

        private DBManufacturerTemplate GetManufacturerTemplateFromReader(IDataReader dataReader)
        {
            DBManufacturerTemplate manufacturerTemplate = new DBManufacturerTemplate();
            manufacturerTemplate.ManufacturerTemplateID = NopSqlDataHelper.GetInt(dataReader, "ManufacturerTemplateID");
            manufacturerTemplate.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            manufacturerTemplate.TemplatePath = NopSqlDataHelper.GetString(dataReader, "TemplatePath");
            manufacturerTemplate.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            manufacturerTemplate.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            manufacturerTemplate.UpdatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "UpdatedOn");
            return manufacturerTemplate;
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
        /// Deletes a category template
        /// </summary>
        /// <param name="CategoryTemplateID">Category template identifier</param>
        public override void DeleteCategoryTemplate(int CategoryTemplateID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DBCategoryTemplate categoryTemplate = GetCategoryTemplateByID(CategoryTemplateID);
            if (categoryTemplate != null)
            {
                DbCommand dbCommand = db.GetStoredProcCommand("Nop_CategoryTemplateDelete");
                db.AddInParameter(dbCommand, "CategoryTemplateID", DbType.Int32, CategoryTemplateID);
                int retValue = db.ExecuteNonQuery(dbCommand);
            }
        }

        /// <summary>
        /// Gets all category templates
        /// </summary>
        /// <returns>Category template collection</returns>
        public override DBCategoryTemplateCollection GetAllCategoryTemplates()
        {
            DBCategoryTemplateCollection categoryTemplateCollection = new DBCategoryTemplateCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CategoryTemplateLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBCategoryTemplate categoryTemplate = GetCategoryTemplateFromReader(dataReader);
                    categoryTemplateCollection.Add(categoryTemplate);
                }
            }

            return categoryTemplateCollection;
        }

        /// <summary>
        /// Gets a category template
        /// </summary>
        /// <param name="CategoryTemplateID">Category template identifier</param>
        /// <returns>A category template</returns>
        public override DBCategoryTemplate GetCategoryTemplateByID(int CategoryTemplateID)
        {
            DBCategoryTemplate categoryTemplate = null;
            if (CategoryTemplateID == 0)
                return categoryTemplate;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CategoryTemplateLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "CategoryTemplateID", DbType.Int32, CategoryTemplateID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    categoryTemplate = GetCategoryTemplateFromReader(dataReader);
                }
            }
            return categoryTemplate;
        }

        /// <summary>
        /// Inserts a category template
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="TemplatePath">The template path</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>A category template</returns>
        public override DBCategoryTemplate InsertCategoryTemplate(string Name, string TemplatePath,
            int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn)
        {
            DBCategoryTemplate categoryTemplate = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CategoryTemplateInsert");
            db.AddOutParameter(dbCommand, "CategoryTemplateID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "TemplatePath", DbType.String, TemplatePath);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, UpdatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int CategoryTemplateID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@CategoryTemplateID"));
                categoryTemplate = GetCategoryTemplateByID(CategoryTemplateID);
            }
            return categoryTemplate;
        }

        /// <summary>
        /// Updates the category template
        /// </summary>
        /// <param name="CategoryTemplateID">Category template identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="TemplatePath">The template path</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>A category template</returns>
        public override DBCategoryTemplate UpdateCategoryTemplate(int CategoryTemplateID, string Name, string TemplatePath,
            int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn)
        {
            DBCategoryTemplate categoryTemplate = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CategoryTemplateUpdate");
            db.AddInParameter(dbCommand, "CategoryTemplateID", DbType.Int32, CategoryTemplateID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "TemplatePath", DbType.String, TemplatePath);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, UpdatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                categoryTemplate = GetCategoryTemplateByID(CategoryTemplateID);


            return categoryTemplate;
        }
        
        /// <summary>
        /// Deletes a manufacturer template
        /// </summary>
        /// <param name="ManufacturerTemplateID">Manufacturer template identifier</param>
        public override void DeleteManufacturerTemplate(int ManufacturerTemplateID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DBManufacturerTemplate manufacturerTemplate = GetManufacturerTemplateByID(ManufacturerTemplateID);
            if (manufacturerTemplate != null)
            {
                DbCommand dbCommand = db.GetStoredProcCommand("Nop_ManufacturerTemplateDelete");
                db.AddInParameter(dbCommand, "ManufacturerTemplateID", DbType.Int32, ManufacturerTemplateID);
                int retValue = db.ExecuteNonQuery(dbCommand);
            }
        }

        /// <summary>
        /// Gets all manufacturer templates
        /// </summary>
        /// <returns>Manufacturer template collection</returns>
        public override DBManufacturerTemplateCollection GetAllManufacturerTemplates()
        {
            DBManufacturerTemplateCollection manufacturerTemplateCollection = new DBManufacturerTemplateCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ManufacturerTemplateLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBManufacturerTemplate manufacturerTemplate = GetManufacturerTemplateFromReader(dataReader);
                    manufacturerTemplateCollection.Add(manufacturerTemplate);
                }
            }

            return manufacturerTemplateCollection;
        }

        /// <summary>
        /// Gets a manufacturer template
        /// </summary>
        /// <param name="ManufacturerTemplateID">Manufacturer template identifier</param>
        /// <returns>Manufacturer template</returns>
        public override DBManufacturerTemplate GetManufacturerTemplateByID(int ManufacturerTemplateID)
        {
            DBManufacturerTemplate manufacturerTemplate = null;
            if (ManufacturerTemplateID == 0)
                return manufacturerTemplate;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ManufacturerTemplateLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ManufacturerTemplateID", DbType.Int32, ManufacturerTemplateID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    manufacturerTemplate = GetManufacturerTemplateFromReader(dataReader);
                }
            }
            return manufacturerTemplate;
        }

        /// <summary>
        /// Inserts a manufacturer template
        /// </summary>
        /// <param name="Name">The manufacturer template identifier</param>
        /// <param name="TemplatePath">The template path</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Manufacturer template</returns>
        public override DBManufacturerTemplate InsertManufacturerTemplate(string Name, string TemplatePath,
            int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn)
        {
            DBManufacturerTemplate manufacturerTemplate = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ManufacturerTemplateInsert");
            db.AddOutParameter(dbCommand, "ManufacturerTemplateID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "TemplatePath", DbType.String, TemplatePath);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, UpdatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int ManufacturerTemplateID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ManufacturerTemplateID"));
                manufacturerTemplate = GetManufacturerTemplateByID(ManufacturerTemplateID);
            }
            return manufacturerTemplate;
        }

        /// <summary>
        /// Updates the manufacturer template
        /// </summary>
        /// <param name="ManufacturerTemplateID">Manufacturer template identifer</param>
        /// <param name="Name">The manufacturer template identifier</param>
        /// <param name="TemplatePath">The template path</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Manufacturer template</returns>
        public override DBManufacturerTemplate UpdateManufacturerTemplate(int ManufacturerTemplateID, string Name, string TemplatePath,
            int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn)
        {
            DBManufacturerTemplate manufacturerTemplate = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ManufacturerTemplateUpdate");
            db.AddInParameter(dbCommand, "ManufacturerTemplateID", DbType.Int32, ManufacturerTemplateID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "TemplatePath", DbType.String, TemplatePath);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, UpdatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                manufacturerTemplate = GetManufacturerTemplateByID(ManufacturerTemplateID);

            return manufacturerTemplate;
        }

        /// <summary>
        /// Deletes a product template
        /// </summary>
        /// <param name="ProductTemplateID">Product template identifier</param>
        public override void DeleteProductTemplate(int ProductTemplateID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DBProductTemplate productTemplate = GetProductTemplateByID(ProductTemplateID);
            if (productTemplate != null)
            {
                DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductTemplateDelete");
                db.AddInParameter(dbCommand, "ProductTemplateID", DbType.Int32, ProductTemplateID);
                int retValue = db.ExecuteNonQuery(dbCommand);
            }
        }

        /// <summary>
        /// Gets all product templates
        /// </summary>
        /// <returns>Product template collection</returns>
        public override DBProductTemplateCollection GetAllProductTemplates()
        {
            DBProductTemplateCollection productTemplateCollection = new DBProductTemplateCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductTemplateLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBProductTemplate productTemplate = GetProductTemplateFromReader(dataReader);
                    productTemplateCollection.Add(productTemplate);
                }
            }

            return productTemplateCollection;
        }

        /// <summary>
        /// Gets a product template
        /// </summary>
        /// <param name="ProductTemplateID">Product template identifier</param>
        /// <returns>Product template</returns>
        public override DBProductTemplate GetProductTemplateByID(int ProductTemplateID)
        {
            DBProductTemplate productTemplate = null;
            if (ProductTemplateID == 0)
                return productTemplate;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductTemplateLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ProductTemplateID", DbType.Int32, ProductTemplateID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    productTemplate = GetProductTemplateFromReader(dataReader);
                }
            }
            return productTemplate;
        }

        /// <summary>
        /// Inserts a product template
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="TemplatePath">The template path</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Product template</returns>
        public override DBProductTemplate InsertProductTemplate(string Name, string TemplatePath,
            int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn)
        {
            DBProductTemplate productTemplate = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductTemplateInsert");
            db.AddOutParameter(dbCommand, "ProductTemplateID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "TemplatePath", DbType.String, TemplatePath);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, UpdatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int ProductTemplateID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ProductTemplateID"));
                productTemplate = GetProductTemplateByID(ProductTemplateID);
            }
            return productTemplate;
        }

        /// <summary>
        /// Updates the product template
        /// </summary>
        /// <param name="ProductTemplateID">The product template identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="TemplatePath">The template path</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Product template</returns>
        public override DBProductTemplate UpdateProductTemplate(int ProductTemplateID, string Name, string TemplatePath,
            int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn)
        {
            DBProductTemplate productTemplate = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductTemplateUpdate");
            db.AddInParameter(dbCommand, "ProductTemplateID", DbType.Int32, ProductTemplateID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "TemplatePath", DbType.String, TemplatePath);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, UpdatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                productTemplate = GetProductTemplateByID(ProductTemplateID);


            return productTemplate;
        }

        #endregion
    }
}
