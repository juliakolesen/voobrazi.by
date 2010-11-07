<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="NopSolutions.NopCommerce.Web.Administration.Payment.Moneybookers.ConfigurePaymentMethod" Codebehind="ConfigurePaymentMethod.ascx.cs" %>
<table class="adminContent">
    <tr>
        <td colspan="2" width="100%">
            <hr />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <b>If you're using this gateway remember that you should set your store primary currency
                to US Dollar.</b>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            Moneybookers Email:
        </td>
        <td class="adminData">
            <asp:TextBox runat="server" ID="txtEmail" CssClass="adminInput"></asp:TextBox>
        </td>
    </tr>
</table>
