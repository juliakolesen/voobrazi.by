<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PayFooter.ascx.cs" Inherits="NopSolutions.NopCommerce.Web.Modules.PayFooter" %>
<table width="1050" border="0" cellspacing="0" cellpadding="0" align="center" class="payFooter">
    <tr valign="middle">
        <td width="100px" align="center">
            <a class="pay" title="ОПЛАТА" href="../HowToPay.aspx">ОПЛАТА</a>
        </td>
        <td width="150px" align="center">
            <a class="delivery" title="ДОСТАВКА" href="../AboutDelivery.aspx">ДОСТАВКА</a>
        </td>
        <td width="150px">
            <img width="130" src="/images/VisaMastercard.gif" height="28" alt="" />
        </td>
        <td width="80px">
            <asp:HyperLink runat="server" ID="sitemapLink" title="Карта сайта" NavigateUrl="~/SiteMap.aspx">
					<img src="../images/ff_images/sitemap.png" alt=""/>
            </asp:HyperLink>
        </td>
        <td width="300px" align="right">
            <a href="<%=Page.ResolveUrl("~/ShoppingCart.aspx")%>" class="shoppingCart">
                <img src="<%=Page.ResolveUrl("~/images/ff_images/recycle.jpg")%>" alt="" align="absbottom" title="Корзина"/>
                Корзина (<span class="amount"><%=GetCount()%></span><span class="amount_text">товаров</span>)</a>
        </td>
        <td width="270px" align="center">
            Валюта:
            <asp:DropDownList ID="ddlCur1" runat="server" OnSelectedIndexChanged="DdlCurSelectedValueChanged"
                AutoPostBack="true">
                <asp:ListItem Value="BYR" Text="Белорусские рубли" />
                <asp:ListItem Value="USD" Text="Доллары США" />
            </asp:DropDownList>
        </td>
    </tr>
</table>
