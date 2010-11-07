<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.InfoBlockControl"
    CodeBehind="InfoBlock.ascx.cs" %>
<div class="infoblock-navigation">
    <div class="title">
        <%=GetLocaleResourceString("Content.Information")%>
    </div>
    <div class="clear">
    </div>
    <div class="listbox">
        <ul>
            <li><a href="<%=Page.ResolveUrl("~/ContactUs.aspx")%>">
                <%=GetLocaleResourceString("ContactUs.ContactUs")%></a> </li>
            <li><a href="<%=Page.ResolveUrl("~/AboutUs.aspx")%>">
                <%=GetLocaleResourceString("Content.AboutUs")%></a></li>
            <% if (BlogManager.BlogEnabled)
               { %>
            <li><a href="<%=Page.ResolveUrl("~/Blog.aspx")%>">
                <%=GetLocaleResourceString("Blog.Blog")%></a></li>
            <%} %>
            <% if (ForumManager.ForumsEnabled)
               { %>
            <li><a href="<%= SEOHelper.GetForumMainURL()%> ">
                <%=GetLocaleResourceString("Forum.Forums")%></a></li>
            <%} %>
            <% if (ProductManager.RecentlyAddedProductsEnabled)
               { %>
            <li><a href="<%=Page.ResolveUrl("~/RecentlyAddedProducts.aspx")%>">
                <%=GetLocaleResourceString("Products.NewProducts")%></a></li>
            <%} %>
            <% if (ProductManager.RecentlyViewedProductsEnabled)
               { %>
            <li><a href="<%=Page.ResolveUrl("~/RecentlyViewedProducts.aspx")%>">
                <%=GetLocaleResourceString("Products.RecentlyViewedProducts")%></a></li>
            <%} %>
            <% if (ProductManager.CompareProductsEnabled)
               { %>
            <li><a href="<%=Page.ResolveUrl("~/CompareProducts.aspx")%>">
                <%=GetLocaleResourceString("Products.CompareProductsList")%></a></li>
            <%} %>
            <li><a href="<%=Page.ResolveUrl("~/ShippingInfo.aspx")%>">
                <%=GetLocaleResourceString("Content.Shipping&Returns")%></a></li>
            <li><a href="<%=Page.ResolveUrl("~/PrivacyInfo.aspx")%>">
                <%=GetLocaleResourceString("Content.PrivacyNotice")%></a></li>
            <li><a href="<%=Page.ResolveUrl("~/ConditionsInfo.aspx")%>">
                <%=GetLocaleResourceString("Content.ConditionsOfUse")%></a></li>
        </ul>
    </div>
</div>
