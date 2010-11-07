<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.FooterControl"
	CodeBehind="Footer.ascx.cs" %>
<style>
	footer_1 p, footer_2 p
	{
		margin: 0;
		padding: 0;
	}
</style>
<%@ Register TagPrefix="nopCommerce" TagName="Topic" Src="~/Modules/Topic.ascx" %>
<div class="footer_1">
	<nopCommerce:Topic ID="topicAboutUs" runat="server" TopicName="FooterLeft" />
</div>
<div class="footer_2">
	<nopCommerce:Topic ID="topic1" runat="server" TopicName="FooterRight" />
</div>
