<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.ProductPicturesControl"
    CodeBehind="ProductPictures.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="NumericTextBox" Src="NumericTextBox.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="SimpleTextBox" Src="SimpleTextBox.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="ToolTipLabel" Src="ToolTipLabelControl.ascx" %>
<asp:GridView ID="gvwImages" runat="server" AutoGenerateColumns="false" DataKeyNames="ProductPictureID"
    OnRowDeleting="gvwImages_RowDeleting" OnRowDataBound="gvwImages_RowDataBound"
    OnRowCommand="gvwImages_RowCommand" Width="100%">
    <Columns>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.ProductPictures.Image %>" ItemStyle-Width="50%">
            <ItemTemplate>
                <asp:Image ID="iProductPicture" runat="server" />
                <asp:HiddenField ID="hfProductPictureID" runat="server" Value='<%# Eval("ProductPictureID") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.ProductPictures.DisplayOrder %>"
            HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <nopCommerce:NumericTextBox runat="server" CssClass="adminInput" Width="50px" ID="txtProductPictureDisplayOrder"
                    Value='<%# Eval("DisplayOrder") %>' RequiredErrorMessage="<% $NopResources:Admin.ProductPictures.DisplayOrder.RequiredErrorMessage %>"
                    RangeErrorMessage="<% $NopResources:Admin.ProductPictures.DisplayOrder.RangeErrorMessage %>"
                    ValidationGroup="ProductPictures" MinimumValue="-99999" MaximumValue="99999">
                </nopCommerce:NumericTextBox>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.ProductPictures.Update %>"
            HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <asp:Button ID="btnUpdate" runat="server" CssClass="adminButton" Text="<% $NopResources:Admin.ProductPictures.Update %>"
                    ValidationGroup="ProductPictures" CommandName="UpdateProductPicture" ToolTip="<% $NopResources:Admin.ProductPictures.Update.Tooltip %>" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.ProductPictures.Delete %>"
            HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <asp:Button ID="btnDeletePicture" runat="server" CssClass="adminButton" Text="<% $NopResources:Admin.ProductPictures.Delete %>"
                    CausesValidation="false" CommandName="Delete" ToolTip="<% $NopResources:Admin.ProductPictures.Delete.Tooltip %>" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<p>
    <strong>
        <%=GetLocaleResourceString("Admin.ProductPictures.AddNewPicture")%>
    </strong>
</p>
<table class="adminContent">
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblSelectPicture" Text="<% $NopResources:Admin.ProductPictures.SelectPicture %>"
                ToolTip="<% $NopResources:Admin.ProductPictures.SelectPicture.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:FileUpload class="text" ID="fuProductPicture" CssClass="adminInput" runat="server"
                ToolTip="<% $NopResources:Admin.ProductPictures.FileUpload %>" />
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblProductDisplayOrder" Text="<% $NopResources:Admin.ProductPictures.New.DisplayOrder %>"
                ToolTip="<% $NopResources:Admin.ProductPictures.New.DisplayOrder.Tooltip %>"
                ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <nopCommerce:NumericTextBox runat="server" CssClass="adminInput" ID="txtProductPictureDisplayOrder"
                Value="1" RequiredErrorMessage="<% $NopResources:Admin.ProductPictures.New.DisplayOrder.RequiredErrorMessage %>"
                RangeErrorMessage="<% $NopResources:Admin.ProductPictures.New.DisplayOrder.RangeErrorMessage %>"
                MinimumValue="-99999" MaximumValue="99999" ValidationGroup="UploadNewProductPicture">
            </nopCommerce:NumericTextBox>
        </td>
    </tr>
    <tr>
        <td colspan="2" align="left">
            <asp:Button runat="server" ID="btnUploadProductPicture" CssClass="adminButton" Text="<% $NopResources:Admin.ProductPictures.UploadButton.Text %>"
                ValidationGroup="UploadNewProductPicture" OnClick="btnUploadProductPicture_Click"
                ToolTip="<% $NopResources:Admin.ProductPictures.UploadButton.Tooltip %>" />
        </td>
    </tr>
</table>
