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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.BusinessLogic.Promo.Affiliates;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Common;

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class CustomerInfoControl : BaseNopAdministrationUserControl
    {
        private void BindData()
        {
            pnlTimeZone.Visible = DateTimeHelper.AllowCustomersToSetTimeZone;
            pnlUsername.Visible = CustomerManager.UsernamesEnabled;

            Customer customer = CustomerManager.GetCustomerByID(this.CustomerID);
            if (customer != null)
            {
                this.txtEmail.Text = customer.Email;

                if (CustomerManager.UsernamesEnabled)
                {
                    txtUsername.Visible = false;
                    lblUsername.Visible = true;
                    txtUsername.Text = customer.Username;
                    lblUsername.Text = customer.Username;
                }
                pnlPassword.Visible = false;
                
                if (customer.Gender.ToLower() == "m")
                    rbGenderM.Checked = true;
                else
                    rbGenderF.Checked = true;

                txtFirstName.Text = customer.FirstName;
                txtLastName.Text = customer.LastName;

                if (customer.DateOfBirth.HasValue)
                    txtDateOfBirth.Text = customer.DateOfBirth.Value.ToString("d");

                txtCompany.Text = customer.Company;
                txtStreetAddress.Text = customer.StreetAddress;
                txtStreetAddress2.Text = customer.StreetAddress2;
                txtZipPostalCode.Text = customer.ZipPostalCode;
                txtCity.Text = customer.City;
                CommonHelper.SelectListItem(ddlCountry, customer.CountryID.ToString());
                FillStateProvinceDropDowns();
                CommonHelper.SelectListItem(ddlStateProvince, customer.StateProvinceID.ToString());
                txtPhoneNumber.Text = customer.PhoneNumber;
                txtFaxNumber.Text = customer.FaxNumber;
                cbNewsletter.Checked = customer.ReceiveNewsletter;

                if (DateTimeHelper.AllowCustomersToSetTimeZone)
                {
                    CommonHelper.SelectListItem(this.ddlTimeZone, customer.TimeZoneID);
                }

                CommonHelper.SelectListItem(this.ddlAffiliate, customer.AffiliateID);
                this.cbIsTaxExempt.Checked = customer.IsTaxExempt;
                this.cbIsAdmin.Checked = customer.IsAdmin;
                this.cbIsForumModerator.Checked = customer.IsForumModerator;
                this.txtAdminComment.Text = customer.AdminComment;
                this.cbActive.Checked = customer.Active;
                this.pnlRegistrationDate.Visible = true;
                this.lblRegistrationDate.Text = DateTimeHelper.ConvertToUserTime(customer.RegistrationDate).ToString();
            }
            else
            {
                if (CustomerManager.UsernamesEnabled)
                {
                    txtUsername.Visible = true;
                    lblUsername.Visible = false;
                }
                this.pnlPassword.Visible = true;
                if (DateTimeHelper.AllowCustomersToSetTimeZone)
                {
                    CommonHelper.SelectListItem(this.ddlTimeZone, DateTimeHelper.DefaultStoreTimeZone.Id);
                }
                this.pnlRegistrationDate.Visible = false;
            }
        }

        private void FillAffiliatDropDowns()
        {
            this.ddlAffiliate.Items.Clear();
            ListItem ddlAffiliateItem = new ListItem(GetLocaleResourceString("Admin.CustomerInfo.Affiliate.None"), "0");
            this.ddlAffiliate.Items.Add(ddlAffiliateItem);
            AffiliateCollection affiliateCollection = AffiliateManager.GetAllAffiliates();
            foreach (Affiliate affiliate in affiliateCollection)
            {
                ListItem ddlAffiliateItem2 = new ListItem(affiliate.LastName + " (ID=" + affiliate.AffiliateID.ToString() + ")", affiliate.AffiliateID.ToString());
                this.ddlAffiliate.Items.Add(ddlAffiliateItem2);
            }
        }

        private void FillCountryDropDowns()
        {
            ddlCountry.Items.Clear();
            CountryCollection countryCollection = CountryManager.GetAllCountriesForRegistration();
            foreach (Country country in countryCollection)
            {
                ListItem ddlCountryItem2 = new ListItem(country.Name, country.CountryID.ToString());
                ddlCountry.Items.Add(ddlCountryItem2);
            }
        }

        private void FillStateProvinceDropDowns()
        {
            ddlStateProvince.Items.Clear();
            int countryID = 0;
            if (ddlCountry.SelectedItem != null)
                countryID = int.Parse(ddlCountry.SelectedItem.Value);

            StateProvinceCollection stateProvinceCollection = StateProvinceManager.GetStateProvincesByCountryID(countryID);
            foreach (StateProvince stateProvince in stateProvinceCollection)
            {
                ListItem ddlStateProviceItem2 = new ListItem(stateProvince.Name, stateProvince.StateProvinceID.ToString());
                ddlStateProvince.Items.Add(ddlStateProviceItem2);
            }
            if (stateProvinceCollection.Count == 0)
            {
                ListItem ddlStateProvinceItem = new ListItem(GetLocaleResourceString("Admin.Common.State.Other"), "0");
                ddlStateProvince.Items.Add(ddlStateProvinceItem);
            }
        }

        private void FillTimeZones()
        {
            this.ddlTimeZone.Items.Clear();
            if (DateTimeHelper.AllowCustomersToSetTimeZone)
            {
                ReadOnlyCollection<TimeZoneInfo> timeZones = DateTimeHelper.GetSystemTimeZones();
                foreach (TimeZoneInfo timeZone in timeZones)
                {
                    string timeZoneName = timeZone.DisplayName;
                    ListItem ddlTimeZoneItem2 = new ListItem(timeZoneName, timeZone.Id);
                    this.ddlTimeZone.Items.Add(ddlTimeZoneItem2);
                }
            }
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillStateProvinceDropDowns();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.FillAffiliatDropDowns();
                this.FillCountryDropDowns();
                this.FillStateProvinceDropDowns();
                this.FillTimeZones();
                this.BindData();
            }
        }

        public Customer SaveInfo()
        {
            Customer customer = CustomerManager.GetCustomerByID(this.CustomerID);

            string email = txtEmail.Text.Trim();
            int affiliateID = int.Parse(this.ddlAffiliate.SelectedItem.Value);
            bool isTaxExempt=cbIsTaxExempt.Checked;
            bool isAdmin=cbIsAdmin.Checked;
            bool isForumModerator=cbIsForumModerator.Checked;
            bool active= cbActive.Checked;
            string adminComment = txtAdminComment.Text.Trim();

            if (customer != null)
            {
                customer = CustomerManager.SetEmail(customer.CustomerID, email);

                customer = CustomerManager.UpdateCustomer(customer.CustomerID, customer.CustomerGUID,
                    customer.Email, customer.Username, customer.PasswordHash,
                    customer.SaltKey, affiliateID,
                    customer.BillingAddressID, customer.ShippingAddressID,
                    customer.LastPaymentMethodID, customer.LastAppliedCouponCode,
                    customer.LanguageID, customer.CurrencyID, customer.TaxDisplayType,
                    isTaxExempt, isAdmin, customer.IsGuest, isForumModerator,
                    customer.TotalForumPosts, customer.Signature, adminComment, active,
                    customer.Deleted, customer.RegistrationDate, customer.TimeZoneID, customer.AvatarID);
            }
            else
            {
                string username = txtUsername.Text;
                string password = txtPassword.Text;
                MembershipCreateStatus createStatus = MembershipCreateStatus.Success;
                customer = CustomerManager.AddCustomer(Guid.NewGuid(), email, username,
                    password, affiliateID,
                    0, 0, 0, string.Empty, NopContext.Current.WorkingLanguage.LanguageID,
                    NopContext.Current.WorkingCurrency.CurrencyID, NopContext.Current.TaxDisplayType,
                    isTaxExempt, isAdmin,
                    false, isForumModerator,
                    0, string.Empty, adminComment, active,
                    false, DateTime.Now, string.Empty, 0, out createStatus);
                if (createStatus != MembershipCreateStatus.Success)
                {
                    throw new NopException(string.Format("Could not create new customer: {0}", createStatus.ToString()));
                }
            }

            if (rbGenderM.Checked)
                customer.Gender = "M";
            else
                customer.Gender = "F";

            customer.FirstName = txtFirstName.Text;
            customer.LastName = txtLastName.Text;
            try
            {
                DateTime dateOfBirth = DateTime.Parse(txtDateOfBirth.Text);
                customer.DateOfBirth = dateOfBirth;
            }
            catch
            {
                customer.DateOfBirth = null;
            }

            customer.Company = txtCompany.Text;
            customer.StreetAddress = txtStreetAddress.Text;
            customer.StreetAddress2 = txtStreetAddress2.Text;
            customer.ZipPostalCode = txtZipPostalCode.Text;
            customer.City = txtCity.Text;
            customer.CountryID = int.Parse(ddlCountry.SelectedItem.Value);
            customer.StateProvinceID = int.Parse(ddlStateProvince.SelectedItem.Value);
            customer.PhoneNumber = txtPhoneNumber.Text;
            customer.FaxNumber = txtFaxNumber.Text;
            customer.ReceiveNewsletter = cbNewsletter.Checked;

            if (DateTimeHelper.AllowCustomersToSetTimeZone)
            {
                if (ddlTimeZone.SelectedItem != null && !String.IsNullOrEmpty(ddlTimeZone.SelectedItem.Value))
                {
                    string timeZoneID = ddlTimeZone.SelectedItem.Value;
                    TimeZoneInfo timeZone = DateTimeHelper.FindTimeZoneById(timeZoneID);
                    if (timeZone != null)
                        customer = CustomerManager.SetTimeZoneID(customer.CustomerID, timeZone.Id);
                }
            }

            return customer;
        }

        public int CustomerID
        {
            get
            {
                return CommonHelper.QueryStringInt("CustomerID");
            }
        }
    }
}