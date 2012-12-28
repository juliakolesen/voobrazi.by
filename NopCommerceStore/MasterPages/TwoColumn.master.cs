using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Categories;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Controls;

namespace NopSolutions.NopCommerce.Web.MasterPages
{
	public partial class TwoColumn : BaseNopNestedMasterPage
	{
		public Image imgSalesLeader
		{
			get { return imgLeader; }
		}

		public HyperLink hlSalesLeader
		{
			get { return hlLeader; }
		}

		public HtmlContainerControl SalesLeader
		{
			get { return divLeader; }
		}

		public Image imgUniqueProposal
		{
			get { return imgUnique; }
		}

		public HyperLink hlUniqueProposal
		{
			get { return hlUnique; }
		}

		public HtmlContainerControl Unique
		{
			get { return divUnique; }
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				BindData();
			}
		}

		public decimal? MinPriceConverted;
		public decimal? MaxPriceConverted;
		public List<int> PSOFilterOption;

		protected void BindData()
		{
			Category category = CategoryManager.GetCategoryByID(CategoryID);
			if (category == null)
				return;

			decimal? minPrice;
			decimal? maxPrice;
			

			PSOFilterOption = ctrlProductSpecificationFilter.GetAlreadyFilteredSpecOptionIDs();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (Session["productsPager"] != null)
			{
				Pager productsPager = Session["productsPager"] as Pager;

				ctrlProductSpecificationFilter.ExcludedQueryStringParams = productsPager.QueryStringProperty;
				ctrlProductSpecificationFilter.CategoryID = CategoryID;

				ctrlProductSpecificationFilter.ReservedQueryStringParams = "CategoryID,";
				ctrlProductSpecificationFilter.ReservedQueryStringParams += ",";
				ctrlProductSpecificationFilter.ReservedQueryStringParams += productsPager.QueryStringProperty;
			}
		}

		public int CategoryID
		{
			get
			{
				int categoryId = CommonHelper.QueryStringInt("CategoryID");
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

				return categoryId;
			}
		}
		public int ProductID
		{
			get
			{
				return CommonHelper.QueryStringInt("ProductID");
			}
		}
	}
}
