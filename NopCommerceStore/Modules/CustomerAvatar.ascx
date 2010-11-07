<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Modules.CustomerAvatarControl"
    CodeBehind="CustomerAvatar.ascx.cs" %>
<div class="CustomerAvatar">
    <asp:Panel runat="server" ID="pnlCustomerAvatarError" CssClass="ErrorBlock">
        <div class="messageError">
            <asp:Literal ID="lCustomerAvatarErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
        </div>
    </asp:Panel>
    <div class="clear">
    </div>
    <div class="SectionBody">
        <asp:Image ID="iAvatar" runat="server" AlternateText="Avatar" />
        <br />
        <asp:FileUpload ID="fuAvatar" runat="server" ToolTip="Choose a new avatar image to upload." />
        <br />
        <asp:Button ID="btnUploadAvatar" runat="server" OnClick="btnUploadAvatar_Click" Text="<% $NopResources:Account.UploadAvatar %>"
            SkinID="UploadAvatarButton" /><br style="line-height: 1px;" />
        <br />
        <%=GetLocaleResourceString("Account.UploadAvatarRules")%>
    </div>
    <div class="clear">
    </div>
    <div class="Button">
        <asp:Button ID="btnRemoveAvatar" runat="server" OnClick="btnRemoveAvatar_Click" Text="<% $NopResources:Account.RemoveAvatar %>"
            CausesValidation="false" SkinID="RemoveAvatarButton" /><br style="line-height: 1px;" />
    </div>
</div>
