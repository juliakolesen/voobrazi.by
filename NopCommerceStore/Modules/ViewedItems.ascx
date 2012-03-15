<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewedItems.ascx.cs"
    Inherits="NopSolutions.NopCommerce.Web.Modules.ViewedItems" %>
<%@ Register TagPrefix="nopCommerce" TagName="ProductBoxSmall" Src="~/Modules/ProductBoxSmall.ascx" %>
<asp:Panel ID="viewedItemsLnk" runat="server">
    <a id="link" class="headers" title="Вы смотрели:" href="../ViewedItems.aspx">
        Вы смотрели:</a>
</asp:Panel>
<br />
<asp:DataList ID="dlProducts" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
    RepeatLayout="Table" ItemStyle-VerticalAlign="top">
    <ItemTemplate>
        <nopCommerce:ProductBoxSmall ID="ctrlProductBox" Product='<%# Container.DataItem %>'
            runat="server" />
    </ItemTemplate>
</asp:DataList>