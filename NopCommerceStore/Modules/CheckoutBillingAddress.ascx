<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.CheckoutBillingAddressControl"
	CodeBehind="CheckoutBillingAddress.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="SimpleTextBox" Src="~/Modules/SimpleTextBox.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="OrderSummary" Src="~/Modules/OrderSummary.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="AddressEdit" Src="~/Modules/AddressEdit.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="AddressDisplay" Src="~/Modules/AddressDisplay.ascx" %>
<div class="CheckoutPage">
	<%--    <div class="title">
        <%=GetLocaleResourceString("Checkout.BillingAddress")%>
    </div>--%>
	<div class="clear">
	</div>
	<table width="100%">
		<tr>
			<td colspan="2">
				<asp:RadioButton runat="server" ID="rbCourier" Checked="true" oncheckedchanged="rbCourier_CheckedChanged" AutoPostBack="true" GroupName="delivery" />
				Доставка курьером &nbsp;&nbsp;&nbsp;&nbsp;
				<asp:RadioButton runat="server" ID="rbSelf" GroupName="delivery" 
					oncheckedchanged="rbSelf_CheckedChanged" AutoPostBack="true" /><a target="_blank"
					href="/Contacts.aspx">Самовывоз из салона</a>
			</td>
		</tr>
		<tr>
			<td width="50%">
				<div class="CheckoutData">
					<asp:Panel runat="server" ID="pnlSelectBillingAddress">
						<div class="SelectAddressTitle">
							Использовать текущие сведения о заказчике:
						</div>
						<asp:DataList ID="dlAddress" runat="server" RepeatColumns="1" RepeatDirection="Horizontal"
							RepeatLayout="Table" ItemStyle-CssClass="ItemBox">
							<ItemTemplate>
								<nopCommerce:AddressDisplay ID="adAddress" runat="server" Address='<%# Container.DataItem %>'
									ShowDeleteButton="false" ShowEditButton="false"></nopCommerce:AddressDisplay>
								<br />
								<asp:Button runat="server" CommandName="Select" ID="btnSelect" Text='Использовать текущие сведения'
									OnCommand="btnSelect_Command" ValidationGroup="SelectBillingAddress" CommandArgument='<%# Eval("AddressID") %>'
									SkinID="SelectBillingAddressButton" />
							</ItemTemplate>
						</asp:DataList>
					</asp:Panel>
					<br />
					<div class="clear">
					</div>
					<div class="EnterAddressTitle">
						<asp:Label runat="server" Visible="false" ID="lEnterBillingAddress"></asp:Label>
						Или ввести новые сведения о заказчике:
					</div>
					<br />
					<div class="clear">
					</div>
					<div class="EnterAddress">
						<div runat="server" id="pnlTheSameAsShippingAddress" class="TheSameAddress">
							<asp:Button runat="server" ID="btnTheSameAsShippingAddress" Text="<% $NopResources:Checkout.BillingAddressTheSameAsShippingAddress %>"
								CausesValidation="false" OnClick="btnTheSameAsShippingAddress_Click" SkinID="SameAsShippingAddressButton" />
						</div>
						<div class="EnterAddressBody">
							<nopCommerce:AddressEdit ID="AddressDisplayCtrl" runat="server" IsNew="true" IsBillingAddress="false" />
						</div>
						<div class="clear">
						</div>
						<div class="Button">
							<br />
							<asp:Button runat="server" ID="btnNextStep" Text="Использовать новые сведения" OnClick="btnNextStep_Click"
								SkinID="NewAddressNextStepButton" />
						</div>
						<div class="ErrorBlock">
							<div class="messageError">
								<asp:Literal runat="server" ID="lError" EnableViewState="false"></asp:Literal>
							</div>
						</div>
					</div>
					<div class="clear">
					</div>
					<div class="OrderSummaryTitle">
						<h3>
							<%=GetLocaleResourceString("Checkout.OrderSummary")%></h3>
					</div>
					<div class="clear">
					</div>
					<div class="OrderSummaryBody">
						<nopCommerce:OrderSummary ID="OrderSummaryControl" runat="server" IsShoppingCart="false">
						</nopCommerce:OrderSummary>
					</div>
				</div>
			</td>
			<td valign="top" width="50%">
				Если Получатель заказа не Заказчик (дарите своей любимой, поздравляете с юбилеем
				друга и т.д.) -- заполните форму Получателя (данные, которые будут использоваться
				нашей службой доставки):
				<table>
					<tr>
						<td>
							<%=GetLocaleResourceString("Address.FirstName")%>:
						</td>
						<td>
							<asp:TextBox runat="server" ID="txtFirstName" />
						</td>
					</tr>
					<tr>
						<td>
							<%=GetLocaleResourceString("Address.LastName")%>:
						</td>
						<td>
							<asp:TextBox runat="server" ID="txtLastName" />
						</td>
					</tr>
					<tr>
						<td>
							<%=GetLocaleResourceString("Address.PhoneNumber")%>:
						</td>
						<td>
							<asp:TextBox runat="server" ID="txtPhoneNumber" />
						</td>
					</tr>
					<tr>
						<td>
							<%=GetLocaleResourceString("Address.Address1")%>:
						</td>
						<td>
							<asp:TextBox runat="server" ID="txtAddress1" />
						</td>
					</tr>
					<tr>
						<td valign="top">
							Дополнительно:
						</td>
						<td>
							<asp:TextBox runat="server" ID="tbAdditionalInfo" TextMode="MultiLine" Height="100"
								Width="100%" />
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</div>
