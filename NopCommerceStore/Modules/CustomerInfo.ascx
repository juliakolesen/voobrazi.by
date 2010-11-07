<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.CustomerInfoControl"
    CodeBehind="CustomerInfo.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="EmailTextBox" Src="~/Modules/EmailTextBox.ascx" %>
<div class="CustomerInfoBox">
    <div class="messageError">
        <asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
    </div>
    <div class="clear">
    </div>
    <div class="SectionTitle">
        <%=GetLocaleResourceString("Account.YourPersonalDetails")%>
    </div>
    <div class="clear">
    </div>
    <div class="SectionBody">
        <table class="TableContainer">
            <tbody>
                <tr class="Row">
                    <td class="ItemName">
                        <%=GetLocaleResourceString("Account.Gender")%>:
                    </td>
                    <td class="ItemValue">
                        <asp:RadioButton runat="server" ID="rbGenderM" GroupName="Gender" Text="<% $NopResources:Account.GenderMale %>"
                            Checked="true" />
                        <asp:RadioButton runat="server" ID="rbGenderF" GroupName="Gender" Text="<% $NopResources:Account.GenderFemale %>" />
                    </td>
                </tr>
                <tr class="Row">
                    <td class="ItemName">
                        <%=GetLocaleResourceString("Account.FirstName")%>:
                    </td>
                    <td class="ItemValue">
                        <asp:TextBox ID="txtFirstName" runat="server" MaxLength="40"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName"
                            ErrorMessage="<% $NopResources:Account.FirstNameIsRequired %>" ToolTip="<% $NopResources:Account.FirstNameIsRequired %>"
                            ValidationGroup="CustomerInfo">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr class="Row">
                    <td class="ItemName">
                        <%=GetLocaleResourceString("Account.LastName")%>:
                    </td>
                    <td class="ItemValue">
                        <asp:TextBox ID="txtLastName" runat="server" MaxLength="40"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtLastName"
                            ErrorMessage="<% $NopResources:Account.LastNameIsRequired %>" ToolTip="<% $NopResources:Account.LastNameIsRequired %>"
                            ValidationGroup="CustomerInfo">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr class="Row">
                    <td class="ItemName">
                        <%=GetLocaleResourceString("Account.DateOfBirth")%>:
                    </td>
                    <td class="ItemValue">
                        <asp:TextBox runat="server" ID="txtDateOfBirth" />
                        <asp:ImageButton runat="Server" ID="iDateOfBirth" ImageUrl="~/images/Calendar_scheduleHS.png"
                            AlternateText="Click to show calendar" /><br />
                        <ajaxToolkit:CalendarExtender ID="cDateOfBirthButtonExtender" runat="server" TargetControlID="txtDateOfBirth"
                            PopupButtonID="iDateOfBirth" />
                    </td>
                </tr>
                <tr class="Row">
                    <td class="ItemName">
                        <%=GetLocaleResourceString("Account.E-Mail")%>:
                    </td>
                    <td class="ItemValue">
                        <asp:Label runat="server" ID="lblEmail" />
                        <asp:TextBox ID="txtEmail" runat="server" MaxLength="40"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                            ErrorMessage="<% $NopResources:Account.E-MailRequired %>" ToolTip="<% $NopResources:Account.E-MailRequired %>"
                            Display="Dynamic" ValidationGroup="CustomerInfo">*</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator runat="server" ID="revEmail" Display="Dynamic" ControlToValidate="txtEmail"
                            ErrorMessage="<% $NopResources:Account.InvalidEmail %>" ToolTip="<% $NopResources:Account.InvalidEmail %>"
                            ValidationExpression="[\w\.-]+(\+[\w-]*)?@([\w-]+\.)+[\w-]+" ValidationGroup="CustomerInfo">*</asp:RegularExpressionValidator>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div class="clear">
    </div>
    <div class="SectionTitle">
        <%=GetLocaleResourceString("Account.CompanyDetails")%>
    </div>
    <div class="clear">
    </div>
    <div class="SectionBody">
        <table class="TableContainer">
            <tbody>
                <tr class="Row">
                    <td class="ItemName">
                        <%=GetLocaleResourceString("Account.CompanyName")%>:
                    </td>
                    <td class="ItemValue">
                        <asp:TextBox ID="txtCompany" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div class="clear">
    </div>
    <div class="SectionTitle">
        <%=GetLocaleResourceString("Account.YourAddress")%>
    </div>
    <div class="clear">
    </div>
    <div class="SectionBody">
        <table class="TableContainer">
            <tbody>
                <tr class="Row">
                    <td class="ItemName">
                        <%=GetLocaleResourceString("Account.StreetAddress")%>:
                    </td>
                    <td class="ItemValue">
                        <asp:TextBox ID="txtStreetAddress" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvStreetAddress" runat="server" ControlToValidate="txtStreetAddress"
                            ErrorMessage="<% $NopResources:Account.StreetAddressIsRequired %>" ToolTip="<% $NopResources:Account.StreetAddressIsRequired %>"
                            ValidationGroup="CustomerInfo">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr class="Row">
                    <td class="ItemName">
                        <%=GetLocaleResourceString("Account.StreetAddress2")%>:
                    </td>
                    <td class="ItemValue">
                        <asp:TextBox ID="txtStreetAddress2" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr class="Row">
                    <td class="ItemName">
                        <%=GetLocaleResourceString("Account.PostCode")%>:
                    </td>
                    <td class="ItemValue">
                        <asp:TextBox ID="txtZipPostalCode" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvZipPostalCode" runat="server" ControlToValidate="txtZipPostalCode"
                            ErrorMessage="<% $NopResources:Account.ZipPostalCodeIsRequired %>" ToolTip="<% $NopResources:Account.ZipPostalCodeIsRequired %>"
                            ValidationGroup="CustomerInfo">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr class="Row">
                    <td class="ItemName">
                        <%=GetLocaleResourceString("Account.City")%>:
                    </td>
                    <td class="ItemValue">
                        <asp:TextBox ID="txtCity" runat="server" MaxLength="40"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvCity" runat="server" ControlToValidate="txtCity"
                            ErrorMessage="<% $NopResources:Account.CityIsRequired %>" ToolTip="<% $NopResources:Account.CityIsRequired %>"
                            ValidationGroup="CustomerInfo">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr class="Row">
                    <td class="ItemName">
                        <%=GetLocaleResourceString("Account.Country")%>:
                    </td>
                    <td class="ItemValue">
                        <asp:DropDownList ID="ddlCountry" AutoPostBack="True" runat="server" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged"
                            Width="137px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr class="Row">
                    <td class="ItemName">
                        <%=GetLocaleResourceString("Account.StateProvince")%>:
                    </td>
                    <td class="ItemValue">
                        <asp:DropDownList ID="ddlStateProvince" AutoPostBack="False" runat="server" Width="137px">
                        </asp:DropDownList>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div class="clear">
    </div>
    <div class="SectionTitle">
        <%=GetLocaleResourceString("Account.YourContactInformation")%>
    </div>
    <div class="clear">
    </div>
    <div class="SectionBody">
        <table class="TableContainer">
            <tbody>
                <tr class="Row">
                    <td class="ItemName">
                        <%=GetLocaleResourceString("Account.TelephoneNumber")%>:
                    </td>
                    <td class="ItemValue">
                        <asp:TextBox ID="txtPhoneNumber" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvPhoneNumber" runat="server" ControlToValidate="txtPhoneNumber"
                            ErrorMessage="<% $NopResources:Account.PhoneNumberIsRequired %>" ToolTip="<% $NopResources:Account.PhoneNumberIsRequired %>"
                            ValidationGroup="CustomerInfo">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr class="Row">
                    <td class="ItemName">
                        <%=GetLocaleResourceString("Account.FaxNumber")%>:
                    </td>
                    <td class="ItemValue">
                        <asp:TextBox ID="txtFaxNumber" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div class="clear">
    </div>
    <div class="SectionTitle">
        <%=GetLocaleResourceString("Account.Options")%>
    </div>
    <div class="clear">
    </div>
    <div class="SectionBody">
        <table class="TableContainer">
            <tbody>
                <tr class="Row">
                    <td class="ItemName">
                        <%=GetLocaleResourceString("Account.Newsletter")%>:
                    </td>
                    <td class="ItemValue">
                        <asp:CheckBox ID="cbNewsletter" runat="server"></asp:CheckBox>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div class="clear">
    </div>
    <asp:PlaceHolder runat="server" ID="divPreferences">
        <div class="SectionTitle">
            <%=GetLocaleResourceString("Account.Preferences")%>
        </div>
        <div class="clear">
        </div>
        <div class="SectionBody">
            <table class="TableContainer">
                <tbody>
                    <tr class="Row" runat="server" id="trTimeZone">
                        <td class="ItemName">
                            <%=GetLocaleResourceString("Account.TimeZone")%>:
                        </td>
                        <td class="ItemValue">
                            <asp:DropDownList ID="ddlTimeZone" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="Row" runat="server" id="trSignature">
                        <td class="ItemName">
                            <%=GetLocaleResourceString("Account.Signature")%>:
                        </td>
                        <td class="ItemValue">
                            <asp:TextBox ID="txtSignature" runat="server" TextMode="MultiLine" MaxLength="300"
                                SkinID="AccountSignatureText"></asp:TextBox>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="clear">
        </div>
    </asp:PlaceHolder>
    <div class="Button">
        <asp:Button runat="server" ID="btnSaveCustomerInfo" Text="<% $NopResources:Account.Save %>"
            ValidationGroup="CustomerInfo" OnClick="btnSaveCustomerInfo_Click" SkinID="SaveCustomerInfoButton">
        </asp:Button>
    </div>
</div>
