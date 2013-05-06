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
using System.Linq;
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
using NopSolutions.NopCommerce.BusinessLogic.Products.Specs;
using NopSolutions.NopCommerce.BusinessLogic.Colors;

namespace NopSolutions.NopCommerce.Web.Templates.Categories
{
    public partial class ProductsInGrid : BaseNopUserControl
    {
        private int pageSize = Int32.MaxValue;
        private int totalItemCount = 0;
        private const int minPageSize = 12;
        private const int countLines = 2;
        private const int rowCount = 3;

        private void Page_Load(object sender, EventArgs e)
        {
            System.Web.SiteMap.Providers["NopDefaultXmlSiteMapProvider"].SiteMapResolve += this.ExpandCategoryPaths;
        }

        private SiteMapNode ExpandCategoryPaths(Object sender, SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = null;
            if (e.Provider.CurrentNode == null)
            {
                currentNode = e.Provider.FindSiteMapNode("~/Sitemap-Categories.aspx").Clone(true);
            }
            else
            {
                currentNode = e.Provider.CurrentNode.Clone(true);
            }

            ChangeCategoryMap(e, currentNode);
            if (0 != ProductID)
            {
                currentNode = ChangeProductMap(e, currentNode);
            }

            return currentNode;
        }

        private SiteMapNode ChangeProductMap(SiteMapResolveEventArgs e, SiteMapNode parent)
        {

            SiteMapNode currentProductNode = e.Provider.FindSiteMapNode("~/Sitemap-Products.aspx").Clone(true);
            try
            {
                SiteMapNode tempNode = currentProductNode;
                Product product = ProductManager.GetProductByID(ProductID);
                tempNode.Url = SEOHelper.GetProductURL(product.ProductID);
                tempNode.Description = product.Name;
                tempNode.Title = product.Name;
                tempNode.ParentNode = parent;
            }
            catch
            {
            }

            return currentProductNode;
        }

        private void ChangeCategoryMap(SiteMapResolveEventArgs e, SiteMapNode currentNode)
        {
            try
            {
                SiteMapNode tempNode = currentNode;
                Category category = CategoryManager.GetCategoryByID(CategoryID);
                if (0 != CategoryID)
                {
                    tempNode.Url = SEOHelper.GetCategoryURL(category.CategoryID);
                    CategoryCollection categCollection = CategoryManager.GetBreadCrumb(category.CategoryID);
                    string categoryPath = categCollection.Aggregate(String.Empty,
                                                                    (current, c) =>
                                                                    current +
                                                                    String.Format(
                                                                        String.IsNullOrEmpty(current) ? "{0}" : "/{0}",
                                                                        c.Name));

                    tempNode.Title = categoryPath;
                    tempNode.Description = categoryPath;
                }
            }
            catch
            {
            }
        }

        protected void BindData()
        {
            Category category = CategoryManager.GetCategoryByID(CategoryID);
            //настройка видимости фильтра
            Category baseCategory = category;
            Category prevCat = category;
            while (baseCategory.ParentCategory != null)
            {
                prevCat = baseCategory;
                baseCategory = baseCategory.ParentCategory;
            }

            string categoryName = baseCategory.Name.ToLower();
            AdjustFilters(prevCat, categoryName);
            AdjustColorsFilter(categoryName, category);

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

            lDescription.Text = category.Description;

            int totalRecords;
            Int32.TryParse(productsCount.SelectedItem.Text, out pageSize);
            if (pageSize == 0)
            {
                pageSize = Int32.MaxValue;
            }

            SortParameter sortParameter = GetSortParameter();
            List<int> psoFilterOptions = new List<int>();
            psoFilterOptions.AddRange(((TwoColumn)Page.Master).PSOFilterOption);
            psoFilterOptions.AddRange(this.designVariant.GetDesignVariantIds());
            psoFilterOptions.AddRange(this.weddingBunchVariant.GetDesignVariantIds());
            psoFilterOptions.AddRange(this.houseFlowersVariant.GetDesignVariantIds());
            decimal? priceMin = CommonHelper.QueryStringInt("minCost", 0);
            if (priceMin == 0)
            {
                var twoColumn = (TwoColumn) Page.Master;
                if (twoColumn != null) priceMin = twoColumn.MinPriceConverted;
            }
            decimal? priceMax = CommonHelper.QueryStringInt("maxCost", 0);
            if(priceMax == 0)
            {
                var twoColumn = (TwoColumn) Page.Master;
                if (twoColumn != null) priceMax = twoColumn.MaxPriceConverted;
            }

            int minHeight = CommonHelper.QueryStringInt("minHeight", 0);
            int maxHeight = CommonHelper.QueryStringInt("maxHeight", int.MaxValue);
            int minWidth = CommonHelper.QueryStringInt("minWidth", 0);
            int maxWidth = CommonHelper.QueryStringInt("maxWidth", int.MaxValue);

            ProductCollection productCollection = ProductManager.GetAllProducts(CategoryID,
                0, null, priceMin, priceMax,
                pageSize, CurrentPageIndex, psoFilterOptions, (int)sortParameter.SortBy, sortParameter.Ascending,
                minHeight, maxHeight, minWidth, maxWidth, out totalRecords);

            SetItemsToGrid(totalRecords, productCollection, categoryName);
        }

        private void AdjustFilters(Category prevCat, string categoryName)
        {
            if (categoryName.Equals("живые цветы", StringComparison.CurrentCultureIgnoreCase))
            {
                designVariant.Visible = true;
            }

            if (categoryName.Equals("свадебная флористика", StringComparison.CurrentCultureIgnoreCase)
                && prevCat.Name.Equals("букет невесты", StringComparison.CurrentCultureIgnoreCase))
            {
                weddingBunchVariant.Visible = true;
            }

            if (categoryName.Equals("комнатные растения", StringComparison.CurrentCultureIgnoreCase))
            {
                houseFlowersVariant.Visible = true;
            }
        }

        private void SetItemsToGrid(int totalRecords, ProductCollection productCollection, string category)
        {
            if (productCollection.Count > 0)
            {
                productsPagerBottom.PageSize = productsPager.PageSize = pageSize;
                productsPagerBottom.TotalRecords = productsPager.TotalRecords = totalRecords;
                productsPagerBottom.PageIndex = productsPager.PageIndex = CurrentPageIndex;
                Session.Add("productsPager", productsPager);

                if (productCollection.Count > countLines * rowCount
                    && (category.Equals("живые цветы", StringComparison.CurrentCultureIgnoreCase)
                    || category.Equals("Букеты и композиции", StringComparison.CurrentCultureIgnoreCase)))
                {
                    int countFirstPart = countLines * rowCount;
                    ProductCollection part1 = new ProductCollection();
                    part1.AddRange(productCollection.GetRange(0, countFirstPart));
                    ProductCollection part2 = new ProductCollection();
                    part2.AddRange(productCollection.GetRange(countFirstPart, productCollection.Count - countFirstPart));
                    dlProducts.DataSource = part1;
                    dlProducts.DataBind();
                    dlProducts2.DataSource = part2;
                    dlProducts2.DataBind();
                }
                else
                {
                    dlProducts.DataSource = productCollection;
                    dlProducts.DataBind();
                    dlProducts2.Visible = false;
                    indOrderBanner.Visible = false;
                }
            }
            else
            {
                dlProducts.Visible = false;
                dlProducts2.Visible = false;
                indOrderBanner.Visible = false;
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

        private void AdjustColorsFilter()
        {
            Category curCategory = CategoryManager.GetCategoryByID(CategoryID);
            Category category = curCategory;
            while (category.ParentCategory != null)
            {
                category = category.ParentCategory;
            }

            string categoryName = category.Name.ToLower();
            AdjustColorsFilter(categoryName, curCategory);
        }

        private void AdjustColorsFilter(string categoryName, Category category)
        {
            SpecificationAttribute specAttrColor = ColorManager.GetColorSpecificationAttribute();
            if (specAttrColor != null)
            {
                List<int> saOptions = ProductManager.GetProductSpecificationAttributeOptionsByCategory(category.CategoryID, specAttrColor.SpecificationAttributeID);

                if (saOptions.Count > 0)
                {
                    colorsFilter.Visible = true;
                    List<ColorItem> colors = ColorManager.GetColorsBySAOID(saOptions);
                    colorsFilter.Colors = colors;
                    colorsFilter.CategoryName = categoryName;
                }
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

        private void FillSortBy()
        {
            this.sortBy.Items.Add("По названию");
            this.sortBy.Items.Add("По популярности");
            this.sortBy.Items.Add("По цене вниз");
            this.sortBy.Items.Add("По цене вверх");
            this.sortBy.Items.Add("По новизне");
            this.sortBy.SelectedIndex = CommonHelper.QueryStringInt("sortBy", 4);
        }

        protected void productsCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList drList = (DropDownList)sender;
            string url = CommonHelper.GetThisPageURL(true);
            url = CommonHelper.RemoveQueryString(url, "PageIndex");
            if (drList == sortBy)
            {
                url = CommonHelper.RemoveQueryString(url, "sortBy");
                int sortByIndex = sortBy.SelectedIndex;
                if (sortByIndex != 1)
                {
                    if (!url.Contains("?"))
                        url += "?";
                    else
                        url += "&";

                    Response.Redirect(String.Format("{0}sortBy={1}", url, sortByIndex));
                }
                else { Response.Redirect(url); }
            }
            else
            {
                url = CommonHelper.RemoveQueryString(url, "pageSize");
                Int32.TryParse(productsCount.SelectedItem.Text, out pageSize);
                if (!url.Contains("?"))
                    url += "?";
                else
                    url += "&";

                Response.Redirect(String.Format("{0}pageSize={1}", url, pageSize));
            }
        }

        private SortParameter GetSortParameter()
        {
            SortParameter sortParam;
            switch (sortBy.SelectedIndex)
            {
                case 1:
                    sortParam = new SortParameter(FieldToSort.Popularity, false);
                    break;
                case 2:
                    sortParam = new SortParameter(FieldToSort.Price, false);
                    break;
                case 3:
                    sortParam = new SortParameter(FieldToSort.Price, true);
                    break;
                case 4:
                    sortParam = new SortParameter(FieldToSort.Novelity, false);
                    break;
                default:
                    sortParam = new SortParameter(FieldToSort.Name, true);
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
            Name = 0,
            Popularity = 1,
            Price = 2,
            Novelity = 3
        }
    }
}