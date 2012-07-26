<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.OrderSummaryControl"
	CodeBehind="OrderSummary.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="GoogleCheckoutButton" Src="~/Modules/GoogleCheckoutButton.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="OrderTotals" Src="~/Modules/OrderTotals.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="OrderIndividualOrders" Src="~/Modules/OrderIndividualOrders.ascx" %>

<asp:Panel class="OrderSummaryContent" runat="server" ID="pnlEmptyCart">
	<%=GetLocaleResourceString("ShoppingCart.CartIsEmpty")%>
</asp:Panel>
<asp:Panel class="OrderSummaryContent" runat="server" ID="pnlCart">
	<%if (IsShoppingCart)
   { %>
	<asp:Panel runat="server" ID="phCoupon" CssClass="CouponBox">
		<%=GetLocaleResourceString("ShoppingCart.CouponCode")%>
		<asp:TextBox ID="txtCouponCode" runat="server" Width="125px" />&nbsp;
		<asp:Button runat="server" ID="btnApplyCouponCode" OnClick="btnApplyCouponCode_Click"
			Text="<% $NopResources:ShoppingCart.ApplyCouponCode %>" SkinID="ApplyCouponCodeButton"
			CausesValidation="false" />
	</asp:Panel>
	<div class="clear">
	</div>
	<%} %>
    
    <%if (!IsShoppingCartEmpty)
	  { %>
	<table class="cart" id="cartTable">
		<tbody>
			<tr class="cart-header-row" id="cart_header_row">
				<%if (IsShoppingCart)
	  { %>
				<td width="10%">
					<%=GetLocaleResourceString("ShoppingCart.Remove")%>
				</td>
				<%} %>
				<%if (SettingManager.GetSettingValueBoolean("Display.ShowProductImagesOnShoppingCart"))
	  {%>
				<td class="picture">
				</td>
				<%} %>
				<td width="40%">
					<%=GetLocaleResourceString("ShoppingCart.Product(s)")%>
				</td>
				<td width="20%">
					<%=GetLocaleResourceString("ShoppingCart.UnitPrice")%>
				</td>
				<td width="10%">
					<%=GetLocaleResourceString("ShoppingCart.Quantity")%>
				</td>
				<td width="20%" class="end">
					<%=GetLocaleResourceString("ShoppingCart.ItemTotal")%>
				</td>
			</tr>
			<asp:Repeater ID="rptShoppingCart" runat="server">
				<ItemTemplate>
					<tr class="cart-item-row">
						<%if (IsShoppingCart)
		{ %>
						<td width="10%">
							<asp:CheckBox runat="server" ID="cbRemoveFromCart" />
						</td>
						<%} %>
						<%if (SettingManager.GetSettingValueBoolean("Display.ShowProductImagesOnShoppingCart"))
		{%>
						<td class="productpicture">
							<asp:Image ID="iProductVariantPicture" runat="server" ImageUrl='<%#GetProductVariantImageUrl((ShoppingCartItem)Container.DataItem)%>'
								AlternateText="Product picture" />
						</td>
						<%} %>
						<td width="40%" class="product">
							<a href='<%#GetProductURL((ShoppingCartItem)Container.DataItem)%>' title="View details">
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
							<%if (IsShoppingCart)
		 { %>
							<asp:TextBox ID="txtQuantity" size="4" runat="server" Text='<%# Eval("Quantity") %>'
								SkinID="ShoppingCartQuantityText" />
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
    <%} %>
	<div class="clear">
	</div>
	<div class="cart-footer">
    <br />
    <%if (!IsIndOrdersEmpty)
      { %>
    <nopCommerce:OrderIndividualOrders runat="server" ID="orderIndOrders"></nopCommerce:OrderIndividualOrders>
     <%} %>
		<%if (IsShoppingCart)
	{ %>
		<div class="clear">
		</div>
		<div class="Buttons">
			<div class="CommonButtons">
				<asp:Button ID="btnUpdate" OnClick="btnUpdate_Click" runat="server" Text="<% $NopResources:ShoppingCart.UpdateCart %>"
					SkinID="UpdateCartButton" />
				<asp:Button ID="btnContinueShopping" OnClick="btnContinueShopping_Click" runat="server"
					Text="Вернуться в магазин" SkinID="ContinueShoppingButton" />
				<asp:Button ID="btnCheckout" OnClick="btnCheckout_Click" runat="server" Text="<% $NopResources:ShoppingCart.Checkout %>"
					SkinID="CheckoutButton" />
				<br />
			</div>
			<div class="AddonButtons">
				<nopCommerce:GoogleCheckoutButton runat="server" ID="btnGoogleCheckoutButton"></nopCommerce:GoogleCheckoutButton>
			</div>
		</div>
		<%} %>
		<br />
		<nopCommerce:OrderTotals runat="server" ID="ctrlOrderTotals"></nopCommerce:OrderTotals>
	</div>
</asp:Panel>
