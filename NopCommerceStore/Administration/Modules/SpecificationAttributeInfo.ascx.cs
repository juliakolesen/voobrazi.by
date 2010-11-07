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
using NopSolutions.NopCommerce.BusinessLogic.Products.Specs;
using NopSolutions.NopCommerce.Common.Utils;
using System.Web.UI.WebControls;

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class SpecificationAttributeInfoControl : BaseNopAdministrationUserControl
    {
        private void BindData()
        {
            SpecificationAttribute specificationAttribute = SpecificationAttributeManager.GetSpecificationAttributeByID(this.SpecificationAttributeID);
            if (specificationAttribute != null)
            {
                this.txtName.Text = specificationAttribute.Name;
                this.txtDisplayOrder.Value = specificationAttribute.DisplayOrder;
            }

            SpecificationAttributeOptionCollection saoCol = SpecificationAttributeManager.GetSpecificationAttributeOptionsBySpecificationAttribute(SpecificationAttributeID);
            grdSpecificationAttributeOptions.DataSource = saoCol;
            grdSpecificationAttributeOptions.DataBind();

           }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.BindData();
            }

            if (SpecificationAttributeID <= 0)
                pnlSpecAttrOptions.Visible = false;
        }

        protected void btnAddSpecificationAttributeOption_Click(object sender, EventArgs e)
        {
            Response.Redirect("SpecificationAttributeOptionAdd.aspx?SpecificationAttributeID=" + this.SpecificationAttributeID);
        }

        public SpecificationAttribute SaveInfo()
        {
            SpecificationAttribute specificationAttribute = SpecificationAttributeManager.GetSpecificationAttributeByID(this.SpecificationAttributeID);

            if (specificationAttribute != null)
            {
                specificationAttribute = SpecificationAttributeManager.UpdateSpecificationAttribute(specificationAttribute.SpecificationAttributeID, txtName.Text, txtDisplayOrder.Value);
            }
            else
            {
                specificationAttribute = SpecificationAttributeManager.InsertSpecificationAttribute(txtName.Text, txtDisplayOrder.Value);
            }
            return specificationAttribute;
        }

        protected void OnSpecificationAttributeOptionsCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UpdateOption")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = grdSpecificationAttributeOptions.Rows[index];
                SimpleTextBox txtName = row.FindControl("txtOptionName") as SimpleTextBox;
                NumericTextBox txtDisplayOrder = row.FindControl("txtOptionDisplayOrder") as NumericTextBox;
                HiddenField hfSpecificationAttributeOptionID = row.FindControl("hfSpecificationAttributeOptionID") as HiddenField;

                string name = txtName.Text;
                int displayOrder = txtDisplayOrder.Value;
                int saoID = int.Parse(hfSpecificationAttributeOptionID.Value);

                SpecificationAttributeOption sao = SpecificationAttributeManager.GetSpecificationAttributeOptionByID(saoID);
                if (sao != null)
                    SpecificationAttributeManager.UpdateSpecificationAttributeOptions(saoID, SpecificationAttributeID, name, displayOrder);

                BindData();
            }
        }

        protected void OnSpecificationAttributeOptionsDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int saoID = (int)grdSpecificationAttributeOptions.DataKeys[e.RowIndex]["SpecificationAttributeOptionID"];
            SpecificationAttributeOption sao = SpecificationAttributeManager.GetSpecificationAttributeOptionByID(saoID);
            if (sao != null)
            {
                SpecificationAttributeManager.DeleteSpecificationAttributeOption(sao.SpecificationAttributeOptionID);
                BindData();
            }
        }

        protected void OnSpecificationAttributeOptionsDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btnUpdate = e.Row.FindControl("btnUpdate") as Button;
                if (btnUpdate != null)
                    btnUpdate.CommandArgument = e.Row.RowIndex.ToString();
            }
        }

        public int SpecificationAttributeID
        {
            get
            {
                return CommonHelper.QueryStringInt("SpecificationAttributeID");
            }
        }
    }
}