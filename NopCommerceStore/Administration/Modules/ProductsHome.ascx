<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.ProductsHomeControl"
    CodeBehind="ProductsHome.ascx.cs" %>
<div class="section-title">
    <img src="Common/ico-catalog.png" alt="<%=GetLocaleResourceString("Admin.ProductsHome.ProductsHome")%>" />
    <%=GetLocaleResourceString("Admin.ProductsHome.ProductsHome")%>
</div>
<div class="homepage">
    <div class="intro">
        <p>
            <%=GetLocaleResourceString("Admin.ProductsHome.intro")%>
        </p>
    </div>
    <div class="options">
        <ul>
            <li>
                <div class="title">
                    <a href="Products.aspx" title="<%=GetLocaleResourceString("Admin.ProductsHome.Products.TitleDescription")%>">
                        <%=GetLocaleResourceString("Admin.ProductsHome.Products.Title")%></a>
                </div>
                <div class="description">
                    <p>
                        <%=GetLocaleResourceString("Admin.ProductsHome.Products.Description1")%>
                    </p>
                    <p>
                        <%=GetLocaleResourceString("Admin.ProductsHome.Products.Description2")%>
                    </p>
                </div>
            </li>
            <li>
                <div class="title">
                    <a href="ProductReviews.aspx" title="<%=GetLocaleResourceString("Admin.ProductsHome.ProductReviews.TitleDescription")%>">
                        <%=GetLocaleResourceString("Admin.ProductsHome.ProductReviews.Title")%></a>
                </div>
                <div class="description">
                    <p>
                        <%=GetLocaleResourceString("Admin.ProductsHome.ProductReviews.Description")%>
                    </p>
                </div>
            </li>
            <li>
                <div class="title">
                    <a href="ProductVariantsLowStock.aspx" title="<%=GetLocaleResourceString("Admin.ProductsHome.ProductVariantsLowStock.TitleDescription")%>">
                        <%=GetLocaleResourceString("Admin.ProductsHome.ProductVariantsLowStock.Title")%></a>
                </div>
                <div class="description">
                    <p>
                        <%=GetLocaleResourceString("Admin.ProductsHome.ProductVariantsLowStock.Description")%>
                    </p>
                </div>
            </li>
        </ul>
    </div>
</div>
