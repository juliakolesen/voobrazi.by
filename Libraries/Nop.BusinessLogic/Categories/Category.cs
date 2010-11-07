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
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.Promo.Discounts;
using NopSolutions.NopCommerce.BusinessLogic.Templates;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using System.Globalization;



namespace NopSolutions.NopCommerce.BusinessLogic.Categories
{
    /// <summary>
    /// Represents a category
    /// </summary>
    public partial class Category : BaseEntity
    {
        #region Ctor
        /// <summary>
        /// Creates a new instance of the Category class
        /// </summary>
        public Category()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the category identifier
        /// </summary>
        public int CategoryID { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the template identifier
        /// </summary>
        public int TemplateID { get; set; }

        /// <summary>
        /// Gets or sets the meta keywords
        /// </summary>
        public string MetaKeywords { get; set; }

        /// <summary>
        /// Gets or sets the meta description
        /// </summary>
        public string MetaDescription { get; set; }

        /// <summary>
        /// Gets or sets the meta title
        /// </summary>
        public string MetaTitle { get; set; }

        /// <summary>
        /// Gets or sets the search-engine name
        /// </summary>
        public string SEName { get; set; }

        /// <summary>
        /// Gets or sets the parent category identifier
        /// </summary>
        public int ParentCategoryID { get; set; }

        /// <summary>
        /// Gets or sets the picture identifier
        /// </summary>
        public int PictureID { get; set; }

        /// <summary>
        /// Gets or sets the page size
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the available price ranges
        /// </summary>
        public string PriceRanges { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is published
        /// </summary>
        public bool Published { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity has been deleted
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }
        
        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance update
        /// </summary>
        public DateTime UpdatedOn { get; set; }
        #endregion

        #region Custom Properties

        /// <summary>
        /// Gets the parent category
        /// </summary>
        public Category ParentCategory
        {
            get
            {
                return CategoryManager.GetCategoryByID(ParentCategoryID);
            }
        }

        /// <summary>
        /// Gets the category template
        /// </summary>
        public CategoryTemplate CategoryTemplate
        {
            get
            {
                return TemplateManager.GetCategoryTemplateByID(TemplateID);
            }
        }

        /// <summary>
        /// Gets the products
        /// </summary>
        public ProductCategoryCollection ProductCategories
        {
            get
            {
                return CategoryManager.GetProductCategoriesByCategoryID(CategoryID);
            }
        }

        /// <summary>
        /// Gets the picture
        /// </summary>
        public Picture Picture
        {
            get
            {
                return PictureManager.GetPictureByID(PictureID);
            }
        }

        /// <summary>
        /// Gets the discounts of the category
        /// </summary>
        public DiscountCollection Discounts
        {
            get
            {
                return DiscountManager.GetDiscountsByCategoryID(CategoryID);
            }
        }

        /// <summary>
        /// Gets the featured products of the category
        /// </summary>
        public ProductCollection FeaturedProducts
        {
            get
            {
                int totalFeaturedRecords = 0;
                ProductCollection featuredProducts = ProductManager.GetAllProducts(CategoryID,
                    0, true, int.MaxValue - 1, 0, out totalFeaturedRecords);
                return featuredProducts;
            }
        }

        #endregion
    }
}
