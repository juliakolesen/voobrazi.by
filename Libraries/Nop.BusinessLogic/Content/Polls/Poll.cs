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
using System.Text;
using NopSolutions.NopCommerce.BusinessLogic.Directory;



namespace NopSolutions.NopCommerce.BusinessLogic.Content.Polls
{
    /// <summary>
    /// Represents a poll
    /// </summary>
    public partial class Poll : BaseEntity
    {
        #region Ctor
        /// <summary>
        /// Creates a new instance of the Poll class
        /// </summary>
        public Poll()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the poll identifier
        /// </summary>
        public int PollID { get; set; }

        /// <summary>
        /// Gets or sets the language identifier
        /// </summary>
        public int LanguageID { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Gets or sets the system keyword
        /// </summary>
        public string SystemKeyword { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether the entity is published
        /// </summary>
        public bool Published { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

        #endregion

        #region Custom Properties
        /// <summary>
        /// Gets the language
        /// </summary>
        public Language Language
        {
            get
            {
                return LanguageManager.GetLanguageByID(LanguageID);
            }
        }

        /// <summary>
        /// Gets the poll total vote record count
        /// </summary>
        public int TotalVotes
        {
            get
            {
                int result = 0;
                PollAnswerCollection pollAnswers = this.PollAnswers;
                foreach (PollAnswer pollAnswer in pollAnswers)
                    result += pollAnswer.Count;
                return result;
            }
        }
        /// <summary>
        /// Gets the poll answers
        /// </summary>
        public PollAnswerCollection PollAnswers
        {
            get
            {
                return PollManager.GetPollAnswersByPollID(PollID);
            }
        }

        #endregion
    }

}
