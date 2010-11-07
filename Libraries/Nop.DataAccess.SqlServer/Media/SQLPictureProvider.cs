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

namespace NopSolutions.NopCommerce.DataAccess.Media
{
    /// <summary>
    /// Picture provider for SQL Server
    /// </summary>
    public partial class SQLPictureProvider : DBPictureProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBPicture GetPictureFromReader(IDataReader dataReader)
        {
            DBPicture picture = new DBPicture();
            picture.PictureID = NopSqlDataHelper.GetInt(dataReader, "PictureID");
            picture.PictureBinary = NopSqlDataHelper.GetBytes(dataReader, "PictureBinary");
            picture.Extension = NopSqlDataHelper.GetString(dataReader, "Extension");
            picture.IsNew = NopSqlDataHelper.GetBoolean(dataReader, "IsNew");
            return picture;
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
        /// Gets a picture
        /// </summary>
        /// <param name="PictureID">Picture identifier</param>
        /// <returns>Picture</returns>
        public override DBPicture GetPictureByID(int PictureID)
        {
            DBPicture picture = null;
            if (PictureID == 0)
                return picture;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PictureLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "PictureID", DbType.Int32, PictureID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    picture = GetPictureFromReader(dataReader);
                }
            }
            return picture;
        }

        /// <summary>
        /// Deletes a picture
        /// </summary>
        /// <param name="PictureID">Picture identifier</param>
        public override void DeletePicture(int PictureID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PictureDelete");
            db.AddInParameter(dbCommand, "PictureID", DbType.Int32, PictureID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Inserts a picture
        /// </summary>
        /// <param name="PictureBinary">The picture binary</param>
        /// <param name="Extension">The picture extension</param>
        /// <param name="IsNew">A value indicating whether the picture is new</param>
        /// <returns>Picture</returns>
        public override DBPicture InsertPicture(byte[] PictureBinary, string Extension, bool IsNew)
        {
            DBPicture picture = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PictureInsert");
            db.AddOutParameter(dbCommand, "PictureID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "PictureBinary", DbType.Binary, PictureBinary);
            db.AddInParameter(dbCommand, "Extension", DbType.String, Extension);
            db.AddInParameter(dbCommand, "IsNew", DbType.Boolean, IsNew);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int PictureID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@PictureID"));
                picture = GetPictureByID(PictureID);
            }
            return picture;
        }

        /// <summary>
        /// Updates the picture
        /// </summary>
        /// <param name="PictureID">The picture identifier</param>
        /// <param name="PictureBinary">The picture binary</param>
        /// <param name="Extension">The picture extension</param>
        /// <param name="IsNew">A value indicating whether the picture is new</param>
        /// <returns>Picture</returns>
        public override DBPicture UpdatePicture(int PictureID, byte[] PictureBinary, string Extension, bool IsNew)
        {
            DBPicture picture = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PictureUpdate");
            db.AddInParameter(dbCommand, "PictureID", DbType.Int32, PictureID);
            db.AddInParameter(dbCommand, "PictureBinary", DbType.Binary, PictureBinary);
            db.AddInParameter(dbCommand, "Extension", DbType.String, Extension);
            db.AddInParameter(dbCommand, "IsNew", DbType.Boolean, IsNew);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                picture = GetPictureByID(PictureID);

            return picture;
        }
        #endregion
    }
}
