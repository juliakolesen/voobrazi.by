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
using NopSolutions.NopCommerce.BusinessLogic.Products.Specs;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Colors;

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class SpecificationAttributeOptionAddControl : BaseNopAdministrationUserControl
    {
        private bool IsColor
        {
            get 
            {
                SpecificationAttribute sa = SpecificationAttributeManager.GetSpecificationAttributeByID(SpecificationAttributeID);
                return (sa != null && sa.Name.Equals("цвет", StringComparison.CurrentCultureIgnoreCase));
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsColor)
            {
                ToolTipLabelColor.Visible = true;
                txtColorArgb.Visible = true;
                cusCustom.Enabled = true;
            }

            hlBack.NavigateUrl = "~/Administration/SpecificationAttributeDetails.aspx?SpecificationAttributeID=" + SpecificationAttributeID;
        }

        protected void AddButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    SpecificationAttributeOption sao = null;
                    if(IsColor)
                    {
                        ColorManager.InsertColor(txtNewOptionName.Text, Int32.Parse(txtColorArgb.Text, System.Globalization.NumberStyles.AllowHexSpecifier));
                    }
                    sao = SpecificationAttributeManager.InsertSpecificationAttributeOption(SpecificationAttributeID, txtNewOptionName.Text, txtNewOptionDisplayOrder.Value);
                    Response.Redirect("SpecificationAttributeDetails.aspx?SpecificationAttributeID=" + sao.SpecificationAttributeID.ToString());
                }
                catch (Exception exc)
                {
                    ProcessException(exc);
                }
            }
        }

        public int SpecificationAttributeID
        {
            get
            {
                return CommonHelper.QueryStringInt("SpecificationAttributeID");
            }
        }

        protected void cusCustom_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            if (ColorManager.GetColorByColorName(txtNewOptionName.Text) == null)
                e.IsValid = true;
            else
                e.IsValid = false;
        }
    }
}