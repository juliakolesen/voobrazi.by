using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Orders;

namespace NopSolutions.NopCommerce.Web
{
    public partial class IndividualOrder : BaseNopPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int sentMessage = CommonHelper.QueryStringInt("sentMessage");
                int indOrderID = CommonHelper.QueryStringInt("orderID", -1);
                if (sentMessage == 0 && indOrderID == -1)
                {
                    this.indivOrderControl.Visible = true;
                    this.sentMessageLabel.Visible = false;
                    this.orderSenrTopic.Visible = false;
                }
                else 
                {
                    this.indivOrderControl.Visible = false;
                    this.sentMessageLabel.Visible = true;
                    this.orderSenrTopic.Visible = true;
                    String order = String.Empty;
                    String startText = String.Empty; 
                    if (indOrderID == -1)
                    {
                        order = (String)Session["quickOrderMessage"];
                        startText = "Ваш индивидуальный  заказ цветов  отправлен. Вы заказали ";
                    }
                    else
                    {
                        BusinessLogic.Orders.IndividualOrder indOrder = IndividualOrderManager.GetIndividualOrderByID(indOrderID);
                        order = indOrder != null ? indOrder.OrderText: "удален.";
                        startText = "Ваш индивидуальный заказ ";
                    }

                    this.sentMessageLabel.Text = String.Format("{0}{1}", startText, order);
                }
            }
        }
    }
}