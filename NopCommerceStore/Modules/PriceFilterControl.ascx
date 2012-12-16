<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PriceFilterControl.ascx.cs"
    Inherits="NopSolutions.NopCommerce.Web.Modules.PriceFilterControl" %>
<script src="../Scripts/jquery-1.6.1.min.js" type="text/javascript"></script>
<script src="../Scripts/jquery.main.js" type="text/javascript"></script>
<script src="../Scripts/jquery.ui-slider.js" type="text/javascript"></script>
<link href="../css/main.css" rel="stylesheet" type="text/css" />
<div class="formCost">
    <label for="minCost">
        Цена: от</label>
    <input type="text" id="minCost" name="minCost" value="0"/>
    <div class="sliderCont">
        <div id="slider" class="ui-slider ui-slider-horizontal ui-widget-content ui-corner-all">
            <div class="ui-slider-range ui-widget-header" style="left: 100%; width: 0%;">
            </div>
            <a class="ui-slider-handle ui-corner-all" href="#" style="left: 48.199999999999996%;">
            </a><a class="ui-slider-handle ui-corner-all" href="#" style="left: 100%;"></a>
        </div>
    </div>
    <label for="maxCost" class="maxCost">
        до
    </label>
    <input type="text" id="maxCost" name="maxCost" value="<%=GetMaxValue%>"/>
    <input type="hidden" id="maxCostConst" value="<%=GetMaxValue%>"/>
</div>
