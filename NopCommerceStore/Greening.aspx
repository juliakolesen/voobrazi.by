<%@ Page MasterPageFile="~/MasterPages/OneColumn.master" Language="C#" AutoEventWireup="true"
 CodeBehind="Greening.aspx.cs" Title="Озеленение, оформление цветами" Inherits="NopCommerceStore.Greening" %>

<%@ Register TagPrefix="nopCommerce" TagName="Topic" Src="~/Modules/Topic.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="Server">
    <nopCommerce:Topic ID="topicGreening" runat="server" TopicName="Greening">
    </nopCommerce:Topic>
</asp:Content>
