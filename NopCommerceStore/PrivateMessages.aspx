<%@ Page Language="C#" MasterPageFile="~/MasterPages/OneColumn.master" AutoEventWireup="true"
    Inherits="NopSolutions.NopCommerce.Web.PrivateMessagesPage" CodeBehind="PrivateMessages.aspx.cs" %>

<%@ Register TagPrefix="nopCommerce" TagName="PrivateMessagesInbox" Src="~/Modules/PrivateMessagesInbox.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="PrivateMessagesSentItems" Src="~/Modules/PrivateMessagesSentItems.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="Server">
    <div class="PrivateMessagesPage">
        <div class="title">
            <%=GetLocaleResourceString("PrivateMessages.PrivateMessages")%>
        </div>
        <div class="clear">
        </div>
        <div class="body">
            <ajaxToolkit:TabContainer runat="server" ID="PrivateMessagesTabs" ActiveTabIndex="0"
                SkinID="TabContainer-PrivateMessages">
                <ajaxToolkit:TabPanel runat="server" ID="pnlPrivateMessagesInbox" HeaderText="<% $NopResources:PrivateMessages.Inbox %>">
                    <ContentTemplate>
                        <nopCommerce:PrivateMessagesInbox ID="ctrlPrivateMessagesInbox" runat="server" />
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel runat="server" ID="pnlPrivateMessagesSentItems" HeaderText="<% $NopResources:PrivateMessages.SentItems %>">
                    <ContentTemplate>
                        <nopCommerce:PrivateMessagesSentItems ID="ctrlPrivateMessagesSentItems" runat="server" />
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
            </ajaxToolkit:TabContainer>
        </div>
    </div>
</asp:Content>
