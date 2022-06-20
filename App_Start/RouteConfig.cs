using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AudioBook
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Book", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Book",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Book", action = "Book", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CreateBook",
                url: "{controller}/{action}/{book_id}",
                defaults: new { controller = "Book", action = "CreateBook", id = UrlParameter.Optional }
            );
        }
    }
}
