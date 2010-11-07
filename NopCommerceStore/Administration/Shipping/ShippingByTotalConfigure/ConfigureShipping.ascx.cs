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
using System.Web.UI.WebControls;
using NopSolutions.NopCommerce.BusinessLogic.Shipping;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Web.Administration.Modules;
using NopSolutions.NopCommerce.Web.Templates.Shipping;

namespace NopSolutions.NopCommerce.Web.Administration.Shipping.ShippingByTotalConfigure
{
    public partial class ConfigureShipping : BaseNopAdministrationUserControl, IConfigureShippingRateComputationMethodModule
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                FillDropDowns();
                BindData();
            }
        }

        private void FillDropDowns()
        {
            ddlShippingMethod.Items.Clear();
            ShippingMethodCollection shippingMethodCollection = ShippingMethodManager.GetAllShippingMethods();
            foreach (ShippingMethod shippingMethod in shippingMethodCollection)
            {
                ListItem item = new ListItem(shippingMethod.Name, shippingMethod.ShippingMethodID.ToString());
                ddlShippingMethod.Items.Add(item);
            }
        }

        private void BindData()
        {
            ShippingByTotalCollection shippingByTotalCollection = ShippingByTotalManager.GetAll();
            gvShippingByTotals.DataSource = shippingByTotalCollection;
            gvShippingByTotals.DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                int shippingMethodID = int.Parse(this.ddlShippingMethod.SelectedItem.Value);
                ShippingByTotal shippingByTotal = ShippingByTotalManager.InsertShippingByTotal(shippingMethodID,
                    txtFrom.Value, txtTo.Value, cbUsePercentage.Checked,
                    txtShippingChargePercentage.Value, txtShippingChargeAmount.Value);

                BindData();
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        protected void gvShippingByTotals_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UpdateShippingByTotal")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvShippingByTotals.Rows[index];

                HiddenField hfShippingByTotalID = row.FindControl("hfShippingByTotalID") as HiddenField;
                DropDownList ddlShippingMethod = row.FindControl("ddlShippingMethod") as DropDownList;
                DecimalTextBox txtFrom = row.FindControl("txtFrom") as DecimalTextBox;
                DecimalTextBox txtTo = row.FindControl("txtTo") as DecimalTextBox;
                CheckBox cbUsePercentage = row.FindControl("cbUsePercentage") as CheckBox;
                DecimalTextBox txtShippingChargePercentage = row.FindControl("txtShippingChargePercentage") as DecimalTextBox;
                DecimalTextBox txtShippingChargeAmount = row.FindControl("txtShippingChargeAmount") as DecimalTextBox;

                int shippingByTotalID = int.Parse(hfShippingByTotalID.Value);
                int shippingMethodID = int.Parse(ddlShippingMethod.SelectedItem.Value);
                ShippingByTotal shippingByTotal = ShippingByTotalManager.GetByID(shippingByTotalID);

                if (shippingByTotal != null)
                    ShippingByTotalManager.UpdateShippingByTotal(shippingByTotal.ShippingByTotalID,
                      shippingMethodID, txtFrom.Value,txtTo.Value,cbUsePercentage.Checked,
                      txtShippingChargePercentage.Value,txtShippingChargeAmount.Value);

                BindData();
            }
        }

        protected void gvShippingByTotals_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ShippingByTotal shippingByTotal = (ShippingByTotal)e.Row.DataItem;
             
                Button btnUpdate = e.Row.FindControl("btnUpdate") as Button;
                if (btnUpdate != null)
                    btnUpdate.CommandArgument = e.Row.RowIndex.ToString();

                DropDownList ddlShippingMethod = e.Row.FindControl("ddlShippingMethod") as DropDownList;
                ddlShippingMethod.Items.Clear();
                ShippingMethodCollection shippingMethodCollection = ShippingMethodManager.GetAllShippingMethods();
                foreach (ShippingMethod shippingMethod in shippingMethodCollection)
                {
                    ListItem item = new ListItem(shippingMethod.Name, shippingMethod.ShippingMethodID.ToString());
                    ddlShippingMethod.Items.Add(item);
                    if (shippingByTotal.ShippingMethodID == shippingMethod.ShippingMethodID)
                        item.Selected = true;
                }
            }
        }

        protected void gvShippingByTotals_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int shippingByTotalID = (int)gvShippingByTotals.DataKeys[e.RowIndex]["ShippingByTotalID"];
            ShippingByTotal shippingByTotal = ShippingByTotalManager.GetByID(shippingByTotalID);
            if (shippingByTotal != null)
            {
                ShippingByTotalManager.DeleteShippingByTotal(shippingByTotal.ShippingByTotalID);
                BindData();
            }
        }

        public void Save()
        {
     
        }
    }
}
