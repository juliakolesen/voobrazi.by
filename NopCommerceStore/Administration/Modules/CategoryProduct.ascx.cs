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
using NopSolutions.NopCommerce.BusinessLogic.Categories;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Promo.Discounts;
using NopSolutions.NopCommerce.BusinessLogic.Templates;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Web.Administration.Modules;
 

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class CategoryProductControl : BaseNopAdministrationUserControl
    {
        private void BindData()
        {
            Category category = CategoryManager.GetCategoryByID(this.CategoryID);

            if (category != null)
            {
                ProductCategoryCollection existingProductCategoryCollection = category.ProductCategories;
                List<ProductCategoryMappingHelperClass> productCategoryMappings = GetProductCategoryMappings(existingProductCategoryCollection);
                gvProductCategoryMappings.DataSource = productCategoryMappings;
                gvProductCategoryMappings.DataBind();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnAddNew.OnClientClick = string.Format("javascript:OpenWindow('CategoryProductAdd.aspx?cid={0}&BtnID={1}', 800, 600, true); return false;", this.CategoryID, btnRefresh.ClientID);
            
            if (!Page.IsPostBack)
            {
                this.BindData();
            }
        }

        public void SaveInfo()
        {
            Category category = CategoryManager.GetCategoryByID(this.CategoryID);

            if (category != null)
            {
                foreach (GridViewRow row in gvProductCategoryMappings.Rows)
                {
                    CheckBox cbProductInfo = row.FindControl("cbProductInfo") as CheckBox;
                    HiddenField hfProductID = row.FindControl("hfProductID") as HiddenField;
                    HiddenField hfProductCategoryID = row.FindControl("hfProductCategoryID") as HiddenField;
                    CheckBox cbFeatured = row.FindControl("cbFeatured") as CheckBox;
                    NumericTextBox txtRowDisplayOrder = row.FindControl("txtDisplayOrder") as NumericTextBox;

                    int productID = int.Parse(hfProductID.Value);
                    int productCategoryID = int.Parse(hfProductCategoryID.Value);
                    bool featured = cbFeatured.Checked;
                    int displayOrder = txtRowDisplayOrder.Value;

                    if (productCategoryID > 0 && !cbProductInfo.Checked)
                        CategoryManager.DeleteProductCategory(productCategoryID);
                    if (productCategoryID > 0 && cbProductInfo.Checked)
                        CategoryManager.UpdateProductCategory(productCategoryID, productID, category.CategoryID, featured, displayOrder);
                }
            }
        }

        protected void gvProductCategoryMappings_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProductCategoryMappings.PageIndex = e.NewPageIndex;
            BindData();
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            this.BindData();
        }

        private List<ProductCategoryMappingHelperClass> GetProductCategoryMappings(ProductCategoryCollection ExistingProductCategoryCollection)
        {
            List<ProductCategoryMappingHelperClass> result = new List<ProductCategoryMappingHelperClass>();
            foreach (ProductCategory pc in ExistingProductCategoryCollection)
            {
                Product product = pc.Product;
                if (product != null)
                {
                    ProductCategoryMappingHelperClass pcmhc = new ProductCategoryMappingHelperClass();
                    pcmhc.ProductCategoryID = pc.ProductCategoryID;
                    pcmhc.ProductID = pc.ProductID;
                    pcmhc.ProductInfo = product.Name;
                    pcmhc.IsMapped = true;
                    pcmhc.IsFeatured = pc.IsFeaturedProduct;
                    pcmhc.DisplayOrder = pc.DisplayOrder;
                    result.Add(pcmhc);
                }
            }

            return result;
        }

        [Serializable]
        private class ProductCategoryMappingHelperClass
        {
            public int ProductCategoryID { get; set; }
            public int ProductID { get; set; }
            public string ProductInfo { get; set; }
            public bool IsMapped { get; set; }
            public bool IsFeatured { get; set; }
            public int DisplayOrder { get; set; }
        }

        public int CategoryID
        {
            get
            {
                return CommonHelper.QueryStringInt("CategoryID");
            }
        }
    }
}