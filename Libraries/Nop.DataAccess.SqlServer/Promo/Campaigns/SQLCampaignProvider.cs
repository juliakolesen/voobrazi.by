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

namespace NopSolutions.NopCommerce.DataAccess.Promo.Campaigns
{
    /// <summary>
    /// Campaign provider for SQL Server
    /// </summary>
    public partial class SQLCampaignProvider : DBCampaignProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBCampaign GetCampaignFromReader(IDataReader dataReader)
        {
            DBCampaign campaign = new DBCampaign();
            campaign.CampaignID = NopSqlDataHelper.GetInt(dataReader, "CampaignID");
            campaign.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            campaign.Subject = NopSqlDataHelper.GetString(dataReader, "Subject");
            campaign.Body = NopSqlDataHelper.GetString(dataReader, "Body");
            campaign.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            return campaign;
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
        /// Gets a campaign by campaign identifier
        /// </summary>
        /// <param name="CampaignID">Campaign identifier</param>
        /// <returns>Message template</returns>
        public override DBCampaign GetCampaignByID(int CampaignID)
        {
            DBCampaign campaign = null;
            if (CampaignID == 0)
                return campaign;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CampaignLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "CampaignID", DbType.Int32, CampaignID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    campaign = GetCampaignFromReader(dataReader);
                }
            }
            return campaign;
        }

        /// <summary>
        /// Deletes a campaign
        /// </summary>
        /// <param name="CampaignID">Campaign identifier</param>
        public override void DeleteCampaign(int CampaignID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CampaignDelete");
            db.AddInParameter(dbCommand, "CampaignID", DbType.Int32, CampaignID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets all campaigns
        /// </summary>
        /// <returns>Campaign collection</returns>
        public override DBCampaignCollection GetAllCampaigns()
        {
            DBCampaignCollection campaignCollection = new DBCampaignCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CampaignLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    DBCampaign campaign = GetCampaignFromReader(dataReader);
                    campaignCollection.Add(campaign);
                }
            }
            return campaignCollection;
        }

        /// <summary>
        /// Inserts a campaign
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="Subject">The subject</param>
        /// <param name="Body">The body</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>Campaign</returns>
        public override DBCampaign InsertCampaign(string Name, string Subject, string Body, DateTime CreatedOn)
        {
            DBCampaign campaign = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CampaignInsert");
            db.AddOutParameter(dbCommand, "CampaignID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "Subject", DbType.String, Subject);
            db.AddInParameter(dbCommand, "Body", DbType.String, Body);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int CampaignID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@CampaignID"));
                campaign = GetCampaignByID(CampaignID);
            }
            return campaign;
        }

        /// <summary>
        /// Updates the campaign
        /// </summary>
        /// <param name="CampaignID">The campaign identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="Subject">The subject</param>
        /// <param name="Body">The body</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>Campaign</returns>
        public override DBCampaign UpdateCampaign(int CampaignID,
           string Name, string Subject, string Body, DateTime CreatedOn)
        {
            DBCampaign campaign = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CampaignUpdate");
            db.AddInParameter(dbCommand, "CampaignID", DbType.Int32, CampaignID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "Subject", DbType.String, Subject);
            db.AddInParameter(dbCommand, "Body", DbType.String, Body);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                campaign = GetCampaignByID(CampaignID);

            return campaign;
        }
        #endregion
    }
}

