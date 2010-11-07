<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.FroogleControl"
    CodeBehind="Froogle.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="NumericTextBox" Src="NumericTextBox.ascx" %>
<div class="section-header">
    <div class="title">
        <img src="Common/ico-promotions.png" alt="<%=GetLocaleResourceString("Admin.Froogle.Title")%>" />
        <%=GetLocaleResourceString("Admin.Froogle.Title")%>
    </div>
    <div class="options">
        <asp:Button runat="server" Text="<% $NopResources:Admin.Froogle.SaveButton.Text %>"
            CssClass="adminButtonBlue" ID="btnSave" ValidationGroup="CategorySettings" OnClick="btnSave_Click"
            ToolTip="<% $NopResources:Admin.Froogle.SaveButton.ToolTip%>" />
        <asp:Button runat="server" Text="<% $NopResources:Admin.Froogle.GenerateButton.Text %>"
            CssClass="adminButtonBlue" ID="btnGenerate" ValidationGroup="GenerateFroogle"
            OnClick="btnGenerate_Click" ToolTip="<% $NopResources:Admin.Froogle.GenerateButton.Tooltip %>" />
    </div>
</div>
<table width="100%">
    <tr>
        <td class="adminTitle">
            <%=GetLocaleResourceString("Admin.Froogle.AllowPublicAccess")%>
        </td>
        <td class="adminData">
            <asp:CheckBox runat="server" ID="cbAllowPublicFroogleAccess" />
        </td>
    </tr>
</table>
<p>
</p>
<br />
<asp:Label runat="server" ID="lblResult" EnableViewState="false" />
