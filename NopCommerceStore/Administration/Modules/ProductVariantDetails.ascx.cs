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
    public partial class ProductVariantDetailsControl : BaseNopAdministrationUserControl
    {
        private void BindData()
        {
            ProductVariant productVariant = ProductManager.GetProductVariantByID(this.ProductVariantID);
            if (productVariant != null)
            {
                Product product = productVariant.Product;
                if (product == null)
                    Response.Redirect("Products.aspx");
                lblProductName.Text = Server.HtmlEncode(product.Name);
                hlProductURL.NavigateUrl = CommonHelper.GetStoreAdminLocation() + "ProductDetails.aspx?ProductID=" + product.ProductID.ToString();
            }
            else
                Response.Redirect("Products.aspx");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.SelectTab(this.ProductVariantTabs, this.TabID);
                this.BindData();
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    ProductVariant productVariant = ctrlProductVariantInfo.SaveInfo();
                    ctrlProductVariantTierPrices.SaveInfo();
                    ctrlProductVariantAttributes.SaveInfo();
                    ctrlProductVariantDiscounts.SaveInfo();
                    Response.Redirect(string.Format("ProductVariantDetails.aspx?ProductVariantID={0}&TabID={1}", productVariant.ProductVariantID, this.GetActiveTabID(this.ProductVariantTabs)));
                }
                catch (Exception exc)
                {
                    ProcessException(exc);
                }
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                ProductVariant productVariant = ProductManager.GetProductVariantByID(this.ProductVariantID);
                if (productVariant != null)
                {
                    ProductManager.MarkProductVariantAsDeleted(productVariant.ProductVariantID);
                    string url = CommonHelper.GetStoreAdminLocation() + "ProductDetails.aspx?ProductID=" + productVariant.Product.ProductID.ToString();
                    Response.Redirect(url);
                }
                else
                { 
                    Response.Redirect("Products.aspx");
                }
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        public int ProductVariantID
        {
            get
            {
                return CommonHelper.QueryStringInt("ProductVariantID");
            }
        }

        protected string TabID
        {
            get
            {
                return CommonHelper.QueryString("TabID");
            }
        }
    }
}