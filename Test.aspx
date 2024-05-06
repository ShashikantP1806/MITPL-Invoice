<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true"
    CodeFile="Test.aspx.cs" Inherits="Test" %>

<%@ Register Assembly="CalendarExtenderPlus" Namespace="AjaxControlToolkitPlus" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <center>
        <div id="DivHeading">
            View Invoice
        </div>
    </center>
    <table id="tbldrp" runat="server">
        <tr>
            <td>
                Business Unit:
                <asp:DropDownList ID="drpBusiness" AutoPostBack="true" runat="server">
                </asp:DropDownList>
            </td>
            <td>
                Invoice Status:
                <asp:DropDownList ID="drpInvoiceStatus" AutoPostBack="true" runat="server">
                    <asp:ListItem Selected="True">Draft</asp:ListItem>
                    <asp:ListItem>Paid</asp:ListItem>
                    <asp:ListItem>Unpaid</asp:ListItem>
                    <asp:ListItem>Revised</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                Client:
                <asp:DropDownList ID="ddlClient" runat="server" AutoPostBack="true">
                </asp:DropDownList>
            </td>
            <td>
                <asp:Button ID="btnIntimate" runat="server" Text="Intimate" Enabled="false" Visible="false" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lbltest" runat="server" Visible="false"></asp:Label>
            </td>
        </tr>
    </table>
    <table>
        <asp:GridView ID="grdInvoices" runat="server" ShowFooter="false" AllowPaging="true"
            PageSize="10" AutoGenerateColumns="true">
            <Columns>
                <asp:TemplateField HeaderText="d">
                    <ItemTemplate>
                        <asp:Label ID="lbldfd" runat="server">sdflkd</asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="d">
                    <ItemTemplate>
                        <asp:Label ID="lbldfd" runat="server">sdflkd</asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="d">
                    <ItemTemplate>
                        <asp:Label ID="lbldfd" runat="server">sdflkd</asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
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
