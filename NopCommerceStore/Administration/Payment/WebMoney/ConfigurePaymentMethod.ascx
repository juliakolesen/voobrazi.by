<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConfigurePaymentMethod.ascx.cs" Inherits="NopSolutions.NopCommerce.Web.Administration.Payment.WebMoney.ConfigurePaymentMethod" %>
<table>
    <tr>
        <td>
            Use Sandbox: 
        </td>
        <td>
            <asp:Checkbox runat="server" ID="chbUseSandbox" />
        </td>
    </tr>
    <tr>
        <td>
            Режим симуляции: 
        </td>
        <td>
            <asp:DropDownList runat="server" id="ddlSimulationMode">
                <asp:ListItem value="0" selected="False">Успешно для всех платежей</asp:ListItem>
                <asp:ListItem value="1" selected="False">С ошибкой для всех платежей</asp:ListItem>
                <asp:ListItem value="2" selected="True">80% успешно, 20% с ошибкой</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>
            Номер кошелька: 
        </td>
        <td>
            <asp:TextBox runat="server" ID="tbWMId" />
        </td>
    </tr>
</table>