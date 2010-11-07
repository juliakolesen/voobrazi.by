<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="NopSolutions.NopCommerce.Web.Modules.CustomerAddressesControl" Codebehind="CustomerAddresses.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="AddressDisplay" Src="~/Modules/AddressDisplay.ascx" %>
<div class="CustomerAddresses">
    <div class="SectionTitle">
        <%=GetLocaleResourceString("Account.BillingAddressBookEntries")%></div>
    <div class="clear">
    </div>
    <div class="AdressList">
        <asp:Repeater ID="rptrBillingAddresses" runat="server">
            <ItemTemplate>
                <div class="AddressItem">
                    <nopCommerce:AddressDisplay ID="AddressDisplayCtrl" runat="server" Address='<%# Container.DataItem %>'
                        ShowEditButton="true" ShowDeleteButton="true" />
                </div>
            </ItemTemplate>
            <SeparatorTemplate>
                <div class="clear">
                </div>
            </SeparatorTemplate>
        </asp:Repeater>
    </div>
    <div class="clear">
    </div>
    <div class="AddButton">
        <asp:Button ID="btnAddBillingAddress" OnClick="btnAddBillingAddress_Click" runat="server"
            Text="<% $NopResources:Account.AddBillingAddress %>" ValidationGroup="BillingAddresses" SkinID="AddBillingAddressButton" />
    </div>
    <div class="SectionTitle">
        <%=GetLocaleResourceString("Account.ShippingAddressBookEntries")%></div>
    <div class="AdressList">
        <asp:Repeater ID="rptrShippingAddresses" runat="server">
            <ItemTemplate>
                <div class="AddressItem">
                    <nopCommerce:AddressDisplay ID="AddressDisplayCtrl" runat="server" Address='<%# Container.DataItem %>'
                        ShowEditButton="true" ShowDeleteButton="true" />
                </div>
            </ItemTemplate>
            <SeparatorTemplate>
                <div class="clear">
                </div>
            </SeparatorTemplate>
        </asp:Repeater>
    </div>
    <div class="AddButton">
        <asp:Button ID="btnAddShippingAddress" OnClick="btnAddShippingAddress_Click" runat="server"
            Text="<% $NopResources:Account.AddShippingAddress %>" ValidationGroup="ShippingAddresses" SkinID="AddShippingAddressButton" />
    </div>
    <div class="clear">
    </div>
</div>
