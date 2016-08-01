using SQLite.Net;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BowlingAverageTracker.Dto
{
    public class Series : BaseDto
    {
        public static string deleteSeriesQuery = "delete from Series where Id = ?";
        public static string deleteByLeagueQuery = "delete from Series where LeagueId = ?";
        public static string deleteByBowlerQuery = "delete from Series where LeagueId in " +
                                                   "(select Id from League where BowlerId = ?)";
        private static string averageQuery = "select round(avg(Score), 2) as Value from Game where SeriesId = ?";
        private static string sumQuery = "select sum(Score) as Value from Game where SeriesId = ?";

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTimeOffset Date { get; set; }

        [ForeignKey(typeof(League))]
        public int LeagueId { get; set; }

        [ManyToOne]
        public League League { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Game> Games { get; }

        public Series()
        {
            Games = new List<Game>();
        }

        public float Average
        {
            get
            {
                using (SQLiteConnection conn = getDBConnection())
                {
                    return conn.Query<FloatWrapper>(averageQuery, Id).First().Value;
                }
            }
        }

        public int TotalPins
        {
            get
            {
                using (SQLiteConnection conn = getDBConnection())
                {
                    return conn.Query<IntWrapper>(sumQuery, Id).First().Value;
                }
            }
        }

        public DateTimeOffset LocalDate
        {
            get { return Date.ToLocalTime(); }
        }

        public string DateString
        {
            get { return LocalDate.ToString("D"); }
        }
    }
}
