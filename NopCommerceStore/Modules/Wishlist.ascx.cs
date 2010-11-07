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
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Localization;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Products.Attributes;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.BusinessLogic.Shipping;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Media;


namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class WishlistControl : BaseNopUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Visible = SettingManager.GetSettingValueBoolean("Common.EnableWishlist");
        }

        public void BindData()
        {
            ShoppingCart Cart = ShoppingCartManager.GetShoppingCartByCustomerSessionGUID(ShoppingCartTypeEnum.Wishlist, this.CustomerSessionGuid);

            if (Cart.Count > 0)
            {
                pnlEmptyCart.Visible = false;
                pnlCart.Visible = true;

                rptShoppingCart.DataSource = Cart;
                rptShoppingCart.DataBind();
                ValidateWishlist();
            }
            else
            {
                pnlEmptyCart.Visible = true;
                pnlCart.Visible = false;
            }
        }

        /// <summary>
        /// Validates wishlist
        /// </summary>
        /// <returns>Indicates whether there're some warnings/errors</returns>
        protected bool ValidateWishlist()
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

        protected void UpdateWishlist()
        {
            if (!IsEditable)
                return;

            bool hasErrors = ValidateWishlist();

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
                            ShoppingCartManager.DeleteShoppingCartItem(shoppingCartItemID, false);
                        else
                        {
                            int.TryParse(txtQuantity.Text, out quantity);
                            List<string> addToCartWarning = ShoppingCartManager.UpdateCart(shoppingCartItemID, quantity, false);
                        }
                    }
                }

                Response.Redirect("~/Wishlist.aspx");
            }
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
            string unitPriceString = PriceHelper.FormatPrice(shoppingCartUnitPriceWithDiscount);

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
            string subTotalString = PriceHelper.FormatPrice(shoppingCartItemSubTotalWithDiscount);

            sb.Append("<span class=\"productPrice\">");
            sb.Append(subTotalString);
            sb.Append("</span>");

            decimal shoppingCartItemDiscountBase = TaxManager.GetPrice(shoppingCartItem.ProductVariant, PriceHelper.GetDiscountAmount(shoppingCartItem));
            if (shoppingCartItemDiscountBase > decimal.Zero)
            {
                decimal shoppingCartItemDiscount = CurrencyManager.ConvertCurrency(shoppingCartItemDiscountBase, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingCurrency);
                string discountString = PriceHelper.FormatPrice(shoppingCartItemDiscount);

                sb.Append("<br />");
                sb.Append(GetLocaleResourceString("Wishlist.ItemYouSave"));
                sb.Append("&nbsp;&nbsp;");
                sb.Append(discountString);
            }
            return sb.ToString();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateWishlist();
        }

        [DefaultValue(false)]
        public bool IsEditable
        {
            get
            {
                object obj2 = this.ViewState["IsEditable"];
                return ((obj2 != null) && ((bool)obj2));
            }
            set
            {
                this.ViewState["IsEditable"] = value;
            }
        }

        public Guid CustomerSessionGuid
        {
            get
            {
                object obj2 = this.ViewState["CustomerSessionGuid"];
                if (obj2 != null)
                    return (Guid)obj2;
                else
                    return Guid.Empty;
            }
            set
            {
                this.ViewState["CustomerSessionGuid"] = value;
            }
        }
    }
}