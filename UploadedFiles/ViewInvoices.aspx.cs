using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class ViewInvoices : System.Web.UI.Page
{
    MITInvoiceDataContext dbobj = new MITInvoiceDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillBusinessUnit();
            if (drpBusiness.Items.Count > 0)
            {
                FillClient();
                FillGrid();
                ViewState["SendI"] = null;
            }
            else
                tbldrp.Visible = false;
        }
    }

    private void FillBusinessUnit()
    {
        if (Global.UserType == "DIRECTOR" || Global.DepartmentName == "ACCT")
        {
            var BUData = from DBData in dbobj.DepartmentMasters
                         where DBData.IsActive
                         select new
                         {
                             BUID = DBData.DepartmentId,
                             BUName = DBData.DepartmentName
                         };
            if (BUData.Count() > 0)
            {
                drpBusiness.DataSource = BUData;
                drpBusiness.DataTextField = "BUName";
                drpBusiness.DataValueField = "BUID";
                drpBusiness.DataBind();
                drpBusiness.Items.Insert(0, "--All--");
                drpBusiness.SelectedIndex = 0;
            }
        }
        else
        {
            var BUData = from DBData in dbobj.DepartmentMasters
                         where DBData.IsActive && DBData.UserId == Convert.ToInt64(Global.UserId)
                         select new
                         {
                             BUID = DBData.DepartmentId,
                             BUName = DBData.DepartmentName
                         };
            if (BUData.Count() > 0)
            {
                drpBusiness.DataSource = BUData;
                drpBusiness.DataTextField = "BUName";
                drpBusiness.DataValueField = "BUID";
                drpBusiness.DataBind();
                drpBusiness.SelectedIndex = 0;
            }
        }
    }

    private void BlankGrid()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("InvoiceID"));
        dt.Columns.Add(new DataColumn("InvoiceNo"));
        dt.Columns.Add(new DataColumn("ClientName"));
        dt.Columns.Add(new DataColumn("InvoiceAmount"));
        dt.Columns.Add(new DataColumn("InvoiceDate"));
        dt.Columns.Add(new DataColumn("PaidDate"));
        dt.Columns.Add(new DataColumn("InvoiceStatus"));

        gvInv.DataSource = dt;
        dt.Rows.Add(dt.NewRow());
        gvInv.DataBind();
        int TotalCols = gvInv.Rows[0].Cells.Count;
        gvInv.Rows[0].Cells.Clear();
        gvInv.Rows[0].Cells.Add(new TableCell());
        gvInv.Rows[0].Cells[0].ColumnSpan = TotalCols;
        gvInv.Rows[0].Cells[0].Text = "No Record to Display";
        gvInv.Columns[9].Visible = false;
        btnIntimate.Enabled = false;
    }

    private void FillGrid()
    {
        if (Global.UserType == "DIRECTOR" || Global.DepartmentName == "ACCT")
        {
            if (drpBusiness.SelectedIndex == 0)
            {
                if (ddlClient.SelectedIndex == 0)
                {
                    var InvoiceData = from DBData in dbobj.InvoiceMasters
                                      join
                                      ClientData in dbobj.ClientMasters
                                      on
                                      DBData.ClientId equals ClientData.ClientId
                                      join
                                      CurrencyData in dbobj.CurrencyMasters
                                      on
                                      ClientData.CurrencyId equals CurrencyData.CurrencyId
                                      where DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false
                                      select new
                                      {
                                          InvoiceID = DBData.InvoiceId,
                                          InvoiceNo = DBData.InvoiceNumber,
                                          ClientName = ClientData.ClientName,
                                          InvoiceAmount = "&#" + (CurrencyData.CurrencySymbol + ";").Replace(";;", ";") + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                                                                                                                  where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                                                                                                  select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                                                                                                                                                                          where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                                                                                                                                                          select InvData.TotalAmt.Value).Sum().ToString()),
                                          InvoiceDate = DBData.InvoiceDate,
                                          PaidDate = DBData.PaidDate.Value == DBData.PaidDate.Value == null ? "" : DBData.PaidDate.ToString(),
                                          InvoiceStatus = DBData.InvoiceStatus
                                      };
                    if (InvoiceData.Count() > 0)
                    {
                        gvInv.DataSource = InvoiceData;
                        gvInv.DataBind();
                    }
                    else
                        BlankGrid();
                }
                else
                {
                    var InvoiceData = from DBData in dbobj.InvoiceMasters
                                      join
                                      ClientData in dbobj.ClientMasters
                                      on
                                      DBData.ClientId equals ClientData.ClientId
                                      join
                                      CurrencyData in dbobj.CurrencyMasters
                                      on
                                      ClientData.CurrencyId equals CurrencyData.CurrencyId
                                      where DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && ClientData.ClientId == Convert.ToInt64(ddlClient.SelectedValue)
                                      select new
                                      {
                                          InvoiceID = DBData.InvoiceId,
                                          InvoiceNo = DBData.InvoiceNumber,
                                          ClientName = ClientData.ClientName,
                                          InvoiceAmount = "&#" + (CurrencyData.CurrencySymbol + ";").Replace(";;", ";") + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                                                                                                                  where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                                                                                                  select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                                                                                                                                                                          where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                                                                                                                                                          select InvData.TotalAmt.Value).Sum().ToString()),
                                          InvoiceDate = DBData.InvoiceDate,
                                          PaidDate = DBData.PaidDate.Value == DBData.PaidDate.Value == null ? "" : DBData.PaidDate.ToString(),
                                          InvoiceStatus = DBData.InvoiceStatus
                                      };
                    if (InvoiceData.Count() > 0)
                    {
                        gvInv.DataSource = InvoiceData;
                        gvInv.DataBind();
                    }
                    else
                        BlankGrid();
                }
            }
            else
            {
                if (ddlClient.SelectedIndex == 0)
                {
                    var InvoiceData = from DBData in dbobj.InvoiceMasters
                                      join
                                      ClientData in dbobj.ClientMasters
                                      on
                                      DBData.ClientId equals ClientData.ClientId
                                      join
                                      CurrencyData in dbobj.CurrencyMasters
                                      on
                                      ClientData.CurrencyId equals CurrencyData.CurrencyId
                                      where DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && ClientData.DepartmentId == Convert.ToInt64(drpBusiness.SelectedValue)
                                      select new
                                      {
                                          InvoiceID = DBData.InvoiceId,
                                          InvoiceNo = DBData.InvoiceNumber,
                                          ClientName = ClientData.ClientName,
                                          InvoiceAmount = "&#" + (CurrencyData.CurrencySymbol + ";").Replace(";;", ";") + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                                                                                                                  where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                                                                                                  select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                                                                                                                                                                          where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                                                                                                                                                          select InvData.TotalAmt.Value).Sum().ToString()),
                                          InvoiceDate = DBData.InvoiceDate,
                                          PaidDate = DBData.PaidDate.Value == DBData.PaidDate.Value == null ? "" : DBData.PaidDate.ToString(),
                                          InvoiceStatus = DBData.InvoiceStatus
                                      };
                    if (InvoiceData.Count() > 0)
                    {
                        gvInv.DataSource = InvoiceData;
                        gvInv.DataBind();
                    }
                    else
                        BlankGrid();
                }
                else
                {
                    var InvoiceData = from DBData in dbobj.InvoiceMasters
                                      join
                                      ClientData in dbobj.ClientMasters
                                      on
                                      DBData.ClientId equals ClientData.ClientId
                                      join
                                      CurrencyData in dbobj.CurrencyMasters
                                      on
                                      ClientData.CurrencyId equals CurrencyData.CurrencyId
                                      where DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && ClientData.ClientId == Convert.ToInt64(ddlClient.SelectedValue)
                                      && ClientData.DepartmentId == Convert.ToInt64(drpBusiness.SelectedValue)
                                      select new
                                      {
                                          InvoiceID = DBData.InvoiceId,
                                          InvoiceNo = DBData.InvoiceNumber,
                                          ClientName = ClientData.ClientName,
                                          InvoiceAmount = "&#" + (CurrencyData.CurrencySymbol + ";").Replace(";;", ";") + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                                                                                                                  where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                                                                                                  select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                                                                                                                                                                          where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                                                                                                                                                          select InvData.TotalAmt.Value).Sum().ToString()),
                                          InvoiceDate = DBData.InvoiceDate,
                                          PaidDate = DBData.PaidDate.Value == DBData.PaidDate.Value == null ? "" : DBData.PaidDate.ToString(),
                                          InvoiceStatus = DBData.InvoiceStatus
                                      };
                    if (InvoiceData.Count() > 0)
                    {
                        gvInv.DataSource = InvoiceData;
                        gvInv.DataBind();
                    }
                    else
                        BlankGrid();
                }
            }
        }
        else
        {
            if (ddlClient.SelectedIndex == 0)
            {
                var InvoiceData = from DBData in dbobj.InvoiceMasters
                                  join
                                  ClientData in dbobj.ClientMasters
                                  on
                                  DBData.ClientId equals ClientData.ClientId
                                  join
                                  CurrencyData in dbobj.CurrencyMasters
                                  on
                                  ClientData.CurrencyId equals CurrencyData.CurrencyId
                                  where ClientData.DepartmentId == Convert.ToInt64(drpBusiness.SelectedValue) && DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false
                                  select new
                                  {
                                      InvoiceID = DBData.InvoiceId,
                                      InvoiceNo = DBData.InvoiceNumber,
                                      ClientName = ClientData.ClientName,
                                      InvoiceAmount = "&#" + (CurrencyData.CurrencySymbol + ";").Replace(";;", ";") + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                                                                                                              where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                                                                                              select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                                                                                                                                                                      where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                                                                                                                                                      select InvData.TotalAmt.Value).Sum().ToString()),
                                      InvoiceDate = DBData.InvoiceDate,
                                      PaidDate = DBData.PaidDate.Value == DBData.PaidDate.Value == null ? "" : DBData.PaidDate.ToString(),
                                      InvoiceStatus = DBData.InvoiceStatus
                                  };
                if (InvoiceData.Count() > 0)
                {
                    gvInv.DataSource = InvoiceData;
                    gvInv.DataBind();
                }
                else
                    BlankGrid();
            }
            else
            {
                var InvoiceData = from DBData in dbobj.InvoiceMasters
                                  join
                                  ClientData in dbobj.ClientMasters
                                  on
                                  DBData.ClientId equals ClientData.ClientId
                                  join
                                  CurrencyData in dbobj.CurrencyMasters
                                  on
                                  ClientData.CurrencyId equals CurrencyData.CurrencyId
                                  where ClientData.DepartmentId == Convert.ToInt64(drpBusiness.SelectedValue) && DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false
                                        && ClientData.ClientId == Convert.ToInt64(ddlClient.SelectedValue)
                                  select new
                                  {
                                      InvoiceID = DBData.InvoiceId,
                                      InvoiceNo = DBData.InvoiceNumber,
                                      ClientName = ClientData.ClientName,
                                      InvoiceAmount = "&#" + (CurrencyData.CurrencySymbol + ";").Replace(";;", ";") + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                                                                                                              where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                                                                                              select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                                                                                                                                                                      where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                                                                                                                                                      select InvData.TotalAmt.Value).Sum().ToString()),
                                      InvoiceDate = DBData.InvoiceDate,
                                      PaidDate = DBData.PaidDate.Value == DBData.PaidDate.Value == null ? "" : DBData.PaidDate.ToString(),
                                      InvoiceStatus = DBData.InvoiceStatus
                                  };
                if (InvoiceData.Count() > 0)
                {
                    gvInv.DataSource = InvoiceData;
                    gvInv.DataBind();
                }
                else
                    BlankGrid();
            }
        }

    }

    protected void drpBusiness_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
        FillClient();
        if (ddlClient.SelectedIndex == 0 && drpInvoiceStatus.Text == "Unpaid")
        {
            gvInv.Columns[9].Visible = false;
            btnIntimate.Visible = false;
        }
        else
        {
            if (gvInv.Rows[0].Cells[0].Text != "No Record to Display" && drpInvoiceStatus.Text == "Unpaid" && Global.UserType == "DIRECTOR" && drpBusiness.SelectedIndex != 0)
            {
                btnIntimate.Visible = true;
                gvInv.Columns[9].Visible = true;
            }
            else
            {
                gvInv.Columns[9].Visible = false;
                btnIntimate.Visible = false;
            }
        }
        ViewState["SendI"] = "";
        lbltest.Text = ViewState["SendI"].ToString();
    }

    protected void drpInvoiceStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        //TextBox txtMasterSearch = (TextBox)this.Master.FindControl("txtMasterSearch");
        //txtMasterSearch.Text = "";
        FillGrid();
        if (ddlClient.SelectedIndex == 0 && drpInvoiceStatus.Text == "Unpaid")
        {
            gvInv.Columns[9].Visible = false;
            btnIntimate.Visible = false;
        }
        else
        {
            if (gvInv.Rows[0].Cells[0].Text != "No Record to Display" && drpInvoiceStatus.Text == "Unpaid" && Global.UserType == "DIRECTOR" && drpBusiness.SelectedIndex != 0)
            {
                gvInv.Columns[9].Visible = true;
                btnIntimate.Visible = true;
            }
            else
            {
                btnIntimate.Visible = false;
                gvInv.Columns[9].Visible = false;
            }
        }
        ViewState["SendI"] = "";
        lbltest.Text = ViewState["SendI"].ToString();
    }

    protected void gvInv_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ddlClient.SelectedIndex != 0 && drpBusiness.SelectedIndex != 0)
            {
                var InvoiceData = (from DBData in dbobj.InvoiceMasters
                                   join
                                   ClientData in dbobj.ClientMasters
                                   on
                                   DBData.ClientId equals ClientData.ClientId
                                   join
                                   CurrencyData in dbobj.CurrencyMasters
                                   on
                                   ClientData.CurrencyId equals CurrencyData.CurrencyId
                                   where DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && ClientData.ClientId == Convert.ToInt64(ddlClient.SelectedValue)
                                   && ClientData.DepartmentId == Convert.ToInt64(drpBusiness.SelectedValue)
                                   select new
                                   {
                                       DBData.InvoiceId
                                   }).ToArray();
                if (ViewState["SendI"] != null)
                {
                    string[] f = ViewState["SendI"].ToString().Split(',');
                    CheckBox chkHdrALL = (CheckBox)e.Row.FindControl("chkSelectAll");
                    for (int h = 0; h < InvoiceData.Count(); h++)
                    {

                        if (InvoiceData[h].InvoiceId.ToString() == f[h])
                        {
                            chkHdrALL.Checked = true;
                        }
                        else
                        {
                            chkHdrALL.Checked = false;
                            return;
                        }
                    }
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.EmptyDataRow)
        {
            ImageButton imgbtnEdit = (ImageButton)e.Row.FindControl("imgbtnEdit");
            ImageButton imgbtnDelete = (ImageButton)e.Row.FindControl("imgbtnDelete");
            if (Global.UserType == "DIRECTOR")
            {
                if (imgbtnDelete != null)
                    imgbtnDelete.Visible = true;
                if (drpInvoiceStatus.SelectedItem.ToString() == "Unpaid")
                {
                    if (imgbtnEdit != null)
                        imgbtnEdit.Visible = true;
                    if (imgbtnDelete != null)
                        imgbtnDelete.Visible = false;
                }
            }
            else
            {
                if (drpInvoiceStatus.SelectedItem.ToString() == "Draft")
                {
                    if (imgbtnDelete != null)
                        imgbtnDelete.Visible = true;
                }
            }
            Label lblInvoicePaidDate = (Label)e.Row.FindControl("lblInvoicePaidDate");
            if (lblInvoicePaidDate != null)
            {
                if (lblInvoicePaidDate.Text != "")
                    lblInvoicePaidDate.Text = Convert.ToDateTime(lblInvoicePaidDate.Text).ToString("dd-MMM-yyyy");
            }

            HyperLink hypreInv = (HyperLink)e.Row.FindControl("hyperlnkInvoiceNo");
            Label lblInvID = (Label)e.Row.FindControl("lblInvoiceID");
            CheckBox chkcl = (CheckBox)e.Row.FindControl("chkClient");
            if (ViewState["SendI"] != null)
            {
                string[] Inv = ViewState["SendI"].ToString().Split(',');
                for (int j = 0; j < Inv.Count(); j++)
                {
                    if (Inv[j].Equals(lblInvID.Text))
                    {
                        chkcl.Checked = true;
                    }
                }
            }

            hypreInv.NavigateUrl = "NewInvoice?InvID=" + Global.Encrypt(lblInvID.Text);
        }
    }

    protected void gvInv_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvInv.EditIndex = -1;
        gvInv.ShowFooter = true;
        gvInv.FooterRow.Visible = true;
        gvInv.PagerSettings.Visible = true;
        FillGrid();
    }

    protected void gvInv_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvInv.EditIndex = e.NewEditIndex;
        gvInv.ShowFooter = false;
        gvInv.FooterRow.Visible = false;
        gvInv.PagerSettings.Visible = false;
        FillGrid();
        TextBox txtInvoicePaidDate = (TextBox)gvInv.Rows[e.NewEditIndex].FindControl("txtInvoicePaidDate");
        txtInvoicePaidDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
    }

    protected void gvInv_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        Label lblInvoiceID = (Label)gvInv.Rows[e.RowIndex].FindControl("lblInvoiceID");
        TextBox txtInvoicePaidDate = (TextBox)gvInv.Rows[e.RowIndex].FindControl("txtInvoicePaidDate");
        DropDownList drpStatus = (DropDownList)gvInv.Rows[e.RowIndex].FindControl("drpStatus");
        var UpdateData = from DBInvoice in dbobj.InvoiceMasters
                         where DBInvoice.InvoiceId == Convert.ToInt64(lblInvoiceID.Text)
                         select DBInvoice;
        if (UpdateData.Count() > 0)
        {
            var SingleUpdate = UpdateData.Single();
            SingleUpdate.InvoiceStatus = drpStatus.SelectedItem.ToString();
            SingleUpdate.IsPaid = true;
            SingleUpdate.PaidDate = Convert.ToDateTime(txtInvoicePaidDate.Text);
            SingleUpdate.ModifyBy = Convert.ToInt64(Global.UserId);
            SingleUpdate.ModifyDate = DateTime.Now;
            dbobj.SubmitChanges();
            dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
            gvInv.EditIndex = -1;
            gvInv.ShowFooter = true;
            gvInv.FooterRow.Visible = true;
            gvInv.PagerSettings.Visible = true;
            FillGrid();
        }
    }

    protected void gvInv_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        Label lblInvoiceID = (Label)gvInv.Rows[e.RowIndex].FindControl("lblInvoiceID");
        var DelData = from DBInvoice in dbobj.InvoiceMasters
                      where DBInvoice.InvoiceId == Convert.ToInt64(lblInvoiceID.Text)
                      select DBInvoice;
        if (DelData.Count() > 0)
        {
            var SingleDelete = DelData.Single();
            SingleDelete.ModifyBy = Convert.ToInt64(Global.UserId);
            SingleDelete.ModifyDate = DateTime.Now;
            SingleDelete.IsDeleted = true;
            dbobj.SubmitChanges();
            dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
            gvInv.EditIndex = -1;
            gvInv.ShowFooter = true;
            gvInv.FooterRow.Visible = true;
            gvInv.PagerSettings.Visible = true;
            FillGrid();
        }
    }

    protected void gvInv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvInv.PageIndex = e.NewPageIndex;
        FillGrid();
    }

    protected void FillClient()
    {
        if (Global.UserType == "DIRECTOR" || Global.DepartmentName == "ACCT")
        {
            var ClientData = from dbClient in dbobj.ClientMasters
                             select new
                             {
                                 Id = dbClient.ClientId,
                                 CName = dbClient.ClientName
                             };
            if (ClientData.Count() > 0)
            {
                ddlClient.DataSource = ClientData;
                ddlClient.DataValueField = "Id";
                ddlClient.DataTextField = "CName";
                ddlClient.DataBind();
            }
        }
        else
        {
            var ClientData = from dbClient in dbobj.ClientMasters
                             join dbDepartment in dbobj.DepartmentMasters
                             on dbClient.DepartmentId equals dbDepartment.DepartmentId
                             join dbUser in dbobj.UserMasters
                             on dbDepartment.UserId equals dbUser.UserId
                             where dbClient.DepartmentId == Convert.ToInt64(drpBusiness.SelectedValue)
                             // && dbUser.UserId == Convert.ToInt64(Global.UserId) 
                             select new
                             {
                                 Id = dbClient.ClientId,
                                 CName = dbClient.ClientName
                             };
            if (ClientData.Count() > 0)
            {
                ddlClient.DataSource = ClientData;
                ddlClient.DataValueField = "Id";
                ddlClient.DataTextField = "CName";
                ddlClient.DataBind();
            }
        }
        ddlClient.Items.Insert(0, "--Select--");
    }

    protected void ddlClient_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
        if (ddlClient.SelectedIndex == 0 && drpInvoiceStatus.Text == "Unpaid")
        {
            gvInv.Columns[9].Visible = false;
            btnIntimate.Visible = false;
        }
        else
        {
            if (gvInv.Rows[0].Cells[0].Text != "No Record to Display" && drpInvoiceStatus.Text == "Unpaid" && Global.UserType == "DIRECTOR" && drpBusiness.SelectedIndex != 0)
            {
                gvInv.Columns[9].Visible = true;
                btnIntimate.Visible = true;
            }
            else
            {
                gvInv.Columns[9].Visible = false;
                btnIntimate.Visible = false;
            }
        }
        ViewState["SendI"] = "";
        lbltest.Text = ViewState["SendI"].ToString();
    }

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)sender;
        GridViewRow gr = (GridViewRow)chk.NamingContainer;
        CheckBox chkSelectAll = (CheckBox)gr.FindControl("chkSelectAll");
        ////if (gvInv != null)
        ////{
        ////    foreach (GridViewRow row in gvInv.Rows)
        ////    {
        ////        CheckBox chkClient = (CheckBox)row.FindControl("chkClient"); 
        ////        if (chkSelectAll.Checked)
        ////        {
        ////            chkClient.Checked = true;
        ////            btnIntimate.Enabled = true;
        ////        }
        ////        else
        ////        {
        ////            btnIntimate.Enabled = false;
        ////            chkClient.Checked = false;
        ////        }
        ////    }
        ////}

        var InvoiceData = from DBData in dbobj.InvoiceMasters
                          join
                          ClientData in dbobj.ClientMasters
                          on
                          DBData.ClientId equals ClientData.ClientId
                          join
                          CurrencyData in dbobj.CurrencyMasters
                          on
                          ClientData.CurrencyId equals CurrencyData.CurrencyId
                          where DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && ClientData.ClientId == Convert.ToInt64(ddlClient.SelectedValue)
                          && ClientData.DepartmentId == Convert.ToInt64(drpBusiness.SelectedValue)
                          select new
                          {
                              InvoiceID = DBData.InvoiceId
                          };
        if (InvoiceData.Count() > 0)
        {
            ViewState["SendI"] = "";


            foreach (GridViewRow row in gvInv.Rows)
            {
                CheckBox chkClient = (CheckBox)row.FindControl("chkClient");
                chkClient.Checked = true;
            }
            foreach (var d in InvoiceData)
            {
                ViewState["SendI"] = ViewState["SendI"].ToString() + d.InvoiceID.ToString() + ",";
            }
            lbltest.Text = ViewState["SendI"].ToString();
        }
        if (chkSelectAll.Checked == false)
        {
            ViewState["SendI"] = "";
            lbltest.Text = ViewState["SendI"].ToString();
            foreach (GridViewRow row in gvInv.Rows)
            {
                CheckBox chkClient = (CheckBox)row.FindControl("chkClient");
                chkClient.Checked = false;
            }
        }

        if (ViewState["SendI"] != null && ViewState["SendI"].ToString() != "" && ViewState["SendI"].ToString() != " ")
        {
            btnIntimate.Enabled = true;
        }
        else
            btnIntimate.Enabled = false;
    }

    protected void chkClient_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)sender;
        GridViewRow gr = (GridViewRow)chk.NamingContainer;
        CheckBox chkClient = (CheckBox)gr.FindControl("chkClient");

        var InvoiceData = from DBData in dbobj.InvoiceMasters
                          join
                          ClientData in dbobj.ClientMasters
                          on
                          DBData.ClientId equals ClientData.ClientId
                          join
                          CurrencyData in dbobj.CurrencyMasters
                          on
                          ClientData.CurrencyId equals CurrencyData.CurrencyId
                          where DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && ClientData.ClientId == Convert.ToInt64(ddlClient.SelectedValue)
                          && ClientData.DepartmentId == Convert.ToInt64(drpBusiness.SelectedValue)
                          select new
                          {
                              InvoiceID = DBData.InvoiceId
                          };

        int RowsNo = InvoiceData.Count();
        CheckBox chkAll = (CheckBox)(gvInv.HeaderRow.FindControl("chkSelectAll"));

        //ViewState["SendI"] = null;

        CheckBox ckcl = (CheckBox)(gr.FindControl("chkClient"));
        Label lblIdInv = (Label)(gr.FindControl("lblInvoiceID"));
        if (ckcl.Checked == true)
        {
            ViewState["SendI"] = ViewState["SendI"] + lblIdInv.Text + ",";
            lbltest.Text = ViewState["SendI"].ToString();
        }
        else
        {
            ViewState["SendI"] = ViewState["SendI"].ToString().Replace(lblIdInv.Text + ",", "");
            lbltest.Text = ViewState["SendI"].ToString();
        }

        //for (int i = 0; i < RowsNo; i++)
        //{
        //    CheckBox ckcl = (CheckBox)(gvInv.Rows[i].FindControl("chkClient"));
        //    Label lblIdInv = (Label)(gvInv.Rows[i].FindControl("lblInvoiceID"));
        //    if (ckcl.Checked == true)
        //    {
        //        cnt += 1;
        //        btnIntimate.Enabled = true;
        //        ViewState["SendI"] = ViewState["SendI"] + lblIdInv.Text + ",";
        //        lbltest.Text = ViewState["SendI"].ToString();
        //    }
        //    else
        //    {
        //        ViewState["SendI"] = ViewState["SendI"].ToString();
        //    }
        //}
        if (ViewState["SendI"] != null)
        {

            string[] g = ViewState["SendI"].ToString().Split(',');

            int cnt = g.Count();

            if (RowsNo == cnt - 1)
            {
                chkAll.Checked = true;
                ViewState["SendI"] = "";
                foreach (var d in InvoiceData)
                {
                    ViewState["SendI"] = ViewState["SendI"].ToString() + d.InvoiceID.ToString() + ",";
                    lbltest.Text = ViewState["SendI"].ToString();
                }
            }
            else
            {
                chkAll.Checked = false;

            }
        }
        else
            chkAll.Checked = false;

        if (ViewState["SendI"] != null && ViewState["SendI"].ToString() != "" && ViewState["SendI"].ToString() != " ")
        {
            btnIntimate.Enabled = true;
        }
        else
            btnIntimate.Enabled = false;

        ////for (int i = 0; i < RowsNo; i++)
        ////{
        ////    CheckBox ckcla = (CheckBox)(gvInv.Rows[i].FindControl("chkClient"));
        ////    if (ckcla.Checked == true)
        ////    {
        ////        btnIntimate.Enabled = true;
        ////        return;
        ////    }
        ////    else
        ////    {
        ////        btnIntimate.Enabled = false;
        ////    }
        ////}
    }

    protected void btnIntimate_Click(object sender, EventArgs e)
    {


        Response.Redirect("SendInvoiceEmail?" + Global.Encrypt("i=" + ViewState["SendI"].ToString().Substring(0, ViewState["SendI"].ToString().Length - 1) + "&P=F"));
        //int cnt = 0;
        //foreach (GridViewRow row in gvInv.Rows)
        //{
        //    CheckBox chkClient = (CheckBox)row.FindControl("chkClient");
        //    if (chkClient.Checked)
        //    {
        //        cnt += 1;
        //    }
        //}
        //Page page1 = HttpContext.Current.Handler as Page;
        //ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + cnt + "')", true);
    }
}
