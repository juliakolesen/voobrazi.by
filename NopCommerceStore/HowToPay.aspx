<%@ Page Language="C#" MasterPageFile="~/MasterPages/OneColumn.master" AutoEventWireup="true"
    CodeBehind="HowToPay.aspx.cs" Title="Как оплатить" Inherits="NopCommerceStore.HowToPay" %>

<%@ Register TagPrefix="nopCommerce" TagName="Topic" Src="~/Modules/Topic.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="Server">
    <nopCommerce:Topic ID="topicHowToPay" runat="server" TopicName="HowToPay">
    </nopCommerce:Topic>
</asp:Content>