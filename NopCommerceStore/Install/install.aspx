<%@ Page Language="C#" Inherits="NopSolutions.NopCommerce.Web.Install.InstallPage"
    Theme="Install" CodeBehind="install.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>nopCommerce installation</title>
</head>
<body class="container_main">
    <form id="Form1" method="post" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" class="container_installer">
        <tr class="Top">
            <td class="Left">
                &nbsp;
            </td>
            <td class="Center">
                &nbsp;
            </td>
            <td class="Right">
                &nbsp;
            </td>
        </tr>
        <tr class="Middle">
            <td class="Left">
                &nbsp;
            </td>
            <td class="Center">
                <asp:Label CssClass="headerText" Text="nopCommerce installation" runat="server"></asp:Label><br />
                <asp:Image ID="imgHeader" runat="server" CssClass="headerImg" />
                <asp:Panel runat="server" ID="pnlWizard" CssClass="content">
                    <asp:Wizard ID="wzdInstaller" runat="server" DisplaySideBar="False" OnActiveStepChanged="OnActiveStepChanged"
                        OnNextButtonClick="OnNextButtonClick" ActiveStepIndex="1">
                        <StepNavigationTemplate>
                            <div style="float: right; padding: 0px 10px 0px 0px;">
                                <asp:Button ID="StepPrevButton" Source="file" runat="server" CommandName="MovePrevious"
                                    Text="Back" Width="100" />
                                <asp:Button ID="StepNextButton" runat="server" CommandName="MoveNext" Width="100"
                                    Text="Next" />
                            </div>
                        </StepNavigationTemplate>
                        <StartNavigationTemplate>
                            <div style="float: right; padding: 0px 10px 0px 0px;">
                                <asp:Button ID="StepNextButton" runat="server" CommandName="MoveNext" Width="100"
                                    Text="Next" />
                            </div>
                        </StartNavigationTemplate>
                        <WizardSteps>
                            <asp:WizardStep ID="stpWelcome" runat="server">
                                <table class="wizardStep" border="0" cellpadding="0" cellspacing="10">
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblWelcome" runat="server" Text="Thank you for choosing nopCommerce!"
                                                CssClass="title"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <p>
                                                <a href="http://www.nopCommerce.com/" target="_blank">nopCommerce</a> is the leading
                                                ASP.NET online shop e-commerce solution. This wizard will guide you through the
                                                process of configuring <strong>nopCommerce
                                                    <%=GetNewVersion()%></strong>
                                                <br />
                                                <br />
                                                To complete this wizard you must know some information regarding your database server
                                                ("connection string"). Please contact your ISP if necessary. If you're installing
                                                on a local machine or server you might need information from your System Admin.<br />
                                                <br />
                                                P.S. Before you start any upgrade don't forget to backup.
                                            </p>
                                            <p>
                                                <asp:RadioButton ID="radInstall" runat="server" Text="Install nopCommerce" AutoPostBack="True"
                                                    GroupName="InstallUpgrade" Checked="True"></asp:RadioButton>
                                                <br />
                                                <asp:RadioButton ID="radUpgrade" runat="server" Text="Upgrade from previous version"
                                                    GroupName="InstallUpgrade"></asp:RadioButton>
                                            </p>
                                            <p>
                                                Press <b>"Next"</b> to start the wizard.
                                            </p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </asp:WizardStep>
                            <asp:WizardStep ID="stpUserServer" runat="server">
                                <table class="wizardStep" border="0" cellpadding="0" cellspacing="10">
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblSQLServer" runat="server" Text="SQL server" CssClass="title"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td nowrap="nowrap" align="right">
                                            <asp:Label ID="lblServerName" runat="server" Text="SQL Server name or IP address:"></asp:Label>
                                        </td>
                                        <td width="100%">
                                            <asp:TextBox ID="txtServerName" CssClass="textBox" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="left">
                                            <asp:RadioButton ID="radSQLAuthentication" runat="server" Text="Use SQL Server account"
                                                AutoPostBack="True" GroupName="AuthenticationType" Checked="True"></asp:RadioButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td nowrap="nowrap" align="right">
                                            <asp:Label ID="lblUsername" runat="server" Text="Username:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtUsername" CssClass="textBox" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td nowrap="nowrap" align="right">
                                            <asp:Label ID="lblPassword" runat="server" Text="Password:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPassword" CssClass="textBox" runat="server" TextMode="Password"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="left">
                                            <asp:RadioButton ID="radWindowsAuthentication" runat="server" AutoPostBack="True"
                                                GroupName="AuthenticationType"></asp:RadioButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </asp:WizardStep>
                            <asp:WizardStep ID="stpDatabase" runat="server">
                                <table class="wizardStep" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label ID="lblDatabase" runat="server" Text="Database" CssClass="title"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:RadioButton ID="radCreateNew" runat="server" Text="Create a new database" AutoPostBack="True"
                                                GroupName="DatabaseType" Checked="True"></asp:RadioButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 25px;">
                                            &nbsp;
                                        </td>
                                        <td nowrap="nowrap" align="left" style="width: 140px;">
                                            <asp:Label ID="lblNewDatabaseName" runat="server" Text="New database name:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNewDatabaseName" CssClass="textBox" runat="server" Enabled="False"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:RadioButton ID="radUseExisting" runat="server" Text="Use an existing empty database"
                                                AutoPostBack="True" GroupName="DatabaseType"></asp:RadioButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 25px;">
                                            &nbsp;
                                        </td>
                                        <td nowrap="nowrap" align="left" style="width: 140px;">
                                            <asp:Label ID="lblExistingDatabaseName" runat="server" Text="Existing database name:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtExistingDatabaseName" CssClass="textBox" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 25px;">
                                            &nbsp;
                                        </td>
                                        <td colspan="2">
                                            <asp:CheckBox ID="chkCreateSampleData" runat="server" Checked="True" Text="Create sample data">
                                            </asp:CheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Panel ID="pnlLog" runat="server" Visible="False" Width="100%">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblLog" runat="server" Text="Setup log:" CssClass="title"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Panel runat="server" ID="pnlGroupLog">
                                                                <asp:TextBox ID="txtLog" runat="server" CssClass="log" TextMode="MultiLine" ReadOnly="True" />
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </asp:WizardStep>
                            <asp:WizardStep ID="stpConnectionString" runat="server" AllowReturn="false" StepType="Start">
                                <asp:Panel ID="pnlConnectionString" runat="server">
                                    <asp:Label ID="lblConnectionString" runat="server" Text="Connection string" CssClass="title"
                                        Visible="False"></asp:Label>
                                    <table class="wizardStep" border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="lblErrorConnMessage" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </asp:WizardStep>
                            <asp:WizardStep ID="stpFinish" runat="server" StepType="Complete">
                                <asp:Panel ID="pnlFinished" runat="server">
                                    <table class="wizardStep" border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblCompleted" runat="server" Text="Congratulations - nopCommerce is now properly configured."></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <p>
                                                    Once you're done with the steps above, you can start using nopCommerce by clicking
                                                    the button below.
                                                </p>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <p>
                                                    <p>
                                                        If you need information on how to use nopCommerce, visit <a href="http://www.nopCommerce.com/Documentation.aspx">
                                                            the documentation section on nopCommerce.com</a>.<br />
                                                        <br />
                                                    </p>
                                                </p>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:LinkButton ID="btnWebSite" runat="server" Text="Go to site" OnClick="btnGoToSite_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </asp:WizardStep>
                        </WizardSteps>
                    </asp:Wizard>
                </asp:Panel>
                <asp:Panel ID="pnlPermission" runat="server" CssClass="content" Visible="false">
                    <div style="text-align: left; padding: 0px 0px 10px 20px;">
                        <asp:Label ID="lblPermission" runat="server" />
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlButtons" runat="server" Visible="false">
                    <asp:Button ID="btnPermissionTest" runat="server" Width="150" Text="Test permissions again"
                        OnClick="btnPermissionTest_Click" />&nbsp;<asp:Button ID="btnPermissionSkip" runat="server"
                            Text="Skip" OnClick="btnPermissionSkip_Click" />
                </asp:Panel>
                <asp:Panel ID="pnlPermissionSuccess" runat="server" Visible="false">
                    <asp:Label ID="lblPermissionSuccess" runat="server" /><br />
                    <br />
                    <asp:Button ID="btnPermissionContinue" runat="server" Text="Continue" OnClick="btnPermissionContinue_Click" />
                </asp:Panel>
            </td>
            <td class="Right">
            </td>
        </tr>
        <tr class="Bottom">
            <td class="Left">
                &nbsp;
            </td>
            <td class="Center">
                &nbsp;
            </td>
            <td class="Right">
                &nbsp;
            </td>
        </tr>
    </table>
    <div class="container_footer">
        <div style="text-align: right; padding: 0px 0px 0px 0px; color: #606060;">
            <asp:Label ID="lblVersion" runat="server" />
        </div>
    </div>
    <asp:Panel ID="pnlError" runat="server" CssClass="container_error">
        <div style="text-align: left; padding: 0px 0px 10px 20px;">
            <asp:Label ID="lblError" runat="server" CssClass="errorLabel"></asp:Label>
        </div>
    </asp:Panel>
    </form>
</body>
</html>
