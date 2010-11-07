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

namespace NopSolutions.NopCommerce.DataAccess.Promo.Discounts
{
    /// <summary>
    /// Acts as a base class for deriving custom discount provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/DiscountProvider")]
    public abstract partial class DBDiscountProvider : BaseDBProvider
    {
        #region Methods

        /// <summary>
        /// Gets a discount
        /// </summary>
        /// <param name="DiscountID">Discount identifier</param>
        /// <returns>Discount</returns>
        public abstract DBDiscount GetDiscountByID(int DiscountID);

        /// <summary>
        /// Gets all discounts
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="DiscountTypeID">Discount type identifier; null to load all discount</param>
        /// <returns>Discount collection</returns>
        public abstract DBDiscountCollection GetAllDiscounts(bool showHidden, int? DiscountTypeID);

        /// <summary>
        /// Inserts a discount
        /// </summary>
        /// <param name="DiscountTypeID">The discount type identifier</param>
        /// <param name="DiscountRequirementID">The discount requirement identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="UsePercentage">A value indicating whether to use percentage</param>
        /// <param name="DiscountPercentage">The discount percentage</param>
        /// <param name="DiscountAmount">The discount amount</param>
        /// <param name="StartDate">The discount start date and time</param>
        /// <param name="EndDate">The discount end date and time</param>
        /// <param name="RequiresCouponCode">The value indicating whether discount requires coupon code</param>
        /// <param name="CouponCode">The coupon code</param>
        /// <param name="Deleted">A value indicating whether the entity has been deleted</param>
        /// <returns>Discount</returns>
        public abstract DBDiscount InsertDiscount(int DiscountTypeID, int DiscountRequirementID, 
            string Name, bool UsePercentage, decimal DiscountPercentage, decimal DiscountAmount,
            DateTime StartDate, DateTime EndDate, bool RequiresCouponCode, string CouponCode, bool Deleted);

        /// <summary>
        /// Updates the discount
        /// </summary>
        /// <param name="DiscountID">Discount identifier</param>
        /// <param name="DiscountTypeID">The discount type identifier</param>
        /// <param name="DiscountRequirementID">The discount requirement identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="UsePercentage">A value indicating whether to use percentage</param>
        /// <param name="DiscountPercentage">The discount percentage</param>
        /// <param name="DiscountAmount">The discount amount</param>
        /// <param name="StartDate">The discount start date and time</param>
        /// <param name="EndDate">The discount end date and time</param>
        /// <param name="RequiresCouponCode">The value indicating whether discount requires coupon code</param>
        /// <param name="CouponCode">The coupon code</param>
        /// <param name="Deleted">A value indicating whether the entity has been deleted</param>
        /// <returns>Discount</returns>
        public abstract DBDiscount UpdateDiscount(int DiscountID, int DiscountTypeID, int DiscountRequirementID, string Name, bool UsePercentage, decimal DiscountPercentage, decimal DiscountAmount,
            DateTime StartDate, DateTime EndDate, bool RequiresCouponCode, string CouponCode, bool Deleted);

        /// <summary>
        /// Adds a discount to a product variant
        /// </summary>
        /// <param name="ProductVariantID">Product variant identifier</param>
        /// <param name="DiscountID">Discount identifier</param>
        public abstract void AddDiscountToProductVariant(int ProductVariantID, int DiscountID);

        /// <summary>
        /// Removes a discount from a product variant
        /// </summary>
        /// <param name="ProductVariantID">Product variant identifier</param>
        /// <param name="DiscountID">Discount identifier</param>
        public abstract void RemoveDiscountFromProductVariant(int ProductVariantID, int DiscountID);

        /// <summary>
        /// Gets a discount collection of a product variant
        /// </summary>
        /// <param name="ProductVariantID">Product variant identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Discount collection</returns>
        public abstract DBDiscountCollection GetDiscountsByProductVariantID(int ProductVariantID, bool showHidden);

        /// <summary>
        /// Adds a discount to a category
        /// </summary>
        /// <param name="CategoryID">Category identifier</param>
        /// <param name="DiscountID">Discount identifier</param>
        public abstract void AddDiscountToCategory(int CategoryID, int DiscountID);

        /// <summary>
        /// Removes a discount from a category
        /// </summary>
        /// <param name="CategoryID">Category identifier</param>
        /// <param name="DiscountID">Discount identifier</param>
        public abstract void RemoveDiscountFromCategory(int CategoryID, int DiscountID);

        /// <summary>
        /// Gets a discount collection of a category
        /// </summary>
        /// <param name="CategoryID">Category identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Discount collection</returns>
        public abstract DBDiscountCollection GetDiscountsByCategoryID(int CategoryID, bool showHidden);

        /// <summary>
        /// Gets all discount requirements
        /// </summary>
        /// <returns>Discount requirement collection</returns>
        public abstract DBDiscountRequirementCollection GetAllDiscountRequirements();

        /// <summary>
        /// Gets all discount types
        /// </summary>
        /// <returns>Discount type collection</returns>
        public abstract DBDiscountTypeCollection GetAllDiscountTypes();        
        #endregion
    }
}
