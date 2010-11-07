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
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Web;
using NopSolutions.NopCommerce.BusinessLogic.Audit;
using NopSolutions.NopCommerce.BusinessLogic.Caching;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Localization;
using NopSolutions.NopCommerce.BusinessLogic.Messages;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Products.Attributes;
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.BusinessLogic.Security;
using NopSolutions.NopCommerce.BusinessLogic.Shipping;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.BusinessLogic.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Utils.Html;
using NopSolutions.NopCommerce.Common;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Common.Utils.Html;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Orders;

namespace NopSolutions.NopCommerce.BusinessLogic.Orders
{
	/// <summary>
	/// Order manager
	/// </summary>
	public partial class OrderManager
	{
		#region Constants
		private const string ORDERSTATUSES_ALL_KEY = "Nop.orderstatus.all";
		private const string ORDERSTATUSES_BY_ID_KEY = "Nop.orderstatus.id-{0}";
		private const string ORDERSTATUSES_PATTERN_KEY = "Nop.orderstatus.";
		#endregion

		#region Utilities

		private static OrderCollection DBMapping(DBOrderCollection dbCollection)
		{
			if (dbCollection == null)
				return null;

			OrderCollection collection = new OrderCollection();
			foreach (DBOrder dbItem in dbCollection)
			{
				Order item = DBMapping(dbItem);
				collection.Add(item);
			}

			return collection;
		}

		private static Order DBMapping(DBOrder dbItem)
		{
			if (dbItem == null)
				return null;

			Order item = new Order();
			item.OrderID = dbItem.OrderID;
			item.OrderGUID = dbItem.OrderGUID;
			item.CustomerID = dbItem.CustomerID;
			item.CustomerLanguageID = dbItem.CustomerLanguageID;
			item.CustomerTaxDisplayTypeID = dbItem.CustomerTaxDisplayTypeID;
			item.OrderSubtotalInclTax = dbItem.OrderSubtotalInclTax;
			item.OrderSubtotalExclTax = dbItem.OrderSubtotalExclTax;
			item.OrderShippingInclTax = dbItem.OrderShippingInclTax;
			item.OrderShippingExclTax = dbItem.OrderShippingExclTax;
			item.PaymentMethodAdditionalFeeInclTax = dbItem.PaymentMethodAdditionalFeeInclTax;
			item.PaymentMethodAdditionalFeeExclTax = dbItem.PaymentMethodAdditionalFeeExclTax;
			item.OrderTax = dbItem.OrderTax;
			item.OrderTotal = dbItem.OrderTotal;
			item.OrderDiscount = dbItem.OrderDiscount;
			item.OrderSubtotalInclTaxInCustomerCurrency = dbItem.OrderSubtotalInclTaxInCustomerCurrency;
			item.OrderSubtotalExclTaxInCustomerCurrency = dbItem.OrderSubtotalExclTaxInCustomerCurrency;
			item.OrderShippingInclTaxInCustomerCurrency = dbItem.OrderShippingInclTaxInCustomerCurrency;
			item.OrderShippingExclTaxInCustomerCurrency = dbItem.OrderShippingExclTaxInCustomerCurrency;
			item.PaymentMethodAdditionalFeeInclTaxInCustomerCurrency = dbItem.PaymentMethodAdditionalFeeInclTaxInCustomerCurrency;
			item.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency = dbItem.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency;
			item.OrderTaxInCustomerCurrency = dbItem.OrderTaxInCustomerCurrency;
			item.OrderTotalInCustomerCurrency = dbItem.OrderTotalInCustomerCurrency;
			item.CustomerCurrencyCode = dbItem.CustomerCurrencyCode;
			item.OrderWeight = dbItem.OrderWeight;
			item.AffiliateID = dbItem.AffiliateID;
			item.OrderStatusID = dbItem.OrderStatusID;
			item.AllowStoringCreditCardNumber = dbItem.AllowStoringCreditCardNumber;
			item.CardType = dbItem.CardType;
			item.CardName = dbItem.CardName;
			item.CardNumber = dbItem.CardNumber;
			item.MaskedCreditCardNumber = dbItem.MaskedCreditCardNumber;
			item.CardCVV2 = dbItem.CardCVV2;
			item.CardExpirationMonth = dbItem.CardExpirationMonth;
			item.CardExpirationYear = dbItem.CardExpirationYear;
			item.PaymentMethodID = dbItem.PaymentMethodID;
			item.PaymentMethodName = dbItem.PaymentMethodName;
			item.AuthorizationTransactionID = dbItem.AuthorizationTransactionID;
			item.AuthorizationTransactionCode = dbItem.AuthorizationTransactionCode;
			item.AuthorizationTransactionResult = dbItem.AuthorizationTransactionResult;
			item.CaptureTransactionID = dbItem.CaptureTransactionID;
			item.CaptureTransactionResult = dbItem.CaptureTransactionResult;
			item.PurchaseOrderNumber = dbItem.PurchaseOrderNumber;
			item.PaymentStatusID = dbItem.PaymentStatusID;
			item.BillingFirstName = dbItem.BillingFirstName;
			item.BillingLastName = dbItem.BillingLastName;
			item.BillingPhoneNumber = dbItem.BillingPhoneNumber;
			item.BillingEmail = dbItem.BillingEmail;
			item.BillingFaxNumber = dbItem.BillingFaxNumber;
			item.BillingCompany = dbItem.BillingCompany;
			item.BillingAddress1 = dbItem.BillingAddress1;
			item.BillingAddress2 = dbItem.BillingAddress2;
			item.BillingCity = dbItem.BillingCity;
			item.BillingStateProvince = dbItem.BillingStateProvince;
			item.BillingStateProvinceID = dbItem.BillingStateProvinceID;
			item.BillingZipPostalCode = dbItem.BillingZipPostalCode;
			item.BillingCountry = dbItem.BillingCountry;
			item.BillingCountryID = dbItem.BillingCountryID;
			item.ShippingStatusID = dbItem.ShippingStatusID;
			item.ShippingFirstName = dbItem.ShippingFirstName;
			item.ShippingLastName = dbItem.ShippingLastName;
			item.ShippingPhoneNumber = dbItem.ShippingPhoneNumber;
			item.ShippingEmail = dbItem.ShippingEmail;
			item.ShippingFaxNumber = dbItem.ShippingFaxNumber;
			item.ShippingCompany = dbItem.ShippingCompany;
			item.ShippingAddress1 = dbItem.ShippingAddress1;
			item.ShippingAddress2 = dbItem.ShippingAddress2;
			item.ShippingCity = dbItem.ShippingCity;
			item.ShippingStateProvince = dbItem.ShippingStateProvince;
			item.ShippingStateProvinceID = dbItem.ShippingStateProvinceID;
			item.ShippingZipPostalCode = dbItem.ShippingZipPostalCode;
			item.ShippingCountry = dbItem.ShippingCountry;
			item.ShippingCountryID = dbItem.ShippingCountryID;
			item.ShippingMethod = dbItem.ShippingMethod;
			item.ShippingRateComputationMethodID = dbItem.ShippingRateComputationMethodID;
			item.ShippedDate = dbItem.ShippedDate;
			item.Deleted = dbItem.Deleted;
			item.CreatedOn = dbItem.CreatedOn;

			return item;
		}

		private static OrderNoteCollection DBMapping(DBOrderNoteCollection dbCollection)
		{
			if (dbCollection == null)
				return null;

			OrderNoteCollection collection = new OrderNoteCollection();
			foreach (DBOrderNote dbItem in dbCollection)
			{
				OrderNote item = DBMapping(dbItem);
				collection.Add(item);
			}

			return collection;
		}

		private static OrderNote DBMapping(DBOrderNote dbItem)
		{
			if (dbItem == null)
				return null;

			OrderNote item = new OrderNote();
			item.OrderNoteID = dbItem.OrderNoteID;
			item.OrderID = dbItem.OrderID;
			item.Note = dbItem.Note;
			item.CreatedOn = dbItem.CreatedOn;

			return item;
		}

		private static OrderProductVariantCollection DBMapping(DBOrderProductVariantCollection dbCollection)
		{
			if (dbCollection == null)
				return null;

			OrderProductVariantCollection collection = new OrderProductVariantCollection();
			foreach (DBOrderProductVariant dbItem in dbCollection)
			{
				OrderProductVariant item = DBMapping(dbItem);
				collection.Add(item);
			}

			return collection;
		}

		private static OrderProductVariant DBMapping(DBOrderProductVariant dbItem)
		{
			if (dbItem == null)
				return null;

			OrderProductVariant item = new OrderProductVariant();
			item.OrderProductVariantID = dbItem.OrderProductVariantID;
			item.OrderID = dbItem.OrderID;
			item.ProductVariantID = dbItem.ProductVariantID;
			item.UnitPriceInclTax = dbItem.UnitPriceInclTax;
			item.UnitPriceExclTax = dbItem.UnitPriceExclTax;
			item.PriceInclTax = dbItem.PriceInclTax;
			item.PriceExclTax = dbItem.PriceExclTax;
			item.UnitPriceInclTaxInCustomerCurrency = dbItem.UnitPriceInclTaxInCustomerCurrency;
			item.UnitPriceExclTaxInCustomerCurrency = dbItem.UnitPriceExclTaxInCustomerCurrency;
			item.PriceInclTaxInCustomerCurrency = dbItem.PriceInclTaxInCustomerCurrency;
			item.PriceExclTaxInCustomerCurrency = dbItem.PriceExclTaxInCustomerCurrency;
			item.AttributeDescription = dbItem.AttributeDescription;
			item.Quantity = dbItem.Quantity;
			item.DiscountAmountInclTax = dbItem.DiscountAmountInclTax;
			item.DiscountAmountExclTax = dbItem.DiscountAmountExclTax;
			item.DownloadCount = dbItem.DownloadCount;

			return item;
		}

		private static OrderStatusCollection DBMapping(DBOrderStatusCollection dbCollection)
		{
			if (dbCollection == null)
				return null;

			OrderStatusCollection collection = new OrderStatusCollection();
			foreach (DBOrderStatus dbItem in dbCollection)
			{
				OrderStatus item = DBMapping(dbItem);
				collection.Add(item);
			}

			return collection;
		}

		private static OrderStatus DBMapping(DBOrderStatus dbItem)
		{
			if (dbItem == null)
				return null;

			OrderStatus item = new OrderStatus();
			item.OrderStatusID = dbItem.OrderStatusID;
			item.Name = dbItem.Name;

			return item;
		}

		/// <summary>
		/// Sets an order status
		/// </summary>
		/// <param name="OrderID">Order identifier</param>
		/// <param name="OS">New order status</param>
		/// <param name="notifyCustomer">True to notify customer</param>
		/// <returns>Order</returns>
		protected static Order SetOrderStatus(int OrderID, OrderStatusEnum OS, bool notifyCustomer)
		{
			Order order = GetOrderByID(OrderID);
			if (order != null)
			{
				if (order.OrderStatus == OS)
					return order;

				InsertOrderNote(OrderID, string.Format("Order status has been changed to {0}", OS.ToString()), DateTime.Now);

				if (order.OrderStatus != OrderStatusEnum.Complete &&
					OS == OrderStatusEnum.Complete
					&& notifyCustomer)
				{
					int orderCompletedCustomerNotificationQueuedEmailID = MessageManager.SendOrderCompletedCustomerNotification(order, order.CustomerLanguageID);
					InsertOrderNote(OrderID, string.Format("\"Order completed\" email (to customer) has been queued. Queued email identifier: {0}.", orderCompletedCustomerNotificationQueuedEmailID), DateTime.Now);
				}

				if (order.OrderStatus != OrderStatusEnum.Cancelled &&
					OS == OrderStatusEnum.Cancelled
					&& notifyCustomer)
				{
					int orderCancelledCustomerNotificationQueuedEmailID = MessageManager.SendOrderCancelledCustomerNotification(order, order.CustomerLanguageID);
					InsertOrderNote(OrderID, string.Format("\"Order cancelled\" email (to customer) has been queued. Queued email identifier: {0}.", orderCancelledCustomerNotificationQueuedEmailID), DateTime.Now);
				}

				order = UpdateOrder(order.OrderID, order.OrderGUID, order.CustomerID, order.CustomerLanguageID,
					order.CustomerTaxDisplayType, order.OrderSubtotalInclTax, order.OrderSubtotalExclTax, order.OrderShippingInclTax,
					order.OrderShippingExclTax, order.PaymentMethodAdditionalFeeInclTax, order.PaymentMethodAdditionalFeeExclTax,
					order.OrderTax, order.OrderTotal, order.OrderDiscount,
					order.OrderSubtotalInclTaxInCustomerCurrency, order.OrderSubtotalExclTaxInCustomerCurrency,
					order.OrderShippingInclTaxInCustomerCurrency, order.OrderShippingExclTaxInCustomerCurrency,
					order.PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, order.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
					order.OrderTaxInCustomerCurrency, order.OrderTotalInCustomerCurrency, order.CustomerCurrencyCode, order.OrderWeight,
					order.AffiliateID, OS, order.AllowStoringCreditCardNumber, order.CardType,
					order.CardName, order.CardNumber, order.MaskedCreditCardNumber,
					order.CardCVV2, order.CardExpirationMonth, order.CardExpirationYear,
					order.PaymentMethodID, order.PaymentMethodName,
					order.AuthorizationTransactionID,
					order.AuthorizationTransactionCode, order.AuthorizationTransactionResult,
					order.CaptureTransactionID, order.CaptureTransactionResult,
					order.PurchaseOrderNumber, order.PaymentStatus,
					order.BillingFirstName, order.BillingLastName, order.BillingPhoneNumber,
					order.BillingEmail, order.BillingFaxNumber, order.BillingCompany, order.BillingAddress1,
					order.BillingAddress2, order.BillingCity,
					order.BillingStateProvince, order.BillingStateProvinceID, order.BillingZipPostalCode,
					order.BillingCountry, order.BillingCountryID, order.ShippingStatus,
					order.ShippingFirstName, order.ShippingLastName, order.ShippingPhoneNumber,
					order.ShippingEmail, order.ShippingFaxNumber, order.ShippingCompany,
					order.ShippingAddress1, order.ShippingAddress2, order.ShippingCity,
					order.ShippingStateProvince, order.ShippingStateProvinceID, order.ShippingZipPostalCode,
					order.ShippingCountry, order.ShippingCountryID,
					order.ShippingMethod, order.ShippingRateComputationMethodID, order.ShippedDate,
					order.Deleted, order.CreatedOn);
			}
			return order;
		}

		/// <summary>
		/// Checks order status
		/// </summary>
		/// <param name="OrderID">Order identifier</param>
		/// <returns>Validated order</returns>
		protected static Order CheckOrderStatus(int OrderID)
		{
			Order order = GetOrderByID(OrderID);
			if (order == null)
				return null;

			if (order.OrderStatus == OrderStatusEnum.Pending)
			{
				if (order.PaymentStatus == PaymentStatusEnum.Authorized || order.PaymentStatus == PaymentStatusEnum.Paid)
				{
					order = SetOrderStatus(OrderID, OrderStatusEnum.Processing, false);
				}
			}

			if (order.OrderStatus == OrderStatusEnum.Pending)
			{
				if (order.ShippingStatus == ShippingStatusEnum.Shipped)
				{
					order = SetOrderStatus(OrderID, OrderStatusEnum.Processing, false);
				}
			}

			if (order.OrderStatus != OrderStatusEnum.Cancelled && order.OrderStatus != OrderStatusEnum.Complete)
			{
				if (order.PaymentStatus == PaymentStatusEnum.Paid)
				{
					if (!CanShip(order))
					{
						order = SetOrderStatus(OrderID, OrderStatusEnum.Complete, true);
					}
				}
			}

			return order;
		}

		private static OrderAverageReportLine DBMapping(DBOrderAverageReportLine dbItem)
		{
			if (dbItem == null)
				return null;

			OrderAverageReportLine item = new OrderAverageReportLine();
			item.SumTodayOrders = dbItem.SumTodayOrders;
			item.CountTodayOrders = dbItem.CountTodayOrders;
			item.SumThisWeekOrders = dbItem.SumThisWeekOrders;
			item.CountThisWeekOrders = dbItem.CountThisWeekOrders;
			item.SumThisMonthOrders = dbItem.SumThisMonthOrders;
			item.CountThisMonthOrders = dbItem.CountThisMonthOrders;
			item.SumThisYearOrders = dbItem.SumThisYearOrders;
			item.CountThisYearOrders = dbItem.CountThisYearOrders;
			item.SumAllTimeOrders = dbItem.SumAllTimeOrders;
			item.CountAllTimeOrders = dbItem.CountAllTimeOrders;

			return item;
		}

		private static List<BestSellersReportLine> DBMapping(List<DBBestSellersReportLine> dbCollection)
		{
			if (dbCollection == null)
				return null;

			List<BestSellersReportLine> collection = new List<BestSellersReportLine>();
			foreach (DBBestSellersReportLine dbItem in dbCollection)
			{
				BestSellersReportLine item = DBMapping(dbItem);
				collection.Add(item);
			}

			return collection;
		}

		private static BestSellersReportLine DBMapping(DBBestSellersReportLine dbItem)
		{
			if (dbItem == null)
				return null;

			BestSellersReportLine item = new BestSellersReportLine();
			item.ProductVariantID = dbItem.ProductVariantID;
			item.SalesTotalCount = dbItem.SalesTotalCount;
			item.SalesTotalAmount = dbItem.SalesTotalAmount;

			return item;
		}

		#endregion

		#region Methods
		/// <summary>
		/// Gets an order
		/// </summary>
		/// <param name="OrderID">The order identifier</param>
		/// <returns>Order</returns>
		public static Order GetOrderByID(int OrderID)
		{
			if (OrderID == 0)
				return null;

			DBOrder dbItem = DBProviderManager<DBOrderProvider>.Provider.GetOrderByID(OrderID);
			Order order = DBMapping(dbItem);
			return order;
		}

		/// <summary>
		/// Marks an order as deleted
		/// </summary>
		/// <param name="OrderID">The order identifier</param>
		public static void MarkOrderAsDeleted(int OrderID)
		{
			Order order = GetOrderByID(OrderID);
			if (order != null)
			{
				UpdateOrder(order.OrderID, order.OrderGUID, order.CustomerID, order.CustomerLanguageID,
					order.CustomerTaxDisplayType, order.OrderSubtotalInclTax, order.OrderSubtotalExclTax, order.OrderShippingInclTax,
				   order.OrderShippingExclTax, order.PaymentMethodAdditionalFeeInclTax, order.PaymentMethodAdditionalFeeExclTax,
				   order.OrderTax, order.OrderTotal, order.OrderDiscount,
				   order.OrderSubtotalInclTaxInCustomerCurrency, order.OrderSubtotalExclTaxInCustomerCurrency,
				   order.OrderShippingInclTaxInCustomerCurrency, order.OrderShippingExclTaxInCustomerCurrency,
				   order.PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, order.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
				   order.OrderTaxInCustomerCurrency, order.OrderTotalInCustomerCurrency, order.CustomerCurrencyCode, order.OrderWeight,
				   order.AffiliateID, order.OrderStatus, order.AllowStoringCreditCardNumber, order.CardType,
				   order.CardName, order.CardNumber, order.MaskedCreditCardNumber,
					order.CardCVV2, order.CardExpirationMonth, order.CardExpirationYear,
					order.PaymentMethodID, order.PaymentMethodName, order.AuthorizationTransactionID,
					order.AuthorizationTransactionCode, order.AuthorizationTransactionResult,
					order.CaptureTransactionID, order.CaptureTransactionResult,
					order.PurchaseOrderNumber, order.PaymentStatus,
					order.BillingFirstName, order.BillingLastName, order.BillingPhoneNumber,
					order.BillingEmail, order.BillingFaxNumber, order.BillingCompany, order.BillingAddress1,
					order.BillingAddress2, order.BillingCity, order.BillingStateProvince,
					order.BillingStateProvinceID, order.BillingZipPostalCode, order.BillingCountry,
					order.BillingCountryID, order.ShippingStatus,
					order.ShippingFirstName, order.ShippingLastName, order.ShippingPhoneNumber,
					order.ShippingEmail, order.ShippingFaxNumber, order.ShippingCompany,
					order.ShippingAddress1, order.ShippingAddress2, order.ShippingCity,
					order.ShippingStateProvince, order.ShippingStateProvinceID, order.ShippingZipPostalCode,
					order.ShippingCountry, order.ShippingCountryID,
					order.ShippingMethod, order.ShippingRateComputationMethodID, order.ShippedDate, true,
					order.CreatedOn);
			}
		}

		/// <summary>
		/// Search orders
		/// </summary>
		/// <param name="StartTime">Order start time; null to load all orders</param>
		/// <param name="EndTime">Order end time; null to load all orders</param>
		/// <param name="CustomerEmail">Customer email</param>
		/// <param name="OS">Order status; null to load all orders</param>
		/// <param name="PS">Order payment status; null to load all orders</param>
		/// <param name="SS">Order shippment status; null to load all orders</param>
		/// <returns>Order collection</returns>
		public static OrderCollection SearchOrders(DateTime? StartTime, DateTime? EndTime, string CustomerEmail, OrderStatusEnum? OS, PaymentStatusEnum? PS, ShippingStatusEnum? SS)
		{
			int? orderStatusID = null;
			if (OS.HasValue)
				orderStatusID = (int)OS.Value;

			int? paymentStatusID = null;
			if (PS.HasValue)
				paymentStatusID = (int)PS.Value;

			int? shippingStatusID = null;
			if (SS.HasValue)
				shippingStatusID = (int)SS.Value;

			DBOrderCollection dbCollection = DBProviderManager<DBOrderProvider>.Provider.SearchOrders(StartTime, EndTime, CustomerEmail, orderStatusID, paymentStatusID, shippingStatusID);
			OrderCollection orders = DBMapping(dbCollection);
			return orders;
		}

		/// <summary>
		/// Load all orders
		/// </summary>
		/// <returns>Order collection</returns>
		public static OrderCollection LoadAllOrders()
		{
			return SearchOrders(null, null, string.Empty, null, null, null);
		}

		/// <summary>
		/// Get order product variant sales report
		/// </summary>
		/// <param name="StartTime">Order start time; null to load all</param>
		/// <param name="EndTime">Order end time; null to load all</param>
		/// <param name="OS">Order status; null to load all orders</param>
		/// <param name="PS">Order payment status; null to load all orders</param>
		/// <returns>Result</returns>
		public static IDataReader OrderProductVariantReport(DateTime? StartTime, DateTime? EndTime,
			OrderStatusEnum? OS, PaymentStatusEnum? PS)
		{
			int? orderStatusID = null;
			if (OS.HasValue)
				orderStatusID = (int)OS.Value;

			int? paymentStatusID = null;
			if (PS.HasValue)
				paymentStatusID = (int)PS.Value;

			return DBProviderManager<DBOrderProvider>.Provider.OrderProductVariantReport(StartTime, EndTime, orderStatusID, paymentStatusID);
		}

		/// <summary>
		/// Get the bests sellers report
		/// </summary>
		/// <param name="LastDays">Last number of days</param>
		/// <param name="RecordsToReturn">Number of products to return</param>
		/// <param name="OrderBy">1 - order by total count, 2 - Order by total amount</param>
		/// <returns>Result</returns>
		public static List<BestSellersReportLine> BestSellersReport(int LastDays, int RecordsToReturn, int OrderBy)
		{
			List<DBBestSellersReportLine> dbCollection = DBProviderManager<DBOrderProvider>.Provider.BestSellersReport(LastDays, RecordsToReturn, OrderBy);
			List<BestSellersReportLine> report = DBMapping(dbCollection);
			return report;
		}

		/// <summary>
		/// Get order average report
		/// </summary>
		/// <param name="OS">Order status; null to load all orders</param>
		/// <returns>Result</returns>
		public static OrderAverageReportLine OrderAverageReport(OrderStatusEnum OS)
		{
			int orderStatusID = (int)OS;
			DBOrderAverageReportLine dbItem = DBProviderManager<DBOrderProvider>.Provider.OrderAverageReport(orderStatusID);
			OrderAverageReportLine orderAverageReportLine = DBMapping(dbItem);
			orderAverageReportLine.OrderStatus = OS;
			return orderAverageReportLine;
		}

		/// <summary>
		/// Gets all orders by customer identifier
		/// </summary>
		/// <param name="CustomerID">Customer identifier</param>
		/// <returns>Order collection</returns>
		public static OrderCollection GetOrdersByCustomerID(int CustomerID)
		{
			DBOrderCollection dbCollection = DBProviderManager<DBOrderProvider>.Provider.GetOrdersByCustomerID(CustomerID);
			OrderCollection orders = DBMapping(dbCollection);
			return orders;
		}

		/// <summary>
		/// Gets an order by authorization transaction identifier
		/// </summary>
		/// <param name="AuthorizationTransactionID">Authorization transaction identifier</param>
		/// <param name="PaymentMethodID">Payment method identifier</param>
		/// <returns>Order</returns>
		public static Order GetOrderByAuthorizationTransactionIDAndPaymentMethodID(string AuthorizationTransactionID, int PaymentMethodID)
		{
			DBOrder dbItem = DBProviderManager<DBOrderProvider>.Provider.GetOrderByAuthorizationTransactionIDAndPaymentMethodID(AuthorizationTransactionID, PaymentMethodID);
			Order order = DBMapping(dbItem);
			return order;
		}

		/// <summary>
		/// Gets all orders by affiliate identifier
		/// </summary>
		/// <param name="AffiliateID">Affiliate identifier</param>
		/// <returns>Order collection</returns>
		public static OrderCollection GetOrdersByAffiliateID(int AffiliateID)
		{
			DBOrderCollection dbCollection = DBProviderManager<DBOrderProvider>.Provider.GetOrdersByAffiliateID(AffiliateID);
			OrderCollection orders = DBMapping(dbCollection);
			return orders;
		}

		/// <summary>
		/// Inserts an order
		/// </summary>
		/// <param name="OrderGUID">The order identifier</param>
		/// <param name="CustomerID">The customer identifier</param>
		/// <param name="CustomerLanguageID">The customer language identifier</param>
		/// <param name="CustomerTaxDisplayType">The customer tax display type</param>
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
		/// <param name="OrderStatus">The order status</param>
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
		/// <param name="PaymentStatus">The payment status</param>
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
		/// <param name="ShippingStatus">The shipping status</param>
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
		public static Order InsertOrder(Guid OrderGUID, int CustomerID, int CustomerLanguageID,
			TaxDisplayTypeEnum CustomerTaxDisplayType, decimal OrderSubtotalInclTax, decimal OrderSubtotalExclTax,
			decimal OrderShippingInclTax, decimal OrderShippingExclTax,
			decimal PaymentMethodAdditionalFeeInclTax, decimal PaymentMethodAdditionalFeeExclTax,
			decimal OrderTax, decimal OrderTotal, decimal OrderDiscount,
			decimal OrderSubtotalInclTaxInCustomerCurrency, decimal OrderSubtotalExclTaxInCustomerCurrency,
			decimal OrderShippingInclTaxInCustomerCurrency, decimal OrderShippingExclTaxInCustomerCurrency,
			decimal PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, decimal PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
			decimal OrderTaxInCustomerCurrency, decimal OrderTotalInCustomerCurrency,
			string CustomerCurrencyCode, decimal OrderWeight,
			int AffiliateID, OrderStatusEnum OrderStatus, bool AllowStoringCreditCardNumber, string CardType,
			string CardName, string CardNumber, string MaskedCreditCardNumber, string CardCVV2,
			string CardExpirationMonth, string CardExpirationYear, int PaymentMethodID,
			string PaymentMethodName, string AuthorizationTransactionID, string AuthorizationTransactionCode,
			string AuthorizationTransactionResult, string CaptureTransactionID, string CaptureTransactionResult,
			string PurchaseOrderNumber, PaymentStatusEnum PaymentStatus, string BillingFirstName, string BillingLastName,
			string BillingPhoneNumber, string BillingEmail, string BillingFaxNumber, string BillingCompany,
			string BillingAddress1, string BillingAddress2, string BillingCity, string BillingStateProvince,
			int BillingStateProvinceID, string BillingZipPostalCode, string BillingCountry,
			int BillingCountryID, ShippingStatusEnum ShippingStatus, string ShippingFirstName,
			string ShippingLastName, string ShippingPhoneNumber, string ShippingEmail,
			string ShippingFaxNumber, string ShippingCompany, string ShippingAddress1,
			string ShippingAddress2, string ShippingCity, string ShippingStateProvince,
			int ShippingStateProvinceID, string ShippingZipPostalCode,
			string ShippingCountry, int ShippingCountryID, string ShippingMethod, int ShippingRateComputationMethodID, DateTime? ShippedDate,
			bool Deleted, DateTime CreatedOn)
		{
			if (CustomerCurrencyCode == null)
				CustomerCurrencyCode = string.Empty;
			if (CardType == null)
				CardType = string.Empty;
			if (CardName == null)
				CardName = string.Empty;
			if (CardNumber == null)
				CardNumber = string.Empty;
			if (CardCVV2 == null)
				CardCVV2 = string.Empty;
			if (CardExpirationMonth == null)
				CardExpirationMonth = string.Empty;
			if (CardExpirationYear == null)
				CardExpirationYear = string.Empty;
			if (PaymentMethodName == null)
				PaymentMethodName = string.Empty;
			if (AuthorizationTransactionID == null)
				AuthorizationTransactionID = string.Empty;
			if (AuthorizationTransactionCode == null)
				AuthorizationTransactionCode = string.Empty;
			if (AuthorizationTransactionResult == null)
				AuthorizationTransactionResult = string.Empty;
			if (CaptureTransactionID == null)
				CaptureTransactionID = string.Empty;
			if (CaptureTransactionResult == null)
				CaptureTransactionResult = string.Empty;
			if (PurchaseOrderNumber == null)
				PurchaseOrderNumber = string.Empty;
			if (BillingFirstName == null)
				BillingFirstName = string.Empty;
			if (BillingLastName == null)
				BillingLastName = string.Empty;
			if (BillingPhoneNumber == null)
				BillingPhoneNumber = string.Empty;
			if (BillingEmail == null)
				BillingEmail = string.Empty;
			if (BillingFaxNumber == null)
				BillingFaxNumber = string.Empty;
			if (BillingCompany == null)
				BillingCompany = string.Empty;
			if (BillingZipPostalCode == null)
				BillingZipPostalCode = string.Empty;
			if (BillingCountry == null)
				BillingCountry = string.Empty;
			if (ShippingLastName == null)
				ShippingLastName = string.Empty;
			if (ShippingPhoneNumber == null)
				ShippingPhoneNumber = string.Empty;
			if (ShippingEmail == null)
				ShippingEmail = string.Empty;
			if (ShippingFaxNumber == null)
				ShippingFaxNumber = string.Empty;
			if (ShippingCompany == null)
				ShippingCompany = string.Empty;
			if (ShippingAddress1 == null)
				ShippingAddress1 = string.Empty;
			if (ShippingAddress2 == null)
				ShippingAddress2 = string.Empty;
			if (ShippingCity == null)
				ShippingCity = string.Empty;
			if (ShippingStateProvince == null)
				ShippingStateProvince = string.Empty;
			if (ShippingZipPostalCode == null)
				ShippingZipPostalCode = string.Empty;
			if (ShippingCountry == null)
				ShippingCountry = string.Empty;
			if (ShippingMethod == null)
				ShippingMethod = string.Empty;

			if (ShippedDate.HasValue)
				ShippedDate = DateTimeHelper.ConvertToUtcTime(ShippedDate.Value);
			CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);

			DBOrder dbItem = DBProviderManager<DBOrderProvider>.Provider.InsertOrder(OrderGUID, CustomerID, CustomerLanguageID,
			 (int)CustomerTaxDisplayType, OrderSubtotalInclTax, OrderSubtotalExclTax,
			 OrderShippingInclTax, OrderShippingExclTax,
			 PaymentMethodAdditionalFeeInclTax, PaymentMethodAdditionalFeeExclTax,
			 OrderTax, OrderTotal, OrderDiscount,
			 OrderSubtotalInclTaxInCustomerCurrency, OrderSubtotalExclTaxInCustomerCurrency,
			 OrderShippingInclTaxInCustomerCurrency, OrderShippingExclTaxInCustomerCurrency,
			 PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
			 OrderTaxInCustomerCurrency, OrderTotalInCustomerCurrency, CustomerCurrencyCode, OrderWeight,
			 AffiliateID, (int)OrderStatus, AllowStoringCreditCardNumber,
			 CardType, CardName, CardNumber, MaskedCreditCardNumber, CardCVV2,
			 CardExpirationMonth, CardExpirationYear, PaymentMethodID,
			 PaymentMethodName, AuthorizationTransactionID, AuthorizationTransactionCode,
			 AuthorizationTransactionResult, CaptureTransactionID, CaptureTransactionResult, PurchaseOrderNumber,
			 (int)PaymentStatus, BillingFirstName, BillingLastName,
			 BillingPhoneNumber, BillingEmail, BillingFaxNumber, BillingCompany,
			 BillingAddress1, BillingAddress2, BillingCity, BillingStateProvince, BillingStateProvinceID,
			 BillingZipPostalCode, BillingCountry, BillingCountryID, (int)ShippingStatus, ShippingFirstName, ShippingLastName,
			 ShippingPhoneNumber, ShippingEmail,
			 ShippingFaxNumber, ShippingCompany, ShippingAddress1,
			 ShippingAddress2, ShippingCity, ShippingStateProvince, ShippingStateProvinceID, ShippingZipPostalCode,
			 ShippingCountry, ShippingCountryID, ShippingMethod, ShippingRateComputationMethodID, ShippedDate,
			 Deleted, CreatedOn);

			Order order = DBMapping(dbItem);
			return order;
		}

		/// <summary>
		/// Updates the order
		/// </summary>
		/// <param name="OrderID">he order identifier</param>
		/// <param name="OrderGUID">The order identifier</param>
		/// <param name="CustomerID">The customer identifier</param>
		/// <param name="CustomerLanguageID">The customer language identifier</param>
		/// <param name="CustomerTaxDisplayType">The customer tax display type</param>
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
		/// <param name="OrderStatus">The order status</param>
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
		/// <param name="PaymentStatus">The payment status</param>
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
		/// <param name="ShippingStatus">The shipping status</param>
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
		public static Order UpdateOrder(int OrderID, Guid OrderGUID, int CustomerID, int CustomerLanguageID,
			TaxDisplayTypeEnum CustomerTaxDisplayType, decimal OrderSubtotalInclTax, decimal OrderSubtotalExclTax,
			decimal OrderShippingInclTax, decimal OrderShippingExclTax,
			decimal PaymentMethodAdditionalFeeInclTax, decimal PaymentMethodAdditionalFeeExclTax,
			decimal OrderTax, decimal OrderTotal, decimal OrderDiscount,
			decimal OrderSubtotalInclTaxInCustomerCurrency, decimal OrderSubtotalExclTaxInCustomerCurrency,
			decimal OrderShippingInclTaxInCustomerCurrency, decimal OrderShippingExclTaxInCustomerCurrency,
			decimal PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, decimal PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
			decimal OrderTaxInCustomerCurrency, decimal OrderTotalInCustomerCurrency,
			string CustomerCurrencyCode, decimal OrderWeight,
			int AffiliateID, OrderStatusEnum OrderStatus, bool AllowStoringCreditCardNumber,
			string CardType, string CardName, string CardNumber, string MaskedCreditCardNumber, string CardCVV2,
			string CardExpirationMonth, string CardExpirationYear, int PaymentMethodID,
			string PaymentMethodName, string AuthorizationTransactionID, string AuthorizationTransactionCode,
			string AuthorizationTransactionResult, string CaptureTransactionID, string CaptureTransactionResult,
			string PurchaseOrderNumber, PaymentStatusEnum PaymentStatus, string BillingFirstName, string BillingLastName,
			string BillingPhoneNumber, string BillingEmail, string BillingFaxNumber, string BillingCompany,
			string BillingAddress1, string BillingAddress2, string BillingCity, string BillingStateProvince,
			int BillingStateProvinceID, string BillingZipPostalCode, string BillingCountry,
			int BillingCountryID, ShippingStatusEnum ShippingStatus, string ShippingFirstName,
			string ShippingLastName, string ShippingPhoneNumber, string ShippingEmail,
			string ShippingFaxNumber, string ShippingCompany, string ShippingAddress1,
			string ShippingAddress2, string ShippingCity, string ShippingStateProvince,
			int ShippingStateProvinceID, string ShippingZipPostalCode,
			string ShippingCountry, int ShippingCountryID, string ShippingMethod, int ShippingRateComputationMethodID, DateTime? ShippedDate,
			bool Deleted, DateTime CreatedOn)
		{
			if (ShippedDate.HasValue)
				ShippedDate = DateTimeHelper.ConvertToUtcTime(ShippedDate.Value);
			CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);

			DBOrder dbItem = DBProviderManager<DBOrderProvider>.Provider.UpdateOrder(OrderID, OrderGUID, CustomerID, CustomerLanguageID,
				(int)CustomerTaxDisplayType, OrderSubtotalInclTax, OrderSubtotalExclTax,
				OrderShippingInclTax, OrderShippingExclTax,
				PaymentMethodAdditionalFeeInclTax, PaymentMethodAdditionalFeeExclTax,
				OrderTax, OrderTotal, OrderDiscount,
				OrderSubtotalInclTaxInCustomerCurrency, OrderSubtotalExclTaxInCustomerCurrency,
				OrderShippingInclTaxInCustomerCurrency, OrderShippingExclTaxInCustomerCurrency,
				PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
				OrderTaxInCustomerCurrency, OrderTotalInCustomerCurrency, CustomerCurrencyCode, OrderWeight,
				AffiliateID, (int)OrderStatus, AllowStoringCreditCardNumber,
				CardType, CardName, CardNumber, MaskedCreditCardNumber, CardCVV2,
				CardExpirationMonth, CardExpirationYear, PaymentMethodID,
				PaymentMethodName, AuthorizationTransactionID, AuthorizationTransactionCode,
				AuthorizationTransactionResult, CaptureTransactionID, CaptureTransactionResult, PurchaseOrderNumber,
				(int)PaymentStatus, BillingFirstName, BillingLastName,
				BillingPhoneNumber, BillingEmail, BillingFaxNumber, BillingCompany,
				BillingAddress1, BillingAddress2, BillingCity, BillingStateProvince, BillingStateProvinceID,
				BillingZipPostalCode, BillingCountry, BillingCountryID, (int)ShippingStatus, ShippingFirstName, ShippingLastName,
				ShippingPhoneNumber, ShippingEmail,
				ShippingFaxNumber, ShippingCompany, ShippingAddress1,
				ShippingAddress2, ShippingCity, ShippingStateProvince, ShippingStateProvinceID, ShippingZipPostalCode,
				ShippingCountry, ShippingCountryID, ShippingMethod, ShippingRateComputationMethodID, ShippedDate,
				Deleted, CreatedOn);

			Order order = DBMapping(dbItem);
			return order;
		}

		/// <summary>
		/// Places an order
		/// </summary>
		/// <param name="paymentInfo">Payment info</param>
		/// <param name="customer">Customer</param>
		/// <param name="OrderID">Order identifier</param>
		/// <returns>The error status, or String.Empty if no errors</returns>
		public static string PlaceOrder(PaymentInfo paymentInfo, Customer customer, out int OrderID)
		{
			Guid OrderGuid = Guid.NewGuid();
			return PlaceOrder(paymentInfo, customer, OrderGuid, out OrderID);
		}

		/// <summary>
		/// Places an order
		/// </summary>
		/// <param name="paymentInfo">Payment info</param>
		/// <param name="customer">Customer</param>
		/// <param name="OrderGuid">Order GUID to use</param>
		/// <param name="OrderID">Order identifier</param>
		/// <returns>The error status, or String.Empty if no errors</returns>
		public static string PlaceOrder(PaymentInfo paymentInfo, Customer customer, Guid OrderGuid,
			out int OrderID)
		{
			OrderID = 0;
			ProcessPaymentResult processPaymentResult = new ProcessPaymentResult();
			try
			{
				if (customer == null)
					throw new ArgumentNullException("customer");

				if (customer.IsGuest && !CustomerManager.AnonymousCheckoutAllowed)
					throw new NopException("Anonymous checkout is not allowed");


				paymentInfo.BillingAddress.Email = customer.Email;

				//if (!CommonHelper.IsValidEmail(customer.Email))
				//{
				//    throw new NopException("Email is not valid");
				//}

				if (paymentInfo == null)
					throw new ArgumentNullException("paymentInfo");

				if (paymentInfo.BillingAddress == null)
					throw new NopException("Billing address not provided");

				//if (!CommonHelper.IsValidEmail(paymentInfo.BillingAddress.Email))
				//{
				//    throw new NopException("Email is not valid");
				//}

				//PaymentMethod paymentMethod = PaymentMethodManager.GetPaymentMethodByID(paymentInfo.PaymentMethodID);
				//if (paymentMethod == null)
				//    throw new NopException("Payment method couldn't be loaded");

				//if (!paymentMethod.IsActive)
				//    throw new NopException("Payment method is not active");

				if (paymentInfo.CreditCardCVV2 == null)
					paymentInfo.CreditCardCVV2 = string.Empty;

				if (paymentInfo.CreditCardName == null)
					paymentInfo.CreditCardName = string.Empty;

				if (paymentInfo.CreditCardNumber == null)
					paymentInfo.CreditCardNumber = string.Empty;

				if (paymentInfo.CreditCardType == null)
					paymentInfo.CreditCardType = string.Empty;

				if (paymentInfo.PurchaseOrderNumber == null)
					paymentInfo.PurchaseOrderNumber = string.Empty;

				ShoppingCart cart = ShoppingCartManager.GetCustomerShoppingCart(customer.CustomerID, ShoppingCartTypeEnum.ShoppingCart);

				foreach (ShoppingCartItem sci in cart)
				{
					List<string> sciWarnings = ShoppingCartManager.GetShoppingCartItemWarnings(sci.ShoppingCartType,
						sci.ProductVariantID, sci.AttributesXML, sci.Quantity);

					if (sciWarnings.Count > 0)
					{
						StringBuilder warningsSb = new StringBuilder();
						foreach (string warning in sciWarnings)
						{
							warningsSb.Append(warning);
							warningsSb.Append(";");
						}
						throw new NopException(warningsSb.ToString());
					}
				}

				TaxDisplayTypeEnum customerTaxDisplayType = TaxDisplayTypeEnum.IncludingTax;
				if (TaxManager.AllowCustomersToSelectTaxDisplayType)
					customerTaxDisplayType = customer.TaxDisplayType;
				else
					customerTaxDisplayType = TaxManager.TaxDisplayType;

				decimal orderSubTotalDiscount;
				string SubTotalError1 = string.Empty;
				string SubTotalError2 = string.Empty;
				decimal orderSubTotalInclTax = ShoppingCartManager.GetShoppingCartSubTotal(cart, customer, out orderSubTotalDiscount, true, ref SubTotalError1);
				decimal orderSubTotalExclTax = ShoppingCartManager.GetShoppingCartSubTotal(cart, customer, out orderSubTotalDiscount, false, ref SubTotalError2);
				if (!String.IsNullOrEmpty(SubTotalError1) || !String.IsNullOrEmpty(SubTotalError2))
					throw new NopException("Sub total couldn't be calculated");

				decimal orderWeight = ShippingManager.GetShoppingCartTotalWeigth(cart);
				bool shoppingCartRequiresShipping = ShippingManager.ShoppingCartRequiresShipping(cart);
				//if (shoppingCartRequiresShipping)
				//{
				//    if (paymentInfo.ShippingAddress == null)
				//        throw new NopException("Shipping address is not provided");

				//    if (!CommonHelper.IsValidEmail(paymentInfo.ShippingAddress.Email))
				//    {
				//        throw new NopException("Email is not valid");
				//    }
				//}

				string ShippingTotalError1 = string.Empty;
				string ShippingTotalError2 = string.Empty;
				decimal? orderShippingTotalInclTax = ShippingManager.GetShoppingCartShippingTotal(cart, customer, true, ref ShippingTotalError1);
				decimal? orderShippingTotalExclTax = ShippingManager.GetShoppingCartShippingTotal(cart, customer, false, ref ShippingTotalError2);
				if (!orderShippingTotalInclTax.HasValue || !orderShippingTotalExclTax.HasValue)
					throw new NopException("Shipping total couldn't be calculated");

				string PaymentAdditionalFeeError1 = string.Empty;
				string PaymentAdditionalFeeError2 = string.Empty;
				decimal paymentAdditionalFee = PaymentManager.GetAdditionalHandlingFee(paymentInfo.PaymentMethodID);
				decimal paymentAdditionalFeeInclTax = TaxManager.GetPaymentMethodAdditionalFee(paymentAdditionalFee, true, customer, ref PaymentAdditionalFeeError1);
				decimal paymentAdditionalFeeExclTax = TaxManager.GetPaymentMethodAdditionalFee(paymentAdditionalFee, false, customer, ref PaymentAdditionalFeeError2);
				if (!String.IsNullOrEmpty(PaymentAdditionalFeeError1))
					throw new NopException("Payment method fee couldn't be calculated");
				if (!String.IsNullOrEmpty(PaymentAdditionalFeeError2))
					throw new NopException("Payment method fee couldn't be calculated");

				string TaxError = string.Empty;
				decimal orderTaxTotal = TaxManager.GetTaxTotal(cart, paymentInfo.PaymentMethodID, customer, ref TaxError);
				if (!String.IsNullOrEmpty(TaxError))
					throw new NopException("Tax total couldn't be calculated");

				decimal? orderTotal = ShoppingCartManager.GetShoppingCartTotal(cart, paymentInfo.PaymentMethodID, customer);
				if (!orderTotal.HasValue)
					throw new NopException("Order total couldn't be calculated");
				paymentInfo.OrderTotal = orderTotal.Value;

				decimal orderSubtotalInclTaxInCustomerCurrency = CurrencyManager.ConvertCurrency(orderSubTotalInclTax, CurrencyManager.PrimaryStoreCurrency, paymentInfo.CustomerCurrency);
				decimal orderSubtotalExclTaxInCustomerCurrency = CurrencyManager.ConvertCurrency(orderSubTotalExclTax, CurrencyManager.PrimaryStoreCurrency, paymentInfo.CustomerCurrency);
				decimal orderShippingInclTaxInCustomerCurrency = CurrencyManager.ConvertCurrency(orderShippingTotalInclTax.Value, CurrencyManager.PrimaryStoreCurrency, paymentInfo.CustomerCurrency);
				decimal orderShippingExclTaxInCustomerCurrency = CurrencyManager.ConvertCurrency(orderShippingTotalExclTax.Value, CurrencyManager.PrimaryStoreCurrency, paymentInfo.CustomerCurrency);
				decimal paymentAdditionalFeeInclTaxInCustomerCurrency = CurrencyManager.ConvertCurrency(paymentAdditionalFeeInclTax, CurrencyManager.PrimaryStoreCurrency, paymentInfo.CustomerCurrency);
				decimal paymentAdditionalFeeExclTaxInCustomerCurrency = CurrencyManager.ConvertCurrency(paymentAdditionalFeeExclTax, CurrencyManager.PrimaryStoreCurrency, paymentInfo.CustomerCurrency);
				decimal orderTaxInCustomerCurrency = CurrencyManager.ConvertCurrency(orderTaxTotal, CurrencyManager.PrimaryStoreCurrency, paymentInfo.CustomerCurrency);
				decimal orderTotalInCustomerCurrency = CurrencyManager.ConvertCurrency(orderTotal.Value, CurrencyManager.PrimaryStoreCurrency, paymentInfo.CustomerCurrency);
				string customerCurrencyCode = paymentInfo.CustomerCurrency.CurrencyCode;

				string billingStateProvince = string.Empty;
				int billingStateProvinceID = 0;
				string billingCountry = string.Empty;
				int billingCountryID = 0;
				if (paymentInfo.BillingAddress.StateProvince != null)
				{
					billingStateProvince = paymentInfo.BillingAddress.StateProvince.Name;
					billingStateProvinceID = paymentInfo.BillingAddress.StateProvince.StateProvinceID;
				}
				if (paymentInfo.BillingAddress.Country != null)
				{
					billingCountry = paymentInfo.BillingAddress.Country.Name;
					billingCountryID = paymentInfo.BillingAddress.Country.CountryID;

					if (!paymentInfo.BillingAddress.Country.AllowsBilling)
					{
						throw new NopException(string.Format("{0} is not allowed for billing", billingCountry));
					}
				}
				string shippingFirstName = string.Empty;
				string shippingLastName = string.Empty;
				string shippingPhoneNumber = string.Empty;
				string shippingEmail = string.Empty;
				string shippingFaxNumber = string.Empty;
				string shippingCompany = string.Empty;
				string shippingAddress1 = string.Empty;
				string shippingAddress2 = string.Empty;
				string shippingCity = string.Empty;
				string shippingStateProvince = string.Empty;
				int shippingStateProvinceID = 0;
				string shippingZipPostalCode = string.Empty;
				string shippingCountry = string.Empty;
				int shippingCountryID = 0;
				string shippingMethodName = string.Empty;
				if (shoppingCartRequiresShipping)
				{
					Address shippingAddress = paymentInfo.ShippingAddress;
					if (shippingAddress != null)
					{
						shippingFirstName = shippingAddress.FirstName;
						shippingLastName = shippingAddress.LastName;
						shippingPhoneNumber = shippingAddress.PhoneNumber;
						shippingEmail = shippingAddress.Email;
						shippingFaxNumber = shippingAddress.FaxNumber;
						shippingCompany = shippingAddress.Company;
						shippingAddress1 = shippingAddress.Address1;
						shippingAddress2 = shippingAddress.Address2;
						shippingCity = shippingAddress.City;
						if (shippingAddress.StateProvince != null)
						{
							shippingStateProvince = shippingAddress.StateProvince.Name;
							shippingStateProvinceID = shippingAddress.StateProvince.StateProvinceID;
						}
						shippingZipPostalCode = shippingAddress.ZipPostalCode;
						if (shippingAddress.Country != null)
						{
							shippingCountry = shippingAddress.Country.Name;
							shippingCountryID = shippingAddress.Country.CountryID;

							if (!shippingAddress.Country.AllowsShipping)
							{
								throw new NopException(string.Format("{0} is not allowed for shipping", shippingCountry));
							}
						}
						shippingMethodName = string.Empty;
						ShippingOption shippingOption = customer.LastShippingOption;
						if (shippingOption != null)
							shippingMethodName = shippingOption.Name;
					}
				}

				int activeShippingRateComputationMethodID = 0;
				ShippingRateComputationMethod activeShippingRateComputationMethod = ShippingManager.ActiveShippingRateComputationMethod;
				if (activeShippingRateComputationMethod != null)
				{
					activeShippingRateComputationMethodID = activeShippingRateComputationMethod.ShippingRateComputationMethodID;
				}

				//PaymentManager.ProcessPayment(paymentInfo, customer, OrderGuid, ref processPaymentResult);

				int customerLanguageID = paymentInfo.CustomerLanguage.LanguageID;
				if (String.IsNullOrEmpty(processPaymentResult.Error))
				{
					ShippingStatusEnum shippingStatusEnum = ShippingStatusEnum.NotYetShipped;
					if (!shoppingCartRequiresShipping)
						shippingStatusEnum = ShippingStatusEnum.ShippingNotRequired;

					Order order = InsertOrder(OrderGuid,
						 customer.CustomerID,
						 customerLanguageID,
						 customerTaxDisplayType,
						 orderSubTotalInclTax,
						 orderSubTotalExclTax,
						 orderShippingTotalInclTax.Value,
						 orderShippingTotalExclTax.Value,
						 paymentAdditionalFeeInclTax,
						 paymentAdditionalFeeExclTax,
						 orderTaxTotal,
						 orderTotal.Value,
						 orderSubTotalDiscount,
						 orderSubtotalInclTaxInCustomerCurrency,
						 orderSubtotalExclTaxInCustomerCurrency,
						 orderShippingInclTaxInCustomerCurrency,
						 orderShippingExclTaxInCustomerCurrency,
						 paymentAdditionalFeeInclTaxInCustomerCurrency,
						 paymentAdditionalFeeExclTaxInCustomerCurrency,
						 orderTaxInCustomerCurrency,
						 orderTotalInCustomerCurrency,
						 customerCurrencyCode,
						 orderWeight,
						 customer.AffiliateID,
						 OrderStatusEnum.Pending,
						 processPaymentResult.AllowStoringCreditCardNumber,
						 SecurityHelper.Encrypt(paymentInfo.CreditCardType),
						 SecurityHelper.Encrypt(paymentInfo.CreditCardName),
						 SecurityHelper.Encrypt(paymentInfo.CreditCardNumber),
						 SecurityHelper.Encrypt(PaymentManager.GetMaskedCreditCardNumber(paymentInfo.CreditCardNumber)),
						 processPaymentResult.AllowStoringCreditCardNumber ? SecurityHelper.Encrypt(paymentInfo.CreditCardCVV2) : string.Empty,
						 SecurityHelper.Encrypt(paymentInfo.CreditCardExpireMonth.ToString()),
						 SecurityHelper.Encrypt(paymentInfo.CreditCardExpireYear.ToString()),
						 0,//paymentMethod.PaymentMethodID,
						 "",//paymentMethod.Name,
						 processPaymentResult.AuthorizationTransactionID,
						 processPaymentResult.AuthorizationTransactionCode,
						 processPaymentResult.AuthorizationTransactionResult,
						 processPaymentResult.CaptureTransactionID,
						 processPaymentResult.CaptureTransactionResult,
						 paymentInfo.PurchaseOrderNumber,
						 processPaymentResult.PaymentStatus,
						 paymentInfo.BillingAddress.FirstName,
						 paymentInfo.BillingAddress.LastName,
						 paymentInfo.BillingAddress.PhoneNumber,
						 paymentInfo.BillingAddress.Email,
						 paymentInfo.BillingAddress.FaxNumber,
						 paymentInfo.BillingAddress.Company,
						 paymentInfo.BillingAddress.Address1,
						 paymentInfo.BillingAddress.Address2,
						 paymentInfo.BillingAddress.City,
						 billingStateProvince,
						 billingStateProvinceID,
						 paymentInfo.BillingAddress.ZipPostalCode,
						 billingCountry,
						 billingCountryID,
						 shippingStatusEnum,
						 shippingFirstName,
						 shippingLastName,
						 shippingPhoneNumber,
						 shippingEmail,
						 shippingFaxNumber,
						 shippingCompany,
						 shippingAddress1,
						 shippingAddress2,
						 shippingCity,
						 shippingStateProvince,
						 shippingStateProvinceID,
						 shippingZipPostalCode,
						 shippingCountry,
						 shippingCountryID,
						 shippingMethodName,
						 activeShippingRateComputationMethodID,
						 null,
						 false,
						 DateTime.Now);

					OrderID = order.OrderID;

					foreach (ShoppingCartItem sc in cart)
					{
						decimal scUnitPriceInclTax = TaxManager.GetPrice(sc.ProductVariant, PriceHelper.GetUnitPrice(sc, customer, true), true, customer);
						decimal scUnitPriceExclTax = TaxManager.GetPrice(sc.ProductVariant, PriceHelper.GetUnitPrice(sc, customer, true), false, customer);
						decimal scSubTotalInclTax = TaxManager.GetPrice(sc.ProductVariant, PriceHelper.GetSubTotal(sc, customer, true), true, customer);
						decimal scSubTotalExclTax = TaxManager.GetPrice(sc.ProductVariant, PriceHelper.GetSubTotal(sc, customer, true), false, customer);
						decimal scUnitPriceInclTaxInCustomerCurrency = CurrencyManager.ConvertCurrency(scUnitPriceInclTax, CurrencyManager.PrimaryStoreCurrency, paymentInfo.CustomerCurrency);
						decimal scUnitPriceExclTaxInCustomerCurrency = CurrencyManager.ConvertCurrency(scUnitPriceExclTax, CurrencyManager.PrimaryStoreCurrency, paymentInfo.CustomerCurrency);
						decimal scSubTotalInclTaxInCustomerCurrency = CurrencyManager.ConvertCurrency(scSubTotalInclTax, CurrencyManager.PrimaryStoreCurrency, paymentInfo.CustomerCurrency);
						decimal scSubTotalExclTaxInCustomerCurrency = CurrencyManager.ConvertCurrency(scSubTotalExclTax, CurrencyManager.PrimaryStoreCurrency, paymentInfo.CustomerCurrency);

						decimal discountAmountInclTax = TaxManager.GetPrice(sc.ProductVariant, PriceHelper.GetDiscountAmount(sc, customer), true, customer);
						decimal discountAmountExclTax = TaxManager.GetPrice(sc.ProductVariant, PriceHelper.GetDiscountAmount(sc, customer), false, customer);

						string attributeDescription = ProductAttributeHelper.FormatAttributes(sc.ProductVariant, sc.AttributesXML);

						InsertOrderProductVariant(order.OrderID,
							sc.ProductVariantID, scUnitPriceInclTax, scUnitPriceExclTax, scSubTotalInclTax, scSubTotalExclTax,
							scUnitPriceInclTaxInCustomerCurrency, scUnitPriceExclTaxInCustomerCurrency,
							scSubTotalInclTaxInCustomerCurrency, scSubTotalExclTaxInCustomerCurrency,
							attributeDescription, sc.Quantity, discountAmountInclTax, discountAmountExclTax, 0);

						ShoppingCartManager.DeleteShoppingCartItem(sc.ShoppingCartItemID, false);

						ProductManager.AdjustInventory(sc.ProductVariantID, true, sc.Quantity);
					}

					InsertOrderNote(OrderID, string.Format("Order placed"), DateTime.Now);

					//int orderPlacedStoreOwnerNotificationQueuedEmailID = MessageManager.SendOrderPlacedStoreOwnerNotification(order, LocalizationManager.DefaultAdminLanguage.LanguageID);
					//InsertOrderNote(OrderID, string.Format("\"Order placed\" email (to store owner) has been queued. Queued email identifier: {0}.", orderPlacedStoreOwnerNotificationQueuedEmailID), DateTime.Now);

					//int orderPlacedCustomerNotificationQueuedEmailID = MessageManager.SendOrderPlacedCustomerNotification(order, order.CustomerLanguageID);
					//InsertOrderNote(OrderID, string.Format("\"Order placed\" email (to customer) has been queued. Queued email identifier: {0}.", orderPlacedCustomerNotificationQueuedEmailID), DateTime.Now);

					order = CheckOrderStatus(order.OrderID);

					CustomerManager.ResetCheckoutData(customer.CustomerID, true);
				}
			}
			catch (Exception exc)
			{
				processPaymentResult.Error = exc.Message;
				processPaymentResult.FullError = exc.ToString();
			}

			if (!String.IsNullOrEmpty(processPaymentResult.Error))
			{
				LogManager.InsertLog(LogTypeEnum.OrderError, string.Format("Error while placing order. {0}", processPaymentResult.Error), processPaymentResult.FullError);
			}
			return processPaymentResult.Error;
		}

		/// <summary>
		/// Gets a value indicating whether shipping is allowed
		/// </summary>
		/// <param name="order">Order</param>
		/// <returns>A value indicating whether shipping is allowed</returns>
		public static bool CanShip(Order order)
		{
			if (order == null)
				return false;

			if (order.OrderStatus == OrderStatusEnum.Cancelled)
				return false;

			if (order.ShippingStatus == ShippingStatusEnum.NotYetShipped)
				return true;

			return false;
		}

		/// <summary>
		/// Ships order
		/// </summary>
		/// <param name="OrderID">Order identifier</param>
		/// <param name="notifyCustomer">True to notify customer</param>
		/// <returns>Updated order</returns>
		public static Order Ship(int OrderID, bool notifyCustomer)
		{
			Order order = GetOrderByID(OrderID);
			if (order == null)
				return order;

			if (!CanShip(order))
				throw new NopException("Can not do shipment for order.");

			DateTime ShippedDate = DateTimeHelper.ConvertToUtcTime(DateTime.Now);
			order = UpdateOrder(order.OrderID, order.OrderGUID, order.CustomerID, order.CustomerLanguageID,
				order.CustomerTaxDisplayType, order.OrderSubtotalInclTax, order.OrderSubtotalExclTax, order.OrderShippingInclTax,
				order.OrderShippingExclTax, order.PaymentMethodAdditionalFeeInclTax, order.PaymentMethodAdditionalFeeExclTax,
				order.OrderTax, order.OrderTotal, order.OrderDiscount,
				order.OrderSubtotalInclTaxInCustomerCurrency, order.OrderSubtotalExclTaxInCustomerCurrency,
				order.OrderShippingInclTaxInCustomerCurrency, order.OrderShippingExclTaxInCustomerCurrency,
				order.PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, order.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
				order.OrderTaxInCustomerCurrency, order.OrderTotalInCustomerCurrency, order.CustomerCurrencyCode, order.OrderWeight,
				order.AffiliateID, order.OrderStatus, order.AllowStoringCreditCardNumber, order.CardType,
				order.CardName, order.CardNumber, order.MaskedCreditCardNumber,
				order.CardCVV2, order.CardExpirationMonth, order.CardExpirationYear,
				order.PaymentMethodID, order.PaymentMethodName,
				order.AuthorizationTransactionID,
				order.AuthorizationTransactionCode, order.AuthorizationTransactionResult,
				order.CaptureTransactionID, order.CaptureTransactionResult,
				order.PurchaseOrderNumber, order.PaymentStatus,
				order.BillingFirstName, order.BillingLastName, order.BillingPhoneNumber,
				order.BillingEmail, order.BillingFaxNumber, order.BillingCompany, order.BillingAddress1,
				order.BillingAddress2, order.BillingCity,
				order.BillingStateProvince, order.BillingStateProvinceID, order.BillingZipPostalCode,
				order.BillingCountry, order.BillingCountryID, ShippingStatusEnum.Shipped,
				order.ShippingFirstName, order.ShippingLastName, order.ShippingPhoneNumber,
				order.ShippingEmail, order.ShippingFaxNumber, order.ShippingCompany,
				order.ShippingAddress1, order.ShippingAddress2, order.ShippingCity,
				order.ShippingStateProvince, order.ShippingStateProvinceID, order.ShippingZipPostalCode,
				order.ShippingCountry, order.ShippingCountryID,
				order.ShippingMethod, order.ShippingRateComputationMethodID, ShippedDate, order.Deleted,
				order.CreatedOn);

			InsertOrderNote(order.OrderID, string.Format("Order has been shipped"), DateTime.Now);

			if (notifyCustomer)
			{
				int orderShippedCustomerNotificationQueuedEmailID = MessageManager.SendOrderShippedCustomerNotification(order, order.CustomerLanguageID);
				InsertOrderNote(order.OrderID, string.Format("\"Shipped\" email (to customer) has been queued. Queued email identifier: {0}.", orderShippedCustomerNotificationQueuedEmailID), DateTime.Now);
			}

			order = CheckOrderStatus(order.OrderID);

			return order;
		}

		/// <summary>
		/// Gets a value indicating whether cancel is allowed
		/// </summary>
		/// <param name="order">Order</param>
		/// <returns>A value indicating whether cancel is allowed</returns>
		public static bool CanCancelOrder(Order order)
		{
			if (order == null)
				return false;

			if (order.OrderStatus == OrderStatusEnum.Cancelled)
				return false;

			return true;
		}

		/// <summary>
		/// Cancels order
		/// </summary>
		/// <param name="OrderID">Order identifier</param>
		/// <param name="notifyCustomer">True to notify customer</param>
		/// <returns>Cancelled order</returns>
		public static Order CancelOrder(int OrderID, bool notifyCustomer)
		{
			Order order = GetOrderByID(OrderID);
			if (order == null)
				return order;

			if (!CanCancelOrder(order))
				throw new NopException("Can not do cancel for order.");

			//TODO call payment method cancel() (mark as voided/refunded)

			order = SetOrderStatus(order.OrderID, OrderStatusEnum.Cancelled, notifyCustomer);

			InsertOrderNote(order.OrderID, string.Format("Order has been cancelled"), DateTime.Now);

			foreach (OrderProductVariant opv in order.OrderProductVariants)
				ProductManager.AdjustInventory(opv.ProductVariantID, false, opv.Quantity);

			return order;
		}

		/// <summary>
		/// Gets a value indicating whether capture from admin panel is allowed
		/// </summary>
		/// <param name="order">Order</param>
		/// <returns>A value indicating whether capture from admin panel is allowed</returns>
		public static bool CanCapture(Order order)
		{
			if (order == null)
				return false;

			if (order.OrderStatus == OrderStatusEnum.Cancelled ||
				order.OrderStatus == OrderStatusEnum.Pending)
				return false;

			if (order.PaymentStatus == PaymentStatusEnum.Authorized &&
				PaymentManager.CanCapture(order.PaymentMethodID))
				return true;

			return false;
		}

		/// <summary>
		/// Captures order (from admin panel)
		/// </summary>
		/// <param name="OrderID">Order identifier</param>
		/// <param name="Error">Error</param>
		/// <returns>Captured order</returns>
		public static Order Capture(int OrderID, ref string Error)
		{
			Order order = GetOrderByID(OrderID);
			if (order == null)
				return order;

			if (!CanCapture(order))
				throw new NopException("Can not do capture for order.");

			ProcessPaymentResult processPaymentResult = new ProcessPaymentResult();
			try
			{
				//old info from placing order
				processPaymentResult.AuthorizationTransactionID = order.AuthorizationTransactionID;
				processPaymentResult.AuthorizationTransactionCode = order.AuthorizationTransactionCode;
				processPaymentResult.AuthorizationTransactionResult = order.AuthorizationTransactionResult;
				processPaymentResult.CaptureTransactionID = order.CaptureTransactionID;
				processPaymentResult.CaptureTransactionResult = order.CaptureTransactionResult;
				processPaymentResult.PaymentStatus = order.PaymentStatus;

				PaymentManager.Capture(order, ref processPaymentResult);

				if (String.IsNullOrEmpty(processPaymentResult.Error))
				{
					InsertOrderNote(order.OrderID, string.Format("Order has been captured"), DateTime.Now);

					order = UpdateOrder(order.OrderID, order.OrderGUID, order.CustomerID, order.CustomerLanguageID,
						order.CustomerTaxDisplayType, order.OrderSubtotalInclTax, order.OrderSubtotalExclTax, order.OrderShippingInclTax,
						order.OrderShippingExclTax, order.PaymentMethodAdditionalFeeInclTax, order.PaymentMethodAdditionalFeeExclTax,
						order.OrderTax, order.OrderTotal, order.OrderDiscount,
						order.OrderSubtotalInclTaxInCustomerCurrency, order.OrderSubtotalExclTaxInCustomerCurrency,
						order.OrderShippingInclTaxInCustomerCurrency, order.OrderShippingExclTaxInCustomerCurrency,
						order.PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, order.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
						order.OrderTaxInCustomerCurrency, order.OrderTotalInCustomerCurrency, order.CustomerCurrencyCode, order.OrderWeight,
						order.AffiliateID, order.OrderStatus, order.AllowStoringCreditCardNumber,
						order.CardType, order.CardName, order.CardNumber, order.MaskedCreditCardNumber,
						order.CardCVV2, order.CardExpirationMonth, order.CardExpirationYear,
						order.PaymentMethodID, order.PaymentMethodName,
						processPaymentResult.AuthorizationTransactionID,
						processPaymentResult.AuthorizationTransactionCode,
						processPaymentResult.AuthorizationTransactionResult,
						processPaymentResult.CaptureTransactionID,
						processPaymentResult.CaptureTransactionResult,
						order.PurchaseOrderNumber,
						processPaymentResult.PaymentStatus,
						order.BillingFirstName, order.BillingLastName, order.BillingPhoneNumber,
						order.BillingEmail, order.BillingFaxNumber, order.BillingCompany, order.BillingAddress1,
						order.BillingAddress2, order.BillingCity,
						order.BillingStateProvince, order.BillingStateProvinceID, order.BillingZipPostalCode,
						order.BillingCountry, order.BillingCountryID, order.ShippingStatus,
						order.ShippingFirstName, order.ShippingLastName, order.ShippingPhoneNumber,
						order.ShippingEmail, order.ShippingFaxNumber, order.ShippingCompany,
						order.ShippingAddress1, order.ShippingAddress2, order.ShippingCity,
						order.ShippingStateProvince, order.ShippingStateProvinceID, order.ShippingZipPostalCode,
						order.ShippingCountry, order.ShippingCountryID,
						order.ShippingMethod, order.ShippingRateComputationMethodID, order.ShippedDate, order.Deleted,
						order.CreatedOn);
				}
				else
				{
					InsertOrderNote(order.OrderID, string.Format("Unable to capture order. Error: {0}", processPaymentResult.Error), DateTime.Now);

				}
				order = CheckOrderStatus(order.OrderID);
			}
			catch (Exception exc)
			{
				processPaymentResult.Error = exc.Message;
				processPaymentResult.FullError = exc.ToString();
			}

			if (!String.IsNullOrEmpty(processPaymentResult.Error))
			{
				Error = processPaymentResult.Error;
				LogManager.InsertLog(LogTypeEnum.OrderError, string.Format("Error capturing order. {0}", processPaymentResult.Error), processPaymentResult.FullError);
			}
			return order;
		}

		/// <summary>
		/// Marks order as authorized
		/// </summary>
		/// <param name="OrderID">Order identifier</param>
		/// <returns>Authorized order</returns>
		public static Order MarkAsAuthorized(int OrderID)
		{
			Order order = GetOrderByID(OrderID);
			if (order == null)
				return order;

			order = UpdateOrder(order.OrderID, order.OrderGUID, order.CustomerID, order.CustomerLanguageID,
				order.CustomerTaxDisplayType, order.OrderSubtotalInclTax, order.OrderSubtotalExclTax, order.OrderShippingInclTax,
				   order.OrderShippingExclTax, order.PaymentMethodAdditionalFeeInclTax, order.PaymentMethodAdditionalFeeExclTax,
				   order.OrderTax, order.OrderTotal, order.OrderDiscount,
				   order.OrderSubtotalInclTaxInCustomerCurrency, order.OrderSubtotalExclTaxInCustomerCurrency,
				   order.OrderShippingInclTaxInCustomerCurrency, order.OrderShippingExclTaxInCustomerCurrency,
				   order.PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, order.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
				   order.OrderTaxInCustomerCurrency, order.OrderTotalInCustomerCurrency, order.CustomerCurrencyCode, order.OrderWeight,
				   order.AffiliateID, order.OrderStatus, order.AllowStoringCreditCardNumber,
				   order.CardType, order.CardName, order.CardNumber, order.MaskedCreditCardNumber,
				   order.CardCVV2, order.CardExpirationMonth, order.CardExpirationYear,
					 order.PaymentMethodID, order.PaymentMethodName,
					 order.AuthorizationTransactionID,
					 order.AuthorizationTransactionCode, order.AuthorizationTransactionResult,
					 order.CaptureTransactionID, order.CaptureTransactionResult,
					 order.PurchaseOrderNumber, PaymentStatusEnum.Authorized,
					 order.BillingFirstName, order.BillingLastName, order.BillingPhoneNumber,
					 order.BillingEmail, order.BillingFaxNumber, order.BillingCompany, order.BillingAddress1,
					 order.BillingAddress2, order.BillingCity,
					 order.BillingStateProvince, order.BillingStateProvinceID, order.BillingZipPostalCode,
					 order.BillingCountry, order.BillingCountryID, order.ShippingStatus,
					 order.ShippingFirstName, order.ShippingLastName, order.ShippingPhoneNumber,
					 order.ShippingEmail, order.ShippingFaxNumber, order.ShippingCompany,
					 order.ShippingAddress1, order.ShippingAddress2, order.ShippingCity,
					 order.ShippingStateProvince, order.ShippingStateProvinceID, order.ShippingZipPostalCode,
					 order.ShippingCountry, order.ShippingCountryID,
					 order.ShippingMethod, order.ShippingRateComputationMethodID, order.ShippedDate, order.Deleted,
					 order.CreatedOn);

			InsertOrderNote(order.OrderID, string.Format("Order has been marked as authorized"), DateTime.Now);

			order = CheckOrderStatus(order.OrderID);

			return order;
		}

		/// <summary>
		/// Gets a value indicating whether order can be marked as paid
		/// </summary>
		/// <param name="order">Order</param>
		/// <returns>A value indicating whether order can be marked as paid</returns>
		public static bool CanMarkOrderAsPaid(Order order)
		{
			if (order == null)
				return false;

			if (order.OrderStatus == OrderStatusEnum.Cancelled)
				return false;

			if (order.PaymentStatus == PaymentStatusEnum.Paid)
				return false;

			return true;
		}

		/// <summary>
		/// Marks order as paid
		/// </summary>
		/// <param name="OrderID">Order identifier</param>
		/// <returns>Updated order</returns>
		public static Order MarkOrderAsPaid(int OrderID)
		{
			Order order = GetOrderByID(OrderID);
			if (order == null)
				return order;

			if (!CanMarkOrderAsPaid(order))
				throw new NopException("You can't mark this order as paid");

			order = UpdateOrder(order.OrderID, order.OrderGUID, order.CustomerID, order.CustomerLanguageID,
				order.CustomerTaxDisplayType, order.OrderSubtotalInclTax, order.OrderSubtotalExclTax, order.OrderShippingInclTax,
				   order.OrderShippingExclTax, order.PaymentMethodAdditionalFeeInclTax, order.PaymentMethodAdditionalFeeExclTax,
				   order.OrderTax, order.OrderTotal, order.OrderDiscount,
				   order.OrderSubtotalInclTaxInCustomerCurrency, order.OrderSubtotalExclTaxInCustomerCurrency,
				   order.OrderShippingInclTaxInCustomerCurrency, order.OrderShippingExclTaxInCustomerCurrency,
				   order.PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, order.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
				   order.OrderTaxInCustomerCurrency, order.OrderTotalInCustomerCurrency, order.CustomerCurrencyCode, order.OrderWeight,
				   order.AffiliateID, order.OrderStatus, order.AllowStoringCreditCardNumber, order.CardType,
				   order.CardName, order.CardNumber, order.MaskedCreditCardNumber,
					 order.CardCVV2, order.CardExpirationMonth, order.CardExpirationYear,
					 order.PaymentMethodID, order.PaymentMethodName,
					 order.AuthorizationTransactionID,
					 order.AuthorizationTransactionCode, order.AuthorizationTransactionResult,
					 order.CaptureTransactionID, order.CaptureTransactionResult,
					 order.PurchaseOrderNumber, PaymentStatusEnum.Paid,
					 order.BillingFirstName, order.BillingLastName, order.BillingPhoneNumber,
					 order.BillingEmail, order.BillingFaxNumber, order.BillingCompany, order.BillingAddress1,
					 order.BillingAddress2, order.BillingCity,
					 order.BillingStateProvince, order.BillingStateProvinceID, order.BillingZipPostalCode,
					 order.BillingCountry, order.BillingCountryID, order.ShippingStatus,
					 order.ShippingFirstName, order.ShippingLastName, order.ShippingPhoneNumber,
					 order.ShippingEmail, order.ShippingFaxNumber, order.ShippingCompany,
					 order.ShippingAddress1, order.ShippingAddress2, order.ShippingCity,
					 order.ShippingStateProvince, order.ShippingStateProvinceID, order.ShippingZipPostalCode,
					 order.ShippingCountry, order.ShippingCountryID,
					 order.ShippingMethod, order.ShippingRateComputationMethodID, order.ShippedDate, order.Deleted,
					 order.CreatedOn);

			InsertOrderNote(order.OrderID, string.Format("Order has been marked as paid"), DateTime.Now);

			order = CheckOrderStatus(order.OrderID);

			return order;
		}

		/// <summary>
		/// Gets a value indicating whether downloads are allowed
		/// </summary>
		/// <param name="order">Order to check</param>
		/// <returns>True if downloads are allowed; otherwise, false.</returns>
		public static bool AreDownloadsAllowed(Order order)
		{
			if (order == null)
				return false;

			if (order.OrderStatus == OrderStatusEnum.Pending || order.OrderStatus == OrderStatusEnum.Cancelled)
				return false;

			if (order.PaymentStatus == PaymentStatusEnum.Paid)
				return true;

			return false;
		}

		/// <summary>
		/// Gets an order note
		/// </summary>
		/// <param name="OrderNoteID">Order note identifier</param>
		/// <returns>Order note</returns>
		public static OrderNote GetOrderNoteByID(int OrderNoteID)
		{
			if (OrderNoteID == 0)
				return null;

			DBOrderNote dbItem = DBProviderManager<DBOrderProvider>.Provider.GetOrderNoteByID(OrderNoteID);
			OrderNote orderNote = DBMapping(dbItem);
			return orderNote;
		}

		/// <summary>
		/// Gets an order notes by order identifier
		/// </summary>
		/// <param name="OrderID">Order identifier</param>
		/// <returns>Order note collection</returns>
		public static OrderNoteCollection GetOrderNoteByOrderID(int OrderID)
		{
			DBOrderNoteCollection dbCollection = DBProviderManager<DBOrderProvider>.Provider.GetOrderNoteByOrderID(OrderID);
			OrderNoteCollection orderNotes = DBMapping(dbCollection);
			return orderNotes;
		}

		/// <summary>
		/// Deletes an order note
		/// </summary>
		/// <param name="OrderNoteID">Order note identifier</param>
		public static void DeleteOrderNote(int OrderNoteID)
		{
			DBProviderManager<DBOrderProvider>.Provider.DeleteOrderNote(OrderNoteID);
		}

		/// <summary>
		/// Inserts an order note
		/// </summary>
		/// <param name="OrderID">The order identifier</param>
		/// <param name="Note">The note</param>
		/// <param name="CreatedOn">The date and time of order note creation</param>
		/// <returns>Order note</returns>
		public static OrderNote InsertOrderNote(int OrderID, string Note, DateTime CreatedOn)
		{
			CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);

			DBOrderNote dbItem = DBProviderManager<DBOrderProvider>.Provider.InsertOrderNote(OrderID, Note, CreatedOn);
			OrderNote orderNote = DBMapping(dbItem);
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
		public static OrderNote UpdateOrderNote(int OrderNoteID, int OrderID, string Note, DateTime CreatedOn)
		{
			CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);

			DBOrderNote dbItem = DBProviderManager<DBOrderProvider>.Provider.UpdateOrderNote(OrderNoteID, OrderID, Note, CreatedOn);
			OrderNote orderNote = DBMapping(dbItem);
			return orderNote;
		}

		/// <summary>
		/// Gets an order product variant
		/// </summary>
		/// <param name="OrderProductVariantID">Order product variant identifier</param>
		/// <returns>Order product variant</returns>
		public static OrderProductVariant GetOrderProductVariantByID(int OrderProductVariantID)
		{
			if (OrderProductVariantID == 0)
				return null;

			DBOrderProductVariant dbItem = DBProviderManager<DBOrderProvider>.Provider.GetOrderProductVariantByID(OrderProductVariantID);
			OrderProductVariant orderProductVariant = DBMapping(dbItem);
			return orderProductVariant;
		}

		/// <summary>
		/// Gets an order product variants by the order identifier
		/// </summary>
		/// <param name="OrderID">The order identifier</param>
		/// <returns>Order product variant collection</returns>
		public static OrderProductVariantCollection GetOrderProductVariantsByOrderID(int OrderID)
		{
			DBOrderProductVariantCollection dbCollection = DBProviderManager<DBOrderProvider>.Provider.GetOrderProductVariantsByOrderID(OrderID);
			OrderProductVariantCollection orderProductVariants = DBMapping(dbCollection);
			return orderProductVariants;
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
		public static OrderProductVariant InsertOrderProductVariant(int OrderID, int ProductVariantID,
			decimal UnitPriceInclTax, decimal UnitPriceExclTax, decimal PriceInclTax, decimal PriceExclTax,
			decimal UnitPriceInclTaxInCustomerCurrency, decimal UnitPriceExclTaxInCustomerCurrency,
			decimal PriceInclTaxInCustomerCurrency, decimal PriceExclTaxInCustomerCurrency,
			string AttributeDescription, int Quantity,
			decimal DiscountAmountInclTax, decimal DiscountAmountExclTax, int DownloadCount)
		{
			if (AttributeDescription == null)
				AttributeDescription = string.Empty;

			DBOrderProductVariant dbItem = DBProviderManager<DBOrderProvider>.Provider.InsertOrderProductVariant(OrderID,
				ProductVariantID, UnitPriceInclTax, UnitPriceExclTax, PriceInclTax, PriceExclTax,
				UnitPriceInclTaxInCustomerCurrency, UnitPriceExclTaxInCustomerCurrency,
				PriceInclTaxInCustomerCurrency, PriceExclTaxInCustomerCurrency,
				AttributeDescription, Quantity, DiscountAmountInclTax,
				DiscountAmountExclTax, DownloadCount);
			OrderProductVariant log = DBMapping(dbItem);
			return log;
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
		/// <param name="DownloadCount">The downloads count</param>
		/// <returns>Order product variant</returns>
		public static OrderProductVariant UpdateOrderProductVariant(int OrderProductVariantID,
			int OrderID, int ProductVariantID,
			decimal UnitPriceInclTax, decimal UnitPriceExclTax, decimal PriceInclTax, decimal PriceExclTax,
			decimal UnitPriceInclTaxInCustomerCurrency, decimal UnitPriceExclTaxInCustomerCurrency,
			decimal PriceInclTaxInCustomerCurrency, decimal PriceExclTaxInCustomerCurrency,
			string AttributeDescription, int Quantity,
			decimal DiscountAmountInclTax, decimal DiscountAmountExclTax, int DownloadCount)
		{
			if (AttributeDescription == null)
				AttributeDescription = string.Empty;

			DBOrderProductVariant dbItem = DBProviderManager<DBOrderProvider>.Provider.UpdateOrderProductVariant(
				OrderProductVariantID, OrderID,
				ProductVariantID, UnitPriceInclTax, UnitPriceExclTax, PriceInclTax, PriceExclTax,
				UnitPriceInclTaxInCustomerCurrency, UnitPriceExclTaxInCustomerCurrency,
				PriceInclTaxInCustomerCurrency, PriceExclTaxInCustomerCurrency,
				AttributeDescription, Quantity, DiscountAmountInclTax,
				DiscountAmountExclTax, DownloadCount);
			OrderProductVariant log = DBMapping(dbItem);
			return log;
		}

		/// <summary>
		/// Increase an order product variant download count
		/// </summary>
		/// <param name="OrderProductVariantID">Order product variant identifier</param>
		/// <returns>Order product variant</returns>
		public static OrderProductVariant IncreaseOrderProductDownloadCount(int OrderProductVariantID)
		{
			OrderProductVariant orderProductVariant = GetOrderProductVariantByID(OrderProductVariantID);
			if (orderProductVariant == null)
				throw new NopException("Order product variant could not be loaded");

			int newDownloadCount = orderProductVariant.DownloadCount + 1;

			orderProductVariant = UpdateOrderProductVariant(orderProductVariant.OrderProductVariantID,
				orderProductVariant.OrderID, orderProductVariant.ProductVariantID,
				orderProductVariant.UnitPriceInclTax, orderProductVariant.UnitPriceExclTax,
				orderProductVariant.PriceInclTax, orderProductVariant.PriceExclTax,
				orderProductVariant.UnitPriceInclTaxInCustomerCurrency, orderProductVariant.UnitPriceExclTaxInCustomerCurrency,
				orderProductVariant.PriceInclTaxInCustomerCurrency, orderProductVariant.PriceExclTaxInCustomerCurrency,
				orderProductVariant.AttributeDescription, orderProductVariant.Quantity,
				orderProductVariant.DiscountAmountInclTax, orderProductVariant.DiscountAmountExclTax, newDownloadCount);

			return orderProductVariant;
		}

		/// <summary>
		/// Gets an order status full name
		/// </summary>
		/// <param name="OrderStatusID">Order status identifier</param>
		/// <returns>Order status name</returns>
		public static string GetOrderStatusName(int OrderStatusID)
		{
			OrderStatus orderStatus = GetOrderStatusByID(OrderStatusID);
			if (orderStatus != null)
				return orderStatus.Name;
			else
				return ((OrderStatusEnum)OrderStatusID).ToString();
		}

		/// <summary>
		/// Gets an order status by ID
		/// </summary>
		/// <param name="OrderStatusID">Order status identifier</param>
		/// <returns>Order status</returns>
		public static OrderStatus GetOrderStatusByID(int OrderStatusID)
		{
			if (OrderStatusID == 0)
				return null;

			string key = string.Format(ORDERSTATUSES_BY_ID_KEY, OrderStatusID);
			object obj2 = NopCache.Get(key);
			if (OrderManager.CacheEnabled && (obj2 != null))
			{
				return (OrderStatus)obj2;
			}

			DBOrderStatus dbItem = DBProviderManager<DBOrderProvider>.Provider.GetOrderStatusByID(OrderStatusID);
			OrderStatus orderStatus = DBMapping(dbItem);

			if (OrderManager.CacheEnabled)
			{
				NopCache.Max(key, orderStatus);
			}
			return orderStatus;
		}

		/// <summary>
		/// Gets all order statuses
		/// </summary>
		/// <returns>Order status collection</returns>
		public static OrderStatusCollection GetAllOrderStatuses()
		{
			string key = string.Format(ORDERSTATUSES_ALL_KEY);
			object obj2 = NopCache.Get(key);
			if (OrderManager.CacheEnabled && (obj2 != null))
			{
				return (OrderStatusCollection)obj2;
			}

			DBOrderStatusCollection dbCollection = DBProviderManager<DBOrderProvider>.Provider.GetAllOrderStatuses();
			OrderStatusCollection orderStatusCollection = DBMapping(dbCollection);

			if (OrderManager.CacheEnabled)
			{
				NopCache.Max(key, orderStatusCollection);
			}
			return orderStatusCollection;
		}

		/// <summary>
		/// Formats the order note text
		/// </summary>
		/// <param name="Text">Text</param>
		/// <returns>Formatted text</returns>
		public static string FormatOrderNoteText(string Text)
		{
			if (String.IsNullOrEmpty(Text))
				return string.Empty;

			Text = HtmlHelper.FormatText(Text, false, true, false, false, false, false);
			return Text;
		}

		/// <summary>
		/// Gets an order report
		/// </summary>
		/// <param name="OS">Order status; null to load all orders</param>
		/// <param name="PS">Order payment status; null to load all orders</param>
		/// <param name="SS">Order shippment status; null to load all orders</param>
		/// <returns>IDataReader</returns>
		public static IDataReader GetOrderReport(OrderStatusEnum? OS, PaymentStatusEnum? PS, ShippingStatusEnum? SS)
		{
			int? orderStatusID = null;
			if (OS.HasValue)
				orderStatusID = (int)OS.Value;

			int? paymentStatusID = null;
			if (PS.HasValue)
				paymentStatusID = (int)PS.Value;

			int? shippmentStatusID = null;
			if (SS.HasValue)
				shippmentStatusID = (int)SS.Value;

			return DBProviderManager<DBOrderProvider>.Provider.GetOrderReport(orderStatusID, paymentStatusID, shippmentStatusID);
		}
		#endregion

		#region Property
		/// <summary>
		/// Gets a value indicating whether cache is enabled
		/// </summary>
		public static bool CacheEnabled
		{
			get
			{
				return SettingManager.GetSettingValueBoolean("Cache.OrderManager.CacheEnabled");
			}
		}
		#endregion
	}
}