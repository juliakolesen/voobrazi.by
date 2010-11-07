<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.ProductSpecificationFilterControl"
    CodeBehind="ProductSpecificationFilter.ascx.cs" %>
<asp:Panel runat="server" ID="pnlPSOSelector">
    <asp:Repeater ID="rptFilterByPSO" runat="server" OnItemDataBound="rptFilterByPSO_OnItemDataBound">
        <ItemTemplate>
            <%#addSpecificationAttribute()%>
            <asp:HyperLink ID="lnkFilter" runat="server">
				            <%#Server.HtmlEncode(Eval("SpecificationAttributeOptionName").ToString())%>
            </asp:HyperLink>
            <br />
        </ItemTemplate>
    </asp:Repeater>
    </div></td></tr></table>
    <asp:HiddenField runat="server" ID="hfLastSA" />
</asp:Panel>
<asp:Panel runat="server" ID="pnlAlreadyFilteredPSO">
    <p class="headers">
        Фильтр по:</p>
    <asp:Repeater ID="rptAlreadyFilteredPSO" runat="server" OnItemDataBound="rptAlreadyFilteredPSO_OnItemDataBound">
        <ItemTemplate>
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td style="padding-right: 12px; padding-bottom: 5px;">
                        <%#Server.HtmlEncode(Eval("SpecificationAttributeName").ToString())%>:
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 8px;">
                        <span class="inpL"></span><span class="textInp">
                            <%#Server.HtmlEncode(Eval("SpecificationAttributeOptionName").ToString())%></span>
                        <span class="inpR"></span>
                    </td>
                </tr>
            </table>
            <div style="clear: both;">
            </div>
        </ItemTemplate>
    </asp:Repeater>
</asp:Panel>
<asp:Panel runat="server" ID="pnlRemoveFilter">
    <br />
    <asp:HyperLink ID="hlRemoveFilter" runat="server">
            Убрать фильтр
    </asp:HyperLink>
</asp:Panel>
