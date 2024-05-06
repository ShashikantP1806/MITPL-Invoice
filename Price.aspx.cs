using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Price : System.Web.UI.Page
{
    MITInvoiceDataContext dbobj = new MITInvoiceDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Global.UserType == "BUSINESS UNIT MANAGER")
        {
            if (!IsPostBack)
            {
                FillGrid();
                FillClient();
                FillPriceType();
                FillProcess();
            }
        }
        else
        {
            Response.Redirect("Authorize");
        }
    }

    protected void FillGrid()
    {
        var Price = from DBData in dbobj.PriceMasters
                    where DBData.IsDeleted == false
                    orderby DBData.ClientMaster.ClientName
                    select new
                    {
                        PriceId = DBData.PriceId,
                        ClientName = DBData.ClientMaster.ClientName,
                        PriceType = DBData.PriceTypeMaster.PriceType,
                        Process = DBData.ProcessMaster.ProcessName,
                        UnitPrice = DBData.UnitPrice
                    };
        if (Price.Count() > 0)
        {
            gridPrice.DataSource = Price;
            gridPrice.DataBind();
        }
        else
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PriceId");
            dt.Columns.Add("ClientName");
            dt.Columns.Add("PriceType");
            dt.Columns.Add("Process");
            dt.Columns.Add("UnitPrice");
            gridPrice.DataSource = dt;
            dt.Rows.Add(dt.NewRow());
            gridPrice.DataBind();
            int TotalCols = gridPrice.Rows[0].Cells.Count;
            gridPrice.Rows[0].Cells.Clear();
            gridPrice.Rows[0].Cells.Add(new TableCell());
            gridPrice.Rows[0].Cells[0].ColumnSpan = TotalCols;
            gridPrice.Rows[0].Cells[0].Text = "No records to display";
        }
    }

    protected void FillClient()
    {
        DropDownList ddlClient = (DropDownList)gridPrice.FooterRow.FindControl("ddlClient");
        var Client = from DBData in dbobj.ClientMasters
                     where DBData.IsActive == true
                     orderby DBData.ClientName
                     select new
                     {
                         ClientId = DBData.ClientId,
                         ClientName = DBData.ClientName
                     };
        if (Client.Count() > 0)
        {
            ddlClient.DataSource = Client;
            ddlClient.DataTextField = "ClientName";
            ddlClient.DataValueField = "ClientId";
            ddlClient.DataBind();
        }
        ddlClient.Items.Insert(0, "-- Select --");
    }

    protected void FillPriceType()
    {
        DropDownList ddlPriceType = (DropDownList)gridPrice.FooterRow.FindControl("ddlPriceType");
        var PriceType = from DBData in dbobj.PriceTypeMasters
                        orderby DBData.PriceType
                        select new
                        {
                            PriceType = DBData.PriceType,
                            PriceTypeId = DBData.PriceTypeId
                        };
        if (PriceType.Count() > 0)
        {
            ddlPriceType.DataSource = PriceType;
            ddlPriceType.DataTextField = "PriceType";
            ddlPriceType.DataValueField = "PriceTypeId";
            ddlPriceType.DataBind();
        }
        ddlPriceType.Items.Insert(0, "-- Select --");
    }

    protected void FillProcess()
    {
        DropDownList ddlProcess = (DropDownList)gridPrice.FooterRow.FindControl("ddlProcess");
        var Process = from DBData in dbobj.ProcessMasters
                      orderby DBData.ProcessName
                      select new
                      {
                          ProcessId = DBData.ProcessId,
                          ProcessName = DBData.ProcessName
                      };
        if (Process.Count() > 0)
        {
            ddlProcess.DataSource = Process;
            ddlProcess.DataTextField = "ProcessName";
            ddlProcess.DataValueField = "ProcessId";
            ddlProcess.DataBind();
        }
        ddlProcess.Items.Insert(0, "-- Select --");
    }

    protected void gridPrice_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Page page1 = HttpContext.Current.Handler as Page;
        DropDownList ddlClient = (DropDownList)gridPrice.FooterRow.FindControl("ddlClient");
        DropDownList ddlPriceType = (DropDownList)gridPrice.FooterRow.FindControl("ddlPriceType");
        TextBox txtUnitPrice = (TextBox)gridPrice.FooterRow.FindControl("txtUnitPrice");
        DropDownList ddlProcess = (DropDownList)gridPrice.FooterRow.FindControl("ddlProcess");
        switch (e.CommandName)
        {
            case "Insert":
                var DupP = from DBData in dbobj.PriceMasters
                           where DBData.IsDeleted == false && DBData.ClientId == Convert.ToInt64(ddlClient.SelectedValue) && DBData.PriceTypeId == Convert.ToInt64(ddlPriceType.SelectedValue) && DBData.ProcessId == Convert.ToInt64(ddlProcess.SelectedValue)
                           select DBData;
                if (DupP.Count() == 0)
                {
                    PriceMaster pm = new PriceMaster();
                    pm.ClientId = Convert.ToInt64(ddlClient.SelectedValue);
                    pm.PriceTypeId = Convert.ToInt64(ddlPriceType.SelectedValue);
                    pm.UnitPrice = Convert.ToDecimal(txtUnitPrice.Text.Trim());
                    pm.ProcessId = Convert.ToInt64(ddlProcess.SelectedValue);
                    pm.IsDeleted = false;
                    pm.CreatedBy = Convert.ToInt64(Global.UserId);
                    dbobj.PriceMasters.InsertOnSubmit(pm);
                    dbobj.SubmitChanges();
                    dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                    ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('Unit price successfully add')", true);
                    FillGrid();
                    FillClient();
                    FillPriceType();
                    FillProcess();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('Unit price already exists')", true);
                }
                break;
            case "Delete":
                GridViewRow gr = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                Label lblPriceId = (Label)gr.FindControl("lblPriceId");
                var DelPrice = from DelP in dbobj.PriceMasters
                               where DelP.PriceId == Convert.ToInt64(lblPriceId.Text)
                               select DelP;
                if (DelPrice.Count() > 0)
                {
                    var Price = DelPrice.Single();
                    Price.IsDeleted = true;
                    dbobj.SubmitChanges();
                    dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                    ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('Unit price successfully deleted')", true);
                    FillGrid();
                    FillClient();
                    FillPriceType();
                    FillProcess();
                }
                break;
        }
    }

    protected void gridPrice_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }

    protected void gridPrice_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gridPrice.EditIndex = e.NewEditIndex;
        gridPrice.ShowFooter = false;
        gridPrice.FooterRow.Visible = false;
        gridPrice.PagerSettings.Visible = false;
        Label lblClient = (Label)gridPrice.Rows[e.NewEditIndex].FindControl("lblClient");
        Label lblPriceType = (Label)gridPrice.Rows[e.NewEditIndex].FindControl("lblPriceType");
        Label lblProcess = (Label)gridPrice.Rows[e.NewEditIndex].FindControl("lblProcess");
        FillGrid();
        DropDownList ddlEdtClient = (DropDownList)gridPrice.Rows[e.NewEditIndex].FindControl("ddlEdtClient");
        DropDownList ddlEdtPriceType = (DropDownList)gridPrice.Rows[e.NewEditIndex].FindControl("ddlEdtPriceType");
        DropDownList ddlEdtProcess = (DropDownList)gridPrice.Rows[e.NewEditIndex].FindControl("ddlEdtProcess");

        ddlEdtClient.Items.FindByText(lblClient.Text).Selected = true;
        ddlEdtPriceType.Items.FindByText(lblPriceType.Text).Selected = true;
        ddlEdtProcess.Items.FindByText(lblProcess.Text).Selected = true;
    }

    protected void gridPrice_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gridPrice.EditIndex = -1;
        gridPrice.ShowFooter = true;
        gridPrice.FooterRow.Visible = true;
        gridPrice.PagerSettings.Visible = true;
        FillGrid();
        FillClient();
        FillPriceType();
        FillProcess();
    }

    protected void gridPrice_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        Page page1 = HttpContext.Current.Handler as Page;
        Label lblPriceId = (Label)gridPrice.Rows[e.RowIndex].FindControl("lblPriceId");
        DropDownList ddlEdtClient = (DropDownList)gridPrice.Rows[e.RowIndex].FindControl("ddlEdtClient");
        DropDownList ddlEdtPriceType = (DropDownList)gridPrice.Rows[e.RowIndex].FindControl("ddlEdtPriceType");
        TextBox txtEdtUnitPrice = (TextBox)gridPrice.Rows[e.RowIndex].FindControl("txtEdtUnitPrice");
        DropDownList ddlEdtProcess = (DropDownList)gridPrice.Rows[e.RowIndex].FindControl("ddlEdtProcess");

        var UpdateData = from DBData in dbobj.PriceMasters
                         where DBData.PriceId == Convert.ToInt64(lblPriceId.Text)
                         select DBData;
        if (UpdateData.Count() > 0)
        {
            var DupP = (from DBData in dbobj.PriceMasters
                       where DBData.IsDeleted == false && DBData.ClientId == Convert.ToInt64(ddlEdtClient.SelectedValue) && DBData.PriceTypeId == Convert.ToInt64(ddlEdtPriceType.SelectedValue) && DBData.ProcessId == Convert.ToInt64(ddlEdtProcess.SelectedValue)
                       select DBData).Except(UpdateData);
            if (DupP.Count() == 0)
            {
                var SingleUpdate = UpdateData.Single();
                SingleUpdate.ClientId = Convert.ToInt64(ddlEdtClient.SelectedValue);
                SingleUpdate.PriceTypeId = Convert.ToInt64(ddlEdtPriceType.SelectedValue);
                SingleUpdate.UnitPrice = Convert.ToDecimal(txtEdtUnitPrice.Text.Trim());
                SingleUpdate.ProcessId = Convert.ToInt64(ddlEdtProcess.SelectedValue);
                SingleUpdate.ModifyBy = Convert.ToInt64(Global.UserId);
                SingleUpdate.ModifyDate = DateTime.Now;
                dbobj.SubmitChanges();
                dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                gridPrice.EditIndex = -1;
                gridPrice.ShowFooter = true;
                gridPrice.FooterRow.Visible = true;
                gridPrice.PagerSettings.Visible = true;
                FillGrid();
                FillClient();
                FillPriceType();
                FillProcess();
            }
            else
            {
                ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('Unit price already exists')", true);
            }
        }
    }

    protected void gridPrice_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridPrice.PageIndex = e.NewPageIndex;
        FillGrid();
        FillClient();
        FillPriceType();
        FillProcess();
    }
}
