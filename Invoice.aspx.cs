using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text;
using System.Web.Script.Services;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Services;

public partial class Invoice : System.Web.UI.Page
{
    /*************************************************************************/
    /*                       Form to Create/Update Invoice                   */
    /*************************************************************************/

    public static DataTable dtInvoice = new DataTable();
    MITInvoiceDataContext dbobj = new MITInvoiceDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        //Only Non-Administrator users are allowed to view this page
        if (Global.UserType != "ADMINISTRATOR")
        {
            if (!IsPostBack)
            {
                ViewState["InvID"] = null;
                ViewState["InvNO"] = null;
                ViewState["CreatedBy"] = "";
                FillBusinessUnit();

                if (Request.QueryString["InvID"] == null)
                {
                    txtInvoiceStart.Text = "01-" + DateTime.Now.AddMonths(-1).ToString("MMM-yyyy");
                    txtInvoiceEnd.Text = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month) + "-" + DateTime.Now.AddMonths(-1).ToString("MMM-yyyy");
                    //                    txtInvoice.Text = "01-" + DateTime.Now.ToString("MMM-yyyy");
                    txtInvoice.Text = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month) + "-" + DateTime.Now.AddMonths(-1).ToString("MMM-yyyy");
                    txtOrderDate.Text = txtInvoiceEnd.Text;
                    //txtOrderDate.Enabled = false;
                }
                else
                {
                    if (Request.QueryString["InvID"] != null && Request.QueryString["InvID"] != "")
                    {
                        string qs = Request.QueryString["InvID"].Replace(" ", "+");

                        ViewState["InvID"] = Convert.ToInt64(Global.Decrypt(qs));
                    }

                    FillInvoiceData();
                    FillPriceType();
                }

                txtInvoiceStart_CalenderExtenderPlus.MaximumDate = DateTime.Now;
                txtInvoiceEnd_CalendarExtenderPlus.MaximumDate = DateTime.Now;
                txtInvoiceEnd_CalendarExtenderPlus.MinimumDate = DateTime.ParseExact(txtInvoiceStart.Text, "dd-MMM-yyyy", null);
                txtInvoice_CalendarExtenderPlus.MaximumDate = DateTime.Now;
            }
            if (Global.UserType == "DIRECTOR" || Global.DepartmentName == "ACCT")
            {
                btnBack.Visible = false;
                grdInvoiceData.Columns[7].Visible = false;
            }
        }
        else
        {
            Response.Redirect("Authorize");
        }
    }

    //Fill Invoice data from Database Entries
    private void FillInvoiceData()
    {
        //*** Change Global to ViewState["InvID"]

        var InvData = from DBData in dbobj.InvoiceMasters
                          //***where DBData.InvoiceId == Convert.ToInt64(Global.InvoiceID)
                      where DBData.InvoiceId == Convert.ToInt64(ViewState["InvID"])
                      select DBData;
        if (InvData.Count() > 0)
        {
            drpBU.SelectedValue = InvData.Single().ClientMaster.DepartmentId.ToString();
            FillClient();
            drpClient.SelectedValue = InvData.Single().ClientId.ToString();
            txtInvoice.Text = InvData.Single().InvoiceDate.ToString("dd-MMM-yyyy");
            txtInvoiceEnd.Text = InvData.Single().InvoiceEndDate.ToString("dd-MMM-yyyy");
            txtInvoiceStart.Text = InvData.Single().InvoiceStartDate.ToString("dd-MMM-yyyy");
            if (InvData.Single().InvoiceFor.ToString() == "USA")
                rdbUsa.Checked = true;
            else
                rdbIndia.Checked = true;
            FillClientContact();
            if (InvData.Single().ProjectFrom.ToString() != null && InvData.Single().ProjectFrom.ToString() != "0")
                drpProjectFrom.Items.FindByValue(InvData.Single().ProjectFrom.ToString()).Selected = true;
            txtOrderNo.Text = "";
            txtRemarks.Text = InvData.Single().Remarks;

            //*** Change Global to ViewState["InvID"]
            //Global.InvoiceNo = InvData.Single().InvoiceNumber;
            ViewState["InvNO"] = InvData.Single().InvoiceNumber;

            if (InvData.Single().PONumber != null)
            {
                txtOrderNo.Text = InvData.Single().PONumber;
                txtOrderDate.Text = InvData.Single().PODate.Value.ToString("dd-MMM-yyyy");
            }
            //else
            //    txtOrderDate.Enabled = false;
            btnSaveInvoiceInfo.Text = "Update";

            if (InvData.Single().InvoiceStatus != "Draft")
            {
                btnBack.Enabled = false;
                btnSendEmail.Enabled = false;

                grdInvoiceData.ShowFooter = false;
                grdInvoiceData.Columns[7].Visible = false;

            }
            if (InvData.Single().InvoiceStatus == "Unpaid" && Request.QueryString["InvID"] != "")
                btnRevised.Visible = true;
            else
                btnRevised.Visible = false;

            if (InvData.Single().InvoiceStatus == "Draft")
            {
                //if (InvData.Single().ApprovedBy != null && Global.UserIsSendEmail)
                //{
                //    btnSendEmail.Enabled = true;
                //    grdInvoiceData.ShowFooter = false;
                //    grdInvoiceData.Columns[7].Visible = false;
                //    btnBack.Enabled = false;
                //}
                //else
                //{
                //    btnSendEmail.Enabled = false;
                //    grdInvoiceData.ShowFooter = true;
                //    grdInvoiceData.Columns[7].Visible = true;
                //    btnBack.Enabled = true;
                //}

                if (Global.UserIsSendEmail)
                {
                    btnSendEmail.Enabled = true;
                    // grdInvoiceData.ShowFooter = false;
                    // grdInvoiceData.Columns[7].Visible = false;
                    // btnBack.Enabled = false;

                }
                else
                {
                    btnSendEmail.Enabled = false;
                    // grdInvoiceData.ShowFooter = true;
                    // grdInvoiceData.Columns[7].Visible = true;
                    // btnBack.Enabled = true;

                }

                grdInvoiceData.ShowFooter = true;
                grdInvoiceData.Columns[7].Visible = true;

            }

            drpBU.Enabled = false;
            ViewState["CreatedBy"] = InvData.Single().CreatedBy == null ? "" : (InvData.Single().CreatedBy).ToString();
            FillGrid();

            if (InvData.Single().CreatedBy != Convert.ToInt64(Global.UserId))
            {
                grdInvoiceData.FooterRow.Visible = false;
            }
            multiViewInvoice.ActiveViewIndex = 1;
        }
    }

    public static byte[] StreamToByte(Stream input)
    {
        byte[] buffer = new byte[16 * 1024];
        using (MemoryStream ms = new MemoryStream())
        {
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                ms.Write(buffer, 0, read);
            }
            return ms.ToArray();
        }
    }

    //Export (Download) Invoice to XLS or PDF format
    private void ExportFile(MemoryStream stm, string fileName)
    {
        stm.Position = 0;
        Response.Clear();
        Response.ContentType = "application/force-download";
        Response.AddHeader("content-disposition", "attachment;    filename=" + fileName);
        Response.BinaryWrite(stm.ToArray());
        Response.Flush();
        stm.Close();
        Response.End();
    }

    //Module to display default text for blank gridview
    private void BlankGrid()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("InvoiceDetailsId"));
        dt.Columns.Add(new DataColumn("ItemDesc"));
        dt.Columns.Add(new DataColumn("PriceType"));
        dt.Columns.Add(new DataColumn("Qty"));
        dt.Columns.Add(new DataColumn("UnitPrice"));
        dt.Columns.Add(new DataColumn("TotalAmt"));

        grdInvoiceData.DataSource = dt;
        dt.Rows.Add(dt.NewRow());
        grdInvoiceData.DataBind();
        int TotalCols = grdInvoiceData.Rows[0].Cells.Count;
        grdInvoiceData.Rows[0].Cells.Clear();
        grdInvoiceData.Rows[0].Cells.Add(new TableCell());
        grdInvoiceData.Rows[0].Cells[0].ColumnSpan = TotalCols;
        grdInvoiceData.Rows[0].Cells[0].Text = "No Record to Display";
    }

    //Module to fill Grid with Invoice line item entries
    private void FillGrid()
    {
        // Get Invoice Data
        lblClientName.Text = drpClient.SelectedItem.Text;
        lblInvoiceDate.Text = txtInvoice.Text;

        //*** Change Global to ViewState["InvID"]
        //lblInvoiceNo.Text = Global.InvoiceNo;

        string iNo = ViewState["InvNO"].ToString();
        char[] ciNo = iNo.ToCharArray();
        if (char.IsLetter(ciNo[ciNo.Length - 1]))
            lblInvoiceNo.Text = iNo.Substring(0, iNo.Length - 1) + " Revised";
        else
            lblInvoiceNo.Text = iNo;

        lblInvoicePeriod.Text = (Convert.ToDateTime(txtInvoiceStart.Text).ToString("MMM-yyyy") == Convert.ToDateTime(txtInvoiceEnd.Text).ToString("MMM-yyyy")) ? Convert.ToDateTime(txtInvoiceStart.Text).ToString("MMM-yyyy") : Convert.ToDateTime(txtInvoiceStart.Text).ToString("MMM") + "-" + Convert.ToDateTime(txtInvoiceEnd.Text).ToString("MMM-yyyy");
        lblOrderDate.Text = txtOrderNo.Text != "" ? Convert.ToDateTime(txtOrderDate.Text).ToString("dd-MMM-yyyy") : string.Empty;
        lblOrderNo.Text = txtOrderNo.Text;
        lblProjectFrom.Text = drpProjectFrom.Items.Count > 0 ? (drpProjectFrom.SelectedIndex > 0 ? drpProjectFrom.SelectedItem.Text : string.Empty) : string.Empty;

        string ClientCurrency = string.Empty;
        var CurrData = from DBData in dbobj.CurrencyMasters
                       join
                       ClientData in dbobj.ClientMasters
                       on
                       DBData.CurrencyId equals ClientData.CurrencyId
                       join
                       InvoiceData in dbobj.InvoiceMasters
                       on
                       ClientData.ClientId equals InvoiceData.ClientId
                       //where InvoiceData.InvoiceId == Convert.ToInt64(Global.InvoiceID)
                       where InvoiceData.InvoiceId == Convert.ToInt64(ViewState["InvID"])
                       select DBData;
        if (CurrData.Count() > 0)
        {
            //ClientCurrency = "&#" + (CurrData.Single().CurrencySymbol + ";").Replace(";;", ";");
            ClientCurrency = CurrData.Single().CurrencyCode;
        }
        //
        lblTotalInvoiceAmount.Text = ClientCurrency + " " + Math.Round(Convert.ToDecimal(((from DBData in dbobj.InvoiceDetailsMasters
                                                                                               //where DBData.InvoiceId == Convert.ToInt64(Global.InvoiceID)
                                                                                           where DBData.InvoiceId == Convert.ToInt64(ViewState["InvID"])
                                                                                           select DBData.TotalAmt.Value).Count() == 0 ? "0.00" : Convert.ToString(((from DBData in dbobj.InvoiceDetailsMasters
                                                                                                                                                                        //where DBData.InvoiceId == Convert.ToInt64(Global.InvoiceID)
                                                                                                                                                                    where DBData.InvoiceId == Convert.ToInt64(ViewState["InvID"])

                                                                                                                                                                    select DBData.TotalAmt).Sum())))), 2, MidpointRounding.ToEven);
        /*
        string value = lblTotalInvoiceAmount.Text.Replace(ClientCurrency, "");

        decimal dVal = Convert.ToDecimal(value.ToString());
        //-//value.ToString("C", CultureInfo.CurrentCulture)
        //-//LabelTest.Text = dVal.ToString("C", System.Globalization.CultureInfo.CurrentCulture);
        //-//LabelTest.Text = String.Format("{0:##,###.##}", dVal.ToString());
        LabelTest.Text = dVal.ToString("#,#.00", System.Globalization.CultureInfo.CurrentCulture);
        */

        // Get Invoice Line Item Data
        var InvData = from DBData in dbobj.InvoiceDetailsMasters
                          //where DBData.InvoiceId == Convert.ToInt64(Global.InvoiceID)
                      where DBData.InvoiceId == Convert.ToInt64(ViewState["InvID"])
                      select DBData;

        if (InvData.Count() > 0)
        {
            grdInvoiceData.DataSource = InvData;
            grdInvoiceData.DataBind();
        }
        else
            BlankGrid();
    }

    //private void Old_FillPriceType()
    //{
    //    var PriceData = from DBData in dbobj.PriceTypeMasters
    //                    select DBData;

    //    DropDownList drpPriceType = (DropDownList)grdInvoiceData.FooterRow.FindControl("drpAddPriceType");
    //    if (drpPriceType != null)
    //    {
    //        drpPriceType.DataSource = PriceData;
    //        drpPriceType.DataTextField = "PriceType";
    //        drpPriceType.DataValueField = "PriceTypeID";
    //        drpPriceType.DataBind();
    //    }
    //}

    //Fill Business Unit for User's alloted business unit

    private void FillPriceType()
    {
        DropDownList drpPriceType = (DropDownList)grdInvoiceData.FooterRow.FindControl("drpAddPriceType");

        var PriceData = from DBData in dbobj.PriceTypeNews
                        where DBData.IsDeleted == false
                        orderby DBData.PriceType
                        select new
                        {
                            PriceTypeId = DBData.PriceTypeId,
                            PriceType = DBData.PriceType
                        };



        if (PriceData.Count() > 0)
        {
            drpPriceType.DataSource = PriceData;
            drpPriceType.DataTextField = "PriceType";
            drpPriceType.DataValueField = "PriceTypeID";
            drpPriceType.DataBind();
        }
        drpPriceType.Items.Insert(0, "-- Select --");

    }

    private void FillBusinessUnit()
    {
        if (Global.UserType == "DIRECTOR" || Global.DepartmentName == "ACCT")
        {
            var BUData = from DBData in dbobj.DepartmentMasters
                         where DBData.IsActive
                         //&& DBData.UserId.Value == 1 //it will replace with session["UserID"]
                         select new
                         {
                             BUName = DBData.DepartmentName,
                             BUID = DBData.DepartmentId
                         };

            drpBU.DataSource = BUData;
            drpBU.DataTextField = "BUName";
            drpBU.DataValueField = "BUID";
            drpBU.DataBind();

            if (BUData.Count() > 0)
            {
                drpClient.SelectedIndex = 0;
                FillClient();
            }
        }
        else
        {
            var BUData = from DBData in dbobj.DepartmentMasters
                         where DBData.IsActive && (DBData.UserId == Convert.ToInt64(Global.UserId) || DBData.DepartmentId == Convert.ToInt64(Global.Department))
                         //&& DBData.UserId.Value == 1 //it will replace with session["UserID"]
                         select new
                         {
                             BUName = DBData.DepartmentName,
                             BUID = DBData.DepartmentId
                         };

            drpBU.DataSource = BUData;
            drpBU.DataTextField = "BUName";
            drpBU.DataValueField = "BUID";
            drpBU.DataBind();

            if (BUData.Count() > 0)
            {
                drpClient.SelectedIndex = 0;
                FillClient();
            }
        }
    }


    //Fill Client related to selected business unit
    private void FillClient()
    {
        // if (Global.UserM1 && Global.UserM2)
        // {
        var ClientData = from DBData in dbobj.ClientMasters
                         join
                         DeptData in dbobj.DepartmentMasters
                         on
                         DBData.DepartmentId equals DeptData.DepartmentId
                         where DeptData.DepartmentId == Convert.ToInt64(drpBU.SelectedValue) && (DBData.C_M1 == Global.UserM1 || DBData.C_M2 == Global.UserM2) && DBData.IsActive
                         orderby DBData.ClientName ascending
                         select new
                         {
                             ClientName = DBData.ClientName,
                             ClientID = DBData.ClientId
                         };
        drpClient.DataSource = ClientData;
        drpClient.DataTextField = "ClientName";
        drpClient.DataValueField = "ClientID";

        drpClient.DataBind();
        if (drpClient.Items.Count > 1)
        {
            drpClient.Items.Insert(0, "-- Select --");
            drpClient.SelectedIndex = 0;
        }
        // }
        // else if (Global.UserM1 && !Global.UserM2)
        // {
        //     var ClientData = from DBData in dbobj.ClientMasters
        //                      join
        //                      DeptData in dbobj.DepartmentMasters
        //                      on
        //                      DBData.DepartmentId equals DeptData.DepartmentId
        //                      where DeptData.DepartmentId == Convert.ToInt64(drpBU.SelectedValue) && (DBData.C_M1 == Global.UserM1 && DBData.C_M2 == Global.UserM2)
        //                      &&
        //                      DBData.IsActive
        //                      select new
        //                      {
        //                          ClientName = DBData.ClientName,
        //                          ClientID = DBData.ClientId
        //                      };
        //     drpClient.DataSource = ClientData;
        //     drpClient.DataTextField = "ClientName";
        //     drpClient.DataValueField = "ClientID";
        //     drpClient.DataBind();
        //     if (drpClient.Items.Count > 1)
        //     {
        //         drpClient.Items.Insert(0, "-- Select --");
        //         drpClient.SelectedIndex = 0;
        //     }
        // }
        FillClientContact();
    }


    //Fill Client related to selected business unit
    // private void FillClient_Existing()
    // {
    //     var ClientData = from DBData in dbobj.ClientMasters
    //                      join
    //                      DeptData in dbobj.DepartmentMasters
    //                      on
    //                      DBData.DepartmentId equals DeptData.DepartmentId
    //                      where DeptData.DepartmentId == Convert.ToInt64(drpBU.SelectedValue)
    //                      &&
    //                      DBData.IsActive
    //                      select new
    //                      {
    //                          ClientName = DBData.ClientName,
    //                          ClientID = DBData.ClientId
    //                      };
    //     drpClient.DataSource = ClientData;
    //     drpClient.DataTextField = "ClientName";
    //     drpClient.DataValueField = "ClientID";
    //     drpClient.DataBind();
    //     if (drpClient.Items.Count > 1)
    //     {
    //         drpClient.Items.Insert(0, "-- Select --");
    //         drpClient.SelectedIndex = 0;
    //     }
    //     FillClientContact();
    // }

    //Fill client contacts related to selected client


    private void FillClientContact()
    {
        if (drpClient.Items.Count > 0)
        {
            if (drpClient.SelectedItem.ToString() != "-- Select --")
            {
                var ContactData = from DBData in dbobj.ClientContactMasters
                                  where DBData.ClientId == Convert.ToInt64(drpClient.SelectedValue) && !DBData.IsDeleted
                                  select new
                                  {
                                      ContactID = DBData.ClientContactId,
                                      ContactName = DBData.Name
                                  };
                drpProjectFrom.DataSource = ContactData;
                drpProjectFrom.DataTextField = "ContactName";
                drpProjectFrom.DataValueField = "ContactID";
                drpProjectFrom.DataBind();
            }
            else
                drpProjectFrom.Items.Clear();

            drpProjectFrom.Items.Insert(0, "-- Select --");
        }
    }


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
                       on
                       DBData.ClientId equals ClientData.ClientId
                       orderby DBData.InvoiceSeqNo descending
                       where DBData.InvoiceNumber.Contains(FY) && ClientData.DepartmentId == iDeptID && !DBData.IsDeleted.Value && 
                       DBData.InvoiceFor == "USA"
                       select DBData);
        if (InvData.Count() > 0)
        {
            sSeqUSA_Number = (InvData.First().InvoiceSeqNo + 1).ToString("000");
        }


        return sSeqUSA_Number;
    }

    //// Generate Invoice Number only for India
    private string GetInvoiceNumber_INDIA(string FY, string dName, string seqNumber)
    {
        string sNumber = string.Empty;
        string departmentName = string.Empty;
        //= "MIT/" + financialYear + "/" + sDept + "/" + seqNo;
        departmentName = drpBU.SelectedItem.Text.Substring(0, 2);

        sNumber = "MITPL" + FY + departmentName + "/" + seqNumber;

        //if (dName.Length > 4)
        //    departmentName = drpBU.SelectedItem.Text.Substring(0, 4);
        //else
        //    departmentName = drpBU.SelectedItem.Text;

        ////sNumber = "MIT/" + FY + "/" + departmentName + "/" + seqNumber;


        return sNumber;
    }

    private string GetInvoiceNumber_USA(string FY, string dName, string seqNumber)
    {
        string sNumber = string.Empty;

        sNumber = "MIT/" + FY + "/" + dName + "/" + seqNumber;

        return sNumber;
    }

    //Save invoiceinfo and generate new invoice number
    protected void btnSaveInvoiceInfo_Click(object sender, EventArgs e)
    {
        Page page = HttpContext.Current.Handler as Page;
        if (txtOrderDate.Text == "" && txtOrderNo.Text.Trim() != "")
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), "Email Error", "alert('Order Date cannot be blank');", true);

        }
        else
        {
            if (drpClient.SelectedItem.ToString() == "-- Select --")
            {
                ScriptManager.RegisterStartupScript(page, page.GetType(), "MITPL Invoice", "alert('Please select client');", true);
            }
            else
            {
                if (txtOrderNo.Text.Trim() != "")
                {
                    string strOrdNo = txtOrderNo.Text.Trim().ToLower();
                    var ordData = from DBData in dbobj.InvoiceMasters
                                  where !DBData.IsDeleted.Value && DBData.PONumber.ToLower() == strOrdNo && DBData.ClientId == Convert.ToInt64(drpClient.SelectedValue) && DBData.InvoiceEndDate.Date == Convert.ToDateTime(txtInvoiceEnd.Text).Date && DBData.InvoiceStartDate.Date == Convert.ToDateTime(txtInvoiceStart.Text).Date
                                  select DBData;

                    if (ordData.Count() > 0)
                    {
                        if (btnSaveInvoiceInfo.Text == "Update")
                        {
                            /***********************************************************************/
                            /* These two lines are added to allow update invoice with revision     */
                            /***********************************************************************/
                            var currInvDetail = ordData.Where(y => y.InvoiceId == Convert.ToInt64(ViewState["InvID"]));
                            var Odata = ordData.Select(y => y.InvoiceId != Convert.ToInt64(ViewState["InvID"]) && y.InvoiceSeqNo != currInvDetail.SingleOrDefault().InvoiceSeqNo);
                            //var Odata = ordData.Select(y => y.InvoiceId != Convert.ToInt64(ViewState["InvID"]));
                            /**********************************************************************/

                            if (Odata.AsEnumerable().ElementAt(0).ToString() != "False")
                            {
                                ScriptManager.RegisterStartupScript(page, page.GetType(), "MITPL Invoice", "alert('Cannot generate invoice for same PO# with same client and same invoice period');", true);
                                return;
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(page, page.GetType(), "MITPL Invoice", "alert('Cannot generate invoice for same PO# with same client and same invoice period');", true);
                            return;
                        }
                    }
                }

                if (btnSaveInvoiceInfo.Text == "Save")
                {
                    #region Insert Data
                    InvoiceMaster inv = new InvoiceMaster();

                    inv.ClientId = Convert.ToInt64(drpClient.SelectedValue);
                    inv.InvoiceDate = Convert.ToDateTime(txtInvoice.Text);
                    inv.InvoiceStartDate = Convert.ToDateTime(txtInvoiceStart.Text);
                    inv.InvoiceEndDate = Convert.ToDateTime(txtInvoiceEnd.Text);

                    string financialYear = string.Empty;
                    int CurrYear = Convert.ToDateTime(txtInvoice.Text).Year;

                    if (rdbUsa.Checked == true)
                    {
                        inv.InvoiceFor = "USA";
                        if (inv.InvoiceDate <= Convert.ToDateTime("03/31/" + CurrYear))
                            financialYear = (CurrYear - 1) + "-" + CurrYear.ToString().Substring(2, 2);
                        else
                            financialYear = CurrYear + "-" + (CurrYear + 1).ToString().Substring(2, 2);
                    }
                    else
                    {
                        inv.InvoiceFor = "INDIA";
                        if (inv.InvoiceDate <= Convert.ToDateTime("03/31/" + CurrYear))
                            financialYear = (CurrYear - 1).ToString().Substring(2, 2) + "-" + CurrYear.ToString().Substring(2, 2);
                        else
                            financialYear = CurrYear.ToString().Substring(2, 2) + "-" + (CurrYear + 1).ToString().Substring(2, 2);
                        
                    }

                    if (txtOrderNo.Text.Trim() != "")
                    {
                        inv.PONumber = txtOrderNo.Text;
                        inv.PODate = Convert.ToDateTime(txtOrderDate.Text);
                    }

                    /* These are the function to return Sequence number for USA and India  */
                    /* For USA number format '001' and India number format '01'  */
                    /* Code updated by Jignesh on 13-Apr-2018  */

                    string seqNo = string.Empty;
                    if (rdbUsa.Checked == true)
                        seqNo = GetSequanceUSA(financialYear, Convert.ToInt64(drpBU.SelectedValue));
                    else
                        seqNo = GetSequanceINDIA(financialYear, Convert.ToInt64(drpBU.SelectedValue));

                    
                    inv.InvoiceSeqNo = Convert.ToInt64(seqNo);

                    if (rdbUsa.Checked)
                        //inv.InvoiceNumber = GetInvoiceNumber_USA "MIT/" + financialYear + "/" + drpBU.SelectedItem.Text + "/" + seqNo;
                        inv.InvoiceNumber = GetInvoiceNumber_USA(financialYear, drpBU.SelectedItem.Text, seqNo);
                    else
                        inv.InvoiceNumber = GetInvoiceNumber_INDIA(financialYear, drpBU.SelectedItem.Text, seqNo);

                    inv.InvoiceStatus = "Draft";
                    if (drpProjectFrom.SelectedIndex != 0)
                        inv.ProjectFrom = Convert.ToInt64(drpProjectFrom.SelectedValue);
                    else
                        inv.ProjectFrom = 0;
                    inv.IsDeleted = false;
                    inv.IsRevised = false;
                    inv.Remarks = txtRemarks.Text.Trim();
                    inv.CreatedBy = Convert.ToInt64(Global.UserId);
                    inv.CreatedDate = DateTime.Now;
                    inv.ModifyDate = DateTime.Now;
                    inv.ModifyBy = Convert.ToInt64(Global.UserId);

                    ////// 1
                    dbobj.InvoiceMasters.InsertOnSubmit(inv);
                    dbobj.SubmitChanges();

                    //*** Change Globle to Response.Redirect
                    //Global.InvoiceID = inv.InvoiceId;
                    //Global.InvoiceNo = inv.InvoiceNumber;
                    ViewState["InvID"] = inv.InvoiceId;
                    ViewState["InvNO"] = inv.InvoiceNumber;
                    btnSaveInvoiceInfo.Text = "Update";

                    #endregion
                }
                else
                {
                    #region Update Data
                    //// Remain update code for country wise code
                    ////
                    var invoiceData = from DBData in dbobj.InvoiceMasters
                                          //where DBData.InvoiceId == Global.InvoiceID
                                      where DBData.InvoiceId == Convert.ToInt64(ViewState["InvID"])
                                      select DBData;
                    if (invoiceData.Count() > 0)
                    {
                        invoiceData.Single().ClientId = Convert.ToInt64(drpClient.SelectedValue);
                        invoiceData.Single().InvoiceDate = Convert.ToDateTime(txtInvoice.Text);
                        invoiceData.Single().InvoiceStartDate = Convert.ToDateTime(txtInvoiceStart.Text);
                        invoiceData.Single().InvoiceEndDate = Convert.ToDateTime(txtInvoiceEnd.Text);

                        string financialYearUp = string.Empty;
                        int CurrYearUp = Convert.ToDateTime(txtInvoice.Text).Year;
                        if (rdbUsa.Checked == true)
                        {
                            invoiceData.Single().InvoiceFor = "USA";
                            if (invoiceData.Single().InvoiceDate <= Convert.ToDateTime("03/31/" + CurrYearUp))
                                financialYearUp = (CurrYearUp - 1) + "-" + CurrYearUp.ToString().Substring(2, 2);
                            else
                                financialYearUp = CurrYearUp + "-" + (CurrYearUp + 1).ToString().Substring(2, 2);

                        }
                        else
                        {
                            invoiceData.Single().InvoiceFor = "INDIA";

                            if (invoiceData.Single().InvoiceDate <= Convert.ToDateTime("03/31/" + CurrYearUp))
                                financialYearUp = (CurrYearUp - 1).ToString().Substring(2, 2) + "-" + CurrYearUp.ToString().Substring(2, 2);
                            else
                                financialYearUp = CurrYearUp.ToString().Substring(2, 2) + "-" + (CurrYearUp + 1).ToString().Substring(2, 2);
                        }

                        if (txtOrderNo.Text.Trim() != "")
                        {
                            invoiceData.Single().PONumber = txtOrderNo.Text;
                            invoiceData.Single().PODate = Convert.ToDateTime(txtOrderDate.Text);
                        }
                        else
                        {
                            invoiceData.Single().PONumber = null;
                            invoiceData.Single().PODate = null;
                        }
                        #region for update Invoice Number

                        /* These are the function to return Sequence number for USA and India  */
                        /* For USA number format '001' and India number format '01'  */
                        /* Code updated by Jignesh on 13-Apr-2018  */

                        string seqNoUp = string.Empty;
                        ////if (rdbUsa.Checked == true)
                        ////    seqNoUp = GetSequanceUSA(financialYearUp, Convert.ToInt64(drpBU.SelectedValue));
                        ////else
                        ////    seqNoUp = GetSequanceINDIA(financialYearUp, Convert.ToInt64(drpBU.SelectedValue));

                        //// For update invoice number set format if revise invoice to set format country wise
                        if (invoiceData.Single().Revision != null)
                        {
                            if (rdbUsa.Checked)
                                //invoiceData.Single().InvoiceNumber = "MIT/" + financialYearUp + "/" + drpBU.SelectedItem.Text + "/" + invoiceData.Single().InvoiceSeqNo.ToString("000") + invoiceData.Single().Revision;
                                invoiceData.Single().InvoiceNumber = GetInvoiceNumber_USA(financialYearUp, drpBU.SelectedItem.Text, invoiceData.Single().InvoiceSeqNo.ToString("000")) + invoiceData.Single().Revision;
                            else
                                invoiceData.Single().InvoiceNumber = GetInvoiceNumber_INDIA(financialYearUp, drpBU.SelectedItem.Text, invoiceData.Single().InvoiceSeqNo.ToString("00")) + invoiceData.Single().Revision;
                        }
                        else
                        {

                            if (rdbUsa.Checked)
                                invoiceData.Single().InvoiceNumber = GetInvoiceNumber_USA(financialYearUp, drpBU.SelectedItem.Text, invoiceData.Single().InvoiceSeqNo.ToString("000"));
                            else
                                invoiceData.Single().InvoiceNumber = GetInvoiceNumber_INDIA(financialYearUp, drpBU.SelectedItem.Text, invoiceData.Single().InvoiceSeqNo.ToString("00"));
                        }
                        #endregion


                        if (drpProjectFrom.SelectedIndex != 0)
                            invoiceData.Single().ProjectFrom = Convert.ToInt64(drpProjectFrom.SelectedValue);
                        else
                            invoiceData.Single().ProjectFrom = 0;
                        invoiceData.Single().Remarks = txtRemarks.Text.Trim();
                        invoiceData.Single().ModifyDate = DateTime.Now;
                        invoiceData.Single().ModifyBy = Convert.ToInt64(Global.UserId);

                        dbobj.SubmitChanges();
                    }
                    #endregion
                }
                dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);

                //FillGrid();
                //multiViewInvoice.ActiveViewIndex = 1;
                //Response.Redirect("NewInvoice?InvID=" + Global.Encrypt(Global.InvoiceID.ToString()));
                Response.Redirect("NewInvoice?InvID=" + Global.Encrypt(ViewState["InvID"].ToString()));
            }
        }
    }


    protected void drpBU_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillClient();
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        btnSaveInvoiceInfo.Text = "Update";
        multiViewInvoice.ActiveViewIndex = 0;
    }

    protected void txtAddUnitPrice_TextChanged(object sender, EventArgs e)
    {
        TextBox txtAddQty = (TextBox)grdInvoiceData.FooterRow.FindControl("txtAddQuantity");
        TextBox txtUnitPrice = (TextBox)grdInvoiceData.FooterRow.FindControl("txtAddUnitPrice");
        Label lblTotalAmount = (Label)grdInvoiceData.FooterRow.FindControl("lblAddAmount");

        if (txtAddQty.Text.Trim() != "" && txtUnitPrice.Text.Trim() != "")
            lblTotalAmount.Text = (Convert.ToDecimal(txtAddQty.Text) * Convert.ToDecimal(txtUnitPrice.Text)).ToString("0.00");
        else
            lblTotalAmount.Text = "0.00";
    }

    //Add invoice line item entry
    protected void imgbtnAdd_Click(object sender, ImageClickEventArgs e)
    {
        TextBox txtAServices = (TextBox)grdInvoiceData.FooterRow.FindControl("txtAddServices");
        TextBox txtAQty = (TextBox)grdInvoiceData.FooterRow.FindControl("txtAddQuantity");
        TextBox txtAUnitPrice = (TextBox)grdInvoiceData.FooterRow.FindControl("txtAddUnitPrice");
        DropDownList drpAPriceType = (DropDownList)grdInvoiceData.FooterRow.FindControl("drpAddPriceType");

        InvoiceDetailsMaster invDetail = new InvoiceDetailsMaster();

        //invDetail.InvoiceId = Convert.ToInt64(Session["InvoiceID"]);
        invDetail.InvoiceId = Convert.ToInt64(ViewState["InvID"]);
        invDetail.ItemDesc = txtAServices.Text.Trim();
        invDetail.PriceType = drpAPriceType.SelectedItem.Text.Trim().ToUpper();
        if (txtAQty.Text.Trim() != "")
        {
            invDetail.Qty = Convert.ToDecimal(txtAQty.Text);
            if (txtAUnitPrice.Text.Trim() != "")
            {
                invDetail.UnitPrice = Convert.ToDecimal(txtAUnitPrice.Text);
                invDetail.TotalAmt = Math.Round(Convert.ToDecimal(txtAQty.Text) * Convert.ToDecimal(txtAUnitPrice.Text), 2, MidpointRounding.AwayFromZero);
            }
            else
            {
                invDetail.UnitPrice = Convert.ToDecimal(0.00);
                invDetail.TotalAmt = Convert.ToDecimal(0.00);
            }

        }
        else
            invDetail.TotalAmt = Convert.ToDecimal(0.00);

        invDetail.CreatedBy = Convert.ToInt64(Global.UserId);
        invDetail.CreatedDate = DateTime.Now;
        dbobj.InvoiceDetailsMasters.InsertOnSubmit(invDetail);
        dbobj.SubmitChanges();
        dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);

        txtAUnitPrice.Text = "";
        txtAServices.Text = "";
        txtAQty.Text = "";

        FillGrid();
        FillPriceType();
    }

    protected void grdInvoiceData_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grdInvoiceData.EditIndex = -1;
        grdInvoiceData.ShowFooter = true;
        FillGrid();
    }

    protected void grdInvoiceData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.EmptyDataRow)
        {
            ImageButton imgbtnEdit = (ImageButton)e.Row.FindControl("imgbtnEdit");
            ImageButton imgbtnDelete = (ImageButton)e.Row.FindControl("imgbtnDelete");
            if (ViewState["CreatedBy"].ToString() != Global.UserId && ViewState["CreatedBy"].ToString() != "")
            {
                imgbtnEdit.Visible = false;
                imgbtnDelete.Visible = false;
            }
        }
    }

    protected void grdInvoiceData_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grdInvoiceData.EditIndex = e.NewEditIndex;
        grdInvoiceData.ShowFooter = false;
        grdInvoiceData.PagerSettings.Visible = false;
        FillGrid();

        Label lblEditPriceType = (Label)grdInvoiceData.Rows[e.NewEditIndex].FindControl("lblEditPriceType");

        DropDownList drpEditPriceType = (DropDownList)grdInvoiceData.Rows[e.NewEditIndex].FindControl("drpPriceType");

        var PriceEditData = from DBData in dbobj.PriceTypeNews
                            where DBData.IsDeleted == false
                            orderby DBData.PriceType
                            select new
                            {
                                PriceTypeId = DBData.PriceTypeId,
                                PriceType = DBData.PriceType
                            };



        if (PriceEditData.Count() > 0)
        {
            drpEditPriceType.DataSource = PriceEditData;
            drpEditPriceType.DataTextField = "PriceType";
            drpEditPriceType.DataValueField = "PriceTypeID";
            drpEditPriceType.DataBind();
        }

        drpEditPriceType.Items.Insert(0, "-- Select --");

        ListItem li = drpEditPriceType.Items.FindByText(lblEditPriceType.Text.Trim().ToUpper());
        if (li != null)
            drpEditPriceType.Items.FindByText(lblEditPriceType.Text.Trim().ToUpper()).Selected = true;

    }

    protected void grdInvoiceData_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow gvr = grdInvoiceData.Rows[e.RowIndex];
        Label lblItmID = (Label)gvr.FindControl("lblItemID");

        TextBox txtUDesc = (TextBox)gvr.FindControl("txtServices");
        TextBox txtUQty = (TextBox)gvr.FindControl("txtQuantity");
        TextBox txtUUnitPrice = (TextBox)gvr.FindControl("txtUnitPrice");

        DropDownList drpUPriceType = (DropDownList)gvr.FindControl("drpPriceType");


        var InvItem = from DBData in dbobj.InvoiceDetailsMasters
                      where DBData.InvoiceDetailsId == Convert.ToInt64(lblItmID.Text)
                      select DBData;
        if (InvItem.Count() > 0)
        {
            InvItem.Single().ItemDesc = txtUDesc.Text.Trim();
            InvItem.Single().PriceType = drpUPriceType.SelectedItem.Text.Trim().ToUpper().ToString();
            if (txtUQty.Text.Trim() != "")
            {

                InvItem.Single().Qty = Convert.ToDecimal(txtUQty.Text);
                if (txtUUnitPrice.Text.Trim() != "")
                {
                    InvItem.Single().UnitPrice = Convert.ToDecimal(txtUUnitPrice.Text);
                    InvItem.Single().TotalAmt = Math.Round(Convert.ToDecimal(txtUQty.Text) * Convert.ToDecimal(txtUUnitPrice.Text), 2, MidpointRounding.AwayFromZero);
                }
                else
                {
                    InvItem.Single().UnitPrice = 0;
                    InvItem.Single().TotalAmt = Convert.ToDecimal(0.00);
                }
            }
            else
            {
                InvItem.Single().Qty = null;
                InvItem.Single().UnitPrice = null;
                InvItem.Single().TotalAmt = Convert.ToDecimal(0.00);
            }
            InvItem.Single().ModifyBy = Convert.ToInt64(Global.UserId);
            InvItem.Single().Modifydate = DateTime.Now;
            dbobj.SubmitChanges();
            dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
        }
        grdInvoiceData.EditIndex = -1;
        grdInvoiceData.ShowFooter = true;
        FillGrid();
        FillPriceType();
    }

    //Export Invoice to XLS format (This module is replaced by GetInoice method of Global File)
    private void ExportToXLS()
    {
        //if (Global.InvoiceID != null)
        if (ViewState["InvID"] != null)
        {
            MemoryStream stm = new MemoryStream();
            int NextSRNo = 2;

            Aspose.Cells.License lic = new Aspose.Cells.License();
            string licPath = Server.MapPath("Bin");
            lic.SetLicense(licPath + @"\aspose.lic");

            string templatePath = Server.MapPath("InvoiceTemplate");


            var InvDetails = from DBData in dbobj.InvoiceMasters
                                 //where DBData.InvoiceId == Convert.ToInt64(Global.InvoiceID)
                             where DBData.InvoiceId == Convert.ToInt64(ViewState["InvID"])
                             select DBData;
            if (InvDetails.Count() > 0)
            {
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook(templatePath + @"\Invoice Template2.xls");
                Aspose.Cells.Worksheet ws = wb.Worksheets["Invoice"];

                ws.Cells[10, 11].Value = InvDetails.Single().InvoiceNumber;
                ws.Cells[11, 11].Value = InvDetails.Single().InvoiceDate.ToString("dd-MMM-yy");


                if (InvDetails.Single().PONumber != null)
                {
                    ws.Cells[26, 2].Value = InvDetails.Single().PONumber;
                    ws.Cells[26, 3].Value = InvDetails.Single().PODate;
                }

                //Add Client Details
                ws.Cells[26, 6].Value = lblProjectFrom.Text;
                var ClientData = from DBData in dbobj.ClientMasters
                                 join
                                 CurrencyData in dbobj.CurrencyMasters
                                 on
                                 DBData.CurrencyId equals CurrencyData.CurrencyId
                                 join
                                 CityData in dbobj.CityMasters
                                 on
                                 DBData.City1 equals CityData.CityId
                                 join
                                 StateData in dbobj.StateMasters
                                 on
                                 DBData.State1 equals StateData.StateId
                                 join
                                 CountyData in dbobj.CountryMasters
                                 on
                                 DBData.Country1 equals CountyData.CountryId
                                 where DBData.ClientId == InvDetails.Single().ClientId
                                 &&
                                 !CurrencyData.IsDeleted
                                 select new
                                 {
                                     ClientName = DBData.ClientName,
                                     Address = DBData.Address1 + Environment.NewLine + CityData.CityName + ", " + StateData.StateName + " " + DBData.Zip_Postal1,
                                     Country = CountyData.CountryName,
                                     CurrencyID = DBData.CurrencyId,
                                     CurrencyName = CurrencyData.CurrencyName,
                                     CurrencyCode = CurrencyData.CurrencyCode,
                                     CurrencySymbol = CurrencyData.CurrencySymbol
                                 };
                ws.Cells[20, 2].Value = ClientData.Single().ClientName;
                ws.Cells[21, 2].Value = ClientData.Single().Address;
                ws.Cells[22, 2].Value = ClientData.Single().Country;


                /***********************************************************************/
                /*          Update Currency                                            */
                /***********************************************************************/
                Aspose.Cells.Range rngAmountCurr = ws.Cells.CreateRange(31, 12, 26, 1);
                Aspose.Cells.Style styleAmountCurr = ws.Cells[31, 12].GetStyle();
                styleAmountCurr.Number = 1;
                styleAmountCurr.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.00";
                rngAmountCurr.SetStyle(styleAmountCurr);

                Aspose.Cells.Range rngAmountCurr2 = ws.Cells.CreateRange(30, 12, 1, 1);
                Aspose.Cells.Style styleAmountCurr2 = ws.Cells[30, 12].GetStyle();
                styleAmountCurr2.Number = 1;
                styleAmountCurr2.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.00";
                rngAmountCurr2.SetStyle(styleAmountCurr2);

                Aspose.Cells.Range rngAmountCurr3 = ws.Cells.CreateRange(57, 12, 1, 1);
                Aspose.Cells.Style styleAmountCurr3 = ws.Cells[57, 12].GetStyle();
                styleAmountCurr3.Number = 1;
                styleAmountCurr3.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.00";
                rngAmountCurr3.SetStyle(styleAmountCurr3);


                Aspose.Cells.Range rngUnitPriceCurr = ws.Cells.CreateRange(31, 10, 26, 1);
                Aspose.Cells.Style styleUnitPriceCurr = ws.Cells[31, 10].GetStyle();
                styleUnitPriceCurr.Number = 1;
                styleUnitPriceCurr.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.00";
                rngUnitPriceCurr.SetStyle(styleUnitPriceCurr);

                Aspose.Cells.Range rngUnitPriceCurr2 = ws.Cells.CreateRange(30, 10, 1, 1);
                Aspose.Cells.Style styleUnitPriceCurr2 = ws.Cells[30, 10].GetStyle();
                styleUnitPriceCurr2.Number = 1;
                styleUnitPriceCurr2.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.00";
                rngUnitPriceCurr2.SetStyle(styleUnitPriceCurr2);

                Aspose.Cells.Range rngUnitPriceCurr3 = ws.Cells.CreateRange(57, 10, 1, 1);
                Aspose.Cells.Style styleUnitPriceCurr3 = ws.Cells[57, 10].GetStyle();
                styleUnitPriceCurr3.Number = 1;
                styleUnitPriceCurr3.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.00";
                rngUnitPriceCurr3.SetStyle(styleUnitPriceCurr3);


                Aspose.Cells.Range rngTotalCurr = ws.Cells.CreateRange(59, 12, 1, 1);
                Aspose.Cells.Style styleTotalCurr = ws.Cells[59, 12].GetStyle();
                styleTotalCurr.Number = 1;
                styleTotalCurr.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.00";
                rngTotalCurr.SetStyle(styleTotalCurr);


                Aspose.Cells.Range rngPayAmtCurr = ws.Cells.CreateRange(65, 12, 1, 1);
                Aspose.Cells.Style stylePayAmtCurr = ws.Cells[65, 12].GetStyle();
                stylePayAmtCurr.Number = 1;
                stylePayAmtCurr.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.00";
                rngPayAmtCurr.SetStyle(stylePayAmtCurr);

                /***********************************************************************/



                int RecordCnt = 0;

                var InvData = from DBData in dbobj.InvoiceDetailsMasters
                                  //where DBData.InvoiceId == Convert.ToInt64(Global.InvoiceID)
                              where DBData.InvoiceId == Convert.ToInt64(ViewState["InvID"])
                              select DBData;
                if (InvData.Count() > 0)
                {

                    foreach (var inv in InvData)
                    {
                        if (ws.Cells[30, 3].Value == null)
                        {
                            ws.Cells[30, 2].Value = 1;
                            ws.Cells[30, 3].Value = inv.ItemDesc;

                            if (!(inv.Qty == 0 || inv.Qty == null))
                            {
                                ws.Cells[30, 8].Value = inv.Qty.Value;
                                ws.Cells[30, 10].Value = inv.UnitPrice.Value;
                                ws.Cells[30, 11].Value = inv.PriceType;
                            }

                            Aspose.Cells.Style desStyle = ws.Cells[30, 3].GetStyle();
                            desStyle.IsTextWrapped = true;
                            ws.Cells[30, 3].SetStyle(desStyle);

                            ws.Cells[30, 25].Value = inv.ItemDesc;

                            ws.AutoFitRow(30);
                            ws.Cells[30, 25].Value = string.Empty;
                            RecordCnt++;
                        }
                        else
                        {
                            //Insert New Row
                            if (30 + RecordCnt > 57)
                            {
                                ws.Cells.InsertRow(30 + RecordCnt);
                                ws.Cells.InsertRow(30 + RecordCnt);

                                ws.Cells[30 + RecordCnt - 1, 2].SetStyle(ws.Cells[30, 2].GetStyle());
                                ws.Cells[30 + RecordCnt, 2].SetStyle(ws.Cells[30, 2].GetStyle());

                                Aspose.Cells.Range rngDesc = ws.Cells.CreateRange(30 + RecordCnt, 3, 1, 5);
                                if (!ws.Cells[30 + RecordCnt, 3].IsMerged)
                                    rngDesc.Merge();

                                rngDesc = ws.Cells.CreateRange(30 + RecordCnt + 1, 3, 1, 5);
                                if (!ws.Cells[31 + RecordCnt, 3].IsMerged)
                                    rngDesc.Merge();

                                Aspose.Cells.Style styleQty = ws.Cells[30, 8].GetStyle();
                                Aspose.Cells.Range rngQty = ws.Cells.CreateRange(30 + RecordCnt, 8, 1, 2);
                                if (!ws.Cells[30 + RecordCnt, 8].IsMerged)
                                    rngQty.Merge();
                                styleQty.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.None;
                                styleQty.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.None;
                                styleQty.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                                styleQty.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                                rngQty.SetStyle(styleQty);



                                rngQty = ws.Cells.CreateRange(30 + RecordCnt + 1, 8, 1, 2);
                                if (!ws.Cells[31 + RecordCnt, 8].IsMerged)
                                    rngQty.Merge();
                                rngQty.SetStyle(styleQty);

                                Aspose.Cells.Range rngTotalAmount = ws.Cells.CreateRange(30 + RecordCnt, 12, 1, 2);
                                if (!ws.Cells[30 + RecordCnt, 12].IsMerged)
                                    rngTotalAmount.Merge();
                                Aspose.Cells.Style styleTotalAmount = ws.Cells[30, 12].GetStyle();
                                styleTotalAmount.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.None;
                                styleTotalAmount.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.None;
                                styleTotalAmount.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                                styleTotalAmount.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                                rngTotalAmount.SetStyle(styleTotalAmount);


                                rngTotalAmount = ws.Cells.CreateRange(30 + RecordCnt + 1, 12, 1, 2);
                                if (!ws.Cells[31 + RecordCnt, 12].IsMerged)
                                    rngTotalAmount.Merge();
                                rngTotalAmount.SetStyle(styleTotalAmount);


                                Aspose.Cells.Style styleUnitPrice = ws.Cells[30, 10].GetStyle();
                                styleUnitPrice.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.None;
                                styleUnitPrice.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.None;
                                styleUnitPrice.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                                styleUnitPrice.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                                ws.Cells[30 + RecordCnt, 10].SetStyle(styleUnitPrice);
                                ws.Cells[31 + RecordCnt, 10].SetStyle(styleUnitPrice);
                            }
                            //ws.Cells[31 + RecordCnt, 2].Value = ((RecordCnt / 2) + 2).ToString();
                            ws.Cells[31 + RecordCnt, 3].Value = inv.ItemDesc;

                            if (!(inv.Qty == 0 || inv.Qty == null))
                            {
                                ws.Cells[31 + RecordCnt, 2].Value = NextSRNo;
                                ws.Cells[31 + RecordCnt, 8].Value = inv.Qty.Value;
                                ws.Cells[31 + RecordCnt, 10].Value = inv.UnitPrice.Value;
                                ws.Cells[31 + RecordCnt, 11].Value = inv.PriceType;
                                NextSRNo++;
                            }


                            ws.Cells[31 + RecordCnt, 25].Value = inv.ItemDesc;
                            ws.Cells[31 + RecordCnt, 25].GetStyle().IsTextWrapped = true;
                            //ws.Cells[31 + RecordCnt, 12].Formula = "=IF(I" + (32 + RecordCnt) + "=\"\",\"\",IF(K" + (32 + RecordCnt) + "=\"\",\"\",ROUND(I" + (32 + RecordCnt) + "*K" + (32 + RecordCnt) + ",2)))";
                            Aspose.Cells.Style desStyle = ws.Cells[30 + RecordCnt, 3].GetStyle();
                            desStyle.IsTextWrapped = true;
                            ws.Cells[30 + RecordCnt, 3].SetStyle(desStyle);

                            ws.AutoFitRow(30 + RecordCnt);
                            ws.Cells[31 + RecordCnt, 25].Value = string.Empty;
                            RecordCnt++;
                            RecordCnt++;
                        }
                    }
                }


                //Aspose.Cells.Style styleTest = ws.Cells[6, 2].GetStyle();
                //styleTest.Number = 1;
                //string myEncodedString = HttpUtility.HtmlEncode("\u003b");
                //StringWriter myWriter = new StringWriter();
                //HttpUtility.HtmlDecode(myEncodedString, myWriter);
                //styleTest.Custom = "[$" + HttpUtility.HtmlDecode("&#xffe5;") + "] 0.00";



                //ws.Cells[6, 2].Value = 115.25;
                //ws.Cells[6, 2].SetStyle(styleTest);

                wb.CalculateFormula();

                wb.Save(stm, Aspose.Cells.SaveFormat.Excel97To2003);
                string fileName = InvDetails.Single().InvoiceNumber.Replace("/", "_") + ".xls";
                ExportFile(stm, fileName);
            }
        }
    }

    //Export Invoice to PDF format (This module is replaced by GetInoice method of Global File)
    private void ExportToPDF()
    {
        //if (Global.InvoiceID != null)
        if (ViewState["InvID"] != null)
        {
            MemoryStream stm = new MemoryStream();
            int NextSRNo = 2;

            Aspose.Cells.License lic = new Aspose.Cells.License();
            string licPath = Server.MapPath("Bin");
            lic.SetLicense(licPath + @"\aspose.lic");

            string templatePath = Server.MapPath("InvoiceTemplate");


            var InvDetails = from DBData in dbobj.InvoiceMasters
                                 //where DBData.InvoiceId == Convert.ToInt64(Global.InvoiceID)
                             where DBData.InvoiceId == Convert.ToInt64(ViewState["InvID"])
                             select DBData;
            if (InvDetails.Count() > 0)
            {
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook(templatePath + @"\Invoice Template2.xls");
                Aspose.Cells.Worksheet ws = wb.Worksheets["Invoice"];

                ws.Cells[10, 11].Value = InvDetails.Single().InvoiceNumber;
                ws.Cells[11, 11].Value = InvDetails.Single().InvoiceDate.ToString("dd-MMM-yy");


                if (InvDetails.Single().PONumber != null)
                {
                    ws.Cells[26, 2].Value = InvDetails.Single().PONumber;
                    ws.Cells[26, 3].Value = InvDetails.Single().PODate;
                }

                //Add Client Details
                ws.Cells[26, 6].Value = lblProjectFrom.Text;
                var ClientData = from DBData in dbobj.ClientMasters
                                 join
                                 CurrencyData in dbobj.CurrencyMasters
                                 on
                                 DBData.CurrencyId equals CurrencyData.CurrencyId
                                 join
                                 CityData in dbobj.CityMasters
                                 on
                                 DBData.City1 equals CityData.CityId
                                 join
                                 StateData in dbobj.StateMasters
                                 on
                                 DBData.State1 equals StateData.StateId
                                 join
                                 CountyData in dbobj.CountryMasters
                                 on
                                 DBData.Country1 equals CountyData.CountryId
                                 where DBData.ClientId == InvDetails.Single().ClientId
                                 &&
                                 !CurrencyData.IsDeleted
                                 select new
                                 {
                                     ClientName = DBData.ClientName,
                                     Address = DBData.Address1 + Environment.NewLine + CityData.CityName + ", " + StateData.StateName + " " + DBData.Zip_Postal1,
                                     Country = CountyData.CountryName,
                                     CurrencyID = DBData.CurrencyId,
                                     CurrencyName = CurrencyData.CurrencyName,
                                     CurrencyCode = CurrencyData.CurrencyCode,
                                     CurrencySymbol = CurrencyData.CurrencySymbol
                                 };
                ws.Cells[20, 2].Value = ClientData.Single().ClientName;
                ws.Cells[21, 2].Value = ClientData.Single().Address;
                ws.Cells[22, 2].Value = ClientData.Single().Country;


                /***********************************************************************/
                /*          Update Currency                                            */
                /***********************************************************************/
                Aspose.Cells.Range rngAmountCurr = ws.Cells.CreateRange(31, 12, 26, 1);
                Aspose.Cells.Style styleAmountCurr = ws.Cells[31, 12].GetStyle();
                styleAmountCurr.Number = 1;
                styleAmountCurr.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.00";
                rngAmountCurr.SetStyle(styleAmountCurr);

                Aspose.Cells.Range rngAmountCurr2 = ws.Cells.CreateRange(30, 12, 1, 1);
                Aspose.Cells.Style styleAmountCurr2 = ws.Cells[30, 12].GetStyle();
                styleAmountCurr2.Number = 1;
                styleAmountCurr2.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.00";
                rngAmountCurr2.SetStyle(styleAmountCurr2);

                Aspose.Cells.Range rngAmountCurr3 = ws.Cells.CreateRange(57, 12, 1, 1);
                Aspose.Cells.Style styleAmountCurr3 = ws.Cells[57, 12].GetStyle();
                styleAmountCurr3.Number = 1;
                styleAmountCurr3.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.00";
                rngAmountCurr3.SetStyle(styleAmountCurr3);


                Aspose.Cells.Range rngUnitPriceCurr = ws.Cells.CreateRange(31, 10, 26, 1);
                Aspose.Cells.Style styleUnitPriceCurr = ws.Cells[31, 10].GetStyle();
                styleUnitPriceCurr.Number = 1;
                styleUnitPriceCurr.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.00";
                rngUnitPriceCurr.SetStyle(styleUnitPriceCurr);

                Aspose.Cells.Range rngUnitPriceCurr2 = ws.Cells.CreateRange(30, 10, 1, 1);
                Aspose.Cells.Style styleUnitPriceCurr2 = ws.Cells[30, 10].GetStyle();
                styleUnitPriceCurr2.Number = 1;
                styleUnitPriceCurr2.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.00";
                rngUnitPriceCurr2.SetStyle(styleUnitPriceCurr2);

                Aspose.Cells.Range rngUnitPriceCurr3 = ws.Cells.CreateRange(57, 10, 1, 1);
                Aspose.Cells.Style styleUnitPriceCurr3 = ws.Cells[57, 10].GetStyle();
                styleUnitPriceCurr3.Number = 1;
                styleUnitPriceCurr3.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.00";
                rngUnitPriceCurr3.SetStyle(styleUnitPriceCurr3);


                Aspose.Cells.Range rngTotalCurr = ws.Cells.CreateRange(59, 12, 1, 1);
                Aspose.Cells.Style styleTotalCurr = ws.Cells[59, 12].GetStyle();
                styleTotalCurr.Number = 1;
                styleTotalCurr.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.00";
                rngTotalCurr.SetStyle(styleTotalCurr);


                Aspose.Cells.Range rngPayAmtCurr = ws.Cells.CreateRange(65, 12, 1, 1);
                Aspose.Cells.Style stylePayAmtCurr = ws.Cells[65, 12].GetStyle();
                stylePayAmtCurr.Number = 1;
                stylePayAmtCurr.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.00";
                rngPayAmtCurr.SetStyle(stylePayAmtCurr);

                /***********************************************************************/



                int RecordCnt = 0;

                var InvData = from DBData in dbobj.InvoiceDetailsMasters
                                  //where DBData.InvoiceId == Convert.ToInt64(Global.InvoiceID)
                              where DBData.InvoiceId == Convert.ToInt64(ViewState["InvID"])
                              select DBData;
                if (InvData.Count() > 0)
                {

                    foreach (var inv in InvData)
                    {
                        if (ws.Cells[30, 3].Value == null)
                        {
                            ws.Cells[30, 2].Value = 1;
                            ws.Cells[30, 3].Value = inv.ItemDesc;

                            if (!(inv.Qty == 0 || inv.Qty == null))
                            {
                                ws.Cells[30, 8].Value = inv.Qty.Value;
                                ws.Cells[30, 10].Value = inv.UnitPrice.Value;
                                ws.Cells[30, 11].Value = inv.PriceType;
                            }

                            Aspose.Cells.Style desStyle = ws.Cells[30, 3].GetStyle();
                            desStyle.IsTextWrapped = true;
                            ws.Cells[30, 3].SetStyle(desStyle);
                            ws.AutoFitRow(30);
                            RecordCnt++;
                        }
                        else
                        {
                            //Insert New Row
                            if (30 + RecordCnt > 57)
                            {
                                ws.Cells.InsertRow(30 + RecordCnt);
                                ws.Cells.InsertRow(30 + RecordCnt);

                                ws.Cells[30 + RecordCnt - 1, 2].SetStyle(ws.Cells[30, 2].GetStyle());
                                ws.Cells[30 + RecordCnt, 2].SetStyle(ws.Cells[30, 2].GetStyle());

                                Aspose.Cells.Range rngDesc = ws.Cells.CreateRange(30 + RecordCnt, 3, 1, 5);
                                if (!ws.Cells[30 + RecordCnt, 3].IsMerged)
                                    rngDesc.Merge();

                                rngDesc = ws.Cells.CreateRange(30 + RecordCnt + 1, 3, 1, 5);
                                if (!ws.Cells[31 + RecordCnt, 3].IsMerged)
                                    rngDesc.Merge();

                                Aspose.Cells.Style styleQty = ws.Cells[30, 8].GetStyle();
                                Aspose.Cells.Range rngQty = ws.Cells.CreateRange(30 + RecordCnt, 8, 1, 2);
                                if (!ws.Cells[30 + RecordCnt, 8].IsMerged)
                                    rngQty.Merge();
                                styleQty.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.None;
                                styleQty.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.None;
                                styleQty.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                                styleQty.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                                rngQty.SetStyle(styleQty);



                                rngQty = ws.Cells.CreateRange(30 + RecordCnt + 1, 8, 1, 2);
                                if (!ws.Cells[31 + RecordCnt, 8].IsMerged)
                                    rngQty.Merge();
                                rngQty.SetStyle(styleQty);

                                Aspose.Cells.Range rngTotalAmount = ws.Cells.CreateRange(30 + RecordCnt, 12, 1, 2);
                                if (!ws.Cells[30 + RecordCnt, 12].IsMerged)
                                    rngTotalAmount.Merge();
                                Aspose.Cells.Style styleTotalAmount = ws.Cells[30, 12].GetStyle();
                                styleTotalAmount.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.None;
                                styleTotalAmount.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.None;
                                styleTotalAmount.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                                styleTotalAmount.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                                styleTotalAmount.Number = 1;
                                styleTotalAmount.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.00";
                                rngTotalAmount.SetStyle(styleTotalAmount);


                                rngTotalAmount = ws.Cells.CreateRange(30 + RecordCnt + 1, 12, 1, 2);
                                if (!ws.Cells[31 + RecordCnt, 12].IsMerged)
                                    rngTotalAmount.Merge();
                                styleTotalAmount.Number = 1;
                                styleTotalAmount.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.00";
                                rngTotalAmount.SetStyle(styleTotalAmount);


                                Aspose.Cells.Style styleUnitPrice = ws.Cells[30, 10].GetStyle();
                                styleUnitPrice.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.None;
                                styleUnitPrice.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.None;
                                styleUnitPrice.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                                styleUnitPrice.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                                styleUnitPrice.Number = 1;
                                styleUnitPrice.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.00";


                                ws.Cells[30 + RecordCnt, 10].SetStyle(styleUnitPrice);
                                ws.Cells[31 + RecordCnt, 10].SetStyle(styleUnitPrice);
                            }
                            //ws.Cells[31 + RecordCnt, 2].Value = ((RecordCnt / 2) + 2).ToString();
                            ws.Cells[31 + RecordCnt, 3].Value = inv.ItemDesc;

                            if (!(inv.Qty == 0 || inv.Qty == null))
                            {
                                ws.Cells[31 + RecordCnt, 2].Value = NextSRNo;
                                ws.Cells[31 + RecordCnt, 8].Value = inv.Qty.Value;
                                ws.Cells[31 + RecordCnt, 10].Value = inv.UnitPrice.Value;
                                ws.Cells[31 + RecordCnt, 11].Value = inv.PriceType;
                                NextSRNo++;
                            }

                            Aspose.Cells.Style desStyle = ws.Cells[31 + RecordCnt, 3].GetStyle();
                            desStyle.IsTextWrapped = true;
                            ws.Cells[31 + RecordCnt, 3].SetStyle(desStyle);

                            //ws.Cells[31 + RecordCnt, 12].Formula = "=IF(I" + (32 + RecordCnt) + "=\"\",\"\",IF(K" + (32 + RecordCnt) + "=\"\",\"\",ROUND(I" + (32 + RecordCnt) + "*K" + (32 + RecordCnt) + ",2)))";


                            ws.AutoFitRow(30 + RecordCnt);
                            RecordCnt++;
                            RecordCnt++;
                        }
                    }
                }


                wb.CalculateFormula();

                wb.Save(stm, Aspose.Cells.SaveFormat.Pdf);
                string fileName = InvDetails.Single().InvoiceNumber.Replace("/", "_") + ".pdf";
                ExportFile(stm, fileName);
            }
        }
    }

    protected void btnExportXLS_Click(object sender, EventArgs e)
    {
        //ExportToXLS();

        string InvoiceNumber = string.Empty;
        //MemoryStream ms = Global.GetInvoice(Global.InvoiceID, "xls", out InvoiceNumber);
        MemoryStream ms = Global.GetInvoice(Convert.ToInt64(ViewState["InvID"]), "xls", out InvoiceNumber);
        ExportFile(ms, InvoiceNumber + ".xls");



        //if (Global.InvoiceID != null)
        //{
        //    MemoryStream stm = new MemoryStream();
        //    int NextSRNo = 2;

        //    Aspose.Cells.License lic = new Aspose.Cells.License();
        //    string licPath = Server.MapPath("Bin");
        //    lic.SetLicense(licPath + @"\aspose.lic");

        //    string templatePath = Server.MapPath("InvoiceTemplate");


        //    var InvDetails = from DBData in dbobj.InvoiceMasters
        //                     where DBData.InvoiceId == Convert.ToInt64(Global.InvoiceID)
        //                     select DBData;
        //    if (InvDetails.Count() > 0)
        //    {
        //        Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook(templatePath + @"\Invoice Template.xls");
        //        Aspose.Cells.Worksheet ws = wb.Worksheets["Invoice"];

        //        ws.Cells[10, 11].Value = InvDetails.Single().InvoiceNumber;
        //        ws.Cells[11, 11].Value = InvDetails.Single().InvoiceDate.ToString("dd-MMM-yy");


        //        if (InvDetails.Single().PONumber != null)
        //        {
        //            ws.Cells[26, 2].Value = InvDetails.Single().PONumber;
        //            ws.Cells[26, 3].Value = InvDetails.Single().PODate;
        //        }

        //        //Add Client Details
        //        ws.Cells[26, 6].Value = lblProjectFrom.Text;
        //        var ClientData = from DBData in dbobj.ClientMasters
        //                         join
        //                         CityData in dbobj.CityMasters
        //                         on
        //                         DBData.City1 equals CityData.CityId
        //                         join
        //                         StateData in dbobj.StateMasters
        //                         on
        //                         DBData.State1 equals StateData.StateId
        //                         join
        //                         CountyData in dbobj.CountryMasters
        //                         on
        //                         DBData.Country1 equals CountyData.CountryId
        //                         where DBData.ClientId == InvDetails.Single().ClientId
        //                         select new
        //                         {
        //                             ClientName = DBData.ClientName,
        //                             Address = DBData.Address1 + Environment.NewLine + CityData.CityName + ", " + StateData.StateName + " " + DBData.Zip_Postal1,
        //                             Country = CountyData.CountryName
        //                         };
        //        ws.Cells[20, 2].Value = ClientData.Single().ClientName;
        //        ws.Cells[21, 2].Value = ClientData.Single().Address;
        //        ws.Cells[22, 2].Value = ClientData.Single().Country;


        //        int RecordCnt = 0;

        //        var InvData = from DBData in dbobj.InvoiceDetailsMasters
        //                      where DBData.InvoiceId == Convert.ToInt64(Session["InvoiceID"])
        //                      select DBData;
        //        if (InvData.Count() > 0)
        //        {

        //            foreach (var inv in InvData)
        //            {
        //                if (ws.Cells[30, 3].Value == null)
        //                {
        //                    ws.Cells[30, 2].Value = 1;
        //                    ws.Cells[30, 3].Value = inv.ItemDesc;

        //                    Aspose.Cells.Style styleQtyFirst = ws.Cells[30, 8].GetStyle();
        //                    styleQtyFirst.Number = 0;
        //                    ws.Cells[30, 8].SetStyle(styleQtyFirst);

        //                    Aspose.Cells.Style styleUPriceFirst = ws.Cells[30, 10].GetStyle();
        //                    styleQtyFirst.Number = 0;
        //                    ws.Cells[30, 8].SetStyle(styleUPriceFirst);


        //                    if (!(inv.Qty == 0 || inv.Qty == null))
        //                    {
        //                        ws.Cells[30, 8].Value = inv.Qty.Value;
        //                        ws.Cells[30, 10].Value = inv.UnitPrice.Value;
        //                        ws.Cells[30, 11].Value = inv.PriceType;
        //                    }
        //                    ws.AutoFitRow(30);
        //                    RecordCnt++;
        //                }
        //                else
        //                {
        //                    //Insert New Row

        //                    ws.Cells.InsertRow(30 + RecordCnt);
        //                    ws.Cells.InsertRow(30 + RecordCnt);

        //                    ws.Cells[30 + RecordCnt - 1, 2].SetStyle(ws.Cells[30, 2].GetStyle());
        //                    ws.Cells[30 + RecordCnt, 2].SetStyle(ws.Cells[30, 2].GetStyle());

        //                    Aspose.Cells.Range rngDesc = ws.Cells.CreateRange(30 + RecordCnt, 3, 1, 5);
        //                    if (!ws.Cells[30 + RecordCnt, 3].IsMerged)
        //                        rngDesc.Merge();

        //                    rngDesc = ws.Cells.CreateRange(30 + RecordCnt + 1, 3, 1, 5);
        //                    if (!ws.Cells[31 + RecordCnt, 3].IsMerged)
        //                        rngDesc.Merge();

        //                    Aspose.Cells.Style styleQty = ws.Cells[30, 8].GetStyle();
        //                    Aspose.Cells.Range rngQty = ws.Cells.CreateRange(30 + RecordCnt, 8, 1, 2);
        //                    if (!ws.Cells[30 + RecordCnt, 8].IsMerged)
        //                        rngQty.Merge();
        //                    styleQty.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.None;
        //                    styleQty.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.None;
        //                    styleQty.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
        //                    styleQty.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
        //                    rngQty.SetStyle(styleQty);



        //                    rngQty = ws.Cells.CreateRange(30 + RecordCnt + 1, 8, 1, 2);
        //                    if (!ws.Cells[31 + RecordCnt, 8].IsMerged)
        //                        rngQty.Merge();
        //                    rngQty.SetStyle(styleQty);

        //                    Aspose.Cells.Range rngTotalAmount = ws.Cells.CreateRange(30 + RecordCnt, 12, 1, 2);
        //                    if (!ws.Cells[30 + RecordCnt, 12].IsMerged)
        //                        rngTotalAmount.Merge();
        //                    Aspose.Cells.Style styleTotalAmount = ws.Cells[30, 12].GetStyle();
        //                    styleTotalAmount.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.None;
        //                    styleTotalAmount.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.None;
        //                    styleTotalAmount.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
        //                    styleTotalAmount.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
        //                    rngTotalAmount.SetStyle(styleTotalAmount);


        //                    rngTotalAmount = ws.Cells.CreateRange(30 + RecordCnt + 1, 12, 1, 2);
        //                    if (!ws.Cells[31 + RecordCnt, 12].IsMerged)
        //                        rngTotalAmount.Merge();
        //                    rngTotalAmount.SetStyle(styleTotalAmount);


        //                    Aspose.Cells.Style styleUnitPrice = ws.Cells[30, 10].GetStyle();
        //                    styleUnitPrice.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.None;
        //                    styleUnitPrice.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.None;
        //                    styleUnitPrice.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
        //                    styleUnitPrice.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
        //                    ws.Cells[30 + RecordCnt, 10].SetStyle(styleUnitPrice);
        //                    ws.Cells[31 + RecordCnt, 10].SetStyle(styleUnitPrice);

        //                    //ws.Cells[31 + RecordCnt, 2].Value = ((RecordCnt / 2) + 2).ToString();
        //                    ws.Cells[31 + RecordCnt, 3].Value = inv.ItemDesc;

        //                    if (!(inv.Qty == 0 || inv.Qty == null))
        //                    {
        //                        ws.Cells[31 + RecordCnt, 2].Value = NextSRNo;
        //                        ws.Cells[31 + RecordCnt, 8].Value = inv.Qty.Value;
        //                        ws.Cells[31 + RecordCnt, 10].Value = inv.UnitPrice.Value;
        //                        ws.Cells[31 + RecordCnt, 11].Value = inv.PriceType;
        //                        NextSRNo++;
        //                    }



        //                    ws.Cells[31 + RecordCnt, 12].Formula = "=IF(I" + (32 + RecordCnt) + "=\"\",\"\",IF(K" + (32 + RecordCnt) + "=\"\",\"\",ROUND(I" + (32 + RecordCnt) + "*K" + (32 + RecordCnt) + ",2)))";


        //                    ws.AutoFitRow(30 + RecordCnt);
        //                    RecordCnt++;
        //                    RecordCnt++;
        //                }
        //            }
        //        }
        //        //Aspose.Cells.Style styleRS = ws.Cells[6, 2].GetStyle();
        //        ////styleRS.Custom = "[$" + "\u20A8" + "] 0.00";x20b9
        //        //styleRS.Custom = "[$" + "xa3;" + "] 0.00";
        //        ////styleRS.Custom = "[$#,##0.00]";
        //        //Aspose.Cells.StyleFlag flag = new Aspose.Cells.StyleFlag();
        //        //flag.NumberFormat = true;
        //        //ws.Cells[6, 2].SetStyle(styleRS, flag);
        //        //ws.Cells[6, 2].Value = 115.25;

        //        wb.CalculateFormula();

        //        wb.Save(stm, Aspose.Cells.SaveFormat.Excel97To2003);
        //        string fileName = "MIT-USA-" + InvDetails.Single().InvoiceNumber.Replace("/", "_") + ".xls";
        //        ExportFile(stm, fileName);
        //    }
        //}
    }

    protected void btnExportPDF_Click(object sender, EventArgs e)
    {
        string InvoiceNumber = string.Empty;
        
        MemoryStream ms = Global.GetInvoice(Convert.ToInt64(ViewState["InvID"]), "pdf", out InvoiceNumber);
        ExportFile(ms, InvoiceNumber + ".pdf");        
    }

    //Show invoice preview in browser
    protected void btnShowPreview_Click(object sender, EventArgs e)
    {
        //ClientScript.RegisterStartupScript(this.GetType(), "Show Preview", "window.open('PreviewInvoices?" + Global.Encrypt("i=" + Global.InvoiceID) + "', null,'height=700,width=750,status=yes,toolbar=no,menubar=no,location=no');", true);

        ClientScript.RegisterStartupScript(this.GetType(), "Show Preview", "window.open('PreviewInvoices?" + Global.Encrypt("i=" + ViewState["InvID"]) + "', null,'height=700,width=750,status=yes,toolbar=no,menubar=no,location=no');", true);

        //Response.Write("<script language='javascript'>window.open('PreviewInvoice.aspx?i=" + Global.InvoiceID + "', null,'height=700,width=750,status=yes,toolbar=no,menubar=no,location=no');</script>");
    }

    //Click on SendEmail button will redirect user to another page
    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        Global.PrevUrl = this.Request.Url.ToString();
        //Response.Write("<script language='javascript'>window.open('SendInvoiceEmail.aspx?i=" + Global.InvoiceID + "', null,'height=700,width=750,status=yes,toolbar=no,menubar=no,location=no');</script>");
        /////Response.Redirect("SendInvoiceEmail.aspx?i=" + Global.InvoiceID+"&P=I");

        //Response.Redirect("SendInvoiceEmail?" + Global.Encrypt("i=" + Global.InvoiceID + "&P=I"));
        Response.Redirect("SendInvoiceEmail?" + Global.Encrypt("i=" + ViewState["InvID"] + "&P=I"));
    }

    //protected void txtOrderNo_TextChanged(object sender, EventArgs e)
    //{
    //    if (txtOrderNo.Text.Trim() == "")
    //    {
    //        txtOrderDate.Text = "";
    //        txtOrderDate.Enabled = false;
    //    }
    //    else
    //        txtOrderDate.Enabled = true;
    //}

    protected void drpClient_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillClientContact();
    }

    protected void grdInvoiceData_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int RowIndex = e.RowIndex;
        GridViewRow gr = grdInvoiceData.Rows[RowIndex];
        Label lblInvoiceItem = (Label)gr.FindControl("lblItemID");
        Int64 InvoiceItemID = Convert.ToInt64(lblInvoiceItem.Text);

        var InvoiceItemData = from DBData in dbobj.InvoiceDetailsMasters
                              where DBData.InvoiceDetailsId == InvoiceItemID
                              select DBData;
        if (InvoiceItemData.Count() > 0)
        {
            dbobj.InvoiceDetailsMasters.DeleteOnSubmit(InvoiceItemData.Single());
            dbobj.SubmitChanges();
            dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
        }
        FillGrid();
    }

    //Generate Revised Invoice with Old invoice data
    protected void btnRevised_Click(object sender, EventArgs e)
    {
        //*** Invoice Info.
        var Invoice = from DBInvoice in dbobj.InvoiceMasters
                          //where DBInvoice.InvoiceId == Convert.ToInt64(Global.InvoiceID)
                      where DBInvoice.InvoiceId == Convert.ToInt64(ViewState["InvID"])
                      select DBInvoice;
        if (Invoice.Count() > 0)
        {
            //*** Update Old Invoice
            var SInvoice = Invoice.Single();
            SInvoice.InvoiceStatus = "Revised";
            SInvoice.ModifyBy = Convert.ToInt64(Global.UserId);
            SInvoice.ModifyDate = DateTime.Now;
            SInvoice.IsRevised = true;
            dbobj.SubmitChanges();
            dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);

            //*** Generate New Revised Invoice
            InvoiceMaster im = new InvoiceMaster();
            im.ClientId = SInvoice.ClientId;
            im.InvoiceSeqNo = SInvoice.InvoiceSeqNo;
            im.InvoiceDate = DateTime.Now;
            if (SInvoice.Revision == null)
            {
                im.Revision = "A";
                im.InvoiceNumber = SInvoice.InvoiceNumber + "A";
            }
            else
            {
                char[] temp = SInvoice.Revision.ToCharArray();
                int re = (int)temp[0] + 1;
                im.Revision = ((char)re).ToString();
                im.InvoiceNumber = (SInvoice.InvoiceNumber.Substring(0, SInvoice.InvoiceNumber.Length - 1)) + ((char)re).ToString();
            }
            im.InvoiceFor = SInvoice.InvoiceFor;
            im.PONumber = SInvoice.PONumber;
            im.PODate = SInvoice.PODate;
            im.InvoiceStartDate = SInvoice.InvoiceStartDate;
            im.InvoiceEndDate = SInvoice.InvoiceEndDate;
            im.ProjectFrom = SInvoice.ProjectFrom;
            im.AttachmentName = SInvoice.AttachmentName;
            im.Remarks = SInvoice.Remarks;
            im.InvoiceStatus = "Draft";
            im.CreatedBy = Convert.ToInt64(Global.UserId);
            im.CreatedDate = DateTime.Now;
            im.IsRevised = false;
            im.IsDeleted = false;
            dbobj.InvoiceMasters.InsertOnSubmit(im);
            dbobj.SubmitChanges();
            dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
            ViewState["InvID"] = im.InvoiceId;

            //*** Invoice Details
            var Details = from DBDetails in dbobj.InvoiceDetailsMasters
                          where DBDetails.InvoiceId == SInvoice.InvoiceId
                          select DBDetails;
            if (Details.Count() > 0)
            {
                foreach (var Det in Details)
                {
                    InvoiceDetailsMaster IDM = new InvoiceDetailsMaster();
                    IDM.InvoiceId = im.InvoiceId;
                    IDM.ItemDesc = Det.ItemDesc;
                    IDM.PriceType = Det.PriceType;
                    IDM.Qty = Det.Qty;
                    IDM.UnitPrice = Det.UnitPrice;
                    IDM.Discount = Det.Discount;
                    IDM.TotalAmt = Det.TotalAmt;
                    IDM.CreatedBy = Convert.ToInt64(Global.UserId);
                    IDM.CreatedDate = DateTime.Now;
                    dbobj.InvoiceDetailsMasters.InsertOnSubmit(IDM);
                    dbobj.SubmitChanges();
                    dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                }
            }

            //*** Change Globle to Response.Redirect
            //Global.InvoiceNo = im.InvoiceNumber;
            //Global.InvoiceID = im.InvoiceId;
        }
        //Response.Redirect("NewInvoice?InvID=" + Global.Encrypt(Global.InvoiceID.ToString()));

        Response.Redirect("NewInvoice?InvID=" + Global.Encrypt(ViewState["InvID"].ToString()));
        btnSendEmail.Enabled = true;
        btnBack.Enabled = true;
    }

    protected void txtInvoiceStart_TextChanged(object sender, EventArgs e)
    {
        txtInvoiceEnd_CalendarExtenderPlus.MinimumDate = DateTime.ParseExact(txtInvoiceStart.Text, "dd-MMM-yyyy", null);
    }

    // Auto Fill Services 21-05-2014
    //[WebMethod]
    //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    //public static List<string> GetServices(string pre)
    //{
    //    List<string> allGetServices = new List<string>();

    //    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MITPLInvoiceConnectionString"].ConnectionString);
    //    conn.Open();
    //    string query = "select distinct idm.ItemDesc from InvoiceDetailsMaster as idm join InvoiceMaster as im on idm.InvoiceId = im.InvoiceId join ClientMaster as cm on im.ClientId = cm.ClientId where idm.ItemDesc like '" + pre + "%' and cm.DepartmentId=" + Global.Department + "";

    //    SqlCommand cmd = new SqlCommand(query, conn);
    //    DataTable dt = new DataTable();
    //    dt.Load(cmd.ExecuteReader());

    //    for (int i = 0; i < dt.Rows.Count; i++)
    //    {
    //        allGetServices.Add(dt.Rows[i][0].ToString());
    //    }
    //    //using (MyDatabaseEntities dc = new MyDatabaseEntities())
    //    //{

    //    //    allCompanyName = (from a in dc.TopCompanies
    //    //                      where a.CompanyName.StartsWith(pre)
    //    //                      select a.CompanyName).ToList();
    //    //}
    //    return allGetServices;
    //}

}
