namespace NbuReservationSystem.Web.Controllers
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;

    using NbuReservationSystem.Common;
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
            // TODO: process google entries

            this.hallsRepository.Add(hall);
            this.hallsRepository.Save();

            var response = new NewHallSuccess { };

            return this.View("NewHallSuccess", response);
        }
    }
}