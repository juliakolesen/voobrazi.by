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

using System.ComponentModel;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Content.Forums;

namespace NopSolutions.NopCommerce.Web
{
    /// <summary>
    /// Forum helper
    /// </summary>
    public partial class ForumHelper
    {
        #region Methods

        public static int GetCurrentUserSentPrivateMessagesCount()
        {
            int totalRecords = 0;
            GetCurrentUserSentPrivateMessages(0, 1, out totalRecords);
            return totalRecords;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static PrivateMessageCollection GetCurrentUserSentPrivateMessages(int StartIndex, int PageSize)
        {
            int totalRecord = 0;
            return GetCurrentUserSentPrivateMessages(StartIndex, PageSize, out totalRecord);
        }

        public static PrivateMessageCollection GetCurrentUserSentPrivateMessages(int StartIndex, int PageSize, out int totalRecords)
        {
            PrivateMessageCollection result = new PrivateMessageCollection();
            if (PageSize <= 0)
                PageSize = 10;
            if (PageSize == int.MaxValue)
                PageSize = int.MaxValue - 1;

            int PageIndex = StartIndex / PageSize;

            totalRecords = 0;

            if (NopContext.Current.User == null)
                return result;

            result = ForumManager.GetAllPrivateMessages(NopContext.Current.User.CustomerID, 0, null, false, null,
                string.Empty, PageSize, PageIndex, out totalRecords);
            return result;
        }

        public static int GetCurrentUserInboxPrivateMessagesCount()
        {
            int totalRecords = 0;
            GetCurrentUserInboxPrivateMessages(0, 1, out totalRecords);
            return totalRecords;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static PrivateMessageCollection GetCurrentUserInboxPrivateMessages(int StartIndex, int PageSize)
        {
            int totalRecord = 0;
            return GetCurrentUserInboxPrivateMessages(StartIndex, PageSize, out totalRecord);
        }

        public static PrivateMessageCollection GetCurrentUserInboxPrivateMessages(int StartIndex, int PageSize, out int totalRecords)
        {
            PrivateMessageCollection result = new PrivateMessageCollection();
            if (PageSize <= 0)
                PageSize = 10;
            if (PageSize == int.MaxValue)
                PageSize = int.MaxValue - 1;

            int PageIndex = StartIndex / PageSize;

            totalRecords = 0;

            if (NopContext.Current.User == null)
                return result;

            result = ForumManager.GetAllPrivateMessages(0, NopContext.Current.User.CustomerID, null, null, false,
                string.Empty, PageSize, PageIndex, out totalRecords);
            return result;
        }
        #endregion
    }
}
