<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserMusicOverview.aspx.cs" Inherits="PLSoundShare.UserMusicOverview" %>

<%@ Register Src="~/Controls/Header.ascx" TagPrefix="template" TagName="Header" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Music Overview</title>
    <link rel="stylesheet" href="/CSS/style.css" />
    <link rel="stylesheet" href="/CSS/bootstrap.min.css" />
    <link rel="shortcut icon" type="image/x-icon" href="/IMG/favicon.ico" />
    <script src="/JS/bootstrap.min.js"></script>
    <script src="/JS/jquery-3.2.1.min.js"></script>
</head>
<body>
    <form id="form1" class="container" runat="server" visible="True">
        <template:Header runat="server" ID="Header1" />
        <h1><%: Request["username"] %></h1>
        <asp:Label ID="UserMusicOverview_Error" runat="server"></asp:Label>
        <asp:GridView ID="UserMusicOverview_Table" CssClass="table table-bordered" runat="server"
            AutoGenerateColumns="False"
            EmptyDataText="No Files from this User"
            DataKeyNames="ID" OnRowDataBound="UserMusicOverview_Table_RowDataBound"
            OnRowCommand="UserMusicOverview_Table_Like">
            <Columns>
                <asp:BoundField DataField="fileAlias" HeaderText="Title" />
                <asp:BoundField DataField="description" HeaderText="Description" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <audio controls="controls">
                            <source src="/AudioUpload/<%# Eval("ID") %>.mp3" type="audio/mpeg">
                            Your browser does not support the audio element.
                        </audio>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:ButtonField Text="Like" ButtonType="Link" ControlStyle-CssClass="btn btn-success" CommandName="Like" headertext=""></asp:ButtonField>
                <asp:ButtonField Text="Unlike" ButtonType="Link" ControlStyle-CssClass="btn btn-danger" CommandName="Unlike" headertext=""></asp:ButtonField>
            </Columns>
        </asp:GridView>
    </form>
</body>
</html>
