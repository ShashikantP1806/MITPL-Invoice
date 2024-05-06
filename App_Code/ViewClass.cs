using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Summary description for ViewClass
/// </summary>
public class ViewClass
{
    MITInvoiceDataContext db = new MITInvoiceDataContext();
    public ViewClass()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public DataTable GetTableView(string tablename, int RecordId)
    {
        DataTable dt = new DataTable();
        //if (tablename.Contains("CountryMaster"))
        //{
        //    var Country = from DBData in db.CountryMasters
        //                  join State in db.StateMasters on DBData.CountryId equals State.CountryId
        //                  where DBData.CountryId == RecordId
        //                  select new
        //                  {
        //                      CountryName = DBData.CountryName,
        //                      StateName = State.StateName,
        //                  };
        //    dt.Columns.Add("CountryName");
        //    dt.Columns.Add("StateName");
        //    DataRow row = null;
        //    foreach (var rowobj in Country)
        //    {
        //        row = dt.NewRow();
        //        dt.Rows.Add(rowobj.CountryName, rowobj.StateName);
        //    }
        //}
        //if (tablename.Contains("StateMaster"))
        //{
        //    //var State = from DBData in db.StateMasters
        //    //            join Country in db.CountryMasters on DBData.CountryId equals Country.CountryId
        //    //            where DBData.CountryId == RecordId
        //    //            select new
        //    //            {
        //    //                StateName = DBData.StateName,
        //    //                CountryName=Country.CountryName
        //    //            };
        //    //dt.Columns.Add("StateName");
        //    //dt.Columns.Add("CountryName");
        //    //DataRow row = null;
        //    //foreach (var rowobj in State)
        //    //{
        //    //    row = dt.NewRow();
        //    //    dt.Rows.Add(rowobj.StateName, rowobj.CountryName);
        //    //}
        //}
        //if (tablename.Contains("CityMasters"))
        //{
        //    var City = from DBData in db.CityMasters
        //                join State in db.StateMasters on DBData.StateId equals State.StateId
        //                join Country in db.CountryMasters on DBData.CountryId equals Country.CountryId
        //                where DBData.CountryId == RecordId
        //                select new
        //                {
        //                    StateName = DBData.StateName,
        //                    CountryName = Country.CountryName
        //                };
        //    dt.Columns.Add("StateName");
        //    dt.Columns.Add("CountryName");
        //    DataRow row = null;
        //    foreach (var rowobj in State)
        //    {
        //        row = dt.NewRow();
        //        dt.Rows.Add(rowobj.StateName, rowobj.CountryName);
        //    }
        //}
        return dt;
    }

}
