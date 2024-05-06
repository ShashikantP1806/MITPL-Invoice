<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true"
    CodeFile="Department.aspx.cs" Inherits="Department" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <center>
                <div id="DivHeading">
                    Manage Department
                </div>
            </center>
            <table width="100%">
                <tr>
                    <td>
                        <asp:Label ID="lblError" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gridDepartment" runat="server" AutoGenerateColumns="False" Width="100%"
                            ShowFooter="true" OnRowCommand="gridDepartment_RowCommand" OnRowDeleting="gridDepartment_RowDeleting"
                            OnRowEditing="gridDepartment_RowEditing" OnRowCancelingEdit="gridDepartment_RowCancelingEdit"
                            OnRowUpdating="gridDepartment_RowUpdating" AllowPaging="True" OnPageIndexChanging="gridDepartment_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSr" runat="server" Text='<%#Container.DataItemIndex + 1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Department Id" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDepartmentId" runat="server" Text='<%#Eval("DepartmentId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Department Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDepartmentName1" runat="server" Text='<%#HighlightText(Eval("DepartmentName").ToString()) %>'></asp:Label>
                                        <asp:Label ID="lblDepartmentName" runat="server" Text='<%#Eval("DepartmentName") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDepartmentEdtName" runat="server" MaxLength="30" Text='<%#Eval("DepartmentName") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rqtxtDepartmentEdtName" runat="server" Text="*" ControlToValidate="txtDepartmentEdtName"
                                            ToolTip="Required" ErrorMessage="Please enter department name" SetFocusOnError="true"
                                            ValidationGroup="vgu"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtDepartmentName" runat="server" MaxLength="30"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rqtxtDepartmentName" runat="server" Text="*" ControlToValidate="txtDepartmentName"
                                            ToolTip="Required" ErrorMessage="Please enter department name" SetFocusOnError="true"
                                            ValidationGroup="vg"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Department Head">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDepartmentHOD1" runat="server" Text='<%#HighlightText(Eval("DepartmentHOD").ToString()) %>'></asp:Label>
                                        <asp:Label ID="lblDepartmentHOD" runat="server" Text='<%#Eval("DepartmentHOD") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlEdtUser" runat="server">
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlUser" runat="server">
                                        </asp:DropDownList>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit | Delete">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgEdit" runat="server" ImageUrl="~/images/editing.png" Height="25px"
                                            Width="25px" ToolTip="Edit" CommandName="Edit" />
                                        &nbsp|&nbsp
                                        <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="~/images/delete.png" Height="25px"
                                            Width="25px" ToolTip="Delete" CommandName="Delete" AlternateText="Delete" OnClientClick="return confirm('Do you want to delete this country?');" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton ID="imgUpdate" runat="server" AlternateText="Update" CommandName="Update"
                                            ToolTip="Update" ImageUrl="~/images/update.png" ValidationGroup="vgu" Height="25px"
                                            Width="25px" />
                                        <asp:ValidationSummary ID="vsUpdate" runat="server" ValidationGroup="vgu" HeaderText="MITL Invoice"
                                            ShowMessageBox="true" ShowSummary="false" DisplayMode="BulletList" />
                                        &nbsp;|&nbsp;<asp:ImageButton ID="imgCancel" runat="server" AlternateText="Cancel"
                                            CommandName="Cancel" ToolTip="Cancel Update" ImageUrl="~/images/Cancel.png" Height="25px"
                                            Width="25px" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:ImageButton ID="imdAdd" runat="server" ImageUrl="~/images/Add.png" Height="25px"
                                            Width="25px" ToolTip="Add" ValidationGroup="vg" CommandName="Insert" />
                                        <asp:ValidationSummary ID="vsAdd" runat="server" ValidationGroup="vg" HeaderText="MITL Invoice"
                                            ShowMessageBox="true" ShowSummary="false" DisplayMode="BulletList" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder4" runat="Server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ContentPlaceHolder5" runat="Server">
</asp:Content>
