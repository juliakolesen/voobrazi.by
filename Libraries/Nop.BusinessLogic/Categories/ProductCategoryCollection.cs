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



namespace NopSolutions.NopCommerce.BusinessLogic.Categories
{
    /// <summary>
    /// Represents a ProductCategory collection
    /// </summary>
    public partial class ProductCategoryCollection : BaseEntityCollection<ProductCategory>
    {
        #region Methods
        /// <summary>
        /// Returns a ProductCategory that has the specified values
        /// </summary>
        /// <param name="ProductID">Product identifier</param>
        /// <param name="CategoryID">Category identifier</param>
        /// <returns>A ProductCategory that has the specified values; otherwise null</returns>
        public ProductCategory FindProductCategory(int ProductID, int CategoryID)
        {
            foreach (ProductCategory productCategory in this)
                if (productCategory.ProductID == ProductID && productCategory.CategoryID == CategoryID)
                    return productCategory;
            return null;
        }
        #endregion
    }
}
