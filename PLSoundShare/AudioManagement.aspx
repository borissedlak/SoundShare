<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AudioManagement.aspx.cs" Inherits="PLSoundShare.AudioManagement" %>

<%@ Register Src="~/Controls/Header.ascx" TagPrefix="template" TagName="Header" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Audio Management</title>
    <link rel="stylesheet" href="/CSS/style.css" />
    <link rel="stylesheet" href="/CSS/bootstrap.min.css" />
    <link rel="shortcut icon" type="image/x-icon" href="/IMG/favicon.ico" />
    <script src="/JS/jquery-3.2.1.min.js"></script>
    <script src="/JS/bootstrap.min.js"></script>

    <script>
        //Validating the file size on the client side first in order to prevent unneccessary requests
        //Returning true/false for the validation performed on upload button click
        function validateFileSize() {

            var filesize = (jQuery("#AudioManagement_FileUpload"))[0].files[0].size;
            console.log(filesize);

            if (filesize > 10485760) {
                jQuery("#AudioManagement_Error").text("Files must be less than 10MByte!");
                return false;
            }
            else {
                return true;
            }

        }
    </script>
</head>
<body>
    <form id="form1" class="container" runat="server">
        <template:Header runat="server" ID="Header" />
        <div id="AudioManagement_Upload" class="form-group col-md-6">
            <h1>Audio Upload</h1>
            <asp:Label ID="AudioManagement_AliasLabel" runat="server" AssociatedControlID="AudioManagement_Alias">Alias</asp:Label>
            <asp:RequiredFieldValidator ID="AudioManagement_AliasRequire" runat="server" ControlToValidate="AudioManagement_Alias" ErrorMessage="Alias is required." ToolTip="Alias is required.">*</asp:RequiredFieldValidator>
            <asp:TextBox ID="AudioManagement_Alias" runat="server" CssClass="form-control" placeholder="Alias"></asp:TextBox>
            <br />
            <asp:Label ID="AudioManagement_DescriptionLabel" runat="server" AssociatedControlID="AudioManagement_Description">Description</asp:Label>
            <asp:TextBox ID="AudioManagement_Description" runat="server" CssClass="form-control" placeholder="Description"></asp:TextBox>
            <br />
            <asp:Label ID="AudioManagement_FileUploadLabel" runat="server" AssociatedControlID="AudioManagement_FileUpload">File Upload</asp:Label>
            <asp:FileUpload ID="AudioManagement_FileUpload" runat="server" />
            <asp:RegularExpressionValidator ID="AudioManagement_FileExtensionValidator" runat="server" ForeColor="Red" ErrorMessage="Only mp3 files are allowed!"
                ValidationExpression="^.+(.mp3|.MP3)$" ControlToValidate="AudioManagement_FileUpload"></asp:RegularExpressionValidator>
            <br />
            <asp:Button ID="AudioManagement_FileUploadButton" runat="server" Text="Submit" OnClick="AudioManagement_FileUploadButton_Click"
                OnClientClick="if (validateFileSize() == false) return(false);" UseSubmitBehavior="false" />
            <asp:Label ID="AudioManagement_Error" runat="server" ForeColor="Red"></asp:Label>
        </div>
        <div id="AudioManagement_Overview" class="col-md-12">
            <hr />
            <h1>File Management</h1>
            <asp:GridView ID="AudioManagement_Table" runat="server" CssClass="table table-bordered"
                AutoGenerateColumns="False" OnRowDeleting="AudioManagement_Table_RowDeleting"
                EmptyDataText="You have no uploaded files." DataKeyNames="ID">
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
                    <asp:CommandField ShowDeleteButton="True" DeleteText="Delete" ControlStyle-CssClass="btn btn-danger" />
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
