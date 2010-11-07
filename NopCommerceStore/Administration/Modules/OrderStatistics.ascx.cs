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
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Shipping;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.DataAccess;

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class OrderStatisticsControl : BaseNopAdministrationUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            //pending
            decimal os_pendingTotalSum = decimal.Zero;
            int os_pendingCount = 0;
            using (IDataReader orders_os_pending = OrderManager.GetOrderReport(OrderStatusEnum.Pending, null, null))
            {
                while (orders_os_pending.Read())
                {

                    os_pendingTotalSum = NopSqlDataHelper.GetDecimal(orders_os_pending, "Total");
                    os_pendingCount = NopSqlDataHelper.GetInt(orders_os_pending, "Count");
                }
                lblTotalIncomplete.Text = os_pendingCount.ToString();
                lblTotalIncompleteValue.Text = PriceHelper.FormatPrice(os_pendingTotalSum, true, false);
            }

            //not paid
            decimal ps_pendingTotalSum = decimal.Zero;
            int ps_pendingCount = 0;
            using (IDataReader orders_ps_pending = OrderManager.GetOrderReport(null, PaymentStatusEnum.Pending, null))
            {
                while (orders_ps_pending.Read())
                {
                    ps_pendingTotalSum = NopSqlDataHelper.GetDecimal(orders_ps_pending, "Total");
                    ps_pendingCount = NopSqlDataHelper.GetInt(orders_ps_pending, "Count");
                }
                lblTotalUnpaid.Text = ps_pendingCount.ToString();
                lblTotalUnpaidValue.Text = PriceHelper.FormatPrice(ps_pendingTotalSum, true, false);
            }

            //not shipped
            decimal ss_pendingTotalSum = decimal.Zero;
            int ss_pendingCount = 0;
            using (IDataReader orders_ss_pending = OrderManager.GetOrderReport(null, null, ShippingStatusEnum.NotYetShipped))
            {
                while (orders_ss_pending.Read())
                {
                    ss_pendingTotalSum = NopSqlDataHelper.GetDecimal(orders_ss_pending, "Total");
                    ss_pendingCount = NopSqlDataHelper.GetInt(orders_ss_pending, "Count");
                }
                lblTotalUnshipped.Text = ss_pendingCount.ToString();
                lblTotalUnshippedValue.Text = PriceHelper.FormatPrice(ss_pendingTotalSum, true, false);
            }
        }
    }
}

