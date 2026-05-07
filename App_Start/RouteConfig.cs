using System.Web.Mvc;
using System.Web.Routing;

namespace InscriptionCLG
{
    public class RouteConfig
    {
        public static string DefaultAction()
        {
            return "/Teachers/List";
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Accounts", action = "Login", id = UrlParameter.Optional }
            );
        }
    }
}
