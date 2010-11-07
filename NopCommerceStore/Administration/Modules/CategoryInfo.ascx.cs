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
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic.Categories;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Promo.Discounts;
using NopSolutions.NopCommerce.BusinessLogic.Templates;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Web.Administration.Modules;
 

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class CategoryInfoControl : BaseNopAdministrationUserControl
    {
        private void BindData()
        {
            Category category = CategoryManager.GetCategoryByID(this.CategoryID);

            if (category != null)
            {
                this.txtName.Text = category.Name;
                this.txtDescription.Value = category.Description;
                CommonHelper.SelectListItem(this.ddlTemplate, category.TemplateID);
                ParentCategory.SelectedCategoryId = category.ParentCategoryID;

                Picture categoryPicture = category.Picture;
                this.btnRemoveCategoryImage.Visible = categoryPicture != null;
                string pictureUrl = PictureManager.GetPictureUrl(categoryPicture, 100);
                this.iCategoryPicture.Visible = true;
                this.iCategoryPicture.ImageUrl = pictureUrl;

                this.txtPriceRanges.Text = category.PriceRanges;
                this.cbPublished.Checked = category.Published;
                this.txtDisplayOrder.Value = category.DisplayOrder;
                this.ParentCategory.BindData();
            }
            else
            {
                this.btnRemoveCategoryImage.Visible = false;
                this.iCategoryPicture.Visible = false;

                ParentCategory.SelectedCategoryId = this.ParentCategoryID;
                ParentCategory.BindData();
            }
        }

        private void FillDropDowns()
        {
            this.ddlTemplate.Items.Clear();
            CategoryTemplateCollection categoryTemplates = TemplateManager.GetAllCategoryTemplates();
            foreach (CategoryTemplate categoryTemplate in categoryTemplates)
            {
                ListItem item2 = new ListItem(categoryTemplate.Name, categoryTemplate.CategoryTemplateID.ToString());
                this.ddlTemplate.Items.Add(item2);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.FillDropDowns();
                this.BindData();
            }
        }

        public Category SaveInfo()
        {
            Category category = CategoryManager.GetCategoryByID(this.CategoryID);

            if (category != null)
            {
                Picture categoryPicture = category.Picture;
                HttpPostedFile categoryPictureFile = fuCategoryPicture.PostedFile;
                if ((categoryPictureFile != null) && (!String.IsNullOrEmpty(categoryPictureFile.FileName)))
                {
                    byte[] categoryPictureBinary = PictureManager.GetPictureBits(categoryPictureFile.InputStream, categoryPictureFile.ContentLength);
                    if (categoryPicture != null)
                        categoryPicture = PictureManager.UpdatePicture(categoryPicture.PictureID, categoryPictureBinary, categoryPictureFile.ContentType, true);
                    else
                        categoryPicture = PictureManager.InsertPicture(categoryPictureBinary, categoryPictureFile.ContentType, true);
                }
                int categoryPictureID = 0;
                if (categoryPicture != null)
                    categoryPictureID = categoryPicture.PictureID;

                category = CategoryManager.UpdateCategory(category.CategoryID, txtName.Text, txtDescription.Value, int.Parse(this.ddlTemplate.SelectedItem.Value),
                     category.MetaKeywords, category.MetaDescription, category.MetaTitle, category.SEName, ParentCategory.SelectedCategoryId,
                    categoryPictureID, category.PageSize, txtPriceRanges.Text, cbPublished.Checked, category.Deleted, txtDisplayOrder.Value, category.CreatedOn, DateTime.Now);
            }
            else
            {
                Picture categoryPicture = null;
                HttpPostedFile categoryPictureFile = fuCategoryPicture.PostedFile;
                if ((categoryPictureFile != null) && (!String.IsNullOrEmpty(categoryPictureFile.FileName)))
                {
                    byte[] categoryPictureBinary = PictureManager.GetPictureBits(categoryPictureFile.InputStream, categoryPictureFile.ContentLength);
                    categoryPicture = PictureManager.InsertPicture(categoryPictureBinary, categoryPictureFile.ContentType, true);
                }
                int categoryPictureID = 0;
                if (categoryPicture != null)
                    categoryPictureID = categoryPicture.PictureID;

                DateTime nowDT = DateTime.Now;
                category = CategoryManager.InsertCategory(txtName.Text, txtDescription.Value, int.Parse(this.ddlTemplate.SelectedItem.Value),
                         string.Empty, string.Empty, string.Empty, string.Empty, ParentCategory.SelectedCategoryId,
                         categoryPictureID, 10, txtPriceRanges.Text, cbPublished.Checked, false, txtDisplayOrder.Value, nowDT, nowDT);
            }
            return category;
        }

        protected void btnRemoveCategoryImage_Click(object sender, EventArgs e)
        {
            try
            {
                Category category = CategoryManager.GetCategoryByID(this.CategoryID);
                if (category != null)
                {
                    PictureManager.DeletePicture(category.PictureID);
                    CategoryManager.RemoveCategoryPicture(category.CategoryID);
                    BindData();
                }
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        public int ParentCategoryID
        {
            get
            {
                return CommonHelper.QueryStringInt("ParentCategoryID");
            }
        }

        public int CategoryID
        {
            get
            {
                return CommonHelper.QueryStringInt("CategoryID");
            }
        }
    }
}