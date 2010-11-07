<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.ProductDetailsControl"
    CodeBehind="ProductDetails.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="ProductInfoEdit" Src="ProductInfoEdit.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="ProductVariants" Src="ProductVariants.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="ProductSEO" Src="ProductSEO.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="ProductCategory" Src="ProductCategory.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="ProductManufacturer" Src="ProductManufacturer.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="RelatedProducts" Src="RelatedProducts.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="ProductPictures" Src="ProductPictures.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="ProductSpecifications" Src="ProductSpecifications.ascx" %>
<div class="section-header">
    <div class="title">
        <img src="Common/ico-catalog.png" alt="<%=GetLocaleResourceString("Admin.ProductDetails.EditProductDetails")%>" />
        <%=GetLocaleResourceString("Admin.ProductDetails.EditProductDetails")%>
        <a href="Products.aspx" title="<%=GetLocaleResourceString("Admin.ProductDetails.BackToProductList")%>">
            (<%=GetLocaleResourceString("Admin.ProductDetails.BackToProductList")%>)</a>
    </div>
    <div class="options">
        <asp:Button ID="SaveButton" runat="server" CssClass="adminButtonBlue" Text="<% $NopResources:Admin.ProductDetails.SaveButton.Text %>"
            OnClick="SaveButton_Click" ToolTip="<% $NopResources:Admin.ProductDetails.SaveButton.Tooltip %>" />
        <asp:Button ID="DeleteButton" runat="server" CssClass="adminButtonBlue" Text="<% $NopResources:Admin.ProductDetails.DeleteButton.Text %>"
            OnClick="DeleteButton_Click" CausesValidation="false" ToolTip="<% $NopResources:Admin.ProductDetails.DeleteButton.Tooltip %>" />
    </div>
</div>
<ajaxToolkit:TabContainer runat="server" ID="ProductTabs" ActiveTabIndex="0">
    <ajaxToolkit:TabPanel runat="server" ID="pnlProductInfo" HeaderText="<% $NopResources:Admin.ProductDetails.ProductInfo %>">
        <ContentTemplate>
            <nopCommerce:ProductInfoEdit ID="ctrlProductInfoEdit" runat="server" />
        </ContentTemplate>
    </ajaxToolkit:TabPanel>
    <ajaxToolkit:TabPanel runat="server" ID="pnlProductSEO" HeaderText="<% $NopResources:Admin.ProductDetails.SEO %>">
        <ContentTemplate>
            <nopCommerce:ProductSEO ID="ctrlProductSEO" runat="server" />
        </ContentTemplate>
    </ajaxToolkit:TabPanel>
    <ajaxToolkit:TabPanel runat="server" ID="pnlProductVariants" HeaderText="<% $NopResources:Admin.ProductDetails.ProductVariants %>">
        <ContentTemplate>
            <nopCommerce:ProductVariants ID="ctrlProductVariants" runat="server" />
        </ContentTemplate>
    </ajaxToolkit:TabPanel>
    <ajaxToolkit:TabPanel runat="server" ID="pnlCategoryMappings" HeaderText="<% $NopResources:Admin.ProductDetails.CategoryMappings %>">
        <ContentTemplate>
            <nopCommerce:ProductCategory ID="ctrlProductCategory" runat="server" />
        </ContentTemplate>
    </ajaxToolkit:TabPanel>
    <ajaxToolkit:TabPanel runat="server" ID="pnlManufacturerMappings" HeaderText="<% $NopResources:Admin.ProductDetails.ManufacturerMappings %>">
        <ContentTemplate>
            <nopCommerce:ProductManufacturer ID="ctrlProductManufacturer" runat="server" />
        </ContentTemplate>
    </ajaxToolkit:TabPanel>
    <ajaxToolkit:TabPanel runat="server" ID="pnlRelatedProducts" HeaderText="<% $NopResources:Admin.ProductDetails.RelatedProducts %>">
        <ContentTemplate>
            <nopCommerce:RelatedProducts ID="ctrlRelatedProducts" runat="server" />
        </ContentTemplate>
    </ajaxToolkit:TabPanel>
    <ajaxToolkit:TabPanel runat="server" ID="pnlPictures" HeaderText="<% $NopResources:Admin.ProductDetails.Pictures %>">
        <ContentTemplate>
            <nopCommerce:ProductPictures ID="ctrlProductPictures" runat="server" />
        </ContentTemplate>
    </ajaxToolkit:TabPanel>
    <ajaxToolkit:TabPanel runat="server" ID="pnlProductSpecification" HeaderText="<% $NopResources:Admin.ProductDetails.ProductSpecification %>">
        <ContentTemplate>
            <nopCommerce:ProductSpecifications ID="ctrlProductSpecifications" runat="server" />
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
