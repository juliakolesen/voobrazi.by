using System;

namespace NopSolutions.NopCommerce.BusinessLogic.Orders
{
    /// <summary>
    /// Represents a viewed item
    /// </summary>
    public partial class ViewedItem : BaseEntity
    {
        #region Ctor
        /// <summary>
        /// Creates a new instance of the viewed item class
        /// </summary>
        public ViewedItem()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the viewed item identifier
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
