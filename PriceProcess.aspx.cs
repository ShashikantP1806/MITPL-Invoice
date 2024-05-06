using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class PriceProcess : System.Web.UI.Page
{
    MITInvoiceDataContext dbobj = new MITInvoiceDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Global.UserType == "BUSINESS UNIT MANAGER")
        {
            if (!IsPostBack)
            {
                FillGrid();
                FillDepartment();
            }
        }
        else
        {
            Response.Redirect("Authorize");
        }
    }

    protected void FillGrid()
    {
        var Process = from DBData in dbobj.ProcessMasters
                      orderby DBData.ProcessName
                      select new
                      {
                          ProcessId = DBData.ProcessId,
                          ProcessName = DBData.ProcessName,
                          DepartmentName = DBData.DepartmentMaster.DepartmentName
                      };
        if (Process.Count() > 0)
        {
            gridProcess.DataSource = Process;
            gridProcess.DataBind();
        }
        else
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ProcessId");
            dt.Columns.Add("ProcessName");
            dt.Columns.Add("DepartmentName");
            gridProcess.DataSource = dt;
            dt.Rows.Add(dt.NewRow());
            gridProcess.DataBind();
            int TotalCols = gridProcess.Rows[0].Cells.Count;
            gridProcess.Rows[0].Cells.Clear();
            gridProcess.Rows[0].Cells.Add(new TableCell());
            gridProcess.Rows[0].Cells[0].ColumnSpan = TotalCols;
            gridProcess.Rows[0].Cells[0].Text = "No records to display";
        }
    }

    protected void FillDepartment()
    {
        DropDownList ddlDepartment = (DropDownList)gridProcess.FooterRow.FindControl("ddlDepartment");
        var Department = from DBData in dbobj.DepartmentMasters
                         where DBData.IsActive == true
                         orderby DBData.DepartmentName
                         select new
                         {
                             DepartmentName = DBData.DepartmentName,
                             DepartmentId = DBData.DepartmentId
                         };
        if (Department.Count() > 0)
        {
            ddlDepartment.DataSource = Department;
            ddlDepartment.DataTextField = "DepartmentName";
            ddlDepartment.DataValueField = "DepartmentId";
            ddlDepartment.DataBind();
        }
        ddlDepartment.Items.Insert(0, "-- Select --");
    }

    protected void gridProcess_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Page page1 = HttpContext.Current.Handler as Page;
        TextBox txtProcessName = (TextBox)gridProcess.FooterRow.FindControl("txtProcessName");
        DropDownList ddlDepartment = (DropDownList)gridProcess.FooterRow.FindControl("ddlDepartment");
        switch (e.CommandName)
        {
            case "Insert":
                var DupProcess = from DP in dbobj.ProcessMasters
                                 where DP.ProcessName == txtProcessName.Text.Trim().ToUpper() && DP.DepartmentId == Convert.ToInt64(ddlDepartment.SelectedValue)
                                 select DP;
                if (DupProcess.Count() > 0)
                {
                    ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + txtProcessName.Text.Trim().ToUpper() + " process already exists" + "')", true);
                }
                else
                {
                    ProcessMaster pm = new ProcessMaster();
                    pm.ProcessName = txtProcessName.Text.Trim().ToUpper();
                    pm.DepartmentId = Convert.ToInt64(ddlDepartment.SelectedValue);
                    pm.CreatedBy = Convert.ToInt64(Global.UserId);
                    dbobj.ProcessMasters.InsertOnSubmit(pm);
                    dbobj.SubmitChanges();
                    dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                    ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + txtProcessName.Text.Trim().ToUpper() + " process add successfully" + "')", true);
                }

                FillGrid();
                FillDepartment();
                break;
            case "Delete":
                GridViewRow gr = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                Label lblProcessId = (Label)gr.FindControl("lblProcessId");
                Label lblProcessName = (Label)gr.FindControl("lblProcessName");

                var DelProcess = from DelP in dbobj.ProcessMasters
                                 where DelP.ProcessId == Convert.ToInt64(lblProcessId.Text)
                                 select DelP;
                if (DelProcess.Count() > 0)
                {
                    var SingleProcess = DelProcess.Single();
                    dbobj.ProcessMasters.DeleteOnSubmit(SingleProcess);
                    try
                    {
                        dbobj.SubmitChanges();
                        dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                        //ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + SingleProcess.ProcessName.ToUpper() + " process deleted" + "')", true);
                    }
                    catch
                    {
                        OpenWindow(this, "MITPLInvoice", "DeletePopup?id=" + lblProcessId.Text + "&name=" + lblProcessName.Text + "&page=process", 650, 350);
                    }
                }
                FillGrid();
                FillDepartment();
                break;
        }
    }

    protected void gridProcess_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }

    protected void gridProcess_RowEditing(object sender, GridViewEditEventArgs e)
    {
        Label lblDepartmentName = (Label)gridProcess.Rows[e.NewEditIndex].FindControl("lblDepartmentName");
        gridProcess.EditIndex = e.NewEditIndex;
        gridProcess.ShowFooter = false;
        gridProcess.FooterRow.Visible = false;
        gridProcess.PagerSettings.Visible = false;
        FillGrid();
        FillDepartment();
        DropDownList ddlEdtDepartment = (DropDownList)gridProcess.Rows[e.NewEditIndex].FindControl("ddlEdtDepartment");
        ddlEdtDepartment.Items.FindByText(lblDepartmentName.Text).Selected = true;
    }

    protected void gridProcess_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gridProcess.EditIndex = -1;
        gridProcess.ShowFooter = true;
        gridProcess.FooterRow.Visible = true;
        gridProcess.PagerSettings.Visible = true;
        FillGrid();
        FillDepartment();
    }

    protected void gridProcess_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        Page page1 = HttpContext.Current.Handler as Page;
        Label lblProcessId = (Label)gridProcess.Rows[e.RowIndex].FindControl("lblProcessId");
        TextBox txtEdtProcessName = (TextBox)gridProcess.Rows[e.RowIndex].FindControl("txtEdtProcessName");
        DropDownList ddlEdtDepartment = (DropDownList)gridProcess.Rows[e.RowIndex].FindControl("ddlEdtDepartment");

        var UpdateData = from DBData in dbobj.ProcessMasters
                         where DBData.ProcessId == Convert.ToInt64(lblProcessId.Text)
                         select DBData;
        if (UpdateData.Count() > 0)
        {
            var DupData = (from DupP in dbobj.ProcessMasters
                           where DupP.ProcessName.ToUpper() == txtEdtProcessName.Text.Trim().ToUpper() && DupP.DepartmentId == Convert.ToInt64(ddlEdtDepartment.SelectedValue)
                           select DupP).Except(UpdateData);
            if (DupData.Count() > 0)
            {
                ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + DupData.Single().ProcessName.ToUpper() + " duplicate process" + "');", true);
            }
            else
            {

                var SingleUpdate = UpdateData.Single();
                SingleUpdate.ProcessName = txtEdtProcessName.Text.Trim().ToUpper();
                SingleUpdate.DepartmentId = Convert.ToInt64(ddlEdtDepartment.SelectedValue);
                SingleUpdate.ModifyBy = Convert.ToInt64(Global.UserId);
                SingleUpdate.ModifyDate = DateTime.Now;
                dbobj.SubmitChanges();
                dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                gridProcess.EditIndex = -1;
                gridProcess.ShowFooter = true;
                gridProcess.FooterRow.Visible = true;
                gridProcess.PagerSettings.Visible = true;
                FillGrid();
                FillDepartment();
            }
        }
    }

    protected void gridProcess_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridProcess.PageIndex = e.NewPageIndex;
        FillGrid();
        FillDepartment();
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
