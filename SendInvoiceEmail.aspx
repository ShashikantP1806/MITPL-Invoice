<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SendInvoiceEmail.aspx.cs"
    Theme="Theme1" Inherits="SendInvoiceEmail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MITPL e-Invoice System</title>
    <link id="Link1" runat="server" rel="shortcut icon" href="images/NewLogo.ico" type="image/x-icon" />
    <link rel="stylesheet" href="css/style.css" type="text/css" media="all" />

    <script src="js/jquery-1.7.2.min.js" type="text/javascript"></script>

    <script src="js/functions.js" type="text/javascript"></script>

    <style type="text/css">
        .opaqueLayer
        {
            display: none;
            position: absolute;
            top: 0px;
            left: 0px;
            opacity: 0.6;
            filter: alpha(opacity=60);
            background-color: #000000;
            z-index: 1000;
        }
        .questionLayer
        {
            position: absolute;
            top: 0px;
            left: 0px;
            width: 350px;
            height: 200px;
            display: none;
            z-index: 1001;
            border: 2px solid black;
            background-color: #FFFFFF;
            text-align: center;
            vertical-align: middle;
            padding: 10px;
        }
    </style>
    <style type="text/css">
        .ajaximage
        {
            /*background-color: #D4D0C8;*/ /*background-color:#F3F5F7;*/
            margin-left: 0px;
            margin-right: 0px;
            font-family: Verdana;
        }
        .ajaximage th
        {
            background-color: #A5B5C5;
            padding: 0px;
        }
        .ajaximage td
        {
            background-color: White;
            padding: 0px;
        }
    </style>

    <script type="text/javascript">
        function getBrowserHeight() {
            var intH = 0;
            var intW = 0;


            if (typeof window.innerWidth == 'number') {
                intH = window.innerHeight;
                intW = window.innerWidth;
            }
            else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) {
                intH = document.documentElement.clientHeight;
                intW = document.documentElement.clientWidth;
            }
            else if (document.body && (document.body.clientWidth || document.body.clientHeight)) {
                intH = document.body.clientHeight;
                intW = document.body.clientWidth;
            }


            return { width: parseInt(intW), height: parseInt(intH) };
        }


        function setLayerPosition() {
            var shadow = document.getElementById("shadow");
            var question = document.getElementById("question");


            var bws = getBrowserHeight();
            shadow.style.width = bws.width + "px";
            shadow.style.height = bws.height + "px";


            question.style.left = parseInt((bws.width - 350) / 2);
            question.style.top = parseInt((bws.height - 200) / 2);


            shadow = null;
            question = null;
        }


        function showLayer() {
            setLayerPosition();


            var shadow = document.getElementById("shadow");
            var question = document.getElementById("question");


            shadow.style.display = "block";
            question.style.display = "block";


            shadow = null;
            question = null;
        }


        function hideLayer() {
            var shadow = document.getElementById("shadow");
            var question = document.getElementById("question");


            shadow.style.display = "none";
            question.style.display = "none";


            shadow = null;
            question = null;
        }


        window.onresize = setLayerPosition;
    </script>

    <style>
        .imgbtnStyle
        {
            vertical-align: middle;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
   
            <div id="wrapper">
                <!-- shell -->
                <div class="shell">
                    <!-- container -->
                    <div class="container">
                        <div class="main" style="min-height: 450px;">
                            <br />
                            <center>
                                <div id="DivHeading">
                                    Send Email
                                </div>
                            </center>
                            <asp:ScriptManager ID="scrManager" runat="server">
                            </asp:ScriptManager>
                            <div id="shadow" class="opaqueLayer">
                            </div>
                            <div id="question" class="questionLayer">
                                <br />
                                <br />
                                <br />
                                Please wait...
                                <br />
                                <br />
                                <br />
                            </div>
                            <div>
                                <asp:HiddenField ID="hfRemoveID" runat="server" />
                                <table width="100%">
                                    <tr>
                                        <td style="width: 100px">
                                            From:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblMailFrom" Text="corp@mangalaminfotech.com" runat="server"></asp:Label>
                                            <%--<asp:Label ID="lblmailfromRCM" Text="Test@Test.com" runat="server"></asp:Label>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            To:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTo" runat="server" Width="90%" CssClass="TextBox"></asp:TextBox>
                                            <asp:ImageButton ID="imgAddRecp" runat="server" ImageUrl="~/images/Add.png" AlternateText="Add Recipient"
                                                Height="20px" Width="20px" OnClick="imgAddRecp_Click" ToolTip="Add Recipient"
                                                CssClass="imgbtnStyle" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            CC:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCC" runat="server" Width="90%" CssClass="TextBox"></asp:TextBox>
                                            <asp:ImageButton ID="imgAddCC" runat="server" ImageUrl="~/images/Add.png" AlternateText="Add CC"
                                                Height="20px" Width="20px" OnClick="imgAddCC_Click" ToolTip="Add CC" CssClass="imgbtnStyle" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            BCC:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBCC" runat="server" Width="90%" CssClass="TextBox"></asp:TextBox>
                                            <asp:ImageButton ID="imgAddBCC" runat="server" ImageUrl="~/images/Add.png" AlternateText="Add BCC"
                                                Height="20px" Width="20px" ToolTip="Add BCC" OnClick="imgAddBCC_Click" CssClass="imgbtnStyle" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Subject:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSubject" runat="server" Width="90%" CssClass="TextBox"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trSendInvoice" runat="server">
                                        <td>
                                            Invoice:
                                        </td>
                                        <td>
                                            <%--<iframe id="iframInvoice" runat="server" height="100px" width="100px"></iframe>--%>
                                            <div id="divInvoice" runat="server">
                                            
                                            </div>
                                            <asp:LinkButton ID="lnkInvoice" runat="server" OnClick="lnkInvoice_Click">
                                            </asp:LinkButton>
                                            <%--<asp:ImageButton ID="imgbtnRemove" runat="server" CommandName="Remove" AlternateText="Remove"
                                                Height="10px" Width="10px" ImageUrl="~/images/Remove.ico" OnClick="imgbtnRemove_Click"
                                                OnClientClick="return confirm('Do you want to remove invoice pdf file?');" />--%>
                                        </td>
                                    </tr>
                                    <tr id="trFollowupInvoice" runat="server" visible="false">
                                        <td>
                                            Attachment(s):
                                        </td>
                                        <td>
                                            <div id="divAttachments" runat="server">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Attachment(s):
                                        </td>
                                        <td>
                                            <asp:FileUpload ID="fileUpload1" runat="server" /><br />
                                            <asp:FileUpload ID="fileUpload2" runat="server" /><br />
                                            <asp:FileUpload ID="fileUpload3" runat="server" /><br />
                                            <asp:FileUpload ID="fileUpload4" runat="server" /><br />
                                            <asp:FileUpload ID="fileUpload5" runat="server" /><br />
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <cc2:Editor ID="editorEmail" runat="server" Height="300px"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnSendEmail" runat="server" Text="Send Email" OnClick="btnSendEmail_Click"
                                                OnClientClick="showLayer();" />
                                            
                                            <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" Width="100px" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- end of shell -->
            </div>
      
    </form>
</body>
</html>
