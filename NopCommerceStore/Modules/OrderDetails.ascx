<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.OrderDetailsControl"
	CodeBehind="OrderDetails.ascx.cs" %>
<div class="OrderDetails">
	<table width="100%">
		<tr>
			<td style="text-align: left;">
				<div class="title">
					<%=GetLocaleResourceString("Order.OrderInformation")%>
				</div>
			</td>
			<td runat="server" visible="false" style="text-align: right;">
				<%if (!this.HidePrintButton)
	  { %>
				<asp:HyperLink runat="server" ID="lnkPrint" Text="Распечатать" Target="_blank" CssClass="orderdetailsprintbutton" />
				<%} %>
			</td>
		</tr>
	</table>
	<div class="clear">
	</div>
	<div class="info">
		<div class="OrderOverview">
			<table width="100%" cellspacing="0" cellpadding="2" border="0">
				<tbody>
					<tr>
						<td colspan="2">
							<b>
								<%=GetLocaleResourceString("Order.Order#")%><asp:Label ID="lblOrderID" runat="server" />
							</b>
						</td>
					</tr>
					<tr>
						<td class="smallText">
							<%=GetLocaleResourceString("Order.OrderDate")%>:
							<asp:Label ID="lblCreatedOn" runat="server"></asp:Label>
						</td>
						<td align="right" class="smallText">
							<%=GetLocaleResourceString("Order.OrderTotal")%>:
							<asp:Label ID="lblOrderTotal" runat="server"></asp:Label>
						</td>
					</tr>
					<tr>
						<td class="smallText">
							<%=GetLocaleResourceString("Order.OrderStatus")%>
							<asp:Label ID="lblOrderStatus" runat="server"></asp:Label>
						</td>
						<td colspan="2">
						</td>
					</tr>
				</tbody>
			</table>
		</div>
		<div class="clear">
		</div>
		<asp:Panel CssClass="ShippingBox" runat="server" ID="pnlShipping">
			<table width="100%" border="0">
				<tbody>
					<tr>
						<td>
							<b>
								<%=GetLocaleResourceString("Order.ShippingAddress")%></b>
						</td>
					</tr>
					<tr>
						<td>
							<asp:Literal ID="lShippingFirstName" runat="server"></asp:Literal>
							<asp:Literal ID="lShippingLastName" runat="server"></asp:Literal><br />
							<div>
								<%=GetLocaleResourceString("Order.Email")%>:
								<asp:Literal ID="lShippingEmail" runat="server"></asp:Literal></div>
							<div>
								<%=GetLocaleResourceString("Order.Phone")%>:
								<asp:Literal ID="lShippingPhoneNumber" runat="server"></asp:Literal></div>
							<div>
								<%=GetLocaleResourceString("Order.Fax")%>:
								<asp:Literal ID="lShippingFaxNumber" runat="server"></asp:Literal></div>
							<asp:Panel ID="pnlShippingCompany" runat="server">
								<asp:Literal ID="lShippingCompany" runat="server"></asp:Literal></asp:Panel>
							<div>
								<asp:Literal ID="lShippingAddress1" runat="server"></asp:Literal></div>
							<asp:Panel ID="pnlShippingAddress2" runat="server">
								<asp:Literal ID="lShippingAddress2" runat="server"></asp:Literal></asp:Panel>
							<div>
								<asp:Literal ID="lShippingCity" runat="server"></asp:Literal>
								<asp:Literal ID="lShippingStateProvince" Visible="false" runat="server"></asp:Literal>
								<asp:Literal ID="lShippingZipPostalCode" runat="server"></asp:Literal></div>
							<asp:Panel ID="pnlShippingCountry" Visible="false" runat="server">
								<asp:Literal ID="lShippingCountry" Visible="false" runat="server"></asp:Literal></asp:Panel>
						</td>
					</tr>
					<tr>
						<td>
							<b>
								<%=GetLocaleResourceString("Order.ShippingMethod")%></b>
						</td>
					</tr>
					<tr>
						<td>
							<asp:Label ID="lblShippingMethod" runat="server"></asp:Label>
						</td>
					</tr>
					<tr>
						<td>
							<b>
								<%=GetLocaleResourceString("Order.ShippedOn")%></b>
						</td>
					</tr>
					<tr>
						<td>
							<asp:Label ID="lblShippedDate" runat="server"></asp:Label>
						</td>
					</tr>
					<tr>
						<td>
							<b>
								<%=GetLocaleResourceString("Order.Weight")%></b>
						</td>
					</tr>
					<tr>
						<td>
							<asp:Label ID="lblOrderWeight" runat="server"></asp:Label>
						</td>
					</tr>
				</tbody>
			</table>
		</asp:Panel>
		<div class="clear">
		</div>
		<div class="SectionTitle">
			<%=GetLocaleResourceString("Order.BillingInformation")%>
		</div>
		<div class="BillingBox">
			<table width="100%" border="0">
				<tbody>
					<tr>
						<td>
							<table width="100%" cellspacing="3" cellpadding="2" border="0">
								<tbody>
									<tr>
										<td>
											<b>
												<%=GetLocaleResourceString("Order.BillingAddress")%></b>
										</td>
									</tr>
									<tr>
										<td>
											<asp:Literal ID="lBillingFirstName" runat="server"></asp:Literal>
											<asp:Literal ID="lBillingLastName" runat="server"></asp:Literal><br />
											<div>
												<%=GetLocaleResourceString("Order.Email")%>:
												<asp:Literal ID="lBillingEmail" runat="server"></asp:Literal></div>
											<div>
												<%=GetLocaleResourceString("Order.Phone")%>:
												<asp:Literal ID="lBillingPhoneNumber" runat="server"></asp:Literal></div>
											<div>
												<%=GetLocaleResourceString("Order.Fax")%>:
												<asp:Literal ID="lBillingFaxNumber" runat="server"></asp:Literal></div>
											<asp:Panel ID="pnlBillingCompany" runat="server">
												<asp:Literal ID="lBillingCompany" runat="server"></asp:Literal></asp:Panel>
											<div>
												<asp:Literal ID="lBillingAddress1" runat="server"></asp:Literal></div>
											<asp:Panel ID="pnlBillingAddress2" runat="server">
												<asp:Literal ID="lBillingAddress2" runat="server"></asp:Literal></asp:Panel>
											<div>
												<asp:Literal ID="lBillingCity" runat="server"></asp:Literal>
												<asp:Literal ID="lBillingStateProvince" Visible="false" runat="server"></asp:Literal>
												<asp:Literal ID="lBillingZipPostalCode" runat="server"></asp:Literal></div>
											<asp:Panel ID="pnlBillingCountry" Visible="false" runat="server">
												<asp:Literal ID="lBillingCountry" Visible="false" runat="server"></asp:Literal></asp:Panel>
										</td>
									</tr>
									<tr runat="server" visible="false">
										<td>
											<b>
												<%=GetLocaleResourceString("Order.PaymentMethod")%></b>
										</td>
									</tr>
									<tr  runat="server" visible="false">
										<td>
											<asp:Literal runat="server" ID="lPaymentMethod"></asp:Literal>
										</td>
									</tr>
								</tbody>
							</table>
						</td>
					</tr>
					<tr>
						<td align="right">
							<table width="100%" cellspacing="0" cellpadding="2" border="0">
								<tbody>
									<tr>
										<td width="100%" align="right">
											<b>
												<%=GetLocaleResourceString("Order.Sub-Total")%>:</b>
										</td>
										<td align="right">
											<span style="white-space: nowrap;">
												<asp:Label ID="lblOrderSubtotal" runat="server"></asp:Label>
											</span>
										</td>
									</tr>
									<tr>
										<td runat="server" width="100%" align="right">
											<b>
												<%=GetLocaleResourceString("Order.Shipping")%>:</b>
										</td>
										<td align="right">
											<span style="white-space: nowrap;">
												<asp:Label ID="lblOrderShipping" runat="server"></asp:Label>
											</span>
										</td>
									</tr>
									<asp:Panel runat="server" Visible="false">
										<asp:PlaceHolder Visible="false" runat="server" ID="phPaymentMethodAdditionalFee">
											<tr>
												<td width="100%" align="right">
													<b>
														<%=GetLocaleResourceString("Order.PaymentMethodAdditionalFee")%>:</b>
												</td>
												<td align="right">
													<span style="white-space: nowrap;">
														<asp:Label ID="lblPaymentMethodAdditionalFee" runat="server"></asp:Label>
													</span>
												</td>
											</tr>
										</asp:PlaceHolder>
										<asp:PlaceHolder Visible="false" runat="server" ID="phTaxTotal">
											<tr>
												<td width="100%" align="right">
													<b>
														<%=GetLocaleResourceString("Order.Tax")%>:</b>
												</td>
												<td align="right">
													<span style="white-space: nowrap;">
														<asp:Label ID="lblOrderTax" runat="server"></asp:Label>
													</span>
												</td>
											</tr>
										</asp:PlaceHolder>
									</asp:Panel>
									<tr>
										<td width="100%" align="right">
											<b>
												<%=GetLocaleResourceString("Order.OrderTotal")%>:</b>
										</td>
										<td align="right">
											<b><span style="white-space: nowrap;">
												<asp:Label ID="lblOrderTotal2" runat="server"></asp:Label>
											</span></b>
										</td>
									</tr>
								</tbody>
							</table>
						</td>
					</tr>
				</tbody>
			</table>
		</div>
		<div class="clear">
		</div>
		<div class="SectionTitle">
			<%=GetLocaleResourceString("Order.Product(s)")%></div>
		<div class="clear">
		</div>
		<div class="ProductsBox">
			<asp:GridView ID="gvOrderProductVariants" runat="server" AutoGenerateColumns="False"
				Width="100%">
				<Columns>
					<asp:BoundField DataField="OrderProductVariantID" HeaderText="OrderProductVariantID"
						Visible="False"></asp:BoundField>
					<asp:TemplateField HeaderText="<% $NopResources:Order.ProductsGrid.Name %>">
						<ItemTemplate>
							<div style="padding-left: 10px; padding-right: 10px; text-align: left;">
								<em><a href='<%#GetProductURL(Convert.ToInt32(Eval("ProductVariantID")))%>'>
									<%#Server.HtmlEncode(GetProductVariantName(Convert.ToInt32(Eval("ProductVariantID"))))%></a></em>
								<%#Eval("AttributeDescription")%>
							</div>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="<% $NopResources:Order.ProductsGrid.Download %>" HeaderStyle-HorizontalAlign="Center"
						ItemStyle-HorizontalAlign="Center">
						<ItemTemplate>
							<div style="padding-left: 10px; padding-right: 10px; text-align: left;">
								<%#GetDownloadURL(Container.DataItem as OrderProductVariant)%>
							</div>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="<% $NopResources:Order.ProductsGrid.Price %>" HeaderStyle-HorizontalAlign="Center"
						ItemStyle-HorizontalAlign="Center">
						<ItemTemplate>
							<div style="padding-left: 10px; padding-right: 10px; text-align: left;">
								<%#GetProductVariantUnitPrice(Container.DataItem as OrderProductVariant)%>
							</div>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="Quantity" HeaderText="<% $NopResources:Order.ProductsGrid.Quantity %>"
						HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
					<asp:TemplateField HeaderText="<% $NopResources:Order.ProductsGrid.Total %>" HeaderStyle-HorizontalAlign="Center"
						ItemStyle-HorizontalAlign="Center">
						<ItemTemplate>
							<div style="padding-left: 10px; padding-right: 10px; text-align: left;">
								<%#GetProductVariantSubTotal(Container.DataItem as OrderProductVariant)%>
							</div>
						</ItemTemplate>
					</asp:TemplateField>
				</Columns>
			</asp:GridView>
		</div>
	</div>
</div>
