namespace NbuReservationSystem.Web.Routes.Tests
{
    using System;
    using System.Web.Routing;

    using Controllers;

    using MvcRouteTester;

    using Xunit;

    public class ReservationsRoutesTests
    {
        [Fact]
        public void TestCalendatById()
        {
            const string Url = "/Reservations/DayTab?year=2017&month=1&day=18&hallName=library";

            var routeParams = new { year = 2017, month = 1, day = 18, hallName = "library" };
            var routeCollection = new RouteCollection();

            RouteConfig.RegisterRoutes(routeCollection);
            routeCollection.ShouldMap(Url).To<ReservationsController>(c =>
                c.DayTab(routeParams.year, routeParams.month, routeParams.day, routeParams.hallName)
            );
        }
    }
}
