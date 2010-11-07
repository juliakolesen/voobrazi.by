<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.ProductRatingControl"
    CodeBehind="ProductRating.ascx.cs" %>
<div class="ProductRatingBox">
    <ajaxToolkit:Rating ID="productRating" AutoPostBack="true" runat="server" CurrentRating="2"
        MaxRating="5" StarCssClass="ratingStar" WaitingStarCssClass="savedRatingStar"
        FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar" OnChanged="productRating_Changed"
        Style="float: left;" />
    <br />
    <br />
    <asp:Label runat="server" ID="lblProductRatingResult"></asp:Label>
</div>
