<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.CategoryNavigation"
    CodeBehind="CategoryNavigation.ascx.cs" %>
<div class="flowersWholesale" style="padding-top: 20px">
    <asp:HyperLink runat="server" ID="flowersWholesaleLink" title="÷веты оптом"
        NavigateUrl="~/FlowersWholesale.aspx">
					<img src="../images/ff_images/FlowersWholesale.gif" alt="" height="150" />
    </asp:HyperLink>
</div>
<div class="submenu">
    <asp:PlaceHolder runat="server" ID="phCategories" />
</div>
