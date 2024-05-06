<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true"
    CodeFile="AboutUs.aspx.cs" Inherits="AboutUs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <center>
        <div id="DivHeading">
            About Us
        </div>
    </center>
    <div>
        <div id="aboutusDIV">
            <%--<center>
                <div id="DivHeading" style="padding-top: 15px;">
                    <asp:Label ID="Label2" runat="server" Text="
                        Developed and Maintained by System and Software Department (SSD) Team - Mangalam
                        Information Technologies Pvt. Ltd." Font-Size="14px"></asp:Label>
                </div>
            </center>--%>
            <b>
                <table>
                    <tr>
                        <td>
                            1.
                        </td>
                        <td>
                            <asp:Image runat="server" ID="imgMayur" ImageUrl="~/images/user.gif" Height="50%"
                                Width="50%" />
                        </td>
                        <td>
                            Mayur B. Mehta
                        </td>
                        <td>
                            Software Developer (M.C.A.)
                        </td>
                        <td>
                            mayur.mehta@mangalaminfotech.com
                        </td>
                    </tr>
                    <tr>
                        <td>
                            2.
                        </td>
                        <td>
                            <asp:Image runat="server" ID="imgJinal" ImageUrl="~/images/user.gif" Height="50%"
                                Width="50%" />
                        </td>
                        <td>
                            Jinal N. Jhaveri
                        </td>
                        <td>
                            Software Developer (B.E. Computer)
                        </td>
                        <td>
                            system@mangalaminfotech.net
                        </td>
                    </tr>
                    <tr>
                        <td>
                            3.
                        </td>
                        <td style="width: 20%">
                            <asp:Image runat="server" ID="imgManish" ImageUrl="~/images/user.gif" Height="50%"
                                Width="50%" />
                        </td>
                        <td>
                            Mahendra L. Kanzariya
                        </td>
                        <td>
                            Software Trainee (M.C.A.)
                        </td>
                        <td>
                            system@mangalaminfotech.net
                        </td>
                    </tr>
                </table>
                <div style="margin-top: 35px;">
                    <marquee>                 
                 This web-application is for internal use only</marquee>
                </div>
            </b>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder4" runat="Server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ContentPlaceHolder5" runat="Server">
</asp:Content>
