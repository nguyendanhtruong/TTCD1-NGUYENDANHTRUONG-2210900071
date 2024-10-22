using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TTCD1_NGUYENDANHTRUONG_2210900071
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
    name: "ConfirmOrder",
    url: "GioHangs/ConfirmOrder/{id}",
    defaults: new { controller = "GioHangs", action = "ConfirmOrder", id = UrlParameter.Optional }
);
            routes.MapRoute(
    name: "Admin",
    url: "Admins/{action}/{id}",
    defaults: new { controller = "Admins", action = "Index", id = UrlParameter.Optional }
);


        }

    }
}
