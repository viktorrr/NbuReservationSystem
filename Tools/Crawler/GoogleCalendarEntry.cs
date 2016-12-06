namespace Crawler
{
    using System;
    using System.Collections.Generic;

    public class GoogleCalendarEntry
    {
        public DateTime Date { get; set; }

        public List<string> Events { get; set; }
    }
}
