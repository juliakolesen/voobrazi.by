<%@ Page Language="C#" MasterPageFile="~/MasterPages/OneColumn.master" AutoEventWireup="true"
    Inherits="NopSolutions.NopCommerce.Web.RegisterPage" Codebehind="Register.aspx.cs" %>

<%@ Register TagPrefix="nopCommerce" TagName="CustomerRegister" Src="~/Modules/CustomerRegister.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="Server">
    <nopCommerce:CustomerRegister ID="ctrlCustomerRegister" runat="server" />
</asp:Content>
