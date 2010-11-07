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
using NopSolutions.NopCommerce.DataAccess.Directory;

namespace NopSolutions.NopCommerce.BusinessLogic.Directory
{
    /// <summary>
    /// State province manager
    /// </summary>
    public partial class StateProvinceManager
    {
        #region Constants
        private const string STATEPROVINCES_ALL_KEY = "Nop.stateprovince.all-{0}";
        private const string STATEPROVINCES_BY_ID_KEY = "Nop.stateprovince.id-{0}";
        private const string STATEPROVINCES_PATTERN_KEY = "Nop.stateprovince.";
        #endregion

        #region Utilities
        private static StateProvinceCollection DBMapping(DBStateProvinceCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            StateProvinceCollection collection = new StateProvinceCollection();
            foreach (DBStateProvince dbItem in dbCollection)
            {
                StateProvince item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static StateProvince DBMapping(DBStateProvince dbItem)
        {
            if (dbItem == null)
                return null;

            StateProvince item = new StateProvince();
            item.StateProvinceID = dbItem.StateProvinceID;
            item.CountryID = dbItem.CountryID;
            item.Name = dbItem.Name;
            item.Abbreviation = dbItem.Abbreviation;
            item.DisplayOrder = dbItem.DisplayOrder;

            return item;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Deletes a state/province
        /// </summary>
        /// <param name="StateProvinceID">The state/province identifier</param>
        public static void DeleteStateProvince(int StateProvinceID)
        {
            DBProviderManager<DBStateProvinceProvider>.Provider.DeleteStateProvince(StateProvinceID);
            if (StateProvinceManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(STATEPROVINCES_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets a state/province
        /// </summary>
        /// <param name="StateProvinceID">The state/province identifier</param>
        /// <returns>State/province</returns>
        public static StateProvince GetStateProvinceByID(int StateProvinceID)
        {
            if (StateProvinceID == 0)
                return null;

            string key = string.Format(STATEPROVINCES_BY_ID_KEY, StateProvinceID);
            object obj2 = NopCache.Get(key);
            if (StateProvinceManager.CacheEnabled && (obj2 != null))
            {
                return (StateProvince)obj2;
            }

            DBStateProvince dbItem = DBProviderManager<DBStateProvinceProvider>.Provider.GetStateProvinceByID(StateProvinceID);
            StateProvince stateProvince = DBMapping(dbItem);

            if (StateProvinceManager.CacheEnabled)
            {
                NopCache.Max(key, stateProvince);
            }
            return stateProvince;
        }

        /// <summary>
        /// Gets a state/province 
        /// </summary>
        /// <param name="Abbreviation">The state/province abbreviation</param>
        /// <returns>State/province</returns>
        public static StateProvince GetStateProvinceByAbbreviation(string Abbreviation)
        {
            DBStateProvince dbItem = DBProviderManager<DBStateProvinceProvider>.Provider.GetStateProvinceByAbbreviation(Abbreviation);
            StateProvince stateProvince = DBMapping(dbItem);
            return stateProvince;
        }
        
        /// <summary>
        /// Gets a state/province collection by country identifier
        /// </summary>
        /// <param name="CountryID">Country identifier</param>
        /// <returns>State/province collection</returns>
        public static StateProvinceCollection GetStateProvincesByCountryID(int CountryID)
        {
            string key = string.Format(STATEPROVINCES_ALL_KEY, CountryID);
            object obj2 = NopCache.Get(key);
            if (StateProvinceManager.CacheEnabled && (obj2 != null))
            {
                return (StateProvinceCollection)obj2;
            }

            DBStateProvinceCollection dbCollection = DBProviderManager<DBStateProvinceProvider>.Provider.GetStateProvincesByCountryID(CountryID);
            StateProvinceCollection stateProvinceCollection = DBMapping(dbCollection);

            if (StateProvinceManager.CacheEnabled)
            {
                NopCache.Max(key, stateProvinceCollection);
            }
            return stateProvinceCollection;
        }

        /// <summary>
        /// Inserts a state/province
        /// </summary>
        /// <param name="CountryID">The country identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="Abbreviation">The abbreviation</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>State/province</returns>
        public static StateProvince InsertStateProvince(int CountryID, string Name, string Abbreviation, int DisplayOrder)
        {
            DBStateProvince dbItem = DBProviderManager<DBStateProvinceProvider>.Provider.InsertStateProvince(CountryID, Name, Abbreviation, DisplayOrder);
            StateProvince stateProvince = DBMapping(dbItem);

            if (StateProvinceManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(STATEPROVINCES_PATTERN_KEY);
            }

            return stateProvince;
        }

        /// <summary>
        /// Updates a state/province
        /// </summary>
        /// <param name="StateProvinceID">The state/province identifier</param>
        /// <param name="CountryID">The country identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="Abbreviation">The abbreviation</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>State/province</returns>
        public static StateProvince UpdateStateProvince(int StateProvinceID, int CountryID, string Name, string Abbreviation, int DisplayOrder)
        {
            DBStateProvince dbItem = DBProviderManager<DBStateProvinceProvider>.Provider.UpdateStateProvince(StateProvinceID, CountryID, Name, Abbreviation, DisplayOrder);
            StateProvince stateProvince = DBMapping(dbItem);

            if (StateProvinceManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(STATEPROVINCES_PATTERN_KEY);
            }

            return stateProvince;
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
                return SettingManager.GetSettingValueBoolean("Cache.StateProvinceManager.CacheEnabled");
            }
        }
        #endregion
    }
}
