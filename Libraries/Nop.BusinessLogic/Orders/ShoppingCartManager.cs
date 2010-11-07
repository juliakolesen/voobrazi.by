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
using System.Linq;
using System.Text;
using NopSolutions.NopCommerce.BusinessLogic.Caching;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Localization;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Products.Attributes;
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.BusinessLogic.Promo.Discounts;
using NopSolutions.NopCommerce.BusinessLogic.Shipping;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Orders;

namespace NopSolutions.NopCommerce.BusinessLogic.Orders
{
    /// <summary>
    /// Shopping cart manager
    /// </summary>
    public partial class ShoppingCartManager
    {
        #region Utilities
        private static ShoppingCart DBMapping(DBShoppingCart dbCollection)
        {
            if (dbCollection == null)
                return null;

            ShoppingCart collection = new ShoppingCart();
            foreach (DBShoppingCartItem dbItem in dbCollection)
            {
                ShoppingCartItem item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ShoppingCartItem DBMapping(DBShoppingCartItem dbItem)
        {
            if (dbItem == null)
                return null;

            ShoppingCartItem item = new ShoppingCartItem();
            item.ShoppingCartItemID = dbItem.ShoppingCartItemID;
            item.ShoppingCartTypeID = dbItem.ShoppingCartTypeID;
            item.CustomerSessionGUID = dbItem.CustomerSessionGUID;
            item.ProductVariantID = dbItem.ProductVariantID;
            item.AttributesXML = dbItem.AttributesXML;
            item.Quantity = dbItem.Quantity;
            item.CreatedOn = dbItem.CreatedOn;
            item.UpdatedOn = dbItem.UpdatedOn;

            return item;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a shopping cart item
        /// </summary>
        /// <param name="ShoppingCartItemID">The shopping cart item identifier</param>
        /// <param name="ResetCheckoutData">A value indicating whether to reset checkout data</param>
        public static void DeleteShoppingCartItem(int ShoppingCartItemID, bool ResetCheckoutData)
        {
            if (ResetCheckoutData)
            {
                if (NopContext.Current.Session != null)
                {
                    CustomerManager.ResetCheckoutData(NopContext.Current.Session.CustomerID, false);
                }
            }

            ShoppingCartItem shoppingCartItem = GetShoppingCartItemByID(ShoppingCartItemID);
            if (shoppingCartItem != null)
            {
                DBProviderManager<DBShoppingCartProvider>.Provider.DeleteShoppingCartItem(ShoppingCartItemID);
            }
        }

        /// <summary>
        /// Gets a shopping cart by customer session GUID
        /// </summary>
        /// <param name="ShoppingCartType">Shopping cart type</param>
        /// <param name="CustomerSessionGUID">The customer session identifier</param>
        /// <returns>Cart</returns>
        public static ShoppingCart GetShoppingCartByCustomerSessionGUID(ShoppingCartTypeEnum ShoppingCartType, Guid CustomerSessionGUID)
        {
            DBShoppingCart dbCollection = DBProviderManager<DBShoppingCartProvider>.Provider.GetShoppingCartByCustomerSessionGUID((int)ShoppingCartType, CustomerSessionGUID);
            ShoppingCart shoppingCart = DBMapping(dbCollection);
            return shoppingCart;
        }

        /// <summary>
        /// Gets a shopping cart item
        /// </summary>
        /// <param name="ShoppingCartItemID">The shopping cart item identifier</param>
        /// <returns>Shopping cart item</returns>
        public static ShoppingCartItem GetShoppingCartItemByID(int ShoppingCartItemID)
        {
            if (ShoppingCartItemID == 0)
                return null;

            DBShoppingCartItem dbItem = DBProviderManager<DBShoppingCartProvider>.Provider.GetShoppingCartItemByID(ShoppingCartItemID);
            ShoppingCartItem shoppingCartItem = DBMapping(dbItem);
            return shoppingCartItem;
        }

        /// <summary>
        /// Inserts a shopping cart item
        /// </summary>
        /// <param name="ShoppingCartType">The shopping cart type</param>
        /// <param name="CustomerSessionGUID">The customer session identifier</param>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <param name="AttributesXML">The product variant attributes</param>
        /// <param name="Quantity">The quantity</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Shopping cart item</returns>
        internal static ShoppingCartItem InsertShoppingCartItem(ShoppingCartTypeEnum ShoppingCartType, Guid CustomerSessionGUID,
          int ProductVariantID, string AttributesXML, int Quantity,
           DateTime CreatedOn, DateTime UpdatedOn)
        {
            if (AttributesXML == null)
                AttributesXML = string.Empty;

            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            UpdatedOn = DateTimeHelper.ConvertToUtcTime(UpdatedOn);

            DBShoppingCartItem dbItem = DBProviderManager<DBShoppingCartProvider>.Provider.InsertShoppingCartItem((int)ShoppingCartType,
                CustomerSessionGUID, ProductVariantID, AttributesXML,
                Quantity, CreatedOn, UpdatedOn);

            ShoppingCartItem shoppingCartItem = DBMapping(dbItem);
            return shoppingCartItem;
        }

        /// <summary>
        /// Updates the shopping cart item
        /// </summary>
        /// <param name="ShoppingCartItemID">The shopping cart item identifier</param>
        /// <param name="ShoppingCartType">The shopping cart type</param>
        /// <param name="CustomerSessionGUID">The customer session identifier</param>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <param name="AttributesXML">The product variant attributes</param>
        /// <param name="Quantity">The quantity</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Shopping cart item</returns>
        internal static ShoppingCartItem UpdateShoppingCartItem(int ShoppingCartItemID,
            ShoppingCartTypeEnum ShoppingCartType, Guid CustomerSessionGUID,
           int ProductVariantID, string AttributesXML, int Quantity,
            DateTime CreatedOn, DateTime UpdatedOn)
        {
            if (ShoppingCartItemID == 0)
                return null;

            if (AttributesXML == null)
                AttributesXML = string.Empty;

            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            UpdatedOn = DateTimeHelper.ConvertToUtcTime(UpdatedOn);

            DBShoppingCartItem dbItem = DBProviderManager<DBShoppingCartProvider>.Provider.UpdateShoppingCartItem(ShoppingCartItemID, (int)ShoppingCartType,
                CustomerSessionGUID, ProductVariantID, AttributesXML, 
                Quantity, CreatedOn, UpdatedOn);

            ShoppingCartItem shoppingCartItem = DBMapping(dbItem);
            return shoppingCartItem;
        }

        /// <summary>
        /// Gets current user shopping cart
        /// </summary>
        /// <param name="ShoppingCartType">Shopping cart type</param>
        /// <returns>Cart</returns>
        public static ShoppingCart GetCurrentShoppingCart(ShoppingCartTypeEnum ShoppingCartType)
        {
            if (NopContext.Current.Session == null)
                return new ShoppingCart();
            Guid CustomerSessionGUID = NopContext.Current.Session.CustomerSessionGUID;
            return GetShoppingCartByCustomerSessionGUID(ShoppingCartType, CustomerSessionGUID);
        }

        /// <summary>
        /// Gets shopping cart
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="ShoppingCartType">Shopping cart type</param>
        /// <returns>Cart</returns>
        public static ShoppingCart GetCustomerShoppingCart(int CustomerID, ShoppingCartTypeEnum ShoppingCartType)
        {
            CustomerSession customerSession = CustomerManager.GetCustomerSessionByCustomerID(CustomerID);
            if (customerSession == null)
                return new ShoppingCart();
            Guid CustomerSessionGUID = customerSession.CustomerSessionGUID;
            return GetShoppingCartByCustomerSessionGUID(ShoppingCartType, CustomerSessionGUID);
        }

        /// <summary>
        /// Gets shopping cart total
        /// </summary>
        /// <param name="Cart">Cart</param>
        /// <param name="customer">Customer</param>
        /// <returns>Shopping cart total;Null if shopping cart total couldn't be calculated now</returns>
        public static decimal? GetShoppingCartTotal(ShoppingCart Cart, Customer customer)
        {
            return GetShoppingCartTotal(Cart, 0, customer);
        }

        /// <summary>
        /// Gets shopping cart total
        /// </summary>
        /// <param name="Cart">Cart</param>
        /// <param name="PaymentMethodID">Payment method identifier</param>
        /// <param name="customer">Customer</param>
        /// <returns>Shopping cart total;Null if shopping cart total couldn't be calculated now</returns>
        public static decimal? GetShoppingCartTotal(ShoppingCart Cart, int PaymentMethodID, Customer customer)
        {
            string SubTotalError = string.Empty;
            string ShippingError = string.Empty;
            string PaymentMethodAdditionalFeeError = string.Empty;
            string TaxError = string.Empty;

            //subtotal without tax
            decimal shoppingCartSubTotalDiscount;
            decimal shoppingCartSubTotalExclTax = GetShoppingCartSubTotal(Cart, customer, out shoppingCartSubTotalDiscount, false, ref SubTotalError);
            if (!String.IsNullOrEmpty(SubTotalError))
                return null;

            //shipping without tax
            decimal? shoppingCartShipping = ShippingManager.GetShoppingCartShippingTotal(Cart, customer, false, ref ShippingError);
            if (!String.IsNullOrEmpty(ShippingError))
                return null;

            //payment method additional fee without tax
            decimal paymentMethodAdditionalFeeWithoutTax = decimal.Zero;
            if (PaymentMethodID > 0)
            {
                decimal paymentMethodAdditionalFee = PaymentManager.GetAdditionalHandlingFee(PaymentMethodID);
                paymentMethodAdditionalFeeWithoutTax = TaxManager.GetPaymentMethodAdditionalFee(paymentMethodAdditionalFee, false, customer, ref PaymentMethodAdditionalFeeError);
            }

            //tax
            decimal shoppingCartTax = TaxManager.GetTaxTotal(Cart, PaymentMethodID, customer, ref TaxError);
            if (!String.IsNullOrEmpty(TaxError))
                return null;

            if (shoppingCartShipping.HasValue)
                return Math.Round(shoppingCartSubTotalExclTax + shoppingCartShipping.Value + paymentMethodAdditionalFeeWithoutTax + shoppingCartTax, 2);
            else
                return null;
        }

        /// <summary>
        /// Gets shopping cart subtotal
        /// </summary>
        /// <param name="Cart">Cart</param>
        /// <param name="customer">Customer</param>
        /// <returns>Shopping cart subtotal</returns>
        public static decimal GetShoppingCartSubTotal(ShoppingCart Cart, Customer customer)
        {
            string Error = string.Empty;
            return GetShoppingCartSubTotal(Cart, customer, ref Error);
        }
        
        /// <summary>
        /// Gets shopping cart subtotal
        /// </summary>
        /// <param name="Cart">Cart</param>
        /// <param name="customer">Customer</param>
        /// <param name="Error">Error</param>
        /// <returns>Shopping cart subtotal</returns>
        public static decimal GetShoppingCartSubTotal(ShoppingCart Cart, Customer customer, 
            ref string Error)
        {
            decimal SubTotalDiscount = decimal.Zero;
            return GetShoppingCartSubTotal(Cart, customer, out SubTotalDiscount, ref Error);
        }
        
        /// <summary>
        /// Gets shopping cart subtotal
        /// </summary>
        /// <param name="Cart">Cart</param>
        /// <param name="customer">Customer</param>
        /// <param name="SubTotalDiscount">Subtotal discount</param>
        /// <returns>Shopping cart subtotal</returns>
        public static decimal GetShoppingCartSubTotal(ShoppingCart Cart, Customer customer, 
            out decimal SubTotalDiscount)
        {
            string Error = string.Empty;
            return GetShoppingCartSubTotal(Cart, customer, out SubTotalDiscount, ref Error);
        }
        
        /// <summary>
        /// Gets shopping cart subtotal
        /// </summary>
        /// <param name="Cart">Cart</param>
        /// <param name="customer">Customer</param>
        /// <param name="SubTotalDiscount">Subtotal discount</param>
        /// <param name="Error">Error</param>
        /// <returns>Shopping cart subtotal</returns>
        public static decimal GetShoppingCartSubTotal(ShoppingCart Cart, Customer customer,
            out decimal SubTotalDiscount, ref string Error)
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
            return GetShoppingCartSubTotal(Cart, customer, out SubTotalDiscount, includingTax, ref Error);
        }
        
        /// <summary>
        /// Gets shopping cart subtotal
        /// </summary>
        /// <param name="Cart">Cart</param>
        /// <param name="customer">Customer</param>
        /// <param name="SubTotalDiscount">Subtotal discount</param>
        /// <param name="includingTax">A value indicating whether calculated price should include tax</param>
        /// <returns>Shopping cart subtotal</returns>
        public static decimal GetShoppingCartSubTotal(ShoppingCart Cart, Customer customer, 
            out decimal SubTotalDiscount, bool includingTax)
        {
            string Error = string.Empty;
            return GetShoppingCartSubTotal(Cart, customer, out SubTotalDiscount, includingTax, ref Error);
        }
        
        /// <summary>
        /// Gets shopping cart subtotal
        /// </summary>
        /// <param name="Cart">Cart</param>
        /// <param name="customer">Customer</param>
        /// <param name="SubTotalDiscount">Subtotal discount</param>
        /// <param name="includingTax">A value indicating whether calculated price should include tax</param>
        /// <param name="Error">Error</param>
        /// <returns>Shopping cart subtotal</returns>
        public static decimal GetShoppingCartSubTotal(ShoppingCart Cart, Customer customer,
            out decimal SubTotalDiscount, bool includingTax, ref string Error)
        {
            SubTotalDiscount = decimal.Zero;

            decimal subTotalWithoutDiscount = decimal.Zero;
            decimal subTotalWithDiscount = decimal.Zero;
            foreach (ShoppingCartItem shoppingCartItem in Cart)
            {
                string Error2 = string.Empty;
                decimal scSubTotal = PriceHelper.GetSubTotal(shoppingCartItem, customer, true);
                subTotalWithoutDiscount += TaxManager.GetPrice(shoppingCartItem.ProductVariant, scSubTotal, includingTax, customer, ref Error2);
                if (!String.IsNullOrEmpty(Error2))
                {
                    Error = Error2;
                }
            }


            SubTotalDiscount = GetOrderDiscount(customer, subTotalWithoutDiscount);

            subTotalWithDiscount = subTotalWithoutDiscount - SubTotalDiscount;
            if (subTotalWithDiscount < decimal.Zero)
                subTotalWithDiscount = decimal.Zero;

            subTotalWithDiscount = Math.Round(subTotalWithDiscount, 2);

            return subTotalWithDiscount;
        }

        /// <summary>
        /// Gets an order discount
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="orderSubTotal">Order subtotal</param>
        /// <returns>Order discount</returns>
        public static decimal GetOrderDiscount(Customer customer, decimal orderSubTotal)
        {
            decimal SubTotalDiscount = decimal.Zero;
            int customerID = 0;
            if (customer != null)
                customerID = customer.CustomerID;

            string customerCouponCode = string.Empty;
            if (customer != null)
                customerCouponCode = customer.LastAppliedCouponCode;

            DiscountCollection allDiscounts = DiscountManager.GetAllDiscounts(DiscountTypeEnum.AssignedToWholeOrder);
            DiscountCollection allowedDiscounts = new DiscountCollection();
            foreach (Discount _discount in allDiscounts)
            {
                if (_discount.IsActive(customerCouponCode) &&
                    _discount.DiscountType == DiscountTypeEnum.AssignedToWholeOrder &&
                    !allowedDiscounts.ContainsDiscount(_discount.Name))
                {
                    switch (_discount.DiscountRequirement)
                    {
                        case DiscountRequirementEnum.None:
                            {
                                allowedDiscounts.Add(_discount);
                            }
                            break;
                        case DiscountRequirementEnum.MustBeAssignedToCustomerRole:
                            {
                                if (_discount.CheckCustomerRoleRequirement(customerID))
                                    allowedDiscounts.Add(_discount);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            Discount preferredDiscount = DiscountManager.GetPreferredDiscount(allowedDiscounts, orderSubTotal);
            if (preferredDiscount != null)
            {
                SubTotalDiscount = preferredDiscount.GetDiscountAmount(orderSubTotal);
            }

            if (SubTotalDiscount < decimal.Zero)
                SubTotalDiscount = decimal.Zero;

            SubTotalDiscount = Math.Round(SubTotalDiscount, 2);

            return SubTotalDiscount;
        }
        
        /// <summary>
        /// Validates whether this shopping cart item is allowed
        /// </summary>
        /// <param name="ShoppingCartType">Shopping cart type</param>
        /// <param name="ProductVariantID">Product variant identifier</param>
        /// <param name="SelectedAttributes">Selected attributes</param>
        /// <param name="Quantity">Quantity</param>
        /// <returns>Warnings</returns>
        public static List<string> GetShoppingCartItemWarnings(ShoppingCartTypeEnum ShoppingCartType,
            int ProductVariantID, string SelectedAttributes, int Quantity)
        {
            List<string> warnings = new List<string>();
            ProductVariant productVariant = ProductManager.GetProductVariantByID(ProductVariantID);
            if (productVariant == null)
            {
                warnings.Add(string.Format("Product variant (ID={0}) can not be loaded", ProductVariantID));
                return warnings;
            }

            Product product = productVariant.Product;
            if (product == null)
            {
                warnings.Add(string.Format("Product (ID={0}) can not be loaded", productVariant.ProductID));
                return warnings;
            }

            if (product.Deleted || productVariant.Deleted)
            {
                warnings.Add("Product is deleted");
                return warnings;
            }

            if (!product.Published || !productVariant.Published)
            {
                warnings.Add("Product is not published");
            }

            if (productVariant.DisableBuyButton)
            {
                warnings.Add("Buying is disabled");
            }

            if (Quantity < productVariant.OrderMinimumQuantity)
            {
                warnings.Add(string.Format(LocalizationManager.GetLocaleResourceString("ShoppingCart.MinimumQuantity"), productVariant.OrderMinimumQuantity));
            }

            if (Quantity > productVariant.OrderMaximumQuantity)
            {
                warnings.Add(string.Format(LocalizationManager.GetLocaleResourceString("ShoppingCart.MaximumQuantity"), productVariant.OrderMaximumQuantity));
            }

            if (productVariant.ManageInventory)
            {
                if (productVariant.StockQuantity < Quantity)
                {
                    int maximumQuantityCanBeAdded = productVariant.StockQuantity;
                    warnings.Add(string.Format(LocalizationManager.GetLocaleResourceString("ShoppingCart.QuantityExceedsStock"), maximumQuantityCanBeAdded));
                }
            }

            if (productVariant.AvailableStartDateTime.HasValue)
            {
                DateTime now = DateTimeHelper.ConvertToUtcTime(DateTime.Now);
                if (productVariant.AvailableStartDateTime.Value.CompareTo(now) > 0)
                {
                    warnings.Add("Product is not available");
                }
            }
            else if (productVariant.AvailableEndDateTime.HasValue)
            {
                DateTime now = DateTimeHelper.ConvertToUtcTime(DateTime.Now);
                if (productVariant.AvailableEndDateTime.Value.CompareTo(now) < 0)
                {
                    warnings.Add("Product is not available");
                }
            }

            //selected attributes
            ProductVariantAttributeCollection pva1Collection = ProductAttributeHelper.ParseProductVariantAttributes(SelectedAttributes);
            foreach (ProductVariantAttribute pva1 in pva1Collection)
            {
                ProductVariant pv1 = pva1.ProductVariant;
                if (pv1 != null)
                {
                    if (pv1.ProductVariantID != productVariant.ProductVariantID)
                    {
                        warnings.Add("Attribute error");
                    }
                }
                else
                {
                    warnings.Add("Attribute error");
                    return warnings;
                }
            }

            //existing product attributes
            ProductVariantAttributeCollection pva2Collection = productVariant.ProductVariantAttributes;
            foreach (ProductVariantAttribute pva2 in pva2Collection)
            {
                if (pva2.IsRequired)
                {
                    bool found = false;
                    //selected attributes
                    foreach (ProductVariantAttribute pva1 in pva1Collection)
                    {
                        if (pva1.ProductVariantAttributeID == pva2.ProductVariantAttributeID)
                        {
                            List<string> pvaValuesStr = ProductAttributeHelper.ParseValues(SelectedAttributes, pva1.ProductVariantAttributeID);
                            foreach (string str1 in pvaValuesStr)
                            {
                                if (!String.IsNullOrEmpty(str1.Trim()))
                                {
                                    found = true;
                                    break;
                                }
                            }
                        }
                    }

                    //if not found
                    if (!found)
                    {
                        if (!string.IsNullOrEmpty(pva2.TextPrompt))
                        {
                            warnings.Add(pva2.TextPrompt);
                        }
                        else
                        {
                            warnings.Add(string.Format(LocalizationManager.GetLocaleResourceString("ShoppingCart.SelectAttribute"), pva2.ProductAttribute.Name));
                        }
                    }
                }
            }

            return warnings;
        }

        /// <summary>
        /// Add a product variant to shopping cart
        /// </summary>
        /// <param name="ShoppingCartType">Shopping cart type</param>
        /// <param name="ProductVariantID">Product variant identifier</param>
        /// <param name="SelectedAttributes">Selected attributes</param>
        /// <param name="Quantity">Quantity</param>
        /// <returns>Warnings</returns>
        public static List<string> AddToCart(ShoppingCartTypeEnum ShoppingCartType, int ProductVariantID,
            string SelectedAttributes, int Quantity)
        {
            List<string> warnings = new List<string>();
            if (ShoppingCartType == ShoppingCartTypeEnum.Wishlist && !SettingManager.GetSettingValueBoolean("Common.EnableWishlist"))
                return warnings;

            if (NopContext.Current.Session == null)
                NopContext.Current.Session = NopContext.Current.GetSession(true);

            Guid CustomerSessionGUID = NopContext.Current.Session.CustomerSessionGUID;

            CustomerManager.ResetCheckoutData(NopContext.Current.Session.CustomerID, false);

            ShoppingCart Cart = GetShoppingCartByCustomerSessionGUID(ShoppingCartType, CustomerSessionGUID);
            ShoppingCartItem shoppingCartItem = null;


            foreach (ShoppingCartItem _shoppingCartItem in Cart)
            {
                if (_shoppingCartItem.ProductVariantID == ProductVariantID)
                {
                    if (ProductAttributeHelper.ParseProductVariantAttributeIDs(_shoppingCartItem.AttributesXML).Count == ProductAttributeHelper.ParseProductVariantAttributeIDs(SelectedAttributes).Count)
                    {
                        bool attributeEquals = true;

                        ProductVariantAttributeCollection pva1Collection = ProductAttributeHelper.ParseProductVariantAttributes(SelectedAttributes);
                        ProductVariantAttributeCollection pva2Collection = ProductAttributeHelper.ParseProductVariantAttributes(_shoppingCartItem.AttributesXML);
                        foreach (ProductVariantAttribute pva1 in pva1Collection)
                        {
                            foreach (ProductVariantAttribute pva2 in pva2Collection)
                            {
                                if (pva1.ProductVariantAttributeID == pva2.ProductVariantAttributeID)
                                {
                                    List<string> pvaValues1Str = ProductAttributeHelper.ParseValues(SelectedAttributes, pva1.ProductVariantAttributeID);
                                    List<string> pvaValues2Str = ProductAttributeHelper.ParseValues(_shoppingCartItem.AttributesXML, pva2.ProductVariantAttributeID);
                                    if (pvaValues1Str.Count == pvaValues2Str.Count)
                                    {
                                        foreach (string str1 in pvaValues1Str)
                                        {
                                            bool hasAttribute = false;
                                            foreach (string str2 in pvaValues2Str)
                                            {
                                                if (str1.Trim().ToLower() == str2.Trim().ToLower())
                                                {
                                                    hasAttribute = true;
                                                    break;
                                                }
                                            }

                                            if (!hasAttribute)
                                            {
                                                attributeEquals = false;
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        attributeEquals = false;
                                        break;
                                    }
                                }
                            }
                        }
                        if (attributeEquals)
                            shoppingCartItem = _shoppingCartItem;
                    }
                }
            }

            DateTime now = DateTime.Now;
            if (shoppingCartItem != null)
            {
                int newQuantity = shoppingCartItem.Quantity + Quantity;
                warnings.AddRange(GetShoppingCartItemWarnings(ShoppingCartType, ProductVariantID, 
                    SelectedAttributes, newQuantity));

                if (warnings.Count == 0)
                {
                    UpdateShoppingCartItem(shoppingCartItem.ShoppingCartItemID, ShoppingCartType, 
                        CustomerSessionGUID, ProductVariantID, SelectedAttributes, newQuantity, shoppingCartItem.CreatedOn, now);
                }
            }
            else
            {
                warnings.AddRange(GetShoppingCartItemWarnings(ShoppingCartType, ProductVariantID, 
                    SelectedAttributes, Quantity));
                if (warnings.Count == 0)
                {
                    InsertShoppingCartItem(ShoppingCartType, CustomerSessionGUID, ProductVariantID, 
                        SelectedAttributes, Quantity, now, now);
                }
            }

            return warnings;
        }

        /// <summary>
        /// Updates the shopping cart item
        /// </summary>
        /// <param name="ShoppingCartItemID">Shopping cart item identifier</param>
        /// <param name="NewQuantity">New shopping cart item quantity</param>
        /// <param name="ResetCheckoutData">A value indicating whether to reset checkout data</param>
        /// <returns>Warnings</returns>
        public static List<string> UpdateCart(int ShoppingCartItemID, int NewQuantity,
            bool ResetCheckoutData)
        {
            List<string> warnings = new List<string>();

            if (NopContext.Current.Session == null)
                return warnings;

            ShoppingCartItem shoppingCartItem = GetShoppingCartItemByID(ShoppingCartItemID);
            if (shoppingCartItem != null)
            {
                if (ResetCheckoutData)
                {
                    CustomerManager.ResetCheckoutData(NopContext.Current.Session.CustomerID, false);
                }
                if (NewQuantity > 0)
                {
                    warnings.AddRange(GetShoppingCartItemWarnings(shoppingCartItem.ShoppingCartType,
                        shoppingCartItem.ProductVariantID, shoppingCartItem.AttributesXML, NewQuantity));
                    if (warnings.Count == 0)
                    {
                        UpdateShoppingCartItem(shoppingCartItem.ShoppingCartItemID, shoppingCartItem.ShoppingCartType, shoppingCartItem.CustomerSessionGUID,
                            shoppingCartItem.ProductVariantID, shoppingCartItem.AttributesXML,
                            NewQuantity, shoppingCartItem.CreatedOn, DateTime.Now);
                    }
                }
                else
                {
                    DeleteShoppingCartItem(shoppingCartItem.ShoppingCartItemID, ResetCheckoutData);
                }
            }

            return warnings;
        }

        #endregion
    }
}