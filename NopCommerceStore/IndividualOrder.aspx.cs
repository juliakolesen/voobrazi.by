using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.Web
{
    public partial class IndividualOrder : BaseNopPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int sentMessage = CommonHelper.QueryStringInt("sentMessage");
                if (sentMessage == 0)
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
                    String order = (String)Session["quickOrderMessage"];
                    this.sentMessageLabel.Text = String.Format("{0}{1}", "Ваш индивидуальный  заказ цветов  отправлен. Вы заказали ", order);
                }
            }
        }
    }
}