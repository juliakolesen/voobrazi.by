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

namespace NopSolutions.NopCommerce.DataAccess.Content.Forums
{
    /// <summary>
    /// Acts as a base class for deriving custom forum provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/ForumProvider")]
    public abstract partial class DBForumProvider : BaseDBProvider
    {
        #region Methods

        /// <summary>
        /// Deletes a forum group
        /// </summary>
        /// <param name="ForumGroupID">The forum group identifier</param>
        public abstract void DeleteForumGroup(int ForumGroupID);

        /// <summary>
        /// Gets a forum group
        /// </summary>
        /// <param name="ForumGroupID">The forum group identifier</param>
        /// <returns>Forum group</returns>
        public abstract DBForumGroup GetForumGroupByID(int ForumGroupID);

        /// <summary>
        /// Gets all forum groups
        /// </summary>
        /// <returns>Forum groups</returns>
        public abstract DBForumGroupCollection GetAllForumGroups();

        /// <summary>
        /// Inserts a forum group
        /// </summary>
        /// <param name="Name">The language name</param>
        /// <param name="Description">The description</param>
        /// <param name="DisplayOrder">The display order</param>        
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Forum group</returns>
        public abstract DBForumGroup InsertForumGroup(string Name, string Description,
            int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn);

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
        public abstract DBForumGroup UpdateForumGroup(int ForumGroupID, string Name, string Description,
            int DisplayOrder, DateTime CreatedOn, DateTime UpdatedOn);

        /// <summary>
        /// Deletes a forum
        /// </summary>
        /// <param name="ForumID">The forum identifier</param>
        public abstract void DeleteForum(int ForumID);

        /// <summary>
        /// Gets a forum
        /// </summary>
        /// <param name="ForumID">The forum identifier</param>
        /// <returns>Forum</returns>
        public abstract DBForum GetForumByID(int ForumID);

        /// <summary>
        /// Gets forums by group identifier
        /// </summary>
        /// <param name="ForumGroupID">The forum group identifier</param>
        /// <returns>Forums</returns>
        public abstract DBForumCollection GetAllForumsByGroupID(int ForumGroupID);

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
        public abstract DBForum InsertForum(int ForumGroupID,
            string Name, string Description,
            int NumTopics, int NumPosts, int LastTopicID, int LastPostID,
            int LastPostUserID, DateTime? LastPostTime, int DisplayOrder,
            DateTime CreatedOn, DateTime UpdatedOn);

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
        public abstract DBForum UpdateForum(int ForumID, int ForumGroupID,
            string Name, string Description,
            int NumTopics, int NumPosts, int LastTopicID, int LastPostID,
            int LastPostUserID, DateTime? LastPostTime, int DisplayOrder,
            DateTime CreatedOn, DateTime UpdatedOn);

        /// <summary>
        /// Update forum stats
        /// </summary>
        /// <param name="ForumID">The forum identifier</param>
        public abstract void UpdateForumStats(int ForumID);

        /// <summary>
        /// Deletes a topic
        /// </summary>
        /// <param name="ForumTopicID">The topic identifier</param>
        public abstract void DeleteTopic(int ForumTopicID);

        /// <summary>
        /// Gets a topic
        /// </summary>
        /// <param name="ForumTopicID">The topic identifier</param>
        /// <param name="IncreaseViews">The value indicating whether to increase topic views</param>
        /// <returns>Topic</returns>
        public abstract DBForumTopic GetTopicByID(int ForumTopicID, bool IncreaseViews);

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
        public abstract DBForumTopicCollection GetAllTopics(int ForumID, int UserID, string Keywords,
            bool SearchPosts, int PageSize, int PageIndex, out int TotalRecords);

        /// <summary>
        /// Gets active topics
        /// </summary>
        /// <param name="ForumID">The forum group identifier</param>
        /// <param name="TopicCount">Topic count. 0 if you want to get all topics</param>
        /// <returns>Topics</returns>
        public abstract DBForumTopicCollection GetActiveTopics(int ForumID, int TopicCount);

        /// <summary>
        /// Inserts a topic
        /// </summary>
        /// <param name="ForumID">The forum identifier</param>
        /// <param name="UserID">The user identifier</param>
        /// <param name="TopicTypeID">The topic type identifier</param>
        /// <param name="Subject">The subject</param>
        /// <param name="NumPosts">The number of posts</param>
        /// <param name="Views">The number of views</param>
        /// <param name="LastPostID">The last post identifier</param>
        /// <param name="LastPostUserID">The last post user identifier</param>
        /// <param name="LastPostTime">The last post date and time</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Topic</returns>
        public abstract DBForumTopic InsertTopic(int ForumID, int UserID,
            int TopicTypeID, string Subject,
            int NumPosts, int Views, int LastPostID,
            int LastPostUserID, DateTime? LastPostTime,
            DateTime CreatedOn, DateTime UpdatedOn);

        /// <summary>
        /// Updates the topic
        /// </summary>
        /// <param name="ForumTopicID">The forum topic identifier</param>
        /// <param name="ForumID">The forum identifier</param>
        /// <param name="UserID">The user identifier</param>
        /// <param name="TopicTypeID">The topic type identifier</param>
        /// <param name="Subject">The subject</param>
        /// <param name="NumPosts">The number of posts</param>
        /// <param name="Views">The number of views</param>
        /// <param name="LastPostID">The last post identifier</param>
        /// <param name="LastPostUserID">The last post user identifier</param>
        /// <param name="LastPostTime">The last post date and time</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Topic</returns>
        public abstract DBForumTopic UpdateTopic(int ForumTopicID, int ForumID, int UserID,
            int TopicTypeID, string Subject,
            int NumPosts, int Views, int LastPostID,
            int LastPostUserID, DateTime? LastPostTime,
            DateTime CreatedOn, DateTime UpdatedOn);

        /// <summary>
        /// Deletes a post
        /// </summary>
        /// <param name="ForumPostID">The post identifier</param>
        public abstract void DeletePost(int ForumPostID);

        /// <summary>
        /// Gets a post
        /// </summary>
        /// <param name="ForumPostID">The post identifier</param>
        /// <returns>Post</returns>
        public abstract DBForumPost GetPostByID(int ForumPostID);

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
        public abstract DBForumPostCollection GetAllPosts(int ForumTopicID, int UserID, string Keywords,
            bool AscSort, int PageSize, int PageIndex, out int TotalRecords);

        /// <summary>
        /// Inserts a post
        /// </summary>
        /// <param name="ForumTopicID">The forum topic identifier</param>
        /// <param name="UserID">The user identifier</param>
        /// <param name="Text">The text</param>
        /// <param name="IPAddress">The IP address</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Post</returns>
        public abstract DBForumPost InsertPost(int ForumTopicID, int UserID,
            string Text, string IPAddress, DateTime CreatedOn, DateTime UpdatedOn);

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
        public abstract DBForumPost UpdatePost(int ForumPostID, int ForumTopicID, int UserID,
            string Text, string IPAddress, DateTime CreatedOn, DateTime UpdatedOn);

        /// <summary>
        /// Deletes a private message
        /// </summary>
        /// <param name="ForumPrivateMessageID">The private message identifier</param>
        public abstract void DeletePrivateMessage(int ForumPrivateMessageID);

        /// <summary>
        /// Gets a private message
        /// </summary>
        /// <param name="ForumPrivateMessageID">The private message identifier</param>
        /// <returns>Private message</returns>
        public abstract DBPrivateMessage GetPrivateMessageByID(int ForumPrivateMessageID);

        /// <summary>
        /// Gets private messages
        /// </summary>
        /// <param name="FromUserID">The user identifier who sent the message</param>
        /// <param name="ToUserID">The user identifier who should receive the message</param>
        /// <param name="IsRead">A value indicating whether loaded messages are read. false - to load not read messages only, 1 to load read messages only, null to load all messages</param>
        /// <param name="IsDeletedByAuthor">A value indicating whether loaded messages are deleted by author. false - messages are not deleted by author, null to load all messages</param>
        /// <param name="IsDeletedByRecipient">A value indicating whether loaded messages are deleted by recipient. false - messages are not deleted by recipient, null to load all messages</param>
        /// <param name="Keywords">Keywords</param>
        /// <param name="PageSize">Page size</param>
        /// <param name="PageIndex">Page index</param>
        /// <param name="TotalRecords">Total records</param>
        /// <returns>Private messages</returns>
        public abstract DBPrivateMessageCollection GetAllPrivateMessages(int FromUserID, int ToUserID,
            bool? IsRead, bool? IsDeletedByAuthor, bool? IsDeletedByRecipient, 
            string Keywords, int PageSize, int PageIndex, out int TotalRecords);

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
        public abstract DBPrivateMessage InsertPrivateMessage(int FromUserID, int ToUserID,
            string Subject, string Text, bool IsRead,
            bool IsDeletedByAuthor, bool IsDeletedByRecipient, DateTime CreatedOn);

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
        public abstract DBPrivateMessage UpdatePrivateMessage(int PrivateMessageID, int FromUserID, int ToUserID,
            string Subject, string Text, bool IsRead,
            bool IsDeletedByAuthor, bool IsDeletedByRecipient, DateTime CreatedOn);

        /// <summary>
        /// Deletes a forum subscription
        /// </summary>
        /// <param name="ForumSubscriptionID">The forum subscription identifier</param>
        public abstract void DeleteSubscription(int ForumSubscriptionID);

        /// <summary>
        /// Gets a forum subscription
        /// </summary>
        /// <param name="ForumSubscriptionID">The forum subscription identifier</param>
        /// <returns>Forum subscription</returns>
        public abstract DBForumSubscription GetSubscriptionByID(int ForumSubscriptionID);

        /// <summary>
        /// Gets a forum subscription
        /// </summary>
        /// <param name="SubscriptionGUID">The forum subscription identifier</param>
        /// <returns>Forum subscription</returns>
        public abstract DBForumSubscription GetSubscriptionByGUID(int SubscriptionGUID);

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
        public abstract DBForumSubscriptionCollection GetAllSubscriptions(int UserID, int ForumID,
            int TopicID, int PageSize, int PageIndex, out int TotalRecords);

        /// <summary>
        /// Inserts a forum subscription
        /// </summary>
        /// <param name="SubscriptionGUID">The forum subscription identifier</param>
        /// <param name="UserID">The user identifier</param>
        /// <param name="ForumID">The forum identifier</param>
        /// <param name="TopicID">The topic identifier</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>Forum subscription</returns>
        public abstract DBForumSubscription InsertSubscription(Guid SubscriptionGUID, int UserID,
            int ForumID, int TopicID, DateTime CreatedOn);

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
        public abstract DBForumSubscription UpdateSubscription(int SubscriptionID, Guid SubscriptionGUID, int UserID,
            int ForumID, int TopicID, DateTime CreatedOn);

        #endregion
    }
}
