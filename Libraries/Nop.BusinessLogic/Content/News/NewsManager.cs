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
using System.Web;
using NopSolutions.NopCommerce.BusinessLogic.Caching;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Localization;
using NopSolutions.NopCommerce.BusinessLogic.Messages;
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.BusinessLogic.Utils.Html;
using NopSolutions.NopCommerce.Common.Utils.Html;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Content.NewsManagement;

namespace NopSolutions.NopCommerce.BusinessLogic.Content.NewsManagement
{
    /// <summary>
    /// News manager
    /// </summary>
    public partial class NewsManager
    {
        #region Constants
        private const string NEWS_BY_ID_KEY = "Nop.news.id-{0}";
        private const string NEWS_PATTERN_KEY = "Nop.news.";
        #endregion

        #region Utilities
        private static NewsCollection DBMapping(DBNewsCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            NewsCollection collection = new NewsCollection();
            foreach (DBNews dbItem in dbCollection)
            {
                News item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static News DBMapping(DBNews dbItem)
        {
            if (dbItem == null)
                return null;

            News item = new News();
            item.NewsID = dbItem.NewsID;
            item.LanguageID = dbItem.LanguageID;
            item.Title = dbItem.Title;
            item.Short = dbItem.Short;
            item.Full = dbItem.Full;
            item.Published = dbItem.Published;
            item.AllowComments = dbItem.AllowComments;
            item.CreatedOn = dbItem.CreatedOn;

            return item;
        }

        private static NewsCommentCollection DBMapping(DBNewsCommentCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            NewsCommentCollection collection = new NewsCommentCollection();
            foreach (DBNewsComment dbItem in dbCollection)
            {
                NewsComment item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static NewsComment DBMapping(DBNewsComment dbItem)
        {
            if (dbItem == null)
                return null;

            NewsComment item = new NewsComment();
            item.NewsCommentID = dbItem.NewsCommentID;
            item.NewsID = dbItem.NewsID;
            item.CustomerID = dbItem.CustomerID;
            item.Title = dbItem.Title;
            item.Comment = dbItem.Comment;
            item.CreatedOn = dbItem.CreatedOn;

            return item;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets a news
        /// </summary>
        /// <param name="NewsID">The news identifier</param>
        /// <returns>News</returns>
        public static News GetNewsByID(int NewsID)
        {
            if (NewsID == 0)
                return null;

            string key = string.Format(NEWS_BY_ID_KEY, NewsID);
            object obj2 = NopCache.Get(key);
            if (NewsManager.CacheEnabled && (obj2 != null))
            {
                return (News)obj2;
            }

            DBNews dbItem = DBProviderManager<DBNewsProvider>.Provider.GetNewsByID(NewsID);
            News news = DBMapping(dbItem);

            if (NewsManager.CacheEnabled)
            {
                NopCache.Max(key, news);
            }
            return news;
        }

        /// <summary>
        /// Deletes a news
        /// </summary>
        /// <param name="NewsID">The news identifier</param>
        public static void DeleteNews(int NewsID)
        {
            DBProviderManager<DBNewsProvider>.Provider.DeleteNews(NewsID);
            if (NewsManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(NEWS_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets news item collection
        /// </summary>
        /// <param name="LanguageID">Language identifier. 0 if you want to get all news</param>
        /// <param name="NewsCount">News item count. 0 if you want to get all news</param>
        /// <returns>News item collection</returns>
        public static NewsCollection GetNews(int LanguageID, int NewsCount)
        {
            bool showHidden = NopContext.Current.IsAdmin;
            DBNewsCollection dbCollection = DBProviderManager<DBNewsProvider>.Provider.GetNews(LanguageID, NewsCount, showHidden);
            NewsCollection collection = DBMapping(dbCollection);
            return collection;
        }

        /// <summary>
        /// Gets all news
        /// </summary>
        /// <param name="LanguageID">Language identifier. 0 if you want to get all news</param>
        /// <returns>News item collection</returns>
        public static NewsCollection GetAllNews(int LanguageID)
        {
            return GetNews(LanguageID, 0);
        }

        /// <summary>
        /// Inserts a news item
        /// </summary>
        /// <param name="LanguageID">The language identifier</param>
        /// <param name="Title">The news title</param>
        /// <param name="Short">The short text</param>
        /// <param name="Full">The full text</param>
        /// <param name="Published">A value indicating whether the entity is published</param>
        /// <param name="AllowComments">A value indicating whether the entity allows comments</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>News item</returns>
        public static News InsertNews(int LanguageID, string Title, string Short,
            string Full, bool Published, bool AllowComments, DateTime CreatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);

            DBNews dbItem = DBProviderManager<DBNewsProvider>.Provider.InsertNews(LanguageID, Title, Short,
                Full, Published, AllowComments, CreatedOn);
            News news = DBMapping(dbItem);

            if (NewsManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(NEWS_PATTERN_KEY);
            }

            return news;
        }

        /// <summary>
        /// Updates the news item
        /// </summary>
        /// <param name="NewsID">The news identifier</param>
        /// <param name="LanguageID">The language identifier</param>
        /// <param name="Title">The news title</param>
        /// <param name="Short">The short text</param>
        /// <param name="Full">The full text</param>
        /// <param name="Published">A value indicating whether the entity is published</param>
        /// <param name="AllowComments">A value indicating whether the entity allows comments</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>News item</returns>
        public static News UpdateNews(int NewsID, int LanguageID, string Title, string Short,
            string Full, bool Published, bool AllowComments, DateTime CreatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);

            DBNews dbItem = DBProviderManager<DBNewsProvider>.Provider.UpdateNews(NewsID, LanguageID, Title, Short,
                Full, Published, AllowComments, CreatedOn);
            News news = DBMapping(dbItem);

            if (NewsManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(NEWS_PATTERN_KEY);
            }

            return news;
        }

        /// <summary>
        /// Gets a news comment
        /// </summary>
        /// <param name="NewsCommentID">News comment identifer</param>
        /// <returns>News comment</returns>
        public static NewsComment GetNewsCommentByID(int NewsCommentID)
        {
            if (NewsCommentID == 0)
                return null;

            DBNewsComment dbItem = DBProviderManager<DBNewsProvider>.Provider.GetNewsCommentByID(NewsCommentID);
            NewsComment newsComment = DBMapping(dbItem);
            return newsComment;
        }

        /// <summary>
        /// Gets a news comment collection by news identifier
        /// </summary>
        /// <param name="NewsID">The news identifier</param>
        /// <returns>News comment collection</returns>
        public static NewsCommentCollection GetNewsCommentsByNewsID(int NewsID)
        {
            DBNewsCommentCollection dbCollection = DBProviderManager<DBNewsProvider>.Provider.GetNewsCommentsByNewsID(NewsID);
            NewsCommentCollection collection = DBMapping(dbCollection);
            return collection;
        }

        /// <summary>
        /// Deletes a news comment
        /// </summary>
        /// <param name="NewsCommentID">The news comment identifier</param>
        public static void DeleteNewsComment(int NewsCommentID)
        {
            DBProviderManager<DBNewsProvider>.Provider.DeleteNewsComment(NewsCommentID);
        }

        /// <summary>
        /// Gets all news comments
        /// </summary>
        /// <returns>News comment collection</returns>
        public static NewsCommentCollection GetAllNewsComments()
        {
            DBNewsCommentCollection dbCollection = DBProviderManager<DBNewsProvider>.Provider.GetAllNewsComments();
            NewsCommentCollection collection = DBMapping(dbCollection);
            return collection;
        }

        /// <summary>
        /// Inserts a news comment
        /// </summary>
        /// <param name="NewsID">The news identifier</param>
        /// <param name="CustomerID">The customer identifier</param>
        /// <param name="Title">The title</param>
        /// <param name="Comment">The comment</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>News comment</returns>
        public static NewsComment InsertNewsComment(int NewsID, int CustomerID, string Title,
            string Comment, DateTime CreatedOn)
        {
            return InsertNewsComment(NewsID, CustomerID, Title, Comment, CreatedOn, NewsManager.NotifyAboutNewNewsComments);
        }

        /// <summary>
        /// Inserts a news comment
        /// </summary>
        /// <param name="NewsID">The news identifier</param>
        /// <param name="CustomerID">The customer identifier</param>
        /// <param name="Title">The title</param>
        /// <param name="Comment">The comment</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="notify">A value indicating whether to notify the store owner</param>
        /// <returns>News comment</returns>
        public static NewsComment InsertNewsComment(int NewsID, int CustomerID, string Title,
            string Comment, DateTime CreatedOn, bool notify)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);

            DBNewsComment dbItem = DBProviderManager<DBNewsProvider>.Provider.InsertNewsComment(NewsID, CustomerID, Title,
                Comment, CreatedOn);
            NewsComment newsComment = DBMapping(dbItem);
            
            if (notify)
            {
                MessageManager.SendNewsCommentNotificationMessage(newsComment, LocalizationManager.DefaultAdminLanguage.LanguageID);
            }

            return newsComment;
        }

        /// <summary>
        /// Updates the news comment
        /// </summary>
        /// <param name="NewsCommentID">The news comment identifier</param>
        /// <param name="NewsID">The news identifier</param>
        /// <param name="CustomerID">The customer identifier</param>
        /// <param name="Title">The title</param>
        /// <param name="Comment">The comment</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>News comment</returns>
        public static NewsComment UpdateNewsComment(int NewsCommentID, int NewsID, int CustomerID, string Title,
            string Comment, DateTime CreatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);

            DBNewsComment dbItem = DBProviderManager<DBNewsProvider>.Provider.UpdateNewsComment(NewsCommentID, NewsID, CustomerID, Title,
                Comment, CreatedOn);
            NewsComment newsComment = DBMapping(dbItem);
            return newsComment;
        }

        /// <summary>
        /// Formats the text
        /// </summary>
        /// <param name="Text">Text</param>
        /// <returns>Formatted text</returns>
        public static string FormatCommentText(string Text)
        {
            if (String.IsNullOrEmpty(Text))
                return string.Empty;

            Text = HtmlHelper.FormatText(Text, false, true, false, false, false, false);
            return Text;
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
                return SettingManager.GetSettingValueBoolean("Cache.NewsManager.CacheEnabled");
            }
        }
        
        /// <summary>
        /// Gets or sets a value indicating whether news are enabled
        /// </summary>
        public static bool NewsEnabled
        {
            get
            {
                bool newsEnabled = SettingManager.GetSettingValueBoolean("News.NewsEnabled");
                return newsEnabled;
            }
            set
            {
                SettingManager.SetParam("News.NewsEnabled", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether not registered user can leave comments
        /// </summary>
        public static bool AllowNotRegisteredUsersToLeaveComments
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("News.AllowNotRegisteredUsersToLeaveComments");
            }
            set
            {
                SettingManager.SetParam("News.AllowNotRegisteredUsersToLeaveComments", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to notify about new news comments
        /// </summary>
        public static bool NotifyAboutNewNewsComments
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("News.NotifyAboutNewNewsComments");
            }
            set
            {
                SettingManager.SetParam("News.NotifyAboutNewNewsComments", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show news on the main page
        /// </summary>
        public static bool ShowNewsOnMainPage
        {
            get
            {
                bool showNewsOnMainPage = SettingManager.GetSettingValueBoolean("Display.ShowNewsOnMainPage");
                return showNewsOnMainPage;
            }
            set
            {
                SettingManager.SetParam("Display.ShowNewsOnMainPage", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating news count displayed on the main page
        /// </summary>
        public static int MainPageNewsCount
        {
            get
            {
                int mainPageNewsCount = SettingManager.GetSettingValueInteger("Display.MainPageNewsCount");
                return mainPageNewsCount;
            }
            set
            {
                SettingManager.SetParam("Display.MainPageNewsCount", value.ToString());
            }
        }
        #endregion
    }
}
