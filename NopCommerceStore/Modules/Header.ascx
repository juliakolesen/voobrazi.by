<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.HeaderControl"
    CodeBehind="Header.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="CurrencySelector" Src="~/Modules/CurrencySelector.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="LanguageSelector" Src="~/Modules/LanguageSelector.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="TaxDisplayTypeSelector" Src="~/Modules/TaxDisplayTypeSelector.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="Topic" Src="~/Modules/Topic.ascx" %>
<asp:LoginView ID="topLoginView" runat="server">
    <AnonymousTemplate>
        <a href="<%=Page.ResolveUrl("~/Login.aspx")%>" title="Вход">Вход</a>&nbsp;&nbsp;&nbsp;/&nbsp;&nbsp;<a
            href="<%=Page.ResolveUrl("~/Register.aspx")%>" title="Регистрация">Регистрация</a>
        <br />
        <br />
        <div class="logo">
            <script type="text/javascript">

                var _gaq = _gaq || [];
                _gaq.push(['_setAccount', 'UA-9734510-32']);
                _gaq.push(['_addOrganic', 'images.google.ru', 'q']);
                _gaq.push(['_addOrganic', ('mail.ru', 'q')]);
                _gaq.push(['_addOrganic', ('rambler.ru', 'words')]);
                _gaq.push(['_addOrganic', ('nigma.ru', 's')]);
                _gaq.push(['_addOrganic', ('blogs.yandex.ru', 'text')]);
                _gaq.push(['_addOrganic', ('yandex.ru', 'text')]);
                _gaq.push(['_addOrganic', ('aport.ru', 'r')]);
                _gaq.push(['_addOrganic', ('akavita.by', 'z')]);
                _gaq.push(['_addOrganic', ('search.tut.by', 'query')]);
                _gaq.push(['_addOrganic', ('all.by', 'query')]);
                _gaq.push(['_trackPageview']);

                (function () {
                    var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
                    ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
                    var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
                })();

            </script>
            <a href="/" title="Воображение">
                <img src="<%=Page.ResolveUrl("~/images/ff_images/logo.gif")%>" alt="" />
            </a>
        </div>
        <div class="logo_main">
            <a href="/" title="Воображение">
                <img src="<%=Page.ResolveUrl("~/images/ff_images/mainimg_logo.jpg")%>" alt="" height="90px" />
            </a>
        </div>
    </AnonymousTemplate>
    <LoggedInTemplate>
        Добро пожаловать, <span class="name">
            <%=Page.User.Identity.Name %></span>&nbsp;&nbsp;&nbsp;/&nbsp;&nbsp;<a href="<%=Page.ResolveUrl("~/Logout.aspx")%>"
                title="Выйти">Выйти</a>
        <br />
        <% if (Page.User.IsInRole("Admin"))
           { %>
        <li><a href="<%=Page.ResolveUrl("~/Administration/Default.aspx")%>" class="ico-admin">
            <%=GetLocaleResourceString("Account.Administration")%></a> </li>
        <%} %>
        <br />
        <div class="logo">
            <a href="/" title="Воображение">
                <img src="<%=Page.ResolveUrl("~/images/ff_images/logo.gif")%>" alt="" width="299"
                    height="90" />
        </div>
        <div class="logo_main">
            <a href="/" title="Воображение">
                <img src="<%=Page.ResolveUrl("~/images/ff_images/mainimg_logo.jpg")%>" alt="" height="90px" />
            </a>
        </div>
    </LoggedInTemplate>
</asp:LoginView>
<table width="600px">
    <tr>
        <td width="500px">
            <span style="padding-right: 25px;">
                <nopCommerce:Topic ID="ContactPhones" runat="server" TopicName="ContactPhones" />
            </span>
        </td>
    </tr>
</table>
