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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NopSolutions.NopCommerce.BusinessLogic.ExportImport;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.BusinessLogic.Promo.Affiliates;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Web.Administration.Modules;

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class PricelistInfoControl : BaseNopAdministrationUserControl
    {
        private int CompareCultures(CultureInfo x, CultureInfo y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                if (y == null)
                {
                    return 1;
                }
                else
                {

                    return x.IetfLanguageTag.CompareTo(y.IetfLanguageTag);
                }
            }
        }

        protected void BindData()
        {
            StringBuilder allowedTokensString = new StringBuilder();
            string[] allowedTokens = Pricelist.GetListOfAllowedTokens();
            for (int i = 0; i < allowedTokens.Length; i++)
            {
                string token = allowedTokens[i];
                allowedTokensString.Append(token);
                if (i != allowedTokens.Length - 1)
                    allowedTokensString.Append(", ");
            }
            this.lblAllowedTokens.Text = allowedTokensString.ToString();

            Pricelist pricelist = ProductManager.GetPricelistByID(this.PricelistID);
            if (pricelist != null)
            {
                this.txtAdminNotes.Text = pricelist.AdminNotes;
                this.txtBody.Text = pricelist.Body;
                this.txtCacheTime.Value = pricelist.CacheTime;
                this.txtDescription.Text = pricelist.Description;
                this.txtDisplayName.Text = pricelist.DisplayName;
                this.txtFooter.Text = pricelist.Footer;
                this.txtHeader.Text = pricelist.Header;
                this.txtPricelistGuid.Text = pricelist.PricelistGuid;
                this.txtShortName.Text = pricelist.ShortName;
                CommonHelper.SelectListItem(this.ddlExportMode, pricelist.ExportModeID);
                CommonHelper.SelectListItem(this.ddlExportType, pricelist.ExportTypeID);
                CommonHelper.SelectListItem(this.ddlPriceAdjustmentType, pricelist.PriceAdjustmentTypeID);
                CommonHelper.SelectListItem(this.ddlAffiliate, pricelist.AffiliateID);
                this.chkOverrideIndivAdjustment.Checked = pricelist.OverrideIndivAdjustment;
                this.txtPriceAdjustment.Value = pricelist.PriceAdjustment;
                this.ddlFormatLocalization.SelectedValue = pricelist.FormatLocalization;

                ProductVariantCollection productVariants = new ProductVariantCollection();
                ProductCollection products = ProductManager.GetAllProducts();
                foreach (Product product in products)
                {
                    productVariants.AddRange(product.ProductVariants);
                }
                if (productVariants.Count > 0)
                {
                    gvProductVariants.DataSource = productVariants;
                    gvProductVariants.DataBind();
                }
            }
            else
            {
                ddlFormatLocalization.SelectedValue = System.Globalization.CultureInfo.CurrentCulture.IetfLanguageTag;

                ProductVariantCollection productVariants = new ProductVariantCollection();
                ProductCollection products = ProductManager.GetAllProducts();
                foreach (Product product in products)
                {
                    productVariants.AddRange(product.ProductVariants);
                }
                if (productVariants.Count > 0)
                {
                    gvProductVariants.DataSource = productVariants;
                    gvProductVariants.DataBind();
                }
            }
        }

        protected void FillDropDowns()
        {
            CommonHelper.FillDropDownWithEnum(this.ddlExportMode, typeof(PriceListExportModeEnum));

            CommonHelper.FillDropDownWithEnum(this.ddlExportType, typeof(PriceListExportTypeEnum));

            CommonHelper.FillDropDownWithEnum(this.ddlPriceAdjustmentType, typeof(PriceAdjustmentTypeEnum));

            List<CultureInfo> cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures).ToList();
            cultures.Sort(CompareCultures);
            this.ddlFormatLocalization.Items.Clear();
            foreach (CultureInfo ci in cultures)
            {
                string name = string.Format("{0}. {1}", ci.IetfLanguageTag, ci.EnglishName);
                ListItem item2 = new ListItem(name, ci.IetfLanguageTag);
                this.ddlFormatLocalization.Items.Add(item2);
            }

            this.ddlAffiliate.Items.Clear();
            ListItem ddlAffiliateItem = new ListItem(GetLocaleResourceString("Admin.PricelistInfo.Affiliate.None"), "0");
            this.ddlAffiliate.Items.Add(ddlAffiliateItem);
            AffiliateCollection affiliateCollection = AffiliateManager.GetAllAffiliates();
            foreach (Affiliate affiliate in affiliateCollection)
            {
                ListItem ddlAffiliateItem2 = new ListItem(affiliate.LastName + " (ID=" + affiliate.AffiliateID.ToString() + ")", affiliate.AffiliateID.ToString());
                this.ddlAffiliate.Items.Add(ddlAffiliateItem2);
            }
        }

        private void TogglePanels()
        {
            PriceListExportModeEnum exportMode = (PriceListExportModeEnum)int.Parse(this.ddlExportMode.SelectedItem.Value);
            pnlProductVariants.Visible = exportMode == PriceListExportModeEnum.AssignedProducts;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.FillDropDowns();
                this.BindData();
                this.TogglePanels();
            }
        }

        protected void SavePricelistChanges(int priceListID)
        {
            foreach (GridViewRow objRow in gvProductVariants.Rows)
            {
                if (objRow.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkSelected = objRow.FindControl("chkSelected") as CheckBox;
                    DropDownList ddlPriceAdjustmentType = objRow.FindControl("ddlPriceAdjustmentType") as DropDownList;
                    DecimalTextBox txtPriceAdjustment = objRow.FindControl("txtPriceAdjustment") as DecimalTextBox;
                    HiddenField hfProductVariantPricelistID = objRow.FindControl("hfProductVariantPricelistID") as HiddenField;
                    HiddenField hfProductVariantID = objRow.FindControl("hfProductVariantID") as HiddenField;

                    int productVariantPricelistID = 0;
                    int.TryParse(hfProductVariantPricelistID.Value, out productVariantPricelistID);

                    ProductVariantPricelist productVariantPricelist = ProductManager.GetProductVariantPricelistByID(productVariantPricelistID);
                    if (chkSelected.Checked)
                    {
                        int productVariantID = 0;
                        int.TryParse(hfProductVariantID.Value, out productVariantID);

                        PriceAdjustmentTypeEnum priceAdjustmentType = (PriceAdjustmentTypeEnum)Enum.ToObject(typeof(PriceAdjustmentTypeEnum), int.Parse(ddlPriceAdjustmentType.SelectedItem.Value));
                        decimal priceAdjustment = txtPriceAdjustment.Value;

                        if (productVariantPricelist != null)
                        {
                            productVariantPricelist = ProductManager.UpdateProductVariantPricelist(productVariantPricelistID, productVariantPricelist.ProductVariantID,
                               productVariantPricelist.PricelistID, priceAdjustmentType, priceAdjustment, DateTime.Now);
                        }
                        else
                        {
                            productVariantPricelist = ProductManager.InsertProductVariantPricelist(productVariantID,
                                priceListID, priceAdjustmentType, priceAdjustment, DateTime.Now);
                        }
                    }
                    else
                    {
                        if (productVariantPricelist != null)
                            ProductManager.DeleteProductVariantPricelist(productVariantPricelistID);
                    }
                }
            }
        }

        protected void gvProductVariants_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ProductVariant productVariant = (ProductVariant)e.Row.DataItem;

                DropDownList ddlPriceAdjustmentType = e.Row.FindControl("ddlPriceAdjustmentType") as DropDownList;
                CheckBox chkSelected = e.Row.FindControl("chkSelected") as CheckBox;
                DecimalTextBox txtPriceAdjustment = e.Row.FindControl("txtPriceAdjustment") as DecimalTextBox;
                HiddenField hfProductVariantPricelistID = e.Row.FindControl("hfProductVariantPricelistID") as HiddenField;

                if (chkSelected != null && ddlPriceAdjustmentType != null && txtPriceAdjustment != null && hfProductVariantPricelistID != null)
                {
                    CommonHelper.FillDropDownWithEnum(ddlPriceAdjustmentType, typeof(PriceAdjustmentTypeEnum));

                    ProductVariantPricelist productVariantPricelist = ProductManager.GetProductVariantPricelist(
                        productVariant.ProductVariantID, this.PricelistID);

                    if (productVariantPricelist != null)
                    {
                        chkSelected.Checked = true;
                        CommonHelper.SelectListItem(ddlPriceAdjustmentType, productVariantPricelist.PriceAdjustmentTypeID);
                        txtPriceAdjustment.Value = productVariantPricelist.PriceAdjustment;
                        hfProductVariantPricelistID.Value = productVariantPricelist.ProductVariantPricelistID.ToString();
                    }
                }
            }
        }

        protected void ddlExportMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            TogglePanels();
        }

        public Pricelist SaveInfo()
        {
            int affiliateID = int.Parse(ddlAffiliate.SelectedValue);

            PriceListExportModeEnum exportMode = (PriceListExportModeEnum)Enum.ToObject(typeof(PriceListExportModeEnum), int.Parse(this.ddlExportMode.SelectedItem.Value));
            PriceListExportTypeEnum exportType = (PriceListExportTypeEnum)Enum.ToObject(typeof(PriceListExportTypeEnum), int.Parse(this.ddlExportType.SelectedItem.Value));
            PriceAdjustmentTypeEnum priceAdjustmentType = (PriceAdjustmentTypeEnum)Enum.ToObject(typeof(PriceAdjustmentTypeEnum), int.Parse(this.ddlPriceAdjustmentType.SelectedItem.Value));
            decimal priceAdjustment = txtPriceAdjustment.Value;

            Pricelist pricelist = ProductManager.GetPricelistByID(this.PricelistID);
            if (pricelist != null)
            {
                pricelist = ProductManager.UpdatePricelist(pricelist.PricelistID,
                    exportMode, exportType, affiliateID, this.txtDisplayName.Text,
                    this.txtShortName.Text, this.txtPricelistGuid.Text, this.txtCacheTime.Value,
                    this.ddlFormatLocalization.SelectedValue, this.txtDescription.Text,
                    this.txtAdminNotes.Text, this.txtHeader.Text, this.txtBody.Text, this.txtFooter.Text,
                    priceAdjustmentType, this.txtPriceAdjustment.Value, this.chkOverrideIndivAdjustment.Checked,
                    pricelist.CreatedOn, DateTime.Now);

                SavePricelistChanges(pricelist.PricelistID);

            }
            else
            {
                pricelist = ProductManager.InsertPricelist(
                    exportMode, exportType, affiliateID, this.txtDisplayName.Text,
                    this.txtShortName.Text, this.txtPricelistGuid.Text, this.txtCacheTime.Value, this.ddlFormatLocalization.SelectedValue,
                    this.txtDescription.Text, this.txtAdminNotes.Text, this.txtHeader.Text, this.txtBody.Text, this.txtFooter.Text,
                    priceAdjustmentType, priceAdjustment, chkOverrideIndivAdjustment.Checked,
                    DateTime.Now, DateTime.Now);

                SavePricelistChanges(pricelist.PricelistID);
            }

            return pricelist;
        }

        public int PricelistID
        {
            get
            {
                return CommonHelper.QueryStringInt("PricelistID");
            }
        }
    }
}