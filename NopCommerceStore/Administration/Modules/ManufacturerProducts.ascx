<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.ManufacturerProductsControl"
    CodeBehind="ManufacturerProducts.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="NumericTextBox" Src="NumericTextBox.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="SimpleTextBox" Src="SimpleTextBox.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="ToolTipLabel" Src="ToolTipLabelControl.ascx" %>

<script language="javascript">
    function OpenWindow(query, w, h, scroll) {
        var l = (screen.width - w) / 2;
        var t = (screen.height - h) / 2;

        winprops = 'resizable=1, height=' + h + ',width=' + w + ',top=' + t + ',left=' + l + 'w';
        if (scroll) winprops += ',scrollbars=1';
        var f = window.open(query, "_blank", winprops);
    }
</script>


<table class="adminContent">
    <tr>
        <td width="100%">
            <asp:UpdatePanel ID="upProductManufacturerMappings" runat="server">
                <ContentTemplate>
                    <nopCommerce:NopDataPagerGridView ID="gvProductManufacturerMappings" runat="server"
                        AutoGenerateColumns="false" Width="100%" OnPageIndexChanging="gvProductManufacturerMappings_PageIndexChanging"
                        AllowPaging="true">
                        <Columns>
                            <asp:TemplateField HeaderText="<% $NopResources:Admin.ManufacturerProducts.Product %>"
                                ItemStyle-Width="60%">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbProductInfo" runat="server" Text='<%#Server.HtmlEncode(Eval("ProductInfo").ToString()) %>'
                                        Checked='<%# Eval("IsMapped") %>' ToolTip="<% $NopResources:Admin.ManufacturerProducts.Product.Tooltip %>" />
                                    <asp:HiddenField ID="hfProductID" runat="server" Value='<%# Eval("ProductID") %>' />
                                    <asp:HiddenField ID="hfProductManufacturerID" runat="server" Value='<%# Eval("ProductManufacturerID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<% $NopResources:Admin.ManufacturerProducts.View %>"
                                HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="13%" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <a href='ProductDetails.aspx?ProductID=<%# Eval("ProductID") %>' title="<%#GetLocaleResourceString("Admin.ManufacturerProducts.View.Tooltip")%>">
                                        <%#GetLocaleResourceString("Admin.ManufacturerProducts.View")%></a>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<% $NopResources:Admin.ManufacturerProducts.FeaturedProduct %>"
                                HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="13%" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbFeatured" runat="server" Checked='<%# Eval("IsFeatured") %>'
                                        ToolTip="<% $NopResources:Admin.ManufacturerProducts.FeaturedProduct.Tooltip %>" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<% $NopResources:Admin.ManufacturerProducts.DisplayOrder %>"
                                HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="13%" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <nopCommerce:NumericTextBox runat="server" CssClass="adminInput" Width="50px" ID="txtDisplayOrder"
                                        Value='<%# Eval("DisplayOrder") %>' RequiredErrorMessage="<% $NopResources:Admin.ManufacturerProducts.DisplayOrder.RequiredErrorMessage %>"
                                        RangeErrorMessage="<% $NopResources:Admin.ManufacturerProducts.DisplayOrder.RangeErrorMessage %>"
                                        MinimumValue="-99999" MaximumValue="99999"></nopCommerce:NumericTextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </nopCommerce:NopDataPagerGridView>
                    <div class="pager">
                        <asp:DataPager ID="pagerProductManufacturerMappings" runat="server" PageSize="15"
                            PagedControlID="gvProductManufacturerMappings">
                            <Fields>
                                <asp:NextPreviousPagerField ButtonCssClass="command" FirstPageText="�" PreviousPageText="�"
                                    RenderDisabledButtonsAsLabels="true" ShowFirstPageButton="true" ShowPreviousPageButton="true"
                                    ShowLastPageButton="false" ShowNextPageButton="false" />
                                <asp:NumericPagerField ButtonCount="7" NumericButtonCssClass="command" CurrentPageLabelCssClass="current"
                                    NextPreviousButtonCssClass="command" />
                                <asp:NextPreviousPagerField ButtonCssClass="command" LastPageText="�" NextPageText="�"
                                    RenderDisabledButtonsAsLabels="true" ShowFirstPageButton="false" ShowPreviousPageButton="false"
                                    ShowLastPageButton="true" ShowNextPageButton="true" />
                            </Fields>
                        </asp:DataPager>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdateProgress ID="updateProgressManufacturerProductMappings" runat="server"
                AssociatedUpdatePanelID="upProductManufacturerMappings">
                <ProgressTemplate>
                    <div class="progress">
                        <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/images/UpdateProgress.gif" />
                        <%=GetLocaleResourceString("Admin.Common.Wait...")%>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </td>
    </tr>
    <tr>
        <td width="100%">
            <asp:Button ID="btnAddNew" runat="server" CssClass="adminButton" Text="<% $NopResources:Admin.ManufacturerProducts.AddNewButton.Text %>" />
            <asp:Button ID="btnRefresh" runat="server" Style="display: none" CausesValidation="false"
                CssClass="adminButton" Text="Refresh" OnClick="btnRefresh_Click" ToolTip="Refresh list" />
        </td>
    </tr>
</table>
