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
    /// "Shipping by total" manager
    /// </summary>
    public partial class ShippingByTotalManager
    {
        #region Utilities
        private static ShippingByTotalCollection DBMapping(DBShippingByTotalCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            ShippingByTotalCollection collection = new ShippingByTotalCollection();
            foreach (DBShippingByTotal dbItem in dbCollection)
            {
                ShippingByTotal item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ShippingByTotal DBMapping(DBShippingByTotal dbItem)
        {
            if (dbItem == null)
                return null;

            ShippingByTotal item = new ShippingByTotal();
            item.ShippingByTotalID = dbItem.ShippingByTotalID;
            item.ShippingMethodID = dbItem.ShippingMethodID;
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
        /// Get a ShippingByTotal
        /// </summary>
        /// <param name="ShippingByTotalID">ShippingByTotal identifier</param>
        /// <returns>ShippingByTotal</returns>
        public static ShippingByTotal GetByID(int ShippingByTotalID)
        {
            if (ShippingByTotalID == 0)
                return null;

            DBShippingByTotal dbItem = DBProviderManager<DBShippingByTotalProvider>.Provider.GetByID(ShippingByTotalID);
            ShippingByTotal shippingByTotal = DBMapping(dbItem);
            return shippingByTotal;
        }

        /// <summary>
        /// Deletes a ShippingByTotal
        /// </summary>
        /// <param name="ShippingByTotalID">ShippingByTotal identifier</param>
        public static void DeleteShippingByTotal(int ShippingByTotalID)
        {
            DBProviderManager<DBShippingByTotalProvider>.Provider.DeleteShippingByTotal(ShippingByTotalID);
        }

        /// <summary>
        /// Gets all ShippingByTotals
        /// </summary>
        /// <returns>ShippingByTotal collection</returns>
        public static ShippingByTotalCollection GetAll()
        {
            DBShippingByTotalCollection dbCollection = DBProviderManager<DBShippingByTotalProvider>.Provider.GetAll();
            ShippingByTotalCollection collection = DBMapping(dbCollection);
            return collection;
        }

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
        public static ShippingByTotal InsertShippingByTotal(int ShippingMethodID, decimal From, decimal To,
            bool UsePercentage, decimal ShippingChargePercentage, decimal ShippingChargeAmount)
        {
            DBShippingByTotal dbItem = DBProviderManager<DBShippingByTotalProvider>.Provider.InsertShippingByTotal(ShippingMethodID, From, To, UsePercentage,
                ShippingChargePercentage, ShippingChargeAmount);
            ShippingByTotal shippingByTotal = DBMapping(dbItem);
            return shippingByTotal;
        }

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
        public static ShippingByTotal UpdateShippingByTotal(int ShippingByTotalID, int ShippingMethodID, decimal From, decimal To,
            bool UsePercentage, decimal ShippingChargePercentage, decimal ShippingChargeAmount)
        {
            DBShippingByTotal dbItem = DBProviderManager<DBShippingByTotalProvider>.Provider.UpdateShippingByTotal(ShippingByTotalID, ShippingMethodID, From, To, UsePercentage,
                ShippingChargePercentage, ShippingChargeAmount);
            ShippingByTotal shippingByTotal = DBMapping(dbItem);
            return shippingByTotal;
        }

        /// <summary>
        /// Gets all ShippingByTotals by shipping method identifier
        /// </summary>
        /// <param name="ShippingMethodID">The shipping method identifier</param>
        /// <returns>ShippingByTotal collection</returns>
        public static ShippingByTotalCollection GetAllByShippingMethodID(int ShippingMethodID)
        {
            DBShippingByTotalCollection dbCollection = DBProviderManager<DBShippingByTotalProvider>.Provider.GetAllByShippingMethodID(ShippingMethodID);
            ShippingByTotalCollection collection = DBMapping(dbCollection);
            return collection;
        }
        #endregion
    }
}
