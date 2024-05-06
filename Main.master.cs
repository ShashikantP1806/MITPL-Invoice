using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Main : System.Web.UI.MasterPage
{
    MITInvoiceDataContext dbobj = new MITInvoiceDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserName"] != null)
        {
            switch (Global.UserType)
            {
                case "DIRECTOR":
                    lnkManageUsers.Visible = false;
                    lnkCurrency.Visible = false;
                    lnkContry.Visible = false;
                    lnkState.Visible = false;
                    lnkCity.Visible = false;
                    lnkNewInvoice.Visible = false;
                    liDepartmentMenu.Visible = false;
                    lnkFooterDepartment.Visible = false;
                    lnkMonthlySales.Visible = true;
                    lnkMonthlySalesNew.Visible = true; //// added by Jignesh Mistry on 15-Oct-2020
                    if (Global.UserM2)
                        liManageConsultant.Visible = true; //// added by Jignesh Mistry on 21-Oct-2020
                    
                    liPriceMenu.Visible = false; // Price Menu Visible false for Director
                    lnkPrice.Visible = false;
                    lnkPriceProcess.Visible = false;
                    lnkPriceType.Visible = false;
                    break;
                case "BUSINESS UNIT MANAGER":
                    lnkManageUsers.Visible = false;
                    liDepartmentMenu.Visible = false;
                    lnkContry.Visible = false;
                    lnkState.Visible = false;
                    lnkCity.Visible = false;
                    lnkCurrency.Visible = false;
                    lnkFooterDepartment.Visible = false;
                    lnkMonthlySales.Visible = false;
                    lnkMonthlySalesNew.Visible = false; //// added by Jignesh Mistry on 15-Oct-2020
                    liManageConsultant.Visible = true; //// added by Jignesh Mistry on 19-Nov-2020
                    liProformaInvoice.Visible = true;

                    // if (Global.UserM2)
                    // {
                    //     liManageConsultant.Visible = true; //// added by Jignesh Mistry on 19-Nov-2020
                    //     liProformaInvoice.Visible = true;
                    // }
                    // else
                    // {
                    //     liManageConsultant.Visible = false; //// added by Jignesh Mistry on 19-Nov-2020
                    //     liProformaInvoice.Visible = false;
                    // }
                    //if (Global.UserM1 && Global.UserM2)
                    //    liManageConsultant.Visible = true; //// added by Jignesh Mistry on 21-Oct-2020
                    //else
                    //    liManageConsultant.Visible = false; //// added by Jignesh Mistry on 21-Oct-2020
                    lnkPrice.Visible = false;
                    lnkPriceProcess.Visible = false;
                    lnkPriceType.Visible = true;
                    if (Global.DepartmentName == "ACCT")
                    {
                        lnkNewInvoice.Visible = false;
                        lnkMonthlySales.Visible = true;
                    }
                    break;
                case "BUSINESS ASSOCIATES":
                    liDepartmentMenu.Visible = false;
                    //liClientMenu.Visible = false;
                    lnkContry.Visible = false;
                    lnkState.Visible = false;
                    lnkCity.Visible = false;
                    lnkCurrency.Visible = false;

                    lnkManageUsers.Visible = false;
                    lnkNewInvoice.Visible = false;
                    lnkFooterDepartment.Visible = false;   
                    lnkFooterClient.Visible = false;
                    lnkMatrixReport.Visible = false;
                    lnkMonthlySales.Visible = false; 
                    lnkMonthlySalesNew.Visible = false; //// added by Jignesh Mistry on 15-Oct-2020
                    liManageConsultant.Visible = false; //// added by Jignesh Mistry on 21-Oct-2020
                    liProformaInvoice.Visible= false; //// added by Jignesh Mistry on 19-Nov-2020
                    lnkPrice.Visible = false;
                    lnkPriceProcess.Visible = false;
                    lnkPriceType.Visible = true;
                    if (Global.DepartmentName != "ACCT")
                    {
                        lnkNewInvoice.Visible = true;
                        if (Global.UserM2)
                        {
                            liProformaInvoice.Visible = true;
                        }
                        lnkMatrixReport.Visible = true;
                    }
                    else
                    {
                        if (Global.DepartmentName == "ACCT")
                        {
                            liPriceMenu.Visible = false;
                            // Added on 3-Jan-2022 SP (Give access for ProformaInvoice)
                            if (Global.UserEmpCode == "614")
                            { 
                                liProformaInvoice.Visible = true;
                                lnkProformaInvoice.Visible = false;
                            }
                        }
                        

                        lnkMatrixReport.Visible = true;
                        lnkMonthlySales.Visible = true;
                        lnkMonthlySalesNew.Visible = true; //// added by Jignesh Mistry on 15-Oct-2020
                        if (Global.DepartmentName != "ACCT" && Global.UserM2)
                        {
                            liManageConsultant.Visible = true; //// added by Jignesh Mistry on 21-Oct-2020
                            //liProformaInvoice.Visible = true; //// added by Jignesh Mistry on 19-Nov-2020
                        }
                    }
                    break;
                case "ADMINISTRATOR":
                    lnkClient.Visible = false;
                    liInvoiceMenu.Visible = false;
                    lnkFooterInvoice.Visible = false;
                    lnkFooterClient.Visible = false;
                    lnkMatrixReport.Visible = false;
                    lnkMonthlySales.Visible = false;
                    lnkMonthlySalesNew.Visible = false; //// added by Jignesh Mistry on 15-Oct-2020
                    liManageConsultant.Visible = false; //// added by Jignesh Mistry on 21-Oct-2020
                    liProformaInvoice.Visible = false; //// added by Jignesh Mistry on 19-Nov-2020
                    liPriceMenu.Visible = false; // Price Menu Visible false for Director
                    lnkPrice.Visible = false;
                    lnkPriceProcess.Visible = false;
                    lnkPriceType.Visible = false;
                    break;
                default:
                    break;
            }

            switch (this.ContentPlaceHolder1.Page.ToString())
            {
                case "ASP.city_aspx":
                    lnkCity_Click(this, e);
                    txtMasterSearch.Visible = true;
                    btnMasterSearch.Visible = true;
                    txtWMasterSearch.WatermarkText = "Search City";
                    break;
                case "ASP.company_aspx":
                    lnkManageConsultant_Click(this, e);
                    txtMasterSearch.Visible = true;
                    btnMasterSearch.Visible = true;
                    txtWMasterSearch.WatermarkText = "Search Company";
                    break;
                case "ASP.companypaymentdetails_aspx":
                    lnkInvPayment_Click(this, e);
                    txtMasterSearch.Visible = true;
                    btnMasterSearch.Visible = true;
                    txtWMasterSearch.WatermarkText = "Search Invoice Payment";
                    break;
                case "ASP.client_aspx":
                    lnkClient_Click(this, e);
                    txtMasterSearch.Visible = true;
                    btnMasterSearch.Visible = true;
                    txtWMasterSearch.WatermarkText = "Search Client";
                    break;
                case "ASP.clientcontact_aspx":
                    lnkClientContact_Click(this, e);
                    break;
                case "ASP.country_aspx":
                    lnkContry_Click(this, e);
                    txtMasterSearch.Visible = true;
                    btnMasterSearch.Visible = true;
                    txtWMasterSearch.WatermarkText = "Search Country";
                    break;
                case "ASP.currency_aspx":
                    lnkCurrency_Click(this, e);
                    txtMasterSearch.Visible = true;
                    btnMasterSearch.Visible = true;
                    txtWMasterSearch.WatermarkText = "Search Currency";
                    break;
                case "ASP.department_aspx":
                    lnkManageDepartments_Click(this, e);
                    txtMasterSearch.Visible = true;
                    btnMasterSearch.Visible = true;
                    txtWMasterSearch.WatermarkText = "Search Department";
                    break;
                case "ASP.home_aspx":
                    lnkHome_Click(this, e);
                    txtSearch.Visible = true;
                    txtMasterSearch.Visible = false;
                    break;
                case "ASP.invoice_aspx":
                    lnkNewInvoice_Click(this, e);
                    txtSearch.Visible = true;
                    txtMasterSearch.Visible = false;
                    break;
                case "ASP.price_aspx":
                    lnkPrice_Click(this, e);
                    break;
                case "ASP.priceprocess_aspx":
                    lnkPriceProcess_Click(this, e);
                    break;
                case "ASP.pricetype_aspx":
                    lnkPriceType_Click(this, e);
                    break;
                case "ASP.state_aspx":
                    lnkState_Click(this, e);
                    txtMasterSearch.Visible = true;
                    btnMasterSearch.Visible = true;
                    txtWMasterSearch.WatermarkText = "Search State";
                    break;
                case "ASP.user_aspx":
                    lnkUserMenu_Click(this, e);
                    txtMasterSearch.Visible = true;
                    btnMasterSearch.Visible = true;
                    txtWMasterSearch.WatermarkText = "Search User";
                    break;
                case "ASP.userprofile_aspx":
                    lnkUserProfile_Click(this, e);
                    //txtSearch.Visible = true;
                    txtMasterSearch.Visible = false;
                    break;
                case "ASP.viewinvoices_aspx":
                    lnkViewInvoice_Click(this, e);
                    txtMasterSearch.Visible = true;
                    btnMasterSearch.Visible = true;
                    txtWMasterSearch.WatermarkText = "Search Invoice";
                    break;
                case "ASP.invsearch_aspx":
                    txtSearch.Visible = true;
                    txtMasterSearch.Visible = false;
                    break;
                case "ASP.matrixreport_aspx":
                    lnkMatrixReport_Click(this, e);
                    txtSearch.Visible = true;
                    txtMasterSearch.Visible = false;
                    break;
                case "ASP.monthlysales_aspx":
                    lnkMonthlySales_Click(this, e);
                    txtSearch.Visible = true;
                    txtMasterSearch.Visible = false;
                    break;
                case "ASP.monthlysalesnew_aspx":
                    //// added by Jignesh Mistry on 15-Oct-2020
                    lnkMonthlySalesNew_Click(this, e);
                    txtSearch.Visible = true;
                    txtMasterSearch.Visible = false;
                    break;
                case "ASP.ProformaInvoice_aspx":
                    //// added by Shashikant Patel on 02-Nov-2020
                    lnkProformaInvoice_Click(this, e);
                    txtSearch.Visible = true;
                    txtMasterSearch.Visible = false;
                    break;
                case "ASP.ViewProformaInvoices_aspx":
                    //// added by Shashikant Patel on 02-Nov-2020
                    lnkViewProformaInvoice_Click(this, e);
                    txtSearch.Visible = true;
                    txtMasterSearch.Visible = false;
                    break;
                default:
                    liManageConsultant.Attributes.Add("class", "inactive");
                    liClientMenu.Attributes.Add("class", "inactive");
                    liDepartmentMenu.Attributes.Add("class", "inactive");
                    liHomeMenu.Attributes.Add("class", "inactive");
                    liInvoiceMenu.Attributes.Add("class", "inactive");
                    liPriceMenu.Attributes.Add("class", "inactive");
                    liUserMenu.Attributes.Add("class", "inactive");
                    break;
            }
        }
        else
        {
            if (this.ContentPlaceHolder1.Page.ToString() == "ASP.aboutus_aspx")
            {
                mainmenu.Visible = false;
                footermenu.Visible = false;
                loginntn.Visible = true;
                if (IsPostBack)
                {
                    Response.Redirect("AboutUs");
                }
            }
            else if (this.ContentPlaceHolder1.Page.ToString() == "ASP.customerror_aspx")
            {
                mainmenu.Visible = false;
                footermenu.Visible = false;
                loginntn.Visible = true;
                if (IsPostBack)
                {
                    Response.Redirect("Error");
                }
            }
            else
                Response.Redirect("Login");
        }
    }

    protected void lbtnLogout_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Response.Redirect("Login");
    }

    protected void lnkManageDepartments_Click(object sender, EventArgs e)
    {
        liManageConsultant.Attributes.Add("class", "inactive");
        liClientMenu.Attributes.Add("class", "inactive");
        liDepartmentMenu.Attributes.Add("class", "active");
        liHomeMenu.Attributes.Add("class", "inactive");
        liInvoiceMenu.Attributes.Add("class", "inactive");
        liPriceMenu.Attributes.Add("class", "inactive");
        liUserMenu.Attributes.Add("class", "inactive");
    }

    protected void lnkHome_Click(object sender, EventArgs e)
    {
        liManageConsultant.Attributes.Add("class", "inactive");
        liClientMenu.Attributes.Add("class", "inactive");
        liDepartmentMenu.Attributes.Add("class", "inactive");
        liHomeMenu.Attributes.Add("class", "active");
        liInvoiceMenu.Attributes.Add("class", "inactive");
        liPriceMenu.Attributes.Add("class", "inactive");

        liUserMenu.Attributes.Add("class", "inactive");
    }

    protected void lnkUserMenu_Click(object sender, EventArgs e)
    {
        liManageConsultant.Attributes.Add("class", "inactive");
        liClientMenu.Attributes.Add("class", "inactive");
        liDepartmentMenu.Attributes.Add("class", "inactive");
        liHomeMenu.Attributes.Add("class", "inactive");
        liInvoiceMenu.Attributes.Add("class", "inactive");
        liPriceMenu.Attributes.Add("class", "inactive");
        liUserMenu.Attributes.Add("class", "active");
    }

    protected void lnkManageUsers_Click(object sender, EventArgs e)
    {
        liManageConsultant.Attributes.Add("class", "inactive");
        liClientMenu.Attributes.Add("class", "inactive");
        liDepartmentMenu.Attributes.Add("class", "inactive");
        liHomeMenu.Attributes.Add("class", "inactive");
        liInvoiceMenu.Attributes.Add("class", "inactive");
        liPriceMenu.Attributes.Add("class", "inactive");
        liUserMenu.Attributes.Add("class", "active");
    }

    protected void lnkUserProfile_Click(object sender, EventArgs e)
    {
        liManageConsultant.Attributes.Add("class", "inactive");
        liClientMenu.Attributes.Add("class", "inactive");
        liDepartmentMenu.Attributes.Add("class", "inactive");
        liHomeMenu.Attributes.Add("class", "inactive");
        liInvoiceMenu.Attributes.Add("class", "inactive");
        liPriceMenu.Attributes.Add("class", "inactive");
        liUserMenu.Attributes.Add("class", "active");
    }

    protected void lnkClient_Click(object sender, EventArgs e)
    {
        liManageConsultant.Attributes.Add("class", "inactive");
        liClientMenu.Attributes.Add("class", "active");
        liDepartmentMenu.Attributes.Add("class", "inactive");
        liHomeMenu.Attributes.Add("class", "inactive");
        liInvoiceMenu.Attributes.Add("class", "inactive");
        liPriceMenu.Attributes.Add("class", "inactive");
        liUserMenu.Attributes.Add("class", "inactive");
    }

    protected void lnkClientContact_Click(object sender, EventArgs e)
    {
        liManageConsultant.Attributes.Add("class", "inactive");
        liClientMenu.Attributes.Add("class", "active");
        liDepartmentMenu.Attributes.Add("class", "inactive");
        liHomeMenu.Attributes.Add("class", "inactive");
        liInvoiceMenu.Attributes.Add("class", "inactive");
        liPriceMenu.Attributes.Add("class", "inactive");
        liUserMenu.Attributes.Add("class", "inactive");
    }

    protected void lnkContry_Click(object sender, EventArgs e)
    {
        liManageConsultant.Attributes.Add("class", "inactive");
        liClientMenu.Attributes.Add("class", "active");
        liDepartmentMenu.Attributes.Add("class", "inactive");
        liHomeMenu.Attributes.Add("class", "inactive");
        liInvoiceMenu.Attributes.Add("class", "inactive");
        liPriceMenu.Attributes.Add("class", "inactive");
        liUserMenu.Attributes.Add("class", "inactive");
    }

    protected void lnkState_Click(object sender, EventArgs e)
    {
        liManageConsultant.Attributes.Add("class", "inactive");
        liClientMenu.Attributes.Add("class", "active");
        liDepartmentMenu.Attributes.Add("class", "inactive");
        liHomeMenu.Attributes.Add("class", "inactive");
        liInvoiceMenu.Attributes.Add("class", "inactive");
        liPriceMenu.Attributes.Add("class", "inactive");
        liUserMenu.Attributes.Add("class", "inactive");
    }

    protected void lnkCity_Click(object sender, EventArgs e)
    {
        liManageConsultant.Attributes.Add("class", "inactive");
        liClientMenu.Attributes.Add("class", "active");
        liDepartmentMenu.Attributes.Add("class", "inactive");
        liHomeMenu.Attributes.Add("class", "inactive");
        liInvoiceMenu.Attributes.Add("class", "inactive");
        liPriceMenu.Attributes.Add("class", "inactive");
        liUserMenu.Attributes.Add("class", "inactive");
    }

    protected void lnkCurrency_Click(object sender, EventArgs e)
    {
        liManageConsultant.Attributes.Add("class", "inactive");
        liClientMenu.Attributes.Add("class", "active");
        liDepartmentMenu.Attributes.Add("class", "inactive");
        liHomeMenu.Attributes.Add("class", "inactive");
        liInvoiceMenu.Attributes.Add("class", "inactive");
        liPriceMenu.Attributes.Add("class", "inactive");
        liUserMenu.Attributes.Add("class", "inactive");
    }

    protected void lnkNewInvoice_Click(object sender, EventArgs e)
    {
        liManageConsultant.Attributes.Add("class", "inactive");
        liClientMenu.Attributes.Add("class", "inactive");
        liDepartmentMenu.Attributes.Add("class", "inactive");
        liHomeMenu.Attributes.Add("class", "inactive");
        liInvoiceMenu.Attributes.Add("class", "active");
        liPriceMenu.Attributes.Add("class", "inactive");
        liUserMenu.Attributes.Add("class", "inactive");
    }

    protected void lnkViewInvoice_Click(object sender, EventArgs e)
    {
        liManageConsultant.Attributes.Add("class", "inactive");
        liClientMenu.Attributes.Add("class", "inactive");
        liDepartmentMenu.Attributes.Add("class", "inactive");
        liHomeMenu.Attributes.Add("class", "inactive");
        liInvoiceMenu.Attributes.Add("class", "active");
        liPriceMenu.Attributes.Add("class", "inactive");
        liUserMenu.Attributes.Add("class", "inactive");
    }

    protected void lnkPrice_Click(object sender, EventArgs e)
    {
        liManageConsultant.Attributes.Add("class", "inactive");
        liClientMenu.Attributes.Add("class", "inactive");
        liDepartmentMenu.Attributes.Add("class", "inactive");
        liHomeMenu.Attributes.Add("class", "inactive");
        liInvoiceMenu.Attributes.Add("class", "inactive");
        liPriceMenu.Attributes.Add("class", "active");
        liUserMenu.Attributes.Add("class", "inactive");
    }

    protected void lnkPriceProcess_Click(object sender, EventArgs e)
    {
        liManageConsultant.Attributes.Add("class", "inactive");
        liClientMenu.Attributes.Add("class", "inactive");
        liDepartmentMenu.Attributes.Add("class", "inactive");
        liHomeMenu.Attributes.Add("class", "inactive");
        liInvoiceMenu.Attributes.Add("class", "inactive");
        liPriceMenu.Attributes.Add("class", "active");
        liUserMenu.Attributes.Add("class", "inactive");
    }

    protected void lnkPriceType_Click(object sender, EventArgs e)
    {
        liManageConsultant.Attributes.Add("class", "inactive");
        liClientMenu.Attributes.Add("class", "inactive");
        liDepartmentMenu.Attributes.Add("class", "inactive");
        liHomeMenu.Attributes.Add("class", "inactive");
        liInvoiceMenu.Attributes.Add("class", "inactive");
        liPriceMenu.Attributes.Add("class", "active");
        liUserMenu.Attributes.Add("class", "inactive");
    }

    protected void lnkMatrixReport_Click(object sender, EventArgs e)
    {
        liManageConsultant.Attributes.Add("class", "inactive");
        liClientMenu.Attributes.Add("class", "inactive");
        liDepartmentMenu.Attributes.Add("class", "inactive");
        liHomeMenu.Attributes.Add("class", "inactive");
        liInvoiceMenu.Attributes.Add("class", "active");
        liPriceMenu.Attributes.Add("class", "inactive");
        liUserMenu.Attributes.Add("class", "inactive");
    }

    protected void lnkMonthlySales_Click(object sender, EventArgs e)
    {
        liManageConsultant.Attributes.Add("class", "inactive");
        liClientMenu.Attributes.Add("class", "inactive");
        liDepartmentMenu.Attributes.Add("class", "inactive");
        liHomeMenu.Attributes.Add("class", "inactive");
        liInvoiceMenu.Attributes.Add("class", "active");
        liPriceMenu.Attributes.Add("class", "inactive");
        liUserMenu.Attributes.Add("class", "inactive");
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (txtSearch.Text.Trim() != "")
        {
            Response.Redirect("InvSearch.aspx?" + Global.Encrypt("inv=" + txtSearch.Text.Trim()));
        }
    }

    protected void SearchUser()
    {
        GridView gridUser = (GridView)ContentPlaceHolder1.FindControl("gridUser");
        if (gridUser != null)
        {
            try
            {
                int Empid = Convert.ToInt32(txtMasterSearch.Text.Trim());
                var UserData = from DBData in dbobj.UserMasters
                               orderby DBData.DepartmentMaster.DepartmentName, DBData.EmpId
                               where DBData.EmpId.ToString().Contains(Empid.ToString())
                               select new
                               {
                                   UserId = DBData.UserId,
                                   EmpId = DBData.EmpId,
                                   FirstName = DBData.FirstName,
                                   MiddleName = DBData.MiddleName,
                                   LastName = DBData.LastName,
                                   UserName = DBData.UserName.ToLower(),
                                   Department = DBData.DepartmentMaster.DepartmentName,
                                   Email = DBData.Email,
                                   UserType = DBData.UserType,
                                   ActiveInactive = DBData.IsActive == true ? "Active" : "Inactive"
                               };
                if (UserData.Count() > 0)
                {
                    gridUser.DataSource = UserData;
                    gridUser.DataBind();
                }
                else
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("UserId");
                    dt.Columns.Add("EmpId");
                    dt.Columns.Add("FirstName");
                    dt.Columns.Add("MiddleName");
                    dt.Columns.Add("LastName");
                    dt.Columns.Add("UserName");
                    dt.Columns.Add("Department");
                    dt.Columns.Add("Email");
                    dt.Columns.Add("UserType");
                    dt.Columns.Add("ActiveInactive");
                    gridUser.DataSource = dt;
                    dt.Rows.Add(dt.NewRow());
                    gridUser.DataBind();
                    int TotalCols = gridUser.Rows[0].Cells.Count;
                    gridUser.Rows[0].Cells.Clear();
                    gridUser.Rows[0].Cells.Add(new TableCell());
                    gridUser.Rows[0].Cells[0].ColumnSpan = TotalCols;
                    gridUser.Rows[0].Cells[0].Text = "No records to display";
                }
            }
            catch
            {
                var UserData = (from DBData in dbobj.UserMasters
                                orderby DBData.DepartmentMaster.DepartmentName, DBData.EmpId
                                where DBData.FirstName.Contains(txtMasterSearch.Text.Trim()) || DBData.LastName.Contains(txtMasterSearch.Text.Trim()) || DBData.DepartmentMaster.DepartmentName.Contains(txtMasterSearch.Text.Trim())
                                select new
                                {
                                    UserId = DBData.UserId,
                                    EmpId = DBData.EmpId,
                                    FirstName = DBData.FirstName,
                                    MiddleName = DBData.MiddleName,
                                    LastName = DBData.LastName,
                                    UserName = DBData.UserName.ToLower(),
                                    Department = DBData.DepartmentMaster.DepartmentName,
                                    Email = DBData.Email,
                                    UserType = DBData.UserType,
                                    ActiveInactive = DBData.IsActive == true ? "Active" : "Inactive"
                                });
                if (UserData.Count() > 0)
                {
                    gridUser.DataSource = UserData;
                    gridUser.DataBind();
                }
                else
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("UserId");
                    dt.Columns.Add("EmpId");
                    dt.Columns.Add("FirstName");
                    dt.Columns.Add("MiddleName");
                    dt.Columns.Add("LastName");
                    dt.Columns.Add("UserName");
                    dt.Columns.Add("Department");
                    dt.Columns.Add("Email");
                    dt.Columns.Add("UserType");
                    dt.Columns.Add("ActiveInactive");
                    gridUser.DataSource = dt;
                    dt.Rows.Add(dt.NewRow());
                    gridUser.DataBind();
                    int TotalCols = gridUser.Rows[0].Cells.Count;
                    gridUser.Rows[0].Cells.Clear();
                    gridUser.Rows[0].Cells.Add(new TableCell());
                    gridUser.Rows[0].Cells[0].ColumnSpan = TotalCols;
                    gridUser.Rows[0].Cells[0].Text = "No records to display";
                }
            }
        }
    }

    protected void SearchDepartment()
    {
        GridView gridDepartment = (GridView)ContentPlaceHolder1.FindControl("gridDepartment");
        if (gridDepartment != null)
        {
            var Department = from DBData in dbobj.DepartmentMasters
                             where DBData.IsActive == true && DBData.DepartmentName.Contains(txtMasterSearch.Text.Trim())
                             orderby DBData.DepartmentName
                             select new
                             {
                                 DepartmentId = DBData.DepartmentId,
                                 DepartmentName = DBData.DepartmentName,
                                 DepartmentHOD = DBData.UserMaster.FirstName + " " + DBData.UserMaster.LastName
                             };
            if (Department.Count() > 0)
            {
                gridDepartment.DataSource = Department;
                gridDepartment.DataBind();
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("DepartmentId");
                dt.Columns.Add("DepartmentName");
                dt.Columns.Add("DepartmentHOD");
                gridDepartment.DataSource = dt;
                dt.Rows.Add(dt.NewRow());
                gridDepartment.DataBind();
                int TotalCols = gridDepartment.Rows[0].Cells.Count;
                gridDepartment.Rows[0].Cells.Clear();
                gridDepartment.Rows[0].Cells.Add(new TableCell());
                gridDepartment.Rows[0].Cells[0].ColumnSpan = TotalCols;
                gridDepartment.Rows[0].Cells[0].Text = "No records to display";
            }
        }
    }

    protected void SearchClient()
    {
        GridView gridClient = (GridView)ContentPlaceHolder1.FindControl("gridClient");
        if (gridClient != null)
        {
            if (Global.UserType == "DIRECTOR" || Global.DepartmentName == "ACCT")
            {
                var Client = from DBData in dbobj.ClientMasters
                             where DBData.IsActive == true && DBData.ClientName.Contains(txtMasterSearch.Text.Trim())
                             orderby DBData.ClientName
                             select new
                             {
                                 ClientId = DBData.ClientId,
                                 ClientName = DBData.ClientName,
                                 DepartmentId = DBData.DepartmentId,
                                 Department = DBData.DepartmentMaster.DepartmentName,
                                 Phone = DBData.Phone,
                                 Address1 = "To: " + DBData.Address1 + ",<br>" + (from DBC in dbobj.CityMasters
                                                                                  where DBC.CityId == DBData.City1
                                                                                  select DBC.CityName).First() + ", " + (from DBState in dbobj.StateMasters
                                                                                                                         where DBState.StateId == DBData.State1
                                                                                                                         select DBState.StateName).First() + ",<br>" + (from DBCounty in dbobj.CountryMasters
                                                                                                                                                                        where DBCounty.CountryId == DBData.Country1
                                                                                                                                                                        select DBCounty.CountryName).First() + " - " + (DBData.Zip_Postal1 != null ? DBData.Zip_Postal1 : " "),
                                 Address2 = DBData.Address2 == null ? "-" : "To: " + DBData.Address2 + ",<br>" + ((from DBC in dbobj.CityMasters
                                                                                                                   where DBC.CityId == DBData.City2
                                                                                                                   select DBC.CityName).First() != null ? (from DBC in dbobj.CityMasters
                                                                                                                                                           where DBC.CityId == DBData.City2
                                                                                                                                                           select DBC.CityName).First() : "") + ", " + ((from DBState in dbobj.StateMasters
                                                                                                                                                                                                         where DBState.StateId == DBData.State2
                                                                                                                                                                                                         select DBState.StateName).First() != null ? (from DBState in dbobj.StateMasters
                                                                                                                                                                                                                                                      where DBState.StateId == DBData.State2
                                                                                                                                                                                                                                                      select DBState.StateName).First() : " ") + ",<br>" + ((from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                                                                                                                                             where DBCounty.CountryId == DBData.Country2
                                                                                                                                                                                                                                                                                                             select DBCounty.CountryName).First() != null ? (from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                                                                                                                                                                                             where DBCounty.CountryId == DBData.Country2
                                                                                                                                                                                                                                                                                                                                                             select DBCounty.CountryName).First() : "") + " - " + (DBData.Zip_Postal1 != null ? DBData.Zip_Postal1 : " "),
                                 Website = DBData.Website == null ? "-" : DBData.Website,
                                 //Currency = ""
                                 Currency = DBData.CurrencyMaster.CurrencyCode + "-" + ("&#" + DBData.CurrencyMaster.CurrencySymbol)
                             };
                if (Client.Count() > 0)
                {
                    gridClient.DataSource = Client;
                    gridClient.DataBind();
                }
                else
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("ClientId");
                    dt.Columns.Add("ClientName");
                    dt.Columns.Add("Phone");
                    dt.Columns.Add("Address1");
                    dt.Columns.Add("Address2");
                    dt.Columns.Add("Website");
                    dt.Columns.Add("DepartmentId");
                    dt.Columns.Add("Department");
                    dt.Columns.Add("Currency");
                    gridClient.DataSource = dt;
                    dt.Rows.Add(dt.NewRow());
                    gridClient.DataBind();
                    int TotalCols = gridClient.Rows[0].Cells.Count;
                    gridClient.Rows[0].Cells.Clear();
                    gridClient.Rows[0].Cells.Add(new TableCell());
                    gridClient.Rows[0].Cells[0].ColumnSpan = TotalCols;
                    gridClient.Rows[0].Cells[0].Text = "No records to display";
                }
            }
            else
            {
                var Client = from DBData in dbobj.ClientMasters
                             where DBData.IsActive == true && DBData.DepartmentId == Convert.ToInt64(Global.Department) && DBData.ClientName.Contains(txtMasterSearch.Text.Trim())
                             orderby DBData.ClientName
                             select new
                             {
                                 ClientId = DBData.ClientId,
                                 ClientName = DBData.ClientName,
                                 DepartmentId = DBData.DepartmentId,
                                 Department = DBData.DepartmentMaster.DepartmentName,
                                 Phone = DBData.Phone,
                                 Address1 = "To: " + DBData.Address1 + ",<br>" + (from DBC in dbobj.CityMasters
                                                                                  where DBC.CityId == DBData.City1
                                                                                  select DBC.CityName).First() + ", " + (from DBState in dbobj.StateMasters
                                                                                                                         where DBState.StateId == DBData.State1
                                                                                                                         select DBState.StateName).First() + ",<br>" + (from DBCounty in dbobj.CountryMasters
                                                                                                                                                                        where DBCounty.CountryId == DBData.Country1
                                                                                                                                                                        select DBCounty.CountryName).First() + " - " + (DBData.Zip_Postal1 != null ? DBData.Zip_Postal1 : " "),
                                 Address2 = DBData.Address2 == null ? "-" : "To: " + DBData.Address2 + ",<br>" + ((from DBC in dbobj.CityMasters
                                                                                                                   where DBC.CityId == DBData.City2
                                                                                                                   select DBC.CityName).First() != null ? (from DBC in dbobj.CityMasters
                                                                                                                                                           where DBC.CityId == DBData.City2
                                                                                                                                                           select DBC.CityName).First() : "") + ", " + ((from DBState in dbobj.StateMasters
                                                                                                                                                                                                         where DBState.StateId == DBData.State2
                                                                                                                                                                                                         select DBState.StateName).First() != null ? (from DBState in dbobj.StateMasters
                                                                                                                                                                                                                                                      where DBState.StateId == DBData.State2
                                                                                                                                                                                                                                                      select DBState.StateName).First() : " ") + ",<br>" + ((from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                                                                                                                                             where DBCounty.CountryId == DBData.Country2
                                                                                                                                                                                                                                                                                                             select DBCounty.CountryName).First() != null ? (from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                                                                                                                                                                                             where DBCounty.CountryId == DBData.Country2
                                                                                                                                                                                                                                                                                                                                                             select DBCounty.CountryName).First() : "") + " - " + (DBData.Zip_Postal1 != null ? DBData.Zip_Postal1 : " "),
                                 Website = DBData.Website == null ? "-" : DBData.Website,
                                 //Currency = ""
                                 Currency = DBData.CurrencyMaster.CurrencyCode + "-" + ("&#" + DBData.CurrencyMaster.CurrencySymbol)
                             };
                if (Client.Count() > 0)
                {
                    gridClient.DataSource = Client;
                    gridClient.DataBind();
                }
                else
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("ClientId");
                    dt.Columns.Add("ClientName");
                    dt.Columns.Add("Phone");
                    dt.Columns.Add("Address1");
                    dt.Columns.Add("Address2");
                    dt.Columns.Add("Website");
                    dt.Columns.Add("DepartmentId");
                    dt.Columns.Add("Department");
                    dt.Columns.Add("Currency");
                    gridClient.DataSource = dt;
                    dt.Rows.Add(dt.NewRow());
                    gridClient.DataBind();
                    int TotalCols = gridClient.Rows[0].Cells.Count;
                    gridClient.Rows[0].Cells.Clear();
                    gridClient.Rows[0].Cells.Add(new TableCell());
                    gridClient.Rows[0].Cells[0].ColumnSpan = TotalCols;
                    gridClient.Rows[0].Cells[0].Text = "No records to display";
                }
            }
        }
    }

    protected void SearchCurrency()
    {
        GridView gridCurrency = (GridView)ContentPlaceHolder1.FindControl("gridCurrency");
        if (gridCurrency != null)
        {
            var Currency = from DBData in dbobj.CurrencyMasters
                           where DBData.IsDeleted == false && (DBData.CurrencyName.Contains(txtMasterSearch.Text.Trim()) || DBData.CurrencyCode.Contains(txtMasterSearch.Text.Trim()))
                           orderby DBData.CurrencyName
                           select new
                           {
                               CurrencyId = DBData.CurrencyId,
                               CurrencyName = DBData.CurrencyName,
                               CurrencyCode = DBData.CurrencyCode,
                               CurrencySymbol = DBData.CurrencySymbol == "" ? "" : "&#" + DBData.CurrencySymbol
                           };
            if (Currency.Count() > 0)
            {
                gridCurrency.DataSource = Currency;
                gridCurrency.DataBind();
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("CurrencyId");
                dt.Columns.Add("CurrencyName");
                dt.Columns.Add("CurrencyCode");
                dt.Columns.Add("CurrencySymbol");
                gridCurrency.DataSource = dt;
                dt.Rows.Add(dt.NewRow());
                gridCurrency.DataBind();
                int TotalCols = gridCurrency.Rows[0].Cells.Count;
                gridCurrency.Rows[0].Cells.Clear();
                gridCurrency.Rows[0].Cells.Add(new TableCell());
                gridCurrency.Rows[0].Cells[0].ColumnSpan = TotalCols;
                gridCurrency.Rows[0].Cells[0].Text = "No records to display";
            }
        }
    }

    protected void SearchCountry()
    {
        GridView gridCountry = (GridView)ContentPlaceHolder1.FindControl("gridCountry");
        if (gridCountry != null)
        {
            var Country = from DBData in dbobj.CountryMasters
                          where DBData.CountryName.Contains(txtMasterSearch.Text.Trim())
                          orderby DBData.CountryName
                          select new
                          {
                              CountryId = DBData.CountryId,
                              CountryName = DBData.CountryName
                          };
            if (Country.Count() > 0)
            {
                gridCountry.DataSource = Country;
                gridCountry.DataBind();
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("CountryId");
                dt.Columns.Add("CountryName");
                gridCountry.DataSource = dt;
                dt.Rows.Add(dt.NewRow());
                gridCountry.DataBind();
                int TotalCols = gridCountry.Rows[0].Cells.Count;
                gridCountry.Rows[0].Cells.Clear();
                gridCountry.Rows[0].Cells.Add(new TableCell());
                gridCountry.Rows[0].Cells[0].ColumnSpan = TotalCols;
                gridCountry.Rows[0].Cells[0].Text = "No records to display";
            }
        }
    }

    protected void SearchState()
    {
        GridView gridState = (GridView)ContentPlaceHolder1.FindControl("gridState");
        if (gridState != null)
        {
            var State = from DBData in dbobj.StateMasters
                        where DBData.StateName.Contains(txtMasterSearch.Text.Trim()) || DBData.CountryMaster.CountryName.Contains(txtMasterSearch.Text.Trim())
                        orderby DBData.CountryMaster.CountryName, DBData.StateName
                        select new
                        {
                            StateId = DBData.StateId,
                            StateName = DBData.StateName,
                            CountryName = DBData.CountryMaster.CountryName,
                            CountryId = DBData.CountryId
                        };
            if (State.Count() > 0)
            {
                gridState.DataSource = State;
                gridState.DataBind();
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("StateId");
                dt.Columns.Add("StateName");
                dt.Columns.Add("CountryName");
                dt.Columns.Add("CountryId");
                gridState.DataSource = dt;
                dt.Rows.Add(dt.NewRow());
                gridState.DataBind();
                int TotalCols = gridState.Rows[0].Cells.Count;
                gridState.Rows[0].Cells.Clear();
                gridState.Rows[0].Cells.Add(new TableCell());
                gridState.Rows[0].Cells[0].ColumnSpan = TotalCols;
                gridState.Rows[0].Cells[0].Text = "No records to display";
            }
        }
    }

    protected void SearchCity()
    {
        GridView gridCity = (GridView)ContentPlaceHolder1.FindControl("gridCity");
        if (gridCity != null)
        {
            var City = from DBData in dbobj.CityMasters
                       where DBData.CityName.Contains(txtMasterSearch.Text.Trim()) || DBData.StateMaster.StateName.Contains(txtMasterSearch.Text.Trim()) || DBData.CountryMaster.CountryName.Contains(txtMasterSearch.Text.Trim())
                       orderby DBData.CountryMaster.CountryName, DBData.StateMaster.StateName, DBData.CityName
                       select new
                       {
                           CityId = DBData.CityId,
                           CityName = DBData.CityName,
                           StateId = DBData.StateId,
                           StateName = DBData.StateMaster.StateName,
                           CountryName = DBData.CountryMaster.CountryName,
                           CountryId = DBData.CountryId
                       };
            if (City.Count() > 0)
            {
                gridCity.DataSource = City;
                gridCity.DataBind();
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("CityId");
                dt.Columns.Add("CityName");
                dt.Columns.Add("StateId");
                dt.Columns.Add("StateName");
                dt.Columns.Add("CountryName");
                dt.Columns.Add("CountryId");
                gridCity.DataSource = dt;
                dt.Rows.Add(dt.NewRow());
                gridCity.DataBind();
                int TotalCols = gridCity.Rows[0].Cells.Count;
                gridCity.Rows[0].Cells.Clear();
                gridCity.Rows[0].Cells.Add(new TableCell());
                gridCity.Rows[0].Cells[0].ColumnSpan = TotalCols;
                gridCity.Rows[0].Cells[0].Text = "No records to display";
            }
        }
    }

    protected void SearchInvoice()
    {
        GridView gvInv = (GridView)ContentPlaceHolder1.FindControl("gvInv");
        DropDownList drpBusiness = (DropDownList)ContentPlaceHolder1.FindControl("drpBusiness");
        DropDownList drpInvoiceStatus = (DropDownList)ContentPlaceHolder1.FindControl("drpInvoiceStatus");
        DropDownList ddlClient = (DropDownList)ContentPlaceHolder1.FindControl("ddlClient");
        if (gvInv != null)
        {
            try
            {
                //where DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && DBData.InvoiceSeqNo == Seq
                //where ClientData.DepartmentId == Convert.ToInt64(drpBusiness.SelectedValue) && DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && DBData.InvoiceSeqNo == Seq
                int Seq = Convert.ToInt32(txtMasterSearch.Text.Trim());
                if (Global.UserType == "DIRECTOR" || Global.DepartmentName == "ACCT")
                {
                    if (drpBusiness.SelectedIndex == 0)
                    {
                        if (ddlClient.SelectedIndex == 0)
                        {
                            var InvoiceData = from DBData in dbobj.InvoiceMasters
                                              join ClientData in dbobj.ClientMasters
                                              on DBData.ClientId equals ClientData.ClientId
                                              join CurrencyData in dbobj.CurrencyMasters
                                              on ClientData.CurrencyId equals CurrencyData.CurrencyId
                                              where DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && DBData.InvoiceSeqNo == Seq
                                              select new
                                              {
                                                  InvoiceID = DBData.InvoiceId,
                                                  InvoiceNo = DBData.InvoiceNumber,
                                                  ClientName = ClientData.ClientName,
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
                                                  InvoiceStatus = DBData.InvoiceStatus
                                              };
                            if (InvoiceData.Count() > 0)
                            {
                                gvInv.DataSource = InvoiceData;
                                gvInv.DataBind();
                            }
                            else
                                InvBlankGrid();
                        }
                        else
                        {
                            var InvoiceData = from DBData in dbobj.InvoiceMasters
                                              join ClientData in dbobj.ClientMasters
                                              on DBData.ClientId equals ClientData.ClientId
                                              join CurrencyData in dbobj.CurrencyMasters
                                              on ClientData.CurrencyId equals CurrencyData.CurrencyId
                                              where DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && ClientData.ClientId == Convert.ToInt64(ddlClient.SelectedValue) && DBData.InvoiceSeqNo == Seq
                                              select new
                                              {
                                                  InvoiceID = DBData.InvoiceId,
                                                  InvoiceNo = DBData.InvoiceNumber,
                                                  ClientName = ClientData.ClientName,
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
                                                  InvoiceStatus = DBData.InvoiceStatus
                                              };
                            if (InvoiceData.Count() > 0)
                            {
                                gvInv.DataSource = InvoiceData;
                                gvInv.DataBind();
                            }
                            else
                                InvBlankGrid();
                        }
                    }
                    else
                    {
                        if (ddlClient.SelectedIndex == 0)
                        {
                            var InvoiceData = from DBData in dbobj.InvoiceMasters
                                              join ClientData in dbobj.ClientMasters
                                              on DBData.ClientId equals ClientData.ClientId
                                              join CurrencyData in dbobj.CurrencyMasters
                                              on ClientData.CurrencyId equals CurrencyData.CurrencyId
                                              where DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && ClientData.DepartmentId == Convert.ToInt64(drpBusiness.SelectedValue) && DBData.InvoiceSeqNo == Seq
                                              select new
                                              {
                                                  InvoiceID = DBData.InvoiceId,
                                                  InvoiceNo = DBData.InvoiceNumber,
                                                  ClientName = ClientData.ClientName,
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
                                                  InvoiceStatus = DBData.InvoiceStatus
                                              };
                            if (InvoiceData.Count() > 0)
                            {
                                gvInv.DataSource = InvoiceData;
                                gvInv.DataBind();
                            }
                            else
                                InvBlankGrid();
                        }
                        else
                        {
                            var InvoiceData = from DBData in dbobj.InvoiceMasters
                                              join ClientData in dbobj.ClientMasters
                                              on DBData.ClientId equals ClientData.ClientId
                                              join CurrencyData in dbobj.CurrencyMasters
                                              on ClientData.CurrencyId equals CurrencyData.CurrencyId
                                              where DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && ClientData.ClientId == Convert.ToInt64(ddlClient.SelectedValue) && DBData.InvoiceSeqNo == Seq
                                              && ClientData.DepartmentId == Convert.ToInt64(drpBusiness.SelectedValue)
                                              select new
                                              {
                                                  InvoiceID = DBData.InvoiceId,
                                                  InvoiceNo = DBData.InvoiceNumber,
                                                  ClientName = ClientData.ClientName,
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
                                                  InvoiceStatus = DBData.InvoiceStatus
                                              };
                            if (InvoiceData.Count() > 0)
                            {
                                gvInv.DataSource = InvoiceData;
                                gvInv.DataBind();
                            }
                            else
                                InvBlankGrid();
                        }
                    }
                }
                else
                {
                    if (ddlClient.SelectedIndex == 0)
                    {
                        var InvoiceData = from DBData in dbobj.InvoiceMasters
                                          join ClientData in dbobj.ClientMasters
                                          on DBData.ClientId equals ClientData.ClientId
                                          join CurrencyData in dbobj.CurrencyMasters
                                          on ClientData.CurrencyId equals CurrencyData.CurrencyId
                                          where ClientData.DepartmentId == Convert.ToInt64(drpBusiness.SelectedValue) && DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && DBData.InvoiceSeqNo == Seq
                                          select new
                                          {
                                              InvoiceID = DBData.InvoiceId,
                                              InvoiceNo = DBData.InvoiceNumber,
                                              ClientName = ClientData.ClientName,
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
                                              InvoiceStatus = DBData.InvoiceStatus
                                          };
                        if (InvoiceData.Count() > 0)
                        {
                            gvInv.DataSource = InvoiceData;
                            gvInv.DataBind();
                        }
                        else
                            InvBlankGrid();
                    }
                    else
                    {
                        var InvoiceData = from DBData in dbobj.InvoiceMasters
                                          join ClientData in dbobj.ClientMasters
                                          on DBData.ClientId equals ClientData.ClientId
                                          join CurrencyData in dbobj.CurrencyMasters
                                          on ClientData.CurrencyId equals CurrencyData.CurrencyId
                                          where ClientData.DepartmentId == Convert.ToInt64(drpBusiness.SelectedValue) && DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && DBData.InvoiceSeqNo == Seq
                                                && ClientData.ClientId == Convert.ToInt64(ddlClient.SelectedValue)
                                          select new
                                          {
                                              InvoiceID = DBData.InvoiceId,
                                              InvoiceNo = DBData.InvoiceNumber,
                                              ClientName = ClientData.ClientName,
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
                                              InvoiceStatus = DBData.InvoiceStatus
                                          };
                        if (InvoiceData.Count() > 0)
                        {
                            gvInv.DataSource = InvoiceData;
                            gvInv.DataBind();
                        }
                        else
                            InvBlankGrid();
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
                            var InvoiceData = from DBData in dbobj.InvoiceMasters
                                              join ClientData in dbobj.ClientMasters
                                              on DBData.ClientId equals ClientData.ClientId
                                              join CurrencyData in dbobj.CurrencyMasters
                                              on ClientData.CurrencyId equals CurrencyData.CurrencyId
                                              where DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && DBData.ClientMaster.ClientName.Contains(txtMasterSearch.Text.Trim())
                                              select new
                                              {
                                                  InvoiceID = DBData.InvoiceId,
                                                  InvoiceNo = DBData.InvoiceNumber,
                                                  ClientName = ClientData.ClientName,
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
                                                  InvoiceStatus = DBData.InvoiceStatus
                                              };
                            if (InvoiceData.Count() > 0)
                            {
                                gvInv.DataSource = InvoiceData;
                                gvInv.DataBind();
                            }
                            else
                                InvBlankGrid();
                        }
                        else
                        {
                            var InvoiceData = from DBData in dbobj.InvoiceMasters
                                              join ClientData in dbobj.ClientMasters
                                              on DBData.ClientId equals ClientData.ClientId
                                              join CurrencyData in dbobj.CurrencyMasters
                                              on ClientData.CurrencyId equals CurrencyData.CurrencyId
                                              where DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && ClientData.ClientId == Convert.ToInt64(ddlClient.SelectedValue) && DBData.ClientMaster.ClientName.Contains(txtMasterSearch.Text.Trim())
                                              select new
                                              {
                                                  InvoiceID = DBData.InvoiceId,
                                                  InvoiceNo = DBData.InvoiceNumber,
                                                  ClientName = ClientData.ClientName,
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
                                                  InvoiceStatus = DBData.InvoiceStatus
                                              };
                            if (InvoiceData.Count() > 0)
                            {
                                gvInv.DataSource = InvoiceData;
                                gvInv.DataBind();
                            }
                            else
                                InvBlankGrid();
                        }
                    }
                    else
                    {
                        if (ddlClient.SelectedIndex == 0)
                        {
                            var InvoiceData = from DBData in dbobj.InvoiceMasters
                                              join ClientData in dbobj.ClientMasters
                                              on DBData.ClientId equals ClientData.ClientId
                                              join CurrencyData in dbobj.CurrencyMasters
                                              on ClientData.CurrencyId equals CurrencyData.CurrencyId
                                              where DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && ClientData.DepartmentId == Convert.ToInt64(drpBusiness.SelectedValue) && DBData.ClientMaster.ClientName.Contains(txtMasterSearch.Text.Trim())
                                              select new
                                              {
                                                  InvoiceID = DBData.InvoiceId,
                                                  InvoiceNo = DBData.InvoiceNumber,
                                                  ClientName = ClientData.ClientName,
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
                                                  InvoiceStatus = DBData.InvoiceStatus
                                              };
                            if (InvoiceData.Count() > 0)
                            {
                                gvInv.DataSource = InvoiceData;
                                gvInv.DataBind();
                            }
                            else
                                InvBlankGrid();
                        }
                        else
                        {
                            var InvoiceData = from DBData in dbobj.InvoiceMasters
                                              join ClientData in dbobj.ClientMasters
                                              on DBData.ClientId equals ClientData.ClientId
                                              join CurrencyData in dbobj.CurrencyMasters
                                              on ClientData.CurrencyId equals CurrencyData.CurrencyId
                                              where DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && ClientData.ClientId == Convert.ToInt64(ddlClient.SelectedValue) && DBData.ClientMaster.ClientName.Contains(txtMasterSearch.Text.Trim())
                                              && ClientData.DepartmentId == Convert.ToInt64(drpBusiness.SelectedValue)
                                              select new
                                              {
                                                  InvoiceID = DBData.InvoiceId,
                                                  InvoiceNo = DBData.InvoiceNumber,
                                                  ClientName = ClientData.ClientName,
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
                                                  InvoiceStatus = DBData.InvoiceStatus
                                              };
                            if (InvoiceData.Count() > 0)
                            {
                                gvInv.DataSource = InvoiceData;
                                gvInv.DataBind();
                            }
                            else
                                InvBlankGrid();
                        }
                    }
                }
                else
                {
                    if (ddlClient.SelectedIndex == 0)
                    {
                        var InvoiceData = from DBData in dbobj.InvoiceMasters
                                          join ClientData in dbobj.ClientMasters
                                          on DBData.ClientId equals ClientData.ClientId
                                          join CurrencyData in dbobj.CurrencyMasters
                                          on ClientData.CurrencyId equals CurrencyData.CurrencyId
                                          where ClientData.DepartmentId == Convert.ToInt64(drpBusiness.SelectedValue) && DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && DBData.ClientMaster.ClientName.Contains(txtMasterSearch.Text.Trim())
                                          select new
                                          {
                                              InvoiceID = DBData.InvoiceId,
                                              InvoiceNo = DBData.InvoiceNumber,
                                              ClientName = ClientData.ClientName,
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
                                              InvoiceStatus = DBData.InvoiceStatus
                                          };
                        if (InvoiceData.Count() > 0)
                        {
                            gvInv.DataSource = InvoiceData;
                            gvInv.DataBind();
                        }
                        else
                            InvBlankGrid();
                    }
                    else
                    {
                        var InvoiceData = from DBData in dbobj.InvoiceMasters
                                          join ClientData in dbobj.ClientMasters
                                          on DBData.ClientId equals ClientData.ClientId
                                          join CurrencyData in dbobj.CurrencyMasters
                                          on ClientData.CurrencyId equals CurrencyData.CurrencyId
                                          where ClientData.DepartmentId == Convert.ToInt64(drpBusiness.SelectedValue) && DBData.InvoiceStatus == drpInvoiceStatus.SelectedValue && DBData.IsDeleted == false && DBData.ClientMaster.ClientName.Contains(txtMasterSearch.Text.Trim())
                                                && ClientData.ClientId == Convert.ToInt64(ddlClient.SelectedValue)
                                          select new
                                          {
                                              InvoiceID = DBData.InvoiceId,
                                              InvoiceNo = DBData.InvoiceNumber,
                                              ClientName = ClientData.ClientName,
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
                                              InvoiceStatus = DBData.InvoiceStatus
                                          };
                        if (InvoiceData.Count() > 0)
                        {
                            gvInv.DataSource = InvoiceData;
                            gvInv.DataBind();
                        }
                        else
                            InvBlankGrid();
                    }
                }
            }
        }
    }

    private void InvBlankGrid()
    {
        GridView gvInv = (GridView)ContentPlaceHolder1.FindControl("gvInv");
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
    }

    protected void btnMasterSearch_Click(object sender, EventArgs e)
    {
        switch (this.ContentPlaceHolder1.Page.ToString())
        {
            case "ASP.city_aspx":
                //SearchCity();
                callChildPageMethod("FillGrid", null);
                callChildPageMethod("FillState", null);
                break;
            case "ASP.client_aspx":
                //SearchClient();
                callChildPageMethod("FillGrid", null);
                break;
            case "ASP.clientcontact_aspx":
                break;
            case "ASP.country_aspx":
                //SearchCountry();
                callChildPageMethod("FillGrid", null);
                break;
            case "ASP.currency_aspx":
                //SearchCurrency();
                callChildPageMethod("FillGrid", null);
                break;
            case "ASP.department_aspx":
                //SearchDepartment();
                callChildPageMethod("FillGrid", null);
                callChildPageMethod("FillUser", null);
                break;
            case "ASP.home_aspx":
                break;
            case "ASP.invoice_aspx":
                break;
            case "ASP.price_aspx":
                break;
            case "ASP.priceprocess_aspx":
                break;
            case "ASP.pricetype_aspx":
                break;
            case "ASP.state_aspx":
                //SearchState();
                callChildPageMethod("FillGrid", null);
                break;
            case "ASP.user_aspx":
                //SearchUser();
                callChildPageMethod("FillGrid", null);
                callChildPageMethod("FillDepartment", null);
                break;
            case "ASP.userprofile_aspx":
                break;
            case "ASP.viewinvoices_aspx":
                //SearchInvoice();
                callChildPageMethod("FillGrid", null);
                break;
            default:
                break;
        }
    }

    protected void lnkFooterInvoice_Click(object sender, EventArgs e)
    {
        switch (Global.UserType)
        {
            case "DIRECTOR":
                Response.Redirect("ViewInvoice");
                break;
            case "BUSINESS ASSOCIATES":
                Response.Redirect("ViewInvoice");
                break;
            default:
                Response.Redirect("NewInvoice");
                break;
        }
    }

    private object callChildPageMethod(string methodName, params object[] parameters)
    {
        Type objType = this.Page.GetType();
        System.Reflection.MethodInfo mi = objType.GetMethod(methodName);
        if (mi == null) return null;
        return mi.Invoke(this.Page, parameters);
    }


    protected void lnkMonthlySalesNew_Click(object sender, EventArgs e)
    {
        liManageConsultant.Attributes.Add("class", "inactive");
        liClientMenu.Attributes.Add("class", "inactive");
        liDepartmentMenu.Attributes.Add("class", "inactive");
        liHomeMenu.Attributes.Add("class", "inactive");
        liInvoiceMenu.Attributes.Add("class", "active");
        liPriceMenu.Attributes.Add("class", "inactive");
        liUserMenu.Attributes.Add("class", "inactive");
    }



    protected void lnkManageConsultant_Click(object sender, EventArgs e)
    {
        liManageConsultant.Attributes.Add("class", "active");
        liClientMenu.Attributes.Add("class", "inactive");
        liDepartmentMenu.Attributes.Add("class", "inactive");
        liHomeMenu.Attributes.Add("class", "inactive");
        liInvoiceMenu.Attributes.Add("class", "inactive");
        liPriceMenu.Attributes.Add("class", "inactive");
        liUserMenu.Attributes.Add("class", "inactive");
    }

    protected void lnkProformaInvoice_Click(object sender, EventArgs e)
    {
        liManageConsultant.Attributes.Add("class", "inactive");
        liClientMenu.Attributes.Add("class", "inactive");
        liDepartmentMenu.Attributes.Add("class", "inactive");
        liHomeMenu.Attributes.Add("class", "inactive");
        liInvoiceMenu.Attributes.Add("class", "active");
        liPriceMenu.Attributes.Add("class", "inactive");
        liUserMenu.Attributes.Add("class", "inactive");
    }

    protected void lnkViewProformaInvoice_Click(object sender, EventArgs e)
    {
        liManageConsultant.Attributes.Add("class", "inactive");
        liClientMenu.Attributes.Add("class", "inactive");
        liDepartmentMenu.Attributes.Add("class", "inactive");
        liHomeMenu.Attributes.Add("class", "inactive");
        liInvoiceMenu.Attributes.Add("class", "active");
        liPriceMenu.Attributes.Add("class", "inactive");
        liUserMenu.Attributes.Add("class", "inactive");
    }

    protected void lnkInvPayment_Click(object sender, EventArgs e)
    {
        liManageConsultant.Attributes.Add("class", "active");
        liClientMenu.Attributes.Add("class", "inactive");
        liDepartmentMenu.Attributes.Add("class", "inactive");
        liHomeMenu.Attributes.Add("class", "inactive");
        liInvoiceMenu.Attributes.Add("class", "inactive");
        liPriceMenu.Attributes.Add("class", "inactive");
        liUserMenu.Attributes.Add("class", "inactive");
    }

    protected void lnkCommInv_Click(object sender, EventArgs e)
    {
        liManageConsultant.Attributes.Add("class", "inactive");
        liClientMenu.Attributes.Add("class", "inactive");
        liDepartmentMenu.Attributes.Add("class", "inactive");
        liHomeMenu.Attributes.Add("class", "inactive");
        liInvoiceMenu.Attributes.Add("class", "active");
        liPriceMenu.Attributes.Add("class", "inactive");
        liUserMenu.Attributes.Add("class", "inactive");
    }
}
