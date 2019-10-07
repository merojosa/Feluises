using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FeLuisesScrumDEV
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            /*
            routes.MapRoute(
                name: "Module Details",
                url: "Modules/Details/{idProjectFKPK}/{idModulePK}",
                defaults: new { controller = "Modules", action = "Details", idProjectFKPK= UrlParameter.Optional, idModulePK= UrlParameter.Optional }
            );
            */
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
