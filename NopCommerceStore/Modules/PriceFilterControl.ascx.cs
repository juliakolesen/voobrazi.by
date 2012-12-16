using System;
using System.Web;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Utils;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class PriceFilterControl : BaseNopUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected string GetMaxValue
        {
            get
            {
                decimal maxPrice = ProductManager.GetMaxPrice(CategoryId);
                if (HttpContext.Current.Request.Cookies["Currency"] != null 
                    && HttpContext.Current.Request.Cookies["Currency"].Value == "USD")
                {
                    maxPrice = Math.Round(PriceConverter.ToUsd(maxPrice));
                }

                int maxPriceInt = Convert.ToInt32(maxPrice);
                return maxPriceInt.ToString();
            }
        }

        private int CategoryId
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

        private int ProductID
        {
            get
            {
                return CommonHelper.QueryStringInt("ProductID");
            }
        }
    }
}