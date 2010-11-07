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
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic.Manufacturers;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Templates;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Web.Administration.Modules;

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class ManufacturerProductsControl : BaseNopAdministrationUserControl
    {
        private void BindData()
        {
            Manufacturer manufacturer = ManufacturerManager.GetManufacturerByID(this.ManufacturerID);
            if (manufacturer != null)
            {
                ProductManufacturerCollection existingProductManufacturerCollection = manufacturer.ProductManufacturers;
                List<ProductManufacturerMappingHelperClass> productManufacturerMappings = GetProductManufacturerMappings(existingProductManufacturerCollection);
                gvProductManufacturerMappings.DataSource = productManufacturerMappings;
                gvProductManufacturerMappings.DataBind();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnAddNew.OnClientClick = string.Format("javascript:OpenWindow('ManufacturerProductAdd.aspx?mid={0}&BtnID={1}', 800, 600, true); return false;", this.ManufacturerID, btnRefresh.ClientID);

            if (!Page.IsPostBack)
            {
                this.BindData();
            }
        }

        public void SaveInfo()
        {
            Manufacturer manufacturer = ManufacturerManager.GetManufacturerByID(this.ManufacturerID);

            if (manufacturer != null)
            {
                foreach (GridViewRow row in gvProductManufacturerMappings.Rows)
                {
                    CheckBox cbProductInfo = row.FindControl("cbProductInfo") as CheckBox;
                    HiddenField hfProductID = row.FindControl("hfProductID") as HiddenField;
                    HiddenField hfProductManufacturerID = row.FindControl("hfProductManufacturerID") as HiddenField;
                    CheckBox cbFeatured = row.FindControl("cbFeatured") as CheckBox;
                    NumericTextBox txtRowDisplayOrder = row.FindControl("txtDisplayOrder") as NumericTextBox;

                    int productID = int.Parse(hfProductID.Value);
                    int productManufacturerID = int.Parse(hfProductManufacturerID.Value);
                    bool featured = cbFeatured.Checked;
                    int displayOrder = txtRowDisplayOrder.Value;

                    if (productManufacturerID > 0 && !cbProductInfo.Checked)
                        ManufacturerManager.DeleteProductManufacturer(productManufacturerID);
                    if (productManufacturerID > 0 && cbProductInfo.Checked)
                        ManufacturerManager.UpdateProductManufacturer(productManufacturerID, productID, manufacturer.ManufacturerID, featured, displayOrder);
                }
            }
        }

        protected void gvProductManufacturerMappings_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProductManufacturerMappings.PageIndex = e.NewPageIndex;
            BindData();
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            this.BindData();
        }

        private List<ProductManufacturerMappingHelperClass> GetProductManufacturerMappings(ProductManufacturerCollection ExistingProductManufacturerCollection)
        {
            List<ProductManufacturerMappingHelperClass> result = new List<ProductManufacturerMappingHelperClass>();
            foreach (ProductManufacturer pm in ExistingProductManufacturerCollection)
            {
                Product product = pm.Product;
                if (product != null)
                {
                    ProductManufacturerMappingHelperClass pmmhc = new ProductManufacturerMappingHelperClass();
                    pmmhc.ProductManufacturerID = pm.ProductManufacturerID;
                    pmmhc.ProductID = pm.ProductID;
                    pmmhc.ProductInfo = product.Name;
                    pmmhc.IsMapped = true;
                    pmmhc.IsFeatured = pm.IsFeaturedProduct;
                    pmmhc.DisplayOrder = pm.DisplayOrder;
                    result.Add(pmmhc);
                }
            }

            return result;
        }

        [Serializable]
        private class ProductManufacturerMappingHelperClass
        {
            public int ProductManufacturerID { get; set; }
            public int ProductID { get; set; }
            public string ProductInfo { get; set; }
            public bool IsMapped { get; set; }
            public bool IsFeatured { get; set; }
            public int DisplayOrder { get; set; }
        }

        public int ManufacturerID
        {
            get
            {
                return CommonHelper.QueryStringInt("ManufacturerID");
            }
        }
    }
}