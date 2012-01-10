<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchResultsControl.ascx.cs" 
Inherits="NopSolutions.NopCommerce.Web.Modules.SearchResultsControl" %>
<%@ Register TagPrefix="nopCommerce" TagName="ProductBox1" Src="~/Modules/ProductBox1.ascx" %>
        <div class="navigator">
            <table>
                <tr>
                    <td width="250px">
                        <asp:DropDownList ID="sortBy" AutoPostBack="true" runat="server" OnSelectedIndexChanged="productsCount_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td width="250px">
                        <nopCommerce:Pager runat="server" ID="productsPager" FirstButtonText="" LastButtonText=""
                            NextButtonText="»" PreviousButtonText="«" CurrentPageText="Pager.CurrentPage" />
                    </td>
                    <td width="250px">
                        <asp:DropDownList ID="productsCount" AutoPostBack="true" runat="server" OnSelectedIndexChanged="productsCount_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
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
<br />
<asp:Label runat="server" ID="lblNoProductsFound" Text="<% $NopResources: Admin.Products.NoProductsFound%>"
    Visible="false"></asp:Label>