using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserProfile : System.Web.UI.Page
{
    MITInvoiceDataContext dbobj = new MITInvoiceDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.Master.FindControl("txtSearch").Visible = true;
            if (Request.Form.ToString() == "")
            {
                if (Request.QueryString.Count != 0)
                {
                    this.Master.FindControl("txtSearch").Visible = false;
                    this.Master.FindControl("mainmenu").Visible = false;
                    this.Master.FindControl("footermenu").Visible = false;
                    this.Master.FindControl("lnkSSD").Visible = false;
                    this.Master.FindControl("lblSSD").Visible = true;
                    btnOk.Text = "Update";
                    btnOk.ToolTip = "Update";
                    btnCancel.Visible = true;
                    txtFName.Enabled = true;
                    txtMName.Enabled = true;
                    txtLName.Enabled = true;
                    txtDesignation.Enabled = true;
                    txtEmail.Enabled = true;
                    txtContact.Enabled = true;
                    txtPassword.Enabled = true;
                }
            }
            FillProfile();
        }
    }

    protected void FillProfile()
    {
        var Profile = from DBData in dbobj.UserMasters
                      where DBData.IsActive == true && DBData.UserId == Convert.ToInt64(Global.UserId)
                      select DBData;
        if (Profile.Count() > 0)
        {
            var FPro = Profile.First();
            txtEmpId.Text = FPro.EmpId.ToString();
            txtUserName.Text = FPro.UserName;
            txtFName.Text = FPro.FirstName;
            txtMName.Text = FPro.MiddleName;
            txtLName.Text = FPro.LastName;
            txtDepartment.Text = FPro.DepartmentMaster.DepartmentName;
            txtDesignation.Text = FPro.Designation;
            txtEmail.Text = FPro.Email;
            txtContact.Text = FPro.Contact;
            txtUserType.Text = FPro.UserType;
            string Pass = Global.Decrypt(Global.BinaryToString(FPro.Password.ToArray()));
            txtPassword.Attributes.Add("value", Pass);
        }
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        if (btnOk.Text == "Edit")
        {
            btnOk.Text = "Update";
            btnOk.ToolTip = "Update";
            btnCancel.Visible = true;
            txtFName.Enabled = true;
            txtMName.Enabled = true;
            txtLName.Enabled = true;
            txtDesignation.Enabled = true;
            txtEmail.Enabled = true;
            txtContact.Enabled = true;
            txtPassword.Enabled = true;
        }
        else if (btnOk.Text == "Update")
        {
            Page page1 = HttpContext.Current.Handler as Page;

            var Update = from DBData in dbobj.UserMasters
                         where DBData.UserId == Convert.ToInt64(Global.UserId)
                         select DBData;
            if (Update.Count() > 0)
            {
                var FUpdate = Update.First();
                if (txtPassword.Text.Trim().Length >= 6 && !txtPassword.Text.Trim().ToUpper().Contains("MITPL") && !txtPassword.Text.Trim().ToUpper().Contains("MITL") && !txtPassword.Text.Trim().ToUpper().Contains(FUpdate.FirstName.ToUpper()) && !txtPassword.Text.Trim().ToUpper().Contains(FUpdate.LastName.ToUpper()))
                {
                    FUpdate.FirstName = txtFName.Text.Trim().ToUpper();
                    FUpdate.MiddleName = txtMName.Text.Trim().ToUpper();
                    FUpdate.LastName = txtLName.Text.Trim().ToUpper();
                    FUpdate.Designation = txtDesignation.Text.Trim().ToUpper();
                    FUpdate.Email = txtEmail.Text.Trim().ToLower();
                    FUpdate.Contact = txtContact.Text.Trim();
                    string pass = Global.Encrypt(txtPassword.Text.Trim());
                    FUpdate.Password = Global.StringToBinary(pass);
                    FUpdate.ModifyBy =Convert.ToInt64(Global.UserId);
                    FUpdate.ModifyDate = DateTime.Now;
                    dbobj.SubmitChanges();
                    dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);

                    //ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('Profile updated')", true);

                    this.Master.FindControl("mainmenu").Visible = true;
                    this.Master.FindControl("footermenu").Visible = true;
                    this.Master.FindControl("lnkSSD").Visible = true;
                    this.Master.FindControl("lblSSD").Visible = false;


                    btnOk.Text = "Edit";
                    btnOk.ToolTip = "Edit";
                    btnCancel.Visible = false;
                    txtFName.Enabled = false;
                    txtMName.Enabled = false;
                    txtLName.Enabled = false;
                    txtDesignation.Enabled = false;
                    txtEmail.Enabled = false;
                    txtContact.Enabled = false;
                    txtPassword.Enabled = false;
                    lblError.Visible = false;
                }
                else
                {
                    lblError.Visible = true;
                    if (txtPassword.Text.Trim().Length < 6)
                        lblError.Text = "Password Length should be minimum 6 characters";
                    else if (txtPassword.Text.Trim().ToUpper().Contains(FUpdate.FirstName.ToUpper()) || txtPassword.Text.Trim().ToUpper().Contains(FUpdate.LastName.ToUpper()))
                        lblError.Text = "Password should not contains 'FirstName' or 'LastName'";
                    else
                        lblError.Text = "Password should not contains 'Mitl' or 'Mitpl'";
                }
            }
        }
        FillProfile();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (this.Master.FindControl("mainmenu").Visible == false)
        {
            Session.Abandon();
            Response.Redirect("Login");
        }
        btnOk.Text = "Edit";
        btnOk.ToolTip = "Edit";
        btnCancel.Visible = false;
        txtFName.Enabled = false;
        txtMName.Enabled = false;
        txtLName.Enabled = false;
        txtDesignation.Enabled = false;
        txtEmail.Enabled = false;
        txtContact.Enabled = false;
        txtPassword.Enabled = false;
        lblError.Visible = false;
        FillProfile();
    }
}
