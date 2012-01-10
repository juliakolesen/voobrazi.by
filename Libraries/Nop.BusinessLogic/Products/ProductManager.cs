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
using System.Web;
using NopSolutions.NopCommerce.BusinessLogic.Caching;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.Localization;
using NopSolutions.NopCommerce.BusinessLogic.Messages;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.Common.Utils.Html;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Products;

namespace NopSolutions.NopCommerce.BusinessLogic.Products
{
    /// <summary>
    /// Product manager
    /// </summary>
    public partial class ProductManager
    {
        #region Constants
        private const string PRODUCTS_BY_ID_KEY = "Nop.product.id-{0}";
        private const string PRODUCTVARIANTS_ALL_KEY = "Nop.productvariant.all-{0}-{1}";
        private const string PRODUCTVARIANTS_BY_ID_KEY = "Nop.productvariant.id-{0}";
        private const string TIERPRICES_ALLBYPRODUCTVARIANTID_KEY = "Nop.tierprice.allbyproductvariantid-{0}";
        private const string PRODUCTS_PATTERN_KEY = "Nop.product.";
        private const string PRODUCTVARIANTS_PATTERN_KEY = "Nop.productvariant.";
        private const string TIERPRICES_PATTERN_KEY = "Nop.tierprice.";
        #endregion

        #region Utilities
        private static ProductCollection DBMapping(DBProductCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            ProductCollection collection = new ProductCollection();
            foreach (DBProduct dbItem in dbCollection)
            {
                Product item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static Product DBMapping(DBProduct dbItem)
        {
            if (dbItem == null)
                return null;

            Product item = new Product();
            item.ProductID = dbItem.ProductID;
            item.Name = dbItem.Name;
            item.ShortDescription = dbItem.ShortDescription;
            item.FullDescription = dbItem.FullDescription;
            item.AdminComment = dbItem.AdminComment;
            item.ProductTypeID = dbItem.ProductTypeID;
            item.TemplateID = dbItem.TemplateID;
            item.ShowOnHomePage = dbItem.ShowOnHomePage;
            item.MetaKeywords = dbItem.MetaKeywords;
            item.MetaDescription = dbItem.MetaDescription;
            item.MetaTitle = dbItem.MetaTitle;
            item.SEName = dbItem.SEName;
            item.AllowCustomerReviews = dbItem.AllowCustomerReviews;
            item.AllowCustomerRatings = dbItem.AllowCustomerRatings;
            item.RatingSum = dbItem.RatingSum;
            item.TotalRatingVotes = dbItem.TotalRatingVotes;
            item.Published = dbItem.Published;
            item.Deleted = dbItem.Deleted;
            item.CreatedOn = dbItem.CreatedOn;
            item.UpdatedOn = dbItem.UpdatedOn;

            return item;
        }

        private static ProductPictureCollection DBMapping(DBProductPictureCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            ProductPictureCollection collection = new ProductPictureCollection();
            foreach (DBProductPicture dbItem in dbCollection)
            {
                ProductPicture item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ProductPicture DBMapping(DBProductPicture dbItem)
        {
            if (dbItem == null)
                return null;

            ProductPicture item = new ProductPicture();
            item.ProductPictureID = dbItem.ProductPictureID;
            item.ProductID = dbItem.ProductID;
            item.PictureID = dbItem.PictureID;
            item.DisplayOrder = dbItem.DisplayOrder;

            return item;
        }

        private static ProductReviewCollection DBMapping(DBProductReviewCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            ProductReviewCollection collection = new ProductReviewCollection();
            foreach (DBProductReview dbItem in dbCollection)
            {
                ProductReview item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ProductReview DBMapping(DBProductReview dbItem)
        {
            if (dbItem == null)
                return null;

            ProductReview item = new ProductReview();
            item.ProductReviewID = dbItem.ProductReviewID;
            item.ProductID = dbItem.ProductID;
            item.CustomerID = dbItem.CustomerID;
            item.Title = dbItem.Title;
            item.ReviewText = dbItem.ReviewText;
            item.Rating = dbItem.Rating;
            item.HelpfulYesTotal = dbItem.HelpfulYesTotal;
            item.HelpfulNoTotal = dbItem.HelpfulNoTotal;
            item.IsApproved = dbItem.IsApproved;
            item.CreatedOn = dbItem.CreatedOn;

            return item;
        }

        private static ProductTypeCollection DBMapping(DBProductTypeCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            ProductTypeCollection collection = new ProductTypeCollection();
            foreach (DBProductType dbItem in dbCollection)
            {
                ProductType item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ProductType DBMapping(DBProductType dbItem)
        {
            if (dbItem == null)
                return null;

            ProductType item = new ProductType();
            item.ProductTypeID = dbItem.ProductTypeID;
            item.Name = dbItem.Name;
            item.DisplayOrder = dbItem.DisplayOrder;
            item.CreatedOn = dbItem.CreatedOn;
            item.UpdatedOn = dbItem.UpdatedOn;

            return item;
        }

        private static ProductVariantCollection DBMapping(DBProductVariantCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            ProductVariantCollection collection = new ProductVariantCollection();
            foreach (DBProductVariant dbItem in dbCollection)
            {
                ProductVariant item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ProductVariant DBMapping(DBProductVariant dbItem)
        {
            if (dbItem == null)
                return null;

            ProductVariant item = new ProductVariant();
            item.ProductVariantID = dbItem.ProductVariantID;
            item.ProductID = dbItem.ProductID;
            item.Name = dbItem.Name;
            item.SKU = dbItem.SKU;
            item.Description = dbItem.Description;
            item.AdminComment = dbItem.AdminComment;
            item.ManufacturerPartNumber = dbItem.ManufacturerPartNumber;
            item.IsDownload = dbItem.IsDownload;
            item.DownloadID = dbItem.DownloadID;
            item.UnlimitedDownloads = dbItem.UnlimitedDownloads;
            item.MaxNumberOfDownloads = dbItem.MaxNumberOfDownloads;
            item.HasSampleDownload = dbItem.HasSampleDownload;
            item.SampleDownloadID = dbItem.SampleDownloadID;
            item.IsShipEnabled = dbItem.IsShipEnabled;
            item.IsFreeShipping = dbItem.IsFreeShipping;
            item.AdditionalShippingCharge = dbItem.AdditionalShippingCharge;
            item.IsTaxExempt = dbItem.IsTaxExempt;
            item.TaxCategoryID = dbItem.TaxCategoryID;
            item.ManageInventory = dbItem.ManageInventory;
            item.StockQuantity = dbItem.StockQuantity;
            item.MinStockQuantity = dbItem.MinStockQuantity;
            item.LowStockActivityID = dbItem.LowStockActivityID;
            item.NotifyAdminForQuantityBelow = dbItem.NotifyAdminForQuantityBelow;
            item.OrderMinimumQuantity = dbItem.OrderMinimumQuantity;
            item.OrderMaximumQuantity = dbItem.OrderMaximumQuantity;
            item.WarehouseId = dbItem.WarehouseId;
            item.DisableBuyButton = dbItem.DisableBuyButton;
            item.Price = dbItem.Price;
            item.OldPrice = dbItem.OldPrice;
            item.Weight = dbItem.Weight;
            item.Length = dbItem.Length;
            item.Width = dbItem.Width;
            item.Height = dbItem.Height;
            item.PictureID = dbItem.PictureID;
            item.AvailableStartDateTime = dbItem.AvailableStartDateTime;
            item.AvailableEndDateTime = dbItem.AvailableEndDateTime;
            item.Published = dbItem.Published;
            item.Deleted = dbItem.Deleted;
            item.DisplayOrder = dbItem.DisplayOrder;
            item.CreatedOn = dbItem.CreatedOn;
            item.UpdatedOn = dbItem.UpdatedOn;
            return item;
        }

        private static RelatedProductCollection DBMapping(DBRelatedProductCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            RelatedProductCollection collection = new RelatedProductCollection();
            foreach (DBRelatedProduct dbItem in dbCollection)
            {
                RelatedProduct item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static RelatedProduct DBMapping(DBRelatedProduct dbItem)
        {
            if (dbItem == null)
                return null;

            RelatedProduct item = new RelatedProduct();
            item.RelatedProductID = dbItem.RelatedProductID;
            item.ProductID1 = dbItem.ProductID1;
            item.ProductID2 = dbItem.ProductID2;
            item.DisplayOrder = dbItem.DisplayOrder;

            return item;
        }

        private static PricelistCollection DBMapping(DBPricelistCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            PricelistCollection collection = new PricelistCollection();
            foreach (DBPricelist dbItem in dbCollection)
            {
                Pricelist item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static Pricelist DBMapping(DBPricelist dbItem)
        {
            if (dbItem == null)
                return null;

            Pricelist item = new Pricelist();
            item.PricelistID = dbItem.PricelistID;
            item.ExportModeID = dbItem.ExportModeID;
            item.ExportTypeID = dbItem.ExportTypeID;
            item.AffiliateID = dbItem.AffiliateID;
            item.DisplayName = dbItem.DisplayName;
            item.ShortName = dbItem.ShortName;
            item.PricelistGuid = dbItem.PricelistGuid;
            item.CacheTime = dbItem.CacheTime;
            item.FormatLocalization = dbItem.FormatLocalization;
            item.Description = dbItem.Description;
            item.AdminNotes = dbItem.AdminNotes;
            item.Header = dbItem.Header;
            item.Body = dbItem.Body;
            item.Footer = dbItem.Footer;
            item.PriceAdjustment = dbItem.PriceAdjustment;
            item.PriceAdjustmentTypeID = dbItem.PriceAdjustmentTypeID;
            item.OverrideIndivAdjustment = dbItem.OverrideIndivAdjustment;
            item.CreatedOn = dbItem.CreatedOn;
            item.UpdatedOn = dbItem.UpdatedOn;

            return item;
        }

        private static ProductVariantPricelistCollection DBMapping(DBProductVariantPricelistCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            ProductVariantPricelistCollection collection = new ProductVariantPricelistCollection();
            foreach (DBProductVariantPricelist dbItem in dbCollection)
            {
                ProductVariantPricelist item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ProductVariantPricelist DBMapping(DBProductVariantPricelist dbItem)
        {
            if (dbItem == null)
                return null;

            ProductVariantPricelist item = new ProductVariantPricelist();
            item.ProductVariantPricelistID = dbItem.ProductVariantPricelistID;
            item.ProductVariantID = dbItem.ProductVariantID;
            item.PricelistID = dbItem.PricelistID;
            item.PriceAdjustmentTypeID = dbItem.PriceAdjustmentTypeID;
            item.PriceAdjustment = dbItem.PriceAdjustment;
            item.UpdatedOn = dbItem.UpdatedOn;

            return item;
        }

        private static TierPriceCollection DBMapping(DBTierPriceCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            TierPriceCollection collection = new TierPriceCollection();
            foreach (DBTierPrice dbItem in dbCollection)
            {
                TierPrice item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static TierPrice DBMapping(DBTierPrice dbItem)
        {
            if (dbItem == null)
                return null;

            TierPrice item = new TierPrice();
            item.TierPriceID = dbItem.TierPriceID;
            item.ProductVariantID = dbItem.ProductVariantID;
            item.Quantity = dbItem.Quantity;
            item.Price = dbItem.Price;

            return item;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Marks a product as deleted
        /// </summary>
        /// <param name="ProductID">Product identifier</param>
        public static void MarkProductAsDeleted(int ProductID)
        {
            Product product = GetProductByID(ProductID);
            if (product != null)
            {
                product = UpdateProduct(product.ProductID, product.Name, product.ShortDescription,
                    product.FullDescription, product.AdminComment, product.ProductTypeID,
                    product.TemplateID, product.ShowOnHomePage, product.MetaKeywords, product.MetaDescription,
                    product.MetaTitle, product.SEName, product.AllowCustomerReviews, product.AllowCustomerRatings, product.RatingSum,
                    product.TotalRatingVotes, product.Published, true, product.CreatedOn, product.UpdatedOn);

                foreach (ProductVariant productVariant in product.ProductVariants)
                    MarkProductVariantAsDeleted(productVariant.ProductVariantID);
            }
        }

        /// <summary>
        /// Gets all products
        /// </summary>
        /// <returns>Product collection</returns>
        public static ProductCollection GetAllProducts()
        {
            bool showHidden = NopContext.Current.IsAdmin;
            return GetAllProducts(showHidden);
        }

        /// <summary>
        /// Gets all products
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product collection</returns>
        public static ProductCollection GetAllProducts(bool showHidden)
        {
            DBProductCollection dbCollection = DBProviderManager<DBProductProvider>.Provider.GetAllProducts(showHidden);
            ProductCollection products = DBMapping(dbCollection);
            return products;
        }

        /// <summary>
        /// Gets all products
        /// </summary>
        /// <param name="PageSize">Page size</param>
        /// <param name="PageIndex">Page index</param>
        /// <param name="TotalRecords">Total records</param>
        /// <returns>Product collection</returns>
        public static ProductCollection GetAllProducts(int PageSize, int PageIndex, out int TotalRecords)
        {
            return GetAllProducts(0, 0, null, null, null,
                string.Empty, false, PageSize, PageIndex, null, 0, true, out TotalRecords);
        }

        /// <summary>
        /// Gets all products
        /// </summary>
        /// <param name="CategoryID">Category identifier</param>
        /// <param name="ManufacturerID">Manufacturer identifier</param>
        /// <param name="FeaturedProducts">A value indicating whether loaded products are marked as featured (relates only to categories and manufacturers). 0 to load featured products only, 1 to load not featured products only, null to load all products</param>
        /// <param name="PageSize">Page size</param>
        /// <param name="PageIndex">Page index</param>
        /// <param name="TotalRecords">Total records</param>
        /// <returns>Product collection</returns>
        public static ProductCollection GetAllProducts(int CategoryID, int ManufacturerID,
            bool? FeaturedProducts, int PageSize, int PageIndex, out int TotalRecords)
        {
            return GetAllProducts(CategoryID, ManufacturerID, FeaturedProducts, null, null,
                string.Empty, false, PageSize, PageIndex, null, 0, true, out TotalRecords);
        }

        /// <summary>
        /// Gets all products
        /// </summary>
        /// <param name="Keywords">Keywords</param>
        /// <param name="SearchDescriptions">A value indicating whether to search in descriptions</param>
        /// <param name="PageSize">Page size</param>
        /// <param name="PageIndex">Page index</param>
        /// <param name="TotalRecords">Total records</param>
        /// <returns>Product collection</returns>
        public static ProductCollection GetAllProducts(string Keywords, bool SearchDescriptions,
            int PageSize, int PageIndex, out int TotalRecords)
        {
            return GetAllProducts(0, 0, null, null, null,
                Keywords, SearchDescriptions, PageSize, PageIndex, null, 0, true,  out TotalRecords);
        }

        /// <summary>
        /// Gets all products
        /// </summary>
        /// <param name="CategoryID">Category identifier</param>
        /// <param name="ManufacturerID">Manufacturer identifier</param>
        /// <param name="FeaturedProducts">A value indicating whether loaded products are marked as featured (relates only to categories and manufacturers). 0 to load featured products only, 1 to load not featured products only, null to load all products</param>
        /// <param name="Keywords">Keywords</param>
        /// <param name="SearchDescriptions">A value indicating whether to search in descriptions</param>
        /// <param name="PageSize">Page size</param>
        /// <param name="PageIndex">Page index</param>
        /// <param name="FilteredSpecs">Filtered product specification identifiers</param>
        /// <param name="TotalRecords">Total records</param>
        /// <returns>Product collection</returns>
        public static ProductCollection GetAllProducts(int CategoryID, int ManufacturerID,
            bool? FeaturedProducts, string Keywords, bool SearchDescriptions,
            int PageSize, int PageIndex, List<int> FilteredSpecs, out int TotalRecords)
        {
            return GetAllProducts(CategoryID, ManufacturerID, FeaturedProducts, null, null,
                Keywords, SearchDescriptions, PageSize, PageIndex, FilteredSpecs, 0, true, out TotalRecords);
        }

        /// <summary>
        /// Gets all products
        /// </summary>
        /// <param name="CategoryID">Category identifier</param>
        /// <param name="ManufacturerID">Manufacturer identifier</param>
        /// <param name="FeaturedProducts">A value indicating whether loaded products are marked as featured (relates only to categories and manufacturers). 0 to load featured products only, 1 to load not featured products only, null to load all products</param>
        /// <param name="PriceMin">Minimum price</param>
        /// <param name="PriceMax">Maximum price</param>
        /// <param name="PageSize">Page size</param>
        /// <param name="PageIndex">Page index</param>
        /// <param name="FilteredSpecs">Filtered product specification identifiers</param>
        /// <param name="TotalRecords">Total records</param>
        /// <returns>Product collection</returns>
        public static ProductCollection GetAllProducts(int CategoryID, int ManufacturerID,
            bool? FeaturedProducts, decimal? PriceMin, decimal? PriceMax,
            int PageSize, int PageIndex, List<int> FilteredSpecs, out int TotalRecords)
        {
            return GetAllProducts(CategoryID, ManufacturerID, FeaturedProducts, PriceMin, PriceMax,
                string.Empty, false, PageSize, PageIndex, FilteredSpecs, 0, true, out TotalRecords);
        }

        /// <summary>
        /// Gets all products
        /// </summary>
        /// <param name="CategoryID">Category identifier</param>
        /// <param name="ManufacturerID">Manufacturer identifier</param>
        /// <param name="FeaturedProducts">A value indicating whether loaded products are marked as featured (relates only to categories and manufacturers). 0 to load featured products only, 1 to load not featured products only, null to load all products</param>
        /// <param name="PriceMin">Minimum price</param>
        /// <param name="PriceMax">Maximum price</param>
        /// <param name="PageSize">Page size</param>
        /// <param name="PageIndex">Page index</param>
        /// <param name="FilteredSpecs">Filtered product specification identifiers</param>
        /// <param name="SortBy">field to search</param>
        /// <param name="SortTo">sort direction</param>
        /// <param name="TotalRecords">Total records</param>
        /// <returns>Product collection</returns>
        public static ProductCollection GetAllProducts(int CategoryID, int ManufacturerID,
            bool? FeaturedProducts, decimal? PriceMin, decimal? PriceMax,
            int PageSize, int PageIndex, List<int> FilteredSpecs, int SortBy, bool SortTo, out int TotalRecords)
        {
            return GetAllProducts(CategoryID, ManufacturerID, FeaturedProducts, PriceMin, PriceMax,
                string.Empty, false, PageSize, PageIndex, FilteredSpecs, SortBy, SortTo, out TotalRecords);
        }
        /// <summary>
        /// Gets all products
        /// </summary>
        /// <param name="CategoryID">Category identifier</param>
        /// <param name="ManufacturerID">Manufacturer identifier</param>
        /// <param name="FeaturedProducts">A value indicating whether loaded products are marked as featured (relates only to categories and manufacturers). 0 to load featured products only, 1 to load not featured products only, null to load all products</param>
        /// <param name="PriceMin">Minimum price</param>
        /// <param name="PriceMax">Maximum price</param>
        /// <param name="Keywords">Keywords</param>
        /// <param name="SearchDescriptions">A value indicating whether to search in descriptions</param>
        /// <param name="PageSize">Page size</param>
        /// <param name="PageIndex">Page index</param>
        /// <param name="FilteredSpecs">Filtered product specification identifiers</param>
        /// <param name="SortBy">field to search</param>
        /// <param name="SortTo">sort direction</param>
        /// <param name="TotalRecords">Total records</param>
        /// <returns>Product collection</returns>
        public static ProductCollection GetAllProducts(int CategoryID, int ManufacturerID,
            bool? FeaturedProducts, decimal? PriceMin, decimal? PriceMax, string Keywords, bool SearchDescriptions,
            int PageSize, int PageIndex, List<int> FilteredSpecs,int SortBy, bool SortTo, out int TotalRecords)
        {
            if (PageSize <= 0)
                PageSize = 10;
            if (PageSize == int.MaxValue)
                PageSize = int.MaxValue - 1;

            if (PageIndex < 0)
                PageIndex = 0;
            if (PageIndex == int.MaxValue)
                PageIndex = int.MaxValue - 1;

            bool showHidden = NopContext.Current.IsAdmin;
            DBProductCollection dbCollection = DBProviderManager<DBProductProvider>.Provider.GetAllProducts(CategoryID,
               ManufacturerID, FeaturedProducts, PriceMin, PriceMax, Keywords, SearchDescriptions,
               PageSize, PageIndex, FilteredSpecs, showHidden, SortBy, SortTo, out TotalRecords);
            ProductCollection products = DBMapping(dbCollection);
            return products;
        }

        /// <summary>
        /// Gets all products displayed on the home page
        /// </summary>
        /// <returns>Product collection</returns>
        public static ProductCollection GetAllProductsDisplayedOnHomePage()
        {
            bool showHidden = NopContext.Current.IsAdmin;
            DBProductCollection dbCollection = DBProviderManager<DBProductProvider>.Provider.GetAllProductsDisplayedOnHomePage(showHidden);
            ProductCollection products = DBMapping(dbCollection);
            return products;
        }

        /// <summary>
        /// Gets product
        /// </summary>
        /// <param name="ProductID">Product identifier</param>
        /// <returns>Product</returns>
        public static Product GetProductByID(int ProductID)
        {
            if (ProductID == 0)
                return null;

            string key = string.Format(PRODUCTS_BY_ID_KEY, ProductID);
            object obj2 = NopCache.Get(key);
            if (ProductManager.CacheEnabled && (obj2 != null))
            {
                return (Product)obj2;
            }

            DBProduct dbItem = DBProviderManager<DBProductProvider>.Provider.GetProductByID(ProductID);
            Product product = DBMapping(dbItem);

            if (ProductManager.CacheEnabled)
            {
                NopCache.Max(key, product);
            }
            return product;
        }

        /// <summary>
        /// Inserts a product
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="ShortDescription">The short description</param>
        /// <param name="FullDescription">The full description</param>
        /// <param name="AdminComment">The admin comment</param>
        /// <param name="ProductTypeID">The product type identifier</param>
        /// <param name="TemplateID">The template identifier</param>
        /// <param name="ShowOnHomePage">A value indicating whether to show the product on the home page</param>
        /// <param name="MetaKeywords">The meta keywords</param>
        /// <param name="MetaDescription">The meta description</param>
        /// <param name="MetaTitle">The meta title</param>
        /// <param name="SEName">The search-engine name</param>
        /// <param name="AllowCustomerReviews">A value indicating whether the product allows customer reviews</param>
        /// <param name="AllowCustomerRatings">A value indicating whether the product allows customer ratings</param>
        /// <param name="RatingSum">The rating sum</param>
        /// <param name="TotalRatingVotes">The total rating votes</param>
        /// <param name="Published">A value indicating whether the entity is published</param>
        /// <param name="Deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="CreatedOn">The date and time of product creation</param>
        /// <param name="UpdatedOn">The date and time of product update</param>
        /// <returns>Product</returns>
        public static Product InsertProduct(string Name, string ShortDescription,
            string FullDescription, string AdminComment,
            int ProductTypeID, int TemplateID, bool ShowOnHomePage,
            string MetaKeywords, string MetaDescription, string MetaTitle,
            string SEName, bool AllowCustomerReviews, bool AllowCustomerRatings, int RatingSum,
            int TotalRatingVotes, bool Published, bool Deleted,
            DateTime CreatedOn, DateTime UpdatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            UpdatedOn = DateTimeHelper.ConvertToUtcTime(UpdatedOn);

            DBProduct dbItem = DBProviderManager<DBProductProvider>.Provider.InsertProduct(Name, ShortDescription,
                FullDescription, AdminComment, ProductTypeID, TemplateID, ShowOnHomePage,
                MetaKeywords, MetaDescription, MetaTitle, SEName, AllowCustomerReviews,
                AllowCustomerRatings, RatingSum, TotalRatingVotes, Published, Deleted, CreatedOn, UpdatedOn);

            Product product = DBMapping(dbItem);

            if (ProductManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTS_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTS_PATTERN_KEY);
                NopCache.RemoveByPattern(TIERPRICES_PATTERN_KEY);
            }
            return product;
        }

        /// <summary>
        /// Updates the product
        /// </summary>
        /// <param name="ProductID">Product identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="ShortDescription">The short description</param>
        /// <param name="FullDescription">The full description</param>
        /// <param name="AdminComment">The admin comment</param>
        /// <param name="ProductTypeID">The product type identifier</param>
        /// <param name="ShowOnHomePage">A value indicating whether to show the product on the home page</param>
        /// <param name="TemplateID">The template identifier</param>
        /// <param name="MetaKeywords">The meta keywords</param>
        /// <param name="MetaDescription">The meta description</param>
        /// <param name="MetaTitle">The meta title</param>
        /// <param name="SEName">The search-engine name</param>
        /// <param name="AllowCustomerReviews">A value indicating whether the product allows customer reviews</param>
        /// <param name="AllowCustomerRatings">A value indicating whether the product allows customer ratings</param>
        /// <param name="RatingSum">The rating sum</param>
        /// <param name="TotalRatingVotes">The total rating votes</param>
        /// <param name="Published">A value indicating whether the entity is published</param>
        /// <param name="Deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="CreatedOn">The date and time of product creation</param>
        /// <param name="UpdatedOn">The date and time of product update</param>
        /// <returns>Product</returns>
        public static Product UpdateProduct(int ProductID, string Name, string ShortDescription,
            string FullDescription, string AdminComment,
            int ProductTypeID, int TemplateID, bool ShowOnHomePage,
            string MetaKeywords, string MetaDescription, string MetaTitle,
            string SEName, bool AllowCustomerReviews, bool AllowCustomerRatings, int RatingSum,
            int TotalRatingVotes, bool Published, bool Deleted,
            DateTime CreatedOn, DateTime UpdatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            UpdatedOn = DateTimeHelper.ConvertToUtcTime(UpdatedOn);

            DBProduct dbItem = DBProviderManager<DBProductProvider>.Provider.UpdateProduct(ProductID, Name, ShortDescription,
                FullDescription, AdminComment, ProductTypeID, TemplateID,
                ShowOnHomePage, MetaKeywords, MetaDescription, MetaTitle,
                SEName, AllowCustomerReviews, AllowCustomerRatings, RatingSum, TotalRatingVotes,
                Published, Deleted, CreatedOn, UpdatedOn);

            Product product = DBMapping(dbItem);

            if (ProductManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTS_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTS_PATTERN_KEY);
                NopCache.RemoveByPattern(TIERPRICES_PATTERN_KEY);
            }

            return product;
        }

        /// <summary>
        /// Gets a list of products purchased by other customers who purchased the above
        /// </summary>
        /// <param name="ProductID">Product identifier</param>
        /// <returns>Product collection</returns>
        public static ProductCollection GetProductsAlsoPurchasedByID(int ProductID)
        {
            int TotalRecords = 0;

            ProductCollection products = GetProductsAlsoPurchasedByID(ProductID, ProductManager.ProductsAlsoPurchasedNumber, 0, out TotalRecords);
            return products;
        }

        /// <summary>
        /// Gets a list of products purchased by other customers who purchased the above
        /// </summary>
        /// <param name="ProductID">Product identifier</param>
        /// <param name="PageSize">Page size</param>
        /// <param name="PageIndex">Page index</param>
        /// <param name="TotalRecords">Total records</param>
        /// <returns>Product collection</returns>
        public static ProductCollection GetProductsAlsoPurchasedByID(int ProductID,
            int PageSize, int PageIndex, out int TotalRecords)
        {
            if (PageSize <= 0)
                PageSize = 10;
            if (PageSize == int.MaxValue)
                PageSize = int.MaxValue - 1;

            if (PageIndex < 0)
                PageIndex = 0;
            if (PageIndex == int.MaxValue)
                PageIndex = int.MaxValue - 1;

            bool showHidden = NopContext.Current.IsAdmin;
            DBProductCollection dbCollection = DBProviderManager<DBProductProvider>.Provider.GetProductsAlsoPurchasedByID(ProductID,
               showHidden, PageSize, PageIndex, out TotalRecords);
            ProductCollection products = DBMapping(dbCollection);
            return products;
        }

        /// <summary>
        /// Sets a product rating
        /// </summary>
        /// <param name="ProductID">Product identifer</param>
        /// <param name="Rating">Rating</param>
        public static void SetProductRating(int ProductID, int Rating)
        {
            if (NopContext.Current.User == null)
            {
                return;
            }
            if (NopContext.Current.User != null && NopContext.Current.User.IsGuest)
            {
                return;
            }

            if (Rating < 1 || Rating > 5)
                Rating = 1;
            DateTime RatedOn = DateTimeHelper.ConvertToUtcTime(DateTime.Now);
            DBProviderManager<DBProductProvider>.Provider.SetProductRating(ProductID, NopContext.Current.User.CustomerID,
                Rating, RatedOn);

            if (ProductManager.CacheEnabled)
            {
                string key = string.Format(PRODUCTS_BY_ID_KEY, ProductID);
                NopCache.Remove(key);
            }
        }

        /// <summary>
        /// Clears a "compare products" list
        /// </summary>
        public static void ClearCompareProducts()
        {
            HttpCookie compareCookie = HttpContext.Current.Request.Cookies.Get("NopCommerce.CompareProducts");
            if (compareCookie != null)
            {
                compareCookie.Values.Clear();
                compareCookie.Expires = DateTime.Now.AddYears(-1);
                HttpContext.Current.Response.Cookies.Set(compareCookie);
            }
        }

        /// <summary>
        /// Gets a "compare products" list
        /// </summary>
        /// <returns></returns>
        public static ProductCollection GetCompareProducts()
        {
            ProductCollection products = new ProductCollection();
            List<int> productIDs = GetCompareProductsIDs();
            foreach (int productID in productIDs)
            {
                Product product = GetProductByID(productID);
                if (product != null && product.Published && !product.Deleted)
                    products.Add(product);
            }
            return products;
        }

        /// <summary>
        /// Gets a "compare products" identifier list
        /// </summary>
        /// <returns>"compare products" identifier list</returns>
        public static List<int> GetCompareProductsIDs()
        {
            List<int> productIDs = new List<int>();
            HttpCookie compareCookie = HttpContext.Current.Request.Cookies.Get("NopCommerce.CompareProducts");
            if ((compareCookie == null) || (compareCookie.Values == null))
                return productIDs;
            string[] values = compareCookie.Values.GetValues("CompareProductIDs");
            if (values == null)
                return productIDs;
            foreach (string productId in values)
            {
                int prodId = int.Parse(productId);
                if (!productIDs.Contains(prodId))
                    productIDs.Add(prodId);
            }

            return productIDs;
        }

        /// <summary>
        /// Removes a product from a "compare products" list
        /// </summary>
        /// <param name="ProductID">Product identifer</param>
        public static void RemoveProductFromCompareList(int ProductID)
        {
            List<int> oldProductIDs = GetCompareProductsIDs();
            List<int> newProductIDs = new List<int>();
            newProductIDs.AddRange(oldProductIDs);
            newProductIDs.Remove(ProductID);

            HttpCookie compareCookie = HttpContext.Current.Request.Cookies.Get("NopCommerce.CompareProducts");
            if ((compareCookie == null) || (compareCookie.Values == null))
                return;
            compareCookie.Values.Clear();
            foreach (int newProductID in newProductIDs)
                compareCookie.Values.Add("CompareProductIDs", newProductID.ToString());
            compareCookie.Expires = DateTime.Now.AddDays(10.0);
            HttpContext.Current.Response.Cookies.Set(compareCookie);
        }

        /// <summary>
        /// Adds a product to a "compare products" list
        /// </summary>
        /// <param name="ProductID">Product identifer</param>
        public static void AddProductToCompareList(int ProductID)
        {
            if (!ProductManager.CompareProductsEnabled)
                return;

            List<int> oldProductIDs = GetCompareProductsIDs();
            List<int> newProductIDs = new List<int>();
            newProductIDs.Add(ProductID);
            foreach (int oldProductID in oldProductIDs)
                if (oldProductID != ProductID)
                    newProductIDs.Add(oldProductID);

            HttpCookie compareCookie = HttpContext.Current.Request.Cookies.Get("NopCommerce.CompareProducts");
            if (compareCookie == null)
                compareCookie = new HttpCookie("NopCommerce.CompareProducts");
            compareCookie.Values.Clear();
            int maxProducts = 5;
            int i = 1;
            foreach (int newProductID in newProductIDs)
            {
                compareCookie.Values.Add("CompareProductIDs", newProductID.ToString());
                if (i == maxProducts)
                    break;
                i++;
            }
            compareCookie.Expires = DateTime.Now.AddDays(10.0);
            HttpContext.Current.Response.Cookies.Set(compareCookie);
        }

        /// <summary>
        /// Gets a "recently viewed products" list
        /// </summary>
        /// <param name="Number">Number of products to load</param>
        /// <returns>"recently viewed products" list</returns>
        public static ProductCollection GetRecentlyViewedProducts(int Number)
        {
            ProductCollection products = new ProductCollection();
            List<int> productIDs = GetRecentlyViewedProductsIDs(Number);
            foreach (int productID in productIDs)
            {
                Product product = GetProductByID(productID);
                if (product != null && product.Published && !product.Deleted)
                    products.Add(product);
            }
            return products;
        }

        /// <summary>
        /// Gets a "recently viewed products" identifier list
        /// </summary>
        /// <returns>"recently viewed products" list</returns>
        public static List<int> GetRecentlyViewedProductsIDs()
        {
            return GetRecentlyViewedProductsIDs(int.MaxValue);
        }

        /// <summary>
        /// Gets a "recently viewed products" identifier list
        /// </summary>
        /// <param name="Number">Number of products to load</param>
        /// <returns>"recently viewed products" list</returns>
        public static List<int> GetRecentlyViewedProductsIDs(int Number)
        {
            List<int> productIDs = new List<int>();
            HttpCookie recentlyViewedCookie = HttpContext.Current.Request.Cookies.Get("NopCommerce.RecentlyViewedProducts");
            if ((recentlyViewedCookie == null) || (recentlyViewedCookie.Values == null))
                return productIDs;
            string[] values = recentlyViewedCookie.Values.GetValues("RecentlyViewedProductIDs");
            if (values == null)
                return productIDs;
            foreach (string productId in values)
            {
                int prodId = int.Parse(productId);
                if (!productIDs.Contains(prodId))
                {
                    productIDs.Add(prodId);
                    if (productIDs.Count >= Number)
                        break;
                }

            }

            return productIDs;
        }

        /// <summary>
        /// Adds a product to a recently viewed products list
        /// </summary>
        /// <param name="ProductID">Product identifier</param>
        public static void AddProductToRecentlyViewedList(int ProductID)
        {
            if (!ProductManager.RecentlyViewedProductsEnabled)
                return;

            List<int> oldProductIDs = GetRecentlyViewedProductsIDs();
            List<int> newProductIDs = new List<int>();
            newProductIDs.Add(ProductID);
            foreach (int oldProductID in oldProductIDs)
                if (oldProductID != ProductID)
                    newProductIDs.Add(oldProductID);

            HttpCookie recentlyViewedCookie = HttpContext.Current.Request.Cookies.Get("NopCommerce.RecentlyViewedProducts");
            if (recentlyViewedCookie == null)
                recentlyViewedCookie = new HttpCookie("NopCommerce.RecentlyViewedProducts");
            recentlyViewedCookie.Values.Clear();
            int maxProducts = SettingManager.GetSettingValueInteger("Display.RecentlyViewedProductCount");
            if (maxProducts <= 0)
                maxProducts = 10;
            int i = 1;
            foreach (int newProductID in newProductIDs)
            {
                recentlyViewedCookie.Values.Add("RecentlyViewedProductIDs", newProductID.ToString());
                if (i == maxProducts)
                    break;
                i++;
            }
            recentlyViewedCookie.Expires = DateTime.Now.AddDays(10.0);
            HttpContext.Current.Response.Cookies.Set(recentlyViewedCookie);
        }

        /// <summary>
        /// Gets a recently added products list
        /// </summary>
        /// <param name="Number">Number of products to load</param>
        /// <returns>"recently added" product list</returns>
        public static ProductCollection GetRecentlyAddedProducts(int Number)
        {
            bool showHidden = NopContext.Current.IsAdmin;
            DBProductCollection dbCollection = DBProviderManager<DBProductProvider>.Provider.GetRecentlyAddedProducts(Number, showHidden);
            ProductCollection products = DBMapping(dbCollection);
            return products;
        }

        /// <summary>
        /// Direct add to cart allowed
        /// </summary>
        /// <param name="ProductID">Product identifier</param>
        /// <param name="ProductVariantID">Default product variant identifier for adding to cart</param>
        /// <returns>A value indicating whether direct add to cart is allowed</returns>
        public static bool DirectAddToCartAllowed(int ProductID, out int ProductVariantID)
        {
            bool result = false;
            ProductVariantID = 0;
            Product product = GetProductByID(ProductID);
            if (product != null)
            {
                ProductVariantCollection productVariants = product.ProductVariants;
                if (productVariants.Count == 1)
                {
                    ProductVariant defaultProductVariant = productVariants[0];

                    List<string> addToCartWarnings = ShoppingCartManager.GetShoppingCartItemWarnings(ShoppingCartTypeEnum.ShoppingCart,
                        defaultProductVariant.ProductVariantID, string.Empty, 1);

                    if (addToCartWarnings.Count == 0)
                    {
                        ProductVariantID = defaultProductVariant.ProductVariantID;
                        result = true;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Deletes a product picture mapping
        /// </summary>
        /// <param name="ProductPictureID">Product picture mapping identifier</param>
        public static void DeleteProductPicture(int ProductPictureID)
        {
            DBProviderManager<DBProductProvider>.Provider.DeleteProductPicture(ProductPictureID);
        }

        /// <summary>
        /// Gets a product picture mapping
        /// </summary>
        /// <param name="ProductPictureID">Product picture mapping identifier</param>
        /// <returns>Product picture mapping</returns>
        public static ProductPicture GetProductPictureByID(int ProductPictureID)
        {
            if (ProductPictureID == 0)
                return null;

            DBProductPicture dbItem = DBProviderManager<DBProductProvider>.Provider.GetProductPictureByID(ProductPictureID);
            ProductPicture productPicture = DBMapping(dbItem);
            return productPicture;
        }

        /// <summary>
        /// Inserts a product picture mapping
        /// </summary>
        /// <param name="ProductID">Product identifier</param>
        /// <param name="PictureID">Picture identifier</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Product picture mapping</returns>
        public static ProductPicture InsertProductPicture(int ProductID,
            int PictureID, int DisplayOrder)
        {
            DBProductPicture dbItem = DBProviderManager<DBProductProvider>.Provider.InsertProductPicture(ProductID,
            PictureID, DisplayOrder);
            ProductPicture productPicture = DBMapping(dbItem);
            return productPicture;
        }

        /// <summary>
        /// Updates the product picture mapping
        /// </summary>
        /// <param name="ProductPictureID">Product picture mapping identifier</param>
        /// <param name="ProductID">Product identifier</param>
        /// <param name="PictureID">Picture identifier</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Product picture mapping</returns>
        public static ProductPicture UpdateProductPicture(int ProductPictureID, int ProductID,
            int PictureID, int DisplayOrder)
        {
            DBProductPicture dbItem = DBProviderManager<DBProductProvider>.Provider.UpdateProductPicture(ProductPictureID, ProductID,
             PictureID, DisplayOrder);
            ProductPicture productPicture = DBMapping(dbItem);
            return productPicture;
        }

        /// <summary>
        /// Gets all product picture mappings by product identifier
        /// </summary>
        /// <param name="ProductID">Product identifier</param>
        /// <returns>Product picture mapping collection</returns>
        public static ProductPictureCollection GetProductPicturesByProductID(int ProductID)
        {
            DBProductPictureCollection dbCollection = DBProviderManager<DBProductProvider>.Provider.GetProductPicturesByProductID(ProductID);
            ProductPictureCollection productPictures = DBMapping(dbCollection);
            return productPictures;
        }

        /// <summary>
        /// Gets a product review
        /// </summary>
        /// <param name="ProductReviewID">Product review identifier</param>
        /// <returns>Product review</returns>
        public static ProductReview GetProductReviewByID(int ProductReviewID)
        {
            if (ProductReviewID == 0)
                return null;

            DBProductReview dbItem = DBProviderManager<DBProductProvider>.Provider.GetProductReviewByID(ProductReviewID);
            ProductReview productReview = DBMapping(dbItem);
            return productReview;
        }

        /// <summary>
        /// Gets a product review collection by product identifier
        /// </summary>
        /// <param name="ProductID">Product identifier</param>
        /// <returns>Product review collection</returns>
        public static ProductReviewCollection GetProductReviewByProductID(int ProductID)
        {
            bool showHidden = NopContext.Current.IsAdmin;
            DBProductReviewCollection dbCollection = DBProviderManager<DBProductProvider>.Provider.GetProductReviewByProductID(ProductID, showHidden);
            ProductReviewCollection productReviews = DBMapping(dbCollection);
            return productReviews;
        }

        /// <summary>
        /// Deletes a product review
        /// </summary>
        /// <param name="ProductReviewID">Product review identifier</param>
        public static void DeleteProductReview(int ProductReviewID)
        {
            DBProviderManager<DBProductProvider>.Provider.DeleteProductReview(ProductReviewID);
        }

        /// <summary>
        /// Gets all product reviews
        /// </summary>
        /// <returns>Product review collection</returns>
        public static ProductReviewCollection GetAllProductReviews()
        {
            bool showHidden = NopContext.Current.IsAdmin;
            DBProductReviewCollection dbCollection = DBProviderManager<DBProductProvider>.Provider.GetAllProductReviews(showHidden);
            ProductReviewCollection productReviews = DBMapping(dbCollection);
            return productReviews;
        }

        /// <summary>
        /// Inserts a product review
        /// </summary>
        /// <param name="ProductID">The product identifier</param>
        /// <param name="CustomerID">The customer identifier</param>
        /// <param name="Title">The review title</param>
        /// <param name="ReviewText">The review text</param>
        /// <param name="Rating">The review rating</param>
        /// <param name="HelpfulYesTotal">Review helpful votes total</param>
        /// <param name="HelpfulNoTotal">Review not helpful votes total</param>
        /// <param name="IsApproved">A value indicating whether the product review is approved</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>Product review</returns>
        public static ProductReview InsertProductReview(int ProductID, int CustomerID,
            string Title, string ReviewText, int Rating, int HelpfulYesTotal,
            int HelpfulNoTotal, bool IsApproved, DateTime CreatedOn)
        {
            return InsertProductReview(ProductID, CustomerID,
             Title, ReviewText, Rating, HelpfulYesTotal,
             HelpfulNoTotal, IsApproved, CreatedOn, ProductManager.NotifyAboutNewProductReviews);
        }
        /// <summary>
        /// Inserts a product review
        /// </summary>
        /// <param name="ProductID">The product identifier</param>
        /// <param name="CustomerID">The customer identifier</param>
        /// <param name="Title">The review title</param>
        /// <param name="ReviewText">The review text</param>
        /// <param name="Rating">The review rating</param>
        /// <param name="HelpfulYesTotal">Review helpful votes total</param>
        /// <param name="HelpfulNoTotal">Review not helpful votes total</param>
        /// <param name="IsApproved">A value indicating whether the product review is approved</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="notify">A value indicating whether to notify the store owner</param>
        /// <returns>Product review</returns>
        public static ProductReview InsertProductReview(int ProductID, int CustomerID,
            string Title, string ReviewText, int Rating, int HelpfulYesTotal,
            int HelpfulNoTotal, bool IsApproved, DateTime CreatedOn, bool notify)
        {
            if (Rating < 1)
                Rating = 1;
            if (Rating > 5)
                Rating = 5;

            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);

            DBProductReview dbItem = DBProviderManager<DBProductProvider>.Provider.InsertProductReview(ProductID, CustomerID,
                Title, ReviewText, Rating, HelpfulYesTotal, HelpfulNoTotal,
                IsApproved, CreatedOn);
            ProductReview productReview = DBMapping(dbItem);

            if (notify)
            {
                MessageManager.SendProductReviewNotificationMessage(productReview, LocalizationManager.DefaultAdminLanguage.LanguageID);
            }

            return productReview;
        }

        /// <summary>
        /// Updates the product review
        /// </summary>
        /// <param name="ProductReviewID">The product review identifier</param>
        /// <param name="ProductID">The product identifier</param>
        /// <param name="CustomerID">The customer identifier</param>
        /// <param name="Title">The review title</param>
        /// <param name="ReviewText">The review text</param>
        /// <param name="Rating">The review rating</param>
        /// <param name="HelpfulYesTotal">Review helpful votes total</param>
        /// <param name="HelpfulNoTotal">Review not helpful votes total</param>
        /// <param name="IsApproved">A value indicating whether the product review is approved</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>Product review</returns>
        public static ProductReview UpdateProductReview(int ProductReviewID,
            int ProductID, int CustomerID, string Title,
            string ReviewText, int Rating, int HelpfulYesTotal,
            int HelpfulNoTotal, bool IsApproved, DateTime CreatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);

            DBProductReview dbItem = DBProviderManager<DBProductProvider>.Provider.UpdateProductReview(ProductReviewID,
                ProductID, CustomerID, Title, ReviewText, Rating,
                HelpfulYesTotal, HelpfulNoTotal, IsApproved, CreatedOn);
            ProductReview productReview = DBMapping(dbItem);
            return productReview;
        }

        /// <summary>
        /// Sets a product rating helpfulness
        /// </summary>
        /// <param name="ProductReviewID">Product review identifer</param>
        /// <param name="WasHelpful">A value indicating whether the product review was helpful or not </param>
        public static void SetProductRatingHelpfulness(int ProductReviewID, bool WasHelpful)
        {
            if (NopContext.Current.User == null)
            {
                return;
            }
            if (NopContext.Current.User != null && NopContext.Current.User.IsGuest)
            {
                return;
            }

            DBProviderManager<DBProductProvider>.Provider.SetProductRatingHelpfulness(ProductReviewID,
                NopContext.Current.User.CustomerID, WasHelpful);
        }

        /// <summary>
        /// Gets all product types
        /// </summary>
        /// <returns>Product type collection</returns>
        public static ProductTypeCollection GetAllProductTypes()
        {
            DBProductTypeCollection dbCollection = DBProviderManager<DBProductProvider>.Provider.GetAllProductTypes();
            ProductTypeCollection productTypes = DBMapping(dbCollection);
            return productTypes;
        }

        /// <summary>
        /// Gets a product type
        /// </summary>
        /// <param name="ProductTypeID">Product type identifier</param>
        /// <returns>Product type</returns>
        public static ProductType GetProductTypeByID(int ProductTypeID)
        {
            if (ProductTypeID == 0)
                return null;

            DBProductType dbItem = DBProviderManager<DBProductProvider>.Provider.GetProductTypeByID(ProductTypeID);
            ProductType productType = DBMapping(dbItem);
            return productType;
        }

        /// <summary>
        /// Marks a product variant as deleted
        /// </summary>
        /// <param name="ProductVariantID">Product variant identifier</param>
        public static void MarkProductVariantAsDeleted(int ProductVariantID)
        {
            ProductVariant productVariant = GetProductVariantByID(ProductVariantID);
            if (productVariant != null)
            {
                productVariant = UpdateProductVariant(productVariant.ProductVariantID, productVariant.ProductID, productVariant.Name,
                    productVariant.SKU, productVariant.Description, productVariant.AdminComment, productVariant.ManufacturerPartNumber,
                    productVariant.IsDownload, productVariant.DownloadID,
                    productVariant.UnlimitedDownloads,productVariant.MaxNumberOfDownloads,
                    productVariant.HasSampleDownload,productVariant.SampleDownloadID,
                    productVariant.IsShipEnabled, productVariant.IsFreeShipping, productVariant.AdditionalShippingCharge,
                    productVariant.IsTaxExempt, productVariant.TaxCategoryID,
                    productVariant.ManageInventory, productVariant.StockQuantity,
                    productVariant.MinStockQuantity, productVariant.LowStockActivity,
                    productVariant.NotifyAdminForQuantityBelow, productVariant.OrderMinimumQuantity, productVariant.OrderMaximumQuantity,
                    productVariant.WarehouseId, productVariant.DisableBuyButton,
                    productVariant.Price, productVariant.OldPrice,
                    productVariant.Weight, productVariant.Length, productVariant.Width, productVariant.Height, productVariant.PictureID,
                    productVariant.AvailableStartDateTime, productVariant.AvailableEndDateTime,
                    productVariant.Published, true, productVariant.DisplayOrder, productVariant.CreatedOn, productVariant.UpdatedOn);
            }
        }

        /// <summary>
        /// Adjusts inventory
        /// </summary>
        /// <param name="ProductVariantID">Product variant identifier</param>
        /// <param name="decrease">A value indicating whether to increase or descrease product variant stock quantity</param>
        /// <param name="Quantity">Quantity</param>
        public static void AdjustInventory(int ProductVariantID, bool decrease, int Quantity)
        {
            ProductVariant productVariant = GetProductVariantByID(ProductVariantID);
            if (productVariant == null || productVariant.IsDownload)
                return;

            if (!productVariant.ManageInventory)
                return;

            int newStockQuantity = 0;
            if (decrease)
                newStockQuantity = productVariant.StockQuantity - Quantity;
            else
                newStockQuantity = productVariant.StockQuantity + Quantity;

            bool newPublished = productVariant.Published;
            bool newDisableBuyButton = productVariant.DisableBuyButton;

            //check if minimum quantity is reached
            if (productVariant.MinStockQuantity >= newStockQuantity)
            {
                switch (productVariant.LowStockActivity)
                {
                    case LowStockActivityEnum.DisableBuyButton:
                        newDisableBuyButton = true;
                        break;
                    case LowStockActivityEnum.Unpublish:
                        newPublished = false;
                        break;
                    default:
                        break;
                }
            }

            if (productVariant.ManageInventory)
            {
                if (decrease && productVariant.NotifyAdminForQuantityBelow > newStockQuantity)
                {
                    MessageManager.SendQuantityBelowStoreOwnerNotification(productVariant, LocalizationManager.DefaultAdminLanguage.LanguageID);
                }
            }


            if (productVariant != null)
            {
                productVariant = UpdateProductVariant(productVariant.ProductVariantID, productVariant.ProductID, productVariant.Name,
                     productVariant.SKU, productVariant.Description, productVariant.AdminComment, productVariant.ManufacturerPartNumber,
                     productVariant.IsDownload, productVariant.DownloadID,
                     productVariant.UnlimitedDownloads, productVariant.MaxNumberOfDownloads,
                     productVariant.HasSampleDownload, productVariant.SampleDownloadID, productVariant.IsShipEnabled,
                     productVariant.IsFreeShipping, productVariant.AdditionalShippingCharge,
                     productVariant.IsTaxExempt, productVariant.TaxCategoryID,
                     productVariant.ManageInventory, newStockQuantity, productVariant.MinStockQuantity, productVariant.LowStockActivity,
                     productVariant.NotifyAdminForQuantityBelow, productVariant.OrderMinimumQuantity, productVariant.OrderMaximumQuantity,
                     productVariant.WarehouseId, newDisableBuyButton, productVariant.Price, productVariant.OldPrice,
                     productVariant.Weight, productVariant.Length, productVariant.Width, productVariant.Height, productVariant.PictureID,
                     productVariant.AvailableStartDateTime, productVariant.AvailableEndDateTime,
                     newPublished, productVariant.Deleted, productVariant.DisplayOrder, productVariant.CreatedOn, productVariant.UpdatedOn);

                Product product = productVariant.Product;
                bool allProductVariantsUnpublished = true;
                foreach (ProductVariant pv2 in product.ProductVariants)
                {
                    if (pv2.Published)
                    {
                        allProductVariantsUnpublished = false;
                        break;
                    }
                }

                if (allProductVariantsUnpublished)
                {
                    UpdateProduct(product.ProductID, product.Name, product.ShortDescription,
                        product.FullDescription, product.AdminComment, product.ProductTypeID,
                        product.TemplateID, product.ShowOnHomePage, product.MetaKeywords, product.MetaDescription,
                        product.MetaTitle, product.SEName, product.AllowCustomerReviews, product.AllowCustomerRatings, product.RatingSum,
                        product.TotalRatingVotes, false, product.Deleted, product.CreatedOn, product.UpdatedOn);
                }
            }
        }

        /// <summary>
        /// Remove a product variant picture
        /// </summary>
        /// <param name="ProductVariantID">Product variant identifier</param>
        public static void RemoveProductVariantPicture(int ProductVariantID)
        {
            ProductVariant productVariant = GetProductVariantByID(ProductVariantID);
            if (productVariant != null)
            {
                UpdateProductVariant(productVariant.ProductVariantID, productVariant.ProductID, productVariant.Name,
                    productVariant.SKU, productVariant.Description, productVariant.AdminComment, productVariant.ManufacturerPartNumber,
                    productVariant.IsDownload, productVariant.DownloadID,
                    productVariant.UnlimitedDownloads, productVariant.MaxNumberOfDownloads,
                    productVariant.HasSampleDownload, productVariant.SampleDownloadID, productVariant.IsShipEnabled,
                    productVariant.IsFreeShipping, productVariant.AdditionalShippingCharge,
                    productVariant.IsTaxExempt, productVariant.TaxCategoryID, productVariant.ManageInventory,
                    productVariant.StockQuantity, productVariant.MinStockQuantity,
                    productVariant.LowStockActivity, productVariant.NotifyAdminForQuantityBelow,
                    productVariant.OrderMinimumQuantity,
                    productVariant.OrderMaximumQuantity, productVariant.WarehouseId,
                    productVariant.DisableBuyButton, productVariant.Price, productVariant.OldPrice,
                    productVariant.Weight, productVariant.Length, productVariant.Width, productVariant.Height, 0,
                    productVariant.AvailableStartDateTime, productVariant.AvailableEndDateTime,
                    productVariant.Published, productVariant.Deleted, productVariant.DisplayOrder, productVariant.CreatedOn, productVariant.UpdatedOn);
            }
        }

        /// <summary>
        /// Get low stock product variants
        /// </summary>
        /// <returns>Result</returns>
        public static ProductVariantCollection GetLowStockProductVariants()
        {
            DBProductVariantCollection dbCollection = DBProviderManager<DBProductProvider>.Provider.GetLowStockProductVariants();
            ProductVariantCollection productVariants = DBMapping(dbCollection);
            return productVariants;
        }

        /// <summary>
        /// Remove a product variant download
        /// </summary>
        /// <param name="ProductVariantID">Product variant identifier</param>
        public static void RemoveProductVariantDownload(int ProductVariantID)
        {
            ProductVariant productVariant = GetProductVariantByID(ProductVariantID);
            if (productVariant != null)
            {
                UpdateProductVariant(productVariant.ProductVariantID, productVariant.ProductID, productVariant.Name,
                    productVariant.SKU, productVariant.Description, productVariant.AdminComment,
                    productVariant.ManufacturerPartNumber, productVariant.IsDownload, 0,
                    productVariant.UnlimitedDownloads, productVariant.MaxNumberOfDownloads,
                    productVariant.HasSampleDownload, productVariant.SampleDownloadID,
                    productVariant.IsShipEnabled, productVariant.IsFreeShipping, productVariant.AdditionalShippingCharge,
                    productVariant.IsTaxExempt, productVariant.TaxCategoryID, productVariant.ManageInventory,
                    productVariant.StockQuantity, productVariant.MinStockQuantity,
                    productVariant.LowStockActivity, productVariant.NotifyAdminForQuantityBelow,
                    productVariant.OrderMinimumQuantity, productVariant.OrderMaximumQuantity, productVariant.WarehouseId,
                    productVariant.DisableBuyButton, productVariant.Price, productVariant.OldPrice,
                    productVariant.Weight, productVariant.Length, productVariant.Width, productVariant.Height,
                    productVariant.PictureID, productVariant.AvailableStartDateTime, productVariant.AvailableEndDateTime,
                    productVariant.Published, productVariant.Deleted,
                    productVariant.DisplayOrder, productVariant.CreatedOn, productVariant.UpdatedOn);
            }
        }

        /// <summary>
        /// Remove a product variant sample download
        /// </summary>
        /// <param name="ProductVariantID">Product variant identifier</param>
        public static void RemoveProductVariantSampleDownload(int ProductVariantID)
        {
            ProductVariant productVariant = GetProductVariantByID(ProductVariantID);
            if (productVariant != null)
            {
                UpdateProductVariant(productVariant.ProductVariantID, productVariant.ProductID, productVariant.Name,
                    productVariant.SKU, productVariant.Description, productVariant.AdminComment,
                    productVariant.ManufacturerPartNumber, productVariant.IsDownload, productVariant.DownloadID,
                    productVariant.UnlimitedDownloads, productVariant.MaxNumberOfDownloads,
                    productVariant.HasSampleDownload, 0, productVariant.IsShipEnabled,
                    productVariant.IsFreeShipping, productVariant.AdditionalShippingCharge,
                    productVariant.IsTaxExempt, productVariant.TaxCategoryID, productVariant.ManageInventory,
                    productVariant.StockQuantity, productVariant.MinStockQuantity,
                    productVariant.LowStockActivity, productVariant.NotifyAdminForQuantityBelow,
                    productVariant.OrderMinimumQuantity, productVariant.OrderMaximumQuantity, productVariant.WarehouseId,
                    productVariant.DisableBuyButton, productVariant.Price, productVariant.OldPrice,
                    productVariant.Weight, productVariant.Length, productVariant.Width, productVariant.Height,
                    productVariant.PictureID, productVariant.AvailableStartDateTime, productVariant.AvailableEndDateTime,
                    productVariant.Published, productVariant.Deleted,
                    productVariant.DisplayOrder, productVariant.CreatedOn, productVariant.UpdatedOn);
            }
        }

        /// <summary>
        /// Gets a product variant
        /// </summary>
        /// <param name="ProductVariantID">Product variant identifier</param>
        /// <returns>Product variant</returns>
        public static ProductVariant GetProductVariantByID(int ProductVariantID)
        {
            if (ProductVariantID == 0)
                return null;

            string key = string.Format(PRODUCTVARIANTS_BY_ID_KEY, ProductVariantID);
            object obj2 = NopCache.Get(key);
            if (ProductManager.CacheEnabled && (obj2 != null))
            {
                return (ProductVariant)obj2;
            }

            DBProductVariant dbItem = DBProviderManager<DBProductProvider>.Provider.GetProductVariantByID(ProductVariantID);
            ProductVariant productVariant = DBMapping(dbItem);

            if (ProductManager.CacheEnabled)
            {
                NopCache.Max(key, productVariant);
            }
            return productVariant;
        }

        /// <summary>
        /// Inserts a product variant
        /// </summary>
        /// <param name="ProductID">The product identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="SKU">The SKU</param>
        /// <param name="Description">The description</param>
        /// <param name="AdminComment">The admin comment</param>
        /// <param name="ManufacturerPartNumber">The manufacturer part number</param>
        /// <param name="IsDownload">A value indicating whether the product variant is download</param>
        /// <param name="DownloadID">The download identifier</param>
        /// <param name="UnlimitedDownloads">The value indicating whether this downloadable product can be downloaded unlimited number of times</param>
        /// <param name="MaxNumberOfDownloads">The maximum number of downloads</param>
        /// <param name="HasSampleDownload">The value indicating whether the product variant has a sample download file</param>
        /// <param name="SampleDownloadID">The sample download identifier</param>
        /// <param name="IsShipEnabled">A value indicating whether the entity is ship enabled</param>
        /// <param name="IsFreeShipping">A value indicating whether the entity is free shipping</param>
        /// <param name="AdditionalShippingCharge">The additional shipping charge</param>
        /// <param name="IsTaxExempt">A value indicating whether the product variant is marked as tax exempt</param>
        /// <param name="TaxCategoryID">The tax category identifier</param>
        /// <param name="ManageInventory">The value indicating whether to manage inventory</param>
        /// <param name="StockQuantity">The stock quantity</param>
        /// <param name="MinStockQuantity">The minimum stock quantity</param>
        /// <param name="LowStockActivity">The low stock activity</param>
        /// <param name="NotifyAdminForQuantityBelow">The quantity when admin should be notified</param>
        /// <param name="OrderMinimumQuantity">The order minimum quantity</param>
        /// <param name="OrderMaximumQuantity">The order maximum quantity</param>
        /// <param name="WarehouseId">The warehouse identifier</param>
        /// <param name="DisableBuyButton">A value indicating whether to disable buy button</param>
        /// <param name="Price">The price</param>
        /// <param name="OldPrice">The old price</param>
        /// <param name="Weight">The weight</param>
        /// <param name="Length">The length</param>
        /// <param name="Width">The width</param>
        /// <param name="Height">The height</param>
        /// <param name="PictureID">The picture identifier</param>
        /// <param name="AvailableStartDateTime">The available start date and time</param>
        /// <param name="AvailableEndDateTime">The available end date and time</param>
        /// <param name="Published">A value indicating whether the entity is published</param>
        /// <param name="Deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Product variant</returns>
        public static ProductVariant InsertProductVariant(int ProductID, string Name,
            string SKU, string Description, string AdminComment,
            string ManufacturerPartNumber, bool IsDownload, int DownloadID,
            bool UnlimitedDownloads, int MaxNumberOfDownloads,
            bool HasSampleDownload, int SampleDownloadID, bool IsShipEnabled,
            bool IsFreeShipping, decimal AdditionalShippingCharge,
            bool IsTaxExempt, int TaxCategoryID, bool ManageInventory,
            int StockQuantity, int MinStockQuantity, LowStockActivityEnum LowStockActivity,
            int NotifyAdminForQuantityBelow, int OrderMinimumQuantity, int OrderMaximumQuantity,
            int WarehouseId, bool DisableBuyButton, decimal Price, decimal OldPrice,
            decimal Weight, decimal Length, decimal Width, decimal Height, int PictureID,
            DateTime? AvailableStartDateTime, DateTime? AvailableEndDateTime,
           bool Published, bool Deleted, int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn)
        {
            if (AvailableStartDateTime.HasValue)
                AvailableStartDateTime = DateTimeHelper.ConvertToUtcTime(AvailableStartDateTime.Value);
            if (AvailableEndDateTime.HasValue)
                AvailableEndDateTime = DateTimeHelper.ConvertToUtcTime(AvailableEndDateTime.Value);
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            UpdatedOn = DateTimeHelper.ConvertToUtcTime(UpdatedOn);

            DBProductVariant dbItem = DBProviderManager<DBProductProvider>.Provider.InsertProductVariant(ProductID, Name, SKU, Description, AdminComment,
                ManufacturerPartNumber, IsDownload, DownloadID, UnlimitedDownloads, MaxNumberOfDownloads,
                HasSampleDownload, SampleDownloadID, IsShipEnabled,
                IsFreeShipping, AdditionalShippingCharge, IsTaxExempt, TaxCategoryID, ManageInventory,
                StockQuantity, MinStockQuantity, (int)LowStockActivity, NotifyAdminForQuantityBelow,
                OrderMinimumQuantity, OrderMaximumQuantity, WarehouseId, DisableBuyButton,
                Price, OldPrice,
                Weight, Length, Width, Height, PictureID,
                AvailableStartDateTime, AvailableEndDateTime,
                Published, Deleted, DisplayOrder, CreatedOn, UpdatedOn);
            ProductVariant productVariant = DBMapping(dbItem);

            if (ProductManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTS_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTS_PATTERN_KEY);
                NopCache.RemoveByPattern(TIERPRICES_PATTERN_KEY);
            }

            return productVariant;
        }

        /// <summary>
        /// Updates the product variant
        /// </summary>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <param name="ProductID">The product identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="SKU">The SKU</param>
        /// <param name="Description">The description</param>
        /// <param name="AdminComment">The admin comment</param>
        /// <param name="ManufacturerPartNumber">The manufacturer part number</param>
        /// <param name="IsDownload">A value indicating whether the product variant is download</param>
        /// <param name="DownloadID">The download identifier</param>
        /// <param name="UnlimitedDownloads">The value indicating whether this downloadable product can be downloaded unlimited number of times</param>
        /// <param name="MaxNumberOfDownloads">The maximum number of downloads</param>
        /// <param name="HasSampleDownload">The value indicating whether the product variant has a sample download file</param>
        /// <param name="SampleDownloadID">The sample download identifier</param>
        /// <param name="IsShipEnabled">A value indicating whether the entity is ship enabled</param>
        /// <param name="IsFreeShipping">A value indicating whether the entity is free shipping</param>
        /// <param name="AdditionalShippingCharge">The additional shipping charge</param>
        /// <param name="IsTaxExempt">A value indicating whether the product variant is marked as tax exempt</param>
        /// <param name="TaxCategoryID">The tax category identifier</param>
        /// <param name="ManageInventory">The value indicating whether to manage inventory</param>
        /// <param name="StockQuantity">The stock quantity</param>
        /// <param name="MinStockQuantity">The minimum stock quantity</param>
        /// <param name="LowStockActivity">The low stock activity</param>
        /// <param name="NotifyAdminForQuantityBelow">The quantity when admin should be notified</param>
        /// <param name="OrderMinimumQuantity">The order minimum quantity</param>
        /// <param name="OrderMaximumQuantity">The order maximum quantity</param>
        /// <param name="WarehouseId">The warehouse identifier</param>
        /// <param name="DisableBuyButton">A value indicating whether to disable buy button</param>
        /// <param name="Price">The price</param>
        /// <param name="OldPrice">The old price</param>
        /// <param name="Weight">The weight</param>
        /// <param name="Length">The length</param>
        /// <param name="Width">The width</param>
        /// <param name="Height">The height</param>
        /// <param name="PictureID">The picture identifier</param>
        /// <param name="AvailableStartDateTime">The available start date and time</param>
        /// <param name="AvailableEndDateTime">The available end date and time</param>
        /// <param name="Published">A value indicating whether the entity is published</param>
        /// <param name="Deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Product variant</returns>
        public static ProductVariant UpdateProductVariant(int ProductVariantID, int ProductID, string Name, string SKU, string Description, string AdminComment,
            string ManufacturerPartNumber, bool IsDownload, int DownloadID,
            bool UnlimitedDownloads, int MaxNumberOfDownloads,
            bool HasSampleDownload, int SampleDownloadID, bool IsShipEnabled,
            bool IsFreeShipping, decimal AdditionalShippingCharge,
            bool IsTaxExempt, int TaxCategoryID, bool ManageInventory,
            int StockQuantity, int MinStockQuantity, LowStockActivityEnum LowStockActivity,
            int NotifyAdminForQuantityBelow, int OrderMinimumQuantity, int OrderMaximumQuantity,
            int WarehouseId, bool DisableBuyButton,
            decimal Price, decimal OldPrice, decimal Weight, decimal Length, decimal Width,
            decimal Height, int PictureID, DateTime? AvailableStartDateTime, DateTime? AvailableEndDateTime,
            bool Published, bool Deleted, int DisplayOrder,
            DateTime CreatedOn, DateTime UpdatedOn)
        {
            if (AvailableStartDateTime.HasValue)
                AvailableStartDateTime = DateTimeHelper.ConvertToUtcTime(AvailableStartDateTime.Value);
            if (AvailableEndDateTime.HasValue)
                AvailableEndDateTime = DateTimeHelper.ConvertToUtcTime(AvailableEndDateTime.Value);
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            UpdatedOn = DateTimeHelper.ConvertToUtcTime(UpdatedOn);

            DBProductVariant dbItem = DBProviderManager<DBProductProvider>.Provider.UpdateProductVariant(ProductVariantID,
                ProductID, Name, SKU, Description, AdminComment,
                ManufacturerPartNumber, IsDownload, DownloadID, UnlimitedDownloads, MaxNumberOfDownloads,
                HasSampleDownload, SampleDownloadID, IsShipEnabled,
                IsFreeShipping, AdditionalShippingCharge, IsTaxExempt, TaxCategoryID, ManageInventory, StockQuantity,
                MinStockQuantity, (int)LowStockActivity, NotifyAdminForQuantityBelow,
                OrderMinimumQuantity, OrderMaximumQuantity, WarehouseId, DisableBuyButton,
                Price, OldPrice,
                Weight, Length, Width, Height, PictureID,
                AvailableStartDateTime, AvailableEndDateTime,
                Published, Deleted, DisplayOrder, CreatedOn, UpdatedOn);

            ProductVariant productVariant = DBMapping(dbItem);

            if (ProductManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTS_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTS_PATTERN_KEY);
                NopCache.RemoveByPattern(TIERPRICES_PATTERN_KEY);
            }

            return productVariant;
        }

        /// <summary>
        /// Gets product variants by product identifier
        /// </summary>
        /// <param name="ProductID">The product identifier</param>
        /// <returns>Product variant collection</returns>
        public static ProductVariantCollection GetProductVariantsByProductID(int ProductID)
        {
            bool showHidden = NopContext.Current.IsAdmin;
            return GetProductVariantsByProductID(ProductID, showHidden);
        }

        /// <summary>
        /// Gets product variants by product identifier
        /// </summary>
        /// <param name="ProductID">The product identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product variant collection</returns>
        public static ProductVariantCollection GetProductVariantsByProductID(int ProductID, bool showHidden)
        {
            string key = string.Format(PRODUCTVARIANTS_ALL_KEY, showHidden, ProductID);
            object obj2 = NopCache.Get(key);
            if (ProductManager.CacheEnabled && (obj2 != null))
            {
                return (ProductVariantCollection)obj2;
            }

            DBProductVariantCollection dbCollection = DBProviderManager<DBProductProvider>.Provider.GetProductVariantsByProductID(ProductID, showHidden);
            ProductVariantCollection productVariants = DBMapping(dbCollection);

            if (ProductManager.CacheEnabled)
            {
                NopCache.Max(key, productVariants);
            }
            return productVariants;
        }

        /// <summary>
        /// Deletes a related product
        /// </summary>
        /// <param name="RelatedProductID">Related product identifer</param>
        public static void DeleteRelatedProduct(int RelatedProductID)
        {
            DBProviderManager<DBProductProvider>.Provider.DeleteRelatedProduct(RelatedProductID);
        }

        /// <summary>
        /// Gets a related product collection by product identifier
        /// </summary>
        /// <param name="ProductID1">The first product identifier</param>
        /// <returns>Related product collection</returns>
        public static RelatedProductCollection GetRelatedProductsByProductID1(int ProductID1)
        {
            bool showHidden = NopContext.Current.IsAdmin;
            DBRelatedProductCollection dbCollection = DBProviderManager<DBProductProvider>.Provider.GetRelatedProductsByProductID1(ProductID1, showHidden);
            RelatedProductCollection relatedProducts = DBMapping(dbCollection);
            return relatedProducts;
        }

        /// <summary>
        /// Gets a related product
        /// </summary>
        /// <param name="RelatedProductID">Related product identifer</param>
        /// <returns></returns>
        public static RelatedProduct GetRelatedProductByID(int RelatedProductID)
        {
            if (RelatedProductID == 0)
                return null;

            DBRelatedProduct dbItem = DBProviderManager<DBProductProvider>.Provider.GetRelatedProductByID(RelatedProductID);
            RelatedProduct relatedProduct = DBMapping(dbItem);
            return relatedProduct;
        }

        /// <summary>
        /// Inserts a related product
        /// </summary>
        /// <param name="ProductID1">The first product identifier</param>
        /// <param name="ProductID2">The second product identifier</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Related product</returns>
        public static RelatedProduct InsertRelatedProduct(int ProductID1, int ProductID2, int DisplayOrder)
        {
            DBRelatedProduct dbItem = DBProviderManager<DBProductProvider>.Provider.InsertRelatedProduct(ProductID1, ProductID2, DisplayOrder);
            RelatedProduct relatedProduct = DBMapping(dbItem);
            return relatedProduct;
        }

        /// <summary>
        /// Updates a related product
        /// </summary>
        /// <param name="RelatedProductID">The related product identifier</param>
        /// <param name="ProductID1">The first product identifier</param>
        /// <param name="ProductID2">The second product identifier</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Related product</returns>
        public static RelatedProduct UpdateRelatedProduct(int RelatedProductID, int ProductID1, int ProductID2,
            int DisplayOrder)
        {
            DBRelatedProduct dbItem = DBProviderManager<DBProductProvider>.Provider.UpdateRelatedProduct(RelatedProductID, ProductID1, ProductID2, DisplayOrder);
            RelatedProduct relatedProduct = DBMapping(dbItem);
            return relatedProduct;
        }

        /// <summary>
        /// Gets all product variants directly assigned to a pricelist
        /// </summary>
        /// <param name="PricelistID"></param>
        /// <returns></returns>
        public static ProductVariantCollection GetProductVariantsByPricelistID(int PricelistID)
        {

            DBProductVariantCollection dbItem = DBProviderManager<DBProductProvider>.Provider.GetProductVariantsByPricelistID(PricelistID);

            ProductVariantCollection newProductVariantCollection = ProductManager.DBMapping(dbItem);

            return newProductVariantCollection;
        }

        /// <summary>
        /// Deletes a Pricelist
        /// </summary>
        /// <param name="PricelistID">The PricelistID of the item to be deleted</param>
        public static void DeletePricelist(int PricelistID)
        {
            DBProviderManager<DBProductProvider>.Provider.DeletePricelist(PricelistID);
        }

        /// <summary>
        /// Gets a collection of all available pricelists
        /// </summary>
        /// <returns>Collection of pricelists</returns>
        public static PricelistCollection GetAllPricelists()
        {
            DBPricelistCollection dbCollection = DBProviderManager<DBProductProvider>.Provider.GetAllPricelists();
            PricelistCollection pricelists = DBMapping(dbCollection);
            return pricelists;
        }

        /// <summary>
        /// Gets a Pricelist
        /// </summary>
        /// <param name="PricelistID">Pricelist identifier</param>
        /// <returns>Pricelist</returns>
        public static Pricelist GetPricelistByID(int PricelistID)
        {
            if (PricelistID == 0)
                return null;

            DBPricelist dbItem = DBProviderManager<DBProductProvider>.Provider.GetPricelistByID(PricelistID);
            Pricelist newPricelist = DBMapping(dbItem);

            return newPricelist;
        }

        /// <summary>
        /// Gets a Pricelist
        /// </summary>
        /// <param name="PricelistGUID">Pricelist GUID</param>
        /// <returns>Pricelist</returns>
        public static Pricelist GetPricelistByGUID(string PricelistGUID)
        {
            DBPricelist dbItem = DBProviderManager<DBProductProvider>.Provider.GetPricelistByGUID(PricelistGUID);
            Pricelist newPricelist = DBMapping(dbItem);

            return newPricelist;
        }

        /// <summary>
        /// Inserts a Pricelist
        /// </summary>
        /// <param name="ExportMode">Mode of list creation (Export all, assigned only, assigned only with special price)</param>
        /// <param name="ExportType">CSV or XML</param>
        /// <param name="AffiliateID">Affiliate connected to this pricelist (optional), links will be created with AffiliateID</param>
        /// <param name="DisplayName">Displayedname</param>
        /// <param name="ShortName">shortname to identify the pricelist</param>
        /// <param name="PricelistGuid">unique identifier to get pricelist "anonymous"</param>
        /// <param name="CacheTime">how long will the pricelist be in cached before new creation</param>
        /// <param name="FormatLocalization">what localization will be used (numeric formats, etc.) en-US, de-DE etc.</param>
        /// <param name="Description">Displayed description</param>
        /// <param name="AdminNotes">Admin can put some notes here, not displayed in public</param>
        /// <param name="Header">Headerline of the exported file (plain text)</param>
        /// <param name="Body">template for an exportet productvariant, uses delimiters and replacement strings</param>
        /// <param name="Footer">Footer line of the exportet file (plain text)</param>
        /// <param name="PriceAdjustmentType">type of price adjustment (if used) (relative or absolute)</param>
        /// <param name="PriceAdjustment">price will be adjusted by this amount (in accordance with PriceAdjustmentType)</param>
        /// <param name="OverrideIndivAdjustment">use individual adjustment, if available, or override</param>
        /// <param name="CreatedOn">when was this record originally created</param>
        /// <param name="UpdatedOn">last time this recordset was updated</param>
        /// <returns>Pricelist</returns>
        public static Pricelist InsertPricelist(PriceListExportModeEnum ExportMode, PriceListExportTypeEnum ExportType, int AffiliateID,
            string DisplayName, string ShortName, string PricelistGuid, int CacheTime, string FormatLocalization,
            string Description, string AdminNotes, string Header, string Body, string Footer,
            PriceAdjustmentTypeEnum PriceAdjustmentType, decimal PriceAdjustment, bool OverrideIndivAdjustment,
            DateTime CreatedOn, DateTime UpdatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            UpdatedOn = DateTimeHelper.ConvertToUtcTime(UpdatedOn);

            DBPricelist dbItem = DBProviderManager<DBProductProvider>.Provider.InsertPricelist((int)ExportMode, (int)ExportType, AffiliateID,
                DisplayName, ShortName, PricelistGuid, CacheTime, FormatLocalization,
                Description, AdminNotes, Header, Body, Footer,
                (int)PriceAdjustmentType, PriceAdjustment, OverrideIndivAdjustment,
                CreatedOn, UpdatedOn);
            Pricelist newPricelist = DBMapping(dbItem);

            return newPricelist;
        }

        /// <summary>
        /// Updates the Pricelist
        /// </summary>
        /// <param name="PricelistID">Unique Identifier</param>
        /// <param name="ExportMode">Mode of list creation (Export all, assigned only, assigned only with special price)</param>
        /// <param name="ExportType">CSV or XML</param>
        /// <param name="AffiliateID">Affiliate connected to this pricelist (optional), links will be created with AffiliateID</param>
        /// <param name="DisplayName">Displayedname</param>
        /// <param name="ShortName">shortname to identify the pricelist</param>
        /// <param name="PricelistGuid">unique identifier to get pricelist "anonymous"</param>
        /// <param name="CacheTime">how long will the pricelist be in cached before new creation</param>
        /// <param name="FormatLocalization">what localization will be used (numeric formats, etc.) en-US, de-DE etc.</param>
        /// <param name="Description">Displayed description</param>
        /// <param name="AdminNotes">Admin can put some notes here, not displayed in public</param>
        /// <param name="Header">Headerline of the exported file (plain text)</param>
        /// <param name="Body">template for an exportet productvariant, uses delimiters and replacement strings</param>
        /// <param name="Footer">Footer line of the exportet file (plain text)</param>
        /// <param name="PriceAdjustmentType">type of price adjustment (if used) (relative or absolute)</param>
        /// <param name="PriceAdjustment">price will be adjusted by this amount (in accordance with PriceAdjustmentType)</param>
        /// <param name="OverrideIndivAdjustment">use individual adjustment, if available, or override</param>
        /// <param name="CreatedOn">when was this record originally created</param>
        /// <param name="UpdatedOn">last time this recordset was updated</param>
        /// <returns>Pricelist</returns>
        public static Pricelist UpdatePricelist(int PricelistID, PriceListExportModeEnum ExportMode, PriceListExportTypeEnum ExportType, int AffiliateID,
            string DisplayName, string ShortName, string PricelistGuid, int CacheTime, string FormatLocalization,
            string Description, string AdminNotes,
            string Header, string Body, string Footer,
            PriceAdjustmentTypeEnum PriceAdjustmentType, decimal PriceAdjustment, bool OverrideIndivAdjustment,
            DateTime CreatedOn, DateTime UpdatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            UpdatedOn = DateTimeHelper.ConvertToUtcTime(UpdatedOn);

            DBPricelist dbItem = DBProviderManager<DBProductProvider>.Provider.UpdatePricelist(PricelistID, (int)ExportMode,
                (int)ExportType, AffiliateID, DisplayName, ShortName, PricelistGuid,
                CacheTime, FormatLocalization, Description, AdminNotes, Header, Body, Footer,
                (int)PriceAdjustmentType, PriceAdjustment, OverrideIndivAdjustment,
                CreatedOn, UpdatedOn);
            Pricelist newPricelist = DBMapping(dbItem);

            return newPricelist;
        }

        /// <summary>
        /// Deletes a ProductVariantPricelist
        /// </summary>
        /// <param name="ProductVariantPricelistID">ProductVariantPricelist identifier</param>
        public static void DeleteProductVariantPricelist(int ProductVariantPricelistID)
        {
            if (ProductVariantPricelistID == 0)
                return;
            DBProviderManager<DBProductProvider>.Provider.DeleteProductVariantPricelist(ProductVariantPricelistID);
        }

        /// <summary>
        /// Gets a ProductVariantPricelist
        /// </summary>
        /// <param name="ProductVariantPricelistID">ProductVariantPricelist identifier</param>
        /// <returns>ProductVariantPricelist</returns>
        public static ProductVariantPricelist GetProductVariantPricelistByID(int ProductVariantPricelistID)
        {
            if (ProductVariantPricelistID == 0)
                return null;

            DBProductVariantPricelist dbItem = DBProviderManager<DBProductProvider>.Provider.GetProductVariantPricelistByID(ProductVariantPricelistID);
            ProductVariantPricelist newProductVariantPricelist = DBMapping(dbItem);

            return newProductVariantPricelist;
        }

        /// <summary>
        /// Gets ProductVariantPricelist
        /// </summary>
        /// <param name="ProductVariantID">ProductVariant identifier</param>
        /// <param name="PricelistID">Pricelist identifier</param>
        /// <returns>ProductVariantPricelist</returns>
        public static ProductVariantPricelist GetProductVariantPricelist(int ProductVariantID, int PricelistID)
        {
            DBProductVariantPricelist dbItem = DBProviderManager<DBProductProvider>.Provider.GetProductVariantPricelist(ProductVariantID, PricelistID);
            ProductVariantPricelist newProductVariantPricelist = DBMapping(dbItem);

            return newProductVariantPricelist;
        }

        /// <summary>
        /// Inserts a ProductVariantPricelist
        /// </summary>
        /// <param name="ProductVariantID">The product variant identifer</param>
        /// <param name="PricelistID">The pricelist identifier</param>
        /// <param name="PriceAdjustmentType">The type of price adjustment (if used) (relative or absolute)</param>
        /// <param name="PriceAdjustment">The price will be adjusted by this amount</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>ProductVariantPricelist</returns>
        public static ProductVariantPricelist InsertProductVariantPricelist(int ProductVariantID, int PricelistID,
            PriceAdjustmentTypeEnum PriceAdjustmentType, decimal PriceAdjustment,
            DateTime UpdatedOn)
        {
            UpdatedOn = DateTimeHelper.ConvertToUtcTime(UpdatedOn);

            DBProductVariantPricelist dbItem = DBProviderManager<DBProductProvider>.Provider.InsertProductVariantPricelist(ProductVariantID,
                PricelistID, (int)PriceAdjustmentType, PriceAdjustment, UpdatedOn);
            ProductVariantPricelist newProductVariantPricelist = DBMapping(dbItem);

            return newProductVariantPricelist;
        }

        /// <summary>
        /// Updates the ProductVariantPricelist
        /// </summary>
        /// <param name="ProductVariantPricelistID">The product variant pricelist identifier</param>
        /// <param name="ProductVariantID">The product variant identifer</param>
        /// <param name="PricelistID">The pricelist identifier</param>
        /// <param name="PriceAdjustmentType">The type of price adjustment (if used) (relative or absolute)</param>
        /// <param name="PriceAdjustment">The price will be adjusted by this amount</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>ProductVariantPricelist</returns>
        public static ProductVariantPricelist UpdateProductVariantPricelist(int ProductVariantPricelistID, int ProductVariantID, int PricelistID,
            PriceAdjustmentTypeEnum PriceAdjustmentType, decimal PriceAdjustment,
            DateTime UpdatedOn)
        {
            if (ProductVariantPricelistID == 0)
                return null;

            UpdatedOn = DateTimeHelper.ConvertToUtcTime(UpdatedOn);

            DBProductVariantPricelist dbItem = DBProviderManager<DBProductProvider>.Provider.UpdateProductVariantPricelist(ProductVariantPricelistID,
                ProductVariantID, PricelistID, (int)PriceAdjustmentType,
                PriceAdjustment, UpdatedOn);
            ProductVariantPricelist newProductVariantPricelist = DBMapping(dbItem);

            return newProductVariantPricelist;
        }

        /// <summary>
        /// Gets a tier price
        /// </summary>
        /// <param name="TierPriceID">Tier price identifier</param>
        /// <returns>Tier price</returns>
        public static TierPrice GetTierPriceByID(int TierPriceID)
        {
            if (TierPriceID == 0)
                return null;

            DBTierPrice dbItem = DBProviderManager<DBProductProvider>.Provider.GetTierPriceByID(TierPriceID);
            TierPrice tierPrice = DBMapping(dbItem);
            return tierPrice;
        }

        /// <summary>
        /// Gets tier prices by product variant identifier
        /// </summary>
        /// <param name="ProductVariantID">Product variant identifier</param>
        /// <returns>Tier price collection</returns>
        public static TierPriceCollection GetTierPricesByProductVariantID(int ProductVariantID)
        {
            if (ProductVariantID == 0)
                return new TierPriceCollection();

            string key = string.Format(TIERPRICES_ALLBYPRODUCTVARIANTID_KEY, ProductVariantID);
            object obj2 = NopCache.Get(key);
            if (ProductManager.CacheEnabled && (obj2 != null))
            {
                return (TierPriceCollection)obj2;
            }

            DBTierPriceCollection dbCollection = DBProviderManager<DBProductProvider>.Provider.GetTierPricesByProductVariantID(ProductVariantID);
            TierPriceCollection tierPriceCollection = DBMapping(dbCollection);

            if (ProductManager.CacheEnabled)
            {
                NopCache.Max(key, tierPriceCollection);
            }
            return tierPriceCollection;
        }

        /// <summary>
        /// Deletes a tier price
        /// </summary>
        /// <param name="TierPriceID">Tier price identifier</param>
        public static void DeleteTierPrice(int TierPriceID)
        {
            DBProviderManager<DBProductProvider>.Provider.DeleteTierPrice(TierPriceID);

            if (ProductManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTS_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTS_PATTERN_KEY);
                NopCache.RemoveByPattern(TIERPRICES_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Inserts a tier price
        /// </summary>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <param name="Quantity">The quantity</param>
        /// <param name="Price">The price</param>
        /// <returns>Tier price</returns>
        public static TierPrice InsertTierPrice(int ProductVariantID, int Quantity, decimal Price)
        {
            DBTierPrice dbItem = DBProviderManager<DBProductProvider>.Provider.InsertTierPrice(ProductVariantID, Quantity, Price);
            TierPrice tierPrice = DBMapping(dbItem);

            if (ProductManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTS_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTS_PATTERN_KEY);
                NopCache.RemoveByPattern(TIERPRICES_PATTERN_KEY);
            }
            return tierPrice;
        }

        /// <summary>
        /// Updates the tier price
        /// </summary>
        /// <param name="TierPriceID">The tier price identifier</param>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <param name="Quantity">The quantity</param>
        /// <param name="Price">The price</param>
        /// <returns>Tier price</returns>
        public static TierPrice UpdateTierPrice(int TierPriceID, int ProductVariantID, int Quantity, decimal Price)
        {
            DBTierPrice dbItem = DBProviderManager<DBProductProvider>.Provider.UpdateTierPrice(TierPriceID, ProductVariantID, Quantity, Price);
            TierPrice tierPrice = DBMapping(dbItem);

            if (ProductManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTS_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTS_PATTERN_KEY);
                NopCache.RemoveByPattern(TIERPRICES_PATTERN_KEY);
            }

            return tierPrice;
        }

        /// <summary>
        /// Formats the text
        /// </summary>
        /// <param name="Text">Text</param>
        /// <returns>Formatted text</returns>
        public static string FormatProductReviewText(string Text)
        {
            if (String.IsNullOrEmpty(Text))
                return string.Empty;

            Text = HtmlHelper.FormatText(Text, false, true, false, false, false, false);

            return Text;
        }

        /// <summary>
        /// Formats the email a friend text
        /// </summary>
        /// <param name="Text">Text</param>
        /// <returns>Formatted text</returns>
        public static string FormatEmailAFriendText(string Text)
        {
            if (String.IsNullOrEmpty(Text))
                return string.Empty;

            Text = HtmlHelper.FormatText(Text, false, true, false, false, false, false);
            return Text;
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
                return SettingManager.GetSettingValueBoolean("Cache.ProductManager.CacheEnabled");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether "Recently viewed products" feature is enabled
        /// </summary>
        public static bool RecentlyViewedProductsEnabled
        {
            get
            {
                bool recentlyViewedProductsEnabled = SettingManager.GetSettingValueBoolean("Display.RecentlyViewedProductsEnabled");
                return recentlyViewedProductsEnabled;
            }
            set
            {
                SettingManager.SetParam("Display.RecentlyViewedProductsEnabled", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether "Recently added products" feature is enabled
        /// </summary>
        public static bool RecentlyAddedProductsEnabled
        {
            get
            {
                bool recentlyAddedProductsEnabled = SettingManager.GetSettingValueBoolean("Display.RecentlyAddedProductsEnabled");
                return recentlyAddedProductsEnabled;
            }
            set
            {
                SettingManager.SetParam("Display.RecentlyAddedProductsEnabled", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether "Compare products" feature is enabled
        /// </summary>
        public static bool CompareProductsEnabled
        {
            get
            {
                bool compareProductsEnabled = SettingManager.GetSettingValueBoolean("Common.EnableCompareProducts");
                return compareProductsEnabled;
            }
            set
            {
                SettingManager.SetParam("Common.EnableCompareProducts", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets "List of products purchased by other customers who purchased the above" option is enable
        /// </summary>
        public static bool ProductsAlsoPurchasedEnabled
        {
            get
            {
                bool productsAlsoPurchased = SettingManager.GetSettingValueBoolean("Display.ListOfProductsAlsoPurchasedEnabled");
                return productsAlsoPurchased;
            }
            set
            {
                SettingManager.SetParam("Display.ListOfProductsAlsoPurchasedEnabled", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a number of products also purchased by other customers to display
        /// </summary>
        public static int ProductsAlsoPurchasedNumber
        {
            get
            {
                int productsAlsoPurchasedNumber = SettingManager.GetSettingValueInteger("Display.ListOfProductsAlsoPurchasedNumberToDisplay");
                return productsAlsoPurchasedNumber;
            }
            set
            {
                SettingManager.SetParam("Display.ListOfProductsAlsoPurchasedNumberToDisplay", value.ToString());
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether to notify about new product reviews
        /// </summary>
        public static bool NotifyAboutNewProductReviews
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("Product.NotifyAboutNewProductReviews");
            }
            set
            {
                SettingManager.SetParam("Product.NotifyAboutNewProductReviews", value.ToString());
            }
        }
        #endregion
    }
}