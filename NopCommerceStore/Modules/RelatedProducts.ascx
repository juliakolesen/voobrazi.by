<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.RelatedProductsControl"
    CodeBehind="RelatedProducts.ascx.cs" %>
<p class="pink_">
    Сопутствующие товары:</p>
<div class="ramka">
    <div class="navigatorRelatedProducts">
        <nopCommerce:Pager runat="server" ID="relatedProductPager" FirstButtonText="" LastButtonText=""
            NextButtonText="»" PreviousButtonText="«" CurrentPageText="Pager.CurrentPage" />
    </div>
    <asp:Repeater ID="dlRelatedProducts" runat="server" OnItemDataBound="dlRelatedProducts_ItemDataBound">
        <HeaderTemplate>
            <table border="0" cellspacing="0" cellpadding="0" class="small_preview" align="center">
                <tr>
        </HeaderTemplate>
        <ItemTemplate>
            <td width="96" align="center">
                <div class="small-price">
                    <div class="price">
                        <asp:Label runat="server" ID="lblPrice" /></div>
                </div>
                <asp:HyperLink ID="hlProduct" runat="server">
                    <asp:Image ID="hlImageLink" Height="82" runat="server" /></asp:HyperLink>
            </td>
        </ItemTemplate>
        <FooterTemplate>
            </tr> </table>
        </FooterTemplate>
    </asp:Repeater>
</div>
