<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.ForumPostControl"
    CodeBehind="ForumPost.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="PrivateMessageSendButton" Src="~/Modules/PrivateMessageSendButton.ascx" %>
<asp:Literal runat="server" ID="lAnchor"></asp:Literal>
<div class="forumpost">
    <div class="postinfo">
        <div class="manage">
            <asp:LinkButton runat="server" ID="btnEdit" Text="<% $NopResources:Forum.EditPost %>"
                OnClick="btnEdit_Click" SkinID="EditPostLinkButton" />
            <asp:LinkButton runat="server" ID="btnDelete" Text="<% $NopResources:Forum.DeletePost %>"
                OnClick="btnDelete_Click" SkinID="DeletePostLinkButton" />
        </div>
        <div class="userinfo">
            <asp:HyperLink ID="hlUser" runat="server" CssClass="username" />
            <asp:Label ID="lblUser" runat="server" CssClass="username" />
            <div class="avatar">
                <asp:Image runat="server" ID="imgAvatar" AlternateText="Avatar" CssClass="avatarImg" />
            </div>
            <div class="userstats">
                <asp:PlaceHolder runat="server" ID="phStatus">
                    <div class="status">
                        <%=GetLocaleResourceString("Forum.Status")%>:
                        <asp:Label runat="server" ID="lblStatus" CssClass="statvalue" />
                    </div>
                </asp:PlaceHolder>
                <asp:PlaceHolder runat="server" ID="phTotalPosts">
                    <div class="totalposts">
                        <%=GetLocaleResourceString("Forum.TotalPosts")%>:
                        <asp:Label runat="server" ID="lblTotalPosts" CssClass="statvalue" />
                    </div>
                </asp:PlaceHolder>
                <asp:PlaceHolder runat="server" ID="phJoined">
                    <div class="joined">
                        <%=GetLocaleResourceString("Forum.Joined")%>:
                        <asp:Label runat="server" ID="lblJoined" CssClass="statvalue" />
                    </div>
                </asp:PlaceHolder>
                <asp:PlaceHolder runat="server" ID="phLocation">
                    <div class="location">
                        <%=GetLocaleResourceString("Forum.Location")%>:
                        <asp:Label runat="server" ID="lblLocation" CssClass="statvalue" /></div>
                </asp:PlaceHolder>
                <asp:PlaceHolder runat="server" ID="phPM">
                    <div>
                        <nopCommerce:PrivateMessageSendButton runat="server" ID="btnSendPM" />
                    </div>
                </asp:PlaceHolder>
            </div>
        </div>
    </div>
    <div class="postcontent">
        <div class="posttime">
            <%=GetLocaleResourceString("Forum.Posted")%>:
            <asp:Label ID="lblDate" CssClass="statvalue" runat="server" />
        </div>
        <div class="postbody">
            <asp:Label ID="lblText" CssClass="posttext" runat="server"></asp:Label>
            <asp:Label ID="lblForumPostID" runat="server" Visible="false"></asp:Label>
        </div>
        <asp:Panel runat="server" ID="pnlSignature" CssClass="signature">
            <asp:Label ID="lblSignature" runat="server"></asp:Label>
        </asp:Panel>
    </div>
    <div class="clear">
    </div>
</div>
