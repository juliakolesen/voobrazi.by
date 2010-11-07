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
    /// Acts as a base class for deriving custom language provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/LanguageProvider")]
    public abstract partial class DBLanguageProvider : BaseDBProvider
    {
        #region Methods
        /// <summary>
        /// Deletes a language
        /// </summary>
        /// <param name="LanguageID">Language identifier</param>
        public abstract void DeleteLanguage(int LanguageID);

        /// <summary>
        /// Gets all languages
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Language collection</returns>
        public abstract DBLanguageCollection GetAllLanguages(bool showHidden);

        /// <summary>
        /// Gets a language
        /// </summary>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Language</returns>
        public abstract DBLanguage GetLanguageByID(int LanguageID);

        /// <summary>
        /// Inserts a language
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="LanguageCulture">The language culture</param>
        /// <param name="Published">A value indicating whether the language is published</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Language</returns>
        public abstract DBLanguage InsertLanguage(string Name, string LanguageCulture, bool Published, int DisplayOrder);

        /// <summary>
        /// Updates a language
        /// </summary>
        /// <param name="LanguageID">Language identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="LanguageCulture">The language culture</param>
        /// <param name="Published">A value indicating whether the language is published</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Language</returns>
        public abstract DBLanguage UpdateLanguage(int LanguageID, string Name, string LanguageCulture, bool Published, int DisplayOrder);
        #endregion
    }
}
