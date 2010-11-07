<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.WishlistControl"
    CodeBehind="Wishlist.ascx.cs" %>
<asp:Panel class="WishlistContent" runat="server" ID="pnlEmptyCart">
    <%=GetLocaleResourceString("Wishlist.WishlistIsEmpty")%>
</asp:Panel>
<asp:Panel class="WishlistContent" runat="server" ID="pnlCart">
    <table class="cart">
        <tbody>
            <tr class="cart-header-row">
                <%if (IsEditable)
                  { %>
                <td width="10%">
                    <%=GetLocaleResourceString("Wishlist.Remove")%>
                </td>
                <%} %>
                <%if (SettingManager.GetSettingValueBoolean("Display.ShowProductImagesOnWishList"))
                  {%>
                <td class="picture">
                </td>
                <%} %>
                <td width="40%">
                    <%=GetLocaleResourceString("Wishlist.Product(s)")%>
                </td>
                <td width="20%">
                    <%=GetLocaleResourceString("Wishlist.UnitPrice")%>
                </td>
                <td width="10%">
                    <%=GetLocaleResourceString("Wishlist.Quantity")%>
                </td>
                <td width="20%" class="end">
                    <%=GetLocaleResourceString("Wishlist.ItemTotal")%>
                </td>
            </tr>
            <asp:Repeater ID="rptShoppingCart" runat="server">
                <ItemTemplate>
                    <tr class="cart-item-row">
                        <%if (IsEditable)
                          { %>
                        <td width="10%">
                            <asp:CheckBox runat="server" ID="cbRemoveFromCart" />
                        </td>
                        <%} %>
                        <%if (SettingManager.GetSettingValueBoolean("Display.ShowProductImagesOnWishList"))
                          {%>
                        <td class="productpicture">
                            <asp:Image ID="iProductVariantPicture" runat="server" ImageUrl='<%#GetProductVariantImageUrl((ShoppingCartItem)Container.DataItem)%>'
                                AlternateText="Product picture" />
                        </td>
                        <%} %>
                        <td class="product">
                            <a href='<%#GetProductURL((ShoppingCartItem)Container.DataItem)%>'>
                                <%#Server.HtmlEncode(GetProductVariantName((ShoppingCartItem)Container.DataItem))%></a>
                            <%#GetAttributeDescription((ShoppingCartItem)Container.DataItem)%>
                            <asp:Panel runat="server" ID="pnlWarnings" CssClass="WarningBox" EnableViewState="false"
                                Visible="false">
                                <asp:Label runat="server" ID="lblWarning" CssClass="WarningText" EnableViewState="false"
                                    Visible="false"></asp:Label>
                            </asp:Panel>
                        </td>
                        <td width="20%">
                            <%#GetShoppingCartItemUnitPriceString((ShoppingCartItem)Container.DataItem)%>
                        </td>
                        <td width="10%">
                            <%if (IsEditable)
                              { %>
                            <asp:TextBox ID="txtQuantity" size="4" runat="server" Text='<%# Eval("Quantity") %>'
                                SkinID="WishListQuantityText" />
                            <%}
                              else
                              { %>
                            <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("Quantity") %>' CssClass="Label" />
                            <%} %>
                        </td>
                        <td width="20%" class="end">
                            <%#GetShoppingCartItemSubTotalString((ShoppingCartItem)Container.DataItem)%>
                            <asp:Label ID="lblShoppingCartItemID" runat="server" Visible="false" Text='<%# Eval("ShoppingCartItemID") %>' />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>
    <%if (IsEditable)
      { %>
    <asp:Button ID="btnUpdate" OnClick="btnUpdate_Click" runat="server" Text="<% $NopResources:Wishlist.UpdateWishlist %>"
        SkinID="UpdateWishListButton" />
    <%} %>
</asp:Panel>
