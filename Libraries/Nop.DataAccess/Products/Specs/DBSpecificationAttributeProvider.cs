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
using System.Data;
using System.Xml;

namespace NopSolutions.NopCommerce.DataAccess.Products.Specs
{
    /// <summary>
    /// Acts as a base class for deriving custom specification attribute provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/SpecificationAttributeProvider")]
    public abstract partial class DBSpecificationAttributeProvider : BaseDBProvider
    {
        #region Methods

        #region Specification attribute

        /// <summary>
        /// Gets a specification attribute
        /// </summary>
        /// <param name="SpecificationAttributeID">The specification attribute identifier</param>
        /// <returns>Specification attribute</returns>
        public abstract DBSpecificationAttribute GetSpecificationAttributeByID(int SpecificationAttributeID);

        /// <summary>
        /// Gets specification attribute collection
        /// </summary>
        /// <returns>Specification attribute collection</returns>
        public abstract DBSpecificationAttributeCollection GetSpecificationAttributes();

        /// <summary>
        /// Deletes a specification attribute
        /// </summary>
        /// <param name="SpecificationAttributeID">The specification attribute identifier</param>
        public abstract void DeleteSpecificationAttribute(int SpecificationAttributeID);

        /// <summary>
        /// Inserts a specification attribute
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="displayOrder">Display order</param>
        /// <returns>Specification attribute</returns>
        public abstract DBSpecificationAttribute InsertSpecificationAttribute(string name, int displayOrder);

        /// <summary>
        /// Updates the specification attribute
        /// </summary>
        /// <param name="specificationAttributeID">The specification attribute identifier</param>
        /// <param name="name">The name</param>
        /// <param name="displayOrder">Display order</param>
        /// <returns>Specification attribute</returns>
        public abstract DBSpecificationAttribute UpdateSpecificationAttribute(int specificationAttributeID, string name, int displayOrder);

        #endregion

        #region Specification attribute option

        /// <summary>
        /// Gets a specification attribute option collection
        /// </summary>
        /// <returns>Specification attribute option collection</returns>
        public abstract DBSpecificationAttributeOptionCollection GetSpecificationAttributeOptions();

        /// <summary>
        /// Gets a specification attribute option
        /// </summary>
        /// <param name="specificationAttributeOptionID">The specification attribute option identifier</param>
        /// <returns>Specification attribute option</returns>
        public abstract DBSpecificationAttributeOption GetSpecificationAttributeOptionByID(int specificationAttributeOptionID);

        /// <summary>
        /// Gets specification attribute option collection
        /// </summary>
        /// <param name="specificationAttributeID">Specification attribute unique identifier</param>
        /// <returns>Specification attribute option collection</returns>
        public abstract DBSpecificationAttributeOptionCollection GetSpecificationAttributeOptionsBySpecificationAttributeID(int specificationAttributeID);

        /// <summary>
        /// Inserts a specification attribute option
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="displayOrder">Display order</param>
        /// <returns>Specification attribute option</returns>
        public abstract DBSpecificationAttributeOption InsertSpecificationAttributeOption(int specificationAttributeID, string name, int displayOrder);

        /// <summary>
        /// Updates the specification attribute option
        /// </summary>
        /// <param name="specificationAttributeOptionID">The specification attribute option identifier</param>
        /// <param name="specificationAttributeID">The specification attribute identifier</param>
        /// <param name="name">The name</param>
        /// <param name="displayOrder">Display order</param>
        /// <returns>Specification attribute option</returns>
        public abstract DBSpecificationAttributeOption UpdateSpecificationAttributeOption(int specificationAttributeOptionID, int specificationAttributeID, string name, int displayOrder);

        /// <summary>
        /// Deletes a specification attribute option
        /// </summary>
        /// <param name="specificationAttributeOptionID">The specification attribute option identifier</param>
        public abstract void DeleteSpecificationAttributeOption(int specificationAttributeOptionID);

        #endregion

        #region Product specification attribute

        /// <summary>
        /// Gets a product specification attribute mapping collection
        /// </summary>
        /// <param name="ProductID">Product identifier</param>
        /// <param name="AllowFiltering">0 to load attributes with AllowFiltering set to false, 0 to load attributes with AllowFiltering set to true, null to load all attributes</param>
        /// <param name="ShowOnProductPage">0 to load attributes with ShowOnProductPage set to false, 0 to load attributes with ShowOnProductPage set to true, null to load all attributes</param>
        /// <returns>Product specification attribute mapping collection</returns>
        public abstract DBProductSpecificationAttributeCollection GetProductSpecificationAttributesByProductID(int ProductID, bool? AllowFiltering, bool? ShowOnProductPage);

        /// <summary>
        /// Gets a product specification attribute mapping 
        /// </summary>
        /// <param name="ProductSpecificationAttributeID">Product specification attribute mapping identifier</param>
        /// <returns>Product specification attribute mapping</returns>
        public abstract DBProductSpecificationAttribute GetProductSpecificationAttributeByID(int ProductSpecificationAttributeID);

        /// <summary>
        /// Gets all specification attribute option filter mapping collection by category id
        /// </summary>
        /// <param name="CategoryID">Product category identifier</param>
        /// <returns>Specification attribute option filter mapping collection</returns>
        public abstract DBSpecificationAttributeOptionFilterCollection GetSpecificationAttributeOptionFilterByCategoryID(int CategoryID);

        /// <summary>
        /// Inserts a product specification attribute mapping
        /// </summary>
        /// <param name="productID">Product identifier</param>
        /// <param name="specificationAttributeOptionID">Specification attribute option identifier</param>
        /// <param name="allowFiltering">Allow product filtering by this attribute</param>
        /// <param name="showOnProductPage">Show the attribute on the product page</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Product specification attribute mapping</returns>
        public abstract DBProductSpecificationAttribute InsertProductSpecificationAttribute(int productID, int specificationAttributeOptionID,
                 bool allowFiltering, bool showOnProductPage, int displayOrder);

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
        public abstract DBProductSpecificationAttribute UpdateProductSpecificationAttribute(int productSpecificationAttributeID,
               int productID, int specificationAttributeOptionID, bool allowFiltering, bool showOnProductPage, int displayOrder);

        /// <summary>
        /// Deletes a product specification attribute mapping
        /// </summary>
        /// <param name="ProductSpecificationAttributeID">Product specification attribute identifier</param>
        public abstract void DeleteProductSpecificationAttribute(int ProductSpecificationAttributeID);

        #endregion

        #endregion
    }
}
