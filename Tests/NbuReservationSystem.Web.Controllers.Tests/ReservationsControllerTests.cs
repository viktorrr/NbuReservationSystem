namespace NbuReservationSystem.Web.Controllers.Tests
{
    using System;

    using Moq;

    using NbuReservationSystem.Data.Common;
    using NbuReservationSystem.Data.Models;
    using NbuReservationSystem.Services.Web;
    using NbuReservationSystem.Web.Models.Enums;
    using NbuReservationSystem.Web.Models.Requests.Reservations;

    using Xunit;

    public class ReservationsControllerTests
    {
        private readonly IReservationsService reservationsService;
        private readonly IEmailService emailService;
        private readonly ITokenGenerator tokenGenerator;
        private readonly IRepository<Reservation> reservations;
        private readonly IRepository<Hall> halls;
        private readonly ReservationsController controller;

        public ReservationsControllerTests()
        {
            this.reservationsService = new Mock<IReservationsService>(MockBehavior.Strict).Object;
            this.emailService = new Mock<IEmailService>(MockBehavior.Strict).Object;
            this.tokenGenerator = new Mock<ITokenGenerator>(MockBehavior.Strict).Object;
            this.reservations = new Mock<IRepository<Reservation>>(MockBehavior.Strict).Object;
            this.halls = new Mock<IRepository<Hall>>(MockBehavior.Strict).Object;
            this.controller = new ReservationsController(
                this.reservationsService,
                this.emailService,
                this.tokenGenerator,
                this.halls,
                this.reservations
            );
        }

        [Fact]
        public void RequestWithDateInThePastShouldResultInError()
        {
            // arrange
            var model = new ReservationViewModel
            {
                Date = new DateTime(1000, 1, 1)
            };

            // act
            var view = this.controller.New(model);

            // assert
            AssertExtenstions.ModelErrorIsSet(view, () => model.Date);
        }

        [Fact]
        public void RequestWithStartingHourInThePastShouldResultInError()
        {
            // arrange
            var dateInThePast = DateTime.UtcNow.AddHours(-5);
            var startHour = new TimeSpan(dateInThePast.Hour, dateInThePast.Minute, dateInThePast.Second);
            var model = new ReservationViewModel
            {
                Date = DateTime.UtcNow,
                StartHour = startHour
            };

            // act
            var view = this.controller.New(model);

            // assert
            AssertExtenstions.ModelErrorIsSet(view, () => model.StartHour);
        }

        [Fact]
        public void RequestWithEndHourBeforeStartShouldResultInError()
        {
            // arrange
            var dateInTheFuture = DateTime.UtcNow.Date.AddDays(3);
            var startHour = new TimeSpan(13, 30, 0);
            var endHour = new TimeSpan(5, 5, 5);

            var model = new ReservationViewModel
            {
                Date = dateInTheFuture,
                StartHour = startHour,
                EndHour = endHour
            };

            // act
            var view = this.controller.New(model);

            // assert
            AssertExtenstions.ModelErrorIsSet(view, () => model.EndHour);
        }

        [Fact]
        public void RequestWithEndDateBeforeStartDateShouldResultInError()
        {
            // arrange
            var startDate = DateTime.UtcNow.Date.AddDays(10);
            var endDate = DateTime.UtcNow.Date.AddDays(5);

            var model = new ReservationViewModel
            {
                Date = startDate,
                RepetitionPolicy = { EndDate = endDate, RepetitionType = RepetitionType.EndOnSpecificDate }
            };

            // act
            var view = this.controller.New(model);

            // assert
            AssertExtenstions.ModelErrorIsSet(view);
        }
    }
}
