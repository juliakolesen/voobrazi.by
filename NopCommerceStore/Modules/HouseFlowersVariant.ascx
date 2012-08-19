<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HouseFlowersVariant.ascx.cs"
    Inherits="NopSolutions.NopCommerce.Web.Modules.HouseFlowersVariant" %>
<div class="design">
    <table runat="server" id="allTable" cellpadding="0" cellspacing="0">
        <tr>
            <td align="center" width="200">
                <div>
                    Требования к освещению:
                </div>
            </td>
            <td align="center">
                <asp:DropDownList ID="light" AutoPostBack="true" runat="server" OnSelectedIndexChanged="light_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
</div>
