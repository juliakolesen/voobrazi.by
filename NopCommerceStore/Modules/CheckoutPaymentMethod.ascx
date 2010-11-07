<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.CheckoutPaymentMethodControl"
    CodeBehind="CheckoutPaymentMethod.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="OrderSummary" Src="~/Modules/OrderSummary.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="PaypalExpressButton" Src="~/Modules/PaypalExpressButton.ascx" %>
<div class="CheckoutPage">
    <div class="title">
        <%=GetLocaleResourceString("Checkout.SelectPaymentMethod")%>
    </div>
    <div class="clear">
    </div>
    <div class="CheckoutData">
        <nopCommerce:PaypalExpressButton runat="server" ID="btnPaypalExpressButton"></nopCommerce:PaypalExpressButton>
        <br />
        <asp:PlaceHolder runat="server" ID="phSelectPaymentMethod">
            <div class="PaymentMethods">
                <asp:DataList runat="server" ID="dlPaymentMethod" DataKeyField="PaymentMethodID">
                    <ItemTemplate>
                        <div class="PaymentMethodItem" style="text-align: left;">
                            <nopCommerce:GlobalRadioButton runat="server" ID="rdPaymentMethod" Checked="false"
                                GroupName="paymentMethodGroup" />
                            <%#Server.HtmlEncode(Eval("VisibleName").ToString()) %>
                            <%#Server.HtmlEncode(FormatPaymentMethodInfo(((PaymentMethod)Container.DataItem)))%>
                        </div>
                    </ItemTemplate>
                </asp:DataList>
                <div class="clear">
                </div>
                <div class="SelectButton">
                    <asp:Button runat="server" ID="btnNextStep" Text="<% $NopResources:Checkout.NextButton %>"
                        OnClick="btnNextStep_Click" SkinID="PaymentMethodNextStepButton" />
                </div>
            </div>
        </asp:PlaceHolder>
        <div class="clear">
        </div>
        <asp:PlaceHolder runat="server" ID="phNoPaymentMethods" Visible="false">
            <div class="NoPaymentMethods">
                <%=GetLocaleResourceString("Checkout.NoPaymentMethods")%>
            </div>
        </asp:PlaceHolder>
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
