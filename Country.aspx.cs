using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;

public partial class Country : System.Web.UI.Page
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
            var Country = from DBData in dbobj.CountryMasters
                          orderby DBData.CountryName
                          select new
                          {
                              CountryId = DBData.CountryId,
                              CountryName = DBData.CountryName
                          };
            if (Country.Count() > 0)
            {
                gridCountry.DataSource = Country;
                gridCountry.DataBind();
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("CountryId");
                dt.Columns.Add("CountryName");
                gridCountry.DataSource = dt;
                dt.Rows.Add(dt.NewRow());
                gridCountry.DataBind();
                int TotalCols = gridCountry.Rows[0].Cells.Count;
                gridCountry.Rows[0].Cells.Clear();
                gridCountry.Rows[0].Cells.Add(new TableCell());
                gridCountry.Rows[0].Cells[0].ColumnSpan = TotalCols;
                gridCountry.Rows[0].Cells[0].Text = "No records to display";
            }
        }
        else
        {
            var Country = from DBData in dbobj.CountryMasters
                          where DBData.CountryName.Contains(txtMasterSearch.Text.Trim())
                          orderby DBData.CountryName
                          select new
                          {
                              CountryId = DBData.CountryId,
                              CountryName = DBData.CountryName
                          };
            if (Country.Count() > 0)
            {
                gridCountry.DataSource = Country;
                gridCountry.DataBind();
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("CountryId");
                dt.Columns.Add("CountryName");
                gridCountry.DataSource = dt;
                dt.Rows.Add(dt.NewRow());
                gridCountry.DataBind();
                int TotalCols = gridCountry.Rows[0].Cells.Count;
                gridCountry.Rows[0].Cells.Clear();
                gridCountry.Rows[0].Cells.Add(new TableCell());
                gridCountry.Rows[0].Cells[0].ColumnSpan = TotalCols;
                gridCountry.Rows[0].Cells[0].Text = "No records to display";
            }
        }
    }

    protected void gridCountry_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Page page1 = HttpContext.Current.Handler as Page;
        TextBox txtCountryName = (TextBox)gridCountry.FooterRow.FindControl("txtCountryName");
        switch (e.CommandName)
        {
            case "Insert":

                var DupContry = from DC in dbobj.CountryMasters
                                where DC.CountryName == txtCountryName.Text.Trim().ToUpper()
                                select DC;
                if (DupContry.Count() > 0)
                {
                    ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + txtCountryName.Text.Trim().ToUpper() + " country already exists" + "')", true);
                }
                else
                {
                    CountryMaster cm = new CountryMaster();
                    cm.CountryName = txtCountryName.Text.Trim().ToUpper();
                    cm.CreatedBy = Convert.ToInt64(Global.UserId);
                    cm.CreatedDate = DateTime.Now;
                    dbobj.CountryMasters.InsertOnSubmit(cm);
                    dbobj.SubmitChanges();
                    dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                    ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + txtCountryName.Text.Trim().ToUpper() + " country add successfully" + "')", true);
                }
                FillGrid();

                break;
            case "Delete":
                GridViewRow gr = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                Label lblCountryId = (Label)gr.FindControl("lblCountryId");
                string CountryName;
                var DelCountry = from DelC in dbobj.CountryMasters
                                 where DelC.CountryId == Convert.ToInt64(lblCountryId.Text)
                                 select DelC;
                if (DelCountry.Count() > 0)
                {
                    var Country = DelCountry.Single();
                    CountryName = Country.CountryName;
                    dbobj.CountryMasters.DeleteOnSubmit(Country);
                    try
                    {
                        dbobj.SubmitChanges();
                        dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                        //ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + Country.CountryName.ToUpper() + " county name deleted" + "')", true);
                        FillGrid();
                    }
                    catch
                    {
                        OpenWindow(this, "MITPLInvoice", "DeletePopup?id=" + lblCountryId.Text + "&name=" + CountryName + "&page=country", 650, 350);
                    }
                }
                break;
        }
    }

    protected void gridCountry_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    protected void gridCountry_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gridCountry.EditIndex = e.NewEditIndex;
        gridCountry.ShowFooter = false;
        gridCountry.FooterRow.Visible = false;
        gridCountry.PagerSettings.Visible = false;
        FillGrid();
    }

    protected void gridCountry_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gridCountry.EditIndex = -1;
        gridCountry.ShowFooter = true;
        gridCountry.FooterRow.Visible = true;
        gridCountry.PagerSettings.Visible = true;
        FillGrid();
    }

    protected void gridCountry_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        Page page1 = HttpContext.Current.Handler as Page;
        Label lblCountryId = (Label)gridCountry.Rows[e.RowIndex].FindControl("lblCountryId");
        TextBox txtCountryName = (TextBox)gridCountry.Rows[e.RowIndex].FindControl("txtEdtCountryName");

        var UpdateData = from DBData in dbobj.CountryMasters
                         where DBData.CountryId == Convert.ToInt64(lblCountryId.Text)
                         select DBData;
        if (UpdateData.Count() > 0)
        {
            var DupData = (from DupC in dbobj.CountryMasters
                           where DupC.CountryName.ToUpper() == txtCountryName.Text.Trim().ToUpper()
                           select DupC).Except(UpdateData);
            if (DupData.Count() > 0)
            {
                ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + DupData.Single().CountryName.ToUpper() + " duplicate country name" + "');", true);
            }
            else
            {
                var SingleUpdate = UpdateData.Single();
                SingleUpdate.CountryName = txtCountryName.Text.Trim().ToUpper();
                SingleUpdate.ModifyBy = Convert.ToInt64(Global.UserId);
                SingleUpdate.ModifyDate = DateTime.Now;
                dbobj.SubmitChanges();
                dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                gridCountry.EditIndex = -1;
                gridCountry.ShowFooter = true;
                gridCountry.FooterRow.Visible = true;
                gridCountry.PagerSettings.Visible = true;
                FillGrid();
            }
        }
    }

    protected void gridCountry_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridCountry.PageIndex = e.NewPageIndex;
        FillGrid();
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
