<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CurrencyHelp.aspx.cs" Inherits="CurrencyHelp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MITPL e-Invoice System</title>
    <link id="Link1" runat="server" rel="shortcut icon" href="images/NewLogo.ico" type="image/x-icon" />
    <link href="css/style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <br />
    <center>
    
        <div id="DivHeading">
            Currency Symbol Code
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
                <table width="100%" align="center">
                    <tr>
                        <th align="center">
                            Symbol
                        </th>
                        <th align="left">
                            Code
                        </th>
                        <th align="left">
                            Description
                        </th>
                        <th align="center">
                            Symbol
                        </th>
                        <th align="left">
                            Code
                        </th>
                        <th align="left">
                            Description
                        </th>
                    </tr>
                    <tr>
                        <td align="center">
                            &#x20b9;
                        </td>
                        <td align="left">
                            x20B9;
                        </td>
                        <td align="left">
                            INDIAN RUPEE
                        </td>
                        <td align="center">
                            &#x20A0;
                        </td>
                        <td align="left">
                            x20A0;
                        </td>
                        <td align="left">
                            EURO-CURRENCY
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            &#x20A1;
                        </td>
                        <td align="left">
                            x20A1;
                        </td>
                        <td align="left">
                            COLON
                        </td>
                        <td align="center">
                            &#x20A2;
                        </td>
                        <td align="left">
                            x20A2;
                        </td>
                        <td align="left">
                            CRUZEIRO
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            &#x20A3;
                        </td>
                        <td align="left">
                            x20A3;
                        </td>
                        <td align="left">
                            FRENCH FRANC
                        </td>
                        <td align="center">
                            &#x20A4;
                        </td>
                        <td align="left">
                            x20A4;
                        </td>
                        <td align="left">
                            LIRA
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            &#x20A5;
                        </td>
                        <td align="left">
                            x20A5;
                        </td>
                        <td align="left">
                            MILL
                        </td>
                        <td align="center">
                            &#x20A6;
                        </td>
                        <td align="left">
                            x20A6;
                        </td>
                        <td align="left">
                            NAIRA
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            &#x20A7;
                        </td>
                        <td align="left">
                            x20A7;
                        </td>
                        <td align="left">
                            PESETA
                        </td>
                        <td align="center">
                            &#x20A8;
                        </td>
                        <td align="left">
                            x20A8;
                        </td>
                        <td align="left">
                            RUPEE
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            &#x20A9;
                        </td>
                        <td align="left">
                            x20A9;
                        </td>
                        <td align="left">
                            WON
                        </td>
                        <td align="center">
                            &#x20AA;
                        </td>
                        <td align="left">
                            x20AA;
                        </td>
                        <td align="left">
                            NEW SHEQEL
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            &#x20AB;
                        </td>
                        <td align="left">
                            x20AB;
                        </td>
                        <td align="left">
                            DONG
                        </td>
                        <td align="center">
                            &#x20AC;
                        </td>
                        <td align="left">
                            x20AC;
                        </td>
                        <td align="left">
                            EURO
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            &#x20AD;
                        </td>
                        <td align="left">
                            x20AD;
                        </td>
                        <td align="left">
                            KIP
                        </td>
                        <td align="center">
                            &#x20AE;
                        </td>
                        <td align="left">
                            x20AE;
                        </td>
                        <td align="left">
                            TUGRIK
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            &#x20AF;
                        </td>
                        <td align="left">
                            x20AF;
                        </td>
                        <td align="left">
                            DRACHMA
                        </td>
                        <td align="center">
                            &#x20B0;
                        </td>
                        <td align="left">
                            x20B0;
                        </td>
                        <td align="left">
                            GERMAN PENNY
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            &#x20B1;
                        </td>
                        <td align="left">
                            x20B1;
                        </td>
                        <td align="left">
                            PESO
                        </td>
                        <td align="center">
                            &#x20B4;
                        </td>
                        <td align="left">
                            x20B4;
                        </td>
                        <td align="left">
                            HRYVNIA
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            &#x20B5;
                        </td>
                        <td align="left">
                            x20B5;
                        </td>
                        <td align="left">
                            CEDI
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            &#x0024;
                        </td>
                        <td align="left">
                            xFF04;
                        </td>
                        <td align="left">
                            DOLLAR
                        </td>
                        <td align="center">
                            &#x00A2;
                        </td>
                        <td align="left">
                            x00A2;
                        </td>
                        <td align="left">
                            CENT
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            &#x00A3;
                        </td>
                        <td align="left">
                            x00A3;
                        </td>
                        <td align="left">
                            POUND
                        </td>
                        <td align="center">
                            &#x00A4;
                        </td>
                        <td align="left">
                            x00A4;
                        </td>
                        <td align="left">
                            GENERAL CURRENCY
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            &#x00A5;
                        </td>
                        <td align="left">
                            x00A5;
                        </td>
                        <td align="left">
                            YEN
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" align="left">
                            <b>--> More</b>
                            <br />
                            1. <a href="http://www.currencysymbols.in/">www.currencysymbols.in </a>
                            <br />
                            2. <a href="http://www.alanwood.net/unicode/currency_symbols.html">www.alanwood.net
                            </a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
