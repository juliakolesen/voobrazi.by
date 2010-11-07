<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.ProductAttributeDetailsControl"
    CodeBehind="ProductAttributeDetails.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="ProductAttributeInfo" Src="ProductAttributeInfo.ascx" %>
<div class="section-header">
    <div class="title">
        <img src="Common/ico-catalog.png" alt="<%=GetLocaleResourceString("Admin.ProductAttributeDetails.Title")%>" />
        <%=GetLocaleResourceString("Admin.ProductAttributeDetails.Title")%><a href="ProductAttributes.aspx"
            title="<%=GetLocaleResourceString("Admin.ProductAttributeDetails.BackToAttributeList")%>">
            (<%=GetLocaleResourceString("Admin.ProductAttributeDetails.BackToAttributeList")%>)</a>
    </div>
    <div class="options">
        <asp:Button ID="SaveButton" runat="server" CssClass="adminButtonBlue" Text="<% $NopResources:Admin.ProductAttributeDetails.SaveButton.Text %>"
            OnClick="SaveButton_Click" ToolTip="<% $NopResources:Admin.ProductAttributeDetails.SaveButton.Tooltip %>" />
        <asp:Button ID="DeleteButton" runat="server" CssClass="adminButtonBlue" Text="<% $NopResources:Admin.ProductAttributeDetails.DeleteButton.Text %>"
            OnClick="DeleteButton_Click" CausesValidation="false" ToolTip="<% $NopResources:Admin.ProductAttributeDetails.DeleteButton.Tooltip %>" />
    </div>
</div>
<nopCommerce:ProductAttributeInfo ID="ctrlProductAttributeInfo" runat="server" />
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
