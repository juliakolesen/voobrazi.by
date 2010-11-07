<%@ Page Language="C#" MasterPageFile="~/MasterPages/OneColumn.master" AutoEventWireup="true"
	Inherits="NopSolutions.NopCommerce.Web.CheckoutCompletedPage" CodeBehind="CheckoutCompleted.aspx.cs" %>

<%@ Register TagPrefix="nopCommerce" TagName="CheckoutCompleted" Src="~/Modules/CheckoutCompleted.ascx" %>
<%@ Register Src="~/Modules/OrderProgress.ascx" TagName="OrderProgress" TagPrefix="nopCommerce" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="Server">
	<nopCommerce:OrderProgress ID="OrderProgressControl" Visible="false" runat="server"
		OrderProgressStep="Complete"></nopCommerce:OrderProgress>
	<nopCommerce:CheckoutCompleted ID="ctrlCheckoutCompleted" runat="server" />
</asp:Content>
