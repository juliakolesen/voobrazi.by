<%@ Page Language="C#" MasterPageFile="~/MasterPages/OneColumn.master" AutoEventWireup="true"
    Inherits="NopSolutions.NopCommerce.Web.AccountPage" CodeBehind="Account.aspx.cs" %>

<%@ Register TagPrefix="nopCommerce" TagName="CustomerInfo" Src="~/Modules/CustomerInfo.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="CustomerAddresses" Src="~/Modules/CustomerAddresses.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="CustomerOrders" Src="~/Modules/CustomerOrders.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="CustomerChangePassword" Src="~/Modules/CustomerChangePassword.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="CustomerAvatar" Src="~/Modules/CustomerAvatar.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="Server">
    <div class="AccountPage">
        <div class="title">
            <%=GetLocaleResourceString("Account.MyAccount")%>
        </div>
        <div class="clear">
        </div>
        <div class="body">
            <ajaxToolkit:TabContainer runat="server" ID="CustomerTabs" ActiveTabIndex="0" SkinID="TabContainer-MyAccount">
                <ajaxToolkit:TabPanel runat="server" ID="pnlCustomerInfo" HeaderText="<% $NopResources:Account.CustomerInfo %>">
                    <ContentTemplate>
                        <nopCommerce:CustomerInfo ID="ctrlCustomerInfo" runat="server" />
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel runat="server" ID="pnlCustomerAddresses" HeaderText="<% $NopResources:Account.CustomerAddresses %>">
                    <ContentTemplate>
                        <nopCommerce:CustomerAddresses ID="ctrlCustomerAddresses" runat="server" />
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel runat="server" ID="pnlCustomerOrders" HeaderText="<% $NopResources:Account.CustomerOrders %>">
                    <ContentTemplate>
                        <nopCommerce:CustomerOrders ID="ctrlCustomerOrders" runat="server" />
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel runat="server" ID="pnlChangePassword" HeaderText="<% $NopResources:Account.ChangePassword %>">
                    <ContentTemplate>
                        <nopCommerce:CustomerChangePassword ID="ctrlCustomerChangePassword" runat="server" />
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel runat="server" ID="pnlAvatar" HeaderText="<% $NopResources:Account.Avatar %>">
                    <ContentTemplate>
                        <nopCommerce:CustomerAvatar ID="ctrlCustomerAvatar" runat="server" />
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
            </ajaxToolkit:TabContainer>
        </div>
    </div>
</asp:Content>
