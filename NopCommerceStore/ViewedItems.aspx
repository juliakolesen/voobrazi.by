<%@ Page Language="C#" MasterPageFile="~/MasterPages/OneColumn.master" AutoEventWireup="true"
    CodeBehind="ViewedItems.aspx.cs" Title="Вы смотрели" Inherits="NopCommerceStore.ViewedItems" %>

<%@ Register TagPrefix="nopCommerce" TagName="ProductBox1" Src="~/Modules/ProductBox1.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="Server">
    <div class="navigator">
        <table>
            <tr>
                <td width="275px">
                </td>
                <td width="200px">
                    <nopCommerce:Pager runat="server" ID="productsPager" FirstButtonText="" LastButtonText=""
                        NextButtonText="»" PreviousButtonText="«" CurrentPageText="Pager.CurrentPage" />
                </td>
                <td width="130px">
                    Показать:
                </td>
                <td width="100px">
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
</asp:Content>
