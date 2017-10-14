<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Header.ascx.cs" Inherits="PLSoundShare.Controls.Header" %>
<nav class="navbar navbar-default">
    <div class="container-fluid">
        <div class="navbar-header">
            <asp:HyperLink ID="Header_Home" class="navbar-brand" runat="server" NavigateUrl="../UserOverview.aspx">Home</asp:HyperLink>
            <% if (this.Page.User.Identity.IsAuthenticated && Session["loggedInUser"] != null){ %>
            <asp:HyperLink ID="Header_AudioManagement" class="navbar-brand" runat="server" NavigateUrl="../AudioManagement.aspx">AudioManagement</asp:HyperLink>
            <% } %>
        </div>
        <div class="navbar-right">
            <asp:LoginName ID="Header_LoginName" class="navbar-brand" runat="server" />
            <asp:LoginStatus ID="Header_LoginStatus" class="navbar-brand" runat="server" LoginText="Login" LogoutText="Sign Out" />
        </div>
    </div>
</nav>
