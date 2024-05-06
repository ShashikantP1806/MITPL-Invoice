<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Company.aspx.cs" Inherits="Company" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>
        $(function () {
            $(document).tooltip({
                position: {
                    my: "center bottom-20",
                    at: "center top",
                    using: function (position, feedback) {
                        $(this).css(position);
                        $("<div>")
            .addClass("arrow")
            .addClass(feedback.vertical)
            .addClass(feedback.horizontal)
            .appendTo(this);
                    }
                }
            });
        });
    </script>

    <style type="text/css">
        .ModalPopupBG {
            background-color: Gray;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:MultiView ID="mvCompany" runat="server">
        <asp:View ID="viewManageCompany" runat="server">
            <center>
                <div id="DivHeading">
                    Manage Company
                </div>
            </center>
            <table width="100%">
                <tr>
                    <td align="left">
                        <asp:Button ID="btnNewCompany" runat="server" Text="New Company" ToolTip="Add new company"
                            OnClick="btnNewCompany_Click" />
                        <asp:Label ID="lblError2" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                    </td>
                    <td align="right">Department :&nbsp;<asp:DropDownList ID="ddlDepartmentSearch" runat="server" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlDepartmentSearch_SelectedIndexChanged">
                    </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="gridCompany" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            OnPageIndexChanging="gridCompany_PageIndexChanging" OnRowCommand="gridCompany_RowCommand"
                            OnRowDeleting="gridCompany_RowDeleting" ShowFooter="false" Width="100%" OnRowDataBound="gridCompany_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSr" runat="server" Text="<%#Container.DataItemIndex + 1 %>"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Company ID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCompanyID" runat="server" Text='<%#Eval("CompanyID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Company Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCompanyName1" runat="server" Text='<%#HighlightText(Eval("CompanyName").ToString()) %>'></asp:Label>
                                        <asp:Label ID="lblCompanyName" runat="server" Text='<%#Eval("CompanyName") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                    <ControlStyle Width="120px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Department Id" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDepartmentId" runat="server" Text='<%#Eval("DepartmentId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Department">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDepartment" runat="server" Text='<%#Eval("Department") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ControlStyle Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Phone">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPhone" runat="server" Text='<%#Eval("Phone") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ControlStyle Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Address">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAddress" runat="server" Text='<%#Eval("Address") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ControlStyle Width="220px" />
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="Website">
                                    <ItemTemplate>
                                        <asp:Label ID="lblWebsite" runat="server" Text='<%#Eval("Website") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ControlStyle Width="150px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Currency">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCurrency" runat="server" Text='<%#Eval("Currency") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ControlStyle Width="100px" /> 
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Contact">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtnAddIndividualContact" runat="server" Text="Add/View" OnClick="lbtnAddIndividualContact_Click"></asp:LinkButton>
                                    </ItemTemplate>
                                   
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit | Delete">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgEdit" runat="server" Height="20px" ImageUrl="~/images/editing.png"
                                            OnClick="imgEdit_Click" ToolTip="Edit" Width="20px" />
                                        <asp:Label ID="lbl1" runat="server" Text=" | "></asp:Label>
                                        <asp:ImageButton ID="imgDelete" runat="server" AlternateText="Delete" CommandName="Delete"
                                            Height="20px" ImageUrl="~/images/delete.png" OnClientClick="return confirm('Do you want to delete this country?');"
                                            ToolTip="Delete" Width="20px" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton ID="imgUpdate" runat="server" AlternateText="Update" CommandName="Update"
                                            Height="20px" ImageUrl="~/images/update.png" ToolTip="Update" Width="20px" />
                                        &nbsp;|&nbsp;<asp:ImageButton ID="imgCancel" runat="server" AlternateText="Cancel"
                                            CommandName="Cancel" Height="20px" ImageUrl="~/images/Cancel.png" ToolTip="Cancel Update"
                                            Width="20px" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="viewAddCompany" runat="server">
            <center>
                <div id="DivHeading">
                    Add Company
                </div>
            </center>
            <center>
                <table width="80%">
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="lblError1" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Company Name :
                                    <asp:Label ID="lblrqName" runat="server" Text="*" ForeColor="Blue"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCompanyName" runat="server" MaxLength="600" ToolTip="Company Name"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rqCompanyName" runat="server" Text="*" ControlToValidate="txtCompanyName"
                                ToolTip="Required" ErrorMessage="Pleae enter company name" SetFocusOnError="true"
                                ValidationGroup="vg"></asp:RequiredFieldValidator>
                        </td>
                        <td></td>
                    </tr>

                    <tr>
                        <td>Company Phone :
                                    <asp:Label ID="lblrqCompanyPhone" runat="server" Text="*" ForeColor="Blue"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPhone" runat="server" MaxLength="15" ToolTip="Phone number"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rqPhone" runat="server" Text="*" ControlToValidate="txtPhone"
                                ToolTip="Required" ErrorMessage="Please enter phone number" SetFocusOnError="true"
                                ValidationGroup="vg"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revPhone" runat="server" ControlToValidate="txtPhone"
                                ErrorMessage="Telephone number must be 7-15 digit" ValidationExpression="^[0-9]{7,15}$"
                                Text="*" ValidationGroup="vg" SetFocusOnError="true" ToolTip="Telephone number must be 7-15 digit"></asp:RegularExpressionValidator>
                        </td>
                        <td></td>

                    </tr>
                    <tr>
                         <td>Department :
                                    <asp:Label ID="lblrqDepartment" runat="server" Text="*" ForeColor="Blue"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDepartment" runat="server" ToolTip="Select department">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rqDepartment" runat="server" Text="*" ControlToValidate="ddlDepartment"
                                ErrorMessage="Please select department" SetFocusOnError="true" InitialValue="-- Select --"
                                ToolTip="Required" ValidationGroup="vg"></asp:RequiredFieldValidator>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>Currency :
                                    <asp:Label ID="lblrqCurrency" runat="server" Text="*" ForeColor="Blue"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCurrency" runat="server">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rqCurrency" runat="server" Text="*" ControlToValidate="ddlCurrency"
                                ErrorMessage="Please select currency" SetFocusOnError="true" InitialValue="-- Select --"
                                ToolTip="Required" ValidationGroup="vg"></asp:RequiredFieldValidator>
                        </td>
                        <td></td>

                    </tr>

                    <tr>
                        <td>Address :
                                    <asp:Label ID="lblrqAddress" runat="server" Text="*" ForeColor="Blue"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" MaxLength="250"
                                Height="50" Width="200" ToolTip="First address"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rqAddress" runat="server" Text="*" ToolTip="Required"
                                ControlToValidate="txtAddress" ErrorMessage="Please enter company's address"
                                SetFocusOnError="true" ValidationGroup="vg"></asp:RequiredFieldValidator>
                        </td>
                        <td></td>

                    </tr>

                    <tr>
                        <td>Website :
                        </td>
                        <td>
                            <asp:TextBox ID="txtWebsite" runat="server" MaxLength="250" ToolTip="Website URL"></asp:TextBox>
                        </td>
                        <td>&nbsp;
                        </td>

                    </tr>
                    <tr>
                        <td>Remarks :
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Height="60" Width="300"
                                ToolTip="Remarks"></asp:TextBox>
                        </td>
                        <td></td>

                    </tr>
                     <tr>
                        <td><asp:Label ID="lblClient_M1" runat="server" Text="M1 :"></asp:Label>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkClient_M1" runat="server" />
                        </td>
                        <td></td>

                    </tr>
                     <tr>
                        <td><asp:Label ID="lblClient_M2" runat="server" Text="M2 :"></asp:Label>
                        </td>
                        <td>
                             <asp:CheckBox ID="chkClient_M2" runat="server" />
                        </td>
                        <td></td>

                    </tr>


                    <tr>
                        <td colspan="4">
                            <asp:Label ID="Label1" runat="server" Text="*" ForeColor="Blue" Font-Bold="true"></asp:Label>
                            <b>Field should be required</b></td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" ValidationGroup="vg" />
                            &nbsp&nbsp
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                            <asp:ValidationSummary ID="vs" runat="server" ValidationGroup="vg" DisplayMode="BulletList"
                                HeaderText="MITPLInvoice" ShowSummary="false" ShowMessageBox="false" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;
                        </td>
                    </tr>
                </table>
            </center>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <Triggers>
                  <%--  <asp:AsyncPostBackTrigger ControlID="lbtnAddState" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="lbtnAddCity" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="lbtnAddCountry" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="lbtnAddCity2" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="lbtnAddState2" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="lbtnAddCountry2" EventName="Click" />--%>
                </Triggers>
                <ContentTemplate>
                    <asp:Button ID="btnShowPopup" runat="server" Text="fdfdfsfd" Style="display: none" />
                    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" PopupControlID="pnlpopup"
                        TargetControlID="btnShowPopup" BackgroundCssClass="ModalPopupBG">
                    </asp:ModalPopupExtender>
                    <asp:Panel ID="pnlpopup" runat="server" CssClass="modalBackground" Style="display: none;">


                        <table width="100%">
                            <tr>
                                <td colspan="2" align="center">
                                    <h3>
                                        <asp:Label ID="lblTitle" runat="server" ForeColor="#0D6895"></asp:Label>
                                    </h3>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="left">&nbsp;&nbsp;<asp:Label ID="lblName" runat="server" ForeColor="Black"></asp:Label>
                                </td>
                                <td align="left">&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;<asp:TextBox ID="txtName" runat="server" Width="200px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rqName" runat="server" ControlToValidate="txtName"
                                        ErrorMessage="Enter value" Text="*" ValidationGroup="rqName" SetFocusOnError="true"
                                        ToolTip="Required"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Button ID="btnOk" runat="server" OnClick="btnOk_Click" Text="Save" ValidationGroup="rqName" />
                                    <asp:Button ID="btnMPCancel" runat="server" Text="Cancel" />
                                </td>
                            </tr>
                        </table>

                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder4" runat="Server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ContentPlaceHolder5" runat="Server">
</asp:Content>

