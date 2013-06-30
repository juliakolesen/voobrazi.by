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
<div class="payFooterCommon payFooterLeft">
</div>
<div class="payFooterCommon payFooterRight">
</div>
<div class="payFooterCommon payFooter">
    <div style="margin-left: -535px;">
    <a style="margin-right:-5px" title="Контакты" href="../Contacts.aspx">Контакты</a> | 
    <a style="margin-right:-5px; margin-left:-5px;" title="Доставка" href="../AboutDelivery.aspx">Доставка</a> |
    <a style="margin-left:-5px;" title="Оплата" href="../HowToPay.aspx">Оплата </a>
    </div>
    <div style="margin-left: -125px;">
        <a title="Оплата" href="../HowToPay.aspx">
       <img src="../images/ff_images/background-pay-menu-white-background.gif" style="margin-top: 6px;" alt=""  /></a>
    </div>
    <div style="margin-left: 125px;">|</div>
    <div style="margin-left: 137px;">
    <a title="Корзина" href="<%=Page.ResolveUrl("~/ShoppingCart.aspx")%>">
       <img style="margin-top: 1px;" src="../images/ff_images/background-pay-menu-white-background_recycle.gif" alt=""  /></a>
    </div>
    <div class="payFooterCurrency" style="margin-left: 195px; margin-top: 3px;">
    <a title="Корзина" href="<%=Page.ResolveUrl("~/ShoppingCart.aspx")%>">
        <span class="amount_text"> <%=GetShoppingCartSum()%></span>
        <br />
        <span class="amount_text">(</span> <span class="amount">
                <%=ShoppingCartManager.GetCurrentShoppingCart(ShoppingCartTypeEnum.ShoppingCart).Count%></span>
        <span class="amount_text">товаров)</span>
        </a>
    </div>
    <div style="margin-left: 304px;">|</div>
    <div style="margin-left: 316px; font-size: 24px; margin-top: 3px;">
        <a>Валюта:</a>
        <asp:LinkButton ID="lbtnBr" runat="server" OnClick="OnCurrencyChange">BYR</asp:LinkButton>
        /
        <asp:LinkButton ID="lbtnUSD" runat="server" OnClick="OnCurrencyChange">USD</asp:LinkButton>
    </div>
</div>
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
