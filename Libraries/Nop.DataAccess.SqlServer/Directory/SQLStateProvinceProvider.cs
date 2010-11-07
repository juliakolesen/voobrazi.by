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

namespace NopSolutions.NopCommerce.DataAccess.Directory
{
    /// <summary>
    /// State/province provider for SQL Server
    /// </summary>
    public partial class SQLStateProvinceProvider : DBStateProvinceProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBStateProvince GetStateProvinceFromReader(IDataReader dataReader)
        {
            DBStateProvince stateProvince = new DBStateProvince();
            stateProvince.StateProvinceID = NopSqlDataHelper.GetInt(dataReader, "StateProvinceID");
            stateProvince.CountryID = NopSqlDataHelper.GetInt(dataReader, "CountryID");
            stateProvince.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            stateProvince.Abbreviation = NopSqlDataHelper.GetString(dataReader, "Abbreviation");
            stateProvince.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            return stateProvince;
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
        /// Deletes a state/province
        /// </summary>
        /// <param name="StateProvinceID">The state/province identifier</param>
        public override void DeleteStateProvince(int StateProvinceID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_StateProvinceDelete");
            db.AddInParameter(dbCommand, "StateProvinceID", DbType.Int32, StateProvinceID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a state/province
        /// </summary>
        /// <param name="StateProvinceID">The state/province identifier</param>
        /// <returns>State/province</returns>
        public override DBStateProvince GetStateProvinceByID(int StateProvinceID)
        {
            DBStateProvince stateProvince = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_StateProvinceLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "StateProvinceID", DbType.Int32, StateProvinceID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    stateProvince = GetStateProvinceFromReader(dataReader);
                }
            }
            return stateProvince;
        }

        /// <summary>
        /// Gets a state/province 
        /// </summary>
        /// <param name="Abbreviation">The state/province abbreviation</param>
        /// <returns>State/province</returns>
        public override DBStateProvince GetStateProvinceByAbbreviation(string Abbreviation)
        {
            DBStateProvince stateProvince = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_StateProvinceLoadByAbbreviation");
            db.AddInParameter(dbCommand, "Abbreviation", DbType.String, Abbreviation);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    stateProvince = GetStateProvinceFromReader(dataReader);
                }
            }
            return stateProvince;
        }

        /// <summary>
        /// Gets a state/province collection by country identifier
        /// </summary>
        /// <param name="CountryID">Country identifier</param>
        /// <returns>State/province collection</returns>
        public override DBStateProvinceCollection GetStateProvincesByCountryID(int CountryID)
        {
            DBStateProvinceCollection stateProvinceCollection = new DBStateProvinceCollection();
            if (CountryID == 0)
                return stateProvinceCollection;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_StateProvinceLoadAllByCountryID");
            db.AddInParameter(dbCommand, "CountryID", DbType.Int32, CountryID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBStateProvince stateProvince = GetStateProvinceFromReader(dataReader);
                    stateProvinceCollection.Add(stateProvince);
                }
            }
            return stateProvinceCollection;
        }

        /// <summary>
        /// Inserts a state/province
        /// </summary>
        /// <param name="CountryID">The country identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="Abbreviation">The abbreviation</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>State/province</returns>
        public override DBStateProvince InsertStateProvince(int CountryID, string Name, string Abbreviation, int DisplayOrder)
        {
            DBStateProvince stateProvince = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_StateProvinceInsert");
            db.AddOutParameter(dbCommand, "StateProvinceID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "CountryID", DbType.Int32, CountryID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "Abbreviation", DbType.String, Abbreviation);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int StateProvinceID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@StateProvinceID"));
                stateProvince = GetStateProvinceByID(StateProvinceID);
            }
            return stateProvince;
        }

        /// <summary>
        /// Updates a state/province
        /// </summary>
        /// <param name="StateProvinceID">The state/province identifier</param>
        /// <param name="CountryID">The country identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="Abbreviation">The abbreviation</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>State/province</returns>
        public override DBStateProvince UpdateStateProvince(int StateProvinceID, int CountryID, string Name, string Abbreviation, int DisplayOrder)
        {
            DBStateProvince stateProvince = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_StateProvinceUpdate");
            db.AddInParameter(dbCommand, "StateProvinceID", DbType.Int32, StateProvinceID);
            db.AddInParameter(dbCommand, "CountryID", DbType.Int32, CountryID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "Abbreviation", DbType.String, Abbreviation);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                stateProvince = GetStateProvinceByID(StateProvinceID);

            return stateProvince;
        }
        #endregion
    }
}
