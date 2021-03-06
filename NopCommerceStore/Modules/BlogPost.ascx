﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.BlogPostControl"
    CodeBehind="BlogPost.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="BlogComment" Src="~/Modules/BlogComment.ascx" %>
<div class="blogpost">
    <div class="title">
        <asp:Literal runat="server" ID="lBlogPostTitle"></asp:Literal>
    </div>
    <div class="clear">
    </div>
    <div class="postDate">
        <asp:Literal runat="server" ID="lCreatedOn"></asp:Literal>
    </div>
    <div class="postbody">
        <asp:Literal runat="server" ID="lBlogPostBody"></asp:Literal>
    </div>
    <div id="pnlComments" runat="server" class="blogComments">
        <div class="title">
            <%=GetLocaleResourceString("Blog.Comments")%>
        </div>
        <div class="clear">
        </div>
        <div class="newComment">
            <table>
                <tr>
                    <td colspan="2" class="leaveTitle">
                        <strong>
                            <asp:Literal runat="server" ID="lblLeaveYourComment"></asp:Literal>
                        </strong>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%=GetLocaleResourceString("Blog.CommentText")%>:
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtComment" TextMode="MultiLine" ValidationGroup="NewComment"
                            SkinID="BlogAddCommentCommentText"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvComment" runat="server" ControlToValidate="txtComment"
                            ErrorMessage="<% $NopResources:Blog.PleaseEnterCommentText %>" ToolTip="<% $NopResources:Blog.PleaseEnterCommentText %>"
                            ValidationGroup="NewComment">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr runat="server" id="pnlError">
                    <td class="messageError" colspan="2">
                        <asp:Literal ID="lErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td class="button">
                        <asp:Button runat="server" ID="btnComment" Text="<% $NopResources:Blog.NewCommentButton %>"
                            ValidationGroup="NewComment" OnClick="btnComment_Click" SkinID="BlogPostAddCommentButton">
                        </asp:Button>
                    </td>
                </tr>
            </table>
        </div>
        <div class="clear">
        </div>
        <div class="commentList">
            <asp:Repeater ID="rptrComments" runat="server">
                <ItemTemplate>
                    <nopCommerce:BlogComment ID="ctrlBlogComment" runat="server" BlogComment='<%# Container.DataItem %>' />
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</div>
