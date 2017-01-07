namespace NbuReservationSystem.Services.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NbuReservationSystem.Data.Common;
    using NbuReservationSystem.Data.Models;
    using NbuReservationSystem.Web.Models.Requests.Reservations;
    using NbuReservationSystem.Web.Models.Responses.Reservations;

    public class ReservationsService : IReservationsService
    {
        // synchronization locker to make sure that
        // adding reservations is a blocking operation
        // this is a bit of performance killer, but it'll
        // guarantee data consistency
        private static readonly object Locker = new object();

        // services
        private readonly ICalendarService calendarService;
        private readonly IRandomStringGenerator stringGenerator;

        // data
        private readonly IRepository<Reservation> reservations;
        private readonly IRepository<Organiser> organizers;

        public ReservationsService(
            ICalendarService calendarService,
            IRandomStringGenerator stringGenerator,
            IRepository<Reservation> reservations,
            IRepository<Organiser> organizers)
        {
            this.calendarService = calendarService;
            this.stringGenerator = stringGenerator;

            this.reservations = reservations;
            this.organizers = organizers;
        }

        public int AddReservations(ReservationViewModel model, string ip)
        {
            lock (Locker)
            {
                var reservationDates = this.calendarService.CalculateDates(model).ToList();
                var datesAreFree = this.CheckIfAllDatesFree(reservationDates, model.StartHour, model.EndHour);

                if (!datesAreFree)
                {
                    return -1;
                }

                var organiser = this.CreateOrganizer(model, ip);
                var token = this.stringGenerator.Generate();

                foreach (var reservationDate in reservationDates)
                {
                    var reservation = CreateReservation(model, organiser, reservationDate, token);
                    this.reservations.Add(reservation);
                }

                this.reservations.Save();

                return reservationDates.Count;
            }
        }

        public MonthlyReservationsViewModel GetReservations(int year, int month)
        {
            var now = new DateTime(year, month, 1).ToUniversalTime().AddHours(2);

            var startDay = this.calendarService.GetFirstDayOfMonthView(now.Year, now.Month);
            var endDay = this.calendarService.GetLastDayOfMonthView(now.Year, now.Month);

            var reservationsByDay = this.GetReservations(startDay, endDay);
            var weeks = CreateWeeklyReservations(reservationsByDay, month, startDay);

            return new MonthlyReservationsViewModel(weeks, year, month);
        }

        public DayViewModel GetReservations(DateTime date)
        {
            var selectedReservations = this.reservations.AllBy(x => x.Date == date).ToList();
            return new DayViewModel { Day = date, Reservations = selectedReservations };
        }

        private static Reservation CreateReservation(ReservationViewModel model, Organiser organiser, DateTime date, string token)
        {
            return new Reservation
            {
                Title = model.Title,
                Assignor = model.Assignor,
                Date = date,
                StartHour = model.StartHour,
                EndHour = model.EndHour,
                Description = model.Description,
                Organiser = organiser,
                IsEquipementRequired = model.IsEquipmentRequired,
                Token = token
            };
        }

        private static List<WeekViewModel> CreateWeeklyReservations(
            IReadOnlyDictionary<DateTime, IEnumerable<Reservation>> reservationsByDay, int month, DateTime currentDay)
        {
            var weeks = new List<WeekViewModel>(5);
            for (int i = 0; i < 5; i++)
            {
                var days = new List<DayViewModel>();

                for (int j = 0; j < 7; j++)
                {
                    var reservations = new List<Reservation>();
                    if (reservationsByDay.ContainsKey(currentDay))
                    {
                        reservations = reservationsByDay[currentDay].ToList();
                    }

                    var dayModel = new DayViewModel
                    {
                        Reservations = reservations,
                        Day = currentDay
                    };

                    currentDay = currentDay.AddDays(1);
                    days.Add(dayModel);
                }

                var week = new WeekViewModel { Days = days, Month = month };
                weeks.Add(week);
            }

            return weeks;
        }

        private Organiser CreateOrganizer(ReservationViewModel model, string ip)
        {
            var organizer = new Organiser
            {
                IP = ip,
                PhoneNumber = model.Organizer.PhoneNumber,
                Email = model.Organizer.Email,
                Name = model.Organizer.Name
            };

            this.organizers.Add(organizer);
            this.organizers.Save();

            return organizer;
        }

        private bool CheckIfAllDatesFree(List<DateTime> dates, TimeSpan startHour, TimeSpan endHour)
        {
            // not sure if this is tbe best performance-wise solution, but..
            return !this.reservations.All().Any(
                r => r.Date == dates.FirstOrDefault(
                    d => (d == r.Date) && startHour <= r.EndHour && endHour >= r.StartHour)
            );
        }

        private Dictionary<DateTime, IEnumerable<Reservation>> GetReservations(DateTime from, DateTime to)
        {
            return this.reservations.All()
                .Where(x => x.Date >= from && x.Date <= to)
                .OrderBy(x => x.Date)
                .GroupBy(x => x.Date, (day, currentReservations) => new { day, currentReservations })
                .ToDictionary(x => x.day, x => x.currentReservations);
        }
    }
}
