<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="CommonClientInvoice.aspx.cs" Inherits="CommonClientInvoice" %>

<%@ Register Assembly="CalendarExtenderPlus" Namespace="AjaxControlToolkitPlus" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script type="text/javascript">
        function onCalendarShown() {
            var cal = $find("calendar1");
            //Setting the default mode to month
            cal._switchMode("months", true);
            //Iterate every month Item and attach click event to it
            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.addHandler(row.cells[j].firstChild, "click", call);
                    }
                }
            }
        }

        function onCalendarHidden() {
            var cal = $find("calendar1");
            //Iterate every month Item and remove click event from it
            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.removeHandler(row.cells[j].firstChild, "click", call);
                    }
                }
            }
        }

        function call(eventElement) {
            var target = eventElement.target;
            switch (target.mode) {
                case "month":
                    var cal = $find("calendar1");
                    cal._visibleDate = target.date;
                    cal.set_selectedDate(target.date);
                    cal._switchMonth(target.date);
                    cal._blur.post(true);
                    cal.raiseDateSelectionChanged();
                    break;
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <center>
        <table width="50%">
            <tr>
                <td>
                    Client: 
                </td>
                <td>
                    <asp:DropDownList ID="ddlCCE" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCCE_SelectedIndexChanged"></asp:DropDownList>
                </td>

                 <td>
                    Month: 
                </td>
                <td>
                    <asp:TextBox ID="txtMonth" runat="server" AutoPostBack="True" onKeyPress="javascript: return false;"
                            onKeyDown="javascript: return false;" onPaste="javascript: return false;" OnTextChanged="txtMonth_TextChanged"></asp:TextBox>
                        <cc1:CalendarExtenderPlus ID="txtMonth_CalendarExtenderPlus" runat="server" Format="MMM-yyyy"
                            TargetControlID="txtMonth" Enabled="true" PopupButtonID="txtMonth" OnClientShown="onCalendarShown"
                            OnClientHidden="onCalendarHidden" BehaviorID="calendar1">
                        </cc1:CalendarExtenderPlus>
                </td>
            </tr>
        </table>
        <br />
        <asp:GridView ID="GVInvoiceFiles" runat="server" AutoGenerateColumns="False" EnableModelValidation="True">
            <Columns>
                <asp:TemplateField HeaderText="Invoice Number">                    
                    <ItemTemplate>
                        <asp:Label ID="lblInvNum" runat="server" Text='<%#Bind("InvoiceNumber") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>

            <Columns>
                <asp:TemplateField HeaderText="Invoice Date">                    
                    <ItemTemplate>
                        <asp:Label ID="lblInvDate" runat="server" Text='<%#Bind("InvoiceDate","{0:dd, MMM yyyy}") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>

             <Columns>
                <asp:TemplateField HeaderText="Client Name">                    
                    <ItemTemplate>
                        <asp:Label ID="lblClientName" runat="server" Text='<%#Bind("ClientName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>

            <Columns>
                <asp:TemplateField HeaderText="Invoice File Name">                    
                    <ItemTemplate>
                        <asp:Label ID="lblInvFName" runat="server" Text='<%#Bind("FileName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>

            <Columns>
                <asp:TemplateField HeaderText="Attachment Id">                    
                    <ItemTemplate>
                        <asp:Label ID="lblAttNum" runat="server" Text='<%#Bind("AttID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>

            <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
        </asp:GridView>

        <asp:Button ID="btnDownload" runat="server" Text="Download" OnClick="btnDownload_Click" />
    </center>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder4" Runat="Server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ContentPlaceHolder5" Runat="Server">
</asp:Content>

