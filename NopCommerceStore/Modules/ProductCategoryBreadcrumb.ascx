<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.ProductCategoryBreadcrumb"
	CodeBehind="ProductCategoryBreadcrumb.ascx.cs" %>
<div class="breadcrumb">
	<asp:Repeater ID="rptrCategoryBreadcrumb" runat="server">
		<ItemTemplate>
			<%#Server.HtmlEncode(Eval("Name").ToString()) %>
		</ItemTemplate>
		<SeparatorTemplate>
			/
		</SeparatorTemplate>
	</asp:Repeater>
	/
	<asp:Label runat="server" ID="hlProduct" />
	<br />
</div>
