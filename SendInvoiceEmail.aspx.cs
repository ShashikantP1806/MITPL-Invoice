using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Globalization;

public partial class SendInvoiceEmail : System.Web.UI.Page
{
    /*************************************************************************/
    /*       Form to send email with attachments to client                   */
    /*************************************************************************/

    MITInvoiceDataContext dbobj = new MITInvoiceDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        //Only Non-Administrator Users are allowed to view this page
        if (Global.UserType != "ADMINISTRATOR")
        {
            if (!IsPostBack)
            {
                string QueryString = Global.Decrypt(Request.Url.ToString().Substring(Request.Url.ToString().IndexOf("?") + 1, Request.Url.ToString().Length - (Request.Url.ToString().IndexOf("?") + 1)));
                string[] QS = QueryString.Split('&');
                string QS0 = QS[0].Substring(2, QS[0].Length - 2);
                string QS1 = QS[1].Substring(2, QS[1].Length - 2);

                ViewState["QS0"] = QS0;
                ViewState["QS1"] = QS1;

                if (ViewState["QS1"] != null)
                {
                    if (ViewState["QS1"].ToString() == "I")
                        SetEmailData();
                    else
                    {
                        SetFollowupData();
                    }
                }
            }
            if (ViewState["QS1"] != null)
            {
                //QS1==F means Fill This Form for Invoice Followup
                if (ViewState["QS1"].ToString() == "F")
                    SetFollowupLink();
                //QS1==I means Fill This Form for Send Invoice to Client
                if (ViewState["QS1"].ToString() == "I")
                    SetInvoiceLink();
            }
        }
        else
        {
            //Redirect unauthorized user to authorize page
            Response.Redirect("Authorize");
        }
    }

    //Set Links for Invoice PDF files
    protected void SetInvoiceLink()
    {
        trFollowupInvoice.Visible = false;
        trSendInvoice.Visible = true;

        string[] InvoiceIDs = ViewState["QS0"].ToString().Split(',');
        if (InvoiceIDs[0] != "")
        {
            for (int i = 0; i < InvoiceIDs.Length; i++)
            {
                //lnkInvoice.Text = "<img src='images/PDF.ico' Height=20px Width=20px/>" + Global.InvoiceNo.Replace("/", "_");

                var InvoiceData = from DBData in dbobj.InvoiceMasters
                                  join
                                  ClientData in dbobj.ClientMasters
                                  on
                                  DBData.ClientId equals ClientData.ClientId
                                  ////where DBData.InvoiceId == Convert.ToInt64(Request.QueryString["i"])
                                  where DBData.InvoiceId == Convert.ToInt64(InvoiceIDs[i])
                                  select new
                                  {
                                      InvoiceNo = DBData.InvoiceNumber,
                                      PrimaryEmail = ClientData.PrimaryEmail,
                                      RecipientEmail = ClientData.DefaultRecipient, //////////// Change by Jignesh on 29-May-2019
                                      CCEmail = ClientData.DefaultCC, //////////// Change by Jignesh on 29-May-2019
                                      ProjectFrom = DBData.ProjectFrom.Value == 0 ? string.Empty : (from CData in dbobj.ClientContactMasters
                                                                                                    where CData.ClientContactId == DBData.ProjectFrom.Value
                                                                                                    select CData.EmailAddress
                                                                                                      ).Single(),
                                      PONumber = DBData.PONumber == null ? string.Empty : DBData.PONumber,
                                      InvoiceStartDate = DBData.InvoiceStartDate,
                                      InvoiceEndDate = DBData.InvoiceEndDate
                                  };
                if (InvoiceData.Count() > 0)
                {
                    LinkButton lnkbtn = new LinkButton();
                    lnkbtn.ID = "lnkbtn" + InvoiceIDs[i];

                    // Line updated on 24-01-2014 to display Invoice No without Revision No like INV-002A will be INV-002
                    string iNo = System.Text.RegularExpressions.Regex.Match(InvoiceData.Single().InvoiceNo.Substring(InvoiceData.Single().InvoiceNo.Length - 1, 1), "[a-zA-Z]$").Success ? InvoiceData.Single().InvoiceNo.Substring(0, InvoiceData.Single().InvoiceNo.Length - 1) : InvoiceData.Single().InvoiceNo;


                    //lnkbtn.Text = "<img src='images/PDF.ico' Height=20px Width=20px/>" + InvoiceData.Single().InvoiceNo.Replace("/", "_");
                    lnkbtn.Text = "<img src='images/PDF.ico' Height=20px Width=20px/>" + iNo.Replace("/", "_");
                    lnkbtn.Attributes.Add("runat", "server");
                    lnkbtn.Click += new EventHandler(lnkInvoice_Click);
                    //lnkbtn.Command += new EventHandler(lnkInvoice_Click);
                    divInvoice.Controls.Add(lnkbtn);

                    ImageButton imgRemoveDynamic = new ImageButton();
                    imgRemoveDynamic.ID = "imgbtn" + InvoiceIDs[i];
                    imgRemoveDynamic.ImageUrl = "~/images/Remove.ico";
                    imgRemoveDynamic.Height = 10;
                    imgRemoveDynamic.Width = 10;
                    imgRemoveDynamic.OnClientClick = "return confirm('Do you want to remove file?');";
                    imgRemoveDynamic.Click += new ImageClickEventHandler(imgbtnRemove_Click);
                    divInvoice.Controls.Add(imgRemoveDynamic);

                    Label lblBR = new Label();
                    lblBR.Text = "<br/>";
                    divInvoice.Controls.Add(lblBR);
                }
            }
        }
    }

    //Fill Form with Data for send email to client
    private void SetEmailData()
    {
        trFollowupInvoice.Visible = false;
        trSendInvoice.Visible = true;

        string[] InvoiceIDs = ViewState["QS0"].ToString().Split(',');
        string InvPeriod = string.Empty;
        string PONos = string.Empty;
        string InvNo = string.Empty;
        string InvoiceNos = string.Empty;
        string recpList = string.Empty;
        string ccList = string.Empty;
        bool IsClient_M1 = false;
        bool IsClient_M2 = false;

        System.Text.StringBuilder sbInvoiceTable = new System.Text.StringBuilder();
        sbInvoiceTable.AppendLine("<Table cellspacing=0 cellpadding=0 style=\"border-left: 1px solid black; border-top: 1px solid black;\"><tr><th  valign=\"bottom\" nowrap=\"\" style=\"border-right: 1px solid black; border-bottom: 1px solid black; background-color: silver;\">Sr. No.</th><th valign=\"bottom\" nowrap=\"\" style=\"border-right: 1px solid black; border-bottom: 1px solid black; background-color: silver;\">Invoice Date</th><th valign=\"bottom\" nowrap=\"\" style=\"border-right: 1px solid black; border-bottom: 1px solid black; background-color: silver;\">Invoice No.</th><th valign=\"bottom\" nowrap=\"\" style=\"border-right: 1px solid black; border-bottom: 1px solid black; background-color: silver;\">Invoice Amount</th valign=\"bottom\" nowrap=\"\" style=\"border-right: 1px solid black; border-bottom: 1px solid black; background-color: silver;\"></tr>");
        decimal totalAmount = 0;
        string clientCurrancyName = string.Empty;
        string clientCurrancySymbol = string.Empty;
        string invFor = string.Empty;
        //sb.AppendLine("<Table cellspacing=0 cellpadding=0 style=\"border-left: 1px solid black; border-top: 1px solid black;\"><tr><th valign=\"bottom\" nowrap=\"\" style=\"border-right: 1px solid black; border-bottom: 1px solid black; background-color: silver;\">Invoice No.</th><th  valign=\"bottom\" nowrap=\"\" style=\"border-right: 1px solid black; border-bottom: 1px solid black; background-color: silver;\">Invoice Period</th>");
        for (int i = 0; i < InvoiceIDs.Length; i++)
        {
            //lnkInvoice.Text = "<img src='images/PDF.ico' Height=20px Width=20px/>" + Global.InvoiceNo.Replace("/", "_");
            if (InvoiceIDs[i] != "")
            {
                var InvoiceData = from DBData in dbobj.InvoiceMasters
                                  join
                                  ClientData in dbobj.ClientMasters
                                  on
                                  DBData.ClientId equals ClientData.ClientId
                                  ////where DBData.InvoiceId == Convert.ToInt64(Request.QueryString["i"])
                                  where DBData.InvoiceId == Convert.ToInt64(InvoiceIDs[i])
                                  select new
                                  {
                                      InvoiceNo = DBData.InvoiceNumber,
                                      InvoiceDate = DBData.InvoiceDate,
                                      PrimaryEmail = ClientData.PrimaryEmail,
                                      RecipientEmail = ClientData.DefaultRecipient, //////////// Change by Jignesh on 29-May-2019
                                      CCEmail = ClientData.DefaultCC, //////////// Change by Jignesh on 29-May-2019
                                      ClientM1 = ClientData.C_M1, //////////// Change by Jignesh on 21-Aug-2020
                                      ClientM2 = ClientData.C_M2, //////////// Change by Jignesh on 21-Aug-2020
                                      InvFor = DBData.InvoiceFor,
                                      ClientCurrency = (from currData in dbobj.CurrencyMasters
                                                        where currData.CurrencyId == ClientData.CurrencyId
                                                        select currData.CurrencyName).Single(),
                                      ClientCurrencySymbol = (from currData in dbobj.CurrencyMasters
                                                        where currData.CurrencyId == ClientData.CurrencyId
                                                        select currData.CurrencySymbol).Single(), //////////// Change by Jignesh on 26-Oct-2021
                                      ProjectFrom = DBData.ProjectFrom.Value == 0 ? string.Empty : (from CData in dbobj.ClientContactMasters
                                                                                                    where CData.ClientContactId == DBData.ProjectFrom.Value
                                                                                                    select CData.EmailAddress
                                                                                                      ).Single(),
                                      PONumber = DBData.PONumber == null ? string.Empty : DBData.PONumber,
                                      InvoiceStartDate = DBData.InvoiceStartDate,
                                      InvoiceEndDate = DBData.InvoiceEndDate,
                                      InvoiceAmount = Math.Round(Convert.ToDecimal((from iData in dbobj.InvoiceDetailsMasters
                                                                                    where iData.InvoiceId == Convert.ToInt64(InvoiceIDs[i])
                                                                                    select iData.TotalAmt).Count() == 0 ? 0 : (from iData in dbobj.InvoiceDetailsMasters
                                                                                                                               where iData.InvoiceId == Convert.ToInt64(InvoiceIDs[i])
                                                                                                                               select iData.TotalAmt).Sum()), 2, MidpointRounding.AwayFromZero)

                                  };
                if (InvoiceData.Count() > 0)
                {
                    //// Added by Jignesh on 21-Aug-2020
                    IsClient_M1 = Convert.ToBoolean(InvoiceData.Single().ClientM1);
                    IsClient_M2 = Convert.ToBoolean(InvoiceData.Single().ClientM2);

                    string iNo = System.Text.RegularExpressions.Regex.Match(InvoiceData.Single().InvoiceNo.Substring(InvoiceData.Single().InvoiceNo.Length - 1, 1), "[a-zA-Z]$").Success ? InvoiceData.Single().InvoiceNo.Substring(0, InvoiceData.Single().InvoiceNo.Length - 1) : InvoiceData.Single().InvoiceNo;
                    //InvoiceNos = InvoiceNos == string.Empty ? InvoiceData.Single().InvoiceNo : InvoiceNos + ", " + InvoiceData.Single().InvoiceNo;
                    InvoiceNos = InvoiceNos == string.Empty ? iNo : InvoiceNos + ", " + iNo;
                    invFor = invFor == string.Empty ? InvoiceData.Single().InvFor : invFor;

                    ////if (recpList == string.Empty)
                    ////    recpList = InvoiceData.Single().PrimaryEmail + (InvoiceData.Single().ProjectFrom == "" ? "" : ";" + InvoiceData.Single().ProjectFrom);
                    ////else
                    ////    recpList = recpList + ";" + (InvoiceData.Single().PrimaryEmail + (InvoiceData.Single().ProjectFrom == "" ? "" : ";" + InvoiceData.Single().ProjectFrom));

                    //////////// Change by Jignesh on 29-May-2019
                    if (recpList == string.Empty)
                        recpList = InvoiceData.Single().PrimaryEmail + (InvoiceData.Single().ProjectFrom == "" ? "" : ";" + InvoiceData.Single().ProjectFrom) + (InvoiceData.Single().RecipientEmail == "" ? "" : ";" + InvoiceData.Single().RecipientEmail);
                    else
                        recpList = recpList + ";" + (InvoiceData.Single().PrimaryEmail + (InvoiceData.Single().ProjectFrom == "" ? "" : ";" + InvoiceData.Single().ProjectFrom) + (InvoiceData.Single().RecipientEmail == "" ? "" : ";" + InvoiceData.Single().RecipientEmail));

                    //////////// Added by Jignesh on 29-May-2019
                    ccList = InvoiceData.Single().CCEmail == "" ? "" : InvoiceData.Single().CCEmail;


                    if (InvoiceData.Single().InvoiceStartDate.Date.ToString("MMM yyyy") == InvoiceData.Single().InvoiceEndDate.Date.ToString("MMM yyyy"))
                    {
                        InvPeriod = InvPeriod == string.Empty ? InvoiceData.Single().InvoiceStartDate.Date.ToString("MMMM yyyy") : InvPeriod + ", " + InvoiceData.Single().InvoiceStartDate.Date.ToString("MMMM yyyy");
                    }
                    else
                    {
                        if (InvoiceData.Single().InvoiceStartDate.Date.Year == InvoiceData.Single().InvoiceEndDate.Date.Year)
                        {
                            string iPeriod = InvoiceData.Single().InvoiceStartDate.Date.ToString("MMMM") + " - " + InvoiceData.Single().InvoiceEndDate.Date.ToString("MMMM yyyy");
                            InvPeriod = InvPeriod == string.Empty ? iPeriod : InvPeriod + ", " + iPeriod;
                        }
                        else
                        {
                            string iPeriod = InvoiceData.Single().InvoiceStartDate.Date.ToString("MMMM yy") + " - " + InvoiceData.Single().InvoiceEndDate.Date.ToString("MMMM yy");
                            InvPeriod = InvPeriod == string.Empty ? iPeriod : InvPeriod + ", " + iPeriod;
                        }
                    }

                    if (InvoiceData.Single().PONumber != "")
                    {
                        PONos = PONos == string.Empty ? InvoiceData.Single().PONumber : PONos + ", " + InvoiceData.Single().PONumber;
                    }

                    InvPeriod = InvPeriod + (InvoiceData.Single().PONumber == "" ? string.Empty : " (PO #: " + InvoiceData.Single().PONumber + ")");
                    //InvNo = InvNo != "" ? InvNo + ", " + InvoiceData.Single().InvoiceNo : InvoiceData.Single().InvoiceNo;
                    InvNo = InvNo != "" ? InvNo + ", " + iNo : iNo;


                    //sb.AppendLine("<tr><td valign=\"bottom\" nowrap=\"\" style=\"border-right: 1px solid black; border-bottom: 1px solid black;\">" + InvoiceData.Single().InvoiceNo + "</td><td valign=\"bottom\" nowrap=\"\" style=\"border-right: 1px solid black; border-bottom: 1px solid black;\">" + InvPeriod + "</td></tr>");

                    /********************************************************/
                    /*          Table for Invoice List
                    /********************************************************/
                    if (clientCurrancyName == string.Empty)
                        clientCurrancyName = InvoiceData.Single().ClientCurrency;

                    if (clientCurrancySymbol == string.Empty) //////////// Change by Jignesh on 26-Oct-2021 to set Currency format with comma value
                        clientCurrancySymbol = HttpUtility.HtmlDecode("&#" + (InvoiceData.Single().ClientCurrencySymbol + ";").Replace(";;", ";"));

                    totalAmount = totalAmount + InvoiceData.Single().InvoiceAmount;
                    ////sbInvoiceTable.AppendLine("<tr><td valign=\"bottom\" nowrap=\"\" style=\"border-right: 1px solid black; border-bottom: 1px solid black;\">" + (i + 1).ToString() + "</td><td valign=\"bottom\" nowrap=\"\" style=\"border-right: 1px solid black; border-bottom: 1px solid black;\">" + InvoiceData.Single().InvoiceDate.ToString("MM.dd.yyyy") + "</td><td valign=\"bottom\" nowrap=\"\" style=\"border-right: 1px solid black; border-bottom: 1px solid black;\">" + InvoiceData.Single().InvoiceNo + "  </td><td valign=\"bottom\" align=\"center\" nowrap=\"\" style=\"border-right: 1px solid black; border-bottom: 1px solid black;\">     " + InvoiceData.Single().InvoiceAmount.ToString("0.00") + "</td></tr>");
                    ////// Updated by Jignesh on 20-Oct-2021 to set Amount as Currency format 
                    ////sbInvoiceTable.AppendLine("<tr><td valign=\"bottom\" nowrap=\"\" style=\"border-right: 1px solid black; border-bottom: 1px solid black;\">" + (i + 1).ToString() + "</td><td valign=\"bottom\" nowrap=\"\" style=\"border-right: 1px solid black; border-bottom: 1px solid black;\">" + InvoiceData.Single().InvoiceDate.ToString("MM.dd.yyyy") + "</td><td valign=\"bottom\" nowrap=\"\" style=\"border-right: 1px solid black; border-bottom: 1px solid black;\">" + iNo + "  </td><td valign=\"bottom\" align=\"center\" nowrap=\"\" style=\"border-right: 1px solid black; border-bottom: 1px solid black;\">     " + InvoiceData.Single().InvoiceAmount.ToString("0.00") + "</td></tr>");
                    sbInvoiceTable.AppendLine("<tr><td valign=\"bottom\" nowrap=\"\" style=\"border-right: 1px solid black; border-bottom: 1px solid black;\">" + (i + 1).ToString() + "</td><td valign=\"bottom\" nowrap=\"\" style=\"border-right: 1px solid black; border-bottom: 1px solid black;\">" + InvoiceData.Single().InvoiceDate.ToString("MM.dd.yyyy") + "</td><td valign=\"bottom\" nowrap=\"\" style=\"border-right: 1px solid black; border-bottom: 1px solid black;\">" + iNo + "  </td><td valign=\"bottom\" align=\"center\" nowrap=\"\" style=\"border-right: 1px solid black; border-bottom: 1px solid black;\">     " + clientCurrancySymbol + " " + InvoiceData.Single().InvoiceAmount.ToString("#,#.00") + "</td></tr>");

                }
            }
        }

        ////// Updated by Jignesh on 26-Oct-2021 to set Amount as Currency format 
        ////sbInvoiceTable.AppendLine("<tr><td  valign=\"bottom\" nowrap=\"\" style=\"border-right: 1px solid black; border-bottom: 1px solid black; background-color: silver;\" colspan=\"3\">Total " + clientCurrancyName + "</td><td   valign=\"bottom\" align=\"center\" nowrap=\"\" style=\"border-right: 1px solid black; border-bottom: 1px solid black; background-color: silver;\">" + totalAmount.ToString("0.00") + "</td>");
        sbInvoiceTable.AppendLine("<tr><td  valign=\"bottom\" nowrap=\"\" style=\"border-right: 1px solid black; border-bottom: 1px solid black; background-color: silver;\" colspan=\"3\">Total " + clientCurrancyName + "</td><td   valign=\"bottom\" align=\"center\" nowrap=\"\" style=\"border-right: 1px solid black; border-bottom: 1px solid black; background-color: silver;\">" + clientCurrancySymbol + " " + totalAmount.ToString("#,#.00") + "</td>");
        sbInvoiceTable.AppendLine("</Table><br/>");

        string[] spltInvPeriod = InvPeriod.Replace(", ", ",").Split(',');
        var InvPeriodData = (from iData in spltInvPeriod.AsEnumerable()
                             select iData).Distinct();
        string newInvPeriod = string.Empty;
        foreach (var iD in InvPeriodData)
        {
            newInvPeriod = newInvPeriod == string.Empty ? iD.ToString() : newInvPeriod + ", " + iD.ToString();
        }

        ////InvPeriod = PONos == string.Empty ? newInvPeriod : newInvPeriod + " (PO #: " + PONos + ")";
        string[] recps = recpList.Split(';');
        var RecpList = (from RecipientData in recps.AsEnumerable()
                        select RecipientData).Distinct();

        foreach (var rData in RecpList)
        {
            txtTo.Text = txtTo.Text == "" ? rData.ToString() : txtTo.Text + ";" + rData.ToString();
        }



        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine("Hi, <br/><br/>");
        if (InvoiceIDs.Length > 1)
        {
            txtSubject.Text = "Invoices for the month of " + newInvPeriod;
            sb.AppendLine("Please find attached Invoices & Invoice Details for the month of " + newInvPeriod + ".<br/><br/>");
            //sb.AppendLine("Please find attached Invoices # " + InvNo + " & Invoice Details for the month of " + InvPeriod + ".<br/><br/>");
        }
        else
        {
            sb.AppendLine("Please find attached Invoice & Invoice Details for the month of " + newInvPeriod + ".<br/><br/>");
            //sb.AppendLine("Please find attached Invoices # " + InvNo + " & Invoice Details for the month of " + InvPeriod + ".<br/><br/>");
            txtSubject.Text = "Invoice for the month of " + newInvPeriod;
        }
        sb.Append(sbInvoiceTable);

        sb.AppendLine("Please confirm receipt of the same.<br/><br/>");
        sb.AppendLine("Regards,<br/>");
        sb.AppendLine("Gautam Patel<br/>");
        switch (invFor.ToLower())
        {
            case "india":
                sb.AppendLine("Mangalam Information Technologies Pvt. Ltd.<br/>");
                sb.AppendLine("(ISO 27001:2005 Certified)<br/>");
                sb.AppendLine("Ahmedabad,India<br/>");
                sb.AppendLine("http://www.mangalaminfotech.com/<br/>");
                sb.AppendLine("US Phone: +1-914-461-4342<br/>");
                sb.AppendLine("India Phone : +91-79-26871240/26871241<br/>");
                sb.AppendLine("India Fax : +91-79-26871242<br/>");
                break;
            case "usa":
                sb.AppendLine("Mangalam Infotech USA<br/>");
                sb.AppendLine("<br/>");
                sb.AppendLine("http://www.mangalaminfotech.com/<br/>");
                sb.AppendLine("Phone: +1-914-461-4342<br/>");
                break;
            default:
                sb.AppendLine("Mangalam Infotech USA<br/>");
                sb.AppendLine("<br/>");
                sb.AppendLine("http://www.mangalaminfotech.com/<br/>");
                sb.AppendLine("Phone: +1-914-461-4342<br/>");
                break;
        }


        sb.AppendLine("<br/>");
        sb.AppendLine("==========================================================================<br/>");
        sb.AppendLine("Disclaimer:<br/><br/>");
        sb.AppendLine("\"The information contained in this electronic message and any attachments to this message are <br/>");
        sb.AppendLine("intended for the exclusive use of the addressee(s) and may contain confidential or privileged <br/>");
        sb.AppendLine("information. If you are not the intended recipient, please notify the sender at Mangalam’s email <br/>");
        sb.AppendLine("address immediately and destroy all copies of this message and any attachments.\"<br/>");
        sb.AppendLine("==========================================================================<br/>");

        //*** srinath mitragotri & madhvi bhandari set BCC to CC

        /*****************************************************************************************************/
        /*                          Comment this portion in testing mode
        /*****************************************************************************************************/
        // Old Comment by Jignesh on 21-Aug-2020 
        //txtCC.Text = "srinath.mitragotri@mangalaminfotech.com;madhvi.bhandari@mangalaminfotech.com";

        //// Change by Jignesh on 21-Aug-2020
        //// Client base CC email addess filled in txtCC.txt field
        string CCEmailToAdd = ";madhvi.bhandari@mangalaminfotech.com";
        string strSM = string.Empty, strCM = string.Empty;
        if (IsClient_M1 && !IsClient_M2)
        {
            txtCC.Text = "madhvi.bhandari@mangalaminfotech.com"; //Updated on 18-July-2022
            //txtCC.Text = "srinath.mitragotri@mangalaminfotech.com" + CCEmailToAdd;
        }
        else if (!IsClient_M1 && IsClient_M2)
        {
            txtCC.Text = "chetan.madhani@mangalaminfotech.com" + CCEmailToAdd;
        }
        else if (IsClient_M1 & IsClient_M2)
        {
            txtCC.Text = "chetan.madhani@mangalaminfotech.com" + CCEmailToAdd;
            //txtCC.Text = "srinath.mitragotri@mangalaminfotech.com;chetan.madhani@mangalaminfotech.com" + CCEmailToAdd;
        }

        //txtBCC.Text = "corp@mangalaminfotech.com;cpatel@mangalaminfotech.com";            // Update on 07/Jan/2016
        txtBCC.Text = "corp@mangalaminfotech.com;cpatel@mangalaminfotech.com;ppanchal@mangalaminfotech.com";            // Update on 18/Jul/2022
        /*****************************************************************************************************/

        /*****************************************************************************************************/
        /*                          Default CC value to add if availble
        /*****************************************************************************************************/
        if (ccList != null)
        {
            string[] strCC = ccList.Split(';');
            var getCCList = (from CCData in strCC.AsEnumerable()
                             select CCData).Distinct();

            foreach (var ccListData in getCCList)
            {
                txtCC.Text = txtCC.Text == "" ? ccListData.ToString() : txtCC.Text + ";" + ccListData.ToString();
            }
        }
        /*****************************************************************************************************/


        /*****************************************************************************************************/
        /*                          Comment this portion in Live
        /*****************************************************************************************************/
        //txtTo.Text = "salesforce.mangalam@gmail.com";
        //lblMailFrom.Text = "System@mangalaminfotech.net";
        /*****************************************************************************************************/

        var uData = from DBData in dbobj.UserMasters
                    where DBData.UserId == Convert.ToInt64(Global.UserId)
                    select DBData;
        if (uData.Count() > 0)
        {
            txtBCC.Text = txtBCC.Text == "" ? uData.Single().Email : txtBCC.Text + ";" + uData.Single().Email;
        }

        editorEmail.Content = sb.ToString();
    }

    //Set links for Invoice Files & Other attachments sent with Invoice email to client for followup purpose
    private void SetFollowupLink()
    {
        trSendInvoice.Visible = false;
        trFollowupInvoice.Visible = true;
        string[] InvoiceIDs = ViewState["QS0"].ToString().Split(',');
        string attText = string.Empty;

        for (int i = 0; i < InvoiceIDs.Length; i++)
        {
            Int64 InvID = Convert.ToInt64(InvoiceIDs[i]);
            string filePath = Server.MapPath("UploadedFiles");

            var GetInvoiceData = from DBData in dbobj.InvoiceMasters
                                 where DBData.InvoiceId == InvID
                                 select new
                                 {
                                     ClientID = DBData.ClientId,
                                     InvoiceDate = DBData.InvoiceDate,
                                     InvoiceNo = DBData.InvoiceNumber,
                                     InvoiceValue = (from iData in dbobj.InvoiceDetailsMasters
                                                     where iData.InvoiceId == InvID
                                                     select iData.TotalAmt).Count() == 0 ? 0 : (from iData in dbobj.InvoiceDetailsMasters
                                                                                                where iData.InvoiceId == InvID
                                                                                                select iData.TotalAmt).Sum()

                                 };

            var GetAttchments = from DBData in dbobj.AttachmentMasters
                                where DBData.InvoiceId == InvID
                                select DBData;
            foreach (var a in GetAttchments)
            {
                HyperLink lnkbtn = new HyperLink();
                //LinkButton lnkbtn = new LinkButton();
                lnkbtn.ID = "lnkbtn" + a.AttachmentId;
                lnkbtn.Target = "_blank";


                //lnkbtn.Click += new EventHandler(lnkInvoiceDynamic_Click);
                string extVal = Path.GetExtension(a.AttachmentName).Replace(".", string.Empty);
                if (extVal.ToUpper() == "JPG" || extVal.ToUpper() == "JPEG" || extVal.ToUpper() == "PNG" || extVal.ToUpper() == "PDF" || extVal.ToUpper() == "XLS" || extVal.ToUpper() == "XLSX" || extVal.ToUpper() == "DOC" || extVal.ToUpper() == "DOCX" || extVal.ToUpper() == "GIF" || extVal.ToUpper() == "GIFF" || extVal.ToUpper() == "TIF" || extVal.ToUpper() == "TIFF" || extVal.ToUpper() == "ZIP" || extVal.ToUpper() == "RAR")
                    lnkbtn.Text = "<img src='images/" + Path.GetExtension(a.AttachmentName).Replace(".", string.Empty).ToUpper() + ".ico' Height=20px Width=20px/>" + a.AttachmentName;
                else
                    lnkbtn.Text = "<img src='images/" + "OTHER.ico' Height=20px Width=20px/>" + a.AttachmentName;


                string filename = filePath + @"\" + a.AttachmentId + "." + extVal;
                lnkbtn.NavigateUrl = filename;//.Replace(@"\", @"/");

                divAttachments.Controls.Add(lnkbtn);

                Label lblBlank = new Label();
                lblBlank.Text = " ";
                divAttachments.Controls.Add(lblBlank);


                ImageButton imgRemoveDynamic = new ImageButton();
                imgRemoveDynamic.ID = "imgbtn" + a.AttachmentId;
                imgRemoveDynamic.ImageUrl = "~/images/Remove.ico";
                imgRemoveDynamic.Height = 10;
                imgRemoveDynamic.Width = 10;
                imgRemoveDynamic.OnClientClick = "return confirm('Do you want to remove file?');";
                imgRemoveDynamic.Click += new ImageClickEventHandler(imgbtnRemoveDynamic_Click);
                divAttachments.Controls.Add(imgRemoveDynamic);

                Label lblBlank2 = new Label();
                lblBlank2.Text = " ";
                divAttachments.Controls.Add(lblBlank2);


            }
            Label lblBR = new Label();
            lblBR.Text = "<br/>";
            divAttachments.Controls.Add(lblBR);
        }
    }

    //Fill form with Data for send followup email
    private void SetFollowupData()
    {
        trSendInvoice.Visible = false;
        trFollowupInvoice.Visible = true;
        /////string[] InvoiceIDs = Request.QueryString["i"].ToString().Split(',');
        string[] InvoiceIDs = ViewState["QS0"].ToString().Split(',');
        string attText = string.Empty;
        System.Text.StringBuilder sbTable = new System.Text.StringBuilder();
        string financialYear = string.Empty;
        string recpList = string.Empty; // Added by Jignesh on 06-Jun-2019
        string ccList = string.Empty; // Added by Jignesh on 06-Jun-2019
        Int64 ClientID = 0;
        decimal totalAmount = 0;

        sbTable.AppendLine("<Table cellspacing=0 cellpadding=0 style=\"border-left: 1px solid black; border-top: 1px solid black;\"><tr><th valign=\"bottom\" nowrap=\"\" style=\"border-right: 1px solid black; border-bottom: 1px solid black; background-color: silver;\">Invoice Date</th><th valign=\"bottom\" nowrap=\"\" style=\"border-right: 1px solid black; border-bottom: 1px solid black; background-color: silver;\">Invoice No.</th><th valign=\"bottom\" nowrap=\"\" style=\"border-right: 1px solid black; border-bottom: 1px solid black; background-color: silver;\">Invoice Value</th valign=\"bottom\" nowrap=\"\" style=\"border-right: 1px solid black; border-bottom: 1px solid black; background-color: silver;\"><th  valign=\"bottom\" nowrap=\"\" style=\"border-right: 1px solid black; border-bottom: 1px solid black; background-color: silver;\">Due Amount</th>");
        // Add Attachments links
        for (int i = 0; i < InvoiceIDs.Length; i++)
        {
            Int64 InvID = Convert.ToInt64(InvoiceIDs[i]);
            string filePath = Server.MapPath("UploadedFiles");


            var GetInvoiceData = from DBData in dbobj.InvoiceMasters
                                 where DBData.InvoiceId == InvID
                                 select new
                                 {
                                     ClientID = DBData.ClientId,
                                     InvoiceDate = DBData.InvoiceDate,
                                     InvoiceNo = DBData.InvoiceNumber,
                                     PrimaryEmail = DBData.ClientMaster.PrimaryEmail, // Added by Jignesh on 06-Jun-2019
                                     RecipientEmail = DBData.ClientMaster.DefaultRecipient, // Added by Jignesh on 06-Jun-2019
                                     CCEmail = DBData.ClientMaster.DefaultCC, // Added by Jignesh on 06-Jun-2019
                                     InvoiceValue = Math.Round(Convert.ToDecimal((from iData in dbobj.InvoiceDetailsMasters
                                                                                  where iData.InvoiceId == InvID
                                                                                  select iData.TotalAmt).Count() == 0 ? 0 : (from iData in dbobj.InvoiceDetailsMasters
                                                                                                                             where iData.InvoiceId == InvID
                                                                                                                             select iData.TotalAmt).Sum()), 2, MidpointRounding.AwayFromZero)

                                 };
            if (GetInvoiceData.Count() > 0)
            {
                string iNo = System.Text.RegularExpressions.Regex.Match(GetInvoiceData.Single().InvoiceNo.Substring(GetInvoiceData.Single().InvoiceNo.Length - 1, 1), "[a-zA-Z]$").Success ? GetInvoiceData.Single().InvoiceNo.Substring(0, GetInvoiceData.Single().InvoiceNo.Length - 1) : GetInvoiceData.Single().InvoiceNo;

                sbTable.AppendLine("<tr><td valign=\"bottom\" nowrap=\"\" style=\"border-right: 1px solid black; border-bottom: 1px solid black;\">" + GetInvoiceData.Single().InvoiceDate.ToString("MM.dd.yyyy") + "</td><td valign=\"bottom\" nowrap=\"\" style=\"border-right: 1px solid black; border-bottom: 1px solid black;\">" + iNo + "</td><td valign=\"bottom\" align=\"center\" nowrap=\"\" style=\"border-right: 1px solid black; border-bottom: 1px solid black;\">" + GetInvoiceData.Single().InvoiceValue + "</td><td valign=\"bottom\" align=\"center\" nowrap=\"\" style=\"border-right: 1px solid black; border-bottom: 1px solid black;\">" + GetInvoiceData.Single().InvoiceValue + "</td></tr>");
                if (financialYear == string.Empty)
                {
                    string[] spltStr = GetInvoiceData.Single().InvoiceNo.Split('/');
                    financialYear = spltStr[1];
                    ClientID = GetInvoiceData.Single().ClientID;
                }
                totalAmount = totalAmount + GetInvoiceData.Single().InvoiceValue;

                //// Added by Jignesh on 06-Jun-2019
                if (recpList == string.Empty)
                    recpList = GetInvoiceData.Single().PrimaryEmail + (GetInvoiceData.Single().RecipientEmail == "" ? "" : ";" + GetInvoiceData.Single().RecipientEmail);
                else
                    recpList = recpList + ";" + (GetInvoiceData.Single().PrimaryEmail + (GetInvoiceData.Single().RecipientEmail == "" ? "" : ";" + GetInvoiceData.Single().RecipientEmail));

                //// Added by Jignesh on 06-Jun-2019
                ccList = GetInvoiceData.Single().CCEmail == "" ? "" : GetInvoiceData.Single().CCEmail;
            }

            //    var GetAttchments = from DBData in dbobj.AttachmentMasters
            //                        where DBData.InvoiceId == InvID
            //                        select DBData;
            //    foreach (var a in GetAttchments)
            //    {
            //        HyperLink lnkbtn = new HyperLink();
            //        //LinkButton lnkbtn = new LinkButton();
            //        lnkbtn.ID = "lnkbtn" + a.AttachmentId;
            //        lnkbtn.Target = "_blank";


            //        //lnkbtn.Click += new EventHandler(lnkInvoiceDynamic_Click);
            //        string extVal = Path.GetExtension(a.AttachmentName).Replace(".", string.Empty);
            //        if (extVal.ToUpper() == "JPG" || extVal.ToUpper() == "JPEG" || extVal.ToUpper() == "PNG" || extVal.ToUpper() == "PDF" || extVal.ToUpper() == "XLS" || extVal.ToUpper() == "XLSX" || extVal.ToUpper() == "DOC" || extVal.ToUpper() == "DOCX" || extVal.ToUpper() == "GIF" || extVal.ToUpper() == "GIFF" || extVal.ToUpper() == "TIF" || extVal.ToUpper() == "TIFF" || extVal.ToUpper() == "ZIP" || extVal.ToUpper() == "RAR")
            //            lnkbtn.Text = "<img src='images/" + Path.GetExtension(a.AttachmentName).Replace(".", string.Empty).ToUpper() + ".ico' Height=20px Width=20px/>" + a.AttachmentName;
            //        else
            //            lnkbtn.Text = "<img src='images/" + "OTHER.ico' Height=20px Width=20px/>" + a.AttachmentName;


            //        string filename = filePath + @"\" + a.AttachmentId + "." + extVal;
            //        lnkbtn.NavigateUrl = filename;//.Replace(@"\", @"/");

            //        divAttachments.Controls.Add(lnkbtn);

            //        Label lblBlank = new Label();
            //        lblBlank.Text = " ";
            //        divAttachments.Controls.Add(lblBlank);


            //        ImageButton imgRemoveDynamic = new ImageButton();
            //        imgRemoveDynamic.ID = "imgbtn" + a.AttachmentId;
            //        imgRemoveDynamic.ImageUrl = "~/images/Remove.ico";
            //        imgRemoveDynamic.Height = 10;
            //        imgRemoveDynamic.Width = 10;
            //        imgRemoveDynamic.OnClientClick = "return confirm('Do you want to remove file?');";
            //        imgRemoveDynamic.Click += new ImageClickEventHandler(imgbtnRemoveDynamic_Click);
            //        divAttachments.Controls.Add(imgRemoveDynamic);


            //        Label lblBlank2 = new Label();
            //        lblBlank2.Text = " ";
            //        divAttachments.Controls.Add(lblBlank2);


            //    }
            //    Label lblBR = new Label();
            //    lblBR.Text = "<br/>";
            //    divAttachments.Controls.Add(lblBR);
        }


        sbTable.AppendLine("<tr><td  valign=\"bottom\" nowrap=\"\" style=\"border-right: 1px solid black; border-bottom: 1px solid black; background-color: silver;\" colspan=\"3\">Total Due Amount</td><td   valign=\"bottom\" align=\"center\" nowrap=\"\" style=\"border-right: 1px solid black; border-bottom: 1px solid black; background-color: silver;\">" + totalAmount.ToString("0.00") + "</td>");
        sbTable.AppendLine("</Table>");

        //Fill Default Follow-up mail text

        /*****************************************************************************************************/
        /*                     Comment this portion in testing mode
        /*****************************************************************************************************/
        lblMailFrom.Text = "corp@mangalaminfotech.com";
        //txtBCC.Text = "srinath.mitragotri@mangalaminfotech.com;sarit@mangalaminfotech.com;madhvi.bhandari@mangalaminfotech.com;cpatel@mangalaminfotech.com";
        //txtCC.Text = "corp@mangalaminfotech.com;srinath.mitragotri@mangalaminfotech.com;madhvi.bhandari@mangalaminfotech.com;cpatel@mangalaminfotech.com";
        txtCC.Text = "corp@mangalaminfotech.com;madhvi.bhandari@mangalaminfotech.com;cpatel@mangalaminfotech.com"; //Updated on 18-July-2022
        txtBCC.Text = "ppanchal@mangalaminfotech.com";
        /*****************************************************************************************************/


        /*****************************************************************************************************/
        /*                          Default CC value to add if availble / added on 06-Jun-2019
        /*****************************************************************************************************/
        if (ccList != null)
        {
            string[] strCC = ccList.Split(';');
            var getCCList = (from CCData in strCC.AsEnumerable()
                             select CCData).Distinct();

            foreach (var ccListData in getCCList)
            {
                txtCC.Text = txtCC.Text == "" ? ccListData.ToString() : txtCC.Text + ";" + ccListData.ToString();
            }
        }
        /*****************************************************************************************************/


        /*****************************************************************************************************/
        /*                   Comment this portion in Live
        /*****************************************************************************************************/
        //lblMailFrom.Text = "System@mangalaminfotech.net";

        /*****************************************************************************************************/


        txtSubject.Text = "Outstanding dues - Mangalam";

        //////Get Client Details
        ////var CData = from DBData in dbobj.ClientMasters
        ////            where DBData.ClientId == ClientID
        ////            select DBData;
        ////if (CData.Count() > 0)
        ////{
        ////    txtTo.Text = CData.Single().PrimaryEmail;
        ////}

        ////Get Client Details
        ////Added by Jignesh on 06-Jun-2019
        string[] recps = recpList.Split(';');
        var RecpList = (from RecipientData in recps.AsEnumerable()
                        select RecipientData).Distinct();

        foreach (var rData in RecpList)
        {
            txtTo.Text = txtTo.Text == "" ? rData.ToString() : txtTo.Text + ";" + rData.ToString();
        }


        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine("Hi,<br/>");
        sb.AppendLine("<br/>");
        sb.AppendLine("Greetings from Mangalam Infotech USA<br/>");
        sb.AppendLine("<br/>");
        sb.AppendLine("We would like to draw your attention to a few unpaid invoices for the services we provided to your organization in the year " + financialYear + "<br/>");
        sb.AppendLine("<br/>");

        sb.AppendLine(sbTable.ToString() + "<br/>");



        sb.AppendLine("We have attached copies of the above invoices for your verification. If you need any other details to settle these invoices, we will be happy to provide them. We would really appreciate if you can let us know the expected date for the payment of these invoices.<br/>");
        sb.AppendLine("<br/>");
        sb.AppendLine("Many Thanks & Best Regards,<br/>");
        sb.AppendLine("Gautam Patel<br/>");
        sb.AppendLine("Mangalam Information Technologies Pvt. Ltd.<br/>");
        sb.AppendLine("(ISO 27001:2005 Certified)<br/>");
        sb.AppendLine("Ahmedabad,India<br/>");
        sb.AppendLine("India Phone : +91-79-26871240/26871241<br/>");
        sb.AppendLine("India Fax : +91-79-26871242<br/>");
        sb.AppendLine("US Phone : 914-461-4342<br/>");

        sb.AppendLine("<br/>");
        sb.AppendLine("==========================================================================<br/>");
        sb.AppendLine("Disclaimer:<br/><br/>");
        sb.AppendLine("\"The information contained in this electronic message and any attachments to this message are <br/>");
        sb.AppendLine("intended for the exclusive use of the addressee(s) and may contain confidential or privileged <br/>");
        sb.AppendLine("information. If you are not the intended recipient, please notify the sender at Mangalam’s email <br/>");
        sb.AppendLine("address immediately and destroy all copies of this message and any attachments.\"<br/>");
        sb.AppendLine("==========================================================================<br/>");


        editorEmail.Content = sb.ToString();
    }

    //Add New Email Recipient
    protected void imgAddRecp_Click(object sender, ImageClickEventArgs e)
    {
        ////Response.Write("<script language='javascript'>window.open('PopupWindow?i=" + Request.QueryString["i"] + "&c=r', null,'height=600,width=650,status=yes,toolbar=no,menubar=no,location=no');openwindow.focus();</script>");
        if (ViewState["QS1"].ToString() == "I")
            Response.Write("<script language='javascript'>window.open('PopupWindow?i=" + ViewState["QS0"] + "&c=r', null,'height=600,width=650,status=yes,toolbar=no,menubar=no,location=no');openwindow.focus();</script>");
        else
        {
            string[] spltStr = ViewState["QS0"].ToString().Split(',');
            Response.Write("<script language='javascript'>window.open('PopupWindow?i=" + spltStr[0] + "&c=r', null,'height=600,width=650,status=yes,toolbar=no,menubar=no,location=no');openwindow.focus();</script>");
        }
        //ClientScript.RegisterStartupScript(this.GetType(), "Open Invoice", "window.open('PopupWindow?i=" + Request.QueryString["i"] + "&c=r', null,'height=500,width=550,status=yes,toolbar=no,menubar=no,location=no');openwindow.focus();", true);
    }

    //Add New Email CC
    protected void imgAddCC_Click(object sender, ImageClickEventArgs e)
    {
        /////Response.Write("<script language='javascript'>window.open('PopupWindow?i=" + Request.QueryString["i"] + "&c=c', null,'height=600,width=650,status=yes,toolbar=no,menubar=no,location=no');openwindow.focus();</script>");
        if (ViewState["QS1"].ToString() == "I")
            Response.Write("<script language='javascript'>window.open('PopupWindow?i=" + ViewState["QS0"] + "&c=c', null,'height=600,width=650,status=yes,toolbar=no,menubar=no,location=no');openwindow.focus();</script>");
        else
        {
            string[] spltStr = ViewState["QS0"].ToString().Split(',');
            Response.Write("<script language='javascript'>window.open('PopupWindow?i=" + spltStr[0] + "&c=c', null,'height=600,width=650,status=yes,toolbar=no,menubar=no,location=no');openwindow.focus();</script>");
        }
        //ClientScript.RegisterStartupScript(this.GetType(), "Open Invoice", "window.open('PopupWindow?i=" + Request.QueryString["i"] + "&c=c', null,'height=500,width=550,status=yes,toolbar=no,menubar=no,location=no');openwindow.focus();", true);
    }

    //Add New Email BCC
    protected void imgAddBCC_Click(object sender, ImageClickEventArgs e)
    {
        /////Response.Write("<script language='javascript'>window.open('PopupWindow?i=" + Request.QueryString["i"] + "&c=b', null,'height=600,width=650,status=yes,toolbar=no,menubar=no,location=no');openwindow.focus();</script>");
        Response.Write("<script language='javascript'>window.open('PopupWindow?i=" + ViewState["QS0"] + "&c=b', null,'status=yes,toolbar=no,menubar=no,location=no');openwindow.focus();</script>");
        //ClientScript.RegisterStartupScript(this.GetType(), "Open Invoice", "window.open('PopupWindow?i=" + ViewState["QS0"] + "&c=b', null,'height=500,width=550,status=yes,toolbar=no,menubar=no,location=no');openwindow.focus();", true);
    }

    //Click event for Invoice Link
    protected void lnkInvoice_Click(object sender, EventArgs e)
    {
        //Response.Write("<script language='javascript'>window.open('PreviewInvoice?i=" + Request.QueryString["i"] + "', null,'height=700,width=750,status=yes,toolbar=no,menubar=no,location=no');</script>");
        /////ClientScript.RegisterStartupScript(this.GetType(), "Open Invoice", "window.open('PreviewInvoice?i=" + Request.QueryString["i"] + "', null,'height=700,width=750,status=yes,toolbar=no,menubar=no,location=no');", true);
        LinkButton lnk = (LinkButton)sender;
        string InvID = lnk.ID.ToString().Replace("lnkbtn", "");
        //ClientScript.RegisterStartupScript(this.GetType(), "Open Invoice", "window.open('PreviewInvoices?" + Global.Encrypt("i=" + ViewState["QS0"]) + "', null,'height=700,width=750,status=yes,toolbar=no,menubar=no,location=no');", true);
        ClientScript.RegisterStartupScript(this.GetType(), "Open Invoice", "window.open('PreviewInvoices?" + Global.Encrypt("i=" + InvID) + "', null,'height=700,width=750,status=yes,toolbar=no,menubar=no,location=no');", true);
    }

    //Click event for Invoice Followup Links
    protected void lnkInvoiceDynamic_Click(object sender, EventArgs e)
    {
        LinkButton lnkbtn = (LinkButton)sender;
        Int64 invID = Convert.ToInt64(lnkbtn.ID.Replace("lnkbtn", string.Empty));
        var AttaName = from DBData in dbobj.AttachmentMasters
                       where DBData.AttachmentId == invID
                       select DBData;
        string filePath = Server.MapPath("UploadedFiles");
        string filename = filePath + @"\" + AttaName.Single().AttachmentId + Path.GetExtension(AttaName.Single().AttachmentName);
        //Response.Write("<script language='javascript'>window.open('PreviewInvoice?i=" + Request.QueryString["i"] + "', null,'height=700,width=750,status=yes,toolbar=no,menubar=no,location=no');</script>");
        ClientScript.RegisterStartupScript(this.GetType(), "Open Invoice", "window.open('" + filename + "', null,'height=700,width=750,status=yes,toolbar=no,menubar=no,location=no');", true);
    }

    //Send email to client
    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        string attachIDs = string.Empty;
        Page page = HttpContext.Current.Handler as Page;
        ClientScript.RegisterStartupScript(this.GetType(), "Test", "showLayer();", true);

        //Send Email for New Invoice
        if (ViewState["QS1"].ToString() == "I")
        {
            try
            {
                string[] InvoiceIDs = ViewState["QS0"].ToString().Split(',');
                string filePath = Server.MapPath("UploadedFiles");

                for (int i = 0; i < InvoiceIDs.Length; i++)
                {
                    if (InvoiceIDs[i] != "")
                    {
                        //Delete Previous Attachment Values
                        var GetAtt = from DBData in dbobj.AttachmentMasters
                                         ////where DBData.InvoiceId == Convert.ToInt64(Request.QueryString["i"])
                                         //where DBData.InvoiceId == Convert.ToInt64(ViewState["QS0"])
                                     where DBData.InvoiceId == Convert.ToInt64(InvoiceIDs[i])
                                     select DBData;
                        if (GetAtt.Count() > 0)
                        {
                            dbobj.AttachmentMasters.DeleteAllOnSubmit(GetAtt);
                            dbobj.SubmitChanges();
                            dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                        }

                        // Upload Invoice File

                        string lnkName = "lnkbtn" + InvoiceIDs[i];
                        LinkButton lnkBtn = (LinkButton)this.FindControl(lnkName);
                        if (lnkBtn != null)
                        {
                            if (lnkBtn.Visible)
                            {
                                if (lnkBtn.Text != "")
                                {
                                    string invoiceName = string.Empty;
                                    /////MemoryStream ms = Global.GetInvoice(Convert.ToInt64(Request.QueryString["i"]), "pdf", out invoiceName);
                                    MemoryStream ms = Global.GetInvoice(Convert.ToInt64(InvoiceIDs[i]), "pdf", out invoiceName);

                                    AttachmentMaster attachFile = new AttachmentMaster();
                                    attachFile.AttachmentName = invoiceName + ".pdf";
                                    /////attachFile.InvoiceId = Convert.ToInt64(Request.QueryString["i"]);
                                    attachFile.InvoiceId = Convert.ToInt64(InvoiceIDs[i]);
                                    attachFile.IsInvoice = true;
                                    attachFile.CreatedBy = Convert.ToInt64(Global.UserId);
                                    attachFile.CreatedDate = DateTime.Now;

                                    dbobj.AttachmentMasters.InsertOnSubmit(attachFile);
                                    dbobj.SubmitChanges();
                                    attachIDs = attachIDs != "" ? attachIDs + "," + attachFile.AttachmentId.ToString() : attachFile.AttachmentId.ToString();

                                    using (FileStream file = new FileStream(filePath + @"\" + attachFile.AttachmentId + ".pdf", FileMode.Create, FileAccess.Write))
                                    {
                                        ms.WriteTo(file);
                                    }
                                }
                            }
                        }
                    }
                }
                //upload Attachments-1
                if (fileUpload1.HasFile)
                {
                    AttachmentMaster attachFile1 = new AttachmentMaster();
                    attachFile1.AttachmentName = fileUpload1.FileName;
                    /////attachFile1.InvoiceId = Convert.ToInt64(Request.QueryString["i"]);
                    if (InvoiceIDs.Count() > 1)
                        attachFile1.InvoiceId = Convert.ToInt64(InvoiceIDs[0]);
                    else
                        attachFile1.InvoiceId = Convert.ToInt64(ViewState["QS0"]);
                    attachFile1.IsInvoice = false;
                    attachFile1.CreatedBy = Convert.ToInt64(Global.UserId);
                    attachFile1.CreatedDate = DateTime.Now;

                    dbobj.AttachmentMasters.InsertOnSubmit(attachFile1);
                    dbobj.SubmitChanges();
                    attachIDs = attachIDs != "" ? attachIDs + "," + attachFile1.AttachmentId.ToString() : attachFile1.AttachmentId.ToString();

                    fileUpload1.SaveAs(filePath + @"\" + attachFile1.AttachmentId.ToString() + Path.GetExtension(fileUpload1.FileName));
                }

                //upload Attachments-2
                if (fileUpload2.HasFile)
                {
                    AttachmentMaster attachFile2 = new AttachmentMaster();
                    attachFile2.AttachmentName = fileUpload2.FileName;
                    /////attachFile2.InvoiceId = Convert.ToInt64(Request.QueryString["i"]);
                    if (InvoiceIDs.Count() > 1)
                        attachFile2.InvoiceId = Convert.ToInt64(InvoiceIDs[0]);
                    else
                        attachFile2.InvoiceId = Convert.ToInt64(ViewState["QS0"]);
                    attachFile2.IsInvoice = false;
                    attachFile2.CreatedBy = Convert.ToInt64(Global.UserId);
                    attachFile2.CreatedDate = DateTime.Now;

                    dbobj.AttachmentMasters.InsertOnSubmit(attachFile2);
                    dbobj.SubmitChanges();
                    attachIDs = attachIDs != "" ? attachIDs + "," + attachFile2.AttachmentId.ToString() : attachFile2.AttachmentId.ToString();

                    fileUpload2.SaveAs(filePath + @"\" + attachFile2.AttachmentId.ToString() + Path.GetExtension(fileUpload2.FileName));
                }
                // New Added on 10-March-2023 By SP
                //upload Attachments-3
                if (fileUpload3.HasFile)
                {
                    AttachmentMaster attachFile3 = new AttachmentMaster();
                    attachFile3.AttachmentName = fileUpload3.FileName;
                    /////attachFile2.InvoiceId = Convert.ToInt64(Request.QueryString["i"]);
                    if (InvoiceIDs.Count() > 1)
                        attachFile3.InvoiceId = Convert.ToInt64(InvoiceIDs[0]);
                    else
                        attachFile3.InvoiceId = Convert.ToInt64(ViewState["QS0"]);
                    attachFile3.IsInvoice = false;
                    attachFile3.CreatedBy = Convert.ToInt64(Global.UserId);
                    attachFile3.CreatedDate = DateTime.Now;

                    dbobj.AttachmentMasters.InsertOnSubmit(attachFile3);
                    dbobj.SubmitChanges();
                    attachIDs = attachIDs != "" ? attachIDs + "," + attachFile3.AttachmentId.ToString() : attachFile3.AttachmentId.ToString();

                    fileUpload3.SaveAs(filePath + @"\" + attachFile3.AttachmentId.ToString() + Path.GetExtension(fileUpload3.FileName));
                }

                // New Added on 10-March-2023 By SP
                //upload Attachments-4
                if (fileUpload4.HasFile)
                {
                    AttachmentMaster attachFile4 = new AttachmentMaster();
                    attachFile4.AttachmentName = fileUpload4.FileName;
                    /////attachFile2.InvoiceId = Convert.ToInt64(Request.QueryString["i"]);
                    if (InvoiceIDs.Count() > 1)
                        attachFile4.InvoiceId = Convert.ToInt64(InvoiceIDs[0]);
                    else
                        attachFile4.InvoiceId = Convert.ToInt64(ViewState["QS0"]);
                    attachFile4.IsInvoice = false;
                    attachFile4.CreatedBy = Convert.ToInt64(Global.UserId);
                    attachFile4.CreatedDate = DateTime.Now;

                    dbobj.AttachmentMasters.InsertOnSubmit(attachFile4);
                    dbobj.SubmitChanges();
                    attachIDs = attachIDs != "" ? attachIDs + "," + attachFile4.AttachmentId.ToString() : attachFile4.AttachmentId.ToString();

                    fileUpload4.SaveAs(filePath + @"\" + attachFile4.AttachmentId.ToString() + Path.GetExtension(fileUpload4.FileName));
                }

                // New Added on 10-March-2023 By SP
                //upload Attachments-5
                if (fileUpload5.HasFile)
                {
                    AttachmentMaster attachFile5 = new AttachmentMaster();
                    attachFile5.AttachmentName = fileUpload5.FileName;
                    /////attachFile2.InvoiceId = Convert.ToInt64(Request.QueryString["i"]);
                    if (InvoiceIDs.Count() > 1)
                        attachFile5.InvoiceId = Convert.ToInt64(InvoiceIDs[0]);
                    else
                        attachFile5.InvoiceId = Convert.ToInt64(ViewState["QS0"]);
                    attachFile5.IsInvoice = false;
                    attachFile5.CreatedBy = Convert.ToInt64(Global.UserId);
                    attachFile5.CreatedDate = DateTime.Now;

                    dbobj.AttachmentMasters.InsertOnSubmit(attachFile5);
                    dbobj.SubmitChanges();
                    attachIDs = attachIDs != "" ? attachIDs + "," + attachFile5.AttachmentId.ToString() : attachFile5.AttachmentId.ToString();

                    fileUpload5.SaveAs(filePath + @"\" + attachFile5.AttachmentId.ToString() + Path.GetExtension(fileUpload5.FileName));
                }

                /////string status = Global.SendEmail(lblMailFrom.Text, txtTo.Text, txtCC.Text, txtBCC.Text, Convert.ToInt64(Request.QueryString["i"]), attachIDs, txtSubject.Text, editorEmail.Content);
                //***string status = Global.SendEmail(lblMailFrom.Text, txtTo.Text, txtCC.Text, txtBCC.Text, Convert.ToInt64(InvoiceIDs[i]), attachIDs, txtSubject.Text, editorEmail.Content);

                //string status = "";
                ////Live
                string status = Global.SendEmail(lblMailFrom.Text, txtTo.Text, txtCC.Text, txtBCC.Text, attachIDs, txtSubject.Text, editorEmail.Content);

                if (status == "Sucess")
                {
                    for (int i = 0; i < InvoiceIDs.Length; i++)
                    {
                        if (InvoiceIDs[i] != "")
                        {
                            SentEmail sentEmail = new SentEmail();
                            sentEmail.AttachIDs = attachIDs;
                            /////sentEmail.InvoiceID = Convert.ToInt64(Request.QueryString["i"]);
                            sentEmail.InvoiceID = Convert.ToInt64(InvoiceIDs[i]);
                            sentEmail.MailBCC = txtBCC.Text;
                            sentEmail.MailCC = txtCC.Text;
                            sentEmail.MailSentBy = Convert.ToInt64(Global.UserId);
                            sentEmail.MailSentDate = DateTime.Now;
                            sentEmail.MailSubject = txtSubject.Text;
                            sentEmail.MailTo = txtTo.Text;
                            sentEmail.MailBody = editorEmail.Content;

                            dbobj.SentEmails.InsertOnSubmit(sentEmail);
                            dbobj.SubmitChanges();

                            var GetInvData = from DBData in dbobj.InvoiceMasters
                                                 /////where DBData.InvoiceId == Convert.ToInt64(Request.QueryString["i"])
                                             where DBData.InvoiceId == Convert.ToInt64(InvoiceIDs[i])
                                             select DBData;
                            if (GetInvData.Count() > 0)
                            {
                                GetInvData.Single().InvoiceStatus = "Unpaid";
                                dbobj.SubmitChanges();
                            }

                            dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                            ClientScript.RegisterStartupScript(this.GetType(), "Test", "hideLayer();", true);
                            ScriptManager.RegisterStartupScript(page, page.GetType(), "Email Sent", "alert('Email has been sent');window.location.href='" + Global.PrevUrl + "';", true);
                        }
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Test", "hideLayer();", true);
                    ScriptManager.RegisterStartupScript(page, page.GetType(), "Email Sent", "alert('Unable to sent email: " + status + "');window.location.href='" + Global.PrevUrl + "';", true);
                }
            }
            catch (Exception ex)
            {
                //nothing
                ScriptManager.RegisterStartupScript(page, page.GetType(), "Email Error", "alert('Unable to sent Email: " + ex.Message + "');", true);
                ClientScript.RegisterStartupScript(this.GetType(), "Test", "hideLayer();", true);
            }
        }
        //Send Invoice for Outstanding invoice follow-up
        else
        {
            //IF ViewStat["QS1"]=="F"
            try
            {
                ControlCollection ctrls = divAttachments.Controls;
                foreach (Control ctrl in ctrls)
                {
                    if (ctrl.GetType() == typeof(HyperLink))
                    {
                        HyperLink hyp = ctrl as HyperLink;
                        if (hyp.Visible)
                        {
                            if (attachIDs == string.Empty)
                                attachIDs = hyp.ID.Replace("lnkbtn", string.Empty);
                            else
                                attachIDs = attachIDs + "," + hyp.ID.Replace("lnkbtn", string.Empty);
                        }
                    }
                }
                ////Live
                string status = Global.SendEmail(lblMailFrom.Text, txtTo.Text, txtCC.Text, txtBCC.Text, attachIDs, fileUpload1, fileUpload2, txtSubject.Text, editorEmail.Content);
                //string status = "";
                if (status == "Sucess")
                {
                    string[] spltStr = ViewState["QS0"].ToString().Split(',');
                    for (int i = 0; i < spltStr.Length; i++)
                    {
                        FollowUpMaster fm = new FollowUpMaster();
                        fm.InvoiceId = Convert.ToInt64(spltStr[i]);
                        fm.FollowUpDate = DateTime.Now;
                        fm.CreatedBy = Convert.ToInt64(Global.UserId);
                        fm.CreatedDate = DateTime.Now;

                        dbobj.FollowUpMasters.InsertOnSubmit(fm);
                        dbobj.SubmitChanges();
                    }

                    dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                    ClientScript.RegisterStartupScript(this.GetType(), "Test", "hideLayer();", true);
                    ScriptManager.RegisterStartupScript(page, page.GetType(), "Email Sent", "alert('Email has been sent');window.location.href='" + Global.PrevUrl + "';", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Test", "hideLayer();", true);
                    ScriptManager.RegisterStartupScript(page, page.GetType(), "Email Sent", "alert('Unable to sent email: " + status + "');window.location.href='" + Global.PrevUrl + "';", true);
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(page, page.GetType(), "Email Error", "alert('Unable to sent Email: " + ex.Message + "');", true);
                ClientScript.RegisterStartupScript(this.GetType(), "Test", "hideLayer();", true);
            }
        }
        ClientScript.RegisterStartupScript(this.GetType(), "Test", "hideLayer();", true);
    }

    //Remove Attachment for Send Invoice
    protected void imgbtnRemove_Click(object sender, ImageClickEventArgs e)
    {
        //lnkInvoice.Text = "";
        //lnkInvoice.Visible = false;
        //imgbtnRemove.Visible = false;

        ImageButton imgbtn = (ImageButton)sender;
        Int64 btnID = Convert.ToInt64(imgbtn.ID.Replace("imgbtn", string.Empty));
        string lnkName = "lnkbtn" + btnID;
        LinkButton linkButton = (LinkButton)this.FindControl(lnkName);
        if (linkButton != null)
        {
            linkButton.Visible = false;
            imgbtn.Visible = false;
            this.Controls.Remove(linkButton);
            //ViewState["QS0"] = ViewState["QS0"].ToString().Replace(btnID.ToString(), string.Empty);
            //if (ViewState["QS0"].ToString().IndexOf(",") == 0 || ViewState["QS0"].ToString().IndexOf(",") == ViewState["QS0"].ToString().Length - 1)
            //{
            //    ViewState["QS0"] = ViewState["QS0"].ToString().Replace(",", string.Empty);
            //}
        }
        //SetEmailData();
    }

    //Remove attachments for send followup email
    protected void imgbtnRemoveDynamic_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton imgbtn = (ImageButton)sender;
        Int64 btnID = Convert.ToInt64(imgbtn.ID.Replace("imgbtn", string.Empty));

        string hypName = "lnkbtn" + btnID;
        HyperLink hyperlnkBtn = (HyperLink)this.FindControl(hypName);
        if (hyperlnkBtn != null)
        {
            hyperlnkBtn.Visible = false;
            imgbtn.Visible = false;
            this.Controls.Remove(hyperlnkBtn);
            hfRemoveID.Value = hfRemoveID.Value == "" ? btnID.ToString() : hfRemoveID.Value + "," + btnID.ToString();
        }
        lnkInvoice.Text = "";
        lnkInvoice.Visible = false;
        //imgbtnRemove.Visible = false;
    }

    //Back to previous page
    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (Global.PrevUrl != null)
            Response.Redirect(Global.PrevUrl);
    }
}
