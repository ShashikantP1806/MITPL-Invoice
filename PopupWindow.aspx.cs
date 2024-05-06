using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupWindow : System.Web.UI.Page
{
    MITInvoiceDataContext dbobj = new MITInvoiceDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Global.UserType != "ADMINISTRATOR")
        {
            if (!IsPostBack)
            {
                FillGrid();
            }
        }
        else
        {
            Response.Redirect("Authorize");
        }
    }

    private void FillGrid()
    {
        if (Request.QueryString["i"] != null)
        {
            lblUserType.Text = Request.QueryString["c"] == "r" ? "Recipient" : (Request.QueryString["c"] == "c" ? "CC" : "BCC");
            string[] id = Request.QueryString["i"].ToString().Split(',');
            if (Request.QueryString["c"] == "r" || Request.QueryString["c"] == "c")
            {

                var ClientData = from DBData in dbobj.ClientContactMasters
                                 join
                                 InvoiceData in dbobj.InvoiceMasters
                                 on
                                 DBData.ClientId equals InvoiceData.ClientId
                                 where !DBData.IsDeleted && InvoiceData.InvoiceId == Convert.ToInt64(id[0])
                                 select new
                                 {
                                     ID = DBData.ClientContactId,
                                     ClientName = DBData.Name,
                                     ClientEmail = DBData.EmailAddress
                                 };
                grdClient.DataSource = ClientData;
                grdClient.DataBind();
                if (ClientData.Count() == 0)
                    btnSelect.Enabled = false;
            }
            else if (Request.QueryString["c"] == "b")
            {
                var InterUserData = from DBData in dbobj.UserMasters
                                    where DBData.IsActive
                                    select new
                                    {
                                        ID = DBData.UserId,
                                        ClientName = DBData.FirstName + " " + DBData.LastName,
                                        ClientEmail = DBData.Email
                                    };
                grdClient.DataSource = InterUserData;
                grdClient.DataBind();
                if (InterUserData.Count() == 0)
                    btnSelect.Enabled = false;
            }
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Write("<script language='javascript'>this.window.close();</script>");
    }

    protected void btnSelect_Click(object sender, EventArgs e)
    {
        string clientList = string.Empty;
        //foreach (GridViewRow gvr in grdClient.Rows)
        //{
        //    CheckBox chkC = (CheckBox)gvr.FindControl("chkClient");
        //    if (chkC.Checked)
        //    {
        //        Label lblEmail = (Label)gvr.FindControl("lblEmail");
        //        clientList = clientList == string.Empty ? lblEmail.Text : clientList + ";" + lblEmail.Text;
        //    }
        //}
        clientList = ViewState["emailID"].ToString();
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "Call my function", "alert('" + ViewState["temp"] + "')", true);

        switch (Request.QueryString["c"].ToString())
        {
            case "r":
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Call my function", "postValueRecp('" + clientList + "');", true);
                break;
            case "c":
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Call my function", "postValueCC('" + clientList + "');", true);
                break;
            case "b":
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Call my function", "postValueBCC('" + clientList + "');", true);
                break;
        }




    }

    protected void grdClient_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdClient.PageIndex = e.NewPageIndex;
        FillGrid();
    }

    protected void chkClient_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)sender;
        GridViewRow gr = (GridViewRow)chk.NamingContainer;
        Label lblSrNo = (Label)gr.FindControl("lblID");
        Label lblEmail = (Label)gr.FindControl("lblEmail");
        int RowID = gr.RowIndex;
        if (chk.Checked)
        {
            hfSelected.Value = hfSelected.Value == "" ? lblSrNo.Text : hfSelected.Value + "," + lblSrNo.Text;
            ViewState["emailID"] = ViewState["emailID"] == null ? lblEmail.Text : ViewState["emailID"] + ";" + lblEmail.Text;
        }
        else
        {
            if (hfSelected.Value != "")
            {
                string[] spltStr = hfSelected.Value.Split(',');
                string[] spltEmails = ViewState["emailID"].ToString().Split(';');
                string newVal = string.Empty;
                string emailIDs = string.Empty;
                for (int i = 0; i < spltStr.Length; i++)
                {
                    if (spltStr[i] != lblSrNo.Text)
                    {
                        newVal = newVal == string.Empty ? spltStr[i] : newVal + "," + spltStr[i];
                        emailIDs = emailIDs == string.Empty ? spltEmails[i] : emailIDs + ";" + spltEmails[i];
                    }
                }
                hfSelected.Value = newVal;
                ViewState["emailID"] = emailIDs;

            }
        }
    }

    protected void grdClient_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (hfSelected.Value != "")
        {
            string[] spltStr = hfSelected.Value.Split(',');

            //for (int i = 0; i < grdClient.Rows.Count; i++)
            //{
            GridViewRow gr = e.Row;
            Label lblSrNo = (Label)gr.FindControl("lblID");
            if (lblSrNo != null)
            {
                var IsSelected = from cData in spltStr.AsEnumerable()
                                 where cData.Equals(lblSrNo.Text)
                                 select cData;
                if (IsSelected.Count() > 0)
                {
                    CheckBox chkC = (CheckBox)gr.FindControl("chkClient");
                    chkC.Checked = true;
                }
            }
            //}

            //for (int i = 0; i < spltStr.Length; i++)
            //{
            //    GridViewRow gr = grdClient.Rows[Convert.ToInt32(spltStr[i])];
            //    if (gr != null)
            //    {

            //    }
            //}
        }
    }
}
