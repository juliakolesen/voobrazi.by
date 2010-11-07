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
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Categories;

namespace NopSolutions.NopCommerce.BusinessLogic.Categories
{
    /// <summary>
    /// Category manager
    /// </summary>
    public partial class CategoryManager
    {
        #region Constants
        private const string CATEGORIES_ALL_KEY = "Nop.category.all-{0}-{1}";
        private const string CATEGORIES_BY_ID_KEY = "Nop.category.id-{0}";
        private const string PRODUCTCATEGORIES_ALLBYCATEGORYID_KEY = "Nop.productcategory.allbycategoryid-{0}-{1}";
        private const string PRODUCTCATEGORIES_ALLBYPRODUCTID_KEY = "Nop.productcategory.allbyproductid-{0}-{1}";
        private const string PRODUCTCATEGORIES_BY_ID_KEY = "Nop.productcategory.id-{0}";
        private const string CATEGORIES_PATTERN_KEY = "Nop.category.";
        private const string PRODUCTCATEGORIES_PATTERN_KEY = "Nop.productcategory.";

        #endregion

        #region Utilities
        private static CategoryCollection DBMapping(DBCategoryCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            CategoryCollection collection = new CategoryCollection();
            foreach (DBCategory dbItem in dbCollection)
            {
                Category item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static Category DBMapping(DBCategory dbItem)
        {
            if (dbItem == null)
                return null;

            Category item = new Category();
            item.CategoryID = dbItem.CategoryID;
            item.Name = dbItem.Name;
            item.Description = dbItem.Description;
            item.TemplateID = dbItem.TemplateID;
            item.MetaKeywords = dbItem.MetaKeywords;
            item.MetaDescription = dbItem.MetaDescription;
            item.MetaTitle = dbItem.MetaTitle;
            item.SEName = dbItem.SEName;
            item.ParentCategoryID = dbItem.ParentCategoryID;
            item.PictureID = dbItem.PictureID;
            item.PageSize = dbItem.PageSize;
            item.PriceRanges = dbItem.PriceRanges;
            item.Published = dbItem.Published;
            item.Deleted = dbItem.Deleted;
            item.DisplayOrder = dbItem.DisplayOrder;
            item.CreatedOn = dbItem.CreatedOn;
            item.UpdatedOn = dbItem.UpdatedOn;

            return item;
        }

        private static ProductCategoryCollection DBMapping(DBProductCategoryCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            ProductCategoryCollection collection = new ProductCategoryCollection();
            foreach (DBProductCategory dbItem in dbCollection)
            {
                ProductCategory item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ProductCategory DBMapping(DBProductCategory dbItem)
        {
            if (dbItem == null)
                return null;

            ProductCategory item = new ProductCategory();
            item.ProductCategoryID = dbItem.ProductCategoryID;
            item.ProductID = dbItem.ProductID;
            item.CategoryID = dbItem.CategoryID;
            item.IsFeaturedProduct = dbItem.IsFeaturedProduct;
            item.DisplayOrder = dbItem.DisplayOrder;

            return item;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Marks category as deleted
        /// </summary>
        /// <param name="CategoryID">Category identifier</param>
        public static void MarkCategoryAsDeleted(int CategoryID)
        {
            Category category = GetCategoryByID(CategoryID);
            if (category != null)
            {
                category = UpdateCategory(category.CategoryID, category.Name, category.Description, category.TemplateID, category.MetaKeywords,
                     category.MetaDescription, category.MetaTitle, category.SEName, category.ParentCategoryID,
                     category.PictureID, category.PageSize, category.PriceRanges, 
                     category.Published, true, category.DisplayOrder,
                     category.CreatedOn, category.UpdatedOn);
            }
        }

        /// <summary>
        /// Removes category picture
        /// </summary>
        /// <param name="CategoryID">Category identifier</param>
        public static void RemoveCategoryPicture(int CategoryID)
        {
            Category category = GetCategoryByID(CategoryID);
            if (category != null)
            {
                UpdateCategory(category.CategoryID, category.Name, category.Description, category.TemplateID, category.MetaKeywords,
                   category.MetaDescription, category.MetaTitle, category.SEName, category.ParentCategoryID,
                   0, category.PageSize, category.PriceRanges, 
                   category.Published, category.Deleted, category.DisplayOrder,
                   category.CreatedOn, category.UpdatedOn);
            }
        }

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="ParentCategoryID">Parent category identifier</param>
        /// <returns>Category collection</returns>
        public static CategoryCollection GetAllCategories(int ParentCategoryID)
        {
            bool showHidden = NopContext.Current.IsAdmin;
            return GetAllCategories(ParentCategoryID, showHidden);
        }

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="ParentCategoryID">Parent category identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Category collection</returns>
        public static CategoryCollection GetAllCategories(int ParentCategoryID, bool showHidden)
        {
            string key = string.Format(CATEGORIES_ALL_KEY, showHidden, ParentCategoryID);
            object obj2 = NopCache.Get(key);
            if (CategoryManager.CategoriesCacheEnabled && (obj2 != null))
            {
                return (CategoryCollection)obj2;
            }
            DBCategoryCollection dbCollection = DBProviderManager<DBCategoryProvider>.Provider.GetAllCategories(ParentCategoryID, showHidden);
            CategoryCollection categoryCollection = DBMapping(dbCollection);

            if (CategoryManager.CategoriesCacheEnabled)
            {
                NopCache.Max(key, categoryCollection);
            }
            return categoryCollection;
        }

        /// <summary>
        /// Gets a category
        /// </summary>
        /// <param name="CategoryID">Category identifier</param>
        /// <returns>Category</returns>
        public static Category GetCategoryByID(int CategoryID)
        {
            if (CategoryID == 0)
                return null;

            string key = string.Format(CATEGORIES_BY_ID_KEY, CategoryID);
            object obj2 = NopCache.Get(key);
            if (CategoryManager.CategoriesCacheEnabled && (obj2 != null))
            {
                return (Category)obj2;
            }
            DBCategory dbItem = DBProviderManager<DBCategoryProvider>.Provider.GetCategoryByID(CategoryID);
            Category category = DBMapping(dbItem);

            if (CategoryManager.CategoriesCacheEnabled)
            {
                NopCache.Max(key, category);
            }
            return category;
        }

        /// <summary>
        /// Gets a category breadcrumb
        /// </summary>
        /// <param name="CategoryID">Category identifier</param>
        /// <returns>Category</returns>
        public static CategoryCollection GetBreadCrumb(int CategoryID)
        {
            CategoryCollection breadCrumb = new CategoryCollection();
            Category category = GetCategoryByID(CategoryID);
            while (category != null)
            {
                breadCrumb.Add(category);
                category = category.ParentCategory;
            }
            breadCrumb.Reverse();
            return breadCrumb;
        }

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
        public static Category InsertCategory(string Name, string Description,
            int TemplateID, string MetaKeywords, string MetaDescription, string MetaTitle,
            string SEName, int ParentCategoryID, int PictureID, int PageSize, string PriceRanges,
            bool Published, bool Deleted, int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            UpdatedOn = DateTimeHelper.ConvertToUtcTime(UpdatedOn);

            DBCategory dbItem = DBProviderManager<DBCategoryProvider>.Provider.InsertCategory(Name, Description,
                TemplateID, MetaKeywords, MetaDescription, MetaTitle,
                SEName, ParentCategoryID, PictureID, PageSize,PriceRanges, Published, Deleted,
                DisplayOrder, CreatedOn, UpdatedOn);

            Category category = DBMapping(dbItem);

            if (CategoryManager.CategoriesCacheEnabled || CategoryManager.MappingsCacheEnabled)
            {
                NopCache.RemoveByPattern(CATEGORIES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTCATEGORIES_PATTERN_KEY);
            }

            return category;
        }

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
        public static Category UpdateCategory(int CategoryID, string Name, string Description,
            int TemplateID, string MetaKeywords, string MetaDescription, string MetaTitle,
            string SEName, int ParentCategoryID, int PictureID, int PageSize, string PriceRanges,
            bool Published, bool Deleted, int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            UpdatedOn = DateTimeHelper.ConvertToUtcTime(UpdatedOn);

            //validate category hierarchy
            Category parentCategory = GetCategoryByID(ParentCategoryID);
            while (parentCategory != null)
            {
                if (CategoryID == parentCategory.CategoryID)
                {
                    ParentCategoryID = 0;
                    break;
                }
                parentCategory = GetCategoryByID(parentCategory.ParentCategoryID);
            }

            DBCategory dbItem = DBProviderManager<DBCategoryProvider>.Provider.UpdateCategory(CategoryID, Name, Description,
            TemplateID, MetaKeywords, MetaDescription, MetaTitle,
            SEName, ParentCategoryID, PictureID, PageSize, PriceRanges, Published, Deleted,
            DisplayOrder, CreatedOn, UpdatedOn);

            Category category = DBMapping(dbItem);
            if (CategoryManager.CategoriesCacheEnabled || CategoryManager.MappingsCacheEnabled)
            {
                NopCache.RemoveByPattern(CATEGORIES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTCATEGORIES_PATTERN_KEY);
            }

            return category;
        }

        /// <summary>
        /// Deletes a product category mapping
        /// </summary>
        /// <param name="ProductCategoryID">Product category identifier</param>
        public static void DeleteProductCategory(int ProductCategoryID)
        {
            DBProviderManager<DBCategoryProvider>.Provider.DeleteProductCategory(ProductCategoryID);

            if (CategoryManager.CategoriesCacheEnabled || CategoryManager.MappingsCacheEnabled)
            {
                NopCache.RemoveByPattern(CATEGORIES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTCATEGORIES_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets product category mapping collection
        /// </summary>
        /// <param name="CategoryID">Category identifier</param>
        /// <returns>Product a category mapping collection</returns>
        public static ProductCategoryCollection GetProductCategoriesByCategoryID(int CategoryID)
        {
            bool showHidden = NopContext.Current.IsAdmin;
            string key = string.Format(PRODUCTCATEGORIES_ALLBYCATEGORYID_KEY, showHidden, CategoryID);
            object obj2 = NopCache.Get(key);
            if (CategoryManager.MappingsCacheEnabled && (obj2 != null))
            {
                return (ProductCategoryCollection)obj2;
            }

            DBProductCategoryCollection dbCollection = DBProviderManager<DBCategoryProvider>.Provider.GetProductCategoriesByCategoryID(CategoryID, showHidden);
            ProductCategoryCollection productCategoryCollection = DBMapping(dbCollection);

            if (CategoryManager.MappingsCacheEnabled)
            {
                NopCache.Max(key, productCategoryCollection);
            }
            return productCategoryCollection;
        }

        /// <summary>
        /// Gets a product category mapping collection
        /// </summary>
        /// <param name="ProductID">Product identifier</param>
        /// <returns>Product category mapping collection</returns>
        public static ProductCategoryCollection GetProductCategoriesByProductID(int ProductID)
        {
            bool showHidden = NopContext.Current.IsAdmin;
            string key = string.Format(PRODUCTCATEGORIES_ALLBYPRODUCTID_KEY, showHidden, ProductID);
            object obj2 = NopCache.Get(key);
            if (CategoryManager.MappingsCacheEnabled && (obj2 != null))
            {
                return (ProductCategoryCollection)obj2;
            }

            DBProductCategoryCollection dbCollection = DBProviderManager<DBCategoryProvider>.Provider.GetProductCategoriesByProductID(ProductID, showHidden);
            ProductCategoryCollection productCategoryCollection = DBMapping(dbCollection);

            if (CategoryManager.MappingsCacheEnabled)
            {
                NopCache.Max(key, productCategoryCollection);
            }
            return productCategoryCollection;
        }

        /// <summary>
        /// Gets a product category mapping 
        /// </summary>
        /// <param name="ProductCategoryID">Product category mapping identifier</param>
        /// <returns>Product category mapping</returns>
        public static ProductCategory GetProductCategoryByID(int ProductCategoryID)
        {
            string key = string.Format(PRODUCTCATEGORIES_BY_ID_KEY, ProductCategoryID);
            object obj2 = NopCache.Get(key);
            if (CategoryManager.MappingsCacheEnabled && (obj2 != null))
            {
                return (ProductCategory)obj2;
            }

            DBProductCategory dbItem = DBProviderManager<DBCategoryProvider>.Provider.GetProductCategoryByID(ProductCategoryID);
            ProductCategory productCategory = DBMapping(dbItem);

            if (CategoryManager.MappingsCacheEnabled)
            {
                NopCache.Max(key, productCategory);
            }
            return productCategory;
        }

        /// <summary>
        /// Inserts a product category mapping
        /// </summary>
        /// <param name="ProductID">Product identifier</param>
        /// <param name="CategoryID">Category identifier</param>
        /// <param name="IsFeaturedProduct">A value indicating whether the product is featured</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Product category mapping </returns>
        public static ProductCategory InsertProductCategory(int ProductID, int CategoryID,
           bool IsFeaturedProduct, int DisplayOrder)
        {
            DBProductCategory dbItem = DBProviderManager<DBCategoryProvider>.Provider.InsertProductCategory(ProductID, CategoryID,
            IsFeaturedProduct, DisplayOrder);

            ProductCategory productCategory = DBMapping(dbItem);
            if (CategoryManager.CategoriesCacheEnabled || CategoryManager.MappingsCacheEnabled)
            {
                NopCache.RemoveByPattern(CATEGORIES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTCATEGORIES_PATTERN_KEY);
            }
            return productCategory;
        }

        /// <summary>
        /// Updates the product category mapping 
        /// </summary>
        /// <param name="ProductCategoryID">Product category mapping  identifier</param>
        /// <param name="ProductID">Product identifier</param>
        /// <param name="CategoryID">Category identifier</param>
        /// <param name="IsFeaturedProduct">A value indicating whether the product is featured</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Product category mapping </returns>
        public static ProductCategory UpdateProductCategory(int ProductCategoryID, int ProductID, int CategoryID,
           bool IsFeaturedProduct, int DisplayOrder)
        {
            DBProductCategory dbItem = DBProviderManager<DBCategoryProvider>.Provider.UpdateProductCategory(ProductCategoryID, ProductID, CategoryID,
            IsFeaturedProduct, DisplayOrder);
            ProductCategory productCategory = DBMapping(dbItem);

            if (CategoryManager.CategoriesCacheEnabled || CategoryManager.MappingsCacheEnabled)
            {
                NopCache.RemoveByPattern(CATEGORIES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTCATEGORIES_PATTERN_KEY);
            }
            return productCategory;
        }
        #endregion
        
        #region Property
        /// <summary>
        /// Gets a value indicating whether categories cache is enabled
        /// </summary>
        public static bool CategoriesCacheEnabled
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("Cache.CategoryManager.CategoriesCacheEnabled");
            }
        }

        /// <summary>
        /// Gets a value indicating whether mappings cache is enabled
        /// </summary>
        public static bool MappingsCacheEnabled
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("Cache.CategoryManager.MappingsCacheEnabled");
            }
        }
        #endregion
    }
}
