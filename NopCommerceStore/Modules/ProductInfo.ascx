<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.ProductInfoControl"
    CodeBehind="ProductInfo.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="RelatedProducts" Src="~/Modules/RelatedProducts.ascx" %>
<script src="../Scripts/jquery.js" type="text/javascript"></script>
<script src="../Scripts/daGallery.js" type="text/javascript"></script>
<script src="../Scripts/jquery.scroller.js" type="text/javascript"></script>
<link href="../css/jquery.scroller.css" rel="stylesheet" type="text/css" />
<link href="../css/ScrollerImagesTemplate.css" rel="stylesheet" type="text/css" />
<script language="javascript" type="text/javascript">
    $(function() {
        $('#basic').scroller();
    });
</script>
<style>
    .seccont_top .titl
    {
        padding-top: 13px;
    }
</style>
<div class="seccont_middle">
    <div class="text">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr valign="top">
                <td class="information">
                    <h2>
                        <asp:Literal ID="lProductName" runat="server" /></h2>
                    <p>
                        <asp:Literal ID="lShortDescription" runat="server" /></p>
                    <asp:Label runat="server" ID="lblAttributes" />
                    <span class="pink">Цена:</span>
                    <asp:Label runat="server" ID="lblPrice1" /><br />
                    <br />
                    <p runat="server" id="pUniqueProposal" visible="false" class="smaller">
                        <span class="pink">*</span> Для этого товара действует уникальное предложение по
                        доставке в любую точку города Минска.</p>
                </td>
                <td align="center">
                    <div class="large-price">
                        <div class="price">
                            <asp:Literal runat="server" ID="lblPrice2" /></div>
                    </div>
                    <%--<div style="width: 399px; height: 335px;">
						<a runat="server" id="aImg" rel="lightbox-cats" target="_blank" >
							<asp:Image ID="defaultImage"  runat="server" />
							<img src="/Images/zoom-icon7.gif" alt="" style="position: relative; bottom:238px; right:30px; cursor:pointer;" />
						</a>
					</div>--%>
                    <div class="daGallery">
                        <div class="gPicSpace">
                            <a id="a_imgProduct" name="a_imgProduct" rel="galI1" target="_blank" runat="server">
                                <img id="imgProduct" name="imgProduct" runat="server" class="daGalleryImage" />
                            </a>
                        </div>
                        <div style="text-align: center;">
                            <div class="page-wrap">
                                <asp:Panel ID="PanelImages" runat="server">
                                    <article>
		                        	<section id="basic">
                                           <ul>
                                                <asp:DataList ID="dlImages" runat="server" RepeatDirection="Horizontal"
                                                    RepeatLayout="Table" ItemStyle-VerticalAlign="top" HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <li><a class="gPic" rel="galI" target="_blank" href='<%# GetPictureUrl((ProductPicture)Container.DataItem, largeImageSize)  %>'
                                                            title='<%# AlternateText %>'>
                                                            <img src='<%# GetPictureUrl((ProductPicture)Container.DataItem, smallImageSize)  %>'
                                                                onmouseover="document.imgProduct.src = '<%# GetPictureUrl((ProductPicture)Container.DataItem, middleImageSize)  %>';
                                                                             document.getElementById('<%# a_imgProduct.ClientID%>').href = '<%# GetPictureUrl((ProductPicture)Container.DataItem, largeImageSize)  %>';">
                                                        </a></li>
                                                    </ItemTemplate>
                                                </asp:DataList>
                                            </ul>
                                      </section>
		                       </article>
                                </asp:Panel>
                            </div>
                        </div>
                        <script type="text/javascript">
                            DaGallery.init();
                        </script>
                    </div>
                    <table runat="server" id="tblOrderButtons" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:LinkButton ID="lbOrder" runat="server" CssClass="button-order" OnCommand="btnAddToCart_Click" />
                            </td>
                            <td style="padding-left: 10px;">
                                <asp:LinkButton ID="lbOrderAndCheckout" runat="server" CssClass="button-order2" OnCommand="lbOrderAndCheckout_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <nopCommerce:RelatedProducts ID="ctrlRelatedProducts" runat="server" />
                </td>
            </tr>
        </table>
        <div class="post">
            <asp:Literal ID="lFullDescription" runat="server" />
        </div>
    </div>
</div>
