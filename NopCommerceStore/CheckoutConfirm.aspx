<%@ Page Language="C#" MasterPageFile="~/MasterPages/OneColumn.master" AutoEventWireup="true"
	Inherits="NopSolutions.NopCommerce.Web.CheckoutConfirmPage" CodeBehind="CheckoutConfirm.aspx.cs" %>

<%@ Register Src="~/Modules/OrderProgress.ascx" TagName="OrderProgress" TagPrefix="nopCommerce" %>
<%@ Register TagPrefix="nopCommerce" TagName="CheckoutConfirm" Src="~/Modules/CheckoutConfirm.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="Server">
	<nopCommerce:OrderProgress ID="OrderProgressControl" Visible="false" runat="server"
		OrderProgressStep="Confirm" />
	<nopCommerce:CheckoutConfirm ID="ctrlCheckoutConfirm" runat="server" />
</asp:Content>
