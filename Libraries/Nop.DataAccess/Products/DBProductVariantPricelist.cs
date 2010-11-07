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
using NopSolutions.NopCommerce.DataAccess;

namespace NopSolutions.NopCommerce.DataAccess.Products
{
    /// <summary>
    /// Represents a ProductVariantPricelist
    /// </summary>
    public partial class DBProductVariantPricelist : BaseDBEntity
    {
        #region Ctor
        /// <summary>
		/// Creates a new instance of the DBProductVariantPricelist class
		/// </summary>
		public DBProductVariantPricelist()
		{
		
		}
		#endregion
				
		#region Properties
        /// <summary>
        /// Gets or sets the product variant pricelist identifier
        /// </summary>
        public int ProductVariantPricelistID { get; set; }

        /// <summary>
        /// Gets or sets the product variant identifer
        /// </summary>
        public int ProductVariantID { get; set; }

        /// <summary>
        /// Gets or sets the pricelist identifier
        /// </summary>
        public int PricelistID { get; set; }

        /// <summary>
        /// Gets or sets the type of price adjustment (if used) (relative or absolute)
        /// </summary>
        public int PriceAdjustmentTypeID { get; set; }

        /// <summary>
        /// Gets or sets the price will be adjusted by this amount (in accordance with PriceAdjustmentType)
        /// </summary>
        public decimal PriceAdjustment { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance update
        /// </summary>
        public DateTime UpdatedOn { get; set; }
		
		#endregion
	}
}

