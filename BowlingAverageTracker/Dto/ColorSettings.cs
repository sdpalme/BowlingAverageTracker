using SQLite.Net.Attributes;
using System;

namespace BowlingAverageTracker.Dto
{
    public class ColorSettings : BaseDto
    {
        [PrimaryKey]
        public int Id { get; set; }

        public UInt32 BackgroundColor { get; set; }
        public UInt32 TextColor { get; set; }

        public ColorSettings()
        {
        }
    }
}
