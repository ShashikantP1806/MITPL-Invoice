using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;

public partial class City : System.Web.UI.Page
{
    MITInvoiceDataContext dbobj = new MITInvoiceDataContext();
    private string SearchString = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Global.UserType == "ADMINISTRATOR")
        {
            if (!IsPostBack)
            {
                FillGrid();
                FillState();    
                TextBox txt = (TextBox)this.Master.FindControl("txtMasterSearch");
                SearchString = txt.Text;
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
            var City = from DBData in dbobj.CityMasters
                       orderby DBData.CountryMaster.CountryName, DBData.StateMaster.StateName, DBData.CityName
                       select new
                       {
                           CityId = DBData.CityId,
                           CityName = DBData.CityName,
                           StateId = DBData.StateId,
                           StateName = DBData.StateMaster.StateName,
                           CountryName = DBData.CountryMaster.CountryName,
                           CountryId = DBData.CountryId
                       };
            if (City.Count() > 0)
            {
                gridCity.DataSource = City;
                gridCity.DataBind();
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("CityId");
                dt.Columns.Add("CityName");
                dt.Columns.Add("StateId");
                dt.Columns.Add("StateName");
                dt.Columns.Add("CountryName");
                dt.Columns.Add("CountryId");
                gridCity.DataSource = dt;
                dt.Rows.Add(dt.NewRow());
                gridCity.DataBind();
                int TotalCols = gridCity.Rows[0].Cells.Count;
                gridCity.Rows[0].Cells.Clear();
                gridCity.Rows[0].Cells.Add(new TableCell());
                gridCity.Rows[0].Cells[0].ColumnSpan = TotalCols;
                gridCity.Rows[0].Cells[0].Text = "No records to display";
            }
        }
        else
        {
            var City = from DBData in dbobj.CityMasters
                       where DBData.CityName.Contains(txtMasterSearch.Text.Trim()) || DBData.StateMaster.StateName.Contains(txtMasterSearch.Text.Trim()) || DBData.CountryMaster.CountryName.Contains(txtMasterSearch.Text.Trim())
                       orderby DBData.CountryMaster.CountryName, DBData.StateMaster.StateName, DBData.CityName
                       select new
                       {
                           CityId = DBData.CityId,
                           CityName = DBData.CityName,
                           StateId = DBData.StateId,
                           StateName = DBData.StateMaster.StateName,
                           CountryName = DBData.CountryMaster.CountryName,
                           CountryId = DBData.CountryId
                       };
            if (City.Count() > 0)
            {
                gridCity.DataSource = City;
                gridCity.DataBind();
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("CityId");
                dt.Columns.Add("CityName");
                dt.Columns.Add("StateId");
                dt.Columns.Add("StateName");
                dt.Columns.Add("CountryName");
                dt.Columns.Add("CountryId");
                gridCity.DataSource = dt;
                dt.Rows.Add(dt.NewRow());
                gridCity.DataBind();
                int TotalCols = gridCity.Rows[0].Cells.Count;
                gridCity.Rows[0].Cells.Clear();
                gridCity.Rows[0].Cells.Add(new TableCell());
                gridCity.Rows[0].Cells[0].ColumnSpan = TotalCols;
                gridCity.Rows[0].Cells[0].Text = "No records to display";
            }
        }

    }

    public void FillState()
    {
        DropDownList ddlState = (DropDownList)gridCity.FooterRow.FindControl("ddlState");
        DropDownList ddlCountry = (DropDownList)gridCity.FooterRow.FindControl("ddlCountry");
        if (ddlCountry.SelectedIndex != -1)
        {
            var State = from DB in dbobj.StateMasters
                        where DB.CountryId == Convert.ToInt64(ddlCountry.SelectedValue)
                        orderby DB.StateName
                        select new
                        {
                            StateId = DB.StateId,
                            StateName = DB.StateName
                        };
            ddlState.Items.Clear();
            if (State.Count() > 0)
            {
                ddlState.DataSource = State;
                ddlState.DataTextField = "StateName";
                ddlState.DataValueField = "StateId";
                ddlState.DataBind();
            }
        }
    }

    protected void gridCity_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Page page1 = HttpContext.Current.Handler as Page;
        TextBox txtCityName = (TextBox)gridCity.FooterRow.FindControl("txtCityName");
        DropDownList ddlState = (DropDownList)gridCity.FooterRow.FindControl("ddlState");
        DropDownList ddlCountry = (DropDownList)gridCity.FooterRow.FindControl("ddlCountry");
        switch (e.CommandName)
        {
            case "Insert":
                if (ddlCountry.SelectedIndex != -1 && ddlState.SelectedIndex != -1)
                {
                    var DupCity = from DC in dbobj.CityMasters
                                  where DC.CityName == txtCityName.Text.Trim().ToUpper() && DC.StateId == Convert.ToInt64(ddlState.SelectedValue) && DC.CountryId == Convert.ToInt64(ddlCountry.SelectedValue)
                                  select DC;
                    if (DupCity.Count() > 0)
                    {
                        ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + DupCity.Single().CityName.ToUpper() + " city already exists in " + DupCity.Single().StateMaster.StateName + " (" + DupCity.Single().CountryMaster.CountryName + ")" + "')", true);
                    }
                    else
                    {
                        CityMaster cm = new CityMaster();
                        cm.CityName = txtCityName.Text.Trim().ToUpper();
                        cm.StateId = Convert.ToInt64(ddlState.SelectedValue);
                        cm.CountryId = Convert.ToInt64(ddlCountry.SelectedValue);
                        cm.CreatedBy = Convert.ToInt64(Global.UserId);
                        cm.CreatedDate = DateTime.Now;
                        dbobj.CityMasters.InsertOnSubmit(cm);
                        dbobj.SubmitChanges();
                        dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                        ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + DupCity.Single().CityName.ToUpper() + " city add in " + DupCity.Single().StateMaster.StateName + " (" + DupCity.Single().CountryMaster.CountryName + ")" + "')", true);
                    }
                    FillGrid();
                    FillState();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('Select country & state')", true);
                }

                break;
            case "Delete":
                GridViewRow gr = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                Label lblCityId = (Label)gr.FindControl("lblCityId");
                Label lblCityName = (Label)gr.FindControl("lblCityName");

                var DelCity = from DelC in dbobj.CityMasters
                              where DelC.CityId == Convert.ToInt64(lblCityId.Text)
                              select DelC;
                if (DelCity.Count() > 0)
                {
                    var City = DelCity.Single();
                    dbobj.CityMasters.DeleteOnSubmit(City);
                    try
                    {
                        dbobj.SubmitChanges();
                        dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                        //ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + City.CityName + " city in " + City.StateMaster.StateName.ToUpper() + " (" + City.CountryMaster.CountryName.ToUpper() + ")" + " is deleted" + "')", true);
                    }
                    catch
                    {
                        OpenWindow(this, "MITPLInvoice", "DeletePopup?id=" + lblCityId.Text + "&name=" + lblCityName.Text + "&page=city", 650, 350);
                    }
                }
                FillGrid();
                break;
        }
    }

    protected void gridCity_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }

    protected void gridCity_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gridCity.EditIndex = e.NewEditIndex;
        gridCity.ShowFooter = false;
        gridCity.FooterRow.Visible = false;
        gridCity.PagerSettings.Visible = false;
        Label lblCountry = (Label)gridCity.Rows[e.NewEditIndex].FindControl("lblCountry");
        Label lblState = (Label)gridCity.Rows[e.NewEditIndex].FindControl("lblState");
        FillGrid();
        DropDownList ddlEdtCountry = (DropDownList)gridCity.Rows[e.NewEditIndex].FindControl("ddlEdtCountry");
        ddlEdtCountry.Items.FindByText(lblCountry.Text).Selected = true;

        DropDownList ddlEdtState = (DropDownList)gridCity.Rows[e.NewEditIndex].FindControl("ddlEdtState");
        var State = from DB in dbobj.StateMasters
                    where DB.CountryId == Convert.ToInt64(ddlEdtCountry.SelectedValue)
                    orderby DB.StateName
                    select new
                    {
                        StateId = DB.StateId,
                        StateName = DB.StateName
                    };
        ddlEdtState.Items.Clear();
        if (State.Count() > 0)
        {
            ddlEdtState.DataSource = State;
            ddlEdtState.DataTextField = "StateName";
            ddlEdtState.DataValueField = "StateId";
            ddlEdtState.DataBind();
        }
        ddlEdtState.Items.FindByText(lblState.Text).Selected = true;
    }

    protected void gridCity_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gridCity.EditIndex = -1;
        gridCity.ShowFooter = true;
        gridCity.FooterRow.Visible = true;
        gridCity.PagerSettings.Visible = true;
        FillGrid();
        FillState();
    }

    protected void gridCity_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        Page page1 = HttpContext.Current.Handler as Page;
        Label lblCityId = (Label)gridCity.Rows[e.RowIndex].FindControl("lblCityId");
        TextBox txtEdtCityName = (TextBox)gridCity.Rows[e.RowIndex].FindControl("txtEdtCityName");
        DropDownList ddlEdtCountry = (DropDownList)gridCity.Rows[e.RowIndex].FindControl("ddlEdtCountry");
        DropDownList ddlEdtState = (DropDownList)gridCity.Rows[e.RowIndex].FindControl("ddlEdtState");

        var UpdateData = from DBData in dbobj.CityMasters
                         where DBData.CityId == Convert.ToInt64(lblCityId.Text)
                         select DBData;
        if (UpdateData.Count() > 0)
        {
            var DupData = (from DupC in dbobj.CityMasters
                           where DupC.CityName.ToUpper() == txtEdtCityName.Text.Trim().ToUpper() && DupC.StateId == Convert.ToInt64(ddlEdtState.SelectedValue) && DupC.CountryId == Convert.ToInt64(ddlEdtCountry.SelectedValue)
                           select DupC).Except(UpdateData);
            if (DupData.Count() > 0)
            {
                ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + DupData.Single().CityName + " city already exists in " + DupData.Single().StateMaster.StateName + " (" + DupData.Single().CountryMaster.CountryName + ")" + "')", true);
            }
            else
            {
                var SingleUpdate = UpdateData.Single();
                SingleUpdate.CityName = txtEdtCityName.Text.Trim().ToUpper();
                SingleUpdate.CountryId = Convert.ToInt64(ddlEdtCountry.SelectedValue);
                SingleUpdate.StateId = Convert.ToInt64(ddlEdtState.SelectedValue);
                SingleUpdate.ModifyBy = Convert.ToInt64(Global.UserId);
                SingleUpdate.ModifyDate = DateTime.Now;
                dbobj.SubmitChanges();
                dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                gridCity.EditIndex = -1;
                gridCity.ShowFooter = true;
                gridCity.FooterRow.Visible = true;
                gridCity.PagerSettings.Visible = true;
                FillGrid();
                FillState();
            }
        }
    }

    protected void gridCity_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridCity.PageIndex = e.NewPageIndex;
        FillGrid();
    }

    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillState();
    }

    protected void ddlEdtCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddl.NamingContainer;
        DropDownList ddlEdtCountry = (DropDownList)row.FindControl("ddlEdtCountry");
        DropDownList ddlEdtState = (DropDownList)row.FindControl("ddlEdtState");

        var State = from DB in dbobj.StateMasters
                    where DB.CountryId == Convert.ToInt64(ddlEdtCountry.SelectedValue)
                    orderby DB.StateName
                    select new
                    {
                        StateId = DB.StateId,
                        StateName = DB.StateName
                    };
        ddlEdtState.Items.Clear();
        if (State.Count() > 0)
        {
            ddlEdtState.DataSource = State;
            ddlEdtState.DataTextField = "StateName";
            ddlEdtState.DataValueField = "StateId";
            ddlEdtState.DataBind();
        }
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
