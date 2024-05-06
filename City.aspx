<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true"
    CodeFile="City.aspx.cs" Inherits="City" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <center>
                <div id="DivHeading">
                    Manage City
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
                        <asp:GridView ID="gridCity" runat="server" AutoGenerateColumns="False" Width="100%"
                            ShowFooter="true" OnRowCommand="gridCity_RowCommand" OnRowDeleting="gridCity_RowDeleting"
                            OnRowEditing="gridCity_RowEditing" OnRowCancelingEdit="gridCity_RowCancelingEdit"
                            OnRowUpdating="gridCity_RowUpdating" AllowPaging="True" OnPageIndexChanging="gridCity_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSr" runat="server" Text='<%#Container.DataItemIndex + 1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="City Id" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCityId" runat="server" Text='<%#Eval("CityId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Country Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCountry1" runat="server" Text='<%#HighlightText(Eval("CountryName").ToString()) %>'></asp:Label>
                                        <asp:Label ID="lblCountry" runat="server" Text='<%#Eval("CountryName") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlEdtCountry" runat="server" DataSourceID="LinqDataSourceEdt"
                                            DataTextField="CountryName" DataValueField="CountryId" AutoPostBack="True" OnSelectedIndexChanged="ddlEdtCountry_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:LinqDataSource ID="LinqDataSourceEdt" runat="server" ContextTypeName="MITInvoiceDataContext"
                                            OrderBy="CountryName" Select="new (CountryId, CountryName)" TableName="CountryMasters">
                                        </asp:LinqDataSource>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlCountry" runat="server" DataSourceID="LinqDataSource1" DataTextField="CountryName"
                                            DataValueField="CountryId" AutoPostBack="True" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:LinqDataSource ID="LinqDataSource1" runat="server" ContextTypeName="MITInvoiceDataContext"
                                            OrderBy="CountryName" Select="new (CountryId, CountryName)" TableName="CountryMasters">
                                        </asp:LinqDataSource>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="State Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblState1" runat="server" Text='<%#HighlightText(Eval("StateName").ToString()) %>'></asp:Label>
                                        <asp:Label ID="lblState" runat="server" Text='<%#Eval("StateName")%>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlEdtState" runat="server">
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlState" runat="server">
                                        </asp:DropDownList>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="City Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCityName1" runat="server" Text='<%# HighlightText(Eval("CityName").ToString()) %>'></asp:Label>
                                        <asp:Label ID="lblCityName" runat="server" Text='<%#Eval("CityName")%>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEdtCityName" runat="server" MaxLength="50" Text='<%#Eval("CityName") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rqtxtEdtCityName" runat="server" Text="*" ControlToValidate="txtEdtCityName"
                                            ToolTip="Required" ErrorMessage="Please enter city name" SetFocusOnError="true"
                                            ValidationGroup="vgu"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtCityName" runat="server" MaxLength="50"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rqtxtCityName" runat="server" Text="*" ControlToValidate="txtCityName"
                                            ToolTip="Required" ErrorMessage="Please enter city name" SetFocusOnError="true"
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
