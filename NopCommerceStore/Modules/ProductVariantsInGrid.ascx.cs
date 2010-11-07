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
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Audit;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.Localization;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class ProductVariantsInGridControl : BaseNopUserControl
    {
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
                ProductVariantCollection productVariants = product.ProductVariants;
                if (productVariants.Count > 0)
                {
                    rptVariants.DataSource = productVariants;
                    rptVariants.DataBind();
                }
                else
                    this.Visible = false;
            }
            else
                this.Visible = false;
        }

        protected void rptVariants_OnItemCommand(Object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "AddToCart" || e.CommandName == "AddToWishlist")
            {
                NumericTextBox txtQuantity = e.Item.FindControl("txtQuantity") as NumericTextBox;
                Label productVariantID = e.Item.FindControl("ProductVariantID") as Label;
                ProductAttributesControl ctrlProductAttributes = e.Item.FindControl("ctrlProductAttributes") as ProductAttributesControl;
                Label lblError = e.Item.FindControl("lblError") as Label;

                try
                {
                    if (e.CommandName == "AddToCart")
                    {
                        List<string> addToCartWarnings = ShoppingCartManager.AddToCart(ShoppingCartTypeEnum.ShoppingCart,
                            Convert.ToInt32(productVariantID.Text),
                            ctrlProductAttributes.SelectedAttributes, 
                            txtQuantity.Value);
                        if (addToCartWarnings.Count == 0)
                        {
                            Response.Redirect("~/ShoppingCart.aspx");
                        }
                        else
                        {
                            StringBuilder addToCartWarningsSb = new StringBuilder();
                            for (int i = 0; i < addToCartWarnings.Count; i++)
                            {
                                addToCartWarningsSb.Append(Server.HtmlEncode(addToCartWarnings[i]));
                                if (i != addToCartWarnings.Count - 1)
                                {
                                    addToCartWarningsSb.Append("<br />");
                                }
                            }
                            lblError.Text = addToCartWarningsSb.ToString();
                        }
                    }

                    if (e.CommandName == "AddToWishlist")
                    {
                        List<string> addToCartWarnings = ShoppingCartManager.AddToCart(ShoppingCartTypeEnum.Wishlist,
                            Convert.ToInt32(productVariantID.Text),
                            ctrlProductAttributes.SelectedAttributes, 
                            txtQuantity.Value);
                        if (addToCartWarnings.Count == 0)
                        {
                            Response.Redirect("~/Wishlist.aspx");
                        }
                        else
                        {
                            StringBuilder addToCartWarningsSb = new StringBuilder();
                            for (int i = 0; i < addToCartWarnings.Count; i++)
                            {
                                addToCartWarningsSb.Append(Server.HtmlEncode(addToCartWarnings[i]));
                                if (i != addToCartWarnings.Count - 1)
                                {
                                    addToCartWarningsSb.Append("<br />");
                                }
                            }
                            lblError.Text = addToCartWarningsSb.ToString();
                        }
                    }
                }
                catch (Exception exc)
                {
                    LogManager.InsertLog(LogTypeEnum.CustomerError, exc.Message, exc);
                    lblError.Text = Server.HtmlEncode(exc.Message);
                }
            }
        }

        protected void rptVariants_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ProductVariant productVariant = e.Item.DataItem as ProductVariant;
                Panel pnlDownloadSample = e.Item.FindControl("pnlDownloadSample") as Panel;
                HyperLink hlDownloadSample = e.Item.FindControl("hlDownloadSample") as HyperLink;
                Image iProductVariantPicture = e.Item.FindControl("iProductVariantPicture") as Image;
                NumericTextBox txtQuantity = e.Item.FindControl("txtQuantity") as NumericTextBox;
                Button btnAddToCart = e.Item.FindControl("btnAddToCart") as Button;
                Button btnAddToWishlist = e.Item.FindControl("btnAddToWishlist") as Button;

                if (iProductVariantPicture != null)
                {
                    Picture productVariantPicture = productVariant.Picture;
                    string pictureUrl = PictureManager.GetPictureUrl(productVariantPicture, SettingManager.GetSettingValueInteger("Media.Product.VariantImageSize", 125), false);
                    if (String.IsNullOrEmpty(pictureUrl))
                        iProductVariantPicture.Visible = false;
                    else
                        iProductVariantPicture.ImageUrl = pictureUrl;
                    iProductVariantPicture.ToolTip = String.Format(GetLocaleResourceString("Media.Product.ImageAlternateTextFormat"), productVariant.Name);
                    iProductVariantPicture.AlternateText = String.Format(GetLocaleResourceString("Media.Product.ImageAlternateTextFormat"), productVariant.Name);
                }

                btnAddToWishlist.Visible = SettingManager.GetSettingValueBoolean("Common.EnableWishlist");

                if (!productVariant.DisableBuyButton)
                {
                    txtQuantity.ValidationGroup = string.Format("ProductVariant{0}", productVariant.ProductVariantID);
                    btnAddToCart.ValidationGroup = string.Format("ProductVariant{0}", productVariant.ProductVariantID);
                    btnAddToWishlist.ValidationGroup = string.Format("ProductVariant{0}", productVariant.ProductVariantID);

                    txtQuantity.Value = productVariant.OrderMinimumQuantity;
                }
                else
                {
                    txtQuantity.Visible = false;
                    btnAddToCart.Visible = false;
                    btnAddToWishlist.Visible = false;
                }

                if (pnlDownloadSample != null && hlDownloadSample != null)
                {
                    if (productVariant.IsDownload && productVariant.HasSampleDownload)
                    {
                        pnlDownloadSample.Visible = true;
                        hlDownloadSample.NavigateUrl = DownloadManager.GetSampleDownloadUrl(productVariant);
                    }
                    else
                    {
                        pnlDownloadSample.Visible = false;
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