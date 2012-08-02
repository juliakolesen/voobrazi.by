<%@ Page Language="C#" MasterPageFile="~/MasterPages/OneColumn.master" AutoEventWireup="true"
    CodeBehind="FlowersWholesale.aspx.cs" Title="Цветы оптом" Inherits="NopCommerceStore.FlowersWholesale" %>

<%@ Register TagPrefix="nopCommerce" TagName="Topic" Src="~/Modules/Topic.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="Server">
    <nopCommerce:Topic ID="topicFlowersWholesale" runat="server" TopicName="FlowersWholesale">
    </nopCommerce:Topic>
</asp:Content>