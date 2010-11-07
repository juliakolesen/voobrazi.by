<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="NopSolutions.NopCommerce.Web.Modules.RecentlyViewedProductsControl" Codebehind="RecentlyViewedProducts.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="ProductBox1" Src="~/Modules/ProductBox1.ascx" %>
<div class="RecentlyViewedProducts">
    <div class="title">
        <%=GetLocaleResourceString("Products.RecentlyViewedProducts")%>
    </div>
    <div class="clear">
    </div>
    <div class="ProductGrid">
        <asp:DataList ID="dlCatalog" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
            RepeatLayout="Table" ItemStyle-CssClass="ItemBox">
            <ItemTemplate>
                <nopCommerce:ProductBox1 ID="ctrlProductBox" Product='<%# Container.DataItem %>' runat="server" />
            </ItemTemplate>
        </asp:DataList>
    </div>
</div>
