<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.SpecificationAttributeAddControl"
    CodeBehind="SpecificationAttributeAdd.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="SpecificationAttributeInfo" Src="SpecificationAttributeInfo.ascx" %>
<div class="section-header">
    <div class="title">
        <img src="Common/ico-catalog.png" alt="<%=GetLocaleResourceString("Admin.SpecificationAttributeAdd.Title")%>" />
        <%=GetLocaleResourceString("Admin.SpecificationAttributeAdd.Title")%>
        <a href="SpecificationAttributes.aspx" title="<%=GetLocaleResourceString("Admin.SpecificationAttributeAdd.BackToAttributeList")%>">
            (<%=GetLocaleResourceString("Admin.SpecificationAttributeAdd.BackToAttributeList")%>)</a>
    </div>
    <div class="options">
        <asp:Button ID="AddButton" runat="server" Text="<% $NopResources:Admin.SpecificationAttributeAdd.SaveButton.Text %>"
            CssClass="adminButtonBlue" OnClick="AddButton_Click" ToolTip="<% $NopResources:Admin.SpecificationAttributeAdd.SaveButton.Tooltip %>" />
    </div>
</div>
<nopCommerce:SpecificationAttributeInfo ID="ctrlSpecificationAttributeInfo" runat="server" />
