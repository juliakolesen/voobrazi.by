<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="NopSolutions.NopCommerce.Web.Administration.Payment.TwoCheckout.ConfigurePaymentMethod" Codebehind="ConfigurePaymentMethod.ascx.cs" %>
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
            Use Sandbox:
        </td>
        <td class="adminData">
            <asp:CheckBox ID="cbUseSandbox" runat="server"></asp:CheckBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            Vendor ID:
        </td>
        <td class="adminData">
            <asp:TextBox runat="server" ID="txtVendorID" CssClass="adminInput"></asp:TextBox>
        </td>
    </tr>
</table>
