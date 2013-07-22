<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestPage.aspx.cs" Inherits="NopCommerceStore.TestPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        #foot_panel_all
        {
            width: 100%;
            height: 45px;
            position: fixed;
            bottom: 0px;
            left: 0px;
            z-index: 500;
            font-family: 'Comic Sans MS';
            font-size: 26px;
            color: #FFFFFF;
            font-weight: 500;
            font-style: normal;
        }
        
        #foot_panel
        {
            width: 1050px;
            height: 45px;
            margin: 0 auto;
            background: url(../../images/ff_images/background-pay-menu.gif) repeat-x center center;
            z-index: 500;
        }

    </style>
</head>
<body>
    <span style="font-size: 300px;">a q w e e r r t t t y y u u i i o o op </span>
    <form runat="server">
    <%--<table width="1050" border="0" cellspacing="0" cellpadding="0" align="center" class="payFooter">
        <tr valign="middle">
            <td width="145px" align="center">
                <a class="delivery" title="Контакты" href="Contacts.aspx">Контакты</a>
            </td>
            <td width="145px" align="center">
                <a class="delivery" title="Доставка" href="AboutDelivery.aspx">Доставка</a>
            </td>
            <td width="365px" align="left">
                <a class="pay" title="Оплата" href="HowToPay.aspx">Оплата
                    <img src="/images/VisaMastercard.gif" alt="" height="25px" style="padding-top: 9px;
                        padding-left: 13px;" />
                </a>
            </td>
            <td width="155px" style="font-size: 12px; font-family: Arial; text-align: center;">
                <a href="<%=Page.ResolveUrl("~/ShoppingCart.aspx")%>" class="shCart">
                    <img src="<%=Page.ResolveUrl("~/images/ff_images/recycle.gif")%>" alt="" title="Корзина"
                        height="40px" style="margin-bottom: 2px;" />
                    <div style="display: inline-block; vertical-align: middle;">
                        <span class="amount_text">1230000</span>
                        <br />
                        <span class="amount_text">(</span> <span class="amount">12</span> <span class="amount_text">
                            товаров)</span>
                    </div>
                </a>
            </td>
            <td width="230px" align="center">
                <div style="font-size: 24px;">
                    <a>Валюта:</a>
                    <asp:LinkButton ID="lbtnBr" runat="server">BYR</asp:LinkButton>
                    /
                    <asp:LinkButton ID="lbtnUSD" runat="server">USD</asp:LinkButton>
                </div>
            </td>
        </tr>
    </table>--%>
    <div class="payFooterCommon payFooterLeft">
    </div>
    <div class="payFooterCommon payFooterRight">
    </div>
    <div id="foot_panel_all">
        <div id="foot_panel">
            <%--<div id="oplata">
                <a href="">
                    <img src="images/test/oplata.png"></a>
            </div>
            <div class="cart">
                <span class="cart_result">
                    <div class="cart1">
                        <a href="">Моя корзина</a> <span class="cart_counter">0</span>
                    </div>
                    <div class="cart2">
                        <span class="cart_sum">0 руб</span>
                    </div>
                </span><a href="" class="to_order">
                    <img class="nohover" src="images/test/to_order.jpg"
                        alt="to_order" />
                    <img class="hover" src="images/test/to_order_hover.jpg"
                        alt="to_order" />
                </a>
            </div>--%><%--Их футер--%>
            <%--Мое переделанное с их футером--%>
                <div style="float:left; margin-left: 5px;">
                    <a style="margin-right: -5px" title="Контакты" href="./Contacts.aspx">Контакты</a>
                    | <a style="margin-right: -5px; margin-left: -5px;" title="Доставка" href="./AboutDelivery.aspx">
                        Доставка</a> | <a style="margin-left: -5px;" title="Оплата" href="./HowToPay.aspx">
                            Оплата</a>   
                </div>
                <div style="float:left">
                    <a title="Оплата" href="./HowToPay.aspx">
                        <img src="./images/ff_images/background-pay-menu-white-background.gif" style="margin-top: 6px; margin-left: 5px;"
                            alt="" /></a>
                </div>
                <div style="float:left">|</div>
                <div style="float:left">
                    <a title="Корзина" href="<%=Page.ResolveUrl("~/ShoppingCart.aspx")%>">
                        <img style="margin-top: 1px;" src="./images/ff_images/background-pay-menu-white-background_recycle.gif"
                            alt="" /></a>
                </div>
                <div class="payFooterCurrency" style="margin-top: 3px; margin-left: -115px; float:left;">
                    <a title="Корзина" href="<%=Page.ResolveUrl("~/ShoppingCart.aspx")%>"><span class="amount_text">
                        sum</span>
                        <br />
                        <span class="amount_text">(</span> <span class="amount">count</span>
                        <span class="amount_text">товаров)</span> </a>
                </div>
                <div style="float:left">|</div>
                <div style="font-size: 24px; margin-top: 3px; float:left;">
                    <a>Валюта:</a>
                    <asp:LinkButton ID="lbtnBr" runat="server">BYR</asp:LinkButton>
                    /
                    <asp:LinkButton ID="lbtnUSD" runat="server">USD</asp:LinkButton>
                </div>
        </div>
    </div>
    </form>
</body>
</html>
