<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/OneColumn.master"
    CodeBehind="SiteMap.aspx.cs" Inherits="NopSolutions.NopCommerce.Web.SiteMap" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cph1">
    <ul class="sitemap">
        <asp:Literal runat="server" ID="lSitemapContent" EnableViewState="false" />
    </ul>
</asp:Content>
