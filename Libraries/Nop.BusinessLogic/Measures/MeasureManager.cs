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
using NopSolutions.NopCommerce.DataAccess.Measures;

namespace NopSolutions.NopCommerce.BusinessLogic.Measures
{
    /// <summary>
    /// Measure dimension manager
    /// </summary>
    public partial class MeasureManager
    {
        #region Constants
        private const string MEASUREDIMENSIONS_ALL_KEY = "Nop.measuredimension.all";
        private const string MEASUREDIMENSIONS_BY_ID_KEY = "Nop.measuredimension.id-{0}";
        private const string MEASUREWEIGHTS_ALL_KEY = "Nop.measureweight.all";
        private const string MEASUREWEIGHTS_BY_ID_KEY = "Nop.measureweight.id-{0}";
        private const string MEASUREDIMENSIONS_PATTERN_KEY = "Nop.measuredimension.";
        private const string MEASUREWEIGHTS_PATTERN_KEY = "Nop.measureweight.";
        #endregion

        #region Utilities
        private static MeasureDimensionCollection DBMapping(DBMeasureDimensionCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            MeasureDimensionCollection collection = new MeasureDimensionCollection();
            foreach (DBMeasureDimension dbItem in dbCollection)
            {
                MeasureDimension item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static MeasureDimension DBMapping(DBMeasureDimension dbItem)
        {
            if (dbItem == null)
                return null;

            MeasureDimension item = new MeasureDimension();
            item.MeasureDimensionID = dbItem.MeasureDimensionID;
            item.Name = dbItem.Name;
            item.SystemKeyword = dbItem.SystemKeyword;
            item.DisplayOrder = dbItem.DisplayOrder;

            return item;
        }

        private static MeasureWeightCollection DBMapping(DBMeasureWeightCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            MeasureWeightCollection collection = new MeasureWeightCollection();
            foreach (DBMeasureWeight dbItem in dbCollection)
            {
                MeasureWeight item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static MeasureWeight DBMapping(DBMeasureWeight dbItem)
        {
            if (dbItem == null)
                return null;

            MeasureWeight item = new MeasureWeight();
            item.MeasureWeightID = dbItem.MeasureWeightID;
            item.Name = dbItem.Name;
            item.SystemKeyword = dbItem.SystemKeyword;
            item.DisplayOrder = dbItem.DisplayOrder;

            return item;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets a measure dimension by identifier
        /// </summary>
        /// <param name="MeasureDimensionID">Measure dimension identifier</param>
        /// <returns>Measure dimension</returns>
        public static MeasureDimension GetMeasureDimensionByID(int MeasureDimensionID)
        {
            if (MeasureDimensionID == 0)
                return null;

            string key = string.Format(MEASUREDIMENSIONS_BY_ID_KEY, MeasureDimensionID);
            object obj2 = NopCache.Get(key);
            if (MeasureManager.CacheEnabled && (obj2 != null))
            {
                return (MeasureDimension)obj2;
            }

            DBMeasureDimension dbItem = DBProviderManager<DBMeasureProvider>.Provider.GetMeasureDimensionByID(MeasureDimensionID);
            MeasureDimension measureDimension = DBMapping(dbItem);

            if (MeasureManager.CacheEnabled)
            {
                NopCache.Max(key, measureDimension);
            }
            return measureDimension;
        }
        
        /// <summary>
        /// Gets all measure dimensions
        /// </summary>
        /// <returns>Measure dimension collection</returns>
        public static MeasureDimensionCollection GetAllMeasureDimensions()
        {
            string key = MEASUREDIMENSIONS_ALL_KEY;
            object obj2 = NopCache.Get(key);
            if (MeasureManager.CacheEnabled && (obj2 != null))
            {
                return (MeasureDimensionCollection)obj2;
            }

            DBMeasureDimensionCollection dbCollection = DBProviderManager<DBMeasureProvider>.Provider.GetAllMeasureDimensions();
            MeasureDimensionCollection measureDimensionCollection = DBMapping(dbCollection);

            if (MeasureManager.CacheEnabled)
            {
                NopCache.Max(key, measureDimensionCollection);
            }
            return measureDimensionCollection;
        }

        /// <summary>
        /// Gets a measure weight by identifier
        /// </summary>
        /// <param name="MeasureWeightID">Measure weight identifier</param>
        /// <returns>Measure weight</returns>
        public static MeasureWeight GetMeasureWeightByID(int MeasureWeightID)
        {
            if (MeasureWeightID == 0)
                return null;

            string key = string.Format(MEASUREWEIGHTS_BY_ID_KEY, MeasureWeightID);
            object obj2 = NopCache.Get(key);
            if (MeasureManager.CacheEnabled && (obj2 != null))
            {
                return (MeasureWeight)obj2;
            }

            DBMeasureWeight dbItem = DBProviderManager<DBMeasureProvider>.Provider.GetMeasureWeightByID(MeasureWeightID);
            MeasureWeight measureWeight = DBMapping(dbItem);

            if (MeasureManager.CacheEnabled)
            {
                NopCache.Max(key, measureWeight);
            }
            return measureWeight;
        }

        /// <summary>
        /// Gets all measure weights
        /// </summary>
        /// <returns>Measure weight collection</returns>
        public static MeasureWeightCollection GetAllMeasureWeights()
        {
            string key = MEASUREWEIGHTS_ALL_KEY;
            object obj2 = NopCache.Get(key);
            if (MeasureManager.CacheEnabled && (obj2 != null))
            {
                return (MeasureWeightCollection)obj2;
            }

            DBMeasureWeightCollection dbCollection = DBProviderManager<DBMeasureProvider>.Provider.GetAllMeasureWeights();
            MeasureWeightCollection measureWeightCollection = DBMapping(dbCollection);

            if (MeasureManager.CacheEnabled)
            {
                NopCache.Max(key, measureWeightCollection);
            }
            return measureWeightCollection;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the dimension that will be used as a default
        /// </summary>
        public static MeasureDimension BaseDimensionIn
        {
            get
            {
                int baseDimensionIn = SettingManager.GetSettingValueInteger("Common.BaseDimensionIn");
                return MeasureManager.GetMeasureDimensionByID(baseDimensionIn);
            }
            set
            {
                if (value != null)
                    SettingManager.SetParam("Common.BaseDimensionIn", value.MeasureDimensionID.ToString());
            }
        }
       
        /// <summary>
        /// Gets or sets the weight that will be used as a default
        /// </summary>
        public static MeasureWeight BaseWeightIn
        {
            get
            {
                int baseWeightIn = SettingManager.GetSettingValueInteger("Common.BaseWeightIn");
                return MeasureManager.GetMeasureWeightByID(baseWeightIn);
            }
            set
            {
                if (value != null)
                    SettingManager.SetParam("Common.BaseWeightIn", value.MeasureWeightID.ToString());
            }
        }

        /// <summary>
        /// Gets a value indicating whether cache is enabled
        /// </summary>
        public static bool CacheEnabled
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("Cache.MeasureManager.CacheEnabled");
            }
        }
        #endregion
    }
}
