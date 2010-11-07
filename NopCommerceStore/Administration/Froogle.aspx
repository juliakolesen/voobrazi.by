<%@ Page Language="C#" MasterPageFile="~/Administration/main.master" AutoEventWireup="true"
    Inherits="NopSolutions.NopCommerce.Web.Administration.Administration_Froogle"
    CodeBehind="Froogle.aspx.cs" %>

<%@ Register TagPrefix="nopCommerce" TagName="Froogle" Src="Modules/Froogle.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="server">
    <nopCommerce:Froogle runat="server" ID="ctrlFroogle" />
</asp:Content>
