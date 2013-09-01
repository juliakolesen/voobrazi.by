<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MainPage.ascx.cs" Inherits="NopSolutions.NopCommerce.Web.Modules.MainPage" %>
<%@ Register TagPrefix="nopCommerce" TagName="Topic" Src="~/Modules/Topic.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="SlideShow" Src="~/Modules/SlideShow.ascx" %>
<table class="mainTable" border="1">
    <tr>
        <td rowspan="3" colspan="3" class="mainTableBigCell">
            <nopCommerce:SlideShow ID="slideShow" runat="server" class="mainTableCentering">
            </nopCommerce:SlideShow>
        </td>
        <td class="mainTableTd">
            <div class="mainTableDiv" id="div1" runat="server">
                <nopCommerce:Topic ID="topicDefaultPage1" runat="server" />
            </div>
        </td>
    </tr>
    <tr>
        <td class="mainTableTd">
            <div class="mainTableDiv" id="div2" runat="server">
                <nopCommerce:Topic ID="topicDefaultPage2" runat="server" />
            </div>
        </td>
    </tr>
    <tr>
        <td class="mainTableTd">
            <div class="mainTableDiv" id="div3" runat="server">
                <nopCommerce:Topic ID="topicDefaultPage3" runat="server" />
            </div>
        </td>
    </tr>
    <tr>
        <td class="mainTableTd">
            <div class="mainTableDiv" id="div4" runat="server">
                <nopCommerce:Topic ID="topicDefaultPage4" runat="server" />
            </div>
        </td>
        <td class="mainTableTd">
            <div class="mainTableDiv" id="div5" runat="server">
                <nopCommerce:Topic ID="topicDefaultPage5" runat="server" />
            </div>
        </td>
        <td class="mainTableTd">
            <div class="mainTableDiv" id="div6" runat="server">
                <nopCommerce:Topic ID="topicDefaultPage6" runat="server" />
            </div>
        </td>
        <td class="mainTableTd">
            <div class="mainTableDiv" id="div7" runat="server">
                <nopCommerce:Topic ID="topicDefaultPage7" runat="server" />
            </div>
        </td>
    </tr>
</table>
