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
using System.Data;

namespace NopSolutions.NopCommerce.DataAccess.Orders
{
    /// <summary>
    /// Acts as a base class for deriving custom order provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/OrderProvider")]
    public abstract partial class DBOrderProvider : BaseDBProvider
    {
        #region Methods
        /// <summary>
        /// Gets an order
        /// </summary>
        /// <param name="OrderID">The order identifier</param>
        /// <returns>Order</returns>
        public abstract DBOrder GetOrderByID(int OrderID);

        /// <summary>
        /// Search orders
        /// </summary>
        /// <param name="StartTime">Order start time; null to load all orders</param>
        /// <param name="EndTime">Order end time; null to load all orders</param>
        /// <param name="CustomerEmail">Customer email</param>
        /// <param name="OrderStatusID">Order status identifier; null to load all orders</param>
        /// <param name="PaymentStatusID">Order payment status identifier; null to load all orders</param>
        /// <param name="ShippingStatusID">Order shipping status identifier; null to load all orders</param>
        /// <returns>Order collection</returns>
        public abstract DBOrderCollection SearchOrders(DateTime? StartTime, DateTime? EndTime, string CustomerEmail, int? OrderStatusID, int? PaymentStatusID, int? ShippingStatusID);

        /// <summary>
        /// Get order product variant sales report
        /// </summary>
        /// <param name="StartTime">Order start time; null to load all</param>
        /// <param name="EndTime">Order end time; null to load all</param>
        /// <param name="OrderStatusID">Order status identifier; null to load all orders</param>
        /// <param name="PaymentStatusID">Order payment status identifier; null to load all orders</param>
        /// <returns>Result</returns>
        public abstract IDataReader OrderProductVariantReport(DateTime? StartTime, DateTime? EndTime, int? OrderStatusID, int? PaymentStatusID);

        /// <summary>
        /// Get the bests sellers report
        /// </summary>
        /// <param name="LastDays">Last number of days</param>
        /// <param name="RecordsToReturn">Number of products to return</param>
        /// <param name="OrderBy">1 - order by total count, 2 - Order by total amount</param>
        /// <returns>Result</returns>
        public abstract List<DBBestSellersReportLine> BestSellersReport(int LastDays, int RecordsToReturn, int OrderBy);

        /// <summary>
        /// Get order average report
        /// </summary>
        /// <param name="OrderStatusID">Order status identifier</param>
        /// <returns>Result</returns>
        public abstract DBOrderAverageReportLine OrderAverageReport(int OrderStatusID);

        /// <summary>
        /// Gets all orders by customer identifier
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <returns>Order collection</returns>
        public abstract DBOrderCollection GetOrdersByCustomerID(int CustomerID);

        /// <summary>
        /// Gets an order by authorization transaction identifier
        /// </summary>
        /// <param name="AuthorizationTransactionID">Authorization transaction identifier</param>
        /// <param name="PaymentMethodID">Payment method identifier</param>
        /// <returns>Order</returns>
        public abstract DBOrder GetOrderByAuthorizationTransactionIDAndPaymentMethodID(string AuthorizationTransactionID, int PaymentMethodID);

        /// <summary>
        /// Gets all orders by affiliate identifier
        /// </summary>
        /// <param name="AffiliateID">Affiliate identifier</param>
        /// <returns>Order collection</returns>
        public abstract DBOrderCollection GetOrdersByAffiliateID(int AffiliateID);

        /// <summary>
        /// Inserts an order
        /// </summary>
        /// <param name="OrderGUID">The order identifier</param>
        /// <param name="CustomerID">The customer identifier</param>
        /// <param name="CustomerLanguageID">The customer language identifier</param>
        /// <param name="CustomerTaxDisplayTypeID">The customer tax display type identifier</param>
        /// <param name="OrderSubtotalInclTax">The order subtotal (incl tax)</param>
        /// <param name="OrderSubtotalExclTax">The order subtotal (excl tax)</param>
        /// <param name="OrderShippingInclTax">The order shipping (incl tax)</param>
        /// <param name="OrderShippingExclTax">The order shipping (excl tax)</param>
        /// <param name="PaymentMethodAdditionalFeeInclTax">The payment method additional fee (incl tax)</param>
        /// <param name="PaymentMethodAdditionalFeeExclTax">The payment method additional fee (excl tax)</param>
        /// <param name="OrderTax">The order tax</param>
        /// <param name="OrderTotal">The order total</param>
        /// <param name="OrderDiscount">The order discount</param>
        /// <param name="OrderSubtotalInclTaxInCustomerCurrency">The order subtotal incl tax (customer currency)</param>
        /// <param name="OrderSubtotalExclTaxInCustomerCurrency">The order subtotal excl tax (customer currency)</param>
        /// <param name="OrderShippingInclTaxInCustomerCurrency">The order shipping incl tax (customer currency)</param>
        /// <param name="OrderShippingExclTaxInCustomerCurrency">The order shipping excl tax (customer currency)</param>
        /// <param name="PaymentMethodAdditionalFeeInclTaxInCustomerCurrency">The payment method additional fee incl tax (customer currency)</param>
        /// <param name="PaymentMethodAdditionalFeeExclTaxInCustomerCurrency">The payment method additional fee excl tax (customer currency)</param>
        /// <param name="OrderTaxInCustomerCurrency">The order tax (customer currency)</param>
        /// <param name="OrderTotalInCustomerCurrency">The order total (customer currency)</param>
        /// <param name="CustomerCurrencyCode">The customer currency code</param>
        /// <param name="OrderWeight">The order weight</param>
        /// <param name="AffiliateID">The affiliate identifier</param>
        /// <param name="OrderStatusID">The order status identifier</param>
        /// <param name="AllowStoringCreditCardNumber">The value indicating whether storing of credit card number is allowed</param>
        /// <param name="CardType">The card type</param>
        /// <param name="CardName">The card name</param>
        /// <param name="CardNumber">The card number</param>
        /// <param name="MaskedCreditCardNumber">The masked credit card number</param>
        /// <param name="CardCVV2">The card CVV2</param>
        /// <param name="CardExpirationMonth">The card expiration month</param>
        /// <param name="CardExpirationYear">The card expiration year</param>
        /// <param name="PaymentMethodID">The payment method identifier</param>
        /// <param name="PaymentMethodName">The payment method name</param>
        /// <param name="AuthorizationTransactionID">The authorization transaction ID</param>
        /// <param name="AuthorizationTransactionCode">The authorization transaction code</param>
        /// <param name="AuthorizationTransactionResult">The authorization transaction result</param>
        /// <param name="CaptureTransactionID">The capture transaction ID</param>
        /// <param name="CaptureTransactionResult">The capture transaction result</param>
        /// <param name="PurchaseOrderNumber">The purchase order number</param>
        /// <param name="PaymentStatusID">The payment status identifier</param>
        /// <param name="BillingFirstName">The billing first name</param>
        /// <param name="BillingLastName">The billing last name</param>
        /// <param name="BillingPhoneNumber">he billing phone number</param>
        /// <param name="BillingEmail">The billing email</param>
        /// <param name="BillingFaxNumber">The billing fax number</param>
        /// <param name="BillingCompany">The billing company</param>
        /// <param name="BillingAddress1">The billing address 1</param>
        /// <param name="BillingAddress2">The billing address 2</param>
        /// <param name="BillingCity">The billing city</param>
        /// <param name="BillingStateProvince">The billing state/province</param>
        /// <param name="BillingStateProvinceID">The billing state/province identifier</param>
        /// <param name="BillingZipPostalCode">The billing zip/postal code</param>
        /// <param name="BillingCountry">The billing country</param>
        /// <param name="BillingCountryID">The billing country identifier</param>
        /// <param name="ShippingStatusID">The shipping status identifier</param>
        /// <param name="ShippingFirstName">The shipping first name</param>
        /// <param name="ShippingLastName">The shipping last name</param>
        /// <param name="ShippingPhoneNumber">The shipping phone number</param>
        /// <param name="ShippingEmail">The shipping email</param>
        /// <param name="ShippingFaxNumber">The shipping fax number</param>
        /// <param name="ShippingCompany">The shipping  company</param>
        /// <param name="ShippingAddress1">The shipping address 1</param>
        /// <param name="ShippingAddress2">The shipping address 2</param>
        /// <param name="ShippingCity">The shipping city</param>
        /// <param name="ShippingStateProvince">The shipping state/province</param>
        /// <param name="ShippingStateProvinceID">The shipping state/province identifier</param>
        /// <param name="ShippingZipPostalCode">The shipping zip/postal code</param>
        /// <param name="ShippingCountry">The shipping country</param>
        /// <param name="ShippingCountryID">The shipping country identifier</param>
        /// <param name="ShippingMethod">The shipping method</param>
        /// <param name="ShippingRateComputationMethodID">The shipping rate computation method identifier</param>
        /// <param name="ShippedDate">The shipped date and time</param>
        /// <param name="Deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="CreatedOn">The date and time of order creation</param>
        /// <returns>Order</returns>
        public abstract DBOrder InsertOrder(Guid OrderGUID, int CustomerID, int CustomerLanguageID,
            int CustomerTaxDisplayTypeID, decimal OrderSubtotalInclTax, decimal OrderSubtotalExclTax,
            decimal OrderShippingInclTax, decimal OrderShippingExclTax, 
            decimal PaymentMethodAdditionalFeeInclTax, decimal PaymentMethodAdditionalFeeExclTax,
            decimal OrderTax, decimal OrderTotal, decimal OrderDiscount,
            decimal OrderSubtotalInclTaxInCustomerCurrency, decimal OrderSubtotalExclTaxInCustomerCurrency,
            decimal OrderShippingInclTaxInCustomerCurrency, decimal OrderShippingExclTaxInCustomerCurrency,
            decimal PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, decimal PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
            decimal OrderTaxInCustomerCurrency, decimal OrderTotalInCustomerCurrency, 
            string CustomerCurrencyCode, decimal OrderWeight,
            int AffiliateID, int OrderStatusID, bool AllowStoringCreditCardNumber, string CardType, 
            string CardName, string CardNumber, string MaskedCreditCardNumber, string CardCVV2,
            string CardExpirationMonth, string CardExpirationYear, int PaymentMethodID,
            string PaymentMethodName, string AuthorizationTransactionID, string AuthorizationTransactionCode,
            string AuthorizationTransactionResult, string CaptureTransactionID, string CaptureTransactionResult,
            string PurchaseOrderNumber, int PaymentStatusID, string BillingFirstName, string BillingLastName,
            string BillingPhoneNumber, string BillingEmail, string BillingFaxNumber, string BillingCompany,
            string BillingAddress1, string BillingAddress2, string BillingCity, string BillingStateProvince,
            int BillingStateProvinceID, string BillingZipPostalCode, string BillingCountry,
            int BillingCountryID, int ShippingStatusID, string ShippingFirstName,
            string ShippingLastName, string ShippingPhoneNumber, string ShippingEmail,
            string ShippingFaxNumber, string ShippingCompany, string ShippingAddress1,
            string ShippingAddress2, string ShippingCity, string ShippingStateProvince,
            int ShippingStateProvinceID, string ShippingZipPostalCode,
            string ShippingCountry, int ShippingCountryID, string ShippingMethod, int ShippingRateComputationMethodID, DateTime? ShippedDate,
            bool Deleted, DateTime CreatedOn);

        /// <summary>
        /// Updates the order
        /// </summary>
        /// <param name="OrderID">The order identifier</param>
        /// <param name="OrderGUID">The order identifier</param>
        /// <param name="CustomerID">The customer identifier</param>
        /// <param name="CustomerLanguageID">The customer language identifier</param>
        /// <param name="CustomerTaxDisplayTypeID">The customer tax display type identifier</param>
        /// <param name="OrderSubtotalInclTax">The order subtotal (incl tax)</param>
        /// <param name="OrderSubtotalExclTax">The order subtotal (excl tax)</param>
        /// <param name="OrderShippingInclTax">The order shipping (incl tax)</param>
        /// <param name="OrderShippingExclTax">The order shipping (excl tax)</param>
        /// <param name="PaymentMethodAdditionalFeeInclTax">The payment method additional fee (incl tax)</param>
        /// <param name="PaymentMethodAdditionalFeeExclTax">The payment method additional fee (excl tax)</param>
        /// <param name="OrderTax">The order tax</param>
        /// <param name="OrderTotal">The order total</param>
        /// <param name="OrderDiscount">The order discount</param>
        /// <param name="OrderSubtotalInclTaxInCustomerCurrency">The order subtotal incl tax (customer currency)</param>
        /// <param name="OrderSubtotalExclTaxInCustomerCurrency">The order subtotal excl tax (customer currency)</param>
        /// <param name="OrderShippingInclTaxInCustomerCurrency">The order shipping incl tax (customer currency)</param>
        /// <param name="OrderShippingExclTaxInCustomerCurrency">The order shipping excl tax (customer currency)</param>
        /// <param name="PaymentMethodAdditionalFeeInclTaxInCustomerCurrency">The payment method additional fee incl tax (customer currency)</param>
        /// <param name="PaymentMethodAdditionalFeeExclTaxInCustomerCurrency">The payment method additional fee excl tax (customer currency)</param>
        /// <param name="OrderTaxInCustomerCurrency">The order tax (customer currency)</param>
        /// <param name="OrderTotalInCustomerCurrency">The order total (customer currency)</param>
        /// <param name="CustomerCurrencyCode">The customer currency code</param>
        /// <param name="OrderWeight">The order weight</param>
        /// <param name="AffiliateID">The affiliate identifier</param>
        /// <param name="OrderStatusID">The order status identifier</param>
        /// <param name="AllowStoringCreditCardNumber">The value indicating whether storing of credit card number is allowed</param>
        /// <param name="CardType">The card type</param>
        /// <param name="CardName">The card name</param>
        /// <param name="CardNumber">The card number</param>
        /// <param name="MaskedCreditCardNumber">The masked credit card number</param>
        /// <param name="CardCVV2">The card CVV2</param>
        /// <param name="CardExpirationMonth">The card expiration month</param>
        /// <param name="CardExpirationYear">The card expiration year</param>
        /// <param name="PaymentMethodID">The payment method identifier</param>
        /// <param name="PaymentMethodName">The payment method name</param>
        /// <param name="AuthorizationTransactionID">The authorization transaction ID</param>
        /// <param name="AuthorizationTransactionCode">The authorization transaction code</param>
        /// <param name="AuthorizationTransactionResult">The authorization transaction result</param>
        /// <param name="CaptureTransactionID">The capture transaction ID</param>
        /// <param name="CaptureTransactionResult">The capture transaction result</param>
        /// <param name="PurchaseOrderNumber">The purchase order number</param>
        /// <param name="PaymentStatusID">The payment status identifier</param>
        /// <param name="BillingFirstName">The billing first name</param>
        /// <param name="BillingLastName">The billing last name</param>
        /// <param name="BillingPhoneNumber">he billing phone number</param>
        /// <param name="BillingEmail">The billing email</param>
        /// <param name="BillingFaxNumber">The billing fax number</param>
        /// <param name="BillingCompany">The billing company</param>
        /// <param name="BillingAddress1">The billing address 1</param>
        /// <param name="BillingAddress2">The billing address 2</param>
        /// <param name="BillingCity">The billing city</param>
        /// <param name="BillingStateProvince">The billing state/province</param>
        /// <param name="BillingStateProvinceID">The billing state/province identifier</param>
        /// <param name="BillingZipPostalCode">The billing zip/postal code</param>
        /// <param name="BillingCountry">The billing country</param>
        /// <param name="BillingCountryID">The billing country identifier</param>
        /// <param name="ShippingStatusID">The shipping status identifier</param>
        /// <param name="ShippingFirstName">The shipping first name</param>
        /// <param name="ShippingLastName">The shipping last name</param>
        /// <param name="ShippingPhoneNumber">The shipping phone number</param>
        /// <param name="ShippingEmail">The shipping email</param>
        /// <param name="ShippingFaxNumber">The shipping fax number</param>
        /// <param name="ShippingCompany">The shipping  company</param>
        /// <param name="ShippingAddress1">The shipping address 1</param>
        /// <param name="ShippingAddress2">The shipping address 2</param>
        /// <param name="ShippingCity">The shipping city</param>
        /// <param name="ShippingStateProvince">The shipping state/province</param>
        /// <param name="ShippingStateProvinceID">The shipping state/province identifier</param>
        /// <param name="ShippingZipPostalCode">The shipping zip/postal code</param>
        /// <param name="ShippingCountry">The shipping country</param>
        /// <param name="ShippingCountryID">The shipping country identifier</param>
        /// <param name="ShippingMethod">The shipping method</param>
        /// <param name="ShippingRateComputationMethodID">The shipping rate computation method identifier</param>
        /// <param name="ShippedDate">The shipped date and time</param>
        /// <param name="Deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="CreatedOn">The date and time of order creation</param>
        /// <returns>Order</returns>
        public abstract DBOrder UpdateOrder(int OrderID, Guid OrderGUID, int CustomerID, int CustomerLanguageID,
            int CustomerTaxDisplayTypeID, decimal OrderSubtotalInclTax, decimal OrderSubtotalExclTax,
            decimal OrderShippingInclTax, decimal OrderShippingExclTax,
            decimal PaymentMethodAdditionalFeeInclTax, decimal PaymentMethodAdditionalFeeExclTax, 
            decimal OrderTax, decimal OrderTotal, decimal OrderDiscount,
            decimal OrderSubtotalInclTaxInCustomerCurrency, decimal OrderSubtotalExclTaxInCustomerCurrency,
            decimal OrderShippingInclTaxInCustomerCurrency, decimal OrderShippingExclTaxInCustomerCurrency,
            decimal PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, decimal PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
            decimal OrderTaxInCustomerCurrency, decimal OrderTotalInCustomerCurrency,
            string CustomerCurrencyCode, decimal OrderWeight,
            int AffiliateID, int OrderStatusID, bool AllowStoringCreditCardNumber, string CardType,
            string CardName, string CardNumber, string MaskedCreditCardNumber, string CardCVV2,
            string CardExpirationMonth, string CardExpirationYear, int PaymentMethodID,
            string PaymentMethodName, string AuthorizationTransactionID, string AuthorizationTransactionCode,
            string AuthorizationTransactionResult, string CaptureTransactionID, string CaptureTransactionResult,
            string PurchaseOrderNumber, int PaymentStatusID, string BillingFirstName, string BillingLastName,
            string BillingPhoneNumber, string BillingEmail, string BillingFaxNumber, string BillingCompany,
            string BillingAddress1, string BillingAddress2, string BillingCity, string BillingStateProvince,
            int BillingStateProvinceID, string BillingZipPostalCode, string BillingCountry,
            int BillingCountryID, int ShippingStatusID, string ShippingFirstName,
            string ShippingLastName, string ShippingPhoneNumber, string ShippingEmail,
            string ShippingFaxNumber, string ShippingCompany, string ShippingAddress1,
            string ShippingAddress2, string ShippingCity, string ShippingStateProvince,
            int ShippingStateProvinceID, string ShippingZipPostalCode,
            string ShippingCountry, int ShippingCountryID, string ShippingMethod, int ShippingRateComputationMethodID, DateTime? ShippedDate,
            bool Deleted, DateTime CreatedOn);

        /// <summary>
        /// Gets an order note
        /// </summary>
        /// <param name="OrderNoteID">Order note identifier</param>
        /// <returns>Order note</returns>
        public abstract DBOrderNote GetOrderNoteByID(int OrderNoteID);

        /// <summary>
        /// Gets an order notes by order identifier
        /// </summary>
        /// <param name="OrderID">Order identifier</param>
        /// <returns>Order note collection</returns>
        public abstract DBOrderNoteCollection GetOrderNoteByOrderID(int OrderID);

        /// <summary>
        /// Deletes an order note
        /// </summary>
        /// <param name="OrderNoteID">Order note identifier</param>
        public abstract void DeleteOrderNote(int OrderNoteID);

        /// <summary>
        /// Inserts an order note
        /// </summary>
        /// <param name="OrderID">The order identifier</param>
        /// <param name="Note">The note</param>
        /// <param name="CreatedOn">The date and time of order note creation</param>
        /// <returns>Order note</returns>
        public abstract DBOrderNote InsertOrderNote(int OrderID, string Note, DateTime CreatedOn);

        /// <summary>
        /// Updates the order note
        /// </summary>
        /// <param name="OrderNoteID">The order note identifier</param>
        /// <param name="OrderID">The order identifier</param>
        /// <param name="Note">The note</param>
        /// <param name="CreatedOn">The date and time of order note creation</param>
        /// <returns>Order note</returns>
        public abstract DBOrderNote UpdateOrderNote(int OrderNoteID, int OrderID, string Note, DateTime CreatedOn);

        /// <summary>
        /// Gets an order product variant
        /// </summary>
        /// <param name="OrderProductVariantID">Order product variant identifier</param>
        /// <returns>Order product variant</returns>
        public abstract DBOrderProductVariant GetOrderProductVariantByID(int OrderProductVariantID);

        /// <summary>
        /// Gets an order product variants by the order identifier
        /// </summary>
        /// <param name="OrderID">The order identifier</param>
        /// <returns>Order product variant collection</returns>
        public abstract DBOrderProductVariantCollection GetOrderProductVariantsByOrderID(int OrderID);

        /// <summary>
        /// Inserts a order product variant
        /// </summary>
        /// <param name="OrderID">The order identifier</param>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <param name="UnitPriceInclTax">The unit price in primary store currency (incl tax)</param>
        /// <param name="UnitPriceExclTax">The unit price in primary store currency (excl tax)</param>
        /// <param name="PriceInclTax">The price in primary store currency (incl tax)</param>
        /// <param name="PriceExclTax">The price in primary store currency (excl tax)</param>
        /// <param name="UnitPriceInclTaxInCustomerCurrency">The unit price in primary store currency (incl tax)</param>
        /// <param name="UnitPriceExclTaxInCustomerCurrency">The unit price in customer currency (excl tax)</param>
        /// <param name="PriceInclTaxInCustomerCurrency">The price in primary store currency (incl tax)</param>
        /// <param name="PriceExclTaxInCustomerCurrency">The price in customer currency (excl tax)</param>
        /// <param name="AttributeDescription">The attribute description</param>
        /// <param name="Quantity">The quantity</param>
        /// <param name="DiscountAmountInclTax">The discount amount (incl tax)</param>
        /// <param name="DiscountAmountExclTax">The discount amount (excl tax)</param>
        /// <param name="DownloadCount">The download count</param>
        /// <returns>Order product variant</returns>
        public abstract DBOrderProductVariant InsertOrderProductVariant(int OrderID, int ProductVariantID,
            decimal UnitPriceInclTax, decimal UnitPriceExclTax, decimal PriceInclTax, decimal PriceExclTax,
            decimal UnitPriceInclTaxInCustomerCurrency, decimal UnitPriceExclTaxInCustomerCurrency,
            decimal PriceInclTaxInCustomerCurrency, decimal PriceExclTaxInCustomerCurrency,
            string AttributeDescription, int Quantity,
            decimal DiscountAmountInclTax, decimal DiscountAmountExclTax, int DownloadCount);

        /// <summary>
        /// Updates the order product variant
        /// </summary>
        /// <param name="OrderProductVariantID">The order product variant identifier</param>
        /// <param name="OrderID">The order identifier</param>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <param name="UnitPriceInclTax">The unit price in primary store currency (incl tax)</param>
        /// <param name="UnitPriceExclTax">The unit price in primary store currency (excl tax)</param>
        /// <param name="PriceInclTax">The price in primary store currency (incl tax)</param>
        /// <param name="PriceExclTax">The price in primary store currency (excl tax)</param>
        /// <param name="UnitPriceInclTaxInCustomerCurrency">The unit price in primary store currency (incl tax)</param>
        /// <param name="UnitPriceExclTaxInCustomerCurrency">The unit price in customer currency (excl tax)</param>
        /// <param name="PriceInclTaxInCustomerCurrency">The price in primary store currency (incl tax)</param>
        /// <param name="PriceExclTaxInCustomerCurrency">The price in customer currency (excl tax)</param>
        /// <param name="AttributeDescription">The attribute description</param>
        /// <param name="Quantity">The quantity</param>
        /// <param name="DiscountAmountInclTax">The discount amount (incl tax)</param>
        /// <param name="DiscountAmountExclTax">The discount amount (excl tax)</param>
        /// <param name="DownloadCount">The download count</param>
        /// <returns>Order product variant</returns>
        public abstract DBOrderProductVariant UpdateOrderProductVariant(int OrderProductVariantID, 
            int OrderID, int ProductVariantID,
            decimal UnitPriceInclTax, decimal UnitPriceExclTax, decimal PriceInclTax, decimal PriceExclTax,
            decimal UnitPriceInclTaxInCustomerCurrency, decimal UnitPriceExclTaxInCustomerCurrency,
            decimal PriceInclTaxInCustomerCurrency, decimal PriceExclTaxInCustomerCurrency,
            string AttributeDescription, int Quantity,
            decimal DiscountAmountInclTax, decimal DiscountAmountExclTax, int DownloadCount);

        /// <summary>
        /// Gets an order status by ID
        /// </summary>
        /// <param name="OrderStatusID">Order status identifier</param>
        /// <returns>Order status</returns>
        public abstract DBOrderStatus GetOrderStatusByID(int OrderStatusID);

        /// <summary>
        /// Gets all order statuses
        /// </summary>
        /// <returns>Order status collection</returns>
        public abstract DBOrderStatusCollection GetAllOrderStatuses();

        /// <summary>
        /// Gets an order report
        /// </summary>
        /// <param name="OrderStatusID">Order status identifier; null to load all orders</param>
        /// <param name="PaymentStatusID">Order payment status identifier; null to load all orders</param>
        /// <param name="ShippingStatusID">Order shipping status identifier; null to load all orders</param>
        /// <returns>IDataReader</returns>
        public abstract IDataReader GetOrderReport(int? OrderStatusID, int? PaymentStatusID, int? ShippingStatusID);

        #endregion
    }
}
