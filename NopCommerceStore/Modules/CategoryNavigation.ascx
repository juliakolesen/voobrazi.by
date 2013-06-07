<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.CategoryNavigation"
    CodeBehind="CategoryNavigation.ascx.cs" %>

<div class="flowersWholesale">
    <table>
        <tr>
            <td>
                <div id="banner">
                    <asp:HyperLink runat="server" ID="flowersWholesaleLink" title="Цветы оптом" NavigateUrl="~/FlowersWholesale.aspx">
					<img src="../images/ff_images/FlowersWholesale.gif" alt="" height="150" />
                    </asp:HyperLink>
                </div>
            </td>
            <td>
                <a id="openA" href="javascript:void(0);" onclick="javascript: 
                                                                              $('#banner').slideToggle('slow');
                                                                                if (imgClose.style.visibility == 'visible') {
                                                                                    imgClose.style.visibility = 'hidden';
                                                                                    imgOpen.style.visibility = 'visible';
                                                                                } else {
                                                                                    imgClose.style.visibility = 'visible';
                                                                                    imgOpen.style.visibility = 'hidden';
                                                                                }">
                    <img id="imgClose" src="../images/unchecked.gif" alt="" style="visibility:visible" title="Свернуть"/>
                    <img id="imgOpen" src="../images/checked.gif" alt="" style="visibility:hidden" title="Развернуть"/>
                </a>
            </td>
        </tr>
    </table>
</div>
<div class="submenu">
    <asp:PlaceHolder runat="server" ID="phCategories" />
</div>
