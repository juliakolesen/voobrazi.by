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
    public partial class ProductInfoControl : BaseNopAdministrationUserControl
    {
        private void BindData()
        {
            Product product = ProductManager.GetProductByID(this.ProductID);
            if (product != null)
            {
                this.txtName.Text = product.Name;
                this.txtShortDescription.Text = product.ShortDescription;
                this.txtFullDescription.Value = product.FullDescription;
                this.txtAdminComment.Text = product.AdminComment;
                CommonHelper.SelectListItem(this.ddlProductType, product.ProductTypeID);
                CommonHelper.SelectListItem(this.ddlTemplate, product.TemplateID);
                this.cbShowOnHomePage.Checked = product.ShowOnHomePage;

                this.cbAllowCustomerReviews.Checked = product.AllowCustomerReviews;
                this.cbAllowCustomerRatings.Checked = product.AllowCustomerRatings;
                this.cbPublished.Checked = product.Published;

                ProductReviewCollection productReviews = product.ProductReviews;
                if (productReviews.Count > 0)
                {
                    hlViewReviews.Visible = true;
                    hlViewReviews.Text = string.Format(GetLocaleResourceString("Admin.ProductInfo.AllowCustomerReviewsView"), productReviews.Count);
                    hlViewReviews.NavigateUrl = CommonHelper.GetStoreAdminLocation() + "ProductReviews.aspx?ProductID=" + product.ProductID.ToString();
                }
                else
                    hlViewReviews.Visible = false;
            }
        }

        private void FillDropDowns()
        {
            this.ddlTemplate.Items.Clear();
            ProductTemplateCollection productTemplateCollection = TemplateManager.GetAllProductTemplates();
            foreach (ProductTemplate productTemplate in productTemplateCollection)
            {
                ListItem item2 = new ListItem(productTemplate.Name, productTemplate.ProductTemplateID.ToString());
                this.ddlTemplate.Items.Add(item2);
            }

            this.ddlProductType.Items.Clear();
            ProductTypeCollection productTypeCollection = ProductManager.GetAllProductTypes();
            foreach (ProductType productType in productTypeCollection)
            {
                ListItem item2 = new ListItem(productType.Name, productType.ProductTypeID.ToString());
                this.ddlProductType.Items.Add(item2);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.FillDropDowns();
                this.BindData();
            }
        }

        public Product SaveInfo()
        {
            Product product = ProductManager.GetProductByID(this.ProductID);
            if (product != null)
            {
                product = ProductManager.UpdateProduct(product.ProductID, txtName.Text, txtShortDescription.Text, txtFullDescription.Value, txtAdminComment.Text,
                     int.Parse(this.ddlProductType.SelectedItem.Value), int.Parse(this.ddlTemplate.SelectedItem.Value),
                     cbShowOnHomePage.Checked, product.MetaKeywords, product.MetaDescription,
                     product.MetaTitle, product.SEName, cbAllowCustomerReviews.Checked,
                     cbAllowCustomerRatings.Checked, product.RatingSum, product.TotalRatingVotes, cbPublished.Checked,
                     product.Deleted, product.CreatedOn, DateTime.Now);
            }
            return product;
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