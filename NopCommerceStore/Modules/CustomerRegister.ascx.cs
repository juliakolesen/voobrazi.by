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
using System.ComponentModel;
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
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Common.Xml;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class CustomerRegisterControl : BaseNopUserControl
    {
        private void ApplyLocalization()
        {
            RequiredFieldValidator EmailRequired = CreateUserWizardStep1.ContentTemplateContainer.FindControl("EmailRequired") as RequiredFieldValidator;
            if (EmailRequired != null)
            {
                if (CustomerManager.UsernamesEnabled)
                {
                    EmailRequired.ErrorMessage = GetLocaleResourceString("Account.E-MailRequired");
                    EmailRequired.ToolTip = GetLocaleResourceString("Account.E-MailRequired");
                }
                else
                {
                    //EmailRequired is not enabled
                }
            }

            RegularExpressionValidator revEmail = CreateUserWizardStep1.ContentTemplateContainer.FindControl("revEmail") as RegularExpressionValidator;
            if (revEmail != null)
            {
                if (CustomerManager.UsernamesEnabled)
                {
                    revEmail.ErrorMessage = GetLocaleResourceString("Account.InvalidEmail");
                    revEmail.ToolTip = GetLocaleResourceString("Account.InvalidEmail");
                }
                else
                {
                    //revEmail is not enabled
                }
            }


            Literal lUsernameOrEmail = CreateUserWizardStep1.ContentTemplateContainer.FindControl("lUsernameOrEmail") as Literal;
            if (lUsernameOrEmail != null)
            {
                if (CustomerManager.UsernamesEnabled)
                {
                    lUsernameOrEmail.Text = GetLocaleResourceString("Account.Username");
                }
                else
                {
                    lUsernameOrEmail.Text = GetLocaleResourceString("Account.E-Mail");
                }
            }

            RequiredFieldValidator UserNameOrEmailRequired = CreateUserWizardStep1.ContentTemplateContainer.FindControl("UserNameOrEmailRequired") as RequiredFieldValidator;
            if (UserNameOrEmailRequired != null)
            {
                if (CustomerManager.UsernamesEnabled)
                {
                    UserNameOrEmailRequired.ErrorMessage = GetLocaleResourceString("Account.UserNameRequired");
                    UserNameOrEmailRequired.ToolTip = GetLocaleResourceString("Account.UserNameRequired");
                }
                else
                {
                    UserNameOrEmailRequired.ErrorMessage = GetLocaleResourceString("Account.E-MailRequired");
                    UserNameOrEmailRequired.ToolTip = GetLocaleResourceString("Account.E-MailRequired");
                }
            }

            RegularExpressionValidator refUserNameOrEmail = CreateUserWizardStep1.ContentTemplateContainer.FindControl("refUserNameOrEmail") as RegularExpressionValidator;
            if (refUserNameOrEmail != null)
            {
                if (CustomerManager.UsernamesEnabled)
                {
                    //refUserNameOrEmail is not enabled
                }
                else
                {
                    refUserNameOrEmail.ErrorMessage = GetLocaleResourceString("Account.InvalidEmail");
                    refUserNameOrEmail.ToolTip = GetLocaleResourceString("Account.InvalidEmail");
                }
            }

            Label lblCompleteStep = CompleteWizardStep1.ContentTemplateContainer.FindControl("lblCompleteStep") as Label;
            if (lblCompleteStep != null)
            {
                if (CustomerManager.RegistrationEmailValidation)
                {
                    lblCompleteStep.Text = GetLocaleResourceString("Account.ActivationEmailHasBeenSent");
                }
                else
                {
                    lblCompleteStep.Text = GetLocaleResourceString("Account.RegistrationCompleted");
                }
            }
        }

        public void CreatedUser(object sender, EventArgs e)
        {
            RadioButton rbGenderM = (RadioButton)CreateUserWizardStep1.ContentTemplateContainer.FindControl("rbGenderM");
            RadioButton rbGenderF = (RadioButton)CreateUserWizardStep1.ContentTemplateContainer.FindControl("rbGenderF");
            TextBox txtFirstName = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("txtFirstName");
            TextBox txtLastName = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("txtLastName");
            TextBox txtDateOfBirth = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("txtDateOfBirth");
            TextBox UserName = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("UserName");
            TextBox txtCompany = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("txtCompany");
            TextBox txtStreetAddress = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("txtStreetAddress");
            TextBox txtStreetAddress2 = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("txtStreetAddress2");
            TextBox txtZipPostalCode = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("txtZipPostalCode");
            TextBox txtCity = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("txtCity");
            TextBox txtPhoneNumber = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("txtPhoneNumber");
            TextBox txtFaxNumber = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("txtFaxNumber");
            DropDownList ddlCountry = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddlCountry");
            DropDownList ddlStateProvince = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddlStateProvince");
            CheckBox cbNewsletter = (CheckBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("cbNewsletter");

            Customer customer = null;
            if (CustomerManager.UsernamesEnabled)
            {
                customer = CustomerManager.GetCustomerByUsername(UserName.Text.Trim());
            }
            else
            {
                customer = CustomerManager.GetCustomerByEmail(UserName.Text.Trim());
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
            customer.PhoneNumber = txtPhoneNumber.Text;
            customer.FaxNumber = txtFaxNumber.Text;
            customer.CountryID = int.Parse(ddlCountry.SelectedItem.Value);
            customer.StateProvinceID = int.Parse(ddlStateProvince.SelectedItem.Value);
            customer.ReceiveNewsletter = cbNewsletter.Checked;

            Address billingAddress = new Address();
            billingAddress.CustomerID = customer.CustomerID;
            billingAddress.IsBillingAddress = true;
            billingAddress.FirstName = customer.FirstName;
            billingAddress.LastName = customer.LastName;
            billingAddress.PhoneNumber = customer.PhoneNumber;
            billingAddress.Email = customer.Email;
            billingAddress.FaxNumber = customer.FaxNumber;
            billingAddress.Company = customer.Company;
            billingAddress.Address1 = customer.StreetAddress;
            billingAddress.Address2 = customer.StreetAddress2;
            billingAddress.City = customer.City;
            billingAddress.StateProvinceID = customer.StateProvinceID;
            billingAddress.ZipPostalCode = customer.ZipPostalCode;
            billingAddress.CountryID = customer.CountryID;
            billingAddress.CreatedOn = customer.RegistrationDate;
            if (CustomerManager.CanUseAddressAsBillingAddress(billingAddress))
            {
                billingAddress = CustomerManager.InsertAddress(billingAddress.CustomerID, billingAddress.IsBillingAddress,
                    billingAddress.FirstName, billingAddress.LastName, billingAddress.PhoneNumber,
                    billingAddress.Email, billingAddress.FaxNumber, billingAddress.Company,
                    billingAddress.Address1, billingAddress.Address2,
                    billingAddress.City, billingAddress.StateProvinceID,
                    billingAddress.ZipPostalCode, billingAddress.CountryID, DateTime.Now, DateTime.Now);
            }
            Address shippingAddress = new Address();
            shippingAddress.CustomerID = customer.CustomerID;
            shippingAddress.IsBillingAddress = false;
            shippingAddress.FirstName = customer.FirstName;
            shippingAddress.LastName = customer.LastName;
            shippingAddress.PhoneNumber = customer.PhoneNumber;
            shippingAddress.Email = customer.Email;
            shippingAddress.FaxNumber = customer.FaxNumber;
            shippingAddress.Company = customer.Company;
            shippingAddress.Address1 = customer.StreetAddress;
            shippingAddress.Address2 = customer.StreetAddress2;
            shippingAddress.City = customer.City;
            shippingAddress.StateProvinceID = customer.StateProvinceID;
            shippingAddress.ZipPostalCode = customer.ZipPostalCode;
            shippingAddress.CountryID = customer.CountryID;
            shippingAddress.CreatedOn = customer.RegistrationDate;
            if (CustomerManager.CanUseAddressAsShippingAddress(shippingAddress))
            {
                shippingAddress = CustomerManager.InsertAddress(shippingAddress.CustomerID, shippingAddress.IsBillingAddress,
                    shippingAddress.FirstName, shippingAddress.LastName, shippingAddress.PhoneNumber,
                    shippingAddress.Email, shippingAddress.FaxNumber, shippingAddress.Company,
                    shippingAddress.Address1, shippingAddress.Address2,
                    shippingAddress.City, shippingAddress.StateProvinceID,
                    shippingAddress.ZipPostalCode, shippingAddress.CountryID, DateTime.Now, DateTime.Now);
            }
        }

        public void CreatingUser(object sender, LoginCancelEventArgs e)
        {
            if (SettingManager.GetSettingValueBoolean("Common.RegisterCaptchaImageEnabled"))
            {
                CaptchaControl CaptchaCtrl = CreateUserWizardStep1.ContentTemplateContainer.FindControl("CaptchaCtrl") as CaptchaControl;
                if (CaptchaCtrl != null)
                {
                    if (!CaptchaCtrl.ValidateCaptcha())
                    {
                        e.Cancel = true;
                    }
                }
            }
        }

        protected void CreateUserError(object sender, EventArgs e)
        {
            if (SettingManager.GetSettingValueBoolean("Common.RegisterCaptchaImageEnabled"))
            {
                CaptchaControl CaptchaCtrl = CreateUserWizardStep1.ContentTemplateContainer.FindControl("CaptchaCtrl") as CaptchaControl;
                if (CaptchaCtrl != null)
                {
                    CaptchaCtrl.RegenerateCode();
                }
            }
        }
        
        private void FillCountryDropDowns()
        {
            DropDownList ddlCountry = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddlCountry");
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
            DropDownList ddlCountry = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddlCountry");
            DropDownList ddlStateProvince = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddlStateProvince");
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
                ListItem ddlStateProvinceItem = new ListItem(GetLocaleResourceString("Address.StateProvinceNonUS"), "0");
                ddlStateProvince.Items.Add(ddlStateProvinceItem);
            }
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillStateProvinceDropDowns();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.ApplyLocalization();

            CreateUserForm.Visible = !CustomerManager.NewCustomerRegistrationDisabled;
            topicRegistrationNotAllowed.Visible = CustomerManager.NewCustomerRegistrationDisabled;

            if (!Page.IsPostBack)
            {
                if (NopContext.Current.User != null && !NopContext.Current.User.IsGuest)
                {
                    CustomerManager.Logout();
                    Response.Redirect("~/Register.aspx");
                }

                #region Username/emails hack
                HtmlTableRow pnlEmail = CreateUserWizardStep1.ContentTemplateContainer.FindControl("pnlEmail") as HtmlTableRow;
                if (pnlEmail != null)
                {
                    pnlEmail.Visible = CustomerManager.UsernamesEnabled;
                }
                RegularExpressionValidator refUserNameOrEmail = CreateUserWizardStep1.ContentTemplateContainer.FindControl("refUserNameOrEmail") as RegularExpressionValidator;
                if (refUserNameOrEmail != null)
                {
                    refUserNameOrEmail.Enabled = !CustomerManager.UsernamesEnabled;
                }
                #endregion

                this.FillCountryDropDowns();
                this.FillStateProvinceDropDowns();
                this.DataBind();
            }

            CaptchaControl CaptchaCtrl = CreateUserWizardStep1.ContentTemplateContainer.FindControl("CaptchaCtrl") as CaptchaControl;
            if (CaptchaCtrl != null)
            {
                CaptchaCtrl.Visible = SettingManager.GetSettingValueBoolean("Common.RegisterCaptchaImageEnabled");
            }
        }
    }
}