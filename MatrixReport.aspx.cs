using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

public partial class MatrixReport : System.Web.UI.Page
{
    MITInvoiceDataContext dbobj = new MITInvoiceDataContext();
    DataTable dt = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Global.UserType != "ADMINISTRATOR")
        {
            if (!IsPostBack)
            {
                string financialYear = string.Empty;
                int CurrYear = DateTime.Now.Year;
                if (DateTime.Now <= Convert.ToDateTime("03/31/" + CurrYear))
                {
                    txtFromMonth.Text = "Apr-" + (CurrYear - 1);
                    txtToMonth.Text = "Mar-" + CurrYear;
                }
                else
                {
                    txtFromMonth.Text = "Apr-" + CurrYear;
                    txtToMonth.Text = "Mar-" + (CurrYear + 1);
                }
                txtFromMonth_CalendarExtenderPlus.MaximumDate = DateTime.ParseExact(txtToMonth.Text, "MMM-yyyy", null);
                txtToMonth_CalendarExtenderPlus.MinimumDate = DateTime.ParseExact(txtFromMonth.Text, "MMM-yyyy", null);
                txtToMonth_CalendarExtenderPlus.MaximumDate = DateTime.ParseExact(txtToMonth.Text, "MMM-yyyy", null);
                FillDepartment();
                FillClient();
                FillGrid();
            }
        }
        else
        {
            Response.Redirect("Authorize");
        }
    }

    protected void FillGrid_Existing()
    {

        string s1, s2;
        DateTime dat1, dat2;
        s1 = DateTime.ParseExact(txtFromMonth.Text, "MMM-yyyy", null).ToString("MM/dd/yyyy");
        s2 = DateTime.ParseExact(txtToMonth.Text, "MMM-yyyy", null).ToString("MM/dd/yyyy");
        dat1 = Convert.ToDateTime(s1);
        dat2 = Convert.ToDateTime(s2);
        dt.Columns.Add("ClientName");

        Boolean isnew = false;
        while (dat1 <= dat2)
        {
            string dColName = " " + dat1.ToString("MMM-yy");
            if (dt.Columns[dColName] == null)
                dt.Columns.Add(dColName);


            if (ddlDepartment.SelectedIndex != -1 && ddlClient.SelectedIndex != -1)
            {
                if (ddlDepartment.SelectedItem.ToString() == "-- All --" && ddlClient.SelectedItem.ToString() == "-- All --")
                {

                    // Fill report for all department client

                    var Client = from DBClient in dbobj.ClientMasters
                                 where DBClient.IsActive == true
                                 select new
                                 {
                                     CName = DBClient.ClientName,
                                     CId = DBClient.ClientId
                                 };
                    if (Client.Count() > 0)
                    {
                        int mk = 0;
                        foreach (var c in Client)
                        {
                            if (!isnew)
                            {
                                DataRow dr = dt.NewRow();
                                var Invoice = from DBInvoice in dbobj.InvoiceMasters
                                              join DBDetail in dbobj.InvoiceDetailsMasters
                                              on DBInvoice.InvoiceId equals DBDetail.InvoiceId
                                              where DBInvoice.IsDeleted == false && DBInvoice.ClientId == c.CId && DBInvoice.InvoiceStartDate.Month == dat1.Month && DBInvoice.InvoiceStartDate.Year == dat1.Year && DBInvoice.IsRevised == false
                                              group DBDetail by new { DBInvoice.ClientId } into G
                                              select new
                                              {
                                                  Total = G.Sum(y => y.TotalAmt)
                                              };
                                if (Invoice.Count() > 0)
                                {
                                    foreach (var ind in Invoice)
                                    {
                                        dr["ClientName"] = c.CName;
                                        dr[dColName] = ind.Total;
                                    }
                                }
                                else
                                {
                                    dr["ClientName"] = c.CName;
                                    dr[dColName] = "00.00";
                                }
                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                DataRow dr = dt.Rows[mk];
                                dr.BeginEdit();
                                var Invoice = from DBInvoice in dbobj.InvoiceMasters
                                              join DBDetail in dbobj.InvoiceDetailsMasters
                                              on DBInvoice.InvoiceId equals DBDetail.InvoiceId
                                              where DBInvoice.IsDeleted == false && DBInvoice.ClientId == c.CId && DBInvoice.InvoiceStartDate.Month == dat1.Month && DBInvoice.InvoiceStartDate.Year == dat1.Year && DBInvoice.IsRevised == false
                                              group DBDetail by new { DBInvoice.ClientId } into G
                                              select new
                                              {
                                                  Total = G.Sum(y => y.TotalAmt)
                                              };
                                if (Invoice.Count() > 0)
                                {
                                    foreach (var ind in Invoice)
                                    {
                                        dr["ClientName"] = c.CName;
                                        dr[dColName] = ind.Total;
                                    }
                                }
                                else
                                {
                                    dr["ClientName"] = c.CName;
                                    dr[dColName] = "00.00";
                                }
                                dr.EndEdit();
                                dt.AcceptChanges();
                                mk++;
                            }
                        }
                        isnew = true;
                    }
                }
                else if (ddlDepartment.SelectedItem.ToString() == "-- All --" && ddlClient.SelectedItem.ToString() != "-- All --")
                {
                    // Fill report for all department but selected client

                    var Client = from DBClient in dbobj.ClientMasters
                                 where DBClient.IsActive == true && DBClient.ClientId == Convert.ToInt64(ddlClient.SelectedValue)
                                 select new
                                 {
                                     CName = DBClient.ClientName,
                                     CId = DBClient.ClientId
                                 };
                    if (Client.Count() > 0)
                    {
                        int mk = 0;
                        foreach (var c in Client)
                        {
                            if (!isnew)
                            {
                                DataRow dr = dt.NewRow();
                                var Invoice = from DBInvoice in dbobj.InvoiceMasters
                                              join DBDetail in dbobj.InvoiceDetailsMasters
                                              on DBInvoice.InvoiceId equals DBDetail.InvoiceId
                                              where DBInvoice.IsDeleted == false && DBInvoice.ClientId == c.CId && DBInvoice.InvoiceStartDate.Month == dat1.Month && DBInvoice.InvoiceStartDate.Year == dat1.Year && DBInvoice.IsRevised == false
                                              group DBDetail by new { DBInvoice.ClientId } into G
                                              select new
                                              {
                                                  Total = G.Sum(y => y.TotalAmt)
                                              };
                                if (Invoice.Count() > 0)
                                {
                                    foreach (var ind in Invoice)
                                    {
                                        dr["ClientName"] = c.CName;
                                        dr[dColName] = ind.Total;
                                    }
                                }
                                else
                                {
                                    dr["ClientName"] = c.CName;
                                    dr[dColName] = "00.00";
                                }
                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                DataRow dr = dt.Rows[mk];
                                dr.BeginEdit();
                                var Invoice = from DBInvoice in dbobj.InvoiceMasters
                                              join DBDetail in dbobj.InvoiceDetailsMasters
                                              on DBInvoice.InvoiceId equals DBDetail.InvoiceId
                                              where DBInvoice.IsDeleted == false && DBInvoice.ClientId == c.CId && DBInvoice.InvoiceStartDate.Month == dat1.Month && DBInvoice.InvoiceStartDate.Year == dat1.Year && DBInvoice.IsRevised == false
                                              group DBDetail by new { DBInvoice.ClientId } into G
                                              select new
                                              {
                                                  Total = G.Sum(y => y.TotalAmt)
                                              };
                                if (Invoice.Count() > 0)
                                {
                                    foreach (var ind in Invoice)
                                    {
                                        dr["ClientName"] = c.CName;
                                        dr[dColName] = ind.Total;
                                    }
                                }
                                else
                                {
                                    dr["ClientName"] = c.CName;
                                    dr[dColName] = "00.00";
                                }
                                dr.EndEdit();
                                dt.AcceptChanges();
                                mk++;
                            }
                        }
                        isnew = true;
                    }
                }
                else if (ddlDepartment.SelectedItem.ToString() != "-- All --" && ddlClient.SelectedItem.ToString() == "-- All --")
                {
                    // Fill report for all client of selected department

                    var Client = from DBClient in dbobj.ClientMasters
                                 where DBClient.IsActive == true && DBClient.DepartmentId == Convert.ToInt64(ddlDepartment.SelectedValue)
                                 select new
                                 {
                                     CName = DBClient.ClientName,
                                     CId = DBClient.ClientId
                                 };
                    if (Client.Count() > 0)
                    {
                        int mk = 0;
                        foreach (var c in Client)
                        {
                            if (!isnew)
                            {
                                DataRow dr = dt.NewRow();
                                var Invoice = from DBInvoice in dbobj.InvoiceMasters
                                              join DBDetail in dbobj.InvoiceDetailsMasters
                                              on DBInvoice.InvoiceId equals DBDetail.InvoiceId
                                              where DBInvoice.IsDeleted == false && DBInvoice.ClientId == c.CId && DBInvoice.InvoiceStartDate.Month == dat1.Month && DBInvoice.InvoiceStartDate.Year == dat1.Year && DBInvoice.IsRevised == false
                                              group DBDetail by new { DBInvoice.ClientId } into G
                                              select new
                                              {
                                                  Total = G.Sum(y => y.TotalAmt)
                                              };
                                if (Invoice.Count() > 0)
                                {
                                    foreach (var ind in Invoice)
                                    {
                                        dr["ClientName"] = c.CName;
                                        dr[dColName] = ind.Total;
                                    }
                                }
                                else
                                {
                                    dr["ClientName"] = c.CName;
                                    dr[dColName] = "00.00";
                                }
                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                DataRow dr = dt.Rows[mk];
                                dr.BeginEdit();
                                var Invoice = from DBInvoice in dbobj.InvoiceMasters
                                              join DBDetail in dbobj.InvoiceDetailsMasters
                                              on DBInvoice.InvoiceId equals DBDetail.InvoiceId
                                              where DBInvoice.IsDeleted == false && DBInvoice.ClientId == c.CId && DBInvoice.InvoiceStartDate.Month == dat1.Month && DBInvoice.InvoiceStartDate.Year == dat1.Year && DBInvoice.IsRevised == false
                                              group DBDetail by new { DBInvoice.ClientId } into G
                                              select new
                                              {
                                                  Total = G.Sum(y => y.TotalAmt)
                                              };
                                if (Invoice.Count() > 0)
                                {
                                    foreach (var ind in Invoice)
                                    {
                                        dr["ClientName"] = c.CName;
                                        dr[dColName] = ind.Total;
                                    }
                                }
                                else
                                {
                                    dr["ClientName"] = c.CName;
                                    dr[dColName] = "00.00";
                                }
                                dr.EndEdit();
                                dt.AcceptChanges();
                                mk++;
                            }
                        }
                        isnew = true;
                    }
                }
                else if (ddlDepartment.SelectedItem.ToString() != "-- All --" && ddlClient.SelectedItem.ToString() != "-- All --")
                {
                    // Fill report for selected deparment and client

                    var Client = from DBClient in dbobj.ClientMasters
                                 where DBClient.IsActive == true && DBClient.DepartmentId == Convert.ToInt64(ddlDepartment.SelectedValue) && DBClient.ClientId == Convert.ToInt64(ddlClient.SelectedValue)
                                 select new
                                 {
                                     CName = DBClient.ClientName,
                                     CId = DBClient.ClientId
                                 };
                    if (Client.Count() > 0)
                    {
                        int mk = 0;
                        foreach (var c in Client)
                        {
                            if (!isnew)
                            {
                                DataRow dr = dt.NewRow();
                                var Invoice = from DBInvoice in dbobj.InvoiceMasters
                                              join DBDetail in dbobj.InvoiceDetailsMasters
                                              on DBInvoice.InvoiceId equals DBDetail.InvoiceId
                                              where DBInvoice.IsDeleted == false && DBInvoice.ClientId == c.CId && DBInvoice.InvoiceStartDate.Month == dat1.Month && DBInvoice.InvoiceStartDate.Year == dat1.Year && DBInvoice.IsRevised == false
                                              group DBDetail by new { DBInvoice.ClientId } into G
                                              select new
                                              {
                                                  Total = G.Sum(y => y.TotalAmt)
                                              };
                                if (Invoice.Count() > 0)
                                {
                                    foreach (var ind in Invoice)
                                    {
                                        dr["ClientName"] = c.CName;
                                        dr[dColName] = ind.Total;
                                    }
                                }
                                else
                                {
                                    dr["ClientName"] = c.CName;
                                    dr[dColName] = "00.00";
                                }
                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                DataRow dr = dt.Rows[mk];
                                dr.BeginEdit();
                                var Invoice = from DBInvoice in dbobj.InvoiceMasters
                                              join DBDetail in dbobj.InvoiceDetailsMasters
                                              on DBInvoice.InvoiceId equals DBDetail.InvoiceId
                                              where DBInvoice.IsDeleted == false && DBInvoice.ClientId == c.CId && DBInvoice.InvoiceStartDate.Month == dat1.Month && DBInvoice.InvoiceStartDate.Year == dat1.Year && DBInvoice.IsRevised == false
                                              group DBDetail by new { DBInvoice.ClientId } into G
                                              select new
                                              {
                                                  Total = G.Sum(y => y.TotalAmt)
                                              };
                                if (Invoice.Count() > 0)
                                {
                                    foreach (var ind in Invoice)
                                    {
                                        dr["ClientName"] = c.CName;
                                        dr[dColName] = ind.Total;
                                    }
                                }
                                else
                                {
                                    dr["ClientName"] = c.CName;
                                    dr[dColName] = "00.00";
                                }
                                dr.EndEdit();
                                dt.AcceptChanges();
                                mk++;
                            }
                        }
                        isnew = true;
                    }
                }
            }
            else
                dt.Rows.Clear();
            dat1 = dat1.AddMonths(1);
        }

        if (dt.Rows.Count > 0)
        {
            btnExcel.Enabled = true;
            dt.Columns.Add("Total");
            gvMatrix.DataSource = dt;
            gvMatrix.DataBind();

            // Sum of amount rows(Client) wise
            decimal RTotal = 0;
            for (int r = 0; r < gvMatrix.Rows.Count; r++)
            {
                for (int c = 1; c < gvMatrix.Rows[r].Cells.Count - 1; c++)
                {
                    RTotal += Convert.ToDecimal(gvMatrix.Rows[r].Cells[c].Text);
                    gvMatrix.Rows[r].Cells[gvMatrix.Rows[r].Cells.Count - 1].Text = RTotal.ToString("00.00");
                }
                RTotal = 0;
            }

            // Sum of amount column(Month) wise
            decimal CTotal = 0;
            for (int c = 1; c < gvMatrix.Rows[0].Cells.Count; c++)
            {
                for (int r = 0; r < gvMatrix.Rows.Count; r++)
                {
                    CTotal += Convert.ToDecimal(gvMatrix.Rows[r].Cells[c].Text);
                    gvMatrix.FooterRow.Cells[c].Text = CTotal.ToString("00.00");
                }
                CTotal = 0;
            }
            gvMatrix.FooterRow.Cells[0].Text = "Total";
        }
        else
        {
            btnExcel.Enabled = false;
            int a = dt.Columns.Count;
            gvMatrix.DataSource = dt;
            dt.Rows.Add(dt.NewRow());
            gvMatrix.DataBind();
            int TotalCols = gvMatrix.Rows[0].Cells.Count;
            gvMatrix.Rows[0].Cells.Clear();
            gvMatrix.Rows[0].Cells.Add(new TableCell());
            gvMatrix.Rows[0].Cells[0].ColumnSpan = TotalCols;
            gvMatrix.Rows[0].Cells[0].Text = "No records to display";
        }
    }

    protected void FillGrid()
    {

        string s1, s2;
        DateTime dat1, dat2;
        s1 = DateTime.ParseExact(txtFromMonth.Text, "MMM-yyyy", null).ToString("MM/dd/yyyy");
        s2 = DateTime.ParseExact(txtToMonth.Text, "MMM-yyyy", null).ToString("MM/dd/yyyy");
        dat1 = Convert.ToDateTime(s1);
        dat2 = Convert.ToDateTime(s2);
        dt.Columns.Add("ClientName");

        Boolean isnew = false;
        while (dat1 <= dat2)
        {
            string dColName = " " + dat1.ToString("MMM-yy");
            if (dt.Columns[dColName] == null)
                dt.Columns.Add(dColName);


            if (ddlDepartment.SelectedIndex != -1 && ddlClient.SelectedIndex != -1)
            {
                if (ddlDepartment.SelectedItem.ToString() == "-- All --" && ddlClient.SelectedItem.ToString() == "-- All --")
                {

                    // Fill report for all department client
                    // 18-Aug-2020 by Jignesh
                    // added : && (DBClient.C_M1 == Global.UserM1 || DBClient.C_M2 == Global.UserM2)
                    var Client = from DBClient in dbobj.ClientMasters
                                 where DBClient.IsActive == true && (DBClient.C_M1 == Global.UserM1 || DBClient.C_M2 == Global.UserM2)
                                 select new
                                 {
                                     CName = DBClient.ClientName,
                                     CId = DBClient.ClientId
                                 };
                    if (Client.Count() > 0)
                    {
                        int mk = 0;
                        foreach (var c in Client)
                        {
                            if (!isnew)
                            {
                                DataRow dr = dt.NewRow();
                                var Invoice = from DBInvoice in dbobj.InvoiceMasters
                                              join DBDetail in dbobj.InvoiceDetailsMasters
                                              on DBInvoice.InvoiceId equals DBDetail.InvoiceId
                                              //where DBInvoice.IsDeleted == false && DBInvoice.ClientId == c.CId && DBInvoice.InvoiceStartDate.Month == dat1.Month && DBInvoice.InvoiceStartDate.Year == dat1.Year && DBInvoice.IsRevised == false
                                              where DBInvoice.IsDeleted == false && DBInvoice.ClientId == c.CId && DBInvoice.InvoiceDate.Month == dat1.Month && DBInvoice.InvoiceDate.Year == dat1.Year && DBInvoice.IsRevised == false //// Change on 27-Aug-2021 by Jignesh to set InvoiceDate instead of StartDate 
                                              group DBDetail by new { DBInvoice.ClientId } into G
                                              select new
                                              {
                                                  Total = G.Sum(y => y.TotalAmt)
                                              };
                                if (Invoice.Count() > 0)
                                {
                                    foreach (var ind in Invoice)
                                    {
                                        dr["ClientName"] = c.CName;
                                        dr[dColName] = ind.Total;
                                    }
                                }
                                else
                                {
                                    dr["ClientName"] = c.CName;
                                    dr[dColName] = "00.00";
                                }
                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                DataRow dr = dt.Rows[mk];
                                dr.BeginEdit();
                                var Invoice = from DBInvoice in dbobj.InvoiceMasters
                                              join DBDetail in dbobj.InvoiceDetailsMasters
                                              on DBInvoice.InvoiceId equals DBDetail.InvoiceId
                                              //where DBInvoice.IsDeleted == false && DBInvoice.ClientId == c.CId && DBInvoice.InvoiceStartDate.Month == dat1.Month && DBInvoice.InvoiceStartDate.Year == dat1.Year && DBInvoice.IsRevised == false
                                              where DBInvoice.IsDeleted == false && DBInvoice.ClientId == c.CId && DBInvoice.InvoiceDate.Month == dat1.Month && DBInvoice.InvoiceDate.Year == dat1.Year && DBInvoice.IsRevised == false //// Change on 27-Aug-2021 by Jignesh to set InvoiceDate instead of StartDate 
                                              group DBDetail by new { DBInvoice.ClientId } into G
                                              select new
                                              {
                                                  Total = G.Sum(y => y.TotalAmt)
                                              };
                                if (Invoice.Count() > 0)
                                {
                                    foreach (var ind in Invoice)
                                    {
                                        dr["ClientName"] = c.CName;
                                        dr[dColName] = ind.Total;
                                    }
                                }
                                else
                                {
                                    dr["ClientName"] = c.CName;
                                    dr[dColName] = "00.00";
                                }
                                dr.EndEdit();
                                dt.AcceptChanges();
                                mk++;
                            }
                        }
                        isnew = true;
                    }
                }
                else if (ddlDepartment.SelectedItem.ToString() == "-- All --" && ddlClient.SelectedItem.ToString() != "-- All --")
                {
                    // Fill report for all department but selected client

                    // 18-Aug-2020 by Jignesh
                    // added : && (DBClient.C_M1 == Global.UserM1 || DBClient.C_M2 == Global.UserM2)

                    var Client = from DBClient in dbobj.ClientMasters
                                 where DBClient.IsActive == true && DBClient.ClientId == Convert.ToInt64(ddlClient.SelectedValue) && (DBClient.C_M1 == Global.UserM1 || DBClient.C_M2 == Global.UserM2)
                                 select new
                                 {
                                     CName = DBClient.ClientName,
                                     CId = DBClient.ClientId
                                 };
                    if (Client.Count() > 0)
                    {
                        int mk = 0;
                        foreach (var c in Client)
                        {
                            if (!isnew)
                            {
                                DataRow dr = dt.NewRow();
                                var Invoice = from DBInvoice in dbobj.InvoiceMasters
                                              join DBDetail in dbobj.InvoiceDetailsMasters
                                              on DBInvoice.InvoiceId equals DBDetail.InvoiceId
                                              //where DBInvoice.IsDeleted == false && DBInvoice.ClientId == c.CId && DBInvoice.InvoiceStartDate.Month == dat1.Month && DBInvoice.InvoiceStartDate.Year == dat1.Year && DBInvoice.IsRevised == false
                                              where DBInvoice.IsDeleted == false && DBInvoice.ClientId == c.CId && DBInvoice.InvoiceDate.Month == dat1.Month && DBInvoice.InvoiceDate.Year == dat1.Year && DBInvoice.IsRevised == false //// Change on 27-Aug-2021 by Jignesh to set InvoiceDate instead of StartDate 
                                              group DBDetail by new { DBInvoice.ClientId } into G
                                              select new
                                              {
                                                  Total = G.Sum(y => y.TotalAmt)
                                              };
                                if (Invoice.Count() > 0)
                                {
                                    foreach (var ind in Invoice)
                                    {
                                        dr["ClientName"] = c.CName;
                                        dr[dColName] = ind.Total;
                                    }
                                }
                                else
                                {
                                    dr["ClientName"] = c.CName;
                                    dr[dColName] = "00.00";
                                }
                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                DataRow dr = dt.Rows[mk];
                                dr.BeginEdit();
                                var Invoice = from DBInvoice in dbobj.InvoiceMasters
                                              join DBDetail in dbobj.InvoiceDetailsMasters
                                              on DBInvoice.InvoiceId equals DBDetail.InvoiceId
                                              //where DBInvoice.IsDeleted == false && DBInvoice.ClientId == c.CId && DBInvoice.InvoiceStartDate.Month == dat1.Month && DBInvoice.InvoiceStartDate.Year == dat1.Year && DBInvoice.IsRevised == false
                                              where DBInvoice.IsDeleted == false && DBInvoice.ClientId == c.CId && DBInvoice.InvoiceDate.Month == dat1.Month && DBInvoice.InvoiceDate.Year == dat1.Year && DBInvoice.IsRevised == false //// Change on 27-Aug-2021 by Jignesh to set InvoiceDate instead of StartDate 
                                              group DBDetail by new { DBInvoice.ClientId } into G
                                              select new
                                              {
                                                  Total = G.Sum(y => y.TotalAmt)
                                              };
                                if (Invoice.Count() > 0)
                                {
                                    foreach (var ind in Invoice)
                                    {
                                        dr["ClientName"] = c.CName;
                                        dr[dColName] = ind.Total;
                                    }
                                }
                                else
                                {
                                    dr["ClientName"] = c.CName;
                                    dr[dColName] = "00.00";
                                }
                                dr.EndEdit();
                                dt.AcceptChanges();
                                mk++;
                            }
                        }
                        isnew = true;
                    }
                }
                else if (ddlDepartment.SelectedItem.ToString() != "-- All --" && ddlClient.SelectedItem.ToString() == "-- All --")
                {
                    // Fill report for all client of selected department

                    // 18-Aug-2020 by Jignesh
                    // added : && (DBClient.C_M1 == Global.UserM1 || DBClient.C_M2 == Global.UserM2)

                    var Client = from DBClient in dbobj.ClientMasters
                                 where DBClient.IsActive == true && DBClient.DepartmentId == Convert.ToInt64(ddlDepartment.SelectedValue) && (DBClient.C_M1 == Global.UserM1 || DBClient.C_M2 == Global.UserM2)
                                 select new
                                 {
                                     CName = DBClient.ClientName,
                                     CId = DBClient.ClientId
                                 };
                    if (Client.Count() > 0)
                    {
                        int mk = 0;
                        foreach (var c in Client)
                        {
                            if (!isnew)
                            {
                                DataRow dr = dt.NewRow();
                                var Invoice = from DBInvoice in dbobj.InvoiceMasters
                                              join DBDetail in dbobj.InvoiceDetailsMasters
                                              on DBInvoice.InvoiceId equals DBDetail.InvoiceId
                                              //where DBInvoice.IsDeleted == false && DBInvoice.ClientId == c.CId && DBInvoice.InvoiceStartDate.Month == dat1.Month && DBInvoice.InvoiceStartDate.Year == dat1.Year && DBInvoice.IsRevised == false
                                              where DBInvoice.IsDeleted == false && DBInvoice.ClientId == c.CId && DBInvoice.InvoiceDate.Month == dat1.Month && DBInvoice.InvoiceDate.Year == dat1.Year && DBInvoice.IsRevised == false //// Change on 27-Aug-2021 by Jignesh to set InvoiceDate instead of StartDate 
                                              group DBDetail by new { DBInvoice.ClientId } into G
                                              select new
                                              {
                                                  Total = G.Sum(y => y.TotalAmt)
                                              };
                                if (Invoice.Count() > 0)
                                {
                                    foreach (var ind in Invoice)
                                    {
                                        dr["ClientName"] = c.CName;
                                        dr[dColName] = ind.Total;
                                    }
                                }
                                else
                                {
                                    dr["ClientName"] = c.CName;
                                    dr[dColName] = "00.00";
                                }
                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                DataRow dr = dt.Rows[mk];
                                dr.BeginEdit();
                                var Invoice = from DBInvoice in dbobj.InvoiceMasters
                                              join DBDetail in dbobj.InvoiceDetailsMasters
                                              on DBInvoice.InvoiceId equals DBDetail.InvoiceId
                                              //where DBInvoice.IsDeleted == false && DBInvoice.ClientId == c.CId && DBInvoice.InvoiceStartDate.Month == dat1.Month && DBInvoice.InvoiceStartDate.Year == dat1.Year && DBInvoice.IsRevised == false
                                              where DBInvoice.IsDeleted == false && DBInvoice.ClientId == c.CId && DBInvoice.InvoiceDate.Month == dat1.Month && DBInvoice.InvoiceDate.Year == dat1.Year && DBInvoice.IsRevised == false //// Change on 27-Aug-2021 by Jignesh to set InvoiceDate instead of StartDate 
                                              group DBDetail by new { DBInvoice.ClientId } into G
                                              select new
                                              {
                                                  Total = G.Sum(y => y.TotalAmt)
                                              };
                                if (Invoice.Count() > 0)
                                {
                                    foreach (var ind in Invoice)
                                    {
                                        dr["ClientName"] = c.CName;
                                        dr[dColName] = ind.Total;
                                    }
                                }
                                else
                                {
                                    dr["ClientName"] = c.CName;
                                    dr[dColName] = "00.00";
                                }
                                dr.EndEdit();
                                dt.AcceptChanges();
                                mk++;
                            }
                        }
                        isnew = true;
                    }
                }
                else if (ddlDepartment.SelectedItem.ToString() != "-- All --" && ddlClient.SelectedItem.ToString() != "-- All --")
                {
                    // Fill report for selected deparment and client

                    
                    var Client = from DBClient in dbobj.ClientMasters
                                 where DBClient.IsActive == true && DBClient.DepartmentId == Convert.ToInt64(ddlDepartment.SelectedValue) && DBClient.ClientId == Convert.ToInt64(ddlClient.SelectedValue)
                                 select new
                                 {
                                     CName = DBClient.ClientName,
                                     CId = DBClient.ClientId
                                 };
                    if (Client.Count() > 0)
                    {
                        int mk = 0;
                        foreach (var c in Client)
                        {
                            if (!isnew)
                            {
                                DataRow dr = dt.NewRow();
                                var Invoice = from DBInvoice in dbobj.InvoiceMasters
                                              join DBDetail in dbobj.InvoiceDetailsMasters
                                              on DBInvoice.InvoiceId equals DBDetail.InvoiceId
                                              //where DBInvoice.IsDeleted == false && DBInvoice.ClientId == c.CId && DBInvoice.InvoiceStartDate.Month == dat1.Month && DBInvoice.InvoiceStartDate.Year == dat1.Year && DBInvoice.IsRevised == false
                                              where DBInvoice.IsDeleted == false && DBInvoice.ClientId == c.CId && DBInvoice.InvoiceDate.Month == dat1.Month && DBInvoice.InvoiceDate.Year == dat1.Year && DBInvoice.IsRevised == false //// Change on 27-Aug-2021 by Jignesh to set InvoiceDate instead of StartDate 
                                              group DBDetail by new { DBInvoice.ClientId } into G
                                              select new
                                              {
                                                  Total = G.Sum(y => y.TotalAmt)
                                              };
                                if (Invoice.Count() > 0)
                                {
                                    foreach (var ind in Invoice)
                                    {
                                        dr["ClientName"] = c.CName;
                                        dr[dColName] = ind.Total;
                                    }
                                }
                                else
                                {
                                    dr["ClientName"] = c.CName;
                                    dr[dColName] = "00.00";
                                }
                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                DataRow dr = dt.Rows[mk];
                                dr.BeginEdit();
                                var Invoice = from DBInvoice in dbobj.InvoiceMasters
                                              join DBDetail in dbobj.InvoiceDetailsMasters
                                              on DBInvoice.InvoiceId equals DBDetail.InvoiceId
                                              //where DBInvoice.IsDeleted == false && DBInvoice.ClientId == c.CId && DBInvoice.InvoiceStartDate.Month == dat1.Month && DBInvoice.InvoiceStartDate.Year == dat1.Year && DBInvoice.IsRevised == false
                                              where DBInvoice.IsDeleted == false && DBInvoice.ClientId == c.CId && DBInvoice.InvoiceDate.Month == dat1.Month && DBInvoice.InvoiceDate.Year == dat1.Year && DBInvoice.IsRevised == false //// Change on 27-Aug-2021 by Jignesh to set InvoiceDate instead of StartDate 
                                              group DBDetail by new { DBInvoice.ClientId } into G
                                              select new
                                              {
                                                  Total = G.Sum(y => y.TotalAmt)
                                              };
                                if (Invoice.Count() > 0)
                                {
                                    foreach (var ind in Invoice)
                                    {
                                        dr["ClientName"] = c.CName;
                                        dr[dColName] = ind.Total;
                                    }
                                }
                                else
                                {
                                    dr["ClientName"] = c.CName;
                                    dr[dColName] = "00.00";
                                }
                                dr.EndEdit();
                                dt.AcceptChanges();
                                mk++;
                            }
                        }
                        isnew = true;
                    }
                }
            }
            else
                dt.Rows.Clear();
            dat1 = dat1.AddMonths(1);
        }

        if (dt.Rows.Count > 0)
        {
            btnExcel.Enabled = true;
            dt.Columns.Add("Total");
            gvMatrix.DataSource = dt;
            gvMatrix.DataBind();

            // Sum of amount rows(Client) wise
            decimal RTotal = 0;
            for (int r = 0; r < gvMatrix.Rows.Count; r++)
            {
                for (int c = 1; c < gvMatrix.Rows[r].Cells.Count - 1; c++)
                {
                    RTotal += Convert.ToDecimal(gvMatrix.Rows[r].Cells[c].Text);
                    gvMatrix.Rows[r].Cells[gvMatrix.Rows[r].Cells.Count - 1].Text = RTotal.ToString("00.00");
                }
                RTotal = 0;
            }

            // Sum of amount column(Month) wise
            decimal CTotal = 0;
            for (int c = 1; c < gvMatrix.Rows[0].Cells.Count; c++)
            {
                for (int r = 0; r < gvMatrix.Rows.Count; r++)
                {
                    CTotal += Convert.ToDecimal(gvMatrix.Rows[r].Cells[c].Text);
                    gvMatrix.FooterRow.Cells[c].Text = CTotal.ToString("00.00");
                }
                CTotal = 0;
            }
            gvMatrix.FooterRow.Cells[0].Text = "Total";
        }
        else
        {
            btnExcel.Enabled = false;
            int a = dt.Columns.Count;
            gvMatrix.DataSource = dt;
            dt.Rows.Add(dt.NewRow());
            gvMatrix.DataBind();
            int TotalCols = gvMatrix.Rows[0].Cells.Count;
            gvMatrix.Rows[0].Cells.Clear();
            gvMatrix.Rows[0].Cells.Add(new TableCell());
            gvMatrix.Rows[0].Cells[0].ColumnSpan = TotalCols;
            gvMatrix.Rows[0].Cells[0].Text = "No records to display";
        }
    }

    protected void FillClient_Existing()
    {
        if (ddlDepartment.SelectedIndex != -1)
        {
            if (ddlDepartment.SelectedItem.ToString() == "-- All --")
            {
                if (Global.UserType == "DIRECTOR" || Global.DepartmentName == "ACCT")
                {
                    var Client = from DBClient in dbobj.ClientMasters
                                 where DBClient.IsActive == true
                                 orderby DBClient.ClientName
                                 select new
                                 {
                                     Name = DBClient.ClientName,
                                     Id = DBClient.ClientId
                                 };
                    if (Client.Count() > 0)
                    {
                        ddlClient.DataSource = Client;
                        ddlClient.DataTextField = "Name";
                        ddlClient.DataValueField = "Id";
                        ddlClient.DataBind();
                    }
                }
                else
                {
                    var Client = from DBClient in dbobj.ClientMasters
                                 where DBClient.IsActive == true && DBClient.DepartmentMaster.UserId == Convert.ToInt64(Global.UserId)
                                 orderby DBClient.ClientName
                                 select new
                                 {
                                     Name = DBClient.ClientName,
                                     Id = DBClient.ClientId
                                 };
                    if (Client.Count() > 0)
                    {
                        ddlClient.DataSource = Client;
                        ddlClient.DataTextField = "Name";
                        ddlClient.DataValueField = "Id";
                        ddlClient.DataBind();
                    }
                }
                if (ddlClient.Items.Count > 1)
                {
                    ddlClient.Items.Insert(0, "-- All --");
                }
            }
            else
            {
                ddlClient.Items.Clear();
                var Client = from DBClient in dbobj.ClientMasters
                             where DBClient.IsActive == true && DBClient.DepartmentId == Convert.ToInt64(ddlDepartment.SelectedValue)
                             orderby DBClient.ClientName
                             select new
                             {
                                 Name = DBClient.ClientName,
                                 Id = DBClient.ClientId
                             };
                if (Client.Count() > 0)
                {
                    ddlClient.DataSource = Client;
                    ddlClient.DataTextField = "Name";
                    ddlClient.DataValueField = "Id";
                    ddlClient.DataBind();
                }
                if (ddlClient.Items.Count > 1)
                {
                    ddlClient.Items.Insert(0, "-- All --");
                }
            }
        }
    }

    protected void FillClient()
    {
        if (ddlDepartment.SelectedIndex != -1)
        {
            if (ddlDepartment.SelectedItem.ToString() == "-- All --")
            {
                // 18-Aug 2020 by Jignesh
                // added : && (DBClient.C_M1 == Global.UserM1 || DBClient.C_M2 == Global.UserM2)
                if (Global.UserType == "DIRECTOR" || Global.DepartmentName == "ACCT")
                {
                    var Client = from DBClient in dbobj.ClientMasters
                                 where DBClient.IsActive == true && (DBClient.C_M1 == Global.UserM1 || DBClient.C_M2 == Global.UserM2)
                                 orderby DBClient.ClientName
                                 select new
                                 {
                                     Name = DBClient.ClientName,
                                     Id = DBClient.ClientId
                                 };
                    if (Client.Count() > 0)
                    {
                        ddlClient.DataSource = Client;
                        ddlClient.DataTextField = "Name";
                        ddlClient.DataValueField = "Id";
                        ddlClient.DataBind();
                    }
                }
                else
                {
                    // 18-Aug 2020 by Jignesh
                    // added: DBClient.DepartmentMaster.UserId == Convert.ToInt64(Global.UserId)
                    var Client = from DBClient in dbobj.ClientMasters
                                 where DBClient.IsActive == true && DBClient.DepartmentId == Convert.ToInt64(ddlDepartment.SelectedValue) && (DBClient.C_M1 == Global.UserM1 || DBClient.C_M2 == Global.UserM2)
                                 orderby DBClient.ClientName
                                 select new
                                 {
                                     Name = DBClient.ClientName,
                                     Id = DBClient.ClientId
                                 };
                    if (Client.Count() > 0)
                    {
                        ddlClient.DataSource = Client;
                        ddlClient.DataTextField = "Name";
                        ddlClient.DataValueField = "Id";
                        ddlClient.DataBind();
                    }
                }
                if (ddlClient.Items.Count > 1)
                {
                    ddlClient.Items.Insert(0, "-- All --");
                }
            }
            else
            {
                // 18-Aug 2020 by Jignesh
                // added: && (DBClient.C_M1 == Global.UserM1 || DBClient.C_M2 == Global.UserM2)
                ddlClient.Items.Clear();
                var Client = from DBClient in dbobj.ClientMasters
                             where DBClient.IsActive == true &&  DBClient.DepartmentId == Convert.ToInt64(ddlDepartment.SelectedValue) && (DBClient.C_M1 == Global.UserM1 || DBClient.C_M2 == Global.UserM2)
                             orderby DBClient.ClientName
                             select new
                             {
                                 Name = DBClient.ClientName,
                                 Id = DBClient.ClientId
                             };
                if (Client.Count() > 0)
                {
                    ddlClient.DataSource = Client;
                    ddlClient.DataTextField = "Name";
                    ddlClient.DataValueField = "Id";
                    ddlClient.DataBind();
                }
                if (ddlClient.Items.Count > 1)
                {
                    ddlClient.Items.Insert(0, "-- All --");
                }
            }
        }
    }

    protected void FillDepartment_xxNew()
    {
        if (Global.UserType == "DIRECTOR" || Global.DepartmentName == "ACCT")
        {
            var Department = from DBDepartment in dbobj.DepartmentMasters
                             where DBDepartment.IsActive == true
                             orderby DBDepartment.DepartmentName
                             select new
                             {
                                 Name = DBDepartment.DepartmentName,
                                 Id = DBDepartment.DepartmentId
                             };
            if (Department.Count() > 0)
            {
                ddlDepartment.DataSource = Department;
                ddlDepartment.DataTextField = "Name";
                ddlDepartment.DataValueField = "Id";
                ddlDepartment.DataBind();
            }
            ddlDepartment.Items.Insert(0, "-- All --");
        }
        else
        {
            var Department = from DBDepartment in dbobj.DepartmentMasters
                             where DBDepartment.IsActive == true && DBDepartment.UserId == Convert.ToInt64(Global.UserId)
                             orderby DBDepartment.DepartmentName
                             select new
                             {
                                 Name = DBDepartment.DepartmentName,
                                 Id = DBDepartment.DepartmentId
                             };
            if (Department.Count() > 0)
            {
                ddlDepartment.DataSource = Department;
                ddlDepartment.DataTextField = "Name";
                ddlDepartment.DataValueField = "Id";
                ddlDepartment.DataBind();
            }
            //if (ddlDepartment.Items.Count > 1)
            //{
            //    ddlDepartment.Items.Insert(0, "-- All --");
            //}
        }
    }

    protected void FillDepartment()
    {
        if (Global.UserType == "DIRECTOR" || Global.DepartmentName == "ACCT")
        {
            var Department = from DBDepartment in dbobj.DepartmentMasters
                             where DBDepartment.IsActive == true
                             orderby DBDepartment.DepartmentName
                             select new
                             {
                                 Name = DBDepartment.DepartmentName,
                                 Id = DBDepartment.DepartmentId
                             };
            if (Department.Count() > 0)
            {
                ddlDepartment.DataSource = Department;
                ddlDepartment.DataTextField = "Name";
                ddlDepartment.DataValueField = "Id";
                ddlDepartment.DataBind();
            }
            ddlDepartment.Items.Insert(0, "-- All --");
        }
        else
        {
            var Department = from DBDepartment in dbobj.DepartmentMasters
                             where DBDepartment.IsActive == true && (DBDepartment.DepartmentId == Convert.ToInt64(Global.Department) ||  DBDepartment.UserId == Convert.ToInt64(Global.UserId))
                             orderby DBDepartment.DepartmentName
                             select new
                             {
                                 Name = DBDepartment.DepartmentName,
                                 Id = DBDepartment.DepartmentId
                             };
            if (Department.Count() > 0)
            {
                ddlDepartment.DataSource = Department;
                ddlDepartment.DataTextField = "Name";
                ddlDepartment.DataValueField = "Id";
                ddlDepartment.DataBind();
            }
            //if (ddlDepartment.Items.Count > 1)
            //{
            //    ddlDepartment.Items.Insert(0, "-- All --");
            //}
        }
    }

    protected void txtFromMonth_TextChanged(object sender, EventArgs e)
    {
        txtToMonth_CalendarExtenderPlus.MinimumDate = DateTime.ParseExact(txtFromMonth.Text, "MMM-yyyy", null);
        FillGrid();
    }

    protected void gvMatrix_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 1; i < e.Row.Cells.Count - 1; i++)
            {
                if (e.Row.Cells[i].Text != "00.00")
                {
                    e.Row.Cells[i].CssClass = "bkcolor";
                }
            }
        }
    }

    protected void txtToMonth_TextChanged(object sender, EventArgs e)
    {
        FillGrid();
    }

    protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillClient();
        FillGrid();
    }

    protected void ddlClient_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        // Report excel sheet

        Response.Clear();
        Response.Buffer = true;
        string filename = "MonthlyInvoiceSummary_MITPLInvoice.xls";
        Response.AddHeader("content-disposition", "attachment;filename=" + filename);
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";

        using (System.IO.StringWriter sw = new System.IO.StringWriter())
        {
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            //To Export all pages            
            gvMatrix.AllowPaging = false;
            FillGrid();

            gvMatrix.HeaderRow.BackColor = System.Drawing.Color.White;
            foreach (TableCell cell in gvMatrix.HeaderRow.Cells)
                cell.BackColor = gvMatrix.HeaderStyle.BackColor;

            foreach (GridViewRow row in gvMatrix.Rows)
            {
                row.BackColor = System.Drawing.Color.White;
                foreach (TableCell cell in row.Cells)
                {
                    if (row.RowIndex % 2 == 0)
                        cell.BackColor = gvMatrix.AlternatingRowStyle.BackColor;
                    else
                        cell.BackColor = gvMatrix.RowStyle.BackColor;
                    cell.CssClass = "textmode";
                }
            }

            gvMatrix.FooterRow.BackColor = System.Drawing.Color.White;
            foreach (TableCell cell in gvMatrix.FooterRow.Cells)
                cell.BackColor = gvMatrix.FooterStyle.BackColor;

            gvMatrix.RenderControl(hw);

            //style to format numbers to string
            string style = @"<style> .textmode { } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
    }

    protected void gvMatrix_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvMatrix.PageIndex = e.NewPageIndex;
        FillGrid();
    }
}