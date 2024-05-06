using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.InteropServices.ComTypes;
using System.Data;
using System.Net;
using Ionic.Zip;
using System.Data.Linq.Mapping;

public partial class CommonClientInvoice : System.Web.UI.Page
{

    MITInvoiceDataContext dbobj = new MITInvoiceDataContext();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Global.UserId != null)
        {
            if (!IsPostBack)
            {
                GVInvoiceFiles.Columns[4].Visible = false;

                btnDownload.Visible = false;
                txtMonth.Text = DateTime.Now.ToString("MMM-yyyy");
                txtMonth_CalendarExtenderPlus.MaximumDate = DateTime.Now;
                FillClient();
                FillInvoiceFileGrid();
            }
        }        
    }

    protected void FillClient()
    {
        var Client = from DBClient in dbobj.ClientMasters
                     where DBClient.IsActive == true && (DBClient.C_M1 == Global.UserM1 || DBClient.C_M2 == Global.UserM2)
                     orderby DBClient.ClientName
                     select new
                     {
                         Name = DBClient.ClientName,
                         Id = DBClient.ClientId
                     };
        if (Client.Count() > 0)
        {
            ddlCCE.DataSource = Client;
            ddlCCE.DataTextField = "Name";
            ddlCCE.DataValueField = "Id";
            ddlCCE.DataBind();
            ddlCCE.Items.Insert(0, "--Select--");
        }
    }

    protected void txtMonth_TextChanged(object sender, EventArgs e)
    {
        FillInvoiceFileGrid();
    }

    protected void FillInvoiceFileGrid()
    {
        DateTime CDate = Convert.ToDateTime(DateTime.ParseExact(txtMonth.Text, "MMM-yyyy", null).ToString("MM/dd/yyyy"));
        
        if (ddlCCE.SelectedItem.Text == "--Select--")
        {
            
        }
        else
        {
            int test = Convert.ToInt32(ddlCCE.SelectedValue);

            var InvoiceFile = from DBInvoiceFile in dbobj.InvoiceMasters
                              join DBCliM in dbobj.ClientMasters
                              on DBInvoiceFile.ClientId equals DBCliM.ClientId
                              join AttM in dbobj.AttachmentMasters
                              on DBInvoiceFile.InvoiceId equals AttM.InvoiceId
                              where DBInvoiceFile.InvoiceDate.Month == CDate.Month && DBInvoiceFile.InvoiceDate.Year == CDate.Year
                              && AttM.AttachmentName.Contains("MIT_")
                              && Convert.ToInt32(DBInvoiceFile.ClientId) == Convert.ToInt32(ddlCCE.SelectedValue)
                              select new
                              {
                                  InvoiceNumber = DBInvoiceFile.InvoiceNumber,
                                  InvoiceDate = DBInvoiceFile.InvoiceDate,
                                  ClientName = DBCliM.ClientName,
                                  FileName = AttM.AttachmentName,
                                  AttID = AttM.AttachmentId
                              };

            if (InvoiceFile.Count() != 0)
            {
                GVInvoiceFiles.DataSource = InvoiceFile;
                GVInvoiceFiles.DataBind();
                btnDownload.Visible = true;
            }
            else 
            {
                DataTable dt = new DataTable();
                GVInvoiceFiles.DataSource = dt;
                GVInvoiceFiles.DataBind();
                btnDownload.Visible = false;
            }
        }
        
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        //string path = @"D:\" + ddlCCE.SelectedItem.Text + " - " + txtMonth.Text;
        //
        //if (!(Directory.Exists(path)))
        //{
        //    Directory.CreateDirectory(path);            
        //}

        string fiedFilePath = string.Empty;
        string CopyFile = string.Empty;

        string FolderName = ddlCCE.SelectedItem.Text + " - " + txtMonth.Text + ".Zip";

        ZipFile zip = new ZipFile();
        zip.AlternateEncodingUsage = ZipOption.AsNecessary;
        zip.AddDirectoryByName(FolderName);

        foreach (GridViewRow row in GVInvoiceFiles.Rows)
        {
            //Label txtStoreNumber = row.Cells[2].FindControl("lblInvFName") as Label;
            Label txtlblAttNum = row.Cells[3].FindControl("lblAttNum") as Label;

            fiedFilePath = Server.MapPath("~/UploadedFiles") + @"\" + txtlblAttNum.Text + ".pdf";

            //CopyFile = path + @"\" + txtlblAttNum.Text + ".pdf";
            //File.Copy(fiedFilePath, CopyFile);

            zip.AddFile(fiedFilePath, FolderName);
        }
        Response.Clear();
        Response.BufferOutput = false;        
        Response.ContentType = "application/zip";
        Response.AddHeader("content-disposition", "attachment; filename=" + FolderName);
        zip.Save(Response.OutputStream);
        Response.End();        

    }

    protected void ddlCCE_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillInvoiceFileGrid();
    }
}