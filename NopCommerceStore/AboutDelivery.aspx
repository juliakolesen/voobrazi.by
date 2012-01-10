<%@ Page Language="C#" MasterPageFile="~/MasterPages/OneColumn.master" AutoEventWireup="true"
    CodeBehind="AboutDelivery.aspx.cs" Title="О Доставке" Inherits="NopCommerceStore.AboutDelivery" %>

<%@ Register TagPrefix="nopCommerce" TagName="Topic" Src="~/Modules/Topic.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="Server">
    <nopCommerce:Topic ID="topicAboutDelivery" runat="server" TopicName="AboutDelivery">
    </nopCommerce:Topic>
</asp:Content>