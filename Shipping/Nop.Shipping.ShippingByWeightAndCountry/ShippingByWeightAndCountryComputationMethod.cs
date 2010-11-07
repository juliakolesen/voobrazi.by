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
using System.Linq;
using System.Text;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Shipping;
using NopSolutions.NopCommerce.Common;

namespace NopSolutions.NopCommerce.Shipping.Methods.ShippingByWeightAndCountryCM
{
    /// <summary>
    /// Shipping by weight and country computation method
    /// </summary>
    public class ShippingByWeightAndCountryComputationMethod : IShippingRateComputationMethod
    {
        #region Utilities

        private decimal GetRate(decimal subTotal, decimal weight, int ShippingMethodID, int CountryID)
        {
            decimal shippingTotal = decimal.Zero;
            ShippingByWeightAndCountry shippingByWeightAndCountry = null;
            ShippingByWeightAndCountryCollection shippingByWeightAndCountryCollection = ShippingByWeightAndCountryManager.GetAllByShippingMethodIDAndCountryID(ShippingMethodID, CountryID);
            foreach (ShippingByWeightAndCountry shippingByWeightAndCountry2 in shippingByWeightAndCountryCollection)
            {
                if ((weight >= shippingByWeightAndCountry2.From) && (weight <= shippingByWeightAndCountry2.To))
                {
                    shippingByWeightAndCountry = shippingByWeightAndCountry2;
                    break;
                }
            }
            if (shippingByWeightAndCountry == null)
                return decimal.Zero;
            if (shippingByWeightAndCountry.UsePercentage && shippingByWeightAndCountry.ShippingChargePercentage <= decimal.Zero)
                return decimal.Zero;
            if (!shippingByWeightAndCountry.UsePercentage && shippingByWeightAndCountry.ShippingChargeAmount <= decimal.Zero)
                return decimal.Zero;
            if (shippingByWeightAndCountry.UsePercentage)
                shippingTotal = Math.Round((decimal)((((float)subTotal) * ((float)shippingByWeightAndCountry.ShippingChargePercentage)) / 100f), 2);
            else
                shippingTotal = shippingByWeightAndCountry.ShippingChargeAmount * weight;

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
            if (ShipmentPackage.ShippingAddress == null)
            {
                Error = "Shipping address is not set";
                return shippingOptions;
            }
            if (ShipmentPackage.ShippingAddress.Country == null)
            {
                Error = "Shipping country is not set";
                return shippingOptions;
            }

            decimal subTotal = decimal.Zero;
            foreach (ShoppingCartItem shoppingCartItem in ShipmentPackage.Items)
            {
                if (shoppingCartItem.IsFreeShipping)
                    continue;
                subTotal += PriceHelper.GetSubTotal(shoppingCartItem, ShipmentPackage.Customer, true);
            }
            decimal weight = ShippingManager.GetShoppingCartTotalWeigth(ShipmentPackage.Items);

            ShippingMethodCollection shippingMethods = ShippingMethodManager.GetAllShippingMethods();
            foreach (ShippingMethod shippingMethod in shippingMethods)
            {
                ShippingOption shippingOption = new ShippingOption();
                shippingOption.Name = shippingMethod.Name;
                shippingOption.Description = shippingMethod.Description;
                shippingOption.Rate = GetRate(subTotal, weight, shippingMethod.ShippingMethodID, ShipmentPackage.ShippingAddress.Country.CountryID);
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
