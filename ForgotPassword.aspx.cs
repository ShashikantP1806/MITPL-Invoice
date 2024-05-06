using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Data;
using System.Net;

public partial class ForgotPassword : System.Web.UI.Page
{
    MITInvoiceDataContext dbobj = new MITInvoiceDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            txtUserID.Focus();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Login");
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        var UserData = from DBData in dbobj.UserMasters
                       where DBData.IsActive == true && DBData.EmpId.ToString() == txtUserID.Text.Trim()
                       select new
                       {
                           userid = DBData.UserId,
                           empid = DBData.EmpId,
                           FirstName = DBData.FirstName,
                           LastName = DBData.LastName,
                           UserName = DBData.UserName,
                           EmailId = DBData.Email
                       };
        if (UserData.Count() > 0)
        {
            // Send password link to user email ID for reset password
            
            var userss = UserData.Single();

            //Code updated by Jignesh on 07-Jan-2020
            //var EmailConfig = (from dbEmail in dbobj.EmailConfigurations where dbEmail.EConfigId == 1 select dbEmail).Single();

            TechReportingDataContext dbtech = new TechReportingDataContext();
            var EmailConfig = (from dbEmail in dbtech.MITPLSiteEmailConfigurations
                               where dbEmail.SiteName == "Invoice"
                               select dbEmail).Single();


            SmtpClient emailClient = new SmtpClient();
            MailMessage message = new MailMessage();
            
            string validate = DateTime.Now.ToString();
            string uId = userss.userid.ToString();
            string Passlink = "http://invoice.mitplreports.com/login?validate=" + validate + "&u=" + uId + "&MON";
            //string Passlink = "http://localhost:3453/MITPLInvoice/Login.aspx?validate=" + validate + "&u=" + uId + "&MON";
            //string emailadd = "system@mangalaminfotech.net";
            string emailadd = "corp@mangalaminfotech.com";
            MailAddress fromAddress = new MailAddress(emailadd);
            message.From = fromAddress;
            message.To.Add(userss.EmailId);
            message.Subject = "MITPL Invoice Password Reset";
            message.IsBodyHtml = true;
            message.Body = "Hello " + userss.FirstName + " " + userss.LastName + ", <br/><br/><b><font color=\"#56A5EC\"><h3>Password Token</h3></font></b><br/>Reset your password with this temporary code. Please note that this link is only active for 6 hours after receipt. After this time limit has expired the code will not work and you will need to resubmit the password change request. Click on the following link to reset your password:<br/>" + Passlink + "<br/><br/><b> Your Password will be changed to \"Mitpl1234\"</b>.<br/><br/> Thanks and Regards,<br/>System and Software Department (SSD) Team ";
            
            emailClient.EnableSsl = true;
            emailClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            emailClient.Port = Convert.ToInt32(EmailConfig.Port);
            emailClient.Host = EmailConfig.Host;
            emailClient.UseDefaultCredentials = false;
            string HostPassword = Global.Decrypt(Global.BinaryToString(EmailConfig.HostPassword.ToArray()));
            emailClient.Credentials = new System.Net.NetworkCredential(EmailConfig.HostUserName, HostPassword);

            ServicePointManager.SecurityProtocol = (SecurityProtocolType)48 | (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
            emailClient.Send(message);

            lblError.Text = "Reset password link has been sent to your registered Email address with us";

            PasswordLink PasLin = new PasswordLink();
            PasLin.EmpCode = Convert.ToInt64(txtUserID.Text.Trim());
            PasLin.PasswordLink1 = Passlink.ToString();
            PasLin.DateCreated = DateTime.Now;
            PasLin.IsUsed = false;
            dbobj.PasswordLinks.InsertOnSubmit(PasLin);
            dbobj.SubmitChanges();
            dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
            txtUserID.Text = "";
        }
        else
        {
            lblError.Text = "User doesnot exists or user is not active, contact System and Sofware (SSD) Department Team";
            lblError.ForeColor = System.Drawing.Color.Red;
        }
    }
}
