using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class PriceType : System.Web.UI.Page
{
    MITInvoiceDataContext dbobj = new MITInvoiceDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Global.UserType == "BUSINESS UNIT MANAGER" || Global.UserType == "BUSINESS ASSOCIATES")
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

    protected void FillGrid()
    {
        var PriceType = from DBData in dbobj.PriceTypeNews
                        where DBData.IsDeleted == false
                        orderby DBData.PriceType
                        select new
                        {
                            PriceTypeId = DBData.PriceTypeId,
                            PriceType = DBData.PriceType
                        };
        if (PriceType.Count() > 0)
        {
            gridPriceType.DataSource = PriceType;
            gridPriceType.DataBind();
        }
        else
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PriceTypeId");
            dt.Columns.Add("PriceType");
            gridPriceType.DataSource = dt;
            dt.Rows.Add(dt.NewRow());
            gridPriceType.DataBind();
            int TotalCols = gridPriceType.Rows[0].Cells.Count;
            gridPriceType.Rows[0].Cells.Clear();
            gridPriceType.Rows[0].Cells.Add(new TableCell());
            gridPriceType.Rows[0].Cells[0].ColumnSpan = TotalCols;
            gridPriceType.Rows[0].Cells[0].Text = "No records to display";
        }
    }

    protected void gridPriceType_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Page page1 = HttpContext.Current.Handler as Page;
        TextBox txtPriceType = (TextBox)gridPriceType.FooterRow.FindControl("txtPriceType");
        switch (e.CommandName)
        {
            case "Insert":

                var DupPriceType = from DP in dbobj.PriceTypeNews
                                   where DP.PriceType == txtPriceType.Text.Trim().ToUpper()
                                   select DP;
                if (DupPriceType.Count() > 0)
                {
                    ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + txtPriceType.Text.Trim().ToUpper() + " price type already exists" + "')", true);                   
                }
                else
                {
                    PriceTypeNew pm = new PriceTypeNew();
                    pm.PriceType = txtPriceType.Text.Trim().ToUpper();
                    pm.IsDeleted = false;
                    pm.CreatedBy = Convert.ToInt64(Global.UserId);
                    pm.CreatedDate = DateTime.Now;
                    dbobj.PriceTypeNews.InsertOnSubmit(pm);
                    dbobj.SubmitChanges();
                    dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);

                    ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + txtPriceType.Text.Trim().ToUpper() + " price type add successfully" + "')", true);
                }
                FillGrid();
                break;
            case "Delete":
                GridViewRow gr = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                Label lblPriceTypeId = (Label)gr.FindControl("lblPriceTypeId");
                Label lblPriceType = (Label)gr.FindControl("lblPriceType");

                var DelPriceType = from DelPT in dbobj.PriceTypeNews
                                   where DelPT.PriceTypeId == Convert.ToInt64(lblPriceTypeId.Text)
                                   select DelPT;
                if (DelPriceType.Count() > 0)
                {
                    var PriceType = DelPriceType.Single();
                    PriceType.IsDeleted = true;
                    //dbobj.PriceTypeNews.DeleteOnSubmit(PriceType);
                    try
                    {
                        dbobj.SubmitChanges();
                        dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                        //ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + PriceType.PriceType.ToUpper() + " price type deleted" + "')", true);
                    }
                    catch
                    {
                        OpenWindow(this, "MITPLInvoice", "DeletePopup?id=" + lblPriceTypeId.Text + "&name=" + lblPriceType.Text + "&page=pricetype", 650, 350);
                    }
                }
                FillGrid();
                break;
        }
    }

    protected void gridPriceType_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }

    protected void gridPriceType_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gridPriceType.EditIndex = e.NewEditIndex;
        gridPriceType.ShowFooter = false;
        gridPriceType.FooterRow.Visible = false;
        gridPriceType.PagerSettings.Visible = false;
        FillGrid();
    }

    protected void gridPriceType_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gridPriceType.EditIndex = -1;
        gridPriceType.ShowFooter = true;
        gridPriceType.FooterRow.Visible = true;
        gridPriceType.PagerSettings.Visible = true;
        FillGrid();
    }

    protected void gridPriceType_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        Page page1 = HttpContext.Current.Handler as Page;
        Label lblPriceTypeId = (Label)gridPriceType.Rows[e.RowIndex].FindControl("lblPriceTypeId");
        TextBox txtEdtPriceType = (TextBox)gridPriceType.Rows[e.RowIndex].FindControl("txtEdtPriceType");

        var UpdateData = from DBData in dbobj.PriceTypeNews
                         where DBData.PriceTypeId == Convert.ToInt64(lblPriceTypeId.Text)
                         select DBData;
        if (UpdateData.Count() > 0)
        {
            var DupData = (from DupPT in dbobj.PriceTypeNews
                           where DupPT.PriceType.ToUpper() == txtEdtPriceType.Text.Trim().ToUpper()
                           select DupPT).Except(UpdateData);
            if (DupData.Count() > 0)
            {
                ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + DupData.Single().PriceType.ToUpper() + " duplicate price type" + "');", true);
            }
            else
            {
                var SingleUpdate = UpdateData.Single();
                SingleUpdate.PriceType = txtEdtPriceType.Text.Trim().ToUpper();
                SingleUpdate.ModifyBy = Convert.ToInt64(Global.UserId);
                SingleUpdate.ModifyDate = DateTime.Now;
                dbobj.SubmitChanges();
                dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                gridPriceType.EditIndex = -1;
                gridPriceType.ShowFooter = true;
                gridPriceType.FooterRow.Visible = true;
                gridPriceType.PagerSettings.Visible = true;
                FillGrid();
            }
        }
    }

    protected void gridPriceType_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridPriceType.PageIndex = e.NewPageIndex;
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
}
