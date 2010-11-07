<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.TaxProvidersControl"
    CodeBehind="TaxProviders.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="DecimalTextBox" Src="DecimalTextBox.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="ToolTipLabel" Src="ToolTipLabelControl.ascx" %>
<div class="section-header">
    <div class="title">
        <img src="Common/ico-configuration.png" alt="<%=GetLocaleResourceString("Admin.TaxProviders.Title")%>" />
        <%=GetLocaleResourceString("Admin.TaxProviders.Title")%>
    </div>
    <div class="options">
        <input type="button" onclick="location.href='TaxProviderAdd.aspx'" value="<%=GetLocaleResourceString("Admin.TaxProviders.AddNewButton.Text")%>"
            id="btnAddNew" class="adminButtonBlue" title="<%=GetLocaleResourceString("Admin.TaxProviders.AddNewButton.Tooltip")%>" />
    </div>
</div>
<br />
<asp:UpdatePanel ID="UpdatePanelTaxProviders" runat="server">
    <ContentTemplate>
        <asp:GridView ID="gvTaxProviders" runat="server" AutoGenerateColumns="False" Width="100%">
            <Columns>
                <asp:BoundField DataField="TaxProvidersID" HeaderText="TaxProviders ID" Visible="False">
                </asp:BoundField>
                <asp:TemplateField HeaderText="<% $NopResources:Admin.TaxProviders.Name %>" ItemStyle-Width="60%">
                    <ItemTemplate>
                        <%#Eval("Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<% $NopResources:Admin.TaxProviders.DisplayOrder %>"
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="13%" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%#Eval("DisplayOrder")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<% $NopResources:Admin.TaxProviders.IsDefault %>"
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="13%" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:HiddenField runat="server" ID="hfTaxProviderID" Value='<%#Eval("TaxProviderID")%>' />
                        <asp:RadioButton runat="server" ID="rdbIsDefault" Checked='<%#Eval("IsDefault")%>'
                            OnCheckedChanged="rdbIsDefault_CheckedChanged" AutoPostBack="true" ToolTip="<% $NopResources:Admin.TaxProviders.IsDefault.Tooltip %>" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<% $NopResources:Admin.TaxProviders.Edit %>" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-Width="13%" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <a href="TaxProviderDetails.aspx?TaxProviderID=<%#Eval("TaxProviderID")%>" title="<%#GetLocaleResourceString("Admin.TaxProviders.Edit.Tooltip")%>">
                            <%#GetLocaleResourceString("Admin.TaxProviders.Edit")%>
                        </a>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdateProgress ID="up1" runat="server">
    <ProgressTemplate>
        <div class="progress">
            <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/images/UpdateProgress.gif" />
            <%=GetLocaleResourceString("Admin.Common.Wait...")%>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
