<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.CustomerDetailsControl"
    CodeBehind="CustomerDetails.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="CustomerInfo" Src="CustomerInfo.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="CustomerBillingAddresses" Src="CustomerBillingAddresses.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="CustomerShippingAddresses" Src="CustomerShippingAddresses.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="CustomerOrders" Src="CustomerOrders.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="CustomerRoleMappings" Src="CustomerRoleMappings.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="CustomerShoppingCart" Src="CustomerShoppingCart.ascx" %>
<div class="section-header">
    <div class="title">
        <img src="Common/ico-customers.png" alt="<%=GetLocaleResourceString("Admin.CustomerDetails.Title")%>" />
        <%=GetLocaleResourceString("Admin.CustomerDetails.Title")%>
        <a href="Customers.aspx" title="<%=GetLocaleResourceString("Admin.CustomerDetails.BackToCustomers")%>">
            (<%=GetLocaleResourceString("Admin.CustomerDetails.BackToCustomers")%>)</a>
    </div>
    <div class="options">
        <asp:Button ID="SaveButton" runat="server" CssClass="adminButtonBlue" Text="<% $NopResources:Admin.CustomerDetails.SaveButton %>"
            OnClick="SaveButton_Click" ToolTip="<% $NopResources:Admin.CustomerDetails.SaveButton.Tooltip %>" />
        <asp:Button ID="DeleteButton" runat="server" CssClass="adminButtonBlue" Text="<% $NopResources:Admin.CustomerDetails.DeleteButton %>"
            OnClick="DeleteButton_Click" CausesValidation="false" ToolTip="<% $NopResources:Admin.CustomerDetails.DeleteButton.Tooltip %>" />
    </div>
</div>
<ajaxToolkit:TabContainer runat="server" ID="CustomerTabs" ActiveTabIndex="0">
    <ajaxToolkit:TabPanel runat="server" ID="pnlCustomerInfo" HeaderText="<% $NopResources:Admin.CustomerDetails.CustomerInfo %>">
        <ContentTemplate>
            <nopCommerce:CustomerInfo runat="server" ID="ctrlCustomerInfo"></nopCommerce:CustomerInfo>
        </ContentTemplate>
    </ajaxToolkit:TabPanel>
    <ajaxToolkit:TabPanel runat="server" ID="pnlCustomerBillingAddresses" HeaderText="<% $NopResources:Admin.CustomerDetails.BillingAddresses %>">
        <ContentTemplate>
            <nopCommerce:CustomerBillingAddresses runat="server" ID="ctrlCustomerBillingAddresses">
            </nopCommerce:CustomerBillingAddresses>
        </ContentTemplate>
    </ajaxToolkit:TabPanel>
    <ajaxToolkit:TabPanel runat="server" ID="pnlCustomerShippingAddresses" HeaderText="<% $NopResources:Admin.CustomerDetails.ShippingAddresses %>">
        <ContentTemplate>
            <nopCommerce:CustomerShippingAddresses runat="server" ID="ctrlCustomerShippingAddresses">
            </nopCommerce:CustomerShippingAddresses>
        </ContentTemplate>
    </ajaxToolkit:TabPanel>
    <ajaxToolkit:TabPanel runat="server" ID="pnlCustomerOrders" HeaderText="<% $NopResources:Admin.CustomerDetails.Orders %>">
        <ContentTemplate>
            <nopCommerce:CustomerOrders runat="server" ID="ctrlCustomerOrders"></nopCommerce:CustomerOrders>
        </ContentTemplate>
    </ajaxToolkit:TabPanel>
    <ajaxToolkit:TabPanel runat="server" ID="pnlCustomerRoleMappings" HeaderText="<% $NopResources:Admin.CustomerDetails.Roles %>">
        <ContentTemplate>
            <nopCommerce:CustomerRoleMappings runat="server" ID="ctrlCustomerRoleMappings"></nopCommerce:CustomerRoleMappings>
        </ContentTemplate>
    </ajaxToolkit:TabPanel>
    <ajaxToolkit:TabPanel runat="server" ID="pnlCustomerShoppingCart" HeaderText="<% $NopResources:Admin.CustomerDetails.CurrentCart %>">
        <ContentTemplate>
            <nopCommerce:CustomerShoppingCart runat="server" ID="ctrlCurrentShoppingCart"></nopCommerce:CustomerShoppingCart>
        </ContentTemplate>
    </ajaxToolkit:TabPanel>
</ajaxToolkit:TabContainer>
<ajaxToolkit:ConfirmButtonExtender ID="ConfirmDeleteButtonExtender" runat="server"
    TargetControlID="DeleteButton" DisplayModalPopupID="ModalPopupExtenderDelete" />
<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderDelete" runat="server" TargetControlID="DeleteButton"
    PopupControlID="pnlDeletePopup" OkControlID="deleteButtonOk" CancelControlID="deleteButtonCancel"
    BackgroundCssClass="modalBackground" />
<asp:Panel ID="pnlDeletePopup" runat="server" Style="display: none; width: 250px;
    background-color: White; border-width: 2px; border-color: Black; border-style: solid;
    padding: 20px;">
    <div style="text-align: center;">
        <%=GetLocaleResourceString("Admin.Common.AreYouSure")%>
        <br />
        <br />
        <asp:Button ID="deleteButtonOk" runat="server" Text="<% $NopResources:Admin.Common.Yes %>" CssClass="adminButton" CausesValidation="false" />
        <asp:Button ID="deleteButtonCancel" runat="server" Text="<% $NopResources:Admin.Common.No %>" CssClass="adminButton"
            CausesValidation="false" />
    </div>
</asp:Panel>
