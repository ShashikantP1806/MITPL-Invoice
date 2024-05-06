<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ForgotPassword.aspx.cs" Inherits="ForgotPassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MITPL e-Invoice System</title>
    <link id="Link1" runat="server" rel="shortcut icon" href="images/NewLogo.ico" type="image/x-icon" />
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <%--
    <script type="text/javascript">
        function goBack() {
            window.history.back()
        }</script>--%>

    <script type="text/javascript">
        window.history.forward(-1);
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <center>
        <div id="LoginScreen" class="LoginScreen" style="padding: 15px 8px 0 8px;">
            <img src="css/images/MITPL logo.gif" height="73px" />
            <table style="width: 90%; height: 149px; padding: 10px 0 0 70px;">
                <tr>
                    <td>
                        Emp. Id:
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtUserID" EnableViewState="false" runat="server" Autocomplete="Off"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvEmpId" runat="server" ErrorMessage="*" ControlToValidate="txtUserID" SetFocusOnError="true"
                           ToolTip="Required" ValidationGroup="vgReset"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revEmpId" runat="server" ErrorMessage="*" ControlToValidate="txtUserID" SetFocusOnError="true"
                            tooltip="Emp Id should be number" ValidationExpression="^\d+$" ValidationGroup="vgReset"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td  colspan="2">
                        <asp:Button ID="btnReset" Text="Submit" runat="server" OnClick="btnReset_Click" ValidationGroup="vgReset" />
                        <asp:Button ID="btnCancel" Text="Cancel" runat="server" OnClick="btnCancel_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="left">
                        <asp:Label ID="lblError" ForeColor="Black" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div style="background-color: #5D7B9D; width: 95%; height: 20px; text-align: center;
            -moz-border-radius: 5px; border-radius: 5px; vertical-align: middle; line-height: 1.5em;
            margin-top: 225px;">
            <div style="background-color: #5D7B9D; -moz-border-radius: 5px; border-radius: 5px;">
                <div style="background-color: #5D7B9D; -moz-border-radius: 5px; border-radius: 5px;">
                    <span style="color: WhiteSmoke;">Copyright © 2013 </span><a href="http://www.mangalaminfotech.com"
                        target="_blank" class="Linkcss"><span>Mangalam Information Technologies Pvt. Ltd.</span></a><span
                            style="color: WhiteSmoke;"> | </span><a href="http://www.mangalaminfotech.com" target="_blank">
                                <span style="color: White;"></span></a><span style="color: White;">Developed by:
                    
                    
                    
                    
                    
                    </span><a href="AboutUs.aspx" class="Linkcss"><span>System and Software Department (SSD)
                        Team</span></a>
                </div>
                <!-- end of templatemo_footer -->
            </div>
        </div>
    </center>
    </form>
</body>
</html>
