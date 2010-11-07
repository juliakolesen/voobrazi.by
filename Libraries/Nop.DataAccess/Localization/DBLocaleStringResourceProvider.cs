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

namespace NopSolutions.NopCommerce.DataAccess.Localization
{
    /// <summary>
    /// Acts as a base class for deriving custom locale string resource provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/LocaleStringResourceProvider")]
    public abstract partial class DBLocaleStringResourceProvider : BaseDBProvider
    {
        #region Methods
        /// <summary>
        /// Deletes a locale string resource
        /// </summary>
        /// <param name="LocaleStringResourceID">Locale string resource identifier</param>
        public abstract void DeleteLocaleStringResource(int LocaleStringResourceID);

        /// <summary>
        /// Gets a locale string resource
        /// </summary>
        /// <param name="LocaleStringResourceID">Locale string resource identifier</param>
        /// <returns>Locale string resource</returns>
        public abstract DBLocaleStringResource GetLocaleStringResourceByID(int LocaleStringResourceID);

        /// <summary>
        /// Gets all locale string resources by language identifier
        /// </summary>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Locale string resource collection</returns>
        public abstract DBLocaleStringResourceCollection GetAllResourcesByLanguageID(int LanguageID);

        /// <summary>
        /// Inserts a locale string resource
        /// </summary>
        /// <param name="LanguageID">The language identifier</param>
        /// <param name="ResourceName">The resource name</param>
        /// <param name="ResourceValue">The resource value</param>
        /// <returns>Locale string resource</returns>
        public abstract DBLocaleStringResource InsertLocaleStringResource(int LanguageID, string ResourceName, string ResourceValue);

        /// <summary>
        /// Updates the locale string resource
        /// </summary>
        /// <param name="LocaleStringResourceID">The locale string resource identifier</param>
        /// <param name="LanguageID">The language identifier</param>
        /// <param name="ResourceName">The resource name</param>
        /// <param name="ResourceValue">The resource value</param>
        /// <returns>Locale string resource</returns>
        public abstract DBLocaleStringResource UpdateLocaleStringResource(int LocaleStringResourceID, int LanguageID, string ResourceName, string ResourceValue);
        #endregion
    }
}
