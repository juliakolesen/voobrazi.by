<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.AttributesHomeControl"
    CodeBehind="AttributesHome.ascx.cs" %>
<div class="section-title">
    <img src="Common/ico-catalog.png" alt="<%=GetLocaleResourceString("Admin.AttributesHome.AttributesHome")%>" />
    <%=GetLocaleResourceString("Admin.AttributesHome.AttributesHome")%>
</div>
<div class="homepage">
    <div class="intro">
        <p>
            <%=GetLocaleResourceString("Admin.AttributesHome.intro")%>
        </p>
    </div>
    <div class="options">
        <ul>
            <li>
                <div class="title">
                    <a href="ProductAttributes.aspx" title="<%=GetLocaleResourceString("Admin.AttributesHome.ProductAttributes.TitleDescription")%>">
                        <%=GetLocaleResourceString("Admin.AttributesHome.ProductAttributes.Title")%></a>
                </div>
                <div class="description">
                    <p>
                        <%=GetLocaleResourceString("Admin.AttributesHome.ProductAttributes.Description")%>
                    </p>
                </div>
            </li>
            <li>
                <div class="title">
                    <a href="SpecificationAttributes.aspx" title="<%=GetLocaleResourceString("Admin.AttributesHome.SpecificationAttributes.TitleDescription")%>">
                        <%=GetLocaleResourceString("Admin.AttributesHome.SpecificationAttributes.Title")%></a>
                </div>
                <div class="description">
                    <p>
                        <%=GetLocaleResourceString("Admin.AttributesHome.SpecificationAttributes.Description")%>
                    </p>
                </div>
            </li>
        </ul>
    </div>
</div>
