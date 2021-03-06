﻿//------------------------------------------------------------------------------
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
using System.Web.UI;
using AjaxControlToolkit;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Audit;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Localization;

namespace NopSolutions.NopCommerce.Web
{
    public partial class BaseNopAdministrationUserControl : UserControl
    {
        public BaseNopAdministrationUserControl()
        {

        }

        protected void SelectTab(TabContainer tabContainer, string TabID)
        {
            if (tabContainer == null)
                throw new ArgumentNullException("tabContainer");

            if (!String.IsNullOrEmpty(TabID))
            {
                AjaxControlToolkit.TabPanel tab = tabContainer.FindControl(TabID) as AjaxControlToolkit.TabPanel;
                if (tab != null)
                {
                    tabContainer.ActiveTab = tab;
                }
            }
        }

        protected string GetActiveTabID(TabContainer tabContainer)
        {
            if (tabContainer == null)
                throw new ArgumentNullException("tabContainer");

            if (tabContainer.ActiveTab != null)
                return tabContainer.ActiveTab.ID;

            return string.Empty;
        }

        protected void ProcessException(Exception exc)
        {
            LogManager.InsertLog(LogTypeEnum.AdministrationArea, exc.Message, exc);
            if (SettingManager.GetSettingValueBoolean("Display.AdminArea.ShowFullErrors"))
            {
                ShowError(exc.Message, exc.ToString());
            }
            else
            {
                ShowError(exc.Message, string.Empty);
            }
        }

        protected void ShowMessage(string Message)
        {
            if (this.Page == null)
                return;

            MasterPage masterPage = this.Page.Master;
            if (masterPage == null)
                return;

            BaseNopAdministrationMasterPage nopAdministrationMasterPage = masterPage as BaseNopAdministrationMasterPage;
            if (nopAdministrationMasterPage != null)
                nopAdministrationMasterPage.ShowMessage(Message, string.Empty);
        }

        protected void ShowError(string Message)
        {
            ShowError(Message, string.Empty);
        }

        protected void ShowError(string Message, string CompleteMessage)
        {
            if (this.Page == null)
                return;

            MasterPage masterPage = this.Page.Master;
            if (masterPage == null)
                return;

            BaseNopAdministrationMasterPage nopAdministrationMasterPage = masterPage as BaseNopAdministrationMasterPage;
            if (nopAdministrationMasterPage != null)
                nopAdministrationMasterPage.ShowMessage(Message, CompleteMessage);
        }

        protected string GetLocaleResourceString(string ResourceName)
        {
            Language language = NopContext.Current.WorkingLanguage;
            return LocalizationManager.GetLocaleResourceString(ResourceName, language.LanguageID);
        }
    }
}