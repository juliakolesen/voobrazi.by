<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BlacklistNetworkDetails.ascx.cs"
    Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.BlacklistNetworkDetailsControl" %>
<%@ Register TagPrefix="nopCommerce" TagName="BlacklistNetworkInfo" Src="BlacklistNetworkInfo.ascx" %>
<div class="section-header">
    <div class="title">
        <img src="Common/ico-blacklist.png" alt="<%=GetLocaleResourceString("Admin.BlacklistNetworkDetails.Title")%>" />
        <%=GetLocaleResourceString("Admin.BlacklistNetworkDetails.Title")%>
        <a href="Blacklist.aspx" title="<%=GetLocaleResourceString("Admin.BlacklistNetworkDetails.BackToBlacklist")%>">
            (<%=GetLocaleResourceString("Admin.BlacklistNetworkDetails.BackToBlacklist")%>)</a>
    </div>
    <div class="options">
        <asp:Button ID="btnSave" runat="server" CssClass="adminButtonBlue" Text="<% $NopResources:Admin.BlacklistNetworkDetails.SaveButton.Text %>"
            OnClick="OnSaveClick" ToolTip="<% $NopResources:Admin.BlacklistNetworkDetails.SaveButton.Tooltip %>" />
        <asp:Button ID="btnDelete" runat="server" CssClass="adminButtonBlue" Text="<% $NopResources:Admin.BlacklistNetworkDetails.DeleteButton.Text %>"
            OnClick="OnDeleteClick" CausesValidation="false" ToolTip="<% $NopResources:Admin.BlacklistNetworkDetails.DeleteButton.Tooltip %>" />
    </div>
</div>
<p>
</p>
<nopCommerce:BlacklistNetworkInfo ID="ctrlBlacklist" runat="server" />
<ajaxToolkit:ConfirmButtonExtender ID="ConfirmDeleteButtonExtender" runat="server"
    TargetControlID="btnDelete" DisplayModalPopupID="ModalPopupExtenderDelete" />
<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderDelete" runat="server" TargetControlID="btnDelete"
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
