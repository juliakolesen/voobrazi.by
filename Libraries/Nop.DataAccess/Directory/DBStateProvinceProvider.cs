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
    /// Acts as a base class for deriving custom state/province provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/StateProvinceProvider")]
    public abstract partial class DBStateProvinceProvider : BaseDBProvider
    {
        #region Methods
        /// <summary>
        /// Deletes a state/province
        /// </summary>
        /// <param name="StateProvinceID">The state/province identifier</param>
        public abstract void DeleteStateProvince(int StateProvinceID);

        /// <summary>
        /// Gets a state/province
        /// </summary>
        /// <param name="StateProvinceID">The state/province identifier</param>
        /// <returns>State/province</returns>
        public abstract DBStateProvince GetStateProvinceByID(int StateProvinceID);

        /// <summary>
        /// Gets a state/province 
        /// </summary>
        /// <param name="Abbreviation">The state/province abbreviation</param>
        /// <returns>State/province</returns>
        public abstract DBStateProvince GetStateProvinceByAbbreviation(string Abbreviation);

        /// <summary>
        /// Gets a state/province collection by country identifier
        /// </summary>
        /// <param name="CountryID">Country identifier</param>
        /// <returns>State/province collection</returns>
        public abstract DBStateProvinceCollection GetStateProvincesByCountryID(int CountryID);

        /// <summary>
        /// Inserts a state/province
        /// </summary>
        /// <param name="CountryID">The country identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="Abbreviation">The abbreviation</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>State/province</returns>
        public abstract DBStateProvince InsertStateProvince(int CountryID, string Name, string Abbreviation, int DisplayOrder);

        /// <summary>
        /// Updates a state/province
        /// </summary>
        /// <param name="StateProvinceID">The state/province identifier</param>
        /// <param name="CountryID">The country identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="Abbreviation">The abbreviation</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>State/province</returns>
        public abstract DBStateProvince UpdateStateProvince(int StateProvinceID, int CountryID, string Name, string Abbreviation, int DisplayOrder);
        #endregion
    }
}
