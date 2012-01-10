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
using System.Configuration.Provider;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace NopSolutions.NopCommerce.DataAccess.Products
{
    /// <summary>
    /// Product provider for SQL Server
    /// </summary>
    public partial class SQLProductProvider : DBProductProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBProduct GetProductFromReader(IDataReader dataReader)
        {
            DBProduct product = new DBProduct();
            product.ProductID = NopSqlDataHelper.GetInt(dataReader, "ProductID");
            product.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            product.ShortDescription = NopSqlDataHelper.GetString(dataReader, "ShortDescription");
            product.FullDescription = NopSqlDataHelper.GetString(dataReader, "FullDescription");
            product.AdminComment = NopSqlDataHelper.GetString(dataReader, "AdminComment");
            product.ProductTypeID = NopSqlDataHelper.GetInt(dataReader, "ProductTypeID");
            product.TemplateID = NopSqlDataHelper.GetInt(dataReader, "TemplateID");
            product.ShowOnHomePage = NopSqlDataHelper.GetBoolean(dataReader, "ShowOnHomePage");
            product.MetaKeywords = NopSqlDataHelper.GetString(dataReader, "MetaKeywords");
            product.MetaDescription = NopSqlDataHelper.GetString(dataReader, "MetaDescription");
            product.MetaTitle = NopSqlDataHelper.GetString(dataReader, "MetaTitle");
            product.SEName = NopSqlDataHelper.GetString(dataReader, "SEName");
            product.AllowCustomerReviews = NopSqlDataHelper.GetBoolean(dataReader, "AllowCustomerReviews");
            product.AllowCustomerRatings = NopSqlDataHelper.GetBoolean(dataReader, "AllowCustomerRatings");
            product.RatingSum = NopSqlDataHelper.GetInt(dataReader, "RatingSum");
            product.TotalRatingVotes = NopSqlDataHelper.GetInt(dataReader, "TotalRatingVotes");
            product.Published = NopSqlDataHelper.GetBoolean(dataReader, "Published");
            product.Deleted = NopSqlDataHelper.GetBoolean(dataReader, "Deleted");
            product.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            product.UpdatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "UpdatedOn");
            return product;
        }

        private DBProductPicture GetProductPictureFromReader(IDataReader dataReader)
        {
            DBProductPicture productPicture = new DBProductPicture();
            productPicture.ProductPictureID = NopSqlDataHelper.GetInt(dataReader, "ProductPictureID");
            productPicture.ProductID = NopSqlDataHelper.GetInt(dataReader, "ProductID");
            productPicture.PictureID = NopSqlDataHelper.GetInt(dataReader, "PictureID");
            productPicture.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            return productPicture;
        }

        private DBProductReview GetProductReviewFromReader(IDataReader dataReader)
        {
            DBProductReview productReview = new DBProductReview();
            productReview.ProductReviewID = NopSqlDataHelper.GetInt(dataReader, "ProductReviewID");
            productReview.ProductID = NopSqlDataHelper.GetInt(dataReader, "ProductID");
            productReview.CustomerID = NopSqlDataHelper.GetInt(dataReader, "CustomerID");
            productReview.Title = NopSqlDataHelper.GetString(dataReader, "Title");
            productReview.ReviewText = NopSqlDataHelper.GetString(dataReader, "ReviewText");
            productReview.Rating = NopSqlDataHelper.GetInt(dataReader, "Rating");
            productReview.HelpfulYesTotal = NopSqlDataHelper.GetInt(dataReader, "HelpfulYesTotal");
            productReview.HelpfulNoTotal = NopSqlDataHelper.GetInt(dataReader, "HelpfulNoTotal");
            productReview.IsApproved = NopSqlDataHelper.GetBoolean(dataReader, "IsApproved");
            productReview.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            return productReview;
        }

        private DBProductType GetProductTypeFromReader(IDataReader dataReader)
        {
            DBProductType productType = new DBProductType();
            productType.ProductTypeID = NopSqlDataHelper.GetInt(dataReader, "ProductTypeID");
            productType.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            productType.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            productType.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            productType.UpdatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "UpdatedOn");
            return productType;
        }

        private DBProductVariant GetProductVariantFromReader(IDataReader dataReader)
        {
            DBProductVariant productVariant = new DBProductVariant();
            productVariant.ProductVariantID = NopSqlDataHelper.GetInt(dataReader, "ProductVariantID");
            productVariant.ProductID = NopSqlDataHelper.GetInt(dataReader, "ProductID");
            productVariant.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            productVariant.SKU = NopSqlDataHelper.GetString(dataReader, "SKU");
            productVariant.Description = NopSqlDataHelper.GetString(dataReader, "Description");
            productVariant.AdminComment = NopSqlDataHelper.GetString(dataReader, "AdminComment");
            productVariant.ManufacturerPartNumber = NopSqlDataHelper.GetString(dataReader, "ManufacturerPartNumber");
            productVariant.IsDownload = NopSqlDataHelper.GetBoolean(dataReader, "IsDownload");
            productVariant.DownloadID = NopSqlDataHelper.GetInt(dataReader, "DownloadID");
            productVariant.UnlimitedDownloads = NopSqlDataHelper.GetBoolean(dataReader, "UnlimitedDownloads");
            productVariant.MaxNumberOfDownloads = NopSqlDataHelper.GetInt(dataReader, "MaxNumberOfDownloads");
            productVariant.HasSampleDownload = NopSqlDataHelper.GetBoolean(dataReader, "HasSampleDownload");
            productVariant.SampleDownloadID = NopSqlDataHelper.GetInt(dataReader, "SampleDownloadID");
            productVariant.IsShipEnabled = NopSqlDataHelper.GetBoolean(dataReader, "IsShipEnabled");
            productVariant.IsFreeShipping = NopSqlDataHelper.GetBoolean(dataReader, "IsFreeShipping");
            productVariant.AdditionalShippingCharge = NopSqlDataHelper.GetDecimal(dataReader, "AdditionalShippingCharge");
            productVariant.IsTaxExempt = NopSqlDataHelper.GetBoolean(dataReader, "IsTaxExempt");
            productVariant.TaxCategoryID = NopSqlDataHelper.GetInt(dataReader, "TaxCategoryID");
            productVariant.ManageInventory = NopSqlDataHelper.GetBoolean(dataReader, "ManageInventory");
            productVariant.StockQuantity = NopSqlDataHelper.GetInt(dataReader, "StockQuantity");
            productVariant.MinStockQuantity = NopSqlDataHelper.GetInt(dataReader, "MinStockQuantity");
            productVariant.LowStockActivityID = NopSqlDataHelper.GetInt(dataReader, "LowStockActivityID");
            productVariant.NotifyAdminForQuantityBelow = NopSqlDataHelper.GetInt(dataReader, "NotifyAdminForQuantityBelow");
            productVariant.OrderMinimumQuantity = NopSqlDataHelper.GetInt(dataReader, "OrderMinimumQuantity");
            productVariant.OrderMaximumQuantity = NopSqlDataHelper.GetInt(dataReader, "OrderMaximumQuantity");
            productVariant.WarehouseId = NopSqlDataHelper.GetInt(dataReader, "WarehouseId");
            productVariant.DisableBuyButton = NopSqlDataHelper.GetBoolean(dataReader, "DisableBuyButton");
            productVariant.Price = NopSqlDataHelper.GetDecimal(dataReader, "Price");
            productVariant.OldPrice = NopSqlDataHelper.GetDecimal(dataReader, "OldPrice");
            productVariant.Weight = NopSqlDataHelper.GetDecimal(dataReader, "Weight");
            productVariant.Length = NopSqlDataHelper.GetDecimal(dataReader, "Length");
            productVariant.Width = NopSqlDataHelper.GetDecimal(dataReader, "Width");
            productVariant.Height = NopSqlDataHelper.GetDecimal(dataReader, "Height");
            productVariant.PictureID = NopSqlDataHelper.GetInt(dataReader, "PictureID");
            productVariant.AvailableStartDateTime = NopSqlDataHelper.GetNullableUtcDateTime(dataReader, "AvailableStartDateTime");
            productVariant.AvailableEndDateTime = NopSqlDataHelper.GetNullableUtcDateTime(dataReader, "AvailableEndDateTime");
            productVariant.Published = NopSqlDataHelper.GetBoolean(dataReader, "Published");
            productVariant.Deleted = NopSqlDataHelper.GetBoolean(dataReader, "Deleted");
            productVariant.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            productVariant.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            productVariant.UpdatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "UpdatedOn");
            return productVariant;
        }

        private DBRelatedProduct GetRelatedProductFromReader(IDataReader dataReader)
        {
            DBRelatedProduct relatedProduct = new DBRelatedProduct();
            relatedProduct.RelatedProductID = NopSqlDataHelper.GetInt(dataReader, "RelatedProductID");
            relatedProduct.ProductID1 = NopSqlDataHelper.GetInt(dataReader, "ProductID1");
            relatedProduct.ProductID2 = NopSqlDataHelper.GetInt(dataReader, "ProductID2");
            relatedProduct.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            return relatedProduct;
        }

        private DBPricelist GetPricelistFromReader(IDataReader dataReader)
        {
            DBPricelist newPricelist = new DBPricelist();

            newPricelist.PricelistID = NopSqlDataHelper.GetInt(dataReader, "PricelistID");
            newPricelist.ExportModeID = NopSqlDataHelper.GetInt(dataReader, "ExportModeID");
            newPricelist.ExportTypeID = NopSqlDataHelper.GetInt(dataReader, "ExportTypeID");
            newPricelist.AffiliateID = NopSqlDataHelper.GetInt(dataReader, "AffiliateID");
            newPricelist.DisplayName = NopSqlDataHelper.GetString(dataReader, "DisplayName");
            newPricelist.ShortName = NopSqlDataHelper.GetString(dataReader, "ShortName");
            newPricelist.PricelistGuid = NopSqlDataHelper.GetString(dataReader, "PricelistGuid");
            newPricelist.CacheTime = NopSqlDataHelper.GetInt(dataReader, "CacheTime");
            newPricelist.FormatLocalization = NopSqlDataHelper.GetString(dataReader, "FormatLocalization");
            newPricelist.Description = NopSqlDataHelper.GetString(dataReader, "Description");
            newPricelist.AdminNotes = NopSqlDataHelper.GetString(dataReader, "AdminNotes");
            newPricelist.Header = NopSqlDataHelper.GetString(dataReader, "Header");
            newPricelist.Body = NopSqlDataHelper.GetString(dataReader, "Body");
            newPricelist.Footer = NopSqlDataHelper.GetString(dataReader, "Footer");
            newPricelist.PriceAdjustment = NopSqlDataHelper.GetDecimal(dataReader, "PriceAdjustment");
            newPricelist.PriceAdjustmentTypeID = NopSqlDataHelper.GetInt(dataReader, "PriceAdjustmentTypeID");
            newPricelist.OverrideIndivAdjustment = NopSqlDataHelper.GetBoolean(dataReader, "OverrideIndivAdjustment");
            newPricelist.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            newPricelist.UpdatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "UpdatedOn");

            return newPricelist;
        }

        private DBProductVariantPricelist GetProductVariantPricelistFromReader(IDataReader dataReader)
        {
            DBProductVariantPricelist newProductVariantPricelist = new DBProductVariantPricelist();

            newProductVariantPricelist.ProductVariantPricelistID = NopSqlDataHelper.GetInt(dataReader, "ProductVariantPricelistID");
            newProductVariantPricelist.ProductVariantID = NopSqlDataHelper.GetInt(dataReader, "ProductVariantID");
            newProductVariantPricelist.PricelistID = NopSqlDataHelper.GetInt(dataReader, "PricelistID");
            newProductVariantPricelist.PriceAdjustmentTypeID = NopSqlDataHelper.GetInt(dataReader, "PriceAdjustmentTypeID");
            newProductVariantPricelist.PriceAdjustment = NopSqlDataHelper.GetDecimal(dataReader, "PriceAdjustment");
            newProductVariantPricelist.UpdatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "UpdatedOn");

            return newProductVariantPricelist;
        }

        private DBTierPrice GetTierPriceFromReader(IDataReader dataReader)
        {
            DBTierPrice newTierPrice = new DBTierPrice();

            newTierPrice.TierPriceID = NopSqlDataHelper.GetInt(dataReader, "TierPriceID");
            newTierPrice.ProductVariantID = NopSqlDataHelper.GetInt(dataReader, "ProductVariantID");
            newTierPrice.Quantity = NopSqlDataHelper.GetInt(dataReader, "Quantity");
            newTierPrice.Price = NopSqlDataHelper.GetDecimal(dataReader, "Price");

            return newTierPrice;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the provider with the property values specified in the application's configuration file. This method is not intended to be used directly from your code
        /// </summary>
        /// <param name="name">The name of the provider instance to initialize</param>
        /// <param name="config">A NameValueCollection that contains the names and values of configuration options for the provider.</param>
        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            base.Initialize(name, config);

            string connectionStringName = config["connectionStringName"];
            if (String.IsNullOrEmpty(connectionStringName))
                throw new ProviderException("Connection name not specified");
            this._sqlConnectionString = NopSqlDataHelper.GetConnectionString(connectionStringName);
            if ((this._sqlConnectionString == null) || (this._sqlConnectionString.Length < 1))
            {
                throw new ProviderException(string.Format("Connection string not found. {0}", connectionStringName));
            }
            config.Remove("connectionStringName");

            if (config.Count > 0)
            {
                string key = config.GetKey(0);
                if (!string.IsNullOrEmpty(key))
                {
                    throw new ProviderException(string.Format("Provider unrecognized attribute. {0}", new object[] { key }));
                }
            }
        }

        /// <summary>
        /// Gets all products
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product collection</returns>
        public override DBProductCollection GetAllProducts(bool showHidden)
        {
            DBProductCollection productCollection = new DBProductCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductLoadAll");
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBProduct product = GetProductFromReader(dataReader);
                    productCollection.Add(product);
                }
            }

            return productCollection;
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
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="TotalRecords">Total records</param>
        /// <returns>Product collection</returns>
        public override DBProductCollection GetAllProducts(int CategoryID, int ManufacturerID,
            bool? FeaturedProducts, decimal? PriceMin, decimal? PriceMax, string Keywords,
            bool SearchDescriptions, int PageSize, int PageIndex,
            List<int> FilteredSpecs, bool showHidden, int SortBy, bool SortTo, out int TotalRecords)
        {
            TotalRecords = 0;
            DBProductCollection productCollection = new DBProductCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductLoadAllPaged");
            db.AddInParameter(dbCommand, "CategoryID", DbType.Int32, CategoryID);
            db.AddInParameter(dbCommand, "ManufacturerID", DbType.Int32, ManufacturerID);
            if (FeaturedProducts.HasValue)
                db.AddInParameter(dbCommand, "FeaturedProducts", DbType.Boolean, FeaturedProducts.Value);
            else
                db.AddInParameter(dbCommand, "FeaturedProducts", DbType.Boolean, null);
            if (PriceMin.HasValue)
                db.AddInParameter(dbCommand, "PriceMin", DbType.Decimal, PriceMin.Value);
            else
                db.AddInParameter(dbCommand, "PriceMin", DbType.Decimal, null);
            if (PriceMax.HasValue)
                db.AddInParameter(dbCommand, "PriceMax", DbType.Decimal, PriceMax.Value);
            else
                db.AddInParameter(dbCommand, "PriceMax", DbType.Decimal, null);
            db.AddInParameter(dbCommand, "Keywords", DbType.String, Keywords);
            db.AddInParameter(dbCommand, "SearchDescriptions", DbType.Boolean, SearchDescriptions);
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, PageSize);
            db.AddInParameter(dbCommand, "PageIndex", DbType.Int32, PageIndex);
            db.AddInParameter(dbCommand, "SortBy", DbType.Int32, SortBy);
            db.AddInParameter(dbCommand, "SortTo", DbType.Boolean, SortTo);

            string commaSeparatedSpecIDs = string.Empty;
            if (FilteredSpecs != null)
            {
                FilteredSpecs.Sort();
                for (int i = 0; i < FilteredSpecs.Count; i++)
                {
                    commaSeparatedSpecIDs += FilteredSpecs[i].ToString();
                    if (i != FilteredSpecs.Count - 1)
                    {
                        commaSeparatedSpecIDs += ",";
                    }
                }
            }
            db.AddInParameter(dbCommand, "FilteredSpecs", DbType.String, commaSeparatedSpecIDs);
            db.AddOutParameter(dbCommand, "TotalRecords", DbType.Int32, 0);

            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBProduct product = GetProductFromReader(dataReader);
                    productCollection.Add(product);
                }
            }
            TotalRecords = Convert.ToInt32(db.GetParameterValue(dbCommand, "@TotalRecords"));

            return productCollection;
        }

        /// <summary>
        /// Gets all products displayed on the home page
        /// </summary>
        /// <returns>Product collection</returns>
        public override DBProductCollection GetAllProductsDisplayedOnHomePage(bool showHidden)
        {
            DBProductCollection productCollection = new DBProductCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductLoadDisplayedOnHomePage");
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBProduct product = GetProductFromReader(dataReader);
                    productCollection.Add(product);
                }
            }

            return productCollection;
        }

        /// <summary>
        /// Gets product
        /// </summary>
        /// <param name="ProductID">Product identifier</param>
        /// <returns>Product</returns>
        public override DBProduct GetProductByID(int ProductID)
        {
            DBProduct product = null;
            if (ProductID == 0)
                return product;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, ProductID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    product = GetProductFromReader(dataReader);
                }
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
        public override DBProduct InsertProduct(string Name, string ShortDescription,
            string FullDescription, string AdminComment,
            int ProductTypeID, int TemplateID, bool ShowOnHomePage,
            string MetaKeywords, string MetaDescription, string MetaTitle,
            string SEName, bool AllowCustomerReviews, bool AllowCustomerRatings,
            int RatingSum, int TotalRatingVotes, bool Published,
            bool Deleted, DateTime CreatedOn, DateTime UpdatedOn)
        {
            DBProduct product = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductInsert");
            db.AddOutParameter(dbCommand, "ProductID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "ShortDescription", DbType.String, ShortDescription);
            db.AddInParameter(dbCommand, "FullDescription", DbType.String, FullDescription);
            db.AddInParameter(dbCommand, "AdminComment", DbType.String, AdminComment);
            db.AddInParameter(dbCommand, "ProductTypeID", DbType.Int32, ProductTypeID);
            db.AddInParameter(dbCommand, "TemplateID", DbType.Int32, TemplateID);
            db.AddInParameter(dbCommand, "ShowOnHomePage", DbType.Boolean, ShowOnHomePage);
            db.AddInParameter(dbCommand, "MetaKeywords", DbType.String, MetaKeywords);
            db.AddInParameter(dbCommand, "MetaDescription", DbType.String, MetaDescription);
            db.AddInParameter(dbCommand, "MetaTitle", DbType.String, MetaTitle);
            db.AddInParameter(dbCommand, "SEName", DbType.String, SEName);
            db.AddInParameter(dbCommand, "AllowCustomerReviews", DbType.Boolean, AllowCustomerReviews);
            db.AddInParameter(dbCommand, "AllowCustomerRatings", DbType.Boolean, AllowCustomerRatings);
            db.AddInParameter(dbCommand, "RatingSum", DbType.Int32, RatingSum);
            db.AddInParameter(dbCommand, "TotalRatingVotes", DbType.Int32, TotalRatingVotes);
            db.AddInParameter(dbCommand, "Published", DbType.Boolean, Published);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, Deleted);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, UpdatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int ProductID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ProductID"));
                product = GetProductByID(ProductID);
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
        public override DBProduct UpdateProduct(int ProductID, string Name, string ShortDescription,
            string FullDescription, string AdminComment, int ProductTypeID,
            int TemplateID, bool ShowOnHomePage, string MetaKeywords,
            string MetaDescription, string MetaTitle,
            string SEName, bool AllowCustomerReviews, bool AllowCustomerRatings,
            int RatingSum, int TotalRatingVotes, bool Published,
            bool Deleted, DateTime CreatedOn, DateTime UpdatedOn)
        {
            DBProduct product = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductUpdate");
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, ProductID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "ShortDescription", DbType.String, ShortDescription);
            db.AddInParameter(dbCommand, "FullDescription", DbType.String, FullDescription);
            db.AddInParameter(dbCommand, "AdminComment", DbType.String, AdminComment);
            db.AddInParameter(dbCommand, "ProductTypeID", DbType.Int32, ProductTypeID);
            db.AddInParameter(dbCommand, "TemplateID", DbType.Int32, TemplateID);
            db.AddInParameter(dbCommand, "ShowOnHomePage", DbType.Boolean, ShowOnHomePage);
            db.AddInParameter(dbCommand, "MetaKeywords", DbType.String, MetaKeywords);
            db.AddInParameter(dbCommand, "MetaDescription", DbType.String, MetaDescription);
            db.AddInParameter(dbCommand, "MetaTitle", DbType.String, MetaTitle);
            db.AddInParameter(dbCommand, "SEName", DbType.String, SEName);
            db.AddInParameter(dbCommand, "AllowCustomerReviews", DbType.Boolean, AllowCustomerReviews);
            db.AddInParameter(dbCommand, "AllowCustomerRatings", DbType.Boolean, AllowCustomerRatings);
            db.AddInParameter(dbCommand, "RatingSum", DbType.Int32, RatingSum);
            db.AddInParameter(dbCommand, "TotalRatingVotes", DbType.Int32, TotalRatingVotes);
            db.AddInParameter(dbCommand, "Published", DbType.Boolean, Published);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, Deleted);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, UpdatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                product = GetProductByID(ProductID);

            return product;
        }

        /// <summary>
        /// Gets a list of products purchased by other customers who purchased the above
        /// </summary>
        /// <param name="ProductID">Product identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="PageSize">Page size</param>
        /// <param name="PageIndex">Page index</param>
        /// <param name="TotalRecords">Total records</param>
        /// <returns>Product collection</returns>
        public override DBProductCollection GetProductsAlsoPurchasedByID(int ProductID,
            bool showHidden, int PageSize, int PageIndex, out int TotalRecords)
        {
            TotalRecords = 0;
            DBProductCollection productCollection = new DBProductCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductAlsoPurchasedLoadByProductID");
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, ProductID);
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, PageSize);
            db.AddInParameter(dbCommand, "PageIndex", DbType.Int32, PageIndex);
            db.AddOutParameter(dbCommand, "TotalRecords", DbType.Int32, 0);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBProduct product = GetProductFromReader(dataReader);
                    productCollection.Add(product);
                }
            }
            TotalRecords = Convert.ToInt32(db.GetParameterValue(dbCommand, "@TotalRecords"));

            return productCollection;
        }

        /// <summary>
        /// Sets a product rating
        /// </summary>
        /// <param name="ProductID">Product identifer</param>
        /// <param name="CustomerID">Customer identifer</param>
        /// <param name="Rating">Rating</param>
        /// <param name="RatedOn">Rating was created on</param>
        public override void SetProductRating(int ProductID, int CustomerID, int Rating, DateTime RatedOn)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductRatingCreate");
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, ProductID);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, CustomerID);
            db.AddInParameter(dbCommand, "Rating", DbType.Int32, Rating);
            db.AddInParameter(dbCommand, "RatedOn", DbType.DateTime, RatedOn);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a recently added products list
        /// </summary>
        /// <param name="Number">Number of products to load</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Recently added products list</returns>
        public override DBProductCollection GetRecentlyAddedProducts(int Number, bool showHidden)
        {
            DBProductCollection productCollection = new DBProductCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductLoadRecentlyAdded");
            db.AddInParameter(dbCommand, "Number", DbType.Int32, Number);
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBProduct product = GetProductFromReader(dataReader);
                    productCollection.Add(product);
                }
            }

            return productCollection;
        }

        /// <summary>
        /// Deletes a product picture mapping
        /// </summary>
        /// <param name="ProductPictureID">Product picture mapping identifier</param>
        public override void DeleteProductPicture(int ProductPictureID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductPictureDelete");
            db.AddInParameter(dbCommand, "ProductPictureID", DbType.Int32, ProductPictureID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a product picture mapping
        /// </summary>
        /// <param name="ProductPictureID">Product picture mapping identifier</param>
        /// <returns>Product picture mapping</returns>
        public override DBProductPicture GetProductPictureByID(int ProductPictureID)
        {
            DBProductPicture productPicture = null;
            if (ProductPictureID == 0)
                return productPicture;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductPictureLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ProductPictureID", DbType.Int32, ProductPictureID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    productPicture = GetProductPictureFromReader(dataReader);
                }
            }
            return productPicture;
        }

        /// <summary>
        /// Inserts a product picture mapping
        /// </summary>
        /// <param name="ProductID">Product identifier</param>
        /// <param name="PictureID">Picture identifier</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Product picture mapping</returns>
        public override DBProductPicture InsertProductPicture(int ProductID,
          int PictureID, int DisplayOrder)
        {
            DBProductPicture productPicture = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductPictureInsert");
            db.AddOutParameter(dbCommand, "ProductPictureID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, ProductID);
            db.AddInParameter(dbCommand, "PictureID", DbType.Int32, PictureID);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int ProductPictureID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ProductPictureID"));
                productPicture = GetProductPictureByID(ProductPictureID);
            }
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
        public override DBProductPicture UpdateProductPicture(int ProductPictureID, int ProductID,
            int PictureID, int DisplayOrder)
        {
            DBProductPicture productPicture = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductPictureUpdate");
            db.AddInParameter(dbCommand, "ProductPictureID", DbType.Int32, ProductPictureID);
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, ProductID);
            db.AddInParameter(dbCommand, "PictureID", DbType.Int32, PictureID);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                productPicture = GetProductPictureByID(ProductPictureID);

            return productPicture;
        }

        /// <summary>
        /// Gets all product picture mappings by product identifier
        /// </summary>
        /// <param name="ProductID">Product identifier</param>
        /// <returns>Product picture mapping collection</returns>
        public override DBProductPictureCollection GetProductPicturesByProductID(int ProductID)
        {
            DBProductPictureCollection productPictureCollection = new DBProductPictureCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductPictureLoadAllByProductID");
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, ProductID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBProductPicture productPicture = GetProductPictureFromReader(dataReader);
                    productPictureCollection.Add(productPicture);
                }
            }

            return productPictureCollection;
        }

        /// <summary>
        /// Gets a product review
        /// </summary>
        /// <param name="ProductReviewID">Product review identifier</param>
        /// <returns>Product review</returns>
        public override DBProductReview GetProductReviewByID(int ProductReviewID)
        {
            DBProductReview productReview = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductReviewLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ProductReviewID", DbType.Int32, ProductReviewID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    productReview = GetProductReviewFromReader(dataReader);
                }
            }
            return productReview;
        }

        /// <summary>
        /// Gets a product review collection by product identifier
        /// </summary>
        /// <param name="ProductID">Product identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product review collection</returns>
        public override DBProductReviewCollection GetProductReviewByProductID(int ProductID, bool showHidden)
        {
            DBProductReviewCollection productReviewCollection = new DBProductReviewCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductReviewLoadByProductID");
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, ProductID);
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBProductReview productReview = GetProductReviewFromReader(dataReader);
                    productReviewCollection.Add(productReview);
                }
            }
            return productReviewCollection;
        }

        /// <summary>
        /// Deletes a product review
        /// </summary>
        /// <param name="ProductReviewID">Product review identifier</param>
        public override void DeleteProductReview(int ProductReviewID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductReviewDelete");
            db.AddInParameter(dbCommand, "ProductReviewID", DbType.Int32, ProductReviewID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets all product reviews
        /// </summary>
        /// <returns>Product review collection</returns>
        public override DBProductReviewCollection GetAllProductReviews(bool showHidden)
        {
            DBProductReviewCollection productReviewCollection = new DBProductReviewCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductReviewLoadAll");
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBProductReview productReview = GetProductReviewFromReader(dataReader);
                    productReviewCollection.Add(productReview);
                }
            }

            return productReviewCollection;
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
        public override DBProductReview InsertProductReview(int ProductID, int CustomerID, string Title,
            string ReviewText, int Rating, int HelpfulYesTotal,
            int HelpfulNoTotal, bool IsApproved, DateTime CreatedOn)
        {
            DBProductReview productReview = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductReviewInsert");
            db.AddOutParameter(dbCommand, "ProductReviewID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, ProductID);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, CustomerID);
            db.AddInParameter(dbCommand, "Title", DbType.String, Title);
            db.AddInParameter(dbCommand, "ReviewText", DbType.String, ReviewText);
            db.AddInParameter(dbCommand, "Rating", DbType.Int32, Rating);
            db.AddInParameter(dbCommand, "HelpfulYesTotal", DbType.Int32, HelpfulYesTotal);
            db.AddInParameter(dbCommand, "HelpfulNoTotal", DbType.Int32, HelpfulNoTotal);
            db.AddInParameter(dbCommand, "IsApproved", DbType.Boolean, IsApproved);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int ProductReviewID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ProductReviewID"));
                productReview = GetProductReviewByID(ProductReviewID);
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
        public override DBProductReview UpdateProductReview(int ProductReviewID, int ProductID, int CustomerID, string Title,
            string ReviewText, int Rating, int HelpfulYesTotal,
            int HelpfulNoTotal, bool IsApproved, DateTime CreatedOn)
        {
            DBProductReview productReview = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductReviewUpdate");
            db.AddInParameter(dbCommand, "ProductReviewID", DbType.Int32, ProductReviewID);
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, ProductID);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, CustomerID);
            db.AddInParameter(dbCommand, "Title", DbType.String, Title);
            db.AddInParameter(dbCommand, "ReviewText", DbType.String, ReviewText);
            db.AddInParameter(dbCommand, "Rating", DbType.Int32, Rating);
            db.AddInParameter(dbCommand, "HelpfulYesTotal", DbType.Int32, HelpfulYesTotal);
            db.AddInParameter(dbCommand, "HelpfulNoTotal", DbType.Int32, HelpfulNoTotal);
            db.AddInParameter(dbCommand, "IsApproved", DbType.Boolean, IsApproved);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);

            if (db.ExecuteNonQuery(dbCommand) > 0)
                productReview = GetProductReviewByID(ProductReviewID);

            return productReview;
        }

        /// <summary>
        /// Sets a product rating helpfulness
        /// </summary>
        /// <param name="ProductReviewID">Product review identifer</param>
        /// <param name="CustomerID">Customer identifer</param>
        /// <param name="WasHelpful">A value indicating whether the product review was helpful or not </param>
        public override void SetProductRatingHelpfulness(int ProductReviewID,
            int CustomerID, bool WasHelpful)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductReviewHelpfulnessCreate");
            db.AddInParameter(dbCommand, "ProductReviewID", DbType.Int32, ProductReviewID);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, CustomerID);
            db.AddInParameter(dbCommand, "WasHelpful", DbType.Boolean, WasHelpful);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a product variant
        /// </summary>
        /// <param name="ProductVariantID">Product variant identifier</param>
        /// <returns>Product variant</returns>
        public override DBProductVariant GetProductVariantByID(int ProductVariantID)
        {
            DBProductVariant productVariant = null;
            if (ProductVariantID == 0)
                return productVariant;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, ProductVariantID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    productVariant = GetProductVariantFromReader(dataReader);
                }
            }
            return productVariant;
        }

        /// <summary>
        /// Get low stock product variants
        /// </summary>
        /// <returns>Result</returns>
        public override DBProductVariantCollection GetLowStockProductVariants()
        {
            DBProductVariantCollection productVariantCollection = new DBProductVariantCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantLoadLowStock");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBProductVariant productVariant = GetProductVariantFromReader(dataReader);
                    productVariantCollection.Add(productVariant);
                }
            }

            return productVariantCollection;
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
        public override DBProductVariant InsertProductVariant(int ProductID, string Name, string SKU,
            string Description, string AdminComment, string ManufacturerPartNumber, bool IsDownload,
            int DownloadID, bool UnlimitedDownloads, int MaxNumberOfDownloads,
            bool HasSampleDownload, int SampleDownloadID, bool IsShipEnabled, bool IsFreeShipping,
            decimal AdditionalShippingCharge, bool IsTaxExempt, int TaxCategoryID,
            bool ManageInventory, int StockQuantity, int MinStockQuantity, int LowStockActivityID,
            int NotifyAdminForQuantityBelow, int OrderMinimumQuantity, int OrderMaximumQuantity,
            int WarehouseId, bool DisableBuyButton, decimal Price, decimal OldPrice,
            decimal Weight, decimal Length, decimal Width, decimal Height, int PictureID,
            DateTime? AvailableStartDateTime, DateTime? AvailableEndDateTime,
            bool Published, bool Deleted, int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn)
        {
            if (AvailableStartDateTime.HasValue)
            {
                if (AvailableStartDateTime.Value.Year < 1900)
                    AvailableStartDateTime = new DateTime(1900, AvailableStartDateTime.Value.Month, AvailableStartDateTime.Value.Day);
                if (AvailableStartDateTime.Value.Year > 2999)
                    AvailableStartDateTime = new DateTime(2999, AvailableStartDateTime.Value.Month, AvailableStartDateTime.Value.Day);
            }
            if (AvailableEndDateTime.HasValue)
            {
                if (AvailableEndDateTime.Value.Year < 1900)
                    AvailableEndDateTime = new DateTime(1900, AvailableEndDateTime.Value.Month, AvailableEndDateTime.Value.Day);
                if (AvailableEndDateTime.Value.Year > 2999)
                    AvailableEndDateTime = new DateTime(2999, AvailableEndDateTime.Value.Month, AvailableEndDateTime.Value.Day);
            }

            DBProductVariant productVariant = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantInsert");
            db.AddOutParameter(dbCommand, "ProductVariantID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, ProductID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "SKU", DbType.String, SKU);
            db.AddInParameter(dbCommand, "Description", DbType.String, Description);
            db.AddInParameter(dbCommand, "AdminComment", DbType.String, AdminComment);
            db.AddInParameter(dbCommand, "ManufacturerPartNumber", DbType.String, ManufacturerPartNumber);
            db.AddInParameter(dbCommand, "IsDownload", DbType.Boolean, IsDownload);
            db.AddInParameter(dbCommand, "DownloadID", DbType.Int32, DownloadID);
            db.AddInParameter(dbCommand, "UnlimitedDownloads", DbType.Boolean, UnlimitedDownloads);
            db.AddInParameter(dbCommand, "MaxNumberOfDownloads", DbType.Int32, MaxNumberOfDownloads);
            db.AddInParameter(dbCommand, "HasSampleDownload", DbType.Boolean, HasSampleDownload);
            db.AddInParameter(dbCommand, "SampleDownloadID", DbType.Int32, SampleDownloadID);
            db.AddInParameter(dbCommand, "IsShipEnabled", DbType.Boolean, IsShipEnabled);
            db.AddInParameter(dbCommand, "IsFreeShipping", DbType.Boolean, IsFreeShipping);
            db.AddInParameter(dbCommand, "AdditionalShippingCharge", DbType.Decimal, AdditionalShippingCharge);
            db.AddInParameter(dbCommand, "IsTaxExempt", DbType.Boolean, IsTaxExempt);
            db.AddInParameter(dbCommand, "TaxCategoryID", DbType.Int32, TaxCategoryID);
            db.AddInParameter(dbCommand, "ManageInventory", DbType.Boolean, ManageInventory);
            db.AddInParameter(dbCommand, "StockQuantity", DbType.Int32, StockQuantity);
            db.AddInParameter(dbCommand, "MinStockQuantity", DbType.Int32, MinStockQuantity);
            db.AddInParameter(dbCommand, "LowStockActivityID", DbType.Int32, LowStockActivityID);
            db.AddInParameter(dbCommand, "NotifyAdminForQuantityBelow", DbType.Int32, NotifyAdminForQuantityBelow);
            db.AddInParameter(dbCommand, "OrderMinimumQuantity", DbType.Int32, OrderMinimumQuantity);
            db.AddInParameter(dbCommand, "OrderMaximumQuantity", DbType.Int32, OrderMaximumQuantity);
            db.AddInParameter(dbCommand, "WarehouseId", DbType.Int32, WarehouseId);
            db.AddInParameter(dbCommand, "DisableBuyButton", DbType.Boolean, DisableBuyButton);
            db.AddInParameter(dbCommand, "Price", DbType.Decimal, Price);
            db.AddInParameter(dbCommand, "OldPrice", DbType.Decimal, OldPrice);
            db.AddInParameter(dbCommand, "Weight", DbType.Decimal, Weight);
            db.AddInParameter(dbCommand, "Length", DbType.Decimal, Length);
            db.AddInParameter(dbCommand, "Width", DbType.Decimal, Width);
            db.AddInParameter(dbCommand, "Height", DbType.Decimal, Height);
            db.AddInParameter(dbCommand, "PictureID", DbType.Int32, PictureID);
            if (AvailableStartDateTime.HasValue)
                db.AddInParameter(dbCommand, "AvailableStartDateTime", DbType.DateTime, AvailableStartDateTime.Value);
            else
                db.AddInParameter(dbCommand, "AvailableStartDateTime", DbType.DateTime, DBNull.Value);
            if (AvailableEndDateTime.HasValue)
                db.AddInParameter(dbCommand, "AvailableEndDateTime", DbType.DateTime, AvailableEndDateTime.Value);
            else
                db.AddInParameter(dbCommand, "AvailableEndDateTime", DbType.DateTime, DBNull.Value);
            db.AddInParameter(dbCommand, "Published", DbType.Boolean, Published);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, Deleted);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, UpdatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int ProductVariantID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ProductVariantID"));
                productVariant = GetProductVariantByID(ProductVariantID);
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
        public override DBProductVariant UpdateProductVariant(int ProductVariantID, int ProductID,
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
           bool Published, bool Deleted, int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn)
        {
            if (AvailableStartDateTime.HasValue)
            {
                if (AvailableStartDateTime.Value.Year < 1900)
                    AvailableStartDateTime = new DateTime(1900, AvailableStartDateTime.Value.Month, AvailableStartDateTime.Value.Day);
                if (AvailableStartDateTime.Value.Year > 2999)
                    AvailableStartDateTime = new DateTime(2998, AvailableStartDateTime.Value.Month, AvailableStartDateTime.Value.Day);
            }
            if (AvailableEndDateTime.HasValue)
            {
                if (AvailableEndDateTime.Value.Year < 1900)
                    AvailableEndDateTime = new DateTime(1900, AvailableEndDateTime.Value.Month, AvailableEndDateTime.Value.Day);
                if (AvailableEndDateTime.Value.Year > 2999)
                    AvailableEndDateTime = new DateTime(2998, AvailableEndDateTime.Value.Month, AvailableEndDateTime.Value.Day);
            }

            DBProductVariant productVariant = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantUpdate");
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, ProductVariantID);
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, ProductID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "SKU", DbType.String, SKU);
            db.AddInParameter(dbCommand, "Description", DbType.String, Description);
            db.AddInParameter(dbCommand, "AdminComment", DbType.String, AdminComment);
            db.AddInParameter(dbCommand, "ManufacturerPartNumber", DbType.String, ManufacturerPartNumber);
            db.AddInParameter(dbCommand, "IsDownload", DbType.Boolean, IsDownload);
            db.AddInParameter(dbCommand, "DownloadID", DbType.Int32, DownloadID);
            db.AddInParameter(dbCommand, "UnlimitedDownloads", DbType.Boolean, UnlimitedDownloads);
            db.AddInParameter(dbCommand, "MaxNumberOfDownloads", DbType.Int32, MaxNumberOfDownloads);
            db.AddInParameter(dbCommand, "HasSampleDownload", DbType.Boolean, HasSampleDownload);
            db.AddInParameter(dbCommand, "SampleDownloadID", DbType.Int32, SampleDownloadID);
            db.AddInParameter(dbCommand, "IsShipEnabled", DbType.Boolean, IsShipEnabled);
            db.AddInParameter(dbCommand, "IsFreeShipping", DbType.Boolean, IsFreeShipping);
            db.AddInParameter(dbCommand, "AdditionalShippingCharge", DbType.Decimal, AdditionalShippingCharge);
            db.AddInParameter(dbCommand, "IsTaxExempt", DbType.Boolean, IsTaxExempt);
            db.AddInParameter(dbCommand, "TaxCategoryID", DbType.Int32, TaxCategoryID);
            db.AddInParameter(dbCommand, "ManageInventory", DbType.Boolean, ManageInventory);
            db.AddInParameter(dbCommand, "StockQuantity", DbType.Int32, StockQuantity);
            db.AddInParameter(dbCommand, "MinStockQuantity", DbType.Int32, MinStockQuantity);
            db.AddInParameter(dbCommand, "LowStockActivityID", DbType.Int32, LowStockActivityID);
            db.AddInParameter(dbCommand, "NotifyAdminForQuantityBelow", DbType.Int32, NotifyAdminForQuantityBelow);
            db.AddInParameter(dbCommand, "OrderMinimumQuantity", DbType.Int32, OrderMinimumQuantity);
            db.AddInParameter(dbCommand, "OrderMaximumQuantity", DbType.Int32, OrderMaximumQuantity);
            db.AddInParameter(dbCommand, "WarehouseId", DbType.Int32, WarehouseId);
            db.AddInParameter(dbCommand, "DisableBuyButton", DbType.Boolean, DisableBuyButton);
            db.AddInParameter(dbCommand, "Price", DbType.Decimal, Price);
            db.AddInParameter(dbCommand, "OldPrice", DbType.Decimal, OldPrice);
            db.AddInParameter(dbCommand, "Weight", DbType.Decimal, Weight);
            db.AddInParameter(dbCommand, "Length", DbType.Decimal, Length);
            db.AddInParameter(dbCommand, "Width", DbType.Decimal, Width);
            db.AddInParameter(dbCommand, "Height", DbType.Decimal, Height);
            db.AddInParameter(dbCommand, "PictureID", DbType.Int32, PictureID);
            if (AvailableStartDateTime.HasValue)
                db.AddInParameter(dbCommand, "AvailableStartDateTime", DbType.DateTime, AvailableStartDateTime.Value);
            else
                db.AddInParameter(dbCommand, "AvailableStartDateTime", DbType.DateTime, DBNull.Value);
            if (AvailableEndDateTime.HasValue)
                db.AddInParameter(dbCommand, "AvailableEndDateTime", DbType.DateTime, AvailableEndDateTime.Value);
            else
                db.AddInParameter(dbCommand, "AvailableEndDateTime", DbType.DateTime, DBNull.Value);
            db.AddInParameter(dbCommand, "Published", DbType.Boolean, Published);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, Deleted);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, UpdatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                productVariant = GetProductVariantByID(ProductVariantID);

            return productVariant;
        }

        /// <summary>
        /// Gets product variants by product identifier
        /// </summary>
        /// <param name="ProductID">The product identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product variant collection</returns>
        public override DBProductVariantCollection GetProductVariantsByProductID(int ProductID, bool showHidden)
        {
            DBProductVariantCollection productVariantCollection = new DBProductVariantCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantLoadByProductID");
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, ProductID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBProductVariant productVariant = GetProductVariantFromReader(dataReader);
                    productVariantCollection.Add(productVariant);
                }
            }

            return productVariantCollection;
        }

        /// <summary>
        /// Deletes a related product
        /// </summary>
        /// <param name="RelatedProductID">Related product identifer</param>
        public override void DeleteRelatedProduct(int RelatedProductID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_RelatedProductDelete");
            db.AddInParameter(dbCommand, "RelatedProductID", DbType.Int32, RelatedProductID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a related product collection by product identifier
        /// </summary>
        /// <param name="ProductID1">The first product identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Related product collection</returns>
        public override DBRelatedProductCollection GetRelatedProductsByProductID1(int ProductID1, bool showHidden)
        {
            DBRelatedProductCollection relatedProductCollection = new DBRelatedProductCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_RelatedProductLoadByProductID1");
            db.AddInParameter(dbCommand, "ProductID1", DbType.Int32, ProductID1);
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBRelatedProduct relatedProduct = GetRelatedProductFromReader(dataReader);
                    relatedProductCollection.Add(relatedProduct);
                }
            }

            return relatedProductCollection;
        }

        /// <summary>
        /// Gets a related product
        /// </summary>
        /// <param name="RelatedProductID">Related product identifer</param>
        /// <returns></returns>
        public override DBRelatedProduct GetRelatedProductByID(int RelatedProductID)
        {
            DBRelatedProduct relatedProduct = null;
            if (RelatedProductID == 0)
                return relatedProduct;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_RelatedProductLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "RelatedProductID", DbType.Int32, RelatedProductID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    relatedProduct = GetRelatedProductFromReader(dataReader);
                }
            }
            return relatedProduct;
        }

        /// <summary>
        /// Inserts a related product
        /// </summary>
        /// <param name="ProductID1">The first product identifier</param>
        /// <param name="ProductID2">The second product identifier</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Related product</returns>
        public override DBRelatedProduct InsertRelatedProduct(int ProductID1, int ProductID2, int DisplayOrder)
        {
            DBRelatedProduct relatedProduct = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_RelatedProductInsert");
            db.AddOutParameter(dbCommand, "RelatedProductID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "ProductID1", DbType.Int32, ProductID1);
            db.AddInParameter(dbCommand, "ProductID2", DbType.Int32, ProductID2);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int RelatedProductID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@RelatedProductID"));
                relatedProduct = GetRelatedProductByID(RelatedProductID);
            }
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
        public override DBRelatedProduct UpdateRelatedProduct(int RelatedProductID, int ProductID1, int ProductID2,
            int DisplayOrder)
        {
            DBRelatedProduct relatedProduct = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_RelatedProductUpdate");
            db.AddInParameter(dbCommand, "RelatedProductID", DbType.Int32, RelatedProductID);
            db.AddInParameter(dbCommand, "ProductID1", DbType.Int32, ProductID1);
            db.AddInParameter(dbCommand, "ProductID2", DbType.Int32, ProductID2);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                relatedProduct = GetRelatedProductByID(RelatedProductID);

            return relatedProduct;
        }

        /// <summary>
        /// Gets all product types
        /// </summary>
        /// <returns>Product type collection</returns>
        public override DBProductTypeCollection GetAllProductTypes()
        {
            DBProductTypeCollection productTypeCollection = new DBProductTypeCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductTypeLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBProductType productType = GetProductTypeFromReader(dataReader);
                    productTypeCollection.Add(productType);
                }
            }

            return productTypeCollection;
        }

        /// <summary>
        /// Gets a product type
        /// </summary>
        /// <param name="ProductTypeID">Product type identifier</param>
        /// <returns>Product type</returns>
        public override DBProductType GetProductTypeByID(int ProductTypeID)
        {
            DBProductType productType = null;
            if (ProductTypeID == 0)
                return productType;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductTypeLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ProductTypeID", DbType.Int32, ProductTypeID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    productType = GetProductTypeFromReader(dataReader);
                }
            }
            return productType;
        }

        /// <summary>
        /// Gets all product variants directly assigned to a pricelist
        /// </summary>
        /// <param name="PricelistID"></param>
        /// <returns></returns>
        public override DBProductVariantCollection GetProductVariantsByPricelistID(int PricelistID)
        {
            DBProductVariantCollection productVariantCollection = new DBProductVariantCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantLoadByPricelistID");
            db.AddInParameter(dbCommand, "PricelistID", DbType.Int32, PricelistID);

            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBProductVariant productVariant = GetProductVariantFromReader(dataReader);
                    productVariantCollection.Add(productVariant);
                }
            }

            return productVariantCollection;
        }

        /// <summary>
        /// Gets a collection of all available pricelists
        /// </summary>
        /// <returns>Collection of pricelists</returns>
        public override DBPricelistCollection GetAllPricelists()
        {
            DBPricelistCollection pricelistCollection = new DBPricelistCollection();

            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PricelistLoadAll");

            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBPricelist newPricelist = GetPricelistFromReader(dataReader);
                    pricelistCollection.Add(newPricelist);
                }
            }

            return pricelistCollection;
        }

        /// <summary>
        /// Gets a pricelist
        /// </summary>
        /// <param name="PricelistID">Pricelist identifier</param>
        /// <returns>Pricelist</returns>
        public override DBPricelist GetPricelistByID(int PricelistID)
        {
            DBPricelist pricelist = null;

            if (PricelistID == 0)
                return pricelist;

            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PricelistLoadByPrimaryKey");

            db.AddInParameter(dbCommand, "PricelistID", DbType.Int32, PricelistID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    pricelist = GetPricelistFromReader(dataReader);
                }
            }
            return pricelist;
        }

        /// <summary>
        /// Gets a pricelist
        /// </summary>
        /// <param name="PricelistGUID">Pricelist GUID</param>
        /// <returns>Pricelist</returns>
        public override DBPricelist GetPricelistByGUID(string PricelistGUID)
        {
            DBPricelist pricelist = null;

            if (PricelistGUID.Length == 0)
                return pricelist;

            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PricelistLoadByGuid");

            db.AddInParameter(dbCommand, "PricelistGuid", DbType.String, PricelistGUID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    pricelist = GetPricelistFromReader(dataReader);
                }
            }
            return pricelist;
        }

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
        public override DBPricelist InsertPricelist(int ExportModeID, int ExportTypeID, int? AffiliateID,
            string DisplayName, string ShortName, string PricelistGuid, int CacheTime, string FormatLocalization,
            string Description, string AdminNotes,
            string Header, string Body, string Footer,
            int PriceAdjustmentTypeID, decimal PriceAdjustment, bool OverrideIndivAdjustment,
            DateTime CreatedOn, DateTime UpdatedOn)
        {
            DBPricelist pricelist = null;

            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PricelistInsert");

            db.AddOutParameter(dbCommand, "PricelistID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "ExportModeID", DbType.Int32, ExportModeID);
            db.AddInParameter(dbCommand, "ExportTypeID", DbType.Int32, ExportTypeID);
            db.AddInParameter(dbCommand, "AffiliateID", DbType.Int32, AffiliateID);
            db.AddInParameter(dbCommand, "DisplayName", DbType.String, DisplayName);
            db.AddInParameter(dbCommand, "ShortName", DbType.String, ShortName);
            db.AddInParameter(dbCommand, "PricelistGuid", DbType.String, PricelistGuid);
            db.AddInParameter(dbCommand, "CacheTime", DbType.Int32, CacheTime);
            db.AddInParameter(dbCommand, "FormatLocalization", DbType.String, FormatLocalization);
            db.AddInParameter(dbCommand, "Description", DbType.String, Description);
            db.AddInParameter(dbCommand, "AdminNotes", DbType.String, AdminNotes);
            db.AddInParameter(dbCommand, "Header", DbType.String, Header);
            db.AddInParameter(dbCommand, "Body", DbType.String, Body);
            db.AddInParameter(dbCommand, "Footer", DbType.String, Footer);
            db.AddInParameter(dbCommand, "PriceAdjustmentTypeID", DbType.Int32, PriceAdjustmentTypeID);
            db.AddInParameter(dbCommand, "PriceAdjustment", DbType.Currency, PriceAdjustment);
            db.AddInParameter(dbCommand, "OverrideIndivAdjustment", DbType.Boolean, OverrideIndivAdjustment);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, UpdatedOn);

            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int PricelistID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@PricelistID"));
                pricelist = GetPricelistByID(PricelistID);
            }

            return pricelist;
        }

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
        public override DBPricelist UpdatePricelist(int PricelistID, int ExportModeID, int ExportTypeID, int? AffiliateID,
            string DisplayName, string ShortName, string PricelistGuid, int CacheTime, string FormatLocalization,
            string Description, string AdminNotes,
            string Header, string Body, string Footer,
            int PriceAdjustmentTypeID, decimal PriceAdjustment, bool OverrideIndivAdjustment,
            DateTime CreatedOn, DateTime UpdatedOn)
        {
            DBPricelist pricelist = null;

            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PricelistUpdate");

            db.AddInParameter(dbCommand, "PricelistID", DbType.Int32, PricelistID);
            db.AddInParameter(dbCommand, "ExportModeID", DbType.Int32, ExportModeID);
            db.AddInParameter(dbCommand, "ExportTypeID", DbType.Int32, ExportTypeID);
            db.AddInParameter(dbCommand, "AffiliateID", DbType.Int32, AffiliateID);
            db.AddInParameter(dbCommand, "DisplayName", DbType.String, DisplayName);
            db.AddInParameter(dbCommand, "ShortName", DbType.String, ShortName);
            db.AddInParameter(dbCommand, "PricelistGuid", DbType.String, PricelistGuid);
            db.AddInParameter(dbCommand, "CacheTime", DbType.Int32, CacheTime);
            db.AddInParameter(dbCommand, "FormatLocalization", DbType.String, FormatLocalization);
            db.AddInParameter(dbCommand, "Description", DbType.String, Description);
            db.AddInParameter(dbCommand, "AdminNotes", DbType.String, AdminNotes);
            db.AddInParameter(dbCommand, "Header", DbType.String, Header);
            db.AddInParameter(dbCommand, "Body", DbType.String, Body);
            db.AddInParameter(dbCommand, "Footer", DbType.String, Footer);
            db.AddInParameter(dbCommand, "PriceAdjustmentTypeID", DbType.Int32, PriceAdjustmentTypeID);
            db.AddInParameter(dbCommand, "PriceAdjustment", DbType.Currency, PriceAdjustment);
            db.AddInParameter(dbCommand, "OverrideIndivAdjustment", DbType.Boolean, OverrideIndivAdjustment);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, UpdatedOn);

            if (db.ExecuteNonQuery(dbCommand) > 0)
                pricelist = GetPricelistByID(PricelistID);

            return pricelist;
        }

        /// <summary>
        /// Deletes a Pricelist
        /// </summary>
        /// <param name="PricelistID">The PricelistID of the item to be deleted</param>
        public override void DeletePricelist(int PricelistID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PricelistDelete");
            db.AddInParameter(dbCommand, "PricelistID", DbType.Int32, PricelistID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Deletes a ProductVariantPricelist
        /// </summary>
        /// <param name="ProductVariantPricelistID">ProductVariantPricelist identifier</param>
        public override void DeleteProductVariantPricelist(int ProductVariantPricelistID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariant_Pricelist_MappingDelete");
            db.AddInParameter(dbCommand, "ProductVariantPricelistID", DbType.Int32, ProductVariantPricelistID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a ProductVariantPricelist
        /// </summary>
        /// <param name="ProductVariantPricelistID">ProductVariantPricelist identifier</param>
        /// <returns>ProductVariantPricelist</returns>
        public override DBProductVariantPricelist GetProductVariantPricelistByID(int ProductVariantPricelistID)
        {
            DBProductVariantPricelist productVariantPricelist = null;

            if (ProductVariantPricelistID == 0)
                return productVariantPricelist;

            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariant_Pricelist_MappingLoadByPrimaryKey");

            db.AddInParameter(dbCommand, "ProductVariantPricelistID", DbType.Int32, ProductVariantPricelistID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    productVariantPricelist = GetProductVariantPricelistFromReader(dataReader);
                }
            }
            return productVariantPricelist;
        }

        /// <summary>
        /// Gets ProductVariantPricelist
        /// </summary>
        /// <param name="ProductVariantID">ProductVariant identifier</param>
        /// <param name="PricelistID">Pricelist identifier</param>
        /// <returns>ProductVariantPricelist</returns>
        public override DBProductVariantPricelist GetProductVariantPricelist(int ProductVariantID, int PricelistID)
        {
            DBProductVariantPricelist productVariantPricelist = null;

            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariant_Pricelist_MappingLoadByProductVariantIDAndPricelistID");

            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, ProductVariantID);
            db.AddInParameter(dbCommand, "PricelistID", DbType.Int32, PricelistID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    productVariantPricelist = GetProductVariantPricelistFromReader(dataReader);
                }
            }
            return productVariantPricelist;
        }

        /// <summary>
        /// Inserts a ProductVariantPricelist
        /// </summary>
        /// <param name="ProductVariantID">The product variant identifer</param>
        /// <param name="PricelistID">The pricelist identifier</param>
        /// <param name="PriceAdjustmentTypeID">Price adjustment type identifier</param>
        /// <param name="PriceAdjustment">The price will be adjusted by this amount</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>ProductVariantPricelist</returns>
        public override DBProductVariantPricelist InsertProductVariantPricelist(int ProductVariantID,
            int PricelistID, int PriceAdjustmentTypeID, decimal PriceAdjustment,
            DateTime UpdatedOn)
        {
            DBProductVariantPricelist productVariantPricelist = null;

            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariant_Pricelist_MappingInsert");

            db.AddOutParameter(dbCommand, "ProductVariantPricelistID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, ProductVariantID);
            db.AddInParameter(dbCommand, "PricelistID", DbType.Int32, PricelistID);
            db.AddInParameter(dbCommand, "PriceAdjustmentTypeID", DbType.Int32, PriceAdjustmentTypeID);
            db.AddInParameter(dbCommand, "PriceAdjustment", DbType.Decimal, PriceAdjustment);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, UpdatedOn);

            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int ProductVariantPricelistID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ProductVariantPricelistID"));
                productVariantPricelist = GetProductVariantPricelistByID(ProductVariantPricelistID);
            }

            return productVariantPricelist;
        }

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
        public override DBProductVariantPricelist UpdateProductVariantPricelist(int ProductVariantPricelistID, int ProductVariantID,
            int PricelistID, int PriceAdjustmentTypeID, decimal PriceAdjustment,
            DateTime UpdatedOn)
        {
            DBProductVariantPricelist productVariantPricelist = null;

            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariant_Pricelist_MappingUpdate");

            db.AddInParameter(dbCommand, "ProductVariantPricelistID", DbType.Int32, ProductVariantPricelistID);
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, ProductVariantID);
            db.AddInParameter(dbCommand, "PricelistID", DbType.Int32, PricelistID);
            db.AddInParameter(dbCommand, "PriceAdjustmentTypeID", DbType.Int32, PriceAdjustmentTypeID);
            db.AddInParameter(dbCommand, "PriceAdjustment", DbType.Decimal, PriceAdjustment);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, UpdatedOn);

            if (db.ExecuteNonQuery(dbCommand) > 0)
                productVariantPricelist = GetProductVariantPricelistByID(ProductVariantPricelistID);

            return productVariantPricelist;
        }

        /// <summary>
        /// Gets a tier price
        /// </summary>
        /// <param name="TierPriceID">Tier price identifier</param>
        /// <returns>Tier price</returns>
        public override DBTierPrice GetTierPriceByID(int TierPriceID)
        {
            DBTierPrice tierPrice = null;
            if (TierPriceID == 0)
                return tierPrice;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TierPriceLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "TierPriceID", DbType.Int32, TierPriceID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    tierPrice = GetTierPriceFromReader(dataReader);
                }
            }
            return tierPrice;
        }

        /// <summary>
        /// Gets tier prices by product variant identifier
        /// </summary>
        /// <param name="ProductVariantID">Product variant identifier</param>
        /// <returns>Tier price collection</returns>
        public override DBTierPriceCollection GetTierPricesByProductVariantID(int ProductVariantID)
        {
            DBTierPriceCollection tierPriceCollection = new DBTierPriceCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TierPriceLoadAllByProductVariantID");
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, ProductVariantID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBTierPrice tierPrice = GetTierPriceFromReader(dataReader);
                    tierPriceCollection.Add(tierPrice);
                }
            }

            return tierPriceCollection;
        }

        /// <summary>
        /// Deletes a tier price
        /// </summary>
        /// <param name="TierPriceID">Tier price identifier</param>
        public override void DeleteTierPrice(int TierPriceID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TierPriceDelete");
            db.AddInParameter(dbCommand, "TierPriceID", DbType.Int32, TierPriceID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Inserts a tier price
        /// </summary>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <param name="Quantity">The quantity</param>
        /// <param name="Price">The price</param>
        /// <returns>Tier price</returns>
        public override DBTierPrice InsertTierPrice(int ProductVariantID, int Quantity, decimal Price)
        {
            DBTierPrice tierPrice = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TierPriceInsert");
            db.AddOutParameter(dbCommand, "TierPriceID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, ProductVariantID);
            db.AddInParameter(dbCommand, "Quantity", DbType.Int32, Quantity);
            db.AddInParameter(dbCommand, "Price", DbType.Decimal, Price);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int TierPriceID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@TierPriceID"));
                tierPrice = GetTierPriceByID(TierPriceID);
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
        public override DBTierPrice UpdateTierPrice(int TierPriceID, int ProductVariantID, int Quantity, decimal Price)
        {
            DBTierPrice tierPrice = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TierPriceUpdate");
            db.AddInParameter(dbCommand, "TierPriceID", DbType.Int32, TierPriceID);
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, ProductVariantID);
            db.AddInParameter(dbCommand, "Quantity", DbType.Int32, Quantity);
            db.AddInParameter(dbCommand, "Price", DbType.Decimal, Price);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                tierPrice = GetTierPriceByID(TierPriceID);

            return tierPrice;
        }

        #endregion
    }
}
