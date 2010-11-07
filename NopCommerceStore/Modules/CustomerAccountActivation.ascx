<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.CustomerAccountActivationControl"
    CodeBehind="CustomerAccountActivation.ascx.cs" %>
<div class="AccountActivationPage">
    <div class="title">
        <%=GetLocaleResourceString("Account.AccountActivation")%>
    </div>
    <div class="clear">
    </div>
    <div class="body">
        <div runat="server">
            <strong>
                <asp:Literal runat="server" ID="lResult"></asp:Literal>
            </strong>
        </div>
    </div>
</div>
