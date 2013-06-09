<%@ Page Language="C#" MasterPageFile="~/MasterPages/OneColumn.master" AutoEventWireup="true" 
CodeBehind="NewsAll.aspx.cs" Title="Новости" Inherits="NopCommerceStore.NewsAll" %>


<%@ Register TagPrefix="nopCommerce" TagName="NewsList" Src="~/Modules/NewsList.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="Server">
    <nopCommerce:NewsList ID="newsList" runat="server">
    </nopCommerce:NewsList>
</asp:Content>