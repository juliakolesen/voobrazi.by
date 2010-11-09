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
using System.IO;
using System.Xml;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Localization;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Tax;

namespace NopSolutions.NopCommerce.BusinessLogic.ExportImport
{
    /// <summary>
    /// Import manager
    /// </summary>
    public partial class ImportManager
    {
        #region Utilities
        private static Dictionary<string, string> GetResources(string content)
        {
            Dictionary<string, string> resources = new Dictionary<string, string>();

            StringReader _sr = new StringReader(content);
            XmlTextReader _xtr = new XmlTextReader(_sr);
            _xtr.XmlResolver = null;
            _xtr.WhitespaceHandling = WhitespaceHandling.None;
            _xtr.Read();

            if ((_xtr.NodeType == XmlNodeType.Element) && (_xtr.Name == "Language"))
            {
                while (_xtr.Read())
                {
                    if ((_xtr.NodeType == XmlNodeType.Element) && (_xtr.Name == "LocaleResource"))
                    {
                        string resourceName = _xtr["Name"];

                        _xtr.Read();
                        if ((_xtr.NodeType == XmlNodeType.Element) && (_xtr.Name == "Value"))
                        {
                            _xtr.Read();
                            if (_xtr.NodeType == XmlNodeType.Text)
                            {
                                string resourceValue = _xtr.Value;
                                resources.Add(resourceName, resourceValue);
                            }
                        }
                    }
                }
            }

            return resources;
        }

        private static int SaveResourceStrings(int LanguageID, Dictionary<string, string> resources)
        {
            int total = 0;
            foreach (KeyValuePair<string, string> kvp in resources)
            {
                string resourceName = kvp.Key;
                string resourceValue = kvp.Value;
                LocaleStringResourceManager.InsertLocaleStringResource(LanguageID, resourceName, resourceValue);
                total++;
            }

            return total;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Import locale resources
        /// </summary>
        /// <param name="LanguageID">Language identifier</param>
        /// <param name="content">Recources in XML format</param>
        /// <returns>Impotred resources count</returns>
        public static int ImportResources(int LanguageID, string content)
        {
            int totalLocalStringRecources = 0;
            Dictionary<string, string> resources = GetResources(content);
            totalLocalStringRecources = SaveResourceStrings(LanguageID, resources);
            return totalLocalStringRecources;
        }

        /// <summary>
        /// Import customer list from XLS file
        /// </summary>
        /// <param name="FilePath">Excel file path</param>
        public static void ImportCustomersFromXLS(string FilePath)
        {
            using (ExcelHelper excelHelper = new ExcelHelper(FilePath))
            {
                excelHelper.HDR = "YES";
                excelHelper.IMEX = "1";

                DataTable dt = excelHelper.ReadTable("Customers");
                foreach (DataRow dr in dt.Rows)
                {
                    int customerID = Convert.ToInt32(dr["CustomerID"]);
                    Guid customerGUID = new Guid(dr["CustomerGUID"].ToString());
                    string email = dr["Email"].ToString();
                    string username = dr["Username"].ToString();
                    string passwordHash = dr["PasswordHash"].ToString();
                    string saltKey = dr["SaltKey"].ToString();
                    int affiliateID = Convert.ToInt32(dr["AffiliateID"]);
                    int billingAddressID = Convert.ToInt32(dr["BillingAddressID"]);
                    int shippingAddressID = Convert.ToInt32(dr["ShippingAddressID"]);
                    int lastPaymentMethodID = Convert.ToInt32(dr["LastPaymentMethodID"]);
                    string lastAppliedCouponCode = dr["LastAppliedCouponCode"].ToString();
                    int languageID = Convert.ToInt32(dr["LanguageID"]);
                    int currencyID = Convert.ToInt32(dr["CurrencyID"]);
                    int taxDisplayTypeID = Convert.ToInt32(dr["TaxDisplayTypeID"]);
                    bool isTaxExempt = Convert.ToBoolean(dr["IsTaxExempt"]);
                    bool isAdmin = Convert.ToBoolean(dr["IsAdmin"]);
                    bool isGuest = Convert.ToBoolean(dr["IsGuest"]);
                    bool isForumModerator = Convert.ToBoolean(dr["IsForumModerator"]);
                    int totalForumPosts = Convert.ToInt32(dr["TotalForumPosts"]);
                    string signature = dr["Signature"].ToString();
                    string adminComment = dr["AdminComment"].ToString();
                    bool active = Convert.ToBoolean(dr["Active"]);
                    bool deleted = Convert.ToBoolean(dr["Deleted"]);
                    DateTime registrationDate = Convert.ToDateTime(dr["RegistrationDate"]);
                    string timeZoneID = dr["TimeZoneID"].ToString();
                    int avatarID = Convert.ToInt32(dr["AvatarID"]);

                    //custom properties
                    string gender = dr["Gender"].ToString();
                    string firstName = dr["FirstName"].ToString();
                    string lastName = dr["LastName"].ToString();
                    string company = dr["Company"].ToString();
                    string streetAddress = dr["StreetAddress"].ToString();
                    string streetAddress2 = dr["StreetAddress2"].ToString();
                    string zipPostalCode = dr["ZipPostalCode"].ToString();
                    string city = dr["City"].ToString();
                    string phoneNumber = dr["PhoneNumber"].ToString();
                    string faxNumber = dr["FaxNumber"].ToString();
                    int countryID = Convert.ToInt32(dr["CountryID"]);
                    int stateProvinceID = Convert.ToInt32(dr["StateProvinceID"]);
                    bool receiveNewsletter = Convert.ToBoolean(dr["ReceiveNewsletter"]);


                    Customer customer = CustomerManager.GetCustomerByEmail(email);
                    if (customer == null)
                    {
                        //no customers found
                        customer = CustomerManager.AddCustomerForced(customerGUID, email, username,
                            passwordHash, saltKey, affiliateID, billingAddressID, shippingAddressID, lastPaymentMethodID,
                            lastAppliedCouponCode, languageID, currencyID, (TaxDisplayTypeEnum)taxDisplayTypeID, isTaxExempt,
                            isAdmin, isGuest, isForumModerator, totalForumPosts, signature,
                            adminComment, active, deleted, registrationDate, timeZoneID, avatarID);
                    }
                    else
                    {
                        if (!customer.IsGuest)
                        {
                            //customer is not a guest
                            customer = CustomerManager.UpdateCustomer(customer.CustomerID, customer.CustomerGUID,
                                email, username, passwordHash, saltKey, affiliateID, billingAddressID,
                                shippingAddressID, lastPaymentMethodID, lastAppliedCouponCode, languageID, currencyID,
                                (TaxDisplayTypeEnum)taxDisplayTypeID, isTaxExempt, isAdmin, isGuest,
                                isForumModerator, totalForumPosts, signature, adminComment,
                                active, deleted, registrationDate, timeZoneID, avatarID);
                        }
                        else
                        {
                            //customer is a guest
                            customer = CustomerManager.GetCustomerByGUID(customerGUID);
                            if (customer == null)
                            {
                                customer = CustomerManager.AddCustomerForced(customerGUID, email, username,
                                    passwordHash, saltKey, affiliateID, billingAddressID, shippingAddressID, lastPaymentMethodID,
                                    lastAppliedCouponCode, languageID, currencyID, (TaxDisplayTypeEnum)taxDisplayTypeID, isTaxExempt,
                                    isAdmin, isGuest, isForumModerator, totalForumPosts, signature,
                                    adminComment, active, deleted, registrationDate, timeZoneID, avatarID);
                            }
                            else
                            {
                                customer = CustomerManager.UpdateCustomer(customer.CustomerID, customer.CustomerGUID,
                                    email, username, passwordHash, saltKey, affiliateID, billingAddressID,
                                    shippingAddressID, lastPaymentMethodID, lastAppliedCouponCode, languageID, currencyID,
                                    (TaxDisplayTypeEnum)taxDisplayTypeID, isTaxExempt, isAdmin, isGuest,
                                    isForumModerator, totalForumPosts, signature, adminComment,
                                    active, deleted, registrationDate, timeZoneID, avatarID);
                            }
                        }
                    }
                    customer.Gender = gender;
                    customer.FirstName = firstName;
                    customer.LastName = lastName;
                    customer.Company = company;
                    customer.StreetAddress = streetAddress;
                    customer.StreetAddress2 = streetAddress2;
                    customer.ZipPostalCode = zipPostalCode;
                    customer.City = city;
                    customer.PhoneNumber = phoneNumber;
                    customer.FaxNumber = faxNumber;
                    customer.CountryID = countryID;
                    customer.StateProvinceID = stateProvinceID;
                    customer.ReceiveNewsletter = receiveNewsletter;
                }
            }
        }

        /// <summary>
        /// Import products from XLS file
        /// </summary>
        /// <param name="FilePath">Excel file path</param>
        public static void ImportProductsFromXLS(string FilePath)
        {
            using (ExcelHelper excelHelper = new ExcelHelper(FilePath))
            {
                excelHelper.HDR = "YES";
                excelHelper.IMEX = "1";

                DataTable dt = excelHelper.ReadTable("Products");
                foreach (DataRow dr in dt.Rows)
                {
                    string Name = dr["Name"].ToString();
                    string ShortDescription = dr["ShortDescription"].ToString();
                    string FullDescription = dr["FullDescription"].ToString();
                    int ProductTypeID = Convert.ToInt32(dr["ProductTypeID"]);
                    int TemplateID = Convert.ToInt32(dr["TemplateID"]);
                    bool ShowOnHomePage = Convert.ToBoolean(dr["ShowOnHomePage"]);
                    string MetaKeywords = dr["MetaKeywords"].ToString();
                    string MetaDescription = dr["MetaDescription"].ToString();
                    string MetaTitle = dr["MetaTitle"].ToString();
                    bool AllowCustomerReviews = Convert.ToBoolean(dr["AllowCustomerReviews"]);
                    bool AllowCustomerRatings = Convert.ToBoolean(dr["AllowCustomerRatings"]);
                    bool Published = Convert.ToBoolean(dr["Published"]);
                    string SKU = dr["SKU"].ToString();
                    string ManufacturerPartNumber = dr["ManufacturerPartNumber"].ToString();
                    bool IsDownload = Convert.ToBoolean(dr["IsDownload"]);
                    int DownloadID = Convert.ToInt32(dr["DownloadID"]);
                    bool UnlimitedDownloads = Convert.ToBoolean(dr["UnlimitedDownloads"]);
                    int MaxNumberOfDownloads = Convert.ToInt32(dr["MaxNumberOfDownloads"]);
                    bool HasSampleDownload = Convert.ToBoolean(dr["HasSampleDownload"]);
                    int SampleDownloadID = Convert.ToInt32(dr["SampleDownloadID"]);
                    bool IsShipEnabled = Convert.ToBoolean(dr["IsShipEnabled"]);
                    bool IsFreeShipping = Convert.ToBoolean(dr["IsFreeShipping"]);
                    decimal AdditionalShippingCharge = Convert.ToDecimal(dr["AdditionalShippingCharge"]);
                    bool IsTaxExempt = Convert.ToBoolean(dr["IsTaxExempt"]);
                    int TaxCategoryID = Convert.ToInt32(dr["TaxCategoryID"]);
                    bool ManageInventory = Convert.ToBoolean(dr["ManageInventory"]);
                    int StockQuantity = Convert.ToInt32(dr["StockQuantity"]);
                    int MinStockQuantity = Convert.ToInt32(dr["MinStockQuantity"]);
                    int LowStockActivityID = Convert.ToInt32(dr["LowStockActivityID"]);
                    int NotifyAdminForQuantityBelow = Convert.ToInt32(dr["NotifyAdminForQuantityBelow"]);
                    int OrderMinimumQuantity = Convert.ToInt32(dr["OrderMinimumQuantity"]);
                    int OrderMaximumQuantity = Convert.ToInt32(dr["OrderMaximumQuantity"]);
                    bool DisableBuyButton = Convert.ToBoolean(dr["DisableBuyButton"]);
                    decimal Price = Convert.ToDecimal(dr["Price"]);
                    decimal OldPrice = Convert.ToDecimal(dr["OldPrice"]);
                    decimal Weight = Convert.ToDecimal(dr["Weight"]);
                    decimal Length = Convert.ToDecimal(dr["Length"]);
                    decimal Width = Convert.ToDecimal(dr["Width"]);
                    decimal Height = Convert.ToDecimal(dr["Height"]);
                    DateTime CreatedOn = Convert.ToDateTime(dr["CreatedOn"]);

                    Product product = ProductManager.InsertProduct(Name, ShortDescription, FullDescription,
                        string.Empty, ProductTypeID, TemplateID, ShowOnHomePage, MetaKeywords, MetaDescription,
                        MetaTitle, string.Empty, AllowCustomerReviews, AllowCustomerRatings, 0, 0,
                        Published, false, CreatedOn, DateTime.Now);

                    ProductVariant productVariant = ProductManager.InsertProductVariant(product.ProductID,
                        string.Empty, SKU, string.Empty, string.Empty, ManufacturerPartNumber, IsDownload, DownloadID,
                        UnlimitedDownloads, MaxNumberOfDownloads, HasSampleDownload, SampleDownloadID,
                        IsShipEnabled, IsFreeShipping, AdditionalShippingCharge, IsTaxExempt,
                        TaxCategoryID, ManageInventory, StockQuantity, MinStockQuantity,
                        (LowStockActivityEnum)LowStockActivityID, NotifyAdminForQuantityBelow,
                        OrderMinimumQuantity, OrderMaximumQuantity, 0, DisableBuyButton, Price, OldPrice, Weight, Length, Width, Height, 0, null, null,
                        true, false, 1, CreatedOn, DateTime.Now);
                }
            }
        }
        #endregion
    }
}
