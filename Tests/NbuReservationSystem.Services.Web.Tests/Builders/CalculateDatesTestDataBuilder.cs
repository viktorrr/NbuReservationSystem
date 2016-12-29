namespace NbuReservationSystem.Services.Web.Tests.Builders
{
    using System;
    using System.Collections.Generic;

    using NbuReservationSystem.Services.Web.Tests.TestData;
    using NbuReservationSystem.Web.Models.Enums;
    using NbuReservationSystem.Web.Models.Requests.Reservations;

    public class CalculateDatesTestDataBuilder
    {
        private DateTime startDate;
        private TimeSpan beginsOn;
        private TimeSpan endsOn;
        private IList<bool> repetitionDates;

        // repetition policy
        private DateTime endDate;
        private CancellationType cancellationType;
        private int window;
        private int repetitionsCount;

        // expectations
        private List<DateTime> expectedDates = new List<DateTime>();

        public CalculateDatesTestDataBuilder StartDate(int year, int month, int day)
        {
            this.startDate = new DateTime(year, month, day);
            return this;
        }

        public CalculateDatesTestDataBuilder EndDate(int year, int month, int day)
        {
            this.endDate = new DateTime(year, month, day);
            return this;
        }

        public CalculateDatesTestDataBuilder BeginsOn(TimeSpan time)
        {
            this.beginsOn = time;
            return this;
        }

        public CalculateDatesTestDataBuilder EndsOn(TimeSpan time)
        {
            this.endsOn = time;
            return this;
        }

        public CalculateDatesTestDataBuilder SetCancellationType(CancellationType type)
        {
            this.cancellationType = type;
            return this;
        }

        public CalculateDatesTestDataBuilder Window(int value)
        {
            this.window = value;
            return this;
        }

        public CalculateDatesTestDataBuilder Repetitions(int value)
        {
            this.repetitionsCount = value;
            this.cancellationType = CancellationType.ExactRepetitionCount;
            return this;
        }

        public CalculateDatesTestDataBuilder RepetitionDays(params Day[] days)
        {
            this.repetitionDates = CreateDaysToRepeat(days);
            return this;
        }

        public CalculateDatesTestDataBuilder ExpectedDate(DateTime date)
        {
            this.expectedDates.Add(date);
            return this;
        }

        public CalculateDatesTestDataBuilder ExpectedDate(int year, int month, int day)
        {
            this.expectedDates.Add(new DateTime(year, month, day));
            return this;
        }

        public CalculateDatesTestDataBuilder ExpectedDates(params DateTime[] dates)
        {
            this.expectedDates.AddRange(dates);
            return this;
        }

        public CalculateDatesTestData Build()
        {
            var model = new ReservationViewModel
            {
                Date = this.startDate,
                StartHour = this.beginsOn,
                EndHour = this.endsOn,
                RepetitionPolicy = new RepetitionPolicy
                {
                    CancellationType = this.cancellationType,
                    EndDate = this.endDate,
                    RepetitionWindow = this.window,
                    Repetitions = this.repetitionsCount,
                    RepetitionDays = this.repetitionDates
                }
            };

            return new CalculateDatesTestData
            {
                Model = model,
                ExpectedDates = this.expectedDates.ToArray()
            };
        }

        private static IList<bool> CreateDaysToRepeat(params Day[] days)
        {
            var result = new List<bool>();
            for (int i = 0; i < 7; i++)
            {
                result.Add(false);
            }

            foreach (var day in days)
            {
                var dayIndex = (int)day;
                result[dayIndex] = true;
            }

            return result;
        }
    }
}
