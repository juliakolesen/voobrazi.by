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
using System.Text;
using System.Web.UI.WebControls;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Products.Specs;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.Web.Modules
{

    public partial class ProductInfoControl : BaseNopUserControl
    {
        protected string AlternateText { get; set; }

        protected int smallImageSize = 100;

        protected int middleImageSize = 335;

        protected int largeImageSize = 500;

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
                ProductVariantCollection productVariantCollection = product.ProductVariants;
                ProductVariant productVariant = null;
                if (productVariantCollection.Count > 0)
                {
                    if (!product.HasMultipleVariants)
                    {
                        productVariant = productVariantCollection[0];

                        decimal finalPriceWithoutDiscountBase = TaxManager.GetPrice(productVariant, PriceHelper.GetFinalPrice(productVariant, false));

                        lblPrice2.Text = PriceHelper.FormatPrice(finalPriceWithoutDiscountBase);
                        if (Request.Cookies["Currency"] != null && Request.Cookies["Currency"].Value == "USD")
                        {
                            lblPrice1.Text = lblPrice2.Text = lblPrice2.Text + "$";
                        }
                        else
                        {
                            lblPrice1.Text = lblPrice2.Text + " руб";
                        }
                    }
                    else
                    {
                        productVariant = product.MinimalPriceProductVariant;
                        if (productVariant != null)
                        {
                            decimal fromPriceBase = TaxManager.GetPrice(productVariant, PriceHelper.GetFinalPrice(productVariant, false));
                            decimal fromPrice = CurrencyManager.ConvertCurrency(fromPriceBase, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingCurrency);
                            lblPrice2.Text = String.Format(GetLocaleResourceString("Products.PriceRangeFromText"), PriceHelper.FormatPrice(fromPrice));
                            lblPrice1.Text = String.Format(GetLocaleResourceString("Products.PriceRangeFromText"), PriceHelper.FormatPrice(fromPrice));
                        }
                    }
                }

                lProductName.Text = Server.HtmlEncode(product.Name);
                lShortDescription.Text = product.ShortDescription;
                lFullDescription.Text = product.FullDescription;
                imgProduct.Alt = AlternateText = String.Format(GetLocaleResourceString("Media.Product.ImageAlternateTextFormat"), product.Name);

                if (product.ProductPictures.Count > 0)
                {
                    imgProduct.Src = PictureManager.GetPictureUrl(product.ProductPictures[0].PictureID, middleImageSize);
                    bigImage.HRef = PictureManager.GetPictureUrl(product.ProductPictures[0].PictureID, largeImageSize);
                }
                else
                {
                    imgProduct.Src = PictureManager.GetDefaultPictureUrl(middleImageSize);
                    bigImage.HRef = PictureManager.GetDefaultPictureUrl(largeImageSize); ;
                }
                ProductPictureCollection productPictures = product.ProductPictures;
                if (productPictures.Count > 1)
                {
                    this.dlImages.DataSource = productPictures;
                    this.dlImages.DataBind();
                }

                lbOrder.CommandArgument = product.ProductID.ToString();
                lbOrderAndCheckout.CommandArgument = product.ProductID.ToString();
                tblOrderButtons.Visible = productVariantCollection.Count != 0 && productVariantCollection[0].StockQuantity != 0;


                StringBuilder attributes = new StringBuilder();
                foreach (ProductSpecificationAttribute psa in SpecificationAttributeManager.GetProductSpecificationAttributesByProductID(product.ProductID, false, null))
                {
                    if (psa.SpecificationAttribute.Name == "Уникальное предложение")
                        pUniqueProposal.Visible = psa.SpecificationAttributeOption.Name == "Да";
                    else
                        attributes.AppendFormat(@"<p><span class=""pink"">{0}:</span> {1}</p>", psa.SpecificationAttribute.Name, psa.SpecificationAttributeOption.Name);
                }

                lblAttributes.Text += attributes.ToString();
                if (NopContext.Current.Session == null)
                    NopContext.Current.Session = NopContext.Current.GetSession(true);
                Guid CustomerSessionGUID = NopContext.Current.Session.CustomerSessionGUID;
                ViewedItemManager.InsertViewedItem(CustomerSessionGUID, productVariant.ProductVariantID, DateTime.Now); 
            }
            else
                Visible = false;
        }

        protected override void OnPreRender(EventArgs e)
        {
            string slimBox = CommonHelper.GetStoreLocation() + "Scripts/slimbox2.js";
            Page.ClientScript.RegisterClientScriptInclude(slimBox, slimBox);

            base.OnPreRender(e);
        }

        protected void btnAddToCart_Click(object sender, CommandEventArgs e)
        {
            int productID = Convert.ToInt32(e.CommandArgument);
            int productVariantID = 0;
            if (ProductManager.DirectAddToCartAllowed(productID, out productVariantID))
            {
                List<string> addToCartWarnings = ShoppingCartManager.AddToCart(ShoppingCartTypeEnum.ShoppingCart,
                    productVariantID, string.Empty, 1);
                string productURL = SEOHelper.GetProductURL(productID);
                Response.Redirect(productURL);
            }
            else
            {
                string productURL = SEOHelper.GetProductURL(productID);
                Response.Redirect(productURL);
            }
        }

        public int ProductID
        {
            get
            {
                return CommonHelper.QueryStringInt("ProductID");
            }
        }

        protected void lbOrderAndCheckout_Click(object sender, CommandEventArgs e)
        {
            int productID = Convert.ToInt32(e.CommandArgument);
            int productVariantID = 0;
            if (ProductManager.DirectAddToCartAllowed(productID, out productVariantID))
                ShoppingCartManager.AddToCart(ShoppingCartTypeEnum.ShoppingCart, productVariantID, string.Empty, 1);

            Response.Redirect("/ShoppingCart.aspx");
        }

        protected string GetPictureUrl(ProductPicture picture, int size)
        {
            return PictureManager.GetPictureUrl(picture.PictureID, size);
        }
    }
}