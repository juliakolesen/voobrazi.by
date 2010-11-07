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
using NopSolutions.NopCommerce.DataAccess.Manufacturers;

namespace NopSolutions.NopCommerce.BusinessLogic.Manufacturers
{
    /// <summary>
    /// Manufacturer manager
    /// </summary>
    public partial class ManufacturerManager
    {
        #region Constants
        private const string MANUFACTURERS_ALL_KEY = "Nop.manufacturer.all-{0}";
        private const string MANUFACTURERS_BY_ID_KEY = "Nop.manufacturer.id-{0}";
        private const string PRODUCTMANUFACTURERS_ALLBYMANUFACTURERID_KEY = "Nop.productmanufacturer.allbymanufacturerid-{0}-{1}";
        private const string PRODUCTMANUFACTURERS_ALLBYPRODUCTID_KEY = "Nop.productmanufacturer.allbyproductid-{0}-{1}";
        private const string PRODUCTMANUFACTURERS_BY_ID_KEY = "Nop.productmanufacturer.id-{0}";
        private const string MANUFACTURERS_PATTERN_KEY = "Nop.manufacturer.";
        private const string PRODUCTMANUFACTURERS_PATTERN_KEY = "Nop.productmanufacturer.";
        #endregion

        #region Utilities
        private static ManufacturerCollection DBMapping(DBManufacturerCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            ManufacturerCollection collection = new ManufacturerCollection();
            foreach (DBManufacturer dbItem in dbCollection)
            {
                Manufacturer item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static Manufacturer DBMapping(DBManufacturer dbItem)
        {
            if (dbItem == null)
                return null;

            Manufacturer item = new Manufacturer();
            item.ManufacturerID = dbItem.ManufacturerID;
            item.Name = dbItem.Name;
            item.Description = dbItem.Description;
            item.TemplateID = dbItem.TemplateID;
            item.MetaKeywords = dbItem.MetaKeywords;
            item.MetaDescription = dbItem.MetaDescription;
            item.MetaTitle = dbItem.MetaTitle;
            item.SEName = dbItem.SEName;
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

        private static ProductManufacturerCollection DBMapping(DBProductManufacturerCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            ProductManufacturerCollection collection = new ProductManufacturerCollection();
            foreach (DBProductManufacturer dbItem in dbCollection)
            {
                ProductManufacturer item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ProductManufacturer DBMapping(DBProductManufacturer dbItem)
        {
            if (dbItem == null)
                return null;

            ProductManufacturer item = new ProductManufacturer();
            item.ProductManufacturerID = dbItem.ProductManufacturerID;
            item.ProductID = dbItem.ProductID;
            item.ManufacturerID = dbItem.ManufacturerID;
            item.IsFeaturedProduct = dbItem.IsFeaturedProduct;
            item.DisplayOrder = dbItem.DisplayOrder;

            return item;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Marks a manufacturer as deleted
        /// </summary>
        /// <param name="ManufacturerID">Manufacturer identifer</param>
        public static void MarkManufacturerAsDeleted(int ManufacturerID)
        {
            Manufacturer manufacturer = GetManufacturerByID(ManufacturerID);
            if (manufacturer != null)
            {
                manufacturer = UpdateManufacturer(manufacturer.ManufacturerID, manufacturer.Name, manufacturer.Description,
                    manufacturer.TemplateID, manufacturer.MetaKeywords,
                    manufacturer.MetaDescription, manufacturer.MetaTitle,
                    manufacturer.SEName, manufacturer.PictureID, manufacturer.PageSize,
                    manufacturer.PriceRanges, manufacturer.Published,
                    true, manufacturer.DisplayOrder, manufacturer.CreatedOn, manufacturer.UpdatedOn);
            }
        }

        /// <summary>
        /// Removes a manufacturer picture
        /// </summary>
        /// <param name="ManufacturerID">Manufacturer identifier</param>
        public static void RemoveManufacturerPicture(int ManufacturerID)
        {
            Manufacturer manufacturer = GetManufacturerByID(ManufacturerID);
            if (manufacturer != null)
            {
                UpdateManufacturer(manufacturer.ManufacturerID, manufacturer.Name, manufacturer.Description,
                    manufacturer.TemplateID, manufacturer.MetaKeywords,
                    manufacturer.MetaDescription, manufacturer.MetaTitle,
                    manufacturer.SEName, 0, manufacturer.PageSize, manufacturer.PriceRanges,
                    manufacturer.Published, manufacturer.Deleted, manufacturer.DisplayOrder, 
                    manufacturer.CreatedOn, manufacturer.UpdatedOn);
            }
        }

        /// <summary>
        /// Gets all manufacturers
        /// </summary>
        /// <returns>Manufacturer collection</returns>
        public static ManufacturerCollection GetAllManufacturers()
        {
            bool showHidden = NopContext.Current.IsAdmin;
            return GetAllManufacturers(showHidden);
        }

        /// <summary>
        /// Gets all manufacturers
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Manufacturer collection</returns>
        public static ManufacturerCollection GetAllManufacturers(bool showHidden)
        {
            string key = string.Format(MANUFACTURERS_ALL_KEY, showHidden);
            object obj2 = NopCache.Get(key);
            if (ManufacturerManager.ManufacturersCacheEnabled && (obj2 != null))
            {
                return (ManufacturerCollection)obj2;
            }

            DBManufacturerCollection dbCollection = DBProviderManager<DBManufacturerProvider>.Provider.GetAllManufacturers(showHidden);
            ManufacturerCollection manufacturerCollection = DBMapping(dbCollection);

            if (ManufacturerManager.ManufacturersCacheEnabled)
            {
                NopCache.Max(key, manufacturerCollection);
            }
            return manufacturerCollection;
        }

        /// <summary>
        /// Gets a manufacturer
        /// </summary>
        /// <param name="ManufacturerID">Manufacturer identifier</param>
        /// <returns>Manufacturer</returns>
        public static Manufacturer GetManufacturerByID(int ManufacturerID)
        {
            if (ManufacturerID == 0)
                return null;

            string key = string.Format(MANUFACTURERS_BY_ID_KEY, ManufacturerID);
            object obj2 = NopCache.Get(key);
            if (ManufacturerManager.ManufacturersCacheEnabled && (obj2 != null))
            {
                return (Manufacturer)obj2;
            }

            DBManufacturer dbItem = DBProviderManager<DBManufacturerProvider>.Provider.GetManufacturerByID(ManufacturerID);
            Manufacturer manufacturer = DBMapping(dbItem);

            if (ManufacturerManager.ManufacturersCacheEnabled)
            {
                NopCache.Max(key, manufacturer);
            }
            return manufacturer;
        }

        /// <summary>
        /// Inserts a manufacturer
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="Description">The description</param>
        /// <param name="TemplateID">The template identifier</param>
        /// <param name="MetaKeywords">The meta keywords</param>
        /// <param name="MetaDescription">The meta description</param>
        /// <param name="MetaTitle">The meta title</param>
        /// <param name="SEName">The search-engine name</param>
        /// <param name="PictureID">The parent picture identifier</param>
        /// <param name="PageSize">The page size</param>
        /// <param name="PriceRanges">The price ranges</param>
        /// <param name="Published">A value indicating whether the entity is published</param>
        /// <param name="Deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Manufacturer</returns>
        public static Manufacturer InsertManufacturer(string Name, string Description,
            int TemplateID, string MetaKeywords, string MetaDescription, string MetaTitle,
            string SEName, int PictureID, int PageSize, string PriceRanges, bool Published, bool Deleted,
            int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            UpdatedOn = DateTimeHelper.ConvertToUtcTime(UpdatedOn);

            DBManufacturer dbItem = DBProviderManager<DBManufacturerProvider>.Provider.InsertManufacturer(Name, Description,
                TemplateID, MetaKeywords, MetaDescription, MetaTitle,
                SEName, PictureID, PageSize, PriceRanges, Published, Deleted,
                DisplayOrder, CreatedOn, UpdatedOn);
            Manufacturer manufacturer = DBMapping(dbItem);

            if (ManufacturerManager.ManufacturersCacheEnabled || ManufacturerManager.MappingsCacheEnabled)
            {
                NopCache.RemoveByPattern(MANUFACTURERS_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTMANUFACTURERS_PATTERN_KEY);
            }

            return manufacturer;
        }

        /// <summary>
        /// Updates the manufacturer
        /// </summary>
        /// <param name="ManufacturerID">Manufacturer identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="Description">The description</param>
        /// <param name="TemplateID">The template identifier</param>
        /// <param name="MetaKeywords">The meta keywords</param>
        /// <param name="MetaDescription">The meta description</param>
        /// <param name="MetaTitle">The meta title</param>
        /// <param name="SEName">The search-engine name</param>
        /// <param name="PictureID">The parent picture identifier</param>
        /// <param name="PageSize">The page size</param>
        /// <param name="PriceRanges">The price ranges</param>
        /// <param name="Published">A value indicating whether the entity is published</param>
        /// <param name="Deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Manufacturer</returns>
        public static Manufacturer UpdateManufacturer(int ManufacturerID, string Name, string Description,
            int TemplateID, string MetaKeywords, string MetaDescription, string MetaTitle,
            string SEName, int PictureID, int PageSize, string PriceRanges, bool Published, bool Deleted,
            int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            UpdatedOn = DateTimeHelper.ConvertToUtcTime(UpdatedOn);

            DBManufacturer dbItem = DBProviderManager<DBManufacturerProvider>.Provider.UpdateManufacturer(ManufacturerID, Name, Description,
                TemplateID, MetaKeywords, MetaDescription, MetaTitle,
                SEName, PictureID, PageSize, PriceRanges, Published, Deleted,
                DisplayOrder, CreatedOn, UpdatedOn);
            Manufacturer manufacturer = DBMapping(dbItem);

            if (ManufacturerManager.ManufacturersCacheEnabled || ManufacturerManager.MappingsCacheEnabled)
            {
                NopCache.RemoveByPattern(MANUFACTURERS_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTMANUFACTURERS_PATTERN_KEY);
            }

            return manufacturer;
        }

        /// <summary>
        /// Deletes a product manufacturer mapping
        /// </summary>
        /// <param name="ProductManufacturerID">Product manufacturer mapping identifer</param>
        public static void DeleteProductManufacturer(int ProductManufacturerID)
        {
            DBProviderManager<DBManufacturerProvider>.Provider.DeleteProductManufacturer(ProductManufacturerID);

            if (ManufacturerManager.ManufacturersCacheEnabled || ManufacturerManager.MappingsCacheEnabled)
            {
                NopCache.RemoveByPattern(MANUFACTURERS_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTMANUFACTURERS_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets product category manufacturer collection
        /// </summary>
        /// <param name="ManufacturerID">Manufacturer identifier</param>
        /// <returns>Product category manufacturer collection</returns>
        public static ProductManufacturerCollection GetProductManufacturersByManufacturerID(int ManufacturerID)
        {
            bool showHidden = NopContext.Current.IsAdmin;
            string key = string.Format(PRODUCTMANUFACTURERS_ALLBYMANUFACTURERID_KEY, showHidden, ManufacturerID);
            object obj2 = NopCache.Get(key);
            if (ManufacturerManager.MappingsCacheEnabled && (obj2 != null))
            {
                return (ProductManufacturerCollection)obj2;
            }

            DBProductManufacturerCollection dbCollection = DBProviderManager<DBManufacturerProvider>.Provider.GetProductManufacturersByManufacturerID(ManufacturerID, showHidden);
            ProductManufacturerCollection productManufacturerCollection = DBMapping(dbCollection);

            if (ManufacturerManager.MappingsCacheEnabled)
            {
                NopCache.Max(key, productManufacturerCollection);
            }
            return productManufacturerCollection;
        }

        /// <summary>
        /// Gets a product manufacturer mapping collection
        /// </summary>
        /// <param name="ProductID">Product identifier</param>
        /// <returns>Product manufacturer mapping collection</returns>
        public static ProductManufacturerCollection GetProductManufacturersByProductID(int ProductID)
        {
            bool showHidden = NopContext.Current.IsAdmin;
            string key = string.Format(PRODUCTMANUFACTURERS_ALLBYPRODUCTID_KEY, showHidden, ProductID);
            object obj2 = NopCache.Get(key);
            if (ManufacturerManager.MappingsCacheEnabled && (obj2 != null))
            {
                return (ProductManufacturerCollection)obj2;
            }

            DBProductManufacturerCollection dbCollection = DBProviderManager<DBManufacturerProvider>.Provider.GetProductManufacturersByProductID(ProductID, showHidden);
            ProductManufacturerCollection productManufacturerCollection = DBMapping(dbCollection);

            if (ManufacturerManager.MappingsCacheEnabled)
            {
                NopCache.Max(key, productManufacturerCollection);
            }
            return productManufacturerCollection;
        }

        /// <summary>
        /// Gets a product manufacturer mapping 
        /// </summary>
        /// <param name="ProductManufacturerID">Product manufacturer mapping identifier</param>
        /// <returns>Product manufacturer mapping</returns>
        public static ProductManufacturer GetProductManufacturerByID(int ProductManufacturerID)
        {
            if (ProductManufacturerID == 0)
                return null;

            string key = string.Format(PRODUCTMANUFACTURERS_BY_ID_KEY, ProductManufacturerID);
            object obj2 = NopCache.Get(key);
            if (ManufacturerManager.MappingsCacheEnabled && (obj2 != null))
            {
                return (ProductManufacturer)obj2;
            }

            DBProductManufacturer dbItem = DBProviderManager<DBManufacturerProvider>.Provider.GetProductManufacturerByID(ProductManufacturerID);
            ProductManufacturer productManufacturer = DBMapping(dbItem);

            if (ManufacturerManager.MappingsCacheEnabled)
            {
                NopCache.Max(key, productManufacturer);
            }
            return productManufacturer;
        }

        /// <summary>
        /// Inserts a product manufacturer mapping
        /// </summary>
        /// <param name="ProductID">Product identifier</param>
        /// <param name="ManufacturerID">Manufacturer identifier</param>
        /// <param name="IsFeaturedProduct">A value indicating whether the product is featured</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Product manufacturer mapping </returns>
        public static ProductManufacturer InsertProductManufacturer(int ProductID, int ManufacturerID,
           bool IsFeaturedProduct, int DisplayOrder)
        {
            DBProductManufacturer dbItem = DBProviderManager<DBManufacturerProvider>.Provider.InsertProductManufacturer(ProductID,
                ManufacturerID, IsFeaturedProduct, DisplayOrder);
            ProductManufacturer productManufacturer = DBMapping(dbItem);

            if (ManufacturerManager.ManufacturersCacheEnabled || ManufacturerManager.MappingsCacheEnabled)
            {
                NopCache.RemoveByPattern(MANUFACTURERS_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTMANUFACTURERS_PATTERN_KEY);
            }

            return productManufacturer;
        }

        /// <summary>
        /// Updates the product manufacturer mapping
        /// </summary>
        /// <param name="ProductManufacturerID">Product manufacturer mapping identifier</param>
        /// <param name="ProductID">Product identifier</param>
        /// <param name="ManufacturerID">Manufacturer identifier</param>
        /// <param name="IsFeaturedProduct">A value indicating whether the product is featured</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Product manufacturer mapping </returns>
        public static ProductManufacturer UpdateProductManufacturer(int ProductManufacturerID, int ProductID, int ManufacturerID,
           bool IsFeaturedProduct, int DisplayOrder)
        {
            DBProductManufacturer dbItem = DBProviderManager<DBManufacturerProvider>.Provider.UpdateProductManufacturer(ProductManufacturerID,
                ProductID, ManufacturerID, IsFeaturedProduct, DisplayOrder);
            ProductManufacturer productManufacturer = DBMapping(dbItem);

            if (ManufacturerManager.ManufacturersCacheEnabled || ManufacturerManager.MappingsCacheEnabled)
            {
                NopCache.RemoveByPattern(MANUFACTURERS_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTMANUFACTURERS_PATTERN_KEY);
            }

            return productManufacturer;
        }
        #endregion

        #region Property

        /// <summary>
        /// Gets a value indicating whether manufacturers cache is enabled
        /// </summary>
        public static bool ManufacturersCacheEnabled
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("Cache.ManufacturerManager.ManufacturersCacheEnabled");
            }
        }

        /// <summary>
        /// Gets a value indicating whether mappings cache is enabled
        /// </summary>
        public static bool MappingsCacheEnabled
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("Cache.ManufacturerManager.MappingsCacheEnabled");
            }
        }
        #endregion
    }
}
