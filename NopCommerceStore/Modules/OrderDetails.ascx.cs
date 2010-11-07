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
using NopSolutions.NopCommerce.BusinessLogic.Measures;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.BusinessLogic.Shipping;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Profile;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class OrderDetailsControl : BaseNopUserControl
    {
        #region Fields
        Order order = null;
        #endregion

        #region Utilities
        private void BindData()
        {
            this.lnkPrint.NavigateUrl = Page.ResolveUrl("~/PrintOrderDetails.aspx?OrderID=" + this.OrderID);
            this.lblOrderID.Text = order.OrderID.ToString();
            this.lblCreatedOn.Text = DateTimeHelper.ConvertToUserTime(order.CreatedOn).ToString("D");
            this.lblOrderStatus.Text = OrderManager.GetOrderStatusName(order.OrderStatusID);

            if (order.ShippingStatus != ShippingStatusEnum.ShippingNotRequired)
            {
                this.pnlShipping.Visible = true;
                this.lShippingFirstName.Text = Server.HtmlEncode(order.ShippingFirstName);
                this.lShippingLastName.Text = Server.HtmlEncode(order.ShippingLastName);
                this.lShippingPhoneNumber.Text = Server.HtmlEncode(order.ShippingPhoneNumber);
				this.lShippingEmail.Text = Server.HtmlEncode(!string.IsNullOrEmpty(NopContext.Current.User.BillingAddress.Email) ? NopContext.Current.User.BillingAddress.Email : NopContext.Current.User.Email);
                this.lShippingFaxNumber.Text = Server.HtmlEncode(order.ShippingFaxNumber);
                if (!String.IsNullOrEmpty(order.ShippingCompany))
                    this.lShippingCompany.Text = Server.HtmlEncode(order.ShippingCompany);
                else
                    pnlShippingCompany.Visible = false;
                this.lShippingAddress1.Text = Server.HtmlEncode(order.ShippingAddress1);
                if (!String.IsNullOrEmpty(order.ShippingAddress2))
                    this.lShippingAddress2.Text = Server.HtmlEncode(order.ShippingAddress2);
                else
                    pnlShippingAddress2.Visible = false;
                this.lShippingCity.Text = Server.HtmlEncode(order.ShippingCity);
                this.lShippingStateProvince.Text = Server.HtmlEncode(order.ShippingStateProvince);
                this.lShippingZipPostalCode.Text = Server.HtmlEncode(order.ShippingZipPostalCode);
                if (!String.IsNullOrEmpty(order.ShippingCountry))
                    this.lShippingCountry.Text = Server.HtmlEncode(order.ShippingCountry);
                else
                    pnlShippingCountry.Visible = false;

                this.lblShippingMethod.Text = Server.HtmlEncode(order.ShippingMethod);
                this.lblOrderWeight.Text = string.Format("{0} [{1}]", order.OrderWeight, MeasureManager.BaseWeightIn.Name);

                //TODO use order.ShippingStatus
                if (order.ShippedDate.HasValue)
                    this.lblShippedDate.Text = DateTimeHelper.ConvertToUserTime(order.ShippedDate.Value).ToString();
                else
                    this.lblShippedDate.Text = GetLocaleResourceString("Order.NotYetShipped");
            }
            else
                this.pnlShipping.Visible = false;

            this.lBillingFirstName.Text = Server.HtmlEncode(order.BillingFirstName);
            this.lBillingLastName.Text = Server.HtmlEncode(order.BillingLastName);
            this.lBillingPhoneNumber.Text = Server.HtmlEncode(order.BillingPhoneNumber);
			this.lBillingEmail.Text = Server.HtmlEncode(!string.IsNullOrEmpty(NopContext.Current.User.BillingAddress.Email) ? NopContext.Current.User.BillingAddress.Email : NopContext.Current.User.Email);
            this.lBillingFaxNumber.Text = Server.HtmlEncode(order.BillingFaxNumber);
            if (!String.IsNullOrEmpty(order.BillingCompany))
                this.lBillingCompany.Text = Server.HtmlEncode(order.BillingCompany);
            else
                pnlBillingCompany.Visible = false;
            this.lBillingAddress1.Text = Server.HtmlEncode(order.BillingAddress1);
            if (!String.IsNullOrEmpty(order.BillingAddress2))
                this.lBillingAddress2.Text = Server.HtmlEncode(order.BillingAddress2);
            else
                pnlBillingAddress2.Visible = false;
            this.lBillingCity.Text = Server.HtmlEncode(order.BillingCity);
            this.lBillingStateProvince.Text = Server.HtmlEncode(order.BillingStateProvince);
            this.lBillingZipPostalCode.Text = Server.HtmlEncode(order.BillingZipPostalCode);
            if (!String.IsNullOrEmpty(order.BillingCountry))
                this.lBillingCountry.Text = Server.HtmlEncode(order.BillingCountry);
            else
                pnlBillingCountry.Visible = false;


            PaymentMethod paymentMethod = PaymentMethodManager.GetPaymentMethodByID(order.PaymentMethodID);
            if (paymentMethod != null)
                this.lPaymentMethod.Text = paymentMethod.VisibleName;
            else
                this.lPaymentMethod.Text = order.PaymentMethodName;

            switch (order.CustomerTaxDisplayType)
            {
                case TaxDisplayTypeEnum.ExcludingTax:
                    {
                        this.lblOrderSubtotal.Text = PriceHelper.FormatPrice(order.OrderSubtotalExclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, NopContext.Current.WorkingLanguage, false);
                        this.lblOrderShipping.Text = PriceHelper.FormatShippingPrice(order.OrderShippingExclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, NopContext.Current.WorkingLanguage, false);
                        this.lblPaymentMethodAdditionalFee.Text = PriceHelper.FormatPaymentMethodAdditionalFee(order.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, NopContext.Current.WorkingLanguage, false);
                    }
                    break;
                case TaxDisplayTypeEnum.IncludingTax:
                    {
                        this.lblOrderSubtotal.Text = PriceHelper.FormatPrice(order.OrderSubtotalInclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, NopContext.Current.WorkingLanguage, true);
                        this.lblOrderShipping.Text = PriceHelper.FormatShippingPrice(order.OrderShippingInclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, NopContext.Current.WorkingLanguage, true);
                        this.lblPaymentMethodAdditionalFee.Text = PriceHelper.FormatPaymentMethodAdditionalFee(order.PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, NopContext.Current.WorkingLanguage, true);
                    }
                    break;
            }

            bool displayPaymentMethodFee = true;
            if (order.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency == decimal.Zero)
            {
                displayPaymentMethodFee = false;
            }
            phPaymentMethodAdditionalFee.Visible = displayPaymentMethodFee;
            
            bool displayTax = true;
            if (TaxManager.HideTaxInOrderSummary && order.CustomerTaxDisplayType == TaxDisplayTypeEnum.IncludingTax)
            {
                displayTax = false;
            }
            else
            {
                if (order.OrderTax == 0 && TaxManager.HideZeroTax)
                {
                    displayTax = false;
                }
                else
                {
                    string taxStr = PriceHelper.FormatPrice(order.OrderTaxInCustomerCurrency, true, order.CustomerCurrencyCode, false);
                    this.lblOrderTax.Text = taxStr;
                }
            }
            phTaxTotal.Visible = displayTax;

            string orderTotalStr = PriceHelper.FormatPrice(order.OrderTotalInCustomerCurrency, true, order.CustomerCurrencyCode, false);
            this.lblOrderTotal.Text = orderTotalStr;
            this.lblOrderTotal2.Text = orderTotalStr;

            OrderProductVariantCollection orderProductVariants = order.OrderProductVariants;
            bool hasDownloadableItems = false;
            foreach (OrderProductVariant orderProductVariant in orderProductVariants)
            {
                ProductVariant productVariant = orderProductVariant.ProductVariant;
                if (productVariant != null)
                {
                    if (productVariant.IsDownload && OrderManager.AreDownloadsAllowed(order))
                    {
                        hasDownloadableItems = true;
                        break;
                    }
                }
            }
            gvOrderProductVariants.Columns[2].Visible = hasDownloadableItems;
            gvOrderProductVariants.DataSource = orderProductVariants;
            gvOrderProductVariants.DataBind();
        }
        #endregion

        #region Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            if (NopContext.Current.User == null)
            {
                string loginURL = CommonHelper.GetLoginPageURL(true);
                Response.Redirect(loginURL);
            }
            order = OrderManager.GetOrderByID(this.OrderID);
            if (order == null || order.Deleted || NopContext.Current.User.CustomerID != order.CustomerID)
            {
                string loginURL = CommonHelper.GetLoginPageURL(true);
                Response.Redirect(loginURL);
            }

            if (!Page.IsPostBack)
            {
                this.BindData();
            }
        }
        #endregion

        #region Methods
        public string GetProductVariantName(int ProductVariantID)
        {
            ProductVariant productVariant = ProductManager.GetProductVariantByID(ProductVariantID);
            if (productVariant != null)
                return productVariant.FullProductName;
            return "Not available. ID=" + ProductVariantID.ToString();
        }

        public string GetProductURL(int ProductVariantID)
        {
            ProductVariant productVariant = ProductManager.GetProductVariantByID(ProductVariantID);
            if (productVariant != null)
                return SEOHelper.GetProductURL(productVariant.ProductID);
            return string.Empty;
        }

        public string GetDownloadURL(OrderProductVariant orderProductVariant)
        {
            string result = string.Empty;
            ProductVariant productVariant = orderProductVariant.ProductVariant;
            if (productVariant != null)
            {
                if (productVariant.IsDownload && OrderManager.AreDownloadsAllowed(order))
                {
                    //if (productVariant.Download != null)
                    //{
                        result = string.Format("<a class=\"link\" href=\"{0}\" >{1}</a>", DownloadManager.GetDownloadUrl(orderProductVariant), GetLocaleResourceString("Order.Download"));
                    //}
                    //else
                    //    result = "Not available anymore";
                }
            }
            else
                result = "Not available. Product variant ID=" + orderProductVariant.ProductVariantID.ToString();
            return result;
        }

        public string GetProductVariantUnitPrice(OrderProductVariant orderProductVariant)
        {
            string result = string.Empty;
            switch (order.CustomerTaxDisplayType)
            {
                case TaxDisplayTypeEnum.ExcludingTax:
                    result = PriceHelper.FormatPrice(orderProductVariant.UnitPriceExclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, NopContext.Current.WorkingLanguage, false);
                    break;
                case TaxDisplayTypeEnum.IncludingTax:
                    result = PriceHelper.FormatPrice(orderProductVariant.UnitPriceInclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, NopContext.Current.WorkingLanguage, true);
                    break;
            }

            return result;
        }

        public string GetProductVariantSubTotal(OrderProductVariant orderProductVariant)
        {
            string result = string.Empty;
            switch (order.CustomerTaxDisplayType)
            {
                case TaxDisplayTypeEnum.ExcludingTax:
                    result = PriceHelper.FormatPrice(orderProductVariant.PriceExclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, NopContext.Current.WorkingLanguage, false);
                    break;
                case TaxDisplayTypeEnum.IncludingTax:
                    result = PriceHelper.FormatPrice(orderProductVariant.PriceInclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, NopContext.Current.WorkingLanguage, true);
                    break;
            }
            
           return result;
        }

        #endregion

        #region Properties
        public int OrderID
        {
            get
            {
                return CommonHelper.QueryStringInt("OrderID");
            }
        }

        public bool HidePrintButton
        {
            get
            {
                if (ViewState["HidePrintButton"] == null)
                    return false;
                else
                    return (bool)ViewState["HidePrintButton"];
            }
            set
            { 
                ViewState["HidePrintButton"] = value; 
            }
        }
        #endregion
    }
}