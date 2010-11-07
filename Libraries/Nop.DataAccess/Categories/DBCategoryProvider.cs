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


namespace NopSolutions.NopCommerce.DataAccess.Categories
{
    /// <summary>
    /// Acts as a base class for deriving custom category provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/CategoryProvider")]
    public abstract partial class DBCategoryProvider : BaseDBProvider
    {
        #region Methods
        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="ParentCategoryID">Parent category identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Category collection</returns>
        public abstract DBCategoryCollection GetAllCategories(int ParentCategoryID, bool showHidden);

        /// <summary>
        /// Gets a category
        /// </summary>
        /// <param name="CategoryID">Category identifier</param>
        /// <returns>Category</returns>
        public abstract DBCategory GetCategoryByID(int CategoryID);
        
        /// <summary>
        /// Inserts category identifier
        /// </summary>
        /// <param name="Name">The category name</param>
        /// <param name="Description">The description</param>
        /// <param name="TemplateID">The template identifier</param>
        /// <param name="MetaKeywords">The meta keywords</param>
        /// <param name="MetaDescription">The meta description</param>
        /// <param name="MetaTitle">The meta title</param>
        /// <param name="SEName">The search-engine name</param>
        /// <param name="ParentCategoryID">The parent category identifier</param>
        /// <param name="PictureID">The picture identifier</param>
        /// <param name="PageSize">The page size</param>
        /// <param name="PriceRanges">The price ranges</param>
        /// <param name="Published">A value indicating whether the entity is published</param>
        /// <param name="Deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Category</returns>
        public abstract DBCategory InsertCategory(string Name, string Description,
            int TemplateID, string MetaKeywords, string MetaDescription, string MetaTitle,
            string SEName, int ParentCategoryID, int PictureID, int PageSize, string PriceRanges, bool Published, bool Deleted,
            int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn);

        /// <summary>
        /// Updates the category
        /// </summary>
        /// <param name="CategoryID">Category identifier</param>
        /// <param name="Name">The category name</param>
        /// <param name="Description">The description</param>
        /// <param name="TemplateID">The template identifier</param>
        /// <param name="MetaKeywords">The meta keywords</param>
        /// <param name="MetaDescription">The meta description</param>
        /// <param name="MetaTitle">The meta title</param>
        /// <param name="SEName">The search-engine name</param>
        /// <param name="ParentCategoryID">The parent category identifier</param>
        /// <param name="PictureID">The picture identifier</param>
        /// <param name="PageSize">The page size</param>
        /// <param name="PriceRanges">The price ranges</param>
        /// <param name="Published">A value indicating whether the entity is published</param>
        /// <param name="Deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Category</returns>
        public abstract DBCategory UpdateCategory(int CategoryID, string Name, string Description,
            int TemplateID, string MetaKeywords, string MetaDescription, string MetaTitle,
            string SEName, int ParentCategoryID, int PictureID, int PageSize, string PriceRanges, bool Published, bool Deleted,
            int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn);

        /// <summary>
        /// Deletes a product category mapping
        /// </summary>
        /// <param name="ProductCategoryID">Product category identifier</param>
        public abstract void DeleteProductCategory(int ProductCategoryID);

        /// <summary>
        /// Gets product category mapping collection
        /// </summary>
        /// <param name="CategoryID">Category identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product a category mapping collection</returns>
        public abstract DBProductCategoryCollection GetProductCategoriesByCategoryID(int CategoryID, bool showHidden);

        /// <summary>
        /// Gets a product category mapping collection
        /// </summary>
        /// <param name="ProductID">Product identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product category mapping collection</returns>
        public abstract DBProductCategoryCollection GetProductCategoriesByProductID(int ProductID, bool showHidden);

        /// <summary>
        /// Gets a product category mapping 
        /// </summary>
        /// <param name="ProductCategoryID">Product category mapping identifier</param>
        /// <returns>Product category mapping</returns>
        public abstract DBProductCategory GetProductCategoryByID(int ProductCategoryID);

        /// <summary>
        /// Inserts a product category mapping
        /// </summary>
        /// <param name="ProductID">Product identifier</param>
        /// <param name="CategoryID">Category identifier</param>
        /// <param name="IsFeaturedProduct">A value indicating whether the product is featured</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Product category mapping </returns>
        public abstract DBProductCategory InsertProductCategory(int ProductID, int CategoryID,
            bool IsFeaturedProduct, int DisplayOrder);

        /// <summary>
        /// Updates the product category mapping 
        /// </summary>
        /// <param name="ProductCategoryID">Product category mapping  identifier</param>
        /// <param name="ProductID">Product identifier</param>
        /// <param name="CategoryID">Category identifier</param>
        /// <param name="IsFeaturedProduct">A value indicating whether the product is featured</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Product category mapping </returns>
        public abstract DBProductCategory UpdateProductCategory(int ProductCategoryID,
            int ProductID, int CategoryID, bool IsFeaturedProduct, int DisplayOrder);
        #endregion
    }
}
