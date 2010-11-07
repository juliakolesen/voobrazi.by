<%@ Page Language="C#" MasterPageFile="~/Administration/main.master" AutoEventWireup="true"
    Inherits="NopSolutions.NopCommerce.Web.Administration.Administration_SpecificationAttributeOptionAdd"
    CodeBehind="SpecificationAttributeOptionAdd.aspx.cs" ValidateRequest="false" %>

<%@ Register TagPrefix="nopCommerce" TagName="SpecificationAttributeOptionAdd" Src="Modules/SpecificationAttributeOptionAdd.ascx" %>
<asp:Content ID="c1" ContentPlaceHolderID="cph1" runat="Server">
    <nopCommerce:SpecificationAttributeOptionAdd runat="server" ID="ctrlSpecificationAttributeOptionAdd" />
</asp:Content>
