using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CompanyContact : System.Web.UI.Page
{
    MITInvoiceDataContext dbobj = new MITInvoiceDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Global.UserType == "BUSINESS UNIT MANAGER" || Global.UserType == "DIRECTOR")
        {
            if (!IsPostBack)
            {

                string qs = Request.QueryString["m"].Replace(" ", "+");
                if (Global.Decrypt(qs) == "View")
                {
                    FillGrid();
                   // gridClientContact.FooterRow.Visible = false;
                   // gridClientContact.Columns[7].Visible = false;
                }
                else
                {

                   FillGrid();
                   FillCompany();
                }
            }
        }
        else
        {
            Response.Redirect("Authorize");
        }
    }

    protected void FillGrid()
    {

        string qsCID = Request.QueryString["CID"].Replace(" ", "+");
        var CompanyContact = from DBData in dbobj.Individual_ContactMasters
                            where DBData.IsDelete == false && DBData.CompanyID == Convert.ToInt64(Global.Decrypt(qsCID))
                            orderby DBData.ContactPerson
                            select new
                            {
                                CompanyContactId = DBData.IndividualID,
                                CompanyName = DBData.CompanyMaster.CompanyName,
                                ContactPersonName = DBData.ContactPerson,
                                ContactNumber = DBData.ContactNumber,
                                DepartmentId = DBData.CompanyMaster.DepartmentMaster.DepartmentId,
                                Email = DBData.EmailAddress,
                                Description = DBData.Description

                            };
        if (CompanyContact.Count() > 0)
        {
            gridCompanyContact.DataSource = CompanyContact;
            gridCompanyContact.DataBind();
        }
        else
        {
            BlankGrid();

        }

    }

    protected void BlankGrid()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("CompanyContactId");
        dt.Columns.Add("CompanyName");
        dt.Columns.Add("ContactPersonName");
        dt.Columns.Add("ContactNumber");
        dt.Columns.Add("DepartmentId");
        dt.Columns.Add("Email");
        dt.Columns.Add("Description");
        gridCompanyContact.DataSource = dt;
        dt.Rows.Add(dt.NewRow());
        gridCompanyContact.DataBind();
        int TotalCols = gridCompanyContact.Rows[0].Cells.Count;
        gridCompanyContact.Rows[0].Cells.Clear();
        gridCompanyContact.Rows[0].Cells.Add(new TableCell());
        gridCompanyContact.Rows[0].Cells[0].ColumnSpan = TotalCols;
        gridCompanyContact.Rows[0].Cells[0].Text = "No records to display";
    }

    protected void FillCompany()
    {

        DropDownList ddlCompanyName = (DropDownList)gridCompanyContact.FooterRow.FindControl("ddlCompanyName");
        // 18-Aug-2020 by Jignesh
        // added : && (DBData.C_M1 == Global.UserM1 || DBData.C_M2 == Global.UserM2)
        var Client = from DBData in dbobj.CompanyMasters
                     where DBData.IsDelete == false && DBData.DepartmentMaster.DepartmentId == Convert.ToInt64(Global.Department)
                     orderby DBData.CompanyName
                     select new
                     {
                         CompanyID = DBData.CompanyID,
                         CompanyName = DBData.CompanyName
                     };
        if (Client.Count() > 0)
        {
            ddlCompanyName.DataSource = Client;
            ddlCompanyName.DataTextField = "CompanyName";
            ddlCompanyName.DataValueField = "CompanyID";
            ddlCompanyName.DataBind();
        }
        string qsCID = Request.QueryString["CID"].Replace(" ", "+");
        ddlCompanyName.Items.FindByValue(Global.Decrypt(qsCID)).Selected = true;
        ddlCompanyName.Enabled = false;
    }
    
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("ManageConsultant");
    }

    protected void gridCompanyContact_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Page page1 = HttpContext.Current.Handler as Page;
        DropDownList ddlCompanyName = (DropDownList)gridCompanyContact.FooterRow.FindControl("ddlCompanyName");
        TextBox txtContactPersonName = (TextBox)gridCompanyContact.FooterRow.FindControl("txtContactPersonName");
        TextBox txtContactNumber = (TextBox)gridCompanyContact.FooterRow.FindControl("txtContactNumber");
        TextBox txtEmail = (TextBox)gridCompanyContact.FooterRow.FindControl("txtEmail");
        TextBox txtDescription = (TextBox)gridCompanyContact.FooterRow.FindControl("txtDescription"); 

        switch (e.CommandName)
        {
            case "Insert":
                Individual_ContactMaster icm = new Individual_ContactMaster();
                icm.CompanyID= Convert.ToInt64(ddlCompanyName.SelectedValue);
                icm.ContactPerson= txtContactPersonName.Text.Trim();
                icm.ContactNumber = txtContactNumber.Text.Trim();
                icm.EmailAddress = txtEmail.Text.Trim();
                icm.Description= txtDescription.Text.Trim();
                icm.IsDelete = false;
                icm.CreatedBy = Convert.ToInt64(Global.UserId);
                icm.CreatedDate = DateTime.Now;
                icm.ModifyBy= Convert.ToInt64(Global.UserId);
                icm.ModifyDate= DateTime.Now;
                dbobj.Individual_ContactMasters.InsertOnSubmit(icm);
                dbobj.SubmitChanges();
                dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('Save Successfully')", true);
                FillGrid();
                FillCompany();
                break;
            case "Delete":
                GridViewRow gr = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                Label lblIndividualContactId = (Label)gr.FindControl("lblIndividualContactId");
                var DelCompanyContact = from DelCC in dbobj.Individual_ContactMasters
                                       where DelCC.IndividualID == Convert.ToInt64(lblIndividualContactId.Text)
                                       select DelCC;
                if (DelCompanyContact.Count() > 0)
                {
                    var CompanyContact = DelCompanyContact.Single();
                    CompanyContact.IsDelete = true;
                    dbobj.SubmitChanges();
                    dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                    //ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('Delete Successfully')", true);
                    FillGrid();
                    FillCompany();
                }
                break;
        }
    }

    protected void gridCompanyContact_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    protected void gridCompanyContact_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gridCompanyContact.EditIndex = e.NewEditIndex;
        gridCompanyContact.ShowFooter = false;
        gridCompanyContact.FooterRow.Visible = false;
        gridCompanyContact.PagerSettings.Visible = false;
        Label lblCompanyName = (Label)gridCompanyContact.Rows[e.NewEditIndex].FindControl("lblCompanyName");
        FillGrid();
        DropDownList ddlEdtCompanyName = (DropDownList)gridCompanyContact.Rows[e.NewEditIndex].FindControl("ddlEdtCompanyName");
        var Client = from DBData in dbobj.CompanyMasters
                     where DBData.IsDelete == false && DBData.DepartmentID == Convert.ToInt64(Global.Department)
                     orderby DBData.CompanyName
                     select new
                     {
                         CompanyID = DBData.CompanyID,
                         CompanyName = DBData.CompanyName
                     };
        if (Client.Count() > 0)
        {
            ddlEdtCompanyName.DataSource = Client;
            ddlEdtCompanyName.DataTextField = "CompanyName";
            ddlEdtCompanyName.DataValueField = "CompanyID";
            ddlEdtCompanyName.DataBind();
        }
        ddlEdtCompanyName.Items.FindByText(lblCompanyName.Text).Selected = true;
    }

    protected void gridCompanyContact_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gridCompanyContact.EditIndex = -1;
        gridCompanyContact.ShowFooter = true;
        gridCompanyContact.FooterRow.Visible = true;
        gridCompanyContact.PagerSettings.Visible = true;
        FillGrid();
        FillCompany();
    }

    protected void gridCompanyContact_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridCompanyContact.PageIndex = e.NewPageIndex;
        FillGrid();
        FillCompany();
    }

    protected void gridCompanyContact_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        Page page1 = HttpContext.Current.Handler as Page;
        Label lblIndividualContactId = (Label)gridCompanyContact.Rows[e.RowIndex].FindControl("lblIndividualContactId");
        DropDownList ddlEdtCompanyName = (DropDownList)gridCompanyContact.Rows[e.RowIndex].FindControl("ddlEdtCompanyName");
        TextBox txtEdtContactPersonName = (TextBox)gridCompanyContact.Rows[e.RowIndex].FindControl("txtEdtContactPersonName");
        TextBox txtEdtContactNumber = (TextBox)gridCompanyContact.Rows[e.RowIndex].FindControl("txtEdtContactNumber");
        TextBox txtEdtEmail = (TextBox)gridCompanyContact.Rows[e.RowIndex].FindControl("txtEdtEmail");
        TextBox txtEdtDescription = (TextBox)gridCompanyContact.Rows[e.RowIndex].FindControl("txtEdtDescription");


        var UpdateData = from DBData in dbobj.Individual_ContactMasters
                         where DBData.IndividualID == Convert.ToInt64(lblIndividualContactId.Text)
                         select DBData;
        if (UpdateData.Count() > 0)
        {
            var SingleUpdate = UpdateData.Single();
            SingleUpdate.CompanyID = Convert.ToInt64(ddlEdtCompanyName.SelectedValue);
            SingleUpdate.ContactPerson = txtEdtContactPersonName.Text.Trim();
            SingleUpdate.ContactNumber = txtEdtContactNumber.Text.Trim();
            SingleUpdate.EmailAddress = txtEdtEmail.Text.Trim();
            SingleUpdate.Description = txtEdtDescription.Text.Trim();
            SingleUpdate.ModifyBy = Convert.ToInt64(Global.UserId);
            SingleUpdate.ModifyDate = DateTime.Now;
            dbobj.SubmitChanges();
            dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
            gridCompanyContact.EditIndex = -1;
            gridCompanyContact.ShowFooter = true;
            gridCompanyContact.FooterRow.Visible = true;
            gridCompanyContact.PagerSettings.Visible = true;
            FillGrid();
            FillCompany();
        }
    }

    protected void gridCompanyContact_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblDepartmentId = (Label)e.Row.FindControl("lblDepartmentId");
            Label lbl1 = (Label)e.Row.FindControl("lbl1");
            ImageButton imgEdit = (ImageButton)e.Row.FindControl("imgEdit");
            ImageButton imgDelete = (ImageButton)e.Row.FindControl("imgDelete");

            var Dept = from DB in dbobj.DepartmentMasters
                       where DB.IsActive == true && DB.UserId == Convert.ToInt64(Global.UserId)
                       select DB.DepartmentId;
            if (lblDepartmentId.Text != "")
            {
                if (!Dept.Contains(Convert.ToInt64(lblDepartmentId.Text)))
                {
                    //if (lblDepartmentId.Text != Global.Department)
                    //{
                    imgEdit.Visible = false;
                    imgDelete.Visible = false;
                    lbl1.Visible = false;
                    //}
                }
            }
        }
    }
}