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
using NopSolutions.NopCommerce.BusinessLogic.Localization;
using NopSolutions.NopCommerce.BusinessLogic.Manufacturers;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class ProductsControl : BaseNopAdministrationUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                FillDropDowns();
                BindGrid();
            }
        }

        protected void gvProducts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvProducts.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void FillDropDowns()
        {
            ParentCategory.EmptyItemText = GetLocaleResourceString("Admin.Common.All");
            ParentCategory.BindData();

            this.ddlManufacturer.Items.Clear();
            ListItem itemEmptyManufacturer = new ListItem(GetLocaleResourceString("Admin.Common.All"), "0");
            this.ddlManufacturer.Items.Add(itemEmptyManufacturer);
            ManufacturerCollection manufacturers = ManufacturerManager.GetAllManufacturers();
            foreach (Manufacturer manufacturer in manufacturers)
            {
                ListItem item2 = new ListItem(manufacturer.Name, manufacturer.ManufacturerID.ToString());
                this.ddlManufacturer.Items.Add(item2);
            }
        }

        protected ProductCollection GetProducts()
        {
            string productName = txtProductName.Text;
            int categoryID = ParentCategory.SelectedCategoryId;
            int manufacturerID = int.Parse(this.ddlManufacturer.SelectedItem.Value);

            int totalRecords = 0;            
            ProductCollection products = ProductManager.GetAllProducts(categoryID, manufacturerID, null,
                null, null, productName, false, int.MaxValue, 0, null, out totalRecords);
            return products;
        }

        protected void BindGrid()
        {
            ProductCollection products = GetProducts();
            if (products.Count > 0)
            {
                this.gvProducts.Visible = true;
                this.lblNoProductsFound.Visible = false;
                this.gvProducts.DataSource = products;
                this.gvProducts.DataBind();
            }
            else
            {
                this.gvProducts.Visible = false;
                this.lblNoProductsFound.Visible = true;
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    BindGrid();
                }
                catch (Exception exc)
                {
                    ProcessException(exc);
                }
            }
        }

        protected void btnExportXML_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    string fileName = string.Format("products_{0}.xml", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    
                    ProductCollection products = GetProducts();
                    string xml = ExportManager.ExportProductsToXML(products);
                    CommonHelper.WriteResponseXML(xml, fileName);
                }
                catch (Exception exc)
                {
                    ProcessException(exc);
                }
            }
        }
        
        protected void btnExportXLS_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    string fileName = string.Format("products_{0}.xls", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    string filePath = string.Format("{0}files\\ExportImport\\{1}", HttpContext.Current.Request.PhysicalApplicationPath, fileName);
                    ProductCollection products = GetProducts();

                    ExportManager.ExportProductsToXLS(filePath, products);
                    CommonHelper.WriteResponseXLS(filePath, fileName);
                }
                catch (Exception exc)
                {
                    ProcessException(exc);
                }
            }
        }

        protected void btnImportXLS_Click(object sender, EventArgs e)
        {
            if (fuXlsFile.PostedFile != null && !String.IsNullOrEmpty(fuXlsFile.FileName))
            {
                try
                {
                    byte[] fileBytes = fuXlsFile.FileBytes;
                    string extension = "xls";
                    if (fuXlsFile.FileName.EndsWith("xlsx"))
                        extension = "xlsx";

                    string fileName = string.Format("products_{0}.{1}", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"), extension);
                    string filePath = string.Format("{0}files\\ExportImport\\{1}", HttpContext.Current.Request.PhysicalApplicationPath, fileName);

                    File.WriteAllBytes(filePath, fileBytes);
                    ImportManager.ImportProductsFromXLS(filePath);
                }
                catch (Exception ex)
                {
                    ProcessException(ex);
                }
            }
        }
    }
}