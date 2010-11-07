<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.PromotionsHomeControl"
    CodeBehind="PromotionsHome.ascx.cs" %>
<div class="section-title">
    <img src="Common/ico-promotions.png" alt="<%=GetLocaleResourceString("Admin.PromotionsHome.PromotionsHome")%>" />
    <%=GetLocaleResourceString("Admin.PromotionsHome.PromotionsHome")%>
</div>
<div class="homepage">
    <div class="intro">
        <p>
            <%=GetLocaleResourceString("Admin.PromotionsHome.intro")%>
        </p>
    </div>
    <div class="options">
        <ul>
            <li>
                <div class="title">
                    <a href="Affiliates.aspx" title="<%=GetLocaleResourceString("Admin.PromotionsHome.Affiliates.TitleDescription")%>">
                        <%=GetLocaleResourceString("Admin.PromotionsHome.Affiliates.Title")%></a>
                </div>
                <div class="description">
                    <p>
                        <%=GetLocaleResourceString("Admin.PromotionsHome.Affiliates.Description1")%></p>
                    <p>
                        <%=GetLocaleResourceString("Admin.PromotionsHome.Affiliates.Description2")%>
                    </p>
                </div>
            </li>
            <li>
                <div class="title">
                    <a href="Campaigns.aspx" title="<%=GetLocaleResourceString("Admin.PromotionsHome.Campaigns.TitleDescription")%>">
                        <%=GetLocaleResourceString("Admin.PromotionsHome.Campaigns.Title")%></a>
                </div>
                <div class="description">
                    <p>
                        <%=GetLocaleResourceString("Admin.PromotionsHome.Campaigns.Description")%>
                    </p>
                </div>
            </li>
            <li>
                <div class="title">
                    <a href="Discounts.aspx" title="<%=GetLocaleResourceString("Admin.PromotionsHome.Discounts.TitleDescription")%>">
                        <%=GetLocaleResourceString("Admin.PromotionsHome.Discounts.Title")%></a>
                </div>
                <div class="description">
                    <p>
                        <%=GetLocaleResourceString("Admin.PromotionsHome.Discounts.Description1")%>
                    </p>
                    <p>
                        <%=GetLocaleResourceString("Admin.PromotionsHome.Discounts.Description2")%>
                    </p>
                </div>
            </li>
            <li>
                <div class="title">
                    <a href="Pricelist.aspx" title="<%=GetLocaleResourceString("Admin.PromotionsHome.Pricelist.TitleDescription")%>">
                        <%=GetLocaleResourceString("Admin.PromotionsHome.Pricelist.Title")%></a>
                </div>
                <div class="description">
                    <p>
                        <%=GetLocaleResourceString("Admin.PromotionsHome.Pricelist.Description")%>
                    </p>
                </div>
            </li>
            <li>
                <div class="title">
                    <a href="PromotionProvidersHome.aspx" title="<%=GetLocaleResourceString("Admin.PromotionsHome.PromotionProvidersHome.TitleDescription")%>">
                        <%=GetLocaleResourceString("Admin.PromotionsHome.PromotionProvidersHome.Title")%></a>
                </div>
                <div class="description">
                    <p>
                        <%=GetLocaleResourceString("Admin.PromotionsHome.PromotionProvidersHome.Description")%>
                    </p>
                </div>
            </li>
        </ul>
    </div>
</div>
