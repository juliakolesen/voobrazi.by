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
                ddlCur1.SelectedValue = Request.Cookies["Currency"].Value;
            }
        }

        protected void DdlCurSelectedValueChanged(object sender, EventArgs e)
        {
            Response.Cookies.Add(new HttpCookie("Currency", ((DropDownList)sender).SelectedValue));
            Response.Redirect(Request.Url.ToString());
        }

        protected int GetCount()
        {
            int cartCount = ShoppingCartManager.GetCurrentShoppingCart(ShoppingCartTypeEnum.ShoppingCart).Count;
            int indOrders = IndividualOrderManager.GetCurrentUserIndividualOrders().Count;
            return cartCount + indOrders;
        }
    }
}