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
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.BusinessLogic.Promo.Affiliates;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class CustomerRoleMappingsControl : BaseNopAdministrationUserControl
    {
        private void BindData()
        {
            Customer customer = CustomerManager.GetCustomerByID(this.CustomerID);
            if (customer != null)
            {
                CustomerRoleCollection customerRoles = customer.CustomerRoles;
                List<int> _customerRoleIDs = new List<int>();
                foreach (CustomerRole customerRole in customerRoles)
                    _customerRoleIDs.Add(customerRole.CustomerRoleID);
                CustomerRoleMappingControl.SelectedCustomerRoleIDs = _customerRoleIDs;
                CustomerRoleMappingControl.BindData();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {           
                this.BindData();
            }
        }

        public void SaveInfo()
        {
            Customer customer = CustomerManager.GetCustomerByID(this.CustomerID);

            if (customer != null)
            {
                foreach (CustomerRole customerRole in CustomerManager.GetCustomerRolesByCustomerID(customer.CustomerID))
                    CustomerManager.RemoveCustomerFromRole(customer.CustomerID, customerRole.CustomerRoleID);
                foreach (int customerRoleID in CustomerRoleMappingControl.SelectedCustomerRoleIDs)
                    CustomerManager.AddCustomerToRole(customer.CustomerID, customerRoleID);
            }
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