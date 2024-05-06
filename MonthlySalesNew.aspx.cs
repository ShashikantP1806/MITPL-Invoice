using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MonthlySalesNew : System.Web.UI.Page
{
    MITInvoiceDataContext dbobj = new MITInvoiceDataContext();
    DataTable dt = new DataTable();
    bool IsShowDelCol = false;
    bool IsShowStatusCol = false;
    bool IsShowRevisionCol = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Global.UserType == "DIRECTOR" || Global.DepartmentName == "ACCT")
        {
            if (!IsPostBack)
            {
                // IsShowDelCol = Convert.ToBoolean(lblShowHideDelColumn.Text);
                // IsShowStatusCol = Convert.ToBoolean(lblShowHideStatusColumn.Text);
                txtMonth.Text = DateTime.Now.ToString("MMM-yyyy");
                txtMonth_CalendarExtenderPlus.MaximumDate = DateTime.Now.AddMonths(-1);
                FillInvoice();
            }
        }
        else
        {
            Response.Redirect("Authorize");
        }
    }

    protected void txtMonth_TextChanged(object sender, EventArgs e)
    {
        FillInvoice();
    }

    // protected override void OnPreRender(EventArgs e)
    // {
    //     base.OnPreRender(e);
    //     gvMonthlySales.Columns[15].Visible = false;
    // }

    protected void FillInvoice()
    {
        dt.Columns.Add("S.N", typeof(string));
        dt.Columns.Add("NAME OF THE PARTY", typeof(string));
        dt.Columns.Add("INV #", typeof(int));
        dt.Columns.Add("DATE", typeof(string));
        dt.Columns.Add("AMOUNT", typeof(double));
        dt.Columns.Add("Sub-Amt.", typeof(double));
        dt.Columns.Add("P.O. No.", typeof(string));
        dt.Columns.Add("Services", typeof(string));
        dt.Columns.Add("Code No.", typeof(string));
        dt.Columns.Add("QTY", typeof(double));
        dt.Columns.Add("UNIT PRICE", typeof(double));
        dt.Columns.Add("PER", typeof(string));
        dt.Columns.Add("PROJECT NAME", typeof(string));
        dt.Columns.Add("CONTAINER ID # / BOX #", typeof(string));
        dt.Columns.Add("IMAGES", typeof(string));
        dt.Columns.Add("CURRENCY", typeof(string));
        dt.Columns.Add("INVOICE STATUS", typeof(string));
        dt.Columns.Add("ISDELETED", typeof(string));
        dt.Columns.Add("ISREVISION", typeof(string));


        DataRow drFirst = dt.NewRow();
        drFirst["NAME OF THE PARTY"] = "DEPARTMENTWISE E-INVOICES :";
        dt.Rows.Add(drFirst);

        string PrvDept = string.Empty;

        DateTime CDate = Convert.ToDateTime(DateTime.ParseExact(txtMonth.Text, "MMM-yyyy", null).ToString("MM/dd/yyyy"));
        var Invoice = from DBInvoice in dbobj.InvoiceMasters
                          //where DBInvoice.IsRevised == false && DBInvoice.InvoiceEndDate.Month == CDate.Month && DBInvoice.InvoiceStartDate.Year == CDate.Year
                      where DBInvoice.IsRevised == false && DBInvoice.InvoiceDate.Month == CDate.Month && DBInvoice.InvoiceDate.Year == CDate.Year //// Change on 27-Aug-2021 by Jignesh to set Invoice Date instead of EndDate 
                      orderby DBInvoice.InvoiceNumber
                      select DBInvoice;
        foreach (var inv in Invoice)
        {
            string Dept = inv.ClientMaster.DepartmentMaster.DepartmentName;
            if (PrvDept != Dept)
            {
                DataRow drBblank = dt.NewRow();
                dt.Rows.Add(drBblank);

                DataRow drDept = dt.NewRow();
                drDept["NAME OF THE PARTY"] = Dept;
                PrvDept = Dept;
                dt.Rows.Add(drDept);
            }

            Boolean isFirst = true;
            var InvoiceDetails = from DBDetails in dbobj.InvoiceDetailsMasters
                                 where DBDetails.InvoiceId == inv.InvoiceId && DBDetails.Qty != null && DBDetails.Qty != 0
                                 select DBDetails;
            foreach (var invDetail in InvoiceDetails)
            {
                DataRow drDetail = dt.NewRow();
                if (isFirst == true)
                {
                    drDetail["NAME OF THE PARTY"] = invDetail.InvoiceMaster.ClientMaster.ClientName;
                    drDetail["INV #"] = (int)invDetail.InvoiceMaster.InvoiceSeqNo;
                    ////drDetail["DATE"] = invDetail.InvoiceMaster.InvoiceEndDate.ToString("MM/dd/yyyy");
                    drDetail["DATE"] = invDetail.InvoiceMaster.InvoiceDate.ToString("MM/dd/yyyy"); //// Change on 14-Sep-2020 by Jignesh to set Invoice Date instead of EndDate 
                    drDetail["AMOUNT"] = (double)Math.Round(Convert.ToDecimal(((from DBData in dbobj.InvoiceDetailsMasters
                                                                                where DBData.InvoiceId == inv.InvoiceId
                                                                                select DBData.TotalAmt.Value).Count() == 0 ? "0.00" : Convert.ToString(((from DBData in dbobj.InvoiceDetailsMasters
                                                                                                                                                         where DBData.InvoiceId == inv.InvoiceId
                                                                                                                                                         select DBData.TotalAmt).Sum())))), 2, MidpointRounding.ToEven);
                    drDetail["Sub-Amt."] = (double)invDetail.TotalAmt;
                    drDetail["P.O. No."] = invDetail.InvoiceMaster.PONumber;
                    drDetail["Services"] = invDetail.ItemDesc;
                    ////drDetail["Code No."] = invDetail.InvoiceMaster.InvoiceEndDate.ToString("MMdd") + Dept.Substring(0, 1).ToUpper() + invDetail.InvoiceMaster.InvoiceSeqNo + invDetail.InvoiceMaster.InvoiceEndDate.ToString("yy");
                    drDetail["Code No."] = invDetail.InvoiceMaster.InvoiceDate.ToString("MMdd") + Dept.Substring(0, 1).ToUpper() + invDetail.InvoiceMaster.InvoiceSeqNo + invDetail.InvoiceMaster.InvoiceDate.ToString("yy"); //// Change on 14-Sep-2020 by Jignesh to set Invoice Date instead of EndDate 
                    drDetail["QTY"] = (double)invDetail.Qty;
                    drDetail["UNIT PRICE"] = (double)invDetail.UnitPrice;
                    drDetail["PER"] = invDetail.PriceType;
                    drDetail["PROJECT NAME"] = "";
                    drDetail["CONTAINER ID # / BOX #"] = "";
                    drDetail["IMAGES"] = "";
                    drDetail["CURRENCY"] = invDetail.InvoiceMaster.ClientMaster.CurrencyMaster.CurrencyCode;
                    drDetail["INVOICE STATUS"] = invDetail.InvoiceMaster.InvoiceStatus.ToString().ToUpper();
                    drDetail["ISDELETED"] = invDetail.InvoiceMaster.IsDeleted.ToString();
                    if (invDetail.InvoiceMaster.Revision != null)
                        drDetail["ISREVISION"] = invDetail.InvoiceMaster.Revision == null ? string.Empty : invDetail.InvoiceMaster.Revision.ToString();

                    isFirst = false;
                }
                else
                {
                    drDetail["NAME OF THE PARTY"] = invDetail.InvoiceMaster.ClientMaster.ClientName;
                    drDetail["INV #"] = 0;
                    ////drDetail["DATE"] = invDetail.InvoiceMaster.InvoiceEndDate.ToString("MM/dd/yyyy");
                    drDetail["DATE"] = invDetail.InvoiceMaster.InvoiceDate.ToString("MM/dd/yyyy"); //// Change on 14-Sep-2020 by Jignesh to set Invoice Date instead of EndDate 
                    drDetail["AMOUNT"] = 0.00;
                    drDetail["Sub-Amt."] = (double)invDetail.TotalAmt;
                    drDetail["P.O. No."] = invDetail.InvoiceMaster.PONumber;
                    drDetail["Services"] = invDetail.ItemDesc;
                    ////drDetail["Code No."] = invDetail.InvoiceMaster.InvoiceEndDate.ToString("MMdd") + Dept.Substring(0, 1).ToUpper() + invDetail.InvoiceMaster.InvoiceSeqNo + invDetail.InvoiceMaster.InvoiceEndDate.ToString("yy");
                    drDetail["Code No."] = invDetail.InvoiceMaster.InvoiceDate.ToString("MMdd") + Dept.Substring(0, 1).ToUpper() + invDetail.InvoiceMaster.InvoiceSeqNo + invDetail.InvoiceMaster.InvoiceDate.ToString("yy"); //// Change on 14-Sep-2020 by Jignesh to set Invoice Date instead of EndDate 
                    drDetail["QTY"] = (double)invDetail.Qty;
                    drDetail["UNIT PRICE"] = (double)invDetail.UnitPrice;
                    drDetail["PER"] = invDetail.PriceType;
                    drDetail["PROJECT NAME"] = "";
                    drDetail["CONTAINER ID # / BOX #"] = "";
                    drDetail["IMAGES"] = "";
                    drDetail["CURRENCY"] = invDetail.InvoiceMaster.ClientMaster.CurrencyMaster.CurrencyCode;
                    drDetail["INVOICE STATUS"] = invDetail.InvoiceMaster.InvoiceStatus.ToString().ToUpper();
                    drDetail["ISDELETED"] = invDetail.InvoiceMaster.IsDeleted.ToString();
                    if (invDetail.InvoiceMaster.Revision != null)
                        drDetail["ISREVISION"] = invDetail.InvoiceMaster.Revision == null ? string.Empty : invDetail.InvoiceMaster.Revision.ToString();

                }
                dt.Rows.Add(drDetail);
            }

            //// Added on 26-Nov-2021 by JM: View invoice records if invoice details not found
            #region View invoice records if invoice details not found
            if (InvoiceDetails.Count() == 0)
            {
                DataRow drDetail = dt.NewRow();
                //if (isFirst == true)
                //{
                drDetail["NAME OF THE PARTY"] = inv.ClientMaster.ClientName;
                drDetail["INV #"] = (int)inv.InvoiceSeqNo;
                ////drDetail["DATE"] = invDetail.InvoiceMaster.InvoiceEndDate.ToString("MM/dd/yyyy");
                drDetail["DATE"] = inv.InvoiceDate.ToString("MM/dd/yyyy"); //// Change on 14-Sep-2020 by Jignesh to set Invoice Date instead of EndDate 
                drDetail["AMOUNT"] = (double)Math.Round(Convert.ToDecimal(((from DBData in dbobj.InvoiceDetailsMasters
                                                                            where DBData.InvoiceId == inv.InvoiceId
                                                                            select DBData.TotalAmt.Value).Count() == 0 ? "0.00" : Convert.ToString(((from DBData in dbobj.InvoiceDetailsMasters
                                                                                                                                                     where DBData.InvoiceId == inv.InvoiceId
                                                                                                                                                     select DBData.TotalAmt).Sum())))), 2, MidpointRounding.ToEven);
                drDetail["Sub-Amt."] = 0.00;
                drDetail["P.O. No."] = inv.PONumber;
                drDetail["Services"] = "";
                ////drDetail["Code No."] = invDetail.InvoiceMaster.InvoiceEndDate.ToString("MMdd") + Dept.Substring(0, 1).ToUpper() + invDetail.InvoiceMaster.InvoiceSeqNo + invDetail.InvoiceMaster.InvoiceEndDate.ToString("yy");
                drDetail["Code No."] = inv.InvoiceDate.ToString("MMdd") + Dept.Substring(0, 1).ToUpper() + inv.InvoiceSeqNo + inv.InvoiceDate.ToString("yy"); //// Change on 14-Sep-2020 by Jignesh to set Invoice Date instead of EndDate 
                drDetail["QTY"] = 0.00;
                drDetail["UNIT PRICE"] = 0.00;
                drDetail["PER"] = "";
                drDetail["PROJECT NAME"] = "";
                drDetail["CONTAINER ID # / BOX #"] = "";
                drDetail["IMAGES"] = "";
                drDetail["CURRENCY"] = inv.ClientMaster.CurrencyMaster.CurrencyCode;
                drDetail["INVOICE STATUS"] = inv.InvoiceStatus.ToString().ToUpper();
                drDetail["ISDELETED"] = inv.IsDeleted.ToString();
                if (inv.Revision != null)
                    drDetail["ISREVISION"] = inv.Revision == null ? string.Empty : inv.Revision.ToString();
                //    isFirst = false;
                // }
                dt.Rows.Add(drDetail);
            }
            #endregion

        }

        if (dt.Rows.Count > 0)
        {

            gvMonthlySales.DataSource = dt;
            gvMonthlySales.DataBind();

            gvMonthlySales.HeaderRow.Cells[16].Visible = IsShowStatusCol; //INVOICE STATUS column hide
            gvMonthlySales.FooterRow.Cells[16].Visible = IsShowStatusCol; //INVOICE STATUS column hide
            gvMonthlySales.HeaderRow.Cells[17].Visible = IsShowDelCol; //ISDELETED column hide
            gvMonthlySales.FooterRow.Cells[17].Visible = IsShowDelCol; //ISDELETED column hide
            gvMonthlySales.HeaderRow.Cells[18].Visible = IsShowRevisionCol; //ISREVISION column hide
            gvMonthlySales.FooterRow.Cells[18].Visible = IsShowRevisionCol; //ISREVISION column hide

            btnExcel.Enabled = true;
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


    protected void btnExcel_Click(object sender, EventArgs e)
    {
        DateTime CDate = Convert.ToDateTime(DateTime.ParseExact(txtMonth.Text, "MMM-yyyy", null).ToString("MM/dd/yyyy"));

        Aspose.Cells.License lic = new Aspose.Cells.License();
        string licPath = HttpContext.Current.Server.MapPath("Bin");
        lic.SetLicense(licPath + @"\aspose.lic");
        Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
        Aspose.Cells.Worksheet ws = wb.Worksheets[0];
        ws.Name = txtMonth.Text;

        // Style Header
        Aspose.Cells.Style styleHeader = ws.Cells[0, 50].GetStyle();
        styleHeader.HorizontalAlignment = TextAlignmentType.Center;
        styleHeader.Font.IsBold = true;

        // Style SubHeader
        Aspose.Cells.Style styleSubHeader = ws.Cells[0, 50].GetStyle();
        styleSubHeader.HorizontalAlignment = TextAlignmentType.Center;
        styleSubHeader.Font.IsBold = true;
        styleSubHeader.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
        styleSubHeader.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
        styleSubHeader.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
        styleSubHeader.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;

        // Style Department Header
        Aspose.Cells.Style styleDeptHeader = ws.Cells[0, 50].GetStyle();
        styleSubHeader.HorizontalAlignment = TextAlignmentType.Left;
        styleDeptHeader.Font.IsBold = true;
        styleDeptHeader.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
        styleDeptHeader.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
        styleDeptHeader.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
        styleDeptHeader.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;

        // Style Details Item
        Aspose.Cells.Style styleItem = ws.Cells[0, 50].GetStyle();
        styleItem.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
        styleItem.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
        styleItem.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
        styleItem.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;

        // Style Currency
        Aspose.Cells.Style styleAmountCurr = ws.Cells[0, 50].GetStyle();
        styleAmountCurr.Number = 1;
        styleAmountCurr.Font.IsBold = true;
        //styleAmountCurr.ForegroundColor = System.Drawing.Color.Yellow;
        //styleAmountCurr.Pattern = BackgroundType.Solid;
        styleAmountCurr.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
        styleAmountCurr.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
        styleAmountCurr.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
        styleAmountCurr.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;

        // Style Department header cell color
        Aspose.Cells.Style styleDeptHeadeColor = ws.Cells[0, 50].GetStyle();
        styleDeptHeadeColor.Font.IsBold = true;
        styleDeptHeadeColor.ForegroundColor = System.Drawing.Color.Green;
        styleDeptHeadeColor.Pattern = BackgroundType.Solid;
        styleDeptHeadeColor.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Medium;
        styleDeptHeadeColor.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Medium;
        styleDeptHeadeColor.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Medium;
        styleDeptHeadeColor.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Medium;

        // Style Department cell color
        Aspose.Cells.Style styleDeptColor = ws.Cells[0, 50].GetStyle();
        styleDeptColor.Font.IsBold = true;
        styleDeptColor.ForegroundColor = System.Drawing.Color.YellowGreen;
        styleDeptColor.Pattern = BackgroundType.Solid;
        styleDeptColor.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Medium;
        styleDeptColor.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Medium;
        styleDeptColor.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Medium;
        styleDeptColor.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Medium;




        // Mangalam USA
        ws.Cells[1, 0].Value = "SALES INVOICES  FOR THE MONTH OF " + txtMonth.Text + "  -          MANGALAM INFOTECCH USA";
        ws.Cells.Merge(1, 0, 1, 16);
        ws.Cells[1, 0].SetStyle(styleHeader);

        ws.Cells[2, 16].Value = "MITL to MIT-USA";
        ws.Cells.Merge(2, 16, 1, 3);
        // Set AStyle MIT to USA
        Aspose.Cells.Range rngMITtoUSA = ws.Cells.CreateRange(2, 16, 1, 3);
        rngMITtoUSA.SetStyle(styleSubHeader);

        ws.Cells[3, 1].Value = "S.N";
        ws.Cells[3, 2].Value = "NAME OF THE PARTY";
        ws.Cells[3, 3].Value = "INV #";
        ws.Cells[3, 4].Value = "DATE";
        ws.Cells[3, 5].Value = " AMOUNT";
        ws.Cells[3, 6].Value = "Sub-Amt.";
        ws.Cells[3, 7].Value = " P.O. No.";
        ws.Cells[3, 8].Value = "Services";
        ws.Cells[3, 9].Value = "Code No.";
        ws.Cells[3, 10].Value = "QTY";
        ws.Cells[3, 11].Value = "UNIT PRICE";
        ws.Cells[3, 12].Value = "PER";
        ws.Cells[3, 13].Value = "PROJECT NAME";
        ws.Cells[3, 14].Value = "CONTAINER ID # / BOX #";
        ws.Cells[3, 15].Value = "IMAGES";
        ws.Cells[3, 16].Value = "Qty";
        ws.Cells[3, 17].Value = "Rate";
        ws.Cells[3, 18].Value = "US$";

        // Set sub Header style
        Aspose.Cells.Range rngSubHeader = ws.Cells.CreateRange(3, 1, 1, 19);
        rngSubHeader.SetStyle(styleSubHeader);

        ws.Cells[4, 2].Value = "DEPARTMENTWISE E-INVOICES :";
        // Set Department Header Style
        Aspose.Cells.Range rngDeptHeader = ws.Cells.CreateRange(4, 1, 1, 19);
        rngDeptHeader.SetStyle(styleSubHeader);

        int rval = 5;
        int[] DeptRVal = new int[20];
        int arrayIndex = 0;
        string PrvDept = string.Empty;
        Boolean FirstDept = true;


        var Invoice = from DBInvoice in dbobj.InvoiceMasters
                      where DBInvoice.IsRevised == false && DBInvoice.InvoiceFor == "USA" && DBInvoice.InvoiceDate.Month == CDate.Month && DBInvoice.InvoiceDate.Year == CDate.Year
                      orderby DBInvoice.InvoiceNumber
                      select DBInvoice;
        foreach (var inv in Invoice)
        {
            string Dept = inv.ClientMaster.DepartmentMaster.DepartmentName;
            if (PrvDept != Dept)
            {
                arrayIndex++;
                if (FirstDept == true)
                {
                    ws.Cells[rval, 2].Value = Dept;
                    // Set Department Header Style
                    Aspose.Cells.Range rngDept = ws.Cells.CreateRange(rval, 1, 1, 19);
                    rngDept.SetStyle(styleSubHeader);

                    PrvDept = Dept;
                    DeptRVal[arrayIndex] = rval; rval++;
                    FirstDept = false;
                }
                else
                {
                    Aspose.Cells.Range rngBlanck = ws.Cells.CreateRange(rval, 1, 1, 19);
                    rngBlanck.SetStyle(styleSubHeader);
                    rval++;

                    ws.Cells[rval, 2].Value = Dept;
                    // Set Department Header Style
                    Aspose.Cells.Range rngDept = ws.Cells.CreateRange(rval, 1, 1, 19);
                    rngDept.SetStyle(styleSubHeader);

                    PrvDept = Dept;
                    DeptRVal[arrayIndex] = rval;
                    rval++;
                }
            }

            Boolean isFirst = true;
            var InvoiceDetails = from DBDetails in dbobj.InvoiceDetailsMasters
                                 where DBDetails.InvoiceId == inv.InvoiceId && DBDetails.Qty != null && DBDetails.Qty != 0
                                 select DBDetails;
            foreach (var invDetail in InvoiceDetails)
            {
                if (isFirst == true)
                {
                    ws.Cells[rval, 2].Value = invDetail.InvoiceMaster.ClientMaster.ClientName;
                    ws.Cells[rval, 3].Value = (int)invDetail.InvoiceMaster.InvoiceSeqNo;

                    //// Change on 14-Sep-2020 by Jignesh to set Invoice Date instead of EndDate 
                    ////ws.Cells[rval, 4].Value = invDetail.InvoiceMaster.InvoiceEndDate.ToString("MM/dd/yyyy");
                    ws.Cells[rval, 4].Value = invDetail.InvoiceMaster.InvoiceDate.ToString("MM/dd/yyyy");

                    // Set Item Style
                    Aspose.Cells.Range rngItem = ws.Cells.CreateRange(rval, 1, 1, 4);
                    rngItem.SetStyle(styleItem);
                    //Set New Invoice Style
                    Aspose.Cells.Style NewStyle = ws.Cells[(rval), 4].GetStyle();
                    NewStyle.ForegroundColor = System.Drawing.Color.Yellow;
                    NewStyle.Pattern = BackgroundType.Solid;
                    Aspose.Cells.Range rngNew = ws.Cells.CreateRange(rval, 3, 1, 1);
                    rngNew.SetStyle(NewStyle);

                    //// Added on 03-Dec-2021 by JM: Deleted invoice highlighted with red color (Mangalam USA)
                    if (invDetail.InvoiceMaster.IsDeleted == true)
                    {
                        Aspose.Cells.Style deleteStyle = ws.Cells[rval, 3].GetStyle();
                        deleteStyle.ForegroundColor = System.Drawing.Color.Red;
                        ws.Cells[rval, 3].SetStyle(deleteStyle);
                    }
                    else if (!string.IsNullOrEmpty(invDetail.InvoiceMaster.Revision))
                    {
                        //// Added on 14-Feb-2022 by JM: Deleted invoice highlighted with red color (Mangalam USA)
                        Aspose.Cells.Style RevisedStyle = ws.Cells[rval, 3].GetStyle();
                        RevisedStyle.ForegroundColor = System.Drawing.Color.LightSkyBlue;
                        ws.Cells[rval, 3].SetStyle(RevisedStyle);
                    }


                    ws.Cells[rval, 5].Value = (double)Math.Round(Convert.ToDecimal(((from DBData in dbobj.InvoiceDetailsMasters
                                                                                     where DBData.InvoiceId == inv.InvoiceId
                                                                                     select DBData.TotalAmt.Value).Count() == 0 ? "0.00" : Convert.ToString(((from DBData in dbobj.InvoiceDetailsMasters
                                                                                                                                                              where DBData.InvoiceId == inv.InvoiceId
                                                                                                                                                              select DBData.TotalAmt).Sum())))), 2, MidpointRounding.ToEven);
                    ws.Cells[rval, 6].Value = (double)invDetail.TotalAmt;
                    //Set Currency Style
                    styleAmountCurr.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (inv.ClientMaster.CurrencyMaster.CurrencySymbol + ";").Replace(";;", ";")) + "]0.00";
                    Aspose.Cells.Range rngAmountCurr = ws.Cells.CreateRange(rval, 5, 1, 2);
                    rngAmountCurr.SetStyle(styleAmountCurr);

                    ws.Cells[rval, 7].Value = invDetail.InvoiceMaster.PONumber;
                    ws.Cells[rval, 8].Value = invDetail.ItemDesc;
                    //// Change on 14-Sep-2020 by Jignesh to set Invoice Date instead of EndDate 
                    ////ws.Cells[rval, 9].Value = invDetail.InvoiceMaster.InvoiceEndDate.ToString("MMdd") + Dept.Substring(0, 1).ToUpper() + invDetail.InvoiceMaster.InvoiceSeqNo + invDetail.InvoiceMaster.InvoiceEndDate.ToString("yy");
                    ws.Cells[rval, 9].Value = invDetail.InvoiceMaster.InvoiceDate.ToString("MMdd") + Dept.Substring(0, 1).ToUpper() + invDetail.InvoiceMaster.InvoiceSeqNo + invDetail.InvoiceMaster.InvoiceDate.ToString("yy");
                    ws.Cells[rval, 10].Value = (double)invDetail.Qty;
                    ws.Cells[rval, 11].Value = (double)invDetail.UnitPrice;
                    ws.Cells[rval, 12].Value = invDetail.PriceType;
                    ws.Cells[rval, 13].Value = "";
                    ws.Cells[rval, 14].Value = "";
                    ws.Cells[rval, 15].Value = "";
                    ws.Cells[rval, 16].Formula = "=K" + (rval + 1);
                    ws.Cells[rval, 17].Formula = "=L" + (rval + 1) + "*50%";
                    ws.Cells[rval, 18].Formula = "=Q" + (rval + 1) + "*R" + (rval + 1);
                    // Set Item Style
                    Aspose.Cells.Range rngItem2 = ws.Cells.CreateRange(rval, 7, 1, 13);
                    rngItem2.SetStyle(styleItem);

                    rval++;
                    isFirst = false;
                }
                else
                {
                    ws.Cells[rval, 2].Value = invDetail.InvoiceMaster.ClientMaster.ClientName;
                    ws.Cells[rval, 3].Value = 0;
                    //// Change on 14-Sep-2020 by Jignesh to set Invoice Date instead of EndDate 
                    ////ws.Cells[rval, 4].Value = invDetail.InvoiceMaster.InvoiceEndDate.ToString("MM/dd/yyyy");
                    ws.Cells[rval, 4].Value = invDetail.InvoiceMaster.InvoiceDate.ToString("MM/dd/yyyy");
                    // Set Item Style
                    Aspose.Cells.Range rngItem = ws.Cells.CreateRange(rval, 1, 1, 4);
                    rngItem.SetStyle(styleItem);

                    //// Added on 03-Dec-2021 by JM: Deleted invoice highlighted with red color (Mangalam USA)
                    if (invDetail.InvoiceMaster.IsDeleted == true)
                    {
                        Aspose.Cells.Style deleteStyle = ws.Cells[rval, 3].GetStyle();
                        deleteStyle.ForegroundColor = System.Drawing.Color.Red;
                        ws.Cells[rval, 3].SetStyle(deleteStyle);
                    }
                    else if (!string.IsNullOrEmpty(invDetail.InvoiceMaster.Revision))
                    {
                        //// Added on 14-Feb-2022 by JM: Deleted invoice highlighted with red color (Mangalam USA)
                        Aspose.Cells.Style RevisedStyle = ws.Cells[rval, 3].GetStyle();
                        RevisedStyle.ForegroundColor = System.Drawing.Color.LightSkyBlue;
                        ws.Cells[rval, 3].SetStyle(RevisedStyle);
                    }

                    ws.Cells[rval, 5].Value = (double)0.00;
                    ws.Cells[rval, 6].Value = (double)invDetail.TotalAmt;
                    //Set Currency Style
                    styleAmountCurr.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (inv.ClientMaster.CurrencyMaster.CurrencySymbol + ";").Replace(";;", ";")) + "]0.00";
                    Aspose.Cells.Range rngAmountCurr = ws.Cells.CreateRange(rval, 5, 1, 2);
                    rngAmountCurr.SetStyle(styleAmountCurr);

                    ws.Cells[rval, 7].Value = invDetail.InvoiceMaster.PONumber;
                    ws.Cells[rval, 8].Value = invDetail.ItemDesc;
                    //// Change on 14-Sep-2020 by Jignesh to set Invoice Date instead of EndDate 
                    ////ws.Cells[rval, 9].Value = invDetail.InvoiceMaster.InvoiceEndDate.ToString("MMdd") + Dept.Substring(0, 1).ToUpper() + invDetail.InvoiceMaster.InvoiceSeqNo + invDetail.InvoiceMaster.InvoiceEndDate.ToString("yy");
                    ws.Cells[rval, 9].Value = invDetail.InvoiceMaster.InvoiceDate.ToString("MMdd") + Dept.Substring(0, 1).ToUpper() + invDetail.InvoiceMaster.InvoiceSeqNo + invDetail.InvoiceMaster.InvoiceDate.ToString("yy");
                    ws.Cells[rval, 10].Value = (double)invDetail.Qty;
                    ws.Cells[rval, 11].Value = (double)invDetail.UnitPrice;
                    ws.Cells[rval, 12].Value = invDetail.PriceType;
                    ws.Cells[rval, 13].Value = "";
                    ws.Cells[rval, 14].Value = "";
                    ws.Cells[rval, 15].Value = "";
                    ws.Cells[rval, 16].Formula = "=K" + (rval + 1);
                    ws.Cells[rval, 17].Formula = "=L" + (rval + 1) + "*50%";
                    ws.Cells[rval, 18].Formula = "=Q" + (rval + 1) + "*R" + (rval + 1);
                    // Set Item Style
                    Aspose.Cells.Range rngItem2 = ws.Cells.CreateRange(rval, 7, 1, 13);
                    rngItem2.SetStyle(styleItem);

                    rval++;
                }
            }

            //// Added on 02-Dec-2021 by JM: View invoice records if invoice details not found (Mangalam USA)
            #region View invoice records if invoice details not found (Mangalam USA)
            if (InvoiceDetails.Count() == 0)
            {
                ws.Cells[rval, 2].Value = inv.ClientMaster.ClientName;
                ws.Cells[rval, 3].Value = (int)inv.InvoiceSeqNo;
                //// Change on 14-Sep-2020 by Jignesh to set Invoice Date instead of EndDate 
                ////ws.Cells[rval, 4].Value = invDetail.InvoiceMaster.InvoiceEndDate.ToString("MM/dd/yyyy");
                ws.Cells[rval, 4].Value = inv.InvoiceDate.ToString("MM/dd/yyyy");

                // Set Item Style
                Aspose.Cells.Range rngItem = ws.Cells.CreateRange(rval, 1, 1, 4);
                rngItem.SetStyle(styleItem);
                //Set New Invoice Style
                Aspose.Cells.Style NewStyle = ws.Cells[(rval), 4].GetStyle();
                NewStyle.ForegroundColor = System.Drawing.Color.Yellow;
                NewStyle.Pattern = BackgroundType.Solid;
                Aspose.Cells.Range rngNew = ws.Cells.CreateRange(rval, 3, 1, 1);
                rngNew.SetStyle(NewStyle);

                //// Added on 03-Dec-2021 by JM: Deleted invoice highlighted with red color (Mangalam USA)
                if (inv.IsDeleted == true)
                {
                    Aspose.Cells.Style deleteStyle = ws.Cells[rval, 3].GetStyle();
                    deleteStyle.ForegroundColor = System.Drawing.Color.Red;
                    ws.Cells[rval, 3].SetStyle(deleteStyle);
                }
                else if (!string.IsNullOrEmpty(inv.Revision))
                {
                    //// Added on 14-Feb-2022 by JM: Deleted invoice highlighted with red color (Mangalam USA)
                    Aspose.Cells.Style RevisedStyle = ws.Cells[rval, 3].GetStyle();
                    RevisedStyle.ForegroundColor = System.Drawing.Color.LightSkyBlue;
                    ws.Cells[rval, 3].SetStyle(RevisedStyle);
                }

                ws.Cells[rval, 5].Value = (double)Math.Round(Convert.ToDecimal(((from DBData in dbobj.InvoiceDetailsMasters
                                                                                 where DBData.InvoiceId == inv.InvoiceId
                                                                                 select DBData.TotalAmt.Value).Count() == 0 ? "0.00" : Convert.ToString(((from DBData in dbobj.InvoiceDetailsMasters
                                                                                                                                                          where DBData.InvoiceId == inv.InvoiceId
                                                                                                                                                          select DBData.TotalAmt).Sum())))), 2, MidpointRounding.ToEven);
                ws.Cells[rval, 6].Value = 0.00;
                //Set Currency Style
                styleAmountCurr.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (inv.ClientMaster.CurrencyMaster.CurrencySymbol + ";").Replace(";;", ";")) + "]0.00";
                Aspose.Cells.Range rngAmountCurr = ws.Cells.CreateRange(rval, 5, 1, 2);
                rngAmountCurr.SetStyle(styleAmountCurr);

                ws.Cells[rval, 7].Value = inv.PONumber;
                ws.Cells[rval, 8].Value = "";
                //// Change on 14-Sep-2020 by Jignesh to set Invoice Date instead of EndDate 
                ////ws.Cells[rval, 9].Value = invDetail.InvoiceMaster.InvoiceEndDate.ToString("MMdd") + Dept.Substring(0, 1).ToUpper() + invDetail.InvoiceMaster.InvoiceSeqNo + invDetail.InvoiceMaster.InvoiceEndDate.ToString("yy");
                ws.Cells[rval, 9].Value = inv.InvoiceDate.ToString("MMdd") + Dept.Substring(0, 1).ToUpper() + inv.InvoiceSeqNo + inv.InvoiceDate.ToString("yy");
                ws.Cells[rval, 10].Value = 0.00;
                ws.Cells[rval, 11].Value = 0.00;
                ws.Cells[rval, 12].Value = "";
                ws.Cells[rval, 13].Value = "";
                ws.Cells[rval, 14].Value = "";
                ws.Cells[rval, 15].Value = "";
                ws.Cells[rval, 16].Formula = "=K" + (rval + 1);
                ws.Cells[rval, 17].Formula = "=L" + (rval + 1) + "*50%";
                ws.Cells[rval, 18].Formula = "=Q" + (rval + 1) + "*R" + (rval + 1);
                // Set Item Style
                Aspose.Cells.Range rngItem2 = ws.Cells.CreateRange(rval, 7, 1, 13);
                rngItem2.SetStyle(styleItem);

                rval++;
                //isFirst = false;
            }
            #endregion
        }

        ws.Cells[rval, 5].Formula = "=SUM(F7:F" + rval + ")";
        ws.Cells[rval, 18].Formula = "=SUM(S7:S" + rval + ")";

        // Set Item Style
        Aspose.Cells.Range rngTotal = ws.Cells.CreateRange(rval, 5, 1, 1);
        rngTotal.SetStyle(styleAmountCurr);
        Aspose.Cells.Range rngTotal1 = ws.Cells.CreateRange(rval, 1, 1, 4);
        rngTotal1.SetStyle(styleItem);
        Aspose.Cells.Range rngTotal2 = ws.Cells.CreateRange(rval, 6, 1, 12);
        rngTotal2.SetStyle(styleItem);
        Aspose.Cells.Range rngTotal3 = ws.Cells.CreateRange(rval, 18, 1, 1);
        rngTotal3.SetStyle(styleAmountCurr);
        Aspose.Cells.Range rngTotal4 = ws.Cells.CreateRange(rval, 19, 1, 1);
        rngTotal4.SetStyle(styleItem);


        int rvalTotal = rval + 2;
        ws.Cells[(rval + 2), 5].Formula = "=F" + (rval + 1) + "*50/100";
        ws.Cells[(rval + 2), 7].Formula = "=S" + (rval + 1) + "*2";
        ws.Cells[(rval + 2), 5].SetStyle(styleAmountCurr);
        int rvalT = rval + 2;




        // Mangalam India
        rval = rval + 4;

        ws.Cells[rval, 0].Value = "SALES INVOICE FOR THE MONTH OF " + txtMonth.Text + "  -  MANGALAM INFORMATION TECHNOLOGIES PVT. LTD. AHMEDABAD";
        ws.Cells.Merge(rval, 0, 1, 16);
        ws.Cells[rval, 0].SetStyle(styleHeader);

        rval = rval + 2;
        int RvalIdia = rval;
        ws.Cells[rval, 1].Value = "S.N";
        ws.Cells[rval, 2].Value = "NAME OF THE PARTY";
        ws.Cells[rval, 3].Value = "INV #";
        ws.Cells[rval, 4].Value = "DATE";
        ws.Cells[rval, 5].Value = " AMOUNT";
        ws.Cells[rval, 6].Value = "Sub-Amt.";
        ws.Cells[rval, 7].Value = " P.O. No.";
        ws.Cells[rval, 8].Value = "Services";
        ws.Cells[rval, 9].Value = "Code No.";
        ws.Cells[rval, 10].Value = "QTY";
        ws.Cells[rval, 11].Value = "UNIT PRICE";
        ws.Cells[rval, 12].Value = "PER";
        ws.Cells[rval, 13].Value = "PROJECT NAME";
        ws.Cells[rval, 14].Value = "CONTAINER ID # / BOX #";
        ws.Cells[rval, 15].Value = "IMAGES";

        // Set sub Header style
        Aspose.Cells.Range rngSubHeaderI = ws.Cells.CreateRange(rval, 1, 1, 19);
        rngSubHeaderI.SetStyle(styleSubHeader);


        rval++;
        int rvalIndia = rval + 1;
        string PrvParty = string.Empty;
        Boolean FirstParty = true;

        var InvoiceIdia = from DBInvoice in dbobj.InvoiceMasters
                          where DBInvoice.IsRevised == false && DBInvoice.InvoiceFor == "INDIA" && DBInvoice.InvoiceEndDate.Month == CDate.Month && DBInvoice.InvoiceStartDate.Year == CDate.Year
                          orderby DBInvoice.InvoiceNumber
                          select DBInvoice;
        foreach (var inv in InvoiceIdia)
        {
            string Party = inv.ClientMaster.ClientName;
            if (PrvParty != Party)
            {
                if (FirstParty == true)
                    FirstParty = false;
                else
                {
                    Aspose.Cells.Range rngBlanck = ws.Cells.CreateRange(rval, 1, 1, 19);
                    rngBlanck.SetStyle(styleSubHeader);
                    rval++;
                    PrvParty = inv.ClientMaster.ClientName;
                }
            }

            Boolean isFirst = true;
            var InvoiceDetails = from DBDetails in dbobj.InvoiceDetailsMasters
                                 where DBDetails.InvoiceId == inv.InvoiceId && DBDetails.Qty != null && DBDetails.Qty != 0
                                 select DBDetails;
            foreach (var invDetail in InvoiceDetails)
            {
                if (isFirst == true)
                {
                    ws.Cells[rval, 2].Value = invDetail.InvoiceMaster.ClientMaster.ClientName;
                    ws.Cells[rval, 3].Value = (int)invDetail.InvoiceMaster.InvoiceSeqNo;
                    //// Change on 14-Sep-2020 by Jignesh to set Invoice Date instead of EndDate 
                    ////ws.Cells[rval, 4].Value = invDetail.InvoiceMaster.InvoiceEndDate.ToString("MM/dd/yyyy");
                    ws.Cells[rval, 4].Value = invDetail.InvoiceMaster.InvoiceDate.ToString("MM/dd/yyyy");
                    // Set Item Style
                    Aspose.Cells.Range rngItem = ws.Cells.CreateRange(rval, 1, 1, 4);
                    rngItem.SetStyle(styleItem);
                    //Set New Invoice Style
                    Aspose.Cells.Style NewStyle = ws.Cells[(rval), 4].GetStyle();
                    NewStyle.ForegroundColor = System.Drawing.Color.Yellow;
                    NewStyle.Pattern = BackgroundType.Solid;
                    Aspose.Cells.Range rngNew = ws.Cells.CreateRange(rval, 3, 1, 1);
                    rngNew.SetStyle(NewStyle);

                    //// Added on 03-Dec-2021 by JM: Deleted invoice highlighted with red color (Mangalam USA)
                    if (invDetail.InvoiceMaster.IsDeleted == true)
                    {
                        Aspose.Cells.Style deleteStyle = ws.Cells[rval, 3].GetStyle();
                        deleteStyle.ForegroundColor = System.Drawing.Color.Red;
                        ws.Cells[rval, 3].SetStyle(deleteStyle);
                    }
                    else if (!string.IsNullOrEmpty(invDetail.InvoiceMaster.Revision))
                    {
                        //// Added on 14-Feb-2022 by JM: Deleted invoice highlighted with red color (Mangalam USA)
                        Aspose.Cells.Style RevisedStyle = ws.Cells[rval, 3].GetStyle();
                        RevisedStyle.ForegroundColor = System.Drawing.Color.LightSkyBlue;
                        ws.Cells[rval, 3].SetStyle(RevisedStyle);
                    }

                    ws.Cells[rval, 5].Value = (double)Math.Round(Convert.ToDecimal(((from DBData in dbobj.InvoiceDetailsMasters
                                                                                     where DBData.InvoiceId == inv.InvoiceId
                                                                                     select DBData.TotalAmt.Value).Count() == 0 ? "0.00" : Convert.ToString(((from DBData in dbobj.InvoiceDetailsMasters
                                                                                                                                                              where DBData.InvoiceId == inv.InvoiceId
                                                                                                                                                              select DBData.TotalAmt).Sum())))), 2, MidpointRounding.ToEven);
                    ws.Cells[rval, 6].Value = (double)invDetail.TotalAmt;
                    //Set Currency Style
                    styleAmountCurr.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (inv.ClientMaster.CurrencyMaster.CurrencySymbol + ";").Replace(";;", ";")) + "]0.00";
                    Aspose.Cells.Range rngAmountCurr = ws.Cells.CreateRange(rval, 5, 1, 2);
                    rngAmountCurr.SetStyle(styleAmountCurr);

                    ws.Cells[rval, 7].Value = invDetail.InvoiceMaster.PONumber;
                    ws.Cells[rval, 8].Value = invDetail.ItemDesc;
                    //// Change on 14-Sep-2020 by Jignesh to set Invoice Date instead of EndDate 
                    ////ws.Cells[rval, 9].Value = invDetail.InvoiceMaster.InvoiceEndDate.ToString("MMdd") + inv.ClientMaster.DepartmentMaster.DepartmentName.Substring(0, 1).ToUpper() + invDetail.InvoiceMaster.InvoiceSeqNo + invDetail.InvoiceMaster.InvoiceEndDate.ToString("yy");
                    ws.Cells[rval, 9].Value = invDetail.InvoiceMaster.InvoiceDate.ToString("MMdd") + inv.ClientMaster.DepartmentMaster.DepartmentName.Substring(0, 1).ToUpper() + invDetail.InvoiceMaster.InvoiceSeqNo + invDetail.InvoiceMaster.InvoiceDate.ToString("yy");
                    ws.Cells[rval, 10].Value = (double)invDetail.Qty;
                    ws.Cells[rval, 11].Value = (double)invDetail.UnitPrice;
                    ws.Cells[rval, 12].Value = invDetail.PriceType;
                    // Set Item Style
                    Aspose.Cells.Range rngItem2 = ws.Cells.CreateRange(rval, 7, 1, 13);
                    rngItem2.SetStyle(styleItem);

                    rval++;
                    isFirst = false;
                }
                else
                {
                    ws.Cells[rval, 2].Value = invDetail.InvoiceMaster.ClientMaster.ClientName;
                    ws.Cells[rval, 3].Value = 0;

                    //// Change on 14-Sep-2020 by Jignesh to set Invoice Date instead of EndDate 
                    ////ws.Cells[rval, 4].Value = invDetail.InvoiceMaster.InvoiceEndDate.ToString("MM/dd/yyyy");
                    ws.Cells[rval, 4].Value = invDetail.InvoiceMaster.InvoiceDate.ToString("MM/dd/yyyy");
                    // Set Item Style
                    Aspose.Cells.Range rngItem = ws.Cells.CreateRange(rval, 1, 1, 4);
                    rngItem.SetStyle(styleItem);

                    //// Added on 03-Dec-2021 by JM: Deleted invoice highlighted with red color (Mangalam USA)
                    if (invDetail.InvoiceMaster.IsDeleted == true)
                    {
                        Aspose.Cells.Style deleteStyle = ws.Cells[rval, 3].GetStyle();
                        deleteStyle.ForegroundColor = System.Drawing.Color.Red;
                        ws.Cells[rval, 3].SetStyle(deleteStyle);
                    }
                    else if (!string.IsNullOrEmpty(invDetail.InvoiceMaster.Revision))
                    {
                        //// Added on 14-Feb-2022 by JM: Deleted invoice highlighted with red color (Mangalam USA)
                        Aspose.Cells.Style RevisedStyle = ws.Cells[rval, 3].GetStyle();
                        RevisedStyle.ForegroundColor = System.Drawing.Color.LightSkyBlue;
                        ws.Cells[rval, 3].SetStyle(RevisedStyle);
                    }


                    ws.Cells[rval, 5].Value = (double)0.00;
                    ws.Cells[rval, 6].Value = (double)invDetail.TotalAmt;
                    //Set Currency Style
                    styleAmountCurr.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (inv.ClientMaster.CurrencyMaster.CurrencySymbol + ";").Replace(";;", ";")) + "]0.00";
                    Aspose.Cells.Range rngAmountCurr = ws.Cells.CreateRange(rval, 5, 1, 2);
                    rngAmountCurr.SetStyle(styleAmountCurr);

                    ws.Cells[rval, 7].Value = invDetail.InvoiceMaster.PONumber;
                    ws.Cells[rval, 8].Value = invDetail.ItemDesc;
                    //// Change on 14-Sep-2020 by Jignesh to set Invoice Date instead of EndDate 
                    ////ws.Cells[rval, 9].Value = invDetail.InvoiceMaster.InvoiceEndDate.ToString("MMdd") + inv.ClientMaster.DepartmentMaster.DepartmentName.Substring(0, 1).ToUpper() + invDetail.InvoiceMaster.InvoiceSeqNo + invDetail.InvoiceMaster.InvoiceEndDate.ToString("yy");
                    ws.Cells[rval, 9].Value = invDetail.InvoiceMaster.InvoiceDate.ToString("MMdd") + inv.ClientMaster.DepartmentMaster.DepartmentName.Substring(0, 1).ToUpper() + invDetail.InvoiceMaster.InvoiceSeqNo + invDetail.InvoiceMaster.InvoiceDate.ToString("yy");
                    ws.Cells[rval, 10].Value = (double)invDetail.Qty;
                    ws.Cells[rval, 11].Value = (double)invDetail.UnitPrice;
                    ws.Cells[rval, 12].Value = invDetail.PriceType;
                    // Set Item Style
                    Aspose.Cells.Range rngItem2 = ws.Cells.CreateRange(rval, 7, 1, 13);
                    rngItem2.SetStyle(styleItem);

                    rval++;
                }
            } //// For loop end for Invoice Details

            //// Added on 02-Dec-2021 by JM: View invoice records if invoice details not found (Mangalam India)
            #region View invoice records if invoice details not found (Mangalam India) 
            if (InvoiceDetails.Count() == 0)
            {
                ws.Cells[rval, 2].Value = inv.ClientMaster.ClientName;
                ws.Cells[rval, 3].Value = (int)inv.InvoiceSeqNo;
                //// Change on 14-Sep-2020 by Jignesh to set Invoice Date instead of EndDate 
                ////ws.Cells[rval, 4].Value = invDetail.InvoiceMaster.InvoiceEndDate.ToString("MM/dd/yyyy");
                ws.Cells[rval, 4].Value = inv.InvoiceDate.ToString("MM/dd/yyyy");
                // Set Item Style
                Aspose.Cells.Range rngItem = ws.Cells.CreateRange(rval, 1, 1, 4);
                rngItem.SetStyle(styleItem);
                //Set New Invoice Style
                Aspose.Cells.Style NewStyle = ws.Cells[(rval), 4].GetStyle();
                NewStyle.ForegroundColor = System.Drawing.Color.Yellow;
                NewStyle.Pattern = BackgroundType.Solid;
                Aspose.Cells.Range rngNew = ws.Cells.CreateRange(rval, 3, 1, 1);
                rngNew.SetStyle(NewStyle);

                //// Added on 03-Dec-2021 by JM: Deleted invoice highlighted with red color (Mangalam USA)
                if (inv.IsDeleted == true)
                {
                    Aspose.Cells.Style deleteStyle = ws.Cells[rval, 3].GetStyle();
                    deleteStyle.ForegroundColor = System.Drawing.Color.Red;
                    ws.Cells[rval, 3].SetStyle(deleteStyle);
                }
                else if (!string.IsNullOrEmpty(inv.Revision))
                {
                    Aspose.Cells.Style RevisedStyle = ws.Cells[rval, 3].GetStyle();
                    RevisedStyle.ForegroundColor = System.Drawing.Color.LightSkyBlue;
                    ws.Cells[rval, 3].SetStyle(RevisedStyle);
                }

                ws.Cells[rval, 5].Value = (double)Math.Round(Convert.ToDecimal(((from DBData in dbobj.InvoiceDetailsMasters
                                                                                 where DBData.InvoiceId == inv.InvoiceId
                                                                                 select DBData.TotalAmt.Value).Count() == 0 ? "0.00" : Convert.ToString(((from DBData in dbobj.InvoiceDetailsMasters
                                                                                                                                                          where DBData.InvoiceId == inv.InvoiceId
                                                                                                                                                          select DBData.TotalAmt).Sum())))), 2, MidpointRounding.ToEven);
                ws.Cells[rval, 6].Value = 0.00;
                //Set Currency Style
                styleAmountCurr.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (inv.ClientMaster.CurrencyMaster.CurrencySymbol + ";").Replace(";;", ";")) + "]0.00";
                Aspose.Cells.Range rngAmountCurr = ws.Cells.CreateRange(rval, 5, 1, 2);
                rngAmountCurr.SetStyle(styleAmountCurr);

                ws.Cells[rval, 7].Value = inv.PONumber;
                ws.Cells[rval, 8].Value = "";
                //// Change on 14-Sep-2020 by Jignesh to set Invoice Date instead of EndDate 
                ////ws.Cells[rval, 9].Value = invDetail.InvoiceMaster.InvoiceEndDate.ToString("MMdd") + inv.ClientMaster.DepartmentMaster.DepartmentName.Substring(0, 1).ToUpper() + invDetail.InvoiceMaster.InvoiceSeqNo + invDetail.InvoiceMaster.InvoiceEndDate.ToString("yy");
                ws.Cells[rval, 9].Value = inv.InvoiceDate.ToString("MMdd") + inv.ClientMaster.DepartmentMaster.DepartmentName.Substring(0, 1).ToUpper() + inv.InvoiceSeqNo + inv.InvoiceDate.ToString("yy");
                ws.Cells[rval, 10].Value = 0.00;
                ws.Cells[rval, 11].Value = 0.00;
                ws.Cells[rval, 12].Value = "";
                // Set Item Style
                Aspose.Cells.Range rngItem2 = ws.Cells.CreateRange(rval, 7, 1, 13);
                rngItem2.SetStyle(styleItem);

                rval++;

            }
            #endregion

        } //// For loop end for Invoice India
        Aspose.Cells.Range rngBlanck1 = ws.Cells.CreateRange(rval, 1, 1, 19);
        rngBlanck1.SetStyle(styleSubHeader);
        rval++;
        ws.Cells[rval, 2].Value = "Mangalam Infotecch USA";
        ws.Cells[rval, 3].Value = "00000";
        ws.Cells[rval, 4].Value = CDate.Month + "/" + DateTime.DaysInMonth(CDate.Year, CDate.Month) + "/" + CDate.Year;

        ws.Cells[rval, 7].Formula = "=F" + (rvalTotal + 1);
        int rvalUSA = rval + 1;


        // Set Item Style
        Aspose.Cells.Style s = ws.Cells[rvalT, 5].GetStyle();
        ws.Cells[rval, 7].SetStyle(s);
        //Aspose.Cells.Range rngItemM = ws.Cells.CreateRange(rval, 7, 1, 1);
        //rngItemM.SetStyle(styleAmountCurr);
        Aspose.Cells.Range rngItemM1 = ws.Cells.CreateRange(rval, 1, 1, 6);
        rngItemM1.SetStyle(styleItem);
        Aspose.Cells.Range rngItemM2 = ws.Cells.CreateRange(rval, 8, 1, 12);
        rngItemM2.SetStyle(styleItem);

        rval++;
        ws.Cells[rval, 7].Value = "Total";
        // Set Item Style
        Aspose.Cells.Range rngTotalI = ws.Cells.CreateRange(rval, 1, 1, 19);
        rngTotalI.SetStyle(styleSubHeader);

        rval++;
        ws.Cells[rval, 4].Value = "Total";
        ws.Cells[rval, 5].Formula = "=SUM(F" + rvalIndia + ":F" + rval + ")";
        ws.Cells[rval, 7].Formula = "=H" + rvalUSA + "+F" + (rval + 1);
        // Set Item Style
        Aspose.Cells.Range rngTotalI1 = ws.Cells.CreateRange(rval, 1, 1, 4);
        rngTotalI1.SetStyle(styleSubHeader);
        Aspose.Cells.Range rngTotalI2 = ws.Cells.CreateRange(rval, 5, 1, 3);
        rngTotalI2.SetStyle(s);
        Aspose.Cells.Range rngTotalI4 = ws.Cells.CreateRange(rval, 8, 1, 12);
        rngTotalI4.SetStyle(s);

        //Set Sytel for Department color
        ws.Cells[4, 2].SetStyle(styleDeptHeadeColor);
        for (int i = 0; i < DeptRVal.Count(); i++)
        {
            if (DeptRVal[i] != 0)
                ws.Cells[DeptRVal[i], 2].SetStyle(styleDeptColor);
        }

        wb.CalculateFormula();
        ws.AutoFitColumns();
        ws.Cells.Columns[0].Width = 1.43;
        ws.Cells.Columns[7].Width = 18;
        ws.Cells.Columns[8].Width = 35;

        //wb.Save(@"C:\Documents and Settings\MahendraK\Desktop\MonthlySales_" + txtMonth.Text + ".xls");
        MemoryStream stm = new MemoryStream();
        wb.Save(stm, Aspose.Cells.SaveFormat.Excel97To2003);
        ExportFile(stm, "NewMonthlySales_" + txtMonth.Text + ".xls");
    }

    protected void gvMonthlySales_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && gvMonthlySales.EditIndex != e.Row.RowIndex)
        {
            e.Row.Cells[16].Visible = IsShowStatusCol;
            e.Row.Cells[17].Visible = IsShowDelCol;

            GridViewRow grv = e.Row;

            // Update by Jignesh on 08-Nov-2022 For duplicate invoice visible if already deleted but status is Draft/Unpaid
            if (grv.Cells[16].Text.Trim().ToUpper().Equals("DRAFT"))
                e.Row.BackColor = Color.LightGray;

            if (grv.Cells[16].Text.Trim().ToUpper().Equals("PAID"))
                e.Row.BackColor = Color.LightCyan;

            if (grv.Cells[16].Text.Trim().ToUpper().Equals("UNPAID"))
                e.Row.BackColor = Color.LightGreen;

            if (grv.Cells[17].Text.Trim().ToUpper().Equals("TRUE"))
                e.Row.BackColor = Color.Red;

            if ( grv.Cells[17].Text.Trim().ToUpper().Equals("FALSE") && (grv.Cells[18].Text.Replace("&nbsp;", "").Trim() != "" || grv.Cells[18].Text.Replace("&nbsp;", "").Trim() != string.Empty) )
            //if ((grv.Cells[18].Text.Replace("&nbsp;", "").Trim() != "" || grv.Cells[18].Text.Replace("&nbsp;", "").Trim() != string.Empty))
            {
                if (!string.IsNullOrEmpty(grv.Cells[18].Text.Trim()))
                    e.Row.BackColor = Color.LightSkyBlue;
            }

            /*
            if (grv.Cells[16].Text.Trim().ToUpper().Equals("DRAFT"))
                e.Row.BackColor = Color.LightGray;

            if (grv.Cells[16].Text.Trim().ToUpper().Equals("PAID"))
                e.Row.BackColor = Color.LightCyan;

            if (grv.Cells[16].Text.Trim().ToUpper().Equals("UNPAID"))
                e.Row.BackColor = Color.LightGreen;

            if (grv.Cells[17].Text.Trim().ToUpper().Equals("TRUE"))
                e.Row.BackColor = Color.Red;

            if (grv.Cells[18].Text.Replace("&nbsp;", "").Trim() != "" || grv.Cells[18].Text.Replace("&nbsp;", "").Trim() != string.Empty)
            {
                if (!string.IsNullOrEmpty(grv.Cells[18].Text.Trim()))
                    e.Row.BackColor = Color.LightSkyBlue;
            }
            */

            // if (lblStatus.Text.ToLower() == "DRAFT")
            //     e.Row.BackColor = Color.LightGray;
            // else if (lblStatus.Text.ToLower() == "completed")
            //     e.Row.BackColor = Color.BurlyWood;
            // else if (lblStatus.Text.ToLower() == "on hold")
            //     e.Row.BackColor = Color.LightSeaGreen;
            // else if (lblStatus.Text.ToLower() == "pending")
            //     e.Row.BackColor = Color.DarkKhaki;
        }
    }


}