using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;

public partial class Currency : System.Web.UI.Page
{
    MITInvoiceDataContext dbobj = new MITInvoiceDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Global.UserType == "ADMINISTRATOR")
        {
            if (!IsPostBack)
            {
                FillGrid();
            }
        }
        else
        {
            Response.Redirect("Authorize");
        }
    }

    public void FillGrid()
    {
        TextBox txtMasterSearch = (TextBox)this.Master.FindControl("txtMasterSearch");
        if (txtMasterSearch.Text == "")
        {
            var Currency = from DBData in dbobj.CurrencyMasters
                           where DBData.IsDeleted == false
                           orderby DBData.CurrencyName
                           select new
                           {
                               CurrencyId = DBData.CurrencyId,
                               CurrencyName = DBData.CurrencyName,
                               CurrencyCode = DBData.CurrencyCode,
                               CurrencySymbol = DBData.CurrencySymbol == "" ? "" : "&#" + DBData.CurrencySymbol
                           };
            if (Currency.Count() > 0)
            {
                gridCurrency.DataSource = Currency;
                gridCurrency.DataBind();
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("CurrencyId");
                dt.Columns.Add("CurrencyName");
                dt.Columns.Add("CurrencyCode");
                dt.Columns.Add("CurrencySymbol");
                gridCurrency.DataSource = dt;
                dt.Rows.Add(dt.NewRow());
                gridCurrency.DataBind();
                int TotalCols = gridCurrency.Rows[0].Cells.Count;
                gridCurrency.Rows[0].Cells.Clear();
                gridCurrency.Rows[0].Cells.Add(new TableCell());
                gridCurrency.Rows[0].Cells[0].ColumnSpan = TotalCols;
                gridCurrency.Rows[0].Cells[0].Text = "No records to display";
            }
        }
        else
        {
            var Currency = from DBData in dbobj.CurrencyMasters
                           where DBData.IsDeleted == false && (DBData.CurrencyName.Contains(txtMasterSearch.Text.Trim()) || DBData.CurrencyCode.Contains(txtMasterSearch.Text.Trim()))
                           orderby DBData.CurrencyName
                           select new
                           {
                               CurrencyId = DBData.CurrencyId,
                               CurrencyName = DBData.CurrencyName,
                               CurrencyCode = DBData.CurrencyCode,
                               CurrencySymbol = DBData.CurrencySymbol == "" ? "" : "&#" + DBData.CurrencySymbol
                           };
            if (Currency.Count() > 0)
            {
                gridCurrency.DataSource = Currency;
                gridCurrency.DataBind();
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("CurrencyId");
                dt.Columns.Add("CurrencyName");
                dt.Columns.Add("CurrencyCode");
                dt.Columns.Add("CurrencySymbol");
                gridCurrency.DataSource = dt;
                dt.Rows.Add(dt.NewRow());
                gridCurrency.DataBind();
                int TotalCols = gridCurrency.Rows[0].Cells.Count;
                gridCurrency.Rows[0].Cells.Clear();
                gridCurrency.Rows[0].Cells.Add(new TableCell());
                gridCurrency.Rows[0].Cells[0].ColumnSpan = TotalCols;
                gridCurrency.Rows[0].Cells[0].Text = "No records to display";
            }
        }
    }

    protected void gridCurrency_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Page page1 = HttpContext.Current.Handler as Page;
        TextBox txtCurrencyName = (TextBox)gridCurrency.FooterRow.FindControl("txtCurrencyName");
        TextBox txtCurrencyCode = (TextBox)gridCurrency.FooterRow.FindControl("txtCurrencyCode");
        TextBox txtCurrencySymbol = (TextBox)gridCurrency.FooterRow.FindControl("txtCurrencySymbol");
        switch (e.CommandName)
        {
            case "Insert":
                var DupCurrency = from DC in dbobj.CurrencyMasters
                                  where DC.CurrencyCode == txtCurrencyCode.Text.Trim().ToUpper() && DC.IsDeleted == false
                                  select DC;
                if (DupCurrency.Count() > 0)
                {
                    ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + txtCurrencyCode.Text.Trim().ToUpper() + " currency code already exists" + "')", true);
                }
                else
                {
                    CurrencyMaster cm = new CurrencyMaster();
                    cm.CurrencyName = txtCurrencyName.Text.Trim();
                    cm.CurrencyCode = txtCurrencyCode.Text.Trim().ToUpper();
                    string str = txtCurrencySymbol.Text.Replace("&", "");
                    cm.CurrencySymbol = str.Replace("#", "");
                    cm.IsDeleted = false;
                    cm.CreatedBy = Convert.ToInt64(Global.UserId);
                    cm.CreatedDate = DateTime.Now;
                    dbobj.CurrencyMasters.InsertOnSubmit(cm);
                    dbobj.SubmitChanges();
                    dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                    ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + txtCurrencyName.Text.Trim().ToUpper() + " currency add successfully" + "')", true);
                    FillGrid();
                }
                break;
            case "Delete":
                GridViewRow gr = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                Label lblCurrencyId = (Label)gr.FindControl("lblCurrencyId");
                Label lblCurrencyName = (Label)gr.FindControl("lblCurrencyName");
                var ClientData = from DBClient in dbobj.ClientMasters
                                 where DBClient.IsActive == true && DBClient.CurrencyId == Convert.ToInt64(lblCurrencyId.Text)
                                 select DBClient;
                if (ClientData.Count() == 0)
                {
                    var DelCurrency = from DelC in dbobj.CurrencyMasters
                                      where DelC.CurrencyId == Convert.ToInt64(lblCurrencyId.Text) && DelC.IsDeleted == false
                                      select DelC;
                    if (DelCurrency.Count() > 0)
                    {
                        var Currency = DelCurrency.Single();
                        Currency.IsDeleted = true;
                        dbobj.SubmitChanges();
                        dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                        //ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + Currency.CurrencyName.ToUpper() + " currency deleted" + "')", true);
                        FillGrid();
                    }
                }
                else
                    OpenWindow(this, "MITPLInvoice", "DeletePopup?id=" + lblCurrencyId.Text + "&name=" + lblCurrencyName.Text + "&page=currency", 650, 350);
                break;
        }
    }

    protected void gridCurrency_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    protected void gridCurrency_RowEditing(object sender, GridViewEditEventArgs e)
    {
        Label lblCurrencySymbol = (Label)gridCurrency.Rows[e.NewEditIndex].FindControl("lblCurrencySymbol");
        gridCurrency.EditIndex = e.NewEditIndex;
        gridCurrency.ShowFooter = false;
        gridCurrency.FooterRow.Visible = false;
        gridCurrency.PagerSettings.Visible = false;
        FillGrid();
        TextBox txtEdtCurrencySymbol = (TextBox)gridCurrency.Rows[e.NewEditIndex].FindControl("txtEdtCurrencySymbol");
        txtEdtCurrencySymbol.Text = txtEdtCurrencySymbol.Text.Replace("&#", "");

    }

    protected void gridCurrency_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gridCurrency.EditIndex = -1;
        gridCurrency.ShowFooter = true;
        gridCurrency.FooterRow.Visible = true;
        gridCurrency.PagerSettings.Visible = true;
        FillGrid();
    }

    protected void gridCurrency_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        Page page1 = HttpContext.Current.Handler as Page;
        Label lblCurrencyId = (Label)gridCurrency.Rows[e.RowIndex].FindControl("lblCurrencyId");
        TextBox txtEdtCurrencyName = (TextBox)gridCurrency.Rows[e.RowIndex].FindControl("txtEdtCurrencyName");
        TextBox txtEdtCurrencyCode = (TextBox)gridCurrency.Rows[e.RowIndex].FindControl("txtEdtCurrencyCode");
        TextBox txtEdtCurrencySymbol = (TextBox)gridCurrency.Rows[e.RowIndex].FindControl("txtEdtCurrencySymbol");

        var UpdateData = from DBData in dbobj.CurrencyMasters
                         where DBData.CurrencyId == Convert.ToInt64(lblCurrencyId.Text)
                         select DBData;
        if (UpdateData.Count() > 0)
        {
            var DupData = (from DupC in dbobj.CurrencyMasters
                           where DupC.CurrencyCode.ToUpper() == txtEdtCurrencyCode.Text.Trim().ToUpper()
                           select DupC).Except(UpdateData);
            if (DupData.Count() > 0)
            {
                ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + txtEdtCurrencyCode.Text.Trim().ToUpper() + " duplicate currency code" + "');", true);
            }
            else
            {

                var SingleUpdate = UpdateData.Single();
                SingleUpdate.CurrencyName = txtEdtCurrencyName.Text.Trim().ToUpper();
                SingleUpdate.CurrencyCode = txtEdtCurrencyCode.Text.Trim().ToUpper();
                string str = txtEdtCurrencySymbol.Text.Replace("&", "");
                SingleUpdate.CurrencySymbol = str.Replace("#", "");
                SingleUpdate.ModifyBy = Convert.ToInt64(Global.UserId);
                SingleUpdate.ModifyDate = DateTime.Now;
                dbobj.SubmitChanges();
                dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                gridCurrency.EditIndex = -1;
                gridCurrency.ShowFooter = true;
                gridCurrency.FooterRow.Visible = true;
                gridCurrency.PagerSettings.Visible = true;
                FillGrid();
            }
        }
    }

    protected void gridCurrency_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridCurrency.PageIndex = e.NewPageIndex;
        FillGrid();
    }

    protected void txtCurrencySymbol_TextChanged(object sender, EventArgs e)
    {
        TextBox txtCurrencySymbol = (TextBox)gridCurrency.FooterRow.FindControl("txtCurrencySymbol");
        Label lblFCurrencySymbol = (Label)gridCurrency.FooterRow.FindControl("lblFCurrencySymbol");
        string str = txtCurrencySymbol.Text.Replace("&", "");
        string temp = str.Replace("#", "");
        if (temp == "")
            lblFCurrencySymbol.Text = "";
        else
            lblFCurrencySymbol.Text = "&#" + temp;
    }

    protected void imgbtnHelp_Click(object sender, ImageClickEventArgs e)
    {
        OpenWindowHelp(this, "MITPLInvoice", "CurrencyCode");
    }

    public static void OpenWindowHelp(Page currentPage, String window, String htmlPage)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("popWin=window.open('");
        sb.Append(htmlPage);
        sb.Append("','");
        sb.Append(window);
        sb.Append(",left=300,top=150,toolbar=no,location=center,directories=no,status=no,menubar=no,resizable=no");
        sb.Append("');");
        sb.Append("popWin.focus();");
        ScriptManager.RegisterClientScriptBlock(currentPage, typeof(StateMaster), "OpenWindow", sb.ToString(), true);
    }

    public static void OpenWindow(Page currentPage, String window, String htmlPage, Int32 width, Int32 height)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("popWin=window.open('");
        sb.Append(htmlPage);
        sb.Append("','");
        sb.Append(window);
        sb.Append("','width=");
        sb.Append(width);
        sb.Append(",height=");
        sb.Append(height);
        sb.Append(",left=300,top=150,toolbar=no,location=center,directories=no,status=no,menubar=no,scrollbars=no,resizable=no");
        sb.Append("');");
        sb.Append("popWin.focus();");

        ScriptManager.RegisterClientScriptBlock(currentPage, typeof(CountryMaster), "OpenWindow", sb.ToString(), true);
    }


    public string HighlightText(string InputTxt)
    {
        TextBox txt = (TextBox)this.Master.FindControl("txtMasterSearch");
        string Search_Str = txt.Text;

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
