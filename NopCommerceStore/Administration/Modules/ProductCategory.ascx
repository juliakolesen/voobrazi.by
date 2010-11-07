<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.ProductCategoryControl"
    CodeBehind="ProductCategory.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="NumericTextBox" Src="NumericTextBox.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="SimpleTextBox" Src="SimpleTextBox.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="ToolTipLabel" Src="ToolTipLabelControl.ascx" %>
<asp:GridView ID="gvCategoryMappings" runat="server" AutoGenerateColumns="false"
    Width="100%">
    <Columns>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.ProductCategory.Category %>"
            ItemStyle-Width="60%">
            <ItemTemplate>
                <asp:CheckBox ID="cbCategoryInfo" runat="server" Text='<%# Server.HtmlEncode(Eval("CategoryInfo").ToString()) %>'
                    Checked='<%# Eval("IsMapped") %>' ToolTip="<% $NopResources:Admin.ProductCategory.Category.Tooltip %>" />
                <asp:HiddenField ID="hfCategoryID" runat="server" Value='<%# Eval("CategoryID") %>' />
                <asp:HiddenField ID="hfProductCategoryID" runat="server" Value='<%# Eval("ProductCategoryID") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.ProductCategory.View %>" HeaderStyle-HorizontalAlign="Center"
            ItemStyle-Width="13%" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <a href='CategoryDetails.aspx?CategoryID=<%# Eval("CategoryID") %>' title="<%#GetLocaleResourceString("Admin.ProductCategory.View.Tooltip")%>">
                    <%#GetLocaleResourceString("Admin.ProductCategory.View")%></a>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.ProductCategory.FeaturedProduct %>"
            HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="13%" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <asp:CheckBox ID="cbFeatured" runat="server" Checked='<%# Eval("IsFeatured") %>'
                    ToolTip="<% $NopResources:Admin.ProductCategory.FeaturedProduct.Tooltip %>" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.ProductCategory.DisplayOrder %>"
            HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="13%" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <nopCommerce:NumericTextBox runat="server" CssClass="adminInput" Width="50px" ID="txtDisplayOrder"
                    Value='<%# Eval("DisplayOrder") %>' RequiredErrorMessage="<% $NopResources:Admin.ProductCategory.DisplayOrder.RequiredErrorMessage %>"
                    RangeErrorMessage="<% $NopResources:Admin.ProductCategory.DisplayOrder.RangeErrorMessage %>"
                    MinimumValue="-99999" MaximumValue="99999"></nopCommerce:NumericTextBox>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
