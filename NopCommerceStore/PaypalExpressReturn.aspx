<%@ Page Language="C#" MasterPageFile="~/MasterPages/TwoColumn.master" AutoEventWireup="true"
    Inherits="NopSolutions.NopCommerce.Web.PaypalExpressReturnPage" CodeBehind="PaypalExpressReturn.aspx.cs" %>

<%@ Register TagPrefix="nopCommerce" TagName="OrderSummary" Src="~/Modules/OrderSummary.ascx" %>
<%@ Register Src="~/Modules/OrderProgress.ascx" TagName="OrderProgress" TagPrefix="nopCommerce" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="Server">
    <nopCommerce:OrderProgress ID="OrderProgressControl" runat="server" OrderProgressStep="Confirm" />
    <div class="CheckoutPage">
        <div class="title">
            <%=GetLocaleResourceString("Checkout.ConfirmYourOrder")%>
        </div>
    </div>
    <div class="clear">
    </div>
    <div class="CheckoutData">
        <div class="ConfirmOrder">
            <div class="SelectButton">
                <asp:Button runat="server" ID="btnNextStep" Text="<% $NopResources:Checkout.ConfirmButton %>" OnClick="btnNextStep_Click">
                </asp:Button>
            </div>
            <div class="clear">
            </div>
            <div class="ErrorBlock">
                <div class="messageError">
                    <asp:Literal runat="server" ID="lError"></asp:Literal>
                </div>
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
</asp:Content>
