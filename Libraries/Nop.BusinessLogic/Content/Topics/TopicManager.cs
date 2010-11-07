//------------------------------------------------------------------------------
// The contents of this file are title to the nopCommerce Public License Version 1.0 ("License"); you may not use this file except in compliance with the License.
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
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using NopSolutions.NopCommerce.BusinessLogic.Audit;
using NopSolutions.NopCommerce.BusinessLogic.Caching;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Localization;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.BusinessLogic.Utils;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Content.Topics;

namespace NopSolutions.NopCommerce.BusinessLogic.Content.Topics
{
    /// <summary>
    /// Message manager
    /// </summary>
    public partial class TopicManager
    {
        #region Utilities

        private static TopicCollection DBMapping(DBTopicCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            TopicCollection collection = new TopicCollection();
            foreach (DBTopic dbItem in dbCollection)
            {
                Topic item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static Topic DBMapping(DBTopic dbItem)
        {
            if (dbItem == null)
                return null;

            Topic item = new Topic();
            item.TopicID = dbItem.TopicID;
            item.Name = dbItem.Name;

            return item;
        }

        private static LocalizedTopicCollection DBMapping(DBLocalizedTopicCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            LocalizedTopicCollection collection = new LocalizedTopicCollection();
            foreach (DBLocalizedTopic dbItem in dbCollection)
            {
                LocalizedTopic item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static LocalizedTopic DBMapping(DBLocalizedTopic dbItem)
        {
            if (dbItem == null)
                return null;

            LocalizedTopic item = new LocalizedTopic();
            item.TopicLocalizedID = dbItem.TopicLocalizedID;
            item.TopicID = dbItem.TopicID;
            item.LanguageID = dbItem.LanguageID;
            item.Title = dbItem.Title;
            item.Body = dbItem.Body;
            item.CreatedOn = dbItem.CreatedOn;
            item.UpdatedOn = dbItem.UpdatedOn;

            return item;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a topic
        /// </summary>
        /// <param name="TopicID">Topic identifier</param>
        public static void DeleteTopic(int TopicID)
        {
            DBProviderManager<DBTopicProvider>.Provider.DeleteTopic(TopicID);
        }

        /// <summary>
        /// Inserts a topic
        /// </summary>
        /// <param name="Name">The name</param>
        /// <returns>Topic</returns>
        public static Topic InsertTopic(string Name)
        {
            DBTopic dbItem = DBProviderManager<DBTopicProvider>.Provider.InsertTopic(Name);
            Topic topic = DBMapping(dbItem);
            return topic;
        }

        /// <summary>
        /// Updates the topic
        /// </summary>
        /// <param name="TopicID">The topic identifier</param>
        /// <param name="Name">The name</param>
        /// <returns>Topic</returns>
        public static Topic UpdateTopic(int TopicID, string Name)
        {
            DBTopic dbItem = DBProviderManager<DBTopicProvider>.Provider.UpdateTopic(TopicID, Name);
            Topic topic = DBMapping(dbItem);
            return topic;
        }

        /// <summary>
        /// Gets a topic by template identifier
        /// </summary>
        /// <param name="TopicID">topic identifier</param>
        /// <returns>topic</returns>
        public static Topic GetTopicByID(int TopicID)
        {
            if (TopicID == 0)
                return null;

            DBTopic dbItem = DBProviderManager<DBTopicProvider>.Provider.GetTopicByID(TopicID);
            Topic Topic = DBMapping(dbItem);
            return Topic;
        }

        /// <summary>
        /// Gets all topics
        /// </summary>
        /// <returns>topic collection</returns>
        public static TopicCollection GetAllTopics()
        {
            DBTopicCollection dbCollection = DBProviderManager<DBTopicProvider>.Provider.GetAllTopics();
            TopicCollection collection = DBMapping(dbCollection);
            return collection;
        }

        /// <summary>
        /// Gets a localized topic by identifier
        /// </summary>
        /// <param name="LocalizedTopicID">Localized topic identifier</param>
        /// <returns>Localized topic</returns>
        public static LocalizedTopic GetLocalizedTopicByID(int LocalizedTopicID)
        {
            if (LocalizedTopicID == 0)
                return null;

            DBLocalizedTopic dbItem = DBProviderManager<DBTopicProvider>.Provider.GetLocalizedTopicByID(LocalizedTopicID);
            LocalizedTopic localizedTopic = DBMapping(dbItem);
            return localizedTopic;
        }

        /// <summary>
        /// Gets a localized topic by parent TopicID and language identifier
        /// </summary>
        /// <param name="TopicID">The topic identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Localized topic</returns>
        public static LocalizedTopic GetLocalizedTopic(int TopicID, int LanguageID)
        {
            DBLocalizedTopic dbItem = DBProviderManager<DBTopicProvider>.Provider.GetLocalizedTopic(TopicID, LanguageID);
            LocalizedTopic localizedTopic = DBMapping(dbItem);
            return localizedTopic;
        }
        
        /// <summary>
        /// Gets a localized topic by name and language identifier
        /// </summary>
        /// <param name="Name">topic name</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Localized topic</returns>
        public static LocalizedTopic GetLocalizedTopic(string Name, int LanguageID)
        {
            DBLocalizedTopic dbItem = DBProviderManager<DBTopicProvider>.Provider.GetLocalizedTopic(Name, LanguageID);
            LocalizedTopic localizedTopic = DBMapping(dbItem);
            return localizedTopic;
        }

        /// <summary>
        /// Deletes a localized topic
        /// </summary>
        /// <param name="LocalizedTopicID">topic identifier</param>
        public static void DeleteLocalizedTopic(int LocalizedTopicID)
        {
            DBProviderManager<DBTopicProvider>.Provider.DeleteLocalizedTopic(LocalizedTopicID);
        }

        /// <summary>
        /// Gets all localized topics
        /// </summary>
        /// <param name="TopicName">topic name</param>
        /// <returns>Localized topic collection</returns>
        public static LocalizedTopicCollection GetAllLocalizedTopics(string TopicName)
        {
            DBLocalizedTopicCollection dbCollection = DBProviderManager<DBTopicProvider>.Provider.GetAllLocalizedTopics(TopicName);
            LocalizedTopicCollection localizedTopics = DBMapping(dbCollection);
            return localizedTopics;
        }

        /// <summary>
        /// Inserts a localized topic
        /// </summary>
        /// <param name="TopicID">The topic identifier</param>
        /// <param name="LanguageID">The language identifier</param>
        /// <param name="Title">The title</param>
        /// <param name="Body">The body</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Localized topic</returns>
        public static LocalizedTopic InsertLocalizedTopic(int TopicID,
            int LanguageID, string Title, string Body, 
            DateTime CreatedOn, DateTime UpdatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            UpdatedOn = DateTimeHelper.ConvertToUtcTime(UpdatedOn);

            DBLocalizedTopic dbItem = DBProviderManager<DBTopicProvider>.Provider.InsertLocalizedTopic(TopicID,
                LanguageID, Title, Body, CreatedOn,UpdatedOn);
            LocalizedTopic localizedTopic = DBMapping(dbItem);
            return localizedTopic;
        }

        /// <summary>
        /// Updates the localized topic
        /// </summary>
        /// <param name="TopicLocalizedID">The localized topic identifier</param>
        /// <param name="TopicID">The topic identifier</param>
        /// <param name="LanguageID">The language identifier</param>
        /// <param name="Title">The title</param>
        /// <param name="Body">The body</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Localized topic</returns>
        public static LocalizedTopic UpdateLocalizedTopic(int TopicLocalizedID,
            int TopicID, int LanguageID,
            string Title, string Body, 
            DateTime CreatedOn, DateTime UpdatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            UpdatedOn = DateTimeHelper.ConvertToUtcTime(UpdatedOn);

            DBLocalizedTopic dbItem = DBProviderManager<DBTopicProvider>.Provider.UpdateLocalizedTopic(TopicLocalizedID,
                TopicID, LanguageID, Title, Body, CreatedOn, UpdatedOn);
            LocalizedTopic localizedTopic = DBMapping(dbItem);
            return localizedTopic;
        }

        #endregion
    }
}