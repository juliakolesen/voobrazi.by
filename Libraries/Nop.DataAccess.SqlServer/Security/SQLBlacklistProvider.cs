using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NopSolutions.NopCommerce.DataAccess.Security;
using Microsoft.Practices.EnterpriseLibrary.Data;
using NopSolutions.NopCommerce.DataAccess;
using System.Data.Common;
using System.Data;
using System.Configuration.Provider;
using System.Collections.Specialized;

namespace NopSolutions.NopCommerce.DataAccess.Security
{
    /// <summary>
    /// Blacklist provider for SQL Server
    /// </summary>
    public partial class SQLBlacklistProvider : DBBlacklistProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        /// <summary>
        /// Gets an IP address from a data reader
        /// </summary>
        /// <param name="dataReader">IDataReader</param>
        /// <returns>DBBannedIpAddress</returns>
        private DBBannedIpAddress GetIpAddressFromReader(IDataReader dataReader)
        {
            DBBannedIpAddress ipAddress = new DBBannedIpAddress();
            ipAddress.BannedIpAddressID = NopSqlDataHelper.GetInt(dataReader, "BannedIpAddressID");
            ipAddress.Address = NopSqlDataHelper.GetString(dataReader, "Address");
            ipAddress.Comment = NopSqlDataHelper.GetString(dataReader, "Comment");
            ipAddress.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            ipAddress.UpdatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "UpdatedOn");
            return ipAddress;
        }

        /// <summary>
        /// Gets an IP network from a data reader
        /// </summary>
        /// <param name="dataReader">IDataReader</param>
        /// <returns>DBBannedIpNetwork</returns>
        private DBBannedIpNetwork GetIpNetworkFromReader(IDataReader dataReader)
        {
            DBBannedIpNetwork ipNetwork = new DBBannedIpNetwork();
            ipNetwork.BannedIpNetworkID = NopSqlDataHelper.GetInt(dataReader, "BannedIpNetworkID");
            ipNetwork.StartAddress = NopSqlDataHelper.GetString(dataReader, "StartAddress");
            ipNetwork.EndAddress = NopSqlDataHelper.GetString(dataReader, "EndAddress");
            ipNetwork.Comment = NopSqlDataHelper.GetString(dataReader, "Comment");
            ipNetwork.IpException = NopSqlDataHelper.GetString(dataReader, "IpException");
            ipNetwork.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            ipNetwork.UpdatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "UpdatedOn");
            return ipNetwork;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initializes the provider with the property values specified in the application's configuration file. 
        /// This method is not intended to be used directly from your code
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
        /// Gets an IP address by its ID
        /// </summary>
        /// <param name="bannedIpAddressID">IP address unique identifier</param>
        /// <returns>IP address</returns>
        public override DBBannedIpAddress GetIpAddressByID(int bannedIpAddressID)
        {
            DBBannedIpAddress ipAddress = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BannedIpAddressLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "BannedIpAddressID", DbType.Int32, bannedIpAddressID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    ipAddress = GetIpAddressFromReader(dataReader);
                }
            }
            return ipAddress;
        }

        /// <summary>
        /// Gets all IP addresses
        /// </summary>
        /// <returns>IP address collection</returns>
        public override DBBannedIpAddressCollection GetIpAddressAll()
        {
            DBBannedIpAddressCollection ipCollection = new DBBannedIpAddressCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BannedIpAddressLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBBannedIpAddress setting = GetIpAddressFromReader(dataReader);
                    ipCollection.Add(setting);
                }
            }
            return ipCollection;
        }

        /// <summary>
        /// Inserts an IP address
        /// </summary>
        /// <param name="address">IP address</param>
        /// <param name="comment">Reason why the IP was banned</param>
        /// <param name="createdOn">When the record was inserted</param>
        /// <param name="updatedOn">When the record was last updated</param>
        /// <returns>Banned IP address</returns>
        public override DBBannedIpAddress InsertBannedIpAddress(string address, string comment, DateTime createdOn, DateTime updatedOn)
        {
            DBBannedIpAddress ipAddress = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BannedIpAddressInsert");
            db.AddOutParameter(dbCommand, "BannedIpAddressID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Address", DbType.String, address);
            db.AddInParameter(dbCommand, "Comment", DbType.String, comment);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int ipAddressID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@BannedIpAddressID"));
                ipAddress = GetIpAddressByID(ipAddressID);
            }
            return ipAddress;
        }

        /// <summary>
        /// Updates an IP address
        /// </summary>
        /// <param name="bannedIpAddressID">IP address unique identifier</param>
        /// <param name="address">IP address</param>
        /// <param name="comment">Reason why the IP was banned</param>
        /// <param name="createdOn">When the record was last updated</param>
        /// <param name="updatedOn">When the record was last updated</param>
        /// <returns>Banned IP address</returns>
        public override DBBannedIpAddress UpdateBannedIpAddress(int bannedIpAddressID, string address, string comment, DateTime createdOn, DateTime updatedOn)
        {
            DBBannedIpAddress ipAddress = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BannedIpAddressUpdate");
            db.AddInParameter(dbCommand, "BannedIpAddressID", DbType.Int32, bannedIpAddressID);
            db.AddInParameter(dbCommand, "Address", DbType.String, address);
            db.AddInParameter(dbCommand, "Comment", DbType.String, comment);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                ipAddress = GetIpAddressByID(bannedIpAddressID);

            return ipAddress;
        }

        /// <summary>
        /// Deletes an IP address
        /// </summary>
        /// <param name="bannedIpAddressID">IP address unique identifier</param>
        public override void DeleteBannedIpAddress(int bannedIpAddressID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BannedIpAddressDelete");
            db.AddInParameter(dbCommand, "BannedIpAddressID", DbType.Int32, bannedIpAddressID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets an IP Network by its ID
        /// </summary>
        /// <param name="bannedIpNetworkID">IP network unique identifier</param>
        /// <returns>IP network</returns>
        public override DBBannedIpNetwork GetIpNetworkByID(int bannedIpNetworkID)
        {
            DBBannedIpNetwork ipNetwork = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BannedIpNetworkLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "BannedIpNetworkID", DbType.Int32, bannedIpNetworkID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    ipNetwork = GetIpNetworkFromReader(dataReader);
                }
            }
            return ipNetwork;
        }

        /// <summary>
        /// Gets all IP networks
        /// </summary>
        /// <returns>IP network collection</returns>
        public override DBBannedIpNetworkCollection GetIpNetworkAll()
        {
            DBBannedIpNetworkCollection ipCollection = new DBBannedIpNetworkCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BannedIpNetworkLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBBannedIpNetwork ipNetwork = GetIpNetworkFromReader(dataReader);
                    ipCollection.Add(ipNetwork);
                }
            }
            return ipCollection;
        }

        /// <summary>
        /// Inserts an IP network
        /// </summary>
        /// <param name="startAddress">First IP address in the range</param>
        /// <param name="endAddress">Last IP address in the range</param>
        /// <param name="comment">Reason why the IP network was banned</param>
        /// <param name="ipException">Exceptions within the range</param>
        /// <param name="createdOn">When the record was inserted</param>
        /// <param name="updatedOn">When the record was last updated</param>
        /// <returns>IP network</returns>
        public override DBBannedIpNetwork InsertBannedIpNetwork(string startAddress, string endAddress, string comment, string ipException, DateTime createdOn, DateTime updatedOn)
        {
            DBBannedIpNetwork ipNetwork = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BannedIpNetworkInsert");
            db.AddOutParameter(dbCommand, "BannedIpNetworkID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "StartAddress", DbType.String, startAddress);
            db.AddInParameter(dbCommand, "EndAddress", DbType.String, endAddress);
            db.AddInParameter(dbCommand, "Comment", DbType.String, comment);
            db.AddInParameter(dbCommand, "IpException", DbType.String, ipException);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int ipNetworkID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@BannedIpNetworkID"));
                ipNetwork = GetIpNetworkByID(ipNetworkID);
            }
            return ipNetwork;
        }

        /// <summary>
        /// Updates an IP network
        /// </summary>
        /// <param name="bannedIpNetworkID">IP network unique identifier</param>
        /// <param name="startAddress">First IP address in the range</param>
        /// <param name="endAddress">Last IP address in the range</param>
        /// <param name="comment">Reason why the IP network was banned</param>
        /// <param name="ipException">Exceptions within the range</param>
        /// <param name="createdOn">When the record was inserted</param>
        /// <param name="updatedOn">When the record was last updated</param>
        /// <returns>IP network</returns>
        public override DBBannedIpNetwork UpdateBannedIpNetwork(int bannedIpNetworkID, string startAddress, string endAddress, string comment, string ipException, DateTime createdOn, DateTime updatedOn)
        {
            DBBannedIpNetwork ipNetwork = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BannedIpNetworkUpdate");
            db.AddInParameter(dbCommand, "BannedIpNetworkID", DbType.Int32, bannedIpNetworkID);
            db.AddInParameter(dbCommand, "StartAddress", DbType.String, startAddress);
            db.AddInParameter(dbCommand, "EndAddress", DbType.String, endAddress);
            db.AddInParameter(dbCommand, "Comment", DbType.String, comment);
            db.AddInParameter(dbCommand, "IpException", DbType.String, ipException);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                ipNetwork = GetIpNetworkByID(bannedIpNetworkID);

            return ipNetwork;
        }

        /// <summary>
        /// Deletes an IP network
        /// </summary>
        /// <param name="bannedIpNetworkID">IP network unique identifier</param>
        public override void DeleteBannedIpNetwork(int bannedIpNetworkID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BannedIpNetworkDelete");
            db.AddInParameter(dbCommand, "BannedIpNetworkID", DbType.Int32, bannedIpNetworkID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }
        #endregion
    }
}
