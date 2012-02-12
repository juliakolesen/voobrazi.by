<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IndividualOrderControl.ascx.cs"
    Inherits="NopSolutions.NopCommerce.Web.Modules.IndividualOrderControl" %>
<%@ Register TagPrefix="nopCommerce" TagName="Topic" Src="~/Modules/Topic.ascx" %>
<script language="javascript" type="text/javascript">
    function ChangeControlVisibility(radioButList, drpId) {
        var drpDownList;
        var txtBox;
        switch (drpId){
            case "1":
                drpDownList = document.getElementById("<%=UserVariants.ClientID %>");
                txtBox = document.getElementById("<%=ownUser.ClientID %>");
                break;
            case "2":
                drpDownList = document.getElementById("<%=reasonVar.ClientID %>");
                txtBox = document.getElementById("<%=ownReason.ClientID %>");
                break;
            case "3":
                drpDownList = document.getElementById("<%=flowerVar.ClientID %>");
                txtBox = document.getElementById("<%=ownFlower.ClientID %>");
                break;
            case "4":
                drpDownList = document.getElementById("<%=colourVar.ClientID %>");
                txtBox = document.getElementById("<%=ownColour.ClientID %>");
                break;
            case "5":
                drpDownList = document.getElementById("<%=bunchVar.ClientID %>");
                txtBox = document.getElementById("<%=ownVariant.ClientID %>");
                break;
        }

        if (radioButList.rows[0].cells[0].firstChild.checked)
        {
            drpDownList.style.visibility = "hidden";
            txtBox.style.visibility = "hidden";
        }
       if (radioButList.rows[1].cells[0].firstChild.checked)
        {
            drpDownList.style.visibility = "visible";
            txtBox.style.visibility = "hidden";
        }
        if (radioButList.rows[2].cells[0].firstChild.checked)
        {
            drpDownList.style.visibility = "hidden";
            txtBox.style.visibility = "visible";
        }
    }
</script>
<nopCommerce:Topic ID="topicIndividualOrder" runat="server" TopicName="IndividualOrder">
</nopCommerce:Topic>
</br>
<table>
    <tr>
        <td colspan="3">
            <asp:Label ID="label1" runat="server" Text="1. Для кого цветы:" Font-Bold="true"></asp:Label>
        </td>
    </tr>
    <tr>
        <td width="150" rowspan="3">
            <asp:RadioButtonList runat="server" ID="userVarList" >
                <asp:ListItem Text="Не важно" Selected="True"></asp:ListItem>
                <asp:ListItem Text="Выбрать вариант"></asp:ListItem>
                <asp:ListItem Text="Ваш вариант"></asp:ListItem>
            </asp:RadioButtonList>
        </td>
        <td height="20">
        </td>
    </tr>
    <tr>
        <td width="207" height="20">
            <asp:DropDownList ID="UserVariants" runat="server" Width="207" >
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td width="200" height="20">
            <asp:TextBox ID="ownUser" runat="server" Width="200" > 
            </asp:TextBox>
        </td>
    </tr>
    <tr>
        <td colspan="3">
            <asp:Label ID="reasonLabel" runat="server" Text="2. Какой повод/ праздник:" Font-Bold="true">
            </asp:Label>
        </td>
    </tr>
    <tr>
        <td width="150" rowspan="3">
            <asp:RadioButtonList runat="server" ID="reasonVarList">
                <asp:ListItem Text="Не важно" Selected="True"></asp:ListItem>
                <asp:ListItem Text="Выбрать вариант"></asp:ListItem>
                <asp:ListItem Text="Ваш вариант"></asp:ListItem>
            </asp:RadioButtonList>
        </td>
        <td height="20">
        </td>
    </tr>
    <tr>
        <td width="207" height="20">
            <asp:DropDownList ID="reasonVar" runat="server" Width="207">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td width="200" height="20">
            <asp:TextBox ID="ownReason" runat="server" Width="200">
            </asp:TextBox>
        </td>
    </tr>
    <tr>
        <td colspan="3">
            <asp:Label ID="FlowerLabel" runat="server" Text="3. Какие цветы предпочитаете:" Font-Bold="true">
            </asp:Label>
        </td>
    </tr>
    <tr>
        <td width="150" rowspan="3">
            <asp:RadioButtonList runat="server" ID="flowerVarList">
                <asp:ListItem Text="Не важно" Selected="True"></asp:ListItem>
                <asp:ListItem Text="Выбрать вариант"></asp:ListItem>
                <asp:ListItem Text="Ваш вариант"></asp:ListItem>
            </asp:RadioButtonList>
        </td>
        <td height="20">
        </td>
    </tr>
    <tr>
        <td width="207" height="20">
            <asp:DropDownList ID="flowerVar" runat="server" Width="207">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td width="200" height="20">
            <asp:TextBox ID="ownFlower" runat="server" Width="200">
            </asp:TextBox>
        </td>
    </tr>
    <tr>
        <td colspan="3">
            <asp:Label ID="ColourLabel" runat="server" Text="4. Какая цветовая гамма:" Font-Bold="true">
            </asp:Label>
        </td>
    </tr>
    <tr>
        <td width="150" rowspan="3">
            <asp:RadioButtonList runat="server" ID="colourVarList">
                <asp:ListItem Text="Не важно" Selected="True"></asp:ListItem>
                <asp:ListItem Text="Выбрать вариант"></asp:ListItem>
                <asp:ListItem Text="Ваш вариант"></asp:ListItem>
            </asp:RadioButtonList>
        </td>
        <td height="20">
        </td>
    </tr>
    <tr>
        <td width="207" height="20">
            <asp:DropDownList ID="colourVar" runat="server" Width="207">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td width="200" height="20">
            <asp:TextBox ID="ownColour" runat="server" Width="200">
            </asp:TextBox>
        </td>
    </tr>
    <tr>
        <td colspan="3">
            <asp:Label ID="Label2" runat="server" Text="5. Какой вариант букета/композиции предпочитаете:"
                Font-Bold="true">
            </asp:Label>
        </td>
    </tr>
    <tr>
        <td width="150" rowspan="3">
            <asp:RadioButtonList runat="server" ID="bunchVarList">
                <asp:ListItem Text="Не имеет значения/ на ваш выбор" Selected="True"></asp:ListItem>
                <asp:ListItem Text="Выбрать вариант"></asp:ListItem>
                <asp:ListItem Text="Ваш вариант"></asp:ListItem>
            </asp:RadioButtonList>
        </td>
        <td height="20">
        </td>
    </tr>
    <tr>
        <td width="207" height="20">
            <asp:DropDownList ID="bunchVar" runat="server" Width="207">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td width="200" height="20">
            <asp:TextBox ID="ownVariant" runat="server" Width="200">
            </asp:TextBox>
        </td>
        <td width="150">
            <asp:Label ID="popupLabel1" runat="server" Text="? Подсказка" ForeColor="Red" />
            <asp:Panel ID="Panel1" runat="server" CssClass="popupControl">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="container" style="margin-top: 20px; width: 500px;">
                            <div class="cont_top" style="width: 500px; background: transparent url(../../images/ff_images/header2.gif) no-repeat scroll center left;">
                                <div class="titl">
                                    Подсказка</div>
                            </div>
                            <div class="cont_middle" style="width: 500px; background: transparent url(../../images/ff_images/middle2.gif) repeat-y scroll center center;">
                                <div class="text">
                                    <nopCommerce:Topic ID="compositionVar" runat="server" TopicName="CompositionVariants" />
                                </div>
                            </div>
                            <div class="cont_bottom" style="width: 500px; background: transparent url(../../images/ff_images/bottom2.gif) no-repeat scroll center center;">
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
            <ajaxToolkit:PopupControlExtender ID="popupWindow1" Position="Left" runat="server"
                PopupControlID="Panel1" TargetControlID="popupLabel1" OffsetX="-300" OffsetY="-300" />
        </td>
    </tr>
    <tr>
        <td width="300">
            <asp:Label ID="PriceLabel" runat="server" Text="6. Цена:" Font-Bold="true">
            </asp:Label>
        </td>
    </tr>
    <tr>
        <td width="200">
            <asp:TextBox ID="Price" runat="server" Width="200">
            </asp:TextBox>
        </td>
        <td width="300">
            <asp:RequiredFieldValidator ID="priceValue1" ControlToValidate="Price" runat="server"
                Display="Dynamic" Text="Поле обязательно для заполнения!" ValidationGroup="InfoGroup">
            </asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="priceValue2" runat="server" ControlToValidate="Price"
                ValidationExpression="[0-9]*" Text="Цена должна состоять только из цифр!" Display="Dynamic"
                ValidationGroup="InfoGroup" />
        </td>
        <td width="150">
            <asp:Label ID="popupLabel2" runat="server" Text="? Подсказка" ForeColor="Red" />
            <asp:Panel ID="Panel2" runat="server" CssClass="popupControl">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <div class="container" style="margin-top: 20px; width: 500px;">
                            <div class="cont_top" style="width: 500px; background: transparent url(../../images/ff_images/header2.gif) no-repeat scroll center left;">
                                <div class="titl">
                                    Подсказка</div>
                            </div>
                            <div class="cont_middle" style="width: 500px; background: transparent url(../../images/ff_images/middle2.gif) repeat-y scroll center center;">
                                <div class="text">
                                    <nopCommerce:Topic ID="Topic1" runat="server" TopicName="PriceVariants" />
                                </div>
                            </div>
                            <div class="cont_bottom" style="width: 500px; background: transparent url(../../images/ff_images/bottom2.gif) no-repeat scroll center center;">
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
            <ajaxToolkit:PopupControlExtender ID="popupWindow2" Position="Left" runat="server"
                PopupControlID="Panel2" TargetControlID="popupLabel2" OffsetX="-300" OffsetY="-350" />
        </td>
    </tr>
</table>
<br />
<fieldset id="Fieldset1" class="group1" runat="server" style="width: 200px;">
    <legend>Быстрый заказ:</legend>
    <table>
        <tr>
            <td width="100">
                <asp:Label runat="server" Text="Имя" ID="nameLabel" />
            </td>
            <td>
                <asp:TextBox runat="server" ID="nameTextBox" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="userInfoValidator" ControlToValidate="nameTextBox"
                    runat="server" Display="Dynamic" Text="Не указано имя!" ValidationGroup="InfoGroup" />
            </td>
        </tr>
        <tr>
            <td width="100">
                <asp:Label runat="server" Text="Телефон" ID="phoneLabel" />
            </td>
            <td>
                <asp:TextBox runat="server" ID="phoneTextBox" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="phoneRequiredValidator" ControlToValidate="phoneTextBox"
                    runat="server" Display="Dynamic" Text="Не указан телефон!" ValidationGroup="InfoGroup" />
            </td>
        </tr>
    </table>
    <asp:Button ID="fastOrder" runat="server" ValidationGroup="InfoGroup" Text="Отправить быстрый заказ"
        OnCommand="fastOrder_Click"></asp:Button>
</fieldset>
