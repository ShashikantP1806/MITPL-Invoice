<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Web;
using System.IO;
using System.Linq;
public class Handler : IHttpHandler
{

    MITInvoiceDataContext dbobj = new MITInvoiceDataContext();
    public void ProcessRequest(HttpContext context)
    {
        if (context.Request.QueryString["i"] != null)
        {
            MemoryStream stm = new MemoryStream();
            int NextSRNo = 2;

            Aspose.Cells.License lic = new Aspose.Cells.License();
            string licPath = context.Server.MapPath("Bin");
            lic.SetLicense(licPath + @"\aspose.lic");

            string templatePath = context.Server.MapPath("InvoiceTemplate");


            var InvDetails = from DBData in dbobj.InvoiceMasters
                             where DBData.InvoiceId == Convert.ToInt64(context.Request.QueryString["i"])
                             select DBData;
            if (InvDetails.Count() > 0)
            {
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook(templatePath + @"\Invoice Template2.xls");
                Aspose.Cells.Worksheet ws = wb.Worksheets["Invoice"];

                ws.Cells[10, 11].Value = InvDetails.Single().InvoiceNumber;
                ws.Cells[11, 11].Value = InvDetails.Single().InvoiceDate.ToString("dd-MMM-yy");


                if (InvDetails.Single().PONumber != null)
                {
                    ws.Cells[26, 2].Value = InvDetails.Single().PONumber;
                    ws.Cells[26, 3].Value = InvDetails.Single().PODate;
                }

                //Add Client Details
                ws.Cells[26, 6].Value = (InvDetails.Single().ProjectFrom == 0 || InvDetails.Single().ProjectFrom == null) ? string.Empty : (from DBData in dbobj.ClientContactMasters
                                                                                               where DBData.ClientContactId == InvDetails.Single().ProjectFrom
                                                                                               select DBData.Name).Single();
                var ClientData = from DBData in dbobj.ClientMasters
                                 join
                                 CurrencyData in dbobj.CurrencyMasters
                                 on
                                 DBData.CurrencyId equals CurrencyData.CurrencyId
                                 join
                                 CityData in dbobj.CityMasters
                                 on
                                 DBData.City1 equals CityData.CityId
                                 join
                                 StateData in dbobj.StateMasters
                                 on
                                 DBData.State1 equals StateData.StateId
                                 join
                                 CountyData in dbobj.CountryMasters
                                 on
                                 DBData.Country1 equals CountyData.CountryId
                                 where DBData.ClientId == InvDetails.Single().ClientId
                                 &&
                                 !CurrencyData.IsDeleted
                                 select new
                                 {
                                     ClientName = DBData.ClientName,
                                     Address = DBData.Address1 + Environment.NewLine + CityData.CityName + ", " + StateData.StateName + " " + DBData.Zip_Postal1,
                                     Country = CountyData.CountryName,
                                     CurrencyID = DBData.CurrencyId,
                                     CurrencyName = CurrencyData.CurrencyName,
                                     CurrencyCode = CurrencyData.CurrencyCode,
                                     CurrencySymbol = CurrencyData.CurrencySymbol
                                 };
                ws.Cells[20, 2].Value = ClientData.Single().ClientName;
                ws.Cells[21, 2].Value = ClientData.Single().Address;
                ws.Cells[22, 2].Value = ClientData.Single().Country;


                /***********************************************************************/
                /*          Update Currency                                            */
                /***********************************************************************/
                Aspose.Cells.Range rngAmountCurr = ws.Cells.CreateRange(31, 12, 26, 1);
                Aspose.Cells.Style styleAmountCurr = ws.Cells[31, 12].GetStyle();
                styleAmountCurr.Number = 1;
                styleAmountCurr.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.00";
                rngAmountCurr.SetStyle(styleAmountCurr);

                Aspose.Cells.Range rngAmountCurr2 = ws.Cells.CreateRange(30, 12, 1, 1);
                Aspose.Cells.Style styleAmountCurr2 = ws.Cells[30, 12].GetStyle();
                styleAmountCurr2.Number = 1;
                styleAmountCurr2.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.00";
                rngAmountCurr2.SetStyle(styleAmountCurr2);

                Aspose.Cells.Range rngAmountCurr3 = ws.Cells.CreateRange(57, 12, 1, 1);
                Aspose.Cells.Style styleAmountCurr3 = ws.Cells[57, 12].GetStyle();
                styleAmountCurr3.Number = 1;
                styleAmountCurr3.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.00";
                rngAmountCurr3.SetStyle(styleAmountCurr3);


                Aspose.Cells.Range rngUnitPriceCurr = ws.Cells.CreateRange(31, 10, 26, 1);
                Aspose.Cells.Style styleUnitPriceCurr = ws.Cells[31, 10].GetStyle();
                styleUnitPriceCurr.Number = 1;
                styleUnitPriceCurr.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.00";
                rngUnitPriceCurr.SetStyle(styleUnitPriceCurr);

                Aspose.Cells.Range rngUnitPriceCurr2 = ws.Cells.CreateRange(30, 10, 1, 1);
                Aspose.Cells.Style styleUnitPriceCurr2 = ws.Cells[30, 10].GetStyle();
                styleUnitPriceCurr2.Number = 1;
                styleUnitPriceCurr2.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.00";
                rngUnitPriceCurr2.SetStyle(styleUnitPriceCurr2);

                Aspose.Cells.Range rngUnitPriceCurr3 = ws.Cells.CreateRange(57, 10, 1, 1);
                Aspose.Cells.Style styleUnitPriceCurr3 = ws.Cells[57, 10].GetStyle();
                styleUnitPriceCurr3.Number = 1;
                styleUnitPriceCurr3.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.00";
                rngUnitPriceCurr3.SetStyle(styleUnitPriceCurr3);


                Aspose.Cells.Range rngTotalCurr = ws.Cells.CreateRange(59, 12, 1, 1);
                Aspose.Cells.Style styleTotalCurr = ws.Cells[59, 12].GetStyle();
                styleTotalCurr.Number = 1;
                styleTotalCurr.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.00";
                rngTotalCurr.SetStyle(styleTotalCurr);


                Aspose.Cells.Range rngPayAmtCurr = ws.Cells.CreateRange(65, 12, 1, 1);
                Aspose.Cells.Style stylePayAmtCurr = ws.Cells[65, 12].GetStyle();
                stylePayAmtCurr.Number = 1;
                stylePayAmtCurr.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.00";
                rngPayAmtCurr.SetStyle(stylePayAmtCurr);

                /***********************************************************************/



                int RecordCnt = 0;

                var InvData = from DBData in dbobj.InvoiceDetailsMasters
                              where DBData.InvoiceId == Convert.ToInt64(context.Request.QueryString["i"])
                              select DBData;
                if (InvData.Count() > 0)
                {

                    foreach (var inv in InvData)
                    {
                        if (ws.Cells[30, 3].Value == null)
                        {
                            ws.Cells[30, 2].Value = 1;
                            ws.Cells[30, 3].Value = inv.ItemDesc;

                            if (!(inv.Qty == 0 || inv.Qty == null))
                            {
                                ws.Cells[30, 8].Value = inv.Qty.Value;
                                ws.Cells[30, 10].Value = inv.UnitPrice.Value;
                                ws.Cells[30, 11].Value = inv.PriceType;
                            }
                            ws.AutoFitRow(30);
                            RecordCnt++;
                        }
                        else
                        {
                            //Insert New Row
                            if (30 + RecordCnt > 57)
                            {
                                ws.Cells.InsertRow(30 + RecordCnt);
                                ws.Cells.InsertRow(30 + RecordCnt);

                                ws.Cells[30 + RecordCnt - 1, 2].SetStyle(ws.Cells[30, 2].GetStyle());
                                ws.Cells[30 + RecordCnt, 2].SetStyle(ws.Cells[30, 2].GetStyle());

                                Aspose.Cells.Range rngDesc = ws.Cells.CreateRange(30 + RecordCnt, 3, 1, 5);
                                if (!ws.Cells[30 + RecordCnt, 3].IsMerged)
                                    rngDesc.Merge();

                                rngDesc = ws.Cells.CreateRange(30 + RecordCnt + 1, 3, 1, 5);
                                if (!ws.Cells[31 + RecordCnt, 3].IsMerged)
                                    rngDesc.Merge();

                                Aspose.Cells.Style styleQty = ws.Cells[30, 8].GetStyle();
                                Aspose.Cells.Range rngQty = ws.Cells.CreateRange(30 + RecordCnt, 8, 1, 2);
                                if (!ws.Cells[30 + RecordCnt, 8].IsMerged)
                                    rngQty.Merge();
                                styleQty.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.None;
                                styleQty.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.None;
                                styleQty.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                                styleQty.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                                rngQty.SetStyle(styleQty);



                                rngQty = ws.Cells.CreateRange(30 + RecordCnt + 1, 8, 1, 2);
                                if (!ws.Cells[31 + RecordCnt, 8].IsMerged)
                                    rngQty.Merge();
                                rngQty.SetStyle(styleQty);

                                Aspose.Cells.Range rngTotalAmount = ws.Cells.CreateRange(30 + RecordCnt, 12, 1, 2);
                                if (!ws.Cells[30 + RecordCnt, 12].IsMerged)
                                    rngTotalAmount.Merge();
                                Aspose.Cells.Style styleTotalAmount = ws.Cells[30, 12].GetStyle();
                                styleTotalAmount.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.None;
                                styleTotalAmount.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.None;
                                styleTotalAmount.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                                styleTotalAmount.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                                styleTotalAmount.Number = 1;
                                styleTotalAmount.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.00";
                                rngTotalAmount.SetStyle(styleTotalAmount);


                                rngTotalAmount = ws.Cells.CreateRange(30 + RecordCnt + 1, 12, 1, 2);
                                if (!ws.Cells[31 + RecordCnt, 12].IsMerged)
                                    rngTotalAmount.Merge();
                                styleTotalAmount.Number = 1;
                                styleTotalAmount.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.00";
                                rngTotalAmount.SetStyle(styleTotalAmount);


                                Aspose.Cells.Style styleUnitPrice = ws.Cells[30, 10].GetStyle();
                                styleUnitPrice.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.None;
                                styleUnitPrice.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.None;
                                styleUnitPrice.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                                styleUnitPrice.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                                styleUnitPrice.Number = 1;
                                styleUnitPrice.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.00";


                                ws.Cells[30 + RecordCnt, 10].SetStyle(styleUnitPrice);
                                ws.Cells[31 + RecordCnt, 10].SetStyle(styleUnitPrice);
                            }
                            //ws.Cells[31 + RecordCnt, 2].Value = ((RecordCnt / 2) + 2).ToString();
                            ws.Cells[31 + RecordCnt, 3].Value = inv.ItemDesc;

                            if (!(inv.Qty == 0 || inv.Qty == null))
                            {
                                ws.Cells[31 + RecordCnt, 2].Value = NextSRNo;
                                ws.Cells[31 + RecordCnt, 8].Value = inv.Qty.Value;
                                ws.Cells[31 + RecordCnt, 10].Value = inv.UnitPrice.Value;
                                ws.Cells[31 + RecordCnt, 11].Value = inv.PriceType;
                                NextSRNo++;
                            }



                            //ws.Cells[31 + RecordCnt, 12].Formula = "=IF(I" + (32 + RecordCnt) + "=\"\",\"\",IF(K" + (32 + RecordCnt) + "=\"\",\"\",ROUND(I" + (32 + RecordCnt) + "*K" + (32 + RecordCnt) + ",2)))";


                            ws.AutoFitRow(30 + RecordCnt);
                            RecordCnt++;
                            RecordCnt++;
                        }
                    }
                }


                wb.CalculateFormula();

                wb.Save(stm, Aspose.Cells.SaveFormat.Pdf);
            }


            stm.Position = 0;
            context.Response.Clear();
            context.Response.ContentType = "application/pdf";
            context.Response.AddHeader("content-length", stm.Length.ToString());
            context.Response.BinaryWrite(stm.ToArray());
            context.Response.Flush();
            stm.Close();
            context.Response.End();
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

   
}