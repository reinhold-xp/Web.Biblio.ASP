using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Biblio.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Accueil",
                url: "Accueil-Biblio",
                defaults: new { controller = "Home", action = "Index" }
            );

            routes.MapRoute("HomeEn", "en", new { lang = "en", controller = "Home", action = "Index" });
            routes.MapRoute("HomeFr", "fr", new { lang = "fr", controller = "Home", action = "Index" });

            routes.MapRoute(
                name: "Livres",
                url: "Livres-Biblio",
                defaults: new { controller = "Book", action = "Index" }
            );

            routes.MapRoute(
                name: "Detail",
                url: "Livre/{id}",
                defaults: new { controller = "Book", action = "Details" }
            );

            routes.MapRoute(
                name: "Contact",
                url: "Contact-Biblio",
                defaults: new { controller = "Home", action = "Contact" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
