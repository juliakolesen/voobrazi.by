<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.ProductBox1Control"
	CodeBehind="ProductBox1.ascx.cs" %>

	<table cellpadding="0" cellspacing="0">
		<tr>
			<td>
				<div class="middle-price">
					<div class="price">
						<asp:Literal ID="lblPrice" runat="server" /></div>
				</div>
				<div style="height:198px; width:196px;">
					<asp:HyperLink ID="hlImageLink" runat="server" />
				</div>
			</td>
		</tr>
		<tr>
			<td>
				<div align="left" id="divProductInfoLink" style="cursor:pointer;" onclick='document.location.href=document.getElementById("<%=hlImageLink.ClientID %>").href;'><img src="/images/ff_images/recycle.jpg" alt="" height="30" /> заказать</div>
			</td>
		</tr>
		<tr>
			<td>
				<h1 style="margin-top:2px;">
					<asp:Literal ID="hlProduct" runat="server" /></h1>
				<asp:Literal runat="server" ID="lShortDescription"></asp:Literal>
			</td>
		</tr>
	</table>
