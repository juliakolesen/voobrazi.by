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
using System.Web.UI;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.BusinessLogic.Templates;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Common;
namespace NopSolutions.NopCommerce.Web
{
    public partial class ProductPage : BaseNopPage
    {
        Product product;

        private void CreateChildControlsTree()
        {
            product = ProductManager.GetProductByID(this.ProductID);
            if (product != null)
            {
                Control child;

                ProductTemplate productTemplate = product.ProductTemplate;
                if (productTemplate == null)
                    throw new NopException(string.Format("Product template path can not be empty. Product ID={0}", product.ProductID));

                child = LoadControl(productTemplate.TemplatePath);
                ProductPlaceHolder.Controls.Add(child);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            CreateChildControlsTree();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CommonHelper.EnsureNonSSL();

            	((MasterPages.TwoColumn) Master).Unique.Visible = false;
				((MasterPages.TwoColumn)Master).SalesLeader.Visible = false;
            }

            if (product == null || product.Deleted)
                Response.Redirect("~/Default.aspx");

            string title = CommonHelper.IIF(!string.IsNullOrEmpty(product.MetaTitle), product.MetaTitle, product.Name);
            SEOHelper.RenderTitle(this, title, true);
            SEOHelper.RenderMetaTag(this, "description", product.MetaDescription, true);
            SEOHelper.RenderMetaTag(this, "keywords", product.MetaKeywords, true);

            if (!Page.IsPostBack)
            {
                ProductManager.AddProductToRecentlyViewedList(product.ProductID);
                NopContext.Current.LastProductPageVisited = CommonHelper.GetThisPageURL(true);
            }
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