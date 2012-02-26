<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewedItems.ascx.cs"
    Inherits="NopSolutions.NopCommerce.Web.Modules.ViewedItems" %>
<%@ Register TagPrefix="nopCommerce" TagName="ProductBoxSmall" Src="~/Modules/ProductBoxSmall.ascx" %>

<div class="headers" id="link">
<asp:LinkButton ID="viewedItemsLink" Text="Вы смотрели:" ToolTip="Вы смотрели:" PostBackUrl="../ViewedItems.aspx" runat="server">
</asp:LinkButton>
</div>
<br />
<asp:DataList ID="dlProducts" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
    RepeatLayout="Table" ItemStyle-VerticalAlign="top">
    <ItemTemplate>
        <nopCommerce:ProductBoxSmall ID="ctrlProductBox" Product='<%# Container.DataItem %>'
            runat="server" />
    </ItemTemplate>
</asp:DataList>
