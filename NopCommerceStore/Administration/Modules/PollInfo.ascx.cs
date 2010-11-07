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
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic.Content.Polls;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Web.Administration.Modules;

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class PollInfoControl : BaseNopAdministrationUserControl
    {
        private void FillDropDowns()
        {
            this.ddlLanguage.Items.Clear();
            LanguageCollection languages = LanguageManager.GetAllLanguages();
            foreach (Language language in languages)
            {
                ListItem item2 = new ListItem(language.Name, language.LanguageID.ToString());
                this.ddlLanguage.Items.Add(item2);
            }
        }

        private void BindData()
        {
            Poll poll = PollManager.GetPollByID(this.PollID);
            if (poll != null)
            {
                CommonHelper.SelectListItem(this.ddlLanguage, poll.LanguageID);
                this.txtName.Text = poll.Name;
                this.txtSystemKeyword.Text = poll.SystemKeyword;
                this.cbPublished.Checked = poll.Published;
                this.txtDisplayOrder.Value = poll.DisplayOrder;

                pnlPollAnswers.Visible = true;
                PollAnswerCollection pollAnswers = poll.PollAnswers;
                gvPollAnswers.DataSource = pollAnswers;
                gvPollAnswers.DataBind();
            }
            else
            {
                pnlPollAnswers.Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.FillDropDowns();
                this.BindData();
            }
        }

        protected void btnAddPollAnswer_Click(object sender, EventArgs e)
        {
            try
            {
                PollAnswer pollAnswer = PollManager.InsertPollAnswer(this.PollID,
                    txtPollAnswerName.Text, 0, txtPollAnswerDisplayOrder.Value);

                BindData();
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        protected void gvPollAnswers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UpdatePollAnswer")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvPollAnswers.Rows[index];

                HiddenField hfPollAnswerID = row.FindControl("hfPollAnswerID") as HiddenField;
                SimpleTextBox txtName = row.FindControl("txtName") as SimpleTextBox;
                NumericTextBox txtDisplayOrder = row.FindControl("txtDisplayOrder") as NumericTextBox;

                int pollAnswerID = int.Parse(hfPollAnswerID.Value);
                PollAnswer pollAnswer = PollManager.GetPollAnswerByID(pollAnswerID);

                if (pollAnswer != null)
                    pollAnswer = PollManager.UpdatePoll(pollAnswer.PollAnswerID, pollAnswer.PollID,
                       txtName.Text, pollAnswer.Count, txtDisplayOrder.Value);

                BindData();
            }
        }

        protected void gvPollAnswers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                PollAnswer pollAnswer = (PollAnswer)e.Row.DataItem;

                Button btnUpdate = e.Row.FindControl("btnUpdate") as Button;
                if (btnUpdate != null)
                    btnUpdate.CommandArgument = e.Row.RowIndex.ToString();
            }
        }

        protected void gvPollAnswers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int pollAnswerID = (int)gvPollAnswers.DataKeys[e.RowIndex]["PollAnswerID"];
            PollAnswer pollAnswer = PollManager.GetPollAnswerByID(pollAnswerID);
            if (pollAnswer != null)
            {
                PollManager.DeletePollAnswer(pollAnswer.PollAnswerID);
                BindData();
            }
        }

        public Poll SaveInfo()
        {
            Poll poll = PollManager.GetPollByID(this.PollID);

            if (poll != null)
            {
                poll = PollManager.UpdatePoll(poll.PollID, int.Parse(this.ddlLanguage.SelectedItem.Value),
                    txtName.Text, txtSystemKeyword.Text, cbPublished.Checked, txtDisplayOrder.Value);
            }
            else
            {
                poll = PollManager.InsertPoll(int.Parse(this.ddlLanguage.SelectedItem.Value),
                txtName.Text, txtSystemKeyword.Text, cbPublished.Checked, txtDisplayOrder.Value);
            }
            return poll;
        }

        public int PollID
        {
            get
            {
                return CommonHelper.QueryStringInt("PollID");
            }
        }
    }
}