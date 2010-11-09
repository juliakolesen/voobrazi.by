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
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using NopSolutions.NopCommerce.BusinessLogic.Localization;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Shipping;
using NopSolutions.NopCommerce.BusinessLogic.Tax;

namespace NopSolutions.NopCommerce.BusinessLogic.Utils
{
    /// <summary>
    /// Represents a PDF helper
    /// </summary>
    public partial class PDFHelper
    {
        #region Methods

        /// <summary>
        /// Print an order to PDF
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <param name="FilePath">File path</param>
        public static void PrintOrderToPDF(Order order, int LanguageID, string FilePath)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            if (String.IsNullOrEmpty(FilePath))
                throw new ArgumentNullException("FilePath");

            Document doc = new Document();

            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(FilePath, FileMode.Create));
            doc.Open();

            doc.Add(new Paragraph(String.Format(LocalizationManager.GetLocaleResourceString("PDFInvoice.Order#", LanguageID), order.OrderID)));
            doc.Add(new Paragraph(String.Format(LocalizationManager.GetLocaleResourceString("PDFInvoice.OrderDate", LanguageID), order.CreatedOn)));
            
            //billing info
            doc.Add(new Paragraph(" "));
            doc.Add(new Paragraph(LocalizationManager.GetLocaleResourceString("PDFInvoice.BillingInformation", LanguageID)));
            doc.Add(new Paragraph("   " + String.Format(LocalizationManager.GetLocaleResourceString("PDFInvoice.Name", LanguageID), order.BillingFullName)));
            doc.Add(new Paragraph("   " + String.Format(LocalizationManager.GetLocaleResourceString("PDFInvoice.Phone", LanguageID), order.BillingPhoneNumber)));
            if (!String.IsNullOrEmpty(order.BillingFaxNumber))
                doc.Add(new Paragraph("   " + String.Format(LocalizationManager.GetLocaleResourceString("PDFInvoice.Fax", LanguageID), order.BillingFaxNumber)));
            doc.Add(new Paragraph("   " + String.Format(LocalizationManager.GetLocaleResourceString("PDFInvoice.Address", LanguageID), order.BillingAddress1)));
            if (!String.IsNullOrEmpty(order.BillingAddress2))
                doc.Add(new Paragraph("   " + String.Format(LocalizationManager.GetLocaleResourceString("PDFInvoice.Address2", LanguageID), order.BillingAddress2)));
            doc.Add(new Paragraph("   " + String.Format("{0}, {1}", order.BillingCountry, order.BillingStateProvince)));
            doc.Add(new Paragraph("   " + String.Format("{0}, {1}", order.BillingCity, order.BillingZipPostalCode)));
            doc.Add(new Paragraph(" "));

            //shipping info
            if (order.ShippingStatus != ShippingStatusEnum.ShippingNotRequired)
            {
                doc.Add(new Paragraph(" "));
                doc.Add(new Paragraph(LocalizationManager.GetLocaleResourceString("PDFInvoice.ShippingInformation", LanguageID)));
                doc.Add(new Paragraph("   " + String.Format(LocalizationManager.GetLocaleResourceString("PDFInvoice.Name", LanguageID), order.ShippingFullName)));
                doc.Add(new Paragraph("   " + String.Format(LocalizationManager.GetLocaleResourceString("PDFInvoice.Phone", LanguageID), order.ShippingPhoneNumber)));
                if (!String.IsNullOrEmpty(order.ShippingFaxNumber))
                    doc.Add(new Paragraph("   " + String.Format(LocalizationManager.GetLocaleResourceString("PDFInvoice.Fax", LanguageID), order.ShippingFaxNumber)));
                doc.Add(new Paragraph("   " + String.Format(LocalizationManager.GetLocaleResourceString("PDFInvoice.Address", LanguageID), order.ShippingAddress1)));
                if (!String.IsNullOrEmpty(order.ShippingAddress2))
                    doc.Add(new Paragraph("   " + String.Format(LocalizationManager.GetLocaleResourceString("PDFInvoice.Address2", LanguageID), order.ShippingAddress2)));
                doc.Add(new Paragraph("   " + String.Format("{0}, {1}", order.ShippingCountry, order.ShippingStateProvince)));
                doc.Add(new Paragraph("   " + String.Format("{0}, {1}", order.ShippingCity, order.ShippingZipPostalCode)));
                doc.Add(new Paragraph(" "));
            }




            //products
            doc.Add(new Paragraph(LocalizationManager.GetLocaleResourceString("PDFInvoice.Product(s)", LanguageID)));
            doc.Add(new Paragraph(" "));

            OrderProductVariantCollection productCollection = order.OrderProductVariants;
            Table tbl = new Table(4, productCollection.Count + 1);
            tbl.AutoFillEmptyCells = true;
            tbl.Cellpadding = 2f;
            tbl.SetWidths(new int[4] { 40, 25, 10, 25 });

            Cell c1 = new Cell(LocalizationManager.GetLocaleResourceString("PDFInvoice.ProductName", LanguageID));
            c1.Header = true;
            c1.HorizontalAlignment = Element.ALIGN_CENTER;
            c1.VerticalAlignment = Element.ALIGN_MIDDLE;
            tbl.AddCell(c1, 0, 0);

            Cell c2 = new Cell(LocalizationManager.GetLocaleResourceString("PDFInvoice.ProductPrice", LanguageID));
            c2.Header = true;
            c2.HorizontalAlignment = Element.ALIGN_CENTER;
            c2.VerticalAlignment = Element.ALIGN_MIDDLE;
            tbl.AddCell(c2, 0, 1);

            Cell c3 = new Cell(LocalizationManager.GetLocaleResourceString("PDFInvoice.ProductQuantity", LanguageID));
            c3.Header = true;
            c3.HorizontalAlignment = Element.ALIGN_CENTER;
            c3.VerticalAlignment = Element.ALIGN_MIDDLE;
            tbl.AddCell(c3, 0, 2);

            Cell c4 = new Cell(LocalizationManager.GetLocaleResourceString("PDFInvoice.ProductTotal", LanguageID));
            c4.Header = true;
            c4.HorizontalAlignment = Element.ALIGN_CENTER;
            c4.VerticalAlignment = Element.ALIGN_MIDDLE;
            tbl.AddCell(c4, 0, 3);

            tbl.Width = 90f;

            for (int i = 0; i < productCollection.Count; i++)
            {
                OrderProductVariant orderProductVariant = productCollection[i];
                int row = i + 1;

                string name = String.Format("Not available. ID={0}", orderProductVariant.ProductVariantID);
                ProductVariant pv = ProductManager.GetProductVariantByID(orderProductVariant.ProductVariantID);
                if (pv != null)
                {
                    name = pv.FullProductName;
                }
                tbl.AddCell(new Cell(name), row, 0);

                string unitPrice = string.Empty;
                switch (order.CustomerTaxDisplayType)
                {
                    case TaxDisplayTypeEnum.ExcludingTax:
                        unitPrice = PriceHelper.FormatPrice(orderProductVariant.UnitPriceExclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, NopContext.Current.WorkingLanguage, false);
                        break;
                    case TaxDisplayTypeEnum.IncludingTax:
                        unitPrice = PriceHelper.FormatPrice(orderProductVariant.UnitPriceInclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, NopContext.Current.WorkingLanguage, true);
                        break;
                }

                tbl.AddCell(new Cell(unitPrice), row, 1);

                tbl.AddCell(new Cell(orderProductVariant.Quantity.ToString()), row, 2);

                string subTotal = string.Empty;
                switch (order.CustomerTaxDisplayType)
                {
                    case TaxDisplayTypeEnum.ExcludingTax:
                        subTotal = PriceHelper.FormatPrice(orderProductVariant.PriceExclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, NopContext.Current.WorkingLanguage, false);
                        break;
                    case TaxDisplayTypeEnum.IncludingTax:
                        subTotal = PriceHelper.FormatPrice(orderProductVariant.PriceInclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, NopContext.Current.WorkingLanguage, true);
                        break;
                }
                tbl.AddCell(new Cell(subTotal), row, 3);
            }

            doc.Add(tbl);



            //subtotal
            doc.Add(new Paragraph(" "));
            Paragraph p1 = null;
            switch (order.CustomerTaxDisplayType)
            {
                case TaxDisplayTypeEnum.ExcludingTax:
                    {
                        string orderSubtotalExclTaxStr = PriceHelper.FormatPrice(order.OrderSubtotalExclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, NopContext.Current.WorkingLanguage, false);
                        p1 = new Paragraph(String.Format("{0} {1}", LocalizationManager.GetLocaleResourceString("PDFInvoice.Sub-Total", LanguageID), orderSubtotalExclTaxStr));
                    }
                    break;
                case TaxDisplayTypeEnum.IncludingTax:
                    {
                        string orderSubtotalInclTaxStr = PriceHelper.FormatPrice(order.OrderSubtotalInclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, NopContext.Current.WorkingLanguage, true);
                        p1 = new Paragraph(String.Format("{0} {1}", LocalizationManager.GetLocaleResourceString("PDFInvoice.Sub-Total", LanguageID), orderSubtotalInclTaxStr));
                    }
                    break;
            }
            if (p1 != null)
            {
                p1.Alignment = Element.ALIGN_RIGHT;
                doc.Add(p1);
            }

            //shipping
            Paragraph p2 = null;
            switch (order.CustomerTaxDisplayType)
            {
                case TaxDisplayTypeEnum.ExcludingTax:
                    {
                        string orderShippingExclTaxStr = PriceHelper.FormatShippingPrice(order.OrderShippingExclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, NopContext.Current.WorkingLanguage, false);
                        p2 = new Paragraph(String.Format("{0} {1}", LocalizationManager.GetLocaleResourceString("PDFInvoice.Shipping", LanguageID), orderShippingExclTaxStr));
                    }
                    break;
                case TaxDisplayTypeEnum.IncludingTax:
                    {
                        string orderShippingInclTaxStr = PriceHelper.FormatShippingPrice(order.OrderShippingInclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, NopContext.Current.WorkingLanguage, true);
                        p2 = new Paragraph(String.Format("{0} {1}", LocalizationManager.GetLocaleResourceString("PDFInvoice.Shipping", LanguageID), orderShippingInclTaxStr));
                    }
                    break;
            }
            
            if (p2 != null)
            {
                p2.Alignment = Element.ALIGN_RIGHT;
                doc.Add(p2);
            }

            //payment fee
            bool displayPaymentMethodFee = true;
            if (order.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency == decimal.Zero)
            {
                displayPaymentMethodFee = false;
            }
            if (displayPaymentMethodFee)
            {
                Paragraph p3 = null;
                switch (order.CustomerTaxDisplayType)
                {
                    case TaxDisplayTypeEnum.ExcludingTax:
                        {
                            string paymentMethodAdditionalFeeExclTaxStr = PriceHelper.FormatPaymentMethodAdditionalFee(order.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, NopContext.Current.WorkingLanguage, false);
                            p3 = new Paragraph(String.Format("{0} {1}", LocalizationManager.GetLocaleResourceString("PDFInvoice.PaymentMethodAdditionalFee", LanguageID), paymentMethodAdditionalFeeExclTaxStr));
                        }
                        break;
                    case TaxDisplayTypeEnum.IncludingTax:
                        {
                            string paymentMethodAdditionalFeeInclTaxStr = PriceHelper.FormatPaymentMethodAdditionalFee(order.PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, NopContext.Current.WorkingLanguage, true);
                            p3 = new Paragraph(String.Format("{0} {1}", LocalizationManager.GetLocaleResourceString("PDFInvoice.PaymentMethodAdditionalFee", LanguageID), paymentMethodAdditionalFeeInclTaxStr));
                        }
                        break;
                }
                if (p3 != null)
                {
                    p3.Alignment = Element.ALIGN_RIGHT;
                    doc.Add(p3);
                }
            }

            string taxStr = string.Empty;
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
                    taxStr = string.Format("{0} ({1})", order.OrderTaxInCustomerCurrency.ToString("N"), order.CustomerCurrencyCode);
                }
            }
            if (displayTax)
            {
                Paragraph p4 = new Paragraph(String.Format("{0} {1}", LocalizationManager.GetLocaleResourceString("PDFInvoice.Tax", LanguageID), taxStr));
                p4.Alignment = Element.ALIGN_RIGHT;
                doc.Add(p4);
            }

            string totalStr = string.Format("{0} ({1})", order.OrderTotalInCustomerCurrency.ToString("N"), order.CustomerCurrencyCode);
            Paragraph p5 = new Paragraph(String.Format("{0} {1}", LocalizationManager.GetLocaleResourceString("PDFInvoice.OrderTotal", LanguageID), totalStr));
            p5.Alignment = Element.ALIGN_RIGHT;
            doc.Add(p5);

            doc.Close();
        }
        #endregion
    }
}
