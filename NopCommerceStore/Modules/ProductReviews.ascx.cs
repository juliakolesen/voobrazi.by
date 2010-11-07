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
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class ProductReviews : BaseNopUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }

        protected void BindData()
        {
            Product product = ProductManager.GetProductByID(ProductID);
            if (product != null && product.AllowCustomerReviews)
            {
                pnlReviews.Visible = true;
                ProductReviewCollection productReviews = product.ProductReviews;
                if (productReviews.Count > 0)
                {
                    rptrProductReviews.DataSource = productReviews;
                    rptrProductReviews.DataBind();
                }
            }
            else
                this.Visible = false;
        }

        protected void btnWriteReview_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string writeReviewURL = CommonHelper.GetStoreLocation() + "ProductReviewAdd.aspx?ProductID=" + this.ProductID.ToString();
                Response.Redirect(writeReviewURL);
            }
        }

        protected string GetCustomerInfo(int CustomerID)
        {
            string customerInfo = string.Empty;
            Customer customer = CustomerManager.GetCustomerByID(CustomerID);
            if (customer != null)
            {
                customerInfo = CustomerManager.FormatUserName(customer);
            }
            else
            {
                customerInfo = "Not registered";
            }
            return customerInfo;
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