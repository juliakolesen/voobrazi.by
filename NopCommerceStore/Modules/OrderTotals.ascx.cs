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
using System.Web;
using System.Web.Caching;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Localization;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Products.Attributes;
using NopSolutions.NopCommerce.BusinessLogic.Promo.Discounts;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.BusinessLogic.Shipping;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Media;

namespace NopSolutions.NopCommerce.Web.Modules
{
	public partial class OrderTotalsControl : BaseNopUserControl
	{
		public void BindData()
		{
			ShoppingCart Cart = ShoppingCartManager.GetCurrentShoppingCart(ShoppingCartTypeEnum.ShoppingCart);

			if (Cart.Count > 0)
			{
				//payment method (if already selected)
				int paymentMethodID = 0;
				if (NopContext.Current.User != null)
					paymentMethodID = NopContext.Current.User.LastPaymentMethodID;

				//subtotal
				string SubTotalError = string.Empty;
				decimal shoppingCartSubTotalDiscountBase;
				decimal shoppingCartSubTotalBase = ShoppingCartManager.GetShoppingCartSubTotal(Cart, NopContext.Current.User, out shoppingCartSubTotalDiscountBase, ref SubTotalError);
				if (String.IsNullOrEmpty(SubTotalError))
				{
					decimal shoppingCartSubTotal = CurrencyManager.ConvertCurrency(shoppingCartSubTotalBase, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingCurrency);
					lblSubTotalAmount.Text = PriceHelper.FormatPrice(shoppingCartSubTotal);
					lblSubTotalAmount.CssClass = "productPrice";

					if (shoppingCartSubTotalDiscountBase > decimal.Zero)
					{
						decimal shoppingCartSubTotalDiscount = CurrencyManager.ConvertCurrency(shoppingCartSubTotalDiscountBase, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingCurrency);
						lblSubTotalDiscountAmount.Text = PriceHelper.FormatPrice(shoppingCartSubTotalDiscount, true, false);
						phSubTotalDiscount.Visible = true;
					}
					else
					{
						phSubTotalDiscount.Visible = false;
					}
				}
				else
				{
					lblSubTotalAmount.Text = GetLocaleResourceString("ShoppingCart.CalculatedDuringCheckout");
					lblSubTotalAmount.CssClass = string.Empty;
				}

				//shipping info
				bool shoppingCartRequiresShipping = ShippingManager.ShoppingCartRequiresShipping(Cart) && Session["SelfOrder"] == null;
				if (shoppingCartRequiresShipping)
				{
					decimal? shoppingCartShippingBase = ShippingManager.GetShoppingCartShippingTotal(Cart, NopContext.Current.User);
					if (shoppingCartShippingBase.HasValue)
					{
						decimal shoppingCartShipping = CurrencyManager.ConvertCurrency(shoppingCartShippingBase.Value, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingCurrency);
						lblShippingAmount.Text = PriceHelper.FormatShippingPrice(shoppingCartShipping, true);
						lblShippingAmount.CssClass = "productPrice";
					}
					else
					{
						lblShippingAmount.Text = GetLocaleResourceString("ShoppingCart.CalculatedDuringCheckout");
						lblShippingAmount.CssClass = string.Empty;
					}
				}
				else
				{
					lblShippingAmount.Text = GetLocaleResourceString("ShoppingCart.ShippingNotRequired");
					lblShippingAmount.CssClass = string.Empty;
				}

				//payment method fee
				bool displayPaymentMethodFee = true;
				decimal paymentMethodAdditionalFee = PaymentManager.GetAdditionalHandlingFee(paymentMethodID);
				decimal paymentMethodAdditionalFeeWithTaxBase = TaxManager.GetPaymentMethodAdditionalFee(paymentMethodAdditionalFee, NopContext.Current.User);
				if (paymentMethodAdditionalFeeWithTaxBase > decimal.Zero)
				{
					decimal paymentMethodAdditionalFeeWithTax = CurrencyManager.ConvertCurrency(paymentMethodAdditionalFeeWithTaxBase, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingCurrency);
					lblPaymentMethodAdditionalFee.Text = PriceHelper.FormatPaymentMethodAdditionalFee(paymentMethodAdditionalFeeWithTax, true);
				}
				else
				{
					displayPaymentMethodFee = false;
				}
				phPaymentMethodAdditionalFee.Visible = displayPaymentMethodFee;

				//tax
				bool displayTax = true;
				if (TaxManager.HideTaxInOrderSummary && NopContext.Current.TaxDisplayType == TaxDisplayTypeEnum.IncludingTax)
				{
					displayTax = false;
				}
				else
				{
					string TaxError = string.Empty;
					decimal shoppingCartTaxBase = TaxManager.GetTaxTotal(Cart, paymentMethodID, NopContext.Current.User, ref TaxError);
					decimal shoppingCartTax = CurrencyManager.ConvertCurrency(shoppingCartTaxBase, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingCurrency);

					//if (String.IsNullOrEmpty(TaxError))
					//{
					//    if (shoppingCartTaxBase == 0 && TaxManager.HideZeroTax)
					//    {
					//        displayTax = false;
					//    }
					//    else
					//    {
					//        lblTaxAmount.Text = PriceHelper.FormatPrice(shoppingCartTax, true, false);
					//        lblTaxAmount.CssClass = "productPrice";
					//    }
					//}
					//else
					//{
					//    lblTaxAmount.Text = GetLocaleResourceString("ShoppingCart.CalculatedDuringCheckout");
					//    lblTaxAmount.CssClass = string.Empty;
					//}
				}
				phTaxTotal.Visible = false;// displayTax;

				//total
				decimal? shoppingCartTotalBase = ShoppingCartManager.GetShoppingCartTotal(Cart, paymentMethodID, NopContext.Current.User);
				if (shoppingCartTotalBase.HasValue)
				{
					decimal shoppingCartTotal = CurrencyManager.ConvertCurrency(shoppingCartTotalBase.Value, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingCurrency);

					if (Session["SelfOrder"] != null)
						shoppingCartTotal -= ShippingManager.GetShoppingCartShippingTotal(Cart).Value;
					lblTotalAmount.Text = PriceHelper.FormatPrice(shoppingCartTotal, true, false);
					lblTotalAmount.CssClass = "productPrice";
				}
				else
				{
					lblTotalAmount.Text = GetLocaleResourceString("ShoppingCart.CalculatedDuringCheckout");
					lblTotalAmount.CssClass = string.Empty;
				}
			}
			else
			{
				this.Visible = false;
			}
		}
	}
}