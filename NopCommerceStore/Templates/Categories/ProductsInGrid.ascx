<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Templates.Categories.ProductsInGrid"
    CodeBehind="ProductsInGrid.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="ProductBox1" Src="~/Modules/ProductBox1.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="DesignVariant" Src="~/Modules/DesignVariant.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="WeddingBunchVariant" Src="~/Modules/WeddingBunchVariant.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="HouseFlowersVariant" Src="~/Modules/HouseFlowersVariant.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="ColorsFilterControl" Src="~/Modules/ColorsFilterControl.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="PriceFilterControl" Src="~/Modules/PriceFilterControl.ascx" %>
<style>
    .seccont_middle .text
    {
        padding: 0 40px 1px;
    }
</style>
<div class="seccontainer">
    <div class="seccont_top">
        <div class="titl">
            <h2>
                <asp:SiteMapPath ID="siteMapPath" runat="server" PathSeparator=" / " SiteMapProvider="NopDefaultXmlSiteMapProvider">
                </asp:SiteMapPath>
            </h2>
        </div>
    </div>
    <div class="seccont_middle">
        <div class="navigator">
            <p>
                <asp:Literal runat="server" ID="SmallDescription"></asp:Literal>
            </p>
            <table>
                <tr>
                    <td width="125px">
                        Сортировать:
                    </td>
                    <td width="150px">
                        <asp:DropDownList ID="sortBy" AutoPostBack="true" runat="server" OnSelectedIndexChanged="productsCount_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td width="200px">
                        <nopCommerce:Pager runat="server" ID="productsPager" FirstButtonText="" LastButtonText=""
                            NextButtonText="»" PreviousButtonText="«" CurrentPageText="Pager.CurrentPage" />
                    </td>
                    <td width="100px">
                        Показать:
                    </td>
                    <td width="130px">
                        <asp:DropDownList ID="productsCount" AutoPostBack="true" runat="server" OnSelectedIndexChanged="productsCount_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <th colspan="4" style="height: 10px">
                        <nopCommerce:PriceFilterControl ID="priceFilter" runat="server"></nopCommerce:PriceFilterControl>
                    </th>
                </tr>
            </table>
        </div>
        <nopCommerce:DesignVariant ID="designVariant" runat="server" Visible="false" />
        <nopCommerce:WeddingBunchVariant ID="weddingBunchVariant" runat="server" Visible="false" />
        <nopCommerce:HouseFlowersVariant ID="houseFlowersVariant" runat="server" Visible="false" />
        <nopCommerce:ColorsFilterControl ID="colorsFilter" runat="server" />
        <div class="text">
            <p style="margin-top: 5px;">
                <img src="<%= Page.ResolveUrl("~/images/ff_images/horizontal-separator.gif") %>"
                    alt="" /></p>
            <asp:DataList ID="dlProducts" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"
                RepeatLayout="Table" ItemStyle-CssClass="flow_item" ItemStyle-VerticalAlign="top">
                <ItemTemplate>
                    <nopCommerce:ProductBox1 ID="ctrlProductBox" Product='<%# Container.DataItem %>'
                        runat="server" />
                </ItemTemplate>
            </asp:DataList>
            <asp:HyperLink runat="server" ID="indOrderBanner" title="Оформите индивидуальный заказ"
                NavigateUrl="~/IndividualOrder.aspx">
					<img src="../../images/ff_images/individualOderBanner.gif" alt="" height="100"/>
            </asp:HyperLink>
            <asp:DataList ID="dlProducts2" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"
                RepeatLayout="Table" ItemStyle-CssClass="flow_item" ItemStyle-VerticalAlign="top">
                <ItemTemplate>
                    <nopCommerce:ProductBox1 ID="ctrlProductBox2" Product='<%# Container.DataItem %>'
                        runat="server" />
                </ItemTemplate>
            </asp:DataList>
        </div>
        <div class="navigator">
            <nopCommerce:Pager runat="server" ID="productsPagerBottom" FirstButtonText="" LastButtonText=""
                NextButtonText="»" PreviousButtonText="«" CurrentPageText="Pager.CurrentPage" />
        </div>
        <div class="text">
            <p>
                <asp:Literal runat="server" ID="lDescription"></asp:Literal>
            </p>
        </div>
    </div>
    <div class="seccont_bottom">
    </div>
</div>
