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
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Xml;

namespace NopSolutions.NopCommerce.DataAccess.Products.Specs
{
    /// <summary>
    /// Specification attribute provider for SQL Server
    /// </summary>
    public partial class SQLSpecificationAttributeProvider : DBSpecificationAttributeProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities

        /// <summary>
        /// Maps a data reader to a spacification attribute
        /// </summary>
        /// <param name="dataReader">IDataReader</param>
        /// <returns>Specification attribute</returns>
        private DBSpecificationAttribute GetSpecificationAttributeFromReader(IDataReader dataReader)
        {
            DBSpecificationAttribute specificationAttribute = new DBSpecificationAttribute();
            specificationAttribute.SpecificationAttributeID = NopSqlDataHelper.GetInt(dataReader, "SpecificationAttributeID");
            specificationAttribute.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            specificationAttribute.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            return specificationAttribute;
        }

        /// <summary>
        /// Maps a data reader to a specification attribute option
        /// </summary>
        /// <param name="dataReader">IDataReader</param>
        /// <returns>Specification attribute option</returns>
        private DBSpecificationAttributeOption GetSpecificationAttributeOptionFromReader(IDataReader dataReader)
        {
            DBSpecificationAttributeOption specificationAttributeOption = new DBSpecificationAttributeOption();
            specificationAttributeOption.SpecificationAttributeOptionID = NopSqlDataHelper.GetInt(dataReader, "SpecificationAttributeOptionID");
            specificationAttributeOption.SpecificationAttributeID = NopSqlDataHelper.GetInt(dataReader, "SpecificationAttributeID");
            specificationAttributeOption.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            specificationAttributeOption.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            return specificationAttributeOption;
        }

        /// <summary>
        /// Maps a data reader to a product specification attribute
        /// </summary>
        /// <param name="dataReader">IDataReader</param>
        /// <returns>Product specification attribute</returns>
        private DBProductSpecificationAttribute GetProductSpecificationAttributeFromReader(IDataReader dataReader)
        {
            DBProductSpecificationAttribute productSpecificationAttribute = new DBProductSpecificationAttribute();
            productSpecificationAttribute.ProductSpecificationAttributeID = NopSqlDataHelper.GetInt(dataReader, "ProductSpecificationAttributeID");
            productSpecificationAttribute.ProductID = NopSqlDataHelper.GetInt(dataReader, "ProductID");
            productSpecificationAttribute.SpecificationAttributeOptionID = NopSqlDataHelper.GetInt(dataReader, "SpecificationAttributeOptionID");
            productSpecificationAttribute.AllowFiltering = NopSqlDataHelper.GetBoolean(dataReader, "AllowFiltering");
            productSpecificationAttribute.ShowOnProductPage = NopSqlDataHelper.GetBoolean(dataReader, "ShowOnProductPage");
            productSpecificationAttribute.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            return productSpecificationAttribute;
        }

        /// <summary>
        /// Maps a data reader to a specification attribute option filter
        /// </summary>
        /// <param name="dataReader">IDataReader</param>
        /// <returns>Specification attribute option filter</returns>
        private DBSpecificationAttributeOptionFilter GetSpecificationAttributeOptionFilterFromReader(IDataReader dataReader)
        {
            DBSpecificationAttributeOptionFilter item = new DBSpecificationAttributeOptionFilter();
            item.SpecificationAttributeID = NopSqlDataHelper.GetInt(dataReader, "SpecificationAttributeID");
            item.SpecificationAttributeName = NopSqlDataHelper.GetString(dataReader, "SpecificationAttributeName");
            item.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            item.SpecificationAttributeOptionID = NopSqlDataHelper.GetInt(dataReader, "SpecificationAttributeOptionID");
            item.SpecificationAttributeOptionName = NopSqlDataHelper.GetString(dataReader, "SpecificationAttributeOptionName");
            return item;
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

        #region SpecificationAttribute

        /// <summary>
        /// Gets a specification attribute
        /// </summary>
        /// <param name="SpecificationAttributeID">The specification attribute identifier</param>
        /// <returns>Specification attribute</returns>
        public override DBSpecificationAttribute GetSpecificationAttributeByID(int SpecificationAttributeID)
        {
            DBSpecificationAttribute specificationAttribute = null;
            if (SpecificationAttributeID == 0)
                return specificationAttribute;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SpecificationAttributeLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "SpecificationAttributeID", DbType.Int32, SpecificationAttributeID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    specificationAttribute = GetSpecificationAttributeFromReader(dataReader);
                }
            }
            return specificationAttribute;
        }

        /// <summary>
        /// Gets specification attribute collection
        /// </summary>
        /// <returns>Specification attribute collection</returns>
        public override DBSpecificationAttributeCollection GetSpecificationAttributes()
        {
            DBSpecificationAttributeCollection specificationAttributeCollection = new DBSpecificationAttributeCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SpecificationAttributeLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBSpecificationAttribute specificationAttribute = GetSpecificationAttributeFromReader(dataReader);
                    specificationAttributeCollection.Add(specificationAttribute);
                }
            }

            return specificationAttributeCollection;
        }

        /// <summary>
        /// Deletes a specification attribute
        /// </summary>
        /// <param name="SpecificationAttributeID">The specification attribute identifier</param>
        public override void DeleteSpecificationAttribute(int SpecificationAttributeID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SpecificationAttributeDelete");
            db.AddInParameter(dbCommand, "SpecificationAttributeID", DbType.Int32, SpecificationAttributeID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Inserts a specification attribute
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="displayOrder">Display order</param>
        /// <returns>Specification attribute</returns>
        public override DBSpecificationAttribute InsertSpecificationAttribute(string name, int displayOrder)
        {
            DBSpecificationAttribute specificationAttribute = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SpecificationAttributeInsert");
            db.AddOutParameter(dbCommand, "SpecificationAttributeID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.String, displayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int SpecificationAttributeID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@SpecificationAttributeID"));
                specificationAttribute = GetSpecificationAttributeByID(SpecificationAttributeID);
            }
            return specificationAttribute;
        }

        /// <summary>
        /// Updates the specification attribute
        /// </summary>
        /// <param name="specificationAttributeID">The specification attribute identifier</param>
        /// <param name="name">The name</param>
        /// <param name="displayOrder">Display order</param>
        /// <returns>Specification attribute</returns>
        public override DBSpecificationAttribute UpdateSpecificationAttribute(int specificationAttributeID, string name, int displayOrder)
        {
            DBSpecificationAttribute specificationAttribute = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SpecificationAttributeUpdate");
            db.AddInParameter(dbCommand, "SpecificationAttributeID", DbType.Int32, specificationAttributeID);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.String, displayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                specificationAttribute = GetSpecificationAttributeByID(specificationAttributeID);

            return specificationAttribute;
        }

        #endregion

        #region SpecificationAttributeOption
        /// <summary>
        /// Gets a specification attribute option collection
        /// </summary>
        /// <returns>Specification attribute option collection</returns>
        public override DBSpecificationAttributeOptionCollection GetSpecificationAttributeOptions()
        {
            DBSpecificationAttributeOptionCollection saoCol = new DBSpecificationAttributeOptionCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SpecificationAttributeOptionLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBSpecificationAttributeOption sao = GetSpecificationAttributeOptionFromReader(dataReader);
                    saoCol.Add(sao);
                }
            }

            return saoCol;
        }

        /// <summary>
        /// Gets a specification attribute option
        /// </summary>
        /// <param name="specificationAttributeOptionID">The specification attribute option identifier</param>
        /// <returns>Specification attribute option</returns>
        public override DBSpecificationAttributeOption GetSpecificationAttributeOptionByID(int specificationAttributeOptionID)
        {
            DBSpecificationAttributeOption sao = null;
            if (specificationAttributeOptionID == 0)
                return sao;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SpecificationAttributeOptionLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "SpecificationAttributeOptionID", DbType.Int32, specificationAttributeOptionID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    sao = GetSpecificationAttributeOptionFromReader(dataReader);
                }
            }
            return sao;
        }

        /// <summary>
        /// Gets specification attribute option collection
        /// </summary>
        /// <param name="specificationAttributeID">Specification attribute unique identifier</param>
        /// <returns>Specification attribute option collection</returns>
        public override DBSpecificationAttributeOptionCollection GetSpecificationAttributeOptionsBySpecificationAttributeID(int specificationAttributeID)
        {
            DBSpecificationAttributeOptionCollection saoCol = new DBSpecificationAttributeOptionCollection();
            if (specificationAttributeID == 0)
                return saoCol;

            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SpecificationAttributeOptionLoadBySpecificationAttributeID");
            db.AddInParameter(dbCommand, "SpecificationAttributeID", DbType.Int32, specificationAttributeID);

            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBSpecificationAttributeOption sao = GetSpecificationAttributeOptionFromReader(dataReader);
                    saoCol.Add(sao);
                }
            }

            return saoCol;
        }

        /// <summary>
        /// Inserts a specification attribute option
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="displayOrder">Display order</param>
        /// <returns>Specification attribute option</returns>
        public override DBSpecificationAttributeOption InsertSpecificationAttributeOption(int specificationAttributeID, string name, int displayOrder)
        {
            DBSpecificationAttributeOption sao = null;

            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SpecificationAttributeOptionInsert");

            db.AddOutParameter(dbCommand, "SpecificationAttributeOptionID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "SpecificationAttributeID", DbType.Int32, specificationAttributeID);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.String, displayOrder);

            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int saoID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@SpecificationAttributeOptionID"));
                sao = GetSpecificationAttributeOptionByID(saoID);
            }
            return sao;
        }

        /// <summary>
        /// Updates the specification attribute option
        /// </summary>
        /// <param name="specificationAttributeOptionID">The specification attribute option identifier</param>
        /// <param name="name">The name</param>
        /// <param name="displayOrder">Display order</param>
        /// <returns>Specification attribute option</returns>
        public override DBSpecificationAttributeOption UpdateSpecificationAttributeOption(int specificationAttributeOptionID, int specificationAttributeID, string name, int displayOrder)
        {
            DBSpecificationAttributeOption sao = null;

            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SpecificationAttributeOptionUpdate");

            db.AddInParameter(dbCommand, "SpecificationAttributeOptionID", DbType.Int32, specificationAttributeOptionID);
            db.AddInParameter(dbCommand, "SpecificationAttributeID", DbType.Int32, specificationAttributeID);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.String, displayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                sao = GetSpecificationAttributeOptionByID(specificationAttributeOptionID);

            return sao;
        }

        /// <summary>
        /// Deletes a specification attribute option
        /// </summary>
        /// <param name="specificationAttributeOptionID">The specification attribute option identifier</param>
        public override void DeleteSpecificationAttributeOption(int specificationAttributeOptionID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SpecificationAttributeOptionDelete");
            db.AddInParameter(dbCommand, "SpecificationAttributeOptionID", DbType.Int32, specificationAttributeOptionID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }
        #endregion

        #region ProductSpecificationAttribute

        /// <summary>
        /// Deletes a product specification attribute mapping
        /// </summary>
        /// <param name="ProductSpecificationAttributeID">Product specification attribute identifier</param>
        public override void DeleteProductSpecificationAttribute(int ProductSpecificationAttributeID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Product_SpecificationAttribute_MappingDelete");
            db.AddInParameter(dbCommand, "ProductSpecificationAttributeID", DbType.Int32, ProductSpecificationAttributeID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a product specification attribute mapping collection
        /// </summary>
        /// <param name="ProductID">Product identifier</param>
        /// <param name="AllowFiltering">0 to load attributes with AllowFiltering set to false, 0 to load attributes with AllowFiltering set to true, null to load all attributes</param>
        /// <param name="ShowOnProductPage">0 to load attributes with ShowOnProductPage set to false, 0 to load attributes with ShowOnProductPage set to true, null to load all attributes</param>
        /// <returns>Product specification attribute mapping collection</returns>
        public override DBProductSpecificationAttributeCollection GetProductSpecificationAttributesByProductID(int ProductID, bool? AllowFiltering, bool? ShowOnProductPage)
        {
            DBProductSpecificationAttributeCollection productSpecificationAttributes = new DBProductSpecificationAttributeCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Product_SpecificationAttribute_MappingLoadByProductID");
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, ProductID);
            if (AllowFiltering.HasValue)
                db.AddInParameter(dbCommand, "AllowFiltering", DbType.Boolean, AllowFiltering.Value);
            else
                db.AddInParameter(dbCommand, "AllowFiltering", DbType.Boolean, null);
            if (ShowOnProductPage.HasValue)
                db.AddInParameter(dbCommand, "ShowOnProductPage", DbType.Boolean, ShowOnProductPage.Value);
            else
                db.AddInParameter(dbCommand, "ShowOnProductPage", DbType.Boolean, null);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBProductSpecificationAttribute productSpecificationAttribute = GetProductSpecificationAttributeFromReader(dataReader);
                    productSpecificationAttributes.Add(productSpecificationAttribute);
                }
            }

            return productSpecificationAttributes;
        }

        /// <summary>
        /// Gets a product specification attribute mapping 
        /// </summary>
        /// <param name="ProductSpecificationAttributeID">Product specification attribute mapping identifier</param>
        /// <returns>Product specification attribute mapping</returns>
        public override DBProductSpecificationAttribute GetProductSpecificationAttributeByID(int ProductSpecificationAttributeID)
        {
            DBProductSpecificationAttribute productSpecificationAttribute = null;
            if (ProductSpecificationAttributeID == 0)
                return productSpecificationAttribute;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Product_SpecificationAttribute_MappingLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ProductSpecificationAttributeID", DbType.Int32, ProductSpecificationAttributeID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    productSpecificationAttribute = GetProductSpecificationAttributeFromReader(dataReader);
                }
            }
            return productSpecificationAttribute;
        }

        /// <summary>
        /// Inserts a product specification attribute mapping
        /// </summary>
        /// <param name="productID">Product identifier</param>
        /// <param name="specificationAttributeOptionID">Specification attribute option identifier</param>
        /// <param name="allowFiltering">Allow product filtering by this attribute</param>
        /// <param name="showOnProductPage">Show the attribute on the product page</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Product specification attribute mapping</returns>
        public override DBProductSpecificationAttribute InsertProductSpecificationAttribute(int productID, int specificationAttributeOptionID,
                 bool allowFiltering, bool showOnProductPage, int displayOrder)
        {
            DBProductSpecificationAttribute productSpecificationAttribute = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Product_SpecificationAttribute_MappingInsert");
            db.AddOutParameter(dbCommand, "ProductSpecificationAttributeID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, productID);
            db.AddInParameter(dbCommand, "SpecificationAttributeOptionID", DbType.Int32, specificationAttributeOptionID);
            db.AddInParameter(dbCommand, "AllowFiltering", DbType.Boolean, allowFiltering);
            db.AddInParameter(dbCommand, "ShowOnProductPage", DbType.Boolean, showOnProductPage);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int ProductSpecificationAttributeID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ProductSpecificationAttributeID"));
                productSpecificationAttribute = GetProductSpecificationAttributeByID(ProductSpecificationAttributeID);
            }
            return productSpecificationAttribute;
        }

        /// <summary>
        /// Updates the product specification attribute mapping
        /// </summary>
        /// <param name="productSpecificationAttributeID">product specification attribute mapping identifier</param>
        /// <param name="productID">Product identifier</param>
        /// <param name="specificationAttributeOptionID">Specification attribute option identifier</param>
        /// <param name="allowFiltering">Allow product filtering by this attribute</param>
        /// <param name="showOnProductPage">Show the attribute onn the product page</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Product specification attribute mapping</returns>
        public override DBProductSpecificationAttribute UpdateProductSpecificationAttribute(int productSpecificationAttributeID,
               int productID, int specificationAttributeOptionID, bool allowFiltering, bool showOnProductPage, int displayOrder)
        {
            DBProductSpecificationAttribute productSpecificationAttribute = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Product_SpecificationAttribute_MappingUpdate");
            db.AddInParameter(dbCommand, "ProductSpecificationAttributeID", DbType.Int32, productSpecificationAttributeID);
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, productID);
            db.AddInParameter(dbCommand, "SpecificationAttributeOptionID", DbType.Int32, specificationAttributeOptionID);
            db.AddInParameter(dbCommand, "AllowFiltering", DbType.Boolean, allowFiltering);
            db.AddInParameter(dbCommand, "ShowOnProductPage", DbType.Boolean, showOnProductPage);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                productSpecificationAttribute = GetProductSpecificationAttributeByID(productSpecificationAttributeID);

            return productSpecificationAttribute;
        }

        #endregion

        #region SpecificationAttributeOptionFilter

        /// <summary>
        /// Gets a specification attribute option filter mapping collection by category id
        /// </summary>
        /// <param name="CategoryID">Product category identifier</param>
        /// <returns>Specification attribute option filter mapping collection</returns>
        public override DBSpecificationAttributeOptionFilterCollection GetSpecificationAttributeOptionFilterByCategoryID(int CategoryID)
        {
            DBSpecificationAttributeOptionFilterCollection col = new DBSpecificationAttributeOptionFilterCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SpecificationAttributeOptionFilter_LoadByFilter");
            
            db.AddInParameter(dbCommand, "CategoryID", DbType.Int32, CategoryID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBSpecificationAttributeOptionFilter item = GetSpecificationAttributeOptionFilterFromReader(dataReader);
                    col.Add(item);
                }
            }
            return col;
        }

        #endregion

        #endregion
    }
}
