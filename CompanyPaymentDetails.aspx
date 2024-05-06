<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="CompanyPaymentDetails.aspx.cs" Inherits="CompanyPaymentDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            $(document).tooltip({
                position: {
                    my: "center bottom-20",
                    at: "center top",
                    using: function (position, feedback) {
                        $(this).css(position);
                        $("<div>")
            .addClass("arrow")
            .addClass(feedback.vertical)
            .addClass(feedback.horizontal)
            .appendTo(this);
                    }
                }
            });
        });
    </script>

    <style type="text/css">
        .ModalPopupBG {
            background-color: Gray;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }

        .FormatRadioButtonList label {
            margin-left: 10px;
            vertical-align: text-bottom;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:HiddenField ID="InvListShow" runat="server" Value="15" />
    <asp:MultiView ID="mvCompanyPaymentDetails" runat="server">
        <asp:View ID="viewCompanyPayment" runat="server">
            <center>
                <div id="DivHeading">
                    Payment Details
                </div>
            </center>
            <table width="100%">
                <tr>
                    <td align="left">
                        <asp:Button ID="btnPaymentDetails" runat="server" Text="New Payment Details" Width="150px" ToolTip="Add new company payment details"
                            OnClick="btnPaymentDetails_Click" />
                        <asp:Label ID="lblError2" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                    </td>
                    <td align="right">
                        Department :&nbsp;<asp:DropDownList ID="ddlDepartmentSearch" runat="server" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlDepartmentSearch_SelectedIndexChanged">
                    </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <%--OnPageIndexChanging="gridCompany_PageIndexChanging" OnRowCommand="gridCompany_RowCommand"
                            OnRowDeleting="gridCompany_RowDeleting" OnRowDataBound="gridCompany_RowDataBound"--%>
                        <asp:GridView ID="gridPaymentDetails" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            ShowFooter="false" PageSize="10" Width="100%" OnPageIndexChanging="gridPaymentDetails_PageIndexChanging" OnRowCommand="gridPaymentDetails_RowCommand"
                            OnRowDeleting="gridPaymentDetails_RowDeleting" OnRowDataBound="gridPaymentDetails_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSr" runat="server" Text="<%#Container.DataItemIndex + 1 %>"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CPDID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCPDID" runat="server" Text='<%#Eval("CPDID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Inv Number">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInvoiceNumber" runat="server" Text='<%#Eval("InvoiceNumber") %>' Visible="true"></asp:Label>
                                    </ItemTemplate>
                                    <ControlStyle Width="170px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Inv Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInvoiceAmount" runat="server" Text='<%#Eval("InvoiceAmount") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ControlStyle Width="90px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Inv Currency" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInvoiceCurrency" runat="server" Text='<%#Eval("InvoiceCurrency") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ControlStyle Width="80px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount Received">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAmountReceived" runat="server" Text='<%#Eval("AmountReceived") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ControlStyle Width="120px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Consultant Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblConsultantName" runat="server" Text='<%#Eval("ConsultantName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ControlStyle Width="120px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Contact Person">
                                    <ItemTemplate>
                                        <asp:Label ID="lblContactPersonName" runat="server" Text='<%#Eval("ContactPersonName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ControlStyle Width="120px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Paid Currency" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPaidCurrency" runat="server" Text='<%#Eval("PaidCurrency") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ControlStyle Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Paid Amount" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblPaidAmount" runat="server" Text='<%#Eval("PaidAmount") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ControlStyle Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remarks">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Remarks") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ControlStyle Width="100px" />
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Edit | Delete">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgEdit" runat="server" Height="20px" ImageUrl="~/images/editing.png"
                                            OnClick="imgEdit_Click" ToolTip="Edit" Width="20px" />
                                        <asp:Label ID="lbl1" runat="server" Text=" | "></asp:Label>
                                        <asp:ImageButton ID="imgDelete" runat="server" AlternateText="Delete" CommandName="Delete"
                                            Height="20px" ImageUrl="~/images/delete.png" OnClientClick="return confirm('Do you want to delete this country?');"
                                            ToolTip="Delete" Width="20px" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton ID="imgUpdate" runat="server" AlternateText="Update" CommandName="Update"
                                            Height="20px" ImageUrl="~/images/update.png" ToolTip="Update" Width="20px" />
                                        &nbsp;|&nbsp;<asp:ImageButton ID="imgCancel" runat="server" AlternateText="Cancel"
                                            CommandName="Cancel" Height="20px" ImageUrl="~/images/Cancel.png" ToolTip="Cancel Update"
                                            Width="20px" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>

        </asp:View>

        <asp:View ID="viewAddPaymentDetails" runat="server">
            <center>
                <div id="DivHeading">
                    Add Payment Details
                </div>
            </center>
            <center>
                <table width="80%">
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="lblError1" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Invoice Number :
                                    <asp:Label ID="lblrqInvNumber" runat="server" Text="*" ForeColor="Blue"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblInvID" runat="server" Visible="false"></asp:Label>
                            <asp:TextBox ID="txtInvNumber" runat="server" MaxLength="600" ToolTip="Invoice Number"></asp:TextBox>
                            &nbsp;<asp:ImageButton ID="imgInvList" runat="server" AlternateText="InvList" CommandName="InvList"
                                Height="15px" ImageUrl="~/images/Search.PNG" ToolTip="Invoice List" Width="15px" OnClick="imgInvList_Click" />&nbsp;
                            <asp:RequiredFieldValidator ID="rqInvNumber" runat="server" Text="*" ControlToValidate="txtInvNumber"
                                ToolTip="Required" ErrorMessage="Pleae enter invoice number" SetFocusOnError="true"
                                ValidationGroup="vg"></asp:RequiredFieldValidator>
                        </td>
                        <td></td>
                    </tr>

                    <tr>
                        <td>Invoice Amount :
                                    <asp:Label ID="lblrqInvAmount" runat="server" Text="*" ForeColor="Blue"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblInvAmount" runat="server" Text="" ></asp:Label>
                            
                            <%--<asp:TextBox ID="txtPhone" runat="server" MaxLength="15" ToolTip="Phone number"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rqPhone" runat="server" Text="*" ControlToValidate="txtPhone"
                                ToolTip="Required" ErrorMessage="Please enter phone number" SetFocusOnError="true"
                                ValidationGroup="vg"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revPhone" runat="server" ControlToValidate="txtPhone"
                                ErrorMessage="Telephone number must be 7-15 digit" ValidationExpression="^[0-9]{7,15}$"
                                Text="*" ValidationGroup="vg" SetFocusOnError="true" ToolTip="Telephone number must be 7-15 digit"></asp:RegularExpressionValidator>--%>
                        </td>
                        <td></td>

                    </tr>
                    <tr>
                        <td>Invoice Currency:
                                    <asp:Label ID="lblrqInvCurrency" runat="server" Text="*" ForeColor="Blue"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblInvCurrency" runat="server" Text=""></asp:Label>
                            <%--<asp:DropDownList ID="ddlDepartment" runat="server" ToolTip="Select department">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rqDepartment" runat="server" Text="*" ControlToValidate="ddlDepartment"
                                ErrorMessage="Please select department" SetFocusOnError="true" InitialValue="-- Select --"
                                ToolTip="Required" ValidationGroup="vg"></asp:RequiredFieldValidator>--%>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>Amount Received :
                                    <asp:Label ID="lblrqAmtReceived" runat="server" Text="*" ForeColor="Blue"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAmtReceived" runat="server" MaxLength="15" ToolTip="Amount Received"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rqv" runat="server" Text="*" ControlToValidate="txtAmtReceived"
                                ToolTip="Required" ErrorMessage="Please enter amount" SetFocusOnError="true"
                                ValidationGroup="vg"></asp:RequiredFieldValidator>
                            <%--<asp:DropDownList ID="ddlCurrency" runat="server">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rqCurrency" runat="server" Text="*" ControlToValidate="ddlCurrency"
                                ErrorMessage="Please select currency" SetFocusOnError="true" InitialValue="-- Select --"
                                ToolTip="Required" ValidationGroup="vg"></asp:RequiredFieldValidator>--%>
                        </td>
                        <td></td>

                    </tr>

                    <tr>
                        <td>Consultant Company :
                                    <asp:Label ID="lblrqConsultant" runat="server" Text="*" ForeColor="Blue"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlConsultant" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlConsultant_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rqConsultant" runat="server" Text="*" ControlToValidate="ddlConsultant"
                                ErrorMessage="Please select Consultant" SetFocusOnError="true" InitialValue="-- Select --"
                                ToolTip="Required" ValidationGroup="vg"></asp:RequiredFieldValidator>
                        </td>
                        <td></td>

                    </tr>

                    <tr>
                        <td>Contact Person Name :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlContactPerson" runat="server">
                            </asp:DropDownList>
                            <%--<asp:RequiredFieldValidator ID="rqContactPerson" runat="server" Text="*" ControlToValidate="ddlContactPerson"
                                ErrorMessage="Please select Contact Person" SetFocusOnError="true" InitialValue="-- Select --"
                                ToolTip="Required" ValidationGroup="vg"></asp:RequiredFieldValidator>--%>

                        </td>
                        <td>&nbsp;
                        </td>

                    </tr>

                    <tr>
                        <td>Paid Currency :
                                    <asp:Label ID="lblPaidCurrency" runat="server" Text="*" ForeColor="Blue"></asp:Label>
                        </td>
                        <td>
                             <asp:Label ID="txtPaidCurrency" runat="server" Text="" ></asp:Label>
                           <%-- <asp:TextBox ID="txtPaidCurrency" runat="server" MaxLength="15" ToolTip="Paid Currency"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rqPaidCurrency" runat="server" Text="*" ControlToValidate="txtPaidCurrency"
                                ToolTip="Required" ErrorMessage="Please enter paid currency" SetFocusOnError="true"
                                ValidationGroup="vg"></asp:RequiredFieldValidator>--%>
                        </td>
                        <td></td>

                    </tr>

                    <tr>
                        <td>Paid Amount :
                                    <asp:Label ID="lblPaidAmount" runat="server" Text="*" ForeColor="Blue"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPaidAmount" runat="server" MaxLength="15" ToolTip="Paid Amount"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rqPaidAmount" runat="server" Text="*" ControlToValidate="txtPaidAmount"
                                ToolTip="Required" ErrorMessage="Please enter paid amount" SetFocusOnError="true"
                                ValidationGroup="vg"></asp:RequiredFieldValidator>
                        </td>
                        <td></td>

                    </tr>
                    <tr>
                        <td>Remarks :
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Height="60" Width="300"
                                ToolTip="Remarks"></asp:TextBox>
                        </td>
                        <td></td>

                    </tr>

                    <tr>
                        <td colspan="4">
                            <asp:Label ID="Label1" runat="server" Text="*" ForeColor="Blue" Font-Bold="true"></asp:Label>
                            <b>Field should be required</b></td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" ValidationGroup="vg" />
                            &nbsp&nbsp
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                            <asp:ValidationSummary ID="vs" runat="server" ValidationGroup="vg" DisplayMode="BulletList"
                                HeaderText="MITPLInvoice" ShowSummary="false" ShowMessageBox="false" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;
                        </td>
                    </tr>
                </table>
            </center>

        </asp:View>
        <asp:View ID="mvInvList" runat="server">
            <center>
                <div id="DivHeading">
                    Select Invoice
                </div>
            </center>
            <center>
                <table width="80%">
                    <tr>
                        <td >
                            <asp:Button ID="btnBack" Text="Back" runat="server" OnClick="btnBack_Click" />
                            
                        </td>
                        <td style="text-align:right">
                           <asp:Label ID="lblSearchMore" runat="server" Text="To search more invoice -> " ForeColor="royalblue" Font-Bold="true"></asp:Label>&nbsp;&nbsp<asp:TextBox ID="txtSearchInvList" runat="server" Width="180px" ToolTip="Search Invoice" Placeholder="Search invoice number"></asp:TextBox>
                            &nbsp;&nbsp;
                            <asp:Button ID="btnSearchInv" Text="Search" runat="server" OnClick="btnSearchInv_Click" />
                        </td>
                    </tr>
                    <tr style="line-height:1em">
                        <td colspan="2" style="color:royalblue;font-weight:bold;">
                            <br />Following(s) Invoice List
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="border:0px solid black; border-collapse:collapse; margin-left:5px">
                            <asp:RadioButtonList ID="rblInvList" runat="server" CssClass="FormatRadioButtonList" OnSelectedIndexChanged="rblInvList_SelectedIndexChanged" AutoPostBack="true"></asp:RadioButtonList>
                             <asp:Label ID="lblInvListFound" runat="server" Text="No record(s) found"   ForeColor="Red" Font-Bold="true" Visible="false"></asp:Label>
                        </td>

                    </tr>
                    <tr style="line-height:1em">
                        <td colspan="2">

                        </td>
                    </tr>
                </table>
            </center>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <%--  <asp:AsyncPostBackTrigger ControlID="lbtnAddState" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="lbtnAddCity" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="lbtnAddCountry" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="lbtnAddCity2" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="lbtnAddState2" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="lbtnAddCountry2" EventName="Click" />--%>
                </Triggers>
                <ContentTemplate>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:View>

    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder4" runat="Server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ContentPlaceHolder5" runat="Server">
</asp:Content>

