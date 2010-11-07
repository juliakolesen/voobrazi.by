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
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Payment;

namespace NopSolutions.NopCommerce.BusinessLogic.Payment
{
    /// <summary>
    /// Credit card type manager
    /// </summary>
    public partial class CreditCardTypeManager
    {
        #region Constants
        private const string CREDITCARDS_ALL_KEY = "Nop.creditcard.all";
        private const string CREDITCARDS_BY_ID_KEY = "Nop.creditcard.id-{0}";
        private const string CREDITCARDS_PATTERN_KEY = "Nop.creditcard.";
        #endregion

        #region Utilities
        private static CreditCardTypeCollection DBMapping(DBCreditCardTypeCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            CreditCardTypeCollection collection = new CreditCardTypeCollection();
            foreach (DBCreditCardType dbItem in dbCollection)
            {
                CreditCardType item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static CreditCardType DBMapping(DBCreditCardType dbItem)
        {
            if (dbItem == null)
                return null;

            CreditCardType item = new CreditCardType();
            item.CreditCardTypeID = dbItem.CreditCardTypeID;
            item.Name = dbItem.Name;
            item.SystemKeyword = dbItem.SystemKeyword;
            item.DisplayOrder = dbItem.DisplayOrder;
            item.Deleted = dbItem.Deleted;

            return item;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets a credit card type
        /// </summary>
        /// <param name="CreditCardTypeID">Credit card type identifier</param>
        /// <returns>Credit card type</returns>
        public static CreditCardType GetCreditCardTypeByID(int CreditCardTypeID)
        {
            if (CreditCardTypeID == 0)
                return null;

            string key = string.Format(CREDITCARDS_BY_ID_KEY, CreditCardTypeID);
            object obj2 = NopCache.Get(key);
            if (CreditCardTypeManager.CacheEnabled && (obj2 != null))
            {
                return (CreditCardType)obj2;
            }

            DBCreditCardType dbItem = DBProviderManager<DBCreditCardTypeProvider>.Provider.GetCreditCardTypeByID(CreditCardTypeID);
            CreditCardType creditCardType = DBMapping(dbItem);

            if (CreditCardTypeManager.CacheEnabled)
            {
                NopCache.Max(key, creditCardType);
            }
            return creditCardType;
        }

        /// <summary>
        /// Marks a credit card type as deleted
        /// </summary>
        /// <param name="CreditCardTypeID">Credit card type identifier</param>
        public static void MarkCreditCardTypeAsDeleted(int CreditCardTypeID)
        {
            CreditCardType creditCardType = GetCreditCardTypeByID(CreditCardTypeID);
            if (creditCardType != null)
            {
                UpdateCreditCardType(creditCardType.CreditCardTypeID, creditCardType.Name, creditCardType.SystemKeyword, creditCardType.DisplayOrder, true);
            }
            if (CreditCardTypeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(CREDITCARDS_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets all credit card types
        /// </summary>
        /// <returns>Credit card type collection</returns>
        public static CreditCardTypeCollection GetAllCreditCardTypes()
        {
            string key = string.Format(CREDITCARDS_ALL_KEY);
            object obj2 = NopCache.Get(key);
            if (CreditCardTypeManager.CacheEnabled && (obj2 != null))
            {
                return (CreditCardTypeCollection)obj2;
            }

            DBCreditCardTypeCollection dbCollection = DBProviderManager<DBCreditCardTypeProvider>.Provider.GetAllCreditCardTypes();
            CreditCardTypeCollection creditCardTypeCollection = DBMapping(dbCollection);

            if (CreditCardTypeManager.CacheEnabled)
            {
                NopCache.Max(key, creditCardTypeCollection);
            }
            return creditCardTypeCollection;
        }

        /// <summary>
        /// Inserts a credit card type
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="SystemKeyword">The system keyword</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="Deleted">A value indicating whether the entity has been deleted</param>
        /// <returns>A credit card type</returns>
        public static CreditCardType InsertCreditCardType(string Name, string SystemKeyword, int DisplayOrder, bool Deleted)
        {
            DBCreditCardType dbItem = DBProviderManager<DBCreditCardTypeProvider>.Provider.InsertCreditCardType(Name, SystemKeyword, DisplayOrder, Deleted);
            CreditCardType creditCardType = DBMapping(dbItem);
            if (CreditCardTypeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(CREDITCARDS_PATTERN_KEY);
            }
            return creditCardType;
        }

        /// <summary>
        /// Updates the credit card type
        /// </summary>
        /// <param name="CreditCardTypeID">Credit card type identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="SystemKeyword">The system keyword</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="Deleted">A value indicating whether the entity has been deleted</param>
        /// <returns>A credit card type</returns>
        public static CreditCardType UpdateCreditCardType(int CreditCardTypeID, string Name, string SystemKeyword,
            int DisplayOrder, bool Deleted)
        {
            DBCreditCardType dbItem = DBProviderManager<DBCreditCardTypeProvider>.Provider.UpdateCreditCardType(CreditCardTypeID, Name,
                SystemKeyword, DisplayOrder, Deleted);
            CreditCardType creditCardType = DBMapping(dbItem);
            if (CreditCardTypeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(CREDITCARDS_PATTERN_KEY);
            }
            return creditCardType;
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
                return SettingManager.GetSettingValueBoolean("Cache.CreditCardTypeManager.CacheEnabled");
            }
        }
        #endregion
    }
}
