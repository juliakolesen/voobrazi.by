<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.ProductVariantAttributesControl"
    CodeBehind="ProductVariantAttributes.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="SelectDiscountsControl" Src="SelectDiscountsControl.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="NumericTextBox" Src="NumericTextBox.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="DecimalTextBox" Src="DecimalTextBox.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="ToolTipLabel" Src="ToolTipLabelControl.ascx" %>
<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<asp:GridView ID="gvProductVariantAttributes" runat="server" AutoGenerateColumns="false"
    DataKeyNames="ProductVariantAttributeID" OnRowDeleting="gvProductVariantAttributes_RowDeleting"
    OnRowDataBound="gvProductVariantAttributes_RowDataBound" OnRowCommand="gvProductVariantAttributes_RowCommand"
    Width="100%">
    <Columns>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.ProductVariantAttributes.Attribute %>"
            ItemStyle-Width="20%">
            <ItemTemplate>
                <asp:DropDownList ID="ddlProductAttribute" runat="server" />
                <asp:HiddenField ID="hfProductVariantAttributeID" runat="server" Value='<%# Eval("ProductVariantAttributeID") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.ProductVariantAttributes.TextPrompt %>"
            ItemStyle-Width="15%">
            <ItemTemplate>
                <asp:TextBox ID="txtTextPrompt" runat="server" Value='<%# Eval("TextPrompt") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.ProductVariantAttributes.IsRequired %>"
            ItemStyle-Width="10%">
            <ItemTemplate>
                <asp:CheckBox ID="cbIsRequired" runat="server" Checked='<%# Eval("IsRequired") %>'
                    ToolTip="<% $NopResources:Admin.ProductVariantAttributes.IsRequired.Tooltip %>" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.ProductVariantAttributes.ControlType %>"
            ItemStyle-Width="10%">
            <ItemTemplate>
                <asp:DropDownList ID="ddlAttributeControlType" runat="server" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.ProductVariantAttributes.DisplayOrder %>"
            HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <nopCommerce:NumericTextBox runat="server" CssClass="adminInput" Width="50px" ID="txtDisplayOrder"
                    Value='<%# Eval("DisplayOrder") %>' RequiredErrorMessage="<% $NopResources:Admin.ProductVariantAttributes.DisplayOrder.RequiredErrorMessage %>"
                    RangeErrorMessage="<% $NopResources:Admin.ProductVariantAttributes.DisplayOrder.RangeErrorMessage %>"
                    ValidationGroup="ProductVariantAttribute" MinimumValue="-99999" MaximumValue="99999">
                </nopCommerce:NumericTextBox>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.ProductVariantAttributes.Values %>"
            ItemStyle-Width="15%">
            <ItemTemplate>
                <asp:HyperLink runat="server" ID="hlAttributeValues" Text="View/Edit values"></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.ProductVariantAttributes.Update %>"
            HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <asp:Button ID="btnUpdate" runat="server" CssClass="adminButton" Text="<% $NopResources:Admin.ProductVariantAttributes.Update %>"
                    ValidationGroup="ProductVariantAttribute" CommandName="UpdateProductVariantAttribute"
                    ToolTip="<% $NopResources:Admin.ProductVariantAttributes.Update.Tooltip %>" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.ProductVariantAttributes.Delete %>"
            HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <asp:Button ID="btnDeleteProductVariantAttribute" runat="server" CssClass="adminButton"
                    Text="<% $NopResources:Admin.ProductVariantAttributes.Delete %>" CausesValidation="false"
                    CommandName="Delete" ToolTip="<% $NopResources:Admin.ProductVariantAttributes.Delete.Tooltip %>" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<p>
    <strong>
        <%=GetLocaleResourceString("Admin.ProductVariantAttributes.AddNew")%>
    </strong>
</p>
<table class="adminContent">
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblAttribute" Text="<% $NopResources:Admin.ProductVariantAttributes.New.Attribute %>"
                ToolTip="<% $NopResources:Admin.ProductVariantAttributes.New.Attribute.Tooltip %>"
                ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:DropDownList class="text" ID="ddlNewProductAttributes" AutoPostBack="False"
                CssClass="adminInput" runat="server">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblTextPrompt" Text="<% $NopResources:Admin.ProductVariantAttributes.New.TextPrompt %>"
                ToolTip="<% $NopResources:Admin.ProductVariantAttributes.New.TextPrompt.Tooltip %>"
                ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:TextBox runat="server" CssClass="adminInput" ID="txtNewTextPrompt"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblAttributeRequired" Text="<% $NopResources:Admin.ProductVariantAttributes.New.Required %>"
                ToolTip="<% $NopResources:Admin.ProductVariantAttributes.New.Required.Tooltip %>"
                ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:CheckBox ID="cbNewProductVariantAttributeIsRequired" runat="server" Checked="true" />
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblAttributeControlType" Text="<% $NopResources:Admin.ProductVariantAttributes.New.ControlType %>"
                ToolTip="<% $NopResources:Admin.ProductVariantAttributes.New.ControlType.Tooltip %>"
                ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:DropDownList class="text" ID="ddlAttributeControlType" AutoPostBack="False"
                CssClass="adminInput" runat="server">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblAttributeDisplayOrder" Text="<% $NopResources:Admin.ProductVariantAttributes.New.DisplayOrder %>"
                ToolTip="<% $NopResources:Admin.ProductVariantAttributes.New.DisplayOrder.Tooltip %>"
                ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <nopCommerce:NumericTextBox runat="server" CssClass="adminInput" ID="txtNewProductVariantAttributeDisplayOrder"
                Value="1" RequiredErrorMessage="<% $NopResources:Admin.ProductVariantAttributes.New.DisplayOrder.RequiredErrorMessage %>"
                RangeErrorMessage="<% $NopResources:Admin.ProductVariantAttributes.New.DisplayOrder.RangeErrorMessage %>"
                MinimumValue="-99999" MaximumValue="99999" ValidationGroup="NewProductVariantAttribute">
            </nopCommerce:NumericTextBox>
        </td>
    </tr>
    <tr>
        <td colspan="2" align="left">
            <asp:Button runat="server" ID="btnNewProductVariantAttribute" CssClass="adminButton"
                Text="<% $NopResources:Admin.ProductVariantAttributes.New.AddNewButton.Text %>"
                ValidationGroup="NewProductVariantAttribute" OnClick="btnNewProductVariantAttribute_Click"
                ToolTip="<% $NopResources:Admin.ProductVariantAttributes.New.AddNewButton.Tooltip %>" />
        </td>
    </tr>
</table>
