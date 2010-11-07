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
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using NopSolutions.NopCommerce.BusinessLogic.Categories;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.ExportImport;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Common.Xml;

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class CategoriesControl : BaseNopAdministrationUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            cbShowCategoriesOnMainPage.Checked = SettingManager.GetSettingValueBoolean("Display.ShowCategoriesOnMainPage");

            string menu = GetCategories(0);
            string categoryAddURL = CommonHelper.GetStoreAdminLocation() + "CategoryAdd.aspx";
            menu = string.Format("<siteMapNode title=\"{0}\" url=\"{1}\">" + menu + "<siteMapNode  title=\"{2} \" url=\"{3}\"></siteMapNode></siteMapNode>", GetLocaleResourceString("Admin.Categories.Categories"), string.Empty, GetLocaleResourceString("Admin.Categories.AddNewCategory"), categoryAddURL);

            ds.Data = menu;
            CategoryTreeView.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    SettingManager.SetParam("Display.ShowCategoriesOnMainPage", cbShowCategoriesOnMainPage.Checked.ToString());
                }
                catch (Exception exc)
                {
                    ProcessException(exc);
                }
            }
        }

        protected string GetCategories(int ForParentEntityID)
        {
            StringBuilder tmpS = new StringBuilder(4096);

            CategoryCollection categoryCollection = CategoryManager.GetAllCategories(ForParentEntityID);

            for (int i = 0; i < categoryCollection.Count; i++)
            {
                Category category = categoryCollection[i];
                string categoryDetailsURL = CommonHelper.GetStoreAdminLocation() + "CategoryDetails.aspx?CategoryID=" + category.CategoryID.ToString();
                tmpS.Append("<siteMapNode  title=\"" + XmlHelper.XmlEncodeAttribute(category.Name)
                    + "\" url=\"" + categoryDetailsURL + "\">");
                if (CategoryManager.GetAllCategories(category.CategoryID).Count > 0)
                    tmpS.Append(GetCategories(category.CategoryID));

                string categoryAddURL = CommonHelper.GetStoreAdminLocation() + "CategoryAdd.aspx?ParentCategoryID=" + category.CategoryID.ToString();
                tmpS.Append("<siteMapNode  title=\"" + GetLocaleResourceString("Admin.Categories.AddNewCategory") + "\" url=\"" + categoryAddURL + "\"></siteMapNode>");

                bool hideProducts = SettingManager.GetSettingValueBoolean("Display.HideProductsOnCategoriesHomePage");
                if (!hideProducts)
                {
                    tmpS.Append("<siteMapNode  title=\"" + GetLocaleResourceString("Admin.Categories.Products") + "\" url=\"" + string.Empty + "\">");
                    //string productAddURL = CommonHelper.GetStoreAdminLocation() + "ProductAdd.aspx?CategoryID=" + category.CategoryID.ToString();
                    //tmpS.Append("<siteMapNode  title=\"Add new product:\" url=\"" + productAddURL + "\"></siteMapNode>");
                    int totalRecords = 0;
                    ProductCollection products = ProductManager.GetAllProducts(category.CategoryID, 0, null, int.MaxValue, 0, out totalRecords);
                    foreach (Product product in products)
                    {
                        string productDetailsURL = CommonHelper.GetStoreAdminLocation() + "ProductDetails.aspx?ProductID=" + product.ProductID.ToString();
                        tmpS.Append("<siteMapNode  title=\"" + XmlHelper.XmlEncodeAttribute(product.Name) + "\" url=\"" + productDetailsURL + "\">");
                        tmpS.Append("</siteMapNode>");
                    }
                    tmpS.Append("</siteMapNode>");
                }

                tmpS.Append("</siteMapNode>");
            }
            return tmpS.ToString();
        }

        protected void btnExportXML_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    string fileName = string.Format("categories_{0}.xml", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    string xml = ExportManager.ExportCategoriesToXML();
                    CommonHelper.WriteResponseXML(xml, fileName);
                }
                catch (Exception exc)
                {
                    ProcessException(exc);
                }
            }
        }
    }
}