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
using System.Text;



namespace NopSolutions.NopCommerce.BusinessLogic.Products.Attributes
{
    /// <summary>
    /// Represents a product variant attribute value
    /// </summary>
    public partial class ProductVariantAttributeValue : BaseEntity
    {
        #region Ctor
        /// <summary>
        /// Creates a new instance of the ProductVariantAttributeValue class
        /// </summary>
        public ProductVariantAttributeValue()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the product variant attribute value identifier
        /// </summary>
        public int ProductVariantAttributeValueID { get; set; }

        /// <summary>
        /// Gets or sets the product variant attribute mapping identifier
        /// </summary>
        public int ProductVariantAttributeID { get; set; }

        /// <summary>
        /// Gets or sets the product variant attribute name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the price adjustment
        /// </summary>
        public decimal PriceAdjustment { get; set; }

        /// <summary>
        /// Gets or sets the weight adjustment
        /// </summary>
        public decimal WeightAdjustment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the value is pre-selected
        /// </summary>
        public bool IsPreSelected { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }
        #endregion

        #region Custom Properties
        /// <summary>
        /// Gets the product variant attribute
        /// </summary>
        public ProductVariantAttribute ProductVariantAttribute
        {
            get
            {
                return ProductAttributeManager.GetProductVariantAttributeByID(ProductVariantAttributeID);
            }
        }

        #endregion
    }

}
