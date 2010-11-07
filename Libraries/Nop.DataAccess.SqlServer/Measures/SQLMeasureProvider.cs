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

namespace NopSolutions.NopCommerce.DataAccess.Measures
{
    /// <summary>
    /// Measure provider for SQL Server
    /// </summary>
    public partial class SQLMeasureProvider : DBMeasureProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBMeasureWeight GetMeasureWeightFromReader(IDataReader dataReader)
        {
            DBMeasureWeight measureWeight = new DBMeasureWeight();
            measureWeight.MeasureWeightID = NopSqlDataHelper.GetInt(dataReader, "MeasureWeightID");
            measureWeight.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            measureWeight.SystemKeyword = NopSqlDataHelper.GetString(dataReader, "SystemKeyword");
            measureWeight.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            return measureWeight;
        }

        private DBMeasureDimension GetMeasureDimensionFromReader(IDataReader dataReader)
        {
            DBMeasureDimension measureDimension = new DBMeasureDimension();
            measureDimension.MeasureDimensionID = NopSqlDataHelper.GetInt(dataReader, "MeasureDimensionID");
            measureDimension.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            measureDimension.SystemKeyword = NopSqlDataHelper.GetString(dataReader, "SystemKeyword");
            measureDimension.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            return measureDimension;
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
        /// Gets a measure dimension by identifier
        /// </summary>
        /// <param name="MeasureDimensionID">Measure dimension identifier</param>
        /// <returns>Measure dimension</returns>
        public override DBMeasureDimension GetMeasureDimensionByID(int MeasureDimensionID)
        {
            DBMeasureDimension measureDimension = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_MeasureDimensionLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "MeasureDimensionID", DbType.Int32, MeasureDimensionID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    measureDimension = GetMeasureDimensionFromReader(dataReader);
                }
            }
            return measureDimension;
        }

        /// <summary>
        /// Gets all measure dimensions
        /// </summary>
        /// <returns>Measure dimension collection</returns>
        public override DBMeasureDimensionCollection GetAllMeasureDimensions()
        {

            DBMeasureDimensionCollection measureDimensionCollection = new DBMeasureDimensionCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_MeasureDimensionLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBMeasureDimension measureDimension = GetMeasureDimensionFromReader(dataReader);
                    measureDimensionCollection.Add(measureDimension);
                }
            }

            return measureDimensionCollection;
        }

        /// <summary>
        /// Gets a measure weight by identifier
        /// </summary>
        /// <param name="MeasureWeightID">Measure weight identifier</param>
        /// <returns>Measure weight</returns>
        public override DBMeasureWeight GetMeasureWeightByID(int MeasureWeightID)
        {
            DBMeasureWeight measureWeight = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_MeasureWeightLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "MeasureWeightID", DbType.Int32, MeasureWeightID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    measureWeight = GetMeasureWeightFromReader(dataReader);
                }
            }
            return measureWeight;
        }

        /// <summary>
        /// Gets all measure weights
        /// </summary>
        /// <returns>Measure weight collection</returns>
        public override DBMeasureWeightCollection GetAllMeasureWeights()
        {

            DBMeasureWeightCollection measureWeightCollection = new DBMeasureWeightCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_MeasureWeightLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBMeasureWeight measureWeight = GetMeasureWeightFromReader(dataReader);
                    measureWeightCollection.Add(measureWeight);
                }
            }

            return measureWeightCollection;
        }


        #endregion
    }
}
