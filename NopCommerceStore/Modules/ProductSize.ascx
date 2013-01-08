<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductSize.ascx.cs"
    Inherits="NopSolutions.NopCommerce.Web.Modules.ProductSize" %>
<link href="../css/style.css" rel="stylesheet" type="text/css" />
<link href="../css/jquery-ui-1.8.19.custom.css" rel="stylesheet" type="text/css" />
<script src="../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<script src="../Scripts/jquery-ui-1.8.19.custom.min.js" type="text/javascript"></script>
<script src="../Scripts/jquery.dual_slider.js" type="text/javascript"></script>
<div class="div_size">
    <table>
        <tr>
            <td width="40%">
                <div id="slider_height">
                </div>
                <img src="../images/ff_images/slider/flowers.jpg" width="180" height="180" />
                <div id="slider_width">
                </div>
            </td>
            <td width="400">
                <div>
                    Высота:
                    <label for="minHeight">
                        От:
                        <input type="text" name="price" id="minHeight" />
                    </label>
                    <label for="maxHeight">
                        До:
                        <input type="text" name="price2" id="maxHeight" value="<%=GetMaxHeight%>"/>
                        <input type="hidden" id="maxHeightConst" value="<%=GetMaxHeight%>"/>
                    </label>
                </div>
                <br />
                <div>
                    Ширина:
                    <label for="minWidth">
                        От:
                        <input type="text" name="price" id="minWidth" />
                    </label>
                    <label for="maxWidth">
                        До:
                        <input type="text" name="price2" id="maxWidth" value="<%=GetMaxWidth%>"/>
                        <input type="hidden" id="maxWidthConst" value="<%=GetMaxWidth%>"/>
                    </label>
                </div>
            </td>
        </tr>
    </table>
</div>
