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
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Localization;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class LocaleStringResourceInfoControl : BaseNopAdministrationUserControl
    {
        private void BindData()
        {
            LocaleStringResource localeStringResource = LocaleStringResourceManager.GetLocaleStringResourceByID(this.LocaleStringResourceID);
            if (localeStringResource != null)
            {
                Language language = LanguageManager.GetLanguageByID(localeStringResource.LanguageID);
                if (language != null)
                    lblLanguage.Text = Server.HtmlEncode(language.Name);
                else
                    Response.Redirect("LocaleStringResources.aspx");

                this.txtResourceName.Text = localeStringResource.ResourceName;
                this.txtResourceValue.Text = localeStringResource.ResourceValue;
            }
            else
            {
                Language language = LanguageManager.GetLanguageByID(this.LanguageID);
                if (language != null)
                    lblLanguage.Text = Server.HtmlEncode(language.Name);
                else
                    Response.Redirect("LocaleStringResources.aspx");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.BindData();
            }
        }

        public LocaleStringResource SaveInfo()
        {
            LocaleStringResource localeStringResource = LocaleStringResourceManager.GetLocaleStringResourceByID(this.LocaleStringResourceID);

            if (localeStringResource != null)
            {
                localeStringResource = LocaleStringResourceManager.UpdateLocaleStringResource(localeStringResource.LocaleStringResourceID,
                   localeStringResource.LanguageID, txtResourceName.Text, txtResourceValue.Text);
            }
            else
            {
                localeStringResource = LocaleStringResourceManager.InsertLocaleStringResource(
                        this.LanguageID, txtResourceName.Text,
                        txtResourceValue.Text);
            }

            return localeStringResource;
        }

        public int LanguageID
        {
            get
            {
                return CommonHelper.QueryStringInt("LanguageID");
            }
        }

        public int LocaleStringResourceID
        {
            get
            {
                return CommonHelper.QueryStringInt("LocaleStringResourceID");
            }
        }
    }
}