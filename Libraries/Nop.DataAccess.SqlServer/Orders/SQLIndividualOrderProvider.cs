using System;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Collections.Specialized;
using System.Configuration.Provider;

namespace NopSolutions.NopCommerce.DataAccess.Orders
{
    class SQLIndividualOrderProvider: DBIndividualOrderProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBIndividualOrder GetIndividualOrderFromReader(IDataReader dataReader)
        {
            DBIndividualOrder indivOrder = new DBIndividualOrder();
            indivOrder.IndividualOrderID = NopSqlDataHelper.GetInt(dataReader, "IndividualOrderID");
            indivOrder.CurrentUserSessionGuid = NopSqlDataHelper.GetGuid(dataReader, "CustomerSessionGUID");
            indivOrder.Price = NopSqlDataHelper.GetLong(dataReader, "Price");
            indivOrder.SerialNumberInShCart = NopSqlDataHelper.GetInt(dataReader, "SerialNumberInShCart");
            indivOrder.OrderText = NopSqlDataHelper.GetString(dataReader, "OrderText");
            return indivOrder;
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
        /// Deletes a individual order
        /// </summary>
        /// <param name="ViewedItemID">The individual order identifier</param>
        public override void DeleteIndividualOrder(int IndividualOrderID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("IndividualOrderDelete");
            db.AddInParameter(dbCommand, "IndividualOrderID", DbType.Int32, IndividualOrderID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        
        public override DBIndividualOrderCollection IndividualOrderGetByCustomerSessionGUID(Guid userGuid)
        {
            DBIndividualOrderCollection indivOrders = new DBIndividualOrderCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("IndividualOrderGetByCustomerSessionGUID");
            db.AddInParameter(dbCommand, "CustomerSessionGUID", DbType.Guid, userGuid);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBIndividualOrder indivOrder = GetIndividualOrderFromReader(dataReader);
                    indivOrders.Add(indivOrder);
                }
            }

            return indivOrders;
        }

        public override DBIndividualOrder InsertIndividualOrder(Guid userGuid,
                                                  long Price, String OrderText)
        {
            DBIndividualOrder indivOrder = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("IndividualOrderInsert");
            db.AddOutParameter(dbCommand, "IndividualOrderID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "CustomerSessionGUID", DbType.Guid, userGuid);
            db.AddInParameter(dbCommand, "Price", DbType.Int64, Price);
            db.AddInParameter(dbCommand, "OrderText", DbType.String, OrderText);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int indivOrderID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@IndividualOrderID"));
                indivOrder = GetIndividualOrderByID(indivOrderID);
            }

            return indivOrder;
        }

        public override DBIndividualOrder GetIndividualOrderByID(int individualOrderID)
        {
            DBIndividualOrder indivOrder = null;
            if (individualOrderID == 0)
                return indivOrder;

            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("IndividualOrderLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "IndividualOrderID", DbType.Int32, individualOrderID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    indivOrder = GetIndividualOrderFromReader(dataReader);
                }
            }

            return indivOrder;
        }

        #endregion Methods
    }
}
