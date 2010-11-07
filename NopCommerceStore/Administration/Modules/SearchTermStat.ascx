<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.SearchTermStatControl"
    CodeBehind="SearchTermStat.ascx.cs" %>
<div class="statisticsTitle">
    <%=GetLocaleResourceString("Admin.SearchTermStat.PopularSearches")%>
</div>
<asp:GridView ID="gvSearchTermStat" runat="server" AutoGenerateColumns="False">
    <Columns>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.SearchTermStat.SearchTerm %>"
            ItemStyle-Width="150px">
            <ItemTemplate>
                <%#Server.HtmlEncode(Eval("SearchTerm").ToString())%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="SearchCount" HeaderText="<% $NopResources:Admin.SearchTermStat.Count %>"
            ItemStyle-Width="50px"></asp:BoundField>
    </Columns>
</asp:GridView>
