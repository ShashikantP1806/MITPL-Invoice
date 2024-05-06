<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true"
    CodeFile="UserProfile.aspx.cs" Inherits="UserProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
   <%-- <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>--%>
            <center>
                <div id="DivHeading">
                    User Profile
                </div>
            </center>
            <center>
                <table width="60%">
                    <tr>
                        <td>
                            Emp ID :
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmpId" runat="server" Enabled="false"></asp:TextBox>
                        </td>
                        <td>
                            User Name :
                        </td>
                        <td>
                            <asp:TextBox ID="txtUserName" runat="server" Enabled="false" MaxLength="20"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            First Name :
                        </td>
                        <td>
                            <asp:TextBox ID="txtFName" runat="server" Enabled="false" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rqtxtFName" runat="server" ControlToValidate="txtFName"
                                SetFocusOnError="true" Text="*" ErrorMessage="Please enter first name" ToolTip="Required" ValidationGroup="vg"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            User Type :
                        </td>
                        <td>
                            <asp:TextBox ID="txtUserType" runat="server" Enabled="false" MaxLength="25"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Middle Name :
                        </td>
                        <td>
                            <asp:TextBox ID="txtMName" runat="server" Enabled="false" MaxLength="20"></asp:TextBox>
                        </td>
                        <td>
                            Department :
                        </td>
                        <td>
                            <asp:TextBox ID="txtDepartment" runat="server" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Last Name :
                        </td>
                        <td>
                            <asp:TextBox ID="txtLName" runat="server" Enabled="false" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rqtxtLName" runat="server" ControlToValidate="txtLName"
                                SetFocusOnError="true" Text="*" ErrorMessage="Please enter last name" ToolTip="Required" ValidationGroup="vg"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            Designation :
                        </td>
                        <td>
                            <asp:TextBox ID="txtDesignation" runat="server" Enabled="false" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Email :
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmail" runat="server" Enabled="false" MaxLength="150"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rqtxtEmail" runat="server" ControlToValidate="txtEmail"
                                SetFocusOnError="true" Text="*" ErrorMessage="Please enter e-mail" ToolTip="Required" ValidationGroup="vg"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="reEmail" runat="server" ControlToValidate="txtEmail"
                                Text="*" ErrorMessage="Please enter valid e-mail" ValidationGroup="vg" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                SetFocusOnError="True"></asp:RegularExpressionValidator>
                        </td>
                        <td>
                            Contact No. :
                        </td>
                        <td>
                            <asp:TextBox ID="txtContact" runat="server" Enabled="false" MaxLength="13"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="retxtContact" runat="server" ControlToValidate="txtContact"
                                ErrorMessage="Please enter your valid mobile number" ValidationExpression="^\d{10,15}?$"
                                Text="*" ValidationGroup="vg" SetFocusOnError="true"></asp:RegularExpressionValidator><br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Password :
                        </td>
                        <td>
                            <asp:TextBox ID="txtPassword" runat="server" Enabled="false" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rqtxtPassword" runat="server" ControlToValidate="txtPassword"
                                SetFocusOnError="true" Text="*" ErrorMessage="Please enter Password" ToolTip="Required" ValidationGroup="vg"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                            <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:Button ID="btnOk" runat="server" Text="Edit" ToolTip="Edit" OnClick="btnOk_Click"
                                ValidationGroup="vg" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" Visible="false"
                                OnClick="btnCancel_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="lblmsg" runat="server" Text="* Password Length should be minimum 6 characters<br>* Password should not contains 'Mitl' or 'Mitpl'<br>* Password should not contains 'FirstName' or 'LastName'"></asp:Label>
                        </td>
                    </tr>
                </table>
            </center>
        <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder4" runat="Server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ContentPlaceHolder5" runat="Server">
</asp:Content>
