<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PayFooter.ascx.cs" Inherits="NopSolutions.NopCommerce.Web.Modules.PayFooter" %>

<table class="flowersWholesale" style="margin-bottom: 5px;">
    <tr>
        <td>
            <div id="banner">
                <asp:HyperLink runat="server" ID="flowersWholesaleLink" title="Цветы оптом" NavigateUrl="~/FlowersWholesale.aspx">
					<img id = "mainImg" src="../images/ff_images/FlowersWholesale.gif" alt="" height="113px"/>
                </asp:HyperLink>
            </div>
            <asp:HyperLink runat="server" ID="flowersWholesaleLinkSmall" title="Цветы оптом"
                NavigateUrl="~/FlowersWholesale.aspx">
					<img id = "mainImgSmall" src="../images/ff_images/FlowersWholesaleSmall.gif" alt="" height="22px" style="display: none;"/>
            </asp:HyperLink>
        </td>
        <td valign="top" style="padding-top: 5px;">
            <a id="openA" href="javascript:void(0);" onclick="toggle()">
                <img id="imgClose" src="../images/ff_images/arrow_down.gif" alt="" style="display: block;"
                    title="Свернуть" />
                <img id="imgOpen" src="../images/ff_images/arrow_up.gif" alt="" style="display: none;"
                    title="Развернуть" />
            </a>
        </td>
    </tr>
</table>
<table width="1050" border="0" cellspacing="0" cellpadding="0" align="center" class="payFooter">
    <tr valign="middle">
        <td width="145px" align="center">
            <a class="delivery" title="Контакты" href="../Contacts.aspx">Контакты</a>
        </td>
        <td width="145px" align="center">
            <a class="delivery" title="Доставка" href="../AboutDelivery.aspx">Доставка</a>
        </td>
        <td width="365px" align="left">
            <a class="pay" title="Оплата" href="../HowToPay.aspx">Оплата
                <img src="../images/VisaMastercard.gif" alt="" height="25px" style="padding-top: 9px;
                    padding-left: 13px;" />
            </a>
        </td>
        <%--        <td width="60px" align="center">
            <asp:HyperLink runat="server" ID="sitemapLink" title="Карта сайта" NavigateUrl="~/SiteMap.aspx">
					<img src="../images/ff_images/sitemap.gif" alt="" height="40px" style="padding-top: 7px;"/>
            </asp:HyperLink>
        </td>--%>
        <td width="155px" style="font-size:12px; font-family:Arial; text-align:center;">
            <a href="<%=Page.ResolveUrl("~/ShoppingCart.aspx")%>" class="shCart">
                 <img src="<%=Page.ResolveUrl("~/images/ff_images/recycle.gif")%>" alt="" title="Корзина"
                        height="40px" style="margin-bottom: 2px;"/>
                        <div style="display:inline-block; vertical-align:middle;">
                <span class="amount_text">
                    <%=GetShoppingCartSum()%></span> <br/>
                <span class="amount_text">(</span>
                <span class="amount"><%=ShoppingCartManager.GetCurrentShoppingCart(ShoppingCartTypeEnum.ShoppingCart).Count%></span>
                <span class="amount_text">товаров)</span>
                </div>
                </a>
        </td>
        <td width="230px" align="center">
            <div style="font-size: 24px;">
                <a>Валюта:</a>
                <asp:LinkButton ID="lbtnBr" runat="server" OnClick="OnCurrencyChange">BYR</asp:LinkButton>
                /
                <asp:LinkButton ID="lbtnUSD" runat="server" OnClick="OnCurrencyChange">USD</asp:LinkButton>
            </div>
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
