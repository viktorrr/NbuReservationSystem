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
            const string Url = "/Reservations/DayTab/2016-12-01";

            var date = DateTime.Parse("2016-12-01");

            var routeCollection = new RouteCollection();
            RouteConfig.RegisterRoutes(routeCollection);
            routeCollection.ShouldMap(Url).To<ReservationsController>(c => c.DayTab(date));
        }
    }
}
