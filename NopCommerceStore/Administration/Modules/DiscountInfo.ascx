<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.DiscountInfoControl"
    CodeBehind="DiscountInfo.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="DecimalTextBox" Src="DecimalTextBox.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="SimpleTextBox" Src="SimpleTextBox.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="SelectCustomerRolesControl" Src="SelectCustomerRolesControl.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="ToolTipLabel" Src="ToolTipLabelControl.ascx" %>
<ajaxToolkit:TabContainer runat="server" ID="DiscountTabs" ActiveTabIndex="0">
    <ajaxToolkit:TabPanel runat="server" ID="pnlDiscountInfo" HeaderText="<% $NopResources:Admin.DiscountInfo.DiscountInfo %>">
        <ContentTemplate>
            <table class="adminContent">
                <tr>
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblDiscountType" Text="<% $NopResources:Admin.DiscountInfo.DiscountType %>"
                            ToolTip="<% $NopResources:Admin.DiscountInfo.DiscountType.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:DropDownList ID="ddlDiscountType" CssClass="adminInput" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblDiscountRequirement" Text="<% $NopResources:Admin.DiscountInfo.DiscountRequirement %>"
                            ToolTip="<% $NopResources:Admin.DiscountInfo.DiscountRequirement.Tooltip %>"
                            ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:DropDownList ID="ddlDiscountRequirement" AutoPostBack="True" CssClass="adminInput"
                            runat="server" OnSelectedIndexChanged="ddlDiscountRequirement_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblName" Text="<% $NopResources:Admin.DiscountInfo.Name %>"
                            ToolTip="<% $NopResources:Admin.DiscountInfo.Name.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <nopCommerce:SimpleTextBox runat="server" ID="txtName" CssClass="adminInput" ErrorMessage="<% $NopResources:Admin.DiscountInfo.Name.ErrorMessage %>">
                        </nopCommerce:SimpleTextBox>
                    </td>
                </tr>
                <tr>
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblUsePercentage" Text="<% $NopResources:Admin.DiscountInfo.UsePercentage %>"
                            ToolTip="<% $NopResources:Admin.DiscountInfo.UsePercentage.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:CheckBox runat="server" ID="cbUsePercentage" AutoPostBack="True" OnCheckedChanged="cbUsePercentage_CheckedChanged">
                        </asp:CheckBox>
                    </td>
                </tr>
                <tr runat="server" id="pnlDiscountPercentage">
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblDiscountPercentage" Text="<% $NopResources:Admin.DiscountInfo.DiscountPercentage %>"
                            ToolTip="<% $NopResources:Admin.DiscountInfo.DiscountPercentage.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <nopCommerce:DecimalTextBox runat="server" CssClass="adminInput" ID="txtDiscountPercentage"
                            Value="0" RequiredErrorMessage="<% $NopResources:Admin.DiscountInfo.DiscountPercentage.RequiredErrorMessage %>"
                            RangeErrorMessage="<% $NopResources:Admin.DiscountInfo.DiscountPercentage.RangeErrorMessage %>"
                            MinimumValue="0" MaximumValue="100"></nopCommerce:DecimalTextBox>
                    </td>
                </tr>
                <tr runat="server" id="pnlDiscountAmount">
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblDiscountAmount" Text="<% $NopResources:Admin.DiscountInfo.DiscountAmount %>"
                            ToolTip="<% $NopResources:Admin.DiscountInfo.DiscountAmount.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                        [<%=CurrencyManager.PrimaryStoreCurrency.CurrencyCode%>]:
                    </td>
                    <td class="adminData">
                        <nopCommerce:DecimalTextBox runat="server" CssClass="adminInput" ID="txtDiscountAmount"
                            Value="0" RequiredErrorMessage="<% $NopResources:Admin.DiscountInfo.DiscountAmount.RequiredErrorMessage %>"
                            MinimumValue="0" MaximumValue="999999" RangeErrorMessage="<% $NopResources:Admin.DiscountInfo.DiscountAmount.RangeErrorMessage %>">
                        </nopCommerce:DecimalTextBox>
                    </td>
                </tr>
                <tr>
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblStartDate" Text="<% $NopResources:Admin.DiscountInfo.StartDate %>"
                            ToolTip="<% $NopResources:Admin.DiscountInfo.StartDate.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:TextBox runat="server" ID="txtStartDate" />
                        <asp:ImageButton runat="Server" ID="iStartDate" ImageUrl="~/images/Calendar_scheduleHS.png"
                            AlternateText="<% $NopResources:Admin.DiscountInfo.StartDate.ShowCalendar %>" /><br />
                        <ajaxToolkit:CalendarExtender ID="cStartDateButtonExtender" runat="server" TargetControlID="txtStartDate"
                            PopupButtonID="iStartDate" />
                    </td>
                </tr>
                <tr>
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblEndDate" Text="<% $NopResources:Admin.DiscountInfo.EndDate %>"
                            ToolTip="<% $NopResources:Admin.DiscountInfo.EndDate.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:TextBox runat="server" ID="txtEndDate" />
                        <asp:ImageButton runat="Server" ID="iEndDate" ImageUrl="~/images/Calendar_scheduleHS.png"
                            AlternateText="<% $NopResources:Admin.DiscountInfo.EndDate.ShowCalendar %>" /><br />
                        <ajaxToolkit:CalendarExtender ID="cEndDateButtonExtender" runat="server" TargetControlID="txtEndDate"
                            PopupButtonID="iEndDate" />
                    </td>
                </tr>
                <tr>
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblRequiresCouponCode" Text="<% $NopResources:Admin.DiscountInfo.RequiresCouponCode %>"
                            ToolTip="<% $NopResources:Admin.DiscountInfo.RequiresCouponCode.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:CheckBox runat="server" ID="cbRequiresCouponCode" AutoPostBack="True" OnCheckedChanged="cbRequiresCouponCode_CheckedChanged">
                        </asp:CheckBox>
                    </td>
                </tr>
                <tr runat="server" id="pnlCouponCode">
                    <td class="adminTitle">
                        <nopCommerce:ToolTipLabel runat="server" ID="lblCouponCode" Text="<% $NopResources:Admin.DiscountInfo.CouponCode %>"
                            ToolTip="<% $NopResources:Admin.DiscountInfo.CouponCode.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
                    </td>
                    <td class="adminData">
                        <asp:TextBox ID="txtCouponCode" runat="server" CssClass="adminInput"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </ajaxToolkit:TabPanel>
    <ajaxToolkit:TabPanel runat="server" ID="pnlCustomerRoles" HeaderText="<% $NopResources:Admin.DiscountInfo.CustomerRoles %>">
        <ContentTemplate>
            <table class="adminContent">
                <tr>
                    <td>
                        <nopCommerce:SelectCustomerRolesControl ID="CustomerRoleMappingControl" runat="server"
                            CssClass="adminInput"></nopCommerce:SelectCustomerRolesControl>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </ajaxToolkit:TabPanel>
</ajaxToolkit:TabContainer>