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
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Shipping;
using NopSolutions.NopCommerce.Common.Utils;
 

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class CheckoutShippingAddressControl : BaseNopUserControl
    {
        ShoppingCart Cart = null;

        private Address ShippingAddress
        {
            get
            {
                Address address = AddressDisplayCtrl.Address;
                if (address.AddressID != 0 && NopContext.Current.User != null)
                {
                    Address prevAddress = CustomerManager.GetAddressByID(address.AddressID);
                    if (prevAddress.CustomerID != NopContext.Current.User.CustomerID)
                        return null;
                    address.CustomerID = prevAddress.CustomerID;
                    address.CreatedOn = prevAddress.CreatedOn;
                }
                else
                {
                    address.CreatedOn = DateTime.Now;
                }

                return address;
            }
        }

        private void SelectAddress(Address shippingAddress)
        {
            if (shippingAddress == null)
            {
                NopContext.Current.User =CustomerManager.SetDefaultShippingAddress(NopContext.Current.User.CustomerID, 0);
                return;
            }

            if (shippingAddress.AddressID == 0)
            { 
                //check if address already exists
                Address shippingAddress2 = NopContext.Current.User.ShippingAddresses.FindAddress(shippingAddress.FirstName,
                    shippingAddress.LastName, shippingAddress.PhoneNumber, shippingAddress.Email,
                    shippingAddress.FaxNumber, shippingAddress.Company, 
                    shippingAddress.Address1, shippingAddress.Address2,
                    shippingAddress.City, shippingAddress.StateProvinceID, shippingAddress.ZipPostalCode,
                    shippingAddress.CountryID);

                if (shippingAddress2 != null)
                {
                    shippingAddress = shippingAddress2;
                }
                else
                {
                    shippingAddress = CustomerManager.InsertAddress(NopContext.Current.User.CustomerID, false, shippingAddress.FirstName,
                              shippingAddress.LastName, shippingAddress.PhoneNumber, shippingAddress.Email,
                              shippingAddress.FaxNumber, shippingAddress.Company, shippingAddress.Address1, shippingAddress.Address2,
                              shippingAddress.City, shippingAddress.StateProvinceID, shippingAddress.ZipPostalCode,
                              shippingAddress.CountryID, DateTime.Now, DateTime.Now);
                }
            }

            NopContext.Current.User = CustomerManager.SetDefaultShippingAddress(NopContext.Current.User.CustomerID, shippingAddress.AddressID);

            Response.Redirect("~/CheckoutBillingAddress.aspx");
        }
        
        protected AddressCollection getAllowedShippingAddresses(Customer customer)
        {
            AddressCollection addresses = new AddressCollection();
            if (customer == null)
                return addresses;

            foreach (Address address in customer.ShippingAddresses)
            {
                Country country = address.Country;
                if (country != null && country.AllowsShipping)
                {
                    addresses.Add(address);
                }
            }

            return addresses;
        }

        protected void btnSelect_Command(object sender, CommandEventArgs e)
        {
            if (Page.IsValid)
            {
                int addressID = int.Parse(e.CommandArgument.ToString());
                Address shippingAddress = CustomerManager.GetAddressByID(addressID);
                if (shippingAddress != null && NopContext.Current.User != null)
                {
                    Address prevAddress = CustomerManager.GetAddressByID(shippingAddress.AddressID);
                    if (prevAddress.CustomerID != NopContext.Current.User.CustomerID)
                        return;
                }

                SelectAddress(shippingAddress);
            }
        }

        protected void btnNextStep_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Address shippingAddress = this.ShippingAddress;
                SelectAddress(shippingAddress);
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

            if (!Page.IsPostBack)
            {
                CustomerManager.ResetCheckoutData(NopContext.Current.User.CustomerID, false);
            }
            bool shoppingCartRequiresShipping = ShippingManager.ShoppingCartRequiresShipping(Cart);
            if (!shoppingCartRequiresShipping)
            {
                SelectAddress(null);
                Response.Redirect("~/CheckoutBillingAddress.aspx");
            }

            if (!Page.IsPostBack)
            {
                AddressCollection addresses = getAllowedShippingAddresses(NopContext.Current.User);
                if (addresses.Count > 0)
                {
                    dlAddress.DataSource = addresses;
                    dlAddress.DataBind();
                    lEnterShippingAddress.Text = GetLocaleResourceString("Checkout.OrEnterNewAddress");
                }
                else
                {
                    pnlSelectShippingAddress.Visible = false;
                    lEnterShippingAddress.Text = GetLocaleResourceString("Checkout.EnterShippingAddress");
                }
            }
        }
    }
}