<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.CustomerAddControl"
    CodeBehind="CustomerAdd.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="CustomerInfo" Src="CustomerInfo.ascx" %>
<div class="section-header">
    <div class="title">
        <img src="Common/ico-customers.png" alt="<%=GetLocaleResourceString("Admin.CustomerAdd.Title")%>" />
        <%=GetLocaleResourceString("Admin.CustomerAdd.Title")%>
        <a href="Customers.aspx" title="<%=GetLocaleResourceString("Admin.CustomerAdd.BackToCustomers")%>">
            (<%=GetLocaleResourceString("Admin.CustomerAdd.BackToCustomers")%>)</a>
    </div>
    <div class="options">
        <asp:Button ID="AddButton" runat="server" Text="<% $NopResources:Admin.CustomerAdd.AddButton %>"
            CssClass="adminButtonBlue" OnClick="AddButton_Click" ToolTip="<% $NopResources:Admin.CustomerAdd.AddButton.Tooltip %>" />
    </div>
</div>
<nopCommerce:CustomerInfo ID="ctrlCustomerInfo" runat="server" />
