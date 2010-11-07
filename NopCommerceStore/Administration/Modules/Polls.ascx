<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.PollsControl"
    CodeBehind="Polls.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="ToolTipLabel" Src="ToolTipLabelControl.ascx" %>
<div class="section-header">
    <div class="title">
        <img src="Common/ico-content.png" alt="<%=GetLocaleResourceString("Admin.Polls.Title")%>" />
        <%=GetLocaleResourceString("Admin.Polls.Title")%>
    </div>
    <div class="options">
        <asp:Button runat="server" Text="<% $NopResources:Admin.Polls.SaveButton.Text %>"
            CssClass="adminButtonBlue" ID="btnSave" ValidationGroup="PollsSettings" OnClick="btnSave_Click"
            ToolTip="<% $NopResources:Admin.Polls.SaveButton.Tooltip %>" />
        <input type="button" onclick="location.href='PollAdd.aspx'" value="<%=GetLocaleResourceString("Admin.Polls.AddNewButton.Text")%>"
            id="btnAddNew" class="adminButtonBlue" title="<%=GetLocaleResourceString("Admin.Polls.AddNewButton.Tooltip")%>" />
    </div>
</div>
<table width="100%">
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblShowOnMainPage" Text="<% $NopResources:Admin.Polls.ShowOnMainPage %>"
                ToolTip="<% $NopResources:Admin.Polls.ShowOnMainPage.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:CheckBox runat="server" ID="cbShowPollsOnMainPage" Checked="true"></asp:CheckBox>
        </td>
    </tr>
</table>
<p>
</p>
<asp:GridView ID="gvPolls" runat="server" AutoGenerateColumns="False" Width="100%">
    <Columns>
        <asp:BoundField DataField="PollID" HeaderText="Poll ID" Visible="False"></asp:BoundField>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.Polls.Name %>" ItemStyle-Width="30%">
            <ItemTemplate>
                <%#Server.HtmlEncode(Eval("Name").ToString())%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.Polls.Language %>" HeaderStyle-HorizontalAlign="Center"
            ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <%#((Language)Eval("Language")).Name%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.Polls.DisplayOrder %>" HeaderStyle-HorizontalAlign="Center"
            ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <%#Eval("DisplayOrder")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.Polls.Published %>" HeaderStyle-HorizontalAlign="Center"
            ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <nopCommerce:ImageCheckBox runat="server" ID="cbPublished" Checked='<%# Eval("Published") %>'>
                </nopCommerce:ImageCheckBox>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.Polls.Edit %>" HeaderStyle-HorizontalAlign="Center"
            ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <a href="PollDetails.aspx?PollID=<%#Eval("PollID")%>" title="<%#GetLocaleResourceString("Admin.Polls.Edit.Tooltip")%>">
                    <%#GetLocaleResourceString("Admin.Polls.Edit")%></a>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
