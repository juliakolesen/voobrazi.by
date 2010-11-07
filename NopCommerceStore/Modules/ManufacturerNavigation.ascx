<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.ManufacturerNavigation"
    CodeBehind="ManufacturerNavigation.ascx.cs" %>
<div class="manufacturer-navigation">
    <div class="title">
        <%=GetLocaleResourceString("Manufacturer.Manufacturers")%>
    </div>
    <div class="clear">
    </div>
    <div class="listbox">
        <asp:Repeater ID="rptrManufacturers" runat="server" OnItemDataBound="rptrManufacturers_ItemDataBound">
            <HeaderTemplate>
                <ul>
            </HeaderTemplate>
            <ItemTemplate>
                <li>
                    <asp:HyperLink ID="hlManufacturer" runat="server" Text='<%#Server.HtmlEncode(Eval("Name").ToString()) %>'
                        CssClass='<%# ((int)Eval("ManufacturerID") == this.ManufacturerID) ? "active" : "inactive" %>' />
                </li>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
    </div>
</div>
