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
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Localization;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Products.Attributes;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class ProductAttributesControl : BaseNopUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CreateAttributeControls();
        }

        protected void CreateAttributeControls()
        {
            ProductVariant productVariant = ProductManager.GetProductVariantByID(this.ProductVariantID);
            if (productVariant != null)
            {
                ProductVariantAttributeCollection productVariantAttributes = productVariant.ProductVariantAttributes;
                if (productVariantAttributes.Count > 0)
                {
                    this.Visible = true;
                    foreach (ProductVariantAttribute attribute in productVariantAttributes)
                    {
                        Panel divAttribute = new Panel();
                        Label attributeTitle = new Label();
                        if (attribute.IsRequired)
                            attributeTitle.Text = "<span>*</span> ";

                        //text prompt
                        string textPrompt = string.Empty;
                        if (!string.IsNullOrEmpty(attribute.TextPrompt))
                            textPrompt = attribute.TextPrompt;
                        else
                            textPrompt += attribute.ProductAttribute.Name;

                        attributeTitle.Text += Server.HtmlEncode(textPrompt);
                        attributeTitle.Style.Add("font-weight", "bold");

                        //description
                        if (!string.IsNullOrEmpty(attribute.ProductAttribute.Description))
                            attributeTitle.Text += string.Format("<div>{0}</div>",
                                                                 Server.HtmlEncode(
                                                                     attribute.ProductAttribute.Description));

                        bool addBreak = true;
                        switch (attribute.AttributeControlType)
                        {
                            case AttributeControlTypeEnum.TextBox:
                                {
                                    addBreak = false;
                                }
                                break;
                            default:
                                break;
                        }
                        if (addBreak)
                        {
                            attributeTitle.Text += "<br />";
                        }
                        else
                        {
                            attributeTitle.Text += "&nbsp;&nbsp;&nbsp;";
                        }
                        divAttribute.Controls.Add(attributeTitle);

                        switch (attribute.AttributeControlType)
                        {
                            case AttributeControlTypeEnum.DropdownList:
                                {
                                    DropDownList ddlAttributes = new DropDownList();
                                    ddlAttributes.ID = attribute.ProductAttribute.Name;
                                    if (!attribute.IsRequired)
                                    {
                                        ddlAttributes.Items.Add(new ListItem("---", "0"));
                                    }
                                    ProductVariantAttributeValueCollection pvaValues = attribute.ProductVariantAttributeValues;
                                    foreach (ProductVariantAttributeValue pvaValue in pvaValues)
                                    {
                                        string pvaValueName = pvaValue.Name;
                                        decimal priceAdjustmentBase = TaxManager.GetPrice(productVariant, pvaValue.PriceAdjustment);
                                        decimal priceAdjustment = CurrencyManager.ConvertCurrency(priceAdjustmentBase, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingCurrency);
                                        if (priceAdjustmentBase > decimal.Zero)
                                            pvaValueName += string.Format(" [+{0}]", PriceHelper.FormatPrice(priceAdjustment, false, false));
                                        ListItem pvaValueItem = new ListItem(pvaValueName, pvaValue.ProductVariantAttributeValueID.ToString());
                                        pvaValueItem.Selected = pvaValue.IsPreSelected;
                                        ddlAttributes.Items.Add(pvaValueItem);
                                    }
                                    divAttribute.Controls.Add(ddlAttributes);
                                }
                                break;
                            case AttributeControlTypeEnum.RadioList:
                                {
                                    RadioButtonList rblAttributes = new RadioButtonList();
                                    rblAttributes.ID = attribute.ProductAttribute.Name;
                                    ProductVariantAttributeValueCollection pvaValues = attribute.ProductVariantAttributeValues;
                                    foreach (ProductVariantAttributeValue pvaValue in pvaValues)
                                    {
                                        string pvaValueName = pvaValue.Name;
                                        decimal priceAdjustmentBase = TaxManager.GetPrice(productVariant, pvaValue.PriceAdjustment);
                                        decimal priceAdjustment = CurrencyManager.ConvertCurrency(priceAdjustmentBase, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingCurrency);
                                        if (priceAdjustmentBase > decimal.Zero)
                                            pvaValueName += string.Format(" [+{0}]", PriceHelper.FormatPrice(priceAdjustment, false, false));
                                        ListItem pvaValueItem = new ListItem(Server.HtmlEncode(pvaValueName), pvaValue.ProductVariantAttributeValueID.ToString());
                                        pvaValueItem.Selected = pvaValue.IsPreSelected;
                                        rblAttributes.Items.Add(pvaValueItem);
                                    }
                                    divAttribute.Controls.Add(rblAttributes);
                                }
                                break;
                            case AttributeControlTypeEnum.Checkboxes:
                                {
                                    CheckBoxList cblAttributes = new CheckBoxList();
                                    cblAttributes.ID = attribute.ProductAttribute.Name;
                                    ProductVariantAttributeValueCollection pvaValues = attribute.ProductVariantAttributeValues;
                                    foreach (ProductVariantAttributeValue pvaValue in pvaValues)
                                    {
                                        string pvaValueName = pvaValue.Name;
                                        decimal priceAdjustmentBase = TaxManager.GetPrice(productVariant, pvaValue.PriceAdjustment);
                                        decimal priceAdjustment = CurrencyManager.ConvertCurrency(priceAdjustmentBase, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingCurrency);
                                        if (priceAdjustmentBase > decimal.Zero)
                                            pvaValueName += string.Format(" [+{0}]", PriceHelper.FormatPrice(priceAdjustment, false, false));
                                        ListItem pvaValueItem = new ListItem(Server.HtmlEncode(pvaValueName), pvaValue.ProductVariantAttributeValueID.ToString());
                                        pvaValueItem.Selected = pvaValue.IsPreSelected;
                                        cblAttributes.Items.Add(pvaValueItem);
                                    }
                                    divAttribute.Controls.Add(cblAttributes);
                                }
                                break;
                            case AttributeControlTypeEnum.TextBox:
                                {
                                    TextBox txtAttribute = new TextBox();
                                    txtAttribute.ID = attribute.ProductAttribute.Name;
                                    divAttribute.Controls.Add(txtAttribute);
                                }
                                break;
                            default:
                                break;
                        }
                        phAttributes.Controls.Add(divAttribute);
                    }
                }
                else
                {
                    this.Visible = false;
                }
            }
            else
            {
                this.Visible = false;
            }
        }

        public string SelectedAttributes
        {
            get
            {
                string selectedAttributes = string.Empty;
                ProductVariantAttributeCollection productVariantAttributes =
                    ProductAttributeManager.GetProductVariantAttributesByProductVariantID(this.ProductVariantID);
                foreach (ProductVariantAttribute attribute in productVariantAttributes)
                {
                    switch (attribute.AttributeControlType)
                    {
                        case AttributeControlTypeEnum.DropdownList:
                            {
                                DropDownList ddlAttributes =
                                    phAttributes.FindControl(attribute.ProductAttribute.Name) as DropDownList;
                                if (ddlAttributes != null)
                                {
                                    int selectedAttributeID = 0;
                                    if (!String.IsNullOrEmpty(ddlAttributes.SelectedValue))
                                    {
                                        selectedAttributeID = int.Parse(ddlAttributes.SelectedValue);
                                    }
                                    if (selectedAttributeID > 0)
                                    {
                                        selectedAttributes = ProductAttributeHelper.AddAttribute(selectedAttributes,
                                                                                                 attribute,
                                                                                                 selectedAttributeID.
                                                                                                     ToString());
                                    }
                                }
                            }
                            break;
                        case AttributeControlTypeEnum.RadioList:
                            {
                                RadioButtonList rblAttributes =
                                    phAttributes.FindControl(attribute.ProductAttribute.Name) as RadioButtonList;
                                if (rblAttributes != null)
                                {
                                    int selectedAttributeID = 0;
                                    if (!String.IsNullOrEmpty(rblAttributes.SelectedValue))
                                    {
                                        selectedAttributeID = int.Parse(rblAttributes.SelectedValue);
                                    }
                                    if (selectedAttributeID > 0)
                                    {
                                        selectedAttributes = ProductAttributeHelper.AddAttribute(selectedAttributes,
                                                                                                 attribute,
                                                                                                 selectedAttributeID.
                                                                                                     ToString());
                                    }
                                }
                            }
                            break;
                        case AttributeControlTypeEnum.Checkboxes:
                            {
                                CheckBoxList cblAttributes =
                                    phAttributes.FindControl(attribute.ProductAttribute.Name) as CheckBoxList;
                                if (cblAttributes != null)
                                {
                                    foreach (ListItem item in cblAttributes.Items)
                                    {
                                        if (item.Selected)
                                        {
                                            int selectedAttributeID = 0;
                                            if (!String.IsNullOrEmpty(item.Value))
                                            {
                                                selectedAttributeID = int.Parse(item.Value);
                                            }
                                            if (selectedAttributeID > 0)
                                            {
                                                selectedAttributes =
                                                    ProductAttributeHelper.AddAttribute(selectedAttributes, attribute,
                                                                                        selectedAttributeID.ToString());
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case AttributeControlTypeEnum.TextBox:
                            {
                                TextBox txtAttribute =
                                    phAttributes.FindControl(attribute.ProductAttribute.Name) as TextBox;
                                if (txtAttribute != null)
                                {
                                    string enteredText = txtAttribute.Text.Trim();
                                    if (!String.IsNullOrEmpty(enteredText))
                                    {
                                        selectedAttributes = ProductAttributeHelper.AddAttribute(selectedAttributes,
                                                                                                 attribute, enteredText);
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
                return selectedAttributes;
            }
        }

        public int ProductVariantID
        {
            get
            {
                if (ViewState["ProductVariantID"] == null)
                    return 0;
                else
                    return (int)ViewState["ProductVariantID"];
            }
            set { ViewState["ProductVariantID"] = value; }
        }

    }
}