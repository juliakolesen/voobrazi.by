<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.CustomerOrdersControl"
    CodeBehind="CustomerOrders.ascx.cs" %>
<asp:Repeater ID="rptrOrders" runat="server">
    <ItemTemplate>
        <div>
            <p>
                <strong>
                    <%#GetLocaleResourceString("Admin.CustomerOrders.OrderID")%>
                    <%#Eval("OrderID")%>
                </strong>
            </p>
            <%#GetLocaleResourceString("Admin.CustomerOrders.Date")%>
            <%#DateTimeHelper.ConvertToUserTime((DateTime)Eval("CreatedOn")).ToString()%>
            <br />
            <%#GetLocaleResourceString("Admin.CustomerOrders.OrderStatus")%>
            <%#OrderManager.GetOrderStatusName(Convert.ToInt32(Eval("OrderStatusID")))%>
            <br />
            <%#GetLocaleResourceString("Admin.CustomerOrders.PaymentStatus")%>
            <%#PaymentStatusManager.GetPaymentStatusName(Convert.ToInt32(Eval("PaymentStatusID")))%>
            <br />
            <%#GetLocaleResourceString("Admin.CustomerOrders.ShippingStatus")%>
            <%#ShippingStatusManager.GetShippingStatusName(Convert.ToInt32(Eval("ShippingStatusID")))%>
            <br />
            <%#GetLocaleResourceString("Admin.CustomerOrders.OrderTotal")%>
            <%#Server.HtmlEncode(PriceHelper.FormatPrice(Convert.ToDecimal(Eval("OrderTotal")), true, false))%>
            <p>
                <a href="OrderDetails.aspx?OrderID=<%#Eval("OrderID")%>">
                    <%#GetLocaleResourceString("Admin.CustomerOrders.Details")%></a>
            </p>
        </div>
    </ItemTemplate>
    <SeparatorTemplate>
        <hr />
    </SeparatorTemplate>
</asp:Repeater>
