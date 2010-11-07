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
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic.Categories;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Promo.Discounts;
using NopSolutions.NopCommerce.BusinessLogic.Templates;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Web.Administration.Modules;
 

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class CategoryDetailsControl : BaseNopAdministrationUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.SelectTab(this.CategoryTabs, this.TabID);
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    Category category = ctrlCategoryInfo.SaveInfo();
                    ctrlCategorySEO.SaveInfo();
                    ctrlCategoryProduct.SaveInfo();
                    ctrlCategoryDiscount.SaveInfo();
                    Response.Redirect(string.Format("CategoryDetails.aspx?CategoryID={0}&TabID={1}", category.CategoryID, this.GetActiveTabID(this.CategoryTabs)));
                }
                catch (Exception exc)
                {
                    ProcessException(exc);
                }
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                CategoryManager.MarkCategoryAsDeleted(this.CategoryID);
                Response.Redirect("Categories.aspx");
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        public int CategoryID
        {
            get
            {
                return CommonHelper.QueryStringInt("CategoryID");
            }
        }

        protected string TabID
        {
            get
            {
                return CommonHelper.QueryString("TabID");
            }
        }
    }
}