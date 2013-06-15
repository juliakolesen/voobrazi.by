<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PayFooter.ascx.cs" Inherits="NopSolutions.NopCommerce.Web.Modules.PayFooter" %>
<table class="flowersWholesale" style="margin-bottom:5px;">
    <tr>
        <td>
            <div id="banner">
                <asp:HyperLink runat="server" ID="flowersWholesaleLink" title="Цветы оптом" NavigateUrl="~/FlowersWholesale.aspx">
					<img id = "mainImg" src="../images/ff_images/FlowersWholesale.gif" alt="" height="150px" />
                </asp:HyperLink>
            </div>
            <asp:HyperLink runat="server" ID="flowersWholesaleLinkSmall" title="Цветы оптом"
                NavigateUrl="~/FlowersWholesale.aspx" >
					<img id = "mainImgSmall" src="../images/ff_images/FlowersWholesaleSmall.gif" alt="" height="30px" style="display:none;"/>
            </asp:HyperLink>
        </td>
        <td valign="top" style="padding-top:5px;">
            <a id="openA" href="javascript:void(0);" onclick="toggle()">
                <img id="imgClose" src="../images/ff_images/arrow_down.gif" alt="" style="display:block;"
                    title="Свернуть" />
                <img id="imgOpen" src="../images/ff_images/arrow_up.gif" alt="" style="display:none;"
                    title="Развернуть" />
            </a>
        </td>
    </tr>
</table>
<table width="1050" border="0" cellspacing="0" cellpadding="0" align="center" class="payFooter">
    <tr valign="middle">
        <td width="120px" align="center">
            <a class="pay" title="Оплата" href="../HowToPay.aspx">Оплата</a> |
        </td>
        <td width="215px" align="center">
            <a class="delivery" title="Доставка" href="../AboutDelivery.aspx">Доставка</a> |
            Безнал
        </td>
        <td width="80px">
            <img width="86px" src="/images/VisaMastercard.gif" height="28" alt="" style="padding-top: 2px;" />
        </td>
        <td width="1px">
            |
        </td>
        <td width="60px" align="center">
            <asp:HyperLink runat="server" ID="sitemapLink" title="Карта сайта" NavigateUrl="~/SiteMap.aspx">
					<img src="../images/ff_images/sitemap.gif" alt="" height="40px" style="padding-top: 7px;"/>
            </asp:HyperLink>
        </td>
        <td width="1px">
            |
        </td>
        <td width="200px" align="right">
            <a href="<%=Page.ResolveUrl("~/ShoppingCart.aspx")%>" class="shoppingCart">Корзина:
                (<span class="amount"><%=GetCount()%></span><span class="amount_text">товаров</span>)</a>
        </td>
        <td width="230px" align="center">
            <a>Валюта:</a>
            <asp:LinkButton ID="lbtnBr" runat="server" onclick="OnCurrencyChange">BYR</asp:LinkButton>
            /
            <asp:LinkButton ID="lbtnUSD" runat="server" onclick="OnCurrencyChange">USD</asp:LinkButton>
        </td>
    </tr>
</table>
<script language="javascript" type="text/javascript">
    function toggle() {
        document.getElementById('mainImgSmall').style.display = 'none';
        $('#banner').slideToggle('slow', afterToggle);
    }

    function afterToggle() {
        if (document.getElementById('imgClose').style.display == 'block') {
            document.getElementById('imgClose').style.display = 'none';
            document.getElementById('imgOpen').style.display = 'block';
            document.getElementById('mainImgSmall').style.display = 'block';
        } else {
            document.getElementById('imgClose').style.display = 'block';
            document.getElementById('imgOpen').style.display = 'none';
        }
    } 
</script>
