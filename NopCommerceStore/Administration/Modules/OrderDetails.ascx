<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.OrderDetailsControl"
    CodeBehind="OrderDetails.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="ToolTipLabel" Src="ToolTipLabelControl.ascx" %>
<div class="section-header">
    <div class="title">
        <img src="Common/ico-sales.png" alt="<%=GetLocaleResourceString("Admin.OrderDetails.Title")%>" />
        <%=GetLocaleResourceString("Admin.OrderDetails.Title")%>
        <a href="Orders.aspx" title="<%=GetLocaleResourceString("Admin.OrderDetails.BackToOrders")%>">
            (<%=GetLocaleResourceString("Admin.OrderDetails.BackToOrders")%>)</a>
    </div>
    <div class="options">
        <asp:Button ID="btnGetInvoicePDF" runat="server" CssClass="adminButtonBlue" Text="<% $NopResources:Admin.OrderDetails.InvoicePDF.Text %>"
            OnClick="btnGetInvoicePDF_Click" CausesValidation="false" ToolTip="<% $NopResources:Admin.OrderDetails.InvoicePDF.Tooltip %>" />
        <asp:Button ID="DeleteButton" runat="server" CssClass="adminButtonBlue" Text="<% $NopResources:Admin.OrderDetails.DeleteButton.Text %>"
            OnClick="DeleteButton_Click" CausesValidation="false" ToolTip="<% $NopResources:Admin.OrderDetails.DeleteButton.Tooltip %>" />
    </div>
</div>
<ajaxToolkit:TabContainer runat="server" ID="OrderTabs" ActiveTabIndex="0">
    <ajaxToolkit:TabPanel runat="server" ID="pnlOrderInfo" HeaderText="<% $NopResources:Admin.OrderDetails.OrderInfo %>">
        <ContentTemplate>
            <table class="adminContent">
                <tr>
                    <td class="adminTitle">
                        <strong>
                            <nopCommerce:ToolTipLabel runat="server" ID="lblOrderStatusTitle" Text="<% $NopResources:Admin.OrderDetails.OrderStatus %>"
                                ToolTip="<% $NopResources:Admin.OrderDetails.OrderStatus.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                        </strong>
                    </td>
                    <td class="adminData">
                        <b>
                            <asp:Label ID="lblOrderStatus" runat="server"></asp:Label></b>&nbsp;
                        <asp:Button ID="CancelOrderButton" runat="server" CssClass="adminButton" Text="<% $NopResources:Admin.OrderDetails.CancelButton.Text %>"
                            OnClick="CancelOrderButton_Click" CausesValidation="false"></asp:Button>
                        <ajaxToolkit:ConfirmButtonExtender ID="ConfirmCancelOrderButtonExtender" runat="server"
                            TargetControlID="CancelOrderButton" DisplayModalPopupID="ModalPopupExtenderCancelOrder" />
                        <br />
                        <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderCancelOrder" runat="server"
                            TargetControlID="CancelOrderButton" PopupControlID="pnlCancelOrderPopup" OkControlID="cancelOrderButtonOk"
                            CancelControlID="cancelOrderButtonCancel" BackgroundCssClass="modalBackground" />
                        <asp:Panel ID="pnlCancelOrderPopup" runat="server" Style="display: none; width: 250px;
                            background-color: White; border-width: 2px; border-color: Black; border-style: solid;
                            padding: 20px;">
                            <div style="text-align: center;">
                                <%=GetLocaleResourceString("Admin.Common.AreYouSure")%>
                                <br />
                                <br />
                                <asp:Button ID="cancelOrderButtonOk" runat="server" Text="<% $NopResources:Admin.Common.OK %>" CssClass="adminButton"
                                    CausesValidation="false" />
                                <asp:Button ID="cancelOrderButtonCancel" runat="server" Text="<% $NopResources:Admin.Common.Cancel %>" CssClass="adminButton"
                                    CausesValidation="false" />
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblOrderIDTitle" Text="<% $NopResources:Admin.OrderDetails.OrderID %>"
                            ToolTip="<% $NopResources:Admin.OrderDetails.OrderID.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:Label ID="lblOrderID" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblOrderGUIDTitle" Text="<% $NopResources:Admin.OrderDetails.OrderGUID %>"
                            ToolTip="<% $NopResources:Admin.OrderDetails.OrderGUID.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:Label ID="lblOrderGUID" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr class="adminGroup">
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblCustomerTitle" Text="<% $NopResources:Admin.OrderDetails.Customer %>"
                            ToolTip="<% $NopResources:Admin.OrderDetails.Customer.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:Label ID="lblCustomer" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr runat="server" id="divAffiliate">
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblAffiliateTitle" Text="<% $NopResources:Admin.OrderDetails.Affiliate %>"
                            ToolTip="<% $NopResources:Admin.OrderDetails.Affiliate.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:Label ID="lblAffiliate" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblOrderDiscountTitle" Text="<% $NopResources:Admin.OrderDetails.Discount %>"
                            ToolTip="<% $NopResources:Admin.OrderDetails.Discount.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:Label ID="lblOrderDiscount" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr runat="server" id="pnlOrderSubtotalInclTax">
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblOrderSubtotalInclTaxTitle" Text="<% $NopResources:Admin.OrderDetails.SubtotalInclTax %>"
                            ToolTip="<% $NopResources:Admin.OrderDetails.SubtotalInclTax.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:Label ID="lblOrderSubtotalInclTax" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr runat="server" id="pnlOrderSubtotalExclTax">
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblOrderSubtotalExclTaxTitle" Text="<% $NopResources: Admin.OrderDetails.SubtotalExclTax%>"
                            ToolTip="<% $NopResources:Admin.OrderDetails.SubtotalExclTax.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:Label ID="lblOrderSubtotalExclTax" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr runat="server" id="pnlOrderShippingInclTax">
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblOrderShippingInclTaxTitle" Text="<% $NopResources:Admin.OrderDetails.ShippingInclTax %>"
                            ToolTip="<% $NopResources:Admin.OrderDetails.ShippingInclTax.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:Label ID="lblOrderShippingInclTax" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr runat="server" id="pnlOrderShippingExclTax">
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblOrderShippingExclTaxTitle" Text="<% $NopResources:Admin.OrderDetails.ShippingExclTax %>"
                            ToolTip="<% $NopResources:Admin.OrderDetails.ShippingExclTax.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:Label ID="lblOrderShippingExclTax" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr runat="server" id="pnlPaymentMethodAdditionalFeeInclTax">
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblPaymentMethodAdditionalFeeInclTaxTitle"
                            Text="<% $NopResources:Admin.OrderDetails.PaymentMethodAdditionalFeeInclTax %>"
                            ToolTip="<% $NopResources:Admin.OrderDetails.PaymentMethodAdditionalFeeInclTax.Tooltip %>"
                            ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:Label ID="lblPaymentMethodAdditionalFeeInclTax" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr runat="server" id="pnlPaymentMethodAdditionalFeeExclTax">
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblPaymentMethodAdditionalFeeExclTaxTitle"
                            Text="<% $NopResources:Admin.OrderDetails.PaymentMethodAdditionalFeeExclTax %>"
                            ToolTip="<% $NopResources:Admin.OrderDetails.PaymentMethodAdditionalFeeExclTax.Tooltip %>"
                            ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:Label ID="lblPaymentMethodAdditionalFeeExclTax" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblOrderTaxTitle" Text="<% $NopResources: Admin.OrderDetails.OrderTax%>"
                            ToolTip="<% $NopResources:Admin.OrderDetails.OrderTax.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:Label ID="lblOrderTax" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblOrderTotalTitle" Text="<% $NopResources:Admin.OrderDetails.OrderTotal %>"
                            ToolTip="<% $NopResources:Admin.OrderDetails.OrderTotal.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:Label ID="lblOrderTotal" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr class="adminGroup">
                    <td class="adminTitle">
                    </td>
                    <td class="adminData">
                    </td>
                </tr>
                <tr class="adminGroup" runat="server" id="pnlCartType">
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblCartTypeTitle" Text="<% $NopResources:Admin.OrderDetails.CartType %>"
                            ToolTip="<% $NopResources:Admin.OrderDetails.CartType.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:Label ID="lblCardType" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr runat="server" id="pnlCardName">
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblCardNameTitle" Text="<% $NopResources:Admin.OrderDetails.CardName %>"
                            ToolTip="<% $NopResources:Admin.OrderDetails.CardName.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:Label ID="lblCardName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr runat="server" id="pnlCardNumber">
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblCardNumberTitle" Text="<% $NopResources:Admin.OrderDetails.CardNumber %>"
                            ToolTip="<% $NopResources:Admin.OrderDetails.CardNumber.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:Label ID="lblCardNumber" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr runat="server" id="pnlCardCVV2">
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblCardCVV2Title" Text="<% $NopResources:Admin.OrderDetails.CardCVV2 %>"
                            ToolTip="<% $NopResources:Admin.OrderDetails.CardCVV2.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:Label ID="lblCardCVV2" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr runat="server" id="pnlCardExpiryMonth">
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblCardExpiryMonthTitle" Text="<% $NopResources:Admin.OrderDetails.CardExpiryMonth %>"
                            ToolTip="<% $NopResources:Admin.OrderDetails.CardExpiryMonth.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:Label ID="lblCardExpirationMonth" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr runat="server" id="pnlCardExpiryYear">
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblCardExpiryYear" Text="<% $NopResources:Admin.OrderDetails.CardExpiryYear %>"
                            ToolTip="<% $NopResources:Admin.OrderDetails.CardExpiryYear.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:Label ID="lblCardExpirationYear" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblPONumberTitle" Text="<% $NopResources:Admin.OrderDetails.PONumber %>"
                            ToolTip="<% $NopResources:Admin.OrderDetails.PONumber.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:Label ID="lblPONumber" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblPaymentMethodTitle" Text="<% $NopResources:Admin.OrderDetails.PaymentMethod %>"
                            ToolTip="<% $NopResources:Admin.OrderDetails.PaymentMethod.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:Label ID="lblPaymentMethodName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblPaymentStatusTitle" Text="<% $NopResources:Admin.OrderDetails.PaymentStatus %>"
                            ToolTip="<% $NopResources:Admin.OrderDetails.PaymentStatus.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:Label runat="server" ID="lblPaymentStatus"></asp:Label>&nbsp;
                        <asp:Button ID="btnCapture" CssClass="adminButton" runat="server" Text="<% $NopResources:Admin.OrderDetails.CaptureButton.Text %>"
                            OnClick="btnCapture_Click" ToolTip="<% $NopResources:Admin.OrderDetails.CaptureButton.Tooltip %>" />&nbsp;
                        <asp:Button ID="btnMarkAsPaid" CssClass="adminButton" runat="server" Text="<% $NopResources:Admin.OrderDetails.MarkAsPaidButton.Text %>"
                            OnClick="btnMarkAsPaid_Click" ToolTip="<% $NopResources:Admin.OrderDetails.MarkAsPaidButton.Tooltip %>" />
                        <div style="color: red">
                            <b>
                                <asp:Label runat="server" ID="lblCaptureError" EnableViewState="false"></asp:Label>
                            </b>
                        </div>
                    </td>
                    </td>
                </tr>
                <tr>
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblOrderCreatedOnTitle" Text="<% $NopResources:Admin.OrderDetails.CreatedOn %>"
                            ToolTip="<% $NopResources:Admin.OrderDetails.CreatedOn.Tooltip%>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:Label ID="lblCreatedOn" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </ajaxToolkit:TabPanel>
    <ajaxToolkit:TabPanel runat="server" ID="pnlOrderBillingInfo" HeaderText="<% $NopResources:Admin.OrderDetails.BillingInfo %>">
        <ContentTemplate>
            <table class="adminContent">
                <tr>
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblBillingAddress" Text="<% $NopResources:Admin.OrderDetails.BillingAddress %>"
                            ToolTip="<% $NopResources:Admin.OrderDetails.BillingAddress.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <div>
                            <div style="font-weight: bold">
                                <asp:Literal ID="lBillingFirstName" runat="server"></asp:Literal>
                                <asp:Literal ID="lBillingLastName" runat="server"></asp:Literal>
                            </div>
                            <div>
                                <%=GetLocaleResourceString("Admin.OrderDetails.BillingAddress.Email")%>
                                <asp:Literal ID="lBillingEmail" runat="server"></asp:Literal></div>
                            <div>
                                <%=GetLocaleResourceString("Admin.OrderDetails.BillingAddress.Phone")%>
                                <asp:Literal ID="lBillingPhoneNumber" runat="server"></asp:Literal></div>
                            <div>
                                <%=GetLocaleResourceString("Admin.OrderDetails.BillingAddress.Fax")%>
                                <asp:Literal ID="lBillingFaxNumber" runat="server"></asp:Literal></div>
                            <asp:Panel ID="pnlBillingCompany" runat="server">
                                <asp:Literal ID="lBillingCompany" runat="server"></asp:Literal></asp:Panel>
                            <div>
                                <asp:Literal ID="lBillingAddress1" runat="server"></asp:Literal></div>
                            <asp:Panel ID="pnlBillingAddress2" runat="server">
                                <asp:Literal ID="lBillingAddress2" runat="server"></asp:Literal></asp:Panel>
                            <div>
                                <asp:Literal ID="lBillingCity" runat="server"></asp:Literal>,
                                <asp:Literal ID="lBillingStateProvince" runat="server"></asp:Literal>
                                <asp:Literal ID="lBillingZipPostalCode" runat="server"></asp:Literal></div>
                            <asp:Panel ID="pnlBillingCountry" runat="server">
                                <asp:Literal ID="lBillingCountry" runat="server"></asp:Literal></asp:Panel>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </ajaxToolkit:TabPanel>
    <ajaxToolkit:TabPanel runat="server" ID="pnlOrderShippingInfo" HeaderText="<% $NopResources:Admin.OrderDetails.ShippingInfo %>">
        <ContentTemplate>
            <table class="adminContent">
                <tr runat="server" id="divShippingNotRequired" visible="false">
                    <td class="adminTitle">
                    </td>
                    <td class="adminData">
                        <%=GetLocaleResourceString("Admin.OrderDetails.ShippingNotRequired")%>
                    </td>
                </tr>
                <tr runat="server" id="divShippingAddress">
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblShippingAddress" Text="<% $NopResources:Admin.OrderDetails.ShippingAddress %>"
                            ToolTip="<% $NopResources:Admin.OrderDetails.ShippingAddress.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <div>
                            <div style="font-weight: bold">
                                <asp:Literal ID="lShippingFirstName" runat="server"></asp:Literal>
                                <asp:Literal ID="lShippingLastName" runat="server"></asp:Literal>
                            </div>
                            <div>
                                <%=GetLocaleResourceString("Admin.OrderDetails.ShippingAddress.Email")%>
                                <asp:Literal ID="lShippingEmail" runat="server"></asp:Literal></div>
                            <div>
                                <%=GetLocaleResourceString("Admin.OrderDetails.ShippingAddress.Phone")%>
                                <asp:Literal ID="lShippingPhoneNumber" runat="server"></asp:Literal></div>
                            <div>
                                <%=GetLocaleResourceString("Admin.OrderDetails.ShippingAddress.Fax")%>
                                <asp:Literal ID="lShippingFaxNumber" runat="server"></asp:Literal></div>
                            <asp:Panel ID="pnlShippingCompany" runat="server">
                                <asp:Literal ID="lShippingCompany" runat="server"></asp:Literal></asp:Panel>
                            <div>
                                <asp:Literal ID="lShippingAddress1" runat="server"></asp:Literal></div>
                            <asp:Panel ID="pnlShippingAddress2" runat="server">
                                <asp:Literal ID="lShippingAddress2" runat="server"></asp:Literal></asp:Panel>
                            <div>
                                <asp:Literal ID="lShippingCity" runat="server"></asp:Literal>,
                                <asp:Literal ID="lShippingStateProvince" runat="server"></asp:Literal>
                                <asp:Literal ID="lShippingZipPostalCode" runat="server"></asp:Literal></div>
                            <asp:Panel ID="pnlShippingCountry" runat="server">
                                <asp:Literal ID="lShippingCountry" runat="server"></asp:Literal></asp:Panel>
                        </div>
                    </td>
                </tr>
                <tr runat="server" id="divShippingWeight">
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblOrderWeightTitle" Text="<% $NopResources:Admin.OrderDetails.OrderWeight %>"
                            ToolTip="<% $NopResources:Admin.OrderDetails.OrderWeight.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:Label ID="lblOrderWeight" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr runat="server" id="divShippingMethod">
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblShippingMethodTitle" Text="<% $NopResources:Admin.OrderDetails.ShippingMethod %>"
                            ToolTip="<% $NopResources:Admin.OrderDetails.ShippingMethod.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:Label ID="lblShippingMethod" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr runat="server" id="divShippedDate">
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblShippedDateTitle" Text="<% $NopResources:Admin.OrderDetails.ShippedDate %>"
                            ToolTip="<% $NopResources:Admin.OrderDetails.ShippedDate.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:Label ID="lblShippedDate" runat="server"></asp:Label>
                        <asp:Button ID="btnSetAsShipped" CssClass="adminButton" runat="server" Text="<% $NopResources:Admin.OrderDetails.SetAsShippedButton.Text %>"
                            OnClick="btnSetAsShipped_Click"></asp:Button>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </ajaxToolkit:TabPanel>
    <ajaxToolkit:TabPanel runat="server" ID="pnlOrderProducts" HeaderText="<% $NopResources:Admin.OrderDetails.Products %>">
        <ContentTemplate>
            <table class="adminContent">
                <tr>
                    <td class="adminData">
                        <asp:GridView ID="gvOrderProductVariants" runat="server" AutoGenerateColumns="False"
                            Width="100%">
                            <Columns>
                                <asp:BoundField DataField="OrderProductVariantID" HeaderText="Order Product Variant ID"
                                    Visible="False"></asp:BoundField>
                                <asp:TemplateField HeaderText="<% $NopResources:Admin.OrderDetails.Products.Name %>"
                                    ItemStyle-Width="30%">
                                    <ItemTemplate>
                                        <div style="padding-left: 10px; padding-right: 10px; text-align: left;">
                                            <em><a href='<%#GetProductURL(Convert.ToInt32(Eval("ProductVariantID")))%>' title="<%#GetLocaleResourceString("Admin.OrderDetails.Products.Name.Tooltip")%>">
                                                <%#Server.HtmlEncode(GetProductVariantName(Convert.ToInt32(Eval("ProductVariantID"))))%></a></em>
                                            <%#Eval("AttributeDescription")%>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<% $NopResources:Admin.OrderDetails.Products.Download %>"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <%#GetDownloadURL(Container.DataItem as OrderProductVariant)%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<% $NopResources:Admin.OrderDetails.Products.Price %>"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <%#GetProductVariantUnitPrice(Container.DataItem as OrderProductVariant)%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Quantity"
                                    HeaderText="Quantity" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="10%"
                                    ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                <asp:TemplateField HeaderText="<% $NopResources:Admin.OrderDetails.Products.Discount %>"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <%#GetOrderProductVariantDiscountAmount(Container.DataItem as OrderProductVariant)%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<% $NopResources:Admin.OrderDetails.Products.Discount %>"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <%#GetOrderProductVariantSubTotal(Container.DataItem as OrderProductVariant)%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </ajaxToolkit:TabPanel>
    <ajaxToolkit:TabPanel runat="server" ID="pnlOrderNotes" HeaderText="<% $NopResources:Admin.OrderDetails.OrderNotes %>">
        <ContentTemplate>
            <table class="adminContent">
                <tr>
                    <td class="adminData" colspan="2">
                        <asp:GridView ID="gvOrderNotes" runat="server" DataKeyNames="OrderNoteID" AutoGenerateColumns="False"
                            Width="100%" OnRowDeleting="gvOrderNotes_RowDeleting">
                            <Columns>
                                <asp:BoundField DataField="OrderNoteID" HeaderText="OrderNoteID" Visible="False">
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="<% $NopResources:Admin.OrderDetails.OrderNotes.CreatedOn %>"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <%#DateTimeHelper.ConvertToUserTime((DateTime)Eval("CreatedOn")).ToString()%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<% $NopResources:Admin.OrderDetails.OrderNotes.Note %>"
                                    ItemStyle-Width="70%">
                                    <ItemTemplate>
                                        <div style="padding-left: 10px; padding-right: 10px; text-align: left;">
                                            <%#OrderManager.FormatOrderNoteText((string)Eval("Note"))%>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<% $NopResources:Admin.OrderDetails.OrderNotes.Delete %>"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Button ID="btnDelete" runat="server" CssClass="adminButton" Text="<% $NopResources:Admin.OrderDetails.OrderNotes.Delete %>"
                                            CausesValidation="false" CommandName="Delete" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td class="adminData" colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblNewOrderNote" Text="<% $NopResources:Admin.OrderDetails.OrderNotes.New.Note %>"
                            ToolTip="<% $NopResources:Admin.OrderDetails.OrderNotes.New.Note.Tooltip %>"
                            ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:TextBox runat="server" ID="txtNewOrderNote" TextMode="MultiLine" Height="150px"
                            Width="500px"></asp:TextBox>
                        <br />
                        <asp:Button ID="btnAddNewOrderNote" CssClass="adminButton" runat="server" Text="<% $NopResources:Admin.OrderDetails.OrderNotes.New.AddNewButton.Text %>"
                            OnClick="btnAddNewOrderNote_Click" ToolTip="<% $NopResources:Admin.OrderDetails.OrderNotes.New.AddNewButton.Tooltip %>" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </ajaxToolkit:TabPanel>
</ajaxToolkit:TabContainer>
<ajaxToolkit:ConfirmButtonExtender ID="ConfirmDeleteButtonExtender" runat="server"
    TargetControlID="DeleteButton" DisplayModalPopupID="ModalPopupExtenderDelete" />
<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderDelete" runat="server" TargetControlID="DeleteButton"
    PopupControlID="pnlDeletePopup" OkControlID="deleteButtonOk" CancelControlID="deleteButtonCancel"
    BackgroundCssClass="modalBackground" />
<asp:Panel ID="pnlDeletePopup" runat="server" Style="display: none; width: 250px;
    background-color: White; border-width: 2px; border-color: Black; border-style: solid;
    padding: 20px;">
    <div style="text-align: center;">
        <%=GetLocaleResourceString("Admin.Common.AreYouSure")%>
        <br />
        <br />
        <asp:Button ID="deleteButtonOk" runat="server" Text="<% $NopResources:Admin.Common.Yes %>" CssClass="adminButton" CausesValidation="false" />
        <asp:Button ID="deleteButtonCancel" runat="server" Text="<% $NopResources:Admin.Common.No %>" CssClass="adminButton"
            CausesValidation="false" />
    </div>
</asp:Panel>
