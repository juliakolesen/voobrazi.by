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
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic.Categories;
using NopSolutions.NopCommerce.BusinessLogic.Manufacturers;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Products.Specs;
using NopSolutions.NopCommerce.BusinessLogic.Templates;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Web.Administration.Modules;

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class ProductPicturesControl : BaseNopAdministrationUserControl
    {
        private void BindData()
        {
            Product product = ProductManager.GetProductByID(this.ProductID);
            if (product != null)
            {
                ProductPictureCollection productPictures = product.ProductPictures;
                if (productPictures.Count > 0)
                {
                    gvwImages.Visible = true;
                    gvwImages.DataSource = productPictures;
                    gvwImages.DataBind();
                }
                else
                    gvwImages.Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.BindData();
            }
        }

        public void SaveInfo()
        {
        }

        protected void btnUploadProductPicture_Click(object sender, EventArgs e)
        {
            try
            {
                Product product = ProductManager.GetProductByID(this.ProductID);
                if (product != null)
                {
                    Picture picture = null;
                    HttpPostedFile productPictureFile = fuProductPicture.PostedFile;
                    if ((productPictureFile != null) && (!String.IsNullOrEmpty(productPictureFile.FileName)))
                    {
                        byte[] productPictureBinary = PictureManager.GetPictureBits(productPictureFile.InputStream, productPictureFile.ContentLength);
                        picture = PictureManager.InsertPicture(productPictureBinary, productPictureFile.ContentType, true);
                    }
                    if (picture != null)
                    {
                        ProductPicture productPicture = ProductManager.InsertProductPicture(product.ProductID, picture.PictureID, txtProductPictureDisplayOrder.Value);
                    }

                    BindData();
                }
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        protected void gvwImages_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UpdateProductPicture")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvwImages.Rows[index];
                NumericTextBox txtProductPictureDisplayOrder = row.FindControl("txtProductPictureDisplayOrder") as NumericTextBox;
                HiddenField hfProductPictureID = row.FindControl("hfProductPictureID") as HiddenField;

                int displayOrder = txtProductPictureDisplayOrder.Value;
                int productPictureID = int.Parse(hfProductPictureID.Value);
                ProductPicture productPicture = ProductManager.GetProductPictureByID(productPictureID);

                if (productPicture != null)
                    ProductManager.UpdateProductPicture(productPicture.ProductPictureID,
                       productPicture.ProductID, productPicture.PictureID, displayOrder);

                BindData();
            }
        }

        protected void gvwImages_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ProductPicture productPicture = (ProductPicture)e.Row.DataItem;
                Image iProductPicture = e.Row.FindControl("iProductPicture") as Image;
                if (iProductPicture != null)
                    iProductPicture.ImageUrl = PictureManager.GetPictureUrl(productPicture.PictureID);

                Button btnUpdate = e.Row.FindControl("btnUpdate") as Button;
                if (btnUpdate != null)
                    btnUpdate.CommandArgument = e.Row.RowIndex.ToString();
            }
        }

        protected void gvwImages_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int productPictureID = (int)gvwImages.DataKeys[e.RowIndex]["ProductPictureID"];
            ProductPicture productPicture = ProductManager.GetProductPictureByID(productPictureID);
            if (productPicture != null)
            {
                PictureManager.DeletePicture(productPicture.PictureID);
                ProductManager.DeleteProductPicture(productPicture.ProductPictureID);
                BindData();
            }
        }

        public int ProductID
        {
            get
            {
                return CommonHelper.QueryStringInt("ProductID");
            }
        }
    }
}