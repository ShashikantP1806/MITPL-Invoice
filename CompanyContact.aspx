<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="CompanyContact.aspx.cs" Inherits="CompanyContact" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <center>
                <div id="DivHeading">
                    Client Contact
                </div>
            </center>
            <table width="100%">
                <tr>
                    <td>
                        <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />
                        <asp:Label ID="lblError" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gridCompanyContact" runat="server" AutoGenerateColumns="False" Width="100%"
                            ShowFooter="true" OnRowCommand="gridCompanyContact_RowCommand" OnRowDeleting="gridCompanyContact_RowDeleting"
                            OnRowEditing="gridCompanyContact_RowEditing" OnRowCancelingEdit="gridCompanyContact_RowCancelingEdit"
                            OnRowUpdating="gridCompanyContact_RowUpdating" AllowPaging="True" OnPageIndexChanging="gridCompanyContact_PageIndexChanging"
                            OnRowDataBound="gridCompanyContact_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSr" runat="server" Text='<%#Container.DataItemIndex + 1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Individual Contact Id" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIndividualContactId" runat="server" Text='<%#Eval("CompanyContactId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Company Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCompanyName" runat="server" Text='<%#Eval("CompanyName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlEdtCompanyName" runat="server">
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlCompanyName" runat="server">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rqCompanyName" runat="server" ControlToValidate="ddlCompanyName"
                                            InitialValue="-- Select --" SetFocusOnError="true" Text="*" ErrorMessage="Please select company name"
                                            ToolTip="Required" ValidationGroup="vg"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Contact Person">
                                    <ItemTemplate>
                                        <asp:Label ID="lblContactPersonName" runat="server" Text='<%#Eval("ContactPersonName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEdtContactPersonName" runat="server" MaxLength="150" Text='<%#Eval("ContactPersonName") %>' ToolTip="Contact Person Name"></asp:TextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtContactPersonName" runat="server" MaxLength="150" ToolTip="Contact Person Name"></asp:TextBox><br />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Contact Number">
                                    <ItemTemplate>
                                        <asp:Label ID="lblContactNumber" runat="server" Text='<%#Eval("ContactNumber") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEdtContactNumber" runat="server" MaxLength="15" Text='<%#Eval("ContactNumber") %>' ToolTip="Contact Number"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="revEdtMobile" runat="server" ControlToValidate="txtEdtContactNumber"
                                            ErrorMessage="Contact number must be 7-15 digit" ValidationExpression="^[0-9]{7,15}$"
                                            Text="*" ValidationGroup="vgu" SetFocusOnError="true"></asp:RegularExpressionValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtContactNumber" runat="server" MaxLength="15" ToolTip="Contact Number"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="revMobile" runat="server" ControlToValidate="txtContactNumber"
                                            ErrorMessage="Contact number must be 7-15 digit" ValidationExpression="^[0-9]{7,15}$"
                                            Text="*" ValidationGroup="vg" SetFocusOnError="true"></asp:RegularExpressionValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Department Id" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDepartmentId" runat="server" Text='<%#Eval("DepartmentId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="E-Mail">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmail" runat="server" Text='<%#Eval("Email") %>' ></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEdtEmail" runat="server" MaxLength="150" Text='<%#Eval("Email") %>' ToolTip="Email"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rqEdtEmail" runat="server" ControlToValidate="txtEdtEmail"
                                            Text="*" ErrorMessage="Enter e-mail" ToolTip="Required" ValidationGroup="vgu"
                                            SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revEdtEmail" runat="server" ControlToValidate="txtEdtEmail"
                                            Text="*" ErrorMessage="Enter valid e-mail" ValidationGroup="vgu" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                            SetFocusOnError="True"></asp:RegularExpressionValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtEmail" runat="server" MaxLength="150" ToolTip="Email"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rqEmail" runat="server" ControlToValidate="txtEmail"
                                            Text="*" ErrorMessage="Enter e-mail" ToolTip="Required" ValidationGroup="vg"
                                            SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                                            Text="*" ErrorMessage="Enter valid e-mail" ValidationGroup="vg" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                            SetFocusOnError="True"></asp:RegularExpressionValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Contact Remarks">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("Description") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEdtDescription" runat="server" MaxLength="255" Text='<%#Eval("Description") %>'  TextMode="MultiLine" Height="25" Width="300" ToolTip="Description"></asp:TextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtDescription" runat="server" MaxLength="255"  TextMode="MultiLine" Height="25" Width="300" ToolTip="Description"></asp:TextBox><br />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit | Delete">
                                    <ItemTemplate>
                                        &nbsp;<asp:ImageButton ID="imgEdit" runat="server" ImageUrl="~/images/editing.png"
                                            Height="20px" Width="20px" ToolTip="Edit" CommandName="Edit" />
                                        <asp:Label ID="lbl1" runat="server" Text="|"></asp:Label>
                                        <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="~/images/delete.png" Height="20px"
                                            Width="20px" ToolTip="Delete" CommandName="Delete" AlternateText="Delete" OnClientClick="return confirm('Do you want to delete this country?');" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton ID="imgUpdate" runat="server" AlternateText="Update" CommandName="Update"
                                            ToolTip="Update" ImageUrl="~/images/update.png" ValidationGroup="vgu" Height="20px"
                                            Width="20px" />
                                        <asp:ValidationSummary ID="vsUpdate" runat="server" ValidationGroup="vgu" ShowMessageBox="true"
                                            ShowSummary="false" DisplayMode="BulletList" HeaderText="MITL Invoice" />
                                        &nbsp;|&nbsp;<asp:ImageButton ID="imgCancel" runat="server" AlternateText="Cancel"
                                            CommandName="Cancel" ToolTip="Cancel Update" ImageUrl="~/images/Cancel.png" Height="20px"
                                            Width="20px" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:ImageButton ID="imdAdd" runat="server" ImageUrl="~/images/Add.png" Height="20px"
                                            Width="20px" ToolTip="Add" CommandName="Insert" ValidationGroup="vg" />
                                        <asp:ValidationSummary ID="vsAdd" runat="server" ValidationGroup="vg" ShowMessageBox="true"
                                            ShowSummary="false" DisplayMode="BulletList" HeaderText="MITL Invoice" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnBack" />
        </Triggers>
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

