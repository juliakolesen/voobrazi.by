<%@ Control Language="C#" AutoEventWireup="true" Inherits="NopSolutions.NopCommerce.Web.Administration.Modules.ShippingRateComputationMethodsControl"
    CodeBehind="ShippingRateComputationMethods.ascx.cs" %>
<%@ Register TagPrefix="nopCommerce" TagName="ToolTipLabel" Src="ToolTipLabelControl.ascx" %>
<%@ Register TagPrefix="nopCommerce" TagName="DecimalTextBox" Src="DecimalTextBox.ascx" %>
<div class="section-header">
    <div class="title">
        <img src="Common/ico-configuration.png" alt="<%=GetLocaleResourceString("Admin.ShippingRateComputationMethods.Title")%>" />
        <%=GetLocaleResourceString("Admin.ShippingRateComputationMethods.Title")%>
    </div>
    <div class="options">
        <input type="button" onclick="location.href='ShippingRateComputationMethodAdd.aspx'"
            value="<%=GetLocaleResourceString("Admin.ShippingRateComputationMethods.AddButton.Text")%>"
            id="btnAddNew" class="adminButtonBlue" title="<%=GetLocaleResourceString("Admin.ShippingRateComputationMethods.AddButton.Tooltip")%>" />
    </div>
</div>
<br />
<asp:UpdatePanel ID="UpdatePanelShippingRateComputationMethods" runat="server">
    <ContentTemplate>
        <asp:GridView ID="gvShippingRateComputationMethods" runat="server" AutoGenerateColumns="False"
            Width="100%">
            <Columns>
                <asp:BoundField DataField="ShippingRateComputationMethodID" HeaderText="ShippingRateComputationMethod ID"
                    Visible="False"></asp:BoundField>
                <asp:TemplateField HeaderText="<% $NopResources:Admin.ShippingRateComputationMethods.Name %>"
                    ItemStyle-Width="60%">
                    <ItemTemplate>
                        <%#Eval("Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<% $NopResources:Admin.ShippingRateComputationMethods.DisplayOrder %>"
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="13%" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%#Eval("DisplayOrder")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<% $NopResources:Admin.ShippingRateComputationMethods.IsDefault %>"
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="13%" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:HiddenField runat="server" ID="hfShippingRateComputationMethodID" Value='<%#Eval("ShippingRateComputationMethodID")%>' />
                        <asp:RadioButton runat="server" ID="rdbIsDefault" Checked='<%#Eval("IsDefault")%>'
                            OnCheckedChanged="rdbIsDefault_CheckedChanged" AutoPostBack="true" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<% $NopResources:Admin.ShippingRateComputationMethods.Edit %>"
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="13%" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <a href="ShippingRateComputationMethodDetails.aspx?ShippingRateComputationMethodID=<%#Eval("ShippingRateComputationMethodID")%>"
                            title="<%#GetLocaleResourceString("Admin.ShippingRateComputationMethods.Edit.Tooltip")%>">
                            <%#GetLocaleResourceString("Admin.ShippingRateComputationMethods.Edit")%></a>
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
