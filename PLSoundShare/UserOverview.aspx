<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserOverview.aspx.cs" Inherits="PLSoundShare.UserOverview" %>
<%@ Register Src="~/Controls/Header.ascx" TagPrefix="template" TagName="Header" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Overview</title>
    <link rel="stylesheet" href="/CSS/style.css" />
    <link rel="stylesheet" href="/CSS/bootstrap.min.css" />
    <link rel="shortcut icon" type="image/x-icon" href="/IMG/favicon.ico" />
    <script src="/JS/bootstrap.min.js"></script>
    <script src="/JS/jquery-3.2.1.min.js"></script>
</head>
<body>
    <form id="form1" class="container" runat="server" visible="True">
        <template:Header runat="server" ID="Header" />
        <asp:Label ID="UserOverview_Error" runat="server"></asp:Label>
        <asp:GridView ID="UserOverview_Table" CssClass="table table-bordered" runat="server"
            AutoGenerateColumns="False"
            EmptyDataText="Keine Kunden in der Datenbank">
            <Columns>
                <asp:BoundField DataField="username" HeaderText="Username" />
                <asp:BoundField DataField="email" HeaderText="Email" />
                <asp:HyperLinkField HeaderText="Select User" ControlStyle-CssClass="btn btn-primary" Text="Select" DataNavigateUrlFields="username" DataNavigateUrlFormatString="/UserMusicOverview.aspx?username={0}" />
            </Columns>
        </asp:GridView>
    </form>
</body>
</html>
