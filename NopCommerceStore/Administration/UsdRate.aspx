<%@ Page Language="C#"  MasterPageFile="~/Administration/main.master" AutoEventWireup="true" CodeBehind="UsdRate.aspx.cs" Inherits="NopCommerceStore.Administration.UsdRate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="server">
    <asp:ValidationSummary runat="server" ShowSummary="true" EnableClientScript="true" />

    Курс доллара к белорусскому рублю:

    <asp:TextBox runat="server" ID="tbUsdRate" Text="0" />
    <asp:RegularExpressionValidator runat="server" ControlToValidate="tbUsdRate" EnableClientScript="true" 
        ValidationExpression="\d*" ErrorMessage="Значение должно быть целым числом." Display="None" />
    <asp:Button ID="btnUpdate" runat="server" Text="Обновить" OnClick="BtnUpdateClick" CausesValidation="true" />
</asp:Content>
