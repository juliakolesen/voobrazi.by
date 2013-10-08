<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.TopicInfoControl"
    CodeBehind="TopicInfo.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="ToolTipLabel" Src="ToolTipLabelControl.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="SimpleTextBox" Src="SimpleTextBox.ascx" %>
<table class="adminContent">
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblName" Text="<% $NopResources:Admin.TopicInfo.Name %>"
                ToolTip="<% $NopResources:Admin.TopicInfo.Name.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <nopCommerce:SimpleTextBox runat="server" ID="txtName" CssClass="adminInput" ErrorMessage="<% $NopResources:Admin.TopicInfo.Name.ErrorMessage %>">
            </nopCommerce:SimpleTextBox>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <table class="adminContent">
                <tr>
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblMetaKeywords" Text="<% $NopResources:Admin.ProductSEO.MetaKeywords %>"
                            ToolTip="<% $NopResources:Admin.ProductSEO.MetaKeywords.ToolTip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:TextBox ID="txtMetaKeywords" CssClass="adminInput" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblMetaDescription" Text="<% $NopResources:Admin.ProductSEO.MetaDescription %>"
                            ToolTip="<% $NopResources:Admin.TopicInfo.ProductSEO.ToolTip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:TextBox ID="txtMetaDescription" CssClass="adminInput" runat="server" TextMode="MultiLine"
                            Height="100"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblProductMetaTitle" Text="<% $NopResources:Admin.ProductSEO.MetaTitle %>"
                            ToolTip="<% $NopResources:Admin.TopicInfo.ProductSEO.ToolTip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:TextBox ID="txtMetaTitle" CssClass="adminInput" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
