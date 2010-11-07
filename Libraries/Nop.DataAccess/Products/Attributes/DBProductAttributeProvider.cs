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
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Web.Configuration;
using System.Web.Hosting;

namespace NopSolutions.NopCommerce.DataAccess.Products.Attributes
{
    /// <summary>
    /// Acts as a base class for deriving custom product attribute provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/ProductAttributeProvider")]
    public abstract partial class DBProductAttributeProvider : BaseDBProvider
    {
        #region Methods
        /// <summary>
        /// Deletes a product attribute
        /// </summary>
        /// <param name="ProductAttributeID">Product attribute identifier</param>
        public abstract void DeleteProductAttribute(int ProductAttributeID);

        /// <summary>
        /// Gets all product attributes
        /// </summary>
        /// <returns>Product attribute collection</returns>
        public abstract DBProductAttributeCollection GetAllProductAttributes();

        /// <summary>
        /// Gets a product attribute 
        /// </summary>
        /// <param name="ProductAttributeID">Product attribute identifier</param>
        /// <returns>Product attribute </returns>
        public abstract DBProductAttribute GetProductAttributeByID(int ProductAttributeID);

        /// <summary>
        /// Inserts a product attribute
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="Description">The description</param>
        /// <returns>Product attribute </returns>
        public abstract DBProductAttribute InsertProductAttribute(string Name, string Description);

        /// <summary>
        /// Updates the product attribute
        /// </summary>
        /// <param name="ProductAttributeID">Product attribute identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="Description">The description</param>
        /// <returns>Product attribute </returns>
        public abstract DBProductAttribute UpdateProductAttribute(int ProductAttributeID, string Name,
            string Description);

        /// <summary>
        /// Deletes a product variant attribute mapping
        /// </summary>
        /// <param name="ProductVariantAttributeID">Product variant attribute mapping identifier</param>
        public abstract void DeleteProductVariantAttribute(int ProductVariantAttributeID);

        /// <summary>
        /// Gets product variant attribute mappings by product identifier
        /// </summary>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <returns>Product variant attribute mapping collection</returns>
        public abstract DBProductVariantAttributeCollection GetProductVariantAttributesByProductVariantID(int ProductVariantID);

        /// <summary>
        /// Gets a product variant attribute mapping
        /// </summary>
        /// <param name="ProductVariantAttributeID">Product variant attribute mapping identifier</param>
        /// <returns>Product variant attribute mapping</returns>
        public abstract DBProductVariantAttribute GetProductVariantAttributeByID(int ProductVariantAttributeID);

        /// <summary>
        /// Inserts a product variant attribute mapping
        /// </summary>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <param name="ProductAttributeID">The product attribute identifier</param>
        /// <param name="TextPrompt">The text prompt</param>
        /// <param name="IsRequired">The value indicating whether the entity is required</param>
        /// <param name="AttributeControlTypeID">The attribute control type identifier</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Product variant attribute mapping</returns>
        public abstract DBProductVariantAttribute InsertProductVariantAttribute(int ProductVariantID,
            int ProductAttributeID, string  TextPrompt, bool IsRequired, int AttributeControlTypeID, int DisplayOrder);

        /// <summary>
        /// Updates the product variant attribute mapping
        /// </summary>
        /// <param name="ProductVariantAttributeID">The product variant attribute mapping identifier</param>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <param name="ProductAttributeID">The product attribute identifier</param>
        /// <param name="TextPrompt">The text prompt</param>
        /// <param name="IsRequired">The value indicating whether the entity is required</param>
        /// <param name="AttributeControlTypeID">The attribute control type identifier</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Product variant attribute mapping</returns>
        public abstract DBProductVariantAttribute UpdateProductVariantAttribute(int ProductVariantAttributeID, int ProductVariantID,
            int ProductAttributeID, string TextPrompt, bool IsRequired, int AttributeControlTypeID, int DisplayOrder);

        /// <summary>
        /// Deletes a product variant attribute value
        /// </summary>
        /// <param name="ProductVariantAttributeValueID">Product variant attribute value identifier</param>
        public abstract void DeleteProductVariantAttributeValue(int ProductVariantAttributeValueID);

        /// <summary>
        /// Gets product variant attribute values by product identifier
        /// </summary>
        /// <param name="ProductVariantAttributeID">The product variant attribute mapping identifier</param>
        /// <returns>Product variant attribute mapping collection</returns>
        public abstract DBProductVariantAttributeValueCollection GetProductVariantAttributeValues(int ProductVariantAttributeID);

        /// <summary>
        /// Gets a product variant attribute value
        /// </summary>
        /// <param name="ProductVariantAttributeValueID">Product variant attribute value identifier</param>
        /// <returns>Product variant attribute value</returns>
        public abstract DBProductVariantAttributeValue GetProductVariantAttributeValueByID(int ProductVariantAttributeValueID);

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
        public abstract DBProductVariantAttributeValue InsertProductVariantAttributeValue(int ProductVariantAttributeID,
            string Name, decimal PriceAdjustment, decimal WeightAdjustment,
            bool IsPreSelected, int DisplayOrder);

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
        public abstract DBProductVariantAttributeValue UpdateProductVariantAttributeValue(int ProductVariantAttributeValueID,
            int ProductVariantAttributeID, string Name, decimal PriceAdjustment, 
            decimal WeightAdjustment, bool IsPreSelected, int DisplayOrder);
        
        #endregion
    }
}
