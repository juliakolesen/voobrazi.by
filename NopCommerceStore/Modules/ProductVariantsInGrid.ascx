<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.ProductVariantsInGridControl"
    CodeBehind="ProductVariantsInGrid.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="SimpleTextBox" Src="~/Modules/SimpleTextBox.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="NumericTextBox" Src="~/Modules/NumericTextBox.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="ProductAttributes" Src="~/Modules/ProductAttributes.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="ProductPrice" Src="~/Modules/ProductPrice.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="TierPrices" Src="~/Modules/TierPrices.ascx" %>
<%@ Reference Control="~/Modules/ProductAttributes.ascx" %>
<%@ Reference Control="~/Modules/EmailTextBox.ascx" %>
<div class="ProductVariantList">
    <asp:Repeater ID="rptVariants" runat="server" OnItemCommand="rptVariants_OnItemCommand"
        OnItemDataBound="rptVariants_OnItemDataBound">
        <ItemTemplate>
            <div class="ProductVariantLine">
                <div class="picture">
                    <asp:Image ID="iProductVariantPicture" runat="server" />
                </div>
                <div class="overview">
                    <div class="productname">
                        <%#Server.HtmlEncode(Eval("Name").ToString())%>
                    </div>
                    <asp:Label runat="server" ID="ProductVariantID" Text='<%#Eval("ProductVariantID")%>'
                        Visible="false" />
                </div>
                <div class="description">
                    <asp:Literal runat="server" ID="lDescription" Visible='<%# !String.IsNullOrEmpty(Eval("Description").ToString()) %>'
                        Text='<%# Eval("Description")%>'>
                    </asp:Literal>
                </div>
                <asp:Panel runat="server" ID="pnlDownloadSample" Visible="false" CssClass="downloadsample">
                    <span class="downloadsamplebutton">
                        <asp:HyperLink runat="server" ID="hlDownloadSample" Text="<% $NopResources:Products.DownloadSample %>">
                        </asp:HyperLink>
                    </span>
                </asp:Panel>
                <nopCommerce:TierPrices ID="ctrlTierPrices" runat="server" ProductVariantID='<%#Eval("ProductVariantID") %>'>
                </nopCommerce:TierPrices>
                <div class="clear">
                </div>
                <div class="attributes">
                    <nopCommerce:ProductAttributes ID="ctrlProductAttributes" runat="server" ProductVariantID='<%#Eval("ProductVariantID") %>'>
                    </nopCommerce:ProductAttributes>
                </div>
                <div class="price">
                    <nopCommerce:ProductPrice ID="ctrlProductPrice" runat="server" ProductVariantID='<%#Eval("ProductVariantID") %>'>
                    </nopCommerce:ProductPrice>
                </div>
                <div class="addinfo">
                    <nopCommerce:NumericTextBox runat="server" ID="txtQuantity" Value="1" RequiredErrorMessage="<% $NopResources:Products.EnterQuantity %>"
                        RangeErrorMessage="<% $NopResources:Products.QuantityRange %>" MinimumValue="1"
                        MaximumValue="999999" Width="50"></nopCommerce:NumericTextBox>
                    <asp:Button ID="btnAddToCart" runat="server" Text="<% $NopResources:Products.AddToCart %>"
                        CommandName="AddToCart" CommandArgument='<%#Eval("ProductVariantID")%>' SkinID="ProductVariantAddToCartButton">
                    </asp:Button>
                    <asp:Button ID="btnAddToWishlist" runat="server" Text="<% $NopResources:Wishlist.AddToWishlist %>"
                        CommandName="AddToWishlist" CommandArgument='<%#Eval("ProductVariantID")%>' SkinID="ProductVariantAddToWishlistButton">
                    </asp:Button>
                </div>
                <div class="clear">
                </div>
                <asp:Label runat="server" ID="lblError" EnableViewState="false" CssClass="error" />
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>
