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
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Content.Polls;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.Common.Utils;
 
 

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class PollControl : BaseNopUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                rfvPollAnswers.ValidationGroup = string.Format("Poll.{0}", this.PollID);
                btnSubmitVoteRecord.ValidationGroup = string.Format("Poll.{0}", this.PollID);
                Customer customer = NopContext.Current.User;
                bool showResults = false;
                if (customer != null)
                    showResults = PollManager.PollVotingRecordExists(this.PollID, customer.CustomerID);
            
                BindData(showResults);
            }
        }

        protected void BindData(bool showResults)
        {
            Poll poll = PollManager.GetPollByID(this.PollID);
            if (poll != null && poll.Published)
            {
                lblPollName.Text = Server.HtmlEncode(poll.Name);
                lblTotalVotes.Text = string.Format(GetLocaleResourceString("Polls.TotalVotes"), poll.TotalVotes);

                PollAnswerCollection pollAnswers = poll.PollAnswers;
                pnlTakePoll.Visible = !showResults;
                pnlPollResults.Visible = showResults;
                if (showResults)
                {
                    dlResults.DataSource = pollAnswers;
                    dlResults.DataBind();
                }
                else
                {
                    rblPollAnswers.DataSource = pollAnswers;
                    rblPollAnswers.DataBind();
                }
            }
            else
            {
                pnlTakePoll.Visible = false;
                pnlPollResults.Visible = false;
            }
        }

        protected void btnSubmitVoteRecord_Click(object sender, EventArgs e)
        {
            Customer customer = NopContext.Current.User;
            if (rblPollAnswers.SelectedItem != null && customer != null && !customer.IsGuest)
            {
                int pollAnswerID = Convert.ToInt32(rblPollAnswers.SelectedItem.Value);
                if (!PollManager.PollVotingRecordExists(this.PollID, customer.CustomerID))
                    PollManager.CreatePollVotingRecord(pollAnswerID, customer.CustomerID);
                BindData(true);
            }
            else
            {
                string loginURL = CommonHelper.GetLoginPageURL(true);
                Response.Redirect(loginURL);
            }
        }

        protected void dlResults_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            Image imgPercentage = (Image)e.Item.FindControl("imgPercentage");
            Label lblPercentage = (Label)e.Item.FindControl("lblPercentage");

            Poll poll = PollManager.GetPollByID(this.PollID);
            if (poll != null)
            {
                int pollAnswerVoteCount = (int)DataBinder.Eval(e.Item.DataItem, "Count");
                int pollTotalVoteCount = poll.TotalVotes;
                if (pollTotalVoteCount > 0)
                {
                    double pct = (Convert.ToDouble(pollAnswerVoteCount) / Convert.ToDouble(pollTotalVoteCount)) * Convert.ToDouble(100);
                    lblPercentage.Text = pct.ToString("0.0") + "%";
                    imgPercentage.Width = Unit.Percentage(pct);
                }
                else
                {
                    lblPercentage.Text = "0%";
                    imgPercentage.Visible = false;
                }
            }
        }

        public int PollID
        {
            get
            {
                if (ViewState["PollID"] == null)
                    return -1;
                else
                    return (int)ViewState["PollID"];
            }
            set { ViewState["PollID"] = value; }
        }
    }
}
