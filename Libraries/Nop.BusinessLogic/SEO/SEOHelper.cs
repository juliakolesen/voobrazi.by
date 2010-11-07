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
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using NopSolutions.NopCommerce.BusinessLogic.Categories;
using NopSolutions.NopCommerce.BusinessLogic.Configuration;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.Content.Blog;
using NopSolutions.NopCommerce.BusinessLogic.Content.Forums;
using NopSolutions.NopCommerce.BusinessLogic.Content.NewsManagement;
using NopSolutions.NopCommerce.BusinessLogic.Content.Topics;
using NopSolutions.NopCommerce.BusinessLogic.Manufacturers;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Utils;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.BusinessLogic.SEO
{
    /// <summary>
    /// Represents a SEO helper
    /// </summary>
    public partial class SEOHelper
    {
        #region Methods
        /// <summary>
        /// Renders page meta tag
        /// </summary>
        /// <param name="page">Page instance</param>
        /// <param name="name">Meta name</param>
        /// <param name="content">Content</param>
        /// <param name="OverwriteExisting">Overwrite existing content if exists</param>
        public static void RenderMetaTag(Page page, string name, string content, bool OverwriteExisting)
        {
            if (page == null || page.Header == null)
                return;

            if (content == null)
                content = string.Empty;

            foreach (Control control in page.Header.Controls)
                if (control is HtmlMeta)
                {
                    HtmlMeta meta = (HtmlMeta)control;
                    if (meta.Name.ToLower().Equals(name.ToLower()) && !string.IsNullOrEmpty(content))
                    {
                        if (OverwriteExisting)
                            meta.Content = content;
                        else
                        {
                            if (String.IsNullOrEmpty(meta.Content))
                                meta.Content = content;
                        }
                    }
                }
        }

        /// <summary>
        /// Renders page title
        /// </summary>
        /// <param name="page">Page instance</param>
        /// <param name="title">Page title</param>
        /// <param name="OverwriteExisting">Overwrite existing content if exists</param>
        public static void RenderTitle(Page page, string title, bool OverwriteExisting)
        {
            bool includeStoreNameInTitle = SettingManager.GetSettingValueBoolean("SEO.IncludeStoreNameInTitle");
            RenderTitle(page, title, includeStoreNameInTitle, OverwriteExisting);
        }

        /// <summary>
        /// Renders page title
        /// </summary>
        /// <param name="page">Page instance</param>
        /// <param name="title">Page title</param>
        /// <param name="IncludeStoreNameInTitle">Include store name in title</param>
        /// <param name="OverwriteExisting">Overwrite existing content if exists</param>
        public static void RenderTitle(Page page, string title, bool IncludeStoreNameInTitle, bool OverwriteExisting)
        {
            if (page == null || page.Header == null)
                return;

            if (IncludeStoreNameInTitle)
                title = SettingManager.StoreName + ". " + title;

            if (String.IsNullOrEmpty(title))
                return;

            if (OverwriteExisting)
                page.Title = HttpUtility.HtmlEncode(title);
            else
            {
                if (String.IsNullOrEmpty(page.Title))
                    page.Title = HttpUtility.HtmlEncode(title);
            }
        }

        /// <summary>
        /// Renders an RSS link to the page
        /// </summary>
        /// <param name="page">Page instance</param>
        /// <param name="title">RSS Title</param>
        /// <param name="href">Path to the RSS feed</param>
        public static void RenderHeaderRSSLink(Page page, string title, string href)
        {
            if (page == null || page.Header == null)
                return;

            HtmlGenericControl link = new HtmlGenericControl("link");
            link.Attributes.Add("href", href);
            link.Attributes.Add("type", "application/rss+xml");
            link.Attributes.Add("rel", "alternate");
            link.Attributes.Add("title", title);
            page.Header.Controls.Add(link);
        }

        /// <summary>
        /// Get SE name
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>Result</returns>
        public static string GetSEName(string name)
        {
            if (String.IsNullOrEmpty(name))
                return string.Empty;
            string OKChars = "abcdefghijklmnopqrstuvwxyz1234567890 _-";
            name = name.Trim().ToLowerInvariant();
            StringBuilder sb = new StringBuilder();
            foreach (char c in name.ToCharArray())
                if (OKChars.Contains(c.ToString()))
                    sb.Append(c);
            string name2 = sb.ToString();
            name2 = name2.Replace(" ", "-");
            while (name2.Contains("--"))
                name2 = name2.Replace("--", "-");
            while (name2.Contains("__"))
                name2 = name2.Replace("__", "_");
            return HttpContext.Current.Server.UrlEncode(name2);
        }

        /// <summary>
        /// Gets product URL
        /// </summary>
        /// <param name="ProductID">Product identifier</param>
        /// <returns>Product URL</returns>
        public static string GetProductURL(int ProductID)
        {
            Product product = ProductManager.GetProductByID(ProductID);
            return GetProductURL(product);
        }

        /// <summary>
        /// Gets product URL
        /// </summary>
        /// <param name="product">Product</param>
        /// <returns>Product URL</returns>
        public static string GetProductURL(Product product)
        {
            if (product == null)
                throw new ArgumentNullException("product");
            string seName = GetSEName(product.SEName);
            if (String.IsNullOrEmpty(seName))
            {
                seName = GetSEName(product.Name);
            }
            string url = string.Format(SettingManager.GetSettingValue("SEO.Product.UrlRewriteFormat"), CommonHelper.GetStoreLocation(), product.ProductID, seName);
            return url;
        }

        /// <summary>
        /// Gets product email a friend URL
        /// </summary>
        /// <param name="ProductID">Product identifier</param>
        /// <returns>Product email a friend URL</returns>
        public static string GetProductEmailAFriendURL(int ProductID)
        {
            string url = string.Format("{0}ProductEmailAFriend.aspx?ProductID={1}", CommonHelper.GetStoreLocation(), ProductID);
            return url;
        }

        /// <summary>
        /// Gets manufacturer URL
        /// </summary>
        /// <param name="ManufacturerID">Manufacturer identifier</param>
        /// <returns>Manufacturer URL</returns>
        public static string GetManufacturerURL(int ManufacturerID)
        {
            Manufacturer manufacturer = ManufacturerManager.GetManufacturerByID(ManufacturerID);
            return GetManufacturerURL(manufacturer);
        }

        /// <summary>
        /// Gets manufacturer URL
        /// </summary>
        /// <param name="manufacturer">Manufacturer</param>
        /// <returns>Manufacturer URL</returns>
        public static string GetManufacturerURL(Manufacturer manufacturer)
        {
            if (manufacturer == null)
                throw new ArgumentNullException("manufacturer");
            string seName = GetSEName(manufacturer.SEName);
            if (String.IsNullOrEmpty(seName))
            {
                seName = GetSEName(manufacturer.Name);
            } 
            string url = string.Format(SettingManager.GetSettingValue("SEO.Manufacturer.UrlRewriteFormat"), CommonHelper.GetStoreLocation(), manufacturer.ManufacturerID, seName);
            return url;
        }

        /// <summary>
        /// Gets category URL
        /// </summary>
        /// <param name="CategoryID">Category identifier</param>
        /// <returns>Category URL</returns>
        public static string GetCategoryURL(int CategoryID)
        {
            Category category = CategoryManager.GetCategoryByID(CategoryID);
            return GetCategoryURL(category);
        }

        /// <summary>
        /// Gets category URL
        /// </summary>
        /// <param name="category">Category</param>
        /// <returns>Category URL</returns>
        public static string GetCategoryURL(Category category)
        {
            if (category == null)
                throw new ArgumentNullException("category"); 
            string seName = GetSEName(category.SEName);
            if (String.IsNullOrEmpty(seName))
            {
                seName = GetSEName(category.Name);
            }
            string url = string.Format(SettingManager.GetSettingValue("SEO.Category.UrlRewriteFormat"), CommonHelper.GetStoreLocation(), category.CategoryID, seName);
            return url;
        }

        /// <summary>
        /// Gets blog post URL
        /// </summary>
        /// <param name="BlogPostID">Blog post identifier</param>
        /// <returns>Blog post URL</returns>
        public static string GetBlogPostURL(int BlogPostID)
        {
            BlogPost blogPost = BlogManager.GetBlogPostByID(BlogPostID);
            return GetBlogPostURL(blogPost);
        }

        /// <summary>
        /// Gets blog post URL
        /// </summary>
        /// <param name="blogPost">Blog post</param>
        /// <returns>Blog post URL</returns>
        public static string GetBlogPostURL(BlogPost blogPost)
        {
            if (blogPost == null)
                throw new ArgumentNullException("blogPost"); 
            string seName = GetSEName(blogPost.BlogPostTitle);
            string url = string.Format(SettingManager.GetSettingValue("SEO.Blog.UrlRewriteFormat"), CommonHelper.GetStoreLocation(), blogPost.BlogPostID, seName);
            return url;
        }

        /// <summary>
        /// Gets news URL
        /// </summary>
        /// <param name="NewsID">News identifier</param>
        /// <returns>News URL</returns>
        public static string GetNewsURL(int NewsID)
        {
            News news = NewsManager.GetNewsByID(NewsID);
            return GetNewsURL(news);
        }

        /// <summary>
        /// Gets news URL
        /// </summary>
        /// <param name="news">News item</param>
        /// <returns>News URL</returns>
        public static string GetNewsURL(News news)
        {
            if (news == null)
                throw new ArgumentNullException("news"); 
            string seName = GetSEName(news.Title);
            string url = string.Format(SettingManager.GetSettingValue("SEO.News.UrlRewriteFormat"), CommonHelper.GetStoreLocation(), news.NewsID, seName);
            return url;
        }

        /// <summary>
        /// Gets news Rss URL
        /// </summary>
        /// <returns>News Rss URL</returns>
        public static string GetNewsRssURL()
        {
            return GetNewsRssURL(NopContext.Current.WorkingLanguage.LanguageID);
        }

        /// <summary>
        /// Gets news Rss URL
        /// </summary>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>News Rss URL</returns>
        public static string GetNewsRssURL(int LanguageID)
        {
            return CommonHelper.GetStoreLocation() + "NewsRSS.aspx?LanguageID=" + LanguageID.ToString();
        }

        /// <summary>
        /// Gets blog Rss URL
        /// </summary>
        /// <returns>Blog Rss URL</returns>
        public static string GetBlogRssURL()
        {
            return GetBlogRssURL(NopContext.Current.WorkingLanguage.LanguageID);
        }

        /// <summary>
        /// Gets blog Rss URL
        /// </summary>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Blog Rss URL</returns>
        public static string GetBlogRssURL(int LanguageID)
        {
            return CommonHelper.GetStoreLocation() + "BlogRSS.aspx?LanguageID=" + LanguageID.ToString();
        }

        /// <summary>
        /// Gets forum URL
        /// </summary>
        /// <returns>Forum URL</returns>
        public static string GetForumMainURL()
        {
            string url = string.Format("{0}Boards/", CommonHelper.GetStoreLocation());
            return url;
        }

        /// <summary>
        /// Gets forum group URL
        /// </summary>
        /// <param name="ForumGroupID">Forum group identifier</param>
        /// <returns>Forum group URL</returns>
        public static string GetForumGroupURL(int ForumGroupID)
        {
            ForumGroup forumGroup = ForumManager.GetForumGroupByID(ForumGroupID);
            return GetForumGroupURL(forumGroup);
        }

        /// <summary>
        /// Gets forum group URL
        /// </summary>
        /// <param name="forumGroup">Forum group</param>
        /// <returns>Forum group URL</returns>
        public static string GetForumGroupURL(ForumGroup forumGroup)
        {
            if (forumGroup == null)
                throw new ArgumentNullException("forumGroup");

            string url = string.Format("{0}Boards/ForumGroup.aspx?ForumGroupID={1}", CommonHelper.GetStoreLocation(), forumGroup.ForumGroupID);
            return url;
        }

        /// <summary>
        /// Gets forum URL
        /// </summary>
        /// <param name="ForumID">Forum identifier</param>
        /// <returns>Forum URL</returns>
        public static string GetForumURL(int ForumID)
        {
            Forum forum = ForumManager.GetForumByID(ForumID);
            return GetForumURL(forum);
        }

        /// <summary>
        /// Gets forum URL
        /// </summary>
        /// <param name="forum">Forum</param>
        /// <returns>Forum URL</returns>
        public static string GetForumURL(Forum forum)
        {
            if (forum == null)
                throw new ArgumentNullException("forum");

            string url = string.Format("{0}Boards/Forum.aspx?ForumID={1}", CommonHelper.GetStoreLocation(), forum.ForumID);
            return url;
        }

        /// <summary>
        /// Gets move topic URL
        /// </summary>
        /// <param name="forumTopic">Forum topic</param>
        /// <returns>Forum URL</returns>
        public static string GetMoveForumTopicURL(ForumTopic forumTopic)
        {
            if (forumTopic == null)
                throw new ArgumentNullException("forumTopic");

            string url = string.Format("{0}Boards/MoveTopic.aspx?TopicID={1}", CommonHelper.GetStoreLocation(), forumTopic.ForumTopicID);
            return url;
        }

        /// <summary>
        /// Gets forum search URL
        /// </summary>
        /// <param name="ForumID">Forum identifier</param>
        /// <returns>Forum URL</returns>
        public static string GetForumSearchURL(string SearchTerms)
        {
            string url = string.Format("{0}Boards/Search.aspx?searchTerms={1}", CommonHelper.GetStoreLocation(), HttpUtility.UrlEncode(SearchTerms));
            return url;
        }

        /// <summary>
        /// Gets forum topic URL
        /// </summary>
        /// <param name="TopicID">Forum topic identifier</param>
        /// <returns>Forum topic URL</returns>
        public static string GetForumTopicURL(int TopicID)
        {
            return GetForumTopicURL(TopicID, "p", null); 
        }

        /// <summary>
        /// Gets forum topic URL
        /// </summary>
        /// <param name="TopicID">Forum topic identifier</param>
        /// <param name="QueryStringProperty">Query string property</param>
        /// <param name="PageIndex">Page index</param>
        /// <returns>Forum topic URL</returns>
        public static string GetForumTopicURL(int TopicID, string QueryStringProperty, int? PageIndex)
        {
            return GetForumTopicURL(TopicID, QueryStringProperty, PageIndex, null);
        }

        /// <summary>
        /// Gets forum topic URL
        /// </summary>
        /// <param name="TopicID">Forum topic identifier</param>
        /// <param name="QueryStringProperty">Query string property</param>
        /// <param name="PageIndex">Page index</param>
        /// <param name="PostID">Post identifier (anchor)</param>
        /// <returns>Forum topic URL</returns>
        public static string GetForumTopicURL(int TopicID, string QueryStringProperty, 
            int? PageIndex, int? PostID)
        {
            string url = string.Empty;
            url = string.Format("{0}Boards/Topic.aspx?TopicID={1}", CommonHelper.GetStoreLocation(), TopicID);
            if (PageIndex.HasValue && PageIndex.Value > 1)
            {
                url += string.Format("&{0}={1}", QueryStringProperty, PageIndex.Value);
            }
            if (PostID.HasValue)
            {
                url += string.Format("#{0}", PostID.Value);
            }
            return url;
        }

        /// <summary>
        /// Gets new forum topic URL
        /// </summary>
        /// <param name="ForumID">Forum identifier</param>
        /// <returns>New forum topic URL</returns>
        public static string GetNewForumTopicURL(int ForumID)
        {
            string url = string.Format("{0}Boards/TopicNew.aspx?ForumID={1}", CommonHelper.GetStoreLocation(), ForumID);
            return url;
        }

        /// <summary>
        /// Gets edit topic URL
        /// </summary>
        /// <param name="TopicID">Forum post identifier</param>
        /// <returns>Edit forum post URL</returns>
        public static string GetEditForumTopicURL(int TopicID)
        {
            string url = string.Format("{0}Boards/TopicEdit.aspx?TopicID={1}", CommonHelper.GetStoreLocation(), TopicID);
            return url;
        }

        /// <summary>
        /// Gets new forum post URL
        /// </summary>
        /// <param name="TopicID">Forum topic identifier</param>
        /// <returns>New forum post URL</returns>
        public static string GetNewForumPostURL(int TopicID)
        {
            string url = string.Format("{0}Boards/PostNew.aspx?TopicID={1}", CommonHelper.GetStoreLocation(), TopicID);
            return url;
        }

        /// <summary>
        /// Gets edit post URL
        /// </summary>
        /// <param name="ForumPostID">Forum post identifier</param>
        /// <returns>Edit forum post URL</returns>
        public static string GetEditForumPostURL(int ForumPostID)
        {
            string url = string.Format("{0}Boards/PostEdit.aspx?PostID={1}", CommonHelper.GetStoreLocation(), ForumPostID);
            return url;
        }

        /// <summary>
        /// Gets forum user profile URL
        /// </summary>
        /// <param name="UserID">User identifier</param>
        /// <returns>Forum topic URL</returns>
        public static string GetUserProfileURL(int UserID)
        {
            string url = string.Format("{0}Profile.aspx?UserID={1}", CommonHelper.GetStoreLocation(), UserID);
            return url;
        }

        /// <summary>
        /// Gets Topic page URL
        /// </summary>
        /// <param name="TopicID">Topic identifier</param>
        /// <param name="Title">Localized topic title</param>
        /// <returns>Topic page URL</returns>
        public static string GetTopicUrl(int TopicID, string Title)
        {
            string url = string.Format(SettingManager.GetSettingValue("SEO.Topic.UrlRewriteFormat"), CommonHelper.GetStoreLocation(), TopicID, GetSEName(Title));
            return url;
        }

        #endregion
    }
}
