<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Routing" %>

<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup
        RegisterRoutes(RouteTable.Routes);
    }

    public static void RegisterRoutes(RouteCollection routes)
    {
        routes.Add("HomeRoute", new Route
        (
         "Home",
         new CustomRouteHandler("~/Home.aspx")
        ));

        routes.Add("ManageUsersRoute", new Route
        (
         "ManageUsers",
         new CustomRouteHandler("~/User.aspx")
        ));

        routes.Add("ProfileRoute", new Route
        (
         "Profile",
         new CustomRouteHandler("~/UserProfile.aspx")
        ));

        routes.Add("ManageDepartmentsRoute", new Route
        (
         "ManageDepartments",
         new CustomRouteHandler("~/Department.aspx")
        ));

        routes.Add("ManageClientRoute", new Route
        (
         "ManageClient",
         new CustomRouteHandler("~/Client.aspx")
        ));

        routes.Add("ClientContactRoute", new Route
        (
         "ClientContact",
         new CustomRouteHandler("~/ClientContact.aspx")
        ));

        routes.Add("ManageCountryRoute", new Route
        (
         "ManageCountry",
         new CustomRouteHandler("~/Country.aspx")
        ));

        routes.Add("ManageStateRoute", new Route
        (
         "ManageState",
         new CustomRouteHandler("~/State.aspx")
        ));

        routes.Add("ManageCityRoute", new Route
        (
         "ManageCity",
         new CustomRouteHandler("~/City.aspx")
        ));

        routes.Add("ManageCurrencyRoute", new Route
        (
         "ManageCurrency",
         new CustomRouteHandler("~/Currency.aspx")
        ));

        routes.Add("NewInvoiceRoute", new Route
        (
         "NewInvoice",
         new CustomRouteHandler("~/Invoice.aspx")
        ));

        routes.Add("ViewInvoicesRoute", new Route
        (
         "ViewInvoice",
         new CustomRouteHandler("~/ViewInvoices.aspx")
        ));

        // Added by Shashikant 03-Nov-2020
        routes.Add("NewProformaInvoiceRoute", new Route
        (
         "ProformaInvoice",
         new CustomRouteHandler("~/ProformaInvoice.aspx")
        ));

        routes.Add("NewViewProformaInvoiceRoute", new Route
        (
         "ViewProformaInvoice",
         new CustomRouteHandler("~/ViewProformaInvoices.aspx")
        ));

        routes.Add("PriceRoute", new Route
        (
         "Price",
         new CustomRouteHandler("~/Price.aspx")
        ));

        routes.Add("PriceProcessRoute", new Route
        (
         "PriceProcess",
         new CustomRouteHandler("~/PriceProcess.aspx")
        ));

        routes.Add("PriceTypeRoute", new Route
        (
         "PriceType",
         new CustomRouteHandler("~/PriceType.aspx")
        ));

        routes.Add("AboutUsRoute", new Route
        (
         "AboutUs",
         new CustomRouteHandler("~/AboutUs.aspx")
        ));

        routes.Add("CurrencyHelpRoute", new Route
        (
         "CurrencyCode",
         new CustomRouteHandler("~/CurrencyHelp.aspx")
        ));

        routes.Add("DeletePopupRoute", new Route
        (
         "DeletePopup",
         new CustomRouteHandler("~/DeletePopup.aspx")
        ));

        routes.Add("ForgotPasswordRoute", new Route
        (
         "ForgotPassword",
         new CustomRouteHandler("~/ForgotPassword.aspx")
        ));

        routes.Add("LoginRoute", new Route
        (
         "Login",
         new CustomRouteHandler("~/Login.aspx")
        ));

        routes.Add("PopupWindowRoute", new Route
        (
         "PopupWindow",
         new CustomRouteHandler("~/PopupWindow.aspx")
        ));

        routes.Add("PreviewInvoiceRoute", new Route
        (
         "PreviewInvoices",
         new CustomRouteHandler("~/PreviewInvoice.aspx")
        ));

        // Added by Shashikant 04-Nov-2020
        routes.Add("PreviewProfomaInvoiceRoute", new Route
        (
         "PreviewProfomaInvoice",
         new CustomRouteHandler("~/PreviewProfomaInvoice.aspx")
        ));

        routes.Add("SendInvoiceEmailRoute", new Route
        (
         "SendInvoiceEmail",
         new CustomRouteHandler("~/SendInvoiceEmail.aspx")
        ));

        routes.Add("AuthorizeRoute", new Route
        (
         "Authorize",
         new CustomRouteHandler("~/Authorization.aspx")
        ));

        routes.Add("MatrixReportRoute", new Route
       (
        "MonthwiseClientReport",
        new CustomRouteHandler("~/MatrixReport.aspx")
       ));

        routes.Add("MonthlySalesRoute", new Route
       (
        "MonthlySales",
        new CustomRouteHandler("~/MonthlySales.aspx")
       ));

          routes.Add("ManageRoute", new Route
       (
        "Manage",
        new CustomRouteHandler("~/ManageAccount.aspx")
       ));

        routes.Add("MonthlySalesNewRoute", new Route
       (
        "MonthlySalesNew",
        new CustomRouteHandler("~/MonthlySalesNew.aspx")
       ));

        routes.Add("ManageConsultantRoute", new Route
        (
         "ManageConsultant",
         new CustomRouteHandler("~/Company.aspx")
        ));

        routes.Add("ManageConsultantContactRoute", new Route
        (
         "ConsultantContact",
         new CustomRouteHandler("~/CompanyContact.aspx")
        ));

         routes.Add("ManageInvoicePayment", new Route
        (
         "InvoicePaymentDetails",
         new CustomRouteHandler("~/CompanyPaymentDetails.aspx")
        ));

        // Shahsikant 11-April-2023
        routes.Add("CommonClientInvoice", new Route
        (
         "CommonClientInvoiceClient",
         new CustomRouteHandler("~/CommonClientInvoice.aspx")
        ));


        routes.Add("errorRoute", new Route
      (
       "Error",
       new CustomRouteHandler("~/CustomError.aspx")
      ));
    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs
      //  Response.Redirect("Error");
    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>

