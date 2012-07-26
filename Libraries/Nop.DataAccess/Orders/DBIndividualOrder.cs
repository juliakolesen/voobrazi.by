using System;

namespace NopSolutions.NopCommerce.DataAccess.Orders
{
    /// <summary>
    /// Represents a individual order item
    /// </summary>
    public partial class DBIndividualOrder : BaseDBEntity
    {
        #region Ctor
        /// <summary>
        /// Creates a new instance of DBIndividualOrder class
        /// </summary>
        public DBIndividualOrder()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the viewed cart item identifier
        /// </summary>
        public int IndividualOrderID { get; set; }

        /// <summary>
        /// Gets or sets the shopping cart identifier
        /// </summary>
        public Guid CurrentUserSessionGuid { get; set; }

        /// <summary>
        /// Gets or sets the price of individual order
        /// </summary>
        public long Price { get; set; }

        /// <summary>
        /// Gets or sets serial number in shopping cart
        /// </summary>
        public int SerialNumberInShCart { get; set; }

        /// <summary>
        /// gets or sets text of individual order
        /// </summary>
        public String OrderText { get; set; }
        #endregion 
    }
}
