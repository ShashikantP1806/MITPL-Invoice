using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;

public partial class User : System.Web.UI.Page
{
    MITInvoiceDataContext dbobj = new MITInvoiceDataContext();
    UnicodeEncoding ByteConverter = new UnicodeEncoding();
    RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Global.UserType == "ADMINISTRATOR")
        {
            if (!IsPostBack)
            {
                FillGrid();
                FillDepartment();
            }
        }
        else
        {
            Response.Redirect("Authorize");
        }
    }

   

    public void FillGrid()
    {
        TextBox txtMasterSearch = (TextBox)this.Master.FindControl("txtMasterSearch");
        if (txtMasterSearch.Text == "")
        {
            var User = from DBData in dbobj.UserMasters
                       orderby DBData.EmpId
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
                           ActiveInactive = DBData.IsActive == true ? "Active" : "Inactive",
                           U_M1 = DBData.U_M1,
                           U_M2 = DBData.U_M2,
                           UserSentEmail = DBData.IsUserSentEmail,
                           ProformaApproveAccess = DBData.ProformaApproveAccess
                       };
            if (User.Count() > 0)
            {
                gridUser.DataSource = User;
                gridUser.DataBind();
            }
            else
            {
                BlankGrid();
                //DataTable dt = new DataTable();
                //dt.Columns.Add("UserId");
                //dt.Columns.Add("EmpId");
                //dt.Columns.Add("FirstName");
                //dt.Columns.Add("MiddleName");
                //dt.Columns.Add("LastName");
                //dt.Columns.Add("UserName");
                //dt.Columns.Add("Department");
                //dt.Columns.Add("Email");
                //dt.Columns.Add("UserType");
                //dt.Columns.Add("ActiveInactive");
                //dt.Columns.Add("U_M1"); //// Added by Jignesh on 20-Jul-2020
                //dt.Columns.Add("U_M2"); //// Added by Jignesh on 20-Jul-2020
                //gridUser.DataSource = dt;
                //dt.Rows.Add(dt.NewRow());
                //gridUser.DataBind();
                //int TotalCols = gridUser.Rows[0].Cells.Count;
                //gridUser.Rows[0].Cells.Clear();
                //gridUser.Rows[0].Cells.Add(new TableCell());
                //gridUser.Rows[0].Cells[0].ColumnSpan = TotalCols;
                //gridUser.Rows[0].Cells[0].Text = "No records to display";
            }
        }
        else
        {
            try
            {
                int Empid = Convert.ToInt32(txtMasterSearch.Text.Trim());
                var UserData = from DBData in dbobj.UserMasters
                               orderby DBData.EmpId
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
                                   ActiveInactive = DBData.IsActive == true ? "Active" : "Inactive",
                                   U_M1 = DBData.U_M1,
                                   U_M2 = DBData.U_M2
                               };
                if (UserData.Count() > 0)
                {
                    gridUser.DataSource = UserData;
                    gridUser.DataBind();
                }
                else
                {
                    BlankGrid();
                }
            }
            catch
            {
                var UserData = (from DBData in dbobj.UserMasters
                                orderby DBData.EmpId
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
                                    ActiveInactive = DBData.IsActive == true ? "Active" : "Inactive",
                                    U_M1 = DBData.U_M1,
                                    U_M2 = DBData.U_M2
                                });
                if (UserData.Count() > 0)
                {
                    gridUser.DataSource = UserData;
                    gridUser.DataBind();
                }
                else
                {
                    BlankGrid();
                }
            }
        }
    }

    protected void BlankGrid()
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
        dt.Columns.Add("U_M1"); //// Added by Jignesh on 20-Jul-2020
        dt.Columns.Add("U_M2"); //// Added by Jignesh on 20-Jul-2020
        dt.Columns.Add("UserSentEmail"); //// Added by Jignesh on 20-Jul-2020
        dt.Columns.Add("ProformaApproveAccess"); //// Added by Jignesh on 01-Sep-2022
        gridUser.DataSource = dt;
        dt.Rows.Add(dt.NewRow());
        gridUser.DataBind();
        int TotalCols = gridUser.Rows[0].Cells.Count;
        gridUser.Rows[0].Cells.Clear();
        gridUser.Rows[0].Cells.Add(new TableCell());
        gridUser.Rows[0].Cells[0].ColumnSpan = TotalCols;
        gridUser.Rows[0].Cells[0].Text = "No records to display";
    }

    public void FillDepartment()
    {
        DropDownList ddlDepartment = (DropDownList)gridUser.FooterRow.FindControl("ddlDepartment");
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
            ddlDepartment.DataSource = Department;
            ddlDepartment.DataTextField = "DepartmentName";
            ddlDepartment.DataValueField = "DepartmentId";
            ddlDepartment.DataBind();
        }
        ddlDepartment.Items.Insert(0, "-- Select --");
    }

    protected void gridUser_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Page page1 = HttpContext.Current.Handler as Page;
        switch (e.CommandName)
        {
            case "Insert":
                TextBox txtEmpId = (TextBox)gridUser.FooterRow.FindControl("txtEmpId");
                ViewState["EmpIID"] = txtEmpId.Text.Trim();
                TextBox txtFirstName = (TextBox)gridUser.FooterRow.FindControl("txtFirstName");
                ViewState["FName"] = Global.TitleCase(txtFirstName.Text.Trim());
                TextBox txtMiddleName = (TextBox)gridUser.FooterRow.FindControl("txtMiddleName");
                TextBox txtLastName = (TextBox)gridUser.FooterRow.FindControl("txtLastName");
                ViewState["LName"] = Global.TitleCase(txtLastName.Text.Trim());
                TextBox txtUserName = (TextBox)gridUser.FooterRow.FindControl("txtUserName");
                DropDownList ddlDepartment = (DropDownList)gridUser.FooterRow.FindControl("ddlDepartment");
                TextBox txtEmail = (TextBox)gridUser.FooterRow.FindControl("txtEmail");
                DropDownList ddlUserType = (DropDownList)gridUser.FooterRow.FindControl("ddlUserType");
                CheckBox chkM1_IsActive = (CheckBox)gridUser.FooterRow.FindControl("chkM1_IsActive"); //// Added by Jignesh on 20-Jul-2020
                CheckBox chkM2_IsActive = (CheckBox)gridUser.FooterRow.FindControl("chkM2_IsActive"); //// Added by Jignesh on 20-Jul-2020
                CheckBox chkIsUserSentEmail = (CheckBox)gridUser.FooterRow.FindControl("chkIsUserSentEmail"); //// Added by Jignesh on 20-Jul-2020
                CheckBox chkIsProformaApproveAccess = (CheckBox)gridUser.FooterRow.FindControl("chkIsProformaApproveAccess"); //// Added by Jignesh on 01-Sep-2022

                UserMaster um = new UserMaster();
                um.EmpId = Convert.ToInt64(txtEmpId.Text);
                um.FirstName = txtFirstName.Text.Trim().ToUpper();
                um.MiddleName = txtMiddleName.Text.Trim().ToUpper();
                um.LastName = txtLastName.Text.Trim().ToUpper();
                um.UserName = txtUserName.Text.Trim().ToUpper();
                um.DepartmentId = Convert.ToInt64(ddlDepartment.SelectedValue);
                um.Email = txtEmail.Text.Trim().ToLower();
                um.UserType = ddlUserType.SelectedValue.ToString();
                um.Password = Global.StringToBinary(Global.Encrypt("Mitpl1234"));
                um.IsActive = true;
                um.U_M1 = chkM1_IsActive.Checked; //// Added by Jignesh on 20-Jul-2020
                um.U_M2 = chkM2_IsActive.Checked; //// Added by Jignesh on 20-Jul-2020
                um.IsUserSentEmail = chkIsUserSentEmail.Checked; //// Added by Jignesh on 20-Jul-2020
                um.ProformaApproveAccess = chkIsProformaApproveAccess.Checked; //// Added by Jignesh on 01-Sep-2022
                um.CreatedBy = Convert.ToInt64(Global.UserId);
                um.CreatedDate = DateTime.Now;
                
                dbobj.UserMasters.InsertOnSubmit(um);
                dbobj.SubmitChanges();
                dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                //ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + txtFirstName.Text.Trim().ToUpper() + " " + txtLastName.Text.Trim().ToUpper() + " successfully add')", true);
                FillGrid();
                FillDepartment();
                ViewState["UserEmail"] = txtEmail.Text.Trim().ToLower();
                this.ModalPopupExtender1.Show();
                break;
            case "Delete":
                GridViewRow gr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                Label lblUserId = (Label)gr.FindControl("lblUserId");
                Label lblFirstName = (Label)gr.FindControl("lblFirstName");
                Label lblLastName = (Label)gr.FindControl("lblLastName");
                var DeptData = from Dept in dbobj.DepartmentMasters
                               where Dept.UserId == Convert.ToInt64(lblUserId.Text)
                               select Dept;
                if (DeptData.Count() == 0)
                {
                    var DelUser = from DelU in dbobj.UserMasters
                                  where DelU.UserId == Convert.ToInt64(lblUserId.Text)
                                  select DelU;
                    if (DelUser.Count() > 0)
                    {
                        var UserS = DelUser.First();
                        if (UserS.IsActive == true)
                            UserS.IsActive = false;
                        else
                            UserS.IsActive = true;
                        dbobj.SubmitChanges();
                        dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                        //ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('User " + UserS.FirstName.ToUpper() + " " + UserS.LastName.ToUpper() + " is deleted" + "')", true);
                    }
                    FillGrid();
                    FillDepartment();
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + DeptData.First().UserMaster.FirstName.ToUpper() + " " + DeptData.First().UserMaster.LastName.ToUpper() + " user can not be deleted, It is already used by other" + "')", true);
                    OpenWindow(this, "MITPLInvoice", "DeletePopup.aspx?id=" + lblUserId.Text + "&name=" + lblFirstName.Text + lblLastName.Text + "&page=user", 650, 350);
                }
                break;
        }
    }

    protected void gridUser_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    protected void gridUser_RowEditing(object sender, GridViewEditEventArgs e)
    {
        Label lblDepartment = (Label)gridUser.Rows[e.NewEditIndex].FindControl("lblDepartment");
        Label lblUserType = (Label)gridUser.Rows[e.NewEditIndex].FindControl("lblUserType");
        gridUser.EditIndex = e.NewEditIndex;
        gridUser.ShowFooter = false;
        gridUser.FooterRow.Visible = false;
        gridUser.PagerSettings.Visible = false;
        FillGrid();
        DropDownList ddlEdtDepartment = (DropDownList)gridUser.Rows[e.NewEditIndex].FindControl("ddlEdtDepartment");
        DropDownList ddlEdtUserType = (DropDownList)gridUser.Rows[e.NewEditIndex].FindControl("ddlEdtUserType");
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
            ddlEdtDepartment.DataSource = Department;
            ddlEdtDepartment.DataTextField = "DepartmentName";
            ddlEdtDepartment.DataValueField = "DepartmentId";
            ddlEdtDepartment.DataBind();
        }
        ddlEdtDepartment.Items.FindByText(lblDepartment.Text).Selected = true;
        ddlEdtUserType.Items.FindByText(lblUserType.Text).Selected = true;
    }

    protected void gridUser_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gridUser.EditIndex = -1;
        gridUser.ShowFooter = true;
        gridUser.FooterRow.Visible = true;
        gridUser.PagerSettings.Visible = true;
        FillGrid();
        FillDepartment();
    }

    protected void gridUser_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        Label lblUserId = (Label)gridUser.Rows[e.RowIndex].FindControl("lblUserId");
        TextBox txtEdtEmpId = (TextBox)gridUser.Rows[e.RowIndex].FindControl("txtEdtEmpId");
        TextBox txtEdtFirstName = (TextBox)gridUser.Rows[e.RowIndex].FindControl("txtEdtFirstName");
        TextBox txtEdtMiddleName = (TextBox)gridUser.Rows[e.RowIndex].FindControl("txtEdtMiddleName");
        TextBox txtEdtLastName = (TextBox)gridUser.Rows[e.RowIndex].FindControl("txtEdtLastName");
        DropDownList ddlEdtDepartment = (DropDownList)gridUser.Rows[e.RowIndex].FindControl("ddlEdtDepartment");
        TextBox txtEdtEmail = (TextBox)gridUser.Rows[e.RowIndex].FindControl("txtEdtEmail");
        DropDownList ddlEdtUserType = (DropDownList)gridUser.Rows[e.RowIndex].FindControl("ddlEdtUserType");
        CheckBox chkEditM1_IsActive = (CheckBox)gridUser.Rows[e.RowIndex].FindControl("chkEditM1_IsActive"); //// Added by Jignesh on 20-Jul-2020
        CheckBox chkEditM2_IsActive = (CheckBox)gridUser.Rows[e.RowIndex].FindControl("chkEditM2_IsActive"); //// Added by Jignesh on 20-Jul-2020
        CheckBox chkEditIsUserSentEmail = (CheckBox)gridUser.Rows[e.RowIndex].FindControl("chkEditIsUserSentEmail"); //// Added by Jignesh on 20-Jul-2020
        CheckBox chkEditIsProformaApproveAccess = (CheckBox)gridUser.Rows[e.RowIndex].FindControl("chkEditIsProformaApproveAccess"); //// Added by Jignesh on 01-Sep-2022
        


        var UpdateData = from DBData in dbobj.UserMasters
                         where DBData.UserId == Convert.ToInt64(lblUserId.Text)
                         select DBData;
        if (UpdateData.Count() > 0)
        {
            var SingleUpdate = UpdateData.Single();
            SingleUpdate.EmpId = Convert.ToInt64(txtEdtEmpId.Text);
            SingleUpdate.FirstName = txtEdtFirstName.Text.Trim().ToUpper();
            SingleUpdate.MiddleName = txtEdtMiddleName.Text.Trim().ToUpper();
            SingleUpdate.LastName = txtEdtLastName.Text.Trim().ToUpper();
            SingleUpdate.DepartmentId = Convert.ToInt64(ddlEdtDepartment.SelectedValue);
            SingleUpdate.Email = txtEdtEmail.Text.Trim().ToLower();
            SingleUpdate.UserType = ddlEdtUserType.SelectedItem.ToString();
            SingleUpdate.ModifyBy = Convert.ToInt64(Global.UserId);
            SingleUpdate.ModifyDate = DateTime.Now;
            SingleUpdate.U_M1 = chkEditM1_IsActive.Checked; //// Added by Jignesh on 20-Jul-2020
            SingleUpdate.U_M2 = chkEditM2_IsActive.Checked; //// Added by Jignesh on 20-Jul-2020
            SingleUpdate.IsUserSentEmail= chkEditIsUserSentEmail.Checked; //// Added by Jignesh on 20-Jul-2020
            SingleUpdate.ProformaApproveAccess = chkEditIsProformaApproveAccess.Checked; //// Added by Jignesh on 01-Sep-2022

            dbobj.SubmitChanges();
            dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
            gridUser.EditIndex = -1;
            gridUser.ShowFooter = true;
            gridUser.FooterRow.Visible = true;
            gridUser.PagerSettings.Visible = true;
            FillGrid();
            FillDepartment();
        }
    }

    protected void gridUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridUser.PageIndex = e.NewPageIndex;
        FillGrid();
        FillDepartment();
    }

    protected void txtLastName_TextChanged(object sender, EventArgs e)
    {
        TextBox txtFirstName = (TextBox)gridUser.FooterRow.FindControl("txtFirstName");
        TextBox txtMiddleName = (TextBox)gridUser.FooterRow.FindControl("txtMiddleName");
        TextBox txtLastName = (TextBox)gridUser.FooterRow.FindControl("txtLastName");
        TextBox txtUserName = (TextBox)gridUser.FooterRow.FindControl("txtUserName");
        DropDownList ddlDepartment = (DropDownList)gridUser.FooterRow.FindControl("ddlDepartment");
        txtUserName.Text = "";
        if (txtLastName.Text != "")
        {
            string UserName = txtFirstName.Text.Trim() + txtLastName.Text.Trim().Substring(0, 1);
            var ExisUserName = from DBUserName in dbobj.UserMasters
                               where DBUserName.UserName == UserName.ToUpper()
                               select DBUserName;
            if (ExisUserName.Count() > 0)
            {
                for (int i = 1; i < 15; i++)
                {
                    if (i < txtLastName.Text.Trim().Length)
                    {
                        if (txtMiddleName.Text != "")
                        {
                            string MUserName = txtFirstName.Text.Trim() + txtMiddleName.Text.Trim().Substring(0, 1) + txtLastName.Text.Trim().Substring(0, i);
                            var ExisMUserName = from DBMUserName in dbobj.UserMasters
                                                where DBMUserName.UserName == MUserName.ToUpper()
                                                select DBMUserName;
                            if (ExisMUserName.Count() == 0)
                            {
                                txtUserName.Text = MUserName;
                                break;
                            }
                        }
                        else
                        {
                            string LUserName = txtFirstName.Text.Trim() + txtLastName.Text.Trim().Substring(0, i + 1);
                            var ExisMUserName = from DBMUserName in dbobj.UserMasters
                                                where DBMUserName.UserName == LUserName.ToUpper()
                                                select DBMUserName;
                            if (ExisMUserName.Count() == 0)
                            {
                                txtUserName.Text = LUserName;
                                break;
                            }
                        }
                    }
                    else
                    {
                        string EUserName = txtFirstName.Text.Trim() + txtLastName.Text.Trim() + ((i + 1) - txtLastName.Text.Trim().Length);
                        var ExisMUserName = from DBMUserName in dbobj.UserMasters
                                            where DBMUserName.UserName == EUserName.ToUpper()
                                            select DBMUserName;
                        if (ExisMUserName.Count() == 0)
                        {
                            txtUserName.Text = EUserName;
                            break;
                        }
                    }
                }
            }
            else
                txtUserName.Text = UserName;
            ddlDepartment.Focus();
        }
    }

    protected void txtFirstName_TextChanged(object sender, EventArgs e)
    {
        //TextBox txtMiddleName = (TextBox)gridUser.FooterRow.FindControl("txtMiddleName");
        //txtLastName_TextChanged(this, e);
        //txtMiddleName.Focus();
    }

    protected void imdAdd_Click(object sender, ImageClickEventArgs e)
    {
    }

    protected void imgUpdate_Click(object sender, ImageClickEventArgs e)
    {

    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            Page page1 = HttpContext.Current.Handler as Page;
            if (chkSendNotify.Checked)
            {
                if (txtEmailUser.Text.Trim().ToLower() == ViewState["UserEmail"].ToString())
                {
                    Global.SendEmailwithDisplay("corp@mangalaminfotech.com", txtEmailUser.Text.Trim().ToLower(), "New User Created", "Hello " + ViewState["FName"] + " " + ViewState["LName"] + ",<br/>New user on <b>Manglam e-Invoice System</b> associated with this email-id  has been created. Login credentials are as follows:<br/><br/>Emp. Id: " + ViewState["EmpIID"] + "<br/>Password: Mitpl1234<br/><br/>Please change your password when you login for first time else you will not allowed to use all the functionalities of application. <br/><br/>Thanks and Regards,<br/>Mangalam e-Invoice System Administrator<br/>http://invoice.mitplreports.com", "MITPL e-Invoice System Administrator");
                    FillGrid();
                    FillDepartment();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + txtEmailUser.Text.Trim().ToLower() + " doesnot match with email entered in user')", true);
                    this.ModalPopupExtender1.Show();
                }
            }
            txtEmailUser.Text = "";
        }

        catch { }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            this.ModalPopupExtender1.Hide();
        }

        catch { }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string GetDynamicContent(string contextKey)
    {
        return default(string);
    }

    public static void OpenWindow(Page currentPage, String window, String htmlPage, Int32 width, Int32 height)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("popWin=window.open('");
        sb.Append(htmlPage);
        sb.Append("','");
        sb.Append(window);
        sb.Append("','width=");
        sb.Append(width);
        sb.Append(",height=");
        sb.Append(height);
        sb.Append(",left=300,top=150,toolbar=no,location=center,directories=no,status=no,menubar=no,scrollbars=no,resizable=no");
        sb.Append("');");
        sb.Append("popWin.focus();");

        ScriptManager.RegisterClientScriptBlock(currentPage, typeof(CountryMaster), "OpenWindow", sb.ToString(), true);
    }

    protected void gridUser_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton Status = (LinkButton)e.Row.FindControl("lnkbtnStatus");
            Label lblFName = (Label)e.Row.FindControl("lblFirstName");
            Label lblLFName = (Label)e.Row.FindControl("lblLastName");
            if (Status != null)
            {
                if (Status.Text == "Active")
                {
                    Status.ToolTip = lblFName.Text + " " + lblLFName.Text + " is an active user, click to Inactive";
                }
                else
                {
                    Status.ToolTip = lblFName.Text + " " + lblLFName.Text + " is an inactive user, click to Active";
                }
            }
        }
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

}
