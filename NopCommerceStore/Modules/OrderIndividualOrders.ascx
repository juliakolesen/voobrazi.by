<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderIndividualOrders.ascx.cs"
    Inherits="NopSolutions.NopCommerce.Web.Modules.OrderIndividualOrders" %>
<asp:Label ID="indOrderLabel" runat="server" Text="Индивидуальные заказы:" Font-Bold></asp:Label>
<table class="cart">
    <tbody>
        <tr class="cart-header-row">
        <%if (IsShoppingCart)
                      { %>
            <td width="10%">
                <%=GetLocaleResourceString("ShoppingCart.Remove")%>
            </td>
            <%} %>
            <td width="40%">
                <%=GetLocaleResourceString("ShoppingCart.Product(s)")%>
            </td>
            <td width="20%">
                <%=GetLocaleResourceString("ShoppingCart.UnitPrice")%>
            </td>
        </tr>
        <asp:Repeater ID="rptIndOrder" runat="server">
            <ItemTemplate>
                <tr class="cart-item-row">
                    <%if (IsShoppingCart)
                      { %>
                    <td width="10%">
                        <asp:CheckBox runat="server" ID="cbRemoveIndOrderFromCart" />
                    </td>
                    <%} %>
                    <td width="40%" class="indOrder">
                        <a href='<%#GetIndividualOrderURL((IndividualOrder)Container.DataItem)%>' title="View details">
                            <%#Server.HtmlEncode(GetIndividualOrderTitle((IndividualOrder)Container.DataItem))%></a>
                    </td>
                    <td width="20%">
                        <%#GetIndividualOrderPriceString((IndividualOrder)Container.DataItem)%>
                    </td>
                    <td width="20%" class="end">
                        <asp:Label ID="lblIndOrderItemID" runat="server" Visible="false" Text='<%# Eval("IndividualOrderID") %>' />
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </tbody>
</table>
