using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;

public partial class State : System.Web.UI.Page
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
            var State = from DBData in dbobj.StateMasters
                        orderby DBData.CountryMaster.CountryName, DBData.StateName
                        select new
                        {
                            StateId = DBData.StateId,
                            StateName = DBData.StateName,
                            CountryName = DBData.CountryMaster.CountryName,
                            CountryId = DBData.CountryId
                        };
            if (State.Count() > 0)
            {
                gridState.DataSource = State;
                gridState.DataBind();
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("StateId");
                dt.Columns.Add("StateName");
                dt.Columns.Add("CountryName");
                dt.Columns.Add("CountryId");
                gridState.DataSource = dt;
                dt.Rows.Add(dt.NewRow());
                gridState.DataBind();
                int TotalCols = gridState.Rows[0].Cells.Count;
                gridState.Rows[0].Cells.Clear();
                gridState.Rows[0].Cells.Add(new TableCell());
                gridState.Rows[0].Cells[0].ColumnSpan = TotalCols;
                gridState.Rows[0].Cells[0].Text = "No records to display";
            }
        }
        else
        {
            var State = from DBData in dbobj.StateMasters
                        where DBData.StateName.Contains(txtMasterSearch.Text.Trim()) || DBData.CountryMaster.CountryName.Contains(txtMasterSearch.Text.Trim())
                        orderby DBData.CountryMaster.CountryName, DBData.StateName
                        select new
                        {
                            StateId = DBData.StateId,
                            StateName = DBData.StateName,
                            CountryName = DBData.CountryMaster.CountryName,
                            CountryId = DBData.CountryId
                        };
            if (State.Count() > 0)
            {
                gridState.DataSource = State;
                gridState.DataBind();
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("StateId");
                dt.Columns.Add("StateName");
                dt.Columns.Add("CountryName");
                dt.Columns.Add("CountryId");
                gridState.DataSource = dt;
                dt.Rows.Add(dt.NewRow());
                gridState.DataBind();
                int TotalCols = gridState.Rows[0].Cells.Count;
                gridState.Rows[0].Cells.Clear();
                gridState.Rows[0].Cells.Add(new TableCell());
                gridState.Rows[0].Cells[0].ColumnSpan = TotalCols;
                gridState.Rows[0].Cells[0].Text = "No records to display";
            }
        }

    }

    protected void gridState_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Page page1 = HttpContext.Current.Handler as Page;
        TextBox txtStateName = (TextBox)gridState.FooterRow.FindControl("txtStateName");
        DropDownList ddlCountry = (DropDownList)gridState.FooterRow.FindControl("ddlCountry");
        switch (e.CommandName)
        {
            case "Insert":
                if (ddlCountry.SelectedIndex != -1)
                {
                    var DupState = from DC in dbobj.StateMasters
                                   where DC.StateName == txtStateName.Text.Trim().ToUpper() && DC.CountryId == Convert.ToInt64(ddlCountry.SelectedValue)
                                   select DC;
                    if (DupState.Count() > 0)
                    {
                        ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + DupState.Single().StateName + " state already exists in " + DupState.Single().CountryMaster.CountryName + "')", true);
                    }
                    else
                    {
                        StateMaster sm = new StateMaster();
                        sm.StateName = txtStateName.Text.Trim().ToUpper();
                        sm.CountryId = Convert.ToInt64(ddlCountry.SelectedValue);
                        sm.CreatedBy = Convert.ToInt64(Global.UserId);
                        sm.CreatedDate = DateTime.Now;
                        dbobj.StateMasters.InsertOnSubmit(sm);
                        dbobj.SubmitChanges();
                        dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                        ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + txtStateName.Text.Trim().ToUpper() + " state add in " + ddlCountry.SelectedItem.ToString().ToUpper() + "')", true);
                    }

                    FillGrid();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('Select country')", true);
                }
                break;
            case "Delete":
                GridViewRow gr = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                Label lblStateId = (Label)gr.FindControl("lblStateId");
                string StateName;
                var DelState = from DelS in dbobj.StateMasters
                               where DelS.StateId == Convert.ToInt64(lblStateId.Text)
                               select DelS;
                if (DelState.Count() > 0)
                {
                    var State = DelState.Single();
                    StateName = State.StateName;
                    dbobj.StateMasters.DeleteOnSubmit(State);
                    try
                    {
                        dbobj.SubmitChanges();
                        dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                        //ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + State.StateName + " state in " + State.CountryMaster.CountryName + " is deleted" + "')", true);
                        FillGrid();
                    }
                    catch
                    {
                        OpenWindow(this, "MITPLInvoice", "DeletePopup?id=" + lblStateId.Text + "&name=" + StateName + "&page=state", 650, 350);
                    }
                }

                break;
        }
    }

    protected void gridState_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }

    protected void gridState_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gridState.EditIndex = e.NewEditIndex;
        gridState.ShowFooter = false;
        gridState.FooterRow.Visible = false;
        gridState.PagerSettings.Visible = false;
        Label lblCountry = (Label)gridState.Rows[e.NewEditIndex].FindControl("lblCountry");
        FillGrid();
        DropDownList ddlEdtCountry = (DropDownList)gridState.Rows[e.NewEditIndex].FindControl("ddlEdtCountry");
        ddlEdtCountry.Items.FindByText(lblCountry.Text).Selected = true;
    }

    protected void gridState_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gridState.EditIndex = -1;
        gridState.ShowFooter = true;
        gridState.FooterRow.Visible = true;
        gridState.PagerSettings.Visible = true;
        FillGrid();
    }

    protected void gridState_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        Page page1 = HttpContext.Current.Handler as Page;
        Label lblStateId = (Label)gridState.Rows[e.RowIndex].FindControl("lblStateId");
        TextBox txtEdtStateName = (TextBox)gridState.Rows[e.RowIndex].FindControl("txtEdtStateName");
        DropDownList ddlEdtCountry = (DropDownList)gridState.Rows[e.RowIndex].FindControl("ddlEdtCountry");

        var UpdateData = from DBData in dbobj.StateMasters
                         where DBData.StateId == Convert.ToInt64(lblStateId.Text)
                         select DBData;
        if (UpdateData.Count() > 0)
        {
            var DupData = (from DupC in dbobj.StateMasters
                           where DupC.StateName.ToUpper() == txtEdtStateName.Text.Trim().ToUpper() && DupC.CountryId == Convert.ToInt64(ddlEdtCountry.SelectedValue)
                           select DupC).Except(UpdateData);
            if (DupData.Count() > 0)
            {
                ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + DupData.Single().StateName + " state already exists in " + DupData.Single().CountryMaster.CountryName + "')", true);
            }
            else
            {
                var SingleUpdate = UpdateData.Single();
                SingleUpdate.StateName = txtEdtStateName.Text.Trim().ToUpper();
                SingleUpdate.CountryId = Convert.ToInt64(ddlEdtCountry.SelectedValue);
                SingleUpdate.ModifyBy = Convert.ToInt64(Global.UserId);
                SingleUpdate.ModifyDate = DateTime.Now;
                dbobj.SubmitChanges();
                dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                gridState.EditIndex = -1;
                gridState.ShowFooter = true;
                gridState.FooterRow.Visible = true;
                gridState.PagerSettings.Visible = true;
                FillGrid();
            }
        }
    }

    protected void gridState_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridState.PageIndex = e.NewPageIndex;
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

        ScriptManager.RegisterClientScriptBlock(currentPage, typeof(StateMaster), "OpenWindow", sb.ToString(), true);
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
