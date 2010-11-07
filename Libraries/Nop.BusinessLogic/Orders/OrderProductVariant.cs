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
using System.Text;
using NopSolutions.NopCommerce.BusinessLogic.Products;


namespace NopSolutions.NopCommerce.BusinessLogic.Orders
{
    /// <summary>
    /// Represents an order product variant
    /// </summary>
    public partial class OrderProductVariant : BaseEntity
    {
        #region Ctor
        /// <summary>
        /// Creates a new instance of the OrderProductVariant class
        /// </summary>
        public OrderProductVariant()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the order product variant identifier
        /// </summary>
        public int OrderProductVariantID { get; set; }

        /// <summary>
        /// Gets or sets the order identifier
        /// </summary>
        public int OrderID { get; set; }

        /// <summary>
        /// Gets or sets the product variant identifier
        /// </summary>
        public int ProductVariantID { get; set; }

        /// <summary>
        /// Gets or sets the unit price in primary store currency (incl tax)
        /// </summary>
        public decimal UnitPriceInclTax { get; set; }

        /// <summary>
        /// Gets or sets the unit price in primary store currency (excl tax)
        /// </summary>
        public decimal UnitPriceExclTax { get; set; }

        /// <summary>
        /// Gets or sets the price in primary store currency (incl tax)
        /// </summary>
        public decimal PriceInclTax { get; set; }

        /// <summary>
        /// Gets or sets the price in primary store currency (excl tax)
        /// </summary>
        public decimal PriceExclTax { get; set; }

        /// <summary>
        /// Gets or sets the unit price in customer currency (incl tax)
        /// </summary>
        public decimal UnitPriceInclTaxInCustomerCurrency { get; set; }

        /// <summary>
        /// Gets or sets the unit price in customer currency (excl tax)
        /// </summary>
        public decimal UnitPriceExclTaxInCustomerCurrency { get; set; }

        /// <summary>
        /// Gets or sets the price in customer currency (incl tax)
        /// </summary>
        public decimal PriceInclTaxInCustomerCurrency { get; set; }

        /// <summary>
        /// Gets or sets the price in customer currency (excl tax)
        /// </summary>
        public decimal PriceExclTaxInCustomerCurrency { get; set; }

        /// <summary>
        /// Gets or sets the attribute description
        /// </summary>
        public string AttributeDescription { get; set; }

        /// <summary>
        /// Gets or sets the quantity
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the discount amount (incl tax)
        /// </summary>
        public decimal DiscountAmountInclTax { get; set; }

        /// <summary>
        /// Gets or sets the discount amount (excl tax)
        /// </summary>
        public decimal DiscountAmountExclTax { get; set; }
        
        /// <summary>
        /// Gets or sets the download count
        /// </summary>
        public int DownloadCount { get; set; }
        #endregion

        #region Custom Properties
        /// <summary>
        /// Gets the order
        /// </summary>
        public Order Order
        {
            get
            {
                return OrderManager.GetOrderByID(OrderID);
            }
        }

        /// <summary>
        /// Gets the product variant
        /// </summary>
        public ProductVariant ProductVariant
        {
            get
            {
                return ProductManager.GetProductVariantByID(ProductVariantID);
            }
        }
        #endregion
    }
}
