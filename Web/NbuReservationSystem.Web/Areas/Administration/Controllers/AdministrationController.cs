namespace NbuReservationSystem.Web.Areas.Administration.Controllers
{
    using System.Web.Mvc;

    using NbuReservationSystem.Common;
    using NbuReservationSystem.Web.Controllers;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public class AdministrationController : BaseController
    {
    }
}
