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
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Promo.Discounts;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Common;
 
namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class DiscountInfoControl : BaseNopAdministrationUserControl
    {
        private void FillDropDowns()
        {
            this.ddlDiscountType.Items.Clear();
            DiscountTypeCollection discountTypes = DiscountManager.GetAllDiscountTypes();
            foreach (DiscountType discountType in discountTypes)
            {
                ListItem item2 = new ListItem(discountType.Name, discountType.DiscountTypeID.ToString());
                this.ddlDiscountType.Items.Add(item2);
            }

            this.ddlDiscountRequirement.Items.Clear();
            DiscountRequirementCollection discountRequirements = DiscountManager.GetAllDiscountRequirements();
            foreach (DiscountRequirement discountRequirement in discountRequirements)
            {
                ListItem item2 = new ListItem(discountRequirement.Name, discountRequirement.DiscountRequirementID.ToString());
                this.ddlDiscountRequirement.Items.Add(item2);
            }
        }

        private void BindData()
        {
            Discount discount = DiscountManager.GetDiscountByID(this.DiscountID);
            if (discount != null)
            {
                CommonHelper.SelectListItem(this.ddlDiscountType, discount.DiscountTypeID);
                CommonHelper.SelectListItem(this.ddlDiscountRequirement, discount.DiscountRequirementID);
                this.txtName.Text = discount.Name;
                this.cbUsePercentage.Checked = discount.UsePercentage;
                this.txtDiscountPercentage.Value = discount.DiscountPercentage;
                this.txtDiscountAmount.Value = discount.DiscountAmount;
                this.cStartDateButtonExtender.SelectedDate = discount.StartDate;
                this.cEndDateButtonExtender.SelectedDate = discount.EndDate;
                this.cbRequiresCouponCode.Checked = discount.RequiresCouponCode;
                this.txtCouponCode.Text = discount.CouponCode;

                CustomerRoleCollection customerRoles = discount.CustomerRoles;
                List<int> _customerRoleIDs = new List<int>();
                foreach (CustomerRole customerRole in customerRoles)
                    _customerRoleIDs.Add(customerRole.CustomerRoleID);
                CustomerRoleMappingControl.SelectedCustomerRoleIDs = _customerRoleIDs;
                CustomerRoleMappingControl.BindData();

            }
            else
            {
                List<int> _customerRoleIDs = new List<int>();
                CustomerRoleMappingControl.SelectedCustomerRoleIDs = _customerRoleIDs;
                CustomerRoleMappingControl.BindData();
            }
        }

        private void TogglePanels()
        {
            pnlDiscountPercentage.Visible = cbUsePercentage.Checked;
            pnlDiscountAmount.Visible = !cbUsePercentage.Checked;
            pnlCouponCode.Visible = cbRequiresCouponCode.Checked;

            DiscountRequirementEnum discountRequirement = (DiscountRequirementEnum)int.Parse(this.ddlDiscountRequirement.SelectedItem.Value);
            pnlCustomerRoles.Visible = discountRequirement == DiscountRequirementEnum.MustBeAssignedToCustomerRole;
        }

        private void SetDefaultValues()
        {
            txtStartDate.Text = DateTime.UtcNow.AddDays(-2).ToString(cStartDateButtonExtender.Format);
            txtEndDate.Text = DateTime.UtcNow.AddYears(1).ToString(cEndDateButtonExtender.Format);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.FillDropDowns();
                this.SetDefaultValues();
                this.BindData();
                this.TogglePanels();
            }
        }

        public Discount SaveInfo()
        {
            DateTime discountStartDate = DateTime.MinValue;
            DateTime discountEndDate = DateTime.MinValue;
            if (!DateTime.TryParse(txtStartDate.Text, out discountStartDate))
                throw new NopException("Start date is not set");
            if (!DateTime.TryParse(txtEndDate.Text, out discountEndDate))
                throw new NopException("End date is not set");
            discountStartDate = DateTime.SpecifyKind(discountStartDate, DateTimeKind.Utc);
            discountEndDate = DateTime.SpecifyKind(discountEndDate, DateTimeKind.Utc);
            
            Discount discount = DiscountManager.GetDiscountByID(this.DiscountID);

            if (discount != null)
            {
                discount = DiscountManager.UpdateDiscount(discount.DiscountID,
                    (DiscountTypeEnum)int.Parse(this.ddlDiscountType.SelectedItem.Value),
                    (DiscountRequirementEnum)int.Parse(this.ddlDiscountRequirement.SelectedItem.Value),
                    txtName.Text,
                    cbUsePercentage.Checked,
                    txtDiscountPercentage.Value,
                    txtDiscountAmount.Value,
                    discountStartDate,
                    discountEndDate,
                    cbRequiresCouponCode.Checked,
                    txtCouponCode.Text.Trim(),
                    discount.Deleted);

                foreach (CustomerRole customerRole in discount.CustomerRoles)
                    CustomerManager.RemoveDiscountFromCustomerRole(customerRole.CustomerRoleID, discount.DiscountID);
                foreach (int customerRoleID in CustomerRoleMappingControl.SelectedCustomerRoleIDs)
                    CustomerManager.AddDiscountToCustomerRole(customerRoleID, discount.DiscountID);

            }
            else
            {
                discount = DiscountManager.InsertDiscount((DiscountTypeEnum)int.Parse(this.ddlDiscountType.SelectedItem.Value),
                    (DiscountRequirementEnum)int.Parse(this.ddlDiscountRequirement.SelectedItem.Value),
                    txtName.Text,
                    cbUsePercentage.Checked,
                    txtDiscountPercentage.Value,
                    txtDiscountAmount.Value,
                    discountStartDate,
                    discountEndDate,
                    cbRequiresCouponCode.Checked,
                    txtCouponCode.Text.Trim(),
                    false);

                foreach (int customerRoleID in CustomerRoleMappingControl.SelectedCustomerRoleIDs)
                    CustomerManager.AddDiscountToCustomerRole(customerRoleID, discount.DiscountID);

            }

            return discount;
        }

        protected void ddlDiscountRequirement_SelectedIndexChanged(object sender, EventArgs e)
        {
            TogglePanels();
        }

        protected void cbRequiresCouponCode_CheckedChanged(object sender, EventArgs e)
        {
            TogglePanels();
        }

        protected void cbUsePercentage_CheckedChanged(object sender, EventArgs e)
        {
            TogglePanels();
        }
        
        public int DiscountID
        {
            get
            {
                return CommonHelper.QueryStringInt("DiscountID");
            }
        }
    }
}