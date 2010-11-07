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
    public partial class RelatedProductsControl : BaseNopAdministrationUserControl
    {
        private void BindData()
        {
            Product product = ProductManager.GetProductByID(this.ProductID);
            if (product != null)
            {
                RelatedProductCollection existingRelatedProductCollection = product.RelatedProducts;
                List<RelatedProductHelperClass> relatedProducts = GetRelatedProducts(existingRelatedProductCollection);
                gvRelatedProducts.DataSource = relatedProducts;
                gvRelatedProducts.DataBind();
            }
        }

        private List<RelatedProductHelperClass> GetRelatedProducts(RelatedProductCollection ExistingRelatedProductCollection)
        {
            List<RelatedProductHelperClass> result = new List<RelatedProductHelperClass>();
            foreach (RelatedProduct relatedProduct in ExistingRelatedProductCollection)
            {
                Product product = relatedProduct.Product2;
                if (product != null)
                {
                    RelatedProductHelperClass rphc = new RelatedProductHelperClass();
                    rphc.RelatedProductID = relatedProduct.RelatedProductID;
                    rphc.ProductID2 = product.ProductID;
                    rphc.ProductInfo2 = product.Name;
                    rphc.IsMapped = true;
                    rphc.DisplayOrder = relatedProduct.DisplayOrder;
                    result.Add(rphc);
                }
            }

            return result;
        }

        [Serializable]
        private class RelatedProductHelperClass
        {
            public int RelatedProductID { get; set; }
            public int ProductID2 { get; set; }
            public string ProductInfo2 { get; set; }
            public bool IsMapped { get; set; }
            public int DisplayOrder { get; set; }
        }

        public void SaveInfo()
        {
            Product product = ProductManager.GetProductByID(this.ProductID);
            if (product != null)
            {
                foreach (GridViewRow row in gvRelatedProducts.Rows)
                {
                    CheckBox cbProductInfo2 = row.FindControl("cbProductInfo2") as CheckBox;
                    HiddenField hfProductID2 = row.FindControl("hfProductID2") as HiddenField;
                    HiddenField hfRelatedProductID = row.FindControl("hfRelatedProductID") as HiddenField;
                    NumericTextBox txtRowDisplayOrder = row.FindControl("txtDisplayOrder") as NumericTextBox;
                    int relatedProductID = int.Parse(hfRelatedProductID.Value);
                    int productID2 = int.Parse(hfProductID2.Value);
                    int displayOrder = txtRowDisplayOrder.Value;

                    if (relatedProductID > 0 && !cbProductInfo2.Checked)
                        ProductManager.DeleteRelatedProduct(relatedProductID);
                    if (relatedProductID > 0 && cbProductInfo2.Checked)
                        ProductManager.UpdateRelatedProduct(relatedProductID, product.ProductID, productID2, displayOrder);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnAddNew.OnClientClick = string.Format("javascript:OpenWindow('RelatedProductAdd.aspx?pid={0}&BtnID={1}', 800, 600, true); return false;", this.ProductID, btnRefresh.ClientID);
            
            if (!Page.IsPostBack)
            {
                this.BindData();
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            this.BindData();
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