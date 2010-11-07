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
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.Content.Topics;


namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class TopicPageControl : BaseNopUserControl
    {
        protected override void OnInit(EventArgs e)
        {
            BindData();
            base.OnInit(e);
        }

        private void BindData()
        {
            LocalizedTopic localizedTopic =
                TopicManager.GetLocalizedTopic(this.TopicID, NopContext.Current.WorkingLanguage.LanguageID);
            if (localizedTopic != null)
            {
                if (!string.IsNullOrEmpty(localizedTopic.Title))
                {
                    lTitle.Text = Server.HtmlEncode(localizedTopic.Title);
                }
                else
                {
                    lTitle.Visible = false;
                }
                if (!string.IsNullOrEmpty(localizedTopic.Body))
                {
                    lBody.Text = localizedTopic.Body;
                }
                else
                {
                    lBody.Visible = false;
                }
            }
            else
                Response.Redirect("~/Default.aspx");
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
