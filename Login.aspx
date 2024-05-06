<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MITPL e-Invoice System</title>
    <link id="Link1" runat="server" rel="shortcut icon" href="images/NewLogo.ico" type="image/x-icon" />
    <link href="css/style.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        window.history.forward(-1);
    </script>

</head>
<body>  
    <form id="form1" runat="server">
    <center>
        <div id="LoginScreen" class="LoginScreen" style="padding: 15px 8px 0 8px">
            <img src="css/images/MITPL logo.gif" height="73px" />
    <table style="width: 90%; height: 149px; padding: 10px 0 0 70px">              
                <tr id="trerror" runat="server" visible="false">
                    <td>
                        <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table align="center">
                            <tr>
                                <td>
                                    Emp. Id:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtUserId" runat="server" autocomplete="off"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rqtxtUserId" runat="server" ControlToValidate="txtUserId"
                                        Text="*" ErrorMessage="User name cannot be blank" ValidationGroup="vg" SetFocusOnError="true"
                                        ToolTip="Required"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revtxtUserId" runat="server" ControlToValidate="txtUserId"
                                        Text="*" ErrorMessage="Emp Id should be number" ToolTip="Emp Id should be number" ValidationGroup="vg" SetFocusOnError="true"
                                        ValidationExpression="^[0-9]*$"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Password:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rqtxtPassword" runat="server" ControlToValidate="txtPassword"
                                        Text="*" ErrorMessage="Password cannot be blank" ValidationGroup="vg" SetFocusOnError="true"
                                        ToolTip="Required"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td align="right" style="padding-right:28px; padding-top:5px;">
                                    <asp:Button ID="btnLogin" runat="server" Text="Login" ValidationGroup="vg" OnClick="btnLogin_Click"  Height="30px" Width="75px"/><br />
                                    <asp:LinkButton ID="lbtnForgotPassword" runat="server" Text="Forgot Password?" OnClick="lbtnForgotPassword_Click"></asp:LinkButton>
                                    <asp:ValidationSummary ID="vsLogin" runat="server" ValidationGroup="vg" ShowMessageBox="true"
                                        ShowSummary="false" DisplayMode="BulletList" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div style="background-color: #5D7B9D; width: 95%; height: 20px; text-align: center;
            -moz-border-radius: 5px; border-radius: 5px; vertical-align: middle; line-height: 1.5em;
            margin-top: 225px;">
            <div style="background-color: #5D7B9D; -moz-border-radius: 5px; border-radius: 5px;">
                <div style="background-color: #5D7B9D; -moz-border-radius: 5px; border-radius: 5px;">
                    <%--<span style="color: WhiteSmoke;">Copyright © 2013 </span>--%>
                    <span style="color: WhiteSmoke;">Copyright &copy; <%= DateTime.Now.Year %> </span><a href="http://www.mangalaminfotech.com"
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
