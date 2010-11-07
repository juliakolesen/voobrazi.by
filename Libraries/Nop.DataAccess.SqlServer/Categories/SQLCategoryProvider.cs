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

namespace NopSolutions.NopCommerce.DataAccess.Categories
{
    /// <summary>
    /// Category provider for SQL Server
    /// </summary>
    public partial class SQLCategoryProvider : DBCategoryProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBCategory GetCategoryFromReader(IDataReader dataReader)
        {
            DBCategory category = new DBCategory();
            category.CategoryID = NopSqlDataHelper.GetInt(dataReader, "CategoryID");
            category.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            category.Description = NopSqlDataHelper.GetString(dataReader, "Description");
            category.TemplateID = NopSqlDataHelper.GetInt(dataReader, "TemplateID");
            category.MetaKeywords = NopSqlDataHelper.GetString(dataReader, "MetaKeywords");
            category.MetaDescription = NopSqlDataHelper.GetString(dataReader, "MetaDescription");
            category.MetaTitle = NopSqlDataHelper.GetString(dataReader, "MetaTitle");
            category.SEName = NopSqlDataHelper.GetString(dataReader, "SEName");
            category.ParentCategoryID = NopSqlDataHelper.GetInt(dataReader, "ParentCategoryID");
            category.PictureID = NopSqlDataHelper.GetInt(dataReader, "PictureID");
            category.PageSize = NopSqlDataHelper.GetInt(dataReader, "PageSize");
            category.PriceRanges = NopSqlDataHelper.GetString(dataReader, "PriceRanges");
            category.Published = NopSqlDataHelper.GetBoolean(dataReader, "Published");
            category.Deleted = NopSqlDataHelper.GetBoolean(dataReader, "Deleted");
            category.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            category.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            category.UpdatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "UpdatedOn");
            return category;
        }

        private DBProductCategory GetProductCategoryFromReader(IDataReader dataReader)
        {
            DBProductCategory productCategory = new DBProductCategory();
            productCategory.ProductCategoryID = NopSqlDataHelper.GetInt(dataReader, "ProductCategoryID");
            productCategory.ProductID = NopSqlDataHelper.GetInt(dataReader, "ProductID");
            productCategory.CategoryID = NopSqlDataHelper.GetInt(dataReader, "CategoryID");
            productCategory.IsFeaturedProduct = NopSqlDataHelper.GetBoolean(dataReader, "IsFeaturedProduct");
            productCategory.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            return productCategory;
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
        /// Gets all categories
        /// </summary>
        /// <param name="ParentCategoryID">Parent category identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Category collection</returns>
        public override DBCategoryCollection GetAllCategories(int ParentCategoryID, bool showHidden)
        {
            DBCategoryCollection categoryCollection = new DBCategoryCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CategoryLoadAll");
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            db.AddInParameter(dbCommand, "ParentCategoryID", DbType.Int32, ParentCategoryID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBCategory category = GetCategoryFromReader(dataReader);
                    categoryCollection.Add(category);
                }
            }

            return categoryCollection;
        }

        /// <summary>
        /// Gets a category
        /// </summary>
        /// <param name="CategoryID">Category identifier</param>
        /// <returns>Category</returns>
        public override DBCategory GetCategoryByID(int CategoryID)
        {

            DBCategory category = null;
            if (CategoryID == 0)
                return category;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CategoryLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "CategoryID", DbType.Int32, CategoryID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    category = GetCategoryFromReader(dataReader);
                }
            }
            return category;
        }

        /// <summary>
        /// Inserts category identifier
        /// </summary>
        /// <param name="Name">The category name</param>
        /// <param name="Description">The description</param>
        /// <param name="TemplateID">The template identifier</param>
        /// <param name="MetaKeywords">The meta keywords</param>
        /// <param name="MetaDescription">The meta description</param>
        /// <param name="MetaTitle">The meta title</param>
        /// <param name="SEName">The search-engine name</param>
        /// <param name="ParentCategoryID">The parent category identifier</param>
        /// <param name="PictureID">The picture identifier</param>
        /// <param name="PageSize">The page size</param>
        /// <param name="PriceRanges">The price ranges</param>
        /// <param name="Published">A value indicating whether the entity is published</param>
        /// <param name="Deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Category</returns>
        public override DBCategory InsertCategory(string Name, string Description,
            int TemplateID, string MetaKeywords, string MetaDescription, string MetaTitle,
            string SEName, int ParentCategoryID, int PictureID, int PageSize, string PriceRanges, bool Published, bool Deleted,
            int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn)
        {
            DBCategory category = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CategoryInsert");
            db.AddOutParameter(dbCommand, "CategoryID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "Description", DbType.String, Description);
            db.AddInParameter(dbCommand, "TemplateID", DbType.Int32, TemplateID);
            db.AddInParameter(dbCommand, "MetaKeywords", DbType.String, MetaKeywords);
            db.AddInParameter(dbCommand, "MetaDescription", DbType.String, MetaDescription);
            db.AddInParameter(dbCommand, "MetaTitle", DbType.String, MetaTitle);
            db.AddInParameter(dbCommand, "SEName", DbType.String, SEName);
            db.AddInParameter(dbCommand, "ParentCategoryID", DbType.Int32, ParentCategoryID);
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
                int CategoryID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@CategoryID"));
                category = GetCategoryByID(CategoryID);
            }
            return category;
        }

        /// <summary>
        /// Updates the category
        /// </summary>
        /// <param name="CategoryID">Category identifier</param>
        /// <param name="Name">The category name</param>
        /// <param name="Description">The description</param>
        /// <param name="TemplateID">The template identifier</param>
        /// <param name="MetaKeywords">The meta keywords</param>
        /// <param name="MetaDescription">The meta description</param>
        /// <param name="MetaTitle">The meta title</param>
        /// <param name="SEName">The search-engine name</param>
        /// <param name="ParentCategoryID">The parent category identifier</param>
        /// <param name="PictureID">The picture identifier</param>
        /// <param name="PageSize">The page size</param>
        /// <param name="PriceRanges">The price ranges</param>
        /// <param name="Published">A value indicating whether the entity is published</param>
        /// <param name="Deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Category</returns>
        public override DBCategory UpdateCategory(int CategoryID, string Name, string Description,
            int TemplateID, string MetaKeywords, string MetaDescription, string MetaTitle,
            string SEName, int ParentCategoryID, int PictureID, int PageSize, string PriceRanges, bool Published, bool Deleted,
            int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn)
        {
            DBCategory category = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CategoryUpdate");
            db.AddInParameter(dbCommand, "CategoryID", DbType.Int32, CategoryID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "Description", DbType.String, Description);
            db.AddInParameter(dbCommand, "TemplateID", DbType.Int32, TemplateID);
            db.AddInParameter(dbCommand, "MetaKeywords", DbType.String, MetaKeywords);
            db.AddInParameter(dbCommand, "MetaDescription", DbType.String, MetaDescription);
            db.AddInParameter(dbCommand, "MetaTitle", DbType.String, MetaTitle);
            db.AddInParameter(dbCommand, "SEName", DbType.String, SEName);
            db.AddInParameter(dbCommand, "ParentCategoryID", DbType.Int32, ParentCategoryID);
            db.AddInParameter(dbCommand, "PictureID", DbType.Int32, PictureID);
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, PageSize);
            db.AddInParameter(dbCommand, "PriceRanges", DbType.String, PriceRanges);
            db.AddInParameter(dbCommand, "Published", DbType.Boolean, Published);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, Deleted);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, UpdatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                category = GetCategoryByID(CategoryID);

            return category;
        }

        /// <summary>
        /// Deletes a product category mapping
        /// </summary>
        /// <param name="ProductCategoryID">Product category identifier</param>
        public override void DeleteProductCategory(int ProductCategoryID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Product_Category_MappingDelete");
            db.AddInParameter(dbCommand, "ProductCategoryID", DbType.Int32, ProductCategoryID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets product category mapping collection
        /// </summary>
        /// <param name="CategoryID">Category identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product a category mapping collection</returns>
        public override DBProductCategoryCollection GetProductCategoriesByCategoryID(int CategoryID, bool showHidden)
        {
            DBProductCategoryCollection productCategoryCollection = new DBProductCategoryCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Product_Category_MappingLoadByCategoryID");
            db.AddInParameter(dbCommand, "CategoryID", DbType.Int32, CategoryID);
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBProductCategory productCategory = GetProductCategoryFromReader(dataReader);
                    productCategoryCollection.Add(productCategory);
                }
            }

            return productCategoryCollection;
        }

        /// <summary>
        /// Gets a product category mapping collection
        /// </summary>
        /// <param name="ProductID">Product identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product category mapping collection</returns>
        public override DBProductCategoryCollection GetProductCategoriesByProductID(int ProductID, bool showHidden)
        {
            DBProductCategoryCollection productCategoryCollection = new DBProductCategoryCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Product_Category_MappingLoadByProductID");
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, ProductID);
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBProductCategory productCategory = GetProductCategoryFromReader(dataReader);
                    productCategoryCollection.Add(productCategory);
                }
            }

            return productCategoryCollection;
        }

        /// <summary>
        /// Gets a product category mapping 
        /// </summary>
        /// <param name="ProductCategoryID">Product category mapping identifier</param>
        /// <returns>Product category mapping</returns>
        public override DBProductCategory GetProductCategoryByID(int ProductCategoryID)
        {

            DBProductCategory productCategory = null;
            if (ProductCategoryID == 0)
                return productCategory;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Product_Category_MappingLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ProductCategoryID", DbType.Int32, ProductCategoryID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    productCategory = GetProductCategoryFromReader(dataReader);
                }
            }
            return productCategory;
        }

        /// <summary>
        /// Inserts a product category mapping
        /// </summary>
        /// <param name="ProductID">Product identifier</param>
        /// <param name="CategoryID">Category identifier</param>
        /// <param name="IsFeaturedProduct">A value indicating whether the product is featured</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Product category mapping </returns>
        public override DBProductCategory InsertProductCategory(int ProductID, int CategoryID,
            bool IsFeaturedProduct, int DisplayOrder)
        {
            DBProductCategory productCategory = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Product_Category_MappingInsert");
            db.AddOutParameter(dbCommand, "ProductCategoryID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, ProductID);
            db.AddInParameter(dbCommand, "CategoryID", DbType.Int32, CategoryID);
            db.AddInParameter(dbCommand, "IsFeaturedProduct", DbType.Boolean, IsFeaturedProduct);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int ProductCategoryID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ProductCategoryID"));
                productCategory = GetProductCategoryByID(ProductCategoryID);
            }
            return productCategory;
        }

        /// <summary>
        /// Updates the product category mapping 
        /// </summary>
        /// <param name="ProductCategoryID">Product category mapping  identifier</param>
        /// <param name="ProductID">Product identifier</param>
        /// <param name="CategoryID">Category identifier</param>
        /// <param name="IsFeaturedProduct">A value indicating whether the product is featured</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Product category mapping </returns>
        public override DBProductCategory UpdateProductCategory(int ProductCategoryID,
            int ProductID, int CategoryID, bool IsFeaturedProduct, int DisplayOrder)
        {
            DBProductCategory productCategory = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Product_Category_MappingUpdate");
            db.AddInParameter(dbCommand, "ProductCategoryID", DbType.Int32, ProductCategoryID);
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, ProductID);
            db.AddInParameter(dbCommand, "CategoryID", DbType.Int32, CategoryID);
            db.AddInParameter(dbCommand, "IsFeaturedProduct", DbType.Boolean, IsFeaturedProduct);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                productCategory = GetProductCategoryByID(ProductCategoryID);

            return productCategory;
        }
        #endregion
    }
}
