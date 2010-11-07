<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="NopSolutions.NopCommerce.Web.Modules.RecentlyAddedProductsControl" Codebehind="RecentlyAddedProducts.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="ProductBox1" Src="~/Modules/ProductBox1.ascx" %>
<div class="RecentlyAddedProducts">
    <div class="title">
        <table width="100%">
            <tr>
                <td style="text-align: left;">
                    <%=GetLocaleResourceString("Products.NewProducts")%>
                </td>
                <td style="text-align: right;">
                    <a href="<%=Page.ResolveUrl("~/RecentlyAddedProductsRSS.aspx")%>">
                        <asp:Image ID="imgRSS" runat="server" ImageUrl="~/images/icon_rss.gif" AlternateText="RSS" /></a>
                </td>
            </tr>
        </table>
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
