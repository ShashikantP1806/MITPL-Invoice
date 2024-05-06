using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Net.Mail;
using Aspose.Pdf.Text;
using System.Net;

public class Global
{
    UnicodeEncoding ByteConverter = new UnicodeEncoding();
    RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

    public Global()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    //Properties with Session used in application
    public static string UserId
    {
        get
        {
            return (string)HttpContext.Current.Session["UserId"];
        }
        set
        {
            HttpContext.Current.Session["UserId"] = value;
        }
    }

    public static string UserEmpCode
    {
        get
        {
            return (string)HttpContext.Current.Session["UserEmpCode"];
        }
        set
        {
            HttpContext.Current.Session["UserEmpCode"] = value;
        }
    }

    public static string UserProformaApproveAccess
    {
        get
        {
            return (string)HttpContext.Current.Session["UserProformaApproveAccess"];
        }
        set
        {
            HttpContext.Current.Session["UserProformaApproveAccess"] = value;
        }
    }

    public static string UserName
    {
        get
        {
            return (string)HttpContext.Current.Session["UserName"];
        }
        set
        {
            HttpContext.Current.Session["UserName"] = value;
        }
    }

    public static string Department
    {
        get
        {
            return (string)HttpContext.Current.Session["Department"];
        }
        set
        {
            HttpContext.Current.Session["Department"] = value;
        }
    }

    public static string DepartmentName
    {
        get
        {
            return (string)HttpContext.Current.Session["DepartmentName"];
        }
        set
        {
            HttpContext.Current.Session["DepartmentName"] = value;
        }
    }

    public static string UserType
    {
        get
        {
            return (string)HttpContext.Current.Session["UserType"];
        }
        set
        {
            HttpContext.Current.Session["UserType"] = value;
        }
    }

    public static Int64 InvoiceID
    {
        get
        {
            return (Int64)HttpContext.Current.Session["InvoiceID"];
        }
        set
        {
            HttpContext.Current.Session["InvoiceID"] = value;
        }
    }

    public static string InvoiceNo
    {
        get
        {
            return (string)HttpContext.Current.Session["InvoiceNo"];
        }
        set
        {
            HttpContext.Current.Session["InvoiceNo"] = value;
        }
    }

    public static string PrevUrl
    {
        get
        {
            return (string)HttpContext.Current.Session["PrevUrl"];
        }
        set
        {
            HttpContext.Current.Session["PrevUrl"] = value;
        }
    }


    //// Added by Jignesh on 24-Jul-2020

    public static bool UserM1
    {
        get
        {
            return (bool)HttpContext.Current.Session["UserM1"];
        }
        set
        {
            HttpContext.Current.Session["UserM1"] = value;
        }
    }

    public static bool UserM2
    {
        get
        {
            return (bool)HttpContext.Current.Session["UserM2"];
        }
        set
        {
            HttpContext.Current.Session["UserM2"] = value;
        }
    }

    public static bool UserIsSendEmail
    {
        get
        {
            return (bool)HttpContext.Current.Session["UserIsSendEmail"];
        }
        set
        {
            HttpContext.Current.Session["UserIsSendEmail"] = value;
        }
    }




    public static string sKey = "J1n@L";
    static byte[] bytes = ASCIIEncoding.ASCII.GetBytes("J1n@L");

    //Encode String
    public static string Encode(string value)
    {
        TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
        des.IV = new byte[8];
        PasswordDeriveBytes pdb = new PasswordDeriveBytes(sKey, new byte[-1 + 1]);
        des.Key = pdb.CryptDeriveKey("RC2", "MD5", 128, new byte[8]);
        System.IO.MemoryStream ms = new System.IO.MemoryStream(value.Length);
        System.Security.Cryptography.CryptoStream encStream = new System.Security.Cryptography.CryptoStream(ms, des.CreateEncryptor(), System.Security.Cryptography.CryptoStreamMode.Write);
        byte[] plainBytes = System.Text.Encoding.UTF8.GetBytes(value);
        encStream.Write(plainBytes, 0, plainBytes.Length);
        encStream.FlushFinalBlock();
        byte[] encryptedBytes = new byte[Convert.ToInt32(ms.Length - 1) + 1];
        ms.Position = 0;
        ms.Read(encryptedBytes, 0, Convert.ToInt32(ms.Length));
        encStream.Close();
        return Convert.ToBase64String(encryptedBytes);
    }

    //Decode String
    public static string Decode(string value)
    {
        System.Security.Cryptography.TripleDESCryptoServiceProvider des = new System.Security.Cryptography.TripleDESCryptoServiceProvider();
        des.IV = new byte[8];
        System.Security.Cryptography.PasswordDeriveBytes pdb = new System.Security.Cryptography.PasswordDeriveBytes(sKey, new byte[-1 + 1]);
        des.Key = pdb.CryptDeriveKey("RC2", "MD5", 128, new byte[8]);
        byte[] encryptedBytes = Convert.FromBase64String(value);
        System.IO.MemoryStream ms = new System.IO.MemoryStream(value.Length);
        System.Security.Cryptography.CryptoStream decStream = new System.Security.Cryptography.CryptoStream(ms, des.CreateDecryptor(), System.Security.Cryptography.CryptoStreamMode.Write);
        decStream.Write(encryptedBytes, 0, encryptedBytes.Length);
        decStream.FlushFinalBlock();
        byte[] plainBytes = new byte[Convert.ToInt32(ms.Length - 1) + 1];
        ms.Position = 0;
        ms.Read(plainBytes, 0, Convert.ToInt32(ms.Length));
        decStream.Close();
        return System.Text.Encoding.UTF8.GetString(plainBytes);
    }

    //Convert string to Binary Data
    public static byte[] StringToBinary(string str)
    {
        System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
        return encoding.GetBytes(str);
    }

    //Convert Binary Data to String
    
    
    private static byte[] key = { };

    private static byte[] IV = { 38, 55, 206, 48, 28, 64, 20, 16 };

    private static string stringKey = "!5663a#KN=";

    public static string EncryptMITPLLogin(string text)
    {

        try
        {

            key = Encoding.UTF8.GetBytes(stringKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            Byte[] byteArray = Encoding.UTF8.GetBytes(text);
            MemoryStream memoryStream = new MemoryStream();

            CryptoStream cryptoStream = new CryptoStream(memoryStream,

                des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
            cryptoStream.Write(byteArray, 0, byteArray.Length);
            cryptoStream.FlushFinalBlock();
            return Convert.ToBase64String(memoryStream.ToArray());
        }

        catch (Exception ex)
        {
            // Handle Exception Here
        }
        return string.Empty;
    }

    public static string DecryptMITPLLogin(string text)
    {

        try
        {
            key = Encoding.UTF8.GetBytes(stringKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            Byte[] byteArray = Convert.FromBase64String(text);
            MemoryStream memoryStream = new MemoryStream();

            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
            cryptoStream.Write(byteArray, 0, byteArray.Length);
            cryptoStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }
        catch (Exception ex)
        {
            // Handle Exception Here
        }
        return string.Empty;
    }

    public static string Encrypt(string plainText)
    {
        byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

        byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
        var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
        var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

        byte[] cipherTextBytes;

        using (var memoryStream = new MemoryStream())
        {
            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            {
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                cryptoStream.FlushFinalBlock();
                cipherTextBytes = memoryStream.ToArray();
                cryptoStream.Close();
            }
            memoryStream.Close();
        }
        return Convert.ToBase64String(cipherTextBytes);
    }


    #region =============================================================================================================================================
    static readonly string PasswordHash = "P@@Sw0rd";
    static readonly string SaltKey = "S@LT&KEY";
    static readonly string VIKey = "@1B2c3D4e5F6g7H8";

    public static string BinaryToString(byte[] stringbyte)
    {
        System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
        return encoding.GetString(stringbyte);
    }    
    public static string Decrypt(string encryptedText)
    {
        byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
        byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
        var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };
    
        var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
        var memoryStream = new MemoryStream(cipherTextBytes);
        var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        byte[] plainTextBytes = new byte[cipherTextBytes.Length];
    
        int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
        memoryStream.Close();
        cryptoStream.Close();
        return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
    }

    private static void getSMTPClient(out string MailStatus, out SmtpClient sc)
    {
        TechReportingDataContext dbtech = new TechReportingDataContext();
        var EmailConfig = (from dbEmail in dbtech.MITPLSiteEmailConfigurations
                           where dbEmail.SiteName == "Invoice"
                           select dbEmail).Single();

        MailStatus = "";
        sc = new SmtpClient();
        sc.Port = Convert.ToInt32(EmailConfig.Port);
        sc.Host = EmailConfig.Host;
        sc.UseDefaultCredentials = true;
        string HostPassword = Global.Decrypt(Global.BinaryToString(EmailConfig.HostPassword.ToArray()));
        sc.Credentials = new System.Net.NetworkCredential(EmailConfig.HostUserName, HostPassword);
        sc.EnableSsl = true;
        ServicePointManager.SecurityProtocol = (SecurityProtocolType)48 | (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
    }
    #endregion =========================================================================================================================================
    //Convert HexaString to Unicode String
    public static string HexaToUniCode(string HexaString)
    {
        string uniCodeString = string.Empty;

        uint utf32 = uint.Parse(HexaString, System.Globalization.NumberStyles.HexNumber);
        string s = Encoding.UTF32.GetString(BitConverter.GetBytes(utf32));
        //foreach (char c in s.ToCharArray())
        //{
        //    uniCodeString+= (uint)c);
        //}
        return s;
    }

    //Convert Number to Words
    static String NumToWords(double number, string CurrencyName)
    {
        string words = "";
        double intPart;
        double decPart = 0;
        if (number == 0)
            return "ZERO " + CurrencyName.ToUpper() + " ONLY";
        try
        {
            string[] splitter = number.ToString("0.00").Split('.');
            intPart = double.Parse(splitter[0]);
            decPart = double.Parse(splitter[1]);
        }
        catch
        {
            intPart = number;
        }

        words = NumWords(intPart);

        //if (decPart > 0)
        //{
        //    if (words != "")
        //        words += " and ";
        //    int counter = decPart.ToString().Length;
        //    switch (counter)
        //    {
        //        case 1: words += NumWords(decPart) + " tenths"; break;
        //        case 2: words += NumWords(decPart) + " hundredths"; break;
        //        case 3: words += NumWords(decPart) + " thousandths"; break;
        //        case 4: words += NumWords(decPart) + " ten-thousandths"; break;
        //        case 5: words += NumWords(decPart) + " hundred-thousandths"; break;
        //        case 6: words += NumWords(decPart) + " millionths"; break;
        //        case 7: words += NumWords(decPart) + " ten-millionths"; break;
        //    }
        //}
        return CurrencyName.ToUpper() + " " + words.ToUpper() + (decPart == 0 ? string.Empty : " & " + decPart.ToString() + @"/100") + " ONLY";
    }

    static String NumWords(double number) //converts double to words
    {
        string[] numbersArr = new string[] { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
        string[] tensArr = new string[] { "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninty" };
        string[] suffixesArr = new string[] { "thousand", "million", "billion", "trillion", "quadrillion", "quintillion", "sextillion", "septillion", "octillion", "nonillion", "decillion", "undecillion", "duodecillion", "tredecillion", "Quattuordecillion", "Quindecillion", "Sexdecillion", "Septdecillion", "Octodecillion", "Novemdecillion", "Vigintillion" };
        string words = "";

        bool tens = false;

        if (number < 0)
        {
            words += "negative ";
            number *= -1;
        }

        int power = (suffixesArr.Length + 1) * 3;

        while (power > 3)
        {
            double pow = Math.Pow(10, power);
            if (number > pow)
            {
                if (number % Math.Pow(10, power) > 0)
                {
                    words += NumWords(Math.Floor(number / pow)) + " " + suffixesArr[(power / 3) - 1] + " ";
                }
                else if (number % pow > 0)
                {
                    words += NumWords(Math.Floor(number / pow)) + " " + suffixesArr[(power / 3) - 1];
                }
                number %= pow;
            }
            power -= 3;
        }
        if (number >= 1000)
        {
            if (number % 1000 > 0) words += NumWords(Math.Floor(number / 1000)) + " thousand ";
            else words += NumWords(Math.Floor(number / 1000)) + " thousand";
            number %= 1000;
        }
        if (0 <= number && number <= 999)
        {
            if ((int)number / 100 > 0)
            {
                words += NumWords(Math.Floor(number / 100)) + " hundred";
                number %= 100;
            }
            if ((int)number / 10 > 1)
            {
                if (words != "")
                    words += " ";
                words += tensArr[(int)number / 10 - 2];
                tens = true;
                number %= 10;
            }

            if (number < 20 && number != 0)
            {
                if (words != "" && tens == false)
                    words += " ";
                words += (tens ? " " + numbersArr[(int)number - 1] : numbersArr[(int)number - 1]);
                number -= Math.Floor(number);
            }
        }

        return words.ToUpper();

    }

    //Generate Invoice from Datbase Entry and save it into Memory stream
    public static MemoryStream GetInvoice(Int64 InvoiceID, string fileType, out string InvoiceNumber)
    {
        MITInvoiceDataContext dbobj = new MITInvoiceDataContext();
        MemoryStream stm = new MemoryStream();
        InvoiceNumber = string.Empty;
        if (InvoiceID > 0)
        {

            int NextSRNo = 2;

            int NumToTextRow = 61;

            Aspose.Cells.License lic = new Aspose.Cells.License();
            string licPath = HttpContext.Current.Server.MapPath("Bin");
            lic.SetLicense(licPath + @"\aspose.lic");

            Aspose.Pdf.License licPDF = new Aspose.Pdf.License();
            licPDF.SetLicense(licPath + @"\asposePDF.lic");

            string templatePath = HttpContext.Current.Server.MapPath("InvoiceTemplate");


            var InvDetails = from DBData in dbobj.InvoiceMasters
                             where DBData.InvoiceId == InvoiceID
                             select DBData;

            if (InvDetails.Count() > 0)
            {
                string dbInvNo = InvDetails.Single().InvoiceNumber;
                char[] invLastVal = dbInvNo.ToCharArray();

                if (char.IsLetter(invLastVal[invLastVal.Length - 1]))
                    InvoiceNumber = InvDetails.Single().InvoiceNumber.Substring(0, InvDetails.Single().InvoiceNumber.Length - 1).Replace(@"/", "_");
                else
                    InvoiceNumber = InvDetails.Single().InvoiceNumber.Replace(@"/", "_");

                Aspose.Cells.Workbook wb = null;

                switch (InvDetails.Single().InvoiceFor.ToLower())
                {
                    case "india":
                        wb = new Aspose.Cells.Workbook(templatePath + @"\Invoice Template1.xls");
                        break;
                    case "usa":
                        wb = new Aspose.Cells.Workbook(templatePath + @"\Invoice Template2.xls");
                        break;
                }


                Aspose.Cells.Worksheet ws = wb.Worksheets["Invoice"];

                ws.PageSetup.PrintTitleRows = "$7:$29";



                if (char.IsLetter(invLastVal[invLastVal.Length - 1]))
                {
                    ws.Cells[10, 11].Value = dbInvNo.Substring(0, dbInvNo.Length - 1) + " Revised";
                }
                else
                    ws.Cells[10, 11].Value = InvDetails.Single().InvoiceNumber;// +(InvDetails.Single().Revision == null ? string.Empty : InvDetails.Single().Revision);
                ws.Cells[11, 11].Value = InvDetails.Single().InvoiceDate.ToString("dd-MMM-yy");


                if (InvDetails.Single().PONumber != null)
                {
                   
                    ws.Cells[26, 2].Value = InvDetails.Single().PONumber;
                    if (InvDetails.Single().PODate != null)
                    {
                        DateTime dtPODate = (DateTime)InvDetails.Single().PODate;
                        ws.Cells[26, 5].Value = dtPODate.ToString("dd-MMM-yy");
                    }
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
                                     Address = DBData.Address1,// + Environment.NewLine + CityData.CityName + ", " + StateData.StateName + " " + DBData.Zip_Postal1,
                                     City = CityData.CityName,
                                     State = StateData.StateName,
                                     PostalCode = DBData.Zip_Postal1,
                                     Country = CountyData.CountryName,
                                     CurrencyID = DBData.CurrencyId,
                                     CurrencyName = CurrencyData.CurrencyName,
                                     CurrencyCode = CurrencyData.CurrencyCode,
                                     CurrencySymbol = CurrencyData.CurrencySymbol,
                                     ClientGSTIN = DBData.GSTIN.Trim()
                                 };
                ws.Cells[20, 2].Value = ClientData.Single().ClientName;
                ws.Cells[21, 2].Value = ClientData.Single().Address;
                ws.Cells[22, 2].Value = ClientData.Single().City != ClientData.Single().State ? ClientData.Single().City + ", " + ClientData.Single().State + " - " + ClientData.Single().PostalCode + " " + ClientData.Single().Country : ClientData.Single().State + " - " + ClientData.Single().PostalCode + " " + ClientData.Single().Country;
                if (ClientData.Single().Country.ToLower() == "india" && InvDetails.Single().InvoiceFor.ToLower() == "india")
                {
                    if (ClientData.Single().ClientGSTIN.Trim() != null || ClientData.Single().ClientGSTIN.Trim() != "")
                        ws.Cells[23, 2].Value = "GSTIN: " + ClientData.Single().ClientGSTIN;
                }



                /***********************************************************************/
                /*          Update Currency                                            */
                /***********************************************************************/
                /***********************************************************************/
                /*  Updated all aspose style currency with comma in amount format on 26-Oct-2021 by JM  */
                /***********************************************************************/
                Aspose.Cells.Range rngAmountCurr = ws.Cells.CreateRange(31, 12, 26, 1);
                Aspose.Cells.Style styleAmountCurr = ws.Cells[31, 12].GetStyle();
                styleAmountCurr.Number = 1;
                //styleAmountCurr.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.00";
                styleAmountCurr.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] #,#.00";
                rngAmountCurr.SetStyle(styleAmountCurr);

                Aspose.Cells.Range rngAmountCurr2 = ws.Cells.CreateRange(30, 12, 1, 1);
                Aspose.Cells.Style styleAmountCurr2 = ws.Cells[30, 12].GetStyle();
                styleAmountCurr2.Number = 1;
                //styleAmountCurr2.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.00";
                styleAmountCurr2.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] #,#.00";
                rngAmountCurr2.SetStyle(styleAmountCurr2);

                Aspose.Cells.Range rngAmountCurr3 = ws.Cells.CreateRange(57, 12, 1, 1);
                Aspose.Cells.Style styleAmountCurr3 = ws.Cells[57, 12].GetStyle();
                styleAmountCurr3.Number = 1;
                //styleAmountCurr3.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.00";
                styleAmountCurr3.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] #,#.00";
                rngAmountCurr3.SetStyle(styleAmountCurr3);


                Aspose.Cells.Range rngUnitPriceCurr = ws.Cells.CreateRange(31, 10, 26, 1);
                Aspose.Cells.Style styleUnitPriceCurr = ws.Cells[31, 10].GetStyle();
                styleUnitPriceCurr.Number = 1;
                //styleUnitPriceCurr.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.0000";
                styleUnitPriceCurr.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] #,#.0000";
                rngUnitPriceCurr.SetStyle(styleUnitPriceCurr);

                Aspose.Cells.Range rngUnitPriceCurr2 = ws.Cells.CreateRange(30, 10, 1, 1);
                Aspose.Cells.Style styleUnitPriceCurr2 = ws.Cells[30, 10].GetStyle();
                styleUnitPriceCurr2.Number = 1;
                styleUnitPriceCurr2.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] #,#.0000";
                //styleUnitPriceCurr2.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.0000";
                rngUnitPriceCurr2.SetStyle(styleUnitPriceCurr2);

                Aspose.Cells.Range rngUnitPriceCurr3 = ws.Cells.CreateRange(57, 10, 1, 1);
                Aspose.Cells.Style styleUnitPriceCurr3 = ws.Cells[57, 10].GetStyle();
                styleUnitPriceCurr3.Number = 1;
                //styleUnitPriceCurr3.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.0000";
                styleUnitPriceCurr3.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] #,#.0000";
                rngUnitPriceCurr3.SetStyle(styleUnitPriceCurr3);


                Aspose.Cells.Range rngTotalCurr = ws.Cells.CreateRange(59, 12, 1, 1);
                Aspose.Cells.Style styleTotalCurr = ws.Cells[59, 12].GetStyle();
                styleTotalCurr.Number = 1;
                //styleTotalCurr.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.00";
                styleTotalCurr.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] #,#.00";
                rngTotalCurr.SetStyle(styleTotalCurr);


                Aspose.Cells.Range rngPayAmtCurr = ws.Cells.CreateRange(65, 12, 1, 1);
                Aspose.Cells.Style stylePayAmtCurr = ws.Cells[65, 12].GetStyle();
                stylePayAmtCurr.Number = 1;

                //stylePayAmtCurr.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.00";
                stylePayAmtCurr.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] #,#.00";
                rngPayAmtCurr.SetStyle(stylePayAmtCurr);


                ws.Cells[65, 9].Value = ws.Cells[65, 9].Value.ToString().Replace("$", HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")));

                /***********************************************************************/



                int RecordCnt = 0;

                var InvData = from DBData in dbobj.InvoiceDetailsMasters
                              where DBData.InvoiceId == InvoiceID
                              select DBData;
                if (InvData.Count() > 0)
                {

                    foreach (var inv in InvData)
                    {
                        /***********************************************************************/
                        /*  Updated all Unit Price currency with 2 decimal point amount format on 
                            25-Apr-2022 by JM  */
                        /***********************************************************************/
                        Aspose.Cells.Range rngUnitPrice = ws.Cells.CreateRange(30, 10, 1, 1);
                        Aspose.Cells.Style styleUPUnitPrice = ws.Cells[30, 10].GetStyle();
                        if (inv.UserMaster.U_M1 == true)
                        {
                            styleUPUnitPrice.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] #,#.0000";
                        }
                        else
                        {
                            //styleUnitPrice.Custom = "#,#.00"; //// Updated by Jignesh on 25-Apr-2022
                            styleUPUnitPrice.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] #,#.00";
                        }
                        /****************************** End ************************************/

                        if (ws.Cells[30, 3].Value == null)
                        {
                            ws.Cells[30, 2].Value = 1;
                            ws.Cells[30, 3].Value = inv.ItemDesc;

                            if (!(inv.Qty == 0 || inv.Qty == null))
                            {
                                Aspose.Cells.Range rngQty = ws.Cells.CreateRange(30, 8, 1, 1);
                                Aspose.Cells.Style styleQty = ws.Cells[30, 8].GetStyle();
                                styleQty.Number = 1;
                                if (inv.PriceType == "Document" || inv.PriceType == "Image")
                                    styleQty.Custom = "0";
                                else
                                    styleQty.Custom = "#,#.000"; //// Updated by Jignesh on 08-Nov-2021     
                                rngQty.SetStyle(styleQty);



                                rngUnitPrice.SetStyle(styleUPUnitPrice);


                                ws.Cells[30, 8].Value = inv.Qty.Value;
                                ws.Cells[30, 10].Value = inv.UnitPrice.Value;
                                ws.Cells[30, 11].Value = inv.PriceType;
                            }

                            Aspose.Cells.Style desStyle = ws.Cells[30, 3].GetStyle();
                            desStyle.IsTextWrapped = true;
                            ws.Cells[30, 3].SetStyle(desStyle);

                            ws.Cells[30, 25].Value = inv.ItemDesc;

                            ws.AutoFitRow(30);

                            Aspose.Cells.Range rngHeightF = ws.Cells.CreateRange(30, 25, 1, 1);
                            double rowHeightF = rngHeightF.RowHeight;
                            ws.Cells[30, 25].Value = string.Empty;
                            ws.Cells.CreateRange(30, 3, 1, 1).RowHeight = rowHeightF;

                            RecordCnt++;
                        }
                        else
                        {
                            //Insert New Row
                            if ((30 + RecordCnt) >= 56)
                            {
                                ws.Cells.InsertRow(30 + RecordCnt);
                                ws.AutoFitRow(30 + RecordCnt);
                                ws.Cells.InsertRow(30 + RecordCnt);
                                ws.AutoFitRow(30 + RecordCnt);

                                NumToTextRow = NumToTextRow + 2;

                                ws.Cells[30 + RecordCnt - 1, 2].SetStyle(ws.Cells[30 + RecordCnt - 2, 2].GetStyle());
                                ws.Cells[30 + RecordCnt, 2].SetStyle(ws.Cells[30 + RecordCnt - 2, 2].GetStyle());

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

                                ws.Cells[31 + RecordCnt, 12].Formula = "=IF(I" + (31 + RecordCnt + 1) + "=\"\",\"\",IF(K" + (31 + RecordCnt + 1) + "=\"\",\"\",ROUND(I" + (31 + RecordCnt + 1) + "*K" + (31 + RecordCnt + 1) + ",2)))";
                                Aspose.Cells.Range rngTotalAmount = ws.Cells.CreateRange(30 + RecordCnt, 12, 1, 2);
                                if (!ws.Cells[30 + RecordCnt, 12].IsMerged)
                                    rngTotalAmount.Merge();
                                Aspose.Cells.Style styleTotalAmount = ws.Cells[30, 12].GetStyle();
                                styleTotalAmount.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.None;
                                styleTotalAmount.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.None;
                                styleTotalAmount.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                                styleTotalAmount.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                                styleTotalAmount.Number = 1;
                                //styleTotalAmount.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.00";
                                styleTotalAmount.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] #,#.00";
                                rngTotalAmount.SetStyle(styleTotalAmount);


                                rngTotalAmount = ws.Cells.CreateRange(30 + RecordCnt + 1, 12, 1, 2);
                                if (!ws.Cells[31 + RecordCnt, 12].IsMerged)
                                    rngTotalAmount.Merge();
                                styleTotalAmount.Number = 1;
                                //styleTotalAmount.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.00";
                                styleTotalAmount.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] #,#.00";
                                rngTotalAmount.SetStyle(styleTotalAmount);


                                Aspose.Cells.Style styleUnitPrice = ws.Cells[30, 10].GetStyle();
                                styleUnitPrice.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.None;
                                styleUnitPrice.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.None;
                                styleUnitPrice.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                                styleUnitPrice.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                                styleUnitPrice.Number = 1;
                                //styleUnitPrice.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.0000";

                                /***********************************************************************/
                                /*  Updated all Unit Price currency with 2 decimal point amount format on 
                                    25-Apr-2022 by JM  */
                                /***********************************************************************/
                                if (inv.UserMaster.U_M1 == true)
                                    styleUnitPrice.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] #,#.0000";
                                else
                                    styleUnitPrice.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] #,#.00";
                                /****************************** End ************************************/

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

                                Aspose.Cells.Range rngQty = ws.Cells.CreateRange(31 + RecordCnt, 8, 1, 1);
                                Aspose.Cells.Style styleQty = ws.Cells[31 + RecordCnt, 8].GetStyle();
                                styleQty.Number = 1;
                                if (inv.PriceType == "Document" || inv.PriceType == "Image")
                                    styleQty.Custom = "0";
                                else
                                    styleQty.Custom = "#,#.000"; //// Updated by Jignesh on 08-Nov-2021 
                                                                 //styleQty.Custom = "0.000";

                                rngQty.SetStyle(styleQty);

                                /***********************************************************************/
                                /*  Updated all Unit Price currency with 2 decimal point amount format on 
                                    25-Apr-2022 by JM  */
                                /***********************************************************************/

                                Aspose.Cells.Range rngUPrice = ws.Cells.CreateRange(31 + RecordCnt, 10, 1, 1);
                                Aspose.Cells.Style styleUPrice = ws.Cells[31 + RecordCnt, 10].GetStyle();
                                if (inv.UserMaster.U_M1 == true)
                                    styleUPrice.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] #,#.0000";
                                else
                                    styleUPrice.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] #,#.00";

                                rngUPrice.SetStyle(styleUPrice);
                                /****************************** End ************************************/

                                NextSRNo++;
                            }


                            ws.Cells[31 + RecordCnt, 25].Value = inv.ItemDesc;
                            ws.Cells[31 + RecordCnt, 25].GetStyle().IsTextWrapped = true;
                            //ws.Cells[31 + RecordCnt, 12].Formula = "=IF(I" + (32 + RecordCnt) + "=\"\",\"\",IF(K" + (32 + RecordCnt) + "=\"\",\"\",ROUND(I" + (32 + RecordCnt) + "*K" + (32 + RecordCnt) + ",2)))";
                            Aspose.Cells.Style desStyle = ws.Cells[30 + RecordCnt, 3].GetStyle();
                            desStyle.IsTextWrapped = true;
                            ws.Cells[30 + RecordCnt, 3].SetStyle(desStyle);

                            ws.AutoFitRow(31 + RecordCnt);

                            Aspose.Cells.Range rngHeight = ws.Cells.CreateRange(31 + RecordCnt, 25, 1, 1);
                            double rowHeight = rngHeight.RowHeight;
                            ws.Cells[31 + RecordCnt, 25].Value = string.Empty;
                            ws.Cells.CreateRange(31 + RecordCnt, 3, 1, 1).RowHeight = rowHeight;
                            RecordCnt++;
                            RecordCnt++;
                        }
                    }
                }
                wb.CalculateFormula();
                ws.Cells[NumToTextRow, 10].Value = NumToWords(Convert.ToDouble(ws.Cells[NumToTextRow - 2, 12].Value), ClientData.Single().CurrencyCode);
                //Save Files for Export TO XLS/PDF & View Invoice into Stream
                switch (fileType)
                {
                    case "xls":
                        wb.Save(stm, Aspose.Cells.SaveFormat.Excel97To2003);
                        break;
                    case "xlsx":
                        wb.Save(stm, Aspose.Cells.SaveFormat.Xlsx);
                        break;
                    case "pdf":
                        wb.Save(stm, Aspose.Cells.SaveFormat.Pdf);

                        ///*********************************************************************************************/
                        ///*          Code updated by Mayur Mehta on 06/05/2014 to add PageNumber in PDF               */
                        ///*********************************************************************************************/
                        Aspose.Pdf.Document pdfDoc = new Aspose.Pdf.Document(stm);

                        for (int pdfPg = 1; pdfPg <= pdfDoc.Pages.Count; pdfPg++)
                        {
                            //Aspose.Pdf.Text.TextFragment tf = new Aspose.Pdf.Text.TextFragment("Continue on page " + (pdfPg + 1));
                            Aspose.Pdf.Text.TextFragment tf = new Aspose.Pdf.Text.TextFragment("Page " + pdfPg + " of " + pdfDoc.Pages.Count);
                            //tf.Position = new Aspose.Pdf.Text.Position(510, 12);
                            tf.Position = new Aspose.Pdf.Text.Position(365, 700);
                            tf.TextState.FontSize = 8;
                            tf.TextState.Font = FontRepository.FindFont("Verdana");
                            tf.TextState.FontStyle = Aspose.Pdf.Text.FontStyles.Bold;

                            Aspose.Pdf.Text.TextBuilder tb = new Aspose.Pdf.Text.TextBuilder(pdfDoc.Pages[pdfPg]);
                            tb.AppendText(tf);
                        }

                        pdfDoc.Save();
                        pdfDoc.Dispose();

                        break;
                }

            }
        }
        return stm;
    }

    public static MemoryStream GetProformaInvoice(Int64 InvoiceID, string fileType, out string InvoiceNumber)
    {
        MITInvoiceDataContext dbobj = new MITInvoiceDataContext();
        MemoryStream stm = new MemoryStream();
        InvoiceNumber = string.Empty;
        if (InvoiceID > 0)
        {

            int NextSRNo = 2;

            int NumToTextRow = 61;

            Aspose.Cells.License lic = new Aspose.Cells.License();
            string licPath = HttpContext.Current.Server.MapPath("Bin");
            lic.SetLicense(licPath + @"\aspose.lic");

            Aspose.Pdf.License licPDF = new Aspose.Pdf.License();
            licPDF.SetLicense(licPath + @"\asposePDF.lic");

            string templatePath = HttpContext.Current.Server.MapPath("InvoiceTemplate");


            var InvDetails = from DBData in dbobj.ProformaInvoiceMasters
                             where DBData.ProInvoiceId == InvoiceID
                             select DBData;

            if (InvDetails.Count() > 0)
            {
                string dbInvNo = InvDetails.Single().InvoiceNumber;
                char[] invLastVal = dbInvNo.ToCharArray();

                if (char.IsLetter(invLastVal[invLastVal.Length - 1]))
                    InvoiceNumber = InvDetails.Single().InvoiceNumber.Substring(0, InvDetails.Single().InvoiceNumber.Length - 1).Replace(@"/", "_");
                else
                    InvoiceNumber = InvDetails.Single().InvoiceNumber.Replace(@"/", "_");

                Aspose.Cells.Workbook wb = null;

                switch (InvDetails.Single().InvoiceFor.ToLower())
                {
                    case "india":
                        wb = new Aspose.Cells.Workbook(templatePath + @"\Invoice Template1.xls");
                        break;
                    case "usa":
                        wb = new Aspose.Cells.Workbook(templatePath + @"\Invoice Template2.xls");
                        break;
                }


                Aspose.Cells.Worksheet ws = wb.Worksheets["Invoice"];

                ws.PageSetup.PrintTitleRows = "$7:$29";



                if (char.IsLetter(invLastVal[invLastVal.Length - 1]))
                {
                    ws.Cells[10, 11].Value = dbInvNo.Substring(0, dbInvNo.Length - 1) + " Revised";
                }
                else
                    ws.Cells[10, 11].Value = InvDetails.Single().InvoiceNumber;// +(InvDetails.Single().Revision == null ? string.Empty : InvDetails.Single().Revision);
                ws.Cells[11, 11].Value = InvDetails.Single().InvoiceDate.ToString("dd-MMM-yy");


                if (InvDetails.Single().PONumber != null)
                {
                    ws.Cells[26, 2].Value = InvDetails.Single().PONumber;
                    //ws.Cells[26, 5].Value = InvDetails.Single().PODate;
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
                                     Address = DBData.Address1,// + Environment.NewLine + CityData.CityName + ", " + StateData.StateName + " " + DBData.Zip_Postal1,
                                     City = CityData.CityName,
                                     State = StateData.StateName,
                                     PostalCode = DBData.Zip_Postal1,
                                     Country = CountyData.CountryName,
                                     CurrencyID = DBData.CurrencyId,
                                     CurrencyName = CurrencyData.CurrencyName,
                                     CurrencyCode = CurrencyData.CurrencyCode,
                                     CurrencySymbol = CurrencyData.CurrencySymbol
                                 };
                ws.Cells[20, 2].Value = ClientData.Single().ClientName;
                ws.Cells[21, 2].Value = ClientData.Single().Address;
                ws.Cells[22, 2].Value = ClientData.Single().City != ClientData.Single().State ? ClientData.Single().City + ", " + ClientData.Single().State + " - " + ClientData.Single().PostalCode + " " + ClientData.Single().Country : ClientData.Single().State + " - " + ClientData.Single().PostalCode + " " + ClientData.Single().Country;



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
                styleUnitPriceCurr.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.0000";
                rngUnitPriceCurr.SetStyle(styleUnitPriceCurr);

                Aspose.Cells.Range rngUnitPriceCurr2 = ws.Cells.CreateRange(30, 10, 1, 1);
                Aspose.Cells.Style styleUnitPriceCurr2 = ws.Cells[30, 10].GetStyle();
                styleUnitPriceCurr2.Number = 1;
                styleUnitPriceCurr2.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.0000";
                rngUnitPriceCurr2.SetStyle(styleUnitPriceCurr2);

                Aspose.Cells.Range rngUnitPriceCurr3 = ws.Cells.CreateRange(57, 10, 1, 1);
                Aspose.Cells.Style styleUnitPriceCurr3 = ws.Cells[57, 10].GetStyle();
                styleUnitPriceCurr3.Number = 1;
                styleUnitPriceCurr3.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.0000";
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


                ws.Cells[65, 9].Value = ws.Cells[65, 9].Value.ToString().Replace("$", HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")));

                /***********************************************************************/

                int RecordCnt = 0;

                var InvData = from DBData in dbobj.ProformaInvoiceDetailsMasters
                              where DBData.ProInvoiceId == InvoiceID
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
                                Aspose.Cells.Range rngQty = ws.Cells.CreateRange(30, 8, 1, 1);
                                Aspose.Cells.Style styleQty = ws.Cells[30, 8].GetStyle();
                                styleQty.Number = 1;
                                if (inv.PriceType == "Document" || inv.PriceType == "Image")
                                    styleQty.Custom = "0";
                                else
                                    styleQty.Custom = "0.000";

                                rngQty.SetStyle(styleQty);

                                ws.Cells[30, 8].Value = inv.Qty.Value;
                                ws.Cells[30, 10].Value = inv.UnitPrice.Value;
                                ws.Cells[30, 11].Value = inv.PriceType;
                            }

                            Aspose.Cells.Style desStyle = ws.Cells[30, 3].GetStyle();
                            desStyle.IsTextWrapped = true;
                            ws.Cells[30, 3].SetStyle(desStyle);

                            ws.Cells[30, 25].Value = inv.ItemDesc;

                            ws.AutoFitRow(30);

                            Aspose.Cells.Range rngHeightF = ws.Cells.CreateRange(30, 25, 1, 1);
                            double rowHeightF = rngHeightF.RowHeight;
                            ws.Cells[30, 25].Value = string.Empty;
                            ws.Cells.CreateRange(30, 3, 1, 1).RowHeight = rowHeightF;

                            RecordCnt++;
                        }
                        else
                        {
                            //Insert New Row
                            if ((30 + RecordCnt) >= 56)
                            {
                                ws.Cells.InsertRow(30 + RecordCnt);
                                ws.AutoFitRow(30 + RecordCnt);
                                ws.Cells.InsertRow(30 + RecordCnt);
                                ws.AutoFitRow(30 + RecordCnt);

                                NumToTextRow = NumToTextRow + 2;

                                ws.Cells[30 + RecordCnt - 1, 2].SetStyle(ws.Cells[30 + RecordCnt - 2, 2].GetStyle());
                                ws.Cells[30 + RecordCnt, 2].SetStyle(ws.Cells[30 + RecordCnt - 2, 2].GetStyle());

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

                                ws.Cells[31 + RecordCnt, 12].Formula = "=IF(I" + (31 + RecordCnt + 1) + "=\"\",\"\",IF(K" + (31 + RecordCnt + 1) + "=\"\",\"\",ROUND(I" + (31 + RecordCnt + 1) + "*K" + (31 + RecordCnt + 1) + ",2)))";
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
                                styleUnitPrice.Custom = "[$" + HttpUtility.HtmlDecode("&#" + (ClientData.Single().CurrencySymbol + ";").Replace(";;", ";")) + "] 0.0000";


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

                                Aspose.Cells.Range rngQty = ws.Cells.CreateRange(31 + RecordCnt, 8, 1, 1);
                                Aspose.Cells.Style styleQty = ws.Cells[31 + RecordCnt, 8].GetStyle();
                                styleQty.Number = 1;
                                if (inv.PriceType == "Document" || inv.PriceType == "Image")
                                    styleQty.Custom = "0";
                                else
                                    styleQty.Custom = "0.000";

                                rngQty.SetStyle(styleQty);

                                NextSRNo++;
                            }


                            ws.Cells[31 + RecordCnt, 25].Value = inv.ItemDesc;
                            ws.Cells[31 + RecordCnt, 25].GetStyle().IsTextWrapped = true;
                            //ws.Cells[31 + RecordCnt, 12].Formula = "=IF(I" + (32 + RecordCnt) + "=\"\",\"\",IF(K" + (32 + RecordCnt) + "=\"\",\"\",ROUND(I" + (32 + RecordCnt) + "*K" + (32 + RecordCnt) + ",2)))";
                            Aspose.Cells.Style desStyle = ws.Cells[30 + RecordCnt, 3].GetStyle();
                            desStyle.IsTextWrapped = true;
                            ws.Cells[30 + RecordCnt, 3].SetStyle(desStyle);

                            ws.AutoFitRow(31 + RecordCnt);

                            Aspose.Cells.Range rngHeight = ws.Cells.CreateRange(31 + RecordCnt, 25, 1, 1);
                            double rowHeight = rngHeight.RowHeight;
                            ws.Cells[31 + RecordCnt, 25].Value = string.Empty;
                            ws.Cells.CreateRange(31 + RecordCnt, 3, 1, 1).RowHeight = rowHeight;
                            RecordCnt++;
                            RecordCnt++;
                        }
                    }
                }
                wb.CalculateFormula();
                ws.Cells[NumToTextRow, 10].Value = NumToWords(Convert.ToDouble(ws.Cells[NumToTextRow - 2, 12].Value), ClientData.Single().CurrencyCode);

                //Save Files for Export TO XLS/PDF & View Invoice into Stream
                switch (fileType)
                {
                    case "xls":
                        wb.Save(stm, Aspose.Cells.SaveFormat.Excel97To2003);
                        break;
                    case "xlsx":
                        wb.Save(stm, Aspose.Cells.SaveFormat.Xlsx);
                        break;
                    case "pdf":
                        wb.Save(stm, Aspose.Cells.SaveFormat.Pdf);

                        ///*********************************************************************************************/
                        ///*          Code updated by Mayur Mehta on 06/05/2014 to add PageNumber in PDF               */
                        ///*********************************************************************************************/
                        Aspose.Pdf.Document pdfDoc = new Aspose.Pdf.Document(stm);

                        for (int pdfPg = 1; pdfPg <= pdfDoc.Pages.Count; pdfPg++)
                        {
                            Aspose.Pdf.Text.TextFragment tf = new Aspose.Pdf.Text.TextFragment("Page " + pdfPg + " of " + pdfDoc.Pages.Count);
                            tf.Position = new Aspose.Pdf.Text.Position(365, 700);
                            tf.TextState.FontSize = 8;
                            tf.TextState.Font = FontRepository.FindFont("Verdana");
                            tf.TextState.FontStyle = Aspose.Pdf.Text.FontStyles.Bold;

                            Aspose.Pdf.Text.TextBuilder tb = new Aspose.Pdf.Text.TextBuilder(pdfDoc.Pages[pdfPg]);
                            tb.AppendText(tf);
                        }

                        pdfDoc.Save();
                        pdfDoc.Dispose();

                        break;
                }

            }
        }
        return stm;
    }    

    public static string SendEmail(string EmailSender, string Recipient, string CC, string BCC, string attachmentIds, string EmailSubject, string EmailBody)
    {
        MITInvoiceDataContext dbobj = new MITInvoiceDataContext();
        Recipient = Recipient.Replace(", ", ",").Replace("; ", ";").Replace(",,", ",").Replace(";;", ";").Replace(",;", ",").Replace(";", ",");
        CC = CC.Replace(", ", ",").Replace("; ", ";").Replace(",,", ",").Replace(";;", ";").Replace(",;", ",").Replace(";", ",");
        BCC = BCC.Replace(", ", ",").Replace("; ", ";").Replace(",,", ",").Replace(";;", ";").Replace(",;", ",").Replace(";", ",");
        
        string MailStatus;
        SmtpClient sc;
        getSMTPClient(out MailStatus, out sc);

        try
        {
            MailMessage mail = new MailMessage();
            mail.Subject = EmailSubject;
            mail.IsBodyHtml = true;
            mail.Body = EmailBody;
            mail.From = new MailAddress(EmailSender);

            string[] recpStr = Recipient.Replace(", ", ",").Replace(",,", ",").Split(',');
            for (int i = 0; i < recpStr.Length; i++)
            {
                if (recpStr[i] != "")
                    mail.To.Add(new MailAddress(recpStr[i]));
            }
            if (CC != "")
            {
                string[] ccStr = CC.Replace(", ", ",").Replace(",,", ",").Split(',');
                for (int i = 0; i < ccStr.Length; i++)
                {
                    if (ccStr[i] != "")
                        mail.CC.Add(new MailAddress(ccStr[i]));
                }
            }

            if (BCC != "")
            {
                string[] bccStr = BCC.Replace(", ", ",").Replace(",,", ",").Split(',');
                for (int i = 0; i < bccStr.Length; i++)
                {
                    if (bccStr[i] != "")
                        mail.Bcc.Add(new MailAddress(bccStr[i]));
                }
            }
            string attchFilePath = HttpContext.Current.Server.MapPath("UploadedFiles");
            if (attachmentIds != string.Empty)
            {
                string[] spltStr = attachmentIds.Split(',');
                for (int i = 0; i < spltStr.Length; i++)
                {
                    Int64 FileID = Convert.ToInt64(spltStr[i]);
                    var fileDetail = (from DBData in dbobj.AttachmentMasters
                                      where DBData.AttachmentId == FileID
                                      select new
                                      {
                                          FileName = DBData.AttachmentName
                                      }).Single();
                    FileStream fs = new FileStream((attchFilePath + @"\" + FileID + Path.GetExtension(fileDetail.FileName)), FileMode.Open);
                    Attachment attchFile = new Attachment(fs, fileDetail.FileName);
                    mail.Attachments.Add(attchFile);
                }
            }

            //ServicePointManager.SecurityProtocol = (SecurityProtocolType)48 | (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
            sc.Send(mail);
            MailStatus = "Sucess";
        }
        catch (Exception ex)
        {

            //MailStatus = ex.Message;

            MailStatus = ex.Message;
            MailMessage mail = new MailMessage();
            mail.Subject = "Unable to send email notification";
            mail.IsBodyHtml = true;
            mail.Body = "Unable to send email notification for " + EmailSubject + " to " + Recipient + "<br/> Error: " + ex.Message;
            mail.From = new MailAddress("corp@mangalaminfotech.com");
            mail.To.Add(new MailAddress("system@mangalaminfotech.net"));
            //ServicePointManager.SecurityProtocol = (SecurityProtocolType)48 | (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
            sc.Send(mail);
        }

        return MailStatus;
    }    

    //Send Email
    public static string SendEmail(string EmailSender, string Recipient, string CC, string BCC, Int64 InvoiceID, string attachmentIds, string EmailSubject, string EmailBody)
    {
        MITInvoiceDataContext dbobj = new MITInvoiceDataContext();

        
        Recipient = Recipient.Replace(", ", ",").Replace("; ", ";").Replace(",,", ",").Replace(";;", ";").Replace(",;", ",").Replace(";", ",");
        CC = CC.Replace(", ", ",").Replace("; ", ";").Replace(",,", ",").Replace(";;", ";").Replace(",;", ",").Replace(";", ",");
        BCC = BCC.Replace(", ", ",").Replace("; ", ";").Replace(",,", ",").Replace(";;", ";").Replace(",;", ",").Replace(";", ",");

        string MailStatus;
        SmtpClient sc;
        getSMTPClient(out MailStatus, out sc);

        try
        {
            MailMessage mail = new MailMessage();
            mail.Subject = EmailSubject;
            mail.IsBodyHtml = true;
            mail.Body = EmailBody;
            mail.From = new MailAddress(EmailSender);

            string[] recpStr = Recipient.Replace(", ", ",").Replace(",,", ",").Split(',');
            for (int i = 0; i < recpStr.Length; i++)
            {
                if (recpStr[i] != "")
                    mail.To.Add(new MailAddress(recpStr[i]));
            }
            if (CC != "")
            {
                string[] ccStr = CC.Replace(", ", ",").Replace(",,", ",").Split(',');
                for (int i = 0; i < ccStr.Length; i++)
                {
                    if (ccStr[i] != "")
                        mail.CC.Add(new MailAddress(ccStr[i]));
                }
            }

            if (BCC != "")
            {
                string[] bccStr = BCC.Replace(", ", ",").Replace(",,", ",").Split(',');
                for (int i = 0; i < bccStr.Length; i++)
                {
                    if (bccStr[i] != "")
                        mail.Bcc.Add(new MailAddress(bccStr[i]));
                }
            }            
            string attchFilePath = HttpContext.Current.Server.MapPath("UploadedFiles");
            if (attachmentIds != string.Empty)
            {
                string[] spltStr = attachmentIds.Split(',');
                for (int i = 0; i < spltStr.Length; i++)
                {
                    Int64 FileID = Convert.ToInt64(spltStr[i]);
                    var fileDetail = (from DBData in dbobj.AttachmentMasters
                                      where DBData.AttachmentId == FileID
                                      select new
                                      {
                                          FileName = DBData.AttachmentName
                                      }).Single();
                    FileStream fs = new FileStream((attchFilePath + @"\" + FileID + Path.GetExtension(fileDetail.FileName)), FileMode.Open);
                    Attachment attchFile = new Attachment(fs, fileDetail.FileName);
                    mail.Attachments.Add(attchFile);
                }
            }

            //ServicePointManager.SecurityProtocol = (SecurityProtocolType)48 | (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
            sc.Send(mail);
            MailStatus = "Sucess";
        }
        catch (Exception ex)
        {
            MailStatus = ex.Message;
            MailMessage mail = new MailMessage();
            mail.Subject = "Unable to send email notification";
            mail.IsBodyHtml = true;
            mail.Body = "Unable to send email notification for " + EmailSubject + " to " + Recipient + "<br/> Error: " + ex.Message;
            mail.From = new MailAddress("corp@mangalaminfotech.com");
            mail.To.Add(new MailAddress("system@mangalaminfotech.net"));
            //ServicePointManager.SecurityProtocol = (SecurityProtocolType)48 | (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
            sc.Send(mail);
        }

        return MailStatus;
    }

    //Send Email
    public static string SendEmail(string EmailSender, string Recipient, string CC, string BCC, string attachmentIds, System.Web.UI.WebControls.FileUpload fu1, System.Web.UI.WebControls.FileUpload fu2, string EmailSubject, string EmailBody)
    {
        MITInvoiceDataContext dbobj = new MITInvoiceDataContext();
        
        Recipient = Recipient.Replace(", ", ",").Replace("; ", ";").Replace(",,", ",").Replace(";;", ";").Replace(",;", ",").Replace(";", ",");
        CC = CC.Replace(", ", ",").Replace("; ", ";").Replace(",,", ",").Replace(";;", ";").Replace(",;", ",").Replace(";", ",");
        BCC = BCC.Replace(", ", ",").Replace("; ", ";").Replace(",,", ",").Replace(";;", ";").Replace(",;", ",").Replace(";", ",");

        string MailStatus;
        SmtpClient sc;
        getSMTPClient(out MailStatus, out sc);

        try
        {
            MailMessage mail = new MailMessage();
            mail.Subject = EmailSubject;
            mail.IsBodyHtml = true;
            mail.Body = EmailBody;
            mail.From = new MailAddress(EmailSender);

            string[] recpStr = Recipient.Replace(", ", ",").Replace(",,", ",").Split(',');
            for (int i = 0; i < recpStr.Length; i++)
            {
                if (recpStr[i] != "")
                    mail.To.Add(new MailAddress(recpStr[i]));
            }
            if (CC != "")
            {
                string[] ccStr = CC.Replace(", ", ",").Replace(",,", ",").Split(',');
                for (int i = 0; i < ccStr.Length; i++)
                {
                    if (ccStr[i] != null)
                        mail.CC.Add(new MailAddress(ccStr[i]));
                }
            }

            if (BCC != "")
            {
                string[] bccStr = BCC.Replace(", ", ",").Replace(",,", ",").Split(',');
                for (int i = 0; i < bccStr.Length; i++)
                {
                    if (bccStr[i] != null)
                        mail.Bcc.Add(new MailAddress(bccStr[i]));
                }
            }

            string attchFilePath = HttpContext.Current.Server.MapPath("UploadedFiles");
            if (attachmentIds != string.Empty)
            {
                string[] spltStr = attachmentIds.Split(',');
                for (int i = 0; i < spltStr.Length; i++)
                {
                    Int64 FileID = Convert.ToInt64(spltStr[i]);
                    var fileDetail = (from DBData in dbobj.AttachmentMasters
                                      where DBData.AttachmentId == FileID
                                      select new
                                      {
                                          FileName = DBData.AttachmentName
                                      }).Single();
                    FileStream fs = new FileStream((attchFilePath + @"\" + FileID + Path.GetExtension(fileDetail.FileName)), FileMode.Open);
                    Attachment attchFile = new Attachment(fs, fileDetail.FileName);
                    mail.Attachments.Add(attchFile);
                }
            }
            if (fu1 != null)
            {
                if (fu1.HasFile)
                {
                    MemoryStream ms = new MemoryStream(fu1.FileBytes);
                    Attachment attchFile = new Attachment(ms, fu1.FileName);
                    mail.Attachments.Add(attchFile);
                }
            }

            if (fu2 != null)
            {
                if (fu2.HasFile)
                {
                    MemoryStream ms = new MemoryStream(fu2.FileBytes);
                    Attachment attchFile = new Attachment(ms, fu2.FileName);
                    mail.Attachments.Add(attchFile);
                }
            }

            //ServicePointManager.SecurityProtocol = (SecurityProtocolType)48 | (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
            sc.Send(mail);
            MailStatus = "Sucess";
        }
        catch (Exception ex)
        {
            MailStatus = ex.Message;
            MailMessage mail = new MailMessage();
            mail.Subject = "Unable to send email notification";
            mail.IsBodyHtml = true;
            mail.Body = "Unable to send email notification for " + EmailSubject + " to " + Recipient + "<br/> Error: " + ex.Message;
            mail.From = new MailAddress("corp@mangalaminfotech.com");
            mail.To.Add(new MailAddress("system@mangalaminfotech.net"));
            //ServicePointManager.SecurityProtocol = (SecurityProtocolType)48 | (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
            sc.Send(mail);
        }

        return MailStatus;
    }

    //Send Email
    public static string SendEmail(string EmailSender, string Recipient, string CC, string EmailSubject, string EmailBody)
    {
        MITInvoiceDataContext dbobj = new MITInvoiceDataContext();
        
        Recipient = Recipient.Replace(", ", ",").Replace("; ", ";").Replace(",,", ",").Replace(";;", ";").Replace(",;", ",").Replace(";", ",");
        CC = CC.Replace(", ", ",").Replace("; ", ";").Replace(",,", ",").Replace(";;", ";").Replace(",;", ",").Replace(";", ",");

        string MailStatus;
        SmtpClient sc;
        getSMTPClient(out MailStatus, out sc);

        try
        {
            MailMessage mail = new MailMessage();
            mail.Subject = EmailSubject;
            mail.IsBodyHtml = true;
            mail.Body = EmailBody;
            mail.From = new MailAddress(EmailSender);

            string[] recpStr = Recipient.Replace(", ", ",").Replace(",,", ",").Split(',');
            for (int i = 0; i < recpStr.Length; i++)
            {
                mail.To.Add(new MailAddress(recpStr[i]));
            }

            string[] ccStr = CC.Replace(", ", ",").Replace(",,", ",").Split(',');
            for (int i = 0; i < ccStr.Length; i++)
            {
                mail.CC.Add(new MailAddress(ccStr[i]));
            }

            //ServicePointManager.SecurityProtocol = (SecurityProtocolType)48 | (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
            sc.Send(mail);
            MailStatus = "Sucess";
        }
        catch (Exception ex)
        {
            MailStatus = ex.Message;
            MailMessage mail = new MailMessage();
            mail.Subject = "Unable to send email notification";
            mail.IsBodyHtml = true;
            mail.Body = "Unable to send email notification for " + EmailSubject + " to " + Recipient + "<br/> Error: " + ex.Message;
            mail.From = new MailAddress("corp@mangalaminfotech.com");
            mail.To.Add(new MailAddress("system@mangalaminfotech.net"));
            //ServicePointManager.SecurityProtocol = (SecurityProtocolType)48 | (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
            sc.Send(mail);
        }

        return MailStatus;
    }

    //Send Email
    public static string SendEmail(string EmailSender, string Recipient, string EmailSubject, string EmailBody)
    {
        MITInvoiceDataContext dbobj = new MITInvoiceDataContext();
        
        Recipient = Recipient.Replace(", ", ",").Replace("; ", ";").Replace(",,", ",").Replace(";;", ";").Replace(",;", ",").Replace(";", ",");

        string MailStatus;
        SmtpClient sc;
        getSMTPClient(out MailStatus, out sc);

        try
        {
            MailMessage mail = new MailMessage();
            mail.Subject = EmailSubject;
            mail.IsBodyHtml = true;
            mail.Body = EmailBody;
            mail.From = new MailAddress(EmailSender);

            string[] recpStr = Recipient.Replace(", ", ",").Replace(",,", ",").Split(',');
            for (int i = 0; i < recpStr.Length; i++)
            {
                mail.To.Add(new MailAddress(recpStr[i]));
            }

            //ServicePointManager.SecurityProtocol = (SecurityProtocolType)48 | (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
            sc.Send(mail);
            MailStatus = "Sucess";
        }
        catch (Exception ex)
        {
            MailStatus = ex.Message;
            MailMessage mail = new MailMessage();
            mail.Subject = "Unable to send email notification";
            mail.IsBodyHtml = true;
            mail.Body = "Unable to send email notification for " + EmailSubject + " to " + Recipient + "<br/> Error: " + ex.Message;
            mail.From = new MailAddress("corp@mangalaminfotech.com");
            mail.To.Add(new MailAddress("system@mangalaminfotech.net"));
            //ServicePointManager.SecurityProtocol = (SecurityProtocolType)48 | (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
            sc.Send(mail);
        }

        return MailStatus;
    }

    //Send Email for test not used in code
    public static string SendEmailTest(string EmailSender, string Recipient, string CC, string BCC, string EmailSubject, string EmailBody)
    {
        MITInvoiceDataContext dbobj = new MITInvoiceDataContext();
        
        Recipient = Recipient.Replace(", ", ",").Replace("; ", ";").Replace(",,", ",").Replace(";;", ";").Replace(",;", ",").Replace(";", ",");
        CC = CC.Replace(", ", ",").Replace("; ", ";").Replace(",,", ",").Replace(";;", ";").Replace(",;", ",").Replace(";", ",");
        BCC = BCC.Replace(", ", ",").Replace("; ", ";").Replace(",,", ",").Replace(";;", ";").Replace(",;", ",").Replace(";", ",");

        string MailStatus;
        SmtpClient sc;
        getSMTPClient(out MailStatus, out sc);

        try
        {
            MailMessage mail = new MailMessage();
            mail.Subject = EmailSubject;
            mail.IsBodyHtml = true;
            mail.Body = EmailBody;
            mail.From = new MailAddress(EmailSender);

            string[] recpStr = Recipient.Replace(", ", ",").Replace(",,", ",").Split(',');
            for (int i = 0; i < recpStr.Length; i++)
            {
                if (recpStr[i] != "")
                    mail.To.Add(new MailAddress(recpStr[i]));
            }
            if (CC != "")
            {
                string[] ccStr = CC.Replace(", ", ",").Replace(",,", ",").Split(',');
                for (int i = 0; i < ccStr.Length; i++)
                {
                    if (ccStr[i] != null)
                        mail.CC.Add(new MailAddress(ccStr[i]));
                }
            }

            if (BCC != "")
            {
                string[] bccStr = BCC.Replace(", ", ",").Replace(",,", ",").Split(',');
                for (int i = 0; i < bccStr.Length; i++)
                {
                    if (bccStr[i] != null)
                        mail.Bcc.Add(new MailAddress(bccStr[i]));
                }
            }
            //ServicePointManager.SecurityProtocol = (SecurityProtocolType)48 | (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
            sc.Send(mail);
            MailStatus = "Sucess";
        }
        catch (Exception ex)
        {
            MailStatus = ex.Message;
            MailMessage mail = new MailMessage();
            mail.Subject = "Unable to send email notification(Testing)";
            mail.IsBodyHtml = true;
            mail.Body = "Unable to send email notification for " + EmailSubject + " to " + Recipient + "<br/> Error: " + ex.Message;
            mail.From = new MailAddress("corp@mangalaminfotech.com");
            mail.To.Add(new MailAddress("salesforce.mangalam@gmail.com"));
            //ServicePointManager.SecurityProtocol = (SecurityProtocolType)48 | (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
            sc.Send(mail);
        }

        return MailStatus;
    }

    //Convert string to TitleCase
    public static String TitleCase(String strParam)
    {
        String strTitleCase = strParam.Substring(0, 1).ToUpper();
        strParam = strParam.Substring(1).ToLower();
        String strPrev = "";
        bool caps = true;
        for (int iIndex = 0; iIndex < strParam.Length; iIndex++)
        {
            if (iIndex > 1)
            {
                strPrev = strParam.Substring(iIndex - 1, 1);
            }
            if (strPrev.Equals(" ") ||
                strPrev.Equals("\t") ||
                strPrev.Equals("\n") ||
                strPrev.Equals(".") ||
                strPrev.Equals("(") ||
                strPrev.Equals(")")
                )
            {
                if (strPrev.Equals("("))
                    caps = false;
                strTitleCase += strParam.Substring(iIndex, 1).ToUpper();
            }
            else
            {
                if (strPrev.Equals(")"))
                    caps = true;
                if (caps == false)
                    strTitleCase += strParam.Substring(iIndex, 1).ToUpper();
                else
                    strTitleCase += strParam.Substring(iIndex, 1);
            }
        }
        return strTitleCase;
    }

    //Send Email for test
    public static string SendEmailwithDisplay(string EmailSender, string Recipient, string EmailSubject, string EmailBody, string DisplayName)
    {
        MITInvoiceDataContext dbobj = new MITInvoiceDataContext();
        
        Recipient = Recipient.Replace(", ", ",").Replace("; ", ";").Replace(",,", ",").Replace(";;", ";").Replace(",;", ",").Replace(";", ",");

        string MailStatus;
        SmtpClient sc;
        getSMTPClient(out MailStatus, out sc);

        try
        {
            MailMessage mail = new MailMessage();
            mail.Subject = EmailSubject;
            mail.IsBodyHtml = true;
            mail.Body = EmailBody;
            mail.From = new MailAddress(EmailSender, DisplayName);

            string[] recpStr = Recipient.Replace(", ", ",").Replace(",,", ",").Split(',');
            for (int i = 0; i < recpStr.Length; i++)
            {
                mail.To.Add(new MailAddress(recpStr[i]));
            }

            //ServicePointManager.SecurityProtocol = (SecurityProtocolType)48 | (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
            sc.Send(mail);
            MailStatus = "Sucess";
        }
        catch (Exception ex)
        {
            MailStatus = ex.Message;
            MailMessage mail = new MailMessage();
            mail.Subject = "Unable to send email notification";
            mail.IsBodyHtml = true;
            mail.Body = "Unable to send email notification for " + EmailSubject + " to " + Recipient + "<br/> Error: " + ex.Message;
            mail.From = new MailAddress("corp@mangalaminfotech.com");
            mail.To.Add(new MailAddress("system@mangalaminfotech.net"));
            //ServicePointManager.SecurityProtocol = (SecurityProtocolType)48 | (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
            sc.Send(mail);
        }

        return MailStatus;
    }
}
