using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class ClientContact : System.Web.UI.Page
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
                    gridClientContact.FooterRow.Visible = false;
                    gridClientContact.Columns[7].Visible = false;
                }
                else
                {

                    FillGrid();
                    FillClient();
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
        var ClientContact = from DBData in dbobj.ClientContactMasters
                            where DBData.IsDeleted == false && DBData.ClientId == Convert.ToInt64(Global.Decrypt(qsCID))
                            orderby DBData.Name
                            select new
                            {
                                ClientContactId = DBData.ClientContactId,
                                ClientName = DBData.ClientMaster.ClientName,
                                ContactPersonName = DBData.Name,
                                ContactNumber = DBData.ContactNumber,
                                DepartmentId = DBData.ClientMaster.DepartmentId,
                                Email = DBData.EmailAddress
                            };
        if (ClientContact.Count() > 0)
        {
            gridClientContact.DataSource = ClientContact;
            gridClientContact.DataBind();
        }
        else
        {
            BlankGrid();

        }

    }

    protected void BlankGrid()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ClientContactId");
        dt.Columns.Add("ClientName");
        dt.Columns.Add("ContactPersonName");
        dt.Columns.Add("ContactNumber");
        dt.Columns.Add("DepartmentId");
        dt.Columns.Add("Email");
        gridClientContact.DataSource = dt;
        dt.Rows.Add(dt.NewRow());
        gridClientContact.DataBind();
        int TotalCols = gridClientContact.Rows[0].Cells.Count;
        gridClientContact.Rows[0].Cells.Clear();
        gridClientContact.Rows[0].Cells.Add(new TableCell());
        gridClientContact.Rows[0].Cells[0].ColumnSpan = TotalCols;
        gridClientContact.Rows[0].Cells[0].Text = "No records to display";
    }

    protected void FillClient()
    {

        DropDownList ddlClientName = (DropDownList)gridClientContact.FooterRow.FindControl("ddlClientName");
        // 18-Aug-2020 by Jignesh
        // added : && (DBData.C_M1 == Global.UserM1 || DBData.C_M2 == Global.UserM2)
        var Client = from DBData in dbobj.ClientMasters
                     //where DBData.IsActive == true && DBData.DepartmentMaster.DepartmentId == Convert.ToInt64(Global.Department) && (DBData.C_M1 == Global.UserM1 || DBData.C_M2 == Global.UserM2)
                     where DBData.IsActive == true && DBData.DepartmentMaster.UserId== Convert.ToInt64(Global.UserId) && (DBData.C_M1 == Global.UserM1 || DBData.C_M2 == Global.UserM2)
                     orderby DBData.ClientName
                     select new
                     {
                         ClientId = DBData.ClientId,
                         ClientName = DBData.ClientName
                     };
        if (Client.Count() > 0)
        {
            ddlClientName.DataSource = Client;
            ddlClientName.DataTextField = "ClientName";
            ddlClientName.DataValueField = "ClientId";
            ddlClientName.DataBind();
        }
        string qsCID = Request.QueryString["CID"].Replace(" ", "+");
        ddlClientName.Items.FindByValue(Global.Decrypt(qsCID)).Selected = true;
        ddlClientName.Enabled = false;
    }


    protected void FillClient_Existing()
    {
        DropDownList ddlClientName = (DropDownList)gridClientContact.FooterRow.FindControl("ddlClientName");
        var Client = from DBData in dbobj.ClientMasters
                     where DBData.IsActive == true && DBData.DepartmentMaster.UserId == Convert.ToInt64(Global.UserId)
                     orderby DBData.ClientName
                     select new
                     {
                         ClientId = DBData.ClientId,
                         ClientName = DBData.ClientName
                     };
        if (Client.Count() > 0)
        {
            ddlClientName.DataSource = Client;
            ddlClientName.DataTextField = "ClientName";
            ddlClientName.DataValueField = "ClientId";
            ddlClientName.DataBind();
        }
        string qsCID = Request.QueryString["CID"].Replace(" ", "+");
        ddlClientName.Items.FindByValue(Global.Decrypt(qsCID)).Selected = true;
        ddlClientName.Enabled = false;
    }

    protected void gridClientContact_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Page page1 = HttpContext.Current.Handler as Page;
        DropDownList ddlClientName = (DropDownList)gridClientContact.FooterRow.FindControl("ddlClientName");
        TextBox txtContactPersonName = (TextBox)gridClientContact.FooterRow.FindControl("txtContactPersonName");
        TextBox txtContactNumber = (TextBox)gridClientContact.FooterRow.FindControl("txtContactNumber");
        TextBox txtEmail = (TextBox)gridClientContact.FooterRow.FindControl("txtEmail");

        switch (e.CommandName)
        {
            case "Insert":
                ClientContactMaster ccm = new ClientContactMaster();
                ccm.ClientId = Convert.ToInt64(ddlClientName.SelectedValue);
                ccm.Name = txtContactPersonName.Text.Trim();
                ccm.ContactNumber = txtContactNumber.Text.Trim();
                ccm.EmailAddress = txtEmail.Text.Trim();
                ccm.IsDeleted = false;
                ccm.CratedBy = Convert.ToInt64(Global.UserId);
                ccm.CreatedDate = DateTime.Now;
                dbobj.ClientContactMasters.InsertOnSubmit(ccm);
                dbobj.SubmitChanges();
                dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('Save Successfully')", true);
                FillGrid();
                FillClient();
                break;
            case "Delete":
                GridViewRow gr = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                Label lblClientContactId = (Label)gr.FindControl("lblClientContactId");
                var DelClientContact = from DelCC in dbobj.ClientContactMasters
                                       where DelCC.ClientContactId == Convert.ToInt64(lblClientContactId.Text)
                                       select DelCC;
                if (DelClientContact.Count() > 0)
                {
                    var ClientContact = DelClientContact.Single();
                    ClientContact.IsDeleted = true;
                    dbobj.SubmitChanges();
                    dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                    //ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('Delete Successfully')", true);
                    FillGrid();
                    FillClient();
                }
                break;
        }
    }

    protected void gridClientContact_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }

    protected void gridClientContact_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gridClientContact.EditIndex = e.NewEditIndex;
        gridClientContact.ShowFooter = false;
        gridClientContact.FooterRow.Visible = false;
        gridClientContact.PagerSettings.Visible = false;
        Label lblClientName = (Label)gridClientContact.Rows[e.NewEditIndex].FindControl("lblClientName");
        FillGrid();
        DropDownList ddlEdtClientName = (DropDownList)gridClientContact.Rows[e.NewEditIndex].FindControl("ddlEdtClientName");
        var Client = from DBData in dbobj.ClientMasters
                     where DBData.IsActive == true && DBData.DepartmentId == Convert.ToInt64(Global.Department)
                     orderby DBData.ClientName
                     select new
                     {
                         ClientId = DBData.ClientId,
                         ClientName = DBData.ClientName
                     };
        if (Client.Count() > 0)
        {
            ddlEdtClientName.DataSource = Client;
            ddlEdtClientName.DataTextField = "ClientName";
            ddlEdtClientName.DataValueField = "ClientId";
            ddlEdtClientName.DataBind();
        }
        ddlEdtClientName.Items.FindByText(lblClientName.Text).Selected = true;
    }

    protected void gridClientContact_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gridClientContact.EditIndex = -1;
        gridClientContact.ShowFooter = true;
        gridClientContact.FooterRow.Visible = true;
        gridClientContact.PagerSettings.Visible = true;
        FillGrid();
        FillClient();
    }

    protected void gridClientContact_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        Page page1 = HttpContext.Current.Handler as Page;
        Label lblClientContactId = (Label)gridClientContact.Rows[e.RowIndex].FindControl("lblClientContactId");
        DropDownList ddlEdtClientName = (DropDownList)gridClientContact.Rows[e.RowIndex].FindControl("ddlEdtClientName");
        TextBox txtEdtContactPersonName = (TextBox)gridClientContact.Rows[e.RowIndex].FindControl("txtEdtContactPersonName");
        TextBox txtEdtContactNumber = (TextBox)gridClientContact.Rows[e.RowIndex].FindControl("txtEdtContactNumber");
        TextBox txtEdtEmail = (TextBox)gridClientContact.Rows[e.RowIndex].FindControl("txtEdtEmail");


        var UpdateData = from DBData in dbobj.ClientContactMasters
                         where DBData.ClientContactId == Convert.ToInt64(lblClientContactId.Text)
                         select DBData;
        if (UpdateData.Count() > 0)
        {
            var SingleUpdate = UpdateData.Single();
            SingleUpdate.ClientId = Convert.ToInt64(ddlEdtClientName.SelectedValue);
            SingleUpdate.Name = txtEdtContactPersonName.Text.Trim();
            SingleUpdate.ContactNumber = txtEdtContactNumber.Text.Trim();
            SingleUpdate.EmailAddress = txtEdtEmail.Text.Trim();
            SingleUpdate.ModifyBy = Convert.ToInt64(Global.UserId);
            SingleUpdate.ModifyDtae = DateTime.Now;
            dbobj.SubmitChanges();
            dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
            gridClientContact.EditIndex = -1;
            gridClientContact.ShowFooter = true;
            gridClientContact.FooterRow.Visible = true;
            gridClientContact.PagerSettings.Visible = true;
            FillGrid();
            FillClient();
        }
    }

    protected void gridClientContact_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridClientContact.PageIndex = e.NewPageIndex;
        FillGrid();
        FillClient();
    }

    protected void gridClientContact_RowDataBound(object sender, GridViewRowEventArgs e)
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

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("ManageClient");
    }
}
