<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.SpecificationAttributeOptionAddControl"
    CodeBehind="SpecificationAttributeOptionAdd.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="SimpleTextBox" Src="SimpleTextBox.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="NumericTextBox" Src="NumericTextBox.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="ToolTipLabel" Src="ToolTipLabelControl.ascx" %>
<script type="text/javascript" language="javascript">
    function colorChanged(sender) {
        sender.get_element().style.color =
       "#" + sender.get_selectedColor();
    }
</script>
<div class="section-header">
    <div class="title">
        <img src="Common/ico-catalog.png" alt="<%=GetLocaleResourceString("Admin.SpecificationAttributeOptionAdd.Title")%>" />
        <%=GetLocaleResourceString("Admin.SpecificationAttributeOptionAdd.Title")%>
        <asp:HyperLink ID="hlBack" runat="server" Text="<% $NopResources:Admin.SpecificationAttributeOptionAdd.BackToAttributeDetails %>"
            ToolTip="<% $NopResources:Admin.SpecificationAttributeOptionAdd.BackToAttributeDetails %>" />
    </div>
    <div class="options">
        <asp:Button ID="AddButton" runat="server" Text="<% $NopResources:Admin.SpecificationAttributeOptionAdd.SaveButton.Text %>"
            CssClass="adminButtonBlue" OnClick="AddButton_Click" ToolTip="<% $NopResources:Admin.SpecificationAttributeOptionAdd.SaveButton.Tooltip %>" />
    </div>
</div>
<table class="adminContent">
    <tbody>
        <tr>
            <td class="adminTitle">
                <nopCommerce:ToolTipLabel runat="server" ID="lblOptionName" Text="<% $NopResources:Admin.SpecificationAttributeOptionAdd.Name %>"
                    ToolTip="<% $NopResources:Admin.SpecificationAttributeOptionAdd.Name.Tooltip %>"
                    ToolTipImage="~/Administration/Common/ico-help.gif" />
            </td>
            <td class="adminData">
                <nopCommerce:SimpleTextBox runat="server" ID="txtNewOptionName" CssClass="adminInput" />
            </td>
        </tr>
        <tr>
            <td class="adminTitle">
                <nopCommerce:ToolTipLabel runat="server" ID="lblOptionDisplayOrder" Text="<% $NopResources:Admin.SpecificationAttributeOptionAdd.DisplayOrder %>"
                    ToolTip="<% $NopResources:Admin.SpecificationAttributeOptionAdd.DisplayOrder.Tooltip %>"
                    ToolTipImage="~/Administration/Common/ico-help.gif" />
            </td>
            <td class="adminData">
                <nopCommerce:NumericTextBox runat="server" CssClass="adminInput" ID="txtNewOptionDisplayOrder"
                    Value="1" RequiredErrorMessage="<% $NopResources:Admin.SpecificationAttributeOptionAdd.DisplayOrder.RequiredErrorMessage %>"
                    RangeErrorMessage="<% $NopResources:Admin.SpecificationAttributeOptionAdd.DisplayOrder.RangeErrorMessage %>"
                    MinimumValue="-99999" MaximumValue="99999" />
            </td>
        </tr>
        <tr>
            <td class="adminTitle">
                <nopCommerce:ToolTipLabel runat="server" ID="ToolTipLabelColor" Text="«начение цвета"
                    ToolTip="«начение цвета" ToolTipImage="~/Administration/Common/ico-help.gif"
                    Visible="false" />
            </td>
            <td class="adminData">
                <asp:TextBox runat="server" ID="txtColorArgb" CssClass="adminInput" Visible="false" />
            </td>
            <td>
                <ajaxToolkit:ColorPickerExtender runat="server" ID="colorPicker" TargetControlID="txtColorArgb"
                    OnClientColorSelectionChanged="colorChanged">
                </ajaxToolkit:ColorPickerExtender>
            </td>
        </tr>
        <tr>
            <td>
                <asp:CustomValidator runat="server" ID="cusCustom" ControlToValidate="txtColorArgb"
                    OnServerValidate="cusCustom_ServerValidate" Enabled="false" 
                    ErrorMessage = "÷вет с таким именем уже существует!"></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:FileUpload ID="FileUploadControl" runat="server" Visible="false"/>
            </td>
        </tr>
    </tbody>
</table>
