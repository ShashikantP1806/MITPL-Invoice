using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Management;
using System.Net;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Data;

public partial class Login : System.Web.UI.Page
{
    MITInvoiceDataContext dbobj = new MITInvoiceDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        trerror.Visible = false;
        lblError.Text = "";
        if (!IsPostBack)
        {
            Session["UserName"] = null;
            Session["Department"] = null;
            if (Request.QueryString.Keys.Count > 0)
            {
                if (Request.QueryString["validate"] != null)
                {
                    string dte = Request.QueryString["validate"].ToString();
                    string usId = Request.QueryString["u"].ToString();
                    ResetPasswordLink(usId, dte);
                }
                else
                {
                    // login using MITPL Login website
                    if (Request.QueryString["uid"] != null)
                    {
                        string uName = Global.DecryptMITPLLogin(Request.QueryString["uid"].ToString().Replace(" ", "+"));
                        string pwd = Global.DecryptMITPLLogin(Request.QueryString["pass"].ToString().Replace(" ", "+"));

                        txtUserId.Text = uName;
                        txtPassword.Attributes["value"] = pwd;
                        txtPassword.Text = txtPassword.Attributes["value"];
                        btnLogin_Click(this, e);
                    }
                }
            }
        }
        txtUserId.Focus();
    }

    protected void ResetPasswordLink(string userId, string validate)
    {
        string dte = validate.ToString();
        string uId = userId.ToString();
        //string dte = Global.Decrypt(validate.ToString().Replace(" ", "+"));
        //string uId = Global.Decrypt(userId.ToString().Replace(" ", "+"));
        DateTime dtlnk;
        dtlnk = Convert.ToDateTime(dte);
        if (DateTime.Now <= dtlnk.AddHours(6))
        {
            var Pass = from Dbdata in dbobj.PasswordLinks
                       where Dbdata.IsUsed == false && Dbdata.PasswordLink1 == Request.Url.ToString()
                       select Dbdata;
            if (Pass.Count() > 0)
            {
                var pass = Pass.Single();
                var PassChange = from DBData in dbobj.UserMasters
                                 where DBData.UserId == Convert.ToInt32(uId) && DBData.IsActive == true
                                 select DBData;
                if (PassChange.Count() > 0)
                {
                    var UpdatePass = PassChange.Single();
                    UpdatePass.Password = Global.StringToBinary(Global.Encrypt("Mitpl1234"));
                    pass.IsUsed = true;
                    pass.DateUsed = DateTime.Now;
                    dbobj.SubmitChanges();
                    dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                    trerror.Visible = true;
                    lblError.Text = "Password has been changed";
                }
                else
                {
                    trerror.Visible = true;
                    lblError.Text = "Invalid User Id";
                }
            }
            else
            {
                trerror.Visible = true;
                lblError.Text = "This password token has already been used once";
            }
        }
        else
        {
            trerror.Visible = true;
            lblError.Text = "This password token is expired, please resubmit password change request again";
        }
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        var UserData = from DBUser in dbobj.UserMasters
                       where DBUser.EmpId == Convert.ToInt64(txtUserId.Text.Trim())
                       select DBUser;
        if (UserData.Count() == 0)
        {
            trerror.Visible = true;
            lblError.Text = "EmpId doesnot exists.";
            txtUserId.Focus();
        }
        else
        {
            var LoginUser = UserData.First();
            if (!LoginUser.IsActive)
            {
                trerror.Visible = true;
                lblError.Text = "Inactive User";
                txtUserId.Focus();
            }
            else
            {
                string asd = Global.BinaryToString(LoginUser.Password.ToArray());
                string PWD = Global.Decrypt(asd);
                if (txtPassword.Text.Trim().Equals(PWD))
                {
                    Global.UserId = LoginUser.UserId.ToString();
                    Global.UserEmpCode = LoginUser.EmpId.ToString(); // updated on 01-Sep-2022 for Access 'ProformaInvoices' Approved Access
                    Global.UserProformaApproveAccess = LoginUser.ProformaApproveAccess.ToString();
                    Global.UserName = LoginUser.FirstName + " " + LoginUser.LastName;
                    Global.Department = LoginUser.DepartmentId.ToString();
                    Global.DepartmentName = LoginUser.DepartmentMaster.DepartmentName;
                    Global.UserType = LoginUser.UserType;

                    //////// Added by Jignesh on 24-Jul-2020 ////////
                    Global.UserM1 = Convert.ToBoolean(LoginUser.U_M1);
                    Global.UserM2 = Convert.ToBoolean(LoginUser.U_M2);
                    Global.UserIsSendEmail = Convert.ToBoolean(LoginUser.IsUserSentEmail);
                    /////////////////////////////////////////////////


                    if (txtPassword.Text.Trim().ToUpper().Contains("MITPL") || txtPassword.Text.Trim().ToUpper().Contains("MITL") || txtPassword.Text.Trim().Length < 6)
                        Response.Redirect("Profile?" + Global.Encrypt("a=a"));
                    else
                    {
                        if (Global.DepartmentName.ToUpper() == "TECH")
                            Response.Redirect("Manage");
                        else
                            Response.Redirect("Home");
                    }

                }
                else
                {
                    trerror.Visible = true;
                    lblError.Text = "Wrong password";
                    txtPassword.Focus();
                }
            }
        }
    }

    protected void lbtnForgotPassword_Click(object sender, EventArgs e)
    {
        Response.Redirect("ForgotPassword.aspx");
    }
}
