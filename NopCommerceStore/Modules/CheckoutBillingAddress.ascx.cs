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
using System.Web.UI;
using System.Web.UI.WebControls;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Audit;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class CheckoutBillingAddressControl : BaseNopUserControl
    {
        ShoppingCart Cart = null;

        private Address BillingAddress
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

        private void SelectAddress(Address billingAddress)
        {
            if (billingAddress == null)
            {
                NopContext.Current.User = CustomerManager.SetDefaultBillingAddress(NopContext.Current.User.CustomerID, 0);
                return;
            }

            if (billingAddress.AddressID == 0)
            {
                //check if address already exists
                Address billingAddress2 = NopContext.Current.User.BillingAddresses.FindAddress(billingAddress.FirstName,
                     billingAddress.LastName, billingAddress.PhoneNumber, billingAddress.Email,
                     billingAddress.FaxNumber, billingAddress.Company,
                     billingAddress.Address1, billingAddress.Address2,
                     billingAddress.City, billingAddress.StateProvinceID, billingAddress.ZipPostalCode,
                     billingAddress.CountryID);

                if (billingAddress2 != null)
                {
                    billingAddress = billingAddress2;
                }
                else
                {
                    billingAddress = CustomerManager.InsertAddress(NopContext.Current.User.CustomerID, true, billingAddress.FirstName,
                        billingAddress.LastName, billingAddress.PhoneNumber, billingAddress.Email,
                        billingAddress.FaxNumber, billingAddress.Company, billingAddress.Address1, billingAddress.Address2,
                        billingAddress.City, billingAddress.StateProvinceID, billingAddress.ZipPostalCode,
                        billingAddress.CountryID, DateTime.Now, DateTime.Now);
                }
            }

            Session.Add("Delivery", rbCourier.Checked);

            if (!string.IsNullOrEmpty(txtFirstName.Text.Trim()) || !string.IsNullOrEmpty(txtLastName.Text.Trim()) && !string.IsNullOrEmpty(txtAddress1.Text))
            {
                Session.Add("fn", txtFirstName.Text.Trim());
                Session.Add("ln", txtLastName.Text.Trim());
                Session.Add("pn", txtPhoneNumber.Text.Trim());
                Session.Add("address", txtAddress1.Text.Trim());
                Session.Add("ai", tbAdditionalInfo.Text.Trim());
            }

            NopContext.Current.User = CustomerManager.SetDefaultBillingAddress(NopContext.Current.User.CustomerID, billingAddress.AddressID);

            string error = string.Empty;
            //decimal? taxTotal = TaxManager.CalculateTaxTotal(Cart, billingAddress, NopContext.Current.User, ref error);
            //NopContext.Current.User.LastCalculatedTax = taxTotal;
            if (!String.IsNullOrEmpty(error))
            {
                LogManager.InsertLog(LogTypeEnum.TaxError, error, error);
                lError.Text = Server.HtmlEncode(error);
            }
            else
            {
                Response.Redirect("~/CheckoutPaymentMethod.aspx");
            }
        }

        protected AddressCollection getAllowedBillingAddresses(Customer customer)
        {
            AddressCollection addresses = new AddressCollection();
            if (customer == null)
                return addresses;

            foreach (Address address in customer.BillingAddresses)
            {
                Country country = address.Country;
                if (country != null && country.AllowsBilling)
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
                Address billingAddress = CustomerManager.GetAddressByID(addressID);
                if (billingAddress != null && NopContext.Current.User != null)
                {
                    Address prevAddress = CustomerManager.GetAddressByID(billingAddress.AddressID);
                    if (prevAddress.CustomerID != NopContext.Current.User.CustomerID)
                        return;
                }

                SelectAddress(billingAddress);
            }
        }

        protected void btnNextStep_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Address billingAddress = this.BillingAddress;
                SelectAddress(billingAddress);
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
            IndividualOrderCollection indOrders = IndividualOrderManager.GetCurrentUserIndividualOrders();
            if (Cart.Count == 0 && indOrders.Count == 0)
                Response.Redirect("~/ShoppingCart.aspx");

            if (!Page.IsPostBack)
            {
                Address shippingAddress = NopContext.Current.User.ShippingAddress;
                pnlTheSameAsShippingAddress.Visible = CustomerManager.CanUseAddressAsBillingAddress(shippingAddress);

                AddressCollection addresses = getAllowedBillingAddresses(NopContext.Current.User);

                if (addresses.Count > 0)
                {
                    //bind data
                    //if (addresses.Count > 1)
                    //    addresses.RemoveRange(1, addresses.Count - 1);
                    dlAddress.DataSource = addresses;
                    dlAddress.DataBind();
                    lEnterBillingAddress.Text = GetLocaleResourceString("Checkout.OrEnterNewAddress");
                }
                else
                {
                    pnlSelectBillingAddress.Visible = false;
                    lEnterBillingAddress.Text = GetLocaleResourceString("Checkout.EnterBillingAddress");
                }
            }
        }

        protected void btnTheSameAsShippingAddress_Click(object sender, EventArgs e)
        {
            Address shippingAddress = NopContext.Current.User.ShippingAddress;
            if (shippingAddress != null && CustomerManager.CanUseAddressAsBillingAddress(shippingAddress))
            {
                Address billingAddress = new Address();
                billingAddress.AddressID = 0;
                billingAddress.CustomerID = shippingAddress.CustomerID;
                billingAddress.IsBillingAddress = true;
                billingAddress.FirstName = shippingAddress.FirstName;
                billingAddress.LastName = shippingAddress.LastName;
                billingAddress.PhoneNumber = shippingAddress.PhoneNumber;
                billingAddress.Email = shippingAddress.Email;
                billingAddress.FaxNumber = shippingAddress.FaxNumber;
                billingAddress.Company = shippingAddress.Company;
                billingAddress.Address1 = shippingAddress.Address1;
                billingAddress.Address2 = shippingAddress.Address2;
                billingAddress.City = shippingAddress.City;
                billingAddress.StateProvinceID = shippingAddress.StateProvinceID;
                billingAddress.ZipPostalCode = shippingAddress.ZipPostalCode;
                billingAddress.CountryID = shippingAddress.CountryID;
                billingAddress.CreatedOn = shippingAddress.CreatedOn;

                AddressDisplayCtrl.Address = billingAddress;
            }
            else
            {
                pnlTheSameAsShippingAddress.Visible = false;
            }
        }

        protected void rbSelf_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSelf.Checked)
                Session.Add("SelfOrder", true);
            OrderSummaryControl.BindData();
        }
        protected void rbCourier_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCourier.Checked)
                Session.Remove("SelfOrder");
            OrderSummaryControl.BindData();
        }
    }
}