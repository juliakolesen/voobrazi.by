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
using System.Web.UI.WebControls;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.Categories;
using NopSolutions.NopCommerce.Web.MasterPages;
using System.Collections.Generic;
using NopSolutions.NopCommerce.BusinessLogic.Products.Specs;
using System.Text.RegularExpressions;

namespace NopSolutions.NopCommerce.Web.Modules
{
	public partial class RelatedProductsControl : BaseNopUserControl
	{
        private int pageSize = 6;
        private int totalItemCount = 0;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
				BindData();
		}

		protected void BindData()
		{
			Product product = ProductManager.GetProductByID(ProductID);
			if (product != null)
			{
                int totalRecords = 0;
                RelatedProductCollection relatedProducts = ProductManager.GetRelatedProductsByProductID1Paged(ProductID, 
                    CurrentPageIndex, pageSize, ref totalRecords);
                
				if (relatedProducts.Count > 0)
				{
					this.Visible = true;
					dlRelatedProducts.DataSource = relatedProducts;
					dlRelatedProducts.DataBind();
                    this.relatedProductPager.PageSize = pageSize;
                    this.relatedProductPager.TotalRecords = totalRecords;
                    this.relatedProductPager.PageIndex = CurrentPageIndex;
				}
				else
					this.Visible = false;
			}
			else
				this.Visible = false;
		}

        public int CurrentPageIndex
        {
            get
            {
                int _pageIndex = CommonHelper.QueryStringInt(relatedProductPager.QueryStringProperty);
                _pageIndex--;
                if (_pageIndex < 0)
                    _pageIndex = 0;
                return _pageIndex;
            }
        }

		protected void dlRelatedProducts_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				RelatedProduct relatedProduct = e.Item.DataItem as RelatedProduct;
				Product product = relatedProduct.Product2;

				if (relatedProduct != null && product != null)
				{
					string productURL = SEOHelper.GetProductURL(product);

					Label lblPrice = e.Item.FindControl("lblPrice") as Label;

					if (product.ProductVariants.Count > 0)
					{
						if (!product.HasMultipleVariants)
						{
							ProductVariant productVariant = product.ProductVariants[0];

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
					Image hlImageLink = e.Item.FindControl("hlImageLink") as Image;
					if (hlImageLink != null)
					{
						ProductPictureCollection productPictures = product.ProductPictures;
						if (productPictures.Count > 0)
							hlImageLink.ImageUrl = PictureManager.GetPictureUrl(productPictures[0].Picture, SettingManager.GetSettingValueInteger("Media.Product.ThumbnailImageSize", 84), true);
						else
							hlImageLink.ImageUrl = PictureManager.GetDefaultPictureUrl(SettingManager.GetSettingValueInteger("Media.Product.ThumbnailImageSize", 84));
						//hlImageLink.NavigateUrl = productURL;
						//hlImageLink.ToolTip = String.Format(GetLocaleResourceString("Media.Product.ImageLinkTitleFormat"), product.Name);
						//hlImageLink.Text = String.Format(GetLocaleResourceString("Media.Product.ImageAlternateTextFormat"), product.Name);
					}

					HyperLink hlProduct = e.Item.FindControl("hlProduct") as HyperLink;
					if (hlProduct != null)
					{
						hlProduct.NavigateUrl = productURL;
						//hlProduct.Text = product.Name;
					}
				}
			}
		}

		public int ProductID
		{
			get
			{
				return CommonHelper.QueryStringInt("ProductID");
			}
		}
	}
}