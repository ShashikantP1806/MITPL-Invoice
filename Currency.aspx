<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true"
    CodeFile="Currency.aspx.cs" Inherits="Currency" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">
        function FocusNext(cid) {
            //            var i = document.getElementById(cid.id).getAttribute('tabindex');
            //            var iKeyCode = 0;
            var charStr = String.fromCharCode(window.event.keyCode);
            if (charStr == "&" || charStr == "#") {
                alert("Please don't enter '&' and '#'");
            }
            var grid = document.getElementById("<%=gridCurrency.ClientID%>");
            var txtCurrencySymbols = document.getElementById("ctl00_ContentPlaceHolder1_gridCurrency_ctl12_txtCurrencySymbol");

            txtCurrencySymbols.value = txtCurrencySymbols.value.replace('&', "");
            txtCurrencySymbols.value = txtCurrencySymbols.value.replace('#', "");


        }
        function FocusNext2(cid) {
            //            var i = document.getElementById(cid.id).getAttribute('tabindex');
            //            var iKeyCode = 0;
            var charStr = String.fromCharCode(window.event.keyCode);
            if (charStr == "&" || charStr == "#") {
                alert("Please don't enter '&' and '#'");
            }
            //            var grid = document.getElementById("<%=gridCurrency.ClientID%>");
            //            var txtCurrencySymbols = document.getElementById(id);

            //            txtCurrencySymbols.value = txtCurrencySymbols.value.replace('&', "");
            //            txtCurrencySymbols.value = txtCurrencySymbols.value.replace('#', "");


        }
    </script>

    <%--<script type="text/javascript">
        $(document).ready(function() {
            $("#<%=gridCurrency.ClientID %> INPUT[id$='txtCurrencySymbol']").keypress(
function IsNumericHandler(e) {
    var unicode = e.charCode ? e.charCode : e.keyCode

    //if the key isn't the backspace key (which we should allow)
    if (unicode != 8) {
        //if not a number
        if (unicode >= 57)
        //disable key press
            return false;
    }
});
        });
    </script>--%>
    <%--<script type="text/javascript" language="javascript">
        function numeralsOnly(evt) {
            var grid = document.getElementById("<%=gridCurrency.ClientID%>");
            var txtCurrencySymbols = document.getElementById("ctl00_ContentPlaceHolder1_gridCurrency_ctl11_txtCurrencySymbol");
            if (txtCurrencySymbols.value != "") {
                alert("Enter numerals only in this field!");
                return false;
            }
            return true;
        }
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <center>
                <div id="DivHeading">
                    Manage Currency
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
                        <asp:GridView ID="gridCurrency" runat="server" AutoGenerateColumns="False" Width="100%"
                            ShowFooter="true" OnRowCommand="gridCurrency_RowCommand" OnRowDeleting="gridCurrency_RowDeleting"
                            OnRowEditing="gridCurrency_RowEditing" OnRowCancelingEdit="gridCurrency_RowCancelingEdit"
                            OnRowUpdating="gridCurrency_RowUpdating" AllowPaging="True" OnPageIndexChanging="gridCurrency_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSr" runat="server" Text='<%#Container.DataItemIndex + 1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Currency Id" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCurrencyId" runat="server" Text='<%#Eval("CurrencyId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCurrencyName1" runat="server" Text='<%#HighlightText(Eval("CurrencyName").ToString()) %>'></asp:Label>
                                        <asp:Label ID="lblCurrencyName" runat="server" Text='<%#Eval("CurrencyName")%>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEdtCurrencyName" runat="server" MaxLength="25" Text='<%#Eval("CurrencyName") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rqEdtCurrencyName" runat="server" ControlToValidate="txtEdtCurrencyName"
                                            SetFocusOnError="true" Text="*" ErrorMessage="Please enter currency name" ToolTip="Required"
                                            ValidationGroup="vg"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtCurrencyName" runat="server" MaxLength="25"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rqCurrencyName" runat="server" ControlToValidate="txtCurrencyName"
                                            SetFocusOnError="true" Text="*" ErrorMessage="Please enter currency name" ToolTip="Required"
                                            ValidationGroup="vg"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Code">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCurrencyCode1" runat="server" Text='<%#HighlightText(Eval("CurrencyCode").ToString()) %>'></asp:Label>
                                        <asp:Label ID="lblCurrencyCode" runat="server" Text='<%#Eval("CurrencyCode") %>'
                                            Visible="false"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEdtCurrencyCode" runat="server" MaxLength="5" Text='<%#Eval("CurrencyCode") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rqEdtCurrencyCode" runat="server" ControlToValidate="txtEdtCurrencyCode"
                                            SetFocusOnError="true" Text="*" ErrorMessage="Please enter currency code" ToolTip="Required"
                                            ValidationGroup="vgu"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtCurrencyCode" runat="server" MaxLength="5"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rqCurrencyCode" runat="server" ControlToValidate="txtCurrencyCode"
                                            SetFocusOnError="true" Text="*" ErrorMessage="Please enter currency code" ToolTip="Required"
                                            ValidationGroup="vg"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Symbol">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCurrencySymbol" runat="server" Text='<%#Eval("CurrencySymbol") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEdtCurrencySymbol" runat="server" MaxLength="50" ToolTip="Please delete '&#' in code"
                                            onKeyPress="FocusNext2(this)" Text='<%#Eval("CurrencySymbol") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:ImageButton ID="imgbtnHelp" runat="server" AlternateText="Help?" ImageUrl="~/images/Help.PNG"
                                            Height="15px" Width="15px" ToolTip="Help?" OnClick="imgbtnHelp_Click" OnClientClick="aspnetForm.target ='_blank';" />
                                        <asp:TextBox ID="txtCurrencySymbol" runat="server" MaxLength="50" Width="60px" AutoPostBack="True"
                                            OnTextChanged="txtCurrencySymbol_TextChanged" onKeyPress="FocusNext(this)"></asp:TextBox>
                                        <asp:Label ID="lblFCurrencySymbol" runat="server"></asp:Label>
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
