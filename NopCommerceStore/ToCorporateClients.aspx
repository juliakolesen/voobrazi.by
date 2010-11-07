<%@ Page Language="C#" MasterPageFile="~/MasterPages/OneColumn.master" AutoEventWireup="true"
    CodeBehind="ToCorporateClients.aspx.cs" Title="Корпоративным клиентам" Inherits="NopCommerceStore.ToCorporateClients" %>

<%@ Register TagPrefix="nopCommerce" TagName="Topic" Src="~/Modules/Topic.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="Server">
    <nopCommerce:Topic ID="topicToCorporateClients" runat="server" TopicName="ToCorporateClients">
    </nopCommerce:Topic>
</asp:Content>
