<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.CheckoutShippingMethodControl"
    CodeBehind="CheckoutShippingMethod.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="OrderSummary" Src="~/Modules/OrderSummary.ascx" %>
<div class="CheckoutPage">
    <div class="title">
        <%=GetLocaleResourceString("Checkout.SelectShippingMethod")%>
    </div>
    <div class="clear">
    </div>
    <div class="CheckoutData">
        <asp:Panel runat="server" ID="phSelectShippingMethod">
            <div class="ShippingOptions">
                <asp:DataList runat="server" ID="dlShippingOptions" DataKeyField="Name">
                    <ItemTemplate>
                        <div class="ShippingOptionItem">
                            <div class="OptionName">
                                <nopCommerce:GlobalRadioButton runat="server" ID="rdShippingOption" Checked="false"
                                    GroupName="shippingOptionGroup" />
                                <%#Server.HtmlEncode(Eval("Name").ToString()) %>
                                <%#Server.HtmlEncode(FormatShippingOption(((ShippingOption)Container.DataItem)))%>
                            </div>
                            <div class="OptionDescription">
                                <%#Eval("Description") %>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:DataList>
                <div class="clear">
                </div>
                <div class="SelectButton">
                    <asp:Button runat="server" ID="btnNextStep" Text="<% $NopResources:Checkout.NextButton %>"
                        OnClick="btnNextStep_Click" SkinID="ShippingMethodNextStepButton" />
                </div>
            </div>
            <div class="clear">
            </div>
            <div class="ErrorBlock">
                <div class="messageError">
                    <asp:Literal runat="server" ID="lError" EnableViewState="false"></asp:Literal>
                </div>
            </div>
        </asp:Panel>
        <div class="clear">
        </div>
        <asp:Panel runat="server" ID="phShippingIsNotAllowed" Visible="false">
            <div class="ShippingNotAllowed">
                <%=GetLocaleResourceString("Checkout.ShippingIsNotAllowed")%>
            </div>
        </asp:Panel>
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
