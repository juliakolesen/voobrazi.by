<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.CompareProductsControl"
    CodeBehind="CompareProducts.ascx.cs" %>
<div class="compareProducts">
    <div class="title">
        <%=GetLocaleResourceString("Products.CompareProducts")%>
    </div>
    <div class="clear">
    </div>
    <div class="body">
        <asp:LinkButton ID="btnClearCompareProductsList" OnClick="btnClearCompareProductsList_Click"
            runat="server" Text="<% $NopResources:Products.CompareProductsClearList %>"></asp:LinkButton>
        <br />
        <br />
        <table cellpadding="0" cellspacing="0" border="0" id="tblCompareProducts" runat="server"
            class="compareProductsTable">
        </table>
    </div>
</div>
