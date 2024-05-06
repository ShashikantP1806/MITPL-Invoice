<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true"
    CodeFile="Price.aspx.cs" Inherits="Price" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
        <center>
        <div id="DivHeading">
            Price Master
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
                        <asp:GridView ID="gridPrice" runat="server" AutoGenerateColumns="False" Width="100%"
                            ShowFooter="true" OnRowCommand="gridPrice_RowCommand" OnRowDeleting="gridPrice_RowDeleting"
                            OnRowEditing="gridPrice_RowEditing" OnRowCancelingEdit="gridPrice_RowCancelingEdit"
                            OnRowUpdating="gridPrice_RowUpdating" AllowPaging="True" OnPageIndexChanging="gridPrice_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSr" runat="server" Text='<%#Container.DataItemIndex + 1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Price Id" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPriceId" runat="server" Text='<%#Eval("PriceId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Client Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblClient" runat="server" Text='<%#Eval("ClientName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlEdtClient" runat="server" DataSourceID="LinqDataSourceC"
                                            DataTextField="ClientName" DataValueField="ClientId">
                                        </asp:DropDownList>
                                        <asp:LinqDataSource ID="LinqDataSourceC" runat="server" ContextTypeName="MITInvoiceDataContext"
                                            OrderBy="ClientName" Select="new (ClientId, ClientName)" TableName="ClientMasters"
                                            Where="IsActive == @IsActive">
                                            <WhereParameters>
                                                <asp:Parameter DefaultValue="True" Name="IsActive" Type="Boolean" />
                                            </WhereParameters>
                                        </asp:LinqDataSource>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlClient" runat="server">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rqClient" runat="server" ControlToValidate="ddlClient"
                                            Text="*" ErrorMessage="Please select client" ValidationGroup="vg" SetFocusOnError="true"
                                            ToolTip="Required" InitialValue="-- Select --"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Price Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPriceType" runat="server" Text='<%#Eval("PriceType") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlEdtPriceType" runat="server" DataSourceID="LinqDataSourceP"
                                            DataTextField="PriceType" DataValueField="PriceTypeId">
                                        </asp:DropDownList>
                                        <asp:LinqDataSource ID="LinqDataSourceP" runat="server" ContextTypeName="MITInvoiceDataContext"
                                            OrderBy="PriceType" Select="new (PriceTypeId, PriceType)" TableName="PriceTypeMasters">
                                        </asp:LinqDataSource>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlPriceType" runat="server">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rqPriceType" runat="server" ControlToValidate="ddlPriceType"
                                            Text="*" ErrorMessage="Please select price type" ValidationGroup="vg" SetFocusOnError="true"
                                            ToolTip="Required" InitialValue="-- Select --"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Process">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProcess" runat="server" Text='<%#Eval("Process") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlEdtProcess" runat="server" DataSourceID="LinqDataSource1"
                                            DataTextField="ProcessName" DataValueField="ProcessId">
                                        </asp:DropDownList>
                                        <asp:LinqDataSource ID="LinqDataSource1" runat="server" ContextTypeName="MITInvoiceDataContext"
                                            OrderBy="ProcessName" Select="new (ProcessId, ProcessName)" TableName="ProcessMasters">
                                        </asp:LinqDataSource>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlProcess" runat="server">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rqProcess" runat="server" ControlToValidate="ddlProcess"
                                            Text="*" ErrorMessage="Please select process" ValidationGroup="vg" SetFocusOnError="true"
                                            ToolTip="Required" InitialValue="-- Select --"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Unit Price">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUnitPrice" runat="server" Text='<%#Eval("UnitPrice") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEdtUnitPrice" runat="server" MaxLength="7" Text='<%#Eval("UnitPrice") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rqEdtUnitPrice" runat="server" ControlToValidate="txtEdtUnitPrice"
                                            Text="*" ErrorMessage="Please enter unit price" ValidationGroup="vgu" SetFocusOnError="true"
                                            ToolTip="Required"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revEdtUnitPrice" runat="server" ControlToValidate="txtEdtUnitPrice"
                                            ErrorMessage="Please enter valid unit price" ValidationExpression="^\d{1,4}(\.\d{0,2})?$"
                                            Text="*" ValidationGroup="vgu" SetFocusOnError="true" ToolTip="Unit price upto 4 digit and 2 decimal point"></asp:RegularExpressionValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtUnitPrice" runat="server" MaxLength="7"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rqUnitPrice" runat="server" ControlToValidate="txtUnitPrice"
                                            Text="*" ErrorMessage="Please enter unit price" ValidationGroup="vg" SetFocusOnError="true"
                                            ToolTip="Required"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revUnitPrice" runat="server" ControlToValidate="txtUnitPrice"
                                            ErrorMessage="Please enter valid unit price" ValidationExpression="^\d{1,4}(\.\d{0,2})?$"
                                            Text="*" ValidationGroup="vg" SetFocusOnError="true" ToolTip="Unit price upto 4 digit and 2 decimal point"></asp:RegularExpressionValidator>
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
                                            ToolTip="Update" ImageUrl="~/images/update.png" Height="20px" Width="20px" ValidationGroup="vgu" />
                                        <asp:ValidationSummary ID="vsUpdate" runat="server" ValidationGroup="vgu" ShowMessageBox="true"
                                            ShowSummary="false" DisplayMode="BulletList" />
                                        &nbsp;|&nbsp;<asp:ImageButton ID="imgCancel" runat="server" AlternateText="Cancel"
                                            CommandName="Cancel" ToolTip="Cancel Update" ImageUrl="~/images/Cancel.png" Height="20px"
                                            Width="20px" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:ImageButton ID="imdAdd" runat="server" ImageUrl="~/images/Add.png" Height="20px"
                                            Width="20px" ToolTip="Add" CommandName="Insert" ValidationGroup="vg" />
                                        <asp:ValidationSummary ID="vsAdd" runat="server" ValidationGroup="vg" ShowMessageBox="true"
                                            ShowSummary="false" DisplayMode="BulletList" />
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
