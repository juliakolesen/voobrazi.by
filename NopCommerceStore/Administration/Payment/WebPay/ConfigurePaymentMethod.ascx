<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConfigurePaymentMethod.ascx.cs" Inherits="NopSolutions.NopCommerce.Web.Administration.Payment.WebPay.ConfigurePaymentMethod" %>
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
            Version: 
        </td>
        <td>
            <asp:TextBox runat="server" ID="tbVersion" />
        </td>
    </tr>
    <tr>
        <td>
            Store Id: 
        </td>
        <td>
            <asp:TextBox runat="server" ID="tbStoreId" />
        </td>
    </tr>
    <tr>
        <td>
            Store Name: 
        </td>
        <td>
            <asp:TextBox runat="server" Width="450" ID="tbStoreName" />
        </td>
    </tr>
    <tr>
        <td>
            Return Url: 
        </td>
        <td>
            <asp:TextBox runat="server" Width="450" ID="tbReturnUrl" />
        </td>
    </tr>
    <tr>
        <td>
            Cancel Url: 
        </td>
        <td>
            <asp:TextBox runat="server" Width="450" ID="tbCancelUrl" />
        </td>
    </tr>
    <tr>
        <td>
            Notify Url: 
        </td>
        <td>
            <asp:TextBox runat="server" Width="450" ID="tbNotifyUrl" />
        </td>
    </tr>
    <tr>
        <td>
            Shipping Name: 
        </td>
        <td>
            <asp:TextBox runat="server" Width="450" ID="tbShippingName" />
        </td>
    </tr>
    <tr>
        <td>
            Secret Key: 
        </td>
        <td>
            <asp:TextBox runat="server" Width="450" ID="tbSecretKey" />
        </td>
    </tr>
    <tr>
        <td>
            Password: 
        </td>
        <td>
            <asp:TextBox runat="server" Width="450" ID="tbPassword" />
        </td>
    </tr>
</table>