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
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Products.Attributes;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Web.Administration.Modules;

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class ProductVariantAttributeValuesControl : BaseNopAdministrationUserControl
    {
        private void BindData()
        {
            ProductVariantAttribute productVariantAttribute = ProductAttributeManager.GetProductVariantAttributeByID(this.ProductVariantAttributeID);
            if (productVariantAttribute != null)
            {
                ProductVariant productVariant = productVariantAttribute.ProductVariant;
                if (productVariant == null)
                    Response.Redirect("Products.aspx");
                ProductAttribute productAttribute = productVariantAttribute.ProductAttribute;
                if (productAttribute == null)
                    Response.Redirect("Products.aspx");

                this.lblTitle.Text = string.Format(GetLocaleResourceString("Admin.ProductVariantAttributeValues.AddEdit"), Server.HtmlEncode(productAttribute.Name), Server.HtmlEncode(productVariant.FullProductName));
                this.hlProductURL.NavigateUrl = CommonHelper.GetStoreAdminLocation() + "ProductVariantDetails.aspx?ProductVariantID=" + productVariant.ProductVariantID;
                
                ProductVariantAttributeValueCollection productVariantAttributeValues = productVariantAttribute.ProductVariantAttributeValues;
                if (productVariantAttributeValues.Count > 0)
                {
                    this.gvProductVariantAttributeValues.Visible = true;
                    this.gvProductVariantAttributeValues.DataSource = productVariantAttributeValues;
                    this.gvProductVariantAttributeValues.DataBind();
                }
                else
                    this.gvProductVariantAttributeValues.Visible = false;
            }
            else
                Response.Redirect("Products.aspx");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                gvProductVariantAttributeValues.Columns[1].HeaderText = string.Format("Price Adjustment [{0}]", CurrencyManager.PrimaryStoreCurrency.CurrencyCode);

                this.BindData();
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                ProductVariantAttribute productVariantAttribute = ProductAttributeManager.GetProductVariantAttributeByID(this.ProductVariantAttributeID);
                if (productVariantAttribute != null)
                {
                    ProductVariantAttributeValue productVariantAttributeValue = ProductAttributeManager.InsertProductVariantAttributeValue(productVariantAttribute.ProductVariantAttributeID,
                        txtNewName.Text, txtNewPriceAdjustment.Value, txtNewWeightAdjustment.Value,
                        cbNewIsPreSelected.Checked, txtNewDisplayOrder.Value);

                    BindData();

                    txtNewName.Text = string.Empty;
                    txtNewPriceAdjustment.Value = 0;
                    txtNewWeightAdjustment.Value = 0;
                    txtNewDisplayOrder.Value = 1;
                }
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        protected void gvProductVariantAttributeValues_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UpdateProductVariantAttributeValue")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvProductVariantAttributeValues.Rows[index];

                HiddenField hfProductVariantAttributeValueID = row.FindControl("hfProductVariantAttributeValueID") as HiddenField;
                SimpleTextBox txtName = row.FindControl("txtName") as SimpleTextBox;
                DecimalTextBox txtPriceAdjustment = row.FindControl("txtPriceAdjustment") as DecimalTextBox;
                DecimalTextBox txtWeightAdjustment = row.FindControl("txtWeightAdjustment") as DecimalTextBox;
                CheckBox cbIsPreSelected = row.FindControl("cbIsPreSelected") as CheckBox;
                NumericTextBox txtDisplayOrder = row.FindControl("txtDisplayOrder") as NumericTextBox;

                int productVariantAttributeValueID = int.Parse(hfProductVariantAttributeValueID.Value);
                string name = txtName.Text;
                decimal priceAdjustment = txtPriceAdjustment.Value;
                decimal weightAdjustment = txtWeightAdjustment.Value;
                bool isPreSelected = cbIsPreSelected.Checked;
                int displayOrder = txtDisplayOrder.Value;

                ProductVariantAttributeValue productVariantAttributeValue = ProductAttributeManager.GetProductVariantAttributeValueByID(productVariantAttributeValueID);

                if (productVariantAttributeValue != null)
                    ProductAttributeManager.UpdateProductVariantAttributeValue(productVariantAttributeValue.ProductVariantAttributeValueID,
                       productVariantAttributeValue.ProductVariantAttributeID, name,
                       priceAdjustment, weightAdjustment, isPreSelected, displayOrder);

                BindData();
            }
        }

        protected void gvProductVariantAttributeValues_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ProductVariantAttributeValue productVariantAttributeValue = (ProductVariantAttributeValue)e.Row.DataItem;

                Button btnUpdate = e.Row.FindControl("btnUpdate") as Button;
                if (btnUpdate != null)
                    btnUpdate.CommandArgument = e.Row.RowIndex.ToString();
            }
        }

        protected void gvProductVariantAttributeValues_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int productVariantAttributeValueID = (int)gvProductVariantAttributeValues.DataKeys[e.RowIndex]["ProductVariantAttributeValueID"];
            ProductAttributeManager.DeleteProductVariantAttributeValue(productVariantAttributeValueID);
            BindData();
        }

        public int ProductVariantAttributeID
        {
            get
            {
                return CommonHelper.QueryStringInt("ProductVariantAttributeID");
            }
        }
    }
}