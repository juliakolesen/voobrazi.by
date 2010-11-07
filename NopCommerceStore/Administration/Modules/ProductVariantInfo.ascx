<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.ProductVariantInfoControl"
    CodeBehind="ProductVariantInfo.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="NumericTextBox" Src="NumericTextBox.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="DecimalTextBox" Src="DecimalTextBox.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="ToolTipLabel" Src="ToolTipLabelControl.ascx" %>
<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<table class="adminContent">
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblProductVariantName" Text="<% $NopResources:Admin.ProductVariantInfo.ProductVariantName %>"
                ToolTip="<% $NopResources:Admin.ProductVariantInfo.ProductVariantName.Tooltip %>"
                ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtName" runat="server" CssClass="adminInput"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblSKU" Text="<% $NopResources:Admin.ProductVariantInfo.SKU %>"
                ToolTip="<% $NopResources:Admin.ProductVariantInfo.SKU.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtSKU" runat="server" CssClass="adminInput"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblImage" Text="<% $NopResources:Admin.ProductVariantInfo.Image %>"
                ToolTip="<% $NopResources:Admin.ProductVariantInfo.Image.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:Image ID="iProductVariantPicture" runat="server" />
            <br />
            <asp:Button ID="btnRemoveProductVariantImage" CssClass="adminButton" CausesValidation="false"
                runat="server" Text="<% $NopResources:Admin.ProductVariantInfo.RemoveImage %>"
                OnClick="btnRemoveProductVariantImage_Click" Visible="false" />
            <br />
            <asp:FileUpload ID="fuProductVariantPicture" CssClass="adminInput" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblDescription" Text="<% $NopResources:Admin.ProductVariantInfo.Description %>"
                ToolTip="<% $NopResources:Admin.ProductVariantInfo.Description.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <FCKeditorV2:FCKeditor ID="txtDescription" runat="server" AutoDetectLanguage="false"
                Height="350">
            </FCKeditorV2:FCKeditor>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblAdminComment" Text="<% $NopResources:Admin.ProductVariantInfo.AdminComment %>"
                ToolTip="<% $NopResources:Admin.ProductVariantInfo.AdminComment.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtAdminComment" runat="server" CssClass="adminInput" TextMode="MultiLine"
                Height="100"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblManufacturerPartNumber" Text="<% $NopResources:Admin.ProductVariantInfo.ManufacturerPartNumber %>"
                ToolTip="<% $NopResources:Admin.ProductVariantInfo.ManufacturerPartNumber.Tooltip %>"
                ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtManufacturerPartNumber" CssClass="adminInput" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr class="adminGroup">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblIsDownload" Text="<% $NopResources:Admin.ProductVariantInfo.IsDownload %>"
                ToolTip="<% $NopResources:Admin.ProductVariantInfo.IsDownload.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:CheckBox ID="cbIsDownload" runat="server" Checked="False" AutoPostBack="True"
                OnCheckedChanged="cbIsDownload_CheckedChanged"></asp:CheckBox>
        </td>
    </tr>
    <tr runat="server" id="pnlUseDownloadURL">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblUseDownloadURL" Text="<% $NopResources:Admin.ProductVariantInfo.UseDownloadURL %>"
                ToolTip="<% $NopResources:Admin.ProductVariantInfo.UseDownloadURL.Tooltip %>"
                ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:CheckBox ID="cbUseDownloadURL" runat="server" Checked="False" AutoPostBack="True"
                OnCheckedChanged="cbUseDownloadURL_CheckedChanged"></asp:CheckBox>
        </td>
    </tr>
    <tr runat="server" id="pnlDownloadURL">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblDownloadURL" Text="<% $NopResources:Admin.ProductVariantInfo.DownloadURL %>"
                ToolTip="<% $NopResources:Admin.ProductVariantInfo.DownloadURL.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtDownloadURL" CssClass="adminInput" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr runat="server" id="pnlDownloadFile">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblDownloadFile" Text="<% $NopResources:Admin.ProductVariantInfo.DownloadFile %>"
                ToolTip="<% $NopResources:Admin.ProductVariantInfo.DownloadFile.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:HyperLink ID="hlProductVariantDownload" runat="server" Text="<% $NopResources:Admin.ProductVariantInfo.DownloadFile.Download %>" />
            <br />
            <asp:Button ID="btnRemoveProductVariantDownload" CssClass="adminButton" CausesValidation="false"
                runat="server" Text="<% $NopResources:Admin.ProductVariantInfo.DownloadFile.Remove %>"
                OnClick="btnRemoveProductVariantDownload_Click" Visible="false" />
            <br />
            <asp:FileUpload ID="fuProductVariantDownload" CssClass="adminInput" runat="server" />
        </td>
    </tr>
    <tr runat="server" id="pnlUnlimitedDownloads">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblUnlimitedDownloads" Text="<% $NopResources:Admin.ProductVariantInfo.UnlimitedDownloads %>"
                ToolTip="<% $NopResources:Admin.ProductVariantInfo.UnlimitedDownloads.Tooltip %>"
                ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:CheckBox ID="cbUnlimitedDownloads" runat="server" Checked="True" AutoPostBack="True"
                OnCheckedChanged="cbUnlimitedDownloads_CheckedChanged"></asp:CheckBox>
        </td>
    </tr>
    <tr runat="server" id="pnlMaxNumberOfDownloads">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblMaxNumberOfDownloads" Text="<% $NopResources:Admin.ProductVariantInfo.MaxNumberOfDownloads %>"
                ToolTip="<% $NopResources:Admin.ProductVariantInfo.MaxNumberOfDownloads.Tooltip %>"
                ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <nopCommerce:NumericTextBox runat="server" CssClass="adminInput" ID="txtMaxNumberOfDownloads"
                RequiredErrorMessage="<% $NopResources:Admin.ProductVariantInfo.MaxNumberOfDownloads.RequiredErrorMessage %>"
                MinimumValue="0" MaximumValue="99999999" Value="10" RangeErrorMessage="<% $NopResources:Admin.ProductVariantInfo.MaxNumberOfDownloads.RangeErrorMessage %>">
            </nopCommerce:NumericTextBox>
        </td>
    </tr>
    <tr runat="server" id="pnlHasSampleDownload">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblHasSampleDownload" Text="<% $NopResources:Admin.ProductVariantInfo.HasSampleDownload %>"
                ToolTip="<% $NopResources:Admin.ProductVariantInfo.HasSampleDownload.Tooltip %>"
                ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:CheckBox ID="cbHasSampleDownload" runat="server" Checked="False" AutoPostBack="True"
                OnCheckedChanged="cbHasSampleDownload_CheckedChanged"></asp:CheckBox>
        </td>
    </tr>
    <tr runat="server" id="pnlUseSampleDownloadURL">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblUseSampleDownloadURL" Text="<% $NopResources:Admin.ProductVariantInfo.UseSampleDownloadURL %>"
                ToolTip="<% $NopResources:Admin.ProductVariantInfo.UseSampleDownloadURL.Tooltip %>"
                ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:CheckBox ID="cbUseSampleDownloadURL" runat="server" Checked="False" AutoPostBack="True"
                OnCheckedChanged="cbUseSampleDownloadURL_CheckedChanged"></asp:CheckBox>
        </td>
    </tr>
    <tr runat="server" id="pnlSampleDownloadURL">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblSampleDownloadURL" Text="<% $NopResources:Admin.ProductVariantInfo.SampleDownloadURL %>"
                ToolTip="<% $NopResources:Admin.ProductVariantInfo.SampleDownloadURL.Tooltip %>"
                ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtSampleDownloadURL" CssClass="adminInput" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr runat="server" id="pnlSampleDownloadFile">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblSampleDownloadFile" Text="<% $NopResources:Admin.ProductVariantInfo.SampleDownloadFile %>"
                ToolTip="<% $NopResources:Admin.ProductVariantInfo.SampleDownloadFile.Tooltip %>"
                ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:HyperLink ID="hlProductVariantSampleDownload" runat="server" Text="<% $NopResources:Admin.ProductVariantInfo.SampleDownloadFile.Download %>" />
            <br />
            <asp:Button ID="btnRemoveProductVariantSampleDownload" CssClass="adminButton" CausesValidation="false"
                runat="server" Text="<% $NopResources:Admin.ProductVariantInfo.SampleDownloadFile.Remove %>"
                OnClick="btnRemoveProductVariantSampleDownload_Click" Visible="false" />
            <br />
            <asp:FileUpload ID="fuProductVariantSampleDownload" CssClass="adminInput" runat="server" />
        </td>
    </tr>
    <tr class="adminGroup">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblShipEnabled" Text="<% $NopResources:Admin.ProductVariantInfo.ShipEnabled %>"
                ToolTip="<% $NopResources:Admin.ProductVariantInfo.ShipEnabled.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:CheckBox ID="cbIsShipEnabled" runat="server" Checked="True"></asp:CheckBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblFreeShipping" Text="<% $NopResources:Admin.ProductVariantInfo.FreeShipping %>"
                ToolTip="<% $NopResources:Admin.ProductVariantInfo.FreeShipping.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:CheckBox ID="cbIsFreeShipping" runat="server" Checked="False"></asp:CheckBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblAdditionalShippingCharge" Text="<% $NopResources:Admin.ProductVariantInfo.AdditionalShippingCharge %>"
                ToolTip="<% $NopResources:Admin.ProductVariantInfo.AdditionalShippingCharge.Tooltip %>"
                ToolTipImage="~/Administration/Common/ico-help.gif" />
            [<%=CurrencyManager.PrimaryStoreCurrency.CurrencyCode%>]:
        </td>
        <td class="adminData">
            <nopCommerce:DecimalTextBox runat="server" CssClass="adminInput" ID="txtAdditionalShippingCharge"
                Value="0" RequiredErrorMessage="<% $NopResources:Admin.ProductVariantInfo.AdditionalShippingCharge.RequiredErrorMessage %>"
                MinimumValue="0" MaximumValue="99999999" RangeErrorMessage="<% $NopResources:Admin.ProductVariantInfo.AdditionalShippingCharge.RangeErrorMessage %>">
            </nopCommerce:DecimalTextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblTaxExempt" Text="<% $NopResources:Admin.ProductVariantInfo.TaxExempt %>"
                ToolTip="<% $NopResources:Admin.ProductVariantInfo.TaxExempt.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:CheckBox ID="cbIsTaxExempt" runat="server" Checked="False"></asp:CheckBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblTaxCategory" Text="<% $NopResources:Admin.ProductVariantInfo.TaxCategory %>"
                ToolTip="<% $NopResources:Admin.ProductVariantInfo.TaxCategory.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:DropDownList ID="ddlTaxCategory" AutoPostBack="False" CssClass="adminInput"
                runat="server">
            </asp:DropDownList>
        </td>
    </tr>
    <tr class="adminGroup">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblManageStock" Text="<% $NopResources:Admin.ProductVariantInfo.ManageStock %>"
                ToolTip="<% $NopResources:Admin.ProductVariantInfo.ManageStock.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:CheckBox ID="cbManageStock" runat="server" Checked="True" AutoPostBack="True"
                OnCheckedChanged="cbManageStock_CheckedChanged"></asp:CheckBox>
        </td>
    </tr>
    <tr runat="server" id="pnlStockQuantity">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblStockQuantity" Text="<% $NopResources:Admin.ProductVariantInfo.StockQuantity %>"
                ToolTip="<% $NopResources:Admin.ProductVariantInfo.StockQuantity.Tooltip %>"
                ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <nopCommerce:NumericTextBox runat="server" CssClass="adminInput" ID="txtStockQuantity"
                RequiredErrorMessage="<% $NopResources:Admin.ProductVariantInfo.StockQuantity.RequiredErrorMessage %>"
                MinimumValue="0" MaximumValue="99999999" Value="10000" RangeErrorMessage="<% $NopResources:Admin.ProductVariantInfo.StockQuantity.RangeErrorMessage %>">
            </nopCommerce:NumericTextBox>
        </td>
    </tr>
    <tr runat="server" id="pnlMinStockQuantity">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblMinStockQuantity" Text="<% $NopResources:Admin.ProductVariantInfo.MinStockQuantity %>"
                ToolTip="<% $NopResources:Admin.ProductVariantInfo.MinStockQuantity.Tooltip %>"
                ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <nopCommerce:NumericTextBox runat="server" CssClass="adminInput" ID="txtMinStockQuantity"
                RequiredErrorMessage="<% $NopResources:Admin.ProductVariantInfo.MinStockQuantity.RequiredErrorMessage %>"
                MinimumValue="0" MaximumValue="99999999" Value="0" RangeErrorMessage="<% $NopResources:Admin.ProductVariantInfo.MinStockQuantity.RangeErrorMessage %>">
            </nopCommerce:NumericTextBox>
        </td>
    </tr>
    <tr runat="server" id="pnlLowStockActivity">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblLowStockActivity" Text="<% $NopResources:Admin.ProductVariantInfo.LowStockActivity %>"
                ToolTip="<% $NopResources:Admin.ProductVariantInfo.LowStockActivity.Tooltip %>"
                ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:DropDownList ID="ddlLowStockActivity" AutoPostBack="False" CssClass="adminInput"
                runat="server">
            </asp:DropDownList>
        </td>
    </tr>
    <tr runat="server" id="pnlNotifyForQuantityBelow">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblNotifyForQuantityBelow" Text="<% $NopResources:Admin.ProductVariantInfo.NotifyForQuantityBelow %>"
                ToolTip="<% $NopResources:Admin.ProductVariantInfo.NotifyForQuantityBelow.Tooltip %>"
                ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <nopCommerce:NumericTextBox runat="server" CssClass="adminInput" ID="txtNotifyForQuantityBelow"
                RequiredErrorMessage="<% $NopResources:Admin.ProductVariantInfo.NotifyForQuantityBelow.RequiredErrorMessage %>"
                MinimumValue="1" MaximumValue="99999999" Value="1" RangeErrorMessage="<% $NopResources:Admin.ProductVariantInfo.NotifyForQuantityBelow.RangeErrorMessage %>">
            </nopCommerce:NumericTextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblOrderMinimumQuantity" Text="<% $NopResources:Admin.ProductVariantInfo.OrderMinimumQuantity %>"
                ToolTip="<% $NopResources:Admin.ProductVariantInfo.OrderMinimumQuantity.Tooltip %>"
                ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <nopCommerce:NumericTextBox runat="server" CssClass="adminInput" ID="txtOrderMinimumQuantity"
                RequiredErrorMessage="<% $NopResources:Admin.ProductVariantInfo.OrderMinimumQuantity.RequiredErrorMessage %>"
                MinimumValue="1" MaximumValue="99999999" Value="1" RangeErrorMessage="<% $NopResources:Admin.ProductVariantInfo.OrderMinimumQuantity.RangeErrorMessage %>">
            </nopCommerce:NumericTextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblOrderMaximumQuantity" Text="<% $NopResources:Admin.ProductVariantInfo.OrderMaximumQuantity %>"
                ToolTip="<% $NopResources:Admin.ProductVariantInfo.OrderMaximumQuantity.Tooltip %>"
                ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <nopCommerce:NumericTextBox runat="server" CssClass="adminInput" ID="txtOrderMaximumQuantity"
                RequiredErrorMessage="<% $NopResources:Admin.ProductVariantInfo.OrderMaximumQuantity.RequiredErrorMessage %>"
                MinimumValue="1" MaximumValue="99999999" Value="10000" RangeErrorMessage="<% $NopResources:Admin.ProductVariantInfo.OrderMaximumQuantity.RangeErrorMessage %>">
            </nopCommerce:NumericTextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblWarehouse" Text="<% $NopResources:Admin.ProductVariantInfo.Warehouse %>"
                ToolTip="<% $NopResources:Admin.ProductVariantInfo.Warehouse.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:DropDownList ID="ddlWarehouse" AutoPostBack="False" CssClass="adminInput" runat="server">
            </asp:DropDownList>
        </td>
    </tr>
    <tr class="adminGroup">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblDisableBuyButton" Text="<% $NopResources:Admin.ProductVariantInfo.DisableBuyButton %>"
                ToolTip="<% $NopResources:Admin.ProductVariantInfo.DisableBuyButton.Tooltip %>"
                ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:CheckBox ID="cbDisableBuyButton" runat="server" Checked="False"></asp:CheckBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblPrice" Text="<% $NopResources:Admin.ProductVariantInfo.Price %>"
                ToolTip="<% $NopResources:Admin.ProductVariantInfo.Price.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
            [<%=CurrencyManager.PrimaryStoreCurrency.CurrencyCode%>]:
        </td>
        <td class="adminData">
            <nopCommerce:DecimalTextBox runat="server" CssClass="adminInput" ID="txtPrice" Value="0"
                RequiredErrorMessage="<% $NopResources:Admin.ProductVariantInfo.Price.RequiredErrorMessage %>"
                MinimumValue="0" MaximumValue="99999999" RangeErrorMessage="<% $NopResources:Admin.ProductVariantInfo.Price.RangeErrorMessage %>">
            </nopCommerce:DecimalTextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblOldPrice" Text="<% $NopResources:Admin.ProductVariantInfo.OldPrice %>"
                ToolTip="<% $NopResources:Admin.ProductVariantInfo.OldPrice.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
            [<%=CurrencyManager.PrimaryStoreCurrency.CurrencyCode%>]:
        </td>
        <td class="adminData">
            <nopCommerce:DecimalTextBox runat="server" CssClass="adminInput" ID="txtOldPrice"
                Value="0" RequiredErrorMessage="<% $NopResources:Admin.ProductVariantInfo.OldPrice.RequiredErrorMessage %>"
                MinimumValue="0" MaximumValue="99999999" RangeErrorMessage="<% $NopResources:Admin.ProductVariantInfo.OldPrice.RangeErrorMessage %>">
            </nopCommerce:DecimalTextBox>
        </td>
    </tr>
    <tr class="adminGroup">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblAvailableStartDateTime" Text="<% $NopResources:Admin.ProductVariantInfo.AvailableStartDateTime %>"
                ToolTip="<% $NopResources:Admin.ProductVariantInfo.AvailableStartDateTime.ToolTip %>"
                ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:TextBox runat="server" ID="txtAvailableStartDateTime" />
            <asp:ImageButton runat="Server" ID="iAvailableStartDateTime" ImageUrl="~/images/Calendar_scheduleHS.png"
                AlternateText="<% $NopResources:Admin.ProductVariantInfo.AvailableStartDateTime.ClickToShowCalendar %>" /><br />
            <ajaxToolkit:CalendarExtender ID="cAvailableStartDateTimeButtonExtender" runat="server"
                TargetControlID="txtAvailableStartDateTime" PopupButtonID="iAvailableStartDateTime" />
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblAvailableEndDateTime" Text="<% $NopResources:Admin.ProductVariantInfo.AvailableEndDateTime %>"
                ToolTip="<% $NopResources:Admin.ProductVariantInfo.AvailableEndDateTime.ToolTip %>"
                ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:TextBox runat="server" ID="txtAvailableEndDateTime" />
            <asp:ImageButton runat="Server" ID="iAvailableEndDateTime" ImageUrl="~/images/Calendar_scheduleHS.png"
                AlternateText="<% $NopResources:Admin.ProductVariantInfo.AvailableEndDateTime.ClickToShowCalendar %>" /><br />
            <ajaxToolkit:CalendarExtender ID="cAvailableEndDateTimeButtonExtender" runat="server"
                TargetControlID="txtAvailableEndDateTime" PopupButtonID="iAvailableEndDateTime" />
        </td>
    </tr>
    <tr class="adminGroup">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblWeight" Text="<% $NopResources:Admin.ProductVariantInfo.Weight %>"
                ToolTip="<% $NopResources:Admin.ProductVariantInfo.Weight.ToolTip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
            [<%=MeasureManager.BaseWeightIn.Name%>]:
        </td>
        <td class="adminData">
            <nopCommerce:DecimalTextBox runat="server" CssClass="adminInput" ID="txtWeight" Value="0"
                RequiredErrorMessage="<% $NopResources:Admin.ProductVariantInfo.Weight.RequiredErrorMessage %>"
                MinimumValue="0" MaximumValue="99999999" RangeErrorMessage="<% $NopResources:Admin.ProductVariantInfo.Weight.RangeErrorMessage %>">
            </nopCommerce:DecimalTextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblLength" Text="<% $NopResources:Admin.ProductVariantInfo.Length %>"
                ToolTip="<% $NopResources:Admin.ProductVariantInfo.Length.ToolTip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
            [<%=MeasureManager.BaseDimensionIn.Name%>]:
        </td>
        <td class="adminData">
            <nopCommerce:DecimalTextBox runat="server" CssClass="adminInput" ID="txtLength" Value="0"
                RequiredErrorMessage="<% $NopResources:Admin.ProductVariantInfo.Length.RequiredErrorMessage %>"
                MinimumValue="0" MaximumValue="99999999" RangeErrorMessage="<% $NopResources:Admin.ProductVariantInfo.Length.RangeErrorMessage %>">
            </nopCommerce:DecimalTextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblWidth" Text="<% $NopResources:Admin.ProductVariantInfo.Width %>"
                ToolTip="<% $NopResources:Admin.ProductVariantInfo.Width.ToolTip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
            [<%=MeasureManager.BaseDimensionIn.Name%>]:
        </td>
        <td class="adminData">
            <nopCommerce:DecimalTextBox runat="server" CssClass="adminInput" ID="txtWidth" Value="0"
                RequiredErrorMessage="<% $NopResources:Admin.ProductVariantInfo.Width.RequiredErrorMessage %>"
                MinimumValue="0" MaximumValue="99999999" RangeErrorMessage="<% $NopResources:Admin.ProductVariantInfo.Width.RangeErrorMessage %>">
            </nopCommerce:DecimalTextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblHeight" Text="<% $NopResources: Admin.ProductVariantInfo.Height%>"
                ToolTip="<% $NopResources:Admin.ProductVariantInfo.Height.ToolTip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
            [<%=MeasureManager.BaseDimensionIn.Name%>]:
        </td>
        <td class="adminData">
            <nopCommerce:DecimalTextBox runat="server" CssClass="adminInput" ID="txtHeight" Value="0"
                RequiredErrorMessage="<% $NopResources:Admin.ProductVariantInfo.Height.RequiredErrorMessage %>"
                MinimumValue="0" MaximumValue="99999999" RangeErrorMessage="<% $NopResources:Admin.ProductVariantInfo.Height.RangeErrorMessage %>">
            </nopCommerce:DecimalTextBox>
        </td>
    </tr>
    <tr class="adminGroup">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblProductPublished" Text="<% $NopResources:Admin.ProductVariantInfo.Published %>"
                ToolTip="<% $NopResources:Admin.ProductVariantInfo.Published.ToolTip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:CheckBox ID="cbPublished" runat="server" Checked="True"></asp:CheckBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblDisplayOrder" Text="<% $NopResources:Admin.ProductVariantInfo.DisplayOrder %>"
                ToolTip="<% $NopResources:Admin.ProductVariantInfo.DisplayOrder.ToolTip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <nopCommerce:NumericTextBox runat="server" CssClass="adminInput" ID="txtDisplayOrder"
                Value="1" RequiredErrorMessage="<% $NopResources:Admin.ProductVariantInfo.DisplayOrder.RequiredErrorMessage %>"
                MinimumValue="-99999" MaximumValue="99999" RangeErrorMessage="<% $NopResources:Admin.ProductVariantInfo.DisplayOrder.RangeErrorMessage %>">
            </nopCommerce:NumericTextBox>
        </td>
    </tr>
</table>
