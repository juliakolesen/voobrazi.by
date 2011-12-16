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
        private int pageSize = 12;
        private int totalItemCount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            BindGrid();
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
            int id = 0;
            ProductCollection products = ProductManager.GetAllProducts(0, 0, null, null, null,
                                                     productName, false, pageSize, CurrentPageIndex, null, out totalItemCount);

            return products;
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
    }
}