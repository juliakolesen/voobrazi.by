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
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Audit;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Localization;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Shipping;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.Common.Utils;


namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class CheckoutShippingMethodControl : BaseNopUserControl
    {
        private ShoppingCart Cart = null;

        protected string FormatShippingOption(ShippingOption shippingOption)
        {
            decimal rateBase = TaxManager.GetShippingPrice(shippingOption.Rate, NopContext.Current.User);
            decimal rate = CurrencyManager.ConvertCurrency(rateBase, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingCurrency);
            string rateStr = PriceHelper.FormatShippingPrice(rate, true);
            return string.Format("({0})", rateStr);
        }

        public ShippingOption SelectedShippingOption
        {
            get
            {
                ShippingOption shippingOption = null;
                foreach (DataListItem item in this.dlShippingOptions.Items)
                {
                    RadioButton rdShippingOption = (RadioButton)item.FindControl("rdShippingOption");
                    if (rdShippingOption.Checked)
                    {
                        string name = this.dlShippingOptions.DataKeys[item.ItemIndex].ToString();

                        string error = string.Empty;
                        ShippingOptionCollection shippingOptions = ShippingManager.GetShippingOptions(Cart, NopContext.Current.User, NopContext.Current.User.ShippingAddress, ref error);
                        shippingOption = shippingOptions.Find((so) => so.Name == name);
                        break;
                    }
                }
                return shippingOption;
            }
        }

        protected void btnNextStep_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                ShippingOption shippingOption = this.SelectedShippingOption;
                if (shippingOption != null)
                {
                    NopContext.Current.User.LastShippingOption = shippingOption;
                    Response.Redirect("~/CheckoutPaymentMethod.aspx");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((NopContext.Current.User == null) || (NopContext.Current.User.IsGuest && !CustomerManager.AnonymousCheckoutAllowed))
            {
                string loginURL = CommonHelper.GetLoginPageURL(true);
                Response.Redirect(loginURL);
            }

            Cart = ShoppingCartManager.GetCurrentShoppingCart(ShoppingCartTypeEnum.ShoppingCart);
            if (Cart.Count == 0)
                Response.Redirect("~/ShoppingCart.aspx");

            bool shoppingCartRequiresShipping = ShippingManager.ShoppingCartRequiresShipping(Cart);
            if (!shoppingCartRequiresShipping)
            {
                NopContext.Current.User.LastShippingOption = null;
                Response.Redirect("~/CheckoutPaymentMethod.aspx");
            }

            if (!Page.IsPostBack)
            {
                if (shoppingCartRequiresShipping)
                {
                    //ShipmentPackage shipmentPackage = ShippingManager.CreateShipmentPackage(Cart, NopContext.Current.User.ShippingAddress);
                    string error = string.Empty;
                    ShippingOptionCollection shippingOptions = ShippingManager.GetShippingOptions(Cart, NopContext.Current.User, NopContext.Current.User.ShippingAddress, ref error);
                    if (!String.IsNullOrEmpty(error))
                    {
                        LogManager.InsertLog(LogTypeEnum.ShippingErrror, error, error);
                        lError.Text = Server.HtmlEncode(error);
                    }
                    else
                    {
                        if (shippingOptions.Count > 0)
                        {
                            dlShippingOptions.DataSource = shippingOptions;
                            dlShippingOptions.DataBind();
                        }
                        else
                        {
                            phSelectShippingMethod.Visible = false;
                            phShippingIsNotAllowed.Visible = true;
                        }
                    }
                }
            }
        }
    }
}