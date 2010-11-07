<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.LanguageSelectorControl"
    CodeBehind="LanguageSelector.ascx.cs" %>
<asp:DropDownList runat="server" ID="ddlLanguages" AutoPostBack="true" OnSelectedIndexChanged="ddlLanguages_OnSelectedIndexChanged"
    SkinID="LanguageList">
</asp:DropDownList>
