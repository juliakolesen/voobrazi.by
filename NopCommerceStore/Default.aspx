<%@ Page Language="C#" MasterPageFile="~/MasterPages/OneColumnHome.master" AutoEventWireup="true"
    Inherits="NopSolutions.NopCommerce.Web.Default" Title="Интернет магазин по продаже цветов и комнатных растений с доставкой по минску. У нас всегда можно купить комнатные цветы, цветы в горшках. В нашем цветочном интернет магазине есть доставка цветов по минску." CodeBehind="Default.aspx.cs" %>

<%@ Register TagPrefix="nopCommerce" TagName="Topic" Src="~/Modules/Topic.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="Server">
	<style>
		.cont_top .titl { padding-top: 6px; }
	</style>

    <nopCommerce:Topic ID="topicAboutUs" runat="server" TopicName="AboutUs" />
</asp:Content>
