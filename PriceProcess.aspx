<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true"
    CodeFile="PriceProcess.aspx.cs" Inherits="PriceProcess" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <center>
                <div id="DivHeading">
                    Process Master
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
                        <asp:GridView ID="gridProcess" runat="server" AutoGenerateColumns="False" Width="100%"
                            ShowFooter="true" OnRowCommand="gridProcess_RowCommand" OnRowDeleting="gridProcess_RowDeleting"
                            OnRowEditing="gridProcess_RowEditing" OnRowCancelingEdit="gridProcess_RowCancelingEdit"
                            OnRowUpdating="gridProcess_RowUpdating" AllowPaging="True" OnPageIndexChanging="gridProcess_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSr" runat="server" Text='<%#Container.DataItemIndex + 1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Process Id" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProcessId" runat="server" Text='<%#Eval("ProcessId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Process Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProcessName" runat="server" Text='<%#Eval("ProcessName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEdtProcessName" runat="server" MaxLength="50" Text='<%#Eval("ProcessName") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rqProcessName" runat="server" ControlToValidate="txtEdtProcessName"
                                            Text="*" ErrorMessage="Please enter process name" SetFocusOnError="true" ValidationGroup="vgu"
                                            ToolTip="Required"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtProcessName" runat="server" MaxLength="50"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rqProcessName" runat="server" ControlToValidate="txtProcessName"
                                            Text="*" ErrorMessage="Please enter process name" SetFocusOnError="true" ValidationGroup="vg"
                                            ToolTip="Required"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Department">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDepartmentName" runat="server" Text='<%#Eval("DepartmentName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlEdtDepartment" runat="server" DataSourceID="LinqDataSource1"
                                            DataTextField="DepartmentName" DataValueField="DepartmentId">
                                        </asp:DropDownList>
                                        <asp:LinqDataSource ID="LinqDataSource1" runat="server" ContextTypeName="MITInvoiceDataContext"
                                            OrderBy="DepartmentName" Select="new (DepartmentId, DepartmentName)" TableName="DepartmentMasters"
                                            Where="IsActive == @IsActive">
                                            <WhereParameters>
                                                <asp:Parameter DefaultValue="true" Name="IsActive" Type="Boolean" />
                                            </WhereParameters>
                                        </asp:LinqDataSource>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlDepartment" runat="server">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rqDepartment" runat="server" Text="*" ErrorMessage="Please select department"
                                            ControlToValidate="ddlDepartment" SetFocusOnError="true" ValidationGroup="vg"
                                            InitialValue="-- Select --" ToolTip="Required"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit | Delete">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgEdit" runat="server" ImageUrl="~/images/editing.png" Height="20px"
                                            Width="20px" ToolTip="Edit" CommandName="Edit" />
                                        &nbsp|&nbsp
                                        <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="~/images/delete.png" Height="20px"
                                            Width="20px" ToolTip="Delete" CommandName="Delete" AlternateText="Delete" OnClientClick="return confirm('Do you want to delete this process?');" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton ID="imgUpdate" runat="server" AlternateText="Update" CommandName="Update"
                                            ToolTip="Update" ImageUrl="~/images/update.png" Height="20px" Width="20px" ValidationGroup="vgu" />
                                        <asp:ValidationSummary ID="vsUpdate" runat="server" ValidationGroup="vgu" ShowMessageBox="true"
                                            ShowSummary="false" DisplayMode="BulletList" HeaderText="MITPLInvoice" />
                                        &nbsp;|&nbsp;<asp:ImageButton ID="imgCancel" runat="server" AlternateText="Cancel"
                                            CommandName="Cancel" ToolTip="Cancel Update" ImageUrl="~/images/Cancel.png" Height="20px"
                                            Width="20px" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:ImageButton ID="imdAdd" runat="server" ImageUrl="~/images/Add.png" Height="20px"
                                            Width="20px" ToolTip="Add" CommandName="Insert" ValidationGroup="vg" />
                                        <asp:ValidationSummary ID="vsAdd" runat="server" ValidationGroup="vg" ShowMessageBox="true"
                                            ShowSummary="false" DisplayMode="BulletList" HeaderText="MITPLInvoice" />
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
