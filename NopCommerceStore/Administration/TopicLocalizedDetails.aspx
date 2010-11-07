<%@ Page Language="C#" MasterPageFile="~/Administration/main.master" AutoEventWireup="true"
    Inherits="NopSolutions.NopCommerce.Web.Administration.Administration_TopicLocalizedDetails"
    ValidateRequest="false" CodeBehind="TopicLocalizedDetails.aspx.cs" %>

<%@ Register TagPrefix="nopCommerce" TagName="TopicLocalizedDetails" Src="Modules/TopicLocalizedDetails.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="server">
    <nopCommerce:TopicLocalizedDetails runat="server" ID="ctrlTopicLocalizedDetails" />
</asp:Content>
