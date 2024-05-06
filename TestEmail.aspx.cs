using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TestEmail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSend_Click(object sender, EventArgs e)
    {
        string status = Global.SendEmailTest("mayur.mehta@mangalaminfotech.com", "salesforce.mangalam@gmail.com", string.Empty, string.Empty, "Testing Email", "Hi, Please inform system department, if you receive this mail. ");
        if (status == "Sucess")
        {
            lblMessage.Text = "Message sent Sucessfuly";
        }
        else
        {
            lblMessage.Text = "Error...........................!";
        }
    }
}
