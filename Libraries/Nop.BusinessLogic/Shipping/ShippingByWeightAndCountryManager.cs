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
using NopSolutions.NopCommerce.BusinessLogic.Caching;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Shipping;

namespace NopSolutions.NopCommerce.BusinessLogic.Shipping
{
    /// <summary>
    /// "ShippingByWeightAndCountry" manager
    /// </summary>
    public partial class ShippingByWeightAndCountryManager
    {
        #region Utilities
        private static ShippingByWeightAndCountryCollection DBMapping(DBShippingByWeightAndCountryCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            ShippingByWeightAndCountryCollection collection = new ShippingByWeightAndCountryCollection();
            foreach (DBShippingByWeightAndCountry dbItem in dbCollection)
            {
                ShippingByWeightAndCountry item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ShippingByWeightAndCountry DBMapping(DBShippingByWeightAndCountry dbItem)
        {
            if (dbItem == null)
                return null;

            ShippingByWeightAndCountry item = new ShippingByWeightAndCountry();
            item.ShippingByWeightAndCountryID = dbItem.ShippingByWeightAndCountryID;
            item.ShippingMethodID = dbItem.ShippingMethodID;
            item.CountryID = dbItem.CountryID;
            item.From = dbItem.From;
            item.To = dbItem.To;
            item.UsePercentage = dbItem.UsePercentage;
            item.ShippingChargePercentage = dbItem.ShippingChargePercentage;
            item.ShippingChargeAmount = dbItem.ShippingChargeAmount;

            return item;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets a ShippingByWeightAndCountry
        /// </summary>
        /// <param name="ShippingByWeightAndCountryID">ShippingByWeightAndCountry identifier</param>
        /// <returns>ShippingByWeightAndCountry</returns>
        public static ShippingByWeightAndCountry GetByID(int ShippingByWeightAndCountryID)
        {
            if (ShippingByWeightAndCountryID == 0)
                return null;

            DBShippingByWeightAndCountry dbItem = DBProviderManager<DBShippingByWeightAndCountryProvider>.Provider.GetByID(ShippingByWeightAndCountryID);
            ShippingByWeightAndCountry shippingByWeightAndCountry = DBMapping(dbItem);
            return shippingByWeightAndCountry;
        }

        /// <summary>
        /// Deletes a ShippingByWeightAndCountry
        /// </summary>
        /// <param name="ShippingByWeightAndCountryID">ShippingByWeightAndCountry identifier</param>
        public static void DeleteShippingByWeightAndCountry(int ShippingByWeightAndCountryID)
        {
            DBProviderManager<DBShippingByWeightAndCountryProvider>.Provider.DeleteShippingByWeightAndCountry(ShippingByWeightAndCountryID);
        }

        /// <summary>
        /// Gets all ShippingByWeightAndCountrys
        /// </summary>
        /// <returns>ShippingByWeightAndCountry collection</returns>
        public static ShippingByWeightAndCountryCollection GetAll()
        {
            DBShippingByWeightAndCountryCollection dbCollection = DBProviderManager<DBShippingByWeightAndCountryProvider>.Provider.GetAll();
            ShippingByWeightAndCountryCollection collection = DBMapping(dbCollection);
            return collection;
        }

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
        public static ShippingByWeightAndCountry InsertShippingByWeightAndCountry(int ShippingMethodID,
            int CountryID, decimal From, decimal To, bool UsePercentage,
            decimal ShippingChargePercentage, decimal ShippingChargeAmount)
        {
            DBShippingByWeightAndCountry dbItem = DBProviderManager<DBShippingByWeightAndCountryProvider>.Provider.InsertShippingByWeightAndCountry(ShippingMethodID,
                CountryID, From, To, UsePercentage,
                ShippingChargePercentage, ShippingChargeAmount);
            ShippingByWeightAndCountry shippingByWeightAndCountry = DBMapping(dbItem);
            return shippingByWeightAndCountry;
        }

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
        public static ShippingByWeightAndCountry UpdateShippingByWeightAndCountry(int ShippingByWeightAndCountryID,
            int ShippingMethodID, int CountryID, decimal From, decimal To,
            bool UsePercentage, decimal ShippingChargePercentage, decimal ShippingChargeAmount)
        {
            DBShippingByWeightAndCountry dbItem = DBProviderManager<DBShippingByWeightAndCountryProvider>.Provider.UpdateShippingByWeightAndCountry(ShippingByWeightAndCountryID,
                ShippingMethodID, CountryID, From, To, UsePercentage,
                ShippingChargePercentage, ShippingChargeAmount);
            ShippingByWeightAndCountry shippingByWeightAndCountry = DBMapping(dbItem);
            return shippingByWeightAndCountry;
        }

        /// <summary>
        /// Gets all ShippingByWeightAndCountrys by shipping method identifier
        /// </summary>
        /// <param name="ShippingMethodID">The shipping method identifier</param>
        /// <param name="CountryID">The country identifier</param>
        /// <returns>ShippingByWeightAndCountry collection</returns>
        public static ShippingByWeightAndCountryCollection GetAllByShippingMethodIDAndCountryID(int ShippingMethodID, int CountryID)
        {
            DBShippingByWeightAndCountryCollection dbCollection = DBProviderManager<DBShippingByWeightAndCountryProvider>.Provider.GetAllByShippingMethodIDAndCountryID(ShippingMethodID, CountryID);
            ShippingByWeightAndCountryCollection collection = DBMapping(dbCollection);
            return collection;
        }
        #endregion
    }
}
