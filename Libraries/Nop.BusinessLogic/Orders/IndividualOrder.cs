using System;

namespace NopSolutions.NopCommerce.BusinessLogic.Orders
{
    /// <summary>
    /// Represents a individual order
    /// </summary>
    public partial class IndividualOrder : BaseEntity
    {
        #region Ctor
        /// <summary>
        /// Creates a new instance of the individual order class
        /// </summary>
        public IndividualOrder()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the individual order identifier
        /// </summary>
        public int IndividualOrderID { get; set; }

        /// <summary>
        /// Gets or sets the shopping cart identifier
        /// </summary>
        public Guid CurrentUserSessionGuid { get; set; }

        /// <summary>
        /// Gets or sets the price of order
        /// </summary>
        public long Price { get; set; }

        /// <summary>
        /// Gets or sets serial number of individual order inside shopping cart
        /// </summary>
        public int SerialNumberInShCart { get; set; }

        /// <summary>
        /// Gets or sets text of individual order
        /// </summary>
        public String OrderText { get; set; }

        #endregion
    }
}

   