<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="NopSolutions.NopCommerce.Web.Administration.Shipping.FixedRateConfigure.ConfigureShipping" Codebehind="ConfigureShipping.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="DecimalTextBox" Src="../../Modules/DecimalTextBox.ascx" %>
<table class="adminContent">
    <tr>
        <td colspan="2" width="100%">
            <hr />
        </td>
    </tr>
    <tr>
    <tr>
        <td class="adminTitle">
            Fixed shipping rate [<%=CurrencyManager.PrimaryStoreCurrency.CurrencyCode%>]:
        </td>
        <td class="adminData">
            <nopCommerce:DecimalTextBox runat="server" ID="txtFixedRate" Value="0"
                RequiredErrorMessage="Fixed rate is required" MinimumValue="0" MaximumValue="999999"
                RangeErrorMessage="The value must be from 0 to 999999" CssClass="adminInput">
            </nopCommerce:DecimalTextBox>
        </td>
    </tr>
</table>
