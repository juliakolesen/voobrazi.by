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
    public partial class ProductSEOControl : BaseNopAdministrationUserControl
    {
        private void BindData()
        {
            Product product = ProductManager.GetProductByID(this.ProductID);
            if (product != null)
            {
                this.txtMetaKeywords.Text = product.MetaKeywords;
                this.txtMetaDescription.Text = product.MetaDescription;
                this.txtMetaTitle.Text = product.MetaTitle;
                this.txtSEName.Text = product.SEName;
            }
            else
            {
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
                product = ProductManager.UpdateProduct(product.ProductID, product.Name, product.ShortDescription,
                    product.FullDescription, product.AdminComment, product.ProductTypeID,
                    product.TemplateID, product.ShowOnHomePage, txtMetaKeywords.Text,
                    txtMetaDescription.Text, txtMetaTitle.Text, txtSEName.Text, 
                    product.AllowCustomerReviews,product.AllowCustomerRatings, product.RatingSum,
                    product.TotalRatingVotes, product.Published, product.Deleted, product.CreatedOn, DateTime.Now);
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