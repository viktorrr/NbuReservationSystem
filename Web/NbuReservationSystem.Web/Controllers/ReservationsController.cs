namespace NbuReservationSystem.Web.Controllers
{
    using System;
    using System.Net;
    using System.Web.Mvc;

    using NbuReservationSystem.Web.Models.Requests.Reservations;

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

        [HttpGet]
        public ActionResult New(int? year, int? month, int? day)
        {
            // TODO: implement me!
            // TODO: validate that the given date is > now
            // TODO: create a model, which has the given dates and return the view
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(AddReservationViewModel viewModel)
        {
            // TODO: implement me!
            // TODO: validate that the date is > now
            // TODO: validate that the check boxes are exactly 7
            // TODO: validate that the hall is free for the given date
            // TODO: generate reservations based on the repetition policy
            // TODO: generate unique token!
            // TODO: save the user's IP
            // TODO: send an email
            // TODO: redirect to /calendar with success message -> mail + content
            return this.View();
        }
    }
}
