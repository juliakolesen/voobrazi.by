<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.CheckoutConfirmControl"
	CodeBehind="CheckoutConfirm.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="OrderSummary" Src="~/Modules/OrderSummary.ascx" %>
<div class="CheckoutPage">
	<%--	<div class="title">
		<%=GetLocaleResourceString("Checkout.ConfirmYourOrder")%>
	</div>--%>
	<asp:Panel runat="server" ID="pnlConfirmType">
		Уведомление о доставке:<br />
		<br />
		<div style="padding-left: 10px;">
			<asp:CheckBox runat="server" ID="chbByMail" Text="Письмом на Е-Mail" TextAlign="Right" /><br />
			<asp:CheckBox runat="server" ID="chbSMS" Text="SMS сообщение" TextAlign="Right" />
		</div>
		<br />
		Комментарии (дата, время доставки и прочее):
		<br />
		<div style="padding-left: 10px;">
			<br />
			<asp:TextBox runat="server" TextMode="MultiLine" Height="150" Width="300" ID="tbComments" />
		</div>
	</asp:Panel>
	<div class="clear">
	</div>
	<div class="CheckoutData">
		<div class="ConfirmOrder">
			<div class="SelectButton">
				<br />
				<asp:Button runat="server" ID="btnNextStep" Text="<% $NopResources:Checkout.ConfirmButton %>"
					OnClick="btnNextStep_Click" SkinID="ConfirmOrderNextStepButton" />
			</div>
			<div class="clear">
			</div>
			<div class="ErrorBlock">
				<div class="messageError">
					<asp:Literal runat="server" ID="lError" EnableViewState="false"></asp:Literal>
				</div>
			</div>
		</div>
		<div class="clear">
		</div>
		<br />
		<div class="OrderSummaryTitle">
			<%=GetLocaleResourceString("Checkout.OrderSummary")%>
		</div>
		<div class="clear">
		</div>
		<div class="OrderSummaryBody">
			<nopCommerce:OrderSummary ID="OrderSummaryControl" runat="server" IsShoppingCart="false">
			</nopCommerce:OrderSummary>
		</div>
	</div>
</div>
