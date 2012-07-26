using System;

namespace NopSolutions.NopCommerce.DataAccess.Orders
{
    /// <summary>
    /// Acts as a base class for deriving custom shopping cart provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/IndividualOrderProvider")]
    public abstract partial class DBIndividualOrderProvider : BaseDBProvider
    {
        #region Methods
        /// <summary>
        /// Deletes a individual order
        /// </summary>
        /// <param name="ViewedItemID">The individual order identifier</param>
        public abstract void DeleteIndividualOrder(int IndividualOrderID);

        /// <summary>
        /// Gets a viewed by customer session GUID
        /// </summary>
        /// <param name="ShoppingCartID">The shopping Cart ID</param>
        public abstract DBIndividualOrderCollection IndividualOrderGetByCustomerSessionGUID(Guid userGuid);

        /// <summary>
        /// Inserts a individual order
        /// </summary>
        /// <param name="ShoppingCartID">The shopping cart ID</param>
        /// <param name="Price">The price of individual order</param>
        /// <param name="OrderText">TText of individual order</param>
        /// <returns>Viewed item</returns>
        public abstract DBIndividualOrder InsertIndividualOrder(Guid userGuid,
                                                  long Price, String OrderText);

        /// <summary>
        /// Gets a individual order
        /// </summary>
        /// <param name="individualOrderID">The individual order identifier</param>
        /// <returns>Viewed item</returns>
        public abstract DBIndividualOrder GetIndividualOrderByID(int individualOrderID);

        #endregion
    }
}
