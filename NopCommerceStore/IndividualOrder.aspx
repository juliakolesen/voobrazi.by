<%@ Page Title="Индивидуальный заказ цветов" Language="C#" MasterPageFile="~/MasterPages/OneColumn.master"
    AutoEventWireup="true" CodeBehind="IndividualOrder.aspx.cs" Inherits="NopSolutions.NopCommerce.Web.IndividualOrder" %>

<%@ Register TagPrefix="nopCommerce" TagName="IndividualOrderControl" Src="~/Modules/IndividualOrderControl.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="Topic" Src="~/Modules/Topic.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="server">
    <nopCommerce:IndividualOrderControl runat="server" ID="indivOrderControl" />
    <asp:Label runat="server" ID="sentMessageLabel" />
    <nopCommerce:Topic ID="orderSenrTopic" runat="server" TopicName="SentOrder"/>
</asp:Content>
