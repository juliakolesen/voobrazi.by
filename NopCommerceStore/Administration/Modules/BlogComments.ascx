<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.BlogCommentsControl"
    CodeBehind="BlogComments.ascx.cs" %>
<div class="section-title">
    <img src="Common/ico-content.png" alt="<%=GetLocaleResourceString("Admin.BlogComments.Title")%>" />
    <%=GetLocaleResourceString("Admin.BlogComments.Title")%>
</div>
<asp:DataPager ID="pagerBlogComments" runat="server" PagedControlID="lvBlogComments"
    PageSize="10">
    <Fields>
        <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" ShowLastPageButton="True" />
    </Fields>
</asp:DataPager>
<asp:ListView ID="lvBlogComments" runat="server" DataKeyNames="BlogCommentID" OnPagePropertiesChanging="lvBlogComments_OnPagePropertiesChanging">
    <LayoutTemplate>
        <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
    </LayoutTemplate>
    <ItemTemplate>
        <p>
            <%#BlogManager.FormatCommentText((string)Eval("CommentText"))%>
        </p>
        <p>
            <%#DateTimeHelper.ConvertToUserTime((DateTime)Eval("CreatedOn")).ToString()%>
            -
            <%#GetCustomerInfo (Convert.ToInt32(Eval("CustomerID")))%>
        </p>
        <p>
            <a href="BlogPostDetails.aspx?BlogPostID=<%#Eval("BlogPostID")%>">
                <%#Server.HtmlEncode(((BlogPost)Eval("BlogPost")).BlogPostTitle)%></a>
        </p>
        <asp:Button runat="server" ID="btnEditBlogComment" CssClass="adminButton" Text="<% $NopResources:Admin.BlogComments.EditButton.Text %>"
            ToolTip="<% $NopResources:Admin.BlogComments.EditButton.Tooltip %>" CommandName="Edit"
            OnCommand="btnEditBlogComment_Click" CommandArgument='<%#Eval("BlogCommentID")%>' />
        <asp:Button runat="server" ID="btnDeleteBlogComment" CssClass="adminButton" Text="<% $NopResources:Admin.BlogComments.DeleteButton.Text %>"
            ToolTip="<% $NopResources:Admin.BlogComments.DeleteButton.Tooltip %>" CommandName="Delete"
            OnCommand="btnDeleteBlogComment_Click" CommandArgument='<%#Eval("BlogCommentID")%>' />
    </ItemTemplate>
    <ItemSeparatorTemplate>
        <hr />
    </ItemSeparatorTemplate>
</asp:ListView>
