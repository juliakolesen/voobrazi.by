<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.AffiliateDetailsControl"
    CodeBehind="AffiliateDetails.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="AffiliateInfo" Src="AffiliateInfo.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="AffiliateCustomers" Src="AffiliateCustomers.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="AffiliateOrders" Src="AffiliateOrders.ascx" %>
<div class="section-header">
    <div class="title">
        <img src="Common/ico-promotions.png" alt="<%=GetLocaleResourceString("Admin.AffiliateDetails.Title")%>" />
        <%=GetLocaleResourceString("Admin.AffiliateDetails.Title")%>
        <a href="Affiliates.aspx" title="<%=GetLocaleResourceString("Admin.AffiliateDetails.BackToAffiliates")%>">
            (<%=GetLocaleResourceString("Admin.AffiliateDetails.BackToAffiliates")%>)</a>
    </div>
    <div class="options">
        <asp:Button ID="SaveButton" runat="server" CssClass="adminButtonBlue" Text="<% $NopResources:Admin.AffiliateDetails.SaveButton.Text %>"
            OnClick="SaveButton_Click" ToolTip="<% $NopResources:Admin.AffiliateDetails.SaveButton.Tooltip %>" />
        <asp:Button ID="DeleteButton" runat="server" CssClass="adminButtonBlue" Text="<% $NopResources:Admin.AffiliateDetails.DeleteButton.Text %>"
            OnClick="DeleteButton_Click" CausesValidation="false" ToolTip="<% $NopResources:Admin.AffiliateDetails.DeleteButton.Tooltip %>" />
    </div>
</div>
<ajaxToolkit:TabContainer runat="server" ID="AffiliateTabs" ActiveTabIndex="0">
    <ajaxToolkit:TabPanel runat="server" ID="pnlAffiliateInfo" HeaderText="<% $NopResources:Admin.AffiliateDetails.AffiliateInfo %>">
        <ContentTemplate>
            <nopCommerce:AffiliateInfo ID="ctrlAffiliateInfo" runat="server" />
        </ContentTemplate>
    </ajaxToolkit:TabPanel>
    <ajaxToolkit:TabPanel runat="server" ID="pnlAffiliatedCustomers" HeaderText="<% $NopResources:Admin.AffiliateDetails.Customers %>">
        <ContentTemplate>
            <nopCommerce:AffiliateCustomers ID="ctrlAffiliateCustomers" runat="server" />
        </ContentTemplate>
    </ajaxToolkit:TabPanel>
    <ajaxToolkit:TabPanel runat="server" ID="pnlAffiliateOrders" HeaderText="<% $NopResources:Admin.AffiliateDetails.Orders %>">
        <ContentTemplate>
            <nopCommerce:AffiliateOrders ID="ctrlAffiliateOrders" runat="server" />
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
