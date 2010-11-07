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
using System.Web.Caching;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.Web
{
    public partial class AddressEditPage : BaseNopPage
    {
        private void BindData()
        {
            AddressEditControl.IsBillingAddress = this.IsBillingAddress;
            if (this.AddressID > 0)
            {
                Address address = CustomerManager.GetAddressByID(this.AddressID);
                if (address != null)
                {
                    lHeaderTitle.Text = GetLocaleResourceString("Address.UpdateAddressTitle");
                    btnSave.Text = GetLocaleResourceString("Address.UpdateAddress");
                    btnDelete.Visible = true;
                    AddressEditControl.IsNew = false;
                    AddressEditControl.IsBillingAddress = address.IsBillingAddress;
                    AddressEditControl.Address = address;
                }
                else
                    Response.Redirect("~/Default.aspx");
            }
            else
            {
                lHeaderTitle.Text = GetLocaleResourceString("Address.NewAddressTitle");
                btnSave.Text = GetLocaleResourceString("Address.AddAddress");
                btnDelete.Visible = false;
                AddressEditControl.IsNew = true;
                AddressEditControl.IsBillingAddress = this.IsBillingAddress;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CommonHelper.EnsureSSL();
            }
            
            string title = GetLocaleResourceString("PageTitle.AddressEdit");
            SEOHelper.RenderTitle(this, title, true);

            Response.CacheControl = "private";
            Response.Expires = 0;
            Response.AddHeader("pragma", "no-cache");

            if (NopContext.Current.User == null)
            {
                string loginURL = CommonHelper.GetLoginPageURL(true);
                Response.Redirect(loginURL);
            }
            Address address = CustomerManager.GetAddressByID(this.AddressID);
            if (address != null)
            {
                Customer addressCustomer = address.Customer;
                if (addressCustomer == null || addressCustomer.CustomerID != NopContext.Current.User.CustomerID)
                {
                    string loginURL = CommonHelper.GetLoginPageURL(true);
                    Response.Redirect(loginURL);
                }

                if (DeleteAddress)
                {
                    CustomerManager.DeleteAddress(address.AddressID);
                    Response.Redirect("~/Account.aspx");
                }
            }

            if (!Page.IsPostBack)
            {
                this.BindData();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Address oldAddress = CustomerManager.GetAddressByID(this.AddressID);
                Address inputedAddress = AddressEditControl.Address;
                if (oldAddress != null)
                {
                    CustomerManager.UpdateAddress(oldAddress.AddressID, oldAddress.CustomerID, oldAddress.IsBillingAddress,
                        inputedAddress.FirstName, inputedAddress.LastName,
                        inputedAddress.PhoneNumber, inputedAddress.Email, inputedAddress.FaxNumber,
                        inputedAddress.Company, inputedAddress.Address1, inputedAddress.Address2,
                        inputedAddress.City, inputedAddress.StateProvinceID, inputedAddress.ZipPostalCode,
                        inputedAddress.CountryID, oldAddress.CreatedOn, DateTime.Now);
                }
                else
                {
                    CustomerManager.InsertAddress(NopContext.Current.User.CustomerID, this.IsBillingAddress,
                        inputedAddress.FirstName, inputedAddress.LastName,
                        inputedAddress.PhoneNumber, inputedAddress.Email, inputedAddress.FaxNumber,
                        inputedAddress.Company, inputedAddress.Address1, inputedAddress.Address2,
                        inputedAddress.City, inputedAddress.StateProvinceID, inputedAddress.ZipPostalCode,
                        inputedAddress.CountryID, DateTime.Now, DateTime.Now);
                }
                Response.Redirect("~/Account.aspx");
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            CustomerManager.DeleteAddress(this.AddressID);
            Response.Redirect("~/Account.aspx");
        }
        
        public int AddressID
        {
            get
            {
                return CommonHelper.QueryStringInt("AddressID");
            }
        }

        public bool IsBillingAddress
        {
            get
            {
                return CommonHelper.QueryStringBool("IsBillingAddress");
            }
        }
        
        public bool DeleteAddress
        {
            get
            {
                return CommonHelper.QueryStringBool("Delete");
            }
        }
    }
}