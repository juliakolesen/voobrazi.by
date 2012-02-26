using System;

namespace NopSolutions.NopCommerce.DataAccess.Orders
{
    /// <summary>
    /// Represents a shopping cart item
    /// </summary>
    public partial class DBViewedItem : BaseDBEntity
    {
        #region Ctor
        /// <summary>
        /// Creates a new instance of DBShoppingCartItem class
        /// </summary>
        public DBViewedItem()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the viewed cart item identifier
        /// </summary>
        public int ViewedItemID { get; set; }

        /// <summary>
        /// Gets or sets the customer session identifier
        /// </summary>
        public Guid CustomerSessionGUID { get; set; }

        /// <summary>
        /// Gets or sets the product variant identifier
        /// </summary>
        public int ProductVariantID { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        public DateTime CreatedOn { get; set; }
        #endregion 
    }
}

