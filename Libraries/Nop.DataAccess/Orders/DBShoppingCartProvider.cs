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
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Web.Configuration;
using System.Web.Hosting;

namespace NopSolutions.NopCommerce.DataAccess.Orders
{
    /// <summary>
    /// Acts as a base class for deriving custom shopping cart provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/ShoppingCartProvider")]
    public abstract partial class DBShoppingCartProvider : BaseDBProvider
    {
        #region Methods
        /// <summary>
        /// Deletes a shopping cart item
        /// </summary>
        /// <param name="ShoppingCartItemID">The shopping cart item identifier</param>
        public abstract void DeleteShoppingCartItem(int ShoppingCartItemID);

        /// <summary>
        /// Gets a shopping cart by customer session GUID
        /// </summary>
        /// <param name="ShoppingCartTypeID">Shopping cart type identifier</param>
        /// <param name="CustomerSessionGUID">The customer session identifier</param>
        /// <returns>Cart</returns>
        public abstract DBShoppingCart GetShoppingCartByCustomerSessionGUID(int ShoppingCartTypeID, Guid CustomerSessionGUID);

        /// <summary>
        /// Gets a shopping cart item
        /// </summary>
        /// <param name="ShoppingCartItemID">The shopping cart item identifier</param>
        /// <returns>Shopping cart item</returns>
        public abstract DBShoppingCartItem GetShoppingCartItemByID(int ShoppingCartItemID);

        /// <summary>
        /// Inserts a shopping cart item
        /// </summary>
        /// <param name="ShoppingCartTypeID">The shopping cart type identifier</param>
        /// <param name="CustomerSessionGUID">The customer session identifier</param>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <param name="AttributesXML">The product variant attributes</param>
        /// <param name="Quantity">The quantity</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Shopping cart item</returns>
        public abstract DBShoppingCartItem InsertShoppingCartItem(int ShoppingCartTypeID, Guid CustomerSessionGUID,
          int ProductVariantID, string AttributesXML, int Quantity,
           DateTime CreatedOn, DateTime UpdatedOn);

        /// <summary>
        /// Updates the shopping cart item
        /// </summary>
        /// <param name="ShoppingCartItemID">The shopping cart item identifier</param>
        /// <param name="ShoppingCartTypeID">The shopping cart type identifier</param>
        /// <param name="CustomerSessionGUID">The customer session identifier</param>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <param name="AttributesXML">The product variant attributes</param>
        /// <param name="Quantity">The quantity</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Shopping cart item</returns>
        public abstract DBShoppingCartItem UpdateShoppingCartItem(int ShoppingCartItemID, int ShoppingCartTypeID, Guid CustomerSessionGUID,
          int ProductVariantID, string AttributesXML, int Quantity,
           DateTime CreatedOn, DateTime UpdatedOn);
        #endregion
    }
}

