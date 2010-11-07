<%@ Page Language="C#" MasterPageFile="~/MasterPages/OneColumn.master" AutoEventWireup="true"
    CodeBehind="AboutSalon.aspx.cs" Title="О салоне цветов “Воображение”" Inherits="NopCommerceStore.AboutSalon" %>

<%@ Register TagPrefix="nopCommerce" TagName="Topic" Src="~/Modules/Topic.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="Server">
    <nopCommerce:Topic ID="topicAboutSalon" runat="server" TopicName="AboutSalon">
    </nopCommerce:Topic>
</asp:Content>
