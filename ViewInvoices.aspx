<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true"
    CodeFile="ViewInvoices.aspx.cs" Inherits="ViewInvoices" %>

<%@ Register Assembly="CalendarExtenderPlus" Namespace="AjaxControlToolkitPlus" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="upViewInvoice" runat="server">
        <Triggers>
            
            <asp:PostBackTrigger ControlID="btnSendEmail" />
            <asp:PostBackTrigger ControlID="btnIntimate" />
            <%--<asp:PostBackTrigger ControlID="imgbtnEdit" />
            <asp:PostBackTrigger ControlID="imgbtnDelete" />
                
            <asp:PostBackTrigger ControlID="imgbtnUpdate" />
            <asp:PostBackTrigger ControlID="imgbtnCancel" />--%>
        </Triggers>
        <ContentTemplate>
            <center>
                <div id="DivHeading">
                    View Invoice
                </div>
            </center>
            <table id="tbldrp" runat="server">
                <tr>
                    <td>
                        Business Unit:
                        <asp:DropDownList ID="drpBusiness" AutoPostBack="true" runat="server" OnSelectedIndexChanged="drpBusiness_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        Invoice Status:
                        <asp:DropDownList ID="drpInvoiceStatus" AutoPostBack="true" runat="server" OnSelectedIndexChanged="drpInvoiceStatus_SelectedIndexChanged">
                            <asp:ListItem Selected="True">Draft</asp:ListItem>
                            <asp:ListItem>Paid</asp:ListItem>
                            <asp:ListItem>Unpaid</asp:ListItem>
                            <asp:ListItem>Revised</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        Client:
                        <asp:DropDownList ID="ddlClient" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlClient_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btnIntimate" runat="server" Text="Intimate" Enabled="false" OnClick="btnIntimate_Click"
                            Visible="false" />
                        <asp:Button ID="btnSendEmail" runat="server" Text="Send Mail" Enabled="false" Visible="false"
                            OnClick="btnSendEmail_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbltest" runat="server" Visible="false"></asp:Label>
                    </td>
                </tr>
            </table>
            <table>
                <asp:GridView ID="gvInv" runat="server" ShowFooter="false" AllowPaging="true" PageSize="10"
                    AutoGenerateColumns="false" OnRowDataBound="gvInv_RowDataBound" OnRowCancelingEdit="gvInv_RowCancelingEdit"
                    OnRowDeleting="gvInv_RowDeleting" OnRowEditing="gvInv_RowEditing" OnRowUpdating="gvInv_RowUpdating"
                    OnPageIndexChanging="gvInv_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="Sr. No.">
                            <ItemTemplate>
                                <%# Container.DataItemIndex+1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="InvoiceID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblInvoiceID" runat="server" Text='<%#Eval("InvoiceID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>                        
                         <asp:TemplateField HeaderText="Invoice For">
                            <ItemTemplate>
                                <asp:Label ID="lblInvoiceFor" runat="server" Text='<%#Eval("InvoiceFor") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Invoice Number">
                            <ItemTemplate>
                                <asp:HyperLink ID="hyperlnkInvoiceNo" runat="server" Text='<%# Eval ("InvoiceNum").ToString()+ HighlightText(Eval("InvSeq").ToString()) %>' Target="_blank" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Proforma Invoice Number">
                            <ItemTemplate>
                                <asp:HyperLink ID="hyperlnkProInvNo" runat="server" Text='<%# Eval ("ProInvoiceNum") %>' Target="_blank" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Client">
                            <ItemTemplate>
                                <asp:Label ID="lblClient1" runat="server" Text='<%#HighlightText(Eval("ClientName").ToString()) %>'></asp:Label>
                                <asp:Label ID="lblClient" runat="server" Text='<%#Eval("ClientName") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amount">
                            <ItemTemplate>
                                <asp:Label ID="lblInvoiceAmount" runat="server" Text='<%#Eval("InvoiceAmount") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Invoice Date">
                            <ItemTemplate>
                                <asp:Label ID="lblInvoiceDate" runat="server" Text='<%#Eval("InvoiceDate", "{0:dd-MMM-yy}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Paid Date">
                            <ItemTemplate>
                                <asp:Label ID="lblInvoicePaidDate" runat="server" Text='<%#Eval("PaidDate", "{0:dd-MMM-yy}") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtInvoicePaidDate" runat="server" AutoPostBack="True" onKeyPress="javascript: return false;"
                                    onKeyDown="javascript: return false;" onPaste="javascript: return false;"></asp:TextBox>
                                <cc1:CalendarExtenderPlus ID="txtInvoicePaidDate_CalenderExtenderPlus" runat="server"
                                    Format="dd-MMM-yyyy" TargetControlID="txtInvoicePaidDate" Enabled="true" PopupButtonID="txtInvoicePaidDate">
                                </cc1:CalendarExtenderPlus>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("InvoiceStatus") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="drpStatus" runat="server">
                                    <asp:ListItem Selected="True">Paid</asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>                        
                        <asp:TemplateField HeaderText="Edit | Delete">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnEdit" runat="server" AlternateText="Edit" ToolTip="Edit"
                                    CommandName="Edit" Visible="false" ImageUrl="~/images/editing.png" Height="20px"
                                    Width="20px" />
                                <asp:ImageButton ID="imgbtnDelete" runat="server" AlternateText="Delete" ToolTip="Delete"
                                    CommandName="Delete" Visible="false" ImageUrl="~/images/delete.png" OnClientClick="return confirm('Do you want to delete this invoice?');"
                                    Height="20px" Width="20px" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton ID="imgbtnUpdate" runat="server" AlternateText="Update" ToolTip="Update"
                                    CommandName="Update" ImageUrl="~/images/update.png" Height="20px" Width="20px" />
                                <asp:ImageButton ID="imgbtnCancel" runat="server" AlternateText="Cancel" ToolTip="Cancel"
                                    CommandName="Cancel" ImageUrl="~/images/cancel.png" Height="20px" Width="20px" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="20%" Visible="false">
                            <HeaderTemplate>
                                &nbsp&nbsp
                                <asp:CheckBox ID="chkSelectAll" runat="server" Text=" Select All" AutoPostBack="True"
                                    OnCheckedChanged="chkSelectAll_CheckedChanged"></asp:CheckBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkClient" runat="server" AutoPostBack="True" OnCheckedChanged="chkClient_CheckedChanged">
                                </asp:CheckBox>
                            </ItemTemplate>
                            <HeaderStyle Width="20%" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="MainProInvoiceID" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblMainProInvoiceID" runat="server" Text='<%#Eval("MainProInvoiceID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>  
                    </Columns>
                </asp:GridView>
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
