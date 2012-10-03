<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ColorsFilterControl.ascx.cs"
    Inherits="NopSolutions.NopCommerce.Web.Modules.ColorsFilterControl" %>
<%@ Register TagPrefix="nopCommerce" TagName="ColorCell" Src="~/Modules/ColorCell.ascx" %>
<center>
    <asp:Label ID="colorsLabel" runat="server" Text="Выбранный цвет:"></asp:Label>
    <table>
        <tr>
            <td>
                <asp:DataList ID="rptColors" runat="server" RepeatDirection="Horizontal">
                    <ItemTemplate>
                        <nopCommerce:ColorCell ID="cell" runat="server" ColorItem='<%# Container.DataItem %>' />
                    </ItemTemplate>
                </asp:DataList>
            </td>
            <td>
                <asp:ImageButton ID="colorFilterDefault" runat="server" OnClick="onclick" ImageUrl="~/images/unchecked.gif"
                    ToolTip="Фильтр по цветам неактивен" />
            </td>
        </tr>
    </table>
</center>
