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
using System.Configuration.Provider;
using System.Configuration;
using System.Web.Hosting;
using System.Web.Configuration;
using System.Collections.Specialized;

namespace NopSolutions.NopCommerce.DataAccess.Shipping
{
    /// <summary>
    /// Acts as a base class for deriving custom ShippingByTotal provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/ShippingByTotalProvider")]
    public abstract partial class DBShippingByTotalProvider : BaseDBProvider
    {
        #region Methods
        /// <summary>
        /// Get a ShippingByTotal
        /// </summary>
        /// <param name="ShippingByTotalID">ShippingByTotal identifier</param>
        /// <returns>ShippingByTotal</returns>
        public abstract DBShippingByTotal GetByID(int ShippingByTotalID);

        /// <summary>
        /// Deletes a ShippingByTotal
        /// </summary>
        /// <param name="ShippingByTotalID">ShippingByTotal identifier</param>
        public abstract void DeleteShippingByTotal(int ShippingByTotalID);

        /// <summary>
        /// Gets all ShippingByTotals
        /// </summary>
        /// <returns>ShippingByTotal collection</returns>
        public abstract DBShippingByTotalCollection GetAll();

        /// <summary>
        /// Inserts a ShippingByTotal
        /// </summary>
        /// <param name="ShippingMethodID">The shipping method identifier</param>
        /// <param name="From">The "from" value</param>
        /// <param name="To">The "to" value</param>
        /// <param name="UsePercentage">A value indicating whether to use percentage</param>
        /// <param name="ShippingChargePercentage">The shipping charge percentage</param>
        /// <param name="ShippingChargeAmount">The shipping charge amount</param>
        /// <returns>ShippingByTotal</returns>
        public abstract DBShippingByTotal InsertShippingByTotal(int ShippingMethodID, decimal From, decimal To,
            bool UsePercentage, decimal ShippingChargePercentage, decimal ShippingChargeAmount);

        /// <summary>
        /// Updates the ShippingByTotal
        /// </summary>
        /// <param name="ShippingByTotalID">The ShippingByTotal identifier</param>
        /// <param name="ShippingMethodID">The shipping method identifier</param>
        /// <param name="From">The "from" value</param>
        /// <param name="To">The "to" value</param>
        /// <param name="UsePercentage">A value indicating whether to use percentage</param>
        /// <param name="ShippingChargePercentage">The shipping charge percentage</param>
        /// <param name="ShippingChargeAmount">The shipping charge amount</param>
        /// <returns>ShippingByTotal</returns>
        public abstract DBShippingByTotal UpdateShippingByTotal(int ShippingByTotalID, int ShippingMethodID, decimal From, decimal To,
            bool UsePercentage, decimal ShippingChargePercentage, decimal ShippingChargeAmount);

        /// <summary>
        /// Gets all ShippingByTotals by shipping method identifier
        /// </summary>
        /// <param name="ShippingMethodID">The shipping method identifier</param>
        /// <returns>ShippingByTotal collection</returns>
        public abstract DBShippingByTotalCollection GetAllByShippingMethodID(int ShippingMethodID);
        #endregion
    }
}
