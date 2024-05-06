using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;


public partial class Department : System.Web.UI.Page
{
    MITInvoiceDataContext dbobj = new MITInvoiceDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Global.UserType == "ADMINISTRATOR")
        {
            if (!IsPostBack)
            {
                FillGrid();
                FillUser();
            }
        }
        else
        {
            Response.Redirect("Authorize");
        }
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

    public void FillGrid()
    {
        TextBox txtMasterSearch = (TextBox)this.Master.FindControl("txtMasterSearch");
        if (txtMasterSearch.Text == "")
        {
            var Department = from DBData in dbobj.DepartmentMasters
                             where DBData.IsActive == true
                             orderby DBData.DepartmentName
                             select new
                             {
                                 DepartmentId = DBData.DepartmentId,
                                 DepartmentName = DBData.DepartmentName,
                                 DepartmentHOD = DBData.UserMaster.FirstName + " " + DBData.UserMaster.LastName
                             };
            if (Department.Count() > 0)
            {
                gridDepartment.DataSource = Department;
                gridDepartment.DataBind();
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("DepartmentId");
                dt.Columns.Add("DepartmentName");
                dt.Columns.Add("DepartmentHOD");
                gridDepartment.DataSource = dt;
                dt.Rows.Add(dt.NewRow());
                gridDepartment.DataBind();
                int TotalCols = gridDepartment.Rows[0].Cells.Count;
                gridDepartment.Rows[0].Cells.Clear();
                gridDepartment.Rows[0].Cells.Add(new TableCell());
                gridDepartment.Rows[0].Cells[0].ColumnSpan = TotalCols;
                gridDepartment.Rows[0].Cells[0].Text = "No records to display";
            }
        }
        else
        {
            var Department = from DBData in dbobj.DepartmentMasters
                             where DBData.IsActive == true && DBData.DepartmentName.Contains(txtMasterSearch.Text.Trim()) || DBData.UserMaster.FirstName.Contains(txtMasterSearch.Text.Trim()) || DBData.UserMaster.LastName.Contains(txtMasterSearch.Text.Trim())
                             orderby DBData.DepartmentName
                             select new
                             {
                                 DepartmentId = DBData.DepartmentId,
                                 DepartmentName = DBData.DepartmentName,
                                 DepartmentHOD = DBData.UserMaster.FirstName + " " + DBData.UserMaster.LastName
                             };
            if (Department.Count() > 0)
            {
                gridDepartment.DataSource = Department;
                gridDepartment.DataBind();
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("DepartmentId");
                dt.Columns.Add("DepartmentName");
                dt.Columns.Add("DepartmentHOD");
                gridDepartment.DataSource = dt;
                dt.Rows.Add(dt.NewRow());
                gridDepartment.DataBind();
                int TotalCols = gridDepartment.Rows[0].Cells.Count;
                gridDepartment.Rows[0].Cells.Clear();
                gridDepartment.Rows[0].Cells.Add(new TableCell());
                gridDepartment.Rows[0].Cells[0].ColumnSpan = TotalCols;
                gridDepartment.Rows[0].Cells[0].Text = "No records to display";
            }
        }
    }

    public void FillUser()
    {
        DropDownList ddlUser = (DropDownList)gridDepartment.FooterRow.FindControl("ddlUser");
        var User = from DBData in dbobj.UserMasters
                   where DBData.IsActive == true
                   select new
                   {
                       UserId = DBData.UserId,
                       UserName = DBData.FirstName + " " + DBData.LastName
                   };
        ddlUser.DataSource = User.OrderBy(a => a.UserName);
        ddlUser.DataTextField = "UserName";
        ddlUser.DataValueField = "UserId";
        ddlUser.DataBind();
        ddlUser.Items.Insert(0, "-- Select --");
    }

    protected void gridDepartment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Page page1 = HttpContext.Current.Handler as Page;
        TextBox txtDepartmentName = (TextBox)gridDepartment.FooterRow.FindControl("txtDepartmentName");
        DropDownList ddlUser = (DropDownList)gridDepartment.FooterRow.FindControl("ddlUser");
        switch (e.CommandName)
        {
            case "Insert":

                var DupDepartment = from DC in dbobj.DepartmentMasters
                                    where DC.DepartmentName == txtDepartmentName.Text.Trim().ToUpper()
                                    select DC;
                if (DupDepartment.Count() > 0)
                    ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + txtDepartmentName.Text.Trim().ToUpper() + " department already exists" + "')", true);
                else
                {
                    DepartmentMaster dm = new DepartmentMaster();
                    dm.DepartmentName = txtDepartmentName.Text.Trim().ToUpper();
                    if (ddlUser.SelectedIndex != 0 && ddlUser.SelectedIndex != -1)
                        dm.UserId = Convert.ToInt64(ddlUser.SelectedValue);
                    dm.IsActive = true;
                    dm.CreatedBy = Convert.ToInt64(Global.UserId);
                    dm.CreatedDate = DateTime.Now;
                    dbobj.DepartmentMasters.InsertOnSubmit(dm);
                    dbobj.SubmitChanges();
                    dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);

                    ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + txtDepartmentName.Text.Trim().ToUpper() + " department add successfully" + "')", true);
                }
                FillGrid();
                FillUser();
                break;
            case "Delete":
                GridViewRow gr = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                Label lblDepartmentId = (Label)gr.FindControl("lblDepartmentId");
                Label lblDepartmentName = (Label)gr.FindControl("lblDepartmentName");

                var UserData = from DBUser in dbobj.UserMasters
                               where DBUser.DepartmentId == Convert.ToInt64(lblDepartmentId.Text)
                               select DBUser;
                var ClientDate = from DBClient in dbobj.ClientMasters
                                 where DBClient.DepartmentId == Convert.ToInt64(lblDepartmentId.Text)
                                 select DBClient;
                var ProcessData = from DBProces in dbobj.ProcessMasters
                                  where DBProces.DepartmentId == Convert.ToInt64(lblDepartmentId.Text)
                                  select DBProces;
                if (UserData.Count() == 0 && ClientDate.Count() == 0 && ProcessData.Count() == 0)
                {
                    var DelDepartment = from DelD in dbobj.DepartmentMasters
                                        where DelD.DepartmentId == Convert.ToInt64(lblDepartmentId.Text)
                                        select DelD;
                    if (DelDepartment.Count() > 0)
                    {
                        var Department = DelDepartment.Single();
                        Department.IsActive = false;
                        dbobj.SubmitChanges();
                        dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                        //ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + Department.DepartmentName.ToUpper() + " department name deleted" + "')", true);
                    }
                    FillGrid();
                    FillUser();
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + UserData.First().DepartmentMaster.DepartmentName + " department can not be deleted, It is already used by other" + "')", true);
                    OpenWindow(this, "MITPLInvoice", "DeletePopup?id=" + lblDepartmentId.Text + "&name=" + lblDepartmentName.Text + "&page=dept", 650, 350);
                }
                break;
        }
    }

    protected void gridDepartment_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    protected void gridDepartment_RowEditing(object sender, GridViewEditEventArgs e)
    {
        Label lblDepartmentHOD = (Label)gridDepartment.Rows[e.NewEditIndex].FindControl("lblDepartmentHOD");
        gridDepartment.EditIndex = e.NewEditIndex;
        gridDepartment.ShowFooter = false;
        gridDepartment.FooterRow.Visible = false;
        gridDepartment.PagerSettings.Visible = false;

        FillGrid();
        DropDownList ddlEdtUser = (DropDownList)gridDepartment.Rows[e.NewEditIndex].FindControl("ddlEdtUser");
        var EdtUser = from DBData in dbobj.UserMasters
                      where DBData.IsActive == true
                      select new
                      {
                          UserId = DBData.UserId,
                          UserName = DBData.FirstName + " " + DBData.LastName
                      };
        if (EdtUser.Count() > 0)
        {
            ddlEdtUser.DataSource = EdtUser.OrderBy(a => a.UserName);
            ddlEdtUser.DataTextField = "UserName";
            ddlEdtUser.DataValueField = "UserId";
            ddlEdtUser.DataBind();
            if (lblDepartmentHOD.Text != "")
            {
                ddlEdtUser.Items.FindByText(lblDepartmentHOD.Text).Selected = true;
            }
            ddlEdtUser.Items.Insert(0, "-- Select --");
        }
    }

    protected void gridDepartment_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gridDepartment.EditIndex = -1;
        gridDepartment.ShowFooter = true;
        gridDepartment.FooterRow.Visible = true;
        gridDepartment.PagerSettings.Visible = true;
        FillGrid();
        FillUser();
    }

    protected void gridDepartment_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        Page page1 = HttpContext.Current.Handler as Page;
        Label lblDepartmentId = (Label)gridDepartment.Rows[e.RowIndex].FindControl("lblDepartmentId");
        TextBox txtDepartmentEdtName = (TextBox)gridDepartment.Rows[e.RowIndex].FindControl("txtDepartmentEdtName");
        DropDownList ddlEdtUser = (DropDownList)gridDepartment.Rows[e.RowIndex].FindControl("ddlEdtUser");

        var UpdateData = from DBData in dbobj.DepartmentMasters
                         where DBData.DepartmentId == Convert.ToInt64(lblDepartmentId.Text)
                         select DBData;
        if (UpdateData.Count() > 0)
        {
            var DupData = (from DupC in dbobj.DepartmentMasters
                           where DupC.DepartmentName.ToUpper() == txtDepartmentEdtName.Text.Trim().ToUpper()
                           select DupC).Except(UpdateData);
            if (DupData.Count() > 0)
                ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + DupData.Single().DepartmentName.ToUpper() + " duplicate country name" + "');", true);
            else
            {
                var SingleUpdate = UpdateData.Single();
                SingleUpdate.DepartmentName = txtDepartmentEdtName.Text.Trim().ToUpper();
                if (ddlEdtUser.SelectedIndex != 0)
                    SingleUpdate.UserId = Convert.ToInt64(ddlEdtUser.SelectedValue);
                else
                    SingleUpdate.UserId = null;
                SingleUpdate.ModifyBy = Convert.ToInt64(Global.UserId);
                SingleUpdate.ModifyDate = DateTime.Now;
                dbobj.SubmitChanges();
                dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                gridDepartment.EditIndex = -1;
                gridDepartment.ShowFooter = true;
                gridDepartment.FooterRow.Visible = true;
                gridDepartment.PagerSettings.Visible = true;
                FillGrid();
                FillUser();
            }
        }
    }

    protected void gridDepartment_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridDepartment.PageIndex = e.NewPageIndex;
        FillGrid();
        FillUser();
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
}
