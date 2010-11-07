<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.TodaysPollControl" Codebehind="TodaysPoll.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="Poll" Src="~/Modules/Poll.ascx" %>
<div class="todaysPollBox">
    <div class="title">
        <%=GetLocaleResourceString("Polls.TodaysPoll")%>
    </div>
    <div class="clear">
    </div>
    <div class="pollItem">
        <nopCommerce:Poll ID="PollControl" runat="server" />
    </div>
</div>
