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

namespace NopSolutions.NopCommerce.DataAccess.Payment
{
    /// <summary>
    /// Payment method provider for SQL Server
    /// </summary>
    public partial class SQLPaymentMethodProvider : DBPaymentMethodProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBPaymentMethod GetPaymentMethodFromReader(IDataReader dataReader)
        {
            DBPaymentMethod paymentMethod = new DBPaymentMethod();
            paymentMethod.PaymentMethodID = NopSqlDataHelper.GetInt(dataReader, "PaymentMethodID");
            paymentMethod.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            paymentMethod.VisibleName = NopSqlDataHelper.GetString(dataReader, "VisibleName");
            paymentMethod.Description = NopSqlDataHelper.GetString(dataReader, "Description");
            paymentMethod.ConfigureTemplatePath = NopSqlDataHelper.GetString(dataReader, "ConfigureTemplatePath");
            paymentMethod.UserTemplatePath = NopSqlDataHelper.GetString(dataReader, "UserTemplatePath");
            paymentMethod.ClassName = NopSqlDataHelper.GetString(dataReader, "ClassName");
            paymentMethod.SystemKeyword = NopSqlDataHelper.GetString(dataReader, "SystemKeyword");
            paymentMethod.IsActive = NopSqlDataHelper.GetBoolean(dataReader, "IsActive");
            paymentMethod.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            return paymentMethod;
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
        /// Deletes a payment method
        /// </summary>
        /// <param name="PaymentMethodID">Payment method identifier</param>
        public override void DeletePaymentMethod(int PaymentMethodID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PaymentMethodDelete");
            db.AddInParameter(dbCommand, "PaymentMethodID", DbType.Int32, PaymentMethodID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a payment method
        /// </summary>
        /// <param name="PaymentMethodID">Payment method identifier</param>
        /// <returns>Payment method</returns>
        public override DBPaymentMethod GetPaymentMethodByID(int PaymentMethodID)
        {
            DBPaymentMethod paymentMethod = null;
            if (PaymentMethodID == 0)
                return paymentMethod;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PaymentMethodLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "PaymentMethodID", DbType.Int32, PaymentMethodID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    paymentMethod = GetPaymentMethodFromReader(dataReader);
                }
            }
            return paymentMethod;
        }

        /// <summary>
        /// Gets a payment method
        /// </summary>
        /// <param name="SystemKeyword">Payment method system keyword</param>
        /// <returns>Payment method</returns>
        public override DBPaymentMethod GetPaymentMethodBySystemKeyword(string SystemKeyword)
        {

            DBPaymentMethod paymentMethod = null;
            if (String.IsNullOrEmpty(SystemKeyword))
                return paymentMethod;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PaymentMethodLoadBySystemKeyword");
            db.AddInParameter(dbCommand, "SystemKeyword", DbType.String, SystemKeyword);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    paymentMethod = GetPaymentMethodFromReader(dataReader);
                }
            }
            return paymentMethod;
        }

        /// <summary>
        /// Gets all payment methods
        /// </summary>
        /// <returns>Payment method collection</returns>
        public override DBPaymentMethodCollection GetAllPaymentMethods(bool showHidden)
        {
            DBPaymentMethodCollection paymentMethodCollection = new DBPaymentMethodCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PaymentMethodLoadAll");
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBPaymentMethod paymentMethod = GetPaymentMethodFromReader(dataReader);
                    paymentMethodCollection.Add(paymentMethod);
                }
            }
            return paymentMethodCollection;
        }

        /// <summary>
        /// Inserts a payment method
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="VisibleName">The visible name</param>
        /// <param name="Description">The description</param>
        /// <param name="ConfigureTemplatePath">The configure template path</param>
        /// <param name="UserTemplatePath">The user template path</param>
        /// <param name="ClassName">The class name</param>
        /// <param name="SystemKeyword">The system keyword</param>
        /// <param name="IsActive">A value indicating whether the payment method is active</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Payment method</returns>
        public override DBPaymentMethod InsertPaymentMethod(string Name, string VisibleName, string Description,
           string ConfigureTemplatePath, string UserTemplatePath, string ClassName,string SystemKeyword,  bool IsActive, int DisplayOrder)
        {
            DBPaymentMethod paymentMethod = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PaymentMethodInsert");
            db.AddOutParameter(dbCommand, "PaymentMethodID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "VisibleName", DbType.String, VisibleName);
            db.AddInParameter(dbCommand, "Description", DbType.String, Description);
            db.AddInParameter(dbCommand, "ConfigureTemplatePath", DbType.String, ConfigureTemplatePath);
            db.AddInParameter(dbCommand, "UserTemplatePath", DbType.String, UserTemplatePath);
            db.AddInParameter(dbCommand, "ClassName", DbType.String, ClassName);
            db.AddInParameter(dbCommand, "SystemKeyword", DbType.String, SystemKeyword);
            db.AddInParameter(dbCommand, "IsActive", DbType.Boolean, IsActive);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int PaymentMethodID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@PaymentMethodID"));
                paymentMethod = GetPaymentMethodByID(PaymentMethodID);
            }
            return paymentMethod;
        }

        /// <summary>
        /// Updates the payment method
        /// </summary>
        /// <param name="PaymentMethodID">The payment method identifer</param>
        /// <param name="Name">The name</param>
        /// <param name="VisibleName">The visible name</param>
        /// <param name="Description">The description</param>
        /// <param name="ConfigureTemplatePath">The configure template path</param>
        /// <param name="UserTemplatePath">The user template path</param>
        /// <param name="ClassName">The class name</param>
        /// <param name="SystemKeyword">The system keyword</param>
        /// <param name="IsActive">A value indicating whether the payment method is active</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Payment method</returns>
        public override DBPaymentMethod UpdatePaymentMethod(int PaymentMethodID, string Name, string VisibleName, string Description,
           string ConfigureTemplatePath, string UserTemplatePath, string ClassName,string SystemKeyword, bool IsActive, int DisplayOrder)
        {
            DBPaymentMethod paymentMethod = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PaymentMethodUpdate");
            db.AddInParameter(dbCommand, "PaymentMethodID", DbType.Int32, PaymentMethodID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "VisibleName", DbType.String, VisibleName);
            db.AddInParameter(dbCommand, "Description", DbType.String, Description);
            db.AddInParameter(dbCommand, "ConfigureTemplatePath", DbType.String, ConfigureTemplatePath);
            db.AddInParameter(dbCommand, "UserTemplatePath", DbType.String, UserTemplatePath);
            db.AddInParameter(dbCommand, "ClassName", DbType.String, ClassName);
            db.AddInParameter(dbCommand, "SystemKeyword", DbType.String, SystemKeyword);
            db.AddInParameter(dbCommand, "IsActive", DbType.Boolean, IsActive);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                paymentMethod = GetPaymentMethodByID(PaymentMethodID);

            return paymentMethod;
        }
        #endregion
    }
}
