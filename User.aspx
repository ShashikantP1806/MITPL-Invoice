<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true"
    CodeFile="User.aspx.cs" Inherits="User" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .ModalPopupBG {
            background-color: Gray;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }

        .paraGraphtext {
            word-wrap: break-word;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <center>
        <div id="DivHeading">
            Manage Users
        </div>
    </center>
    <table width="100%">
        <tr>
            <td>
                <asp:Label ID="lblError" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                <asp:UpdatePanel ID="Up1" runat="server">
                    <ContentTemplate>
                        <div style="overflow: auto;">
                            <asp:GridView ID="gridUser" runat="server" AutoGenerateColumns="False" ShowFooter="true"
                                OnRowCommand="gridUser_RowCommand" OnRowEditing="gridUser_RowEditing" OnRowCancelingEdit="gridUser_RowCancelingEdit"
                                OnRowUpdating="gridUser_RowUpdating" AllowPaging="True" OnPageIndexChanging="gridUser_PageIndexChanging"
                                OnRowDataBound="gridUser_RowDataBound" OnRowDeleting="gridUser_RowDeleting">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr. No." HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSr" runat="server" Text='<%#Container.DataItemIndex + 1 %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="40px" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="User Id" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUserId" runat="server" Text='<%#Eval("UserId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Emp. Id" HeaderStyle-Width="15px" ItemStyle-Width="10%"
                                        ControlStyle-Width="15px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEmpId1" runat="server" Text='<%#HighlightText(Eval("EmpId").ToString()) %>'></asp:Label>
                                            <asp:Label ID="lblEmpId" runat="server" Text='<%#Eval("EmpId")%>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEdtEmpId" runat="server" Text='<%#Eval("EmpId") %>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rqEEmpId" runat="server" ControlToValidate="txtEdtEmpId"
                                                Font-Size="Small" SetFocusOnError="true" Text="*" ErrorMessage="Please enter employee id"
                                                ToolTip="Required" ValidationGroup="vge"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="revtxtEdtEmpId" runat="server" ControlToValidate="txtEdtEmpId"
                                                Text="*" ErrorMessage="*" ValidationGroup="vge" SetFocusOnError="true" ValidationExpression="^[0-9]*$"></asp:RegularExpressionValidator>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtEmpId" runat="server" Width="50%"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rqEmpId" runat="server" ControlToValidate="txtEmpId"
                                                Font-Size="Small" SetFocusOnError="true" Text="*" ErrorMessage="Please enter employee id"
                                                ToolTip="Required" ValidationGroup="vg"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="revtxtEmpId" runat="server" ControlToValidate="txtEmpId"
                                                Text="*" ErrorMessage="Employee-id should be number" ValidationGroup="vg" SetFocusOnError="true"
                                                ValidationExpression="^[0-9]*$"></asp:RegularExpressionValidator>
                                        </FooterTemplate>
                                        <ControlStyle Width="45px" />
                                        <HeaderStyle Width="15px" />
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="First Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFirstName1" runat="server" Text='<%#HighlightText(Eval("FirstName").ToString()) %>'></asp:Label>
                                            <asp:Label ID="lblFirstName" runat="server" Text='<%#Eval("FirstName")%>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEdtFirstName" runat="server" MaxLength="20" Text='<%#Eval("FirstName") %>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rqEFName" runat="server" ControlToValidate="txtEdtFirstName"
                                                Font-Size="Small" SetFocusOnError="true" Text="*" ErrorMessage="Please enter firstname"
                                                ToolTip="Required" ValidationGroup="vge"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFirstName" runat="server" MaxLength="20" Width="80px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rqFName" runat="server" ControlToValidate="txtFirstName"
                                                Font-Size="Small" SetFocusOnError="true" Text="*" ErrorMessage="Please enter firstname"
                                                ToolTip="Required" ValidationGroup="vg"></asp:RequiredFieldValidator>
                                        </FooterTemplate>
                                        <ControlStyle Width="100px" />
                                        <HeaderStyle Width="100px" />
                                        <FooterStyle Width="100px" />
                                        <ItemStyle Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Middle Name" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMiddleName" runat="server" Text='<%#Eval("MiddleName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEdtMiddleName" runat="server" MaxLength="20" Text='<%#Eval("MiddleName") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtMiddleName" runat="server" MaxLength="20"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Last Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLastName1" runat="server" Text='<%#HighlightText(Eval("LastName").ToString()) %>'></asp:Label>
                                            <asp:Label ID="lblLastName" runat="server" Text='<%#Eval("LastName") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEdtLastName" runat="server" MaxLength="20" Text='<%#Eval("LastName") %>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rqELName" runat="server" ControlToValidate="txtEdtLastName"
                                                Font-Size="Small" SetFocusOnError="true" Text="*" ErrorMessage="Please enter lastname"
                                                ToolTip="Required" ValidationGroup="vge"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtLastName" runat="server" MaxLength="20" AutoPostBack="true" OnTextChanged="txtLastName_TextChanged"
                                                Width="80px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rqLName" runat="server" ControlToValidate="txtLastName"
                                                Font-Size="Small" SetFocusOnError="true" Text="*" ErrorMessage="Please enter lastname"
                                                ToolTip="Required" ValidationGroup="vg"></asp:RequiredFieldValidator>
                                        </FooterTemplate>
                                        <ControlStyle Width="100px" />
                                        <HeaderStyle Width="100px" />
                                        <FooterStyle Width="100px" />
                                        <ItemStyle Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="User Name" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUserName" runat="server" Text='<%#Eval("UserName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtUserName" runat="server" MaxLength="20" Enabled="false"></asp:TextBox>&nbsp
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Department">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDepartment1" runat="server" Text='<%#HighlightText(Eval("Department").ToString()) %>'></asp:Label>
                                            <asp:Label ID="lblDepartment" runat="server" Text='<%#Eval("Department") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlEdtDepartment" runat="server">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlDepartment" runat="server" Width="100px">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rqDepartment" runat="server" InitialValue="-- Select --"
                                                ErrorMessage="Pease select department" SetFocusOnError="true" Text="*" Font-Size="Small"
                                                ToolTip="Required" ValidationGroup="vg" ControlToValidate="ddlDepartment"></asp:RequiredFieldValidator>
                                        </FooterTemplate>
                                        <ControlStyle Width="115px" />
                                        <HeaderStyle Width="115px" />
                                        <FooterStyle Width="115px" />
                                        <ItemStyle Width="115px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="E-Mail">
                                        <ItemTemplate>
                                            <div class="paraGraphtext">
                                                <asp:Label ID="lblEmail" runat="server" Text='<%#Eval("Email") %>'></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEdtEmail" runat="server" MaxLength="150" Text='<%#Eval("Email") %>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rqEEmail" runat="server" ControlToValidate="txtEdtEmail"
                                                Font-Size="Small" SetFocusOnError="true" Text="*" ErrorMessage="Please enter e-mail"
                                                ToolTip="Required" ValidationGroup="vge"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="reEEmail" runat="server" ControlToValidate="txtEdtEmail"
                                                Text="*" ErrorMessage="Please enter valid e-mail" ToolTip="Required" ValidationGroup="vge"
                                                Font-Size="Small" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                SetFocusOnError="True"></asp:RegularExpressionValidator>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtEmail" runat="server" MaxLength="150" Width="200px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rqEmail" runat="server" ControlToValidate="txtEmail"
                                                Font-Size="Small" SetFocusOnError="true" Text="*" ErrorMessage="Please enter e-mail"
                                                ToolTip="Required" ValidationGroup="vg"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="reEmail" runat="server" ControlToValidate="txtEmail"
                                                Text="*" ErrorMessage="Please enter valid e-mail" ValidationGroup="vg" Font-Size="Small"
                                                ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" SetFocusOnError="True"></asp:RegularExpressionValidator>
                                        </FooterTemplate>
                                        <ControlStyle Width="225px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="User Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUserType" runat="server" Text='<%#Eval("UserType") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlEdtUserType" runat="server">
                                                <asp:ListItem>ADMINISTRATOR</asp:ListItem>
                                                <asp:ListItem>BUSINESS ASSOCIATES</asp:ListItem>
                                                <asp:ListItem>BUSINESS UNIT MANAGER</asp:ListItem>
                                                <asp:ListItem>CLIENT</asp:ListItem>
                                                <asp:ListItem>DIRECTOR</asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlUserType" runat="server">
                                                <asp:ListItem>ADMINISTRATOR</asp:ListItem>
                                                <asp:ListItem>BUSINESS ASSOCIATES</asp:ListItem>
                                                <asp:ListItem Selected="True">BUSINESS UNIT MANAGER</asp:ListItem>
                                                <asp:ListItem>CLIENT</asp:ListItem>
                                                <asp:ListItem>DIRECTOR</asp:ListItem>
                                            </asp:DropDownList>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="User M1" Visible="true" ItemStyle-HorizontalAlign="Center" >
                                        <ItemTemplate>
                                           <%-- <asp:Label ID="lblUsrM1" runat="server" Text='<%#Eval("U_M1").ToString().Equals("True") ? "Active" : "Inactive" %>'></asp:Label>--%>
                                             <asp:Label ID="lblUsrM1" runat="server" Text='<%#Eval("U_M1").ToString()%>' Width="50px"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="chkEditM1_IsActive" runat="server" Checked='<%# Convert.ToBoolean(Eval("U_M1"))%>' />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:CheckBox ID="chkM1_IsActive" runat="server" Checked="true"  />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="User M2" Visible="true" ItemStyle-HorizontalAlign="Center" >
                                        <ItemTemplate>
                                            <%--<asp:Label ID="lblUsrM2" runat="server" Text='<%#Eval("U_M2").ToString().Equals("True") ? "Active" : "Inactive" %>'></asp:Label>--%>
                                            <asp:Label ID="lblUsrM2" runat="server" Text='<%#Eval("U_M2").ToString()%>' Width="50px"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="chkEditM2_IsActive" runat="server" Checked='<%# Convert.ToBoolean(Eval("U_M2"))%>' />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:CheckBox ID="chkM2_IsActive" runat="server" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="User Sent Email" Visible="true" ItemStyle-HorizontalAlign="Center" >
                                        <ItemTemplate>
                                            <%--<asp:Label ID="lblUsrM2" runat="server" Text='<%#Eval("U_M2").ToString().Equals("True") ? "Active" : "Inactive" %>'></asp:Label>--%>
                                            <asp:Label ID="lblUserSentEmail" runat="server" Text='<%#Eval("UserSentEmail").ToString()%>' Width="50px"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="chkEditIsUserSentEmail" runat="server" Checked='<%# Convert.ToBoolean(Eval("UserSentEmail"))%>' />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:CheckBox ID="chkIsUserSentEmail" runat="server" />
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="User Proforma Approve Access" Visible="true" ItemStyle-HorizontalAlign="Center" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblUserProformaApproveAccess" runat="server" Text='<%#Eval("ProformaApproveAccess").ToString()%>' Width="50px"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="chkEditIsProformaApproveAccess" runat="server" Checked='<%# Convert.ToBoolean(Eval("ProformaApproveAccess"))%>' />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:CheckBox ID="chkIsProformaApproveAccess" runat="server" />
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgEdit" runat="server" ImageUrl="~/images/editing.png" Height="20px"
                                                Width="20px" ToolTip="Edit" CommandName="Edit" />
                                            <%--<asp:ImageButton ID="imgDelete" runat="server" ImageUrl="~/images/delete.png"
                                                Height="20px" Width="20px" ToolTip="Delete" CommandName="Delete" AlternateText="Delete"
                                                OnClientClick="return confirm('Do you want to delete this user?');" />--%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:ImageButton ID="imgUpdate" runat="server" AlternateText="Update" CommandName="Update"
                                                ToolTip="Update" ImageUrl="~/images/update.png" Height="20px" Width="20px" OnClick="imgUpdate_Click"
                                                ValidationGroup="vge" />
                                            <asp:ValidationSummary ID="vsEUser" runat="server" DisplayMode="BulletList" HeaderText="MITPL_Invoice"
                                                ValidationGroup="vge" ShowSummary="false" ShowMessageBox="true" />
                                            &nbsp;|&nbsp;<asp:ImageButton ID="imgCancel" runat="server" AlternateText="Cancel"
                                                CommandName="Cancel" ToolTip="Cancel Update" ImageUrl="~/images/Cancel.png" Height="20px"
                                                Width="20px" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton ID="imdAdd" runat="server" ImageUrl="~/images/Add.png" Height="20px"
                                                Width="20px" ToolTip="Add" CommandName="Insert" OnClick="imdAdd_Click" ValidationGroup="vg" />
                                            <asp:ValidationSummary ID="vsUser" runat="server" DisplayMode="BulletList" ValidationGroup="vg"
                                                ShowSummary="false" ShowMessageBox="true" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkbtnStatus" runat="server" Text='<%#Eval("ActiveInactive") %>'
                                                CommandName="Delete"></asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblAct" runat="server" Text='<%#Eval("ActiveInactive") %>'></asp:Label>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle Wrap="true" />
                                <AlternatingRowStyle Wrap="true" />
                                <EditRowStyle Width="150px" />
                            </asp:GridView>
                            <asp:Button ID="btnShowPopup" runat="server" Text="fdfdfsfd" Style="display: none" />
                            <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" PopupControlID="pnlpopup"
                                TargetControlID="btnShowPopup" BackgroundCssClass="ModalPopupBG">
                            </asp:ModalPopupExtender>
                            <asp:Panel ID="pnlpopup" runat="server" CssClass="modalBackground" Style="display: none">
                                <table width="100%">
                                    <tr>
                                        <td colspan="2" align="center">
                                            <h3>
                                                <asp:Label ID="lblEmailVerify" runat="server" Text="Enter user email again to verify"
                                                    ForeColor="#0D6895"></asp:Label>
                                            </h3>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">&nbsp;&nbsp;<asp:Label ID="lblEmailUser" runat="server" ForeColor="Black" Text="User Email : "></asp:Label>
                                        </td>
                                        <td align="left">&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;<asp:TextBox ID="txtEmailUser" runat="server" Width="200px"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmailUser"
                                                ErrorMessage="Please enter valid email" Text="*" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                ValidationGroup="vgVerify"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <asp:CheckBox ID="chkSendNotify" runat="server" Checked="true" Text="Send Password to User" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:Button ID="btnOk" runat="server" OnClick="btnOk_Click" Text="Save" ValidationGroup="vgVerify"
                                                Width="70px" />
                                            <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel"
                                                Width="70px" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder4" runat="Server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ContentPlaceHolder5" runat="Server">
</asp:Content>
