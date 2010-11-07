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

namespace NopSolutions.NopCommerce.DataAccess.Shipping
{
    /// <summary>
    /// Acts as a base class for deriving custom shipping method provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/ShippingMethodProvider")]
    public abstract partial class DBShippingMethodProvider : BaseDBProvider
    {
        #region Methods
        /// <summary>
        /// Deletes a shipping method
        /// </summary>
        /// <param name="ShippingMethodID">The shipping method identifier</param>
        public abstract void DeleteShippingMethod(int ShippingMethodID);

        /// <summary>
        /// Gets a shipping method
        /// </summary>
        /// <param name="ShippingMethodID">The shipping method identifier</param>
        /// <returns>Shipping method</returns>
        public abstract DBShippingMethod GetShippingMethodByID(int ShippingMethodID);

        /// <summary>
        /// Gets all shipping methods
        /// </summary>
        /// <returns>Shipping method collection</returns>
        public abstract DBShippingMethodCollection GetAllShippingMethods();

        /// <summary>
        /// Inserts a shipping method
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="Description">The description</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Shipping method</returns>
        public abstract DBShippingMethod InsertShippingMethod(string Name, string Description, int DisplayOrder);

        /// <summary>
        /// Updates the shipping method
        /// </summary>
        /// <param name="ShippingMethodID">The shipping method identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="Description">The description</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Shipping method</returns>
        public abstract DBShippingMethod UpdateShippingMethod(int ShippingMethodID, string Name, string Description,
            int DisplayOrder);
        #endregion
    }
}
