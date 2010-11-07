<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.ProductSEOControl"
    CodeBehind="ProductSEO.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="ToolTipLabel" Src="ToolTipLabelControl.ascx" %>
<table class="adminContent">
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblProductMetaKeywords" Text="<% $NopResources:Admin.ProductSEO.MetaKeywords %>"
                ToolTip="<% $NopResources:Admin.ProductSEO.MetaKeywords.ToolTip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtMetaKeywords" CssClass="adminInput" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblProductMetaDescription" Text="<% $NopResources:Admin.ProductSEO.MetaDescription %>"
                ToolTip="<% $NopResources:Admin.ProductSEO.MetaDescription.ToolTip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtMetaDescription" CssClass="adminInput" runat="server" TextMode="MultiLine"
                Height="100"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblProductMetaTitle" Text="<% $NopResources:Admin.ProductSEO.MetaTitle %>"
                ToolTip="<% $NopResources:Admin.ProductSEO.MetaTitle.ToolTip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtMetaTitle" CssClass="adminInput" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblProductSEName" Text="<% $NopResources:Admin.ProductSEO.SEName %>"
                ToolTip="<% $NopResources:Admin.ProductSEO.SEName.ToolTip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtSEName" CssClass="adminInput" runat="server"></asp:TextBox>
        </td>
    </tr>
</table>
