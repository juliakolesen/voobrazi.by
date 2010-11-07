<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.ConfigurationHomeControl"
    CodeBehind="ConfigurationHome.ascx.cs" %>
<div class="section-title">
    <img src="Common/ico-configuration.png" alt="<%=GetLocaleResourceString("Admin.ConfigurationHome.ConfigurationHome")%>" />
    <%=GetLocaleResourceString("Admin.ConfigurationHome.ConfigurationHome")%>
</div>
<div class="homepage">
    <div class="intro">
        <p>
            <%=GetLocaleResourceString("Admin.ConfigurationHome.intro")%>
        </p>
    </div>
    <div class="options">
        <ul>
            <li>
                <div class="title">
                    <a href="GlobalSettings.aspx" title="<%=GetLocaleResourceString("Admin.ConfigurationHome.GlobalSettings.TitleDescription")%>">
                        <%=GetLocaleResourceString("Admin.ConfigurationHome.GlobalSettings.Title")%></a>
                </div>
                <div class="description">
                    <p>
                        <%=GetLocaleResourceString("Admin.ConfigurationHome.GlobalSettings.Description")%>
                    </p>
                </div>
            </li>
            <li>
                <div class="title">
                    <a href="PaymentSettingsHome.aspx" title="<%=GetLocaleResourceString("Admin.ConfigurationHome.PaymentSettingsHome.TitleDescription")%>">
                        <%=GetLocaleResourceString("Admin.ConfigurationHome.PaymentSettingsHome.Title")%></a>
                </div>
                <div class="description">
                    <p>
                        <%=GetLocaleResourceString("Admin.ConfigurationHome.PaymentSettingsHome.Description")%>
                    </p>
                </div>
            </li>
            <li>
                <div class="title">
                    <a href="TaxSettingsHome.aspx" title="<%=GetLocaleResourceString("Admin.ConfigurationHome.TaxSettingsHome.TitleDescription")%>">
                        <%=GetLocaleResourceString("Admin.ConfigurationHome.TaxSettingsHome.Title")%></a>
                </div>
                <div class="description">
                    <p>
                        <%=GetLocaleResourceString("Admin.ConfigurationHome.TaxSettingsHome.Description")%>
                    </p>
                </div>
            </li>
            <li>
                <div class="title">
                    <a href="ShippingSettingsHome.aspx" title="<%=GetLocaleResourceString("Admin.ConfigurationHome.ShippingSettingsHome.TitleDescription")%>">
                        <%=GetLocaleResourceString("Admin.ConfigurationHome.ShippingSettingsHome.Title")%></a>
                </div>
                <div class="description">
                    <p>
                        <%=GetLocaleResourceString("Admin.ConfigurationHome.ShippingSettingsHome.Description")%>
                    </p>
                </div>
            </li>
            <li>
                <div class="title">
                    <a href="LocationSettingsHome.aspx" title="<%=GetLocaleResourceString("Admin.ConfigurationHome.LocationSettingsHome.TitleDescription")%>">
                        <%=GetLocaleResourceString("Admin.ConfigurationHome.LocationSettingsHome.Title")%></a>
                </div>
                <div class="description">
                    <p>
                        <%=GetLocaleResourceString("Admin.ConfigurationHome.LocationSettingsHome.Description")%>
                    </p>
                </div>
            </li>
            <li>
                <div class="title">
                    <a href="Settings.aspx" title="<%=GetLocaleResourceString("Admin.ConfigurationHome.Settings.TitleDescription")%>">
                        <%=GetLocaleResourceString("Admin.ConfigurationHome.Settings.Title")%></a>
                </div>
                <div class="description">
                    <p>
                        <%=GetLocaleResourceString("Admin.ConfigurationHome.Settings.Description")%>
                    </p>
                </div>
            </li>
        </ul>
    </div>
</div>
