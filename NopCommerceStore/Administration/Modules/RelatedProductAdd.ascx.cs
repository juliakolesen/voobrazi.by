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
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic.Manufacturers;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.Common;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class RelatedProductAddControl : BaseNopAdministrationUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                FillDropDowns();
            }
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
                null, null, productName, false, 1000, 0, null, out totalRecords);
            return products;
        }

        protected void BindGrid()
        {
            ProductCollection products = GetProducts();
            if (products.Count > 0)
            {
                this.gvProducts.Visible = true;
                this.btnSave.Visible = true;
                this.lblNoProductsFound.Visible = false;

                this.gvProducts.DataSource = products;
                this.gvProducts.DataBind();
            }
            else
            {
                this.gvProducts.Visible = false;
                this.btnSave.Visible = false;
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Product product = ProductManager.GetProductByID(this.ProductID);
            if (product != null)
            {
                RelatedProductCollection existingRelatedProducts = product.RelatedProducts;

                foreach (GridViewRow row in gvProducts.Rows)
                {
                    try
                    {
                        CheckBox cbProductInfo = row.FindControl("cbProductInfo") as CheckBox;
                        HiddenField hfProductID = row.FindControl("hfProductID") as HiddenField;
                        NumericTextBox txtRowDisplayOrder = row.FindControl("txtDisplayOrder") as NumericTextBox;
                        int productID = int.Parse(hfProductID.Value);
                        int displayOrder = txtRowDisplayOrder.Value;
                        if (cbProductInfo.Checked)
                        {
                            if (existingRelatedProducts.FindRelatedProduct(this.ProductID, productID) == null)
                            {
                                ProductManager.InsertRelatedProduct(this.ProductID, productID, displayOrder);
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        ProcessException(exc);
                    }
                }
            }

            this.Page.ClientScript.RegisterStartupScript(typeof(RelatedProductAddControl), "closerefresh", "<script language=javascript>try {window.opener.document.forms[0]." + this.BtnID + ".click();}catch (e){} window.close();</script>");
        }

        protected void gvProducts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProducts.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        private string BtnID
        {
            get
            {
                object obj2 = base.Request.QueryString["BtnID"];
                if (obj2 == null)
                {
                    return string.Empty;
                }
                return obj2.ToString();
            }
        }

        public int ProductID
        {
            get
            {
                object obj2 = base.Request.QueryString["pid"];
                if (obj2 == null)
                {
                    return 0;
                }
                return int.Parse(obj2.ToString());
            }
        }
    }
}