<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.BlogControl"
    CodeBehind="Blog.ascx.cs" %>
<div class="blog">
    <div class="title">
        <table width="100%">
            <tr>
                <td style="text-align: left; vertical-align: middle;">
                    <%=GetLocaleResourceString("Blog.Blog")%>
                </td>
                <td style="text-align: right; vertical-align: middle;">
                    <a href="<%= GetBlogRSSUrl()%>">
                        <asp:Image ID="imgRSS" runat="server" ImageUrl="~/images/icon_rss.gif" AlternateText="RSS" />
                    </a>
                </td>
            </tr>
        </table>
    </div>
    <div class="clear">
    </div>
    <div class="blogposts">
        <asp:Repeater ID="rptrBlogPosts" runat="server" OnItemDataBound="rptrBlogPosts_ItemDataBound">
            <ItemTemplate>
                <div class="post">
                    <a class="blogTitle" href="<%#SEOHelper.GetBlogPostURL(Convert.ToInt32(Eval("BlogPostID")))%>">
                        <%#Server.HtmlEncode(Eval("BlogPostTitle").ToString())%></a><span class="blogDate">
                            -
                            <%#DateTimeHelper.ConvertToUserTime((DateTime)Eval("CreatedOn")).ToString("d")%>
                        </span>
                    <div class="blogBody">
                        <%#Eval("BlogPostBody")%>
                    </div>
                    <a href="<%#SEOHelper.GetBlogPostURL(Convert.ToInt32(Eval("BlogPostID")))%>" class="blogDetails">
                        <asp:Literal ID="lComments" runat="server"></asp:Literal>
                    </a>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</div>
