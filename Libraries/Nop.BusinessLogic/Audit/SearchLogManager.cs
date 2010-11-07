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
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Audit;

namespace NopSolutions.NopCommerce.BusinessLogic.Audit
{
    /// <summary>
    /// Search log manager
    /// </summary>
    public partial class SearchLogManager
    {
        #region Utilities
        private static SearchLogCollection DBMapping(DBSearchLogCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            SearchLogCollection collection = new SearchLogCollection();
            foreach (DBSearchLog dbItem in dbCollection)
            {
                SearchLog item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static SearchLog DBMapping(DBSearchLog dbItem)
        {
            if (dbItem == null)
                return null;

            SearchLog item = new SearchLog();
            item.SearchLogID = dbItem.SearchLogID;
            item.SearchTerm = dbItem.SearchTerm;
            item.CustomerID = dbItem.CustomerID;
            item.CreatedOn = dbItem.CreatedOn;

            return item;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get search term stats
        /// </summary>
        /// <param name="StartTime">Start time; null to load all</param>
        /// <param name="EndTime">End time; null to load all</param>
        /// <param name="Count">Item count. 0 if you want to get all items</param>
        /// <returns>Result</returns>
        public static IDataReader SearchTermReport(DateTime? StartTime, DateTime? EndTime, int Count)
        {
            return DBProviderManager<DBSearchLogProvider>.Provider.SearchTermReport(StartTime, EndTime, Count);
        }

        /// <summary>
        /// Gets all search log items
        /// </summary>
        /// <returns>Search log collection</returns>
        public static SearchLogCollection GetAllSearchLogs()
        {
            DBSearchLogCollection dbCollection = DBProviderManager<DBSearchLogProvider>.Provider.GetAllSearchLogs();
            SearchLogCollection collection = DBMapping(dbCollection);
            return collection;
        }

        /// <summary>
        /// Gets a search log item
        /// </summary>
        /// <param name="SearchLogID">The search log item identifier</param>
        /// <returns>Search log item</returns>
        public static SearchLog GetSearchLogByID(int SearchLogID)
        {
            if (SearchLogID == 0)
                return null;

            DBSearchLog dbItem = DBProviderManager<DBSearchLogProvider>.Provider.GetSearchLogByID(SearchLogID);
            SearchLog item = DBMapping(dbItem);
            return item;
        }

        /// <summary>
        /// Inserts a search log item
        /// </summary>
        /// <param name="SearchTerm">The search term</param>
        /// <param name="CustomerID">The customer identifier</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>Search log item</returns>
        public static SearchLog InsertSearchLog(string SearchTerm, int CustomerID, DateTime CreatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);

            DBSearchLog dbItem = DBProviderManager<DBSearchLogProvider>.Provider.InsertSearchLog(SearchTerm, CustomerID, CreatedOn);
            SearchLog item = DBMapping(dbItem);
            return item;
        }
        #endregion
    }
}
