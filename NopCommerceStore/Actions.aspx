<%@ Page Language="C#" MasterPageFile="~/MasterPages/OneColumn.master" AutoEventWireup="true" 
CodeBehind="Actions.aspx.cs" Title="Акции" Inherits="NopCommerceStore.Actions" %>

<%@ Register TagPrefix="nopCommerce" TagName="Topic" Src="~/Modules/Topic.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="Server">
    <nopCommerce:Topic ID="topicActions" runat="server" TopicName="Actions">
    </nopCommerce:Topic>
</asp:Content>
