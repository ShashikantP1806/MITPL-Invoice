using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Data;

public partial class InvSearch : System.Web.UI.Page
{
    MITInvoiceDataContext dbobj = new MITInvoiceDataContext();

    private string SearchString = "";


    protected void Page_Load(object sender, EventArgs e)
    {
        if (Global.UserType != "ADMINISTRATOR")
        {
            if (!IsPostBack)
            {
                if (Request.Url.ToString().Contains("?"))
                {
                    string QueryString = (Request.Url.ToString().Substring(Request.Url.ToString().IndexOf("?") + 1, Request.Url.ToString().Length - (Request.Url.ToString().IndexOf("?") + 1))).Replace(" ", "+");
                    QueryString = Global.Decrypt(QueryString);
                    string QS0 = QueryString.Substring(4, QueryString.Length - 4);
                    {
                        //string qs = Request.QueryString["inv"].Replace(" ", "+");
                        SearchInvoice(QS0);
                        //SearchInvoice(Convert.ToInt64(Global.Decrypt(Request.QueryString[0]))); //.Replace(" ", "+")        
                        TextBox txt = (TextBox)this.Master.FindControl("txtSearch");
                        txt.Text = QS0;

                        SearchString = QS0;
                        SearchInvoice(SearchString);
                    }
                }
            }
        }
        else
        {
            Response.Redirect("Authorize");
        }
    }

    protected void SearchInvoice(string SeqNo)
    {
        //// 25-Aug-2020 By Jignesh
        //// added && (dbClient.C_M1 == Global.UserM1 || dbClient.C_M2 == Global.UserM2)

        var Search = from dbInvData in dbobj.InvoiceMasters
                     join dbClient in dbobj.ClientMasters
                     on dbInvData.ClientId equals dbClient.ClientId
                     where (dbInvData.InvoiceSeqNo.ToString().Contains(SeqNo.ToString()) || (dbInvData.InvoiceSeqNo.ToString() + dbInvData.Revision.ToString()).Contains(SeqNo)) && Convert.ToInt64(Global.Department) == dbClient.DepartmentId && dbInvData.IsDeleted == false && (dbClient.C_M1 == Global.UserM1 || dbClient.C_M2 == Global.UserM2)
                     select new
                     {
                         InvoiceNum = dbInvData.InvoiceNumber.Substring(0, dbInvData.InvoiceNumber.LastIndexOf("/") + 1),
                         InvSeq = dbInvData.Revision == null ? dbInvData.InvoiceSeqNo.ToString().PadLeft(3, '0') : dbInvData.InvoiceSeqNo.ToString().PadLeft(3, '0') + dbInvData.Revision,
                         InvId = dbInvData.InvoiceId
                     };
        if (Search.Count() > 0)
        {
            gvSearchInv.DataSource = Search;
            gvSearchInv.DataBind();
        }
        else
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("InvoiceNum");
            dt.Columns.Add("InvSeq");
            dt.Columns.Add("InvId");
            gvSearchInv.DataSource = dt;
            dt.Rows.Add(dt.NewRow());
            gvSearchInv.DataBind();
            int TotalCols = gvSearchInv.Rows[0].Cells.Count;
            gvSearchInv.Rows[0].Cells.Clear();
            gvSearchInv.Rows[0].Cells.Add(new TableCell());
            gvSearchInv.Rows[0].Cells[0].ColumnSpan = TotalCols;
            gvSearchInv.Rows[0].Cells[0].Text = "No records to display";
        }
    }

    protected void gvSearchInv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvSearchInv.PageIndex = e.NewPageIndex;
        ////string qs = Request.QueryString[0].Replace(" ", "+");
        ////SearchInvoice(Convert.ToInt64(Global.Decrypt(qs)));

        string QueryString = (Request.Url.ToString().Substring(Request.Url.ToString().IndexOf("?") + 1, Request.Url.ToString().Length - (Request.Url.ToString().IndexOf("?") + 1))).Replace(" ", "+");
        QueryString = Global.Decrypt(QueryString);
        string QS0 = QueryString.Substring(4, QueryString.Length - 4);
        SearchInvoice(QS0);
        TextBox txt = (TextBox)this.Master.FindControl("txtSearch");
        txt.Text = QS0;
    }

    protected void gvSearchInv_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label InvId = (Label)e.Row.FindControl("lblId");
            HyperLink lnkInv = (HyperLink)e.Row.FindControl("lnkInv");
            lnkInv.NavigateUrl = "NewInvoice?InvID=" + Global.Encrypt(InvId.Text);
        }
    }

    public string HighlightText(string InputTxt)
    {
        string QueryString = (Request.Url.ToString().Substring(Request.Url.ToString().IndexOf("?") + 1, Request.Url.ToString().Length - (Request.Url.ToString().IndexOf("?") + 1))).Replace(" ", "+");
        QueryString = Global.Decrypt(QueryString);
        string QS0 = QueryString.Substring(4, QueryString.Length - 4);

        //TextBox txt = (TextBox)this.Master.FindControl("txtSearch");
        string Search_Str = QS0;
        // Setup the regular expression and add the Or operator.
        Regex RegExp = new Regex(Search_Str.Replace(" ", "|").Trim(), RegexOptions.IgnoreCase);
        // Highlight keywords by calling the
        //delegate each time a keyword is found.
        return RegExp.Replace(InputTxt, new MatchEvaluator(ReplaceKeyWords));
    }

    public string ReplaceKeyWords(Match m)
    {
        return ("<span class=highlight>" + m.Value + "</span>");
    }
}
