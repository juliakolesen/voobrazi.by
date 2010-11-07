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

namespace NopSolutions.NopCommerce.DataAccess.Measures
{
    /// <summary>
    /// Acts as a base class for deriving custom measure provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/MeasureProvider")]
    public abstract partial class DBMeasureProvider : BaseDBProvider
    {
        #region Methods
        /// <summary>
        /// Gets a measure dimension by identifier
        /// </summary>
        /// <param name="MeasureDimensionID">Measure dimension identifier</param>
        /// <returns>Measure dimension</returns>
        public abstract DBMeasureDimension GetMeasureDimensionByID(int MeasureDimensionID);

        /// <summary>
        /// Gets all measure dimensions
        /// </summary>
        /// <returns>Measure dimension collection</returns>
        public abstract DBMeasureDimensionCollection GetAllMeasureDimensions();

        /// <summary>
        /// Gets a measure weight by identifier
        /// </summary>
        /// <param name="MeasureWeightID">Measure weight identifier</param>
        /// <returns>Measure weight</returns>
        public abstract DBMeasureWeight GetMeasureWeightByID(int MeasureWeightID);
        
        /// <summary>
        /// Gets all measure weights
        /// </summary>
        /// <returns>Measure weight collection</returns>
        public abstract DBMeasureWeightCollection GetAllMeasureWeights();
        #endregion
    }
}
