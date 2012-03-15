using System;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Orders;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Profile;

namespace NopSolutions.NopCommerce.BusinessLogic.Orders
{
    public partial class ViewedItemManager
    {
        #region Utilities
        private static ViewedItemsCollection DBMapping(DBViewedItemsCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            ViewedItemsCollection collection = new ViewedItemsCollection();
            foreach (DBViewedItem dbItem in dbCollection)
            {
                ViewedItem item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ViewedItem DBMapping(DBViewedItem dbItem)
        {
            if (dbItem == null)
                return null;

            ViewedItem item = new ViewedItem();
            item.ViewedItemID = dbItem.ViewedItemID;
            item.CustomerSessionGUID = dbItem.CustomerSessionGUID;
            item.ProductVariantID = dbItem.ProductVariantID;
            item.CreatedOn = dbItem.CreatedOn;

            return item;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a viewed item
        /// </summary>
        /// <param name="viewedItemID">The shopping cart item identifier</param>
        /// <param name="ResetCheckoutData">A value indicating whether to reset checkout data</param>
        public static void DeleteViewedItem(int viewedItemID, bool ResetCheckoutData)
        {
            if (ResetCheckoutData)
            {
                if (NopContext.Current.Session != null)
                {
                    CustomerManager.ResetCheckoutData(NopContext.Current.Session.CustomerID, false);
                }
            }

            ViewedItem viewedItem = GetViewedItemByID(viewedItemID);
            if (viewedItem != null)
            {
                DBProviderManager<DBViewedItemProvider>.Provider.DeleteViewedItem(viewedItemID);
            }
        }

        /// <summary>
        /// Gets a viewed items by customer session GUID
        /// </summary>
        /// <param name="CustomerSessionGUID">The customer session identifier</param>
        /// <param name="Count">count items per page</param>
        /// <param name="PageIndex">number of page</param>
        /// <returns>Cart</returns>
        public static ViewedItemsCollection GetViewedItemsByCustomerSessionGUID(Guid CustomerSessionGUID, int Count,
                                                                                int PageIndex, out int TotalRecords)
        {
            DBViewedItemsCollection dbCollection = DBProviderManager<DBViewedItemProvider>.Provider.GetViewedItemByCustomerSessionGUID(CustomerSessionGUID, Count, PageIndex, out TotalRecords);
            ViewedItemsCollection viewedItemsCollection = DBMapping(dbCollection);
            return viewedItemsCollection;
        }

        /// <summary>
        /// Gets viewed item
        /// </summary>
        /// <param name="viewedItemID">The viewed item identifier</param>
        /// <returns>Viewed item</returns>
        public static ViewedItem GetViewedItemByID(int viewedItemID)
        {
            if (viewedItemID == 0)
                return null;

            DBViewedItem dbItem = DBProviderManager<DBViewedItemProvider>.Provider.GetViewedItemByID(viewedItemID);
            ViewedItem viewedItem = DBMapping(dbItem);
            return viewedItem;
        }

        /// <summary>
        /// Inserts a viewed item
        /// </summary>
        /// <param name="CustomerSessionGUID">The customer session identifier</param>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>Viewed item</returns>
        public static ViewedItem InsertViewedItem(Guid CustomerSessionGUID,
                                                  int ProductVariantID, DateTime CreatedOn)
        {
            if (CustomerSessionGUID == null || ProductVariantID == null)
                return null;

            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);

            ViewedItem itemFromBase = GetViewedItemByProductVariantID(ProductVariantID);
            DBViewedItem dbItem = null;
            if (itemFromBase == null)
            {
                dbItem = DBProviderManager<DBViewedItemProvider>.Provider.InsertViewedItem(CustomerSessionGUID,
                                                         ProductVariantID, CreatedOn);
            }
            else 
            {
                if (itemFromBase.CreatedOn < CreatedOn)
                {
                    dbItem = DBProviderManager<DBViewedItemProvider>.Provider.UpdateViewedItem(itemFromBase.ViewedItemID, 
                                                          itemFromBase.CustomerSessionGUID, ProductVariantID, CreatedOn);
                }
            }
            ViewedItem viewedItem = DBMapping(dbItem);
            return viewedItem;
        }

        /// <summary>
        /// Gets current user viewedItems
        /// </summary>
        /// <param name="Count">count items per page</param>
        /// <param name="PageIndex">number of page</param>
        /// <returns>Viewed items</returns>
        public static ViewedItemsCollection GetCurrentViewedItem(int Count, int PageIndex, out int TotalRecords)
        {
            if (NopContext.Current.Session == null)
            {
                TotalRecords = 0;
                return new ViewedItemsCollection();
            }
            Guid CustomerSessionGUID = NopContext.Current.Session.CustomerSessionGUID;
            return GetViewedItemsByCustomerSessionGUID(CustomerSessionGUID, Count, PageIndex, out TotalRecords);
        }


        /// <summary>
        /// Updates the viewed item
        /// </summary>
        /// <param name="ViewedItemID">The viewed item identifier</param>
        /// <param name="CustomerSessionGUID">The customer session identifier</param>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>Shopping cart item</returns>
        internal static ViewedItem UpdateShoppingCartItemint(int ViewedItemID,Guid CustomerSessionGUID,
                                                                  int ProductVariantID, DateTime CreatedOn)
        {
            if (ViewedItemID == 0)
                return null;

            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);

            DBViewedItem dbItem = DBProviderManager<DBViewedItemProvider>.Provider.UpdateViewedItem(ViewedItemID, CustomerSessionGUID,
                                       ProductVariantID, CreatedOn);

            ViewedItem viewedItem = DBMapping(dbItem);
            return viewedItem;
        }

        /// <summary>
        /// Gets viewed item
        /// </summary>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <returns>Viewed item</returns>
        public static ViewedItem GetViewedItemByProductVariantID(int ProductVariantID)
        {
            if (ProductVariantID == 0)
                return null;

            if (NopContext.Current.Session == null)
            {
                return new ViewedItem();
            }

            Guid CustomerSessionGUID = NopContext.Current.Session.CustomerSessionGUID;

            DBViewedItem dbItem = DBProviderManager<DBViewedItemProvider>.Provider.ViewedItemLoadByProductVariantID(ProductVariantID, CustomerSessionGUID);
            ViewedItem viewedItem = DBMapping(dbItem);
            return viewedItem;
        }

        #endregion
    }
}
