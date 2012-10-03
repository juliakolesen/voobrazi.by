using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NopSolutions.NopCommerce.DataAccess;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using System.Configuration.Provider;

namespace NopSolutions.NopCommerce.DataAccess.Colors
{
    class SqlColorsProvider:DBColorsProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBColors GetColorFromReader(IDataReader dataReader)
        {
            DBColors color = new DBColors();
            color.ColorName = NopSqlDataHelper.GetString(dataReader, "ColorName");
            color.ColorArgb = NopSqlDataHelper.GetLong(dataReader, "ColorArgb");
            color.ColorID = NopSqlDataHelper.GetInt(dataReader, "ColorID");

            return color;
        }
        #endregion

        #region Methods

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

        public override DBColors GetColorByColorName(string colorName)
        {
            DBColors color = null;
            if (String.IsNullOrEmpty(colorName))
                return color;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("ColorGetByColorName");
            db.AddInParameter(dbCommand, "ColorName", DbType.String, colorName);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    color = GetColorFromReader(dataReader);
                }
            }
            return color;
        }

        public override bool InsertColor(string colorName, int colorArgb)
        {
            DBColors color = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("ColorsInsert");
            db.AddInParameter(dbCommand, "ColorName", DbType.String, colorName);
            db.AddInParameter(dbCommand, "ColorArgb", DbType.Int32, colorArgb);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                return true;
            }

            return false;
        }

        public override void DeleteColor(string colorName)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("ColorsDelete");
            db.AddInParameter(dbCommand, "ColorName", DbType.String, colorName);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        public override void UpdateName(string oldName, string newName)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("ColorUpdateName");
            db.AddInParameter(dbCommand, "OldColorName", DbType.String, oldName);
            db.AddInParameter(dbCommand, "NewColorName", DbType.String, newName);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        #endregion Methods
    }
}
