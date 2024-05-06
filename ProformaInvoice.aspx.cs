using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ProformaInvoice : System.Web.UI.Page
{
    MITInvoiceDataContext dbobj = new MITInvoiceDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
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
                    txtProInvoiceStart.Text = "01-" + DateTime.Now.AddMonths(-1).ToString("MMM-yyyy");
                    txtProInvoiceEnd.Text = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month) + "-" + DateTime.Now.AddMonths(-1).ToString("MMM-yyyy");

                    txtProInvoice.Text = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month) + "-" + DateTime.Now.AddMonths(-1).ToString("MMM-yyyy");
                    txtProOrderDate.Text = txtProInvoiceEnd.Text;
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
                txtProInvoiceEnd_CalendarExtenderPlus.MaximumDate = DateTime.Now;
                //txtProInvoiceEnd_CalendarExtenderPlus.MinimumDate = DateTime.ParseExact(txtProInvoiceStart.Text, "dd-MMM-yyyy", null);
                //txtProInvoice_CalendarExtenderPlus.MaximumDate = DateTime.Now;
            }
        }
    }

    private void FillInvoiceData()
    {
        var InvData = from DBData in dbobj.ProformaInvoiceMasters
                      where DBData.ProInvoiceId == Convert.ToInt64(ViewState["InvID"])
                      select DBData;

        if (InvData.Count() > 0)
        {
            drpProBU.SelectedValue = InvData.Single().ClientMaster.DepartmentId.ToString();
            FillClient();
            drpProClient.SelectedValue = InvData.Single().ClientId.ToString();
            txtProInvoice.Text = InvData.Single().InvoiceDate.ToString("dd-MMM-yyyy");
            txtProInvoiceEnd.Text = InvData.Single().InvoiceEndDate.ToString("dd-MMM-yyyy");
            txtProInvoiceStart.Text = InvData.Single().InvoiceStartDate.ToString("dd-MMM-yyyy");
            if (InvData.Single().InvoiceFor.ToString() == "USA")
                rdbProUsa.Checked = true;
            else
                rdbProIndia.Checked = true;
            FillClientContact();
            if (InvData.Single().ProjectFrom.ToString() != null && InvData.Single().ProjectFrom.ToString() != "0")
                drpProProjectFrom.Items.FindByValue(InvData.Single().ProjectFrom.ToString()).Selected = true;
            txtProOrderNo.Text = "";
            txtProRemarks.Text = InvData.Single().Remarks;
            ViewState["InvNO"] = InvData.Single().InvoiceNumber;

            if (InvData.Single().PONumber != null)
            {
                txtProOrderNo.Text = InvData.Single().PONumber;
                txtProOrderDate.Text = InvData.Single().PODate.Value.ToString("dd-MMM-yyyy");
            }

            btnSaveInvoiceInfo.Text = "Update";

            if (InvData.Single().InvoiceStatus != "Draft")
            {                
                grdInvoiceData.ShowFooter = true;
                grdInvoiceData.Columns[7].Visible = true;
            }

            if (InvData.Single().InvoiceStatus == "Unpaid" && Request.QueryString["InvID"] != "")
                btnRevised.Visible = true;
            else
                btnRevised.Visible = false;

            drpProBU.Enabled = false;
            ViewState["CreatedBy"] = InvData.Single().CreatedBy == null ? "" : (InvData.Single().CreatedBy).ToString();
            FillGrid();

            if (InvData.Single().CreatedBy != Convert.ToInt64(Global.UserId))
            {
                grdInvoiceData.FooterRow.Visible = false;
            }

            MultiViewProformaInvoice.ActiveViewIndex = 1;
        }
    }

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

    private void BlankGrid()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("ProInvoiceDetailsId"));
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

    private void FillGrid()
    {
        lblClientName.Text = drpProClient.SelectedItem.Text;
        lblInvoiceDate.Text = txtProInvoice.Text;

        string iNo = ViewState["InvNO"].ToString();
        char[] ciNo = iNo.ToCharArray();
        if (char.IsLetter(ciNo[ciNo.Length - 1]))
            lblInvoiceNo.Text = iNo.Substring(0, iNo.Length - 1) + " Revised";
        else
            lblInvoiceNo.Text = iNo;

        lblInvoicePeriod.Text = (Convert.ToDateTime(txtProInvoiceStart.Text).ToString("MMM-yyyy") == Convert.ToDateTime(txtProInvoiceEnd.Text).ToString("MMM-yyyy")) ? Convert.ToDateTime(txtProInvoiceStart.Text).ToString("MMM-yyyy") : Convert.ToDateTime(txtProInvoiceStart.Text).ToString("MMM") + "-" + Convert.ToDateTime(txtProInvoiceEnd.Text).ToString("MMM-yyyy");
        lblOrderDate.Text = txtProOrderNo.Text != "" ? Convert.ToDateTime(txtProOrderDate.Text).ToString("dd-MMM-yyyy") : string.Empty;
        lblOrderNo.Text = txtProOrderNo.Text;
        lblProjectFrom.Text = drpProProjectFrom.Items.Count > 0 ? (drpProProjectFrom.SelectedIndex > 0 ? drpProProjectFrom.SelectedItem.Text : string.Empty) : string.Empty;

        string ClientCurrency = string.Empty;
        var CurrData = from DBData in dbobj.CurrencyMasters
                       join
                       ClientData in dbobj.ClientMasters
                       on
                       DBData.CurrencyId equals ClientData.CurrencyId
                       join
                       InvoiceData in dbobj.ProformaInvoiceMasters
                       on
                       ClientData.ClientId equals InvoiceData.ClientId
                       where InvoiceData.ProInvoiceId == Convert.ToInt64(ViewState["InvID"])
                       select DBData;
        if (CurrData.Count() > 0)
        {
            ClientCurrency = CurrData.Single().CurrencyCode;
        }

        lblTotalInvoiceAmount.Text = ClientCurrency + " " + Math.Round(Convert.ToDecimal(((from DBData in dbobj.ProformaInvoiceDetailsMasters
                                                                                           where DBData.ProInvoiceId == Convert.ToInt64(ViewState["InvID"])
                                                                                           select DBData.TotalAmt.Value).Count() == 0 ? "0.00" : Convert.ToString(((from DBData in dbobj.ProformaInvoiceDetailsMasters
                                                                                                                                                                    where DBData.ProInvoiceId == Convert.ToInt64(ViewState["InvID"])
                                                                                                                                                                    select DBData.TotalAmt).Sum())))), 2, MidpointRounding.ToEven);

        // Get Invoice Line Item Data
        var InvData = from DBData in dbobj.ProformaInvoiceDetailsMasters
                      where DBData.ProInvoiceId == Convert.ToInt64(ViewState["InvID"])
                      select DBData;

        if (InvData.Count() > 0)
        {
            grdInvoiceData.DataSource = InvData;
            grdInvoiceData.DataBind();
        }
        else
            BlankGrid();

    }

    private void FillBusinessUnit()
    {
        if (Global.UserType == "DIRECTOR" || Global.DepartmentName == "ACCT")
        {
            var BUData = from DBData in dbobj.DepartmentMasters
                         where DBData.IsActive
                         select new
                         {
                             BUName = DBData.DepartmentName,
                             BUID = DBData.DepartmentId
                         };

            drpProBU.DataSource = BUData;
            drpProBU.DataTextField = "BUName";
            drpProBU.DataValueField = "BUID";
            drpProBU.DataBind();

            if (BUData.Count() > 0)
            {
                drpProClient.SelectedIndex = 0;
                FillClient();
            }
        }
        else
        {
            var BUData = from DBData in dbobj.DepartmentMasters
                         where DBData.IsActive && (DBData.UserId == Convert.ToInt64(Global.UserId) || DBData.DepartmentId == Convert.ToInt64(Global.Department))
                         select new
                         {
                             BUName = DBData.DepartmentName,
                             BUID = DBData.DepartmentId
                         };

            drpProBU.DataSource = BUData;
            drpProBU.DataTextField = "BUName";
            drpProBU.DataValueField = "BUID";
            drpProBU.DataBind();

            if (BUData.Count() > 0)
            {
                drpProClient.SelectedIndex = 0;
                FillClient();
            }
        }
    }

    private void FillClient()
    {
        var ClientData = from DBData in dbobj.ClientMasters
                         join
                         DeptData in dbobj.DepartmentMasters
                         on
                         DBData.DepartmentId equals DeptData.DepartmentId
                         where DeptData.DepartmentId == Convert.ToInt64(drpProBU.SelectedValue) && (DBData.C_M1 == Global.UserM1 || DBData.C_M2 == Global.UserM2) && DBData.IsActive
                         orderby DBData.ClientName ascending
                         select new
                         {
                             ClientName = DBData.ClientName,
                             ClientID = DBData.ClientId
                         };
        drpProClient.DataSource = ClientData;
        drpProClient.DataTextField = "ClientName";
        drpProClient.DataValueField = "ClientID";
        drpProClient.DataBind();
        if (drpProClient.Items.Count > 1)
        {
            drpProClient.Items.Insert(0, "-- Select --");
            drpProClient.SelectedIndex = 0;
        }

        FillClientContact();
    }

    private void FillClientContact()
    {
        if (drpProClient.Items.Count > 0)
        {
            if (drpProClient.SelectedItem.ToString() != "-- Select --")
            {
                var ContactData = from DBData in dbobj.ClientContactMasters
                                  where DBData.ClientId == Convert.ToInt64(drpProClient.SelectedValue) && !DBData.IsDeleted
                                  select new
                                  {
                                      ContactID = DBData.ClientContactId,
                                      ContactName = DBData.Name
                                  };
                drpProProjectFrom.DataSource = ContactData;
                drpProProjectFrom.DataTextField = "ContactName";
                drpProProjectFrom.DataValueField = "ContactID";
                drpProProjectFrom.DataBind();
            }
            else
                drpProProjectFrom.Items.Clear();

            drpProProjectFrom.Items.Insert(0, "-- Select --");
        }
    }

    private string GetSequanceINDIA(string FY, Int64 iDeptID)
    {
        string sSeqINDIA_Number = "01";

        var InvData = (from DBData in dbobj.ProformaInvoiceMasters
                       join
                       ClientData in dbobj.ClientMasters
                       on
                       DBData.ClientId equals ClientData.ClientId
                       orderby DBData.InvoiceSeqNo descending
                       where DBData.InvoiceNumber.Contains(FY) && ClientData.DepartmentId == iDeptID && 
                       !DBData.IsDeleted.Value && DBData.InvoiceFor == "INDIA"
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

        var InvData = (from DBData in dbobj.ProformaInvoiceMasters
                       join
                       ClientData in dbobj.ClientMasters
                       on DBData.ClientId equals ClientData.ClientId
                       orderby DBData.InvoiceSeqNo descending
                       where DBData.InvoiceNumber.Contains(FY) && ClientData.DepartmentId == iDeptID && 
                       !DBData.IsDeleted.Value && DBData.InvoiceFor == "USA"
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
        string departmentName = string.Empty;

        departmentName = drpProBU.SelectedItem.Text.Substring(0, 2);

        sNumber = "Proforma MITPL" + FY + departmentName + "/" + seqNumber;

        return sNumber;
    }    

    private string GetProfomaInvoiceNumber_USA(string FY, string dName, string seqNumber)
    {
        string sNumber = string.Empty;

        sNumber = "Proforma MIT/" + FY + "/" + dName + "/" + seqNumber;

        return sNumber;
    }

    protected void btnSaveInvoiceInfo_Click(object sender, EventArgs e)
    {
        Page page = HttpContext.Current.Handler as Page;
        if (txtProOrderDate.Text == "" && txtProOrderNo.Text.Trim() != "")
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), "Email Error", "alert('Order Date cannot be blank');", true);

        }
        else
        {
            if (drpProClient.SelectedItem.ToString() == "-- Select --")
            {
                ScriptManager.RegisterStartupScript(page, page.GetType(), "MITPL Invoice", "alert('Please select client');", true);
            }
            else
            {
                if (txtProOrderNo.Text.Trim() != "")
                {
                    string strOrdNo = txtProOrderNo.Text.Trim().ToLower();
                    //var ordData = from DBData in dbobj.InvoiceMasters
                    //              where !DBData.IsDeleted.Value && DBData.PONumber.ToLower() == strOrdNo && DBData.ClientId == Convert.ToInt64(drpProClient.SelectedValue) && DBData.InvoiceEndDate.Date == Convert.ToDateTime(txtProInvoiceEnd.Text).Date && DBData.InvoiceStartDate.Date == Convert.ToDateTime(txtProInvoiceStart.Text).Date
                    //              select DBData;

                    var ordData = from DBData in dbobj.ProformaInvoiceMasters
                                  where !DBData.IsDeleted.Value && DBData.PONumber.ToLower() == strOrdNo && 
                                  DBData.ClientId == Convert.ToInt64(drpProClient.SelectedValue) && 
                                  DBData.InvoiceEndDate.Date == Convert.ToDateTime(txtProInvoiceEnd.Text).Date && 
                                  DBData.InvoiceStartDate.Date == Convert.ToDateTime(txtProInvoiceStart.Text).Date
                                  select DBData;

                    if (ordData.Count() > 0)
                    {
                        if (btnSaveInvoiceInfo.Text == "Update")
                        {
                            /***********************************************************************/
                            /* These two lines are added to allow update invoice with revision     */
                            /***********************************************************************/
                            var currInvDetail = ordData.Where(y => y.ProInvoiceId == Convert.ToInt64(ViewState["InvID"]));
                            var Odata = ordData.Select(y => y.ProInvoiceId != Convert.ToInt64(ViewState["InvID"]) && y.InvoiceSeqNo != currInvDetail.SingleOrDefault().InvoiceSeqNo);
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
                    #region save Invoice record
                    ProformaInvoiceMaster proInv = new ProformaInvoiceMaster();

                    proInv.ClientId = Convert.ToInt64(drpProClient.SelectedValue);
                    proInv.InvoiceDate = Convert.ToDateTime(txtProInvoice.Text);
                    proInv.InvoiceStartDate = Convert.ToDateTime(txtProInvoiceStart.Text);
                    proInv.InvoiceEndDate = Convert.ToDateTime(txtProInvoiceEnd.Text);
                    proInv.BusinessUnit = Convert.ToInt64(drpProBU.SelectedValue);

                    string financialYear = string.Empty;
                    int CurrYear = Convert.ToDateTime(txtProInvoice.Text).Year;

                    if (rdbProUsa.Checked == true)
                    {
                        proInv.InvoiceFor = "USA";
                        if (proInv.InvoiceDate <= Convert.ToDateTime("03/31/" + CurrYear))
                            financialYear = (CurrYear - 1) + "-" + CurrYear.ToString().Substring(2, 2);
                        else
                            financialYear = CurrYear + "-" + (CurrYear + 1).ToString().Substring(2, 2);
                    }
                    else
                    {
                        proInv.InvoiceFor = "INDIA";
                        if (proInv.InvoiceDate <= Convert.ToDateTime("03/31/" + CurrYear))
                            financialYear = (CurrYear - 1).ToString().Substring(2, 2) + "-" + CurrYear.ToString().Substring(2, 2);
                        else
                            financialYear = CurrYear.ToString().Substring(2, 2) + "-" + (CurrYear + 1).ToString().Substring(2, 2);
                    }

                    if (txtProOrderNo.Text.Trim() != "")
                    {
                        proInv.PONumber = txtProOrderNo.Text;
                        proInv.PODate = Convert.ToDateTime(txtProOrderDate.Text);
                    }

                    string seqNo = string.Empty;
                    if (rdbProUsa.Checked == true)
                        seqNo = GetSequanceUSA(financialYear, Convert.ToInt64(drpProBU.SelectedValue));
                    else
                        seqNo = GetSequanceINDIA(financialYear, Convert.ToInt64(drpProBU.SelectedValue));

                    proInv.InvoiceSeqNo = Convert.ToInt64(seqNo);

                    if (rdbProUsa.Checked)
                        proInv.InvoiceNumber = GetProfomaInvoiceNumber_USA(financialYear, drpProBU.SelectedItem.Text, seqNo);
                    else
                        proInv.InvoiceNumber = GetProformaInvoiceNumber_INDIA(financialYear, drpProBU.SelectedItem.Text, seqNo);

                    proInv.InvoiceStatus = "Draft";
                    if (drpProProjectFrom.SelectedIndex != 0)
                        proInv.ProjectFrom = Convert.ToInt64(drpProProjectFrom.SelectedValue);
                    else
                        proInv.ProjectFrom = 0;


                    proInv.IsDeleted = false;
                    proInv.IsRevised = false;
                    proInv.Remarks = txtProRemarks.Text.Trim();
                    proInv.CreatedBy = Convert.ToInt64(Global.UserId);
                    proInv.CreatedDate = DateTime.Now;
                    proInv.ModifyDate = DateTime.Now;
                    proInv.ModifyBy = Convert.ToInt64(Global.UserId);

                    dbobj.ProformaInvoiceMasters.InsertOnSubmit(proInv);
                    dbobj.SubmitChanges();

                    ViewState["InvID"] = proInv.ProInvoiceId;
                    ViewState["InvNO"] = proInv.InvoiceNumber;
                    btnSaveInvoiceInfo.Text = "Update";

                    dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);

                    Response.Redirect("ProformaInvoice.aspx?InvID=" + Global.Encrypt(ViewState["InvID"].ToString()));
                    #endregion
                }
                else
                {
                    #region Update Invoice record
                    var invoiceData = from DBData in dbobj.ProformaInvoiceMasters                                          
                                      where DBData.ProInvoiceId == Convert.ToInt64(ViewState["InvID"])
                                      select DBData;
                    if (invoiceData.Count() > 0)
                    {
                        invoiceData.Single().ClientId = Convert.ToInt64(drpProClient.SelectedValue);
                        invoiceData.Single().InvoiceDate = Convert.ToDateTime(txtProInvoice.Text);
                        invoiceData.Single().InvoiceStartDate = Convert.ToDateTime(txtProInvoiceStart.Text);
                        invoiceData.Single().InvoiceEndDate = Convert.ToDateTime(txtProInvoiceEnd.Text);

                        string financialYearUp = string.Empty;
                        int CurrYearUp = Convert.ToDateTime(txtProInvoice.Text).Year;

                        if (rdbProUsa.Checked == true)
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

                        if (txtProOrderNo.Text.Trim() != "")
                        {
                            invoiceData.Single().PONumber = txtProOrderNo.Text;
                            invoiceData.Single().PODate = Convert.ToDateTime(txtProOrderDate.Text);
                        }
                        else
                        {
                            invoiceData.Single().PONumber = null;
                            invoiceData.Single().PODate = null;
                        }
                        #region for update Invoice Number
                        string seqNoUp = string.Empty;

                        if (invoiceData.Single().Revision != null)
                        {
                            if (rdbProUsa.Checked)
                                invoiceData.Single().InvoiceNumber = GetProfomaInvoiceNumber_USA(financialYearUp, drpProBU.SelectedItem.Text, invoiceData.Single().InvoiceSeqNo.ToString("000")) + invoiceData.Single().Revision;
                            else
                                invoiceData.Single().InvoiceNumber = GetProformaInvoiceNumber_INDIA(financialYearUp, drpProBU.SelectedItem.Text, invoiceData.Single().InvoiceSeqNo.ToString("00")) + invoiceData.Single().Revision;
                        }
                        else
                        {

                            if (rdbProUsa.Checked)
                                invoiceData.Single().InvoiceNumber = GetProfomaInvoiceNumber_USA(financialYearUp, drpProBU.SelectedItem.Text, invoiceData.Single().InvoiceSeqNo.ToString("000"));
                            else
                                invoiceData.Single().InvoiceNumber = GetProformaInvoiceNumber_INDIA(financialYearUp, drpProBU.SelectedItem.Text, invoiceData.Single().InvoiceSeqNo.ToString("00"));
                        }
                        #endregion

                        if (drpProProjectFrom.SelectedIndex != 0)
                            invoiceData.Single().ProjectFrom = Convert.ToInt64(drpProProjectFrom.SelectedValue);
                        else
                            invoiceData.Single().ProjectFrom = 0;
                        invoiceData.Single().Remarks = txtProRemarks.Text.Trim();
                        invoiceData.Single().ModifyDate = DateTime.Now;
                        invoiceData.Single().ModifyBy = Convert.ToInt64(Global.UserId);

                        dbobj.SubmitChanges();
                    }
                    #endregion
                }
                dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);

                Response.Redirect("ProformaInvoice?InvID=" + Global.Encrypt(ViewState["InvID"].ToString()));
            }
        }
    }

    protected void drpProClient_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillClientContact();
    }

    protected void txtProInvoiceStart_TextChanged(object sender, EventArgs e)
    {
        txtProInvoiceEnd_CalendarExtenderPlus.MinimumDate = DateTime.ParseExact(txtProInvoiceStart.Text, "dd-MMM-yyyy", null);
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

    protected void btnExportXLS_Click1(object sender, EventArgs e)
    {
        string InvoiceNumber = string.Empty;

        MemoryStream ms = Global.GetProformaInvoice(Convert.ToInt64(ViewState["InvID"]), "xls", out InvoiceNumber);
        ExportFile(ms, InvoiceNumber + ".xls");
    }

    protected void btnExportPDF_Click1(object sender, EventArgs e)
    {
        string InvoiceNumber = string.Empty;

        MemoryStream ms = Global.GetProformaInvoice(Convert.ToInt64(ViewState["InvID"]), "pdf", out InvoiceNumber);
        ExportFile(ms, InvoiceNumber + ".pdf");
    }

    protected void drpProClient_SelectedIndexChanged1(object sender, EventArgs e)
    {
        FillClientContact();
    }

    protected void btnShowPreview_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "Show Preview", "window.open('PreviewProfomaInvoice?" + Global.Encrypt("i=" + ViewState["InvID"]) + "', null,'height=700,width=750,status=yes,toolbar=no,menubar=no,location=no');", true);
    }

    protected void btnRevised_Click(object sender, EventArgs e)
    {
        var Invoice = from DBInvoice in dbobj.ProformaInvoiceMasters
                          //where DBInvoice.InvoiceId == Convert.ToInt64(Global.InvoiceID)
                      where DBInvoice.ProInvoiceId == Convert.ToInt64(ViewState["InvID"])
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
            ProformaInvoiceMaster pim = new ProformaInvoiceMaster();
            pim.ClientId = SInvoice.ClientId;
            pim.InvoiceSeqNo = SInvoice.InvoiceSeqNo;
            pim.InvoiceDate = DateTime.Now;
            if (SInvoice.Revision == null)
            {
                pim.Revision = "A";
                pim.InvoiceNumber = SInvoice.InvoiceNumber + "A";
            }
            else
            {
                char[] temp = SInvoice.Revision.ToCharArray();
                int re = (int)temp[0] + 1;
                pim.Revision = ((char)re).ToString();
                pim.InvoiceNumber = (SInvoice.InvoiceNumber.Substring(0, SInvoice.InvoiceNumber.Length - 1)) + ((char)re).ToString();
            }
            pim.InvoiceFor = SInvoice.InvoiceFor;
            pim.PONumber = SInvoice.PONumber;
            pim.PODate = SInvoice.PODate;
            pim.InvoiceStartDate = SInvoice.InvoiceStartDate;
            pim.InvoiceEndDate = SInvoice.InvoiceEndDate;
            pim.ProjectFrom = SInvoice.ProjectFrom;
            pim.AttachmentName = SInvoice.AttachmentName;
            pim.Remarks = SInvoice.Remarks;
            pim.InvoiceStatus = "Draft";
            pim.CreatedBy = Convert.ToInt64(Global.UserId);
            pim.CreatedDate = DateTime.Now;
            pim.IsRevised = false;
            pim.IsDeleted = false;
            dbobj.ProformaInvoiceMasters.InsertOnSubmit(pim);
            dbobj.SubmitChanges();
            dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
            ViewState["InvID"] = pim.ProInvoiceId;

            //*** Invoice Details
            var Details = from DBDetails in dbobj.ProformaInvoiceDetailsMasters
                          where DBDetails.ProInvoiceId == SInvoice.ProInvoiceId
                          select DBDetails;
            if (Details.Count() > 0)
            {
                foreach (var Det in Details)
                {
                    ProformaInvoiceDetailsMaster IDM = new ProformaInvoiceDetailsMaster();
                    IDM.ProInvoiceId = pim.ProInvoiceId;
                    IDM.ItemDesc = Det.ItemDesc;
                    IDM.PriceType = Det.PriceType;
                    IDM.Qty = Det.Qty;
                    IDM.UnitPrice = Det.UnitPrice;
                    IDM.Discount = Det.Discount;
                    IDM.TotalAmt = Det.TotalAmt;
                    IDM.CreatedBy = Convert.ToInt64(Global.UserId);
                    IDM.CreatedDate = DateTime.Now;
                    dbobj.ProformaInvoiceDetailsMasters.InsertOnSubmit(IDM);
                    dbobj.SubmitChanges();
                    dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                }
            }
        }
        Response.Redirect("ProformaInvoice?InvID=" + Global.Encrypt(ViewState["InvID"].ToString()));        
        btnBack.Enabled = true;
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        btnSaveInvoiceInfo.Text = "Update";
        MultiViewProformaInvoice.ActiveViewIndex = 0;
    }
   
    protected void imgbtnAdd_Click1(object sender, ImageClickEventArgs e)
    {
        TextBox txtAServices = (TextBox)grdInvoiceData.FooterRow.FindControl("txtAddServices");
        TextBox txtAQty = (TextBox)grdInvoiceData.FooterRow.FindControl("txtAddQuantity");
        TextBox txtAUnitPrice = (TextBox)grdInvoiceData.FooterRow.FindControl("txtAddUnitPrice");
        DropDownList drpAPriceType = (DropDownList)grdInvoiceData.FooterRow.FindControl("drpAddPriceType");

        ProformaInvoiceDetailsMaster invDetail = new ProformaInvoiceDetailsMaster();

        invDetail.ProInvoiceId = Convert.ToInt64(ViewState["InvID"]);
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
        dbobj.ProformaInvoiceDetailsMasters.InsertOnSubmit(invDetail);
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


        var InvItem = from DBData in dbobj.ProformaInvoiceDetailsMasters
                      where DBData.ProInvoiceDetailsId == Convert.ToInt64(lblItmID.Text)
                      select DBData;

        if (InvItem.Count() > 0)
        {
            InvItem.Single().ItemDesc = txtUDesc.Text.Trim();
            InvItem.Single().PriceType = drpUPriceType.SelectedItem.Text.Trim().ToUpper();
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

    protected void grdInvoiceData_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int RowIndex = e.RowIndex;
        GridViewRow gr = grdInvoiceData.Rows[RowIndex];
        Label lblInvoiceItem = (Label)gr.FindControl("lblItemID");
        Int64 InvoiceItemID = Convert.ToInt64(lblInvoiceItem.Text);

        var InvoiceItemData = from DBData in dbobj.ProformaInvoiceDetailsMasters
                              where DBData.ProInvoiceDetailsId == InvoiceItemID
                              select DBData;
        if (InvoiceItemData.Count() > 0)
        {
            dbobj.ProformaInvoiceDetailsMasters.DeleteOnSubmit(InvoiceItemData.Single());
            dbobj.SubmitChanges();
            dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
        }
        FillGrid();
    }
}