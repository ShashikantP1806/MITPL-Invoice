<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true"
    CodeFile="InvSearch.aspx.cs" Inherits="InvSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <center>
        <div id="DivHeading">
            Invoice Search
        </div>
        <asp:UpdatePanel ID="uppp" runat="server">
            <ContentTemplate>
                <div>
                    <asp:GridView ID="gvSearchInv" runat="server" OnPageIndexChanging="gvSearchInv_PageIndexChanging"
                        OnRowDataBound="gvSearchInv_RowDataBound" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="InvId" Visible="false" />
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblId" runat="server" Text='<%#Eval ("InvId") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Invoice Number">
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkInv" runat="server" Text='<%# Eval ("InvoiceNum").ToString()+ HighlightText(Eval("InvSeq").ToString()) %>'
                                        Target="_blank" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder4" runat="Server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ContentPlaceHolder5" runat="Server">
</asp:Content>
