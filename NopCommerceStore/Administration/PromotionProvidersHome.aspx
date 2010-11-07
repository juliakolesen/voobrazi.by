<%@ Page Language="C#" MasterPageFile="~/Administration/main.master" AutoEventWireup="true"
    CodeBehind="PromotionProvidersHome.aspx.cs" Inherits="NopSolutions.NopCommerce.Web.Administration.PromotionProvidersHome" %>

<%@ Register Src="Modules/PromotionProvidersHome.ascx" TagName="PromotionProvidersHome"
    TagPrefix="nopCommerce" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cph1">
    <nopCommerce:PromotionProvidersHome ID="ctrlPromotionProvidersHome" runat="server" />
</asp:Content>
