using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace K22CNT4_NGUYENDANHTRUONG_2210900071
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "NdtIndex", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "ConfirmOrder",
               url: "GioHangs/ConfirmOrder/{id}",
               defaults: new { controller = "GioHangs", action = "ConfirmOrder", id = UrlParameter.Optional }
             );
            routes.MapRoute(
                name: "Admin",
                url: "Admins/{action}/{id}",
                defaults: new { controller = "Admins", action = "NdtIndex", id = UrlParameter.Optional }
             );
            routes.MapRoute(
                name: "NguoiDungs",
                url: "NguoiDungs/{action}/{id}",
                defaults: new { controller = "NguoiDungs", action = "NdtIndex", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                    name: "DonHangs",
                    url: "DonHangs/{action}/{id}",
                    defaults: new { controller = "DonHangs", action = "NdtIndex", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Delete",
                url: "DonHangs/Delete/{id}",
                defaults: new { controller = "DonHangs", action = "NdtDelete", id = UrlParameter.Optional }
            );


        }

    }
}