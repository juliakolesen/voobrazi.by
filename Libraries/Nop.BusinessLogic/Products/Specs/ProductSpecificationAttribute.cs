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


namespace NopSolutions.NopCommerce.BusinessLogic.Products.Specs
{
    /// <summary>
    /// Represents a product specification attribute
    /// </summary>
    public partial class ProductSpecificationAttribute : BaseEntity
    {
        #region Ctor
        /// <summary>
        /// Creates a new instance of the ProductSpecificationAttribute class
        /// </summary>
        public ProductSpecificationAttribute()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the product specification attribute identifier
        /// </summary>
        public int ProductSpecificationAttributeID { get; set; }

        /// <summary>
        /// Gets or sets the product identifier
        /// </summary>
        public int ProductID { get; set; }

        /// <summary>
        /// Gets or sets the specification attribute option identifier
        /// </summary>
        public int SpecificationAttributeOptionID { get; set; }

        /// <summary>
        /// Gets or sets whether the attribute can be filtered by
        /// </summary>
        public bool AllowFiltering { get; set; }

        /// <summary>
        /// Gets or sets whether the attrbiute will be shown on the product page
        /// </summary>
        public bool ShowOnProductPage { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }
        #endregion 

        #region Custom Properties

        /// <summary>
        /// Gets the specification attribute
        /// </summary>
        public SpecificationAttribute SpecificationAttribute
        {
            get
            {
                SpecificationAttributeOption specificationAttributeOption = this.SpecificationAttributeOption;
                if (specificationAttributeOption != null)
                    return SpecificationAttributeManager.GetSpecificationAttributeByID(specificationAttributeOption.SpecificationAttributeID);
                else
                    return null;
            }
        }

        /// <summary>
        /// Gets the specification attribute option
        /// </summary>
        public SpecificationAttributeOption SpecificationAttributeOption
        {
            get
            {
                return SpecificationAttributeManager.GetSpecificationAttributeOptionByID(SpecificationAttributeOptionID);
            }
        }

        /// <summary>
        /// Gets the product
        /// </summary>
        public Product Product
        {
            get
            {
                return ProductManager.GetProductByID(ProductID);
            }
        }

        #endregion
    }
}
