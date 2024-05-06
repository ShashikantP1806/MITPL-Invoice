using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CompanyPaymentDetails : System.Web.UI.Page
{

    MITInvoiceDataContext dbobj = new MITInvoiceDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Global.UserType == "BUSINESS UNIT MANAGER" || Global.UserType == "DIRECTOR")
        {
            if (!IsPostBack)
            {

                mvCompanyPaymentDetails.ActiveViewIndex = 0;
                ddlContactPerson.Enabled = false;
                FillDepartmentSearch();
                FillConsultant();
                FillGrid();
            }

            if (Global.UserType == "DIRECTOR")
            {
                btnPaymentDetails.Visible = false;
                //gridCompany.Columns[11].Visible = false;
            }

        }
        else
        {
            Response.Redirect("Authorize");
        }
    }



    protected void FillDepartmentSearch()
    {
        if (Global.UserType == "DIRECTOR" || Global.DepartmentName == "ACCT")
        {
            var Department = from DBData in dbobj.DepartmentMasters
                             where DBData.IsActive == true
                             orderby DBData.DepartmentName
                             select new
                             {
                                 DepartmentId = DBData.DepartmentId,
                                 DepartmentName = DBData.DepartmentName
                             };
            if (Department.Count() > 0)
            {
                ddlDepartmentSearch.DataSource = Department;
                ddlDepartmentSearch.DataTextField = "DepartmentName";
                ddlDepartmentSearch.DataValueField = "DepartmentId";
                ddlDepartmentSearch.DataBind();
            }
            if (ddlDepartmentSearch.Items.Count > 1)
            {
                ddlDepartmentSearch.Items.Insert(0, "-- All --");
            }
        }
        else
        {
            var Department = from DBData in dbobj.DepartmentMasters
                             where DBData.IsActive == true
                             &&
                             (DBData.DepartmentId == Convert.ToInt64(Global.Department) || DBData.UserId == Convert.ToInt64(Global.UserId))
                             orderby DBData.DepartmentName
                             select new
                             {
                                 DepartmentId = DBData.DepartmentId,
                                 DepartmentName = DBData.DepartmentName
                             };
            if (Department.Count() > 0)
            {
                ddlDepartmentSearch.DataSource = Department;
                ddlDepartmentSearch.DataTextField = "DepartmentName";
                ddlDepartmentSearch.DataValueField = "DepartmentId";
                ddlDepartmentSearch.DataBind();
            }
            if (ddlDepartmentSearch.Items.Count > 1)
            {
                ddlDepartmentSearch.Items.Insert(0, "-- All --");
            }
        }
    }

    protected void FillGrid()
    {

        if (ddlDepartmentSearch.SelectedIndex != -1)
        {
            TextBox txtMasterSearch = (TextBox)this.Master.FindControl("txtMasterSearch");
            if (txtMasterSearch.Text == "")
            {
                #region  Only for Director and Account
                if (Global.UserType == "DIRECTOR" || Global.DepartmentName == "ACCT")
                {
                    if (ddlDepartmentSearch.SelectedItem.ToString() == "-- All --")
                    {
                        #region For All Department
                        var PayDetails = (from DBData in dbobj.PaymentDetailsMasters
                                          where DBData.IsDelete == false  && (DBData.InvoiceMaster.ClientMaster.C_M1 == Global.UserM1 || DBData.InvoiceMaster.ClientMaster.C_M2 == Global.UserM2)
                                          select new
                                          {
                                              CPDID = DBData.PaymentID,
                                              InvoiceNumber = DBData.InvoiceMaster.InvoiceNumber,
                                              InvoiceAmount = DBData.InvoiceCurrency + " " + DBData.InvoiceAmount,
                                              InvoiceCurrency = DBData.InvoiceCurrency,
                                              AmountReceived = DBData.AmountReceived,
                                              ConsultantName = DBData.CompanyMaster.CompanyName,
                                              ContactPersonName = DBData.Individual_ContactMaster.ContactPerson,
                                              PaidCurrency = DBData.PaidCurrency,
                                              PaidAmount = DBData.PaidAmount,
                                              Remarks = DBData.Remarks

                                          }).ToList();

                        if (PayDetails.Count() > 0)
                        {
                            gridPaymentDetails.DataSource = PayDetails;
                            gridPaymentDetails.DataBind();

                        }
                        else
                        {
                            BlankGrid();
                        }
                        #endregion
                    }
                    else
                    {
                        #region For Selected Department
                        
                        var PayDetails = (from DBData in dbobj.PaymentDetailsMasters
                                          where DBData.IsDelete == false && DBData.InvoiceMaster.ClientMaster.DepartmentId == Convert.ToInt64(ddlDepartmentSearch.SelectedItem.Value.ToString()) && (DBData.InvoiceMaster.ClientMaster.C_M1 == Global.UserM1 || DBData.InvoiceMaster.ClientMaster.C_M2 == Global.UserM2)
                                          //where DBData.IsDelete == false && DBData.InvoiceMaster.ClientMaster.DepartmentId == Convert.ToInt64(Global.Department)  && (DBData.InvoiceMaster.ClientMaster.C_M1 == Global.UserM1 || DBData.InvoiceMaster.ClientMaster.C_M2 == Global.UserM2)
                                          select new
                                          { 
                                              CPDID = DBData.PaymentID,
                                              InvoiceNumber = DBData.InvoiceMaster.InvoiceNumber,
                                              InvoiceAmount = DBData.InvoiceCurrency + " " + DBData.InvoiceAmount,
                                              InvoiceCurrency = DBData.InvoiceCurrency,
                                              AmountReceived = DBData.AmountReceived,
                                              ConsultantName = DBData.CompanyMaster.CompanyName,
                                              ContactPersonName = DBData.Individual_ContactMaster.ContactPerson,
                                              PaidCurrency = DBData.PaidCurrency,
                                              PaidAmount = DBData.PaidAmount,
                                              Remarks = DBData.Remarks
                        
                                          }).ToList();

                        if (PayDetails.Count() > 0)
                        {
                            gridPaymentDetails.DataSource = PayDetails;
                            gridPaymentDetails.DataBind();

                        }
                        else
                        {
                            BlankGrid();
                        }
                        #endregion
                    }


                }
                else
                {
                    if (ddlDepartmentSearch.SelectedItem.ToString() == "-- All --")
                    {
                        #region For All Department and User ID
                        //&& DBData.CompanyMaster.DepartmentMaster.UserId == Convert.ToInt64(Global.UserId) 
                        var PayDetails = (from DBData in dbobj.PaymentDetailsMasters
                                          where DBData.IsDelete == false && (DBData.InvoiceMaster.ClientMaster.C_M1 == Global.UserM1 || DBData.InvoiceMaster.ClientMaster.C_M2 == Global.UserM2)
                                          select new
                                          {
                                              CPDID = DBData.PaymentID,
                                              InvoiceNumber = DBData.InvoiceMaster.InvoiceNumber,
                                              InvoiceAmount = DBData.InvoiceCurrency + " " + DBData.InvoiceAmount,
                                              InvoiceCurrency = DBData.InvoiceCurrency,
                                              AmountReceived = DBData.AmountReceived,
                                              ConsultantName = DBData.CompanyMaster.CompanyName,
                                              ContactPersonName = DBData.Individual_ContactMaster.ContactPerson,
                                              PaidCurrency = DBData.PaidCurrency,
                                              PaidAmount = DBData.PaidAmount,
                                              Remarks = DBData.Remarks

                                          }).ToList();

                        if (PayDetails.Count() > 0)
                        {
                            gridPaymentDetails.DataSource = PayDetails;
                            gridPaymentDetails.DataBind();

                        }
                        else
                        {
                            BlankGrid();
                        }
                        #endregion
                    }
                    else
                    {
                        #region For Selected Department

                        //Convert.ToInt64(ddlDepartmentSearch.SelectedValue) 
                        var PayDetails = (from DBData in dbobj.PaymentDetailsMasters
                                          where DBData.IsDelete == false && DBData.InvoiceMaster.ClientMaster.DepartmentId == Convert.ToInt64(ddlDepartmentSearch.SelectedValue) && (DBData.InvoiceMaster.ClientMaster.C_M1 == Global.UserM1 || DBData.InvoiceMaster.ClientMaster.C_M2 == Global.UserM2)
                                          select new
                                          {
                                              CPDID = DBData.PaymentID,
                                              InvoiceNumber = DBData.InvoiceMaster.InvoiceNumber,
                                              InvoiceAmount = DBData.InvoiceCurrency + " " + DBData.InvoiceAmount,
                                              InvoiceCurrency = DBData.InvoiceCurrency,
                                              AmountReceived = DBData.AmountReceived,
                                              ConsultantName = DBData.CompanyMaster.CompanyName,
                                              ContactPersonName = DBData.Individual_ContactMaster.ContactPerson,
                                              PaidCurrency = DBData.PaidCurrency,
                                              PaidAmount = DBData.PaidAmount,
                                              Remarks = DBData.Remarks

                                          }).ToList();

                        if (PayDetails.Count() > 0)
                        {
                            gridPaymentDetails.DataSource = PayDetails;
                            gridPaymentDetails.DataBind();

                        }
                        else
                        {
                            BlankGrid();
                        }
                        #endregion
                    }
                }
                #endregion
            }
            else
            {
                if (Global.UserType == "DIRECTOR" || Global.DepartmentName == "ACCT")
                {

                    if (ddlDepartmentSearch.SelectedItem.ToString() == "-- All --")
                    {
                        #region For All Department and Contains department
                        var PayDetails = (from DBData in dbobj.PaymentDetailsMasters
                                          where DBData.IsDelete == false && DBData.CompanyMaster.DepartmentMaster.DepartmentName.Contains(txtMasterSearch.Text.Trim()) && (DBData.InvoiceMaster.ClientMaster.C_M1 == Global.UserM1 || DBData.InvoiceMaster.ClientMaster.C_M2 == Global.UserM2)
                                          select new
                                          {
                                              CPDID = DBData.PaymentID,
                                              InvoiceNumber = DBData.InvoiceMaster.InvoiceNumber,
                                              InvoiceAmount = DBData.InvoiceCurrency + " " + DBData.InvoiceAmount,
                                              InvoiceCurrency = DBData.InvoiceCurrency,
                                              AmountReceived = DBData.AmountReceived,
                                              ConsultantName = DBData.CompanyMaster.CompanyName,
                                              ContactPersonName = DBData.Individual_ContactMaster.ContactPerson,
                                              PaidCurrency = DBData.PaidCurrency,
                                              PaidAmount = DBData.PaidAmount,
                                              Remarks = DBData.Remarks

                                          }).ToList();

                        if (PayDetails.Count() > 0)
                        {
                            gridPaymentDetails.DataSource = PayDetails;
                            gridPaymentDetails.DataBind();

                        }
                        else
                        {
                            BlankGrid();
                        }
                        #endregion
                    }
                    else
                    {
                        #region For Selected Department and Contains department
                        var PayDetails = (from DBData in dbobj.PaymentDetailsMasters
                                          where DBData.IsDelete == false && DBData.InvoiceMaster.ClientMaster.DepartmentId == Convert.ToInt64(ddlDepartmentSearch.SelectedValue) && DBData.CompanyMaster.DepartmentMaster.DepartmentName.Contains(txtMasterSearch.Text.Trim()) && (DBData.InvoiceMaster.ClientMaster.C_M1 == Global.UserM1 || DBData.InvoiceMaster.ClientMaster.C_M2 == Global.UserM2)
                                          select new
                                          {
                                              CPDID = DBData.PaymentID,
                                              InvoiceNumber = DBData.InvoiceMaster.InvoiceNumber,
                                              InvoiceAmount = DBData.InvoiceCurrency + " " + DBData.InvoiceAmount,
                                              InvoiceCurrency = DBData.InvoiceCurrency,
                                              AmountReceived = DBData.AmountReceived,
                                              ConsultantName = DBData.CompanyMaster.CompanyName,
                                              ContactPersonName = DBData.Individual_ContactMaster.ContactPerson,
                                              PaidCurrency = DBData.PaidCurrency,
                                              PaidAmount = DBData.PaidAmount,
                                              Remarks = DBData.Remarks

                                          }).ToList();

                        if (PayDetails.Count() > 0)
                        {
                            gridPaymentDetails.DataSource = PayDetails;
                            gridPaymentDetails.DataBind();

                        }
                        else
                        {
                            BlankGrid();
                        }
                        #endregion
                    }


                }
                else
                {
                    if (ddlDepartmentSearch.SelectedItem.ToString() == "-- All --")
                    {
                        #region For All Department and Contains department
                        var PayDetails = (from DBData in dbobj.PaymentDetailsMasters
                                          where DBData.IsDelete == false && DBData.CompanyMaster.DepartmentMaster.DepartmentName.Contains(txtMasterSearch.Text.Trim()) && (DBData.InvoiceMaster.ClientMaster.C_M1 == Global.UserM1 || DBData.InvoiceMaster.ClientMaster.C_M2 == Global.UserM2)
                                          select new
                                          {
                                              CPDID = DBData.PaymentID,
                                              InvoiceNumber = DBData.InvoiceMaster.InvoiceNumber,
                                              InvoiceAmount = DBData.InvoiceCurrency + " " + DBData.InvoiceAmount,
                                              InvoiceCurrency = DBData.InvoiceCurrency,
                                              AmountReceived = DBData.AmountReceived,
                                              ConsultantName = DBData.CompanyMaster.CompanyName,
                                              ContactPersonName = DBData.Individual_ContactMaster.ContactPerson,
                                              PaidCurrency = DBData.PaidCurrency,
                                              PaidAmount = DBData.PaidAmount,
                                              Remarks = DBData.Remarks

                                          }).ToList();

                        if (PayDetails.Count() > 0)
                        {
                            gridPaymentDetails.DataSource = PayDetails;
                            gridPaymentDetails.DataBind();

                        }
                        else
                        {
                            BlankGrid();
                        }
                        #endregion
                    }
                    else
                    {
                        #region For Selected Department and Contains department
                        var PayDetails = (from DBData in dbobj.PaymentDetailsMasters
                                          where DBData.IsDelete == false && DBData.InvoiceMaster.ClientMaster.DepartmentId == Convert.ToInt64(ddlDepartmentSearch.SelectedValue) && DBData.CompanyMaster.DepartmentMaster.DepartmentName.Contains(txtMasterSearch.Text.Trim()) && (DBData.InvoiceMaster.ClientMaster.C_M1 == Global.UserM1 || DBData.InvoiceMaster.ClientMaster.C_M2 == Global.UserM2)
                                          select new
                                          {
                                              CPDID = DBData.PaymentID,
                                              InvoiceNumber = DBData.InvoiceMaster.InvoiceNumber,
                                              InvoiceAmount = DBData.InvoiceCurrency + " " + DBData.InvoiceAmount,
                                              InvoiceCurrency = DBData.InvoiceCurrency,
                                              AmountReceived = DBData.AmountReceived,
                                              ConsultantName = DBData.CompanyMaster.CompanyName,
                                              ContactPersonName = DBData.Individual_ContactMaster.ContactPerson,
                                              PaidCurrency = DBData.PaidCurrency,
                                              PaidAmount = DBData.PaidAmount,
                                              Remarks = DBData.Remarks

                                          }).ToList();

                        if (PayDetails.Count() > 0)
                        {
                            gridPaymentDetails.DataSource = PayDetails;
                            gridPaymentDetails.DataBind();

                        }
                        else
                        {
                            BlankGrid();
                        }
                        #endregion
                    }
                }
            }
        }
        else
            BlankGrid();
        //
        //                var PayDetails = (from DBData in dbobj.PaymentDetailsMasters
        //                  where DBData.IsDelete == false
        //                  where DBData.InvoiceMaster.ClientMaster.DepartmentId == Convert.ToInt64(Global.Department)
        //                  select new
        //                  {
        //                      CPDID = DBData.PaymentID,
        //                      InvoiceNumber = DBData.InvoiceMaster.InvoiceNumber,
        //                      InvoiceAmount = DBData.InvoiceCurrency + " " + DBData.InvoiceAmount,
        //                      InvoiceCurrency = DBData.InvoiceCurrency,
        //                      AmountReceived = DBData.AmountReceived,
        //                      ConsultantName = DBData.CompanyMaster.CompanyName,
        //                      ContactPersonName = DBData.Individual_ContactMaster.ContactPerson,
        //                      PaidCurrency = DBData.PaidCurrency,
        //                      PaidAmount = DBData.PaidAmount,
        //                      Remarks = DBData.Remarks
        //
        //                  }).ToList();
        //
        //if (PayDetails.Count() > 0)
        //{
        //    gridPaymentDetails.DataSource = PayDetails;
        //    gridPaymentDetails.DataBind();
        //
        //}
        //else
        //{
        //    BlankGrid();
        //}
    }

    protected void BlankGrid()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("CPDID");
        dt.Columns.Add("InvoiceNumber");
        dt.Columns.Add("InvoiceAmount");
        dt.Columns.Add("InvoiceCurrency");
        dt.Columns.Add("AmountReceived");
        dt.Columns.Add("ConsultantName");
        dt.Columns.Add("ContactPersonName");
        dt.Columns.Add("PaidCurrency");
        dt.Columns.Add("PaidAmount");
        dt.Columns.Add("Remarks");


        gridPaymentDetails.DataSource = dt;
        dt.Rows.Add(dt.NewRow());
        gridPaymentDetails.DataBind();
        int TotalCols = gridPaymentDetails.Rows[0].Cells.Count;
        gridPaymentDetails.Rows[0].Cells.Clear();
        gridPaymentDetails.Rows[0].Cells.Add(new TableCell());
        gridPaymentDetails.Rows[0].Cells[0].ColumnSpan = TotalCols;
        gridPaymentDetails.Rows[0].Cells[0].Text = "No records to display";
    }

    protected void Clear()
    {
        txtInvNumber.Text = string.Empty;
        //ddlDepartment.Items.Clear();
        lblInvAmount.Text = string.Empty;
        lblInvCurrency.Text = string.Empty;
        txtAmtReceived.Text = string.Empty;
        ddlConsultant.Items.Clear();
        ddlContactPerson.Items.Clear();
        txtPaidCurrency.Text = string.Empty;
        txtPaidAmount.Text = string.Empty;
        txtRemarks.Text = string.Empty;
        btnSave.Text = "Save";

    }

    protected void FillConsultant()
    {
        var Consultant = from DBData in dbobj.CompanyMasters
                         where DBData.IsDelete == false
                         orderby DBData.CompanyName
                         select new
                         {
                             CompanyID = DBData.CompanyID,
                             CompanyName = DBData.CompanyName
                         };
        if (Consultant.Count() > 0)
        {
            ddlConsultant.DataSource = Consultant;
            ddlConsultant.DataTextField = "CompanyName";
            ddlConsultant.DataValueField = "CompanyID";
            ddlConsultant.DataBind();
        }

        ddlConsultant.Items.Insert(0, "-- Select --");

    }

    protected void imgEdit_Click(object sender, ImageClickEventArgs e)
    {
        mvCompanyPaymentDetails.ActiveViewIndex = 1;
        btnSave.Text = "Update";
        ImageButton ibtn = (ImageButton)sender;
        GridViewRow gr = (GridViewRow)ibtn.NamingContainer;
        Label lblCPDID = (Label)gr.FindControl("lblCPDID");

        var UpDetails = from DBData in dbobj.PaymentDetailsMasters
                        where DBData.IsDelete == false && DBData.PaymentID == Convert.ToInt64(lblCPDID.Text)
                        select DBData;
        if (UpDetails.Count() > 0)
        {
            var singleCompany = UpDetails.Single();
            ViewState["PaymentID"] = singleCompany.PaymentID;
            lblInvID.Text =  singleCompany.InvoiceMaster.InvoiceId.ToString();
            txtInvNumber.Text = singleCompany.InvoiceMaster.InvoiceNumber;
            lblInvAmount.Text = singleCompany.InvoiceAmount.ToString();
            lblInvCurrency.Text = singleCompany.InvoiceCurrency;
            txtAmtReceived.Text = singleCompany.AmountReceived.ToString();

            FillConsultant();
            ddlConsultant.Items.FindByValue(singleCompany.Consultant_Company.ToString()).Selected = true;

            FillContactPerson(Convert.ToInt64(singleCompany.Consultant_Company.ToString()));
            ddlContactPerson.Enabled = true;
            if (singleCompany.IndividualName != null)
            {
                ddlContactPerson.Items.FindByValue(singleCompany.IndividualName.ToString()).Selected = true;

            }

            //string CompanyCurrency = GetCurrencyCode(Convert.ToInt64(singleCompany.Consultant_Company.ToString()));

            
            txtPaidCurrency.Text = singleCompany.PaidCurrency.ToString();
            txtPaidAmount.Text = singleCompany.PaidAmount.ToString();
            txtRemarks.Text = singleCompany.Remarks;
        }
    }

    protected void btnPaymentDetails_Click(object sender, EventArgs e)
    {

        FillConsultant();
        mvCompanyPaymentDetails.ActiveViewIndex = 1;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        FillGrid();
        Clear();
        mvCompanyPaymentDetails.ActiveViewIndex = 0;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Page page1 = HttpContext.Current.Handler as Page;
        if (btnSave.Text == "Save")
        {
            #region New Payment Details

            if (txtInvNumber.Text.Trim() != "")
            {
                if (ddlConsultant.Text != "-- Select --")
                {
                    PaymentDetailsMaster PDM = new PaymentDetailsMaster();
                    PDM.InvoiceNo = Convert.ToInt64(lblInvID.Text);
                    PDM.InvoiceAmount = Convert.ToDecimal(lblInvAmount.Text);
                    //PDM.InvoiceCurrency = Convert.ToDecimal(lblInvCurrency.Text);
                    PDM.AmountReceived = txtAmtReceived.Text.Trim() == "" ? 0 : Convert.ToDecimal(txtAmtReceived.Text.Trim());
                    PDM.Consultant_Company = Convert.ToInt64(ddlConsultant.SelectedItem.Value);
                    PDM.IndividualName = ddlContactPerson.Text == "-- Select --" ? (Int64?)null : Convert.ToInt64(ddlContactPerson.SelectedItem.Value);
                    PDM.PaidCurrency = txtPaidCurrency.Text.Trim() == "" ? string.Empty : txtPaidCurrency.Text.Trim();
                    PDM.PaidAmount = txtPaidAmount.Text.Trim() == "" ? 0 : Convert.ToDecimal(txtPaidAmount.Text);
                    PDM.PaidDate = DateTime.Now;
                    PDM.Remarks = txtRemarks.Text.Trim();
                    PDM.IsDelete = false;
                    dbobj.PaymentDetailsMasters.InsertOnSubmit(PDM);
                    dbobj.SubmitChanges();
                    dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                    ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('Company added successfully')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('Please select consultant')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('Please select invoice')", true);
            }

            #endregion
        }
        if (btnSave.Text == "Update")
        {
            #region Update Existing Details
            var UpPayDetails = from DBData in dbobj.PaymentDetailsMasters
                               where DBData.IsDelete == false && DBData.PaymentID == Convert.ToInt64(ViewState["PaymentID"].ToString())
                               select DBData;
            var singlePayDetails = UpPayDetails.Single();

            singlePayDetails.InvoiceNo = Convert.ToInt64(lblInvID.Text);
            singlePayDetails.InvoiceAmount = Convert.ToDecimal(lblInvAmount.Text);
            singlePayDetails.InvoiceCurrency = lblInvCurrency.Text;
            singlePayDetails.AmountReceived = txtAmtReceived.Text.Trim() == "" ? 0 : Convert.ToDecimal(txtAmtReceived.Text.Trim());
            singlePayDetails.Consultant_Company = Convert.ToInt64(ddlConsultant.SelectedItem.Value);
            singlePayDetails.IndividualName = ddlContactPerson.Text == "-- Select --" ? (Int64?)null : Convert.ToInt64(ddlContactPerson.SelectedItem.Value);
            singlePayDetails.PaidCurrency = txtPaidCurrency.Text.Trim() == "" ? string.Empty : txtPaidCurrency.Text.Trim();
            singlePayDetails.PaidAmount = txtPaidAmount.Text.Trim() == "" ? 0 : Convert.ToDecimal(txtPaidAmount.Text);
            singlePayDetails.Remarks = txtRemarks.Text.Trim();

            dbobj.SubmitChanges();
            dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
            btnSave.Text = "Save";
            #endregion

        }
        mvCompanyPaymentDetails.ActiveViewIndex = 0;
        FillGrid();
        Clear();
    }



    protected void imgInvList_Click(object sender, ImageClickEventArgs e)
    {
        txtSearchInvList.Text = string.Empty;
        SearchInvoice("");
        mvCompanyPaymentDetails.ActiveViewIndex = 2;
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        mvCompanyPaymentDetails.ActiveViewIndex = 1;
    }

    protected void rblInvList_SelectedIndexChanged(object sender, EventArgs e)
    { 
        string sInvoiceID = rblInvList.SelectedItem.Value;

        // string message = "Value: " + rblInvList.SelectedItem.Value;
        // message += " Text: " + rblInvList.SelectedItem.Text;
        // ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + message + "');", true);

        string ClientCurrency = string.Empty, ClientCurrencyCode = string.Empty;
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
                       where InvoiceData.InvoiceId == Convert.ToInt64(sInvoiceID)
                       select DBData;
        if (CurrData.Count() > 0)
        {
            ClientCurrency = "&#" + (CurrData.Single().CurrencySymbol + ";").Replace(";;", ";");
            ClientCurrencyCode = CurrData.Single().CurrencyCode;
        }

        lblInvCurrency.Text = ClientCurrencyCode;
        decimal totalAmount = Math.Round(Convert.ToDecimal(((from DBData in dbobj.InvoiceDetailsMasters
                                                             where DBData.InvoiceId == Convert.ToInt64(sInvoiceID)
                                                             select DBData.TotalAmt.Value).Count() == 0 ? "0.00" : Convert.ToString(((from DBData in dbobj.InvoiceDetailsMasters
                                                                                                                                      where DBData.InvoiceId == Convert.ToInt64(sInvoiceID)
                                                                                                                                      select DBData.TotalAmt).Sum())))), 2, MidpointRounding.ToEven);

        lblInvAmount.Text = totalAmount.ToString();

        txtInvNumber.Text = rblInvList.SelectedItem.Text;
        lblInvID.Text = rblInvList.SelectedItem.Value;

        mvCompanyPaymentDetails.ActiveViewIndex = 1;
    }

    protected void btnSearchInv_Click(object sender, EventArgs e)
    {
        //if (txtSearchInvList.Text.Trim() != "")
        SearchInvoice(txtSearchInvList.Text.Trim());
    }

    protected void SearchInvoice(string SeqNo)
    {
        if (SeqNo.Trim() != "")
        {
            var InvListSearch = (from dbInv in dbobj.InvoiceMasters
                                 join dbClient in dbobj.ClientMasters
                                 on dbInv.ClientId equals dbClient.ClientId
                                 where (dbInv.InvoiceSeqNo.ToString().Contains(SeqNo.ToString()) || (dbInv.InvoiceSeqNo.ToString() + dbInv.Revision.ToString()).Contains(SeqNo)) && Convert.ToInt64(Global.Department) == dbClient.DepartmentId && dbInv.IsDeleted == false && (dbClient.C_M1 == Global.UserM1 || dbClient.C_M2 == Global.UserM2)
                                 orderby dbInv.InvoiceDate descending
                                 select new
                                 {
                                     InvoiceId = dbInv.InvoiceId,
                                     InvoiceNumber = dbInv.InvoiceNumber
                                 });

            if (InvListSearch.Count() > 0)
            {
                rblInvList.DataSource = InvListSearch;
                rblInvList.DataTextField = "InvoiceNumber";
                rblInvList.DataValueField = "InvoiceId";
                rblInvList.DataBind();
                lblInvListFound.Visible = false;
            }
            else
            {
                rblInvList.Items.Clear();
                rblInvList.DataSource = null;
                rblInvList.DataTextField = "InvoiceNumber";
                rblInvList.DataValueField = "InvoiceId";
                rblInvList.DataBind();
                lblInvListFound.Visible = true;
            }
        }
        else
        {

            int iInvListShow = Convert.ToInt32(InvListShow.Value);

            var InvList = (from dbInv in dbobj.InvoiceMasters
                           join dbClient in dbobj.ClientMasters
                           on dbInv.ClientId equals dbClient.ClientId
                           where Convert.ToInt64(Global.Department) == dbClient.DepartmentId && dbInv.IsDeleted == false && (dbClient.C_M1 == Global.UserM1 || dbClient.C_M2 == Global.UserM2)
                           orderby dbInv.InvoiceDate descending
                           select new
                           {
                               InvoiceId = dbInv.InvoiceId,
                               InvoiceNumber = dbInv.InvoiceNumber
                           }).Take(iInvListShow);

            if (InvList.Count() > 0)
            {
                rblInvList.DataSource = InvList;
                rblInvList.DataTextField = "InvoiceNumber";
                rblInvList.DataValueField = "InvoiceId";
                rblInvList.DataBind();
                lblInvListFound.Visible = false;
            }
            else
            {
                rblInvList.Items.Clear();
                rblInvList.DataSource = null;
                rblInvList.DataTextField = "InvoiceNumber";
                rblInvList.DataValueField = "InvoiceId";
                rblInvList.DataBind();
                lblInvListFound.Visible = true;
            }
        }
        mvCompanyPaymentDetails.ActiveViewIndex = 2;
    }

    protected string GetCurrencyCode(Int64 ConsultantID)
    {
        string strCurrencyCode = string.Empty;
        var Consultant = from DBData in dbobj.CompanyMasters
                         where DBData.IsDelete == false && DBData.CompanyID == Convert.ToInt64(ConsultantID)
                         orderby DBData.CompanyName
                         select new
                         {
                             CompanyID = DBData.CompanyID,
                             CompanyName = DBData.CompanyName,
                             CurrencyCode = DBData.CurrencyMaster.CurrencyCode
                         };
        if (Consultant.Count() > 0)
        {
            ViewState["CompanyCurrency"] = Consultant.Single().CurrencyCode;
            strCurrencyCode = Consultant.Single().CurrencyCode;
        }

        return strCurrencyCode;
    }

    protected void ddlConsultant_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlConsultant.Text != "-- Select --")
        {
            txtPaidCurrency.Text = GetCurrencyCode(Convert.ToInt64(ddlConsultant.SelectedItem.Value));

            FillContactPerson(Convert.ToInt64(ddlConsultant.SelectedItem.Value));
            ddlContactPerson.Enabled = true;
        }
        else
        {
            ddlContactPerson.Items.Clear();
            ddlContactPerson.Enabled = false;
        }
    }

    protected void FillContactPerson(Int64? ConsultantID)
    {
        var ContactPerson = from DBData in dbobj.Individual_ContactMasters
                            where DBData.IsDelete == false && DBData.CompanyID == ConsultantID
                            orderby DBData.ContactPerson
                            select new
                            {
                                IndividualID = DBData.IndividualID,
                                ContactPerson = DBData.ContactPerson
                            };
        if (ContactPerson.Count() > 0)
        {
            ddlContactPerson.DataSource = ContactPerson;
            ddlContactPerson.DataTextField = "ContactPerson";
            ddlContactPerson.DataValueField = "IndividualID";
            ddlContactPerson.DataBind();

        }

        ddlContactPerson.Items.Insert(0, "-- Select --");
    }

    protected void gridPaymentDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        /* if (e.Row.RowType == DataControlRowType.DataRow)
         {
             Label lblDepartmentId = (Label)e.Row.FindControl("lblDepartmentId");
             Label lbl1 = (Label)e.Row.FindControl("lbl1");
             ImageButton imgEdit = (ImageButton)e.Row.FindControl("imgEdit");
             ImageButton imgDelete = (ImageButton)e.Row.FindControl("imgDelete");
             LinkButton lbtnAddContact = (LinkButton)e.Row.FindControl("lbtnAddContact");

             var DepartmentId = from DB in dbobj.DepartmentMasters
                                where DB.IsActive == true && (DB.DepartmentId == Convert.ToInt64(Global.Department) || DB.UserId == Convert.ToInt64(Global.UserId))
                                select DB.DepartmentId;
             if (lblDepartmentId.Text != "")
             {
                 if (!DepartmentId.Contains(Convert.ToInt64(lblDepartmentId.Text)))
                 {
                     imgEdit.Visible = false;
                     imgDelete.Visible = false;
                     lbl1.Visible = false;
                     //gridCompany.Columns[11].Visible = false;
                     lbtnAddContact.Text = "View";
                 }
             }
         }*/
    }

    protected void gridPaymentDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    protected void gridPaymentDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Page page1 = HttpContext.Current.Handler as Page;
        //TextBox txtCountryName = (TextBox)gridClient.FooterRow.FindControl("txtCountryName");
        switch (e.CommandName)
        {
            case "Delete":
                GridViewRow gr = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                Label lblCPDID = (Label)gr.FindControl("lblCPDID");
                Label lblCompanyName = (Label)gr.FindControl("lblCompanyName");


                var DelClient = from DelC in dbobj.PaymentDetailsMasters
                                where DelC.PaymentID == Convert.ToInt64(lblCPDID.Text)
                                select DelC;
                if (DelClient.Count() > 0)
                {
                    var Client = DelClient.Single();
                    Client.IsDelete = true;
                    dbobj.SubmitChanges();
                    dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                    //ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + SingleClient.ClientName.ToUpper() + " client name deleted" + "')", true);
                    FillGrid();
                }
                break;
        }
    }

    protected void gridPaymentDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridPaymentDetails.PageIndex = e.NewPageIndex;
        FillGrid();
    }

    protected void ddlDepartmentSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
}