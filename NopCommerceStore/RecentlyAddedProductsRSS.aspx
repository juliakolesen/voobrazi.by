<%@ Page Language="C#" AutoEventWireup="true" ContentType="text/xml" MaintainScrollPositionOnPostback="false"
    EnableTheming="false" Inherits="NopSolutions.NopCommerce.Web.RecentlyAddedProductsRSSPage"
    CodeBehind="RecentlyAddedProductsRSS.aspx.cs" %>

<head id="Head1" runat="server" visible="false">
</head>
<asp:repeater id="rptrRecentlyAddedProducts" runat="server">
    <HeaderTemplate>
        <rss version="2.0">
         <channel>
            <title><![CDATA[<%# SettingManager.StoreName%>: Recently added products]]></title>
            <link><%# CommonHelper.GetStoreLocation(false)%></link>
            <description><%# SettingManager.StoreName%></description>
            <copyright>Copyright <%= DateTime.Now.Year.ToString()%> by <%# SettingManager.StoreName%></copyright>
    </HeaderTemplate>
    <ItemTemplate>
        <item>
         <title><![CDATA[<%# Eval("Name") %>]]></title>
         <author><![CDATA[<%# SettingManager.StoreName%>]]></author>
         <description><![CDATA[<%# Eval("ShortDescription") %>]]></description>
         <link><![CDATA[<%# SEOHelper.GetProductURL(Convert.ToInt32(Eval("ProductID"))) %>]]></link>
         <pubDate><%# string.Format("{0:R}", DateTimeHelper.ConvertToUserTime((DateTime)Eval("CreatedOn")))%></pubDate>
      </item>
    </ItemTemplate>
    <FooterTemplate>
        </channel> </rss>
    </FooterTemplate>    
</asp:repeater>
