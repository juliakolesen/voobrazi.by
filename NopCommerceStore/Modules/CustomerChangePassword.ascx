<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.CustomerChangePasswordControl"
    CodeBehind="CustomerChangePassword.ascx.cs" %>
<div class="CustomerPassRecovery">
    <asp:Panel runat="server" ID="pnlChangePasswordError" CssClass="ErrorBlock">
        <div class="messageError">
            <asp:Literal ID="lChangePasswordErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
        </div>
    </asp:Panel>
    <div class="clear">
    </div>
    <div class="SectionBody">
        <table class="TableContainer">
            <tbody>
                <tr class="Row">
                    <td class="ItemName">
                        <%=GetLocaleResourceString("Account.OldPassword")%>:
                    </td>
                    <td class="ItemValue">
                        <asp:TextBox ID="txtOldPassword" runat="server" MaxLength="50" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvOldPassword" runat="server" ControlToValidate="txtOldPassword"
                            ErrorMessage="<% $NopResources:Account.OldPasswordIsRequired %>" ToolTip="<% $NopResources:Account.OldPasswordIsRequired %>"
                            ValidationGroup="ChangePassword" />
                    </td>
                </tr>
                <tr class="Row">
                    <td class="ItemName">
                        <%=GetLocaleResourceString("Account.NewPassword")%>:
                    </td>
                    <td class="ItemValue">
                        <asp:TextBox ID="txtNewPassword" runat="server" MaxLength="50" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvNewPassword" runat="server" ControlToValidate="txtNewPassword"
                            ErrorMessage="<% $NopResources:Account.NewPasswordIsRequired %>" ToolTip="<% $NopResources:Account.NewPasswordIsRequired %>"
                            ValidationGroup="ChangePassword" />
                    </td>
                </tr>
                <tr class="Row">
                    <td class="ItemName">
                        <%=GetLocaleResourceString("Account.NewPasswordConfirmation")%>:
                    </td>
                    <td class="ItemValue">
                        <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" ControlToValidate="txtConfirmPassword"
                            ErrorMessage="<% $NopResources:Account.ConfirmPasswordIsRequired %>" ToolTip="<% $NopResources:Account.ConfirmPasswordIsRequired %>"
                            ValidationGroup="ChangePassword" />
                    </td>
                </tr>
                <tr class="Row">
                    <td class="ItemValue" colspan="2">
                        <asp:CompareValidator ID="cvPasswordCompare" runat="server" ControlToCompare="txtNewPassword"
                            ControlToValidate="txtConfirmPassword" Display="Dynamic" ErrorMessage="<% $NopResources:Account.EnteredPasswordsDoNotMatch %>"
                            ToolTip="<% $NopResources:Account.EnteredPasswordsDoNotMatch %>" ValidationGroup="ChangePassword" />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div class="clear">
    </div>
    <div class="Button">
        <asp:Button ID="btnChangePassword" runat="server" OnClick="btnChangePassword_Click"
            Text="<% $NopResources:Account.ChangePasswordButton %>" ValidationGroup="ChangePassword"
            SkinID="ChangePasswordButton" /><br style="line-height: 1px;" />
    </div>
</div>
