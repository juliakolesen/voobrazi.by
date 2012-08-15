<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WeddingBunchVariant.ascx.cs"
    Inherits="NopSolutions.NopCommerce.Web.Modules.WeddingBunchVariant" %>
<div class="design">
    <table runat="server" id="allTable" cellpadding="0" cellspacing="0">
        <tr>
            <td align="center" width="200">
                <div>
                    Подбор по параметрам:
                </div>
            </td>
            <td>
                <table runat="server" id="tblSearch" cellpadding="0" cellspacing="0" width="600">
                    <tr>
                        <td align="center">
                            Основные цветы:
                        </td>
                        <td align="center">
                            Форма букета:
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:DropDownList ID="flowers" AutoPostBack="true" runat="server" OnSelectedIndexChanged="flowers_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="bunchForm" AutoPostBack="true" runat="server" OnSelectedIndexChanged="bunchForm_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
