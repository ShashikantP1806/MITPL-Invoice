using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using System.Web.Services;

public partial class Home : System.Web.UI.Page
{
    MITInvoiceDataContext dbobj = new MITInvoiceDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
            if (this.Master.FindControl("mainmenu").Visible == false && Session["UserName"] != null)
            {
                Response.Redirect("Profile");
            }
            else
            {
                DisplayMessage();
                GetInvoice();
            }
            //lblUSD.Text = abc("USD") + " INR";
            //lblGBP.Text = abc("GBP") + " INR";
        }

        if (Global.UserType == "DIRECTOR" || Global.DepartmentName == "ACCT")
        {
            lblInvoice.Text = "Pending Invoice";
        }
    }

    protected void DisplayMessage()
    {
        try
        {
            lblUserName.Text = Global.TitleCase(Session["UserName"].ToString().Substring(0, Session["UserName"].ToString().LastIndexOf(" ")) + ",");
            if (DateTime.Now.TimeOfDay.Hours < 12)
                lblUserMsg.Text = "<p><B>Good Morning</B>,</p><br/>";
            else if (DateTime.Now.TimeOfDay.Hours >= 12 && DateTime.Now.TimeOfDay.Hours < 17)
                lblUserMsg.Text = "<p>Good Afternoon,</p><br/>";
            else
                lblUserMsg.Text = "<p>Good Evening,</p><br/>";
        }
        catch
        {
            // no action
        }
    }


    protected void GetInvoice()
    {
        //Output :  1-1-2019 to 31-1-2020 of below Example
        //DateTime dtPreviousDate = new DateTime(2020, 1, 1);
        //DateTime StartDate1 = new DateTime(dtPreviousDate.Year - 1, dtPreviousDate.Month == 1 ? dtPreviousDate.Month : dtPreviousDate.Month - 1, 1);
        //DateTime EndDate1 = new DateTime(dtPreviousDate.Year, dtPreviousDate.Month, DateTime.DaysInMonth(dtPreviousDate.Year, dtPreviousDate.Month));

        //// Last 1 Year
        //DateTime StartDate = new DateTime(DateTime.Now.Year - 1, DateTime.Now.Month == 1 ? DateTime.Now.Month : DateTime.Now.Month - 1, 1);
        //DateTime EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));


        //DateTime dt = new DateTime(2020, 6, 15);
        //DateTime dtPreviousDate = dt.AddMonths(-6);
        //// Last 6 month
        DateTime dtPreviousDate = DateTime.Now.AddMonths(-2); // 03-Feb-2021 update 2 month data instead of 6 month
        DateTime StartDate = new DateTime(dtPreviousDate.Year, dtPreviousDate.Month, 1);
        DateTime EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));



        if (Global.UserType == "DIRECTOR" || Global.DepartmentName == "ACCT")
        {


            #region Old Query wihout date filter , comment by Jignesh on 14-Oct-2020
            //&& DBData.InvoiceDate.Date >= StartDate.Date && DBData.InvoiceDate.Date <= EndDate.Date

            //var Invoice = from DBData in dbobj.InvoiceMasters
            //              where DBData.InvoiceStatus == "Unpaid"  && DBData.IsDeleted == false && ((DBData.ClientMaster.C_M1 == Global.UserM1 || DBData.ClientMaster.C_M2 == Global.UserM2))
            //              select new
            //              {
            //                  //RequestID=DBData.RequestID, RequestNumber=DBData.RequestNumber,RequestStatus=DBData.RequestStatus,AssignedTo=AssignData.AssignedTo};
            //                  InvoiceId = DBData.InvoiceId,
            //                  InvoiceNumber = DBData.InvoiceNumber,
            //              };
            #endregion

            var Invoice = from DBData in dbobj.InvoiceMasters
                          where DBData.InvoiceStatus == "Unpaid" && DBData.IsDeleted == false && ((DBData.ClientMaster.C_M1 == Global.UserM1 || DBData.ClientMaster.C_M2 == Global.UserM2)) && DBData.InvoiceDate.Date >= StartDate.Date && DBData.InvoiceDate.Date <= EndDate.Date
                          select new
                          {
                              //RequestID=DBData.RequestID, RequestNumber=DBData.RequestNumber,RequestStatus=DBData.RequestStatus,AssignedTo=AssignData.AssignedTo};
                              InvoiceId = DBData.InvoiceId,
                              InvoiceNumber = DBData.InvoiceNumber,
                          };

            //int iCount = Invoice.Count();
            if (Invoice.Count() > 0)
            {
                divNewRequest.InnerHtml = divNewRequest.InnerHtml + "<marquee  class=\"MarqueeClass\" behavior=\"alternate\" direction=\"up\" scrolldelay=225 width=200px height=300px onmouseover=\"this.stop();\" onmouseout=\"this.start()\">";

                string NewUser = "";// "style=\"color:Blue\"";

                foreach (var rData in Invoice)
                {
                    string Destination = "NewInvoice?InvID=" + Global.Encrypt(rData.InvoiceId.ToString());
                    divNewRequest.InnerHtml = divNewRequest.InnerHtml + "<li style=\"height:30px;text-align:left;margin-left:10px;list-style:none\"><a target=\"_blank\"" + NewUser + " href=" + Destination + ">" + rData.InvoiceNumber + "</a></li>";
                }
                divNewRequest.InnerHtml = divNewRequest.InnerHtml + "</marquee>";
            }
            else
            {
                divNewRequest.InnerHtml = divNewRequest.InnerHtml + "<center><li style=\"list-style:none\">No Invoices Found </li></center>";
            }
            //divNewRequest.InnerHtml = divNewRequest.InnerHtml + "</ul></center>";
            //divNewRequest.InnerHtml = divNewRequest.InnerHtml + "</center><table width=100% style=\"border-top-style:solid;border-top-width:thin; border-top-color:black\"><tr><td width=25px style=\"background:Green\"></td><td>Invoice</td></tr></table>";
        }
        else
        {
            var Invoice = from DBData in dbobj.InvoiceMasters
                              //where DBData.InvoiceStatus == "Draft" && (DBData.ClientMaster.DepartmentMaster.UserId == Convert.ToInt64(Global.UserId) || DBData.ClientMaster.DepartmentMaster.DepartmentId == Convert.ToInt64(Global.Department)) && DBData.IsDeleted == false
                              //(DBData.C_M1 == Global.UserM1 || DBData.C_M2 == Global.UserM2)
                          where DBData.InvoiceStatus == "Draft" && (DBData.ClientMaster.C_M1 == Global.UserM1 || DBData.ClientMaster.C_M2 == Global.UserM2) && (DBData.ClientMaster.DepartmentMaster.UserId == Convert.ToInt64(Global.UserId) || DBData.ClientMaster.DepartmentMaster.DepartmentId == Convert.ToInt64(Global.Department)) && DBData.IsDeleted == false
                          select new
                          {
                              //RequestID=DBData.RequestID, RequestNumber=DBData.RequestNumber,RequestStatus=DBData.RequestStatus,AssignedTo=AssignData.AssignedTo};
                              InvoiceId = DBData.InvoiceId,
                              InvoiceNumber = DBData.InvoiceNumber,
                          };
            if (Invoice.Count() > 0)
            {
                divNewRequest.InnerHtml = divNewRequest.InnerHtml + "<marquee  class=\"MarqueeClass\" behavior=\"alternate\" direction=\"up\" scrolldelay=225 width=200px height=300px onmouseover=\"this.stop();\" onmouseout=\"this.start()\">";

                string NewUser = "";// "style=\"color:Blue\"";

                foreach (var rData in Invoice)
                {
                    string Destination = "NewInvoice?InvID=" + Global.Encrypt(rData.InvoiceId.ToString());
                    divNewRequest.InnerHtml = divNewRequest.InnerHtml + "<li style=\"height:30px;text-align:left;margin-left:10px;list-style:none\"><a target=\"_blank\"" + NewUser + " href=" + Destination + ">" + rData.InvoiceNumber + "</a></li>";
                }
                divNewRequest.InnerHtml = divNewRequest.InnerHtml + "</marquee>";
            }
            else
            {
                divNewRequest.InnerHtml = divNewRequest.InnerHtml + "<center><li style=\"list-style:none\">No Invoices Found </li></center>";
            }
            //divNewRequest.InnerHtml = divNewRequest.InnerHtml + "</ul></center>";
            //divNewRequest.InnerHtml = divNewRequest.InnerHtml + "</center><table width=100% style=\"border-top-style:solid;border-top-width:thin; border-top-color:black\"><tr><td width=25px style=\"background:Green\"></td><td>Invoice</td></tr></table>";
        }
    }


    protected void GetInvoice_Existing()
    {
        if (Global.UserType == "DIRECTOR" || Global.DepartmentName == "ACCT")
        {
            var Invoice = from DBData in dbobj.InvoiceMasters
                          where DBData.InvoiceStatus == "Unpaid" && DBData.IsDeleted == false
                          select new
                          {
                              //RequestID=DBData.RequestID, RequestNumber=DBData.RequestNumber,RequestStatus=DBData.RequestStatus,AssignedTo=AssignData.AssignedTo};
                              InvoiceId = DBData.InvoiceId,
                              InvoiceNumber = DBData.InvoiceNumber,
                          };
            if (Invoice.Count() > 0)
            {
                divNewRequest.InnerHtml = divNewRequest.InnerHtml + "<marquee  class=\"MarqueeClass\" behavior=\"alternate\" direction=\"up\" scrolldelay=225 width=200px height=300px onmouseover=\"this.stop();\" onmouseout=\"this.start()\">";

                string NewUser = "";// "style=\"color:Blue\"";

                foreach (var rData in Invoice)
                {
                    string Destination = "NewInvoice?InvID=" + Global.Encrypt(rData.InvoiceId.ToString());
                    divNewRequest.InnerHtml = divNewRequest.InnerHtml + "<li style=\"height:30px;text-align:left;margin-left:10px;list-style:none\"><a target=\"_blank\"" + NewUser + " href=" + Destination + ">" + rData.InvoiceNumber + "</a></li>";
                }
                divNewRequest.InnerHtml = divNewRequest.InnerHtml + "</marquee>";
            }
            else
            {
                divNewRequest.InnerHtml = divNewRequest.InnerHtml + "<center><li style=\"list-style:none\">No Invoices Found </li></center>";
            }
            //divNewRequest.InnerHtml = divNewRequest.InnerHtml + "</ul></center>";
            //divNewRequest.InnerHtml = divNewRequest.InnerHtml + "</center><table width=100% style=\"border-top-style:solid;border-top-width:thin; border-top-color:black\"><tr><td width=25px style=\"background:Green\"></td><td>Invoice</td></tr></table>";
        }
        else
        {
            var Invoice = from DBData in dbobj.InvoiceMasters
                          where DBData.InvoiceStatus == "Draft" && (DBData.ClientMaster.DepartmentMaster.UserId == Convert.ToInt64(Global.UserId) || DBData.ClientMaster.DepartmentMaster.DepartmentId == Convert.ToInt64(Global.Department)) && DBData.IsDeleted == false
                          select new
                          {
                              //RequestID=DBData.RequestID, RequestNumber=DBData.RequestNumber,RequestStatus=DBData.RequestStatus,AssignedTo=AssignData.AssignedTo};
                              InvoiceId = DBData.InvoiceId,
                              InvoiceNumber = DBData.InvoiceNumber,
                          };
            if (Invoice.Count() > 0)
            {
                divNewRequest.InnerHtml = divNewRequest.InnerHtml + "<marquee  class=\"MarqueeClass\" behavior=\"alternate\" direction=\"up\" scrolldelay=225 width=200px height=300px onmouseover=\"this.stop();\" onmouseout=\"this.start()\">";

                string NewUser = "";// "style=\"color:Blue\"";

                foreach (var rData in Invoice)
                {
                    string Destination = "NewInvoice?InvID=" + Global.Encrypt(rData.InvoiceId.ToString());
                    divNewRequest.InnerHtml = divNewRequest.InnerHtml + "<li style=\"height:30px;text-align:left;margin-left:10px;list-style:none\"><a target=\"_blank\"" + NewUser + " href=" + Destination + ">" + rData.InvoiceNumber + "</a></li>";
                }
                divNewRequest.InnerHtml = divNewRequest.InnerHtml + "</marquee>";
            }
            else
            {
                divNewRequest.InnerHtml = divNewRequest.InnerHtml + "<center><li style=\"list-style:none\">No Invoices Found </li></center>";
            }
            //divNewRequest.InnerHtml = divNewRequest.InnerHtml + "</ul></center>";
            //divNewRequest.InnerHtml = divNewRequest.InnerHtml + "</center><table width=100% style=\"border-top-style:solid;border-top-width:thin; border-top-color:black\"><tr><td width=25px style=\"background:Green\"></td><td>Invoice</td></tr></table>";
        }
    }



    //protected void t1_Tick(object sender, EventArgs e)
    //{
    //    //decimal USD = ConvertCurrency(1, "USD", "INR");
    //    //lblUSD.Text = USD.ToString("0.00") + " INR";
    //    //decimal GBP = ConvertCurrency(1, "GBP", "INR");
    //    //lblGBP.Text = GBP.ToString("0.00") + " INR";
    //    lblUSD.Text = abc("USD") + " INR";
    //    lblGBP.Text = abc("GBP") + " INR";
    //}

    public static decimal ConvertCurrency(decimal amount, string fromCurrency, string toCurrency)
    {
        WebClient web = new WebClient();
        string url = string.Format("http://www.google.com/ig/calculator?hl=en&q={2}{0}%3D%3F{1}", fromCurrency.ToUpper(), toCurrency.ToUpper(), amount);
        string response = web.DownloadString(url);
        Regex regex = new Regex("rhs: \\\"(\\d*.\\d*)");
        Match match = regex.Match(response);
        decimal rate = System.Convert.ToDecimal(match.Groups[1].Value);
        return rate;
    }

    [WebMethod]
    public static string abc(string From)
    {
        WebRequest webrequest = WebRequest.Create("http://www.webservicex.net/CurrencyConvertor.asmx/ConversionRate?FromCurrency=" + From + "&ToCurrency=INR");
        HttpWebResponse response = (HttpWebResponse)webrequest.GetResponse();
        Stream dataStream = response.GetResponseStream();
        StreamReader reader = new StreamReader(dataStream);
        string responseFromServer = reader.ReadToEnd();
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(responseFromServer);
        string value = doc.InnerText;
        reader.Close();
        dataStream.Close();
        response.Close();
        return value;
    }

    [WebMethod(EnableSession = true)]
    public static string USD()
    {
        WebRequest webrequest = WebRequest.Create("http://www.webservicex.net/CurrencyConvertor.asmx/ConversionRate?FromCurrency=USD&ToCurrency=INR");
        HttpWebResponse response = (HttpWebResponse)webrequest.GetResponse();
        Stream dataStream = response.GetResponseStream();
        StreamReader reader = new StreamReader(dataStream);
        string responseFromServer = reader.ReadToEnd();
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(responseFromServer);
        string value = doc.InnerText;
        reader.Close();
        dataStream.Close();
        response.Close();
        return value + " INR";
        //string asdf = DateTime.Now.ToLongTimeString();
        //return asdf;
    }

    [WebMethod(EnableSession = true)]
    public static string GBP()
    {
        WebRequest webrequest = WebRequest.Create("http://www.webservicex.net/CurrencyConvertor.asmx/ConversionRate?FromCurrency=GBP&ToCurrency=INR");
        HttpWebResponse response = (HttpWebResponse)webrequest.GetResponse();
        Stream dataStream = response.GetResponseStream();
        StreamReader reader = new StreamReader(dataStream);
        string responseFromServer = reader.ReadToEnd();
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(responseFromServer);
        string value2 = doc.InnerText;
        reader.Close();
        dataStream.Close();
        response.Close();
        return value2 + " INR";
        //string mlk = DateTime.Now.AddHours(1).ToLongTimeString();
        //return mlk;
    }

}

