<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Templates.Products.VariantsInGrid"
	CodeBehind="VariantsInGrid.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="ProductInfo" Src="~/Modules/ProductInfo.ascx" %>

<div class="ordercontainer">
	<div class="seccont_top">
		<div class="titl">
            <asp:SiteMapPath ID="siteMapPath" runat="server" PathSeparator=" / " SiteMapProvider="NopDefaultXmlSiteMapProvider">
            </asp:SiteMapPath>
		</div>
	</div>
	<nopCommerce:ProductInfo ID="ctrlProductInfo" runat="server" />
	<div class="seccont_bottom">
	</div>
</div>