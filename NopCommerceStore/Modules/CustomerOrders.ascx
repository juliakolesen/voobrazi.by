<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.CustomerOrdersControl"
    CodeBehind="CustomerOrders.ascx.cs" %>
<div class="CustomerOrders">
    <div class="OrderList">
        <asp:Repeater ID="rptrOrders" runat="server">
            <ItemTemplate>
                <div class="OrderItem">
                    <table width="100%" cellspacing="0" cellpadding="2" border="0">
                        <tbody>
                            <tr>
                                <td style="vertical-align: middle;">
                                    <b>
                                        <%=GetLocaleResourceString("Account.OrderNumber")%>:
                                        <%#Eval("OrderID")%></b>
                                </td>
                                <td align="right">
                                    <asp:Button runat="server" ID="btnOrderDetails" OnCommand="btnOrderDetails_Click"
                                        Text="<% $NopResources:Common.Details %>" ValidationGroup="OrderDetails" CommandArgument='<%# Eval("OrderID") %>'
                                        SkinID="OrderDetailsButton" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table cellspacing="0" cellpadding="2" border="0">
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <div>
                                                        <%=GetLocaleResourceString("Order.OrderStatus")%>
                                                        <%#OrderManager.GetOrderStatusName(Convert.ToInt32(Eval("OrderStatusID")))%></div>
                                                    <div>
                                                        <%=GetLocaleResourceString("Account.OrderDate")%>:
                                                        <%#DateTimeHelper.ConvertToUserTime((DateTime)Eval("CreatedOn")).ToString()%></div>
                                                    <div>
                                                        <%=GetLocaleResourceString("Account.OrderTotal")%>:
                                                        <%# GetOrderTotal(Container.DataItem as Order)%>
                                                    </div>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </ItemTemplate>
            <SeparatorTemplate>
                <div class="clear">
                </div>
            </SeparatorTemplate>
        </asp:Repeater>
    </div>
</div>
