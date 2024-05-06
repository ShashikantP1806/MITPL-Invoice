<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true"
    CodeFile="MonthlySales.aspx.cs" Inherits="MonthlySales" %>

<%@ Register Assembly="CalendarExtenderPlus" Namespace="AjaxControlToolkitPlus" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

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

    <style type="text/css">
        .bkcolor
        {
            /*background-color: #ECF3FB;*/
            background-color: Aqua;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExcel" />
        </Triggers>
        <ContentTemplate>
            <center>
                <div id="DivHeading">
                    SALES INVOICES FOR MANGALAM INFOTECCH USA
                </div>
            </center>
            <table style="width:100%">
                <tr>
                    <td align="left">
                        Month :
                        <asp:TextBox ID="txtMonth" runat="server" AutoPostBack="True" onKeyPress="javascript: return false;"
                            onKeyDown="javascript: return false;" onPaste="javascript: return false;" 
                            ontextchanged="txtMonth_TextChanged"></asp:TextBox>
                        <cc1:CalendarExtenderPlus ID="txtMonth_CalendarExtenderPlus" runat="server" Format="MMM-yyyy"
                            TargetControlID="txtMonth" Enabled="true" PopupButtonID="txtMonth" OnClientShown="onCalendarShown"
                            OnClientHidden="onCalendarHidden" BehaviorID="calendar1">
                        </cc1:CalendarExtenderPlus>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Button ID="btnExcel" runat="server" Text="Excel" Enabled="false" 
                            onclick="btnExcel_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div style="overflow: auto; width: 100%">
                            <asp:GridView ID="gvMonthlySales" runat="server" AutoGenerateColumns="true" AllowPaging="false">
                            </asp:GridView>
                        </div>
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
