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
using System.Configuration.Provider;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace NopSolutions.NopCommerce.DataAccess.Orders
{
    /// <summary>
    /// Shopping cart provider for SQL Server
    /// </summary>
    public partial class SQLShoppingCartProvider : DBShoppingCartProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBShoppingCartItem GetShoppingCartItemFromReader(IDataReader dataReader)
        {
            DBShoppingCartItem shoppingCartItem = new DBShoppingCartItem();
            shoppingCartItem.ShoppingCartItemID = NopSqlDataHelper.GetInt(dataReader, "ShoppingCartItemID");
            shoppingCartItem.ShoppingCartTypeID = NopSqlDataHelper.GetInt(dataReader, "ShoppingCartTypeID");
            shoppingCartItem.CustomerSessionGUID = NopSqlDataHelper.GetGuid(dataReader, "CustomerSessionGUID");
            shoppingCartItem.ProductVariantID = NopSqlDataHelper.GetInt(dataReader, "ProductVariantID");
            shoppingCartItem.AttributesXML = NopSqlDataHelper.GetString(dataReader, "AttributesXML");
            shoppingCartItem.Quantity = NopSqlDataHelper.GetInt(dataReader, "Quantity");
            shoppingCartItem.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            shoppingCartItem.UpdatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "UpdatedOn");
            return shoppingCartItem;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Initializes the provider with the property values specified in the application's configuration file. This method is not intended to be used directly from your code
        /// </summary>
        /// <param name="name">The name of the provider instance to initialize</param>
        /// <param name="config">A NameValueCollection that contains the names and values of configuration options for the provider.</param>
        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            base.Initialize(name, config);

            string connectionStringName = config["connectionStringName"];
            if (String.IsNullOrEmpty(connectionStringName))
                throw new ProviderException("Connection name not specified");
            this._sqlConnectionString = NopSqlDataHelper.GetConnectionString(connectionStringName);
            if ((this._sqlConnectionString == null) || (this._sqlConnectionString.Length < 1))
            {
                throw new ProviderException(string.Format("Connection string not found. {0}", connectionStringName));
            }
            config.Remove("connectionStringName");

            if (config.Count > 0)
            {
                string key = config.GetKey(0);
                if (!string.IsNullOrEmpty(key))
                {
                    throw new ProviderException(string.Format("Provider unrecognized attribute. {0}", new object[] { key }));
                }
            }
        }

        /// <summary>
        /// Deletes a shopping cart item
        /// </summary>
        /// <param name="ShoppingCartItemID">The shopping cart item identifier</param>
        public override void DeleteShoppingCartItem(int ShoppingCartItemID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShoppingCartItemDelete");
            db.AddInParameter(dbCommand, "ShoppingCartItemID", DbType.Int32, ShoppingCartItemID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a shopping cart by customer session GUID
        /// </summary>
        /// <param name="ShoppingCartTypeID">Shopping cart type identifier</param>
        /// <param name="CustomerSessionGUID">The customer session identifier</param>
        /// <returns>Cart</returns>
        public override DBShoppingCart GetShoppingCartByCustomerSessionGUID(int ShoppingCartTypeID, Guid CustomerSessionGUID)
        {
            DBShoppingCart Cart = new DBShoppingCart();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShoppingCartItemLoadByCustomerSessionGUID");
            db.AddInParameter(dbCommand, "ShoppingCartTypeID", DbType.Int32, ShoppingCartTypeID);
            db.AddInParameter(dbCommand, "CustomerSessionGUID", DbType.Guid, CustomerSessionGUID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBShoppingCartItem shoppingCartItem = GetShoppingCartItemFromReader(dataReader);
                    Cart.Add(shoppingCartItem);
                }
            }

            return Cart;
        }

        /// <summary>
        /// Gets a shopping cart item
        /// </summary>
        /// <param name="ShoppingCartItemID">The shopping cart item identifier</param>
        /// <returns>Shopping cart item</returns>
        public override DBShoppingCartItem GetShoppingCartItemByID(int ShoppingCartItemID)
        {
            DBShoppingCartItem shoppingCartItem = null;
            if (ShoppingCartItemID == 0)
                return shoppingCartItem;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShoppingCartItemLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ShoppingCartItemID", DbType.Int32, ShoppingCartItemID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    shoppingCartItem = GetShoppingCartItemFromReader(dataReader);
                }
            }
            return shoppingCartItem;
        }

        /// <summary>
        /// Inserts a shopping cart item
        /// </summary>
        /// <param name="ShoppingCartTypeID">The shopping cart type identifier</param>
        /// <param name="CustomerSessionGUID">The customer session identifier</param>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <param name="AttributesXML">The product variant attributes</param>
        /// <param name="Quantity">The quantity</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Shopping cart item</returns>
        public override DBShoppingCartItem InsertShoppingCartItem(int ShoppingCartTypeID, Guid CustomerSessionGUID,
          int ProductVariantID, string AttributesXML, int Quantity,
           DateTime CreatedOn, DateTime UpdatedOn)
        {
            DBShoppingCartItem shoppingCartItem = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShoppingCartItemInsert");
            db.AddOutParameter(dbCommand, "ShoppingCartItemID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "ShoppingCartTypeID", DbType.Int32, ShoppingCartTypeID);
            db.AddInParameter(dbCommand, "CustomerSessionGUID", DbType.Guid, CustomerSessionGUID);
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, ProductVariantID);
            db.AddInParameter(dbCommand, "AttributesXML", DbType.Xml, AttributesXML);
            db.AddInParameter(dbCommand, "Quantity", DbType.Int32, Quantity);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, UpdatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int ShoppingCartItemID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ShoppingCartItemID"));
                shoppingCartItem = GetShoppingCartItemByID(ShoppingCartItemID);
            }
            return shoppingCartItem;
        }

        /// <summary>
        /// Updates the shopping cart item
        /// </summary>
        /// <param name="ShoppingCartItemID">The shopping cart item identifier</param>
        /// <param name="ShoppingCartTypeID">The shopping cart type identifier</param>
        /// <param name="CustomerSessionGUID">The customer session identifier</param>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <param name="AttributesXML">The product variant attributes</param>
        /// <param name="Quantity">The quantity</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Shopping cart item</returns>
        public override DBShoppingCartItem UpdateShoppingCartItem(int ShoppingCartItemID, int ShoppingCartTypeID, Guid CustomerSessionGUID,
          int ProductVariantID, string AttributesXML, int Quantity,
           DateTime CreatedOn, DateTime UpdatedOn)
        {
            DBShoppingCartItem shoppingCartItem = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShoppingCartItemUpdate");
            db.AddInParameter(dbCommand, "ShoppingCartItemID", DbType.Int32, ShoppingCartItemID);
            db.AddInParameter(dbCommand, "ShoppingCartTypeID", DbType.Int32, ShoppingCartTypeID);
            db.AddInParameter(dbCommand, "CustomerSessionGUID", DbType.Guid, CustomerSessionGUID);
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, ProductVariantID);
            db.AddInParameter(dbCommand, "AttributesXML", DbType.Xml, AttributesXML);
            db.AddInParameter(dbCommand, "Quantity", DbType.Int32, Quantity);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, UpdatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                shoppingCartItem = GetShoppingCartItemByID(ShoppingCartItemID);


            return shoppingCartItem;
        }
        #endregion
    }
}

