<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.MessageTemplatesControl"
    CodeBehind="MessageTemplates.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="ToolTipLabel" Src="ToolTipLabelControl.ascx" %>
<div class="section-title">
    <img src="Common/ico-content.png" alt="<%=GetLocaleResourceString("Admin.MessageTemplates.Title")%>" />
    <%=GetLocaleResourceString("Admin.MessageTemplates.Title")%>
</div>
<table>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblLanguage" Text="<% $NopResources:Admin.MessageTemplates.Language %>"
                ToolTip="<% $NopResources:Admin.MessageTemplates.Language.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:DropDownList ID="ddlLanguage" AutoPostBack="True" OnSelectedIndexChanged="ddlLanguage_SelectedIndexChanged"
                CssClass="adminInput" runat="server">
            </asp:DropDownList>
        </td>
    </tr>
</table>
<p>
</p>
<asp:GridView ID="gvMessageTemplates" runat="server" AutoGenerateColumns="False"
    Width="100%">
    <Columns>
        <asp:BoundField DataField="MessageTemplateID" HeaderText="MessageTemplate ID" Visible="False">
        </asp:BoundField>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.MessageTemplates.Name %>" ItemStyle-Width="70%">
            <ItemTemplate>
                <%#Eval("Name")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.MessageTemplates.Edit %>" HeaderStyle-HorizontalAlign="Center"
            ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <a href="MessageTemplateDetails.aspx?MessageTemplateID=<%#Eval("MessageTemplateID")%>&LanguageID=<%#GetSelectedLanguageID()%>"
                    title="<%=GetLocaleResourceString("Admin.MessageTemplates.Edit.Tooltip")%>">
                    <%=GetLocaleResourceString("Admin.MessageTemplates.Edit")%>
                </a>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
