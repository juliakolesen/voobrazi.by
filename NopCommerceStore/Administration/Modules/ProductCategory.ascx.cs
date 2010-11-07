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
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic.Categories;
using NopSolutions.NopCommerce.BusinessLogic.Manufacturers;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Products.Specs;
using NopSolutions.NopCommerce.BusinessLogic.Templates;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Web.Administration.Modules;

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class ProductCategoryControl : BaseNopAdministrationUserControl
    {
        private void BindData()
        {
            Product product = ProductManager.GetProductByID(this.ProductID);
            if (product != null)
            {
                ProductCategoryCollection existingProductCategoryCollection = product.ProductCategories;
                List<ProductCategoryMappingHelperClass> productCategoryMappings = GetProductCategoryMappings(0, string.Empty, existingProductCategoryCollection);
                gvCategoryMappings.DataSource = productCategoryMappings;
                gvCategoryMappings.DataBind();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.BindData();
            }
        }

        public void SaveInfo()
        {
            Product product = ProductManager.GetProductByID(this.ProductID);
            if (product != null)
            {
                foreach (GridViewRow row in gvCategoryMappings.Rows)
                {
                    CheckBox cbCategoryInfo = row.FindControl("cbCategoryInfo") as CheckBox;
                    HiddenField hfCategoryID = row.FindControl("hfCategoryID") as HiddenField;
                    HiddenField hfProductCategoryID = row.FindControl("hfProductCategoryID") as HiddenField;
                    CheckBox cbFeatured = row.FindControl("cbFeatured") as CheckBox;
                    NumericTextBox txtRowDisplayOrder = row.FindControl("txtDisplayOrder") as NumericTextBox;
                    int productCategoryID = int.Parse(hfProductCategoryID.Value);
                    int categoryID = int.Parse(hfCategoryID.Value);
                    int displayOrder = txtRowDisplayOrder.Value;

                    if (productCategoryID > 0 && !cbCategoryInfo.Checked)
                        CategoryManager.DeleteProductCategory(productCategoryID);
                    if (productCategoryID > 0 && cbCategoryInfo.Checked)
                        CategoryManager.UpdateProductCategory(productCategoryID, product.ProductID, categoryID, cbFeatured.Checked, displayOrder);
                    if (productCategoryID == 0 && cbCategoryInfo.Checked)
                        CategoryManager.InsertProductCategory(product.ProductID, categoryID, cbFeatured.Checked, displayOrder);
                }
            }
        }

        public int ProductID
        {
            get
            {
                return CommonHelper.QueryStringInt("ProductID");
            }
        }

        private List<ProductCategoryMappingHelperClass> GetProductCategoryMappings(int ForParentCategoryID,
            string prefix, ProductCategoryCollection ExistingProductCategoryCollection)
        {
            CategoryCollection categoryCollection = CategoryManager.GetAllCategories(ForParentCategoryID);
            List<ProductCategoryMappingHelperClass> result = new List<ProductCategoryMappingHelperClass>();
            for (int i = 0; i < categoryCollection.Count; i++)
            {
                Category category = categoryCollection[i];
                ProductCategory existingProductCategory = ExistingProductCategoryCollection.FindProductCategory(this.ProductID, category.CategoryID);
                ProductCategoryMappingHelperClass pcm = new ProductCategoryMappingHelperClass();
                if (existingProductCategory != null)
                {
                    pcm.ProductCategoryID = existingProductCategory.ProductCategoryID;
                    pcm.IsMapped = true;
                    pcm.IsFeatured = existingProductCategory.IsFeaturedProduct;
                    pcm.DisplayOrder = existingProductCategory.DisplayOrder;
                }
                else
                {
                    pcm.DisplayOrder = 1;
                }
                pcm.CategoryID = category.CategoryID;
                pcm.CategoryInfo = prefix + category.Name;
                //if (pcm.CategoryID == this.CategoryID)
                //    pcm.IsMapped = true;
                result.Add(pcm);
                if (CategoryManager.GetAllCategories(category.CategoryID).Count > 0)
                    result.AddRange(GetProductCategoryMappings(category.CategoryID, prefix + "--", ExistingProductCategoryCollection));
            }

            return result;
        }
        
        private class ProductCategoryMappingHelperClass
        {
            public int ProductCategoryID { get; set; }
            public int CategoryID { get; set; }
            public string CategoryInfo { get; set; }
            public bool IsMapped { get; set; }
            public bool IsFeatured { get; set; }
            public int DisplayOrder { get; set; }
        }
    }
}