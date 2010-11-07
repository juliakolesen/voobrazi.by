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
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class LanguageSelectorControl : BaseNopUserControl
    {
        private void BindLanguages()
        {
            LanguageCollection languages = LanguageManager.GetAllLanguages();
            if (languages.Count > 1)
            {
                this.Visible = true;
                this.ddlLanguages.Items.Clear();
                Language customerLanguage = NopContext.Current.WorkingLanguage;
                foreach (Language language in languages)
                {
                    ListItem item = new ListItem(language.Name, language.LanguageID.ToString());
                    this.ddlLanguages.Items.Add(item);
                }
                if (customerLanguage != null)
                    CommonHelper.SelectListItem(this.ddlLanguages, customerLanguage.LanguageID);
            }
            else
                this.Visible = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindLanguages();
            }
        }

        protected void ddlLanguages_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            int languagesID = int.Parse(this.ddlLanguages.SelectedItem.Value);
            Language language = LanguageManager.GetLanguageByID(languagesID);
            if (language != null && language.Published)
            {
                NopContext.Current.WorkingLanguage = language;
                CommonHelper.ReloadCurrentPage();
            }
        }
    }
}
