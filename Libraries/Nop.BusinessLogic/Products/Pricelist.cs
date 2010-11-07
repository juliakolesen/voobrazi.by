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
using System.IO;
using System.Text;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Categories;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Manufacturers;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Products.Attributes;
using NopSolutions.NopCommerce.BusinessLogic.Promo.Discounts;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.BusinessLogic.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Warehouses;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.BusinessLogic.Products
{
	/// <summary>
	/// This object represents the properties and methods of a Pricelist.
	/// </summary>
    public partial class Pricelist : BaseEntity
    {
        #region Ctor
        /// <summary>
        /// Ctor for Pricelist
        /// </summary>
        public Pricelist()
        {
        }
        #endregion

        #region Utilities

        private string createPricelistContents()
        {
            string strContents = "";

            ProductVariantCollection productVariants = new ProductVariantCollection();
            bool blnOverrideAdjustment = this.OverrideIndivAdjustment;

            switch (this.ExportMode)
            {
                case PriceListExportModeEnum.All:
                    {
                        blnOverrideAdjustment = true;
                        foreach (Product product in ProductManager.GetAllProducts(false))
                        {
                            productVariants.AddRange(ProductManager.GetProductVariantsByProductID(product.ProductID, false));
                        }
                    }
                    break;
                case PriceListExportModeEnum.AssignedProducts:
                    {
                        productVariants = ProductManager.GetProductVariantsByPricelistID(this.PricelistID);
                    }
                    break;
                default:
                    break;
            }

            // create new file
            // write header, if provided
            if (!String.IsNullOrEmpty(this.Header))
            {
                strContents += this.Header;
                if (!this.Header.EndsWith("\n"))
                    strContents += "\n";
            }

            // write body
            foreach (ProductVariant productVariant in productVariants)
            {
                // calculate price adjustments
                decimal newPrice = decimal.Zero;

                // if export mode is all, then calculate price
                if (blnOverrideAdjustment)
                {
                    newPrice = getAdjustedPrice(productVariant.Price, this.PriceAdjustmentType, PriceAdjustment);
                }
                else
                {
                    ProductVariantPricelist productVariantPricelist = ProductManager.GetProductVariantPricelist(productVariant.ProductVariantID, this.PricelistID);
                    if (productVariantPricelist != null)
                    {
                        newPrice = getAdjustedPrice(productVariant.Price, productVariantPricelist.PriceAdjustmentType, productVariantPricelist.PriceAdjustment);
                    }
                }
                strContents += replaceMessageTemplateTokens(productVariant, this.Body,
                    this.FormatLocalization, new System.Collections.Specialized.NameValueCollection(), AffiliateID, newPrice);
                if (!this.Body.EndsWith("\n"))
                    strContents += "\n";
            }

            // write footer, if provided
            if (!String.IsNullOrEmpty(this.Footer))
            {
                strContents += this.Header;
                if (!this.Footer.EndsWith("\n"))
                    strContents += "\n";
            }

            return strContents;
        }

        private bool checkCache(int CacheTime, string CachePath)
        {
            // search for youngest file
            string[] files = System.IO.Directory.GetFiles(CachePath, this.ShortName + "*.txt");

            string youngestFileName = "";
            DateTime youngestFileDate = new DateTime(2000, 01, 01);

            foreach (string file in files)
            {
                if (new System.IO.FileInfo(file).CreationTime > youngestFileDate)
                {
                    youngestFileName = file;
                    youngestFileDate = new System.IO.FileInfo(file).CreationTime;
                }
            }

            if (DateTime.Now.AddMinutes(-CacheTime) > youngestFileDate)
                return false;
            else
                return true;
        }

        private string retrieveFromCache(string CachePath)
        {
            // search for youngest file
            string[] files = System.IO.Directory.GetFiles(CachePath, this.ShortName + "*.txt");

            string youngestFileName = "";
            DateTime youngestFileDate = new DateTime(2000, 01, 01);

            foreach (string file in files)
            {
                if (new System.IO.FileInfo(file).CreationTime > youngestFileDate)
                {
                    youngestFileName = file;
                    youngestFileDate = new System.IO.FileInfo(file).CreationTime;
                }
            }

            using (FileStream fs = new FileStream(youngestFileName, FileMode.Open, FileAccess.Read))
            using (StreamReader sr = new StreamReader(fs))
            {
                return sr.ReadToEnd();
            }
        }

        private void saveToCache(string CachePath, string Contents)
        {
            string saveFilePath = string.Format("{0}{1}_{2:yyyyMMdd_HHmmss}.txt", CachePath, this.ShortName, DateTime.Now);

            if (File.Exists(saveFilePath))
            {
                using (FileStream fs = new FileStream(saveFilePath, FileMode.CreateNew, FileAccess.Write))
                using (StreamWriter sw = new StreamWriter(fs))
                    sw.Write(Contents);
            }
        }

        private string getProductUrlWithPricelistProvider(int ProductID, int AffiliateID)
        {
            string URL = SEOHelper.GetProductURL(ProductID);
            if (AffiliateID != 0)
                URL = CommonHelper.ModifyQueryString(URL, "AffiliateID=" + AffiliateID.ToString(), null);
            return URL;
        }

        /// <summary>
        /// Replaces a message template tokens
        /// </summary>
        /// <param name="productVariant">Product variant instance</param>
        /// <param name="Template">Template</param>
        /// <param name="LocalFormat">Localization Provider Short name (en-US, de-DE, etc.)</param>
        /// <param name="AdditionalKeys">Additional keys</param>
        /// <param name="AffiliateID">Affiliate identifier</param>
        /// <param name="Price">Price</param>
        /// <returns>New template</returns>
        protected string replaceMessageTemplateTokens(ProductVariant productVariant, string Template, string LocalFormat, NameValueCollection AdditionalKeys,
            int AffiliateID, decimal Price)
        {
            NameValueCollection tokens = new NameValueCollection();

            IFormatProvider locProvider = new System.Globalization.CultureInfo(LocalFormat);

            string strHelper = Template;

            while (strHelper.Contains("%"))
            {
                strHelper = strHelper.Substring(strHelper.IndexOf("%") + 1);
                string strToken = strHelper.Substring(0, strHelper.IndexOf("%"));
                string strFormat = "";
                strHelper = strHelper.Substring(strHelper.IndexOf("%") + 1);

                if (strToken.Contains(":"))
                {
                    strFormat = strToken.Substring(strToken.IndexOf(":"));
                    strToken = strToken.Substring(0, strToken.IndexOf(":"));
                }

                if (tokens.Get(strToken + strFormat) == null)
                {
                    switch (strToken.ToLower())
                    {
                        case "store.name":
                            {
                                tokens.Add(strToken + strFormat, String.Format(locProvider, "{0" + strFormat + "}", SettingManager.StoreName));
                            }
                            break;
                        case "product.pictureurl":
                            {
                                ProductPictureCollection pictures = productVariant.Product.ProductPictures;
                                if (pictures.Count > 0)
                                {
                                    tokens.Add(strToken + strFormat, String.Format(locProvider, "{0" + strFormat + "}", PictureManager.GetPictureUrl(pictures[0].PictureID)));
                                }
                                else
                                {
                                    tokens.Add(strToken + strFormat, String.Format(locProvider, "{0" + strFormat + "}", string.Empty));
                                }
                            }
                            break;
                        case "pv.producturl":
                            {
                                tokens.Add(strToken + strFormat, String.Format(locProvider, "{0" + strFormat + "}", getProductUrlWithPricelistProvider(productVariant.ProductID, AffiliateID)));
                            }
                            break;
                        case "pv.price":
                            {
                            tokens.Add(strToken + strFormat, String.Format(locProvider, "{0" + strFormat + "}", Price));
                            }
                            break;
                        case "pv.name":
                            {
                                tokens.Add(strToken + strFormat, String.Format(locProvider, "{0" + strFormat + "}", productVariant.FullProductName));
                            }
                            break;
                        case "pv.description":
                            {
                                tokens.Add(strToken + strFormat, String.Format(locProvider, "{0" + strFormat + "}", productVariant.Description));
                            }
                            break;
                        case "product.description":
                            {
                                tokens.Add(strToken + strFormat, String.Format(locProvider, "{0" + strFormat + "}", productVariant.Product.FullDescription));
                            }
                            break;
                        case "product.shortdescription":
                            {
                                tokens.Add(strToken + strFormat, String.Format(locProvider, "{0" + strFormat + "}", productVariant.Product.ShortDescription));
                            }
                            break;
                        case "pv.partnumber":
                            {
                                tokens.Add(strToken + strFormat, String.Format(locProvider, "{0" + strFormat + "}", productVariant.ManufacturerPartNumber));
                            }
                            break;
                        case "product.manufacturer":
                            {
                                string mans = string.Empty;
                                ProductManufacturerCollection productManufacturers = productVariant.Product.ProductManufacturers;
                                foreach (ProductManufacturer pm in productManufacturers)
                                {
                                    mans += ", " + pm.Manufacturer.Name;
                                }
                                if (mans.Length != 0)
                                    mans = mans.Substring(2);

                                if (productManufacturers.Count > 0)
                                    tokens.Add(strToken + strFormat, String.Format(locProvider, "{0" + strFormat + "}", mans));
                            }
                            break;
                        case "product.category":
                            {
                                string cats = string.Empty;
                                ProductCategoryCollection productCategories = productVariant.Product.ProductCategories;
                                foreach (ProductCategory pc in productCategories)
                                {
                                    cats += ", " + pc.Category.Name;
                                }
                                if (cats.Length != 0)
                                    cats = cats.Substring(2);

                                if (productCategories.Count > 0)
                                    tokens.Add(strToken + strFormat, String.Format(locProvider, "{0" + strFormat + "}", cats));
                            }
                            break;
                        case "product.shippingcosts":
                            {
                            }
                            break;
                        default:
                            {
                            tokens.Add(strToken + strFormat, strToken + strFormat);
                            }
                            break;
                    }
                }

            }

            foreach (string token in tokens.Keys)
                Template = Template.Replace(string.Format(@"%{0}%", token), tokens[token]);

            foreach (string token in AdditionalKeys.Keys)
                Template = Template.Replace(string.Format(@"%{0}%", token), AdditionalKeys[token]);

            return Template;
        }

        /// <summary>
        /// Calculate a price adjustment according to adjustment type
        /// </summary>
        /// <param name="Price">Price to adjust</param>
        /// <param name="PriceAdjustmentType">The type of price adjustment calculation (e.g. absolute adjustment)</param>
        /// <param name="PriceAdjustment">The value for price adjustment calculation</param>
        /// <returns>Adjusted price</returns>
        public decimal getAdjustedPrice(decimal Price, PriceAdjustmentTypeEnum PriceAdjustmentType, decimal PriceAdjustment)
        {
            decimal result = Price;

            switch (PriceAdjustmentType)
            {
                case PriceAdjustmentTypeEnum.AbsoluteAdjustment:
                    {
                        result -= PriceAdjustment;
                    }
                    break;
                case PriceAdjustmentTypeEnum.AbsolutePrice:
                    {
                    }
                    break;
                case PriceAdjustmentTypeEnum.RelativeAdjustment:
                    {
                        result = result * ((100 - PriceAdjustment) / 100);
                    }
                    break;
                default:
                    break;
            }

            if (result < decimal.Zero)
                result = decimal.Zero;

            return result;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates price list
        /// </summary>
        /// <param name="CachePath">Cache path where cached price list can be stored</param>
        /// <returns>Created price list</returns>
        public string CreatePricelist(string CachePath)
        {
            string strContents = string.Empty;

            if (checkCache(this.CacheTime, CachePath))
            {
                return retrieveFromCache(CachePath);
            }
            else
            {
                strContents = createPricelistContents();
                saveToCache(CachePath, strContents);
                return strContents;
            }
        }

        /// <summary>
        /// Gets list of allowed tokens
        /// </summary>
        /// <returns>List of allowed tokens</returns>
        public static string[] GetListOfAllowedTokens()
        {
            List<string> allowedTokens = new List<string>();
            allowedTokens.Add("%store.name%");
            allowedTokens.Add("%product.pictureurl%");
            allowedTokens.Add("%pv.producturl%");
            allowedTokens.Add("%pv.price%");
            allowedTokens.Add("%pv.name%");
            allowedTokens.Add("%pv.description%");
            allowedTokens.Add("%product.description%");
            allowedTokens.Add("%product.shortdescription%");
            allowedTokens.Add("%pv.partnumber%");
            allowedTokens.Add("%product.manufacturer%");
            allowedTokens.Add("%product.category%");
            allowedTokens.Add("%product.shippingcosts%");
            return allowedTokens.ToArray();
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the pricelist identifier
        /// </summary>
        public int PricelistID { get; set; }

        /// <summary>
        /// Gets or sets the mode of list creation (Export all, assigned only, assigned only with special price)
        /// </summary>
        public int ExportModeID { get; set; }

        /// <summary>
        /// Gets or sets the export type
        /// </summary>
        public int ExportTypeID { get; set; }

        /// <summary>
        /// Gets or sets the Affiliate connected to this pricelist (optional), links will be created with AffiliateID
        /// </summary>
        public int AffiliateID { get; set; }

        /// <summary>
        /// Gets or sets the Displayedname
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the shortname to identify the pricelist
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier to get pricelist "anonymous"
        /// </summary>
        public string PricelistGuid { get; set; }

        /// <summary>
        /// Gets or sets the how long will the pricelist be in cached before new creation
        /// </summary>
        public int CacheTime { get; set; }

        /// <summary>
        /// Gets or sets the what localization will be used (numeric formats, etc.) en-US, de-DE etc.
        /// </summary>
        public string FormatLocalization { get; set; }

        /// <summary>
        /// Gets or sets the Displayed description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Admin can put some notes here, not displayed in public
        /// </summary>
        public string AdminNotes { get; set; }

        /// <summary>
        /// Gets or sets the Headerline of the exported file (plain text)
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// Gets or sets the template for an exportet productvariant, uses delimiters and replacement strings
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the Footer line of the exportet file (plain text)
        /// </summary>
        public string Footer { get; set; }

        /// <summary>
        /// Gets or sets the type of price adjustment
        /// </summary>
        public int PriceAdjustmentTypeID { get; set; }

        /// <summary>
        /// Gets or sets the price will be adjusted by this amount (in accordance with PriceAdjustmentType)
        /// </summary>
        public decimal PriceAdjustment { get; set; }

        /// <summary>
        /// Gets or sets the use individual adjustment, if available, or override
        /// </summary>
        public bool OverrideIndivAdjustment { get; set; }

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
        /// Gets the export mode
        /// </summary>
        public PriceListExportModeEnum ExportMode
        {
            get
            {
                return (PriceListExportModeEnum)ExportModeID;
            }
            set
            {
                ExportModeID = (int)value;
            }
        }

        /// <summary>
        /// Gets the log type
        /// </summary>
        public PriceListExportTypeEnum ExportType
        {
            get
            {
                return (PriceListExportTypeEnum)ExportTypeID;
            }
            set
            {
                ExportTypeID = (int)value;
            }
        }

        /// <summary>
        /// Gets the price adjustment type
        /// </summary>
        public PriceAdjustmentTypeEnum PriceAdjustmentType
        {
            get
            {
                return (PriceAdjustmentTypeEnum)PriceAdjustmentTypeID;
            }
            set
            {
                PriceAdjustmentTypeID = (int)value;
            }
        }

        #endregion
    }
}

