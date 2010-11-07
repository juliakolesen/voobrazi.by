<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.PromotionProvidersHomeControl"
    CodeBehind="PromotionProvidersHome.ascx.cs" %>
<div class="section-title">
    <img src="Common/ico-promotions.png" alt="<%=GetLocaleResourceString("Admin.PromotionProvidersHome.PromotionProvidersHome")%>" />
    <%=GetLocaleResourceString("Admin.PromotionProvidersHome.PromotionProvidersHome")%>
</div>
<div class="homepage">
    <div class="intro">
        <p>
            <%=GetLocaleResourceString("Admin.PromotionProvidersHome.intro")%>
        </p>
    </div>
    <div class="options">
        <ul>
            <li>
                <div class="title">
                    <a href="Froogle.aspx" title="<%=GetLocaleResourceString("Admin.PromotionProvidersHome.Froogle.TitleDescription")%>">
                        <%=GetLocaleResourceString("Admin.PromotionProvidersHome.Froogle.Title")%></a>
                </div>
                <div class="description">
                    <p>
                        <%=GetLocaleResourceString("Admin.PromotionProvidersHome.Froogle.Description")%>
                    </p>
                </div>
            </li>
        </ul>
    </div>
</div>
