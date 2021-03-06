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
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Content.Topics;

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class TopicInfoControl : BaseNopAdministrationUserControl
    {
        private void BindData()
        {
            Topic topic = TopicManager.GetTopicByID(this.TopicID);
            if (topic != null)
            {
                this.txtName.Text = topic.Name;
                this.txtMetaDescription.Text = topic.MetaDescription ?? String.Empty;
                this.txtMetaKeywords.Text = topic.MetaKeywords ?? String.Empty;
                this.txtMetaTitle.Text = topic.MetaTitle ?? String.Empty;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.BindData();
            }
        }

        public Topic SaveInfo()
        {
            Topic topic = TopicManager.GetTopicByID(this.TopicID);
            if (topic != null)
            {
                topic = TopicManager.UpdateTopic(topic.TopicID, txtName.Text, txtMetaKeywords.Text, txtMetaDescription.Text, txtMetaTitle.Text);
            }
            else
            {
                topic = TopicManager.InsertTopic(txtName.Text, txtMetaKeywords.Text, txtMetaDescription.Text, txtMetaTitle.Text);
            }

            return topic;
        }

        public int TopicID
        {
            get
            {
                return CommonHelper.QueryStringInt("TopicID");
            }
        }
    }
}