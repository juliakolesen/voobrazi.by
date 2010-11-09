<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.PriceRangeFilterControl"
	CodeBehind="PriceRangeFilter.ascx.cs" %>
<p class="headers">
	&raquo;По цене:</p>
<asp:Panel CssClass="var" runat="server" ID="pnlPriceRangeSelector">
	<asp:DataList ID="rptrPriceRange" RepeatColumns="1" RepeatDirection="Vertical" runat="server"
		OnItemDataBound="rptrPriceRange_ItemDataBound">
		<ItemTemplate>
			<asp:HyperLink ID="hlPriceRange" runat="server"></asp:HyperLink>
		</ItemTemplate>
	</asp:DataList>
		<!--<SeparatorTemplate>
			<br />
		</SeparatorTemplate>-->
</asp:Panel>
<asp:Panel runat="server" ID="pnlSelectedPriceRange" Visible="false">
	<span class="inpL"></span>
	<asp:Label ID="lblSelectedPriceRange" CssClass="textInp" runat="server"></asp:Label><span
		class="inpR"></span>
	<p>
		<br />
	</p>
	<asp:HyperLink ID="hlRemoveFilter" runat="server" CssClass="RemovePriceRangeFilter">
            <%=GetLocaleResourceString("Common.PriceRangeFilterRemove")%>
	</asp:HyperLink>
</asp:Panel>
