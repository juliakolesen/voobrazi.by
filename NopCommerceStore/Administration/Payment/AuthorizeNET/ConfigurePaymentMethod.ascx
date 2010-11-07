<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="NopSolutions.NopCommerce.Web.Administration.Payment.AuthorizeNET.ConfigurePaymentMethod" Codebehind="ConfigurePaymentMethod.ascx.cs" %>
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
            Transaction mode:
        </td>
        <td class="adminData">
            <asp:RadioButton runat="server" ID="rbAuthorize" Text="Authorize" GroupName="Mode">
            </asp:RadioButton><br />
            <asp:RadioButton runat="server" ID="rbAuthorizeAndCapture" Text="Authorize and Capture"
                GroupName="Mode"></asp:RadioButton>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            Transaction key:
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtTransactionKey" runat="server" CssClass="adminInput"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            Login ID:
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtLoginID" runat="server" CssClass="adminInput"></asp:TextBox>
        </td>
    </tr>
</table>
