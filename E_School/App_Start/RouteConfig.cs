using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace E_School
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            //routes.IgnoreRoute("a");
            //routes.IgnoreRoute("{resource}.asmx/{*pathInfo}");
            //routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.Add("RouteName", new Route("WebServices/E_Shool_API.asmx", new RouteValueDictionary() { { "controller", null }, { "action", null } }, new ServiceRouteHandler("~/E_Shool_API.asmx")));
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            //routes.IgnoreRoute("{*x}", new { x = @".*\.asmx(/.*)?" });
           // routes.IgnoreRoute("a");
            
        }
    }
}