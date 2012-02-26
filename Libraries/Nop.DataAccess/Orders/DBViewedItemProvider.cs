using System;

namespace NopSolutions.NopCommerce.DataAccess.Orders
{
    /// <summary>
    /// Acts as a base class for deriving custom shopping cart provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/ViewedItemProvider")]
    public abstract partial class DBViewedItemProvider : BaseDBProvider
    {
        #region Methods
        /// <summary>
        /// Deletes a viewed item
        /// </summary>
        /// <param name="ViewedItemID">The viewed item identifier</param>
        public abstract void DeleteViewedItem(int ViewedItemID);

        /// <summary>
        /// Gets a viewed by customer session GUID
        /// </summary>
        /// <param name="CustomerSessionGUID">The customer session identifier</param>
        /// <param name="PageSize">pageSize</param>
        /// <param name="pageIndex">pageIndex</param>
        /// <returns>DBViewedItemsCollection</returns>
        public abstract DBViewedItemsCollection GetViewedItemByCustomerSessionGUID(Guid CustomerSessionGUID, int PageSize, int PageIndex, out int TotalRecords);

        /// <summary>
        /// Inserts a viewed item
        /// </summary>
        /// <param name="CustomerSessionGUID">The customer session identifier</param>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>Viewed item</returns>
        public abstract DBViewedItem InsertViewedItem(Guid CustomerSessionGUID,
                                                  int ProductVariantID, DateTime CreatedOn);

        /// <summary>
        /// Gets a viewed item
        /// </summary>
        /// <param name="viewedItemID">The shopping cart item identifier</param>
        /// <returns>Viewed item</returns>
        public abstract DBViewedItem GetViewedItemByID(int viewedItemID);

        /// <summary>
        /// Updates the viewed item
        /// </summary>
        /// <param name="ViewedItemID">The viewed item identifier</param>
        /// <param name="CustomerSessionGUID">The customer session identifier</param>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>Viewed item</returns>
        public abstract DBViewedItem UpdateViewedItem(int ViewedItemID,Guid CustomerSessionGUID,
                                                                  int ProductVariantID, DateTime CreatedOn);

        /// <summary>
        /// Gets viewed item by the product variant identifier
        /// </summary>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <returns></returns>
        public abstract DBViewedItem ViewedItemLoadByProductVariantID(int ProductVariantID);
        #endregion
    }
}

