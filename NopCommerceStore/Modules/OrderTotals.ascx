<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.OrderTotalsControl"
	CodeBehind="OrderTotals.ascx.cs" %>
<div class="TotalInfo">
	<table class="cart-total">
		<tbody>
			<tr>
				<td class="cart_total_left">
					<strong>
						<%=GetLocaleResourceString("ShoppingCart.Sub-Total")%>:</strong>
				</td>
				<td class="cart_total_right">
					<span style="white-space: nowrap;">
						<asp:Label ID="lblSubTotalAmount" runat="server" CssClass="productPrice" />
					</span>
				</td>
			</tr>
			<asp:PlaceHolder runat="server" ID="phSubTotalDiscount" Visible="false">
				<tr>
					<td class="cart_total_left">
						<strong>
							<%=GetLocaleResourceString("ShoppingCart.Sub-TotalDiscount")%>:</strong>
					</td>
					<td class="cart_total_right">
						<span style="white-space: nowrap;">
							<asp:Label ID="lblSubTotalDiscountAmount" runat="server" CssClass="productPrice" />
						</span>
					</td>
				</tr>
			</asp:PlaceHolder>
			<tr>
				<td class="cart_total_left">
					<strong>
						<%=GetLocaleResourceString("ShoppingCart.Shipping")%>:</strong>
				</td>
				<td class="cart_total_right">
					<span style="white-space: nowrap;">
						<asp:Label ID="lblShippingAmount" runat="server" CssClass="productPrice" />
					</span>
				</td>
			</tr>
			<asp:PlaceHolder Visible="false" runat="server" ID="phPaymentMethodAdditionalFee">
				<tr>
					<td class="cart_total_left">
						<strong>
							<%=GetLocaleResourceString("ShoppingCart.PaymentMethodAdditionalFee")%>:</strong>
					</td>
					<td class="cart_total_right">
						<span style="white-space: nowrap;">
							<asp:Label ID="lblPaymentMethodAdditionalFee" runat="server" CssClass="productPrice" />
						</span>
					</td>
				</tr>
			</asp:PlaceHolder>
			<asp:PlaceHolder Visible="false" runat="server" ID="phTaxTotal">
				<tr>
					<td class="cart_total_left">
						<strong>
							<%=GetLocaleResourceString("ShoppingCart.Tax")%>:</strong>
					</td>
					<td class="cart_total_right">
						<span style="white-space: nowrap;">
							<asp:Label ID="lblTaxAmount" runat="server" CssClass="productPrice" />
						</span>
					</td>
				</tr>
			</asp:PlaceHolder>
			<tr>
				<td class="cart_total_left">
					<strong>
						<%=GetLocaleResourceString("ShoppingCart.OrderTotal")%>:</strong>
				</td>
				<td class="cart_total_right">
					<span style="white-space: nowrap;">
						<asp:Label ID="lblTotalAmount" runat="server" CssClass="productPrice" />
					</span>
				</td>
			</tr>
		</tbody>
	</table>
</div>
