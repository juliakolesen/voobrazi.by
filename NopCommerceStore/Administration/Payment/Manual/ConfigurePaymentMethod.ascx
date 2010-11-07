<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="NopSolutions.NopCommerce.Web.Administration.Payment.Manual.ConfigurePaymentMethod" Codebehind="ConfigurePaymentMethod.ascx.cs" %>
<table class="adminContent">
    <tr>
        <td colspan="2" width="100%">
            <hr />
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            After checkout mark payment as:
        </td>
        <td class="adminData">
            <asp:RadioButton runat="server" ID="rbPending" Text="Pending" GroupName="Mode"></asp:RadioButton><br />
            <asp:RadioButton runat="server" ID="rbAuthorize" Text="Authorized" GroupName="Mode">
            </asp:RadioButton><br />
            <asp:RadioButton runat="server" ID="rbAuthorizeAndCapture" Text="Authorized and Captured (Paid)"
                GroupName="Mode"></asp:RadioButton>
        </td>
    </tr>
</table>
