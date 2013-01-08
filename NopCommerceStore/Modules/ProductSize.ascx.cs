using System;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.Web.MasterPages;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class ProductSize : BaseNopUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected string GetMaxHeight
        {
            get
            {
                decimal maxHeight = ProductManager.GetMaxHeight(((TwoColumn)Page.Master).CategoryID);
                int maxHeightInt = Convert.ToInt32(maxHeight);
                return maxHeightInt.ToString();
            }
        }

        protected string GetMaxWidth
        {
            get
            {
                decimal maxWidth = ProductManager.GetMaxWidth(((TwoColumn)Page.Master).CategoryID);
                int maxWidthInt = Convert.ToInt32(maxWidth);
                return maxWidthInt.ToString();
            }
        }
    }
}