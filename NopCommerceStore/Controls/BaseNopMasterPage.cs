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
using System.IO;
using System.Linq;
using System.Text;
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
using NopSolutions.NopCommerce.BusinessLogic.Promo.Affiliates;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.Web
{
    public partial class BaseNopMasterPage : MasterPage
    {
        public BaseNopMasterPage() :
            base()
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            SetFavIcon();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            AddPoweredBy();
            if (!Page.IsPostBack)
            {
                CheckAffiliate();
            }

            string defaulSEOTitle = SettingManager.GetSettingValue("SEO.DefaultTitle");
            string defaulSEODescription = SettingManager.GetSettingValue("SEO.DefaultMetaDescription");
            string defaulSEOKeywords = SettingManager.GetSettingValue("SEO.DefaultMetaKeywords");
            SEOHelper.RenderTitle(this.Page, defaulSEOTitle, false, false);
            SEOHelper.RenderMetaTag(this.Page, "description", defaulSEODescription, false);
            SEOHelper.RenderMetaTag(this.Page, "keywords", defaulSEOKeywords, false);

            if (SettingManager.GetSettingValueBoolean("Display.ShowNewsHeaderRssURL"))
            {
                SEOHelper.RenderHeaderRSSLink(this.Page, defaulSEOTitle + ": News", SEOHelper.GetNewsRssURL());
            }
            if (SettingManager.GetSettingValueBoolean("Display.ShowBlogHeaderRssURL"))
            {
                SEOHelper.RenderHeaderRSSLink(this.Page, defaulSEOTitle + ": Blog", SEOHelper.GetBlogRssURL());
            }
        }

        protected void CheckAffiliate()
        {
            Affiliate affiliate = AffiliateManager.GetAffiliateByID(CommonHelper.QueryStringInt("AffiliateID"));
            if (affiliate != null && affiliate.Active)
            {
                if (NopContext.Current.User == null)
                {
                    HttpCookie affiliateCookie = HttpContext.Current.Request.Cookies.Get("NopCommerce.AffiliateID");
                    if (affiliateCookie == null)
                        affiliateCookie = new HttpCookie("NopCommerce.AffiliateID");

                    affiliateCookie.Value = affiliate.AffiliateID.ToString();
                    affiliateCookie.Expires = DateTime.Now.AddDays(10.0);
                    HttpContext.Current.Response.Cookies.Set(affiliateCookie);
                }
                else if (NopContext.Current.User.AffiliateID != affiliate.AffiliateID)
                {
                    NopContext.Current.User = CustomerManager.SetAffiliate(NopContext.Current.User.CustomerID, affiliate.AffiliateID);
                }
            }
        }

        protected void SetFavIcon()
        {
            string favIconPath = HttpContext.Current.Request.PhysicalApplicationPath + "favicon.ico";
            if (File.Exists(favIconPath))
            {
                string favIconUrl = CommonHelper.GetStoreLocation() + "favicon.ico";
                HtmlLink htmlLink = new HtmlLink();
                htmlLink.Attributes["rel"] = "SHORTCUT ICON";
                htmlLink.Attributes["href"] = favIconUrl;
                Page.Header.Controls.Add(htmlLink);
            }
        }
        
        protected void AddPoweredBy()
        {
            StringBuilder poweredBy = new StringBuilder();
            poweredBy.Append("<!--Powered by nopCommerce - http://www.nopCommerce.com-->");
            poweredBy.Append("<!--Copyright (c) 2008-2010-->");
            Page.Header.Controls.AddAt(0, new LiteralControl(poweredBy.ToString()));
        }

        protected string GetLocaleResourceString(string ResourceName)
        {
            Language language = NopContext.Current.WorkingLanguage;
            return LocalizationManager.GetLocaleResourceString(ResourceName, language.LanguageID);
        }
    }
}