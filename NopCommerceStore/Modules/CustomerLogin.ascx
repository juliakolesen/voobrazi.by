<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.CustomerLoginControl"
	CodeBehind="CustomerLogin.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="Captcha" Src="~/Modules/Captcha.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="Topic" Src="~/Modules/Topic.ascx" %>
<div class="LoginPage">
	<h2>
		<%=GetLocaleResourceString("Login.Welcome")%>
	</h2>
	<div class="wrapper">
		<%if (!CheckoutAsGuestQuestion)
	{ %>
		<h3>
			<%=GetLocaleResourceString("Login.NewCustomer")%></h3>
		<table>
			<tbody>
				<tr>
					<td>
						<%=GetLocaleResourceString("Login.NewCustomerText")%>
					</td>
				</tr>
				<tr>
					<td align="right" style="padding-right: 20px; padding-top: 20px;">
						<asp:Button runat="server" ID="btnRegister" Text="<% $NopResources:Account.Register %>"
							OnClick="btnRegister_Click" SkinID="RegisterButton" />
					</td>
				</tr>
			</tbody>
		</table>
		<%}
	else
	{ %>
		<div class="new-wrapper">
			<span class="RegisterTitle">
				<%=GetLocaleResourceString("Checkout.CheckoutAsGuestOrRegister")%></span>
			<div class="CheckoutAsGuestOrRegisterBlock">
				<table>
					<tbody>
						<tr>
							<td>
								<nopCommerce:Topic ID="topicCheckoutAsGuestOrRegister" runat="server" TopicName="CheckoutAsGuestOrRegister">
								</nopCommerce:Topic>
							</td>
						</tr>
						<tr>
							<td align="right" style="padding-right: 20px; padding-top: 20px;">
								<asp:Button runat="server" ID="btnCheckoutAsGuest" Text="<% $NopResources:Checkout.CheckoutAsGuest %>"
									OnClick="btnCheckoutAsGuest_Click" SkinID="CheckoutAsGuestButton" />
								<asp:Button runat="server" ID="btnRegister2" Text="<% $NopResources:Account.Register %>"
									OnClick="btnRegister_Click" SkinID="RegisterButton" />
							</td>
						</tr>
					</tbody>
				</table>
			</div>
		</div>
		<%} %>
		<div class="returning-wrapper">
			<h3>
				<%=GetLocaleResourceString("Login.ReturningCustomer")%>
			</h3>
			<asp:Panel ID="pnlLogin" runat="server" DefaultButton="LoginForm$LoginButton" CssClass="LoginBlock">
				<asp:Login ID="LoginForm" TitleText="" OnLoggedIn="OnLoggedIn" OnLoggingIn="OnLoggingIn"
					runat="server" CreateUserUrl="~/Register.aspx" DestinationPageUrl="~/Default.aspx"
					OnLoginError="OnLoginError" RememberMeSet="True" FailureText="<% $NopResources:Login.FailureText %>">
					<LayoutTemplate>
						<table class="LoginTableContainer">
							<tbody>
								<tr class="Row">
									<td class="ItemName">
										<asp:Literal runat="server" ID="lUsernameOrEmail" Text="E-Mail" />:
									</td>
									<td class="ItemValue">
										<asp:TextBox ID="UserName" runat="server"></asp:TextBox>
										<asp:RequiredFieldValidator ID="UserNameOrEmailRequired" runat="server" ControlToValidate="UserName"
											ErrorMessage="Username is required." ToolTip="Username is required." ValidationGroup="ctl00$LoginForm">*</asp:RequiredFieldValidator>
									</td>
								</tr>
								<tr class="Row">
									<td class="ItemName">
										<asp:Literal runat="server" ID="lPassword" Text="<% $NopResources:Login.Password %>" />:
									</td>
									<td class="ItemValue">
										<asp:TextBox ID="Password" TextMode="Password" runat="server" MaxLength="50"></asp:TextBox>
										<asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
											ErrorMessage="<% $NopResources:Login.PasswordRequired %>" ToolTip="<% $NopResources:Login.PasswordRequired %>"
											ValidationGroup="ctl00$LoginForm">*</asp:RequiredFieldValidator>
									</td>
								</tr>
								<tr class="Row">
									<td class="ItemValue" colspan="2">
										<br />
										<asp:CheckBox ID="RememberMe" runat="server" Text="<% $NopResources:Login.RememberMe %>">
										</asp:CheckBox>
									</td>
								</tr>
								<tr class="Row">
									<td class="messageError" colspan="2">
										<asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
									</td>
								</tr>
								<tr class="Row">
									<td class="ForgotPassword" colspan="2">
										<asp:HyperLink ID="hlForgotPassword" runat="server" NavigateUrl="~/PasswordRecovery.aspx"
											Text="<% $NopResources:Login.ForgotPassword %>"></asp:HyperLink>
									</td>
								</tr>
								<tr class="Row">
									<td colspan="2">
										<nopCommerce:Captcha ID="CaptchaCtrl" runat="server" />
									</td>
								</tr>
								<tr class="Row">
									<td colspan="2">
										<br />
										<asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="<% $NopResources:Login.LoginButton %>"
											ValidationGroup="ctl00$LoginForm" SkinID="LoginButton" />
									</td>
								</tr>
							</tbody>
						</table>
					</LayoutTemplate>
				</asp:Login>
			</asp:Panel>
		</div>
	</div>
	<div class="clear">
	</div>
	<nopCommerce:Topic ID="topicHomePageText" runat="server" TopicName="LoginRegistrationInfo">
	</nopCommerce:Topic>
</div>
