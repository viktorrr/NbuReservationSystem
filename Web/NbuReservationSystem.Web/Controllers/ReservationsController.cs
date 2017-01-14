namespace NbuReservationSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using Models.Requests.Reservations;

    using NbuReservationSystem.Data.Common;
    using NbuReservationSystem.Data.Models;
    using NbuReservationSystem.Services.Web;
    using NbuReservationSystem.Web.App_GlobalResources.Reservations;
    using NbuReservationSystem.Web.Models.Enums;

    public class ReservationsController : Controller
    {
        private readonly IReservationsService reservationsService;
        private readonly IEmailService emailService;
        private readonly ITokenGenerator tokenGenerator;
        private readonly IRepository<Hall> hallsRepository;

        public ReservationsController(
            IReservationsService reservationsService,
            IEmailService emailService,
            ITokenGenerator tokenGenerator,
            IRepository<Hall> hallsRepository)
        {
            this.reservationsService = reservationsService;
            this.emailService = emailService;
            this.tokenGenerator = tokenGenerator;
            this.hallsRepository = hallsRepository;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var halls = this.GetHalls();
            return this.View(halls);
        }

        [HttpGet]
        public ViewResult ByHall(int? year, int? month, string hallName)
        {
            if (string.IsNullOrEmpty(hallName))
            {
                return this.View("Index");
            }

            var hall = this.hallsRepository.GetBy(x => x.Name == hallName);
            if (hall == null)
            {
                return this.View("Index");
            }

            var isDateValid = year.HasValue && month.HasValue;
            if (month >= 13 || month <= 0)
            {
                isDateValid = false;
            }

            var now = isDateValid ? new DateTime(year.Value, month.Value, 1) : DateTime.UtcNow.AddHours(2);
            var reservations = this.reservationsService.GetReservations(now.Year, now.Month, hall.Id);

            return this.View(reservations);
        }

        [HttpGet]
        public ActionResult Calendar(int year, int month, string hallName)
        {
            if (!this.Request.IsAjaxRequest())
            {
                this.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("Nope.");
            }

            var hall = this.hallsRepository.GetBy(x => x.Name == hallName);

            var now = new DateTime(year, month, 1);
            var reservations = this.reservationsService.GetReservations(now.Year, now.Month, hall.Id);

            return this.PartialView("_Calendar", reservations);
        }

        [HttpGet]
        public ActionResult DayTab(int year, int month, int day, string hallName)
        {
            if (!this.Request.IsAjaxRequest())
            {
                this.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("Nope.");
            }

            var date = new DateTime(year, month, day);
            var hall = this.hallsRepository.GetBy(x => x.Name == hallName);
            var reservations = this.reservationsService.GetReservations(date, hall.Id);
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
                if (string.IsNullOrEmpty(model.HallName))
                {
                    // TODO: display warning?
                    return this.View(model);
                }

                if (!this.CheckHallExistance(model.HallName))
                {
                    // TODO: display warning?
                    return this.View(model);
                }

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

        private bool CheckHallExistance(string hallName)
        {
            return this.hallsRepository.AllBy(x => x.Name == hallName).Any();
        }
    }
}
