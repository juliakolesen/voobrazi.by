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

using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using NopSolutions.NopCommerce.BusinessLogic.Categories;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Manufacturers;
using NopSolutions.NopCommerce.BusinessLogic.Messages;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Products.Attributes;
using NopSolutions.NopCommerce.BusinessLogic.Products.Specs;
using NopSolutions.NopCommerce.BusinessLogic.Promo.Discounts;
using NopSolutions.NopCommerce.BusinessLogic.Utils;

namespace NopSolutions.NopCommerce.BusinessLogic.ExportImport
{
    /// <summary>
    /// Export manager
    /// </summary>
    public partial class ExportManager
    {
        #region Utilities

        private static void WriteCategories(XmlWriter xmlWriter, int ParentCategoryID)
        {
            CategoryCollection categories = CategoryManager.GetAllCategories(ParentCategoryID);
            if (categories.Count > 0)
            {
                foreach (Category category in categories)
                {
                    xmlWriter.WriteStartElement("Category");
                    xmlWriter.WriteElementString("CategoryID", null, category.CategoryID.ToString());
                    xmlWriter.WriteElementString("Name", null, category.Name);
                    xmlWriter.WriteElementString("Description", null, category.Description);
                    xmlWriter.WriteElementString("TemplateID", null, category.TemplateID.ToString());
                    xmlWriter.WriteElementString("MetaKeywords", null, category.MetaKeywords);
                    xmlWriter.WriteElementString("MetaDescription", null, category.MetaDescription);
                    xmlWriter.WriteElementString("MetaTitle", null, category.MetaTitle);
                    xmlWriter.WriteElementString("SEName", null, category.SEName);
                    xmlWriter.WriteElementString("ParentCategoryID", null, category.ParentCategoryID.ToString());
                    xmlWriter.WriteElementString("PictureID", null, category.PictureID.ToString());
                    xmlWriter.WriteElementString("PageSize", null, category.PageSize.ToString());
                    xmlWriter.WriteElementString("PriceRanges", null, category.PriceRanges);
                    xmlWriter.WriteElementString("Published", null, category.Published.ToString());
                    xmlWriter.WriteElementString("Deleted", null, category.Deleted.ToString());
                    xmlWriter.WriteElementString("DisplayOrder", null, category.DisplayOrder.ToString());
                    xmlWriter.WriteElementString("CreatedOn", null, category.CreatedOn.ToString());
                    xmlWriter.WriteElementString("UpdatedOn", null, category.UpdatedOn.ToString());

                    xmlWriter.WriteStartElement("Products");
                    ProductCategoryCollection productCategories = category.ProductCategories;
                    foreach (ProductCategory productCategory in productCategories)
                    {
                        Product product = productCategory.Product;
                        if (product != null && !product.Deleted)
                        {
                            xmlWriter.WriteStartElement("ProductCategory");
                            xmlWriter.WriteElementString("ProductCategoryID", null, productCategory.ProductCategoryID.ToString());
                            xmlWriter.WriteElementString("ProductID", null, productCategory.ProductID.ToString());
                            xmlWriter.WriteElementString("IsFeaturedProduct", null, productCategory.IsFeaturedProduct.ToString());
                            xmlWriter.WriteElementString("DisplayOrder", null, productCategory.DisplayOrder.ToString());
                            xmlWriter.WriteEndElement();
                        }
                    }
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("SubCategories");
                    WriteCategories(xmlWriter, category.CategoryID);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                }
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Export customer list to xml
        /// </summary>
        /// <param name="customers">Customers</param>
        /// <returns>Result in XML format</returns>
        public static string ExportCustomersToXML(CustomerCollection customers)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter stringWriter = new StringWriter(sb);
            XmlWriter xmlWriter = new XmlTextWriter(stringWriter);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Customers");
            xmlWriter.WriteAttributeString("Version", SiteHelper.GetCurrentVersion());

            foreach (Customer customer in customers)
            {
                xmlWriter.WriteStartElement("Customer");
                xmlWriter.WriteElementString("CustomerID", null, customer.CustomerID.ToString());
                xmlWriter.WriteElementString("CustomerGUID", null, customer.CustomerGUID.ToString());
                xmlWriter.WriteElementString("Email", null, customer.Email);
                xmlWriter.WriteElementString("Username", null, customer.Username);
                xmlWriter.WriteElementString("PasswordHash", null, customer.PasswordHash);
                xmlWriter.WriteElementString("SaltKey", null, customer.SaltKey);
                xmlWriter.WriteElementString("AffiliateID", null, customer.AffiliateID.ToString());
                xmlWriter.WriteElementString("BillingAddressID", null, customer.BillingAddressID.ToString());
                xmlWriter.WriteElementString("ShippingAddressID", null, customer.ShippingAddressID.ToString());
                xmlWriter.WriteElementString("LastPaymentMethodID", null, customer.LastPaymentMethodID.ToString());
                xmlWriter.WriteElementString("LastAppliedCouponCode", null, customer.LastAppliedCouponCode);
                xmlWriter.WriteElementString("LanguageID", null, customer.LanguageID.ToString());
                xmlWriter.WriteElementString("CurrencyID", null, customer.CurrencyID.ToString());
                xmlWriter.WriteElementString("TaxDisplayTypeID", null, customer.TaxDisplayTypeID.ToString());
                xmlWriter.WriteElementString("IsTaxExempt", null, customer.IsTaxExempt.ToString());
                xmlWriter.WriteElementString("IsAdmin", null, customer.IsAdmin.ToString());
                xmlWriter.WriteElementString("IsGuest", null, customer.IsGuest.ToString());
                xmlWriter.WriteElementString("IsForumModerator", null, customer.IsForumModerator.ToString());
                xmlWriter.WriteElementString("TotalForumPosts", null, customer.TotalForumPosts.ToString());
                xmlWriter.WriteElementString("Active", null, customer.Active.ToString());
                xmlWriter.WriteElementString("Deleted", null, customer.Deleted.ToString());
                xmlWriter.WriteElementString("RegistrationDate", null, customer.RegistrationDate.ToString());
                xmlWriter.WriteElementString("TimeZoneID", null, customer.TimeZoneID);
                xmlWriter.WriteElementString("AvatarID", null, customer.AvatarID.ToString());


                xmlWriter.WriteElementString("Gender", null, customer.Gender);
                xmlWriter.WriteElementString("FirstName", null, customer.FirstName);
                xmlWriter.WriteElementString("LastName", null, customer.LastName);
                if (customer.DateOfBirth.HasValue)
                    xmlWriter.WriteElementString("DateOfBirth", null, customer.DateOfBirth.Value.ToBinary().ToString());
                xmlWriter.WriteElementString("Company", null, customer.Company);
                xmlWriter.WriteElementString("StreetAddress", null, customer.StreetAddress);
                xmlWriter.WriteElementString("StreetAddress2", null, customer.StreetAddress2);
                xmlWriter.WriteElementString("ZipPostalCode", null, customer.ZipPostalCode);
                xmlWriter.WriteElementString("City", null, customer.City);
                xmlWriter.WriteElementString("PhoneNumber", null, customer.PhoneNumber);
                xmlWriter.WriteElementString("FaxNumber", null, customer.FaxNumber);

                xmlWriter.WriteElementString("CountryID", null, customer.CountryID.ToString());
                Country country = CountryManager.GetCountryByID(customer.CountryID);
                xmlWriter.WriteElementString("Country", null, (country == null) ? string.Empty : country.Name);

                xmlWriter.WriteElementString("StateProvinceID", null, customer.StateProvinceID.ToString());
                StateProvince stateProvince = StateProvinceManager.GetStateProvinceByID(customer.StateProvinceID);
                xmlWriter.WriteElementString("StateProvince", null, (stateProvince == null) ? string.Empty : stateProvince.Name);
                xmlWriter.WriteElementString("ReceiveNewsletter", null, customer.ReceiveNewsletter.ToString());

                AddressCollection billingAddresses = customer.BillingAddresses;
                if (billingAddresses.Count > 0)
                {
                    xmlWriter.WriteStartElement("BillingAddresses");
                    foreach (Address address in billingAddresses)
                    {
                        xmlWriter.WriteStartElement("Address");
                        xmlWriter.WriteElementString("AddressID", null, address.AddressID.ToString());
                        xmlWriter.WriteElementString("FirstName", null, address.FirstName);
                        xmlWriter.WriteElementString("LastName", null, address.LastName);
                        xmlWriter.WriteElementString("PhoneNumber", null, address.PhoneNumber);
                        xmlWriter.WriteElementString("Email", null, address.Email);
                        xmlWriter.WriteElementString("FaxNumber", null, address.FaxNumber);
                        xmlWriter.WriteElementString("Company", null, address.Company);
                        xmlWriter.WriteElementString("Address1", null, address.Address1);
                        xmlWriter.WriteElementString("Address2", null, address.Address2);
                        xmlWriter.WriteElementString("City", null, address.City);
                        xmlWriter.WriteElementString("StateProvinceID", null, address.StateProvinceID.ToString());
                        xmlWriter.WriteElementString("StateProvince", null, (address.StateProvince == null) ? string.Empty : address.StateProvince.Name);
                        xmlWriter.WriteElementString("ZipPostalCode", null, address.ZipPostalCode);
                        xmlWriter.WriteElementString("CountryID", null, address.CountryID.ToString());
                        xmlWriter.WriteElementString("Country", null, (address.Country == null) ? string.Empty : address.Country.Name);
                        xmlWriter.WriteElementString("CreatedOn", null, address.CreatedOn.ToString());
                        xmlWriter.WriteElementString("UpdatedOn", null, address.UpdatedOn.ToString());
                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement();
                }

                AddressCollection shippingAddresses = customer.ShippingAddresses;
                if (shippingAddresses.Count > 0)
                {
                    xmlWriter.WriteStartElement("ShippingAddresses");
                    foreach (Address address in shippingAddresses)
                    {
                        xmlWriter.WriteStartElement("Address");
                        xmlWriter.WriteElementString("AddressID", null, address.AddressID.ToString());
                        xmlWriter.WriteElementString("FirstName", null, address.FirstName);
                        xmlWriter.WriteElementString("LastName", null, address.LastName);
                        xmlWriter.WriteElementString("PhoneNumber", null, address.PhoneNumber);
                        xmlWriter.WriteElementString("Email", null, address.Email);
                        xmlWriter.WriteElementString("FaxNumber", null, address.FaxNumber);
                        xmlWriter.WriteElementString("Company", null, address.Company);
                        xmlWriter.WriteElementString("Address1", null, address.Address1);
                        xmlWriter.WriteElementString("Address2", null, address.Address2);
                        xmlWriter.WriteElementString("City", null, address.City);
                        xmlWriter.WriteElementString("StateProvinceID", null, address.StateProvinceID.ToString());
                        xmlWriter.WriteElementString("StateProvince", null, (address.StateProvince == null) ? string.Empty : address.StateProvince.Name);
                        xmlWriter.WriteElementString("ZipPostalCode", null, address.ZipPostalCode);
                        xmlWriter.WriteElementString("CountryID", null, address.CountryID.ToString());
                        xmlWriter.WriteElementString("Country", null, (address.Country == null) ? string.Empty : address.Country.Name);
                        xmlWriter.WriteElementString("CreatedOn", null, address.CreatedOn.ToString());
                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            return stringWriter.ToString();
        }

        /// <summary>
        /// Export customer list to XLS
        /// </summary>
        /// <param name="FilePath">File path to use</param>
        /// <param name="customers">Customers</param>
        public static void ExportCustomersToXLS(string FilePath, CustomerCollection customers)
        {
            using (ExcelHelper excelHelper = new ExcelHelper(FilePath))
            {
                excelHelper.HDR = "YES";
                excelHelper.IMEX = "0";
                Dictionary<string, string> tableDefinition = new Dictionary<string,string>();
                tableDefinition.Add("CustomerID", "int");
                tableDefinition.Add("CustomerGUID", "uniqueidentifier");
                tableDefinition.Add("Email", "nvarchar(255)");
                tableDefinition.Add("Username", "nvarchar(255)");
                tableDefinition.Add("PasswordHash", "nvarchar(255)");
                tableDefinition.Add("SaltKey", "nvarchar(255)");
                tableDefinition.Add("AffiliateID", "int");
                tableDefinition.Add("BillingAddressID", "int");
                tableDefinition.Add("ShippingAddressID", "int");
                tableDefinition.Add("LastPaymentMethodID", "int");
                tableDefinition.Add("LastAppliedCouponCode", "nvarchar(255)");
                tableDefinition.Add("LanguageID", "int");
                tableDefinition.Add("CurrencyID", "int");
                tableDefinition.Add("TaxDisplayTypeID", "int");
                tableDefinition.Add("IsTaxExempt", "nvarchar(5)");
                tableDefinition.Add("IsAdmin", "nvarchar(5)");
                tableDefinition.Add("IsGuest", "nvarchar(5)");
                tableDefinition.Add("IsForumModerator", "nvarchar(5)");
                tableDefinition.Add("TotalForumPosts", "int");
                tableDefinition.Add("Signature", "nvarchar(255)");
                tableDefinition.Add("AdminComment", "nvarchar(255)");
                tableDefinition.Add("Active", "nvarchar(5)");
                tableDefinition.Add("Deleted", "nvarchar(5)");
                tableDefinition.Add("RegistrationDate", "datetime");
                tableDefinition.Add("TimeZoneID", "nvarchar(200)");
                tableDefinition.Add("AvatarID", "int");
                tableDefinition.Add("Gender", "nvarchar(100)");
                tableDefinition.Add("FirstName", "nvarchar(100)");
                tableDefinition.Add("LastName", "nvarchar(100)");
                tableDefinition.Add("Company", "nvarchar(100)");
                tableDefinition.Add("StreetAddress", "nvarchar(100)");
                tableDefinition.Add("StreetAddress2", "nvarchar(100)");
                tableDefinition.Add("ZipPostalCode", "nvarchar(100)");
                tableDefinition.Add("City", "nvarchar(100)");
                tableDefinition.Add("PhoneNumber", "nvarchar(100)");
                tableDefinition.Add("FaxNumber", "nvarchar(100)");
                tableDefinition.Add("CountryID", "int");
                tableDefinition.Add("StateProvinceID", "int");
                tableDefinition.Add("ReceiveNewsletter", "nvarchar(5)");
                excelHelper.WriteTable("Customers", tableDefinition);

                foreach (Customer customer in customers)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("INSERT INTO [Customers] (CustomerID, CustomerGUID, Email, Username, PasswordHash, SaltKey, AffiliateID, BillingAddressID, ShippingAddressID, LastPaymentMethodID, LastAppliedCouponCode, LanguageID, CurrencyID, TaxDisplayTypeID, IsTaxExempt, IsAdmin, IsGuest, IsForumModerator, TotalForumPosts, Signature, AdminComment, Active, Deleted, RegistrationDate, TimeZoneID, AvatarID, Gender, FirstName, LastName, Company, StreetAddress, StreetAddress2, ZipPostalCode, City, PhoneNumber, FaxNumber, CountryID, StateProvinceID, ReceiveNewsletter) VALUES (");
                    sb.Append(customer.CustomerID); sb.Append(",");
                    sb.Append('"'); sb.Append(customer.CustomerGUID); sb.Append("\",");
                    sb.Append('"'); sb.Append(customer.Email.Replace('"', '\'')); sb.Append("\",");
                    sb.Append('"'); sb.Append(customer.Username); sb.Append("\",");
                    sb.Append('"'); sb.Append(customer.PasswordHash.Replace('"', '\'')); sb.Append("\",");
                    sb.Append('"'); sb.Append(customer.SaltKey.Replace('"', '\'')); sb.Append("\",");
                    sb.Append(customer.AffiliateID); sb.Append(",");
                    sb.Append(customer.BillingAddressID); sb.Append(",");
                    sb.Append(customer.ShippingAddressID); sb.Append(",");
                    sb.Append(customer.LastPaymentMethodID); sb.Append(",");
                    sb.Append('"'); sb.Append(customer.LastAppliedCouponCode.Replace('"', '\'')); sb.Append("\",");
                    sb.Append(customer.LanguageID); sb.Append(",");
                    sb.Append(customer.CurrencyID); sb.Append(",");
                    sb.Append(customer.TaxDisplayTypeID); sb.Append(',');
                    sb.Append('"'); sb.Append(customer.IsTaxExempt); sb.Append("\",");
                    sb.Append('"'); sb.Append(customer.IsAdmin); sb.Append("\",");
                    sb.Append('"'); sb.Append(customer.IsGuest); sb.Append("\",");
                    sb.Append('"'); sb.Append(customer.IsForumModerator); sb.Append("\",");
                    sb.Append(customer.TotalForumPosts); sb.Append(',');
                    sb.Append('"'); sb.Append(customer.Signature.Replace('"', '\'')); sb.Append("\",");
                    sb.Append('"'); sb.Append(customer.AdminComment.Replace('"', '\'')); sb.Append("\",");
                    sb.Append('"'); sb.Append(customer.Active); sb.Append("\",");
                    sb.Append('"'); sb.Append(customer.Deleted); sb.Append("\",");
                    sb.Append('"'); sb.Append(customer.RegistrationDate); sb.Append("\",");
                    sb.Append('"'); sb.Append(customer.TimeZoneID); sb.Append("\",");
                    sb.Append(customer.AvatarID); sb.Append(',');

                    //custom properties
                    sb.Append('"'); sb.Append(customer.Gender); sb.Append("\",");
                    sb.Append('"'); sb.Append(customer.FirstName); sb.Append("\",");
                    sb.Append('"'); sb.Append(customer.LastName); sb.Append("\",");
                    sb.Append('"'); sb.Append(customer.Company); sb.Append("\",");
                    sb.Append('"'); sb.Append(customer.StreetAddress); sb.Append("\",");
                    sb.Append('"'); sb.Append(customer.StreetAddress2); sb.Append("\",");
                    sb.Append('"'); sb.Append(customer.ZipPostalCode); sb.Append("\",");
                    sb.Append('"'); sb.Append(customer.City); sb.Append("\",");
                    sb.Append('"'); sb.Append(customer.PhoneNumber); sb.Append("\",");
                    sb.Append('"'); sb.Append(customer.FaxNumber); sb.Append("\",");
                    sb.Append(customer.CountryID); sb.Append(',');
                    sb.Append(customer.StateProvinceID); sb.Append(',');
                    sb.Append('"'); sb.Append(customer.ReceiveNewsletter); sb.Append("\"");
                    sb.Append(")");

                    excelHelper.ExecuteCommand(sb.ToString());
                }
            }
        }

        /// <summary>
        /// Export manufacturer list to xml
        /// </summary>
        /// <returns>Result in XML format</returns>
        public static string ExportManufacturersToXML(ManufacturerCollection manufacturers)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter stringWriter = new StringWriter(sb);
            XmlWriter xmlWriter = new XmlTextWriter(stringWriter);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Manufacturers");
            xmlWriter.WriteAttributeString("Version", SiteHelper.GetCurrentVersion());

            foreach (Manufacturer manufacturer in manufacturers)
            {
                xmlWriter.WriteStartElement("Manufacturer");
                xmlWriter.WriteElementString("ManufacturerID", null, manufacturer.ManufacturerID.ToString());
                xmlWriter.WriteElementString("Name", null, manufacturer.Name);
                xmlWriter.WriteElementString("Description", null, manufacturer.Description);
                xmlWriter.WriteElementString("TemplateID", null, manufacturer.TemplateID.ToString());
                xmlWriter.WriteElementString("MetaKeywords", null, manufacturer.MetaKeywords);
                xmlWriter.WriteElementString("MetaDescription", null, manufacturer.MetaDescription);
                xmlWriter.WriteElementString("MetaTitle", null, manufacturer.MetaTitle);
                xmlWriter.WriteElementString("SEName", null, manufacturer.SEName);
                xmlWriter.WriteElementString("PictureID", null, manufacturer.PictureID.ToString());
                xmlWriter.WriteElementString("PageSize", null, manufacturer.PageSize.ToString());
                xmlWriter.WriteElementString("PriceRanges", null, manufacturer.PriceRanges);
                xmlWriter.WriteElementString("Published", null, manufacturer.Published.ToString());
                xmlWriter.WriteElementString("Deleted", null, manufacturer.Deleted.ToString());
                xmlWriter.WriteElementString("DisplayOrder", null, manufacturer.DisplayOrder.ToString());
                xmlWriter.WriteElementString("CreatedOn", null, manufacturer.CreatedOn.ToString());
                xmlWriter.WriteElementString("UpdatedOn", null, manufacturer.UpdatedOn.ToString());

                xmlWriter.WriteStartElement("Products");
                ProductManufacturerCollection productManufacturers = manufacturer.ProductManufacturers;
                foreach (ProductManufacturer productManufacturer in productManufacturers)
                {
                    Product product = productManufacturer.Product;
                    if (product != null && !product.Deleted)
                    {
                        xmlWriter.WriteStartElement("ProductManufacturer");
                        xmlWriter.WriteElementString("ProductManufacturerID", null, productManufacturer.ProductManufacturerID.ToString());
                        xmlWriter.WriteElementString("ProductID", null, productManufacturer.ProductID.ToString());
                        xmlWriter.WriteElementString("IsFeaturedProduct", null, productManufacturer.IsFeaturedProduct.ToString());
                        xmlWriter.WriteElementString("DisplayOrder", null, productManufacturer.DisplayOrder.ToString());
                        xmlWriter.WriteEndElement();
                    }
                }
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            return stringWriter.ToString();
        }

        /// <summary>
        /// Export category list to xml
        /// </summary>
        /// <returns>Result in XML format</returns>
        public static string ExportCategoriesToXML()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter stringWriter = new StringWriter(sb);
            XmlWriter xmlWriter = new XmlTextWriter(stringWriter);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Categories");
            xmlWriter.WriteAttributeString("Version", SiteHelper.GetCurrentVersion());
            WriteCategories(xmlWriter, 0);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            return stringWriter.ToString();
        }

        /// <summary>
        /// Export product list to xml
        /// </summary>
        /// <param name="products">Products</param>
        /// <returns>Result in XML format</returns>
        public static string ExportProductsToXML(List<Product> products)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter stringWriter = new StringWriter(sb);
            XmlWriter xmlWriter = new XmlTextWriter(stringWriter);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Products");
            xmlWriter.WriteAttributeString("Version", SiteHelper.GetCurrentVersion());
            
            foreach (Product product in products)
            {
                xmlWriter.WriteStartElement("Product");
                xmlWriter.WriteElementString("ProductID", null, product.ProductID.ToString());
                xmlWriter.WriteElementString("Name", null, product.Name);
                xmlWriter.WriteElementString("ShortDescription", null, product.ShortDescription);
                xmlWriter.WriteElementString("FullDescription", null, product.FullDescription);
                xmlWriter.WriteElementString("AdminComment", null, product.AdminComment);
                xmlWriter.WriteElementString("ProductTypeID", null, product.ProductTypeID.ToString());
                xmlWriter.WriteElementString("TemplateID", null, product.TemplateID.ToString());
                xmlWriter.WriteElementString("ShowOnHomePage", null, product.ShowOnHomePage.ToString());
                xmlWriter.WriteElementString("MetaKeywords", null, product.MetaKeywords);
                xmlWriter.WriteElementString("MetaDescription", null, product.MetaDescription);
                xmlWriter.WriteElementString("MetaTitle", null, product.MetaTitle);
                xmlWriter.WriteElementString("SEName", null, product.SEName);
                xmlWriter.WriteElementString("AllowCustomerReviews", null, product.AllowCustomerReviews.ToString());
                xmlWriter.WriteElementString("AllowCustomerRatings", null, product.AllowCustomerRatings.ToString());
                xmlWriter.WriteElementString("RatingSum", null, product.RatingSum.ToString());
                xmlWriter.WriteElementString("TotalRatingVotes", null, product.TotalRatingVotes.ToString());
                xmlWriter.WriteElementString("Published", null, product.Published.ToString());
                xmlWriter.WriteElementString("Deleted", null, product.Deleted.ToString());
                xmlWriter.WriteElementString("CreatedOn", null, product.CreatedOn.ToString());
                xmlWriter.WriteElementString("UpdatedOn", null, product.UpdatedOn.ToString());

                xmlWriter.WriteStartElement("ProductVariants");
                ProductVariantCollection productVariants = product.ProductVariants;
                foreach (ProductVariant productVariant in productVariants)
                {
                    xmlWriter.WriteStartElement("ProductVariant");
                    xmlWriter.WriteElementString("ProductVariantID", null, productVariant.ProductVariantID.ToString());
                    xmlWriter.WriteElementString("ProductID", null, productVariant.ProductID.ToString());
                    xmlWriter.WriteElementString("Name", null, productVariant.Name);
                    xmlWriter.WriteElementString("SKU", null, productVariant.SKU);
                    xmlWriter.WriteElementString("Description", null, productVariant.Description);
                    xmlWriter.WriteElementString("AdminComment", null, productVariant.AdminComment);
                    xmlWriter.WriteElementString("ManufacturerPartNumber", null, productVariant.ManufacturerPartNumber);
                    xmlWriter.WriteElementString("IsDownload", null, productVariant.IsDownload.ToString());
                    xmlWriter.WriteElementString("DownloadID", null, productVariant.DownloadID.ToString());
                    xmlWriter.WriteElementString("UnlimitedDownloads", null, productVariant.UnlimitedDownloads.ToString());
                    xmlWriter.WriteElementString("MaxNumberOfDownloads", null, productVariant.MaxNumberOfDownloads.ToString());
                    xmlWriter.WriteElementString("HasSampleDownload", null, productVariant.HasSampleDownload.ToString());
                    xmlWriter.WriteElementString("SampleDownloadID", null, productVariant.SampleDownloadID.ToString());
                    xmlWriter.WriteElementString("IsShipEnabled", null, productVariant.IsShipEnabled.ToString());
                    xmlWriter.WriteElementString("IsFreeShipping", null, productVariant.IsFreeShipping.ToString());
                    xmlWriter.WriteElementString("AdditionalShippingCharge", null, productVariant.AdditionalShippingCharge.ToString());
                    xmlWriter.WriteElementString("IsTaxExempt", null, productVariant.IsTaxExempt.ToString());
                    xmlWriter.WriteElementString("TaxCategoryID", null, productVariant.TaxCategoryID.ToString());
                    xmlWriter.WriteElementString("ManageInventory", null, productVariant.ManageInventory.ToString());
                    xmlWriter.WriteElementString("StockQuantity", null, productVariant.StockQuantity.ToString());
                    xmlWriter.WriteElementString("MinStockQuantity", null, productVariant.MinStockQuantity.ToString());
                    xmlWriter.WriteElementString("LowStockActivityID", null, productVariant.LowStockActivityID.ToString());
                    xmlWriter.WriteElementString("NotifyAdminForQuantityBelow", null, productVariant.NotifyAdminForQuantityBelow.ToString());
                    xmlWriter.WriteElementString("OrderMinimumQuantity", null, productVariant.OrderMinimumQuantity.ToString());
                    xmlWriter.WriteElementString("OrderMaximumQuantity", null, productVariant.OrderMaximumQuantity.ToString());
                    xmlWriter.WriteElementString("WarehouseId", null, productVariant.WarehouseId.ToString());
                    xmlWriter.WriteElementString("DisableBuyButton", null, productVariant.DisableBuyButton.ToString());
                    xmlWriter.WriteElementString("Price", null, productVariant.Price.ToString());
                    xmlWriter.WriteElementString("OldPrice", null, productVariant.OldPrice.ToString());
                    xmlWriter.WriteElementString("Weight", null, productVariant.Weight.ToString());
                    xmlWriter.WriteElementString("Length", null, productVariant.Length.ToString());
                    xmlWriter.WriteElementString("Width", null, productVariant.Width.ToString());
                    xmlWriter.WriteElementString("Height", null, productVariant.Height.ToString());
                    xmlWriter.WriteElementString("PictureID", null, productVariant.PictureID.ToString());
                    xmlWriter.WriteElementString("Published", null, productVariant.Published.ToString());
                    xmlWriter.WriteElementString("Deleted", null, productVariant.Deleted.ToString());
                    xmlWriter.WriteElementString("DisplayOrder", null, productVariant.DisplayOrder.ToString());
                    xmlWriter.WriteElementString("CreatedOn", null, productVariant.CreatedOn.ToString());
                    xmlWriter.WriteElementString("UpdatedOn", null, productVariant.UpdatedOn.ToString());

                    xmlWriter.WriteStartElement("ProductDiscounts");
                    DiscountCollection discounts = productVariant.AllDiscounts;
                    foreach (Discount discount in discounts)
                    {
                        xmlWriter.WriteElementString("DiscountID", null, discount.DiscountID.ToString());
                    }
                    xmlWriter.WriteEndElement();


                    xmlWriter.WriteStartElement("TierPrices");
                    TierPriceCollection tierPrices = productVariant.TierPrices;
                    foreach (TierPrice tierPrice in tierPrices)
                    {
                        xmlWriter.WriteElementString("TierPriceID", null, tierPrice.TierPriceID.ToString());
                        xmlWriter.WriteElementString("Quantity", null, tierPrice.Quantity.ToString());
                        xmlWriter.WriteElementString("Price", null, tierPrice.Price.ToString());
                    }
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("ProductAttributes");
                    ProductVariantAttributeCollection productVariantAttributes = productVariant.ProductVariantAttributes;
                    foreach (ProductVariantAttribute productVariantAttribute in productVariantAttributes)
                    {
                        xmlWriter.WriteStartElement("ProductVariantAttribute");
                        xmlWriter.WriteElementString("ProductVariantAttributeID", null, productVariantAttribute.ProductVariantAttributeID.ToString());
                        xmlWriter.WriteElementString("ProductAttributeID", null, productVariantAttribute.ProductAttributeID.ToString());
                        xmlWriter.WriteElementString("TextPrompt", null, productVariantAttribute.TextPrompt);
                        xmlWriter.WriteElementString("IsRequired", null, productVariantAttribute.IsRequired.ToString());
                        xmlWriter.WriteElementString("AttributeControlTypeID", null, productVariantAttribute.AttributeControlTypeID.ToString());
                        xmlWriter.WriteElementString("DisplayOrder", null, productVariantAttribute.DisplayOrder.ToString());




                        xmlWriter.WriteStartElement("ProductVariantAttributeValues");
                        ProductVariantAttributeValueCollection productVariantAttributeValues = productVariantAttribute.ProductVariantAttributeValues;
                        foreach (ProductVariantAttributeValue productVariantAttributeValue in productVariantAttributeValues)
                        {
                            xmlWriter.WriteElementString("ProductVariantAttributeValueID", null, productVariantAttributeValue.ProductVariantAttributeValueID.ToString());
                            xmlWriter.WriteElementString("Name", null, productVariantAttributeValue.Name);
                            xmlWriter.WriteElementString("PriceAdjustment", null, productVariantAttributeValue.PriceAdjustment.ToString());
                            xmlWriter.WriteElementString("WeightAdjustment", null, productVariantAttributeValue.WeightAdjustment.ToString());
                            xmlWriter.WriteElementString("IsPreSelected", null, productVariantAttributeValue.IsPreSelected.ToString());
                            xmlWriter.WriteElementString("DisplayOrder", null, productVariantAttributeValue.DisplayOrder.ToString());
                        }
                        xmlWriter.WriteEndElement();


                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement();



                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("ProductPictures");
                ProductPictureCollection productPictures = product.ProductPictures;
                foreach (ProductPicture productPicture in productPictures)
                {
                    xmlWriter.WriteStartElement("ProductPicture");
                    xmlWriter.WriteElementString("ProductPictureID", null, productPicture.ProductPictureID.ToString());
                    xmlWriter.WriteElementString("PictureID", null, productPicture.PictureID.ToString());
                    xmlWriter.WriteElementString("DisplayOrder", null, productPicture.DisplayOrder.ToString());
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("RelatedProducts");
                RelatedProductCollection relatedProducts = product.RelatedProducts;
                foreach (RelatedProduct relatedProduct in relatedProducts)
                {
                    xmlWriter.WriteStartElement("RelatedProduct");
                    xmlWriter.WriteElementString("RelatedProductID", null, relatedProduct.RelatedProductID.ToString());
                    xmlWriter.WriteElementString("ProductID1", null, relatedProduct.ProductID1.ToString());
                    xmlWriter.WriteElementString("ProductID2", null, relatedProduct.ProductID2.ToString());
                    xmlWriter.WriteElementString("DisplayOrder", null, relatedProduct.DisplayOrder.ToString());
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("ProductCategories");
                ProductCategoryCollection productCategories = product.ProductCategories;
                foreach (ProductCategory productCategory in productCategories)
                {
                    xmlWriter.WriteStartElement("ProductCategory");
                    xmlWriter.WriteElementString("ProductCategoryID", null, productCategory.ProductCategoryID.ToString());
                    xmlWriter.WriteElementString("CategoryID", null, productCategory.CategoryID.ToString());
                    xmlWriter.WriteElementString("IsFeaturedProduct", null, productCategory.IsFeaturedProduct.ToString());
                    xmlWriter.WriteElementString("DisplayOrder", null, productCategory.DisplayOrder.ToString());
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("ProductManufacturers");
                ProductManufacturerCollection productManufacturers = product.ProductManufacturers;
                foreach (ProductManufacturer productManufacturer in productManufacturers)
                {
                    xmlWriter.WriteStartElement("ProductManufacturer");
                    xmlWriter.WriteElementString("ProductManufacturerID", null, productManufacturer.ProductManufacturerID.ToString());
                    xmlWriter.WriteElementString("ManufacturerID", null, productManufacturer.ManufacturerID.ToString());
                    xmlWriter.WriteElementString("IsFeaturedProduct", null, productManufacturer.IsFeaturedProduct.ToString());
                    xmlWriter.WriteElementString("DisplayOrder", null, productManufacturer.DisplayOrder.ToString());
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("ProductSpecificationAttributes");
                ProductSpecificationAttributeCollection productSpecificationAttributes = SpecificationAttributeManager.GetProductSpecificationAttributesByProductID(product.ProductID);
                foreach (ProductSpecificationAttribute productSpecificationAttribute in productSpecificationAttributes)
                {
                    xmlWriter.WriteStartElement("ProductSpecificationAttribute");
                    xmlWriter.WriteElementString("ProductSpecificationAttributeID", null, productSpecificationAttribute.ProductSpecificationAttributeID.ToString());
                    xmlWriter.WriteElementString("SpecificationAttributeOptionID", null, productSpecificationAttribute.SpecificationAttributeOptionID.ToString());
                    xmlWriter.WriteElementString("AllowFiltering", null, productSpecificationAttribute.AllowFiltering.ToString());
                    xmlWriter.WriteElementString("ShowOnProductPage", null, productSpecificationAttribute.ShowOnProductPage.ToString());
                    xmlWriter.WriteElementString("DisplayOrder", null, productSpecificationAttribute.DisplayOrder.ToString());
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();




                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            return stringWriter.ToString();
        }

        /// <summary>
        /// Export products to XLS
        /// </summary>
        /// <param name="FilePath">File path to use</param>
        /// <param name="products">Products</param>
        public static void ExportProductsToXLS(string FilePath, List<Product> products)
        {
            using (ExcelHelper excelHelper = new ExcelHelper(FilePath))
            {
                excelHelper.HDR = "YES";
                excelHelper.IMEX = "0";
                Dictionary<string, string> tableDefinition = new Dictionary<string, string>();
                int maxStringLength = 200;
                tableDefinition.Add("Name", string.Format("nvarchar({0})",maxStringLength));
                tableDefinition.Add("ShortDescription", string.Format("nvarchar({0})", maxStringLength));
                tableDefinition.Add("FullDescription", string.Format("nvarchar({0})", maxStringLength));
                tableDefinition.Add("ProductTypeID", "int");
                tableDefinition.Add("TemplateID", "int");
                tableDefinition.Add("ShowOnHomePage", "nvarchar(5)");
                tableDefinition.Add("MetaKeywords", string.Format("nvarchar({0})", maxStringLength));
                tableDefinition.Add("MetaDescription", string.Format("nvarchar({0})", maxStringLength));
                tableDefinition.Add("MetaTitle", string.Format("nvarchar({0})", maxStringLength));
                tableDefinition.Add("AllowCustomerReviews", "nvarchar(5)");
                tableDefinition.Add("AllowCustomerRatings", "nvarchar(5)");
                tableDefinition.Add("Published", "nvarchar(5)");
                tableDefinition.Add("SKU", string.Format("nvarchar(200)", maxStringLength));
                tableDefinition.Add("ManufacturerPartNumber", string.Format("nvarchar({0})", maxStringLength));
                tableDefinition.Add("IsDownload", "nvarchar(5)");
                tableDefinition.Add("DownloadID", "int");
                tableDefinition.Add("UnlimitedDownloads", "nvarchar(5)");
                tableDefinition.Add("MaxNumberOfDownloads", "int");
                tableDefinition.Add("HasSampleDownload", "nvarchar(5)");
                tableDefinition.Add("SampleDownloadID", "int");
                tableDefinition.Add("IsShipEnabled", "nvarchar(5)");
                tableDefinition.Add("IsFreeShipping", "nvarchar(5)");
                tableDefinition.Add("AdditionalShippingCharge", "decimal");
                tableDefinition.Add("IsTaxExempt", "nvarchar(5)");
                tableDefinition.Add("TaxCategoryID", "int");
                tableDefinition.Add("ManageInventory", "nvarchar(5)");
                tableDefinition.Add("StockQuantity", "int");
                tableDefinition.Add("MinStockQuantity", "int");
                tableDefinition.Add("LowStockActivityID", "int");
                tableDefinition.Add("NotifyAdminForQuantityBelow", "int");
                tableDefinition.Add("OrderMinimumQuantity", "int");
                tableDefinition.Add("OrderMaximumQuantity", "int");
                tableDefinition.Add("DisableBuyButton", "nvarchar(5)");
                tableDefinition.Add("Price", "decimal");
                tableDefinition.Add("OldPrice", "decimal");
                tableDefinition.Add("Weight", "decimal");
                tableDefinition.Add("Length", "decimal");
                tableDefinition.Add("Width", "decimal");
                tableDefinition.Add("Height", "decimal");
                tableDefinition.Add("CreatedOn", "datetime");
                excelHelper.WriteTable("Products", tableDefinition);

                foreach (Product p in products)
                {
                    ProductVariantCollection productVariants = p.ProductVariants;
                    foreach (ProductVariant pv in productVariants)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("INSERT INTO [Products] (Name, ShortDescription,FullDescription,ProductTypeID,TemplateID,ShowOnHomePage,MetaKeywords,MetaDescription,MetaTitle,AllowCustomerReviews,AllowCustomerRatings,Published,SKU,ManufacturerPartNumber,IsDownload,DownloadID,UnlimitedDownloads,MaxNumberOfDownloads,HasSampleDownload,SampleDownloadID,IsShipEnabled,IsFreeShipping,AdditionalShippingCharge,IsTaxExempt,TaxCategoryID,ManageInventory,StockQuantity,MinStockQuantity,LowStockActivityID,NotifyAdminForQuantityBelow,OrderMinimumQuantity,OrderMaximumQuantity,DisableBuyButton,Price,OldPrice,Weight, Length, Width, Height, CreatedOn) VALUES (");
                        string name = p.Name;
                        if (name.Length > maxStringLength)
                            name = name.Substring(0, maxStringLength);
                        sb.Append('"'); sb.Append(name.Replace('"', '\'')); sb.Append("\",");
                        string shortDescription = p.ShortDescription;
                        if (shortDescription.Length > maxStringLength)
                            shortDescription = shortDescription.Substring(0, maxStringLength);
                        sb.Append('"'); sb.Append(shortDescription.Replace('"', '\'')); sb.Append("\",");
                        string fullDescription = p.FullDescription;
                        if (fullDescription.Length > maxStringLength)
                            fullDescription = fullDescription.Substring(0, maxStringLength);
                        sb.Append('"'); sb.Append(fullDescription.Replace('"', '\'')); sb.Append("\",");
                        sb.Append(p.ProductTypeID); sb.Append(",");
                        sb.Append(p.TemplateID); sb.Append(",");
                        sb.Append('"'); sb.Append(p.ShowOnHomePage); sb.Append("\",");
                        string metaKeywords = p.MetaKeywords;
                        if (metaKeywords.Length > maxStringLength)
                            metaKeywords = metaKeywords.Substring(0, maxStringLength);
                        sb.Append('"'); sb.Append(metaKeywords.Replace('"', '\'')); sb.Append("\",");
                        string metaDescription = p.MetaDescription;
                        if (metaDescription.Length > maxStringLength)
                            metaDescription = metaDescription.Substring(0, maxStringLength);
                        sb.Append('"'); sb.Append(metaDescription.Replace('"', '\'')); sb.Append("\",");
                        string metaTitle = p.MetaTitle;
                        if (metaTitle.Length > maxStringLength)
                            metaTitle = metaTitle.Substring(0, maxStringLength);
                        sb.Append('"'); sb.Append(metaTitle.Replace('"', '\'')); sb.Append("\",");
                        sb.Append('"'); sb.Append(p.AllowCustomerReviews); sb.Append("\",");
                        sb.Append('"'); sb.Append(p.AllowCustomerRatings); sb.Append("\",");
                        sb.Append('"'); sb.Append(p.Published); sb.Append("\",");
                        string SKU = pv.SKU;
                        if (SKU.Length > maxStringLength)
                            SKU = SKU.Substring(0, maxStringLength);
                        sb.Append('"'); sb.Append(SKU.Replace('"', '\'')); sb.Append("\",");
                        string manufacturerPartNumber = pv.ManufacturerPartNumber;
                        if (manufacturerPartNumber.Length > maxStringLength)
                            manufacturerPartNumber = manufacturerPartNumber.Substring(0, maxStringLength);
                        sb.Append('"'); sb.Append(manufacturerPartNumber.Replace('"', '\'')); sb.Append("\",");
                        sb.Append('"'); sb.Append(pv.IsDownload); sb.Append("\",");
                        sb.Append(pv.DownloadID); sb.Append(",");
                        sb.Append('"'); sb.Append(pv.UnlimitedDownloads); sb.Append("\",");
                        sb.Append(pv.MaxNumberOfDownloads); sb.Append(",");
                        sb.Append('"'); sb.Append(pv.HasSampleDownload); sb.Append("\",");
                        sb.Append(pv.SampleDownloadID); sb.Append(",");
                        sb.Append('"'); sb.Append(pv.IsShipEnabled); sb.Append("\",");
                        sb.Append('"'); sb.Append(pv.IsFreeShipping); sb.Append("\",");
                        sb.Append(pv.AdditionalShippingCharge); sb.Append(",");
                        sb.Append('"'); sb.Append(pv.IsTaxExempt); sb.Append("\",");
                        sb.Append(pv.TaxCategoryID); sb.Append(",");
                        sb.Append('"'); sb.Append(pv.ManageInventory); sb.Append("\",");
                        sb.Append(pv.StockQuantity); sb.Append(",");
                        sb.Append(pv.MinStockQuantity); sb.Append(",");
                        sb.Append(pv.LowStockActivityID); sb.Append(",");
                        sb.Append(pv.NotifyAdminForQuantityBelow); sb.Append(",");
                        sb.Append(pv.OrderMinimumQuantity); sb.Append(",");
                        sb.Append(pv.OrderMaximumQuantity); sb.Append(",");
                        sb.Append('"'); sb.Append(pv.DisableBuyButton); sb.Append("\",");
                        sb.Append(pv.Price); sb.Append(",");
                        sb.Append(pv.OldPrice); sb.Append(",");
                        sb.Append(pv.Weight); sb.Append(",");
                        sb.Append(pv.Length); sb.Append(",");
                        sb.Append(pv.Width); sb.Append(",");
                        sb.Append(pv.Height); sb.Append(",");
                        sb.Append('"'); sb.Append(pv.CreatedOn); sb.Append("\"");
                        sb.Append(")");

                        excelHelper.ExecuteCommand(sb.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Export order list to xml
        /// </summary>
        /// <param name="orders">Orders</param>
        /// <returns>Result in XML format</returns>
        public static string ExportOrdersToXML(OrderCollection orders)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter stringWriter = new StringWriter(sb);
            XmlWriter xmlWriter = new XmlTextWriter(stringWriter);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Orders");
            xmlWriter.WriteAttributeString("Version", SiteHelper.GetCurrentVersion());

            foreach (Order order in orders)
            {
                xmlWriter.WriteStartElement("Order");
                xmlWriter.WriteElementString("OrderID", null, order.OrderID.ToString());
                xmlWriter.WriteElementString("OrderGUID", null, order.OrderGUID.ToString());
                xmlWriter.WriteElementString("CustomerID", null, order.CustomerID.ToString());
                xmlWriter.WriteElementString("CustomerLanguageID", null, order.CustomerLanguageID.ToString());
                xmlWriter.WriteElementString("CustomerTaxDisplayTypeID", null, order.CustomerTaxDisplayTypeID.ToString());
                xmlWriter.WriteElementString("OrderSubtotalInclTax", null, order.OrderSubtotalInclTax.ToString());
                xmlWriter.WriteElementString("OrderSubtotalExclTax", null, order.OrderSubtotalExclTax.ToString());
                xmlWriter.WriteElementString("OrderShippingInclTax", null, order.OrderShippingInclTax.ToString());
                xmlWriter.WriteElementString("OrderShippingExclTax", null, order.OrderShippingExclTax.ToString());
                xmlWriter.WriteElementString("PaymentMethodAdditionalFeeInclTax", null, order.PaymentMethodAdditionalFeeInclTax.ToString());
                xmlWriter.WriteElementString("PaymentMethodAdditionalFeeExclTax", null, order.PaymentMethodAdditionalFeeExclTax.ToString());
                xmlWriter.WriteElementString("OrderTax", null, order.OrderTax.ToString());
                xmlWriter.WriteElementString("OrderTotal", null, order.OrderTotal.ToString());
                xmlWriter.WriteElementString("OrderDiscount", null, order.OrderDiscount.ToString());
                xmlWriter.WriteElementString("OrderSubtotalInclTaxInCustomerCurrency", null, order.OrderSubtotalInclTaxInCustomerCurrency.ToString());
                xmlWriter.WriteElementString("OrderSubtotalExclTaxInCustomerCurrency", null, order.OrderSubtotalExclTaxInCustomerCurrency.ToString());
                xmlWriter.WriteElementString("OrderShippingInclTaxInCustomerCurrency", null, order.OrderShippingInclTaxInCustomerCurrency.ToString());
                xmlWriter.WriteElementString("OrderShippingExclTaxInCustomerCurrency", null, order.OrderShippingExclTaxInCustomerCurrency.ToString());
                xmlWriter.WriteElementString("PaymentMethodAdditionalFeeInclTaxInCustomerCurrency", null, order.PaymentMethodAdditionalFeeInclTaxInCustomerCurrency.ToString());
                xmlWriter.WriteElementString("PaymentMethodAdditionalFeeExclTaxInCustomerCurrency", null, order.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency.ToString());
                xmlWriter.WriteElementString("OrderTaxInCustomerCurrency", null, order.OrderTaxInCustomerCurrency.ToString());
                xmlWriter.WriteElementString("OrderTotalInCustomerCurrency", null, order.OrderTotalInCustomerCurrency.ToString());
                xmlWriter.WriteElementString("CustomerCurrencyCode", null, order.CustomerCurrencyCode);
                xmlWriter.WriteElementString("OrderWeight", null, order.OrderWeight.ToString());
                xmlWriter.WriteElementString("AffiliateID", null, order.AffiliateID.ToString());
                xmlWriter.WriteElementString("OrderStatusID", null, order.OrderStatusID.ToString());
                xmlWriter.WriteElementString("AllowStoringCreditCardNumber", null, order.AllowStoringCreditCardNumber.ToString());
                xmlWriter.WriteElementString("CardType", null, order.CardType);
                xmlWriter.WriteElementString("CardName", null, order.CardName);
                xmlWriter.WriteElementString("CardNumber", null, order.CardNumber);
                xmlWriter.WriteElementString("MaskedCreditCardNumber", null, order.MaskedCreditCardNumber);
                xmlWriter.WriteElementString("CardCVV2", null, order.CardCVV2);
                xmlWriter.WriteElementString("CardExpirationMonth", null, order.CardExpirationMonth);
                xmlWriter.WriteElementString("CardExpirationYear", null, order.CardExpirationYear);
                xmlWriter.WriteElementString("PaymentMethodID", null, order.PaymentMethodID.ToString());
                xmlWriter.WriteElementString("PaymentMethodName", null, order.PaymentMethodName);
                xmlWriter.WriteElementString("AuthorizationTransactionID", null, order.AuthorizationTransactionID);
                xmlWriter.WriteElementString("AuthorizationTransactionCode", null, order.AuthorizationTransactionCode);
                xmlWriter.WriteElementString("AuthorizationTransactionResult", null, order.AuthorizationTransactionResult);
                xmlWriter.WriteElementString("CaptureTransactionID", null, order.CaptureTransactionID);
                xmlWriter.WriteElementString("CaptureTransactionResult", null, order.CaptureTransactionResult);
                xmlWriter.WriteElementString("PurchaseOrderNumber", null, order.PurchaseOrderNumber);
                xmlWriter.WriteElementString("PaymentStatusID", null, order.PaymentStatusID.ToString());
                xmlWriter.WriteElementString("BillingFirstName", null, order.BillingFirstName);
                xmlWriter.WriteElementString("BillingLastName", null, order.BillingLastName);
                xmlWriter.WriteElementString("BillingPhoneNumber", null, order.BillingPhoneNumber);
                xmlWriter.WriteElementString("BillingEmail", null, order.BillingEmail);
                xmlWriter.WriteElementString("BillingFaxNumber", null, order.BillingFaxNumber);
                xmlWriter.WriteElementString("BillingCompany", null, order.BillingCompany);
                xmlWriter.WriteElementString("BillingAddress1", null, order.BillingAddress1);
                xmlWriter.WriteElementString("BillingAddress2", null, order.BillingAddress2);
                xmlWriter.WriteElementString("BillingCity", null, order.BillingCity);
                xmlWriter.WriteElementString("BillingStateProvince", null, order.BillingStateProvince);
                xmlWriter.WriteElementString("BillingStateProvinceID", null, order.BillingStateProvinceID.ToString());
                xmlWriter.WriteElementString("BillingCountry", null, order.BillingCountry);
                xmlWriter.WriteElementString("BillingCountryID", null, order.BillingCountryID.ToString());
                xmlWriter.WriteElementString("BillingZipPostalCode", null, order.BillingZipPostalCode);
                xmlWriter.WriteElementString("ShippingStatusID", null, order.ShippingStatusID.ToString());
                xmlWriter.WriteElementString("ShippingFirstName", null, order.ShippingFirstName);
                xmlWriter.WriteElementString("ShippingLastName", null, order.ShippingLastName);
                xmlWriter.WriteElementString("ShippingPhoneNumber", null, order.ShippingPhoneNumber);
                xmlWriter.WriteElementString("ShippingEmail", null, order.ShippingEmail);
                xmlWriter.WriteElementString("ShippingFaxNumber", null, order.ShippingFaxNumber);
                xmlWriter.WriteElementString("ShippingCompany", null, order.ShippingCompany);
                xmlWriter.WriteElementString("ShippingAddress1", null, order.ShippingAddress1);
                xmlWriter.WriteElementString("ShippingAddress2", null, order.ShippingAddress2);
                xmlWriter.WriteElementString("ShippingCity", null, order.ShippingCity);
                xmlWriter.WriteElementString("ShippingStateProvince", null, order.ShippingStateProvince);
                xmlWriter.WriteElementString("ShippingStateProvinceID", null, order.ShippingStateProvinceID.ToString());
                xmlWriter.WriteElementString("ShippingCountry", null, order.ShippingCountry);
                xmlWriter.WriteElementString("ShippingCountryID", null, order.ShippingCountryID.ToString());
                xmlWriter.WriteElementString("ShippingZipPostalCode", null, order.ShippingZipPostalCode);
                xmlWriter.WriteElementString("ShippingMethod", null, order.ShippingMethod);
                xmlWriter.WriteElementString("ShippingRateComputationMethodID", null, order.ShippingRateComputationMethodID.ToString());
                xmlWriter.WriteElementString("ShippedDate", null, (order.ShippedDate == null) ? string.Empty : order.ShippedDate.Value.ToString());
                xmlWriter.WriteElementString("Deleted", null, order.Deleted.ToString());
                xmlWriter.WriteElementString("CreatedOn", null, order.CreatedOn.ToString());

                OrderProductVariantCollection orderProductVariants = order.OrderProductVariants;
                if (orderProductVariants.Count > 0)
                {
                    xmlWriter.WriteStartElement("OrderProductVariants");
                    foreach (OrderProductVariant orderProductVariant in orderProductVariants)
                    {
                        xmlWriter.WriteStartElement("OrderProductVariant");
                        xmlWriter.WriteElementString("OrderProductVariantID", null, orderProductVariant.OrderProductVariantID.ToString());
                        xmlWriter.WriteElementString("ProductVariantID", null, orderProductVariant.ProductVariantID.ToString());

                        ProductVariant productVariant = orderProductVariant.ProductVariant;
                        if (productVariant != null)
                            xmlWriter.WriteElementString("ProductVariantName", null, productVariant.FullProductName);


                        xmlWriter.WriteElementString("UnitPriceInclTax", null, orderProductVariant.UnitPriceInclTax.ToString());
                        xmlWriter.WriteElementString("UnitPriceExclTax", null, orderProductVariant.UnitPriceExclTax.ToString());
                        xmlWriter.WriteElementString("PriceInclTax", null, orderProductVariant.PriceInclTax.ToString());
                        xmlWriter.WriteElementString("PriceExclTax", null, orderProductVariant.PriceExclTax.ToString());
                        xmlWriter.WriteElementString("UnitPriceInclTaxInCustomerCurrency", null, orderProductVariant.UnitPriceInclTaxInCustomerCurrency.ToString());
                        xmlWriter.WriteElementString("UnitPriceExclTaxInCustomerCurrency", null, orderProductVariant.UnitPriceExclTaxInCustomerCurrency.ToString());
                        xmlWriter.WriteElementString("PriceInclTaxInCustomerCurrency", null, orderProductVariant.PriceInclTaxInCustomerCurrency.ToString());
                        xmlWriter.WriteElementString("PriceExclTaxInCustomerCurrency", null, orderProductVariant.PriceExclTaxInCustomerCurrency.ToString());
                        xmlWriter.WriteElementString("AttributeDescription", null, orderProductVariant.AttributeDescription);
                        xmlWriter.WriteElementString("Quantity", null, orderProductVariant.Quantity.ToString());
                        xmlWriter.WriteElementString("DiscountAmountInclTax", null, orderProductVariant.DiscountAmountInclTax.ToString());
                        xmlWriter.WriteElementString("DiscountAmountExclTax", null, orderProductVariant.DiscountAmountExclTax.ToString());
                        xmlWriter.WriteElementString("DownloadCount", null, orderProductVariant.DownloadCount.ToString());
                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            return stringWriter.ToString();
        }

        /// <summary>
        /// Export orders to XLS
        /// </summary>
        /// <param name="FilePath">File path to use</param>
        /// <param name="orders">Orders</param>
        public static void ExportOrdersToXLS(string FilePath, OrderCollection orders)
        {
            using (ExcelHelper excelHelper = new ExcelHelper(FilePath))
            {
                excelHelper.HDR = "YES";
                excelHelper.IMEX = "0";
                Dictionary<string, string> tableDefinition = new Dictionary<string, string>();
                tableDefinition.Add("OrderID", "int");
                tableDefinition.Add("OrderGUID", "uniqueidentifier");
                tableDefinition.Add("OrderSubtotalInclTax", "decimal");
                tableDefinition.Add("OrderSubtotalExclTax", "decimal");
                tableDefinition.Add("OrderShippingInclTax", "decimal");
                tableDefinition.Add("OrderShippingExclTax", "decimal");
                tableDefinition.Add("PaymentMethodAdditionalFeeInclTax", "decimal");
                tableDefinition.Add("PaymentMethodAdditionalFeeExclTax", "decimal");
                tableDefinition.Add("OrderTax", "decimal");
                tableDefinition.Add("OrderTotal", "decimal");
                tableDefinition.Add("OrderDiscount", "decimal");
                tableDefinition.Add("OrderSubtotalInclTaxInCustomerCurrency", "decimal");
                tableDefinition.Add("OrderSubtotalExclTaxInCustomerCurrency", "decimal");
                tableDefinition.Add("OrderShippingInclTaxInCustomerCurrency", "decimal");
                tableDefinition.Add("OrderShippingExclTaxInCustomerCurrency", "decimal");
                tableDefinition.Add("PaymentMethodAdditionalFeeInclTaxInCustomerCurrency", "decimal");
                tableDefinition.Add("PaymentMethodAdditionalFeeExclTaxInCustomerCurrency", "decimal");
                tableDefinition.Add("OrderTaxInCustomerCurrency", "decimal");
                tableDefinition.Add("OrderTotalInCustomerCurrency", "decimal");
                tableDefinition.Add("CustomerCurrencyCode", "nvarchar(5)");
                tableDefinition.Add("OrderWeight", "decimal");
                tableDefinition.Add("AffiliateID", "int");
                tableDefinition.Add("OrderStatusID", "int");
                tableDefinition.Add("PaymentMethodID", "int");
                tableDefinition.Add("PaymentMethodName", "nvarchar(100)");
                tableDefinition.Add("PurchaseOrderNumber", "nvarchar(100)");
                tableDefinition.Add("PaymentStatusID", "int");
                tableDefinition.Add("BillingFirstName", "nvarchar(100)");
                tableDefinition.Add("BillingLastName", "nvarchar(100)");
                tableDefinition.Add("BillingPhoneNumber", "nvarchar(50)");
                tableDefinition.Add("BillingEmail", "nvarchar(255)");
                tableDefinition.Add("BillingFaxNumber", "nvarchar(50)");
                tableDefinition.Add("BillingCompany", "nvarchar(100)");
                tableDefinition.Add("BillingAddress1", "nvarchar(100)");
                tableDefinition.Add("BillingAddress2", "nvarchar(100)");
                tableDefinition.Add("BillingCity", "nvarchar(100)");
                tableDefinition.Add("BillingStateProvince", "nvarchar(100)");
                tableDefinition.Add("BillingZipPostalCode", "nvarchar(100)");
                tableDefinition.Add("BillingCountry", "nvarchar(100)");
                tableDefinition.Add("ShippingStatusID", "int");
                tableDefinition.Add("ShippingFirstName", "nvarchar(100)");
                tableDefinition.Add("ShippingLastName", "nvarchar(100)");
                tableDefinition.Add("ShippingPhoneNumber", "nvarchar(50)");
                tableDefinition.Add("ShippingEmail", "nvarchar(255)");
                tableDefinition.Add("ShippingFaxNumber", "nvarchar(50)");
                tableDefinition.Add("ShippingCompany", "nvarchar(100)");
                tableDefinition.Add("ShippingAddress1", "nvarchar(100)");
                tableDefinition.Add("ShippingAddress2", "nvarchar(100)");
                tableDefinition.Add("ShippingCity", "nvarchar(100)");
                tableDefinition.Add("ShippingStateProvince", "nvarchar(100)");
                tableDefinition.Add("ShippingZipPostalCode", "nvarchar(100)");
                tableDefinition.Add("ShippingCountry", "nvarchar(100)");
                tableDefinition.Add("ShippingMethod", "nvarchar(100)");
                tableDefinition.Add("ShippingRateComputationMethodID", "int");
                tableDefinition.Add("CreatedOn", "datetime");
                excelHelper.WriteTable("Orders", tableDefinition);

                foreach (Order order in orders)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("INSERT INTO [Orders] (OrderID, OrderGUID, OrderSubtotalInclTax, OrderSubtotalExclTax, OrderShippingInclTax, OrderShippingExclTax, PaymentMethodAdditionalFeeInclTax, PaymentMethodAdditionalFeeExclTax, OrderTax, OrderTotal, OrderDiscount, OrderSubtotalInclTaxInCustomerCurrency, OrderSubtotalExclTaxInCustomerCurrency, OrderShippingInclTaxInCustomerCurrency, OrderShippingExclTaxInCustomerCurrency, PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, PaymentMethodAdditionalFeeExclTaxInCustomerCurrency, OrderTaxInCustomerCurrency, OrderTotalInCustomerCurrency, CustomerCurrencyCode, OrderWeight, AffiliateID, OrderStatusID, PaymentMethodID, PaymentMethodName, PurchaseOrderNumber, PaymentStatusID, BillingFirstName, BillingLastName, BillingPhoneNumber, BillingEmail, BillingFaxNumber, BillingCompany, BillingAddress1, BillingAddress2, BillingCity, BillingStateProvince, BillingZipPostalCode, BillingCountry, ShippingStatusID,  ShippingFirstName, ShippingLastName, ShippingPhoneNumber, ShippingEmail, ShippingFaxNumber, ShippingCompany,  ShippingAddress1, ShippingAddress2, ShippingCity, ShippingStateProvince, ShippingZipPostalCode, ShippingCountry, ShippingMethod, ShippingRateComputationMethodID, CreatedOn) VALUES (");


                    sb.Append(order.OrderID); sb.Append(",");
                    sb.Append('"'); sb.Append(order.OrderGUID); sb.Append("\",");
                    sb.Append(order.OrderSubtotalInclTax); sb.Append(",");
                    sb.Append(order.OrderSubtotalExclTax); sb.Append(",");
                    sb.Append(order.OrderShippingInclTax); sb.Append(",");
                    sb.Append(order.OrderShippingExclTax); sb.Append(",");
                    sb.Append(order.PaymentMethodAdditionalFeeInclTax); sb.Append(",");
                    sb.Append(order.PaymentMethodAdditionalFeeExclTax); sb.Append(",");
                    sb.Append(order.OrderTax); sb.Append(",");
                    sb.Append(order.OrderTotal); sb.Append(",");
                    sb.Append(order.OrderDiscount); sb.Append(",");
                    sb.Append(order.OrderSubtotalInclTaxInCustomerCurrency); sb.Append(",");
                    sb.Append(order.OrderSubtotalExclTaxInCustomerCurrency); sb.Append(",");
                    sb.Append(order.OrderShippingInclTaxInCustomerCurrency); sb.Append(",");
                    sb.Append(order.OrderShippingExclTaxInCustomerCurrency); sb.Append(",");
                    sb.Append(order.PaymentMethodAdditionalFeeInclTaxInCustomerCurrency); sb.Append(",");
                    sb.Append(order.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency); sb.Append(",");
                    sb.Append(order.OrderTaxInCustomerCurrency); sb.Append(",");
                    sb.Append(order.OrderTotalInCustomerCurrency); sb.Append(",");
                    sb.Append('"'); sb.Append(order.CustomerCurrencyCode.Replace('"', '\'')); sb.Append("\",");
                    sb.Append(order.OrderWeight); sb.Append(",");
                    sb.Append(order.AffiliateID); sb.Append(",");
                    sb.Append(order.OrderStatusID); sb.Append(",");
                    sb.Append(order.PaymentMethodID); sb.Append(",");
                    sb.Append('"'); sb.Append(order.PaymentMethodName.Replace('"', '\'')); sb.Append("\",");
                    sb.Append('"'); sb.Append(order.PurchaseOrderNumber.Replace('"', '\'')); sb.Append("\",");
                    sb.Append(order.PaymentStatusID); sb.Append(",");
                    sb.Append('"'); sb.Append(order.BillingFirstName.Replace('"', '\'')); sb.Append("\",");
                    sb.Append('"'); sb.Append(order.BillingLastName.Replace('"', '\'')); sb.Append("\",");
                    sb.Append('"'); sb.Append(order.BillingPhoneNumber.Replace('"', '\'')); sb.Append("\",");
                    sb.Append('"'); sb.Append(order.BillingEmail.Replace('"', '\'')); sb.Append("\",");
                    sb.Append('"'); sb.Append(order.BillingFaxNumber.Replace('"', '\'')); sb.Append("\",");
                    sb.Append('"'); sb.Append(order.BillingCompany.Replace('"', '\'')); sb.Append("\",");
                    sb.Append('"'); sb.Append(order.BillingAddress1.Replace('"', '\'')); sb.Append("\",");
                    sb.Append('"'); sb.Append(order.BillingAddress2.Replace('"', '\'')); sb.Append("\",");
                    sb.Append('"'); sb.Append(order.BillingCity.Replace('"', '\'')); sb.Append("\",");
                    sb.Append('"'); sb.Append(order.BillingStateProvince.Replace('"', '\'')); sb.Append("\",");
                    sb.Append('"'); sb.Append(order.BillingZipPostalCode.Replace('"', '\'')); sb.Append("\",");
                    sb.Append('"'); sb.Append(order.BillingCountry.Replace('"', '\'')); sb.Append("\",");
                    sb.Append(order.ShippingStatusID); sb.Append(",");
                    sb.Append('"'); sb.Append(order.ShippingFirstName.Replace('"', '\'')); sb.Append("\",");
                    sb.Append('"'); sb.Append(order.ShippingLastName.Replace('"', '\'')); sb.Append("\",");
                    sb.Append('"'); sb.Append(order.ShippingPhoneNumber.Replace('"', '\'')); sb.Append("\",");
                    sb.Append('"'); sb.Append(order.ShippingEmail.Replace('"', '\'')); sb.Append("\",");
                    sb.Append('"'); sb.Append(order.ShippingFaxNumber.Replace('"', '\'')); sb.Append("\",");
                    sb.Append('"'); sb.Append(order.ShippingCompany.Replace('"', '\'')); sb.Append("\",");
                    sb.Append('"'); sb.Append(order.ShippingAddress1.Replace('"', '\'')); sb.Append("\",");
                    sb.Append('"'); sb.Append(order.ShippingAddress2.Replace('"', '\'')); sb.Append("\",");
                    sb.Append('"'); sb.Append(order.ShippingCity.Replace('"', '\'')); sb.Append("\",");
                    sb.Append('"'); sb.Append(order.ShippingStateProvince.Replace('"', '\'')); sb.Append("\",");
                    sb.Append('"'); sb.Append(order.ShippingZipPostalCode.Replace('"', '\'')); sb.Append("\",");
                    sb.Append('"'); sb.Append(order.BillingCountry.Replace('"', '\'')); sb.Append("\",");
                    sb.Append('"'); sb.Append(order.ShippingMethod.Replace('"', '\'')); sb.Append("\",");
                    sb.Append(order.ShippingRateComputationMethodID); sb.Append(",");
                    sb.Append('"'); sb.Append(order.CreatedOn); sb.Append("\"");
                    sb.Append(")");

                    excelHelper.ExecuteCommand(sb.ToString());
                }
            }
        }

        /// <summary>
        /// Export message tokens to xml
        /// </summary>
        /// <returns>Result in XML format</returns>
        public static string ExportMessageTokensToXML()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter stringWriter = new StringWriter(sb);
            XmlWriter xmlWriter = new XmlTextWriter(stringWriter);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Tokens");
            string[] allowedTokens = MessageManager.GetListOfAllowedTokens();
            for (int i = 0; i < allowedTokens.Length; i++)
            {
                string token = allowedTokens[i];
                string tokenName = token.Replace("%", "");
                xmlWriter.WriteStartElement("Token");
                xmlWriter.WriteAttributeString("name", tokenName);
                xmlWriter.WriteAttributeString("value", token);
                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndElement();
            xmlWriter.Close();
            return stringWriter.ToString();
        }

        #endregion
    }
}
