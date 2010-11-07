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
using System.Web.Compilation;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Shipping;
using NopSolutions.NopCommerce.Common;


namespace NopSolutions.NopCommerce.BusinessLogic.Tax
{
    /// <summary>
    /// Tax manager
    /// </summary>
    public partial class TaxManager
    {
        #region Utilities

        /// <summary>
        /// Gets a value indicating whether tax is free
        /// </summary>
        /// <param name="productVariant">Product variant</param>
        /// <param name="customer">Customer</param>
        /// <returns>A value indicating whether tax is free</returns>
        protected static bool IsFreeTax(ProductVariant productVariant, Customer customer)
        {
            if (customer != null)
            {
                if (customer.IsTaxExempt)
                    return true;

                CustomerRoleCollection customerRoles = customer.CustomerRoles;
                foreach (CustomerRole customerRole in customerRoles)
                    if (customerRole.TaxExempt)
                        return true;
            }
            
            if (productVariant == null)
            {
                return false;
            }

            if (productVariant.IsTaxExempt)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Create request for tax calculation
        /// </summary>
        /// <param name="productVariant">Product variant</param>
        /// <param name="TaxClassID">Tax class identifier</param>
        /// <param name="customer">Customer</param>
        /// <returns>Package for tax calculation</returns>
        protected static CalculateTaxRequest CreateCalculateTaxRequest(ProductVariant productVariant, int TaxClassID, Customer customer)
        {
            CalculateTaxRequest calculateTaxRequest = new CalculateTaxRequest();
            calculateTaxRequest.Customer = customer;
            calculateTaxRequest.Item = productVariant;
            calculateTaxRequest.TaxClassID = TaxClassID;

            TaxBasedOnEnum basedOn = TaxManager.TaxBasedOn;

            if (basedOn == TaxBasedOnEnum.BillingAddress)
            {
                if (customer == null || customer.BillingAddress == null)
                {
                    basedOn = TaxBasedOnEnum.DefaultAddress;
                }
            }
            if (basedOn == TaxBasedOnEnum.ShippingAddress)
            {
                if (customer == null || customer.ShippingAddress == null)
                {
                    basedOn = TaxBasedOnEnum.DefaultAddress;
                }
            }

            Address address = null;

            switch (basedOn)
            {
                case TaxBasedOnEnum.BillingAddress:
                    {
                        address = customer.BillingAddress;
                    }
                    break;
                case TaxBasedOnEnum.ShippingAddress:
                    {
                        address = customer.ShippingAddress;
                    }
                    break;
                case TaxBasedOnEnum.DefaultAddress:
                    {
                        address = TaxManager.DefaultTaxAddress;
                    }
                    break;
                case TaxBasedOnEnum.ShippingOrigin:
                    {
                        address = ShippingManager.ShippingOrigin;
                    }
                    break;
            }

            calculateTaxRequest.Address = address;
            return calculateTaxRequest;
        }

        /// <summary>
        /// Calculated price
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="percent">Percent</param>
        /// <param name="increase">Increase</param>
        /// <returns>New price</returns>
        protected static decimal CalculatePrice(decimal price, decimal percent, bool increase)
        {
            decimal result = decimal.Zero;
            if (increase)
            {
                result = price * (1 + percent / 100);
            }
            else
            {
                result = price - (price) / (100 + percent) * percent;
            }
            return result;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Gets tax
        /// </summary>
        /// <param name="Cart">Shopping cart</param>
        /// <param name="customer">Customer</param>
        /// <param name="Error">Error</param>
        /// <returns>Tax total</returns>
        public static decimal GetTaxTotal(ShoppingCart Cart, Customer customer, ref string Error)
        {
            return GetTaxTotal(Cart, 0, customer, ref Error);
        }

        /// <summary>
        /// Gets tax
        /// </summary>
        /// <param name="Cart">Shopping cart</param>
        /// <param name="PaymentMethodID">Payment method identifier</param>
        /// <param name="customer">Customer</param>
        /// <param name="Error">Error</param>
        /// <returns>Tax total</returns>
        public static decimal GetTaxTotal(ShoppingCart Cart, int PaymentMethodID, 
            Customer customer, ref string Error)
        {
            decimal taxTotal = decimal.Zero;

            //items
            decimal itemsTaxTotal = decimal.Zero;
            foreach (ShoppingCartItem shoppingCartItem in Cart)
            {
                decimal subTotalWithoutDiscountExclTax = decimal.Zero;
                decimal subTotalWithoutDiscountInclTax = decimal.Zero;

                string Error1 = string.Empty;
                string Error2 = string.Empty;
                subTotalWithoutDiscountExclTax = TaxManager.GetPrice(shoppingCartItem.ProductVariant, PriceHelper.GetSubTotal(shoppingCartItem, customer, true), false, customer, ref Error1);
                subTotalWithoutDiscountInclTax = TaxManager.GetPrice(shoppingCartItem.ProductVariant, PriceHelper.GetSubTotal(shoppingCartItem, customer, true), true, customer, ref Error2);
                if (!String.IsNullOrEmpty(Error1))
                {
                    Error = Error1;
                }
                if (!String.IsNullOrEmpty(Error2))
                {
                    Error = Error2;
                }

                decimal shoppingCartItemTax = subTotalWithoutDiscountInclTax - subTotalWithoutDiscountExclTax;
                itemsTaxTotal += shoppingCartItemTax;
            }

            //shipping
            decimal shippingTax = decimal.Zero;
            if (TaxManager.ShippingIsTaxable)
            {
                string Error3 = string.Empty;
                string Error4 = string.Empty;
                decimal? shippingExclTax = ShippingManager.GetShoppingCartShippingTotal(Cart, customer, false, ref Error3);
                decimal? shippingInclTax = ShippingManager.GetShoppingCartShippingTotal(Cart, customer, true, ref Error4);
                if (!String.IsNullOrEmpty(Error3))
                {
                    Error = Error3;
                }
                if (!String.IsNullOrEmpty(Error4))
                {
                    Error = Error4;
                }
                if (shippingExclTax.HasValue && shippingInclTax.HasValue)
                {
                    shippingTax = shippingInclTax.Value - shippingExclTax.Value;
                }
            }

            //payment method additional fee
            decimal paymentMethodAdditionalFeeTax = decimal.Zero;
            if (TaxManager.PaymentMethodAdditionalFeeIsTaxable)
            {
                string Error5 = string.Empty;
                string Error6 = string.Empty;
                decimal paymentMethodAdditionalFee = PaymentManager.GetAdditionalHandlingFee(PaymentMethodID);
                decimal? paymentMethodAdditionalFeeExclTax = TaxManager.GetPaymentMethodAdditionalFee(paymentMethodAdditionalFee, false, customer, ref Error5);
                decimal? paymentMethodAdditionalFeeInclTax = TaxManager.GetPaymentMethodAdditionalFee(paymentMethodAdditionalFee, true, customer, ref Error6);
                if (!String.IsNullOrEmpty(Error5))
                {
                    Error = Error5;
                }
                if (!String.IsNullOrEmpty(Error6))
                {
                    Error = Error6;
                }
                if (paymentMethodAdditionalFeeExclTax.HasValue && paymentMethodAdditionalFeeInclTax.HasValue)
                {
                    paymentMethodAdditionalFeeTax = paymentMethodAdditionalFeeInclTax.Value - paymentMethodAdditionalFeeExclTax.Value;
                }
            }

            taxTotal = itemsTaxTotal + shippingTax + paymentMethodAdditionalFeeTax;
            taxTotal = Math.Round(taxTotal, 2);
            return taxTotal;
        }

        /// <summary>
        /// Gets tax rate
        /// </summary>
        /// <param name="productVariant">Product variant</param>
        /// <param name="customer">Customer</param>
        /// <param name="Error">Error</param>
        /// <returns>Tax rate</returns>
        public static decimal GetTaxRate(ProductVariant productVariant, Customer customer, ref string Error)
        {
            return GetTaxRate(productVariant, 0, customer, ref Error);
        }

        /// <summary>
        /// Gets tax rate
        /// </summary>
        /// <param name="TaxClassID">Tax class identifier</param>
        /// <param name="customer">Customer</param>
        /// <param name="Error">Error</param>
        /// <returns>Tax rate</returns>
        public static decimal GetTaxRate(int TaxClassID, Customer customer, ref string Error)
        {
            return GetTaxRate(null, TaxClassID, customer, ref Error);
        }
        
        /// <summary>
        /// Gets tax rate
        /// </summary>
        /// <param name="productVariant">Product variant</param>
        /// <param name="TaxClassID">Tax class identifier</param>
        /// <param name="customer">Customer</param>
        /// <param name="Error">Error</param>
        /// <returns>Tax rate</returns>
        public static decimal GetTaxRate(ProductVariant productVariant, int TaxClassID, Customer customer, ref string Error)
        {
            bool isFreeTax = IsFreeTax(productVariant, customer);
            if (isFreeTax)
                return decimal.Zero;

            CalculateTaxRequest calculateTaxRequest = CreateCalculateTaxRequest(productVariant, TaxClassID, customer);

            TaxProvider activeTaxProvider = TaxManager.ActiveTaxProvider;
            if (activeTaxProvider == null)
                throw new NopException("Tax provider could not be loaded");
            ITaxProvider iTaxProvider = Activator.CreateInstance(Type.GetType(activeTaxProvider.ClassName)) as ITaxProvider;

            decimal taxRate = iTaxProvider.GetTaxRate(calculateTaxRequest, ref Error);

            return taxRate;
        }
        
        /// <summary>
        /// Gets price
        /// </summary>
        /// <param name="productVariant">Product variant</param>
        /// <param name="price">Price</param>
        /// <returns>Price</returns>
        public static decimal GetPrice(ProductVariant productVariant, decimal price)
        {
            string Error = string.Empty;
            return GetPrice(productVariant, price, ref Error);
        }

        /// <summary>
        /// Gets price
        /// </summary>
        /// <param name="productVariant">Product variant</param>
        /// <param name="price">Price</param>
        /// <param name="Error">Error</param>
        /// <returns>Price</returns>
        public static decimal GetPrice(ProductVariant productVariant, decimal price, ref string Error)
        {
            Customer customer = NopContext.Current.User;
            return GetPrice(productVariant, price, customer, ref Error);
        }

        /// <summary>
        /// Gets price
        /// </summary>
        /// <param name="productVariant">Product variant</param>
        /// <param name="price">Price</param>
        /// <param name="customer">Customer</param>
        /// <returns>Price</returns>
        public static decimal GetPrice(ProductVariant productVariant, decimal price, Customer customer)
        {
            string Error = string.Empty;
            return GetPrice(productVariant, price, customer, ref Error);
        }

        /// <summary>
        /// Gets price
        /// </summary>
        /// <param name="productVariant">Product variant</param>
        /// <param name="price">Price</param>
        /// <param name="customer">Customer</param>
        /// <param name="Error">Error</param>
        /// <returns>Price</returns>
        public static decimal GetPrice(ProductVariant productVariant, decimal price,
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
            return GetPrice(productVariant, price, includingTax, customer, ref Error);
        }

        /// <summary>
        /// Gets price
        /// </summary>
        /// <param name="productVariant">Product variant</param>
        /// <param name="price">Price</param>
        /// <param name="includingTax">A value indicating whether calculated price should include tax</param>
        /// <param name="customer">Customer</param>
        /// <returns>Price</returns>
        public static decimal GetPrice(ProductVariant productVariant, decimal price,
            bool includingTax, Customer customer)
        {
            string Error = string.Empty;
            return GetPrice(productVariant, price, includingTax, customer, ref Error);
        }

        /// <summary>
        /// Gets price
        /// </summary>
        /// <param name="productVariant">Product variant</param>
        /// <param name="price">Price</param>
        /// <param name="includingTax">A value indicating whether calculated price should include tax</param>
        /// <param name="customer">Customer</param>
        /// <param name="Error">Error</param>
        /// <returns>Price</returns>
        public static decimal GetPrice(ProductVariant productVariant, decimal price,
            bool includingTax, Customer customer, ref string Error)
        {
            bool priceIncludesTax = TaxManager.PricesIncludeTax;
            int taxClassID = 0;
            return GetPrice(productVariant, taxClassID, price, includingTax, customer, priceIncludesTax, ref Error);
        }

        /// <summary>
        /// Gets price
        /// </summary>
        /// <param name="productVariant">Product variant</param>
        /// <param name="TaxClassID">Tax class identifier</param>
        /// <param name="price">Price</param>
        /// <param name="includingTax">A value indicating whether calculated price should include tax</param>
        /// <param name="customer">Customer</param>
        /// <param name="priceIncludesTax">A value indicating whether price already includes tax</param>
        /// <returns>Price</returns>
        public static decimal GetPrice(ProductVariant productVariant, int TaxClassID, decimal price,
            bool includingTax, Customer customer, bool priceIncludesTax)
        {
            string Error = string.Empty;
            return GetPrice(productVariant, TaxClassID, price, includingTax, customer, priceIncludesTax, ref Error);           
        }

        /// <summary>
        /// Gets price
        /// </summary>
        /// <param name="productVariant">Product variant</param>
        /// <param name="TaxClassID">Tax class identifier</param>
        /// <param name="price">Price</param>
        /// <param name="includingTax">A value indicating whether calculated price should include tax</param>
        /// <param name="customer">Customer</param>
        /// <param name="priceIncludesTax">A value indicating whether price already includes tax</param>
        /// <param name="Error">Error</param>
        /// <returns>Price</returns>
        public static decimal GetPrice(ProductVariant productVariant, int TaxClassID, decimal price,
            bool includingTax, Customer customer, bool priceIncludesTax, ref string Error)
        {
            if (priceIncludesTax)
            {
                //decimal includingPercent = GetTaxRate(productVariant, customer, ref Error);
                //if (includingTax)
                //{
                //    decimal percent = GetTaxRate(productVariant, customer, ref Error);
                //    price = CalculatePrice(price, includingPercent, false);
                //    price = CalculatePrice(price, percent, true);
                //}
                //else
                //{
                //    price = CalculatePrice(price, includingPercent, false);
                //}


                if (!includingTax)
                {
                    decimal includingPercent = GetTaxRate(productVariant, TaxClassID, customer, ref Error);
                    price = CalculatePrice(price, includingPercent, false);
                }
            }
            else
            {
                if (includingTax)
                {
                    decimal percent = GetTaxRate(productVariant, TaxClassID, customer, ref Error);
                    price = CalculatePrice(price, percent, true);
                }
            }

            price = Math.Round(price, 2);

            return price;
        }

        /// <summary>
        /// Gets shipping price
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="customer">Customer</param>
        /// <returns>Price</returns>
        public static decimal GetShippingPrice(decimal price, Customer customer)
        {
            string Error = string.Empty;
            return GetShippingPrice(price, customer, ref Error);
        }

        /// <summary>
        /// Gets shipping price
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="customer">Customer</param>
        /// <param name="Error">Error</param>
        /// <returns>Price</returns>
        public static decimal GetShippingPrice(decimal price,  Customer customer, ref string Error)
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
            return GetShippingPrice(price, includingTax, customer, ref Error);
        }

        /// <summary>
        /// Gets shipping price
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="includingTax">A value indicating whether calculated price should include tax</param>
        /// <param name="customer">Customer</param>
        /// <returns>Price</returns>
        public static decimal GetShippingPrice(decimal price, bool includingTax, Customer customer)
        {
            string Error = string.Empty;
            return GetShippingPrice(price, includingTax, customer, ref Error);
        }

        /// <summary>
        /// Gets shipping price
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="includingTax">A value indicating whether calculated price should include tax</param>
        /// <param name="customer">Customer</param>
        /// <param name="Error">Error</param>
        /// <returns>Price</returns>
        public static decimal GetShippingPrice(decimal price, bool includingTax, Customer customer, ref string Error)
        {
            if (!TaxManager.ShippingIsTaxable)
            {
                return price;
            }
            int taxClassID = TaxManager.ShippingTaxClassID;
            bool priceIncludesTax = TaxManager.ShippingPriceIncludesTax;
            return GetPrice(null, taxClassID, price, includingTax, customer, priceIncludesTax, ref Error);
        }

        /// <summary>
        /// Gets payment method additional handling fee
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="customer">Customer</param>
        /// <returns>Price</returns>
        public static decimal GetPaymentMethodAdditionalFee(decimal price, Customer customer)
        {
            string Error = string.Empty;
            return GetPaymentMethodAdditionalFee(price, customer, ref Error);
        }

        /// <summary>
        /// Gets payment method additional handling fee
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="customer">Customer</param>
        /// <param name="Error">Error</param>
        /// <returns>Price</returns>
        public static decimal GetPaymentMethodAdditionalFee(decimal price, Customer customer, ref string Error)
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
            return GetPaymentMethodAdditionalFee(price, includingTax, customer, ref Error);
        }

        /// <summary>
        /// Gets payment method additional handling fee
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="includingTax">A value indicating whether calculated price should include tax</param>
        /// <param name="customer">Customer</param>
        /// <returns>Price</returns>
        public static decimal GetPaymentMethodAdditionalFee(decimal price, bool includingTax, Customer customer)
        {
            string Error = string.Empty;
            return GetPaymentMethodAdditionalFee(price, includingTax, customer, ref Error);
        }

        /// <summary>
        /// Gets payment method additional handling fee
        /// </summary>
        /// <param name="price">Price</param>
        /// <param name="includingTax">A value indicating whether calculated price should include tax</param>
        /// <param name="customer">Customer</param>
        /// <param name="Error">Error</param>
        /// <returns>Price</returns>
        public static decimal GetPaymentMethodAdditionalFee(decimal price, bool includingTax, Customer customer, ref string Error)
        {
            if (!TaxManager.PaymentMethodAdditionalFeeIsTaxable)
            {
                return price;
            }
            int taxClassID = TaxManager.PaymentMethodAdditionalFeeTaxClassID;
            bool priceIncludesTax = TaxManager.PaymentMethodAdditionalFeeIncludesTax;
            return GetPrice(null, taxClassID, price, includingTax, customer, priceIncludesTax, ref Error);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Tax based on
        /// </summary>
        public static TaxBasedOnEnum TaxBasedOn
        {
            get
            {
                int taxBasedOn = SettingManager.GetSettingValueInteger("Tax.TaxBasedOn");
                return (TaxBasedOnEnum)taxBasedOn;
            }
            set
            {
                SettingManager.SetParam("Tax.TaxBasedOn", ((int)value).ToString());
            }
        }

        /// <summary>
        /// Tax display type
        /// </summary>
        public static TaxDisplayTypeEnum TaxDisplayType
        {
            get
            {
                int taxBasedOn = SettingManager.GetSettingValueInteger("Tax.TaxDisplayType");
                return (TaxDisplayTypeEnum)taxBasedOn;
            }
            set
            {
                SettingManager.SetParam("Tax.TaxDisplayType", ((int)value).ToString());
            }
        }
        
        /// <summary>
        /// Gets or sets an active shipping rate computation method
        /// </summary>
        public static TaxProvider ActiveTaxProvider
        {
            get
            {
                int taxProviderID = SettingManager.GetSettingValueInteger("Tax.TaxProvider.ActiveID");
                return TaxProviderManager.GetTaxProviderByID(taxProviderID);
            }
            set
            {
                //TODO reset last calculated tax for all customers
                if (value != null)
                    SettingManager.SetParam("Tax.TaxProvider.ActiveID", value.TaxProviderID.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a default tax address
        /// </summary>
        public static Address DefaultTaxAddress
        {
            get
            {
                int countryID = SettingManager.GetSettingValueInteger("Tax.DefaultTaxAddress.CountryID");
                int stateProvinceID = SettingManager.GetSettingValueInteger("Tax.DefaultTaxAddress.StateProvinceID");
                string zipPostalCode = SettingManager.GetSettingValue("Tax.DefaultTaxAddress.ZipPostalCode");
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

                SettingManager.SetParam("Tax.DefaultTaxAddress.CountryID", countryID.ToString());
                SettingManager.SetParam("Tax.DefaultTaxAddress.StateProvinceID", stateProvinceID.ToString());
                SettingManager.SetParam("Tax.DefaultTaxAddress.ZipPostalCode", zipPostalCode);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to display tax suffix
        /// </summary>
        public static bool DisplayTaxSuffix
        {
            get
            {
                bool displayTaxSuffix = SettingManager.GetSettingValueBoolean("Tax.DisplayTaxSuffix");
                return displayTaxSuffix;
            }
            set
            {
                SettingManager.SetParam("Tax.DisplayTaxSuffix", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether prices incude tax
        /// </summary>
        public static bool PricesIncludeTax
        {
            get
            {
                bool pricesIncludeTax = SettingManager.GetSettingValueBoolean("Tax.PricesIncludeTax");
                return pricesIncludeTax;
            }
            set
            {
                SettingManager.SetParam("Tax.PricesIncludeTax", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether customers are allowed to select tax display type
        /// </summary>
        public static bool AllowCustomersToSelectTaxDisplayType
        {
            get
            {
                bool allowCustomersToSelectTaxDisplayType = SettingManager.GetSettingValueBoolean("Tax.AllowCustomersToSelectTaxDisplayType");
                return allowCustomersToSelectTaxDisplayType;
            }
            set
            {
                SettingManager.SetParam("Tax.AllowCustomersToSelectTaxDisplayType", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to hide zero tax
        /// </summary>
        public static bool HideZeroTax
        {
            get
            {
                bool hideZeroTax = SettingManager.GetSettingValueBoolean("Tax.HideZeroTax");
                return hideZeroTax;
            }
            set
            {
                SettingManager.SetParam("Tax.HideZeroTax", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to hide tax in order summary when prices are shown tax inclusive
        /// </summary>
        public static bool HideTaxInOrderSummary
        {
            get
            {
                bool hideTaxInOrderSummary = SettingManager.GetSettingValueBoolean("Tax.HideTaxInOrderSummary");
                return hideTaxInOrderSummary;
            }
            set
            {
                SettingManager.SetParam("Tax.HideTaxInOrderSummary", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether shipping price is taxable
        /// </summary>
        public static bool ShippingIsTaxable
        {
            get
            {
                bool shippingIsTaxable = SettingManager.GetSettingValueBoolean("Tax.ShippingIsTaxable");
                return shippingIsTaxable;
            }
            set
            {
                SettingManager.SetParam("Tax.ShippingIsTaxable", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether shipping price incudes tax
        /// </summary>
        public static bool ShippingPriceIncludesTax
        {
            get
            {
                bool shippingPriceIncludesTax = SettingManager.GetSettingValueBoolean("Tax.ShippingPriceIncludesTax");
                return shippingPriceIncludesTax;
            }
            set
            {
                SettingManager.SetParam("Tax.ShippingPriceIncludesTax", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the shipping tax class identifier
        /// </summary>
        public static int ShippingTaxClassID
        {
            get
            {
                int shippingTaxClassID = SettingManager.GetSettingValueInteger("Tax.ShippingTaxClassID");
                return shippingTaxClassID;
            }
            set
            {
                SettingManager.SetParam("Tax.ShippingTaxClassID", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether payment method additional fee is taxable
        /// </summary>
        public static bool PaymentMethodAdditionalFeeIsTaxable
        {
            get
            {
                bool paymentMethodAdditionalFeeIsTaxable = SettingManager.GetSettingValueBoolean("Tax.PaymentMethodAdditionalFeeIsTaxable");
                return paymentMethodAdditionalFeeIsTaxable;
            }
            set
            {
                SettingManager.SetParam("Tax.PaymentMethodAdditionalFeeIsTaxable", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether payment method additional fee incudes tax
        /// </summary>
        public static bool PaymentMethodAdditionalFeeIncludesTax
        {
            get
            {
                bool paymentMethodAdditionalFeeIncludesTax = SettingManager.GetSettingValueBoolean("Tax.PaymentMethodAdditionalFeeIncludesTax");
                return paymentMethodAdditionalFeeIncludesTax;
            }
            set
            {
                SettingManager.SetParam("Tax.PaymentMethodAdditionalFeeIncludesTax", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the payment method additional fee tax class identifier
        /// </summary>
        public static int PaymentMethodAdditionalFeeTaxClassID
        {
            get
            {
                int paymentMethodAdditionalFeeTaxClassID = SettingManager.GetSettingValueInteger("Tax.PaymentMethodAdditionalFeeTaxClassID");
                return paymentMethodAdditionalFeeTaxClassID;
            }
            set
            {
                SettingManager.SetParam("Tax.PaymentMethodAdditionalFeeTaxClassID", value.ToString());
            }
        }

        #endregion
    }
}
