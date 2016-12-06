namespace NbuReservationSystem.Web.Controllers
{
    using System;
    using System.Net;
    using System.Web.Mvc;

    using Services.Web;

    public class ReservationsController : Controller
    {
        private readonly IReservationsService reservationsService;

        public ReservationsController(IReservationsService reservationsService)
        {
            this.reservationsService = reservationsService;
        }

        [HttpGet]
        public ActionResult Monthly()
        {
            var now = DateTime.UtcNow;
            var reservations = this.reservationsService.GetReservations(now.Year, now.Month);

            return this.View(reservations);
        }

        [HttpGet]
        public ActionResult ByMonth(int? year, int? month)
        {
            if (!year.HasValue || !month.HasValue)
            {
                return this.RedirectToAction("Monthly");
            }

            if (month >= 13 || month <= 0)
            {
                // TODO: log this!
                return this.Content("Nope.");
            }



            // TODO: it utc needed?
            // var now = new DateTime(year.Value, month.Value, 1).ToUniversalTime().AddHours(2);
            var now = new DateTime(year.Value, month.Value, 1);
            var reservations = this.reservationsService.GetReservations(now.Year, now.Month);

            return this.View("Monthly", reservations);
        }

        [HttpGet]
        public ActionResult DayTab(DateTime date)
        {
            if (!this.Request.IsAjaxRequest())
            {
                this.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                // TODO: log this
                return this.Content("Nope.");
            }

            var reservations = this.reservationsService.GetReservations(date);

            return this.PartialView("_DayTab", reservations);
        }

        [HttpPost]
        public ActionResult New()
        {
            // TODO: implement me!
            return this.View();
        }
    }
}
