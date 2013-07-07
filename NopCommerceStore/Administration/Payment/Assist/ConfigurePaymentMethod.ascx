<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConfigurePaymentMethod.ascx.cs"
    Inherits="NopSolutions.NopCommerce.Web.Administration.Payment.Assist.ConfigurePaymentMethod" %>
<table>
    <tr>
        <td>
            OK Url:
        </td>
        <td>
            <asp:TextBox runat="server" Width="450" ID="tbOKUrl" />
        </td>
    </tr>
    <tr>
        <td>
            NO Url:
        </td>
        <td>
            <asp:TextBox runat="server" Width="450" ID="tbNoUrl" />
        </td>
    </tr>
    <tr>
        <td>
            TestMode:
        </td>
        <td>
            <asp:TextBox runat="server" Width="450" ID="tbTestMode" />
        </td>
    </tr>
    <tr>
        <td>
            PaymentUrl:
        </td>
        <td>
            <asp:TextBox runat="server" Width="450" ID="tbUrl" />
        </td>
    </tr>
</table>
