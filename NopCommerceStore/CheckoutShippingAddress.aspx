<%@ Page Language="C#" MasterPageFile="~/MasterPages/OneColumn.master" AutoEventWireup="true"
    Inherits="NopSolutions.NopCommerce.Web.CheckoutShippingAddressPage" CodeBehind="CheckoutShippingAddress.aspx.cs" %>

<%@ Register TagPrefix="nopCommerce" TagName="OrderProgress" Src="~/Modules/OrderProgress.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="CheckoutShippingAddress" Src="~/Modules/CheckoutShippingAddress.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="Server">
    <nopCommerce:OrderProgress ID="OrderProgressControl" runat="server" OrderProgressStep="Address" />
    <nopCommerce:CheckoutShippingAddress ID="ctrlCheckoutShippingAddress" runat="server" />
</asp:Content>
