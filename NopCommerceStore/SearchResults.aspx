<%@ Page Language="C#" MasterPageFile="~/MasterPages/OneColumn.master" AutoEventWireup="true"
     Inherits="NopSolutions.NopCommerce.Web.SearchResults" CodeBehind="SearchResults.aspx.cs" %>

<%@ Register TagPrefix="nopCommerce" TagName="SearchResultsControl" Src="~/Modules/SearchResultsControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="server">
    <nopCommerce:SearchResultsControl runat="server" ID="ctrlProducts" />
</asp:Content>