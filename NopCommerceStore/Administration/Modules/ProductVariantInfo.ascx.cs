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
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.ExportImport;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Products.Attributes;
using NopSolutions.NopCommerce.BusinessLogic.Promo.Discounts;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Warehouses;
using NopSolutions.NopCommerce.Web.Administration.Modules;


namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class ProductVariantInfoControl : BaseNopAdministrationUserControl
    {
        private void BindData()
        {
            ProductVariant productVariant = ProductManager.GetProductVariantByID(this.ProductVariantID);
            if (productVariant != null)
            {
                Product product = productVariant.Product;
                if (product == null)
                    Response.Redirect("Products.aspx");

                this.txtName.Text = productVariant.Name;
                this.txtSKU.Text = productVariant.SKU;
                this.txtDescription.Value = productVariant.Description;
                this.txtAdminComment.Text = productVariant.AdminComment;
                this.txtManufacturerPartNumber.Text = productVariant.ManufacturerPartNumber;

                //downloable products
                this.cbIsDownload.Checked = productVariant.IsDownload;
                Download productVariantDownload = productVariant.Download;
                if (productVariantDownload != null)
                {
                    this.cbUseDownloadURL.Checked = productVariantDownload.UseDownloadURL;
                    this.txtDownloadURL.Text = productVariantDownload.DownloadURL;

                    if (productVariantDownload.DownloadBinary != null)
                    {
                        btnRemoveProductVariantDownload.Visible = true;
                        hlProductVariantDownload.Visible = true;
                        string adminDownloadUrl = DownloadManager.GetAdminDownloadUrl(productVariantDownload);
                        hlProductVariantDownload.NavigateUrl = adminDownloadUrl;
                    }
                    else
                    {
                        btnRemoveProductVariantDownload.Visible = false;
                        hlProductVariantDownload.Visible = false;
                    }
                }
                else
                {
                    btnRemoveProductVariantDownload.Visible = false;
                    hlProductVariantDownload.Visible = false;
                }

                this.cbUnlimitedDownloads.Checked = productVariant.UnlimitedDownloads;
                this.txtMaxNumberOfDownloads.Value = productVariant.MaxNumberOfDownloads;
                this.cbHasSampleDownload.Checked = productVariant.HasSampleDownload;
                Download productVariantSampleDownload = productVariant.SampleDownload;
                if (productVariantSampleDownload != null)
                {
                    this.cbUseSampleDownloadURL.Checked = productVariantSampleDownload.UseDownloadURL;
                    this.txtSampleDownloadURL.Text = productVariantSampleDownload.DownloadURL;

                    if (productVariantSampleDownload.DownloadBinary != null)
                    {
                        btnRemoveProductVariantSampleDownload.Visible = true;
                        hlProductVariantSampleDownload.Visible = true;
                        string adminSampleDownloadUrl = DownloadManager.GetAdminDownloadUrl(productVariantSampleDownload);
                        hlProductVariantSampleDownload.NavigateUrl = adminSampleDownloadUrl;
                    }
                    else
                    {
                        btnRemoveProductVariantSampleDownload.Visible = false;
                        hlProductVariantSampleDownload.Visible = false;
                    }
                }
                else
                {
                    btnRemoveProductVariantSampleDownload.Visible = false;
                    hlProductVariantSampleDownload.Visible = false;
                }

                //shipping
                this.cbIsShipEnabled.Checked = productVariant.IsShipEnabled;
                this.cbIsFreeShipping.Checked = productVariant.IsFreeShipping;
                this.txtAdditionalShippingCharge.Value = productVariant.AdditionalShippingCharge;

                //tax
                this.cbIsTaxExempt.Checked = productVariant.IsTaxExempt;
                CommonHelper.SelectListItem(this.ddlTaxCategory, productVariant.TaxCategoryID);

                //stock management
                this.cbManageStock.Checked = productVariant.ManageInventory;
                this.txtStockQuantity.Value = productVariant.StockQuantity;
                this.txtMinStockQuantity.Value = productVariant.MinStockQuantity;
                CommonHelper.SelectListItem(this.ddlLowStockActivity, productVariant.LowStockActivityID);
                this.txtNotifyForQuantityBelow.Value = productVariant.NotifyAdminForQuantityBelow;
                this.txtOrderMinimumQuantity.Value = productVariant.OrderMinimumQuantity;
                this.txtOrderMaximumQuantity.Value = productVariant.OrderMaximumQuantity;

                //other settings
                CommonHelper.SelectListItem(this.ddlWarehouse, productVariant.WarehouseId);
                this.cbDisableBuyButton.Checked = productVariant.DisableBuyButton;
                this.txtPrice.Value = productVariant.Price;
                this.txtOldPrice.Value = productVariant.OldPrice;
                this.txtWeight.Value = productVariant.Weight;
                this.txtLength.Value = productVariant.Length;
                this.txtWidth.Value = productVariant.Width;
                this.txtHeight.Value = productVariant.Height;

                Picture productVariantPicture = productVariant.Picture;
                btnRemoveProductVariantImage.Visible = productVariantPicture != null;
                string pictureUrl = PictureManager.GetPictureUrl(productVariantPicture, 100);
                this.iProductVariantPicture.Visible = true;
                this.iProductVariantPicture.ImageUrl = pictureUrl;

                if (productVariant.AvailableStartDateTime.HasValue)
                {
                    this.cAvailableStartDateTimeButtonExtender.SelectedDate = productVariant.AvailableStartDateTime.Value;
                }
                if (productVariant.AvailableEndDateTime.HasValue)
                {
                    this.cAvailableEndDateTimeButtonExtender.SelectedDate = productVariant.AvailableEndDateTime.Value;
                }

                this.cbPublished.Checked = productVariant.Published;
                this.txtDisplayOrder.Value = productVariant.DisplayOrder;
            }
            else
            {
                this.btnRemoveProductVariantImage.Visible = false;
                this.iProductVariantPicture.Visible = false;

                btnRemoveProductVariantDownload.Visible = false;
                hlProductVariantDownload.Visible = false;

                btnRemoveProductVariantSampleDownload.Visible = false;
                hlProductVariantSampleDownload.Visible = false;

                Product product = ProductManager.GetProductByID(this.ProductID);
                if (product == null)
                    Response.Redirect("Products.aspx");
            }
        }

        private void FillDropDowns()
        {
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
        {//downloadable products
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
                this.BindData();
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

        public ProductVariant SaveInfo()
        {
            DateTime nowDT = DateTime.Now;

            string name = txtName.Text.Trim();
            string sku = txtSKU.Text.Trim();
            string description = txtDescription.Value.Trim();
            string adminComment = txtAdminComment.Text.Trim();
            string manufacturerPartNumber = txtManufacturerPartNumber.Text.Trim();
            bool isDownload = cbIsDownload.Checked;
            bool unlimitedDownloads = cbUnlimitedDownloads.Checked;
            int maxNumberOfDownloads = txtMaxNumberOfDownloads.Value;
            bool hasSampleDownload = cbHasSampleDownload.Checked;
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
            bool published = cbPublished.Checked;
            int displayOrder = txtDisplayOrder.Value;

            ProductVariant productVariant = ProductManager.GetProductVariantByID(ProductVariantID);
            if (productVariant != null)
            {
                #region Update
                Picture productVariantPicture = productVariant.Picture;
                HttpPostedFile productVariantPictureFile = fuProductVariantPicture.PostedFile;
                if ((productVariantPictureFile != null) && (!String.IsNullOrEmpty(productVariantPictureFile.FileName)))
                {
                    byte[] productVariantPictureBinary = PictureManager.GetPictureBits(productVariantPictureFile.InputStream, productVariantPictureFile.ContentLength);
                    if (productVariantPicture != null)
                        productVariantPicture = PictureManager.UpdatePicture(productVariantPicture.PictureID, productVariantPictureBinary, productVariantPictureFile.ContentType, true);
                    else
                        productVariantPicture = PictureManager.InsertPicture(productVariantPictureBinary, productVariantPictureFile.ContentType, true);
                }
                int productVariantPictureID = 0;
                if (productVariantPicture != null)
                    productVariantPictureID = productVariantPicture.PictureID;

                int productVariantDownloadID = 0;
                if (isDownload)
                {
                    Download productVariantDownload = productVariant.Download;
                    bool useDownloadURL = cbUseDownloadURL.Checked;
                    string downloadURL = txtDownloadURL.Text.Trim();
                    byte[] productVariantDownloadBinary = null;
                    string downloadContentType = string.Empty;
                    string downloadExtension = string.Empty;
                    if (productVariantDownload != null)
                    {
                        productVariantDownloadBinary = productVariantDownload.DownloadBinary;
                        downloadContentType = productVariantDownload.ContentType;
                        downloadExtension = productVariantDownload.Extension;
                    }

                    HttpPostedFile productVariantDownloadFile = fuProductVariantDownload.PostedFile;
                    if ((productVariantDownloadFile != null) && (!String.IsNullOrEmpty(productVariantDownloadFile.FileName)))
                    {
                        productVariantDownloadBinary = DownloadManager.GetDownloadBits(productVariantDownloadFile.InputStream, productVariantDownloadFile.ContentLength);
                        downloadContentType = productVariantDownloadFile.ContentType;
                        downloadExtension = Path.GetExtension(productVariantDownloadFile.FileName);
                    }

                    if (productVariantDownload != null)
                    {
                        productVariantDownload = DownloadManager.UpdateDownload(productVariantDownload.DownloadID, useDownloadURL, downloadURL, productVariantDownloadBinary, downloadContentType, downloadExtension, true);
                    }
                    else
                    {
                        productVariantDownload = DownloadManager.InsertDownload(useDownloadURL, downloadURL, productVariantDownloadBinary, downloadContentType, downloadExtension, true);
                    }
                    
                    productVariantDownloadID = productVariantDownload.DownloadID;
                }

                int productVariantSampleDownloadID = 0;
                if (hasSampleDownload)
                {
                    Download productVariantSampleDownload = productVariant.SampleDownload;
                    bool useSampleDownloadURL = cbUseSampleDownloadURL.Checked;
                    string sampleDownloadURL = txtSampleDownloadURL.Text.Trim();
                    byte[] productVariantSampleDownloadBinary = null;
                    string sampleDownloadContentType = string.Empty;
                    string sampleDownloadExtension = string.Empty;
                    if (productVariantSampleDownload != null)
                    {
                        productVariantSampleDownloadBinary = productVariantSampleDownload.DownloadBinary;
                        sampleDownloadContentType = productVariantSampleDownload.ContentType;
                        sampleDownloadExtension = productVariantSampleDownload.Extension;
                    }

                    HttpPostedFile productVariantSampleDownloadFile = fuProductVariantSampleDownload.PostedFile;
                    if ((productVariantSampleDownloadFile != null) && (!String.IsNullOrEmpty(productVariantSampleDownloadFile.FileName)))
                    {
                        productVariantSampleDownloadBinary = DownloadManager.GetDownloadBits(productVariantSampleDownloadFile.InputStream, productVariantSampleDownloadFile.ContentLength);
                        sampleDownloadContentType = productVariantSampleDownloadFile.ContentType;
                        sampleDownloadExtension = Path.GetExtension(productVariantSampleDownloadFile.FileName);
                    }

                    if (productVariantSampleDownload != null)
                    {
                        productVariantSampleDownload = DownloadManager.UpdateDownload(productVariantSampleDownload.DownloadID, useSampleDownloadURL, sampleDownloadURL, productVariantSampleDownloadBinary, sampleDownloadContentType, sampleDownloadExtension, true);
                    }
                    else
                    {
                        productVariantSampleDownload = DownloadManager.InsertDownload(useSampleDownloadURL, sampleDownloadURL, productVariantSampleDownloadBinary, sampleDownloadContentType, sampleDownloadExtension, true);
                    }

                    productVariantSampleDownloadID = productVariantSampleDownload.DownloadID;
                }

                productVariant = ProductManager.UpdateProductVariant(ProductVariantID,
                    productVariant.ProductID, name, sku, description, adminComment, manufacturerPartNumber,
                    isDownload, productVariantDownloadID, unlimitedDownloads,
                    maxNumberOfDownloads, hasSampleDownload, productVariantSampleDownloadID,
                    isShipEnabled, isFreeShipping, additionalShippingCharge,
                    isTaxExempt, taxCategoryID, manageStock, stockQuantity,
                    minStockQuantity, lowStockActivity, notifyForQuantityBelow,
                    orderMinimumQuantity, orderMaximumQuantity,
                    warehouseID, disableBuyButton, price, oldPrice, weight, length,
                    width, height, productVariantPictureID,
                    availableStartDateTime, availableEndDateTime, published,
                    productVariant.Deleted, displayOrder, productVariant.CreatedOn, nowDT);
                #endregion
            }
            else
            {
                #region Insert
                Product product = ProductManager.GetProductByID(this.ProductID);
                if (product != null)
                {
                    Picture productVariantPicture = null;
                    HttpPostedFile productVariantPictureFile = fuProductVariantPicture.PostedFile;
                    if ((productVariantPictureFile != null) && (!String.IsNullOrEmpty(productVariantPictureFile.FileName)))
                    {
                        byte[] productVariantPictureBinary = PictureManager.GetPictureBits(productVariantPictureFile.InputStream, productVariantPictureFile.ContentLength);
                        productVariantPicture = PictureManager.InsertPicture(productVariantPictureBinary, productVariantPictureFile.ContentType, true);
                    }
                    int productVariantPictureID = 0;
                    if (productVariantPicture != null)
                        productVariantPictureID = productVariantPicture.PictureID;

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

                    productVariant = ProductManager.InsertProductVariant(product.ProductID,
                         name, sku, description, adminComment, manufacturerPartNumber,
                         isDownload, productVariantDownloadID, unlimitedDownloads,
                         maxNumberOfDownloads, hasSampleDownload, productVariantSampleDownloadID,
                         isShipEnabled, isFreeShipping, additionalShippingCharge, isTaxExempt, taxCategoryID,
                         cbManageStock.Checked, txtStockQuantity.Value,
                         minStockQuantity, lowStockActivity, notifyForQuantityBelow,
                         orderMinimumQuantity, orderMaximumQuantity,
                         warehouseID, disableBuyButton, price, oldPrice, weight,
                         length, width, height, productVariantPictureID,
                         availableStartDateTime, availableEndDateTime, published,
                         false, displayOrder, nowDT, nowDT);
                }
                else
                {
                    Response.Redirect("Products.aspx");
                }
                #endregion
            }
            return productVariant;
        }

        protected void btnRemoveProductVariantImage_Click(object sender, EventArgs e)
        {
            try
            {
                ProductVariant productVariant = ProductManager.GetProductVariantByID(this.ProductVariantID);
                if (productVariant != null)
                {
                    PictureManager.DeletePicture(productVariant.PictureID);
                    ProductManager.RemoveProductVariantPicture(productVariant.ProductVariantID);
                    BindData();
                }
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        protected void btnRemoveProductVariantDownload_Click(object sender, EventArgs e)
        {
            try
            {
                ProductVariant productVariant = ProductManager.GetProductVariantByID(this.ProductVariantID);
                if (productVariant != null)
                {
                    Download download = productVariant.Download;
                    if (download != null)
                    {
                        download = DownloadManager.UpdateDownload(download.DownloadID,
                            download.UseDownloadURL, download.DownloadURL, null, 
                            string.Empty, string.Empty, true); 
                        //DownloadManager.DeleteDownload(productVariant.DownloadID);
                        //ProductManager.RemoveProductVariantDownload(productVariant.ProductVariantID);
                    }
                    BindData();
                }
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        protected void btnRemoveProductVariantSampleDownload_Click(object sender, EventArgs e)
        {
            try
            {
                ProductVariant productVariant = ProductManager.GetProductVariantByID(this.ProductVariantID);
                if (productVariant != null)
                {
                    Download download = productVariant.SampleDownload;
                    if (download != null)
                    {
                        download = DownloadManager.UpdateDownload(download.DownloadID,
                            download.UseDownloadURL, download.DownloadURL, null,
                            string.Empty, string.Empty, true);
                        //DownloadManager.DeleteDownload(productVariant.DownloadID);
                        //ProductManager.RemoveProductVariantDownload(productVariant.ProductVariantID);
                    }
                    BindData();
                }
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }
        
        public int ProductID
        {
            get
            {
                return CommonHelper.QueryStringInt("ProductID");
            }
        }

        public int ProductVariantID
        {
            get
            {
                return CommonHelper.QueryStringInt("ProductVariantID");
            }
        }
    }
}