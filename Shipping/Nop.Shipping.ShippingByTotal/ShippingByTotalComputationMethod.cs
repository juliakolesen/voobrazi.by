﻿//------------------------------------------------------------------------------
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
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Shipping;
using NopSolutions.NopCommerce.Common;

namespace NopSolutions.NopCommerce.Shipping.Methods.ShippingByTotalCM
{
    /// <summary>
    /// Shipping by order total computation method
    /// </summary>
    public class ShippingByTotalComputationMethod : IShippingRateComputationMethod
    {
        #region Utilities

        /// <summary>
        /// Gets a shipping rate
        /// </summary>
        /// <param name="subTotal">Subtotal</param>
        /// <param name="ShippingMethodID">Shipping method identifier</param>
        /// <returns>Shipping rate</returns>
        protected decimal GetRate(decimal subTotal, int ShippingMethodID)
        {
            decimal shippingTotal = decimal.Zero;

            ShippingByTotal shippingByTotal = null;
            ShippingByTotalCollection shippingByTotalCollection = ShippingByTotalManager.GetAllByShippingMethodID(ShippingMethodID);
            foreach (ShippingByTotal shippingByTotal2 in shippingByTotalCollection)
            {
                if ((subTotal >= shippingByTotal2.From) && (subTotal <= shippingByTotal2.To))
                {
                    shippingByTotal = shippingByTotal2;
                    break;
                }
            }
            if (shippingByTotal == null)
                return decimal.Zero;
            if (shippingByTotal.UsePercentage && shippingByTotal.ShippingChargePercentage <= decimal.Zero)
                return decimal.Zero;
            if (!shippingByTotal.UsePercentage && shippingByTotal.ShippingChargeAmount <= decimal.Zero)
                return decimal.Zero;
            if (shippingByTotal.UsePercentage)
                shippingTotal = Math.Round((decimal)((((float)subTotal) * ((float)shippingByTotal.ShippingChargePercentage)) / 100f), 2);
            else
                shippingTotal = shippingByTotal.ShippingChargeAmount;

            if (shippingTotal < decimal.Zero)
                shippingTotal = decimal.Zero;
            return shippingTotal;
        }
        #endregion

        #region Methods
        /// <summary>
        ///  Gets available shipping options
        /// </summary>
        /// <param name="ShipmentPackage">Shipment package</param>
        /// <param name="Error">Error</param>
        /// <returns>Shipping options</returns>
        public ShippingOptionCollection GetShippingOptions(ShipmentPackage ShipmentPackage, ref string Error)
        {
            ShippingOptionCollection shippingOptions = new ShippingOptionCollection();

            if (ShipmentPackage == null)
                throw new ArgumentNullException("ShipmentPackage");
            if (ShipmentPackage.Items == null)
                throw new NopException("No shipment items");

            decimal subTotal = decimal.Zero;
            foreach (ShoppingCartItem shoppingCartItem in ShipmentPackage.Items)
            {
                if (shoppingCartItem.IsFreeShipping)
                    continue;
                subTotal += PriceHelper.GetSubTotal(shoppingCartItem, ShipmentPackage.Customer, true);
            }

            ShippingMethodCollection shippingMethods = ShippingMethodManager.GetAllShippingMethods();
            foreach (ShippingMethod shippingMethod in shippingMethods)
            {
                ShippingOption shippingOption = new ShippingOption();
                shippingOption.Name = shippingMethod.Name;
                shippingOption.Description = shippingMethod.Description;
                shippingOption.Rate = GetRate(subTotal, shippingMethod.ShippingMethodID);
                shippingOptions.Add(shippingOption);
            }

            return shippingOptions;
        }
        
        /// <summary>
        /// Gets fixed shipping rate (if shipping rate computation method allows it and the rate can be calculated before checkout).
        /// </summary>
        /// <param name="ShipmentPackage">Shipment package</param>
        /// <returns>Fixed shipping rate; or null if shipping rate could not be calculated before checkout</returns>
        public decimal? GetFixedRate(ShipmentPackage ShipmentPackage)
        {
            return null;
        }

        #endregion
    }
}
