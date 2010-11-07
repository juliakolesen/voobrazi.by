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
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Localization;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Products.Specs;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class CompareProductsControl : BaseNopUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnClearCompareProductsList_Click(object sender, EventArgs e)
        {
            ProductManager.ClearCompareProducts();
            Page.Response.Redirect("~/Default.aspx");
        }

        protected override void OnInit(EventArgs e)
        {
            this.EnsureChildControls();
            base.OnInit(e);
        }

        protected override void CreateChildControls()
        {
            if (!base.ChildControlsCreated)
            {
                this.CreateCompareTable();
                base.CreateChildControls();
                base.ChildControlsCreated = true;
            }
        }

        private void btnRemoveFromList_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Remove")
            {
                ProductManager.RemoveProductFromCompareList(Convert.ToInt32(e.CommandArgument));
                Page.Response.Redirect("~/CompareProducts.aspx");
            }
        }

        private HtmlTableCell AddCell(HtmlTableRow row, string text)
        {
            HtmlTableCell cell = new HtmlTableCell();
            cell.InnerHtml = text;
            row.Cells.Add(cell);
            return cell;
        }

        public void CreateCompareTable()
        {
            this.tblCompareProducts.Rows.Clear();
            this.tblCompareProducts.Width = "100%";
            ProductCollection compareProducts = ProductManager.GetCompareProducts();
            if (compareProducts.Count > 0)
            {
                HtmlTableRow headerRow = new HtmlTableRow();
                this.AddCell(headerRow, "&nbsp;");
                HtmlTableRow productNameRow = new HtmlTableRow();
                this.AddCell(productNameRow, "&nbsp;");
                HtmlTableRow priceRow = new HtmlTableRow();
                HtmlTableCell cell = new HtmlTableCell();
                cell.InnerText = GetLocaleResourceString("Products.CompareProductsPrice");
                cell.Align = "center";
                priceRow.Cells.Add(cell);

                List<int> allAttributeIDs = new List<int>();
                foreach (Product product in compareProducts)
                {
                    ProductSpecificationAttributeCollection productSpecificationAttributes = SpecificationAttributeManager.GetProductSpecificationAttributesByProductID(product.ProductID, null, true);
                    foreach (ProductSpecificationAttribute attribute in productSpecificationAttributes)
                        if (!allAttributeIDs.Contains(attribute.SpecificationAttributeOptionID))
                            allAttributeIDs.Add(attribute.SpecificationAttributeOptionID);
                }

                foreach (Product product in compareProducts)
                {
                    HtmlTableCell headerCell = new HtmlTableCell();
                    HtmlGenericControl headerCellDiv = new HtmlGenericControl("div");
                    Button btnRemoveFromList = new Button();
                    btnRemoveFromList.ToolTip = GetLocaleResourceString("Products.CompareProductsRemoveFromList");
                    btnRemoveFromList.Text = GetLocaleResourceString("Products.CompareProductsRemoveFromList");
                    btnRemoveFromList.CommandName = "Remove";
                    btnRemoveFromList.Command += new CommandEventHandler(this.btnRemoveFromList_Command);
                    btnRemoveFromList.CommandArgument = product.ProductID.ToString();
                    btnRemoveFromList.CausesValidation = false;
                    btnRemoveFromList.CssClass = "removeButton";
                    btnRemoveFromList.ID = "btnRemoveFromList" + product.ProductID.ToString();
                    headerCellDiv.Controls.Add(btnRemoveFromList);

                    HtmlGenericControl productImagePanel = new HtmlGenericControl("p");
                    productImagePanel.Attributes.Add("align", "center");

                    HtmlImage productImage = new HtmlImage();
                    productImage.Border = 0;
                    //productImage.Align = "center";
                    productImage.Alt = "Product image";
                    ProductPictureCollection productPictures = product.ProductPictures;
                    if (productPictures.Count > 0)
                        productImage.Src = PictureManager.GetPictureUrl(productPictures[0].Picture, SettingManager.GetSettingValueInteger("Media.Product.ThumbnailImageSize", 125), true);
                    else
                        productImage.Src = PictureManager.GetDefaultPictureUrl(SettingManager.GetSettingValueInteger("Media.Product.ThumbnailImageSize", 125));
                    productImagePanel.Controls.Add(productImage);

                    headerCellDiv.Controls.Add(productImagePanel);



                    headerCell.Controls.Add(headerCellDiv);
                    headerRow.Cells.Add(headerCell);
                    HtmlTableCell productNameCell = new HtmlTableCell();
                    HyperLink productLink = new HyperLink();
                    productLink.Text = Server.HtmlEncode(product.Name);
                    productLink.NavigateUrl = SEOHelper.GetProductURL(product);
                    productLink.Attributes.Add("class", "link");
                    productNameCell.Align = "center";
                    productNameCell.Controls.Add(productLink);
                    productNameRow.Cells.Add(productNameCell);
                    HtmlTableCell priceCell = new HtmlTableCell();
                    priceCell.Align = "center";
                    ProductVariantCollection productVariantCollection = product.ProductVariants;
                    if (productVariantCollection.Count > 0)
                    {
                        ProductVariant productVariant = productVariantCollection[0];

                        //decimal oldPrice = productVariant.OldPrice;
                        //decimal oldPriceConverted = CurrencyManager.ConvertCurrency(oldPrice, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingCurrency);

                        decimal finalPriceWithoutDiscountBase = TaxManager.GetPrice(productVariant, PriceHelper.GetFinalPrice(productVariant, false));
                        decimal finalPriceWithoutDiscount = CurrencyManager.ConvertCurrency(finalPriceWithoutDiscountBase, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingCurrency);

                        priceCell.InnerText = PriceHelper.FormatPrice(finalPriceWithoutDiscount);
                    }
                    priceRow.Cells.Add(priceCell);
                }
                productNameRow.Attributes.Add("class", "productName");
                priceRow.Attributes.Add("class", "productPrice");
                this.tblCompareProducts.Rows.Add(headerRow);
                this.tblCompareProducts.Rows.Add(productNameRow);
                this.tblCompareProducts.Rows.Add(priceRow);

                if (allAttributeIDs.Count > 0)
                {
                    foreach (int specificationAttributeID in allAttributeIDs)
                    {
                        //SpecificationAttribute attribute = SpecificationAttributeManager.GetSpecificationAttributeByID(specificationAttributeID);
                        SpecificationAttributeOption attributeOption = SpecificationAttributeManager.GetSpecificationAttributeOptionByID(specificationAttributeID);
                        HtmlTableRow productRow = new HtmlTableRow();
                        this.AddCell(productRow, Server.HtmlEncode(attributeOption.SpecificationAttribute.Name)).Align = "left";

                        foreach (Product product2 in compareProducts)
                        {
                            HtmlTableCell productCell = new HtmlTableCell();
                            {
                                ProductSpecificationAttributeCollection productSpecificationAttributes2 = SpecificationAttributeManager.GetProductSpecificationAttributesByProductID(product2.ProductID, null, true);
                                foreach (ProductSpecificationAttribute attribute2 in productSpecificationAttributes2)
                                    if (attributeOption.SpecificationAttributeOptionID == attribute2.SpecificationAttributeOptionID)
                                        productCell.InnerHtml = (!String.IsNullOrEmpty(attribute2.SpecificationAttributeOption.Name)) ? Server.HtmlEncode(attribute2.SpecificationAttributeOption.Name) : "&nbsp;";
                            }
                            productCell.Align = "center";
                            productCell.VAlign = "top";
                            productRow.Cells.Add(productCell);
                        }
                        this.tblCompareProducts.Rows.Add(productRow);
                    }
                }
                string width = Math.Round((decimal)(90M / compareProducts.Count), 0).ToString() + "%";
                for (int i = 0; i < this.tblCompareProducts.Rows.Count; i++)
                {
                    HtmlTableRow row = this.tblCompareProducts.Rows[i];
                    for (int j = 1; j < row.Cells.Count; j++)
                    {
                        if (j == (row.Cells.Count - 1))
                        {
                            row.Cells[j].Style.Add("width", width);
                            row.Cells[j].Style.Add("text-align", "center");
                        }
                        else
                        {
                            row.Cells[j].Style.Add("width", width);
                            row.Cells[j].Style.Add("text-align", "center");
                        }
                    }
                }
            }
            else
            {
                btnClearCompareProductsList.Visible = false;
                tblCompareProducts.Visible = false;
            }
        }
    }
}