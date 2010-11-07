<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.ProductInfoAddControl"
    CodeBehind="ProductInfoAdd.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="NumericTextBox" Src="NumericTextBox.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="SimpleTextBox" Src="SimpleTextBox.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="DecimalTextBox" Src="DecimalTextBox.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="ToolTipLabel" Src="ToolTipLabelControl.ascx" %>
<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<table class="adminContent">
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblProductName" Text="<% $NopResources:Admin.ProductInfo.ProductName %>"
                ToolTip="<% $NopResources:Admin.ProductInfo.ProductName.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <nopCommerce:SimpleTextBox runat="server" CssClass="adminInput" ID="txtName" ErrorMessage="<% $NopResources:Admin.ProductInfo.ProductName.ErrorMessage %>">
            </nopCommerce:SimpleTextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblShortDescription" Text="<% $NopResources:Admin.ProductInfo.ShortDescription %>"
                ToolTip="<% $NopResources:Admin.ProductInfo.ShortDescription.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtShortDescription" runat="server" CssClass="adminInput" TextMode="MultiLine"
                Height="100"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblFullDescription" Text="<% $NopResources:Admin.ProductInfo.FullDescription %>"
                ToolTip="<% $NopResources: Admin.ProductInfo.FullDescription.Tooltip%>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <FCKeditorV2:FCKeditor ID="txtFullDescription" runat="server" AutoDetectLanguage="false"
                Height="350">
            </FCKeditorV2:FCKeditor>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblAdminComment" Text="<% $NopResources:Admin.ProductInfo.AdminComment %>"
                ToolTip="<% $NopResources:Admin.ProductInfo.AdminComment.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtAdminComment" runat="server" CssClass="adminInput" TextMode="MultiLine"
                Height="100"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblProductType" Text="<% $NopResources:Admin.ProductInfo.ProductType %>"
                ToolTip="<% $NopResources:Admin.ProductInfo.ProductType.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:DropDownList ID="ddlProductType" AutoPostBack="False" CssClass="adminInput"
                runat="server">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblProductTemplate" Text="<% $NopResources:Admin.ProductInfo.ProductTemplate %>"
                ToolTip="<% $NopResources:Admin.ProductInfo.ProductTemplate.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:DropDownList ID="ddlTemplate" AutoPostBack="False" CssClass="adminInput" runat="server">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblShowOnHomePage" Text="<% $NopResources:Admin.ProductInfo.ShowOnHomePage %>"
                ToolTip="<% $NopResources:Admin.ProductInfo.ShowOnHomePage.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:CheckBox ID="cbShowOnHomePage" runat="server"></asp:CheckBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblProductPublished" Text="<% $NopResources:Admin.ProductInfo.Published %>"
                ToolTip="<% $NopResources:Admin.ProductInfo.Published.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:CheckBox ID="cbPublished" runat="server" Checked="True"></asp:CheckBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblAllowCustomerReviews" Text="<% $NopResources:Admin.ProductInfo.AllowCustomerReviews %>"
                ToolTip="<% $NopResources:Admin.ProductInfo.AllowCustomerReviews.Tooltip %>"
                ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:CheckBox ID="cbAllowCustomerReviews" runat="server" Checked="True"></asp:CheckBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblAllowCustomerRatings" Text="<% $NopResources:Admin.ProductInfo.AllowCustomerRatings %>"
                ToolTip="<% $NopResources:Admin.ProductInfo.AllowCustomerRatings.Tooltip %>"
                ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:CheckBox ID="cbAllowCustomerRatings" runat="server" Checked="True"></asp:CheckBox>
        </td>
    </tr>
    <tr class="adminGroup">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblSKU" Text="<% $NopResources:Admin.ProductInfo.SKU %>"
                ToolTip="<% $NopResources:Admin.ProductInfo.SKU.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtSKU" runat="server" CssClass="adminInput"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblManufacturerPartNumber" Text="<% $NopResources:Admin.ProductInfo.ManufacturerPartNumber %>"
                ToolTip="<% $NopResources: Admin.ProductInfo.ManufacturerPartNumber.Tooltip%>"
                ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtManufacturerPartNumber" CssClass="adminInput" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr class="adminGroup">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblIsDownload" Text="<% $NopResources:Admin.ProductInfo.IsDownload %>"
                ToolTip="<% $NopResources:Admin.ProductInfo.IsDownload.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:CheckBox ID="cbIsDownload" runat="server" Checked="False" AutoPostBack="True"
                OnCheckedChanged="cbIsDownload_CheckedChanged"></asp:CheckBox>
        </td>
    </tr>
    <tr runat="server" id="pnlUseDownloadURL">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblUseDownloadURL" Text="<% $NopResources: Admin.ProductInfo.UseDownloadURL%>"
                ToolTip="<% $NopResources:Admin.ProductInfo.UseDownloadURL.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:CheckBox ID="cbUseDownloadURL" runat="server" Checked="False" AutoPostBack="True"
                OnCheckedChanged="cbUseDownloadURL_CheckedChanged"></asp:CheckBox>
        </td>
    </tr>
    <tr runat="server" id="pnlDownloadURL">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblDownloadURL" Text="<% $NopResources: Admin.ProductInfo.DownloadURL%>"
                ToolTip="<% $NopResources: Admin.ProductInfo.DownloadURL.Tooltip%>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtDownloadURL" CssClass="adminInput" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr runat="server" id="pnlDownloadFile">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblDownloadFile" Text="<% $NopResources: Admin.ProductInfo.DownloadFile%>"
                ToolTip="<% $NopResources:Admin.ProductInfo.DownloadFile.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:FileUpload ID="fuProductVariantDownload" CssClass="adminInput" runat="server"
                ToolTip="Choose a file" />
        </td>
    </tr>
    <tr runat="server" id="pnlUnlimitedDownloads">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblUnlimitedDownloads" Text="<% $NopResources:Admin.ProductInfo.UnlimitedDownloads %>"
                ToolTip="<% $NopResources:Admin.ProductInfo.UnlimitedDownloads.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:CheckBox ID="cbUnlimitedDownloads" runat="server" Checked="True" AutoPostBack="True"
                OnCheckedChanged="cbUnlimitedDownloads_CheckedChanged"></asp:CheckBox>
        </td>
    </tr>
    <tr runat="server" id="pnlMaxNumberOfDownloads">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblMaxNumberOfDownloads" Text="<% $NopResources: Admin.ProductInfo.MaxNumberOfDownloads%>"
                ToolTip="<% $NopResources:Admin.ProductInfo.MaxNumberOfDownloads.Tooltip %>"
                ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <nopCommerce:NumericTextBox runat="server" CssClass="adminInput" ID="txtMaxNumberOfDownloads"
                RequiredErrorMessage="<% $NopResources:Admin.ProductInfo.MaxNumberOfDownloads.RequiredErrorMessage %>"
                MinimumValue="0" MaximumValue="99999999" Value="10" RangeErrorMessage="<% $NopResources:Admin.ProductInfo.MaxNumberOfDownloads.RangeErrorMessage %>">
            </nopCommerce:NumericTextBox>
        </td>
    </tr>
    <tr runat="server" id="pnlHasSampleDownload">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblHasSampleDownload" Text="<% $NopResources:Admin.ProductInfo.HasSampleDownload %>"
                ToolTip="<% $NopResources:Admin.ProductInfo.HasSampleDownload.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:CheckBox ID="cbHasSampleDownload" runat="server" Checked="False" AutoPostBack="True"
                OnCheckedChanged="cbHasSampleDownload_CheckedChanged"></asp:CheckBox>
        </td>
    </tr>
    <tr runat="server" id="pnlUseSampleDownloadURL">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblUseSampleDownloadURL" Text="<% $NopResources: Admin.ProductInfo.UseSampleDownloadURL%>"
                ToolTip="<% $NopResources: Admin.ProductInfo.UseSampleDownloadURL.Tooltip%>"
                ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:CheckBox ID="cbUseSampleDownloadURL" runat="server" Checked="False" AutoPostBack="True"
                OnCheckedChanged="cbUseSampleDownloadURL_CheckedChanged"></asp:CheckBox>
        </td>
    </tr>
    <tr runat="server" id="pnlSampleDownloadURL">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblSampleDownloadURL" Text="<% $NopResources: Admin.ProductInfo.SampleDownloadURL%>"
                ToolTip="<% $NopResources:Admin.ProductInfo.SampleDownloadURL.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtSampleDownloadURL" CssClass="adminInput" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr runat="server" id="pnlSampleDownloadFile">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblSampleDownloadFile" Text="<% $NopResources:Admin.ProductInfo.SampleDownloadFile %>"
                ToolTip="<% $NopResources:Admin.ProductInfo.SampleDownloadFile.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:FileUpload ID="fuProductVariantSampleDownload" CssClass="adminInput" runat="server"
                ToolTip="Choose a file" />
        </td>
    </tr>
    <tr class="adminGroup">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblShipEnabled" Text="<% $NopResources: Admin.ProductInfo.ShipEnabled%>"
                ToolTip="<% $NopResources:Admin.ProductInfo.ShipEnabled.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:CheckBox ID="cbIsShipEnabled" runat="server" Checked="True"></asp:CheckBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblFreeShipping" Text="<% $NopResources:Admin.ProductInfo.FreeShipping %>"
                ToolTip="<% $NopResources:Admin.ProductInfo.FreeShipping.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:CheckBox ID="cbIsFreeShipping" runat="server" Checked="False"></asp:CheckBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblAdditionalShippingCharge" Text="<% $NopResources: Admin.ProductInfo.AdditionalShippingCharge%>"
                ToolTip="<% $NopResources:Admin.ProductInfo.AdditionalShippingCharge.Tooltip %>"
                ToolTipImage="~/Administration/Common/ico-help.gif" />
            [<%=CurrencyManager.PrimaryStoreCurrency.CurrencyCode%>]:
        </td>
        <td class="adminData">
            <nopCommerce:DecimalTextBox runat="server" CssClass="adminInput" ID="txtAdditionalShippingCharge"
                Value="0" RequiredErrorMessage="<% $NopResources:Admin.ProductInfo.AdditionalShippingCharge.RequiredErrorMessage %>"
                MinimumValue="0" MaximumValue="99999999" RangeErrorMessage="<% $NopResources:Admin.ProductInfo.AdditionalShippingCharge.RangeErrorMessage %>">
            </nopCommerce:DecimalTextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblTaxExempt" Text="<% $NopResources:Admin.ProductInfo.TaxExempt %>"
                ToolTip="<% $NopResources:Admin.ProductInfo.TaxExempt.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:CheckBox ID="cbIsTaxExempt" runat="server" Checked="False"></asp:CheckBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblTaxCategory" Text="<% $NopResources:Admin.ProductInfo.TaxCategory %>"
                ToolTip="<% $NopResources:Admin.ProductInfo.TaxCategory.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:DropDownList ID="ddlTaxCategory" CssClass="adminInput" AutoPostBack="False"
                runat="server">
            </asp:DropDownList>
        </td>
    </tr>
    <tr class="adminGroup">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblManageStock" Text="<% $NopResources:Admin.ProductInfo.ManageStock %>"
                ToolTip="<% $NopResources:Admin.ProductInfo.ManageStock.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:CheckBox ID="cbManageStock" runat="server" Checked="True" AutoPostBack="True"
                OnCheckedChanged="cbManageStock_CheckedChanged"></asp:CheckBox>
        </td>
    </tr>
    <tr runat="server" id="pnlStockQuantity">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblStockQuantity" Text="<% $NopResources:Admin.ProductInfo.StockQuantity %>"
                ToolTip="<% $NopResources:Admin.ProductInfo.StockQuantity.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <nopCommerce:NumericTextBox runat="server" CssClass="adminInput" ID="txtStockQuantity"
                RequiredErrorMessage="<% $NopResources:Admin.ProductInfo.StockQuantity.RequiredErrorMessage %>"
                MinimumValue="0" MaximumValue="99999999" Value="10000" RangeErrorMessage="<% $NopResources:Admin.ProductInfo.StockQuantity.RangeErrorMessage %>">
            </nopCommerce:NumericTextBox>
        </td>
    </tr>
    <tr runat="server" id="pnlMinStockQuantity">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblMinStockQuantity" Text="<% $NopResources:Admin.ProductInfo.MinStockQuantity %>"
                ToolTip="<% $NopResources:Admin.ProductInfo.MinStockQuantity.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <nopCommerce:NumericTextBox runat="server" CssClass="adminInput" ID="txtMinStockQuantity"
                RequiredErrorMessage="<% $NopResources:Admin.ProductInfo.MinStockQuantity.RequiredErrorMessage %>"
                MinimumValue="0" MaximumValue="99999999" Value="0" RangeErrorMessage="<% $NopResources:Admin.ProductInfo.MinStockQuantity.RangeErrorMessage %>">
            </nopCommerce:NumericTextBox>
        </td>
    </tr>
    <tr runat="server" id="pnlLowStockActivity">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblLowStockActivity" Text="<% $NopResources:Admin.ProductInfo.LowStockActivity %>"
                ToolTip="<% $NopResources:Admin.ProductInfo.LowStockActivity.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:DropDownList ID="ddlLowStockActivity" AutoPostBack="False" CssClass="adminInput"
                runat="server">
            </asp:DropDownList>
        </td>
    </tr>
    <tr runat="server" id="pnlNotifyForQuantityBelow">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblNotifyForQuantityBelow" Text="<% $NopResources:Admin.ProductInfo.NotifyForQuantityBelow %>"
                ToolTip="<% $NopResources:Admin.ProductInfo.NotifyForQuantityBelow.Tooltip %>"
                ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <nopCommerce:NumericTextBox runat="server" CssClass="adminInput" ID="txtNotifyForQuantityBelow"
                RequiredErrorMessage="<% $NopResources:Admin.ProductInfo.NotifyForQuantityBelow.RequiredErrorMessage%>"
                MinimumValue="1" MaximumValue="99999999" Value="1" RangeErrorMessage="<% $NopResources:Admin.ProductInfo.NotifyForQuantityBelow.RangeErrorMessage %>">
            </nopCommerce:NumericTextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblOrderMinimumQuantity" Text="<% $NopResources:Admin.ProductInfo.OrderMinimumQuantity %>"
                ToolTip="<% $NopResources:Admin.ProductInfo.OrderMinimumQuantity.Tooltip %>"
                ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <nopCommerce:NumericTextBox runat="server" CssClass="adminInput" ID="txtOrderMinimumQuantity"
                RequiredErrorMessage="<% $NopResources:Admin.ProductInfo.OrderMinimumQuantity.RequiredErrorMessage %>"
                MinimumValue="1" MaximumValue="99999999" Value="1" RangeErrorMessage="<% $NopResources:Admin.ProductInfo.OrderMinimumQuantity.RangeErrorMessage %>">
            </nopCommerce:NumericTextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblOrderMaximumQuantity" Text="<% $NopResources:Admin.ProductInfo.OrderMaximumQuantity %>"
                ToolTip="<% $NopResources:Admin.ProductInfo.OrderMaximumQuantity.Tooltip %>"
                ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <nopCommerce:NumericTextBox runat="server" CssClass="adminInput" ID="txtOrderMaximumQuantity"
                RequiredErrorMessage="<% $NopResources:Admin.ProductInfo.OrderMaximumQuantity.RequiredErrorMessage %>"
                MinimumValue="1" MaximumValue="99999999" Value="10000" RangeErrorMessage="<% $NopResources:Admin.ProductInfo.OrderMaximumQuantity.RangeErrorMessage %>">
            </nopCommerce:NumericTextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblWarehouse" Text="<% $NopResources:Admin.ProductInfo.Warehouse %>"
                ToolTip="<% $NopResources: Admin.ProductInfo.Warehouse.Tooltip%>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:DropDownList ID="ddlWarehouse" CssClass="adminInput" AutoPostBack="False" runat="server">
            </asp:DropDownList>
        </td>
    </tr>
    <tr class="adminGroup">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblDisableBuyButton" Text="<% $NopResources:Admin.ProductInfo.DisableBuyButton %>"
                ToolTip="<% $NopResources:Admin.ProductInfo.DisableBuyButton.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:CheckBox ID="cbDisableBuyButton" runat="server" Checked="False"></asp:CheckBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblPrice" Text="<% $NopResources:Admin.ProductInfo.Price %>"
                ToolTip="<% $NopResources:Admin.ProductInfo.Price.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
            [<%=CurrencyManager.PrimaryStoreCurrency.CurrencyCode%>]:
        </td>
        <td class="adminData">
            <nopCommerce:DecimalTextBox runat="server" CssClass="adminInput" ID="txtPrice" Value="0"
                RequiredErrorMessage="<% $NopResources:Admin.ProductInfo.Price.RequiredErrorMessage %>"
                MinimumValue="0" MaximumValue="99999999" RangeErrorMessage="<% $NopResources:Admin.ProductInfo.Price.RangeErrorMessage %>">
            </nopCommerce:DecimalTextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblOldPrice" Text="<% $NopResources:Admin.ProductInfo.OldPrice %>"
                ToolTip="<% $NopResources:Admin.ProductInfo.OldPrice.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
            [<%=CurrencyManager.PrimaryStoreCurrency.CurrencyCode%>]:
        </td>
        <td class="adminData">
            <nopCommerce:DecimalTextBox runat="server" CssClass="adminInput" ID="txtOldPrice"
                Value="0" RequiredErrorMessage="<% $NopResources:Admin.ProductInfo.OldPrice.RequiredErrorMessage%>"
                MinimumValue="0" MaximumValue="99999999" RangeErrorMessage="<% $NopResources:Admin.ProductInfo.OldPrice.RangeErrorMessage %>">
            </nopCommerce:DecimalTextBox>
        </td>
    </tr>
    <tr class="adminGroup">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblAvailableStartDateTime" Text="<% $NopResources:Admin.ProductInfo.AvailableStartDateTime %>"
                ToolTip="<% $NopResources:Admin.ProductInfo.AvailableStartDateTime.ToolTip %>"
                ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:TextBox runat="server" ID="txtAvailableStartDateTime" />
            <asp:ImageButton runat="Server" ID="iAvailableStartDateTime" ImageUrl="~/images/Calendar_scheduleHS.png"
                AlternateText="<% $NopResources:Admin.ProductInfo.AvailableStartDateTime.ClickToShowCalendar %>" /><br />
            <ajaxToolkit:CalendarExtender ID="cAvailableStartDateTimeButtonExtender" runat="server"
                TargetControlID="txtAvailableStartDateTime" PopupButtonID="iAvailableStartDateTime" />
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblAvailableEndDateTime" Text="<% $NopResources:Admin.ProductInfo.AvailableEndDateTime %>"
                ToolTip="<% $NopResources:Admin.ProductInfo.AvailableEndDateTime.ToolTip %>"
                ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:TextBox runat="server" ID="txtAvailableEndDateTime" />
            <asp:ImageButton runat="Server" ID="iAvailableEndDateTime" ImageUrl="~/images/Calendar_scheduleHS.png"
                AlternateText="<% $NopResources:Admin.ProductInfo.AvailableEndDateTime.ClickToShowCalendar %>" /><br />
            <ajaxToolkit:CalendarExtender ID="cAvailableEndDateTimeButtonExtender" runat="server"
                TargetControlID="txtAvailableEndDateTime" PopupButtonID="iAvailableEndDateTime" />
        </td>
    </tr>
    <tr class="adminGroup">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblWeight" Text="<% $NopResources:Admin.ProductInfo.Weight %>"
                ToolTip="<% $NopResources:Admin.ProductInfo.Weight.ToolTip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
            [<%=MeasureManager.BaseWeightIn.Name%>]:
        </td>
        <td class="adminData">
            <nopCommerce:DecimalTextBox runat="server" CssClass="adminInput" ID="txtWeight" Value="0"
                RequiredErrorMessage="<% $NopResources:Admin.ProductInfo.Weight.RequiredErrorMessage %>"
                MinimumValue="0" MaximumValue="99999999" RangeErrorMessage="<% $NopResources:Admin.ProductInfo.Weight.RangeErrorMessage %>">
            </nopCommerce:DecimalTextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblLength" Text="<% $NopResources:Admin.ProductInfo.Length %>"
                ToolTip="<% $NopResources:Admin.ProductInfo.Length.ToolTip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
            [<%=MeasureManager.BaseDimensionIn.Name%>]:
        </td>
        <td class="adminData">
            <nopCommerce:DecimalTextBox runat="server" CssClass="adminInput" ID="txtLength" Value="0"
                RequiredErrorMessage="<% $NopResources:Admin.ProductInfo.Length.RequiredErrorMessage %>"
                MinimumValue="0" MaximumValue="99999999" RangeErrorMessage="<% $NopResources:Admin.ProductInfo.Length.RangeErrorMessage %>">
            </nopCommerce:DecimalTextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblWidth" Text="<% $NopResources:Admin.ProductInfo.Width %>"
                ToolTip="<% $NopResources:Admin.ProductInfo.Width.ToolTip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
            [<%=MeasureManager.BaseDimensionIn.Name%>]:
        </td>
        <td class="adminData">
            <nopCommerce:DecimalTextBox runat="server" CssClass="adminInput" ID="txtWidth" Value="0"
                RequiredErrorMessage="<% $NopResources:Admin.ProductInfo.Width.RequiredErrorMessage %>"
                MinimumValue="0" MaximumValue="99999999" RangeErrorMessage="<% $NopResources:Admin.ProductInfo.Width.RangeErrorMessage %>">
            </nopCommerce:DecimalTextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblHeight" Text="<% $NopResources:Admin.ProductInfo.Height %>"
                ToolTip="<% $NopResources:Admin.ProductInfo.Height.ToolTip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
            [<%=MeasureManager.BaseDimensionIn.Name%>]:
        </td>
        <td class="adminData">
            <nopCommerce:DecimalTextBox runat="server" CssClass="adminInput" ID="txtHeight" Value="0"
                RequiredErrorMessage="<% $NopResources:Admin.ProductInfo.Height.RequiredErrorMessage %>"
                MinimumValue="0" MaximumValue="99999999" RangeErrorMessage="<% $NopResources:Admin.ProductInfo.Height.RangeErrorMessage %>">
            </nopCommerce:DecimalTextBox>
        </td>
    </tr>
</table>
