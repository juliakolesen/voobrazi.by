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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Configuration;
using System.Configuration.Provider;
using System.Web.Hosting;
using System.Web.Configuration;
using System.Collections.Specialized;

namespace NopSolutions.NopCommerce.DataAccess.Configuration.Settings
{
    /// <summary>
    /// Acts as a base class for deriving custom setting provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/SettingProvider")]
    public abstract partial class DBSettingProvider : BaseDBProvider
    {
        #region Methods
        /// <summary>
        /// Gets a setting
        /// </summary>
        /// <param name="SettingID">Setting identifer</param>
        /// <returns>Setting</returns>
        public abstract DBSetting GetSettingByID(int SettingID);

        /// <summary>
        /// Deletes a setting
        /// </summary>
        /// <param name="SettingID">Setting identifer</param>
        public abstract void DeleteSetting(int SettingID);

        /// <summary>
        /// Gets all settings
        /// </summary>
        /// <returns>Setting collection</returns>
        public abstract DBSettingCollection GetAllSettings();

        /// <summary>
        /// Adds a setting
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="Value">The value</param>
        /// <param name="Description">The description</param>
        /// <returns>Setting</returns>
        public abstract DBSetting AddSetting(string Name, string Value, string Description);

        /// <summary>
        /// Updates a setting
        /// </summary>
        /// <param name="SettingID">Setting identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="Value">The value</param>
        /// <param name="Description">The description</param>
        /// <returns>Setting</returns>
        public abstract DBSetting UpdateSetting(int SettingID, string Name, string Value, string Description);
        #endregion
    }
}
