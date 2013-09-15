<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MainPage.ascx.cs" Inherits="NopSolutions.NopCommerce.Web.Modules.MainPage" %>
<%@ Register TagPrefix="nopCommerce" TagName="Topic" Src="~/Modules/Topic.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="SlideShow" Src="~/Modules/SlideShow.ascx" %>
<table class="mainTable">
    <tr>
        <td colspan="2" class="mainTableBigCell">
            <nopCommerce:SlideShow ID="slideShow" runat="server" class="mainTableCentering">
            </nopCommerce:SlideShow>
        </td>
        <td class="mainTableTd1">
            <div class="mainTableDiv1Back" id="div1" runat="server">
                <div class="mainTableDiv1" runat="server">
                    <nopCommerce:Topic ID="topicDefaultPage1" runat="server" />
                </div>
            </div>
        </td>
    </tr>
    <tr>
        <td class="mainTableTd2">
            <div class="mainTableDiv2" id="div2" runat="server">
                <nopCommerce:Topic ID="topicDefaultPage2" runat="server" />
            </div>
        </td>
        <td class="mainTableTd2">
            <div class="mainTableDiv2" id="div3" runat="server">
                <nopCommerce:Topic ID="topicDefaultPage3" runat="server" />
            </div>
        </td>
        <td class="mainTableTd2">
            <div class="mainTableDiv2" id="div4" runat="server">
                <nopCommerce:Topic ID="topicDefaultPage4" runat="server" />
            </div>
        </td>
    </tr>
</table>
<div class="client">
    Наши Клиенты:</div>
<div class="clientsList">
     <nopCommerce:Topic ID="topic1" runat="server" TopicName="Clients"/>
</div>
<div>
    <div style="margin-top:20px;">
        <asp:HyperLink runat="server" ID="indivOrder" title="Индивидуальный заказ цветов"
            NavigateUrl="~/IndividualOrder.aspx">
					<img src="/images/ff_images/individualOrder.jpg" alt="" height="41" />
        </asp:HyperLink>
    </div>
</div>
