using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;

public partial class Client : System.Web.UI.Page
{
    MITInvoiceDataContext dbobj = new MITInvoiceDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Global.UserType == "BUSINESS UNIT MANAGER" || Global.UserType == "DIRECTOR" || Global.UserType == "BUSINESS ASSOCIATES")
        {
            if (!IsPostBack)
            {
                mvClient.ActiveViewIndex = 0;
                ddlCountryVisible("");
                FillDepartmentSearch();
                FillGrid();
                
            }
            if (Global.UserType == "DIRECTOR")
            {
                btnNewClient.Visible = false;
                gridClient.Columns[11].Visible = false;
            }

            

        }
        else
        {
            Response.Redirect("Authorize");
        }
    }

    protected void FillDepartment()
    {
        var Department = from DBData in dbobj.DepartmentMasters
                         where DBData.IsActive == true
                         &&
                         (DBData.DepartmentId == Convert.ToInt64(Global.Department) || DBData.UserId == Convert.ToInt64(Global.UserId))
                         orderby DBData.DepartmentName
                         select new
                         {
                             DepartmentId = DBData.DepartmentId,
                             DepartmentName = DBData.DepartmentName
                         };
        if (Department.Count() > 0)
        {
            ddlDepartment.DataSource = Department;
            ddlDepartment.DataTextField = "DepartmentName";
            ddlDepartment.DataValueField = "DepartmentId";
            ddlDepartment.DataBind();
        }
        if (ddlDepartment.Items.Count > 1)
        {
            ddlDepartment.Items.Insert(0, "-- Select --");
        }
    }

    protected void FillDepartmentSearch()
    {
        if (Global.UserType == "DIRECTOR" || Global.DepartmentName == "ACCT")
        {
            var Department = from DBData in dbobj.DepartmentMasters
                             where DBData.IsActive == true
                             orderby DBData.DepartmentName
                             select new
                             {
                                 DepartmentId = DBData.DepartmentId,
                                 DepartmentName = DBData.DepartmentName
                             };
            if (Department.Count() > 0)
            {
                ddlDepartmentSearch.DataSource = Department;
                ddlDepartmentSearch.DataTextField = "DepartmentName";
                ddlDepartmentSearch.DataValueField = "DepartmentId";
                ddlDepartmentSearch.DataBind();
            }
            if (ddlDepartmentSearch.Items.Count > 1)
            {
                ddlDepartmentSearch.Items.Insert(0, "-- All --");
            }
        }
        else
        {
            var Department = from DBData in dbobj.DepartmentMasters
                             where DBData.IsActive == true
                             &&
                             (DBData.DepartmentId == Convert.ToInt64(Global.Department) || DBData.UserId == Convert.ToInt64(Global.UserId))
                             orderby DBData.DepartmentName
                             select new
                             {
                                 DepartmentId = DBData.DepartmentId,
                                 DepartmentName = DBData.DepartmentName
                             };
            if (Department.Count() > 0)
            {
                ddlDepartmentSearch.DataSource = Department;
                ddlDepartmentSearch.DataTextField = "DepartmentName";
                ddlDepartmentSearch.DataValueField = "DepartmentId";
                ddlDepartmentSearch.DataBind();
            }
            if (ddlDepartmentSearch.Items.Count > 1)
            {
                ddlDepartmentSearch.Items.Insert(0, "-- All --");
            }
        }
    }

    protected void FillCountry()
    {
        var Country = from DBData in dbobj.CountryMasters
                      orderby DBData.CountryName
                      select DBData;
        if (Country.Count() > 0)
        {
            ddlCountry1.DataSource = Country;
            ddlCountry1.DataTextField = "CountryName";
            ddlCountry1.DataValueField = "CountryId";
            ddlCountry1.DataBind();
            ddlCountry2.DataSource = Country;
            ddlCountry2.DataTextField = "CountryName";
            ddlCountry2.DataValueField = "CountryId";
            ddlCountry2.DataBind();
        }
        ddlCountry1.Items.Insert(0, "-- Select --");
        ddlCountry2.Items.Insert(0, "-- Select --");
    }

    protected void FillState1()
    {
        ddlState1.Items.Clear();
        if (ddlCountry1.SelectedIndex != 0)
        {
            var State1 = from DBData in dbobj.StateMasters
                         where DBData.CountryId == Convert.ToInt64(ddlCountry1.SelectedValue)
                         orderby DBData.StateName
                         select DBData;
            ddlState1.DataSource = State1;
            ddlState1.DataTextField = "StateName";
            ddlState1.DataValueField = "StateId";
            ddlState1.DataBind();
        }
        ddlState1.Items.Insert(0, "-- Select --");
    }

    protected void FillState2()
    {
        ddlState2.Items.Clear();
        if (ddlCountry2.SelectedIndex != 0)
        {
            var State2 = from DBData in dbobj.StateMasters
                         where DBData.CountryId == Convert.ToInt64(ddlCountry2.SelectedValue)
                         orderby DBData.StateName
                         select DBData;
            ddlState2.DataSource = State2;
            ddlState2.DataTextField = "StateName";
            ddlState2.DataValueField = "StateId";
            ddlState2.DataBind();
        }
        ddlState2.Items.Insert(0, "-- Select --");
    }

    protected void FillCity1()
    {
        ddlCity1.Items.Clear();
        if (ddlState1.SelectedItem.ToString() != "-- Select --")
        {
            var City1 = from DBData in dbobj.CityMasters
                        where DBData.StateId == Convert.ToInt64(ddlState1.SelectedValue)
                        orderby DBData.CityName
                        select DBData;
            ddlCity1.DataSource = City1;
            ddlCity1.DataTextField = "CityName";
            ddlCity1.DataValueField = "CityId";
            ddlCity1.DataBind();

        }
        ddlCity1.Items.Insert(0, "-- Select --");
    }

    protected void FillCity2()
    {
        ddlCity2.Items.Clear();
        if (ddlState2.SelectedItem.ToString() != "-- Select --")
        {
            var City2 = from DBData in dbobj.CityMasters
                        where DBData.StateId == Convert.ToInt64(ddlState2.SelectedValue)
                        orderby DBData.CityName
                        select DBData;
            ddlCity2.DataSource = City2;
            ddlCity2.DataTextField = "CityName";
            ddlCity2.DataValueField = "CityId";
            ddlCity2.DataBind();
        }
        ddlCity2.Items.Insert(0, "-- Select --");
    }

    protected void FillCurrency()
    {
        var Currency = from DBData in dbobj.CurrencyMasters
                       where DBData.IsDeleted == false
                       orderby DBData.CurrencyName
                       select new
                       {
                           CurrencyId = DBData.CurrencyId,
                           CurrencyName = DBData.CurrencyName
                       };
        if (Currency.Count() > 0)
        {
            ddlCurrency.DataSource = Currency;
            ddlCurrency.DataTextField = "CurrencyName";
            ddlCurrency.DataValueField = "CurrencyId";
            ddlCurrency.DataBind();
        }
        ddlCurrency.Items.Insert(0, "-- Select --");
    }

    public void FillGrid()
    {
        if (ddlDepartmentSearch.SelectedIndex != -1)
        {
            TextBox txtMasterSearch = (TextBox)this.Master.FindControl("txtMasterSearch");
            if (txtMasterSearch.Text == "")
            {
                if (Global.UserType == "DIRECTOR" || Global.DepartmentName == "ACCT")
                {
                    if (ddlDepartmentSearch.SelectedItem.ToString() == "-- All --")
                    {
                        //// 10-Aug-2020 By Jignesh
                        //// added && (DBData.C_M1 == Global.UserM1 || DBData.C_M2 == Global.UserM2)
                        var Client = from DBData in dbobj.ClientMasters
                                     where DBData.IsActive == true && (DBData.C_M1 == Global.UserM1 || DBData.C_M2 == Global.UserM2)
                                     orderby DBData.DepartmentMaster.DepartmentName, DBData.ClientName
                                     select new
                                     {
                                         ClientId = DBData.ClientId,
                                         ClientName = DBData.ClientName,
                                         DepartmentId = DBData.DepartmentId,
                                         Department = DBData.DepartmentMaster.DepartmentName,
                                         Phone = DBData.Phone,
                                         Address1 = "To: " + DBData.Address1 + ",<br>" + (from DBC in dbobj.CityMasters
                                                                                          where DBC.CityId == DBData.City1
                                                                                          select DBC.CityName).First() + ", " + (from DBState in dbobj.StateMasters
                                                                                                                                 where DBState.StateId == DBData.State1
                                                                                                                                 select DBState.StateName).First() + ",<br>" + (from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                where DBCounty.CountryId == DBData.Country1
                                                                                                                                                                                select DBCounty.CountryName).First() + " - " + (DBData.Zip_Postal1 != null ? DBData.Zip_Postal1 : " "),
                                         Address2 = DBData.Address2 == null ? "-" : "To: " + DBData.Address2 + ",<br>" + ((from DBC in dbobj.CityMasters
                                                                                                                           where DBC.CityId == DBData.City2
                                                                                                                           select DBC.CityName).First() != null ? (from DBC in dbobj.CityMasters
                                                                                                                                                                   where DBC.CityId == DBData.City2
                                                                                                                                                                   select DBC.CityName).First() : "") + ", " + ((from DBState in dbobj.StateMasters
                                                                                                                                                                                                                 where DBState.StateId == DBData.State2
                                                                                                                                                                                                                 select DBState.StateName).First() != null ? (from DBState in dbobj.StateMasters
                                                                                                                                                                                                                                                              where DBState.StateId == DBData.State2
                                                                                                                                                                                                                                                              select DBState.StateName).First() : " ") + ",<br>" + ((from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                                                                                                                                                     where DBCounty.CountryId == DBData.Country2
                                                                                                                                                                                                                                                                                                                     select DBCounty.CountryName).First() != null ? (from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                                                                                                                                                                                                     where DBCounty.CountryId == DBData.Country2
                                                                                                                                                                                                                                                                                                                                                                     select DBCounty.CountryName).First() : "") + " - " + (DBData.Zip_Postal1 != null ? DBData.Zip_Postal1 : " "),
                                         Website = DBData.Website == null ? "-" : DBData.Website,
                                         C_M1 = DBData.C_M1,
                                         C_M2 = DBData.C_M2,
                                         //Currency = ""
                                         Currency = DBData.CurrencyMaster.CurrencyCode + "-" + ("&#" + DBData.CurrencyMaster.CurrencySymbol)
                                     };
                        if (Client.Count() > 0)
                        {
                            gridClient.DataSource = Client;
                            gridClient.DataBind();
                        }
                        else
                        {
                            BlankGrid();
                        }
                    }
                    else
                    {
                        //// 10-Aug-2020 By Jignesh
                        //// added && (DBData.C_M1 == Global.UserM1 || DBData.C_M2 == Global.UserM2)
                        var Client = from DBData in dbobj.ClientMasters
                                     where DBData.IsActive == true && DBData.DepartmentId == Convert.ToInt64(ddlDepartmentSearch.SelectedValue) && (DBData.C_M1 == Global.UserM1 || DBData.C_M2 == Global.UserM2)
                                     orderby DBData.DepartmentMaster.DepartmentName, DBData.ClientName
                                     select new
                                     {
                                         ClientId = DBData.ClientId,
                                         ClientName = DBData.ClientName,
                                         DepartmentId = DBData.DepartmentId,
                                         Department = DBData.DepartmentMaster.DepartmentName,
                                         Phone = DBData.Phone,
                                         Address1 = "To: " + DBData.Address1 + ",<br>" + (from DBC in dbobj.CityMasters
                                                                                          where DBC.CityId == DBData.City1
                                                                                          select DBC.CityName).First() + ", " + (from DBState in dbobj.StateMasters
                                                                                                                                 where DBState.StateId == DBData.State1
                                                                                                                                 select DBState.StateName).First() + ",<br>" + (from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                where DBCounty.CountryId == DBData.Country1
                                                                                                                                                                                select DBCounty.CountryName).First() + " - " + (DBData.Zip_Postal1 != null ? DBData.Zip_Postal1 : " "),
                                         Address2 = DBData.Address2 == null ? "-" : "To: " + DBData.Address2 + ",<br>" + ((from DBC in dbobj.CityMasters
                                                                                                                           where DBC.CityId == DBData.City2
                                                                                                                           select DBC.CityName).First() != null ? (from DBC in dbobj.CityMasters
                                                                                                                                                                   where DBC.CityId == DBData.City2
                                                                                                                                                                   select DBC.CityName).First() : "") + ", " + ((from DBState in dbobj.StateMasters
                                                                                                                                                                                                                 where DBState.StateId == DBData.State2
                                                                                                                                                                                                                 select DBState.StateName).First() != null ? (from DBState in dbobj.StateMasters
                                                                                                                                                                                                                                                              where DBState.StateId == DBData.State2
                                                                                                                                                                                                                                                              select DBState.StateName).First() : " ") + ",<br>" + ((from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                                                                                                                                                     where DBCounty.CountryId == DBData.Country2
                                                                                                                                                                                                                                                                                                                     select DBCounty.CountryName).First() != null ? (from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                                                                                                                                                                                                     where DBCounty.CountryId == DBData.Country2
                                                                                                                                                                                                                                                                                                                                                                     select DBCounty.CountryName).First() : "") + " - " + (DBData.Zip_Postal1 != null ? DBData.Zip_Postal1 : " "),
                                         Website = DBData.Website == null ? "-" : DBData.Website,
                                         C_M1 = DBData.C_M1,
                                         C_M2 = DBData.C_M2,
                                         //Currency = ""
                                         Currency = DBData.CurrencyMaster.CurrencyCode + "-" + ("&#" + DBData.CurrencyMaster.CurrencySymbol)
                                     };
                        if (Client.Count() > 0)
                        {
                            gridClient.DataSource = Client;
                            gridClient.DataBind();
                        }
                        else
                        {
                            BlankGrid();
                        }
                    }
                }
                else
                {
                    if (ddlDepartmentSearch.SelectedItem.ToString() == "-- All --")
                    {

                        //// 10-Aug-2020 By Jignesh
                        //// added   : && (DBData.C_M1 == Global.UserM1 || DBData.C_M2 == Global.UserM2)
                        //// added : && DBData.DepartmentMaster.UserId == Convert.ToInt64(Global.UserId) (Revert back again on 05-Oct-2020 by Jignesh)

                        var Client = from DBData in dbobj.ClientMasters
                                     where DBData.IsActive == true && (DBData.C_M1 == Global.UserM1 || DBData.C_M2 == Global.UserM2) && DBData.DepartmentMaster.UserId == Convert.ToInt64(Global.UserId)
                                     orderby DBData.DepartmentMaster.DepartmentName, DBData.ClientName
                                     select new
                                     {
                                         ClientId = DBData.ClientId,
                                         ClientName = DBData.ClientName,
                                         DepartmentId = DBData.DepartmentId,
                                         Department = DBData.DepartmentMaster.DepartmentName,
                                         Phone = DBData.Phone,
                                         Address1 = "To: " + DBData.Address1 + ",<br>" + (from DBC in dbobj.CityMasters
                                                                                          where DBC.CityId == DBData.City1
                                                                                          select DBC.CityName).First() + ", " + (from DBState in dbobj.StateMasters
                                                                                                                                 where DBState.StateId == DBData.State1
                                                                                                                                 select DBState.StateName).First() + ",<br>" + (from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                where DBCounty.CountryId == DBData.Country1
                                                                                                                                                                                select DBCounty.CountryName).First() + " - " + (DBData.Zip_Postal1 != null ? DBData.Zip_Postal1 : " "),
                                         Address2 = DBData.Address2 == null ? "-" : "To: " + DBData.Address2 + ",<br>" + ((from DBC in dbobj.CityMasters
                                                                                                                           where DBC.CityId == DBData.City2
                                                                                                                           select DBC.CityName).First() != null ? (from DBC in dbobj.CityMasters
                                                                                                                                                                   where DBC.CityId == DBData.City2
                                                                                                                                                                   select DBC.CityName).First() : "") + ", " + ((from DBState in dbobj.StateMasters
                                                                                                                                                                                                                 where DBState.StateId == DBData.State2
                                                                                                                                                                                                                 select DBState.StateName).First() != null ? (from DBState in dbobj.StateMasters
                                                                                                                                                                                                                                                              where DBState.StateId == DBData.State2
                                                                                                                                                                                                                                                              select DBState.StateName).First() : " ") + ",<br>" + ((from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                                                                                                                                                     where DBCounty.CountryId == DBData.Country2
                                                                                                                                                                                                                                                                                                                     select DBCounty.CountryName).First() != null ? (from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                                                                                                                                                                                                     where DBCounty.CountryId == DBData.Country2
                                                                                                                                                                                                                                                                                                                                                                     select DBCounty.CountryName).First() : "") + " - " + (DBData.Zip_Postal1 != null ? DBData.Zip_Postal1 : " "),
                                         Website = DBData.Website == null ? "-" : DBData.Website,
                                         C_M1 = DBData.C_M1,
                                         C_M2 = DBData.C_M2,
                                         //Currency = ""
                                         Currency = DBData.CurrencyMaster.CurrencyCode + "-" + ("&#" + DBData.CurrencyMaster.CurrencySymbol)
                                     };
                        if (Client.Count() > 0)
                        {
                            gridClient.DataSource = Client;
                            gridClient.DataBind();
                        }
                        else
                        {
                            BlankGrid();
                        }
                    }
                    else
                    {
                        //// 24-Jul-2020 By Jignesh
                        //// added   : && (DBData.C_M1 == Global.UserM1 || DBData.C_M2 == Global.UserM2)
                        //// removed : && DBData.DepartmentMaster.UserId == Convert.ToInt64(Global.UserId)
                        var Client = from DBData in dbobj.ClientMasters
                                     where DBData.IsActive == true && DBData.DepartmentId == Convert.ToInt64(ddlDepartmentSearch.SelectedValue) && (DBData.C_M1 == Global.UserM1 || DBData.C_M2 == Global.UserM2)
                                     orderby DBData.DepartmentMaster.DepartmentName, DBData.ClientName
                                     select new
                                     {
                                         ClientId = DBData.ClientId,
                                         ClientName = DBData.ClientName,
                                         DepartmentId = DBData.DepartmentId,
                                         Department = DBData.DepartmentMaster.DepartmentName,
                                         Phone = DBData.Phone,
                                         Address1 = "To: " + DBData.Address1 + ",<br>" + (from DBC in dbobj.CityMasters
                                                                                          where DBC.CityId == DBData.City1
                                                                                          select DBC.CityName).First() + ", " + (from DBState in dbobj.StateMasters
                                                                                                                                 where DBState.StateId == DBData.State1
                                                                                                                                 select DBState.StateName).First() + ",<br>" + (from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                where DBCounty.CountryId == DBData.Country1
                                                                                                                                                                                select DBCounty.CountryName).First() + " - " + (DBData.Zip_Postal1 != null ? DBData.Zip_Postal1 : " "),
                                         Address2 = DBData.Address2 == null ? "-" : "To: " + DBData.Address2 + ",<br>" + ((from DBC in dbobj.CityMasters
                                                                                                                           where DBC.CityId == DBData.City2
                                                                                                                           select DBC.CityName).First() != null ? (from DBC in dbobj.CityMasters
                                                                                                                                                                   where DBC.CityId == DBData.City2
                                                                                                                                                                   select DBC.CityName).First() : "") + ", " + ((from DBState in dbobj.StateMasters
                                                                                                                                                                                                                 where DBState.StateId == DBData.State2
                                                                                                                                                                                                                 select DBState.StateName).First() != null ? (from DBState in dbobj.StateMasters
                                                                                                                                                                                                                                                              where DBState.StateId == DBData.State2
                                                                                                                                                                                                                                                              select DBState.StateName).First() : " ") + ",<br>" + ((from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                                                                                                                                                     where DBCounty.CountryId == DBData.Country2
                                                                                                                                                                                                                                                                                                                     select DBCounty.CountryName).First() != null ? (from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                                                                                                                                                                                                     where DBCounty.CountryId == DBData.Country2
                                                                                                                                                                                                                                                                                                                                                                     select DBCounty.CountryName).First() : "") + " - " + (DBData.Zip_Postal1 != null ? DBData.Zip_Postal1 : " "),
                                         Website = DBData.Website == null ? "-" : DBData.Website,
                                         C_M1 = DBData.C_M1,
                                         C_M2 = DBData.C_M2,
                                         //Currency = ""
                                         Currency = DBData.CurrencyMaster.CurrencyCode + "-" + ("&#" + DBData.CurrencyMaster.CurrencySymbol)
                                     };
                        if (Client.Count() > 0)
                        {
                            gridClient.DataSource = Client;
                            gridClient.DataBind();
                        }
                        else
                        {
                            BlankGrid();
                        }
                    }
                }
            }
            else
            {
                if (Global.UserType == "DIRECTOR" || Global.DepartmentName == "ACCT")
                {
                    if (ddlDepartmentSearch.SelectedItem.ToString() == "-- All --")
                    {
                        //// added && (DBData.C_M1 == Global.UserM1 || DBData.C_M2 == Global.UserM2) on 10-Aug-2020 by Jignesh
                        var Client = from DBData in dbobj.ClientMasters
                                     where DBData.IsActive == true && DBData.ClientName.Contains(txtMasterSearch.Text.Trim()) && (DBData.C_M1 == Global.UserM1 || DBData.C_M2 == Global.UserM2)
                                     orderby DBData.DepartmentMaster.DepartmentName, DBData.ClientName
                                     select new
                                     {
                                         ClientId = DBData.ClientId,
                                         ClientName = DBData.ClientName,
                                         DepartmentId = DBData.DepartmentId,
                                         Department = DBData.DepartmentMaster.DepartmentName,
                                         Phone = DBData.Phone,
                                         Address1 = "To: " + DBData.Address1 + ",<br>" + (from DBC in dbobj.CityMasters
                                                                                          where DBC.CityId == DBData.City1
                                                                                          select DBC.CityName).First() + ", " + (from DBState in dbobj.StateMasters
                                                                                                                                 where DBState.StateId == DBData.State1
                                                                                                                                 select DBState.StateName).First() + ",<br>" + (from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                where DBCounty.CountryId == DBData.Country1
                                                                                                                                                                                select DBCounty.CountryName).First() + " - " + (DBData.Zip_Postal1 != null ? DBData.Zip_Postal1 : " "),
                                         Address2 = DBData.Address2 == null ? "-" : "To: " + DBData.Address2 + ",<br>" + ((from DBC in dbobj.CityMasters
                                                                                                                           where DBC.CityId == DBData.City2
                                                                                                                           select DBC.CityName).First() != null ? (from DBC in dbobj.CityMasters
                                                                                                                                                                   where DBC.CityId == DBData.City2
                                                                                                                                                                   select DBC.CityName).First() : "") + ", " + ((from DBState in dbobj.StateMasters
                                                                                                                                                                                                                 where DBState.StateId == DBData.State2
                                                                                                                                                                                                                 select DBState.StateName).First() != null ? (from DBState in dbobj.StateMasters
                                                                                                                                                                                                                                                              where DBState.StateId == DBData.State2
                                                                                                                                                                                                                                                              select DBState.StateName).First() : " ") + ",<br>" + ((from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                                                                                                                                                     where DBCounty.CountryId == DBData.Country2
                                                                                                                                                                                                                                                                                                                     select DBCounty.CountryName).First() != null ? (from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                                                                                                                                                                                                     where DBCounty.CountryId == DBData.Country2
                                                                                                                                                                                                                                                                                                                                                                     select DBCounty.CountryName).First() : "") + " - " + (DBData.Zip_Postal1 != null ? DBData.Zip_Postal1 : " "),
                                         Website = DBData.Website == null ? "-" : DBData.Website,
                                         C_M1 = DBData.C_M1,
                                         C_M2 = DBData.C_M2,
                                         //Currency = ""
                                         Currency = DBData.CurrencyMaster.CurrencyCode + "-" + ("&#" + DBData.CurrencyMaster.CurrencySymbol)
                                     };
                        if (Client.Count() > 0)
                        {
                            gridClient.DataSource = Client;
                            gridClient.DataBind();
                        }
                        else
                        {
                            BlankGrid();
                        }
                    }
                    else
                    {
                        //// 10-Aug-2020 By Jignesh
                        //// added && (DBData.C_M1 == Global.UserM1 || DBData.C_M2 == Global.UserM2)
                        var Client = from DBData in dbobj.ClientMasters
                                     where DBData.IsActive == true && DBData.DepartmentId == Convert.ToInt64(ddlDepartmentSearch.SelectedValue) && DBData.ClientName.Contains(txtMasterSearch.Text.Trim()) && (DBData.C_M1 == Global.UserM1 || DBData.C_M2 == Global.UserM2)
                                     orderby DBData.DepartmentMaster.DepartmentName, DBData.ClientName
                                     select new
                                     {
                                         ClientId = DBData.ClientId,
                                         ClientName = DBData.ClientName,
                                         DepartmentId = DBData.DepartmentId,
                                         Department = DBData.DepartmentMaster.DepartmentName,
                                         Phone = DBData.Phone,
                                         Address1 = "To: " + DBData.Address1 + ",<br>" + (from DBC in dbobj.CityMasters
                                                                                          where DBC.CityId == DBData.City1
                                                                                          select DBC.CityName).First() + ", " + (from DBState in dbobj.StateMasters
                                                                                                                                 where DBState.StateId == DBData.State1
                                                                                                                                 select DBState.StateName).First() + ",<br>" + (from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                where DBCounty.CountryId == DBData.Country1
                                                                                                                                                                                select DBCounty.CountryName).First() + " - " + (DBData.Zip_Postal1 != null ? DBData.Zip_Postal1 : " "),
                                         Address2 = DBData.Address2 == null ? "-" : "To: " + DBData.Address2 + ",<br>" + ((from DBC in dbobj.CityMasters
                                                                                                                           where DBC.CityId == DBData.City2
                                                                                                                           select DBC.CityName).First() != null ? (from DBC in dbobj.CityMasters
                                                                                                                                                                   where DBC.CityId == DBData.City2
                                                                                                                                                                   select DBC.CityName).First() : "") + ", " + ((from DBState in dbobj.StateMasters
                                                                                                                                                                                                                 where DBState.StateId == DBData.State2
                                                                                                                                                                                                                 select DBState.StateName).First() != null ? (from DBState in dbobj.StateMasters
                                                                                                                                                                                                                                                              where DBState.StateId == DBData.State2
                                                                                                                                                                                                                                                              select DBState.StateName).First() : " ") + ",<br>" + ((from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                                                                                                                                                     where DBCounty.CountryId == DBData.Country2
                                                                                                                                                                                                                                                                                                                     select DBCounty.CountryName).First() != null ? (from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                                                                                                                                                                                                     where DBCounty.CountryId == DBData.Country2
                                                                                                                                                                                                                                                                                                                                                                     select DBCounty.CountryName).First() : "") + " - " + (DBData.Zip_Postal1 != null ? DBData.Zip_Postal1 : " "),
                                         Website = DBData.Website == null ? "-" : DBData.Website,
                                         C_M1 = DBData.C_M1,
                                         C_M2 = DBData.C_M2,
                                         //Currency = ""
                                         Currency = DBData.CurrencyMaster.CurrencyCode + "-" + ("&#" + DBData.CurrencyMaster.CurrencySymbol)
                                     };
                        if (Client.Count() > 0)
                        {
                            gridClient.DataSource = Client;
                            gridClient.DataBind();
                        }
                        else
                        {
                            BlankGrid();
                        }
                    }
                }
                else
                {

                    if (ddlDepartmentSearch.SelectedItem.ToString() == "-- All --")
                    {
                        //// 10-Aug-2020 By Jignesh
                        //// added && (DBData.C_M1 == Global.UserM1 || DBData.C_M2 == Global.UserM2)
                        //// removed : && DBData.DepartmentMaster.UserId == Convert.ToInt64(Global.UserId)
                        var Client = from DBData in dbobj.ClientMasters
                                     where DBData.IsActive == true && DBData.ClientName.Contains(txtMasterSearch.Text.Trim()) && (DBData.C_M1 == Global.UserM1 || DBData.C_M2 == Global.UserM2)
                                     orderby DBData.DepartmentMaster.DepartmentName, DBData.ClientName
                                     select new
                                     {
                                         ClientId = DBData.ClientId,
                                         ClientName = DBData.ClientName,
                                         DepartmentId = DBData.DepartmentId,
                                         Department = DBData.DepartmentMaster.DepartmentName,
                                         Phone = DBData.Phone,
                                         Address1 = "To: " + DBData.Address1 + ",<br>" + (from DBC in dbobj.CityMasters
                                                                                          where DBC.CityId == DBData.City1
                                                                                          select DBC.CityName).First() + ", " + (from DBState in dbobj.StateMasters
                                                                                                                                 where DBState.StateId == DBData.State1
                                                                                                                                 select DBState.StateName).First() + ",<br>" + (from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                where DBCounty.CountryId == DBData.Country1
                                                                                                                                                                                select DBCounty.CountryName).First() + " - " + (DBData.Zip_Postal1 != null ? DBData.Zip_Postal1 : " "),
                                         Address2 = DBData.Address2 == null ? "-" : "To: " + DBData.Address2 + ",<br>" + ((from DBC in dbobj.CityMasters
                                                                                                                           where DBC.CityId == DBData.City2
                                                                                                                           select DBC.CityName).First() != null ? (from DBC in dbobj.CityMasters
                                                                                                                                                                   where DBC.CityId == DBData.City2
                                                                                                                                                                   select DBC.CityName).First() : "") + ", " + ((from DBState in dbobj.StateMasters
                                                                                                                                                                                                                 where DBState.StateId == DBData.State2
                                                                                                                                                                                                                 select DBState.StateName).First() != null ? (from DBState in dbobj.StateMasters
                                                                                                                                                                                                                                                              where DBState.StateId == DBData.State2
                                                                                                                                                                                                                                                              select DBState.StateName).First() : " ") + ",<br>" + ((from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                                                                                                                                                     where DBCounty.CountryId == DBData.Country2
                                                                                                                                                                                                                                                                                                                     select DBCounty.CountryName).First() != null ? (from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                                                                                                                                                                                                     where DBCounty.CountryId == DBData.Country2
                                                                                                                                                                                                                                                                                                                                                                     select DBCounty.CountryName).First() : "") + " - " + (DBData.Zip_Postal1 != null ? DBData.Zip_Postal1 : " "),
                                         Website = DBData.Website == null ? "-" : DBData.Website,
                                         C_M1 = DBData.C_M1,
                                         C_M2 = DBData.C_M2,
                                         //Currency = ""
                                         Currency = DBData.CurrencyMaster.CurrencyCode + "-" + ("&#" + DBData.CurrencyMaster.CurrencySymbol)
                                     };
                        if (Client.Count() > 0)
                        {
                            gridClient.DataSource = Client;
                            gridClient.DataBind();
                        }
                        else
                        {
                            BlankGrid();
                        }
                    }
                    else
                    {
                        //// 10-Aug-2020 By Jignesh
                        //// added && (DBData.C_M1 == Global.UserM1 || DBData.C_M2 == Global.UserM2)
                        //// removed : && DBData.DepartmentMaster.UserId == Convert.ToInt64(Global.UserId)

                        var Client = from DBData in dbobj.ClientMasters
                                     where DBData.IsActive == true && DBData.DepartmentId == Convert.ToInt64(ddlDepartmentSearch.SelectedValue) && DBData.ClientName.Contains(txtMasterSearch.Text.Trim()) && (DBData.C_M1 == Global.UserM1 || DBData.C_M2 == Global.UserM2)
                                     orderby DBData.DepartmentMaster.DepartmentName, DBData.ClientName
                                     select new
                                     {
                                         ClientId = DBData.ClientId,
                                         ClientName = DBData.ClientName,
                                         DepartmentId = DBData.DepartmentId,
                                         Department = DBData.DepartmentMaster.DepartmentName,
                                         Phone = DBData.Phone,
                                         Address1 = "To: " + DBData.Address1 + ",<br>" + (from DBC in dbobj.CityMasters
                                                                                          where DBC.CityId == DBData.City1
                                                                                          select DBC.CityName).First() + ", " + (from DBState in dbobj.StateMasters
                                                                                                                                 where DBState.StateId == DBData.State1
                                                                                                                                 select DBState.StateName).First() + ",<br>" + (from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                where DBCounty.CountryId == DBData.Country1
                                                                                                                                                                                select DBCounty.CountryName).First() + " - " + (DBData.Zip_Postal1 != null ? DBData.Zip_Postal1 : " "),
                                         Address2 = DBData.Address2 == null ? "-" : "To: " + DBData.Address2 + ",<br>" + ((from DBC in dbobj.CityMasters
                                                                                                                           where DBC.CityId == DBData.City2
                                                                                                                           select DBC.CityName).First() != null ? (from DBC in dbobj.CityMasters
                                                                                                                                                                   where DBC.CityId == DBData.City2
                                                                                                                                                                   select DBC.CityName).First() : "") + ", " + ((from DBState in dbobj.StateMasters
                                                                                                                                                                                                                 where DBState.StateId == DBData.State2
                                                                                                                                                                                                                 select DBState.StateName).First() != null ? (from DBState in dbobj.StateMasters
                                                                                                                                                                                                                                                              where DBState.StateId == DBData.State2
                                                                                                                                                                                                                                                              select DBState.StateName).First() : " ") + ",<br>" + ((from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                                                                                                                                                     where DBCounty.CountryId == DBData.Country2
                                                                                                                                                                                                                                                                                                                     select DBCounty.CountryName).First() != null ? (from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                                                                                                                                                                                                     where DBCounty.CountryId == DBData.Country2
                                                                                                                                                                                                                                                                                                                                                                     select DBCounty.CountryName).First() : "") + " - " + (DBData.Zip_Postal1 != null ? DBData.Zip_Postal1 : " "),
                                         Website = DBData.Website == null ? "-" : DBData.Website,
                                         C_M1 = DBData.C_M1,
                                         C_M2 = DBData.C_M2,
                                         //Currency = ""
                                         Currency = DBData.CurrencyMaster.CurrencyCode + "-" + ("&#" + DBData.CurrencyMaster.CurrencySymbol)
                                     };
                        if (Client.Count() > 0)
                        {
                            gridClient.DataSource = Client;
                            gridClient.DataBind();
                        }
                        else
                        {
                            BlankGrid();
                        }
                    }
                }
            }
        }
        else
            BlankGrid();
    }

    ////10-Aug-2020 by Jignesh
    public void FillGrid_Existing()
    {
        if (ddlDepartmentSearch.SelectedIndex != -1)
        {
            TextBox txtMasterSearch = (TextBox)this.Master.FindControl("txtMasterSearch");
            if (txtMasterSearch.Text == "")
            {
                if (Global.UserType == "DIRECTOR" || Global.DepartmentName == "ACCT")
                {
                    if (ddlDepartmentSearch.SelectedItem.ToString() == "-- All --")
                    {
                        //// added && (DBData.C_M1 == Global.UserM1 || DBData.C_M2 == Global.UserM2)
                        var Client = from DBData in dbobj.ClientMasters
                                     where DBData.IsActive == true
                                     orderby DBData.DepartmentMaster.DepartmentName, DBData.ClientName
                                     select new
                                     {
                                         ClientId = DBData.ClientId,
                                         ClientName = DBData.ClientName,
                                         DepartmentId = DBData.DepartmentId,
                                         Department = DBData.DepartmentMaster.DepartmentName,
                                         Phone = DBData.Phone,
                                         Address1 = "To: " + DBData.Address1 + ",<br>" + (from DBC in dbobj.CityMasters
                                                                                          where DBC.CityId == DBData.City1
                                                                                          select DBC.CityName).First() + ", " + (from DBState in dbobj.StateMasters
                                                                                                                                 where DBState.StateId == DBData.State1
                                                                                                                                 select DBState.StateName).First() + ",<br>" + (from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                where DBCounty.CountryId == DBData.Country1
                                                                                                                                                                                select DBCounty.CountryName).First() + " - " + (DBData.Zip_Postal1 != null ? DBData.Zip_Postal1 : " "),
                                         Address2 = DBData.Address2 == null ? "-" : "To: " + DBData.Address2 + ",<br>" + ((from DBC in dbobj.CityMasters
                                                                                                                           where DBC.CityId == DBData.City2
                                                                                                                           select DBC.CityName).First() != null ? (from DBC in dbobj.CityMasters
                                                                                                                                                                   where DBC.CityId == DBData.City2
                                                                                                                                                                   select DBC.CityName).First() : "") + ", " + ((from DBState in dbobj.StateMasters
                                                                                                                                                                                                                 where DBState.StateId == DBData.State2
                                                                                                                                                                                                                 select DBState.StateName).First() != null ? (from DBState in dbobj.StateMasters
                                                                                                                                                                                                                                                              where DBState.StateId == DBData.State2
                                                                                                                                                                                                                                                              select DBState.StateName).First() : " ") + ",<br>" + ((from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                                                                                                                                                     where DBCounty.CountryId == DBData.Country2
                                                                                                                                                                                                                                                                                                                     select DBCounty.CountryName).First() != null ? (from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                                                                                                                                                                                                     where DBCounty.CountryId == DBData.Country2
                                                                                                                                                                                                                                                                                                                                                                     select DBCounty.CountryName).First() : "") + " - " + (DBData.Zip_Postal1 != null ? DBData.Zip_Postal1 : " "),
                                         Website = DBData.Website == null ? "-" : DBData.Website,
                                         C_M1 = DBData.C_M1,
                                         C_M2 = DBData.C_M2,
                                         //Currency = ""
                                         Currency = DBData.CurrencyMaster.CurrencyCode + "-" + ("&#" + DBData.CurrencyMaster.CurrencySymbol)
                                     };
                        if (Client.Count() > 0)
                        {
                            gridClient.DataSource = Client;
                            gridClient.DataBind();
                        }
                        else
                        {
                            BlankGrid();
                        }
                    }
                    else
                    {
                        var Client = from DBData in dbobj.ClientMasters
                                     where DBData.IsActive == true && DBData.DepartmentId == Convert.ToInt64(ddlDepartmentSearch.SelectedValue)
                                     orderby DBData.DepartmentMaster.DepartmentName, DBData.ClientName
                                     select new
                                     {
                                         ClientId = DBData.ClientId,
                                         ClientName = DBData.ClientName,
                                         DepartmentId = DBData.DepartmentId,
                                         Department = DBData.DepartmentMaster.DepartmentName,
                                         Phone = DBData.Phone,
                                         Address1 = "To: " + DBData.Address1 + ",<br>" + (from DBC in dbobj.CityMasters
                                                                                          where DBC.CityId == DBData.City1
                                                                                          select DBC.CityName).First() + ", " + (from DBState in dbobj.StateMasters
                                                                                                                                 where DBState.StateId == DBData.State1
                                                                                                                                 select DBState.StateName).First() + ",<br>" + (from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                where DBCounty.CountryId == DBData.Country1
                                                                                                                                                                                select DBCounty.CountryName).First() + " - " + (DBData.Zip_Postal1 != null ? DBData.Zip_Postal1 : " "),
                                         Address2 = DBData.Address2 == null ? "-" : "To: " + DBData.Address2 + ",<br>" + ((from DBC in dbobj.CityMasters
                                                                                                                           where DBC.CityId == DBData.City2
                                                                                                                           select DBC.CityName).First() != null ? (from DBC in dbobj.CityMasters
                                                                                                                                                                   where DBC.CityId == DBData.City2
                                                                                                                                                                   select DBC.CityName).First() : "") + ", " + ((from DBState in dbobj.StateMasters
                                                                                                                                                                                                                 where DBState.StateId == DBData.State2
                                                                                                                                                                                                                 select DBState.StateName).First() != null ? (from DBState in dbobj.StateMasters
                                                                                                                                                                                                                                                              where DBState.StateId == DBData.State2
                                                                                                                                                                                                                                                              select DBState.StateName).First() : " ") + ",<br>" + ((from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                                                                                                                                                     where DBCounty.CountryId == DBData.Country2
                                                                                                                                                                                                                                                                                                                     select DBCounty.CountryName).First() != null ? (from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                                                                                                                                                                                                     where DBCounty.CountryId == DBData.Country2
                                                                                                                                                                                                                                                                                                                                                                     select DBCounty.CountryName).First() : "") + " - " + (DBData.Zip_Postal1 != null ? DBData.Zip_Postal1 : " "),
                                         Website = DBData.Website == null ? "-" : DBData.Website,
                                         C_M1 = DBData.C_M1,
                                         C_M2 = DBData.C_M2,
                                         //Currency = ""
                                         Currency = DBData.CurrencyMaster.CurrencyCode + "-" + ("&#" + DBData.CurrencyMaster.CurrencySymbol)
                                     };
                        if (Client.Count() > 0)
                        {
                            gridClient.DataSource = Client;
                            gridClient.DataBind();
                        }
                        else
                        {
                            BlankGrid();
                        }
                    }
                }
                else
                {
                    if (ddlDepartmentSearch.SelectedItem.ToString() == "-- All --")
                    {
                        var Client = from DBData in dbobj.ClientMasters
                                     where DBData.IsActive == true && DBData.DepartmentMaster.UserId == Convert.ToInt64(Global.UserId)
                                     orderby DBData.DepartmentMaster.DepartmentName, DBData.ClientName
                                     select new
                                     {
                                         ClientId = DBData.ClientId,
                                         ClientName = DBData.ClientName,
                                         DepartmentId = DBData.DepartmentId,
                                         Department = DBData.DepartmentMaster.DepartmentName,
                                         Phone = DBData.Phone,
                                         Address1 = "To: " + DBData.Address1 + ",<br>" + (from DBC in dbobj.CityMasters
                                                                                          where DBC.CityId == DBData.City1
                                                                                          select DBC.CityName).First() + ", " + (from DBState in dbobj.StateMasters
                                                                                                                                 where DBState.StateId == DBData.State1
                                                                                                                                 select DBState.StateName).First() + ",<br>" + (from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                where DBCounty.CountryId == DBData.Country1
                                                                                                                                                                                select DBCounty.CountryName).First() + " - " + (DBData.Zip_Postal1 != null ? DBData.Zip_Postal1 : " "),
                                         Address2 = DBData.Address2 == null ? "-" : "To: " + DBData.Address2 + ",<br>" + ((from DBC in dbobj.CityMasters
                                                                                                                           where DBC.CityId == DBData.City2
                                                                                                                           select DBC.CityName).First() != null ? (from DBC in dbobj.CityMasters
                                                                                                                                                                   where DBC.CityId == DBData.City2
                                                                                                                                                                   select DBC.CityName).First() : "") + ", " + ((from DBState in dbobj.StateMasters
                                                                                                                                                                                                                 where DBState.StateId == DBData.State2
                                                                                                                                                                                                                 select DBState.StateName).First() != null ? (from DBState in dbobj.StateMasters
                                                                                                                                                                                                                                                              where DBState.StateId == DBData.State2
                                                                                                                                                                                                                                                              select DBState.StateName).First() : " ") + ",<br>" + ((from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                                                                                                                                                     where DBCounty.CountryId == DBData.Country2
                                                                                                                                                                                                                                                                                                                     select DBCounty.CountryName).First() != null ? (from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                                                                                                                                                                                                     where DBCounty.CountryId == DBData.Country2
                                                                                                                                                                                                                                                                                                                                                                     select DBCounty.CountryName).First() : "") + " - " + (DBData.Zip_Postal1 != null ? DBData.Zip_Postal1 : " "),
                                         Website = DBData.Website == null ? "-" : DBData.Website,
                                         C_M1 = DBData.C_M1,
                                         C_M2 = DBData.C_M2,
                                         //Currency = ""
                                         Currency = DBData.CurrencyMaster.CurrencyCode + "-" + ("&#" + DBData.CurrencyMaster.CurrencySymbol)
                                     };
                        if (Client.Count() > 0)
                        {
                            gridClient.DataSource = Client;
                            gridClient.DataBind();
                        }
                        else
                        {
                            BlankGrid();
                        }
                    }
                    else
                    {
                        //// 24-Jul-2020 By Jignesh
                        //// added   : && (DBData.C_M1 == Global.UserM1 || DBData.C_M2 == Global.UserM2)
                        //// removed : && DBData.DepartmentMaster.UserId == Convert.ToInt64(Global.UserId)
                        var Client = from DBData in dbobj.ClientMasters
                                     where DBData.IsActive == true && DBData.DepartmentId == Convert.ToInt64(ddlDepartmentSearch.SelectedValue) && DBData.DepartmentMaster.UserId == Convert.ToInt64(Global.UserId)
                                     orderby DBData.DepartmentMaster.DepartmentName, DBData.ClientName
                                     select new
                                     {
                                         ClientId = DBData.ClientId,
                                         ClientName = DBData.ClientName,
                                         DepartmentId = DBData.DepartmentId,
                                         Department = DBData.DepartmentMaster.DepartmentName,
                                         Phone = DBData.Phone,
                                         Address1 = "To: " + DBData.Address1 + ",<br>" + (from DBC in dbobj.CityMasters
                                                                                          where DBC.CityId == DBData.City1
                                                                                          select DBC.CityName).First() + ", " + (from DBState in dbobj.StateMasters
                                                                                                                                 where DBState.StateId == DBData.State1
                                                                                                                                 select DBState.StateName).First() + ",<br>" + (from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                where DBCounty.CountryId == DBData.Country1
                                                                                                                                                                                select DBCounty.CountryName).First() + " - " + (DBData.Zip_Postal1 != null ? DBData.Zip_Postal1 : " "),
                                         Address2 = DBData.Address2 == null ? "-" : "To: " + DBData.Address2 + ",<br>" + ((from DBC in dbobj.CityMasters
                                                                                                                           where DBC.CityId == DBData.City2
                                                                                                                           select DBC.CityName).First() != null ? (from DBC in dbobj.CityMasters
                                                                                                                                                                   where DBC.CityId == DBData.City2
                                                                                                                                                                   select DBC.CityName).First() : "") + ", " + ((from DBState in dbobj.StateMasters
                                                                                                                                                                                                                 where DBState.StateId == DBData.State2
                                                                                                                                                                                                                 select DBState.StateName).First() != null ? (from DBState in dbobj.StateMasters
                                                                                                                                                                                                                                                              where DBState.StateId == DBData.State2
                                                                                                                                                                                                                                                              select DBState.StateName).First() : " ") + ",<br>" + ((from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                                                                                                                                                     where DBCounty.CountryId == DBData.Country2
                                                                                                                                                                                                                                                                                                                     select DBCounty.CountryName).First() != null ? (from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                                                                                                                                                                                                     where DBCounty.CountryId == DBData.Country2
                                                                                                                                                                                                                                                                                                                                                                     select DBCounty.CountryName).First() : "") + " - " + (DBData.Zip_Postal1 != null ? DBData.Zip_Postal1 : " "),
                                         Website = DBData.Website == null ? "-" : DBData.Website,
                                         C_M1 = DBData.C_M1,
                                         C_M2 = DBData.C_M2,
                                         //Currency = ""
                                         Currency = DBData.CurrencyMaster.CurrencyCode + "-" + ("&#" + DBData.CurrencyMaster.CurrencySymbol)
                                     };
                        if (Client.Count() > 0)
                        {
                            gridClient.DataSource = Client;
                            gridClient.DataBind();
                        }
                        else
                        {
                            BlankGrid();
                        }
                    }
                }
            }
            else
            {
                if (Global.UserType == "DIRECTOR" || Global.DepartmentName == "ACCT")
                {
                    if (ddlDepartmentSearch.SelectedItem.ToString() == "-- All --")
                    {
                        var Client = from DBData in dbobj.ClientMasters
                                     where DBData.IsActive == true && DBData.ClientName.Contains(txtMasterSearch.Text.Trim())
                                     orderby DBData.DepartmentMaster.DepartmentName, DBData.ClientName
                                     select new
                                     {
                                         ClientId = DBData.ClientId,
                                         ClientName = DBData.ClientName,
                                         DepartmentId = DBData.DepartmentId,
                                         Department = DBData.DepartmentMaster.DepartmentName,
                                         Phone = DBData.Phone,
                                         Address1 = "To: " + DBData.Address1 + ",<br>" + (from DBC in dbobj.CityMasters
                                                                                          where DBC.CityId == DBData.City1
                                                                                          select DBC.CityName).First() + ", " + (from DBState in dbobj.StateMasters
                                                                                                                                 where DBState.StateId == DBData.State1
                                                                                                                                 select DBState.StateName).First() + ",<br>" + (from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                where DBCounty.CountryId == DBData.Country1
                                                                                                                                                                                select DBCounty.CountryName).First() + " - " + (DBData.Zip_Postal1 != null ? DBData.Zip_Postal1 : " "),
                                         Address2 = DBData.Address2 == null ? "-" : "To: " + DBData.Address2 + ",<br>" + ((from DBC in dbobj.CityMasters
                                                                                                                           where DBC.CityId == DBData.City2
                                                                                                                           select DBC.CityName).First() != null ? (from DBC in dbobj.CityMasters
                                                                                                                                                                   where DBC.CityId == DBData.City2
                                                                                                                                                                   select DBC.CityName).First() : "") + ", " + ((from DBState in dbobj.StateMasters
                                                                                                                                                                                                                 where DBState.StateId == DBData.State2
                                                                                                                                                                                                                 select DBState.StateName).First() != null ? (from DBState in dbobj.StateMasters
                                                                                                                                                                                                                                                              where DBState.StateId == DBData.State2
                                                                                                                                                                                                                                                              select DBState.StateName).First() : " ") + ",<br>" + ((from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                                                                                                                                                     where DBCounty.CountryId == DBData.Country2
                                                                                                                                                                                                                                                                                                                     select DBCounty.CountryName).First() != null ? (from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                                                                                                                                                                                                     where DBCounty.CountryId == DBData.Country2
                                                                                                                                                                                                                                                                                                                                                                     select DBCounty.CountryName).First() : "") + " - " + (DBData.Zip_Postal1 != null ? DBData.Zip_Postal1 : " "),
                                         Website = DBData.Website == null ? "-" : DBData.Website,
                                         C_M1 = DBData.C_M1,
                                         C_M2 = DBData.C_M2,
                                         //Currency = ""
                                         Currency = DBData.CurrencyMaster.CurrencyCode + "-" + ("&#" + DBData.CurrencyMaster.CurrencySymbol)
                                     };
                        if (Client.Count() > 0)
                        {
                            gridClient.DataSource = Client;
                            gridClient.DataBind();
                        }
                        else
                        {
                            BlankGrid();
                        }
                    }
                    else
                    {
                        var Client = from DBData in dbobj.ClientMasters
                                     where DBData.IsActive == true && DBData.DepartmentId == Convert.ToInt64(ddlDepartmentSearch.SelectedValue) && DBData.ClientName.Contains(txtMasterSearch.Text.Trim())
                                     orderby DBData.DepartmentMaster.DepartmentName, DBData.ClientName
                                     select new
                                     {
                                         ClientId = DBData.ClientId,
                                         ClientName = DBData.ClientName,
                                         DepartmentId = DBData.DepartmentId,
                                         Department = DBData.DepartmentMaster.DepartmentName,
                                         Phone = DBData.Phone,
                                         Address1 = "To: " + DBData.Address1 + ",<br>" + (from DBC in dbobj.CityMasters
                                                                                          where DBC.CityId == DBData.City1
                                                                                          select DBC.CityName).First() + ", " + (from DBState in dbobj.StateMasters
                                                                                                                                 where DBState.StateId == DBData.State1
                                                                                                                                 select DBState.StateName).First() + ",<br>" + (from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                where DBCounty.CountryId == DBData.Country1
                                                                                                                                                                                select DBCounty.CountryName).First() + " - " + (DBData.Zip_Postal1 != null ? DBData.Zip_Postal1 : " "),
                                         Address2 = DBData.Address2 == null ? "-" : "To: " + DBData.Address2 + ",<br>" + ((from DBC in dbobj.CityMasters
                                                                                                                           where DBC.CityId == DBData.City2
                                                                                                                           select DBC.CityName).First() != null ? (from DBC in dbobj.CityMasters
                                                                                                                                                                   where DBC.CityId == DBData.City2
                                                                                                                                                                   select DBC.CityName).First() : "") + ", " + ((from DBState in dbobj.StateMasters
                                                                                                                                                                                                                 where DBState.StateId == DBData.State2
                                                                                                                                                                                                                 select DBState.StateName).First() != null ? (from DBState in dbobj.StateMasters
                                                                                                                                                                                                                                                              where DBState.StateId == DBData.State2
                                                                                                                                                                                                                                                              select DBState.StateName).First() : " ") + ",<br>" + ((from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                                                                                                                                                     where DBCounty.CountryId == DBData.Country2
                                                                                                                                                                                                                                                                                                                     select DBCounty.CountryName).First() != null ? (from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                                                                                                                                                                                                     where DBCounty.CountryId == DBData.Country2
                                                                                                                                                                                                                                                                                                                                                                     select DBCounty.CountryName).First() : "") + " - " + (DBData.Zip_Postal1 != null ? DBData.Zip_Postal1 : " "),
                                         Website = DBData.Website == null ? "-" : DBData.Website,
                                         C_M1 = DBData.C_M1,
                                         C_M2 = DBData.C_M2,
                                         //Currency = ""
                                         Currency = DBData.CurrencyMaster.CurrencyCode + "-" + ("&#" + DBData.CurrencyMaster.CurrencySymbol)
                                     };
                        if (Client.Count() > 0)
                        {
                            gridClient.DataSource = Client;
                            gridClient.DataBind();
                        }
                        else
                        {
                            BlankGrid();
                        }
                    }
                }
                else
                {
                    if (ddlDepartmentSearch.SelectedItem.ToString() == "-- All --")
                    {
                        var Client = from DBData in dbobj.ClientMasters
                                     where DBData.IsActive == true && DBData.DepartmentMaster.UserId == Convert.ToInt64(Global.UserId) && DBData.ClientName.Contains(txtMasterSearch.Text.Trim())
                                     orderby DBData.DepartmentMaster.DepartmentName, DBData.ClientName
                                     select new
                                     {
                                         ClientId = DBData.ClientId,
                                         ClientName = DBData.ClientName,
                                         DepartmentId = DBData.DepartmentId,
                                         Department = DBData.DepartmentMaster.DepartmentName,
                                         Phone = DBData.Phone,
                                         Address1 = "To: " + DBData.Address1 + ",<br>" + (from DBC in dbobj.CityMasters
                                                                                          where DBC.CityId == DBData.City1
                                                                                          select DBC.CityName).First() + ", " + (from DBState in dbobj.StateMasters
                                                                                                                                 where DBState.StateId == DBData.State1
                                                                                                                                 select DBState.StateName).First() + ",<br>" + (from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                where DBCounty.CountryId == DBData.Country1
                                                                                                                                                                                select DBCounty.CountryName).First() + " - " + (DBData.Zip_Postal1 != null ? DBData.Zip_Postal1 : " "),
                                         Address2 = DBData.Address2 == null ? "-" : "To: " + DBData.Address2 + ",<br>" + ((from DBC in dbobj.CityMasters
                                                                                                                           where DBC.CityId == DBData.City2
                                                                                                                           select DBC.CityName).First() != null ? (from DBC in dbobj.CityMasters
                                                                                                                                                                   where DBC.CityId == DBData.City2
                                                                                                                                                                   select DBC.CityName).First() : "") + ", " + ((from DBState in dbobj.StateMasters
                                                                                                                                                                                                                 where DBState.StateId == DBData.State2
                                                                                                                                                                                                                 select DBState.StateName).First() != null ? (from DBState in dbobj.StateMasters
                                                                                                                                                                                                                                                              where DBState.StateId == DBData.State2
                                                                                                                                                                                                                                                              select DBState.StateName).First() : " ") + ",<br>" + ((from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                                                                                                                                                     where DBCounty.CountryId == DBData.Country2
                                                                                                                                                                                                                                                                                                                     select DBCounty.CountryName).First() != null ? (from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                                                                                                                                                                                                     where DBCounty.CountryId == DBData.Country2
                                                                                                                                                                                                                                                                                                                                                                     select DBCounty.CountryName).First() : "") + " - " + (DBData.Zip_Postal1 != null ? DBData.Zip_Postal1 : " "),
                                         Website = DBData.Website == null ? "-" : DBData.Website,
                                         C_M1 = DBData.C_M1,
                                         C_M2 = DBData.C_M2,
                                         //Currency = ""
                                         Currency = DBData.CurrencyMaster.CurrencyCode + "-" + ("&#" + DBData.CurrencyMaster.CurrencySymbol)
                                     };
                        if (Client.Count() > 0)
                        {
                            gridClient.DataSource = Client;
                            gridClient.DataBind();
                        }
                        else
                        {
                            BlankGrid();
                        }
                    }
                    else
                    {
                        var Client = from DBData in dbobj.ClientMasters
                                     where DBData.IsActive == true && DBData.DepartmentMaster.UserId == Convert.ToInt64(Global.UserId) && DBData.DepartmentId == Convert.ToInt64(ddlDepartmentSearch.SelectedValue) && DBData.ClientName.Contains(txtMasterSearch.Text.Trim())
                                     orderby DBData.DepartmentMaster.DepartmentName, DBData.ClientName
                                     select new
                                     {
                                         ClientId = DBData.ClientId,
                                         ClientName = DBData.ClientName,
                                         DepartmentId = DBData.DepartmentId,
                                         Department = DBData.DepartmentMaster.DepartmentName,
                                         Phone = DBData.Phone,
                                         Address1 = "To: " + DBData.Address1 + ",<br>" + (from DBC in dbobj.CityMasters
                                                                                          where DBC.CityId == DBData.City1
                                                                                          select DBC.CityName).First() + ", " + (from DBState in dbobj.StateMasters
                                                                                                                                 where DBState.StateId == DBData.State1
                                                                                                                                 select DBState.StateName).First() + ",<br>" + (from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                where DBCounty.CountryId == DBData.Country1
                                                                                                                                                                                select DBCounty.CountryName).First() + " - " + (DBData.Zip_Postal1 != null ? DBData.Zip_Postal1 : " "),
                                         Address2 = DBData.Address2 == null ? "-" : "To: " + DBData.Address2 + ",<br>" + ((from DBC in dbobj.CityMasters
                                                                                                                           where DBC.CityId == DBData.City2
                                                                                                                           select DBC.CityName).First() != null ? (from DBC in dbobj.CityMasters
                                                                                                                                                                   where DBC.CityId == DBData.City2
                                                                                                                                                                   select DBC.CityName).First() : "") + ", " + ((from DBState in dbobj.StateMasters
                                                                                                                                                                                                                 where DBState.StateId == DBData.State2
                                                                                                                                                                                                                 select DBState.StateName).First() != null ? (from DBState in dbobj.StateMasters
                                                                                                                                                                                                                                                              where DBState.StateId == DBData.State2
                                                                                                                                                                                                                                                              select DBState.StateName).First() : " ") + ",<br>" + ((from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                                                                                                                                                     where DBCounty.CountryId == DBData.Country2
                                                                                                                                                                                                                                                                                                                     select DBCounty.CountryName).First() != null ? (from DBCounty in dbobj.CountryMasters
                                                                                                                                                                                                                                                                                                                                                                     where DBCounty.CountryId == DBData.Country2
                                                                                                                                                                                                                                                                                                                                                                     select DBCounty.CountryName).First() : "") + " - " + (DBData.Zip_Postal1 != null ? DBData.Zip_Postal1 : " "),
                                         Website = DBData.Website == null ? "-" : DBData.Website,
                                         C_M1 = DBData.C_M1,
                                         C_M2 = DBData.C_M2,
                                         //Currency = ""
                                         Currency = DBData.CurrencyMaster.CurrencyCode + "-" + ("&#" + DBData.CurrencyMaster.CurrencySymbol)
                                     };
                        if (Client.Count() > 0)
                        {
                            gridClient.DataSource = Client;
                            gridClient.DataBind();
                        }
                        else
                        {
                            BlankGrid();
                        }
                    }
                }
            }
        }
        else
            BlankGrid();
    }

    protected void BlankGrid()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ClientId");
        dt.Columns.Add("ClientName");
        dt.Columns.Add("DepartmentId");
        dt.Columns.Add("Department");
        dt.Columns.Add("Phone");
        dt.Columns.Add("Address1");
        dt.Columns.Add("Address2");
        dt.Columns.Add("Website");
        dt.Columns.Add("C_M1"); //// Added by Jignesh on 20-Jul-2020
        dt.Columns.Add("C_M2"); //// Added by Jignesh on 20-Jul-2020
        dt.Columns.Add("Currency");

        //dt.Columns.Add("ClientId");
        //dt.Columns.Add("ClientName");
        //dt.Columns.Add("Phone");
        //dt.Columns.Add("Address1");
        //dt.Columns.Add("Address2");
        //dt.Columns.Add("Website");
        //dt.Columns.Add("DepartmentId");
        //dt.Columns.Add("Department");
        //dt.Columns.Add("Currency");
        gridClient.DataSource = dt;
        dt.Rows.Add(dt.NewRow());
        gridClient.DataBind();
        int TotalCols = gridClient.Rows[0].Cells.Count;
        gridClient.Rows[0].Cells.Clear();
        gridClient.Rows[0].Cells.Add(new TableCell());
        gridClient.Rows[0].Cells[0].ColumnSpan = TotalCols;
        gridClient.Rows[0].Cells[0].Text = "No records to display";
    }

    protected void Clear()
    {
        txtClientName.Text = "";
        ddlDepartment.Items.Clear();
        txtPrimaryEmail.Text = "";
        txtSkypeId.Text = "";
        txtPhone.Text = "";
        txtMobile.Text = "";
        txtFax.Text = "";
        txtAddress1.Text = "";
        ddlCountry1.Items.Clear();
        ddlState1.Items.Clear();
        ddlCity1.Items.Clear();
        txtZipCode1.Text = "";
        txtAddress2.Text = "";
        ddlCountry2.Items.Clear();
        ddlState2.Items.Clear();
        ddlCity2.Items.Clear();
        txtZipCode2.Text = "";
        txtWebsite.Text = "";

        UserM1_M2_Visible(); //// Global.UserM1 and Global.UserM2

        ddlCountryVisible(""); //// New change By Jignesh on 13-Jul-2022

        // // if (Global.UserM1 && Global.UserM2)
        // // {
        // //     chkClient_M1.Checked = true;
        // //     chkClient_M1.Visible = true;
        // //     lblClient_M1.Visible = true;
        // // 
        // //     chkClient_M2.Checked = true;
        // //     chkClient_M2.Visible = true;
        // //     lblClient_M2.Visible = true;
        // // }
        // // else
        // // {
        // //     if (Global.UserM1)
        // //     {
        // //         chkClient_M1.Checked = true;
        // //         chkClient_M1.Visible = false;
        // //         lblClient_M1.Visible = false;
        // //     }
        // //     else
        // //     {
        // //         chkClient_M1.Checked = false;
        // //         chkClient_M1.Visible = false;
        // //         lblClient_M1.Visible = false;
        // //     }
        // //     if (Global.UserM2)
        // //     {
        // //         chkClient_M2.Checked = true;
        // //         chkClient_M2.Visible = false;
        // //         lblClient_M2.Visible = false;
        // //     }
        // //     else
        // //     {
        // //         chkClient_M2.Checked = false;
        // //         chkClient_M2.Visible = false;
        // //         lblClient_M2.Visible = false;
        // //     }
        // // }

        ddlCurrency.Items.Clear();
        txtRemarks.Text = "";
        lbtnAddState.Visible = false;
        lbtnAddState2.Visible = false;
        lbtnAddCity.Visible = false;
        lbtnAddCity2.Visible = false;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Page page1 = HttpContext.Current.Handler as Page;
        if (btnSave.Text == "Save")
        {
            ClientMaster cm = new ClientMaster();
            cm.ClientName = txtClientName.Text.Trim();
            cm.PrimaryEmail = txtPrimaryEmail.Text.Trim();
            if (txtSkypeId.Text != "")
                cm.Skype = txtSkypeId.Text.Trim();

            ////////This part added for default 'Recipient' and 'CC' email set To SendInvoiceEmail
            //////// By Jignesh on 29-May-2019
            if (txtRecipientEmail.Text != "")
                cm.DefaultRecipient = txtRecipientEmail.Text.Trim();

            if (txtCCEmail.Text != "")
                cm.DefaultCC = txtCCEmail.Text.Trim();
            ////////////////

            cm.Phone = txtPhone.Text.Trim();
            if (txtMobile.Text != "")
                cm.Mobile = txtMobile.Text.Trim();
            if (txtFax.Text != "")
                cm.Fax = txtFax.Text.Trim();
            cm.Address1 = txtAddress1.Text.Trim();
            cm.Country1 = Convert.ToInt64(ddlCountry1.SelectedValue);
            cm.State1 = Convert.ToInt64(ddlState1.SelectedValue);
            cm.City1 = Convert.ToInt64(ddlCity1.SelectedValue);
            if (txtZipCode1.Text != "")
                cm.Zip_Postal1 = txtZipCode1.Text.Trim();
            if (txtAddress2.Text != "")
                cm.Address2 = txtAddress2.Text.Trim();
            if (ddlCountry2.SelectedItem.ToString() != "-- Select --")
                cm.Country2 = Convert.ToInt64(ddlCountry2.SelectedValue);
            if (ddlState2.SelectedItem.ToString() != "-- Select --")
                cm.State2 = Convert.ToInt64(ddlState2.SelectedValue);
            if (ddlCity2.SelectedItem.ToString() != "-- Select --")
                cm.City2 = Convert.ToInt64(ddlCity2.SelectedValue);
            if (txtZipCode2.Text != "")
                cm.Zip_Postal2 = txtZipCode2.Text.Trim();
            if (txtWebsite.Text != "")
                cm.Website = txtWebsite.Text.Trim();
            cm.DepartmentId = Convert.ToInt64(ddlDepartment.SelectedValue);
            if (ddlCurrency.SelectedItem.ToString() != "-- Select --")
                cm.CurrencyId = Convert.ToInt64(ddlCurrency.SelectedValue);
            else
                cm.CurrencyId = null;
            if (txtRemarks.Text != "")
                cm.Remarks = txtRemarks.Text.Trim();
            cm.IsActive = true;

            //// For multiple access
            cm.C_M1 = chkClient_M1.Checked; //// New change By Jignesh on 20-Jul-2020
            cm.C_M2 = chkClient_M2.Checked; //// New change By Jignesh on 20-Jul-2020

            cm.GSTIN = txtGSTIN.Text.Trim(); //// New change By Jignesh on 13-Jul-2022

            cm.CreatedBy = Convert.ToInt64(Global.UserId);
            cm.CreatedDate = DateTime.Now;
            dbobj.ClientMasters.InsertOnSubmit(cm);
            dbobj.SubmitChanges();
            dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
            ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('Client added successfully')", true);
        }
        if (btnSave.Text == "Update")
        {
            var UpClient = from DBData in dbobj.ClientMasters
                           where DBData.IsActive == true && DBData.ClientId == Convert.ToInt64(ViewState["ClientId"].ToString())
                           select DBData;
            var SingleClient = UpClient.Single();
            SingleClient.ClientName = txtClientName.Text.Trim();
            SingleClient.PrimaryEmail = txtPrimaryEmail.Text.Trim();
            if (txtSkypeId.Text != "")
                SingleClient.Skype = txtSkypeId.Text.Trim();
            else
                SingleClient.Skype = null;

            ////////This part added for default 'Recipient' and 'CC' email set To SendInvoiceEmail
            //////// By Jignesh on 29-May-2019
            if (txtRecipientEmail.Text != "")
                SingleClient.DefaultRecipient = txtRecipientEmail.Text.Trim();
            else
                SingleClient.DefaultRecipient = null;

            if (txtCCEmail.Text != "")
                SingleClient.DefaultCC = txtCCEmail.Text.Trim();
            else
                SingleClient.DefaultCC = null;
            ////////////////

            SingleClient.Phone = txtPhone.Text.Trim();
            if (txtMobile.Text != "")
                SingleClient.Mobile = txtMobile.Text.Trim();
            else
                SingleClient.Mobile = null;
            if (txtFax.Text != "")
                SingleClient.Fax = txtFax.Text.Trim();
            else
                SingleClient.Fax = null;
            SingleClient.Address1 = txtAddress1.Text.Trim();
            SingleClient.Country1 = Convert.ToInt64(ddlCountry1.SelectedValue);
            SingleClient.State1 = Convert.ToInt64(ddlState1.SelectedValue);
            SingleClient.City1 = Convert.ToInt64(ddlCity1.SelectedValue);
            if (txtZipCode1.Text != "")
                SingleClient.Zip_Postal1 = txtZipCode1.Text.Trim();
            else
                SingleClient.Zip_Postal1 = null;
            if (txtAddress2.Text != "")
                SingleClient.Address2 = txtAddress2.Text.Trim();
            else
                SingleClient.Address2 = null;
            if (ddlCountry2.SelectedItem.ToString() != "-- Select --")
                SingleClient.Country2 = Convert.ToInt64(ddlCountry2.SelectedValue);
            else
                SingleClient.Country2 = null;
            if (ddlState2.SelectedItem.ToString() != "-- Select --")
                SingleClient.State2 = Convert.ToInt64(ddlState2.SelectedValue);
            else
                SingleClient.State2 = null;
            if (ddlCity2.SelectedItem.ToString() != "-- Select --")
                SingleClient.City2 = Convert.ToInt64(ddlCity2.SelectedValue);
            else
                SingleClient.City2 = null;
            if (txtZipCode2.Text != "")
                SingleClient.Zip_Postal2 = txtZipCode2.Text.Trim();
            else
                SingleClient.Zip_Postal2 = null;
            if (txtWebsite.Text != "")
                SingleClient.Website = txtWebsite.Text.Trim();
            else
                SingleClient.Website = null;
            SingleClient.DepartmentId = Convert.ToInt64(ddlDepartment.SelectedValue);
            if (ddlCurrency.SelectedItem.ToString() != "-- Select --")
                SingleClient.CurrencyId = Convert.ToInt64(ddlCurrency.SelectedValue);
            else
                SingleClient.CurrencyId = null;
            if (txtRemarks.Text != "")
                SingleClient.Remarks = txtRemarks.Text.Trim();
            else
                SingleClient.Remarks = null;

            //// For multiple access
            SingleClient.C_M1 = chkClient_M1.Checked; //// New change By Jignesh on 20-Jul-2020
            SingleClient.C_M2 = chkClient_M2.Checked; //// New change By Jignesh on 20-Jul-2020

            SingleClient.GSTIN = txtGSTIN.Text.Trim(); //// New change By Jignesh on 13-Jul-2022

            SingleClient.ModifyBy = Convert.ToInt64(Global.UserId);
            SingleClient.ModifyDate = DateTime.Now;
            dbobj.SubmitChanges();
            dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
            btnSave.Text = "Save";
        }
        mvClient.ActiveViewIndex = 0;
        FillGrid();
        Clear();
    }

    protected void ddlCountryVisible(string selectedCountryName)
    {
        //if (ddlCountry1.SelectedItem.Text.ToUpper().ToString() == "INDIA")
        if (selectedCountryName.ToUpper() == "INDIA")
        {
            lblGSTIN.Visible = true;
            lblGSTINReq.Visible = true;
            txtGSTIN.Visible = true;
            rqGSTIN.Visible = true;
            revGSTIN.Visible = true;
            rqGSTIN.ValidationGroup = "vg";
            revGSTIN.ValidationGroup = "vg";
        }
        else
        {
            lblGSTIN.Visible = false;
            lblGSTINReq.Visible = false;
            txtGSTIN.Visible = false;
            txtGSTIN.Text = "";
            rqGSTIN.Visible= false;
            revGSTIN.Visible = false; 
            rqGSTIN.ValidationGroup = "";
            revGSTIN.ValidationGroup = "";
        }
    }

    protected void ddlCountry1_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillState1();
        FillCity1();
        if (ddlCountry1.SelectedItem.ToString() != "-- Select --")
        {
            lbtnAddState.Visible = true;
            lblAddDelCountry.Visible = true;
            lbtnDelCountry.Visible = true;
            lblAddDelState1.Visible = false;
            lbtnDelState1.Visible = false;
            lbtnAddCity.Visible = false;
            lblAddDelCity1.Visible = false;
            lbtnDelCity1.Visible = false;
        }
        else
        {
            lblAddDelCountry.Visible = false;
            lbtnDelCountry.Visible = false;
            lbtnAddState.Visible = false;
            lblAddDelState1.Visible = false;
            lbtnDelState1.Visible = false;
            lbtnAddCity.Visible = false;
            lblAddDelCity1.Visible = false;
            lbtnDelCity1.Visible = false;
        }
        //// New change By Jignesh on 13-Jul-2022
        ddlCountryVisible(ddlCountry1.SelectedItem.Text.ToUpper().ToString());

        ddlCountry1.Focus();
    }

    protected void ddlCountry2_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillState2();
        FillCity2();
        if (ddlCountry2.SelectedItem.ToString() != "-- Select --")
        {
            lbtnAddState2.Visible = true;
            lblAddDelCountry2.Visible = true;
            lbtnDelCountry2.Visible = true;
            lblAddDelState2.Visible = false;
            lbtnDelState2.Visible = false;
            lbtnAddCity2.Visible = false;
            lblAddDelCity2.Visible = false;
            lbtnDelCity2.Visible = false;
        }
        else
        {
            lblAddDelCountry2.Visible = false;
            lbtnDelCountry2.Visible = false;
            lbtnAddState2.Visible = false;
            lblAddDelState2.Visible = false;
            lbtnDelState2.Visible = false;
            lbtnAddCity2.Visible = false;
            lblAddDelCity2.Visible = false;
            lbtnDelCity2.Visible = false;
        }
        ddlCountry2.Focus();
    }

    protected void ddlState1_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCity1();
        if (ddlState1.SelectedItem.ToString() != "-- Select --")
        {
            lbtnAddCity.Visible = true;
            lblAddDelState1.Visible = true;
            lbtnDelState1.Visible = true;
            lblAddDelState1.Visible = true;
            lbtnDelState1.Visible = true;
            lblAddDelCity1.Visible = false;
            lbtnDelCity1.Visible = false;
        }
        else
        {
            lblAddDelState1.Visible = false;
            lbtnDelState1.Visible = false;
            lbtnAddCity.Visible = false;
            lblAddDelCity1.Visible = false;
            lbtnDelCity1.Visible = false;
        }
        ddlState1.Focus();
    }

    protected void ddlState2_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCity2();
        if (ddlState2.SelectedItem.ToString() != "-- Select --")
        {
            lbtnAddCity2.Visible = true;
            lblAddDelState2.Visible = true;
            lbtnDelState2.Visible = true;
            lblAddDelState2.Visible = true;
            lbtnDelState2.Visible = true;
            lblAddDelCity2.Visible = false;
            lbtnDelCity2.Visible = false;
        }
        else
        {
            lblAddDelState2.Visible = false;
            lbtnDelState2.Visible = false;
            lbtnAddCity2.Visible = false;
            lblAddDelCity2.Visible = false;
            lbtnDelCity2.Visible = false;
        }
        ddlState2.Focus();
    }

    protected void ddlCity1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCity1.SelectedItem.ToString() != "-- Select --")
        {
            lblAddDelCity1.Visible = true;
            lbtnDelCity1.Visible = true;
        }
        else
        {
            lblAddDelCity1.Visible = false;
            lbtnDelCity1.Visible = false;
        }
    }

    protected void ddlCity2_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCity2.SelectedItem.ToString() != "-- Select --")
        {
            lblAddDelCity2.Visible = true;
            lbtnDelCity2.Visible = true;
        }
        else
        {
            lblAddDelCity2.Visible = false;
            lbtnDelCity2.Visible = false;
        }
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        Page page1 = HttpContext.Current.Handler as Page;
        if (ViewState["Table"].ToString() == "Country")
        {
            var DupCountry = from DupC in dbobj.CountryMasters
                             where DupC.CountryName == txtName.Text.ToUpper()
                             select DupC;
            if (DupCountry.Count() == 0)
            {
                CountryMaster cm = new CountryMaster();
                cm.CountryName = txtName.Text.ToUpper();
                dbobj.CountryMasters.InsertOnSubmit(cm);
                dbobj.SubmitChanges();
                dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                //ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + txtName.Text.Trim().ToUpper() + " country added successfully" + "')", true);
                string Country1 = null;
                if (ddlCountry1.SelectedIndex != 0)
                    Country1 = ddlCountry1.SelectedValue;
                string Country2 = null;
                if (ddlCountry2.SelectedIndex != 0)
                    Country2 = ddlCountry2.SelectedValue;
                FillCountry();
                if (Country1 != null && ddlCountry1.Items.FindByValue(Country1) != null)
                    ddlCountry1.Items.FindByValue(Country1).Selected = true;
                if (Country2 != null && ddlCountry2.Items.FindByValue(Country2) != null)
                    ddlCountry2.Items.FindByValue(Country2).Selected = true;
                //ModalPopupExtender1.Hide();
            }
            else
            {
                ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + txtName.Text.Trim().ToUpper() + " country already exists" + "')", true);
                ModalPopupExtender1.Show();
            }
        }
        if (ViewState["Table"].ToString() == "State")
        {
            var DupState = from DupS in dbobj.StateMasters
                           where DupS.StateName == txtName.Text.ToUpper()
                           select DupS;
            if (DupState.Count() == 0)
            {
                StateMaster sm = new StateMaster();
                sm.StateName = txtName.Text.ToUpper();
                sm.CountryId = Convert.ToInt64(ddlCountry1.SelectedValue);
                dbobj.StateMasters.InsertOnSubmit(sm);
                dbobj.SubmitChanges();
                dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                //ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + txtName.Text.Trim().ToUpper() + " state add in " + ddlCountry1.SelectedItem.ToString().ToUpper() + "')", true);
                string State1 = null;
                if (ddlState1.SelectedIndex != 0)
                    State1 = ddlState1.SelectedValue;
                string State2 = null;
                if (ddlState2.SelectedIndex != 0)
                    State2 = ddlState2.SelectedValue;
                FillState1();
                FillState2();
                if (State1 != null && ddlState1.Items.FindByValue(State1) != null)
                    ddlState1.Items.FindByValue(State1).Selected = true;
                if (State2 != null && ddlState2.Items.FindByValue(State2) != null)
                    ddlState2.Items.FindByValue(State2).Selected = true;
            }
            else
            {
                ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + DupState.Single().StateName + " state already exists in " + DupState.Single().CountryMaster.CountryName + "')", true);
                ModalPopupExtender1.Show();
            }
        }
        if (ViewState["Table"].ToString() == "State2")
        {
            var DupState = from DupS in dbobj.StateMasters
                           where DupS.StateName == txtName.Text.ToUpper()
                           select DupS;
            if (DupState.Count() == 0)
            {
                StateMaster sm = new StateMaster();
                sm.StateName = txtName.Text.ToUpper();
                sm.CountryId = Convert.ToInt64(ddlCountry2.SelectedValue);
                dbobj.StateMasters.InsertOnSubmit(sm);
                dbobj.SubmitChanges();
                dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                //ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + txtName.Text.Trim().ToUpper() + " state add in " + ddlCountry1.SelectedItem.ToString().ToUpper() + "')", true);
                string State1 = null;
                if (ddlState1.SelectedIndex != 0)
                    State1 = ddlState1.SelectedValue;
                string State2 = null;
                if (ddlState2.SelectedIndex != 0)
                    State2 = ddlState2.SelectedValue;
                FillState1();
                FillState2();
                if (State1 != null && ddlState1.Items.FindByValue(State1) != null)
                    ddlState1.Items.FindByValue(State1).Selected = true;
                if (State2 != null && ddlState2.Items.FindByValue(State2) != null)
                    ddlState2.Items.FindByValue(State2).Selected = true;
            }
            else
            {
                ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + DupState.Single().StateName + " state already exists in " + DupState.Single().CountryMaster.CountryName + "')", true);
                ModalPopupExtender1.Show();
            }
        }
        if (ViewState["Table"].ToString() == "City")
        {
            var DupCity = from DupC in dbobj.CityMasters
                          where DupC.CityName == txtName.Text.ToUpper() && DupC.StateId == Convert.ToInt64(ddlState1.SelectedValue) && DupC.CountryId == Convert.ToInt64(ddlCountry1.SelectedValue)
                          select DupC;
            if (DupCity.Count() == 0)
            {
                CityMaster cm = new CityMaster();
                cm.CityName = txtName.Text.ToUpper();
                cm.StateId = Convert.ToInt64(ddlState1.SelectedValue);
                cm.CountryId = Convert.ToInt64(ddlCountry1.SelectedValue);
                dbobj.CityMasters.InsertOnSubmit(cm);
                dbobj.SubmitChanges();
                dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                //ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + DupCity.Single().CityName.ToUpper() + " city add in " + DupCity.Single().StateMaster.StateName + " (" + DupCity.Single().CountryMaster.CountryName + ")" + "')", true);
                string City1 = null;
                if (ddlCity1.SelectedIndex != 0)
                    City1 = ddlCity1.SelectedValue;
                string City2 = null;
                if (ddlCity2.SelectedIndex != 0)
                    City2 = ddlCity2.SelectedValue;
                FillCity1();
                FillCity2();
                if (City1 != null && ddlCity1.Items.FindByValue(City1) != null)
                    ddlCity1.Items.FindByValue(City1).Selected = true;
                if (City2 != null && ddlCity2.Items.FindByValue(City2) != null)
                    ddlCity2.Items.FindByValue(City2).Selected = true;
            }
            else
            {
                ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + DupCity.Single().CityName.ToUpper() + " city already exists in " + DupCity.Single().StateMaster.StateName + " (" + DupCity.Single().CountryMaster.CountryName + ")" + "')", true);
                ModalPopupExtender1.Show();
            }
        }
        if (ViewState["Table"].ToString() == "City2")
        {
            var DupCity = from DupC in dbobj.CityMasters
                          where DupC.CityName == txtName.Text.ToUpper() && DupC.StateId == Convert.ToInt64(ddlState2.SelectedValue) && DupC.CountryId == Convert.ToInt64(ddlCountry2.SelectedValue)
                          select DupC;
            if (DupCity.Count() == 0)
            {
                CityMaster cm = new CityMaster();
                cm.CityName = txtName.Text.ToUpper();
                cm.StateId = Convert.ToInt64(ddlState2.SelectedValue);
                cm.CountryId = Convert.ToInt64(ddlCountry2.SelectedValue);
                dbobj.CityMasters.InsertOnSubmit(cm);
                dbobj.SubmitChanges();
                dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                //ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + DupCity.Single().CityName.ToUpper() + " city add in " + DupCity.Single().StateMaster.StateName + " (" + DupCity.Single().CountryMaster.CountryName + ")" + "')", true);
                string City1 = null;
                if (ddlCity1.SelectedIndex != 0)
                    City1 = ddlCity1.SelectedValue;
                string City2 = null;
                if (ddlCity2.SelectedIndex != 0)
                    City2 = ddlCity2.SelectedValue;
                FillCity1();
                FillCity2();
                if (City1 != null && ddlCity1.Items.FindByValue(City1) != null)
                    ddlCity1.Items.FindByValue(City1).Selected = true;
                if (City2 != null && ddlCity2.Items.FindByValue(City2) != null)
                    ddlCity2.Items.FindByValue(City2).Selected = true;
            }
            else
            {
                ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + DupCity.Single().CityName.ToUpper() + " city already exists in " + DupCity.Single().StateMaster.StateName + " (" + DupCity.Single().CountryMaster.CountryName + ")" + "')", true);
                ModalPopupExtender1.Show();
            }
        }
    }

    protected void lbtnAddCountry_Click(object sender, EventArgs e)
    {
        ModalPopupExtender1.Show();
        lblTitle.Text = "Add country";
        lblName.Text = "Country name :";
        txtName.Text = "";
        ViewState["Table"] = "Country";
    }

    protected void lbtnDelCountry_Click(object sender, EventArgs e)
    {
        Page page1 = HttpContext.Current.Handler as Page;
        if (ddlCountry1.SelectedIndex != -1 && ddlCountry1.SelectedIndex != 0)
        {
            var DelCountry = from DelC in dbobj.CountryMasters
                             where DelC.CountryId == Convert.ToInt64(ddlCountry1.SelectedValue)
                             select DelC;
            if (DelCountry.Count() > 0)
            {
                var Country = DelCountry.Single();
                dbobj.CountryMasters.DeleteOnSubmit(Country);
                try
                {
                    dbobj.SubmitChanges();
                    dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                    //ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + Country.CountryName.ToUpper() + " county name deleted" + "')", true);
                    string Country1 = ddlCountry1.SelectedValue;
                    string Country2 = ddlCountry2.SelectedValue;
                    FillCountry();
                    if (ddlCountry1.Items.FindByValue(Country1) != null)
                        ddlCountry1.Items.FindByValue(Country1).Selected = true;
                    if (ddlCountry2.Items.FindByValue(Country2) != null)
                        ddlCountry2.Items.FindByValue(Country2).Selected = true;
                    if (ddlCountry1.SelectedItem.ToString() == ddlCountry2.SelectedItem.ToString())
                    {
                        lblAddDelCountry.Visible = false;
                        lbtnDelCountry.Visible = false;
                        lblAddDelCountry2.Visible = false;
                        lbtnDelCountry2.Visible = false;
                    }
                    else
                    {
                        lblAddDelCountry.Visible = false;
                        lbtnDelCountry.Visible = false;
                    }
                }
                catch
                {
                    OpenWindow(this, "MITPLInvoice", "DeletePopup?id=" + ddlCountry1.SelectedValue + "&name=" + ddlCountry1.SelectedItem + "&page=country", 650, 350);
                }
            }
        }
    }

    protected void lbtnDelCountry2_Click(object sender, EventArgs e)
    {
        Page page1 = HttpContext.Current.Handler as Page;
        if (ddlCountry2.SelectedIndex != -1 && ddlCountry2.SelectedIndex != 0)
        {
            var DelCountry = from DelC in dbobj.CountryMasters
                             where DelC.CountryId == Convert.ToInt64(ddlCountry2.SelectedValue)
                             select DelC;
            if (DelCountry.Count() > 0)
            {
                var Country = DelCountry.Single();
                dbobj.CountryMasters.DeleteOnSubmit(Country);
                try
                {
                    dbobj.SubmitChanges();
                    dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                    //ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + Country.CountryName.ToUpper() + " county name deleted" + "')", true);
                    string Country1 = ddlCountry1.SelectedValue;
                    string Country2 = ddlCountry2.SelectedValue;
                    FillCountry();
                    if (ddlCountry1.Items.FindByValue(Country1) != null)
                        ddlCountry1.Items.FindByValue(Country1).Selected = true;
                    if (ddlCountry2.Items.FindByValue(Country2) != null)
                        ddlCountry2.Items.FindByValue(Country2).Selected = true;
                    if (ddlCountry1.SelectedItem.ToString() == ddlCountry2.SelectedItem.ToString())
                    {
                        lblAddDelCountry.Visible = false;
                        lbtnDelCountry.Visible = false;
                        lblAddDelCountry2.Visible = false;
                        lbtnDelCountry2.Visible = false;
                    }
                    else
                    {
                        lblAddDelCountry2.Visible = false;
                        lbtnDelCountry2.Visible = false;
                    }
                }
                catch
                {
                    OpenWindow(this, "MITPLInvoice", "DeletePopup?id=" + ddlCountry2.SelectedValue + "&name=" + ddlCountry2.SelectedItem + "&page=country", 650, 350);
                }
            }
        }
    }

    protected void lbtnAddState_Click(object sender, EventArgs e)
    {
        Page page1 = HttpContext.Current.Handler as Page;
        if (ddlCountry1.SelectedItem.ToString() == "-- Select --")
        {
            ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('Please select country')", true);
        }
        else
        {

            ModalPopupExtender1.Show();
            lblTitle.Text = "Add state";
            lblName.Text = "State name :";
            txtName.Text = "";
            ViewState["Table"] = "State";
        }
    }

    protected void lbtnAddState2_Click(object sender, EventArgs e)
    {
        Page page1 = HttpContext.Current.Handler as Page;
        if (ddlCountry2.SelectedItem.ToString() == "-- Select --")
        {
            ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('Please select country')", true);
        }
        else
        {

            ModalPopupExtender1.Show();
            lblTitle.Text = "Add state";
            lblName.Text = "State name :";
            txtName.Text = "";
            ViewState["Table"] = "State2";
        }
    }

    protected void lbtnDelState_Click(object sender, EventArgs e)
    {
        Page page1 = HttpContext.Current.Handler as Page;
        var DelState = from DelS in dbobj.StateMasters
                       where DelS.StateId == Convert.ToInt64(ddlState1.SelectedValue)
                       select DelS;
        if (DelState.Count() > 0)
        {
            var State = DelState.Single();
            dbobj.StateMasters.DeleteOnSubmit(State);
            try
            {
                dbobj.SubmitChanges();
                dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                //ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + State.StateName + " state in " + State.CountryMaster.CountryName + " is deleted" + "')", true);
                string state1 = ddlState1.SelectedValue;
                string state2 = ddlState2.SelectedValue;
                FillState1();
                FillState2();
                if (ddlState1.Items.FindByValue(state1) != null)
                    ddlState1.Items.FindByValue(state1).Selected = true;
                if (ddlState2.Items.FindByValue(state2) != null)
                    ddlState2.Items.FindByValue(state2).Selected = true;
                if (ddlState1.SelectedItem.ToString() == ddlState2.SelectedItem.ToString())
                {
                    lblAddDelState1.Visible = false;
                    lbtnDelState1.Visible = false;
                    lblAddDelState2.Visible = false;
                    lbtnDelState2.Visible = false;
                }
                else
                {
                    lblAddDelState1.Visible = false;
                    lbtnDelState1.Visible = false;
                }
            }
            catch
            {
                OpenWindow(this, "MITPLInvoice", "DeletePopup?id=" + ddlState1.SelectedValue + "&name=" + ddlState1.SelectedItem + "&page=state", 650, 350);
            }
        }
    }

    protected void lbtnDelState2_Click(object sender, EventArgs e)
    {
        Page page1 = HttpContext.Current.Handler as Page;
        var DelState = from DelS in dbobj.StateMasters
                       where DelS.StateId == Convert.ToInt64(ddlState2.SelectedValue)
                       select DelS;
        if (DelState.Count() > 0)
        {
            var State = DelState.Single();
            dbobj.StateMasters.DeleteOnSubmit(State);
            try
            {
                dbobj.SubmitChanges();
                dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                //ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + State.StateName + " state in " + State.CountryMaster.CountryName + " is deleted" + "')", true);
                string state1 = ddlState1.SelectedValue;
                string state2 = ddlState2.SelectedValue;
                FillState1();
                FillState2();
                if (ddlState1.Items.FindByValue(state1) != null)
                    ddlState1.Items.FindByValue(state1).Selected = true;
                if (ddlState2.Items.FindByValue(state2) != null)
                    ddlState2.Items.FindByValue(state2).Selected = true;
                if (ddlState1.SelectedItem.ToString() == ddlState2.SelectedItem.ToString())
                {
                    lblAddDelState1.Visible = false;
                    lbtnDelState1.Visible = false;
                    lblAddDelState2.Visible = false;
                    lbtnDelState2.Visible = false;
                }
                else
                {
                    lblAddDelState2.Visible = false;
                    lbtnDelState2.Visible = false;
                }
            }
            catch
            {
                OpenWindow(this, "MITPLInvoice", "DeletePopup?id=" + ddlState2.SelectedValue + "&name=" + ddlState2.SelectedItem + "&page=state", 650, 350);
            }
        }
    }

    protected void lbtnAddCity_Click(object sender, EventArgs e)
    {
        Page page1 = HttpContext.Current.Handler as Page;
        if (ddlCountry1.SelectedItem.ToString() == "-- Select --")
        {
            ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('Please select country')", true);
        }
        else
        {
            if (ddlState1.SelectedItem.ToString() == "-- Select --")
            {
                ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('Please select state')", true);
            }
            else
            {
                ModalPopupExtender1.Show();
                lblTitle.Text = "Add city";
                lblName.Text = "City name :";
                txtName.Text = "";
                ViewState["Table"] = "City";
            }
        }
    }

    protected void lbtnAddCity2_Click(object sender, EventArgs e)
    {
        Page page1 = HttpContext.Current.Handler as Page;
        if (ddlCountry2.SelectedItem.ToString() == "-- Select --")
        {
            ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('Please select country')", true);
        }
        else
        {
            if (ddlState2.SelectedItem.ToString() == "-- Select --")
            {
                ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('Please select state')", true);
            }
            else
            {
                ModalPopupExtender1.Show();
                lblTitle.Text = "Add city";
                lblName.Text = "City name :";
                txtName.Text = "";
                ViewState["Table"] = "City2";
            }
        }
    }

    protected void lbtnDelCity1_Click(object sender, EventArgs e)
    {
        Page page1 = HttpContext.Current.Handler as Page;
        var DelCity = from DelC in dbobj.CityMasters
                      where DelC.CityId == Convert.ToInt64(ddlCity1.SelectedValue)
                      select DelC;
        if (DelCity.Count() > 0)
        {
            var City = DelCity.Single();
            dbobj.CityMasters.DeleteOnSubmit(City);
            try
            {
                dbobj.SubmitChanges();
                dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                //ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + City.CityName + " city in " + City.StateMaster.StateName.ToUpper() + " (" + City.CountryMaster.CountryName.ToUpper() + ")" + " is deleted" + "')", true);
                string city1 = ddlCity1.SelectedValue;
                string city2 = ddlCity2.SelectedValue;
                FillCity1();
                FillCity2();
                if (ddlCity1.Items.FindByValue(city1) != null)
                    ddlCity1.Items.FindByValue(city1).Selected = true;
                if (ddlCity2.Items.FindByValue(city2) != null)
                    ddlCity2.Items.FindByValue(city2).Selected = true;
                if (ddlCity1.SelectedItem.ToString() == ddlCity2.SelectedItem.ToString())
                {
                    lblAddDelCity1.Visible = false;
                    lbtnDelCity1.Visible = false;
                    lblAddDelCity2.Visible = false;
                    lbtnDelCity2.Visible = false;
                }
                else
                {
                    lblAddDelCity1.Visible = false;
                    lbtnDelCity1.Visible = false;
                }
            }
            catch
            {
                OpenWindow(this, "MITPLInvoice", "DeletePopup?id=" + ddlCity1.SelectedValue + "&name=" + ddlCity1.SelectedItem + "&page=city", 650, 350);
            }
        }
    }

    protected void lbtnDelCity2_Click(object sender, EventArgs e)
    {
        Page page1 = HttpContext.Current.Handler as Page;
        var DelCity = from DelC in dbobj.CityMasters
                      where DelC.CityId == Convert.ToInt64(ddlCity2.SelectedValue)
                      select DelC;
        if (DelCity.Count() > 0)
        {
            var City = DelCity.Single();
            dbobj.CityMasters.DeleteOnSubmit(City);
            try
            {
                dbobj.SubmitChanges();
                dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                //ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + City.CityName + " city in " + City.StateMaster.StateName.ToUpper() + " (" + City.CountryMaster.CountryName.ToUpper() + ")" + " is deleted" + "')", true);
                string city1 = ddlCity1.SelectedValue;
                string city2 = ddlCity2.SelectedValue;
                FillCity1();
                FillCity2();
                if (ddlCity1.Items.FindByValue(city1) != null)
                    ddlCity1.Items.FindByValue(city1).Selected = true;
                if (ddlCity2.Items.FindByValue(city2) != null)
                    ddlCity2.Items.FindByValue(city2).Selected = true;
                if (ddlCity1.SelectedItem.ToString() == ddlCity2.SelectedItem.ToString())
                {
                    lblAddDelCity1.Visible = false;
                    lbtnDelCity1.Visible = false;
                    lblAddDelCity2.Visible = false;
                    lbtnDelCity2.Visible = false;
                }
                else
                {
                    lblAddDelCity2.Visible = false;
                    lbtnDelCity2.Visible = false;
                }
            }
            catch
            {
                OpenWindow(this, "MITPLInvoice", "DeletePopup?id=" + ddlCity2.SelectedValue + "&name=" + ddlCity2.SelectedItem + "&page=city", 650, 350);
            }
        }
    }

    protected void gridClient_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Page page1 = HttpContext.Current.Handler as Page;
        //TextBox txtCountryName = (TextBox)gridClient.FooterRow.FindControl("txtCountryName");
        switch (e.CommandName)
        {
            case "Delete":
                GridViewRow gr = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                Label lblClientId = (Label)gr.FindControl("lblClientId");
                Label lblClientName = (Label)gr.FindControl("lblClientName");
                var PriceData = from DBPrice in dbobj.PriceMasters
                                where DBPrice.ClientId == Convert.ToInt64(lblClientId.Text) && DBPrice.IsDeleted == false
                                select DBPrice;
                var InvoiceData = from DBInvoice in dbobj.InvoiceMasters
                                  where DBInvoice.ClientId == Convert.ToInt64(lblClientId.Text) && DBInvoice.IsPaid == false
                                  select DBInvoice;
                var CContactData = from DBContact in dbobj.ClientContactMasters
                                   where DBContact.ClientId == Convert.ToInt64(lblClientId.Text) && DBContact.IsDeleted == false
                                   select DBContact;
                if (PriceData.Count() == 0 && InvoiceData.Count() == 0 && CContactData.Count() == 0)
                {
                    var DelClient = from DelC in dbobj.ClientMasters
                                    where DelC.ClientId == Convert.ToInt64(lblClientId.Text)
                                    select DelC;
                    if (DelClient.Count() > 0)
                    {
                        var Client = DelClient.Single();
                        Client.IsActive = false;
                        dbobj.SubmitChanges();
                        dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                        //ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + SingleClient.ClientName.ToUpper() + " client name deleted" + "')", true);
                        FillGrid();
                    }
                }
                else
                    OpenWindow(this, "MITPLInvoice", "DeletePopup?id=" + lblClientId.Text + "&name=" + lblClientName.Text + "&page=client", 650, 350);
                break;
        }
    }

    protected void gridClient_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    protected void gridClient_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridClient.PageIndex = e.NewPageIndex;
        FillGrid();
    }

    protected void btnNewClient_Click(object sender, EventArgs e)
    {
        FillDepartment();
        FillCountry();
        FillState1();
        FillState2();
        FillCity1();
        FillCity2();
        FillCurrency();

        UserM1_M2_Visible(); //// Global.UserM1 and Global.UserM2

        ddlCountryVisible(""); //// New change By Jignesh on 13-Jul-2022
        // // if (Global.UserM1 && Global.UserM2)
        // // {
        // //     chkClient_M1.Checked = true;
        // //     chkClient_M1.Visible = true;
        // //     lblClient_M1.Visible = true;
        // // 
        // //     chkClient_M2.Checked = true;
        // //     chkClient_M2.Visible = true;
        // //     lblClient_M2.Visible = true;
        // // }
        // // else
        // // {
        // //     if (Global.UserM1)
        // //     {
        // //         chkClient_M1.Checked = true;
        // //         chkClient_M1.Visible = false;
        // //         lblClient_M1.Visible = false;
        // //     }
        // //     else
        // //     {
        // //         chkClient_M1.Checked = false;
        // //         chkClient_M1.Visible = false;
        // //         lblClient_M1.Visible = false;
        // //     }
        // //     if (Global.UserM2)
        // //     {
        // //         chkClient_M2.Checked = true;
        // //         chkClient_M2.Visible = false;
        // //         lblClient_M2.Visible = false;
        // //     }
        // //     else
        // //     {
        // //         chkClient_M2.Checked = false;
        // //         chkClient_M2.Visible = false;
        // //         lblClient_M2.Visible = false;
        // //     }
        // // }


        mvClient.ActiveViewIndex = 1;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        mvClient.ActiveViewIndex = 0;
        btnSave.Text = "Save";
        lbtnAddState.Visible = false;
        lbtnAddCity.Visible = false;
        Clear();
        FillGrid();
    }

    protected void imgEdit_Click(object sender, ImageClickEventArgs e)
    {
        mvClient.ActiveViewIndex = 1;
        btnSave.Text = "Update";
        ImageButton ibtn = (ImageButton)sender;
        GridViewRow gr = (GridViewRow)ibtn.NamingContainer;
        Label lblClientId = (Label)gr.FindControl("lblClientId");

        UserM1_M2_Visible();

        var UpClient = from DBData in dbobj.ClientMasters
                       where DBData.IsActive == true && DBData.ClientId == Convert.ToInt64(lblClientId.Text)
                       select DBData;
        if (UpClient.Count() > 0)
        {
            var SingleClient = UpClient.Single();
            ViewState["ClientId"] = SingleClient.ClientId;
            txtClientName.Text = SingleClient.ClientName;
            FillDepartment();
            ddlDepartment.Items.FindByValue(SingleClient.DepartmentId.ToString()).Selected = true;
            txtPrimaryEmail.Text = SingleClient.PrimaryEmail;
            txtRecipientEmail.Text = SingleClient.DefaultRecipient; ////Added by Jignesh on 11-Jun-2019
            txtCCEmail.Text = SingleClient.DefaultCC; ////Added by Jignesh on 11-Jun-2019
            txtSkypeId.Text = SingleClient.Skype;
            txtPhone.Text = SingleClient.Phone;
            txtMobile.Text = SingleClient.Mobile;
            txtFax.Text = SingleClient.Fax;
            txtAddress1.Text = SingleClient.Address1;
            FillCountry();
            ddlCountry1.Items.FindByValue(SingleClient.Country1.ToString()).Selected = true;
            //// New change By Jignesh on 13-Jul-2022
            ddlCountryVisible(ddlCountry1.SelectedItem.Text.ToUpper().ToString());
            txtGSTIN.Text = SingleClient.GSTIN;

            FillState1();
            ddlState1.Items.FindByValue(SingleClient.State1.ToString()).Selected = true;
            lbtnAddState.Visible = true;
            FillCity1();
            ddlCity1.Items.FindByValue(SingleClient.City1.ToString()).Selected = true;
            lbtnAddCity.Visible = true;
            txtZipCode1.Text = SingleClient.Zip_Postal1;
            txtAddress2.Text = SingleClient.Address2;
            if (SingleClient.Country2 != null)
                ddlCountry2.Items.FindByValue(SingleClient.Country2.ToString()).Selected = true;
            FillState2();
            if (SingleClient.State2 != null)
            {
                ddlState2.Items.FindByValue(SingleClient.State2.ToString()).Selected = true;
                lbtnAddState2.Visible = true;
            }
            FillCity2();
            if (SingleClient.City2 != null)
            {
                ddlCity2.Items.FindByValue(SingleClient.City2.ToString()).Selected = true;
                lbtnAddCity2.Visible = true;
            }
            txtZipCode2.Text = SingleClient.Zip_Postal2;
            txtWebsite.Text = SingleClient.Website;

            chkClient_M1.Checked = Convert.ToBoolean(SingleClient.C_M1); //// Added By Jignesh on 20-Jul-2020
            chkClient_M2.Checked = Convert.ToBoolean(SingleClient.C_M2); //// Added By Jignesh on 20-Jul-2020

            FillCurrency();
            if (SingleClient.CurrencyId != null)
                ddlCurrency.Items.FindByValue(SingleClient.CurrencyId.ToString()).Selected = true;
            txtRemarks.Text = SingleClient.Remarks;
        }
    }

    public static void OpenWindow(Page currentPage, String window, String htmlPage, Int32 width, Int32 height)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("popWin=window.open('");
        sb.Append(htmlPage);
        sb.Append("','");
        sb.Append(window);
        sb.Append("','width=");
        sb.Append(width);
        sb.Append(",height=");
        sb.Append(height);
        sb.Append(",left=300,top=150,toolbar=no,location=center,directories=no,status=no,menubar=no,scrollbars=no,resizable=no");
        sb.Append("');");
        sb.Append("popWin.focus();");

        ScriptManager.RegisterClientScriptBlock(currentPage, typeof(CountryMaster), "OpenWindow", sb.ToString(), true);
    }

    protected void gridClient_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblDepartmentId = (Label)e.Row.FindControl("lblDepartmentId");
            Label lbl1 = (Label)e.Row.FindControl("lbl1");
            ImageButton imgEdit = (ImageButton)e.Row.FindControl("imgEdit");
            ImageButton imgDelete = (ImageButton)e.Row.FindControl("imgDelete");
            LinkButton lbtnAddContact = (LinkButton)e.Row.FindControl("lbtnAddContact");

            var DepartmentId = from DB in dbobj.DepartmentMasters
                               where DB.IsActive == true && (DB.DepartmentId == Convert.ToInt64(Global.Department) || DB.UserId == Convert.ToInt64(Global.UserId))
                               select DB.DepartmentId;
            if (lblDepartmentId.Text != "")
            {
                if (!DepartmentId.Contains(Convert.ToInt64(lblDepartmentId.Text)))
                {
                    imgEdit.Visible = false;
                    imgDelete.Visible = false;
                    lbl1.Visible = false;
                    gridClient.Columns[11].Visible = false;
                    lbtnAddContact.Text = "View";
                }
            }


            /*var DepartmentId = from DB in dbobj.DepartmentMasters
                               where DB.IsActive == true && DB.UserId == Convert.ToInt64(Global.UserId)
                               select DB.DepartmentId;
            if (lblDepartmentId.Text != "")
            {
                if (!DepartmentId.Contains(Convert.ToInt64(lblDepartmentId.Text)))
                {
                    imgEdit.Visible = false;
                    imgDelete.Visible = false;
                    lbl1.Visible = false;
                    gridClient.Columns[11].Visible = false;
                    lbtnAddContact.Text = "View";
                }
            }*/
        }
    }

    protected void lbtnAddContact_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        GridViewRow gr = (GridViewRow)btn.NamingContainer;
        Label lblClientId = (Label)gr.FindControl("lblClientId");
        LinkButton lbtnAddContact = (LinkButton)gr.FindControl("lbtnAddContact");
        Response.Redirect("ClientContact?CID=" + Global.Encrypt(lblClientId.Text) + "&m=" + Global.Encrypt(lbtnAddContact.Text));
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "redirect", "location.href = 'ClientContact'", true);
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "redirect", "responce.redirect('ClientContact?CID=" + Global.Encrypt(lblClientId.Text) + "&m=" + Global.Encrypt(lbtnAddContact.Text)+ "')", true);
        //Page.ClientScript.RegisterStartupScript(this.GetType,,)

    }

    protected void ddlDepartmentSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }

    public string HighlightText(string InputTxt)
    {
        TextBox txt = (TextBox)this.Master.FindControl("txtMasterSearch");
        string Search_Str = txt.Text;

        // Setup the regular expression and add the Or operator.
        Regex RegExp = new Regex(Search_Str.Replace(" ", "|").Trim(), RegexOptions.IgnoreCase);
        // Highlight keywords by calling the
        //delegate each time a keyword is found.
        return RegExp.Replace(InputTxt, new MatchEvaluator(ReplaceKeyWords));
    }

    public string ReplaceKeyWords(Match m)
    {
        return ("<span class=highlight>" + m.Value + "</span>");
    }

    public void UserM1_M2_Visible()
    {
        if (Global.UserM1 && Global.UserM2)
        {
            chkClient_M1.Checked = true;
            chkClient_M1.Visible = true;
            lblClient_M1.Visible = true;

            chkClient_M2.Checked = true;
            chkClient_M2.Visible = true;
            lblClient_M2.Visible = true;
        }
        else
        {
            if (Global.UserM1)
            {
                chkClient_M1.Checked = true;
                chkClient_M1.Visible = false;
                lblClient_M1.Visible = false;
            }
            else
            {
                chkClient_M1.Checked = false;
                chkClient_M1.Visible = false;
                lblClient_M1.Visible = false;
            }
            if (Global.UserM2)
            {
                chkClient_M2.Checked = true;
                chkClient_M2.Visible = false;
                lblClient_M2.Visible = false;
            }
            else
            {
                chkClient_M2.Checked = false;
                chkClient_M2.Visible = false;
                lblClient_M2.Visible = false;
            }
        }
    }

}
