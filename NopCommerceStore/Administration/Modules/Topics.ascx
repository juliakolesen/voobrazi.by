<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.TopicsControl"
    CodeBehind="Topics.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="ToolTipLabel" Src="ToolTipLabelControl.ascx" %>
<div class="section-header">
    <div class="title">
        <img src="Common/ico-content.png" alt="<%=GetLocaleResourceString("Admin.Topics.Title")%>" />
        <%=GetLocaleResourceString("Admin.Topics.Title")%>
    </div>
    <div class="options">
        <input type="button" onclick="location.href='TopicAdd.aspx'" value="<%=GetLocaleResourceString("Admin.Topics.AddNewButton.Text")%>"
            id="btnAddNew" class="adminButtonBlue" title="<%=GetLocaleResourceString("Admin.Topics.AddNewButton.Tooltip")%>" />
    </div>
</div>
<table>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblLanguage" Text="<% $NopResources:Admin.Topics.Language %>"
                ToolTip="<% $NopResources:Admin.Topics.Language.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
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
<asp:GridView ID="gvTopics" runat="server" AutoGenerateColumns="False" Width="100%">
    <Columns>
        <asp:BoundField DataField="TopicID" HeaderText="Topic ID" Visible="False"></asp:BoundField>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.Topics.Name %>" ItemStyle-Width="40%">
            <ItemTemplate>
                <%#Eval("Name")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.Topics.EditInfo %>" HeaderStyle-HorizontalAlign="Center"
            ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <a href="TopicDetails.aspx?TopicID=<%#Eval("TopicID")%>" title="<%#GetLocaleResourceString("Admin.Topics.EditInfo.Tooltip")%>">
                    <%#GetLocaleResourceString("Admin.Topics.EditInfo.Link")%></a>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.Topics.EditContent %>" HeaderStyle-HorizontalAlign="Center"
            ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <a href="TopicLocalizedDetails.aspx?TopicID=<%#Eval("TopicID")%>&LanguageID=<%#GetSelectedLanguageID()%>"
                    title="<%#GetLocaleResourceString("Admin.Topics.EditContent.Tooltip")%>">
                    <%#GetLocaleResourceString("Admin.Topics.EditContent.Link")%></a>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
