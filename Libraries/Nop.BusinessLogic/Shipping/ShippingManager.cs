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
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.Common;


namespace NopSolutions.NopCommerce.BusinessLogic.Shipping
{
    /// <summary>
    /// Shipping manager
    /// </summary>
    public partial class ShippingManager
    {
        #region Utilities

        /// <summary>
        /// Gets a value indicating whether shipping is free
        /// </summary>
        /// <param name="Cart">Cart</param>
        /// <param name="customer">Customer</param>
        /// <returns>A value indicating whether shipping is free</returns>
        protected static bool IsFreeShipping(ShoppingCart Cart, Customer customer)
        {
            if (customer != null)
            {
                CustomerRoleCollection customerRoles = customer.CustomerRoles;
                foreach (CustomerRole customerRole in customerRoles)
                    if (customerRole.FreeShipping)
                        return true;
            }

            bool shoppingCartRequiresShipping = ShoppingCartRequiresShipping(Cart);
            if (!shoppingCartRequiresShipping)
                return true;

            decimal orderSubTotalDiscount;
            decimal orderSubTotal = ShoppingCartManager.GetShoppingCartSubTotal(Cart, customer, out orderSubTotalDiscount);
            if (SettingManager.GetSettingValueBoolean("Shipping.FreeShippingOverX.Enabled"))
            {
                decimal freeShippingOverX = SettingManager.GetSettingValueDecimalNative("Shipping.FreeShippingOverX.Value");
                if (orderSubTotal > freeShippingOverX)
                    return true;
            }

            bool allItemsAreFreeShipping = true;
            foreach (ShoppingCartItem sc in Cart)
                if (!sc.IsFreeShipping)
                {
                    allItemsAreFreeShipping = false;
                    break;
                }
            if (allItemsAreFreeShipping)
                return true;

            return false;
        }

        /// <summary>
        /// Create shipment package from shopping cart
        /// </summary>
        /// <param name="Cart">Shopping cart</param>
        /// <param name="customer">Customer</param>
        /// <param name="ShippingAddress">Shipping address</param>
        /// <returns>Shipment package</returns>
        protected static ShipmentPackage CreateShipmentPackage(ShoppingCart Cart, Customer customer, Address ShippingAddress)
        {
            ShipmentPackage shipmentPackage = new ShipmentPackage();
            shipmentPackage.Customer = customer;
            shipmentPackage.Items = new ShoppingCart();
            foreach (ShoppingCartItem sc in Cart)
                if (sc.IsShipEnabled)
                    shipmentPackage.Items.Add(sc);
            shipmentPackage.ShippingAddress = ShippingAddress;
            //TODO set values from warehouses or shipping origin
            shipmentPackage.CountryFrom = null;
            shipmentPackage.StateProvinceFrom = null;
            shipmentPackage.ZipPostalCodeFrom = string.Empty;
            return shipmentPackage;

        }

        #endregion

        #region Methods
        /// <summary>
        /// Gets shopping cart weight
        /// </summary>
        /// <param name="Cart">Cart</param>
        /// <returns>Shopping cart weight</returns>
        public static decimal GetShoppingCartTotalWeigth(ShoppingCart Cart)
        {
            decimal totalWeight = decimal.Zero;
            foreach (ShoppingCartItem shoppingCartItem in Cart)
                totalWeight += shoppingCartItem.TotalWeigth;
            return totalWeight;
        }

        /// <summary>
        /// Gets shopping cart shipping total
        /// </summary>
        /// <param name="Cart">Cart</param>
        /// <returns>Shipping total</returns>
        public static decimal? GetShoppingCartShippingTotal(ShoppingCart Cart)
        {
            string Error = string.Empty;
            return GetShoppingCartShippingTotal(Cart, ref Error);
        }

        /// <summary>
        /// Gets shopping cart shipping total
        /// </summary>
        /// <param name="Cart">Cart</param>
        /// <param name="Error">Error</param>
        /// <returns>Shipping total</returns>
        public static decimal? GetShoppingCartShippingTotal(ShoppingCart Cart, ref string Error)
        {
            Customer customer = NopContext.Current.User;
            return GetShoppingCartShippingTotal(Cart, customer, ref Error);
        }

        /// <summary>
        /// Gets shopping cart shipping total
        /// </summary>
        /// <param name="Cart">Cart</param>
        /// <param name="customer">Customer</param>
        /// <returns>Shipping total</returns>
        public static decimal? GetShoppingCartShippingTotal(ShoppingCart Cart, Customer customer)
        {
            string Error = string.Empty;
            return GetShoppingCartShippingTotal(Cart, customer, ref Error);
        }

        /// <summary>
        /// Gets shopping cart shipping total
        /// </summary>
        /// <param name="Cart">Cart</param>
        /// <param name="customer">Customer</param>
        /// <param name="Error">Error</param>
        /// <returns>Shipping total</returns>
        public static decimal? GetShoppingCartShippingTotal(ShoppingCart Cart,
            Customer customer, ref string Error)
        {
            bool includingTax = false;
            switch (NopContext.Current.TaxDisplayType)
            {
                case TaxDisplayTypeEnum.ExcludingTax:
                    includingTax = false;
                    break;
                case TaxDisplayTypeEnum.IncludingTax:
                    includingTax = true;
                    break;
            }
            return GetShoppingCartShippingTotal(Cart, customer, includingTax, ref Error);
        }

        /// <summary>
        /// Gets shopping cart shipping total
        /// </summary>
        /// <param name="Cart">Cart</param>
        /// <param name="customer">Customer</param>
        /// <param name="includingTax">A value indicating whether calculated price should include tax</param>
        /// <returns>Shipping total</returns>
        public static decimal? GetShoppingCartShippingTotal(ShoppingCart Cart,
            Customer customer, bool includingTax)
        {
            string Error = string.Empty;
            return GetShoppingCartShippingTotal(Cart, customer, includingTax, ref Error);
        }

        /// <summary>
        /// Gets shopping cart shipping total
        /// </summary>
        /// <param name="Cart">Cart</param>
        /// <param name="customer">Customer</param>
        /// <param name="includingTax">A value indicating whether calculated price should include tax</param>
        /// <param name="Error">Error</param>
        /// <returns>Shipping total</returns>
        public static decimal? GetShoppingCartShippingTotal(ShoppingCart Cart, 
            Customer customer, bool includingTax, ref string Error)
        {
            decimal? shippingTotal = null;

            bool isFreeShipping = IsFreeShipping(Cart, customer);
            if (isFreeShipping)
                return decimal.Zero;

            ShippingOption lastShippingOption = null;            
            if (customer != null)
            {
                lastShippingOption = customer.LastShippingOption;
            }

            if (lastShippingOption != null)
            {
                //use last shipping option (get from cache)
                shippingTotal = TaxManager.GetShippingPrice(lastShippingOption.Rate,
                    includingTax,
                    customer,
                    ref Error);
            }
            else
            {
                //use fixed rate (if possible)
                Address shippingAddress = null;
                if (customer != null)
                {
                    shippingAddress = customer.ShippingAddress;
                }
                ShipmentPackage ShipmentPackage = CreateShipmentPackage(Cart, customer, shippingAddress);
                ShippingRateComputationMethod activeShippingRateComputationMethod = ShippingManager.ActiveShippingRateComputationMethod;
                if (activeShippingRateComputationMethod == null)
                    throw new NopException("Shipping rate computation method could not be loaded");
                IShippingRateComputationMethod iShippingRateComputationMethod = Activator.CreateInstance(Type.GetType(activeShippingRateComputationMethod.ClassName)) as IShippingRateComputationMethod;

                decimal? fixedRate = iShippingRateComputationMethod.GetFixedRate(ShipmentPackage);
                if (fixedRate.HasValue)
                {
                    decimal additionalShippingCharge = GetShoppingCartAdditionalShippingCharge(Cart, customer);

                    shippingTotal = TaxManager.GetShippingPrice(fixedRate.Value + additionalShippingCharge,
                       includingTax,
                       customer,
                       ref Error);
                }
            }            

            if (!shippingTotal.HasValue)
            {
                Error = "Shipping total could not be calculated";
            }
            else
            {
                shippingTotal = Math.Round(shippingTotal.Value, 2);
            }
            return shippingTotal;
        }

        /// <summary>
        /// Indicates whether the shopping cart requires shipping
        /// </summary>
        /// <param name="Cart">Cart</param>
        /// <returns>True if the shopping cart requires shipping; otherwise, false.</returns>
        public static bool ShoppingCartRequiresShipping(ShoppingCart Cart)
        {
            foreach (ShoppingCartItem shoppingCartItem in Cart)
                if (shoppingCartItem.IsShipEnabled)
                    return true;
            return false;
        }

        /// <summary>
        /// Gets shopping cart additional shipping charge
        /// </summary>
        /// <param name="Cart">Cart</param>
        /// <param name="customer">Customer</param>
        /// <returns>Additional shipping charge</returns>
        public static decimal GetShoppingCartAdditionalShippingCharge(ShoppingCart Cart, Customer customer)
        {
            decimal additionalShippingCharge = decimal.Zero;

            bool isFreeShipping = IsFreeShipping(Cart, customer);
            if (isFreeShipping)
                return decimal.Zero;

            foreach (ShoppingCartItem shoppingCartItem in Cart)
                additionalShippingCharge += shoppingCartItem.AdditionalShippingCharge;

            return additionalShippingCharge;
        }

        /// <summary>
        ///  Gets available shipping options
        /// </summary>
        /// <param name="Cart">Shopping cart</param>
        /// <param name="customer">Customer</param>
        /// <param name="ShippingAddress">Shipping address</param>
        /// <param name="Error">Error</param>
        /// <returns>Shipping options</returns>
        public static ShippingOptionCollection GetShippingOptions(ShoppingCart Cart, Customer customer, Address ShippingAddress, ref string Error)
        {
            if (Cart == null)
                throw new ArgumentNullException("Cart");

            bool isFreeShipping = IsFreeShipping(Cart, customer);

            ShipmentPackage ShipmentPackage = CreateShipmentPackage(Cart, customer, ShippingAddress);
            ShippingRateComputationMethod activeShippingRateComputationMethod = ShippingManager.ActiveShippingRateComputationMethod;
            if (activeShippingRateComputationMethod == null)
                throw new NopException("Shipping rate computation method could not be loaded");
            IShippingRateComputationMethod iShippingRateComputationMethod = Activator.CreateInstance(Type.GetType(activeShippingRateComputationMethod.ClassName)) as IShippingRateComputationMethod;

            ShippingOptionCollection shippingOptions = iShippingRateComputationMethod.GetShippingOptions(ShipmentPackage, ref Error);

            decimal additionalShippingCharge = GetShoppingCartAdditionalShippingCharge(Cart, customer);
            shippingOptions.ForEach(so => so.Rate += additionalShippingCharge);
            
            if (isFreeShipping)
            {
                shippingOptions.ForEach(so => so.Rate = decimal.Zero);
            }

            shippingOptions.ForEach(so => so.Rate = Math.Round(so.Rate, 2));

            return shippingOptions;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets an active shipping rate computation method
        /// </summary>
        public static ShippingRateComputationMethod ActiveShippingRateComputationMethod
        {
            get
            {
                //TODO reset last shipping options for all customers
                int shippingRateComputationMethodID = SettingManager.GetSettingValueInteger("Shipping.ShippingRateComputationMethod.ActiveID");
                return ShippingRateComputationMethodManager.GetShippingRateComputationMethodByID(shippingRateComputationMethodID);
            }
            set
            {
                if (value != null)
                    SettingManager.SetParam("Shipping.ShippingRateComputationMethod.ActiveID", value.ShippingRateComputationMethodID.ToString());
            }
        }
        
        /// <summary>
        /// Gets or sets a default shipping origin address
        /// </summary>
        public static Address ShippingOrigin
        {
            get
            {
                int countryID = SettingManager.GetSettingValueInteger("Shipping.ShippingOrigin.CountryID");
                int stateProvinceID = SettingManager.GetSettingValueInteger("Shipping.ShippingOrigin.StateProvinceID");
                string zipPostalCode = SettingManager.GetSettingValue("Shipping.ShippingOrigin.ZipPostalCode");
                Address address = new Address();
                address.CountryID = countryID;
                address.StateProvinceID = stateProvinceID;
                address.ZipPostalCode = zipPostalCode;
                return address;
            }
            set
            {
                int countryID = 0;
                int stateProvinceID = 0;
                string zipPostalCode = string.Empty;

                if (value != null)
                {
                    countryID = value.CountryID;
                    stateProvinceID = value.StateProvinceID;
                    zipPostalCode = value.ZipPostalCode;
                }

                SettingManager.SetParam("Shipping.ShippingOrigin.CountryID", countryID.ToString());
                SettingManager.SetParam("Shipping.ShippingOrigin.StateProvinceID", stateProvinceID.ToString());
                SettingManager.SetParam("Shipping.ShippingOrigin.ZipPostalCode", zipPostalCode);
            }
        }
        #endregion
    }
}
