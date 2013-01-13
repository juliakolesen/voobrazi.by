<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HouseFlowersVariant.ascx.cs"
    Inherits="NopSolutions.NopCommerce.Web.Modules.HouseFlowersVariant" %>
<%@ Register TagPrefix="nopCommerce" TagName="ProductSize" Src="~/Modules/ProductSize.ascx" %>
<div class="design">
    <table runat="server" id="allTable" cellpadding="0" cellspacing="0">
        <tr>
            <td width="480">
                <nopCommerce:ProductSize ID="productSize" runat="server" />
            </td>
            <td align="center" width="200">
                <div>
                    Требования к освещению:
                    <asp:DropDownList ID="light" AutoPostBack="true" runat="server" OnSelectedIndexChanged="light_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
            </td>
        </tr>
    </table>
</div>
