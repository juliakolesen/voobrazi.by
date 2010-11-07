<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="NopSolutions.NopCommerce.Web.Templates.Categories.ProductsInLines2" Codebehind="ProductsInLines2.ascx.cs" %>
<div class="CategoryPage">
    <div class="CategoryTitle">
        <asp:Literal runat="server" ID="lName"></asp:Literal>
    </div>
    <div class="clear">
    </div>
    <div class="CategoryDescription">
        <asp:Literal runat="server" ID="lDescription"></asp:Literal>
    </div>
    <div class="clear">
    </div>
    <div class="SubCategoryList">
        <asp:Repeater ID="rptrSubCategories" runat="server" OnItemDataBound="rptrSubCategories_ItemDataBound">
            <ItemTemplate>
                <asp:HyperLink ID="hlCategory" runat="server" Text='<%#Server.HtmlEncode(Eval("Name").ToString()) %>' />
            </ItemTemplate>
            <SeparatorTemplate>
                <br />
            </SeparatorTemplate>
        </asp:Repeater>
    </div>
    <div class="clear">
    </div>
    <div class="ProductList2">
        <asp:ListView ID="lvCatalog" runat="server" OnItemDataBound="lvCatalog_ItemDataBound">
            <LayoutTemplate>
                <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
            </LayoutTemplate>
            <ItemTemplate>
                <asp:HyperLink ID="hlProduct" runat="server" Text='<%#Server.HtmlEncode(Eval("Name").ToString()) %>' />
            </ItemTemplate>
            <ItemSeparatorTemplate>
                <br />
            </ItemSeparatorTemplate>
        </asp:ListView>
        <nopCommerce:Pager runat="server" ID="catalogPager" FirstButtonText="<% $NopResources:Pager.First %>"
            LastButtonText="<% $NopResources:Pager.Last %>" NextButtonText="<% $NopResources:Pager.Next %>"
            PreviousButtonText="<% $NopResources:Pager.Previous %>" CurrentPageText="Pager.CurrentPage" />
    </div>
</div>
