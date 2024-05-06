<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true"
    CodeFile="Client.aspx.cs" Inherits="Client" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <meta charset="utf-8" />
    <%--<link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />--%>
    <%--<script src="http://code.jquery.com/jquery-1.9.1.js"></script>--%>
    <%--<script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>--%>
    <%--<link href="css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="js/jquery-ui.js" type="text/javascript"></script>
    <link rel="stylesheet" href="/resources/demos/style.css" />--%>
    <%--<style>
        .ui-tooltip, .arrow:after
        {
            background: Green;
            border: 1px solid white;
        }
        .ui-tooltip
        {
            padding: 5px 5px;
            color: white;
            border-radius: 10px;
            font: 12px "Helvetica Neue" , Sans-Serif;
            box-shadow: 0 0 7px black;
        }
        .arrow
        {
            width: 70px;
            height: 16px;
            overflow: hidden;
            position: absolute;
            left: 50%;
            margin-left: -35px;
            bottom: -16px;
        }
        .arrow.top
        {
            top: -16px;
            bottom: auto;
        }
        .arrow.left
        {
            left: 20%;
        }
        .arrow:after
        {
            content: "";
            position: absolute;
            left: 20px;
            top: -20px;
            width: 25px;
            height: 25px;
            box-shadow: 6px 5px 9px -9px black;
            -webkit-transform: rotate(45deg);
            -moz-transform: rotate(45deg);
            -ms-transform: rotate(45deg);
            -o-transform: rotate(45deg);
            tranform: rotate(45deg);
        }
        .arrow.top:after
        {
            bottom: -20px;
            top: auto;
        }
    </style>--%>

    <script>
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:UpdatePanel ID="up1" UpdateMode="Conditional" runat="server" RenderMode="Inline" ChildrenAsTriggers="true" EnableViewState="false">--%>
    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" ChildrenAsTriggers="true">--%>
    <%--<ContentTemplate>--%>


    <asp:MultiView ID="mvClient" runat="server">
        <asp:View ID="viewmanageClient" runat="server">
            <center>
                <div id="DivHeading">
                    Manage Client
                </div>
            </center>
            <table width="100%">
                <tr>
                    <td align="left">
                        <asp:Button ID="btnNewClient" runat="server" Text="New Client" ToolTip="Add new client"
                            OnClick="btnNewClient_Click" />
                        <asp:Label ID="lblError2" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                    </td>
                    <td align="right">Department :&nbsp;<asp:DropDownList ID="ddlDepartmentSearch" runat="server" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlDepartmentSearch_SelectedIndexChanged">
                    </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="gridClient" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            OnPageIndexChanging="gridClient_PageIndexChanging" OnRowCommand="gridClient_RowCommand"
                            OnRowDeleting="gridClient_RowDeleting" ShowFooter="false" Width="100%" OnRowDataBound="gridClient_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSr" runat="server" Text="<%#Container.DataItemIndex + 1 %>"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Client Id" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblClientId" runat="server" Text='<%#Eval("ClientId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Client Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblClientName1" runat="server" Text='<%#HighlightText(Eval("ClientName").ToString()) %>'></asp:Label>
                                        <asp:Label ID="lblClientName" runat="server" Text='<%#Eval("ClientName") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Department Id" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDepartmentId" runat="server" Text='<%#Eval("DepartmentId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Department">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDepartment" runat="server" Text='<%#Eval("Department") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Phone">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPhone" runat="server" Text='<%#Eval("Phone") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Address1">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAddress1" runat="server" Text='<%#Eval("Address1") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Address2">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAddress2" runat="server" Text='<%#Eval("Address2") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Website">
                                    <ItemTemplate>
                                        <asp:Label ID="lblWebsite" runat="server" Text='<%#Eval("Website") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ControlStyle Width="250px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Currency">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCurrency" runat="server" Text='<%#Eval("Currency") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ControlStyle Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Contact">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtnAddContact" runat="server" Text="Add/View" OnClick="lbtnAddContact_Click"></asp:LinkButton>
                                    </ItemTemplate>
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
        <asp:View ID="viewAddClient" runat="server">
            <center>
                <div id="DivHeading">
                    Add Client
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
                        <td>Client Name :
                                    <asp:Label ID="lblrqName" runat="server" Text="*" ForeColor="Blue"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtClientName" runat="server" MaxLength="600" ToolTip="Client Name"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rqClientName" runat="server" Text="*" ControlToValidate="txtClientName"
                                ToolTip="Required" ErrorMessage="Please enter client name" SetFocusOnError="true"
                                ValidationGroup="vg"></asp:RequiredFieldValidator>
                        </td>
                        <td>Department :
                                    <asp:Label ID="lblrqDepartment" runat="server" Text="*" ForeColor="Blue"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDepartment" runat="server" ToolTip="Select department">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rqDepartment" runat="server" Text="*" ControlToValidate="ddlDepartment"
                                ErrorMessage="Please select department" SetFocusOnError="true" InitialValue="-- Select --"
                                ToolTip="Required" ValidationGroup="vg"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Primary e-mail :
                                    <asp:Label ID="lblrqemail" runat="server" Text="*" ForeColor="Blue"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPrimaryEmail" runat="server" MaxLength="150" ToolTip="PrimaryEmail"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rqtxtPrimaryEmail" runat="server" ControlToValidate="txtPrimaryEmail"
                                SetFocusOnError="true" Text="*" ErrorMessage="Please enter e-mail" ValidationGroup="vg"
                                ToolTip="Required"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="retxtPrimaryEmail" runat="server" ControlToValidate="txtPrimaryEmail"
                                Text="*" ErrorMessage="Please enter valid e-mail" ToolTip="Invalid email" ValidationGroup="vg"
                                ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" SetFocusOnError="True"></asp:RegularExpressionValidator>
                        </td>
                        <td>Skype ID :
                        </td>
                        <td>
                            <asp:TextBox ID="txtSkypeId" runat="server" MaxLength="100" ToolTip="SkypeID"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Recipient e-mail:                              
                        </td>
                        <td>
                            <%--This is default 'Recipient Email' added on 29-May-2019 by Jignesh--%>
                            <%--<asp:TextBox ID="txtRecipientEmail" runat="server" MaxLength="150" ToolTip="Recipient e-mail"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="revDefaultRecipient" runat="server" ControlToValidate="txtRecipientEmail"
                                Text="*" ErrorMessage="Please enter valid e-mail" ToolTip="Invalid email" ValidationGroup="vg"
                                ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" SetFocusOnError="True"></asp:RegularExpressionValidator>--%>

                            <asp:TextBox ID="txtRecipientEmail" runat="server" MaxLength="150" ToolTip="Recipient e-mail"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="revDefaultRecipient" runat="server" ControlToValidate="txtRecipientEmail"
                                Text="*" ErrorMessage="Please enter valid e-mail" ToolTip="Invalid email" ValidationGroup="vg"
                                ValidationExpression="^((\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)\s*[;]{0,1}\s*)+$" SetFocusOnError="True"></asp:RegularExpressionValidator>
                        </td>
                        <td>CC e-mail:
                        </td>
                        <td>
                            <%--This is default 'CC Email' added on 29-May-2019 by Jignesh--%>
                            <%--<asp:TextBox ID="txtCCEmail" runat="server" MaxLength="100" ToolTip="CC e-mail"></asp:TextBox>
                             <asp:RegularExpressionValidator ID="revDefaultCC" runat="server" ControlToValidate="txtCCEmail"
                                Text="*" ErrorMessage="Please enter valid e-mail" ToolTip="Invalid email" ValidationGroup="vg"
                                ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" SetFocusOnError="True"></asp:RegularExpressionValidator>--%>

                            <asp:TextBox ID="txtCCEmail" runat="server" MaxLength="150" ToolTip="CC e-mail"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="revDefaultCC" runat="server" ControlToValidate="txtCCEmail"
                                Text="*" ErrorMessage="Please enter valid e-mail" ToolTip="Invalid email" ValidationGroup="vg"
                                ValidationExpression="^((\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)\s*[;]{0,1}\s*)+$" SetFocusOnError="True"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Phone :
                                    <asp:Label ID="lblrqPhone" runat="server" Text="*" ForeColor="Blue"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPhone" runat="server" MaxLength="15" ToolTip="Phone number"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rqPhone" runat="server" Text="*" ControlToValidate="txtPhone"
                                ToolTip="Required" ErrorMessage="Please enter phone number" SetFocusOnError="true"
                                ValidationGroup="vg"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revPhone" runat="server" ControlToValidate="txtPhone"
                                ErrorMessage="Telephone number must be 7-15 digit" ValidationExpression="^[0-9]{7,15}$"
                                Text="*" ValidationGroup="vg" SetFocusOnError="true" ToolTip="Telephone number must be 7-15 digit"></asp:RegularExpressionValidator>
                        </td>
                        <td>Mobile :
                        </td>
                        <td>
                            <asp:TextBox ID="txtMobile" runat="server" MaxLength="15" ToolTip="Mobile number"></asp:TextBox>&nbsp;<asp:Label
                                ID="lblMobileNumberFormat" runat="server" Text="(+91-9712345678)" Font-Size="Small"></asp:Label>
                            <asp:RegularExpressionValidator ID="revMobile" runat="server" ControlToValidate="txtMobile"
                                ErrorMessage="Enter valid mobile number" ValidationExpression="^(\+\d{1,3})\-\d{10,15}?$"
                                Text="*" ValidationGroup="vg" SetFocusOnError="true" ToolTip="Mobile number should be (e.g. +91 and mobile number)"></asp:RegularExpressionValidator><br />
                        </td>
                    </tr>
                    <tr>
                        <td>Fax :
                        </td>
                        <td>
                            <asp:TextBox ID="txtFax" runat="server" MaxLength="15" ToolTip="Fax number"></asp:TextBox>
                        </td>
                        <td>Currency :
                                    <asp:Label ID="lblrqCurrency" runat="server" Text="*" ForeColor="Blue"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCurrency" runat="server">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rqCurrency" runat="server" Text="*" ControlToValidate="ddlCurrency"
                                ErrorMessage="Please select currency" SetFocusOnError="true" InitialValue="-- Select --"
                                ToolTip="Required" ValidationGroup="vg"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <b>Address 1</b>
                        </td>
                        <td colspan="2">
                            <b>Address 2</b>
                        </td>
                    </tr>
                    <tr>
                        <td>Address :
                                    <asp:Label ID="lblrqAddress" runat="server" Text="*" ForeColor="Blue"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAddress1" runat="server" TextMode="MultiLine" MaxLength="250"
                                Height="50" Width="200" ToolTip="First address"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rqAddress" runat="server" Text="*" ToolTip="Required"
                                ControlToValidate="txtAddress1" ErrorMessage="Please enter client's address"
                                SetFocusOnError="true" ValidationGroup="vg"></asp:RequiredFieldValidator>
                        </td>
                        <td>Address :
                        </td>
                        <td>
                            <asp:TextBox ID="txtAddress2" runat="server" TextMode="MultiLine" MaxLength="250"
                                Height="50" Width="200" ToolTip="Second address"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Country :
                                    <asp:Label ID="lblrqCountry" runat="server" Text="*" ForeColor="Blue"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCountry1" runat="server" ToolTip="Select country" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlCountry1_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rqCountry" runat="server" Text="*" ControlToValidate="ddlCountry1"
                                ErrorMessage="Please select client's country" SetFocusOnError="true" InitialValue="-- Select --"
                                ToolTip="Required" ValidationGroup="vg"></asp:RequiredFieldValidator>
                            <asp:LinkButton ID="lbtnAddCountry" runat="server" Text="Add" ToolTip="Add country"
                                OnClick="lbtnAddCountry_Click"></asp:LinkButton>
                            <asp:Label ID="lblAddDelCountry" runat="server" Text="/" Visible="false"></asp:Label>
                            <asp:LinkButton ID="lbtnDelCountry" runat="server" Text="Del" ToolTip="Delete country"
                                OnClick="lbtnDelCountry_Click" Visible="false"></asp:LinkButton>
                        </td>
                        <td>Country :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCountry2" runat="server" ToolTip="Select country" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlCountry2_SelectedIndexChanged">
                            </asp:DropDownList>
                            &nbsp;&nbsp;
                                    <asp:LinkButton ID="lbtnAddCountry2" runat="server" Text="Add" ToolTip="Add country"
                                        OnClick="lbtnAddCountry_Click"></asp:LinkButton>
                            <asp:Label ID="lblAddDelCountry2" runat="server" Text="/" Visible="false"></asp:Label>
                            <asp:LinkButton ID="lbtnDelCountry2" runat="server" Text="Del" ToolTip="Delete country"
                                OnClick="lbtnDelCountry2_Click" Visible="false"></asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td>State :
                                    <asp:Label ID="lblrqState" runat="server" Text="*" ForeColor="Blue"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlState1" runat="server" ToolTip="Select state" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlState1_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rqState" runat="server" Text="*" ControlToValidate="ddlState1"
                                ErrorMessage="Please select client's state" SetFocusOnError="true" InitialValue="-- Select --"
                                ToolTip="Required" ValidationGroup="vg"></asp:RequiredFieldValidator>
                            <asp:LinkButton ID="lbtnAddState" runat="server" Text="Add" ToolTip="Add state" Visible="false"
                                OnClick="lbtnAddState_Click"></asp:LinkButton>
                            <asp:Label ID="lblAddDelState1" runat="server" Text="/" Visible="false"></asp:Label>
                            <asp:LinkButton ID="lbtnDelState1" runat="server" Text="Del" ToolTip="Delete state"
                                OnClick="lbtnDelState_Click" Visible="false"></asp:LinkButton>
                        </td>
                        <td>State :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlState2" runat="server" ToolTip="Select state" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlState2_SelectedIndexChanged">
                            </asp:DropDownList>
                            &nbsp;&nbsp;
                                    <asp:LinkButton ID="lbtnAddState2" runat="server" Text="Add" ToolTip="Add state"
                                        Visible="false" OnClick="lbtnAddState2_Click"></asp:LinkButton>
                            <asp:Label ID="lblAddDelState2" runat="server" Text="/" Visible="false"></asp:Label>
                            <asp:LinkButton ID="lbtnDelState2" runat="server" Text="Del" ToolTip="Delete state"
                                OnClick="lbtnDelState2_Click" Visible="false"></asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td>City :
                                    <asp:Label ID="lblrqCity" runat="server" Text="*" ForeColor="Blue"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCity1" runat="server" ToolTip="Select city" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlCity1_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rqCity" runat="server" Text="*" ControlToValidate="ddlCity1"
                                ErrorMessage="Please select client's city" SetFocusOnError="true" InitialValue="-- Select --"
                                ToolTip="Required" ValidationGroup="vg"></asp:RequiredFieldValidator>
                            <asp:LinkButton ID="lbtnAddCity" runat="server" Text="Add" ToolTip="Add city" Visible="false"
                                OnClick="lbtnAddCity_Click"></asp:LinkButton>
                            <asp:Label ID="lblAddDelCity1" runat="server" Text="/" Visible="false"></asp:Label>
                            <asp:LinkButton ID="lbtnDelCity1" runat="server" Text="Del" ToolTip="Delete city"
                                OnClick="lbtnDelCity1_Click" Visible="false"></asp:LinkButton>
                        </td>
                        <td>City :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCity2" runat="server" ToolTip="Select city" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlCity2_SelectedIndexChanged">
                            </asp:DropDownList>
                            &nbsp;&nbsp;
                                    <asp:LinkButton ID="lbtnAddCity2" runat="server" Text="Add" ToolTip="Add city" Visible="false"
                                        OnClick="lbtnAddCity2_Click"></asp:LinkButton>
                            <asp:Label ID="lblAddDelCity2" runat="server" Text="/" Visible="false"></asp:Label>
                            <asp:LinkButton ID="lbtnDelCity2" runat="server" Text="Del" ToolTip="Delete city"
                                OnClick="lbtnDelCity2_Click" Visible="false"></asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td>Zip/Postal Code :
                        </td>
                        <td>
                            <asp:TextBox ID="txtZipCode1" runat="server" MaxLength="10" ToolTip="Zip/Postal code"></asp:TextBox>
                        </td>
                        <td>Zip/Postal Code :
                        </td>
                        <td>
                            <asp:TextBox ID="txtZipCode2" runat="server" MaxLength="10" ToolTip="Zip/Postal Code"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Website :
                        </td>
                        <td>
                            <asp:TextBox ID="txtWebsite" runat="server" MaxLength="250" ToolTip="Website URL"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblGSTIN" runat="server" Text="GSTIN : " ></asp:Label>
                            <asp:Label ID="lblGSTINReq" runat="server" Text="*" ForeColor="Blue"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtGSTIN" runat="server" MaxLength="15" ToolTip="GSTIN Number"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rqGSTIN" runat="server" Text="*" ControlToValidate="txtGSTIN"
                                ToolTip="Required" ErrorMessage="Please enter GSTIN number" SetFocusOnError="true"
                                ></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revGSTIN" Display="Dynamic" ControlToValidate = "txtGSTIN" ValidationExpression = "^[\s\S]{15,}$" 
                                runat="server" ErrorMessage="Minimum 15 characters required." ></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Remarks :
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Height="60" Width="300"
                                ToolTip="Remarks"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblClient_M1" runat="server" Text="M1 :"></asp:Label>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkClient_M1" runat="server" />
                        </td>
                        <td>
                            <asp:Label ID="lblClient_M2" runat="server" Text="M2 :"></asp:Label>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkClient_M2" runat="server" />
                        </td>
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
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="lbtnAddState" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="lbtnAddCity" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="lbtnAddCountry" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="lbtnAddCity2" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="lbtnAddState2" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="lbtnAddCountry2" EventName="Click" />
                </Triggers>
                <ContentTemplate>
                    <asp:Button ID="btnShowPopup" runat="server" Text="fdfdfsfd" Style="display: none" />
                    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" PopupControlID="pnlpopup"
                        TargetControlID="btnShowPopup" BackgroundCssClass="ModalPopupBG">
                    </asp:ModalPopupExtender>
                    <asp:Panel ID="pnlpopup" runat="server" CssClass="modalBackground" Style="display: none;">


                        <table width="100%">
                            <tr>
                                <td colspan="2" align="center">
                                    <h3>
                                        <asp:Label ID="lblTitle" runat="server" ForeColor="#0D6895"></asp:Label>
                                    </h3>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="left">&nbsp;&nbsp;<asp:Label ID="lblName" runat="server" ForeColor="Black"></asp:Label>
                                </td>
                                <td align="left">&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;<asp:TextBox ID="txtName" runat="server" Width="200px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rqName" runat="server" ControlToValidate="txtName"
                                        ErrorMessage="Enter value" Text="*" ValidationGroup="rqName" SetFocusOnError="true"
                                        ToolTip="Required"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Button ID="btnOk" runat="server" OnClick="btnOk_Click" Text="Save" ValidationGroup="rqName" />
                                    <asp:Button ID="btnMPCancel" runat="server" Text="Cancel" />
                                </td>
                            </tr>
                        </table>

                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:View>
    </asp:MultiView>
    <%--</ContentTemplate>--%>
    <%--</asp:UpdatePanel>--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder4" runat="Server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ContentPlaceHolder5" runat="Server">
</asp:Content>
