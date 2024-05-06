<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true"
    CodeFile="PriceType.aspx.cs" Inherits="PriceType" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <center>
                <div id="DivHeading">
                    Price Type
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
                        <asp:GridView ID="gridPriceType" runat="server" AutoGenerateColumns="False" Width="100%"
                            ShowFooter="true" OnRowCommand="gridPriceType_RowCommand" OnRowDeleting="gridPriceType_RowDeleting"
                            OnRowEditing="gridPriceType_RowEditing" OnRowCancelingEdit="gridPriceType_RowCancelingEdit"
                            OnRowUpdating="gridPriceType_RowUpdating" AllowPaging="True" OnPageIndexChanging="gridPriceType_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSr" runat="server" Text='<%#Container.DataItemIndex + 1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Price Type Id" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPriceTypeId" runat="server" Text='<%#Eval("PriceTypeId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Price Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPriceType" runat="server" Text='<%#Eval("PriceType") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEdtPriceType" runat="server" MaxLength="50" Text='<%#Eval("PriceType") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rqtxtEdtPriceType" runat="server" Text="*" ControlToValidate="txtEdtPriceType"
                                            ToolTip="Required" ErrorMessage="Please enter price type" SetFocusOnError="true"
                                            ValidationGroup="vgu"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtPriceType" runat="server" MaxLength="50"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rqtxtPriceType" runat="server" Text="*" ControlToValidate="txtPriceType"
                                            ToolTip="Required" ErrorMessage="Please enter price type" SetFocusOnError="true"
                                            ValidationGroup="vg"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit | Delete">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgEdit" runat="server" ImageUrl="~/images/editing.png" Height="20px"
                                            Width="20px" ToolTip="Edit" CommandName="Edit" />
                                        &nbsp|&nbsp
                                        <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="~/images/delete.png" Height="20px"
                                            Width="20px" ToolTip="Delete" CommandName="Delete" AlternateText="Delete" OnClientClick="return confirm('Do you want to delete this country?');" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton ID="imgUpdate" runat="server" AlternateText="Update" CommandName="Update"
                                            ToolTip="Update" ImageUrl="~/images/update.png" ValidationGroup="vgu" Height="20px"
                                            Width="20px" />
                                        <asp:ValidationSummary ID="vsUpdate" runat="server" ValidationGroup="vgu" HeaderText="MITL Invoice"
                                            ShowMessageBox="true" ShowSummary="false" DisplayMode="BulletList" />
                                        &nbsp;|&nbsp;<asp:ImageButton ID="imgCancel" runat="server" AlternateText="Cancel"
                                            CommandName="Cancel" ToolTip="Cancel Update" ImageUrl="~/images/Cancel.png" Height="20px"
                                            Width="20px" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:ImageButton ID="imdAdd" runat="server" ImageUrl="~/images/Add.png" Height="20px"
                                            Width="20px" ToolTip="Add" CommandName="Insert" ValidationGroup="vg" />
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
