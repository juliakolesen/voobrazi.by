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
namespace NopSolutions.NopCommerce.DataAccess.Promo.Discounts
{
    /// <summary>
    /// Discount provider for SQL Server
    /// </summary>
    public partial class SQLDiscountProvider : DBDiscountProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBDiscount GetDiscountFromReader(IDataReader dataReader)
        {
            DBDiscount discount = new DBDiscount();
            discount.DiscountID = NopSqlDataHelper.GetInt(dataReader, "DiscountID");
            discount.DiscountTypeID = NopSqlDataHelper.GetInt(dataReader, "DiscountTypeID");
            discount.DiscountRequirementID = NopSqlDataHelper.GetInt(dataReader, "DiscountRequirementID");
            discount.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            discount.UsePercentage = NopSqlDataHelper.GetBoolean(dataReader, "UsePercentage");
            discount.DiscountPercentage = NopSqlDataHelper.GetDecimal(dataReader, "DiscountPercentage");
            discount.DiscountAmount = NopSqlDataHelper.GetDecimal(dataReader, "DiscountAmount");
            discount.StartDate = NopSqlDataHelper.GetUtcDateTime(dataReader, "StartDate");
            discount.EndDate = NopSqlDataHelper.GetUtcDateTime(dataReader, "EndDate");
            discount.RequiresCouponCode = NopSqlDataHelper.GetBoolean(dataReader, "RequiresCouponCode");
            discount.CouponCode = NopSqlDataHelper.GetString(dataReader, "CouponCode");
            discount.Deleted = NopSqlDataHelper.GetBoolean(dataReader, "Deleted");
            return discount;
        }

        private DBDiscountRequirement GetDiscountRequirementFromReader(IDataReader dataReader)
        {
            DBDiscountRequirement discountRequirement = new DBDiscountRequirement();
            discountRequirement.DiscountRequirementID = NopSqlDataHelper.GetInt(dataReader, "DiscountRequirementID");
            discountRequirement.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            return discountRequirement;
        }

        private DBDiscountType GetDiscountTypeFromReader(IDataReader dataReader)
        {
            DBDiscountType discountType = new DBDiscountType();
            discountType.DiscountTypeID = NopSqlDataHelper.GetInt(dataReader, "DiscountTypeID");
            discountType.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            return discountType;
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
        /// Gets a discount
        /// </summary>
        /// <param name="DiscountID">Discount identifier</param>
        /// <returns>Discount</returns>
        public override DBDiscount GetDiscountByID(int DiscountID)
        {
            DBDiscount discount = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_DiscountLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "DiscountID", DbType.Int32, DiscountID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    discount = GetDiscountFromReader(dataReader);
                }
            }
            return discount;
        }

        /// <summary>
        /// Gets all discounts
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="DiscountTypeID">Discount type identifier; null to load all discount</param>
        /// <returns>Discount collection</returns>
        public override DBDiscountCollection GetAllDiscounts(bool showHidden, int? DiscountTypeID)
        {
            DBDiscountCollection discountCollection = new DBDiscountCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_DiscountLoadAll");
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            if (DiscountTypeID.HasValue)
                db.AddInParameter(dbCommand, "DiscountTypeID", DbType.Int32, DiscountTypeID.Value);
            else
                db.AddInParameter(dbCommand, "DiscountTypeID", DbType.Int32, null);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBDiscount discount = GetDiscountFromReader(dataReader);
                    discountCollection.Add(discount);
                }
            }

            return discountCollection;
        }

        /// <summary>
        /// Inserts a discount
        /// </summary>
        /// <param name="DiscountTypeID">The discount type identifier</param>
        /// <param name="DiscountRequirementID">The discount requirement identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="UsePercentage">A value indicating whether to use percentage</param>
        /// <param name="DiscountPercentage">The discount percentage</param>
        /// <param name="DiscountAmount">The discount amount</param>
        /// <param name="StartDate">The discount start date and time</param>
        /// <param name="EndDate">The discount end date and time</param>
        /// <param name="RequiresCouponCode">The value indicating whether discount requires coupon code</param>
        /// <param name="CouponCode">The coupon code</param>
        /// <param name="Deleted">A value indicating whether the entity has been deleted</param>
        /// <returns>Discount</returns>
        public override DBDiscount InsertDiscount(int DiscountTypeID, int DiscountRequirementID, string Name, bool UsePercentage, decimal DiscountPercentage, decimal DiscountAmount,
            DateTime StartDate, DateTime EndDate, bool RequiresCouponCode, string CouponCode, bool Deleted)
        {
            DBDiscount discount = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_DiscountInsert");
            db.AddOutParameter(dbCommand, "DiscountID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "DiscountTypeID", DbType.Int32, DiscountTypeID);
            db.AddInParameter(dbCommand, "DiscountRequirementID", DbType.Int32, DiscountRequirementID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "UsePercentage", DbType.Boolean, UsePercentage);
            db.AddInParameter(dbCommand, "DiscountPercentage", DbType.Decimal, DiscountPercentage);
            db.AddInParameter(dbCommand, "DiscountAmount", DbType.Decimal, DiscountAmount);
            db.AddInParameter(dbCommand, "StartDate", DbType.DateTime, StartDate);
            db.AddInParameter(dbCommand, "EndDate", DbType.DateTime, EndDate);
            db.AddInParameter(dbCommand, "RequiresCouponCode", DbType.Boolean, RequiresCouponCode);
            db.AddInParameter(dbCommand, "CouponCode", DbType.String, CouponCode);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, Deleted);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int DiscountID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@DiscountID"));
                discount = GetDiscountByID(DiscountID);
            }
            return discount;
        }

        /// <summary>
        /// Updates the discount
        /// </summary>
        /// <param name="DiscountID">Discount identifier</param>
        /// <param name="DiscountTypeID">The discount type identifier</param>
        /// <param name="DiscountRequirementID">The discount requirement identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="UsePercentage">A value indicating whether to use percentage</param>
        /// <param name="DiscountPercentage">The discount percentage</param>
        /// <param name="DiscountAmount">The discount amount</param>
        /// <param name="StartDate">The discount start date and time</param>
        /// <param name="EndDate">The discount end date and time</param>
        /// <param name="RequiresCouponCode">The value indicating whether discount requires coupon code</param>
        /// <param name="CouponCode">The coupon code</param>
        /// <param name="Deleted">A value indicating whether the entity has been deleted</param>
        /// <returns>Discount</returns>
        public override DBDiscount UpdateDiscount(int DiscountID, int DiscountTypeID, int DiscountRequirementID, string Name, bool UsePercentage, decimal DiscountPercentage, decimal DiscountAmount,
            DateTime StartDate, DateTime EndDate, bool RequiresCouponCode, string CouponCode, bool Deleted)
        {
            DBDiscount discount = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_DiscountUpdate");
            db.AddInParameter(dbCommand, "DiscountID", DbType.Int32, DiscountID);
            db.AddInParameter(dbCommand, "DiscountTypeID", DbType.Int32, DiscountTypeID);
            db.AddInParameter(dbCommand, "DiscountRequirementID", DbType.Int32, DiscountRequirementID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "UsePercentage", DbType.Boolean, UsePercentage);
            db.AddInParameter(dbCommand, "DiscountPercentage", DbType.Decimal, DiscountPercentage);
            db.AddInParameter(dbCommand, "DiscountAmount", DbType.Decimal, DiscountAmount);
            db.AddInParameter(dbCommand, "StartDate", DbType.DateTime, StartDate);
            db.AddInParameter(dbCommand, "EndDate", DbType.DateTime, EndDate);
            db.AddInParameter(dbCommand, "RequiresCouponCode", DbType.Boolean, RequiresCouponCode);
            db.AddInParameter(dbCommand, "CouponCode", DbType.String, CouponCode);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, Deleted);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                discount = GetDiscountByID(DiscountID);

            return discount;
        }

        /// <summary>
        /// Adds a discount to a product variant
        /// </summary>
        /// <param name="ProductVariantID">Product variant identifier</param>
        /// <param name="DiscountID">Discount identifier</param>
        public override void AddDiscountToProductVariant(int ProductVariantID, int DiscountID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariant_Discount_MappingInsert");
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, ProductVariantID);
            db.AddInParameter(dbCommand, "DiscountID", DbType.Int32, DiscountID);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Removes a discount from a product variant
        /// </summary>
        /// <param name="ProductVariantID">Product variant identifier</param>
        /// <param name="DiscountID">Discount identifier</param>
        public override void RemoveDiscountFromProductVariant(int ProductVariantID, int DiscountID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariant_Discount_MappingDelete");
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, ProductVariantID);
            db.AddInParameter(dbCommand, "DiscountID", DbType.Int32, DiscountID);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a discount collection of a product variant
        /// </summary>
        /// <param name="ProductVariantID">Product variant identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Discount collection</returns>
        public override DBDiscountCollection GetDiscountsByProductVariantID(int ProductVariantID, bool showHidden)
        {
            DBDiscountCollection discountCollection = new DBDiscountCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_DiscountLoadByProductVariantID");
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, ProductVariantID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBDiscount discount = GetDiscountFromReader(dataReader);
                    discountCollection.Add(discount);
                }
            }

            return discountCollection;
        }

        /// <summary>
        /// Adds a discount to a category
        /// </summary>
        /// <param name="CategoryID">Category identifier</param>
        /// <param name="DiscountID">Discount identifier</param>
        public override void AddDiscountToCategory(int CategoryID, int DiscountID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Category_Discount_MappingInsert");
            db.AddInParameter(dbCommand, "CategoryID", DbType.Int32, CategoryID);
            db.AddInParameter(dbCommand, "DiscountID", DbType.Int32, DiscountID);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Removes a discount from a category
        /// </summary>
        /// <param name="CategoryID">Category identifier</param>
        /// <param name="DiscountID">Discount identifier</param>
        public override void RemoveDiscountFromCategory(int CategoryID, int DiscountID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Category_Discount_MappingDelete");
            db.AddInParameter(dbCommand, "CategoryID", DbType.Int32, CategoryID);
            db.AddInParameter(dbCommand, "DiscountID", DbType.Int32, DiscountID);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a discount collection of a category
        /// </summary>
        /// <param name="CategoryID">Category identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Discount collection</returns>
        public override DBDiscountCollection GetDiscountsByCategoryID(int CategoryID, bool showHidden)
        {
            DBDiscountCollection discountCollection = new DBDiscountCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_DiscountLoadByCategoryID");
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            db.AddInParameter(dbCommand, "CategoryID", DbType.Int32, CategoryID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBDiscount discount = GetDiscountFromReader(dataReader);
                    discountCollection.Add(discount);
                }
            }

            return discountCollection;
        }

        /// <summary>
        /// Gets all discount requirements
        /// </summary>
        /// <returns>Discount requirement collection</returns>
        public override DBDiscountRequirementCollection GetAllDiscountRequirements()
        {
            DBDiscountRequirementCollection discountRequirementCollection = new DBDiscountRequirementCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_DiscountRequirementLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBDiscountRequirement discountRequirement = GetDiscountRequirementFromReader(dataReader);
                    discountRequirementCollection.Add(discountRequirement);
                }
            }

            return discountRequirementCollection;
        }

        /// <summary>
        /// Gets all discount types
        /// </summary>
        /// <returns>Discount type collection</returns>
        public override DBDiscountTypeCollection GetAllDiscountTypes()
        {
            DBDiscountTypeCollection discountTypeCollection = new DBDiscountTypeCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_DiscountTypeLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBDiscountType discountType = GetDiscountTypeFromReader(dataReader);
                    discountTypeCollection.Add(discountType);
                }
            }

            return discountTypeCollection;
        }
        
        #endregion
    }
}
