<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.CurrencyDetailsControl"
    CodeBehind="CurrencyDetails.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="CurrencyInfo" Src="CurrencyInfo.ascx" %>
<div class="section-header">
    <div class="title">
        <img src="Common/ico-configuration.png" alt="<%=GetLocaleResourceString("Admin.Currencies.CurrencyDetails.Title")%>" />
        <%=GetLocaleResourceString("Admin.Currencies.CurrencyDetails.Title")%>
        <a href="Currencies.aspx" title="<%=GetLocaleResourceString("Admin.Currencies.CurrencyDetails.BackToCurrencies")%>">
            (<%=GetLocaleResourceString("Admin.Currencies.CurrencyDetails.BackToCurrencies")%>)</a>
    </div>
    <div class="options">
        <asp:Button ID="SaveButton" runat="server" CssClass="adminButtonBlue" Text="<% $NopResources:Admin.Currencies.CurrencyDetails.SaveButton.Text %>"
            OnClick="SaveButton_Click" ToolTip="<% $NopResources:Admin.Currencies.CurrencyDetails.SaveButton.Tooltip %>" />
        <asp:Button ID="DeleteButton" runat="server" CssClass="adminButtonBlue" Text="<% $NopResources:Admin.Currencies.CurrencyDetails.DeleteButton.Text %>"
            OnClick="DeleteButton_Click" CausesValidation="false" ToolTip="<% $NopResources:Admin.Currencies.CurrencyDetails.DeleteButton.Tooltip %>" />
    </div>
</div>
<nopCommerce:CurrencyInfo ID="ctrlCurrencyInfo" runat="server" />
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
