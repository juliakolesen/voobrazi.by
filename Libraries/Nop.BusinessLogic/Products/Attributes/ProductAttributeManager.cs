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
using NopSolutions.NopCommerce.BusinessLogic.Caching;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Products.Attributes;

namespace NopSolutions.NopCommerce.BusinessLogic.Products.Attributes
{
    /// <summary>
    /// Product attribute manager
    /// </summary>
    public partial class ProductAttributeManager
    {
        #region Constants
        private const string PRODUCTATTRIBUTES_ALL_KEY = "Nop.productattribute.all";
        private const string PRODUCTATTRIBUTES_BY_ID_KEY = "Nop.productattribute.id-{0}";
        private const string PRODUCTVARIANTATTRIBUTES_ALL_KEY = "Nop.productvariantattribute.all-{0}";
        private const string PRODUCTVARIANTATTRIBUTES_BY_ID_KEY = "Nop.productvariantattribute.id-{0}";
        private const string PRODUCTVARIANTATTRIBUTEVALUES_ALL_KEY = "Nop.productvariantattributevalue.all-{0}";
        private const string PRODUCTVARIANTATTRIBUTEVALUES_BY_ID_KEY = "Nop.productvariantattributevalue.id-{0}";
        private const string PRODUCTATTRIBUTES_PATTERN_KEY = "Nop.productattribute.";
        private const string PRODUCTVARIANTATTRIBUTES_PATTERN_KEY = "Nop.productvariantattribute.";
        private const string PRODUCTVARIANTATTRIBUTEVALUES_PATTERN_KEY = "Nop.productvariantattributevalue.";
        #endregion

        #region Utilities
        private static ProductAttributeCollection DBMapping(DBProductAttributeCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            ProductAttributeCollection collection = new ProductAttributeCollection();
            foreach (DBProductAttribute dbItem in dbCollection)
            {
                ProductAttribute item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ProductAttribute DBMapping(DBProductAttribute dbItem)
        {
            if (dbItem == null)
                return null;

            ProductAttribute item = new ProductAttribute();
            item.ProductAttributeID = dbItem.ProductAttributeID;
            item.Name = dbItem.Name;
            item.Description = dbItem.Description;

            return item;
        }

        private static ProductVariantAttributeCollection DBMapping(DBProductVariantAttributeCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            ProductVariantAttributeCollection collection = new ProductVariantAttributeCollection();
            foreach (DBProductVariantAttribute dbItem in dbCollection)
            {
                ProductVariantAttribute item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ProductVariantAttribute DBMapping(DBProductVariantAttribute dbItem)
        {
            if (dbItem == null)
                return null;

            ProductVariantAttribute item = new ProductVariantAttribute();
            item.ProductVariantAttributeID = dbItem.ProductVariantAttributeID;
            item.ProductVariantID = dbItem.ProductVariantID;
            item.ProductAttributeID = dbItem.ProductAttributeID;
            item.TextPrompt = dbItem.TextPrompt;
            item.IsRequired = dbItem.IsRequired;
            item.AttributeControlTypeID = dbItem.AttributeControlTypeID;
            item.DisplayOrder = dbItem.DisplayOrder;

            return item;
        }

        private static ProductVariantAttributeValueCollection DBMapping(DBProductVariantAttributeValueCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            ProductVariantAttributeValueCollection collection = new ProductVariantAttributeValueCollection();
            foreach (DBProductVariantAttributeValue dbItem in dbCollection)
            {
                ProductVariantAttributeValue item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ProductVariantAttributeValue DBMapping(DBProductVariantAttributeValue dbItem)
        {
            if (dbItem == null)
                return null;

            ProductVariantAttributeValue item = new ProductVariantAttributeValue();
            item.ProductVariantAttributeValueID = dbItem.ProductVariantAttributeValueID;
            item.ProductVariantAttributeID = dbItem.ProductVariantAttributeID;
            item.Name = dbItem.Name;
            item.PriceAdjustment = dbItem.PriceAdjustment;
            item.WeightAdjustment = dbItem.WeightAdjustment;
            item.IsPreSelected = dbItem.IsPreSelected;
            item.DisplayOrder = dbItem.DisplayOrder;

            return item;
        }
       
        #endregion

        #region Methods
        /// <summary>
        /// Deletes a product attribute
        /// </summary>
        /// <param name="ProductAttributeID">Product attribute identifier</param>
        public static void DeleteProductAttribute(int ProductAttributeID)
        {
            DBProviderManager<DBProductAttributeProvider>.Provider.DeleteProductAttribute(ProductAttributeID);
            if (ProductAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTEVALUES_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets all product attributes
        /// </summary>
        /// <returns>Product attribute collection</returns>
        public static ProductAttributeCollection GetAllProductAttributes()
        {
            string key = PRODUCTATTRIBUTES_ALL_KEY;
            object obj2 = NopCache.Get(key);
            if (ProductAttributeManager.CacheEnabled && (obj2 != null))
            {
                return (ProductAttributeCollection)obj2;
            }

            DBProductAttributeCollection dbCollection = DBProviderManager<DBProductAttributeProvider>.Provider.GetAllProductAttributes();
            ProductAttributeCollection productAttributes = DBMapping(dbCollection);

            if (ProductAttributeManager.CacheEnabled)
            {
                NopCache.Max(key, productAttributes);
            }
            return productAttributes;
        }

        /// <summary>
        /// Gets a product attribute 
        /// </summary>
        /// <param name="ProductAttributeID">Product attribute identifier</param>
        /// <returns>Product attribute </returns>
        public static ProductAttribute GetProductAttributeByID(int ProductAttributeID)
        {
            if (ProductAttributeID == 0)
                return null;

            string key = string.Format(PRODUCTATTRIBUTES_BY_ID_KEY, ProductAttributeID);
            object obj2 = NopCache.Get(key);
            if (ProductAttributeManager.CacheEnabled && (obj2 != null))
            {
                return (ProductAttribute)obj2;
            }

            DBProductAttribute dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.GetProductAttributeByID(ProductAttributeID);
            ProductAttribute productAttribute = DBMapping(dbItem);

            if (ProductAttributeManager.CacheEnabled)
            {
                NopCache.Max(key, productAttribute);
            }
            return productAttribute;
        }

        /// <summary>
        /// Inserts a product attribute
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="Description">The description</param>
        /// <returns>Product attribute </returns>
        public static ProductAttribute InsertProductAttribute(string Name, string Description)
        {
            DBProductAttribute dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.InsertProductAttribute(Name, Description);
            ProductAttribute productAttribute = DBMapping(dbItem);
            if (ProductAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTEVALUES_PATTERN_KEY);
            }
            return productAttribute;
        }

        /// <summary>
        /// Updates the product attribute
        /// </summary>
        /// <param name="ProductAttributeID">Product attribute identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="Description">The description</param>
        /// <returns>Product attribute </returns>
        public static ProductAttribute UpdateProductAttribute(int ProductAttributeID, string Name,
            string Description)
        {
            DBProductAttribute dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.UpdateProductAttribute(ProductAttributeID,
                Name, Description);
            ProductAttribute productAttribute = DBMapping(dbItem);
            if (ProductAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTEVALUES_PATTERN_KEY);
            }

            return productAttribute;
        }

        /// <summary>
        /// Deletes a product variant attribute mapping
        /// </summary>
        /// <param name="ProductVariantAttributeID">Product variant attribute mapping identifier</param>
        public static void DeleteProductVariantAttribute(int ProductVariantAttributeID)
        {
            DBProviderManager<DBProductAttributeProvider>.Provider.DeleteProductVariantAttribute(ProductVariantAttributeID);
            if (ProductAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTEVALUES_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets product variant attribute mappings by product identifier
        /// </summary>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <returns>Product variant attribute mapping collection</returns>
        public static ProductVariantAttributeCollection GetProductVariantAttributesByProductVariantID(int ProductVariantID)
        {
            string key = string.Format(PRODUCTVARIANTATTRIBUTES_ALL_KEY, ProductVariantID);
            object obj2 = NopCache.Get(key);
            if (ProductAttributeManager.CacheEnabled && (obj2 != null))
            {
                return (ProductVariantAttributeCollection)obj2;
            }

            DBProductVariantAttributeCollection dbCollection = DBProviderManager<DBProductAttributeProvider>.Provider.GetProductVariantAttributesByProductVariantID(ProductVariantID);
            ProductVariantAttributeCollection productVariantAttributes = DBMapping(dbCollection);

            if (ProductAttributeManager.CacheEnabled)
            {
                NopCache.Max(key, productVariantAttributes);
            }
            return productVariantAttributes;
        }

        /// <summary>
        /// Gets a product variant attribute mapping
        /// </summary>
        /// <param name="ProductVariantAttributeID">Product variant attribute mapping identifier</param>
        /// <returns>Product variant attribute mapping</returns>
        public static ProductVariantAttribute GetProductVariantAttributeByID(int ProductVariantAttributeID)
        {
            if (ProductVariantAttributeID == 0)
                return null;

            string key = string.Format(PRODUCTVARIANTATTRIBUTES_BY_ID_KEY, ProductVariantAttributeID);
            object obj2 = NopCache.Get(key);
            if (ProductAttributeManager.CacheEnabled && (obj2 != null))
            {
                return (ProductVariantAttribute)obj2;
            }

            DBProductVariantAttribute dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.GetProductVariantAttributeByID(ProductVariantAttributeID);
            ProductVariantAttribute productVariantAttribute = DBMapping(dbItem);

            if (ProductAttributeManager.CacheEnabled)
            {
                NopCache.Max(key, productVariantAttribute);
            }
            return productVariantAttribute;
        }

        /// <summary>
        /// Inserts a product variant attribute mapping
        /// </summary>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <param name="ProductAttributeID">The product attribute identifier</param>
        /// <param name="TextPrompt">The text prompt</param>
        /// <param name="IsRequired">The value indicating whether the entity is required</param>
        /// <param name="AttributeControlType">The attribute control type</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Product variant attribute mapping</returns>
        public static ProductVariantAttribute InsertProductVariantAttribute(int ProductVariantID,
            int ProductAttributeID, string TextPrompt, bool IsRequired, AttributeControlTypeEnum AttributeControlType, int DisplayOrder)
        {
            DBProductVariantAttribute dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.InsertProductVariantAttribute(ProductVariantID,
                ProductAttributeID, TextPrompt, IsRequired, (int)AttributeControlType, DisplayOrder);
            ProductVariantAttribute productVariantAttribute = DBMapping(dbItem);

            if (ProductAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTEVALUES_PATTERN_KEY);
            }

            return productVariantAttribute;
        }

        /// <summary>
        /// Updates the product variant attribute mapping
        /// </summary>
        /// <param name="ProductVariantAttributeID">The product variant attribute mapping identifier</param>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <param name="ProductAttributeID">The product attribute identifier</param>
        /// <param name="TextPrompt">The text prompt</param>
        /// <param name="IsRequired">The value indicating whether the entity is required</param>
        /// <param name="AttributeControlType">The attribute control type</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Product variant attribute mapping</returns>
        public static ProductVariantAttribute UpdateProductVariantAttribute(int ProductVariantAttributeID, int ProductVariantID,
            int ProductAttributeID, string TextPrompt, bool IsRequired, AttributeControlTypeEnum AttributeControlType, int DisplayOrder)
        {
            DBProductVariantAttribute dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.UpdateProductVariantAttribute(ProductVariantAttributeID,
                ProductVariantID, ProductAttributeID, TextPrompt, IsRequired, (int)AttributeControlType, DisplayOrder);
            ProductVariantAttribute productVariantAttribute = DBMapping(dbItem);

            if (ProductAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTEVALUES_PATTERN_KEY);
            }

            return productVariantAttribute;
        }

        /// <summary>
        /// Deletes a product variant attribute value
        /// </summary>
        /// <param name="ProductVariantAttributeValueID">Product variant attribute value identifier</param>
        public static void DeleteProductVariantAttributeValue(int ProductVariantAttributeValueID)
        {
            DBProviderManager<DBProductAttributeProvider>.Provider.DeleteProductVariantAttributeValue(ProductVariantAttributeValueID);
            if (ProductAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTEVALUES_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets product variant attribute values by product identifier
        /// </summary>
        /// <param name="ProductVariantAttributeID">The product variant attribute mapping identifier</param>
        /// <returns>Product variant attribute mapping collection</returns>
        public static ProductVariantAttributeValueCollection GetProductVariantAttributeValues(int ProductVariantAttributeID)
        {
            string key = string.Format(PRODUCTVARIANTATTRIBUTEVALUES_ALL_KEY, ProductVariantAttributeID);
            object obj2 = NopCache.Get(key);
            if (ProductAttributeManager.CacheEnabled && (obj2 != null))
            {
                return (ProductVariantAttributeValueCollection)obj2;
            }

            DBProductVariantAttributeValueCollection dbCollection = DBProviderManager<DBProductAttributeProvider>.Provider.GetProductVariantAttributeValues(ProductVariantAttributeID);
            ProductVariantAttributeValueCollection productVariantAttributeValues = DBMapping(dbCollection);

            if (ProductAttributeManager.CacheEnabled)
            {
                NopCache.Max(key, productVariantAttributeValues);
            }
            return productVariantAttributeValues;
        }

        /// <summary>
        /// Gets a product variant attribute value
        /// </summary>
        /// <param name="ProductVariantAttributeValueID">Product variant attribute value identifier</param>
        /// <returns>Product variant attribute value</returns>
        public static ProductVariantAttributeValue GetProductVariantAttributeValueByID(int ProductVariantAttributeValueID)
        {
            if (ProductVariantAttributeValueID == 0)
                return null;

            string key = string.Format(PRODUCTVARIANTATTRIBUTEVALUES_BY_ID_KEY, ProductVariantAttributeValueID);
            object obj2 = NopCache.Get(key);
            if (ProductAttributeManager.CacheEnabled && (obj2 != null))
            {
                return (ProductVariantAttributeValue)obj2;
            }

            DBProductVariantAttributeValue dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.GetProductVariantAttributeValueByID(ProductVariantAttributeValueID);
            ProductVariantAttributeValue productVariantAttributeValue = DBMapping(dbItem);
            if (ProductAttributeManager.CacheEnabled)
            {
                NopCache.Max(key, productVariantAttributeValue);
            }
            return productVariantAttributeValue;
        }

        /// <summary>
        /// Inserts a product variant attribute value
        /// </summary>
        /// <param name="ProductVariantAttributeID">The product variant attribute mapping identifier</param>
        /// <param name="Name">The product variant attribute name</param>
        /// <param name="PriceAdjustment">The price adjustment</param>
        /// <param name="WeightAdjustment">The weight adjustment</param>
        /// <param name="IsPreSelected">The value indicating whether the value is pre-selected</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Product variant attribute value</returns>
        public static ProductVariantAttributeValue InsertProductVariantAttributeValue(int ProductVariantAttributeID,
            string Name, decimal PriceAdjustment, decimal WeightAdjustment,
            bool IsPreSelected, int DisplayOrder)
        {
            DBProductVariantAttributeValue dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.InsertProductVariantAttributeValue(ProductVariantAttributeID,
                Name, PriceAdjustment, WeightAdjustment, IsPreSelected, DisplayOrder);
            ProductVariantAttributeValue productVariantAttributeValue = DBMapping(dbItem);

            if (ProductAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTEVALUES_PATTERN_KEY);
            }

            return productVariantAttributeValue;
        }

        /// <summary>
        /// Updates the product variant attribute value
        /// </summary>
        /// <param name="ProductVariantAttributeValueID">The product variant attribute value identifier</param>
        /// <param name="ProductVariantAttributeID">The product variant attribute mapping identifier</param>
        /// <param name="Name">The product variant attribute name</param>
        /// <param name="PriceAdjustment">The price adjustment</param>
        /// <param name="WeightAdjustment">The weight adjustment</param>
        /// <param name="IsPreSelected">The value indicating whether the value is pre-selected</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Product variant attribute value</returns>
        public static ProductVariantAttributeValue UpdateProductVariantAttributeValue(int ProductVariantAttributeValueID,
            int ProductVariantAttributeID, string Name, decimal PriceAdjustment,
            decimal WeightAdjustment, bool IsPreSelected, int DisplayOrder)
        {
            DBProductVariantAttributeValue dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.UpdateProductVariantAttributeValue(ProductVariantAttributeValueID,
                ProductVariantAttributeID, Name, PriceAdjustment, WeightAdjustment, IsPreSelected, DisplayOrder);
            ProductVariantAttributeValue productVariantAttributeValue = DBMapping(dbItem);

            if (ProductAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTEVALUES_PATTERN_KEY);
            }

            return productVariantAttributeValue;
        }

        #endregion

        #region Property
        /// <summary>
        /// Gets a value indicating whether cache is enabled
        /// </summary>
        public static bool CacheEnabled
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("Cache.ProductAttributeManager.CacheEnabled");
            }
        }
        #endregion
    }
}
