<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.CategoryDetailsControl"
    CodeBehind="CategoryDetails.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="CategoryInfo" Src="CategoryInfo.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="CategorySEO" Src="CategorySEO.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="CategoryProduct" Src="CategoryProduct.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="CategoryDiscount" Src="CategoryDiscount.ascx" %>
<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<div class="section-header">
    <div class="title">
        <img src="Common/ico-catalog.png" alt="<%=GetLocaleResourceString("Admin.CategoryDetails.EditCategoryDetails")%>" />
        <%=GetLocaleResourceString("Admin.CategoryDetails.EditCategoryDetails")%>
        <a href="Categories.aspx" title="<%=GetLocaleResourceString("Admin.CategoryDetails.BackToCategoryList")%>">
            (<%=GetLocaleResourceString("Admin.CategoryDetails.BackToCategoryList")%>)</a>
    </div>
    <div class="options">
        <asp:Button ID="SaveButton" runat="server" CssClass="adminButtonBlue" Text="<% $NopResources:Admin.CategoryDetails.SaveButton.Text %>"
            OnClick="SaveButton_Click" ToolTip="<% $NopResources:Admin.CategoryDetails.SaveButton.ToolTip %>" />
        <asp:Button ID="DeleteButton" runat="server" CssClass="adminButtonBlue" Text="<% $NopResources:Admin.CategoryDetails.DeleteButton.Text %>"
            OnClick="DeleteButton_Click" CausesValidation="false" ToolTip="<% $NopResources:Admin.CategoryDetails.DeleteButton.ToolTip %>" />
    </div>
</div>
<ajaxToolkit:TabContainer runat="server" ID="CategoryTabs" ActiveTabIndex="0">
    <ajaxToolkit:TabPanel runat="server" ID="pnlCategoryInfo" HeaderText="<% $NopResources: Admin.CategoryDetails.CategoryInfo%>">
        <ContentTemplate>
            <nopCommerce:CategoryInfo ID="ctrlCategoryInfo" runat="server" />
        </ContentTemplate>
    </ajaxToolkit:TabPanel>
    <ajaxToolkit:TabPanel runat="server" ID="pnlCategorySEO" HeaderText="<% $NopResources: Admin.CategoryDetails.SEO%>">
        <ContentTemplate>
            <nopCommerce:CategorySEO ID="ctrlCategorySEO" runat="server" />
        </ContentTemplate>
    </ajaxToolkit:TabPanel>
    <ajaxToolkit:TabPanel runat="server" ID="pnlProductCategoryMappings" HeaderText="<% $NopResources:Admin.CategoryDetails.Products %>">
        <ContentTemplate>
            <nopCommerce:CategoryProduct ID="ctrlCategoryProduct" runat="server" />
        </ContentTemplate>
    </ajaxToolkit:TabPanel>
    <ajaxToolkit:TabPanel runat="server" ID="pnlDiscountMappings" HeaderText="<% $NopResources:Admin.CategoryDetails.Discounts %>">
        <ContentTemplate>
            <nopCommerce:CategoryDiscount ID="ctrlCategoryDiscount" runat="server" />
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
        <p>
        </p>
        <asp:Button ID="deleteButtonOk" runat="server" Text="<% $NopResources:Admin.Common.Yes %>" CssClass="adminButton" CausesValidation="false" />
        <asp:Button ID="deleteButtonCancel" runat="server" Text="<% $NopResources:Admin.Common.No %>" CssClass="adminButton"
            CausesValidation="false" />
    </div>
</asp:Panel>
