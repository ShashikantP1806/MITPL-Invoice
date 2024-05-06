using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ViewProformaInvoices : System.Web.UI.Page
{
    MITInvoiceDataContext dbobj = new MITInvoiceDataContext();
    Int64 DepartmentName = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Global.UserType != "ADMINISTRATOR")
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
            if ((Global.UserType == "BUSINESS UNIT MANAGER" || (Global.UserType == "BUSINESS ASSOCIATES" && Global.DepartmentName != "ACCT")) && drpInvoiceStatus.SelectedItem.ToString() != "Draft")
            {
                gvInv.Columns[11].Visible = false; // Edit | Delete Column
            }
            else
            {
                gvInv.Columns[11].Visible = true; // Edit | Delete Column

            }

            
        }
        else
        {
            Response.Redirect("Authorize");
        }


        if (Global.UserType == "DIRECTOR" || Convert.ToBoolean(Global.UserProformaApproveAccess) == true)
        {
            gvInv.Columns[10].Visible = true; // Approved column
        }
        else
        {
            gvInv.Columns[10].Visible = false; // Approved column
        }
        gvInv.Columns[13].Visible = false; // ApprovedBy column
    }

    private void FillBusinessUnit()
    {
        if (Global.UserType == "DIRECTOR" || Global.DepartmentName == "ACCT")
        {
            var BUData = from DBData in dbobj.DepartmentMasters
                         orderby DBData.DepartmentName
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

            }

            if (Global.UserType == "DIRECTOR" && !Global.UserM1 && Global.UserM2)
            {
                string strDeptName = "SYSTEM";
                ListItem li1 = drpBusiness.Items.FindByText(strDeptName);
                if (li1 != null)
                    drpBusiness.Items.FindByText(strDeptName).Selected = true;

                drpBusiness.Enabled = false;
            }
            else
                drpBusiness.SelectedIndex = 0;

        }
        else
        {
            var BUData = from DBData in dbobj.DepartmentMasters
                         where DBData.IsActive && (DBData.UserId == Convert.ToInt64(Global.UserId) || DBData.DepartmentId == Convert.ToInt64(Global.Department))
                         orderby DBData.DepartmentName
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

    protected void FillClient()
    {
        ddlClient.Items.Clear();
        if (Global.UserType == "DIRECTOR" || Global.DepartmentName == "ACCT")
        {
            if (drpBusiness.SelectedIndex == 0)
            {
                var ClientData = from dbClient in dbobj.ClientMasters
                                 where dbClient.C_M1 == Global.UserM1 || dbClient.C_M2 == Global.UserM2
                                 orderby dbClient.ClientName
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
                                 where dbClient.DepartmentId == Convert.ToInt64(drpBusiness.SelectedValue) && (dbClient.C_M1 == Global.UserM1 || dbClient.C_M2 == Global.UserM2)
                                 orderby dbClient.ClientName
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
        }
        else
        {
            var ClientData = from dbClient in dbobj.ClientMasters
                             join dbDepartment in dbobj.DepartmentMasters
                             on dbClient.DepartmentId equals dbDepartment.DepartmentId
                             join dbUser in dbobj.UserMasters
                             on dbDepartment.UserId equals dbUser.UserId
                             where dbClient.DepartmentId == Convert.ToInt64(drpBusiness.SelectedValue) && (dbClient.C_M1 == Global.UserM1 || dbClient.C_M2 == Global.UserM2)
                             orderby dbClient.ClientName
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
        ddlClient.Items.Insert(0, "--All--");
    }

    private void BlankGrid()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("InvoiceID"));
        dt.Columns.Add(new DataColumn("InvoiceFor"));
        dt.Columns.Add(new DataColumn("InvoiceNum"));
        dt.Columns.Add(new DataColumn("MainInvoiceNumber"));
        dt.Columns.Add(new DataColumn("InvSeq"));
        dt.Columns.Add(new DataColumn("ClientName"));
        dt.Columns.Add(new DataColumn("InvoiceAmount"));
        dt.Columns.Add(new DataColumn("InvoiceDate"));
        dt.Columns.Add(new DataColumn("PaidDate"));
        dt.Columns.Add(new DataColumn("InvoiceStatus"));
        dt.Columns.Add(new DataColumn("InvoiceApprovedBy"));
        dt.Columns.Add(new DataColumn("MainInvoiceID"));

        gvInv.DataSource = dt;
        dt.Rows.Add(dt.NewRow());
        gvInv.DataBind();
        int TotalCols = gvInv.Rows[0].Cells.Count;
        gvInv.Rows[0].Cells.Clear();
        gvInv.Rows[0].Cells.Add(new TableCell());
        gvInv.Rows[0].Cells[0].ColumnSpan = TotalCols;
        gvInv.Rows[0].Cells[0].Text = "No Record to Display";
        gvInv.Columns[12].Visible = false;
    }

    public void FillGrid()
    {
        TextBox txtMasterSearch = (TextBox)this.Master.FindControl("txtMasterSearch");
        if (txtMasterSearch.Text == "")
        {
            if (Global.UserType == "DIRECTOR" || Global.DepartmentName == "ACCT")
            {
                if (drpBusiness.SelectedIndex == 0)
                {
                    if (ddlClient.SelectedIndex == 0)
                    {
                        var InvoiceData = from DBData in dbobj.ProformaInvoiceMasters
                                          join
                                          ClientData in dbobj.ClientMasters
                                          on
                                          DBData.ClientId equals ClientData.ClientId
                                          join
                                          CurrencyData in dbobj.CurrencyMasters
                                          on
                                          ClientData.CurrencyId equals CurrencyData.CurrencyId
                                          where DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2)
                                          select new
                                          {
                                              InvoiceID = DBData.ProInvoiceId,
                                              InvoiceFor = DBData.InvoiceFor,
                                              InvoiceNum = DBData.InvoiceNumber.Substring(0, DBData.InvoiceNumber.LastIndexOf("/") + 1),
                                              InvSeq = (DBData.Revision == null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : (DBData.Revision != null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision : (DBData.Revision == null && DBData.InvoiceFor == "INDIA") ? DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') + DBData.Revision,
                                              ClientName = ClientData.ClientName,
                                              InvoiceApprovedBy = DBData.IsApprovedByInvoice,
                                              MainInvoiceNumber = DBData.InvoiceMaster.InvoiceNumber,
                                              MainInvoiceID = (int?)DBData.InvoiceMaster.InvoiceId,
                                              InvoiceAmount = CurrencyData.CurrencyCode + " " + ((from InvData in dbobj.ProformaInvoiceDetailsMasters
                                                                                                  where InvData.ProInvoiceId == Convert.ToInt64(DBData.ProInvoiceId)
                                                                                                  select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.ProformaInvoiceDetailsMasters
                                                                                                                                                          where InvData.ProInvoiceId == Convert.ToInt64(DBData.ProInvoiceId)
                                                                                                                                                          select InvData.TotalAmt.Value).Sum().ToString()),
                                              InvoiceDate = DBData.InvoiceDate,
                                              PaidDate = DBData.PaidDate.Value == DBData.PaidDate.Value == null ? "" : DBData.PaidDate.ToString(),
                                              InvoiceStatus = DBData.InvoiceStatus,
                                              ApprovedBy = DBData.ApprovedBy == null ? "" : (from uData in dbobj.UserMasters
                                                                                             where uData.UserId == DBData.ApprovedBy
                                                                                             select uData.FirstName + " " + uData.LastName).Single()
                                          };
                        if (InvoiceData.Count() > 0)
                        {
                            gvInv.DataSource = InvoiceData.OrderByDescending(x => x.InvoiceID);
                            gvInv.DataBind();
                        }
                        else
                            BlankGrid();
                    }
                    else
                    {
                        var InvoiceData = from DBData in dbobj.ProformaInvoiceMasters
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
                                              InvoiceID = DBData.ProInvoiceId,
                                              InvoiceFor = DBData.InvoiceFor,
                                              InvoiceNum = DBData.InvoiceNumber.Substring(0, DBData.InvoiceNumber.LastIndexOf("/") + 1),
                                              InvSeq = (DBData.Revision == null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : (DBData.Revision != null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision : (DBData.Revision == null && DBData.InvoiceFor == "INDIA") ? DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') + DBData.Revision,
                                              ClientName = ClientData.ClientName,
                                              InvoiceApprovedBy = DBData.IsApprovedByInvoice,
                                              MainInvoiceNumber = DBData.InvoiceMaster.InvoiceNumber,
                                              MainInvoiceID = (int?)DBData.InvoiceMaster.InvoiceId,

                                              InvoiceAmount = CurrencyData.CurrencyCode + " " + ((from InvData in dbobj.ProformaInvoiceDetailsMasters
                                                                                                  where InvData.ProInvoiceId == Convert.ToInt64(DBData.ProInvoiceId)
                                                                                                  select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.ProformaInvoiceDetailsMasters
                                                                                                                                                          where InvData.ProInvoiceId == Convert.ToInt64(DBData.ProInvoiceId)
                                                                                                                                                          select InvData.TotalAmt.Value).Sum().ToString()),
                                              InvoiceDate = DBData.InvoiceDate,
                                              PaidDate = DBData.PaidDate.Value == DBData.PaidDate.Value == null ? "" : DBData.PaidDate.ToString(),
                                              InvoiceStatus = DBData.InvoiceStatus,
                                              ApprovedBy = DBData.ApprovedBy == null ? "" : (from uData in dbobj.UserMasters
                                                                                             where uData.UserId == DBData.ApprovedBy
                                                                                             select uData.FirstName + " " + uData.LastName).Single()
                                          };
                        if (InvoiceData.Count() > 0)
                        {
                            gvInv.DataSource = InvoiceData.OrderByDescending(x => x.InvoiceID);
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
                        var InvoiceData = from DBData in dbobj.ProformaInvoiceMasters
                                          join
                                          ClientData in dbobj.ClientMasters
                                          on
                                          DBData.ClientId equals ClientData.ClientId
                                          join
                                          CurrencyData in dbobj.CurrencyMasters
                                          on
                                          ClientData.CurrencyId equals CurrencyData.CurrencyId
                                          where DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && ClientData.DepartmentId == Convert.ToInt64(drpBusiness.SelectedValue) && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2)

                                          select new
                                          {
                                              InvoiceID = DBData.ProInvoiceId,
                                              InvoiceFor = DBData.InvoiceFor,
                                              InvoiceNum = DBData.InvoiceNumber.Substring(0, DBData.InvoiceNumber.LastIndexOf("/") + 1),

                                              InvSeq = (DBData.Revision == null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : (DBData.Revision != null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision : (DBData.Revision == null && DBData.InvoiceFor == "INDIA") ? DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') + DBData.Revision,
                                              ClientName = ClientData.ClientName,
                                              InvoiceApprovedBy = DBData.IsApprovedByInvoice,
                                              MainInvoiceNumber = DBData.InvoiceMaster.InvoiceNumber,
                                              MainInvoiceID = (int?)DBData.InvoiceMaster.InvoiceId,

                                              InvoiceAmount = CurrencyData.CurrencyCode + " " + ((from InvData in dbobj.ProformaInvoiceDetailsMasters
                                                                                                  where InvData.ProInvoiceId == Convert.ToInt64(DBData.ProInvoiceId)
                                                                                                  select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.ProformaInvoiceDetailsMasters
                                                                                                                                                          where InvData.ProInvoiceId == Convert.ToInt64(DBData.ProInvoiceId)
                                                                                                                                                          select InvData.TotalAmt.Value).Sum().ToString()),
                                              InvoiceDate = DBData.InvoiceDate,
                                              PaidDate = DBData.PaidDate.Value == DBData.PaidDate.Value == null ? "" : DBData.PaidDate.ToString(),
                                              InvoiceStatus = DBData.InvoiceStatus,
                                              ApprovedBy = DBData.ApprovedBy == null ? "" : (from uData in dbobj.UserMasters
                                                                                             where uData.UserId == DBData.ApprovedBy
                                                                                             select uData.FirstName + " " + uData.LastName).Single()
                                          };
                        if (InvoiceData.Count() > 0)
                        {
                            gvInv.DataSource = InvoiceData.OrderByDescending(x => x.InvoiceID);
                            gvInv.DataBind();
                        }
                        else
                            BlankGrid();
                    }
                    else
                    {
                        var InvoiceData = from DBData in dbobj.ProformaInvoiceMasters
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
                                              InvoiceID = DBData.ProInvoiceId,
                                              InvoiceFor = DBData.InvoiceFor,
                                              InvoiceNum = DBData.InvoiceNumber.Substring(0, DBData.InvoiceNumber.LastIndexOf("/") + 1),

                                              InvSeq = (DBData.Revision == null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : (DBData.Revision != null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision : (DBData.Revision == null && DBData.InvoiceFor == "INDIA") ? DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') + DBData.Revision,
                                              ClientName = ClientData.ClientName,
                                              InvoiceApprovedBy = DBData.IsApprovedByInvoice,
                                              MainInvoiceNumber = DBData.InvoiceMaster.InvoiceNumber,
                                              MainInvoiceID = (int?)DBData.InvoiceMaster.InvoiceId,

                                              InvoiceAmount = CurrencyData.CurrencyCode + " " + ((from InvData in dbobj.ProformaInvoiceDetailsMasters
                                                                                                  where InvData.ProInvoiceId == Convert.ToInt64(DBData.ProInvoiceId)
                                                                                                  select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.ProformaInvoiceDetailsMasters
                                                                                                                                                          where InvData.ProInvoiceId == Convert.ToInt64(DBData.ProInvoiceId)
                                                                                                                                                          select InvData.TotalAmt.Value).Sum().ToString()),
                                              InvoiceDate = DBData.InvoiceDate,
                                              PaidDate = DBData.PaidDate.Value == DBData.PaidDate.Value == null ? "" : DBData.PaidDate.ToString(),
                                              InvoiceStatus = DBData.InvoiceStatus,
                                              ApprovedBy = DBData.ApprovedBy == null ? "" : (from uData in dbobj.UserMasters
                                                                                             where uData.UserId == DBData.ApprovedBy
                                                                                             select uData.FirstName + " " + uData.LastName).Single()
                                          };
                        if (InvoiceData.Count() > 0)
                        {
                            gvInv.DataSource = InvoiceData.OrderByDescending(x => x.InvoiceID);
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
                    var InvoiceData = from DBData in dbobj.ProformaInvoiceMasters
                                      join
                                      ClientData in dbobj.ClientMasters
                                      on
                                      DBData.ClientId equals ClientData.ClientId
                                      join
                                      CurrencyData in dbobj.CurrencyMasters
                                      on
                                      ClientData.CurrencyId equals CurrencyData.CurrencyId
                                      where ClientData.DepartmentId == Convert.ToInt64(drpBusiness.SelectedValue) && DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2) && DBData.IsDeleted == false
                                      select new
                                      {
                                          InvoiceID = DBData.ProInvoiceId,
                                          InvoiceFor = DBData.InvoiceFor,
                                          InvoiceNum = DBData.InvoiceNumber.Substring(0, DBData.InvoiceNumber.LastIndexOf("/") + 1),

                                          InvSeq = (DBData.Revision == null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : (DBData.Revision != null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision : (DBData.Revision == null && DBData.InvoiceFor == "INDIA") ? DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') + DBData.Revision,

                                          ClientName = ClientData.ClientName,
                                          InvoiceApprovedBy = DBData.IsApprovedByInvoice,
                                          MainInvoiceNumber = DBData.InvoiceMaster.InvoiceNumber,
                                          MainInvoiceID = (int?)DBData.InvoiceMaster.InvoiceId,

                                          InvoiceAmount = CurrencyData.CurrencyCode + " " + ((from InvData in dbobj.ProformaInvoiceDetailsMasters
                                                                                              where InvData.ProInvoiceId == Convert.ToInt64(DBData.ProInvoiceId)
                                                                                              select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.ProformaInvoiceDetailsMasters
                                                                                                                                                      where InvData.ProInvoiceId == Convert.ToInt64(DBData.ProInvoiceId)
                                                                                                                                                      select InvData.TotalAmt.Value).Sum().ToString()),
                                          InvoiceDate = DBData.InvoiceDate,
                                          PaidDate = DBData.PaidDate.Value == DBData.PaidDate.Value == null ? "" : DBData.PaidDate.ToString(),
                                          InvoiceStatus = DBData.InvoiceStatus,
                                          ApprovedBy = DBData.ApprovedBy == null ? "" : (from uData in dbobj.UserMasters
                                                                                         where uData.UserId == DBData.ApprovedBy
                                                                                         select uData.FirstName + " " + uData.LastName).Single()

                                      };
                    if (InvoiceData.Count() > 0)
                    {
                        gvInv.DataSource = InvoiceData.OrderByDescending(x => x.InvoiceID);
                        gvInv.DataBind();
                    }
                    else
                        BlankGrid();
                }
                else
                {
                    var InvoiceData = from DBData in dbobj.ProformaInvoiceMasters
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
                                          InvoiceID = DBData.ProInvoiceId,
                                          InvoiceFor = DBData.InvoiceFor,
                                          InvoiceNum = DBData.InvoiceNumber.Substring(0, DBData.InvoiceNumber.LastIndexOf("/") + 1),

                                          InvSeq = (DBData.Revision == null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : (DBData.Revision != null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision : (DBData.Revision == null && DBData.InvoiceFor == "INDIA") ? DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') + DBData.Revision,
                                          ClientName = ClientData.ClientName,
                                          InvoiceApprovedBy = DBData.IsApprovedByInvoice,
                                          MainInvoiceNumber = DBData.InvoiceMaster.InvoiceNumber,
                                          MainInvoiceID = (int?)DBData.InvoiceMaster.InvoiceId,

                                          InvoiceAmount = CurrencyData.CurrencyCode + " " + ((from InvData in dbobj.ProformaInvoiceDetailsMasters
                                                                                              where InvData.ProInvoiceId == Convert.ToInt64(DBData.ProInvoiceId)
                                                                                              select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.ProformaInvoiceDetailsMasters
                                                                                                                                                      where InvData.ProInvoiceId == Convert.ToInt64(DBData.ProInvoiceId)
                                                                                                                                                      select InvData.TotalAmt.Value).Sum().ToString()),
                                          InvoiceDate = DBData.InvoiceDate,
                                          PaidDate = DBData.PaidDate.Value == DBData.PaidDate.Value == null ? "" : DBData.PaidDate.ToString(),
                                          InvoiceStatus = DBData.InvoiceStatus,
                                          ApprovedBy = DBData.ApprovedBy == null ? "" : (from uData in dbobj.UserMasters
                                                                                         where uData.UserId == DBData.ApprovedBy
                                                                                         select uData.FirstName + " " + uData.LastName).Single()
                                      };
                    if (InvoiceData.Count() > 0)
                    {
                        gvInv.DataSource = InvoiceData.OrderByDescending(x => x.InvoiceID);
                        gvInv.DataBind();
                    }
                    else
                        BlankGrid();
                }
            }
        }
        else
        {
            try
            {
                int Seq = Convert.ToInt32(txtMasterSearch.Text.Trim());
                if (Global.UserType == "DIRECTOR" || Global.DepartmentName == "ACCT")
                {
                    if (drpBusiness.SelectedIndex == 0)
                    {
                        if (ddlClient.SelectedIndex == 0)
                        {
                            var InvoiceData = from DBData in dbobj.ProformaInvoiceMasters
                                              join ClientData in dbobj.ClientMasters
                                              on DBData.ClientId equals ClientData.ClientId
                                              join CurrencyData in dbobj.CurrencyMasters
                                              on ClientData.CurrencyId equals CurrencyData.CurrencyId
                                              where DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && DBData.InvoiceSeqNo == Seq && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2)
                                              select new
                                              {
                                                  InvoiceID = DBData.ProInvoiceId,
                                                  InvoiceFor = DBData.InvoiceFor,
                                                  InvoiceNum = DBData.InvoiceNumber.Substring(0, DBData.InvoiceNumber.LastIndexOf("/") + 1),

                                                  InvSeq = (DBData.Revision == null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : (DBData.Revision != null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision : (DBData.Revision == null && DBData.InvoiceFor == "INDIA") ? DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') + DBData.Revision,
                                                  ClientName = ClientData.ClientName,
                                                  InvoiceApprovedBy = DBData.IsApprovedByInvoice,
                                                  MainInvoiceNumber = DBData.InvoiceMaster.InvoiceNumber,
                                                  MainInvoiceID = (int?)DBData.InvoiceMaster.InvoiceId,

                                                  InvoiceAmount = CurrencyData.CurrencyCode + " " + ((from InvData in dbobj.ProformaInvoiceDetailsMasters
                                                                                                      where InvData.ProInvoiceId == Convert.ToInt64(DBData.ProInvoiceId)
                                                                                                      select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.ProformaInvoiceDetailsMasters
                                                                                                                                                              where InvData.ProInvoiceId == Convert.ToInt64(DBData.ProInvoiceId)
                                                                                                                                                              select InvData.TotalAmt.Value).Sum().ToString()),
                                                  InvoiceDate = DBData.InvoiceDate,
                                                  PaidDate = DBData.PaidDate.Value == DBData.PaidDate.Value == null ? "" : DBData.PaidDate.ToString(),
                                                  InvoiceStatus = DBData.InvoiceStatus,
                                                  ApprovedBy = DBData.ApprovedBy == null ? "" : (from uData in dbobj.UserMasters
                                                                                                 where uData.UserId == DBData.ApprovedBy
                                                                                                 select uData.FirstName + " " + uData.LastName).Single()
                                              };
                            if (InvoiceData.Count() > 0)
                            {
                                gvInv.DataSource = InvoiceData.OrderByDescending(x => x.InvoiceID);
                                gvInv.DataBind();
                            }
                            else
                                BlankGrid();
                        }
                        else
                        {

                            var InvoiceData = from DBData in dbobj.ProformaInvoiceMasters
                                              join ClientData in dbobj.ClientMasters
                                              on DBData.ClientId equals ClientData.ClientId
                                              join CurrencyData in dbobj.CurrencyMasters
                                              on ClientData.CurrencyId equals CurrencyData.CurrencyId
                                              where DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && ClientData.ClientId == Convert.ToInt64(ddlClient.SelectedValue) && DBData.InvoiceSeqNo == Seq && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2)
                                              select new
                                              {
                                                  InvoiceID = DBData.ProInvoiceId,
                                                  InvoiceFor = DBData.InvoiceFor,
                                                  InvoiceNum = DBData.InvoiceNumber.Substring(0, DBData.InvoiceNumber.LastIndexOf("/") + 1),

                                                  InvSeq = (DBData.Revision == null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : (DBData.Revision != null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision : (DBData.Revision == null && DBData.InvoiceFor == "INDIA") ? DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') + DBData.Revision,
                                                  ClientName = ClientData.ClientName,
                                                  InvoiceApprovedBy = DBData.IsApprovedByInvoice,
                                                  MainInvoiceNumber = DBData.InvoiceMaster.InvoiceNumber,
                                                  MainInvoiceID = (int?)DBData.InvoiceMaster.InvoiceId,

                                                  InvoiceAmount = CurrencyData.CurrencyCode + " " + ((from InvData in dbobj.ProformaInvoiceDetailsMasters
                                                                                                      where InvData.ProInvoiceId == Convert.ToInt64(DBData.ProInvoiceId)
                                                                                                      select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.ProformaInvoiceDetailsMasters
                                                                                                                                                              where InvData.ProInvoiceId == Convert.ToInt64(DBData.ProInvoiceId)
                                                                                                                                                              select InvData.TotalAmt.Value).Sum().ToString()),
                                                  InvoiceDate = DBData.InvoiceDate,
                                                  PaidDate = DBData.PaidDate.Value == DBData.PaidDate.Value == null ? "" : DBData.PaidDate.ToString(),
                                                  InvoiceStatus = DBData.InvoiceStatus,
                                                  ApprovedBy = DBData.ApprovedBy == null ? "" : (from uData in dbobj.UserMasters
                                                                                                 where uData.UserId == DBData.ApprovedBy
                                                                                                 select uData.FirstName + " " + uData.LastName).Single()
                                              };
                            if (InvoiceData.Count() > 0)
                            {
                                gvInv.DataSource = InvoiceData.OrderByDescending(x => x.InvoiceID);
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

                            var InvoiceData = from DBData in dbobj.ProformaInvoiceMasters
                                              join ClientData in dbobj.ClientMasters
                                              on DBData.ClientId equals ClientData.ClientId
                                              join CurrencyData in dbobj.CurrencyMasters
                                              on ClientData.CurrencyId equals CurrencyData.CurrencyId
                                              where DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && ClientData.DepartmentId == Convert.ToInt64(drpBusiness.SelectedValue) && DBData.InvoiceSeqNo == Seq && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2)
                                              select new
                                              {
                                                  InvoiceID = DBData.ProInvoiceId,
                                                  InvoiceFor = DBData.InvoiceFor,
                                                  InvoiceNum = DBData.InvoiceNumber.Substring(0, DBData.InvoiceNumber.LastIndexOf("/") + 1),

                                                  InvSeq = (DBData.Revision == null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : (DBData.Revision != null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision : (DBData.Revision == null && DBData.InvoiceFor == "INDIA") ? DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') + DBData.Revision,
                                                  ClientName = ClientData.ClientName,
                                                  InvoiceApprovedBy = DBData.IsApprovedByInvoice,
                                                  MainInvoiceNumber = DBData.InvoiceMaster.InvoiceNumber,
                                                  MainInvoiceID = (int?)DBData.InvoiceMaster.InvoiceId,

                                                  InvoiceAmount = CurrencyData.CurrencyCode + " " + ((from InvData in dbobj.ProformaInvoiceDetailsMasters
                                                                                                      where InvData.ProInvoiceId == Convert.ToInt64(DBData.ProInvoiceId)
                                                                                                      select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.ProformaInvoiceDetailsMasters
                                                                                                                                                              where InvData.ProInvoiceId == Convert.ToInt64(DBData.ProInvoiceId)
                                                                                                                                                              select InvData.TotalAmt.Value).Sum().ToString()),
                                                  InvoiceDate = DBData.InvoiceDate,
                                                  PaidDate = DBData.PaidDate.Value == DBData.PaidDate.Value == null ? "" : DBData.PaidDate.ToString(),
                                                  InvoiceStatus = DBData.InvoiceStatus,
                                                  ApprovedBy = DBData.ApprovedBy == null ? "" : (from uData in dbobj.UserMasters
                                                                                                 where uData.UserId == DBData.ApprovedBy
                                                                                                 select uData.FirstName + " " + uData.LastName).Single()
                                              };
                            if (InvoiceData.Count() > 0)
                            {
                                gvInv.DataSource = InvoiceData.OrderByDescending(x => x.InvoiceID);
                                gvInv.DataBind();
                            }
                            else
                                BlankGrid();
                        }
                        else
                        {
                            var InvoiceData = from DBData in dbobj.ProformaInvoiceMasters
                                              join ClientData in dbobj.ClientMasters
                                              on DBData.ClientId equals ClientData.ClientId
                                              join CurrencyData in dbobj.CurrencyMasters
                                              on ClientData.CurrencyId equals CurrencyData.CurrencyId
                                              where DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && ClientData.ClientId == Convert.ToInt64(ddlClient.SelectedValue) && DBData.InvoiceSeqNo == Seq
                                              && ClientData.DepartmentId == Convert.ToInt64(drpBusiness.SelectedValue) && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2)
                                              select new
                                              {
                                                  InvoiceID = DBData.ProInvoiceId,
                                                  InvoiceFor = DBData.InvoiceFor,
                                                  InvoiceNum = DBData.InvoiceNumber.Substring(0, DBData.InvoiceNumber.LastIndexOf("/") + 1),

                                                  InvSeq = (DBData.Revision == null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : (DBData.Revision != null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision : (DBData.Revision == null && DBData.InvoiceFor == "INDIA") ? DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') + DBData.Revision,
                                                  ClientName = ClientData.ClientName,
                                                  InvoiceApprovedBy = DBData.IsApprovedByInvoice,
                                                  MainInvoiceNumber = DBData.InvoiceMaster.InvoiceNumber,
                                                  MainInvoiceID = (int?)DBData.InvoiceMaster.InvoiceId,

                                                  InvoiceAmount = CurrencyData.CurrencyCode + " " + ((from InvData in dbobj.ProformaInvoiceDetailsMasters
                                                                                                      where InvData.ProInvoiceId == Convert.ToInt64(DBData.ProInvoiceId)
                                                                                                      select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.ProformaInvoiceDetailsMasters
                                                                                                                                                              where InvData.ProInvoiceId == Convert.ToInt64(DBData.ProInvoiceId)
                                                                                                                                                              select InvData.TotalAmt.Value).Sum().ToString()),
                                                  InvoiceDate = DBData.InvoiceDate,
                                                  PaidDate = DBData.PaidDate.Value == DBData.PaidDate.Value == null ? "" : DBData.PaidDate.ToString(),
                                                  InvoiceStatus = DBData.InvoiceStatus,
                                                  ApprovedBy = DBData.ApprovedBy == null ? "" : (from uData in dbobj.UserMasters
                                                                                                 where uData.UserId == DBData.ApprovedBy
                                                                                                 select uData.FirstName + " " + uData.LastName).Single()
                                              };
                            if (InvoiceData.Count() > 0)
                            {
                                gvInv.DataSource = InvoiceData.OrderByDescending(x => x.InvoiceID);
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
                        var InvoiceData = from DBData in dbobj.ProformaInvoiceMasters
                                          join ClientData in dbobj.ClientMasters
                                          on DBData.ClientId equals ClientData.ClientId
                                          join CurrencyData in dbobj.CurrencyMasters
                                          on ClientData.CurrencyId equals CurrencyData.CurrencyId
                                          where ClientData.DepartmentId == Convert.ToInt64(drpBusiness.SelectedValue) && DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && DBData.InvoiceSeqNo == Seq && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2)
                                          select new
                                          {
                                              InvoiceID = DBData.ProInvoiceId,
                                              InvoiceFor = DBData.InvoiceFor,
                                              InvoiceNum = DBData.InvoiceNumber.Substring(0, DBData.InvoiceNumber.LastIndexOf("/") + 1),

                                              InvSeq = (DBData.Revision == null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : (DBData.Revision != null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision : (DBData.Revision == null && DBData.InvoiceFor == "INDIA") ? DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') + DBData.Revision,
                                              ClientName = ClientData.ClientName,
                                              InvoiceApprovedBy = DBData.IsApprovedByInvoice,
                                              MainInvoiceNumber = DBData.InvoiceMaster.InvoiceNumber,
                                              MainInvoiceID = (int?)DBData.InvoiceMaster.InvoiceId,

                                              InvoiceAmount = CurrencyData.CurrencyCode + " " + ((from InvData in dbobj.ProformaInvoiceDetailsMasters
                                                                                                  where InvData.ProInvoiceId == Convert.ToInt64(DBData.ProInvoiceId)
                                                                                                  select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.ProformaInvoiceDetailsMasters
                                                                                                                                                          where InvData.ProInvoiceId == Convert.ToInt64(DBData.ProInvoiceId)
                                                                                                                                                          select InvData.TotalAmt.Value).Sum().ToString()),
                                              InvoiceDate = DBData.InvoiceDate,
                                              PaidDate = DBData.PaidDate.Value == DBData.PaidDate.Value == null ? "" : DBData.PaidDate.ToString(),
                                              InvoiceStatus = DBData.InvoiceStatus,
                                              ApprovedBy = DBData.ApprovedBy == null ? "" : (from uData in dbobj.UserMasters
                                                                                             where uData.UserId == DBData.ApprovedBy
                                                                                             select uData.FirstName + " " + uData.LastName).Single()
                                          };
                        if (InvoiceData.Count() > 0)
                        {
                            gvInv.DataSource = InvoiceData.OrderByDescending(x => x.InvoiceID);
                            gvInv.DataBind();
                        }
                        else
                            BlankGrid();
                    }
                    else
                    {
                        var InvoiceData = from DBData in dbobj.ProformaInvoiceMasters
                                          join ClientData in dbobj.ClientMasters
                                          on DBData.ClientId equals ClientData.ClientId
                                          join CurrencyData in dbobj.CurrencyMasters
                                          on ClientData.CurrencyId equals CurrencyData.CurrencyId
                                          where ClientData.DepartmentId == Convert.ToInt64(drpBusiness.SelectedValue) && DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && DBData.InvoiceSeqNo == Seq
                                                && ClientData.ClientId == Convert.ToInt64(ddlClient.SelectedValue) && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2)
                                          select new
                                          {
                                              InvoiceID = DBData.ProInvoiceId,
                                              InvoiceFor = DBData.InvoiceFor,
                                              InvoiceNum = DBData.InvoiceNumber.Substring(0, DBData.InvoiceNumber.LastIndexOf("/") + 1),

                                              InvSeq = (DBData.Revision == null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : (DBData.Revision != null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision : (DBData.Revision == null && DBData.InvoiceFor == "INDIA") ? DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') + DBData.Revision,
                                              ClientName = ClientData.ClientName,
                                              InvoiceApprovedBy = DBData.IsApprovedByInvoice,
                                              MainInvoiceNumber = DBData.InvoiceMaster.InvoiceNumber,
                                              MainInvoiceID = (int?)DBData.InvoiceMaster.InvoiceId,

                                              InvoiceAmount = CurrencyData.CurrencyCode + " " + ((from InvData in dbobj.ProformaInvoiceDetailsMasters
                                                                                                  where InvData.ProInvoiceId == Convert.ToInt64(DBData.ProInvoiceId)
                                                                                                  select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.ProformaInvoiceDetailsMasters
                                                                                                                                                          where InvData.ProInvoiceId == Convert.ToInt64(DBData.ProInvoiceId)
                                                                                                                                                          select InvData.TotalAmt.Value).Sum().ToString()),
                                              InvoiceDate = DBData.InvoiceDate,
                                              PaidDate = DBData.PaidDate.Value == DBData.PaidDate.Value == null ? "" : DBData.PaidDate.ToString(),
                                              InvoiceStatus = DBData.InvoiceStatus,
                                              ApprovedBy = DBData.ApprovedBy == null ? "" : (from uData in dbobj.UserMasters
                                                                                             where uData.UserId == DBData.ApprovedBy
                                                                                             select uData.FirstName + " " + uData.LastName).Single()
                                          };
                        if (InvoiceData.Count() > 0)
                        {
                            gvInv.DataSource = InvoiceData.OrderByDescending(x => x.InvoiceID);
                            gvInv.DataBind();
                        }
                        else
                            BlankGrid();
                    }
                }
            }
            catch
            {
                /*
                if (Global.UserType == "DIRECTOR" || Global.DepartmentName == "ACCT")
                {
                    if (drpBusiness.SelectedIndex == 0)
                    {
                        if (ddlClient.SelectedIndex == 0)
                        {
                            var InvoiceData = from DBData in dbobj.ProformaInvoiceMasters
                                              join ClientData in dbobj.ClientMasters
                                              on DBData.ClientId equals ClientData.ClientId
                                              join CurrencyData in dbobj.CurrencyMasters
                                              on ClientData.CurrencyId equals CurrencyData.CurrencyId
                                              where DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && DBData.ClientMaster.ClientName.Contains(txtMasterSearch.Text.Trim()) && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2)
                                              select new
                                              {
                                                  InvoiceID = DBData.ProInvoiceId,
                                                  InvoiceFor = DBData.InvoiceFor,
                                                  InvoiceNum = DBData.InvoiceNumber.Substring(0, DBData.InvoiceNumber.LastIndexOf("/") + 1),

                                                  InvSeq = (DBData.Revision == null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : (DBData.Revision != null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision : (DBData.Revision == null && DBData.InvoiceFor == "INDIA") ? DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') + DBData.Revision,
                                                  ClientName = ClientData.ClientName,
                                                  InvoiceApprovedBy = DBData.IsApprovedByInvoice,
                                                  MainInvoiceNumber = DBData.InvoiceMaster.InvoiceNumber,
                                                  MainInvoiceID = (int?)DBData.InvoiceMaster.InvoiceId,

                                                  InvoiceAmount = CurrencyData.CurrencyCode + " " + ((from InvData in dbobj.ProformaInvoiceDetailsMasters
                                                                                                      where InvData.ProInvoiceId == Convert.ToInt64(DBData.ProInvoiceId)
                                                                                                      select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.ProformaInvoiceDetailsMasters
                                                                                                                                                              where InvData.ProInvoiceId == Convert.ToInt64(DBData.ProInvoiceId)
                                                                                                                                                              select InvData.TotalAmt.Value).Sum().ToString()),
                                                  InvoiceDate = DBData.InvoiceDate,
                                                  PaidDate = DBData.PaidDate.Value == DBData.PaidDate.Value == null ? "" : DBData.PaidDate.ToString(),
                                                  InvoiceStatus = DBData.InvoiceStatus,
                                                  ApprovedBy = DBData.ApprovedBy == null ? "" : (from uData in dbobj.UserMasters
                                                                                                 where uData.UserId == DBData.ApprovedBy
                                                                                                 select uData.FirstName + " " + uData.LastName).Single()
                                              };
                            if (InvoiceData.Count() > 0)
                            {
                                gvInv.DataSource = InvoiceData.OrderByDescending(x => x.InvoiceID);
                                gvInv.DataBind();
                            }
                            else
                                BlankGrid();
                        }
                        else
                        {
                            var InvoiceData = from DBData in dbobj.ProformaInvoiceMasters
                                              join ClientData in dbobj.ClientMasters
                                              on DBData.ClientId equals ClientData.ClientId
                                              join CurrencyData in dbobj.CurrencyMasters
                                              on ClientData.CurrencyId equals CurrencyData.CurrencyId
                                              where DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && ClientData.ClientId == Convert.ToInt64(ddlClient.SelectedValue) && DBData.ClientMaster.ClientName.Contains(txtMasterSearch.Text.Trim()) && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2)
                                              select new
                                              {
                                                  InvoiceID = DBData.ProInvoiceId,
                                                  InvoiceFor = DBData.InvoiceFor,
                                                  InvoiceNum = DBData.InvoiceNumber.Substring(0, DBData.InvoiceNumber.LastIndexOf("/") + 1),

                                                  InvSeq = (DBData.Revision == null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : (DBData.Revision != null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision : (DBData.Revision == null && DBData.InvoiceFor == "INDIA") ? DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') + DBData.Revision,
                                                  ClientName = ClientData.ClientName,
                                                  InvoiceApprovedBy = DBData.IsApprovedByInvoice,
                                                  MainInvoiceNumber = DBData.InvoiceMaster.InvoiceNumber,
                                                  MainInvoiceID = (int?)DBData.InvoiceMaster.InvoiceId,

                                                  InvoiceAmount = CurrencyData.CurrencyCode + " " + ((from InvData in dbobj.ProformaInvoiceDetailsMasters
                                                                                                      where InvData.ProInvoiceId == Convert.ToInt64(DBData.ProInvoiceId)
                                                                                                      select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.ProformaInvoiceDetailsMasters
                                                                                                                                                              where InvData.ProInvoiceId == Convert.ToInt64(DBData.ProInvoiceId)
                                                                                                                                                              select InvData.TotalAmt.Value).Sum().ToString()),
                                                  InvoiceDate = DBData.InvoiceDate,
                                                  PaidDate = DBData.PaidDate.Value == DBData.PaidDate.Value == null ? "" : DBData.PaidDate.ToString(),
                                                  InvoiceStatus = DBData.InvoiceStatus,
                                                  ApprovedBy = DBData.ApprovedBy == null ? "" : (from uData in dbobj.UserMasters
                                                                                                 where uData.UserId == DBData.ApprovedBy
                                                                                                 select uData.FirstName + " " + uData.LastName).Single()
                                              };
                            if (InvoiceData.Count() > 0)
                            {
                                gvInv.DataSource = InvoiceData.OrderByDescending(x => x.InvoiceID);
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
                            var InvoiceData = from DBData in dbobj.ProformaInvoiceMasters
                                              join ClientData in dbobj.ClientMasters
                                              on DBData.ClientId equals ClientData.ClientId
                                              join CurrencyData in dbobj.CurrencyMasters
                                              on ClientData.CurrencyId equals CurrencyData.CurrencyId
                                              where DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && ClientData.DepartmentId == Convert.ToInt64(drpBusiness.SelectedValue) && DBData.ClientMaster.ClientName.Contains(txtMasterSearch.Text.Trim()) && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2)
                                              select new
                                              {
                                                  InvoiceID = DBData.ProInvoiceId,
                                                  InvoiceFor = DBData.InvoiceFor,
                                                  InvoiceNum = DBData.InvoiceNumber.Substring(0, DBData.InvoiceNumber.LastIndexOf("/") + 1),

                                                  InvSeq = (DBData.Revision == null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : (DBData.Revision != null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision : (DBData.Revision == null && DBData.InvoiceFor == "INDIA") ? DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') + DBData.Revision,
                                                  ClientName = ClientData.ClientName,
                                                  InvoiceApprovedBy = DBData.IsApprovedByInvoice,
                                                  MainInvoiceNumber = DBData.InvoiceMaster.InvoiceNumber,
                                                  MainInvoiceID = (int?)DBData.InvoiceMaster.InvoiceId,

                                                  InvoiceAmount = CurrencyData.CurrencyCode + " " + ((from InvData in dbobj.ProformaInvoiceDetailsMasters
                                                                                                      where InvData.ProInvoiceId == Convert.ToInt64(DBData.ProInvoiceId)
                                                                                                      select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.ProformaInvoiceDetailsMasters
                                                                                                                                                              where InvData.ProInvoiceId == Convert.ToInt64(DBData.ProInvoiceId)
                                                                                                                                                              select InvData.TotalAmt.Value).Sum().ToString()),
                                                  InvoiceDate = DBData.InvoiceDate,
                                                  PaidDate = DBData.PaidDate.Value == DBData.PaidDate.Value == null ? "" : DBData.PaidDate.ToString(),
                                                  InvoiceStatus = DBData.InvoiceStatus,
                                                  ApprovedBy = DBData.ApprovedBy == null ? "" : (from uData in dbobj.UserMasters
                                                                                                 where uData.UserId == DBData.ApprovedBy
                                                                                                 select uData.FirstName + " " + uData.LastName).Single()
                                              };
                            if (InvoiceData.Count() > 0)
                            {
                                gvInv.DataSource = InvoiceData.OrderByDescending(x => x.InvoiceID);
                                gvInv.DataBind();
                            }
                            else
                                BlankGrid();
                        }
                        else
                        {
                            var InvoiceData = from DBData in dbobj.ProformaInvoiceMasters
                                              join ClientData in dbobj.ClientMasters
                                              on DBData.ClientId equals ClientData.ClientId
                                              join CurrencyData in dbobj.CurrencyMasters
                                              on ClientData.CurrencyId equals CurrencyData.CurrencyId
                                              where DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && ClientData.ClientId == Convert.ToInt64(ddlClient.SelectedValue) && DBData.ClientMaster.ClientName.Contains(txtMasterSearch.Text.Trim())
                                              && ClientData.DepartmentId == Convert.ToInt64(drpBusiness.SelectedValue) && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2)
                                              select new
                                              {
                                                  InvoiceID = DBData.ProInvoiceId,
                                                  InvoiceFor = DBData.InvoiceFor,
                                                  InvoiceNum = DBData.InvoiceNumber.Substring(0, DBData.InvoiceNumber.LastIndexOf("/") + 1),

                                                  InvSeq = (DBData.Revision == null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : (DBData.Revision != null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision : (DBData.Revision == null && DBData.InvoiceFor == "INDIA") ? DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') + DBData.Revision,
                                                  ClientName = ClientData.ClientName,
                                                  InvoiceApprovedBy = DBData.IsApprovedByInvoice,
                                                  MainInvoiceNumber = DBData.InvoiceMaster.InvoiceNumber,
                                                  MainInvoiceID = (int?)DBData.InvoiceMaster.InvoiceId,

                                                  InvoiceAmount = CurrencyData.CurrencyCode + " " + ((from InvData in dbobj.ProformaInvoiceDetailsMasters
                                                                                                      where InvData.ProInvoiceId == Convert.ToInt64(DBData.ProInvoiceId)
                                                                                                      select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.ProformaInvoiceDetailsMasters
                                                                                                                                                              where InvData.ProInvoiceId == Convert.ToInt64(DBData.ProInvoiceId)
                                                                                                                                                              select InvData.TotalAmt.Value).Sum().ToString()),
                                                  InvoiceDate = DBData.InvoiceDate,
                                                  PaidDate = DBData.PaidDate.Value == DBData.PaidDate.Value == null ? "" : DBData.PaidDate.ToString(),
                                                  InvoiceStatus = DBData.InvoiceStatus,
                                                  ApprovedBy = DBData.ApprovedBy == null ? "" : (from uData in dbobj.UserMasters
                                                                                                 where uData.UserId == DBData.ApprovedBy
                                                                                                 select uData.FirstName + " " + uData.LastName).Single()
                                              };
                            if (InvoiceData.Count() > 0)
                            {
                                gvInv.DataSource = InvoiceData.OrderByDescending(x => x.InvoiceID);
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
                        var InvoiceData = from DBData in dbobj.ProformaInvoiceMasters
                                          join ClientData in dbobj.ClientMasters
                                          on DBData.ClientId equals ClientData.ClientId
                                          join CurrencyData in dbobj.CurrencyMasters
                                          on ClientData.CurrencyId equals CurrencyData.CurrencyId
                                          where ClientData.DepartmentId == Convert.ToInt64(drpBusiness.SelectedValue) && DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && DBData.ClientMaster.ClientName.Contains(txtMasterSearch.Text.Trim()) && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2)
                                          select new
                                          {
                                              InvoiceID = DBData.ProInvoiceId,
                                              InvoiceFor = DBData.InvoiceFor,
                                              InvoiceNum = DBData.InvoiceNumber.Substring(0, DBData.InvoiceNumber.LastIndexOf("/") + 1),

                                              InvSeq = (DBData.Revision == null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : (DBData.Revision != null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision : (DBData.Revision == null && DBData.InvoiceFor == "INDIA") ? DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') + DBData.Revision,
                                              ClientName = ClientData.ClientName,
                                              InvoiceApprovedBy = DBData.IsApprovedByInvoice,
                                              MainInvoiceNumber = DBData.InvoiceMaster.InvoiceNumber,
                                              MainInvoiceID = (int?)DBData.InvoiceMaster.InvoiceId,

                                              InvoiceAmount = CurrencyData.CurrencyCode + " " + ((from InvData in dbobj.ProformaInvoiceDetailsMasters
                                                                                                  where InvData.ProInvoiceId == Convert.ToInt64(DBData.ProInvoiceId)
                                                                                                  select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.ProformaInvoiceDetailsMasters
                                                                                                                                                          where InvData.ProInvoiceId == Convert.ToInt64(DBData.ProInvoiceId)
                                                                                                                                                          select InvData.TotalAmt.Value).Sum().ToString()),
                                              InvoiceDate = DBData.InvoiceDate,
                                              PaidDate = DBData.PaidDate.Value == DBData.PaidDate.Value == null ? "" : DBData.PaidDate.ToString(),
                                              InvoiceStatus = DBData.InvoiceStatus,
                                              ApprovedBy = DBData.ApprovedBy == null ? "" : (from uData in dbobj.UserMasters
                                                                                             where uData.UserId == DBData.ApprovedBy
                                                                                             select uData.FirstName + " " + uData.LastName).Single()
                                          };
                        if (InvoiceData.Count() > 0)
                        {
                            gvInv.DataSource = InvoiceData.OrderByDescending(x => x.InvoiceID);
                            gvInv.DataBind();
                        }
                        else
                            BlankGrid();
                    }
                    else
                    {

                        var InvoiceData = from DBData in dbobj.ProformaInvoiceMasters
                                          join ClientData in dbobj.ClientMasters
                                          on DBData.ClientId equals ClientData.ClientId
                                          join CurrencyData in dbobj.CurrencyMasters
                                          on ClientData.CurrencyId equals CurrencyData.CurrencyId
                                          where ClientData.DepartmentId == Convert.ToInt64(drpBusiness.SelectedValue) && DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && DBData.ClientMaster.ClientName.Contains(txtMasterSearch.Text.Trim())
                                                && ClientData.ClientId == Convert.ToInt64(ddlClient.SelectedValue) && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2)
                                          select new
                                          {
                                              InvoiceID = DBData.ProInvoiceId,
                                              InvoiceFor = DBData.InvoiceFor,
                                              InvoiceNum = DBData.InvoiceNumber.Substring(0, DBData.InvoiceNumber.LastIndexOf("/") + 1),

                                              InvSeq = (DBData.Revision == null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : (DBData.Revision != null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision : (DBData.Revision == null && DBData.InvoiceFor == "INDIA") ? DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') + DBData.Revision,
                                              ClientName = ClientData.ClientName,
                                              InvoiceApprovedBy = DBData.IsApprovedByInvoice,
                                              MainInvoiceNumber = DBData.InvoiceMaster.InvoiceNumber,
                                              MainInvoiceID = (int?)DBData.InvoiceMaster.InvoiceId,

                                              InvoiceAmount = CurrencyData.CurrencyCode + " " + ((from InvData in dbobj.ProformaInvoiceDetailsMasters
                                                                                                  where InvData.ProInvoiceId == Convert.ToInt64(DBData.ProInvoiceId)
                                                                                                  select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.ProformaInvoiceDetailsMasters
                                                                                                                                                          where InvData.ProInvoiceId == Convert.ToInt64(DBData.ProInvoiceId)
                                                                                                                                                          select InvData.TotalAmt.Value).Sum().ToString()),
                                              InvoiceDate = DBData.InvoiceDate,
                                              PaidDate = DBData.PaidDate.Value == DBData.PaidDate.Value == null ? "" : DBData.PaidDate.ToString(),
                                              InvoiceStatus = DBData.InvoiceStatus,
                                              ApprovedBy = DBData.ApprovedBy == null ? "" : (from uData in dbobj.UserMasters
                                                                                             where uData.UserId == DBData.ApprovedBy
                                                                                             select uData.FirstName + " " + uData.LastName).Single()
                                          };
                        if (InvoiceData.Count() > 0)
                        {
                            gvInv.DataSource = InvoiceData.OrderByDescending(x => x.InvoiceID);
                            gvInv.DataBind();
                        }
                        else
                            BlankGrid();
                    }
                }
                */
            }
        }
    }

    public string HighlightText(string InputTxt)
    {
        TextBox txt = (TextBox)this.Master.FindControl("txtMasterSearch");
        string Search_Str = txt.Text;

        Regex RegExp = new Regex(Search_Str.Replace(" ", "|").Trim(), RegexOptions.IgnoreCase);

        return RegExp.Replace(InputTxt, new MatchEvaluator(ReplaceKeyWords));
    }

    public string ReplaceKeyWords(Match m)
    {
        return ("<span class=highlight>" + m.Value + "</span>");
    }

    protected void gvInv_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ddlClient.SelectedIndex != 0 && drpBusiness.SelectedIndex != 0)
            {
                var InvoiceData = (from DBData in dbobj.ProformaInvoiceMasters
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
                                       DBData.ProInvoiceId
                                   }).ToArray();
                if (ViewState["SendI"] != null)
                {
                    string[] f = ViewState["SendI"].ToString().Split(',');
                    CheckBox chkHdrALL = (CheckBox)e.Row.FindControl("chkSelectAll");
                    for (int h = 0; h < InvoiceData.Count(); h++)
                    {

                        if (InvoiceData[h].ProInvoiceId.ToString() == f[h])
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
            ImageButton imgbtnApprove = (ImageButton)e.Row.FindControl("imgbtnApprove");
            ImageButton imgbtnUnlock = (ImageButton)e.Row.FindControl("imgbtnUnlock");
            Label lblIsApprove = (Label)e.Row.FindControl("lblApprovedBy");
            if (Global.UserType == "DIRECTOR" || Global.DepartmentName == "ACCT")
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

            hypreInv.NavigateUrl = "ProformaInvoice?InvID=" + Global.Encrypt(lblInvID.Text);

            // Main Invoice Table Link
            HyperLink hyperlnkMainInvoiceNo = (HyperLink)e.Row.FindControl("hyperlnkMainInvoiceNo");
            Label MainInvoiceID = (Label)e.Row.FindControl("lblMainInvoiceID");
            if(MainInvoiceID.Text != "")
            hyperlnkMainInvoiceNo.NavigateUrl = "NewInvoice?InvID=" + Global.Encrypt(MainInvoiceID.Text);
        }

        // ----------------------- Set Approved Button Link ------------------------------
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblApprovedBy = e.Row.FindControl("lblApprovedBy") as Label;

            LinkButton lnkbtnApproved = e.Row.FindControl("lnkbtnApproved") as LinkButton;
            Label lblApproved = e.Row.FindControl("lblApproved") as Label;

            if (lblApprovedBy.Text == "")
            {
                lnkbtnApproved.Visible = true;
                lblApproved.Visible = false;
            }
            else
            {
                lnkbtnApproved.Visible = false;
                lblApproved.Visible = true;
            }
        }
        // -------------------------------------------------------------------------------


    }

    protected void gvInv_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvInv.EditIndex = -1;
        gvInv.ShowFooter = true;
        gvInv.FooterRow.Visible = true;
        gvInv.PagerSettings.Visible = true;
        FillGrid();
    }

    protected void gvInv_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        Label lblInvoiceID = (Label)gvInv.Rows[e.RowIndex].FindControl("lblInvoiceID");
        var DelData = from DBInvoice in dbobj.ProformaInvoiceMasters
                      where DBInvoice.ProInvoiceId == Convert.ToInt64(lblInvoiceID.Text)
                      select DBInvoice;
        if (DelData.Count() > 0)
        {
            var SingleDelete = DelData.Single();
            if (SingleDelete.Revision == null)
            {
                SingleDelete.ModifyBy = Convert.ToInt64(Global.UserId);
                SingleDelete.ModifyDate = DateTime.Now;
                SingleDelete.IsDeleted = true;
                dbobj.SubmitChanges();
                dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
            }
            else
            {
                string InvoiceNumber = SingleDelete.InvoiceNumber.Replace(SingleDelete.Revision, "").Trim();
                var DelInvSeries = from DBDelData in dbobj.ProformaInvoiceMasters
                                   where DBDelData.InvoiceNumber.StartsWith(InvoiceNumber) && DBDelData.InvoiceSeqNo == SingleDelete.InvoiceSeqNo
                                   select DBDelData;
                foreach (var inv in DelInvSeries)
                {
                    inv.ModifyBy = Convert.ToInt64(Global.UserId);
                    inv.ModifyDate = DateTime.Now;
                    inv.IsDeleted = true;
                    dbobj.SubmitChanges();
                    dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                }
            }
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
        var UpdateData = from DBInvoice in dbobj.ProformaInvoiceMasters
                         where DBInvoice.ProInvoiceId == Convert.ToInt64(lblInvoiceID.Text)
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

    protected void drpBusiness_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillClient();
        FillGrid();
        if (ddlClient.SelectedIndex == 0 && drpInvoiceStatus.Text == "Unpaid")
        {
            gvInv.Columns[12].Visible = false;
        }
        else
        {
            if (gvInv.Rows[0].Cells[0].Text != "No Record to Display" && drpInvoiceStatus.Text == "Unpaid" && Global.UserType == "DIRECTOR" && drpBusiness.SelectedIndex != 0)
            {
                gvInv.Columns[12].Visible = true;
            }
            else if (gvInv.Rows[0].Cells[0].Text != "No Record to Display" && drpInvoiceStatus.Text == "Draft" && Global.UserType != "DIRECTOR" && ddlClient.SelectedIndex != 0)
            {
                gvInv.Columns[12].Visible = true;
            }
            else
            {
                gvInv.Columns[12].Visible = false;
            }
        }
        ViewState["SendI"] = "";
    }

    protected void drpInvoiceStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        TextBox txtMasterSearch = (TextBox)this.Master.FindControl("txtMasterSearch");
        txtMasterSearch.Text = "";
        FillGrid();
        if (ddlClient.SelectedIndex == 0 && drpInvoiceStatus.Text == "Unpaid")
        {
            gvInv.Columns[12].Visible = false;
        }
        else
        {
            if (gvInv.Rows[0].Cells[0].Text != "No Record to Display" && drpInvoiceStatus.Text == "Unpaid" && Global.UserType == "DIRECTOR" && drpBusiness.SelectedIndex != 0)
            {
                gvInv.Columns[12].Visible = true;
            }
            else if (gvInv.Rows[0].Cells[0].Text != "No Record to Display" && drpInvoiceStatus.Text == "Draft" && Global.UserType != "DIRECTOR" && ddlClient.SelectedIndex != 0)
            {
                gvInv.Columns[12].Visible = true;
            }
            else
            {
                gvInv.Columns[12].Visible = false;
            }
        }
        ViewState["SendI"] = "";
    }

    protected void ddlClient_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
        if (ddlClient.SelectedIndex == 0 && drpInvoiceStatus.Text == "Unpaid")
        {
            gvInv.Columns[12].Visible = false;
        }
        else
        {
            if (gvInv.Rows[0].Cells[0].Text != "No Record to Display" && drpInvoiceStatus.Text == "Unpaid" && (Global.UserType == "DIRECTOR" || Global.DepartmentName == "ACCT") && drpBusiness.SelectedIndex != 0)
            {
                gvInv.Columns[12].Visible = true;
            }
            else if (gvInv.Rows[0].Cells[0].Text != "No Record to Display" && drpInvoiceStatus.Text == "Draft" && Global.UserType != "DIRECTOR" && ddlClient.SelectedIndex != 0)
            {
                gvInv.Columns[12].Visible = true;
            }
            else
            {
                gvInv.Columns[12].Visible = false;
            }
        }
        ViewState["SendI"] = "";
    }

    // Set Invoice Inique Number =======================================================================
    private string GetSequanceINDIA(string FY, Int64 iDeptID)
    {
        string sSeqINDIA_Number = "01";

        var InvData = (from DBData in dbobj.InvoiceMasters
                       join
                       ClientData in dbobj.ClientMasters
                       on
                       DBData.ClientId equals ClientData.ClientId
                       orderby DBData.InvoiceSeqNo descending
                       where DBData.InvoiceNumber.Contains(FY) && ClientData.DepartmentId == iDeptID && !DBData.IsDeleted.Value && DBData.InvoiceFor == "INDIA"
                       select DBData);
        if (InvData.Count() > 0)
        {
            sSeqINDIA_Number = (InvData.First().InvoiceSeqNo + 1).ToString("00");
        }


        return sSeqINDIA_Number;
    }

    private string GetSequanceUSA(string FY, Int64 iDeptID)
    {
        string sSeqUSA_Number = "001";

        var InvData = (from DBData in dbobj.InvoiceMasters
                       join
                       ClientData in dbobj.ClientMasters
                       on DBData.ClientId equals ClientData.ClientId
                       orderby DBData.InvoiceSeqNo descending
                       where DBData.InvoiceNumber.Contains(FY) && ClientData.DepartmentId == iDeptID && !DBData.IsDeleted.Value && DBData.InvoiceFor == "USA"
                       select DBData);
        if (InvData.Count() > 0)
        {
            sSeqUSA_Number = (InvData.First().InvoiceSeqNo + 1).ToString("000");
        }


        return sSeqUSA_Number;
    }

    private string GetProformaInvoiceNumber_INDIA(string FY, string dName, string seqNumber)
    {
        string sNumber = string.Empty;
        string departmentName = dName.Substring(0, 2);

        sNumber = "MITPL" + FY + departmentName + "/" + seqNumber;

        return sNumber;
    }

    private string GetProfomaInvoiceNumber_USA(string FY, string dName, string seqNumber)
    {
        string sNumber = string.Empty;

        sNumber = "MIT/" + FY + "/" + dName + "/" + seqNumber;

        return sNumber;
    }
    // =================================================================================================

    protected void lnkbtnApproved_Click(object sender, EventArgs e)
    {
        DateTime CurrentDate = DateTime.Now;
        int CDate = Convert.ToInt32(CurrentDate.ToString("dd"));
        //int CDate = 5;

        GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
        Label ProInvoiceId = (Label)clickedRow.FindControl("lblInvoiceID");

        var getproformaInvoice = (from pI in dbobj.ProformaInvoiceMasters where pI.ProInvoiceId == Convert.ToInt32(ProInvoiceId.Text) select pI).ToList();

        var GetProInvLI = (from PILI in dbobj.ProformaInvoiceDetailsMasters where PILI.ProInvoiceId == Convert.ToInt32(ProInvoiceId.Text) select PILI).ToList();

        #region Add Invoice into ProformaInvoice to Main InvoiceMaster TABLE

        DepartmentName = Convert.ToInt64(getproformaInvoice.Single().BusinessUnit);

        InvoiceMaster im = new InvoiceMaster();
        im.ClientId = getproformaInvoice.Single().ClientId;
        im.InvoiceSeqNo = getproformaInvoice.Single().InvoiceSeqNo;
        //im.InvoiceNumber = getproformaInvoice.Single().InvoiceNumber;

        string financialYear = string.Empty;
        int CurrYear = Convert.ToDateTime(getproformaInvoice.Single().InvoiceDate).Year;

        if (getproformaInvoice.Single().InvoiceFor == "USA")
        {
            if (getproformaInvoice.Single().InvoiceDate <= Convert.ToDateTime("03/31/" + CurrYear))
                financialYear = (CurrYear - 1) + "-" + CurrYear.ToString().Substring(2, 2);
            else
                financialYear = CurrYear + "-" + (CurrYear + 1).ToString().Substring(2, 2);
        }
        else
        {
            if (getproformaInvoice.Single().InvoiceDate <= Convert.ToDateTime("03/31/" + CurrYear))
                financialYear = (CurrYear - 1).ToString().Substring(2, 2) + "-" + CurrYear.ToString().Substring(2, 2);
            else
                financialYear = CurrYear.ToString().Substring(2, 2) + "-" + (CurrYear + 1).ToString().Substring(2, 2);
        }

        string seqNo = string.Empty;
        if (getproformaInvoice.Single().InvoiceFor == "USA")
            seqNo = GetSequanceUSA(financialYear, DepartmentName);
        else
            seqNo = GetSequanceINDIA(financialYear, DepartmentName);

        im.InvoiceSeqNo = Convert.ToInt64(seqNo);

        var GetDeptName = (from d in dbobj.DepartmentMasters where d.DepartmentId == DepartmentName select d).ToList();

        if (getproformaInvoice.Single().InvoiceFor == "USA")
            im.InvoiceNumber = GetProfomaInvoiceNumber_USA(financialYear, GetDeptName.Single().DepartmentName, seqNo);
        else
            im.InvoiceNumber = GetProformaInvoiceNumber_INDIA(financialYear, GetDeptName.Single().DepartmentName, seqNo);

        im.InvoiceFor = getproformaInvoice.Single().InvoiceFor;
        im.Revision = getproformaInvoice.Single().Revision;
        im.PONumber = getproformaInvoice.Single().PONumber;
        im.PODate = getproformaInvoice.Single().PODate;
        im.InvoiceDate = getproformaInvoice.Single().InvoiceDate;
        //im.InvoiceStartDate = getproformaInvoice.Single().InvoiceStartDate;
        //im.InvoiceEndDate = getproformaInvoice.Single().InvoiceEndDate;

        if (CDate <= 10)
        {
            im.InvoiceStartDate = getproformaInvoice.Single().InvoiceStartDate;
            im.InvoiceEndDate = getproformaInvoice.Single().InvoiceEndDate;
        }
        else
        {
            //im.InvoiceEndDate = getproformaInvoice.Single().InvoiceEndDate.AddMonths(1);
            im.InvoiceStartDate = getproformaInvoice.Single().InvoiceStartDate.AddMonths(1);
            im.InvoiceEndDate = DateTime.Now;
        }

        im.ProjectFrom = getproformaInvoice.Single().ProjectFrom;
        im.AttachmentName = getproformaInvoice.Single().AttachmentName;
        im.Remarks = getproformaInvoice.Single().Remarks;
        im.IsPaid = getproformaInvoice.Single().IsPaid;
        im.PaidDate = getproformaInvoice.Single().PaidDate;
        im.InvoiceStatus = "Draft";
        im.CreatedBy = Convert.ToInt64(Global.UserId);
        im.CreatedDate = DateTime.Now;
        im.ModifyDate = DateTime.Now;
        im.ModifyBy = Convert.ToInt64(Global.UserId);
        im.IsDeleted = false;
        im.IsRevised = false;
        im.ApprovedBy = Convert.ToInt64(Global.UserId);
        im.ApprovedDate = DateTime.Now;
        im.ProInvoiceId = getproformaInvoice.Single().ProInvoiceId;

        dbobj.InvoiceMasters.InsertOnSubmit(im);
        dbobj.SubmitChanges();

        ViewState["InvID"] = im.InvoiceId;
        #endregion

        #region update PromormaInvoice records
        getproformaInvoice.Single().IsApprovedByInvoice = Convert.ToInt64(Global.UserId);
        getproformaInvoice.Single().IsApprovedDateInvoice = DateTime.Now;
        getproformaInvoice.Single().InvoiceStatus = "Approved";
        getproformaInvoice.Single().MainInvoiceID = im.InvoiceId;

        dbobj.SubmitChanges();
        #endregion

        #region Added related Invoice Line Items
        foreach (var ILI in GetProInvLI)
        {
            InvoiceDetailsMaster idm = new InvoiceDetailsMaster();

            idm.InvoiceId = Convert.ToInt32(ViewState["InvID"]);
            idm.ItemDesc = ILI.ItemDesc;
            idm.PriceType = ILI.PriceType;
            idm.Qty = ILI.Qty;
            idm.UnitPrice = ILI.UnitPrice;
            idm.Discount = ILI.Discount;
            idm.TotalAmt = ILI.TotalAmt;
            idm.CreatedBy = Convert.ToInt64(Global.UserId);
            idm.CreatedDate = DateTime.Now;
            idm.ProInvoiceDetailsId = ILI.ProInvoiceDetailsId;

            dbobj.InvoiceDetailsMasters.InsertOnSubmit(idm);
            dbobj.SubmitChanges();
            dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
        }
        #endregion

        FillGrid();

        Response.Redirect("ViewProformaInvoice");

    }

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)sender;
        GridViewRow gr = (GridViewRow)chk.NamingContainer;
        CheckBox chkSelectAll = (CheckBox)gr.FindControl("chkSelectAll");

        var InvoiceData = from DBData in dbobj.ProformaInvoiceMasters
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
                              InvoiceID = DBData.ProInvoiceId
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
        }
        if (chkSelectAll.Checked == false)
        {
            ViewState["SendI"] = "";

            foreach (GridViewRow row in gvInv.Rows)
            {
                CheckBox chkClient = (CheckBox)row.FindControl("chkClient");
                chkClient.Checked = false;
            }
        }
    }

    protected void chkClient_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)sender;
        GridViewRow gr = (GridViewRow)chk.NamingContainer;
        CheckBox chkClient = (CheckBox)gr.FindControl("chkClient");

        var InvoiceData = from DBData in dbobj.ProformaInvoiceMasters
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
                              InvoiceID = DBData.ProInvoiceId
                          };

        int RowsNo = InvoiceData.Count();
        CheckBox chkAll = (CheckBox)(gvInv.HeaderRow.FindControl("chkSelectAll"));

        CheckBox ckcl = (CheckBox)(gr.FindControl("chkClient"));
        Label lblIdInv = (Label)(gr.FindControl("lblInvoiceID"));
        if (ckcl.Checked == true)
        {
            ViewState["SendI"] = ViewState["SendI"] + lblIdInv.Text + ",";
        }
        else
        {
            ViewState["SendI"] = ViewState["SendI"].ToString().Replace(lblIdInv.Text + ",", "");
        }

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
                }
            }
            else
            {
                chkAll.Checked = false;

            }
        }
        else
            chkAll.Checked = false;
    }
}