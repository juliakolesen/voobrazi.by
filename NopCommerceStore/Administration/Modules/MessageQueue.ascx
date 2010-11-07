<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.MessageQueueControl"
    CodeBehind="MessageQueue.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="ToolTipLabel" Src="ToolTipLabelControl.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="NumericTextBox" Src="NumericTextBox.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="SimpleTextBox" Src="SimpleTextBox.ascx" %>
<div class="section-header">
    <div class="title">
        <img src="Common/ico-system.png" alt="<%=GetLocaleResourceString("Admin.MessageQueue.Title")%>" />
        <%=GetLocaleResourceString("Admin.MessageQueue.Title")%>
    </div>
    <div class="options">
        <asp:Button ID="LoadButton" runat="server" Text="<% $NopResources:Admin.MessageQueue.LoadButton.Text %>"
            CssClass="adminButtonBlue" OnClick="LoadButton_Click" ToolTip="<% $NopResources:Admin.MessageQueue.LoadButton.Tooltip %>" />
    </div>
</div>
<table width="100%" class="adminContent">
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblStartDate" Text="<% $NopResources:Admin.MessageQueue.StartDate %>"
                ToolTip="<% $NopResources:Admin.MessageQueue.StartDate.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:TextBox runat="server" ID="txtStartDate" />
            <asp:ImageButton runat="Server" ID="iStartDate" ImageUrl="~/images/Calendar_scheduleHS.png"
                AlternateText="<% $NopResources:Admin.MessageQueue.StartDate.ShowCalendar %>" /><br />
            <ajaxToolkit:CalendarExtender ID="cStartDateButtonExtender" runat="server" TargetControlID="txtStartDate"
                PopupButtonID="iStartDate" />
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblEndDate" Text="<% $NopResources:Admin.MessageQueue.EndDate %>"
                ToolTip="<% $NopResources:Admin.MessageQueue.EndDate.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:TextBox runat="server" ID="txtEndDate" />
            <asp:ImageButton runat="Server" ID="iEndDate" ImageUrl="~/images/Calendar_scheduleHS.png"
                AlternateText="<% $NopResources:Admin.MessageQueue.EndDate.ShowCalendar %>" /><br />
            <ajaxToolkit:CalendarExtender ID="cEndDateButtonExtender" runat="server" TargetControlID="txtEndDate"
                PopupButtonID="iEndDate" />
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblFromEmail" Text="<% $NopResources:Admin.MessageQueue.From %>"
                ToolTip="<% $NopResources:Admin.MessageQueue.From.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtFromEmail" CssClass="adminInput" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblToEmail" Text="<% $NopResources:Admin.MessageQueue.ToEmail %>"
                ToolTip="<% $NopResources:Admin.MessageQueue.ToEmail.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtToEmail" CssClass="adminInput" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblLoadNotSentItemsOnly" Text="<% $NopResources:Admin.MessageQueue.LoadNotSent %>"
                ToolTip="<% $NopResources:Admin.MessageQueue.LoadNotSent.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <asp:CheckBox ID="cbLoadNotSentItemsOnly" runat="server" Checked="true"></asp:CheckBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblMaxSendTries" Text="<% $NopResources:Admin.MessageQueue.MaxSendTries %>"
                ToolTip="<% $NopResources:Admin.MessageQueue.MaxSendTries.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <nopCommerce:NumericTextBox runat="server" CssClass="adminInput" ID="txtMaxSendTries"
                RequiredErrorMessage="<% $NopResources:Admin.MessageQueue.MaxSendTries.RequiredErrorMessage %>"
                MinimumValue="0" MaximumValue="999999" Value="10" RangeErrorMessage="<% $NopResources:Admin.MessageQueue.MaxSendTries.RangeErrorMessage %>">
            </nopCommerce:NumericTextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <nopCommerce:ToolTipLabel runat="server" ID="lblGoDirectlyToEmailNumber" Text="<% $NopResources:Admin.MessageQueue.GoDirectly %>"
                ToolTip="<% $NopResources:Admin.MessageQueue.GoDirectly.Tooltip %>" ToolTipImage="~/Administration/Common/ico-help.gif" />
        </td>
        <td class="adminData">
            <nopCommerce:SimpleTextBox runat="server" CssClass="adminInput" ID="txtEmailID" Width="100px"
                ValidationGroup="GoDirectly" ErrorMessage="<% $NopResources:Admin.MessageQueue.EmailID.Required %>">
            </nopCommerce:SimpleTextBox>
            <asp:Button runat="server" Text="<% $NopResources:Admin.MessageQueue.GoButton.Text %>"
                CssClass="adminButtonBlue" ID="btnGoDirectlyToEmailNumber" OnClick="btnGoDirectlyToEmailNumber_Click"
                ValidationGroup="GoDirectly" ToolTip="<% $NopResources:Admin.MessageQueue.GoButton.Tooltip %>" />
        </td>
    </tr>
</table>
<p>
</p>
<asp:GridView ID="gvQueuedEmails" runat="server" AutoGenerateColumns="False" Width="100%"
    OnPageIndexChanging="gvQueuedEmails_PageIndexChanging" AllowPaging="true" PageSize="15">
    <Columns>
        <asp:BoundField DataField="QueuedEmailID" HeaderText="<% $NopResources:Admin.MessageQueue.QueuedEmailIDColumn %>"
            ItemStyle-Width="12%"></asp:BoundField>
        <asp:BoundField DataField="Priority" HeaderText="<% $NopResources:Admin.MessageQueue.PriorityColumn %>"
            ItemStyle-Width="5%"></asp:BoundField>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.MessageQueue.FromColumn %>"
            ItemStyle-Width="25%">
            <ItemTemplate>
                <%#GetFromInfo(Container.DataItem as QueuedEmail)%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.MessageQueue.ToColumn %>" ItemStyle-Width="25%">
            <ItemTemplate>
                <%#GetToInfo(Container.DataItem as QueuedEmail)%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.MessageQueue.CreatedOnColumn %>"
            HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <%#DateTimeHelper.ConvertToUserTime((DateTime)Eval("CreatedOn")).ToString()%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.MessageQueue.SentOnColumn %>"
            ItemStyle-Width="15%">
            <ItemTemplate>
                <%#GetSentOnInfo(Container.DataItem as QueuedEmail)%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<% $NopResources:Admin.MessageQueue.ViewColumn %>"
            HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <a href="MessageQueueDetails.aspx?QueuedEmailID=<%#Eval("QueuedEmailID")%>">
                    <%#GetLocaleResourceString("Admin.MessageQueue.ViewColumn")%></a>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<br />
<asp:Label runat="server" ID="lblQueuedEmailsFound" Text="<% $NopResources:Admin.MessageQueue.NoEmailsFound %>"
    Visible="false"></asp:Label>