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

    }
}