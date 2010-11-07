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
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Messages;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class MessageQueueDetailsControl : BaseNopAdministrationUserControl
    {
        private void BindData()
        {
            QueuedEmail queuedEmail = MessageManager.GetQueuedEmailByID(this.QueuedEmailID);
            if (queuedEmail != null)
            {
                this.txtPriority.Value = queuedEmail.Priority;
                this.txtFrom.Text = queuedEmail.From;
                this.txtFromName.Text = queuedEmail.FromName;
                this.txtTo.Text = queuedEmail.To;
                this.txtToName.Text = queuedEmail.ToName;
                this.txtCc.Text = queuedEmail.Cc;
                this.txtBcc.Text = queuedEmail.Bcc;
                this.txtSubject.Text = queuedEmail.Subject;
                this.txtBody.Value = queuedEmail.Body;
                this.lblCreatedOn.Text = DateTimeHelper.ConvertToUserTime(queuedEmail.CreatedOn).ToString();
                this.txtSendTries.Value = queuedEmail.SendTries;
                this.lblSentOn.Text = GetSentOnInfo(queuedEmail);
            }
            else
                Response.Redirect("MessageQueue.aspx");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.BindData();
            }
        }

        protected void RequeueButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    QueuedEmail queuedEmail = MessageManager.GetQueuedEmailByID(this.QueuedEmailID);
                    if (queuedEmail != null)
                    {
                        QueuedEmail requeuedEmail = MessageManager.InsertQueuedEmail(txtPriority.Value, 
                            txtFrom.Text, txtFromName.Text,
                            txtTo.Text, txtToName.Text, txtCc.Text, txtBcc.Text,
                            txtSubject.Text, txtBody.Value, DateTime.Now,
                            0, null);
                        Response.Redirect("MessageQueueDetails.aspx?QueuedEmailID=" + requeuedEmail.QueuedEmailID.ToString());
                    }
                    else
                        Response.Redirect("MessageQueue.aspx");
                }
                catch (Exception exc)
                {
                    ProcessException(exc);
                }
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    QueuedEmail queuedEmail = MessageManager.GetQueuedEmailByID(this.QueuedEmailID);
                    if (queuedEmail != null)
                    {
                        queuedEmail = MessageManager.UpdateQueuedEmail(queuedEmail.QueuedEmailID,
                           txtPriority.Value, txtFrom.Text, txtFromName.Text,
                           txtTo.Text, txtToName.Text, txtCc.Text, txtBcc.Text,
                           txtSubject.Text, txtBody.Value, queuedEmail.CreatedOn,
                           txtSendTries.Value, queuedEmail.SentOn);
                        Response.Redirect("MessageQueueDetails.aspx?QueuedEmailID=" + queuedEmail.QueuedEmailID.ToString());
                    }
                    else
                        Response.Redirect("MessageQueue.aspx");
                }
                catch (Exception exc)
                {
                    ProcessException(exc);
                }
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                MessageManager.DeleteQueuedEmail(this.QueuedEmailID);
                Response.Redirect("MessageQueue.aspx");
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        protected string GetSentOnInfo(QueuedEmail queuedEmail)
        {
            if (!queuedEmail.SentOn.HasValue)
                return "Not sent yet";
            else
                return string.Format("Sent on {0}", DateTimeHelper.ConvertToUserTime(queuedEmail.SentOn.Value));
        }

        public int QueuedEmailID
        {
            get
            {
                return CommonHelper.QueryStringInt("QueuedEmailID");
            }
        }
    }
}