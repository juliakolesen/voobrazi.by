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
using System.Web.UI.WebControls;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.BusinessLogic.Tax;

namespace NopSolutions.NopCommerce.Web.Modules
{
	public partial class ProductBox1Control : BaseNopUserControl
	{
		Product product;

		protected void Page_Load(object sender, EventArgs e)
		{
		}

		public override void DataBind()
		{
			base.DataBind();
			BindData();
		}

		private void BindData()
		{
			if (product != null)
			{
				string productURL = SEOHelper.GetProductURL(product);

				hlProduct.Text = Server.HtmlEncode(product.Name);

				ProductPictureCollection productPictures = product.ProductPictures;
				if (productPictures.Count > 0)
				{
					hlImageLink.ImageUrl = PictureManager.GetPictureUrl(productPictures[0].Picture, ProductImageSize, true);
					hlImageLink.ToolTip = String.Format(GetLocaleResourceString("Media.Product.ImageLinkTitleFormat"), product.Name);
					hlImageLink.Text = String.Format(GetLocaleResourceString("Media.Product.ImageAlternateTextFormat"), product.Name);
				}
				else
				{
					hlImageLink.ImageUrl = PictureManager.GetDefaultPictureUrl(this.ProductImageSize);
					hlImageLink.ToolTip = String.Format(GetLocaleResourceString("Media.Product.ImageLinkTitleFormat"), product.Name);
					hlImageLink.Text = String.Format(GetLocaleResourceString("Media.Product.ImageAlternateTextFormat"), product.Name);
				}
				hlImageLink.NavigateUrl = productURL;

				lShortDescription.Text = product.ShortDescription;

				ProductVariantCollection productVariantCollection = product.ProductVariants;
				if (productVariantCollection.Count > 0)
				{
					if (!product.HasMultipleVariants)
					{
						ProductVariant productVariant = productVariantCollection[0];

						decimal oldPriceBase = TaxManager.GetPrice(productVariant, productVariant.OldPrice);
						decimal finalPriceWithoutDiscountBase = TaxManager.GetPrice(productVariant, PriceHelper.GetFinalPrice(productVariant, false));

						decimal oldPrice = CurrencyManager.ConvertCurrency(oldPriceBase, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingCurrency);
						decimal finalPriceWithoutDiscount = CurrencyManager.ConvertCurrency(finalPriceWithoutDiscountBase, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingCurrency);

						lblPrice.Text = PriceHelper.FormatPrice(finalPriceWithoutDiscountBase);
					}
					else
					{
						ProductVariant productVariant = product.MinimalPriceProductVariant;
						if (productVariant != null)
						{
							decimal fromPriceBase = TaxManager.GetPrice(productVariant, PriceHelper.GetFinalPrice(productVariant, false));
							decimal fromPrice = CurrencyManager.ConvertCurrency(fromPriceBase, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingCurrency);
							lblPrice.Text = String.Format(GetLocaleResourceString("Products.PriceRangeFromText"), PriceHelper.FormatPrice(fromPrice));
						}
					}
				}
				else
				{
					lblPrice.Visible = false;
                }

                #region scjaarge's change
                if (!lblPrice.Visible || lblPrice.Text.Trim() == string.Empty)
                {
                    hlImageLink.NavigateUrl = "javascript:return void(0);";
                    divProductInfoLink.Visible = false;
                    notAvailable.Visible = true;
                }
                #endregion scjaarge's change
            }
		}

		protected void btnProductDetails_Click(object sender, CommandEventArgs e)
		{
			int productID = Convert.ToInt32(e.CommandArgument);
			string productURL = SEOHelper.GetProductURL(productID);
			Response.Redirect(productURL);
		}

		protected void btnAddToCart_Click(object sender, CommandEventArgs e)
		{
			int productID = Convert.ToInt32(e.CommandArgument);
			int productVariantID = 0;
			if (ProductManager.DirectAddToCartAllowed(productID, out productVariantID))
			{
				List<string> addToCartWarnings = ShoppingCartManager.AddToCart(ShoppingCartTypeEnum.ShoppingCart,
					productVariantID, string.Empty, 1);
				if (addToCartWarnings.Count == 0)
				{
					Response.Redirect("~/ShoppingCart.aspx");
				}
				else
				{
					string productURL = SEOHelper.GetProductURL(productID);
					Response.Redirect(productURL);
				}
			}
			else
			{
				string productURL = SEOHelper.GetProductURL(productID);
				Response.Redirect(productURL);
			}
		}

		public Product Product
		{
			get
			{
				return product;
			}
			set
			{
				product = value;
			}
		}

		public int ProductImageSize
		{
			get
			{
				if (ViewState["ProductImageSize"] == null)
					return SettingManager.GetSettingValueInteger("Media.Product.ThumbnailImageSize", 196);
				else
					return (int)ViewState["ProductImageSize"];
			}
			set
			{
				ViewState["ProductImageSize"] = value;
			}
		}
	}
}