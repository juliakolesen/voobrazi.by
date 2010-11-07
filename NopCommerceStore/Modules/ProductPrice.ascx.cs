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
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Localization;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class ProductPriceControl : BaseNopUserControl
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
            ProductVariant productVariant = ProductManager.GetProductVariantByID(this.ProductVariantID);
            if (productVariant != null)
            {
                decimal oldPriceBase = TaxManager.GetPrice(productVariant, productVariant.OldPrice);
                decimal finalPriceWithoutDiscountBase = TaxManager.GetPrice(productVariant, PriceHelper.GetFinalPrice(productVariant, false));
                decimal finalPriceWithDiscountBase = TaxManager.GetPrice(productVariant, PriceHelper.GetFinalPrice(productVariant, true));

                decimal oldPrice = CurrencyManager.ConvertCurrency(oldPriceBase, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingCurrency);
                decimal finalPriceWithoutDiscount = CurrencyManager.ConvertCurrency(finalPriceWithoutDiscountBase, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingCurrency);
                decimal finalPriceWithDiscount = CurrencyManager.ConvertCurrency(finalPriceWithDiscountBase, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingCurrency);

                if (finalPriceWithoutDiscountBase != oldPriceBase && oldPriceBase > decimal.Zero)
                {
                    lblOldPrice.Text = PriceHelper.FormatPrice(oldPrice);
                    lblPriceValue.Text = PriceHelper.FormatPrice(finalPriceWithoutDiscount);
                    phOldPrice.Visible = true;
                }
                else
                {
                    lblPriceValue.Text = PriceHelper.FormatPrice(finalPriceWithoutDiscount);
                    phOldPrice.Visible = false;
                }

                if (finalPriceWithoutDiscountBase != finalPriceWithDiscountBase)
                {
                    lblFinalPriceWithDiscount.Text = PriceHelper.FormatPrice(finalPriceWithDiscount);
                    phDiscount.Visible = true;
                }
                else
                {
                    phDiscount.Visible = false;
                }

                if (phDiscount.Visible)
                {
                    lblPriceValue.CssClass = string.Empty;
                }
                else
                {
                    lblPriceValue.CssClass = "productPrice";
                }

                if (phDiscount.Visible || phOldPrice.Visible)
                {
                    lblPrice.Text = GetLocaleResourceString("Products.FinalPriceWithoutDiscount");
                }
            }
            else
                this.Visible = false;
        }

        public int ProductVariantID
        {
            get
            {
                object obj2 = this.ViewState["ProductVariantID"];
                if (obj2 != null)
                    return (int)obj2;
                else
                    return 0;
            }
            set
            {
                this.ViewState["ProductVariantID"] = value;
            }
        }
    }
}