<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.ProductReviews"
    CodeBehind="ProductReviews.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="ProductReviewHelpfulness" Src="~/Modules/ProductReviewHelpfulness.ascx" %>
<div class="productReviewBox">
    <div>
        <asp:Button runat="server" ID="btnWriteReview" Text="<% $NopResources:Products.WriteReview %>"
            OnClick="btnWriteReview_Click" SkinID="ProductWriteReviewButton"></asp:Button>
    </div>
    <div class="clear">
    </div>
    <asp:Panel class="productReviewList" runat="server" ID="pnlReviews">
        <asp:Repeater ID="rptrProductReviews" runat="server">
            <ItemTemplate>
                <div class="productReviewItem">
                    <div class="title">
                        <%#Server.HtmlEncode((string)Eval("Title"))%>
                    </div>
                    <div class="rating">
                        <ajaxToolkit:Rating ID="productRating" AutoPostBack="false" runat="server" MaxRating="5"
                            StarCssClass="ratingStar" WaitingStarCssClass="savedRatingStar" FilledStarCssClass="filledRatingStar"
                            EmptyStarCssClass="emptyRatingStar" ReadOnly="true" CurrentRating='<%#Eval("Rating")%>' />
                    </div>
                    <div class="clear">
                    </div>
                    <%#ProductManager.FormatProductReviewText((string)Eval("ReviewText"))%>
                    <p>
                        <%=GetLocaleResourceString("Products.ProductReviewFrom")%>:
                        <%#Server.HtmlEncode(GetCustomerInfo(Convert.ToInt32(Eval("CustomerID"))))%>
                        |
                        <%=GetLocaleResourceString("Products.ProductReviewCreatedOn")%>:
                        <%#DateTimeHelper.ConvertToUserTime((DateTime)Eval("CreatedOn")).ToString()%>
                    </p>
                    <nopCommerce:ProductReviewHelpfulness ID="ctrlProductReviewHelpfulness" runat="server"
                        ProductReviewID='<%#Eval("ProductReviewID")%>'></nopCommerce:ProductReviewHelpfulness>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </asp:Panel>
</div>
