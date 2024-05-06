<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DeletePopup.aspx.cs" Inherits="DeletePopup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="css/style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="ph" runat="server" style="color: Red">
        <b>Warning:</b><br />
        <asp:Label ID="lblError" Text="" runat="server" ForeColor="Black" Font-Bold="true"></asp:Label>
        <br />
        <br />
    </div>
    </form>
</body>
</html>
