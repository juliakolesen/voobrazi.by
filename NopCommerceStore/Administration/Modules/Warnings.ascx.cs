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
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Measures;
using NopSolutions.NopCommerce.BusinessLogic.Messages;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Content.Topics;
 

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class WarningsControl : BaseNopAdministrationUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }

        protected void BindData()
        {
            StringBuilder warningResult = new StringBuilder();
            if (CurrencyManager.PrimaryExchangeRateCurrency == null)
            {
                warningResult.Append("Primary exchange rate currency is not set. <a href=\"Currencies.aspx\">Set now</a>");
                warningResult.Append("<br />");
                warningResult.Append("<br />");
            }

            if (CurrencyManager.PrimaryStoreCurrency == null)
            {
                warningResult.Append("Primary store currency is not set. <a href=\"Currencies.aspx\">Set now</a>");
                warningResult.Append("<br />");
                warningResult.Append("<br />");
            }

            if (MeasureManager.BaseWeightIn == null)
            {
                warningResult.Append("The weight that will can used as a default is not set. <a href=\"GlobalSettings.aspx\">Set now</a>");
                warningResult.Append("<br />");
                warningResult.Append("<br />");
            }

            if (MeasureManager.BaseDimensionIn == null)
            {
                warningResult.Append("The dimension that will can used as a default is not set. <a href=\"GlobalSettings.aspx\">Set now</a>");
                warningResult.Append("<br />");
                warningResult.Append("<br />");
            }

            LanguageCollection publishedLanguages = LanguageManager.GetAllLanguages(false);
            
            foreach (MessageTemplate messageTemplate in MessageManager.GetAllMessageTemplates())
            {
                foreach (Language language in publishedLanguages)
                {
                    LocalizedMessageTemplate localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(messageTemplate.Name, language.LanguageID);
                    if (localizedMessageTemplate == null)
                    {
                        warningResult.AppendFormat("You don't have localized version of message template [{0}] for {1}. <a href=\"MessageTemplates.aspx\">Create it now</a>", messageTemplate.Name, language.Name);
                        warningResult.Append("<br />");
                        warningResult.Append("<br />");
                    }
                }
            }

            foreach (Topic topic in TopicManager.GetAllTopics())
            {
                foreach (Language language in publishedLanguages)
                {
                    LocalizedTopic localizedTopic = TopicManager.GetLocalizedTopic(topic.Name, language.LanguageID);
                    if (localizedTopic == null)
                    {
                        warningResult.AppendFormat("You don't have localized version of topic [{0}] for {1}. <a href=\"Topics.aspx\">Create it now</a>", topic.Name, language.Name);
                        warningResult.Append("<br />");
                        warningResult.Append("<br />");
                    }
                }
            }

            string warnings =  warningResult.ToString();
            if (!String.IsNullOrEmpty(warnings))
            {
                lblWarnings.Text = warnings;
            }
            else
                this.Visible = false;
        }
    }
}