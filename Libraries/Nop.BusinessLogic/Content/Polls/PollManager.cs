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
using NopSolutions.NopCommerce.BusinessLogic.Caching;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Content.Polls;

namespace NopSolutions.NopCommerce.BusinessLogic.Content.Polls
{
    /// <summary>
    /// Poll manager
    /// </summary>
    public partial class PollManager
    {
        #region Constants
        private const string POLLS_BY_ID_KEY = "Nop.polls.id-{0}";
        private const string POLLANSWERS_BY_POLLID_KEY = "Nop.pollanswers.pollid-{0}";
        private const string POLLS_PATTERN_KEY = "Nop.polls.";
        private const string POLLANSWERS_PATTERN_KEY = "Nop.pollanswers.";
        #endregion

        #region Utilities
        private static PollCollection DBMapping(DBPollCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            PollCollection collection = new PollCollection();
            foreach (DBPoll dbItem in dbCollection)
            {
                Poll item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static Poll DBMapping(DBPoll dbItem)
        {
            if (dbItem == null)
                return null;

            Poll item = new Poll();
            item.PollID = dbItem.PollID;
            item.LanguageID = dbItem.LanguageID;
            item.Name = dbItem.Name;
            item.SystemKeyword = dbItem.SystemKeyword;
            item.Published = dbItem.Published;
            item.DisplayOrder = dbItem.DisplayOrder;

            return item;
        }

        private static PollAnswerCollection DBMapping(DBPollAnswerCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            PollAnswerCollection collection = new PollAnswerCollection();
            foreach (DBPollAnswer dbItem in dbCollection)
            {
                PollAnswer item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static PollAnswer DBMapping(DBPollAnswer dbItem)
        {
            if (dbItem == null)
                return null;

            PollAnswer item = new PollAnswer();
            item.PollAnswerID = dbItem.PollAnswerID;
            item.PollID = dbItem.PollID;
            item.Name = dbItem.Name;
            item.Count = dbItem.Count;
            item.DisplayOrder = dbItem.DisplayOrder;

            return item;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets a poll
        /// </summary>
        /// <param name="PollID">The poll identifier</param>
        /// <returns>Poll</returns>
        public static Poll GetPollByID(int PollID)
        {
            if (PollID == 0)
                return null;

            string key = string.Format(POLLS_BY_ID_KEY, PollID);
            object obj2 = NopCache.Get(key);
            if (PollManager.CacheEnabled && (obj2 != null))
            {
                return (Poll)obj2;
            }
            
            DBPoll dbItem = DBProviderManager<DBPollProvider>.Provider.GetPollByID(PollID);
            Poll poll = DBMapping(dbItem);

            if (PollManager.CacheEnabled)
            {
                NopCache.Max(key, poll);
            }
            return poll;
        }

        /// <summary>
        /// Gets a poll
        /// </summary>
        /// <param name="SystemKeyword">The poll system keyword</param>
        /// <returns>Poll</returns>
        public static Poll GetPollBySystemKeyword(string SystemKeyword)
        {
            DBPoll dbItem = DBProviderManager<DBPollProvider>.Provider.GetPollBySystemKeyword(SystemKeyword);
            Poll poll = DBMapping(dbItem);
            return poll;
        }

        /// <summary>
        /// Gets poll collection
        /// </summary>
        /// <param name="LanguageID">Language identifier. 0 if you want to get all news</param>
        /// <param name="PollCount">Poll count to load. 0 if you want to get all polls</param>
        /// <returns>Poll collection</returns>
        public static PollCollection GetPolls(int LanguageID, int PollCount)
        {
            bool showHidden = NopContext.Current.IsAdmin;
            DBPollCollection dbCollection = DBProviderManager<DBPollProvider>.Provider.GetPolls(LanguageID, PollCount, showHidden);
            PollCollection collection = DBMapping(dbCollection);
            return collection;
        }

        /// <summary>
        /// Deletes a poll
        /// </summary>
        /// <param name="PollID">The poll identifier</param>
        public static void DeletePoll(int PollID)
        {
            DBProviderManager<DBPollProvider>.Provider.DeletePoll(PollID);
            if (PollManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(POLLS_PATTERN_KEY);
                NopCache.RemoveByPattern(POLLANSWERS_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets all polls
        /// </summary>
        /// <param name="LanguageID">Language identifier. 0 if you want to get all news</param>
        /// <returns>Poll collection</returns>
        public static PollCollection GetAllPolls(int LanguageID)
        {
            return GetPolls(LanguageID, 0);
        }

        /// <summary>
        /// Inserts a poll
        /// </summary>
        /// <param name="LanguageID">The language identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="SystemKeyword">The system keyword</param>
        /// <param name="Published">A value indicating whether the entity is published</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Poll</returns>
        public static Poll InsertPoll(int LanguageID, string Name, string SystemKeyword, 
            bool Published, int DisplayOrder)
        {
            DBPoll dbItem = DBProviderManager<DBPollProvider>.Provider.InsertPoll(LanguageID, Name,
                SystemKeyword, Published, DisplayOrder);
            Poll poll = DBMapping(dbItem);

            if (PollManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(POLLS_PATTERN_KEY);
                NopCache.RemoveByPattern(POLLANSWERS_PATTERN_KEY);
            }

            return poll;
        }

        /// <summary>
        /// Updates the poll
        /// </summary>
        /// <param name="PollID">The poll identifier</param>
        /// <param name="LanguageID">The language identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="SystemKeyword">The system keyword</param>
        /// <param name="Published">A value indicating whether the entity is published</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Poll</returns>
        public static Poll UpdatePoll(int PollID, int LanguageID, string Name, string SystemKeyword, 
            bool Published, int DisplayOrder)
        {
            DBPoll dbItem = DBProviderManager<DBPollProvider>.Provider.UpdatePoll(PollID, LanguageID, Name, 
                SystemKeyword, Published, DisplayOrder);
            Poll poll = DBMapping(dbItem);

            if (PollManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(POLLS_PATTERN_KEY);
                NopCache.RemoveByPattern(POLLANSWERS_PATTERN_KEY);
            }

            return poll;
        }

        /// <summary>
        /// Is voting record already exists
        /// </summary>
        /// <param name="PollID">Poll identifier</param>
        /// <param name="CustomerID">Customer identifier</param>
        /// <returns>Poll</returns>
        public static bool PollVotingRecordExists(int PollID, int CustomerID)
        {
            return DBProviderManager<DBPollProvider>.Provider.PollVotingRecordExists(PollID, CustomerID);
        }

        /// <summary>
        /// Gets a poll answer
        /// </summary>
        /// <param name="PollAnswerID">Poll answer identifier</param>
        /// <returns>Poll answer</returns>
        public static PollAnswer GetPollAnswerByID(int PollAnswerID)
        {
            if (PollAnswerID == 0)
                return null;

            DBPollAnswer dbItem = DBProviderManager<DBPollProvider>.Provider.GetPollAnswerByID(PollAnswerID);
            PollAnswer pollAnswer = DBMapping(dbItem);
            return pollAnswer;
        }

        /// <summary>
        /// Gets a poll answers by poll identifier
        /// </summary>
        /// <param name="PollID">Poll identifier</param>
        /// <returns>Poll answer collection</returns>
        public static PollAnswerCollection GetPollAnswersByPollID(int PollID)
        {
            string key = string.Format(POLLANSWERS_BY_POLLID_KEY, PollID);
            object obj2 = NopCache.Get(key);
            if (PollManager.CacheEnabled && (obj2 != null))
            {
                return (PollAnswerCollection)obj2;
            }

            DBPollAnswerCollection dbCollection = DBProviderManager<DBPollProvider>.Provider.GetPollAnswersByPollID(PollID);
            PollAnswerCollection pollAnswerCollection = DBMapping(dbCollection);

            if (PollManager.CacheEnabled)
            {
                NopCache.Max(key, pollAnswerCollection);
            }
            return pollAnswerCollection;
        }

        /// <summary>
        /// Deletes a poll answer
        /// </summary>
        /// <param name="PollAnswerID">Poll answer identifier</param>
        public static void DeletePollAnswer(int PollAnswerID)
        {
            DBProviderManager<DBPollProvider>.Provider.DeletePollAnswer(PollAnswerID);
            if (PollManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(POLLS_PATTERN_KEY);
                NopCache.RemoveByPattern(POLLANSWERS_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Inserts a poll answer
        /// </summary>
        /// <param name="PollID">The poll identifier</param>
        /// <param name="Name">The poll answer name</param>
        /// <param name="Count">The current count</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Poll answer</returns>
        public static PollAnswer InsertPollAnswer(int PollID, string Name, int Count, int DisplayOrder)
        {
            DBPollAnswer dbItem = DBProviderManager<DBPollProvider>.Provider.InsertPollAnswer(PollID, Name, Count, DisplayOrder);
            PollAnswer pollAnswer = DBMapping(dbItem);

            if (PollManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(POLLS_PATTERN_KEY);
                NopCache.RemoveByPattern(POLLANSWERS_PATTERN_KEY);
            }

            return pollAnswer;
        }

        /// <summary>
        /// Updates the poll answer
        /// </summary>
        /// <param name="PollAnswerID">The poll answer identifier</param>
        /// <param name="PollID">The poll identifier</param>
        /// <param name="Name">The poll answer name</param>
        /// <param name="Count">The current count</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Poll answer</returns>
        public static PollAnswer UpdatePoll(int PollAnswerID, int PollID, string Name, int Count, int DisplayOrder)
        {
            DBPollAnswer dbItem = DBProviderManager<DBPollProvider>.Provider.UpdatePollAnswer(PollAnswerID, PollID, Name, Count, DisplayOrder);
            PollAnswer pollAnswer = DBMapping(dbItem);

            if (PollManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(POLLS_PATTERN_KEY);
                NopCache.RemoveByPattern(POLLANSWERS_PATTERN_KEY);
            }

            return pollAnswer;
        }

        /// <summary>
        /// Creates a poll voting record
        /// </summary>
        /// <param name="PollAnswerID">The poll answer identifier</param>
        /// <param name="CustomerID">Customer identifer</param>
        public static void CreatePollVotingRecord(int PollAnswerID, int CustomerID)
        {
            DBProviderManager<DBPollProvider>.Provider.CreatePollVotingRecord(PollAnswerID, CustomerID);
            if (PollManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(POLLS_PATTERN_KEY);
                NopCache.RemoveByPattern(POLLANSWERS_PATTERN_KEY);
            }
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
                return SettingManager.GetSettingValueBoolean("Cache.PollManager.CacheEnabled");
            }
        }
        #endregion
    }
}
