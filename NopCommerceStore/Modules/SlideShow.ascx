<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SlideShow.ascx.cs" Inherits="NopSolutions.NopCommerce.Web.Modules.SlideShow" %>
<%@ Import Namespace="NopSolutions.NopCommerce.BusinessLogic.Content.Topics" %>
<%@ Register TagPrefix="nopCommerce" TagName="Topic" Src="./Topic.ascx" %>
<script src="../Scripts/slideshow.js" type="text/javascript"></script>
<link href="../css/slideshow.css" rel="stylesheet" type="text/css" />
<div id="divSlider" runat="server" class="divSlider">
    <div id="slideshow">
        <div id="slidesContainer">
            <asp:DataList ID="topics" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table"
                ItemStyle-VerticalAlign="top" HorizontalAlign="Center">
                <ItemTemplate>
                    <div class="slide">
                        <nopCommerce:Topic ID="Topic1" runat="server" TopicName='<%# GetTopicName((LocalizedTopic)Container.DataItem)%>' />
                    </div>
                </ItemTemplate>
            </asp:DataList>
        </div>
    </div>
</div>
