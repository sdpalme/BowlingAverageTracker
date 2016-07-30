using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLiteNetExtensions.Attributes;
using SQLite.Net.Attributes;

namespace BowlingAverageTracker.Dto
{
    public class Game : BaseDto
    {
        public static string deleteGameQuery = "delete from Game where Id = ?";
        public static string deleteBySeriesQuery = "delete from Game where SeriesId = ?";
        public static string deleteByLeagueQuery = "delete from Game where SeriesId in " +
                                                   "(select Id from Series where LeagueId = ?)";
        public static string deleteByBowlerQuery = "delete from Game where SeriesId in " +
                                                   "(select Id from Series where LeagueId in " +
                                                   "(select Id from League where BowlerId = ?))";
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int Score { get; set; }

        [ForeignKey(typeof(Series))]
        public int SeriesId { get; set; }

        [ManyToOne]
        public Series Series { get; set; }
    }
}
