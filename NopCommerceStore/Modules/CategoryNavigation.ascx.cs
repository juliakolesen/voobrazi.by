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
using System.Web.UI;
using System.Web.UI.WebControls;
using NopSolutions.NopCommerce.BusinessLogic.Categories;
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
			readonly string[] firstLevelImages = new[] { "vd", "flowers", "svadba", "bouquets", "plants", "gorshok", "care", "vase", "gifts", "letter" };

			protected override void Render(HtmlTextWriter writer)
			{
				if (Level == 0)
				{
					if (ParentId <= 10)
					{
						int hSpace = 0;
						switch (ParentId)
						{
							case 1:
								hSpace = 10;
								break;
							case 2:
								hSpace = 4;
								break;
							case 3:
								hSpace = 6;
								break;
							case 4:
							case 5:
							case 6:
								hSpace = 10;
								break;
							case 7:
								hSpace = 8;
								break;
							case 8:
								hSpace = 14;
								break;
							case 9:
							case 10:
								hSpace = 7;
								break;
							default:
								break;
						}

						writer.WriteLine(string.Format("<div class=\"item_{0}\">", ParentId - 1));
						writer.WriteLine(string.Format("<a href=\"javascript:void(0);\" onclick=\"show_div('div_{0}')\" title=\"{1}\">", ParentId - 1, Title));
						writer.WriteLine(string.Format("<img src=\"" + Page.ResolveUrl("~/images/ff_images/submenu/{0}.jpg") + "\" alt=\"\" hspace=\"{1}\" {2} />{3}</a></div>", firstLevelImages[ParentId - 1], hSpace, ParentId == 9 ? "align=\"absmiddle\"" : "", Title));
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
		}
		#endregion

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
				switch (cat.ParentCategory.Name)
				{
					case "Живые цветы":
						catId = 1;
						break;
					case "Свадебная флористика":
						catId = 2;
						break;
					case "Букеты и композиции":
						catId = 3;
						break;
					case "Комнатные растения":
						catId = 4;
						break;
					case "Горшки для растений":
						catId = 5;
						break;
					case "Грунты / Уход":
						catId = 6;
						break;
					case "Вазы":
						catId = 7;
						break;
					case "Подарки":
						catId = 8;
						break;
					case "Открытки":
						catId = 9;
						break;
					default:
						catId = 0;
						break;
				}
			if (catId != 0)
				ScriptManager.RegisterStartupScript(Page, GetType(), "menu_autoOpen", string.Format("show_div('div_{0}');", catId), true);

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
							categoryId = product.ProductCategories.Find(p => p.Category.ParentCategory != null).CategoryID;
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