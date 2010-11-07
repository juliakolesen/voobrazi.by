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
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Shipping;

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class OrdersControl : BaseNopAdministrationUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                FillDropDowns();
                SetDefaultValues();
            }
        }

        protected void SetDefaultValues()
        {
        }

        protected OrderCollection GetOrders()
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

            ShippingStatusEnum? shippingStatus = null;
            int shippingStatusID = int.Parse(ddlShippingStatus.SelectedItem.Value);
            if (shippingStatusID > 0)
                shippingStatus = (ShippingStatusEnum)Enum.ToObject(typeof(ShippingStatusEnum), shippingStatusID);

            OrderCollection orders = OrderManager.SearchOrders(startDate, endDate, txtCustomerEmail.Text, orderStatus, paymentStatus, shippingStatus);

            return orders;
        }

        protected void FillDropDowns()
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

            this.ddlShippingStatus.Items.Clear();
            ListItem itemShippingStatus = new ListItem(GetLocaleResourceString("Admin.Common.All"), "0");
            this.ddlShippingStatus.Items.Add(itemOrderStatus);
            ShippingStatusCollection shippingStatuses = ShippingStatusManager.GetAllShippingStatuses();
            foreach (ShippingStatus shippingStatus in shippingStatuses)
            {
                ListItem item2 = new ListItem(shippingStatus.Name, shippingStatus.ShippingStatusID.ToString());
                this.ddlShippingStatus.Items.Add(item2);
            }
        }

        protected void btnExportXML_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    string fileName = string.Format("orders_{0}.xml", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));

                    OrderCollection orders = GetOrders();
                    string xml = ExportManager.ExportOrdersToXML(orders);
                    CommonHelper.WriteResponseXML(xml, fileName);
                }
                catch (Exception exc)
                {
                    ProcessException(exc);
                }
            }
        }

        protected void btnExportXLS_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    string fileName = string.Format("orders_{0}.xls", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    string filePath = string.Format("{0}files\\ExportImport\\{1}", HttpContext.Current.Request.PhysicalApplicationPath, fileName);
                    OrderCollection orders = GetOrders();

                    ExportManager.ExportOrdersToXLS(filePath, orders);
                    CommonHelper.WriteResponseXLS(filePath, fileName);
                }
                catch (Exception exc)
                {
                    ProcessException(exc);
                }
            }
        }

        protected void BindGrid()
        {
            OrderCollection orders = GetOrders();
            if (orders.Count > 0)
            {
                this.gvOrders.Visible = true;
                this.lblNoOrdersFound.Visible = false;
                this.gvOrders.DataSource = orders;
                this.gvOrders.DataBind();
            }
            else
            {
                this.gvOrders.Visible = false;
                this.lblNoOrdersFound.Visible = true;
            }
        }

        protected string GetCustomerInfo(int CustomerID)
        {
            string customerInfo = string.Empty;
            Customer customer = CustomerManager.GetCustomerByID(CustomerID);
            if (customer != null)
            {
                if (customer.IsGuest)
                {
                    customerInfo = string.Format("<a href=\"CustomerDetails.aspx?CustomerID={0}\">{1}</a>", customer.CustomerID, GetLocaleResourceString("Admin.Orders.CustomerColumn.Guest"));
                }
                else
                {
                    customerInfo = string.Format("<a href=\"CustomerDetails.aspx?CustomerID={0}\">{1}</a>", customer.CustomerID, Server.HtmlEncode(customer.Email));
                }
            }
            return customerInfo;
        }

        protected void gvOrders_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvOrders.PageIndex = e.NewPageIndex;
            BindGrid();
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

        protected void btnGoDirectlyToOrderNumber_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    int orderID = 0;
                    if (int.TryParse(txtOrderID.Text.Trim(), out orderID))
                    {
                        string url = string.Format("{0}OrderDetails.aspx?OrderID={1}", CommonHelper.GetStoreAdminLocation(), orderID);
                        Response.Redirect(url);
                    }
                }
                catch (Exception exc)
                {
                    ProcessException(exc);
                }
            }
        }
    }
}