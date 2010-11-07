<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.CustomerRoleDetailsControl"
    CodeBehind="CustomerRoleDetails.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="CustomerRoleInfo" Src="CustomerRoleInfo.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="CustomerRoleCustomers" Src="CustomerRoleCustomers.ascx" %>
<div class="section-header">
    <div class="title">
        <img src="Common/ico-customers.png" alt="<%=GetLocaleResourceString("Admin.CustomerRoleDetails.Title")%>" />
        <%=GetLocaleResourceString("Admin.CustomerRoleDetails.Title")%>
        <a href="CustomerRoles.aspx" title="<%=GetLocaleResourceString("Admin.CustomerRoleDetails.BackToCustomerRoles")%>">
            (<%=GetLocaleResourceString("Admin.CustomerRoleDetails.BackToCustomerRoles")%>)</a>
    </div>
    <div class="options">
        <asp:Button ID="SaveButton" runat="server" CssClass="adminButtonBlue" Text="<% $NopResources:Admin.CustomerRoleDetails.SaveButton %>"
            OnClick="SaveButton_Click" ToolTip="<% $NopResources:Admin.CustomerRoleDetails.SaveButton.Tooltip %>" />
        <asp:Button ID="DeleteButton" runat="server" CssClass="adminButtonBlue" Text="<% $NopResources:Admin.CustomerRoleDetails.DeleteButton %>"
            OnClick="DeleteButton_Click" CausesValidation="false" ToolTip="<% $NopResources:Admin.CustomerRoleDetails.DeleteButton.Tooltip %>" />
    </div>
</div>
<ajaxToolkit:TabContainer runat="server" ID="CustomerRoleTabs" ActiveTabIndex="0">
    <ajaxToolkit:TabPanel runat="server" ID="pnlCustomerRoleInfo" HeaderText="<% $NopResources:Admin.CustomerRoleDetails.RoleInfo %>">
        <ContentTemplate>
            <nopCommerce:CustomerRoleInfo ID="ctrlCustomerRoleInfo" runat="server" />
        </ContentTemplate>
    </ajaxToolkit:TabPanel>
    <ajaxToolkit:TabPanel runat="server" ID="pnlCustomers" HeaderText="<% $NopResources:Admin.CustomerRoleDetails.Customers %>">
        <ContentTemplate>
            <nopCommerce:CustomerRoleCustomers ID="ctrlCustomerRoleCustomers" runat="server" />
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
