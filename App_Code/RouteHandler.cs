using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Compilation;
using System.Web.UI;

/// <summary>
/// Summary description for RouteHandler
/// </summary>
public class CustomRouteHandler : IRouteHandler
{
    public CustomRouteHandler(string virtualPath)
    {
        this.VirtualPath = virtualPath;
                
    }

    public string VirtualPath { get; private set; }

    public IHttpHandler GetHttpHandler(RequestContext
          requestContext)
    {
        //if (requestContext.RouteData.Values["r"] != null)
        //{
        //    var display = BuildManager.CreateInstanceFromVirtualPath(
        //                 VirtualPath, typeof(Page)) as IUserDisplay;
        //    display.UniqueName =
        //      requestContext.RouteData.Values["r"] as string;
        //    return display;
        //}
        //else
        //{
        var page = BuildManager.CreateInstanceFromVirtualPath
             (VirtualPath, typeof(Page)) as IHttpHandler;
        return page;
        //}

    }
}
