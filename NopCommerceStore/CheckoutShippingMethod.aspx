<%@ Page Language="C#" MasterPageFile="~/MasterPages/OneColumn.master" AutoEventWireup="true"
    Inherits="NopSolutions.NopCommerce.Web.CheckoutShippingMethodPage" CodeBehind="CheckoutShippingMethod.aspx.cs" %>

<%@ Register Src="~/Modules/OrderProgress.ascx" TagName="OrderProgress" TagPrefix="nopCommerce" %>
<%@ Register TagPrefix="nopCommerce" TagName="CheckoutShippingMethod" Src="~/Modules/CheckoutShippingMethod.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="Server">
    <nopCommerce:OrderProgress ID="OrderProgressControl" runat="server" OrderProgressStep="Shipping">
    </nopCommerce:OrderProgress>
    <nopCommerce:CheckoutShippingMethod ID="ctrlCheckoutShippingMethod" runat="server" />
</asp:Content>
