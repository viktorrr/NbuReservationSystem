namespace NbuReservationSystem.Services.Web.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using Xunit;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "*", Justification = "Code readability")]
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "*", Justification = "Code readability")]
    public class CalendarProviderTests
    {
        private readonly ICalendarService service;

        public CalendarProviderTests()
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
    }
}
