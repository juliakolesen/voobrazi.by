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
using System.Configuration.Provider;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace NopSolutions.NopCommerce.DataAccess.Orders
{
    /// <summary>
    /// Order provider for SQL Server
    /// </summary>
    public partial class SQLOrderProvider : DBOrderProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBOrder GetOrderFromReader(IDataReader dataReader)
        {
            DBOrder order = new DBOrder();
            order.OrderID = NopSqlDataHelper.GetInt(dataReader, "OrderID");
            order.OrderGUID = NopSqlDataHelper.GetGuid(dataReader, "OrderGUID");
            order.CustomerID = NopSqlDataHelper.GetInt(dataReader, "CustomerID");
            order.CustomerLanguageID = NopSqlDataHelper.GetInt(dataReader, "CustomerLanguageID");
            order.CustomerTaxDisplayTypeID = NopSqlDataHelper.GetInt(dataReader, "CustomerTaxDisplayTypeID");
            order.OrderSubtotalInclTax = NopSqlDataHelper.GetDecimal(dataReader, "OrderSubtotalInclTax");
            order.OrderSubtotalExclTax = NopSqlDataHelper.GetDecimal(dataReader, "OrderSubtotalExclTax");
            order.OrderShippingInclTax = NopSqlDataHelper.GetDecimal(dataReader, "OrderShippingInclTax");
            order.OrderShippingExclTax = NopSqlDataHelper.GetDecimal(dataReader, "OrderShippingExclTax");
            order.PaymentMethodAdditionalFeeInclTax = NopSqlDataHelper.GetDecimal(dataReader, "PaymentMethodAdditionalFeeInclTax");
            order.PaymentMethodAdditionalFeeExclTax = NopSqlDataHelper.GetDecimal(dataReader, "PaymentMethodAdditionalFeeExclTax");
            order.OrderTax = NopSqlDataHelper.GetDecimal(dataReader, "OrderTax");
            order.OrderTotal = NopSqlDataHelper.GetDecimal(dataReader, "OrderTotal");
            order.OrderDiscount = NopSqlDataHelper.GetDecimal(dataReader, "OrderDiscount");
            order.OrderSubtotalInclTaxInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "OrderSubtotalInclTaxInCustomerCurrency");
            order.OrderSubtotalExclTaxInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "OrderSubtotalExclTaxInCustomerCurrency");
            order.OrderShippingInclTaxInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "OrderShippingInclTaxInCustomerCurrency");
            order.OrderShippingExclTaxInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "OrderShippingExclTaxInCustomerCurrency");
            order.PaymentMethodAdditionalFeeInclTaxInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "PaymentMethodAdditionalFeeInclTaxInCustomerCurrency");
            order.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "PaymentMethodAdditionalFeeExclTaxInCustomerCurrency");
            order.OrderTaxInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "OrderTaxInCustomerCurrency");
            order.OrderTotalInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "OrderTotalInCustomerCurrency");
            order.CustomerCurrencyCode = NopSqlDataHelper.GetString(dataReader, "CustomerCurrencyCode");
            order.OrderWeight = NopSqlDataHelper.GetDecimal(dataReader, "OrderWeight");
            order.AffiliateID = NopSqlDataHelper.GetInt(dataReader, "AffiliateID");
            order.OrderStatusID = NopSqlDataHelper.GetInt(dataReader, "OrderStatusID");
            order.AllowStoringCreditCardNumber = NopSqlDataHelper.GetBoolean(dataReader, "AllowStoringCreditCardNumber");
            order.CardType = NopSqlDataHelper.GetString(dataReader, "CardType");
            order.CardName = NopSqlDataHelper.GetString(dataReader, "CardName");
            order.CardNumber = NopSqlDataHelper.GetString(dataReader, "CardNumber");
            order.MaskedCreditCardNumber = NopSqlDataHelper.GetString(dataReader, "MaskedCreditCardNumber");
            order.CardCVV2 = NopSqlDataHelper.GetString(dataReader, "CardCVV2");
            order.CardExpirationMonth = NopSqlDataHelper.GetString(dataReader, "CardExpirationMonth");
            order.CardExpirationYear = NopSqlDataHelper.GetString(dataReader, "CardExpirationYear");
            order.PaymentMethodID = NopSqlDataHelper.GetInt(dataReader, "PaymentMethodID");
            order.PaymentMethodName = NopSqlDataHelper.GetString(dataReader, "PaymentMethodName");
            order.AuthorizationTransactionID = NopSqlDataHelper.GetString(dataReader, "AuthorizationTransactionID");
            order.AuthorizationTransactionCode = NopSqlDataHelper.GetString(dataReader, "AuthorizationTransactionCode");
            order.AuthorizationTransactionResult = NopSqlDataHelper.GetString(dataReader, "AuthorizationTransactionResult");
            order.CaptureTransactionID = NopSqlDataHelper.GetString(dataReader, "CaptureTransactionID");
            order.CaptureTransactionResult = NopSqlDataHelper.GetString(dataReader, "CaptureTransactionResult");
            order.PurchaseOrderNumber = NopSqlDataHelper.GetString(dataReader, "PurchaseOrderNumber");
            order.PaymentStatusID = NopSqlDataHelper.GetInt(dataReader, "PaymentStatusID");
            order.BillingFirstName = NopSqlDataHelper.GetString(dataReader, "BillingFirstName");
            order.BillingLastName = NopSqlDataHelper.GetString(dataReader, "BillingLastName");
            order.BillingPhoneNumber = NopSqlDataHelper.GetString(dataReader, "BillingPhoneNumber");
            order.BillingEmail = NopSqlDataHelper.GetString(dataReader, "BillingEmail");
            order.BillingFaxNumber = NopSqlDataHelper.GetString(dataReader, "BillingFaxNumber");
            order.BillingCompany = NopSqlDataHelper.GetString(dataReader, "BillingCompany");
            order.BillingAddress1 = NopSqlDataHelper.GetString(dataReader, "BillingAddress1");
            order.BillingAddress2 = NopSqlDataHelper.GetString(dataReader, "BillingAddress2");
            order.BillingCity = NopSqlDataHelper.GetString(dataReader, "BillingCity");
            order.BillingStateProvince = NopSqlDataHelper.GetString(dataReader, "BillingStateProvince");
            order.BillingStateProvinceID = NopSqlDataHelper.GetInt(dataReader, "BillingStateProvinceID");
            order.BillingZipPostalCode = NopSqlDataHelper.GetString(dataReader, "BillingZipPostalCode");
            order.BillingCountry = NopSqlDataHelper.GetString(dataReader, "BillingCountry");
            order.BillingCountryID = NopSqlDataHelper.GetInt(dataReader, "BillingCountryID");
            order.ShippingStatusID = NopSqlDataHelper.GetInt(dataReader, "ShippingStatusID");
            order.ShippingFirstName = NopSqlDataHelper.GetString(dataReader, "ShippingFirstName");
            order.ShippingLastName = NopSqlDataHelper.GetString(dataReader, "ShippingLastName");
            order.ShippingPhoneNumber = NopSqlDataHelper.GetString(dataReader, "ShippingPhoneNumber");
            order.ShippingEmail = NopSqlDataHelper.GetString(dataReader, "ShippingEmail");
            order.ShippingFaxNumber = NopSqlDataHelper.GetString(dataReader, "ShippingFaxNumber");
            order.ShippingCompany = NopSqlDataHelper.GetString(dataReader, "ShippingCompany");
            order.ShippingAddress1 = NopSqlDataHelper.GetString(dataReader, "ShippingAddress1");
            order.ShippingAddress2 = NopSqlDataHelper.GetString(dataReader, "ShippingAddress2");
            order.ShippingCity = NopSqlDataHelper.GetString(dataReader, "ShippingCity");
            order.ShippingStateProvince = NopSqlDataHelper.GetString(dataReader, "ShippingStateProvince");
            order.ShippingStateProvinceID = NopSqlDataHelper.GetInt(dataReader, "ShippingStateProvinceID");
            order.ShippingZipPostalCode = NopSqlDataHelper.GetString(dataReader, "ShippingZipPostalCode");
            order.ShippingCountry = NopSqlDataHelper.GetString(dataReader, "ShippingCountry");
            order.ShippingCountryID = NopSqlDataHelper.GetInt(dataReader, "ShippingCountryID");
            order.ShippingMethod = NopSqlDataHelper.GetString(dataReader, "ShippingMethod");
            order.ShippingRateComputationMethodID = NopSqlDataHelper.GetInt(dataReader, "ShippingRateComputationMethodID");
            order.ShippedDate = NopSqlDataHelper.GetNullableUtcDateTime(dataReader, "ShippedDate");
            order.Deleted = NopSqlDataHelper.GetBoolean(dataReader, "Deleted");
            order.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            return order;
        }

        private DBOrderNote GetOrderNoteFromReader(IDataReader dataReader)
        {
            DBOrderNote orderNote = new DBOrderNote();
            orderNote.OrderNoteID = NopSqlDataHelper.GetInt(dataReader, "OrderNoteID");
            orderNote.OrderID = NopSqlDataHelper.GetInt(dataReader, "OrderID");
            orderNote.Note = NopSqlDataHelper.GetString(dataReader, "Note");
            orderNote.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            return orderNote;
        }

        private DBOrderProductVariant GetOrderProductVariantFromReader(IDataReader dataReader)
        {
            DBOrderProductVariant orderProductVariant = new DBOrderProductVariant();
            orderProductVariant.OrderProductVariantID = NopSqlDataHelper.GetInt(dataReader, "OrderProductVariantID");
            orderProductVariant.OrderID = NopSqlDataHelper.GetInt(dataReader, "OrderID");
            orderProductVariant.ProductVariantID = NopSqlDataHelper.GetInt(dataReader, "ProductVariantID");
            orderProductVariant.UnitPriceInclTax = NopSqlDataHelper.GetDecimal(dataReader, "UnitPriceInclTax");
            orderProductVariant.UnitPriceExclTax = NopSqlDataHelper.GetDecimal(dataReader, "UnitPriceExclTax");
            orderProductVariant.PriceInclTax = NopSqlDataHelper.GetDecimal(dataReader, "PriceInclTax");
            orderProductVariant.PriceExclTax = NopSqlDataHelper.GetDecimal(dataReader, "PriceExclTax");
            orderProductVariant.UnitPriceInclTaxInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "UnitPriceInclTaxInCustomerCurrency");
            orderProductVariant.UnitPriceExclTaxInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "UnitPriceExclTaxInCustomerCurrency");
            orderProductVariant.PriceInclTaxInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "PriceInclTaxInCustomerCurrency");
            orderProductVariant.PriceExclTaxInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "PriceExclTaxInCustomerCurrency");
            orderProductVariant.AttributeDescription = NopSqlDataHelper.GetString(dataReader, "AttributeDescription");
            orderProductVariant.Quantity = NopSqlDataHelper.GetInt(dataReader, "Quantity");
            orderProductVariant.DiscountAmountInclTax = NopSqlDataHelper.GetDecimal(dataReader, "DiscountAmountInclTax");
            orderProductVariant.DiscountAmountExclTax = NopSqlDataHelper.GetDecimal(dataReader, "DiscountAmountExclTax");
            orderProductVariant.DownloadCount = NopSqlDataHelper.GetInt(dataReader, "DownloadCount");
            return orderProductVariant;
        }

        private DBOrderStatus GetOrderStatusFromReader(IDataReader dataReader)
        {
            DBOrderStatus orderStatus = new DBOrderStatus();
            orderStatus.OrderStatusID = NopSqlDataHelper.GetInt(dataReader, "OrderStatusID");
            orderStatus.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            return orderStatus;
        }

        private DBOrderAverageReportLine GetOrderAverageReportLineFromReader(IDataReader dataReader)
        {
            DBOrderAverageReportLine orderAverageReportLine = new DBOrderAverageReportLine();
            orderAverageReportLine.SumTodayOrders = NopSqlDataHelper.GetDecimal(dataReader, "SumTodayOrders");
            orderAverageReportLine.CountTodayOrders = NopSqlDataHelper.GetDecimal(dataReader, "CountTodayOrders");
            orderAverageReportLine.SumThisWeekOrders = NopSqlDataHelper.GetDecimal(dataReader, "SumThisWeekOrders");
            orderAverageReportLine.CountThisWeekOrders = NopSqlDataHelper.GetDecimal(dataReader, "CountThisWeekOrders");
            orderAverageReportLine.SumThisMonthOrders = NopSqlDataHelper.GetDecimal(dataReader, "SumThisMonthOrders");
            orderAverageReportLine.CountThisMonthOrders = NopSqlDataHelper.GetDecimal(dataReader, "CountThisMonthOrders");
            orderAverageReportLine.SumThisYearOrders = NopSqlDataHelper.GetDecimal(dataReader, "SumThisYearOrders");
            orderAverageReportLine.CountThisYearOrders = NopSqlDataHelper.GetDecimal(dataReader, "CountThisYearOrders");
            orderAverageReportLine.SumAllTimeOrders = NopSqlDataHelper.GetDecimal(dataReader, "SumAllTimeOrders");
            orderAverageReportLine.CountAllTimeOrders = NopSqlDataHelper.GetDecimal(dataReader, "CountAllTimeOrders");
            return orderAverageReportLine;
        }

        private DBBestSellersReportLine GetBestSellersReportLineFromReader(IDataReader dataReader)
        {
            DBBestSellersReportLine bestSellersReportLine = new DBBestSellersReportLine();
            bestSellersReportLine.ProductVariantID = NopSqlDataHelper.GetInt(dataReader, "ProductVariantID");
            bestSellersReportLine.SalesTotalCount = NopSqlDataHelper.GetDecimal(dataReader, "SalesTotalCount");
            bestSellersReportLine.SalesTotalAmount = NopSqlDataHelper.GetDecimal(dataReader, "SalesTotalAmount");
            return bestSellersReportLine;
        }
        
        #endregion

        #region Methods

        /// <summary>
        /// Initializes the provider with the property values specified in the application's configuration file. This method is not intended to be used directly from your code
        /// </summary>
        /// <param name="name">The name of the provider instance to initialize</param>
        /// <param name="config">A NameValueCollection that contains the names and values of configuration options for the provider.</param>
        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            base.Initialize(name, config);

            string connectionStringName = config["connectionStringName"];
            if (String.IsNullOrEmpty(connectionStringName))
                throw new ProviderException("Connection name not specified");
            this._sqlConnectionString = NopSqlDataHelper.GetConnectionString(connectionStringName);
            if ((this._sqlConnectionString == null) || (this._sqlConnectionString.Length < 1))
            {
                throw new ProviderException(string.Format("Connection string not found. {0}", connectionStringName));
            }
            config.Remove("connectionStringName");

            if (config.Count > 0)
            {
                string key = config.GetKey(0);
                if (!string.IsNullOrEmpty(key))
                {
                    throw new ProviderException(string.Format("Provider unrecognized attribute. {0}", new object[] { key }));
                }
            }
        }

        /// <summary>
        /// Gets an order
        /// </summary>
        /// <param name="OrderID">The order identifier</param>
        /// <returns>Order</returns>
        public override DBOrder GetOrderByID(int OrderID)
        {
            DBOrder order = null;
            if (OrderID == 0)
                return order;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "OrderID", DbType.Int32, OrderID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    order = GetOrderFromReader(dataReader);
                }
            }
            return order;
        }

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
        public override DBOrderCollection SearchOrders(DateTime? StartTime, DateTime? EndTime, string CustomerEmail, int? OrderStatusID, int? PaymentStatusID, int? ShippingStatusID)
        {
            DBOrderCollection orderCollection = new DBOrderCollection();

            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderSearch");
            if (StartTime.HasValue)
                db.AddInParameter(dbCommand, "StartTime", DbType.DateTime, StartTime.Value);
            else
                db.AddInParameter(dbCommand, "StartTime", DbType.DateTime, null);
            if (EndTime.HasValue)
                db.AddInParameter(dbCommand, "EndTime", DbType.DateTime, EndTime.Value);
            else
                db.AddInParameter(dbCommand, "EndTime", DbType.DateTime, null);
            db.AddInParameter(dbCommand, "CustomerEmail", DbType.String, CustomerEmail);
            if (OrderStatusID.HasValue)
                db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, OrderStatusID.Value);
            else
                db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, null);
            if (PaymentStatusID.HasValue)
                db.AddInParameter(dbCommand, "PaymentStatusID", DbType.Int32, PaymentStatusID.Value);
            else
                db.AddInParameter(dbCommand, "PaymentStatusID", DbType.Int32, null);

            if (ShippingStatusID.HasValue)
                db.AddInParameter(dbCommand, "ShippingStatusID", DbType.Int32, ShippingStatusID);
            else
                db.AddInParameter(dbCommand, "ShippingStatusID", DbType.Int32, null);

            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBOrder order = GetOrderFromReader(dataReader);
                    orderCollection.Add(order);
                }
            }
            return orderCollection;
        }

        /// <summary>
        /// Get order product variant sales report
        /// </summary>
        /// <param name="StartTime">Order start time; null to load all</param>
        /// <param name="EndTime">Order end time; null to load all</param>
        /// <param name="OrderStatusID">Order status identifier; null to load all orders</param>
        /// <param name="PaymentStatusID">Order payment status identifier; null to load all orders</param>
        /// <returns>Result</returns>
        public override IDataReader OrderProductVariantReport(DateTime? StartTime, DateTime? EndTime, int? OrderStatusID, int? PaymentStatusID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderProductVariantReport");
            if (StartTime.HasValue)
                db.AddInParameter(dbCommand, "StartTime", DbType.DateTime, StartTime.Value);
            else
                db.AddInParameter(dbCommand, "StartTime", DbType.DateTime, null);
            if (EndTime.HasValue)
                db.AddInParameter(dbCommand, "EndTime", DbType.DateTime, EndTime.Value);
            else
                db.AddInParameter(dbCommand, "EndTime", DbType.DateTime, null);
            if (OrderStatusID.HasValue)
                db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, OrderStatusID.Value);
            else
                db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, null);
            if (PaymentStatusID.HasValue)
                db.AddInParameter(dbCommand, "PaymentStatusID", DbType.Int32, PaymentStatusID.Value);
            else
                db.AddInParameter(dbCommand, "PaymentStatusID", DbType.Int32, null);
            return db.ExecuteReader(dbCommand);
        }

        /// <summary>
        /// Get the bests sellers report
        /// </summary>
        /// <param name="LastDays">Last number of days</param>
        /// <param name="RecordsToReturn">Number of products to return</param>
        /// <param name="OrderBy">1 - order by total count, 2 - Order by total amount</param>
        /// <returns>Result</returns>
        public override List<DBBestSellersReportLine> BestSellersReport(int LastDays, int RecordsToReturn, int OrderBy)
        {
            List<DBBestSellersReportLine> result = new List<DBBestSellersReportLine>();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SalesBestSellersReport");
            db.AddInParameter(dbCommand, "LastDays", DbType.Int32, LastDays);
            db.AddInParameter(dbCommand, "RecordsToReturn", DbType.Int32, RecordsToReturn);
            db.AddInParameter(dbCommand, "OrderBy", DbType.Int32, OrderBy);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBBestSellersReportLine bestSellersReportLine = GetBestSellersReportLineFromReader(dataReader);
                    result.Add(bestSellersReportLine);
                }
            }
            return result;
        }

        /// <summary>
        /// Get order average report
        /// </summary>
        /// <param name="OrderStatusID">Order status identifier</param>
        /// <returns>Result</returns>
        public override DBOrderAverageReportLine OrderAverageReport(int OrderStatusID)
        {
            DBOrderAverageReportLine orderAverageReportLine = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderAverageReport");
            db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, OrderStatusID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    orderAverageReportLine = GetOrderAverageReportLineFromReader(dataReader);
                }
            }
            return orderAverageReportLine;
        }

        /// <summary>
        /// Gets all orders by customer identifier
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <returns>Order collection</returns>
        public override DBOrderCollection GetOrdersByCustomerID(int CustomerID)
        {
            DBOrderCollection orderCollection = new DBOrderCollection();
            if (CustomerID == 0)
                return orderCollection;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderLoadByCustomerID");
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, CustomerID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBOrder order = GetOrderFromReader(dataReader);
                    orderCollection.Add(order);
                }
            }
            return orderCollection;
        }

        /// <summary>
        /// Gets an order by authorization transaction identifier
        /// </summary>
        /// <param name="AuthorizationTransactionID">Authorization transaction identifier</param>
        /// <param name="PaymentMethodID">Payment method identifier</param>
        /// <returns>Order</returns>
        public override DBOrder GetOrderByAuthorizationTransactionIDAndPaymentMethodID(string AuthorizationTransactionID, int PaymentMethodID)
        {
            DBOrder order = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderLoadByAuthorizationTransactionIDAndPaymentMethodID");
            db.AddInParameter(dbCommand, "AuthorizationTransactionID", DbType.String, AuthorizationTransactionID);
            db.AddInParameter(dbCommand, "PaymentMethodID", DbType.Int32, PaymentMethodID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    order = GetOrderFromReader(dataReader);
                }
            }
            return order;
        }

        /// <summary>
        /// Gets all orders by affiliate identifier
        /// </summary>
        /// <param name="AffiliateID">Affiliate identifier</param>
        /// <returns>Order collection</returns>
        public override DBOrderCollection GetOrdersByAffiliateID(int AffiliateID)
        {
            DBOrderCollection orderCollection = new DBOrderCollection();
            if (AffiliateID == 0)
                return orderCollection;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderLoadByAffiliateID");
            db.AddInParameter(dbCommand, "AffiliateID", DbType.Int32, AffiliateID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBOrder order = GetOrderFromReader(dataReader);
                    orderCollection.Add(order);
                }
            }
            return orderCollection;
        }

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
        public override DBOrder InsertOrder(Guid OrderGUID, int CustomerID, int CustomerLanguageID,
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
            bool Deleted, DateTime CreatedOn)
        {
            DBOrder order = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderInsert");
            db.AddOutParameter(dbCommand, "OrderID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "OrderGUID", DbType.Guid, OrderGUID);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, CustomerID);
            db.AddInParameter(dbCommand, "CustomerLanguageID", DbType.Int32, CustomerLanguageID);
            db.AddInParameter(dbCommand, "CustomerTaxDisplayTypeID", DbType.Int32, CustomerTaxDisplayTypeID);
            db.AddInParameter(dbCommand, "OrderSubtotalInclTax", DbType.Decimal, OrderSubtotalInclTax);
            db.AddInParameter(dbCommand, "OrderSubtotalExclTax", DbType.Decimal, OrderSubtotalExclTax);
            db.AddInParameter(dbCommand, "OrderShippingInclTax", DbType.Decimal, OrderShippingInclTax);
            db.AddInParameter(dbCommand, "OrderShippingExclTax", DbType.Decimal, OrderShippingExclTax);
            db.AddInParameter(dbCommand, "PaymentMethodAdditionalFeeInclTax", DbType.Decimal, PaymentMethodAdditionalFeeInclTax);
            db.AddInParameter(dbCommand, "PaymentMethodAdditionalFeeExclTax", DbType.Decimal, PaymentMethodAdditionalFeeExclTax);
            db.AddInParameter(dbCommand, "OrderTax", DbType.Decimal, OrderTax);
            db.AddInParameter(dbCommand, "OrderTotal", DbType.Decimal, OrderTotal);
            db.AddInParameter(dbCommand, "OrderDiscount", DbType.Decimal, OrderDiscount);
            db.AddInParameter(dbCommand, "OrderSubtotalInclTaxInCustomerCurrency", DbType.Decimal, OrderSubtotalInclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "OrderSubtotalExclTaxInCustomerCurrency", DbType.Decimal, OrderSubtotalExclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "OrderShippingInclTaxInCustomerCurrency", DbType.Decimal, OrderShippingInclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "OrderShippingExclTaxInCustomerCurrency", DbType.Decimal, OrderShippingExclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "PaymentMethodAdditionalFeeInclTaxInCustomerCurrency", DbType.Decimal, PaymentMethodAdditionalFeeInclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "PaymentMethodAdditionalFeeExclTaxInCustomerCurrency", DbType.Decimal, PaymentMethodAdditionalFeeExclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "OrderTaxInCustomerCurrency", DbType.Decimal, OrderTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "OrderTotalInCustomerCurrency", DbType.Decimal, OrderTotalInCustomerCurrency);
            db.AddInParameter(dbCommand, "CustomerCurrencyCode", DbType.String, CustomerCurrencyCode);
            db.AddInParameter(dbCommand, "OrderWeight", DbType.Decimal, OrderWeight);
            db.AddInParameter(dbCommand, "AffiliateID", DbType.Int32, AffiliateID);
            db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, OrderStatusID);
            db.AddInParameter(dbCommand, "AllowStoringCreditCardNumber", DbType.Boolean, AllowStoringCreditCardNumber);
            db.AddInParameter(dbCommand, "CardType", DbType.String, CardType);
            db.AddInParameter(dbCommand, "CardName", DbType.String, CardName);
            db.AddInParameter(dbCommand, "CardNumber", DbType.String, CardNumber);
            db.AddInParameter(dbCommand, "MaskedCreditCardNumber", DbType.String, MaskedCreditCardNumber);
            db.AddInParameter(dbCommand, "CardCVV2", DbType.String, CardCVV2);
            db.AddInParameter(dbCommand, "CardExpirationMonth", DbType.String, CardExpirationMonth);
            db.AddInParameter(dbCommand, "CardExpirationYear", DbType.String, CardExpirationYear);
            db.AddInParameter(dbCommand, "PaymentMethodID", DbType.Int32, PaymentMethodID);
            db.AddInParameter(dbCommand, "PaymentMethodName", DbType.String, PaymentMethodName);
            db.AddInParameter(dbCommand, "AuthorizationTransactionID", DbType.String, AuthorizationTransactionID);
            db.AddInParameter(dbCommand, "AuthorizationTransactionCode", DbType.String, AuthorizationTransactionCode);
            db.AddInParameter(dbCommand, "AuthorizationTransactionResult", DbType.String, AuthorizationTransactionResult);
            db.AddInParameter(dbCommand, "CaptureTransactionID", DbType.String, CaptureTransactionID);
            db.AddInParameter(dbCommand, "CaptureTransactionResult", DbType.String, CaptureTransactionResult);
            db.AddInParameter(dbCommand, "PurchaseOrderNumber", DbType.String, PurchaseOrderNumber);
            db.AddInParameter(dbCommand, "PaymentStatusID", DbType.Int32, PaymentStatusID);            
            db.AddInParameter(dbCommand, "BillingFirstName", DbType.String, BillingFirstName);
            db.AddInParameter(dbCommand, "BillingLastName", DbType.String, BillingLastName);
            db.AddInParameter(dbCommand, "BillingPhoneNumber", DbType.String, BillingPhoneNumber);
            db.AddInParameter(dbCommand, "BillingEmail", DbType.String, BillingEmail);
            db.AddInParameter(dbCommand, "BillingFaxNumber", DbType.String, BillingFaxNumber);
            db.AddInParameter(dbCommand, "BillingCompany", DbType.String, BillingCompany);
            db.AddInParameter(dbCommand, "BillingAddress1", DbType.String, BillingAddress1);
            db.AddInParameter(dbCommand, "BillingAddress2", DbType.String, BillingAddress2);
            db.AddInParameter(dbCommand, "BillingCity", DbType.String, BillingCity);
            db.AddInParameter(dbCommand, "BillingStateProvince", DbType.String, BillingStateProvince);
            db.AddInParameter(dbCommand, "BillingStateProvinceID", DbType.Int32, BillingStateProvinceID);
            db.AddInParameter(dbCommand, "BillingZipPostalCode", DbType.String, BillingZipPostalCode);
            db.AddInParameter(dbCommand, "BillingCountry", DbType.String, BillingCountry);
            db.AddInParameter(dbCommand, "BillingCountryID", DbType.Int32, BillingCountryID);
            db.AddInParameter(dbCommand, "ShippingStatusID", DbType.Int32, ShippingStatusID);
            db.AddInParameter(dbCommand, "ShippingFirstName", DbType.String, ShippingFirstName);
            db.AddInParameter(dbCommand, "ShippingLastName", DbType.String, ShippingLastName);
            db.AddInParameter(dbCommand, "ShippingPhoneNumber", DbType.String, ShippingPhoneNumber);
            db.AddInParameter(dbCommand, "ShippingEmail", DbType.String, ShippingEmail);
            db.AddInParameter(dbCommand, "ShippingFaxNumber", DbType.String, ShippingFaxNumber);
            db.AddInParameter(dbCommand, "ShippingCompany", DbType.String, ShippingCompany);
            db.AddInParameter(dbCommand, "ShippingAddress1", DbType.String, ShippingAddress1);
            db.AddInParameter(dbCommand, "ShippingAddress2", DbType.String, ShippingAddress2);
            db.AddInParameter(dbCommand, "ShippingCity", DbType.String, ShippingCity);
            db.AddInParameter(dbCommand, "ShippingStateProvince", DbType.String, ShippingStateProvince);
            db.AddInParameter(dbCommand, "ShippingStateProvinceID", DbType.Int32, ShippingStateProvinceID);
            db.AddInParameter(dbCommand, "ShippingZipPostalCode", DbType.String, ShippingZipPostalCode);
            db.AddInParameter(dbCommand, "ShippingCountry", DbType.String, ShippingCountry);
            db.AddInParameter(dbCommand, "ShippingCountryID", DbType.Int32, ShippingCountryID);
            db.AddInParameter(dbCommand, "ShippingMethod", DbType.String, ShippingMethod);
            db.AddInParameter(dbCommand, "ShippingRateComputationMethodID", DbType.Int32, ShippingRateComputationMethodID);
            if (ShippedDate.HasValue)
                db.AddInParameter(dbCommand, "ShippedDate", DbType.DateTime, ShippedDate.Value);
            else
                db.AddInParameter(dbCommand, "ShippedDate", DbType.DateTime, DBNull.Value);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, Deleted);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int OrderID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@OrderID"));
                order = GetOrderByID(OrderID);
            }
            return order;
        }

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
        public override DBOrder UpdateOrder(int OrderID, Guid OrderGUID, int CustomerID, int CustomerLanguageID,
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
            bool Deleted, DateTime CreatedOn)
        {
            DBOrder order = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderUpdate");
            db.AddInParameter(dbCommand, "OrderID", DbType.Int32, OrderID);
            db.AddInParameter(dbCommand, "OrderGUID", DbType.Guid, OrderGUID);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, CustomerID);
            db.AddInParameter(dbCommand, "CustomerLanguageID", DbType.Int32, CustomerLanguageID);
            db.AddInParameter(dbCommand, "CustomerTaxDisplayTypeID", DbType.Int32, CustomerTaxDisplayTypeID);
            db.AddInParameter(dbCommand, "OrderSubtotalInclTax", DbType.Decimal, OrderSubtotalInclTax);
            db.AddInParameter(dbCommand, "OrderSubtotalExclTax", DbType.Decimal, OrderSubtotalExclTax);
            db.AddInParameter(dbCommand, "OrderShippingInclTax", DbType.Decimal, OrderShippingInclTax);
            db.AddInParameter(dbCommand, "OrderShippingExclTax", DbType.Decimal, OrderShippingExclTax);
            db.AddInParameter(dbCommand, "PaymentMethodAdditionalFeeInclTax", DbType.Decimal, PaymentMethodAdditionalFeeInclTax);
            db.AddInParameter(dbCommand, "PaymentMethodAdditionalFeeExclTax", DbType.Decimal, PaymentMethodAdditionalFeeExclTax);
            db.AddInParameter(dbCommand, "OrderTax", DbType.Decimal, OrderTax);
            db.AddInParameter(dbCommand, "OrderTotal", DbType.Decimal, OrderTotal);
            db.AddInParameter(dbCommand, "OrderDiscount", DbType.Decimal, OrderDiscount);
            db.AddInParameter(dbCommand, "OrderSubtotalInclTaxInCustomerCurrency", DbType.Decimal, OrderSubtotalInclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "OrderSubtotalExclTaxInCustomerCurrency", DbType.Decimal, OrderSubtotalExclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "OrderShippingInclTaxInCustomerCurrency", DbType.Decimal, OrderShippingInclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "OrderShippingExclTaxInCustomerCurrency", DbType.Decimal, OrderShippingExclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "PaymentMethodAdditionalFeeInclTaxInCustomerCurrency", DbType.Decimal, PaymentMethodAdditionalFeeInclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "PaymentMethodAdditionalFeeExclTaxInCustomerCurrency", DbType.Decimal, PaymentMethodAdditionalFeeExclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "OrderTaxInCustomerCurrency", DbType.Decimal, OrderTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "OrderTotalInCustomerCurrency", DbType.Decimal, OrderTotalInCustomerCurrency);
            db.AddInParameter(dbCommand, "CustomerCurrencyCode", DbType.String, CustomerCurrencyCode);
            db.AddInParameter(dbCommand, "OrderWeight", DbType.Decimal, OrderWeight);
            db.AddInParameter(dbCommand, "AffiliateID", DbType.Int32, AffiliateID);
            db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, OrderStatusID);
            db.AddInParameter(dbCommand, "AllowStoringCreditCardNumber", DbType.Boolean, AllowStoringCreditCardNumber);
            db.AddInParameter(dbCommand, "CardType", DbType.String, CardType);
            db.AddInParameter(dbCommand, "CardName", DbType.String, CardName);
            db.AddInParameter(dbCommand, "CardNumber", DbType.String, CardNumber);
            db.AddInParameter(dbCommand, "MaskedCreditCardNumber", DbType.String, MaskedCreditCardNumber);
            db.AddInParameter(dbCommand, "CardCVV2", DbType.String, CardCVV2);
            db.AddInParameter(dbCommand, "CardExpirationMonth", DbType.String, CardExpirationMonth);
            db.AddInParameter(dbCommand, "CardExpirationYear", DbType.String, CardExpirationYear);
            db.AddInParameter(dbCommand, "PaymentMethodID", DbType.Int32, PaymentMethodID);
            db.AddInParameter(dbCommand, "PaymentMethodName", DbType.String, PaymentMethodName);
            db.AddInParameter(dbCommand, "AuthorizationTransactionID", DbType.String, AuthorizationTransactionID);
            db.AddInParameter(dbCommand, "AuthorizationTransactionCode", DbType.String, AuthorizationTransactionCode);
            db.AddInParameter(dbCommand, "AuthorizationTransactionResult", DbType.String, AuthorizationTransactionResult);
            db.AddInParameter(dbCommand, "CaptureTransactionID", DbType.String, CaptureTransactionID);
            db.AddInParameter(dbCommand, "CaptureTransactionResult", DbType.String, CaptureTransactionResult);
            db.AddInParameter(dbCommand, "PurchaseOrderNumber", DbType.String, PurchaseOrderNumber);
            db.AddInParameter(dbCommand, "PaymentStatusID", DbType.Int32, PaymentStatusID);
            db.AddInParameter(dbCommand, "BillingFirstName", DbType.String, BillingFirstName);
            db.AddInParameter(dbCommand, "BillingLastName", DbType.String, BillingLastName);
            db.AddInParameter(dbCommand, "BillingPhoneNumber", DbType.String, BillingPhoneNumber);
            db.AddInParameter(dbCommand, "BillingEmail", DbType.String, BillingEmail);
            db.AddInParameter(dbCommand, "BillingFaxNumber", DbType.String, BillingFaxNumber);
            db.AddInParameter(dbCommand, "BillingCompany", DbType.String, BillingCompany);
            db.AddInParameter(dbCommand, "BillingAddress1", DbType.String, BillingAddress1);
            db.AddInParameter(dbCommand, "BillingAddress2", DbType.String, BillingAddress2);
            db.AddInParameter(dbCommand, "BillingCity", DbType.String, BillingCity);
            db.AddInParameter(dbCommand, "BillingStateProvince", DbType.String, BillingStateProvince);
            db.AddInParameter(dbCommand, "BillingStateProvinceID", DbType.Int32, BillingStateProvinceID);
            db.AddInParameter(dbCommand, "BillingZipPostalCode", DbType.String, BillingZipPostalCode);
            db.AddInParameter(dbCommand, "BillingCountry", DbType.String, BillingCountry);
            db.AddInParameter(dbCommand, "BillingCountryID", DbType.Int32, BillingCountryID);
            db.AddInParameter(dbCommand, "ShippingStatusID", DbType.Int32, ShippingStatusID);
            db.AddInParameter(dbCommand, "ShippingFirstName", DbType.String, ShippingFirstName);
            db.AddInParameter(dbCommand, "ShippingLastName", DbType.String, ShippingLastName);
            db.AddInParameter(dbCommand, "ShippingPhoneNumber", DbType.String, ShippingPhoneNumber);
            db.AddInParameter(dbCommand, "ShippingEmail", DbType.String, ShippingEmail);
            db.AddInParameter(dbCommand, "ShippingFaxNumber", DbType.String, ShippingFaxNumber);
            db.AddInParameter(dbCommand, "ShippingCompany", DbType.String, ShippingCompany);
            db.AddInParameter(dbCommand, "ShippingAddress1", DbType.String, ShippingAddress1);
            db.AddInParameter(dbCommand, "ShippingAddress2", DbType.String, ShippingAddress2);
            db.AddInParameter(dbCommand, "ShippingCity", DbType.String, ShippingCity);
            db.AddInParameter(dbCommand, "ShippingStateProvince", DbType.String, ShippingStateProvince);
            db.AddInParameter(dbCommand, "ShippingStateProvinceID", DbType.Int32, ShippingStateProvinceID);
            db.AddInParameter(dbCommand, "ShippingZipPostalCode", DbType.String, ShippingZipPostalCode);
            db.AddInParameter(dbCommand, "ShippingCountry", DbType.String, ShippingCountry);
            db.AddInParameter(dbCommand, "ShippingCountryID", DbType.Int32, ShippingCountryID);
            db.AddInParameter(dbCommand, "ShippingMethod", DbType.String, ShippingMethod);
            db.AddInParameter(dbCommand, "ShippingRateComputationMethodID", DbType.Int32, ShippingRateComputationMethodID);
            if (ShippedDate.HasValue)
                db.AddInParameter(dbCommand, "ShippedDate", DbType.DateTime, ShippedDate.Value);
            else
                db.AddInParameter(dbCommand, "ShippedDate", DbType.DateTime, DBNull.Value);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, Deleted);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                order = GetOrderByID(OrderID);

            return order;
        }

        /// <summary>
        /// Gets an order note
        /// </summary>
        /// <param name="OrderNoteID">Order note identifier</param>
        /// <returns>Order note</returns>
        public override DBOrderNote GetOrderNoteByID(int OrderNoteID)
        {
            DBOrderNote orderNote = null;
            if (OrderNoteID == 0)
                return orderNote;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderNoteLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "OrderNoteID", DbType.Int32, OrderNoteID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    orderNote = GetOrderNoteFromReader(dataReader);
                }
            }
            return orderNote;
        }

        /// <summary>
        /// Gets an order notes by order identifier
        /// </summary>
        /// <param name="OrderID">Order identifier</param>
        /// <returns>Order note collection</returns>
        public override DBOrderNoteCollection GetOrderNoteByOrderID(int OrderID)
        {
            DBOrderNoteCollection orderNoteCollection = new DBOrderNoteCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderNoteLoadByOrderID");
            db.AddInParameter(dbCommand, "OrderID", DbType.Int32, OrderID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBOrderNote orderNote = GetOrderNoteFromReader(dataReader);
                    orderNoteCollection.Add(orderNote);
                }
            }

            return orderNoteCollection;
        }

        /// <summary>
        /// Deletes an order note
        /// </summary>
        /// <param name="OrderNoteID">Order note identifier</param>
        public override void DeleteOrderNote(int OrderNoteID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderNoteDelete");
            db.AddInParameter(dbCommand, "OrderNoteID", DbType.Int32, OrderNoteID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Inserts an order note
        /// </summary>
        /// <param name="OrderID">The order identifier</param>
        /// <param name="Note">The note</param>
        /// <param name="CreatedOn">The date and time of order note creation</param>
        /// <returns>Order note</returns>
        public override DBOrderNote InsertOrderNote(int OrderID, string Note, DateTime CreatedOn)
        {
            DBOrderNote orderNote = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderNoteInsert");
            db.AddOutParameter(dbCommand, "OrderNoteID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "OrderID", DbType.Int32, OrderID);
            db.AddInParameter(dbCommand, "Note", DbType.String, Note);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int OrderNoteID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@OrderNoteID"));
                orderNote = GetOrderNoteByID(OrderNoteID);
            }
            return orderNote;
        }

        /// <summary>
        /// Updates the order note
        /// </summary>
        /// <param name="OrderNoteID">The order note identifier</param>
        /// <param name="OrderID">The order identifier</param>
        /// <param name="Note">The note</param>
        /// <param name="CreatedOn">The date and time of order note creation</param>
        /// <returns>Order note</returns>
        public override DBOrderNote UpdateOrderNote(int OrderNoteID, int OrderID, string Note, DateTime CreatedOn)
        {
            DBOrderNote orderNote = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderNoteUpdate");
            db.AddInParameter(dbCommand, "OrderNoteID", DbType.Int32, OrderNoteID);
            db.AddInParameter(dbCommand, "OrderID", DbType.Int32, OrderID);
            db.AddInParameter(dbCommand, "Note", DbType.String, Note);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                orderNote = GetOrderNoteByID(OrderNoteID);

            return orderNote;
        }

        /// <summary>
        /// Gets an order product variant
        /// </summary>
        /// <param name="OrderProductVariantID">Order product variant identifier</param>
        /// <returns>Order product variant</returns>
        public override DBOrderProductVariant GetOrderProductVariantByID(int OrderProductVariantID)
        {
            DBOrderProductVariant orderProductVariant = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderProductVariantLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "OrderProductVariantID", DbType.Int32, OrderProductVariantID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    orderProductVariant = GetOrderProductVariantFromReader(dataReader);
                }
            }
            return orderProductVariant;
        }

        /// <summary>
        /// Gets an order product variants by the order identifier
        /// </summary>
        /// <param name="OrderID">The order identifier</param>
        /// <returns>Order product variant collection</returns>
        public override DBOrderProductVariantCollection GetOrderProductVariantsByOrderID(int OrderID)
        {
            DBOrderProductVariantCollection orderProductVariantCollection = new DBOrderProductVariantCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderProductVariantLoadByOrderID");
            db.AddInParameter(dbCommand, "OrderID", DbType.Int32, OrderID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBOrderProductVariant orderProductVariant = GetOrderProductVariantFromReader(dataReader);
                    orderProductVariantCollection.Add(orderProductVariant);
                }
            }

            return orderProductVariantCollection;
        }

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
        public override DBOrderProductVariant InsertOrderProductVariant(int OrderID, int ProductVariantID,
            decimal UnitPriceInclTax, decimal UnitPriceExclTax, decimal PriceInclTax, decimal PriceExclTax,
            decimal UnitPriceInclTaxInCustomerCurrency, decimal UnitPriceExclTaxInCustomerCurrency,
            decimal PriceInclTaxInCustomerCurrency, decimal PriceExclTaxInCustomerCurrency,
            string AttributeDescription, int Quantity,
            decimal DiscountAmountInclTax, decimal DiscountAmountExclTax, int DownloadCount)
        {

            DBOrderProductVariant orderProductVariant = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderProductVariantInsert");
            db.AddOutParameter(dbCommand, "OrderProductVariantID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "OrderID", DbType.Int32, OrderID);
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, ProductVariantID);
            db.AddInParameter(dbCommand, "UnitPriceInclTax", DbType.Decimal, UnitPriceInclTax);
            db.AddInParameter(dbCommand, "UnitPriceExclTax", DbType.Decimal, UnitPriceExclTax);
            db.AddInParameter(dbCommand, "PriceInclTax", DbType.Decimal, PriceInclTax);
            db.AddInParameter(dbCommand, "PriceExclTax", DbType.Decimal, PriceExclTax);
            db.AddInParameter(dbCommand, "UnitPriceInclTaxInCustomerCurrency", DbType.Decimal, UnitPriceInclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "UnitPriceExclTaxInCustomerCurrency", DbType.Decimal, UnitPriceExclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "PriceInclTaxInCustomerCurrency", DbType.Decimal, PriceInclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "PriceExclTaxInCustomerCurrency", DbType.Decimal, PriceExclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "AttributeDescription", DbType.String, AttributeDescription);
            db.AddInParameter(dbCommand, "Quantity", DbType.Int32, Quantity);
            db.AddInParameter(dbCommand, "DiscountAmountInclTax", DbType.Decimal, DiscountAmountInclTax);
            db.AddInParameter(dbCommand, "DiscountAmountExclTax", DbType.Decimal, DiscountAmountExclTax);
            db.AddInParameter(dbCommand, "DownloadCount", DbType.Int32, DownloadCount);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int OrderProductVariantID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@OrderProductVariantID"));
                orderProductVariant = GetOrderProductVariantByID(OrderProductVariantID);
            }
            return orderProductVariant;
        }

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
        public override DBOrderProductVariant UpdateOrderProductVariant(int OrderProductVariantID,
            int OrderID, int ProductVariantID,
            decimal UnitPriceInclTax, decimal UnitPriceExclTax, decimal PriceInclTax, decimal PriceExclTax,
            decimal UnitPriceInclTaxInCustomerCurrency, decimal UnitPriceExclTaxInCustomerCurrency,
            decimal PriceInclTaxInCustomerCurrency, decimal PriceExclTaxInCustomerCurrency,
            string AttributeDescription, int Quantity,
            decimal DiscountAmountInclTax, decimal DiscountAmountExclTax, int DownloadCount)
        {
            DBOrderProductVariant orderProductVariant = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderProductVariantUpdate");
            db.AddInParameter(dbCommand, "OrderProductVariantID", DbType.Int32, OrderProductVariantID);
            db.AddInParameter(dbCommand, "OrderID", DbType.Int32, OrderID);
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, ProductVariantID);
            db.AddInParameter(dbCommand, "UnitPriceInclTax", DbType.Decimal, UnitPriceInclTax);
            db.AddInParameter(dbCommand, "UnitPriceExclTax", DbType.Decimal, UnitPriceExclTax);
            db.AddInParameter(dbCommand, "PriceInclTax", DbType.Decimal, PriceInclTax);
            db.AddInParameter(dbCommand, "PriceExclTax", DbType.Decimal, PriceExclTax);
            db.AddInParameter(dbCommand, "UnitPriceInclTaxInCustomerCurrency", DbType.Decimal, UnitPriceInclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "UnitPriceExclTaxInCustomerCurrency", DbType.Decimal, UnitPriceExclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "PriceInclTaxInCustomerCurrency", DbType.Decimal, PriceInclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "PriceExclTaxInCustomerCurrency", DbType.Decimal, PriceExclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "AttributeDescription", DbType.String, AttributeDescription);
            db.AddInParameter(dbCommand, "Quantity", DbType.Int32, Quantity);
            db.AddInParameter(dbCommand, "DiscountAmountInclTax", DbType.Decimal, DiscountAmountInclTax);
            db.AddInParameter(dbCommand, "DiscountAmountExclTax", DbType.Decimal, DiscountAmountExclTax);
            db.AddInParameter(dbCommand, "DownloadCount", DbType.Int32, DownloadCount);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                orderProductVariant = GetOrderProductVariantByID(OrderProductVariantID);

            return orderProductVariant;
        }

        /// <summary>
        /// Gets an order status by ID
        /// </summary>
        /// <param name="OrderStatusID">Order status identifier</param>
        /// <returns>Order status</returns>
        public override DBOrderStatus GetOrderStatusByID(int OrderStatusID)
        {

            DBOrderStatus orderStatus = null;
            if (OrderStatusID == 0)
                return orderStatus;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderStatusLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, OrderStatusID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    orderStatus = GetOrderStatusFromReader(dataReader);
                    orderStatus.Name = NopSqlDataHelper.GetString(dataReader, "Name");
                }
            }
            return orderStatus;
        }

        /// <summary>
        /// Gets all order statuses
        /// </summary>
        /// <returns>Order status collection</returns>
        public override DBOrderStatusCollection GetAllOrderStatuses()
        {
            DBOrderStatusCollection orderStatusCollection = new DBOrderStatusCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderStatusLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBOrderStatus orderStatus = GetOrderStatusFromReader(dataReader);
                    orderStatusCollection.Add(orderStatus);
                }
            }

            return orderStatusCollection;
        }

        /// <summary>
        /// Gets an order report
        /// </summary>
        /// <param name="OrderStatusID">Order status identifier; null to load all orders</param>
        /// <param name="PaymentStatusID">Order payment status identifier; null to load all orders</param>
        /// <param name="ShippingStatusID">Order shipping status identifier; null to load all orders</param>
        /// <returns>DataTable</returns>
        public override IDataReader GetOrderReport(int? OrderStatusID, int? PaymentStatusID, int? ShippingStatusID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderIncompleteReport");
            if (OrderStatusID.HasValue)
                db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, OrderStatusID.Value);
            else
                db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, null);
            if (PaymentStatusID.HasValue)
                db.AddInParameter(dbCommand, "PaymentStatusID", DbType.Int32, PaymentStatusID.Value);
            else
                db.AddInParameter(dbCommand, "PaymentStatusID", DbType.Int32, null);

            if (ShippingStatusID.HasValue)
                db.AddInParameter(dbCommand, "ShippingStatusID", DbType.Int32, ShippingStatusID);
            else
                db.AddInParameter(dbCommand, "ShippingStatusID", DbType.Int32, null);

            return db.ExecuteReader(dbCommand);
        }
        #endregion
    }
}
