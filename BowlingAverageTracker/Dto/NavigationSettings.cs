using SQLite.Net.Attributes;
using System;

namespace BowlingAverageTracker.Dto
{
    public class NavigationSettings : BaseDto
    {
        [PrimaryKey]
        public int Id { get; set; }

        public bool SkipLeaguePage { get; set; }
        public bool OneSeriesPerDay { get; set; }

        public NavigationSettings()
        {
        }
    }
}
