namespace NbuReservationSystem.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;
    using System.Web.Mvc;

    using Models.Requests.Reservations;

    using NbuReservationSystem.Common;
    using NbuReservationSystem.Data.Common;
    using NbuReservationSystem.Data.Models;
    using NbuReservationSystem.Services.Web;
    using NbuReservationSystem.Web.App_GlobalResources.Reservations;
    using NbuReservationSystem.Web.Infrastructure.Mapping;
    using NbuReservationSystem.Web.Models.Enums;
    using NbuReservationSystem.Web.Models.Responses.Reservations;

    public class ReservationsController : Controller
    {
        private static readonly Expression<Func<Reservation, ReservationsAdministrationViewModel>> ModelExpression;

        private readonly IReservationsService reservationsService;
        private readonly IRepository<Reservation> reservations;

        static ReservationsController()
        {
            ModelExpression = reservation => new ReservationsAdministrationViewModel
            {
                Title = reservation.Title,
                Date = reservation.Date,
                StartHour = reservation.StartHour,
                EndHour = reservation.EndHour,
                Assignor = reservation.Assignor,
                Email = reservation.Organiser.Email,
                Equipment = reservation.IsEquipementRequired,
                IP = reservation.Organiser.IP,
                PhoneNumber = reservation.Organiser.PhoneNumber,
                Organizer = reservation.Organiser.Name
            };
        }

        public ReservationsController(IReservationsService reservationsService,  IRepository<Reservation> reservations)
        {
            this.reservationsService = reservationsService;
            this.reservations = reservations;
        }

        [HttpGet]
        public ActionResult Index(int? year, int? month)
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
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public ActionResult Administration()
        {
            // TODO: this should NOT live here !!!

            // R.I.P. server-side performance
            var reservations = this.reservations.AllWithDeleted().Select(ModelExpression).ToList();

            // R.I.P. client-side performance
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
        public ActionResult New()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ViewResult New(ReservationViewModel model)
        {
            // TODO: implement me!
            // TODO: validate that the hall is free for the given date
            // TODO: generate reservations based on the repetition policy
            // TODO: send an email
            // TODO: redirect to /calendar with success message -> mail + content

            if (this.ModelState.IsValid)
            {
                var now = DateTime.UtcNow.AddHours(2);
                var repetitionPolicy = model.RepetitionPolicy;

                // make sure the selected date is not in the past
                if (model.Date.Date < now.Date)
                {
                    this.ModelState.AddModelError("Date", ErrorMessages.DateExpired);
                }
                else
                {
                    // we know the event date is today or in the future -> check the hours
                    var beginsOnDate = model.Date
                        .AddHours(model.StartHour.Hours)
                        .AddMinutes(model.StartHour.Hours);

                    if (beginsOnDate <= now)
                    {
                        // starting hour is in the past
                        this.ModelState.AddModelError("StartHour", ErrorMessages.BadHours);
                    }
                    else
                    {
                        // starting hour is in the future ->
                        // check if the event doesn't end before it starts..
                        if (model.StartHour >= model.EndHour)
                        {
                            this.ModelState.AddModelError("EndHour", ErrorMessages.BadHours);
                        }
                    }

                    if (repetitionPolicy.RepetitionType == RepetitionType.EndOnSpecificDate)
                    {
                        if (!repetitionPolicy.EndDate.HasValue)
                        {
                            // TODO: show warning..?
                            return this.View(model);
                        }

                        if (repetitionPolicy.EndDate.Value.Date < model.Date)
                        {
                            this.ModelState.AddModelError(string.Empty, ErrorMessages.EndDateIsBeforeStartDate);
                        }
                    }

                    if (repetitionPolicy.RepetitionDays.Count != 7)
                    {
                        // TODO: show warning..?
                        return this.View(model);
                    }

                    if (repetitionPolicy.RepetitionType != RepetitionType.OneTimeOnly)
                    {
                        if (repetitionPolicy.RepetitionDays.All(x => x == false))
                        {
                            this.ModelState.AddModelError(string.Empty, ErrorMessages.NoSelectedRepetitionDays);
                        }
                    }
                }
            }

            if (this.ModelState.IsValid)
            {
                var ip = this.HttpContext.Request.UserHostAddress;
                var reservations = this.reservationsService.AddReservations(model, ip);

                // TODO: this isn't the best solution..
                this.ViewBag.RequestSucceeded = reservations == -1;
            }

            return this.View(model);
        }
    }
}
