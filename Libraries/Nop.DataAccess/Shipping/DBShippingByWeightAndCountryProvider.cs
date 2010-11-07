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
using System.Web.Hosting;
using System.Web.Configuration;
using System.Collections.Specialized;
using System.Configuration;

namespace NopSolutions.NopCommerce.DataAccess.Shipping
{
    /// <summary>
    /// Acts as a base class for deriving custom ShippingByWeightAndCountry provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/ShippingByWeightAndCountryProvider")]
    public abstract partial class DBShippingByWeightAndCountryProvider : BaseDBProvider
    {
        #region Methods
        /// <summary>
        /// Gets a ShippingByWeightAndCountry
        /// </summary>
        /// <param name="ShippingByWeightAndCountryID">ShippingByWeightAndCountry identifier</param>
        /// <returns>ShippingByWeightAndCountry</returns>
        public abstract DBShippingByWeightAndCountry GetByID(int ShippingByWeightAndCountryID);

        /// <summary>
        /// Deletes a ShippingByWeightAndCountry
        /// </summary>
        /// <param name="ShippingByWeightAndCountryID">ShippingByWeightAndCountry identifier</param>
        public abstract void DeleteShippingByWeightAndCountry(int ShippingByWeightAndCountryID);

        /// <summary>
        /// Gets all ShippingByWeightAndCountrys
        /// </summary>
        /// <returns>ShippingByWeightAndCountry collection</returns>
        public abstract DBShippingByWeightAndCountryCollection GetAll();

        /// <summary>
        /// Inserts a ShippingByWeightAndCountry
        /// </summary>
        /// <param name="ShippingMethodID">The shipping method identifier</param>
        /// <param name="CountryID">The country identifier</param>
        /// <param name="From">The "from" value</param>
        /// <param name="To">The "to" value</param>
        /// <param name="UsePercentage">A value indicating whether to use percentage</param>
        /// <param name="ShippingChargePercentage">The shipping charge percentage</param>
        /// <param name="ShippingChargeAmount">The shipping charge amount</param>
        /// <returns>ShippingByWeightAndCountry</returns>
        public abstract DBShippingByWeightAndCountry InsertShippingByWeightAndCountry(int ShippingMethodID,
            int CountryID, decimal From, decimal To,
            bool UsePercentage, decimal ShippingChargePercentage, decimal ShippingChargeAmount);

        /// <summary>
        /// Updates the ShippingByWeightAndCountry
        /// </summary>
        /// <param name="ShippingByWeightAndCountryID">The ShippingByWeightAndCountry identifier</param>
        /// <param name="ShippingMethodID">The shipping method identifier</param>
        /// <param name="CountryID">The country identifier</param>
        /// <param name="From">The "from" value</param>
        /// <param name="To">The "to" value</param>
        /// <param name="UsePercentage">A value indicating whether to use percentage</param>
        /// <param name="ShippingChargePercentage">The shipping charge percentage</param>
        /// <param name="ShippingChargeAmount">The shipping charge amount</param>
        /// <returns>ShippingByWeightAndCountry</returns>
        public abstract DBShippingByWeightAndCountry UpdateShippingByWeightAndCountry(int ShippingByWeightAndCountryID,
            int ShippingMethodID, int CountryID, decimal From, decimal To, bool UsePercentage, decimal ShippingChargePercentage, decimal ShippingChargeAmount);

        /// <summary>
        /// Gets all ShippingByWeightAndCountrys by shipping method identifier
        /// </summary>
        /// <param name="ShippingMethodID">The shipping method identifier</param>
        /// <param name="CountryID">The country identifier</param>
        /// <returns>ShippingByWeightAndCountry collection</returns>
        public abstract DBShippingByWeightAndCountryCollection GetAllByShippingMethodIDAndCountryID(int ShippingMethodID, int CountryID);
        #endregion
    }
}
