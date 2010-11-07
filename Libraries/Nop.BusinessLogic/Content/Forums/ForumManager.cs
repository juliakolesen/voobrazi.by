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
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Web;
using NopSolutions.NopCommerce.BusinessLogic.Caching;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Messages;
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.BusinessLogic.Utils.Html;
using NopSolutions.NopCommerce.Common;
using NopSolutions.NopCommerce.Common.Utils.Html;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Content.Forums;

namespace NopSolutions.NopCommerce.BusinessLogic.Content.Forums
{
    /// <summary>
    /// Forum manager
    /// </summary>
    public partial class ForumManager
    {
        #region Constants
        private const string FORUMGROUP_ALL_KEY = "Nop.forumgroup.all";
        private const string FORUMGROUP_BY_ID_KEY = "Nop.forumgroup.id-{0}";
        private const string FORUM_ALLBYFORUMGROUPID_KEY = "Nop.forum.allbyforumgroupid-{0}";
        private const string FORUM_BY_ID_KEY = "Nop.forum.id-{0}";
        private const string FORUMGROUP_PATTERN_KEY = "Nop.forumgroup.";
        private const string FORUM_PATTERN_KEY = "Nop.forum.";
        #endregion

        #region Utilities
        private static ForumGroupCollection DBMapping(DBForumGroupCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            ForumGroupCollection collection = new ForumGroupCollection();
            foreach (DBForumGroup dbItem in dbCollection)
            {
                ForumGroup item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ForumGroup DBMapping(DBForumGroup dbItem)
        {
            if (dbItem == null)
                return null;

            ForumGroup item = new ForumGroup();
            item.ForumGroupID = dbItem.ForumGroupID;
            item.Name = dbItem.Name;
            item.Description = dbItem.Description;
            item.DisplayOrder = dbItem.DisplayOrder;
            item.CreatedOn = dbItem.CreatedOn;
            item.UpdatedOn = dbItem.UpdatedOn;

            return item;
        }

        private static ForumCollection DBMapping(DBForumCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            ForumCollection collection = new ForumCollection();
            foreach (DBForum dbItem in dbCollection)
            {
                Forum item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static Forum DBMapping(DBForum dbItem)
        {
            if (dbItem == null)
                return null;

            Forum item = new Forum();
            item.ForumID = dbItem.ForumID;
            item.ForumGroupID = dbItem.ForumGroupID;
            item.Name = dbItem.Name;
            item.Description = dbItem.Description;
            item.NumTopics = dbItem.NumTopics;
            item.NumPosts = dbItem.NumPosts;
            item.LastTopicID = dbItem.LastTopicID;
            item.LastPostID = dbItem.LastPostID;
            item.LastPostUserID = dbItem.LastPostUserID;
            item.LastPostTime = dbItem.LastPostTime;
            item.DisplayOrder = dbItem.DisplayOrder;
            item.CreatedOn = dbItem.CreatedOn;
            item.UpdatedOn = dbItem.UpdatedOn;

            return item;
        }

        private static ForumTopicCollection DBMapping(DBForumTopicCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            ForumTopicCollection collection = new ForumTopicCollection();
            foreach (DBForumTopic dbItem in dbCollection)
            {
                ForumTopic item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ForumTopic DBMapping(DBForumTopic dbItem)
        {
            if (dbItem == null)
                return null;

            ForumTopic item = new ForumTopic();
            item.ForumTopicID = dbItem.ForumTopicID;
            item.ForumID = dbItem.ForumID;
            item.UserID = dbItem.UserID;
            item.TopicTypeID = dbItem.TopicTypeID;
            item.Subject = dbItem.Subject;
            item.NumPosts = dbItem.NumPosts;
            item.Views = dbItem.Views;
            item.LastPostID = dbItem.LastPostID;
            item.LastPostUserID = dbItem.LastPostUserID;
            item.LastPostTime = dbItem.LastPostTime;
            item.CreatedOn = dbItem.CreatedOn;
            item.UpdatedOn = dbItem.UpdatedOn;

            return item;
        }

        private static ForumPostCollection DBMapping(DBForumPostCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            ForumPostCollection collection = new ForumPostCollection();
            foreach (DBForumPost dbItem in dbCollection)
            {
                ForumPost item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ForumPost DBMapping(DBForumPost dbItem)
        {
            if (dbItem == null)
                return null;

            ForumPost item = new ForumPost();
            item.ForumPostID = dbItem.ForumPostID;
            item.TopicID = dbItem.TopicID;
            item.UserID = dbItem.UserID;
            item.Text = dbItem.Text;
            item.IPAddress = dbItem.IPAddress;
            item.CreatedOn = dbItem.CreatedOn;
            item.UpdatedOn = dbItem.UpdatedOn;

            return item;
        }

        private static PrivateMessageCollection DBMapping(DBPrivateMessageCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            PrivateMessageCollection collection = new PrivateMessageCollection();
            foreach (DBPrivateMessage dbItem in dbCollection)
            {
                PrivateMessage item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static PrivateMessage DBMapping(DBPrivateMessage dbItem)
        {
            if (dbItem == null)
                return null;

            PrivateMessage item = new PrivateMessage();
            item.PrivateMessageID = dbItem.PrivateMessageID;
            item.FromUserID = dbItem.FromUserID;
            item.ToUserID = dbItem.ToUserID;
            item.Subject = dbItem.Subject;
            item.Text = dbItem.Text;
            item.IsRead = dbItem.IsRead;
            item.IsDeletedByAuthor = dbItem.IsDeletedByAuthor;
            item.IsDeletedByRecipient = dbItem.IsDeletedByRecipient;
            item.CreatedOn = dbItem.CreatedOn;

            return item;
        }

        private static ForumSubscriptionCollection DBMapping(DBForumSubscriptionCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            ForumSubscriptionCollection collection = new ForumSubscriptionCollection();
            foreach (DBForumSubscription dbItem in dbCollection)
            {
                ForumSubscription item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ForumSubscription DBMapping(DBForumSubscription dbItem)
        {
            if (dbItem == null)
                return null;

            ForumSubscription item = new ForumSubscription();
            item.ForumSubscriptionID = dbItem.ForumSubscriptionID;
            item.SubscriptionGUID = dbItem.SubscriptionGUID;
            item.UserID = dbItem.UserID;
            item.ForumID = dbItem.ForumID;
            item.TopicID = dbItem.TopicID;
            item.CreatedOn = dbItem.CreatedOn;

            return item;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Deletes a forum group
        /// </summary>
        /// <param name="ForumGroupID">The forum group identifier</param>
        public static void DeleteForumGroup(int ForumGroupID)
        {
            DBProviderManager<DBForumProvider>.Provider.DeleteForumGroup(ForumGroupID);

            if (ForumManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(FORUMGROUP_PATTERN_KEY);
                NopCache.RemoveByPattern(FORUM_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets a forum group
        /// </summary>
        /// <param name="ForumGroupID">The forum group identifier</param>
        /// <returns>Forum group</returns>
        public static ForumGroup GetForumGroupByID(int ForumGroupID)
        {
            if (ForumGroupID == 0)
                return null;

            string key = string.Format(FORUMGROUP_BY_ID_KEY, ForumGroupID);
            object obj2 = NopCache.Get(key);
            if (ForumManager.CacheEnabled && (obj2 != null))
            {
                return (ForumGroup)obj2;
            }
            DBForumGroup dbItem = DBProviderManager<DBForumProvider>.Provider.GetForumGroupByID(ForumGroupID);
            ForumGroup forumGroup = DBMapping(dbItem);

            if (ForumManager.CacheEnabled)
            {
                NopCache.Max(key, forumGroup);
            }
            return forumGroup;
        }

        /// <summary>
        /// Gets all forum groups
        /// </summary>
        /// <returns>Forum groups</returns>
        public static ForumGroupCollection GetAllForumGroups()
        {
            string key = string.Format(FORUMGROUP_ALL_KEY);
            object obj2 = NopCache.Get(key);
            if (ForumManager.CacheEnabled && (obj2 != null))
            {
                return (ForumGroupCollection)obj2;
            }
            DBForumGroupCollection dbCollection = DBProviderManager<DBForumProvider>.Provider.GetAllForumGroups();
            ForumGroupCollection forumGroupCollection = DBMapping(dbCollection);

            if (ForumManager.CacheEnabled)
            {
                NopCache.Max(key, forumGroupCollection);
            }
            return forumGroupCollection;
        }

        /// <summary>
        /// Inserts a forum group
        /// </summary>
        /// <param name="Name">The language name</param>
        /// <param name="Description">The description</param>
        /// <param name="DisplayOrder">The display order</param>        
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Forum group</returns>
        public static ForumGroup InsertForumGroup(string Name, string Description,
            int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            UpdatedOn = DateTimeHelper.ConvertToUtcTime(UpdatedOn);

            DBForumGroup dbItem = DBProviderManager<DBForumProvider>.Provider.InsertForumGroup(Name, Description,
            DisplayOrder, CreatedOn, UpdatedOn);

            ForumGroup forumGroup = DBMapping(dbItem);

            if (ForumManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(FORUMGROUP_PATTERN_KEY);
                NopCache.RemoveByPattern(FORUM_PATTERN_KEY);
            }

            return forumGroup;
        }

        /// <summary>
        /// Updates the forum group
        /// </summary>
        /// <param name="ForumGroupID">The forum group identifier</param>
        /// <param name="Name">The language name</param>
        /// <param name="Description">The description</param>
        /// <param name="DisplayOrder">The display order</param>        
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Forum group</returns>
        public static ForumGroup UpdateForumGroup(int ForumGroupID, string Name, string Description,
            int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            UpdatedOn = DateTimeHelper.ConvertToUtcTime(UpdatedOn);

            DBForumGroup dbItem = DBProviderManager<DBForumProvider>.Provider.UpdateForumGroup(ForumGroupID, 
                Name, Description, DisplayOrder, CreatedOn, UpdatedOn);

            ForumGroup forumGroup = DBMapping(dbItem);

            if (ForumManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(FORUMGROUP_PATTERN_KEY);
                NopCache.RemoveByPattern(FORUM_PATTERN_KEY);
            }

            return forumGroup;
        }

        /// <summary>
        /// Deletes a forum
        /// </summary>
        /// <param name="ForumID">The forum identifier</param>
        public static void DeleteForum(int ForumID)
        {
            DBProviderManager<DBForumProvider>.Provider.DeleteForum(ForumID);

            if (ForumManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(FORUMGROUP_PATTERN_KEY);
                NopCache.RemoveByPattern(FORUM_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets a forum
        /// </summary>
        /// <param name="ForumID">The forum identifier</param>
        /// <returns>Forum</returns>
        public static Forum GetForumByID(int ForumID)
        {
            if (ForumID == 0)
                return null;

            string key = string.Format(FORUM_BY_ID_KEY, ForumID);
            object obj2 = NopCache.Get(key);
            if (ForumManager.CacheEnabled && (obj2 != null))
            {
                return (Forum)obj2;
            }
            DBForum dbItem = DBProviderManager<DBForumProvider>.Provider.GetForumByID(ForumID);
            Forum forum = DBMapping(dbItem);

            if (ForumManager.CacheEnabled)
            {
                NopCache.Max(key, forum);
            }
            return forum;
        }

        /// <summary>
        /// Gets forums by group identifier
        /// </summary>
        /// <param name="ForumGroupID">The forum group identifier</param>
        /// <returns>Forums</returns>
        public static ForumCollection GetAllForumsByGroupID(int ForumGroupID)
        {
            string key = string.Format(FORUM_ALLBYFORUMGROUPID_KEY, ForumGroupID);
            object obj2 = NopCache.Get(key);
            if (ForumManager.CacheEnabled && (obj2 != null))
            {
                return (ForumCollection)obj2;
            }

            DBForumCollection dbCollection = DBProviderManager<DBForumProvider>.Provider.GetAllForumsByGroupID(ForumGroupID);
            ForumCollection forumCollection = DBMapping(dbCollection);

            if (ForumManager.CacheEnabled)
            {
                NopCache.Max(key, forumCollection);
            }
            return forumCollection;
        }

        /// <summary>
        /// Inserts a forum
        /// </summary>
        /// <param name="ForumGroupID">The forum group identifier</param>
        /// <param name="Name">The language name</param>
        /// <param name="Description">The description</param>
        /// <param name="NumTopics">The number of topics</param>
        /// <param name="NumPosts">The number of posts</param>
        /// <param name="LastTopicID">The last topic identifier</param>
        /// <param name="LastPostID">The last post identifier</param>
        /// <param name="LastPostUserID">The last post user identifier</param>
        /// <param name="LastPostTime">The last post date and time</param>
        /// <param name="DisplayOrder">The display order</param>        
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Forum</returns>
        public static Forum InsertForum(int ForumGroupID,
            string Name, string Description,
            int NumTopics, int NumPosts, int LastTopicID, int LastPostID,
            int LastPostUserID, DateTime? LastPostTime, int DisplayOrder,
            DateTime CreatedOn, DateTime UpdatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            UpdatedOn = DateTimeHelper.ConvertToUtcTime(UpdatedOn);

            DBForum dbItem = DBProviderManager<DBForumProvider>.Provider.InsertForum(ForumGroupID,
            Name, Description, NumTopics, NumPosts, LastTopicID, LastPostID,
            LastPostUserID, LastPostTime, DisplayOrder, CreatedOn, UpdatedOn);

            Forum forum = DBMapping(dbItem);

            if (ForumManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(FORUMGROUP_PATTERN_KEY);
                NopCache.RemoveByPattern(FORUM_PATTERN_KEY);
            }

            return forum;
        }

        /// <summary>
        /// Updates the forum
        /// </summary>
        /// <param name="ForumID">The forum identifier</param>
        /// <param name="ForumGroupID">The forum group identifier</param>
        /// <param name="Name">The language name</param>
        /// <param name="Description">The description</param>
        /// <param name="NumTopics">The number of topics</param>
        /// <param name="NumPosts">The number of posts</param>
        /// <param name="LastTopicID">The last topic identifier</param>
        /// <param name="LastPostID">The last post identifier</param>
        /// <param name="LastPostUserID">The last post user identifier</param>
        /// <param name="LastPostTime">The last post date and time</param>
        /// <param name="DisplayOrder">The display order</param>        
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Forum</returns>
        public static Forum UpdateForum(int ForumID, int ForumGroupID,
            string Name, string Description,
            int NumTopics, int NumPosts, int LastTopicID, int LastPostID,
            int LastPostUserID, DateTime? LastPostTime, int DisplayOrder,
            DateTime CreatedOn, DateTime UpdatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            UpdatedOn = DateTimeHelper.ConvertToUtcTime(UpdatedOn);

            DBForum dbItem = DBProviderManager<DBForumProvider>.Provider.UpdateForum(ForumID, ForumGroupID,
            Name, Description, NumTopics, NumPosts, LastTopicID, LastPostID,
            LastPostUserID, LastPostTime, DisplayOrder, CreatedOn, UpdatedOn);

            Forum forum = DBMapping(dbItem);

            if (ForumManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(FORUMGROUP_PATTERN_KEY);
                NopCache.RemoveByPattern(FORUM_PATTERN_KEY);
            }

            return forum;
        }

        /// <summary>
        /// Update forum stats
        /// </summary>
        /// <param name="ForumID">The forum identifier</param>
        /// <returns>Forum</returns>
        public static void UpdateForumStats(int ForumID)
        {
            if (ForumID == 0)
                return;

            DBProviderManager<DBForumProvider>.Provider.UpdateForumStats(ForumID);

            if (ForumManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(FORUMGROUP_PATTERN_KEY);
                NopCache.RemoveByPattern(FORUM_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Deletes a topic
        /// </summary>
        /// <param name="ForumTopicID">The topic identifier</param>
        public static void DeleteTopic(int ForumTopicID)
        {
            DBProviderManager<DBForumProvider>.Provider.DeleteTopic(ForumTopicID);

            if (ForumManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(FORUMGROUP_PATTERN_KEY);
                NopCache.RemoveByPattern(FORUM_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets a topic
        /// </summary>
        /// <param name="ForumTopicID">The topic identifier</param>
        /// <returns>Topic</returns>
        public static ForumTopic GetTopicByID(int ForumTopicID)
        {
            return GetTopicByID(ForumTopicID, false);
        }

        /// <summary>
        /// Gets a topic
        /// </summary>
        /// <param name="ForumTopicID">The topic identifier</param>
        /// <param name="IncreaseViews">The value indicating whether to increase topic views</param>
        /// <returns>Topic</returns>
        public static ForumTopic GetTopicByID(int ForumTopicID, bool IncreaseViews)
        {
            if (ForumTopicID == 0)
                return null;

            DBForumTopic dbItem = DBProviderManager<DBForumProvider>.Provider.GetTopicByID(ForumTopicID, IncreaseViews);
            ForumTopic forumTopic = DBMapping(dbItem);

            return forumTopic;
        }

        /// <summary>
        /// Gets all topics
        /// </summary>
        /// <param name="ForumID">The forum group identifier</param>
        /// <param name="UserID">The user identifier</param>
        /// <param name="Keywords">Keywords</param>
        /// <param name="SearchPosts">A value indicating whether to search in posts</param>
        /// <param name="PageSize">Page size</param>
        /// <param name="PageIndex">Page index</param>
        /// <param name="TotalRecords">Total records</param>
        /// <returns>Topics</returns>
        public static ForumTopicCollection GetAllTopics(int ForumID, int UserID, string Keywords,
            bool SearchPosts, int PageSize, int PageIndex, out int TotalRecords)
        {
            if (PageSize <= 0)
                PageSize = 10;
            if (PageSize == int.MaxValue)
                PageSize = int.MaxValue - 1;

            if (PageIndex < 0)
                PageIndex = 0;
            if (PageIndex == int.MaxValue)
                PageIndex = int.MaxValue - 1;

            DBForumTopicCollection dbCollection = DBProviderManager<DBForumProvider>.Provider.GetAllTopics(ForumID,
                UserID, Keywords, SearchPosts, PageSize, PageIndex, out  TotalRecords);
            ForumTopicCollection forumTopicCollection = DBMapping(dbCollection);

            return forumTopicCollection;
        }
        
        /// <summary>
        /// Gets active topics
        /// </summary>
        /// <param name="ForumID">The forum group identifier</param>
        /// <returns>Topics</returns>
        public static ForumTopicCollection GetActiveTopics(int ForumID)
        {
            int topicCount = SettingManager.GetSettingValueInteger("Forums.ActiveDiscussions.TopicCount");

            return GetActiveTopics(ForumID, topicCount);
        }

        /// <summary>
        /// Gets active topics
        /// </summary>
        /// <param name="ForumID">The forum group identifier</param>
        /// <param name="TopicCount">Topic count. 0 if you want to get all topics</param>
        /// <returns>Topics</returns>
        public static ForumTopicCollection GetActiveTopics(int ForumID, int TopicCount)
        {
            DBForumTopicCollection dbCollection = DBProviderManager<DBForumProvider>.Provider.GetActiveTopics(ForumID, TopicCount);
            ForumTopicCollection forumTopicCollection = DBMapping(dbCollection);

            return forumTopicCollection;
        }
        
        /// <summary>
        /// Inserts a topic
        /// </summary>
        /// <param name="ForumID">The forum identifier</param>
        /// <param name="UserID">The user identifier</param>
        /// <param name="TopicType">The topic type</param>
        /// <param name="Subject">The subject</param>
        /// <param name="NumPosts">The number of posts</param>
        /// <param name="Views">The number of views</param>
        /// <param name="LastPostID">The last post identifier</param>
        /// <param name="LastPostUserID">The last post user identifier</param>
        /// <param name="LastPostTime">The last post date and time</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <param name="SendNotifications">A value indicating whether to send notifications to users</param>
        /// <returns>Topic</returns>
        public static ForumTopic InsertTopic(int ForumID, int UserID,
            ForumTopicTypeEnum TopicType, string Subject,
            int NumPosts, int Views, int LastPostID,
            int LastPostUserID, DateTime? LastPostTime,
            DateTime CreatedOn, DateTime UpdatedOn, bool SendNotifications)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            UpdatedOn = DateTimeHelper.ConvertToUtcTime(UpdatedOn);

            if (Subject == null)
                Subject = string.Empty;

            Subject = Subject.Trim();

            if (String.IsNullOrEmpty(Subject))
                throw new NopException("Topic subject cannot be empty");

            if (ForumManager.TopicSubjectMaxLength > 0)
            {
                if (Subject.Length > ForumManager.TopicSubjectMaxLength)
                    Subject = Subject.Substring(0, ForumManager.TopicSubjectMaxLength);
            }

            DBForumTopic dbItem = DBProviderManager<DBForumProvider>.Provider.InsertTopic(ForumID, UserID,
                (int)TopicType, Subject, NumPosts, Views, LastPostID,
                LastPostUserID, LastPostTime, CreatedOn, UpdatedOn);

            ForumTopic forumTopic = DBMapping(dbItem);

            if (ForumManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(FORUMGROUP_PATTERN_KEY);
                NopCache.RemoveByPattern(FORUM_PATTERN_KEY);
            }

            if (SendNotifications)
            {
                Forum forum = forumTopic.Forum;
                ForumSubscriptionCollection subscriptions = GetAllSubscriptions(0, forum.ForumID, 0, int.MaxValue, 0);
                
                foreach (ForumSubscription subscription in subscriptions)
                {
                    if (subscription.UserID == UserID)
                        continue;

                    MessageManager.SendNewForumTopicMessage(subscription.User, forumTopic, forum, NopContext.Current.WorkingLanguage.LanguageID);
                }
            }

            return forumTopic;
        }

        /// <summary>
        /// Updates the topic
        /// </summary>
        /// <param name="ForumTopicID">The forum topic identifier</param>
        /// <param name="ForumID">The forum identifier</param>
        /// <param name="UserID">The user identifier</param>
        /// <param name="TopicType">The topic type</param>
        /// <param name="Subject">The subject</param>
        /// <param name="NumPosts">The number of posts</param>
        /// <param name="Views">The number of views</param>
        /// <param name="LastPostID">The last post identifier</param>
        /// <param name="LastPostUserID">The last post user identifier</param>
        /// <param name="LastPostTime">The last post date and time</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Topic</returns>
        public static ForumTopic UpdateTopic(int ForumTopicID, int ForumID, int UserID,
            ForumTopicTypeEnum TopicType, string Subject,
            int NumPosts, int Views, int LastPostID,
            int LastPostUserID, DateTime? LastPostTime,
            DateTime CreatedOn, DateTime UpdatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            UpdatedOn = DateTimeHelper.ConvertToUtcTime(UpdatedOn);

            if (Subject == null)
                Subject = string.Empty;

            Subject = Subject.Trim();

            if (String.IsNullOrEmpty(Subject))
                throw new NopException("Topic subject cannot be empty");

            if (ForumManager.TopicSubjectMaxLength > 0)
            {
                if (Subject.Length > ForumManager.TopicSubjectMaxLength)
                    Subject = Subject.Substring(0, ForumManager.TopicSubjectMaxLength);
            }

            DBForumTopic dbItem = DBProviderManager<DBForumProvider>.Provider.UpdateTopic(ForumTopicID, ForumID, UserID,
                (int)TopicType, Subject, NumPosts, Views, LastPostID,
                LastPostUserID, LastPostTime, CreatedOn, UpdatedOn);

            ForumTopic forumTopic = DBMapping(dbItem);

            if (ForumManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(FORUMGROUP_PATTERN_KEY);
                NopCache.RemoveByPattern(FORUM_PATTERN_KEY);
            }

            return forumTopic;
        }

        /// <summary>
        /// Moves the topic
        /// </summary>
        /// <param name="ForumTopicID">The forum topic identifier</param>
        /// <param name="NewForumID">New forum identifier</param>
        /// <returns>Moved topic</returns>
        public static ForumTopic MoveTopic(int ForumTopicID, int NewForumID)
        {
            ForumTopic forumTopic = GetTopicByID(ForumTopicID);
            if (forumTopic == null)
                return forumTopic;

            if (ForumManager.IsUserAllowedToMoveTopic(NopContext.Current.User, forumTopic))
            {
                int previousForumID = forumTopic.ForumID;
                Forum newForum = GetForumByID(NewForumID);

                if (newForum != null)
                {
                    if (previousForumID != NewForumID)
                    {
                        forumTopic = UpdateTopic(forumTopic.ForumTopicID, newForum.ForumID,
                            forumTopic.UserID, forumTopic.TopicType, forumTopic.Subject, forumTopic.NumPosts,
                            forumTopic.Views, forumTopic.LastPostID, forumTopic.LastPostUserID,
                            forumTopic.LastPostTime, forumTopic.CreatedOn, DateTime.Now);

                        //update forum stats
                        UpdateForumStats(previousForumID);
                        UpdateForumStats(NewForumID);
                    }
                }
            }
            return forumTopic;
        }

        /// <summary>
        /// Deletes a post
        /// </summary>
        /// <param name="ForumPostID">The post identifier</param>
        public static void DeletePost(int ForumPostID)
        {
            ForumPost forumPost = ForumManager.GetPostByID(ForumPostID);
            int ForumTopicID = 0;
            if (forumPost != null)
            {
                ForumTopicID = forumPost.TopicID;
            }
             
            //delete topic if it was the first post
            ForumTopic forumTopic = ForumManager.GetTopicByID(ForumTopicID);
            if (forumTopic != null)
            {
                ForumPost firstPost = forumTopic.FirstPost;
                if (firstPost != null && firstPost.ForumPostID == ForumPostID)
                {
                    ForumManager.DeleteTopic(forumTopic.ForumTopicID);
                }
            }

            DBProviderManager<DBForumProvider>.Provider.DeletePost(ForumPostID);
          

            if (ForumManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(FORUMGROUP_PATTERN_KEY);
                NopCache.RemoveByPattern(FORUM_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets a post
        /// </summary>
        /// <param name="ForumPostID">The post identifier</param>
        /// <returns>Post</returns>
        public static ForumPost GetPostByID(int ForumPostID)
        {
            if (ForumPostID == 0)
                return null;

            DBForumPost dbItem = DBProviderManager<DBForumProvider>.Provider.GetPostByID(ForumPostID);
            ForumPost forumPost = DBMapping(dbItem);

            return forumPost;
        }

        /// <summary>
        /// Gets all posts
        /// </summary>
        /// <param name="ForumTopicID">The forum topic identifier</param>
        /// <param name="UserID">The user identifier</param>
        /// <param name="Keywords">Keywords</param>
        /// <param name="PageSize">Page size</param>
        /// <param name="PageIndex">Page index</param>
        /// <param name="TotalRecords">Total records</param>
        /// <returns>Posts</returns>
        public static ForumPostCollection GetAllPosts(int ForumTopicID, int UserID, string Keywords,
            int PageSize, int PageIndex, out int TotalRecords)
        {
            return GetAllPosts(ForumTopicID, UserID, Keywords, true,
                PageSize, PageIndex, out  TotalRecords);
        }

        /// <summary>
        /// Gets all posts
        /// </summary>
        /// <param name="ForumTopicID">The forum topic identifier</param>
        /// <param name="UserID">The user identifier</param>
        /// <param name="Keywords">Keywords</param>
        /// <param name="AscSort">Sort order</param>
        /// <param name="PageSize">Page size</param>
        /// <param name="PageIndex">Page index</param>
        /// <param name="TotalRecords">Total records</param>
        /// <returns>Posts</returns>
        public static ForumPostCollection GetAllPosts(int ForumTopicID, int UserID, string Keywords,
            bool AscSort, int PageSize, int PageIndex, out int TotalRecords)
        {
            if (PageSize <= 0)
                PageSize = 10;
            if (PageSize == int.MaxValue)
                PageSize = int.MaxValue - 1;

            if (PageIndex < 0)
                PageIndex = 0;
            if (PageIndex == int.MaxValue)
                PageIndex = int.MaxValue - 1;

            DBForumPostCollection dbCollection = DBProviderManager<DBForumProvider>.Provider.GetAllPosts(ForumTopicID,
                UserID, Keywords, AscSort, PageSize, PageIndex, out TotalRecords);
            ForumPostCollection forumPostCollection = DBMapping(dbCollection);

            return forumPostCollection;
        }

        /// <summary>
        /// Inserts a post
        /// </summary>
        /// <param name="ForumTopicID">The forum topic identifier</param>
        /// <param name="UserID">The user identifier</param>
        /// <param name="Text">The text</param>
        /// <param name="IPAddress">The IP address</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <param name="SendNotifications">A value indicating whether to send notifications to users</param>
        /// <returns>Post</returns>
        public static ForumPost InsertPost(int ForumTopicID, int UserID,
            string Text, string IPAddress, DateTime CreatedOn, DateTime UpdatedOn, bool SendNotifications)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            UpdatedOn = DateTimeHelper.ConvertToUtcTime(UpdatedOn);

            if (Text == null)
                Text = string.Empty;

            Text = Text.Trim();

            if (String.IsNullOrEmpty(Text))
                throw new NopException("Text cannot be empty");

            if (ForumManager.PostMaxLength > 0)
            {
                if (Text.Length > ForumManager.PostMaxLength)
                    Text = Text.Substring(0, ForumManager.PostMaxLength);
            }

            DBForumPost dbItem = DBProviderManager<DBForumProvider>.Provider.InsertPost(ForumTopicID, UserID,
            Text, IPAddress, CreatedOn, UpdatedOn);

            ForumPost forumPost = DBMapping(dbItem);

            if (ForumManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(FORUMGROUP_PATTERN_KEY);
                NopCache.RemoveByPattern(FORUM_PATTERN_KEY);
            }

            if (SendNotifications)
            {
                ForumTopic forumTopic = forumPost.Topic;
                Forum forum = forumTopic.Forum;
                ForumSubscriptionCollection subscriptions = GetAllSubscriptions(0, 0, forumTopic.ForumTopicID, int.MaxValue, 0);
                
                foreach (ForumSubscription subscription in subscriptions)
                {
                    if (subscription.UserID == UserID)
                        continue;

                    MessageManager.SendNewForumPostMessage(subscription.User, forumTopic, forum, NopContext.Current.WorkingLanguage.LanguageID);
                }
            }

            return forumPost;
        }

        /// <summary>
        /// Updates the post
        /// </summary>
        /// <param name="ForumPostID">The forum post identifier</param>
        /// <param name="ForumTopicID">The forum topic identifier</param>
        /// <param name="UserID">The user identifier</param>
        /// <param name="Text">The text</param>
        /// <param name="IPAddress">The IP address</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Post</returns>
        public static ForumPost UpdatePost(int ForumPostID, int ForumTopicID, int UserID,
            string Text, string IPAddress, DateTime CreatedOn, DateTime UpdatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            UpdatedOn = DateTimeHelper.ConvertToUtcTime(UpdatedOn);

            if (Text == null)
                Text = string.Empty;

            Text = Text.Trim();

            if (String.IsNullOrEmpty(Text))
                throw new NopException("Text cannot be empty");

            if (ForumManager.PostMaxLength > 0)
            {
                if (Text.Length > ForumManager.PostMaxLength)
                    Text = Text.Substring(0, ForumManager.PostMaxLength);
            }
            DBForumPost dbItem = DBProviderManager<DBForumProvider>.Provider.UpdatePost(ForumPostID, ForumTopicID, UserID,
            Text, IPAddress, CreatedOn, UpdatedOn);

            ForumPost forumPost = DBMapping(dbItem);

            if (ForumManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(FORUMGROUP_PATTERN_KEY);
                NopCache.RemoveByPattern(FORUM_PATTERN_KEY);
            }

            return forumPost;
        }

        /// <summary>
        /// Deletes a private message
        /// </summary>
        /// <param name="ForumPrivateMessageID">The private message identifier</param>
        public static void DeletePrivateMessage(int ForumPrivateMessageID)
        {
            DBProviderManager<DBForumProvider>.Provider.DeletePrivateMessage(ForumPrivateMessageID);
        }

        /// <summary>
        /// Gets a private message
        /// </summary>
        /// <param name="ForumPrivateMessageID">The private message identifier</param>
        /// <returns>Private message</returns>
        public static PrivateMessage GetPrivateMessageByID(int ForumPrivateMessageID)
        {
            if (ForumPrivateMessageID == 0)
                return null;

            DBPrivateMessage dbItem = DBProviderManager<DBForumProvider>.Provider.GetPrivateMessageByID(ForumPrivateMessageID);
            PrivateMessage privateMessage = DBMapping(dbItem);

            return privateMessage;
        }

        /// <summary>
        /// Gets private messages
        /// </summary>
        /// <param name="FromUserID">The user identifier who sent the message</param>
        /// <param name="ToUserID">The user identifier who should receive the message</param>
        /// <param name="IsRead">A value indicating whether loaded messages are read. false - to load not read messages only, true - to load read messages only, null to load all messages</param>
        /// <param name="IsDeletedByAuthor">A value indicating whether loaded messages are deleted by author. false - messages are not deleted by author, null to load all messages</param>
        /// <param name="IsDeletedByRecipient">A value indicating whether loaded messages are deleted by recipient. false - messages are not deleted by recipient, null to load all messages</param>
        /// <param name="Keywords">Keywords</param>
        /// <param name="PageSize">Page size</param>
        /// <param name="PageIndex">Page index</param>
        /// <param name="TotalRecords">Total records</param>
        /// <returns>Private messages</returns>
        public static PrivateMessageCollection GetAllPrivateMessages(int FromUserID, int ToUserID,
            bool? IsRead, bool? IsDeletedByAuthor, bool? IsDeletedByRecipient, 
            string Keywords, int PageSize, int PageIndex, out int TotalRecords)
        {
            if (PageSize <= 0)
                PageSize = 10;
            if (PageSize == int.MaxValue)
                PageSize = int.MaxValue - 1;

            if (PageIndex < 0)
                PageIndex = 0;
            if (PageIndex == int.MaxValue)
                PageIndex = int.MaxValue - 1;

            DBPrivateMessageCollection dbCollection = DBProviderManager<DBForumProvider>.Provider.GetAllPrivateMessages(FromUserID,
                ToUserID, IsRead, IsDeletedByAuthor, IsDeletedByRecipient, Keywords, PageSize, PageIndex, out TotalRecords);
            PrivateMessageCollection privateMessageCollection = DBMapping(dbCollection);

            return privateMessageCollection;
        }

        /// <summary>
        /// Inserts a private message
        /// </summary>
        /// <param name="FromUserID">The user identifier who sent the message</param>
        /// <param name="ToUserID">The user identifier who should receive the message</param>
        /// <param name="Subject">The subject</param>
        /// <param name="Text">The text</param>
        /// <param name="IsRead">The value indivating whether message is read</param>
        /// <param name="IsDeletedByAuthor">The value indivating whether message is deleted by author</param>
        /// <param name="IsDeletedByRecipient">The value indivating whether message is deleted by recipient</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>Private message</returns>
        public static PrivateMessage InsertPrivateMessage(int FromUserID, int ToUserID,
            string Subject, string Text, bool IsRead,
            bool IsDeletedByAuthor, bool IsDeletedByRecipient, DateTime CreatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);

            if (Subject == null)
                Subject = string.Empty;
            Subject = Subject.Trim();
            if (String.IsNullOrEmpty(Subject))
                throw new NopException("Subject cannot be empty");
            if (ForumManager.PMSubjectMaxLength > 0)
            {
                if (Subject.Length > ForumManager.PMSubjectMaxLength)
                    Subject = Subject.Substring(0, ForumManager.PMSubjectMaxLength);
            }

            if (Text == null)
                Text = string.Empty;
            Text = Text.Trim();
            if (String.IsNullOrEmpty(Text))
                throw new NopException("Text cannot be empty");
            if (ForumManager.PMTextMaxLength > 0)
            {
                if (Text.Length > ForumManager.PMTextMaxLength)
                    Text = Text.Substring(0, ForumManager.PMTextMaxLength);
            }

            DBPrivateMessage dbItem = DBProviderManager<DBForumProvider>.Provider.InsertPrivateMessage(FromUserID, ToUserID,
            Subject, Text, IsRead, IsDeletedByAuthor, IsDeletedByRecipient, CreatedOn);

            PrivateMessage privateMessage = DBMapping(dbItem);

            return privateMessage;
        }

        /// <summary>
        /// Updates the private message
        /// </summary>
        /// <param name="PrivateMessageID">The private message identifier</param>
        /// <param name="FromUserID">The user identifier who sent the message</param>
        /// <param name="ToUserID">The user identifier who should receive the message</param>
        /// <param name="Subject">The subject</param>
        /// <param name="Text">The text</param>
        /// <param name="IsRead">The value indivating whether message is read</param>
        /// <param name="IsDeletedByAuthor">The value indivating whether message is deleted by author</param>
        /// <param name="IsDeletedByRecipient">The value indivating whether message is deleted by recipient</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>Private message</returns>
        public static PrivateMessage UpdatePrivateMessage(int PrivateMessageID, int FromUserID, int ToUserID,
            string Subject, string Text, bool IsRead,
            bool IsDeletedByAuthor, bool IsDeletedByRecipient, DateTime CreatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);

            if (Subject == null)
                Subject = string.Empty;
            Subject = Subject.Trim();
            if (String.IsNullOrEmpty(Subject))
                throw new NopException("Subject cannot be empty");
            if (ForumManager.PMSubjectMaxLength > 0)
            {
                if (Subject.Length > ForumManager.PMSubjectMaxLength)
                    Subject = Subject.Substring(0, ForumManager.PMSubjectMaxLength);
            }

            if (Text == null)
                Text = string.Empty;
            Text = Text.Trim();
            if (String.IsNullOrEmpty(Text))
                throw new NopException("Text cannot be empty");
            if (ForumManager.PMTextMaxLength > 0)
            {
                if (Text.Length > ForumManager.PMTextMaxLength)
                    Text = Text.Substring(0, ForumManager.PMTextMaxLength);
            }

            if (IsDeletedByAuthor && IsDeletedByRecipient)
            {
                DeletePrivateMessage(PrivateMessageID);
                return null;
            }
            else
            {
                DBPrivateMessage dbItem = DBProviderManager<DBForumProvider>.Provider.UpdatePrivateMessage(PrivateMessageID,
                    FromUserID, ToUserID, Subject, Text, IsRead, IsDeletedByAuthor, IsDeletedByRecipient, CreatedOn);
                PrivateMessage privateMessage = DBMapping(dbItem);
                return privateMessage;
            }
        }

        /// <summary>
        /// Deletes a forum subscription
        /// </summary>
        /// <param name="ForumSubscriptionID">The forum subscription identifier</param>
        public static void DeleteSubscription(int ForumSubscriptionID)
        {
            DBProviderManager<DBForumProvider>.Provider.DeleteSubscription(ForumSubscriptionID);
        }

        /// <summary>
        /// Gets a forum subscription
        /// </summary>
        /// <param name="ForumSubscriptionID">The forum subscription identifier</param>
        /// <returns>Forum subscription</returns>
        public static ForumSubscription GetSubscriptionByID(int ForumSubscriptionID)
        {
            if (ForumSubscriptionID == 0)
                return null;

            DBForumSubscription dbItem = DBProviderManager<DBForumProvider>.Provider.GetSubscriptionByID(ForumSubscriptionID);
            ForumSubscription forumSubscription = DBMapping(dbItem);

            return forumSubscription;
        }

        /// <summary>
        /// Gets a forum subscription
        /// </summary>
        /// <param name="SubscriptionGUID">The forum subscription identifier</param>
        /// <returns>Forum subscription</returns>
        public static ForumSubscription GetSubscriptionByGUID(int SubscriptionGUID)
        {
            DBForumSubscription dbItem = DBProviderManager<DBForumProvider>.Provider.GetSubscriptionByGUID(SubscriptionGUID);
            ForumSubscription forumSubscription = DBMapping(dbItem);

            return forumSubscription;
        }

        /// <summary>
        /// Gets forum subscriptions
        /// </summary>
        /// <param name="UserID">The user identifier</param>
        /// <param name="ForumID">The forum identifier</param>
        /// <param name="TopicID">The topic identifier</param>
        /// <param name="PageSize">Page size</param>
        /// <param name="PageIndex">Page index</param>
        /// <returns>Forum subscriptions</returns>
        public static ForumSubscriptionCollection GetAllSubscriptions(int UserID, int ForumID,
            int TopicID, int PageSize, int PageIndex)
        {
            int TotalRecords = 0;
            return GetAllSubscriptions(UserID, ForumID,
             TopicID, PageSize, PageIndex, out TotalRecords);
        }

        /// <summary>
        /// Gets forum subscriptions
        /// </summary>
        /// <param name="UserID">The user identifier</param>
        /// <param name="ForumID">The forum identifier</param>
        /// <param name="TopicID">The topic identifier</param>
        /// <param name="PageSize">Page size</param>
        /// <param name="PageIndex">Page index</param>
        /// <param name="TotalRecords">Total records</param>
        /// <returns>Forum subscriptions</returns>
        public static ForumSubscriptionCollection GetAllSubscriptions(int UserID, int ForumID,
            int TopicID, int PageSize, int PageIndex, out int TotalRecords)
        {
            if (PageSize <= 0)
                PageSize = 10;
            if (PageSize == int.MaxValue)
                PageSize = int.MaxValue - 1;

            if (PageIndex < 0)
                PageIndex = 0;
            if (PageIndex == int.MaxValue)
                PageIndex = int.MaxValue - 1;

            DBForumSubscriptionCollection dbCollection = DBProviderManager<DBForumProvider>.Provider.GetAllSubscriptions(UserID,
                ForumID, TopicID, PageSize, PageIndex, out TotalRecords);
            ForumSubscriptionCollection forumSubscriptionCollection = DBMapping(dbCollection);

            return forumSubscriptionCollection;
        }

        /// <summary>
        /// Inserts a forum subscription
        /// </summary>
        /// <param name="SubscriptionGUID">The forum subscription identifier</param>
        /// <param name="UserID">The user identifier</param>
        /// <param name="ForumID">The forum identifier</param>
        /// <param name="TopicID">The topic identifier</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>Forum subscription</returns>
        public static ForumSubscription InsertSubscription(Guid SubscriptionGUID, int UserID,
            int ForumID, int TopicID, DateTime CreatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);

            DBForumSubscription dbItem = DBProviderManager<DBForumProvider>.Provider.InsertSubscription(SubscriptionGUID, UserID,
            ForumID, TopicID, CreatedOn);

            ForumSubscription forumSubscription = DBMapping(dbItem);

            return forumSubscription;
        }

        /// <summary>
        /// Updates the forum subscription
        /// </summary>
        /// <param name="SubscriptionID">The forum subscription identifier</param>
        /// <param name="SubscriptionGUID">The forum subscription identifier</param>
        /// <param name="UserID">The user identifier</param>
        /// <param name="ForumID">The forum identifier</param>
        /// <param name="TopicID">The topic identifier</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>Forum subscription</returns>
        public static ForumSubscription UpdateSubscription(int SubscriptionID, Guid SubscriptionGUID, 
            int UserID,  int ForumID, int TopicID, DateTime CreatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);

            DBForumSubscription dbItem = DBProviderManager<DBForumProvider>.Provider.UpdateSubscription(SubscriptionID,
                SubscriptionGUID, UserID, ForumID, TopicID, CreatedOn);

            ForumSubscription forumSubscription = DBMapping(dbItem);

            return forumSubscription;
        }

        /// <summary>
        /// Check whether user is allowed to create new topics
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="forum">Forum</param>
        /// <returns>True if allowed, otherwise false</returns>
        public static bool IsUserAllowedToCreateTopic(Customer customer, Forum forum)
        {
            if (forum == null)
                return false;

            if (customer == null)
                return false;

            if (customer.IsGuest)
                return false;

            if (customer.IsForumModerator)
                return true;

            return true;
        }

        /// <summary>
        /// Check whether user is allowed to edit topic
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="topic">Topic</param>
        /// <returns>True if allowed, otherwise false</returns>
        public static bool IsUserAllowedToEditTopic(Customer customer, ForumTopic topic)
        {
            if (topic == null)
                return false;

            if (customer == null)
                return false;

            if (customer.IsGuest)
                return false;

            if (customer.IsForumModerator)
                return true;

            if (ForumManager.AllowCustomersToEditPosts)
            {
                bool ownTopic = customer.CustomerID == topic.UserID;
                return ownTopic;
            }

            return false;
        }

        /// <summary>
        /// Check whether user is allowed to move topic
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="topic">Topic</param>
        /// <returns>True if allowed, otherwise false</returns>
        public static bool IsUserAllowedToMoveTopic(Customer customer, ForumTopic topic)
        {
            if (topic == null)
                return false;

            if (customer == null)
                return false;

            if (customer.IsGuest)
                return false;

            if (customer.IsForumModerator)
                return true;

            return false;
        }

        /// <summary>
        /// Check whether user is allowed to delete topic
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="topic">Topic</param>
        /// <returns>True if allowed, otherwise false</returns>
        public static bool IsUserAllowedToDeleteTopic(Customer customer, ForumTopic topic)
        {
            if (topic == null)
                return false;

            if (customer == null)
                return false;

            if (customer.IsGuest)
                return false;

            if (customer.IsForumModerator)
                return true;
            
            if (ForumManager.AllowCustomersToDeletePosts)
            {
                bool ownTopic = customer.CustomerID == topic.UserID;
                return ownTopic;
            }

            return false;
        }

        /// <summary>
        /// Check whether user is allowed to create new post
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="topic">Topic</param>
        /// <returns>True if allowed, otherwise false</returns>
        public static bool IsUserAllowedToCreatePost(Customer customer, ForumTopic topic)
        {
            if (topic == null)
                return false;

            if (customer == null)
                return false;

            if (customer.IsGuest)
                return false;

            if (customer.IsForumModerator)
                return true;

            return true;
        }

        /// <summary>
        /// Check whether user is allowed to edit post
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="post">Topic</param>
        /// <returns>True if allowed, otherwise false</returns>
        public static bool IsUserAllowedToEditPost(Customer customer, ForumPost post)
        {
            if (post == null)
                return false;

            if (customer == null)
                return false;

            if (customer.IsGuest)
                return false;

            if (customer.IsForumModerator)
                return true;
            
            if (ForumManager.AllowCustomersToEditPosts)
            {
                bool ownPost = customer.CustomerID == post.UserID;
                return ownPost;
            }

            return false;
        }

        /// <summary>
        /// Check whether user is allowed to delete post
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="post">Topic</param>
        /// <returns>True if allowed, otherwise false</returns>
        public static bool IsUserAllowedToDeletePost(Customer customer, ForumPost post)
        {
            if (post == null)
                return false;

            if (customer == null)
                return false;

            if (customer.IsGuest)
                return false;

            if (customer.IsForumModerator)
                return true;

            if (ForumManager.AllowCustomersToDeletePosts)
            {
                bool ownPost = customer.CustomerID == post.UserID;
                return ownPost;
            }

            return false;
        }

        /// <summary>
        /// Check whether user is allowed to set topic priority
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <returns>True if allowed, otherwise false</returns>
        public static bool IsUserAllowedToSetTopicPriority(Customer customer)
        {
            if (customer == null)
                return false;

            if (customer.IsGuest)
                return false;

            if (customer.IsForumModerator)
                return true;

            return false;
        }

        /// <summary>
        /// Check whether user is allowed to watch topics
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <returns>True if allowed, otherwise false</returns>
        public static bool IsUserAllowedToSubscribe(int CustomerID)
        {
            Customer customer = CustomerManager.GetCustomerByID(CustomerID);
            return IsUserAllowedToSubscribe(customer);
        }

        /// <summary>
        /// Check whether user is allowed to watch topics
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <returns>True if allowed, otherwise false</returns>
        public static bool IsUserAllowedToSubscribe(Customer customer)
        {
            if (customer == null)
                return false;

            if (customer.IsGuest)
                return false;

            return true;
        }

        /// <summary>
        /// Formats the posts text
        /// </summary>
        /// <param name="Text">Text</param>
        /// <returns>Formatted text</returns>
        public static string FormatPostText(string Text)
        {
            if (String.IsNullOrEmpty(Text))
                return string.Empty;

            switch (ForumManager.ForumEditor)
            {
                case EditorTypeEnum.SimpleTextBox:
                    {
                        Text = HtmlHelper.FormatText(Text, false, true, false, false, false, false);
                    }
                    break;
                case EditorTypeEnum.BBCodeEditor:
                    {
                        Text = HtmlHelper.FormatText(Text, false, true, false, true, false, false);
                    }
                    break;
                case EditorTypeEnum.HtmlEditor:
                    {
                        Text = HtmlHelper.FormatText(Text, false, false, true, false, false, false);
                    }
                    break;
                default:
                    break;
            }

            return Text;
        }

        /// <summary>
        /// Formats the signature text
        /// </summary>
        /// <param name="Text">Text</param>
        /// <returns>Formatted text</returns>
        public static string FormatSignatureText(string Text)
        {
            if (String.IsNullOrEmpty(Text))
                return string.Empty;

            Text = HtmlHelper.FormatText(Text, false, true, false, false, false, false);
            return Text;
        }

        /// <summary>
        /// Formats the private message text
        /// </summary>
        /// <param name="Text">Text</param>
        /// <returns>Formatted text</returns>
        public static string FormatPrivateMessageText(string Text)
        {
            if (String.IsNullOrEmpty(Text))
                return string.Empty;

            Text = HtmlHelper.FormatText(Text, false, true, false, true, false, false);

            return Text;
        }

        /// <summary>
        /// Strips topic subject
        /// </summary>
        /// <param name="Subject">Subject</param>
        /// <returns>Formatted subject</returns>
        public static string StripTopicSubject(string Subject)
        {
            if (String.IsNullOrEmpty(Subject))
                return Subject;
            int StrippedTopicMaxLength = SettingManager.GetSettingValueInteger("Forums.StrippedTopicMaxLength", 45);
            if (StrippedTopicMaxLength > 0)
            {
                if (Subject.Length > StrippedTopicMaxLength)
                {
                    int index = Subject.IndexOf(" ", StrippedTopicMaxLength);
                    if (index > 0)
                    {
                        Subject = Subject.Substring(0, index);
                        Subject += "...";
                    }
                }
            }

            return Subject;
        }

        /// <summary>
        /// Calculates topic page index by post identifier
        /// </summary>
        /// <param name="ForumTopicID">Topic identifier</param>
        /// <param name="PageSize">Page size</param>
        /// <param name="PostID">Post identifier</param>
        /// <returns>Page index</returns>
        public static int CalculateTopicPageIndex(int ForumTopicID, int PageSize, int PostID)
        {
            int pageIndex = 0;
            int totalRecords = 0;
            ForumPostCollection forumPosts = ForumManager.GetAllPosts(ForumTopicID, 0, 
                string.Empty, true, int.MaxValue, 0, out totalRecords);

            for (int i = 0; i < totalRecords; i++)
            {
                if (forumPosts[i].ForumPostID == PostID)
                {
                    if (PageSize > 0)
                    {
                        pageIndex = i/PageSize;
                    }
                }
            }

            return pageIndex;
        }

        #endregion
        
        #region Property

        /// <summary>
        /// Gets a value indicating whether cache is enabled
        /// </summary>
        public static bool CacheEnabled
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("Cache.ForumManager.CacheEnabled");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether forums are enabled
        /// </summary>
        public static bool ForumsEnabled
        {
            get
            {
                bool forumsEnabled = SettingManager.GetSettingValueBoolean("Forums.ForumsEnabled");
                return forumsEnabled;
            }
            set
            {
                SettingManager.SetParam("Forums.ForumsEnabled", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether customers are allowed to edit posts that they created.
        /// </summary>
        public static bool AllowCustomersToEditPosts
        {
            get
            {
                bool allowCustomersToEditPosts = SettingManager.GetSettingValueBoolean("Forums.CustomersAllowedToEditPosts");
                return allowCustomersToEditPosts;
            }
            set
            {
                SettingManager.SetParam("Forums.CustomersAllowedToEditPosts", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether customers are allowed to delete posts that they created.
        /// </summary>
        public static bool AllowCustomersToDeletePosts
        {
            get
            {
                bool allowCustomersToDeletePosts = SettingManager.GetSettingValueBoolean("Forums.CustomersAllowedToDeletePosts");
                return allowCustomersToDeletePosts;
            }
            set
            {
                SettingManager.SetParam("Forums.CustomersAllowedToDeletePosts", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets maximum length of topic subject
        /// </summary>
        public static int TopicSubjectMaxLength
        {
            get
            {
                int topicSubjectMaxLength = SettingManager.GetSettingValueInteger("Forums.TopicSubjectMaxLength");
                return topicSubjectMaxLength;
            }
            set
            {
                SettingManager.SetParam("Forums.TopicSubjectMaxLength", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets maximum length of post
        /// </summary>
        public static int PostMaxLength
        {
            get
            {
                int postMaxLength = SettingManager.GetSettingValueInteger("Forums.PostMaxLength");
                return postMaxLength;
            }
            set
            {
                SettingManager.SetParam("Forums.PostMaxLength", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets the page size for topics in forums
        /// </summary>
        public static int TopicsPageSize
        {
            get
            {
                int topicsPageSize = SettingManager.GetSettingValueInteger("Forums.TopicsPageSize");
                return topicsPageSize;
            }
            set
            {
                SettingManager.SetParam("Forums.TopicsPageSize", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets the page size for posts in topics
        /// </summary>
        public static int PostsPageSize
        {
            get
            {
                int postsPageSize = SettingManager.GetSettingValueInteger("Forums.PostsPageSize");
                return postsPageSize;
            }
            set
            {
                SettingManager.SetParam("Forums.PostsPageSize", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets the page size for search result
        /// </summary>
        public static int SearchResultsPageSize
        {
            get
            {
                int searchResultsPageSize = SettingManager.GetSettingValueInteger("Forums.SearchResultsPageSize");
                return searchResultsPageSize;
            }
            set
            {
                SettingManager.SetParam("Forums.SearchResultsPageSize", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets the page size for latest user post
        /// </summary>
        public static int LatestUserPostsPageSize
        {
            get
            {
                int latestUserPostsPageSize = SettingManager.GetSettingValueInteger("Forums.LatestUserPostsPageSize");
                return latestUserPostsPageSize;
            }
            set
            {
                SettingManager.SetParam("Forums.LatestUserPostsPageSize", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show customers forum post count
        /// </summary>
        public static bool ShowCustomersPostCount
        {
            get
            {
                bool showCustomersPostCount = SettingManager.GetSettingValueBoolean("Forums.ShowCustomersPostCount");
                return showCustomersPostCount;
            }
            set
            {
                SettingManager.SetParam("Forums.ShowCustomersPostCount", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a forum editor type
        /// </summary>
        public static EditorTypeEnum ForumEditor
        {
            get
            {
                int forumEditorTypeID = SettingManager.GetSettingValueInteger("Forums.EditorType");
                return (EditorTypeEnum)Enum.ToObject(typeof(EditorTypeEnum), forumEditorTypeID);
            }
            set
            {
                SettingManager.SetParam("Forums.EditorType", ((int)value).ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether customers are allowed to specify signature.
        /// </summary>
        public static bool SignaturesEnabled
        {
            get
            {
                bool signaturesEnabled = SettingManager.GetSettingValueBoolean("Forums.SignatureEnabled");
                return signaturesEnabled;
            }
            set
            {
                SettingManager.SetParam("Forums.SignatureEnabled", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether private messages are allowed
        /// </summary>
        public static bool AllowPrivateMessages
        {
            get
            {
                bool forumsEnabled = SettingManager.GetSettingValueBoolean("Messaging.AllowPM");
                return forumsEnabled;
            }
            set
            {
                SettingManager.SetParam("Messaging.AllowPM", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets maximum length of pm subject
        /// </summary>
        public static int PMSubjectMaxLength
        {
            get
            {
                int pmSubjectMaxLength = SettingManager.GetSettingValueInteger("Messaging.PMSubjectMaxLength");
                return pmSubjectMaxLength;
            }
            set
            {
                SettingManager.SetParam("Messaging.PMSubjectMaxLength", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets maximum length of pm message
        /// </summary>
        public static int PMTextMaxLength
        {
            get
            {
                int pmTextMaxLength = SettingManager.GetSettingValueInteger("Messaging.PMTextMaxLength");
                return pmTextMaxLength;
            }
            set
            {
                SettingManager.SetParam("Messaging.PMTextMaxLength", value.ToString());
            }
        }

        #endregion
    }
}
