namespace NbuReservationSystem.Web
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "NewReservation",
                url: "Reservations/New/{year}/{month}/{day}",
                defaults: new {
                    controller = "Reservations", action = "New",
                    year = UrlParameter.Optional, month = UrlParameter.Optional, day = UrlParameter.Optional
                }
            );

            routes.MapRoute(
                name: "MonthlyReservations",
                url: "Reservations/ByMonth/{year}/{month}",
                defaults: new { controller = "Reservations", action = "ByMonth" }
            );

            routes.MapRoute(
                name: "DayTab",
                url: "Reservations/DayTab/{date}",
                defaults: new { controller = "Reservations", action = "DayTab" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
