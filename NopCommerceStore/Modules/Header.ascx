<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.HeaderControl"
	CodeBehind="Header.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="CurrencySelector" Src="~/Modules/CurrencySelector.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="LanguageSelector" Src="~/Modules/LanguageSelector.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="TaxDisplayTypeSelector" Src="~/Modules/TaxDisplayTypeSelector.ascx" %>
<asp:LoginView ID="topLoginView" runat="server">
	<AnonymousTemplate>
		<a href="<%=Page.ResolveUrl("~/Login.aspx")%>" title="����">����</a>&nbsp;&nbsp;&nbsp;/&nbsp;&nbsp;<a
			href="<%=Page.ResolveUrl("~/Register.aspx")%>" title="�����������">�����������</a><br /><br />
		<div class="logo">
			<a href="/" title="�����������">
				<img src="<%=Page.ResolveUrl("~/images/ff_images/logo.gif")%>" alt="" width="299"
					height="90" /></a></div>
		<a href="<%=Page.ResolveUrl("~/ShoppingCart.aspx")%>">
			<img src="<%=Page.ResolveUrl("~/images/ff_images/recycle.jpg")%>" alt="" align="absbottom"
				title="�������" /> ������� (<span class="amount"><%=ShoppingCartManager.GetCurrentShoppingCart(ShoppingCartTypeEnum.ShoppingCart).Count%></span>
		<span class="amount_text">�������</span>)</a>
	</AnonymousTemplate>
	<LoggedInTemplate>
		<div class="logo">
			<a href="/" title="�����������">
				<img src="<%=Page.ResolveUrl("~/images/ff_images/logo.gif")%>" alt="" width="299"
					height="90" /></a></div>
		����� ����������, <span class="name">
			<%=Page.User.Identity.Name %></span>&nbsp;&nbsp;&nbsp;/&nbsp;&nbsp;<a href="<%=Page.ResolveUrl("~/Logout.aspx")%>" title="�����">�����</a><br />
		<a href="<%=Page.ResolveUrl("~/ShoppingCart.aspx")%>">
			<img src="<%=Page.ResolveUrl("~/images/ff_images/recycle.jpg")%>" alt="" align="absbottom"
				title="�������" /> ������� (<span class="amount"><%=ShoppingCartManager.GetCurrentShoppingCart(ShoppingCartTypeEnum.ShoppingCart).Count%></span>
		<span class="amount_text">�������</span>)</a>
	</LoggedInTemplate>
</asp:LoginView>
<% if (Page.User.IsInRole("Admin"))
   { %>
<li><a href="<%=Page.ResolveUrl("~/Administration/Default.aspx")%>" class="ico-admin">
	<%=GetLocaleResourceString("Account.Administration")%></a> </li>
<%} %>
