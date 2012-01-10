<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchControlDefault.ascx.cs" 
Inherits="NopSolutions.NopCommerce.Web.Modules.SearchControlDefault" %>
<asp:Panel ID="Panel1" DefaultButton="btnSearch1" runat="server">
    <table runat="server" id="tblSearch" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <asp:TextBox ID="tbSearchCriteria1" runat="server" />
            </td>
            <td style="padding-left: 5px;">
                <asp:LinkButton ID="btnSearch1" runat="server" CssClass="button-search" OnCommand="btnSearch1_Click" />
            </td>
        </tr>
    </table>
</asp:Panel>
