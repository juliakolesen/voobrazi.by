<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.PrivateMessagesInboxControl"
    CodeBehind="PrivateMessagesInbox.ascx.cs" %>
<div class="PrivateMessagesBox">
    <div class="PrivateMessages">
        <asp:GridView ID="gvInbox" DataKeyNames="PrivateMessageID" runat="server" AllowPaging="True"
            AutoGenerateColumns="False" CellPadding="4" DataSourceID="odsInbox" GridLines="None"
            PageSize="10" SkinID="PrivateMessagesGrid">
            <Columns>
                <asp:TemplateField HeaderText="" ItemStyle-Width="5%">
                    <ItemTemplate>
                        <asp:CheckBox ID="cbSelect" runat="server" />
                        <asp:HiddenField ID="hfPrivateMessageID" runat="server" Value='<%# Eval("PrivateMessageID") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<% $NopResources:PrivateMessages.Inbox.FromColumn %>"
                    ItemStyle-Width="20%">
                    <ItemTemplate>
                        <%#GetFromInfo(Convert.ToInt32(Eval("FromUserID")))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<% $NopResources:PrivateMessages.Inbox.SubjectColumn %>"
                    ItemStyle-Width="50%">
                    <ItemTemplate>
                        <%#GetSubjectInfo(Container.DataItem as PrivateMessage)%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<% $NopResources:PrivateMessages.Inbox.DateColumn %>"
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="25%" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%#DateTimeHelper.ConvertToUserTime((DateTime)Eval("CreatedOn")).ToString()%>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div>
        <asp:ObjectDataSource ID="odsInbox" runat="server" SelectMethod="GetCurrentUserInboxPrivateMessages"
            EnablePaging="true" TypeName="NopSolutions.NopCommerce.Web.ForumHelper" StartRowIndexParameterName="StartIndex"
            MaximumRowsParameterName="PageSize" SelectCountMethod="GetCurrentUserInboxPrivateMessagesCount">
        </asp:ObjectDataSource>
    </div>
    <div class="clear">
    </div>
    <div class="Button">
        <asp:Button runat="server" ID="btnDeleteSelected" Text="<% $NopResources:PrivateMessages.Inbox.DeleteSelected %>"
            ValidationGroup="InboxPrivateMessages" OnClick="btnDeleteSelected_Click" SkinID="DeleteSelectedPMButton">
        </asp:Button>
    </div>
</div>
