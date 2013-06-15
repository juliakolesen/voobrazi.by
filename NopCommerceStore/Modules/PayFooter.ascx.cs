using System;
using System.Web;
using System.Web.UI.WebControls;
using NopSolutions.NopCommerce.BusinessLogic.Orders;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class PayFooter : BaseNopUserControl
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack && Request.Cookies["Currency"] != null)
            {
                SetActiveCurrency(Request.Cookies["Currency"].Value != "BYR");
            }
            else
            {
                SetActiveCurrency(false);
            }
        }

        private void SetActiveCurrency(bool isUsd)
        {
            lbtnUSD.Font.Bold = isUsd;
            lbtnBr.Font.Bold = !isUsd;
        }

        protected int GetCount()
        {
            int cartCount = ShoppingCartManager.GetCurrentShoppingCart(ShoppingCartTypeEnum.ShoppingCart).Count;
            int indOrders = IndividualOrderManager.GetCurrentUserIndividualOrders().Count;
            return cartCount + indOrders;
        }

        protected void OnCurrencyChange(object sender, EventArgs e)
        {
            Response.Cookies.Add(new HttpCookie("Currency", (sender as LinkButton).Text));
            Response.Redirect(Request.Url.ToString());
        }

    }
}