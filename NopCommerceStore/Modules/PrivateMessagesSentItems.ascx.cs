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
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Audit;
using NopSolutions.NopCommerce.BusinessLogic.Content.Forums;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Common.Xml;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class PrivateMessagesSentItemsControl : BaseNopUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (gvSent.Rows.Count > 0)
            {
                btnDeleteSelected.Visible = true;
            }
            else
            {
                btnDeleteSelected.Visible = false;
            }
            base.OnPreRender(e);
        }

        protected string GetToInfo(int CustomerID)
        {
            string customerInfo = string.Empty;
            Customer customer = CustomerManager.GetCustomerByID(CustomerID);
            if (customer != null && !customer.IsGuest)
            {
                if (CustomerManager.AllowViewingProfiles)
                {
                    customerInfo = string.Format("<a href=\"{0}\">{1}</a>", SEOHelper.GetUserProfileURL(customer.CustomerID), Server.HtmlEncode(CustomerManager.FormatUserName(customer)));
                }
                else
                {
                    customerInfo = Server.HtmlEncode(CustomerManager.FormatUserName(customer));
                }           
            }
            return customerInfo; 
        }

        protected string GetSubjectInfo(PrivateMessage pm)
        {
            string result = string.Empty;
            string subjectInfo = Server.HtmlEncode(pm.Subject);
            result = string.Format("<a href=\"{0}ViewPM.aspx?PM={1}\">{2}</a>", CommonHelper.GetStoreLocation(), pm.PrivateMessageID, subjectInfo);
            return result;
        }

        protected void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    foreach (GridViewRow row in gvSent.Rows)
                    {
                        CheckBox cbSelect = row.FindControl("cbSelect") as CheckBox;
                        HiddenField hfPrivateMessageID = row.FindControl("hfPrivateMessageID") as HiddenField;
                        if (cbSelect != null && hfPrivateMessageID != null)
                        {
                            bool selected = cbSelect.Checked;
                            int pmID = int.Parse(hfPrivateMessageID.Value);
                            if (selected)
                            {
                                PrivateMessage pm = ForumManager.GetPrivateMessageByID(pmID);
                                if (pm != null)
                                {
                                    if (pm.FromUserID == NopContext.Current.User.CustomerID)
                                    {
                                        pm = ForumManager.UpdatePrivateMessage(pm.PrivateMessageID, pm.FromUserID, pm.ToUserID,
                                            pm.Subject, pm.Text, pm.IsRead, true, pm.IsDeletedByRecipient, pm.CreatedOn);
                                    }
                                }
                            }
                        }
                    }

                    Response.Redirect(CommonHelper.GetStoreLocation() + "PrivateMessages.aspx?Tab=sent");
                }
                catch (Exception exc)
                {
                    LogManager.InsertLog(LogTypeEnum.CustomerError, exc.Message, exc);
                }
            }
        }
    }
}