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
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Froogle;

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class FroogleControl : BaseNopAdministrationUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            cbAllowPublicFroogleAccess.Checked = SettingManager.GetSettingValueBoolean("Froogle.AllowPublicFroogleAccess");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    SettingManager.SetParam("Froogle.AllowPublicFroogleAccess", cbAllowPublicFroogleAccess.Checked.ToString());
                }
                catch (Exception exc)
                {
                    ProcessException(exc);
                }
            }
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    string fileName = string.Format("froogle_{0}.xml", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    string filePath = string.Format("{0}files\\froogle\\{1}", HttpContext.Current.Request.PhysicalApplicationPath, fileName);
                    using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                    {
                        FroogleService.GenerateFeed(fs);
                    }

                    string clickhereStr = string.Format("<a href=\"{0}files/froogle/{1}\" target=\"_blank\">{2}</a>", CommonHelper.GetStoreLocation(false), fileName, GetLocaleResourceString("Admin.Froogle.ClickHere"));
                    string result = string.Format(GetLocaleResourceString("Admin.Froogle.SuccessResult"), clickhereStr);
                    lblResult.Text = result;
                }
                catch (Exception exc)
                {
                    ProcessException(exc);
                }
            }
        }
    }
}