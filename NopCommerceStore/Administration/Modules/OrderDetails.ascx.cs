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
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Localization;
using NopSolutions.NopCommerce.BusinessLogic.Measures;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.BusinessLogic.Promo.Affiliates;
using NopSolutions.NopCommerce.BusinessLogic.Security;
using NopSolutions.NopCommerce.BusinessLogic.Shipping;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.BusinessLogic.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Utils.Html;
using NopSolutions.NopCommerce.Common.Utils;


namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class OrderDetailsControl : BaseNopAdministrationUserControl
    {
        private void BindData()
        {
            Order order = OrderManager.GetOrderByID(this.OrderID);
            if (order != null)
            {
                this.lblOrderStatus.Text = OrderManager.GetOrderStatusName(order.OrderStatusID);
                this.CancelOrderButton.Visible = OrderManager.CanCancelOrder(order);
                this.lblOrderID.Text = order.OrderID.ToString();
                this.lblOrderGUID.Text = order.OrderGUID.ToString();

                Customer customer = order.Customer;
                if (customer != null)
                {
                    if (customer.IsGuest)
                    {
                        this.lblCustomer.Text = string.Format("<a href=\"CustomerDetails.aspx?CustomerID={0}\">{1}</a>", customer.CustomerID, GetLocaleResourceString("Admin.OrderDetails.Guest"));
                    }
                    else
                    {
                        this.lblCustomer.Text = string.Format("<a href=\"CustomerDetails.aspx?CustomerID={0}\">{1}</a>", customer.CustomerID, Server.HtmlEncode(customer.Email));
                    }
                }
                else
                {
                    this.lblCustomer.Text = "Customer was deleted";
                }

                Affiliate affiliate = order.Affiliate;
                if (affiliate != null)
                {
                    this.lblAffiliate.Text = string.Format("<a href=\"AffiliateDetails.aspx?AffiliateID={0}\">{1}</a>", affiliate.AffiliateID, Server.HtmlEncode(affiliate.FullName));
                    this.divAffiliate.Visible = true;
                }
                else
                    this.divAffiliate.Visible = false;

                this.lblCreatedOn.Text = DateTimeHelper.ConvertToUserTime(order.CreatedOn).ToString();

                BindOrderTotals(order);
                BindPaymentInfo(order);
                BindBillingInfo(order);
                BindShippingInfo(order);
                BindProductInfo(order);
                BindOrderNotes(order);
            }
            else
                Response.Redirect("Orders.aspx");
        }

        private void BindPaymentInfo(Order order)
        {
            if (order == null)
                return;

            string cardTypeDecrypted = SecurityHelper.Decrypt(order.CardType);
            if (!String.IsNullOrEmpty(cardTypeDecrypted))
            {
                this.lblCardType.Text = Server.HtmlEncode(cardTypeDecrypted);
            }
            else
            {
                pnlCartType.Visible = false;
            }

            string cardNameDecrypted = SecurityHelper.Decrypt(order.CardName);
            if (!String.IsNullOrEmpty(cardNameDecrypted))
            {
                this.lblCardName.Text = Server.HtmlEncode(cardNameDecrypted);
            }
            else
            {
                pnlCardName.Visible = false;
            }

            if (order.AllowStoringCreditCardNumber)
            {
                string cardNumberDecrypted = SecurityHelper.Decrypt(order.CardNumber);
                if (!String.IsNullOrEmpty(cardNumberDecrypted))
                {
                    this.lblCardNumber.Text = Server.HtmlEncode(cardNumberDecrypted);
                }
                else
                {
                    pnlCardNumber.Visible = false;
                }
            }
            else
            {
                string maskedCreditCardNumberDecrypted = SecurityHelper.Decrypt(order.MaskedCreditCardNumber);
                if (!String.IsNullOrEmpty(maskedCreditCardNumberDecrypted))
                {
                    this.lblCardNumber.Text = Server.HtmlEncode(maskedCreditCardNumberDecrypted);
                }
                else
                {
                    pnlCardNumber.Visible = false;
                }
            }

            if (order.AllowStoringCreditCardNumber)
            {
                string cardCVV2Decrypted = SecurityHelper.Decrypt(order.CardCVV2);
                this.lblCardCVV2.Text = Server.HtmlEncode(cardCVV2Decrypted);
            }
            else
            {
                pnlCardCVV2.Visible = false;
            }

            string cardExpirationMonthDecrypted = SecurityHelper.Decrypt(order.CardExpirationMonth);
            if (!String.IsNullOrEmpty(cardExpirationMonthDecrypted) && cardExpirationMonthDecrypted != "0")
            {
                this.lblCardExpirationMonth.Text = cardExpirationMonthDecrypted;
            }
            else
            {
                pnlCardExpiryMonth.Visible = false;
            }

            string cardExpirationYearDecrypted = SecurityHelper.Decrypt(order.CardExpirationYear);
            if (!String.IsNullOrEmpty(cardExpirationYearDecrypted)&& cardExpirationYearDecrypted != "0")
            {
                this.lblCardExpirationYear.Text = cardExpirationYearDecrypted;
            }
            else
            {
                pnlCardExpiryYear.Visible = false;
            }

            this.lblPONumber.Text = Server.HtmlEncode(order.PurchaseOrderNumber);
            this.lblPaymentMethodName.Text = Server.HtmlEncode(order.PaymentMethodName);
            this.lblPaymentStatus.Text = PaymentStatusManager.GetPaymentStatusName(order.PaymentStatusID);
            this.btnCapture.Visible = OrderManager.CanCapture(order);
            this.btnMarkAsPaid.Visible = OrderManager.CanMarkOrderAsPaid(order);
        }

        protected void BindBillingInfo(Order order)
        {
            if (order == null)
                return;

            this.lBillingFirstName.Text = Server.HtmlEncode(order.BillingFirstName);
            this.lBillingLastName.Text = Server.HtmlEncode(order.BillingLastName);
            this.lBillingPhoneNumber.Text = Server.HtmlEncode(order.BillingPhoneNumber);
            this.lBillingEmail.Text = Server.HtmlEncode(order.BillingEmail);
            this.lBillingFaxNumber.Text = Server.HtmlEncode(order.BillingFaxNumber);
            if (!String.IsNullOrEmpty(order.BillingCompany))
                this.lBillingCompany.Text = Server.HtmlEncode(order.BillingCompany);
            else
                this.pnlBillingCompany.Visible = false;
            this.lBillingAddress1.Text = Server.HtmlEncode(order.BillingAddress1);
            if (!String.IsNullOrEmpty(order.BillingAddress2))
                this.lBillingAddress2.Text = Server.HtmlEncode(order.BillingAddress2);
            else
                this.pnlBillingAddress2.Visible = false;
            this.lBillingCity.Text = Server.HtmlEncode(order.BillingCity);
            if (!String.IsNullOrEmpty(order.BillingCountry))
                this.lBillingCountry.Text = Server.HtmlEncode(order.BillingCountry);
            else
                this.pnlBillingCountry.Visible = false;
            if (!String.IsNullOrEmpty(order.BillingStateProvince))
                this.lBillingStateProvince.Text = Server.HtmlEncode(order.BillingStateProvince);
            this.lBillingZipPostalCode.Text = Server.HtmlEncode(order.BillingZipPostalCode);
        }

        protected void BindShippingInfo(Order order)
        {
            if (order == null)
                return;

            if (order.ShippingStatus != ShippingStatusEnum.ShippingNotRequired)
            {
                this.lShippingFirstName.Text = Server.HtmlEncode(order.ShippingFirstName);
                this.lShippingLastName.Text = Server.HtmlEncode(order.ShippingLastName);
                this.lShippingPhoneNumber.Text = Server.HtmlEncode(order.ShippingPhoneNumber);
                this.lShippingEmail.Text = Server.HtmlEncode(order.ShippingEmail);
                this.lShippingFaxNumber.Text = Server.HtmlEncode(order.ShippingFaxNumber);
                if (!String.IsNullOrEmpty(order.ShippingCompany))
                    this.lShippingCompany.Text = Server.HtmlEncode(order.ShippingCompany);
                else
                    this.pnlShippingCompany.Visible = false;
                this.lShippingAddress1.Text = Server.HtmlEncode(order.ShippingAddress1);
                if (!String.IsNullOrEmpty(order.ShippingAddress2))
                    this.lShippingAddress2.Text = Server.HtmlEncode(order.ShippingAddress2);
                else
                    this.pnlShippingAddress2.Visible = false;
                this.lShippingCity.Text = Server.HtmlEncode(order.ShippingCity);
                if (!String.IsNullOrEmpty(order.ShippingCountry))
                    this.lShippingCountry.Text = Server.HtmlEncode(order.ShippingCountry);
                else
                    this.pnlShippingCountry.Visible = false;
                if (!String.IsNullOrEmpty(order.ShippingStateProvince))
                    this.lShippingStateProvince.Text = Server.HtmlEncode(order.ShippingStateProvince);
                this.lShippingZipPostalCode.Text = Server.HtmlEncode(order.ShippingZipPostalCode);

                this.lblShippingMethod.Text = Server.HtmlEncode(order.ShippingMethod);

                this.btnSetAsShipped.Visible = OrderManager.CanShip(order);

                if (order.ShippedDate.HasValue)
                {
                    this.lblShippedDate.Text = DateTimeHelper.ConvertToUserTime(order.ShippedDate.Value).ToString();
                }

                this.lblOrderWeight.Text = string.Format("{0} [{1}]", order.OrderWeight, MeasureManager.BaseWeightIn.Name);
                
                this.divShippingNotRequired.Visible = false;
                this.divShippingAddress.Visible = true;
                this.divShippingWeight.Visible = true;
                this.divShippingMethod.Visible = true;
                this.divShippedDate.Visible = true;
            }
            else
            {
                this.divShippingNotRequired.Visible = true;
                this.divShippingAddress.Visible = false;
                this.divShippingWeight.Visible = false;
                this.divShippingMethod.Visible = false;
                this.divShippedDate.Visible = false;
            }
        }

        protected void BindProductInfo(Order order)
        {
            if (order == null)
                return;

            OrderProductVariantCollection orderProductVariants = order.OrderProductVariants;
            this.gvOrderProductVariants.DataSource = orderProductVariants;
            this.gvOrderProductVariants.DataBind();

        }

        private void BindOrderTotals(Order order)
        {
            if (order == null)
                return;

            //subtotal
            string orderSubtotalInclTaxStr = PriceHelper.FormatPrice(order.OrderSubtotalInclTax, true, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingLanguage, true);
            string orderSubtotalExclTaxStr = PriceHelper.FormatPrice(order.OrderSubtotalExclTax, true, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingLanguage, false);
            if (TaxManager.AllowCustomersToSelectTaxDisplayType)
            {
                this.lblOrderSubtotalInclTax.Text = orderSubtotalInclTaxStr;
                this.lblOrderSubtotalExclTax.Text = orderSubtotalExclTaxStr;
                this.pnlOrderSubtotalInclTax.Visible = true;
                this.pnlOrderSubtotalExclTax.Visible = true;
            }
            else
            {
                switch (TaxManager.TaxDisplayType)
                {
                    case TaxDisplayTypeEnum.ExcludingTax:
                        {
                            this.lblOrderSubtotalExclTax.Text = orderSubtotalExclTaxStr;
                            this.pnlOrderSubtotalInclTax.Visible = false;
                            this.pnlOrderSubtotalExclTax.Visible = true;
                        }
                        break;
                    case TaxDisplayTypeEnum.IncludingTax:
                        {
                            this.lblOrderSubtotalInclTax.Text = orderSubtotalInclTaxStr;
                            this.pnlOrderSubtotalInclTax.Visible = true;
                            this.pnlOrderSubtotalExclTax.Visible = false;
                        }
                        break;
                    default:
                        {
                            this.pnlOrderSubtotalInclTax.Visible = false;
                            this.pnlOrderSubtotalExclTax.Visible = false;
                        }
                        break;
                }
            }

            //shipping
            string orderShippingInclTaxStr = PriceHelper.FormatShippingPrice(order.OrderShippingInclTax, true, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingLanguage, true);
            string orderShippingExclTaxStr = PriceHelper.FormatShippingPrice(order.OrderShippingExclTax, true, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingLanguage, false);
            if (TaxManager.ShippingIsTaxable)
            {
                if (TaxManager.AllowCustomersToSelectTaxDisplayType)
                {
                    this.lblOrderShippingInclTax.Text = orderShippingInclTaxStr;
                    this.lblOrderShippingExclTax.Text = orderShippingExclTaxStr;
                    this.pnlOrderShippingInclTax.Visible = true;
                    this.pnlOrderShippingExclTax.Visible = true;
                }
                else
                {
                    switch (TaxManager.TaxDisplayType)
                    {
                        case TaxDisplayTypeEnum.ExcludingTax:
                            {
                                this.lblOrderShippingExclTax.Text = orderShippingExclTaxStr;
                                this.pnlOrderShippingInclTax.Visible = false;
                                this.pnlOrderShippingExclTax.Visible = true;
                            }
                            break;
                        case TaxDisplayTypeEnum.IncludingTax:
                            {
                                this.lblOrderShippingInclTax.Text = orderShippingInclTaxStr;
                                this.pnlOrderShippingInclTax.Visible = true;
                                this.pnlOrderShippingExclTax.Visible = false;
                            }
                            break;
                        default:
                            {
                                this.pnlOrderShippingInclTax.Visible = false;
                                this.pnlOrderShippingExclTax.Visible = false;
                            }
                            break;
                    }
                }
            }
            else
            {
                switch (TaxManager.TaxDisplayType)
                {
                    case TaxDisplayTypeEnum.ExcludingTax:
                        {
                            this.lblOrderShippingExclTax.Text = orderShippingExclTaxStr;
                            this.pnlOrderShippingInclTax.Visible = false;
                            this.pnlOrderShippingExclTax.Visible = true;
                        }
                        break;
                    case TaxDisplayTypeEnum.IncludingTax:
                        {
                            this.lblOrderShippingInclTax.Text = orderShippingInclTaxStr;
                            this.pnlOrderShippingInclTax.Visible = true;
                            this.pnlOrderShippingExclTax.Visible = false;
                        }
                        break;
                    default:
                        {
                            this.pnlOrderShippingInclTax.Visible = false;
                            this.pnlOrderShippingExclTax.Visible = false;
                        }
                        break;
                }
            }

            //payment method additional fee
            string paymentMethodAdditionalFeeInclTaxStr = PriceHelper.FormatPaymentMethodAdditionalFee(order.PaymentMethodAdditionalFeeInclTax, true, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingLanguage, true);
            string paymentMethodAdditionalFeeExclTaxStr = PriceHelper.FormatPaymentMethodAdditionalFee(order.PaymentMethodAdditionalFeeExclTax, true, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingLanguage, false);
            if (order.PaymentMethodAdditionalFeeInclTax > decimal.Zero)
            {
                if (TaxManager.PaymentMethodAdditionalFeeIsTaxable)
                {
                    if (TaxManager.AllowCustomersToSelectTaxDisplayType)
                    {
                        this.lblPaymentMethodAdditionalFeeInclTax.Text = paymentMethodAdditionalFeeInclTaxStr;
                        this.lblPaymentMethodAdditionalFeeExclTax.Text = paymentMethodAdditionalFeeExclTaxStr;
                        this.pnlPaymentMethodAdditionalFeeInclTax.Visible = true;
                        this.pnlPaymentMethodAdditionalFeeExclTax.Visible = true;
                    }
                    else
                    {
                        switch (TaxManager.TaxDisplayType)
                        {
                            case TaxDisplayTypeEnum.ExcludingTax:
                                {
                                    this.lblPaymentMethodAdditionalFeeExclTax.Text = paymentMethodAdditionalFeeExclTaxStr;
                                    this.pnlPaymentMethodAdditionalFeeInclTax.Visible = false;
                                    this.pnlPaymentMethodAdditionalFeeExclTax.Visible = true;
                                }
                                break;
                            case TaxDisplayTypeEnum.IncludingTax:
                                {
                                    this.lblPaymentMethodAdditionalFeeInclTax.Text = paymentMethodAdditionalFeeInclTaxStr;
                                    this.pnlPaymentMethodAdditionalFeeInclTax.Visible = true;
                                    this.pnlPaymentMethodAdditionalFeeExclTax.Visible = false;
                                }
                                break;
                            default:
                                {
                                    this.pnlPaymentMethodAdditionalFeeInclTax.Visible = false;
                                    this.pnlPaymentMethodAdditionalFeeExclTax.Visible = false;
                                }
                                break;
                        }
                    }
                }
                else
                {
                    switch (TaxManager.TaxDisplayType)
                    {
                        case TaxDisplayTypeEnum.ExcludingTax:
                            {
                                this.lblPaymentMethodAdditionalFeeExclTax.Text = paymentMethodAdditionalFeeExclTaxStr;
                                this.pnlPaymentMethodAdditionalFeeInclTax.Visible = false;
                                this.pnlPaymentMethodAdditionalFeeExclTax.Visible = true;
                            }
                            break;
                        case TaxDisplayTypeEnum.IncludingTax:
                            {
                                this.lblPaymentMethodAdditionalFeeInclTax.Text = paymentMethodAdditionalFeeInclTaxStr;
                                this.pnlPaymentMethodAdditionalFeeInclTax.Visible = true;
                                this.pnlPaymentMethodAdditionalFeeExclTax.Visible = false;
                            }
                            break;
                        default:
                            {
                                this.pnlPaymentMethodAdditionalFeeInclTax.Visible = false;
                                this.pnlPaymentMethodAdditionalFeeExclTax.Visible = false;
                            }
                            break;
                    }
                }
            }
            else
            {
                this.pnlPaymentMethodAdditionalFeeInclTax.Visible = false;
                this.pnlPaymentMethodAdditionalFeeExclTax.Visible = false;
            }

            this.lblOrderDiscount.Text = PriceHelper.FormatPrice(order.OrderDiscount, true, false);
            this.lblOrderTax.Text = PriceHelper.FormatPrice(order.OrderTax, true, false);
            this.lblOrderTotal.Text = PriceHelper.FormatPrice(order.OrderTotal, true, false);
            //this.lblOrderTotalInCustomerCurrencyCode.Text = string.Format("{0} ({1})", order.OrderTotalInCustomerCurrency, order.CustomerCurrencyCode);

        }

        private void BindOrderNotes()
        {
            Order order = OrderManager.GetOrderByID(this.OrderID);
            BindOrderNotes(order);
        }

        private void BindOrderNotes(Order order)
        {
            if (order == null)
                return;

            OrderNoteCollection orderNotes = order.OrderNotes;
            this.gvOrderNotes.DataSource = orderNotes;
            this.gvOrderNotes.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.BindData();
            }
        }

        protected void btnSetAsShipped_Click(object sender, EventArgs e)
        {
            try
            {
                OrderManager.Ship(this.OrderID, true);
                BindData();
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        protected void btnCapture_Click(object sender, EventArgs e)
        {
            try
            {
                string error = string.Empty;
                OrderManager.Capture(this.OrderID, ref error);
                if (String.IsNullOrEmpty(error))
                {
                    BindData();
                }
                else
                {
                    lblCaptureError.Text = error;
                }
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        protected void CancelOrderButton_Click(object sender, EventArgs e)
        {
            try
            {
                OrderManager.CancelOrder(this.OrderID, true);
                BindData();
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        protected void btnMarkAsPaid_Click(object sender, EventArgs e)
        {
            try
            {
                OrderManager.MarkOrderAsPaid(this.OrderID);
                BindData();
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        protected void btnAddNewOrderNote_Click(object sender, EventArgs e)
        {
            try
            {
                string note = txtNewOrderNote.Text.Trim();
                if (String.IsNullOrEmpty(note))
                    return;

                OrderNote orderNote = OrderManager.InsertOrderNote(this.OrderID, note, DateTime.Now);
                BindData();
                txtNewOrderNote.Text = string.Empty;
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        protected void btnGetInvoicePDF_Click(object sender, EventArgs e)
        {
            try
            {
                Order order = OrderManager.GetOrderByID(this.OrderID);

                string fileName = string.Format("order_{0}_{1}.pdf", order.OrderID, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                string filePath = string.Format("{0}files\\ExportImport\\{1}", HttpContext.Current.Request.PhysicalApplicationPath, fileName);

                PDFHelper.PrintOrderToPDF(order, NopContext.Current.WorkingLanguage.LanguageID, filePath);
                CommonHelper.WriteResponsePDF(filePath, fileName);
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                OrderManager.MarkOrderAsDeleted(this.OrderID);
                Response.Redirect("Orders.aspx");
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        protected void gvOrderNotes_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int orderNoteID = (int)gvOrderNotes.DataKeys[e.RowIndex]["OrderNoteID"];
            OrderManager.DeleteOrderNote(orderNoteID);
            BindOrderNotes();
        }

        public string GetProductURL(int ProductVariantID)
        {
            string result = string.Empty;
            ProductVariant productVariant = ProductManager.GetProductVariantByID(ProductVariantID);
            if (productVariant != null)
                result = "ProductVariantDetails.aspx?ProductVariantID=" + productVariant.ProductVariantID.ToString();
            else
                result = "Not available. Product variant ID=" + productVariant.ProductVariantID.ToString();
            return result;
        }

        public string GetProductVariantName(int ProductVariantID)
        {
            ProductVariant productVariant = ProductManager.GetProductVariantByID(ProductVariantID);
            if (productVariant != null)
                return productVariant.FullProductName;
            return "Not available. ID=" + ProductVariantID.ToString();
        }

        public string GetDownloadURL(OrderProductVariant orderProductVariant)
        {
            string result = string.Empty;
            ProductVariant productVariant = orderProductVariant.ProductVariant;
            if (productVariant != null)
            {
                if (productVariant.IsDownload)
                {
                    Download download = productVariant.Download;
                    if (download != null)
                        result = string.Format("<a href=\"{0}\" >Download</a>", DownloadManager.GetAdminDownloadUrl(download));
                    else
                        result = "Not available anymore";
                }
            }
            else
                result = "Not available. Product variant ID = " + orderProductVariant.ProductVariantID.ToString();
            return result;
        }

        public string GetProductVariantUnitPrice(OrderProductVariant orderProductVariant)
        {
            string result = string.Empty;

            Order order = orderProductVariant.Order;
            if (TaxManager.AllowCustomersToSelectTaxDisplayType)
            {
                string unitPriceInclTaxStr = PriceHelper.FormatPrice(orderProductVariant.UnitPriceInclTax, true, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingLanguage, true, true);
                string unitPriceExclTaxStr = PriceHelper.FormatPrice(orderProductVariant.UnitPriceExclTax, true, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingLanguage, false, true);            
                
                result = unitPriceInclTaxStr;
                result += "<br />";
                result += unitPriceExclTaxStr;
            }
            else
            {
                switch (TaxManager.TaxDisplayType)
                {
                    case TaxDisplayTypeEnum.ExcludingTax:
                        {
                            string unitPriceExclTaxStr = PriceHelper.FormatPrice(orderProductVariant.UnitPriceExclTax, true, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingLanguage, false);
                            result += unitPriceExclTaxStr;
                        }
                        break;
                    case TaxDisplayTypeEnum.IncludingTax:
                        {
                            string unitPriceInclTaxStr = PriceHelper.FormatPrice(orderProductVariant.UnitPriceInclTax, true, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingLanguage, true);
                            result = unitPriceInclTaxStr;
                        }
                        break;
                    default:
                        break;
                }
            }

            return result;
        }

        public string GetOrderProductVariantDiscountAmount(OrderProductVariant orderProductVariant)
        {
            string result = string.Empty;

            Order order = orderProductVariant.Order;
            if (TaxManager.AllowCustomersToSelectTaxDisplayType)
            {
                string discountAmountInclTaxStr = PriceHelper.FormatPrice(orderProductVariant.DiscountAmountInclTax, true, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingLanguage, true, true);
                string discountAmountExclTaxStr = PriceHelper.FormatPrice(orderProductVariant.DiscountAmountExclTax, true, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingLanguage, false, true);
            
                result = discountAmountInclTaxStr;
                result += "<br />";
                result += discountAmountExclTaxStr;
            }
            else
            {
                switch (TaxManager.TaxDisplayType)
                {
                    case TaxDisplayTypeEnum.ExcludingTax:
                        {
                            string discountAmountExclTaxStr = PriceHelper.FormatPrice(orderProductVariant.DiscountAmountExclTax, true, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingLanguage, false);            
                            result += discountAmountExclTaxStr;
                        }
                        break;
                    case TaxDisplayTypeEnum.IncludingTax:
                        {
                            string discountAmountInclTaxStr = PriceHelper.FormatPrice(orderProductVariant.DiscountAmountInclTax, true, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingLanguage, true);
                            result = discountAmountInclTaxStr;
                        }
                        break;
                    default:
                        break;
                }
            }

            return result;
        }

        public string GetOrderProductVariantSubTotal(OrderProductVariant orderProductVariant)
        {
            string result = string.Empty;

            Order order = orderProductVariant.Order;
            if (TaxManager.AllowCustomersToSelectTaxDisplayType)
            {
                string subTotalInclTaxStr = PriceHelper.FormatPrice(orderProductVariant.PriceInclTax, true, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingLanguage, true, true);
                string subTotalExclTaxStr = PriceHelper.FormatPrice(orderProductVariant.PriceExclTax, true, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingLanguage, false, true);
                
                result = subTotalInclTaxStr;
                result += "<br />";
                result += subTotalExclTaxStr;
            }
            else
            {
                switch (TaxManager.TaxDisplayType)
                {
                    case TaxDisplayTypeEnum.ExcludingTax:
                        {
                            string subTotalExclTaxStr = PriceHelper.FormatPrice(orderProductVariant.PriceExclTax, true, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingLanguage, false);
                            result += subTotalExclTaxStr;
                        }
                        break;
                    case TaxDisplayTypeEnum.IncludingTax:
                        {
                            string subTotalInclTaxStr = PriceHelper.FormatPrice(orderProductVariant.PriceInclTax, true, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingLanguage, true);
                            result = subTotalInclTaxStr;
                        }
                        break;
                    default:
                        break;
                }
            }

            return result;
        }

        public int OrderID
        {
            get
            {
                return CommonHelper.QueryStringInt("OrderID");
            }
        }
    }
}