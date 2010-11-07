<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.SystemHomeControl"
    CodeBehind="SystemHome.ascx.cs" %>
<div class="section-title">
    <img src="Common/ico-system.png" alt="<%=GetLocaleResourceString("Admin.SystemHome.SystemHome")%>" />
    <%=GetLocaleResourceString("Admin.SystemHome.SystemHome")%>
</div>
<div class="homepage">
    <div class="intro">
        <p>
            <%=GetLocaleResourceString("Admin.SystemHome.intro")%>
        </p>
    </div>
    <div class="options">
        <ul>
            <li>
                <div class="title">
                    <a href="Logs.aspx" title="<%=GetLocaleResourceString("Admin.SystemHome.Logs.TitleDescription")%>">
                        <%=GetLocaleResourceString("Admin.SystemHome.Logs.Title")%></a>
                </div>
                <div class="description">
                    <p>
                        <%=GetLocaleResourceString("Admin.SystemHome.Logs.Description")%>
                    </p>
                </div>
            </li>
            <li>
                <div class="title">
                    <a href="MessageQueue.aspx" title="<%=GetLocaleResourceString("Admin.SystemHome.MessageQueue.TitleDescription")%>">
                        <%=GetLocaleResourceString("Admin.SystemHome.MessageQueue.Title")%></a>
                </div>
                <div class="description">
                    <p>
                        <%=GetLocaleResourceString("Admin.SystemHome.MessageQueue.Description")%>
                    </p>
                </div>
            </li>
        </ul>
    </div>
</div>
