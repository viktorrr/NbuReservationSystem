namespace NbuReservationSystem.Web.Controllers
{
    using System.Web.Mvc;

    using NbuReservationSystem.Data.Common;
    using NbuReservationSystem.Data.Models;
    using NbuReservationSystem.Web.Models.Requests.Halls;
    using NbuReservationSystem.Web.Models.Responses.Halls;

    // TODO: admin-only access!
    public class HallsController : BaseController
    {
        private readonly IRepository<Hall> hallsRepository;
        private readonly IRepository<Reservation> reservationsRepository;

        public HallsController(IRepository<Hall> hallsRepository)
        {
            this.hallsRepository = hallsRepository;
        }

        [HttpGet]
        public ViewResult New()
        {
            return this.View();
        }

        [HttpPost]
        public ViewResult NewHall(HallViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View("New", model);
            }

            var hall = new Hall { Color = model.Color, Name = model.Name };
            // TODO: process google shit

            this.hallsRepository.Add(hall);
            this.hallsRepository.Save();

            var response = new NewHallSuccess { };

            return this.View("NewHallSuccess", response);
        }
    }
}