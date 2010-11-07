<%@ Page Language="C#" MasterPageFile="~/MasterPages/OneColumn.master" AutoEventWireup="true"
    Inherits="NopSolutions.NopCommerce.Web.CheckoutPaymentMethodPage" CodeBehind="CheckoutPaymentMethod.aspx.cs" %>

<%@ Register Src="~/Modules/OrderProgress.ascx" TagName="OrderProgress" TagPrefix="nopCommerce" %>
<%@ Register TagPrefix="nopCommerce" TagName="CheckoutPaymentMethod" Src="~/Modules/CheckoutPaymentMethod.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="Server">
    <nopCommerce:OrderProgress ID="OrderProgressControl" runat="server" OrderProgressStep="Payment" />
    <nopCommerce:CheckoutPaymentMethod ID="ctrlCheckoutPaymentMethod" runat="server" />
</asp:Content>
