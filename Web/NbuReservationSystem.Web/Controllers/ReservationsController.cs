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
        public ActionResult Monthly(int? year, int? month)
        {
            var isInputValid = year.HasValue && month.HasValue;

            if (month >= 13 || month <= 0)
            {
                // TODO: log this!
                isInputValid = false;
            }

            var now = isInputValid ? new DateTime(year.Value, month.Value, 1) : DateTime.UtcNow;
            var reservations = this.reservationsService.GetReservations(now.Year, now.Month);

            return this.View(reservations);
        }

        [HttpGet]
        public ActionResult ByMonth(int year, int month)
        {
            // TODO: add an "is ajax" filter
            if (!this.Request.IsAjaxRequest())
            {
                this.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("Nope.");
            }

            var now = new DateTime(year, month, 1);
            var reservations = this.reservationsService.GetReservations(now.Year, now.Month);

            return this.PartialView("_Calendar", reservations);
        }

        [HttpGet]
        public ActionResult DayTab(DateTime date)
        {
            if (!this.Request.IsAjaxRequest())
            {
                this.Response.StatusCode = (int)HttpStatusCode.Forbidden;
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
