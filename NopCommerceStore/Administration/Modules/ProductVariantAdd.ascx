<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.ProductVariantAddControl"
    CodeBehind="ProductVariantAdd.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="ProductVariantInfo" Src="ProductVariantInfo.ascx" %>
<div class="section-header">
    <div class="title">
        <img src="Common/ico-catalog.png" alt="<%=GetLocaleResourceString("Admin.ProductVariantAdd.AddNewProductVariant")%>" />
        <%=GetLocaleResourceString("Admin.ProductVariantAdd.AddNewProductVariant")%>:
        <asp:Label ID="lblProductName" runat="server" />
        <asp:HyperLink runat="server" ID="hlProductURL" Text="<% $NopResources:Admin.ProductVariantAdd.BackToProductDetails %>" />
    </div>
    <div class="options">
        <asp:Button ID="SaveButton" runat="server" Text="<% $NopResources:Admin.ProductVariantAdd.SaveButton.Text %>"
            CssClass="adminButtonBlue" OnClick="SaveButton_Click" ToolTip="<% $NopResources:Admin.ProductVariantAdd.SaveButton.Tooltip %>" />
    </div>
</div>
<ajaxToolkit:TabContainer runat="server" ID="ProductVariantTabs" ActiveTabIndex="0">
    <ajaxToolkit:TabPanel runat="server" ID="pnlProductVariantInfo" HeaderText="<% $NopResources:Admin.ProductVariantAdd.ProductVariantInfo %>">
        <ContentTemplate>
            <nopCommerce:ProductVariantInfo runat="server" ID="ctrlProductVariantInfo" />
        </ContentTemplate>
    </ajaxToolkit:TabPanel>
</ajaxToolkit:TabContainer>