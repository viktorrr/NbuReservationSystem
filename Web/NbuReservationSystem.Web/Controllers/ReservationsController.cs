namespace NbuReservationSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
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
    using NbuReservationSystem.Web.Models.Enums;
    using NbuReservationSystem.Web.Models.Responses.Reservations;

    public class ReservationsController : Controller
    {
        private static readonly Expression<Func<Reservation, ReservationsAdministrationViewModel>> ModelExpression;

        private readonly IReservationsService reservationsService;
        private readonly IEmailService emailService;
        private readonly ITokenGenerator tokenGenerator;
        private readonly IRepository<Reservation> reservationsRepository;
        private readonly IRepository<Hall> hallsRepository;

        // TODO: this shouldn't be here!
        static ReservationsController()
        {
            ModelExpression = reservation => new ReservationsAdministrationViewModel
            {
                Title = reservation.Title,
                Date = reservation.Date,
                StartHour = reservation.StartHour,
                EndHour = reservation.EndHour,
                Assignor = reservation.Assignor,
                Email = reservation.Organizer.Email,
                Equipment = reservation.IsEquipementRequired,
                IP = reservation.Organizer.IP,
                PhoneNumber = reservation.Organizer.PhoneNumber,
                Organizer = reservation.Organizer.Name
            };
        }

        public ReservationsController(
            IReservationsService reservationsService,
            IEmailService emailService,
            ITokenGenerator tokenGenerator,
            IRepository<Reservation> reservationsRepository,
            IRepository<Hall> hallsRepository)
        {
            this.reservationsService = reservationsService;
            this.emailService = emailService;
            this.tokenGenerator = tokenGenerator;
            this.reservationsRepository = reservationsRepository;
            this.hallsRepository = hallsRepository;
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
            var reservations = this.reservationsRepository.AllWithDeleted().Select(ModelExpression).ToList();

            // R.I.P. client-side performance
            return this.View(reservations);
        }

        [HttpGet]
        public ActionResult Calendar(int year, int month)
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
            var model = new ReservationViewModel { HallNames = this.GetHalls() };
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ViewResult New(ReservationViewModel model)
        {
            model.HallNames = this.GetHalls();

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
            else
            {
                return this.View(model);
            }

            if (this.ModelState.IsValid)
            {
                var ip = this.HttpContext.Request.UserHostAddress;
                var reservations = this.reservationsService.AddReservations(model, ip);

                // TODO: this isn't the best solution..
                this.ViewBag.RequestSucceeded = reservations == -1;
                this.emailService.SendNewReservationEmail(model, this.tokenGenerator.Generate());
            }

            return this.View(model);
        }

        private IList<string> GetHalls()
        {
            return this.hallsRepository.All().Select(x => x.Name).ToList();
        }
    }
}
