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
    /// Payment status manager
    /// </summary>
    public partial class PaymentStatusManager
    {
        #region Constants
        private const string PAYMENTSTATUSES_ALL_KEY = "Nop.paymentstatus.all";
        private const string PAYMENTSTATUSES_BY_ID_KEY = "Nop.paymentstatus.id-{0}";
        private const string PAYMENTSTATUSES_PATTERN_KEY = "Nop.paymentstatus.";
        #endregion

        #region Utilities
        private static PaymentStatusCollection DBMapping(DBPaymentStatusCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            PaymentStatusCollection collection = new PaymentStatusCollection();
            foreach (DBPaymentStatus dbItem in dbCollection)
            {
                PaymentStatus item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static PaymentStatus DBMapping(DBPaymentStatus dbItem)
        {
            if (dbItem == null)
                return null;

            PaymentStatus item = new PaymentStatus();
            item.PaymentStatusID = dbItem.PaymentStatusID;
            item.Name = dbItem.Name;

            return item;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Gets a payment status full name
        /// </summary>
        /// <param name="PaymentStatusID">Payment status identifier</param>
        /// <returns>Payment status name</returns>
        public static string GetPaymentStatusName(int PaymentStatusID)
        {
            PaymentStatus paymentStatus = GetPaymentStatusByID(PaymentStatusID);
            if (paymentStatus != null)
                return paymentStatus.Name;
            else
                return ((PaymentStatusEnum)PaymentStatusID).ToString();
        }

        /// <summary>
        /// Gets a payment status by ID
        /// </summary>
        /// <param name="PaymentStatusID">payment status identifier</param>
        /// <returns>Payment status</returns>
        public static PaymentStatus GetPaymentStatusByID(int PaymentStatusID)
        {
            if (PaymentStatusID == 0)
                return null;

            string key = string.Format(PAYMENTSTATUSES_BY_ID_KEY, PaymentStatusID);
            object obj2 = NopCache.Get(key);
            if (PaymentStatusManager.CacheEnabled && (obj2 != null))
            {
                return (PaymentStatus)obj2;
            }

            DBPaymentStatus dbItem = DBProviderManager<DBPaymentStatusProvider>.Provider.GetPaymentStatusByID(PaymentStatusID);
            PaymentStatus paymentStatus = DBMapping(dbItem);

            if (PaymentStatusManager.CacheEnabled)
            {
                NopCache.Max(key, paymentStatus);
            }
            return paymentStatus;
        }

        /// <summary>
        /// Gets all payment statuses
        /// </summary>
        /// <returns>Payment status collection</returns>
        public static PaymentStatusCollection GetAllPaymentStatuses()
        {
            string key = string.Format(PAYMENTSTATUSES_ALL_KEY);
            object obj2 = NopCache.Get(key);
            if (PaymentStatusManager.CacheEnabled && (obj2 != null))
            {
                return (PaymentStatusCollection)obj2;
            }

            DBPaymentStatusCollection dbCollection = DBProviderManager<DBPaymentStatusProvider>.Provider.GetAllPaymentStatuses();
            PaymentStatusCollection paymentStatusCollection =  DBMapping(dbCollection);

            if (PaymentStatusManager.CacheEnabled)
            {
                NopCache.Max(key, paymentStatusCollection);
            }
            return paymentStatusCollection;
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
                return SettingManager.GetSettingValueBoolean("Cache.PaymentStatusManager.CacheEnabled");
            }
        }
        #endregion
    }
}
