namespace NbuReservationSystem.Web.Controllers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;

    using AngleSharp.Dom.Html;
    using AngleSharp.Parser.Html;

    using NbuReservationSystem.Common;
    using NbuReservationSystem.Data;
    using NbuReservationSystem.Data.Common;
    using NbuReservationSystem.Data.Models;
    using NbuReservationSystem.Web.Models.Requests.Halls;
    using NbuReservationSystem.Web.Models.Responses.Halls;
    using NbuReservationSystem.Web.Models.Responses.Reservations;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public class AdminController : Controller
    {
        private static readonly Expression<Func<Reservation, ReservationsAdministrationViewModel>> ModelExpression;

        private readonly IRepository<Reservation> reservationsRepository;
        private readonly IRepository<Hall> hallsRepository;

        // TODO: this shouldn't be here!
        static AdminController()
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
                Organizer = reservation.Organizer.Name,
                Id = reservation.Id,
                Deleted = reservation.IsDeleted,
                Token = reservation.Token,
                Hall = reservation.Hall.Name
            };
        }

        public AdminController(IRepository<Reservation> reservationsRepository, IRepository<Hall> hallsRepository)
        {
            this.reservationsRepository = reservationsRepository;
            this.hallsRepository = hallsRepository;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return this.Redirect("/");
        }

        [HttpGet]
        public ActionResult Reservations()
        {
            // R.I.P. server-side performance
            var reservations = this.reservationsRepository
                .AllWithDeleted()
                .Select(ModelExpression)
                .ToList();

            // R.I.P. client-side performance
            return this.View(reservations);
        }

        [HttpGet]
        public ViewResult NewHall()
        {
            return this.View();
        }

        [HttpPost]
        public ViewResult NewHall(HallViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View("NewHall", model);
            }

            var hall = new Hall { Color = model.Color, Name = model.Name };

            this.hallsRepository.Add(hall);
            this.hallsRepository.Save();

            var ip = this.HttpContext.Request.UserHostAddress;
            var response = this.ParseGoogleEntries(model, hall, ip);

            return this.View("NewHallSuccess", response);
        }

        private static IHtmlDocument ParseHtmlDocument(string html)
        {
            try
            {
                var parser = new HtmlParser();
                return parser.Parse(html);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static TimeSpan ParseDateTime(string text)
        {
            if (text.Contains("am"))
            {
                var index = text.IndexOf("am", StringComparison.Ordinal);
                text = text.Substring(0, index);

                int hours;
                if (!int.TryParse(text, out hours))
                {
                    var split = text.Split(':');
                    return new TimeSpan(0, int.Parse(split[0]), int.Parse(split[1]), 0);
                }

                return new TimeSpan(0, hours, 0, 0);
            }

            if (text.Contains("pm"))
            {
                var index = text.IndexOf("pm", StringComparison.Ordinal);
                text = text.Substring(0, index);

                int hours;
                if (!int.TryParse(text, out hours))
                {
                    var split = text.Split(':');
                    hours = int.Parse(split[0]);
                    if (hours != 12)
                    {
                        hours += 12;
                    }

                    return new TimeSpan(0, hours, int.Parse(split[1]), 0);
                }

                return new TimeSpan(0, hours, 0, 0);
            }

            var temp = TimeSpan.Parse(text);
            return new TimeSpan(0, temp.Hours, temp.Minutes, 0);
        }

        private NewHallSuccess ParseGoogleEntries(HallViewModel model, Hall hall, string ip)
        {
            // TODO: this should be a service method..
            var html = model.GoogleCalendarContent;
            var document = ParseHtmlDocument(html);

            var result = new NewHallSuccess();

            if (document == null)
            {
                return result;
            }

            var days = document.QuerySelectorAll(".day");
            var dateTimeRegex = new Regex(@", \d{4}");
            var lastKnownYear = -1;

            foreach (var day in days)
            {
                var children = day.Children.ToList();

                if (children.Count <= 0)
                {
                    continue;
                }

                result.Total++;

                try
                {
                    var dateString = children.First().TextContent;
                    var dateTimeMatch = dateTimeRegex.Match(dateString);
                    DateTime date;

                    if (dateTimeMatch.Success)
                    {
                        date = DateTime.Parse(dateString);
                        lastKnownYear = date.Year;
                    }
                    else
                    {
                        date = DateTime.Parse(dateString + ", " + lastKnownYear);
                    }

                    var organizer = new Organizer
                    {
                        Name = "__system",
                        PhoneNumber = "1234567890",
                        Email = "nbureservationsystem@gmail.com",
                        IP = ip
                    };

                    for (int i = 1; i < children.Count; i++)
                    {
                        var eventTitle = children[i].QuerySelector(".event-title").TextContent;
                        if (string.IsNullOrEmpty(eventTitle))
                        {
                            continue;
                        }

                        var equipment = false;
                        if (eventTitle.Contains("+ техника"))
                        {
                            equipment = true;
                            eventTitle = eventTitle.Substring(0, eventTitle.IndexOf("+ техника"));
                        }

                        var duration =
                            children[i].QuerySelector(".event-time").Attributes[1].Value.Split(
                                    new[] { ',' },
                                    StringSplitOptions.RemoveEmptyEntries)
                                .Last()
                                .Split(new[] { '–' }, StringSplitOptions.RemoveEmptyEntries);

                        var beginsOn = ParseDateTime(duration[0]);
                        var endsOn = ParseDateTime(duration[1]);

                        var reservation = new Reservation
                        {
                            StartHour = beginsOn,
                            EndHour = endsOn,
                            Date = date.Date,
                            Title = eventTitle,
                            IsEquipementRequired = equipment,
                            Assignor = "__system",
                            Description = "<none>",
                            Organizer = organizer,
                            Hall = hall
                        };

                        this.reservationsRepository.Add(reservation);
                        this.reservationsRepository.Save();

                        result.Added++;
                    }
                }
                catch (Exception)
                {
                    // nothing to do
                }
            }

            return result;
        }
    }
}