<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.HeaderMenuControl"
	CodeBehind="HeaderMenu.ascx.cs" %>
<%--<%@ Register TagPrefix="nopCommerce" TagName="SearchBox" Src="~/Modules/SearchBox.ascx" %>--%>
<div class="menu">
	<table width="730" border="0" cellspacing="0" cellpadding="0" align="center" class="menu_tbl">
		<tr valign="middle">
			<td>
				<a class="about" title="О нас" href="<%=Page.ResolveUrl("~/Default.aspx")%>"></a>
			</td>
			<td>
				<img src="<%=Page.ResolveUrl("~/images/ff_images/menu/sep.gif")%>" alt="" width="32" height="32" />
			</td>
			<td>
				<a class="salon" title="О салоне" href="<%=Page.ResolveUrl("~/AboutSalon.aspx")%>"></a>
			</td>
			<td>
				<img src="<%=Page.ResolveUrl("~/images/ff_images/menu/sep.gif")%>" alt="" width="32" height="32" />
			</td>
			<td>
				<a class="clients" title="Корпоративным клиентам" href="<%=Page.ResolveUrl("~/ToCorporateClients.aspx")%>"></a>
			</td>
			<td>
				<img src="<%=Page.ResolveUrl("~/images/ff_images/menu/sep.gif")%>" alt="" width="32" height="32" />
			</td>
			<td>
				<a class="contacts" title="Контакты" href="<%=Page.ResolveUrl("~/Contacts.aspx")%>"></a>
			</td>
		</tr>
	</table>
</div>
<%--<div class="headermenu">
    <div class="searchbox">
        <nopCommerce:SearchBox runat="server" ID="ctrlSearchBox">
        </nopCommerce:SearchBox>
    </div>
    <ul>
        <li><a href="<%=Page.ResolveUrl("~/Default.aspx")%>">
            <%=GetLocaleResourceString("Content.HomePage")%></a> </li>
        <% if (ProductManager.RecentlyAddedProductsEnabled)
           { %>
        <li><a href="<%=Page.ResolveUrl("~/RecentlyAddedProducts.aspx")%>">
            <%=GetLocaleResourceString("Products.NewProducts")%></a> </li>
        <%} %>
        <li><a href="<%=Page.ResolveUrl("~/Search.aspx")%>">
            <%=GetLocaleResourceString("Search.Search")%></a> </li>
        <li><a href="<%=Page.ResolveUrl("~/Account.aspx")%>">
            <%=GetLocaleResourceString("Account.MyAccount")%></a> </li>
        <% if (BlogManager.BlogEnabled)
           { %>
        <li><a href="<%=Page.ResolveUrl("~/Blog.aspx")%>">
            <%=GetLocaleResourceString("Blog.Blog")%></a> </li>
        <%} %>
        <% if (ForumManager.ForumsEnabled)
           { %>
        <li><a href="<%= SEOHelper.GetForumMainURL()%> ">
            <%=GetLocaleResourceString("Forum.Forums")%></a></li>
        <%} %>
        <li><a href="<%=Page.ResolveUrl("~/ContactUs.aspx")%>">
            <%=GetLocaleResourceString("ContactUs.ContactUs")%></a> </li>
    </ul>
</div>--%>
