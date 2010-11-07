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
    public partial class CategorySEOControl : BaseNopAdministrationUserControl
    {
        private void BindData()
        {
            Category category = CategoryManager.GetCategoryByID(this.CategoryID);

            if (category != null)
            {
                this.txtMetaKeywords.Text = category.MetaKeywords;
                this.txtMetaDescription.Text = category.MetaDescription;
                this.txtMetaTitle.Text = category.MetaTitle;
                this.txtSEName.Text = category.SEName;
                this.txtPageSize.Value = category.PageSize;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.BindData();
            }
        }

        public void SaveInfo()
        {
            Category category = CategoryManager.GetCategoryByID(this.CategoryID);

            if (category != null)
            {
                category = CategoryManager.UpdateCategory(category.CategoryID, category.Name, category.Description, category.TemplateID,
                     txtMetaKeywords.Text, txtMetaDescription.Text, txtMetaTitle.Text, txtSEName.Text, category.ParentCategoryID,
                     category.PictureID, txtPageSize.Value, category.PriceRanges, category.Published, category.Deleted, category.DisplayOrder, category.CreatedOn, DateTime.Now);
            }
        }

        public int CategoryID
        {
            get
            {
                return CommonHelper.QueryStringInt("CategoryID");
            }
        }
    }
}