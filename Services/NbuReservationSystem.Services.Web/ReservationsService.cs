﻿namespace NbuReservationSystem.Services.Web
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
        private readonly ITokenGenerator stringGenerator;

        // data
        private readonly IRepository<Reservation> reservations;
        private readonly IRepository<Organizer> organizers;
        private readonly IRepository<Hall> hallsRepository;

        public ReservationsService(
            ICalendarService calendarService,
            ITokenGenerator stringGenerator,
            IRepository<Reservation> reservations,
            IRepository<Organizer> organizers,
            IRepository<Hall> hallsRepository)
        {
            this.calendarService = calendarService;
            this.stringGenerator = stringGenerator;

            this.reservations = reservations;
            this.organizers = organizers;
            this.hallsRepository = hallsRepository;
        }

        public int AddReservations(ReservationViewModel model, string ip)
        {
            lock (Locker)
            {
                var reservationDates = this.calendarService.CalculateDates(model).ToList();
                var hall = this.hallsRepository.AllBy(x => x.Name == model.HallName).First();
                var datesAreFree = this.CheckIfAllDatesFree(reservationDates, model.StartHour, model.EndHour, hall.Id);

                if (!datesAreFree)
                {
                    return -1;
                }

                var organiser = this.CreateOrganizer(model, ip);
                var token = this.stringGenerator.Generate();

                foreach (var reservationDate in reservationDates)
                {
                    var reservation = CreateReservation(model, organiser, reservationDate, token, hall);
                    this.reservations.Add(reservation);
                }

                this.reservations.Save();

                return reservationDates.Count;
            }
        }

        public MonthlyReservationsViewModel GetReservations(int year, int month, int hallId)
        {
            var now = new DateTime(year, month, 1).ToUniversalTime().AddHours(2);
            var hall = this.GetHall(hallId);

            var startDay = this.calendarService.GetFirstDayOfMonthView(now.Year, now.Month);
            var endDay = this.calendarService.GetLastDayOfMonthView(now.Year, now.Month);

            var reservationsByDay = this.GetReservations(startDay, endDay, hallId);
            var weeks = CreateWeeklyReservations(reservationsByDay, month, startDay, hall.Name);

            return new MonthlyReservationsViewModel(weeks, year, month, hall.Name, hall.Color);
        }

        public DayViewModel GetReservations(DateTime date, int hallId)
        {
            var selectedReservations = this.reservations.AllBy(x => x.Date == date && x.HallId == hallId).ToList();
            var hallName = this.GetHall(hallId).Name;

            return new DayViewModel(selectedReservations, date, hallName);
        }

        private static Reservation CreateReservation(ReservationViewModel model, Organizer organizer, DateTime date, string token, Hall hall)
        {
            return new Reservation
            {
                Title = model.Title,
                Assignor = model.Assignor,
                Date = date,
                StartHour = model.StartHour,
                EndHour = model.EndHour,
                Description = model.Description,
                Organizer = organizer,
                IsEquipementRequired = model.IsEquipmentRequired,
                Token = token,
                Hall = hall
            };
        }

        private static List<WeekViewModel> CreateWeeklyReservations(
            IReadOnlyDictionary<DateTime, IEnumerable<Reservation>> reservationsByDay,
            int month,
            DateTime currentDay,
            string hall)
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
                        reservations = reservationsByDay[currentDay].OrderBy(x => x.StartHour).ToList();
                    }

                    var dayModel = new DayViewModel
                    {
                        Reservations = reservations,
                        Day = currentDay,
                        Hall = hall
                    };

                    currentDay = currentDay.AddDays(1);
                    days.Add(dayModel);
                }

                var week = new WeekViewModel { Days = days, Month = month, Hall = hall };
                weeks.Add(week);
            }

            return weeks;
        }

        private Organizer CreateOrganizer(ReservationViewModel model, string ip)
        {
            var organizer = new Organizer
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

        private bool CheckIfAllDatesFree(List<DateTime> dates, TimeSpan startHour, TimeSpan endHour, int hallId)
        {
            // not sure if this is tbe best performance-wise solution, but..
            return !this.reservations.All().Any(
                r => r.HallId == hallId && r.Date == dates.FirstOrDefault(
                    d => (d == r.Date) && startHour <= r.EndHour && endHour >= r.StartHour)
            );
        }

        private Dictionary<DateTime, IEnumerable<Reservation>> GetReservations(DateTime from, DateTime to, int hallId)
        {
            return this.reservations.All()
                .Where(x => x.Date >= from && x.Date <= to && x.HallId == hallId)
                .OrderBy(x => x.Date)
                .GroupBy(x => x.Date, (day, currentReservations) => new { day, currentReservations })
                .ToDictionary(x => x.day, x => x.currentReservations);
        }

        private Hall GetHall(int hallId)
        {
            return this.hallsRepository.GetById(hallId);
        }
    }
}
