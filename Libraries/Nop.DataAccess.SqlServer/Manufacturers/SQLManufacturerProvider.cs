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

namespace NopSolutions.NopCommerce.DataAccess.Manufacturers
{
    /// <summary>
    /// Manufacturer provider for SQL Server
    /// </summary>
    public partial class SQLManufacturerProvider : DBManufacturerProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBManufacturer GetManufacturerFromReader(IDataReader dataReader)
        {
            DBManufacturer manufacturer = new DBManufacturer();
            manufacturer.ManufacturerID = NopSqlDataHelper.GetInt(dataReader, "ManufacturerID");
            manufacturer.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            manufacturer.Description = NopSqlDataHelper.GetString(dataReader, "Description");
            manufacturer.TemplateID = NopSqlDataHelper.GetInt(dataReader, "TemplateID");
            manufacturer.MetaKeywords = NopSqlDataHelper.GetString(dataReader, "MetaKeywords");
            manufacturer.MetaDescription = NopSqlDataHelper.GetString(dataReader, "MetaDescription");
            manufacturer.MetaTitle = NopSqlDataHelper.GetString(dataReader, "MetaTitle");
            manufacturer.SEName = NopSqlDataHelper.GetString(dataReader, "SEName");
            manufacturer.PictureID = NopSqlDataHelper.GetInt(dataReader, "PictureID");
            manufacturer.PageSize = NopSqlDataHelper.GetInt(dataReader, "PageSize");
            manufacturer.PriceRanges = NopSqlDataHelper.GetString(dataReader, "PriceRanges");
            manufacturer.Published = NopSqlDataHelper.GetBoolean(dataReader, "Published");
            manufacturer.Deleted = NopSqlDataHelper.GetBoolean(dataReader, "Deleted");
            manufacturer.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            manufacturer.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            manufacturer.UpdatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "UpdatedOn");
            return manufacturer;
        }

        private DBProductManufacturer GetProductManufacturerFromReader(IDataReader dataReader)
        {
            DBProductManufacturer productManufacturer = new DBProductManufacturer();
            productManufacturer.ProductManufacturerID = NopSqlDataHelper.GetInt(dataReader, "ProductManufacturerID");
            productManufacturer.ProductID = NopSqlDataHelper.GetInt(dataReader, "ProductID");
            productManufacturer.ManufacturerID = NopSqlDataHelper.GetInt(dataReader, "ManufacturerID");
            productManufacturer.IsFeaturedProduct = NopSqlDataHelper.GetBoolean(dataReader, "IsFeaturedProduct");
            productManufacturer.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            return productManufacturer;
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
        /// Gets all manufacturers
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Manufacturer collection</returns>
        public override DBManufacturerCollection GetAllManufacturers(bool showHidden)
        {
            DBManufacturerCollection manufacturerCollection = new DBManufacturerCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ManufacturerLoadAll");
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBManufacturer manufacturer = GetManufacturerFromReader(dataReader);
                    manufacturerCollection.Add(manufacturer);
                }
            }

            return manufacturerCollection;
        }

        /// <summary>
        /// Gets a manufacturer
        /// </summary>
        /// <param name="ManufacturerID">Manufacturer identifier</param>
        /// <returns>Manufacturer</returns>
        public override DBManufacturer GetManufacturerByID(int ManufacturerID)
        {

            DBManufacturer manufacturer = null;
            if (ManufacturerID == 0)
                return manufacturer;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ManufacturerLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ManufacturerID", DbType.Int32, ManufacturerID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    manufacturer = GetManufacturerFromReader(dataReader);
                }
            }
            return manufacturer;
        }

        /// <summary>
        /// Inserts a manufacturer
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="Description">The description</param>
        /// <param name="TemplateID">The template identifier</param>
        /// <param name="MetaKeywords">The meta keywords</param>
        /// <param name="MetaDescription">The meta description</param>
        /// <param name="MetaTitle">The meta title</param>
        /// <param name="SEName">The search-engine name</param>
        /// <param name="PictureID">The parent picture identifier</param>
        /// <param name="PageSize">The page size</param>
        /// <param name="PriceRanges">The price ranges</param>
        /// <param name="Published">A value indicating whether the entity is published</param>
        /// <param name="Deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Manufacturer</returns>
        public override DBManufacturer InsertManufacturer(string Name, string Description,
            int TemplateID, string MetaKeywords, string MetaDescription, string MetaTitle,
            string SEName, int PictureID, int PageSize, string PriceRanges, bool Published, bool Deleted,
            int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn)
        {
            DBManufacturer manufacturer = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ManufacturerInsert");
            db.AddOutParameter(dbCommand, "ManufacturerID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "Description", DbType.String, Description);
            db.AddInParameter(dbCommand, "TemplateID", DbType.Int32, TemplateID);
            db.AddInParameter(dbCommand, "MetaKeywords", DbType.String, MetaKeywords);
            db.AddInParameter(dbCommand, "MetaDescription", DbType.String, MetaDescription);
            db.AddInParameter(dbCommand, "MetaTitle", DbType.String, MetaTitle);
            db.AddInParameter(dbCommand, "SEName", DbType.String, SEName);
            db.AddInParameter(dbCommand, "PictureID", DbType.Int32, PictureID);
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, PageSize);
            db.AddInParameter(dbCommand, "PriceRanges", DbType.String, PriceRanges);
            db.AddInParameter(dbCommand, "Published", DbType.Boolean, Published);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, Deleted);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, UpdatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int ManufacturerID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ManufacturerID"));
                manufacturer = GetManufacturerByID(ManufacturerID);
            }
            return manufacturer;
        }

        /// <summary>
        /// Updates the manufacturer
        /// </summary>
        /// <param name="ManufacturerID">Manufacturer identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="Description">The description</param>
        /// <param name="TemplateID">The template identifier</param>
        /// <param name="MetaKeywords">The meta keywords</param>
        /// <param name="MetaDescription">The meta description</param>
        /// <param name="MetaTitle">The meta title</param>
        /// <param name="SEName">The search-engine name</param>
        /// <param name="PictureID">The parent picture identifier</param>
        /// <param name="PageSize">The page size</param>
        /// <param name="PriceRanges">The price ranges</param>
        /// <param name="Published">A value indicating whether the entity is published</param>
        /// <param name="Deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Manufacturer</returns>
        public override DBManufacturer UpdateManufacturer(int ManufacturerID, string Name, string Description,
            int TemplateID, string MetaKeywords, string MetaDescription, string MetaTitle,
            string SEName, int PictureID, int PageSize, string PriceRanges, bool Published, bool Deleted,
            int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn)
        {
            DBManufacturer manufacturer = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ManufacturerUpdate");
            db.AddInParameter(dbCommand, "ManufacturerID", DbType.Int32, ManufacturerID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "Description", DbType.String, Description);
            db.AddInParameter(dbCommand, "TemplateID", DbType.Int32, TemplateID);
            db.AddInParameter(dbCommand, "MetaKeywords", DbType.String, MetaKeywords);
            db.AddInParameter(dbCommand, "MetaDescription", DbType.String, MetaDescription);
            db.AddInParameter(dbCommand, "MetaTitle", DbType.String, MetaTitle);
            db.AddInParameter(dbCommand, "SEName", DbType.String, SEName);
            db.AddInParameter(dbCommand, "PictureID", DbType.Int32, PictureID);
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, PageSize);
            db.AddInParameter(dbCommand, "PriceRanges", DbType.String, PriceRanges);
            db.AddInParameter(dbCommand, "Published", DbType.Boolean, Published);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, Deleted);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, UpdatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                manufacturer = GetManufacturerByID(ManufacturerID);


            return manufacturer;
        }

        /// <summary>
        /// Deletes a product manufacturer mapping
        /// </summary>
        /// <param name="ProductManufacturerID">Product manufacturer mapping identifer</param>
        public override void DeleteProductManufacturer(int ProductManufacturerID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Product_Manufacturer_MappingDelete");
            db.AddInParameter(dbCommand, "ProductManufacturerID", DbType.Int32, ProductManufacturerID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets product category manufacturer collection
        /// </summary>
        /// <param name="ManufacturerID">Manufacturer identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product category manufacturer collection</returns>
        public override DBProductManufacturerCollection GetProductManufacturersByManufacturerID(int ManufacturerID, bool showHidden)
        {
            DBProductManufacturerCollection productManufacturerCollection = new DBProductManufacturerCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Product_Manufacturer_MappingLoadByManufacturerID");
            db.AddInParameter(dbCommand, "ManufacturerID", DbType.Int32, ManufacturerID);
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBProductManufacturer productManufacturer = GetProductManufacturerFromReader(dataReader);
                    productManufacturerCollection.Add(productManufacturer);
                }
            }

            return productManufacturerCollection;
        }

        /// <summary>
        /// Gets a product manufacturer mapping collection
        /// </summary>
        /// <param name="ProductID">Product identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product manufacturer mapping collection</returns>
        public override DBProductManufacturerCollection GetProductManufacturersByProductID(int ProductID, bool showHidden)
        {
            DBProductManufacturerCollection productManufacturerCollection = new DBProductManufacturerCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Product_Manufacturer_MappingLoadByProductID");
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, ProductID);
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBProductManufacturer productManufacturer = GetProductManufacturerFromReader(dataReader);
                    productManufacturerCollection.Add(productManufacturer);
                }
            }

            return productManufacturerCollection;
        }

        /// <summary>
        /// Gets a product manufacturer mapping 
        /// </summary>
        /// <param name="ProductManufacturerID">Product manufacturer mapping identifier</param>
        /// <returns>Product manufacturer mapping</returns>
        public override DBProductManufacturer GetProductManufacturerByID(int ProductManufacturerID)
        {

            DBProductManufacturer productManufacturer = null;
            if (ProductManufacturerID == 0)
                return productManufacturer;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Product_Manufacturer_MappingLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ProductManufacturerID", DbType.Int32, ProductManufacturerID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    productManufacturer = GetProductManufacturerFromReader(dataReader);
                }
            }
            return productManufacturer;
        }

        /// <summary>
        /// Inserts a product manufacturer mapping
        /// </summary>
        /// <param name="ProductID">Product identifier</param>
        /// <param name="ManufacturerID">Manufacturer identifier</param>
        /// <param name="IsFeaturedProduct">A value indicating whether the product is featured</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Product manufacturer mapping </returns>
        public override DBProductManufacturer InsertProductManufacturer(int ProductID, int ManufacturerID, bool IsFeaturedProduct, int DisplayOrder)
        {
            DBProductManufacturer productManufacturer = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Product_Manufacturer_MappingInsert");
            db.AddOutParameter(dbCommand, "ProductManufacturerID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, ProductID);
            db.AddInParameter(dbCommand, "ManufacturerID", DbType.Int32, ManufacturerID);
            db.AddInParameter(dbCommand, "IsFeaturedProduct", DbType.Boolean, IsFeaturedProduct);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int ProductManufacturerID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ProductManufacturerID"));
                productManufacturer = GetProductManufacturerByID(ProductManufacturerID);
            }
            return productManufacturer;
        }

        /// <summary>
        /// Updates the product manufacturer mapping
        /// </summary>
        /// <param name="ProductManufacturerID">Product manufacturer mapping identifier</param>
        /// <param name="ProductID">Product identifier</param>
        /// <param name="ManufacturerID">Manufacturer identifier</param>
        /// <param name="IsFeaturedProduct">A value indicating whether the product is featured</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Product manufacturer mapping </returns>
        public override DBProductManufacturer UpdateProductManufacturer(int ProductManufacturerID,
            int ProductID, int ManufacturerID, bool IsFeaturedProduct, int DisplayOrder)
        {
            DBProductManufacturer productManufacturer = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Product_Manufacturer_MappingUpdate");
            db.AddInParameter(dbCommand, "ProductManufacturerID", DbType.Int32, ProductManufacturerID);
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, ProductID);
            db.AddInParameter(dbCommand, "ManufacturerID", DbType.Int32, ManufacturerID);
            db.AddInParameter(dbCommand, "IsFeaturedProduct", DbType.Boolean, IsFeaturedProduct);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                productManufacturer = GetProductManufacturerByID(ProductManufacturerID);

            return productManufacturer;
        }
        
        #endregion
    }
}
