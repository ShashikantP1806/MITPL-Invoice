using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

public partial class ViewInvoices : System.Web.UI.Page
{
    MITInvoiceDataContext dbobj = new MITInvoiceDataContext();

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

                ////---gvInv.Columns[9].Visible = false; ////Edit | Del Column Change column index 8 to 9
                gvInv.Columns[10].Visible = false; ////Edit | Del Column Change column index 8 to 9

            }
            else
            {
                ////---gvInv.Columns[9].Visible = true; ////Edit | Del Column
                gvInv.Columns[10].Visible = true; ////Edit | Del Column
            }            
        }
        else
        {
            Response.Redirect("Authorize");
        }
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

            //if (Global.UserType == "DIRECTOR" && Global.UserM1 && Global.UserM2)
            //{
            //    string strDeptName = "SYSTEM";
            //    ListItem li1 = drpBusiness.Items.FindByText(strDeptName);
            //    if (li1 != null)
            //        drpBusiness.Items.FindByText(strDeptName).Selected = true;
            //}
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

    // private void FillBusinessUnit_Existing()
    // {
    //     if (Global.UserType == "DIRECTOR" || Global.DepartmentName == "ACCT")
    //     {
    //         var BUData = from DBData in dbobj.DepartmentMasters
    //                      orderby DBData.DepartmentName
    //                      where DBData.IsActive
    //                      select new
    //                      {
    //                          BUID = DBData.DepartmentId,
    //                          BUName = DBData.DepartmentName
    //                      };
    //         if (BUData.Count() > 0)
    //         {
    //             drpBusiness.DataSource = BUData;
    //             drpBusiness.DataTextField = "BUName";
    //             drpBusiness.DataValueField = "BUID";
    //             drpBusiness.DataBind();
    //             drpBusiness.Items.Insert(0, "--All--");
    //             drpBusiness.SelectedIndex = 0;
    //         }
    //     }
    //     else
    //     {
    //         var BUData = from DBData in dbobj.DepartmentMasters
    //                      where DBData.IsActive && (DBData.UserId == Convert.ToInt64(Global.UserId) || DBData.DepartmentId == Convert.ToInt64(Global.Department))
    //                      orderby DBData.DepartmentName
    //                      select new
    //                      {
    //                          BUID = DBData.DepartmentId,
    //                          BUName = DBData.DepartmentName
    //                      };
    //         if (BUData.Count() > 0)
    //         {
    //             drpBusiness.DataSource = BUData;
    //             drpBusiness.DataTextField = "BUName";
    //             drpBusiness.DataValueField = "BUID";
    //             drpBusiness.DataBind();
    //             drpBusiness.SelectedIndex = 0;
    //         }
    //     }
    // }

    private void BlankGrid()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("InvoiceID"));
        //dt.Columns.Add(new DataColumn("InvoiceNo"));
        dt.Columns.Add(new DataColumn("InvoiceFor"));
        dt.Columns.Add(new DataColumn("InvoiceNum"));
        dt.Columns.Add(new DataColumn("ProInvoiceNum"));
        dt.Columns.Add(new DataColumn("InvSeq"));
        dt.Columns.Add(new DataColumn("ClientName"));
        dt.Columns.Add(new DataColumn("InvoiceAmount"));
        dt.Columns.Add(new DataColumn("InvoiceDate"));
        dt.Columns.Add(new DataColumn("PaidDate"));
        dt.Columns.Add(new DataColumn("InvoiceStatus"));
        dt.Columns.Add(new DataColumn("MainProInvoiceID"));
        //dt.Columns.Add(new DataColumn("ApprovedBy"));

        gvInv.DataSource = dt;
        dt.Rows.Add(dt.NewRow());
        gvInv.DataBind();
        int TotalCols = gvInv.Rows[0].Cells.Count;
        gvInv.Rows[0].Cells.Clear();
        gvInv.Rows[0].Cells.Add(new TableCell());
        gvInv.Rows[0].Cells[0].ColumnSpan = TotalCols;
        gvInv.Rows[0].Cells[0].Text = "No Record to Display";
        ////---gvInv.Columns[10].Visible = false; //// Chcekbox Column Change on 30-Apr-18 by Jignesh
        gvInv.Columns[11].Visible = false; //// Chcekbox Column Change on 30-Apr-18 by Jignesh
        btnIntimate.Enabled = false;
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
                        //// 12-Aug-2020 By Jignesh
                        //// added && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2)
                        var InvoiceData = from DBData in dbobj.InvoiceMasters
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
                                              InvoiceID = DBData.InvoiceId,
                                              //InvoiceNo = DBData.InvoiceNumber,
                                              InvoiceFor = DBData.InvoiceFor, //// Country added by Jignesh for display
                                              InvoiceNum = DBData.InvoiceNumber.Substring(0, DBData.InvoiceNumber.LastIndexOf("/") + 1),
                                              ////InvSeq = DBData.Revision == null ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision,
                                              ////Changes by Jignesh on 25-Apr-2018 for India format "01" and USA format "001"
                                              InvSeq = (DBData.Revision == null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : (DBData.Revision != null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision : (DBData.Revision == null && DBData.InvoiceFor == "INDIA") ? DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') + DBData.Revision,
                                              ClientName = ClientData.ClientName,
                                              ProInvoiceNum = DBData.ProformaInvoiceMaster.InvoiceNumber,
                                              MainProInvoiceID = (int?)DBData.ProformaInvoiceMaster.ProInvoiceId,
                                              //InvoiceAmount = "&#" + (CurrencyData.CurrencySymbol + ";").Replace(";;", ";") + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                              //                                                                                        where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                              //                                                                                        select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                              //                                                                                                                                                where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                              //                                                                                                                                                select InvData.TotalAmt.Value).Sum().ToString()),
                                              InvoiceAmount = CurrencyData.CurrencyCode + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                                                                                  where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                                                                  select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                                                                                                                                          where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
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
                                              //InvoiceNo = DBData.InvoiceNumber,
                                              InvoiceFor = DBData.InvoiceFor, //// Country added by Jignesh for display
                                              InvoiceNum = DBData.InvoiceNumber.Substring(0, DBData.InvoiceNumber.LastIndexOf("/") + 1),
                                              ////InvSeq = DBData.Revision == null ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision,
                                              ////Changes by Jignesh on 25-Apr-2018 for India format "01" and USA format "001"
                                              InvSeq = (DBData.Revision == null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : (DBData.Revision != null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision : (DBData.Revision == null && DBData.InvoiceFor == "INDIA") ? DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') + DBData.Revision,
                                              ClientName = ClientData.ClientName,
                                              ProInvoiceNum = DBData.ProformaInvoiceMaster.InvoiceNumber,
                                              MainProInvoiceID = (int?)DBData.ProformaInvoiceMaster.ProInvoiceId,
                                              //InvoiceAmount = "&#" + (CurrencyData.CurrencySymbol + ";").Replace(";;", ";") + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                              //                                                                                        where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                              //                                                                                        select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                              //                                                                                                                                                where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                              //                                                                                                                                                select InvData.TotalAmt.Value).Sum().ToString()),
                                              InvoiceAmount = CurrencyData.CurrencyCode + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                                                                                  where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                                                                  select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                                                                                                                                          where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
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
                        //// 10-Aug-2020 By Jignesh
                        //// added : && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2)
                        var InvoiceData = from DBData in dbobj.InvoiceMasters
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
                                              InvoiceID = DBData.InvoiceId,
                                              //InvoiceNo = DBData.InvoiceNumber,
                                              InvoiceFor = DBData.InvoiceFor, //// Country added by Jignesh for display
                                              InvoiceNum = DBData.InvoiceNumber.Substring(0, DBData.InvoiceNumber.LastIndexOf("/") + 1),
                                              ////InvSeq = DBData.Revision == null ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision,
                                              ////Changes by Jignesh on 25-Apr-2018 for India format "01" and USA format "001"
                                              InvSeq = (DBData.Revision == null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : (DBData.Revision != null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision : (DBData.Revision == null && DBData.InvoiceFor == "INDIA") ? DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') + DBData.Revision,
                                              ClientName = ClientData.ClientName,
                                              ProInvoiceNum = DBData.ProformaInvoiceMaster.InvoiceNumber,
                                              MainProInvoiceID = (int?)DBData.ProformaInvoiceMaster.ProInvoiceId,
                                              //InvoiceAmount = "&#" + (CurrencyData.CurrencySymbol + ";").Replace(";;", ";") + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                              //                                                                                        where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                              //                                                                                        select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                              //                                                                                                                                                where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                              //                                                                                                                                                select InvData.TotalAmt.Value).Sum().ToString()),
                                              InvoiceAmount = CurrencyData.CurrencyCode + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                                                                                  where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                                                                  select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                                                                                                                                          where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
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
                                              //InvoiceNo = DBData.InvoiceNumber,
                                              InvoiceFor = DBData.InvoiceFor, //// Country added by Jignesh for display
                                              InvoiceNum = DBData.InvoiceNumber.Substring(0, DBData.InvoiceNumber.LastIndexOf("/") + 1),
                                              ////InvSeq = DBData.Revision == null ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision,
                                              ////Changes by Jignesh on 25-Apr-2018 for India format "01" and USA format "001"
                                              InvSeq = (DBData.Revision == null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : (DBData.Revision != null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision : (DBData.Revision == null && DBData.InvoiceFor == "INDIA") ? DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') + DBData.Revision,
                                              ClientName = ClientData.ClientName,
                                              ProInvoiceNum = DBData.ProformaInvoiceMaster.InvoiceNumber,
                                              MainProInvoiceID = (int?)DBData.ProformaInvoiceMaster.ProInvoiceId,
                                              //InvoiceAmount = "&#" + (CurrencyData.CurrencySymbol + ";").Replace(";;", ";") + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                              //                                                                                        where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                              //                                                                                        select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                              //                                                                                                                                                where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                              //                                                                                                                                                select InvData.TotalAmt.Value).Sum().ToString()),
                                              InvoiceAmount = CurrencyData.CurrencyCode + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                                                                                  where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                                                                  select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                                                                                                                                          where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
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
                    //// 10-Aug-2020 By Jignesh
                    //// added (DBData.C_M1 == Global.UserM1 || DBData.C_M2 == Global.UserM2)
                    var InvoiceData = from DBData in dbobj.InvoiceMasters
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
                                          InvoiceID = DBData.InvoiceId,
                                          //InvoiceNo = DBData.InvoiceNumber,
                                          InvoiceFor = DBData.InvoiceFor, //// Country added by Jignesh for display
                                          InvoiceNum = DBData.InvoiceNumber.Substring(0, DBData.InvoiceNumber.LastIndexOf("/") + 1),
                                          //InvSeq = DBData.Revision == null ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') + DBData.Revision,
                                          ////Changes by Jignesh on 25-Apr-2018 for India format "01" and USA format "001"
                                          InvSeq = (DBData.Revision == null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : (DBData.Revision != null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision : (DBData.Revision == null && DBData.InvoiceFor == "INDIA") ? DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') + DBData.Revision,

                                          ClientName = ClientData.ClientName,
                                          ProInvoiceNum = DBData.ProformaInvoiceMaster.InvoiceNumber,
                                          MainProInvoiceID = (int?)DBData.ProformaInvoiceMaster.ProInvoiceId,
                                          //InvoiceAmount = "&#" + (CurrencyData.CurrencySymbol + ";").Replace(";;", ";") + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                          //                                                                                        where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                          //                                                                                        select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                          //                                                                                                                                                where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                          //                                                                                                                                                select InvData.TotalAmt.Value).Sum().ToString()),
                                          InvoiceAmount = CurrencyData.CurrencyCode + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                                                                              where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                                                              select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                                                                                                                                      where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
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
                                          //InvoiceNo = DBData.InvoiceNumber,
                                          InvoiceFor = DBData.InvoiceFor, //// Country added by Jignesh for display
                                          InvoiceNum = DBData.InvoiceNumber.Substring(0, DBData.InvoiceNumber.LastIndexOf("/") + 1),
                                          ////InvSeq = DBData.Revision == null ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision,
                                          ////Changes by Jignesh on 25-Apr-2018 for India format "01" and USA format "001"
                                          InvSeq = (DBData.Revision == null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : (DBData.Revision != null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision : (DBData.Revision == null && DBData.InvoiceFor == "INDIA") ? DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') + DBData.Revision,
                                          ClientName = ClientData.ClientName,
                                          ProInvoiceNum = DBData.ProformaInvoiceMaster.InvoiceNumber,
                                          MainProInvoiceID = (int?)DBData.ProformaInvoiceMaster.ProInvoiceId,
                                          //InvoiceAmount = "&#" + (CurrencyData.CurrencySymbol + ";").Replace(";;", ";") + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                          //                                                                                        where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                          //                                                                                        select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                          //                                                                                                                                                where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                          //                                                                                                                                                select InvData.TotalAmt.Value).Sum().ToString()),
                                          InvoiceAmount = CurrencyData.CurrencyCode + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                                                                              where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                                                              select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                                                                                                                                      where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
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
            //// Search not equals to blank
            try
            {
                int Seq = Convert.ToInt32(txtMasterSearch.Text.Trim());
                if (Global.UserType == "DIRECTOR" || Global.DepartmentName == "ACCT")
                {
                    if (drpBusiness.SelectedIndex == 0)
                    {
                        if (ddlClient.SelectedIndex == 0)
                        {
                            //// 21-Aug-2020 By Jignesh
                            //// added : && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2)
                            var InvoiceData = from DBData in dbobj.InvoiceMasters
                                              join ClientData in dbobj.ClientMasters
                                              on DBData.ClientId equals ClientData.ClientId
                                              join CurrencyData in dbobj.CurrencyMasters
                                              on ClientData.CurrencyId equals CurrencyData.CurrencyId
                                              where DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && DBData.InvoiceSeqNo == Seq && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2)
                                              select new
                                              {
                                                  InvoiceID = DBData.InvoiceId,
                                                  //InvoiceNo = DBData.InvoiceNumber,
                                                  InvoiceFor = DBData.InvoiceFor, //// Country added by Jignesh for display
                                                  InvoiceNum = DBData.InvoiceNumber.Substring(0, DBData.InvoiceNumber.LastIndexOf("/") + 1),
                                                  ////InvSeq = DBData.Revision == null ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision,
                                                  ////Changes by Jignesh on 25-Apr-2018 for India format "01" and USA format "001"
                                                  InvSeq = (DBData.Revision == null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : (DBData.Revision != null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision : (DBData.Revision == null && DBData.InvoiceFor == "INDIA") ? DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') + DBData.Revision,
                                                  ClientName = ClientData.ClientName,
                                                  ProInvoiceNum = DBData.ProformaInvoiceMaster.InvoiceNumber,
                                                  MainProInvoiceID = (int?)DBData.ProformaInvoiceMaster.ProInvoiceId,
                                                  //InvoiceAmount = "&#" + (CurrencyData.CurrencySymbol + ";").Replace(";;", ";") + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                                  //                                                                                        where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                  //                                                                                        select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                                  //                                                                                                                                                where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                  //                                                                                                                                                select InvData.TotalAmt.Value).Sum().ToString()),
                                                  InvoiceAmount = CurrencyData.CurrencyCode + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                                                                                      where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                                                                      select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                                                                                                                                              where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
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
                            //// 21-Aug-2020 By Jignesh
                            //// added : && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2)

                            var InvoiceData = from DBData in dbobj.InvoiceMasters
                                              join ClientData in dbobj.ClientMasters
                                              on DBData.ClientId equals ClientData.ClientId
                                              join CurrencyData in dbobj.CurrencyMasters
                                              on ClientData.CurrencyId equals CurrencyData.CurrencyId
                                              where DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && ClientData.ClientId == Convert.ToInt64(ddlClient.SelectedValue) && DBData.InvoiceSeqNo == Seq && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2)
                                              select new
                                              {
                                                  InvoiceID = DBData.InvoiceId,
                                                  //InvoiceNo = DBData.InvoiceNumber,
                                                  InvoiceFor = DBData.InvoiceFor, //// Country added by Jignesh for display
                                                  InvoiceNum = DBData.InvoiceNumber.Substring(0, DBData.InvoiceNumber.LastIndexOf("/") + 1),
                                                  ////InvSeq = DBData.Revision == null ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision,
                                                  ////Changes by Jignesh on 25-Apr-2018 for India format "01" and USA format "001"
                                                  InvSeq = (DBData.Revision == null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : (DBData.Revision != null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision : (DBData.Revision == null && DBData.InvoiceFor == "INDIA") ? DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') + DBData.Revision,
                                                  ClientName = ClientData.ClientName,
                                                  ProInvoiceNum = DBData.ProformaInvoiceMaster.InvoiceNumber,
                                                  MainProInvoiceID = (int?)DBData.ProformaInvoiceMaster.ProInvoiceId,
                                                  //InvoiceAmount = "&#" + (CurrencyData.CurrencySymbol + ";").Replace(";;", ";") + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                                  //                                                                                        where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                  //                                                                                        select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                                  //                                                                                                                                                where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                  //                                                                                                                                                select InvData.TotalAmt.Value).Sum().ToString()),
                                                  InvoiceAmount = CurrencyData.CurrencyCode + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                                                                                      where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                                                                      select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                                                                                                                                              where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
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
                            //// 21-Aug-2020 By Jignesh
                            //// added : && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2)

                            var InvoiceData = from DBData in dbobj.InvoiceMasters
                                              join ClientData in dbobj.ClientMasters
                                              on DBData.ClientId equals ClientData.ClientId
                                              join CurrencyData in dbobj.CurrencyMasters
                                              on ClientData.CurrencyId equals CurrencyData.CurrencyId
                                              where DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && ClientData.DepartmentId == Convert.ToInt64(drpBusiness.SelectedValue) && DBData.InvoiceSeqNo == Seq && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2)
                                              select new
                                              {
                                                  InvoiceID = DBData.InvoiceId,
                                                  //InvoiceNo = DBData.InvoiceNumber,
                                                  InvoiceFor = DBData.InvoiceFor, //// Country added by Jignesh for display
                                                  InvoiceNum = DBData.InvoiceNumber.Substring(0, DBData.InvoiceNumber.LastIndexOf("/") + 1),
                                                  ////InvSeq = DBData.Revision == null ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision,
                                                  ////Changes by Jignesh on 25-Apr-2018 for India format "01" and USA format "001"
                                                  InvSeq = (DBData.Revision == null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : (DBData.Revision != null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision : (DBData.Revision == null && DBData.InvoiceFor == "INDIA") ? DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') + DBData.Revision,
                                                  ClientName = ClientData.ClientName,
                                                  ProInvoiceNum = DBData.ProformaInvoiceMaster.InvoiceNumber,
                                                  MainProInvoiceID = (int?)DBData.ProformaInvoiceMaster.ProInvoiceId,
                                                  //InvoiceAmount = "&#" + (CurrencyData.CurrencySymbol + ";").Replace(";;", ";") + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                                  //                                                                                        where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                  //                                                                                        select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                                  //                                                                                                                                                where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                  //                                                                                                                                                select InvData.TotalAmt.Value).Sum().ToString()),
                                                  InvoiceAmount = CurrencyData.CurrencyCode + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                                                                                      where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                                                                      select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                                                                                                                                              where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
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
                            //// 21-Aug-2020 By Jignesh
                            //// added : && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2)
                            var InvoiceData = from DBData in dbobj.InvoiceMasters
                                              join ClientData in dbobj.ClientMasters
                                              on DBData.ClientId equals ClientData.ClientId
                                              join CurrencyData in dbobj.CurrencyMasters
                                              on ClientData.CurrencyId equals CurrencyData.CurrencyId
                                              where DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && ClientData.ClientId == Convert.ToInt64(ddlClient.SelectedValue) && DBData.InvoiceSeqNo == Seq
                                              && ClientData.DepartmentId == Convert.ToInt64(drpBusiness.SelectedValue) && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2)
                                              select new
                                              {
                                                  InvoiceID = DBData.InvoiceId,
                                                  //InvoiceNo = DBData.InvoiceNumber,
                                                  InvoiceFor = DBData.InvoiceFor, //// Country added by Jignesh for display
                                                  InvoiceNum = DBData.InvoiceNumber.Substring(0, DBData.InvoiceNumber.LastIndexOf("/") + 1),
                                                  ////InvSeq = DBData.Revision == null ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision,
                                                  ////Changes by Jignesh on 25-Apr-2018 for India format "01" and USA format "001"
                                                  InvSeq = (DBData.Revision == null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : (DBData.Revision != null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision : (DBData.Revision == null && DBData.InvoiceFor == "INDIA") ? DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') + DBData.Revision,
                                                  ClientName = ClientData.ClientName,
                                                  ProInvoiceNum = DBData.ProformaInvoiceMaster.InvoiceNumber,
                                                  MainProInvoiceID = (int?)DBData.ProformaInvoiceMaster.ProInvoiceId,
                                                  //InvoiceAmount = "&#" + (CurrencyData.CurrencySymbol + ";").Replace(";;", ";") + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                                  //                                                                                        where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                  //                                                                                        select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                                  //                                                                                                                                                where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                  //                                                                                                                                                select InvData.TotalAmt.Value).Sum().ToString()),
                                                  InvoiceAmount = CurrencyData.CurrencyCode + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                                                                                      where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                                                                      select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                                                                                                                                              where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
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
                        //// 21-Aug-2020 By Jignesh
                        //// added : && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2)
                        var InvoiceData = from DBData in dbobj.InvoiceMasters
                                          join ClientData in dbobj.ClientMasters
                                          on DBData.ClientId equals ClientData.ClientId
                                          join CurrencyData in dbobj.CurrencyMasters
                                          on ClientData.CurrencyId equals CurrencyData.CurrencyId
                                          where ClientData.DepartmentId == Convert.ToInt64(drpBusiness.SelectedValue) && DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && DBData.InvoiceSeqNo == Seq && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2)
                                          select new
                                          {
                                              InvoiceID = DBData.InvoiceId,
                                              //InvoiceNo = DBData.InvoiceNumber,
                                              InvoiceFor = DBData.InvoiceFor, //// Country added by Jignesh for display
                                              InvoiceNum = DBData.InvoiceNumber.Substring(0, DBData.InvoiceNumber.LastIndexOf("/") + 1),
                                              ////InvSeq = DBData.Revision == null ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision,
                                              ////Changes by Jignesh on 25-Apr-2018 for India format "01" and USA format "001"
                                              InvSeq = (DBData.Revision == null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : (DBData.Revision != null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision : (DBData.Revision == null && DBData.InvoiceFor == "INDIA") ? DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') + DBData.Revision,
                                              ClientName = ClientData.ClientName,
                                              ProInvoiceNum = DBData.ProformaInvoiceMaster.InvoiceNumber,
                                              MainProInvoiceID = (int?)DBData.ProformaInvoiceMaster.ProInvoiceId,
                                              //InvoiceAmount = "&#" + (CurrencyData.CurrencySymbol + ";").Replace(";;", ";") + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                              //                                                                                        where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                              //                                                                                        select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                              //                                                                                                                                                where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                              //                                                                                                                                                select InvData.TotalAmt.Value).Sum().ToString()),
                                              InvoiceAmount = CurrencyData.CurrencyCode + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                                                                                  where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                                                                  select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                                                                                                                                          where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
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
                        //// 21-Aug-2020 By Jignesh
                        //// added : && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2)
                        var InvoiceData = from DBData in dbobj.InvoiceMasters
                                          join ClientData in dbobj.ClientMasters
                                          on DBData.ClientId equals ClientData.ClientId
                                          join CurrencyData in dbobj.CurrencyMasters
                                          on ClientData.CurrencyId equals CurrencyData.CurrencyId
                                          where ClientData.DepartmentId == Convert.ToInt64(drpBusiness.SelectedValue) && DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && DBData.InvoiceSeqNo == Seq
                                                && ClientData.ClientId == Convert.ToInt64(ddlClient.SelectedValue) && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2)
                                          select new
                                          {
                                              InvoiceID = DBData.InvoiceId,
                                              //InvoiceNo = DBData.InvoiceNumber,
                                              InvoiceFor = DBData.InvoiceFor, //// Country added by Jignesh for display
                                              InvoiceNum = DBData.InvoiceNumber.Substring(0, DBData.InvoiceNumber.LastIndexOf("/") + 1),
                                              ////InvSeq = DBData.Revision == null ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision,
                                              ////Changes by Jignesh on 25-Apr-2018 for India format "01" and USA format "001"
                                              InvSeq = (DBData.Revision == null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : (DBData.Revision != null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision : (DBData.Revision == null && DBData.InvoiceFor == "INDIA") ? DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') + DBData.Revision,
                                              ClientName = ClientData.ClientName,
                                              ProInvoiceNum = DBData.ProformaInvoiceMaster.InvoiceNumber,
                                              MainProInvoiceID = (int?)DBData.ProformaInvoiceMaster.ProInvoiceId,
                                              //InvoiceAmount = "&#" + (CurrencyData.CurrencySymbol + ";").Replace(";;", ";") + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                              //                                                                                        where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                              //                                                                                        select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                              //                                                                                                                                                where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                              //                                                                                                                                                select InvData.TotalAmt.Value).Sum().ToString()),
                                              InvoiceAmount = CurrencyData.CurrencyCode + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                                                                                  where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                                                                  select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                                                                                                                                          where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
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
                //where DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && DBData.ClientMaster.ClientName.StartsWith(txtMasterSearch.Text.Trim())
                if (Global.UserType == "DIRECTOR" || Global.DepartmentName == "ACCT")
                {
                    if (drpBusiness.SelectedIndex == 0)
                    {
                        if (ddlClient.SelectedIndex == 0)
                        {
                            //// 25-Aug-2020 By Jignesh
                            //// added : && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2)

                            var InvoiceData = from DBData in dbobj.InvoiceMasters
                                              join ClientData in dbobj.ClientMasters
                                              on DBData.ClientId equals ClientData.ClientId
                                              join CurrencyData in dbobj.CurrencyMasters
                                              on ClientData.CurrencyId equals CurrencyData.CurrencyId
                                              where DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && DBData.ClientMaster.ClientName.Contains(txtMasterSearch.Text.Trim()) && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2)
                                              select new
                                              {
                                                  InvoiceID = DBData.InvoiceId,
                                                  //InvoiceNo = DBData.InvoiceNumber,
                                                  InvoiceFor = DBData.InvoiceFor, //// Country added by Jignesh for display
                                                  InvoiceNum = DBData.InvoiceNumber.Substring(0, DBData.InvoiceNumber.LastIndexOf("/") + 1),
                                                  ////InvSeq = DBData.Revision == null ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision,
                                                  ////Changes by Jignesh on 25-Apr-2018 for India format "01" and USA format "001"
                                                  InvSeq = (DBData.Revision == null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : (DBData.Revision != null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision : (DBData.Revision == null && DBData.InvoiceFor == "INDIA") ? DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') + DBData.Revision,
                                                  ClientName = ClientData.ClientName,
                                                  ProInvoiceNum = DBData.ProformaInvoiceMaster.InvoiceNumber,
                                                  MainProInvoiceID = (int?)DBData.ProformaInvoiceMaster.ProInvoiceId,
                                                  //InvoiceAmount = "&#" + (CurrencyData.CurrencySymbol + ";").Replace(";;", ";") + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                                  //                                                                                        where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                  //                                                                                        select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                                  //                                                                                                                                                where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                  //                                                                                                                                                select InvData.TotalAmt.Value).Sum().ToString()),
                                                  InvoiceAmount = CurrencyData.CurrencyCode + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                                                                                      where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                                                                      select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                                                                                                                                              where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
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
                            //// 25-Aug-2020 By Jignesh
                            //// added : && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2)
                            var InvoiceData = from DBData in dbobj.InvoiceMasters
                                              join ClientData in dbobj.ClientMasters
                                              on DBData.ClientId equals ClientData.ClientId
                                              join CurrencyData in dbobj.CurrencyMasters
                                              on ClientData.CurrencyId equals CurrencyData.CurrencyId
                                              where DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && ClientData.ClientId == Convert.ToInt64(ddlClient.SelectedValue) && DBData.ClientMaster.ClientName.Contains(txtMasterSearch.Text.Trim()) && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2)
                                              select new
                                              {
                                                  InvoiceID = DBData.InvoiceId,
                                                  //InvoiceNo = DBData.InvoiceNumber,
                                                  InvoiceFor = DBData.InvoiceFor, //// Country added by Jignesh for display
                                                  InvoiceNum = DBData.InvoiceNumber.Substring(0, DBData.InvoiceNumber.LastIndexOf("/") + 1),
                                                  ////InvSeq = DBData.Revision == null ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision,
                                                  ////Changes by Jignesh on 25-Apr-2018 for India format "01" and USA format "001"
                                                  InvSeq = (DBData.Revision == null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : (DBData.Revision != null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision : (DBData.Revision == null && DBData.InvoiceFor == "INDIA") ? DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') + DBData.Revision,
                                                  ClientName = ClientData.ClientName,
                                                  ProInvoiceNum = DBData.ProformaInvoiceMaster.InvoiceNumber,
                                                  MainProInvoiceID = (int?)DBData.ProformaInvoiceMaster.ProInvoiceId,
                                                  //InvoiceAmount = "&#" + (CurrencyData.CurrencySymbol + ";").Replace(";;", ";") + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                                  //                                                                                        where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                  //                                                                                        select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                                  //                                                                                                                                                where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                  //                                                                                                                                                select InvData.TotalAmt.Value).Sum().ToString()),
                                                  InvoiceAmount = CurrencyData.CurrencyCode + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                                                                                      where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                                                                      select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                                                                                                                                              where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
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
                            //// 25-Aug-2020 By Jignesh
                            //// added : && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2)
                            var InvoiceData = from DBData in dbobj.InvoiceMasters
                                              join ClientData in dbobj.ClientMasters
                                              on DBData.ClientId equals ClientData.ClientId
                                              join CurrencyData in dbobj.CurrencyMasters
                                              on ClientData.CurrencyId equals CurrencyData.CurrencyId
                                              where DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && ClientData.DepartmentId == Convert.ToInt64(drpBusiness.SelectedValue) && DBData.ClientMaster.ClientName.Contains(txtMasterSearch.Text.Trim()) && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2)
                                              select new
                                              {
                                                  InvoiceID = DBData.InvoiceId,
                                                  //InvoiceNo = DBData.InvoiceNumber,
                                                  InvoiceFor = DBData.InvoiceFor, //// Country added by Jignesh for display
                                                  InvoiceNum = DBData.InvoiceNumber.Substring(0, DBData.InvoiceNumber.LastIndexOf("/") + 1),
                                                  ////InvSeq = DBData.Revision == null ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision,
                                                  ////Changes by Jignesh on 25-Apr-2018 for India format "01" and USA format "001"
                                                  InvSeq = (DBData.Revision == null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : (DBData.Revision != null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision : (DBData.Revision == null && DBData.InvoiceFor == "INDIA") ? DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') + DBData.Revision,
                                                  ClientName = ClientData.ClientName,
                                                  ProInvoiceNum = DBData.ProformaInvoiceMaster.InvoiceNumber,
                                                  MainProInvoiceID = (int?)DBData.ProformaInvoiceMaster.ProInvoiceId,
                                                  //InvoiceAmount = "&#" + (CurrencyData.CurrencySymbol + ";").Replace(";;", ";") + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                                  //                                                                                        where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                  //                                                                                        select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                                  //                                                                                                                                                where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                  //                                                                                                                                                select InvData.TotalAmt.Value).Sum().ToString()),
                                                  InvoiceAmount = CurrencyData.CurrencyCode + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                                                                                      where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                                                                      select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                                                                                                                                              where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
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
                            //// 25-Aug-2020 By Jignesh
                            //// added : && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2)
                            var InvoiceData = from DBData in dbobj.InvoiceMasters
                                              join ClientData in dbobj.ClientMasters
                                              on DBData.ClientId equals ClientData.ClientId
                                              join CurrencyData in dbobj.CurrencyMasters
                                              on ClientData.CurrencyId equals CurrencyData.CurrencyId
                                              where DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && ClientData.ClientId == Convert.ToInt64(ddlClient.SelectedValue) && DBData.ClientMaster.ClientName.Contains(txtMasterSearch.Text.Trim())
                                              && ClientData.DepartmentId == Convert.ToInt64(drpBusiness.SelectedValue) && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2)
                                              select new
                                              {
                                                  InvoiceID = DBData.InvoiceId,
                                                  //InvoiceNo = DBData.InvoiceNumber,
                                                  InvoiceFor = DBData.InvoiceFor, //// Country added by Jignesh for display
                                                  InvoiceNum = DBData.InvoiceNumber.Substring(0, DBData.InvoiceNumber.LastIndexOf("/") + 1),
                                                  ////InvSeq = DBData.Revision == null ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision,
                                                  ////Changes by Jignesh on 25-Apr-2018 for India format "01" and USA format "001"
                                                  InvSeq = (DBData.Revision == null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : (DBData.Revision != null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision : (DBData.Revision == null && DBData.InvoiceFor == "INDIA") ? DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') + DBData.Revision,
                                                  ClientName = ClientData.ClientName,
                                                  ProInvoiceNum = DBData.ProformaInvoiceMaster.InvoiceNumber,
                                                  MainProInvoiceID = (int?)DBData.ProformaInvoiceMaster.ProInvoiceId,
                                                  //InvoiceAmount = "&#" + (CurrencyData.CurrencySymbol + ";").Replace(";;", ";") + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                                  //                                                                                        where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                  //                                                                                        select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                                  //                                                                                                                                                where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                  //                                                                                                                                                select InvData.TotalAmt.Value).Sum().ToString()),
                                                  InvoiceAmount = CurrencyData.CurrencyCode + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                                                                                      where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                                                                      select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                                                                                                                                              where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
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
                        //// 25-Aug-2020 By Jignesh
                        //// added : && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2)
                        var InvoiceData = from DBData in dbobj.InvoiceMasters
                                          join ClientData in dbobj.ClientMasters
                                          on DBData.ClientId equals ClientData.ClientId
                                          join CurrencyData in dbobj.CurrencyMasters
                                          on ClientData.CurrencyId equals CurrencyData.CurrencyId
                                          where ClientData.DepartmentId == Convert.ToInt64(drpBusiness.SelectedValue) && DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && DBData.ClientMaster.ClientName.Contains(txtMasterSearch.Text.Trim()) && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2)
                                          select new
                                          {
                                              InvoiceID = DBData.InvoiceId,
                                              //InvoiceNo = DBData.InvoiceNumber,
                                              InvoiceFor = DBData.InvoiceFor, //// Country added by Jignesh for display
                                              InvoiceNum = DBData.InvoiceNumber.Substring(0, DBData.InvoiceNumber.LastIndexOf("/") + 1),
                                              ////InvSeq = DBData.Revision == null ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision,
                                              ////Changes by Jignesh on 25-Apr-2018 for India format "01" and USA format "001"
                                              InvSeq = (DBData.Revision == null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : (DBData.Revision != null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision : (DBData.Revision == null && DBData.InvoiceFor == "INDIA") ? DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') + DBData.Revision,
                                              ClientName = ClientData.ClientName,
                                              ProInvoiceNum = DBData.ProformaInvoiceMaster.InvoiceNumber,
                                              MainProInvoiceID = (int?)DBData.ProformaInvoiceMaster.ProInvoiceId,
                                              //InvoiceAmount = "&#" + (CurrencyData.CurrencySymbol + ";").Replace(";;", ";") + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                              //                                                                                        where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                              //                                                                                        select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                              //                                                                                                                                                where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                              //                                                                                                                                                select InvData.TotalAmt.Value).Sum().ToString()),
                                              InvoiceAmount = CurrencyData.CurrencyCode + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                                                                                  where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                                                                  select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                                                                                                                                          where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
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
                        //// 25-Aug-2020 By Jignesh
                        //// added : && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2)
                        var InvoiceData = from DBData in dbobj.InvoiceMasters
                                          join ClientData in dbobj.ClientMasters
                                          on DBData.ClientId equals ClientData.ClientId
                                          join CurrencyData in dbobj.CurrencyMasters
                                          on ClientData.CurrencyId equals CurrencyData.CurrencyId
                                          where ClientData.DepartmentId == Convert.ToInt64(drpBusiness.SelectedValue) && DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && DBData.ClientMaster.ClientName.Contains(txtMasterSearch.Text.Trim())
                                                && ClientData.ClientId == Convert.ToInt64(ddlClient.SelectedValue) && (ClientData.C_M1 == Global.UserM1 || ClientData.C_M2 == Global.UserM2)
                                          select new
                                          {
                                              InvoiceID = DBData.InvoiceId,
                                              //InvoiceNo = DBData.InvoiceNumber,
                                              InvoiceFor = DBData.InvoiceFor, //// Country added by Jignesh for display
                                              InvoiceNum = DBData.InvoiceNumber.Substring(0, DBData.InvoiceNumber.LastIndexOf("/") + 1),
                                              ////InvSeq = DBData.Revision == null ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision,
                                              ////Changes by Jignesh on 25-Apr-2018 for India format "01" and USA format "001"
                                              InvSeq = (DBData.Revision == null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') : (DBData.Revision != null && DBData.InvoiceFor == "USA") ? DBData.InvoiceSeqNo.ToString().PadLeft(3, '0') + DBData.Revision : (DBData.Revision == null && DBData.InvoiceFor == "INDIA") ? DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') : DBData.InvoiceSeqNo.ToString().PadLeft(2, '0') + DBData.Revision,
                                              ClientName = ClientData.ClientName,
                                              ProInvoiceNum = DBData.ProformaInvoiceMaster.InvoiceNumber,
                                              MainProInvoiceID = (int?)DBData.ProformaInvoiceMaster.ProInvoiceId,
                                              //InvoiceAmount = "&#" + (CurrencyData.CurrencySymbol + ";").Replace(";;", ";") + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                              //                                                                                        where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                              //                                                                                        select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                              //                                                                                                                                                where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                              //                                                                                                                                                select InvData.TotalAmt.Value).Sum().ToString()),
                                              InvoiceAmount = CurrencyData.CurrencyCode + " " + ((from InvData in dbobj.InvoiceDetailsMasters
                                                                                                  where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
                                                                                                  select InvData.TotalAmt.Value).Count() == 0 ? "0.00" : (from InvData in dbobj.InvoiceDetailsMasters
                                                                                                                                                          where InvData.InvoiceId == Convert.ToInt64(DBData.InvoiceId)
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
        }
    }

    protected void drpBusiness_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillClient();
        FillGrid();
        if (ddlClient.SelectedIndex == 0 && drpInvoiceStatus.Text == "Unpaid")
        {
            ////---gvInv.Columns[10].Visible = false;
            gvInv.Columns[11].Visible = false;
            btnIntimate.Visible = false;
        }
        else
        {
            if (gvInv.Rows[0].Cells[0].Text != "No Record to Display" && drpInvoiceStatus.Text == "Unpaid" && Global.UserType == "DIRECTOR" && drpBusiness.SelectedIndex != 0)
            {
                btnIntimate.Visible = true;
                btnSendEmail.Visible = false;
                ////---gvInv.Columns[10].Visible = true;
                gvInv.Columns[11].Visible = true;
            }
            else if (gvInv.Rows[0].Cells[0].Text != "No Record to Display" && drpInvoiceStatus.Text == "Draft" && Global.UserType != "DIRECTOR" && ddlClient.SelectedIndex != 0)
            {
                ////---gvInv.Columns[10].Visible = true;
                gvInv.Columns[11].Visible = true;
                btnSendEmail.Visible = true;
                btnIntimate.Visible = false;
                btnSendEmail.Enabled = false;
            }
            else
            {
                ////---gvInv.Columns[10].Visible = false;
                gvInv.Columns[11].Visible = false;
                btnIntimate.Visible = false;
                btnSendEmail.Visible = false;
            }
        }
        ViewState["SendI"] = "";
        //lbltest.Text = ViewState["SendI"].ToString();
        //lbltest.Visible = true;
    }

    protected void drpInvoiceStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        TextBox txtMasterSearch = (TextBox)this.Master.FindControl("txtMasterSearch");
        txtMasterSearch.Text = "";
        FillGrid();
        if (ddlClient.SelectedIndex == 0 && drpInvoiceStatus.Text == "Unpaid")
        {
            ////---gvInv.Columns[10].Visible = false;
            gvInv.Columns[11].Visible = false;
            btnIntimate.Visible = false;
        }
        else
        {
            if (gvInv.Rows[0].Cells[0].Text != "No Record to Display" && drpInvoiceStatus.Text == "Unpaid" && Global.UserType == "DIRECTOR" && drpBusiness.SelectedIndex != 0)
            {
                ////---gvInv.Columns[10].Visible = true;
                gvInv.Columns[11].Visible = true;
                btnSendEmail.Visible = false;
                btnIntimate.Visible = true;
            }
            else if (gvInv.Rows[0].Cells[0].Text != "No Record to Display" && drpInvoiceStatus.Text == "Draft" && Global.UserType != "DIRECTOR" && ddlClient.SelectedIndex != 0)
            {
                ////---gvInv.Columns[10].Visible = true;
                gvInv.Columns[11].Visible = true;
                btnSendEmail.Visible = true;
                btnIntimate.Visible = false;
                btnSendEmail.Enabled = false;
            }
            else
            {
                btnIntimate.Visible = false;
                btnSendEmail.Visible = false;
                ////---gvInv.Columns[10].Visible = false;
                gvInv.Columns[11].Visible = false;
            }
        }
        ViewState["SendI"] = "";
        //lbltest.Text = ViewState["SendI"].ToString();
        //lbltest.Visible = true;
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
            ImageButton imgbtnApprove = (ImageButton)e.Row.FindControl("imgbtnApprove");
            ImageButton imgbtnUnlock = (ImageButton)e.Row.FindControl("imgbtnUnlock");
            Label lblIsApprove = (Label)e.Row.FindControl("lblApprovedBy");
            //imgbtnApprove.Visible = false;
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
                //if (drpInvoiceStatus.SelectedItem.ToString() != "Paid")
                //{
                //    if (lblIsApprove.Text != "")
                //        imgbtnUnlock.Visible = true;
                //}
                //if (drpInvoiceStatus.SelectedItem.ToString() == "Draft")
                //{
                //    if (lblIsApprove.Text == "")
                //        imgbtnApprove.Visible = true;
                //}

            }
            else
            {
                if (drpInvoiceStatus.SelectedItem.ToString() == "Draft")
                {
                    if (imgbtnDelete != null)
                        imgbtnDelete.Visible = true;
                    //if (lblIsApprove.Text != "")
                    //    imgbtnApprove.Visible = true;
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

            // Main ProformaInvoice Table Link
            HyperLink hyperlnkMainProInvoiceNo = (HyperLink)e.Row.FindControl("hyperlnkProInvNo");
            Label MainProInvoiceID = (Label)e.Row.FindControl("lblMainProInvoiceID");
            //if (MainProInvoiceID.Text != "")
                hyperlnkMainProInvoiceNo.NavigateUrl = "ProformaInvoice?InvID=" + Global.Encrypt(MainProInvoiceID.Text);
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
                var DelInvSeries = from DBDelData in dbobj.InvoiceMasters
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
                              // && dbUser.UserId == Convert.ToInt64(Global.UserId) 
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

    // protected void FillClient_Existing()
    // {
    //     ddlClient.Items.Clear();
    //     if (Global.UserType == "DIRECTOR" || Global.DepartmentName == "ACCT")
    //     {
    //         if (drpBusiness.SelectedIndex == 0)
    //         {
    //             var ClientData = from dbClient in dbobj.ClientMasters
    //                              orderby dbClient.ClientName
    //                              select new
    //                              {
    //                                  Id = dbClient.ClientId,
    //                                  CName = dbClient.ClientName
    //                              };
    //             if (ClientData.Count() > 0)
    //             {
    //                 ddlClient.DataSource = ClientData;
    //                 ddlClient.DataValueField = "Id";
    //                 ddlClient.DataTextField = "CName";
    //                 ddlClient.DataBind();
    //             }
    //         }
    //         else
    //         {
    //             var ClientData = from dbClient in dbobj.ClientMasters
    //                              where dbClient.DepartmentId == Convert.ToInt64(drpBusiness.SelectedValue)
    //                              orderby dbClient.ClientName
    //                              select new
    //                              {
    //                                  Id = dbClient.ClientId,
    //                                  CName = dbClient.ClientName
    //                              };
    //             if (ClientData.Count() > 0)
    //             {
    //                 ddlClient.DataSource = ClientData;
    //                 ddlClient.DataValueField = "Id";
    //                 ddlClient.DataTextField = "CName";
    //                 ddlClient.DataBind();
    //             }
    //         }
    //     }
    //     else
    //     {
    //         var ClientData = from dbClient in dbobj.ClientMasters
    //                          join dbDepartment in dbobj.DepartmentMasters
    //                          on dbClient.DepartmentId equals dbDepartment.DepartmentId
    //                          join dbUser in dbobj.UserMasters
    //                          on dbDepartment.UserId equals dbUser.UserId
    //                          where dbClient.DepartmentId == Convert.ToInt64(drpBusiness.SelectedValue)
    //                          // && dbUser.UserId == Convert.ToInt64(Global.UserId) 
    //                          orderby dbClient.ClientName
    //                          select new
    //                          {
    //                              Id = dbClient.ClientId,
    //                              CName = dbClient.ClientName
    //                          };
    //         if (ClientData.Count() > 0)
    //         {
    //             ddlClient.DataSource = ClientData;
    //             ddlClient.DataValueField = "Id";
    //             ddlClient.DataTextField = "CName";
    //             ddlClient.DataBind();
    //         }
    //     }
    //     ddlClient.Items.Insert(0, "--All--");
    // }

    protected void ddlClient_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
        if (ddlClient.SelectedIndex == 0 && drpInvoiceStatus.Text == "Unpaid")
        {
            ////---gvInv.Columns[10].Visible = false;
            gvInv.Columns[11].Visible = false;
            btnIntimate.Visible = false;
        }
        else
        {
            if (gvInv.Rows[0].Cells[0].Text != "No Record to Display" && drpInvoiceStatus.Text == "Unpaid" && (Global.UserType == "DIRECTOR" || Global.DepartmentName == "ACCT") && drpBusiness.SelectedIndex != 0)
            {
                ////---gvInv.Columns[10].Visible = true;
                gvInv.Columns[11].Visible = true;
                btnIntimate.Visible = true;
                btnSendEmail.Visible = false;
            }
            else if (gvInv.Rows[0].Cells[0].Text != "No Record to Display" && drpInvoiceStatus.Text == "Draft" && Global.UserType != "DIRECTOR" && ddlClient.SelectedIndex != 0)
            {
                ////---gvInv.Columns[10].Visible = true;
                gvInv.Columns[11].Visible = true;
                btnIntimate.Visible = false;
                btnSendEmail.Visible = true;
                btnSendEmail.Enabled = false;
            }
            else
            {
                ////---gvInv.Columns[10].Visible = false;
                gvInv.Columns[11].Visible = false;
                btnIntimate.Visible = false;
                btnSendEmail.Visible = false;
            }
        }
        ViewState["SendI"] = "";
        //lbltest.Text = ViewState["SendI"].ToString();
        //lbltest.Visible = true;
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
            //lbltest.Text = ViewState["SendI"].ToString();
            //lbltest.Visible = true;
        }
        if (chkSelectAll.Checked == false)
        {
            ViewState["SendI"] = "";
            //lbltest.Text = ViewState["SendI"].ToString();
            //lbltest.Visible = true;
            foreach (GridViewRow row in gvInv.Rows)
            {
                CheckBox chkClient = (CheckBox)row.FindControl("chkClient");
                chkClient.Checked = false;
            }
        }

        if (ViewState["SendI"] != null && ViewState["SendI"].ToString() != "" && ViewState["SendI"].ToString() != " ")
        {
            btnIntimate.Enabled = true;
            btnSendEmail.Enabled = true;
        }
        else
        {
            btnIntimate.Enabled = false;
            btnSendEmail.Enabled = false;
        }
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
            //lbltest.Text = ViewState["SendI"].ToString();
            //lbltest.Visible = true;
        }
        else
        {
            ViewState["SendI"] = ViewState["SendI"].ToString().Replace(lblIdInv.Text + ",", "");
            //lbltest.Text = ViewState["SendI"].ToString();
            //lbltest.Visible = true;
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
                    //lbltest.Text = ViewState["SendI"].ToString();
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
            btnSendEmail.Enabled = true;
        }
        else
        {
            btnIntimate.Enabled = false;
            btnSendEmail.Enabled = false;
        }

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

    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        Global.PrevUrl = this.Request.Url.ToString();
        Response.Redirect("SendInvoiceEmail?" + Global.Encrypt("i=" + ViewState["SendI"].ToString().Substring(0, ViewState["SendI"].ToString().Length - 1) + "&P=I"));
    }

    protected void imgbtnApprove_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton imgbtnApprove = (ImageButton)sender;
        GridViewRow gvr = (GridViewRow)imgbtnApprove.NamingContainer;
        Label lblInvID = (Label)gvr.FindControl("lblInvoiceID");
        if (lblInvID != null)
        {
            var InvData = from DBData in dbobj.InvoiceMasters
                          where DBData.InvoiceId == Convert.ToInt64(lblInvID.Text)
                          select DBData;
            if (InvData.Count() > 0)
            {
                InvData.Single().ApprovedBy = Convert.ToInt64(Global.UserId);
                InvData.Single().ApprovedDate = DateTime.Now;
                dbobj.SubmitChanges();
                dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                FillGrid();
            }
        }
    }

    protected void imgbtnUnlock_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton imgbtnUnlock = (ImageButton)sender;
        GridViewRow gvr = (GridViewRow)imgbtnUnlock.NamingContainer;
        Label lblInvID = (Label)gvr.FindControl("lblInvoiceID");
        if (lblInvID != null)
        {
            var InvData = from DBData in dbobj.InvoiceMasters
                          where DBData.InvoiceId == Convert.ToInt64(lblInvID.Text)
                          select DBData;
            if (InvData.Count() > 0)
            {
                InvData.Single().ApprovedBy = null;
                InvData.Single().InvoiceStatus = "Draft";
                dbobj.SubmitChanges();
                dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                FillGrid();
            }
        }
    }
}