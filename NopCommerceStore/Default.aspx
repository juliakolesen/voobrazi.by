<%@ Page Language="C#" MasterPageFile="~/MasterPages/OneColumnHome.master" AutoEventWireup="true"
    Inherits="NopSolutions.NopCommerce.Web.Default" Title="Главная" CodeBehind="Default.aspx.cs" %>

<%@ Register TagPrefix="nopCommerce" TagName="Topic" Src="~/Modules/Topic.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="Server">
    <nopCommerce:Topic ID="topicAboutUs" runat="server" TopicName="AboutUs" />
</asp:Content>
