using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Linq.Mapping;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing;

public partial class DeletePopup : System.Web.UI.Page
{
    MITInvoiceDataContext db = new MITInvoiceDataContext();
    ViewClass vc = new ViewClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserName"] != null)
        {
            if (Request.QueryString.Keys.Count > 0)
            {
                if (!IsPostBack)
                {
                    int RecordId = Convert.ToInt16(Request.QueryString["id"]);
                    string Name = Request.QueryString["name"];
                    lblError.Text = "You cannnot delete this " + Name + ", as following records are depend on it. To delete this " + Name + ", you need to first delete following records.";
                    var DataModel = new AttributeMappingSource().GetModel(typeof(MITInvoiceDataContext));

                    foreach (var table in DataModel.GetTables())
                    {
                        if (Request.QueryString["page"].ToString() == "country")
                        {
                            #region CountryMaster
                            if (table.TableName.Contains("StateMaster"))
                            {
                                DataTable dt = new DataTable();
                                try
                                {
                                    var State = from DBData in db.StateMasters
                                                where DBData.CountryId == RecordId
                                                select new
                                                {
                                                    StateName = DBData.StateName,
                                                    CountryName = DBData.CountryMaster.CountryName
                                                };
                                    dt.Columns.Add("StateName");
                                    dt.Columns.Add("CountryName");
                                    DataRow row = null;
                                    foreach (var rowobj in State)
                                    {
                                        row = dt.NewRow();
                                        dt.Rows.Add(rowobj.StateName, rowobj.CountryName);
                                    }
                                    if (dt.Rows.Count > 0)
                                    {
                                        GridView gv = new GridView();
                                        gv.ID = table.TableName.ToString();
                                        gv.DataSource = dt;
                                        gv.DataBind();
                                        Label tblname = new Label();
                                        tblname.Text = table.TableName.ToString().Replace("dbo.", "");

                                        tblname.Font.Bold = true;
                                        ph.Controls.Add(tblname);
                                        ph.Controls.Add(gv);
                                        ph.Controls.Add(new Literal() { ID = "row", Text = "<br/><br/>" });
                                        gv.Dispose();
                                    }
                                }
                                catch
                                {
                                }
                                finally
                                {
                                    dt.Dispose();
                                }

                            }
                            if (table.TableName.Contains("CityMaster"))
                            {
                                DataTable dt = new DataTable();
                                try
                                {
                                    var City = from DBData in db.CityMasters
                                               where DBData.CountryId == RecordId
                                               select new
                                               {
                                                   CityName = DBData.CityName,
                                                   StateName = DBData.StateMaster.StateName,
                                                   CountryName = DBData.CountryMaster.CountryName
                                               };
                                    dt.Columns.Add("CityName");
                                    dt.Columns.Add("StateName");
                                    dt.Columns.Add("CountryName");
                                    DataRow row = null;
                                    foreach (var rowobj in City)
                                    {
                                        row = dt.NewRow();
                                        dt.Rows.Add(rowobj.CityName, rowobj.StateName, rowobj.CountryName);
                                    }
                                    if (dt.Rows.Count > 0)
                                    {
                                        GridView gv = new GridView();
                                        gv.ID = table.TableName.ToString();
                                        gv.DataSource = dt;
                                        gv.DataBind();
                                        Label tblname = new Label();
                                        tblname.Text = table.TableName.ToString().Replace("dbo.", "");

                                        tblname.Font.Bold = true;
                                        ph.Controls.Add(tblname);
                                        ph.Controls.Add(gv);
                                        ph.Controls.Add(new Literal() { ID = "row", Text = "<br/><br/>" });
                                        gv.Dispose();
                                    }
                                }
                                catch
                                {
                                }
                                finally
                                {
                                    dt.Dispose();
                                }
                            }
                            if (table.TableName.Contains("ClientMaster"))
                            {
                                DataTable dt = new DataTable();
                                try
                                {
                                    var Client = (from DBData in db.ClientMasters
                                                  join DBC1 in db.CountryMasters on DBData.Country1 equals DBC1.CountryId
                                                  where DBData.Country1 == RecordId && DBData.IsActive == true
                                                  select new
                                                  {
                                                      CientName = DBData.ClientName,
                                                      CountryName = DBC1.CountryName
                                                  }).Union(from DBData in db.ClientMasters
                                                           join DBC2 in db.CountryMasters on DBData.Country2 equals DBC2.CountryId
                                                           where DBData.Country2 == RecordId && DBData.IsActive == true
                                                           select new
                                                           {
                                                               CientName = DBData.ClientName,
                                                               CountryName = DBC2.CountryName
                                                           });
                                    dt.Columns.Add("CientName");
                                    dt.Columns.Add("CountryName");
                                    DataRow row = null;
                                    foreach (var rowobj in Client)
                                    {
                                        row = dt.NewRow();
                                        dt.Rows.Add(rowobj.CientName, rowobj.CountryName);
                                    }
                                    if (dt.Rows.Count > 0)
                                    {
                                        GridView gv = new GridView();
                                        gv.ID = table.TableName.ToString();
                                        gv.DataSource = dt;
                                        gv.DataBind();
                                        Label tblname = new Label();
                                        tblname.Text = table.TableName.ToString().Replace("dbo.", "");

                                        tblname.Font.Bold = true;
                                        ph.Controls.Add(tblname);
                                        ph.Controls.Add(gv);
                                        ph.Controls.Add(new Literal() { ID = "row", Text = "<br/><br/>" });
                                        gv.Dispose();
                                    }
                                }
                                catch
                                {
                                }
                                finally
                                {
                                    dt.Dispose();
                                }
                            }
                            #endregion
                        }
                        if (Request.QueryString["page"].ToString() == "state")
                        {
                            #region StateMaster
                            if (table.TableName.Contains("CityMaster"))
                            {

                                DataTable dt = new DataTable();
                                try
                                {
                                    //dt = vc.GetTableView(table.TableName.ToString(), Convert.ToInt16(RecordId));
                                    var City = from DBData in db.CityMasters
                                               join State in db.StateMasters on DBData.StateId equals State.StateId
                                               where DBData.StateId == RecordId
                                               select new
                                               {
                                                   CityName = DBData.CityName,
                                                   StateName = State.StateName
                                               };
                                    dt.Columns.Add("CityName");
                                    dt.Columns.Add("StateName");
                                    DataRow row = null;
                                    foreach (var rowobj in City)
                                    {
                                        row = dt.NewRow();
                                        dt.Rows.Add(rowobj.CityName, rowobj.StateName);
                                    }
                                    if (dt.Rows.Count > 0)
                                    {
                                        GridView gv = new GridView();
                                        gv.ID = table.TableName.ToString();
                                        gv.DataSource = dt;
                                        gv.DataBind();
                                        Label tblname = new Label();
                                        tblname.Text = table.TableName.ToString().Replace("dbo.", "");

                                        tblname.Font.Bold = true;
                                        ph.Controls.Add(tblname);
                                        ph.Controls.Add(gv);
                                        ph.Controls.Add(new Literal() { ID = "row", Text = "<br/><br/>" });
                                        gv.Dispose();
                                    }
                                }
                                catch
                                {
                                }
                                finally
                                {
                                    dt.Dispose();
                                }

                            }

                            if (table.TableName.Contains("ClientMaster"))
                            {
                                DataTable dt = new DataTable();
                                try
                                {
                                    var Client = (from DBData in db.ClientMasters
                                                  join DBS1 in db.StateMasters on DBData.State1 equals DBS1.StateId
                                                  where DBData.State1 == RecordId && DBData.IsActive == true
                                                  select new
                                                  {
                                                      CientName = DBData.ClientName,
                                                      StateName = DBS1.StateName
                                                  }).Union(from DBData in db.ClientMasters
                                                           join DBS2 in db.StateMasters on DBData.State2 equals DBS2.StateId
                                                           where DBData.State2 == RecordId && DBData.IsActive == true
                                                           select new
                                                           {
                                                               CientName = DBData.ClientName,
                                                               StateName = DBS2.StateName
                                                           });
                                    dt.Columns.Add("CientName");
                                    dt.Columns.Add("StateName");
                                    DataRow row = null;
                                    foreach (var rowobj in Client)
                                    {
                                        row = dt.NewRow();
                                        dt.Rows.Add(rowobj.CientName, rowobj.StateName);
                                    }
                                    if (dt.Rows.Count > 0)
                                    {
                                        GridView gv = new GridView();
                                        gv.ID = table.TableName.ToString();
                                        gv.DataSource = dt;
                                        gv.DataBind();
                                        Label tblname = new Label();
                                        tblname.Text = table.TableName.ToString().Replace("dbo.", "");

                                        tblname.Font.Bold = true;
                                        ph.Controls.Add(tblname);
                                        ph.Controls.Add(gv);
                                        ph.Controls.Add(new Literal() { ID = "row", Text = "<br/><br/>" });
                                        gv.Dispose();
                                    }
                                }
                                catch
                                {
                                }
                                finally
                                {
                                    dt.Dispose();
                                }
                            }
                            #endregion
                        }

                        if (Request.QueryString["page"].ToString() == "city")
                        {
                            #region CityMaster
                            if (table.TableName.Contains("ClientMaster"))
                            {
                                DataTable dt = new DataTable();
                                try
                                {
                                    var Client = (from DBData in db.ClientMasters
                                                  join DBC1 in db.CityMasters on DBData.City1 equals DBC1.CityId
                                                  where DBData.City1 == RecordId && DBData.IsActive == true
                                                  select new
                                                  {
                                                      CientName = DBData.ClientName,
                                                      CityName = DBC1.CityName
                                                  }).Union(from DBData in db.ClientMasters
                                                           join DBC2 in db.CityMasters on DBData.City2 equals DBC2.CityId
                                                           where DBData.City2 == RecordId && DBData.IsActive == true
                                                           select new
                                                           {
                                                               CientName = DBData.ClientName,
                                                               CityName = DBC2.CityName
                                                           });
                                    dt.Columns.Add("CientName");
                                    dt.Columns.Add("CityName");
                                    DataRow row = null;
                                    foreach (var rowobj in Client)
                                    {
                                        row = dt.NewRow();
                                        dt.Rows.Add(rowobj.CientName, rowobj.CityName);
                                    }
                                    if (dt.Rows.Count > 0)
                                    {
                                        GridView gv = new GridView();
                                        gv.ID = table.TableName.ToString();
                                        gv.DataSource = dt;
                                        gv.DataBind();
                                        Label tblname = new Label();
                                        tblname.Text = table.TableName.ToString().Replace("dbo.", "");

                                        tblname.Font.Bold = true;
                                        ph.Controls.Add(tblname);
                                        ph.Controls.Add(gv);
                                        ph.Controls.Add(new Literal() { ID = "row", Text = "<br/><br/>" });
                                        gv.Dispose();
                                    }
                                }
                                catch
                                {
                                }
                                finally
                                {
                                    dt.Dispose();
                                }
                            }
                            #endregion
                        }

                        if (Request.QueryString["page"].ToString() == "user")
                        {
                            #region UserMaster
                            if (table.TableName.Contains("DepartmentMaster"))
                            {

                                DataTable dt = new DataTable();
                                try
                                {
                                    //dt = vc.GetTableView(table.TableName.ToString(), Convert.ToInt16(RecordId));
                                    var Department = from DBData in db.DepartmentMasters
                                                     join DBU in db.UserMasters on DBData.UserId equals DBU.UserId
                                                     where DBData.UserId == RecordId
                                                     select new
                                                     {
                                                         DepartmentName = DBData.DepartmentName,
                                                         UserName = DBU.FirstName + " " + DBU.LastName
                                                     };
                                    dt.Columns.Add("DepartmentName");
                                    dt.Columns.Add("UserName");
                                    DataRow row = null;
                                    foreach (var rowobj in Department)
                                    {
                                        row = dt.NewRow();
                                        dt.Rows.Add(rowobj.DepartmentName, rowobj.UserName);
                                    }
                                    if (dt.Rows.Count > 0)
                                    {
                                        GridView gv = new GridView();
                                        gv.ID = table.TableName.ToString();
                                        gv.DataSource = dt;
                                        gv.DataBind();
                                        Label tblname = new Label();
                                        tblname.Text = table.TableName.ToString().Replace("dbo.", "");

                                        tblname.Font.Bold = true;
                                        ph.Controls.Add(tblname);
                                        ph.Controls.Add(gv);
                                        ph.Controls.Add(new Literal() { ID = "row", Text = "<br/><br/>" });
                                        gv.Dispose();
                                    }
                                }
                                catch
                                {
                                }
                                finally
                                {
                                    dt.Dispose();
                                }

                            }
                            #endregion
                        }
                        if (Request.QueryString["page"].ToString() == "dept")
                        {
                            #region DepartmentMaster
                            if (table.TableName.Contains("UserMaster"))
                            {
                                DataTable dt = new DataTable();
                                try
                                {
                                    //dt = vc.GetTableView(table.TableName.ToString(), Convert.ToInt16(RecordId));
                                    var UserData = from DBData in db.UserMasters
                                                   join DBD in db.DepartmentMasters on DBData.DepartmentId equals DBD.DepartmentId
                                                   where DBData.DepartmentId == RecordId
                                                   select new
                                                   {
                                                       UserName = DBData.FirstName + " " + DBData.LastName,
                                                       DepartmentName = DBD.DepartmentName
                                                   };
                                    dt.Columns.Add("UserName");
                                    dt.Columns.Add("DepartmentName");
                                    DataRow row = null;
                                    foreach (var rowobj in UserData)
                                    {
                                        row = dt.NewRow();
                                        dt.Rows.Add(rowobj.UserName, rowobj.DepartmentName);
                                    }
                                    if (dt.Rows.Count > 0)
                                    {
                                        GridView gv = new GridView();
                                        gv.ID = table.TableName.ToString();
                                        gv.DataSource = dt;
                                        gv.DataBind();
                                        Label tblname = new Label();
                                        tblname.Text = table.TableName.ToString().Replace("dbo.", "");

                                        tblname.Font.Bold = true;
                                        ph.Controls.Add(tblname);
                                        ph.Controls.Add(gv);
                                        ph.Controls.Add(new Literal() { ID = "row", Text = "<br/><br/>" });
                                        gv.Dispose();
                                    }
                                }
                                catch
                                {
                                }
                                finally
                                {
                                    dt.Dispose();
                                }
                            }
                            if (table.TableName.Contains("ClientMaster"))
                            {
                                DataTable dt = new DataTable();
                                try
                                {
                                    //dt = vc.GetTableView(table.TableName.ToString(), Convert.ToInt16(RecordId));
                                    var ClientData = from DBData in db.ClientMasters
                                                     join DBD in db.DepartmentMasters on DBData.DepartmentId equals DBD.DepartmentId
                                                     where DBData.DepartmentId == RecordId
                                                     select new
                                                     {
                                                         ClientName = DBData.ClientName,
                                                         DepartmentName = DBD.DepartmentName
                                                     };
                                    dt.Columns.Add("ClientName");
                                    dt.Columns.Add("DepartmentName");
                                    DataRow row = null;
                                    foreach (var rowobj in ClientData)
                                    {
                                        row = dt.NewRow();
                                        dt.Rows.Add(rowobj.ClientName, rowobj.DepartmentName);
                                    }
                                    if (dt.Rows.Count > 0)
                                    {
                                        GridView gv = new GridView();
                                        gv.ID = table.TableName.ToString();
                                        gv.DataSource = dt;
                                        gv.DataBind();
                                        Label tblname = new Label();
                                        tblname.Text = table.TableName.ToString().Replace("dbo.", "");

                                        tblname.Font.Bold = true;
                                        ph.Controls.Add(tblname);
                                        ph.Controls.Add(gv);
                                        ph.Controls.Add(new Literal() { ID = "row", Text = "<br/><br/>" });
                                        gv.Dispose();
                                    }
                                }
                                catch
                                {
                                }
                                finally
                                {
                                    dt.Dispose();
                                }
                            }
                            if (table.TableName.Contains("ProcessMaster"))
                            {
                                DataTable dt = new DataTable();
                                try
                                {
                                    //dt = vc.GetTableView(table.TableName.ToString(), Convert.ToInt16(RecordId));
                                    var ProcessData = from DBData in db.ProcessMasters
                                                      join DBD in db.DepartmentMasters on DBData.DepartmentId equals DBD.DepartmentId
                                                      where DBData.DepartmentId == RecordId
                                                      select new
                                                      {
                                                          ProcessName = DBData.ProcessName,
                                                          DepartmentName = DBD.DepartmentName
                                                      };
                                    dt.Columns.Add("ProcessName");
                                    dt.Columns.Add("DepartmentName");
                                    DataRow row = null;
                                    foreach (var rowobj in ProcessData)
                                    {
                                        row = dt.NewRow();
                                        dt.Rows.Add(rowobj.ProcessName, rowobj.DepartmentName);
                                    }
                                    if (dt.Rows.Count > 0)
                                    {
                                        GridView gv = new GridView();
                                        gv.ID = table.TableName.ToString();
                                        gv.DataSource = dt;
                                        gv.DataBind();
                                        Label tblname = new Label();
                                        tblname.Text = table.TableName.ToString().Replace("dbo.", "");

                                        tblname.Font.Bold = true;
                                        ph.Controls.Add(tblname);
                                        ph.Controls.Add(gv);
                                        ph.Controls.Add(new Literal() { ID = "row", Text = "<br/><br/>" });
                                        gv.Dispose();
                                    }
                                }
                                catch
                                {
                                }
                                finally
                                {
                                    dt.Dispose();
                                }
                            }
                            #endregion
                        }
                        if (Request.QueryString["page"].ToString() == "process")
                        {
                            #region ProcessMaster
                            if (table.TableName.Contains("PriceMaster"))
                            {
                                DataTable dt = new DataTable();
                                try
                                {
                                    //dt = vc.GetTableView(table.TableName.ToString(), Convert.ToInt16(RecordId));
                                    var PriceData = from DBData in db.PriceMasters
                                                    join DBPro in db.ProcessMasters on DBData.ProcessId equals DBPro.ProcessId
                                                    where DBData.ProcessId == RecordId
                                                    select new
                                                    {
                                                        ClientName = DBData.ClientMaster.ClientName,
                                                        PriceType = DBData.PriceTypeMaster.PriceType,
                                                        Process = DBPro.ProcessName,
                                                        UnitPrice = DBData.UnitPrice
                                                    };
                                    dt.Columns.Add("ClientName");
                                    dt.Columns.Add("PriceType");
                                    dt.Columns.Add("Process");
                                    dt.Columns.Add("UnitPrice");
                                    DataRow row = null;
                                    foreach (var rowobj in PriceData)
                                    {
                                        row = dt.NewRow();
                                        dt.Rows.Add(rowobj.ClientName, rowobj.PriceType, rowobj.Process, rowobj.UnitPrice);
                                    }
                                    if (dt.Rows.Count > 0)
                                    {
                                        GridView gv = new GridView();
                                        gv.ID = table.TableName.ToString();
                                        gv.DataSource = dt;
                                        gv.DataBind();
                                        Label tblname = new Label();
                                        tblname.Text = table.TableName.ToString().Replace("dbo.", "");

                                        tblname.Font.Bold = true;
                                        ph.Controls.Add(tblname);
                                        ph.Controls.Add(gv);
                                        ph.Controls.Add(new Literal() { ID = "row", Text = "<br/><br/>" });
                                        gv.Dispose();
                                    }
                                }
                                catch
                                {
                                }
                                finally
                                {
                                    dt.Dispose();
                                }
                            }
                            #endregion
                        }
                        if (Request.QueryString["page"].ToString() == "currency")
                        {
                            #region CurrencyMaster
                            if (table.TableName.Contains("ClientMaster"))
                            {
                                DataTable dt = new DataTable();
                                try
                                {
                                    //dt = vc.GetTableView(table.TableName.ToString(), Convert.ToInt16(RecordId));
                                    var ClientData = from DBData in db.ClientMasters
                                                     join DBCurr in db.CurrencyMasters on DBData.CurrencyId equals DBCurr.CurrencyId
                                                     where DBData.CurrencyId == RecordId
                                                     select new
                                                     {
                                                         ClientName = DBData.ClientName,
                                                         Currency = DBCurr.CurrencyName
                                                     };
                                    dt.Columns.Add("ClientName");
                                    dt.Columns.Add("Currency");
                                    DataRow row = null;
                                    foreach (var rowobj in ClientData)
                                    {
                                        row = dt.NewRow();
                                        dt.Rows.Add(rowobj.ClientName, rowobj.Currency);
                                    }
                                    if (dt.Rows.Count > 0)
                                    {
                                        GridView gv = new GridView();
                                        gv.ID = table.TableName.ToString();
                                        gv.DataSource = dt;
                                        gv.DataBind();
                                        Label tblname = new Label();
                                        tblname.Text = table.TableName.ToString().Replace("dbo.", "");

                                        tblname.Font.Bold = true;
                                        ph.Controls.Add(tblname);
                                        ph.Controls.Add(gv);
                                        ph.Controls.Add(new Literal() { ID = "row", Text = "<br/><br/>" });
                                        gv.Dispose();
                                    }
                                }
                                catch
                                {
                                }
                                finally
                                {
                                    dt.Dispose();
                                }
                            }
                            #endregion
                        }
                        if (Request.QueryString["page"].ToString() == "pricetype")
                        {
                            #region PriceTypeMaster
                            if (table.TableName.Contains("PriceMaster"))
                            {
                                DataTable dt = new DataTable();
                                try
                                {
                                    //dt = vc.GetTableView(table.TableName.ToString(), Convert.ToInt16(RecordId));
                                    var PriceData = from DBData in db.PriceMasters
                                                    join DBPT in db.PriceTypeMasters on DBData.PriceTypeId equals DBPT.PriceTypeId
                                                    where DBData.PriceTypeId == RecordId
                                                    select new
                                                    {
                                                        ClientName = DBData.ClientMaster.ClientName,
                                                        PriceType = DBPT.PriceType,
                                                        Process = DBData.ProcessMaster.ProcessName,
                                                        UnitPrice = DBData.UnitPrice
                                                    };
                                    dt.Columns.Add("ClientName");
                                    dt.Columns.Add("PriceType");
                                    dt.Columns.Add("Process");
                                    dt.Columns.Add("UnitPrice");
                                    DataRow row = null;
                                    foreach (var rowobj in PriceData)
                                    {
                                        row = dt.NewRow();
                                        dt.Rows.Add(rowobj.ClientName, rowobj.PriceType, rowobj.Process, rowobj.UnitPrice);
                                    }
                                    if (dt.Rows.Count > 0)
                                    {
                                        GridView gv = new GridView();
                                        gv.ID = table.TableName.ToString();
                                        gv.DataSource = dt;
                                        gv.DataBind();
                                        Label tblname = new Label();
                                        tblname.Text = table.TableName.ToString().Replace("dbo.", "");

                                        tblname.Font.Bold = true;
                                        ph.Controls.Add(tblname);
                                        ph.Controls.Add(gv);
                                        ph.Controls.Add(new Literal() { ID = "row", Text = "<br/><br/>" });
                                        gv.Dispose();
                                    }
                                }
                                catch
                                {
                                }
                                finally
                                {
                                    dt.Dispose();
                                }
                            }
                            #endregion
                        }
                        if (Request.QueryString["page"].ToString() == "client")
                        {
                            #region ClientMaster
                            if (table.TableName.Contains("PriceMaster"))
                            {
                                DataTable dt = new DataTable();
                                try
                                {
                                    //dt = vc.GetTableView(table.TableName.ToString(), Convert.ToInt16(RecordId));
                                    var PriceData = from DBData in db.PriceMasters
                                                    join DBC in db.ClientMasters on DBData.ClientId equals DBC.ClientId
                                                    where DBData.ClientId == RecordId
                                                    select new
                                                    {
                                                        PriceType = DBData.PriceTypeMaster.PriceType,
                                                        Process = DBData.ProcessMaster.ProcessName,
                                                        UnitPrice = DBData.UnitPrice,
                                                        ClientName = DBC.ClientName
                                                    };
                                    dt.Columns.Add("PriceType");
                                    dt.Columns.Add("Process");
                                    dt.Columns.Add("UnitPrice");
                                    dt.Columns.Add("ClientName");
                                    DataRow row = null;
                                    foreach (var rowobj in PriceData)
                                    {
                                        row = dt.NewRow();
                                        dt.Rows.Add(rowobj.PriceType, rowobj.Process, rowobj.UnitPrice, rowobj.ClientName);
                                    }
                                    if (dt.Rows.Count > 0)
                                    {
                                        GridView gv = new GridView();
                                        gv.ID = table.TableName.ToString();
                                        gv.DataSource = dt;
                                        gv.DataBind();
                                        Label tblname = new Label();
                                        tblname.Text = table.TableName.ToString().Replace("dbo.", "");

                                        tblname.Font.Bold = true;
                                        ph.Controls.Add(tblname);
                                        ph.Controls.Add(gv);
                                        ph.Controls.Add(new Literal() { ID = "row", Text = "<br/><br/>" });
                                        gv.Dispose();
                                    }
                                }
                                catch
                                {
                                }
                                finally
                                {
                                    dt.Dispose();
                                }
                            }
                            if (table.TableName.Contains("InvoiceMaster"))
                            {
                                DataTable dt = new DataTable();
                                try
                                {
                                    //dt = vc.GetTableView(table.TableName.ToString(), Convert.ToInt16(RecordId));
                                    var InvoiceData = from DBData in db.InvoiceMasters
                                                      join DBC in db.ClientMasters on DBData.ClientId equals DBC.ClientId
                                                      where DBData.ClientId == RecordId
                                                      select new
                                                      {
                                                          InvoiceNo = DBData.InvoiceNumber,
                                                          Paid = DBData.IsPaid,
                                                          ClientName = DBC.ClientName
                                                      };
                                    dt.Columns.Add("InvoiceNo");
                                    dt.Columns.Add("Paid");
                                    dt.Columns.Add("ClientName");
                                    DataRow row = null;
                                    foreach (var rowobj in InvoiceData)
                                    {
                                        row = dt.NewRow();
                                        dt.Rows.Add(rowobj.InvoiceNo, rowobj.Paid, rowobj.ClientName);
                                    }
                                    if (dt.Rows.Count > 0)
                                    {
                                        GridView gv = new GridView();
                                        gv.ID = table.TableName.ToString();
                                        gv.DataSource = dt;
                                        gv.DataBind();
                                        Label tblname = new Label();
                                        tblname.Text = table.TableName.ToString().Replace("dbo.", "");

                                        tblname.Font.Bold = true;
                                        ph.Controls.Add(tblname);
                                        ph.Controls.Add(gv);
                                        ph.Controls.Add(new Literal() { ID = "row", Text = "<br/><br/>" });
                                        gv.Dispose();
                                    }
                                }
                                catch
                                {
                                }
                                finally
                                {
                                    dt.Dispose();
                                }
                            }
                            if (table.TableName.Contains("ClientContactMaster"))
                            {
                                DataTable dt = new DataTable();
                                try
                                {
                                    //dt = vc.GetTableView(table.TableName.ToString(), Convert.ToInt16(RecordId));
                                    var CContactData = from DBData in db.ClientContactMasters
                                                       join DBC in db.ClientMasters on DBData.ClientId equals DBC.ClientId
                                                       where DBData.ClientId == RecordId&& DBData.IsDeleted==false
                                                       select new
                                                       {
                                                           Name = DBData.Name,
                                                           Email = DBData.EmailAddress,
                                                           ClientName = DBC.ClientName
                                                       };
                                    dt.Columns.Add("ContactPerson");
                                    dt.Columns.Add("Email");
                                    dt.Columns.Add("ClientName");
                                    DataRow row = null;
                                    foreach (var rowobj in CContactData)
                                    {
                                        row = dt.NewRow();
                                        dt.Rows.Add(rowobj.Name, rowobj.Email, rowobj.ClientName);
                                    }
                                    if (dt.Rows.Count > 0)
                                    {
                                        GridView gv = new GridView();
                                        gv.ID = table.TableName.ToString();
                                        gv.DataSource = dt;
                                        gv.DataBind();
                                        Label tblname = new Label();
                                        tblname.Text = table.TableName.ToString().Replace("dbo.", "");

                                        tblname.Font.Bold = true;
                                        ph.Controls.Add(tblname);
                                        ph.Controls.Add(gv);
                                        ph.Controls.Add(new Literal() { ID = "row", Text = "<br/><br/>" });
                                        gv.Dispose();
                                    }
                                }
                                catch
                                {
                                }
                                finally
                                {
                                    dt.Dispose();
                                }
                            }
                            #endregion
                        }
                    }
                }
            }
            else
            {
                Response.Redirect("Home");
            }
        }
        else
        {
            Response.Redirect("Login");
        }
    }
}
