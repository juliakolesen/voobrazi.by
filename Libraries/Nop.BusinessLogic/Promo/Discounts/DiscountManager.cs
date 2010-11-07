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
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.Common;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Promo.Discounts;


namespace NopSolutions.NopCommerce.BusinessLogic.Promo.Discounts
{
    /// <summary>
    /// Discount manager
    /// </summary>
    public partial class DiscountManager
    {
        #region Constants
        private const string DISCOUNTS_ALL_KEY = "Nop.discount.all-{0}-{1}";
        private const string DISCOUNTS_BY_ID_KEY = "Nop.discount.id-{0}";
        private const string DISCOUNTS_BY_PRODUCTVARIANTID_KEY = "Nop.discount.byproductvariantid-{0}-{1}";
        private const string DISCOUNTS_BY_CATEGORYID_KEY = "Nop.discount.bycategoryid-{0}-{1}";
        private const string DISCOUNTTYPES_ALL_KEY = "Nop.discounttype.all";
        private const string DISCOUNTREQUIREMENT_ALL_KEY = "Nop.discountrequirement.all";
        private const string DISCOUNTS_PATTERN_KEY = "Nop.discount.";
        private const string DISCOUNTTYPES_PATTERN_KEY = "Nop.discounttype.";
        private const string DISCOUNTREQUIREMENT_PATTERN_KEY = "Nop.discountrequirement.";
        #endregion

        #region Utilities
        private static DiscountCollection DBMapping(DBDiscountCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            DiscountCollection collection = new DiscountCollection();
            foreach (DBDiscount dbItem in dbCollection)
            {
                Discount item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static Discount DBMapping(DBDiscount dbItem)
        {
            if (dbItem == null)
                return null;

            Discount item = new Discount();
            item.DiscountID = dbItem.DiscountID;
            item.DiscountTypeID = dbItem.DiscountTypeID;
            item.DiscountRequirementID = dbItem.DiscountRequirementID;
            item.Name = dbItem.Name;
            item.UsePercentage = dbItem.UsePercentage;
            item.DiscountPercentage = dbItem.DiscountPercentage;
            item.DiscountAmount = dbItem.DiscountAmount;
            item.StartDate = dbItem.StartDate;
            item.EndDate = dbItem.EndDate;
            item.RequiresCouponCode = dbItem.RequiresCouponCode;
            item.CouponCode = dbItem.CouponCode;
            item.Deleted = dbItem.Deleted;

            return item;
        }

        private static DiscountRequirementCollection DBMapping(DBDiscountRequirementCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            DiscountRequirementCollection collection = new DiscountRequirementCollection();
            foreach (DBDiscountRequirement dbItem in dbCollection)
            {
                DiscountRequirement item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static DiscountRequirement DBMapping(DBDiscountRequirement dbItem)
        {
            if (dbItem == null)
                return null;

            DiscountRequirement item = new DiscountRequirement();
            item.DiscountRequirementID = dbItem.DiscountRequirementID;
            item.Name = dbItem.Name;

            return item;
        }

        private static DiscountTypeCollection DBMapping(DBDiscountTypeCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            DiscountTypeCollection collection = new DiscountTypeCollection();
            foreach (DBDiscountType dbItem in dbCollection)
            {
                DiscountType item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static DiscountType DBMapping(DBDiscountType dbItem)
        {
            if (dbItem == null)
                return null;

            DiscountType item = new DiscountType();
            item.DiscountTypeID = dbItem.DiscountTypeID;
            item.Name = dbItem.Name;

            return item;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Gets a preferred discount
        /// </summary>
        /// <param name="Discounts">Discounts to analyze</param>
        /// <param name="Amount">Amount</param>
        /// <returns>Preferred discount</returns>
        public static Discount GetPreferredDiscount(DiscountCollection Discounts, decimal Amount)
        {
            Discount preferredDiscount = null;
            decimal maximumDiscountValue = decimal.Zero;
            foreach (Discount _discount in Discounts)
            {
                decimal currentDiscountValue = _discount.GetDiscountAmount(Amount);
                if (currentDiscountValue > maximumDiscountValue)
                {
                    maximumDiscountValue = currentDiscountValue;
                    preferredDiscount = _discount;
                }
            }

            return preferredDiscount;
        }

        /// <summary>
        /// Gets a discount
        /// </summary>
        /// <param name="DiscountID">Discount identifier</param>
        /// <returns>Discount</returns>
        public static Discount GetDiscountByID(int DiscountID)
        {
            if (DiscountID == 0)
                return null;

            string key = string.Format(DISCOUNTS_BY_ID_KEY, DiscountID);
            object obj2 = NopCache.Get(key);
            if (DiscountManager.CacheEnabled && (obj2 != null))
            {
                return (Discount)obj2;
            }

            DBDiscount dbItem = DBProviderManager<DBDiscountProvider>.Provider.GetDiscountByID(DiscountID);
            Discount discount = DBMapping(dbItem);

            if (DiscountManager.CacheEnabled)
            {
                NopCache.Max(key, discount);
            }
            return discount;
        }

        /// <summary>
        /// Marks discount as deleted
        /// </summary>
        /// <param name="DiscountID">Discount identifier</param>
        public static void MarkDiscountAsDeleted(int DiscountID)
        {
            Discount discount = GetDiscountByID(DiscountID);
            if (discount != null)
            {
                UpdateDiscount(discount.DiscountID, discount.DiscountType, discount.DiscountRequirement, discount.Name,
                    discount.UsePercentage, discount.DiscountPercentage,
                    discount.DiscountAmount, discount.StartDate,
                    discount.EndDate, discount.RequiresCouponCode,
                    discount.CouponCode, true);
            }
        }

        /// <summary>
        /// Get a value indicating whether discounts that require coupon code exist
        /// </summary>
        /// <returns>A value indicating whether discounts that require coupon code exist</returns>
        public static bool HasDiscountsWithCouponCode()
        {
            DiscountCollection discounts = GetAllDiscounts(null);
            return discounts.Find(d => d.RequiresCouponCode) != null;
        }

        /// <summary>
        /// Gets all discounts
        /// </summary>
        /// <param name="DiscountType">Discount type; null to load all discount</param>
        /// <returns>Discount collection</returns>
        public static DiscountCollection GetAllDiscounts(DiscountTypeEnum? DiscountType)
        {
            bool showHidden = NopContext.Current.IsAdmin;
            string key = string.Format(DISCOUNTS_ALL_KEY, showHidden, DiscountType);
            object obj2 = NopCache.Get(key);
            if (DiscountManager.CacheEnabled && (obj2 != null))
            {
                return (DiscountCollection)obj2;
            }

            int? discountTypeID = null;
            if (DiscountType.HasValue)
                discountTypeID = (int)DiscountType.Value;

            DBDiscountCollection dbCollection = DBProviderManager<DBDiscountProvider>.Provider.GetAllDiscounts(showHidden, discountTypeID);
            DiscountCollection discounts = DBMapping(dbCollection);

            if (DiscountManager.CacheEnabled)
            {
                NopCache.Max(key, discounts);
            }
            return discounts;
        }

        /// <summary>
        /// Inserts a discount
        /// </summary>
        /// <param name="DiscountType">The discount type</param>
        /// <param name="DiscountRequirement">The discount requirement</param>
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
        public static Discount InsertDiscount(DiscountTypeEnum DiscountType, DiscountRequirementEnum DiscountRequirement,
            string Name, bool UsePercentage, decimal DiscountPercentage, decimal DiscountAmount,
            DateTime StartDate, DateTime EndDate, bool RequiresCouponCode, string CouponCode, bool Deleted)
        {
            if (StartDate.CompareTo(EndDate) >= 0)
                throw new NopException("Start date should be less then expiration date");

            //if ((DiscountType == DiscountTypeEnum.AssignedToWholeOrder) && !RequiresCouponCode)
            //{
            //    throw new NopException("Discounts assigned to whole order should require coupon code");
            //}

            //if ((DiscountType == DiscountTypeEnum.AssignedToWholeOrder)
            //    && RequiresCouponCode
            //    && String.IsNullOrEmpty(CouponCode))
            //{
            //    throw new NopException("Discounts assigned to whole order should require coupon code. Coupon code could not be empty.");
            //}

            if (RequiresCouponCode && String.IsNullOrEmpty(CouponCode))
            {
                throw new NopException("Discount requires coupon code. Coupon code could not be empty.");
            }

            DBDiscount dbItem = DBProviderManager<DBDiscountProvider>.Provider.InsertDiscount((int)DiscountType, (int)DiscountRequirement, Name,
                UsePercentage, DiscountPercentage, DiscountAmount,
                StartDate, EndDate, RequiresCouponCode, CouponCode, Deleted);
            Discount discount = DBMapping(dbItem);

            if (DiscountManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(DISCOUNTS_PATTERN_KEY);
            }
            return discount;
        }

        /// <summary>
        /// Updates the discount
        /// </summary>
        /// <param name="DiscountID">Discount identifier</param>
        /// <param name="DiscountType">The discount type</param>
        /// <param name="DiscountRequirement">The discount requirement</param>
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
        public static Discount UpdateDiscount(int DiscountID, DiscountTypeEnum DiscountType,
            DiscountRequirementEnum DiscountRequirement, string Name, bool UsePercentage,
            decimal DiscountPercentage, decimal DiscountAmount, DateTime StartDate,
            DateTime EndDate, bool RequiresCouponCode, string CouponCode, bool Deleted)
        {
            if (StartDate.CompareTo(EndDate) >= 0)
                throw new NopException("Start date should be less then expiration date");

            //if ((DiscountType == DiscountTypeEnum.AssignedToWholeOrder) && !RequiresCouponCode)
            //{
            //    throw new NopException("Discounts assigned to whole order should require coupon code");
            //}

            //if ((DiscountType == DiscountTypeEnum.AssignedToWholeOrder)
            //    && RequiresCouponCode
            //    && String.IsNullOrEmpty(CouponCode))
            //{
            //    throw new NopException("Discounts assigned to whole order should require coupon code. Coupon code could not be empty.");
            //}

            if (RequiresCouponCode && String.IsNullOrEmpty(CouponCode))
            {
                throw new NopException("Discount requires coupon code. Coupon code could not be empty.");
            }

            DBDiscount dbItem = DBProviderManager<DBDiscountProvider>.Provider.UpdateDiscount(DiscountID, (int)DiscountType,
                (int)DiscountRequirement, Name, UsePercentage, DiscountPercentage,
                DiscountAmount, StartDate, EndDate,
                RequiresCouponCode, CouponCode, Deleted);
            Discount discount = DBMapping(dbItem);

            if (DiscountManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(DISCOUNTS_PATTERN_KEY);
            }
            return discount;
        }

        /// <summary>
        /// Adds a discount to a product variant
        /// </summary>
        /// <param name="ProductVariantID">Product variant identifier</param>
        /// <param name="DiscountID">Discount identifier</param>
        public static void AddDiscountToProductVariant(int ProductVariantID, int DiscountID)
        {
            DBProviderManager<DBDiscountProvider>.Provider.AddDiscountToProductVariant(ProductVariantID, DiscountID);
            if (DiscountManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(DISCOUNTS_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Removes a discount from a product variant
        /// </summary>
        /// <param name="ProductVariantID">Product variant identifier</param>
        /// <param name="DiscountID">Discount identifier</param>
        public static void RemoveDiscountFromProductVariant(int ProductVariantID, int DiscountID)
        {
            DBProviderManager<DBDiscountProvider>.Provider.RemoveDiscountFromProductVariant(ProductVariantID, DiscountID);
            if (DiscountManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(DISCOUNTS_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets a discount collection of a product variant
        /// </summary>
        /// <param name="ProductVariantID">Product variant identifier</param>
        /// <returns>Discount collection</returns>
        public static DiscountCollection GetDiscountsByProductVariantID(int ProductVariantID)
        {
            bool showHidden = NopContext.Current.IsAdmin;
            string key = string.Format(DISCOUNTS_BY_PRODUCTVARIANTID_KEY, ProductVariantID, showHidden);
            object obj2 = NopCache.Get(key);
            if (DiscountManager.CacheEnabled && (obj2 != null))
            {
                return (DiscountCollection)obj2;
            }

            DBDiscountCollection dbCollection = DBProviderManager<DBDiscountProvider>.Provider.GetDiscountsByProductVariantID(ProductVariantID, showHidden);
            DiscountCollection discounts = DBMapping(dbCollection);

            if (DiscountManager.CacheEnabled)
            {
                NopCache.Max(key, discounts);
            }
            return discounts;
        }

        /// <summary>
        /// Adds a discount to a category
        /// </summary>
        /// <param name="CategoryID">Category identifier</param>
        /// <param name="DiscountID">Discount identifier</param>
        public static void AddDiscountToCategory(int CategoryID, int DiscountID)
        {
            DBProviderManager<DBDiscountProvider>.Provider.AddDiscountToCategory(CategoryID, DiscountID);
            if (DiscountManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(DISCOUNTS_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Removes a discount from a category
        /// </summary>
        /// <param name="CategoryID">Category identifier</param>
        /// <param name="DiscountID">Discount identifier</param>
        public static void RemoveDiscountFromCategory(int CategoryID, int DiscountID)
        {
            DBProviderManager<DBDiscountProvider>.Provider.RemoveDiscountFromCategory(CategoryID, DiscountID);
            if (DiscountManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(DISCOUNTS_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets a discount collection of a category
        /// </summary>
        /// <param name="CategoryID">Category identifier</param>
        /// <returns>Discount collection</returns>
        public static DiscountCollection GetDiscountsByCategoryID(int CategoryID)
        {
            bool showHidden = NopContext.Current.IsAdmin;
            string key = string.Format(DISCOUNTS_BY_CATEGORYID_KEY, CategoryID, showHidden);
            object obj2 = NopCache.Get(key);
            if (DiscountManager.CacheEnabled && (obj2 != null))
            {
                return (DiscountCollection)obj2;
            }

            DBDiscountCollection dbCollection = DBProviderManager<DBDiscountProvider>.Provider.GetDiscountsByCategoryID(CategoryID, showHidden);
            DiscountCollection discounts = DBMapping(dbCollection);

            if (DiscountManager.CacheEnabled)
            {
                NopCache.Max(key, discounts);
            }
            return discounts;
        }

        /// <summary>
        /// Gets all discount requirements
        /// </summary>
        /// <returns>Discount requirement collection</returns>
        public static DiscountRequirementCollection GetAllDiscountRequirements()
        {
            string key = string.Format(DISCOUNTREQUIREMENT_ALL_KEY);
            object obj2 = NopCache.Get(key);
            if (DiscountManager.CacheEnabled && (obj2 != null))
            {
                return (DiscountRequirementCollection)obj2;
            }

            DBDiscountRequirementCollection dbCollection = DBProviderManager<DBDiscountProvider>.Provider.GetAllDiscountRequirements();
            DiscountRequirementCollection discountRequirements = DBMapping(dbCollection);

            if (DiscountManager.CacheEnabled)
            {
                NopCache.Max(key, discountRequirements);
            }
            return discountRequirements;
        }

        /// <summary>
        /// Gets all discount types
        /// </summary>
        /// <returns>Discount type collection</returns>
        public static DiscountTypeCollection GetAllDiscountTypes()
        {
            string key = string.Format(DISCOUNTTYPES_ALL_KEY);
            object obj2 = NopCache.Get(key);
            if (DiscountManager.CacheEnabled && (obj2 != null))
            {
                return (DiscountTypeCollection)obj2;
            }

            DBDiscountTypeCollection dbCollection = DBProviderManager<DBDiscountProvider>.Provider.GetAllDiscountTypes();
            DiscountTypeCollection discountTypeCollection = DBMapping(dbCollection);

            if (DiscountManager.CacheEnabled)
            {
                NopCache.Max(key, discountTypeCollection);
            }
            return discountTypeCollection;
        }
      
        #endregion

        #region Property
        /// <summary>
        /// Gets a value indicating whether cache is enabled
        /// </summary>
        public static bool CacheEnabled
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("Cache.DiscountManager.CacheEnabled");
            }
        }
        #endregion
    }
}
