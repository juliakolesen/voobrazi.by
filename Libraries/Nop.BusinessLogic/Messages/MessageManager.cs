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
using System.Collections.Specialized;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.Content.Blog;
using NopSolutions.NopCommerce.BusinessLogic.Content.Forums;
using NopSolutions.NopCommerce.BusinessLogic.Content.NewsManagement;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Localization;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.BusinessLogic.Shipping;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.Common.Utils.Html;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Messages;

namespace NopSolutions.NopCommerce.BusinessLogic.Messages
{
    /// <summary>
    /// Message manager
    /// </summary>
    public partial class MessageManager
    {
        #region Utilities

        private static MessageTemplateCollection DBMapping(DBMessageTemplateCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            MessageTemplateCollection collection = new MessageTemplateCollection();
            foreach (DBMessageTemplate dbItem in dbCollection)
            {
                MessageTemplate item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static MessageTemplate DBMapping(DBMessageTemplate dbItem)
        {
            if (dbItem == null)
                return null;

            MessageTemplate item = new MessageTemplate();
            item.MessageTemplateID = dbItem.MessageTemplateID;
            item.Name = dbItem.Name;

            return item;
        }

        private static LocalizedMessageTemplateCollection DBMapping(DBLocalizedMessageTemplateCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            LocalizedMessageTemplateCollection collection = new LocalizedMessageTemplateCollection();
            foreach (DBLocalizedMessageTemplate dbItem in dbCollection)
            {
                LocalizedMessageTemplate item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static LocalizedMessageTemplate DBMapping(DBLocalizedMessageTemplate dbItem)
        {
            if (dbItem == null)
                return null;

            LocalizedMessageTemplate item = new LocalizedMessageTemplate();
            item.MessageTemplateLocalizedID = dbItem.MessageTemplateLocalizedID;
            item.MessageTemplateID = dbItem.MessageTemplateID;
            item.LanguageID = dbItem.LanguageID;
            item.Name = dbItem.Name;
            item.Subject = dbItem.Subject;
            item.Body = dbItem.Body;

            return item;
        }

        private static QueuedEmailCollection DBMapping(DBQueuedEmailCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            QueuedEmailCollection collection = new QueuedEmailCollection();
            foreach (DBQueuedEmail dbItem in dbCollection)
            {
                QueuedEmail item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static QueuedEmail DBMapping(DBQueuedEmail dbItem)
        {
            if (dbItem == null)
                return null;

            QueuedEmail item = new QueuedEmail();
            item.QueuedEmailID = dbItem.QueuedEmailID;
            item.Priority = dbItem.Priority;
            item.From = dbItem.From;
            item.FromName = dbItem.FromName;
            item.To = dbItem.To;
            item.ToName = dbItem.ToName;
            item.Cc = dbItem.Cc;
            item.Bcc = dbItem.Bcc;
            item.Subject = dbItem.Subject;
            item.Body = dbItem.Body;
            item.CreatedOn = dbItem.CreatedOn;
            item.SendTries = dbItem.SendTries;
            item.SentOn = dbItem.SentOn;

            return item;
        }

        /// <summary>
        /// Convert a collection to a HTML table
        /// </summary>
        /// <param name="table">Order product variant collection</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>HTML table of products</returns>
        private static string ProductListToHtmlTable(Order order, int LanguageID)
        {
            string result = string.Empty;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<table class=\"table\" border=\"0\" style=\"border:1px solid grey;padding:2px;border-collapse:collapse;\"><thead><tr>");

            sb.AppendLine("<th class=\"header\" style=\"text-align:left;\">" + LocalizationManager.GetLocaleResourceString("Order.ProductsGrid.Name", LanguageID) + "</th>");
            sb.AppendLine("<th class=\"header\" style=\"text-align:right;\">" + LocalizationManager.GetLocaleResourceString("Order.ProductsGrid.Price", LanguageID) + "</th>");
            sb.AppendLine("<th class=\"header\" style=\"text-align:right;\">" + LocalizationManager.GetLocaleResourceString("Order.ProductsGrid.Quantity", LanguageID) + "</th>");
            sb.AppendLine("<th class=\"header\" style=\"text-align:right;\">" + LocalizationManager.GetLocaleResourceString("Order.ProductsGrid.Total", LanguageID) + "</th>");

            sb.AppendLine("</tr></thead><tbody>");
            Language language = LanguageManager.GetLanguageByID(LanguageID);
            if (language == null)
                language = NopContext.Current.WorkingLanguage;

            #region Products
            OrderProductVariantCollection table = order.OrderProductVariants;
            for (int i = 0; i <= table.Count - 1; i++)
            {
                sb.AppendLine("<tr>");

                sb.AppendLine("<td class=\"row\">" + HttpUtility.HtmlEncode(table[i].ProductVariant.FullProductName));
                if (!String.IsNullOrEmpty(table[i].AttributeDescription))
                {
                    sb.AppendLine("<br />");
                    sb.AppendLine(table[i].AttributeDescription);
                }
                sb.AppendLine("</td>");

                string unitPriceStr = string.Empty;
                switch (order.CustomerTaxDisplayType)
                {
                    case TaxDisplayTypeEnum.ExcludingTax:
                        unitPriceStr = PriceHelper.FormatPrice(table[i].UnitPriceExclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, language, false);
                        break;
                    case TaxDisplayTypeEnum.IncludingTax:
                        unitPriceStr = PriceHelper.FormatPrice(table[i].UnitPriceInclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, language, true);
                        break;
                }
                sb.AppendLine("<td class=\"row\">" + unitPriceStr + "</td>");

                sb.AppendLine("<td class=\"row\">" + table[i].Quantity + "</td>");

                string priceStr = string.Empty;
                switch (order.CustomerTaxDisplayType)
                {
                    case TaxDisplayTypeEnum.ExcludingTax:
                        priceStr = PriceHelper.FormatPrice(table[i].PriceExclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, language, false);
                        break;
                    case TaxDisplayTypeEnum.IncludingTax:
                        priceStr = PriceHelper.FormatPrice(table[i].PriceInclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, language, true);
                        break;
                }
                sb.AppendLine("<td class=\"row\">" + priceStr + "</td>");

                sb.AppendLine("</tr>");
            }
            #endregion

            #region Totals
            string CusSubTotal = string.Empty;
            string CusShipTotal = string.Empty;
            string CusPaymentMethodAdditionalFee = string.Empty;
            string CusTaxTotal = string.Empty;
            string CusTotal = string.Empty;
            //subtotal, shipping, payment method fee
            switch (order.CustomerTaxDisplayType)
            {
                case TaxDisplayTypeEnum.ExcludingTax:
                    {
                        CusSubTotal = PriceHelper.FormatPrice(order.OrderSubtotalExclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, language, false);
                        CusShipTotal = PriceHelper.FormatShippingPrice(order.OrderShippingExclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, language, false);
                        CusPaymentMethodAdditionalFee = PriceHelper.FormatPaymentMethodAdditionalFee(order.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, language, false);
                    }
                    break;
                case TaxDisplayTypeEnum.IncludingTax:
                    {
                        CusSubTotal = PriceHelper.FormatPrice(order.OrderSubtotalInclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, language, true);
                        CusShipTotal = PriceHelper.FormatShippingPrice(order.OrderShippingInclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, language, true);
                        CusPaymentMethodAdditionalFee = PriceHelper.FormatPaymentMethodAdditionalFee(order.PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, language, true);
                    }
                    break;
            }

            //shipping
            bool dislayShipping = order.ShippingStatus != ShippingStatusEnum.ShippingNotRequired;
            bool displayPaymentMethodFee = true;
            if (order.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency == decimal.Zero)
            {
                displayPaymentMethodFee = false;
            }

            //tax
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
                    CusTaxTotal = taxStr;
                }
            }

            //total
            CusTotal = PriceHelper.FormatPrice(order.OrderTotalInCustomerCurrency, true, order.CustomerCurrencyCode, false);




            sb.AppendLine("<tr><td style=\"text-align:right;\" colspan=\"2\"></td><td colspan=\"2\">");
            sb.AppendLine("<table class=\"table\" style=\"border:0px solid grey;padding:2px;border-collapse:collapse;\">");
            sb.AppendLine("<tr><td style=\"text-align:right;\" colspan=\"3\"><strong>" + LocalizationManager.GetLocaleResourceString("Order.Sub-Total", LanguageID) + "</strong></td> <td style=\"text-align:right;\"><strong>" + CusSubTotal + "</strong></td></tr>");
            if (dislayShipping)
            {
                sb.AppendLine("<tr><td style=\"text-align:right;\" colspan=\"3\"><strong>" + LocalizationManager.GetLocaleResourceString("Order.Shipping", LanguageID) + "</strong></td> <td style=\"text-align:right;\"><strong>" + CusShipTotal + "</strong></td></tr>");
            }
            if (displayPaymentMethodFee)
            {
                string paymentMethodFeeTitle = LocalizationManager.GetLocaleResourceString("Order.PaymentMethodAdditionalFee", LanguageID);
                sb.AppendLine("<tr><td style=\"text-align:right;\" colspan=\"3\"><strong>" + paymentMethodFeeTitle + "</strong></td> <td style=\"text-align:right;\"><strong>" + CusPaymentMethodAdditionalFee + "</strong></td></tr>");
            }
            if (displayTax)
            {
                sb.AppendLine("<tr><td style=\"text-align:right;\" colspan=\"3\"><strong>" + LocalizationManager.GetLocaleResourceString("Order.Tax", LanguageID) + "</strong></td> <td style=\"text-align:right;\"><strong>" + CusTaxTotal + "</strong></td></tr>");
            }
            sb.AppendLine("<tr><td style=\"text-align:right;\" colspan=\"3\"><strong>" + LocalizationManager.GetLocaleResourceString("Order.OrderTotal", LanguageID) + "</strong></td> <td style=\"text-align:right;\"><strong>" + CusTotal + "</strong></td></tr>");
            sb.AppendLine("</table>");
            sb.AppendLine("</td></tr>");
            #endregion
            
            sb.AppendLine("</tbody></table>");
            result = sb.ToString();
            return result;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a message template by template identifier
        /// </summary>
        /// <param name="MessageTemplateID">Message template identifier</param>
        /// <returns>Message template</returns>
        public static MessageTemplate GetMessageTemplateByID(int MessageTemplateID)
        {
            if (MessageTemplateID == 0)
                return null;

            DBMessageTemplate dbItem = DBProviderManager<DBMessageTemplateProvider>.Provider.GetMessageTemplateByID(MessageTemplateID);
            MessageTemplate messageTemplate = DBMapping(dbItem);
            return messageTemplate;
        }

        /// <summary>
        /// Gets all message templates
        /// </summary>
        /// <returns>Message template collection</returns>
        public static MessageTemplateCollection GetAllMessageTemplates()
        {
            DBMessageTemplateCollection dbCollection = DBProviderManager<DBMessageTemplateProvider>.Provider.GetAllMessageTemplates();
            MessageTemplateCollection collection = DBMapping(dbCollection);
            return collection;
        }

        /// <summary>
        /// Gets a localized message template by identifier
        /// </summary>
        /// <param name="LocalizedMessageTemplateID">Localized message template identifier</param>
        /// <returns>Localized message template</returns>
        public static LocalizedMessageTemplate GetLocalizedMessageTemplateByID(int LocalizedMessageTemplateID)
        {
            if (LocalizedMessageTemplateID == 0)
                return null;

            DBLocalizedMessageTemplate dbItem = DBProviderManager<DBMessageTemplateProvider>.Provider.GetLocalizedMessageTemplateByID(LocalizedMessageTemplateID);
            LocalizedMessageTemplate localizedMessageTemplate = DBMapping(dbItem);
            return localizedMessageTemplate;
        }

        /// <summary>
        /// Gets a localized message template by name and language identifier
        /// </summary>
        /// <param name="Name">Message template name</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Localized message template</returns>
        public static LocalizedMessageTemplate GetLocalizedMessageTemplate(string Name, int LanguageID)
        {
            DBLocalizedMessageTemplate dbItem = DBProviderManager<DBMessageTemplateProvider>.Provider.GetLocalizedMessageTemplate(Name, LanguageID);
            LocalizedMessageTemplate localizedMessageTemplate = DBMapping(dbItem);
            return localizedMessageTemplate;
        }

        /// <summary>
        /// Deletes a localized message template
        /// </summary>
        /// <param name="LocalizedMessageTemplateID">Message template identifier</param>
        public static void DeleteLocalizedMessageTemplate(int LocalizedMessageTemplateID)
        {
            DBProviderManager<DBMessageTemplateProvider>.Provider.DeleteLocalizedMessageTemplate(LocalizedMessageTemplateID);
        }

        /// <summary>
        /// Gets all localized message templates
        /// </summary>
        /// <param name="MessageTemplatesName">Message template name</param>
        /// <returns>Localized message template collection</returns>
        public static LocalizedMessageTemplateCollection GetAllLocalizedMessageTemplates(string MessageTemplatesName)
        {
            DBLocalizedMessageTemplateCollection dbCollection = DBProviderManager<DBMessageTemplateProvider>.Provider.GetAllLocalizedMessageTemplates(MessageTemplatesName);
            LocalizedMessageTemplateCollection localizedMessageTemplates = DBMapping(dbCollection);
            return localizedMessageTemplates;
        }

        /// <summary>
        /// Inserts a localized message template
        /// </summary>
        /// <param name="MessageTemplateID">The message template identifier</param>
        /// <param name="LanguageID">The language identifier</param>
        /// <param name="Subject">The subject</param>
        /// <param name="Body">The body</param>
        /// <returns>Localized message template</returns>
        public static LocalizedMessageTemplate InsertLocalizedMessageTemplate(int MessageTemplateID,
            int LanguageID, string Subject, string Body)
        {
            DBLocalizedMessageTemplate dbItem = DBProviderManager<DBMessageTemplateProvider>.Provider.InsertLocalizedMessageTemplate(MessageTemplateID,
                LanguageID, Subject, Body);
            LocalizedMessageTemplate localizedMessageTemplate = DBMapping(dbItem);
            return localizedMessageTemplate;
        }

        /// <summary>
        /// Updates the localized message template
        /// </summary>
        /// <param name="MessageTemplateLocalizedID">The localized message template identifier</param>
        /// <param name="MessageTemplateID">The message template identifier</param>
        /// <param name="LanguageID">The language identifier</param>
        /// <param name="Subject">The subject</param>
        /// <param name="Body">The body</param>
        /// <returns>Localized message template</returns>
        public static LocalizedMessageTemplate UpdateLocalizedMessageTemplate(int MessageTemplateLocalizedID,
            int MessageTemplateID, int LanguageID,
            string Subject, string Body)
        {
            DBLocalizedMessageTemplate dbItem = DBProviderManager<DBMessageTemplateProvider>.Provider.UpdateLocalizedMessageTemplate(MessageTemplateLocalizedID,
                MessageTemplateID, LanguageID, Subject, Body);
            LocalizedMessageTemplate localizedMessageTemplate = DBMapping(dbItem);
            return localizedMessageTemplate;
        }

        /// <summary>
        /// Gets a queued email by identifier
        /// </summary>
        /// <param name="QueuedEmailID">Email item identifier</param>
        /// <returns>Email item</returns>
        public static QueuedEmail GetQueuedEmailByID(int QueuedEmailID)
        {
            if (QueuedEmailID == 0)
                return null;

            DBQueuedEmail dbItem = DBProviderManager<DBMessageTemplateProvider>.Provider.GetQueuedEmailByID(QueuedEmailID);
            QueuedEmail queuedEmail = DBMapping(dbItem);
            return queuedEmail;
        }

        /// <summary>
        /// Deletes a queued email
        /// </summary>
        /// <param name="QueuedEmailID">Email item identifier</param>
        public static void DeleteQueuedEmail(int QueuedEmailID)
        {
            DBProviderManager<DBMessageTemplateProvider>.Provider.DeleteQueuedEmail(QueuedEmailID);
        }

        /// <summary>
        /// Gets all queued emails
        /// </summary>
        /// <param name="QueuedEmailCount">Email item count. 0 if you want to get all items</param>
        /// <param name="LoadNotSentItemsOnly">A value indicating whether to load only not sent emails</param>
        /// <param name="MaxSendTries">Maximum send tries</param>
        /// <returns>Email item collection</returns>
        public static QueuedEmailCollection GetAllQueuedEmails(int QueuedEmailCount, bool LoadNotSentItemsOnly, int MaxSendTries)
        {
            return GetAllQueuedEmails(string.Empty, string.Empty, null, null, 
                QueuedEmailCount, LoadNotSentItemsOnly, MaxSendTries);
        }

        /// <summary>
        /// Gets all queued emails
        /// </summary>
        /// <param name="FromEmail">From Email</param>
        /// <param name="ToEmail">To Email</param>
        /// <param name="StartTime">The start time</param>
        /// <param name="EndTime">The end time</param>
        /// <param name="QueuedEmailCount">Email item count. 0 if you want to get all items</param>
        /// <param name="LoadNotSentItemsOnly">A value indicating whether to load only not sent emails</param>
        /// <param name="MaxSendTries">Maximum send tries</param>
        /// <returns>Email item collection</returns>
        public static QueuedEmailCollection GetAllQueuedEmails(string FromEmail,
            string ToEmail, DateTime? StartTime, DateTime? EndTime,
            int QueuedEmailCount, bool LoadNotSentItemsOnly, int MaxSendTries)
        {
            if (FromEmail == null)
                FromEmail = string.Empty;
            FromEmail = FromEmail.Trim();

            if (ToEmail == null)
                ToEmail = string.Empty;
            ToEmail = ToEmail.Trim();

            DBQueuedEmailCollection dbCollection = DBProviderManager<DBMessageTemplateProvider>.Provider.GetAllQueuedEmails(FromEmail,
                ToEmail, StartTime, EndTime, QueuedEmailCount, LoadNotSentItemsOnly, MaxSendTries);
            QueuedEmailCollection queuedEmails = DBMapping(dbCollection);
            return queuedEmails;
        }

        /// <summary>
        /// Inserts a queued email
        /// </summary>
        /// <param name="Priority">The priority</param>
        /// <param name="From">From</param>
        /// <param name="To">To</param>
        /// <param name="Cc">Cc</param>
        /// <param name="Bcc">Bcc</param>
        /// <param name="Subject">Subject</param>
        /// <param name="Body">Body</param>
        /// <param name="CreatedOn">The date and time of item creation</param>
        /// <param name="SendTries">The send tries</param>
        /// <param name="SentOn">The sent date and time. Null if email is not sent yet</param>
        /// <returns>Queued email</returns>
        public static QueuedEmail InsertQueuedEmail(int Priority, MailAddress From,
             MailAddress To, string Cc, string Bcc,
            string Subject, string Body, DateTime CreatedOn, int SendTries, DateTime? SentOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            if (SentOn.HasValue)
                SentOn = DateTimeHelper.ConvertToUtcTime(SentOn.Value);

            return InsertQueuedEmail(Priority, From.Address, From.DisplayName,
              To.Address, To.DisplayName, Cc, Bcc, Subject, Body, CreatedOn, SendTries, SentOn);
        }

        /// <summary>
        /// Inserts a queued email
        /// </summary>
        /// <param name="Priority">The priority</param>
        /// <param name="From">From</param>
        /// <param name="FromName">From name</param>
        /// <param name="To">To</param>
        /// <param name="ToName">To name</param>
        /// <param name="Cc">Cc</param>
        /// <param name="Bcc">Bcc</param>
        /// <param name="Subject">Subject</param>
        /// <param name="Body">Body</param>
        /// <param name="CreatedOn">The date and time of item creation</param>
        /// <param name="SendTries">The send tries</param>
        /// <param name="SentOn">The sent date and time. Null if email is not sent yet</param>
        /// <returns>Queued email</returns>
        public static QueuedEmail InsertQueuedEmail(int Priority, string From,
            string FromName, string To, string ToName, string Cc, string Bcc,
            string Subject, string Body, DateTime CreatedOn, int SendTries, DateTime? SentOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            if (SentOn.HasValue)
                SentOn = DateTimeHelper.ConvertToUtcTime(SentOn.Value);

            DBQueuedEmail dbItem = DBProviderManager<DBMessageTemplateProvider>.Provider.InsertQueuedEmail(Priority, From,
                FromName, To, ToName, Cc, Bcc, Subject, Body, CreatedOn, SendTries, SentOn);
            QueuedEmail queuedEmail = DBMapping(dbItem);
            return queuedEmail;
        }

        /// <summary>
        /// Updates a queued email
        /// </summary>
        /// <param name="QueuedEmailID">Email item identifier</param>
        /// <param name="Priority">The priority</param>
        /// <param name="From">From</param>
        /// <param name="FromName">From name</param>
        /// <param name="To">To</param>
        /// <param name="ToName">To name</param>
        /// <param name="Cc">Cc</param>
        /// <param name="Bcc">Bcc</param>
        /// <param name="Subject">Subject</param>
        /// <param name="Body">Body</param>
        /// <param name="CreatedOn">The date and time of item creation</param>
        /// <param name="SendTries">The send tries</param>
        /// <param name="SentOn">The sent date and time. Null if email is not sent yet</param>
        /// <returns>Queued email</returns>
        public static QueuedEmail UpdateQueuedEmail(int QueuedEmailID, int Priority, string From,
            string FromName, string To, string ToName, string Cc, string Bcc,
            string Subject, string Body, DateTime CreatedOn, int SendTries, DateTime? SentOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            if (SentOn.HasValue)
                SentOn = DateTimeHelper.ConvertToUtcTime(SentOn.Value);

            DBQueuedEmail dbItem = DBProviderManager<DBMessageTemplateProvider>.Provider.UpdateQueuedEmail(QueuedEmailID, Priority,
                From, FromName, To, ToName, Cc, Bcc, Subject, Body, CreatedOn, SendTries, SentOn);
            QueuedEmail queuedEmail = DBMapping(dbItem);
            return queuedEmail;
        }

        /// <summary>
        /// Sends an order completed notification to a customer
        /// </summary>
        /// <param name="order">Order instance</param>
        /// <param name="LanguageID">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public static int SendOrderCompletedCustomerNotification(Order order, int LanguageID)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            string TemplateName = "OrderCompleted.CustomerNotification";
            LocalizedMessageTemplate localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(TemplateName, LanguageID);
            if (localizedMessageTemplate == null)
                return 0;
                //throw new NopException(string.Format("Message template ({0}-{1}) couldn't be loaded", TemplateName, LanguageID));

            string subject = ReplaceMessageTemplateTokens(order, localizedMessageTemplate.Subject, LanguageID);
            string body = ReplaceMessageTemplateTokens(order, localizedMessageTemplate.Body, LanguageID);
            MailAddress from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            MailAddress to = new MailAddress(order.BillingEmail, order.BillingFullName);
            QueuedEmail queuedEmail = InsertQueuedEmail(5, from, to, string.Empty, string.Empty, subject, body, DateTime.Now, 0, null);
            return queuedEmail.QueuedEmailID;
        }

        /// <summary>
        /// Sends an order placed notification to a store owner
        /// </summary>
        /// <param name="order">Order instance</param>
        /// <param name="LanguageID">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public static int SendOrderPlacedStoreOwnerNotification(Order order, int LanguageID)
        {
            if (order == null)
                throw new ArgumentNullException("order");


            string TemplateName = "OrderPlaced.StoreOwnerNotification";
            LocalizedMessageTemplate localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(TemplateName, LanguageID);
            if (localizedMessageTemplate == null)
                return 0;
                //throw new NopException(string.Format("Message template ({0}-{1}) couldn't be loaded", TemplateName, LanguageID));

            string subject = ReplaceMessageTemplateTokens(order, localizedMessageTemplate.Subject, LanguageID);
            string body = ReplaceMessageTemplateTokens(order, localizedMessageTemplate.Body, LanguageID);
            MailAddress from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            MailAddress to = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            QueuedEmail queuedEmail = InsertQueuedEmail(5, from, to, string.Empty, string.Empty, subject, body, DateTime.Now, 0, null);
            return queuedEmail.QueuedEmailID;
        }

        /// <summary>
        /// Sends a "quantity below" notification to a store owner
        /// </summary>
        /// <param name="productVariant">Product variant</param>
        /// <param name="LanguageID">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public static int SendQuantityBelowStoreOwnerNotification(ProductVariant productVariant, int LanguageID)
        {
            if (productVariant == null)
                throw new ArgumentNullException("productVariant");

            string TemplateName = "QuantityBelow.StoreOwnerNotification";
            LocalizedMessageTemplate localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(TemplateName, LanguageID);
            if (localizedMessageTemplate == null)
                return 0;
                //throw new NopException(string.Format("Message template ({0}-{1}) couldn't be loaded", TemplateName, LanguageID));

            string subject = ReplaceMessageTemplateTokens(productVariant, localizedMessageTemplate.Subject, LanguageID);
            string body = ReplaceMessageTemplateTokens(productVariant, localizedMessageTemplate.Body, LanguageID);
            MailAddress from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            MailAddress to = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            QueuedEmail queuedEmail = InsertQueuedEmail(5, from, to, string.Empty, string.Empty, subject, body, DateTime.Now, 0, null);
            return queuedEmail.QueuedEmailID;
        }

        /// <summary>
        /// Sends an order placed notification to a customer
        /// </summary>
        /// <param name="order">Order instance</param>
        /// <param name="LanguageID">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public static int SendOrderPlacedCustomerNotification(Order order, int LanguageID)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            string TemplateName = "OrderPlaced.CustomerNotification";
            LocalizedMessageTemplate localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(TemplateName, LanguageID);
            if (localizedMessageTemplate == null)
                return 0;
                //throw new NopException(string.Format("Message template ({0}-{1}) couldn't be loaded", TemplateName, LanguageID));

            string subject = ReplaceMessageTemplateTokens(order, localizedMessageTemplate.Subject, LanguageID);
            string body = ReplaceMessageTemplateTokens(order, localizedMessageTemplate.Body, LanguageID);
            MailAddress from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            MailAddress to = new MailAddress(order.BillingEmail, order.BillingFullName);
            QueuedEmail queuedEmail = InsertQueuedEmail(5, from, to, string.Empty, string.Empty, subject, body, DateTime.Now, 0, null);
            return queuedEmail.QueuedEmailID;
        }

        /// <summary>
        /// Sends an order shipped notification to a customer
        /// </summary>
        /// <param name="order">Order instance</param>
        /// <param name="LanguageID">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public static int SendOrderShippedCustomerNotification(Order order, int LanguageID)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            string TemplateName = "OrderShipped.CustomerNotification";
            LocalizedMessageTemplate localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(TemplateName, LanguageID);
            if (localizedMessageTemplate == null)
                return 0;
                //throw new NopException(string.Format("Message template ({0}-{1}) couldn't be loaded", TemplateName, LanguageID));

            string subject = ReplaceMessageTemplateTokens(order, localizedMessageTemplate.Subject, LanguageID);
            string body = ReplaceMessageTemplateTokens(order, localizedMessageTemplate.Body, LanguageID);
            MailAddress from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            MailAddress to = new MailAddress(order.BillingEmail, order.BillingFullName);            
            QueuedEmail queuedEmail = InsertQueuedEmail(5, from, to, string.Empty, string.Empty, subject, body, DateTime.Now, 0, null);
            return queuedEmail.QueuedEmailID;
        }

        /// <summary>
        /// Sends an order cancelled notification to a customer
        /// </summary>
        /// <param name="order">Order instance</param>
        /// <param name="LanguageID">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public static int SendOrderCancelledCustomerNotification(Order order, int LanguageID)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            string TemplateName = "OrderCancelled.CustomerNotification";
            LocalizedMessageTemplate localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(TemplateName, LanguageID);
            if (localizedMessageTemplate == null)
                return 0;
                //throw new NopException(string.Format("Message template ({0}-{1}) couldn't be loaded", TemplateName, LanguageID));

            string subject = ReplaceMessageTemplateTokens(order, localizedMessageTemplate.Subject, LanguageID);
            string body = ReplaceMessageTemplateTokens(order, localizedMessageTemplate.Body, LanguageID);
            MailAddress from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            MailAddress to = new MailAddress(order.BillingEmail, order.BillingFullName);
            QueuedEmail queuedEmail = InsertQueuedEmail(5, from, to, string.Empty, string.Empty, subject, body, DateTime.Now, 0, null);
            return queuedEmail.QueuedEmailID;
        }

        /// <summary>
        /// Sends a welcome message to a customer
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="LanguageID">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public static int SendCustomerWelcomeMessage(Customer customer, int LanguageID)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");


            string TemplateName = "Customer.WelcomeMessage";
            LocalizedMessageTemplate localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(TemplateName, LanguageID);
            if (localizedMessageTemplate == null)
                return 0;
                //throw new NopException(string.Format("Message template ({0}-{1}) couldn't be loaded", TemplateName, LanguageID));

            string subject = ReplaceMessageTemplateTokens(customer, localizedMessageTemplate.Subject);
            string body = ReplaceMessageTemplateTokens(customer, localizedMessageTemplate.Body);
            MailAddress from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            MailAddress to = new MailAddress(customer.Email, customer.FullName);
            QueuedEmail queuedEmail = InsertQueuedEmail(5, from, to, string.Empty, string.Empty, subject, body, DateTime.Now, 0, null);
            return queuedEmail.QueuedEmailID;
        }

        /// <summary>
        /// Sends an email validation message to a customer
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="LanguageID">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public static int SendCustomerEmailValidationMessage(Customer customer, int LanguageID)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");


            string TemplateName = "Customer.EmailValidationMessage";
            LocalizedMessageTemplate localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(TemplateName, LanguageID);
            if (localizedMessageTemplate == null)
                return 0;
                //throw new NopException(string.Format("Message template ({0}-{1}) couldn't be loaded", TemplateName, LanguageID));

            string subject = ReplaceMessageTemplateTokens(customer, localizedMessageTemplate.Subject);
            string body = ReplaceMessageTemplateTokens(customer, localizedMessageTemplate.Body);
            MailAddress from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            MailAddress to = new MailAddress(customer.Email, customer.FullName);
            QueuedEmail queuedEmail = InsertQueuedEmail(5, from, to, string.Empty, string.Empty, subject, body, DateTime.Now, 0, null);
            return queuedEmail.QueuedEmailID;
        }

        /// <summary>
        /// Sends password recovery message to a customer
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="LanguageID">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public static int SendCustomerPasswordRecoveryMessage(Customer customer, int LanguageID)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            string TemplateName = "Customer.PasswordRecovery";
            LocalizedMessageTemplate localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(TemplateName, LanguageID);
            if (localizedMessageTemplate == null)
                return 0;
                //throw new NopException(string.Format("Message template ({0}-{1}) couldn't be loaded", TemplateName, LanguageID));

            string subject = ReplaceMessageTemplateTokens(customer, localizedMessageTemplate.Subject);
            string body = ReplaceMessageTemplateTokens(customer, localizedMessageTemplate.Body);
            MailAddress from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            MailAddress to = new MailAddress(customer.Email, customer.FullName);
            QueuedEmail queuedEmail = InsertQueuedEmail(5, from, to, string.Empty, string.Empty, subject, body, DateTime.Now, 0, null);
            return queuedEmail.QueuedEmailID;
        }

        /// <summary>
        /// Sends "email a friend" message
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="LanguageID">Message language identifier</param>
        /// <param name="product">Product instance</param>
        /// <param name="FriendsEmail">Friend's email</param>
        /// <param name="PersonalMessage">Personal message</param>
        /// <returns>Queued email identifier</returns>
        public static int SendEmailAFriendMessage(Customer customer, int LanguageID, Product product, string FriendsEmail, string PersonalMessage)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");
            if (product == null)
                throw new ArgumentNullException("product");

            string TemplateName = "Service.EmailAFriend";
            LocalizedMessageTemplate localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(TemplateName, LanguageID);
            if (localizedMessageTemplate == null)
                return 0;
                //throw new NopException(string.Format("Message template ({0}-{1}) couldn't be loaded", TemplateName, LanguageID));

            NameValueCollection additinalKeys = new NameValueCollection();
            additinalKeys.Add("EmailAFriend.PersonalMessage", PersonalMessage);
            string subject = ReplaceMessageTemplateTokens(customer, product, localizedMessageTemplate.Subject, additinalKeys);
            string body = ReplaceMessageTemplateTokens(customer, product, localizedMessageTemplate.Body, additinalKeys);
            MailAddress from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            MailAddress to = new MailAddress(FriendsEmail);
            QueuedEmail queuedEmail = InsertQueuedEmail(5, from, to, string.Empty, string.Empty, subject, body, DateTime.Now, 0, null);
            return queuedEmail.QueuedEmailID;
        }

        /// <summary>
        /// Sends a forum subscription message to a customer
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="forumTopic">Forum Topic</param>
        /// <param name="forum">Forum</param>
        /// <param name="LanguageID">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public static int SendNewForumTopicMessage(Customer customer, ForumTopic forumTopic, Forum forum, int LanguageID)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            string TemplateName = "Forums.NewForumTopic";
            LocalizedMessageTemplate localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(TemplateName, LanguageID);
            if (localizedMessageTemplate == null)
                return 0;
                //throw new NopException(string.Format("Message template ({0}-{1}) couldn't be loaded", TemplateName, LanguageID));

            string subject = ReplaceMessageTemplateTokens(customer, forumTopic, forum, localizedMessageTemplate.Subject);
            string body = ReplaceMessageTemplateTokens(customer, forumTopic, forum, localizedMessageTemplate.Body);
            MailAddress from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            MailAddress to = new MailAddress(customer.Email, customer.FullName);
            QueuedEmail queuedEmail = InsertQueuedEmail(5, from, to, string.Empty, string.Empty, subject, body, DateTime.Now, 0, null);
            return queuedEmail.QueuedEmailID;
        }

        /// <summary>
        /// Sends a forum subscription message to a customer
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="forumTopic">Forum Topic</param>
        /// <param name="forum">Forum</param>
        /// <param name="LanguageID">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public static int SendNewForumPostMessage(Customer customer, ForumTopic forumTopic, Forum forum, int LanguageID)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            string TemplateName = "Forums.NewForumPost";
            LocalizedMessageTemplate localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(TemplateName, LanguageID);
            if (localizedMessageTemplate == null)
                return 0;
                //throw new NopException(string.Format("Message template ({0}-{1}) couldn't be loaded", TemplateName, LanguageID));

            string subject = ReplaceMessageTemplateTokens(customer, forumTopic, forum, localizedMessageTemplate.Subject);
            string body = ReplaceMessageTemplateTokens(customer, forumTopic, forum, localizedMessageTemplate.Body);
            MailAddress from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            MailAddress to = new MailAddress(customer.Email, customer.FullName);
            QueuedEmail queuedEmail = InsertQueuedEmail(5, from, to, string.Empty, string.Empty, subject, body, DateTime.Now, 0, null);
            return queuedEmail.QueuedEmailID;
        }

        /// <summary>
        /// Sends a news comment notification message to a store owner
        /// </summary>
        /// <param name="newsComment">News comment</param>
        /// <param name="LanguageID">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public static int SendNewsCommentNotificationMessage(NewsComment newsComment, int LanguageID)
        {
            if (newsComment == null)
                throw new ArgumentNullException("newsComment");

            string TemplateName = "News.NewsComment";
            LocalizedMessageTemplate localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(TemplateName, LanguageID);
            if (localizedMessageTemplate == null)
                return 0;
                //throw new NopException(string.Format("Message template ({0}-{1}) couldn't be loaded", TemplateName, LanguageID));

            string subject = ReplaceMessageTemplateTokens(newsComment, localizedMessageTemplate.Subject);
            string body = ReplaceMessageTemplateTokens(newsComment, localizedMessageTemplate.Body);
            MailAddress from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            MailAddress to = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            QueuedEmail queuedEmail = InsertQueuedEmail(5, from, to, string.Empty, string.Empty, subject, body, DateTime.Now, 0, null);
            return queuedEmail.QueuedEmailID;
        }

        /// <summary>
        /// Sends a blog comment notification message to a store owner
        /// </summary>
        /// <param name="blogComment">Blog comment</param>
        /// <param name="LanguageID">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public static int SendBlogCommentNotificationMessage(BlogComment blogComment, int LanguageID)
        {
            if (blogComment == null)
                throw new ArgumentNullException("blogComment");

            string TemplateName = "Blog.BlogComment";
            LocalizedMessageTemplate localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(TemplateName, LanguageID);
            if (localizedMessageTemplate == null)
                return 0;
                //throw new NopException(string.Format("Message template ({0}-{1}) couldn't be loaded", TemplateName, LanguageID));

            string subject = ReplaceMessageTemplateTokens(blogComment, localizedMessageTemplate.Subject);
            string body = ReplaceMessageTemplateTokens(blogComment, localizedMessageTemplate.Body);
            MailAddress from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            MailAddress to = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            QueuedEmail queuedEmail = InsertQueuedEmail(5, from, to, string.Empty, string.Empty, subject, body, DateTime.Now, 0, null);
            return queuedEmail.QueuedEmailID;
        }

        /// <summary>
        /// Sends a product review notification message to a store owner
        /// </summary>
        /// <param name="productReview">Product review</param>
        /// <param name="LanguageID">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public static int SendProductReviewNotificationMessage(ProductReview productReview, int LanguageID)
        {
            if (productReview == null)
                throw new ArgumentNullException("productReview");

            string TemplateName = "Product.ProductReview";
            LocalizedMessageTemplate localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(TemplateName, LanguageID);
            if (localizedMessageTemplate == null)
                return 0;
                //throw new NopException(string.Format("Message template ({0}-{1}) couldn't be loaded", TemplateName, LanguageID));

            string subject = ReplaceMessageTemplateTokens(productReview, localizedMessageTemplate.Subject);
            string body = ReplaceMessageTemplateTokens(productReview, localizedMessageTemplate.Body);
            MailAddress from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            MailAddress to = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            QueuedEmail queuedEmail = InsertQueuedEmail(5, from, to, string.Empty, string.Empty, subject, body, DateTime.Now, 0, null);
            return queuedEmail.QueuedEmailID;
        }

        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="Subject">Subject</param>
        /// <param name="Body">Body</param>
        /// <param name="From">From</param>
        /// <param name="To">To</param>
        public static void SendEmail(string Subject, string Body, string From, string To)
        {
            SendEmail(Subject, Body, new MailAddress(From), new MailAddress(To), new List<String>(), new List<String>());
        }

        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="Subject">Subject</param>
        /// <param name="Body">Body</param>
        /// <param name="From">From</param>
        /// <param name="To">To</param>
        public static void SendEmail(string Subject, string Body, MailAddress From, MailAddress To)
        {
            SendEmail(Subject, Body, From, To, new List<String>(), new List<String>());
        }

        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="Subject">Subject</param>
        /// <param name="Body">Body</param>
        /// <param name="From">From</param>
        /// <param name="To">To</param>
        /// <param name="bcc">Bcc</param>
        /// <param name="cc">Cc</param>
        public static void SendEmail(string Subject, string Body, MailAddress From, MailAddress To, List<string> bcc, List<string> cc)
        {
            MailMessage message = new MailMessage();
            message.From = From;
            message.To.Add(To);
            if (null != bcc)
                foreach (string address in bcc)
                {
                    if (!String.IsNullOrEmpty(address))
                        message.Bcc.Add(address);
                }
            if (null != cc)
                foreach (string address in cc)
                {
                    if (!String.IsNullOrEmpty(address))
                        message.CC.Add(address);
                }
            message.Subject = Subject;
            message.Body = Body;
            message.IsBodyHtml = true;

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.UseDefaultCredentials = AdminEmailUseDefaultCredentials;
            smtpClient.Host = AdminEmailHost;
            smtpClient.Port = AdminEmailPort;
            smtpClient.EnableSsl = AdminEmailEnableSsl;
            if (AdminEmailUseDefaultCredentials)
                smtpClient.Credentials = CredentialCache.DefaultNetworkCredentials;
            else
                smtpClient.Credentials = new NetworkCredential(AdminEmailUser, AdminEmailPassword);
            smtpClient.Send(message);
        }

        /// <summary>
        /// Gets list of allowed (supported) message tokens
        /// </summary>
        /// <returns></returns>
        public static string[] GetListOfAllowedTokens()
        {
            List<string> allowedTokens = new List<string>();
            allowedTokens.Add("%Store.Name%");
            allowedTokens.Add("%Store.URL%");
            allowedTokens.Add("%Store.Email%");
            allowedTokens.Add("%Order.OrderNumber%");
            allowedTokens.Add("%Order.CustomerFullName%");
            allowedTokens.Add("%Order.CustomerEmail%");
            allowedTokens.Add("%Order.BillingFirstName%");
            allowedTokens.Add("%Order.BillingLastName%");
            allowedTokens.Add("%Order.BillingPhoneNumber%");
            allowedTokens.Add("%Order.BillingEmail%");
            allowedTokens.Add("%Order.BillingFaxNumber%");
            allowedTokens.Add("%Order.BillingCompany%");
            allowedTokens.Add("%Order.BillingAddress1%");
            allowedTokens.Add("%Order.BillingAddress2%");
            allowedTokens.Add("%Order.BillingCity%");
            allowedTokens.Add("%Order.BillingStateProvince%");
            allowedTokens.Add("%Order.BillingZipPostalCode%");
            allowedTokens.Add("%Order.BillingCountry%");
            allowedTokens.Add("%Order.ShippingMethod%");
            allowedTokens.Add("%Order.ShippingFirstName%");
            allowedTokens.Add("%Order.ShippingLastName%");
            allowedTokens.Add("%Order.ShippingPhoneNumber%");
            allowedTokens.Add("%Order.ShippingEmail%");
            allowedTokens.Add("%Order.ShippingFaxNumber%");
            allowedTokens.Add("%Order.ShippingCompany%");
            allowedTokens.Add("%Order.ShippingAddress1%");
            allowedTokens.Add("%Order.ShippingAddress2%");
            allowedTokens.Add("%Order.ShippingCity%");
            allowedTokens.Add("%Order.ShippingStateProvince%");
            allowedTokens.Add("%Order.ShippingZipPostalCode%");
            allowedTokens.Add("%Order.ShippingCountry%");
            allowedTokens.Add("%Order.Product(s)%");
            allowedTokens.Add("%Order.CreatedOn%");
            allowedTokens.Add("%Order.OrderURLForCustomer%");
            allowedTokens.Add("%Customer.Email%");
            allowedTokens.Add("%Customer.PasswordRecoveryURL%");
            allowedTokens.Add("%Customer.AccountActivationURL%");
            allowedTokens.Add("%Customer.FullName%");
            allowedTokens.Add("%Product.Name%");
            allowedTokens.Add("%Product.ShortDescription%");
            allowedTokens.Add("%Product.ProductURLForCustomer%");
            allowedTokens.Add("%ProductVariant.FullProductName%");
            allowedTokens.Add("%ProductVariant.StockQuantity%");
            allowedTokens.Add("%NewsComment.NewsTitle%");
            allowedTokens.Add("%BlogComment.BlogPostTitle%");
            allowedTokens.Add("%ProductReview.ProductName%");
            return allowedTokens.ToArray();
        }

        /// <summary>
        /// Gets list of allowed (supported) message tokens for campaigns
        /// </summary>
        /// <returns></returns>
        public static string[] GetListOfCampaignAllowedTokens()
        {
            List<string> allowedTokens = new List<string>();
            allowedTokens.Add("%Store.Name%");
            allowedTokens.Add("%Store.URL%");
            allowedTokens.Add("%Store.Email%");
            allowedTokens.Add("%Customer.Email%");
            allowedTokens.Add("%Customer.FullName%");
            return allowedTokens.ToArray();
        }

        /// <summary>
        /// Replaces a message template tokens
        /// </summary>
        /// <param name="order">Order instance</param>
        /// <param name="Template">Template</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>New template</returns>
        public static string ReplaceMessageTemplateTokens(Order order, string Template, int LanguageID)
        {
            NameValueCollection tokens = new NameValueCollection();
            tokens.Add("Store.Name", SettingManager.StoreName);
            tokens.Add("Store.URL", SettingManager.StoreURL);
            tokens.Add("Store.Email", AdminEmailAddress);

            tokens.Add("Order.OrderNumber", order.OrderID.ToString());

            //tokens.Add("Order.CustomerFullName", order.Customer.FullName);
            //tokens.Add("Order.CustomerEmail", order.Customer.Email);

            tokens.Add("Order.CustomerFullName", HttpUtility.HtmlEncode(order.BillingFullName));
            tokens.Add("Order.CustomerEmail", HttpUtility.HtmlEncode(order.BillingEmail));


            tokens.Add("Order.BillingFirstName", HttpUtility.HtmlEncode(order.BillingFirstName));
            tokens.Add("Order.BillingLastName", HttpUtility.HtmlEncode(order.BillingLastName));
            tokens.Add("Order.BillingPhoneNumber", HttpUtility.HtmlEncode(order.BillingPhoneNumber));
            tokens.Add("Order.BillingEmail", HttpUtility.HtmlEncode(order.BillingEmail.ToString()));
            tokens.Add("Order.BillingFaxNumber", HttpUtility.HtmlEncode(order.BillingFaxNumber));
            tokens.Add("Order.BillingCompany", HttpUtility.HtmlEncode(order.BillingCompany));
            tokens.Add("Order.BillingAddress1", HttpUtility.HtmlEncode(order.BillingAddress1));
            tokens.Add("Order.BillingAddress2", HttpUtility.HtmlEncode(order.BillingAddress2));
            tokens.Add("Order.BillingCity", HttpUtility.HtmlEncode(order.BillingCity));
            tokens.Add("Order.BillingStateProvince", HttpUtility.HtmlEncode(order.BillingStateProvince));
            tokens.Add("Order.BillingZipPostalCode", HttpUtility.HtmlEncode(order.BillingZipPostalCode));
            tokens.Add("Order.BillingCountry", HttpUtility.HtmlEncode(order.BillingCountry));

            tokens.Add("Order.ShippingMethod", HttpUtility.HtmlEncode(order.ShippingMethod));

            tokens.Add("Order.ShippingFirstName", HttpUtility.HtmlEncode(order.ShippingFirstName));
            tokens.Add("Order.ShippingLastName", HttpUtility.HtmlEncode(order.ShippingLastName));
            tokens.Add("Order.ShippingPhoneNumber", HttpUtility.HtmlEncode(order.ShippingPhoneNumber));
            tokens.Add("Order.ShippingEmail", HttpUtility.HtmlEncode(order.ShippingEmail.ToString()));
            tokens.Add("Order.ShippingFaxNumber", HttpUtility.HtmlEncode(order.ShippingFaxNumber));
            tokens.Add("Order.ShippingCompany", HttpUtility.HtmlEncode(order.ShippingCompany));
            tokens.Add("Order.ShippingAddress1", HttpUtility.HtmlEncode(order.ShippingAddress1));
            tokens.Add("Order.ShippingAddress2", HttpUtility.HtmlEncode(order.ShippingAddress2));
            tokens.Add("Order.ShippingCity", HttpUtility.HtmlEncode(order.ShippingCity));
            tokens.Add("Order.ShippingStateProvince", HttpUtility.HtmlEncode(order.ShippingStateProvince));
            tokens.Add("Order.ShippingZipPostalCode", HttpUtility.HtmlEncode(order.ShippingZipPostalCode));
            tokens.Add("Order.ShippingCountry", HttpUtility.HtmlEncode(order.ShippingCountry));

            tokens.Add("Order.Product(s)", ProductListToHtmlTable(order, LanguageID));

            Language language = LanguageManager.GetLanguageByID(LanguageID);
            //UNDONE use time zone
            //1. Add new token for store owner
            //2. Convert the date and time according to time zone
            if (language != null && !String.IsNullOrEmpty(language.LanguageCulture))
            {
                tokens.Add("Order.CreatedOn", order.CreatedOn.ToString("D", new CultureInfo(language.LanguageCulture)));
            }
            else
            {
                tokens.Add("Order.CreatedOn", order.CreatedOn.ToString("D"));
            }
            //TODO add "Order.OrderTotal" token
            //tokens.Add("Order.OrderTotal", String.Format("{0} ({1})", order.OrderTotalInCustomerCurrency.ToString("N", new CultureInfo("en-us")), order.CustomerCurrencyCode));
            tokens.Add("Order.OrderURLForCustomer", string.Format("{0}OrderDetails.aspx?OrderID={1}", SettingManager.StoreURL, order.OrderID));

            foreach (string token in tokens.Keys)
                Template = Template.Replace(string.Format(@"%{0}%", token), tokens[token]);

            return Template;
        }

        /// <summary>
        /// Replaces a message template tokens
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="Template">Template</param>
        /// <returns>New template</returns>
        public static string ReplaceMessageTemplateTokens(Customer customer, string Template)
        {
            NameValueCollection tokens = new NameValueCollection();
            tokens.Add("Store.Name", SettingManager.StoreName);
            tokens.Add("Store.URL", SettingManager.StoreURL);
            tokens.Add("Store.Email", AdminEmailAddress);

            tokens.Add("Customer.Email", HttpUtility.HtmlEncode(customer.Email));
            tokens.Add("Customer.FullName", HttpUtility.HtmlEncode(customer.FullName));

            string passwordRecoveryURL = string.Empty;
            passwordRecoveryURL = string.Format("{0}PasswordRecovery.aspx?PRT={1}&Email={2}", SettingManager.StoreURL, customer.PasswordRecoveryToken, customer.Email);
            tokens.Add("Customer.PasswordRecoveryURL", passwordRecoveryURL);
            
            string accountActivationURL = string.Empty;
            accountActivationURL = string.Format("{0}AccountActivation.aspx?ACT={1}&Email={2}", SettingManager.StoreURL, customer.AccountActivationToken, customer.Email);
            tokens.Add("Customer.AccountActivationURL", accountActivationURL);
            
            foreach (string token in tokens.Keys)
                Template = Template.Replace(string.Format(@"%{0}%", token), tokens[token]);

            return Template;
        }

        /// <summary>
        /// Replaces a message template tokens
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="product">Product instance</param>
        /// <param name="Template">Template</param>
        /// <param name="AdditinalKeys">Additinal keys</param>
        /// <returns>New template</returns>
        public static string ReplaceMessageTemplateTokens(Customer customer, Product product,
            string Template, NameValueCollection AdditinalKeys)
        {
            NameValueCollection tokens = new NameValueCollection();
            tokens.Add("Store.Name", SettingManager.StoreName);
            tokens.Add("Store.URL", SettingManager.StoreURL);
            tokens.Add("Store.Email", AdminEmailAddress);

            tokens.Add("Customer.Email", HttpUtility.HtmlEncode(customer.Email));
            tokens.Add("Customer.FullName", HttpUtility.HtmlEncode(customer.FullName));

            tokens.Add("Product.Name", HttpUtility.HtmlEncode(product.Name));
            tokens.Add("Product.ShortDescription", product.ShortDescription);
            tokens.Add("Product.ProductURLForCustomer", SEOHelper.GetProductURL(product));

            foreach (string token in tokens.Keys)
                Template = Template.Replace(string.Format(@"%{0}%", token), tokens[token]);

            foreach (string token in AdditinalKeys.Keys)
                Template = Template.Replace(string.Format(@"%{0}%", token), AdditinalKeys[token]);

            return Template;
        }

        /// <summary>
        /// Replaces a message template tokens
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="forumTopic">Forum Topic</param>
        /// <param name="forum">Forum</param>
        /// <param name="Template">Template</param>
        /// <returns>New template</returns>
        public static string ReplaceMessageTemplateTokens(Customer customer, 
            ForumTopic forumTopic, Forum forum, 
            string Template)
        {
            NameValueCollection tokens = new NameValueCollection();
            tokens.Add("Store.Name", SettingManager.StoreName);
            tokens.Add("Store.URL", SettingManager.StoreURL);
            tokens.Add("Store.Email", AdminEmailAddress);

            tokens.Add("Customer.Email", HttpUtility.HtmlEncode(customer.Email));
            tokens.Add("Customer.FullName", HttpUtility.HtmlEncode(customer.FullName));

            tokens.Add("Forums.TopicURL", SEOHelper.GetForumTopicURL(forumTopic.ForumTopicID));
            tokens.Add("Forums.TopicName", HttpUtility.HtmlEncode(forumTopic.Subject));
            tokens.Add("Forums.ForumURL", SEOHelper.GetForumURL(forum));
            tokens.Add("Forums.ForumName", HttpUtility.HtmlEncode(forum.Name));

            foreach (string token in tokens.Keys)
                Template = Template.Replace(string.Format(@"%{0}%", token), tokens[token]);

            return Template;
        }

        /// <summary>
        /// Replaces a message template tokens
        /// </summary>
        /// <param name="productVariant">Product variant</param>
        /// <param name="Template">Template</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>New template</returns>
        public static string ReplaceMessageTemplateTokens(ProductVariant productVariant, string Template, int LanguageID)
        {
            NameValueCollection tokens = new NameValueCollection();
            tokens.Add("Store.Name", SettingManager.StoreName);
            tokens.Add("Store.URL", SettingManager.StoreURL);
            tokens.Add("Store.Email", AdminEmailAddress);

            tokens.Add("ProductVariant.ID", productVariant.ProductVariantID.ToString());
            tokens.Add("ProductVariant.FullProductName", HttpUtility.HtmlEncode(productVariant.FullProductName));
            tokens.Add("ProductVariant.StockQuantity", productVariant.StockQuantity.ToString());

            foreach (string token in tokens.Keys)
                Template = Template.Replace(string.Format(@"%{0}%", token), tokens[token]);

            return Template;
        }

        /// <summary>
        /// Replaces a message template tokens
        /// </summary>
        /// <param name="newsComment">News comment</param>
        /// <param name="Template">Template</param>
        /// <returns>New template</returns>
        public static string ReplaceMessageTemplateTokens(NewsComment newsComment, string Template)
        {
            NameValueCollection tokens = new NameValueCollection();
            tokens.Add("Store.Name", SettingManager.StoreName);
            tokens.Add("Store.URL", SettingManager.StoreURL);
            tokens.Add("Store.Email", AdminEmailAddress);

            tokens.Add("NewsComment.NewsTitle", HttpUtility.HtmlEncode(newsComment.News.Title));

            foreach (string token in tokens.Keys)
                Template = Template.Replace(string.Format(@"%{0}%", token), tokens[token]);

            return Template;
        }

        /// <summary>
        /// Replaces a message template tokens
        /// </summary>
        /// <param name="blogComment">Blog comment</param>
        /// <param name="Template">Template</param>
        /// <returns>New template</returns>
        public static string ReplaceMessageTemplateTokens(BlogComment blogComment, string Template)
        {
            NameValueCollection tokens = new NameValueCollection();
            tokens.Add("Store.Name", SettingManager.StoreName);
            tokens.Add("Store.URL", SettingManager.StoreURL);
            tokens.Add("Store.Email", AdminEmailAddress);

            tokens.Add("BlogComment.BlogPostTitle", HttpUtility.HtmlEncode(blogComment.BlogPost.BlogPostTitle));

            foreach (string token in tokens.Keys)
                Template = Template.Replace(string.Format(@"%{0}%", token), tokens[token]);

            return Template;
        }

        /// <summary>
        /// Replaces a message template tokens
        /// </summary>
        /// <param name="productReview">Product review</param>
        /// <param name="Template">Template</param>
        /// <returns>New template</returns>
        public static string ReplaceMessageTemplateTokens(ProductReview productReview, string Template)
        {
            NameValueCollection tokens = new NameValueCollection();
            tokens.Add("Store.Name", SettingManager.StoreName);
            tokens.Add("Store.URL", SettingManager.StoreURL);
            tokens.Add("Store.Email", AdminEmailAddress);

            tokens.Add("ProductReview.ProductName", HttpUtility.HtmlEncode(productReview.Product.Name));
            
            foreach (string token in tokens.Keys)
                Template = Template.Replace(string.Format(@"%{0}%", token), tokens[token]);

            return Template;
        }

        /// <summary>
        /// Formats the contact us form text
        /// </summary>
        /// <param name="Text">Text</param>
        /// <returns>Formatted text</returns>
        public static string FormatContactUsFormText(string Text)
        {
            if (String.IsNullOrEmpty(Text))
                return string.Empty;

            Text = HtmlHelper.FormatText(Text, false, true, false, false, false, false);
            return Text;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets an admin email address
        /// </summary>
        public static string AdminEmailAddress
        {
            get
            {
                return SettingManager.GetSettingValue("Email.AdminEmailAddress");
            }
            set
            {
                SettingManager.SetParam("Email.AdminEmailAddress", value.Trim());
            }
        }

        /// <summary>
        /// Gets or sets an admin email display name
        /// </summary>
        public static string AdminEmailDisplayName
        {
            get
            {
                return SettingManager.GetSettingValue("Email.AdminEmailDisplayName");
            }
            set
            {
                SettingManager.SetParam("Email.AdminEmailDisplayName", value.Trim());
            }
        }

        /// <summary>
        /// Gets or sets an admin email host
        /// </summary>
        public static string AdminEmailHost
        {
            get
            {
                return SettingManager.GetSettingValue("Email.AdminEmailHost");
            }
            set
            {
                SettingManager.SetParam("Email.AdminEmailHost", value.Trim());
            }
        }

        /// <summary>
        /// Gets or sets an admin email port
        /// </summary>
        public static int AdminEmailPort
        {
            get
            {
                return SettingManager.GetSettingValueInteger("Email.AdminEmailPort");
            }
            set
            {
                SettingManager.SetParam("Email.AdminEmailPort", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets an admin email user name
        /// </summary>
        public static string AdminEmailUser
        {
            get
            {
                return SettingManager.GetSettingValue("Email.AdminEmailUser");
            }
            set
            {
                SettingManager.SetParam("Email.AdminEmailUser", value.Trim());
            }
        }

        /// <summary>
        /// Gets or sets an admin email password
        /// </summary>
        public static string AdminEmailPassword
        {
            get
            {
                return SettingManager.GetSettingValue("Email.AdminEmailPassword");
            }
            set
            {
                SettingManager.SetParam("Email.AdminEmailPassword", value);
            }
        }

        /// <summary>
        /// Gets or sets a value that controls whether the default system credentials of the application are sent with requests.
        /// </summary>
        public static bool AdminEmailUseDefaultCredentials
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("Email.AdminEmailUseDefaultCredentials");
            }
            set
            {
                SettingManager.SetParam("Email.AdminEmailUseDefaultCredentials", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value that controls whether the SmtpClient uses Secure Sockets Layer (SSL) to encrypt the connection
        /// </summary>
        public static bool AdminEmailEnableSsl
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("Email.AdminEmailEnableSsl");
            }
            set
            {
                SettingManager.SetParam("Email.AdminEmailEnableSsl", value.ToString());
            }
        }

        #endregion
    }
}