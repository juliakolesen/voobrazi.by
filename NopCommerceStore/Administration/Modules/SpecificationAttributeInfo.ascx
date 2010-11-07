<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.SpecificationAttributeInfoControl"
    CodeBehind="SpecificationAttributeInfo.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="SimpleTextBox" Src="SimpleTextBox.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="NumericTextBox" Src="NumericTextBox.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="ToolTipLabel" Src="ToolTipLabelControl.ascx" %>
<table class="adminContent">
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblName" Text="<% $NopResources:Admin.SpecificationAttributeInfo.Name %>"
                ToolTip="<% $NopResources:Admin.SpecificationAttributeInfo.Name.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <nopCommerce:SimpleTextBox runat="server" ID="txtName" CssClass="adminInput" ErrorMessage="<% $NopResources:Admin.SpecificationAttributeInfo.Name.ErrorMessage %>" />
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblDisplayOrder" Text="<% $NopResources:Admin.SpecificationAttributeInfo.DisplayOrder %>"
                ToolTip="<% $NopResources:Admin.SpecificationAttributeInfo.DisplayOrder.Tooltip %>"
                ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <nopCommerce:NumericTextBox runat="server" CssClass="adminInput" ID="txtDisplayOrder"
                Value="1" RequiredErrorMessage="<% $NopResources:Admin.SpecificationAttributeInfo.DisplayOrder.RequiredErrorMessage %>"
                RangeErrorMessage="<% $NopResources:Admin.SpecificationAttributeInfo.DisplayOrder.RangeErrorMessage %>"
                MinimumValue="-99999" MaximumValue="99999" ValidationGroup="NewProductSpecification" />
        </td>
    </tr>
</table>
<div id="pnlSpecAttrOptions" runat="server">
    <hr />
    <p>
        <strong>
            <%=GetLocaleResourceString("Admin.SpecificationAttributeInfo.AttributeOptions")%></strong></p>
    <asp:GridView ID="grdSpecificationAttributeOptions" runat="server" AutoGenerateColumns="false"
        DataKeyNames="SpecificationAttributeOptionID" OnRowDeleting="OnSpecificationAttributeOptionsDeleting"
        OnRowCommand="OnSpecificationAttributeOptionsCommand" OnRowDataBound="OnSpecificationAttributeOptionsDataBound">
        <Columns>
            <asp:TemplateField HeaderText="<% $NopResources:Admin.SpecificationAttributeInfo.AttributeOption %>">
                <ItemTemplate>
                    <nopCommerce:SimpleTextBox runat="server" ID="txtOptionName" ErrorMessage="<% $NopResources:Admin.SpecificationAttributeInfo.AttributeOption.ErrorMessage %>"
                        Text='<%# Eval("Name") %>' ValidationGroup="SpecificationAttributeOption" CssClass="adminInput"
                        Width="100%" />
                    <asp:HiddenField ID="hfSpecificationAttributeOptionID" runat="server" Value='<%# Eval("SpecificationAttributeOptionID") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<% $NopResources:Admin.SpecificationAttributeInfo.AttributeOption.DisplayOrder %>"
                ItemStyle-Width="10%">
                <ItemTemplate>
                    <nopCommerce:NumericTextBox runat="server" CssClass="adminInput" Width="50px" ID="txtOptionDisplayOrder"
                        Value='<%# Eval("DisplayOrder") %>' RequiredErrorMessage="<% $NopResources:Admin.SpecificationAttributeInfo.AttributeOption.DisplayOrder.RequiredErrorMessage %>"
                        RangeErrorMessage="<% $NopResources:Admin.SpecificationAttributeInfo.AttributeOption.DisplayOrder.RangeErrorMessage %>"
                        ValidationGroup="SpecificationAttributeOption" MinimumValue="-99999" MaximumValue="99999" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<% $NopResources:Admin.SpecificationAttributeInfo.AttributeOption.Update %>"
                HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:Button ID="btnUpdate" runat="server" CssClass="adminButton" Text="<% $NopResources:Admin.SpecificationAttributeInfo.AttributeOption.Update %>"
                        ValidationGroup="SpecificationAttributeOption" CommandName="UpdateOption" ToolTip="<% $NopResources:Admin.SpecificationAttributeInfo.AttributeOption.Update.Tooltip %>" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<% $NopResources:Admin.SpecificationAttributeInfo.AttributeOption.Delete %>"
                HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:Button ID="btnDelete" runat="server" CssClass="adminButton" Text="<% $NopResources:Admin.SpecificationAttributeInfo.AttributeOption.Delete %>"
                        CausesValidation="false" CommandName="Delete" ToolTip="<% $NopResources:Admin.SpecificationAttributeInfo.AttributeOption.Delete.Tooltip %>" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            <p>
                <%#GetLocaleResourceString("Admin.SpecificationAttributeInfo.AttributeOption.NoOptions")%></p>
        </EmptyDataTemplate>
    </asp:GridView>
    <p>
        <asp:Button ID="btnAddSpecificationAttributeOption" runat="server" CssClass="adminButtonBlue"
            Text="<% $NopResources:Admin.SpecificationAttributeInfo.AttributeOption.NewButton.Text %>"
            OnClick="btnAddSpecificationAttributeOption_Click" CausesValidation="false" ToolTip="<% $NopResources:Admin.SpecificationAttributeInfo.AttributeOption.NewButton.Tooltip %>" /></p>
</div>
