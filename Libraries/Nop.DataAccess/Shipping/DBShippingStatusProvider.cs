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
using System.Configuration.Provider;
using System.Configuration;
using System.Web.Hosting;
using System.Web.Configuration;
using System.Collections.Specialized;

namespace NopSolutions.NopCommerce.DataAccess.Shipping
{
    /// <summary>
    /// Acts as a base class for deriving custom shipping status provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/ShippingStatusProvider")]
    public abstract partial class DBShippingStatusProvider : BaseDBProvider
    {
        #region Methods

        /// <summary>
        /// Gets a shipping status by ID
        /// </summary>
        /// <param name="ShippingStatusID">Shipping status identifier</param>
        /// <returns>Shipping status</returns>
        public abstract DBShippingStatus GetShippingStatusByID(int ShippingStatusID);

        /// <summary>
        /// Gets all shipping statuses
        /// </summary>
        /// <returns>Shipping status collection</returns>
        public abstract DBShippingStatusCollection GetAllShippingStatuses();

        #endregion
    }
}
