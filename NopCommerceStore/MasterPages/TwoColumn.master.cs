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
			ctrlPriceRangeFilter.PriceRanges = category.PriceRanges;

			decimal? minPrice;
			decimal? maxPrice;
			if (ctrlPriceRangeFilter.SelectedPriceRange != null)
			{
				minPrice = ctrlPriceRangeFilter.SelectedPriceRange.From;
				if (minPrice.HasValue)
				{
					MinPriceConverted = CurrencyManager.ConvertCurrency(minPrice.Value, NopContext.Current.WorkingCurrency, CurrencyManager.PrimaryStoreCurrency);
				}

				maxPrice = ctrlPriceRangeFilter.SelectedPriceRange.To;
				if (maxPrice.HasValue)
				{
					MaxPriceConverted = CurrencyManager.ConvertCurrency(maxPrice.Value, NopContext.Current.WorkingCurrency, CurrencyManager.PrimaryStoreCurrency);
				}
			}

			PSOFilterOption = ctrlProductSpecificationFilter.GetAlreadyFilteredSpecOptionIDs();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (Session["productsPager"] != null)
			{
				Pager productsPager = Session["productsPager"] as Pager;
				ctrlPriceRangeFilter.ExcludedQueryStringParams = productsPager.QueryStringProperty;

				ctrlProductSpecificationFilter.ExcludedQueryStringParams = productsPager.QueryStringProperty;
				ctrlProductSpecificationFilter.CategoryID = CategoryID;

				ctrlProductSpecificationFilter.ReservedQueryStringParams = "CategoryID,";
				ctrlProductSpecificationFilter.ReservedQueryStringParams += ctrlPriceRangeFilter.QueryStringProperty;
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
						categoryId = product.ProductCategories.Find(p => p.Category.ParentCategory != null).CategoryID;
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
