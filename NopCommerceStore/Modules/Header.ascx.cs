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
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Content.Forums;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class HeaderControl : BaseNopUserControl
    {
        protected string GetUnreadPrivateMessages()
        {
            string result = string.Empty;
            if (NopContext.Current.User != null && !NopContext.Current.User.IsGuest)
            {
                int totalRecords = 0;
                PrivateMessageCollection privateMessages = ForumManager.GetAllPrivateMessages(0,
                    NopContext.Current.User.CustomerID, false, null, false, string.Empty, 1, 0, out totalRecords);

                if (totalRecords > 0)
                {
                    result = string.Format(GetLocaleResourceString("PrivateMessages.TotalUnread"), totalRecords);
                }
            }
            return result;
        }
    }
}