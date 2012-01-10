<%@ Page Language="C#" MasterPageFile="~/MasterPages/OneColumn.master" AutoEventWireup="true"
    CodeBehind="HowToOrder.aspx.cs" Title="Как заказать" Inherits="NopCommerceStore.HowToOrder" %>

<%@ Register TagPrefix="nopCommerce" TagName="Topic" Src="~/Modules/Topic.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="Server">
    <nopCommerce:Topic ID="topicHowToOrder" runat="server" TopicName="HowToOrder">
    </nopCommerce:Topic>
</asp:Content>