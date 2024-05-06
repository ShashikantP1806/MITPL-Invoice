using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PreviewProfomaInvoice : System.Web.UI.Page
{
    MITInvoiceDataContext dbobj = new MITInvoiceDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Global.UserType != "ADMINISTRATOR")
        {
            if (!IsPostBack)
                LoadPDF();
        }
        else
        {
            Response.Redirect("Authorize");
        }
    }

    private void LoadPDF()
    {
        string QueryString = Global.Decrypt(Request.Url.ToString().Substring(Request.Url.ToString().IndexOf("?") + 1, Request.Url.ToString().Length - (Request.Url.ToString().IndexOf("?") + 1)));
        string QS0 = QueryString.Substring(2, QueryString.Length - 2);
        string invName = string.Empty;
        MemoryStream stm = Global.GetProformaInvoice(Convert.ToInt64(QS0), "pdf", out invName);
        stm.Position = 0;
        Response.Clear();
        Response.ContentType = "application/pdf";
        Response.AddHeader("content-length", stm.Length.ToString());
        Response.BinaryWrite(stm.ToArray());
        Response.Flush();
        stm.Close();
        Response.End();
    }
}