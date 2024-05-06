<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true"
    CodeFile="Home.aspx.cs" Inherits="Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

  <%--  <script type="text/javascript">

        var myVar = setInterval(function() { callTest() }, 30000);
        function callTest() {
            $.ajax({
                type: "POST",
                url: "Home.aspx/USD",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(msg) {
                    var lbl = document.getElementById("<%=lblUSD.ClientID %>");
                    if (lbl != null)
                        lbl.innerText = msg.d;
                    else
                        alert(msg.d);
                }
            });
        }
    </script>

    <script type="text/javascript">

        var myVar = setInterval(function() { callTest2() }, 30000);
        function callTest2() {
            $.ajax({
                type: "POST",
                url: "Home.aspx/GBP",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(msg2) {
                    var lbl2 = document.getElementById("<%=lblGBP.ClientID %>");
                    if (lbl2 != null)
                        lbl2.innerText = msg2.d;
                    else
                        alert(msg2.d);
                }
            });
        }
    </script>--%>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <center>
        <div id="DivHeading">
            Home
        </div>
    </center>
    <table width="100%">
        <tr id="trerror" runat="server" visible="false">
            <td colspan="2">
                <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 85%; text-align: left; vertical-align: top; padding-left: 20px;">
                Welcome
                <asp:Label ID="lblUserName" runat="server" Text="MITPL Invoice"></asp:Label>
                <p>
                    <asp:Label ID="lblUserMsg" runat="server"></asp:Label></p>
                The MITPL e-Invoice System is a basic invoicing system for you to invoice customers
                for services or products using email.<br />
                <br />
                <b>Main Features</b>
                <ol style="padding-left: 35px;">
                    <li>Simple to use.</li>
                    <li>Add as many clients as needed.</li>
                    <li>Add as many invoices for each customer as needed.</li>
                    <li>Invoices self total the inputted data.</li>
                    <li>You can search through customers or invoices for keywords.</li>
                    <%--<li>Automatic Email Paid Invoice system you can turn on or off.</li>--%>
                    <li>Option to email client or not, also option for viewing invoice as it will view to
                        client.</li>
                    <li>PDF, Excel invoice view.</li>
                    <%--<li>A language file you can edit to change all words on invoice. Like QTY to Rate etc...</li>--%>
                </ol>
                <br />
                We would request you to review this web application and provide us your valuable
                feedback to<a href="mailto:system@mangalaminfotech.net"> system@mangalaminfotech.net</a>.<br />
                <br />
             <%--   <center>
                    <table style="border-color: Black; border-width: thin;">
                        <tr>
                            <td align="center">
                                Pacific Time
                            </td>
                            <td align="center">
                                Central Time
                            </td>
                            <td align="center">
                                Eastern Time
                            </td>
                            <td align="center">
                                UTC / GMT
                            </td>
                            <td align="center">
                                1 USD
                            </td>
                            <td align="center">
                                1 GBP
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <iframe src="http://free.timeanddate.com/clock/i3umg0kq/n137/fn17/fs10/fcfff/tc5d7b9d/pct/ftb/bas2/pa8/tt0/tw0/tm1/th2/tb4"
                                    frameborder="0" width="95" height="44" allowtransparency="true"></iframe>
                            </td>
                            <td>
                                <iframe src="http://free.timeanddate.com/clock/i3umgg9g/n155/fn17/fs10/fcfff/tc5d7b9d/ftb/bas2/pa8/tt0/tw0/tm1/th2/tb4"
                                    frameborder="0" width="95" height="44"></iframe>
                            </td>
                            <td>
                                <iframe src="http://free.timeanddate.com/clock/i3umgg9g/n179/fn17/fs10/fcfff/tc5d7b9d/ftb/bas2/pa8/tt0/tw0/tm1/th2/tb4"
                                    frameborder="0" width="95" height="44"></iframe>
                            </td>
                            <td>
                                <iframe src="http://free.timeanddate.com/clock/i3umgg9g/fn17/fs10/fcfff/tc5d7b9d/ftb/bas2/pa8/tt0/tw0/tm1/th2/tb4"
                                    frameborder="0" width="95" height="44"></iframe>
                            </td>
                            <td align="center">
                                <div class="Currency">
                                    <asp:Label ID="lblUSD" runat="server"></asp:Label></div>
                            </td>
                            <td align="center">
                                <div class="Currency">
                                    <asp:Label ID="lblGBP" runat="server"></asp:Label></div>
                            </td>
                        </tr>
                    </table>
                </center>--%>
                <br />
                <%--<br />
                Thanks and Regards,<br />
                System and Software Department (SSD) Team,<br />
                Mangalam Information Technologies Pvt. Ltd.--%>
            </td>
            <td align="right">
                <div id="divMarquee" runat="server" style="display: block; float: right">
                    <div id="SpecialDiv" style="border-color: #5D7B9D">
                        <div id="DivHeading2">
                            <asp:Label ID="lblInvoice" runat="server" Text="Draft Invoice"></asp:Label>
                        </div>
                        <div id="divNewRequest" runat="server" style="display: inline-block; width: 100%;
                            position: relative;">
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder4" runat="Server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ContentPlaceHolder5" runat="Server">
</asp:Content>
