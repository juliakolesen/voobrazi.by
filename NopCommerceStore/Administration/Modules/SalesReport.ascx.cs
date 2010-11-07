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
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.ExportImport;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class SalesReportControl : BaseNopAdministrationUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                FillDropDowns();
            }
        }

        private void FillDropDowns()
        {
            this.ddlOrderStatus.Items.Clear();
            ListItem itemOrderStatus = new ListItem(GetLocaleResourceString("Admin.Common.All"), "0");
            this.ddlOrderStatus.Items.Add(itemOrderStatus);
            OrderStatusCollection orderStatuses = OrderManager.GetAllOrderStatuses();
            foreach (OrderStatus orderStatus in orderStatuses)
            {
                ListItem item2 = new ListItem(orderStatus.Name, orderStatus.OrderStatusID.ToString());
                this.ddlOrderStatus.Items.Add(item2);
            }

            this.ddlPaymentStatus.Items.Clear();
            ListItem itemPaymentStatus = new ListItem(GetLocaleResourceString("Admin.Common.All"), "0");
            this.ddlPaymentStatus.Items.Add(itemPaymentStatus);
            PaymentStatusCollection paymentStatuses = PaymentStatusManager.GetAllPaymentStatuses();
            foreach (PaymentStatus paymentStatus in paymentStatuses)
            {
                ListItem item2 = new ListItem(paymentStatus.Name, paymentStatus.PaymentStatusID.ToString());
                this.ddlPaymentStatus.Items.Add(item2);
            }
        }

        protected void BindGrid()
        {
            DateTime? startDate = null;
            DateTime? endDate = null;
            DateTime startDateTmp = DateTime.Now;
            if (DateTime.TryParse(txtStartDate.Text, out startDateTmp))
            {
                startDate = DateTime.SpecifyKind(startDateTmp, DateTimeKind.Utc);
            }
            DateTime endDateTmp = DateTime.Now;
            if (DateTime.TryParse(txtEndDate.Text, out endDateTmp))
            {
                endDate = DateTime.SpecifyKind(endDateTmp, DateTimeKind.Utc);
            }

            OrderStatusEnum? orderStatus = null;
            int orderStatusID = int.Parse(ddlOrderStatus.SelectedItem.Value);
            if (orderStatusID > 0)
                orderStatus = (OrderStatusEnum)Enum.ToObject(typeof(OrderStatusEnum), orderStatusID);

            PaymentStatusEnum? paymentStatus = null;
            int paymentStatusID = int.Parse(ddlPaymentStatus.SelectedItem.Value);
            if (paymentStatusID > 0)
                paymentStatus = (PaymentStatusEnum)Enum.ToObject(typeof(PaymentStatusEnum), paymentStatusID);

            gvOrders.DataSource = OrderManager.OrderProductVariantReport(startDate, endDate, orderStatus, paymentStatus);
            gvOrders.DataBind();
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    BindGrid();
                }
                catch (Exception exc)
                {
                    ProcessException(exc);
                }
            }
        }

        public string GetProductURL(int ProductVariantID)
        {
            string result = string.Empty;
            ProductVariant productVariant = ProductManager.GetProductVariantByID(ProductVariantID);
            if (productVariant != null)
                result = "ProductVariantDetails.aspx?ProductVariantID=" + productVariant.ProductVariantID.ToString();
            else
                result = "Not available. Product variant ID=" + productVariant.ProductVariantID.ToString();
            return result;
        }

        public string GetProductVariantName(int ProductVariantID)
        {
            ProductVariant productVariant = ProductManager.GetProductVariantByID(ProductVariantID);
            if (productVariant != null)
                return productVariant.FullProductName;
            return "Not available. ID=" + ProductVariantID.ToString();
        }
    }
}