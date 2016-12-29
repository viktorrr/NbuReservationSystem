namespace NbuReservationSystem.Services.Web.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using NbuReservationSystem.Services.Web.Tests.Builders;
    using NbuReservationSystem.Services.Web.Tests.TestData;
    using NbuReservationSystem.Web.Models.Enums;

    using Xunit;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "*", Justification = "Code readability")]
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "*", Justification = "Code readability")]
    public class CalendarServiceTests
    {
        private readonly ICalendarService service;

        public CalendarServiceTests()
        {
            this.service = new CalendarService();
        }

        public static IEnumerable<object[]> GetFirstDayOfMonthViewTestData => new[]
        {
            // straight forward case - 1st of November is Tuesday
            new object[] { 2016, 11, new DateTime(2016, 10, 31) },

            // corner case - years change
            new object[] { 2016, 01, new DateTime(2015, 12, 28) },

            // corner case - 1st of August is Monday
            new object[] { 2016, 8, new DateTime(2016, 7, 25) },

            // corner case - 1st of May is Sunday
            new object[] { 2016, 5, new DateTime(2016, 4, 25) },
        };

        [Theory]
        [MemberData(nameof(GetFirstDayOfMonthViewTestData))]
        public void TestGetFirstDayOfMonthView(int year, int month, DateTime expected)
        {
            // act
            var result = this.service.GetFirstDayOfMonthView(year, month);

            // assert
            Assert.Equal(expected, result);
            Assert.Equal(DateTimeKind.Utc, result.Kind);
        }

        public static IEnumerable<object[]> GetLastDayOfMonthViewTestData => new[]
        {
            // straight forward case - last day of November is Wednesday
            new object[] { 2016, 11, new DateTime(2016, 12, 4, 23, 59, 59) },

            // corner case - years change
            new object[] { 2016, 12, new DateTime(2017, 1, 1, 23, 59, 59) },

            // corner case - last day of April is Monday
            new object[] { 2017, 4, new DateTime(2017, 4, 30, 23, 59, 59) },

            // corner case - last day of July is Sunday
            new object[] { 2017, 7, new DateTime(2017, 8, 6, 23, 59, 59) },
        };

        [Theory]
        [MemberData(nameof(GetLastDayOfMonthViewTestData))]
        public void TestGetLastDayOfPreviousMonth(int year, int month, DateTime expected)
        {
            // act
            var result = this.service.GetLastDayOfMonthView(year, month);

            // assert
            Assert.Equal(expected, result);
            Assert.Equal(DateTimeKind.Utc, result.Kind);
        }

        internal static CalculateDatesTestDataBuilder Builder => new CalculateDatesTestDataBuilder();

        public static IEnumerable<object[]> CalculateDatesTestData => new[]
        {
            new object[]
            {
                Builder
                    .StartDate(2016, 12, 5)
                    .Repetitions(2)
                    .RepetitionDays(Day.Wednesday, Day.Thursday, Day.Saturday)
                    .ExpectedDates(
                        new DateTime(2016, 12, 5),
                        new DateTime(2016, 12, 7),
                        new DateTime(2016, 12, 8),
                        new DateTime(2016, 12, 10),
                        new DateTime(2016, 12, 14),
                        new DateTime(2016, 12, 15),
                        new DateTime(2016, 12, 17)
                    )
                    .Build()
            },
            new object[]
            {
                Builder
                    .StartDate(2017, 1, 6)
                    .Repetitions(6)
                    .Window(3)
                    .RepetitionDays(Day.Friday)
                    .ExpectedDates(
                        new DateTime(2017, 1, 6),
                        new DateTime(2017, 2, 3),
                        new DateTime(2017, 3, 3),
                        new DateTime(2017, 3, 31),
                        new DateTime(2017, 4, 28),
                        new DateTime(2017, 5, 26)
                    )
                    .Build()
            },
            new object[]
            {
                Builder
                    .StartDate(2016, 12, 3)
                    .Repetitions(1)
                    .Window(2)
                    .RepetitionDays(Day.Sunday)
                    .ExpectedDates(
                        new DateTime(2016, 12, 3),
                        new DateTime(2016, 12, 4)
                    )
                    .Build()
            },
            new object[]
            {
                Builder
                    .StartDate(2017, 1, 6)
                    .Repetitions(4)
                    .RepetitionDays(Day.Friday)
                    .ExpectedDates(
                        new DateTime(2017, 1, 6),
                        new DateTime(2017, 1, 13),
                        new DateTime(2017, 1, 20),
                        new DateTime(2017, 1, 27)
                    )
                    .Build()
            },
            new object[]
            {
                Builder
                    .StartDate(2017, 1, 10)
                    .Repetitions(3)
                    .Window(3)
                    .RepetitionDays(Day.Tuesday)
                    .ExpectedDates(
                        new DateTime(2017, 1, 10),
                        new DateTime(2017, 2, 7),
                        new DateTime(2017, 3, 7)
                    )
                    .Build()
            },
        };

        [Theory]
        [MemberData(nameof(CalculateDatesTestData))]
        internal void TestCalculateDatesForSpecificAmountOfRepetitions(CalculateDatesTestData data)
        {
            // act
            var result = this.service.CalculateDates(data.Model).ToList();

            // assert
            Assert.Equal(result.ToArray(), data.ExpectedDates);
        }
    }
}
