using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ManageAccount : System.Web.UI.Page
{
    MITInvoiceDataContext dbobj = new MITInvoiceDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Global.DepartmentName.ToUpper() == "TECH")
        {
            if (!IsPostBack)
            {
                FillConfiguration();
            }
        }
        else
        {
            //Redirect unauthorized user to authorize page
            Response.Redirect("Authorize");
        }
    }

    public void FillConfiguration()
    {
        var getConfiguration = from dbEmailConfig in dbobj.EmailConfigurations
                               select dbEmailConfig;

        if (getConfiguration.Count() > 0)
        {
            if (getConfiguration.Single().HostPasswordNew != null)
            {
                string PWDBinaryToString = Global.BinaryToString(getConfiguration.Single().HostPasswordNew.ToArray());
                string PWD = Global.Decrypt(PWDBinaryToString);
            
                txtHostPassword.Attributes["value"] = PWD;
                txtHostPassword.Text = txtHostPassword.Attributes["value"];
            }
            else
            {
                txtHostPassword.Attributes["value"] = getConfiguration.Single().HostPassword.ToString();
                txtHostPassword.Text = txtHostPassword.Attributes["value"];
            }
            //um.Password = Global.StringToBinary(Global.Encrypt("Mitpl1234")); //NM@rk#230982
            txtHost.Text = getConfiguration.Single().HOST.ToString();
            txtPORT.Text = getConfiguration.Single().PORT.ToString();
            txtHostUserName.Text = getConfiguration.Single().HostUserName.ToString();
            //txtHostPassword.Attributes.Add("value", getConfiguration.Single().HostPassword.ToString());
            //txtHostPassword.Attributes["value"] = getConfiguration.Single().HostPassword.ToString();
            
           


        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        Page page = HttpContext.Current.Handler as Page;

        var updateConfiguration = from dbEmailConfig in dbobj.EmailConfigurations
                                  where dbEmailConfig.EConfigId == 1
                                  select dbEmailConfig;

        if (updateConfiguration.Count() > 0)
        {
            var update = updateConfiguration.Single();

            update.HOST = txtHost.Text.Trim();
            update.PORT = Convert.ToInt32(txtPORT.Text.Trim());
            update.HostUserName = txtHostUserName.Text.Trim();
            //update.HostPassword = txtHostPassword.Text.Trim();
            update.HostPasswordNew = Global.StringToBinary(Global.Encrypt(txtHostPassword.Text.Trim()));
            dbobj.SubmitChanges();
            dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
        }

        //System.Web.HttpContext.Current.Response.Redirect("Home");
        ScriptManager.RegisterStartupScript(page, page.GetType(), "MITPL Invoice", "alert('Password updated successfully')", true);
        //chkShow.Checked = false;
        //chkShow_CheckedChanged(null, null);
        Response.Redirect("Home");
        //Button1_Click(null, null);
    }

    protected void chkShow_CheckedChanged(object sender, EventArgs e)
    {
        //ViewState["PWD"] = txtHostPassword.Text.Trim();
        if (chkShow.Checked)
            txtHostPassword.TextMode = TextBoxMode.SingleLine;
        else
        {
            txtHostPassword.TextMode = TextBoxMode.Password;
            //txtHostPassword.Attributes.Add("value",txtHostPassword.Text.Trim());
            txtHostPassword.Attributes["value"] = txtHostPassword.Text.Trim();
            txtHostPassword.Text = txtHostPassword.Attributes["value"];
        }
    }


}