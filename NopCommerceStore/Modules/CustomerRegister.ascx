<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.CustomerRegisterControl"
	CodeBehind="CustomerRegister.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="Captcha" Src="~/Modules/Captcha.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="Topic" Src="~/Modules/Topic.ascx" %>
<div class="RegistrationPage">
	<h2>
		<%=GetLocaleResourceString("Account.Registration")%>
	</h2>
	<div class="body">
		<asp:CreateUserWizard ID="CreateUserForm" EmailRegularExpression="[\w\.-]+(\+[\w-]*)?@([\w-]+\.)+[\w-]+"
			RequireEmail="False" runat="server" OnCreatedUser="CreatedUser" OnCreatingUser="CreatingUser"
			OnCreateUserError="CreateUserError" FinishDestinationPageUrl="~/Default.aspx"
			ContinueDestinationPageUrl="~/Default.aspx" Width="100%" LoginCreatedUser="true">
			<WizardSteps>
				<asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server" Title="">
					<ContentTemplate>
						<div class="messageError">
							<asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
						</div>
						<div class="clear">
						</div>
						<h3>
							<%=GetLocaleResourceString("Account.YourPersonalDetails")%>
						</h3>
						<div class="clear">
						</div>
						<div class="SectionBody">
							<table class="TableContainer">
								<tbody>
									<tr runat="server" visible="false" class="Row">
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
												ErrorMessage="First name is required." ToolTip="First name is required." ValidationGroup="CreateUserForm">*</asp:RequiredFieldValidator>
										</td>
									</tr>
									<tr class="Row">
										<td class="ItemName">
											<%=GetLocaleResourceString("Account.LastName")%>:
										</td>
										<td class="ItemValue">
											<asp:TextBox ID="txtLastName" runat="server" MaxLength="40"></asp:TextBox>
											<asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtLastName"
												ErrorMessage="Last name is required." ToolTip="Last name is required." ValidationGroup="CreateUserForm">*</asp:RequiredFieldValidator>
										</td>
									</tr>
									<tr runat="server" visible="false" class="Row">
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
									<%--pnlEmail is visible only when customers are authenticated by usernames and is used to get an email--%>
									<tr class="Row" runat="server" id="pnlEmail">
										<td class="ItemName">
											<%=GetLocaleResourceString("Account.E-Mail")%>:
										</td>
										<td class="ItemValue">
											<asp:TextBox ID="Email" runat="server" MaxLength="40"></asp:TextBox>
											<asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="Email"
												ErrorMessage="Email is required" ToolTip="Email is required" Display="Dynamic"
												ValidationGroup="CreateUserForm">*</asp:RequiredFieldValidator>
											<asp:RegularExpressionValidator runat="server" ID="revEmail" Display="Dynamic" ControlToValidate="Email"
												ErrorMessage="Invalid email" ToolTip="Invalid email" ValidationExpression="[\w\.-]+(\+[\w-]*)?@([\w-]+\.)+[\w-]+"
												ValidationGroup="CreateUserForm">*</asp:RegularExpressionValidator>
										</td>
									</tr>
									<%--this table row is used to get an username when customers are authenticated by usernames--%>
									<%--this table row is used to get an email when customers are authenticated by emails--%>
									<tr class="Row">
										<td class="ItemName">
											<asp:Literal runat="server" ID="lUsernameOrEmail" Text="E-Mail" />:
										</td>
										<td class="ItemValue">
											<asp:TextBox ID="UserName" runat="server" MaxLength="40"></asp:TextBox>
											<asp:RequiredFieldValidator ID="UserNameOrEmailRequired" runat="server" ControlToValidate="UserName"
												ErrorMessage="Username is required" ToolTip="Username is required" Display="Dynamic"
												ValidationGroup="CreateUserForm">*</asp:RequiredFieldValidator>
											<asp:RegularExpressionValidator runat="server" ID="refUserNameOrEmail" Display="Dynamic"
												ControlToValidate="UserName" ErrorMessage="Invalid email" ToolTip="Invalid email"
												ValidationExpression="[\w\.-]+(\+[\w-]*)?@([\w-]+\.)+[\w-]+" ValidationGroup="CreateUserForm">*</asp:RegularExpressionValidator>
										</td>
									</tr>
								</tbody>
							</table>
						</div>
						<div class="clear">
						</div>
						<div runat="server" visible="false" class="SectionTitle">
							<%=GetLocaleResourceString("Account.CompanyDetails")%>
						</div>
						<div class="clear">
						</div>
						<div runat="server" visible="false" class="SectionBody">
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
						<div class="SectionTitle">
							<h3>
								<%=GetLocaleResourceString("Account.YourAddress")%>
							</h3>
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
												ErrorMessage="Street address is required." ToolTip="Street address is required."
												ValidationGroup="CreateUserForm">*</asp:RequiredFieldValidator>
										</td>
									</tr>
									<tr runat="server" visible="false" class="Row">
										<td class="ItemName">
											<%=GetLocaleResourceString("Account.StreetAddress2")%>:
										</td>
										<td class="ItemValue">
											<asp:TextBox ID="txtStreetAddress2" runat="server"></asp:TextBox>
										</td>
									</tr>
									<tr runat="server" visible="false" class="Row">
										<td class="ItemName">
											<%=GetLocaleResourceString("Account.PostCode")%>:
										</td>
										<td class="ItemValue">
											<asp:TextBox ID="txtZipPostalCode" runat="server"></asp:TextBox>
											<asp:RequiredFieldValidator Enabled="false" ID="rfvZipPostalCode" runat="server"
												ControlToValidate="txtZipPostalCode" ErrorMessage="Zip / Postal code is required."
												ToolTip="Zip / Postal code is required." ValidationGroup="CreateUserForm">*</asp:RequiredFieldValidator>
										</td>
									</tr>
									<tr class="Row">
										<td class="ItemName">
											<%=GetLocaleResourceString("Account.City")%>:
										</td>
										<td class="ItemValue">
											<asp:TextBox ID="txtCity" runat="server" MaxLength="40"></asp:TextBox>
											<asp:RequiredFieldValidator ID="rfvCity" runat="server" ControlToValidate="txtCity"
												ErrorMessage="City is required." ToolTip="City is required." ValidationGroup="CreateUserForm">*</asp:RequiredFieldValidator>
										</td>
									</tr>
									<tr runat="server" visible="false" class="Row">
										<td class="ItemName">
											<%=GetLocaleResourceString("Account.Country")%>:
										</td>
										<td class="ItemValue">
											<asp:DropDownList ID="ddlCountry" AutoPostBack="True" runat="server" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged"
												Width="137px">
											</asp:DropDownList>
										</td>
									</tr>
									<tr runat="server" visible="false" class="Row">
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
						<div class="SectionTitle">
							<h3>
								<%=GetLocaleResourceString("Account.YourContactInformation")%>
							</h3>
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
												ErrorMessage="Telephone Number is required." ToolTip="Telephone Number is required."
												ValidationGroup="CreateUserForm">*</asp:RequiredFieldValidator>
										</td>
									</tr>
									<tr class="Row">
										<td class="ItemName">
											Телефон 2:
										</td>
										<td class="ItemValue">
											<asp:TextBox ID="txtFaxNumber" runat="server"></asp:TextBox>
										</td>
									</tr>
								</tbody>
							</table>
						</div>
						<div  runat="server" visible="false" class="SectionTitle">
							<%=GetLocaleResourceString("Account.Options")%>
						</div>
						<div class="clear">
						</div>
						<div runat="server" visible="false" class="SectionBody">
							<table class="TableContainer">
								<tbody>
									<tr class="Row">
										<td class="ItemName">
											<%=GetLocaleResourceString("Account.Newsletter")%>:
										</td>
										<td class="ItemValue">
											<asp:CheckBox ID="cbNewsletter" runat="server" Checked="true"></asp:CheckBox>
										</td>
									</tr>
								</tbody>
							</table>
						</div>
						<div class="SectionTitle">
							<h3>
								<%=GetLocaleResourceString("Account.YourPassword")%>
							</h3>
						</div>
						<div class="clear">
						</div>
						<div class="SectionBody">
							<table class="TableContainer">
								<tbody>
									<tr class="Row">
										<td class="ItemName">
											<%=GetLocaleResourceString("Account.Password")%>:
										</td>
										<td class="ItemValue">
											<asp:TextBox ID="Password" runat="server" MaxLength="50" TextMode="Password"></asp:TextBox>
											<asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
												ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="CreateUserForm">*</asp:RequiredFieldValidator>
										</td>
									</tr>
									<tr class="Row">
										<td class="ItemName">
											<%=GetLocaleResourceString("Account.PasswordConfirmation")%>:
										</td>
										<td class="ItemValue">
											<asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
											<asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" ControlToValidate="ConfirmPassword"
												ErrorMessage="*" ToolTip='Confirm password is required.' ValidationGroup="CreateUserForm"></asp:RequiredFieldValidator>
										</td>
									</tr>
									<tr class="Row">
										<td colspan="2">
											<asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="Password"
												ControlToValidate="ConfirmPassword" Display="Dynamic" ErrorMessage="<% $NopResources:Account.EnteredPasswordsDoNotMatch %>"
												ToolTip="<% $NopResources:Account.EnteredPasswordsDoNotMatch %>" ValidationGroup="CreateUserForm"></asp:CompareValidator>
										</td>
									</tr>
									<tr class="Row">
										<td colspan="2">
											<nopCommerce:Captcha ID="CaptchaCtrl" runat="server" />
										</td>
									</tr>
								</tbody>
							</table>
						</div>
						<div class="clear">
						</div>
					</ContentTemplate>
					<CustomNavigationTemplate>
						<div class="Button">
							<asp:Button ID="StepNextButton" runat="server" CommandName="MoveNext" Text="<% $NopResources:Account.RegisterNextStepButton %>"
								ValidationGroup="CreateUserForm" SkinID="RegisterNextStepButton" />
						</div>
					</CustomNavigationTemplate>
				</asp:CreateUserWizardStep>
				<asp:CompleteWizardStep ID="CompleteWizardStep1" runat="server">
					<ContentTemplate>
						<div class="SectionBody">
							<p>
								<asp:Label runat="server" ID="lblCompleteStep"></asp:Label>
							</p>
							<asp:Button ID="ContinueButton" runat="server" CausesValidation="False" CommandName="Continue"
								Text="<% $NopResources:Account.RegisterContinueButton %>" ValidationGroup="CreateUserForm"
								SkinID="CompleteRegistrationButton" />
						</div>
						<div class="clear">
						</div>
					</ContentTemplate>
				</asp:CompleteWizardStep>
			</WizardSteps>
		</asp:CreateUserWizard>
		<nopCommerce:Topic ID="topicRegistrationNotAllowed" runat="server" TopicName="RegistrationNotAllowed"
			Visible="false"></nopCommerce:Topic>
	</div>
</div>
