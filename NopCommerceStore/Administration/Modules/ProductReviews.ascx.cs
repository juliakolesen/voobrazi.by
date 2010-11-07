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
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class ProductReviewsControl : BaseNopAdministrationUserControl
    {
        protected void btnUpdateProductReview_Click(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Update")
            {
                int productReviewID = Convert.ToInt32(e.CommandArgument);
                ProductReview productReview = ProductManager.GetProductReviewByID(productReviewID);
                if (productReview != null)
                {
                    ProductManager.UpdateProductReview(productReview.ProductReviewID,
                        productReview.ProductID, productReview.CustomerID, productReview.Title,
                        productReview.ReviewText, productReview.Rating, productReview.HelpfulYesTotal, productReview.HelpfulNoTotal, !productReview.IsApproved, productReview.CreatedOn);
                }
                BindData();
            }
        }

        protected void btnEditProductReview_Click(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                int productReviewID = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("ProductReviewDetails.aspx?ProductReviewID=" + productReviewID.ToString());
            }
        }

        protected void btnDeleteProductReview_Click(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                int productReviewID = Convert.ToInt32(e.CommandArgument);
                ProductManager.DeleteProductReview(productReviewID);
                BindData();
            }
        }

        protected void lvProductReviews_OnPagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            this.pagerProductReviews.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            BindData();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                BindData();
        }

        protected string GetCustomerInfo(int CustomerID)
        {
            Customer customer = CustomerManager.GetCustomerByID(CustomerID);
            if (customer != null)
            {
                string customerInfo = string.Format("<a href=\"CustomerDetails.aspx?CustomerID={0}\">{1}</a>", customer.CustomerID, Server.HtmlEncode(customer.Email));
                return customerInfo;
            }
            else
                return string.Empty;
        }

        protected void BindData()
        {
            ProductReviewCollection productReviewCollection = null;
            if (this.ProductID > 0)
                productReviewCollection = ProductManager.GetProductReviewByProductID(ProductID);
            else
                productReviewCollection = ProductManager.GetAllProductReviews();
            lvProductReviews.DataSource = productReviewCollection;
            lvProductReviews.DataBind();
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