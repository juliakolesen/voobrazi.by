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
using System.Configuration;
using System.Web.Hosting;
using System.Web.Configuration;
using System.Collections.Specialized;
using System.Configuration.Provider;

namespace NopSolutions.NopCommerce.DataAccess.Content.Polls
{
    /// <summary>
    /// Acts as a base class for deriving custom poll provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/PollProvider")]
    public abstract partial class DBPollProvider : BaseDBProvider
    {
        #region Methods
        /// <summary>
        /// Gets a poll
        /// </summary>
        /// <param name="PollID">The poll identifier</param>
        /// <returns>Poll</returns>
        public abstract DBPoll GetPollByID(int PollID);

        /// <summary>
        /// Gets a poll
        /// </summary>
        /// <param name="SystemKeyword">Poll system keyword</param>
        /// <returns>Poll</returns>
        public abstract DBPoll GetPollBySystemKeyword(string SystemKeyword);

        /// <summary>
        /// Gets poll collection
        /// </summary>
        /// <param name="LanguageID">Language identifier. 0 if you want to get all news</param>
        /// <param name="PollCount">Poll count to load. 0 if you want to get all polls</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Poll collection</returns>
        public abstract DBPollCollection GetPolls(int LanguageID, int PollCount, bool showHidden);

        /// <summary>
        /// Deletes a poll
        /// </summary>
        /// <param name="PollID">The poll identifier</param>
        public abstract void DeletePoll(int PollID);

        /// <summary>
        /// Inserts a poll
        /// </summary>
        /// <param name="LanguageID">The language identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="SystemKeyword">The system keyword</param>
        /// <param name="Published">A value indicating whether the entity is published</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Poll</returns>
        public abstract DBPoll InsertPoll(int LanguageID, string Name, string SystemKeyword,
            bool Published, int DisplayOrder);

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
        public abstract DBPoll UpdatePoll(int PollID, int LanguageID, string Name, string SystemKeyword, 
            bool Published, int DisplayOrder);

        /// <summary>
        /// Is voting record already exists
        /// </summary>
        /// <param name="PollID">Poll identifier</param>
        /// <param name="CustomerID">Customer identifier</param>
        /// <returns>Poll</returns>
        public abstract bool PollVotingRecordExists(int PollID, int CustomerID);

        /// <summary>
        /// Gets a poll answer
        /// </summary>
        /// <param name="PollAnswerID">Poll answer identifier</param>
        /// <returns>Poll answer</returns>
        public abstract DBPollAnswer GetPollAnswerByID(int PollAnswerID);

        /// <summary>
        /// Gets a poll answers by poll identifier
        /// </summary>
        /// <param name="PollID">Poll identifier</param>
        /// <returns>Poll answer collection</returns>
        public abstract DBPollAnswerCollection GetPollAnswersByPollID(int PollID);

        /// <summary>
        /// Deletes a poll answer
        /// </summary>
        /// <param name="PollAnswerID">Poll answer identifier</param>
        public abstract void DeletePollAnswer(int PollAnswerID);

        /// <summary>
        /// Inserts a poll answer
        /// </summary>
        /// <param name="PollID">The poll identifier</param>
        /// <param name="Name">The poll answer name</param>
        /// <param name="Count">The current count</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Poll answer</returns>
        public abstract DBPollAnswer InsertPollAnswer(int PollID, string Name, int Count, int DisplayOrder);

        /// <summary>
        /// Updates the poll answer
        /// </summary>
        /// <param name="PollAnswerID">The poll answer identifier</param>
        /// <param name="PollID">The poll identifier</param>
        /// <param name="Name">The poll answer name</param>
        /// <param name="Count">The current count</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Poll answer</returns>
        public abstract DBPollAnswer UpdatePollAnswer(int PollAnswerID, int PollID, string Name, int Count, int DisplayOrder);

        /// <summary>
        /// Creates a poll voting record
        /// </summary>
        /// <param name="PollAnswerID">The poll answer identifier</param>
        /// <param name="CustomerID">Customer identifer</param>
        public abstract void CreatePollVotingRecord(int PollAnswerID, int CustomerID);

        #endregion
    }
}
