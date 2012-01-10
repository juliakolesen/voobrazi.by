using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class SearchResultsControl : BaseNopUserControl
    {
        private int pageSize = Int32.MaxValue;
        private int totalItemCount = 0;
        private const int minPageSize = 12;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                FillCounts();
                FillSortBy();
                BindGrid();
            }
        }

        protected void BindGrid()
        {
            ProductCollection products = GetProducts();

            if (products.Count > 0)
            {
                this.dlProducts.Visible = true;
                this.lblNoProductsFound.Visible = false;
                this.dlProducts.DataSource = products;
                this.dlProducts.DataBind();
                this.productsPagerBottom.PageSize = productsPager.PageSize = pageSize;
                this.productsPagerBottom.TotalRecords = productsPager.TotalRecords = totalItemCount;
                this.productsPagerBottom.PageIndex = productsPager.PageIndex = CurrentPageIndex;
            }
            else
            {
                this.lblNoProductsFound.Visible = true;
                this.dlProducts.Visible = false;
            }
        }

        protected ProductCollection GetProducts()
        {
            string productName = CommonHelper.QueryString("searchParameter");
            SortParameter sortParameter = GetSortParameter();
            ProductCollection products = ProductManager.GetAllProducts(0, 0, null, null, null,
                                                     productName, false, pageSize, CurrentPageIndex,
                                                     null, (int)sortParameter.SortBy, sortParameter.Ascending,
                                                     out totalItemCount);

            return products;
        }

        private void FillCounts()
        {
            pageSize = CommonHelper.QueryStringInt("pageSize");
            if (pageSize == 0)
            {
                pageSize = Int32.MaxValue;
            }

            this.productsCount.Items.Add("Показать все");
            this.productsCount.Items.Add("12");
            this.productsCount.Items.Add("24");
            this.productsCount.Items.Add("36");
            this.productsCount.Items.Add("48");
            this.productsCount.SelectedIndex = pageSize/minPageSize;
        }

        private void FillSortBy()
        {
            this.sortBy.Items.Add("По популярности");
            this.sortBy.Items.Add("По цене вниз");
            this.sortBy.Items.Add("По цене вверх");
            this.sortBy.Items.Add("По новизне");
            this.sortBy.SelectedIndex = CommonHelper.QueryStringInt("sortBy");
        }

        public int CurrentPageIndex
        {
            get
            {
                int _pageIndex = CommonHelper.QueryStringInt(productsPager.QueryStringProperty);
                _pageIndex--;
                if (_pageIndex < 0)
                    _pageIndex = 0;
                return _pageIndex;
            }
        }

        protected void productsCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32.TryParse(productsCount.SelectedItem.Text, out pageSize);
            string productName = CommonHelper.QueryString("searchParameter");
            int sortByIndex = sortBy.SelectedIndex;
            Response.Redirect(String.Format("~/SearchResults.aspx?searchParameter={0}&pageSize={1}&sortBy={2}", productName, pageSize, sortByIndex));
        }

        private SortParameter GetSortParameter()
        {
            SortParameter sortParam; 
            switch (sortBy.SelectedIndex)
            { 
                case 0:
                    sortParam = new SortParameter(FieldToSort.Popularity, false);
                    break;
                case 1:
                    sortParam = new SortParameter(FieldToSort.Price, false);
                    break;
                case 2:
                    sortParam = new SortParameter(FieldToSort.Price, true);
                    break;
                case 3:
                    sortParam = new SortParameter(FieldToSort.Novelity, false);
                    break;
                default:
                    sortParam = new SortParameter(FieldToSort.Default, false);
                    break;
            }

            return sortParam;
        }

        public class SortParameter
        {
            public bool Ascending;
            public FieldToSort SortBy;

            public SortParameter(FieldToSort sortBy, bool descending)
            {
                this.SortBy = sortBy;
                this.Ascending = descending;
            }
        }

        [Flags]
        public enum FieldToSort
        {
            Default = 0,
            Popularity = 1,
            Price = 2,
            Novelity = 3
        }
    }
}