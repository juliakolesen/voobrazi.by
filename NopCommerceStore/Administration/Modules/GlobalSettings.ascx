<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.GlobalSettingsControl"
    CodeBehind="GlobalSettings.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="SimpleTextBox" Src="SimpleTextBox.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="NumericTextBox" Src="NumericTextBox.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="EmailTextBox" Src="EmailTextBox.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="ToolTipLabel" Src="ToolTipLabelControl.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="ThemeSelector" Src="ThemeSelectorControl.ascx" %>
<div class="section-header">
    <div class="title">
        <img src="Common/ico-configuration.png" alt="<%=GetLocaleResourceString("Admin.GlobalSettings.Title")%>" />
        <%=GetLocaleResourceString("Admin.GlobalSettings.Title")%>
    </div>
    <div class="options">
        <asp:Button runat="server" Text="<% $NopResources:Admin.GlobalSettings.SaveButton.Text %>"
            CssClass="adminButtonBlue" ID="btnSave" OnClick="btnSave_Click" ToolTip="<% $NopResources:Admin.GlobalSettings.SaveButton.Tooltip %>" />
    </div>
</div>
<div>
    <ajaxToolkit:TabContainer runat="server" ID="CommonSettingsTabs" ActiveTabIndex="0">
        <ajaxToolkit:TabPanel runat="server" ID="pnlGeneral" HeaderText="<% $NopResources:Admin.GlobalSettings.General.Title %>">
            <ContentTemplate>
                <table class="adminContent">
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ID="lblStoreName" Text="<% $NopResources:Admin.GlobalSettings.General.StoreName %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.General.StoreName.Tooltip %>"
                                ToolTipImage="~/Administration/Common/ico-help.gif" />
                        </td>
                        <td class="adminData">
                            <nopCommerce:SimpleTextBox runat="server" ID="txtStoreName" CssClass="adminInput"
                                ErrorMessage="<% $NopResources:Admin.GlobalSettings.General.ErrorMessage %>">
                            </nopCommerce:SimpleTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ID="lblStoreUrl" Text="<% $NopResources:Admin.GlobalSettings.General.StoreUrl %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.General.StoreUrl.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                        </td>
                        <td class="adminData">
                            <nopCommerce:SimpleTextBox runat="server" ID="txtStoreURL" CssClass="adminInput"
                                ErrorMessage="<% $NopResources:Admin.GlobalSettings.General.StoreUrl.ErrorMessage %>">
                            </nopCommerce:SimpleTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ID="lblStoreClosed" Text="<% $NopResources:Admin.GlobalSettings.General.StoreClosed %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.General.StoreClosed.Tooltip %>"
                                ToolTipImage="~/Administration/Common/ico-help.gif" />
                        </td>
                        <td class="adminData">
                            <asp:CheckBox runat="server" ID="cbStoreClosed"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ID="lblAnonymousCheckout" Text="<% $NopResources:Admin.GlobalSettings.General.AnonymousCheckout %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.General.AnonymousCheckout.Tooltip %>"
                                ToolTipImage="~/Administration/Common/ico-help.gif" />
                        </td>
                        <td class="adminData">
                            <asp:CheckBox runat="server" ID="cbAnonymousCheckoutAllowed"></asp:CheckBox>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel runat="server" ID="pnlSEODisplay" HeaderText="<% $NopResources:Admin.GlobalSettings.SEODisplay.Title %>">
            <ContentTemplate>
                <table class="adminContent">
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ID="lblStoreNamePrefix" Text="<% $NopResources:Admin.GlobalSettings.SEODisplay.StoreNamePrefix %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.SEODisplay.StoreNamePrefix.Tooltip %>"
                                ToolTipImage="~/Administration/Common/ico-help.gif" />
                        </td>
                        <td class="adminData">
                            <asp:CheckBox runat="server" ID="cbStoreNameInTitle"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblDefaultTitle" Text="<% $NopResources:Admin.GlobalSettings.SEODisplay.DefaultTitle %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.SEODisplay.DefaultTitle.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <nopCommerce:SimpleTextBox runat="server" ID="txtDefaulSEOTitle" CssClass="adminInput"
                                ErrorMessage="<% $NopResources:Admin.GlobalSettings.SEODisplay.ErrorMessage %>">
                            </nopCommerce:SimpleTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblDefaultMetaDescription" Text="<% $NopResources:Admin.GlobalSettings.SEODisplay.MetaDescription %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.SEODisplay.MetaDescription.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <nopCommerce:SimpleTextBox runat="server" ID="txtDefaulSEODescription" CssClass="adminInput"
                                ErrorMessage="<% $NopResources:Admin.GlobalSettings.SEODisplay.MetaDescription.ErrorMessage %>">
                            </nopCommerce:SimpleTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblDefaultMetaKeywords" Text="<% $NopResources:Admin.GlobalSettings.SEODisplay.MetaKeywords %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.SEODisplay.MetaKeywords.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <nopCommerce:SimpleTextBox runat="server" ID="txtDefaulSEOKeywords" CssClass="adminInput"
                                ErrorMessage="<% $NopResources:Admin.GlobalSettings.SEODisplay.MetaKeywords.ErrorMessage %>">
                            </nopCommerce:SimpleTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblPublicStoreTheme" Text="<% $NopResources:Admin.GlobalSettings.SEODisplay.PublicStoreTheme %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.SEODisplay.PublicStoreTheme.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <nopCommerce:ThemeSelector runat="server" ID="ctrlThemeSelector" />
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblShowWelcomeMessage" Text="<% $NopResources:Admin.GlobalSettings.SEODisplay.ShowWelcomeMessage %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.SEODisplay.ShowWelcomeMessage.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <asp:CheckBox runat="server" ID="cbShowWelcomeMessage" />
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblNewsRssLink" Text="<% $NopResources:Admin.GlobalSettings.SEODisplay.NewsRssLink %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.SEODisplay.NewsRssLink.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <asp:CheckBox runat="server" ID="cbShowNewsHeaderRssURL" />
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblBlogRssLink" Text="<% $NopResources:Admin.GlobalSettings.SEODisplay.BlogRssLink %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.SEODisplay.BlogRssLink.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <asp:CheckBox runat="server" ID="cbShowBlogHeaderRssURL" />
                        </td>
                    </tr>
                </table>
                <p>
                    <strong>
                        <%=GetLocaleResourceString("Admin.GlobalSettings.SEODisplay.UrlRewriting")%></strong>
                </p>
                <table class="adminContent">
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblProductUrlRewriteFormat" Text="<% $NopResources:Admin.GlobalSettings.SEODisplay.ProductUrl %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.SEODisplay.ProductUrl.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <nopCommerce:SimpleTextBox runat="server" ID="txtProductUrlRewriteFormat" CssClass="adminInput"
                                ErrorMessage="<% $NopResources:Admin.GlobalSettings.SEODisplay.ProductUrl.ErrorMessage %>" />
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblCategoryUrlRewriteFormat" Text="<% $NopResources:Admin.GlobalSettings.SEODisplay.CategoryUrl %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.SEODisplay.CategoryUrl.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <nopCommerce:SimpleTextBox runat="server" ID="txtCategoryUrlRewriteFormat" CssClass="adminInput"
                                ErrorMessage="<% $NopResources:Admin.GlobalSettings.SEODisplay.CategoryUrl.ErrorMessage %>" />
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblManufacturerUrlRewriteFormat" Text="<% $NopResources:Admin.GlobalSettings.SEODisplay.ManufacturerUrl %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.SEODisplay.ManufacturerUrl.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <nopCommerce:SimpleTextBox runat="server" ID="txtManufacturerUrlRewriteFormat" CssClass="adminInput"
                                ErrorMessage="<% $NopResources:Admin.GlobalSettings.SEODisplay.ManufacturerUrl.ErrorMessage %>" />
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblNewsUrlRewriteFormat" Text="<% $NopResources:Admin.GlobalSettings.SEODisplay.NewsUrl %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.SEODisplay.NewsUrl.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <nopCommerce:SimpleTextBox runat="server" ID="txtNewsUrlRewriteFormat" CssClass="adminInput"
                                ErrorMessage="<% $NopResources:Admin.GlobalSettings.SEODisplay.NewsUrl.ErrorMessage %>" />
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblBlogUrlRewriteFormat" Text="<% $NopResources:Admin.GlobalSettings.SEODisplay.BlogUrl %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.SEODisplay.BlogUrl.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <nopCommerce:SimpleTextBox runat="server" ID="txtBlogUrlRewriteFormat" CssClass="adminInput"
                                ErrorMessage="<% $NopResources:Admin.GlobalSettings.SEODisplay.BlogUrl.ErrorMessage %>" />
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblTopicUrlRewriteFormat" Text="<% $NopResources:Admin.GlobalSettings.SEODisplay.TopicUrl %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.SEODisplay.TopicUrl.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <nopCommerce:SimpleTextBox runat="server" ID="txtTopicUrlRewriteFormat" CssClass="adminInput"
                                ErrorMessage="<% $NopResources:Admin.GlobalSettings.SEODisplay.TopicUrl.ErrorMessage %>" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel runat="server" ID="pnlMedia" HeaderText="<% $NopResources:Admin.GlobalSettings.Media.Title %>">
            <ContentTemplate>
                <table class="adminContent">
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblMaxImageSize" Text="<% $NopResources:Admin.GlobalSettings.Media.MaxImageSize %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.Media.MaxImageSize.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <nopCommerce:NumericTextBox runat="server" CssClass="adminInput" ID="txtMaxImageSize"
                                RequiredErrorMessage="<% $NopResources:Admin.GlobalSettings.Media.MaxImageSize.RequiredErrorMessage %>"
                                MinimumValue="1" MaximumValue="999999" RangeErrorMessage="<% $NopResources:Admin.GlobalSettings.Media.MaxImageSize.RangeErrorMessage %>"
                                Width="50px" />
                            pixels
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblProductThumbSize" Text="<% $NopResources:Admin.GlobalSettings.Media.ProductThumbSize %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.Media.ProductThumbSize.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <nopCommerce:NumericTextBox runat="server" CssClass="adminInput" ID="txtProductThumbSize"
                                RequiredErrorMessage="<% $NopResources:Admin.GlobalSettings.Media.ProductThumbSize.RequiredErrorMessage %>"
                                MinimumValue="1" MaximumValue="999999" RangeErrorMessage="<% $NopResources:Admin.GlobalSettings.Media.ProductThumbSize.RangeErrorMessage %>"
                                Width="50px" />
                            pixels
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblProductDetailSize" Text="<% $NopResources:Admin.GlobalSettings.Media.ProductDetailSize %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.Media.ProductDetailSize.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <nopCommerce:NumericTextBox runat="server" CssClass="adminInput" ID="txtProductDetailSize"
                                RequiredErrorMessage="<% $NopResources:Admin.GlobalSettings.Media.ProductDetailSize.RequiredErrorMessage %>"
                                MinimumValue="1" MaximumValue="999999" RangeErrorMessage="<% $NopResources:Admin.GlobalSettings.Media.ProductDetailSize.RangeErrorMessage %>"
                                Width="50px" />
                            pixels
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblProductVariantSize" Text="<% $NopResources:Admin.GlobalSettings.Media.ProductVariantSize %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.Media.ProductVariantSize.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <nopCommerce:NumericTextBox runat="server" CssClass="adminInput" ID="txtProductVariantSize"
                                RequiredErrorMessage="<% $NopResources:Admin.GlobalSettings.Media.ProductVariantSize.RequiredErrorMessage %>"
                                MinimumValue="1" MaximumValue="999999" RangeErrorMessage="<% $NopResources:Admin.GlobalSettings.Media.ProductVariantSize.RangeErrorMessage %>"
                                Width="50px" />
                            pixels
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblCategoryThumbSize" Text="<% $NopResources:Admin.GlobalSettings.Media.CategoryThumbSize %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.Media.CategoryThumbSize.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <nopCommerce:NumericTextBox runat="server" CssClass="adminInput" ID="txtCategoryThumbSize"
                                RequiredErrorMessage="<% $NopResources:Admin.GlobalSettings.Media.CategoryThumbSize.RequiredErrorMessage %>"
                                MinimumValue="1" MaximumValue="999999" RangeErrorMessage="<% $NopResources:Admin.GlobalSettings.Media.CategoryThumbSize.RangeErrorMessage %>"
                                Width="50px" />
                            pixels
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblManufacturerThumbSize" Text="<% $NopResources:Admin.GlobalSettings.Media.ManufacturerThumbSize %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.Media.ManufacturerThumbSize.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <nopCommerce:NumericTextBox runat="server" CssClass="adminInput" ID="txtManufacturerThumbSize"
                                RequiredErrorMessage="<% $NopResources:Admin.GlobalSettings.Media.ManufacturerThumbSize.RequiredErrorMessage %>"
                                MinimumValue="1" MaximumValue="999999" RangeErrorMessage="<% $NopResources:Admin.GlobalSettings.Media.ManufacturerThumbSize.RangeErrorMessage %>"
                                Width="50px" />
                            pixels
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblShowCartImages" Text="<% $NopResources:Admin.GlobalSettings.Media.ShowCartImages %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.Media.ShowCartImages.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <asp:CheckBox runat="server" ID="cbShowCartImages" />
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblShowWishListImages" Text="<% $NopResources:Admin.GlobalSettings.Media.ShowWishListImages %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.Media.ShowWishListImages.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <asp:CheckBox runat="server" ID="cbShowWishListImages" />
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblShoppingCartThumbnailSize" Text="<% $NopResources:Admin.GlobalSettings.Media.CartThumbSize %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.Media.CartThumbSize.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <nopCommerce:NumericTextBox runat="server" CssClass="adminInput" ID="txtShoppingCartThumbSize"
                                RequiredErrorMessage="<% $NopResources:Admin.GlobalSettings.Media.CartThumbSize.RequiredErrorMessage %>"
                                MinimumValue="1" MaximumValue="999999" RangeErrorMessage="<% $NopResources:Admin.GlobalSettings.Media.CartThumbSize.RangeErrorMessage %>"
                                Width="50px" />
                            pixels
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel runat="server" ID="pnlUnits" HeaderText="<% $NopResources:Admin.GlobalSettings.Units.Title %>">
            <ContentTemplate>
                <table class="adminContent">
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblBaseWeight" Text="<% $NopResources:Admin.GlobalSettings.Units.BaseWeight %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.Units.BaseWeight.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <asp:DropDownList ID="ddlBaseWeight" AutoPostBack="False" CssClass="adminInput" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblBaseDimension" Text="<% $NopResources:Admin.GlobalSettings.Units.BaseDimension %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.Units.BaseDimension.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <asp:DropDownList ID="ddlBaseDimension" AutoPostBack="False" CssClass="adminInput"
                                runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel runat="server" ID="pnlMailSettings" HeaderText="<% $NopResources:Admin.GlobalSettings.MailSettings.Title %>">
            <ContentTemplate>
                <table class="adminContent">
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblStoreAdminEmail" Text="<% $NopResources:Admin.GlobalSettings.MailSettings.Email %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.MailSettings.Email.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <nopCommerce:EmailTextBox runat="server" CssClass="adminInput" ID="txtAdminEmailAddress">
                            </nopCommerce:EmailTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblStoreAdminEmailDisplayName" Text="<% $NopResources:Admin.GlobalSettings.MailSettings.EmailDisplayName %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.MailSettings.EmailDisplayName.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <asp:TextBox ID="txtAdminEmailDisplayName" CssClass="adminInput" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblHost" Text="<% $NopResources:Admin.GlobalSettings.MailSettings.EmailHost %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.MailSettings.EmailHost.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <asp:TextBox ID="txtAdminEmailHost" CssClass="adminInput" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblPort" Text="<% $NopResources:Admin.GlobalSettings.MailSettings.EmailPort %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.MailSettings.EmailPort.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <asp:TextBox ID="txtAdminEmailPort" CssClass="adminInput" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblUser" Text="<% $NopResources:Admin.GlobalSettings.MailSettings.User %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.MailSettings.User.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <asp:TextBox ID="txtAdminEmailUser" CssClass="adminInput" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblPassword" Text="<% $NopResources:Admin.GlobalSettings.MailSettings.Password %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.MailSettings.Password.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <asp:TextBox ID="txtAdminEmailPassword" CssClass="adminInput" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblMailSSL" Text="<% $NopResources:Admin.GlobalSettings.MailSettings.SSL %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.MailSettings.SSL.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <asp:CheckBox ID="cbAdminEmailEnableSsl" runat="server"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblUseDefaultCredentials" Text="<% $NopResources:Admin.GlobalSettings.MailSettings.DefaultCredentials %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.MailSettings.DefaultCredentials.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <asp:CheckBox ID="cbAdminEmailUseDefaultCredentials" runat="server"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <p>
                                <hr />
                                <strong>
                                    <%=GetLocaleResourceString("Admin.GlobalSettings.MailSettings.SendTestEmail")%></strong>
                            </p>
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblTestEmailTo" Text="<% $NopResources:Admin.GlobalSettings.MailSettings.TestEmailTo %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.MailSettings.TestEmailTo.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <nopCommerce:EmailTextBox runat="server" CssClass="adminInput" ID="txtSendEmailTo"
                                ValidationGroup="SendTestEmail"></nopCommerce:EmailTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                        </td>
                        <td class="adminData">
                            <asp:Button ID="btnSendTestEmail" runat="server" Text="<% $NopResources:Admin.GlobalSettings.MailSettings.SendTestEmailButton.Text %>"
                                CssClass="adminButton" OnClick="btnSendTestEmail_Click" ValidationGroup="SendTestEmail"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.MailSettings.SendTestEmailButton.Tooltip %>" />
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                        </td>
                        <td class="adminData" style="color: red">
                            <asp:Label ID="lblSendTestEmailResult" runat="server" EnableViewState="false">
                            </asp:Label>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel runat="server" ID="pnlSecurity" HeaderText="<% $NopResources:Admin.GlobalSettings.Security.Title %>">
            <ContentTemplate>
                <table class="adminContent">
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblEncryptionPrivateKey" Text="<% $NopResources:Admin.GlobalSettings.Security.PrivateKey %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.Security.PrivateKey.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <asp:TextBox ID="txtEncryptionPrivateKey" CssClass="adminInput" runat="server" ValidationGroup="EncryptionPrivateKey"></asp:TextBox>
                            <asp:Button ID="btnChangeEncryptionPrivateKey" runat="server" Text="<% $NopResources:Admin.GlobalSettings.Security.PrivateKeyButton %>"
                                CssClass="adminButton" OnClick="btnChangeEncryptionPrivateKey_Click" ValidationGroup="EncryptionPrivateKey">
                            </asp:Button>
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                        </td>
                        <td class="adminData" style="color: red">
                            <asp:Label ID="lblChangeEncryptionPrivateKeyResult" runat="server" EnableViewState="false">
                            </asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblLoginCaptcha" Text="<% $NopResources:Admin.GlobalSettings.Security.LoginCaptcha %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.Security.LoginCaptcha.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <asp:CheckBox runat="server" ID="cbEnableLoginCaptchaImage"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblRegistrationCaptcha" Text="<% $NopResources:Admin.GlobalSettings.Security.RegistrationCaptcha %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.Security.RegistrationCaptcha.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <asp:CheckBox runat="server" ID="cbEnableRegisterCaptchaImage"></asp:CheckBox>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel runat="server" ID="pnlCustomerProfiles" HeaderText="<% $NopResources:Admin.GlobalSettings.Profiles.Title %>">
            <ContentTemplate>
                <table class="adminContent">
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ID="lblCustomerNameFormat" Text="<% $NopResources:Admin.GlobalSettings.Profiles.NameFormat %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.Profiles.NameFormat.Tooltip %>"
                                ToolTipImage="~/Administration/Common/ico-help.gif" />
                        </td>
                        <td class="adminData">
                            <asp:DropDownList ID="ddlCustomerNameFormat" runat="server" CssClass="adminInput">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ID="lblCustomersAllowedToUploadAvatars"
                                Text="<% $NopResources:Admin.GlobalSettings.Profiles.AllowedAvatars %>" ToolTip="<% $NopResources:Admin.GlobalSettings.Profiles.AllowedAvatars.Tooltip %>"
                                ToolTipImage="~/Administration/Common/ico-help.gif" />
                        </td>
                        <td class="adminData">
                            <asp:CheckBox ID="cbCustomersAllowedToUploadAvatars" runat="server" OnCheckedChanged="cbCustomersAllowedToUploadAvatars_CheckedChanged"
                                AutoPostBack="true"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr runat="server" id="pnlDefaultAvatarEnabled">
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ID="lblDefaultAvatarEnabled" Text="<% $NopResources:Admin.GlobalSettings.Profiles.DefaultAvatar %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.Profiles.DefaultAvatar.Tooltip %>"
                                ToolTipImage="~/Administration/Common/ico-help.gif" />
                        </td>
                        <td class="adminData">
                            <asp:CheckBox ID="cbDefaultAvatarEnabled" runat="server"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ID="lblAllowViewingProfiles" Text="<% $NopResources:Admin.GlobalSettings.Profiles.ViewingProfiles %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.Profiles.ViewingProfiles.Tooltip %>"
                                ToolTipImage="~/Administration/Common/ico-help.gif" />
                        </td>
                        <td class="adminData">
                            <asp:CheckBox ID="cbAllowViewingProfiles" runat="server"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ID="lblShowCustomersLocation" Text="<% $NopResources:Admin.GlobalSettings.Profiles.ShowLocation %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.Profiles.ShowLocation.Tooltip %>"
                                ToolTipImage="~/Administration/Common/ico-help.gif" />
                        </td>
                        <td class="adminData">
                            <asp:CheckBox ID="cbShowCustomersLocation" runat="server"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ID="lblShowCustomersJoinDate" Text="<% $NopResources:Admin.GlobalSettings.Profiles.ShowJoinDate %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.Profiles.ShowJoinDate.Tooltip %>"
                                ToolTipImage="~/Administration/Common/ico-help.gif" />
                        </td>
                        <td class="adminData">
                            <asp:CheckBox ID="cbShowCustomersJoinDate" runat="server"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ID="lblAllowPM" Text="<% $NopResources:Admin.GlobalSettings.Profiles.AllowPM %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.Profiles.AllowPM.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                        </td>
                        <td class="adminData">
                            <asp:CheckBox ID="cbAllowPM" runat="server"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ID="lblAllowCustomersToSetTimeZone" Text="<% $NopResources:Admin.GlobalSettings.Profiles.AllowToSetTimeZone %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.Profiles.AllowToSetTimeZone.Tooltip %>"
                                ToolTipImage="~/Administration/Common/ico-help.gif" />
                        </td>
                        <td class="adminData">
                            <asp:CheckBox runat="server" ID="cbAllowCustomersToSetTimeZone"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ID="lblCurrentTimeZoneInfo" Text="<% $NopResources:Admin.GlobalSettings.Profiles.CurrentTimeZone %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.Profiles.CurrentTimeZone.Tooltip %>"
                                ToolTipImage="~/Administration/Common/ico-help.gif" />
                        </td>
                        <td class="adminData">
                            <asp:Label runat="server" ID="lblCurrentTimeZone">
                            </asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ID="lblDefaultStoreTimeZone" Text="<% $NopResources:Admin.GlobalSettings.Profiles.DefaultTimeZone %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.Profiles.DefaultTimeZone.Tooltip %>"
                                ToolTipImage="~/Administration/Common/ico-help.gif" />
                        </td>
                        <td class="adminData">
                            <asp:DropDownList runat="server" ID="ddlDefaultStoreTimeZone" CssClass="adminInputNoWidth">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel runat="server" ID="pnlOther" HeaderText="<% $NopResources:Admin.GlobalSettings.Other.Title %>">
            <ContentTemplate>
                <table class="adminContent">
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblUsernamesEnabled" Text="<% $NopResources:Admin.GlobalSettings.Other.Usernames %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.Other.Usernames.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <asp:CheckBox runat="server" ID="cbUsernamesEnabled"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblNewCustomerRegistrationDisabled" Text="<% $NopResources:Admin.GlobalSettings.Other.RegistrationDisabled %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.Other.RegistrationDisabled.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <asp:CheckBox runat="server" ID="cbNewCustomerRegistrationDisabled"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ID="lblAllowNavigationOnlyRegisteredCustomers"
                                Text="<% $NopResources:Admin.GlobalSettings.Other.NavigationOnlyRegisteredCustomers %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.Other.NavigationOnlyRegisteredCustomers.Tooltip %>"
                                ToolTipImage="~/Administration/Common/ico-help.gif" />
                        </td>
                        <td class="adminData">
                            <asp:CheckBox runat="server" ID="cbAllowNavigationOnlyRegisteredCustomers"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblRegistrationEmailValidation" Text="<% $NopResources:Admin.GlobalSettings.Other.EmailValidation %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.Other.EmailValidation.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <asp:CheckBox runat="server" ID="cbRegistrationEmailValidation"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblCompareProducts" Text="<% $NopResources:Admin.GlobalSettings.Other.CompareProducts %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.Other.CompareProducts.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <asp:CheckBox runat="server" ID="cbEnableCompareProducts"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblWishList" Text="<% $NopResources:Admin.GlobalSettings.Other.WishList %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.Other.WishList.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <asp:CheckBox runat="server" ID="cbEnableWishlist"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblEmailAFriend" Text="<% $NopResources:Admin.GlobalSettings.Other.EmailAFriend %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.Other.EmailAFriend.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <asp:CheckBox runat="server" ID="cbEnableEmailAFriend"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblRecentlyViewedProductsEnabled" Text="<% $NopResources:Admin.GlobalSettings.Other.RecentlyViewedProducts %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.Other.RecentlyViewedProducts.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <asp:CheckBox runat="server" ID="cbRecentlyViewedProductsEnabled"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblRecentlyAddedProductsEnabled" Text="<% $NopResources:Admin.GlobalSettings.Other.RecentlyAddedProducts %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.Other.RecentlyAddedProducts.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <asp:CheckBox runat="server" ID="cbRecentlyAddedProductsEnabled"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ID="lblNotifyAboutNewProductReviews" Text="<% $NopResources:Admin.GlobalSettings.Other.NotifyNewProductReviews %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.Other.NotifyNewProductReviews.Tooltip %>"
                                ToolTipImage="~/Administration/Common/ico-help.gif" />
                        </td>
                        <td class="adminData">
                            <asp:CheckBox runat="server" ID="cbNotifyAboutNewProductReviews"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr class="adminGroup">
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblShowBestsellersOnHomePage" Text="<% $NopResources:Admin.GlobalSettings.Other.ShowBestsellersOnHomePage %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.Other.ShowBestsellersOnHomePage.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <asp:CheckBox runat="server" ID="cbShowBestsellersOnHomePage"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr class="adminGroup">
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblProductsAlsoPurchased" Text="<% $NopResources:Admin.GlobalSettings.Other.AlsoPurchased %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.Other.AlsoPurchased.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <asp:CheckBox runat="server" ID="cbProductsAlsoPurchased" OnCheckedChanged="cbProductsAlsoPurchased_CheckedChanged"
                                AutoPostBack="true"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr runat="server" id="pnlProductsAlsoPurchasedNumber">
                        <td class="adminTitle">
                            <nopCommerce:ToolTipLabel runat="server" ToolTipImage="~/Administration/Common/ico-help.gif"
                                ID="lblProductsAlsoPurchasedNumber" Text="<% $NopResources:Admin.GlobalSettings.Other.AlsoPurchasedNumber %>"
                                ToolTip="<% $NopResources:Admin.GlobalSettings.Other.AlsoPurchasedNumber.Tooltip %>" />
                        </td>
                        <td class="adminData">
                            <nopCommerce:NumericTextBox runat="server" CssClass="adminInput" ID="txtProductsAlsoPurchasedNumber"
                                RequiredErrorMessage="<% $NopResources:Admin.GlobalSettings.Other.AlsoPurchasedNumber.RequiredErrorMessage %>"
                                MinimumValue="1" MaximumValue="999999" RangeErrorMessage="<% $NopResources:Admin.GlobalSettings.Other.AlsoPurchasedNumber.RangeErrorMessage %>"
                                Width="50px" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
    </ajaxToolkit:TabContainer>
</div>
