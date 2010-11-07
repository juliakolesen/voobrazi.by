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
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Configuration.Provider;
using System.Configuration;
using System.Web.Hosting;
using System.Collections.Specialized;
using System.Web.Configuration;

namespace NopSolutions.NopCommerce.DataAccess.Content.Topics
{
    /// <summary>
    /// Acts as a base class for deriving custom topic provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/TopicProvider")]
    public abstract partial class DBTopicProvider : BaseDBProvider
    {
        #region Methods

        /// <summary>
        /// Deletes a topic
        /// </summary>
        /// <param name="TopicID">Topic identifier</param>
        public abstract void DeleteTopic(int TopicID);

        /// <summary>
        /// Inserts a topic
        /// </summary>
        /// <param name="Name">The name</param>
        /// <returns>Topic</returns>
        public abstract DBTopic InsertTopic(string Name);

        /// <summary>
        /// Updates the topic
        /// </summary>
        /// <param name="TopicID">The topic identifier</param>
        /// <param name="Name">The name</param>
        /// <returns>Topic</returns>
        public abstract DBTopic UpdateTopic(int TopicID, string Name);

        /// <summary>
        /// Gets a topic by template identifier
        /// </summary>
        /// <param name="TopicID">Topic identifier</param>
        /// <returns>Topic</returns>
        public abstract DBTopic GetTopicByID(int TopicID);

        /// <summary>
        /// Gets all topics
        /// </summary>
        /// <returns>Topic collection</returns>
        public abstract DBTopicCollection GetAllTopics();

        /// <summary>
        /// Gets a localized topic by identifier
        /// </summary>
        /// <param name="LocalizedTopicID">Localized topic identifier</param>
        /// <returns>Localized topic</returns>
        public abstract DBLocalizedTopic GetLocalizedTopicByID(int LocalizedTopicID);

        /// <summary>
        /// Gets a localized topic by parent TopicID and language identifier
        /// </summary>
        /// <param name="TopicID">The topic identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Localized topic</returns>
        public abstract DBLocalizedTopic GetLocalizedTopic(int TopicID, int LanguageID);
        
        /// <summary>
        /// Gets a localized topic by name and language identifier
        /// </summary>
        /// <param name="Name">Topic name</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Localized topic</returns>
        public abstract DBLocalizedTopic GetLocalizedTopic(string Name, int LanguageID);

        /// <summary>
        /// Deletes a localized topic
        /// </summary>
        /// <param name="LocalizedTopicID">Topic identifier</param>
        public abstract void DeleteLocalizedTopic(int LocalizedTopicID);

        /// <summary>
        /// Gets all localized topics
        /// </summary>
        /// <param name="TopicName">Topic name</param>
        /// <returns>Localized topic collection</returns>
        public abstract DBLocalizedTopicCollection GetAllLocalizedTopics(string TopicName);

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
        public abstract DBLocalizedTopic InsertLocalizedTopic(int TopicID,
            int LanguageID, string Title, string Body,
            DateTime CreatedOn, DateTime UpdatedOn);

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
        public abstract DBLocalizedTopic UpdateLocalizedTopic(int TopicLocalizedID,
            int TopicID, int LanguageID,
            string Title, string Body,
            DateTime CreatedOn, DateTime UpdatedOn);

        #endregion
    }
}

