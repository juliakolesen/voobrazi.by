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

namespace NopSolutions.NopCommerce.DataAccess.Warehouses
{
    /// <summary>
    /// Acts as a base class for deriving custom warehouse provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/WarehouseProvider")]
    public abstract partial class DBWarehouseProvider : BaseDBProvider
    {
        #region Methods
        /// <summary>
        /// Gets all warehouses
        /// </summary>
        /// <returns>Warehouse collection</returns>
        public abstract DBWarehouseCollection GetAllWarehouses();

        /// <summary>
        /// Gets a warehouse
        /// </summary>
        /// <param name="WarehouseID">The warehouse identifier</param>
        /// <returns>Warehouse</returns>
        public abstract DBWarehouse GetWarehouseByID(int WarehouseID);

        /// <summary>
        /// Inserts a warehouse
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="PhoneNumber">The phone number</param>
        /// <param name="Email">The email</param>
        /// <param name="FaxNumber">The fax number</param>
        /// <param name="Address1">The address 1</param>
        /// <param name="Address2">The address 2</param>
        /// <param name="City">The city</param>
        /// <param name="StateProvince">The state/province</param>
        /// <param name="ZipPostalCode">The zip/postal code</param>
        /// <param name="CountryID">The country identifier</param>
        /// <param name="Deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Warehouse</returns>
        public abstract DBWarehouse InsertWarehouse(string Name, string PhoneNumber, string Email, string FaxNumber,
            string Address1, string Address2, string City, string StateProvince,
            string ZipPostalCode, int CountryID, bool Deleted, DateTime CreatedOn, DateTime UpdatedOn);

        /// <summary>
        /// Updates the warehouse
        /// </summary>
        /// <param name="WarehouseID">The warehouse identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="PhoneNumber">The phone number</param>
        /// <param name="Email">The email</param>
        /// <param name="FaxNumber">The fax number</param>
        /// <param name="Address1">The address 1</param>
        /// <param name="Address2">The address 2</param>
        /// <param name="City">The city</param>
        /// <param name="StateProvince">The state/province</param>
        /// <param name="ZipPostalCode">The zip/postal code</param>
        /// <param name="CountryID">The country identifier</param>
        /// <param name="Deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Warehouse</returns>
        public abstract DBWarehouse UpdateWarehouse(int WarehouseID, string Name, string PhoneNumber, string Email, string FaxNumber,
            string Address1, string Address2, string City, string StateProvince,
            string ZipPostalCode, int CountryID, bool Deleted, DateTime CreatedOn, DateTime UpdatedOn);
        #endregion
    }
}
