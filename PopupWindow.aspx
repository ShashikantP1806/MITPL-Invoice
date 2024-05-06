<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PopupWindow.aspx.cs" Inherits="PopupWindow" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MITPL e-Invoice System</title>
    <link id="Link1" runat="server" rel="shortcut icon" href="images/NewLogo.ico" type="image/x-icon" />
    <link rel="stylesheet" href="css/style.css" type="text/css" media="all" />

    <script src="js/jquery-1.7.2.min.js" type="text/javascript"></script>

    <script src="js/functions.js" type="text/javascript"></script>

    <script type="text/javascript">
        function postValueRecp(postVal) {
            if (postVal != "") {
                opener.document.form1.txtTo.value = opener.document.form1.txtTo.value == "" ? postVal : opener.document.form1.txtTo.value + ";" + postVal;
            }
            var length = opener.document.form1.txtTo.value.length;
            opener.document.form1.txtTo.setSelectionRange(length, length);
            opener.document.form1.txtTo.focus();
            self.close();
        }

        function postValueCC(postVal) {
            if (postVal != "") {
                opener.document.form1.txtCC.value = opener.document.form1.txtCC.value == "" ? postVal : opener.document.form1.txtCC.value + ";" + postVal;
            }
            var length = opener.document.form1.txtCC.value.length;
            opener.document.form1.txtCC.setSelectionRange(length, length);
            opener.document.form1.txtCC.focus();
            self.close();
        }

        function postValueBCC(postVal) {
            if (postVal != "") {
                opener.document.form1.txtBCC.value = opener.document.form1.txtBCC.value == "" ? postVal : opener.document.form1.txtBCC.value + ";" + postVal;
            }
            var length = opener.document.form1.txtBCC.value.length;
            opener.document.form1.txtBCC.setSelectionRange(length, length);
            opener.document.form1.txtBCC.focus();
            self.close();
        }

        function displayMsg(msgVal) {
            alert(msgVal);
        }
    </script>

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
                            Select User -
                            <asp:Label ID="lblUserType" runat="server"></asp:Label>
                        </div>
                    </center>
                    <div>
                        <asp:HiddenField ID="hfSelected" runat="server" Value="" />
                        <asp:Button ID="btnSelect" runat="server" Text="Select" OnClick="btnSelect_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                    </div>
                    <br />
                    <div>
                        <asp:GridView ID="grdClient" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                            PageSize="15" OnPageIndexChanging="grdClient_PageIndexChanging" OnRowDataBound="grdClient_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="30px">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkClient" runat="server" OnCheckedChanged="chkClient_CheckedChanged" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="30px"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sr. No." HeaderStyle-Width="50px">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                    <HeaderStyle Width="50px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" Text='<%#Eval("ID")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Client Name" HeaderStyle-Width="220px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblClientName" runat="server" Text='<%#Eval("ClientName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="220px"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Email">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmail" runat="server" Text='<%#Eval("ClientEmail") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <EmptyDataTemplate>
                                <asp:Label ID="lblEmpRow" runat="server" Text="No records found"></asp:Label>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
        <!-- end of shell -->
    </div>
    </form>
</body>
</html>
