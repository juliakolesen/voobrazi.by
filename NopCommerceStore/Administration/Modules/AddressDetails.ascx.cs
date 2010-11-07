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
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class AddressDetailsControl : BaseNopAdministrationUserControl
    {
        protected void BindData()
        {
            Address address = CustomerManager.GetAddressByID(this.AddressID);
            if (address != null)
            {
                Customer customer = address.Customer;
                if (customer != null)
                {
                    this.lblCustomer.Text = Server.HtmlEncode(customer.Email);
                    lnkBack.NavigateUrl = CommonHelper.GetStoreAdminLocation() + "CustomerDetails.aspx?CustomerID=" + customer.CustomerID.ToString();
                }
                else
                    Response.Redirect("Customers.aspx");

                this.FillCountryDropDowns(address);

                this.txtFirstName.Text = address.FirstName;
                this.txtLastName.Text = address.LastName;
                this.txtPhoneNumber.Text = address.PhoneNumber;
                this.txtEmail.Text = address.Email;
                this.txtFaxNumber.Text = address.FaxNumber;
                this.txtCompany.Text = address.Company;
                this.txtAddress1.Text = address.Address1;
                this.txtAddress2.Text = address.Address2;
                this.txtCity.Text = address.City;
                CommonHelper.SelectListItem(this.ddlCountry, address.CountryID);
                FillStateProvinceDropDowns();
                CommonHelper.SelectListItem(this.ddlStateProvince, address.StateProvinceID);
                this.txtZipPostalCode.Text = address.ZipPostalCode;
            }
            else
                Response.Redirect("Customers.aspx");
        }

        protected void FillCountryDropDowns(Address address)
        {
            this.ddlCountry.Items.Clear();
            CountryCollection countryCollection = null;
            if (address.IsBillingAddress)
                countryCollection = CountryManager.GetAllCountriesForBilling();
            else
                countryCollection = CountryManager.GetAllCountriesForShipping();
            foreach (Country country in countryCollection)
            {
                ListItem ddlCountryItem2 = new ListItem(country.Name, country.CountryID.ToString());
                this.ddlCountry.Items.Add(ddlCountryItem2);
            }
        }

        protected void FillStateProvinceDropDowns()
        {
            this.ddlStateProvince.Items.Clear();
            int countryID = int.Parse(this.ddlCountry.SelectedItem.Value);

            StateProvinceCollection stateProvinceCollection = StateProvinceManager.GetStateProvincesByCountryID(countryID);
            foreach (StateProvince stateProvince in stateProvinceCollection)
            {
                ListItem ddlStateProviceItem2 = new ListItem(stateProvince.Name, stateProvince.StateProvinceID.ToString());
                this.ddlStateProvince.Items.Add(ddlStateProviceItem2);
            }
            if (stateProvinceCollection.Count == 0)
            {
                ListItem ddlStateProvinceItem = new ListItem(GetLocaleResourceString("Admin.Common.State.Other"), "0");
                this.ddlStateProvince.Items.Add(ddlStateProvinceItem);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.BindData();
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    Address address = CustomerManager.GetAddressByID(this.AddressID);

                    if (address != null)
                    {
                        address = CustomerManager.UpdateAddress(address.AddressID, address.CustomerID, address.IsBillingAddress,
                            txtFirstName.Text, txtLastName.Text,
                            txtPhoneNumber.Text, txtEmail.Text, txtFaxNumber.Text, txtCompany.Text,
                            txtAddress1.Text, txtAddress2.Text, txtCity.Text, 
                            int.Parse(this.ddlStateProvince.SelectedItem.Value), txtZipPostalCode.Text,
                            int.Parse(this.ddlCountry.SelectedItem.Value), address.CreatedOn, DateTime.Now);

                        Response.Redirect("CustomerDetails.aspx?CustomerID=" + address.CustomerID.ToString());
                    }
                    else
                        Response.Redirect("Customers.aspx");
                }
                catch (Exception exc)
                {
                    ProcessException(exc);
                }
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                Address address = CustomerManager.GetAddressByID(this.AddressID);
                CustomerManager.DeleteAddress(this.AddressID);
                if (address != null)
                    Response.Redirect("CustomerDetails.aspx?CustomerID=" + address.CustomerID.ToString());
                else
                    Response.Redirect("Customers.aspx");
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillStateProvinceDropDowns();
        }

        public int AddressID
        {
            get
            {
                return CommonHelper.QueryStringInt("AddressID");
            }
        }
    }
}