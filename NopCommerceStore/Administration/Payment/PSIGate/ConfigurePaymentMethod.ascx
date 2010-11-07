<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="NopSolutions.NopCommerce.Web.Administration.Payment.PSIGate.ConfigurePaymentMethod" Codebehind="ConfigurePaymentMethod.ascx.cs" %>
<table class="adminContent">
    <tr>
        <td colspan="2" width="100%">
            <hr />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <b>If you're using this gateway remember that you should set your store primary currency
                to US Dollar.</b>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            Use Sandbox:
        </td>
        <td class="adminData">
            <asp:CheckBox ID="cbUseSandbox" runat="server"></asp:CheckBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            Store ID:
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtStoreID" runat="server" CssClass="adminInput"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            Passphrase:
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtPassphrase" runat="server" CssClass="adminInput"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td colspan="2"><br /><br /><br /><br />
            <b>Test account</b> <br /><br />To process a transaction through the test account, pass the
            following:
            <br />
            StoreID: teststore
            <br />
            Passphrase: psigate1234
            <br /><br />
            To review your test transactions, log into <a href="https://dev.psigate.com">https://dev.psigate.com</a>
            with the following account information:
            <br />
            CID : 1000001
            <br />
            User: teststore
            <br />
            pass: testpass
            <br /><br />
            If you require an unshared test account, PSI Gate will provide one to you once you have
            begun the application process.
        </td>
    </tr>
</table>
