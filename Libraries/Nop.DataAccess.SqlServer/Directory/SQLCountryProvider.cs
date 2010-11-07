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
    /// Country provider for SQL Server
    /// </summary>
    public partial class SQLCountryProvider : DBCountryProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBCountry GetCountryFromReader(IDataReader dataReader)
        {
            DBCountry country = new DBCountry();
            country.CountryID = NopSqlDataHelper.GetInt(dataReader, "CountryID");
            country.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            country.AllowsRegistration = NopSqlDataHelper.GetBoolean(dataReader, "AllowsRegistration");
            country.AllowsBilling = NopSqlDataHelper.GetBoolean(dataReader, "AllowsBilling");
            country.AllowsShipping = NopSqlDataHelper.GetBoolean(dataReader, "AllowsShipping");
            country.TwoLetterISOCode = NopSqlDataHelper.GetString(dataReader, "TwoLetterISOCode");
            country.ThreeLetterISOCode = NopSqlDataHelper.GetString(dataReader, "ThreeLetterISOCode");
            country.NumericISOCode = NopSqlDataHelper.GetInt(dataReader, "NumericISOCode");
            country.Published = NopSqlDataHelper.GetBoolean(dataReader, "Published");
            country.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            return country;
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
        /// Deletes a country
        /// </summary>
        /// <param name="CountryID">Country identifier</param>
        public override void DeleteCountry(int CountryID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CountryDelete");
            db.AddInParameter(dbCommand, "CountryID", DbType.Int32, CountryID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets all countries
        /// </summary>
        /// <returns>Country collection</returns>
        public override DBCountryCollection GetAllCountries(bool showHidden)
        {
            DBCountryCollection countryCollection = new DBCountryCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CountryLoadAll");
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBCountry country = GetCountryFromReader(dataReader);
                    countryCollection.Add(country);
                }
            }
            return countryCollection;
        }

        /// <summary>
        /// Gets all countries that allow registration
        /// </summary>
        /// <returns>Country collection</returns>
        public override DBCountryCollection GetAllCountriesForRegistration(bool showHidden)
        {
            DBCountryCollection countryCollection = new DBCountryCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CountryLoadAllForRegistration");
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBCountry country = GetCountryFromReader(dataReader);
                    countryCollection.Add(country);
                }
            }
            return countryCollection;
        }

        /// <summary>
        /// Gets all countries that allow billing
        /// </summary>
        /// <returns>Country collection</returns>
        public override DBCountryCollection GetAllCountriesForBilling(bool showHidden)
        {
            DBCountryCollection countryCollection = new DBCountryCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CountryLoadAllForBilling");
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBCountry country = GetCountryFromReader(dataReader);
                    countryCollection.Add(country);
                }
            }
            return countryCollection;
        }

        /// <summary>
        /// Gets all countries that allow shipping
        /// </summary>
        /// <returns>Country collection</returns>
        public override DBCountryCollection GetAllCountriesForShipping(bool showHidden)
        {
            DBCountryCollection countryCollection = new DBCountryCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CountryLoadAllForShipping");
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBCountry country = GetCountryFromReader(dataReader);
                    countryCollection.Add(country);
                }
            }
            return countryCollection;
        }

        /// <summary>
        /// Gets a country 
        /// </summary>
        /// <param name="CountryID">Country identifier</param>
        /// <returns>Country</returns>
        public override DBCountry GetCountryByID(int CountryID)
        {
            DBCountry country = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CountryLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "CountryID", DbType.Int32, CountryID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    country = GetCountryFromReader(dataReader);
                }
            }
            return country;
        }

        /// <summary>
        /// Gets a country by two letter ISO code
        /// </summary>
        /// <param name="TwoLetterISOCode">Country two letter ISO code</param>
        /// <returns>Country</returns>
        public override DBCountry GetCountryByTwoLetterISOCode(string TwoLetterISOCode)
        {
            DBCountry country = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CountryLoadByTwoLetterISOCode");
            db.AddInParameter(dbCommand, "TwoLetterISOCode", DbType.String, TwoLetterISOCode);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    country = GetCountryFromReader(dataReader);
                }
            }
            return country;
        }

        /// <summary>
        /// Gets a country by three letter ISO code
        /// </summary>
        /// <param name="ThreeLetterISOCode">Country three letter ISO code</param>
        /// <returns>Country</returns>
        public override DBCountry GetCountryByThreeLetterISOCode(string ThreeLetterISOCode)
        {
            DBCountry country = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CountryLoadByThreeLetterISOCode");
            db.AddInParameter(dbCommand, "ThreeLetterISOCode", DbType.String, ThreeLetterISOCode);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    country = GetCountryFromReader(dataReader);
                }
            }
            return country;
        }

        /// <summary>
        /// Inserts a country
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="AllowsRegistration">A value indicating whether registration is allowed to this country</param>
        /// <param name="AllowsBilling">A value indicating whether billing is allowed to this country</param>
        /// <param name="AllowsShipping">A value indicating whether shipping is allowed to this country</param>
        /// <param name="TwoLetterISOCode">The two letter ISO code</param>
        /// <param name="ThreeLetterISOCode">The three letter ISO code</param>
        /// <param name="NumericISOCode">The numeric ISO code</param>
        /// <param name="Published">A value indicating whether the entity is published</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Country</returns>
        public override DBCountry InsertCountry(string Name,
            bool AllowsRegistration, bool AllowsBilling, bool AllowsShipping,
            string TwoLetterISOCode, string ThreeLetterISOCode, int NumericISOCode,
            bool Published, int DisplayOrder)
        {
            DBCountry country = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CountryInsert");
            db.AddOutParameter(dbCommand, "CountryID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "AllowsRegistration", DbType.Boolean, AllowsRegistration);
            db.AddInParameter(dbCommand, "AllowsBilling", DbType.Boolean, AllowsBilling);
            db.AddInParameter(dbCommand, "AllowsShipping", DbType.Boolean, AllowsShipping);
            db.AddInParameter(dbCommand, "TwoLetterISOCode", DbType.String, TwoLetterISOCode);
            db.AddInParameter(dbCommand, "ThreeLetterISOCode", DbType.String, ThreeLetterISOCode);
            db.AddInParameter(dbCommand, "NumericISOCode", DbType.Int32, NumericISOCode);
            db.AddInParameter(dbCommand, "Published", DbType.Boolean, Published);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int CountryID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@CountryID"));
                country = GetCountryByID(CountryID);
            }
            return country;
        }

        /// <summary>
        /// Updates the country
        /// </summary>
        /// <param name="CountryID">The country identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="AllowsRegistration">A value indicating whether registration is allowed to this country</param>
        /// <param name="AllowsBilling">A value indicating whether billing is allowed to this country</param>
        /// <param name="AllowsShipping">A value indicating whether shipping is allowed to this country</param>
        /// <param name="TwoLetterISOCode">The two letter ISO code</param>
        /// <param name="ThreeLetterISOCode">The three letter ISO code</param>
        /// <param name="NumericISOCode">The numeric ISO code</param>
        /// <param name="Published">A value indicating whether the entity is published</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Country</returns>
        public override DBCountry UpdateCountry(int CountryID, string Name,
            bool AllowsRegistration, bool AllowsBilling, bool AllowsShipping,
            string TwoLetterISOCode, string ThreeLetterISOCode, int NumericISOCode,
            bool Published, int DisplayOrder)
        {
            DBCountry country = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CountryUpdate");
            db.AddInParameter(dbCommand, "CountryID", DbType.Int32, CountryID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "AllowsRegistration", DbType.Boolean, AllowsRegistration);
            db.AddInParameter(dbCommand, "AllowsBilling", DbType.Boolean, AllowsBilling);
            db.AddInParameter(dbCommand, "AllowsShipping", DbType.Boolean, AllowsShipping);
            db.AddInParameter(dbCommand, "TwoLetterISOCode", DbType.String, TwoLetterISOCode);
            db.AddInParameter(dbCommand, "ThreeLetterISOCode", DbType.String, ThreeLetterISOCode);
            db.AddInParameter(dbCommand, "NumericISOCode", DbType.Int32, NumericISOCode);
            db.AddInParameter(dbCommand, "Published", DbType.Boolean, Published);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                country = GetCountryByID(CountryID);
            return country;
        }
        #endregion
    }
}
