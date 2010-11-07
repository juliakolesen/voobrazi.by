<%@ Page Language="C#" MasterPageFile="~/MasterPages/OneColumn.master" AutoEventWireup="true"
	Inherits="NopSolutions.NopCommerce.Web.CheckoutBillingAddressPage" CodeBehind="CheckoutBillingAddress.aspx.cs" %>

<%@ Register Src="~/Modules/OrderProgress.ascx" TagName="OrderProgress" TagPrefix="nopCommerce" %>
<%@ Register TagPrefix="nopCommerce" TagName="CheckoutBillingAddress" Src="~/Modules/CheckoutBillingAddress.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="Server">
	<nopCommerce:OrderProgress ID="OrderProgressControl" Visible="false" runat="server"
		OrderProgressStep="Address" />
	<nopCommerce:CheckoutBillingAddress ID="ctrlCheckoutBillingAddress" runat="server" />
</asp:Content>
