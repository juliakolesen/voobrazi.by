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

namespace NopSolutions.NopCommerce.DataAccess.Directory
{
    /// <summary>
    /// Acts as a base class for deriving custom currency provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/CurrencyProvider")]
    public abstract partial class DBCurrencyProvider : BaseDBProvider
    {
        #region Methods
        /// <summary>
        /// Deletes currency
        /// </summary>
        /// <param name="CurrencyID">Currency identifier</param>
        public abstract void DeleteCurrency(int CurrencyID);

        /// <summary>
        /// Gets a currency
        /// </summary>
        /// <param name="CurrencyID">Currency identifier</param>
        /// <returns>Currency</returns>
        public abstract DBCurrency GetCurrencyByID(int CurrencyID);

        /// <summary>
        /// Gets all currencies
        /// </summary>
        /// <returns>Currency collection</returns>
        public abstract DBCurrencyCollection GetAllCurrencies(bool showHidden);

        /// <summary>
        /// Inserts a currency
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="CurrencyCode">The currency code</param>
        /// <param name="Rate">The rate</param>
        /// <param name="DisplayLocale">The display locale</param>
        /// <param name="CustomFormatting">The custom formatting</param>
        /// <param name="Published">A value indicating whether the entity is published</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>A currency</returns>
        public abstract DBCurrency InsertCurrency(string Name, string CurrencyCode, decimal Rate,
           string DisplayLocale, string CustomFormatting, bool Published, int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn);

        /// <summary>
        /// Updates the currency
        /// </summary>
        /// <param name="CurrencyID">Currency identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="CurrencyCode">The currency code</param>
        /// <param name="Rate">The rate</param>
        /// <param name="DisplayLocale">The display locale</param>
        /// <param name="CustomFormatting">The custom formatting</param>
        /// <param name="Published">A value indicating whether the entity is published</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>A currency</returns>
        public abstract DBCurrency UpdateCurrency(int CurrencyID, string Name, string CurrencyCode, decimal Rate,
           string DisplayLocale, string CustomFormatting, bool Published, int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn);
        #endregion
    }
}
