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
using NopSolutions.NopCommerce.BusinessLogic.Caching;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Templates;

namespace NopSolutions.NopCommerce.BusinessLogic.Templates
{
    /// <summary>
    /// Category template manager
    /// </summary>
    public partial class TemplateManager
    {
        #region Constants
        private const string CATEGORYTEMPLATES_ALL_KEY = "Nop.categorytemplate.all";
        private const string CATEGORYTEMPLATES_BY_ID_KEY = "Nop.categorytemplate.id-{0}";
        private const string MANUFACTURERTEMPLATES_ALL_KEY = "Nop.manufacturertemplate.all";
        private const string MANUFACTURERTEMPLATES_BY_ID_KEY = "Nop.manufacturertemplate.id-{0}";
        private const string PRODUCTTEMPLATES_ALL_KEY = "Nop.producttemplate.all";
        private const string PRODUCTTEMPLATES_BY_ID_KEY = "Nop.producttemplate.id-{0}";
        private const string CATEGORYTEMPLATES_PATTERN_KEY = "Nop.categorytemplate.";
        private const string MANUFACTURERTEMPLATES_PATTERN_KEY = "Nop.manufacturertemplate.";
        private const string PRODUCTTEMPLATES_PATTERN_KEY = "Nop.producttemplate.";
        #endregion

        #region Utilities
        private static CategoryTemplateCollection DBMapping(DBCategoryTemplateCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            CategoryTemplateCollection collection = new CategoryTemplateCollection();
            foreach (DBCategoryTemplate dbItem in dbCollection)
            {
                CategoryTemplate item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static CategoryTemplate DBMapping(DBCategoryTemplate dbItem)
        {
            if (dbItem == null)
                return null;

            CategoryTemplate item = new CategoryTemplate();
            item.CategoryTemplateID = dbItem.CategoryTemplateID;
            item.Name = dbItem.Name;
            item.TemplatePath = dbItem.TemplatePath;
            item.DisplayOrder = dbItem.DisplayOrder;
            item.CreatedOn = dbItem.CreatedOn;
            item.UpdatedOn = dbItem.UpdatedOn;

            return item;
        }
        
        private static ManufacturerTemplateCollection DBMapping(DBManufacturerTemplateCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            ManufacturerTemplateCollection collection = new ManufacturerTemplateCollection();
            foreach (DBManufacturerTemplate dbItem in dbCollection)
            {
                ManufacturerTemplate item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ManufacturerTemplate DBMapping(DBManufacturerTemplate dbItem)
        {
            if (dbItem == null)
                return null;

            ManufacturerTemplate item = new ManufacturerTemplate();
            item.ManufacturerTemplateID = dbItem.ManufacturerTemplateID;
            item.Name = dbItem.Name;
            item.TemplatePath = dbItem.TemplatePath;
            item.DisplayOrder = dbItem.DisplayOrder;
            item.CreatedOn = dbItem.CreatedOn;
            item.UpdatedOn = dbItem.UpdatedOn;

            return item;
        }
        
        private static ProductTemplateCollection DBMapping(DBProductTemplateCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            ProductTemplateCollection collection = new ProductTemplateCollection();
            foreach (DBProductTemplate dbItem in dbCollection)
            {
                ProductTemplate item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ProductTemplate DBMapping(DBProductTemplate dbItem)
        {
            if (dbItem == null)
                return null;

            ProductTemplate item = new ProductTemplate();
            item.ProductTemplateID = dbItem.ProductTemplateID;
            item.Name = dbItem.Name;
            item.TemplatePath = dbItem.TemplatePath;
            item.DisplayOrder = dbItem.DisplayOrder;
            item.CreatedOn = dbItem.CreatedOn;
            item.UpdatedOn = dbItem.UpdatedOn;

            return item;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Deletes a category template
        /// </summary>
        /// <param name="CategoryTemplateID">Category template identifier</param>
        public static void DeleteCategoryTemplate(int CategoryTemplateID)
        {
            DBProviderManager<DBTemplateProvider>.Provider.DeleteCategoryTemplate(CategoryTemplateID);

            if (TemplateManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(CATEGORYTEMPLATES_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets all category templates
        /// </summary>
        /// <returns>Category template collection</returns>
        public static CategoryTemplateCollection GetAllCategoryTemplates()
        {
            string key = string.Format(CATEGORYTEMPLATES_ALL_KEY);
            object obj2 = NopCache.Get(key);
            if (TemplateManager.CacheEnabled && (obj2 != null))
            {
                return (CategoryTemplateCollection)obj2;
            }

            DBCategoryTemplateCollection dbCollection = DBProviderManager<DBTemplateProvider>.Provider.GetAllCategoryTemplates();
            CategoryTemplateCollection categoryTemplates = DBMapping(dbCollection);

            if (TemplateManager.CacheEnabled)
            {
                NopCache.Max(key, categoryTemplates);
            }
            return categoryTemplates;
        }

        /// <summary>
        /// Gets a category template
        /// </summary>
        /// <param name="CategoryTemplateID">Category template identifier</param>
        /// <returns>A category template</returns>
        public static CategoryTemplate GetCategoryTemplateByID(int CategoryTemplateID)
        {
            if (CategoryTemplateID == 0)
                return null;

            string key = string.Format(CATEGORYTEMPLATES_BY_ID_KEY, CategoryTemplateID);
            object obj2 = NopCache.Get(key);
            if (TemplateManager.CacheEnabled && (obj2 != null))
            {
                return (CategoryTemplate)obj2;
            }

            DBCategoryTemplate dbItem = DBProviderManager<DBTemplateProvider>.Provider.GetCategoryTemplateByID(CategoryTemplateID);
            CategoryTemplate categoryTemplate = DBMapping(dbItem);

            if (TemplateManager.CacheEnabled)
            {
                NopCache.Max(key, categoryTemplate);
            }
            return categoryTemplate;
        }

        /// <summary>
        /// Inserts a category template
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="TemplatePath">The template path</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>A category template</returns>
        public static CategoryTemplate InsertCategoryTemplate(string Name, string TemplatePath,
            int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            UpdatedOn = DateTimeHelper.ConvertToUtcTime(UpdatedOn);

            DBCategoryTemplate dbItem = DBProviderManager<DBTemplateProvider>.Provider.InsertCategoryTemplate(Name, TemplatePath,
            DisplayOrder, CreatedOn, UpdatedOn);
            CategoryTemplate categoryTemplate = DBMapping(dbItem);
            if (TemplateManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(CATEGORYTEMPLATES_PATTERN_KEY);
            }

            return categoryTemplate;
        }

        /// <summary>
        /// Updates the category template
        /// </summary>
        /// <param name="CategoryTemplateID">Category template identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="TemplatePath">The template path</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>A category template</returns>
        public static CategoryTemplate UpdateCategoryTemplate(int CategoryTemplateID, string Name, string TemplatePath,
            int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            UpdatedOn = DateTimeHelper.ConvertToUtcTime(UpdatedOn);

            DBCategoryTemplate dbItem = DBProviderManager<DBTemplateProvider>.Provider.UpdateCategoryTemplate(CategoryTemplateID, Name, TemplatePath,
            DisplayOrder, CreatedOn, UpdatedOn);
            CategoryTemplate categoryTemplate = DBMapping(dbItem);
            if (TemplateManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(CATEGORYTEMPLATES_PATTERN_KEY);
            }

            return categoryTemplate;
        }
        
        /// <summary>
        /// Deletes a manufacturer template
        /// </summary>
        /// <param name="ManufacturerTemplateID">Manufacturer template identifier</param>
        public static void DeleteManufacturerTemplate(int ManufacturerTemplateID)
        {
            DBProviderManager<DBTemplateProvider>.Provider.DeleteManufacturerTemplate(ManufacturerTemplateID);

            if (TemplateManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(MANUFACTURERTEMPLATES_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets all manufacturer templates
        /// </summary>
        /// <returns>Manufacturer template collection</returns>
        public static ManufacturerTemplateCollection GetAllManufacturerTemplates()
        {
            string key = string.Format(MANUFACTURERTEMPLATES_ALL_KEY);
            object obj2 = NopCache.Get(key);
            if (TemplateManager.CacheEnabled && (obj2 != null))
            {
                return (ManufacturerTemplateCollection)obj2;
            }

            DBManufacturerTemplateCollection dbCollection = DBProviderManager<DBTemplateProvider>.Provider.GetAllManufacturerTemplates();
            ManufacturerTemplateCollection manufacturerTemplates = DBMapping(dbCollection);

            if (TemplateManager.CacheEnabled)
            {
                NopCache.Max(key, manufacturerTemplates);
            }
            return manufacturerTemplates;
        }

        /// <summary>
        /// Gets a manufacturer template
        /// </summary>
        /// <param name="ManufacturerTemplateID">Manufacturer template identifier</param>
        /// <returns>Manufacturer template</returns>
        public static ManufacturerTemplate GetManufacturerTemplateByID(int ManufacturerTemplateID)
        {
            if (ManufacturerTemplateID == 0)
                return null;

            string key = string.Format(MANUFACTURERTEMPLATES_BY_ID_KEY, ManufacturerTemplateID);
            object obj2 = NopCache.Get(key);
            if (TemplateManager.CacheEnabled && (obj2 != null))
            {
                return (ManufacturerTemplate)obj2;
            }

            DBManufacturerTemplate dbItem = DBProviderManager<DBTemplateProvider>.Provider.GetManufacturerTemplateByID(ManufacturerTemplateID);
            ManufacturerTemplate manufacturerTemplate = DBMapping(dbItem);

            if (TemplateManager.CacheEnabled)
            {
                NopCache.Max(key, manufacturerTemplate);
            }
            return manufacturerTemplate;
        }

        /// <summary>
        /// Inserts a manufacturer template
        /// </summary>
        /// <param name="Name">The manufacturer template identifier</param>
        /// <param name="TemplatePath">The template path</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Manufacturer template</returns>
        public static ManufacturerTemplate InsertManufacturerTemplate(string Name, string TemplatePath,
            int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            UpdatedOn = DateTimeHelper.ConvertToUtcTime(UpdatedOn);

            DBManufacturerTemplate dbItem = DBProviderManager<DBTemplateProvider>.Provider.InsertManufacturerTemplate(Name,
                TemplatePath, DisplayOrder, CreatedOn, UpdatedOn);
            ManufacturerTemplate manufacturerTemplate = DBMapping(dbItem);
            if (TemplateManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(MANUFACTURERTEMPLATES_PATTERN_KEY);
            }
            return manufacturerTemplate;
        }

        /// <summary>
        /// Updates the manufacturer template
        /// </summary>
        /// <param name="ManufacturerTemplateID">Manufacturer template identifer</param>
        /// <param name="Name">The manufacturer template identifier</param>
        /// <param name="TemplatePath">The template path</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Manufacturer template</returns>
        public static ManufacturerTemplate UpdateManufacturerTemplate(int ManufacturerTemplateID, string Name, string TemplatePath,
            int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            UpdatedOn = DateTimeHelper.ConvertToUtcTime(UpdatedOn);

            DBManufacturerTemplate dbItem = DBProviderManager<DBTemplateProvider>.Provider.UpdateManufacturerTemplate(ManufacturerTemplateID,
                Name, TemplatePath, DisplayOrder, CreatedOn, UpdatedOn);
            ManufacturerTemplate manufacturerTemplate = DBMapping(dbItem);
            if (TemplateManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(MANUFACTURERTEMPLATES_PATTERN_KEY);
            }
            return manufacturerTemplate;
        }
        
        /// <summary>
        /// Deletes a product template
        /// </summary>
        /// <param name="ProductTemplateID">Product template identifier</param>
        public static void DeleteProductTemplate(int ProductTemplateID)
        {
            DBProviderManager<DBTemplateProvider>.Provider.DeleteProductTemplate(ProductTemplateID);

            if (TemplateManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTTEMPLATES_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets all product templates
        /// </summary>
        /// <returns>Product template collection</returns>
        public static ProductTemplateCollection GetAllProductTemplates()
        {
            string key = string.Format(PRODUCTTEMPLATES_ALL_KEY);
            object obj2 = NopCache.Get(key);
            if (TemplateManager.CacheEnabled && (obj2 != null))
            {
                return (ProductTemplateCollection)obj2;
            }

            DBProductTemplateCollection dbCollection = DBProviderManager<DBTemplateProvider>.Provider.GetAllProductTemplates();
            ProductTemplateCollection productTemplates = DBMapping(dbCollection);

            if (TemplateManager.CacheEnabled)
            {
                NopCache.Max(key, productTemplates);
            }
            return productTemplates;
        }

        /// <summary>
        /// Gets a product template
        /// </summary>
        /// <param name="ProductTemplateID">Product template identifier</param>
        /// <returns>Product template</returns>
        public static ProductTemplate GetProductTemplateByID(int ProductTemplateID)
        {
            if (ProductTemplateID == 0)
                return null;

            string key = string.Format(PRODUCTTEMPLATES_BY_ID_KEY, ProductTemplateID);
            object obj2 = NopCache.Get(key);
            if (TemplateManager.CacheEnabled && (obj2 != null))
            {
                return (ProductTemplate)obj2;
            }

            DBProductTemplate dbItem = DBProviderManager<DBTemplateProvider>.Provider.GetProductTemplateByID(ProductTemplateID);
            ProductTemplate productTemplate = DBMapping(dbItem);

            if (TemplateManager.CacheEnabled)
            {
                NopCache.Max(key, productTemplate);
            }
            return productTemplate;
        }

        /// <summary>
        /// Inserts a product template
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="TemplatePath">The template path</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Product template</returns>
        public static ProductTemplate InsertProductTemplate(string Name, string TemplatePath,
            int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            UpdatedOn = DateTimeHelper.ConvertToUtcTime(UpdatedOn);

            DBProductTemplate dbItem = DBProviderManager<DBTemplateProvider>.Provider.InsertProductTemplate(Name, TemplatePath,
                DisplayOrder, CreatedOn, UpdatedOn);
            ProductTemplate productTemplate = DBMapping(dbItem);

            if (TemplateManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTTEMPLATES_PATTERN_KEY);
            }

            return productTemplate;
        }

        /// <summary>
        /// Updates the product template
        /// </summary>
        /// <param name="ProductTemplateID">The product template identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="TemplatePath">The template path</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Product template</returns>
        public static ProductTemplate UpdateProductTemplate(int ProductTemplateID, string Name, string TemplatePath,
            int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            UpdatedOn = DateTimeHelper.ConvertToUtcTime(UpdatedOn);

            DBProductTemplate dbItem = DBProviderManager<DBTemplateProvider>.Provider.UpdateProductTemplate(ProductTemplateID,
                Name, TemplatePath, DisplayOrder, CreatedOn, UpdatedOn);
            ProductTemplate productTemplate = DBMapping(dbItem);

            if (TemplateManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTTEMPLATES_PATTERN_KEY);
            }

            return productTemplate;
        }
        #endregion

        #region Property
        /// <summary>
        /// Gets a value indicating whether cache is enabled
        /// </summary>
        public static bool CacheEnabled
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("Cache.TemplateManager.CacheEnabled");
            }
        }
        #endregion
    }
}