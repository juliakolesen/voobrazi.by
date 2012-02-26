using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopCommerceStore
{
    public partial class ViewedItems : System.Web.UI.Page
    {
        private int pageSize = Int32.MaxValue;
        private int totalItemCount = 0;
        private const int minPageSize = 12;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                FillCounts();
                BindGrid();
            }
        }

        private void FillCounts()
        {
            pageSize = CommonHelper.QueryStringInt("pageSize", -1);
            this.productsCount.Items.Add("Показать все");
            this.productsCount.Items.Add("12");
            this.productsCount.Items.Add("24");
            this.productsCount.Items.Add("36");
            this.productsCount.Items.Add("48");

            if (pageSize == -1)
            {
                pageSize = 24;
            }

            if (pageSize == 0)
            {
                pageSize = Int32.MaxValue;
            }

            this.productsCount.SelectedIndex = pageSize / minPageSize;
        }

        protected void BindGrid()
        {
            ProductCollection products = GetProducts();
            this.dlProducts.DataSource = products;
            this.dlProducts.DataBind();
            this.productsPagerBottom.PageSize = productsPager.PageSize = pageSize;
            this.productsPagerBottom.TotalRecords = productsPager.TotalRecords = totalItemCount;
            this.productsPagerBottom.PageIndex = productsPager.PageIndex = CurrentPageIndex;
        }

        protected ProductCollection GetProducts()
        {
            ViewedItemsCollection collection = ViewedItemManager.GetCurrentViewedItem(pageSize, CurrentPageIndex, out totalItemCount);
            List<int> productsIDs = collection.Select(x => x.ProductVariantID).ToList();
            ProductCollection productCollection = new ProductCollection();
            foreach (var productID in productsIDs)
            {
                ProductVariant product = ProductManager.GetProductVariantByID(productID);
                productCollection.Add(product.Product);
            }

            return productCollection;
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
            DropDownList drList = (DropDownList)sender;
            string url = CommonHelper.GetThisPageURL(true);
            url = CommonHelper.RemoveQueryString(url, "pageSize");
            Int32.TryParse(productsCount.SelectedItem.Text, out pageSize);
            if (!url.Contains("?"))
                url += "?";
            else
                url += "&";

            Response.Redirect(String.Format("{0}pageSize={1}", url, pageSize));
        }
    }
}