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

namespace NopSolutions.NopCommerce.DataAccess.Products
{
    /// <summary>
    /// Acts as a base class for deriving custom product provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/ProductProvider")]
    public abstract partial class DBProductProvider : BaseDBProvider
    {
        #region Methods
        /// <summary>
        /// Gets all products
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product collection</returns>
        public abstract DBProductCollection GetAllProducts(bool showHidden);

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
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="TotalRecords">Total records</param>
        /// <returns>Product collection</returns>
        public abstract DBProductCollection GetAllProducts(int CategoryID, int ManufacturerID,
            bool? FeaturedProducts, decimal? PriceMin, decimal? PriceMax, string Keywords, bool SearchDescriptions,
            int PageSize, int PageIndex, List<int> FilteredSpecs, bool showHidden,
            int SortBy, bool SortTo, int minHeight, int maxHeight, int minWidth, int maxWidth, out int TotalRecords);

        /// <summary>
        /// Gets all products displayed on the home page
        /// </summary>
        /// <returns>Product collection</returns>
        public abstract DBProductCollection GetAllProductsDisplayedOnHomePage(bool showHidden);

        /// <summary>
        /// Gets product
        /// </summary>
        /// <param name="ProductID">Product identifier</param>
        /// <returns>Product</returns>
        public abstract DBProduct GetProductByID(int ProductID);

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
        public abstract DBProduct InsertProduct(string Name, string ShortDescription, string FullDescription, 
            string AdminComment, int ProductTypeID, int TemplateID, bool ShowOnHomePage, 
            string MetaKeywords, string MetaDescription, string MetaTitle,
            string SEName, bool AllowCustomerReviews, bool AllowCustomerRatings, int RatingSum,
            int TotalRatingVotes, bool Published, bool Deleted, DateTime CreatedOn, DateTime UpdatedOn);

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
        public abstract DBProduct UpdateProduct(int ProductID, string Name, string ShortDescription, 
            string FullDescription, string AdminComment, int ProductTypeID, int TemplateID, 
            bool ShowOnHomePage, string MetaKeywords, string MetaDescription, string MetaTitle,
            string SEName, bool AllowCustomerReviews, bool AllowCustomerRatings, 
            int RatingSum, int TotalRatingVotes, 
            bool Published, bool Deleted, DateTime CreatedOn, DateTime UpdatedOn);

        /// <summary>
        /// Gets a list of products purchased by other customers who purchased the above
        /// </summary>
        /// <param name="ProductID">Product identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="PageSize">Page size</param>
        /// <param name="PageIndex">Page index</param>
        /// <param name="TotalRecords">Total records</param>
        /// <returns>Product collection</returns>
        public abstract DBProductCollection GetProductsAlsoPurchasedByID(int ProductID,
            bool showHidden, int PageSize, int PageIndex, out int TotalRecords);

        /// <summary>
        /// Sets a product rating
        /// </summary>
        /// <param name="ProductID">Product identifer</param>
        /// <param name="CustomerID">Customer identifer</param>
        /// <param name="Rating">Rating</param>
        /// <param name="RatedOn">Rating was created on</param>
        public abstract void SetProductRating(int ProductID, int CustomerID, int Rating, DateTime RatedOn);

        /// <summary>
        /// Gets a recently added products list
        /// </summary>
        /// <param name="Number">Number of products to load</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Recently added products list</returns>
        public abstract DBProductCollection GetRecentlyAddedProducts(int Number, bool showHidden);

        /// <summary>
        /// Deletes a product picture mapping
        /// </summary>
        /// <param name="ProductPictureID">Product picture mapping identifier</param>
        public abstract void DeleteProductPicture(int ProductPictureID);

        /// <summary>
        /// Gets a product picture mapping
        /// </summary>
        /// <param name="ProductPictureID">Product picture mapping identifier</param>
        /// <returns>Product picture mapping</returns>
        public abstract DBProductPicture GetProductPictureByID(int ProductPictureID);

        /// <summary>
        /// Inserts a product picture mapping
        /// </summary>
        /// <param name="ProductID">Product identifier</param>
        /// <param name="PictureID">Picture identifier</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Product picture mapping</returns>
        public abstract DBProductPicture InsertProductPicture(int ProductID,
          int PictureID, int DisplayOrder);

        /// <summary>
        /// Updates the product picture mapping
        /// </summary>
        /// <param name="ProductPictureID">Product picture mapping identifier</param>
        /// <param name="ProductID">Product identifier</param>
        /// <param name="PictureID">Picture identifier</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Product picture mapping</returns>
        public abstract DBProductPicture UpdateProductPicture(int ProductPictureID, int ProductID,
            int PictureID, int DisplayOrder);

        /// <summary>
        /// Gets all product picture mappings by product identifier
        /// </summary>
        /// <param name="ProductID">Product identifier</param>
        /// <returns>Product picture mapping collection</returns>
        public abstract DBProductPictureCollection GetProductPicturesByProductID(int ProductID);

        /// <summary>
        /// Gets a product review
        /// </summary>
        /// <param name="ProductReviewID">Product review identifier</param>
        /// <returns>Product review</returns>
        public abstract DBProductReview GetProductReviewByID(int ProductReviewID);

        /// <summary>
        /// Gets a product review collection by product identifier
        /// </summary>
        /// <param name="ProductID">Product identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product review collection</returns>
        public abstract DBProductReviewCollection GetProductReviewByProductID(int ProductID, bool showHidden);

        /// <summary>
        /// Deletes a product review
        /// </summary>
        /// <param name="ProductReviewID">Product review identifier</param>
        public abstract void DeleteProductReview(int ProductReviewID);

        /// <summary>
        /// Gets all product reviews
        /// </summary>
        /// <returns>Product review collection</returns>
        public abstract DBProductReviewCollection GetAllProductReviews(bool showHidden);

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
        public abstract DBProductReview InsertProductReview(int ProductID, int CustomerID, string Title,
            string ReviewText, int Rating, int HelpfulYesTotal,
            int HelpfulNoTotal, bool IsApproved, DateTime CreatedOn);

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
        public abstract DBProductReview UpdateProductReview(int ProductReviewID, int ProductID,
            int CustomerID, string Title, string ReviewText, int Rating, int HelpfulYesTotal,
            int HelpfulNoTotal, bool IsApproved, DateTime CreatedOn);

        /// <summary>
        /// Sets a product rating helpfulness
        /// </summary>
        /// <param name="ProductReviewID">Product review identifer</param>
        /// <param name="CustomerID">Customer identifer</param>
        /// <param name="WasHelpful">A value indicating whether the product review was helpful or not </param>
        public abstract void SetProductRatingHelpfulness(int ProductReviewID,
            int CustomerID, bool WasHelpful);

        /// <summary>
        /// Gets a product variant
        /// </summary>
        /// <param name="ProductVariantID">Product variant identifier</param>
        /// <returns>Product variant</returns>
        public abstract DBProductVariant GetProductVariantByID(int ProductVariantID);

        /// <summary>
        /// Get low stock product variants
        /// </summary>
        /// <returns>Result</returns>
        public abstract DBProductVariantCollection GetLowStockProductVariants();

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
        /// <param name="StockQuantity">The stock quantity</param>
        /// <param name="ManageInventory">The value indicating whether to manage inventory</param>
        /// <param name="MinStockQuantity">The minimum stock quantity</param>
        /// <param name="LowStockActivityID">The low stock activity identifier</param>
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
        public abstract DBProductVariant InsertProductVariant(int ProductID, string Name, string SKU,
            string Description, string AdminComment, string ManufacturerPartNumber, bool IsDownload,
            int DownloadID, bool UnlimitedDownloads, int MaxNumberOfDownloads,
            bool HasSampleDownload, int SampleDownloadID, bool IsShipEnabled, bool IsFreeShipping,
            decimal AdditionalShippingCharge, bool IsTaxExempt, int TaxCategoryID,
            bool ManageInventory, int StockQuantity, int MinStockQuantity, int LowStockActivityID,
            int NotifyAdminForQuantityBelow, int OrderMinimumQuantity, int OrderMaximumQuantity,
            int WarehouseId, bool DisableBuyButton, decimal Price, decimal OldPrice,
            decimal Weight, decimal Length, decimal Width, decimal Height, int PictureID,
            DateTime? AvailableStartDateTime, DateTime? AvailableEndDateTime,
            bool Published, bool Deleted, int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn);

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
        /// <param name="StockQuantity">The stock quantity</param>
        /// <param name="ManageInventory">The value indicating whether to manage inventory</param>
        /// <param name="MinStockQuantity">The minimum stock quantity</param>
        /// <param name="LowStockActivityID">The low stock activity identifier</param>
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
        public abstract DBProductVariant UpdateProductVariant(int ProductVariantID, int ProductID, 
            string Name, string SKU, string Description, string AdminComment,
            string ManufacturerPartNumber, bool IsDownload, int DownloadID, 
            bool UnlimitedDownloads, int MaxNumberOfDownloads,
            bool HasSampleDownload, int SampleDownloadID, bool IsShipEnabled,
            bool IsFreeShipping, decimal AdditionalShippingCharge, 
            bool IsTaxExempt, int TaxCategoryID, bool ManageInventory, 
            int StockQuantity, int MinStockQuantity, int LowStockActivityID,
            int NotifyAdminForQuantityBelow, int OrderMinimumQuantity, int OrderMaximumQuantity, 
            int WarehouseId, bool DisableBuyButton, decimal Price, decimal OldPrice,
            decimal Weight, decimal Length, decimal Width, decimal Height, int PictureID,
            DateTime? AvailableStartDateTime, DateTime? AvailableEndDateTime,
            bool Published, bool Deleted, int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn);

        /// <summary>
        /// Gets product variants by product identifier
        /// </summary>
        /// <param name="ProductID">The product identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product variant collection</returns>
        public abstract DBProductVariantCollection GetProductVariantsByProductID(int ProductID, bool showHidden);

        /// <summary>
        /// Deletes a related product
        /// </summary>
        /// <param name="RelatedProductID">Related product identifer</param>
        public abstract void DeleteRelatedProduct(int RelatedProductID);

        /// <summary>
        /// Gets a related product collection by product identifier
        /// </summary>
        /// <param name="ProductID1">The first product identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Related product collection</returns>
        public abstract DBRelatedProductCollection GetRelatedProductsByProductID1(int ProductID1, bool showHidden);

        /// <summary>
        /// Gets a related product collection by product identifier
        /// </summary>
        /// <param name="ProductID1">The first product identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Related product collection</returns>
        public abstract DBRelatedProductCollection GetRelatedProductsByProductID1Paged(int ProductID1, bool showHidden, int PageIndex,
            int PageSize, ref int TotalRecords);
        
        /// <summary>
        /// Gets a related product
        /// </summary>
        /// <param name="RelatedProductID">Related product identifer</param>
        /// <returns></returns>
        public abstract DBRelatedProduct GetRelatedProductByID(int RelatedProductID);

        /// <summary>
        /// Inserts a related product
        /// </summary>
        /// <param name="ProductID1">The first product identifier</param>
        /// <param name="ProductID2">The second product identifier</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Related product</returns>
        public abstract DBRelatedProduct InsertRelatedProduct(int ProductID1, int ProductID2, int DisplayOrder);

        /// <summary>
        /// Updates a related product
        /// </summary>
        /// <param name="RelatedProductID">The related product identifier</param>
        /// <param name="ProductID1">The first product identifier</param>
        /// <param name="ProductID2">The second product identifier</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Related product</returns>
        public abstract DBRelatedProduct UpdateRelatedProduct(int RelatedProductID, int ProductID1, int ProductID2,
            int DisplayOrder);

        /// <summary>
        /// Gets all product types
        /// </summary>
        /// <returns>Product type collection</returns>
        public abstract DBProductTypeCollection GetAllProductTypes();

        /// <summary>
        /// Gets a product type
        /// </summary>
        /// <param name="ProductTypeID">Product type identifier</param>
        /// <returns>Product type</returns>
        public abstract DBProductType GetProductTypeByID(int ProductTypeID);

        /// <summary>
        /// Gets all product variants directly assigned to a pricelist
        /// </summary>
        /// <param name="PricelistID"></param>
        /// <returns></returns>
        public abstract DBProductVariantCollection GetProductVariantsByPricelistID(int PricelistID);

        /// <summary>
        /// Gets a collection of all available pricelists
        /// </summary>
        /// <returns>Collection of pricelists</returns>
        public abstract DBPricelistCollection GetAllPricelists();

        /// <summary>
        /// Gets a Pricelist
        /// </summary>
        /// <param name="PricelistID">Pricelist identifier</param>
        /// <returns>Pricelist</returns>
        public abstract DBPricelist GetPricelistByID(int PricelistID);

        /// <summary>
        /// Gets a pricelist
        /// </summary>
        /// <param name="PricelistGUID">Pricelist GUID</param>
        /// <returns>Pricelist</returns>
        public abstract DBPricelist GetPricelistByGUID(string PricelistGUID);

        /// <summary>
        /// Inserts a pricelist
        /// </summary>
        /// <param name="ExportModeID">Mode of list creation identifier</param>
        /// <param name="ExportTypeID">Export type identifier</param>
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
        /// <param name="PriceAdjustmentTypeID">Type of price adjustment identifier</param>
        /// <param name="PriceAdjustment">price will be adjusted by this amount</param>
        /// <param name="OverrideIndivAdjustment">Use individual adjustment, if available, or override</param>
        /// <param name="CreatedOn">When was this record originally created</param>
        /// <param name="UpdatedOn">Last time this record was updated</param>
        /// <returns>Pricelist</returns>
        public abstract DBPricelist InsertPricelist(int ExportModeID, int ExportTypeID, int? AffiliateID,
            string DisplayName, string ShortName, string PricelistGuid, int CacheTime, string FormatLocalization,
            string Description, string AdminNotes,
            string Header, string Body, string Footer,
            int PriceAdjustmentTypeID, decimal PriceAdjustment, bool OverrideIndivAdjustment,
            DateTime CreatedOn, DateTime UpdatedOn);

        /// <summary>
        /// Updates the Pricelist
        /// </summary>
        /// <param name="PricelistID">Unique Identifier</param>
        /// <param name="ExportModeID">Mode of list creation identifier</param>
        /// <param name="ExportTypeID">Export type identifier</param>
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
        /// <param name="PriceAdjustmentTypeID">Type of price adjustment identifier</param>
        /// <param name="PriceAdjustment">price will be adjusted by this amount</param>
        /// <param name="OverrideIndivAdjustment">use individual adjustment, if available, or override</param>
        /// <param name="CreatedOn">When was this record originally created</param>
        /// <param name="UpdatedOn">Last time this recordset was updated</param>
        /// <returns>Pricelist</returns>
        public abstract DBPricelist UpdatePricelist(int PricelistID, int ExportModeID, int ExportTypeID, int? AffiliateID,
            string DisplayName, string ShortName, string PricelistGuid, int CacheTime, string FormatLocalization,
            string Description, string AdminNotes,
            string Header, string Body, string Footer,
            int PriceAdjustmentTypeID, decimal PriceAdjustment, bool OverrideIndivAdjustment,
            DateTime CreatedOn, DateTime UpdatedOn);

        /// <summary>
        /// Deletes a Pricelist
        /// </summary>
        /// <param name="PricelistID">The PricelistID of the item to be deleted</param>
        public abstract void DeletePricelist(int PricelistID);

        /// <summary>
        /// Deletes a ProductVariantPricelist
        /// </summary>
        /// <param name="ProductVariantPricelistID">ProductVariantPricelist identifier</param>
        public abstract void DeleteProductVariantPricelist(int ProductVariantPricelistID);

        /// <summary>
        /// Gets a ProductVariantPricelist
        /// </summary>
        /// <param name="ProductVariantPricelistID">ProductVariantPricelist identifier</param>
        /// <returns>ProductVariantPricelist</returns>
        public abstract DBProductVariantPricelist GetProductVariantPricelistByID(int ProductVariantPricelistID);

        /// <summary>
        /// Gets ProductVariantPricelist
        /// </summary>
        /// <param name="ProductVariantID">ProductVariant identifier</param>
        /// <param name="PricelistID">Pricelist identifier</param>
        /// <returns>ProductVariantPricelist</returns>
        public abstract DBProductVariantPricelist GetProductVariantPricelist(int ProductVariantID, int PricelistID);

        /// <summary>
        /// Inserts a ProductVariantPricelist
        /// </summary>
        /// <param name="ProductVariantID">The product variant identifer</param>
        /// <param name="PricelistID">The pricelist identifier</param>
        /// <param name="PriceAdjustmentTypeID">Price adjustment type identifier</param>
        /// <param name="PriceAdjustment">The price will be adjusted by this amount</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>ProductVariantPricelist</returns>
        public abstract DBProductVariantPricelist InsertProductVariantPricelist(int ProductVariantID,
            int PricelistID, int PriceAdjustmentTypeID, decimal PriceAdjustment,
            DateTime UpdatedOn);

        /// <summary>
        /// Updates the ProductVariantPricelist
        /// </summary>
        /// <param name="ProductVariantPricelistID">The product variant pricelist identifier</param>
        /// <param name="ProductVariantID">The product variant identifer</param>
        /// <param name="PricelistID">The pricelist identifier</param>
        /// <param name="PriceAdjustmentTypeID">Price adjustment type identifier</param>
        /// <param name="PriceAdjustment">The price will be adjusted by this amount</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>ProductVariantPricelist</returns>
        public abstract DBProductVariantPricelist UpdateProductVariantPricelist(int ProductVariantPricelistID, int ProductVariantID,
            int PricelistID, int PriceAdjustmentTypeID, decimal PriceAdjustment,
            DateTime UpdatedOn);

        /// <summary>
        /// Gets a tier price
        /// </summary>
        /// <param name="TierPriceID">Tier price identifier</param>
        /// <returns>Tier price</returns>
        public abstract DBTierPrice GetTierPriceByID(int TierPriceID);

        /// <summary>
        /// Gets tier prices by product variant identifier
        /// </summary>
        /// <param name="ProductVariantID">Product variant identifier</param>
        /// <returns>Tier price collection</returns>
        public abstract DBTierPriceCollection GetTierPricesByProductVariantID(int ProductVariantID);

        /// <summary>
        /// Deletes a tier price
        /// </summary>
        /// <param name="TierPriceID">Tier price identifier</param>
        public abstract void DeleteTierPrice(int TierPriceID);

        /// <summary>
        /// Inserts a tier price
        /// </summary>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <param name="Quantity">The quantity</param>
        /// <param name="Price">The price</param>
        /// <returns>Tier price</returns>
        public abstract DBTierPrice InsertTierPrice(int ProductVariantID, int Quantity, decimal Price);

        /// <summary>
        /// Updates the tier price
        /// </summary>
        /// <param name="TierPriceID">The tier price identifier</param>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <param name="Quantity">The quantity</param>
        /// <param name="Price">The price</param>
        /// <returns>Tier price</returns>
        public abstract DBTierPrice UpdateTierPrice(int TierPriceID, int ProductVariantID, int Quantity, decimal Price);

        /// <summary>
        /// Get List of specification attribute options of products with category categoryID
        /// </summary>
        /// <param name="categoryID">category identifier</param>
        /// <param name="specificationAttributeID">specification attribute identifier</param>
        /// <returns></returns>
        public abstract List<int> GetProductSpecificationAttributeOptionsByCategory(int categoryID, int specificationAttributeID);

        public abstract decimal GetMaxPrice(int categoryId);

        #endregion

        public abstract decimal GetMaxHeight(int categoryId);
        public abstract decimal GetMaxWidth(int categoryId);
    }
}
