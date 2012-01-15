<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Templates.Categories.ProductsInGrid"
    CodeBehind="ProductsInGrid.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="ProductBox1" Src="~/Modules/ProductBox1.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="DesignVariant" Src="~/Modules/DesignVariant.ascx" %>
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
                <asp:Repeater ID="rptrCategoryBreadcrumb" runat="server">
                    <ItemTemplate>
                        <%#Server.HtmlEncode(Eval("Name").ToString()) %>
                    </ItemTemplate>
                    <SeparatorTemplate>
                        /
                    </SeparatorTemplate>
                </asp:Repeater>
            </h2>
        </div>
    </div>
    <div class="seccont_middle">
        <div class="navigator">
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
            </table>
        </div>
        <nopCommerce:DesignVariant ID="designVariant" runat="server"/>
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
