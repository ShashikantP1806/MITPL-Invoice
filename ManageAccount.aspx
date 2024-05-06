<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="ManageAccount.aspx.cs" Inherits="ManageAccount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script language="javascript" type="text/javascript">
        function NumericOnly(objEvt) {
            var charCode = (objEvt.which) ? objEvt.which : event.keyCode
            if ((charCode >= 48 && charCode <= 57)) {
                return true;
            }
            else {
                alert("Only numeric values are allowed.");
                return false;
            }
        }

    </script>
    <style type="text/css">
        .HtmlTable1 {
            font-family: Calibri, Times, serif;
            font-size: 18px;
            height: 50px;
            border: 1px solid #4f4f4f;
            border-radius: 10px;
            padding: 5px 5px 5px 5px;
            line-height: 1.6em;
            box-shadow: 0px 5px 18px #4f4f4f;
            width: 55%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <div align="center">
        
        <asp:UpdatePanel ID="up" runat="server">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="chkShow" />
                <asp:PostBackTrigger ControlID="btnUpdate"/>
            </Triggers>
            <ContentTemplate>

                <table class="HtmlTable1" border="0">
                    <tr>
                        <th colspan="2" align="center">
                            <h3>
                                <b>
                                    <asp:Label ID="lblTitle" runat="server" Text="Email Configuration" ForeColor="#0D6895"></asp:Label></b>
                            </h3>
                        </th>
                    </tr>
                    <tr>
                        <td align="right" style="width: 30%;">&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="Host :" ForeColor="Black"></asp:Label>&nbsp;&nbsp;
                        </td>
                        <td align="left">&nbsp;&nbsp;<asp:TextBox ID="txtHost" runat="server" Width="200px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtHost"
                                ErrorMessage="Enter value" Text="*" ValidationGroup="rqName" SetFocusOnError="true"
                                ToolTip="Required"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">&nbsp;&nbsp;<asp:Label ID="Label2" runat="server" Text="PORT :" ForeColor="Black"></asp:Label>&nbsp;&nbsp;
                        </td>
                        <td align="left">&nbsp;&nbsp;<asp:TextBox ID="txtPORT" runat="server" Width="200px" MaxLength="3" OnKeyPress="return NumericOnly(event);"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPORT"
                                ErrorMessage="Enter value" Text="*" ValidationGroup="rqName" SetFocusOnError="true"
                                ToolTip="Required"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">&nbsp;&nbsp;<asp:Label ID="Label3" runat="server" Text="HostUserName :" ForeColor="Black"></asp:Label>&nbsp;&nbsp;
                        </td>
                        <td align="left">&nbsp;&nbsp;<asp:TextBox ID="txtHostUserName" runat="server" Width="200px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtHostUserName"
                                ErrorMessage="Enter value" Text="*" ValidationGroup="rqName" SetFocusOnError="true"
                                ToolTip="Required"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revDefaultRecipient" runat="server" ControlToValidate="txtHostUserName"
                                Text="*" ErrorMessage="Please enter valid e-mail" ToolTip="Invalid email" ValidationGroup="rqName"
                                ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" SetFocusOnError="True"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">&nbsp;&nbsp;<asp:Label ID="lblName" runat="server" Text="HostPassword :" ForeColor="Black"></asp:Label>&nbsp;&nbsp;
                        </td>
                        <td align="left">&nbsp;&nbsp;<asp:TextBox ID="txtHostPassword" runat="server" Width="200px" TextMode="Password"></asp:TextBox>

                             <asp:RequiredFieldValidator ID="rqName" runat="server" ControlToValidate="txtHostPassword"
                                ErrorMessage="Enter value" Text="*" ValidationGroup="rqName" SetFocusOnError="true"
                                ToolTip="Required"></asp:RequiredFieldValidator>

                            <asp:CheckBox ID="chkShow" AutoPostBack="true" runat="server" Checked="false" Text="Show" OnCheckedChanged="chkShow_CheckedChanged" />

                           
                        </td>
                    </tr>
                    <tr style="line-height: 0.6em;">
                        <td colspan="2">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td align="left">&nbsp;&nbsp;<asp:Button ID="btnUpdate" runat="server" Text="Update" ValidationGroup="rqName" OnClick="btnUpdate_Click" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder4" runat="Server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ContentPlaceHolder5" runat="Server">
</asp:Content>

