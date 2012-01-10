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
using System.Collections.Generic;
using System.Web.UI.WebControls;
using NopSolutions.NopCommerce.BusinessLogic.Categories;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Web.MasterPages;
using System.Web;

namespace NopSolutions.NopCommerce.Web.Templates.Categories
{
    public partial class ProductsInGrid : BaseNopUserControl
    {
        private int pageSize = Int32.MaxValue;
        private int totalItemCount = 0;
        private const int minPageSize = 12;

        protected void BindData()
        {
            Category category = CategoryManager.GetCategoryByID(CategoryID);

            // настройка лидеров продаж
            List<BestSellersReportLine> report = OrderManager.BestSellersReport(720, 10, 1);
            if (report.Count == 0)
                ((TwoColumn)Page.Master).SalesLeader.Visible = false;
            else
            {
                int index = report.Count > 9 ? 9 : report.Count - 1;
                ProductVariant productVariant = ProductManager.GetProductVariantByID(report[new Random().Next(index)].ProductVariantID);
                if (productVariant.Product.ProductPictures.Count > 0)
                    ((TwoColumn)Page.Master).imgSalesLeader.ImageUrl = PictureManager.GetPictureUrl(productVariant.Product.ProductPictures[0].Picture);
                else
                    ((TwoColumn)Page.Master).imgSalesLeader.ImageUrl = PictureManager.GetPictureUrl(productVariant.Picture);
                ((TwoColumn)Page.Master).hlSalesLeader.NavigateUrl = SEOHelper.GetProductURL(productVariant.Product);
            }

            // настройка уникального предложения
            if (category.FeaturedProducts.Count > 0)
            {
                Product product = category.FeaturedProducts[new Random().Next(category.FeaturedProducts.Count - 1)];
                if (product.ProductPictures.Count > 0)
                    ((TwoColumn)Page.Master).imgUniqueProposal.ImageUrl = PictureManager.GetPictureUrl(product.ProductPictures[0].Picture);
                else
                    ((TwoColumn)Page.Master).imgUniqueProposal.ImageUrl = PictureManager.GetPictureUrl(product.ProductVariants[0].Picture);
                ((TwoColumn)Page.Master).hlUniqueProposal.NavigateUrl = SEOHelper.GetProductURL(product);
            }
            else
                ((TwoColumn)Page.Master).Unique.Visible = false;

            rptrCategoryBreadcrumb.DataSource = CategoryManager.GetBreadCrumb(CategoryID);
            rptrCategoryBreadcrumb.DataBind();

            lDescription.Text = category.Description;

            int totalRecords;
            Int32.TryParse(productsCount.SelectedItem.Text, out pageSize);
            if (pageSize == 0)
            {
                pageSize = Int32.MaxValue;
            }

            SortParameter sortParameter = GetSortParameter();
            ProductCollection productCollection = ProductManager.GetAllProducts(CategoryID,
                0, null, ((TwoColumn)Page.Master).MinPriceConverted, ((TwoColumn)Page.Master).MaxPriceConverted,
                pageSize, CurrentPageIndex, ((TwoColumn)Page.Master).PSOFilterOption, (int)sortParameter.SortBy, sortParameter.Ascending, out totalRecords);

            if (productCollection.Count > 0)
            {
                productsPagerBottom.PageSize = productsPager.PageSize = pageSize;
                productsPagerBottom.TotalRecords = productsPager.TotalRecords = totalRecords;
                productsPagerBottom.PageIndex = productsPager.PageIndex = CurrentPageIndex;
                Session.Add("productsPager", productsPager);

                dlProducts.DataSource = productCollection;
                dlProducts.DataBind();
            }
            else
            {
                dlProducts.Visible = false;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!Page.IsPostBack)
            {
                FillCounts();
                FillSortBy();
                BindData();
            }
        }

        protected void dlSubCategories_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Category category = e.Item.DataItem as Category;
                string categoryURL = SEOHelper.GetCategoryURL(category.CategoryID);

                HyperLink hlImageLink = e.Item.FindControl("hlImageLink") as HyperLink;
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
                    hlCategory.ToolTip = String.Format(GetLocaleResourceString("Media.Category.ImageLinkTitleFormat"), category.Name);
                    hlCategory.Text = Server.HtmlEncode(category.Name);
                }
            }
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

        public int CategoryID
        {
            get
            {
                int categoryId = CommonHelper.QueryStringInt("CategoryID");
                if (categoryId == 0)
                {
                    Product product = ProductManager.GetProductByID(ProductID);
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
            this.productsCount.SelectedIndex = pageSize / minPageSize;
        }

        private void FillSortBy()
        {
            this.sortBy.Items.Add("По популярности");
            this.sortBy.Items.Add("По цене вниз");
            this.sortBy.Items.Add("По цене вверх");
            this.sortBy.Items.Add("По новизне");
            this.sortBy.SelectedIndex = CommonHelper.QueryStringInt("sortBy");
        }

        protected void productsCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32.TryParse(productsCount.SelectedItem.Text, out pageSize);
            int sortByIndex = sortBy.SelectedIndex;
            Response.Redirect(String.Format("{0}?pageSize={1}&sortBy={2}", HttpContext.Current.Request.Url.AbsolutePath, pageSize, sortByIndex));
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