<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.CustomerInfoControl"
    CodeBehind="CustomerInfo.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="ToolTipLabel" Src="ToolTipLabelControl.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="EmailTextBox" Src="EmailTextBox.ascx" %>
<table class="adminContent">
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblCustomerEmailTitle" Text="<% $NopResources:Admin.CustomerInfo.Email %>"
                ToolTip="<% $NopResources:Admin.CustomerInfo.Email.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <nopCommerce:EmailTextBox runat="server" ID="txtEmail" />
        </td>
    </tr>
    <tr runat="server" id="pnlUsername">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblCustomerUsernameTitle" Text="<% $NopResources:Admin.CustomerInfo.Username %>"
                ToolTip="<% $NopResources:Admin.CustomerInfo.Username.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:TextBox runat="server" ID="txtUsername" />
            <asp:Label runat="server" ID="lblUsername" />
        </td>
    </tr>
    <tr runat="server" id="pnlPassword">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblPasswordTitle" Text="<% $NopResources:Admin.CustomerInfo.Password %>"
                ToolTip="<% $NopResources:Admin.CustomerInfo.Password.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:TextBox runat="server" ID="txtPassword" />
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblGenderTitle" Text="<% $NopResources:Admin.CustomerInfo.Gender %>"
                ToolTip="<% $NopResources:Admin.CustomerInfo.Gender.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:RadioButton runat="server" ID="rbGenderM" GroupName="Gender" Text="<% $NopResources:Admin.CustomerInfo.Gender.Male %>"
                Checked="true" />
            <asp:RadioButton runat="server" ID="rbGenderF" GroupName="Gender" Text="<% $NopResources:Admin.CustomerInfo.Gender.Female %>" />
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblFirstNameTitle" Text="<% $NopResources:Admin.CustomerInfo.FirstName %>"
                ToolTip="<% $NopResources:Admin.CustomerInfo.FirstName.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:TextBox runat="server" ID="txtFirstName" />
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblLastNameTitle" Text="<% $NopResources:Admin.CustomerInfo.LastName %>"
                ToolTip="<% $NopResources:Admin.CustomerInfo.LastName.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:TextBox runat="server" ID="txtLastName" />
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblDateOfBirthTitle" Text="<% $NopResources:Admin.CustomerInfo.DateOfBirth %>"
                ToolTip="<% $NopResources:Admin.CustomerInfo.DateOfBirth.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:TextBox runat="server" ID="txtDateOfBirth" />
            <asp:ImageButton runat="Server" ID="iDateOfBirth" ImageUrl="~/images/Calendar_scheduleHS.png"
                AlternateText="<% $NopResources:Admin.CustomerInfo.DateOfBirth.ShowCalendar %>" /><br />
            <ajaxToolkit:CalendarExtender ID="cDateOfBirthButtonExtender" runat="server" TargetControlID="txtDateOfBirth"
                PopupButtonID="iDateOfBirth" />
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblCompanyTitle" Text="<% $NopResources:Admin.CustomerInfo.Company %>"
                ToolTip="<% $NopResources:Admin.CustomerInfo.Company.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtCompany" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblStreetAddressTitle" Text="<% $NopResources:Admin.CustomerInfo.Address %>"
                ToolTip="<% $NopResources:Admin.CustomerInfo.Address.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtStreetAddress" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblStreetAddress2Title" Text="<% $NopResources:Admin.CustomerInfo.Address2 %>"
                ToolTip="<% $NopResources:Admin.CustomerInfo.Address2.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtStreetAddress2" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblZipPostalCodeTitle" Text="<% $NopResources:Admin.CustomerInfo.Zip %>"
                ToolTip="<% $NopResources:Admin.CustomerInfo.Zip.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtZipPostalCode" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblCityTitle" Text="<% $NopResources:Admin.CustomerInfo.City %>"
                ToolTip="<% $NopResources:Admin.CustomerInfo.City.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtCity" runat="server" MaxLength="40"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblCountryTitle" Text="<% $NopResources:Admin.CustomerInfo.Country %>"
                ToolTip="<% $NopResources:Admin.CustomerInfo.Country.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:DropDownList ID="ddlCountry" AutoPostBack="True" runat="server" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblStateProvinceTitle" Text="<% $NopResources:Admin.CustomerInfo.State %>"
                ToolTip="<% $NopResources:Admin.CustomerInfo.State.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:DropDownList ID="ddlStateProvince" AutoPostBack="False" runat="server" Width="137px">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblTelephoneNumberTitle" Text="<% $NopResources:Admin.CustomerInfo.Phone %>"
                ToolTip="<% $NopResources:Admin.CustomerInfo.Phone.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtPhoneNumber" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr runat="server" id="pnlFaxNumber">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblFaxNumberTitle" Text="<% $NopResources:Admin.CustomerInfo.Fax %>"
                ToolTip="<% $NopResources:Admin.CustomerInfo.Fax.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtFaxNumber" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblNewsletterTitle" Text="<% $NopResources:Admin.CustomerInfo.Newsletter %>"
                ToolTip="<% $NopResources:Admin.CustomerInfo.Newsletter.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:CheckBox ID="cbNewsletter" runat="server"></asp:CheckBox>
        </td>
    </tr>
    <tr runat="server" id="pnlTimeZone">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblTimeZoneTitle" Text="<% $NopResources:Admin.CustomerInfo.TimeZone %>"
                ToolTip="<% $NopResources:Admin.CustomerInfo.TimeZone.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:DropDownList ID="ddlTimeZone" runat="server">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblAffiliateTitle" Text="<% $NopResources:Admin.CustomerInfo.Affiliate %>"
                ToolTip="<% $NopResources:Admin.CustomerInfo.Affiliate.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:DropDownList ID="ddlAffiliate" AutoPostBack="False" CssClass="adminInput" runat="server">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblIsTaxExempt" Text="<% $NopResources:Admin.CustomerInfo.TaxExempt %>"
                ToolTip="<% $NopResources:Admin.CustomerInfo.TaxExempt.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:CheckBox ID="cbIsTaxExempt" runat="server"></asp:CheckBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblIsAdminTitle" Text="<% $NopResources:Admin.CustomerInfo.Admin %>"
                ToolTip="<% $NopResources:Admin.CustomerInfo.Admin.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:CheckBox ID="cbIsAdmin" runat="server"></asp:CheckBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblIsForumModerator" Text="<% $NopResources:Admin.CustomerInfo.ForumModerator %>"
                ToolTip="<% $NopResources:Admin.CustomerInfo.ForumModerator.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:CheckBox ID="cbIsForumModerator" runat="server"></asp:CheckBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblAdminComment" Text="<% $NopResources:Admin.CustomerInfo.AdminComment %>"
                ToolTip="<% $NopResources:Admin.CustomerInfo.AdminComment.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtAdminComment" runat="server" CssClass="adminInput" TextMode="MultiLine"
                Height="100"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblActiveTitle" Text="<% $NopResources:Admin.CustomerInfo.Active %>"
                ToolTip="<% $NopResources:Admin.CustomerInfo.Active.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:CheckBox ID="cbActive" runat="server" Checked="true"></asp:CheckBox>
        </td>
    </tr>
    <tr runat="server" id="pnlRegistrationDate">
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblRegistrationDateTitle" Text="<% $NopResources:Admin.CustomerInfo.RegistrationDate %>"
                ToolTip="<% $NopResources:Admin.CustomerInfo.RegistrationDate.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:Label ID="lblRegistrationDate" runat="server"></asp:Label>
        </td>
    </tr>
</table>
