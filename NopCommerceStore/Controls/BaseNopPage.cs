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
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Localization;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.Web
{
    public partial class BaseNopPage : Page
    {
        #region Fields
        protected Stopwatch executionTimer;
        protected bool showExecutionTimer = false;
        #endregion

        #region Ctor
        public BaseNopPage()
        {
            showExecutionTimer = SettingManager.GetSettingValueBoolean("Display.PageExecutionTimeInfoEnabled");
            if (showExecutionTimer)
            {
                executionTimer = new Stopwatch();
            }
        }
        #endregion

        #region Overrides
        protected override void OnInit(EventArgs e)
        {
            if (showExecutionTimer)
            {
                executionTimer.Start();
            }

            base.OnInit(e);

            if (showExecutionTimer)
            {
                executionTimer.Stop();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            //store is closed
            if (SettingManager.GetSettingValueBoolean("Common.StoreClosed"))
            {
                //it's not login/logout page
                if ((!CommonHelper.GetThisPageURL(false).ToLower().Contains("/login.aspx"))
                    && (!CommonHelper.GetThisPageURL(false).ToLower().Contains("/logout.aspx")))
                {
                    //current customer is not admin
                    if (NopContext.Current.User == null || !NopContext.Current.User.IsAdmin)
                    {
                        Response.Redirect("~/StoreClosed.htm");
                    }
                }
            }

            //allow navigation only for registered customers
            if (CustomerManager.AllowNavigationOnlyRegisteredCustomers)
            {
                if (NopContext.Current.User == null || NopContext.Current.User.IsGuest)
                {
                    //it's not login/logout page
                    if ((!CommonHelper.GetThisPageURL(false).ToLower().Contains("/login.aspx"))
                        && (!CommonHelper.GetThisPageURL(false).ToLower().Contains("/logout.aspx")))
                    {
                        string loginURL = CommonHelper.GetLoginPageURL(false);
                        Response.Redirect(loginURL);
                    }
                }
            }

            if (showExecutionTimer)
            {
                executionTimer.Start();
            }

            base.OnLoad(e);

            if (showExecutionTimer)
            {
                executionTimer.Stop();
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            string publicStoreTheme = SettingManager.GetSettingValue("Display.PublicStoreTheme");
            if (!String.IsNullOrEmpty(publicStoreTheme))
            {
                this.Theme = publicStoreTheme;
            }
            base.OnPreInit(e);
        }

        protected override void CreateChildControls()
        {
            if (showExecutionTimer)
            {
                executionTimer.Start();
            }
            base.CreateChildControls();

            if (showExecutionTimer)
            {
                executionTimer.Stop();
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {

            if (showExecutionTimer)
            {
                executionTimer.Start();
            }
            base.Render(writer);

            if (showExecutionTimer)
            {
                executionTimer.Stop();
                RenderExecutionTimerValue(writer);
            }
        }

        #endregion

        #region Utiilities
        protected virtual void RenderExecutionTimerValue(HtmlTextWriter writer)
        {
            if (showExecutionTimer)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"<div style=""color:#ffffff;background:#000000;font-weight:bold,padding:5px"">");
                sb.Append(String.Format("Page execution time is {0:F10}.<br />", executionTimer.Elapsed.TotalSeconds));
                sb.Append(@"</div>");
                writer.Write(sb.ToString());
            }
        }

        protected string GetLocaleResourceString(string ResourceName)
        {
            Language language = NopContext.Current.WorkingLanguage;
            return LocalizationManager.GetLocaleResourceString(ResourceName, language.LanguageID);
        }
        #endregion
    }
}