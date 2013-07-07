
using System;
using System.Linq;
using System.Web;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Categories;
using NopSolutions.NopCommerce.BusinessLogic.Content.NewsManagement;
using NopSolutions.NopCommerce.BusinessLogic.Content.Topics;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.Web.MasterPages
{
	public partial class root : BaseNopMasterPage
	{
        private void Page_Load(object sender, EventArgs e)
        {
            System.Web.SiteMap.Providers["NopDefaultXmlSiteMapProvider"].SiteMapResolve += this.ExpandCategoryPaths;
        }

        private SiteMapNode ExpandCategoryPaths(Object sender, SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = null;

            if(NewsId != 0)
            {
                currentNode = ChangeNewsMap(e);
            }
            else if (TopicId != 0)
            {
                currentNode = ChangeTopicMap(e);
            }
            else
            {
                if (CategoryId != 0)
                {
                    if (e.Provider.CurrentNode == null)
                    {
                        currentNode = e.Provider.FindSiteMapNode("~/Sitemap-Categories.aspx").Clone(true);
                    }
                    else
                    {
                        currentNode = e.Provider.CurrentNode.Clone(true);
                    }

                    ChangeCategoryMap(e, currentNode);
                }

                if (0 != ProductId)
                {
                    currentNode = ChangeProductMap(e, currentNode);
                }
            }

            return currentNode;
        }

	    private SiteMapNode ChangeNewsMap(SiteMapResolveEventArgs siteMapResolveEventArgs)
	    {
            SiteMapNode currentNode = null;
            try
            {
                currentNode = siteMapResolveEventArgs.Provider.FindSiteMapNode("~/Sitemap-News.aspx").Clone(true);
                News news = NewsManager.GetNewsByID(NewsId);
                if (news != null)
                {
                    currentNode.Url = SEOHelper.GetNewsURL(news);
                    currentNode.Description = news.Title;
                    currentNode.Title = news.Title;
                }
            }
            catch
            {
            }

            return currentNode;
	    }

	    private SiteMapNode ChangeProductMap(SiteMapResolveEventArgs e, SiteMapNode parent)
        {

            SiteMapNode currentProductNode = e.Provider.FindSiteMapNode("~/Sitemap-Products.aspx").Clone(true);
            try
            {
                SiteMapNode tempNode = currentProductNode;
                Product product = ProductManager.GetProductByID(ProductId);
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

        private SiteMapNode ChangeTopicMap(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = null;
            try
            {
                currentNode = e.Provider.FindSiteMapNode("~/Sitemap-Topics.aspx").Clone(true);
                LocalizedTopic localizedTopic =
                TopicManager.GetLocalizedTopic(this.TopicId, NopContext.Current.WorkingLanguage.LanguageID);
                if (localizedTopic != null)
                {
                    currentNode.Url = SEOHelper.GetTopicUrl(TopicId, localizedTopic.Title);
                    currentNode.Description = localizedTopic.Title;
                    currentNode.Title = localizedTopic.Title;
                }
            }
            catch
            {
            }

            return currentNode;
        }

        private void ChangeCategoryMap(SiteMapResolveEventArgs e, SiteMapNode currentNode)
        {
            try
            {
                SiteMapNode tempNode = currentNode;
                Category category = CategoryManager.GetCategoryByID(CategoryId);
                if (0 != CategoryId)
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

        public int CategoryId
        {
            get
            {
                int categoryId = CommonHelper.QueryStringInt("CategoryID");
                try
                {
                    if (categoryId == 0)
                    {
                        Product product = ProductManager.GetProductByID(ProductId);
                        if (product != null)
                        {
                            categoryId =
                                product.ProductCategories.Find(p => p.Category.ParentCategory != null).CategoryID;
                        }
                    }
                }
                catch{//do nothing
                }
                return categoryId;
            }
        }

        public int ProductId
        {
            get
            {
                return CommonHelper.QueryStringInt("ProductID");
            }
        }

        public int TopicId
        {
            get
            {
                return CommonHelper.QueryStringInt("TopicID");
            }
        }

        public int NewsId
        {
            get
            {
                return CommonHelper.QueryStringInt("NewsID");
            }
        }
	}
}
