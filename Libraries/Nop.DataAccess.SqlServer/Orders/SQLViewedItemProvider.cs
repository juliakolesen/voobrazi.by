using System;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Collections.Specialized;
using System.Configuration.Provider;

namespace NopSolutions.NopCommerce.DataAccess.Orders
{
    class SQLViewedItemProvider:DBViewedItemProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBViewedItem GetViewedItemFromReader(IDataReader dataReader)
        {
            DBViewedItem viewedItem = new DBViewedItem();
            viewedItem.ViewedItemID = NopSqlDataHelper.GetInt(dataReader, "ViewedProductID");
            viewedItem.CustomerSessionGUID = NopSqlDataHelper.GetGuid(dataReader, "CustomerSessionGUID");
            viewedItem.ProductVariantID = NopSqlDataHelper.GetInt(dataReader, "ProductVariantID");
            viewedItem.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            return viewedItem;
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
        /// Deletes a viewed item
        /// </summary>
        /// <param name="ShoppingCartItemID">The viewed item identifier</param>
        public override void DeleteViewedItem(int viewedItemID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("ViewedItemDelete");
            db.AddInParameter(dbCommand, "ViewedProductID", DbType.Int32, viewedItemID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a viewed by customer session GUID
        /// </summary>
        /// <param name="CustomerSessionGUID">The customer session identifier</param>
        /// <param name="PageSize">pageSize</param>
        /// <param name="pageIndex">pageIndex</param>
        /// <returns>Viewed items</returns>
        public override DBViewedItemsCollection GetViewedItemByCustomerSessionGUID(Guid CustomerSessionGUID, int PageSize, int PageIndex, out int TotalRecords)
        {
            DBViewedItemsCollection viewedItems = new DBViewedItemsCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("ViewedItemLoadByCustomerSessionGUID");
            db.AddInParameter(dbCommand, "CustomerSessionGUID", DbType.Guid, CustomerSessionGUID);
            db.AddInParameter(dbCommand, "Count", DbType.Int32, PageSize);
            db.AddInParameter(dbCommand, "PageIndex", DbType.Int32, PageIndex);
            db.AddOutParameter(dbCommand, "TotalRecords", DbType.Int32, 0);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBViewedItem viewedItem = GetViewedItemFromReader(dataReader);
                    viewedItems.Add(viewedItem);
                }
            }

            TotalRecords = Convert.ToInt32(db.GetParameterValue(dbCommand, "@TotalRecords"));

            return viewedItems;
        }

        /// <summary>
        /// Inserts a viewed item
        /// </summary>
        /// <param name="CustomerSessionGUID">The customer session identifier</param>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>Viewed item</returns>
        public override DBViewedItem InsertViewedItem(Guid CustomerSessionGUID,
                                      int ProductVariantID, DateTime CreatedOn)
        {
            DBViewedItem viewedItem = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("ViewedItemInsert");
            db.AddOutParameter(dbCommand, "ViewedItemID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "CustomerSessionGUID", DbType.Guid, CustomerSessionGUID);
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, ProductVariantID);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int viewedItemID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ViewedItemID"));
                viewedItem = GetViewedItemByID(viewedItemID);
            }
            return viewedItem;
        }

        /// <summary>
        /// Gets a viewed item
        /// </summary>
        /// <param name="viewedItemID">The shopping cart item identifier</param>
        /// <returns>viewed item</returns>
        public override DBViewedItem GetViewedItemByID(int viewedItemID)
        {
            DBViewedItem viewedItem = null;
            if (viewedItemID == 0)
                return viewedItem;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("ViewedItemLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ViewedItemID", DbType.Int32, viewedItemID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    viewedItem = GetViewedItemFromReader(dataReader);
                }
            }
            return viewedItem;
        }

        /// <summary>
        /// Updates the viewed item
        /// </summary>
        /// <param name="ViewedItemID">The viewed item identifier</param>
        /// <param name="CustomerSessionGUID">The customer session identifier</param>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>Viewed item</returns>
        public override DBViewedItem UpdateViewedItem(int ViewedItemID, Guid CustomerSessionGUID,
                                                                  int ProductVariantID, DateTime CreatedOn)
        {
            DBViewedItem viewedItem = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("ViewedItemUpdate");
            db.AddInParameter(dbCommand, "ViewedItemID", DbType.Int32, ViewedItemID);
            db.AddInParameter(dbCommand, "CustomerSessionGUID", DbType.Guid, CustomerSessionGUID);
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, ProductVariantID);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                viewedItem = GetViewedItemByID(ViewedItemID);

            return viewedItem;
        }

        /// <summary>
        /// Gets viewed item by the product variant identifier
        /// </summary>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <returns></returns>
        public override DBViewedItem ViewedItemLoadByProductVariantID(int ProductVariantID, Guid CustomerSessionGUID)
        {
            DBViewedItem viewedItem = null;
            if (ProductVariantID == 0)
                return viewedItem;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("ViewedItemLoadByProductVariantID");
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, ProductVariantID);
            db.AddInParameter(dbCommand, "CustomerSessionGUID", DbType.Guid, CustomerSessionGUID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    viewedItem = GetViewedItemFromReader(dataReader);
                }
            }
            return viewedItem;
        }
        #endregion
    }
}
