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
using NopSolutions.NopCommerce.BusinessLogic.ExportImport;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Products.Attributes;
using NopSolutions.NopCommerce.BusinessLogic.Promo.Discounts;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Warehouses;
using NopSolutions.NopCommerce.Web.Administration.Modules;
using NopSolutions.NopCommerce.BusinessLogic.Directory;

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class ProductVariantTierPricesControl : BaseNopAdministrationUserControl
    {
        private void BindData()
        {
            ProductVariant productVariant = ProductManager.GetProductVariantByID(this.ProductVariantID);
            if (productVariant != null)
            {
                TierPriceCollection tierPrices = productVariant.TierPrices;
                if (tierPrices.Count > 0)
                {
                    gvTierPrices.Visible = true;
                    gvTierPrices.DataSource = tierPrices;
                    gvTierPrices.DataBind();
                }
                else
                    gvTierPrices.Visible = false;
            }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                gvTierPrices.Columns[1].HeaderText = string.Format("Price [{0}]", CurrencyManager.PrimaryStoreCurrency.CurrencyCode);
                this.BindData();
            }
        }

        public void SaveInfo()
        {
            
        }

        protected void btnNewTierPrice_Click(object sender, EventArgs e)
        {
            try
            {
                ProductVariant productVariant = ProductManager.GetProductVariantByID(this.ProductVariantID);
                if (productVariant != null)
                {
                    decimal price = txtNewPrice.Value;
                    int quantity = txtNewQuantity.Value;
                    TierPrice tierPrice = ProductManager.InsertTierPrice(productVariant.ProductVariantID, quantity, price);

                    BindData();
                }
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        protected void gvTierPrices_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UpdateTierPrice")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvTierPrices.Rows[index];

                HiddenField hfTierPriceID = row.FindControl("hfTierPriceID") as HiddenField;
                NumericTextBox txtQuantity = row.FindControl("txtQuantity") as NumericTextBox;
                DecimalTextBox txtPrice = row.FindControl("txtPrice") as DecimalTextBox;

                int tierPriceID = int.Parse(hfTierPriceID.Value);
                decimal price = txtPrice.Value;
                int quantity = txtQuantity.Value;

                TierPrice tierPrice = ProductManager.GetTierPriceByID(tierPriceID);

                if (tierPrice != null)
                    ProductManager.UpdateTierPrice(tierPrice.TierPriceID,
                       tierPrice.ProductVariantID, quantity, price);

                BindData();
            }
        }

        protected void gvTierPrices_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TierPrice tierPrice = (TierPrice)e.Row.DataItem;

                Button btnUpdate = e.Row.FindControl("btnUpdate") as Button;
                if (btnUpdate != null)
                    btnUpdate.CommandArgument = e.Row.RowIndex.ToString();
            }
        }

        protected void gvTierPrices_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int tierPriceID = (int)gvTierPrices.DataKeys[e.RowIndex]["TierPriceID"];
            ProductManager.DeleteTierPrice(tierPriceID);
            BindData();
        }

        public int ProductVariantID
        {
            get
            {
                return CommonHelper.QueryStringInt("ProductVariantID");
            }
        }
    }
}