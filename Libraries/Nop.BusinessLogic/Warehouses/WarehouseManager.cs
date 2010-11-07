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
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Warehouses;

namespace NopSolutions.NopCommerce.BusinessLogic.Warehouses
{
    /// <summary>
    /// Warehouse manager
    /// </summary>
    public partial class WarehouseManager
    {
        #region Utilities
        private static WarehouseCollection DBMapping(DBWarehouseCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            WarehouseCollection collection = new WarehouseCollection();
            foreach (DBWarehouse dbItem in dbCollection)
            {
                Warehouse item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static Warehouse DBMapping(DBWarehouse dbItem)
        {
            if (dbItem == null)
                return null;

            Warehouse item = new Warehouse();
            item.WarehouseID = dbItem.WarehouseID;
            item.Name = dbItem.Name;
            item.PhoneNumber = dbItem.PhoneNumber;
            item.Email = dbItem.Email;
            item.FaxNumber = dbItem.FaxNumber;
            item.Address1 = dbItem.Address1;
            item.Address2 = dbItem.Address2;
            item.City = dbItem.City;
            item.StateProvince = dbItem.StateProvince;
            item.ZipPostalCode = dbItem.ZipPostalCode;
            item.CountryID = dbItem.CountryID;
            item.Deleted = dbItem.Deleted;
            item.CreatedOn = dbItem.CreatedOn;
            item.UpdatedOn = dbItem.UpdatedOn;

            return item;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Marks a warehouse as deleted
        /// </summary>
        /// <param name="WarehouseID">The warehouse identifier</param>
        public static void MarkWarehouseAsDeleted(int WarehouseID)
        {
            Warehouse warehouse = GetWarehouseByID(WarehouseID);
            if (warehouse != null)
            {
                UpdateWarehouse(warehouse.WarehouseID, warehouse.Name, warehouse.PhoneNumber,
                    warehouse.Email, warehouse.FaxNumber, warehouse.Address1, warehouse.Address2, warehouse.City,
                    warehouse.StateProvince, warehouse.ZipPostalCode, warehouse.CountryID, true, warehouse.CreatedOn, warehouse.UpdatedOn);
            }
        }

        /// <summary>
        /// Gets all warehouses
        /// </summary>
        /// <returns>Warehouse collection</returns>
        public static WarehouseCollection GetAllWarehouses()
        {
            DBWarehouseCollection dbCollection = DBProviderManager<DBWarehouseProvider>.Provider.GetAllWarehouses();
            WarehouseCollection warehouses = DBMapping(dbCollection);
            return warehouses;
        }

        /// <summary>
        /// Gets a warehouse
        /// </summary>
        /// <param name="WarehouseID">The warehouse identifier</param>
        /// <returns>Warehouse</returns>
        public static Warehouse GetWarehouseByID(int WarehouseID)
        {
            if (WarehouseID == 0)
                return null;

            DBWarehouse dbItem = DBProviderManager<DBWarehouseProvider>.Provider.GetWarehouseByID(WarehouseID);
            Warehouse warehouse = DBMapping(dbItem);
            return warehouse;
        }

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
        public static Warehouse InsertWarehouse(string Name, string PhoneNumber, string Email, string FaxNumber,
            string Address1, string Address2, string City, string StateProvince,
            string ZipPostalCode, int CountryID, bool Deleted, DateTime CreatedOn, DateTime UpdatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            UpdatedOn = DateTimeHelper.ConvertToUtcTime(UpdatedOn);

            DBWarehouse dbItem = DBProviderManager<DBWarehouseProvider>.Provider.InsertWarehouse(Name, PhoneNumber, Email, FaxNumber,
             Address1, Address2, City, StateProvince,
             ZipPostalCode, CountryID, Deleted, CreatedOn, UpdatedOn);
            Warehouse warehouse = DBMapping(dbItem);
            return warehouse;
        }

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
        public static Warehouse UpdateWarehouse(int WarehouseID, string Name, string PhoneNumber, string Email, string FaxNumber,
            string Address1, string Address2, string City, string StateProvince,
            string ZipPostalCode, int CountryID, bool Deleted, DateTime CreatedOn, DateTime UpdatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            UpdatedOn = DateTimeHelper.ConvertToUtcTime(UpdatedOn);

            DBWarehouse dbItem = DBProviderManager<DBWarehouseProvider>.Provider.UpdateWarehouse(WarehouseID, Name, PhoneNumber, Email, FaxNumber,
             Address1, Address2, City, StateProvince,
             ZipPostalCode, CountryID, Deleted, CreatedOn, UpdatedOn);
            Warehouse warehouse = DBMapping(dbItem);
            return warehouse;
        }
        #endregion
    }
}
