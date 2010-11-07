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
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.MobileControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Categories;
using NopSolutions.NopCommerce.BusinessLogic.Manufacturers;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Promo.Discounts;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.BusinessLogic.Templates;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Warehouses;
using NopSolutions.NopCommerce.Web.Administration.Modules;
 
namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class ProductInfoAddControl : BaseNopAdministrationUserControl
    {
        private void FillDropDowns()
        {
            this.ddlTemplate.Items.Clear();
            ProductTemplateCollection productTemplateCollection = TemplateManager.GetAllProductTemplates();
            foreach (ProductTemplate productTemplate in productTemplateCollection)
            {
                ListItem item2 = new ListItem(productTemplate.Name, productTemplate.ProductTemplateID.ToString());
                this.ddlTemplate.Items.Add(item2);
            }

            this.ddlProductType.Items.Clear();
            ProductTypeCollection productTypeCollection = ProductManager.GetAllProductTypes();
            foreach (ProductType productType in productTypeCollection)
            {
                ListItem item2 = new ListItem(productType.Name, productType.ProductTypeID.ToString());
                this.ddlProductType.Items.Add(item2);
            }

            this.ddlTaxCategory.Items.Clear();
            ListItem itemTaxCategory = new ListItem("---", "0");
            this.ddlTaxCategory.Items.Add(itemTaxCategory);
            TaxCategoryCollection taxCategoryCollection = TaxCategoryManager.GetAllTaxCategories();
            foreach (TaxCategory taxCategory in taxCategoryCollection)
            {
                ListItem item2 = new ListItem(taxCategory.Name, taxCategory.TaxCategoryID.ToString());
                this.ddlTaxCategory.Items.Add(item2);
            }

            this.ddlWarehouse.Items.Clear();
            ListItem itemWarehouse = new ListItem("---", "0");
            this.ddlWarehouse.Items.Add(itemWarehouse);
            WarehouseCollection warehouseCollection = WarehouseManager.GetAllWarehouses();
            foreach (Warehouse warehouse in warehouseCollection)
            {
                ListItem item2 = new ListItem(warehouse.Name, warehouse.WarehouseID.ToString());
                this.ddlWarehouse.Items.Add(item2);
            }

            CommonHelper.FillDropDownWithEnum(this.ddlLowStockActivity, typeof(LowStockActivityEnum));
        }

        private void TogglePanels()
        {
            //downloadable products
            if (cbIsDownload.Checked)
            {
                pnlUseDownloadURL.Visible = true;
                if (cbUseDownloadURL.Checked)
                {
                    pnlDownloadURL.Visible = true;
                    pnlDownloadFile.Visible = false;
                }
                else
                {
                    pnlDownloadURL.Visible = false;
                    pnlDownloadFile.Visible = true;
                }
                pnlUnlimitedDownloads.Visible = true;
                pnlMaxNumberOfDownloads.Visible = !cbUnlimitedDownloads.Checked; 
                pnlHasSampleDownload.Visible = true;
                if (cbHasSampleDownload.Checked)
                {
                    pnlUseSampleDownloadURL.Visible = true;
                    if (cbUseSampleDownloadURL.Checked)
                    {
                        pnlSampleDownloadURL.Visible = true;
                        pnlSampleDownloadFile.Visible = false;
                    }
                    else
                    {
                        pnlSampleDownloadURL.Visible = false;
                        pnlSampleDownloadFile.Visible = true;
                    }
                }
                else
                {
                    pnlUseSampleDownloadURL.Visible = false;
                    pnlSampleDownloadURL.Visible = false;
                    pnlSampleDownloadFile.Visible = false;
                }
            }
            else
            {
                pnlUseDownloadURL.Visible = false;
                pnlDownloadURL.Visible = false;
                pnlDownloadFile.Visible = false;
                pnlUnlimitedDownloads.Visible = false;
                pnlMaxNumberOfDownloads.Visible = false;
                pnlHasSampleDownload.Visible = false;
                pnlUseSampleDownloadURL.Visible = false;
                pnlSampleDownloadURL.Visible = false;
                pnlSampleDownloadFile.Visible = false;
            }

            //stock management
            pnlStockQuantity.Visible = cbManageStock.Checked;
            pnlMinStockQuantity.Visible = cbManageStock.Checked;
            pnlLowStockActivity.Visible = cbManageStock.Checked;
            pnlNotifyForQuantityBelow.Visible = cbManageStock.Checked;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.FillDropDowns();
                this.TogglePanels();
            }
        }

        protected void cbIsDownload_CheckedChanged(object sender, EventArgs e)
        {
            this.TogglePanels();
        }

        protected void cbUseDownloadURL_CheckedChanged(object sender, EventArgs e)
        {
            this.TogglePanels();
        }

        protected void cbUnlimitedDownloads_CheckedChanged(object sender, EventArgs e)
        {
            this.TogglePanels();
        }

        protected void cbHasSampleDownload_CheckedChanged(object sender, EventArgs e)
        {
            this.TogglePanels();
        }

        protected void cbUseSampleDownloadURL_CheckedChanged(object sender, EventArgs e)
        {
            this.TogglePanels();
        }

        protected void cbManageStock_CheckedChanged(object sender, EventArgs e)
        {
            this.TogglePanels();
        }

        public Product SaveInfo()
        {
            DateTime nowDT = DateTime.Now;

            string name = txtName.Text.Trim();
            string shortDescription = txtShortDescription.Text.Trim();
            string fullDescription = txtFullDescription.Value.Trim();
            string adminComment = txtAdminComment.Text.Trim();
            int productTypeID = int.Parse(this.ddlProductType.SelectedItem.Value);
            int templateID = int.Parse(this.ddlTemplate.SelectedItem.Value);
            bool showOnHomePage = cbShowOnHomePage.Checked;
            bool allowCustomerReviews = cbAllowCustomerReviews.Checked;
            bool allowCustomerRatings = cbAllowCustomerRatings.Checked;
            bool published = cbPublished.Checked;
            string sku = txtSKU.Text.Trim();
            string manufacturerPartNumber = txtManufacturerPartNumber.Text.Trim();
            bool isDownload = cbIsDownload.Checked;
            int productVariantDownloadID = 0;
            if (isDownload)
            {
                bool useDownloadURL = cbUseDownloadURL.Checked;
                string downloadURL = txtDownloadURL.Text.Trim();
                byte[] productVariantDownloadBinary = null;
                string downloadContentType = string.Empty;
                string downloadExtension = string.Empty;

                HttpPostedFile productVariantDownloadFile = fuProductVariantDownload.PostedFile;
                if ((productVariantDownloadFile != null) && (!String.IsNullOrEmpty(productVariantDownloadFile.FileName)))
                {
                    productVariantDownloadBinary = DownloadManager.GetDownloadBits(productVariantDownloadFile.InputStream, productVariantDownloadFile.ContentLength);
                    downloadContentType = productVariantDownloadFile.ContentType;
                    downloadExtension = Path.GetExtension(productVariantDownloadFile.FileName);
                }

                Download productVariantDownload = DownloadManager.InsertDownload(useDownloadURL, downloadURL, productVariantDownloadBinary, downloadContentType, downloadExtension, true);
                productVariantDownloadID = productVariantDownload.DownloadID;
            }

            bool unlimitedDownloads = cbUnlimitedDownloads.Checked;
            int maxNumberOfDownloads = txtMaxNumberOfDownloads.Value;

            bool hasSampleDownload = cbHasSampleDownload.Checked;
            int productVariantSampleDownloadID = 0;
            if (hasSampleDownload)
            {
                bool useSampleDownloadURL = cbUseSampleDownloadURL.Checked;
                string sampleDownloadURL = txtSampleDownloadURL.Text.Trim();
                byte[] productVariantSampleDownloadBinary = null;
                string sampleDownloadContentType = string.Empty;
                string sampleDownloadExtension = string.Empty;

                HttpPostedFile productVariantSampleDownloadFile = fuProductVariantSampleDownload.PostedFile;
                if ((productVariantSampleDownloadFile != null) && (!String.IsNullOrEmpty(productVariantSampleDownloadFile.FileName)))
                {
                    productVariantSampleDownloadBinary = DownloadManager.GetDownloadBits(productVariantSampleDownloadFile.InputStream, productVariantSampleDownloadFile.ContentLength);
                    sampleDownloadContentType = productVariantSampleDownloadFile.ContentType;
                    sampleDownloadExtension = Path.GetExtension(productVariantSampleDownloadFile.FileName);
                }

                Download productVariantSampleDownload = DownloadManager.InsertDownload(useSampleDownloadURL, sampleDownloadURL, productVariantSampleDownloadBinary, sampleDownloadContentType, sampleDownloadExtension, true);
                productVariantSampleDownloadID = productVariantSampleDownload.DownloadID;
            }

            bool isShipEnabled = cbIsShipEnabled.Checked;
            bool isFreeShipping = cbIsFreeShipping.Checked;
            decimal additionalShippingCharge = txtAdditionalShippingCharge.Value;
            bool isTaxExempt = cbIsTaxExempt.Checked;
            int taxCategoryID = int.Parse(this.ddlTaxCategory.SelectedItem.Value);
            bool manageStock = cbManageStock.Checked;
            int stockQuantity = txtStockQuantity.Value;
            int minStockQuantity = txtMinStockQuantity.Value;
            LowStockActivityEnum lowStockActivity = (LowStockActivityEnum)Enum.ToObject(typeof(LowStockActivityEnum), int.Parse(this.ddlLowStockActivity.SelectedItem.Value));
            int notifyForQuantityBelow = txtNotifyForQuantityBelow.Value;
            int orderMinimumQuantity = txtOrderMinimumQuantity.Value;
            int orderMaximumQuantity = txtOrderMaximumQuantity.Value;
            int warehouseID = int.Parse(this.ddlWarehouse.SelectedItem.Value);
            bool disableBuyButton = cbDisableBuyButton.Checked;
            decimal price = txtPrice.Value;
            decimal oldPrice = txtOldPrice.Value;
            decimal weight = txtWeight.Value;
            decimal length = txtLength.Value;
            decimal width = txtWidth.Value;
            decimal height = txtHeight.Value;
            DateTime? availableStartDateTime = null;
            DateTime? availableEndDateTime = null;
            DateTime availableStartDateTimeTmp = nowDT;
            if (DateTime.TryParse(txtAvailableStartDateTime.Text, out availableStartDateTimeTmp))
            {
                availableStartDateTime = DateTime.SpecifyKind(availableStartDateTimeTmp, DateTimeKind.Utc);
            }
            DateTime availableEndDateTimeTmp = nowDT;
            if (DateTime.TryParse(txtAvailableEndDateTime.Text, out availableEndDateTimeTmp))
            {
                availableEndDateTime = DateTime.SpecifyKind(availableEndDateTimeTmp, DateTimeKind.Utc);
            }

            Product product = ProductManager.InsertProduct(name, shortDescription, fullDescription,
                adminComment, productTypeID, templateID, showOnHomePage, string.Empty, string.Empty,
                string.Empty, string.Empty, allowCustomerReviews, allowCustomerRatings, 0, 0,
                 published, false, nowDT, nowDT);

            ProductVariant productVariant = ProductManager.InsertProductVariant(product.ProductID,
                 string.Empty, sku, string.Empty, string.Empty, manufacturerPartNumber,
                 isDownload, productVariantDownloadID, unlimitedDownloads,
                 maxNumberOfDownloads, hasSampleDownload, productVariantSampleDownloadID,
                 isShipEnabled, isFreeShipping, additionalShippingCharge, isTaxExempt,
                 taxCategoryID, manageStock, stockQuantity,
                 minStockQuantity, lowStockActivity, notifyForQuantityBelow,
                 orderMinimumQuantity, orderMaximumQuantity, warehouseID, disableBuyButton,
                 price, oldPrice, weight, length,
                 width, height, 0, availableStartDateTime, availableEndDateTime,
                 published, false, 1, nowDT, nowDT);

            return product;
        }

        public int ProductID
        {
            get
            {
                return CommonHelper.QueryStringInt("ProductID");
            }
        }
    }
}