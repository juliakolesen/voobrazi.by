using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NopSolutions.NopCommerce.BusinessLogic.Categories;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.Audit;
using System.Xml.Linq;
using System.Configuration;

namespace NopSolutions.NopCommerce.Web
{
    public partial class SiteMap : BaseNopPage
    {
        protected override void OnPreRender(EventArgs e)
        {
            Page.Title = "Карта сайта";
            BindData();
            base.OnPreRender(e);
        }

        protected void BindData()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                if (SettingManager.GetSettingValueBoolean("Sitemap.IncludeCategories", true))
                {
                    sb.Append("<li>");
                    sb.AppendFormat("<span>{0}</span>", GetLocaleResourceString("Sitemap.Categories"));
                    sb.Append("<ul>");
                    WriteCategories(sb, CategoryManager.GetAllCategories(0));
                    sb.Append("</ul>");
                    sb.Append("</li>");
                }

                WriteOtherPagesFromXml(sb);//After adding a new page you'll need add it into Pages.xml file

                lSitemapContent.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                lSitemapContent.Text = ex.Message;
                LogManager.InsertLog(LogTypeEnum.CommonError, ex.Message, ex);
            }
        }

        private void WriteCategories(StringBuilder sb, List<Category> categoryCollection)
        {
            foreach (Category category in categoryCollection)
            {
                sb.Append("<li>");
                sb.AppendFormat("<a href=\"{0}\">{1}</a>", SEOHelper.GetCategoryURL(category.CategoryID), Server.HtmlEncode(category.Name));
                var childCategoryCollection = CategoryManager.GetAllCategories(category.CategoryID);
                if (childCategoryCollection.Count > 0)
                {
                    sb.Append("<ul>");
                    WriteCategories(sb, childCategoryCollection);
                    sb.Append("</ul>");
                }
                else
                {
                    int totalCount;
                    var productCollection = ProductManager.GetAllProducts(category.CategoryID,
                                            0, false, int.MaxValue - 1, 0, out totalCount);
                    if (productCollection.Count > 0)
                    {
                        sb.Append("<ul>");
                        WriteProducts(sb, productCollection.ToList());
                        sb.Append("</ul>");
                    }
                }

                sb.Append("</li>");
            }
        }

        private void WriteProducts(StringBuilder sb, List<Product> productCollection)
        {
            foreach (Product product in productCollection)
            {
                string shortDescription = String.IsNullOrEmpty(product.ShortDescription)? ""
                    : String.Format(" - {0}", product.ShortDescription);
                sb.AppendFormat("<li><a href=\"{0}\">{1}</a>{2}</li>",
                    SEOHelper.GetProductURL(product), Server.HtmlEncode(product.Name), shortDescription);
            }
        }

        private void WriteOtherPagesFromXml(StringBuilder sb)
        {
            if (ConfigurationSettings.AppSettings != null)
            {
                string path = Server.MapPath(".") + ConfigurationSettings.AppSettings["PagesWithUrl"];
                XDocument doc = XDocument.Load(path);
                List<PageWithUrl> pages = doc.Element("Pages").Elements("Page").Select(
                    p => new PageWithUrl()
                             {
                                 Name = p.Element("Name").Value,
                                 Url = p.Element("Url").Value
                             }).ToList();

                foreach (var page in pages)
                {
                    sb.AppendFormat("<li><a href=\"{0}{1}\">{2}</a></li>", CommonHelper.GetStoreLocation(), page.Url.Trim(), Server.HtmlEncode(page.Name));
                }
            }
        }

        private class PageWithUrl
        {
            public string Name { get; set; }
            public string Url { get; set; }
        }
    }
}