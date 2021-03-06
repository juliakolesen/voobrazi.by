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
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Shipping;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.BusinessLogic.Utils;

namespace NopSolutions.NopCommerce.Web.Modules
{

    public partial class OrderTotalsControl : BaseNopUserControl
    {
        public void BindData()
        {
            ShoppingCart Cart = ShoppingCartManager.GetCurrentShoppingCart(ShoppingCartTypeEnum.ShoppingCart);

            decimal indOrderTotal = 0;
            IndividualOrderCollection indOrders = new IndividualOrderCollection();
            if (NopContext.Current.Session != null)
            {
                Guid CustomerSessionGUID = NopContext.Current.Session.CustomerSessionGUID;
                indOrders = IndividualOrderManager.GetIndividualOrderByCurrentUserSessionGuid(CustomerSessionGUID);
                indOrderTotal = IndividualOrderManager.GetTotalPriceIndOrders(indOrders);
                if (Request.Cookies["Currency"] != null && Request.Cookies["Currency"].Value == "USD")
                {
                    indOrderTotal = Math.Round(PriceConverter.ToUsd(indOrderTotal));
                }
            }

            if (Cart.Count > 0 || indOrders.Count > 0)
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
                    shoppingCartSubTotal += indOrderTotal;
                    lblSubTotalAmount.Text = PriceHelper.FormatPrice(shoppingCartSubTotal);
                    if (Request.Cookies["Currency"] != null && Request.Cookies["Currency"].Value == "USD")
                    {
                        lblSubTotalAmount.Text += "$";
                    }

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
                        if (Request.Cookies["Currency"] != null && Request.Cookies["Currency"].Value == "USD")
                        {
                            lblShippingAmount.Text += "$";
                        }

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
                    shoppingCartTotal += indOrderTotal;
                    lblTotalAmount.Text = PriceHelper.FormatPrice(shoppingCartTotal);
                    if (Request.Cookies["Currency"] != null && Request.Cookies["Currency"].Value == "USD")
                    {
                        lblTotalAmount.Text += "$";
                    }

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