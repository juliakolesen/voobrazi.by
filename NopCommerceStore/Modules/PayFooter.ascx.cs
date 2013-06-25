using System;
using System.Web;
using System.Web.UI.WebControls;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Utils;

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


        protected string GetShoppingCartSum()
        {
            ShoppingCart cart = ShoppingCartManager.GetCurrentShoppingCart(ShoppingCartTypeEnum.ShoppingCart);
            bool isUsd = Request.Cookies["Currency"] != null && Request.Cookies["Currency"].Value == "USD";
            decimal indOrderTotal = 0;
            IndividualOrderCollection indOrders = new IndividualOrderCollection();
            if (NopContext.Current.Session != null)
            {
                Guid customerSessionGuid = NopContext.Current.Session.CustomerSessionGUID;
                indOrders = IndividualOrderManager.GetIndividualOrderByCurrentUserSessionGuid(customerSessionGuid);
                indOrderTotal = IndividualOrderManager.GetTotalPriceIndOrders(indOrders);
                if (isUsd)
                {
                    indOrderTotal = Math.Round(PriceConverter.ToUsd(indOrderTotal));
                }
            }

            if (cart.Count > 0 || indOrders.Count > 0)
            {
                //subtotal
                string subTotalError = string.Empty;
                decimal shoppingCartSubTotalDiscountBase;
                decimal shoppingCartSubTotalBase = ShoppingCartManager.GetShoppingCartSubTotal(cart, NopContext.Current.User,
                                                                                               out shoppingCartSubTotalDiscountBase,
                                                                                               ref subTotalError);
                if (String.IsNullOrEmpty(subTotalError))
                {
                    decimal shoppingCartSubTotal = CurrencyManager.ConvertCurrency(shoppingCartSubTotalBase,
                                                                                   CurrencyManager.PrimaryStoreCurrency,
                                                                                   NopContext.Current.WorkingCurrency);
                    shoppingCartSubTotal += indOrderTotal;
                    return AddCurrency(PriceHelper.FormatPrice(shoppingCartSubTotal), isUsd);
                }
            }

            return AddCurrency("0", isUsd);
        }

        private string AddCurrency(string price, bool isUsd)
        {
            return String.Format("{0} {1}.", price, isUsd ? "usd" : "руб");
        }
    }
}