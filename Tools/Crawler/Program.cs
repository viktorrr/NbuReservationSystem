namespace Crawler
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    using AngleSharp.Parser.Html;

    using NbuReservationSystem.Data;
    using NbuReservationSystem.Data.Models;

    public static class Program
    {
        public static void Main()
        {
            GoogleCalendarCrawler();
        }

        private static void GoogleCalendarCrawler()
        {
            // TODO: this needs to be an argument
            var html = File.ReadAllText(@"..\..\collective-hall.html");
            var parser = new HtmlParser();
            var document = parser.Parse(html);

            var days = document.QuerySelectorAll(".day");
            var dateTimeRegex = new Regex(@", \d{4}");
            var lastKnownYear = -1;

            var db = new ApplicationDbContext();

            foreach (var day in days)
            {
                var children = day.Children.ToList();

                if (children.Count > 0)
                {

                    var dateString = children.First().TextContent;
                    var dateTimeMatch = dateTimeRegex.Match(dateString);
                    var date = DateTime.UtcNow;

                    if (dateTimeMatch.Success)
                    {
                        date = DateTime.Parse(dateString);
                        lastKnownYear = date.Year;
                    }
                    else
                    {
                        date = DateTime.Parse(dateString + ", " + lastKnownYear);
                    }

                    for (int i = 1; i < children.Count; i++)
                    {
                        var eventTitle = children[i].QuerySelector(".event-title").TextContent;
                        if (string.IsNullOrEmpty(eventTitle))
                        {
                            continue;
                        }

                        var equipment = false;
                        if (eventTitle.Contains("+ техника"))
                        {
                            equipment = true;
                            eventTitle = eventTitle.Substring(0, eventTitle.IndexOf("+ техника"));
                        }

                        var duration = children[i].QuerySelector(".event-time").Attributes[1].Value
                            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Last()
                            .Split(new[] { '–' }, StringSplitOptions.RemoveEmptyEntries);

                        var beginsOn = ParseDateTime(duration[0]);
                        var endsOn = ParseDateTime(duration[1]);

                        var reservation = new Reservation
                        {
                            StartHour = beginsOn,
                            EndHour = endsOn,
                            Date = date,
                            Title = eventTitle,
                            IsEquipementRequired = equipment,
                            Assignor = "__system",
                            Description = "<none>",
                            Organizer = new Organizer
                            {
                                Name = "__system",
                                PhoneNumber = "1234567890",
                                Email = "nbureservationsystem@gmail.com",
                                IP = "::1"
                            }
                        };
                        db.Reservations.Add(reservation);
                        db.SaveChanges();
                    }
                }
            }
        }

        private static TimeSpan ParseDateTime(string text)
        {
            if (text.Contains("am"))
            {
                var index = text.IndexOf("am", StringComparison.Ordinal);
                text = text.Substring(0, index);

                int hours;
                if (!int.TryParse(text, out hours))
                {
                    var split = text.Split(':');
                    return new TimeSpan(0, int.Parse(split[0]), int.Parse(split[1]), 0);
                }

                return new TimeSpan(0, hours, 0, 0);
            }

            if (text.Contains("pm"))
            {
                var index = text.IndexOf("pm", StringComparison.Ordinal);
                text = text.Substring(0, index);

                int hours;
                if (!int.TryParse(text, out hours))
                {
                    var split = text.Split(':');
                    hours = int.Parse(split[0]);
                    if (hours != 12)
                    {
                        hours += 12;
                    }

                    return new TimeSpan(0, hours, int.Parse(split[1]), 0);
                }

                return new TimeSpan(0, hours, 0, 0);
            }

            var temp = TimeSpan.Parse(text);
            return new TimeSpan(0, temp.Hours, temp.Minutes, 0);
        }
    }
}
