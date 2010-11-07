<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.CustomersHomeControl"
    CodeBehind="CustomersHome.ascx.cs" %>
<div class="section-title">
    <img src="Common/ico-customers.png" alt="<%=GetLocaleResourceString("Admin.CustomersHome.CustomersHome")%>" />
    <%=GetLocaleResourceString("Admin.CustomersHome.CustomersHome")%>
</div>
<div class="homepage">
    <div class="intro">
        <p>
            <%=GetLocaleResourceString("Admin.CustomersHome.intro")%>
        </p>
    </div>
    <div class="options">
        <ul>
            <li>
                <div class="title">
                    <a href="Customers.aspx" title="<%=GetLocaleResourceString("Admin.CustomersHome.Customers.TitleDescription")%>">
                        <%=GetLocaleResourceString("Admin.CustomersHome.Customers.Title")%></a>
                </div>
                <div class="description">
                    <p>
                        <%=GetLocaleResourceString("Admin.CustomersHome.Customers.Description")%>
                    </p>
                </div>
            </li>
            <li>
                <div class="title">
                    <a href="CustomerRoles.aspx" title="<%=GetLocaleResourceString("Admin.CustomersHome.CustomerRoles.TitleDescription")%>">
                        <%=GetLocaleResourceString("Admin.CustomersHome.CustomerRoles.Title")%></a>
                </div>
                <div class="description">
                    <p>
                        <%=GetLocaleResourceString("Admin.CustomersHome.CustomerRoles.Description1")%>
                    </p>
                    <p>
                        <%=GetLocaleResourceString("Admin.CustomersHome.CustomerRoles.Description2")%>
                    </p>
                </div>
            </li>
        </ul>
    </div>
</div>
