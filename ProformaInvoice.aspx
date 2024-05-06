<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="ProformaInvoice.aspx.cs" Inherits="ProformaInvoice" %>

<%@ Register assembly="CalendarExtenderPlus" namespace="AjaxControlToolkitPlus" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script language="JavaScript" type="text/javascript">
        window.onbeforeunload = confirmExit;
        function confirmExit()
        {

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="upProformaInvoice" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnShowPreview" />
            <asp:PostBackTrigger ControlID="btnExportPDF" />
            <asp:PostBackTrigger ControlID="btnExportXLS" />            
            <asp:PostBackTrigger ControlID="btnSaveInvoiceInfo" />
            <asp:PostBackTrigger ControlID="btnRevised" />
            
        </Triggers>
        <ContentTemplate>

    <center>
        <div id="DivHeading">
            Proforma Invoice
        </div>
    </center>
    <asp:MultiView ID="MultiViewProformaInvoice" runat="server" ActiveViewIndex="0">

        <asp:View ID="viewProformaInvoiceInfo" runat="server">
            <center>
            <table width="50%">
                <tr>
                    <td>Invoice for: </td>
                    <td>
                        <asp:RadioButton ID="rdbProUsa" runat="server" Checked="true" ForeColor="Black" GroupName="InvFrom" Text="  Mangalam USA" />
                        &nbsp;&nbsp;<asp:RadioButton ID="rdbProIndia" runat="server" ForeColor="Black" GroupName="InvFrom" Text="  Mangalam India" />
                    </td>
                </tr>
                <tr align="left">
                    <td>Business Unit: </td>
                    <td>
                        <asp:DropDownList ID="drpProBU" runat="server" AutoPostBack="true" ValidationGroup="vgInfo">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="reqBU" runat="server" ControlToValidate="drpProBU" ErrorMessage="*" Text="*" ValidationGroup="vgInfo"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr align="left">
                    <td>Client: </td>
                    <td>
                        <asp:DropDownList ID="drpProClient" runat="server" AutoPostBack="True" ValidationGroup="vgInfo" OnSelectedIndexChanged="drpProClient_SelectedIndexChanged1">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="reqClient" runat="server" ControlToValidate="drpProClient" ErrorMessage="*" Text="*" ValidationGroup="vgInfo"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr align="left">
                    <td>Invoice Period Start: </td>
                    <td>
                        <asp:TextBox ID="txtProInvoiceStart" runat="server" AutoPostBack="True" onKeyDown="javascript: return false;" onKeyPress="javascript: return false;" onPaste="javascript: return false;" ValidationGroup="vgInfo"></asp:TextBox>
                        <cc1:CalendarExtenderPlus ID="txtInvoiceStart_CalenderExtenderPlus" runat="server" Enabled="true" Format="dd-MMM-yyyy" PopupButtonID="txtInvoiceStart" TargetControlID="txtProInvoiceStart">
                        </cc1:CalendarExtenderPlus>
                        <asp:RequiredFieldValidator ID="reqInvoiceStart" runat="server" ControlToValidate="txtProInvoiceStart" ErrorMessage="*" Text="*" ValidationGroup="vgInfo"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr align="left">
                    <td>Invoice Period End: </td>
                    <td>
                        <asp:TextBox ID="txtProInvoiceEnd" runat="server" AutoPostBack="True" onKeyDown="javascript: return false;" onKeyPress="javascript: return false;" onPaste="javascript: return false;" ValidationGroup="vgInfo"></asp:TextBox>
                        <cc1:CalendarExtenderPlus ID="txtProInvoiceEnd_CalendarExtenderPlus" runat="server" Enabled="true" Format="dd-MMM-yyyy" PopupButtonID="txtInvoiceEnd" TargetControlID="txtProInvoiceEnd">
                        </cc1:CalendarExtenderPlus>
                        <asp:RequiredFieldValidator ID="reqInvoiceEnd" runat="server" ControlToValidate="txtProInvoiceEnd" ErrorMessage="*" Text="*" ValidationGroup="vgInfo"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr align="left">
                    <td>PO #: </td>
                    <td>
                        <asp:TextBox ID="txtProOrderNo" runat="server" MaxLength="100"></asp:TextBox>
                    </td>
                </tr>
                <tr align="left">
                    <td>PO Date: </td>
                    <td>
                        <asp:TextBox ID="txtProOrderDate" runat="server" AutoPostBack="True" onKeyDown="javascript: return false;" onKeyPress="javascript: return false;" onPaste="javascript: return false;"></asp:TextBox>
                        <cc1:CalendarExtenderPlus ID="txtProOrderDate_CalendarExtenderPlus" runat="server" Enabled="true" Format="dd-MMM-yyyy" PopupButtonID="txtOrderDate" TargetControlID="txtProOrderDate">
                        </cc1:CalendarExtenderPlus>
                    </td>
                </tr>
                <tr align="left">
                    <td>Invoice Date: </td>
                    <td>
                        <asp:TextBox ID="txtProInvoice" runat="server" AutoPostBack="True" onKeyDown="javascript: return false;" onKeyPress="javascript: return false;" onPaste="javascript: return false;" ValidationGroup="vgInfo"></asp:TextBox>
                        <cc1:CalendarExtenderPlus ID="txtProInvoice_CalendarExtenderPlus" runat="server" Enabled="true" Format="dd-MMM-yyyy" PopupButtonID="txtProInvoice" TargetControlID="txtProInvoice">
                        </cc1:CalendarExtenderPlus>
                    </td>
                    <asp:RequiredFieldValidator ID="reqInvoiceDate" runat="server" ControlToValidate="txtProInvoice" ErrorMessage="*" Text="*" ValidationGroup="vgInfo"></asp:RequiredFieldValidator>
                </tr>
                <tr align="left">
                    <td>Project From: </td>
                    <td>
                        <asp:DropDownList ID="drpProProjectFrom" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr align="left">
                    <td>Remarks: </td>
                    <td>
                        <asp:TextBox ID="txtProRemarks" runat="server" CssClass="MultiLineTextBox" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp; </td>
                </tr>
                <tr align="left">
                    <td></td>
                    <td>
                        <asp:Button ID="btnSaveInvoiceInfo" runat="server" Text="Save" ValidationGroup="vgInfo" Width="100px" OnClick="btnSaveInvoiceInfo_Click" />
                    </td>
                </tr>
            </table>
            </center>
        </asp:View>

        <asp:View ID="viewProfomaInvoiceDetail" runat="server">
            <table width="100%">
                        <tr>
                            <td style="width: 80%">
                                <asp:Button ID="btnExportXLS" runat="server" Text="Export to Excel" OnClick="btnExportXLS_Click1" />
                                <asp:Button ID="btnExportPDF" runat="server" Text="Export to PDF" OnClick="btnExportPDF_Click1" />
                                <asp:Button ID="btnShowPreview" runat="server" Text="Show Preview" OnClick="btnShowPreview_Click" />
                                <asp:Button ID="btnRevised" runat="server" Visible="true" Text="Revise Invoice"
                                    OnClientClick="return confirm('Are you sure you want to revise this invoice?');" OnClick="btnRevised_Click" />
                                <asp:Button ID="btnBack" runat="server" Text="Edit Invoice Info" Width="150px" OnClick="btnBack_Click"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table width="100%">
                                    <tr>
                                        <td>
                                            Client:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblClientName" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            Invoice Period:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblInvoicePeriod" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Invoice #:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblInvoiceNo" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            Invoice Date:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblInvoiceDate" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            PO #:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblOrderNo" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            PO Date:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblOrderDate" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Project From:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblProjectFrom" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            Total Amount:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTotalInvoiceAmount" runat="server">
                                            </asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="grdInvoiceData" runat="server" ShowFooter="true" AllowPaging="false"
                                    PageSize="100" AutoGenerateColumns="false" OnRowCancelingEdit="grdInvoiceData_RowCancelingEdit" 
                                    OnRowDataBound="grdInvoiceData_RowDataBound" OnRowDeleting="grdInvoiceData_RowDeleting" 
                                    OnRowEditing="grdInvoiceData_RowEditing" OnRowUpdating="grdInvoiceData_RowUpdating">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr. No." HeaderStyle-Width="50px">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                            <HeaderStyle Width="50px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ItemID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemID" runat="server" Text='<%#Eval("ProInvoiceDetailsId") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="50px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Services" HeaderStyle-Width="45%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblServices" Text='<%#Eval("ItemDesc") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtServices" Text='<%#Eval("ItemDesc") %>' CssClass="MultiLineTextBox"
                                                    runat="server" TextMode="MultiLine" Width="80%" ValidationGroup="vgUpdate">
                                                </asp:TextBox>
                                                <asp:RequiredFieldValidator ID="reqUpdateService" runat="server" Text="*" ErrorMessage="*"
                                                    ControlToValidate="txtServices" ValidationGroup="vgUpdate"></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtAddServices" runat="server" CssClass="MultiLineTextBox" Width="80%"
                                                    TextMode="MultiLine" ValidationGroup="vgAdd" onkeyup="asdf();">
                                                </asp:TextBox>
                                                <asp:RequiredFieldValidator ID="reqAddServices" runat="server" Text="*" ErrorMessage="*"
                                                    ControlToValidate="txtAddServices" ValidationGroup="vgAdd"></asp:RequiredFieldValidator>
                                            </FooterTemplate>
                                            <HeaderStyle Width="45%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Per" HeaderStyle-Width="75px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPriceType" Text='<%#Eval("PriceType") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>

                                                <asp:Label ID="lblEditPriceType" Text='<%#Eval("PriceType") %>' runat="server" Visible="false"></asp:Label>

                                                <asp:DropDownList ID="drpPriceType" runat="server">
                                                </asp:DropDownList>
                                                
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:DropDownList ID="drpAddPriceType" runat="server">
                                                </asp:DropDownList>
                                            </FooterTemplate>
                                            <HeaderStyle Width="75px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quantity" HeaderStyle-Width="150px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQuantity" Text='<%#Eval("Qty") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtQuantity" Text='<%#Eval("Qty") %>' Width="75%" AutoPostBack="false"
                                                    runat="server">
                                                </asp:TextBox>
                                                <asp:RegularExpressionValidator ID="regExUpdateQuantity" runat="server" ControlToValidate="txtQuantity"
                                                    Text="*" ErrorMessage="*" ValidationExpression="[\d]{0,7}([.][\d]{0,3})?" ValidationGroup="vgUpdate"></asp:RegularExpressionValidator>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtAddQuantity" runat="server" AutoPostBack="false"
                                                    Width="75%"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="regExAddQuantity" runat="server" ControlToValidate="txtAddQuantity"
                                                    Text="*" ErrorMessage="*" ValidationExpression="[\d]{0,7}([.][\d]{0,3})?" ValidationGroup="vgAdd"></asp:RegularExpressionValidator>
                                            </FooterTemplate>
                                            <HeaderStyle Width="150px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Unit Price" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUnitPrice" Text='<%#Eval("UnitPrice") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtUnitPrice" Text='<%#Eval("UnitPrice") %>' Width="65%" AutoPostBack="false"
                                                    runat="server"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="regExUpdateUnitPrice" runat="server" ControlToValidate="txtUnitPrice"
                                                    Text="*" ErrorMessage="*" ValidationExpression="\-{0,1}[\d]{0,8}([.][\d]{0,5})?"
                                                    ValidationGroup="vgUpdate"></asp:RegularExpressionValidator>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtAddUnitPrice" runat="server" AutoPostBack="false" Width="60%"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="regExAddUnitPrice" runat="server" ControlToValidate="txtAddUnitPrice"
                                                    Text="*" ErrorMessage="*" ValidationExpression="\-{0,1}[\d]{0,8}([.][\d]{0,5})?"
                                                    ValidationGroup="vgAdd"></asp:RegularExpressionValidator>
                                            </FooterTemplate>
                                            <HeaderStyle Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amount" HeaderStyle-Width="150px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" Text='<%#Eval("TotalAmt") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblAddAmount" runat="server"></asp:Label>
                                            </FooterTemplate>
                                            <HeaderStyle Width="150px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="View | Edit">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgbtnEdit" ImageUrl="~/images/editing.png" AlternateText="Edit"
                                                    CommandName="Edit" runat="server" Height="20px" Width="20px" CausesValidation="false" />
                                                <asp:ImageButton ID="imgbtnDelete" ImageUrl="~/images/delete.png" CommandName="Delete"
                                                    AlternateText="Delete" runat="server" Height="20px" Width="20px" CausesValidation="false"
                                                    OnClientClick="return confirm('Do you want to delete this invoice item?');" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:ImageButton ID="imgbtnUpdate" ImageUrl="~/images/update.png" CommandName="Update"
                                                    AlternateText="Update" runat="server" Height="20px" Width="20px" ValidationGroup="vgUpdate" />
                                                <asp:ImageButton ID="imgbtnCancel" ImageUrl="~/images/Cancel.png" CommandName="Cancel"
                                                    AlternateText="Cancel" runat="server" Height="20px" Width="20px" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:ImageButton ID="imgbtnAdd" ImageUrl="~/images/Add.png" CommandName="Add" AlternateText="Add"
                                                    runat="server" Height="20px" Width="20px" OnClick="imgbtnAdd_Click1" ValidationGroup="vgAdd" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
        </asp:View>
    </asp:MultiView>

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

