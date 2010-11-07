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
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Categories;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class HomePageCategories : BaseNopUserControl
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
            CategoryCollection subCategoryCollection = CategoryManager.GetAllCategories(ParentCategoryID);
            if (subCategoryCollection.Count > 0)
            {
                dlCategories.DataSource = subCategoryCollection;
                dlCategories.DataBind();
            }
            else
                this.Visible = false;
        }

        protected void dlCategories_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Category category = e.Item.DataItem as Category;
                HyperLink hlImageLink = e.Item.FindControl("hlImageLink") as HyperLink;
                string categoryURL = SEOHelper.GetCategoryURL(category.CategoryID);
                if (hlImageLink != null)
                {
                    hlImageLink.ImageUrl = PictureManager.GetPictureUrl(category.PictureID, SettingManager.GetSettingValueInteger("Media.Category.ThumbnailImageSize", 125), true);
                    hlImageLink.NavigateUrl = categoryURL;
                    hlImageLink.ToolTip = String.Format(GetLocaleResourceString("Media.Category.ImageLinkTitleFormat"), category.Name);
                    hlImageLink.Text = String.Format(GetLocaleResourceString("Media.Category.ImageAlternateTextFormat"), category.Name);
                }
               
                HyperLink hlCategory = e.Item.FindControl("hlCategory") as HyperLink;
                if (hlCategory != null)
                {
                    hlCategory.NavigateUrl = categoryURL;
                }
            }
        }


        [DefaultValue(0)]
        public int ParentCategoryID
        {
            get
            {
                if (ViewState["ParentCategoryID"] == null)
                    return 0;
                else
                    return (int)ViewState["ParentCategoryID"];
            }
            set { ViewState["ParentCategoryID"] = value; }
        }
    }
}