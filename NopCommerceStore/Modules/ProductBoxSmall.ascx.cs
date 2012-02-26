using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NopSolutions.NopCommerce.Web;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Tax;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class ProductBoxSmall : BaseNopUserControl
    {
        Product product;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public override void DataBind()
        {
            base.DataBind();
            BindData();
        }

        private void BindData()
        {
            if (product != null)
            {
                string productURL = SEOHelper.GetProductURL(product);

                ProductPictureCollection productPictures = product.ProductPictures;
                if (productPictures.Count > 0)
                {
                    hlImageLink.ImageUrl = PictureManager.GetPictureUrl(productPictures[0].Picture, ProductImageSize, true);
                    hlImageLink.ToolTip = String.Format(GetLocaleResourceString("Media.Product.ImageLinkTitleFormat"), product.Name);
                    hlImageLink.Text = String.Format(GetLocaleResourceString("Media.Product.ImageAlternateTextFormat"), product.Name);
                }
                else
                {
                    hlImageLink.ImageUrl = PictureManager.GetDefaultPictureUrl(this.ProductImageSize);
                    hlImageLink.ToolTip = String.Format(GetLocaleResourceString("Media.Product.ImageLinkTitleFormat"), product.Name);
                    hlImageLink.Text = String.Format(GetLocaleResourceString("Media.Product.ImageAlternateTextFormat"), product.Name);
                }
                hlImageLink.NavigateUrl = productURL;
            }
        }

        public Product Product
        {
            get
            {
                return product;
            }
            set
            {
                product = value;
            }
        }

        public int ProductImageSize
        {
            get
            {
                return 100;
            }
        }
    }
}