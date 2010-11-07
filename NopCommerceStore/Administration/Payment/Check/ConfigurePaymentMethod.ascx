<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="NopSolutions.NopCommerce.Web.Administration.Payment.Check.ConfigurePaymentMethod" Codebehind="ConfigurePaymentMethod.ascx.cs" %>
<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<table class="adminContent">
    <tr>
        <td colspan="2" width="100%">
            <hr />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <b>Enter info that will be shown to customers during checkout:</b>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <FCKeditorV2:FCKeditor ID="txtInfo" runat="server" AutoDetectLanguage="false" Height="350"
                >
            </FCKeditorV2:FCKeditor>
        </td>
    </tr>
</table>
