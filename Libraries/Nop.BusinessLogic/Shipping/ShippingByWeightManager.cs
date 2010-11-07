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
    /// "ShippingByWeight" manager
    /// </summary>
    public partial class ShippingByWeightManager
    {
        #region Utilities
        private static ShippingByWeightCollection DBMapping(DBShippingByWeightCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            ShippingByWeightCollection collection = new ShippingByWeightCollection();
            foreach (DBShippingByWeight dbItem in dbCollection)
            {
                ShippingByWeight item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ShippingByWeight DBMapping(DBShippingByWeight dbItem)
        {
            if (dbItem == null)
                return null;

            ShippingByWeight item = new ShippingByWeight();
            item.ShippingByWeightID = dbItem.ShippingByWeightID;
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
        /// Gets a ShippingByWeight
        /// </summary>
        /// <param name="ShippingByWeightID">ShippingByWeight identifier</param>
        /// <returns>ShippingByWeight</returns>
        public static ShippingByWeight GetByID(int ShippingByWeightID)
        {
            if (ShippingByWeightID == 0)
                return null;

            DBShippingByWeight dbItem = DBProviderManager<DBShippingByWeightProvider>.Provider.GetByID(ShippingByWeightID);
            ShippingByWeight shippingByWeight = DBMapping(dbItem);
            return shippingByWeight;
        }

        /// <summary>
        /// Deletes a ShippingByWeight
        /// </summary>
        /// <param name="ShippingByWeightID">ShippingByWeight identifier</param>
        public static void DeleteShippingByWeight(int ShippingByWeightID)
        {
            DBProviderManager<DBShippingByWeightProvider>.Provider.DeleteShippingByWeight(ShippingByWeightID);
        }

        /// <summary>
        /// Gets all ShippingByWeights
        /// </summary>
        /// <returns>ShippingByWeight collection</returns>
        public static ShippingByWeightCollection GetAll()
        {
            DBShippingByWeightCollection dbCollection = DBProviderManager<DBShippingByWeightProvider>.Provider.GetAll();
            ShippingByWeightCollection collection = DBMapping(dbCollection);
            return collection;
        }

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
        public static ShippingByWeight InsertShippingByWeight(int ShippingMethodID, decimal From, decimal To,
            bool UsePercentage, decimal ShippingChargePercentage, decimal ShippingChargeAmount)
        {
            DBShippingByWeight dbItem = DBProviderManager<DBShippingByWeightProvider>.Provider.InsertShippingByWeight(ShippingMethodID, From, To, UsePercentage,
                ShippingChargePercentage, ShippingChargeAmount);
            ShippingByWeight shippingByWeight = DBMapping(dbItem);
            return shippingByWeight;
        }

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
        public static ShippingByWeight UpdateShippingByWeight(int ShippingByWeightID, int ShippingMethodID, decimal From, decimal To,
            bool UsePercentage, decimal ShippingChargePercentage, decimal ShippingChargeAmount)
        {
            DBShippingByWeight dbItem = DBProviderManager<DBShippingByWeightProvider>.Provider.UpdateShippingByWeight(ShippingByWeightID, ShippingMethodID, From, To, UsePercentage,
                ShippingChargePercentage, ShippingChargeAmount);
            ShippingByWeight shippingByWeight = DBMapping(dbItem);
            return shippingByWeight;
        }

        /// <summary>
        /// Gets all ShippingByWeights by shipping method identifier
        /// </summary>
        /// <param name="ShippingMethodID">The shipping method identifier</param>
        /// <returns>ShippingByWeight collection</returns>
        public static ShippingByWeightCollection GetAllByShippingMethodID(int ShippingMethodID)
        {
            DBShippingByWeightCollection dbCollection = DBProviderManager<DBShippingByWeightProvider>.Provider.GetAllByShippingMethodID(ShippingMethodID);
            ShippingByWeightCollection collection = DBMapping(dbCollection);
            return collection;
        }
        #endregion
    }
}
