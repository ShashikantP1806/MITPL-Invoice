using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;

public partial class Company : System.Web.UI.Page
{
    MITInvoiceDataContext dbobj = new MITInvoiceDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Global.UserType == "BUSINESS UNIT MANAGER" || Global.UserType == "DIRECTOR")
        {
            if (!IsPostBack)
            {
                mvCompany.ActiveViewIndex = 0;
                FillDepartmentSearch();
                FillGrid();
            }
            if (Global.UserType == "DIRECTOR")
            {
                btnNewCompany.Visible = false;
                //gridCompany.Columns[11].Visible = false;
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

    public void FillGrid()
    {
        if (ddlDepartmentSearch.SelectedIndex != -1)
        {
            TextBox txtMasterSearch = (TextBox)this.Master.FindControl("txtMasterSearch");
            if (txtMasterSearch.Text == "")
            {
                // txtMasterSearch is blank

                if (Global.UserType == "DIRECTOR" || Global.DepartmentName == "ACCT")
                {
                    if (ddlDepartmentSearch.SelectedItem.ToString() == "-- All --")
                    {
                        //For UserType = DIRECTOR and Dept: ACCT
                        #region Search with All Department

                        var CompanyData = (from DBData in dbobj.CompanyMasters
                                           where DBData.IsDelete == false
                                           orderby DBData.DepartmentMaster.DepartmentName, DBData.CompanyName
                                           select new
                                           {
                                               CompanyID = DBData.CompanyID,
                                               CompanyName = DBData.CompanyName,
                                               DepartmentId = DBData.DepartmentID,
                                               Department = DBData.DepartmentMaster.DepartmentName,
                                               Phone = DBData.CompanyPhone,
                                               Address = DBData.Address,
                                               Website = DBData.Website == null ? "-" : DBData.Website,
                                               Currency = DBData.CurrencyMaster.CurrencyCode + "-" + ("&#" + DBData.CurrencyMaster.CurrencySymbol)
                                           }).ToList();
                        if (CompanyData.Count() > 0)
                        {
                            gridCompany.DataSource = CompanyData;
                            gridCompany.DataBind();
                        }
                        else
                        {
                            BlankGrid();
                        }

                        #endregion
                    }
                    else
                    {
                        #region Search with Department
                        var CompanyData = (from DBData in dbobj.CompanyMasters
                                           where DBData.IsDelete == false && DBData.DepartmentID == Convert.ToInt64(ddlDepartmentSearch.SelectedValue)
                                           orderby DBData.DepartmentMaster.DepartmentName, DBData.CompanyName
                                           select new
                                           {
                                               CompanyID = DBData.CompanyID,
                                               CompanyName = DBData.CompanyName,
                                               DepartmentId = DBData.DepartmentID,
                                               Department = DBData.DepartmentMaster.DepartmentName,
                                               Phone = DBData.CompanyPhone,
                                               Address = DBData.Address,
                                               Website = DBData.Website == null ? "-" : DBData.Website,
                                               Currency = DBData.CurrencyMaster.CurrencyCode + "-" + ("&#" + DBData.CurrencyMaster.CurrencySymbol)
                                           }).ToList();
                        if (CompanyData.Count() > 0)
                        {
                            gridCompany.DataSource = CompanyData;
                            gridCompany.DataBind();
                        }
                        else
                        {
                            BlankGrid();
                        }
                        #endregion
                    }
                }
                else
                {
                    if (ddlDepartmentSearch.SelectedItem.ToString() == "-- All --")
                    {
                        #region Search with All Department

                        var CompanyData = (from DBData in dbobj.CompanyMasters
                                           where DBData.IsDelete == false && DBData.DepartmentMaster.UserId == Convert.ToInt64(Global.UserId)
                                           orderby DBData.DepartmentMaster.DepartmentName, DBData.CompanyName
                                           select new
                                           {
                                               CompanyID = DBData.CompanyID,
                                               CompanyName = DBData.CompanyName,
                                               DepartmentId = DBData.DepartmentID,
                                               Department = DBData.DepartmentMaster.DepartmentName,
                                               Phone = DBData.CompanyPhone,
                                               Address = DBData.Address,
                                               Website = DBData.Website == null ? "-" : DBData.Website,
                                               Currency = DBData.CurrencyMaster.CurrencyCode + "-" + ("&#" + DBData.CurrencyMaster.CurrencySymbol)
                                           }).ToList();
                        if (CompanyData.Count() > 0)
                        {
                            gridCompany.DataSource = CompanyData;
                            gridCompany.DataBind();
                        }
                        else
                        {
                            BlankGrid();
                        }

                        #endregion
                    }
                    else
                    {
                        #region Search with Department
                        var CompanyData = (from DBData in dbobj.CompanyMasters
                                           where DBData.IsDelete == false && DBData.DepartmentID == Convert.ToInt64(ddlDepartmentSearch.SelectedValue)
                                           orderby DBData.DepartmentMaster.DepartmentName, DBData.CompanyName
                                           select new
                                           {
                                               CompanyID = DBData.CompanyID,
                                               CompanyName = DBData.CompanyName,
                                               DepartmentId = DBData.DepartmentID,
                                               Department = DBData.DepartmentMaster.DepartmentName,
                                               Phone = DBData.CompanyPhone,
                                               Address = DBData.Address,
                                               Website = DBData.Website == null ? "-" : DBData.Website,
                                               Currency = DBData.CurrencyMaster.CurrencyCode + "-" + ("&#" + DBData.CurrencyMaster.CurrencySymbol)
                                           }).ToList();
                        if (CompanyData.Count() > 0)
                        {
                            gridCompany.DataSource = CompanyData;
                            gridCompany.DataBind();
                        }
                        else
                        {
                            BlankGrid();
                        }
                        #endregion
                    }
                }


            }
            else
            {
                // txtMasterSearch is not blank
                if (Global.UserType == "DIRECTOR" || Global.DepartmentName == "ACCT")
                {
                    if (ddlDepartmentSearch.SelectedItem.ToString() == "-- All --")
                    {
                        //For txtMasterSearch is not blank
                        #region Search with All Department and Company Name

                        var CompanyData = (from DBData in dbobj.CompanyMasters
                                           where DBData.IsDelete == false && DBData.CompanyName.Contains(txtMasterSearch.Text.Trim())
                                           orderby DBData.DepartmentMaster.DepartmentName, DBData.CompanyName
                                           select new
                                           {
                                               CompanyID = DBData.CompanyID,
                                               CompanyName = DBData.CompanyName,
                                               DepartmentId = DBData.DepartmentID,
                                               Department = DBData.DepartmentMaster.DepartmentName,
                                               Phone = DBData.CompanyPhone,
                                               Address = DBData.Address,
                                               Website = DBData.Website == null ? "-" : DBData.Website,
                                               Currency = DBData.CurrencyMaster.CurrencyCode + "-" + ("&#" + DBData.CurrencyMaster.CurrencySymbol)
                                           }).ToList();
                        if (CompanyData.Count() > 0)
                        {
                            gridCompany.DataSource = CompanyData;
                            gridCompany.DataBind();
                        }
                        else
                        {
                            BlankGrid();
                        }

                        #endregion
                    }
                    else
                    {
                        #region Search with Department and Company Name
                        var CompanyData = (from DBData in dbobj.CompanyMasters
                                           where DBData.IsDelete == false && DBData.DepartmentID == Convert.ToInt64(ddlDepartmentSearch.SelectedValue) && DBData.CompanyName.Contains(txtMasterSearch.Text.Trim())
                                           orderby DBData.DepartmentMaster.DepartmentName, DBData.CompanyName
                                           select new
                                           {
                                               CompanyID = DBData.CompanyID,
                                               CompanyName = DBData.CompanyName,
                                               DepartmentId = DBData.DepartmentID,
                                               Department = DBData.DepartmentMaster.DepartmentName,
                                               Phone = DBData.CompanyPhone,
                                               Address = DBData.Address,
                                               Website = DBData.Website == null ? "-" : DBData.Website,
                                               Currency = DBData.CurrencyMaster.CurrencyCode + "-" + ("&#" + DBData.CurrencyMaster.CurrencySymbol)
                                           }).ToList();
                        if (CompanyData.Count() > 0)
                        {
                            gridCompany.DataSource = CompanyData;
                            gridCompany.DataBind();
                        }
                        else
                        {
                            BlankGrid();
                        }
                        #endregion
                    }
                }
                else
                {
                    if (ddlDepartmentSearch.SelectedItem.ToString() == "-- All --")
                    {
                        //For txtMasterSearch is not blank
                        #region Search with All Department and Company Name

                        var CompanyData = (from DBData in dbobj.CompanyMasters
                                           where DBData.IsDelete == false && DBData.CompanyName.Contains(txtMasterSearch.Text.Trim())
                                           orderby DBData.DepartmentMaster.DepartmentName, DBData.CompanyName
                                           select new
                                           {
                                               CompanyID = DBData.CompanyID,
                                               CompanyName = DBData.CompanyName,
                                               DepartmentId = DBData.DepartmentID,
                                               Department = DBData.DepartmentMaster.DepartmentName,
                                               Phone = DBData.CompanyPhone,
                                               Address = DBData.Address,
                                               Website = DBData.Website == null ? "-" : DBData.Website,
                                               Currency = DBData.CurrencyMaster.CurrencyCode + "-" + ("&#" + DBData.CurrencyMaster.CurrencySymbol)
                                           }).ToList();
                        if (CompanyData.Count() > 0)
                        {
                            gridCompany.DataSource = CompanyData;
                            gridCompany.DataBind();
                        }
                        else
                        {
                            BlankGrid();
                        }

                        #endregion
                    }
                    else
                    {
                        #region Search with Department and Company Name
                        var CompanyData = (from DBData in dbobj.CompanyMasters
                                           where DBData.IsDelete == false && DBData.DepartmentID == Convert.ToInt64(ddlDepartmentSearch.SelectedValue) && DBData.CompanyName.Contains(txtMasterSearch.Text.Trim())
                                           orderby DBData.DepartmentMaster.DepartmentName, DBData.CompanyName
                                           select new
                                           {
                                               CompanyID = DBData.CompanyID,
                                               CompanyName = DBData.CompanyName,
                                               DepartmentId = DBData.DepartmentID,
                                               Department = DBData.DepartmentMaster.DepartmentName,
                                               Phone = DBData.CompanyPhone,
                                               Address = DBData.Address,
                                               Website = DBData.Website == null ? "-" : DBData.Website,
                                               Currency = DBData.CurrencyMaster.CurrencyCode + "-" + ("&#" + DBData.CurrencyMaster.CurrencySymbol)
                                           }).ToList();
                        if (CompanyData.Count() > 0)
                        {
                            gridCompany.DataSource = CompanyData;
                            gridCompany.DataBind();
                        }
                        else
                        {
                            BlankGrid();
                        }
                        #endregion
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
        dt.Columns.Add("CompanyID");
        dt.Columns.Add("CompanyName");
        dt.Columns.Add("DepartmentId");
        dt.Columns.Add("Department");
        dt.Columns.Add("Phone");
        dt.Columns.Add("Address");
        dt.Columns.Add("Website");
        dt.Columns.Add("Currency");

        gridCompany.DataSource = dt;
        dt.Rows.Add(dt.NewRow());
        gridCompany.DataBind();
        int TotalCols = gridCompany.Rows[0].Cells.Count;
        gridCompany.Rows[0].Cells.Clear();
        gridCompany.Rows[0].Cells.Add(new TableCell());
        gridCompany.Rows[0].Cells[0].ColumnSpan = TotalCols;
        gridCompany.Rows[0].Cells[0].Text = "No records to display";
    }

    protected void Clear()
    {
        txtCompanyName.Text = "";
        ddlDepartment.Items.Clear();
        txtPhone.Text = "";
        txtAddress.Text = "";
        txtWebsite.Text = "";
        ddlCurrency.Items.Clear();
        txtRemarks.Text = "";
        UserM1_M2_Visible(); //// Global.UserM1 and Global.UserM2
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

    protected void gridCompany_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridCompany.PageIndex = e.NewPageIndex;
        FillGrid();
    }

    protected void gridCompany_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Page page1 = HttpContext.Current.Handler as Page;
        //TextBox txtCountryName = (TextBox)gridClient.FooterRow.FindControl("txtCountryName");
        switch (e.CommandName)
        {
            case "Delete":
                GridViewRow gr = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                Label lblCompanyID = (Label)gr.FindControl("lblCompanyID");
                Label lblCompanyName = (Label)gr.FindControl("lblCompanyName");


                var DelClient = from DelC in dbobj.CompanyMasters
                                where DelC.CompanyID == Convert.ToInt64(lblCompanyID.Text)
                                select DelC;
                if (DelClient.Count() > 0)
                {
                    var Client = DelClient.Single();
                    Client.IsDelete = true;
                    dbobj.SubmitChanges();
                    dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
                    //ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('" + SingleClient.ClientName.ToUpper() + " client name deleted" + "')", true);
                    FillGrid();
                }

                //  else
                //      OpenWindow(this, "MITPLInvoice", "DeletePopup?id=" + lblClientId.Text + "&name=" + lblClientName.Text + "&page=client", 650, 350);
                break;
        }

    }

    protected void gridCompany_RowDataBound(object sender, GridViewRowEventArgs e)
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
                    //gridCompany.Columns[11].Visible = false;
                    lbtnAddContact.Text = "View";
                }
            }



        }
    }

    protected void gridCompany_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    protected void lbtnAddIndividualContact_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        GridViewRow gr = (GridViewRow)btn.NamingContainer;
        Label lblCompanyID = (Label)gr.FindControl("lblCompanyID");
        LinkButton lbtnAddIndividualContact = (LinkButton)gr.FindControl("lbtnAddIndividualContact");
        Response.Redirect("ConsultantContact?CID=" + Global.Encrypt(lblCompanyID.Text) + "&m=" + Global.Encrypt(lbtnAddIndividualContact.Text));

    }

    protected void btnNewCompany_Click(object sender, EventArgs e)
    {
        FillDepartment();
        FillCurrency();
        UserM1_M2_Visible();
        mvCompany.ActiveViewIndex = 1;
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

    protected void imgEdit_Click(object sender, ImageClickEventArgs e)
    {
        mvCompany.ActiveViewIndex = 1;
        btnSave.Text = "Update";
        ImageButton ibtn = (ImageButton)sender;
        GridViewRow gr = (GridViewRow)ibtn.NamingContainer;
        Label lblCompanyID = (Label)gr.FindControl("lblCompanyID");

         UserM1_M2_Visible();

        var UpCompany = from DBData in dbobj.CompanyMasters
                        where DBData.IsDelete == false && DBData.CompanyID == Convert.ToInt64(lblCompanyID.Text)
                        select DBData;
        if (UpCompany.Count() > 0)
        {
            var singleCompany = UpCompany.Single();
            ViewState["CompanyID"] = singleCompany.CompanyID;
            txtCompanyName.Text = singleCompany.CompanyName;
            FillDepartment();
            ddlDepartment.Items.FindByValue(singleCompany.DepartmentID.ToString()).Selected = true;
            txtPhone.Text = singleCompany.CompanyPhone;
            txtAddress.Text = singleCompany.Address;
            txtWebsite.Text = singleCompany.Website;
            FillCurrency();
            if (singleCompany.CurrencyType != null)
                ddlCurrency.Items.FindByValue(singleCompany.CurrencyType.ToString()).Selected = true;
            txtRemarks.Text = singleCompany.Description;
        }

    }

    protected void btnOk_Click(object sender, EventArgs e)
    {

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        mvCompany.ActiveViewIndex = 0;
        btnSave.Text = "Save";
        Clear();
        FillGrid();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Page page1 = HttpContext.Current.Handler as Page;
        if (btnSave.Text == "Save")
        {
            #region New Company
            CompanyMaster cm = new CompanyMaster();
            cm.CompanyName = txtCompanyName.Text.Trim().ToUpper();
            cm.CompanyPhone = txtPhone.Text.Trim();

            cm.Address = txtAddress.Text.Trim().ToUpper();

            if (txtWebsite.Text != "")
                cm.Website = txtWebsite.Text.Trim();
            cm.DepartmentID = Convert.ToInt64(ddlDepartment.SelectedValue);
            if (ddlCurrency.SelectedItem.ToString() != "-- Select --")
                cm.CurrencyType = Convert.ToInt64(ddlCurrency.SelectedValue);
            else
                cm.CurrencyType = null;
            if (txtRemarks.Text != "")
                cm.Description = txtRemarks.Text.Trim();
            cm.IsDelete = false;

            cm.CreatedBy = Convert.ToInt64(Global.UserId);
            cm.CreatedDate = DateTime.Now;
            cm.ModifyBy = Convert.ToInt64(Global.UserId);
            cm.ModifyDate = DateTime.Now;
            //// For multiple access
            cm.Com_M1 = chkClient_M1.Checked;
            cm.Com_M2 = chkClient_M2.Checked;

            dbobj.CompanyMasters.InsertOnSubmit(cm);
            dbobj.SubmitChanges();
            dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
            ScriptManager.RegisterStartupScript(page1, page1.GetType(), "MITPL Invoice", "alert('Company added successfully')", true);
            #endregion 
        }
        if (btnSave.Text == "Update")
        {
            #region Update Existing Date
            var UpClient = from DBData in dbobj.CompanyMasters
                           where DBData.IsDelete == false && DBData.CompanyID == Convert.ToInt64(ViewState["CompanyID"].ToString())
                           select DBData;
            var SingleClient = UpClient.Single();
            SingleClient.CompanyName = txtCompanyName.Text.Trim();


            SingleClient.CompanyPhone = txtPhone.Text.Trim();

            SingleClient.Address = txtAddress.Text.Trim();

            if (txtWebsite.Text != "")
                SingleClient.Website = txtWebsite.Text.Trim();
            else
                SingleClient.Website = null;
            SingleClient.DepartmentID = Convert.ToInt64(ddlDepartment.SelectedValue);
            if (ddlCurrency.SelectedItem.ToString() != "-- Select --")
                SingleClient.CurrencyType = Convert.ToInt64(ddlCurrency.SelectedValue);
            else
                SingleClient.CurrencyType = null;
            if (txtRemarks.Text != "")
                SingleClient.Description = txtRemarks.Text.Trim();
            else
                SingleClient.Description = null;


            //// For multiple access
            SingleClient.Com_M1 = chkClient_M1.Checked;
            SingleClient.Com_M2 = chkClient_M2.Checked;

            SingleClient.ModifyBy = Convert.ToInt64(Global.UserId);
            SingleClient.ModifyDate = DateTime.Now;
            dbobj.SubmitChanges();
            dbobj.Refresh(System.Data.Linq.RefreshMode.KeepChanges);
            btnSave.Text = "Save";
            #endregion

        }
        mvCompany.ActiveViewIndex = 0;
        FillGrid();
        Clear();
    }

    protected void ddlDepartmentSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
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

