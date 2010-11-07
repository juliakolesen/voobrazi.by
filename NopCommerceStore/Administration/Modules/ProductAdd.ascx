<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.ProductAddControl"
    CodeBehind="ProductAdd.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="ProductInfoAdd" Src="ProductInfoAdd.ascx" %>
<div class="section-header">
    <div class="title">
        <img src="Common/ico-catalog.png" alt="<%=GetLocaleResourceString("Admin.ProductAdd.AddNewProduct")%>" />
        <%=GetLocaleResourceString("Admin.ProductAdd.AddNewProduct")%>
        <a href="Products.aspx" title="<%=GetLocaleResourceString("Admin.ProductAdd.BackToProductList")%>">
            (<%=GetLocaleResourceString("Admin.ProductAdd.BackToProductList")%>)</a>
    </div>
    <div class="options">
        <asp:Button ID="SaveButton" runat="server" Text="<% $NopResources:Admin.ProductAdd.SaveButton.Text %>"
            CssClass="adminButtonBlue" OnClick="SaveButton_Click" ToolTip="<% $NopResources:Admin.ProductAdd.SaveButton.Tooltip %>" />
    </div>
</div>
<ajaxToolkit:TabContainer runat="server" ID="ProductTabs" ActiveTabIndex="0">
    <ajaxToolkit:TabPanel runat="server" ID="pnlProductInfo" HeaderText="<% $NopResources:Admin.ProductAdd.ProductInfo %>">
        <ContentTemplate>
            <nopCommerce:ProductInfoAdd runat="server" ID="ctrlProductInfoAdd" />
        </ContentTemplate>
    </ajaxToolkit:TabPanel>
</ajaxToolkit:TabContainer>