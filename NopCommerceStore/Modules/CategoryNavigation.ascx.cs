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
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using NopSolutions.NopCommerce.BusinessLogic.Categories;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class CategoryNavigation : BaseNopUserControl
    {
        #region Classes
        public class NopCommerceLi : WebControl, INamingContainer
        {
            protected override void Render(HtmlTextWriter writer)
            {
                if (Level == 0)
                {
                    if (ParentId <= MaxCount)
                    {
                        writer.WriteLine("<div class=\"item\">");
                        string navigateUrl = ChildCategoryUrl;
                        if (String.IsNullOrEmpty(navigateUrl))
                            navigateUrl = "javascript:void(0);";
                        writer.WriteLine(string.Format("<a href=\"{0}\" onclick=\"show_div('div_{1}')\" title=\"{2}\">", navigateUrl, ParentId - 1, Title));
                        string url = Picture != null
                                         ? PictureManager.GetPictureUrl(Picture.PictureID, 35)
                                         : Page.ResolveUrl("~/images/ff_images/submenu/subitem.jpg");
                        writer.WriteLine(string.Format("<img src=\"" + url + "\" alt=\"\" />{0}</a></div>", Title));
                    }
                }
                else if (Level == 1)
                {
                    if (Id == 1)
                        writer.WriteLine(string.Format("<div id=\"div_{0}\" class=\"div_1\" style=\"display: none;\">", ParentId - 1));

                    writer.WriteLine(string.Format("<img src=\"" + Page.ResolveUrl("~/images/ff_images/submenu/subitem.jpg") + "\" alt=\"\" />"));
                    writer.WriteLine(string.Format("<a href=\"{0}\" title=\"{1}\">{1}</a>", NavigateUrl, Title));

                    if (Id < TotalMenuItems)
                        writer.WriteLine("<br />");
                    else
                        writer.WriteLine("</div>");
                }
            }

            public int Id { get; set; }
            public int ParentId { get; set; }
            public int Level { get; set; }
            public int TotalMenuItems { get; set; }
            public string Title { get; set; }
            public string NavigateUrl { get; set; }
            public Picture Picture { get; set; }
            public int MaxCount { get; set; }
            public string ChildCategoryUrl { get; set; }
        }
        #endregion

        private CategoryCollection rootCategories = new CategoryCollection();

        #region Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterClientScriptInclude(GetType(), ID + "_ShowHideScript", Page.ResolveUrl("~/scripts/scripts_3g.js"));
        }
        protected override void OnPreRender(EventArgs e)
        {
            Category cat = CategoryManager.GetCategoryByID(CategoryID);

            int catId = 0;
            if (cat != null)
            {
                Category categoryFromList =
                    rootCategories.FirstOrDefault(category => category.CategoryID == cat.ParentCategory.CategoryID);
                if (categoryFromList != null)
                    catId = rootCategories.IndexOf(categoryFromList);
                if (catId >= 0)
                    ScriptManager.RegisterStartupScript(Page, GetType(), "menu_autoOpen",
                                                        string.Format("show_div('div_{0}');", catId), true);
            }

            base.OnPreRender(e);
        }
        #endregion

        #region Overrides
        protected override void CreateChildControls()
        {
            if (!ChildControlsCreated)
            {
                CreateMenu();
                base.CreateChildControls();
                ChildControlsCreated = true;
            }
        }
        #endregion

        public int CategoryID
        {
            get
            {
                int categoryId;
                if (ViewState["CategoryID"] == null)
                {
                    categoryId = CommonHelper.QueryStringInt("CategoryID");
                    if (categoryId == 0)
                    {
                        Product product = ProductManager.GetProductByID(ProductID);
                        if (product != null)
                        {
                            ProductCategory prodCategory = product.ProductCategories.Find(p => p.Category.ParentCategory != null);
                            if (prodCategory != null)
                            {
                                categoryId = prodCategory.CategoryID;
                            }
                        }
                    }
                }
                else
                    categoryId = (int)ViewState["CategoryID"];
                return categoryId;
            }
            set
            {
                ViewState["CategoryID"] = value;
            }
        }

        public int ProductID
        {
            get
            {
                return CommonHelper.QueryStringInt("ProductID");
            }
        }

        #region Utilities
        protected void CreateMenu()
        {
            CategoryCollection breadCrumb;
            Category currentCategory = CategoryManager.GetCategoryByID(CommonHelper.QueryStringInt("CategoryID"));
            if (currentCategory == null)
            {
                Product product = ProductManager.GetProductByID(CommonHelper.QueryStringInt("ProductID"));
                if (product != null)
                {
                    ProductCategoryCollection productCategories = product.ProductCategories;
                    if (productCategories.Count > 0)
                        currentCategory = productCategories[0].Category;
                }
            }

            if (currentCategory != null)
                breadCrumb = CategoryManager.GetBreadCrumb(currentCategory.CategoryID);
            else
                breadCrumb = new CategoryCollection();

            CreateChildMenu(breadCrumb, 0, currentCategory, 0);
        }

        private int parentId = 1;
        protected void CreateChildMenu(CategoryCollection breadCrumb, int rootCategoryID, Category currentCategory, int level)
        {
            if (level < 2)
            {
                int id = 1;
                CategoryCollection categoryCollection = CategoryManager.GetAllCategories(rootCategoryID);
                if(level == 0)
                {
                    rootCategories = categoryCollection;
                }

                foreach (Category category in categoryCollection)
                {
                    NopCommerceLi link = new NopCommerceLi();
                    phCategories.Controls.Add(link);

                    string categoryURL = SEOHelper.GetCategoryURL(category.CategoryID);

                    link.Id = id;
                    link.ParentId = parentId;
                    link.Level = level;
                    link.TotalMenuItems = categoryCollection.Count;
                    link.Title = Server.HtmlEncode(category.Name);
                    link.NavigateUrl = categoryURL;
                    link.Picture = category.Picture;
                    link.MaxCount = rootCategories.Count;
                    CategoryCollection subCategories = CategoryManager.GetAllCategories(category.CategoryID, false);
                    if(subCategories.Count > 0)
                    {
                        link.ChildCategoryUrl = SEOHelper.GetCategoryURL(subCategories[0].CategoryID);
                    }

                    CreateChildMenu(breadCrumb, category.CategoryID, currentCategory, level + 1);
                    id++;
                    if (level == 0)
                        parentId++;
                }
            }
        }
        #endregion
    }
}