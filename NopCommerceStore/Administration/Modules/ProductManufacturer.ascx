<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.ProductManufacturerControl"
    CodeBehind="ProductManufacturer.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="NumericTextBox" Src="NumericTextBox.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="SimpleTextBox" Src="SimpleTextBox.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="ToolTipLabel" Src="ToolTipLabelControl.ascx" %>
<asp:GridView ID="gvManufacturerMappings" runat="server" AutoGenerateColumns="false"
    Width="100%">
    <Columns>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.ProductManufacturer.Manufacturer %>"
            ItemStyle-Width="60%">
            <ItemTemplate>
                <asp:CheckBox ID="cbManufacturerInfo" runat="server" Text='<%# Server.HtmlEncode(Eval("ManufacturerInfo").ToString()) %>'
                    Checked='<%# Eval("IsMapped") %>' ToolTip="<% $NopResources:Admin.ProductManufacturer.Manufacturer.Tooltip %>" />
                <asp:HiddenField ID="hfManufacturerID" runat="server" Value='<%# Eval("ManufacturerID") %>' />
                <asp:HiddenField ID="hfProductManufacturerID" runat="server" Value='<%# Eval("ProductManufacturerID") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.ProductManufacturer.View %>"
            HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="13%" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <a href='ManufacturerDetails.aspx?ManufacturerID=<%# Eval("ManufacturerID") %>' title="<%#GetLocaleResourceString("Admin.ProductManufacturer.View.Tooltip")%>">
                    <%#GetLocaleResourceString("Admin.ProductManufacturer.View")%></a>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.ProductManufacturer.FeaturedProduct %>"
            HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="13%" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <asp:CheckBox ID="cbFeatured" runat="server" Checked='<%# Eval("IsFeatured") %>'
                    ToolTip="<% $NopResources:Admin.ProductManufacturer.FeaturedProduct.Tooltip %>" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.ProductManufacturer.DisplayOrder %>"
            HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="13%" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <nopCommerce:NumericTextBox runat="server" CssClass="adminInput" Width="50px" ID="txtDisplayOrder"
                    Value='<%# Eval("DisplayOrder") %>' RequiredErrorMessage="<% $NopResources:Admin.ProductManufacturer.DisplayOrder.RequiredErrorMessage %>"
                    RangeErrorMessage="<% $NopResources:Admin.ProductManufacturer.DisplayOrder.RangeErrorMessage %>"
                    MinimumValue="-99999" MaximumValue="99999"></nopCommerce:NumericTextBox>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
