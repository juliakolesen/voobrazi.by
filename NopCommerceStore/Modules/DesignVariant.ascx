<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DesignVariant.ascx.cs"
    Inherits="NopSolutions.NopCommerce.Web.Modules.DesignVariant" %>
<div class="design">
    <table runat="server" id="allTable" cellpadding="0" cellspacing="0">
        <tr>
            <td align="center" width="100">
                <div>
                    Вариант оформления цветов
                </div>
            </td>
            <td>
                <table runat="server" id="tblSearch" cellpadding="0" cellspacing="0" width="600">
                    <tr>
                        <td align="center">
                            Без оформления:
                        </td>
                        <td align="center">
                            Упаковка:
                        </td>
                        <td align="center">
                            Букет:
                        </td>
                        <td align="center">
                            Композиция:
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:DropDownList ID="visoutDesign" AutoPostBack="true" runat="server" OnSelectedIndexChanged="visoutDesign_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="wrapping" AutoPostBack="true" runat="server" OnSelectedIndexChanged="wrapping_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="bunch" AutoPostBack="true" runat="server" OnSelectedIndexChanged="bunch_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="composition" AutoPostBack="true" runat="server" OnSelectedIndexChanged="composition_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
