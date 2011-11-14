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
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web.UI.WebControls;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Products.Attributes;
using NopSolutions.NopCommerce.BusinessLogic.Promo.Discounts;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.Web.Modules
{
    using BusinessLogic.Utils;

    public partial class OrderSummaryControl : BaseNopUserControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.BindData();
        }

        public void BindData()
        {
            ShoppingCart Cart = ShoppingCartManager.GetCurrentShoppingCart(ShoppingCartTypeEnum.ShoppingCart);

            if (Cart.Count > 0)
            {
                pnlEmptyCart.Visible = false;
                pnlCart.Visible = true;

                phCoupon.Visible = DiscountManager.HasDiscountsWithCouponCode();

                rptShoppingCart.DataSource = Cart;
                rptShoppingCart.DataBind();
                ValidateCart();
            }
            else
            {
                pnlEmptyCart.Visible = true;
                pnlCart.Visible = false;
            }

            this.ctrlOrderTotals.BindData();
        }

        /// <summary>
        /// Validates shopping cart
        /// </summary>
        /// <returns>Indicates whether there're some warnings/errors</returns>
        protected bool ValidateCart()
        {
            bool hasErrors = false;
            foreach (RepeaterItem item in rptShoppingCart.Items)
            {
                TextBox txtQuantity = item.FindControl("txtQuantity") as TextBox;
                Label lblShoppingCartItemID = item.FindControl("lblShoppingCartItemID") as Label;
                CheckBox cbRemoveFromCart = item.FindControl("cbRemoveFromCart") as CheckBox;
                Panel pnlWarnings = item.FindControl("pnlWarnings") as Panel;
                Label lblWarning = item.FindControl("lblWarning") as Label;

                int shoppingCartItemID = 0;
                int quantity = 0;
                if (txtQuantity != null && lblShoppingCartItemID != null && cbRemoveFromCart != null)
                {
                    int.TryParse(lblShoppingCartItemID.Text, out shoppingCartItemID);

                    if (!cbRemoveFromCart.Checked)
                    {
                        int.TryParse(txtQuantity.Text, out quantity);
                        ShoppingCartItem sci = ShoppingCartManager.GetShoppingCartItemByID(shoppingCartItemID);

                        List<string> warnings = ShoppingCartManager.GetShoppingCartItemWarnings(sci.ShoppingCartType,
                            sci.ProductVariantID, sci.AttributesXML, quantity);

                        if (warnings.Count > 0)
                        {
                            hasErrors = true;
                            if (pnlWarnings != null && lblWarning != null)
                            {
                                pnlWarnings.Visible = true;
                                lblWarning.Visible = true;

                                StringBuilder addToCartWarningsSb = new StringBuilder();
                                for (int i = 0; i < warnings.Count; i++)
                                {
                                    addToCartWarningsSb.Append(Server.HtmlEncode(warnings[i]));
                                    if (i != warnings.Count - 1)
                                    {
                                        addToCartWarningsSb.Append("<br />");
                                    }
                                }

                                lblWarning.Text = addToCartWarningsSb.ToString();
                            }
                        }
                    }
                }
            }
            return hasErrors;
        }

        protected void UpdateShoppingCart()
        {
            if (!IsShoppingCart)
                return;

            bool hasErrors = ValidateCart();

            if (!hasErrors)
            {
                foreach (RepeaterItem item in rptShoppingCart.Items)
                {
                    TextBox txtQuantity = item.FindControl("txtQuantity") as TextBox;
                    Label lblShoppingCartItemID = item.FindControl("lblShoppingCartItemID") as Label;
                    CheckBox cbRemoveFromCart = item.FindControl("cbRemoveFromCart") as CheckBox;
                    int shoppingCartItemID = 0;
                    int quantity = 0;
                    if (txtQuantity != null && lblShoppingCartItemID != null && cbRemoveFromCart != null)
                    {
                        int.TryParse(lblShoppingCartItemID.Text, out shoppingCartItemID);
                        if (cbRemoveFromCart.Checked)
                        {
                            ShoppingCartManager.DeleteShoppingCartItem(shoppingCartItemID, true);
                        }
                        else
                        {
                            int.TryParse(txtQuantity.Text, out quantity);
                            List<string> addToCartWarning = ShoppingCartManager.UpdateCart(shoppingCartItemID, quantity, true);
                        }
                    }
                }

                Response.Redirect("~/ShoppingCart.aspx");
            }
        }

        protected void ContinueShopping()
        {
            string lastProductPageVisited = NopContext.Current.LastProductPageVisited;
            if (!String.IsNullOrEmpty(lastProductPageVisited))
                Response.Redirect(lastProductPageVisited);
            else
                Response.Redirect("~/Default.aspx");
        }

        protected void Checkout()
        {
            if (NopContext.Current.User == null || NopContext.Current.User.IsGuest)
            {
                string loginURL = CommonHelper.GetLoginPageURL(true, true);
                Response.Redirect(loginURL);
            }
            else
            {
                Response.Redirect("~/Checkout.aspx");
            }
        }

        protected void ApplyCouponCode()
        {
            string couponCode = this.txtCouponCode.Text.Trim();
            CustomerManager.ApplyCouponCode(couponCode);
            this.BindData();
        }

        public string GetProductVariantName(ShoppingCartItem shoppingCartItem)
        {
            ProductVariant productVariant = shoppingCartItem.ProductVariant;
            if (productVariant != null)
                return productVariant.FullProductName;
            return "Not available";
        }

        public string GetProductVariantImageUrl(ShoppingCartItem shoppingCartItem)
        {
            string pictureUrl = String.Empty;
            ProductVariant productVariant = shoppingCartItem.ProductVariant;
            if (productVariant != null)
            {
                Picture productVariantPicture = productVariant.Picture;
                pictureUrl = PictureManager.GetPictureUrl(productVariantPicture, SettingManager.GetSettingValueInteger("Media.ShoppingCart.ThumbnailImageSize", 80), false);
                if (String.IsNullOrEmpty(pictureUrl))
                {
                    Product product = productVariant.Product;
                    ProductPictureCollection productPictures = product.ProductPictures;
                    if (productPictures.Count > 0)
                    {
                        pictureUrl = PictureManager.GetPictureUrl(productPictures[0].PictureID, SettingManager.GetSettingValueInteger("Media.ShoppingCart.ThumbnailImageSize", 80));
                    }
                    else
                    {
                        pictureUrl = PictureManager.GetDefaultPictureUrl(SettingManager.GetSettingValueInteger("Media.ShoppingCart.ThumbnailImageSize", 80));
                    }
                }
            }
            return pictureUrl;
        }

        public string GetProductURL(ShoppingCartItem shoppingCartItem)
        {
            ProductVariant productVariant = shoppingCartItem.ProductVariant;
            if (productVariant != null)
                return SEOHelper.GetProductURL(productVariant.ProductID);
            return string.Empty;
        }

        public string GetAttributeDescription(ShoppingCartItem shoppingCartItem)
        {
            string result = ProductAttributeHelper.FormatAttributes(shoppingCartItem.ProductVariant, shoppingCartItem.AttributesXML);
            return result;
        }

        public string GetShoppingCartItemUnitPriceString(ShoppingCartItem shoppingCartItem)
        {
            StringBuilder sb = new StringBuilder();
            decimal shoppingCartUnitPriceWithDiscountBase = TaxManager.GetPrice(shoppingCartItem.ProductVariant, PriceHelper.GetUnitPrice(shoppingCartItem, true));
            decimal shoppingCartUnitPriceWithDiscount = CurrencyManager.ConvertCurrency(shoppingCartUnitPriceWithDiscountBase, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingCurrency);

            string unitPriceString = "";
            if (Request.Cookies["Currency"] != null && Request.Cookies["Currency"].Value == "USD")
            {
                unitPriceString = PriceHelper.FormatPrice(PriceConverter.ToUsd(shoppingCartUnitPriceWithDiscount)) + "$";
            }
            else
            {
                unitPriceString = PriceHelper.FormatPrice(shoppingCartUnitPriceWithDiscount);
            }

            sb.Append("<span class=\"productPrice\">");
            sb.Append(unitPriceString);
            sb.Append("</span>");
            return sb.ToString();
        }

        public string GetShoppingCartItemSubTotalString(ShoppingCartItem shoppingCartItem)
        {
            StringBuilder sb = new StringBuilder();
            decimal shoppingCartItemSubTotalWithDiscountBase = TaxManager.GetPrice(shoppingCartItem.ProductVariant, PriceHelper.GetSubTotal(shoppingCartItem, true));
            decimal shoppingCartItemSubTotalWithDiscount = CurrencyManager.ConvertCurrency(shoppingCartItemSubTotalWithDiscountBase, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingCurrency);

            string subTotalString = "";
            if (Request.Cookies["Currency"] != null && Request.Cookies["Currency"].Value == "USD")
            {
                subTotalString = PriceHelper.FormatPrice(PriceConverter.ToUsd(shoppingCartItemSubTotalWithDiscount)) + "$";
            }
            else
            {
                subTotalString = PriceHelper.FormatPrice(shoppingCartItemSubTotalWithDiscount);
            }

            sb.Append("<span class=\"productPrice\">");
            sb.Append(subTotalString);
            sb.Append("</span>");

            decimal shoppingCartItemDiscountBase = TaxManager.GetPrice(shoppingCartItem.ProductVariant, PriceHelper.GetDiscountAmount(shoppingCartItem));
            if (shoppingCartItemDiscountBase > decimal.Zero)
            {
                decimal shoppingCartItemDiscount = CurrencyManager.ConvertCurrency(shoppingCartItemDiscountBase, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingCurrency);
                string discountString = PriceHelper.FormatPrice(shoppingCartItemDiscount);

                sb.Append("<br />");
                sb.Append(GetLocaleResourceString("ShoppingCart.ItemYouSave"));
                sb.Append("&nbsp;&nbsp;");
                sb.Append(discountString);
            }
            return sb.ToString();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateShoppingCart();
        }

        protected void btnContinueShopping_Click(object sender, EventArgs e)
        {
            ContinueShopping();
        }

        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            Checkout();
        }

        protected void btnApplyCouponCode_Click(object sender, EventArgs e)
        {
            ApplyCouponCode();
        }

        [DefaultValue(false)]
        public bool IsShoppingCart
        {
            get
            {
                object obj2 = this.ViewState["IsShoppingCart"];
                return ((obj2 != null) && ((bool)obj2));
            }
            set
            {
                this.ViewState["IsShoppingCart"] = value;
            }
        }
    }
}