<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true"
    CodeFile="Country.aspx.cs" Inherits="Country" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <center>
                <center>
                    <div id="DivHeading">
                        Manage Country
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
                            <asp:GridView ID="gridCountry" runat="server" AutoGenerateColumns="False" Width="100%"
                                ShowFooter="true" OnRowCommand="gridCountry_RowCommand" OnRowDeleting="gridCountry_RowDeleting"
                                OnRowEditing="gridCountry_RowEditing" OnRowCancelingEdit="gridCountry_RowCancelingEdit"
                                OnRowUpdating="gridCountry_RowUpdating" AllowPaging="True" OnPageIndexChanging="gridCountry_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr. No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSr" runat="server" Text='<%#Container.DataItemIndex + 1 %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Country Id" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCountryId" runat="server" Text='<%#Eval("CountryId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Country Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCountryName1" runat="server" Text='<%#HighlightText(Eval("CountryName").ToString()) %>'></asp:Label>
                                            <asp:Label ID="lblCountryName" runat="server" Text='<%#Eval("CountryName")%>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEdtCountryName" runat="server" MaxLength="50" Text='<%#Eval("CountryName") %>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rqtxtEdtCountryName" runat="server" Text="*" ControlToValidate="txtEdtCountryName"
                                                ToolTip="Required" ErrorMessage="Please enter cuntry name" SetFocusOnError="true"
                                                ValidationGroup="vgu"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtCountryName" runat="server" MaxLength="50"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rqtxtCountryName" runat="server" Text="*" ControlToValidate="txtCountryName"
                                                ToolTip="Required" ErrorMessage="Please enter Cuntry name" SetFocusOnError="true"
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
                                            <asp:ValidationSummary ID="vsAdd" runat="server" ValidationGroup="vgu" HeaderText="MITL Invoice"
                                                ShowMessageBox="true" ShowSummary="false" DisplayMode="BulletList" />
                                            &nbsp;|&nbsp;<asp:ImageButton ID="imgCancel" runat="server" AlternateText="Cancel"
                                                CommandName="Cancel" ToolTip="Cancel Update" ImageUrl="~/images/Cancel.png" Height="20px"
                                                Width="20px" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton ID="imdAdd" runat="server" ImageUrl="~/images/Add.png" Height="20px"
                                                Width="20px" ToolTip="Add" ValidationGroup="vg" CommandName="Insert" />
                                            <asp:ValidationSummary ID="vsAdd" runat="server" ValidationGroup="vg" HeaderText="MITL Invoice"
                                                ShowMessageBox="true" ShowSummary="false" DisplayMode="BulletList" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </center>
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
