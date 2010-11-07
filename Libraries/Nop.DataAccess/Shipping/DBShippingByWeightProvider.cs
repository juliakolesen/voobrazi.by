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
using System.Web.Configuration;
using System.Web.Hosting;
using System.Collections.Specialized;

namespace NopSolutions.NopCommerce.DataAccess.Shipping
{
    /// <summary>
    /// Acts as a base class for deriving custom ShippingByWeight provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/ShippingByWeightProvider")]
    public abstract partial class DBShippingByWeightProvider : BaseDBProvider
    {
        #region Methods
        /// <summary>
        /// Gets a ShippingByWeight
        /// </summary>
        /// <param name="ShippingByWeightID">ShippingByWeight identifier</param>
        /// <returns>ShippingByWeight</returns>
        public abstract DBShippingByWeight GetByID(int ShippingByWeightID);

        /// <summary>
        /// Deletes a ShippingByWeight
        /// </summary>
        /// <param name="ShippingByWeightID">ShippingByWeight identifier</param>
        public abstract void DeleteShippingByWeight(int ShippingByWeightID);

        /// <summary>
        /// Gets all ShippingByWeights
        /// </summary>
        /// <returns>ShippingByWeight collection</returns>
        public abstract DBShippingByWeightCollection GetAll();

        /// <summary>
        /// Inserts a ShippingByWeight
        /// </summary>
        /// <param name="ShippingMethodID">The shipping method identifier</param>
        /// <param name="From">The "from" value</param>
        /// <param name="To">The "to" value</param>
        /// <param name="UsePercentage">A value indicating whether to use percentage</param>
        /// <param name="ShippingChargePercentage">The shipping charge percentage</param>
        /// <param name="ShippingChargeAmount">The shipping charge amount</param>
        /// <returns>ShippingByWeight</returns>
        public abstract DBShippingByWeight InsertShippingByWeight(int ShippingMethodID, decimal From, decimal To,
            bool UsePercentage, decimal ShippingChargePercentage, decimal ShippingChargeAmount);
        
        /// <summary>
        /// Updates the ShippingByWeight
        /// </summary>
        /// <param name="ShippingByWeightID">The ShippingByWeight identifier</param>
        /// <param name="ShippingMethodID">The shipping method identifier</param>
        /// <param name="From">The "from" value</param>
        /// <param name="To">The "to" value</param>
        /// <param name="UsePercentage">A value indicating whether to use percentage</param>
        /// <param name="ShippingChargePercentage">The shipping charge percentage</param>
        /// <param name="ShippingChargeAmount">The shipping charge amount</param>
        /// <returns>ShippingByWeight</returns>
        public abstract DBShippingByWeight UpdateShippingByWeight(int ShippingByWeightID, int ShippingMethodID, decimal From, decimal To,
            bool UsePercentage, decimal ShippingChargePercentage, decimal ShippingChargeAmount);

        /// <summary>
        /// Gets all ShippingByWeights by shipping method identifier
        /// </summary>
        /// <param name="ShippingMethodID">The shipping method identifier</param>
        /// <returns>ShippingByWeight collection</returns>
        public abstract DBShippingByWeightCollection GetAllByShippingMethodID(int ShippingMethodID);
        #endregion
    }
}
