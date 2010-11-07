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
using NopSolutions.NopCommerce.BusinessLogic.Categories;
using NopSolutions.NopCommerce.BusinessLogic.Manufacturers;
using NopSolutions.NopCommerce.BusinessLogic.Templates;
using NopSolutions.NopCommerce.Common;


namespace NopSolutions.NopCommerce.BusinessLogic.Products
{
    /// <summary>
    /// Represents a product
    /// </summary>
    public partial class Product : BaseEntity
    {
        #region Ctor
        /// <summary>
        /// Creates a new instance of the Product class
        /// </summary>
        public Product()
        {
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the product identifier
        /// </summary>
        public int ProductID { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the short description
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// Gets or sets the full description
        /// </summary>
        public string FullDescription { get; set; }

        /// <summary>
        /// Gets or sets the admin comment
        /// </summary>
        public string AdminComment { get; set; }

        /// <summary>
        /// Gets or sets the product type identifier
        /// </summary>
        public int ProductTypeID { get; set; }

        /// <summary>
        /// Gets or sets the template identifier
        /// </summary>
        public int TemplateID { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether to show the product on home page
        /// </summary>
        public bool ShowOnHomePage { get; set; }

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
        /// Gets or sets a value indicating whether the product allows customer reviews
        /// </summary>
        public bool AllowCustomerReviews { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the product allows customer ratings
        /// </summary>
        public bool AllowCustomerRatings { get; set; }

        /// <summary>
        /// Gets or sets the rating sum
        /// </summary>
        public int RatingSum { get; set; }

        /// <summary>
        /// Gets or sets the total rating votes
        /// </summary>
        public int TotalRatingVotes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is published
        /// </summary>
        public bool Published { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity has been deleted
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets the date and time of product creation
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the date and time of product update
        /// </summary>
        public DateTime UpdatedOn { get; set; }
        #endregion 

        #region Custom Properties
        /// <summary>
        /// Gets the product variants
        /// </summary>
        public ProductVariantCollection ProductVariants
        {
            get
            {
                return ProductManager.GetProductVariantsByProductID(ProductID);
            }
        }

        /// <summary>
        /// Indicates whether Product has more than one variant
        /// </summary>
        public bool HasMultipleVariants
        {
            get
            {
                return (this.ProductVariants.Count > 1);
            }
        }

        /// <summary>
        /// Gets the product template
        /// </summary>
        public ProductTemplate ProductTemplate
        {
            get
            {
                return TemplateManager.GetProductTemplateByID(TemplateID);
            }
        }

        /// <summary>
        /// Gets the product type
        /// </summary>
        public ProductType ProductType
        {
            get
            {
                return ProductManager.GetProductTypeByID(ProductTypeID);
            }
        }

        /// <summary>
        /// Gets the related products
        /// </summary>
        public RelatedProductCollection RelatedProducts
        {
            get
            {
                return ProductManager.GetRelatedProductsByProductID1(ProductID);
            }
        }

        /// <summary>
        /// Gets the product pictures
        /// </summary>
        public ProductPictureCollection ProductPictures
        {
            get
            {
                return ProductManager.GetProductPicturesByProductID(ProductID);
            }
        }

        /// <summary>
        /// Gets the product categories
        /// </summary>
        public ProductCategoryCollection ProductCategories
        {
            get
            {
                return CategoryManager.GetProductCategoriesByProductID(ProductID);
            }
        }

        /// <summary>
        /// Gets the product manufacturers
        /// </summary>
        public ProductManufacturerCollection ProductManufacturers
        {
            get
            {
                return ManufacturerManager.GetProductManufacturersByProductID(ProductID);
            }
        }

        /// <summary>
        /// Gets the product reviews
        /// </summary>
        public ProductReviewCollection ProductReviews
        {
            get
            {
                return ProductManager.GetProductReviewByProductID(ProductID);
            }
        }

        /// <summary>
        /// Returns the product variant with minimal price
        /// </summary>
        public ProductVariant MinimalPriceProductVariant
        {
            get
            {
                ProductVariantCollection productVariants = this.ProductVariants;
                productVariants.Sort(new GenericComparer<ProductVariant>
                    ("Price", GenericComparer<ProductVariant>.SortOrder.Ascending));
                if (productVariants.Count > 0)
                    return productVariants[0];
                else
                    return null;
            }
        }

        /// <summary>
        /// Returns the price range for product variants
        /// </summary>
        public PriceRange ProductPriceRange
        {
            get
            {
                ProductVariantCollection productVariants = this.ProductVariants;
                productVariants.Sort(new GenericComparer<ProductVariant>
                    ("Price", GenericComparer<ProductVariant>.SortOrder.Ascending));
                if (productVariants.Count > 0)
                {
                    return new PriceRange
                    {
                        From = productVariants[0].Price,
                        To = productVariants[(productVariants.Count - 1)].Price
                    };
                }
                else
                {
                    return new PriceRange(); 
                }
            }
        }
        #endregion
    }

}
