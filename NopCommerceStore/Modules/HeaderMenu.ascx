<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.HeaderMenuControl"
    CodeBehind="HeaderMenu.ascx.cs" %>
<table align="center">
    <tr>
        <td>
            <a id="Action" class="amenu" title="�����" href="~/Actions.aspx" runat="server">
            </a>
        </td>
        <td>
            <a id="Clients" class="amenu" title="������������� ��������" href="~/ToCorporateClients.aspx"
                runat="server"></a>
        </td>
        <td>
            <a id="Green" class="amenu" title="����������, ���������� �������" href="~/Greening.aspx"
                runat="server"></a>
        </td>
        <td>
            <a id="News" class="amenu" title="�������, ����������" href="~/NewsAll.aspx" runat="server">
            </a>
        </td>
    </tr>
</table>
<div>
    <img src="<%=Page.ResolveUrl("~/images/ff_images/menu/menu_footer.gif")%>" alt=""
        width="1050" height="30" />
</div>
