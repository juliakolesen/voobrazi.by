<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InformationMenu.ascx.cs"
    Inherits="NopSolutions.NopCommerce.Web.Modules.InformationMenu" %>
<table width="690" border="0" cellspacing="0" cellpadding="0" align="center" class="infomenu_tbl">
    <tr valign="middle">
        <td valign="middle">
            <a class="order" title="Как заказать" href="../HowToOrder.aspx">Как заказать</a>
        </td>
        <td>
            <a class="delivery" title="О доставке" href="../AboutDelivery.aspx">О доставке</a>
        </td>
        <td>
            <a class="pay" title="Как оплатить" href="../HowToPay.aspx">Как оплатить</a>
        </td>
        <td>
            <img width="87" src="/images/VisaMastercard.gif" height="28" />
        </td>
        <td>
            <asp:HyperLink runat="server" ID="sitemapLink" title="Карта сайта" NavigateUrl="~/SiteMap.aspx">
					<img src="../images/ff_images/sitemap.png" alt=""/>
            </asp:HyperLink>
        </td>
    </tr>
</table>
