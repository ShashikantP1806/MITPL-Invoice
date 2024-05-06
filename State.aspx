<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true"
    CodeFile="State.aspx.cs" Inherits="State" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <center>
                <div id="DivHeading">
                    Manage State
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
                        <asp:GridView ID="gridState" runat="server" AutoGenerateColumns="False" Width="100%"
                            ShowFooter="true" OnRowCommand="gridState_RowCommand" OnRowDeleting="gridState_RowDeleting"
                            OnRowEditing="gridState_RowEditing" OnRowCancelingEdit="gridState_RowCancelingEdit"
                            OnRowUpdating="gridState_RowUpdating" AllowPaging="True" OnPageIndexChanging="gridState_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSr" runat="server" Text='<%#Container.DataItemIndex + 1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="State Id" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStateId" runat="server" Text='<%#Eval("StateId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Country Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCountry1" runat="server" Text='<%#HighlightText(Eval("CountryName").ToString()) %>'></asp:Label>
                                        <asp:Label ID="lblCountry" runat="server" Text='<%#Eval("CountryName") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlEdtCountry" runat="server" DataSourceID="LinqDataSource1"
                                            DataTextField="CountryName" DataValueField="CountryId">
                                        </asp:DropDownList>
                                        <asp:LinqDataSource ID="LinqDataSource1" runat="server" ContextTypeName="MITInvoiceDataContext"
                                            OrderBy="CountryName" Select="new (CountryId, CountryName)" TableName="CountryMasters">
                                        </asp:LinqDataSource>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlCountry" runat="server" DataSourceID="LinqDataSource1" DataTextField="CountryName"
                                            DataValueField="CountryId">
                                        </asp:DropDownList>
                                        <asp:LinqDataSource ID="LinqDataSource1" runat="server" ContextTypeName="MITInvoiceDataContext"
                                            OrderBy="CountryName" Select="new (CountryId, CountryName)" TableName="CountryMasters">
                                        </asp:LinqDataSource>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="State Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStateName1" runat="server" Text='<%#HighlightText(Eval("StateName").ToString()) %>'></asp:Label>
                                        <asp:Label ID="lblStateName" runat="server" Text='<%#Eval("StateName") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEdtStateName" runat="server" MaxLength="50" Text='<%#Eval("StateName") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rqtxtEdtStateName" runat="server" Text="*" ControlToValidate="txtEdtStateName"
                                            ToolTip="Required" ErrorMessage="Please enter state name" SetFocusOnError="true"
                                            ValidationGroup="vgu"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtStateName" runat="server" MaxLength="50"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rqtxtStateName" runat="server" Text="*" ControlToValidate="txtStateName"
                                            ToolTip="Required" ErrorMessage="Please enter state name" SetFocusOnError="true"
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
