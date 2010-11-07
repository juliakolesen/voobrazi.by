<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.CheckoutPaymentInfoControl"
    CodeBehind="CheckoutPaymentInfo.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="OrderSummary" Src="~/Modules/OrderSummary.ascx" %>
<div class="CheckoutPage">
    <div class="title">
        <%=GetLocaleResourceString("Checkout.PaymentInfo")%>
    </div>
    <div class="clear">
    </div>
    <div class="CheckoutData">
        <div class="PaymentInfo">
            <div class="Body">
                <asp:PlaceHolder runat="server" ID="PaymentInfoPlaceHolder"></asp:PlaceHolder>
            </div>
            <div class="clear">
            </div>
            <div class="SelectButton">
                <asp:Button runat="server" ID="btnNextStep" Text="<% $NopResources:Checkout.NextButton %>"
                    OnClick="btnNextStep_Click" SkinID="PaymentInfoNextStepButton" />
            </div>
        </div>
        <div class="clear">
        </div>
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
