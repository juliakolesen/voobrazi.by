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
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Payment.Methods.PayPal;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.Common;
namespace NopSolutions.NopCommerce.Web
{
    public partial class WorldpayReturnPage : BaseNopPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region Dummy test card numbers
//            card type
	

//card number
	

//number length
	

//issue no length

//Mastercard
	

//5100080000000000
	

//16
	

//0

//Visa Delta - UK
	

//4406080400000000
	

//16
	

//0

//Visa Delta - Non UK
	

//4462030000000000
	

//16
	

//0

//Visa
	

//4911830000000
	

//13
	

//0

//Visa
	

//4917610000000000
	

//16
	

//0

//American Express
	

//370000200000000
	

//15
	

//0

//Diners
	

//36700102000000
	

//14
	

//0

//JCB
	

//3528000700000000
	

//16
	

//0

//Visa Electron (UK only)
	

//4917300800000000
	

//16
	

//0

//Solo
	

//6334580500000000
	

//16
	

//0

//Solo
	

//633473060000000000
	

//18
	

//1

//Discover Card
	

//6011000400000000
	

//16
	

//0

//Laser
	

//630495060000000000
	

//18
	

//0

//Maestro (UK only)
	

//6759649826438453
	

//16
	

//0

//Visa Purchasing
	

//4484070000000000
	

//16
	

//0
            #endregion

            //TODO implement, validate order
            Response.Redirect("~/Default.aspx");

            Response.CacheControl = "private";
            Response.Expires = 0;
            Response.AddHeader("pragma", "no-cache");
            if ((NopContext.Current.User == null) || (NopContext.Current.User.IsGuest && !CustomerManager.AnonymousCheckoutAllowed))
            {
                string loginURL = CommonHelper.GetLoginPageURL(true);
                Response.Redirect(loginURL);
            }

            if (!Page.IsPostBack)
            {
                string transStatus = this.Page.Request.Form["transStatus"];
                string returnedcallbackPW = this.Page.Request.Form["callbackPW"];
                string orderID = this.Page.Request.Form["cartId"];
                string returnedInstanceID = this.Page.Request.Form["instID"];
                string M_UserID = this.Page.Request.Form["M_UserID"];
                string postcode = this.Page.Request.Form["postcode"];
                string email = this.Page.Request.Form["email"];
                string authCurrency = this.Page.Request.Form["authCurrency"];
                string countryString = this.Page.Request.Form["countryString"];
                string authAmount = this.Page.Request.Form["authAmount"];
                string M_Addr1 = this.Page.Request.Form["M_Addr1"];
                string M_Addr2 = this.Page.Request.Form["M_Addr2"];
                string M_FirstName = this.Page.Request.Form["M_FirstName"];
                string M_LastName = this.Page.Request.Form["M_LastName"];
                string M_Business = this.Page.Request.Form["M_Business"];
                string M_StateCounty = this.Page.Request.Form["M_StateCounty"];

                Order order = OrderManager.GetOrderByID(Convert.ToInt32(orderID));
                if (order == null)
                    throw new NopException(string.Format("The order ID {0} doesn't exists", orderID));

                if (order.CustomerID != NopContext.Current.User.CustomerID)
                    throw new NopException("Not your order");

                string instanceID = SettingManager.GetSettingValue("PaymentMethod.Worldpay.InstanceID");
                if (string.IsNullOrEmpty(instanceID))
                    throw new NopException("Worldpay Instance ID is not set");

                if (string.IsNullOrEmpty(returnedInstanceID))
                    throw new NopException("Returned Worldpay Instance ID is not set");

                string callbackPassword = SettingManager.GetSettingValue("PaymentMethod.Worldpay.CallbackPassword");
                if (string.IsNullOrEmpty(callbackPassword))
                    throw new NopException("Worldpay callback password is not set");

                if (instanceID.Trim() != returnedInstanceID.Trim())
                    throw new NopException(string.Format("The Instance ID (0}) received for order {1} does not match the WorldPay Instance ID stored in the database ({2})", returnedInstanceID, orderID, instanceID));
                    
                if (transStatus.ToLower() != "y")
                    throw new NopException(string.Format("The transaction status received from WorldPay ({0}) for the order {1} was declined.", transStatus, orderID));

                if (returnedcallbackPW.Trim() != callbackPassword.Trim())
                    throw new NopException(string.Format("The callback password ({0}) received within the Worldpay Callback for the order {1} does not match that stored in your database.", returnedcallbackPW, orderID));

                OrderManager.MarkOrderAsPaid(order.OrderID);
                Response.Redirect("~/CheckoutCompleted.aspx");
            }
        }

    }
}