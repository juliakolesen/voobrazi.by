using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NopSolutions.NopCommerce.Web;
using NopSolutions.NopCommerce.BusinessLogic.Messages;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using orders = NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using System.Text;
using NopSolutions.NopCommerce.BusinessLogic.Utils;
using System.ComponentModel;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class OrderIndividualOrders : BaseNopUserControl
    {
        private const String Title = "Цветы живые {0}л";
        private static int number = 1;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.BindData();
        }

        public void BindData()
        {
            if (NopContext.Current.Session == null)
                return ;

            number = 1;
            Guid CustomerSessionGUID = NopContext.Current.Session.CustomerSessionGUID;
            orders.IndividualOrderCollection indOrders = orders.IndividualOrderManager.GetIndividualOrderByCurrentUserSessionGuid(CustomerSessionGUID);

            if (indOrders.Count > 0)
            {
                rptIndOrder.DataSource = indOrders;
                rptIndOrder.DataBind();
            }
        }

        public void UpdateIndividualOrders()
        {
            foreach (RepeaterItem item in rptIndOrder.Items)
            {
                Label lblIndOrderItemID = item.FindControl("lblIndOrderItemID") as Label;
                CheckBox cbRemoveIndOrderFromCart = item.FindControl("cbRemoveIndOrderFromCart") as CheckBox;

                int indOrderID = 0;
                if (lblIndOrderItemID != null && cbRemoveIndOrderFromCart != null)
                {
                    int.TryParse(lblIndOrderItemID.Text, out indOrderID);

                    if (cbRemoveIndOrderFromCart.Checked)
                    {
                        orders.IndividualOrderManager.DeleteIndividualOrder(indOrderID);
                    }
                }
            }
        }

        public String GetIndividualOrderURL(orders.IndividualOrder indOrder)
        {
            if (indOrder != null)
                return SEOHelper.GetIndividualOrderURL(indOrder);
            return string.Empty;
        }

        public String GetIndividualOrderTitle(orders.IndividualOrder indOrder)
        {
            String title = String.Format(Title, number);
            number++;
            return title;
        }

        public String GetIndividualOrderPriceString(orders.IndividualOrder indOrder)
        {
            String price = String.Empty;
            if (indOrder != null)
            {
                if (Request.Cookies["Currency"] != null && Request.Cookies["Currency"].Value == "USD")
                {
                    price = Math.Round(PriceConverter.ToUsd(indOrder.Price)).ToString() + "$";
                }
                else
                {
                    price = indOrder.Price.ToString();
                }
            }

            return price;
        }

        [DefaultValue(false)]
        public bool IsShoppingCart
        {
            get
            {
                object obj2 = this.ViewState["IsShoppingCart"];
                return ((obj2 != null) && ((bool)obj2));
            }
            set
            {
                this.ViewState["IsShoppingCart"] = value;
            }
        }
    }
}