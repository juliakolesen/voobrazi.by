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


namespace NopSolutions.NopCommerce.BusinessLogic.Products
{
    /// <summary>
    /// Represents a related product collection
    /// </summary>
    public partial class RelatedProductCollection : BaseEntityCollection<RelatedProduct>
    {
        /// <summary>
        /// Finds a related product item by specified identifiers
        /// </summary>
        /// <param name="ProductID1">The first product identifier</param>
        /// <param name="ProductID2">The second product identifier</param>
        /// <returns>Found item instance</returns>
        public RelatedProduct FindRelatedProduct(int ProductID1, int ProductID2)
        {
            #region Methods
            foreach (RelatedProduct relatedProduct in this)
                if (relatedProduct.ProductID1 == ProductID1 && relatedProduct.ProductID2 == ProductID2)
                    return relatedProduct;
            return null;
            #endregion
        }
    }
}
