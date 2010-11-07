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
using NopSolutions.NopCommerce.BusinessLogic.Shipping;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Web.Templates.Shipping;

namespace NopSolutions.NopCommerce.Web.Administration
{
    public partial class Administration_ShippingRateComputationMethodDetails : BaseNopAdministrationPage
    {
        private void BindData()
        {
            ShippingRateComputationMethod shippingRateComputationMethod = ShippingRateComputationMethodManager.GetShippingRateComputationMethodByID(this.ShippingRateComputationMethodID);
            if (shippingRateComputationMethod != null)
            {
                this.txtName.Text = shippingRateComputationMethod.Name;
                this.txtDescription.Text = shippingRateComputationMethod.Description;
                this.txtConfigureTemplatePath.Text = shippingRateComputationMethod.ConfigureTemplatePath;
                this.txtClassName.Text = shippingRateComputationMethod.ClassName;
                this.txtDisplayOrder.Value = shippingRateComputationMethod.DisplayOrder;
            }
            else
                Response.Redirect("ShippingRateComputationMethods.aspx");
        }
        
        private void CreateChildControlsTree()
        {
            ShippingRateComputationMethod shippingRateComputationMethod = ShippingRateComputationMethodManager.GetShippingRateComputationMethodByID(this.ShippingRateComputationMethodID);
            if (shippingRateComputationMethod != null)
            {
                Control child = null;
                try
                {
                    child = base.LoadControl(shippingRateComputationMethod.ConfigureTemplatePath);
                    this.ConfigurePlaceHolder.Controls.Add(child);
                }
                catch (Exception exc)
                {
                }
            }
        }

        private IConfigureShippingRateComputationMethodModule GetConfigureModule()
        {
            foreach (Control ctrl in this.ConfigurePlaceHolder.Controls)
                if (ctrl is IConfigureShippingRateComputationMethodModule)
                    return (IConfigureShippingRateComputationMethodModule)ctrl;
            return null;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.CreateChildControlsTree();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.BindData();
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    ShippingRateComputationMethod shippingRateComputationMethod = ShippingRateComputationMethodManager.GetShippingRateComputationMethodByID(this.ShippingRateComputationMethodID);

                    if (shippingRateComputationMethod != null)
                    {
                        shippingRateComputationMethod = ShippingRateComputationMethodManager.UpdateShippingRateComputationMethod(shippingRateComputationMethod.ShippingRateComputationMethodID, txtName.Text,
                            txtDescription.Text, txtConfigureTemplatePath.Text, txtClassName.Text,
                          txtDisplayOrder.Value);

                        IConfigureShippingRateComputationMethodModule configureModule = GetConfigureModule();
                        if (configureModule != null)
                            configureModule.Save();

                        Response.Redirect("ShippingRateComputationMethodDetails.aspx?ShippingRateComputationMethodID=" + shippingRateComputationMethod.ShippingRateComputationMethodID.ToString());
                    }
                    else
                        Response.Redirect("ShippingRateComputationMethods.aspx");
                }
                catch (Exception exc)
                {
                    ProcessException(exc);
                }
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                ShippingRateComputationMethodManager.DeleteShippingRateComputationMethod(this.ShippingRateComputationMethodID);
                Response.Redirect("ShippingRateComputationMethods.aspx");
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        public int ShippingRateComputationMethodID
        {
            get
            {
                return CommonHelper.QueryStringInt("ShippingRateComputationMethodID");
            }
        }
    }
}
